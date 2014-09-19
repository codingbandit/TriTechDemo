using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriTechDemo.Data;

namespace TriTechDemo.ViewModel
{
    public class MainViewModel : BaseEntity
    {
        private FauxDataSource _dataSource;

        public MainViewModel()
        {
            _dataSource = new FauxDataSource();
        }

        public FauxDataSource DataSource
        {
            get { return _dataSource; }
            set { _dataSource = value; NotifyPropertyChanged(); }
        }
    }
}
