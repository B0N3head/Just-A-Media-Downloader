using getSettings;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
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
        public Start next = new Start();
        public int pbar = 0;
        string ffmpegHash = "", ytdlHash = "", ffmpegVersion = "", ffmpegVersions = "4.3.1";
        public theDL()
        {
            InitializeComponent();
            //https://bonehead.xyz/jamDL.json
            try
            {
                if(internet() == true)
                {
                    if (File.Exists(mystuff + @"\ffmpeg.exe"))
                    {
                        ErrorMsg("Uhh... thats awkward this is never ment to show", "Somehow the dependencies exist but don't", "I don't understand?");
                        Application.Restart();
                    }
                    else
                    {
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
                        LogDat("Created Directory at " + mystuff);
                        //Download Everything else

                        var webClient1 = new WebClient();
                        webClient1.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCompleted1);
                        webClient1.DownloadProgressChanged += Wc_DownloadProgressChanged1;
                        LogDat("Starting Download of yt-dl");
                        textBox1.Text = "Grabbing Dependencies 1/2";
                        //https://github.com/HandBrake/HandBrake/releases/download/1.3.3/HandBrakeCLI-1.3.3-win-x86_64.zip sneak peak
                        WebRequest request = WebRequest.Create("https://github.com/ytdl-org/youtube-dl/releases/latest");
                        WebResponse response = request.GetResponse();
                        webClient1.DownloadFileAsync(new Uri(Regex.Replace(response.ResponseUri.ToString() + "/youtube-dl.exe", "tag", "download")), mystuff + @"\youtube-dl.exe");
                    }
                }
                else
                {
                    ErrorMsg("Bruh turn ur internet on", "I'm trying to download my dependencies");
                }

            }
            catch (Exception e)
            {
                //Try to kill any youtube-dl's
                foreach (Process proc in Process.GetProcessesByName("youtube-dl"))
                {
                    proc.Kill();
                }

                //Check if any are still running (if so u gona have to restart)
                Process[] pname = Process.GetProcessesByName("youtube-dl");
                if (pname.Length > 0)
                {
                    ErrorMsg("Please close all instances of youtube-dl before installing dependencies", "the user has youtube - dl open", e.ToString());
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

            var json = new WebClient().DownloadString("https://bonehead.xyz/jamDL.json");
            var jsonOfInfo = UserSettings.FromJson(json);
            ffmpegVersions = jsonOfInfo.FfmpegVersion;

            if (Environment.Is64BitOperatingSystem)
            { 
                webClient2.DownloadFileAsync(new Uri(jsonOfInfo.Ffmpeg64), mystuff + @"\ffmpeg.zip");
                ffmpegHash = jsonOfInfo.Ffmpeg64Hash;
                ffmpegVersion = "-"+ffmpegVersions+"-win64-static";
            }
            else
            { 
                webClient2.DownloadFileAsync(new Uri(jsonOfInfo.Ffmpeg32), mystuff + @"\ffmpeg.zip");
                ffmpegHash = jsonOfInfo.Ffmpeg32Hash;
                ffmpegVersion = "-" + ffmpegVersions + "-win32-static";
            }
        }
        public static bool internet()
        {
            string url = "http://www.google.com";
            try
            {
                WebRequest myRequest = WebRequest.Create(url);
                WebResponse myResponse = myRequest.GetResponse();
            }
            catch (WebException)
            {
                return false;
            }
            return true;
        }
        private void LogDat(string loginfo)
        {
            File.AppendAllText(logpath, "Time: "+DateTime.Now.ToLongTimeString() + " | Info: " + loginfo + Environment.NewLine);
        }

        private void DownloadFileCompleted2(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                textBox1.Text = "Checking MD5 Hash's";
                if (checkIfTheyNotFkd() == true)
                {
                    LogDat("Finished Download of ffmpeg");
                    LogDat("Extracting ffmpeg");
                    textBox1.Text = "Extracting Dependencies";
                    ZipFile.ExtractToDirectory(mystuff + @"\ffmpeg.zip", mystuff);
                    progressBar1.Value = (82);
                    System.Threading.Thread.Sleep(100);
                    LogDat(@"Moving exe's from ffmpeg\bin to " + mystuff);
                    string sourcePath = mystuff + @"\ffmpeg"+ ffmpegVersion + @"\bin";

                    foreach (var sourceFilePath in Directory.GetFiles(sourcePath))
                    {
                        string fileName = Path.GetFileName(sourceFilePath);
                        string destinationFilePath = Path.Combine(mystuff, fileName);
                        File.Copy(sourceFilePath, destinationFilePath, true);
                    }

                    progressBar1.Value = (95);
                    textBox1.Text = "Cleaning Up";
                    Directory.Delete(mystuff + @"\ffmpeg" + ffmpegVersion, true);
                    File.Delete(mystuff + @"\ffmpeg.zip");
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
                    saved.info.Add(infomation);
                    File.WriteAllText(mystuff + @"\userSettings.json", jsonObject.ToString());
                    Application.Restart();
                }
                else
                {
                    ErrorMsg("Somthing is making it hard for me to download my dependencies", "I can't access the internet");
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                ErrorMsg("Somthing went wrong when extracting dependencies", "Unkown error", ex.ToString());
            }

        }

        void ErrorMsg(string Mainmsg, string reason, string exception = "")
        {
            if(exception == "")
            {
                File.AppendAllText(logpath, "Time: " + DateTime.Now.ToLongTimeString() + " | Message: " + Mainmsg + Environment.NewLine + " | ERROR: " + reason + Environment.NewLine);
            }
            else
            {
                File.AppendAllText(logpath, "Time: " + DateTime.Now.ToLongTimeString() + " | Message: " + Mainmsg + Environment.NewLine + " | ERROR: " + reason + Environment.NewLine + " | VS Report: " + exception + Environment.NewLine);
            }

            if (MessageBox.Show(Mainmsg + ". Bonehead plz fix" + Environment.NewLine + "Logfile created at " +logpath + Environment.NewLine + Environment.NewLine + "Could you please make an error report on github?", "Soz I just crashed", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                Process.Start("https://github.com/B0N3head/Just-A-Media-Downloader/issues");
            }
            else
            {
                MessageBox.Show("Sorry for crashing" + Environment.NewLine + "Feel free to reopen the program and try again");
                Environment.Exit(0);
            }
        }

        void Wc_DownloadProgressChanged1(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = (e.ProgressPercentage / 3);
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        void Wc_DownloadProgressChanged2(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = (e.ProgressPercentage / 3 + 33);
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
            LogDat("Pray that youtube-dl.exe is fine");
/*            string firstHash;*/
            string seccHash;
/*            using (FileStream fStream = File.OpenRead(mystuff + @"\youtube-dl.exe"))
            {
                LogDat("Checking hash of handbrake");
                firstHash = GetHash<MD5>(fStream);
                if (firstHash == ytdlHash)
                {
                    using (FileStream fsream = File.OpenRead(mystuff + @"\ffmpeg.zip"))
                    {
                        LogDat("Hash youtube-dl.exe is all good");
                        LogDat("Checking hash of ffmpeg.zip");
                        seccHash = GetHash<MD5>(fsream);
                        if (seccHash == ffmpegHash)
                        {
                            LogDat("Hash ffmpeg.zip is all good");
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
            }*/
            using (FileStream fsream = File.OpenRead(mystuff + @"\ffmpeg.zip"))
            {
                LogDat("Hash youtube-dl.exe is all good");
                LogDat("Checking hash of ffmpeg.zip");
                seccHash = GetHash<MD5>(fsream);
                if (seccHash == ffmpegHash.ToLower())
                {
                    LogDat("Hash ffmpeg.zip is all good");
                    return true;
                }
                else
                {
                    LogDat("Hash ffmpeg.zip is bad");
                    return false;
                }
            }
        }
    }
}
