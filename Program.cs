using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class Program
    {

        public static byte[] StartReadClient(Socket sender)
        {
            byte[] bytes2 = new byte[1024];
            // Connect the socket to the remote endpoint. Catch any errors.  
            try
            {
                Console.WriteLine("Socket connected to {0}",
                    sender.RemoteEndPoint.ToString());

                // Receive the response from the remote device.  
                int bytesRec = sender.Receive(bytes2);
                Console.WriteLine("Echoed test = {0}",
                    Encoding.ASCII.GetString(bytes2, 0, bytesRec));
            
                return bytes2;
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                return null;

            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
                return null;

            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
                return null;

            }

        }

        public static int WriteToClient(byte[] bytes, Socket sender)
        {
            // Connect the socket to the remote endpoint. Catch any errors.  
            try
            {

                Console.WriteLine("Socket connected to {0}",
                    sender.RemoteEndPoint.ToString());
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
            // This example uses port 11000 on the local computer.  
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP  socket.  
            Socket sender = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);
            sender.Connect(remoteEP);

            while (true)
            {
                //read requset from socket
                byte[] msg = StartReadClient(sender);
                string req = Encoding.ASCII.GetString(msg);

                //initalized paramters - should be data from client
                string malletPath = @"c:\mallet";
                string dataPath = @"sample-data\web\news";
                string resultMalletFile = "keys.mallet";
                string resultTxtFile = "keys.txt";
                string DataOfFile = "topics.txt";
                int numTopics = 10;
                List<string> flags = new List<string>();
                flags.Add("keep-sequence");
                flags.Add("remove-stopwords");

                //begin
                IExternal cmd = new CmdWindows(malletPath);
                MalletOpr mallet = new MalletOpr(cmd);

                //run
                mallet.CreateMalletFile(dataPath, resultMalletFile, flags);
                mallet.RunTopics(numTopics, resultMalletFile, resultTxtFile, DataOfFile);
                List<Topic> getTopics = mallet.getTopics();
                List<File> files = mallet.getTopicsForFiles();

                String jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(getTopics);
                String jsonString2 = Newtonsoft.Json.JsonConvert.SerializeObject(files);
                List<String> data = new List<string>();
                data.Add(jsonString);
                data.Add(jsonString2);
                String jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(data);

                //write to socket
                byte[] msg1 = Encoding.ASCII.GetBytes(jsonData);
                int bytes = WriteToClient(msg1, sender);

                // Release the socket.  
                //sender.Shutdown(SocketShutdown.Both);
                //sender.Close();
            }
        }
    }
}