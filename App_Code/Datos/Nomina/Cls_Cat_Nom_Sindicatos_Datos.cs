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
using Presidencia.Sindicatos.Negocios;
using Presidencia.Ayudante_Calendario_Nomina;

namespace Presidencia.Sindicatos.Datos
{
    public class Cls_Cat_Nom_Sindicatos_Datos
    {
        #region (Metodos)

        #region (Metodos Operacion [Alta - Modificar - Eliminar])
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Sindicato
        /// DESCRIPCION : 1.Consulta el último ID dado de alta para poder ingresar el siguiente
        ///               2. Da de Alta el Sindicato en la BD con los datos proporcionados
        ///                  por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 30-Agosto-2010
        /// MODIFICO          : Juan Alberto Hernández Negrete
        /// FECHA_MODIFICO    : 11/Noviembre/2010
        /// CAUSA_MODIFICACION: Agregar relacion con Percepciones y Deducciones
        ///*******************************************************************************
        public static void Alta_Sindicato(Cls_Cat_Nom_Sindicatos_Negocio Datos)
        {
            String Mi_SQL;    //Obtiene la cadena de inserción hacía la base de datos
            Object Sindicato_ID; //Obtiene el ID con la cual se guardo los datos en la base de datos
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
                Mi_SQL = "SELECT NVL(MAX(" + Cat_Nom_Sindicatos.Campo_Sindicato_ID + "),'00000') ";
                Mi_SQL = Mi_SQL + "FROM " + Cat_Nom_Sindicatos.Tabla_Cat_Nom_Sindicatos;
                Sindicato_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(Sindicato_ID))
                {
                    Datos.P_Sindicato_ID = "00001";
                }
                else
                {
                    Datos.P_Sindicato_ID = String.Format("{0:00000}", Convert.ToInt32(Sindicato_ID) + 1);
                }
                //Consulta para la inserción del Sindicato con los datos proporcionados por el usuario
                Mi_SQL = "INSERT INTO " + Cat_Nom_Sindicatos.Tabla_Cat_Nom_Sindicatos + " (";
                Mi_SQL = Mi_SQL + Cat_Nom_Sindicatos.Campo_Sindicato_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Sindicatos.Campo_Nombre + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Sindicatos.Campo_Responsable + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Sindicatos.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Sindicatos.Campo_Comentarios + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Sindicatos.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Sindicatos.Campo_Fecha_Creo + ") VALUES ('";
                Mi_SQL = Mi_SQL + Datos.P_Sindicato_ID + "', '" + Datos.P_Nombre + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Responsable + "', '" + Datos.P_Estatus + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Comentarios + "', '" + Datos.P_Nombre_Usuario + "', SYSDATE)";

                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                //Agrega las Percepciones que le perteneceran al sindicato seleccionado.
                if (Datos.P_Dt_Percepciones.Rows != null)
                {
                    foreach (DataRow Renglon in Datos.P_Dt_Percepciones.Rows)
                    {
                        Mi_SQL = "INSERT INTO " + Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Sin_Per_Ded_Det + " (" +
                        Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Sindicato_ID + ", " +
                        Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID + ", " +
                        Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Cantidad + ") " +
                        "VALUES(" +
                        "'" + Datos.P_Sindicato_ID + "', " +
                        "'" + Renglon[Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID].ToString() + "', " +
                        "" + Convert.ToDouble(Renglon[Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Cantidad].ToString()) + ")";

                        Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                        Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                    }
                }

                //Agrega las Percepciones que le perteneceran al sindicato seleccionado.
                if (Datos.P_Dt_Deducciones.Rows != null)
                {
                    foreach (DataRow Renglon in Datos.P_Dt_Deducciones.Rows)
                    {
                        Mi_SQL = "INSERT INTO " + Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Sin_Per_Ded_Det + " (" +
                        Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Sindicato_ID + ", " +
                        Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID + ", " +
                        Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Cantidad + ") " +
                        "VALUES(" +
                        "'" + Datos.P_Sindicato_ID + "', " +
                        "'" + Renglon[Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID].ToString() + "', " +
                        "" + Convert.ToDouble(Renglon[Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Cantidad].ToString()) + ")";

                        Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                        Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                    }
                }

