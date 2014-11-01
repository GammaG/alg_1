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
using System.Windows.Forms.DataVisualization.Charting;

namespace EuclideanAlgorithm
{
    public partial class Form1 : Form
    {
        public static int numIterations;
        Boolean alternativeDiagramm = true;
        Boolean clearDiagramm = false;

        public Form1()
        {
            InitializeComponent();

            comboBox_Method.SelectedIndex = 0;  //Sub
            diagramComboBox.SelectedIndex = 0;
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
                int a = Convert.ToInt32(numericUpDown_a.Value);
                int b = Convert.ToInt32(numericUpDown_b.Value);
                int numOfLoops = (int)numericUpDown_loops.Value;
                
                ulong l_a = Convert.ToUInt64(a);
                ulong l_b = Convert.ToUInt64(b);

                List<ulong> randomListA = genRandomList(a, b, numOfLoops);
                List<ulong> randomListB = genRandomList(a, b, numOfLoops);

                List<long> listCPUTimes = new List<long>();
                List<int> stepList = new List<int>();
               
                Stopwatch timer = new Stopwatch();   //other way to initialize: Stopwatch timer = Stopwatch.StartNew();
                String mode = "";
                for (int i = 0; i < numOfLoops; i++)
                {
                    numIterations = 0;
                    timer.Reset();
                    timer.Start();
                    ulong _a = randomListA[i];
                    ulong _b = randomListB[i];
                    

                    switch (comboBox_Method.SelectedIndex)
                    {
                        case 0:
                            getGCDSub(_a, _b);
                            mode = "Subtraction";
                            break;
                        case 1:
                            getGCDMod(_a, _b);
                            mode = "Modulo";
                            break;
                        case 2:
                            getGCDPrimeFactorization(_a, _b);
                            mode = "PrimeFactors";
                            break;
                        default:
                            return;
                    }

                    timer.Stop();
                    listCPUTimes.Add(timer.ElapsedTicks);
                    stepList.Add(numIterations);
                    textBox_Results.AppendText("\r\n Iteration " + i.ToString() + ", CPU-time(ticks):" + timer.ElapsedTicks + ", Steps:" +  numIterations);
                }


                
                switch (diagramComboBox.SelectedIndex)
                {
                    case 0:
                     
                        alternativeDiagramm = true;
                        break;
                    case 1:
                       
                        alternativeDiagramm = false;
                        break;

                    default:
                        return;
                }



                //Get Mean and SD
                double meanCPUTicks = getMean(listCPUTimes);
                double varianceCPUTicks = getVariance(listCPUTimes);
                double standardDeviationCPUTicks = Math.Sqrt(varianceCPUTicks);

                textBox_Results.AppendText("\r\n Mean CPU-time(ticks):" + meanCPUTicks);
                textBox_Results.AppendText("\r\n Standard Deviation CPU-time(ticks):" + standardDeviationCPUTicks);


                //add data to chart
                String name = "StandardDeviation for\n"+mode;

                if (clearDiagramm)
                {
                    chart1.Series.Clear();
                }


                if (!chart1.Series.IsUniqueName(mode) & alternativeDiagramm)
                {
                    chart1.Series.Remove(chart1.Series[mode]);
                    chart1.Series.Remove(chart1.Series[name]);
                }
                else if (!chart1.Series.IsUniqueName(mode))
                {
                    chart1.Series.Remove(chart1.Series[mode]);
                }

        
                chart1.Series.Add(mode);
               
                chart1.Series[mode].ChartType = SeriesChartType.Point;
               
                for (int i = 0; i < numOfLoops; i++)
                {
                    ulong x = (randomListA[i]+randomListB[i])/2;
                  
                    long y;
                    if (alternativeDiagramm)
                        y = listCPUTimes[i];
                    else
                        y = stepList[i];
                    chart1.Series[mode].Points.AddXY(Convert.ToInt32(x),y);
                
                }
                if (alternativeDiagramm)
                {
                    chart1.Series.Add(name);
                    chart1.Series[name].ChartType = SeriesChartType.ErrorBar;
                    chart1.Series[name]["ErrorBarSeries"] = mode + ":Y1";
                    chart1.Series[name]["ErrorBarType"] = "StandardDeviation";
                }

                clearDiagramm = false;
                
            }
            catch (ArgumentOutOfRangeException ex)
            {
                textBox_Results.AppendText("\n"+ex.Message);
                
            }

            catch (Exception ex)
            {
                textBox_Results.AppendText("\nYour input is wasn't valid!");
                Console.WriteLine("\n"+ex);
                
            }
        }

        private static List<ulong> generateFactors(List<ulong> a, List<ulong> b){
            List<ulong> list = new List<ulong>();
            for(int i= 0; i < a.Count; i++){

                list[i] = (a[i] + b[i]) / 2;
            }

            return list;
        }
        

        

        private static long getMax(List<long> listCPUTimes)
        {
            long max = 0;
            foreach (long t in listCPUTimes)
            {
                if (t > max)
                {
                    max = t;
                }
            }
            return max;
        }

        private static long getMin(List<long> listCPUTimes)
        {
            long min = long.MaxValue;
            foreach (long t in listCPUTimes)
            {
                if (t < min)
                {
                    min = t;
                }
            }
            return min;
        }


        private static long getMedian(List<long> listCPUTimes)
        {
            long d = 0;
            foreach (long t in listCPUTimes)
            {
                d += t;
            }
            return d / listCPUTimes.Count;

        }

        public static double getMean(List<long> resultset)
        {
            ulong number = Convert.ToUInt64(resultset.Count);
            ulong x = 0;
            foreach (ulong time in resultset)
            {
                x += time;
            }

			return x/number;
        }

        public static double getVariance(List<long> resultSet)
        {
            ulong maxValue = 0;
            ulong secondValue = 0;

            foreach (ulong t1 in resultSet){
                if(t1 > maxValue){
                    secondValue = maxValue;
                    maxValue = t1;
                }
            }

			return maxValue-secondValue;
        }

        public static List<int> getHistogram(double start, double end, List<long> data)
        {
			//ToDo: your implementation
            int num_bins = 1;

            List<int> histo = new List<int>(num_bins);

            return histo;
        }

        public static List<ulong> genRandomList(int min, int max, int count)
        {
            List<ulong> randomList = new List<ulong>();
            Random random = new Random();
            for (int i = 0; i < count; ++i)
            {
                randomList.Add(Convert.ToUInt64( random.Next(min, max + 1)));
            }
            return randomList;
        }

        public static double[] getNormalizedHistogram(double start, double end, List<long> data)
        {
                    
            int num_bins = (int)Math.Round(Math.Sqrt(data.Count));

            double[] histo = new double[num_bins];

           
            


            return histo;
        }

        private void numericUpDown_b_ValueChanged(object sender, EventArgs e)
        {

        }

        private void diagramComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            clearDiagramm = true;
        }

        private void clearD_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
     

        }

     }
}
