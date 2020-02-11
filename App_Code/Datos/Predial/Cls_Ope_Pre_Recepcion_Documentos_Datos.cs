using System;
using System.Data;
using System.Collections.Generic;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Operacion_Predial_Recepcion_Documentos.Negocio;
using Presidencia.Operacion_Predial_Empleados_Activos.Negocio;


namespace Presidencia.Operacion_Predial_Recepcion_Documentos.Datos
{

    public class Cls_Ope_Pre_Recepcion_Documentos_Datos
    {
        public Cls_Ope_Pre_Recepcion_Documentos_Datos()
        {
        }

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Alta_Recepcion_Documentos
        /// 	DESCRIPCIÓN: 1.Consulta el último ID dado de alta para poder ingresar el siguiente
        ///                  2. Da de Alta los datos de la recepción de documentos por notario en la BD con los 
        ///                  datos proporcionados por el usuario
        ///                  Regresa el valor de un contador de tramites
        /// 	PARÁMETROS:
        /// 		1. Datos: Instancia de la clase de negocio con los datos que serán insertados en la base de datos
        /// 	CREO: Roberto González Oseguera
        /// 	FECHA_CREO: 24-mar-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static int Alta_Recepcion_Documentos(Cls_Ope_Pre_Recepcion_Documentos_Negocio Datos, DataTable Tabla_Tramites)
        {
            String Mi_SQL;  //Obtiene la cadena de inserción hacia la base de datos
            Object Documento_ID; //Obtiene el ID con la cual se guardo los datos en la base de datos
            int Contador_Tramites = 0;
            String No_Movimiento;
            String No_Anexo;
            //Variable para transacción 
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion);
            OracleCommand Comando_SQL = new OracleCommand();
            OracleTransaction Transaccion_SQL;

            

            // Si no hay una conexión hacia la bd, abrirla
            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open();
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);
            Comando_SQL.Connection = Conexion_Base;
            Comando_SQL.Transaction = Transaccion_SQL;

