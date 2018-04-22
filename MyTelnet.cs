using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Model;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Client.View
{
    public class MyTelnet : ITelnetClient
    {
        private Socket sock;

        public void connect(string ip, int port)
        {

            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(ip), port);
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                sock.Connect(ipep);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Unable to connect to server." + e.ToString());
                return;
            }
        }
        //disconnect from server
        public void disconnect()
        {
            sock.Shutdown(SocketShutdown.Both);
            sock.Close();
        }
        //send commands in another thread so the program can keep running
        public void write(string command)
        {
            Thread send = new Thread(o => SendCommands(command));
            send.Start();
        }
        public string read()
        {
            string data = "";
            ReceiveOutputs(ref data);
            return data;
        }

        //receive data from server converting it to string
        private void ReceiveOutputs(ref string input)
        {
            byte[] data = new byte[1024];
            int recv = sock.Receive(data);
            input = Encoding.ASCII.GetString(data, 0, recv);
        }
        //send commands to server
        private void SendCommands(string data)
        {
            sock.Send(Encoding.ASCII.GetBytes(data));
        }
    }
}
