using System.Windows;
using Microsoft.Win32;
using System.IO;
namespace Notatnik.NET
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;
        private string scieżkaPliku;
        bool czyTekstZmieniony = false;
        public MainWindow()
        {
            InitializeComponent();
            openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Wybierz plik tekstowy";
            openFileDialog.Filter = "Pliki tekstowe (*.txt)|*.txt|Wszystkie pliki (*.*)|*.*";
            openFileDialog.DefaultExt = "txt";
            openFileDialog.FilterIndex = 1;

            saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Zapisz plik tekstowy";
            saveFileDialog.Filter = openFileDialog.Filter;
            saveFileDialog.DefaultExt = "txt";
            saveFileDialog.FilterIndex = 1;

        }

     

        private void MenuItem_Otworz_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(scieżkaPliku))
            {
                openFileDialog.InitialDirectory = Path.GetDirectoryName(scieżkaPliku);
                openFileDialog.FileName = Path.GetFileName(scieżkaPliku);
            }
            bool? wynik = openFileDialog.ShowDialog();

            if (wynik.HasValue && wynik.Value)
            {
                scieżkaPliku = openFileDialog.FileName;
                textBox.Text = File.ReadAllText(scieżkaPliku);
                StatusBarText.Text = "Otwarto plik: " + scieżkaPliku;
                czyTekstZmieniony = false;
            }
            

        }

        private void MenuItem_ZapiszJakoClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(scieżkaPliku))
            {
                saveFileDialog.InitialDirectory = Path.GetDirectoryName(scieżkaPliku);
                saveFileDialog.FileName = Path.GetFileName(scieżkaPliku);
            }
            bool? wynik = saveFileDialog.ShowDialog();
            if (wynik.HasValue && wynik.Value)
            {
                scieżkaPliku = saveFileDialog.FileName;
                File.WriteAllText(scieżkaPliku, textBox.Text);
                StatusBarText.Text = Path.GetFileName(scieżkaPliku);
                czyTekstZmieniony = false;
            }
           
        }

        private void MenuItem_ZapiszClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(scieżkaPliku))
            {
                File.WriteAllText(scieżkaPliku, textBox.Text);
                czyTekstZmieniony = false;
            }
            else MenuItem_ZapiszJakoClick(sender, e);

        }

        private void MenuItem_ZamknijClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void textBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            czyTekstZmieniony = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (czyTekstZmieniony)
            {
                MessageBoxResult wynik = MessageBox.Show("Czy zapisać zmiany w pliku?", "Notatnik.NET", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (wynik == MessageBoxResult.Yes)
                {
                    MenuItem_ZapiszClick(sender, null);
                }
                else if (wynik == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
