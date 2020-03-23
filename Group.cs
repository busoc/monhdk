using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using System.Linq;

namespace monhdk4
{
    public enum Mode { Realtime, Playback}

    public enum Instance { TEST=0, SIM1=1, SIM2=2, OPS=255}

    public enum Channel { VIC1=1, VIC2=2, LRSD=3}

    public class Group: INotifyPropertyChanged
    {

        public static void Load()
        {
            XmlRootAttribute root = new XmlRootAttribute()
            {
                ElementName = "Group",
            };
            foreach(string file in ConfigFiles())
            {
                using (TextReader reader = new StreamReader(file))
                {
                    XmlSerializer z = new XmlSerializer(typeof(Group), root);
                    Group g = (Group)z.Deserialize(reader);
                }
            }
        }

        public static void Save(Group grp)
        {
            XmlRootAttribute root = new XmlRootAttribute()
            {
                ElementName = "Group",
            };
            string file = Path.Combine(ConfigDir(), $"{grp.Name}.xml");
            using (TextWriter writer = new StreamWriter(file))
            {
                XmlSerializer z = new XmlSerializer(typeof(Group), root);
                z.Serialize(writer, grp);
            }
        }

        private static string[] ConfigFiles()
        {
            return Directory.GetFiles(ConfigDir(), "*.xml", SearchOption.TopDirectoryOnly);
        }

        private static string ConfigDir()
        {
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            dir = Path.Combine(dir, "monhdk");
            Directory.CreateDirectory(dir);

            return dir;
        }

        public static Group Default()
        {
            var g = new Group()
            {
                Addr = IPAddress.Parse("127.0.0.1"),
                Url = new Uri("http://localhost"),
                Port = 1111,
                Name = "connection",
                Limit = 300,
            };
            return g;
        }

        private IPAddress addr;
        private int port;
        private Uri url;
        private string name;
        private int limit;
        private bool connected;
        private ulong count;
        private ulong bad;
        private ulong size;

        private UdpClient client;
        private Task task;
        private ObservableCollection<Entry> entries = new ObservableCollection<Entry>();

        private Dictionary<string, Coze> sources = new Dictionary<string, Coze>();
        private Dictionary<uint, Coze> instances = new Dictionary<uint, Coze>();
        private Dictionary<byte, Coze> channels = new Dictionary<byte, Coze>();
        private Dictionary<string, Coze> upis = new Dictionary<string, Coze>();

        [XmlIgnore]
        public ObservableCollection<Coze> Sources => new ObservableCollection<Coze>(sources.Values);
        [XmlIgnore]
        public ObservableCollection<Coze> Upis => new ObservableCollection<Coze>(upis.Values);
        [XmlIgnore]
        public ObservableCollection<Coze> Instances => new ObservableCollection<Coze>(instances.Values);
        [XmlIgnore]
        public ObservableCollection<Coze> Channels => new ObservableCollection<Coze>(channels.Values);

        [XmlIgnore]
        public IPAddress Addr { 
            get => addr;
            set {
                addr = value;
                Notify("Addr");
            } 
        }
        public int Port { 
            get => port;
            set
            {
                port = value;
                Notify("Port");
            }
        }
        [XmlIgnore]
        public Uri Url { 
            get => url;
            set
            {
                url = value;
                Notify("Url");
            }
        }
        public string Name { 
            get => name;
            set
            {
                name = value;
                Notify("Name");
            }
        }

        [XmlElement("Address")]
        public string XmlAddr
        {
            get => Addr.ToString();
            set => Addr = IPAddress.Parse(value);
        }
        [XmlElement("Url")]
        public string XmlUrl
        {
            get => Url.ToString();
            set => Url = new Uri(value);
        }
        public int Limit { 
            get => limit; 
            set
            {
                limit = value;
                Notify("Limit");
            } 
        }

        [XmlIgnore]
        public ObservableCollection<Entry> Entries { 
            get => entries; 
            set => entries = value; 
        }

        [XmlIgnore]
        public bool IsConnected
        {
            get => connected;
            set
            {
                connected = value;
                Notify("IsConnected");
            }
        }

        [XmlIgnore]
        public ulong Count { 
            get => count; 
            set
            {
                count += value;
                Notify("Count");
            }
        }

        [XmlIgnore]
        public ulong Bad { 
            get => bad;
            set
            {
                bad += value;
                Notify("Bad");
            }
        }

