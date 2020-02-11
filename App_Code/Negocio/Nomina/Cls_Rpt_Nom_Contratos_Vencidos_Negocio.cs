using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Presidencia.Contratos_Vencidos.Datos;

namespace Presidencia.Contratos_Vencidos.Negocio
{
    public class Cls_Rpt_Nom_Contratos_Vencidos_Negocio
    {
        #region VARIABLES LOCALES
            private String Fecha_Inicio;
            private String Fecha_Fin;
            private String No_Empleado;
            private String Nombre_Empleado;
            private String Tipo_Contrato;
            private String Nomina;
            private String Periodo;
            private String Tipo_Nomina;
        #endregion

        #region VARIABLES GLOBALES
            //get y set de P_Fecha_Inicio
            public String P_Fecha_Inicio
            {
                get { return Fecha_Inicio; }
                set { Fecha_Inicio = value; }
            }

            //get y set de P_Fecha_Fin
            public String P_Fecha_Fin
            {
                get { return Fecha_Fin; }
                set { Fecha_Fin = value; }
            }

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

            //get y set de P_Tipo_Contrato
            public String P_Tipo_Contrato
            {
                get { return Tipo_Contrato; }
                set { Tipo_Contrato = value; }
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

            //get y set de P_Tipo_Nomina
            public String P_Tipo_Nomina
            {
                get { return Tipo_Nomina; }
                set { Tipo_Nomina = value; }
            }
        #endregion

        #region METODOS
            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Contratos_Vencidos
            ///DESCRIPCIÓN          : Metodo para obtener los datos de los contratos vencidos
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 13/Enero/2012 
            ///*********************************************************************************************************
            public DataTable Consultar_Contratos_Vencidos()
            {
                return Cls_Rpt_Nom_Contratos_Vencidos_Datos.Consultar_Contratos_Vencidos(this);
            }

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Sueldos_Netos
            ///DESCRIPCIÓN          : Metodo para obtener los datos de los sueldos netos
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 14/Enero/2012 
            ///*********************************************************************************************************
            public DataTable Consultar_Sueldos_Netos()
            {
                return Cls_Rpt_Nom_Contratos_Vencidos_Datos.Consultar_Sueldos_Netos(this);
            }

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Totales_Efectivo
            ///DESCRIPCIÓN          : Metodo para obtener los datos de los totales efectivo
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 14/Enero/2012 
            ///*********************************************************************************************************
            public DataTable Consultar_Totales_Efectivo()
            {
                return Cls_Rpt_Nom_Contratos_Vencidos_Datos.Consultar_Totales_Efectivo(this);
            }
        #endregion
    }
}
