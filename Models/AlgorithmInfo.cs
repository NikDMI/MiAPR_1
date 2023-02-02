using System;
using System.ComponentModel;

namespace MiAPR_1.Models
{
    //represent information about K-average algorithm
    public class AlgorithmInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int ObjectsCount 
        { 
            get 
            {
                return _objectCount;
            } 
            set 
            { 
                if (_objectCount != value)
                {
                    _objectCount = value;
                    Notify("ObjectsCount");
                }
            } 
        }


        public int ClassCount
        {
            get
            {
                return _classCount;
            }
            set
            {
                if (_classCount != value)
                {
                    _classCount = value;
                    Notify("ClassCount");
                }
            }
        }

        private void Notify(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(null, new PropertyChangedEventArgs(prop));
            }
        }

        private int _objectCount;
        private int _classCount;
    }
}
