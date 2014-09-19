using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Data;
using TriTechDemo.Data;
using TriTechDemo.ViewModel;

namespace TriTechDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer _dataChangeTimer = new DispatcherTimer();
       

        public MainWindow()
        {
            InitializeComponent();
            
            _dataChangeTimer.Interval = TimeSpan.FromSeconds(3);
            _dataChangeTimer.Tick += DataChangeTimerTick;
            _dataChangeTimer.Start();

            
        }

        private void DataChangeTimerTick(object sender, EventArgs e)
        {
           MainViewModel vm =  (MainViewModel) spMain.DataContext;
           vm.DataSource.PerformRandomStatusChanges();
           
        }

        private GridViewRow _previousRow;

        private void GvUnitGrid_OnRowIsExpandedChanged(object sender, RowEventArgs e)
        {
            if (e.Row != null)
            {
                var row = (GridViewRow) e.Row;
                if (row.IsExpanded)
                {
                    if (_previousRow != null)
                    {
                        //doesn't work with ICollectionView O.o
                        _previousRow.IsExpanded = false;
                    }
                    _previousRow = row;
                    
                }
            }
        }


    }
}
