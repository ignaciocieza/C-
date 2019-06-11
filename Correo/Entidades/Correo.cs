using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace Entidades
{
    public class Correo:IMostrar<List<Paquete>>
    {
        #region Atributos

        List<Thread> mockPaquetes;
        List<Paquete> paquetes;

        #endregion
        #region Propiedades

        /// <summary>
        /// Ingresa o retorna los datos del campo paquetes
        /// </summary>
        public List<Paquete> Paquetes
        {
            get
            {
                return this.paquetes;
            }
            set
            {
                this.paquetes = value;
            }
        }

        #endregion
        #region Constructor

        /// <summary>
        /// Genera una nueva instancia de la clase Correo
        /// </summary>
        public Correo()
        {
            this.mockPaquetes = new List<Thread>();
            this.paquetes = new List<Paquete>();
        }

        #endregion
        #region Metodos

        /// <summary>
        /// Finaliza todos los hilos activos de la clase correo
        /// </summary>
        public void FinEntregas()
        {
            foreach(Thread temporal in this.mockPaquetes)
            {
                if (temporal.IsAlive)
                {
                    temporal.Abort();
                }
            }
        }

        /// <summary>
        /// Devuelve la informacion del campo paquetes
        /// </summary>
        /// <param name="elementos"></param>
        /// <returns></returns>
        public string MostrarDatos(IMostrar<List<Paquete>> elementos)
        {
            string retorno = "";
            List<Paquete> paquetes = (List<Paquete>)((Correo)elementos).paquetes;
            foreach (Paquete paquete in paquetes)
            {
                retorno += string.Format("{0} para {1} ({2})\n", paquete.TrackingID, paquete.DireccionEntrega, paquete.Estado.ToString());
            }
            return retorno;
        }

        #endregion
        #region Operadores

        /// <summary>
        /// Agrega paquete a coreo y comprueba que no este repetido, sino devuelve excepcion.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="p"></param>
        /// <returns>Correo o excepcion: TrackingIdRepetidoException </returns>
        public static Correo operator +(Correo c, Paquete p)
        {
            Thread t;

            if (!(c.paquetes is null))
            {
                foreach (Paquete temporal in c.paquetes)
                {
                    if (temporal == p)
                    {
                        throw new TrackingIdRepetidoException("Id repetido");
                    }                  
                }
                c.paquetes.Add(p);
                t = new Thread(p.MockCicloDeVida);
                c.mockPaquetes.Add(t);
                t.Start();
            }
            return c;
        }

        #endregion
    }
}
