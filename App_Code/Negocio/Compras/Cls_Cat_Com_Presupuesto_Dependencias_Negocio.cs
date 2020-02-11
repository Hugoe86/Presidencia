using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Catalogo_Compras_Presupuesto_Dependencias.Datos;

/// <summary>
/// Summary description for Cls_Cat_Com_Presupuesto_Dependencias_Negocio
/// </summary>

namespace Presidencia.Catalogo_Compras_Presupuesto_Dependencias1.Negocio
{
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Cls_Cat_Com_Presupuesto_Dependencias_Negocio
    /// DESCRIPCION:           Clase que contiene la definicion de propiedades y metodos
    /// PARAMETROS :     
    /// CREO       :           José Antonio Lopez Hernández
    /// FECHA_CREO :           04/Enero/2011 13:19 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    public class Cls_Cat_Com_Presupuesto_Dependencias_Negocio
    {
        public Cls_Cat_Com_Presupuesto_Dependencias_Negocio()
        {
        }

        #region(Variables Locales)
            private String Dependencia_ID;
            private int Año_Presupuesto;
            private Double Monto_Comprometido;
            private Double Monto_Presupuestal;
            private Double Monto_Disponible;
            private String Comentarios;
            private String Usuario_Creo;
        #endregion

        #region(Variables Publicas)
            public String P_Dependencia_ID
            {
                get { return Dependencia_ID; }
                set { Dependencia_ID = value; }
            }

            public int P_Año_Presupuesto
            {
                get { return Año_Presupuesto; }
                set { Año_Presupuesto = value; }
            }

            public Double P_Monto_Comprometido
            {
                get { return Monto_Comprometido; }
                set { Monto_Comprometido = value; }
            }

            public Double P_Monto_Presupuestal
            {
                get { return Monto_Presupuestal; }
                set { Monto_Presupuestal = value; }
            }

            public Double P_Monto_Disponible
            {
                get { return Monto_Disponible; }
                set { Monto_Disponible = value; }
            }

            public String P_Comentarios
            {
                get { return Comentarios; }
                set { Comentarios = value; }
            }
            
            public String P_Usuario_Creo
            {
                get { return Usuario_Creo; }
                set { Usuario_Creo = value; }
            }
        #endregion

        #region(Metodos)
            /////*******************************************************************************
            ///// NOMBRE DE LA CLASE:     Alta_Presupuesto_Dependencia
            ///// DESCRIPCION:            Dar de Alta un nuevo presupuesto para una dependencia
            /////                         a la base de datos
            ///// PARAMETROS :            
            ///// CREO       :            José Antonio López Hernández
            ///// FECHA_CREO :            04/Enero/2011 13:29
            ///// MODIFICO          :
            ///// FECHA_MODIFICO    :
            ///// CAUSA_MODIFICACION:
            /////*******************************************************************************/
            //public void Alta_Presupuesto_Dependencia()
            //{
            //    Cls_Cat_Com_Presupuesto_Dependencias_Datos  Cls_Cat_Com_Impuestos_Datos.Alta_Impuestos(this);
            //}

            /////*******************************************************************************
            ///// NOMBRE DE LA CLASE:     Baja_Impuestos
            ///// DESCRIPCION:            Eliminar un impuesto existente de la base de datos
            ///// PARAMETROS :            
            ///// CREO       :            Noe Mosqueda Valadez
            ///// FECHA_CREO :            21/Octubre/2010 16:30 
            ///// MODIFICO          :
            ///// FECHA_MODIFICO    :
            ///// CAUSA_MODIFICACION:
            /////*******************************************************************************/
            //public void Baja_Impuestos()
            //{
            //    Cls_Cat_Com_Impuestos_Datos.Baja_Impuestos(this);
            //}

            /////*******************************************************************************
            ///// NOMBRE DE LA CLASE:     Cambio_Impuestos
            ///// DESCRIPCION:            Modificar un impuesto existente de la base de datos
            ///// PARAMETROS :            
            ///// CREO       :            Noe Mosqueda Valadez
            ///// FECHA_CREO :            21/Octubre/2010 16:44 
            ///// MODIFICO          :
            ///// FECHA_MODIFICO    :
            ///// CAUSA_MODIFICACION:
            /////*******************************************************************************/
            //public void Cambio_Impuestos()
            //{
            //    Cls_Cat_Com_Impuestos_Datos.Cambio_Impuestos(this);
            //}

            /////*******************************************************************************
            ///// NOMBRE DE LA CLASE:     Maximo_Impuesto_ID
            ///// DESCRIPCION:            Realizar la consulta del Maximo ID registro de los impuestos
            ///// PARAMETROS :            
            ///// CREO       :            Noe Mosqueda Valadez
            ///// FECHA_CREO :            21/Octubre/2010 17:04 
            ///// MODIFICO          :
            ///// FECHA_MODIFICO    :
            ///// CAUSA_MODIFICACION:
            /////*******************************************************************************/
            //public String Maximo_Impuesto_ID()
            //{
            //    return Cls_Cat_Com_Impuestos_Datos.Maximo_Impuesto_ID();
            //}

            /////*******************************************************************************
            ///// NOMBRE DE LA CLASE:     Consulta_Impuestos
            ///// DESCRIPCION:            Realizar la consulta de los impuestos por criterio de busqueda o por un ID
            ///// PARAMETROS :            
            ///// CREO       :            Noe Mosqueda Valadez
            ///// FECHA_CREO :            21/Octubre/2010 16:57 
            ///// MODIFICO          :
            ///// FECHA_MODIFICO    :
            ///// CAUSA_MODIFICACION:
            /////*******************************************************************************/
            //public DataTable Consulta_Impuestos()
            //{
            //    return Cls_Cat_Com_Impuestos_Datos.Consulta_Impuestos(this);
            //}
        #endregion
    }
}