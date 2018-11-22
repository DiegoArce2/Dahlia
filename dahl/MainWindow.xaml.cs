using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.IO;
using busqueda;
using System.Threading;
using Biblioteca.Entidades;

namespace dahl
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SpeechRecognitionEngine escuchar = new SpeechRecognitionEngine();
        SpeechSynthesizer hablar = new SpeechSynthesizer();
        bool on = true;
        WebBrowser navegador = new WebBrowser();
        public MainWindow()
        {
            InitializeComponent();
            gramaticas();
            TransformGroup tg = new TransformGroup();
            RotateTransform rt = new RotateTransform(30);
            tg.Children.Add(rt);
            ReproductorDePrueba.RenderTransform = tg;
           
           
        }


        private void gramaticas()
        {
            //escuchar.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(File.ReadAllLines("comandos.txt")))));
            escuchar.LoadGrammarAsync(new DictationGrammar());
            escuchar.RequestRecognizerUpdate();
            hablar.SpeakStarted += Hablar_SpeakStarted;
            hablar.SpeakCompleted += Hablar_SpeakCompleted;
            escuchar.SpeechRecognized += Escuchar_SpeechRecognized;
            
            escuchar.SetInputToDefaultAudioDevice();
        }

        private void Hablar_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            //cuando termina de hablar
            on = true;
            ReproductorDePrueba.Stop();
        }

        private void reproducir()
        {
            try
            {
               
                ReproductorDePrueba.Source = new Uri(@"C:\Users\ProyectosPersonales\Videos\2.mp4");
                ReproductorDePrueba.IsMuted = true;
                ReproductorDePrueba.Play();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void Hablar_SpeakStarted(object sender, SpeakStartedEventArgs e)
        {
            //mientras habla
            on = false;
            reproducir();
            

        }

        private void Escuchar_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
          
            if (on==true)
            {
                lblEscuchar.Content=e.Result.Text;
                if (e.Result.Text.ToLower().Contains("busca"))
                {
                  string  busqueda = e.Result.Text;
                    busqueda = busqueda.Replace("buscar","");
                    System.Diagnostics.Process.Start("https://www.google.cl/search?q="+busqueda+"&rlz=1C1SQJL_esCL818CL818&oq=hola&aqs=chrome..69i57j0l5.3967j0j4&sourceid=chrome&ie=UTF-8");
                }else
                {
                    if (e.Result.Text.ToLower().Contains("hora"))
                    {
                        string hora = DateTime.Now.Hour.ToString();
                        hablar.SpeakAsync("son las " + hora + "horas con " + DateTime.Now.Minute + "minutos" + ". . .");
                    }
                    else
                    {
                        if (e.Result.Text.ToLower().Contains("fecha") || e.Result.Text.ToLower().Contains("a cuant estamos"))
                        {
                            string fecha = DateTime.Now.ToShortDateString();
                            hablar.SpeakAsync("la fecha de hoy es "+fecha+ ". . .");
                        }
                        else
                        {
                            if (e.Result.Text.ToLower().Contains("adios")|| e.Result.Text.ToLower().Contains("adiós"))
                            {
                                hablar.SpeakAsync("Que tenga un buen dia" + ". . .");
                                Thread.Sleep(2000);
                                this.Close();
                            }else
                            {
                                Biblioteca.Entidades.Comandos c = new Biblioteca.Entidades.Comandos();
                                c.Comando = e.Result.Text;
                                

                                if (c.buscar()==true)
                                {
                                    hablar.SpeakAsync(c.Respuesta);
                                }
                                else
                                {
                                    hablar.SpeakAsync("Lo siento, no entiendo");
                                }
                            }
                            
                        }
                    }
                }

                
               
            }
        }

        private void activar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                escuchar.RecognizeAsync(RecognizeMode.Multiple); //con este se activa
                activar.Content = "escuchando";
                   

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
               
            }
        }
    }
}
