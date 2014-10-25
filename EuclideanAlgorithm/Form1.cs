using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EuclideanAlgorithm
{
    public partial class Form1 : Form
    {
        public static int numIterations;

        public Form1()
        {
            InitializeComponent();

            comboBox_Method.SelectedIndex = 0;  //Sub
        }

        private void button_getGCD_Click(object sender, EventArgs e)
        {
            try
            {
                ulong l_a = Convert.ToUInt64(numericUpDown_a.Value);
                ulong l_b = Convert.ToUInt64(numericUpDown_b.Value);

                Stopwatch timer = new Stopwatch();   //other way to initialize: Stopwatch timer = Stopwatch.StartNew();

                timer.Start();

                ulong gcd;

                numIterations = 0;

                switch (comboBox_Method.SelectedIndex)
                {
                    case 0:
                        gcd = getGCDSub(l_a, l_b);
                        textBox_Results.AppendText("\r\n Subtraction Method: GCD=" + gcd.ToString());
                        break;
                    case 1:
                        gcd = getGCDMod(l_a, l_b);
                        textBox_Results.AppendText("\r\n Modulo Method: GCD=" + gcd.ToString());
                        break;
                    case 2:
                        gcd = getGCDPrimeFactorization(l_a, l_b);
                        textBox_Results.AppendText("\r\n PrimeFactorization Method: GCD=" + gcd.ToString());
                        break;
                    default:
                        return;
                }

                timer.Stop();

                textBox_Results.AppendText("\r\n CPU-time(ticks):" + timer.ElapsedTicks);
                textBox_Results.AppendText("\r\n CPU-time(ms):" + timer.ElapsedMilliseconds);
                textBox_Results.AppendText("\r\n Number of iterations:" + numIterations);
            }
            catch (Exception ex)
            {
                textBox_Results.AppendText("Your input is wasn't valid!");
                Console.WriteLine(ex);
            }
        }

        public static ulong getGCDMod(ulong a, ulong b)
        {

            while (b != 0)
            {
                numIterations++;
                ulong t = b;
                b = a % b;
                a = t;
            }
            return a;
        }

        public static List<ulong> getPrimeNumbers(ulong n)
        {
           
            List<ulong> primeNumbers = new List<ulong>();
            for (ulong i = 2; i <= n; i++)
            {
                numIterations++;
                while (n % i == 0 )
                {
                    
                    primeNumbers.Add(i);
                    n /= i;
                   

                }
            }

            return primeNumbers;
        }

        public static ulong getGCDPrimeFactorization(ulong a, ulong b)
        {
            
            List<ulong> primeNumbersA = getPrimeNumbers(a);
            List<ulong> primeNumbersB = getPrimeNumbers(b);
            List<ulong> commonFactors = new List<ulong>();
            foreach (ulong _a in primeNumbersA){
                foreach (ulong _b in primeNumbersB)
                {
                    numIterations++;
                    if (_a == _b & !commonFactors.Contains(_a))
                        commonFactors.Add(_a);
                }
            }

            if (!commonFactors.Any())
            {
                return 0;
            }
            ulong x = 1;
            foreach (ulong factor in commonFactors)
            {
                numIterations++;
                x *= factor;
            }
            return x;
            
        }

        public static ulong getGCDSub(ulong a, ulong b)
        {
            if (a == 0)
                return b;

            while (b != 0)
            {
                numIterations++;

                if (a > b)
                    a = a - b;
                else
                    b = b - a;
            }

            return a;
        }


        /** LOOPS */
        private void button_loops_Click(object sender, EventArgs e)
        {
            try
            {
                ulong l_a = Convert.ToUInt64(numericUpDown_a.Value);
                ulong l_b = Convert.ToUInt64(numericUpDown_b.Value);

                List<long> listCPUTimes = new List<long>();

                int numOfLoops = (int)numericUpDown_loops.Value;

                Stopwatch timer = new Stopwatch();   //other way to initialize: Stopwatch timer = Stopwatch.StartNew();

                for (int i = 0; i < numOfLoops; i++)
                {
                    timer.Reset();
                    timer.Start();

                    switch (comboBox_Method.SelectedIndex)
                    {
                        case 0:
                            getGCDSub(l_a, l_b);
                            break;
                        case 1:
                            getGCDMod(l_a, l_b);
                            break;
                        default:
                            return;
                    }

                    timer.Stop();
                    listCPUTimes.Add(timer.ElapsedTicks);
                    textBox_Results.AppendText("\r\n Iteration " + i.ToString() + ", CPU-time(ticks):" + timer.ElapsedTicks);
                }

                //Get Mean and SD
                double meanCPUTicks = getMean(listCPUTimes);
                double varianceCPUTicks = getVariance(listCPUTimes);
                double standardDeviationCPUTicks = Math.Sqrt(varianceCPUTicks);

                textBox_Results.AppendText("\r\n Mean CPU-time(ticks):" + meanCPUTicks);
                textBox_Results.AppendText("\r\n Standard Deviation CPU-time(ticks):" + standardDeviationCPUTicks);

                //Get Median
                //ToDo: your implementation

                //Get Histogram            
                //ToDo: your implementation
                long startHisto = 0; //get min value
                long endHisto = 0; //get max value

                List<int> histo = getHistogram(startHisto, endHisto, listCPUTimes);

                //Get Mode
                //ToDo: your implementation



                //show normalized histogram, probability density of CPU-time (ticks)
                double[] histoNormalized = getNormalizedHistogram(startHisto, endHisto, listCPUTimes);
                textBox_Results.AppendText("\r\n Normalized histogram:");
                for (int i = 0; i < histoNormalized.Count(); i++)
                    textBox_Results.AppendText("\r\n" + i.ToString() + ": " + histoNormalized[i]);

                //add data to chart
                chart1.Series[0].Points.Clear();

                //ToDo: your implementation
                double cpuTicksHistoCounter = 0;

                foreach (double probCPUTicks in histoNormalized)
                {
                    //add datapoint X,Y to chart
                    chart1.Series[0].Points.AddXY(cpuTicksHistoCounter, probCPUTicks);

                    //compute next counter
                    //ToDo: your implementation
                }
            }
            catch (Exception ex)
            {
                textBox_Results.AppendText("Your input is wasn't valid!");
                Console.WriteLine(ex);
            }
        }


        public static double getMean(List<long> resultset)
        {
            //ToDo: your implementation
			return 0;
        }

        public static double getVariance(List<long> resultSet)
        {
            //ToDo: your implementation
			return 0;
        }

        public static List<int> getHistogram(double start, double end, List<long> data)
        {
			//ToDo: your implementation
            int num_bins = 1;

            List<int> histo = new List<int>(num_bins);

            return histo;
        }

        public static List<int> genRandomList(int min, int max, int count)
        {
            List<int> randomList = new List<int>();
            Random random = new Random();
            for (int i = 0; i < count; ++i)
            {
                randomList.Add(random.Next(min, max + 1));
            }
            return randomList;
        }

        public static double[] getNormalizedHistogram(double start, double end, List<long> data)
        {
			//ToDo: your implementation
            int num_bins = (int)Math.Round(Math.Sqrt(data.Count));

            double[] histo = new double[num_bins];

            
            return histo;
        }

       

    }
}
