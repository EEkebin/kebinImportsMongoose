﻿// Discord: kebin#9844.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System.Threading;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
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

        // A VERY primitive and single use-case method of converting a Valve Data File type string to JSON type string.
        // NOT recommended for use anywhere else. Made painstaingly by kebin#9844.
        static string VDFToString(string dir)
        {
            string[] allLines = File.ReadAllLines(dir);
            allLines = allLines.Skip(1).ToArray();
            string code = "";
            for (int i = 0; i < allLines.Length; i++) code += allLines[i] + "\n";
            while (code.Contains("\"\t\t")) code = code.Replace("\"\t\t", "\": ");
            while (code.Contains("\"\n")) code = code.Replace("\"\n", "\",\n");
            Regex pattern = new Regex("\t\"[\\d]+\",");
            MatchCollection matches = pattern.Matches(code);
            for(int i = 0; i < matches.Count; i++){
                int indexOfMatch = matches.ElementAt(i).Index;
                int lengthOfMatch = matches.ElementAt(i).Length;
                int nextChar = indexOfMatch + lengthOfMatch - 1;
                code = new StringBuilder(code) {[nextChar] = ':'}.ToString();
            }
            return code;
        }

        static void Main(string[] args)
        {
            newWebClient client = new newWebClient();
            JSONNode jsonNode;
            string csteamappsDir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86).ToString().Trim() + @"/Steam/steamapps/";
            string amongUsDirectory = "";
            if (File.Exists(csteamappsDir + "libraryfolders.vdf"))
            {
                string code = VDFToString(csteamappsDir + "libraryfolders.vdf");
                jsonNode = JSON.Parse(code);
                for(int i = 1; i < jsonNode.Count; i++){
                    string gamesDirs = jsonNode[i]["path"];
                    if(Directory.Exists(gamesDirs + @"/steamapps/common/Among Us")){
                        amongUsDirectory = gamesDirs + @"/steamapps/common/Among Us";
                    }
                }
                if (amongUsDirectory == "") amongUsDirectory = csteamappsDir + @"/common/Among Us/";

            }
            string appDataLocalDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).ToString().Trim();
            if (Directory.Exists(appDataLocalDir + @"/Temp/kebinImportsMongoose/")) Directory.Delete(appDataLocalDir + @"/Temp/kebinImportsMongoose/", true);
            Directory.CreateDirectory(appDataLocalDir + @"/Temp/kebinImportsMongoose/");
            string downloadDir = appDataLocalDir + @"/Temp/kebinImportsMongoose/";
            string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString().Trim();
            string bclDir = appDataLocalDir + "/Programs/bettercrewlink/";
            string downloadLink = "";
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                UseShellExecute = true
            };
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
            if (Directory.Exists(amongUsDirectory)) Directory.Delete(amongUsDirectory, true);
            url = "steam://install/945360";
            psi.FileName = url;
            Console.WriteLine("\nInstalling Among Us.");
            Thread.Sleep(1000);
            System.Diagnostics.Process.Start(psi);
            Console.Write("\n\nType \"y\" when Among Us is done installing > ");
            while (Console.ReadLine() != "y") ;
            System.IO.DirectoryInfo di = new DirectoryInfo(amongUsDirectory);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
            Console.Write("\nWhich mod would you like to install?\n----------------------------\n1.Town Of Us\n2.Town Of Imposters\n\nPlease input the corresponding number with the mod you would like to install. > ");
            string selectedModString = "";
            Int32 selectedMod = 0;
            do
            {
                do
                {
                    selectedModString = Console.ReadLine();
                } while (Int32.TryParse(selectedModString, out selectedMod) == false);
            } while (selectedMod != 1 && selectedMod != 2);
            if (selectedMod == 1)
            {
                jsonNode = SimpleJSON.JSON.Parse(client.DownloadString("https://api.github.com/repos/polusgg/Town-Of-Us/releases/latest"));
                for (int i = 0; i < jsonNode["assets"].Count; i++)
                {
                    if (jsonNode["assets"][i]["browser_download_url"].ToString().Trim('\"').EndsWith(".zip"))
                    {
                        downloadLink = jsonNode["assets"][i]["browser_download_url"].ToString().Trim('\"');
                    }
                }
            }
            else if (selectedMod == 2)
            {
                jsonNode = SimpleJSON.JSON.Parse(client.DownloadString("https://api.github.com/repos/Town-of-Impostors/TownOfImpostors/releases/latest"));
                for (int i = 0; i < jsonNode["assets"].Count; i++)
                {
                    if (jsonNode["assets"][i]["browser_download_url"].ToString().Trim('\"').EndsWith(".zip"))
                    {
                        downloadLink = jsonNode["assets"][i]["browser_download_url"].ToString().Trim('\"');
                    }
                }
            }
            client.DownloadFile(downloadLink, downloadDir + @"MOD.zip");
            ZipFile.ExtractToDirectory(downloadDir + @"MOD.zip", amongUsDirectory);
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