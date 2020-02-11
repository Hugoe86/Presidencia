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
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Domingos_Trabajados.Negocios;
using Presidencia.Empleados.Negocios;

/// <summary>
/// Summary description for Cls_Ope_Nom_Domingos_Empleados_Datos
/// </summary>
namespace Presidencia.Domingos_Trabajados.Datos
{ 
    public class Cls_Ope_Nom_Domingos_Empleados_Datos
    {
	    public Cls_Ope_Nom_Domingos_Empleados_Datos()
	    {
	    }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Domingo_Trabajado
        /// DESCRIPCION : 1.Consulta el último ID dado de alta para poder ingresar el siguiente
        ///               2. Da de Alta el Domingo Trabajado en la BD con los datos proporcionados
        ///                  por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 02-Diciembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Alta_Domingo_Trabajado(Cls_Ope_Nom_Domingos_Empleados_Negocios Datos)
        {
            String Mi_SQL;       //Obtiene la cadena de inserción hacía la base de datos
            Object No_Domingo;   //Obtiene el ID con la cual se guardo los datos en la base de datos
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;                                                  //Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso

            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos

            try
            {
                Mi_SQL = "SELECT NVL(MAX(" + Ope_Nom_Domingos_Empleado.Campo_No_Domingo + "),'0000000000') ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Nom_Domingos_Empleado.Tabla_Cat_Nom_Ope_Nom_Domingos;
                No_Domingo = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(No_Domingo))
                {
                    Datos.P_No_Domingo = "0000000001";
                }
                else
                {
                    Datos.P_No_Domingo = String.Format("{0:0000000000}", Convert.ToInt64(No_Domingo) + 1);
                }
                //Consulta para la inserción del Domingo Trabajado con los datos proporcionados por el usuario
                Mi_SQL = "INSERT INTO " + Ope_Nom_Domingos_Empleado.Tabla_Cat_Nom_Ope_Nom_Domingos + " (";
                Mi_SQL = Mi_SQL + Ope_Nom_Domingos_Empleado.Campo_No_Domingo + ", ";
                Mi_SQL = Mi_SQL + Ope_Nom_Domingos_Empleado.Campo_Dependencia_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Nom_Domingos_Empleado.Campo_Fecha + ", ";
                Mi_SQL = Mi_SQL + Ope_Nom_Domingos_Empleado.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + Ope_Nom_Domingos_Empleado.Campo_Comentarios + ", ";
                Mi_SQL = Mi_SQL + Ope_Nom_Domingos_Empleado.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Ope_Nom_Domingos_Empleado.Campo_Fecha_Creo + ", ";
                Mi_SQL = Mi_SQL + Ope_Nom_Domingos_Empleado.Campo_Nomina_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Nom_Domingos_Empleado.Campo_No_Nomina + ") VALUES ('";
                Mi_SQL = Mi_SQL + Datos.P_No_Domingo + "', '" + Datos.P_Dependencia_ID + "', ";
                Mi_SQL = Mi_SQL + "TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha) + "','DD/MM/YY'), '";
                Mi_SQL = Mi_SQL + Datos.P_Estatus + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Comentarios + "', '" + Datos.P_Nombre_Usuario + "', SYSDATE, '" + Datos.P_Nomina_ID + "', " + Datos.P_No_Nomina + ")";

                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                //Agrega todos los empleados que fueron asignados a la nomina que se quiere dar de alta en la base de datos
                foreach (DataRow Registro_Empleados in Datos.P_Detalles_Domingos_Empleados.Rows)
                {
                    Mi_SQL = "INSERT INTO " + Ope_Nom_Domingos_Empleado_Detalles.Tabla_Ope_Nom_Domingos_Empleados_Detalles + " (";
                    Mi_SQL = Mi_SQL + Ope_Nom_Domingos_Empleado_Detalles.Campo_No_Domingo + ", ";
                    Mi_SQL = Mi_SQL + Ope_Nom_Domingos_Empleado_Detalles.Campo_Empleado_ID + ", ";
                    Mi_SQL = Mi_SQL + Ope_Nom_Domingos_Empleado_Detalles.Campo_Estatus + ") VALUES ('";
                    Mi_SQL = Mi_SQL + Datos.P_No_Domingo + "', '" + Registro_Empleados[Ope_Nom_Domingos_Empleado_Detalles.Campo_Empleado_ID] + "', ";
                    Mi_SQL = Mi_SQL + "'" + Registro_Empleados[Ope_Nom_Domingos_Empleado_Detalles.Campo_Estatus] + "')";

                    Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                    Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                }
                Transaccion_SQL.Commit(); //Pasa todo el proceso que se encuentra en memoria a la base de datos para ser almacenados los datos
            }
            catch (OracleException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                Conexion_Base.Close();
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Modificar_Domingo_Trabajado
        /// DESCRIPCION : Modifica los datos del Domingo con lo que fueron introducidos 
        ///               por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán modificados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 02-Diciembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Modificar_Domingo_Trabajado(Cls_Ope_Nom_Domingos_Empleados_Negocios Datos)
        {
            String Mi_SQL; //Obtiene la cadena de modificación hacía la base de datos
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;                                                  //Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        

            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos

            try
            {
                //Consulta para la modificación del Domingo Trabajado con los datos proporcionados por el usuario
                Mi_SQL = "UPDATE " + Ope_Nom_Domingos_Empleado.Tabla_Cat_Nom_Ope_Nom_Domingos + " SET ";
                Mi_SQL = Mi_SQL + Ope_Nom_Domingos_Empleado.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "', ";
                Mi_SQL = Mi_SQL + Ope_Nom_Domingos_Empleado.Campo_Fecha + " = TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha) + "','DD/MM/YY'), ";
                Mi_SQL = Mi_SQL + Ope_Nom_Domingos_Empleado.Campo_Estatus + " = '" + Datos.P_Estatus + "', ";
                Mi_SQL = Mi_SQL + Ope_Nom_Domingos_Empleado.Campo_Comentarios + " = '" + Datos.P_Comentarios + "', ";
                Mi_SQL = Mi_SQL + Ope_Nom_Domingos_Empleado.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";
                Mi_SQL = Mi_SQL + Ope_Nom_Domingos_Empleado.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "', ";
                Mi_SQL = Mi_SQL + Ope_Nom_Domingos_Empleado.Campo_No_Nomina + "=" + Datos.P_No_Nomina  + ", ";
                Mi_SQL = Mi_SQL + Ope_Nom_Domingos_Empleado.Campo_Fecha_Modifico + " = SYSDATE WHERE ";
                Mi_SQL = Mi_SQL + Ope_Nom_Domingos_Empleado.Campo_No_Domingo + " = '" + Datos.P_No_Domingo + "'";
                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                //Elimina los empleados que tiene asignado el domingo trabajado para poder agregar los empleados que selecciono el usuario
                Mi_SQL = "DELETE FROM " + Ope_Nom_Domingos_Empleado_Detalles.Tabla_Ope_Nom_Domingos_Empleados_Detalles;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Nom_Domingos_Empleado_Detalles.Campo_No_Domingo + " = '" + Datos.P_No_Domingo + "'";
                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                //Agrega todos los empleados que fueron asignados a la nomina que se quiere dar de alta en la base de datos
                foreach (DataRow Registro_Empleados in Datos.P_Detalles_Domingos_Empleados.Rows)
                {
                    Mi_SQL = "INSERT INTO " + Ope_Nom_Domingos_Empleado_Detalles.Tabla_Ope_Nom_Domingos_Empleados_Detalles + " (";
                    Mi_SQL = Mi_SQL + Ope_Nom_Domingos_Empleado_Detalles.Campo_No_Domingo + ", ";
                    Mi_SQL = Mi_SQL + Ope_Nom_Domingos_Empleado_Detalles.Campo_Empleado_ID + ", ";
                    Mi_SQL = Mi_SQL + Ope_Nom_Domingos_Empleado_Detalles.Campo_Estatus + ") VALUES ('";
                    Mi_SQL = Mi_SQL + Datos.P_No_Domingo + "', '" + Registro_Empleados[Ope_Nom_Domingos_Empleado_Detalles.Campo_Empleado_ID] + "', ";
                    Mi_SQL = Mi_SQL + "'" + Registro_Empleados[Ope_Nom_Domingos_Empleado_Detalles.Campo_Estatus] + "')";

                    Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                    Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                }
                Transaccion_SQL.Commit(); //Pasa todo el proceso que se encuentra en memoria a la base de datos para ser almacenados los datos
            }
            catch (OracleException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                Conexion_Base.Close();
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Eliminar_Domingo_Trabajado
        /// DESCRIPCION : Elimina el Domingo Trabajado que fue seleccionada por el usuario de la BD
        /// PARAMETROS  : Datos: Obtiene que Tipo de Nómina desea eliminar de la BD
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 15-Diciembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Eliminar_Domingo_Trabajado(Cls_Ope_Nom_Domingos_Empleados_Negocios Datos)
        {
            String Mi_SQL; //Obtiene la cadena de modificación hacía la base de datos
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;                                                  //Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        

            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos

            try
            {
                //Elimina los empleados que tiene asignado el domingo trabajado
                Mi_SQL = "DELETE FROM " + Ope_Nom_Domingos_Empleado_Detalles.Tabla_Ope_Nom_Domingos_Empleados_Detalles + " WHERE ";
                Mi_SQL = Mi_SQL + Ope_Nom_Domingos_Empleado_Detalles.Campo_No_Domingo + " = '" + Datos.P_No_Domingo + "'";
                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                //Elimina elimina el domingo trabajado de la base de datos
                Mi_SQL = "DELETE FROM " + Ope_Nom_Domingos_Empleado.Tabla_Cat_Nom_Ope_Nom_Domingos;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Nom_Domingos_Empleado.Campo_No_Domingo + " = '" + Datos.P_No_Domingo + "'";
                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                Transaccion_SQL.Commit();         //Pasa todo el proceso que se encuentra en memoria a la base de datos para ser almacenados los datos
            }
            catch (OracleException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                Conexion_Base.Close();
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Datos_Domingos_Trabajados
        /// DESCRIPCION : Consulta todos los datos del Domingo Trabajado que estan dadas de alta en
        ///               la BD con todos sus datos
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 02-Diciembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Datos_Domingos_Trabajados(Cls_Ope_Nom_Domingos_Empleados_Negocios Datos)
        {
            String Mi_SQL; //Variable para la consulta del domingo trabajado

            try
            {
                //Consulta todos los datos del Tipo de Nómina que se fue seleccionado por el usuario
                Mi_SQL = "SELECT * FROM " + Ope_Nom_Domingos_Empleado.Tabla_Cat_Nom_Ope_Nom_Domingos;
                if (Datos.P_No_Domingo != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Nom_Domingos_Empleado.Campo_No_Domingo + " = '" + Datos.P_No_Domingo + "'";
                }
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
            finally
            {
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Empleados_Domingo_Trabajado
        /// DESCRIPCION : Consulta los Empleados del domingo que se esta consultado
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 02-Diciembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Empleados_Domingo_Trabajado(Cls_Ope_Nom_Domingos_Empleados_Negocios Datos)
        {
            String Mi_SQL; //Variable para la consulta de los empleados que trabajaron el domingo seleccionado por el usuario

            try
            {
                //Consulta los empleados del domingo trabajado que esta siendo Consultado
                Mi_SQL = "SELECT " + Ope_Nom_Domingos_Empleado_Detalles.Tabla_Ope_Nom_Domingos_Empleados_Detalles + "." + Ope_Nom_Domingos_Empleado_Detalles.Campo_Empleado_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Nom_Domingos_Empleado_Detalles.Tabla_Ope_Nom_Domingos_Empleados_Detalles + "." + Ope_Nom_Domingos_Empleado_Detalles.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ";
                Mi_SQL = Mi_SQL + "(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno;
                Mi_SQL = Mi_SQL + "||' '||" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno;
                Mi_SQL = Mi_SQL + "||' '||" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") AS Empleado";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Nom_Domingos_Empleado_Detalles.Tabla_Ope_Nom_Domingos_Empleados_Detalles + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Nom_Domingos_Empleado_Detalles.Tabla_Ope_Nom_Domingos_Empleados_Detalles + "." + Ope_Nom_Domingos_Empleado_Detalles.Campo_Empleado_ID + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID;
                Mi_SQL = Mi_SQL + " AND " + Ope_Nom_Domingos_Empleado_Detalles.Tabla_Ope_Nom_Domingos_Empleados_Detalles + "." + Ope_Nom_Domingos_Empleado_Detalles.Campo_No_Domingo + " = '" + Datos.P_No_Domingo + "'";
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno;
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
            finally
            {
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Domingos_Trabajados
        /// DESCRIPCION : Consulta todos los Domingos Trabajados que estan dadas de alta en
        ///               la BD con todos sus datos
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 02-Diciembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Domingos_Trabajados(Cls_Ope_Nom_Domingos_Empleados_Negocios Datos)
        {
            String Mi_SQL; //Variable para la consulta del domingo trabajado

            try
            {
                //Consulta todos los datos del Tipo de Nómina que se fue seleccionado por el usuario
                Mi_SQL = "SELECT " + Ope_Nom_Domingos_Empleado.Tabla_Cat_Nom_Ope_Nom_Domingos + ".*, ";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS Dependencia";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Nom_Domingos_Empleado.Tabla_Cat_Nom_Ope_Nom_Domingos + ", ";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Tabla_Cat_Dependencias;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Nom_Domingos_Empleado.Tabla_Cat_Nom_Ope_Nom_Domingos + "." + Ope_Nom_Domingos_Empleado.Campo_Dependencia_ID + " = ";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Ope_Nom_Domingos_Empleado.Campo_Dependencia_ID;

                if (!String.IsNullOrEmpty(Datos.P_Empleado_ID))
                {
                    if (Mi_SQL.Contains("WHERE"))
                    {
                        Mi_SQL += " AND " + Ope_Nom_Domingos_Empleado.Tabla_Cat_Nom_Ope_Nom_Domingos + "." + Ope_Nom_Domingos_Empleado.Campo_No_Domingo + " IN (SELECT " + Ope_Nom_Domingos_Empleado_Detalles.Campo_No_Domingo;
                        Mi_SQL += " FROM " + Ope_Nom_Domingos_Empleado_Detalles.Tabla_Ope_Nom_Domingos_Empleados_Detalles + " WHERE " + Ope_Nom_Domingos_Empleado_Detalles.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "')";
                    }
                }

                if (Datos.P_No_Domingo != null)
                {
                    Mi_SQL = Mi_SQL + " AND " + Ope_Nom_Domingos_Empleado.Tabla_Cat_Nom_Ope_Nom_Domingos + "." + Ope_Nom_Domingos_Empleado.Campo_No_Domingo + " = '" + Datos.P_No_Domingo + "'";
                }
                if (Datos.P_Estatus != null)
                {
                    Mi_SQL = Mi_SQL + " AND " + Ope_Nom_Domingos_Empleado.Tabla_Cat_Nom_Ope_Nom_Domingos + "." + Ope_Nom_Domingos_Empleado.Campo_Estatus + " = '" + Datos.P_Estatus + "'";
                }
                if (Datos.P_Dependencia_ID != null)
                {
                    Mi_SQL = Mi_SQL + " AND " + Ope_Nom_Domingos_Empleado.Tabla_Cat_Nom_Ope_Nom_Domingos + "." + Ope_Nom_Domingos_Empleado.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "'";
                }
                if (String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Inicio) != "01/01/0001" && String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Final) != "01/01/0001")
                {
                    Mi_SQL = Mi_SQL + " AND " + Ope_Nom_Domingos_Empleado.Tabla_Cat_Nom_Ope_Nom_Domingos + "." + Ope_Nom_Domingos_Empleado.Campo_Fecha + 
                        " BETWEEN TO_DATE ('" + string.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Inicio) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')";
                    Mi_SQL = Mi_SQL + " AND TO_DATE ('" + string.Format("{0:dd/MM/yyyy}",  Datos.P_Fecha_Final) + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                }
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
            finally
            {
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Domingos_Empleado
        /// DESCRIPCION : Consulta todos los Domingos Trabajados que estan dadas de alta en
        ///               la BD con todos sus datos
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 02-Diciembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Domingos_Empleado(Cls_Ope_Nom_Domingos_Empleados_Negocios Datos)
        {
            String Mi_SQL; //Variable para la consulta del domingo trabajado

            try
            {
                //Consulta todos los datos del Tipo de Nómina que se fue seleccionado por el usuario
                Mi_SQL = "SELECT count(*) As Dias_Domingo_Laborados " +
                 " FROM " +
                 Ope_Nom_Domingos_Empleado.Tabla_Cat_Nom_Ope_Nom_Domingos +
                 " WHERE " +
                 Ope_Nom_Domingos_Empleado.Campo_Estatus + "='ACEPTADO'" +
                 " AND " +
                 " (" + Ope_Nom_Domingos_Empleado.Campo_Fecha + " BETWEEN TO_DATE('" + string.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Inicio) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS') AND TO_DATE('" + string.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Final) + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS'))" +
                 " AND " + Ope_Nom_Domingos_Empleado.Campo_No_Domingo +
                 " IN " +
                 " (SELECT " + Ope_Nom_Domingos_Empleado_Detalles.Campo_No_Domingo +
                 " FROM " + Ope_Nom_Domingos_Empleado_Detalles.Tabla_Ope_Nom_Domingos_Empleados_Detalles +
                 " WHERE " + Ope_Nom_Domingos_Empleado_Detalles.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'" +
                 " AND " + Ope_Nom_Domingos_Empleado_Detalles.Campo_Estatus + "='ACEPTADO')";

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
        
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Domingos
        /// DESCRIPCION : Consulta todos los Domingos Trabajados con sus detalles
        /// PARAMETROS  : Parametros: Indica que registro se desea consultar a la base de datos
        /// CREO        : Francisco Antonio Gallardo Castañeda
        /// FECHA_CREO  : 11-Abril-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Domingos(Cls_Ope_Nom_Domingos_Empleados_Negocios Parametros) {
            DataSet Ds_Domingos = null;
            DataTable Dt_Domingos = new DataTable();
            try {
                String Mi_SQL = "SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " AS EMPLEADO_ID";
                Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno; 
                Mi_SQL = Mi_SQL + " ||' '||  " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno;
                Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " AS NOMBRE";
                Mi_SQL = Mi_SQL + ", " + Ope_Nom_Domingos_Empleado_Detalles.Tabla_Ope_Nom_Domingos_Empleados_Detalles + "." + Ope_Nom_Domingos_Empleado_Detalles.Campo_Estatus + " AS ESTATUS";
                Mi_SQL = Mi_SQL + ", " + Ope_Nom_Domingos_Empleado_Detalles.Tabla_Ope_Nom_Domingos_Empleados_Detalles + "." + Ope_Nom_Domingos_Empleado_Detalles.Campo_Comentarios_Estatus + " AS COMENTARIOS_ESTATUS";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + ", " + Ope_Nom_Domingos_Empleado_Detalles.Tabla_Ope_Nom_Domingos_Empleados_Detalles;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID;
                Mi_SQL = Mi_SQL + " = " + Ope_Nom_Domingos_Empleado_Detalles.Tabla_Ope_Nom_Domingos_Empleados_Detalles + "." + Ope_Nom_Domingos_Empleado_Detalles.Campo_Empleado_ID;
                Mi_SQL = Mi_SQL + " AND " + Ope_Nom_Domingos_Empleado_Detalles.Tabla_Ope_Nom_Domingos_Empleados_Detalles + "." + Ope_Nom_Domingos_Empleado_Detalles.Campo_No_Domingo;
                Mi_SQL = Mi_SQL + " = '" + Parametros.P_No_Domingo + "'";
                Ds_Domingos = OracleHelper.ExecuteDataset(Constantes.Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Domingos != null) {
                    Dt_Domingos = Ds_Domingos.Tables[0];
                }
            } catch (Exception Ex) {
                String Mensaje = "Error al consultar:[" + Ex.Message + "]";
            }
            return Dt_Domingos;
        }


        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Cambiar_Estatus_Domingo
        /// DESCRIPCION :   Cambia el estatus de un domingo trabajado para un empleado, si será
        ///                 autorizado o rechazado.
        /// PARAMETROS  : 
        ///                 1. Parametros. Indica que registro se desea actualizar a la 
        ///                                 base de datos.
        /// CREO        : Francisco Antonio Gallardo Castañeda
        /// FECHA_CREO  : 13-Abril-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Cambiar_Estatus_Domingo(Cls_Ope_Nom_Domingos_Empleados_Negocios Parametros){
            String Mi_SQL;
            Boolean Operacion_Exitosa = false;
            try {
                Mi_SQL = "UPDATE " + Ope_Nom_Domingos_Empleado_Detalles.Tabla_Ope_Nom_Domingos_Empleados_Detalles + " SET " +
                        Ope_Nom_Domingos_Empleado_Detalles.Campo_Estatus + "='" + Parametros.P_Estatus + "', " +
                        Ope_Nom_Domingos_Empleado_Detalles.Campo_Comentarios_Estatus + "='" + Parametros.P_Comentarios + "' WHERE " +
                        Ope_Nom_Domingos_Empleado_Detalles.Campo_No_Domingo + "='" + Parametros.P_No_Domingo + "' AND " +
                        Ope_Nom_Domingos_Empleado_Detalles.Campo_Empleado_ID + "='" + Parametros.P_Empleado_ID + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Operacion_Exitosa = true;
            } catch (Exception Ex) {
                throw new Exception(Ex.Message);
            }
            return Operacion_Exitosa;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Informacion_Empleado_Incidencias
        /// DESCRIPCION : Consulta la informacion necesaria que se mostrara en las tablas de empleadas
        ///               ocupadas para el control de incidencias.
        /// PARAMETROS  : Datos: Obtiene que Empleado desea eliminar de la BD
        /// CREO        : JUan Alberto Hernández Negrete
        /// FECHA_CREO  : 16/Marzo/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Informacion_Empleado_Incidencias(String  Empleado_ID)
        {
            String Mi_SQL = "";//Variable que almacenara la consulta.
            DataTable Dt_Empleados = null;//Variable que almacenara la informacion del empleado consultado.

            try
            {
                Mi_SQL = "SELECT " +
                        Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " AS NO_EMPLEADO, " +
                        "(" +
                        Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + "||' '||" +
                        Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" +
                        Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ") AS NOMBRE_EMPLEADO, " +
                        "(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Salario_Diario + ") AS PAGO_DIA_NORMAL, " +
                         Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Nombre + " AS PUESTO" +
                        " FROM " + Cat_Empleados.Tabla_Cat_Empleados + ", " + Cat_Puestos.Tabla_Cat_Puestos +
                        " WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "='" + Empleado_ID + "' " +
                        " AND (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Puesto_ID + "=" +
                        Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Puesto_ID + ")";

                Dt_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error generado al consultar la información que se mostrara en las incidencias de los empleados. Error: [" + Ex.Message + "]");
            }
            return Dt_Empleados;
        }


        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Reporte_Prima_Dominical
        /// DESCRIPCION : Consulta los detalles referentes al tiempo extra
        /// PARAMETROS  : Datos: Contiene los datos que serán Eliminados en la base de datos
        /// CREO        : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 02/Abril/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Reporte_Prima_Dominical(Cls_Ope_Nom_Domingos_Empleados_Negocios Datos)
        {
            DataTable Dt_Consulta = new DataTable();
            StringBuilder Mi_SQL = new StringBuilder();
            try
            {
                Mi_SQL.Append("Select ");
                Mi_SQL.Append("(cast(" + Ope_Nom_Domingos_Empleado.Tabla_Cat_Nom_Ope_Nom_Domingos + "." + Ope_Nom_Domingos_Empleado.Campo_No_Domingo + " as decimal(10,5) )) as NUMERO_TIEMPO_EXTRA, ");

                Mi_SQL.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno);
                Mi_SQL.Append(" || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno);
                Mi_SQL.Append(" || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") as Nombre_Empleado,");

                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ");
                Mi_SQL.Append(Ope_Nom_Domingos_Empleado.Tabla_Cat_Nom_Ope_Nom_Domingos + "." + Ope_Nom_Domingos_Empleado.Campo_Estatus + ", ");
                Mi_SQL.Append(Ope_Nom_Domingos_Empleado.Tabla_Cat_Nom_Ope_Nom_Domingos + "." + Ope_Nom_Domingos_Empleado.Campo_Comentarios + " as Comentario, ");
                Mi_SQL.Append(Ope_Nom_Domingos_Empleado_Detalles.Tabla_Ope_Nom_Domingos_Empleados_Detalles + "." + Ope_Nom_Domingos_Empleado_Detalles.Campo_Estatus + " as Estatus_Detalle, ");
                Mi_SQL.Append(Ope_Nom_Domingos_Empleado_Detalles.Tabla_Ope_Nom_Domingos_Empleados_Detalles + "." + Ope_Nom_Domingos_Empleado_Detalles.Campo_Comentarios_Estatus + " as Comentario_Detalle ");

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Ope_Nom_Domingos_Empleado.Tabla_Cat_Nom_Ope_Nom_Domingos);

                Mi_SQL.Append(" left outer join " + Ope_Nom_Domingos_Empleado_Detalles.Tabla_Ope_Nom_Domingos_Empleados_Detalles + " on ");
                Mi_SQL.Append(Ope_Nom_Domingos_Empleado.Tabla_Cat_Nom_Ope_Nom_Domingos + "." + Ope_Nom_Domingos_Empleado.Campo_No_Domingo + "=");
                Mi_SQL.Append(Ope_Nom_Domingos_Empleado_Detalles.Tabla_Ope_Nom_Domingos_Empleados_Detalles + "." + Ope_Nom_Domingos_Empleado_Detalles.Campo_No_Domingo);

                Mi_SQL.Append(" left outer join " + Cat_Empleados.Tabla_Cat_Empleados + " on ");
                Mi_SQL.Append(Ope_Nom_Domingos_Empleado_Detalles.Tabla_Ope_Nom_Domingos_Empleados_Detalles + "." + Ope_Nom_Domingos_Empleado_Detalles.Campo_Empleado_ID + "=");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);

                Mi_SQL.Append(" WHERE ");

                Mi_SQL.Append(Ope_Nom_Domingos_Empleado.Tabla_Cat_Nom_Ope_Nom_Domingos + "." + Ope_Nom_Domingos_Empleado.Campo_No_Domingo);
                Mi_SQL.Append("='" + Datos.P_No_Domingo + "' ");

                Mi_SQL.Append(" ORDER BY Nombre_Empleado ");

                Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                return Dt_Consulta;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Eliminar el tiempo extra. Error:[" + Ex.Message + "]");
            }
        }
    }
}