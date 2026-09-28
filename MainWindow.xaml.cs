using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BazaProduktowWPF
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Produkt> listaProduktow = new ObservableCollection<Produkt>();

        public MainWindow()
        {
            InitializeComponent();

            listaProduktow.Add(new Produkt
            {
                Nazwa = "Olej silnikowy",
                Kategoria = "Motoryzacja",
                Cena = 120,
                Ilosc = 5
            });

            listaProduktow.Add(new Produkt
            {
                Nazwa = "Filtr oleju",
                Kategoria = "Motoryzacja",
                Cena = 35,
                Ilosc = 10
            });

            tabelaProduktow.ItemsSource = listaProduktow;
        }

        private bool SprawdzDane(out decimal cena, out int ilosc)
        {
            cena = 0;
            ilosc = 0;

            if (string.IsNullOrWhiteSpace(txtNazwa.Text))
            {
                MessageBox.Show("Podaj nazwę produktu.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtKategoria.Text))
            {
                MessageBox.Show("Podaj kategorię.");
                return false;
            }

            if (!decimal.TryParse(txtCena.Text, out cena) || cena < 0)
            {
                MessageBox.Show("Cena musi być poprawną liczbą większą lub równą 0.");
                return false;
            }

            if (!int.TryParse(txtIlosc.Text, out ilosc) || ilosc < 0)
            {
                MessageBox.Show("Ilość musi być liczbą całkowitą większą lub równą 0.");
                return false;
            }

            return true;
        }

        private void Dodaj_Click(object sender, RoutedEventArgs e)
        {
            if (!SprawdzDane(out decimal cena, out int ilosc))
                return;

            Produkt nowyProdukt = new Produkt();

            nowyProdukt.Nazwa = txtNazwa.Text.Trim();
            nowyProdukt.Kategoria = txtKategoria.Text.Trim();
            nowyProdukt.Cena = cena;
            nowyProdukt.Ilosc = ilosc;

            listaProduktow.Add(nowyProdukt);

            WyczyscPola();
        }

        private void Edytuj_Click(object sender, RoutedEventArgs e)
        {
            Produkt? produkt = tabelaProduktow.SelectedItem as Produkt;

            if (produkt == null)
            {
                MessageBox.Show("Najpierw wybierz produkt z tabeli.");
                return;
            }

            if (!SprawdzDane(out decimal cena, out int ilosc))
                return;

            produkt.Nazwa = txtNazwa.Text.Trim();
            produkt.Kategoria = txtKategoria.Text.Trim();
            produkt.Cena = cena;
            produkt.Ilosc = ilosc;

            tabelaProduktow.Items.Refresh();

            WyczyscPola();
        }

        private void Usun_Click(object sender, RoutedEventArgs e)
        {
            Produkt? produkt = tabelaProduktow.SelectedItem as Produkt;

            if (produkt == null)
            {
                MessageBox.Show("Wybierz produkt, który chcesz usunąć.");
                return;
            }

            MessageBoxResult odpowiedz = MessageBox.Show(
                "Czy na pewno chcesz usunąć wybrany produkt?",
                "Usuwanie",
                MessageBoxButton.YesNo);

            if (odpowiedz == MessageBoxResult.Yes)
            {
                listaProduktow.Remove(produkt);
                WyczyscPola();
            }
        }

        private void Wyczysc_Click(object sender, RoutedEventArgs e)
        {
            WyczyscPola();
        }

        private void WyczyscPola()
        {
            txtNazwa.Clear();
            txtKategoria.Clear();
            txtCena.Clear();
            txtIlosc.Clear();
            tabelaProduktow.SelectedItem = null;
        }

        private void TabelaProduktow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Produkt? produkt = tabelaProduktow.SelectedItem as Produkt;

            if (produkt != null)
            {
                txtNazwa.Text = produkt.Nazwa;
                txtKategoria.Text = produkt.Kategoria;
                txtCena.Text = produkt.Cena.ToString();
                txtIlosc.Text = produkt.Ilosc.ToString();
            }
        }

        private void Szukaj_TextChanged(object sender, TextChangedEventArgs e)
        {
            string szukanyTekst = txtSzukaj.Text.ToLower();

            if (string.IsNullOrWhiteSpace(szukanyTekst))
            {
                tabelaProduktow.ItemsSource = listaProduktow;
                return;
            }

            var wyniki = listaProduktow
                .Where(p => p.Nazwa.ToLower().Contains(szukanyTekst) ||
                            p.Kategoria.ToLower().Contains(szukanyTekst))
                .ToList();

            tabelaProduktow.ItemsSource = wyniki;
        }
    }
}
