// Discord: kebin#9844.

using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Threading;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using SimpleJSON;
using nWeb;

class kebinImportsMongoose
{
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
        for (int i = 0; i < matches.Count; i++)
        {
            int indexOfMatch = matches.ElementAt(i).Index;
            int lengthOfMatch = matches.ElementAt(i).Length;
            int nextChar = indexOfMatch + lengthOfMatch - 1;
            code = new StringBuilder(code) { [nextChar] = ':' }.ToString();
        }
        return code;
    }

    static void Main(string[] args)
    {
        Console.Title = "kebinImportsMongoose";
        Console.Clear();
        newWebClient client = new newWebClient();
        JSONNode jsonNode;
        string csteamappsDir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86).ToString().Trim() + @"/Steam/steamapps/";
        string amongUsDirectory = "";
        if (File.Exists(csteamappsDir + "libraryfolders.vdf") || !Directory.Exists(amongUsDirectory))
        {
            string code = VDFToString(csteamappsDir + "libraryfolders.vdf");
            jsonNode = JSON.Parse(code);
            for (int i = 1; i < jsonNode.Count; i++)
            {
                string gamesDirs = jsonNode[i]["path"];
                if (Directory.Exists(gamesDirs + @"/steamapps/common/Among Us/"))
                {
                    amongUsDirectory = gamesDirs + @"/steamapps/common/Among Us/";
                }
            }
            if (amongUsDirectory == ""  || !Directory.Exists(amongUsDirectory)) amongUsDirectory = csteamappsDir + @"/common/Among Us/";

        }
        string appDataLocalDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).ToString().Trim();
        string kebinImportsMongooseDownloadsDir = appDataLocalDir + @"/Temp/kebinImportsMongoose/Downloads/";
        if (Directory.Exists(kebinImportsMongooseDownloadsDir)) Directory.Delete(kebinImportsMongooseDownloadsDir, true);
        string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString().Trim();
        string bclDir = appDataLocalDir + "/Programs/bettercrewlink/";
        string downloadLink = "";
        var psi = new System.Diagnostics.ProcessStartInfo
        {
            UseShellExecute = true
        };
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("\nBrought to you by kebin#9844.");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n\nThis program will:\n-------------------");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("1. Install Among Us, assuming you have it purchased and it is not installed already.\n2. Install Latest Town Of Us, Town Of Imposters, or The Other Roles mod.\n3. If not installed, install BetterCrewLink.");
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("\nIf you get any errors, and you have already tried disabling your antivirus software, please contact kebin#9844.\n\n");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("PLEASE FOLLOW THE INSTRUCTIONS!\n\n");
        Console.ResetColor();
        Console.WriteLine("Press any key to continue ...");
        Console.ReadKey();
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nUninstalling Among Us.");
        Console.ResetColor();
        string url = "steam://uninstall/945360";
        psi.FileName = url;
        Thread.Sleep(1000);
        System.Diagnostics.Process.Start(psi);
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("\n\nType \"y\" when Among Us is completely uninstalled> ");
        Console.ResetColor();
        while (Console.ReadLine().ToLower() != "y") ;
        if (Directory.Exists(amongUsDirectory)) Directory.Delete(amongUsDirectory, true);
        url = "steam://install/945360";
        psi.FileName = url;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n\nInstalling Among Us.");
        Console.ResetColor();
        Thread.Sleep(1000);
        System.Diagnostics.Process.Start(psi);
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("\n\nType \"y\" when Among Us is done installing > ");
        Console.ResetColor();
        while (Console.ReadLine().ToLower() != "y") ;
        if (File.Exists(csteamappsDir + "libraryfolders.vdf") || !Directory.Exists(amongUsDirectory))
        {
            string code = VDFToString(csteamappsDir + "libraryfolders.vdf");
            jsonNode = JSON.Parse(code);
            for (int i = 1; i < jsonNode.Count; i++)
            {
                string gamesDirs = jsonNode[i]["path"];
                if (Directory.Exists(gamesDirs + @"/steamapps/common/Among Us/"))
                {
                    amongUsDirectory = gamesDirs + @"/steamapps/common/Among Us/";
                }
            }
            if (amongUsDirectory == ""  || !Directory.Exists(amongUsDirectory)) amongUsDirectory = csteamappsDir + @"/common/Among Us/";

        }
        if (Directory.Exists(amongUsDirectory))
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(amongUsDirectory);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("The Among Us installation was not found on any disk that Steam can see.\n");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Perhaps you skipped the installation step. Please follow the instructions.");
            Console.ResetColor();
            Thread.Sleep(3000);
            Environment.Exit(1);
        }
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n\nWhich mod would you like to install?\n-------------------------------------");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("1. Town Of Us\n2. Town Of Imposters\n3. The Other Roles\n");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("Please input the corresponding number with the mod you would like to install. > ");
        Console.ResetColor();
        string selectedModString = "";
        Int32 selectedMod = 0;
        do
        {
            do
            {
                selectedModString = Console.ReadLine();
            } while (Int32.TryParse(selectedModString, out selectedMod) == false);
        } while (selectedMod < 1 || selectedMod > 3);
        if (selectedMod == 1)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\nInstalling Town Of Us.");
            Console.ResetColor();
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
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\nInstalling Town Of Imposters.");
            Console.ResetColor();
            jsonNode = SimpleJSON.JSON.Parse(client.DownloadString("https://api.github.com/repos/Town-of-Impostors/TownOfImpostors/releases/latest"));
            for (int i = 0; i < jsonNode["assets"].Count; i++)
            {
                if (jsonNode["assets"][i]["browser_download_url"].ToString().Trim('\"').EndsWith(".zip"))
                {
                    downloadLink = jsonNode["assets"][i]["browser_download_url"].ToString().Trim('\"');
                }
            }
        }
        else if (selectedMod == 3)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\nInstalling The Other Roles.");
            Console.ResetColor();
            jsonNode = SimpleJSON.JSON.Parse(client.DownloadString("https://api.github.com/repos/Eisbison/TheOtherRoles/releases/latest"));
            for (int i = 0; i < jsonNode["assets"].Count; i++)
            {
                if (jsonNode["assets"][i]["browser_download_url"].ToString().Trim('\"').EndsWith(".zip"))
                {
                    downloadLink = jsonNode["assets"][i]["browser_download_url"].ToString().Trim('\"');
                }
            }
        }
        Directory.CreateDirectory(kebinImportsMongooseDownloadsDir);
        client.DownloadFile(downloadLink, kebinImportsMongooseDownloadsDir + @"MOD.zip");
        ZipFile.ExtractToDirectory(kebinImportsMongooseDownloadsDir + @"MOD.zip", amongUsDirectory);
        Thread.Sleep(2000);
        url = "steam://validate/945360";
        psi.FileName = url;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n\nVerifying Integrity of Among Us.");
        Console.ResetColor();
        Thread.Sleep(1000);
        System.Diagnostics.Process.Start(psi);
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        if (!Directory.Exists(bclDir))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Better-CrewLink Not Found!");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Installing and Launching Better-CrewLink ...");
            Console.ResetColor();
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
            client.DownloadFile(downloadLink, kebinImportsMongooseDownloadsDir + @"BCL.exe");
            startInfo.FileName = startInfo.FileName = "powershell.exe";
            startInfo.Arguments = kebinImportsMongooseDownloadsDir + @".\\BCL.exe";
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Launching Better-CrewLink.");
            Console.ResetColor();
            startInfo.FileName = "powershell.exe";
            startInfo.Arguments = bclDir + @".\\Better-CrewLink.exe";
        }
        process.StartInfo = startInfo;
        process.Start();
        Thread.Sleep(3000);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n\nThanks for using kebin's AutoModInstaller for Among Us, Town Of Us or Town Of Imposters, and BetterCrewLink.\nShow him support by being nice! :'(");
        Console.ResetColor();
        Console.WriteLine("\nPress any key to exit ...");
        Console.ReadKey();
    }
}