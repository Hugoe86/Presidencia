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
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Ajuste_Presupuesto.Negocio;
using System.Text;


namespace Presidencia.Ajuste_Presupuesto.Datos
{
    
    public class Cls_Ope_Psp_Ajuste_Presupuesto_Datos
    {
        
        #region(Metodos)

            public static Boolean Modificar_Presupuesto(Cls_Ope_Psp_Ajuste_Presupuesto_Negocio Datos)
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
                    Mi_SQL.Append("UPDATE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + " SET ");

                    if (!string.IsNullOrEmpty(Datos.P_Monto_Ampliacion))
                    {
                        Mi_SQL.Append(Cat_Com_Dep_Presupuesto.Campo_Monto_Ampliacion + "=" + Datos.P_Monto_Ampliacion + ",");
                    }
                    if (!string.IsNullOrEmpty(Datos.P_Monto_Reduccion))
                    {
                        Mi_SQL.Append(Cat_Com_Dep_Presupuesto.Campo_Monto_Reduccion + "=" + Datos.P_Monto_Reduccion + ",");
                    }
                    if (!string.IsNullOrEmpty(Datos.P_Monto_Incremento))
                    {
                        Mi_SQL.Append(Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible + "=" + Datos.P_Monto_Incremento + ",");
                    }
                    Mi_SQL.Append(Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible +"=" +Datos.P_Monto_Disponible +",");
                    Mi_SQL.Append(Cat_Com_Dep_Presupuesto.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario_Creo+ "', ");
                    Mi_SQL.Append(Cat_Com_Dep_Presupuesto.Campo_Fecha_Modifico + " = SYSDATE ");
                    Mi_SQL.Append(" where " + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID + "=" + Datos.P_Fuente_Financiamiento_Id );
                    Mi_SQL.Append(" And " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + "=" + Datos.P_Unidad_Responsable_Id );
                    Mi_SQL.Append(" And " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID + "=" + Datos.P_Programa_Id );
                    Mi_SQL.Append(" And " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID + "=" + Datos.P_Partida_Id );
                    
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
            
            public static DataTable Consultar_Presupuesto(Cls_Ope_Psp_Ajuste_Presupuesto_Negocio Datos)
            {
                StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
                DataTable Dt_Presupuesto = new DataTable();
                try 
                {
                    Mi_SQL.Append("Select * ");
                    Mi_SQL.Append("From " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto +" ");
                    Mi_SQL.Append(" where " + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID + "=" + Datos.P_Fuente_Financiamiento_Id + "");
                    Mi_SQL.Append(" And " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + "=" + Datos.P_Unidad_Responsable_Id + "");
                    Mi_SQL.Append(" And " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID + "=" + Datos.P_Programa_Id + "");
                    Mi_SQL.Append(" And " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID + "=" + Datos.P_Partida_Id + "");
              
                    Dt_Presupuesto = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
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
                return Dt_Presupuesto;
            }
        #endregion
    }
}