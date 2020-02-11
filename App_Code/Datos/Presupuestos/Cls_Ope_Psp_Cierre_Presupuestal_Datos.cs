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
using System.Text;
using System.Collections.Generic;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Presupuesto_Cierre_Presupuestal.Negocio;

namespace Presidencia.Presupuesto_Cierre_Presupuestal.Datos
{
    public class Cls_Ope_Psp_Cierre_Presupuestal_Datos
    {
        #region(Metodos)
        /// ********************************************************************************************************************
        /// NOMBRE: Modificar_Cierre_Presupuestal
        /// 
        /// COMENTARIOS: Esta operación actualiza un registro  en la tabla de Ope_Psp_Cierre_Presup
        /// 
        /// PARÁMETROS: Datos.- Valor de los campos a Modificar en la tabla de Ope_Psp_Cierre_Presup 
        /// 
        /// USUARIO CREÓ: Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:  16/Noviembre/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static Boolean Modificar_Cierre_Presupuestal(Cls_Ope_Psp_Cierre_Presupuestal_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            OracleTransaction Transaccion = null;////Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        
            OracleConnection Conexion;//Variable para la conexión para la base de datos   
            OracleCommand Comando;//Sirve para la ejecución de las operaciones a la base de datos
            String Mensaje = String.Empty; //Variable que almacena el mensaje de estado de la operación
            Boolean Operacion_Completa = false;

            Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
            Comando = new OracleCommand();
            Conexion.Open();
            Transaccion = Conexion.BeginTransaction();
            Comando.Transaction = Transaccion;
            Comando.Connection = Conexion;
            try
            {
                Mi_SQL.Append("UPDATE " + Ope_Psp_Cierre_Presup.Tabla_Ope_Psp_Cierre_Presup + " SET ");
                Mi_SQL.Append(Ope_Psp_Cierre_Presup.Campo_Enero + "='" + Datos.P_Enero + "',");
                Mi_SQL.Append(Ope_Psp_Cierre_Presup.Campo_Febrero + "='" + Datos.P_Febrero + "',");
                Mi_SQL.Append(Ope_Psp_Cierre_Presup.Campo_Marzo + "='" + Datos.P_Marzo + "',");
                Mi_SQL.Append(Ope_Psp_Cierre_Presup.Campo_Abril + "='" + Datos.P_Abril + "',");
                Mi_SQL.Append(Ope_Psp_Cierre_Presup.Campo_Mayo + "='" + Datos.P_Mayo + "',");
                Mi_SQL.Append(Ope_Psp_Cierre_Presup.Campo_Junio + "='" + Datos.P_Junio + "',");
                Mi_SQL.Append(Ope_Psp_Cierre_Presup.Campo_Julio + "='" + Datos.P_Julio + "',");
                Mi_SQL.Append(Ope_Psp_Cierre_Presup.Campo_Agosto + "='" + Datos.P_Agosto + "',");
                Mi_SQL.Append(Ope_Psp_Cierre_Presup.Campo_Septiembre + "='" + Datos.P_Septiembre + "',");
                Mi_SQL.Append(Ope_Psp_Cierre_Presup.Campo_Octubre + "='" + Datos.P_Octubre + "',");
                Mi_SQL.Append(Ope_Psp_Cierre_Presup.Campo_Noviembre + "='" + Datos.P_Noviembre + "',");
                Mi_SQL.Append(Ope_Psp_Cierre_Presup.Campo_Diciembre + "='" + Datos.P_Diciembre + "',");
                Mi_SQL.Append(Ope_Psp_Cierre_Presup.Campo_Usuario_Modifico + "='" + Datos.P_Usuario_Creo+ "',");
                Mi_SQL.Append(Ope_Psp_Cierre_Presup.Campo_Fecha_Modifico + "="  +"SYSDATE ");   
                Mi_SQL.Append("WHERE " + Ope_Psp_Cierre_Presup.Campo_Anio +"=" + Datos.P_Anio);

                //Ejecutar consulta
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

                Conexion.Close();
                Operacion_Completa = true;
            }
            catch (OracleException Ex)
            {
                if (Transaccion != null)
                {
                    Transaccion.Rollback();
                }
                switch (Ex.Code.ToString())
                {
                    case "2291":
                        Mensaje = "Error: No existe un registro relacionado con esta operacion [" + Ex.Message + "]";
                        break;
                    case "923":
                        Mensaje = "Error: Consulta SQL [" + Ex.Message + "]";
                        break;
                    case "12170":
                        Mensaje = "Error: Conexion con el Servidor [" + Ex.Message + "]";
                        break;
                    default:
                        Mensaje = "Error:  [" + Ex.Message + "]";
                        break;
                }
                throw new Exception(Mensaje, Ex);
            }
            finally
            {
                Conexion.Close();
                Comando = null;
                Conexion = null;
                Transaccion = null;
            }
            return Operacion_Completa;
        }

            /// ********************************************************************************************************************
            /// NOMBRE: Consultar_Estatus
            /// 
            /// COMENTARIOS: Consulta el estatus de los meses de el año deseado
            /// 
            /// PARÁMETROS: Datos.- Valor de los campos a consultar en la tabla de OPE_PSP_CIERRE_PRESUP
            /// 
            /// USUARIO CREÓ: Hugo Enrique RamÍrez Aguilera
            /// FECHA CREÓ:  16/Noviembre/2011
            /// USUARIO MODIFICO:
            /// FECHA MODIFICO:
            /// CAUSA DE LA MODIFICACIÓN:
            /// ********************************************************************************************************************
            public static DataTable Consultar_Estatus(Cls_Ope_Psp_Cierre_Presupuestal_Negocio Datos)
            {
                StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
                DataTable Dt_Estatus = new DataTable();
                try
                {
                    Mi_SQL.Append("Select " + Ope_Psp_Cierre_Presup.Tabla_Ope_Psp_Cierre_Presup + ".* ");
                    Mi_SQL.Append("From " + Ope_Psp_Cierre_Presup.Tabla_Ope_Psp_Cierre_Presup);

                    if (!string.IsNullOrEmpty(Datos.P_Anio))
                    {
                        Mi_SQL.Append(" WHERE " + Ope_Psp_Cierre_Presup.Campo_Anio + "=" + Datos.P_Anio + "");
                    }

                    Dt_Estatus = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al consultar el estatus del año seleccionado dentro del sitema. Error: [" + Ex.Message + "]");
                }
                return Dt_Estatus;
            }
        #endregion

    }
}