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
using Presidencia.Catalogo_Compras_Impuestos.Datos;

/// <summary>
/// Summary description for Cls_Cat_Com_Impuestos_Negocio
/// </summary>

namespace Presidencia.Catalogo_Compras_Impuestos.Negocio
{
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Cls_Cat_Com_Impuestos_Negocios
    /// DESCRIPCION:           Clase que contiene la definicion de propiedades y metodos
    /// PARAMETROS :     
    /// CREO       :           Noe Mosqueda Valadez
    /// FECHA_CREO :           21/Octubre/2010 10:39 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    public class Cls_Cat_Com_Impuestos_Negocio
    {
        public Cls_Cat_Com_Impuestos_Negocio()
        {
        }

        #region(Variables Locales)
            private String Impuesto_ID;
            private String Nombre;
            private Double Porcentaje_Impuesto;
            private String Comentarios;
            private String Usuario_Creo;
        #endregion

        #region(Variables Publicas)
            public String P_Impuesto_ID
            {
                get { return Impuesto_ID; }
                set { Impuesto_ID = value; }
            }

            public String P_Nombre
            {
                get { return Nombre; }
                set { Nombre = value; }
            }

            public Double P_Porcentaje_Impuesto
            {
                get { return Porcentaje_Impuesto; }
                set { Porcentaje_Impuesto = value; }
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
            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Alta_Impuestos
            /// DESCRIPCION:            Dar de Alta un nuevo impuesto a la base de datos
            /// PARAMETROS :            
            /// CREO       :            Noe Mosqueda Valadez
            /// FECHA_CREO :            21/Octubre/2010 14:00 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public void Alta_Impuestos()
            {
                Cls_Cat_Com_Impuestos_Datos.Alta_Impuestos(this);
            }

            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Baja_Impuestos
            /// DESCRIPCION:            Eliminar un impuesto existente de la base de datos
            /// PARAMETROS :            
            /// CREO       :            Noe Mosqueda Valadez
            /// FECHA_CREO :            21/Octubre/2010 16:30 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public void Baja_Impuestos()
            {
                Cls_Cat_Com_Impuestos_Datos.Baja_Impuestos(this);
            }

            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Cambio_Impuestos
            /// DESCRIPCION:            Modificar un impuesto existente de la base de datos
            /// PARAMETROS :            
            /// CREO       :            Noe Mosqueda Valadez
            /// FECHA_CREO :            21/Octubre/2010 16:44 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public void Cambio_Impuestos()
            {
                Cls_Cat_Com_Impuestos_Datos.Cambio_Impuestos(this);
            }

            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Maximo_Impuesto_ID
            /// DESCRIPCION:            Realizar la consulta del Maximo ID registro de los impuestos
            /// PARAMETROS :            
            /// CREO       :            Noe Mosqueda Valadez
            /// FECHA_CREO :            21/Octubre/2010 17:04 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public String Maximo_Impuesto_ID()
            {
                return Cls_Cat_Com_Impuestos_Datos.Maximo_Impuesto_ID();
            }

            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Consulta_Impuestos
            /// DESCRIPCION:            Realizar la consulta de los impuestos por criterio de busqueda o por un ID
            /// PARAMETROS :            
            /// CREO       :            Noe Mosqueda Valadez
            /// FECHA_CREO :            21/Octubre/2010 16:57 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public DataTable Consulta_Impuestos()
            {
                return Cls_Cat_Com_Impuestos_Datos.Consulta_Impuestos(this);
            }
        #endregion
    }
}