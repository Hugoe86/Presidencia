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
using Presidencia.Tipos_Nominas.Negocios;
using Presidencia.Empleados.Negocios;

namespace Presidencia.Tipos_Nominas.Datos
{
    public class Cls_Cat_Tipos_Nominas_Datos
    {
        #region (Metodos Operacion)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Tipo_Nomina
        /// DESCRIPCION : 1.Consulta el último ID dado de alta para poder ingresar el siguiente
        ///               2. Da de Alta el Tipo de Nómins en la BD con los datos proporcionados
        ///                  por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 05-Noviembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Alta_Tipo_Nomina(Cls_Cat_Tipos_Nominas_Negocio Datos)
        {
            String Mi_SQL;           //Obtiene la cadena de inserción hacía la base de datos
            Object Tipo_Nomina_ID;   //Obtiene el ID con la cual se guardo los datos en la base de datos
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
                Mi_SQL = "SELECT NVL(MAX(" + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + "),'00000') ";
                Mi_SQL = Mi_SQL + "FROM " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas;
                Tipo_Nomina_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(Tipo_Nomina_ID))
                {
                    Datos.P_Tipo_Nomina_ID = "00001";
                }
                else
                {
                    Datos.P_Tipo_Nomina_ID = String.Format("{0:00000}", Convert.ToInt32(Tipo_Nomina_ID) + 1);
                }
                //Consulta para la inserción del Tipo de Nómina con los datos proporcionados por el usuario
                Mi_SQL = "INSERT INTO " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " (";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Nominas.Campo_Nomina + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_1 + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_2 + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Nominas.Campo_Dias_Aguinaldo + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Nominas.Campo_Dias_Exenta_Prima_Vacacional + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Nominas.Campo_Dias_Exenta_Aguinaldo + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Nominas.Campo_Despensa + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Nominas.Campo_Comentarios + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Nominas.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Nominas.Campo_Fecha_Creo + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Nominas.Campo_Aplica_ISR + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Nominas.Campo_Actualizar_Salario + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Antiguedad + ") VALUES ('";
                Mi_SQL = Mi_SQL + Datos.P_Tipo_Nomina_ID + "', '" + Datos.P_Nomina + "', ";
                Mi_SQL = Mi_SQL + Datos.P_Dias_Prima_Vacacional_1 + ", ";
                Mi_SQL = Mi_SQL + Datos.P_Dias_Prima_Vacacional_2 + ", ";
                Mi_SQL = Mi_SQL + Datos.P_Dias_Aguinaldo + ", ";
                Mi_SQL = Mi_SQL + Datos.P_Dias_Exenta_Prima_Vacacional + ", ";
                Mi_SQL = Mi_SQL + Datos.P_Dias_Exenta_Aguinaldo + ", ";
                Mi_SQL = Mi_SQL + Datos.P_Despensa + ", '";
                Mi_SQL = Mi_SQL + Datos.P_Comentarios + "', '" + Datos.P_Nombre_Usuario + "', SYSDATE, '" +
                    Datos.P_Aplica_ISR + "', '" + Datos.P_Actualizar_Salario + "', " + Datos.P_Dias_Prima_Antiguedad + ")";

                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                ///*********************************    PERCEPCIONES    ************************************************
                //Inserta las percepciones que le corresponden al empleado.
                if (Datos.P_Percepciones_Nomina is DataTable)
                {
                    if (Datos.P_Percepciones_Nomina.Rows.Count > 0)
                    {
                        //Agrega todas las Percepciones que fueron asignadas a la nomina que se quiere dar de alta en la base de datos
                        foreach (DataRow Registro_Percepcion in Datos.P_Percepciones_Nomina.Rows)
                        {
                            if (Registro_Percepcion is DataRow)
                            {
                                Mi_SQL = "INSERT INTO " + Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Tip_Nom_Per_Ded_Det + " (" +
                                Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Tipo_Nomina_ID + ", " +
                                Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID + ", " +
                                Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad + ", " +
                                Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Aplica_Todos + ") " +
                                "VALUES(" +
                                "'" + Datos.P_Tipo_Nomina_ID + "', " +
                                "'" + Registro_Percepcion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID].ToString() + "', " +
                                "" + Convert.ToDouble(Registro_Percepcion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad].ToString()) + ", " +
                                "'" + Registro_Percepcion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Aplica_Todos].ToString() + "')";

                                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                                if (Registro_Percepcion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Aplica_Todos].ToString().Equals("SI"))
                                {
                                    Aplicar_Concepto_Empleados_Tipo_Nomina(Datos.P_Tipo_Nomina_ID, Registro_Percepcion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID].ToString(),
                                        Convert.ToDouble(Registro_Percepcion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad].ToString()));
                                }
                            }
                        }
                    }
                }


                ///*******************************  DEDUCCIONES *****************************************************
                if (Datos.P_Deducciones_Nomina is DataTable)
                {
                    if (Datos.P_Deducciones_Nomina.Rows.Count > 0){
                    //Agrega todas las Percepciones que fueron asignadas a la nomina que se quiere dar de alta en la base de datos
                        foreach (DataRow Registro_Deduccion in Datos.P_Deducciones_Nomina.Rows)
                        {
                            if (Registro_Deduccion is DataRow)
                            {
                                Mi_SQL = "INSERT INTO " + Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Tip_Nom_Per_Ded_Det + " (" +
                                Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Tipo_Nomina_ID + ", " +
                                Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID + ", " +
                                Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad +  ", " +
                                Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Aplica_Todos + ") " +
                                "VALUES(" +
                                "'" + Datos.P_Tipo_Nomina_ID + "', " +
                                "'" + Registro_Deduccion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID].ToString() + "', " +
                                "" + Convert.ToDouble(Registro_Deduccion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad].ToString()) + ", " +
                                "'" + Registro_Deduccion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Aplica_Todos].ToString() + "')";

                                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                                if (Registro_Deduccion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Aplica_Todos].ToString().Equals("SI"))
                                {
                                    Aplicar_Concepto_Empleados_Tipo_Nomina(Datos.P_Tipo_Nomina_ID, Registro_Deduccion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID].ToString(),
                                        Convert.ToDouble(Registro_Deduccion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad].ToString()));
                                }
                            }
                        }
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
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Modificar_Tipo_Nomina
        /// DESCRIPCION : Modifica los datos del Tipo de Nomina con lo que fueron introducidos 
        ///               por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán modificados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 05-Noviembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Modificar_Tipo_Nomina(Cls_Cat_Tipos_Nominas_Negocio Datos)
        {
            DataTable Dt_Percepciones_Tipo_Nomina = null;
            DataTable Dt_Deducciones_Tipo_Nomina = null;
            Cls_Cat_Tipos_Nominas_Negocio Obj_Tipos_Nomina = new Cls_Cat_Tipos_Nominas_Negocio();

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
                #region (Actualizar Informacion del Tipo Nomina)
                //Consulta para la modificación del Tipo de Nómina con los datos proporcionados por el usuario
                Mi_SQL = "UPDATE " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " SET ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Nominas.Campo_Nomina + " = '" + Datos.P_Nomina + "', ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_1+ " = " + Datos.P_Dias_Prima_Vacacional_1 + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_2 + " = " + Datos.P_Dias_Prima_Vacacional_2 + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Nominas.Campo_Dias_Aguinaldo + " = " + Datos.P_Dias_Aguinaldo + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Nominas.Campo_Dias_Exenta_Prima_Vacacional + " = " + Datos.P_Dias_Exenta_Prima_Vacacional + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Nominas.Campo_Dias_Exenta_Aguinaldo + " = " + Datos.P_Dias_Exenta_Aguinaldo + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Nominas.Campo_Despensa + " = " + Datos.P_Despensa + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Nominas.Campo_Comentarios + " = '" + Datos.P_Comentarios + "', ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Nominas.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Nominas.Campo_Aplica_ISR + "='" + Datos.P_Aplica_ISR + "', ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Nominas.Campo_Actualizar_Salario + "='" + Datos.P_Actualizar_Salario + "', ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Antiguedad + "=" + Datos.P_Dias_Prima_Antiguedad + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Nominas.Campo_Fecha_Modifico + " = SYSDATE WHERE ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + " = '" + Datos.P_Tipo_Nomina_ID + "'";
                
                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                #endregion

                #region (Desaplicar Conceptos a los Empleados)
                Obj_Tipos_Nomina.P_Tipo_Nomina_ID = Datos.P_Tipo_Nomina_ID;
                Obj_Tipos_Nomina.P_Tipo = "PERCEPCION";
                Dt_Percepciones_Tipo_Nomina = Obj_Tipos_Nomina.Consulta_Percepciones_Deducciones_Nomina();

                Dt_Percepciones_Tipo_Nomina = Comparar_Tablas_Obtener_Perc_Deduc_Eliminar(Dt_Percepciones_Tipo_Nomina, Datos.P_Percepciones_Nomina);

                ///*********************************   DESAPLICAR PERCEPCIONES EMPLEADOS   ************************************************
                //Inserta las percepciones que le corresponden al empleado.
                if (Dt_Percepciones_Tipo_Nomina is DataTable)
                {
                    if (Dt_Percepciones_Tipo_Nomina.Rows.Count > 0)
                    {
                        //Agrega todas las Percepciones que fueron asignadas a la nomina que se quiere dar de alta en la base de datos
                        foreach (DataRow Registro_Percepcion in Dt_Percepciones_Tipo_Nomina.Rows)
                        {
                            if (Registro_Percepcion is DataRow)
                            {
                                if (!string.IsNullOrEmpty(Registro_Percepcion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID].ToString()))
                                {
                                    Desaplicar_Conceptos_Empleados_Tipo_Nomina_Eliminada(Datos.P_Tipo_Nomina_ID,
                                        Registro_Percepcion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID].ToString());
                                }
                            }
                        }
                    }
                }

                Obj_Tipos_Nomina.P_Tipo_Nomina_ID = Datos.P_Tipo_Nomina_ID;
                Obj_Tipos_Nomina.P_Tipo = "DEDUCCION";
                Dt_Deducciones_Tipo_Nomina = Obj_Tipos_Nomina.Consulta_Percepciones_Deducciones_Nomina();

                Dt_Deducciones_Tipo_Nomina = Comparar_Tablas_Obtener_Perc_Deduc_Eliminar(Dt_Deducciones_Tipo_Nomina, Datos.P_Deducciones_Nomina);

                ///*******************************  DEDUCCIONES *****************************************************
                if (Dt_Deducciones_Tipo_Nomina is DataTable)
                {
                    if (Dt_Deducciones_Tipo_Nomina.Rows.Count > 0)
                    {
                        //Agrega todas las Percepciones que fueron asignadas a la nomina que se quiere dar de alta en la base de datos
                        foreach (DataRow Registro_Deduccion in Dt_Deducciones_Tipo_Nomina.Rows)
                        {
                            if (Registro_Deduccion is DataRow)
                            {
                                if (!string.IsNullOrEmpty(Registro_Deduccion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID].ToString()))
                                {
                                    Desaplicar_Conceptos_Empleados_Tipo_Nomina_Eliminada(Datos.P_Tipo_Nomina_ID,
                                        Registro_Deduccion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID].ToString());
                                }
                            }
                        }
                    }
                }
                #endregion

                #region(Aplicar Conceptos Nuevos Empleados)
                Obj_Tipos_Nomina.P_Tipo_Nomina_ID = Datos.P_Tipo_Nomina_ID;
                Obj_Tipos_Nomina.P_Tipo = "PERCEPCION";
                Dt_Percepciones_Tipo_Nomina = Obj_Tipos_Nomina.Consulta_Percepciones_Deducciones_Nomina();

                Dt_Percepciones_Tipo_Nomina = Comparar_Tablas_Obtener_Perc_Deduc_Nuevas(Datos.P_Percepciones_Nomina, Dt_Percepciones_Tipo_Nomina);

                ///*********************************   DESAPLICAR PERCEPCIONES EMPLEADOS   ************************************************
                //Inserta las percepciones que le corresponden al empleado.
                if (Dt_Percepciones_Tipo_Nomina is DataTable)
                {
                    if (Dt_Percepciones_Tipo_Nomina.Rows.Count > 0)
                    {
                        //Agrega todas las Percepciones que fueron asignadas a la nomina que se quiere dar de alta en la base de datos
                        foreach (DataRow Registro_Percepcion in Dt_Percepciones_Tipo_Nomina.Rows)
                        {
                            if (Registro_Percepcion is DataRow)
                            {
                                if (Registro_Percepcion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Aplica_Todos].ToString().Equals("SI"))
                                {
                                    Aplicar_Concepto_Empleados_Tipo_Nomina(Datos.P_Tipo_Nomina_ID, Registro_Percepcion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID].ToString(),
                                        Convert.ToDouble(Registro_Percepcion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad].ToString()));
                                }
                            }
                        }
                    }
                }

                Obj_Tipos_Nomina.P_Tipo_Nomina_ID = Datos.P_Tipo_Nomina_ID;
                Obj_Tipos_Nomina.P_Tipo = "DEDUCCION";
                Dt_Deducciones_Tipo_Nomina = Obj_Tipos_Nomina.Consulta_Percepciones_Deducciones_Nomina();

                Dt_Deducciones_Tipo_Nomina = Comparar_Tablas_Obtener_Perc_Deduc_Nuevas(Datos.P_Deducciones_Nomina, Dt_Deducciones_Tipo_Nomina);

                ///*******************************  DEDUCCIONES *****************************************************
                if (Dt_Deducciones_Tipo_Nomina is DataTable)
                {
                    if (Dt_Deducciones_Tipo_Nomina.Rows.Count > 0)
                    {
                        //Agrega todas las Percepciones que fueron asignadas a la nomina que se quiere dar de alta en la base de datos
                        foreach (DataRow Registro_Deduccion in Dt_Deducciones_Tipo_Nomina.Rows)
                        {
                            if (Registro_Deduccion is DataRow)
                            {
                                if (Registro_Deduccion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Aplica_Todos].ToString().Equals("SI"))
                                {
                                    Aplicar_Concepto_Empleados_Tipo_Nomina(Datos.P_Tipo_Nomina_ID, Registro_Deduccion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID].ToString(),
                                        Convert.ToDouble(Registro_Deduccion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad].ToString()));
                                }
                            }
                        }
                    }
                }
                #endregion

                #region(Actualizar Cantidades Conceptos Empleados)
                Obj_Tipos_Nomina.P_Tipo_Nomina_ID = Datos.P_Tipo_Nomina_ID;
                Obj_Tipos_Nomina.P_Tipo = "PERCEPCION";
                Dt_Percepciones_Tipo_Nomina = Obj_Tipos_Nomina.Consulta_Percepciones_Deducciones_Nomina();

                Dt_Percepciones_Tipo_Nomina = Comparar_Tablas_Obtener_Perc_Deduc_Cantidades_Cambiaron(Dt_Percepciones_Tipo_Nomina, Datos.P_Percepciones_Nomina);

                ///*********************************   DESAPLICAR PERCEPCIONES EMPLEADOS   ************************************************
                //Inserta las percepciones que le corresponden al empleado.
                if (Dt_Percepciones_Tipo_Nomina is DataTable)
                {
                    if (Dt_Percepciones_Tipo_Nomina.Rows.Count > 0)
                    {
                        //Agrega todas las Percepciones que fueron asignadas a la nomina que se quiere dar de alta en la base de datos
                        foreach (DataRow Registro_Percepcion in Dt_Percepciones_Tipo_Nomina.Rows)
                        {
                            if (Registro_Percepcion is DataRow)
                            {
                                if (Registro_Percepcion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Aplica_Todos].ToString().Equals("SI"))
                                {
                                    Actualizar_Conceptos_Empleados_Tipo_Nomina_Modificar_Cantidades(Datos.P_Tipo_Nomina_ID, Registro_Percepcion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID].ToString(),
                                        Convert.ToDouble(Registro_Percepcion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad].ToString()));
                                }
                                else {
                                    Desaplicar_Conceptos_Empleados_Tipo_Nomina_Eliminada(Datos.P_Tipo_Nomina_ID,
                                        Registro_Percepcion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID].ToString());
                                }
                            }
                        }
                    }
                }

                Obj_Tipos_Nomina.P_Tipo_Nomina_ID = Datos.P_Tipo_Nomina_ID;
                Obj_Tipos_Nomina.P_Tipo = "DEDUCCION";
                Dt_Deducciones_Tipo_Nomina = Obj_Tipos_Nomina.Consulta_Percepciones_Deducciones_Nomina();

                Dt_Deducciones_Tipo_Nomina = Comparar_Tablas_Obtener_Perc_Deduc_Cantidades_Cambiaron(Dt_Deducciones_Tipo_Nomina, Datos.P_Deducciones_Nomina);

                ///*******************************  DEDUCCIONES *****************************************************
                if (Dt_Deducciones_Tipo_Nomina is DataTable)
                {
                    if (Dt_Deducciones_Tipo_Nomina.Rows.Count > 0)
                    {
                        //Agrega todas las Percepciones que fueron asignadas a la nomina que se quiere dar de alta en la base de datos
                        foreach (DataRow Registro_Deduccion in Dt_Deducciones_Tipo_Nomina.Rows)
                        {
                            if (Registro_Deduccion is DataRow)
                            {
                                if (Registro_Deduccion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Aplica_Todos].ToString().Equals("SI"))
                                {
                                    Actualizar_Conceptos_Empleados_Tipo_Nomina_Modificar_Cantidades(Datos.P_Tipo_Nomina_ID, Registro_Deduccion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID].ToString(),
                                        Convert.ToDouble(Registro_Deduccion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad].ToString()));
                                }
                                else {
                                    Desaplicar_Conceptos_Empleados_Tipo_Nomina_Eliminada(Datos.P_Tipo_Nomina_ID,
                                        Registro_Deduccion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID].ToString());
                                }
                            }
                        }
                    }
                }
                #endregion

                #region(Actualizar Cantidades Conceptos Empleados)
                Obj_Tipos_Nomina.P_Tipo_Nomina_ID = Datos.P_Tipo_Nomina_ID;
                Obj_Tipos_Nomina.P_Tipo = "PERCEPCION";
                Dt_Percepciones_Tipo_Nomina = Obj_Tipos_Nomina.Consulta_Percepciones_Deducciones_Nomina();

                Dt_Percepciones_Tipo_Nomina = Comparar_Tablas_Obtener_Perc_Deduc_Aplica(Dt_Percepciones_Tipo_Nomina, Datos.P_Percepciones_Nomina);

                ///*********************************   DESAPLICAR PERCEPCIONES EMPLEADOS   ************************************************
                //Inserta las percepciones que le corresponden al empleado.
                if (Dt_Percepciones_Tipo_Nomina is DataTable)
                {
                    if (Dt_Percepciones_Tipo_Nomina.Rows.Count > 0)
                    {
                        //Agrega todas las Percepciones que fueron asignadas a la nomina que se quiere dar de alta en la base de datos
                        foreach (DataRow Registro_Percepcion in Dt_Percepciones_Tipo_Nomina.Rows)
                        {
                            if (Registro_Percepcion is DataRow)
                            {
                                if (Registro_Percepcion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Aplica_Todos].ToString().Equals("SI"))
                                {
                                    Aplicar_Concepto_Empleados_Tipo_Nomina(Datos.P_Tipo_Nomina_ID, Registro_Percepcion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID].ToString(),
                                        Convert.ToDouble(Registro_Percepcion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad].ToString()));
                                }
                                else
                                {
                                    Desaplicar_Conceptos_Empleados_Tipo_Nomina_Eliminada(Datos.P_Tipo_Nomina_ID,
                                        Registro_Percepcion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID].ToString());
                                }
                            }
                        }
                    }
                }

                Obj_Tipos_Nomina.P_Tipo_Nomina_ID = Datos.P_Tipo_Nomina_ID;
                Obj_Tipos_Nomina.P_Tipo = "DEDUCCION";
                Dt_Deducciones_Tipo_Nomina = Obj_Tipos_Nomina.Consulta_Percepciones_Deducciones_Nomina();

                Dt_Deducciones_Tipo_Nomina = Comparar_Tablas_Obtener_Perc_Deduc_Aplica(Dt_Deducciones_Tipo_Nomina, Datos.P_Deducciones_Nomina);

                ///*******************************  DEDUCCIONES *****************************************************
                if (Dt_Deducciones_Tipo_Nomina is DataTable)
                {
                    if (Dt_Deducciones_Tipo_Nomina.Rows.Count > 0)
                    {
                        //Agrega todas las Percepciones que fueron asignadas a la nomina que se quiere dar de alta en la base de datos
                        foreach (DataRow Registro_Deduccion in Dt_Deducciones_Tipo_Nomina.Rows)
                        {
                            if (Registro_Deduccion is DataRow)
                            {
                                if (Registro_Deduccion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Aplica_Todos].ToString().Equals("SI"))
                                {
                                    Aplicar_Concepto_Empleados_Tipo_Nomina(Datos.P_Tipo_Nomina_ID, Registro_Deduccion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID].ToString(),
                                        Convert.ToDouble(Registro_Deduccion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad].ToString()));
                                }
                                else
                                {
                                    Desaplicar_Conceptos_Empleados_Tipo_Nomina_Eliminada(Datos.P_Tipo_Nomina_ID,
                                        Registro_Deduccion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID].ToString());
                                }
                            }
                        }
                    }
                }
                #endregion

                #region (Eliminar Conceptos del Tipo de Nomina)
                //Elimina las percepciones y deducciones que tiene asignado la nómina para poder agregar
                //las nuevas percepciones y deducciones seleccionadas por el usuario
                Mi_SQL = "DELETE FROM " + Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Tip_Nom_Per_Ded_Det;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + " = '" + Datos.P_Tipo_Nomina_ID + "'";
                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                #endregion

                #region (Alta Conceptos al Tipo de Nomina)
                ///*********************************    PERCEPCIONES    ************************************************
                //Inserta las percepciones que le corresponden al empleado.
                if (Datos.P_Percepciones_Nomina is DataTable)
                {
                    if (Datos.P_Percepciones_Nomina.Rows.Count > 0)
                    {
                        //Agrega todas las Percepciones que fueron asignadas a la nomina que se quiere dar de alta en la base de datos
                        foreach (DataRow Registro_Percepcion in Datos.P_Percepciones_Nomina.Rows)
                        {
                            if (Registro_Percepcion is DataRow)
                            {
                                Mi_SQL = "INSERT INTO " + Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Tip_Nom_Per_Ded_Det + " (" +
                                Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Tipo_Nomina_ID + ", " +
                                Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID + ", " +
                                Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad + ", " +
                                Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Aplica_Todos + ") " +
                                "VALUES(" +
                                "'" + Datos.P_Tipo_Nomina_ID + "', " +
                                "'" + Registro_Percepcion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID].ToString() + "', " +
                                "" + Convert.ToDouble(Registro_Percepcion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad].ToString()) + ", " +
                                "'" + Registro_Percepcion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Aplica_Todos].ToString() + "')";

                                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                            }
                        }
                    }
                }

                ///*******************************  DEDUCCIONES *****************************************************
                if (Datos.P_Deducciones_Nomina is DataTable)
                {
                    if (Datos.P_Deducciones_Nomina.Rows.Count > 0)
                    {
                        //Agrega todas las Percepciones que fueron asignadas a la nomina que se quiere dar de alta en la base de datos
                        foreach (DataRow Registro_Deduccion in Datos.P_Deducciones_Nomina.Rows)
                        {
                            if (Registro_Deduccion is DataRow)
                            {
                                Mi_SQL = "INSERT INTO " + Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Tip_Nom_Per_Ded_Det + " (" +
                                Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Tipo_Nomina_ID + ", " +
                                Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID + ", " +
                                Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad + ", " +
                                Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Aplica_Todos + ") " +
                                "VALUES(" +
                                "'" + Datos.P_Tipo_Nomina_ID + "', " +
                                "'" + Registro_Deduccion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID].ToString() + "', " +
                                "" + Convert.ToDouble(Registro_Deduccion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad].ToString()) + ", " +
                                "'" + Registro_Deduccion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Aplica_Todos].ToString() + "')";

                                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                            }
                        }
                    }
                }
                #endregion

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
        /// NOMBRE DE LA FUNCION: Eliminar_Tipo_Nomina
        /// DESCRIPCION : Elimina el Tipo de Nómina que fue seleccionada por el usuario de la BD
        /// PARAMETROS  : Datos: Obtiene que Tipo de Nómina desea eliminar de la BD
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 05-Noviembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Eliminar_Tipo_Nomina(Cls_Cat_Tipos_Nominas_Negocio Datos)
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
                //Elimina las percepciones y deducciones que tiene asignado la nómina
                Mi_SQL = "DELETE FROM " + Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Tip_Nom_Per_Ded_Det;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + " = '" + Datos.P_Tipo_Nomina_ID + "'";
                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                Mi_SQL = "DELETE FROM " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " WHERE ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + " = '" + Datos.P_Tipo_Nomina_ID + "'";
                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos


                ///*********************************    PERCEPCIONES    ************************************************
                //Inserta las percepciones que le corresponden al empleado.
                if (Datos.P_Percepciones_Nomina is DataTable)
                {
                    if (Datos.P_Percepciones_Nomina.Rows.Count > 0)
                    {
                        //Agrega todas las Percepciones que fueron asignadas a la nomina que se quiere dar de alta en la base de datos
                        foreach (DataRow Registro_Percepcion in Datos.P_Percepciones_Nomina.Rows)
                        {
                            if (Registro_Percepcion is DataRow)
                            {
                                if (!string.IsNullOrEmpty(Registro_Percepcion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID].ToString()))
                                {
                                    Desaplicar_Conceptos_Empleados_Tipo_Nomina_Eliminada(Datos.P_Tipo_Nomina_ID, Registro_Percepcion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID].ToString());
                                }
                            }
                        }
                    }
                }

                ///*******************************  DEDUCCIONES *****************************************************
                if (Datos.P_Deducciones_Nomina is DataTable)
                {
                    if (Datos.P_Deducciones_Nomina.Rows.Count > 0)
                    {
                        //Agrega todas las Percepciones que fueron asignadas a la nomina que se quiere dar de alta en la base de datos
                        foreach (DataRow Registro_Deduccion in Datos.P_Deducciones_Nomina.Rows)
                        {
                            if (Registro_Deduccion is DataRow)
                            {
                                if (!string.IsNullOrEmpty(Registro_Deduccion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID].ToString()))
                                {
                                    Desaplicar_Conceptos_Empleados_Tipo_Nomina_Eliminada(Datos.P_Tipo_Nomina_ID, Registro_Deduccion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID].ToString());
                                }
                            }
                        }
                    }
                }

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
        #endregion

        #region (Metodos Consulta)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Datos_Tipo_Nomina
        /// DESCRIPCION : Consulta todos los datos del Tipo de Nómina que estan dadas de alta en
        ///               la BD con todos sus datos
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 05-Noviembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Datos_Tipo_Nomina(Cls_Cat_Tipos_Nominas_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de los Tipos de Nóminas

            try
            {
                //Consulta todos los datos del Tipo de Nómina que se fue seleccionado por el usuario
                Mi_SQL = "SELECT * FROM " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas;

                if (Datos.P_Tipo_Nomina_ID != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + " = '" + Datos.P_Tipo_Nomina_ID + "'";
                }
                if (Datos.P_Nomina != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE UPPER(" + Cat_Nom_Tipos_Nominas.Campo_Nomina + ") LIKE UPPER('%" + Datos.P_Nomina + "%')";
                }

                if (!string.IsNullOrEmpty(Datos.P_Actualizar_Salario))
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Nom_Tipos_Nominas.Campo_Actualizar_Salario + "='" + Datos.P_Actualizar_Salario + "'";
                }

                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Nom_Tipos_Nominas.Campo_Nomina;
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
        /// NOMBRE DE LA FUNCION: Consulta_Tipos_Nominas
        /// DESCRIPCION : Consulta los Tipos de Nómina que estan dados de alta en la BD 
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 05-Noviembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Tipos_Nominas(Cls_Cat_Tipos_Nominas_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de los Tipos de Nómminas

            try
            {
                //Consulta los Tipos de Nómina que estan dados de alta en la base de datos
                Mi_SQL = "SELECT " + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + ", " + Cat_Nom_Tipos_Nominas.Campo_Nomina;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas;
                if (Datos.P_Tipo_Nomina_ID != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + " = '" + Datos.P_Tipo_Nomina_ID + "'";
                }
                if (Datos.P_Nomina != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE UPPER(" + Cat_Nom_Tipos_Nominas.Campo_Nomina + ") LIKE UPPER('%" + Datos.P_Nomina + "%')";
                }

                if (!string.IsNullOrEmpty(Datos.P_Actualizar_Salario))
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Nom_Tipos_Nominas.Campo_Actualizar_Salario + "='" + Datos.P_Actualizar_Salario + "'";
                }

                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Nom_Tipos_Nominas.Campo_Nomina;
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
        /// NOMBRE DE LA FUNCION: Consulta_Percepciones_Deducciones_Nomina
        /// DESCRIPCION : Consulta las Percepciones o Deducciones de Nómina que esta 
        ///               siendo consultada 
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 10-Noviembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Percepciones_Deducciones_Nomina(Cls_Cat_Tipos_Nominas_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de los Tipos de Nómminas

            try
            {
                //Consulta las Percepciones o Deducciones de la Nómina que esta siendo Consultada
                Mi_SQL = "SELECT " + Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Tip_Nom_Per_Ded_Det + "." + Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Clave + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Nombre + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Tip_Nom_Per_Ded_Det + "." + Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Tip_Nom_Per_Ded_Det + "." + Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Aplica_Todos;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Tip_Nom_Per_Ded_Det + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Tip_Nom_Per_Ded_Det + "." + Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID + " = " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID;
                Mi_SQL = Mi_SQL + " AND " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Tipo + " = '" + Datos.P_Tipo + "'";
                Mi_SQL = Mi_SQL + " AND " + Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Tip_Nom_Per_Ded_Det + "." + Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Tipo_Nomina_ID + " = '" + Datos.P_Tipo_Nomina_ID + "'";
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Clave;
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
        #endregion

        #region (Ctrl de Percepciones y/o Deducciones Tipo Nomina y a Nivel de Empleado)
        /// ***************************************************************************************************************************
        /// Nombre del Método: Aplicar_Concepto_Empleados_Tipo_Nomina
        /// 
        /// Descripción: Método que aplica el concepto a todos los empleados con el tipo de nómina a modificar o dar de alta.
        /// 
        /// Parámetros: Tipo_Nomina_ID.- Tipo de nómina a modificar o dar de alta. 
        ///             Percepcion_Deduccion_ID.- Conceto [Percepción ó Deduccion] ha aplicar al empleado.
        ///             Cantidad.- Monto a Dar o Retener [+/-] al empleado.
        ///             
        /// Usuario creo: Juan Alberto Hernández Negrete.
        /// Fecha Creo: Enero/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ***************************************************************************************************************************
        private static void Aplicar_Concepto_Empleados_Tipo_Nomina(String Tipo_Nomina_ID, String Percepcion_Deduccion_ID, Double Cantidad)
        {
            Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexión con la capa de negocios del módulo de empleados.
            DataTable Dt_Empleados = null;  //Variable que almacena una lista de los empleados que pertencen a este tipo de nómina.
            String Empleado_ID = "";        //Identificador unico del empleado.

            try
            {
                //Consultamos a todos los empleados con el tipo de nomina que es pasado como parámetro este método.
                Obj_Empleados.P_Tipo_Nomina_ID = Tipo_Nomina_ID;
                Dt_Empleados = Obj_Empleados.Consulta_Empleados_General();

                if (Dt_Empleados is DataTable)
                {
                    if (Dt_Empleados.Rows.Count > 0)
                    {
                        foreach (DataRow Empleado in Dt_Empleados.Rows)
                        {
                            if (Empleado is DataRow)
                            {
                                if (!string.IsNullOrEmpty(Empleado[Cat_Empleados.Campo_Empleado_ID].ToString()))
                                {
                                    Empleado_ID = Empleado[Cat_Empleados.Campo_Empleado_ID].ToString();
                                    //Asignamos y aplicamos el concepto al empleado con este tipo de nómina.
                                    Registro_Percepciones_Deducciones_Tipo_Nomina(Empleado_ID, Percepcion_Deduccion_ID, Cantidad);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al aplicar el conceptos a lo empleados que pertencen al tipo de nomina sobre el que se realiza la operación. Error: [" + Ex.Message + "]");
            }
        }
        /// ***************************************************************************************************************************
        /// Nombre del Método: Registro_Percepciones_Deducciones_Tipo_Nomina
        /// 
        /// Descripción: Método que aplica el concepto por empleado.
        /// 
        /// Parámetros: Empleado_ID.- Identificador del empleado al que se le asignara el concepto. 
        ///             Percepcion_Deduccion_ID.- Conceto [Percepción ó Deduccion] ha aplicar al empleado.
        ///             Cantidad.- Monto a Dar o Retener [+/-] al empleado.
        ///             
        /// Usuario creo: Juan Alberto Hernández Negrete.
        /// Fecha Creo: Enero/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ***************************************************************************************************************************
        private static void Registro_Percepciones_Deducciones_Tipo_Nomina(String Empleado_ID, String Percepcion_Deduccion_ID, Double Cantidad)
        {
            String Mi_SQL;           //Obtiene la cadena de inserción hacía la base de datos
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;

            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos

            try
            {
                Mi_SQL = "INSERT INTO " + Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + " (" +
                    Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID + ", " + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID + ", " +
                    Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Concepto + ", " + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad + ") VALUES(" +
                    "'" + Empleado_ID + "', " +
                    "'" + Percepcion_Deduccion_ID + "', " +
                    "'TIPO_NOMINA', " +
                    Cantidad + ")";
                //OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
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
        /// ***************************************************************************************************************************
        /// Nombre del Método: Desaplicar_Conceptos_Empleados_Tipo_Nomina_Eliminada
        /// 
        /// Descripción: Método que desaplica los conceptos a todos los empleados con el tipo de nómina que es pasado como parámetro.
        /// 
        /// Parámetros: Tipo_Nomina_ID.- Tipo de nómina a modificar o dar de alta. 
        ///             Percepcion_Deduccion_ID.- Conceto [Percepción ó Deduccion] ha aplicar al empleado.
        ///             
        /// Usuario creo: Juan Alberto Hernández Negrete.
        /// Fecha Creo: Enero/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ***************************************************************************************************************************
        private static void Desaplicar_Conceptos_Empleados_Tipo_Nomina_Eliminada(String Tipo_Nomina_ID, String Percepcion_Deduccion_ID)
        {
           Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexión con la capa de negocios del módulo de empleados.
            DataTable Dt_Empleados = null;  //Variable que almacena una lista de los empleados que pertencen a este tipo de nómina.
            String Empleado_ID = "";        //Identificador unico del empleado.

            try
            {
                //Consultamos a todos lo empleados que tienen este tipo de nómina.
                Obj_Empleados.P_Tipo_Nomina_ID = Tipo_Nomina_ID;
                Dt_Empleados = Obj_Empleados.Consulta_Empleados_General();

                if (Dt_Empleados is DataTable)
                {
                    if (Dt_Empleados.Rows.Count > 0)
                    {
                        foreach (DataRow Empleado in Dt_Empleados.Rows)
                        {
                            if (Empleado is DataRow)
                            {
                                if (!string.IsNullOrEmpty(Empleado[Cat_Empleados.Campo_Empleado_ID].ToString()))
                                {
                                    Empleado_ID = Empleado[Cat_Empleados.Campo_Empleado_ID].ToString();
                                    //Quitamos los conceptos por empleado.
                                    Eliminar_Percepciones_Deducciones_Empleados(Empleado_ID, Percepcion_Deduccion_ID);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al quitar desaplicar las percepciones y/o deducciones que le aplicaron en automatico a todos los empleados. Error: [" + Ex.Message + "]");
            }
        }
        /// ***************************************************************************************************************************
        /// Nombre del Método: Eliminar_Percepciones_Deducciones_Empleados
        /// 
        /// Descripción: Método que desaplica los conceptos por empleado.
        /// 
        /// Parámetros: Empleado_ID.- Identificador del empleado al que se le asignara el concepto. 
        ///             Percepcion_Deduccion_ID.- Conceto [Percepción ó Deduccion] ha aplicar al empleado.
        ///             
        /// Usuario creo: Juan Alberto Hernández Negrete.
        /// Fecha Creo: Enero/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ***************************************************************************************************************************
        private static void Eliminar_Percepciones_Deducciones_Empleados(String Empleado_ID, String Percepcion_Deduccion_ID)
        {
            String Mi_SQL;           //Obtiene la cadena de inserción hacía la base de datos
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;

            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos

            try
            {
                Mi_SQL = "DELETE FROM " + Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + " WHERE " +
                          Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID + "='" + Empleado_ID + "' AND " +
                          Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID + "='" + Percepcion_Deduccion_ID + "'";

                //OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
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
        /// ***************************************************************************************************************************
        /// Nombre del Método: Comparar_Tablas_Obtener_Perc_Deduc_Eliminar
        /// 
        /// Descripción: Método que obtiene un listado con las percepciones y/o deducciones que ya no aplicaran al tipo de nómina.
        /// 
        /// Parámetros: Tabla_1.- Listado Actual y el cual comenzara aplicar [Nuevo Listado de conceptos a aplicar al tipo de nómina].
        ///             Tabla_2.- Listado Antes de ajustar. Sobre el cuál se buscara si aun existe un concepto o si ya no o si es nuevo.
        ///             
        /// Usuario creo: Juan Alberto Hernández Negrete.
        /// Fecha Creo: Enero/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ***************************************************************************************************************************
        private static DataTable Comparar_Tablas_Obtener_Perc_Deduc_Eliminar(DataTable Tabla_1, DataTable Tabla_2)
        {
            DataTable Dt_Resultado = null;  //Variable que almacenara las filas de la tabla A que no se encuentran en la tabla B.
            DataRow[] Dr_Encontardo = null; //Variable que almacenara la fila encontrada al consultar la tabla 2.

            try
            {
                Dt_Resultado = Tabla_1.Clone();//Clonamos la tabla 1.

                if (Tabla_1 is DataTable) {
                    if (Tabla_1.Rows.Count > 0) {
                        foreach (DataRow Dr_T1_Renglon in Tabla_1.Rows)
                        {
                            if (Dr_T1_Renglon is DataRow) {
                                if (!String.IsNullOrEmpty(Dr_T1_Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID].ToString()))
                                {
                                    Dr_Encontardo = Tabla_2.Select(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID + "='" +
                                        Dr_T1_Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID].ToString() + "'");

                                    if (Dr_Encontardo != null)
                                    {
                                        if (Dr_Encontardo.Length == 0)
                                        {
                                            Dt_Resultado.ImportRow(Dr_T1_Renglon);
                                            Dt_Resultado.AcceptChanges();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }    
            }
            catch (Exception Ex)
            {
                throw new Exception("Error generado al comparar dos datatables. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }
        /// ***************************************************************************************************************************
        /// Nombre del Método: Comparar_Tablas_Obtener_Perc_Deduc_Nuevas
        /// 
        /// Descripción: Método que obtiene un listado con las percepciones y/o deducciones que comenzaran aplicaran al tipo de nómina.
        /// 
        /// Parámetros: Tabla_1.- Listado Actual y el cual comenzara aplicar [Nuevo Listado de conceptos a aplicar al tipo de nómina].
        ///             Tabla_2.- Listado Antes de ajustar. Sobre el cuál se buscara si aun existe un concepto o si ya no o si es nuevo.
        ///             
        /// Usuario creo: Juan Alberto Hernández Negrete.
        /// Fecha Creo: Enero/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ***************************************************************************************************************************
        private static DataTable Comparar_Tablas_Obtener_Perc_Deduc_Nuevas(DataTable Tabla_1, DataTable Tabla_2)
        {
            DataTable Dt_Resultado = null;  //Variable que almacenara las filas de la tabla A que no se encuentran en la tabla B.
            DataRow[] Dr_Encontardo = null; //Variable que almacenara la fila encontrada al consultar la tabla 2.

            try
            {
                Dt_Resultado = Tabla_1.Clone();//Clonamos la tabla 1.

                if (Tabla_1 is DataTable)
                {
                    if (Tabla_1.Rows.Count > 0)
                    {
                        foreach (DataRow Dr_T1_Renglon in Tabla_1.Rows)
                        {
                            if (Dr_T1_Renglon is DataRow)
                            {
                                if (!String.IsNullOrEmpty(Dr_T1_Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID].ToString()))
                                {
                                    Dr_Encontardo = Tabla_2.Select(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID + "='" +
                                        Dr_T1_Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID].ToString() + "'");

                                    if (Dr_Encontardo != null)
                                    {
                                        if (Dr_Encontardo.Length == 0)
                                        {
                                            Dt_Resultado.ImportRow(Dr_T1_Renglon);
                                            Dt_Resultado.AcceptChanges();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error generado al comparar dos datatables. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }
        /// ***************************************************************************************************************************
        /// Nombre del Método: Comparar_Tablas_Obtener_Perc_Deduc_Cantidades_Cambiaron
        /// 
        /// Descripción: Método que compara las cantidades de los conceptos entre una tabla que contiene el estado anterior de los montos
        ///              y otra que contiene el estado actual de los montos. Esto para obtener un listado de los conceptos que se actualizaron.
        /// 
        /// Parámetros: Tabla_1.- Listado Actual y el cual comenzara aplicar [Nuevo Listado de conceptos a aplicar al tipo de nómina].
        ///             Tabla_2.- Listado Antes de ajustar. Sobre el cuál se buscara si aun existe un concepto o si ya no o si es nuevo.
        ///             
        /// Usuario creo: Juan Alberto Hernández Negrete.
        /// Fecha Creo: Enero/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ***************************************************************************************************************************
        private static DataTable Comparar_Tablas_Obtener_Perc_Deduc_Cantidades_Cambiaron(DataTable Tabla_1, DataTable Tabla_2)
        {
            DataTable Dt_Resultado = null;  //Variable que almacenara las filas de la tabla A que no se encuentran en la tabla B.
            DataRow[] Dr_Encontardo = null; //Variable que almacenara la fila encontrada al consultar la tabla 2.
            Double Cantidad_1  = 0;         //Variable que almacena la cantidad que tenia el concepto asignado antes de la modificacion.
            Double Cantidad_2 = 0;          //Variable que almacena la cantidad que tenia el concepto asignado despues de la modificacion.

            try
            {
                Dt_Resultado = Tabla_1.Clone();//Clonamos la tabla 1.

                if (Tabla_1 is DataTable)
                {
                    if (Tabla_1.Rows.Count > 0)
                    {
                        foreach (DataRow Dr_T1_Renglon in Tabla_1.Rows)
                        {
                            if (Dr_T1_Renglon is DataRow)
                            {
                                if (!String.IsNullOrEmpty(Dr_T1_Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID].ToString()))
                                {
                                    Dr_Encontardo = Tabla_2.Select(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID + "='" +
                                        Dr_T1_Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID].ToString() + "'");

                                    if (Dr_Encontardo != null)
                                    {
                                        if (Dr_Encontardo.Length == 1)
                                        {
                                            if (!string.IsNullOrEmpty(Dr_Encontardo[0][Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad].ToString()))
                                                Cantidad_1 = Convert.ToDouble(Dr_Encontardo[0][Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad].ToString());
                                            if (!string.IsNullOrEmpty(Dr_T1_Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad].ToString()))
                                                Cantidad_2 = Convert.ToDouble(Dr_T1_Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad].ToString());

                                            if ((Cantidad_1 != Cantidad_2))
                                            {
                                                Dt_Resultado.ImportRow(Dr_Encontardo[0]);
                                                Dt_Resultado.AcceptChanges();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error generado al comparar dos datatables. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }
        /// ***************************************************************************************************************************
        /// Nombre del Método: Comparar_Tablas_Obtener_Perc_Deduc_Aplica
        /// 
        /// Descripción: Método que controla si el concepto aplicara a todos los empleados que tienen asignado este tipo de nómina.
        /// 
        /// Parámetros: Tabla_1.- Listado Actual y el cual comenzara aplicar [Nuevo Listado de conceptos a aplicar al tipo de nómina].
        ///             Tabla_2.- Listado Antes de ajustar. Sobre el cuál se buscara si aun existe un concepto o si ya no o si es nuevo.
        ///             
        /// Usuario creo: Juan Alberto Hernández Negrete.
        /// Fecha Creo: Enero/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ***************************************************************************************************************************
        private static DataTable Comparar_Tablas_Obtener_Perc_Deduc_Aplica(DataTable Tabla_1, DataTable Tabla_2)
        {
            DataTable Dt_Resultado = null;  //Variable que almacenara las filas de la tabla A que no se encuentran en la tabla B.
            DataRow[] Dr_Encontardo = null; //Variable que almacenara la fila encontrada al consultar la tabla 2.
            String Aplica_Todos_T1 = "";
            String Aplica_Todos_T2 = "";

            try
            {
                Dt_Resultado = Tabla_1.Clone();//Clonamos la tabla 1.

                if (Tabla_1 is DataTable)
                {
                    if (Tabla_1.Rows.Count > 0)
                    {
                        foreach (DataRow Dr_T1_Renglon in Tabla_1.Rows)
                        {
                            if (Dr_T1_Renglon is DataRow)
                            {
                                if (!String.IsNullOrEmpty(Dr_T1_Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID].ToString()))
                                {
                                    Dr_Encontardo = Tabla_2.Select(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID + "='" +
                                        Dr_T1_Renglon[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID].ToString() + "'");

                                    if (Dr_Encontardo != null)
                                    {
                                        if (Dr_Encontardo.Length == 1)
                                        {
                                            if (!string.IsNullOrEmpty(Dr_Encontardo[0][Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Aplica_Todos].ToString()))
                                                Aplica_Todos_T1 = Dr_Encontardo[0][Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Aplica_Todos].ToString();
                                            if (!string.IsNullOrEmpty(Dr_T1_Renglon[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Aplica_Todos].ToString()))
                                                Aplica_Todos_T2 = Dr_T1_Renglon[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Aplica_Todos].ToString();

                                            if (!(Aplica_Todos_T1.Equals(Aplica_Todos_T2)))
                                            {
                                                Dt_Resultado.ImportRow(Dr_Encontardo[0]);
                                                Dt_Resultado.AcceptChanges();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error generado al comparar dos datatables. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }
        /// ***************************************************************************************************************************
        /// Nombre del Método: Actualizar_Conceptos_Empleados_Tipo_Nomina_Modificar_Cantidades
        /// 
        /// Descripción: Método que actualiza los montos de los conceptos que de cuales se modificaron sus cantidades por tipo de nómina.
        /// 
        /// Parámetros: Tipo_Nomina_ID.- Tipo de nómina del cual se ajustara el monto que tenia asignado el concepto.
        ///             Percepcion_Deduccion_ID.- Concepto [Percepción y/o Deducción] a modificar.
        ///             Cantidad.- Nuevo monto ajustar al concepto.
        ///             
        /// Usuario creo: Juan Alberto Hernández Negrete.
        /// Fecha Creo: Enero/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ***************************************************************************************************************************
        private static void Actualizar_Conceptos_Empleados_Tipo_Nomina_Modificar_Cantidades(String Tipo_Nomina_ID, String Percepcion_Deduccion_ID,
                Double Cantidad)
        {
            Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexión con la capa de negocios del módulo de empleados.
            DataTable Dt_Empleados = null;  //Variable que almacena una lista de los empleados que pertencen a este tipo de nómina.
            String Empleado_ID = "";        //Identificador unico del empleado.

            try
            {
                Obj_Empleados.P_Tipo_Nomina_ID = Tipo_Nomina_ID;
                Dt_Empleados = Obj_Empleados.Consulta_Empleados_General();

                if (Dt_Empleados is DataTable)
                {
                    if (Dt_Empleados.Rows.Count > 0)
                    {
                        foreach (DataRow Empleado in Dt_Empleados.Rows)
                        {
                            if (Empleado is DataRow)
                            {
                                if (!string.IsNullOrEmpty(Empleado[Cat_Empleados.Campo_Empleado_ID].ToString()))
                                {
                                    Empleado_ID = Empleado[Cat_Empleados.Campo_Empleado_ID].ToString();
                                    Modificar_Cantidades_Percepciones_Deducciones_Empleados(Empleado_ID, Percepcion_Deduccion_ID, Cantidad);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al quitar desaplicar las percepciones y/o deducciones que le aplicaron en automatico a todos los empleados. Error: [" + Ex.Message + "]");
            }
        }
        /// ***************************************************************************************************************************
        /// Nombre del Método: Modificar_Cantidades_Percepciones_Deducciones_Empleados
        /// 
        /// Descripción: Método que ejecuta la actualización de los montos a nivel de concepto [Percepción y/o Deducción] y por empleado.
        /// 
        /// Parámetros: Empleado_ID.- Empleado al cuál se le actualizara el monto del concepto por su tipo de nómina.
        ///             Percepcion_Deduccion_ID.- Concepto [Percepción y/o Deducción] a modificar.
        ///             Cantidad.- Nuevo monto ajustar al concepto.
        ///             
        /// Usuario creo: Juan Alberto Hernández Negrete.
        /// Fecha Creo: Enero/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ***************************************************************************************************************************
        private static void Modificar_Cantidades_Percepciones_Deducciones_Empleados(String Empleado_ID, String Percepcion_Deduccion_ID, Double Cantidad)
        {
            String Mi_SQL;           //Obtiene la cadena de inserción hacía la base de datos
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;

            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos

            try
            {
                Mi_SQL = "UPDATE " + Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + " SET " +
                          Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad + "=" + Cantidad +
                          " WHERE " +
                          Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID + "='" + Empleado_ID + "' AND " +
                          Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID + "='" + Percepcion_Deduccion_ID + "'";

                //OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
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
        #endregion
    }
}
