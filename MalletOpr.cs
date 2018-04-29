using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    using System.IO;

    public class MalletOpr
    {
        private IExternal ext;
        private string outputFile;


        public MalletOpr(IExternal cmd)
        {
            ext = cmd;
        }


        public Result CreateMalletFile(string pathOfCurpus,string resultFile,List<string> keys)
        {
            string flag = GetFlags(keys);
            //   string command =String.Format(@"""bin\mallet import-dir --input {0} --output {1} --keep-sequence --remove-stopwords""",pathOfCurpus,resultFile);

            string command =String.Format(@"""bin\mallet import-dir --input {0} --output {1} {2}""",pathOfCurpus,resultFile,flag);

            Result result = ext.Run(command);
            return result;
        }

        private string GetFlags(List<string> keys)
        {
            string flag = "";
            int n = keys.Count;
            int i = 0;
            while(i<n)
            {
                flag += "--" + keys[i];
                flag += " ";
                i++;
            }
            return flag;
        }

        public  List<Topic> getTopics()
        {
            string path = this.ext.GetPath() + @"\" + this.outputFile;
            string[] lines = System.IO.File.ReadAllLines(path);
            List<Topic> topics = new List<Topic>();
            int i = 1; 
            foreach (string line in lines)
            {
                Topic topic = new Topic(i);
                string[] data = line.Split('\t');
                string[] keys = data[2].Split(' ');

                for (int j=0; j<keys.Length; j++)
                {
                    topic.AddKey(keys[j]);
                }

                topics.Add(topic);
                i++;
            }
            return topics;
        }


        public List<File> getTopicsForFiles()
        {
            string path = this.ext.GetPath() + @"\" + "topics.txt";
            string[] lines = System.IO.File.ReadAllLines(path);
            List<File> topics = new List<File>();
            File file;

            foreach (string line in lines)
            {
               file =  getTopicForFile(line);
               topics.Add(file);
            }
            return topics;
        }

    
        public Result RunTopics(int numTopic,string inputFile, string resultPath, string resultPath2)
        {
            this.outputFile = resultPath;
            string command = String.Format( @"""bin\mallet train-topics  --input {0} --num-topics {1} --output-state topic-state.gz --output-topic-keys {2} --output-doc-topics {3}""", inputFile,numTopic.ToString(),resultPath,resultPath2); ;
            Result result = ext.Run(command);
            return result;
        }


        private File getTopicForFile(string data)
        {
            string[] mData = data.Split('\t');
            List<int> numTopics = new List<int>();
            File file = new File(mData[1]);

            for (int i = 2; i < mData.Length; i++)
            {
                float num = float.Parse(mData[i]);
                if (num > 0.25)
                {
                    file.addTopic(i-1);
                }

            }

            return file;

        }

    }

    }
