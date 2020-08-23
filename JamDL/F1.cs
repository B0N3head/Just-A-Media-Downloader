using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using getUserSettings;

namespace MediaDL
{
    public partial class F1 : Form
    {
        string mystuff = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + @"\JAMDL";
        string facepath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + @"\JAMDL\FACES";
        string dirToStore;

        public int timesClicked;
        public Start next = new Start();
        public F1()
        {
            InitializeComponent();
            getForm(MainHelpPanel);
        }

        private void MainsettingsPanel_MouseDown(object sender, MouseEventArgs e) { MoveForm(e); }
        private void panel6_MouseDown(object sender, MouseEventArgs e) { MoveForm(e); }
        private void label2_MouseDown(object sender, MouseEventArgs e) { MoveForm(e); }
        private void MainHelpPanel_MouseDown(object sender, MouseEventArgs e) { MoveForm(e); }

        private void F1_Load(object sender, EventArgs e)
        {
            LoadSettings();
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
/*
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
        }*/

        private void Reinstall_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("You are about to reinstall all of the dependencies" + Environment.NewLine + "There is no real point on doing this as I verify the md5 hash of every download" + Environment.NewLine + "This will also reset your settings btw", "It's a waste of time if you ask me", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + @"\JAMDL", true);
                Application.Restart();
                Environment.Exit(0);
            }
        }

        private void SourceFiles_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/B0N3head/Just-A-Media-Downloader");
        }

        private void SupportedSites_Click(object sender, EventArgs e)
        {
            Process.Start("http://ytdl-org.github.io/youtube-dl/supportedsites.html");
        }

        private void checkBox1_MouseDown(object sender, MouseEventArgs e)
        {
            getForm(MainsettingsPanel);
        }

        private void checkBox2_MouseDown(object sender, MouseEventArgs e)
        {
            getForm(MainsettingsPanel);
        }

        void getForm(Panel paniedls)
        {
            paniedls.Location = new Point(90, 0);
            paniedls.BringToFront();
            paniedls.Visible = true;
        }

        private void checkBox3_MouseDown(object sender, MouseEventArgs e)
        {
            getForm(MainsettingsPanel);
        }

        private void HowWork_Click(object sender, EventArgs e) { Process.Start("https://bonehead.xyz/jamdl.html"); }

        private void F1_MouseDown(object sender, MouseEventArgs e)
        {
            MoveForm(e);
        }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void MoveForm(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, 0xA1, 0x2, 0);
            }
        }

        /*        private void pictureBox1_Click_1(object sender, EventArgs e)
                {
                    timesClicked++;
                    if (timesClicked >= 20 && timesClicked <= 30)
                    {
                        SlowWriter.Write("Fuck off, have yoy got nothing better to do", label1);
                    }
                    else if (timesClicked >= 31 && timesClicked <= 50)
                    {
                        SlowWriter.Write("I promise you are going to regret pressing that button", label1);
                    }
                    else if (timesClicked == 51)
                    {
                        SlowWriter.Write("Cya arround kid", label1);
                        Process.Start("shutdown.exe", "-s -t 50");
                    }
                    else
                    {
                        SlowWriter.Write("This isnt a link, its just a picture", label1);
                    }

                }*/

        // kinda shit if a program shutdown your pc cause u clicked a button

        public void DirLoop()
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    string[] files = Directory.GetFiles(fbd.SelectedPath);
                    dirToStore = fbd.SelectedPath;
                    if (files.Length == 0)
                    {
                        Directory.SetCurrentDirectory(dirToStore);
                        defPathTB.Text = dirToStore;
                    }
                    else
                    {
                        if (MessageBox.Show("Files were found in this directory" + Environment.NewLine + "Are you sure you want to chose this dir", "Are you sure?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            Directory.SetCurrentDirectory(dirToStore);
                            defPathTB.Text = dirToStore;
                        }
                        else
                        {
                            DirLoop();
                        }
                    }
                }
            }
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            getForm(MainsettingsPanel);
        }

        private void LoadSettings()
        {
            if (File.Exists(mystuff + @"\userSettings.json"))
            {
                try
                {
                    string r = File.ReadAllText(mystuff + @"\userSettings.json");
                    var jsonOfInfo = UserSettings.FromJson(r);
                    foreach (var response in jsonOfInfo.Info)
                    {
                        defPathTB.Text = null;
                        checkBox4.Checked = false;
                        checkBox5.Checked = false;
                        checkBox6.Checked = false;
                        checkBox7.Checked = false;
                        if (response.JamdlAutoU == "y")
                            checkBox4.Checked = true;
                        if (response.DepAutoU == "y")
                            checkBox5.Checked = true;
                        if (response.Faces == "y")
                            checkBox6.Checked = true;
                        if (response.FinnishedFolderDialog == "y")
                            checkBox7.Checked = true;
                        if (response.DefultDlPath != null)
                            defPathTB.Text = response.DefultDlPath;

                    }
                }
                catch (Exception h)
                {
                    MessageBox.Show("Somthing is wrong with your user settings" + Environment.NewLine + "If you changed something you might want to check it" + Environment.NewLine + "If not, click ok and I'll fix it for you", "What is going on in here?", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    File.Delete(mystuff + @"\userSettings.json");
                    LoadSettings();
                }

            }
            else
            {
                var jsonObject = new JObject
                {
                    { "Created", DateTime.Now }
                };

                dynamic saved = jsonObject;

                saved.info = new JArray() as dynamic;

                dynamic infomation = new JObject();
                infomation = new JObject();
                infomation.jamdlAutoU = "y";
                infomation.depAutoU = "y";
                infomation.faces = "n";
                infomation.finnishedFolderDialog = "y";
                infomation.DefultDlPath = "";
                saved.emulators.Add(infomation);
                File.WriteAllText(mystuff + @"\userSettings.json", jsonObject.ToString());
                LoadSettings();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DirLoop();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(defPathTB.Text))
            {
                File.Delete(mystuff + "checkDir");
                var jsonObject = new JObject
                {
                    { "Created", DateTime.Now }
                };

                dynamic saved = jsonObject;

                saved.info = new JArray() as dynamic;

                dynamic infomationa = new JObject();
                infomationa = new JObject();
                infomationa.jamdlAutoU = "n";
                infomationa.depAutoU = "n";
                infomationa.faces = "n";
                infomationa.finnishedFolderDialog = "n";
                infomationa.DefultDlPath = " ";
                if (checkBox4.Checked == true) { infomationa.jamdlAutoU = "y"; }
                if (checkBox5.Checked == true) { infomationa.depAutoU = "y"; }
                if (checkBox6.Checked == true) { infomationa.faces = "y"; }
                if (checkBox7.Checked == true) { infomationa.finnishedFolderDialog = "y"; }
                if (defPathTB.Text != null) { infomationa.DefultDlPath = defPathTB.Text; }

                saved.info.Add(infomationa);
                File.WriteAllText(mystuff + @"\userSettings.json", jsonObject.ToString());
                button10.Text = "Settings saved";
                File.Create(mystuff + "checkDir");
            }
            else
            {
                button10.Text = "That dir don't work";
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            getForm(MainHelpPanel);
        }

    }
}
