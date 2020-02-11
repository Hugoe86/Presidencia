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
using System.Collections.Generic;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using System.Text;
using Presidencia.Bitacora_Polizas.Negocio;

namespace Presidencia.Bitacora_Polizas.Datos
{
    public class Cls_Ope_Con_Bitacora_Polizas_Datos
    {
        #region (ALTA - MODIFICACION - ELIMINAR)
        public static void Alta_Bitacora(Cls_Ope_Con_Bitacora_Polizas_Negocio Datos)
        {
            String Mi_SQL;   //Variable de Consulta para la Alta del de una Nueva Mascara
            Object Parametros_ID; //Variable que contendrá el ID de la consulta

            try
            {
                //Busca el maximo ID de la tabla Parametros.
                Mi_SQL = "SELECT NVL(MAX (" + Ope_Con_Bitacora_Polizas.Campo_No_Bitacora + "),'00000') ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Con_Bitacora_Polizas.Tabla_Ope_Con_Bitacora_Polizas;
                Parametros_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(Parametros_ID)) //Si no existen valores en la tabla, asigna el primer valor manualmente.
                {
                    Datos.P_No_Bitacora = "00001";
                }
                else // Si ya existen registros, toma el valor maximo y le suma 1 para el nuevo registro.
                {
                    Datos.P_No_Bitacora = String.Format("{0:0000000000}", Convert.ToInt32(Parametros_ID) + 1);
                }
                //Da de Alta los datos del Nuevo Parametro con los datos proporcionados por el usuario.
                Mi_SQL = "INSERT INTO " + Ope_Con_Bitacora_Polizas.Tabla_Ope_Con_Bitacora_Polizas + " (";
                Mi_SQL = Mi_SQL + Ope_Con_Bitacora_Polizas.Campo_Cuenta_Contable_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Bitacora_Polizas.Campo_Debe + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Bitacora_Polizas.Campo_Fecha_Creo + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Bitacora_Polizas.Campo_Haber + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Bitacora_Polizas.Campo_Mes_Ano + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Bitacora_Polizas.Campo_No_Bitacora + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Bitacora_Polizas.Campo_No_Poliza + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Bitacora_Polizas.Campo_Tipo_Poliza_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Bitacora_Polizas.Campo_Usuario_Creo + ") ";
                Mi_SQL = Mi_SQL + "VALUES ('" + Datos.P_Cuenta_Contable_ID + "', ";
                Mi_SQL = Mi_SQL + Datos.P_Debe + ", ";
                Mi_SQL = Mi_SQL + "SYSDATE, ";
                Mi_SQL = Mi_SQL + Datos.P_Haber + ", '";
                Mi_SQL = Mi_SQL + Datos.P_Mes_Ano + "', '";
                Mi_SQL = Mi_SQL + Datos.P_No_Bitacora + "', '";
                Mi_SQL = Mi_SQL + Datos.P_No_Poliza + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Tipo_Poliza_ID + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Usuario_Creo + "')";

                //Manda Mi_SQL para ser procesada por ORACLE.
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
        }
        #endregion
    }
}
