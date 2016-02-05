using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZVTClient01;
using Protokoll;

namespace ClientConsole
{
    class Program
    {
        static int Main(string[] args)
        {
            int rc = 0;             //Rückgabewerte der Funktionen prüfen
            string Kommando;        //Speichern von ZVT-Kommandos
            bool exit = false;      //Programmsteuerung (beenden)
            Protokoll.ZVT Transportbox; //enthält Konstanten für die Kommandoübersetzung

            while (exit == false)
            {
                Client Kasse = new Client();

                //----------Verbindung aufbauen----------------------------------------
                rc = Kasse.Verbinden();
                if (rc == 0)
                {
                    Console.WriteLine("Kasse offen");
                }
                if (rc == -1)
                {
                    Console.WriteLine("Verbindung zu Server fehlgeschlagen");
                    return -1;
                }
                //----------Verbindung aufbauen--ENDE----------------------------------


                //----------Bytes senden-----------------------------------------------
                Console.WriteLine("Kommando eingeben:");
                Console.WriteLine("[1]Exit");
                Console.WriteLine("[2]Registrieren");
                Kommando = Console.ReadLine();
                int com = Int32.Parse(Kommando);
                if(com == 1) { exit = true; }
                rc = Kasse.Senden(com);
                if (rc == -1)
                {
                    Console.WriteLine("Senden fehlgeschlagen");
                    return -1;
                }
                //----------Bytes senden--ENDE------------------------------------------


                //----------Bytes empfangen---------------------------------------------
                rc = Kasse.Empfangen();
                if (rc == -1)
                {
                    Console.WriteLine("Fehler beim Empfang der Daten");
                    return -1;
                }
                //----------Bytes empfangen--ENDE---------------------------------------


                //-----------Aufräumen--------------------------------------------------
                rc = Kasse.Beenden();
                if (rc == 0)
                {
                    Console.WriteLine("Kasse geschlossen");
                }
                else if (rc == -1)
                {
                    Console.WriteLine("Verbindung nicht sauber getrennt");
                    return -1;
                }

                Kasse = null;
                //-----------Aufräumen--ENDE--------------------------------------------
            }
            Console.WriteLine("Beenden mit Enter");
            Console.ReadLine();
            return 0;
        }
    }
}
