using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protokoll
{
    public class ZVT    //Hier werden vorerst Strukturen, welche vom ZVT-Client und -Server benutzt werden können
    {
        public struct ControlField
        {
            public byte CCRC;   //nur für Server
            public byte APRC;   //nur für Server

            public byte CLASS;  //nur für Client
            public byte INSTR;  //nur für Client
        }
        public struct Password
        {
            public byte part2;
            public byte part1;
            public byte part0;
        }

        public struct APDU      //wird von Client und Server instanziert
        {
            public ControlField CONTROLFIELD;
            public byte LENGTH;         //Laut Protokoll: 1Byte. Wenn length==FF, wird ein 2-Byte "extended length field" angehängt. (Bei Übertragung großer Datenblöcke zb für Softwareupdates)
            public Password PASSWORD;
            public byte CONFIG;
        }


        public class ClassCodes
        {
            public string[] Kommandos;
            public ClassCodes()
            {
                Kommandos = new string[100];
                Kommandos[0] = "Exit";
                Kommandos[1] = "06 00";
            }
        }
    }
}
