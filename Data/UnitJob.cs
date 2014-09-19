using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriTechDemo.Data
{
    public class UnitJob : BaseEntity
    {
        private int _jobId;
        private int _unitId;
        private string _jobDescription;
        private int _jobDurationMinutes;

        public int JobId
        {
            get { return _jobId; }
            set { _jobId = value; NotifyPropertyChanged(); }
        }

        public int UnitId
        {
            get { return _unitId; }
            set { _unitId = value; NotifyPropertyChanged(); }
        }

        public string JobDescription
        {
            get { return _jobDescription; }
            set { _jobDescription = value; NotifyPropertyChanged(); }
        }

        public int JobDurationMinutes
        {
            get { return _jobDurationMinutes; }
            set { _jobDurationMinutes = value; NotifyPropertyChanged(); }
        }
    }
}
