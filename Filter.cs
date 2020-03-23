using System.ComponentModel;

namespace monhdk4
{
    public class Filter: INotifyPropertyChanged
    {
        private string upi = string.Empty;
        
        private bool realtime = true;
        private bool playback = true;

        private bool vic1 = true;
        private bool vic2 = true;
        private bool lrsd = true;

        private bool test = true;
        private bool sim1 = true;
        private bool sim2 = true;
        private bool ops  = true;

        public string UPI { 
            get => upi; 
            set => upi = value;
        }

        public bool Realtime { 
            get => realtime;
            set
            {
                realtime = value;
                Notify("Realtime");
            }
        }
        public bool Playback { 
            get => playback;
            set
            {
                playback = value;
                Notify("Playback");
            }
        }

        public bool Vic1 { 
            get => vic1;
            set
            {
                vic1 = value;
                Notify("Vic1");
            }
        }
        public bool Vic2 { 
            get => vic2;
            set
            {
                vic2 = value;
                Notify("Vic2");
            }
        }
        public bool Lrsd { 
            get => lrsd;
            set
            {
                lrsd = value;
                Notify("Lrsd");
            }
        }
        public bool Test {
            get => test;
            set
            {
                test = value;
                Notify("Test");
            }
        }
        public bool Sim1 { 
            get => sim1;
            set
            {
                sim1 = value;
                Notify("Sim1");
            }
        }
        public bool Sim2 { 
            get => sim2;
            set
            {
                sim2 = value;
                Notify("Sim2");
            }
        }
        public bool Ops { 
            get => ops;
            set {
                ops = value;
                Notify("Ops");
            }
        }

        public bool Keep(object obj)
        {
            Entry e = (Entry)obj;
            if (e == null)
            {
                return false;
            }
            return IsUPI(e) && IsMode(e) && IsInstance(e) && IsChannel(e);
        }

        private bool IsInstance(Entry e)
        {
            if (Test && Sim1 && Sim2 && Ops)
            {
                return true;
            }
            return e.Instance switch
            {
                0 => Test,
                1 => Sim1,
                2 => Sim2,
                255 => Ops,
                _ => false,
            };
        }

        private bool IsChannel(Entry e)
        {
            if (Vic1 && Vic2 && Lrsd)
            {
                return true;
            }
            return e.Channel switch
            {
                1 => Vic1,
                2 => Vic2,
                3 => Lrsd,
                _ => false,
            };
        }

        private bool IsMode(Entry e)
        {
            if (Realtime && Playback)
            {
                return true;
            }
            if (Realtime && e.Realtime)
            {
                return true;
            }
            return Playback && !e.Realtime;
        }

        private bool IsUPI(Entry e)
        {
            return UPI?.Length == 0 || e.UPI == UPI;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Notify(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
