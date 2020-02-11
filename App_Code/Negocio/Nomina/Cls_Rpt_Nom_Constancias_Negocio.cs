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
using Presidencia.Rpt_Constancias.Datos;

namespace Presidencia.Rpt_Constancias.Negocio
{
    public class Cls_Rpt_Nom_Constancias_Negocio
    {
        #region VARIABLES LOCALES
            private String No_Empleado;
            private String Nombre_Empleado;
        #endregion

        #region VARIABLES GLOBALES
            //get y set de P_Nombre_Empleado
            public String P_Nombre_Empleado
            {
                get { return Nombre_Empleado; }
                set { Nombre_Empleado = value; }
            }

            //get y set de P_No_Empleado
            public String P_No_Empleado
            {
                get { return No_Empleado; }
                set { No_Empleado = value; }
        }
        #endregion

        #region METODOS
            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Datos_Empleado
            ///DESCRIPCIÓN          : Metodo para obtener los datos de los empleados
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 11/Abril/2012 
            ///*********************************************************************************************************
            public DataTable Consultar_Datos_Empleado()
            {
                return Cls_Rpt_Nom_Constancias_Datos.Consultar_Datos_Empleado(this);
            }

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Datos_Empleado_Baja
            ///DESCRIPCIÓN          : Metodo para obtener los datos de los empleados
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 11/Abril/2012 
            ///*********************************************************************************************************
            public DataTable Consultar_Datos_Empleado_Baja()
            {
                return Cls_Rpt_Nom_Constancias_Datos.Consultar_Datos_Empleado(this);
            }
        #endregion
    }
}
