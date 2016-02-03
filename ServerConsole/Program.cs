using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZVTServer01;

namespace ServerConsole
{
    class Program
    {
        static int Main(string[] args)
        {
            int rc;                                     //return-code für Fehlerabfrage
            string eingabe;                             //Benutzereingaben (Testzwecke)

            ZVTServer01.Server Host = new Server();     //Objekt des Servers erstellen. Default Port 8000


            //--------------------Verbindung herstellen----------------------------------------------------
            rc = Host.Starten(3);                        //Host online setzen
            if (rc == 0)
            {
                Console.WriteLine("1) Host online - " + Host.ServerInfo);
            }
            else if (rc == -1)
            {
                Console.WriteLine("Host konnte nicht an Port gebunden werden");
                return -1;
            }
            else if (rc == -2)
            {
                Console.WriteLine("Host konnte nicht online gehen");
                return -1;
            }
            //--------------------Verbindung herstellen-ENDE---------------------------------------------



            //--------------------Warten auf Verbindungsanfrage------------------------------------------
            rc = Host.Warten();
            if(rc == 0)
            {
                Console.WriteLine("2) Client hat Verbindung aufgenommen");
            }
            else if(rc == -1)
            {
                Console.WriteLine("Fehler bei Verbindungsaufbau");
                return -1;
            }
            //--------------------Warten auf Verbindungsanfrage-ENDE-------------------------------------



            //--------------------Daten entgegennehmen------------------------------------------------
            rc = Host.Empfangen();
            if(rc == -1)
            {
                Console.WriteLine("Fehler beim Empfang der Daten");
            }
            //-------------------Daten entgegennehmen-ENDE-----------------------------------------------



            //-------------------Parsen-----------------------------------------------------------------
            //-------------------Parsen-ENDE------------------------------------------------------------


            //--------------------Analysieren-----------------------------------------------------------
            //--------------------Analysieren-ENDE------------------------------------------------------


            //--------------------Funktion ausführen---------------------------------------------------
            //--------------------Funktion ausführen--ENDE---------------------------------------------


            //--------------------Senden------------------------------------------------------------
            rc = Host.Senden();
            if(rc == -1)
            {
                Console.WriteLine("Fehler beim Senden zum Client");
                return -1;
            }
            //--------------------Senden---ENDE-----------------------------------------------------


            //--------------------Aufräumen-------------------------------------------------------------
            rc = Host.Beenden();
            if (rc == 0) { Console.WriteLine("7) Host offline"); }
            else if(rc == -1) { Console.WriteLine("Host nicht ordnungsgemäß beendet"); }
            //--------------------Aufräumen--ENDE-------------------------------------------------------


            eingabe = Console.ReadLine();
            return 0;
        }
    }
}
