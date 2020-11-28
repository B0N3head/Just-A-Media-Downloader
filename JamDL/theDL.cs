using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MediaDL
{
    public partial class theDL : Form
    {
        string mystuff = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\JAMDL";
        string logpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\JAMDL\\log.txt";

        string[] ffmpegLinks = new string[] { "https://github.com/marierose147/ffmpeg_windows_exe_with_fdk_aac", "https://web.archive.org/web/20200916091820mp_/https://ffmpeg.zeranoe.com/builds/win64/shared/ffmpeg-4.3.1-win64-shared.zip" };
        string[] ytdlLinks = new string[] { "https://github.com/ytdl-org/youtube-dl", "http://abf-downloads.openmandriva.org/ytdl/youtube-dl.exe" };

        int[] linkValues = new int[] { 0, 0 };

        public Start next = new Start();

        bool randomBool = false;

        string preLogLogs = "";

        public theDL()
        {
            InitializeComponent();
            start();
        }

        void start()
        {

            try
            {
                if (Directory.Exists(mystuff))
                {
                    preLogLogs += "Directory at " + mystuff + " exists" + Environment.NewLine;
                    //Try to kill any active youtube-dl's
                    foreach (Process proc in Process.GetProcessesByName("youtube-dl")) { proc.Kill(); }

                    //Remove anything leftover in the JamDL folder
                    foreach (var sourceFilePath in Directory.GetFiles(mystuff))
                    {
                        string fileName = Path.GetFileName(sourceFilePath);
                        string destinationFilePath = Path.Combine(mystuff, fileName);
                        File.Delete(sourceFilePath);
                    }
                    /*                    string[] paths = new string[] { mystuff + "\\ffmpeg.exe", mystuff + "\\youtube-dl.exe", logpath, mystuff + "\\userInfo", };
                                        foreach (string a in paths)
                                        {
                                            if (File.Exists(a))
                                                File.Delete(a);
                                        }*/

                    LogDat(preLogLogs);

                    //Check if I can access the internet
                    if (internet() == true)
                    {
                        LogDat("The user can access the internet, specifically google");

                        string downHosts = "";
                        int hostsDown = 0;

                        LogDat("Finding online host from list");
                        for (int i = 0; i < ffmpegLinks.Length + 1 && randomBool == false; i++)
                        {
                            if (i > ffmpegLinks.Length)
                            {
                                hostsDown += 1;
                            }
                            else
                            {
                                if (internet(ffmpegLinks[i]))
                                {
                                    randomBool = true;
                                    linkValues = new int[] { i, linkValues[1] };
                                }
                                else
                                {
                                    downHosts += "FFMPEG HOST:" + ffmpegLinks[i]; // Add down host to string
                                }
                            }
                        }

                        for (int i = 0; i < ytdlLinks.Length + 1 && randomBool == false; i++)
                        {
                            if (i > ytdlLinks.Length)
                            {
                                hostsDown += 2;
                            }
                            else
                            {
                                if (internet(ytdlLinks[i]))
                                {
                                    randomBool = true;
                                    linkValues = new int[] { linkValues[0], i };
                                }
                                else
                                {
                                    downHosts += "YTDL HOST:" + ytdlLinks[i]; // Add down host to string
                                }
                            }
                        }
                        LogDat("Down hosts: " + downHosts); // Print out all colected down hosts into text file for debuging if all are down
                        LogDat("Chosen host for ffmpeg " + ffmpegLinks[linkValues[0]] + "\n" + "Chosen host for ytdl " + ytdlLinks[linkValues[1]]);

                        if (hostsDown != 0) // This number will only go over 0 if all hosts for one of the dependecies are down
                        {
                            string build = "";
                            switch (hostsDown)
                            {
                                case 1:
                                    build = "ffmpeg";
                                    break;
                                case 2:
                                    build = "ytdl";
                                    break;
                                case 3:
                                    build = "ffmpeg or ytdl";
                                    break;
                            }

                            ErrorMsg("No online hosts for " + build + " were found, is your internet on?", "Gona need more hosts I guess");
                        }

                        var webClient1 = new WebClient();
                        webClient1.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCompleted1);
                        webClient1.DownloadProgressChanged += Wc_DownloadProgressChanged1;
                        if (linkValues[0] == 0)
                            webClient1.DownloadFileAsync(new Uri(getGithubLink(ytdlLinks[linkValues[0]], "youtube-dl.exe")), mystuff + "\\youtube-dl.exe");
                        else
                            webClient1.DownloadFileAsync(new Uri(ytdlLinks[linkValues[0]]), mystuff + "\\youtube-dl.exe");
                    }
                    else
                    {
                        ErrorMsg("Is your internet on?", "I'm trying to download my dependencies");
                    }
                }
                else
                {
                    preLogLogs += "Directory at " + mystuff + " does not exist, creating it now" + Environment.NewLine;
                    Directory.CreateDirectory(mystuff);
                    start();
                }
            }
            catch (Exception e)
            {
                //Check if any are still running (if so u gona have to restart)
                Process[] pname = Process.GetProcessesByName("youtube-dl");
                if (pname.Length > 0)
                    ErrorMsg("Please close all instances of youtube-dl before installing dependencies", "the user has youtube-dl open", e.ToString());
                else
                    ErrorMsg("Unable to make directory", "Unknown reason", e.ToString());

                Application.Exit();
            }
        }

        private string getGithubLink(string ltbc, string fileWanted) //had added 32 bit downloading var but decieded against it cause i cant be bothered, who tf is still using 32 bit windows
        {
            if (!ltbc.Contains("/releases/latest"))
                ltbc += "/releases/latest";
            return WebRequest.Create(Regex.Replace(WebRequest.Create(ltbc).GetResponse().ResponseUri.ToString() + "/" + fileWanted, "tag", "download")).GetResponse().ResponseUri.ToString();
        }

        private void DownloadFileCompleted1(object sender, AsyncCompletedEventArgs e)
        {
            LogDat("Downloaded yt-dl");

            var webClient2 = new WebClient();
            webClient2.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCompleted2);
            webClient2.DownloadProgressChanged += Wc_DownloadProgressChanged2;

            textBox1.Text = "Grabbing Dependencies 2/2";
            LogDat("Starting Download of ffmpeg");

            if (linkValues[1] == 0)
                webClient2.DownloadFileAsync(new Uri(getGithubLink(ffmpegLinks[linkValues[1]], "ffmpeg-win64-gpl.exe")), mystuff + "\\ffmpeg.exe");
            else
                webClient2.DownloadFileAsync(new Uri(ffmpegLinks[linkValues[1]]), mystuff + "\\ffmpeg.zip");
        }

        public bool internet(string link = "http://www.google.com")
        {
            try
            {
                var request = WebRequest.Create(new Uri(link));
                using (var response = request.GetResponse())
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        return true;
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError &&
                    ex.Response != null)
                {
                    var resp = (HttpWebResponse)ex.Response;
                    if (resp.StatusCode == HttpStatusCode.NotFound)
                    {
                        LogDat(link + " returned a 404 error");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        // used for displaying what the program is doing, but not errors
        private void LogDat(string loginfo) { File.AppendAllText(logpath, "Time: " + DateTime.Now.ToLongTimeString() + " | Info: " + loginfo + Environment.NewLine); }

        private void DownloadFileCompleted2(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                LogDat("Finished Download of ffmpeg");
                progressBar1.Value = (82);
                if (isFileFucked("\\ffmpeg.exe") || isFileFucked("\\youtube-dl.exe"))
                    ErrorMsg("Somthing went wrong while downloading the dependencies", "Internet Issue?", "One of the files has a size 0kb after fully downloaded");

                ZipFile.ExtractToDirectory(mystuff + @"\ffmpeg.zip", mystuff);
                MessageBox.Show("Shot");
                LogDat("Moving exe's from ffmpeg\\bin to " + mystuff);

                File.Copy("ffmpeg-4.3.1-win64-shared\\bin\\ffmpeg.exe", mystuff, true);
                textBox1.Text = "Cleaning Up";
                Directory.Delete(mystuff + "\\ffmpeg\\ffmpeg-4.3.1-win64-shared", true);
                File.Delete(mystuff + @"\ffmpeg.zip");

                LogDat("Creating settings file" + mystuff);
                if (File.Exists(mystuff + "\\userInfo"))
                    File.Delete(mystuff + "\\userInfo");
                File.WriteAllText(mystuff + "\\userInfo", "y|y|y|y|noPath|y" + Environment.NewLine + "Last Edited At: " + DateTime.Now, Encoding.UTF8);
                Application.Restart();
            }
            catch (Exception ex)
            {
                ErrorMsg("Somthing went wrong when extracting dependencies", "Unkown error", ex.ToString());
            }
        }

        bool isFileFucked(string file)
        {
            FileInfo info = new FileInfo(mystuff + file);
            int size = (int)info.Length / 1024;
            if (size > 0)
                return false;
            else
                return true; //rip file
        }


        // used for error reporting
        void ErrorMsg(string Mainmsg, string reason, string exception = "")
        {
            if (!Directory.Exists(mystuff)) { Directory.CreateDirectory(mystuff); }

            if (exception.ToLower().Contains("could not find a part of the path") || exception.ToLower().Contains("being used by another process"))
            {
                MessageBox.Show("Please Close Any Logfiles Contained In The JAMDL Directory" + Environment.NewLine + "Then Press OK", " __Error.WinIOError", MessageBoxButtons.OK);
                if (Directory.Exists(mystuff))
                    start();
            }
            else
            {
                if (exception == "")
                    File.AppendAllText(logpath, "Time: " + DateTime.Now.ToLongTimeString() + Environment.NewLine + "Message: " + Mainmsg + Environment.NewLine + "ERROR: " + reason + Environment.NewLine);
                else
                    File.AppendAllText(logpath, "Time: " + DateTime.Now.ToLongTimeString() + Environment.NewLine + "Message: " + Mainmsg + Environment.NewLine + "ERROR: " + reason + Environment.NewLine + "VS Report: " + exception + Environment.NewLine);

                if (MessageBox.Show(Mainmsg + ". Bonehead plz fix" + Environment.NewLine + "Logfile created at " + logpath + Environment.NewLine + Environment.NewLine + "Could you please make an error report on github?", "Soz I just crashed", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    Process.Start("explorer.exe", mystuff);
                    Process.Start("https://github.com/B0N3head/Just-A-Media-Downloader/issues");
                }
                else
                {
                    MessageBox.Show("Sorry for crashing" + Environment.NewLine + "Please reopen the program and try again.");
                    Environment.Exit(0);
                }
            }
        }

        void Wc_DownloadProgressChanged1(object sender, DownloadProgressChangedEventArgs e) { progressBar1.Value = (e.ProgressPercentage / 3); textBox1.Text = "YT-DL: " + e.BytesReceived / 10240 + "/" + e.TotalBytesToReceive / 10240; }

        void Wc_DownloadProgressChanged2(object sender, DownloadProgressChangedEventArgs e) { progressBar1.Value = (e.ProgressPercentage / 3 + 33); textBox1.Text = "FFMpeg: " + e.BytesReceived / 10240 + "/" + e.TotalBytesToReceive / 10240; }
    }
}
