using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Operacion_Cat_Recepcion_Documentos_Perito_Externo.Negocio;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
using System.Data;
using System.Data.OracleClient;
using Presidencia.Sessiones;
using Presidencia.Bandeja_Solicitudes_Tramites.Negocio;

/// <summary>
/// Summary description for Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Datos
/// </summary>

namespace Presidencia.Operacion_Cat_Recepcion_Documentos_Perito_Externo.Datos
{
    public class Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Motivo_Avaluo
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
        public static Boolean Alta_Documentos(Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio Documentos)
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
            String No_Documento = "";
            No_Documento = Obtener_ID_Consecutivo(Ope_Cat_Doc_Perito_Externo.Tabla_Ope_Cat_Doc_Perito_Externo, Ope_Cat_Doc_Perito_Externo.Campo_No_Documento, "", 10);
            String Temp_Perito_Externo_Id = Obtener_ID_Consecutivo(Cat_Cat_Temp_Peritos_Externos.Tabla_Cat_Cat_Temp_Peritos_Externos, Cat_Cat_Temp_Peritos_Externos.Campo_Temp_Perito_Externo_Id, "", 5);
            try
            {
                Mi_sql = "INSERT INTO " + Cat_Cat_Temp_Peritos_Externos.Tabla_Cat_Cat_Temp_Peritos_Externos;
                Mi_sql += "(" + Cat_Cat_Temp_Peritos_Externos.Campo_Temp_Perito_Externo_Id + ", ";
                Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_Nombre + ", ";
                Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_Apellido_Paterno + ", ";
                Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_Apellido_Materno + ", ";
                Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_Calle + ", ";
                Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_Colonia + ", ";
                Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_Estado + ", ";
                Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_Ciudad + ", ";
                Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_Telefono + ", ";
                Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_Celular + ", ";
                Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_E_Mail + ", ";
                Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_Estatus + ", ";
                Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_Observaciones + ", ";
                Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_Solicitud_id + ", ";
                Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_Informacion + ", ";
                Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_Usuario_Creo + ", ";
                Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_Fecha_Creo + ") VALUES ('";
                Mi_sql += Temp_Perito_Externo_Id + "', '";
                Mi_sql += Documentos.P_Nombre + "', '";
                Mi_sql += Documentos.P_Apellido_Paterno + "', '";
                Mi_sql += Documentos.P_Apellido_Materno + "', '";
                Mi_sql += Documentos.P_Calle + "', '";
                Mi_sql += Documentos.P_Colonia + "', '";
                Mi_sql += Documentos.P_Estado + "', '";
                Mi_sql += Documentos.P_Ciudad + "', '";
                Mi_sql += Documentos.P_Telefono + "', '";
                Mi_sql += Documentos.P_Celular + "', '";
                Mi_sql += Documentos.P_E_Mail + "', '";
                Mi_sql += Documentos.P_Estatus + "', '";
                Mi_sql += Documentos.P_Observaciones + "', '";
                Mi_sql += Documentos.P_Solicitud_Id + "', '";
                Mi_sql += Documentos.P_Informacion + "', '";
                Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_sql += "SYSDATE";
                Mi_sql += ")";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();
                Documentos.P_Temp_Perito_Externo_Id = Temp_Perito_Externo_Id;

                foreach (DataRow Dr_Renglon in Documentos.P_Dt_Archivos.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Doc_Perito_Externo.Tabla_Ope_Cat_Doc_Perito_Externo + "(";
                        Mi_sql += Ope_Cat_Doc_Perito_Externo.Campo_No_Documento + ", ";
                        Mi_sql += Ope_Cat_Doc_Perito_Externo.Campo_Temp_Perito_Externo_Id + ", ";
                        Mi_sql += Ope_Cat_Doc_Perito_Externo.Campo_Documento + ", ";
                        Mi_sql += Ope_Cat_Doc_Perito_Externo.Campo_Ruta_Documento + ", ";
                        Mi_sql += Ope_Cat_Doc_Perito_Externo.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Doc_Perito_Externo.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Documento + "', '";
                        Mi_sql += Documentos.P_Temp_Perito_Externo_Id + "', '";
                        Mi_sql += Dr_Renglon[Ope_Cat_Doc_Perito_Externo.Campo_Documento].ToString() + "', '";
                        Mi_sql += "../Catastro/Archivos/" + Documentos.P_Temp_Perito_Externo_Id + "/" + DateTime.Now.Year + "/" + Dr_Renglon[Ope_Cat_Doc_Perito_Externo.Campo_Ruta_Documento].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                        No_Documento = (Convert.ToInt32(No_Documento) + 1).ToString("0000000000");
                    }
                    //else if (Dr_Renglon["ACCION"].ToString() == "BAJA" && Dr_Renglon["NO_DOCUMENTO"].ToString().Trim().Replace("&nbsp;", "") != "")
                    //{
                    //    Mi_sql = "DELETE " + Ope_Cat_Doc_Perito_Externo.Tabla_Ope_Cat_Doc_Perito_Externo + " WHERE " + Ope_Cat_Doc_Perito_Externo.Campo_No_Documento;
                    //    Mi_sql += "='" + Dr_Renglon[Ope_Cat_Doc_Perito_Externo.Campo_No_Documento].ToString() + "'";
                    //    Cmd.CommandText = Mi_sql;
                    //    Cmd.ExecuteNonQuery();
                    //}
                }

