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

namespace MonteCarloSimulator
{
    /// <summary>
    /// Interaction logic for InputPage.xaml
    /// </summary>
    public partial class InputPage : Page
    {

        public InputPage()
        {
            InitializeComponent();
        }

        private void NavigationButtonClick(object sender, RoutedEventArgs e)
        {
            //track input field values
            int numSims = 0;
            int numPoints = 0;
            //track if var is valid
            bool points = false;
            bool sims = false;
            errorLine.Content = "";

            //try to convert number of simulation input
            try
            {
                numSims = Convert.ToInt32(NumberOfSimulations.Text);
                sims = true;
            }
            catch(FormatException error){
                CustomLogger.Log(error.ToString());

                errorLine.Content += "Invalid Simulations Value";
            }
            catch(Exception error){
                CustomLogger.LogCritical(error.ToString());
            }

            //try to convert number of points input
            try
            {
                numPoints = Convert.ToInt32(PointsToPlot.Text.Replace(",", ""));
                points = true;
            }
            catch (FormatException error)
            {
                CustomLogger.Log(error.ToString());
                if ((string)errorLine.Content != "")
                {
                    errorLine.Content += " And ";
                }
                errorLine.Content += "Invalid Plot Points Value ";
            }
            catch (Exception error)
            {
                CustomLogger.LogCritical(error.ToString());
            }

            //validate valid Simulation range
            if (sims)
                if (numSims < 1)
                {
                    if ((string)errorLine.Content != "")
                    {
                        errorLine.Content += " And ";
                    }
                    errorLine.Content += "At Least 1 Simulation Required";
                    points = false;
                }
            if (numSims > 200)
            {
                {
                    if ((string)errorLine.Content != "")
                    {
                        errorLine.Content += " And ";
                    }
                    errorLine.Content += "No More Than 200 Simulations";
                    points = false;
                }
            }

            //validate valid Plot Points range
            if (points)
                if (numPoints < 10) {
                    if ((string)errorLine.Content != "")
                    {
                        errorLine.Content += " And ";
                    }
                    errorLine.Content += "10 Or More Plot Points Required";
                    points = false;
                }
                if (numPoints > 1000000) {
                {
                    if ((string)errorLine.Content != "")
                    {
                        errorLine.Content += " And ";
                    }
                    errorLine.Content += "Plot Points be 1,000,000 or less";
                    points = false;
                }
            }

            // View simulation results
            if (sims && points) {
                errorLine.Content = "";
                SimulationPage simulationPage = new SimulationPage(numSims, numPoints);
                this.NavigationService.Navigate(simulationPage);
            }
        }

        private void ResetButtonClick(object sender, RoutedEventArgs e)
        {
            NumberOfSimulations.Text = "25";
            PointsToPlot.Text = "10,000";
            errorLine.Content = "";
        }

        //resize circle in square example
        private void ResizeDiagram(object sender, SizeChangedEventArgs e)
        {
            //get canvas
            Grid grid = sender as Grid;

            //pick smallest dimension to keep x by y 1:1 constraint
            double size = Math.Min(grid.ActualWidth, grid.ActualHeight) - 50;

            canvas.Width = size;
            canvas.Height = size;

            //resize rectangle
            rectangle.Width = size;
            rectangle.Height = size;

            //resize circle
            circle.Width = size;
            circle.Height = size;

            grid.InvalidateVisual();
            grid.UpdateLayout();
        }
    }


}
