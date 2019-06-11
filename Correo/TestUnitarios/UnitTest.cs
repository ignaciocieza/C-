using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Entidades;

namespace TestUnitarios
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void PaquetesConIgualId_LanzaTrackingIdRepetidoException()
        {
            string idRepetido = "111";
            Correo tempCorreo;
            Paquete paqueteA;
            Paquete paqueteB;

            tempCorreo = new Correo();
            paqueteA = new Paquete("Dirección", idRepetido);
            paqueteB = new Paquete("Dirección", idRepetido);

            try
            {
                tempCorreo += paqueteA;
                tempCorreo += paqueteB;
                Assert.Fail("Error, LanzaTrackingIdRepetidoException ");
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e,typeof(TrackingIdRepetidoException));
            }            
        }

        [TestMethod]
        public void PaqueteDeCorreo_ListaInstanciada()
        {
            List<Paquete> tempPaquetes;
            Correo tempCorreo;

            tempCorreo = new Correo();
            tempPaquetes = tempCorreo.Paquetes;

            Assert.IsNotNull(tempPaquetes);
        }        
    }
}
