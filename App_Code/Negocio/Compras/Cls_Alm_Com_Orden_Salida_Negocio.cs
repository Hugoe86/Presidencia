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
using Presidencia.Almacen_Orden_Salida.Datos;

/// <summary>
/// Summary description for Cls_Alm_Com_Orden_Salida_Negocio
/// </summary>
namespace Presidencia.Almacen_Orden_Salida.Negocio
{
    public class Cls_Alm_Com_Orden_Salida_Negocio
    {
        public Cls_Alm_Com_Orden_Salida_Negocio()
        {
        }

        #region (Variables Locales)
            private long No_Requisicion;
            private String Busqueda;
            private long No_Salida;
            private String Dependencia_ID;
            private String Area_ID;
            private String Empleado_Solicito_ID;
            private String Tipo_Salida_ID;
            private String Usuario;
            private String Folio;
            private String Estatus;
            private String Empleado_Surtido_ID;
            private Double Total;
            private String Comentarios;
            private long Observacion_ID;
        #endregion

        #region (Variables Publicas)
            public long P_No_Requisicion
            {
                get { return No_Requisicion; }
                set { No_Requisicion = value; }
            }

            public String P_Busqueda
            {
                get { return Busqueda; }
                set { Busqueda = value; }
            }

            public long P_No_Salida
            {
                get { return No_Salida; }
                set { No_Salida = value; }
            }

            public String P_Dependencia_ID
            {
                get { return Dependencia_ID; }
                set { Dependencia_ID = value; }
            }

            public String P_Area_ID
            {
                get { return Area_ID; }
                set { Area_ID = value; }
            }

            public String P_Empleado_Solicito_ID
            {
                get { return Empleado_Solicito_ID; }
                set { Empleado_Solicito_ID = value; }
            }

            public String P_Tipo_Salida_ID
            {
                get { return Tipo_Salida_ID; }
                set { Tipo_Salida_ID = value; }
            }

            public String P_Usuario
            {
                get { return Usuario; }
                set { Usuario = value; }
            }

            public String P_Folio
            {
                get { return Folio; }
                set { Folio = value; }
            }

            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
            }

            public String P_Empleado_Surtido_ID
            {
                get { return Empleado_Surtido_ID; }
                set { Empleado_Surtido_ID = value; }
            }

            public Double P_Total
            {
                get { return Total; }
                set { Total = value; }
            }

            public String P_Comentarios
            {
                get { return Comentarios; }
                set { Comentarios = value; }
            }

            public long P_Observacion_ID
            {
                get { return Observacion_ID; }
                set { Observacion_ID = value; }
            }
        #endregion

        #region (Metodos)
            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Consulta_Requisiciones
            /// DESCRIPCION:            Realizar la consulta de las requisciones de acuerdo a 
            ///                         un criterio de busqueda
            /// PARAMETROS :            
            /// CREO       :            Noe Mosqueda Valadez
            /// FECHA_CREO :            12/Noviembre/2010 19:00 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public DataTable Consulta_Requisiciones()
            {
                return Cls_Alm_Com_Orden_Salida_Datos.Consulta_Requisiciones(this);
            }

            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Consulta_Requisicion_Detalles
            /// DESCRIPCION:            Realizar la consulta de los detalles de una requiscion
            /// PARAMETROS :            
            /// CREO       :            Noe Mosqueda Valadez
            /// FECHA_CREO :            12/Noviembre/2010 10:28 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public DataTable Consulta_Requisicion_Detalles()
            {
                return Cls_Alm_Com_Orden_Salida_Datos.Consulta_Requisicion_Detalles(this);
            }

            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Alta_orden_Salida
            /// DESCRIPCION:            Dar de alta la orden de salida de material
            /// PARAMETROS :            
            /// CREO       :            Noe Mosqueda Valadez
            /// FECHA_CREO :            20/Noviembre/2010 9:28 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public long Alta_Orden_Salida()
            {
                return Cls_Alm_Com_Orden_Salida_Datos.Alta_Orden_Salida(this);
            }

            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Imprime_orden_Salida
            /// DESCRIPCION:            Consultar los datos para la impresion de la orden de salida
            /// PARAMETROS :            
            /// CREO       :            Noe Mosqueda Valadez
            /// FECHA_CREO :            24/Noviembre/2010 16:25 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public DataSet Imprime_Orden_Salida()
            {
                return Cls_Alm_Com_Orden_Salida_Datos.Imprime_Orden_Salida(this);
            }
        #endregion
    }
}