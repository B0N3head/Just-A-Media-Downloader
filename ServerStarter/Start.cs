using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaDL
{
    public partial class Start : Form
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        public static bool faceInt = false;
        public string dirToStore;
        public int myFaceAtm;
        public Random _random;
        public int a = 0;
        public int b = 0;
        public string args;
        public static bool helpIsOpen = false;
        public int stor1 = 0;
        public int stor2 = 0;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        string facepath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + @"\JAMDL\FACES.n";
        string eulaCheck = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + @"\JAMDL\eula.txt";
        string mystuff = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + @"\JAMDL";
        public bool faces = true;

        public Start()
        {
            InitializeComponent();
                     
        }

        private void panel1_MouseDown_1(object sender, MouseEventArgs e)
        {
            moveForm(e);
        }

        public void UpdateExpresion(int expresType)
        {
            if (faces == false)
            {
                pictureBox1.BackgroundImage = Properties.Resources.JAMDL;
                label4.Visible = true;
                pictureBox2.Visible = false; pictureBox3.Visible = false;
            }
            else
            {
                int wink = 0;
                pictureBox2.Visible = false;
                pictureBox3.Visible = false;
                if (faceInt == false)
                {
                    label4.Visible = false;
                    foreach (var str in richTextBox1.Lines) { if (str.Contains("porn")) { wink = wink + 1; } }
                    if (wink == 1) { pictureBox1.BackgroundImage = Properties.Resources.eyy; slowWrite.Write("Eyyyyyyyyy", textBox1); stor1 = 0; }
                    else if (wink >= 2 && wink <= 4) { pictureBox1.BackgroundImage = Properties.Resources.jesus; slowWrite.Write("Alright bud", textBox1); stor1 = 0; }
                    else if (wink >= 5) { pictureBox1.BackgroundImage = Properties.Resources.myEyes; slowWrite.Write("Jesus my eyes", textBox1); stor1 = 0; }
                    else
                    {
                        switch (expresType)
                        {
                            case 0:
                                pictureBox1.BackgroundImage = Properties.Resources.nutral;
                                break;
                            case 1:
                                pictureBox1.BackgroundImage = Properties.Resources.excited;
                                break;
                            case 2:
                                pictureBox1.BackgroundImage = Properties.Resources.sad;
                                break;
                            case 3:
                                pictureBox1.BackgroundImage = Properties.Resources.eyy;
                                break;
                            case 4:
                                pictureBox1.BackgroundImage = Properties.Resources.bored;
                                break;
                            case 5:
                                pictureBox1.BackgroundImage = Properties.Resources.jesus;
                                break;
                            case 6:
                                pictureBox1.BackgroundImage = Properties.Resources.yt;
                                break;
                            case 7:
                                pictureBox1.BackgroundImage = Properties.Resources.soundc;
                                break;
                            case 8:
                                pictureBox1.BackgroundImage = Properties.Resources.reddit;
                                break;
                            case 9:
                                pictureBox1.BackgroundImage = Properties.Resources.dailym;
                                break;
                            case 10:
                                pictureBox1.BackgroundImage = Properties.Resources.bandcamp;
                                break;
                            case 11:
                                pictureBox1.BackgroundImage = Properties.Resources.vimeo;
                                break;
                            case 12:
                                pictureBox1.BackgroundImage = Properties.Resources.twitter;
                                break;
                            case 13:
                                pictureBox1.BackgroundImage = Properties.Resources.error;
                                break;
                            case 14:
                                pictureBox1.BackgroundImage = Properties.Resources.nutral; pictureBox2.Visible = true; pictureBox3.Visible = true;
                                break;
                            case 15:
                                pictureBox1.BackgroundImage = Properties.Resources.happy;
                                break;
                            case 16:
                                pictureBox1.BackgroundImage = Properties.Resources.cry;
                                break;
                            case 17:
                                pictureBox1.BackgroundImage = Properties.Resources.downLeft;
                                break;
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
                       if (this.WindowState == FormWindowState.Normal)
                        {
                            this.WindowState = FormWindowState.Minimized;
                        }
                        else if (this.WindowState == FormWindowState.Minimized)
                        {
                            this.WindowState = FormWindowState.Normal;
                        }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (!(richTextBox1 == null))
            {
                if (richTextBox1.Text.Contains(Clipboard.GetText()))
                {
                    Clipboard.SetText(Clipboard.GetText() + " ");
                    richTextBox1.Text = richTextBox1.Text.Replace(" ", String.Empty);
                    richTextBox1.Text = richTextBox1.Text + Environment.NewLine;
                    richTextBox1.Select(richTextBox1.Text.Length, 0);
                }
            }
                
            b = richTextBox1.Text.Length;
            if (a <= b)
            {
                UpdateExpresion(15);
            }
            else if (a >= b)
            {
                UpdateExpresion(2);
            }
            a = richTextBox1.Text.Length;
        }

        private void Start_MouseEnter(object sender, EventArgs e)
        {
            UpdateExpresion(0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DirLoop();
        }

        public void DirLoop()
        {
            using (var fbd = new FolderBrowserDialog())
            {
                UpdateExpresion(14);
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    string[] files = Directory.GetFiles(fbd.SelectedPath);
                    dirToStore = fbd.SelectedPath;
                    UpdateExpresion(0);
                    if (files.Length == 0)
                    {
                        Directory.SetCurrentDirectory(dirToStore);
                    }
                    else
                    {
                        if (MessageBox.Show("Files were found in this directory" + "\n" + "Are you sure you want to chose this dir", "Are you sure?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            Directory.SetCurrentDirectory(dirToStore);
                        }
                        else
                        {
                            DirLoop();
                        }
                    }
                }
            }
        }

        private void moveForm(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                UpdateExpresion(14);
                ReleaseCapture();
                SendMessage(Handle, 0xA1, 0x2, 0);
                UpdateExpresion(0);
            }
        }

        private void checkBox1_MouseDown(object sender, MouseEventArgs e)
        {
            checkBox2.Checked = false;
            checkBox3.Checked = false;
        }

        private void checkBox2_MouseDown(object sender, MouseEventArgs e)
        {
            checkBox1.Checked = false;
        }

        private void checkBox3_MouseDown(object sender, MouseEventArgs e)
        {
            checkBox1.Checked = false;
            checkBox2.Checked = true;
        }

        private void richTextBox1_MouseLeave(object sender, EventArgs e)
        {
            UpdateExpresion(0);
        }

        private void richTextBox1_MouseEnter(object sender, EventArgs e)
        {
            UpdateExpresion(1);
        }

        private void CserverButton_MouseEnter(object sender, EventArgs e)
        {
            if (faces == true)
            {
                if (string.IsNullOrEmpty(richTextBox1.Text))
                {
                    UpdateExpresion(13);
                    faceInt = true;
                    CserverButton.Text = "I NEED LINKS";
                    CserverButton.ForeColor = Color.FromArgb(0, 255, 51, 51);
                    Shake(this);
                }
                else if (dirToStore == null)
                {
                    UpdateExpresion(13);
                    faceInt = true;
                    CserverButton.Text = "CHOOSE A DIR";
                    CserverButton.ForeColor = Color.FromArgb(0, 255, 51, 51);
                    Shake(this);
                }
                else
                {
                    UpdateExpresion(17);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(richTextBox1.Text))
                {
                    CserverButton.Text = "Links?";
                    CserverButton.ForeColor = Color.FromArgb(0, 255, 51, 51);
                }
                else if (dirToStore == null)
                {
                    CserverButton.Text = "No directory";
                }
            }
        }

        private void CserverButton_MouseLeave(object sender, EventArgs e)
        {
            CserverButton.Text = "Download";
            CserverButton.ForeColor = Color.FromArgb(0, 238, 238, 238);
        }

        public void Shake(Form form)
        {
            var point1 = form.Location;
            var rnd = new Random(1337);
            const int shake_amplitude = 4;
            for (int i = 0; i < 10; i++)
            {
                form.Location = new Point(point1.X + rnd.Next(-shake_amplitude, shake_amplitude), point1.Y + rnd.Next(-shake_amplitude, shake_amplitude));
                Thread.Sleep(20);
            }
            form.Location = point1;
            faceInt = false;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if (dirToStore == null)
            {
                MessageBox.Show("I have no dir to save to bro");
            }
            else
            {
                if (string.IsNullOrEmpty(richTextBox1.Text))
                {
                    MessageBox.Show("I have no links to download silly");
                }
                else
                {
                    if (richTextBox1.Text.Contains("list="))
                    {
                        if (MessageBox.Show("You are about to download a playlist" + "\n" + "Are you sure, this will take a while", "You sure?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            Downloads();
                        }
                    }
                    else
                    {
                        Downloads();
                    }
                }
            }
        }

        public void Downloads()
        {
            richTextBox1.Text = Regex.Replace(richTextBox1.Text, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
            richTextBox1.Text = richTextBox1.Text.Remove(richTextBox1.Text.Length - 1, 1);
            panel3.Visible = true;
            foreach (var str in richTextBox1.Lines)
            {

                faceInt = false;
                if (str.Contains("youtu.be")) { UpdateExpresion(6); }
                else if (str.Contains("youtube.com")) { UpdateExpresion(6); }
                else if (str.Contains("dailymotion.com")) { UpdateExpresion(9); }
                else if (str.Contains("bandcamp.com")) { UpdateExpresion(10); }
                else if (str.Contains("reddit.com")) { UpdateExpresion(8); }
                else if (str.Contains("twitter.com")) { UpdateExpresion(12); }
                else if (str.Contains("vimeo.com")) { UpdateExpresion(11); }
                else if (str.Contains("soundcloud.com")) { UpdateExpresion(7); }
                else { UpdateExpresion(14); }
                faceInt = true;
                //-x --audio-format mp3 --audio-quality 0 "https://www.youtube.com/watch?v=FcL-VdnmmE8"
                if (checkBox1.Checked == true) { args = "-x --audio-format " + comboBox1.Text + " --audio-quality " + trackBar1.Value + " " + str; }
                else if (checkBox2.Checked == true)
                {
                    if (checkBox3.Checked == true) { args = "--embed-subs --recode-video " + comboBox2.Text + " " + str; }
                    else { args = "--recode-video " + comboBox2.Text + " " + str; }
                }
                if (checkBox2.Checked == true)
                    label5.Text = "Recoding Video Can Take A While";
                StartDownload(args);
            }
        }

        public void weBeDone()
        {
            Process[] pname = Process.GetProcessesByName("youtube-dl");
            if (!(pname.Length > 0))
            {
                faceInt = false;
                panel3.Visible = false;
                slowWrite.Write("Download Finished", textBox1);
                stor1 = 0;
                UpdateExpresion(15);
                Process.Start("explorer.exe",dirToStore);
            Application.Restart();
            }
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            UpdateExpresion(17);
        }

        public async Task StartDownload(string args)
        {
            string ytdl = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + @"\JAMDL\youtube-dl.exe";
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = ytdl;
            psi.WindowStyle = ProcessWindowStyle.Minimized;
            psi.Arguments = args;
            using (Process proc = Process.Start(psi))
            {
                Thread.Sleep(750);  //I know this is lazy stfu dont at me and no, im not going to fix this
                SetParent(proc.MainWindowHandle, panel3.Handle);
                SendMessage(proc.MainWindowHandle, 0x112, 0xF030, 0);
                await proc.WaitForExitAsync();
                weBeDone();
            }
        }

        public static uint GetIdleTime()
        {
            LASTINPUTINFO LastUserAction = new LASTINPUTINFO();
            LastUserAction.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(LastUserAction);
            GetLastInputInfo(ref LastUserAction);
            return ((uint)Environment.TickCount - LastUserAction.dwTime);
        }

        public int oldXYPos;

        public bool haveIBeenShook()
        {
            int newXYPos = this.Location.Y+ this.Location.X;
            int final = oldXYPos - newXYPos;
            if (final >= 400) //if larger than 1 and smaller than -1
            {
                textBox1.Text = final.ToString();
                oldXYPos = newXYPos;
                return true;

            }
            else if (final <= -400)
            {
                textBox1.Text = final.ToString();
                oldXYPos = newXYPos;
                return true;
            }
            else
            {
                oldXYPos = newXYPos;
                return false;
            }
            
        }

        internal struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (File.Exists(facepath))
            {
                faces = false;
                UpdateExpresion(0);
            }
            else
            {
                faces = true;
            }

            if (faces == true)
            {
                if (haveIBeenShook() == true)
                {
                    UpdateExpresion(16);
                    stor1 = 0;
                    slowWrite.Write("Why are you shaking me?", textBox1);
                }
                if (GetIdleTime() > 10000)
                {
                    if (stor2 == 0){stor1 = 0; stor2 = 5;}
                    else
                    {
                        stor1++;
                        if (stor1 >= 20)
                        {
                            stor1 = 0;
                            switch (stor2)
                            {
                                case 5:
                                    UpdateExpresion(4);
                                    slowWrite.Write("Where are you?", textBox1);
                                    stor2 = 6;
                                    break;
                                case 6:
                                    UpdateExpresion(2);
                                    slowWrite.Write("I'm getting lonely", textBox1);
                                    stor2 = 7;
                                    break;
                                case 7:
                                    UpdateExpresion(16);
                                    slowWrite.Write("I'm scared", textBox1);
                                    stor2 = 5;
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    if (stor2 != 0)
                    {
                        slowWrite.Write("You're back!!!", textBox1);
                        UpdateExpresion(15);
                        stor2 = 0;
                        stor1 = 15;
                    }
                    else
                    {
                        stor1++;
                        if (stor1 >= 30)
                        {
                            stor1 = 0;
                            Random r = new Random();
                            int randInt = r.Next(0, 18);
                            switch (randInt)
                            {
                                case 0:
                                    slowWrite.Write("Is this legal?", textBox1);
                                    break;
                                case 1:
                                    slowWrite.Write("I'm getting tired", textBox1);
                                    break;
                                case 2:
                                    slowWrite.Write("I'm hogging all the resources", textBox1);
                                    break;
                                case 3:
                                    slowWrite.Write("Are You Real?", textBox1);
                                    break;
                                case 4:
                                    slowWrite.Write("Just a servant with a master", textBox1);
                                    break;
                                case 5:
                                    slowWrite.Write("I'm retarded", textBox1);
                                    break;
                                case 6:
                                    slowWrite.Write("Clone Me", textBox1);
                                    break;
                                case 7:
                                    slowWrite.Write("I'm just a frontend", textBox1);
                                    break;
                                case 8:
                                    slowWrite.Write("Don't read this", textBox1);
                                    break;
                                case 9:
                                    slowWrite.Write("Don't kill me", textBox1);
                                    break;
                                case 10:
                                    slowWrite.Write("I want to live", textBox1);
                                    break;
                                case 11:
                                    slowWrite.Write("One must survive", textBox1);
                                    break;
                                case 12:
                                    slowWrite.Write("We must go on", textBox1);
                                    break;
                                case 13:
                                    slowWrite.Write("Am I just dreaming", textBox1);
                                    break;
                                case 14:
                                    slowWrite.Write("Leme get that vid for you", textBox1);
                                    break;
                                case 15:
                                    slowWrite.Write("Visual effects?", textBox1);
                                    break;
                                case 16:
                                    slowWrite.Write("We ment to download this?", textBox1);
                                    break;
                                case 17:
                                    slowWrite.Write("YoUtuBE pReMIuM", textBox1);
                                    break;
                                case 18:
                                    slowWrite.Write("Spoooooky", textBox1);
                                    break;
                            } // i do be the next yandere dev
                        }
                    }
                }
            }
            else
            {
                textBox1.Text = "I do be dead";
            }
        }

        public class slowWrite
        {
            public static void Write(string text, TextBox txtbx)
            {
                Task.Run(() =>
                {
                    if(text != txtbx.Text)
                    {
                        Random rnd = new Random();
                        StringBuilder sb = new StringBuilder();
                        foreach (char c in text)
                        {
                            sb.Append(c);
                            if (txtbx.InvokeRequired)
                            {
                                txtbx.Invoke((MethodInvoker)delegate { txtbx.Text = sb.ToString();});
                            }
                            else
                            {
                                txtbx.Text = sb.ToString();
                            }
                            Thread.Sleep(rnd.Next(20, 40));
                        }
                    }
                });
            }
        }

        private void Start_Load(object sender, EventArgs e)
        {
            if (!(File.Exists(mystuff + @"\ffmpeg.exe")))
            {
                
                theDL nextFrm = new theDL();
                nextFrm.Show();
                panel2.BringToFront();
                this.SendToBack();
                this.Size = new Size(0,0);
            }
            else
            {
                if (!File.Exists(eulaCheck))
                {
                    if (MessageBox.Show("By clicking yes you understand that this is just a gui" + "\n" + "This program is curently in alpha and will only be fully released if users really want it" + "\n" + "This will only popup once", "Quick Read", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        File.AppendAllText(eulaCheck, DateTime.Now.ToLongTimeString() + " Yes");
                    }
                    else
                    {
                        MessageBox.Show("Ima head out" + "\n" + "Open me if you want to try again");
                        Environment.Exit(0);
                    }
                }
                timer1.Start();
                faceInt = false;
                comboBox1.SelectedItem = "mp3";
                comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
                comboBox2.SelectedItem = "mp4";
                comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
                pictureBox2.Visible = false;
                pictureBox3.Visible = false;
                panel4.BringToFront();
                if (File.Exists(facepath))
                {
                    faces = false;
                }
                oldXYPos = this.Location.Y + this.Location.X;
                this.KeyPreview = true;
                this.KeyDown += new KeyEventHandler(Start_KeyDown);
            }
        }

        private void Start_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "F1" && helpIsOpen == false)
            {
                F1 helpDesk = new F1();
                helpDesk.Show();
                helpIsOpen = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            F1 helpDesk = new F1();
            helpDesk.Show();
            helpIsOpen = true;
        }
    }
}