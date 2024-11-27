using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Data;

namespace SQL_version
{
    public partial class MainWindow : Window
    {
        private Notiz _AktuelleNotiz; // Deklaration des Feldes



        public MainWindow()
        {

            // MessageBox.Show($"gelesene Notizen: {notizenAnzahl} von { NotizenTable.Rows.Count}\ngelesene Kategorien: { kategorienAnzahl} von { KategorienTable.Rows.Count}");


            InitializeComponent();
            _AktuelleNotiz = new Notiz(Kategorie.Sonstiges, ""); // Initialisierung des Feldes im Konstruktor
            InitializeComboBox();
            InitializeSampleData();
            cbxKategorie.SelectedIndex = 0;
            listeAktualisieren();
        }

        private void InitializeComboBox()
        {
            cbxKategorie.ItemsSource = Enum.GetValues(typeof(Kategorie)).Cast<Kategorie>().ToList();
        }

        private void InitializeSampleData()
        {
            new Notiz(Kategorie.Geburtstage, "Mutter: 18.03.1945");
            new Notiz(Kategorie.Geburtstage, "Vater: 21.08.1940");
            new Notiz(Kategorie.Internet, "www.ibb.com\r\nViele interessante Kurse");
            new Notiz(Kategorie.Urlaub, "Mallorca\r\nwar nicht gut!");
            new Notiz(Kategorie.Wichtig, "Steuererklärung\r\nmuss noch gemacht werden!!!");
        }

        private void listeAktualisieren(string suchtext = "")
        {
            if (cbxKategorie.SelectedItem != null)
            {
                Kategorie gewählteKat = (Kategorie)cbxKategorie.SelectedItem;

                var query = Notiz.Notizen.Values.Where(
                    notiz => notiz.Inhalt.Contains(suchtext)
                    && (gewählteKat == Kategorie.Alle || notiz.Kategorie == gewählteKat))
                    .OrderBy(notiz => notiz.Kategorie)
                    .ThenBy(notiz => notiz.Inhalt.Split('\r', '\n')[0]);

                lbxNotizen.ItemsSource = query;
                lbxNotizen.SelectedItem = AktuelleNotiz;
            }
        }

        public Notiz AktuelleNotiz
        {
            get => _AktuelleNotiz;
            set
            {
                _AktuelleNotiz = value;
                tbxNotiz.Text = value?.Inhalt ?? "";
                tbxNotiz.IsEnabled = value != null;
            }
        }

        private void cbxKategorie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxKategorie.SelectedItem is Kategorie selectedKategorie)
            {
                listeAktualisieren();
                btnNeu.IsEnabled = selectedKategorie != Kategorie.Alle;
                Resources["rscFarbe"] = btnNeu.IsEnabled ? Brushes.DarkGray : Brushes.Red;
            }
        }

        private void lbxNotizen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AktuelleNotiz = lbxNotizen.SelectedItem as Notiz;
            btnLöschen.IsEnabled = lbxNotizen.SelectedIndex > -1;

            if (cbxKategorie.SelectedItem is Kategorie selectedKategorie && selectedKategorie == Kategorie.Alle && AktuelleNotiz != null)
            {
                cbxKategorie.SelectedItem = AktuelleNotiz.Kategorie; // Synchronisiere die ComboBox mit der Auswahl in der ListBox
            }
        }

        private void tbxNotiz_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnSpeichern.IsEnabled = AktuelleNotiz != null && !string.IsNullOrEmpty(tbxNotiz.Text);
        }

        private void btnSpeichern_Click(object sender, RoutedEventArgs e)
        {
            if (AktuelleNotiz != null)
            {
                AktuelleNotiz.Inhalt = tbxNotiz.Text;
                listeAktualisieren();
                btnSpeichern.IsEnabled = false;
            }
        }

        private void btnLöschen_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Soll die Notiz wirklich gelöscht werden?", "Notiz löschen", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                Notiz.Entfernen(AktuelleNotiz);
                AktuelleNotiz = null;
                listeAktualisieren();
            }
        }

        private void btnNeu_Click(object sender, RoutedEventArgs e)
        {
            AktuelleNotiz = new Notiz((Kategorie)cbxKategorie.SelectedItem, "Neue Notiz");
            listeAktualisieren();
            tbxNotiz.Focus();
            tbxNotiz.SelectAll();
        }

        private void btnBeenden_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnSuche_Click(object sender, RoutedEventArgs e)
        {
            listeAktualisieren(tbxSuche.Text);
        }

        private void btnClearSearch_Click(object sender, RoutedEventArgs e)
        {
            tbxSuche.Text = "";
            listeAktualisieren();
        }
    }
}
