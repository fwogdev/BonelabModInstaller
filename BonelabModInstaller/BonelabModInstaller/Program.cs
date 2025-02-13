using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using static System.Net.WebRequestMethods;

namespace BonelabModInstaller
{
    internal class Program
    {
        static void AcsiiArt()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" ______     ______     __   __     ______     __         ______     ______                    ");
            Console.WriteLine("/\\  == \\   /\\  __ \\   /\\ \"-.\\ \\   /\\  ___\\   /\\ \\       /\\  __ \\   /\\  == \\                   ");
            Console.WriteLine("\\ \\  __<   \\ \\ \\/\\ \\  \\ \\ \\-.  \\  \\ \\  __\\   \\ \\ \\____  \\ \\  __ \\  \\ \\  __<                   ");
            Console.WriteLine(" \\ \\_____\\  \\ \\_____\\  \\ \\_\\\\\"\\_\\  \\ \\_____\\  \\ \\_____\\  \\ \\_\\ \\_\\  \\ \\_____\\                 ");
            Console.WriteLine("  \\/_____/   \\/_____/   \\/_/ \\/_/   \\/_____/   \\/_____/   \\/_/\\/_/   \\/_____/                 ");
            Console.WriteLine("               made by fwoggamer.   Version: 1                                                                            ");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        static void MainMenu()
        {
            Console.Clear();
            Console.Title = "Bonelab Mod Installer";
            Console.ForegroundColor = ConsoleColor.Gray;
            AcsiiArt();
            Console.WriteLine();
            Console.WriteLine("|   Bonelab Mod Installer");
            Console.WriteLine("|   \"making your life easy.\"");
            Console.WriteLine();
            Console.WriteLine("|   [1] Install Mods");
            Console.WriteLine("|   [2] Install Code Mods");
            Console.WriteLine("|   [3] Open Mods Folder");
            Console.WriteLine("|   [4] Exit");
            Console.Write("|   Option: ");
            string option = Console.ReadLine();
            if (option != string.Empty)
            {
                if (option == "1")
                {
                    InstallModsMenu();
                } else if (option == "2")
                {
                    InstallCodeMods();
                } else if (option == "3")
                {
                    OpenBonelabMods();
                    MainMenu();
                }
                else if (option == "4")
                {
                    Environment.Exit(0);
                }
            }
            MainMenu();
        }

        static void InstallCodeMods()
        {
            Console.Clear();
            AcsiiArt();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red; Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Got BONELAB directory! Continuing...");

            if (CheckBonelabRunning())
            {
                Process[] bonelabProcess = Process.GetProcessesByName("BONELAB_Steam_Windows64");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("BONELAB is open, Do you want to force close it? (Y/N)");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("Option: ");
                string option3 = Console.ReadLine();
                if (option3 != string.Empty)
                {
                    Console.WriteLine();
                    if (option3.ToLower() == "y")
                    {
                        Console.WriteLine("Closing BONELAB...");
                        try
                        {
                            foreach (Process actualProcess in bonelabProcess)
                            {
                                actualProcess.Kill();
                            }
                        }
                        catch (Exception ex) { Console.WriteLine($"Failed to close process, reason: {ex.Message}"); }
                        Thread.Sleep(500);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Closed BONELAB, continuing...");
                    }
                    else if (option3.ToLower() == "n")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("In order to install mods, you must close BONELAB!");
                        Thread.Sleep(3000);
                        MainMenu();
                    }
                }
                else
                {
                    MainMenu();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("BONELAB isnt open, continuing...");
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("(If nothing shows up, please put your mods in this folder.)");
            Console.WriteLine();
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Put Mods In Here");
            string searchPattern = "*.dll";

            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine("Didnt find mods path!");
                Console.WriteLine("Creating mods path...");
                Directory.CreateDirectory(directoryPath);
                Thread.Sleep(2500);
                MainMenu();
            }

            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine("Didnt find mods path!");
                Console.WriteLine("Creating mods path...");
                Directory.CreateDirectory(directoryPath);
                Thread.Sleep(2500);
                MainMenu();
            }

            string[] files = Directory.GetFiles(directoryPath, searchPattern);

