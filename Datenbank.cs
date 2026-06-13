using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TaskManager
{
    public class Datenbank
    {
        // Name der Datei, in der gespeichert wird.
        private string dateiName = "aufgaben.txt";

        //schreibt alle Aufgaben in die Textdatei
        public void Speichern(Aufgabe[] aufgaben)
        {
            
            string[] zeilen = new string[aufgaben.Length];

            for (int i = 0; i < aufgaben.Length; i++)
            {
                Aufgabe a = aufgaben[i];

                
                zeilen[i] = a.Titel + ";"
                          + a.Prioritaet + ";"
                          + a.Datum.ToString("dd.MM.yyyy") + ";"
                          + a.Erledigt;
            }

            // schreibt alle Zeilen in die Datei.
            File.WriteAllLines(dateiName, zeilen);
        }

        // LADEN: liest alle Aufgaben aus der Textdatei zurück
        public Aufgabe[] Laden()
        {
            
            if (File.Exists(dateiName) == false)
            {
                return new Aufgabe[0];
            }

            
            string[] zeilen = File.ReadAllLines(dateiName);

            
            Aufgabe[] aufgaben = new Aufgabe[zeilen.Length];

            for (int i = 0; i < zeilen.Length; i++)
            {
                
                string[] teile = zeilen[i].Split(';');

                string titel = teile[0];
                string prioritaet = teile[1];
                DateTime datum = DateTime.Parse(teile[2]);
                bool erledigt = bool.Parse(teile[3]);

                
                aufgaben[i] = new Aufgabe(titel, prioritaet, datum, erledigt);
            }

            return aufgaben;
        }
    }
}