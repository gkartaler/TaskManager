using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManager
{
    public class Aufgabe
    {
        public string Titel;
        public string Prioritaet;
        public DateTime Datum;
        public bool Erledigt;


        //Konstruktor
        public Aufgabe(string titel, string prioritaet, DateTime datum, bool erledigt)
        {
            Titel = titel;
            Prioritaet = prioritaet;
            Datum = datum;
            Erledigt = erledigt;
        }


        // Gibt eine Rangzahl zurück, damit man Prioritäten der Größe nach vergleichen kann
        public int PrioritaetAlsZahl()
        {
            if (Prioritaet == "Hoch") return 3;
            if (Prioritaet == "Mittel") return 2;
            return 1; 
        }
    }
}