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
using Presidencia.Vacaciones_Empleado.Negocio;
using Presidencia.Empleados.Negocios;
using Presidencia.Vacaciones.Negocios;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Collections.Generic;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using System.Globalization;
using System.Text;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.DateDiff;

namespace Presidencia.Vacaciones_Empleado.Datos
{
    public class Cls_Ope_Nom_Vacaciones_Empleado_Datos
    {
        #region (Metodos)

        #region (Metodos)
        ///**************************************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Vacaciones_Empleado
        /// DESCRIPCION : 1.Consulta el último ID dado de alta para poder ingresar el siguiente
        ///               2. Da de Alta de las vacaciones en la BD con los datos proporcionados por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 23/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///**************************************************************************************************************************************************
        public static Boolean Alta_Vacaciones_Empleado(Cls_Ope_Nom_Vacaciones_Empleado_Negocio Datos)
        {
            String Mi_SQL;    //Obtiene la cadena de inserción hacía la base de datos
            Object No_Vacacion; //Obtiene el ID con la cual se guardo los datos en la base de datos
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;                                                  //Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        
            Boolean Operacion_Completa = false;//Variable que guarda el valor si la operacion se realiza con exito o no.
            Int64 Contador_Detalles_Vacaciones = 0;//Variable que almacenara el consecutivo de los detalles de las vacaciones.

            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }

            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos

            try
            {
                Mi_SQL = "SELECT NVL(MAX(" + OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Vacacion + "),'0000000000') ";
                Mi_SQL = Mi_SQL + "FROM " + OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado;
                No_Vacacion = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(No_Vacacion))
                {
                    Datos.P_No_Vacacion = "0000000001";
                }
                else
                {
                    Datos.P_No_Vacacion = String.Format("{0:0000000000}", Convert.ToInt32(No_Vacacion) + 1);
                }

                Mi_SQL = "INSERT INTO " + OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + " (" +
                    OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Vacacion + ", " +
                    OPE_NOM_VACACIONES_EMPLEADO.Campo_Empleado_ID + ", " +
                    OPE_NOM_VACACIONES_EMPLEADO.Campo_Dependencia_ID + ", " +
                    OPE_NOM_VACACIONES_EMPLEADO.Campo_Fecha_Inicio + ", " +
                    OPE_NOM_VACACIONES_EMPLEADO.Campo_Fecha_Termino + ", " +
                    OPE_NOM_VACACIONES_EMPLEADO.Campo_Fecha_Regreso_Laboral + ", " +
                    OPE_NOM_VACACIONES_EMPLEADO.Campo_Cantidad_Dias + ", " +
                    OPE_NOM_VACACIONES_EMPLEADO.Campo_Estatus + ", " +
                    OPE_NOM_VACACIONES_EMPLEADO.Campo_Comentarios + ", " +
                    OPE_NOM_VACACIONES_EMPLEADO.Campo_Usuario_Creo + ", " +
                    OPE_NOM_VACACIONES_EMPLEADO.Campo_Fecha_Creo + ", " +
                    OPE_NOM_VACACIONES_EMPLEADO.Campo_Estado + ", " +
                    OPE_NOM_VACACIONES_EMPLEADO.Campo_Nomina_ID + ", " +
                    OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Nomina +                     
                    ") VALUES(" +
                    "'" + Datos.P_No_Vacacion + "', " +
                    "'" + Datos.P_Empleado_ID + "', " +
                    "'" + Datos.P_Dependencia_ID + "', " +
                    "'" + Datos.P_Fecha_Inicio + "', " +
                    "'" + Datos.P_Fecha_Termino + "', " +
                    "'" + Datos.P_Fecha_Regreso_Laboral + "', " +
                    "" + Datos.P_Cantidad_Dias + ", " +
                    "'" + Datos.P_Estatus + "', " +
                    "'" + Datos.P_Comentarios + "', " +
                    "'" + Datos.P_Usuario_Creo + "', SYSDATE, 'COMPROMETIDA', ";

                if (!String.IsNullOrEmpty(Datos.P_Nomina_ID))
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Nomina_ID + "', ";
                }
                else {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                if (!String.IsNullOrEmpty(Datos.P_No_Nomina.ToString()))
                {
                    Mi_SQL = Mi_SQL + Datos.P_No_Nomina + ")";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL)";
                }

                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                //Obtenemos el consecutivo de la consulta de identificador del detalles de las vacaciones.
                Contador_Detalles_Vacaciones = Consulta_Identificar_Detalle_Consecutivo();

                if (Datos.P_Dt_Detalles_Vacaciones is DataTable)
                {
                    foreach (DataRow Detalle_Dia_Vacacion in Datos.P_Dt_Detalles_Vacaciones.Rows)
                    {
                        if (Detalle_Dia_Vacacion is DataRow)
                        {
                            Mi_SQL = "INSERT INTO " + Ope_Nom_Vacaciones_Dia_Det.Tabla_Ope_Nom_Vacaciones_Dia_Det + " (" +
                                     Ope_Nom_Vacaciones_Dia_Det.Campo_No_Dia_Vacacion + ", " +
                                     Ope_Nom_Vacaciones_Dia_Det.Campo_No_Vacacion + ", " +
                                     Ope_Nom_Vacaciones_Dia_Det.Campo_Fecha + ", " +
                                     Ope_Nom_Vacaciones_Dia_Det.Campo_Estatus + ", " +
                                     Ope_Nom_Vacaciones_Dia_Det.Campo_Estado + ") VALUES(" +
                                     Contador_Detalles_Vacaciones + ", " +
                                     "'" + Datos.P_No_Vacacion + "', " +
                                     "'" + Detalle_Dia_Vacacion[Ope_Nom_Vacaciones_Dia_Det.Campo_Fecha].ToString() + "', " +
                                     "'" + Detalle_Dia_Vacacion[Ope_Nom_Vacaciones_Dia_Det.Campo_Estatus].ToString() + "', " +
                                     "'" + Detalle_Dia_Vacacion[Ope_Nom_Vacaciones_Dia_Det.Campo_Estado].ToString() + "')";

                            Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                            Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                            ++Contador_Detalles_Vacaciones;//Incrementamos el consecutivo.
                        }
                    }
                }