                //Agrega las Antiguedades que le perteneceran al sindicato seleccionado.
                if (Datos.P_Dt_Deducciones.Rows != null)
                {
                    foreach (DataRow Renglon in Datos.P_Dt_Antiguedad_Sindicatos.Rows)
                    {
                        Mi_SQL = "INSERT INTO " + Cat_Nom_Ant_Sin_Det.Tabla_Cat_Nom_Ant_Sin_Det + " (" +
                        Cat_Nom_Ant_Sin_Det.Campo_Sindicato_ID + ", " +
                        Cat_Nom_Ant_Sin_Det.Campo_Antiguedad_Sindicato_ID + ", " +
                        Cat_Nom_Ant_Sin_Det.Campo_Monto + ") " +
                        "VALUES(" +
                        "'" + Datos.P_Sindicato_ID + "', " +
                        "'" + Renglon[Cat_Nom_Ant_Sin_Det.Campo_Antiguedad_Sindicato_ID].ToString() + "', " +
                        "" + Convert.ToDouble(Renglon[Cat_Nom_Ant_Sin_Det.Campo_Monto].ToString()) + ")";

                        Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                        Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                    }
                }

                //Actualiza las Percepciones y/o Deducciones del Empleado. 
                Actualizar_Percepciones_Deducciones_Sindicato(Datos);

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
        /// NOMBRE DE LA FUNCION: Modificar_Sindicato
        /// DESCRIPCION : Modifica los datos del Sindicato con lo que fueron introducidos por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán modificados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 30-Agosto-2010
        /// MODIFICO          : Juan Alberto Hernández Negrete
        /// FECHA_MODIFICO    : 11/Noviembre/2010
        /// CAUSA_MODIFICACION: Agregar relacion con Percepciones y Deducciones
        ///*******************************************************************************
        public static void Modificar_Sindicato(Cls_Cat_Nom_Sindicatos_Negocio Datos)
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
                //Consulta para la modificación del Sindicato con los datos proporcionados por el usuario
                Mi_SQL = "UPDATE " + Cat_Nom_Sindicatos.Tabla_Cat_Nom_Sindicatos + " SET ";
                Mi_SQL = Mi_SQL + Cat_Nom_Sindicatos.Campo_Nombre + " = '" + Datos.P_Nombre + "', ";
                Mi_SQL = Mi_SQL + Cat_Nom_Sindicatos.Campo_Responsable + " = '" + Datos.P_Responsable + "', ";
                Mi_SQL = Mi_SQL + Cat_Nom_Sindicatos.Campo_Estatus + " = '" + Datos.P_Estatus + "', ";
                Mi_SQL = Mi_SQL + Cat_Nom_Sindicatos.Campo_Comentarios + " = '" + Datos.P_Comentarios + "', ";
                Mi_SQL = Mi_SQL + Cat_Nom_Sindicatos.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";
                Mi_SQL = Mi_SQL + Cat_Nom_Sindicatos.Campo_Fecha_Modifico + " = SYSDATE WHERE ";
                Mi_SQL = Mi_SQL + Cat_Nom_Sindicatos.Campo_Sindicato_ID + " = '" + Datos.P_Sindicato_ID + "'";

                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                //Elimina las percepciones que le pertenecen a la percepcion seleccionada.
                Mi_SQL = "DELETE FROM " + Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Sin_Per_Ded_Det +
                    " WHERE " + Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Sindicato_ID + "='" + Datos.P_Sindicato_ID + "'";

                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                //Agrega las Percepciones que le perteneceran al sindicato seleccionado.
                if (Datos.P_Dt_Percepciones.Rows != null)
                {
                    foreach (DataRow Renglon in Datos.P_Dt_Percepciones.Rows)
                    {
                        Mi_SQL = "INSERT INTO " + Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Sin_Per_Ded_Det + " (" +
                        Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Sindicato_ID + ", " +
                        Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID + ", " +
                        Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Cantidad + ") " +
                        "VALUES(" +
                        "'" + Datos.P_Sindicato_ID + "', " +
                        "'" + Renglon[Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID].ToString() + "', " +
                        "" + Convert.ToDouble(Renglon[Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Cantidad].ToString()) + ")";

                        Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                        Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                    }
                }

