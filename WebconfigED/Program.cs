using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace WebconfigED
{
    class Program
    {
        static void Main(string[] args)
        {


        Start:
            string configfile = "";
            if (args == null || args.Length == 0)
            {
                Console.Write("Path of Config File :");
                configfile = Console.ReadLine();
            }
            else
            {
                configfile = args[0];
                Console.WriteLine("Path of Config File: " + configfile);
            }
            bool pathvalid = false;
            do
            {
                pathvalid = File.Exists(configfile);
                if (!pathvalid)
                {
                    Console.WriteLine("File Not Found");
                }
            } while (!pathvalid);

            string filename = Path.GetFileName(configfile);
            string readfile = configfile;
            bool isAppConfig = false;
             
            if (Path.GetFileName(filename).ToUpper() != "WEB.CONFIG")
            {
                isAppConfig = true;
                File.Copy(configfile, Path.Combine(Path.GetDirectoryName(configfile), filename +"_"+ DateTime.Now.ToString("yyyyMMddHHmmss")));
                File.Delete(Path.Combine(Path.GetDirectoryName(configfile), "web.config"));
                File.Move(configfile, Path.Combine(Path.GetDirectoryName(configfile), "web.config"));
                readfile = Path.Combine(Path.GetDirectoryName(configfile), "web.config");
                
            }
            
            List<string> lstnodes = new List<string>();
            XmlDocument xmlDoc = new XmlDocument();
            using (var fs = new FileStream(readfile, FileMode.Open))
            {
                try
                {
                    xmlDoc.Load(fs);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Not a Valid XML");
                    Console.WriteLine(ex.StackTrace);
                    goto Start;
                }

                Console.WriteLine();
                foreach (XmlNode xmlNode in xmlDoc.DocumentElement)
                {
                    lstnodes.Add(xmlNode.Name);
                    Console.WriteLine(lstnodes.Count.ToString() + ". " + xmlNode.Name);
                }

                fs.Close();
                Console.WriteLine();
            }


        SelectSection:
            string Y = "";
            string endc = "";
            foreach (var item in lstnodes)
            {
                Console.Write(string.Format("Press 'E' to Encrypt 'D' to Decrypt Section ({0}) or Any Key to skip Section: ", item));
                Y = Console.ReadLine().Trim().ToUpper();
                if (Y == "E" || Y == "D")
                {
                    endc = item;
                    Console.WriteLine();
                    break;
                }

            }

            if (endc == "")
            {
                Console.WriteLine();
                Console.Write("Press 'R' Select Again :");
                string R = Console.ReadLine().Trim().ToUpper();
                if (R == "R")
                {
                    goto SelectSection;
                }

            }

            if (endc != "")
            {
                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = Path.Combine(System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory(), "aspnet_regiis.exe");
                start.Arguments = String.Format(" {2} \"{0}\" \"{1}\"", endc, Path.GetDirectoryName(configfile), Y == "E" ? "-pef" : "-pdf");
                Console.WriteLine("{0}{1}", start.FileName, start.Arguments);
                start.UseShellExecute = false;
                start.RedirectStandardOutput = true;

                using (Process process = Process.Start(start))
                {

                    using (StreamReader reader = process.StandardOutput)
                    {
                        string result = reader.ReadToEnd();
                        Console.Write(result);
                    }
                }

                if (isAppConfig)
                {
                    File.Move(Path.Combine(Path.GetDirectoryName(configfile), "web.config"), configfile);
                }

                Console.Write("Press 'S' Start Again or Any Key to Quit:");
                string S = Console.ReadLine().Trim().ToUpper();
                if (S == "S")
                {
                    Console.WriteLine();
                    goto Start;
                }

            }
        }
    }
}