                Transaccion_SQL.Commit();
                Operacion_Completa = true;
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
            return Operacion_Completa;
        }
        ///*************************************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Modificar_Vacaciones_Empleado
        /// DESCRIPCION : 1.-. Modifica los datos del No vacacion seleccionada.
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 23/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*************************************************************************************************************************************************
        public static Boolean Modificar_Vacaciones_Empleado(Cls_Ope_Nom_Vacaciones_Empleado_Negocio Datos)
        {
            String Mi_SQL;    //Obtiene la cadena de inserción hacía la base de datos
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;                                                  //Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        
            Boolean Operacion_Completa = false;//Variable que guarda el valor si la operacion se realiza con exito o no.
            Int64 Contador_Detalles_Vacaciones = 0;//Variable que almacenara el consecutivo de los detalles de las vacaciones.

            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }

            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos

            try
            {
                Mi_SQL = "UPDATE " + OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + " SET " +
                OPE_NOM_VACACIONES_EMPLEADO.Campo_Fecha_Inicio + "='" + Datos.P_Fecha_Inicio + "', " +
                OPE_NOM_VACACIONES_EMPLEADO.Campo_Fecha_Termino + "='" + Datos.P_Fecha_Termino + "', " +
                OPE_NOM_VACACIONES_EMPLEADO.Campo_Fecha_Regreso_Laboral + "='" + Datos.P_Fecha_Regreso_Laboral + "', " +
                OPE_NOM_VACACIONES_EMPLEADO.Campo_Estado + "='COMPROMETIDA', " +
                OPE_NOM_VACACIONES_EMPLEADO.Campo_Cantidad_Dias + "=" + Datos.P_Cantidad_Dias + ", " +
                OPE_NOM_VACACIONES_EMPLEADO.Campo_Estatus + "='" + Datos.P_Estatus + "', " +
                OPE_NOM_VACACIONES_EMPLEADO.Campo_Comentarios + "='" + Datos.P_Comentarios + "', " +
                OPE_NOM_VACACIONES_EMPLEADO.Campo_Usuario_Creo + "='" + Datos.P_Usuario_Creo + "', " +
                OPE_NOM_VACACIONES_EMPLEADO.Campo_Fecha_Modifico + "= SYSDATE" + ", ";

                if (!String.IsNullOrEmpty(Datos.P_Nomina_ID))
                {
                    Mi_SQL = Mi_SQL + OPE_NOM_VACACIONES_EMPLEADO.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + OPE_NOM_VACACIONES_EMPLEADO.Campo_Nomina_ID + "=NULL, ";
                }

                if (!String.IsNullOrEmpty(Datos.P_No_Nomina.ToString()))
                {
                    Mi_SQL = Mi_SQL + OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Nomina + "=" + Datos.P_No_Nomina;
                }
                else
                {
                    Mi_SQL = Mi_SQL + OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Nomina + "=NULL";
                }

                Mi_SQL = Mi_SQL + " WHERE " + OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Vacacion + "='" + Datos.P_No_Vacacion + "'";

                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos


                Mi_SQL = "DELETE FROM " + Ope_Nom_Vacaciones_Dia_Det.Tabla_Ope_Nom_Vacaciones_Dia_Det +
                         " WHERE " + Ope_Nom_Vacaciones_Dia_Det.Campo_No_Vacacion + "='" + Datos.P_No_Vacacion + "'";

                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                

                //Obtenemos el consecutivo de la consulta de identificador del detalles de las vacaciones.
                Contador_Detalles_Vacaciones = Consulta_Identificar_Detalle_Consecutivo();

                foreach (DataRow Detalle_Dia_Vacacion in Datos.P_Dt_Detalles_Vacaciones.Rows)
                {
                    if (Detalle_Dia_Vacacion is DataRow)
                    {
                        Mi_SQL = "INSERT INTO " + Ope_Nom_Vacaciones_Dia_Det.Tabla_Ope_Nom_Vacaciones_Dia_Det + " (" +
                                 Ope_Nom_Vacaciones_Dia_Det.Campo_No_Dia_Vacacion + ", " +
                                 Ope_Nom_Vacaciones_Dia_Det.Campo_No_Vacacion + ", " +
                                 Ope_Nom_Vacaciones_Dia_Det.Campo_Fecha + ", " +
                                 Ope_Nom_Vacaciones_Dia_Det.Campo_Estatus + ", " +
                                 Ope_Nom_Vacaciones_Dia_Det.Campo_Estado + ") VALUES(" +
                                 Contador_Detalles_Vacaciones + ", " +
                                 "'" + Datos.P_No_Vacacion + "', " +
                                 "'" + Detalle_Dia_Vacacion[Ope_Nom_Vacaciones_Dia_Det.Campo_Fecha].ToString() + "', " +
                                 "'" + Detalle_Dia_Vacacion[Ope_Nom_Vacaciones_Dia_Det.Campo_Estatus].ToString() + "', " +
                                 "'" + Detalle_Dia_Vacacion[Ope_Nom_Vacaciones_Dia_Det.Campo_Estado].ToString() + "')";

                        Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                        Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                        ++Contador_Detalles_Vacaciones;//Incrementamos el consecutivo.
                    }
                }

                Transaccion_SQL.Commit();
                Operacion_Completa = true;
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
            return Operacion_Completa;
        }
        ///*************************************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Eliminar_Vacacion_Empleado
        /// DESCRIPCION : 1.-. Elimina la Vacacion seleccionada.
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 23/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Eliminar_Vacacion_Empleado(Cls_Ope_Nom_Vacaciones_Empleado_Negocio Datos) {
            String Mi_Oracle = "";//Variable que almacenara las consulta a realizar.
            Boolean Operacion_Completa = false;//Variable que guarda el valor si la operacion se realiza con exito o no.

            try
            {
                Mi_Oracle = "DELETE FROM " + Ope_Nom_Vacaciones_Dia_Det.Tabla_Ope_Nom_Vacaciones_Dia_Det + " WHERE " +
                            Ope_Nom_Vacaciones_Dia_Det.Campo_No_Vacacion + "='" + Datos.P_No_Vacacion + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);

                Mi_Oracle = "DELETE FROM " + OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + 
                " WHERE " +
                OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Vacacion + "='" + Datos.P_No_Vacacion + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Operacion_Completa;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Modifica_Estatus_Vacaciones
        /// DESCRIPCION : 1.-. Autoriza o Rechaza las vacaciones al empleado.
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 23/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Modifica_Estatus_Vacaciones(Cls_Ope_Nom_Vacaciones_Empleado_Negocio Datos)
        {
            String Mi_Oracle = "";//Variable que almacenara las consulta a realizar.
            Boolean Operacion_Completa = false;//Variable que guarda el valor si la operacion se realiza con exito o no.

            try
            {
                Mi_Oracle = "UPDATE " + OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + " SET ";

                if (!string.IsNullOrEmpty(Datos.P_Estatus))
                {
                    Mi_Oracle = Mi_Oracle + OPE_NOM_VACACIONES_EMPLEADO.Campo_Estatus + "='" + Datos.P_Estatus + "', ";
                }
                else {
                    Mi_Oracle = Mi_Oracle + OPE_NOM_VACACIONES_EMPLEADO.Campo_Estatus + "='NULL', ";
                }
                if (!string.IsNullOrEmpty(Datos.P_Comentarios_Estatus))
                {
                    Mi_Oracle = Mi_Oracle + OPE_NOM_VACACIONES_EMPLEADO.Campo_Comentarios_Estatus + "='" + Datos.P_Comentarios_Estatus + "'";
                }
                else {
                    Mi_Oracle = Mi_Oracle + OPE_NOM_VACACIONES_EMPLEADO.Campo_Comentarios_Estatus + "='NULL'";
                }
                    
                Mi_Oracle = Mi_Oracle + " WHERE " + OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Vacacion + "='" + Datos.P_No_Vacacion + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Operacion_Completa;
        }
        #endregion

        #region (Metodos Consulta)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Dias_Vacaciones_Empleado
        /// DESCRIPCION : 1. Obtiene el tiempo de antiguedad laboral del empleado.
        ///               2. Obtiene el total de dias de vacaciones que le corresponden segun su antiguedad laboral.
        ///               3.- Obtiene el total de dias de vacaciones tomados por el empleado.
        ///               4.- Obtiene el total de dias que el empleado puede tomar actualmente.
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 23/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Int32 Consultar_Dias_Vacaciones_Empleado(Cls_Ope_Nom_Vacaciones_Empleado_Negocio Datos) {
            String Empleado_ID ="";//Variable que almacenara el identificador unico del empleado.
            Int32 Dias_Derecho_Vacaciones_Empleado = 0;//Dias que le corresponden de vacaciones al empleado.
            Int32 Dias_Vacaciones_Totales_Por_Antiguadad = 0;//Los dias de vacaciones a los que empleado tendria derecho sin nunca hubiera tomado vacaciones desde su ingreso.
            Int32 Dias_Vacaciones_Tomados_Empleado = 0;//Los dias de vacaciones que el empleado a tomado previamente desde su ingreso.
            DateTime Fecha_Ingreso_Laboral_Empleado= new DateTime();//Variable que almacenara la fecha de ingreso del empleado.
            DateTime Fecha_Actual;//Fecha que indica el momento en el se hace la consulta de los dias vacaciones disponibles. No es la fecha en la que se tomaran las vacaciones.
            DateTime Fecha_Contador;//Variable que contendra temporalmete una fecha que se ira incrementeando para validar el tiempo que el empleado lleva laborando.
            Int32 Numero_Anios_Laborados = 0;//Variable que almacenara el numero de años que lleva laborando el empleado en la empresa. 
            Int32 Numero_Meses_Laborados = 0;//Variable que almacenara el numero de meses que lleva laborando el empleado en la empresa. 
            DataTable Dt_Empleados_Consulta = null;//Variable que almacenara una lista de empleados.
            DataTable Dt_Tab_Dias_Vacaciones_Consulta = null;//Variable que almacenara una lista del tabulador de dias de vacaciones.
            DataTable Dt_Vacaciones_Tomadas_Empleado = null;//Variable que almacenara una lista con las vacaciones que ha tomado el empleado previamente.
            String Mi_Oracle = "";//Variable para realizar la consulta.

            try
            {
                //Paso 1.- Consultar la fecha de ingreso del empleado, consultando la FECHA de INICIO LABORAL del EMPLEADO.
                //por su numero de empleado.
                Mi_Oracle = "SELECT " + Cat_Empleados.Campo_Fecha_Inicio + ", "  + Cat_Empleados.Campo_Empleado_ID +
                    " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                    " WHERE " + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "'";
                //Ejecutamos la consulta del empleado con el no de empleado proporcionado.
                Dt_Empleados_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
                //Validamos que exista algun empleado con el nuemero proporcionado.
                if (Dt_Empleados_Consulta != null) {
                    if (Dt_Empleados_Consulta.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(Dt_Empleados_Consulta.Rows[0][Cat_Empleados.Campo_Fecha_Inicio].ToString()))
                        {
                            //Obtenemos la fecha de ingreso del empleado a laborar enla empresa.
                            Fecha_Ingreso_Laboral_Empleado = Convert.ToDateTime(string.Format("{0:dd/MM/yyyy}", Dt_Empleados_Consulta.Rows[0][Cat_Empleados.Campo_Fecha_Inicio].ToString()));
                            //Obtenemos el identificador de empleado el [Empleado_ID]
                            if (!string.IsNullOrEmpty(Dt_Empleados_Consulta.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString())) Empleado_ID = Dt_Empleados_Consulta.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString();
                        }
                        else {
                            return 0;
                        }
                    }
                    else {
                        return 0;
                    }
                }

                //Paso 2.- En base a la fecha de ingreso del empleado calcular el tiempo que el empleado lleva laborando en la empresa.
                Fecha_Actual = DateTime.Now;//Obtenemos la fecha actual.
                Fecha_Contador = Fecha_Ingreso_Laboral_Empleado;//Establecemos apartir de que momento se comenzara a contar los años laborados por el empleado, para obtener su antiguedad en la empresa.
                //Hacemos el barrido para obtener los años laborados en la empresa por el empleado.
                while (DateTime.Compare(Fecha_Contador.AddMonths(1), Fecha_Actual) < 0 || DateTime.Compare(Fecha_Contador.AddMonths(1), Fecha_Actual) == 0)
                {                    
                    Fecha_Contador = Fecha_Contador.AddMonths(1);//Sumamos un mes a la fecha de ingreso.
                    Numero_Meses_Laborados = Numero_Meses_Laborados + 1;//Incrementamos en uno los meses laborados.
                    //Validamos sie le numero de meses laborados es igual a 12
                    //si el empleado lleva 12 meses laborados el nemuro de años se incrementa en 1.
                    if (Numero_Meses_Laborados == 12) {
                        Numero_Meses_Laborados = 0;//Volvemos los meses a cero para comenzar de nuevo.
                        Numero_Anios_Laborados = Numero_Anios_Laborados + 1;//Incrementamos los años 1 año mas.
                    }
                }

                //Paso 3.- Hacer un barrido para obtener el total de dias de vacaciones que le corresponden al empleado por antiguedad.
                for (int Contador_Anios_Laborados = 1; Contador_Anios_Laborados <= Numero_Anios_Laborados; Contador_Anios_Laborados++)
                {
                    //Consultar en la tabla de tabulador de dias vacaciones cuantos dias vacaciones 
                    //le corresponden al empleado de acuerdo a su antiguedad laboal.
                    Mi_Oracle = "SELECT " + Tab_Nom_Vacaciones.Campo_Dias + " FROM " + Tab_Nom_Vacaciones.Tabla_Tab_Nom_Vacaciones + " WHERE " +
                        Tab_Nom_Vacaciones.Campo_Antiguedad + "=" + Contador_Anios_Laborados;
                    //Se ejecuta la consulta de el total de los dias que le corresponden por antiguedad.
                    Dt_Tab_Dias_Vacaciones_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
                    //Validamos que si le corresponden dias al trabajador de acuerdo a us antuguedad.
                    if (Dt_Tab_Dias_Vacaciones_Consulta != null)
                    {
                        //Validamos que minimos lleve un año laborando en la empresa.
                        if (Dt_Tab_Dias_Vacaciones_Consulta.Rows.Count > 0)
                        {
                          //Obtenemos el numero de dias que le corresponde al empleado segun su antiguedad laboral.  
                            if (!string.IsNullOrEmpty(Dt_Tab_Dias_Vacaciones_Consulta.Rows[0][Tab_Nom_Vacaciones.Campo_Dias].ToString())) Dias_Vacaciones_Totales_Por_Antiguadad += Convert.ToInt32(Dt_Tab_Dias_Vacaciones_Consulta.Rows[0][Tab_Nom_Vacaciones.Campo_Dias].ToString());
                        }
                    }
                }

                //Paso 4.- Consultar los dias de vacaciones que previamente a tomado el empleado.
                Mi_Oracle = "SELECT " + OPE_NOM_VACACIONES_EMPLEADO.Campo_Cantidad_Dias + " FROM " + OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado +
                    " WHERE " + OPE_NOM_VACACIONES_EMPLEADO.Campo_Empleado_ID + "='" + Empleado_ID + "'";

                //Ejecutar la consulta de los dias de vacaciones que ya a tomado el empleado.
                Dt_Vacaciones_Tomadas_Empleado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
                //Validamos si ya ha tomado vacaciones anteriormente.
                if (Dt_Vacaciones_Tomadas_Empleado != null) {
                    if (Dt_Vacaciones_Tomadas_Empleado.Rows.Count > 0) {
                        //Validamos que por lo menos exista 1 registro de vacaciones.
                        foreach (DataRow Renglon in Dt_Vacaciones_Tomadas_Empleado.Rows) {
                            //Obtenemos el nuemro de dias de vacaciones que a tomado el empleado.
                            if (!string.IsNullOrEmpty(Renglon[OPE_NOM_VACACIONES_EMPLEADO.Campo_Cantidad_Dias].ToString())) Dias_Vacaciones_Tomados_Empleado += Convert.ToInt32(Renglon[OPE_NOM_VACACIONES_EMPLEADO.Campo_Cantidad_Dias].ToString());
                        }
                    }
                }

                //Paso 5.- Obtenemos el total de dias de vacaciones que le quedan al empleado.
                if (Dias_Vacaciones_Totales_Por_Antiguadad >= Dias_Vacaciones_Tomados_Empleado)
                {
                    Dias_Derecho_Vacaciones_Empleado = Dias_Vacaciones_Totales_Por_Antiguadad - Dias_Vacaciones_Tomados_Empleado;
                }
                else {
                    return 0;
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Dias_Derecho_Vacaciones_Empleado;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Vacaciones
        /// DESCRIPCION : 1.Consulta las vacaciones que existen actualmente registradas ene l sistema.
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 23/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Cls_Ope_Nom_Vacaciones_Empleado_Negocio Consultar_Vacaciones(Cls_Ope_Nom_Vacaciones_Empleado_Negocio Datos)
        {
            String Mi_Oracle = "";//Variable que almacenara las consultas que se realizarn contra la BD.
            DataTable Dt_Vacaciones = null;//Variable que almacenara una lista de Vacaciones.
            Cls_Ope_Nom_Vacaciones_Empleado_Negocio Cls_Transporta_Datos_Vacaciones = new Cls_Ope_Nom_Vacaciones_Empleado_Negocio();

            try
            {
                Mi_Oracle = "SELECT " + OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + ".*, " +

                    " (select ( '[' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " || '] -- ' || " +  
                    Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || " +
                    Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || " + 
                    Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") from " +
                    Cat_Empleados.Tabla_Cat_Empleados +
                    " where " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "=" +
                    OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + "." + Cat_Empleados.Campo_Empleado_ID + ") as EMPLEADO " +

                    " FROM " + OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado;

                if (!string.IsNullOrEmpty(Datos.P_Empleado_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + OPE_NOM_VACACIONES_EMPLEADO.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + OPE_NOM_VACACIONES_EMPLEADO.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_No_Vacacion))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Vacacion + "='" + Datos.P_No_Vacacion + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Vacacion + "='" + Datos.P_No_Vacacion + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Dependencia_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + OPE_NOM_VACACIONES_EMPLEADO.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + OPE_NOM_VACACIONES_EMPLEADO.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Estatus))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + OPE_NOM_VACACIONES_EMPLEADO.Campo_Estatus + "='" + Datos.P_Estatus + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + OPE_NOM_VACACIONES_EMPLEADO.Campo_Estatus + "='" + Datos.P_Estatus + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Fecha_Inicio) && !string.IsNullOrEmpty(Datos.P_Fecha_Termino))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND ((" + OPE_NOM_VACACIONES_EMPLEADO.Campo_Fecha_Inicio + " BETWEEN TO_DATE ('" + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Inicio)) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Termino)) + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS'))";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE ((" + OPE_NOM_VACACIONES_EMPLEADO.Campo_Fecha_Inicio + " BETWEEN TO_DATE ('" + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Inicio)) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Termino)) + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS'))";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Fecha_Inicio) && !string.IsNullOrEmpty(Datos.P_Fecha_Termino))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " OR (" + OPE_NOM_VACACIONES_EMPLEADO.Campo_Fecha_Termino + " BETWEEN TO_DATE ('" + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Inicio)) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Termino)) + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')))";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE (" + OPE_NOM_VACACIONES_EMPLEADO.Campo_Fecha_Termino + " BETWEEN TO_DATE ('" + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Inicio)) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Termino)) + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')))";
                    }
                }
                //Ejecutamos la consulta para obtener una lista de las vacaciones.
                Dt_Vacaciones = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
                //Validamos que por lo menos exista alguna vacacion registrada.
                if (Dt_Vacaciones != null)
                {
                    Cls_Transporta_Datos_Vacaciones.P_Dt_Vacaciones = Dt_Vacaciones;
                    if (!string.IsNullOrEmpty(Datos.P_No_Vacacion))
                    {
                        if (Dt_Vacaciones.Rows.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(Dt_Vacaciones.Rows[0][OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Vacacion].ToString())) Cls_Transporta_Datos_Vacaciones.P_No_Vacacion = Dt_Vacaciones.Rows[0][OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Vacacion].ToString();
                            if (!string.IsNullOrEmpty(Dt_Vacaciones.Rows[0][OPE_NOM_VACACIONES_EMPLEADO.Campo_Empleado_ID].ToString())) Cls_Transporta_Datos_Vacaciones.P_Empleado_ID = Dt_Vacaciones.Rows[0][OPE_NOM_VACACIONES_EMPLEADO.Campo_Empleado_ID].ToString();
                            if (!string.IsNullOrEmpty(Dt_Vacaciones.Rows[0][OPE_NOM_VACACIONES_EMPLEADO.Campo_Dependencia_ID].ToString())) Cls_Transporta_Datos_Vacaciones.P_Dependencia_ID = Dt_Vacaciones.Rows[0][OPE_NOM_VACACIONES_EMPLEADO.Campo_Dependencia_ID].ToString();
                            if (!string.IsNullOrEmpty(Dt_Vacaciones.Rows[0][OPE_NOM_VACACIONES_EMPLEADO.Campo_Fecha_Inicio].ToString())) Cls_Transporta_Datos_Vacaciones.P_Fecha_Inicio = string.Format("{0:dd/MM/yyyy}", HttpUtility.HtmlDecode(Dt_Vacaciones.Rows[0][OPE_NOM_VACACIONES_EMPLEADO.Campo_Fecha_Inicio].ToString().Substring(0, 10)));
                            if (!string.IsNullOrEmpty(Dt_Vacaciones.Rows[0][OPE_NOM_VACACIONES_EMPLEADO.Campo_Fecha_Termino].ToString())) Cls_Transporta_Datos_Vacaciones.P_Fecha_Termino = string.Format("{0:dd/MM/yyyy}", HttpUtility.HtmlDecode(Dt_Vacaciones.Rows[0][OPE_NOM_VACACIONES_EMPLEADO.Campo_Fecha_Termino].ToString().Substring(0, 10)));
                            if (!string.IsNullOrEmpty(Dt_Vacaciones.Rows[0][OPE_NOM_VACACIONES_EMPLEADO.Campo_Fecha_Regreso_Laboral].ToString())) Cls_Transporta_Datos_Vacaciones.P_Fecha_Regreso_Laboral = string.Format("{0:dd/MM/yyyy}", HttpUtility.HtmlDecode(Dt_Vacaciones.Rows[0][OPE_NOM_VACACIONES_EMPLEADO.Campo_Fecha_Regreso_Laboral].ToString().Substring(0, 10)));
                            if (!string.IsNullOrEmpty(Dt_Vacaciones.Rows[0][OPE_NOM_VACACIONES_EMPLEADO.Campo_Cantidad_Dias].ToString())) Cls_Transporta_Datos_Vacaciones.P_Cantidad_Dias = Convert.ToInt32(Dt_Vacaciones.Rows[0][OPE_NOM_VACACIONES_EMPLEADO.Campo_Cantidad_Dias].ToString());
                            if (!string.IsNullOrEmpty(Dt_Vacaciones.Rows[0][OPE_NOM_VACACIONES_EMPLEADO.Campo_Estatus].ToString())) Cls_Transporta_Datos_Vacaciones.P_Estatus = Dt_Vacaciones.Rows[0][OPE_NOM_VACACIONES_EMPLEADO.Campo_Estatus].ToString();
                            if (!string.IsNullOrEmpty(Dt_Vacaciones.Rows[0][OPE_NOM_VACACIONES_EMPLEADO.Campo_Comentarios].ToString())) Cls_Transporta_Datos_Vacaciones.P_Comentarios = Dt_Vacaciones.Rows[0][OPE_NOM_VACACIONES_EMPLEADO.Campo_Comentarios].ToString();
                            if (!string.IsNullOrEmpty(Dt_Vacaciones.Rows[0][OPE_NOM_VACACIONES_EMPLEADO.Campo_Nomina_ID].ToString())) Cls_Transporta_Datos_Vacaciones.P_Nomina_ID = Dt_Vacaciones.Rows[0][OPE_NOM_VACACIONES_EMPLEADO.Campo_Nomina_ID].ToString();
                            if (!string.IsNullOrEmpty(Dt_Vacaciones.Rows[0][OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Nomina].ToString())) Cls_Transporta_Datos_Vacaciones.P_No_Nomina = Convert.ToInt32(Dt_Vacaciones.Rows[0][OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Nomina].ToString());
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Cls_Transporta_Datos_Vacaciones;
        }
        ///******************************************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Identificar_Detalle_Consecutivo
        /// DESCRIPCION : 
        /// 
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 23/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///******************************************************************************************************************************************************
        private static Int64 Consulta_Identificar_Detalle_Consecutivo()
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            OracleConnection Conexion = new OracleConnection();//Variable que almacena la conexión.
            OracleCommand Comando = new OracleCommand();//comando que se usara para realizar la consulta.
            OracleTransaction Transaccion;//Variable que manejara las transacciones de la consulta.
            OracleDataReader Lector = null;
            Int64 Identificador_Consecutivo = 0;

            Conexion.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;//Obtenemos la cadena d e conexión.
            Conexion.Open();//Abrimos la conexión.
            Transaccion = Conexion.BeginTransaction();//Obtenemos la instancia para el manejo de transacciones. 
            Comando.Connection = Conexion;//Asignamos la coneccion con la que trabajara, el comando.
            Comando.Transaction = Transaccion;//Asignamos la transaccion que manejara algun posible error cuando el comando ejecute la consulta.

            try
            {
                Mi_SQL.Append("select * from (select " + Ope_Nom_Vacaciones_Dia_Det.Tabla_Ope_Nom_Vacaciones_Dia_Det);
                Mi_SQL.Append(".* FROM " + Ope_Nom_Vacaciones_Dia_Det.Tabla_Ope_Nom_Vacaciones_Dia_Det);
                Mi_SQL.Append(" order by " + Ope_Nom_Vacaciones_Dia_Det.Campo_No_Dia_Vacacion);
                Mi_SQL.Append(" desc) where rownum =1");

                Comando.CommandText = Mi_SQL.ToString();
                Lector = Comando.ExecuteReader();
                Transaccion.Commit();

                if (Lector.Read())
                {
                    Identificador_Consecutivo = Lector.GetInt64(0);
                }

                if (Identificador_Consecutivo is Int64)
                {
                    if (Identificador_Consecutivo == 0)
                    {
                        Identificador_Consecutivo = 1;
                    }
                    else
                    {
                        ++Identificador_Consecutivo;
                    }
                }
                else
                {
                    Identificador_Consecutivo = 1;
                }
            }
            catch (OracleException Ex)
            {
                if (Transaccion != null)
                {
                    Transaccion.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Transaccion != null)
                {
                    Transaccion.Rollback();
                }
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Transaccion != null)
                {
                    Transaccion.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                Conexion.Close();
            }
            return Identificador_Consecutivo;
        }
        ///******************************************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Detalles_Vacaciones
        /// 
        /// DESCRIPCION : Consultalos de los detalles de las vacaciones del empleado. Estos detalles tendran la información de los estatus 
        ///               y el estado de las vacaciones.
        /// 
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 11/Febrero/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///******************************************************************************************************************************************************
        public static DataTable Consulta_Detalles_Vacaciones(Cls_Ope_Nom_Vacaciones_Empleado_Negocio Datos)
        {
            String Mi_SQL = "";//Variable que almacenara la consulta.
            DataTable Dt_Detalles_Vacaciones = null;//Estructura que almacenara la lista de detalles de las vacaciones.

            try
            {
                Mi_SQL = "SELECT " + Ope_Nom_Vacaciones_Dia_Det.Tabla_Ope_Nom_Vacaciones_Dia_Det + ".* FROM " +
                         Ope_Nom_Vacaciones_Dia_Det.Tabla_Ope_Nom_Vacaciones_Dia_Det+
                         " WHERE " + Ope_Nom_Vacaciones_Dia_Det.Campo_No_Vacacion + "='" + Datos.P_No_Vacacion + "'";

                Dt_Detalles_Vacaciones = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los detalles de las vacaciones. Error: [" + Ex.Message + "]");
            }
            return Dt_Detalles_Vacaciones;
        }
        ///******************************************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Cambiar_Estatus_Vacaciones
        /// 
        /// DESCRIPCION : Consultalos de los detalles de las vacaciones del empleado. Estos detalles tendran la información de los estatus 
        ///               y el estado de las vacaciones.
        /// 
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 11/Febrero/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///******************************************************************************************************************************************************
        public static Boolean Cambiar_Estatus_Vacaciones(Cls_Ope_Nom_Vacaciones_Empleado_Negocio Datos)
        {
            String Mi_SQL = "";//Variable que almacena la consulta.
            Boolean Operacion_Completa = false;//Guarda el estatus de la configuración.

            try
            {
                Mi_SQL = "UPDATE " + Ope_Nom_Vacaciones_Dia_Det.Tabla_Ope_Nom_Vacaciones_Dia_Det + " SET " +
                         Ope_Nom_Vacaciones_Dia_Det.Campo_Estado + "='Cancelado', " +
                         Ope_Nom_Vacaciones_Dia_Det.Campo_Estatus + "='Cancelado'" + 
                         " WHERE " + Ope_Nom_Vacaciones_Dia_Det.Campo_No_Dia_Vacacion + "='" + Datos.P_No_Dia_Vacacion + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                Mi_SQL = "UPDATE " + OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + " SET " +
                          OPE_NOM_VACACIONES_EMPLEADO.Campo_Estatus + "='Cancelado'" + 
                          " WHERE " + OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Vacacion + "='" + Datos.P_No_Vacacion + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al cambiar de estado de las vaciones del empleado. Error: [" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }
        ///******************************************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Dias_Comprometidos
        /// 
        /// DESCRIPCION : Consultamos los dias que el empleado tiene en un estado de COMPROMETIDO.
        /// 
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : Enero/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///******************************************************************************************************************************************************
        public static Int32 Consultar_Dias_Comprometidos(Cls_Ope_Nom_Vacaciones_Empleado_Negocio Datos)
        {
            StringBuilder Mi_SQl = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Vacaciones = null;//Variable que almacena los dias de vacaciones que ha tenido el empleado.
            Int32 Dias_Compremetidos = 0;//Variable que almacenara los dias de vacaciones que tiene el empleado en un estado de comprometido.

            try
            {
                Mi_SQl.Append("select ");
                Mi_SQl.Append(OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + ".* ");
                Mi_SQl.Append(" from ");
                Mi_SQl.Append(OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado);
                Mi_SQl.Append(" where ");
                Mi_SQl.Append(OPE_NOM_VACACIONES_EMPLEADO.Campo_Estatus + " <> 'Rechazado'");
                Mi_SQl.Append(" and ");
                Mi_SQl.Append(OPE_NOM_VACACIONES_EMPLEADO.Campo_Estatus + " <> 'Cancelado'");
                Mi_SQl.Append(" and ");
                Mi_SQl.Append(OPE_NOM_VACACIONES_EMPLEADO.Campo_Estado + "='COMPROMETIDA'");
                Mi_SQl.Append(" and ");
                Mi_SQl.Append(OPE_NOM_VACACIONES_EMPLEADO.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'");

                //Consultamos las vacaciones del empleado.
                Dt_Vacaciones = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQl.ToString()).Tables[0];
                
                //Recorremos el listado de vacaciones compretidas para obtener los dias a descontar al empleado a los dias disponibles.
                if (Dt_Vacaciones is DataTable)
                {
                    if (Dt_Vacaciones.Rows.Count > 0)
                    {
                        foreach (DataRow VACACION in Dt_Vacaciones.Rows)
                        {
                            if (VACACION is DataRow)
                            {
                                if (!String.IsNullOrEmpty(VACACION[OPE_NOM_VACACIONES_EMPLEADO.Campo_Cantidad_Dias].ToString()))
                                {
                                    Dias_Compremetidos += Convert.ToInt32(VACACION[OPE_NOM_VACACIONES_EMPLEADO.Campo_Cantidad_Dias].ToString().Trim());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al obtener los dias de vacaciones que el empleado tiene en un estado de COMPROMETIDOS. Error: [" + Ex.Message + "]");
            }
            return Dias_Compremetidos;
        }
        #endregion

        #region (Modulo de Dias Vacaciones Empleado)
        ///****************************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Detalle_Vacaciones_Empleados
        /// DESCRIPCION : Da de alta los detalles de los dias de vacaciones por periodo que tienen 
        ///               los empleados.
        /// 
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 1/Marzo/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///****************************************************************************************************************************************
        public static Boolean Alta_Detalle_Vacaciones_Empleados(Cls_Ope_Nom_Vacaciones_Empleado_Negocio Datos)
        {
            String Mi_SQL;                                                                      //Obtiene la cadena de inserción hacía la base de datos
            Object No_Vacacion_Detalle;                                                         //Obtiene el ID con la cual se guardo los datos en la base de datos
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;                                                  //Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        
            Boolean Operacion_Completa = false;                                                 //Variable que guarda el valor si la operacion se realiza con exito o no.


            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }

            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos

            try
            {
                Mi_SQL = "SELECT NVL(MAX(" + Ope_Nom_Vacaciones_Empleado_Detalles.Campo_No_Vacacion_Detalle + "),'0000000000') ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Nom_Vacaciones_Empleado_Detalles.Tabla_Ope_Nom_Vacaciones_Empl_Det;
                No_Vacacion_Detalle = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(No_Vacacion_Detalle))
                {
                    Datos.P_No_Vacacion_Detalle = "0000000001";
                }
                else
                {
                    Datos.P_No_Vacacion_Detalle = String.Format("{0:0000000000}", Convert.ToInt32(No_Vacacion_Detalle) + 1);
                }

                Mi_SQL = "INSERT INTO " + Ope_Nom_Vacaciones_Empleado_Detalles.Tabla_Ope_Nom_Vacaciones_Empl_Det + " (" +
                          Ope_Nom_Vacaciones_Empleado_Detalles.Campo_No_Vacacion_Detalle + ", " +
                          Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Empleado_ID + ", " +
                          Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Anio + ", " +
                          Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Dias_Disponibles + ", " +
                          Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Dias_Tomados + ", " +
                          Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Periodo_Vacacional + ", " +
                          Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Estatus + ", " +
                          Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Usuario_Creo + ", " +
                          Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Fecha_Creo + 
                          ") VALUES(" +
                          "'" + Datos.P_No_Vacacion_Detalle + "', " +
                          "'" + Datos.P_Empleado_ID + "', " +
                          Datos.P_Anio + ", " +
                          Datos.P_Dias_Disponibles + ", " +
                          Datos.P_Dias_Tomados + ", " +
                          Datos.P_Periodo_Vacacional + ", " +
                          "'" + Datos.P_Estatus_Detalle + "', " +
                          "'" + Datos.P_Usuario_Creo + "', SYSDATE)";

                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                Transaccion_SQL.Commit();
                Operacion_Completa = true;
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
            return Operacion_Completa;
        }
        ///****************************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Modificar_Detalle_Vacaciones_Empleados
        /// 
        /// DESCRIPCION : Modifica los dias disponibles y los dias tomados de vacaciones de acuerdo
        ///               a los dias de vacaciones tomados por el empleado.
        /// 
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 1/Marzo/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///****************************************************************************************************************************************
        public static Boolean Modificar_Detalle_Vacaciones_Empleados(Cls_Ope_Nom_Vacaciones_Empleado_Negocio Datos)
        {
            String Mi_SQL;                                                                      //Obtiene la cadena de inserción hacía la base de datos
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;                                                  //Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        
            Boolean Operacion_Completa = false;                                                 //Variable que guarda el valor si la operacion se realiza con exito o no.


            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }

            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos

            try
            {
                Mi_SQL = "UPDATE " + Ope_Nom_Vacaciones_Empleado_Detalles.Tabla_Ope_Nom_Vacaciones_Empl_Det +
                         " SET " +
                         Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Dias_Disponibles + "=" + Datos.P_Dias_Disponibles + ", " +
                         Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Dias_Tomados + "=" + Datos.P_Dias_Tomados + ", " +
                         Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Usuario_Modifico + "='" + Datos.P_Usuario_Modifico + "', " +
                         Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Fecha_Creo + "=SYSDATE" +
                         " WHERE " +
                         Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "' and " +
                         Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Anio + "=" + Datos.P_Anio + " and " +
                         Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Periodo_Vacacional + "=" + Datos.P_Periodo_Vacacional;

                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                Transaccion_SQL.Commit();
                Operacion_Completa = true;
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
            return Operacion_Completa;
        }
        ///****************************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Cantidad_Dias_Disponiubles_Por_Periodo_Empleado
        /// 
        /// DESCRIPCION : Consulta los dias de vacaciones que el empleado a tomado en el periodo consultado.
        /// 
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 1/Marzo/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///****************************************************************************************************************************************
        public static DataTable Consultar_Cantidad_Dias_Disponiubles_Por_Periodo_Empleado(Cls_Ope_Nom_Vacaciones_Empleado_Negocio Datos)
        {
            String Mi_SQL = "";                         //Variable que almacenara la consulta a ejecutar.
            DataTable Dt_Vacaciones_Empleados = null;   //Variable que almacenara una lista de las vacaciones que tiene registradas el empleado en el sistema.
            Int32 Periodo_Actual = 0;                   //Variable que almacenara el periodo actual vacacional.
            Int32 Periodo_Anterior = 0;                 //Variable que almacenara el periodo anterior vacacional.
            Int32 Periodo_Siguiente = 0;                //Variable que almacenara el siguiente periodo vacacional.
            Int32 Anio_Actual = 0;                      //Variable que almacenara el año actual del calendario de nomina.
            Int32 Anio_Temporal = 0;                    //Variable que almacenara el año en funcion del periodo actual. Podria ser un año  atras o  adelante.

            try
            {
                //Consultamos el periodo en el que nos encontramos actualmente según el calendario de nómina.
                Periodo_Actual = Obtener_Periodo_Vacacional();

                //Creamos la consulta para obtener los dias disponibles que tienen los empleados en un determinado periodo vacacional.
                Mi_SQL = " SELECT " + Ope_Nom_Vacaciones_Empleado_Detalles.Tabla_Ope_Nom_Vacaciones_Empl_Det + ".* " +
                         " FROM " + Ope_Nom_Vacaciones_Empleado_Detalles.Tabla_Ope_Nom_Vacaciones_Empl_Det +
                         " WHERE " + Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'" +
                         " AND " + Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Estatus + "='ACTIVO'";

                //Validamos en que periodo nos encontramos actualmente.
                if (Periodo_Actual == 1)
                {
                    //Si nos encontramos en el PERIODO I, entoncés, El periodo anterior sera 2 II del año anterior.
                    //y el periodo siguiente será el II del mismo año.
                    Periodo_Anterior = 2;
                    Periodo_Siguiente = 2;
                    Anio_Actual = Obtener_Anio_Calendario_Nomina();
                    Anio_Temporal = Anio_Actual - 1;

                    Mi_SQL = Mi_SQL + " AND " + Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Anio + "=" + Anio_Actual + 
                                    " AND " + Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Periodo_Vacacional + " IN (" + Periodo_Actual + ", " + Periodo_Siguiente + ")" +
                                    " UNION ALL " +
                                    " SELECT " + Ope_Nom_Vacaciones_Empleado_Detalles.Tabla_Ope_Nom_Vacaciones_Empl_Det + ".* " +
                                         " FROM " + Ope_Nom_Vacaciones_Empleado_Detalles.Tabla_Ope_Nom_Vacaciones_Empl_Det +
                                         " WHERE " + Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'" +
                                         " AND " + Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Estatus + "='ACTIVO'" +
                                         " AND " + Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Anio + "=" + Anio_Temporal + 
                                         " AND " + Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Periodo_Vacacional + " IN (" + Periodo_Anterior + ")";
                }
                else if(Periodo_Actual ==2){
                    //Si nos encontramos en el PERIODO II, entoncés, el periodo anterior sera el I del mismo año y el periodo anterior 
                    //será el I del siguiente ako.
                    Periodo_Anterior = 1;
                    Periodo_Siguiente = 1;
                    Anio_Actual = Obtener_Anio_Calendario_Nomina();
                    Anio_Temporal = Anio_Actual + 1;

                    Mi_SQL = Mi_SQL + " AND " + Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Anio + "=" + Anio_Actual +
                                    " AND " + Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Periodo_Vacacional + " IN (" + Periodo_Anterior + ", " + Periodo_Actual + ")" +
                                    " UNION ALL " +
                                    " (SELECT " + Ope_Nom_Vacaciones_Empleado_Detalles.Tabla_Ope_Nom_Vacaciones_Empl_Det + ".* " +
                                         " FROM " + Ope_Nom_Vacaciones_Empleado_Detalles.Tabla_Ope_Nom_Vacaciones_Empl_Det +
                                         " WHERE " + Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'" +
                                         " AND " + Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Estatus + "='ACTIVO'" +
                                         " AND " + Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Anio + "=" + Anio_Temporal + 
                                         " AND " + Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Periodo_Vacacional + " IN (" + Periodo_Siguiente + "))";
                }
                //Ejecutamos la consulta de los dias disponibles de los periodos vacacionales [Anterior - Actual - Siguiente].
                Dt_Vacaciones_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];                
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los dias que tiene disponibles para tomar vacaciones el empleado. Error: [" + Ex.Message + "]");
            }
            return Dt_Vacaciones_Empleados;
        }
        ///****************************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Detalles_Vacaciones_Empleados
        /// 
        /// DESCRIPCION : Consulta el detalle del vacacional actual.
        /// 
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 4/Marzo/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///****************************************************************************************************************************************
        public static DataTable Consultar_Vacaciones_Empl_Det(Cls_Ope_Nom_Vacaciones_Empleado_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable que alamacenara la consulta.
            DataTable Dt_Detalles_Vacaciones = null;    //Variable que almacenara la informacion de los detalles de las vacaciones que a tomado el empleado en el periodod actual.
            try
            {
                Mi_SQL.Append(" SELECT " + Ope_Nom_Vacaciones_Empleado_Detalles.Tabla_Ope_Nom_Vacaciones_Empl_Det + ".*");
                Mi_SQL.Append(" FROM " + Ope_Nom_Vacaciones_Empleado_Detalles.Tabla_Ope_Nom_Vacaciones_Empl_Det);
                Mi_SQL.Append(" WHERE " + Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Estatus + "='ACTIVO'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Anio + "=" + Datos.P_Anio);
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Periodo_Vacacional + " IN (" + Datos.P_Periodo_Vacacional + ")");

                Dt_Detalles_Vacaciones = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los detalles de las vacaciones de los empleados de acuerdo al periodo y al año. Error: [" + Ex.Message + "]");
            }
            return Dt_Detalles_Vacaciones;
        }
        ///****************************************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Vacaciones_Dia_Det_Pagados_Pendientes_Tomar
        /// 
        /// DESCRIPCION : Consulta los dias que el empleado tomo de vacaciones y se encuentran en un estatus de pagado y pendiente por tomar.
        /// 
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 7/Marzo/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///****************************************************************************************************************************************
        public static DataTable Consultar_Vacaciones_Dia_Det_Pagados_Pendientes_Tomar(Cls_Ope_Nom_Vacaciones_Empleado_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Vacaciones_Dia_Det = null;    //Variable que almacena los resultados de la búsqueda.   

            try
            {
                Mi_SQL.Append(" SELECT " + Ope_Nom_Vacaciones_Dia_Det.Tabla_Ope_Nom_Vacaciones_Dia_Det + ".*");
                Mi_SQL.Append(" FROM " + Ope_Nom_Vacaciones_Dia_Det.Tabla_Ope_Nom_Vacaciones_Dia_Det);
                Mi_SQL.Append(" WHERE " + Ope_Nom_Vacaciones_Dia_Det.Campo_Estatus + "='Pagado'");
                Mi_SQL.Append(" AND " + Ope_Nom_Vacaciones_Dia_Det.Campo_Estado + "='Pendiente'");

                if (!string.IsNullOrEmpty(Datos.P_Fecha_Inicio) && !string.IsNullOrEmpty(Datos.P_Fecha_Termino))
                {
                    Mi_SQL.Append(" AND " + Ope_Nom_Vacaciones_Dia_Det.Campo_Fecha + " BETWEEN TO_DATE('" + Datos.P_Fecha_Inicio + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')");
                    Mi_SQL.Append(" AND TO_DATE('" + Datos.P_Fecha_Termino + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS')");
                }

                Mi_SQL.Append(" AND " + Ope_Nom_Vacaciones_Dia_Det.Campo_No_Vacacion + " IN ");
                Mi_SQL.Append(" (SELECT " + OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Vacacion);
                Mi_SQL.Append(" FROM " + OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado);
                Mi_SQL.Append(" WHERE " + OPE_NOM_VACACIONES_EMPLEADO.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "')");

                Dt_Vacaciones_Dia_Det = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception)
            {
                throw new Exception("");
            }
            return Dt_Vacaciones_Dia_Det;
        }
        ///***********************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Periodo_Vacacional
        ///
        ///DESCRIPCIÓN: Obtiene periodo vacacional en el que nos encontramos actualmente.
        ///
        ///CREO: Juan Alberto Hernandez Negrete
        ///FECHA_CREO: 9/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///***********************************************************************************************************************************
        private static Int32 Obtener_Periodo_Vacacional()
        {
            Int32 Periodo_Vacacional = 0;           //Variable que almacenara el periodo vacacional actual.    
            Int32 Anio_Calendario_Nomina = 0;       //Variable que almacena el año del calendario de la nomina.
            DateTime Fecha_Actual = DateTime.Now;   //Variable que almacena la fecha actual.

            try
            {
                //Consulta el año actual del periodo nóminal.
                Anio_Calendario_Nomina = Obtener_Anio_Calendario_Nomina();

                if ((DateTime.Compare(Fecha_Actual, new DateTime(Anio_Calendario_Nomina, 1, 1)) >= 0) &&
                    (DateTime.Compare(Fecha_Actual, new DateTime(Anio_Calendario_Nomina, 6, 30)) <= 0))
                {
                    //PERIODO VACACIONAL I
                    Periodo_Vacacional = 1;
                }
                else if ((DateTime.Compare(Fecha_Actual, new DateTime(Anio_Calendario_Nomina, 7, 1)) >= 0) &&
                    (DateTime.Compare(Fecha_Actual, new DateTime(Anio_Calendario_Nomina, 12, 31)) <= 0))
                {
                    //PERIODO VACACIONAL II
                    Periodo_Vacacional = 2;
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al obtener el periodo vacacional. Error: [" + Ex.Message + "]");
            }
            return Periodo_Vacacional;
        }
        ///***********************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Anio_Calendario_Nomina
        ///
        ///DESCRIPCIÓN: Obtiene el año del calendario de nomina que se encuentra vigente actualmente. 
        ///
        ///CREO: Juan Alberto Hernandez Negrete
        ///FECHA_CREO: 9/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///***********************************************************************************************************************************
        private static Int32 Obtener_Anio_Calendario_Nomina()
        {
            Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nomina = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexión con la clase de negocios.
            DataTable Dt_Calendario_Nomina = null;                                                                      //Variable que guardara la información del calendario de nómina consultado.
            Int32 Anio_Calendario_Nomina = 0;                                                                          //Variable que almacena el año del calendario de nomina.         

            try
            {
                //Consultamos la el calendario de nómina que esta activo actualmente. 
                Dt_Calendario_Nomina = Obj_Calendario_Nomina.Consultar_Calendario_Nomina_Fecha_Actual();

                if (Dt_Calendario_Nomina is DataTable)
                {
                    foreach (DataRow Informacion_Calendario_Nomina in Dt_Calendario_Nomina.Rows)
                    {
                        if (Informacion_Calendario_Nomina is DataRow)
                        {
                            if (!String.IsNullOrEmpty(Informacion_Calendario_Nomina[Cat_Nom_Calendario_Nominas.Campo_Anio].ToString()))
                            {
                                Anio_Calendario_Nomina = Convert.ToInt32(Informacion_Calendario_Nomina[Cat_Nom_Calendario_Nominas.Campo_Anio].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar el año del calendario de nómina del año actual. Error: [" + Ex.Message + "]");
            }
            return Anio_Calendario_Nomina;
        }
        ///***********************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Dias_Vacaciones_Base_Formula
        ///
        ///DESCRIPCIÓN: Obtiene los dias de vacaciones que le corresponden al empleado en base a la antiguedad que tiene el empleado
        ///             en la empresa.
        ///             
        ///             FORMULA: 
        ///                     [365-366]   -->     20 Dias Vacaciones
        ///                     N Dias      -->     X  Dias Vacaciones
        ///
        ///CREO: Juan Alberto Hernandez Negrete
        ///FECHA_CREO: 9/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///***********************************************************************************************************************************
        private static Int32 Dias_Vacaciones_Base_Formula(DateTime Fecha_Inicio, DateTime Fecha_Fin)
        {
            Int32 Cantidad_Dias = 0;            //Cantidad de dias que le corresponden al empleado segun su fecha de ingreso a presidencia.
            Int32 DIAS_ANIO = 0;                //Variable que almacena los dias que tiene el año nominal.
            Int32 Dias = 0;                     //Cantidad de dias que lleva el empleado laborando en presidencia.
            Int32 Anio_Calendario_Nomina = 0;   //Variable que almacena el año del periodo nominal.

            try
            {
                //Obtenemos el año del calendario de la nómina.
                Anio_Calendario_Nomina = Obtener_Anio_Calendario_Nomina();
                //Obtenemos la cantidad de dias que tiene el mes de febrero y checamos si se trata de un año bisiesto o no.
                DIAS_ANIO = (DateTime.DaysInMonth(Anio_Calendario_Nomina, 2) == 28) ? 365 : 366;
                //Obtenemos la cantidad de dias que el empleado lleva laborando en presidencia.
                Dias = (Int32)(Cls_DateAndTime.DateDiff(DateInterval.Day, Fecha_Inicio, Fecha_Fin) + 1);
                //Cantidad de dias que le corresponden al empleado de vacaciones si es que el empleado no tiene un año completo en presidencia
                Cantidad_Dias = (Int32)((Dias * 20) / DIAS_ANIO);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al obtener el porcentaje [%] de los dias que le corresponden al" +
                                    "empleado en base a la fecha de ingreso que tiene. Error: [" + Ex.Message + "]");
            }
            return Cantidad_Dias;
        }
        ///***********************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Vacaciones_Autorizadas_Con_Dias_Pendientes_Tomar
        ///
        ///DESCRIPCIÓN: Consulta los dias de vacaciones que tiene el empelado y que se encuentran con un estatus de Autorizado
        ///             y pendientes por tomar.
        ///
        ///CREO: Juan Alberto Hernandez Negrete
        ///FECHA_CREO: 15/Abril/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///***********************************************************************************************************************************
        public static DataTable Consultar_Vacaciones_Autorizadas_Con_Dias_Pendientes_Tomar(Cls_Ope_Nom_Vacaciones_Empleado_Negocio Datos)
        {
            String Mi_SQL = "";//Variable que almacenara la consulta.
            DataTable Dt_Vacaciones = null;//Variable que almacenara los registros de vacaciones encontrados. 

            try
            {
                Mi_SQL = "SELECT " + Ope_Nom_Vacaciones_Dia_Det.Tabla_Ope_Nom_Vacaciones_Dia_Det + ".*" +
                         " FROM " + Ope_Nom_Vacaciones_Dia_Det.Tabla_Ope_Nom_Vacaciones_Dia_Det +
                         " WHERE " + Ope_Nom_Vacaciones_Dia_Det.Campo_Estatus + "='Pendiente'" +
                         " AND " + Ope_Nom_Vacaciones_Dia_Det.Campo_Estado + "='Pendiente'" +
                         " AND " + Ope_Nom_Vacaciones_Dia_Det.Campo_No_Vacacion + " IN " +
                         " (SELECT " + OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Vacacion +
                         " FROM " + OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado +
                         " WHERE " + OPE_NOM_VACACIONES_EMPLEADO.Campo_Estatus + "='Autorizado'" +
                         " AND " + OPE_NOM_VACACIONES_EMPLEADO.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "')";

                Dt_Vacaciones = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Erroa al consultar las solicitudes de vacaciones autorizadas con dias pendientes por tomar. Error: [" + Ex.Message + "]");
            }
            return Dt_Vacaciones;
        }


        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Reporte_Vacaciones
        /// DESCRIPCION : Consulta los detalles referentes a las vacaciones
        /// PARAMETROS  : Datos: Contiene los datos que serán Eliminados en la base de datos
        /// CREO        : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 02/Abril/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Reporte_Vacaciones(Cls_Ope_Nom_Vacaciones_Empleado_Negocio Datos)
        {
            DataTable Dt_Consulta = new DataTable();
            StringBuilder Mi_SQL = new StringBuilder();
            try
            {
                Mi_SQL.Append("Select ");
                Mi_SQL.Append("(cast(" + OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + "." + OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Vacacion + " as decimal(10,5) )) as NUMERO_TIEMPO_EXTRA, ");

                Mi_SQL.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno);
                Mi_SQL.Append(" || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno);
                Mi_SQL.Append(" || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") as Nombre_Empleado,");

                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ");
                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + "." + OPE_NOM_VACACIONES_EMPLEADO.Campo_Estatus + ", ");
                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + "." + OPE_NOM_VACACIONES_EMPLEADO.Campo_Comentarios + " as Comentario, ");
                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + "." + OPE_NOM_VACACIONES_EMPLEADO.Campo_Estatus + " as Estatus_Detalle, ");
                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + "." + OPE_NOM_VACACIONES_EMPLEADO.Campo_Comentarios_Estatus + " as Comentario_Detalle ");

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado);

                
                Mi_SQL.Append(" left outer join " + Cat_Empleados.Tabla_Cat_Empleados + " on ");
                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + "." + OPE_NOM_VACACIONES_EMPLEADO.Campo_Empleado_ID + "=");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);

                Mi_SQL.Append(" WHERE ");

                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + "." + OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Vacacion);
                Mi_SQL.Append("='" + Datos.P_No_Vacacion + "' ");

                Mi_SQL.Append(" ORDER BY Nombre_Empleado ");

                Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                return Dt_Consulta;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Eliminar el tiempo extra. Error:[" + Ex.Message + "]");
            }
        }

        #endregion

        #endregion
    }
}