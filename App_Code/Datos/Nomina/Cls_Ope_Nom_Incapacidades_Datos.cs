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
using Presidencia.Incapacidades.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using System.Text;

namespace Presidencia.Incapacidades.Datos
{
    public class Cls_Ope_Nom_Incapacidades_Datos
    {
        #region(Métodos Operación)
        /// ********************************************************************************************************************
        /// NOMBRE: Alta_Incapacidad
        /// 
        /// DESCRIPCIÓN: Ejecuta el alta de una Incapacidad.
        /// 
        /// PARÁMETROS: Datos: Información a guardar en la base de datos.
        /// 
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 06/Abril/2011 16:59 pm.
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA MODIFICACION:
        /// ********************************************************************************************************************
        public static Boolean Alta_Incapacidad(Cls_Ope_Nom_Incapacidades_Negocio Datos)
        {
            StringBuilder Mi_SQL =new StringBuilder();//Obtiene la cadena de inserción hacía la base de datos
            String Mensaje = ""; //Obtiene la descripción del error ocurrido durante la ejecución de Mi_SQL
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Object No_Incapacidad; //Variable auxiliar
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
                Mi_SQL.Append("SELECT NVL(MAX(" + Ope_Nom_Incapacidades.Campo_No_Incapacidad + "), '0000000000') FROM ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades);

                //Ejecutar consulta
                Cmd.CommandText = Mi_SQL.ToString();
                No_Incapacidad = Cmd.ExecuteScalar();

                //Verificar si no es nulo
                if (!(No_Incapacidad is Nullable))
                {
                    Datos.P_No_Incapacidad = string.Format("{0:0000000000}", (Convert.ToInt32(No_Incapacidad) + 1));
                }
                else
                {
                    Datos.P_No_Incapacidad = "0000000001";
                }

                Mi_SQL = new StringBuilder();

                Mi_SQL.Append("INSERT INTO " + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades);
                Mi_SQL.Append(" (" + Ope_Nom_Incapacidades.Campo_No_Incapacidad + ", ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_Empleado_ID + ", ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_Dependencia_ID + ", ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_Estatus + ", ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_Tipo_Incapacidad + ", ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_Aplica_Pago_Cuarto_Dia + ", ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_Porcentaje_Incapacidad + ", ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_Extencion_Incapacidad + ", ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_Fecha_Inicio + ", ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_Fecha_Fin + ", ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_Comentario + ", ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_Nomina_ID + ", ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_No_Nomina + ", ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_Usuario_Creo + ", ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_Fecha_Creo + ", ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_Dias_Incapacidad + ") VALUES(");
                Mi_SQL.Append("'" + Datos.P_No_Incapacidad + "', ");
                Mi_SQL.Append("'" + Datos.P_Empleado_ID + "', ");
                Mi_SQL.Append("'" + Datos.P_Dependencia_ID + "', ");
                Mi_SQL.Append("'" + Datos.P_Estatus + "', ");
                Mi_SQL.Append("'" + Datos.P_Tipo_Incapacidad + "', ");
                Mi_SQL.Append("'" + Datos.P_Aplica_Pago_Cuarto_Dia + "', ");
                Mi_SQL.Append(Datos.P_Porcentaje_Incapacidad + ", ");
                Mi_SQL.Append("'" + Datos.P_Extencion_Incapacidad + "', ");
                Mi_SQL.Append("'" + Datos.P_Fecha_Inicio_Incapacidad + "', ");
                Mi_SQL.Append("'" + Datos.P_Fecha_Fin_Incapacidad + "', ");
                Mi_SQL.Append("'" + Datos.P_Comentarios + "', ");
                Mi_SQL.Append("'" + Datos.P_Nomina_ID + "', ");
                Mi_SQL.Append(Datos.P_No_Nomina + ", ");
                Mi_SQL.Append("'" + Datos.P_Usuario_Creo + "', ");
                Mi_SQL.Append("SYSDATE, " + Datos.P_Dias_Incapacidad + ")");

                //Ejecutar la consulta
                Cmd.CommandText = Mi_SQL.ToString();
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
        /// ********************************************************************************************************************
        /// NOMBRE: Modificar_Incapacidad
        /// 
        /// DESCRIPCIÓN: Ejecuta la modificación de una Incapacidad.
        /// 
        /// PARÁMETROS: Datos: Información a actualizar en la base de datos.
        /// 
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 06/Abril/2011 17:11 pm.
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA MODIFICACION:
        /// ********************************************************************************************************************
        public static Boolean Modificar_Incapacidad(Cls_Ope_Nom_Incapacidades_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Obtiene la cadena de inserción hacía la base de datos
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
                Mi_SQL.Append("UPDATE " + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + " SET ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_Estatus + "='" + Datos.P_Estatus + "', ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_Tipo_Incapacidad + "='" + Datos.P_Tipo_Incapacidad + "', ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_Aplica_Pago_Cuarto_Dia + "='" + Datos.P_Aplica_Pago_Cuarto_Dia + "', ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_Porcentaje_Incapacidad + "=" + Datos.P_Porcentaje_Incapacidad + ", ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_Extencion_Incapacidad + "='" + Datos.P_Extencion_Incapacidad + "', ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_Fecha_Inicio + "='" + Datos.P_Fecha_Inicio_Incapacidad + "', ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_Fecha_Fin + "='" + Datos.P_Fecha_Fin_Incapacidad + "', ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_Comentario + "='" + Datos.P_Comentarios + "', ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "', ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_No_Nomina + "=" + Datos.P_No_Nomina + ", ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_Dias_Incapacidad + "=" + Datos.P_Dias_Incapacidad + ", ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_Usuario_Modifico + "='" + Datos.P_Usuario_Modifico + "', ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_Fecha_Modifico + "=SYSDATE");
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_No_Incapacidad + "='" + Datos.P_No_Incapacidad + "'");

                //Ejecutar la consulta
                Cmd.CommandText = Mi_SQL.ToString();
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
        /// ********************************************************************************************************************
        /// NOMBRE: Eliminar_Incapacidad
        /// 
        /// DESCRIPCIÓN: Ejecuta la baja de una Incapacidad.
        /// 
        /// PARÁMETROS: Datos: Información a actualizar en la base de datos.
        /// 
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 06/Abril/2011 17:11 pm.
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA MODIFICACION:
        /// ********************************************************************************************************************
        public static Boolean Eliminar_Incapacidad(Cls_Ope_Nom_Incapacidades_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Obtiene la cadena de inserción hacía la base de datos
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
                Mi_SQL.Append("DELETE FROM " + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + " WHERE ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_No_Incapacidad + "='" + Datos.P_No_Incapacidad + "'");

                //Ejecutar la consulta
                Cmd.CommandText = Mi_SQL.ToString();
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
        /// ********************************************************************************************************************
        /// NOMBRE: Modificar_Incapacidad
        /// 
        /// DESCRIPCIÓN: Ejecuta la modificación de una Incapacidad.
        /// 
        /// PARÁMETROS: Datos: Información a actualizar en la base de datos.
        /// 
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 06/Abril/2011 17:11 pm.
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA MODIFICACION:
        /// ********************************************************************************************************************
        public static Boolean Cambiar_Estatus_Incapacidad(Cls_Ope_Nom_Incapacidades_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Obtiene la cadena de inserción hacía la base de datos
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
                Mi_SQL.Append("UPDATE " + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + " SET ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_Estatus + "='" + Datos.P_Estatus + "', ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_Usuario_Modifico + "='" + Datos.P_Usuario_Modifico + "', ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_Fecha_Modifico + "=SYSDATE");
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_No_Incapacidad + "='" + Datos.P_No_Incapacidad + "'");

                //Ejecutar la consulta
                Cmd.CommandText = Mi_SQL.ToString();
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

        #region(Métodos Consulta)
        /// ********************************************************************************************************************
        /// NOMBRE: Consultar_Incapacidades
        /// 
        /// DESCRIPCIÓN: Consulta las incapacidades que existen registradas actualmente en el sistema.
        /// 
        /// PARÁMETROS: Datos: Información a actualizar en la base de datos.
        /// 
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 06/Abril/2011 17:11 pm.
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA MODIFICACION:
        /// ********************************************************************************************************************
        public static DataTable Consultar_Incapacidades(Cls_Ope_Nom_Incapacidades_Negocio Datos)
        {
            DataTable Dt_Incapacidades = null;//Variable que almacenara una lista de las antiguedades que evaluaran los sindicatos.
            String Mi_SQL = "";//Variable que almacenara la consulta.
            try
            {
                Mi_SQL = "SELECT " + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + ".*, " +
                                Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA, " +
                                " (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + "|| ' ' ||" +
                                Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "|| ' ' || " +
                                Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ") As Nombre" +
                            " FROM " +
                                Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + ", " + Cat_Dependencias.Tabla_Cat_Dependencias +
                                ", " + Cat_Empleados.Tabla_Cat_Empleados +
                            " WHERE (" +
                                Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_Dependencia_ID +
                            "=" +
                                Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + ") AND " +
                            "(" + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_Empleado_ID + "=" +
                            Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + ")";

                if (!string.IsNullOrEmpty(Datos.P_No_Incapacidad))
                {
                    if (Mi_SQL.Contains("WHERE"))
                    {
                        Mi_SQL += " AND " + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_No_Incapacidad + "='" + Datos.P_No_Incapacidad + "'";
                    }
                    else
                    {
                        Mi_SQL += " WHERE " + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_No_Incapacidad + "='" + Datos.P_No_Incapacidad + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Empleado_ID))
                {
                    if (Mi_SQL.Contains("WHERE"))
                    {
                        Mi_SQL += " AND " + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'";
                    }
                    else
                    {
                        Mi_SQL += " WHERE " + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Dependencia_ID))
                {
                    if (Mi_SQL.Contains("WHERE"))
                    {
                        Mi_SQL += " AND " + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'";
                    }
                    else
                    {
                        Mi_SQL += " WHERE " + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Estatus))
                {
                    if (Mi_SQL.Contains("WHERE"))
                    {
                        Mi_SQL += " AND " + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_Estatus + "='" + Datos.P_Estatus + "'";
                    }
                    else
                    {
                        Mi_SQL += " WHERE " + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_Estatus + "='" + Datos.P_Estatus + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Nomina_ID))
                {
                    if (Mi_SQL.Contains("WHERE"))
                    {
                        Mi_SQL += " AND " + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "'";
                    }
                    else
                    {
                        Mi_SQL += " WHERE " + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "'";
                    }
                }

                if (Datos.P_No_Nomina is Int32 && Datos.P_No_Nomina > 0)
                {
                    if (Mi_SQL.Contains("WHERE"))
                    {
                        Mi_SQL += " AND " + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_No_Nomina + "=" + Datos.P_No_Nomina;
                    }
                    else
                    {
                        Mi_SQL += " WHERE " + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_No_Nomina + "=" + Datos.P_No_Nomina;
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Fecha_Inicio_Incapacidad) && !string.IsNullOrEmpty(Datos.P_Fecha_Fin_Incapacidad))
                {
                    if (Mi_SQL.Contains("WHERE"))
                    {
                        Mi_SQL += " AND " + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_Fecha_Inicio + " BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio_Incapacidad + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + Datos.P_Fecha_Fin_Incapacidad + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                    }
                    else
                    {
                        Mi_SQL += " WHERE " + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_Fecha_Inicio + " BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio_Incapacidad + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + Datos.P_Fecha_Fin_Incapacidad + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Fecha_Inicio_Incapacidad) && !string.IsNullOrEmpty(Datos.P_Fecha_Fin_Incapacidad))
                {
                    if (Mi_SQL.Contains("WHERE"))
                    {
                        Mi_SQL += " AND " + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_Fecha_Fin + " BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio_Incapacidad + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + Datos.P_Fecha_Fin_Incapacidad + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                    }
                    else
                    {
                        Mi_SQL += " WHERE " + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_Fecha_Fin + " BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio_Incapacidad + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + Datos.P_Fecha_Fin_Incapacidad + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                    }
                }

                Mi_SQL += " ORDER BY " + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_No_Incapacidad + " DESC";

                Dt_Incapacidades = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las Incapacidades. Error: [" + Ex.Message + "]");
            }
            return Dt_Incapacidades;
        }
        /// ********************************************************************************************************************
        /// Nombre: Consultar_Rpt_Incapacidades
        /// 
        /// Descripción: Consulta la información que sera mostrada en el reporte de incapacidades.
        /// 
        /// Parámetros: Datos.- Objeto que contiene los valores que se usaran dentro de la consulta de incapacidades.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creo: 05/Diciembre/2011 10:28 a.m.
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ********************************************************************************************************************
        public static DataTable Consultar_Rpt_Incapacidades(Cls_Ope_Nom_Incapacidades_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            StringBuilder Mi_SQL_Aux = new StringBuilder();
            DataTable Dt_Incapacidades = null;//Variable que almacenara el listado de incapacidades.
            
            try
            {
                Mi_SQL.Append("SELECT ");

                Mi_SQL.Append("(SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Codigo_Programatico +
                   " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                   " WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID +
                   "=" + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_Empleado_ID + ") AS CODIGO_PROGRAMATICO, ");

                Mi_SQL.Append("(SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_RFC +
                   " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                   " WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID +
                   "=" + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_Empleado_ID + ") AS RFC, ");

                Mi_SQL.Append("(SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + 
                    " FROM " + Cat_Empleados.Tabla_Cat_Empleados + 
                    " WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + 
                    "=" + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_Empleado_ID + ") AS NO_EMPLEADO, ");

                Mi_SQL.Append("(SELECT (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") ");
                Mi_SQL.Append(" FROM " + Cat_Empleados.Tabla_Cat_Empleados);
                Mi_SQL.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "="); 
                Mi_SQL.Append(Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_Empleado_ID + ") AS EMPLEADO, ");

                Mi_SQL.Append("(SELECT " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + 
                    " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias + 
                    " WHERE " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + 
                    "=" + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_Dependencia_ID + ") AS DEPENDENCIA,");

                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_Dias_Incapacidad + ", ");

                Mi_SQL.Append("('AÑO-PER   ' || (SELECT " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Anio +                     
                    " FROM " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + 
                    " WHERE " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Nomina_ID + 
                    "=" + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_Nomina_ID+ ") || '-' || ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_No_Nomina + ") AS NOMINA, ");

                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_No_Nomina + ", ");

                Mi_SQL.Append("(SELECT " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio);
                Mi_SQL.Append(" FROM " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles);
                Mi_SQL.Append(" WHERE " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Nomina_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_Nomina_ID);
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_No_Nomina);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_No_Nomina + ") AS FECHA_INICIO_PERIODO, ");

                Mi_SQL.Append("(SELECT " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin);
                Mi_SQL.Append(" FROM " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles);
                Mi_SQL.Append(" WHERE " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Nomina_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_Nomina_ID);
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_No_Nomina);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_No_Nomina + ") AS FECHA_FIN_PERIODO, ");

                Mi_SQL.Append("(SELECT ( 'Del  ' || TO_CHAR(" + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio + ", 'DD-Mon-YYYY')");
                Mi_SQL.Append(" || '  Al  ' || ");
                Mi_SQL.Append("TO_CHAR(" + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin + ", 'DD-Mon-YYYY')");
                Mi_SQL.Append(")");
                Mi_SQL.Append(" FROM " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles);
                Mi_SQL.Append(" WHERE " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Nomina_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_Nomina_ID);
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_No_Nomina);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_No_Nomina + ") AS FECHAS, ");

                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_Fecha_Inicio + ", ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Campo_Fecha_Fin + ", ");

                Mi_SQL.Append("( 'Del ' || TO_CHAR(" + Ope_Nom_Incapacidades.Campo_Fecha_Inicio + ", 'DD-Mon-YYYY') ");
                Mi_SQL.Append(" || ' al ' || TO_CHAR(" + Ope_Nom_Incapacidades.Campo_Fecha_Fin + ", 'DD-Mon-YYYY')) AS FECHA_INCAPACIDAD, ");

                Mi_SQL.Append("(SELECT " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina);
                Mi_SQL.Append(" FROM " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas);
                Mi_SQL.Append(" WHERE " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);
                Mi_SQL.Append("=(SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID);
                Mi_SQL.Append(" FROM " + Cat_Empleados.Tabla_Cat_Empleados);
                Mi_SQL.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);
                Mi_SQL.Append("=" + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_Empleado_ID);
                Mi_SQL.Append(")) AS TIPO_NOMINA");

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades); 
                
                if (!String.IsNullOrEmpty(Datos.P_Tipo_Nomina_ID))
                {
                    Mi_SQL_Aux.Append(" WHERE ");
                    Mi_SQL_Aux.Append(Ope_Nom_Incapacidades.Campo_Empleado_ID + " IN ");
                    Mi_SQL_Aux.Append("(SELECT " + Cat_Empleados.Campo_Empleado_ID);
                    Mi_SQL_Aux.Append(" FROM " + Cat_Empleados.Tabla_Cat_Empleados);
                    Mi_SQL_Aux.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID + "='" + Datos.P_Tipo_Nomina_ID + "')");
                }

                if (!String.IsNullOrEmpty(Datos.P_Nombre_Empleado))
                {
                    if (Mi_SQL_Aux.ToString().Contains("WHERE"))
                    {
                        Mi_SQL_Aux.Append(" AND ");
                        Mi_SQL_Aux.Append(Ope_Nom_Incapacidades.Campo_Empleado_ID + " IN ");
                        Mi_SQL_Aux.Append("(SELECT " + Cat_Empleados.Campo_Empleado_ID);
                        Mi_SQL_Aux.Append(" FROM " + Cat_Empleados.Tabla_Cat_Empleados);
                        Mi_SQL_Aux.Append(" WHERE ");
                        Mi_SQL_Aux.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                        Mi_SQL_Aux.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                        Mi_SQL_Aux.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") ");
                        Mi_SQL_Aux.Append(" LIKE '%" + Datos.P_Nombre_Empleado + "%'");
                        Mi_SQL_Aux.Append(" OR ");
                        Mi_SQL_Aux.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " || ' ' || ");
                        Mi_SQL_Aux.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                        Mi_SQL_Aux.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ") ");
                        Mi_SQL_Aux.Append(" LIKE '%" + Datos.P_Nombre_Empleado + "%')");
                    }
                    else 
                    {
                        Mi_SQL_Aux.Append(" WHERE ");
                        Mi_SQL_Aux.Append(Ope_Nom_Incapacidades.Campo_Empleado_ID + " IN ");
                        Mi_SQL_Aux.Append("(SELECT " + Cat_Empleados.Campo_Empleado_ID);
                        Mi_SQL_Aux.Append(" FROM " + Cat_Empleados.Tabla_Cat_Empleados);
                        Mi_SQL_Aux.Append(" WHERE ");
                        Mi_SQL_Aux.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                        Mi_SQL_Aux.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                        Mi_SQL_Aux.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") ");
                        Mi_SQL_Aux.Append(" LIKE '%"+Datos.P_Nombre_Empleado+"%'");
                        Mi_SQL_Aux.Append(" OR ");
                        Mi_SQL_Aux.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " || ' ' || ");
                        Mi_SQL_Aux.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                        Mi_SQL_Aux.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ") ");
                        Mi_SQL_Aux.Append(" LIKE '%" + Datos.P_Nombre_Empleado + "%')");
                    }
                   
                }

                if (!String.IsNullOrEmpty(Datos.P_No_Empleado))
                {
                    if (Mi_SQL_Aux.ToString().Contains("WHERE"))
                    {
                        Mi_SQL_Aux.Append(" AND ");
                        Mi_SQL_Aux.Append(Ope_Nom_Incapacidades.Campo_Empleado_ID + " IN ");
                        Mi_SQL_Aux.Append("(SELECT " + Cat_Empleados.Campo_Empleado_ID);
                        Mi_SQL_Aux.Append(" FROM " + Cat_Empleados.Tabla_Cat_Empleados);
                        Mi_SQL_Aux.Append(" WHERE " + Cat_Empleados.Campo_No_Empleado + " = '" + String.Format("{0:000000}", Convert.ToInt32(Datos.P_No_Empleado)) + "')");
                       
                    }
                    else
                    {
                        Mi_SQL_Aux.Append(" WHERE ");
                        Mi_SQL_Aux.Append(Ope_Nom_Incapacidades.Campo_Empleado_ID + " IN ");
                        Mi_SQL_Aux.Append("(SELECT " + Cat_Empleados.Campo_Empleado_ID);
                        Mi_SQL_Aux.Append(" FROM " + Cat_Empleados.Tabla_Cat_Empleados);
                        Mi_SQL_Aux.Append(" WHERE " + Cat_Empleados.Campo_No_Empleado + " = '" + String.Format("{0:000000}", Convert.ToInt32(Datos.P_No_Empleado)) + "')");
                    }

                }

                if (!String.IsNullOrEmpty(Datos.P_Nomina_ID))
                {
                    if (Mi_SQL_Aux.ToString().Contains("WHERE"))
                        Mi_SQL_Aux.Append(" AND " + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "'");
                    else
                        Mi_SQL_Aux.Append(" WHERE " + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "'");
                }

                if (Datos.P_No_Nomina > 0)
                {
                    if (Mi_SQL_Aux.ToString().Contains("WHERE"))
                        Mi_SQL_Aux.Append(" AND " + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_No_Nomina + "=" + Datos.P_No_Nomina);
                    else
                        Mi_SQL_Aux.Append(" WHERE " + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_No_Nomina + "=" + Datos.P_No_Nomina);
                }

                if (!String.IsNullOrEmpty(Datos.P_Dependencia_ID))
                {
                    if (Mi_SQL_Aux.ToString().Contains("WHERE"))
                        Mi_SQL_Aux.Append(" AND " + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_Dependencia_ID+ "='" + Datos.P_Dependencia_ID + "'");
                    else
                        Mi_SQL_Aux.Append(" WHERE " + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'");
                }

                Mi_SQL_Aux.Append(" ORDER BY ");

                Mi_SQL_Aux.Append("(SELECT " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina);
                Mi_SQL_Aux.Append(" FROM " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas);
                Mi_SQL_Aux.Append(" WHERE " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);
                Mi_SQL_Aux.Append("=(SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID);
                Mi_SQL_Aux.Append(" FROM " + Cat_Empleados.Tabla_Cat_Empleados);
                Mi_SQL_Aux.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);
                Mi_SQL_Aux.Append("=" + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_Empleado_ID);
                Mi_SQL_Aux.Append(")), ");

                Mi_SQL_Aux.Append("(SELECT " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre +
                   " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias +
                   " WHERE " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID +
                   "=" + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_Dependencia_ID + "), ");

                Mi_SQL_Aux.Append("(SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno +
                   " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                   " WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID +
                   "=" + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades + "." + Ope_Nom_Incapacidades.Campo_Empleado_ID + ") ");

                Mi_SQL_Aux.Append(" ASC ");


                Dt_Incapacidades = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, (Mi_SQL.ToString() + Mi_SQL_Aux.ToString())).Tables[0];

                Dt_Incapacidades.Columns.Add("USUARIO", typeof(String));

                if (Dt_Incapacidades is DataTable) {
                    if (Dt_Incapacidades.Rows.Count > 0) {
                        foreach (DataRow INCAPACIDAD in Dt_Incapacidades.Rows) {
                            if (INCAPACIDAD is DataRow) {
                                INCAPACIDAD["USUARIO"] = "Elaboro: " + Cls_Sessiones.Nombre_Empleado;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar la información para la generacion del reporte de incapacidades. Error: [" + Ex.Message + "]");
            }
            return Dt_Incapacidades;
        }
        /// ********************************************************************************************************************
        /// Nombre: Identificar_Periodo_Nomina
        /// 
        /// Descripción: Consulta la información de los periodos.
        /// 
        /// Parámetros: Datos.- Objeto que contiene los valores que se usaran dentro de la consulta de incapacidades.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creo: 14/Diciembre/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ********************************************************************************************************************
        public static DataTable Identificar_Periodo_Nomina(Cls_Ope_Nom_Incapacidades_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Periodos = null;//Variable que almacenara el periodo consultado.

            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Campo_Nomina_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Campo_Detalle_Nomina_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Campo_No_Nomina + ", ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio + ", ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin);

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles);

                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin + " >= '" + Datos.P_Fecha_Fin_Incapacidad + "'");

                Dt_Periodos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al idenitificar el periodo nominal. Error: [" + Ex.Message + "]");
            }
            return Dt_Periodos;
        }
        /// ********************************************************************************************************************
        /// Nombre: Identificar_Periodo_Nomina_Reloj_Checador
        /// 
        /// Descripción: Consulta la información de los periodos del reloj checador.
        /// 
        /// Parámetros: Datos.- Objeto que contiene los valores que se usaran dentro de la consulta de incapacidades.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creo: Abril/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ********************************************************************************************************************
        public static DataTable Identificar_Periodo_Nomina_Reloj_Checador(Cls_Ope_Nom_Incapacidades_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Periodos = null;//Variable que almacenara el periodo consultado.

            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Nom_Calendario_Reloj.Campo_Nomina_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Calendario_Reloj.Campo_Consecutivo + ", ");
                Mi_SQL.Append(Cat_Nom_Calendario_Reloj.Campo_No_Nomina + ", ");
                Mi_SQL.Append(Cat_Nom_Calendario_Reloj.Campo_Fecha_Inicio + ", ");
                Mi_SQL.Append(Cat_Nom_Calendario_Reloj.Campo_Fecha_Fin);

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Nom_Calendario_Reloj.Tabla_Cat_Nom_Calendario_Reloj);

                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Nom_Calendario_Reloj.Campo_Fecha_Fin + " >= '" + Datos.P_Fecha_Fin_Incapacidad + "'");

                Dt_Periodos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al idenitificar el periodo nominal reloj checador. Error: [" + Ex.Message + "]");
            }
            return Dt_Periodos;
        }

        /// ********************************************************************************************************************
        /// Nombre: Identificar_UR
        /// 
        /// Descripción: Consulta la información de las dependencias.
        /// 
        /// Parámetros: Datos.- Objeto que contiene los valores que se usaran dentro de la consulta de incapacidades.
        /// 
        /// Usuario Creo: Leslie Gonzalez Vazquez
        /// Fecha Creo: 05/Abril/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ********************************************************************************************************************
        public static DataTable Identificar_UR(Cls_Ope_Nom_Incapacidades_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_UR = new DataTable();//Variable que almacenara el periodo consultado.

            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Dependencias.Campo_Clave  + " || ' ' || ");
                Mi_SQL.Append(Cat_Dependencias.Campo_Nombre + " AS Clave_Nombre, ");
                Mi_SQL.Append(Cat_Dependencias.Campo_Dependencia_ID);

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias);

                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Dependencias.Campo_Estatus + " = 'ACTIVO'");

                Mi_SQL.Append(" ORDER BY Clave_Nombre ASC");

                Dt_UR = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al idenitificar las unidades responsables. Error: [" + Ex.Message + "]");
            }
            return Dt_UR;
        }
        #endregion
    }
}
