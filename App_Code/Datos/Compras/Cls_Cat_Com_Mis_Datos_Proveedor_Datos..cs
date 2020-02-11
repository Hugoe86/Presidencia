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
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
using Presidencia.Mis_Datos_Proveedor.Negocio;



/// <summary>
/// Summary description for Cls_Cat_Com_Mis_Datos_Proveedor_Datos
/// </summary>
/// 
namespace Presidencia.Mis_Datos_Proveedor.Datos
{
    public class Cls_Cat_Com_Mis_Datos_Proveedor_Datos
    {

        ///*******************************************************************************
        ///METODOS
        ///******************************************************************************

        #region Metodos

        public static DataTable Consultar_Datos_Proveedor(Cls_Cat_Com_Mis_Datos_Proveedor_Negocio Datos_Negocio)
        {
            String Mi_SQL = "";
            Mi_SQL = "SELECT " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + ".*";
            Mi_SQL = Mi_SQL + ", (SELECT " + Cat_Con_Cuentas_Contables.Campo_Cuenta;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID;
            Mi_SQL = Mi_SQL + " =" + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Cuenta+") AS CUENTA_CONTABLE";
            Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Proveedores.Campo_Proveedor_ID;
            Mi_SQL = Mi_SQL + " ='" + Datos_Negocio.P_Proveedor_ID + "'";


            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

        public static DataTable Consultar_Conceptos_Proveedor(Cls_Cat_Com_Mis_Datos_Proveedor_Negocio Datos_Negocio)
        {
            String Mi_SQL = "";
            Mi_SQL = "SELECT GIRO." + Cat_Com_Giro_Proveedor.Campo_Proveedor_ID + ",";
            Mi_SQL = Mi_SQL + "(SELECT " + Cat_Sap_Concepto.Campo_Clave + "||' '||" + Cat_Sap_Concepto.Campo_Descripcion;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + " WHERE " + Cat_Sap_Concepto.Campo_Concepto_ID;
            Mi_SQL = Mi_SQL + "=GIRO." + Cat_Com_Giro_Proveedor.Campo_Giro_ID + ") AS CONCEPTO";
            Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Giro_Proveedor.Tabla_Cat_Com_Giro_Proveedor + " GIRO";
            Mi_SQL = Mi_SQL + " WHERE GIRO." + Cat_Com_Giro_Proveedor.Campo_Proveedor_ID;
            Mi_SQL = Mi_SQL + "='" + Datos_Negocio.P_Proveedor_ID + "'";


            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

        }


        #endregion



    }//fin del class

}//fin del namespace
