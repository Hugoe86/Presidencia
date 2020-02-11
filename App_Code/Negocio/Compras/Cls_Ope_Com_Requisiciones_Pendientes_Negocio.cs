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
using Presidencia.Almacen_Requisiciones_Pendientes.Datos;

/// <summary>
/// Summary description for Cls_Ope_Com_Requisiciones_Pendientes_Negocio
/// </summary>
namespace Presidencia.Almacen_Requisiciones_Pendientes.Negocio
{
    public class Cls_Ope_Com_Requisiciones_Pendientes_Negocio
    {
        public Cls_Ope_Com_Requisiciones_Pendientes_Negocio()
        {
        }

        #region (Variables_Internas)
            private long No_Requisicion;
            private long Observacion_ID;
            private String Busqueda;
            private String Empleado_Filtrado_ID;
            private String Comentarios;
            private String Estatus_Requisicion;
            private String Usuario;
            private String Tipo_Requisicion;
            private String Fecha_Inicial_B;
            private String Fecha_Final_B;
            private String URL_LINK;
        #endregion

        #region (Variables Publicas)
            public String P_Busqueda
            {
                get { return Busqueda; }
                set { Busqueda = value; }
            }

            public long P_No_Requisicion
            {
                get { return No_Requisicion; }
                set { No_Requisicion = value; }
            }

            public String P_Empleado_Filtrado_ID
            {
                get { return Empleado_Filtrado_ID; }
                set { Empleado_Filtrado_ID = value; }
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

            public String P_Usuario
            {
                get { return Usuario; }
                set { Usuario = value; }
            }

            public String P_Tipo_Requisicion
            {
                get { return Tipo_Requisicion; }
                set { Tipo_Requisicion = value; }
            }
            public String P_Estatus_Requisicion
            {
                get { return Estatus_Requisicion; }
                set { Estatus_Requisicion = value; }
            }
            public String P_Fecha_Inicial_B
            {
                get { return Fecha_Inicial_B; }
                set { Fecha_Inicial_B = value; }
            }
            public String P_Fecha_Final_B
            {
                get { return Fecha_Final_B; }
                set { Fecha_Final_B = value; }
            }

            public String P_URL_LINK
            {
                get { return URL_LINK; }
                set { URL_LINK = value; }
            }
        #endregion

        #region (Metodos)
            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Consulta_Requisiciones_Pendientes
            /// DESCRIPCION:            Realizar la consulta de las requisiciones en estatus 
            ///                         de AUTORIZADA o ALMACEN
            /// PARAMETROS :            
            /// CREO       :            Noe Mosqueda Valadez
            /// FECHA_CREO :            22/Noviembre/2010 13:22 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public DataTable Consulta_Requisiciones_Pendientes()
            {
                return Cls_Ope_Com_Requisiciones_Pendientes_Datos.Consulta_Requisiciones_Pendientes(this);
            }

            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Cambiar_Estatus_Requisiciones
            /// DESCRIPCION:            Modificar el estatus de una requisicion a FILTRADA  o a REVISAR
            ///                         sus observaciones correspondientes.
            /// PARAMETROS :            
            /// CREO       :            Salvador Hernández Ramírez
            /// FECHA_CREO :            14/Abril/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public void Cambiar_Estatus_Requisiciones()
            {
                Cls_Ope_Com_Requisiciones_Pendientes_Datos.Cambiar_Estatus_Requisiciones(this);
            }     
 
            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Consulta_Productos_Requisicion
            /// DESCRIPCION:            Consulta los  productos de la requisicion
            /// PARAMETROS :            
            /// CREO       :            Salvador Hernández Ramírez
            /// FECHA_CREO :            20/Abril/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public DataTable Consulta_Productos_Requisicion()
            {
                return Cls_Ope_Com_Requisiciones_Pendientes_Datos.Consulta_Productos_Requisicion(this);
            }

            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Consulta_Detalles_Requisicion
            /// DESCRIPCION:            Consulta los detalles de la requisicion
            /// PARAMETROS :            
            /// CREO       :            Salvador Hernández Ramírez
            /// FECHA_CREO :            26/Abril/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public DataTable Consulta_Detalles_Requisicion()
            {
                return Cls_Ope_Com_Requisiciones_Pendientes_Datos.Consulta_Detalles_Requisicion(this);
            }


            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Consulta_Menu_ID
            /// DESCRIPCION:            Consulta  el Identificador de la pagina utilizada para el resguardo de los bienes
            /// PARAMETROS :            
            /// CREO       :            Salvador Hernández Ramírez
            /// FECHA_CREO :            26/Abril/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public String Consulta_Menu_ID()
            {
                return Cls_Ope_Com_Requisiciones_Pendientes_Datos.Consulta_Menu_ID(this);
            } 

        #endregion
    }
}