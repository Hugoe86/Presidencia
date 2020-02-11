using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Operacion_Cat_Memorias_Descriptivas.Negocio;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Data;
using SharpContent.ApplicationBlocks.Data;

/// <summary>
/// Summary description for Cls_Ope_Cat_Memorias_Descriptivas_Datos
/// </summary>

namespace Presidencia.Operacion_Cat_Memorias_Descriptivas.Datos
{
    public class Cls_Ope_Cat_Memorias_Descriptivas_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Memoria_Descriptiva
        ///DESCRIPCIÓN: Da de alta en la Base de Datos Un nuevo motivo de avalúo
        ///PARAMENTROS:     
        ///             1. Motivo_Avaluo.   Instancia de la Clase de Negocio de Motivos de avalúo 
        ///                                 con los datos del que van a ser
        ///                                 dados de Alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Alta_Memoria_Descriptiva(Cls_Ope_Cat_Memorias_Descriptivas_Negocio Documentos)
        {
            Boolean Alta = false;
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            String Mi_sql = "";
            String No_Mem_Descript = "";
            No_Mem_Descript = Obtener_ID_Consecutivo(Ope_Cat_Mem_Descript.Tabla_Ope_Cat_Mem_Descript, Ope_Cat_Mem_Descript.Campo_No_Mem_Descript, Ope_Cat_Mem_Descript.Campo_Anio+"="+Documentos.P_Anio, 10);
            String No_Documento = Obtener_ID_Consecutivo(Ope_Cat_Doc_Mem_Descript.Tabla_Ope_Cat_Doc_Mem_Descript, Ope_Cat_Doc_Mem_Descript.Campo_No_Documento, Ope_Cat_Doc_Mem_Descript.Campo_Anio_Documento+"="+Documentos.P_Anio_Documento, 10);
            try
            {
                Mi_sql = "INSERT INTO " + Ope_Cat_Mem_Descript.Tabla_Ope_Cat_Mem_Descript;
                Mi_sql += "(" + Ope_Cat_Mem_Descript.Campo_No_Mem_Descript + ", ";
                Mi_sql += Ope_Cat_Mem_Descript.Campo_Anio + ", ";
                Mi_sql += Ope_Cat_Mem_Descript.Campo_Cantidad_Mem_Descript + ", ";
                Mi_sql += Ope_Cat_Mem_Descript.Campo_Estatus + ", ";
                Mi_sql += Ope_Cat_Mem_Descript.Campo_Observaciones + ", ";
                Mi_sql += Ope_Cat_Mem_Descript.Campo_Tipo + ", ";
                Mi_sql += Ope_Cat_Mem_Descript.Campo_Fraccionamiento + ", ";
                Mi_sql += Ope_Cat_Mem_Descript.Campo_Perito_Externo_ID + ", ";
                Mi_sql += Ope_Cat_Mem_Descript.Campo_Ubicacion + ", ";
                Mi_sql += Ope_Cat_Mem_Descript.Campo_Solicitante + ", ";
                Mi_sql += Ope_Cat_Mem_Descript.Campo_Cuenta_Predial_Id + ", ";
                Mi_sql += Ope_Cat_Mem_Descript.Campo_Tipo_Horientacion + ", ";
                Mi_sql += Ope_Cat_Mem_Descript.Campo_Usuario_Creo + ", ";                
                Mi_sql += Ope_Cat_Mem_Descript.Campo_Fecha_Creo + ") VALUES ('";
                Mi_sql += No_Mem_Descript + "', ";
                Mi_sql += Documentos.P_Anio + ", ";
                Mi_sql += Documentos.P_Cantidad_Mem_Descript + ", '";
                Mi_sql += Documentos.P_Estatus + "', '";
                Mi_sql += Documentos.P_Observaciones + "', '";
                Mi_sql += Documentos.P_Tipo + "', '";
                Mi_sql += Documentos.P_Fraccionamiento + "', '";
                Mi_sql += Documentos.P_Perito_Externo_Id + "', '";
                Mi_sql += Documentos.P_Ubicacion + "', '";
                Mi_sql += Documentos.P_Solicitante + "', '";
                Mi_sql += Documentos.P_Cuenta_Predial_Id + "', '";
                Mi_sql += Documentos.P_Horientacion + "', '";
                Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_sql += "SYSDATE";
                Mi_sql += ")";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();
                Documentos.P_No_Mem_Descript = No_Mem_Descript;

                foreach (DataRow Dr_Renglon in Documentos.P_Dt_Archivos.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Doc_Mem_Descript.Tabla_Ope_Cat_Doc_Mem_Descript + "(";
                        Mi_sql += Ope_Cat_Doc_Mem_Descript.Campo_No_Documento + ", ";
                        Mi_sql += Ope_Cat_Doc_Mem_Descript.Campo_Anio_Documento + ", ";
                        Mi_sql += Ope_Cat_Doc_Mem_Descript.Campo_No_Mem_Descript + ", ";
                        Mi_sql += Ope_Cat_Doc_Mem_Descript.Campo_Anio + ", ";
                        Mi_sql += Ope_Cat_Doc_Mem_Descript.Campo_Regimen_Condominio_Id + ", ";
                        Mi_sql += Ope_Cat_Doc_Mem_Descript.Campo_Ruta_Documento + ", ";
                        Mi_sql += Ope_Cat_Doc_Mem_Descript.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Doc_Mem_Descript.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Documento + "', ";
                        Mi_sql += Dr_Renglon[Ope_Cat_Doc_Mem_Descript.Campo_Anio_Documento].ToString() + ", '";
                        Mi_sql += Documentos.P_No_Mem_Descript + "', ";
                        Mi_sql += Documentos.P_Anio + ", '";
                        Mi_sql += Dr_Renglon[Ope_Cat_Doc_Mem_Descript.Campo_Regimen_Condominio_Id].ToString() + "', '";
                        Mi_sql += "../Catastro/Archivos_Memorias/" + Documentos.P_Anio_Documento + "_" + Documentos.P_No_Mem_Descript + "/" + Dr_Renglon[Ope_Cat_Doc_Mem_Descript.Campo_Ruta_Documento].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                        No_Documento = (Convert.ToInt32(No_Documento) + 1).ToString("0000000000");
                    }
                }
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Alta_Memoria_Descriptiva: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Memoria_Descriptiva
        ///DESCRIPCIÓN: Da de alta en la Base de Datos Un nuevo motivo de avalúo
        ///PARAMENTROS:     
        ///             1. Motivo_Avaluo.   Instancia de la Clase de Negocio de Motivos de avalúo 
        ///                                 con los datos del que van a ser
        ///                                 dados de Alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///
        ///
        ///*******************************************************************************
        public static Boolean Modificar_Memoria_Descriptiva(Cls_Ope_Cat_Memorias_Descriptivas_Negocio Documentos)
        {
            Boolean Alta = false;
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            String Mi_sql = "";
            String No_Documento = Obtener_ID_Consecutivo(Ope_Cat_Doc_Mem_Descript.Tabla_Ope_Cat_Doc_Mem_Descript, Ope_Cat_Doc_Mem_Descript.Campo_No_Documento, Ope_Cat_Doc_Mem_Descript.Campo_Anio_Documento + "=" + Documentos.P_Anio_Documento, 10);
            
            try
            {
                Mi_sql = "UPDATE " + Ope_Cat_Mem_Descript.Tabla_Ope_Cat_Mem_Descript;
                Mi_sql += " SET " + Ope_Cat_Mem_Descript.Campo_Anio + " = '" + Documentos.P_Anio + "', ";
                Mi_sql += Ope_Cat_Mem_Descript.Campo_Cantidad_Mem_Descript + " = '" + Documentos.P_Cantidad_Mem_Descript + "', ";
                Mi_sql += Ope_Cat_Mem_Descript.Campo_Estatus + " = '" + Documentos.P_Estatus + "', ";
                Mi_sql += Ope_Cat_Mem_Descript.Campo_Observaciones + " = '" + Documentos.P_Observaciones + "', ";
                Mi_sql += Ope_Cat_Mem_Descript.Campo_Tipo + " = '" + Documentos.P_Tipo + "', ";
                Mi_sql += Ope_Cat_Mem_Descript.Campo_Ubicacion + " = '" + Documentos.P_Ubicacion + "', ";
                Mi_sql += Ope_Cat_Mem_Descript.Campo_Fraccionamiento + " = '" + Documentos.P_Fraccionamiento + "', ";
                Mi_sql += Ope_Cat_Mem_Descript.Campo_Perito_Externo_ID + " = '" + Documentos.P_Perito_Externo_Id + "', ";
                Mi_sql += Ope_Cat_Mem_Descript.Campo_Solicitante + " = '" + Documentos.P_Solicitante + "', ";
                Mi_sql += Ope_Cat_Mem_Descript.Campo_Cuenta_Predial_Id + " = '" + Documentos.P_Cuenta_Predial_Id + "', ";
                Mi_sql += Ope_Cat_Mem_Descript.Campo_Tipo_Horientacion + " = '" + Documentos.P_Horientacion + "', ";
                Mi_sql += Ope_Cat_Mem_Descript.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";                
                Mi_sql += Ope_Cat_Mem_Descript.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_sql += " WHERE " + Ope_Cat_Mem_Descript.Campo_No_Mem_Descript + " = '" + Documentos.P_No_Mem_Descript + "'";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();
                foreach (DataRow Dr_Renglon in Documentos.P_Dt_Archivos.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Doc_Mem_Descript.Tabla_Ope_Cat_Doc_Mem_Descript + "(";
                        Mi_sql += Ope_Cat_Doc_Mem_Descript.Campo_No_Documento + ", ";
                        Mi_sql += Ope_Cat_Doc_Mem_Descript.Campo_Anio_Documento + ", ";
                        Mi_sql += Ope_Cat_Doc_Mem_Descript.Campo_No_Mem_Descript + ", ";
                        Mi_sql += Ope_Cat_Doc_Mem_Descript.Campo_Anio + ", ";
                        Mi_sql += Ope_Cat_Doc_Mem_Descript.Campo_Regimen_Condominio_Id + ", ";
                        Mi_sql += Ope_Cat_Doc_Mem_Descript.Campo_Ruta_Documento + ", ";
                        Mi_sql += Ope_Cat_Doc_Mem_Descript.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Doc_Mem_Descript.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Documento + "', ";
                        Mi_sql += Dr_Renglon[Ope_Cat_Doc_Mem_Descript.Campo_Anio_Documento].ToString() + ", '";
                        Mi_sql += Documentos.P_No_Mem_Descript + "', ";
                        Mi_sql += Documentos.P_Anio + ", '";
                        Mi_sql += Dr_Renglon[Ope_Cat_Doc_Mem_Descript.Campo_Regimen_Condominio_Id].ToString() + "', '";
                        Mi_sql += "../Catastro/Archivos_Memorias/" + Documentos.P_Anio_Documento + "_" + Documentos.P_No_Mem_Descript + "/" + Dr_Renglon[Ope_Cat_Doc_Mem_Descript.Campo_Ruta_Documento].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();                        
                    }
                    else if (Dr_Renglon["ACCION"].ToString() == "BAJA" && Dr_Renglon["NO_DOCUMENTO"].ToString().Trim().Replace("&nbsp;", "") != "")
                    {
                        Mi_sql = "DELETE " + Ope_Cat_Doc_Mem_Descript.Tabla_Ope_Cat_Doc_Mem_Descript + " WHERE " + Ope_Cat_Doc_Mem_Descript.Campo_No_Documento;
                        Mi_sql += "='" + Dr_Renglon[Ope_Cat_Doc_Mem_Descript.Campo_No_Documento].ToString() + "'";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                }
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Modificacion_Memoria_Descriptiva: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Estatus_Memoria
        ///DESCRIPCIÓN: Modifica El estatus del perito Externo Temporal
        ///PARAMENTROS:     
        ///             1. Documentos.      Instancia de la Clase de Negocio de Recepcion de documentos del perito externo 
        ///                                 con los datos del que van a ser
        ///                                 modificado.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Modificar_Estatus_Memoria(Cls_Ope_Cat_Memorias_Descriptivas_Negocio Documentos)
        {
            Boolean Alta = false;
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            String Mi_sql = "";
            try
            {
                Mi_sql = "UPDATE " + Ope_Cat_Mem_Descript.Tabla_Ope_Cat_Mem_Descript + " SET " +
                    Ope_Cat_Mem_Descript.Campo_Observaciones + "='" + Documentos.P_Observaciones + "', " +
                    Ope_Cat_Mem_Descript.Campo_Estatus + "='" + Documentos.P_Estatus + "' WHERE " +
                    Ope_Cat_Mem_Descript.Campo_No_Mem_Descript +
                    " = '" + Documentos.P_No_Mem_Descript + "' AND " + Ope_Cat_Mem_Descript.Campo_Anio + "=" + Documentos.P_Anio;
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Modificar_Estatus_Memoria: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Memorias_Descriptivas
        ///DESCRIPCIÓN: Consulta los motivos de Avaluo
        ///PARAMENTROS:     
        ///             1. Motivo_Avaluo.   Instancia de la Clase de Negocio de Motivos de Avaluo 
        ///                                 con los datos que servirán de
        ///                                 filtro.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Memorias_Descriptivas(Cls_Ope_Cat_Memorias_Descriptivas_Negocio Memoria)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "SELECT " + Ope_Cat_Mem_Descript.Campo_No_Mem_Descript
                    + ", " + Ope_Cat_Mem_Descript.Campo_Anio
                    + ", " + Ope_Cat_Mem_Descript.Campo_Cantidad_Mem_Descript
                    + ", " + Ope_Cat_Mem_Descript.Campo_Estatus
                    + ", " + Ope_Cat_Mem_Descript.Campo_Observaciones
                    + ", " + Ope_Cat_Mem_Descript.Campo_Tipo
                    + ", " + Ope_Cat_Mem_Descript.Campo_Solicitante
                    + ", " + Ope_Cat_Mem_Descript.Campo_Fraccionamiento
                    + ", " + Ope_Cat_Mem_Descript.Campo_Ubicacion
                    + ", " + Ope_Cat_Mem_Descript.Campo_Perito_Externo_ID
                    + ", " + Ope_Cat_Mem_Descript.Campo_Cuenta_Predial_Id
                    + ", " + Ope_Cat_Mem_Descript.Campo_Tipo_Horientacion                  
                    + ", " + Ope_Cat_Mem_Descript.Campo_Fecha_Creo
                    + ", " + Ope_Cat_Mem_Descript.Campo_Usuario_Creo
                    + " FROM  " + Ope_Cat_Mem_Descript.Tabla_Ope_Cat_Mem_Descript
                    + " WHERE ";
                if (Memoria.P_No_Mem_Descript != null && Memoria.P_No_Mem_Descript.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Mem_Descript.Campo_No_Mem_Descript + " = '" + Memoria.P_No_Mem_Descript + "' AND ";
                }
                if (Memoria.P_Anio != null && Memoria.P_Anio.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Mem_Descript.Campo_Anio + " = " + Memoria.P_Anio + " AND ";
                }
                if (Memoria.P_Cantidad_Mem_Descript != null && Memoria.P_Cantidad_Mem_Descript.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Mem_Descript.Campo_Cantidad_Mem_Descript + " = " + Memoria.P_Cantidad_Mem_Descript + " AND ";
                }
                if (Memoria.P_Solicitante != null && Memoria.P_Solicitante.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Mem_Descript.Campo_Solicitante + " = '" + Memoria.P_Solicitante + "' AND ";
                }
                if (Memoria.P_Fraccionamiento != null && Memoria.P_Fraccionamiento.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Mem_Descript.Campo_Fraccionamiento + " = '" + Memoria.P_Fraccionamiento + "' AND ";
                }
                if (Memoria.P_Ubicacion != null && Memoria.P_Ubicacion.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Mem_Descript.Campo_Ubicacion + " = '" + Memoria.P_Ubicacion + "' AND ";
                }
                if (Memoria.P_Perito_Externo_Id != null && Memoria.P_Perito_Externo_Id.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Mem_Descript.Campo_Perito_Externo_ID + " = '" + Memoria.P_Perito_Externo_Id + "' AND ";
                }              
                if (Memoria.P_Estatus != null && Memoria.P_Estatus.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Mem_Descript.Campo_Estatus + " " + Memoria.P_Estatus + "";
                }
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                Mi_SQL += " ORDER BY " + Ope_Cat_Mem_Descript.Campo_No_Mem_Descript + " DESC";
                // agregar filtro y orden a la consulta
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los documentos. Error: [" + Ex.Message + "]."; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Documentos_Memorias_Descriptivas
        ///DESCRIPCIÓN: Consulta los Documentos del Perito Externo
        ///PARAMENTROS:     
        ///             1. Motivo_Avaluo.   Instancia de la Clase de Negocio de recepcion de documentos del perito externo
        ///                                 con los datos que servirán de
        ///                                 filtro.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Documentos_Memorias_Descriptivas(Cls_Ope_Cat_Memorias_Descriptivas_Negocio Memoria)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "SELECT " + Ope_Cat_Doc_Mem_Descript.Campo_No_Documento
                    + ", " + Ope_Cat_Doc_Mem_Descript.Campo_Anio_Documento
                    + ", " + Ope_Cat_Doc_Mem_Descript.Campo_No_Mem_Descript
                    + ", " + Ope_Cat_Doc_Mem_Descript.Campo_Anio
                    + ", " + Ope_Cat_Doc_Mem_Descript.Campo_Regimen_Condominio_Id
                    + ", (SELECT RC." + Cat_Cat_Reg_Condominio.Campo_Nombre_Documento + " FROM " + Cat_Cat_Reg_Condominio.Tabla_Cat_Cat_Reg_Condominio + " RC WHERE RC." + Cat_Cat_Reg_Condominio.Campo_Regimen_Condominio_ID + "=" + Ope_Cat_Doc_Mem_Descript.Tabla_Ope_Cat_Doc_Mem_Descript + "." + Ope_Cat_Doc_Mem_Descript.Campo_Regimen_Condominio_Id + ") AS DOCUMENTO"
                    + ", " + Ope_Cat_Doc_Mem_Descript.Campo_Ruta_Documento
                    + ", " + Ope_Cat_Doc_Mem_Descript.Campo_Fecha_Creo
                    + ", " + Ope_Cat_Doc_Mem_Descript.Campo_Usuario_Creo
                    + ", " + Ope_Cat_Doc_Mem_Descript.Campo_Fecha_Modifico
                    + ", " + Ope_Cat_Doc_Mem_Descript.Campo_Usuario_Modifico
                    + ", 'NADA' AS ACCION"
                    + " FROM  " + Ope_Cat_Doc_Mem_Descript.Tabla_Ope_Cat_Doc_Mem_Descript
                    + " WHERE ";
                if (Memoria.P_Regimen_Condominio_Id != null && Memoria.P_Regimen_Condominio_Id.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Doc_Mem_Descript.Campo_Regimen_Condominio_Id + " = '" + Memoria.P_Regimen_Condominio_Id + "' AND ";
                }
                if (Memoria.P_No_Documento != null && Memoria.P_No_Documento.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Doc_Mem_Descript.Campo_No_Documento + " = '" + Memoria.P_No_Documento + "' AND ";
                }
                if (Memoria.P_Anio_Documento != null && Memoria.P_Anio_Documento.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Doc_Mem_Descript.Campo_Anio_Documento + " = " + Memoria.P_Anio_Documento + " AND ";
                }
                if (Memoria.P_No_Mem_Descript != null && Memoria.P_No_Mem_Descript.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Doc_Mem_Descript.Campo_No_Mem_Descript + " = '" + Memoria.P_No_Mem_Descript + "' AND ";
                }
                if (Memoria.P_Anio != null && Memoria.P_Anio.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Doc_Mem_Descript.Campo_Anio + " = " + Memoria.P_Anio + " AND ";
                }
                if (Memoria.P_Ruta_Documento != null && Memoria.P_Ruta_Documento.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Doc_Mem_Descript.Campo_Ruta_Documento + " LIKE '%" + Memoria.P_Ruta_Documento + "%' AND ";
                }
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                Mi_SQL += " ORDER BY " + Ope_Cat_Doc_Mem_Descript.Campo_No_Documento + " DESC";
                // agregar filtro y orden a la consulta
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los documentos. Error: [" + Ex.Message + "]."; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : Antonio Salvador Benavides Guardado
        ///FECHA_MODIFICO       : 26/Octubre/2010
        ///CAUSA_MODIFICACIÓN   : Estandarizar variables usadas
        ///*******************************************************************************
        public static String Obtener_ID_Consecutivo(String Tabla, String Campo, String Filtro, Int32 Longitud_ID)
        {
            String Id = Convertir_A_Formato_ID(1, Longitud_ID); ;
            try
            {
                String Mi_SQL = "SELECT MAX(" + Campo + ") FROM " + Tabla;
                if (Filtro != "" && Filtro != null)
                {
                    Mi_SQL += " WHERE " + Filtro;
                }
                Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals(""))
                {
                    Id = Convertir_A_Formato_ID((Convert.ToInt32(Obj_Temp) + 1), Longitud_ID);
                }
            }
            catch (Exception Ex)
            {
                new Exception(Ex.Message);
            }
            return Id;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Convertir_A_Formato_ID
        ///DESCRIPCIÓN: Pasa un numero entero a Formato de ID.
        ///PARAMETROS:     
        ///             1. Dato_ID. Dato que se desea pasar al Formato de ID.
        ///             2. Longitud_ID. Longitud que tendra el ID. 
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : Antonio Salvador Benavides Guardado
        ///FECHA_MODIFICO       : 26/Octubre/2010
        ///CAUSA_MODIFICACIÓN   : Estandarizar variables usadas
        ///*******************************************************************************
        private static String Convertir_A_Formato_ID(Int32 Dato_ID, Int32 Longitud_ID)
        {
            String Retornar = "";
            String Dato = "" + Dato_ID;
            for (int Cont_Temp = Dato.Length; Cont_Temp < Longitud_ID; Cont_Temp++)
            {
                Retornar = Retornar + "0";
            }
            Retornar = Retornar + Dato;
            return Retornar;
        }
    }
}