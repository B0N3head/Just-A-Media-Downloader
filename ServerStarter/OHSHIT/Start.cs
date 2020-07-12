using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;


namespace MediaDL
{
    public partial class Start : Form
    {
        public static bool faceInt = false;
        public string dirToStore;
        public int myFaceAtm;
        public Random _random;
        public int a = 0;
        public int b = 0;
        public string args;
        public Start()
        {
            InitializeComponent();
            faceInt = false;
            comboBox1.SelectedItem = "mp3";
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.SelectedItem = "mp4";
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Start_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                UpdateExpresion(14);
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                UpdateExpresion(0);
            }
        }

        public void UpdateExpresion(int expresType)
        {
            myFaceAtm = expresType;
            int wink = 0;
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
            if (faceInt == false)
            {
                foreach (var str in richTextBox1.Lines) { if (str.Contains("porn")) { wink = wink + 1; } }
                if (wink == 1) { pictureBox1.BackgroundImage = Properties.Resources.eyy; }
                else if (wink >= 2 && wink <= 3) { pictureBox1.BackgroundImage = Properties.Resources.jesus; }
                else if (wink >= 4) { pictureBox1.BackgroundImage = Properties.Resources.myEyes; }
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
                        if (MessageBox.Show("Files were found in this dir. Are you sure you want to chose this dir", "You sure?", MessageBoxButtons.YesNo) == DialogResult.Yes)
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

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                UpdateExpresion(14);
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
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
                Downloads();
            }
        }
        public void Downloads()
        {

            if (string.IsNullOrEmpty(richTextBox1.Text))
            {
                MessageBox.Show("I have no links to download silly");
            }
            else
            {
                Start.panel2.Visible = true;
                Start.panel2.Enabled = true;
                Start.pictureBox4.Location = new Point(0, 31);
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
                    if (checkBox1.Checked == true) { args = "-x --audio-format " + comboBox1.Text + " --audio-quality " + trackBar1.Value + " " + str; }
                    else if (checkBox2.Checked == true)
                    {
                        if (checkBox3.Checked == true) { args = "--embed-subs --recode-video " + comboBox2.Text + " " + str; }
                        else { args = "--recode-video " + comboBox2.Text + " " + str; }
                    }
                    Download.StartDownload(args);
                }
            }
        }
        public static void fukc()
        {

        }

        public static void weBeDone()
        {
            faceInt = false;
            panel2.Visible = false;
            panel2.Enabled = false;
            pictureBox4.Location = new Point(0, 0);
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            UpdateExpresion(17);
        }

        private void checkBox1_MouseEnter(object sender, EventArgs e)
        {
            UpdateExpresion(17);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Random r = new Random();
            int randInt = r.Next(0, 20);
            switch (randInt)
            {
                case 0:
                    pictureBox1.BackgroundImage = Properties.Resources.nutral;
                    break;

            }
        }
    }
}