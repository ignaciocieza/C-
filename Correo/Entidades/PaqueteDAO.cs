using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace Entidades
{  

    public static class PaqueteDAO
    {
        #region Delegados Y Eventos

        public delegate void MiDelegado(string mensaje);

        public static event MiDelegado Excepcion;

        #endregion
        #region Atributos

        static SqlConnection conexion;
        static SqlCommand comando;

        #endregion
        #region Metodos

        /// <summary>
        /// Instancian los atributos de la clase PaqueteDAO, en caso negativo genera una excepcion
        /// </summary>
        static PaqueteDAO()
        {
            try
            {
                string connectionStr = "Data Source=.\\SQLEXPRESS;Initial Catalog=correo-sp-2017;Integrated Security=True";

                PaqueteDAO.conexion = new SqlConnection(connectionStr);
                PaqueteDAO.comando = new SqlCommand();                
            }
            catch(Exception e)
            {
                Excepcion(string.Format("Error en la conexion con el servidor: {0}",e.Message));
            }            
        }
       
        /// <summary>
        /// Guarda datos en la base de datos, en caso negativo genera una excepcion.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool Insertar(Paquete p)
        {
            String consulta;
            bool retorno = false;            

            try
            {
                conexion.Open();              
                consulta = string.Format("INSERT INTO Paquetes (direccionEntrega,trackingID,alumno)  VALUES('{0}','{1}','{2}')",
                    p.DireccionEntrega,p.TrackingID, "Ignacio Cieza");
                comando.CommandText = consulta;
                comando.Connection = conexion;
                comando.ExecuteNonQuery();                
                retorno = true;
            }
            catch (Exception e)
            {
                Excepcion(string.Format("Error en la insercion de datos en la base de datos: {0}", e.Message));
            }
            finally
            {
                if(!(conexion is null))
                {
                    conexion.Close();
                }
            }
            return retorno;
        }       

        #endregion

    }
}
