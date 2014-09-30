using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

using Expression = System.Linq.Expressions.Expression;

namespace TriTechDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer _dataChangeTimer = new DispatcherTimer();

        private Func<Unit, bool> _filterExpression;
        private GridViewRow _previousRow;

        public MainWindow()
        {
            InitializeComponent();
            MainViewModel vm = (MainViewModel)spMain.DataContext;
            vm.DataSource.UnitViewSource.Filter += UnitViewSource_Filter;
            _dataChangeTimer.Interval = TimeSpan.FromMilliseconds(16);
            _dataChangeTimer.Tick += DataChangeTimerTick;
            _dataChangeTimer.Start();

            
        }

        private void DataChangeTimerTick(object sender, EventArgs e)
        {
           MainViewModel vm =  (MainViewModel) spMain.DataContext;
          
           vm.DataSource.PerformRandomStatusChanges();
           
        }

        void UnitViewSource_Filter(object sender, FilterEventArgs e)
        {
            Unit item = (Unit)e.Item;

            if (_filterExpression != null)
            {
                e.Accepted = _filterExpression.Invoke(item);
            }
            else
            {
                e.Accepted = true;
            }
        }

        private void GvUnitGrid_OnFiltered(object sender, GridViewFilteredEventArgs gridViewFilteredEventArgs)
        {
           
            if (gvUnitGrid.FilterDescriptors.Any())
            {
                // get a new compiled filter expression to use to check when values change
                ParameterExpression values = Expression.Parameter(typeof(Unit));
                var filterExpression = gvUnitGrid.FilterDescriptors.CreateFilterExpression(values);
                var compiledExpression = Expression.Lambda<Func<Unit, bool>>(filterExpression, values).Compile();

                _filterExpression = compiledExpression;
            }
            else
            {
                _filterExpression = null;
            }
        }

        private void GvUnitGrid_OnRowIsExpandedChanged(object sender, RowEventArgs e)
        {
            if (e.Row != null)
            {
                var row = (GridViewRow)e.Row;
                if (row.IsExpanded)
                {
                    if (_previousRow != null)
                    {
                        //need something different for ICollectionView? 
                        _previousRow.IsExpanded = false;
                    }
                    _previousRow = row;

                }
            }
        }

        private void GvUnitGrid_OnFiltering(object sender, GridViewFilteringEventArgs e)
        {
           
        }
    }
}
