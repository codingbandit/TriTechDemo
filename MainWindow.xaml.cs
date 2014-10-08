using System;
using System.Collections.Generic;
using System.Diagnostics;
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

using Telerik.Windows.Controls;
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

        private Point positionInScrollViewer;
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

        private void ScrollToSelected()
        {
            if (_previousRow != null)
            {
                var scroller = this.gvUnitGrid.ChildrenOfType<GridViewScrollViewer>().First();
                try
                {
                    // see if the row has moved and move it back
                    FrameworkElement element = _previousRow as FrameworkElement;
                    var transform = element.TransformToVisual(scroller);
                    var currentpositionInScrollViewer = transform.Transform(new Point(0, 0));
                    Debug.WriteLine("Row found at Y offset {0}", currentpositionInScrollViewer.Y);
                    if (Math.Abs(this.positionInScrollViewer.Y - currentpositionInScrollViewer.Y) > 10)
                    {
                        Debug.WriteLine("Original Scroller Y offset {0}", scroller.VerticalOffset);
                        var diff = this.positionInScrollViewer.Y - currentpositionInScrollViewer.Y;
                        Debug.WriteLine("Scroller diff {0}", diff);
                        scroller.ScrollToVerticalOffset(scroller.VerticalOffset - diff);
                        Debug.WriteLine("New Scroller Y offset {0}", scroller.VerticalOffset);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }

        private void GvUnitGrid_OnLayoutUpdated(object sender, EventArgs e)
        {
            // layout has updated, check if we should move the row
            if (cbFreezeRow.IsChecked.HasValue && cbFreezeRow.IsChecked.Value)
            {
                this.ScrollToSelected();
            }
            
        }

        private void CbFreezeRow_OnChecked(object sender, RoutedEventArgs e)
        {
            // track the location of this row
            var scroller = this.gvUnitGrid.ChildrenOfType<GridViewScrollViewer>().First();
            FrameworkElement element = _previousRow as FrameworkElement;
            var transform = element.TransformToVisual(scroller);
            positionInScrollViewer = transform.Transform(new Point(0, 0));
            Debug.WriteLine("Row selected at Y offset {0}", positionInScrollViewer.Y);
            Debug.WriteLine("Scroller Y offset {0}", scroller.VerticalOffset);
        }
    }
}
