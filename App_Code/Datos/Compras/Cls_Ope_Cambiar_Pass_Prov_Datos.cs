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
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Pass_Proveedor.Negocio;



/// <summary>
/// Summary description for Cls_Ope_Cambiar_Pass_Prov_Datos
/// </summary>
/// 
namespace Presidencia.Pass_Proveedor.Datos
{
    public class Cls_Ope_Cambiar_Pass_Prov_Datos
    {

        public static DataTable Consultar_Proveedor(Cls_Ope_Cambiar_Pass_Prov_Negocio Clase_Negocio)
        {
            String Mi_SQL = "";
            DataTable Dt_Proveedores = new DataTable();
            Mi_SQL = "SELECT " + Cat_Com_Proveedores.Campo_Nombre;
            Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Compañia;
            Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Password;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Proveedores.Campo_Proveedor_ID;
            Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Num_Proveedor.Trim() +"'";

            Dt_Proveedores = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Dt_Proveedores;

        }

        public static String Modificar_Password_Proveedor(Cls_Ope_Cambiar_Pass_Prov_Negocio Clase_Negocio)
        {
            String Mi_SQL = "";
            String Mensaje_Error = "";
            try
            {

                Mi_SQL = "UPDATE " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
                Mi_SQL = Mi_SQL + " SET " + Cat_Com_Proveedores.Campo_Password;
                Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Password_Nuevo.Trim() + "'";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Proveedores.Campo_Proveedor_ID;
                Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Num_Proveedor + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            }
            catch (Exception Ex)
            {
                Mensaje_Error = Ex.Message;

            }

            return Mensaje_Error;
        }

    }
}//fin del namespace