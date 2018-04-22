using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace Server
{
    public class CmdWindows : IExternal
    {
        private string path;

        public CmdWindows(string path)
        {
            this.path = path;
        }

        public void SetPath(string path)
        {
            this.path = path;
        }

        public String GetPath()
        {
            return this.path;
        }
        public Result Run(string command)
        {

            ProcessStartInfo ProcessInfo;
            Process Process;
            ProcessInfo = new ProcessStartInfo("cmd.exe", "/K " +  command);
            ProcessInfo.WorkingDirectory = @"c:\Mallet";
            ProcessInfo.CreateNoWindow = true;
            ProcessInfo.UseShellExecute = true;
            Process = Process.Start(ProcessInfo);

            return Result.Success;
        }
    }
}