                //Agrega las Deducciones que le perteneceran al sindicato seleccionado.
                if (Datos.P_Dt_Deducciones.Rows != null)
                {
                    foreach (DataRow Renglon in Datos.P_Dt_Deducciones.Rows)
                    {
                        Mi_SQL = "INSERT INTO " + Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Sin_Per_Ded_Det + " (" +
                        Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Sindicato_ID + ", " +
                        Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID + ", " +
                        Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Cantidad + ") " +
                        "VALUES(" +
                        "'" + Datos.P_Sindicato_ID + "', " +
                        "'" + Renglon[Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID].ToString() + "', " +
                        "" + Convert.ToDouble(Renglon[Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Cantidad].ToString()) + ")";

                        Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                        Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                    }
                }

                //Elimina las antiguedades que le pertenecen al sindicato seleccionado.
                Mi_SQL = "DELETE FROM " + Cat_Nom_Ant_Sin_Det.Tabla_Cat_Nom_Ant_Sin_Det +
                    " WHERE " + Cat_Nom_Ant_Sin_Det.Campo_Sindicato_ID + "='" + Datos.P_Sindicato_ID + "'";

                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                //Agrega las Antiguedades que le perteneceran al sindicato seleccionado.
                if (Datos.P_Dt_Deducciones.Rows != null)
                {
                    foreach (DataRow Renglon in Datos.P_Dt_Antiguedad_Sindicatos.Rows)
                    {
                        Mi_SQL = "INSERT INTO " + Cat_Nom_Ant_Sin_Det.Tabla_Cat_Nom_Ant_Sin_Det + " (" +
                        Cat_Nom_Ant_Sin_Det.Campo_Sindicato_ID + ", " +
                        Cat_Nom_Ant_Sin_Det.Campo_Antiguedad_Sindicato_ID + ", " +
                        Cat_Nom_Ant_Sin_Det.Campo_Monto + ") " +
                        "VALUES(" +
                        "'" + Datos.P_Sindicato_ID + "', " +
                        "'" + Renglon[Cat_Nom_Ant_Sin_Det.Campo_Antiguedad_Sindicato_ID].ToString() + "', " +
                        "" + Convert.ToDouble(Renglon[Cat_Nom_Ant_Sin_Det.Campo_Monto].ToString()) + ")";

                        Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                        Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                    }
                }

