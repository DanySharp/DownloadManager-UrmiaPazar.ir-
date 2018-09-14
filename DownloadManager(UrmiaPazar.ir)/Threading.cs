using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;

namespace DownloadManager_UrmiaPazar.ir_
{
    class Threading
    {
        private static Thread Mythread;
        public void StartDownload()
        {
            Mythread = new Thread(new ThreadStart(DownloadForm));
            Mythread.Start();
        }

        public void DownloadForm()
        {
            NewDownload Frmshow = new NewDownload();
           // Frmshow.Show();

        }

        public void EndDownload()
        {
            Mythread.Abort();
            Mythread = null;
        }
    }
}
