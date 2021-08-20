#pragma warning disable CA1416

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Win32;
using SimpleJSON;
using VDF2STR;
using static semver.CompareVersions;

class kebinImportsMongoose
{
    private static string version = "3.0.0";
    private static JSONNode jsonNode;

    private static async Task downloadFile(string link, string fileName_Extension)
    {
        HttpClientHandler handler = new HttpClientHandler();
        handler.AllowAutoRedirect = true;
        HttpClient client = new HttpClient(handler);
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.246");

        var response = await client.GetAsync(link);
        using (var fs = File.Create(fileName_Extension))
        {
            await response.Content.CopyToAsync(fs);
        }
    }

    private static async Task<string> downloadString(string link)
    {
        HttpClientHandler handler = new HttpClientHandler();
        handler.AllowAutoRedirect = true;
        HttpClient client = new HttpClient(handler);

        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.246");
        var response = await client.GetStringAsync(link);
        return response;
    }
    static void Main(string[] args)
    {
        RegistryKey HKLM = Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\Valve\\Steam");
        string steamPath = HKLM.GetValue("InstallPath").ToString();

        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.FileName = "powershell.exe";

        if (!Directory.Exists(steamPath) || !File.Exists(steamPath + @"/steam.exe"))
        {
            Console.WriteLine("Steam has not been found by kebinImportsMongoose!\nPlease contact kebin#9844 to report this issue.");
            return;
        }


        List<string> steamappsDirs = new List<string>();
        string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString().Trim();
        string appDataLocalDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).ToString().Trim();
        string kebinImportsMongooseDir = appDataLocalDir + @"/Temp/kebinImportsMongoose/";
        string kebinImportsMongooseInstallerDir = appDataLocalDir + @"/Temp/kebinImportsMongoose/Installer/";
        string kebinImportsMongooseDownloadsDir = appDataLocalDir + @"/Temp/kebinImportsMongoose/Downloads/";
        string amongUsDir = "", downloadLink = "";

        var psi = new System.Diagnostics.ProcessStartInfo
        {
            UseShellExecute = true
        };

        if (Directory.Exists(kebinImportsMongooseDir)) Directory.Delete(kebinImportsMongooseDir, true);
        Directory.CreateDirectory(kebinImportsMongooseInstallerDir);
        Directory.CreateDirectory(kebinImportsMongooseDownloadsDir);
        string bclDir = appDataLocalDir + "/Programs/bettercrewlink/";

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

        jsonNode = JSON.Parse(downloadString("https://api.github.com/repos/EEkebin/kebinImportsMongoose/releases/latest").GetAwaiter().GetResult());

        if (compareVersions(version, jsonNode["tag_name"]) == 1)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nUpdating kebinImportsMongoose");
            Console.ResetColor();
            for (int i = 0; i < jsonNode["assets"].Count; i++)
            {
                if (jsonNode["assets"][i]["browser_download_url"].ToString().Trim('\"').ToLower().EndsWith(".exe"))
                {
                    downloadLink = jsonNode["assets"][i]["browser_download_url"].ToString().Trim('\"');
                }
            }
            if (downloadLink != null && downloadLink != "")
            {
                downloadFile(downloadLink, kebinImportsMongooseInstallerDir + @"kebinImportsMongoose.exe").GetAwaiter().GetResult();
                startInfo.Arguments = @"start '" + kebinImportsMongooseInstallerDir + @".\\kebinImportsMongoose.exe'";
                process.StartInfo = startInfo;
                Thread.Sleep(1000);
                process.Start();
                Environment.Exit(-1);
            }
        }

        updatesteamappsLibraries(ref steamPath, ref steamappsDirs);

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
        for (int i = 0; i < steamappsDirs.Count; i++)
        {
            if (Directory.Exists(steamappsDirs[i] + @"/common/Among Us/"))
            {
                Directory.Delete(steamappsDirs[i] + @"/common/Among Us/", true);
            }
        }
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

        updatesteamappsLibraries(ref steamPath, ref steamappsDirs);

        for (int i = 0; i < steamappsDirs.Count; i++)
        {
            if (Directory.Exists(steamappsDirs[i] + @"/common/Among Us/")) amongUsDir = steamappsDirs[i] + @"/common/Among Us/";
        }
        if (Directory.Exists(amongUsDir))
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(amongUsDir);
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
            jsonNode = SimpleJSON.JSON.Parse(downloadString("https://api.github.com/repos/polusgg/Town-Of-Us/releases/latest").GetAwaiter().GetResult());
            for (int i = 0; i < jsonNode["assets"].Count; i++)
            {
                if (jsonNode["assets"][i]["browser_download_url"].ToString().Trim('\"').ToLower().EndsWith(".zip"))
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
            jsonNode = SimpleJSON.JSON.Parse(downloadString("https://api.github.com/repos/Town-of-Impostors/TownOfImpostors/releases/latest").GetAwaiter().GetResult());
            for (int i = 0; i < jsonNode["assets"].Count; i++)
            {
                if (jsonNode["assets"][i]["browser_download_url"].ToString().Trim('\"').ToLower().EndsWith(".zip"))
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
            jsonNode = SimpleJSON.JSON.Parse(downloadString("https://api.github.com/repos/Eisbison/TheOtherRoles/releases/latest").GetAwaiter().GetResult());
            for (int i = 0; i < jsonNode["assets"].Count; i++)
            {
                if (jsonNode["assets"][i]["browser_download_url"].ToString().Trim('\"').ToLower().EndsWith(".zip"))
                {
                    downloadLink = jsonNode["assets"][i]["browser_download_url"].ToString().Trim('\"');
                }
            }
        }
        Directory.CreateDirectory(kebinImportsMongooseDownloadsDir);
        downloadFile(downloadLink, kebinImportsMongooseDownloadsDir + @"MOD.zip").GetAwaiter().GetResult();
        ZipFile.ExtractToDirectory(kebinImportsMongooseDownloadsDir + @"MOD.zip", amongUsDir);
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
            jsonNode = JSON.Parse(downloadString("https://api.github.com/repos/OhMyGuus/BetterCrewLink/releases/latest").GetAwaiter().GetResult());
            for (int i = 0; i < jsonNode["assets"].Count; i++)
            {
                if (jsonNode["assets"][i]["browser_download_url"].ToString().Trim('\"').EndsWith(".exe"))
                {
                    downloadLink = jsonNode["assets"][i]["browser_download_url"].ToString().Trim('\"');
                }
            }
            downloadFile(downloadLink, kebinImportsMongooseDownloadsDir + @"BCL.exe").GetAwaiter().GetResult();
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

    private static void updatesteamappsLibraries(ref string steamPath, ref List<string> steamappsDirs)
    {
        steamappsDirs.Add(steamPath + @"/steamapps/");
        if (File.Exists(steamPath + @"/steamapps/libraryfolders.vdf"))
        {
            jsonNode = JSON.Parse(VDFToString.Convert(steamPath + @"/steamapps/libraryfolders.vdf"));
            for (int i = 0; i < jsonNode.Count; i++)
            {
                if (Directory.Exists(jsonNode[i].ToString().Trim('\"') + @"/steamapps/"))
                {
                    steamappsDirs.Add(jsonNode[i].ToString().Trim('\"') + @"/steamapps/");
                }
                for (int j = 0; j < jsonNode[i].Count; j++)
                {
                    if (Directory.Exists(jsonNode[i][j].ToString().Trim('\"') + @"/steamapps/"))
                    {
                        steamappsDirs.Add(jsonNode[i][j].ToString().Trim('\"') + @"/steamapps/");
                    }
                }
            }
        }
    }
}