        [XmlIgnore]
        public ulong Size { 
            get => size; 
            set
            {
                size += value;
                Notify("Size");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Notify(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public void Start()
        {
            client = new UdpClient(Port, AddressFamily.InterNetwork);
            if (Addr.IsMulticast())
            {
                client.JoinMulticastGroup(Addr);
            }

            task = new Task(ReadPackets);
            task.Start();
            IsConnected = true;
        }

        public void Stop()
        {
            client?.Dispose();
            task?.Wait();

            IsConnected = false;
        }

        private void Update(Entry e)
        {
            Application.Current.Dispatcher.Invoke(() => {
                if (Entries.Count >= Limit)
                {
                    Entries.RemoveAt(0);
                }
                Entries.Add(e);
                Count = e.Count;
                Size = e.Size;
                Bad = e.Bad;

                updateChannelsStat(e);
                updateUpisStat(e);
                updateSourcesStat(e);
                updateInstancesStat(e);
            });
        }

        private void updateChannelsStat(Entry e)
        {
            try
            {
                channels[e.Channel].Update(e);
            }
            catch (Exception _)
            {
                var z = new Coze(e.Channel.ToString())
                {
                    Count = e.Count,
                    Size = e.Size,
                    Bad = e.Bad,
                };
                channels[e.Channel] = z;
            }
        }

        private void updateSourcesStat(Entry e)
        {
            try
            {
                sources[e.Origin].Update(e);
            }
            catch (Exception _)
            {
                var z = new Coze(e.Origin)
                {
                    Count = e.Count,
                    Size = e.Size,
                    Bad = e.Bad,
                };
                sources[e.Origin] = z;
            }
        }

        private void updateUpisStat(Entry e)
        {
            try
            {
                upis[e.UPI].Update(e);
            }
            catch (Exception _)
            {
                var z = new Coze(e.UPI)
                {
                    Count = e.Count,
                    Size = e.Size,
                    Bad = e.Bad,
                };
                upis[e.UPI] = z;
            }
        }

        private void updateInstancesStat(Entry e) 
        {
            try
            {
                instances[e.Instance].Update(e);
            }
            catch(Exception _)
            {
                var z = new Coze(e.Instance.ToString())
                {
                    Count = e.Count,
                    Size = e.Size,
                    Bad = e.Bad,
                };
                instances[e.Instance] = z;
            }
        }

        private void ReadPackets() {
            IPEndPoint point = new IPEndPoint(Addr, Port);
            while (true)
            {
                byte[] bytes;
                try
                {
                    bytes = client.Receive(ref point);
                }
                catch (Exception _)
                {
                    return;
                }
                if (bytes == null)
                {
                    return;
                }
                Entry e = ReadEntry(bytes);
                Update(e);
            }
        }

        private Entry ReadEntry(byte[] bytes)
        {
            using (BinaryReader rs = new BinaryReader(new MemoryStream(bytes)))
            {
                return new Entry()
                {
                    Origin = BigEndian.GetString(rs),
                    Sequence = BigEndian.GetUint32(rs),
                    Instance = BigEndian.GetUint32(rs),
                    Channel = rs.ReadByte(),
                    Realtime = rs.ReadBoolean(),
                    Count = BigEndian.GetUint32(rs),
                    //Size = BigEndian.GetUint32(rs),
                    //Bad = BigEndian.GetUint32(rs),
                    Elapsed = BigEndian.GetInt64(rs),
                    GenTime = BigEndian.GetDateTime(rs),
                    AcqTime = BigEndian.GetDateTime(rs),
                    Reference = BigEndian.GetString(rs),
                    UPI = BigEndian.GetString(rs),
                };
            }
        }
    }

    public class Coze
    {
        private ulong count;
        private ulong size;
        private ulong bad;
        private string name;

        public ulong Count { get => count; set => count += value; }
        public ulong Size { get => size; set => size += value; }
        public ulong Bad { get => bad; set => bad += value; }
        public string Name => name;

        public Coze(string title)
        {
            name = title;
        }

        public void Reset()
        {
            count = 0;
            size = 0;
            bad = 0;
        }

        public void Update(Entry e)
        {
            Count = e.Count;
            Size = e.Size;
            Bad = e.Bad;
        }
    }

    public class Entry
    {
        public string Origin { get; set; }
        public uint Sequence { get; set; }
        public uint Instance { get; set; }
        public byte Channel { get; set; }
        public bool Realtime { get; set; }
        public uint Count { get; set; }
        public uint Size { get; set; }
        public uint Bad { get; set; }
        public long Elapsed { get; set; }
        public DateTime GenTime { get; set; }
        public DateTime AcqTime { get; set; }
        public string Reference { get; set; }
        public string UPI { get; set; }
    }
}
