using Windows.Storage;

namespace Snake
{
    static class PersistentState
    {
        public static int Highscore
        {
            get
            {
                if(values.ContainsKey(nameof(Highscore)))
                {
                    var value = values[nameof(Highscore)];
                    if(value.GetType() == typeof(int))
                    {
                        return (int)value;
                    }
                }
                return 0;
            }
            set
            {
                if (value != Highscore)
                {
                    values[nameof(Highscore)] = value;
                    Save();
                }
            }
        }

        static PersistentState()
        {
            values = new ApplicationDataCompositeValue();

            ApplicationData.Current.DataChanged += Current_DataChanged;

            Current_DataChanged(ApplicationData.Current, null);
        }

        private static void Current_DataChanged(ApplicationData sender, object args)
        {
            values = (ApplicationDataCompositeValue)ApplicationData.Current.RoamingSettings.Values["HighPriority"] ?? new ApplicationDataCompositeValue();
        }

        static void Save()
        {
            ApplicationData.Current.RoamingSettings.Values["HighPriority"] = values;
            ApplicationData.Current.SignalDataChanged();
        }

        static ApplicationDataCompositeValue values;
    }
}