                //Actualiza las Percepciones y/o Deducciones del Empleado. 
                Actualizar_Percepciones_Deducciones_Sindicato(Datos);

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
        /// NOMBRE DE LA FUNCION: Eliminar_Sindicato
        /// DESCRIPCION : Elimina el Sindicato que fue seleccionado por el usuario de la BD
        /// PARAMETROS  : Datos: Obtiene que Sindicato desea eliminar de la BD
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 30-Agosto-2010
        /// MODIFICO          : Juan Alberto Hernández Negrete
        /// FECHA_MODIFICO    : 11/Noviembre/2010
        /// CAUSA_MODIFICACION: Agregar relacion con Percepciones y Deducciones
        ///*******************************************************************************
        public static void Eliminar_Sindicato(Cls_Cat_Nom_Sindicatos_Negocio Datos)
        {
            String Mi_SQL; //Variable de Consulta para la eliminación del sindicato
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
                //Elimina las percepciones que le pertenecen a la percepcion seleccionada.
                Mi_SQL = "DELETE FROM " + Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Sin_Per_Ded_Det +
                    " WHERE " + Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Sindicato_ID + "='" + Datos.P_Sindicato_ID + "'";

                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                //Elimina las antiguedades que le pertenecen al sindicato seleccionado.
                Mi_SQL = "DELETE FROM " + Cat_Nom_Ant_Sin_Det.Tabla_Cat_Nom_Ant_Sin_Det +
                    " WHERE " + Cat_Nom_Ant_Sin_Det.Campo_Sindicato_ID + "='" + Datos.P_Sindicato_ID + "'";

                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                //Elimina el sindicato seleccionado.
                Mi_SQL = "DELETE FROM " + Cat_Nom_Sindicatos.Tabla_Cat_Nom_Sindicatos +
                    " WHERE " + Cat_Nom_Sindicatos.Campo_Sindicato_ID + "='" + Datos.P_Sindicato_ID + "'";

                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

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
        /// NOMBRE DE LA FUNCION: Actualizar_Percepciones_Deducciones_Sindicato
        /// DESCRIPCION : Actualiza las percepciones y/o deducciones que tiene el sindicato
        ///               y las cuales se le actualizaran al empleado.
        /// 
        /// PARAMETROS  : Datos: Obtiene la informacion del Sindicato.
        /// 
        /// CREO        : Juan Alberto Hernández Negrete.
        /// FECHA_CREO  : 05/Enero/2011
        /// MODIFICO          : 
        /// FECHA_MODIFICO    : 
        /// CAUSA_MODIFICACION: 
        ///*******************************************************************************
        private static void Actualizar_Percepciones_Deducciones_Sindicato(Cls_Cat_Nom_Sindicatos_Negocio Datos)
        {
            String Mi_SQL = "";//Variable que almacenará la consulta.
            DataTable Dt_Empleados = null;//Variable que almacenrá una lista de empleado que pertencen a este sindicato.
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;                                                  //Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso       
            String Nomina_ID = String.Empty;
            String No_Nomina = String.Empty; 

            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos

            try
            {
                Nomina_ID = new Cls_Ayudante_Calendario_Nomina().P_Nomina_ID;
                No_Nomina = new Cls_Ayudante_Calendario_Nomina().P_Periodo;

                Mi_SQL = "SELECT " + Cat_Empleados.Tabla_Cat_Empleados + ".* " +
                         " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                         " WHERE " + Cat_Empleados.Campo_Sindicato_ID + "='" + Datos.P_Sindicato_ID + "'";

                Dt_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                foreach (DataRow Renglon in Dt_Empleados.Rows)
                {
                    //Paso I.- Eliminamos las Percepciones Deducciones del Empleado que tiene por su sindicato.
                    Mi_SQL = "DELETE FROM " + Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det +
                             " WHERE " + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID + "='" + Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID] + "'" +
                             " AND " + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Concepto + "='SINDICATO'";

                    Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                    Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    //Paso II.- Actualizar Percepciones Sindicales.
                    foreach (DataRow Renglon_Percepcion in Datos.P_Dt_Percepciones.Rows)
                    {
                        Mi_SQL = "INSERT INTO " + Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det +
                                 " (" +
                                 Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID + ", " +
                                 Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID + ", " +
                                 Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Concepto + ", " + 
                                 Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad + ", " +
                                 Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Nomina_ID + ", " + 
                                 Cat_Nom_Emp_Perc_Dedu_Deta.Campo_No_Nomina +
                                 ") VALUES(" +
                                 "'" + Renglon[Cat_Empleados.Campo_Empleado_ID].ToString() + "', " +
                                 "'" + Renglon_Percepcion[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID].ToString() + "', " +
                                 "'SINDICATO', " +
                                 Renglon_Percepcion[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad].ToString() + ", " +
                                 "'" + Nomina_ID + "', " +
                                 No_Nomina +
                                 ")";

                        Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                        Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                    }
                    //Paso III.- Actualizar Deducciones Sindicales.
                    foreach (DataRow Renglon_Deducciones in Datos.P_Dt_Deducciones.Rows)
                    {
                        Mi_SQL = "INSERT INTO " + Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det +
                                 " (" +
                                 Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID + ", " +
                                 Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID + ", " +
                                 Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Concepto + ", " +
                                 Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad + ", " +
                                 Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Nomina_ID + ", " +
                                 Cat_Nom_Emp_Perc_Dedu_Deta.Campo_No_Nomina +
                                 ") VALUES(" +
                                 "'" + Renglon[Cat_Empleados.Campo_Empleado_ID].ToString() + "', " +
                                 "'" + Renglon_Deducciones[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID].ToString() + "', " +
                                 "'SINDICATO', " +
                                 Renglon_Deducciones[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad].ToString() + ", " +
                                 "'" + Nomina_ID + "', " +
                                 No_Nomina +
                                 ")";

                        Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                        Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                    }
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
        #endregion

        #region (Metodo de Consulta)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Datos_Sindicato
        /// DESCRIPCION : Consulta todos los datos del Sindicato que estan dados de alta en
        ///               la BD con todos sus datos
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 30-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Datos_Sindicato(Cls_Cat_Nom_Sindicatos_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de los Sindicatos

            try
            {
                //Consulta todos los datos del Sindicato que se fue seleccionado por el usuario
                Mi_SQL = "SELECT " + Cat_Nom_Sindicatos.Tabla_Cat_Nom_Sindicatos + ".*  FROM " + Cat_Nom_Sindicatos.Tabla_Cat_Nom_Sindicatos;

                if (Datos.P_Sindicato_ID != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Nom_Sindicatos.Tabla_Cat_Nom_Sindicatos + "." + Cat_Nom_Sindicatos.Campo_Sindicato_ID +
                        " = '" + Datos.P_Sindicato_ID + "'";
                }
                if (Datos.P_Nombre != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE UPPER(" + Cat_Nom_Sindicatos.Tabla_Cat_Nom_Sindicatos + "." + Cat_Nom_Sindicatos.Campo_Nombre +
                        ") LIKE UPPER('%" + Datos.P_Nombre + "%')";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Nom_Sindicatos.Tabla_Cat_Nom_Sindicatos + "." + Cat_Nom_Sindicatos.Campo_Nombre;
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
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
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Sindicato
        /// DESCRIPCION : Consulta los Sindicatos que estan dados de alta en la BD 
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 30-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Sindicato(Cls_Cat_Nom_Sindicatos_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de los Sindicatos

            try
            {
                //Consulta los Sindicatos que estan dados de alta en la base de datos
                Mi_SQL = "SELECT " + Cat_Nom_Sindicatos.Campo_Sindicato_ID + ", " + Cat_Nom_Sindicatos.Campo_Nombre;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Nom_Sindicatos.Tabla_Cat_Nom_Sindicatos;
                if (Datos.P_Sindicato_ID != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Nom_Sindicatos.Campo_Sindicato_ID + " = '" + Datos.P_Sindicato_ID + "'";
                }
                if (Datos.P_Nombre != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE UPPER(" + Cat_Nom_Sindicatos.Campo_Nombre + ") LIKE UPPER('%" + Datos.P_Nombre + "%')";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Nom_Sindicatos.Campo_Nombre;
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
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
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Percepciones_Deducciones
        /// DESCRIPCION :Cosnulta las percepciones deducciones que pertenecen a un
        /// determinado sindicato.
        /// CREO        : Juan ALberto Hernández Negrete
        /// FECHA_CREO  : 10/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Percepciones_Deducciones(Cls_Cat_Nom_Sindicatos_Negocio Datos)
        {
            String Mi_Oracle = "";
            try
            {
                Mi_Oracle = "SELECT " +
                    Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + ", " +
                    Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Nombre + ", " +
                    Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Clave + ", " +
                    Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Sin_Per_Ded_Det + "." + Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Cantidad + ", " +
                     Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion +

                    " FROM " +
                    Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + " INNER JOIN " + Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Sin_Per_Ded_Det +
                    " ON " +
                    Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID +
                    "=" +
                   Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Sin_Per_Ded_Det + "." + Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID +

                    " WHERE " +
                   Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Sin_Per_Ded_Det + "." +
                   Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Sindicato_ID + "='" + Datos.P_Sindicato_ID + "' AND " +
                   Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + " IN " +

                    " ( SELECT " +
                    Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Sin_Per_Ded_Det + "." +
                    Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID + " FROM " +
                    Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Sin_Per_Ded_Det + " INNER JOIN " +
                    Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + " ON " +
                    Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Sin_Per_Ded_Det + "." + Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID + "=" +
                    Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID +

                    " WHERE " +
                   Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Sin_Per_Ded_Det + "." +
                   Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Sindicato_ID + "='" + Datos.P_Sindicato_ID + "' AND " +
                   Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Tipo + "='" + Datos.P_Tipo_Percepcion + "'" +
                   ")";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Percepciones_Deducciones
        /// DESCRIPCION :Cosnulta las percepciones deducciones.
        /// CREO        : Juan ALberto Hernández Negrete
        /// FECHA_CREO  : 10/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Percepciones_Deducciones_Generales(Cls_Cat_Nom_Sindicatos_Negocio Datos)
        {
            String Mi_Oracle = "";
            try
            {
                Mi_Oracle = "SELECT " + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + ", (" + Cat_Nom_Percepcion_Deduccion.Campo_Clave + " || ' ' || " + Cat_Nom_Percepcion_Deduccion.Campo_Nombre + ") AS NOMBRE_CONCEPTO" +
                    " FROM " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + " WHERE " +
                    Cat_Nom_Percepcion_Deduccion.Campo_Tipo + "='" + Datos.P_Tipo_Percepcion + "' AND " +
                    Cat_Nom_Percepcion_Deduccion.Campo_Estatus + "='ACTIVO' AND " +
                    Cat_Nom_Percepcion_Deduccion.Campo_Concepto + "='SINDICATO' AND (" +
                    Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion + "='FIJA' OR " +
                    Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion + "='OPERACION')";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Antiguedades_Sindicato
        /// DESCRIPCION :Cosnulta las antiguedades que le pertenecen al sindicato o aun
        ///              determinado sindicato.
        /// CREO        : Juan ALberto Hernández Negrete
        /// FECHA_CREO  : 27/Diciembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Antiguedades_Sindicales(Cls_Cat_Nom_Sindicatos_Negocio Datos)
        {
            String Mi_Oracle = "";
            try
            {
                Mi_Oracle = "SELECT " +
                    Cat_Nom_Antiguedad_Sindicato.Tabla_Cat_Nom_Antiguedad_Sindicato + "." + Cat_Nom_Antiguedad_Sindicato.Campo_Antiguedad_Sindicato_ID + ", " +
                    Cat_Nom_Antiguedad_Sindicato.Tabla_Cat_Nom_Antiguedad_Sindicato + "." + Cat_Nom_Antiguedad_Sindicato.Campo_Anios + ", " +
                    Cat_Nom_Ant_Sin_Det.Tabla_Cat_Nom_Ant_Sin_Det + "." + Cat_Nom_Ant_Sin_Det.Campo_Monto +

                    " FROM " +
                    Cat_Nom_Antiguedad_Sindicato.Tabla_Cat_Nom_Antiguedad_Sindicato + " INNER JOIN " + Cat_Nom_Ant_Sin_Det.Tabla_Cat_Nom_Ant_Sin_Det +
                    " ON " +
                    Cat_Nom_Antiguedad_Sindicato.Tabla_Cat_Nom_Antiguedad_Sindicato + "." + Cat_Nom_Antiguedad_Sindicato.Campo_Antiguedad_Sindicato_ID +
                    "=" +
                   Cat_Nom_Ant_Sin_Det.Tabla_Cat_Nom_Ant_Sin_Det + "." + Cat_Nom_Ant_Sin_Det.Campo_Antiguedad_Sindicato_ID +

                    " WHERE " +
                   Cat_Nom_Ant_Sin_Det.Tabla_Cat_Nom_Ant_Sin_Det + "." +
                   Cat_Nom_Ant_Sin_Det.Campo_Sindicato_ID + "='" + Datos.P_Sindicato_ID + "' AND " +
                   Cat_Nom_Antiguedad_Sindicato.Tabla_Cat_Nom_Antiguedad_Sindicato + "." + Cat_Nom_Antiguedad_Sindicato.Campo_Antiguedad_Sindicato_ID + " IN " +

                    " ( SELECT " +
                    Cat_Nom_Ant_Sin_Det.Tabla_Cat_Nom_Ant_Sin_Det + "." +
                    Cat_Nom_Ant_Sin_Det.Campo_Antiguedad_Sindicato_ID + " FROM " +
                    Cat_Nom_Ant_Sin_Det.Tabla_Cat_Nom_Ant_Sin_Det + " INNER JOIN " +
                    Cat_Nom_Antiguedad_Sindicato.Tabla_Cat_Nom_Antiguedad_Sindicato + " ON " +
                    Cat_Nom_Ant_Sin_Det.Tabla_Cat_Nom_Ant_Sin_Det + "." + Cat_Nom_Ant_Sin_Det.Campo_Antiguedad_Sindicato_ID + "=" +
                    Cat_Nom_Antiguedad_Sindicato.Tabla_Cat_Nom_Antiguedad_Sindicato + "." + Cat_Nom_Antiguedad_Sindicato.Campo_Antiguedad_Sindicato_ID +

                    " WHERE " +
                   Cat_Nom_Ant_Sin_Det.Tabla_Cat_Nom_Ant_Sin_Det + "." +
                   Cat_Nom_Ant_Sin_Det.Campo_Sindicato_ID + "='" + Datos.P_Sindicato_ID + "'" +
                   ")";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }
        #endregion

        #endregion

    }
}
