// Discord: kebin#9844.

using System;
using System.IO;
using System.Threading;
using System.IO.Compression;
using SimpleJSON;
using nWeb;
using VDF2STR;
using LNK2Path;
using static semver.CompareVersions;

class kebinImportsMongoose
{
    private static string version = "2.1.0";
    static void Main(string[] args)
    {
        Console.Title = "kebinImportsMongoose";
        Console.Clear();
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
        newWebClient client = new newWebClient();
        JSONNode jsonNode;
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.FileName = "powershell.exe";
        string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString().Trim();
        string appDataLocalDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).ToString().Trim();
        string kebinImportsMongooseDir = appDataLocalDir + @"/Temp/kebinImportsMongoose/";
        string kebinImportsMongooseInstallerDir = appDataLocalDir + @"/Temp/kebinImportsMongoose/Installer/";
        if (Directory.Exists(kebinImportsMongooseDir)) Directory.Delete(kebinImportsMongooseDir, true);
        Directory.CreateDirectory(kebinImportsMongooseInstallerDir);
        jsonNode = SimpleJSON.JSON.Parse(client.DownloadString("https://api.github.com/repos/EEkebin/kebinImportsMongoose/releases/latest"));
        if (compareVersions(version, jsonNode["tag_name"]) == 1)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nUpdating kebinImportsMongoose");
            Console.ResetColor();
            client.DownloadFile(@"https://github.com/EEkebin/kebinImportsMongoose/releases/latest/download/kebinImportsMongoose.exe", kebinImportsMongooseInstallerDir + @"kebinImportsMongoose.exe");
            startInfo.Arguments = @"start '" + kebinImportsMongooseInstallerDir + @".\\kebinImportsMongoose.exe'";
            process.StartInfo = startInfo;
            Thread.Sleep(1000);
            process.Start();
            Environment.Exit(-1);
        }
        string startUpFolder1 = Environment.GetFolderPath(Environment.SpecialFolder.Programs) + @"/Steam/";
        string startUpFolder2 = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"/Steam/";
        string amongUsDirectory = "", steamDir = "", steamappsDir = "";
        if ((Directory.Exists(startUpFolder1) && !Directory.Exists(startUpFolder2)) || (Directory.Exists(startUpFolder1) && Directory.Exists(startUpFolder2)))
        {
            if (File.Exists(startUpFolder1 + @"/Steam.lnk"))
            {
                steamDir = LNK2PATH.GetShortcutTarget(startUpFolder1 + @"/Steam.lnk").Replace("steam.exe", "");
            }
            else if (steamDir == "" && File.Exists(startUpFolder2 + @"/Steam.lnk"))
            {
                steamDir = LNK2PATH.GetShortcutTarget(startUpFolder2 + @"/Steam.lnk").Replace("steam.exe", "");
            }
        }
        else if ((!Directory.Exists(startUpFolder1) && Directory.Exists(startUpFolder2)) && steamDir == "")
        {
            if (File.Exists(startUpFolder2 + @"/Steam.lnk"))
            {
                steamDir = LNK2PATH.GetShortcutTarget(startUpFolder2 + @"/Steam.lnk").Replace("steam.exe", "");
            }
        }
        if (steamDir == "" || !File.Exists(steamDir + @"/steam.exe"))
        {
            Console.WriteLine("Steam has not been found by kebinImports. Either it is not installed or this is a bug. Please let kebin#9844 know.");
            Console.ReadKey();
            Environment.Exit(2);
        }
        steamappsDir = steamDir + @"/steamapps/";
        if (File.Exists(steamappsDir + "libraryfolders.vdf") || !Directory.Exists(amongUsDirectory))
        {
            string code = VDFToString.Convert(steamappsDir + "libraryfolders.vdf");
            jsonNode = JSON.Parse(code);
            for (int i = 1; i < jsonNode.Count; i++)
            {
                string gamesDirs = jsonNode[i]["path"];
                if (Directory.Exists(gamesDirs + @"/steamapps/common/Among Us/"))
                {
                    amongUsDirectory = gamesDirs + @"/steamapps/common/Among Us/";
                }
            }
            if (amongUsDirectory == "" || !Directory.Exists(amongUsDirectory)) amongUsDirectory = steamappsDir + @"/common/Among Us/";

        }
        string kebinImportsMongooseDownloadsDir = appDataLocalDir + @"/Temp/kebinImportsMongoose/Downloads/";
        if (Directory.Exists(kebinImportsMongooseDownloadsDir)) Directory.Delete(kebinImportsMongooseDownloadsDir, true);
        string bclDir = appDataLocalDir + "/Programs/bettercrewlink/";
        string downloadLink = "";
        var psi = new System.Diagnostics.ProcessStartInfo
        {
            UseShellExecute = true
        };
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
        Thread.Sleep(5000);
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("\n\nType \"y\" when Among Us is done installing > ");
        Console.ResetColor();
        while (Console.ReadLine().ToLower() != "y") ;
        if (File.Exists(steamappsDir + "libraryfolders.vdf") || !Directory.Exists(amongUsDirectory))
        {
            string code = VDFToString.Convert(steamappsDir + "libraryfolders.vdf");
            jsonNode = JSON.Parse(code);
            for (int i = 1; i < jsonNode.Count; i++)
            {
                string gamesDirs = jsonNode[i]["path"];
                if (Directory.Exists(gamesDirs + @"/steamapps/common/Among Us/"))
                {
                    amongUsDirectory = gamesDirs + @"/steamapps/common/Among Us/";
                }
            }
            if (amongUsDirectory == "" || !Directory.Exists(amongUsDirectory)) amongUsDirectory = steamappsDir + @"/common/Among Us/";

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
        Thread.Sleep(2000);
        System.Diagnostics.Process.Start(psi);
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
            startInfo.Arguments = @"start '" + kebinImportsMongooseDownloadsDir + @".\\BCL.exe'";
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Launching Better-CrewLink.");
            Console.ResetColor();
            startInfo.Arguments = @"start '" + bclDir + @".\\Better-CrewLink.exe'";
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