            foreach (string file in files)
            {
                Console.WriteLine(file);
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Are you SURE you want to install these mods? (Y/N)");
            Console.Write("Option: ");
            string option2 = Console.ReadLine();
            if (option2.ToLower() == "y")
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Installing Mods...");
            } else if (option2.ToLower() == "n")
            {
                MainMenu();
            }

            foreach (string file in files)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine();
                Console.WriteLine(System.IO.Path.GetFileName(file));
                Console.WriteLine($"Is this dll a Code Mod or Plugin? [1: Code Mod | 2: Plugin]");
                string cmop = Console.ReadLine();
                if (cmop == "1")
                {
                    try
                    {
                        string modFolder = Path.Combine(BLSteamDirectory(), "Mods");
                        string destin = Path.Combine(modFolder, Path.GetFileName(file));
                        System.IO.File.Copy(file, destin);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Installed: {file}");
                    } catch (Exception ex) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine($"Error, reason: ({ex.Message})"); Console.ForegroundColor = Console.ForegroundColor = ConsoleColor.Gray; }
                }
                else if (cmop == "2")
                {
                    try
                    {
                        string pluginFolder = Path.Combine(directoryPath, "Plugins");
                        string destin = Path.Combine(pluginFolder, Path.GetFileName(file));
                        System.IO.File.Copy(file, destin);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Installed: {file}");
                    }
                    catch (Exception ex) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine($"Error, reason: ({ex.Message})"); Console.ForegroundColor = Console.ForegroundColor = ConsoleColor.Gray; }
                }
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Finished installing Code Mods/Plugins!");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Do you want to delete the already installed mods? (Y/N)");
            Console.Write("Option: ");
            Console.ForegroundColor = ConsoleColor.Gray;
            string option4 = Console.ReadLine();
            if (option4 != string.Empty)
            {
                Console.WriteLine();
                if (option4.ToLower() == "y")
                {
                    Console.WriteLine("Deleting installed mods...");
                    try
                    {
                        foreach (string file in files)
                        {
                            if (file.Contains(".dll"))
                            {
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.WriteLine($"Deleted: {file}");
                                System.IO.File.Delete(file);
                            }
                        }

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Deleted installed mods!");
                        Thread.Sleep(3500);
                        MainMenu();
                    }
                    catch (Exception ex) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Failed to delete file(s), reason: " + ex.Message); }
                }
                else if (option4.ToLower() == "n")
                {
                    MainMenu();
                }
            }
            MainMenu();
        }

        static string BLSteamDirectory()
        {
            if (Directory.Exists("C:\\SteamLibrary\\steamapps\\common\\BONELAB"))
                return "C:\\SteamLibrary\\steamapps\\common\\BONELAB";
            else if (Directory.Exists("D:\\SteamLibrary\\steamapps\\common\\BONELAB"))
                return "D:\\SteamLibrary\\steamapps\\common\\BONELAB";
            else if (Directory.Exists("E:\\SteamLibrary\\steamapps\\common\\BONELAB"))
                return "E:\\SteamLibrary\\steamapps\\common\\BONELAB";
            else if (Directory.Exists("F:\\SteamLibrary\\steamapps\\common\\BONELAB"))
                return "F:\\SteamLibrary\\steamapps\\common\\BONELAB";
            else
                return string.Empty;
        }

        static async void OpenBonelabMods()
        {
            string directoryPath = $"C:\\Users\\{Environment.UserName}\\AppData\\LocalLow\\Stress Level Zero\\BONELAB\\Mods";
            string codemodsPath = BLSteamDirectory();
            if (Directory.Exists(directoryPath))
            {
                Process.Start(directoryPath);
                await Task.Delay(1000);
                Process.Start(codemodsPath);
            } else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to find BONELAB \"Mods\" directory, do you have Melon Loader/or BONELAB installed?");
                Thread.Sleep(4000);
                MainMenu();
            }
        }

        static bool CheckBonelabRunning()
        {
            Process[] bonelabProcess = Process.GetProcessesByName("BONELAB_Steam_Windows64");
            if (bonelabProcess.Length == 0)
                return false;
            else
                return true;
        }

        static void InstallModsMenu()
        {
            string modsDirectory = string.Empty;
            Console.Clear();
            AcsiiArt();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Checking BONELAB possible locations...");
            if (Directory.Exists($"C:\\Users\\{Environment.UserName}\\AppData\\LocalLow\\Stress Level Zero\\BONELAB\\Mods"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Found BONELAB \"Mods\" directory!");
                modsDirectory = $"C:\\Users\\{Environment.UserName}\\AppData\\LocalLow\\Stress Level Zero\\BONELAB\\Mods"; // may cause antivirus to go off :sob:
                Thread.Sleep(500);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to find BONELAB \"Mods\" directory, do you have Melon Loader/or BONELAB installed?");
                Thread.Sleep(2500);
                MainMenu();
            }

            if (CheckBonelabRunning())
            {
                Process[] bonelabProcess = Process.GetProcessesByName("BONELAB_Steam_Windows64");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("BONELAB is open, Do you want to force close it? (Y/N)");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("Option: ");
                string option3 = Console.ReadLine();
                if (option3 != string.Empty)
                {
                    Console.WriteLine();
                    if (option3.ToLower() == "y")
                    {
                        Console.WriteLine("Closing BONELAB...");
                        try
                        {
                            foreach (Process actualProcess in bonelabProcess)
                            {
                                actualProcess.Kill();
                            }
                        } catch (Exception ex) {  Console.WriteLine($"Failed to close process, reason: {ex.Message}"); }
                        Thread.Sleep(500);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Closed BONELAB, continuing...");
                    } else if (option3.ToLower() == "n")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("In order to install mods, you must close BONELAB!");
                        Thread.Sleep(3000);
                        MainMenu();
                    }
                } else
                {
                    MainMenu();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("BONELAB isnt open, continuing...");
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("(If nothing shows up, please put your mods in this folder.)");
            Console.WriteLine();
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Put Mods In Here");
            string searchPattern = "*.zip";

            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine("Didnt find mods path!");
                Console.WriteLine("Creating mods path...");
                Directory.CreateDirectory(directoryPath);
                Thread.Sleep(2500);
                MainMenu();
            }

            string[] files = Directory.GetFiles(directoryPath, searchPattern);

            foreach (string file in files)
            {
                Console.WriteLine(file);
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Are you SURE you want to install these mods? (Y/N)");
            Console.Write("Option: ");
            string option = Console.ReadLine();
            if (option.ToLower() == "y")
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Installing Mods...");

                foreach (string file in files)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"Installing: {file}");
                    string downloadPath = Path.Combine(modsDirectory, Path.GetFileName(file).Replace(".zip", ""));
                    ZipFile.ExtractToDirectory(file, downloadPath);

                    Thread.Sleep(500);
                    foreach (var fileop in new DirectoryInfo(downloadPath).GetDirectories())
                    {
                        try
                        {
                            if (!Directory.Exists(($@"{modsDirectory}\{fileop.Name}")))
                            {
                                fileop.MoveTo($@"{modsDirectory}\{fileop.Name}");
                            }
                            else
                            {
                                Console.WriteLine($@"You already have: {modsDirectory}\{fileop.Name}! Skipping...");
                            }
                        } catch(Exception ex) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Error: " + ex.Message); }
                    }
                    Directory.Delete(downloadPath, true);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Installed: {file}");
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Finished installing mods!");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Do you want to delete the already installed mods? (Y/N)");
                Console.Write("Option: ");
                Console.ForegroundColor = ConsoleColor.Gray;
                string option4 = Console.ReadLine();
                if (option4 != string.Empty)
                {
                    Console.WriteLine();
                    if (option4.ToLower() == "y")
                    {
                        Console.WriteLine("Deleting installed mods...");
                        try
                        {
                            foreach (string file in files)
                            {
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.WriteLine($"Deleted: {file}");
                                System.IO.File.Delete(file);
                            }

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Deleted installed mods!");
                            Thread.Sleep(3500);
                            MainMenu();
                        } catch (Exception ex) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Failed to delete file(s), reason: " + ex.Message); }
                    }
                    else if (option4.ToLower() == "n")
                    {
                        MainMenu();
                    }
                }
                MainMenu();
            } else if (option.ToLower() == "n")
            {
                MainMenu();
            }
        }

        static void Main(string[] args)
        {
            MainMenu();
        }
    }
}
