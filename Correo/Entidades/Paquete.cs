using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace Entidades
{
    public class Paquete:IMostrar<Paquete>
    {
        #region Delegado

        public delegate void DelegadoEstado(object sender, EventArgs e);

        public event DelegadoEstado InformarEstado;


        #endregion
        #region Enmuerados

        public enum EEstado
        {
            Ingresado,
            EnViaje,
            Entregado
        }

        #endregion
        #region Atributos

        string direccionEntrega;
        EEstado estado;
        string trackingID;

        #endregion
        #region Propiedades

        /// <summary>
        /// Ingresa o retorna valores del campo direccionEntrega. 
        /// </summary>
        public string DireccionEntrega
        {
            get
            {
                return this.direccionEntrega;
            }
            set
            {
                this.direccionEntrega = value;
            }
        }

        /// <summary>
        /// Ingresa o retorna valores del campo estado
        /// </summary>
        public EEstado Estado
        {
            get
            {
                return this.estado;
            }
            set
            {
                this.estado = value;
            }
        }

        /// <summary>
        /// Ingresa o retorna valores del campo trackingID
        /// </summary>
        public string TrackingID
        {
            get
            {
                return this.trackingID;
            }
            set
            {
                this.trackingID = value;
            }
        }
        #endregion
        #region Constructor

        /// <summary>
        /// Genera una nueva instancia de la clase Paquete
        /// </summary>
        /// <param name="direccionEntrega"></param>
        /// <param name="trackingID"></param>
        public Paquete(string direccionEntrega, string trackingID)
        {
            this.direccionEntrega = direccionEntrega;
            this.trackingID = trackingID;
            this.Estado = EEstado.Ingresado;
        }

        #endregion
        #region Metodos

        /// <summary>
        /// Muestra los datos de los campos de paquete
        /// </summary>
        /// <param name="elemento"></param>
        /// <returns></returns>
        public string MostrarDatos(IMostrar<Paquete> elemento)
        {
            Paquete paquete = (Paquete)elemento;

            return string.Format("{0} para {1}\r\n", paquete.trackingID, paquete.direccionEntrega);            
        }

        /// <summary>
        /// Da visibilidad publica al método MostrarDatos
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.MostrarDatos((IMostrar<Paquete>)this);
        }

        /// <summary>
        /// Genera un ciclo en el campo estado y guarda los datos en base de datos
        /// </summary>
        public void MockCicloDeVida()
        {            
            while (this.estado != EEstado.Entregado)
            {
                Thread.Sleep(4000);
                this.estado++;
                this.InformarEstado.Invoke(null, null);
            }            
            if (this.estado == EEstado.Entregado)
            {
                PaqueteDAO.Insertar(this);
            }
        }

        #endregion
        #region SobreCargas

        /// <summary>
        /// Busca igualdad entre paquetes en base al trackingID.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static bool operator ==(Paquete p1, Paquete p2)
        {
            return (p1.trackingID==p2.trackingID);
        }

        /// <summary>
        /// Busca desigualdad entre paquetes en base al trackingID.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static bool operator !=(Paquete p1, Paquete p2)
        {
            return !(p1 == p2);
        }

        #endregion
    }
}
