using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace busqueda
{
    public partial class Form1 : Form
    {
        WebBrowser navegador = new WebBrowser();

        public Form1()
        {
            InitializeComponent();
            navegador.ScriptErrorsSuppressed = true;
            navegador.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(this.datosCargador);
        }
        string busquedas;
        public void datosCargador(object sender, EventArgs e)
        {
            busquedas = navegador.Document.GetElementById("definicion").InnerText;
            foreach (HtmlElement etiqueta in navegador.Document.All)
            {
                
                    busquedas = etiqueta.InnerText;
                
            }
        }
        public string carga()
        {
            navegador.Navigate("https://definicion.de/apellido/");
  
            return busquedas;
        }

    }
}
