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
using System.Threading;
namespace MonteCarloSimulator
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class SimulationPage : Page
    {
        private int iterations;
        private int points;
        private double piAproximation;
        public double PiAproximation
        {
            get { return piAproximation; }
            set { piAproximation = value; }
        }

        //Default Page Constructor
        public SimulationPage()
        {
            iterations = 25;
            points = 100000;

            InitializeComponent();

            RunSimulation(iterations, points);
        }

        //Default Page Constructor
        public SimulationPage(int iterations, int points)
        {
            this.points = points;
            this.iterations = iterations;
            InitializeComponent();
            RunSimulation(iterations, points);

        }

        public void RunSimulation(int numOfResults, int simIntensity) {

            List<MonteCarloResult> sims = new List<MonteCarloResult>();

            MonteCarlo sim = new MonteCarlo(simIntensity);

            for (int i = 0; i < numOfResults; i++) {
            
                sim.Simulate();

                //add results results list
                sims.Add(new MonteCarloResult(){
                        Pi = sim.PiApproximation,
                        Hits = sim.PointsInCircle,
                        Misses = sim.TotalPoints - sim.PointsInCircle});
            }

            results.ItemsSource = sims;

        }

        //resize columns on window resize
        private void SimulationPageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            //get gridview from listview
            ListView listView = sender as ListView;
            GridView gView = listView.View as GridView;

            double workingWidth;

            //if (visibility > 0 )
                //workingWidth = listView.ActualWidth;
            //else
                workingWidth = listView.ActualWidth - SystemParameters.VerticalScrollBarWidth; // take into account vertical scrollbar

            var col1 = 0.33;
            var col2 = 0.33;
            var col3 = 0.34;

            gView.Columns[0].Width = workingWidth * col1;
            gView.Columns[1].Width = workingWidth * col2;
            gView.Columns[2].Width = workingWidth * col3;
        }

        private void RunAgainButtonClick(object sender, RoutedEventArgs e)
        {
            RunSimulation(iterations, points);
        }

        private void GoBackButtonClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

    }

    public class MonteCarloResult
    {
        public double Pi{ get; set; }
        public int  Hits { get; set; }
        public int Misses { get; set; }

        public string PiString{
            get{
                return string.Format("{0:F5}", Pi);}}

        public string HitsString{
            get{
                return string.Format("{0:n0}", Hits);}}

        public string MissesString { 
            get{
            return string.Format("{0:n0}", Misses);}}
    }
}
