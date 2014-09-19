using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriTechDemo.Data
{
    public class Unit : BaseEntity
    {
        private int _id;
        private string _name;
        private string _location;
        private Status _unitStatus;
        private ObservableCollection<UnitJob> _jobs;  

        public int Id
        {
            get { return _id; }
            set { _id = value; NotifyPropertyChanged();}
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyPropertyChanged(); }
        }

        public string Location
        {
            get { return _location; }
            set { _location = value; NotifyPropertyChanged(); }
        }

        

        public ObservableCollection<UnitJob> Jobs
        {
            get { return _jobs; }
            set { _jobs = value; NotifyPropertyChanged(); }
        }

        public Status UnitStatus
        {
            get { return _unitStatus; }
            set { _unitStatus = value; NotifyPropertyChanged(); }
        }
    }
}
