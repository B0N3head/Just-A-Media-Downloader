using System;
using System.Diagnostics;
using System.Drawing;
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

        private void Reinstall_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("You are about to reinstall all of the dependencies" + Environment.NewLine + "There is no real point on doing this as I verify the md5 hash of every download" + Environment.NewLine + "This will also reset your settings btw", "It's a waste of time if you ask me", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + @"\JAMDL", true);
                Application.Restart();
                Environment.Exit(0);
            }
        }

        private void SourceFiles_Click(object sender, EventArgs e) { Process.Start("https://github.com/B0N3head/Just-A-Media-Downloader"); }

        private void SupportedSites_Click(object sender, EventArgs e) { Process.Start("http://ytdl-org.github.io/youtube-dl/supportedsites.html"); }

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
/*            if (File.Exists(mystuff + @"\userSettings.json"))
            {
                try
                {
                    
                }
                catch (Exception h)
                {
                    MessageBox.Show("Somthing is wrong with your user settings" + Environment.NewLine + "If you changed something you might want to check it" + Environment.NewLine + "If not, click ok and I'll reset it for you", "What is going on in here?", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    File.Delete(mystuff + @"\userSettings.json");
                    LoadSettings();
                }

            }
            else
            {
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + @"\JAMDL\userInfo", "y|y|y|y|null|y");
                LoadSettings();
            }*/
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DirLoop();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string a1 = "n", a2 = "n", a3 = "n", a4 = "n", a5 = "null", a6 = "n";
            if (checkBox4.Checked == true) { a1 = "y"; }
            if (checkBox5.Checked == true) { a2 = "y"; }
            if (checkBox6.Checked == true) { a3 = "y"; }
            if (checkBox7.Checked == true) { a4 = "y"; }
            if (defPathTB.Text != null)
                a5 = defPathTB.Text;
            else
                MessageBox.Show("The directory provided isn't valad");
            if (checkBox10.Checked == true) { a6 = "y"; }
            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + @"\JAMDL\userInfo", a1 + "|" + a2 + "|" + a3 + "|" + a4 + "|" + a5 + "|" + a6);
            button10.Text = "Settings Saved";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            getForm(MainHelpPanel);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            checkBox6.Checked = !checkBox3.Checked;
        }
    }
}
