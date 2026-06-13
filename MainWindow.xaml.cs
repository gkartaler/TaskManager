using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TaskManager
{
    public partial class MainWindow : Window
    {
        // Unser Speicher für alle Aufgaben 
        private Aufgabe[] aufgaben = new Aufgabe[0];

        // Die Datenbank zum Speichern/Laden in die Textdatei
        private Datenbank datenbank = new Datenbank();

        public MainWindow()
        {
            InitializeComponent();

            // Beim Start: gespeicherte Aufgaben laden und anzeigen
            aufgaben = datenbank.Laden();
            ListeAnzeigen();
        }

        
        private void HinzufuegenKnopf_Click(object sender, RoutedEventArgs e)
        {
            
            string titel = TitelEingabe.Text;

            
            if (titel == "")
            {
                MessageBox.Show("Bitte einen Titel eingeben.");
                return;
            }

            string prioritaet = HoleAusgewaehltePrioritaet();

            
            DateTime datum = DateTime.Today;
            if (DatumAuswahl.SelectedDate != null)
            {
                datum = (DateTime)DatumAuswahl.SelectedDate;
            }

            
            Aufgabe neue = new Aufgabe(titel, prioritaet, datum, false);

            
            Aufgabe[] groesser = new Aufgabe[aufgaben.Length + 1];
            for (int i = 0; i < aufgaben.Length; i++)
            {
                groesser[i] = aufgaben[i];
            }
            groesser[aufgaben.Length] = neue;
            aufgaben = groesser;

          
            TitelEingabe.Text = "";
            datenbank.Speichern(aufgaben);
            ListeAnzeigen();
        }

        
        private void ErledigtKnopf_Click(object sender, RoutedEventArgs e)
        {
            int index = AufgabenListe.SelectedIndex;
            if (index == -1)
            {
                MessageBox.Show("Bitte zuerst eine Aufgabe auswählen.");
                return;
            }

            
            aufgaben[index].Erledigt = !aufgaben[index].Erledigt;

            datenbank.Speichern(aufgaben);
            ListeAnzeigen();
        }

        
        private void BearbeitenKnopf_Click(object sender, RoutedEventArgs e)
        {
            int index = AufgabenListe.SelectedIndex;
            if (index == -1)
            {
                MessageBox.Show("Bitte zuerst eine Aufgabe auswählen.");
                return;
            }

           
            string neuerTitel = TitelEingabe.Text;
            if (neuerTitel == "")
            {
                MessageBox.Show("Tippe oben den neuen Titel ein, dann Bearbeiten klicken.");
                return;
            }

            aufgaben[index].Titel = neuerTitel;

            TitelEingabe.Text = "";
            datenbank.Speichern(aufgaben);
            ListeAnzeigen();
        }

        
        private void LoeschenKnopf_Click(object sender, RoutedEventArgs e)
        {
            int index = AufgabenListe.SelectedIndex;
            if (index == -1)
            {
                MessageBox.Show("Bitte zuerst eine Aufgabe auswählen.");
                return;
            }

            
            Aufgabe[] kleiner = new Aufgabe[aufgaben.Length - 1];

            
            int j = 0;
            for (int i = 0; i < aufgaben.Length; i++)
            {
                if (i != index)
                {
                    kleiner[j] = aufgaben[i];
                    j++;
                }
            }
            aufgaben = kleiner;

            datenbank.Speichern(aufgaben);
            ListeAnzeigen();
        }

        //  Sortiert mit Bubble Sort
        private void SortierenKnopf_Click(object sender, RoutedEventArgs e)
        {
            
            
            for (int i = 0; i < aufgaben.Length - 1; i++)
            {
                for (int k = 0; k < aufgaben.Length - 1 - i; k++)
                {
                    
                    if (aufgaben[k].PrioritaetAlsZahl() < aufgaben[k + 1].PrioritaetAlsZahl())
                    {
                        Aufgabe merker = aufgaben[k];
                        aufgaben[k] = aufgaben[k + 1];
                        aufgaben[k + 1] = merker;
                    }
                }
            }

            datenbank.Speichern(aufgaben);
            ListeAnzeigen();
        }

        
        private void FilterAuswahl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Wenn das Fenster noch lädt, gibt es die Liste evtl. noch nicht
            if (AufgabenListe == null) return;

            ListeAnzeigen();
        }

        // Hilfsmethode
        private void ListeAnzeigen()
        {
            // Liste leeren
            AufgabenListe.Items.Clear();

            string filter = HoleAusgewaehltenFilter();
            int erledigtAnzahl = 0;

            for (int i = 0; i < aufgaben.Length; i++)
            {
                Aufgabe a = aufgaben[i];

                if (a.Erledigt) erledigtAnzahl++;

                // Filter anwenden: passende Aufgaben überspringen
                if (filter == "Offen" && a.Erledigt) continue;
                if (filter == "Erledigt" && a.Erledigt == false) continue;

                // Häkchen [x] wenn erledigt, sonst [ ]
                string haken = "[ ]";
                if (a.Erledigt) haken = "[x]";

                // Eine Textzeile zusammenbauen
                string zeile = haken + " " + a.Titel
                             + "  (" + a.Prioritaet + ")"
                             + "  fällig am " + a.Datum.ToString("dd.MM.yyyy");

                AufgabenListe.Items.Add(zeile);
            }

            
            ZaehlerText.Text = erledigtAnzahl + " von " + aufgaben.Length + " Aufgaben erledigt";
        }

        // Hilfsmethode
        private string HoleAusgewaehltePrioritaet()
        {
            ComboBoxItem item = (ComboBoxItem)PrioritaetAuswahl.SelectedItem;
            return item.Content.ToString();
        }

        // Hilfsmethode
        private string HoleAusgewaehltenFilter()
        {
            ComboBoxItem item = (ComboBoxItem)FilterAuswahl.SelectedItem;
            return item.Content.ToString();
        }
    }
}