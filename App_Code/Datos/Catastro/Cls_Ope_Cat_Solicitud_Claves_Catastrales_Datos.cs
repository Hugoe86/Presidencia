using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Xml.Linq;
using Presidencia.Operacion_Cat_Solicitud_Claves_Catastrales.Negocio;
using Presidencia.Operacion_Cat_Solicitud_Claves_Catastrales.Datos;
using Presidencia.Catalogo_Cat_Claves_Catastrales.Datos;
using Presidencia.Catalogo_Cat_Claves_Catastrales.Negocio;
using Oracle.DataAccess;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;


/// <summary>
/// Summary description for Cls_Ope_Cat_Solicitud_Claves_Catastrales_Datos
/// </summary>
/// 
namespace Presidencia.Operacion_Cat_Solicitud_Claves_Catastrales.Datos
{
    public class Cls_Ope_Cat_Solicitud_Claves_Catastrales_Datos
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
        ///
        public static Boolean Alta_Solicitud_Claves_Catastrales(Cls_Ope_Cat_Solicitud_Claves_Catastrales_Negocio Documentos)
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
            String No_Claves_Catastrales = "";
            No_Claves_Catastrales = Obtener_ID_Consecutivo(Ope_Cat_Claves_Catastrales.Tabla_Ope_Cat_Claves_Catastrales,
                Ope_Cat_Claves_Catastrales.Campo_No_Claves_Catastrales, Ope_Cat_Claves_Catastrales.Campo_Anio + "=" + Documentos.P_Anio, 10);
            String No_Documento = Obtener_ID_Consecutivo(Ope_Cat_Doc_Clave_Catastral.Tabla_Ope_Cat_Doc_Clave_Catastral, Ope_Cat_Doc_Clave_Catastral.Campo_No_Documento,
                    Ope_Cat_Doc_Clave_Catastral.Campo_Anio_Documento + "=" + Documentos.P_Anio_Documento, 10);
            try
            {
                Mi_sql = "INSERT INTO " + Ope_Cat_Claves_Catastrales.Tabla_Ope_Cat_Claves_Catastrales;
                Mi_sql += "(" + Ope_Cat_Claves_Catastrales.Campo_No_Claves_Catastrales + ", ";
                Mi_sql += Ope_Cat_Claves_Catastrales.Campo_Anio + ", ";
                Mi_sql += Ope_Cat_Claves_Catastrales.Campo_Cantidad_Claves_Catastrales + ", ";
                Mi_sql += Ope_Cat_Claves_Catastrales.Campo_Estatus + ", ";
                Mi_sql += Ope_Cat_Claves_Catastrales.Campo_Solicitante + ", ";
                Mi_sql += Ope_Cat_Claves_Catastrales.Campo_Tipo + ", ";
                Mi_sql += Ope_Cat_Claves_Catastrales.Campo_Correo + ", ";
                Mi_sql += Ope_Cat_Claves_Catastrales.Campo_Observaciones + ", ";
                Mi_sql += Ope_Cat_Claves_Catastrales.Campo_Cuenta_Predial_Id + ", ";
                Mi_sql += Ope_Cat_Claves_Catastrales.Campo_Usuario_Creo + ", ";
                Mi_sql += Ope_Cat_Claves_Catastrales.Campo_Fecha_Creo + ") VALUES ('";
                Mi_sql += No_Claves_Catastrales + "', '";
                Mi_sql += Documentos.P_Anio + "', '";
                Mi_sql += Documentos.P_Cantidad_Claves_Catastrales + "', '";
                Mi_sql += Documentos.P_Estatus + "', '";
                Mi_sql += Documentos.P_Solicitante + "', '";
                Mi_sql += Documentos.P_Tipo + "', '";
                Mi_sql += Documentos.P_Correo + "', '";
                Mi_sql += Documentos.P_Observaciones + "', '";
                Mi_sql += Documentos.P_Cuenta_Predial_Id + "', '";
                Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_sql += "SYSDATE";
                Mi_sql += ")";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();
                Documentos.P_No_Claves_Catastrales = No_Claves_Catastrales;
                foreach (DataRow Dr_Renglon in Documentos.P_Dt_Archivos.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Doc_Clave_Catastral.Tabla_Ope_Cat_Doc_Clave_Catastral + "(";
                        Mi_sql += Ope_Cat_Doc_Clave_Catastral.Campo_No_Documento + ", ";
                        Mi_sql += Ope_Cat_Doc_Clave_Catastral.Campo_Anio_Documento + ", ";
                        Mi_sql += Ope_Cat_Doc_Clave_Catastral.Campo_No_Claves_Catastrales + ", ";
                        Mi_sql += Ope_Cat_Doc_Clave_Catastral.Campo_Anio + ", ";
                        Mi_sql += Ope_Cat_Doc_Clave_Catastral.Campo_Claves_Catastrales_id + ", ";
                        Mi_sql += Ope_Cat_Doc_Clave_Catastral.Campo_Ruta_Documento + ", ";
                        Mi_sql += Ope_Cat_Doc_Clave_Catastral.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Doc_Clave_Catastral.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Documento + "', ";
                        Mi_sql += Dr_Renglon[Ope_Cat_Doc_Clave_Catastral.Campo_Anio_Documento].ToString() + ", '";
                        Mi_sql += Documentos.P_No_Claves_Catastrales + "', ";
                        Mi_sql += Documentos.P_Anio + ", '";
                        Mi_sql += Dr_Renglon[Ope_Cat_Doc_Clave_Catastral.Campo_Claves_Catastrales_id].ToString() + "', '";
                        Mi_sql += "../Catastro/Archivos_CC/" + Documentos.P_Anio_Documento + "_" +Documentos.P_No_Claves_Catastrales 
                            + "/" + Dr_Renglon[Ope_Cat_Doc_Clave_Catastral.Campo_Ruta_Documento].ToString() + "', '";
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
                throw new Exception("Alta_Clave_Catastral: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Solicitud_Claves_Catastrales
        ///DESCRIPCIÓN: Modifica Una solicitud de claves catastrales
        ///PARAMENTROS:             
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        ///
        public static Boolean Modificar_Solicitud_Claves_Catastrales(Cls_Ope_Cat_Solicitud_Claves_Catastrales_Negocio Documentos)
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
                Mi_sql = "UPDATE " + Ope_Cat_Claves_Catastrales.Tabla_Ope_Cat_Claves_Catastrales;
                Mi_sql += " SET " + Ope_Cat_Claves_Catastrales.Campo_Observaciones + " = '" + Documentos.P_Observaciones + "', ";
                Mi_sql += Ope_Cat_Claves_Catastrales.Campo_Cantidad_Claves_Catastrales + " = '" + Documentos.P_Cantidad_Claves_Catastrales + "', ";
                Mi_sql += Ope_Cat_Claves_Catastrales.Campo_Estatus + " = '" + Documentos.P_Estatus + "', ";
                Mi_sql += Ope_Cat_Claves_Catastrales.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_sql += Ope_Cat_Claves_Catastrales.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_sql += " WHERE " + Ope_Cat_Claves_Catastrales.Campo_No_Claves_Catastrales + " = '" + Documentos.P_No_Claves_Catastrales + "'";
                Mi_sql += " AND " + Ope_Cat_Claves_Catastrales.Campo_Anio + " = '" + Documentos.P_Anio + "'";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Modificar_Clave_Catastral: " + E.Message);
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
        ///
        public static DataTable Consultar_Claves_Catastrales(Cls_Ope_Cat_Solicitud_Claves_Catastrales_Negocio Clave)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "SELECT " + Ope_Cat_Claves_Catastrales.Campo_No_Claves_Catastrales
                    + ", " + Ope_Cat_Claves_Catastrales.Campo_Anio
                    + ", " + Ope_Cat_Claves_Catastrales.Campo_Cantidad_Claves_Catastrales
                    + ", " + Ope_Cat_Claves_Catastrales.Campo_Estatus
                    + ", " + Ope_Cat_Claves_Catastrales.Campo_Observaciones
                    + ", " + Ope_Cat_Claves_Catastrales.Campo_Correo
                    + ", " + Ope_Cat_Claves_Catastrales.Campo_Tipo
                    + ", " + Ope_Cat_Claves_Catastrales.Campo_Solicitante
                    + ", " + Ope_Cat_Claves_Catastrales.Campo_Cuenta_Predial_Id
                    + ", (SELECT CP." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas
                    + " CP WHERE CP." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "=" + Ope_Cat_Claves_Catastrales.Tabla_Ope_Cat_Claves_Catastrales
                    + "." + Ope_Cat_Claves_Catastrales.Campo_Cuenta_Predial_Id + ") AS CUENTA_PREDIAL"
                    + ", " + Ope_Cat_Claves_Catastrales.Campo_Fecha_Creo
                    + ", " + Ope_Cat_Claves_Catastrales.Campo_Usuario_Creo
                    + " FROM  " + Ope_Cat_Claves_Catastrales.Tabla_Ope_Cat_Claves_Catastrales
                    + " WHERE ";
                if (Clave.P_No_Claves_Catastrales != null && Clave.P_No_Claves_Catastrales.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Claves_Catastrales.Campo_No_Claves_Catastrales + " = '" + Clave.P_No_Claves_Catastrales + "' AND ";
                }
                if (Clave.P_Anio != null && Clave.P_Anio.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Claves_Catastrales.Campo_Anio + " = " + Clave.P_Anio + " AND ";
                }
                if (Clave.P_Cantidad_Claves_Catastrales != null && Clave.P_Cantidad_Claves_Catastrales.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Claves_Catastrales.Campo_Cantidad_Claves_Catastrales + " = " + Clave.P_Cantidad_Claves_Catastrales + " AND ";
                }
                if (Clave.P_Solicitante != null && Clave.P_Solicitante.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Claves_Catastrales.Campo_Solicitante + " = '" + Clave.P_Solicitante + "' AND ";
                }
                if (Clave.P_Cuenta_Predial_Id != null && Clave.P_Cuenta_Predial_Id.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Claves_Catastrales.Campo_Cuenta_Predial_Id + " = '" + Clave.P_Cuenta_Predial_Id + "' AND ";
                }
                if (Clave.P_Cuenta_Predial != null && Clave.P_Cuenta_Predial.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Claves_Catastrales.Campo_Cuenta_Predial_Id + " IN (SELECT CP." + 
                        Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas 
                        + " CP WHERE CP." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " LIKE '%" + Clave.P_Cuenta_Predial + "%') AND ";
                }
                if (Clave.P_Estatus != null && Clave.P_Estatus.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Claves_Catastrales.Campo_Estatus + " " + Clave.P_Estatus + "";
                }
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                Mi_SQL += " ORDER BY " + Ope_Cat_Claves_Catastrales.Campo_No_Claves_Catastrales + " DESC";
                // AGREGA UN FILTRO PARA DARLE UNA ORDENACION A LA CONSULTA, 
                //EN ESTE CASO SE ORDENARA LA PRIMERA EN LLEGAR AL PRINCIPIO DE LA LISTA, 
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
        ///
        public static DataTable Consultar_Documentos_Claves_Catastrales(Cls_Ope_Cat_Solicitud_Claves_Catastrales_Negocio Clave)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "SELECT " + Ope_Cat_Doc_Clave_Catastral.Campo_No_Documento
                    + ", " + Ope_Cat_Doc_Clave_Catastral.Campo_Anio_Documento
                    + ", " + Ope_Cat_Doc_Clave_Catastral.Campo_No_Claves_Catastrales
                    + ", " + Ope_Cat_Doc_Clave_Catastral.Campo_Anio
                    + ", " + Ope_Cat_Doc_Clave_Catastral.Campo_Claves_Catastrales_id
                    + ", (SELECT RC." + Cat_Cat_Claves_Catastrales.Campo_Identificador
                        + " FROM " + Cat_Cat_Claves_Catastrales.Tabla_Cat_Cat_Claves_Catastrales
                        + " RC WHERE RC." + Cat_Cat_Claves_Catastrales.Campo_Claves_Catastrales_ID
                        + "=" + Ope_Cat_Doc_Clave_Catastral.Tabla_Ope_Cat_Doc_Clave_Catastral + "."
                        + Ope_Cat_Doc_Clave_Catastral.Campo_Claves_Catastrales_id + ") AS DOCUMENTO"
                    + ", " + Ope_Cat_Doc_Clave_Catastral.Campo_Ruta_Documento
                    + ", " + Ope_Cat_Doc_Clave_Catastral.Campo_Fecha_Creo
                    + ", " + Ope_Cat_Doc_Clave_Catastral.Campo_Usuario_Creo
                    + ", " + Ope_Cat_Doc_Clave_Catastral.Campo_Fecha_Modifico
                    + ", " + Ope_Cat_Doc_Clave_Catastral.Campo_Usuario_Modifico
                    + ", 'NADA' AS ACCION"
                    + " FROM  " + Ope_Cat_Doc_Clave_Catastral.Tabla_Ope_Cat_Doc_Clave_Catastral
                    + " WHERE ";
                if (Clave.P_Claves_Catastrales_Id != null && Clave.P_Claves_Catastrales_Id.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Doc_Clave_Catastral.Campo_Claves_Catastrales_id + " = '" + Clave.P_Claves_Catastrales_Id + "' AND ";
                }
                if (Clave.P_No_Documento != null && Clave.P_No_Documento.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Doc_Clave_Catastral.Campo_No_Documento + " = '" + Clave.P_No_Documento + "' AND ";
                }
                if (Clave.P_Anio_Documento != null && Clave.P_Anio_Documento.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Doc_Clave_Catastral.Campo_Anio_Documento + " = " + Clave.P_Anio_Documento + " AND ";
                }
                if (Clave.P_No_Claves_Catastrales != null && Clave.P_No_Claves_Catastrales.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Doc_Clave_Catastral.Campo_No_Claves_Catastrales + " = '" + Clave.P_No_Claves_Catastrales + "' AND ";
                }
                if (Clave.P_Anio != null && Clave.P_Anio.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Doc_Clave_Catastral.Campo_Anio + " = " + Clave.P_Anio + " AND ";
                }
                if (Clave.P_Ruta_Documento != null && Clave.P_Ruta_Documento.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Doc_Clave_Catastral.Campo_Ruta_Documento + " LIKE '%" + Clave.P_Ruta_Documento + "%' AND ";
                }
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                Mi_SQL += " ORDER BY " + Ope_Cat_Doc_Clave_Catastral.Campo_No_Documento + " DESC";
                // AGREGA UN FILTRO PARA DARLE UNA ORDENACION A LA CONSULTA, 
                //EN ESTE CASO SE ORDENARA LA PRIMERA EN LLEGAR AL PRINCIPIO DE LA LISTA,
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
        ///
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
        ///
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
