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
using System.Data.OleDb;
using ADODB;
using Presidencia.Reloj_Checador.Negocios;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;

namespace Presidencia.Reloj_Checador.Datos
{
    public class Cls_Ope_Nom_Reloj_Checador_Datos
    {
        #region (Métodos)

        #region (Métodos Operación)
        public static Boolean Alta_Movimiento_Reloj_Checador(Cls_Ope_Nom_Reloj_Checador_Negocio Datos)
        {
            String Mi_Oracle = "";//Obtiene la cadena de inserción hacía la base de datos
            String Mensaje = ""; //Obtiene la descripción del error ocurrido durante la ejecución de Mi_SQL
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Object No_Movimiento; //Variable auxiliar
            Boolean Operacion_Completa = false;

            Cn.ConnectionString = Cls_Constantes.Str_Conexion;
            Cn.Open();
            //Esta inserción se realiza sin el Ayudante de SQL y con el BeginTrans y Commit para proteger la información
            //el ayudante de SQL solo debe usarse cuando solo se afecte una tabla o para movimientos que NO son críticos
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;

            try
            {
                //Consulta para el ID de la region
                Mi_Oracle = "SELECT NVL(MAX(" + Ope_Nom_Reloj_Checador.Campo_No_Movimiento + "), '0') FROM " +
                    Ope_Nom_Reloj_Checador.Tabla_Ope_Nom_Reloj_Checador;

                //Ejecutar consulta
                Cmd.CommandText = Mi_Oracle;
                No_Movimiento = Cmd.ExecuteScalar();

                //Verificar si no es nulo
                if (!(No_Movimiento is Nullable))
                {
                    Datos.P_No_Movimiento = Convert.ToInt32(No_Movimiento) + 1;
                }
                else
                {
                    Datos.P_No_Movimiento = Convert.ToInt32("1");
                }

                Mi_Oracle = "INSERT INTO " + Ope_Nom_Reloj_Checador.Tabla_Ope_Nom_Reloj_Checador + " ( " +
                            Ope_Nom_Reloj_Checador.Campo_No_Movimiento + ", " +
                            Ope_Nom_Reloj_Checador.Campo_Dependencia_ID + ", " +
                            Ope_Nom_Reloj_Checador.Campo_Fecha_Subio_Informacion + ", " +
                            Ope_Nom_Reloj_Checador.Campo_Nomina_ID + ", " +
                            Ope_Nom_Reloj_Checador.Campo_No_Nomina + ", " +
                            Ope_Nom_Reloj_Checador.Campo_Usuario_Creo + ", " +
                            Ope_Nom_Reloj_Checador.Campo_Fecha_Creo + ") VALUES(" +
                            "" + Datos.P_No_Movimiento + ", " +
                            "'" + Datos.P_Checador_Unidad_Responsable + "', " +
                            "SYSDATE, " +
                            "'" + Datos.P_Nomina_ID + "', " +
                            "" + Datos.P_No_Nomina + ", " +
                            "'" + Datos.P_Usuario_Creo + "', " +
                            "SYSDATE)";

                //Ejecutar la consulta
                Cmd.CommandText = Mi_Oracle;
                Cmd.ExecuteNonQuery();

                //Ejecutar transaccion
                Trans.Commit();
                Operacion_Completa = true;
            }
            catch (OracleException Ex)
            {
                Trans.Rollback();
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code.ToString().Equals("8152"))
                {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar";
                }
                else if (Ex.Code.ToString().Equals("2627"))
                {
                    if (Ex.Message.IndexOf("PRIMARY") != -1)
                    {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar";
                    }
                    else if (Ex.Message.IndexOf("UNIQUE") != -1)
                    {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor";
                    }
                    else
                    {
                        Mensaje = "Error general en la base de datos";
                    }
                }
                else if (Ex.Code.ToString().Equals("547"))
                {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla";
                }
                else if (Ex.Code.ToString().Equals("515"))
                {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar";
                }
                else
                {
                    Mensaje = Ex.Message; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            catch (DBConcurrencyException Ex)
            {
                //Indicamos el mensaje 
                throw new Exception("Lo siento, los datos fueron actualizados por otro Rol. Error: [" + Ex.Message + "]");

            }
            catch (Exception Ex)
            {
                //Indicamos el mensaje 
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                Cn.Close();
            }
            return Operacion_Completa;
        }
        #endregion

        #region (Métodos Consulta)
        public static DataTable Consultar_Reloj_Checador(Cls_Ope_Nom_Reloj_Checador_Negocio Datos)
        {
            OleDbConnection Obj_Conexion = null;
            OleDbCommand Obj_Comando = null;
            OleDbDataAdapter Obj_Adapatador = null;
            DataTable Dt_Checadas = null;
            String Mi_SQL = String.Empty;

            try
            {
                Mi_SQL = "SELECT CHECKINOUT.USERID AS NO_EMPLEADO, CHECKINOUT.CHECKTIME AS FECHA FROM CHECKINOUT ORDER BY CHECKINOUT.CHECKTIME ASC";

                Obj_Conexion = new OleDbConnection(Datos.P_Cadena_Conexion);
                Obj_Conexion.Open();

                Obj_Comando = new OleDbCommand();
                Obj_Comando.Connection = Obj_Conexion;
                Obj_Comando.CommandType = CommandType.Text;
                Obj_Comando.CommandText = Mi_SQL;


                Obj_Adapatador = new OleDbDataAdapter();
                Obj_Adapatador.SelectCommand = Obj_Comando;
                
                Dt_Checadas = new DataTable();
                Obj_Adapatador.Fill(Dt_Checadas);
                Obj_Conexion.Close();
            }
            catch (OleDbException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            return Dt_Checadas;
        }
        #endregion

        #endregion
    }
}
