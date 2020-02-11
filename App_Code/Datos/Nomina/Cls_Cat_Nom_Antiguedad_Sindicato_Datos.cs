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
using Presidencia.Antiguedad_Sindicato.Negocio;

namespace Presidencia.Antiguedad_Sindicato.Datos
{
    public class Cls_Cat_Nom_Antiguedad_Sindicato_Datos
    {
        #region(Metodos)

        #region(Metodos Consulta)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Antiguedad_Sindicato
        /// DESCRIPCION : Consulta todas las antiguedades de Sindicatos que se encuentran dadas de alta
        ///               en el sistema.
        ///               
        /// PARAMETROS  : Datos: Variable que almacena todos la informacion que será almacenada
        ///                      en la tabla de Cat_Nom_Antiguedad_Sindicato
        ///                      
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 24/Diciembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Antiguedad_Sindicato(Cls_Cat_Nom_Antiguedad_Sindicato_Negocio Datos)
        {
            DataTable Dt_Antiguedades_Sindicato = null;//Variable que almacenara una lista de las antiguedades que evaluaran los sindicatos.
            String Mi_Oracle = "";//Variable que almacenara la consulta.
            try
            {
                Mi_Oracle = "SELECT " + Cat_Nom_Antiguedad_Sindicato.Tabla_Cat_Nom_Antiguedad_Sindicato + ".* " +
                            " FROM " +
                            Cat_Nom_Antiguedad_Sindicato.Tabla_Cat_Nom_Antiguedad_Sindicato;

                if (!string.IsNullOrEmpty(Datos.P_Antiguedad_Sindicato_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " OR " + Cat_Nom_Antiguedad_Sindicato.Tabla_Cat_Nom_Antiguedad_Sindicato + "." + Cat_Nom_Antiguedad_Sindicato.Campo_Antiguedad_Sindicato_ID + "='" + Datos.P_Antiguedad_Sindicato_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Nom_Antiguedad_Sindicato.Tabla_Cat_Nom_Antiguedad_Sindicato+ "." + Cat_Nom_Antiguedad_Sindicato.Campo_Antiguedad_Sindicato_ID + "='" + Datos.P_Antiguedad_Sindicato_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Anios.ToString()))
                {
                    if (Datos.P_Anios > 0)
                    {
                        if (Mi_Oracle.Contains("WHERE"))
                        {
                            Mi_Oracle += " OR " + Cat_Nom_Antiguedad_Sindicato.Tabla_Cat_Nom_Antiguedad_Sindicato + "." + Cat_Nom_Antiguedad_Sindicato.Campo_Anios + "='" + Datos.P_Anios + "'";
                        }
                        else
                        {
                            Mi_Oracle += " WHERE " + Cat_Nom_Antiguedad_Sindicato.Tabla_Cat_Nom_Antiguedad_Sindicato + "." + Cat_Nom_Antiguedad_Sindicato.Campo_Anios + "='" + Datos.P_Anios + "'";
                        }
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Comentarios))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " OR " + Cat_Nom_Antiguedad_Sindicato.Tabla_Cat_Nom_Antiguedad_Sindicato + "." + Cat_Nom_Antiguedad_Sindicato.Campo_Comentarios + "='" + Datos.P_Comentarios + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Nom_Antiguedad_Sindicato.Tabla_Cat_Nom_Antiguedad_Sindicato + "." + Cat_Nom_Antiguedad_Sindicato.Campo_Comentarios+ "='" + Datos.P_Comentarios + "'";
                    }
                }

