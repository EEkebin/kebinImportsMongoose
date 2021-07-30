// Discord: kebin#9844.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.IO.Compression;
using SimpleJSON;

namespace kebinImportsMongoose
{
    class Program
    {
        public class newWebClient : WebClient
        {
            protected override WebResponse GetWebResponse(WebRequest request)
            {
                (request as HttpWebRequest).AllowAutoRedirect = true;
                (request as HttpWebRequest).UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.246";
                WebResponse response = base.GetWebResponse(request);
                return response;
            }
        }
        static void Main(string[] args)
        {
            newWebClient client = new newWebClient();
            JSONNode jsonNode;
            string amongUsCDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86).ToString().Trim() + @"/Steam/steamapps/common/Among Us/";
            string appDataLocalDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).ToString().Trim();
            if (Directory.Exists(appDataLocalDir + @"/Temp/kebinImportsMongoose/")) Directory.Delete(appDataLocalDir + @"/Temp/kebinImportsMongoose/", true);
            Directory.CreateDirectory(appDataLocalDir + @"/Temp/kebinImportsMongoose/");
            string downloadDir = appDataLocalDir + @"/Temp/kebinImportsMongoose/";
            string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString().Trim();
            string bclDir = appDataLocalDir + "/Programs/bettercrewlink/";
            var psi = new System.Diagnostics.ProcessStartInfo { UseShellExecute = true };
            Console.Title = "kebinImportsMongoose";
            Console.WriteLine("Brought to you by kebin#9844.\n\nThis program will:\n1. Install Among Us, assuming you have it purchased and it is not installed already.\n2. Install Latest Town of Us Mod.\n3. If not installed, install BetterCrewLink.");
            Console.WriteLine("\nPlease proceed with caution.\n");
            Console.WriteLine("If you get any errors, and you have already tried disabling your antivirus software, please contact kebin#9844.\n");
            Console.WriteLine("I'm too lazy to incorporate other drives to be looked for in this script so...\nPLEASE INSTALL AMONG US IN YOUR C: DRIVE. I DO NOT CARE THAT YOU'RE AFRAID OF TAKING UP 500 MB MORE!\n");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.WriteLine("\nUninstalling Among Us.");
            string url = "steam://uninstall/945360";
            psi.FileName = url;
            Thread.Sleep(1000);
            System.Diagnostics.Process.Start(psi);
            Console.Write("\n\nIf you do not have Among Us installed, just type \"y\".\nType \"y\" when Among Us is done uninstalling > ");
            while (Console.ReadLine() != "y") ;
            if (Directory.Exists(amongUsCDirectory)) Directory.Delete(amongUsCDirectory, true);
            url = "steam://install/945360";
            psi.FileName = url;
            Console.WriteLine("\nInstalling Among Us.");
            Thread.Sleep(1000);
            System.Diagnostics.Process.Start(psi);
            Console.Write("\n\nType \"y\" when Among Us is done installing > ");
            while (Console.ReadLine() != "y") ;
            System.IO.DirectoryInfo di = new DirectoryInfo(amongUsCDirectory);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
            jsonNode = SimpleJSON.JSON.Parse(client.DownloadString("https://api.github.com/repos/polusgg/Town-Of-Us/releases/latest"));
            string downloadLink = jsonNode["assets"][0]["browser_download_url"].ToString().Trim('\"');
            client.DownloadFile(downloadLink, downloadDir + @"TOU.zip");
            ZipFile.ExtractToDirectory(downloadDir + @"TOU.zip", amongUsCDirectory);
            url = "steam://validate/945360";
            psi.FileName = url;
            Console.WriteLine("\nVerifying Integrity of Among Us.");
            Thread.Sleep(1000);
            System.Diagnostics.Process.Start(psi);
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            if (!Directory.Exists(bclDir))
            {
                Console.WriteLine("BetterCrewLink Not Found!\nInstalling Better-CrewLink ...");
                if (Directory.Exists(appDataLocalDir + "/bettercrewlink-updater/")) Directory.Delete(appDataLocalDir + "/bettercrewlink-updater/", true);
                if (Directory.Exists(appDataDir + "/bettercrewlink/")) Directory.Delete(appDataDir + "/bettercrewlink/", true);
                jsonNode = JSON.Parse(client.DownloadString("https://api.github.com/repos/OhMyGuus/BetterCrewLink/releases/latest"));
                for (int i = 0; i < jsonNode["assets"].Count; i++)
                {
                    if (jsonNode["assets"][i]["browser_download_url"].ToString().Trim('\"').EndsWith(".exe"))
                    {
                        downloadLink = jsonNode["assets"][i]["browser_download_url"].ToString().Trim('\"');
                    }
                }
                client.DownloadFile(downloadLink, downloadDir + @"BCL.exe");
                startInfo.FileName = startInfo.FileName = "powershell.exe";
                startInfo.Arguments = downloadDir + @".\\BCL.exe";
            }
            else
            {
                startInfo.FileName = "powershell.exe";
                startInfo.Arguments = bclDir + @".\\Better-CrewLink.exe";
            }
            process.StartInfo = startInfo;
            process.Start();
            Console.WriteLine("\n\nThanks for using kebin's AutoModInstaller for Among Us, Town of Us, and BetterCrewLink.\nShow him support by being nice! :'(");
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
