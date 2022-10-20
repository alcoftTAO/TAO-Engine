using System;
using System.Collections.Generic;
using System.IO;

namespace TAO.Engine.LibGL
{
    public static class LibTOS
    {
        public static string GetOS()
        {
            if (OperatingSystem.IsWindows())
            {
                return "Windows";
            }
            else if (OperatingSystem.IsMacOS())
            {
                return "Mac OS";
            }
            else if (OperatingSystem.IsLinux())
            {
                return "Linux";
            }
            else if (OperatingSystem.IsFreeBSD())
            {
                return "FreeBSD";
            }

            return "Unknown";
        }

        public static void CheckFiles()
        {
            CheckLogsFiles();
            CheckAssetsFiles();
        }

        public static void CheckLogsFiles()
        {
            if (!Directory.Exists("Logs/"))
            {
                Directory.CreateDirectory("Logs/");
            }

            string date = DateTime.Now.ToShortDateString().Replace("/", "-");

            if (!File.Exists("Logs/" + date + ".txt"))
            {
                File.Create("Logs/" + date + ".txt").Close();
            }
        }

        public static void CheckAssetsFiles()
        {
            if (!Directory.Exists("Assets/"))
            {
                Directory.CreateDirectory("Assets/");
            }

            if (!Directory.Exists("Assets/Textures/"))
            {
                Directory.CreateDirectory("Assets/Textures/");
            }

            if (!Directory.Exists("Assets/Textures/Letters/"))
            {
                Directory.CreateDirectory("Assets/Textures/Letters/");
            }

            if (!Directory.Exists("Assets/Sounds/"))
            {
                Directory.CreateDirectory("Assets/Sounds/");
            }
        }

        public static void WriteOnLog(string msg)
        {
            CheckLogsFiles();
            string date = DateTime.Now.ToShortDateString().Replace("/", "-");

            File.WriteAllText("Logs/" + date + ".txt", File.ReadAllText("Logs/" + date + ".txt") + msg + "\n");
        }
    }
}
