using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LootsCounter
{
    public class Loots
    {
        private string LootsCounterFile = "LootsCounter.txt";
        public bool LootsReset { get; set; }
        public int LootsCount { get; set; }
        public int LootsMax { get; set; }
        public string LootsMessage { get; set; }
        public string LootsText { get; set; }


        public Loots()
        {
            LootsReset = Settings.CurrentSettings.ResetCounter;
            LootsMax = Settings.CurrentSettings.ResetAtCount;
            LootsMessage = Settings.CurrentSettings.ResetMessage;
            LootsText = Settings.CurrentSettings.ScreenText;
        }

        public bool AddLootsCounter()
        {
            LootsCount++;
            WriteLootsToFile();
            Console.WriteLine($"Loots given and counted: {LootsCount}");
            if (LootsCount == LootsMax && LootsReset)
            {
                LootsCount = 0;
                return true;
            }

            return false;
        }

        public void ResetLoots()
        {
            LootsCount = 0;
            WriteLootsToFile();
        }

        public void LoadLootsCount()
        {
            try
            {
                string LootsCountText = "";
                using (StreamReader readtext = new StreamReader(LootsCounterFile))
                {
                    LootsCountText = readtext.ReadLine();
                }
                LootsCount = Convert.ToInt16(Regex.Match(LootsCountText.Split('/')[0].Trim(), @"\d+").Value);
                Console.WriteLine(LootsCount);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Something went wrong: {e.Message}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Press any key to close the application...");
                Console.ReadKey();

                Environment.Exit(1);
            }
        }

        public void WriteLootsToFile()
        {
            try
            {
                using (StreamWriter writetext = new StreamWriter(LootsCounterFile))
                {
                    writetext.WriteLine($"{Settings.CurrentSettings.ScreenText} {LootsCount} / {Settings.CurrentSettings.ResetAtCount}");
                }
            } 
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Something went wrong: {e.Message}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Press any key to close the application...");
                Console.ReadKey();

                Environment.Exit(1);
            }
            
        }

        internal void FirstLaunch()
        {
            if (File.Exists(LootsCounterFile))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Loots File Found Press Do You Want To Keep The Current LootsCount? 'Y' / 'N'");
                CheckForKey();

            } else
            {
                ResetLoots();
            }
            
        }

        private void CheckForKey()
        {
            ConsoleKeyInfo KeyRead = Console.ReadKey();

            if (KeyRead.Key == ConsoleKey.Y)
            {
                LoadLootsCount();                
                Console.WriteLine($"Loots Successfully Loaded Current Loots Is: {LootsCount}");
            }
            else if (KeyRead.Key == ConsoleKey.N)
            {
                ResetLoots();
                Console.WriteLine("Loots Susccessfully Reset!");
                
            }
            else
            {
                CheckForKey();
            }
        }
    }
}
