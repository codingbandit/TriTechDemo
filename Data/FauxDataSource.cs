using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Timers;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Threading;


namespace TriTechDemo.Data
{
    public class FauxDataSource : INotifyPropertyChanged
    {
        private int _updateCount = 0;
        private ObservableCollection<Unit> _unitDataSource { get; set; }
        public CollectionViewSource UnitViewSource { get; set; }

        public int UpdateCount
        {
            get { return _updateCount; }
            set { _updateCount = value; NotifyPropertyChanged(); }
        }
 
        public FauxDataSource()
        {
            _unitDataSource = new ObservableCollection<Unit>();
            
            int jobid = 5000;
            
            //generate a few thousand Units, with 1 detail(job) each
            for (int i = 1; i <= 3000; i++)
            {
                Random r = new Random(DateTime.Now.Millisecond);
                Unit u = new Unit();
                u.Id = i;
                u.Name = "Unit " + i;
                u.Location = RandomString(10, false);
                u.UnitStatus = (Status) r.Next(1, 6);
                UnitJob j = new UnitJob();
                j.JobId = jobid--;
                j.JobDescription = RandomString(15, false);
                j.UnitId = i;
                j.JobDurationMinutes = r.Next(5, 20);
                u.Jobs = new ObservableCollection<UnitJob>();
                u.Jobs.Add(j);
                _unitDataSource.Add(u);
            }
            
            UnitViewSource = new CollectionViewSource();
            UnitViewSource.Source = _unitDataSource;
            UnitViewSource.IsLiveFilteringRequested = true;
            UnitViewSource.IsLiveSortingRequested = true;
        }

        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random(DateTime.Now.Millisecond);
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        public void PerformRandomStatusChanges()
        {
            if (_unitDataSource != null)
            {
                Random r = new Random(DateTime.Now.Millisecond);
                //change 5 to 10 records in the datasource
                int numChanges = r.Next(5, 10);
               
                for (int i = 0; i < numChanges; i++)
                {
                    int idx = r.Next(0, _unitDataSource.Count - 1);
                    _unitDataSource[idx].UnitStatus = (Status) r.Next(1, 6);
                }
                UpdateCount++;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
      
    }
}
