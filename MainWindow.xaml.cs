using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Simpaging
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int pgSize = 512;
        string[] PageT = new string[8];
        string[] TextArray;
        List<int> freeFrames = new List<int>();
        int step = 0;



        public int NumberPages(int temp)
        {
            int numPg = 1;
            while ((pgSize * numPg) < temp)
            {
                numPg++;
            }
            return numPg;
        }

        public void AllocateFrame(string[] readfile, int x)
        {


            string[] txtArray = readfile;
            string[] parts = null;
            while (parts == null)
            {
                try
                {
                    parts = txtArray[x].Split(new[] { ' ' });
                }
                catch (NullReferenceException e)
                {
                    MessageBox.Show("No File loaded.");
                    Console.WriteLine(e.Message);
                    return;
                }
            }

            int PID = 0;
            int TextSize = 0;
            int dataSize = 0;

            if (!parts.Contains("Halt"))
            {
                

                try
                {
                    PID = Int32.Parse(parts[0]);
                    TextSize = Int32.Parse(parts[1]);
                    dataSize = Int32.Parse(parts[2]);
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }

                int t = 0;
                int d = 0;

                freeFrames.ForEach(Print);
                if((NumberPages(TextSize) + NumberPages(dataSize)) <=freeFrames.Count)
                {
                    for(int s = 0; s< NumberPages(TextSize); s++)
                    {
                        PageT[freeFrames[0]] = "P" + PID + " Text Page: " + t;
                        t++;
                        freeFrames.RemoveAt(0);
                    }

                    for (int s = 0; s < NumberPages(dataSize); s++)
                    {
                        PageT[freeFrames[0]] = "P" + PID + " Data Page: " + d;
                        d++;
                        freeFrames.RemoveAt(0);
                    }

                }
                else
                {
                    MessageBox.Show("Not Enough Free Frames");
                }
                
            }
            else
            {
                PID = Int32.Parse(parts[0]);
                ClearMem(PID.ToString());
            }
        }

        public string[] GetFrameArray()
        {
            return PageT;
        }

        public void FreshPageT()
        {
            Array.Clear(PageT, 0, PageT.Length);
            freeFrames.Clear();
            for (int x = 0; x < PageT.Length; x++)
            {
       
                PageT[x] = null;
                freeFrames.Add(x);
            }
        }

        public void UpdateTable()
        {
            box0.Text = PageT[0];
            box1.Text = PageT[1];
            box2.Text = PageT[2];
            box3.Text = PageT[3];
            box4.Text = PageT[4];
            box5.Text = PageT[5];
            box6.Text = PageT[6];
            box7.Text = PageT[7];
            freeFramesTB.Text = Convert.ToString(freeFrames.Count);
        }

        public void ClearMem(string PID)
        {   
            for (int a = 0; a < PageT.Length; a++)
            {
                if (PageT[a].Contains("P"+PID))
                {
                    PageT[a] = " ";
                    freeFrames.Add(a);
                }
            }
            UpdateTable();
        }

        private static void Print(int s)
        {
            Console.WriteLine(s);
        }

        public MainWindow()
        {
            InitializeComponent();
            Next.IsEnabled = false;
            Start.IsEnabled = false;
            box0.IsReadOnly = true;
            box1.IsReadOnly = true;
            box2.IsReadOnly = true;
            box3.IsReadOnly = true;
            box4.IsReadOnly = true;
            box5.IsReadOnly = true;
            box6.IsReadOnly = true;
            box7.IsReadOnly = true;
            freeFramesTB.IsReadOnly = true;
        }

        private void LoadBTN(object sender, RoutedEventArgs e)
        {
           TextArray = System.IO.File.ReadAllLines(@"C:\Users\Public\Documents\input3a.txt");
            Start.IsEnabled = true;
        }

        private void NextStep(object sender, RoutedEventArgs e)
        {
            AllocateFrame(TextArray,step);
            UpdateTable();
            step++;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            FreshPageT();
            Next.IsEnabled = true;
            AllocateFrame(TextArray, step);
            UpdateTable();
            step++;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(1);
        }
    }
}
