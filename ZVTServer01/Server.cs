using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace ZVTServer01
{
    public class Server
    {
        //Konstruktoren
        public Server() : this("", 8000) { }

        public Server(string host, int port)
        {
            SocServerListen.Bind()
        }

        public long HostAdr = 
        System.Net.IPEndPoint IpEp = new System.Net.IPEndPoint();
        Socket SocServerListen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        
    }
}
