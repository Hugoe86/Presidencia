using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Ordenamiento_Territorial_Ficha_Revision.Negocio;

/// <summary>
/// Summary description for Cls_Ope_Ort_Ficha_Revision_Datos
/// </summary>
namespace Presidencia.Ordenamiento_Territorial_Ficha_Revision.Datos
{
    public class Cls_Ope_Ort_Ficha_Revision_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Ficha_Revision
        ///DESCRIPCIÓN          : Da de alta en la Base de Datos una nuevo registro
        ///PARAMETROS           : 1. Parametros. Contiene los parametros que se van a dar de
        ///                       Alta en la Base de Datos.
        ///CREO                 : Salvador Vázquez Camacho
        ///FECHA_CREO           : 30/Julio/2010 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static void Alta_Ficha_Revision(Cls_Ope_Ort_Ficha_Revision_Negocio Parametros)
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
            Parametros.P_Solicitud_Interna_ID = Obtener_ID_Consecutivo(Ope_Ort_Ficha_Revision.Tabla_Ope_Ort_Ficha_Revision, Ope_Ort_Ficha_Revision.Campo_Solicitud_Interna_ID, 10);
            try
            {
                String Mi_SQL = "INSERT INTO " + Ope_Ort_Ficha_Revision.Tabla_Ope_Ort_Ficha_Revision;
                Mi_SQL += " (" + Ope_Ort_Ficha_Revision.Campo_Solicitud_Interna_ID;
                Mi_SQL += ", " + Ope_Ort_Ficha_Revision.Campo_Solicitud_ID;
                Mi_SQL += ", " + Ope_Ort_Ficha_Revision.Campo_Zona_ID;
                Mi_SQL += ", " + Ope_Ort_Ficha_Revision.Campo_Area_ID;
                Mi_SQL += ", " + Ope_Ort_Ficha_Revision.Campo_Observacion;
                Mi_SQL += ", " + Ope_Ort_Ficha_Revision.Campo_Fecha_Solicitud;
                Mi_SQL += ", " + Ope_Ort_Ficha_Revision.Campo_Usuario_Creo;
                Mi_SQL += ", " + Ope_Ort_Ficha_Revision.Campo_Fecha_Creo;
                Mi_SQL += ") VALUES ('" + Parametros.P_Solicitud_Interna_ID;
                Mi_SQL += "', '" + Parametros.P_Solicitud_ID;
                Mi_SQL += "', '" + Parametros.P_Zona_ID;
                Mi_SQL += "', '" + Parametros.P_Area_ID;
                Mi_SQL += "', '" + Parametros.P_Observacion;
                Mi_SQL += "', SYSDATE";
                Mi_SQL += " , '" + Parametros.P_Usuario_Creo;
                Mi_SQL += "', SYSDATE )";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            }
            catch (OracleException Ex)
            {
                Trans.Rollback();
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
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
                    Mensaje = "Error al intentar dar de Alta. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Tarjetas_Gasolina
        ///DESCRIPCIÓN          : Modifica en la Base de Datos el registro indicado
        ///PARAMETROS           : 
        ///                     1.Parametros. Contiene los parametros que se van hacer la
        ///                       Modificación en la Base de Datos.
        ///CREO                 : Salvador Vázquez Camacho
        ///FECHA_CREO           : 30/Julio/2010
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static void Modificar_Ficha_Revision(Cls_Ope_Ort_Ficha_Revision_Negocio Parametros)
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
                String Mi_SQL = "UPDATE " + Ope_Ort_Ficha_Revision.Tabla_Ope_Ort_Ficha_Revision;
                Mi_SQL += " SET " + Ope_Ort_Ficha_Revision.Campo_Solicitud_ID + " = '" + Parametros.P_Solicitud_ID;
                Mi_SQL += "', " + Ope_Ort_Ficha_Revision.Campo_Zona_ID + " = '" + Parametros.P_Zona_ID;
                Mi_SQL += "', " + Ope_Ort_Ficha_Revision.Campo_Area_ID + " = '" + Parametros.P_Area_ID;
                Mi_SQL += "', " + Ope_Ort_Ficha_Revision.Campo_Observacion + " = '" + Parametros.P_Observacion;
                Mi_SQL += "', " + Ope_Ort_Ficha_Revision.Campo_Respuesta + " = '" + Parametros.P_Respuesta;
                if (!Parametros.P_Fecha_Solicitud.ToString().Contains("01/01/0001"))
                    Mi_SQL += "', " + Ope_Ort_Ficha_Revision.Campo_Fecha_Solicitud + " = '" + Parametros.P_Fecha_Solicitud.Day + "/" + Parametros.P_Fecha_Solicitud.Month + "/" + Parametros.P_Fecha_Solicitud.Year;
                if (!Parametros.P_Fecha_Respuesta.ToString().Contains("01/01/0001"))
                    Mi_SQL += "', " + Ope_Ort_Ficha_Revision.Campo_Fecha_Respuesta + " = '" + Parametros.P_Fecha_Respuesta.Day + "/" + Parametros.P_Fecha_Respuesta.Month + "/" + Parametros.P_Fecha_Respuesta.Year;
                Mi_SQL += "', " + Ope_Ort_Ficha_Revision.Campo_Usuario_Modifico + " = '" + Parametros.P_Usuario_Modifico;
                Mi_SQL += "', " + Ope_Ort_Ficha_Revision.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL += " WHERE " + Ope_Ort_Ficha_Revision.Campo_Solicitud_Interna_ID + " = '" + Parametros.P_Solicitud_Interna_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
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
                    Mensaje = "Error al intentar Modificar. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Tarjetas_Gasolina
        ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable.
        ///PARAMETROS           : 
        ///                     1.Parametros.Contiene los parametros que se van a utilizar para
        ///                       hacer la consulta de la Base de Datos.
        ///CREO                 : Salvador Vázquez Camacho
        ///FECHA_CREO           : 30/Julio/2010
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Tabla_Ficha_Revision(Cls_Ope_Ort_Ficha_Revision_Negocio Parametros)
        {
            String Mi_SQL = null;
            DataSet Ds_Datos = null;
            DataTable Dt_Datos = new DataTable();
            Boolean Entro_Where = false;
            try
            {
                Mi_SQL = "SELECT FICHA_REVISION." + Ope_Ort_Ficha_Revision.Campo_Solicitud_Interna_ID + " AS SOLICITUD_INTERNA_ID";
                Mi_SQL += ", FICHA_REVISION." + Ope_Ort_Ficha_Revision.Campo_Solicitud_ID + " AS SOLICITUD_ID";
                Mi_SQL += ", FICHA_REVISION." + Ope_Ort_Ficha_Revision.Campo_Zona_ID + " AS ZONA_ID";
                Mi_SQL += ", FICHA_REVISION." + Ope_Ort_Ficha_Revision.Campo_Area_ID + " AS AREA_ID";
                Mi_SQL += ", FICHA_REVISION." + Ope_Ort_Ficha_Revision.Campo_Observacion + " AS OBSERVACION";
                Mi_SQL += ", FICHA_REVISION." + Ope_Ort_Ficha_Revision.Campo_Respuesta + " AS RESPUESTA";
                Mi_SQL += ", FICHA_REVISION." + Ope_Ort_Ficha_Revision.Campo_Fecha_Solicitud + " AS FECHA_SOLICITUD";
                Mi_SQL += ", FICHA_REVISION." + Ope_Ort_Ficha_Revision.Campo_Fecha_Respuesta + " AS FECHA_RESPUESTA";
                Mi_SQL += ", FICHA_REVISION." + Ope_Ort_Ficha_Revision.Campo_Usuario_Creo + " AS CREO";
                Mi_SQL += ", FICHA_REVISION." + Ope_Ort_Ficha_Revision.Campo_Usuario_Modifico + " AS MODIFICO";
                Mi_SQL += ", AREA." + Cat_Areas.Campo_Nombre + " AS AREA";
                Mi_SQL += ", ZONA." + Cat_Ort_Zona.Campo_Nombre + " AS ZONA";
                Mi_SQL += ", SOLICITUD." + Ope_Tra_Solicitud.Campo_Clave_Solicitud + " AS CLAVE_SOLICITUD";
                Mi_SQL += ", SOLICITUD." + Ope_Tra_Solicitud.Campo_Folio+ " AS FOLIO";

                Mi_SQL = Mi_SQL + " FROM " + Ope_Ort_Ficha_Revision.Tabla_Ope_Ort_Ficha_Revision + " FICHA_REVISION";

                Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Areas.Tabla_Cat_Areas + " AREA";
                Mi_SQL = Mi_SQL + " ON FICHA_REVISION." + Ope_Ort_Ficha_Revision.Campo_Area_ID + " = AREA." + Cat_Areas.Campo_Area_ID;
                Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Ort_Zona.Tabla_Cat_Ort_Zona + " ZONA";
                Mi_SQL = Mi_SQL + " ON FICHA_REVISION." + Ope_Ort_Ficha_Revision.Campo_Zona_ID + " = ZONA." + Cat_Ort_Zona.Campo_Zona_ID;
                Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " SOLICITUD";
                Mi_SQL = Mi_SQL + " ON FICHA_REVISION." + Ope_Ort_Ficha_Revision.Campo_Solicitud_ID + " = SOLICITUD." + Ope_Tra_Solicitud.Campo_Solicitud_ID;

                if (!String.IsNullOrEmpty(Parametros.P_Solicitud_Interna_ID))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " FICHA_REVISION." + Ope_Ort_Ficha_Revision.Campo_Solicitud_Interna_ID + " = '" + Parametros.P_Solicitud_Interna_ID + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Solicitud_ID))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " FICHA_REVISION." + Ope_Ort_Ficha_Revision.Campo_Solicitud_ID + " = '" + Parametros.P_Solicitud_ID + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Zona_ID))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " FICHA_REVISION." + Ope_Ort_Ficha_Revision.Campo_Zona_ID + " = '" + Parametros.P_Zona_ID + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Area_ID))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " FICHA_REVISION." + Ope_Ort_Ficha_Revision.Campo_Area_ID + " = '" + Parametros.P_Area_ID + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Observacion))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " FICHA_REVISION." + Ope_Ort_Ficha_Revision.Campo_Observacion + " LIKE'%" + Parametros.P_Solicitud_Interna_ID + "%'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Respuesta))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " FICHA_REVISION." + Ope_Ort_Ficha_Revision.Campo_Respuesta + " LIKE'%" + Parametros.P_Solicitud_Interna_ID + "%'";
                }
                //if (!String.IsNullOrEmpty(Parametros.P_Fecha_Solicitud.ToString()))
                //{
                //    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                //    Mi_SQL += " FICHA_REVISION." + Ope_Ort_Ficha_Revision.Campo_Fecha_Solicitud + " = '" + Parametros.P_Fecha_Solicitud + "'";
                //}
                //if (!String.IsNullOrEmpty(Parametros.P_Fecha_Respuesta.ToString()))
                //{
                //    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                //    Mi_SQL += " FICHA_REVISION." + Ope_Ort_Ficha_Revision.Campo_Respuesta + " = '" + Parametros.P_Fecha_Respuesta + "'";
                //}
                Mi_SQL += " ORDER BY SOLICITUD_INTERNA_ID ASC";
                if (Mi_SQL != null && Mi_SQL.Trim().Length > 0)
                {
                    Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Datos != null)
                {
                    Dt_Datos = Ds_Datos.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Datos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Detalles_Tarjetas_Gasolina
        ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable.
        ///PARAMETROS           : 
        ///                     1.Parametros.Contiene los parametros que se van a utilizar para
        ///                       hacer la consulta de la Base de Datos.
        ///CREO                 : Salvador Vázquez Camacho
        ///FECHA_CREO           : 30/Julio/2010
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Cls_Ope_Ort_Ficha_Revision_Negocio Consultar_Ficha_Revision(Cls_Ope_Ort_Ficha_Revision_Negocio Parametros)
        {
            String Mi_SQL = null;
            Cls_Ope_Ort_Ficha_Revision_Negocio Obj_Cargado = new Cls_Ope_Ort_Ficha_Revision_Negocio();
            try
            {
                Mi_SQL = "SELECT * FROM " + Ope_Ort_Ficha_Revision.Tabla_Ope_Ort_Ficha_Revision + " WHERE " + Ope_Ort_Ficha_Revision.Campo_Solicitud_Interna_ID + " = '" + Parametros.P_Solicitud_Interna_ID + "'";
                OracleDataReader Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                while (Reader.Read())
                {
                    Obj_Cargado.P_Solicitud_Interna_ID = (!String.IsNullOrEmpty(Reader[Ope_Ort_Ficha_Revision.Campo_Solicitud_Interna_ID].ToString())) ? Reader[Ope_Ort_Ficha_Revision.Campo_Solicitud_Interna_ID].ToString() : "";
                    Obj_Cargado.P_Solicitud_ID = (!String.IsNullOrEmpty(Reader[Ope_Ort_Ficha_Revision.Campo_Solicitud_ID].ToString())) ? Reader[Ope_Ort_Ficha_Revision.Campo_Solicitud_ID].ToString() : "";
                    Obj_Cargado.P_Zona_ID = (!String.IsNullOrEmpty(Reader[Ope_Ort_Ficha_Revision.Campo_Zona_ID].ToString())) ? Reader[Ope_Ort_Ficha_Revision.Campo_Zona_ID].ToString() : "";
                    Obj_Cargado.P_Area_ID = (!String.IsNullOrEmpty(Reader[Ope_Ort_Ficha_Revision.Campo_Area_ID].ToString())) ? Reader[Ope_Ort_Ficha_Revision.Campo_Area_ID].ToString() : "";
                    Obj_Cargado.P_Observacion = (!String.IsNullOrEmpty(Reader[Ope_Ort_Ficha_Revision.Campo_Observacion].ToString())) ? Reader[Ope_Ort_Ficha_Revision.Campo_Observacion].ToString() : "";
                    Obj_Cargado.P_Usuario_Creo = (!String.IsNullOrEmpty(Reader[Ope_Ort_Ficha_Revision.Campo_Usuario_Creo].ToString())) ? Reader[Ope_Ort_Ficha_Revision.Campo_Usuario_Creo].ToString() : "";
                    Obj_Cargado.P_Respuesta = (!String.IsNullOrEmpty(Reader[Ope_Ort_Ficha_Revision.Campo_Respuesta].ToString())) ? Reader[Ope_Ort_Ficha_Revision.Campo_Respuesta].ToString() : "";
                    Obj_Cargado.P_Fecha_Solicitud = (!String.IsNullOrEmpty(Reader[Ope_Ort_Ficha_Revision.Campo_Fecha_Solicitud].ToString())) ? Convert.ToDateTime(Reader[Ope_Ort_Ficha_Revision.Campo_Fecha_Solicitud]) : new DateTime();
                    Obj_Cargado.P_Fecha_Respuesta = (!String.IsNullOrEmpty(Reader[Ope_Ort_Ficha_Revision.Campo_Fecha_Respuesta].ToString())) ? Convert.ToDateTime(Reader[Ope_Ort_Ficha_Revision.Campo_Fecha_Respuesta]) : new DateTime();
                }
                Reader.Close();
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Obj_Cargado;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Eliminar_Tarjetas_Gasolina
        ///DESCRIPCIÓN          : Elimina un Registro de la Base de Datos
        ///PARAMETROS           : 
        ///                     1.Parametros.Contiene los parametros que se van a utilizar para
        ///                       hacer la eliminacion de la Base de Datos.
        ///CREO                 : Salvador Vázquez Camacho
        ///FECHA_CREO           : 30/Julio/2010
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static void Eliminar_Ficha_Revision(Cls_Ope_Ort_Ficha_Revision_Negocio Parametros)
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
                String Mi_SQL = "DELETE FROM " + Ope_Ort_Ficha_Revision.Tabla_Ope_Ort_Ficha_Revision;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Ort_Ficha_Revision.Campo_Solicitud_Interna_ID + " = '" + Parametros.P_Solicitud_Interna_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            }
            catch (OracleException Ex)
            {
                if (Ex.Code == 547)
                {
                    Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos. Error: [" + Ex.Message + "]";
                }
                else
                {
                    Mensaje = "Error al intentar eliminar. Error: [" + Ex.Message + "]";
                }
                throw new Exception(Mensaje);
            }
            catch (Exception Ex)
            {
                Mensaje = "Error al intentar eliminar. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Tarjetas_Gasolina
        ///DESCRIPCIÓN          : Modifica en la Base de Datos el registro indicado
        ///PARAMETROS           : 1.Parametros. Contiene los parametros que se van hacer la
        ///                       Modificación en la Base de Datos.
        ///CREO                 : Salvador Vázquez Camacho
        ///FECHA_CREO           : 30/Julio/2010
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static void Respuesta_Ficha_Revision(Cls_Ope_Ort_Ficha_Revision_Negocio Parametros)
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
                String Mi_SQL = "UPDATE " + Ope_Ort_Ficha_Revision.Tabla_Ope_Ort_Ficha_Revision;
                Mi_SQL += " SET " + Ope_Ort_Ficha_Revision.Campo_Respuesta + " = '" + Parametros.P_Respuesta;
                Mi_SQL += "', " + Ope_Ort_Ficha_Revision.Campo_Fecha_Respuesta + " = SYSDATE";
                Mi_SQL += " WHERE " + Ope_Ort_Ficha_Revision.Campo_Solicitud_Interna_ID + " = '" + Parametros.P_Solicitud_Interna_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
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
                    Mensaje = "Error al intentar Modificar. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN         : Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARAMETROS          :     
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010
        ///MODIFICO            : 
        ///FECHA_MODIFICO      : 
        ///CAUSA_MODIFICACIÓN  : 
        ///*******************************************************************************
        public static String Obtener_ID_Consecutivo(String Tabla, String Campo, Int32 Longitud_ID)
        {
            String Id = Convertir_A_Formato_ID(1, Longitud_ID); ;
            try
            {
                String Mi_SQL = "SELECT MAX(" + Campo + ") FROM " + Tabla;
                Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals(""))
                {
                    Id = Convertir_A_Formato_ID((Convert.ToInt32(Obj_Temp) + 1), Longitud_ID);
                }
            }
            catch (OracleException Ex)
            {
                new Exception(Ex.Message);
            }
            return Id;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Convertir_A_Formato_ID
        ///DESCRIPCIÓN         : Pasa un numero entero a Formato de ID.
        ///PARAMETROS          : 1. Dato_ID. Dato que se desea pasar al Formato de ID.
        ///                      2. Longitud_ID. Longitud que tendra el ID. 
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010
        ///MODIFICO            : 
        ///FECHA_MODIFICO      : 
        ///CAUSA_MODIFICACIÓN  : 
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