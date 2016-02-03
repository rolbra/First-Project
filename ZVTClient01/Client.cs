using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace ZVTClient01
{
    public class Client
    {

        //Klassenvariablen
        //Instanzvariablen
        Socket SocClient;
        IPEndPoint ClientEp;
        IPAddress ServerIp;
        int ServerPort;

        byte[] SendBuffer;
        byte[] RecvBuffer;

        private struct ZVTPacket
        {
            struct ControlField
            {
                struct Class
                {
                    int nibble1;
                    int nibble2;
                }
                struct Instr
                {
                    int nibble1;
                    int nibble2;
                }
            }
            int Length;
            struct DataBlock
            {

            }
        }

        //Konstruktoren
        public Client() : this("192.168.2.105", 8000)
        {
            //NOP
        }

        public Client(string hostIp, int port)
        {
            SocClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.ServerPort = port;
            this.ServerIp = System.Net.IPAddress.Parse(hostIp);
            ClientEp = new IPEndPoint(ServerIp, ServerPort);
            SendBuffer = new Byte[128];
            RecvBuffer = new Byte[128];
        }

        //Methoden
        public int Verbinden()
        {
            try
            {
                SocClient.Connect(ClientEp);
            }
            catch(Exception e)
            {
                Console.Write(e.Message);
                return -1;
            }
            return 0;
        }

        //Bytes senden
        public int Senden(string text)
        {
            try
            {
                SendBuffer = Encoding.UTF8.GetBytes(text);
                int anz_bytes = SocClient.Send(SendBuffer);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }

            return 0;
        }

        //Empfangen
        public int Empfangen()
        {
            try
            {
                string text;
                int anz_bytes = SocClient.Receive(RecvBuffer, 128, SocketFlags.None);
                Console.WriteLine("5) {0} Bytes empfangen", anz_bytes);
                text = Encoding.UTF8.GetString(RecvBuffer);
                Console.Write("7) ");
                Console.WriteLine(text);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
            return 0;
        }

        //Beenden
        public int Beenden()
        {
            try
            {
                SocClient.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
            return 0;
        }

        //-----------------------untere Schichten----------------------------------
        private int packen(string kommando)
        {

            return 0;
        }
    }
}
