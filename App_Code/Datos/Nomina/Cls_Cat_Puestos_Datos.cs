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
using Presidencia.Puestos.Negocios;
using Presidencia.Zona_Economica.Negocios;
using Presidencia.Vacaciones_Empleado.Negocio;
using System.Text;
using Presidencia.Utilidades_Nomina;

namespace Presidencia.Puestos.Datos
{
    public class Cls_Cat_Puestos_Datos
    {
        #region (Metodos)

        #region (Metodo de Operacion)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Puesto
        /// DESCRIPCION : 1.Consulta el último ID dado de alta para poder ingresar el siguiente
        ///               2. Da de Alta el Puesto en la BD con los datos proporcionados por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 26-Agosto-2010
        /// MODIFICO          :Francisco Antonio Gallardo Castañeda.
        /// FECHA_MODIFICO    :07-Octubre-2010
        /// CAUSA_MODIFICACION: Se agregaron las transacciones.
        ///*******************************************************************************
        public static void Alta_Puesto(Cls_Cat_Puestos_Negocio Datos)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try
            {
                //SE DAN DE ALTA LOS DATOS GENERALES DEL PUESTO
                String Puesto_ID = Obtener_ID_Consecutivo(Cat_Puestos.Tabla_Cat_Puestos, Cat_Puestos.Campo_Puesto_ID, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Puestos.Tabla_Cat_Puestos + " (";
                Mi_SQL = Mi_SQL + Cat_Puestos.Campo_Puesto_ID + ", " + Cat_Puestos.Campo_Nombre;
                Mi_SQL = Mi_SQL + ", " + Cat_Puestos.Campo_Estatus + ", " + Cat_Puestos.Campo_Comentarios;
                Mi_SQL = Mi_SQL + ", " + Cat_Puestos.Campo_Salario_Mensual + ", " +Cat_Puestos.Campo_Plaza_ID + ", " + Cat_Puestos.Campo_Usuario_Creo;
                Mi_SQL = Mi_SQL + ", " + Cat_Puestos.Campo_Fecha_Creo + ", " + Cat_Puestos.Campo_Aplica_Fondo_Retiro + ", " + Cat_Puestos.Campo_Aplica_PSM + ") VALUES ('" + Puesto_ID + "', '" + Datos.P_Nombre + "'";
                Mi_SQL = Mi_SQL + ", '" + Datos.P_Estatus + "', '" + Datos.P_Comentarios + "', '" + Datos.P_Salario_Mensual + "', '" + Datos.P_Plaza_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Datos.P_Nombre_Usuario + "', SYSDATE, '" + Datos.P_Aplica_Fondo_Retiro +
                         "', '" + Datos.P_Aplica_Psm + "')";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                if (Datos.P_Perfiles != null && Datos.P_Perfiles.Rows.Count > 0)
                {
                    for (int cnt = 0; cnt < Datos.P_Perfiles.Rows.Count; cnt++)
                    {
                        Mi_SQL = "INSERT INTO " + Cat_Tra_Puestos_Perfiles.Tabla_Cat_Tra_Puestos_Perfiles + " (";
                        Mi_SQL = Mi_SQL + Cat_Tra_Puestos_Perfiles.Campo_Perfil_ID + "," + Cat_Tra_Puestos_Perfiles.Campo_Puesto_ID;
                        Mi_SQL = Mi_SQL + ", " + Cat_Tra_Puestos_Perfiles.Campo_UsuarioCreo + ", " + Cat_Tra_Puestos_Perfiles.Campo_FechaCreo + ") VALUES (";
                        Mi_SQL = Mi_SQL + "'" + Datos.P_Perfiles.Rows[cnt][0].ToString() + "','" + Puesto_ID + "'";
                        Mi_SQL = Mi_SQL + ", '" + Datos.P_Nombre_Usuario + "', SYSDATE )";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }
                }
                Trans.Commit();
            }
            catch (OracleException Ex)
            {
                Trans.Rollback();
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152)
                {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar";
                }
                else if (Ex.Code == 2627)
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
                else if (Ex.Code == 547)
                {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla";
                }
                else if (Ex.Code == 515)
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
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error:[" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                if (Cn.State == ConnectionState.Open)
                {
                    Cn.Close();
                }
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Modificar_Puesto
        /// DESCRIPCION : Modifica los datos del puesto con lo que fueron introducidos por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán modificados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 26-Agosto-2010
        /// MODIFICO          :Francisco Antonio Gallardo Castañeda.
        /// FECHA_MODIFICO    :07-Octubre-2010
        /// CAUSA_MODIFICACION: Se agregaron las transacciones.
        ///*******************************************************************************
        public static void Modificar_Puesto(Cls_Cat_Puestos_Negocio Datos)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            String Mi_SQL; //Obtiene la cadena de modificación hacía la base de datos
            try
            {
                //SE ACTUALIZAN LOS DATOS GENERALES DEL PUESTO
                Mi_SQL = "UPDATE " + Cat_Puestos.Tabla_Cat_Puestos + " SET ";
                Mi_SQL = Mi_SQL + Cat_Puestos.Campo_Nombre + " = '" + Datos.P_Nombre + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Puestos.Campo_Estatus + " = '" + Datos.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Puestos.Campo_Comentarios + " = '" + Datos.P_Comentarios + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Puestos.Campo_Salario_Mensual + " = '" + Datos.P_Salario_Mensual + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Puestos.Campo_Plaza_ID + " = '" + Datos.P_Plaza_ID + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Puestos.Campo_Aplica_Fondo_Retiro + "='" + Datos.P_Aplica_Fondo_Retiro + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Puestos.Campo_Aplica_PSM + "='" + Datos.P_Aplica_Psm + "'";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Puestos.Campo_Puesto_ID + " = '" + Datos.P_Puesto_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                //SE ELIMINAN LOS PERFILES ANTERIORES
                Mi_SQL = "DELETE FROM " + Cat_Tra_Puestos_Perfiles.Tabla_Cat_Tra_Puestos_Perfiles;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Tra_Puestos_Perfiles.Campo_Puesto_ID;
                Mi_SQL = Mi_SQL + " = '" + Datos.P_Puesto_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                //SE DAN DE ALTA LOS NUEVOS PERFILES
                if (Datos.P_Perfiles != null && Datos.P_Perfiles.Rows.Count > 0)
                {
                    for (int cnt = 0; cnt < Datos.P_Perfiles.Rows.Count; cnt++)
                    {
                        Mi_SQL = "INSERT INTO " + Cat_Tra_Puestos_Perfiles.Tabla_Cat_Tra_Puestos_Perfiles + " (";
                        Mi_SQL = Mi_SQL + Cat_Tra_Puestos_Perfiles.Campo_Perfil_ID + "," + Cat_Tra_Puestos_Perfiles.Campo_Puesto_ID;
                        Mi_SQL = Mi_SQL + ", " + Cat_Tra_Puestos_Perfiles.Campo_UsuarioCreo + ", " + Cat_Tra_Puestos_Perfiles.Campo_FechaCreo + ") VALUES (";
                        Mi_SQL = Mi_SQL + "'" + Datos.P_Perfiles.Rows[cnt][0].ToString() + "','" + Datos.P_Puesto_ID + "'";
                        Mi_SQL = Mi_SQL + ", '" + Datos.P_Nombre_Usuario + "', SYSDATE )";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }
                }
                //Actualizar los salarios de los empleados.
                Actualizar_Salario_Empleados(Datos.P_Puesto_ID, Datos.P_Salario_Mensual);

                Trans.Commit();
            }
            catch (OracleException Ex)
            {
                Trans.Rollback();
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152)
                {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar";
                }
                else if (Ex.Code == 2627)
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
                else if (Ex.Code == 547)
                {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla";
                }
                else if (Ex.Code == 515)
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
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error:[" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                Cn.Close();
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Eliminar_Puesto
        /// DESCRIPCION : Elimina el puesto que fue seleccionada por el usuario de la BD
        /// PARAMETROS  : Datos: Obtiene que puesto desea eliminar de la BD
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 26-Agosto-2010
        /// MODIFICO          :Francisco Antonio Gallardo Castañeda.
        /// FECHA_MODIFICO    :07-Octubre-2010
        /// CAUSA_MODIFICACION: Se agregaron las transacciones.
        ///*******************************************************************************
        public static void Eliminar_Puesto(Cls_Cat_Puestos_Negocio Datos)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            String Mi_SQL; //Variable de Consulta para la eliminación del puesto
            try
            {
                Mi_SQL = "DELETE FROM " + Cat_Tra_Puestos_Perfiles.Tabla_Cat_Tra_Puestos_Perfiles + " WHERE ";
                Mi_SQL = Mi_SQL + Cat_Tra_Puestos_Perfiles.Campo_Puesto_ID + " = '" + Datos.P_Puesto_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Mi_SQL = "DELETE FROM " + Cat_Puestos.Tabla_Cat_Puestos + " WHERE ";
                Mi_SQL = Mi_SQL + Cat_Puestos.Campo_Puesto_ID + " = '" + Datos.P_Puesto_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            }
            catch (OracleException ex)
            {
                if (ex.Code == 547)
                {
                    Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos";
                }
                else
                {
                    Mensaje = ex.Message;
                }
                throw new Exception(Mensaje);
            }
            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error:[" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                Cn.Close();
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Actualizar_Salario_Empleados
        /// DESCRIPCION : Actualiza el salario diario y  salario diario integrado del empleado.
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 10/Enero/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private static void Actualizar_Salario_Empleados(String Puesto_Id, Double Salario_Mensual)
        {
            String Mensaje = "";
            Double Salario_Diario = 0.0;
            Double Salario_Diario_Integrado = 0.0;
            DataTable Dt_Empleados_Puesto = null;//Variable qque almacenara una lista de empleados con el puesto a modificar. 
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            String Mi_SQL; //Variable de Consulta para la eliminación del puesto
            try
            {
                Salario_Diario = (Salario_Mensual / Cls_Utlidades_Nomina.Dias_Mes_Fijo);

                Mi_SQL = "SELECT " + Cat_Empleados.Tabla_Cat_Empleados + ".* " +
                         " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                         " WHERE " + Cat_Empleados.Campo_Puesto_ID + "='" + Puesto_Id + "'";

                Dt_Empleados_Puesto = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                if (Dt_Empleados_Puesto != null)
                {
                    foreach (DataRow Renglon in Dt_Empleados_Puesto.Rows)
                    {
                        Salario_Diario_Integrado = Calculo_Salario_Diario_Integrado(Salario_Diario, Renglon[Cat_Empleados.Campo_No_Empleado].ToString());

                        Mi_SQL = "UPDATE " + Cat_Empleados.Tabla_Cat_Empleados +
                                 " SET " +
                                 Cat_Empleados.Campo_Salario_Diario + "=" + Salario_Diario + ", " +
                                 Cat_Empleados.Campo_Salario_Diario_Integrado + "=" + Salario_Diario_Integrado +
                                " WHERE " +
                                Cat_Empleados.Campo_Empleado_ID + "='" + Renglon[Cat_Empleados.Campo_Empleado_ID].ToString() + "'";

                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }
                }

                Trans.Commit();
            }
            catch (OracleException ex)
            {
                if (ex.Code == 547)
                {
                    Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos";
                }
                else
                {
                    Mensaje = ex.Message;
                }
                throw new Exception(Mensaje);
            }
            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error:[" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                Cn.Close();
            }
        }
        #endregion

        #region (Metodos de consulta)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Datos_Puestos
        /// DESCRIPCION : Consulta todos los datos un puesto que esta dado de alta en la BD
        ///               con todos sus datos
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 26-Agosto-2010
        /// MODIFICO          : Francisco Antonio Gallardo Castañeda.
        /// FECHA_MODIFICO    : 07-Octubre-2010
        /// CAUSA_MODIFICACION: Agregar Transacciones y Manejarlo con la Clase de Negocio
        ///                     en lugar de DataTable
        ///*******************************************************************************
        public static Cls_Cat_Puestos_Negocio Consulta_Datos_Puestos(Cls_Cat_Puestos_Negocio Datos)
        {
            String Mi_SQL = null; //Variable para la consulta de los puestos 
            Cls_Cat_Puestos_Negocio Puesto = new Cls_Cat_Puestos_Negocio();
            DataTable Dt_Datos_Puesto = null;
            try
            {

                Mi_SQL = "SELECT " + Cat_Puestos.Campo_Nombre + ", " + Cat_Puestos.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", " + Cat_Puestos.Campo_Salario_Mensual + ", " + Cat_Puestos.Campo_Comentarios;
                Mi_SQL = Mi_SQL + ", " + Cat_Puestos.Campo_Plaza_ID;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Puestos.Tabla_Cat_Puestos;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Puestos.Campo_Puesto_ID + " = '" + Datos.P_Puesto_ID + "'";

                Puesto.P_Puesto_ID = Datos.P_Puesto_ID;
                Dt_Datos_Puesto = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                if (Dt_Datos_Puesto != null)
                {
                    if (Dt_Datos_Puesto.Rows.Count > 0)
                    {
                        Puesto.P_Nombre = Dt_Datos_Puesto.Rows[0][0].ToString();
                        Puesto.P_Estatus = Dt_Datos_Puesto.Rows[0][1].ToString();
                        if (!string.IsNullOrEmpty(Dt_Datos_Puesto.Rows[0][2].ToString())) Puesto.P_Salario_Mensual = Convert.ToDouble(Dt_Datos_Puesto.Rows[0][2].ToString());
                        if (!string.IsNullOrEmpty(Dt_Datos_Puesto.Rows[0][3].ToString())) Puesto.P_Comentarios = Dt_Datos_Puesto.Rows[0][3].ToString();
                        if (!string.IsNullOrEmpty(Dt_Datos_Puesto.Rows[0][Cat_Puestos.Campo_Plaza_ID].ToString())) Puesto.P_Plaza_ID = Dt_Datos_Puesto.Rows[0][Cat_Puestos.Campo_Plaza_ID].ToString();
                    }
                }

                Mi_SQL = "SELECT " + Cat_Tra_Puestos_Perfiles.Tabla_Cat_Tra_Puestos_Perfiles + "." + Cat_Tra_Puestos_Perfiles.Campo_Perfil_ID + " AS PERFIL_ID";
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Perfiles.Tabla_Cat_Tra_Perfiles + "." + Cat_Tra_Perfiles.Campo_Nombre + " AS NOMBRE";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Tra_Puestos_Perfiles.Tabla_Cat_Tra_Puestos_Perfiles + ", " + Cat_Tra_Perfiles.Tabla_Cat_Tra_Perfiles;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Tra_Puestos_Perfiles.Tabla_Cat_Tra_Puestos_Perfiles + "." + Cat_Tra_Puestos_Perfiles.Campo_Perfil_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Tra_Perfiles.Tabla_Cat_Tra_Perfiles + "." + Cat_Tra_Perfiles.Campo_Perfil_ID;
                Mi_SQL = Mi_SQL + " AND " + Cat_Tra_Puestos_Perfiles.Tabla_Cat_Tra_Puestos_Perfiles + "." + Cat_Tra_Puestos_Perfiles.Campo_Puesto_ID;
                Mi_SQL = Mi_SQL + " = '" + Datos.P_Puesto_ID + "'";
                DataSet dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataSet == null)
                {
                    Puesto.P_Perfiles = new DataTable();
                }
                else
                {
                    Puesto.P_Perfiles = dataSet.Tables[0];
                }
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error:[" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
            return Puesto;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_DataTable
        /// DESCRIPCION : Consulta los puestos que estan dados de alta en la BD 
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 26-Agosto-2010
        /// MODIFICO          : Francisco Antonio Gallardo Castañeda.
        /// FECHA_MODIFICO    : 07-Octubre-2010
        /// CAUSA_MODIFICACION: Hacer mas general el Metodo y hacer mas reusable.
        ///*******************************************************************************
        public static DataTable Consulta_DataTable(Cls_Cat_Puestos_Negocio Datos)
        {
            String Mi_SQL = null; //Variable para la consulta de los Puestos
            DataTable Tabla = null; // Tabla que se va a retornar
            DataSet Data_Set = null; // Capturala respuesta de la Consulta a la Base de Datos;
            try
            {
                if (Datos.P_Tipo_DataTable.Equals("PUESTOS")) {
                    Mi_SQL = "SELECT " + Cat_Puestos.Campo_Puesto_ID + " AS PUESTO_ID, " + Cat_Puestos.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + ", " + Cat_Puestos.Campo_Estatus + " AS ESTATUS, " + Cat_Puestos.Campo_Plaza_ID + " AS PLAZA_ID";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Puestos.Tabla_Cat_Puestos;
                    if (Datos.P_Nombre != null) {
                        Mi_SQL = Mi_SQL + " WHERE UPPER(" + Cat_Puestos.Campo_Nombre + ") LIKE UPPER('%" + Datos.P_Nombre + "%')";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Puestos.Campo_Nombre;
                } else if (Datos.P_Tipo_DataTable.Equals("PERFILES")) {
                    Mi_SQL = "SELECT " + Cat_Tra_Perfiles.Campo_Perfil_ID + " AS PERFIL_ID, " + Cat_Tra_Perfiles.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Tra_Perfiles.Tabla_Cat_Tra_Perfiles;
                } else if (Datos.P_Tipo_DataTable.Equals("PLAZAS")) {
                    Mi_SQL = "SELECT " + Cat_Nom_Plazas.Campo_Plaza_ID + " AS PLAZA_ID, " + Cat_Nom_Plazas.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Nom_Plazas.Tabla_Cat_Nom_Plazas + " WHERE " + Cat_Nom_Plazas.Campo_Estatus + " = 'ACTIVO'";
                }

                if (Mi_SQL != null && Mi_SQL.Trim().Length > 0)
                {
                    Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Data_Set != null)
                {
                    Tabla = Data_Set.Tables[0];
                }
                else
                {
                    Tabla = new DataTable();
                }
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error:[" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {

            }
            return Tabla;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static String Obtener_ID_Consecutivo(string tabla, string campo, int longitud_id)
        {
            String id = Convertir_A_Formato_ID(1, longitud_id); ;
            try
            {
                String Mi_SQL = "SELECT MAX(" + campo + ") FROM " + tabla;
                Object tmp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (!(tmp is Nullable) && !tmp.ToString().Equals(""))
                {
                    id = Convertir_A_Formato_ID((Convert.ToInt32(tmp) + 1), longitud_id);
                }
            }
            catch (OracleException Ex)
            {
                new Exception(Ex.Message);
            }
            return id;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Convertir_A_Formato_ID
        ///DESCRIPCIÓN: Pasa un numero entero a Formato de ID.
        ///PARAMETROS:     
        ///             1. Dato_ID. Dato que se desea pasar al Formato de ID.
        ///             2. Longitud_ID. Longitud que tendra el ID. 
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static String Convertir_A_Formato_ID(int Dato_ID, int Longitud_ID)
        {
            String retornar = "";
            String Dato = "" + Dato_ID;
            for (int tmp = Dato.Length; tmp < Longitud_ID; tmp++)
            {
                retornar = retornar + "0";
            }
            retornar = retornar + Dato;
            return retornar;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Calculo_Salario_Diario_Integrado
        /// DESCRIPCION : Ejecuta el calculo del salario diario integrado.
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 09/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private static Double Calculo_Salario_Diario_Integrado(Double Salario_Diario, String No_Empleado)
        {
            Cls_Ope_Nom_Vacaciones_Empleado_Negocio Cls_Vacaciones_Empleado = new Cls_Ope_Nom_Vacaciones_Empleado_Negocio();//Variable de conexion con la capa de negocios.
            Cls_Cat_Nom_Zona_Economica_Negocio Zona_Economica = new Cls_Cat_Nom_Zona_Economica_Negocio();//Variable de conexion con la capa de negocios.
            DataTable Dt_Zona_Economica = null;//Variable que alamcenara informacion sobre las zonas economicas.
            Double Prima_Vacacional = 0.0;//Obtiene la prima vacacional del empleado
            Double Aguinaldo = 0.0;//Obtiene el aguinaldo del empleado
            Double Despensa_Total = 0.0;//Obtiene la despensa total que debe resivir el empleado de acuerdo al tipo de pago que tiene
            Double Despensa_Diaria = 0.0;//Obtiene la despensa díaria a resivir el empleado
            Double Excedente_IMSS = 0.0;//Obtiene el excedente del IMSS
            Double Despensa_IMSS = 0.0;//Obtiene la despensa a resivir por parte del IMSS el empleado
            Double SMDF = 0.0;//Variable que almacenara el salario diario en el distrito federal.
            Int32 Dias = 0;//Variable que almacenara los dias de vacaciones que el empleado puede tomar.
            Double Salario_Diario_Integrado = 0.0;//Variable que el almacenara el salario diario itegrado.

            try
            {
                //Consulta el salario diario del distrito federal.
                Zona_Economica.P_Zona_Economica = "SMDF";
                Dt_Zona_Economica = Zona_Economica.Consulta_Datos_Zona_Economica();
                //Valida que se hallaencontrado algun resultado en la bsuqueda de salirio diario integrado de la zona econoimica.
                if (Dt_Zona_Economica != null)
                {
                    if (Dt_Zona_Economica.Rows.Count > 0)
                    {
                        //Obtenemos el salario.
                        SMDF = (!string.IsNullOrEmpty(Dt_Zona_Economica.Rows[0][Cat_Nom_Zona_Economica.Campo_Salario_Diario].ToString().Trim())) ? Convert.ToDouble(Dt_Zona_Economica.Rows[0][Cat_Nom_Zona_Economica.Campo_Salario_Diario].ToString().Trim()) : 0;
                    }
                }
                //Consultamos los dias de vacaciones del empleado con el no de empleado proporcionado.
                Cls_Vacaciones_Empleado.P_No_Empleado = No_Empleado;//Asignamos el no de vacacion que deseamos consultar.
                Dias = Cls_Vacaciones_Empleado.Consultar_Dias_Vacaciones_Empleado();//Consultamos los dias de vacaciones del empleado.

                //Comenzamos con los calculos para obtener el salario diario integrado.
                Prima_Vacacional = (Dias * Salario_Diario * 0.25) / 365;//Obtenemos la prima vacacional
                Aguinaldo = (15 * Salario_Diario) / 365;//obtenemos el aguinaldo
                Despensa_Total = (Salario_Diario * 30.4) * 0.2;//Obtenemos la despensa total.
                Despensa_Diaria = Despensa_Total / 30.4;//Obtenemos la despensa diaria.
                Excedente_IMSS = SMDF * 0.4;//obtenemos el excedente de imss

                if (Despensa_Diaria < Excedente_IMSS) Despensa_IMSS = 0;
                else Despensa_IMSS = Despensa_Diaria - Excedente_IMSS;

                //Formula para obtener el salario diario integrado.
                Salario_Diario_Integrado = (Prima_Vacacional + Aguinaldo + Despensa_IMSS + Salario_Diario);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Salario_Diario_Integrado;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Puestos
        ///DESCRIPCIÓN: Consulta los puestos registrados en sistema.
        ///
        ///CREO: Juan Alberto Hernández Negrete.
        ///FECHA_CREO: 02/Mayo/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Puestos(Cls_Cat_Puestos_Negocio Datos)
        {
            String Mi_SQL = String.Empty;
            DataTable Dt_Puestos = null;

            try
            {
                Mi_SQL = "SELECT " + Cat_Puestos.Campo_Puesto_ID + ", " + Cat_Puestos.Campo_Nombre + ", " + Cat_Puestos.Campo_Salario_Mensual + ", " + Cat_Puestos.Campo_Aplica_Fondo_Retiro + ", " + Cat_Puestos.Campo_Aplica_PSM;
                Mi_SQL += " FROM " + Cat_Puestos.Tabla_Cat_Puestos;

                if (!String.IsNullOrEmpty(Datos.P_Puesto_ID))
                {
                    if (Mi_SQL.Contains("WHERE"))
                        Mi_SQL += " AND " + Cat_Puestos.Campo_Puesto_ID + "='" + Datos.P_Puesto_ID + "'";
                    else
                        Mi_SQL += " WHERE " + Cat_Puestos.Campo_Puesto_ID + "='" + Datos.P_Puesto_ID + "'";
                }

                Dt_Puestos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los puestos. Error: [" + Ex.Message + "]");
            }
            return Dt_Puestos;
        }
        /// **************************************************************************************************************
        /// NOMBRE: Consultar_Puestos_Disponibles_Dependencia
        /// 
        /// DESCRIPCIÓN: Consulta los puestos que se encuentran disponibles en la dependencia seleccionada.
        /// 
        /// PARÁMETROS: 
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete
        /// FECHA CREÓ: 16/Junio/2011 11:43 a.m..
        /// USUARIO MODIFICO:
        /// CAUSA MODIFICACIÓN:
        /// **************************************************************************************************************
        internal static DataTable Consultar_Puestos_Disponibles_Dependencia(Cls_Cat_Puestos_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Puestos_Dependencia = null;

            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + ".*, ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Estatus + " AS ESTATUS_PUESTO, ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Clave + ", ");
                Mi_SQL.Append("(" + Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Clave + " || '*' || ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Nombre + ") AS PUESTO");
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos);
                Mi_SQL.Append(" INNER JOIN ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det);
                Mi_SQL.Append(" ON ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Puesto_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Puesto_ID);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." +  Cat_Nom_Dep_Puestos_Det.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Estatus + "='ACTIVO'");

                if (!String.IsNullOrEmpty(Datos.P_Estatus))
                {
                    Mi_SQL.Append(" AND ");
                    Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + ".");
                    Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Campo_Estatus + "='" + Datos.P_Estatus + "'");
                }

                if (!String.IsNullOrEmpty(Datos.P_Puesto_ID))
                {
                    Mi_SQL.Append(" AND ");
                    Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + ".");
                    Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Campo_Puesto_ID + "='" + Datos.P_Puesto_ID + "'");
                }

                if (!String.IsNullOrEmpty(Datos.P_Empleado_ID))
                {
                    Mi_SQL.Append(" AND ");
                    Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + ".");
                    Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'");
                }

                Dt_Puestos_Dependencia = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los puestos que tiene la dependencia disponibles. Error: [" + Ex.Message + "]");
            }
            return Dt_Puestos_Dependencia;
        }


        public static DataTable Consultar_Puestos_UR(Cls_Cat_Puestos_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable que almacenara la consulta.
            DataTable Dt_Resultado = null;//Variable que almacenara el resultado de la consulta.

            try
            {
                Mi_SQL.Append(" select ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Puesto_ID + ", ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Nombre + ", ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Salario_Mensual + ", ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Aplica_Fondo_Retiro + ", ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Aplica_PSM + ", ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Estatus + " as ESTATUS_PUESTO, ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Clave + ", ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Tipo_Plaza);

                Mi_SQL.Append(" from ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + " right outer join " + Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + " on ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Puesto_ID + " = ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Puesto_ID);

                Mi_SQL.Append(" where ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'");

                Dt_Resultado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los puestos por unidad responsable. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }
        #endregion

        #endregion
    }
}