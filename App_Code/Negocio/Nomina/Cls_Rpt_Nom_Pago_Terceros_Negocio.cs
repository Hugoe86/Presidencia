using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Presidencia.Pago_Terceros.Datos;

namespace Presidencia.Pago_Terceros.Negocio
{
    public class Cls_Rpt_Nom_Pago_Terceros_Negocio
    {
        #region VARIABLES LOCALES
            private String No_Empleado;
            private String Nombre_Empleado;
            private String Nomina;
            private String Periodo;
            private String Tipo_Orden_Judicial;
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

            //get y set de P_Nomina
            public String P_Nomina
            {
                get { return Nomina; }
                set { Nomina = value; }
            }

            //get y set de P_Periodo
            public String P_Periodo
        {
            get { return Periodo; }
            set { Periodo = value; }
        }

            //get y set de P_Periodo
            public String P_Tipo_Orden_Judicial
            {
                get { return Tipo_Orden_Judicial; }
                set { Tipo_Orden_Judicial = value; }
            }
        #endregion

        #region METODOS
            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Pago_Terceros
            ///DESCRIPCIÓN          : Metodo para obtener los datos del reporte de pago  a terceros
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 17/Enero/2012 
            ///*********************************************************************************************************
            public DataTable Consultar_Pago_Terceros()
            {
                return Cls_Rpt_Nom_Pago_Terceros_Datos.Consultar_Pago_Terceros(this);
            }

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Pago_Por_Deducciones_Diversas
            ///DESCRIPCIÓN          : Metodo para obtener los datos del reporte de pago por deducciones diversas
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 18/Enero/2012 
            ///*********************************************************************************************************
            public DataTable Consultar_Pago_Por_Deducciones_Diversas()
            {
                return Cls_Rpt_Nom_Pago_Terceros_Datos.Consultar_Pago_Por_Deducciones_Diversas(this);
            }

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Tipo_Orden_Judicial
            ///DESCRIPCIÓN          : Metodo para obtener los datos de los tipos de orden judicial
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 18/Enero/2012 
            ///*********************************************************************************************************
            public DataTable Consultar_Tipo_Orden_Judicial()
            {
                return Cls_Rpt_Nom_Pago_Terceros_Datos.Consultar_Tipos_Orden_Judicial();
            }
        #endregion
    }
}
