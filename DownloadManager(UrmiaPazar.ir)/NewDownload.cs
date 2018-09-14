using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.Threading;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace DownloadManager_UrmiaPazar.ir_
{
    public partial class NewDownload : Form
    {
        WebClient webcl = new WebClient();
        DateTime Dtime = new DateTime();
        Double recived;
        Threading tred = new Threading();
        public NewDownload()
        {
            InitializeComponent();
        }

        private void NewDownload_Load(object sender, EventArgs e)
        {
            try
            {
                if (Clipboard.ContainsText())
                {
                    Uri txt = new Uri(Clipboard.GetText());
                    if (txt.Scheme == Uri.UriSchemeHttps || txt.Scheme == Uri.UriSchemeHttp)
                    {
                        txtUrladress.Text = Clipboard.GetText();
                    }
                }
            }
            catch 
            {

               
            }
           
        }
        private void DownloadFile()
        {
            tred.StartDownload();
            Dtime = DateTime.Now;
           
            try
            {
                webcl.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadComplete);
                webcl.DownloadProgressChanged += new DownloadProgressChangedEventHandler(progressSet);
                if (txtUrladress.TextLength == 0 || txtxSavePath.TextLength == 0) MessageBox.Show("Check Fields!");
                else
                {
                    Uri getUri = new Uri(txtUrladress.Text);
                    webcl.DownloadFileAsync(getUri, txtxSavePath.Text);
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Error");
            }
            
        }

        private void DownloadComplete(object sender,AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Download Complete!","Attention");
            txtUrladress.Text = "";
            txtxSavePath.Text = "";
            tred.EndDownload();
        }
        private void progressSet(object sender,DownloadProgressChangedEventArgs e)
        {
            
            
            recived = 0;
            int percent = (int)(((double)(progressBar1.Value-progressBar1.Minimum) / (double)(progressBar1.Maximum-progressBar1.Minimum))*100);
            lblpercent.Text = percent.ToString() + " %";
          //  progressBar1.Value = int.Parse(Math.Truncate(percent).ToString());
            progressBar1.Value = e.ProgressPercentage;

            double byteset = double.Parse(e.BytesReceived.ToString());
            double totalbyte = double.Parse(e.BytesReceived.ToString());
            double getSize = byteset / totalbyte * 100;


            lblvolum.Text = Math.Round((decimal)(e.TotalBytesToReceive / 1024),1).ToString()+ " Size KB/s";
            lblRemain.Text = Math.Round((decimal)((e.TotalBytesToReceive - e.BytesReceived) / 1024),1).ToString()+" Remain KB/s";
            lblDownloaded.Text = Math.Round((decimal)(e.BytesReceived /1024),1).ToString()+" Downloaded KB/s";

            DateTime temp = DateTime.Now;
            TimeSpan Tspan = temp - Dtime;
            recived = e.TotalBytesToReceive - recived;
            double speed = Math.Round((recived / 1024) / Tspan.Milliseconds, 1);
            if (speed > 0)
            {
                lblSpeed.Text = speed.ToString();
            }
            Dtime = temp;


        }
       

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                using (var clint = new WebClient())
                using (clint.OpenRead("https://google.com"))
                {
                    DownloadFile();
                }
            }
            catch
            {

                MessageBox.Show("Check Your Connection!","Warning",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
           

           
        }

        private void btnSelectPath_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Title = "Select Directory";
            savefile.FileName = Path.GetFileName(txtUrladress.Text);
            savefile.AddExtension = true;
            savefile.ShowDialog();
            txtxSavePath.Text = savefile.FileName;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            webcl.CancelAsync();
            webcl.Dispose();
            tred.EndDownload();
            this.Close();
            
        }
    }
}
