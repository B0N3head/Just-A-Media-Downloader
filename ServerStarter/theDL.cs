using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace MediaDL
{
    public partial class theDL : Form
    {
        string mystuff = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + @"\JAMDL";
        string logpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + @"\JAMDL\log.txt";
        public Start next = new Start();
        public int pbar = 0;

        public theDL()
        {
            InitializeComponent();
            
            try
            {
                if (File.Exists(mystuff+ @"\ffmpeg.exe"))
                {
                    MessageBox.Show("Uhh... thats awkward this is never ment to show");
                    Application.Restart();
                }
                else
                {
                    this.BringToFront();
                    System.Threading.Thread.Sleep(500);
                    this.Focus();
                    if (Directory.Exists(mystuff))
                    {
                        Directory.Delete(mystuff, true);
                        DirectoryInfo di = Directory.CreateDirectory(mystuff);
                    }
                    else
                    {
                        DirectoryInfo di = Directory.CreateDirectory(mystuff);
                    }
                    File.Delete(logpath);
                    logDatShit("Created Directory at " + mystuff);
                    //Download Everything else
                    var webClient1 = new WebClient();
                    webClient1.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCompleted1);
                    webClient1.DownloadProgressChanged += wc_DownloadProgressChanged1;
                    logDatShit("Starting Download of yt-dl");
                    textBox1.Text = "Grabbing Dependencies 1/2";
                    webClient1.DownloadFileAsync(new Uri("https://github.com/ytdl-org/youtube-dl/releases/download/2020.06.16.1/youtube-dl.exe"), mystuff+ @"\youtube-dl.exe");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to make directory, Bonehead plz fix");
                logDatShit("Unable to create directory at " + mystuff +" Error: "+ e.ToString());
                Application.Exit();
            }
        }

        private void DownloadFileCompleted1(object sender, AsyncCompletedEventArgs e)
        {
            logDatShit("Downloaded yt-dl");
            var webClient2 = new WebClient();
            webClient2.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCompleted2);
            webClient2.DownloadProgressChanged += wc_DownloadProgressChanged2;
            textBox1.Text = "Grabbing Dependencies 2/2";
            logDatShit("Starting Download of ffmpeg");

            if (Environment.Is64BitOperatingSystem)
            {webClient2.DownloadFileAsync(new Uri("https://ffmpeg.zeranoe.com/builds/win64/static/ffmpeg-4.2.3-win64-static.zip"), mystuff + @"\ffmpeg-4.2.3-win64-static.zip");}
            else
            {webClient2.DownloadFileAsync(new Uri("https://ffmpeg.zeranoe.com/builds/win64/static/ffmpeg-4.2.3-win32-static.zip"), mystuff + @"\ffmpeg-4.2.2-win64-static.zip");}
        }
        private void logDatShit(string loginfo)
        {
            File.AppendAllText(logpath, DateTime.Now.ToLongTimeString() + " | " + loginfo + Environment.NewLine);
        }

        private void DownloadFileCompleted2(object sender, AsyncCompletedEventArgs e)
        {
            textBox1.Text = "Checking MD5 Hash's";
            if (checkIfTheyNotFkd() == true)
            {
                logDatShit("Finished Download of ffmpeg");
                logDatShit("Extracting ffmpeg");
                textBox1.Text = "Extracting Dependencies";
                ZipFile.ExtractToDirectory(mystuff + @"\ffmpeg-4.2.3-win64-static.zip", mystuff);
                progressBar1.Value = (82);
                System.Threading.Thread.Sleep(100);
                logDatShit(@"Moving exe's from ffmpeg\bin to " + mystuff);
                string sourcePath = mystuff + @"\ffmpeg-4.2.3-win64-static\bin";

                foreach (var sourceFilePath in Directory.GetFiles(sourcePath))
                {
                    string fileName = Path.GetFileName(sourceFilePath);
                    string destinationFilePath = Path.Combine(mystuff, fileName);
                    File.Copy(sourceFilePath, destinationFilePath, true);
                }
                progressBar1.Value = (95);
                textBox1.Text = "Cleaning Up";
                Directory.Delete(mystuff + @"\ffmpeg-4.2.3-win64-static", true);
                File.Delete(mystuff + @"\ffmpeg-4.2.3-win64-static.zip");
                Application.Restart();
            }
            else
            {
                MessageBox.Show("Somthing is making it hard for me to download my dependencies" + "\n" + @"Check da logs at %appdata%\JAMDL\logs.txt", "Fat Error");
                Directory.Delete(mystuff, true);
                Application.Exit();
            }
        }

        public void Setup_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        void wc_DownloadProgressChanged1(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = (e.ProgressPercentage/3);
        }
        void wc_DownloadProgressChanged2(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = (e.ProgressPercentage/3 + 33);
        }

        public static String GetHash<T>(Stream stream) where T : HashAlgorithm
        {
            StringBuilder sb = new StringBuilder();

            MethodInfo create = typeof(T).GetMethod("Create", new Type[] { });
            using (T crypt = (T)create.Invoke(null, null))  
            {
                byte[] hashBytes = crypt.ComputeHash(stream);
                foreach (byte bt in hashBytes)
                {
                    sb.Append(bt.ToString("x2"));
                }
            }
            return sb.ToString();
        }



        private bool checkIfTheyNotFkd()
        {
            string firstHash;
            string seccHash;
            using (FileStream fStream = File.OpenRead(mystuff + @"\youtube-dl.exe"))
            {
                logDatShit("Checking hash of youtube-dl.exe");
                firstHash = GetHash<MD5>(fStream);
                if (firstHash == "6cc01266a585e60b41f876117c45a50c")
                {
                    using (FileStream fsream = File.OpenRead(mystuff + @"\ffmpeg-4.2.3-win64-static.zip"))
                    {
                        logDatShit("Hash youtube-dl.exe is all good");
                        logDatShit("Checking hash of ffmpeg-4.2.3-win64-static.zip");
                        seccHash = GetHash<MD5>(fsream);
                        if (seccHash == "a4f8ab988b6995d15e4789ee4e9f4c9f")
                        {
                            logDatShit("Hash ffmpeg-4.2.3-win64-static.zip is all good");
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
