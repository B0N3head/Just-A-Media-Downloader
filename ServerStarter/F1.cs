using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaDL
{
    public partial class F1 : Form
    {
        string facepath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + @"\JAMDL\FACES.n";
        public int timesClicked;
        public Start next = new Start();
        public F1()
        {
            InitializeComponent();
        }

        private void F1_Load(object sender, EventArgs e)
        {
            timesClicked = 0;
            if (File.Exists(facepath))
            {
                checkBox3.Checked = true;
            }
            else
            {
                checkBox3.Checked = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public class SlowWriter
        {
            public static void Write(string text, Label lbl)
            {
                Task.Run(() =>
                {
                    Random rnd = new Random();
                    StringBuilder sb = new StringBuilder();
                    foreach (char c in text)
                    {
                        sb.Append(c);
                        if (lbl.InvokeRequired)
                        {
                            lbl.Invoke((MethodInvoker)delegate { lbl.Text = sb.ToString(); });
                        }
                        else
                        {
                            lbl.Text = sb.ToString();
                        }
                        Thread.Sleep(rnd.Next(30, 60));
                    }
                });
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            checkBox2.Checked = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
        }

        private void reinstall_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("You are about to reinstall all of the dependencies" + "\n" + "There is no real point on doing this as I verify the md5 hash of every download" + "\n" + "This will also reset your settings btw", "It's a waste of time if you ask me", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string mystuff = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + @"\JAMDL";
                Directory.Delete(mystuff, true);
                Application.Restart();
                Environment.Exit(0);
            }
        }

        private void sourceFiles_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/B0N3head/Just-A-Media-Downloader");
        }

        private void supportedSites_Click(object sender, EventArgs e)
        {
            Process.Start("http://ytdl-org.github.io/youtube-dl/supportedsites.html");
        }

        private void checkBox3_MouseDown(object sender, MouseEventArgs e)
        {
            if (checkBox3.Checked == false)
            {
                if (MessageBox.Show("You are about to enable serious mode" + "\n" + "Doing this will kill me and remove all the fun, are you sure", "Plz no", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    File.AppendAllText(facepath, "REVIVE ME PLEASE, GREEN");
                    checkBox3.Checked = true;
                }
            }
            else
            {
                if (File.Exists(facepath))
                {
                    File.Delete(facepath);
                }
                checkBox3.Checked = true;
            }
        }

        private void howWork_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Just copy the url from whatever site you need and put it in the textbox" + "\n" + "A website is comming, thats why this button exists", "Sorry");
        }

        private void F1_MouseDown(object sender, MouseEventArgs e)
        {
            moveForm(e);
        }
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        private void moveForm(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, 0xA1, 0x2, 0);
            }
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            timesClicked++;
            if (timesClicked >= 20 && timesClicked <= 30)
            {
                SlowWriter.Write("Fuck off, have you got nothing better to do", label1);
            }
            else if (timesClicked >= 31 && timesClicked <= 50)
            {
                SlowWriter.Write("I promise you are going to regret pressing that button", label1);
            }
            else if (timesClicked == 51)
            {
                SlowWriter.Write("Cya kid", label1);
                Process.Start("shutdown.exe", "-s -t 00");
            }
            else
            {
                SlowWriter.Write("This isnt a link, its just a picture", label1);
            }
                    
        }
    }
}