            try
            {
            //obtener el ultimo ID y sumar 1 para obtener nuevo ID
            Mi_SQL = "SELECT NVL(MAX(" + Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + "),'0000000000') ";
            Mi_SQL = Mi_SQL + "FROM " + Ope_Pre_Recepcion_Documentos.Tabla_Ope_Pre_Recepcion_Documentos;
            Documento_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            if (Convert.IsDBNull(Documento_ID))
            {
                Datos.P_No_Recepcion_Documento = "0000000001";
            }
            else
            {
                Datos.P_No_Recepcion_Documento = String.Format("{0:0000000000}", Convert.ToInt32(Documento_ID) + 1);                
            }
            Contador_Tramites = Convert.ToInt32(Documento_ID);
            //obtener el ID de la tabla de Movimientos de recepciones de documentos -------- No_Movimiento
            Mi_SQL = "SELECT NVL(MAX(" + Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento + "),'0000000000') ";
            Mi_SQL = Mi_SQL + "FROM " + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs;
            Documento_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            if (Convert.IsDBNull(Documento_ID))
            {
                No_Movimiento = "0000000000";
            }
            else
            {
                No_Movimiento = String.Format("{0:0000000000}", Convert.ToInt32(Documento_ID));
            }
            //obtener el ID de la tabla de Anexos de recepciones de documentos -------- No_Anexo
            Mi_SQL = "SELECT NVL(MAX(" + Ope_Pre_Recep_Docs_Anexos.Campo_No_Anexo + "),'0000000000') ";
            Mi_SQL = Mi_SQL + "FROM " + Ope_Pre_Recep_Docs_Anexos.Tabla_Ope_Pre_Recep_Docs_Anexos;
            Documento_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            if (Convert.IsDBNull(Documento_ID))
            {
                No_Anexo = "0000000000";
            }
            else
            {
                No_Anexo = String.Format("{0:0000000000}", Convert.ToInt32(Documento_ID));
            }
            Mi_SQL = "SELECT NVL(MAX(" + Ope_Pre_Historial_Obs_Recep.Campo_No_Observacion_ID + "),'0000000000') ";
            Mi_SQL = Mi_SQL + "FROM " + Ope_Pre_Historial_Obs_Recep.Tabla_Ope_Pre_Historial_Obs_Recep;
            Documento_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            if (Convert.IsDBNull(Documento_ID))
            {
                Datos.P_Observacion_ID = "0000000001";
            }
            else
            {
                Datos.P_Observacion_ID = String.Format("{0:0000000000}", Convert.ToInt32(Documento_ID) + 1);
            }

            //Consulta para la inserción de la Recepción de documento con los datos proporcionados por el usuario
            Mi_SQL = "INSERT INTO " + Ope_Pre_Recepcion_Documentos.Tabla_Ope_Pre_Recepcion_Documentos + " (";
            Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + ", ";
            Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_Fecha + ", ";
            Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_Clave_Tramite + ", ";
            Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_Notario_ID + ", ";
            //COMENTE ESTAS OBSERVAIONES
            //Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_Observaciones + ", ";
            Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_Anio + ", ";
            Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_Usuario_Creo + ", ";
            Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_Fecha_Creo + ") VALUES ('";
            Mi_SQL = Mi_SQL + Datos.P_No_Recepcion_Documento + "', ";
            //Mi_SQL = Mi_SQL + Datos.P_Observaciones + "', ";
            Mi_SQL = Mi_SQL + "SYSDATE, '";
            Mi_SQL = Mi_SQL + Datos.P_Clave_Tramite + "', '";
            Mi_SQL = Mi_SQL + Datos.P_Notario_ID + "',' ";

            //Mi_SQL = Mi_SQL + Datos.P_Observaciones + "', ";
            Mi_SQL = Mi_SQL + DateTime.Now.Year.ToString() + "', '";
            Mi_SQL = Mi_SQL + Datos.P_Nombre_Usuario + "', SYSDATE)";

            Comando_SQL.CommandText = Mi_SQL;
            Comando_SQL.ExecuteNonQuery();              //Ejecutar consulta de alta de recepcion de documentos
            foreach (DataRow Fila_Tramite in Tabla_Tramites.Rows)   // recorrer la tabla de tramites recibida y agregar cada tramite a la tabla movimientos de Recep. Docs.
            {
                Cls_Ope_Pre_Empleado_Activos_Negocio Empleado_Activo = new Cls_Ope_Pre_Empleado_Activos_Negocio();
                No_Movimiento = String.Format("{0:0000000000}", Convert.ToInt32(No_Movimiento) + 1); //nuevo ID
                Mi_SQL = "INSERT INTO " + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs + " (";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento + ", ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + ", ";
                //OBSERVACIONES
                //Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Observaciones + ", ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Numero_Escritura + ", ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Fecha_Escritura + ", ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Cuenta_Predial_ID + ", ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Estatus + ", ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Anio + ", ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Empleado_ID + ", ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Usuario_Creo + ", ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Fecha_Creo + ") VALUES ('";
                Mi_SQL += No_Movimiento + "', '";
                Mi_SQL += Datos.P_No_Recepcion_Documento + "', '";
                //OBSERVACIONES
                //Mi_SQL += Fila_Tramite["OBSERVACIONES"].ToString() + "', '";
                Mi_SQL += Fila_Tramite["NO_ESCRITURA"].ToString() + "', '";
                Mi_SQL += Convert.ToDateTime(Fila_Tramite["FECHA_ESCRITURA"].ToString()).ToString("dd/MM/yyyy") + "', '";   //cambiar formato fecha
                Mi_SQL += Fila_Tramite["CUENTA_PREDIAL_ID"].ToString() + "', '";
                Mi_SQL += "PENDIENTE" + "',";                                     // estatus inicial pendiente
                //Mi_SQL += Fila_Tramite["COMENTARIOS"].ToString() + "', '";
                Mi_SQL += DateTime.Now.Year.ToString() + ",'";
                Mi_SQL += Empleado_Activo.Asignar_Pendiente() + "', '";
                Mi_SQL += Datos.P_Nombre_Usuario + "', SYSDATE)";

                Comando_SQL.CommandText = Mi_SQL;
                Comando_SQL.ExecuteNonQuery();              //Ejecutar consulta de alta de Movimiento de la recepcion de documentos

                if (!String.IsNullOrEmpty(Fila_Tramite["Comentarios"].ToString()))
                {
                    Mi_SQL = "INSERT INTO " + Ope_Pre_Historial_Obs_Recep.Tabla_Ope_Pre_Historial_Obs_Recep + " (";
                    Mi_SQL = Mi_SQL + Ope_Pre_Historial_Obs_Recep.Campo_No_Observacion_ID + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Historial_Obs_Recep.Campo_No_Movimiento + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Historial_Obs_Recep.Campo_Observaciones + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Historial_Obs_Recep.Campo_Usuario_Creo + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Historial_Obs_Recep.Campo_Fecha_Creo + ") VALUES ('";
                    Mi_SQL += Datos.P_Observacion_ID + "', '";
                    Mi_SQL += No_Movimiento + "', UPPER('";
                    Mi_SQL += Fila_Tramite["Comentarios"].ToString().Trim() + "'), '";
                    Mi_SQL += Datos.P_Nombre_Usuario + "', SYSDATE)";

                    Comando_SQL.CommandText = Mi_SQL;
                    Comando_SQL.ExecuteNonQuery();
                    Datos.P_Observacion_ID = String.Format("{0:0000000000}", Convert.ToInt32(Datos.P_Observacion_ID) + 1);
                }

                String[] Arr_Archivos = Fila_Tramite["NOMBRES_ARCHIVO"].ToString().Split(',');  //recuperar nombres de archivos y tipos de documento
                String[] Arr_Tipos_Documento = Fila_Tramite["TIPOS_DOCUMENTO"].ToString().Split(',');
                for (int i = 0; i < Arr_Tipos_Documento.Length - 1; i++)    // recorrer el arreglo de documentos recibidos para cada movimiento
                {
                    No_Anexo = String.Format("{0:0000000000}", Convert.ToInt32(No_Anexo) + 1); // siguiente ID de anexo
                    Mi_SQL = "INSERT INTO " + Ope_Pre_Recep_Docs_Anexos.Tabla_Ope_Pre_Recep_Docs_Anexos + " (";
                    Mi_SQL += Ope_Pre_Recep_Docs_Anexos.Campo_No_Anexo + ", ";
                    Mi_SQL += Ope_Pre_Recep_Docs_Anexos.Campo_No_Movimiento + ", ";
                    Mi_SQL += Ope_Pre_Recep_Docs_Anexos.Campo_Ruta + ", ";
                    Mi_SQL += Ope_Pre_Recep_Docs_Anexos.Campo_Documento_ID + ", ";
                    Mi_SQL += Ope_Pre_Recep_Docs_Anexos.Campo_Usuario_Creo + ", ";
                    Mi_SQL += Ope_Pre_Recep_Docs_Anexos.Campo_Fecha_Creo + ") VALUES ('";
                    Mi_SQL += No_Anexo + "', '";
                    Mi_SQL += No_Movimiento + "', '";
                    Mi_SQL += Arr_Archivos[i] + "', '";
                    Mi_SQL += Arr_Tipos_Documento[i] + "', '";
                    Mi_SQL += Datos.P_Nombre_Usuario + "', SYSDATE)";

                    Comando_SQL.CommandText = Mi_SQL;
                    Comando_SQL.ExecuteNonQuery();              //Ejecutar consulta de alta de Movimiento de la recepcion de documentos
                
            
                
                //FOR PARA INSERTAR OBSERVACIONES
                //for (int i = 0; i < Datos.P_Dt_Observaciones.Rows.Count; i++)
                //{
                    //obtener el ultimo ID y sumar 1 para obtener nuevo ID
                    
                    ////obtener el ID de la tabla de Movimientos de recepciones de documentos -------- No_Movimiento
                    //Mi_SQL = "SELECT NVL(MAX(" + Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento + "),'0000000000') ";
                    //Mi_SQL = Mi_SQL + "FROM " + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs;
                    //Documento_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                    //if (Convert.IsDBNull(Documento_ID))
                    //{
                    //    No_Movimiento = "0000000000";
                    //}
                    //else
                    //{
                    //    No_Movimiento = String.Format("{0:0000000000}", Convert.ToInt32(Documento_ID));
                    //}
                //}

            }

            }
                Transaccion_SQL.Commit();                   //Enviar los cambios a la BD

                //regresar el número de inserciones realizadas
                return Contador_Tramites;
            }
            catch (OracleException Ex)
            {
                if (Transaccion_SQL != null)        // Si el envío de la consulta SQL es nulo, hacer rollback de los datos
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error Alta_Recepcion_Documentos: " + Ex.Message);
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
                Conexion_Base.Close();
            }
        }

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Alta_Recepcion_Documento_Modifica
        /// 	DESCRIPCIÓN: 1.Consulta el último ID dado de alta para poder ingresar el siguiente
        ///                  2. Da de Alta los datos de la recepción de documentos por notario en la BD con los 
        ///                  datos proporcionados por el usuario
        ///                  Regresa el valor de un contador de tramites
        /// 	PARÁMETROS:
        /// 		1. Datos: Instancia de la clase de negocio con los datos que serán insertados en la base de datos
        /// 	CREO: Roberto González Oseguera
        /// 	FECHA_CREO: 24-mar-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static int Alta_Recepcion_Documento_Modifica(Cls_Ope_Pre_Recepcion_Documentos_Negocio Datos, DataTable Tabla_Tramites)
        {
            String Mi_SQL;  //Obtiene la cadena de inserción hacia la base de datos
            Object Documento_ID; //Obtiene el ID con la cual se guardo los datos en la base de datos
            int Contador_Tramites = 0;
            String No_Movimiento;
            String No_Anexo;
            //Variable para transacción 
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion);
            OracleCommand Comando_SQL = new OracleCommand();
            OracleTransaction Transaccion_SQL;

            // Si no hay una conexión hacia la bd, abrirla
            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open();
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);
            Comando_SQL.Connection = Conexion_Base;
            Comando_SQL.Transaction = Transaccion_SQL;

            ////obtener el ultimo ID y sumar 1 para obtener nuevo ID
            //Mi_SQL = "SELECT NVL(MAX(" + Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + "),'0000000000') ";
            //Mi_SQL = Mi_SQL + "FROM " + Ope_Pre_Recepcion_Documentos.Tabla_Ope_Pre_Recepcion_Documentos;
            //Documento_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            //if (Convert.IsDBNull(Documento_ID))
            //{
            //    Datos.P_No_Recepcion_Documento = "0000000001";
            //}
            //else
            //{
            //    Datos.P_No_Recepcion_Documento = String.Format("{0:0000000000}", Convert.ToInt32(Documento_ID) + 1);
            //}
            //obtener el ID de la tabla de Movimientos de recepciones de documentos -------- No_Movimiento
            Mi_SQL = "SELECT NVL(MAX(" + Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento + "),'0000000000') ";
            Mi_SQL = Mi_SQL + "FROM " + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs;
            Documento_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            if (Convert.IsDBNull(Documento_ID))
            {
                No_Movimiento = "0000000000";
            }
            else
            {
                No_Movimiento = String.Format("{0:0000000000}", Convert.ToInt32(Documento_ID));
            }
            //obtener el ID de la tabla de Anexos de recepciones de documentos -------- No_Anexo
            Mi_SQL = "SELECT NVL(MAX(" + Ope_Pre_Recep_Docs_Anexos.Campo_No_Anexo + "),'0000000000') ";
            Mi_SQL = Mi_SQL + "FROM " + Ope_Pre_Recep_Docs_Anexos.Tabla_Ope_Pre_Recep_Docs_Anexos;
            Documento_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            if (Convert.IsDBNull(Documento_ID))
            {
                No_Anexo = "0000000000";
            }
            else
            {
                No_Anexo = String.Format("{0:0000000000}", Convert.ToInt32(Documento_ID));
            }
            Mi_SQL = "SELECT NVL(MAX(" + Ope_Pre_Historial_Obs_Recep.Campo_No_Observacion_ID + "),'0000000000') ";
            Mi_SQL = Mi_SQL + "FROM " + Ope_Pre_Historial_Obs_Recep.Tabla_Ope_Pre_Historial_Obs_Recep;
            Documento_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            if (Convert.IsDBNull(Documento_ID))
            {
                Datos.P_Observacion_ID = "0000000001";
            }
            else
            {
                Datos.P_Observacion_ID = String.Format("{0:0000000000}", Convert.ToInt32(Documento_ID) + 1);
            }
            ////Consulta para la inserción de la Recepción de documento con los datos proporcionados por el usuario
            //Mi_SQL = "INSERT INTO " + Ope_Pre_Recepcion_Documentos.Tabla_Ope_Pre_Recepcion_Documentos + " (";
            //Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + ", ";
            //Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_Fecha + ", ";
            //Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_Clave_Tramite + ", ";
            //Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_Notario_ID + ", ";
            //Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_Observaciones + ", ";
            //Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_Anio + ", ";
            //Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_Usuario_Creo + ", ";
            //Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_Fecha_Creo + ") VALUES ('";
            //Mi_SQL = Mi_SQL + Datos.P_No_Recepcion_Documento + "', ";
            //Mi_SQL = Mi_SQL + "SYSDATE, '";
            //Mi_SQL = Mi_SQL + Datos.P_Clave_Tramite + "', '";
            //Mi_SQL = Mi_SQL + Datos.P_Notario_ID + "',' ";
            //Mi_SQL = Mi_SQL + Datos.P_Observaciones + "', ";
            //Mi_SQL = Mi_SQL + DateTime.Now.Year.ToString() + ", '";
            //Mi_SQL = Mi_SQL + Datos.P_Nombre_Usuario + "', SYSDATE)";

            //Comando_SQL.CommandText = Mi_SQL;
            //Comando_SQL.ExecuteNonQuery();              //Ejecutar consulta de alta de recepcion de documentos
            foreach (DataRow Fila_Tramite in Tabla_Tramites.Rows)   // recorrer la tabla de tramites recibida y agregar cada tramite a la tabla movimientos de Recep. Docs.
            {
                Cls_Ope_Pre_Empleado_Activos_Negocio Empleado_Activo = new Cls_Ope_Pre_Empleado_Activos_Negocio();
                No_Movimiento = String.Format("{0:0000000000}", Convert.ToInt32(No_Movimiento) + 1); //nuevo ID
                Mi_SQL = "INSERT INTO " + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs + " (";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento + ", ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + ", ";
                //OBSERVACIONES
                //Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Observaciones + ", ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Numero_Escritura + ", ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Fecha_Escritura + ", ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Cuenta_Predial_ID + ", ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Estatus + ", ";
                //Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Observaciones + ", ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Anio + ", ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Empleado_ID + ", ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Usuario_Creo + ", ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Fecha_Creo + ") VALUES ('";
                Mi_SQL += No_Movimiento + "', '";
                Mi_SQL += Datos.P_No_Recepcion_Documento + "', '";
                //OBSERVACIONES
                //Mi_SQL += Fila_Tramite["COMENTARIOS"].ToString() + "', '";
                Mi_SQL += Fila_Tramite["NO_ESCRITURA"].ToString() + "', '";
                Mi_SQL += Convert.ToDateTime(Fila_Tramite["FECHA_ESCRITURA"].ToString()).ToString("dd/MM/yyyy") + "', '";   //cambiar formato fecha
                Mi_SQL += Fila_Tramite["CUENTA_PREDIAL_ID"].ToString() + "', '";
                Mi_SQL += "PENDIENTE" + "', ";                                     // estatus inicial pendiente
                //Mi_SQL += Fila_Tramite["COMENTARIOS"].ToString() + "', '";
                Mi_SQL += DateTime.Now.Year.ToString() + ",'";
                Mi_SQL += Empleado_Activo.Asignar_Pendiente() + "', '";
                Mi_SQL += Datos.P_Nombre_Usuario + "', SYSDATE)";

                Comando_SQL.CommandText = Mi_SQL;
                Comando_SQL.ExecuteNonQuery();              //Ejecutar consulta de alta de Movimiento de la recepcion de documentos


                Mi_SQL = "INSERT INTO " + Ope_Pre_Historial_Obs_Recep.Tabla_Ope_Pre_Historial_Obs_Recep + " (";
                Mi_SQL = Mi_SQL + Ope_Pre_Historial_Obs_Recep.Campo_No_Observacion_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Historial_Obs_Recep.Campo_No_Movimiento + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Historial_Obs_Recep.Campo_Observaciones + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Historial_Obs_Recep.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Historial_Obs_Recep.Campo_Fecha_Creo + ") VALUES ('";
                Mi_SQL += Datos.P_Observacion_ID + "', ' UPPER(";
                Mi_SQL += No_Movimiento + "', '";
                Mi_SQL += Fila_Tramite["Comentarios"].ToString().Trim() + "'), '";
                Mi_SQL += Datos.P_Nombre_Usuario + "', SYSDATE)";

                Comando_SQL.CommandText = Mi_SQL;
                Comando_SQL.ExecuteNonQuery();
                Datos.P_Observacion_ID = String.Format("{0:0000000000}", Convert.ToInt32(Datos.P_Observacion_ID) + 1);

                String[] Arr_Archivos = Fila_Tramite["NOMBRES_ARCHIVO"].ToString().Split(',');  //recuperar nombres de archivos y tipos de documento
                String[] Arr_Tipos_Documento = Fila_Tramite["TIPOS_DOCUMENTO"].ToString().Split(',');
                for (int i = 0; i < Arr_Tipos_Documento.Length - 1; i++)    // recorrer el arreglo de documentos recibidos para cada movimiento
                {
                    No_Anexo = String.Format("{0:0000000000}", Convert.ToInt32(No_Anexo) + 1); // siguiente ID de anexo
                    Mi_SQL = "INSERT INTO " + Ope_Pre_Recep_Docs_Anexos.Tabla_Ope_Pre_Recep_Docs_Anexos + " (";
                    Mi_SQL += Ope_Pre_Recep_Docs_Anexos.Campo_No_Anexo + ", ";
                    Mi_SQL += Ope_Pre_Recep_Docs_Anexos.Campo_No_Movimiento + ", ";
                    Mi_SQL += Ope_Pre_Recep_Docs_Anexos.Campo_Ruta + ", ";
                    Mi_SQL += Ope_Pre_Recep_Docs_Anexos.Campo_Documento_ID + ", ";
                    Mi_SQL += Ope_Pre_Recep_Docs_Anexos.Campo_Usuario_Creo + ", ";
                    Mi_SQL += Ope_Pre_Recep_Docs_Anexos.Campo_Fecha_Creo + ") VALUES ('";
                    Mi_SQL += No_Anexo + "', '";
                    Mi_SQL += No_Movimiento + "', '";
                    Mi_SQL += Arr_Archivos[i] + "', '";
                    Mi_SQL += Arr_Tipos_Documento[i] + "', '";
                    Mi_SQL += Datos.P_Nombre_Usuario + "', SYSDATE)";

                    Comando_SQL.CommandText = Mi_SQL;
                    Comando_SQL.ExecuteNonQuery();              //Ejecutar consulta de alta de Movimiento de la recepcion de documentos
                }
            }

            try
            {

                Transaccion_SQL.Commit();                   //Enviar los cambios a la BD

                //regresar el número de inserciones realizadas
                return Contador_Tramites;
            }
            catch (OracleException Ex)
            {
                if (Transaccion_SQL != null)        // Si el envío de la consulta SQL es nulo, hacer rollback de los datos
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error Alta_Recepcion_Documento_Modifica: " + Ex.Message);
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
                Conexion_Base.Close();
            }
        }

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Modificar_Recepcion_Documentos
        /// 	DESCRIPCIÓN: Modifica los datos de una recepción de documentos con lo que introdujo el usuario. 
        /// 	            Regresa el número de filas modificadas
        /// 	            Utiliza una transacción debido a que se afectan las tablas 
        /// 	            Ope_Pre_Recep_Docs_Movs y Ope_Pre_Recep_Docs_Anexos
        /// 	PARÁMETROS:
        /// 		1. Datos: Instancia de la capa de negocio con los datos a modificar del movimiento o tramite
        /// 		2. Anexos_Alta: Datatable con los datos de los anexos a dar de alta 
        /// 		3. Anexos_Actualizar: Datatable con la informacion que se va a actualizar en la tabla anexos
        /// 		4. Anexos_Eliminar: Lista de no_anexo que se van a eliminar de la tabla anexos
        /// 	CREO: Roberto González Oseguera
        /// 	FECHA_CREO: 16-may-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static int Modificar_Recepcion_Documentos(Cls_Ope_Pre_Recepcion_Documentos_Negocio Datos, DataTable Anexos_Alta, DataTable Anexos_Actualizar, List<String> Anexos_Eliminar)
        {
            String Mi_SQL; //Obtiene la cadena de modificación hacia la base de datos
            int Cnt_Filas_Afectadas = 0;
            object Documento_ID;
            String No_Anexo;
            //String No_Comentario;
            //Variable para transacción 
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion);
            OracleCommand Comando_SQL = new OracleCommand();
            OracleTransaction Transaccion_SQL;

            // Si no hay una conexión hacia la bd, abrirla
            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open();
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);
            Comando_SQL.Connection = Conexion_Base;
            Comando_SQL.Transaction = Transaccion_SQL;

            //obtener el ID de la tabla de Anexos de recepciones de documentos -------- No_Anexo
            Mi_SQL = "SELECT NVL(MAX(" + Ope_Pre_Recep_Docs_Anexos.Campo_No_Anexo + "),'0000000000') ";
            Mi_SQL = Mi_SQL + "FROM " + Ope_Pre_Recep_Docs_Anexos.Tabla_Ope_Pre_Recep_Docs_Anexos;


            Documento_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            if (Convert.IsDBNull(Documento_ID))
            {
                No_Anexo = "0000000000";
            }
            else
            {
                No_Anexo = String.Format("{0:0000000000}", Convert.ToInt32(Documento_ID));
            }
            //obtener el ID de la tabla de Comentarios de recepciones de documentos -------- No_Comentario
            //Mi_SQL = "SELECT NVL(MAX(" + Ope_Pre_Recep_Docs_Observ.Campo_No_Observacion + "),'0000000000') ";
            //Mi_SQL = Mi_SQL + "FROM " + Ope_Pre_Recep_Docs_Observ.Tabla_Ope_Pre_Recep_Docs_Observ;
            //Documento_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            //if (Convert.IsDBNull(Documento_ID))
            //{
            //    No_Comentario = "0000000000";
            //}
            //else
            //{
            //    No_Comentario = String.Format("{0:0000000000}", Convert.ToInt32(Documento_ID) + 1 );
            //}


            try
            {

                Mi_SQL = "UPDATE " + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs + " SET ";
                if (!String.IsNullOrEmpty(Datos.P_Cuenta_Predial_ID))
                    Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Cuenta_Predial_ID + " = '" + Datos.P_Cuenta_Predial_ID + "', ";
                if (!String.IsNullOrEmpty(Datos.P_Numero_Escritura))
                    Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Numero_Escritura + " = '" + Datos.P_Numero_Escritura + "', ";
                if (!String.IsNullOrEmpty(Datos.P_Fecha_Escritura))
                    Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Fecha_Escritura + " = '" + Convert.ToDateTime(Datos.P_Fecha_Escritura).ToString("dd/MM/yyyy") + "', ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Estatus + " = '" + Datos.P_Estatus_Movimiento + "', ";
                //observaciones
                //Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Observaciones + " = '" + Datos.P_Observaciones + "', ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL += " WHERE " + Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento + " = '" + Datos.P_No_Movimiento + "'";
                Comando_SQL.CommandText = Mi_SQL;
                Cnt_Filas_Afectadas += Comando_SQL.ExecuteNonQuery();

                //recorrer la lista Anexos_Eliminar y generar cada consulta de eliminar los anexos de la base de datos
                for (int i = 0; i < Anexos_Eliminar.Count; i++)
                {
                    Mi_SQL = "DELETE FROM  {0} WHERE {1} = '{2}'";
                    Mi_SQL = String.Format(Mi_SQL,
                        Ope_Pre_Recep_Docs_Anexos.Tabla_Ope_Pre_Recep_Docs_Anexos,          // 0
                        Ope_Pre_Recep_Docs_Anexos.Campo_No_Anexo,                           // 1
                        Anexos_Eliminar[i]                                                  // 2
                        );
                    Comando_SQL.CommandText = Mi_SQL;
                    Cnt_Filas_Afectadas += Comando_SQL.ExecuteNonQuery();
                }
                
                //recorrer la tabla Anexos_Alta y generar cada consulta de insercion de anexos
                foreach (DataRow Fila_Anexo_Alta in Anexos_Alta.Rows)
                {
                    No_Anexo = String.Format("{0:0000000000}", Convert.ToInt32(No_Anexo) + 1); //nuevo No de anexo
                    Mi_SQL = "INSERT INTO {0} ({1}, {3}, {5}, {7}, {9}, {11}) VALUES ('{2}', '{4}', '{6}', '{8}', '{10}', SYSDATE)";
                    Mi_SQL = String.Format(Mi_SQL,
                        Ope_Pre_Recep_Docs_Anexos.Tabla_Ope_Pre_Recep_Docs_Anexos,          // 0
                        Ope_Pre_Recep_Docs_Anexos.Campo_No_Anexo,                           // 1
                        No_Anexo,                                                           // 2
                        Ope_Pre_Recep_Docs_Anexos.Campo_No_Movimiento,                      // 3
                        Datos.P_No_Movimiento,                                              // 4
                        Ope_Pre_Recep_Docs_Anexos.Campo_Ruta,                               // 5
                        Fila_Anexo_Alta["NOMBRE_ARCHIVO"].ToString(),                       // 6
                        Ope_Pre_Recep_Docs_Anexos.Campo_Documento_ID,                       // 7
                        Fila_Anexo_Alta["CLAVE_DOCUMENTO"].ToString(),                      // 8
                        Ope_Pre_Recep_Docs_Anexos.Campo_Usuario_Creo,                       // 9
                        Datos.P_Nombre_Usuario,                                             // 10
                        Ope_Pre_Recep_Docs_Anexos.Campo_Fecha_Creo                          // 11
                        );
                    Comando_SQL.CommandText = Mi_SQL;
                    Cnt_Filas_Afectadas += Comando_SQL.ExecuteNonQuery();
                }

                //recorrer la tabla Anexos_Alta y generar cada consulta de insercion de anexos
                foreach (DataRow Fila_Anexo_Actualizar in Anexos_Actualizar.Rows)
                {
                    No_Anexo = String.Format("{0:0000000000}", Convert.ToInt32(No_Anexo) + 1); //nuevo No de anexo
                    Mi_SQL = "UPDATE {0} SET {1} = '{2}', {3} = '{4}', {5} = SYSDATE WHERE {6} = '{7}'";
                    Mi_SQL = String.Format(Mi_SQL,
                        Ope_Pre_Recep_Docs_Anexos.Tabla_Ope_Pre_Recep_Docs_Anexos,          // 0
                        Ope_Pre_Recep_Docs_Anexos.Campo_Ruta,                               // 1
                        Fila_Anexo_Actualizar["NOMBRE_ARCHIVO"].ToString(),                 // 2
                        Ope_Pre_Recep_Docs_Anexos.Campo_Usuario_Modifico,                   // 3
                        Datos.P_Nombre_Usuario,                                             // 4
                        Ope_Pre_Recep_Docs_Anexos.Campo_Fecha_Modifico,                     // 5
                        Ope_Pre_Recep_Docs_Anexos.Campo_No_Anexo,                           // 6
                        Fila_Anexo_Actualizar["NO_MOVIMIENTO"].ToString()                  // 7
                        );
                    Comando_SQL.CommandText = Mi_SQL;
                    Cnt_Filas_Afectadas += Comando_SQL.ExecuteNonQuery();
                }

                //Consulta para agregar una observacion
                //Mi_SQL = "INSERT INTO " + Ope_Pre_Recep_Docs_Observ.Tabla_Ope_Pre_Recep_Docs_Observ + " (";
                //Mi_SQL += Ope_Pre_Recep_Docs_Observ.Campo_No_Observacion + ", ";
                //Mi_SQL += Ope_Pre_Recep_Docs_Observ.Campo_No_Movimiento + ", ";
                //Mi_SQL += Ope_Pre_Recep_Docs_Observ.Campo_Observaciones + ", ";
                //Mi_SQL += Ope_Pre_Recep_Docs_Observ.Campo_Fecha + ", ";
                //Mi_SQL += Ope_Pre_Recep_Docs_Observ.Campo_Usuario_Creo + ", ";
                //Mi_SQL += Ope_Pre_Recep_Docs_Observ.Campo_Fecha_Creo + ") VALUES ('";
                //Mi_SQL += No_Comentario + "', '";
                //Mi_SQL += Datos.P_No_Movimiento + "', '";
                //Mi_SQL += Datos.P_Observaciones + "', ";
                //Mi_SQL += "SYSTIMESTAMP , '";
                //Mi_SQL += Datos.P_Nombre_Usuario + "', SYSDATE)";

                //Comando_SQL.CommandText = Mi_SQL;
                //Cnt_Filas_Afectadas += Comando_SQL.ExecuteNonQuery();

                Transaccion_SQL.Commit();

                return Cnt_Filas_Afectadas;
            }
            catch (OracleException Ex)
            {
                if (Transaccion_SQL != null)        // Si el envío de la consulta SQL es nulo, hacer rollback de los datos
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error Alta_Recepcion_Documentos: " + Ex.Message);
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

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Datos_Recepcion_Documentos
        /// 	DESCRIPCIÓN: Consulta todos los campos de las recepciones de documentos en la BD (sin los de auditoría)
        /// 	PARÁMETROS:
        /// 		1. Datos: Indica qué registro se desea consultar a la base de datos
        /// 	CREO: Roberto González Oseguera
        /// 	FECHA_CREO: 22-mar-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Datos_Recepcion_Documentos(Cls_Ope_Pre_Recepcion_Documentos_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL
            String Filtro_SQL = ""; //Variable para el filtro de la consulta SQL

            try
            {
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_Fecha + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_Clave_Tramite + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_Notario_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_Observaciones + ", ";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Recepcion_Documentos.Tabla_Ope_Pre_Recepcion_Documentos;
                if (Datos.P_No_Recepcion_Documento != null)      // Si se recibió un ID de documento filtrar por ese ID
                {
                    Filtro_SQL = " WHERE " + Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + " = '" + Datos.P_No_Recepcion_Documento + "'";
                }
                if (Datos.P_Notario_ID != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar OR y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " AND " + Ope_Pre_Recepcion_Documentos.Campo_Notario_ID + " = '" + Datos.P_Notario_ID + "'";
                    else
                        Filtro_SQL = " WHERE " + Ope_Pre_Recepcion_Documentos.Campo_Notario_ID + " = '" + Datos.P_Notario_ID + "'";
                }
                if (Datos.P_Clave_Tramite != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " AND " + Ope_Pre_Recepcion_Documentos.Campo_Clave_Tramite + " LIKE '" + Datos.P_Clave_Tramite + "'";
                    else
                        Filtro_SQL = " WHERE " + Ope_Pre_Recepcion_Documentos.Campo_Clave_Tramite + " LIKE '" + Datos.P_Clave_Tramite + "'";
                }
                if (Datos.P_Observaciones != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar OR y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " OR UPPER(" + Ope_Pre_Recepcion_Documentos.Campo_Observaciones + ") LIKE UPPER('%" + Datos.P_Observaciones + "%')";
                    else
                        Filtro_SQL = " WHERE UPPER(" + Ope_Pre_Recepcion_Documentos.Campo_Observaciones + ") LIKE UPPER('%" + Datos.P_Observaciones + "%')";
                }
                Mi_SQL = Mi_SQL + Filtro_SQL + " ORDER BY " + Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento;

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

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Recepcion_Documentos
        /// 	DESCRIPCIÓN: Consulta los campos No_Recepcion_Documento, Clave_Tramite y Notario_ID
        /// 	            de las recepciones de documentos en la BD
        /// 	PARÁMETROS:
        /// 		1. Datos: Indica qué registro se desea consultar a la base de datos
        /// 	CREO: Roberto González Oseguera
        /// 	FECHA_CREO: 22-mar-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Recepcion_Documentos(Cls_Ope_Pre_Recepcion_Documentos_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL
            String Filtro_SQL = ""; //Variable para el filtro de la consulta SQL

            try
            {
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_Clave_Tramite + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_Notario_ID + ", ";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Recepcion_Documentos.Tabla_Ope_Pre_Recepcion_Documentos;
                if (Datos.P_No_Recepcion_Documento != null)      // Si se recibió un ID de documento filtrar por ese ID
                {
                    Filtro_SQL = " WHERE " + Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + " = '" + Datos.P_No_Recepcion_Documento + "'";
                }
                if (Datos.P_Notario_ID != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar OR y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " AND " + Ope_Pre_Recepcion_Documentos.Campo_Notario_ID + " = '" + Datos.P_Notario_ID + "'";
                    else
                        Filtro_SQL = " WHERE " + Ope_Pre_Recepcion_Documentos.Campo_Notario_ID + " = '" + Datos.P_Notario_ID + "'";
                }
                if (Datos.P_Clave_Tramite != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " AND " + Ope_Pre_Recepcion_Documentos.Campo_Clave_Tramite + " LIKE '" + Datos.P_Clave_Tramite + "'";
                    else
                        Filtro_SQL = " WHERE " + Ope_Pre_Recepcion_Documentos.Campo_Clave_Tramite + " LIKE '" + Datos.P_Clave_Tramite + "'";
                }
                if (Datos.P_Observaciones != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar OR y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " OR UPPER(" + Ope_Pre_Recepcion_Documentos.Campo_Observaciones + ") LIKE UPPER('%" + Datos.P_Observaciones + "%')";
                    else
                        Filtro_SQL = " WHERE UPPER(" + Ope_Pre_Recepcion_Documentos.Campo_Observaciones + ") LIKE UPPER('%" + Datos.P_Observaciones + "%')";
                }
                Mi_SQL = Mi_SQL + Filtro_SQL + " ORDER BY " + Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento;

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

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Recepciones_Movimientos
        /// 	DESCRIPCIÓN: Consulta los movimientos (conteo) de una recepcion por notario
        /// 	PARÁMETROS:
        /// 		1. Datos: Indica qué registro se desea consultar a la base de datos
        /// 	CREO: Roberto González Oseguera
        /// 	FECHA_CREO: 22-mar-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Recepciones_Movimientos(Cls_Ope_Pre_Recepcion_Documentos_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL

            try
            {
                Mi_SQL = "SELECT UNIQUE(";
                Mi_SQL += "Recepcion." + Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + "), ";
                Mi_SQL += "Recepcion." + Ope_Pre_Recepcion_Documentos.Campo_Fecha + ", ";
                Mi_SQL += "Recepcion." + Ope_Pre_Recepcion_Documentos.Campo_Observaciones + ", ";
                Mi_SQL += Cat_Pre_Notarios.Tabla_Cat_Pre_Notarios + "." + Cat_Pre_Notarios.Campo_Apellido_Paterno + " || ' ' || ";
                Mi_SQL += Cat_Pre_Notarios.Tabla_Cat_Pre_Notarios + "." + Cat_Pre_Notarios.Campo_Apellido_Materno + " || ' ' || ";
                Mi_SQL += Cat_Pre_Notarios.Tabla_Cat_Pre_Notarios + "." + Cat_Pre_Notarios.Campo_Nombre + " AS NOMBRE_NOTARIO, ";
                Mi_SQL += "( SELECT COUNT(*) FROM ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs + " Movs WHERE Movs.";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + " = Recepcion.";
                Mi_SQL += Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento;
                Mi_SQL += ") AS TOTAL_MOVIMIENTOS, ";
                Mi_SQL += "( SELECT COUNT(*) FROM ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs + " Movs WHERE Movs.";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + " = Recepcion.";
                Mi_SQL += Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + " AND Movs.";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Estatus + " LIKE 'PENDIENTE'";
                Mi_SQL += ") AS TOTAL_PENDIENTES, ";
                Mi_SQL += "( SELECT COUNT(*) FROM ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs + " Movs WHERE Movs.";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + " = Recepcion.";
                Mi_SQL += Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + " AND Movs.";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Estatus + " LIKE 'RECHAZADO'";
                Mi_SQL += ") AS TOTAL_RECHAZADOS ";
                if (!String.IsNullOrEmpty(Datos.P_No_Recepcion_Documento))  // si hay un numero de recepciona de documentos, incluir id del notario
                {
                    Mi_SQL += ", Recepcion.";
                    Mi_SQL += Ope_Pre_Recepcion_Documentos.Campo_Notario_ID;
                }
                Mi_SQL += " FROM " + Ope_Pre_Recepcion_Documentos.Tabla_Ope_Pre_Recepcion_Documentos + " Recepcion, ";
                Mi_SQL += Cat_Pre_Notarios.Tabla_Cat_Pre_Notarios + ", ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs;
                Mi_SQL += " WHERE ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs + ".";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + " = Recepcion.";
                Mi_SQL += Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + " AND (";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs + ".";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Estatus + " LIKE 'PENDIENTE' OR ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs + ".";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Estatus + " LIKE 'RECHAZADA') AND ";
                Mi_SQL += " Recepcion." + Ope_Pre_Recepcion_Documentos.Campo_Notario_ID + " = ";
                Mi_SQL += Cat_Pre_Notarios.Tabla_Cat_Pre_Notarios + ".";
                Mi_SQL += Cat_Pre_Notarios.Campo_Notario_ID;

                if (Datos.P_Notario_ID != null)
                {
                    Mi_SQL += " AND Recepcion." + Ope_Pre_Recepcion_Documentos.Campo_Notario_ID + " = '" + Datos.P_Notario_ID + "'";
                }
                else if (!String.IsNullOrEmpty(Datos.P_No_Recepcion_Documento))
                {
                    Mi_SQL += " AND Recepcion." + Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + " = '" + Datos.P_No_Recepcion_Documento + "'";
                }

                Mi_SQL += " ORDER BY Recepcion." + Ope_Pre_Recepcion_Documentos.Campo_Fecha + " DESC";

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


        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Busqueda_Recepciones_Movimientos
        /// 	DESCRIPCIÓN: Consulta los movimientos (conteo) de una recepcion por notario
        /// 	PARÁMETROS:
        /// 		1. Datos: Indica qué registro se desea consultar a la base de datos
        /// 	CREO: Jesus Toledo Rodriguez
        /// 	FECHA_CREO: 23-jul-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
             public static DataTable Busqueda_Recepciones_Movimientos(Cls_Ope_Pre_Recepcion_Documentos_Negocio Datos)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL

            try
            {
                Mi_SQL = "SELECT";
                Mi_SQL += " UNIQUE(REC." + Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + ") AS NO_RECEPCION_DOCUMENTO, ";
                Mi_SQL += " REC." + Ope_Pre_Recepcion_Documentos.Campo_Notario_ID + " AS NOTARIO_ID, ";
                Mi_SQL += " REC." + Ope_Pre_Recepcion_Documentos.Campo_Fecha + " AS FECHA, ";
                //Mi_SQL += " MOV." + Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento + " AS NO_MOVIMIENTO, ";
                //Mi_SQL += " MOV." + Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + " AS NO_RECEPCION_DOCUMENTO, ";
                //Mi_SQL += " MOV." + Ope_Pre_Recep_Docs_Movs.Campo_Numero_Escritura + " AS NUMERO_ESCRITURA, ";
                //Mi_SQL += " MOV." + Ope_Pre_Recep_Docs_Movs.Campo_Cuenta_Predial_ID + " AS CUENTA_PREDIAL_ID, ";
                //Mi_SQL += " MOV." + Ope_Pre_Recep_Docs_Movs.Campo_Estatus + " AS ESTATUS, ";
                Mi_SQL += " NOTA." + Cat_Pre_Notarios.Campo_Notario_ID + " AS NOTARIO_ID, ";
                Mi_SQL += " NOTA." + Cat_Pre_Notarios.Campo_RFC + " AS RFC, ";
                Mi_SQL += " NOTA." + Cat_Pre_Notarios.Campo_Numero_Notaria + " AS NUMERO_NOTARIA, ";
                Mi_SQL += " NOTA." + Cat_Pre_Notarios.Campo_Apellido_Paterno + " ||' '|| ";
                Mi_SQL += " NOTA." + Cat_Pre_Notarios.Campo_Apellido_Materno + " ||' '|| ";
                Mi_SQL += " NOTA." + Cat_Pre_Notarios.Campo_Nombre + " AS NOMBRE_NOTARIO, ";
                //Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " AS CUENTA_PREDIAL_ID, ";
                //Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " AS CUENTA_PREDIAL, ";
                Mi_SQL += " (SELECT COUNT(*) FROM " + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs;
                Mi_SQL += " WHERE " + Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + " = ";
                Mi_SQL += "REC." + Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + ") AS TOTAL_MOVIMIENTOS, ";
                Mi_SQL += " (SELECT COUNT(*) FROM " + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs;
                Mi_SQL += " WHERE " + Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + " = ";
                Mi_SQL += "REC." + Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + " AND ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Estatus + " LIKE 'PENDIENTE') AS TOTAL_PENDIENTES, ";
                Mi_SQL += " (SELECT COUNT(*) FROM " + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs;
                Mi_SQL += " WHERE " + Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + " = ";
                Mi_SQL += "REC." + Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + " AND ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Estatus + " LIKE 'RECHAZADO') AS TOTAL_RECHAZADOS, ";
                Mi_SQL += " (SELECT COUNT(*) FROM " + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs;
                Mi_SQL += " WHERE " + Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + " = ";
                Mi_SQL += "REC." + Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + " AND ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Estatus + " LIKE 'ACEPTADO') AS TOTAL_ACEPTADOS, ";
                Mi_SQL += " (SELECT COUNT(*) FROM " + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs;
                Mi_SQL += " WHERE " + Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + " = ";
                Mi_SQL += "REC." + Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + " AND ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Estatus + " LIKE 'CANCELADO') AS TOTAL_CANCELADOS ";

                Mi_SQL += " FROM ";

                Mi_SQL += Ope_Pre_Recepcion_Documentos.Tabla_Ope_Pre_Recepcion_Documentos + " REC LEFT OUTER JOIN " + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs + " MOV ";
                Mi_SQL += " ON MOV." + Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + " = REC." + Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento;
                Mi_SQL += " LEFT OUTER JOIN " + Cat_Pre_Notarios.Tabla_Cat_Pre_Notarios + " NOTA ON NOTA." + Cat_Pre_Notarios.Campo_Notario_ID + " = ";
                Mi_SQL += " REC." + Ope_Pre_Recepcion_Documentos.Campo_Notario_ID;
                // solo incluir el join de Cat_Pre_Cuentas_Predial para filtrar por cuenta
                if (!String.IsNullOrEmpty(Datos.P_Cuenta_Predial))
                {
                    Mi_SQL += " LEFT OUTER JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CUEN ON CUEN.";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = MOV." + Ope_Pre_Recep_Docs_Movs.Campo_Cuenta_Predial_ID;
                    Mi_SQL += " WHERE ROWNUM=1 AND (MOV.";
                }
                else
                {
                    Mi_SQL += " WHERE (MOV.";
                }
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Estatus + " LIKE('PENDIENTE') OR MOV.";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Estatus + " LIKE('RECHAZADO') ) ";

                if (!String.IsNullOrEmpty(Datos.P_No_Recepcion_Documento))  // si hay un numero de recepciona de documentos, incluir id del notario
                {
                    Mi_SQL += " AND REC.";
                    Mi_SQL += Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + " ='";
                    Mi_SQL += Datos.P_No_Recepcion_Documento + "'";
                }

                if (!String.IsNullOrEmpty(Datos.P_Notario_ID))
                {
                    Mi_SQL += " AND NOTA.";
                    Mi_SQL += Cat_Pre_Notarios.Campo_Notario_ID;
                    Mi_SQL += " LIKE UPPER('%" + Datos.P_Notario_ID + "%')";
                    //Mi_SQL += Datos.P_Notario_ID + "%'";
                }

                if (!String.IsNullOrEmpty(Datos.P_Nombre_Notario))
                {
                    Mi_SQL += " AND (UPPER(NOTA." + Cat_Pre_Notarios.Campo_Nombre + ") LIKE UPPER('%" + Datos.P_Nombre_Notario + "%')";
                    Mi_SQL += " OR UPPER(NOTA." + Cat_Pre_Notarios.Campo_Apellido_Paterno + ") LIKE UPPER('%" + Datos.P_Nombre_Notario + "%')";
                    Mi_SQL += " OR UPPER(NOTA." + Cat_Pre_Notarios.Campo_Apellido_Materno + ") LIKE UPPER('%" + Datos.P_Nombre_Notario + "%')";
                    Mi_SQL += " OR UPPER(NOTA." + Cat_Pre_Notarios.Campo_Apellido_Paterno + ") ||' '|| ";
                    Mi_SQL += " UPPER(NOTA." + Cat_Pre_Notarios.Campo_Apellido_Materno + ") ||' '|| ";
                    Mi_SQL += " UPPER(NOTA." + Cat_Pre_Notarios.Campo_Nombre + ") LIKE UPPER('%" + Datos.P_Nombre_Notario + "%'))";
                }

                if (!String.IsNullOrEmpty(Datos.P_Numero_Notaria))
                {
                    Mi_SQL += " AND NOTA.";
                    Mi_SQL += Cat_Pre_Notarios.Campo_Numero_Notaria;
                    Mi_SQL += " LIKE '%" + Datos.P_Numero_Notaria + "%'";
                    //Mi_SQL += Datos.P_Numero_Notaria + "%'";
                }
                if (!String.IsNullOrEmpty(Datos.P_Cuenta_Predial))
                {
                    Mi_SQL += " AND CUEN.";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " ='";
                    Mi_SQL += Datos.P_Cuenta_Predial + "'";
                }
                if (!String.IsNullOrEmpty(Datos.P_Numero_Escritura))
                {
                    Mi_SQL += " AND MOV.";
                    Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Numero_Escritura + " LIKE '%" + Datos.P_Numero_Escritura + "%'"; ;
                    //Mi_SQL += Datos.P_Numero_Escritura + "%'";
                }

                Mi_SQL += " ORDER BY REC." + Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + " DESC";

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

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Detalles_Movimientos_Recepcion
        /// 	DESCRIPCIÓN: Consulta detalles de los movimientos de una recepcion
        /// 	PARÁMETROS:
        /// 		1. Numero_Recepcion: ID de la recepcion de documentos a consultar
        /// 	CREO: Roberto González Oseguera
        /// 	FECHA_CREO: 13-may-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Detalles_Movimientos_Recepcion(String Numero_Recepcion)
        {
            String Mi_SQL; //Variable para la consulta SQL

            try
            {
                Mi_SQL = "WITH Anexos AS (SELECT ";
                Mi_SQL += Ope_Pre_Recep_Docs_Anexos.Campo_No_Movimiento + ", ";
                Mi_SQL += Ope_Pre_Recep_Docs_Anexos.Campo_Documento_ID + ", row_number() over (partition by ";
                Mi_SQL += Ope_Pre_Recep_Docs_Anexos.Campo_No_Movimiento + " ORDER BY ";
                Mi_SQL += Ope_Pre_Recep_Docs_Anexos.Campo_No_Anexo + " ) rn, count(*) over (partition by ";
                Mi_SQL += Ope_Pre_Recep_Docs_Anexos.Campo_No_Movimiento + " ) cnt FROM  ";
                Mi_SQL += Ope_Pre_Recep_Docs_Anexos.Tabla_Ope_Pre_Recep_Docs_Anexos + ") ";
                Mi_SQL += " SELECT Anexos." + Ope_Pre_Recep_Docs_Anexos.Campo_No_Movimiento + ", ltrim(sys_connect_by_path(";
                Mi_SQL += Cat_Pre_Tipos_Documento.Campo_Nombre_Documento + ",', '),',') Documentos, ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs + ".";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Estatus;
                // subconsulta para agregar numero de cuenta predial
                Mi_SQL += ", ( SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial;
                Mi_SQL += " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas;
                Mi_SQL += " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                Mi_SQL += " = " + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs + "."
                    + Ope_Pre_Recep_Docs_Movs.Campo_Cuenta_Predial_ID;
                Mi_SQL += ") CUENTA_PREDIAL ";
                Mi_SQL += " FROM Anexos, " + Cat_Pre_Tipos_Documento.Tabla_Cat_Pre_Tipos_Documento + ", ";
                Mi_SQL += Ope_Pre_Recepcion_Documentos.Tabla_Ope_Pre_Recepcion_Documentos + ", ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs;
                Mi_SQL += " WHERE ";
                Mi_SQL += Ope_Pre_Recepcion_Documentos.Tabla_Ope_Pre_Recepcion_Documentos + ".";
                Mi_SQL += Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + " = '";
                Mi_SQL += Numero_Recepcion + "' AND ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs + ".";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + " = ";
                Mi_SQL += Ope_Pre_Recepcion_Documentos.Tabla_Ope_Pre_Recepcion_Documentos + ".";
                Mi_SQL += Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + " AND ";
                Mi_SQL += "Anexos." + Ope_Pre_Recep_Docs_Anexos.Campo_No_Movimiento + " = ";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs + ".";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento + " AND ";
                Mi_SQL += Cat_Pre_Tipos_Documento.Tabla_Cat_Pre_Tipos_Documento + ".";
                Mi_SQL += Cat_Pre_Tipos_Documento.Campo_Documento_ID + " = ";
                Mi_SQL += "Anexos." + Ope_Pre_Recep_Docs_Anexos.Campo_Documento_ID;
                Mi_SQL += " AND rn = cnt start with rn = 1 connect by prior ";
                Mi_SQL += "Anexos." + Ope_Pre_Recep_Docs_Anexos.Campo_No_Movimiento + " = ";
                Mi_SQL += "Anexos." + Ope_Pre_Recep_Docs_Anexos.Campo_No_Movimiento + " AND prior rn = rn-1 ";
                Mi_SQL += " ORDER BY Anexos." + Ope_Pre_Recep_Docs_Anexos.Campo_No_Movimiento;

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

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Datos_Movimiento
        /// 	DESCRIPCIÓN: Consulta los datos relacionados con un movimiento
        /// 	PARÁMETROS:
        /// 		1. Datos: no de movimiento a consultar
        /// 	CREO: Roberto González Oseguera
        /// 	FECHA_CREO: 13-may-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Datos_Movimiento(Cls_Ope_Pre_Recepcion_Documentos_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL

            try
            {
                Mi_SQL = "SELECT Movs." + Ope_Pre_Recep_Docs_Movs.Campo_Numero_Escritura;
                Mi_SQL += ", Movs." + Ope_Pre_Recep_Docs_Movs.Campo_Fecha_Escritura;
                //Mi_SQL += ", Movs." + Ope_Pre_Recep_Docs_Movs.Campo_Observaciones;
                Mi_SQL += ", (SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " FROM ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " Cta WHERE Cta.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = Movs.";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Cuenta_Predial_ID + ") AS CUENTA_PREDIAL";
                Mi_SQL += ", Movs." + Ope_Pre_Recep_Docs_Movs.Campo_Cuenta_Predial_ID;
                Mi_SQL += " FROM " + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs + " Movs ";

                if (Datos.P_No_Movimiento != null)      // Si se recibió un numero de movimiento filtrar
                {
                    Mi_SQL += " WHERE Movs." + Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento + " = '" + Datos.P_No_Movimiento + "'";
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

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Anexos_Movimiento
        /// 	DESCRIPCIÓN: Consulta los anexos relacionados con un movimiento
        /// 	PARÁMETROS:
        /// 		1. Datos: no de movimiento a consultar
        /// 	CREO: Roberto González Oseguera
        /// 	FECHA_CREO: 13-may-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Anexos_Movimiento(Cls_Ope_Pre_Recepcion_Documentos_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL
            String Filtro_SQL = ""; //Variable para el filtro de la consulta SQL

            try
            {
                Mi_SQL = "SELECT ";
                Mi_SQL += Ope_Pre_Recep_Docs_Anexos.Campo_No_Anexo + ", ";
                Mi_SQL += Ope_Pre_Recep_Docs_Anexos.Campo_Ruta + ", ";
                Mi_SQL += Ope_Pre_Recep_Docs_Anexos.Campo_Documento_ID;
                Mi_SQL += " FROM " + Ope_Pre_Recep_Docs_Anexos.Tabla_Ope_Pre_Recep_Docs_Anexos;

                if (Datos.P_No_Movimiento != null)      // Si se recibió un numero de movimiento filtrar
                {
                    Filtro_SQL = " WHERE " + Ope_Pre_Recep_Docs_Anexos.Campo_No_Movimiento + " = '" + Datos.P_No_Movimiento + "'";
                }

                Mi_SQL = Mi_SQL + Filtro_SQL + " ORDER BY " + Ope_Pre_Recep_Docs_Anexos.Campo_No_Anexo;

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

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Observaciones_Movimiento
        /// 	DESCRIPCIÓN: Consulta las observaciones relacionados con un movimiento
        /// 	PARÁMETROS:
        /// 		1. Datos: no de movimiento a consultar
        /// 	CREO: Roberto González Oseguera
        /// 	FECHA_CREO: 13-may-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Observaciones_Movimiento(Cls_Ope_Pre_Recepcion_Documentos_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL
            String Filtro_SQL = ""; //Variable para el filtro de la consulta SQL

            try
            {
                Mi_SQL = "SELECT ";
                Mi_SQL += Ope_Pre_Recep_Docs_Observ.Campo_Observaciones + ", TO_CHAR(";
                Mi_SQL += Ope_Pre_Recep_Docs_Observ.Campo_Fecha + ", 'DD/Mon/YYYY HH:MI:SS PM') AS " +
                    Ope_Pre_Recep_Docs_Observ.Campo_Fecha + ", ";
                Mi_SQL += Ope_Pre_Recep_Docs_Observ.Campo_Usuario_Creo;
                Mi_SQL += " FROM " + Ope_Pre_Recep_Docs_Observ.Tabla_Ope_Pre_Recep_Docs_Observ;

                if (Datos.P_No_Movimiento != null)      // Si se recibió un numero de movimiento filtrar
                {
                    Filtro_SQL = " WHERE " + Ope_Pre_Recep_Docs_Observ.Campo_No_Movimiento + " = '" + Datos.P_No_Movimiento + "'";
                }

                Mi_SQL = Mi_SQL + Filtro_SQL + " ORDER BY " + Ope_Pre_Recep_Docs_Observ.Campo_Fecha + " DESC";

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
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Movimiento
        ///DESCRIPCIÓN: Elimina un movimiento de la tabla OPE_PRE_RECEP_DOCS_MOVS
        ///PARAMETROS:   
        ///             1. Movimiento.   Movimiento que se va eliminar.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 06/Septiembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Movimiento(Cls_Ope_Pre_Recepcion_Documentos_Negocio Movimiento)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try
            {
                String Mi_SQL = "DELETE";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Recep_Docs_Anexos.Tabla_Ope_Pre_Recep_Docs_Anexos;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Recep_Docs_Anexos.Campo_No_Movimiento + " = '" + Movimiento.P_No_Movimiento + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                Mi_SQL = "DELETE";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento + " = '" + Movimiento.P_No_Movimiento + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                Trans.Commit();
            }
            
            catch (OracleException Ex)
            {
                Trans.Rollback();
                if (Ex.Code == 8152)
                {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                }
                else if (Ex.Code == 2627)
                {
                    if (Ex.Message.IndexOf("PRIMARY") != -1)
                    {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                    }
                    else if (Ex.Message.IndexOf("UNIQUE") != -1)
                    {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]";
                    }
                    else
                    {
                        Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]";
                    }
                }
                else if (Ex.Code == 547)
                {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]";
                }
                else if (Ex.Code == 515)
                {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                }
                else
                {
                    Mensaje = "Error al intentar elimnar un Registro de Movimientos. Error: [" + Ex.Message + "]";
                }
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }

        }


        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Notarios
        /// 	DESCRIPCIÓN: Consulta los Notarios registrados en la BD
        /// 	PARÁMETROS:
        /// 		1. Datos: Indica qué registro se desea consultar a la base de datos
        /// 	CREO: Roberto González Oseguera
        /// 	FECHA_CREO: 25-mar-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Notarios(Cls_Ope_Pre_Recepcion_Documentos_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL
            String Filtro_SQL = ""; //Variable para el filtro de la consulta SQL

            try
            {
                Mi_SQL = "SELECT " + Cat_Pre_Notarios.Campo_Notario_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Apellido_Paterno + " || ' ' || ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Apellido_Materno + " || ' ' || ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Nombre + " AS NOMBRE_COMPLETO, ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Ciudad + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_RFC + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Numero_Notaria + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_E_Mail;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Notarios.Tabla_Cat_Pre_Notarios;
                if (Datos.P_Notario_ID != null)      // Si se recibió un ID de documento filtrar por ese ID
                {
                    Filtro_SQL = " WHERE " + Cat_Pre_Notarios.Campo_Notario_ID + " = '" + Datos.P_Notario_ID + "'";
                }
                if (Datos.P_Nombre_Notario != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                    {
                        Filtro_SQL = Filtro_SQL + " AND UPPER(" + Cat_Pre_Notarios.Campo_Apellido_Paterno + " || ' ' || ";
                        Filtro_SQL = Filtro_SQL + Cat_Pre_Notarios.Campo_Apellido_Materno + " || ' ' || ";
                        Filtro_SQL = Filtro_SQL + Cat_Pre_Notarios.Campo_Nombre + ") LIKE UPPER('%" + Datos.P_Nombre_Notario + "%')";
                    }
                    else
                    {
                        Filtro_SQL = " WHERE UPPER(" + Cat_Pre_Notarios.Campo_Apellido_Paterno + " || ' ' || ";
                        Filtro_SQL = Filtro_SQL + Cat_Pre_Notarios.Campo_Apellido_Materno + " || ' ' || ";
                        Filtro_SQL = Filtro_SQL + Cat_Pre_Notarios.Campo_Nombre + ") LIKE UPPER('%" + Datos.P_Nombre_Notario + "%')";
                    }
                }
                if (Datos.P_Estatus_Notario != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " AND " + Cat_Pre_Notarios.Campo_Estatus + " = '" + Datos.P_Estatus_Notario + "'";
                    else
                        Filtro_SQL = " WHERE " + Cat_Pre_Notarios.Campo_Estatus + " = '" + Datos.P_Estatus_Notario + "'";
                }
                if (Datos.P_RFC_Notario != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar OR y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " AND UPPER(" + Cat_Pre_Notarios.Campo_RFC + ") LIKE UPPER('%" + Datos.P_RFC_Notario + "%')";
                    else
                        Filtro_SQL = " WHERE UPPER(" + Cat_Pre_Notarios.Campo_RFC + ") LIKE UPPER('%" + Datos.P_RFC_Notario + "%')";
                }
                if (Datos.P_Numero_Notaria != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar OR y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL = Filtro_SQL + " AND TO_NUMBER(" + Cat_Pre_Notarios.Campo_Numero_Notaria + ") = '" + Datos.P_Numero_Notaria + "'";
                    else
                        Filtro_SQL = " WHERE TO_NUMBER(" + Cat_Pre_Notarios.Campo_Numero_Notaria + ") = '" + Datos.P_Numero_Notaria + "'";
                }
                Mi_SQL = Mi_SQL + Filtro_SQL + " ORDER BY " + Cat_Pre_Notarios.Campo_Apellido_Paterno + ", " + Cat_Pre_Notarios.Campo_Apellido_Materno;

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

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Cuentas_Predial
        /// 	DESCRIPCIÓN: Consulta las Cuentas de Predial registradas en la BD (consultando al mismo tiempo 
        /// 	            la calle y el propietario de la cuenta)
        /// 	PARÁMETROS:
        /// 		1. Datos: Indica qué registro se desea consultar en la base de datos
        /// 	CREO: Roberto González Oseguera
        /// 	FECHA_CREO: 25-mar-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Cuentas_Predial(Cls_Ope_Pre_Recepcion_Documentos_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL

            try
            {
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Suspencion + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Interior + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + "." + Cat_Pre_Calles.Campo_Nombre + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + "." + Cat_Pre_Calles.Campo_Nombre + " || ' ' || ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + " || ' ' || ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Interior + " AS DOMICILIO, ";
                Mi_SQL = Mi_SQL + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Propietario_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Tipo + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || ";
                Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || ";
                Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Nombre + " AS NOMBRE_PROPIETARIO";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " LEFT OUTER JOIN ";

                Mi_SQL = Mi_SQL + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " ON " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + "." + Cat_Pre_Calles.Campo_Calle_ID + " = ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + " LEFT OUTER JOIN ";

                Mi_SQL = Mi_SQL + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " ON ";
                Mi_SQL = Mi_SQL + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " = ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " LEFT OUTER JOIN ";

                Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " ON ";
                Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = ";
                Mi_SQL = Mi_SQL + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Contribuyente_ID;


                //Mi_SQL = Mi_SQL + " WHERE ";
                //Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + " = ";
                //Mi_SQL = Mi_SQL + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + "." + Cat_Pre_Calles.Campo_Calle_ID;
                //Mi_SQL = Mi_SQL + " AND ";
                //Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = ";
                //Mi_SQL = Mi_SQL + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID;
                //Mi_SQL = Mi_SQL + " AND ";
                //Mi_SQL = Mi_SQL + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " = ";
                //Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID;

                //if (Datos.P_Cuenta_Predial_ID != null)      // Si se recibió un ID de documento filtrar por ese ID
                //{
                //    Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + 
                //        Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = '" + Datos.P_Cuenta_Predial_ID + "'";
                //}
                if (Datos.P_Cuenta_Predial != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." +
                        Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " = '" + Datos.P_Cuenta_Predial + "'";

                    if (Datos.P_Calle_ID != null)
                    {
                        Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." +
                            Cat_Pre_Cuentas_Predial.Campo_Calle_ID + " = '" + Datos.P_Calle_ID + "'";
                    }
                    if (Datos.P_Estatus_Cuenta_Predial != null)
                    {
                        Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." +
                            Cat_Pre_Cuentas_Predial.Campo_Estatus + " = '" + Datos.P_Estatus_Cuenta_Predial + "'";
                    }
                    if (Datos.P_Tipo_Propietario != null) //Buscar por ID de propietatio
                    {
                        Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." +
                            Cat_Pre_Propietarios.Campo_Tipo + " = '" + Datos.P_Tipo_Propietario + "'";
                    }
                    if (Datos.P_Propietario_ID != null) //Buscar por ID de propietatio
                    {
                        Mi_SQL = Mi_SQL + " AND UPPER(" + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." +
                            Cat_Pre_Propietarios.Campo_Propietario_ID + ") LIKE UPPER('%" + Datos.P_Propietario_ID + "%')";
                    }
                    if (Datos.P_Nombre_Propietario != null) //Buscar por ID de propietatio
                    {
                        Mi_SQL = Mi_SQL + " AND UPPER(" + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || ";
                        Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || ";
                        Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Nombre
                            + ") LIKE UPPER ('%" + Datos.P_Nombre_Propietario + "%')";
                    }
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial;

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
        internal static DataSet Consulta_Reporte_Folio_Impresion(Cls_Ope_Pre_Recepcion_Documentos_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL
            DataSet Ds_Resultado = new DataSet(); //Variable para almacenar las tablas resultantes
            String Documentos = "";

            try
            {
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + "REC." + Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + " ||'/'|| " + "REC." + Ope_Pre_Recepcion_Documentos.Campo_Anio + " AS FOLIO, TO_CHAR(REC.";
                Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_Fecha + ", 'DD/Mon/YYYY HH:MI:SS PM') AS FECHA, REC.";
                
                Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_Usuario_Creo + ", REC.";
                Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_Notario_ID + ", NOTA.";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Nombre + " ||' '|| ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Apellido_Paterno + " ||' '|| ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Apellido_Materno + " AS NOMBRE_NOTARIO, NOTA.";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_RFC + " AS RFC, NOTA.";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Numero_Notaria + " AS NO_NOTARIA";

                Mi_SQL = Mi_SQL + " FROM ";

                Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Tabla_Ope_Pre_Recepcion_Documentos + " REC JOIN ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Tabla_Cat_Pre_Notarios + " NOTA ON ";
                Mi_SQL = Mi_SQL + "NOTA." + Cat_Pre_Notarios.Campo_Notario_ID + " = REC." + Ope_Pre_Recepcion_Documentos.Campo_Notario_ID;

                Mi_SQL = Mi_SQL + " WHERE ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + " = '";
                Mi_SQL = Mi_SQL + Datos.P_No_Recepcion_Documento + "'";

                Ds_Resultado.Tables.Add(OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Copy());
                 Ds_Resultado.Tables[0].TableName = "Recepcion_Documentos";

                Mi_SQL = "";
                Mi_SQL = "SELECT ";
                Mi_SQL += " MOV." + Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + " ||'/'|| ";
                Mi_SQL += " MOV." + Ope_Pre_Recep_Docs_Movs.Campo_Anio + " AS FOLIO, ";
                Mi_SQL += " MOV." + Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento + " AS NO_MOVIMIENTO, ";
                Mi_SQL += " MOV." + Ope_Pre_Recep_Docs_Movs.Campo_Numero_Escritura + " AS NUMERO_ESCRITURA, TO_CHAR(";
                Mi_SQL += " MOV." + Ope_Pre_Recep_Docs_Movs.Campo_Fecha_Escritura + ", 'DD/Mon/YYYY') AS FECHA_ESCRITURA, ";
                Mi_SQL += " MOV." + Ope_Pre_Recep_Docs_Movs.Campo_Cuenta_Predial_ID + ", ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " AS CUENTA_PREDIAL, ";
                Mi_SQL += " NULL AS NOMBRE_DOCUMENTO , ";
                Mi_SQL += "(SELECT " + Ope_Pre_Historial_Obs_Recep.Campo_Observaciones + " FROM " + Ope_Pre_Historial_Obs_Recep.Tabla_Ope_Pre_Historial_Obs_Recep;
                Mi_SQL += " WHERE " + Ope_Pre_Historial_Obs_Recep.Campo_No_Observacion_ID + " IN (SELECT MAX (" + Ope_Pre_Historial_Obs_Recep.Campo_No_Observacion_ID + ") ";
                Mi_SQL += "  FROM " + Ope_Pre_Historial_Obs_Recep.Tabla_Ope_Pre_Historial_Obs_Recep;
                Mi_SQL += " WHERE " + Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento + "=";
                Mi_SQL += "MOV." + Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento + ")) ";
                Mi_SQL += "AS OBSERVACIONES FROM " + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs + " MOV LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CUEN ON CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                Mi_SQL += " = MOV." + Ope_Pre_Recep_Docs_Movs.Campo_Cuenta_Predial_ID + " WHERE ";
                Mi_SQL += " MOV." + Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + " ='" + Datos.P_No_Recepcion_Documento + "'";

               //SELECT  MOV.NO_RECEPCION_DOCUMENTO AS NO_RECEPCION_DOCUMENTO,  MOV.NO_MOVIMIENTO AS NO_MOVIMIENTO,  MOV.NUMERO_ESCRITURA AS NUMERO_ESCRITURA, TO_CHAR( MOV.FECHA_ESCRITURA, 'DD/Mon/YYYY') AS FECHA_ESCRITURA,  MOV.CUENTA_PREDIAL_ID,  CUEN.CUENTA_PREDIAL AS CUENTA_PREDIAL,  NULL AS NOMBRE_DOCUMENTO, (SELECT OBSERVACIONES FROM OPE_PRE_HISTORIAL_OBS_RECEP WHERE NO_OBSERVACION_ID IN (SELECT MAX (NO_OBSERVACION_ID)  FROM OPE_PRE_HISTORIAL_OBS_RECEP WHERE NO_MOVIMIENTO= MOV.NO_MOVIMIENTO))  FROM OPE_PRE_RECEP_DOCS_MOVS MOV LEFT OUTER JOIN CAT_PRE_CUENTAS_PREDIAL CUEN ON CUEN.CUENTA_PREDIAL_ID = MOV.CUENTA_PREDIAL_ID WHERE  MOV.NO_RECEPCION_DOCUMENTO ='0000000101' 


                
                Ds_Resultado.Tables.Add(OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Copy());
                Ds_Resultado.Tables[1].TableName = "Dt_Detalles";


                foreach (DataRow renglon in Ds_Resultado.Tables["Dt_Detalles"].Rows)
                {
                    DataTable Dt_Resultado;
                    Mi_SQL = "";
                    Mi_SQL = "SELECT ";
                    Mi_SQL += " ANX." + Ope_Pre_Recep_Docs_Anexos.Campo_Documento_ID + " AS DOCUMENTO_ID, ANX.";
                    Mi_SQL += Ope_Pre_Recep_Docs_Anexos.Campo_No_Movimiento + ", DOCS.";
                    Mi_SQL += Cat_Pre_Tipos_Documento.Campo_Nombre_Documento + " AS NOMBRE_ANEXO";

                    

                    Mi_SQL += " FROM " + Ope_Pre_Recep_Docs_Anexos.Tabla_Ope_Pre_Recep_Docs_Anexos + " ANX LEFT OUTER JOIN " + Cat_Pre_Tipos_Documento.Tabla_Cat_Pre_Tipos_Documento + " DOCS ON DOCS.";
                    Mi_SQL += Cat_Pre_Tipos_Documento.Campo_Documento_ID + " = ANX." + Ope_Pre_Recep_Docs_Anexos.Campo_Documento_ID + " JOIN ";
                    Mi_SQL += Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs + " MOVS ON MOVS." + Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento + " = ";
                    Mi_SQL += " ANX." + Ope_Pre_Recep_Docs_Anexos.Campo_No_Movimiento;

                    Mi_SQL += " WHERE MOVS." + Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + " = '" + Datos.P_No_Recepcion_Documento + "'";
                    Mi_SQL += " AND MOVS." + Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento + " = '" + renglon["NO_MOVIMIENTO"].ToString() + "'";

                    Dt_Resultado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Copy();
                    //Ds_Resultado.Tables[2].TableName = "Dt_Anexos";    

                    foreach (DataRow Dr_Temp in Dt_Resultado.Rows)
                    {
                        Documentos += Dr_Temp["NOMBRE_ANEXO"].ToString();
                        Documentos += "\n";
                    }
                    renglon["NOMBRE_DOCUMENTO"] = Documentos;
                    Documentos = "";
                }

                return Ds_Resultado;
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
    }//termina clase Cls_Ope_Pre_Recepcion_Documentos_Datos

}//termina namespacespace