using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MediaDL
{
    public partial class theDL : Form
    {
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        string mystuff = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + @"\JAMDL";
        string logpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + @"\JAMDL\log.txt";

        string ytdl = "http://abf-downloads.openmandriva.org/ytdl/youtube-dl.exe";
        string ytdlVersion = "https://yt-dl.org/downloads/latest/youtube-dl.exe";
        string ffmpeg = "https://github.com/marierose147/ffmpeg_windows_exe_with_fdk_aac/releases/latest";

        public Start next = new Start();

        public theDL()
        {
            InitializeComponent();
            try
            {
                //Try to kill any active youtube-dl's
                foreach (Process proc in Process.GetProcessesByName("youtube-dl"))
                {
                    proc.Kill();
                }

                if (internet() == true)
                {
                    if (File.Exists(mystuff + @"\ffmpeg.exe"))
                    {
                        ErrorMsg("Uhh... thats awkward this is never ment to show", "Somehow the dependencies exist but don't, let me try again", "I don't understand?");
                        Application.Restart();
                    }
                    else
                    {
                        if (Directory.Exists(mystuff)) { Directory.Delete(mystuff, true); DirectoryInfo di = Directory.CreateDirectory(mystuff); } else { DirectoryInfo di = Directory.CreateDirectory(mystuff); }

                        File.Delete(logpath);
                        LogDat("Created Directory at " + mystuff);

                        var webClient1 = new WebClient();
                        webClient1.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCompleted1);
                        webClient1.DownloadProgressChanged += Wc_DownloadProgressChanged1;

                        LogDat("Starting Download of yt-dl");
                        textBox1.Text = "Grabbing Dependencies 1/2";
                        if (!internet(ytdl))
                            ErrorMsg("The host for youtube-dl is currently down." + Environment.NewLine + "Please check back later", "Can not reach yt-dl services");
                        if (!internet(ffmpeg))
                            ErrorMsg("The host for ffmpeg is currently down." + Environment.NewLine + "Please check back later", "Can not reach ffmpeg services");

                        webClient1.DownloadFileAsync(new Uri(ytdl), mystuff + @"\youtube-dl.exe");
                    }
                }
                else
                {
                    ErrorMsg("Is your internet on?", "I'm trying to download my dependencies");
                }

            }
            catch (Exception e)
            {
                //Check if any are still running (if so u gona have to restart)
                Process[] pname = Process.GetProcessesByName("youtube-dl");
                if (pname.Length > 0)
                {
                    ErrorMsg("Please close all instances of youtube-dl before installing dependencies", "the user has youtube-dl open", e.ToString());
                }
                else
                {
                    ErrorMsg("Unable to make directory", "Unknown reason", e.ToString());
                }

                Application.Exit();
            }
        }

        private void DownloadFileCompleted1(object sender, AsyncCompletedEventArgs e)
        {
            LogDat("Downloaded yt-dl");
            var webClient2 = new WebClient();
            webClient2.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCompleted2);
            webClient2.DownloadProgressChanged += Wc_DownloadProgressChanged2;
            textBox1.Text = "Grabbing Dependencies 2/2";
            LogDat("Starting Download of ffmpeg");
            
            WebResponse response = WebRequest.Create(ffmpeg).GetResponse();

            LogDat("Is this a valid link?: " + Regex.Replace(response.ResponseUri.ToString() + "/ffmpeg-win64-gpl.exe", "tag", "download"));
            if (Environment.Is64BitOperatingSystem)
            {
                webClient2.DownloadFileAsync(new Uri(Regex.Replace(response.ResponseUri.ToString() + "/ffmpeg-win64-gpl.exe", "tag", "download")), mystuff + @"\ffmpeg.exe");

            }
            else
            {
                webClient2.DownloadFileAsync(new Uri(Regex.Replace(response.ResponseUri.ToString() + "/ffmpeg-win32-gpl.exe", "tag", "download")), mystuff + @"\ffmpeg.exe");
            }
        }

        public static bool internet(string link = "http://www.google.com") { try { WebResponse myResponse = WebRequest.Create(link).GetResponse(); } catch (WebException) { return false; } return true; }

        // used for displaying what the program is doing, but not errors
        private void LogDat(string loginfo) { File.AppendAllText(logpath, "Time: " + DateTime.Now.ToLongTimeString() + " | Info: " + loginfo + Environment.NewLine); }

        private void DownloadFileCompleted2(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                LogDat("Finished Download of ffmpeg");
                progressBar1.Value = (82);
                System.Threading.Thread.Sleep(100);
                LogDat("Creating settings file" + mystuff);
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + @"\JAMDL\userInfo", "y|y|y|y|null|y" + Environment.NewLine + "Created at: " + DateTime.Now, Encoding.UTF8);
                Application.Restart();
            }
            catch (Exception ex)
            {
                ErrorMsg("Somthing went wrong when extracting dependencies", "Unkown error", ex.ToString());
            }
        }

        // used for error reporting
        void ErrorMsg(string Mainmsg, string reason, string exception = "")
        {
            if(exception.ToLowerInvariant().Contains("find me"))
            {
                //specific error responces, eg file is currently being used will tell you to cose da crap
            }

            if (exception == "")
            {
                File.AppendAllText(logpath, "Time: " + DateTime.Now.ToLongTimeString() + Environment.NewLine + "Message: " + Mainmsg + Environment.NewLine + "ERROR: " + reason + Environment.NewLine);
            }
            else
            {
                File.AppendAllText(logpath, "Time: " + DateTime.Now.ToLongTimeString() + Environment.NewLine + "Message: " + Mainmsg + Environment.NewLine + "ERROR: " + reason + Environment.NewLine + "VS Report: " + exception + Environment.NewLine);
            }

            if (MessageBox.Show(Mainmsg + ". Bonehead plz fix" + Environment.NewLine + "Logfile created at " + logpath + Environment.NewLine + Environment.NewLine + "Could you please make an error report on github?", "Soz I just crashed", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                Process.Start("explorer.exe", mystuff);
                Process.Start("https://github.com/B0N3head/Just-A-Media-Downloader/issues");
            }
            else
            {
                MessageBox.Show("Sorry for crashing" + Environment.NewLine + "Plz reopen the program and try again.");
                Environment.Exit(0);
            }
        }

        void Wc_DownloadProgressChanged1(object sender, DownloadProgressChangedEventArgs e){progressBar1.Value = (e.ProgressPercentage / 3);}

        void Wc_DownloadProgressChanged2(object sender, DownloadProgressChangedEventArgs e){progressBar1.Value = (e.ProgressPercentage / 3 + 33);}
    }
}
