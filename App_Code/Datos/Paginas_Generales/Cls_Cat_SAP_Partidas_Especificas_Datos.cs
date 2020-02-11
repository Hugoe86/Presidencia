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
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.SAP_Partidas_Especificas.Negocio;

namespace Presidencia.SAP_Partidas_Especificas.Datos
{
    public class Cls_Cat_SAP_Partidas_Especificas_Datos
    {
        #region (Metodos Consulta)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Partida_Especifica
        /// DESCRIPCION : Consulta la partida especifica de acuerdo a la cuenta contable
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Salvador L. Rea Ayala
        /// FECHA_CREO  : 4/Octubre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Partida_Especifica(Cls_Cat_SAP_Partidas_Especificas_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta para la póliza            
            try
            {
                Mi_SQL = "SELECT " + Cat_Sap_Partidas_Especificas.Campo_Clave + ", ";
                Mi_SQL += Cat_Sap_Partidas_Especificas.Campo_Partida_ID + ", ";
                Mi_SQL += Cat_Sap_Partidas_Especificas.Campo_Nombre;
                Mi_SQL += " FROM " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas;

                if (!String.IsNullOrEmpty(Datos.P_Cuenta))
                    Mi_SQL += " WHERE " + Cat_Sap_Partidas_Especificas.Campo_Cuenta + " = '" + Datos.P_Cuenta + "'";

                if (!String.IsNullOrEmpty(Datos.P_Clave))
                    Mi_SQL += " WHERE " + Cat_Sap_Partidas_Especificas.Campo_Clave + " = '" + Datos.P_Clave + "'";

                if (!String.IsNullOrEmpty(Datos.P_Partida_ID))
                    Mi_SQL += " WHERE " + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + " = '" + Datos.P_Partida_ID + "'";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
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