using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace Server
{
    class Program
    {

        public static int WriteToClient(byte[] bytes, Socket sender)
        {
            // Connect the socket to the remote endpoint. Catch any errors.  
            try
            { 
                int bytesSent = sender.Send(bytes);
                return bytesSent;
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                return 0;

            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
                return 0;

            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
                return 0;

            }

        }

        static void Main(string[] args)
        {

            // Establish the remote endpoint for the socket.  
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888);
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sock.Bind(ipep);
            sock.Listen(0);
            Socket sock2 = sock.Accept();
            //byte[] msg1 = null;
            //  byte[] msg2=null;
            //    String jsonString = "";
            //      String jsonString2 = "";
            //        while (true)
            //        {

            //read requset from socket
            byte[] msg = new byte[1024];
              int recv = sock2.Receive(msg);
              string input = Encoding.ASCII.GetString(msg, 0, recv);
              Console.Write("get : {0}", input);
              //  if (!(input.Equals("more_details")))
               // {
                    Req req = JsonConvert.DeserializeObject<Req>(input);
                    //initalized paramters - should be data from client
                    string malletPath = @"c:\mallet";
                    string dataPath = req.path; //@"C:\mallet\sample-data\web\news.rar";
                    string resultMalletFile = "keys.mallet";
                    string resultTxtFile = "keys.txt";
                    string DataOfFile = "topics.txt";
                    int numTopics = req.topics; //  10;
                    List<string> flags = req.flags; // new List<string>();
                                                    //flags.Add("keep-sequence");
                                                    //flags.Add("remove-stopwords");
                                                    //flags.Add("stoplist-file heb.txt");

                    //begin
                    dataPath = ExtractFolder(dataPath);
                    IExternal cmd = new CmdWindows(malletPath);
                    MalletOpr mallet = new MalletOpr(cmd);

                    //run
                    mallet.CreateMalletFile(dataPath, resultMalletFile, flags);
                    System.Threading.Thread.Sleep(30000);
                    mallet.RunTopics(numTopics, resultMalletFile, resultTxtFile, DataOfFile);
                    System.Threading.Thread.Sleep(60000);

                    //convert to Json
                    List<Topic> getTopics = mallet.getTopics();
                    List<File> files = mallet.getTopicsForFiles();

                    string jsonString = JsonConvert.SerializeObject(getTopics);
                    string jsonString2 = JsonConvert.SerializeObject(files);


                    byte[] msg1 = Encoding.ASCII.GetBytes(jsonString);
                    byte[] msg2 = Encoding.ASCII.GetBytes(jsonString2);

            //    int bytes = WriteToClient(msg1, sock2);
            //      Console.Write("send msg1: {0}  length is : {1}", jsonString, bytes);
            //    System.Threading.Thread.Sleep(10000);

            //}
            //  else
            // {
            //byte[] msg3 = new byte[1024];
            //int recv2 = sock2.Receive(msg3);
            //int bytes2 = WriteToClient(msg2, sock2);
            //Console.Write("send msg2 : {0} length is :{1} ", jsonString2, bytes2);
            //}

            //  List<String> data = new List<string>();
            //  data.Add(jsonString);
            // data.Add(jsonString2);
            Res data = new Res(jsonString);
            Res data2 = new Res(jsonString2);

            String jsonData1 = JsonConvert.SerializeObject(data);
            String jsonData2 = JsonConvert.SerializeObject(data2);


            Res2 data3= new Res2(jsonData2,jsonData1);
            String jsonData = JsonConvert.SerializeObject(data3);


            //write to socket         
            //     byte[] msg5 = Encoding.ASCII.GetBytes(jsonData);
            byte[] msg6 = Encoding.ASCII.GetBytes(jsonData);
            int bytes = WriteToClient(msg6, sock2);
            int bytes2 = WriteToClient(null, sock2);

            Console.Write("send : {0}",jsonData);
            Console.ReadKey();
                


                // Release the socket.  
                //sender.Shutdown(SocketShutdown.Both);
                //sender.Close();
            //}
        }
        public static string ExtractFolder(string path)
        {           
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            //Put the path of installed winrar.exe
            proc.StartInfo.FileName = @"C:\Program Files (x86)\WinRAR\winRAR.exe";
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.EnableRaisingEvents = true;
            string src = path;
            string des = @"C:\mallet\sample-data\web\";
            proc.StartInfo.Arguments = String.Format("x -o+ \"{0}\" \"{1}\"",src, des);
            proc.Start();
            System.Threading.Thread.Sleep(10000);
            return @"sample-data\web\news";
        }

    }
}