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
using Presidencia.Empleados.Negocios;
using Presidencia.Sessiones;
using Presidencia.Bitacora_Eventos;
using Presidencia.DateDiff;
using System.Text;
using Presidencia.Nomina_Pago_Indemnizacion.Negocio;

namespace Presidencia.Nomina_Pago_Indemnizacion.Datos
{
    public class Cls_Cat_Nom_Pago_Indemnizacion_Datos
    {
         ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Modificar_Empleado_Tipo_Finiquito
        /// DESCRIPCION : Modifica el campo de Tipo_Finiquito del empleado
        /// PARAMETROS  : Datos: Contiene los datos que serán modificados en la base de datos
        /// CREO        : Hugo Enrique Ramirez Aguilera
        /// FECHA_CREO  : 04/Abril/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Modificar_Empleado_Tipo_Finiquito(Cls_Cat_Nom_Pago_Indemnizacion_Negocio Datos)
        {
            String Mi_SQL = "";

            try
            {
                Mi_SQL = "UPDATE " + Cat_Empleados.Tabla_Cat_Empleados + " SET ";

                if (Datos.P_Tipo_Finiquito != null)
                {
                    Mi_SQL += Cat_Empleados.Campo_Tipo_Finiquito + " = '" + Datos.P_Tipo_Finiquito + "' ";
                }

                Mi_SQL += " where " + Cat_Empleados.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'";

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
            finally
            {
            }
        }
    }
}