                Dt_Antiguedades_Sindicato = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las antiguedades de sindicatos. Error: [" + Ex.Message + "]");
            }
            return Dt_Antiguedades_Sindicato;
        }
        #endregion

        #region(Metodos Operacion)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Antiguedad_Sindicato
        /// DESCRIPCION : Ejecuta el alta de un registro de la tabla de Cat_Nom_Antiguedad_Sindicato
        /// 
        /// PARAMETROS  : Datos: Variable que almacena todos la informacion que será almacenada
        ///                      en la tabla de Cat_Nom_Antiguedad_Sindicato
        ///                      
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 24/Diciembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Alta_Antiguedad_Sindicato(Cls_Cat_Nom_Antiguedad_Sindicato_Negocio Datos)
        {
            String Mi_Oracle = "";//Obtiene la cadena de inserción hacía la base de datos
            String Mensaje = ""; //Obtiene la descripción del error ocurrido durante la ejecución de Mi_SQL
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Object Antiguedad_Sindicato_ID; //Variable auxiliar
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
                Mi_Oracle = "SELECT NVL(MAX(" + Cat_Nom_Antiguedad_Sindicato.Campo_Antiguedad_Sindicato_ID + "), '00000') FROM " +
                    Cat_Nom_Antiguedad_Sindicato.Tabla_Cat_Nom_Antiguedad_Sindicato;

                //Ejecutar consulta
                Cmd.CommandText = Mi_Oracle;
                Antiguedad_Sindicato_ID = Cmd.ExecuteScalar();

                //Verificar si no es nulo
                if (!(Antiguedad_Sindicato_ID is Nullable))
                {
                    Datos.P_Antiguedad_Sindicato_ID = string.Format("{0:00000}", (Convert.ToInt32(Antiguedad_Sindicato_ID) + 1));
                }
                else
                {
                    Datos.P_Antiguedad_Sindicato_ID = "00001";
                }

                Mi_Oracle = "INSERT INTO " + Cat_Nom_Antiguedad_Sindicato.Tabla_Cat_Nom_Antiguedad_Sindicato + " ( " +
                            Cat_Nom_Antiguedad_Sindicato.Campo_Antiguedad_Sindicato_ID + ", " +
                            Cat_Nom_Antiguedad_Sindicato.Campo_Anios + ", " +
                            Cat_Nom_Antiguedad_Sindicato.Campo_Comentarios + ", " +
                            Cat_Nom_Antiguedad_Sindicato.Campo_Usuario_Creo + ", " +
                            Cat_Nom_Antiguedad_Sindicato.Campo_Fecha_Creo + ") VALUES(" +
                            "'" + Datos.P_Antiguedad_Sindicato_ID + "', " +
                            "" + Datos.P_Anios + ", " +
                            "'" + Datos.P_Comentarios + "', " +
                            "'" + Datos.P_Usuario_Creo + "', SYSDATE)"; 

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
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Modificar_Antiguedad_Sindicato
        /// DESCRIPCION : Ejecuta el actualización de un registro de la tabla de Cat_Nom_Antiguedad_Sindicato
        /// 
        /// PARAMETROS  : Datos: Variable que almacena todos la informacion que será almacenada
        ///                      en la tabla de Cat_Nom_Antiguedad_Sindicato
        ///                      
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 24/Diciembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Modificar_Antiguedad_Sindicato(Cls_Cat_Nom_Antiguedad_Sindicato_Negocio Datos)
        {
            String Mi_Oracle = "";//Obtiene la cadena de inserción hacía la base de datos
            String Mensaje = ""; //Obtiene la descripción del error ocurrido durante la ejecución de Mi_SQL
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
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
                Mi_Oracle = "UPDATE " + 
                            Cat_Nom_Antiguedad_Sindicato.Tabla_Cat_Nom_Antiguedad_Sindicato + 
                            " SET " +
                            Cat_Nom_Antiguedad_Sindicato.Campo_Anios + " =" + Datos.P_Anios + ", " +
                            Cat_Nom_Antiguedad_Sindicato.Campo_Comentarios + " ='" + Datos.P_Comentarios + "', " +
                            Cat_Nom_Antiguedad_Sindicato.Campo_Usuario_Modifico + " ='" + Datos.P_Usuario_Modifico + "', " +
                            Cat_Nom_Antiguedad_Sindicato.Campo_Fecha_Modifico + "=SYSDATE " +
                            " WHERE " +
                            Cat_Nom_Antiguedad_Sindicato.Campo_Antiguedad_Sindicato_ID + "='" + Datos.P_Antiguedad_Sindicato_ID + "'";
                            
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
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Eliminar_Antiguedad_Sindicato
        /// DESCRIPCION :  Ejecuta el baja de un registro de la tabla de Cat_Nom_Antiguedad_Sindicato
        /// 
        /// PARAMETROS  : Datos: Variable que almacena todos la informacion que será almacenada
        ///                      en la tabla de Cat_Nom_Antiguedad_Sindicato
        ///                      
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 24/Diciembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Eliminar_Antiguedad_Sindicato(Cls_Cat_Nom_Antiguedad_Sindicato_Negocio Datos)
        {
            String Mi_Oracle = "";//Obtiene la cadena de inserción hacía la base de datos
            String Mensaje = ""; //Obtiene la descripción del error ocurrido durante la ejecución de Mi_SQL
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
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
                Mi_Oracle = "DELETE FROM " +
                            Cat_Nom_Antiguedad_Sindicato.Tabla_Cat_Nom_Antiguedad_Sindicato +
                            " WHERE " +
                            Cat_Nom_Antiguedad_Sindicato.Campo_Antiguedad_Sindicato_ID + "='" + Datos.P_Antiguedad_Sindicato_ID + "'";

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

        #endregion
    }
}