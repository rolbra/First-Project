using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using Protokoll;

namespace ZVTServer01
{
    public class Server
    {
        //Klassenvariablen

        //Instanzvariablen
        Socket SocServerListen;     //Socket, der Verbindungen zu CLients aufbaut
        IPEndPoint HostEp;          //Sockets werden durch Endpunkten verbunden
        private IPAddress HostIp;   //Host-IP wird für bind()-Aufruf benötigt
        IPHostEntry HostInfo;       //Enthält Informationen wie IP-Adresse
        int HostPort;               //Port, über den der Server erreichbar sein soll
        public string ServerInfo { get;}   //gibt IP-Adresse und Portnr an Aufrufer als String zurück

        Socket SocClient;
        EndPoint ClientEp;
        Byte [] RecvBuffer;         //Empfangspuffer
        Byte [] SendBuffer;         //Sendepuffer

        public string[] TextRecv;
        public string[] TextSend;
        public int indexRecv, indexSend;
        public bool beenden { get; set; }       //fährt den Server herrunter

        Protokoll.ZVT.ClassCodes TransportTools;

        //Konstruktoren
        public Server() : this("192.168.2.105", 8000)       //Konstruktoren verketten.
        {
            //NOP
        }    

        public Server(string hostIp, int port)
        {
            this.HostPort = port;                           
            this.HostInfo = Dns.GetHostEntry("localhost");  //lokale IP soll automatisch bezogen werden | funktioniert nicht!
            //this.HostIp = this.HostInfo.AddressList;
            this.HostIp = System.Net.IPAddress.Parse(hostIp); //IP wird daher zu testen vorgegeben
            this.HostEp = new IPEndPoint(this.HostIp, this.HostPort);
            ServerInfo = HostEp.ToString();
            SocServerListen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);  //TCP-Socket um Client-Verbindungen zu managen
            SendBuffer = new Byte[128];
            RecvBuffer = new Byte[128];
            TextRecv = new string[100];
            TextSend = new string[100];
            beenden = false;
            TransportTools = new ZVT.ClassCodes();
        }

        //Methoden
        //Starten
        public int Starten(int maxClients)
        {
            try
            {
                SocServerListen.Bind(this.HostEp);              //Server an Port binden
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
            try
            {
                SocServerListen.Listen(maxClients);                      //Server überwacht den Port auf eingehende Client-Verbindunsanfragen
            }
            catch
            {
                return -2;
            }
            return 0;
        }

        //Warten
        public int Warten()
        {
            try
            {
                SocClient = SocServerListen.Accept();           //Neue Verbindung für Datenübertragung über SocClient
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
            try {
                if (indexRecv == 99) indexRecv = 0;     //nur die letzten 100 Nachrichten werden gespeichert
                int anz_bytes = SocClient.Receive(RecvBuffer, 128, SocketFlags.None);  //Bytes entgegennehmen und zählen
                string text = Encoding.UTF8.GetString(RecvBuffer, 0, anz_bytes);
                TextRecv[indexRecv] = text;
                Analysieren(TextRecv[indexRecv]);
                indexRecv++;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
            return 0;
        }

        //Antworten
        public int Senden()
        {
            try
            {
                if (indexSend == 99) indexSend = 0;     //nur die letzten 100 Nachrichten werden gespeichert
                TextSend[indexSend] = "Gruss vom Host";              //Text zum senden vorbereiten
                SendBuffer = Encoding.UTF8.GetBytes(TextSend[indexSend++]);  //Zum Transport in Bytes konvertieren
                int anz_bytes = SocClient.Send(SendBuffer); //Senden und Bytes zählen
                
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
                SocServerListen.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
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

        //------------------------untere Schichten----------------------------------
        //Analysieren
        private int Analysieren(string text)
        {
            try
            {
                foreach(string element in TransportTools.Kommandos)
                {
                    if(element == text)
                    {
                        if(element == "Exit") { beenden = true; }
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
            return 0;
        }
    }
}
