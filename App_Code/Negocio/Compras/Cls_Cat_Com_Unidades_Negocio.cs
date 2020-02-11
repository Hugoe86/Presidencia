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
using Presidencia.Catalogo_Compras_Unidades.Datos;
/// <summary>
/// Summary description for Cls_Cat_Com_Unidades_Negocio
/// </summary>
namespace Presidencia.Catalogo_Compras_Unidades.Negocio
{
    public class Cls_Cat_Com_Unidades_Negocio
    {
        public Cls_Cat_Com_Unidades_Negocio()
        {
        }

        #region (Variables Locales)
            private String Unidad_ID;
            private String Nombre;
            private String Abreviatura;
            private String Comentarios;
            private String Usuario;
        #endregion

        #region (Variables Publicas)
            public String P_Unidad_ID
            {
                get { return Unidad_ID; }
                set { Unidad_ID = value; }
            }

            public String P_Nombre
            {
                get { return Nombre; }
                set { Nombre = value; }
            }

            public String P_Abreviatura
            {
                get { return Abreviatura; }
                set { Abreviatura = value; }
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
            /// NOMBRE DE LA CLASE:     Alta_Unidades
            /// DESCRIPCION:            Dar de Alta una nueva unidad a la base de datos
            /// PARAMETROS :            
            /// CREO       :            Noe Mosqueda Valadez
            /// FECHA_CREO :            08/Noviembre/2010 10:11 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public void Alta_Unidades()
            {
                Cls_Cat_Com_Unidades_Datos.Alta_Unidades(this);
            }

            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Baja_Unidades
            /// DESCRIPCION:            Eliminar una unidad existente de la base de datos
            /// PARAMETROS :            
            /// CREO       :            Noe Mosqueda Valadez
            /// FECHA_CREO :            08/Noviembre/2010 10:22 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public void Baja_Unidades()
            {
                Cls_Cat_Com_Unidades_Datos.Baja_Unidades(this);
            }

            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Cambio_Unidades
            /// DESCRIPCION:            Modificar una unidad existente de la base de datos
            /// PARAMETROS :            
            /// CREO       :            Noe Mosqueda Valadez
            /// FECHA_CREO :            08/Noviembre/2010 10:50 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public void Cambio_Unidades()
            {
                Cls_Cat_Com_Unidades_Datos.Cambio_Unidades(this);
            }

            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Consulta_Unidades
            /// DESCRIPCION:            Realizar la consulta de las unidades por criterio de busqueda o por un ID
            /// PARAMETROS :            
            /// CREO       :            Noe Mosqueda Valadez
            /// FECHA_CREO :            08/Noviembre/2010 11:00 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public DataTable Consulta_Unidades()
            {
                return Cls_Cat_Com_Unidades_Datos.Consulta_Unidades(this);
            }
        #endregion
    }
}