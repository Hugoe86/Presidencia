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
using Presidencia.Cuentas_Contables_Fijas.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
/// <summary>
/// Summary description for Cls_Ope_Con_Cuentas_Fijas_Datos
/// </summary>
/// 
namespace Presidencia.Cuentas_Contables_Fijas.Datos
{
    public class Cls_Ope_Con_Cuentas_Fijas_Datos
    {
        public Cls_Ope_Con_Cuentas_Fijas_Datos()
        {

        }
        #region METODOS

        public static DataTable Consultar_Cuentas_Contables(Cls_Ope_Con_Cuentas_Fijas_Negocio Negocio) 
        {
            DataTable _DataTable = null;
            String Mi_Sql = "SELECT * FROM " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables;
            try
            {
                DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    _DataTable = _DataSet.Tables[0];
                }
            }
            catch (Exception Ex) 
            {
                String Str_Ex = Ex.ToString();
                _DataTable = null;
            }
            return _DataTable;
        }

        public static int Guardar_Cuentas_Fijas(Cls_Ope_Con_Cuentas_Fijas_Negocio Negocio)
        {
            int Renglones_Afectados = 0;
            String Mi_Sql = "";
            
            try
            {
                //DataTable Dt_cuentas_Contables = Consultar_Cuentas_Contables(Negocio);
                //if (Dt_cuentas_Contables != null)
                //{
                    Mi_Sql = "UPDATE " + Cat_Con_Cuentas_Fijas.Tabla_Cat_Con_Cuentas_Fijas +
                        " SET " +
                        Cat_Con_Cuentas_Fijas.Campo_Almacen_General + " = '" + Negocio.P_Cuenta_Almacen_General + "', " +
                        Cat_Con_Cuentas_Fijas.Campo_Iva_Acreditable_Compras + " = '" + Negocio.P_Cuenta_Iva_Acreditable_Compras + "'";
                    Renglones_Afectados = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                //}
                //else 
                //{
                //    Mi_Sql = "INSERT INTO " + Cat_Con_Cuentas_Fijas.Tabla_Cat_Con_Cuentas_Fijas +
                //        " ( " + Cat_Con_Cuentas_Fijas.Campo_Almacen_General + "," +
                //        Cat_Con_Cuentas_Fijas.Campo_Iva_Acreditable_Compras + ")" +
                //        " VALUES ('" + Negocio.P_Cuenta_Almacen_General + "','" + Negocio.P_Cuenta_Iva_Acreditable_Compras + "')";
                //    Renglones_Afectados = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql); 
                //}
            }
            catch(Exception Ex)
            {
                String Str_Ex = Ex.ToString();
                Renglones_Afectados = 0;
            }                
            return Renglones_Afectados;
        }
        public static DataTable Consultar_Cuentas_Fijas(Cls_Ope_Con_Cuentas_Fijas_Negocio Negocio)
        {
            DataTable _DataTable = null;
            String Mi_Sql = "SELECT * FROM " + Cat_Con_Cuentas_Fijas.Tabla_Cat_Con_Cuentas_Fijas;
            DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
            {
                _DataTable = _DataSet.Tables[0];
            }
            return _DataTable;
        }

        #endregion

    }
}