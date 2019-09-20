using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WF_BackgroundWork
{
   
    public partial class Form1 : Form
    {
        public int Summ(int from, int to)
        {
            if (from > to)
            {
                int temp = from;
                from = to;
                to = temp;
            }
            int result = from;
            for(int i = from; i <= to; i++)
            {
                result += i;
            }
            return result;
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

            if (bckgWorker.IsBusy == false)
            {
                int from;
                if(!int.TryParse(textBox1.Text,out from))
                {
                    MessageBox.Show("Error first num is doesn't correct");
                    return;
                }
                int to;
                if (!int.TryParse(textBox2.Text, out to))
                {
                    MessageBox.Show("Error first num is doesn't correct");
                    return;
                }
                Examlple ex = new Examlple { From = from, To = to };

                //bckgWorker.RunWorkerAsync();//запускает событие doWork
                bckgWorker.RunWorkerAsync(ex);//запускает событие doWork


                btnStart.Enabled = false;
            }

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (bckgWorker.WorkerSupportsCancellation)
            {
                bckgWorker.CancelAsync();
            }
        }

        private void bckgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Examlple examlple = e.Argument as Examlple;

            e.Result = this.Summ(examlple.From, examlple.To);

            for (int i = 1; i <= 100; i++)
            {
                if (bckgWorker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                //if (i == 50)//принудительно остановили его на 50, и дальше на 100% не дойдет(((
                //{
                //    throw new Exception("Test!");
                //}
                Thread.Sleep(200);
                bckgWorker.ReportProgress(i);
            }
        }

        private void bckgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblResult.Text = $"{e.ProgressPercentage}%";
        }

        private void bckgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lblSum.Text = e.Result.ToString();
            if (e.Cancelled)
            {
                lblResult.Text = $"Operation is delete";
            }
            else if (e.Error!=null)
            {
                lblResult.Text = $"Error:{e.Error.Message}";
            }
            else
            {
                lblResult.Text = $"Every is ok";
            }
            btnStart.Enabled = true;
        }
    }
    class Examlple
    {
        public int From { get; set; }
        public int To { get; set; }

    }
}
