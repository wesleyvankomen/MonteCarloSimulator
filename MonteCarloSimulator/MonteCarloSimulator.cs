using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonteCarloSimulator
{

    public class MonteCarlo
    {
    
        //declare class vars and get/set functions
        private int pointsInCircle;
        public int PointsInCircle{
            get { return pointsInCircle; }}

        private double piAproximation;
        public double PiApproximation {
            get { return piAproximation; }}

        private int totalPoints;
        public int TotalPoints { 
            get { return totalPoints; }}

        public int PointsOutsideCircle{
            get { return totalPoints - pointsInCircle; }}
        
        //random number generator
        private Random randomNumber = new Random();

        //thread locks
        Object randLock, numberLock;


        //Default Constructor
        public MonteCarlo()
        {
            new MonteCarlo(10000);
        }

        //Overload Constructor w/ simulation size specified
        public MonteCarlo(int numberOfPoints)
        {
            this.totalPoints = numberOfPoints;
            this.piAproximation = 0;
            this.pointsInCircle = 0;
            randLock = new object();
            numberLock = new object();
        }

        // Simulates a Monte Carlo experiment in which pi can be approximated through randomly
        // placing points in a 2 by 2 square with a circle of radio 1 inside. Pi is calculated
        // through approximating the area of a circle with a radius of 1.
        public double Simulate()
        {
            //reset class vars
            this.pointsInCircle = 0;

            int threads = 4;

            int workload = totalPoints / threads;

            //spin up 4 threads to perform calculations
            var tasks = new Task[threads];

            for (int i = 0; i < threads - 1; i++)
            {
                tasks[i] = Task.Run(() => PartialSimulation(workload));
            }

            //run partial sim for remainder of work
            workload = TotalPoints - ((threads - 1) * workload);
            tasks[threads-1] = Task.Run(() => PartialSimulation(workload));

            //wait for threads to finish
            Task.WaitAll(tasks);

            //calculate pi (area of 2 by 2 square times percentage of points in circle)
            this.piAproximation = 4.0 * ((double)pointsInCircle / (double)totalPoints);

            return piAproximation;
        }

        private void PartialSimulation(int iterations)
        {
            double x, y;

            // generate points and check if they're inside the circle
            for (int i = 0; i < iterations; i++)
            {
                //generate random point 
                lock (randLock)
                {
                    x = randomNumber.NextDouble();
                    y = randomNumber.NextDouble();
                }
                //check if point is in circle (a^2 + b^2 = c^2, where c is the radius of 1)
                if (x * x + y * y <= 1.0)
                {
                    lock (numberLock) { 
                        this.pointsInCircle++;
                    }
                }
            }
        }

        public override String ToString()
        {
            String result =  pointsInCircle + " | " + TotalPoints + " | " + piAproximation;

            return result;
        }

        public static void Main(string[] args)
        {
            MonteCarlo sim = new MonteCarlo(10000);

            Console.WriteLine("Pi is approximated to be {0} based on a simulation of {1} points",
                sim.Simulate(), sim.totalPoints);

            // pause so results can be viewed
            Console.WriteLine("Press Any Key To Exit");
            Console.ReadKey();
        }
    }
}

