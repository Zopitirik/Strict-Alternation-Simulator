//Name  : Hikmet ERGÜN
//E-Mail : hikmet@hikmetergun.net
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace StrictAlternation
{
    public partial class Form1 : Form
    {
        public static int TMAX = 20;
        private bool CR_FLAG = false; //Critical Region Flag. If it is false, none of process in CR; if its is true one of process in CR
        public Queue<ThreadClass> Queue; //Queue, Threads will be queueing here.
        public int ThreadCounter; //How many threads created
        public ThreadClass TYTemp; //Temporary Variable
        public Random NumberG = new Random(); //Random Number Variable
        public Random WorkingTimeG = new Random(); //Random Number Variable

        public int NumberGenerator()
        {
            return NumberG.Next(1000, 9999);
        } //Random Thread Number Generator
        public int WorkingTimeGenerator()
        {
            return WorkingTimeG.Next(1, 10);
        } //Random Thread Number Generator
        public void EnterCR(ThreadClass T)
        {
            CR_FLAG = true;
            textBox1.AppendText("\r\nThread Enter Critical Region : " + TYTemp.ThreadNumber);
        } //Enter Critical Region
        public void ExitCR(ThreadClass T)
        {
            CR_FLAG = false;
            textBox1.AppendText("\r\nThread Exit Critical Region : " + TYTemp.ThreadNumber);
        } //Leave Critical Region
        public bool CheckCRFlag()
        {
            return CR_FLAG;
        } //Check CR Flag for any thread in critical region
        public bool CheckQueueHaveThread()
        {
            if (Queue.Count == 0) //If Queue Have No Threads, Its Count Equals Zero. 
            {
                return false;
            }
            else
            {
                return true;
            }
        } //if queue have thread it will turn true
        public void CreateThread()
        {
            try
            {
                TYTemp = new ThreadClass();
                TYTemp.ThreadNumber = NumberGenerator();
                TYTemp.WorkingTime = WorkingTimeGenerator();
                Queue.Enqueue(TYTemp);
                textBox1.AppendText("\r\nThread created. Thread Number : " + TYTemp.ThreadNumber + " Working Time : " + TYTemp.WorkingTime);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        } //Create Thread
        public void SimulationTimer_Tick(object sender, EventArgs e)
        {
            if (CheckCRFlag() == true)
            {
                textBox1.AppendText("\r\nOne of process in Critical Region...");
                if (TYTemp.WorkingTime <= 0) //If working time equals or less than zero exit from CR
                {
                    ExitCR(TYTemp);
                }
                else
                {
                    textBox1.AppendText("\r\nThread working... Number : " + TYTemp.ThreadNumber + " Working Time : " + TYTemp.WorkingTime);
                    TYTemp.WorkingTime--;
                }
            }
            else
            {
                if (CheckQueueHaveThread() == false)
                {
                    SimulationTimer.Enabled = false;
                    textBox1.AppendText("\r\nThere is no process wants to enter CR. Simulation Finished.");
                    button1.Enabled = true;
                    button2.Enabled = false;
                    button3.Enabled = false;
                    button4.Enabled = false;
                }
                else
                {
                    try
                    {
                        TYTemp = Queue.Dequeue();
                        TYTemp._Thread.Start();
                        EnterCR(TYTemp);
                        TYTemp.WorkingTime--;
                        textBox1.AppendText("\r\nThread is now working... Thread Number : " + TYTemp.ThreadNumber + " Working Time : " + TYTemp.WorkingTime);
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message);
                    }
                }
            }
        }  //Simulation

        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "" && Int32.Parse(textBox2.Text) <= 20)
            {
                Queue = new Queue<ThreadClass>(); //Creating New Queue Which Will Keep ThreadClass Data
                for (int i = 0; i < Int32.Parse(textBox2.Text); i++)
                {
                    CreateThread();
                }
                SimulationTimer.Enabled = true;
                textBox1.AppendText("\r\nSimulation Started...");
                button1.Enabled = false;
                button2.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                textBox2.Enabled = false;


            }
            else
            {
                textBox1.AppendText("\r\nYou must enter number of thread and the number must be lower than 20");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "Pause")
            {
                SimulationTimer.Enabled = false;
                button2.Text = "Resume";
            }
            else
            {
                SimulationTimer.Enabled = true;
                button2.Text = "Pause";
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            SimulationTimer.Enabled = false;
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            textBox2.Enabled = true;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            CreateThread();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
        }
    }
}
