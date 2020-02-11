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
using Presidencia.Listado_Tramites.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
/// <summary>
/// Summary 
/// </summary>
/// 
namespace Presidencia.Listado_Tramites.Datos
{
    public class Cls_Ope_Tra_Listado_Tramites_Datos
    {
        public Cls_Ope_Tra_Listado_Tramites_Datos()
        {

        }

        public static DataTable Consultar_Tramites(Cls_Ope_Tra_Listado_Tramites_Negocio Negocio) 
        {
            String Mi_Sql = "";
            Mi_Sql = "SELECT * FROM " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites;
            if (Negocio.P_Dependencia_ID !=  null && Negocio.P_Dependencia_ID.Trim().Length > 0) 
            {
                Mi_Sql += " WHERE " + 
                    Cat_Tra_Tramites.Campo_Dependencia_ID + " = '" + Negocio.P_Dependencia_ID + "'";
            }                
            DataTable Dt_Tabla = null;
            DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            if (_DataSet != null && _DataSet.Tables.Count > 0)
            {
                Dt_Tabla = _DataSet.Tables[0];
            }
            return Dt_Tabla;
        }
        public static DataTable Consultar_Unidades_Responsables(Cls_Ope_Tra_Listado_Tramites_Negocio Negocio)
        {
            String Mi_Sql = "";
            Mi_Sql = "SELECT " + Cat_Dependencias.Tabla_Cat_Dependencias + ".*," +
                Cat_Dependencias.Campo_Clave + "||' '||" + Cat_Dependencias.Campo_Comentarios + " AS CLAVE_NOMBRE" +
                " FROM " + 
                Cat_Dependencias.Tabla_Cat_Dependencias;
            if (Negocio.P_Tramite_ID != null && Negocio.P_Tramite_ID.Trim().Length > 0)
            {
                Mi_Sql += " WHERE " +
                Cat_Dependencias.Campo_Dependencia_ID + " = " +
                "(SELECT " + Cat_Tra_Tramites.Campo_Dependencia_ID + " FROM " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites +
                " WHERE " + Cat_Tra_Tramites.Campo_Tramite_ID + " = '" + Negocio.P_Tramite_ID + "')";
            }            
            DataTable Dt_Tabla = null;
            DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            if (_DataSet != null && _DataSet.Tables.Count > 0)
            {
                Dt_Tabla = _DataSet.Tables[0];
            }
            return Dt_Tabla;
        }
    }
}