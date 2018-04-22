using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    using Topic = System.Collections.Generic.List<string>;

    class Program
    {

        static void Main(string[] args)
        {

            //initalized paramters - should be data from client
            string malletPath = @"c:\mallet";
            string dataPath = @"sample-data\web\news";
            string resultMalletFile = "keys.mallet";
            string resultTxtFile = "keys.txt";
            string DataOfFile = "topics.txt";

            int numTopics = 10;
            List<string> flags = new List<string>();
            flags.Add("keep-sequence");
            flags.Add("remove-stopwords" );

            //begin
            IExternal cmd = new CmdWindows(malletPath);
            MalletOpr mallet = new MalletOpr(cmd);

            //run
            mallet.CreateMalletFile(dataPath, resultMalletFile,flags);
            mallet.RunTopics(numTopics, resultMalletFile, resultTxtFile, DataOfFile);
            List<Topic> getTopics = mallet.getTopics();
            List<File> files = mallet.getTopicsForFiles();

            //socket


        }
    }
}