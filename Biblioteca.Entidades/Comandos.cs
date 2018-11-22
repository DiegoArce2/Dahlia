using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using conexion;


namespace Biblioteca.Entidades
{
    public class Comandos
    {
        public int idComando { get; set; }
        public string Comando { get; set; }
        public string Respuesta { get; set; }

        conexion.dahliaEntities contexto = new conexion.dahliaEntities();
        public bool buscar()
        {
            try {
            conexion.Comandos cl = contexto.Comandos.First(u => u.Comando.Equals(this.Comando));
            this.idComando = cl.idComando;
            this.Comando = cl.Comando;
            this.Respuesta = cl.Respuesta;

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al buscar: " + ex.Message);
                return false;

            }
        }
    }
}
