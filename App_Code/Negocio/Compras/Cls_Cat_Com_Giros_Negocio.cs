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
using Presidencia.Catalogo_Compras_Giros.Datos;

/// <summary>
/// Summary description for Cls_Cat_Com_Giros_Negocio
/// </summary>
namespace Presidencia.Catalogo_Compras_Giros.Negocio
{
    public class Cls_Cat_Com_Giros_Negocio
    {
        public Cls_Cat_Com_Giros_Negocio()
        {
        }

        #region (Variables Locales)
            private String Giro_ID;
            private String Nombre;
            private String Estatus;
            private String Comentarios;
            private String Usuario;
        #endregion

        #region (Variables Publicas)
            public String P_Giro_ID
            {
                get { return Giro_ID; }
                set { Giro_ID = value; }
            }

            public String P_Nombre
            {
                get { return Nombre; }
                set { Nombre = value; }
            }

            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
            }

            public String P_Comentarios
            {
                get { return Comentarios; }
                set { Comentarios = value; }
            }

            public String P_Usuario
            {
                get { return Usuario; }
                set { Usuario = value; }
            }
        #endregion

        #region (Metodos)
            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Alta_Giros
            /// DESCRIPCION:            Dar de Alta un nuevo giro a la base de datos
            /// PARAMETROS :            
            /// CREO       :            Noe Mosqueda Valadez
            /// FECHA_CREO :            04/Noviembre/2010 9:49 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public void Alta_Giros()
            {
                Cls_Cat_Com_Giros_Datos.Alta_Giros(this);
            }

            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Baja_Giros
            /// DESCRIPCION:            Eliminar un giro existente de la base de datos
            /// PARAMETROS :            
            /// CREO       :            Noe Mosqueda Valadez
            /// FECHA_CREO :            04/Noviembre/2010 10:09 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public void Baja_Giros()
            {
                Cls_Cat_Com_Giros_Datos.Baja_Giros(this);
            }

            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Cambio_Giros
            /// DESCRIPCION:            Modificar un giro existente de la base de datos
            /// PARAMETROS :            
            /// CREO       :            Noe Mosqueda Valadez
            /// FECHA_CREO :            04/Noviembre/2010 10:26 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public void Cambio_Giros()
            {
                Cls_Cat_Com_Giros_Datos.Cambio_Giros(this);
            }

            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Consulta_Giros
            /// DESCRIPCION:            Realizar la consulta de los giros por criterio de busqueda o por un ID
            /// PARAMETROS :            
            /// CREO       :            Noe Mosqueda Valadez
            /// FECHA_CREO :            04/Noviembre/2010 10:55 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public DataTable Consulta_Giros()
            {
                return Cls_Cat_Com_Giros_Datos.Consulta_Giros(this);
            }
        #endregion
    }
}
