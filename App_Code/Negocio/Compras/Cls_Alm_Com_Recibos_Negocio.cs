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
using Presidencia.Almacen_Recibos.Datos;

/// <summary>
/// Summary description for Cls_Alm_Com_Recibos_Negocio
/// </summary>
namespace Presidencia.Almacen_Recibos.Negocio
{
    public class Cls_Alm_Com_Recibos_Negocio
    {
        public Cls_Alm_Com_Recibos_Negocio()
        {
        }

        #region (Variables Locales)
            private long No_Requisicion;
            private String No_Recibo;
            private String Empleado_Recibo_ID;
            private String Empleado_Almacen_ID;
            private String Usuario;
            private long Observacion_ID;
            private String Area_ID;
            private String Dependencia_ID;
            private String Comentarios;
            private String No_Entrada;

        #endregion

        #region (Variables Publicas)
            public long P_No_Requisicion
            {
                get { return No_Requisicion; }
                set { No_Requisicion = value; }
            }

            public String P_No_Recibo
            {
                get { return No_Recibo; }
                set { No_Recibo = value; }
            }

            public String P_Empleado_Recibo_ID
            {
                get { return Empleado_Recibo_ID; }
                set { Empleado_Recibo_ID = value; }
            }

            public String P_Empleado_Almacen_ID
            {
                get { return Empleado_Almacen_ID; }
                set { Empleado_Almacen_ID = value; }
            }

            public String P_Usuario
            {
                get { return Usuario; }
                set { Usuario = value; }
            }

            public long P_Observacion_ID
            {
                get { return Observacion_ID; }
                set { Observacion_ID = value; }
            }

            public String P_Area_ID
            {
                get { return Area_ID; }
                set { Area_ID = value; }
            }

            public String P_Dependencia_ID
            {
                get { return Dependencia_ID; }
                set { Dependencia_ID = value; }
            }

            public String P_Comentarios
            {
                get { return Comentarios; }
                set { Comentarios = value; }
            }

            public String P_No_Entrada
            {
                get { return No_Entrada; }
                set { No_Entrada = value; }
            }
        #endregion

        #region (Metodos)
            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Alta_Recibo
            /// DESCRIPCION:            Dar de alta un recibo de material
            /// PARAMETROS :            
            /// CREO       :            Noe Mosqueda Valadez
            /// FECHA_CREO :            27/Noviembre/2010 14:00 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public String Alta_Recibo()
            {
                return Cls_Alm_Com_Recibos_Datos.Alta_Recibo(this);
            }

            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Imprime_Recibo
            /// DESCRIPCION:            Consulta para los datos del recibo
            /// PARAMETROS :            
            /// CREO       :            Noe Mosqueda Valadez
            /// FECHA_CREO :            29/Noviembre/2010 17:13 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public DataSet Imprime_Recibo()
            {
                return Cls_Alm_Com_Recibos_Datos.Imprime_Recibo(this);
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
            public DataTable Consulta_Requisiciones_Detalles()
            {
                return Cls_Alm_Com_Recibos_Datos.Consulta_Requisicion_Detalles(this);
            }

            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Consulta_Datos_Requisicion
            /// DESCRIPCION:            Consulta para los datos de la requisicion
            /// PARAMETROS :            
            /// CREO       :            Noe Mosqueda Valadez
            /// FECHA_CREO :            30/Noviembre/2010 9:55 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public DataTable Consulta_Datos_Requisicion()
            {
                return Cls_Alm_Com_Recibos_Datos.Consulta_Datos_Requisicion(this);
            }
        #endregion
    }
}