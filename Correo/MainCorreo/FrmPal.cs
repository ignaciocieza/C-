using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Entidades;


namespace MainCorreo
{
    public partial class FrmPpal : Form
    {
        #region Atributos

        Correo correo;

        #endregion
        
        /// <summary>
        /// Inicializa los componetes del formulario e instancia al atributo correo.
        /// </summary>
        public FrmPpal()
        {
            InitializeComponent();
            correo = new Correo();
        }
       
        /// <summary>
        /// Cierra los hilos activos al cerrar el formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.correo.FinEntregas();
        }

        /// <summary>
        /// Informa en caso de que suceda algun error
        /// </summary>
        /// <param name="mensaje"></param>
        private void PaqueteDAO_Excepcion(string mensaje)
        {
            MessageBox.Show(mensaje);
        }

        /// <summary>
        /// Agrega un paquete al correo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            Paquete paquete = new Paquete(this.txtDireccion.Text.ToString(), this.mtxtTrackingID.Text.ToString());

            paquete.InformarEstado += Paq_InformarEstado;
            PaqueteDAO.Excepcion += PaqueteDAO_Excepcion;

            try
            {                
                this.correo += paquete;              
            }
            catch(TrackingIdRepetidoException ex) 
            {
                MessageBox.Show(string.Format("El trackin ID: {0} ya figura en la lista de envios", this.mtxtTrackingID.Text.ToString()),ex.Message);
            }
            this.ActualizarEstados();
        }

        private void Paq_InformarEstado(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                Paquete.DelegadoEstado d = new Paquete.DelegadoEstado(this.Paq_InformarEstado);
                this.Invoke(d, new object[] { sender, e });
            }
            else
            {
                this.ActualizarEstados();
            }
        }

        /// <summary>
        /// actualiza las listas en base a su estado
        /// </summary>
        private void ActualizarEstados()
        {
            lstEstadoIngresado.Items.Clear();
            lstEstadoEnViaje.Items.Clear();
            lstEstadoEntregado.Items.Clear();

            foreach(Paquete temporal in this.correo.Paquetes)
            {                
                switch (temporal.Estado)
                {
                    case Paquete.EEstado.Ingresado:
                        lstEstadoIngresado.Items.Add(temporal);
                        break;
                    case Paquete.EEstado.EnViaje:
                        lstEstadoEnViaje.Items.Add(temporal);
                        break;
                    case Paquete.EEstado.Entregado:
                        lstEstadoEntregado.Items.Add(temporal);
                        break;
                    default:
                        break;
                }
            }            
        }

        /// <summary>
        /// al hacer clic derecho sobre la lista EstadoEntregado, se abre un menu para mostrar informacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMostrarTodos_Click(object sender, EventArgs e)
        {
            this.MostrarInformacion<List<Paquete>>((IMostrar<List<Paquete>>)correo);
        }

        /// <summary>
        /// Muestra la infromacion del elemento
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elemento"></param>
        private void MostrarInformacion<T>(IMostrar<T> elemento) 
        {          
            if (!(elemento is null))
            {
                this.rtbMostrar.Text = elemento.MostrarDatos(elemento);
                elemento.MostrarDatos(elemento).Guardar("salida.txt");
            }
        }

        /// <summary>
        /// Muestra informacion del objeto seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.MostrarInformacion<Paquete>((IMostrar<Paquete>)lstEstadoEntregado.SelectedItem);
        }
    }
}