                Cls_Ope_Bandeja_Tramites_Negocio Neg_Actualizar_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
                System.Text.StringBuilder My_SQL = new System.Text.StringBuilder();
                My_SQL.Append("SELECT " + Ope_Tra_Solicitud.Campo_Solicitud_ID
                    + "," + Ope_Tra_Solicitud.Campo_Subproceso_ID
                    + "," + Ope_Tra_Solicitud.Campo_Estatus
                    + " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud
                    + " WHERE UPPER(TRIM(" + Ope_Tra_Solicitud.Campo_Solicitud_ID + ")) = UPPER(TRIM('" + Documentos.P_Solicitud_Id + "'))");
                Cmd.CommandText = My_SQL.ToString();
                Cmd.CommandType = CommandType.Text;
                OracleDataReader Dtr_Datos_Solicitud = Cmd.ExecuteReader();

                // si hay datos para leer, agregar pasivo
                if (Dtr_Datos_Solicitud.Read())
                {
                    Neg_Actualizar_Solicitud.P_Comando_Oracle = Cmd;
                    // establecer parámetros para actualizar solicitud
                    Neg_Actualizar_Solicitud.P_Solicitud_ID = Dtr_Datos_Solicitud[Ope_Tra_Solicitud.Campo_Solicitud_ID].ToString().Trim();
                    Neg_Actualizar_Solicitud.P_Estatus = "APROBAR";
                    Neg_Actualizar_Solicitud.P_Subproceso_ID = Dtr_Datos_Solicitud[Ope_Tra_Solicitud.Campo_Subproceso_ID].ToString().Trim();
                    Neg_Actualizar_Solicitud.P_Comentarios = "SOLICITUD REGISTRADA";
                    Neg_Actualizar_Solicitud.P_Usuario = Cls_Sessiones.Nombre_Ciudadano;
                    // llamar método que actualizar la solicitud
                    Neg_Actualizar_Solicitud = Neg_Actualizar_Solicitud.Evaluar_Solicitud();
                }
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Alta_Documentos: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Documentos_Perito_Externo
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
        public static Boolean Alta_Documentos_Perito_Externo(Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio Documentos)
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
            String No_Documento = "";
            No_Documento = Obtener_ID_Consecutivo(Ope_Cat_Doc_Peritos_Vigentes.Tabla_Ope_Cat_Doc_Peritos_Vigentes, Ope_Cat_Doc_Peritos_Vigentes.Campo_No_Documento, "", 10);
            try
            {
                foreach (DataRow Dr_Renglon in Documentos.P_Dt_Archivos.Rows)
                {
                    Mi_sql = "INSERT INTO " + Ope_Cat_Doc_Peritos_Vigentes.Tabla_Ope_Cat_Doc_Peritos_Vigentes + "(";
                    Mi_sql += Ope_Cat_Doc_Peritos_Vigentes.Campo_No_Documento + ", ";
                    Mi_sql += Ope_Cat_Doc_Peritos_Vigentes.Campo_Perito_Externo_Id + ", ";
                    Mi_sql += Ope_Cat_Doc_Peritos_Vigentes.Campo_Documento + ", ";
                    Mi_sql += Ope_Cat_Doc_Peritos_Vigentes.Campo_Ruta_Documento + ", ";
                    Mi_sql += Ope_Cat_Doc_Peritos_Vigentes.Campo_Usuario_Creo + ", ";
                    Mi_sql += Ope_Cat_Doc_Peritos_Vigentes.Campo_Fecha_Creo;
                    Mi_sql += ") VALUES ('";
                    Mi_sql += No_Documento + "', '";
                    Mi_sql += Documentos.P_Perito_Externo_Id + "', '";
                    Mi_sql += Dr_Renglon[Ope_Cat_Doc_Peritos_Vigentes.Campo_Documento].ToString() + "', '";
                    Mi_sql += "../Catastro/Archivos_Peritos_Externos/" + Documentos.P_Perito_Externo_Id + "/" + DateTime.Now.Year + "/" + Dr_Renglon[Ope_Cat_Doc_Perito_Externo.Campo_Ruta_Documento].ToString() + "', '";
                    Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                    Mi_sql += "SYSDATE";
                    Mi_sql += ")";
                    Cmd.CommandText = Mi_sql;
                    Cmd.ExecuteNonQuery();
                    No_Documento = (Convert.ToInt32(No_Documento) + 1).ToString("0000000000");
                }
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Alta_Documentos_Perito_Externo: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Peritos_Temporales
        ///DESCRIPCIÓN: Elimina los documentos y la información temporal del perito externo.
        ///PARAMENTROS:     
        ///             1. Documentos.     Instancia de la Clase de Negocio de Recepcion de documentos
        ///                                 con los datos del que van a ser
        ///                                 eliminados.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Eliminar_Peritos_Temporales(Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio Documentos)
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

                Mi_sql = "DELETE " + Ope_Cat_Doc_Perito_Externo.Tabla_Ope_Cat_Doc_Perito_Externo;
                Mi_sql += " WHERE " + Ope_Cat_Doc_Perito_Externo.Campo_Temp_Perito_Externo_Id + "= '" + Documentos.P_Temp_Perito_Externo_Id + "'";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();

                Mi_sql = "DELETE " + Cat_Cat_Temp_Peritos_Externos.Tabla_Cat_Cat_Temp_Peritos_Externos;
                Mi_sql += " WHERE " + Cat_Cat_Temp_Peritos_Externos.Campo_Temp_Perito_Externo_Id + "= '" + Documentos.P_Temp_Perito_Externo_Id + "'";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();

                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Eliminar_Peritos_Temporales: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Motivo_Avaluo
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
        public static Boolean Alta_Documentos_Refrendo(Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio Documentos)
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
            String No_Documento = "";
            No_Documento = Obtener_ID_Consecutivo(Ope_Cat_Doc_Perito_Externo.Tabla_Ope_Cat_Doc_Perito_Externo, Ope_Cat_Doc_Perito_Externo.Campo_No_Documento, "", 10);
            try
            {
                foreach (DataRow Dr_Renglon in Documentos.P_Dt_Archivos.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Doc_Perito_Externo.Tabla_Ope_Cat_Doc_Perito_Externo + "(";
                        Mi_sql += Ope_Cat_Doc_Perito_Externo.Campo_No_Documento + ", ";
                        Mi_sql += Ope_Cat_Doc_Perito_Externo.Campo_Temp_Perito_Externo_Id + ", ";
                        Mi_sql += Ope_Cat_Doc_Perito_Externo.Campo_Documento + ", ";
                        Mi_sql += Ope_Cat_Doc_Perito_Externo.Campo_Ruta_Documento + ", ";
                        Mi_sql += Ope_Cat_Doc_Perito_Externo.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Doc_Perito_Externo.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Documento + "', '";
                        Mi_sql += Documentos.P_Temp_Perito_Externo_Id + "', '";
                        Mi_sql += Dr_Renglon[Ope_Cat_Doc_Perito_Externo.Campo_Documento].ToString() + "', '";
                        Mi_sql += "../Catastro/Archivos/" + Documentos.P_Temp_Perito_Externo_Id + "/" + DateTime.Now.Year + "/" + Dr_Renglon[Ope_Cat_Doc_Perito_Externo.Campo_Ruta_Documento].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                        No_Documento = (Convert.ToInt32(No_Documento) + 1).ToString("0000000000");
                    }
                    //else if (Dr_Renglon["ACCION"].ToString() == "BAJA" && Dr_Renglon["NO_DOCUMENTO"].ToString().Trim().Replace("&nbsp;", "") != "")
                    //{
                    //    Mi_sql = "DELETE " + Ope_Cat_Doc_Perito_Externo.Tabla_Ope_Cat_Doc_Perito_Externo + " WHERE " + Ope_Cat_Doc_Perito_Externo.Campo_No_Documento;
                    //    Mi_sql += "='" + Dr_Renglon[Ope_Cat_Doc_Perito_Externo.Campo_No_Documento].ToString() + "'";
                    //    Cmd.CommandText = Mi_sql;
                    //    Cmd.ExecuteNonQuery();
                    //}
                }
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Alta_Documentos: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Estatus_Perito_Externo_Temporal
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
        public static Boolean Modificar_Estatus_Perito_Externo_Temporal(Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio Documentos)
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
                Mi_sql = "UPDATE " + Cat_Cat_Temp_Peritos_Externos.Tabla_Cat_Cat_Temp_Peritos_Externos + " SET " +
                    Cat_Cat_Temp_Peritos_Externos.Campo_Estatus + "='" + Documentos.P_Estatus + "' WHERE " +
                    Cat_Cat_Temp_Peritos_Externos.Campo_Temp_Perito_Externo_Id +
                    " = '" + Documentos.P_Temp_Perito_Externo_Id + "'";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();
                Alta = true;

                if (Documentos.P_Estatus == "POR PAGAR")
                {

                    Cls_Ope_Bandeja_Tramites_Negocio Neg_Actualizar_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
                    System.Text.StringBuilder My_SQL = new System.Text.StringBuilder();
                    My_SQL.Append("SELECT " + Ope_Tra_Solicitud.Campo_Solicitud_ID
                        + "," + Ope_Tra_Solicitud.Campo_Subproceso_ID
                        + "," + Ope_Tra_Solicitud.Campo_Estatus
                        + " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud
                        + " WHERE UPPER(TRIM(" + Ope_Tra_Solicitud.Campo_Solicitud_ID + ")) = UPPER(TRIM('" + Documentos.P_Solicitud_Id + "'))");
                    Cmd.CommandText = My_SQL.ToString();
                    Cmd.CommandType = CommandType.Text;
                    OracleDataReader Dtr_Datos_Solicitud = Cmd.ExecuteReader();

                    // si hay datos para leer, agregar pasivo
                    if (Dtr_Datos_Solicitud.Read())
                    {
                        Neg_Actualizar_Solicitud.P_Comando_Oracle = Cmd;
                        // establecer parámetros para actualizar solicitud
                        Neg_Actualizar_Solicitud.P_Solicitud_ID = Dtr_Datos_Solicitud[Ope_Tra_Solicitud.Campo_Solicitud_ID].ToString().Trim();
                        Neg_Actualizar_Solicitud.P_Estatus = "APROBAR";
                        Neg_Actualizar_Solicitud.P_Subproceso_ID = Dtr_Datos_Solicitud[Ope_Tra_Solicitud.Campo_Subproceso_ID].ToString().Trim();
                        Neg_Actualizar_Solicitud.P_Comentarios = "SOLICITUD AUTORIZADO";
                        Neg_Actualizar_Solicitud.P_Usuario = Cls_Sessiones.Nombre_Ciudadano;
                        // llamar método que actualizar la solicitud
                        Neg_Actualizar_Solicitud = Neg_Actualizar_Solicitud.Evaluar_Solicitud();
                    }
                }
                else if (Documentos.P_Estatus == "BAJA")
                {
                    Cls_Ope_Bandeja_Tramites_Negocio Neg_Actualizar_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
                    System.Text.StringBuilder My_SQL = new System.Text.StringBuilder();
                    My_SQL.Append("SELECT " + Ope_Tra_Solicitud.Campo_Solicitud_ID
                        + "," + Ope_Tra_Solicitud.Campo_Subproceso_ID
                        + "," + Ope_Tra_Solicitud.Campo_Estatus
                        + " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud
                        + " WHERE UPPER(TRIM(" + Ope_Tra_Solicitud.Campo_Solicitud_ID + ")) = UPPER(TRIM('" + Documentos.P_Solicitud_Id + "'))");
                    Cmd.CommandText = My_SQL.ToString();
                    Cmd.CommandType = CommandType.Text;
                    OracleDataReader Dtr_Datos_Solicitud = Cmd.ExecuteReader();

                    // si hay datos para leer, agregar pasivo
                    if (Dtr_Datos_Solicitud.Read())
                    {
                        Neg_Actualizar_Solicitud.P_Comando_Oracle = Cmd;
                        // establecer parámetros para actualizar solicitud
                        Neg_Actualizar_Solicitud.P_Solicitud_ID = Dtr_Datos_Solicitud[Ope_Tra_Solicitud.Campo_Solicitud_ID].ToString().Trim();
                        Neg_Actualizar_Solicitud.P_Estatus = "CANCELAR";
                        Neg_Actualizar_Solicitud.P_Subproceso_ID = Dtr_Datos_Solicitud[Ope_Tra_Solicitud.Campo_Subproceso_ID].ToString().Trim();
                        Neg_Actualizar_Solicitud.P_Comentarios = "SOLICITUD CANCELADA";
                        Neg_Actualizar_Solicitud.P_Usuario = Cls_Sessiones.Nombre_Ciudadano;
                        // llamar método que actualizar la solicitud
                        Neg_Actualizar_Solicitud = Neg_Actualizar_Solicitud.Evaluar_Solicitud();
                    }
                }
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Modificar_Estatus_Perito_Externo_Temporal: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Perito_Externo_Est
        ///DESCRIPCIÓN: Modifica un Perito Externo.
        ///PARAMENTROS:     
        ///             1. Perito_Ext.      Instancia de la Clase de Negocio de Peritos Externos
        ///                                 con los datos del que van a ser
        ///                                 modificados.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 14/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Modificar_Perito_Externo_Est(Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio Perito_Ext)
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

                Mi_sql = "UPDATE " + Cat_Cat_Peritos_Externos.Tabla_Cat_Cat_Peritos_Externos;
                Mi_sql += " SET " + Cat_Cat_Peritos_Externos.Campo_Estatus + " = '" + Perito_Ext.P_Estatus + "', ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_sql += " WHERE " + Cat_Cat_Peritos_Externos.Campo_Perito_Externo_Id + " = '" + Perito_Ext.P_Perito_Externo_Id + "'";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Modificar_Perito_Externo: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Motivos_Avaluo
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
        public static DataTable Consultar_Documentos(Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio Motivo_Avaluo)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";

            try
            {
                Mi_SQL = "SELECT " + Ope_Cat_Doc_Perito_Externo.Campo_No_Documento
                    + ", " + Ope_Cat_Doc_Perito_Externo.Campo_Temp_Perito_Externo_Id
                    + ", " + Ope_Cat_Doc_Perito_Externo.Campo_Documento
                    + ", " + Ope_Cat_Doc_Perito_Externo.Campo_Ruta_Documento
                    + ", " + Ope_Cat_Doc_Perito_Externo.Campo_Fecha_Creo
                    + ", " + Ope_Cat_Doc_Perito_Externo.Campo_Usuario_Creo
                    + ", " + Ope_Cat_Doc_Perito_Externo.Campo_Fecha_Modifico
                    + ", " + Ope_Cat_Doc_Perito_Externo.Campo_Usuario_Modifico
                    + ", 'NADA' AS ACCION"
                    + " FROM  " + Ope_Cat_Doc_Perito_Externo.Tabla_Ope_Cat_Doc_Perito_Externo
                    + " WHERE ";
                if (Motivo_Avaluo.P_Temp_Perito_Externo_Id != null && Motivo_Avaluo.P_Temp_Perito_Externo_Id.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Doc_Perito_Externo.Campo_Temp_Perito_Externo_Id + " = '" + Motivo_Avaluo.P_Temp_Perito_Externo_Id + "' AND ";
                }
                if (Motivo_Avaluo.P_No_Documento != null && Motivo_Avaluo.P_No_Documento.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Doc_Perito_Externo.Campo_No_Documento + " = '" + Motivo_Avaluo.P_No_Documento + "' AND ";
                }
                if (Motivo_Avaluo.P_Documento != null && Motivo_Avaluo.P_Documento.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Doc_Perito_Externo.Campo_Documento + " = '" + Motivo_Avaluo.P_Documento + "' AND ";
                }
                if (Motivo_Avaluo.P_Ruta_Documento != null && Motivo_Avaluo.P_Ruta_Documento.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Doc_Perito_Externo.Campo_Ruta_Documento + " LIKE '%" + Motivo_Avaluo.P_Ruta_Documento + "%' AND ";
                }
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                Mi_SQL += " ORDER BY " + Ope_Cat_Doc_Perito_Externo.Campo_Documento + " DESC";
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Documentos_Perito_Externo
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
        public static DataTable Consultar_Documentos_Perito_Externo(Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio Motivo_Avaluo)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";


            try
            {
                Mi_SQL = "SELECT " + Ope_Cat_Doc_Peritos_Vigentes.Campo_No_Documento
                    + ", " + Ope_Cat_Doc_Peritos_Vigentes.Campo_Perito_Externo_Id
                    + ", " + Ope_Cat_Doc_Peritos_Vigentes.Campo_Documento
                    + ", " + Ope_Cat_Doc_Peritos_Vigentes.Campo_Ruta_Documento
                    + ", " + Ope_Cat_Doc_Peritos_Vigentes.Campo_Fecha_Creo
                    + ", " + Ope_Cat_Doc_Peritos_Vigentes.Campo_Usuario_Creo
                    + ", " + Ope_Cat_Doc_Peritos_Vigentes.Campo_Fecha_Modifico
                    + ", " + Ope_Cat_Doc_Peritos_Vigentes.Campo_Usuario_Modifico
                    + ", 'NADA' AS ACCION"
                    + " FROM  " + Ope_Cat_Doc_Peritos_Vigentes.Tabla_Ope_Cat_Doc_Peritos_Vigentes
                    + " WHERE ";
                if (Motivo_Avaluo.P_Perito_Externo_Id != null && Motivo_Avaluo.P_Perito_Externo_Id.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Doc_Peritos_Vigentes.Campo_Perito_Externo_Id + " = '" + Motivo_Avaluo.P_Perito_Externo_Id + "' AND ";
                }
                if (Motivo_Avaluo.P_No_Documento != null && Motivo_Avaluo.P_No_Documento.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Doc_Peritos_Vigentes.Campo_No_Documento + " = '" + Motivo_Avaluo.P_No_Documento + "' AND ";
                }
                if (Motivo_Avaluo.P_Documento != null && Motivo_Avaluo.P_Documento.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Doc_Peritos_Vigentes.Campo_Documento + " = '" + Motivo_Avaluo.P_Documento + "' AND ";
                }
                if (Motivo_Avaluo.P_Ruta_Documento != null && Motivo_Avaluo.P_Ruta_Documento.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Doc_Peritos_Vigentes.Campo_Ruta_Documento + " LIKE '%" + Motivo_Avaluo.P_Ruta_Documento + "%' AND ";
                }
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                Mi_SQL += " ORDER BY " + Ope_Cat_Doc_Peritos_Vigentes.Campo_Documento + " DESC";
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Peritos_Externos_Temporales
        ///DESCRIPCIÓN: Obtiene la tabla de Peritos Externos Temporales
        ///PARAMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 10/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Peritos_Externos_Temporales(Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio Perito_Externo)
        {
            DataTable Tabla = new DataTable();
            String Mi_sql = "";

            try
            {
                Mi_sql = "SELECT " + Cat_Cat_Temp_Peritos_Externos.Campo_Temp_Perito_Externo_Id + ", ";
                Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_Nombre + ", ";
                Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_Apellido_Paterno + ", ";
                Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_Apellido_Materno + ", ";
                Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_Apellido_Paterno + "||' '||" + Cat_Cat_Temp_Peritos_Externos.Campo_Apellido_Materno + "||' '||" + Cat_Cat_Temp_Peritos_Externos.Campo_Nombre + " AS PERITO_EXTERNO, ";
                Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_Calle + ", ";
                Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_Colonia + ", ";
                Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_Estado + ", ";
                Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_Ciudad + ", ";
                Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_Telefono + ", ";
                Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_Celular + ", ";
                Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_E_Mail + ", ";
                Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_Estatus + ", ";
                Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_Observaciones + ", ";
                Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_Solicitud_id + ", ";
                Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_Informacion + ", ";
                Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_Usuario_Creo + ", ";
                Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_Fecha_Creo;
                Mi_sql += " FROM  " + Cat_Cat_Temp_Peritos_Externos.Tabla_Cat_Cat_Temp_Peritos_Externos;
                Mi_sql += " WHERE ";
                if (Perito_Externo.P_Temp_Perito_Externo_Id != null && Perito_Externo.P_Temp_Perito_Externo_Id.Trim() != "")
                {
                    Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_Temp_Perito_Externo_Id + "='" + Perito_Externo.P_Temp_Perito_Externo_Id + "' AND ";
                }
                if (Perito_Externo.P_E_Mail != null && Perito_Externo.P_E_Mail.Trim() != "")
                {
                    Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_E_Mail + "='" + Perito_Externo.P_E_Mail + "' AND ";
                }
                if (Perito_Externo.P_Estatus != null && Perito_Externo.P_Estatus.Trim() != "")
                {
                    Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_Estatus + " " + Perito_Externo.P_Estatus + " AND ";
                }
                if (Perito_Externo.P_Nombre != null && Perito_Externo.P_Nombre.Trim() != "")
                {
                    //Validar con el nombre completo que vendrá cargado en la variable P_Nombre.
                    Mi_sql += Cat_Cat_Temp_Peritos_Externos.Campo_Apellido_Paterno + "||' '||" + Cat_Cat_Temp_Peritos_Externos.Campo_Apellido_Materno + "||' '||" + Cat_Cat_Temp_Peritos_Externos.Campo_Nombre + " LIKE '%" + Perito_Externo.P_Nombre + "%' AND ";
                }
                if (Mi_sql.EndsWith(" AND "))
                {
                    Mi_sql = Mi_sql.Substring(0, Mi_sql.Length - 5);
                }
                if (Mi_sql.EndsWith(" WHERE "))
                {
                    Mi_sql = Mi_sql.Substring(0, Mi_sql.Length - 7);
                }
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_sql);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los Peritos externos del sistema. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Peritos_Externos_Temporales
        ///DESCRIPCIÓN: Obtiene la tabla de Peritos Externos Temporales
        ///PARAMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 10/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Peritos_Externos(Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio Perito_Externo)
        {
            DataTable Tabla = new DataTable();
            String Mi_sql = "";

            try
            {
                Mi_sql = "SELECT " + Cat_Cat_Peritos_Externos.Campo_Perito_Externo_Id + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Nombre + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Apellido_Paterno + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Apellido_Materno + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Apellido_Paterno + "||' '||" + Cat_Cat_Peritos_Externos.Campo_Apellido_Materno + "||' '||" + Cat_Cat_Peritos_Externos.Campo_Nombre + " AS PERITO_EXTERNO, ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Calle + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Colonia + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Estado + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Ciudad + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Telefono + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Celular + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Usuario + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Estatus + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Observaciones + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Informacion + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Usuario_Creo + ", ";
                Mi_sql += Cat_Cat_Peritos_Externos.Campo_Fecha_Creo;
                Mi_sql += " FROM  " + Cat_Cat_Peritos_Externos.Tabla_Cat_Cat_Peritos_Externos;
                Mi_sql += " WHERE ";
                if (Perito_Externo.P_Perito_Externo_Id != null && Perito_Externo.P_Perito_Externo_Id.Trim() != "")
                {
                    Mi_sql += Cat_Cat_Peritos_Externos.Campo_Perito_Externo_Id + "='" + Perito_Externo.P_Perito_Externo_Id + "' AND ";
                }
                if (Perito_Externo.P_E_Mail != null && Perito_Externo.P_E_Mail.Trim() != "")
                {
                    Mi_sql += Cat_Cat_Peritos_Externos.Campo_Usuario + "='" + Perito_Externo.P_E_Mail + "' AND ";
                }
                if (Perito_Externo.P_Estatus != null && Perito_Externo.P_Estatus.Trim() != "")
                {
                    Mi_sql += Cat_Cat_Peritos_Externos.Campo_Estatus + " " + Perito_Externo.P_Estatus + " AND ";
                }
                if (Perito_Externo.P_Nombre != null && Perito_Externo.P_Nombre.Trim() != "")
                {
                    //Validar con el nombre completo que vendrá cargado en la variable P_Nombre.
                    Mi_sql += Cat_Cat_Peritos_Externos.Campo_Apellido_Paterno + "||' '||" + Cat_Cat_Peritos_Externos.Campo_Apellido_Materno + "||' '||" + Cat_Cat_Peritos_Externos.Campo_Nombre + " LIKE '%" + Perito_Externo.P_Nombre + "%' AND ";
                }
                if (Mi_sql.EndsWith(" AND "))
                {
                    Mi_sql = Mi_sql.Substring(0, Mi_sql.Length - 5);
                }
                if (Mi_sql.EndsWith(" WHERE "))
                {
                    Mi_sql = Mi_sql.Substring(0, Mi_sql.Length - 7);
                }
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_sql);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los Peritos externos del sistema. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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