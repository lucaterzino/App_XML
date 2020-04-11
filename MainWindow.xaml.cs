using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace App_XML
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CancellationTokenSource ct;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Btn_Aggiorna_Click(object sender, RoutedEventArgs e)
        {
            ct = new CancellationTokenSource();
            Btn_Aggiorna.IsEnabled = false;
            Btn_Interrompi.IsEnabled = true;
            Lst_Studenti.Items.Clear();
            Task.Factory.StartNew(() => CaricaDati());
        }

        private void CaricaDati()
        {

            string path = @"ListaStudenti.xml";
            XDocument xmlDoc = XDocument.Load(path);
            XElement xmlstudenti = xmlDoc.Element("studenti");

            var xmlstudente = xmlstudenti.Elements("studente");
            Thread.Sleep(800);
            foreach (var item in xmlstudente)
            {
                XElement xmlFirstName = item.Element("nome");          
                XElement xmlPresenze = item.Element("presenze");
                XElement xmlNascita = item.Element("data");
                Studente s = new Studente();
                {
                    s.NomeCompleto = xmlFirstName.Value;
                  
                    s.Presenze = Convert.ToInt32(xmlPresenze.Value);
                    s.DataDiNascita = Convert.ToDateTime(xmlNascita.Value);
                    Dispatcher.Invoke(() => Lst_Studenti.Items.Add(s));
                }

                if (ct.Token.IsCancellationRequested)
                {
                    break;
                }
                Thread.Sleep(800);

            }
            Dispatcher.Invoke(() =>
            {
                Btn_Aggiorna.IsEnabled = true;
                Btn_Interrompi.IsEnabled = false;
                ct = null;
            });
        }

        private void Btn_Interrompi_Click(object sender, RoutedEventArgs e)
        {
            ct.Cancel();
        }

        private void Lst_Studenti_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Studente s = (Studente)Lst_Studenti.SelectedItem;
            if (s != null)
            {
                Lbl_Presenze.Content = s.ToString();
                Txt_Presenze.Text = s.Presenze.ToString();
            }
        }

        private void Btn_SalvaModifiche_Click(object sender, RoutedEventArgs e)
        {
            int flag2 = 0;
            string path = @"ListaStudenti.xml";
            XDocument xmlDoc = XDocument.Load(path);
            XElement xmlstudenti = xmlDoc.Element("studenti");

            var xmlstudente = xmlstudenti.Elements("studente");

            foreach (var item in xmlstudente)
            {
                XElement xmlNomeCompleto = item.Element("nome");
                XElement xmlPresenze = item.Element("presenze");
                XElement xmlNascita = item.Element("data");
                Studente s = new Studente();
                {
                    s.NomeCompleto = xmlNomeCompleto.Value;
                    s.Presenze = Convert.ToInt32(xmlPresenze.Value);
                    s.DataDiNascita = Convert.ToDateTime(xmlNascita.Value);

                }

                if (flag2==flag)
                {
                    item.SetElementValue("presenze", Txt_Presenze.Text);
                    break;
                }
                flag2++;
            }
            xmlDoc.Save("ListaStudenti.xml");
        }

        int flag = 0;

        private void Btn_VisualizzaPresenze_Click(object sender, RoutedEventArgs e)
        {

            string path = @"ListaStudenti.xml";
            XDocument xmlDoc = XDocument.Load(path);
            XElement xmlstudenti = xmlDoc.Element("studenti");

            var xmlstudente = xmlstudenti.Elements("studente");

            foreach (var item in xmlstudente)
            {
                XElement xmlNomeCompleto = item.Element("nome");           
                XElement xmlPresenze = item.Element("presenze");
                XElement xmlNascita = item.Element("data");
                Studente s = new Studente();
                {
                    s.NomeCompleto = xmlNomeCompleto.Value;                
                    s.Presenze = Convert.ToInt32(xmlPresenze.Value);
                    s.DataDiNascita = Convert.ToDateTime(xmlNascita.Value);
                   
                }

                if (Convert.ToString(Lst_Studenti.SelectedItem) == s.NomeCompleto)
                {
                    Txt_Presenze.Text = Convert.ToString(s.Presenze);
                    break;
                }
                flag++;
            }
        }
    }
}
