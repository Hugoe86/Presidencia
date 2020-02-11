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
using Operacion_Predial_Validacion_Recepcion.Negocio;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;

namespace Operacion_Predial_Validacion_Recepcion.Datos
{

    public class Cls_Ope_Pre_Validacion_Recepcion_Datos
    {
        public Cls_Ope_Pre_Validacion_Recepcion_Datos()
        {
        }


        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Generar_Reporte_Contrarecibo
        ///DESCRIPCIÓN: realiza las consultas para llenar el dataset para hacer el reporte del contrarecibo
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 08/01/2011 06:38:19 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataSet Generar_Reporte_Contra_Recibo(Cls_Ope_Pre_Validacion_Recepcion_Negocio Datos)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL
            String Documentos = "";
            DataSet Ds_Resultado = new DataSet(); //Variable para almacenar las tablas resultantes
            
            try
            {               
                Mi_SQL = "";
                Mi_SQL = "SELECT ";
                Mi_SQL += " TO_CHAR(TO_NUMBER(REC." + Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + "))||'/'|| REC." + Ope_Pre_Recepcion_Documentos.Campo_Anio + " AS NO_RECEPCION_DOCUMENTO, ";
                Mi_SQL += " TO_CHAR(TO_NUMBER(REC." + Ope_Pre_Recepcion_Documentos.Campo_Notario_ID + ")) AS NO_NOTARIO_2, TO_CHAR(";
                Mi_SQL += " REC." + Ope_Pre_Recepcion_Documentos.Campo_Fecha + ", 'DD/Mon/YYYY HH:MI:SS PM')   AS FECHA_RECEPCION, ";
                Mi_SQL += " REC." + Ope_Pre_Recepcion_Documentos.Campo_Usuario_Modifico + " AS USUARIO_CREO, ";
                Mi_SQL += " NOTA." + Cat_Pre_Notarios.Campo_Apellido_Paterno + " ||' '|| ";
                Mi_SQL += " NOTA." + Cat_Pre_Notarios.Campo_Apellido_Materno + " ||' '|| ";
                Mi_SQL += " NOTA." + Cat_Pre_Notarios.Campo_Nombre + " AS NOMBRE_NOTARIO, ";
                Mi_SQL += " NOTA." + Cat_Pre_Notarios.Campo_Numero_Notaria + " AS NO_NOTARIO, ";
                Mi_SQL += " 'GENERADO' AS ESTATUS ";
                Mi_SQL += " FROM ";
                Mi_SQL += Ope_Pre_Recepcion_Documentos.Tabla_Ope_Pre_Recepcion_Documentos + " REC JOIN " + Cat_Pre_Notarios.Tabla_Cat_Pre_Notarios + " NOTA ";
                Mi_SQL += " ON NOTA." + Cat_Pre_Notarios.Campo_Notario_ID + " = REC." + Ope_Pre_Recepcion_Documentos.Campo_Notario_ID;
                Mi_SQL += " WHERE ";
                Mi_SQL += " REC." + Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + " = '" + Datos.P_No_Recepcion_Documento + "'";
                //DataTable Dt_Contrarecibo = new DataTable();
                //Dt_Contrarecibo.Copy(
                Ds_Resultado.Tables.Add(OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Copy());
                Ds_Resultado.Tables[0].TableName = "Dt_Contrarecibo";

                Mi_SQL = "";
                Mi_SQL = "SELECT ";
                Mi_SQL += " TO_CHAR(TO_NUMBER(MOV." + Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + ")) AS NO_RECEPCION_DOCUMENTO, ";
                Mi_SQL += " TO_CHAR(TO_NUMBER(MOV." + Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento + ")) AS NO_MOVIMIENTO, ";
                Mi_SQL += " MOV." + Ope_Pre_Recep_Docs_Movs.Campo_Numero_Escritura + " AS NUMERO_ESCRITURA, TO_CHAR(";
                Mi_SQL += " MOV." + Ope_Pre_Recep_Docs_Movs.Campo_Fecha_Escritura + ", 'DD/Mon/YYYY') AS FECHA_ESCRITURA, ";
                //Consulta de cuenta predial si es vacia
                Mi_SQL += " NVL( MOV." + Ope_Pre_Recep_Docs_Movs.Campo_Cuenta_Predial_ID + ", ";
                Mi_SQL += " (SELECT " + Ope_Pre_Contrarecibos.Campo_Cuenta_Predial_ID + " FROM " + 
                        Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + " WHERE " + 
                            Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " = MOV." + Ope_Pre_Recep_Docs_Movs.Campo_No_Contrarecibo + 
                            " AND " + Ope_Pre_Contrarecibos.Campo_Anio + " = MOV." + Ope_Pre_Recep_Docs_Movs.Campo_Anio + ") ) AS CUENTA_PREDIAL_ID, ";
                Mi_SQL += " TO_CHAR(TO_NUMBER(MOV." + Ope_Pre_Recep_Docs_Movs.Campo_No_Contrarecibo + ")) || '/' || ";
                Mi_SQL += " MOV." + Ope_Pre_Recep_Docs_Movs.Campo_Anio + " AS CONTRARECIBO, ";
                Mi_SQL += " MOV." + Ope_Pre_Recep_Docs_Movs.Campo_Usuario_Modifico + ", ";
                //Consulta de cuenta predial si es vacia
                Mi_SQL += " NVL( CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ", ";
                Mi_SQL += " (SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " FROM " +
                        Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " +
                            Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " IN ";
                Mi_SQL += " (SELECT " + Ope_Pre_Contrarecibos.Campo_Cuenta_Predial_ID + " FROM " +
                                      Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + " WHERE " +
                                          Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " = MOV." + Ope_Pre_Recep_Docs_Movs.Campo_No_Contrarecibo +
                                          " AND " + Ope_Pre_Contrarecibos.Campo_Anio + " = MOV." + Ope_Pre_Recep_Docs_Movs.Campo_Anio + "))) AS CUENTA_PREDIAL, "; 
                Mi_SQL += " NULL AS NOMBRE_DOCUMENTO ";
                Mi_SQL += " FROM " + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs + " MOV LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CUEN ON CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                Mi_SQL += " = MOV." + Ope_Pre_Recep_Docs_Movs.Campo_Cuenta_Predial_ID;
                if (!string.IsNullOrEmpty(Datos.P_No_Movimiento))
                {
                    Mi_SQL += " WHERE ";
                    Mi_SQL += " MOV." + Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento + " = " + Datos.P_No_Movimiento;
                }
                else
                {
                    Mi_SQL += " WHERE ";
                    Mi_SQL += " MOV." + Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + " = " + Datos.P_No_Recepcion_Documento;
                }

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

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consultar_Anexos_Recepcion
        ///DESCRIPCIÓN: consulta los documentos anexados a la recepcion de documentos
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 07/27/2011 10:42:15 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Anexos_Recepcion(Cls_Ope_Pre_Validacion_Recepcion_Negocio Datos)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL

            try
            {
                Mi_SQL = "SELECT";
                Mi_SQL = Mi_SQL + " ANX." + Ope_Pre_Recep_Docs_Anexos.Campo_No_Anexo + " AS NO_ANEXO, ";
                Mi_SQL = Mi_SQL + " ANX." + Ope_Pre_Recep_Docs_Anexos.Campo_No_Movimiento + " AS NO_MOVIMIENTO, ";
                Mi_SQL = Mi_SQL + " ANX." + Ope_Pre_Recep_Docs_Anexos.Campo_Ruta + " AS RUTA, ";
                Mi_SQL = Mi_SQL + " ANX." + Ope_Pre_Recep_Docs_Anexos.Campo_Comentarios + " AS COMENTARIOS, ";
                Mi_SQL = Mi_SQL + " ANX." + Ope_Pre_Recep_Docs_Anexos.Campo_Documento_ID + " AS DOCUMENTO_ID, ";
                Mi_SQL = Mi_SQL + " MOV." + Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento + " AS NO_MOVIMIENTO, ";
                Mi_SQL = Mi_SQL + " MOV." + Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + " AS NO_RECEPCION_DOCUMENTO, ";
                Mi_SQL = Mi_SQL + " REC." + Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + " AS NO_RECEPCION_DOCUMENTO, ";
                Mi_SQL = Mi_SQL + " DOC." + Cat_Pre_Tipos_Documento.Campo_Nombre_Documento + " AS NOMBRE_DOCUMENTO ";

                Mi_SQL = Mi_SQL + " FROM ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Anexos.Tabla_Ope_Pre_Recep_Docs_Anexos + " ANX ";
                Mi_SQL = Mi_SQL + " JOIN ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs + " MOV ";
                Mi_SQL = Mi_SQL + " ON ";
                Mi_SQL = Mi_SQL + "MOV." + Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento + " = ANX." + Ope_Pre_Recep_Docs_Anexos.Campo_No_Movimiento;//MOV.NO_MOVIMIENTO = ANX.NO_MOVIMIENTO 
                Mi_SQL = Mi_SQL + " JOIN ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recepcion_Documentos.Tabla_Ope_Pre_Recepcion_Documentos + " REC ";
                Mi_SQL = Mi_SQL + " ON ";
                Mi_SQL = Mi_SQL + " REC." + Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + " = MOV." + Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento; //REC.NO_RECEPCION_DOCUMENTO = MOV.NO_RECEPCION_DOCUMENTO 
                Mi_SQL = Mi_SQL + " JOIN ";
                Mi_SQL = Mi_SQL + Cat_Pre_Tipos_Documento.Tabla_Cat_Pre_Tipos_Documento + " DOC ";
                Mi_SQL = Mi_SQL + " ON ";
                Mi_SQL = Mi_SQL + "DOC." + Cat_Pre_Tipos_Documento.Campo_Documento_ID + " = ANX." + Ope_Pre_Recep_Docs_Anexos.Campo_Documento_ID;

                Mi_SQL = Mi_SQL + " WHERE ";
                Mi_SQL = Mi_SQL + "ANX." + Ope_Pre_Recep_Docs_Anexos.Campo_No_Movimiento + " = '" + Datos.P_No_Movimiento + "'";




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
        public static DataSet Busqueda_Recepciones_Movimientos(Cls_Ope_Pre_Validacion_Recepcion_Negocio Datos)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL
            DataSet Rs_Consulta_Recepciones_Movimientos = new DataSet();

            try
            {
                Mi_SQL = "SELECT";
                Mi_SQL += " REC." + Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + " AS NO_RECEPCION_DOCUMENTO, ";
                Mi_SQL += " REC." + Ope_Pre_Recepcion_Documentos.Campo_Notario_ID + " AS NOTARIO_ID, ";
                Mi_SQL += " REC." + Ope_Pre_Recepcion_Documentos.Campo_Fecha + " AS FECHA, ";
                Mi_SQL += " MOV." + Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento + " AS NO_MOVIMIENTO, ";
                Mi_SQL += " MOV." + Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + " AS NO_RECEPCION_DOCUMENTO, ";
                Mi_SQL += " MOV." + Ope_Pre_Recep_Docs_Movs.Campo_Numero_Escritura + " AS NUMERO_ESCRITURA, TO_CHAR(";
                Mi_SQL += " MOV." + Ope_Pre_Recep_Docs_Movs.Campo_Fecha_Escritura + ", 'DD/Mon/YYYY HH:MI:SS PM') AS FECHA_ESCRITURA, ";
                Mi_SQL += " MOV." + Ope_Pre_Recep_Docs_Movs.Campo_Cuenta_Predial_ID + " AS CUENTA_PREDIAL_ID, ";
                Mi_SQL += " MOV." + Ope_Pre_Recep_Docs_Movs.Campo_Estatus + " AS ESTATUS, ";
                Mi_SQL += " MOV." + Ope_Pre_Recep_Docs_Movs.Campo_Empleado_ID + " AS EMPLEADO_ID,";
                Mi_SQL += " NOTA." + Cat_Pre_Notarios.Campo_Notario_ID + " AS NOTARIO_ID, ";
                Mi_SQL += " NOTA." + Cat_Pre_Notarios.Campo_RFC + " AS RFC, ";
                Mi_SQL += " NOTA." + Cat_Pre_Notarios.Campo_Numero_Notaria + " AS NUMERO_NOTARIA, ";
                Mi_SQL += " NOTA." + Cat_Pre_Notarios.Campo_Apellido_Paterno + " ||' '|| ";
                Mi_SQL += " NOTA." + Cat_Pre_Notarios.Campo_Apellido_Materno + " ||' '|| ";
                Mi_SQL += " NOTA." + Cat_Pre_Notarios.Campo_Nombre + " AS NOMBRE_NOTARIO, ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " AS CUENTA_PREDIAL_ID, ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " AS CUENTA_PREDIAL, ";

                Mi_SQL += " EMPLEADOS." + Cat_Empleados.Campo_Nombre + " ||' '|| ";
                Mi_SQL += " EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno + " ||' '|| ";
                Mi_SQL += " EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno + " AS ASIGNADO, ";
                Mi_SQL += " NULL AS DT_DETALLES_RECEPCION, ";


                Mi_SQL += " (SELECT COUNT(*) FROM " + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs;
                Mi_SQL += " MOVS WHERE MOVS." + Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + " = ";
                Mi_SQL += "REC." + Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + ") AS TOTAL_MOVIMIENTOS, ";

                Mi_SQL += " (SELECT COUNT(*) FROM " + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs;
                Mi_SQL += " MOVS WHERE MOVS." + Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + " = ";
                Mi_SQL += "REC." + Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + " AND MOV.";
                Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Estatus + " LIKE 'PENDIENTE') AS TOTAL_PENDIENTES ";

                Mi_SQL += " FROM ";

                Mi_SQL += Ope_Pre_Recepcion_Documentos.Tabla_Ope_Pre_Recepcion_Documentos + " REC JOIN " + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs + " MOV ";
                Mi_SQL += " ON MOV." + Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + " = REC." + Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento;
                Mi_SQL += " LEFT OUTER JOIN " + Cat_Pre_Notarios.Tabla_Cat_Pre_Notarios + " NOTA ON NOTA." + Cat_Pre_Notarios.Campo_Notario_ID + " = ";
                Mi_SQL += " REC." + Ope_Pre_Recepcion_Documentos.Campo_Notario_ID + " LEFT OUTER JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CUEN ON CUEN.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = MOV." + Ope_Pre_Recep_Docs_Movs.Campo_Cuenta_Predial_ID;
                Mi_SQL += " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS ON EMPLEADOS." + Cat_Empleados.Campo_Empleado_ID + " = ";
                Mi_SQL += " MOV." + Ope_Pre_Recep_Docs_Movs.Campo_Empleado_ID;

                //Mi_SQL += " WHERE MOV.";
                //Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Empleado_ID + " ='";
                //Mi_SQL += Cls_Sessiones.Empleado_ID.ToString() + "'";
                //Mi_SQL += " WHERE (MOV." + Ope_Pre_Recep_Docs_Movs.Campo_Estatus + "='PENDIENTE'";
                //Mi_SQL += " OR MOV." + Ope_Pre_Recep_Docs_Movs.Campo_Estatus + "='RECHAZADO')";
                Mi_SQL += " WHERE";
                if (String.IsNullOrEmpty(Datos.P_Estatus_Movimiento))  // si hay un numero de recepciona de documentos, incluir id del notario
                {
                    Mi_SQL += " MOV." + Ope_Pre_Recep_Docs_Movs.Campo_Estatus;
                    Mi_SQL += " <> ' '";
                }
                else
                {
                    Mi_SQL += " (MOV." + Ope_Pre_Recep_Docs_Movs.Campo_Estatus + "='PENDIENTE')";                    
                }
                if (!String.IsNullOrEmpty(Datos.P_No_Recepcion_Documento))  // si hay un numero de recepciona de documentos, incluir id del notario
                {
                    Mi_SQL += " AND REC.";
                    Mi_SQL += Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + " ='";
                    Mi_SQL += Datos.P_No_Recepcion_Documento + "'";
                }

                if (!String.IsNullOrEmpty(Datos.P_Notario_ID))
                {
                    Mi_SQL += " AND NOTA.";
                    Mi_SQL += Cat_Pre_Notarios.Campo_Notario_ID + "  LIKE '%";
                    Mi_SQL += Datos.P_Notario_ID + "%'";
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
                    Mi_SQL += Cat_Pre_Notarios.Campo_Numero_Notaria + " ='";
                    Mi_SQL += Datos.P_Numero_Notaria + "'";
                }
                if (!String.IsNullOrEmpty(Datos.P_Cuenta_Predial))
                {
                    Mi_SQL += " AND CUEN.";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " LIKE '%";
                    Mi_SQL += Datos.P_Cuenta_Predial + "%'";
                }
                if (!String.IsNullOrEmpty(Datos.P_Numero_Escritura))
                {
                    Mi_SQL += " AND MOV.";
                    Mi_SQL += Ope_Pre_Recep_Docs_Movs.Campo_Numero_Escritura + " ='";
                    Mi_SQL += Datos.P_Numero_Escritura + "'";
                }                
                Mi_SQL += " ORDER BY REC." + Ope_Pre_Recepcion_Documentos.Campo_Fecha + " DESC";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                //Mi_SQL = "SELECT";
                //Mi_SQL += " REC." + Ope_Pre_Recepcion_Documentos.Campo_No_Recepcion_Documento + " AS NO_RECEPCION_DOCUMENTO, ";



                //Rs_Consulta_Recepciones_Movimientos.Tables["Tabla_Cuentas"] = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                //return Rs_Consulta_Recepciones_Movimientos;
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
        ///NOMBRE DE LA FUNCIÓN: Modificar_Recepcion_Movimiento
        ///DESCRIPCIÓN: modificar estatus y observaciones
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 07/27/2011 04:13:58 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        internal static void Modificar_Recepcion_Movimiento(Cls_Ope_Pre_Validacion_Recepcion_Negocio Datos)
        {
            //Declaracion de variables
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion;
            OracleCommand Obj_Comando;
            String Mi_SQL = String.Empty;  //Variable para la sentencia SQL
            String Mensaje = String.Empty; //Variable para el mensaje de error
            String Resultado = ""; //Variable para el resultado
            Object Aux; //Variable auxiliar para las consultas
            int Movimientos;

            try
            {
                Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
                Obj_Comando = new OracleCommand();
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;
                
                    //Formar Sentencia 
                    Mi_SQL = "";
                    Mi_SQL = "SELECT NVL(MAX(" + Ope_Pre_Historial_Obs_Recep.Campo_No_Observacion_ID +
                        "),'0000000001') AS OBSERVACION_ID FROM " +
                            Ope_Pre_Historial_Obs_Recep.Tabla_Ope_Pre_Historial_Obs_Recep;

                    Aux = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                    if (Convert.IsDBNull(Aux))
                    {
                        Datos.P_No_Observacion_ID = "0000000001";
                    }
                    else
                    {
                        Datos.P_No_Observacion_ID = String.Format("{0:0000000000}", Convert.ToInt32(Aux) + 1);
                    }

                    Mi_SQL = "";
                    Mi_SQL = " INSERT INTO " + Ope_Pre_Historial_Obs_Recep.Tabla_Ope_Pre_Historial_Obs_Recep + "(";
                    Mi_SQL = Mi_SQL + Ope_Pre_Historial_Obs_Recep.Campo_No_Observacion_ID + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Historial_Obs_Recep.Campo_No_Movimiento + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Historial_Obs_Recep.Campo_Observaciones + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Historial_Obs_Recep.Campo_Usuario_Creo + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Historial_Obs_Recep.Campo_Fecha_Creo + ")VALUES('";

                    Mi_SQL = Mi_SQL + Datos.P_No_Observacion_ID + "', '";
                    Mi_SQL = Mi_SQL + Datos.P_No_Movimiento + "', '";
                    Mi_SQL = Mi_SQL + Datos.P_Observaciones + "', '";
                    Mi_SQL = Mi_SQL + Datos.P_Empleado_Session + "', SYSDATE)";

                    Obj_Comando.CommandText = Mi_SQL;
                    Obj_Comando.ExecuteNonQuery();
                
                Mi_SQL = "";
                Mi_SQL = "UPDATE " + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs + " SET ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Estatus + " = '";
                Mi_SQL = Mi_SQL + Datos.P_Estatus_Movimiento + "', ";                
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Usuario_Modifico + " = '";
                Mi_SQL = Mi_SQL + Datos.P_Empleado_Session + "',";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Fecha_Modifico + " = SYSDATE ";                
                    Mi_SQL = Mi_SQL + "WHERE ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento + " = '";
                    Mi_SQL = Mi_SQL + Datos.P_No_Movimiento + "' ";                

                //Ejecutar comando
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();

                //Generacion de Contrarecibo
                if (Datos.P_Estatus_Movimiento == "ACEPTADO")
                {
                    DateTime Fecha_;
                    String Fecha_Formato;

                    Fecha_ = Convert.ToDateTime(Datos.P_Fecha_Escritura);
                    Fecha_Formato = String.Format("{0:dd/MM/yyyy}", Fecha_);
                    //Se consulta el siguiente ID para la incercion
                    Mi_SQL = "";
                    Mi_SQL = "SELECT NVL(MAX(" + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo +
                        "),'0000000001') AS CONTRARECIBO_ID FROM " +
                            Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos;

                    Aux = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                    if (Convert.IsDBNull(Aux))
                    {
                        Datos.P_No_Contrarecibo = "0000000001";
                    }
                    else
                    {
                        Datos.P_No_Contrarecibo = String.Format("{0:0000000000}", Convert.ToInt32(Aux) + 1);
                    }

                    //Consulta para la insercion de el registro del contrarecibo
                    
                    Mi_SQL = "";
                    Mi_SQL = "INSERT INTO " + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + "(";
                    Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Campo_Anio + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Campo_Cuenta_Predial_ID + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Campo_No_Escritura + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Campo_Fecha_Escritura + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Campo_Fecha_Liberacion + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Campo_Estatus + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Campo_Usuario_Creo + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Campo_Fecha_Creo + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Campo_Notario_ID + ") VALUES( '";

                    Mi_SQL = Mi_SQL + Datos.P_No_Contrarecibo + "', ";
                    Mi_SQL = Mi_SQL + DateTime.Now.Year.ToString() + ", '";
                    Mi_SQL = Mi_SQL + Datos.P_Cuenta_Predial_ID + "', ";
                    Mi_SQL = Mi_SQL + Datos.P_Numero_Escritura + ", '";
                    Mi_SQL = Mi_SQL + Fecha_Formato + "', SYSDATE, 'GENERADO', '";
                    Mi_SQL = Mi_SQL + Datos.P_Empleado_Session + "', SYSDATE, '";
                    Mi_SQL = Mi_SQL + Datos.P_Notario_ID + "')";

                    Obj_Comando.CommandText = Mi_SQL;
                    Obj_Comando.ExecuteNonQuery();

                    //Formar Sentencia 
                    Mi_SQL = "";
                    Mi_SQL = "UPDATE " + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs + " SET ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_No_Contrarecibo + " = '";
                    Mi_SQL = Mi_SQL + Datos.P_No_Contrarecibo + "' ";
                    Mi_SQL = Mi_SQL + "WHERE ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento + " = '";
                    Mi_SQL = Mi_SQL + Datos.P_No_Movimiento + "' ";

                    //Ejecutar comando
                    Obj_Comando.CommandText = Mi_SQL;
                    Obj_Comando.ExecuteNonQuery();
                }
                //Ejecutar transaccion
                Obj_Transaccion.Commit();
                Obj_Conexion.Close();
            }
            catch (OracleException Ex)
            {
                if (Obj_Transaccion != null)
                {
                    Obj_Transaccion.Rollback();
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
                        Mensaje = "Error:  [" + Ex.Message + "]- " + Mi_SQL;
                        break;
                }

                throw new Exception(Mensaje, Ex);
            }
            finally
            {
                Obj_Comando = null;
                Obj_Conexion = null;
                Obj_Transaccion = null;
            }
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Modificar_Recepcion_Movimiento_Directa
        ///DESCRIPCIÓN: modificar estatus
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 07/27/2011 04:13:58 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        internal static void Modificar_Recepcion_Movimiento_Directa(Cls_Ope_Pre_Validacion_Recepcion_Negocio Datos)
        {
            //Declaracion de variables
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion;
            OracleCommand Obj_Comando;
            String Mi_SQL = String.Empty;  //Variable para la sentencia SQL
            String Mensaje = String.Empty; //Variable para el mensaje de error
            String Resultado = ""; //Variable para el resultado
            Object Aux; //Variable auxiliar para las consultas
            int Movimientos;

            try
            {
                Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
                Obj_Comando = new OracleCommand();
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;                

                Mi_SQL = "";
                Mi_SQL = "UPDATE " + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs + " SET ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Estatus + " = '";
                Mi_SQL = Mi_SQL + Datos.P_Estatus_Movimiento + "', ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Usuario_Modifico + " = '";
                Mi_SQL = Mi_SQL + Datos.P_Empleado_Session + "',";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_Fecha_Modifico + " = SYSDATE ";
                
                    Mi_SQL = Mi_SQL + "WHERE ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + " = '";
                    Mi_SQL = Mi_SQL + Datos.P_No_Recepcion_Documento + "' ";
                

                //Ejecutar comando
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();

                //Generacion de Contrarecibo

                DateTime Fecha_;
                String Fecha_Formato;

                Fecha_ = Convert.ToDateTime(Datos.P_Fecha_Escritura);
                Fecha_Formato = String.Format("{0:dd/MM/yyyy}", Fecha_);
                //Se consulta el siguiente ID para la incercion
                Mi_SQL = "";
                Mi_SQL = "SELECT NVL(MAX(" + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo +
                    "),'0000000001') AS CONTRARECIBO_ID FROM " +
                        Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos;

                Aux = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(Aux))
                {
                    Datos.P_No_Contrarecibo = "0000000001";
                }
                else
                {
                    Datos.P_No_Contrarecibo = String.Format("{0:0000000000}", Convert.ToInt32(Aux) + 1);
                }

                //Consulta para la insercion de el registro del contrarecibo

                Mi_SQL = "";
                Mi_SQL = "INSERT INTO " + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + "(";
                Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Campo_Anio + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Campo_Cuenta_Predial_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Campo_No_Escritura + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Campo_Fecha_Escritura + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Campo_Fecha_Liberacion + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Campo_Fecha_Creo + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Campo_Notario_ID + ") VALUES( '";

                Mi_SQL = Mi_SQL + Datos.P_No_Contrarecibo + "', ";
                Mi_SQL = Mi_SQL + DateTime.Now.Year.ToString() + ", ";
                if (!String.IsNullOrEmpty( Datos.P_Cuenta_Predial_ID) )
                Mi_SQL = Mi_SQL + "'" + Datos.P_Cuenta_Predial_ID + "', ";
                else
                Mi_SQL = Mi_SQL + "NULL,";
                Mi_SQL = Mi_SQL + Datos.P_Numero_Escritura + ", '";
                Mi_SQL = Mi_SQL + Fecha_Formato + "', SYSDATE, 'GENERADO', '";
                Mi_SQL = Mi_SQL + Cls_Sessiones.Nombre_Empleado + "', SYSDATE, '";
                Mi_SQL = Mi_SQL + Datos.P_Notario_ID + "')";

                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();

                //Formar Sentencia 
                Mi_SQL = "";
                Mi_SQL = "UPDATE " + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs + " SET ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_No_Contrarecibo + " = '";
                Mi_SQL = Mi_SQL + Datos.P_No_Contrarecibo + "' ";
                Mi_SQL = Mi_SQL + "WHERE ";
                Mi_SQL = Mi_SQL + Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + " = '";
                Mi_SQL = Mi_SQL + Datos.P_No_Recepcion_Documento + "' ";

                //Ejecutar comando
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();

                //Ejecutar transaccion
                Obj_Transaccion.Commit();
                Obj_Conexion.Close();
            }
            catch (OracleException Ex)
            {
                if (Obj_Transaccion != null)
                {
                    Obj_Transaccion.Rollback();
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
                        Mensaje = "Error:  [" + Ex.Message + "]- " + Mi_SQL;
                        break;
                }

                throw new Exception(Mensaje, Ex);
            }
            finally
            {
                Obj_Comando = null;
                Obj_Conexion = null;
                Obj_Transaccion = null;
            }
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consultar_Observaciones
        ///DESCRIPCIÓN: consulta las observaciones del personal que valido la recepcion de documentos
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 07/27/2011 07:25:55 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************* 
        internal static DataTable Consultar_Observaciones(Cls_Ope_Pre_Validacion_Recepcion_Negocio Datos)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL

            try
            {
                Mi_SQL = "SELECT";
                Mi_SQL = Mi_SQL + " MOV." + Ope_Pre_Historial_Obs_Recep.Campo_Observaciones + " AS OBSERVACIONES, TO_CHAR( ";
                Mi_SQL = Mi_SQL + " MOV." + Ope_Pre_Historial_Obs_Recep.Campo_Fecha_Creo + " , 'DD/Mon/YYYY HH:MI:SS PM') AS FECHA_MODIFICO, ";
                Mi_SQL = Mi_SQL + " MOV." + Ope_Pre_Historial_Obs_Recep.Campo_Usuario_Creo + " AS USUARIO_MODIFICO";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Historial_Obs_Recep.Tabla_Ope_Pre_Historial_Obs_Recep + " MOV ";

                Mi_SQL = Mi_SQL + " WHERE ";
                Mi_SQL = Mi_SQL + "MOV." + Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento + " = '" + Datos.P_No_Movimiento + "'";

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
    }
}

