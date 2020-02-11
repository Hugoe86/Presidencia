using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Ordenamiento_Territorial_Ficha_Revision_Depto.Negocio;

/// <summary>
/// Summary description for Cls_Ope_Ort_Ficha_Revision_Depto_Datos
/// </summary>
namespace Presidencia.Ordenamiento_Territorial_Ficha_Revision_Depto.Datos
{
    public class Cls_Ope_Ort_Ficha_Revision_Depto_Datos
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
        public static void Alta_Ficha_Revision_Depto(Cls_Ope_Ort_Ficha_Revision_Depto_Negocio Parametros)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            Parametros.P_Ficha_Revision_ID = Obtener_ID_Consecutivo(Ope_Ort_Ficha_Revision_Depto.Tabla_Ope_Ort_Ficha_Revision_Depto, Ope_Ort_Ficha_Revision_Depto.Campo_Ficha_Revision_ID, 10);
            try
            {
                String Mi_SQL = "INSERT INTO " + Ope_Ort_Ficha_Revision_Depto.Tabla_Ope_Ort_Ficha_Revision_Depto;
                Mi_SQL += " (" + Ope_Ort_Ficha_Revision_Depto.Campo_Ficha_Revision_ID;
                Mi_SQL += ", " + Ope_Ort_Ficha_Revision_Depto.Campo_Tipo_Tramite;
                Mi_SQL += ", " + Ope_Ort_Ficha_Revision_Depto.Campo_Nombre_Propietario;
                Mi_SQL += ", " + Ope_Ort_Ficha_Revision_Depto.Campo_Calle_Ubicacion;
                Mi_SQL += ", " + Ope_Ort_Ficha_Revision_Depto.Campo_Colonia_Ubicacion;
                Mi_SQL += ", " + Ope_Ort_Ficha_Revision_Depto.Campo_Codigo_Postal;
                Mi_SQL += ", " + Ope_Ort_Ficha_Revision_Depto.Campo_Ciudad_Ubicacion;
                Mi_SQL += ", " + Ope_Ort_Ficha_Revision_Depto.Campo_Estado_Ubicacion;
                Mi_SQL += ", " + Ope_Ort_Ficha_Revision_Depto.Campo_Documentos_Propiedad;
                Mi_SQL += ", " + Ope_Ort_Ficha_Revision_Depto.Campo_Observacion_Juridica;
                Mi_SQL += ", " + Ope_Ort_Ficha_Revision_Depto.Campo_Observacion_Tecnica;
                Mi_SQL += ", " + Ope_Ort_Ficha_Revision_Depto.Campo_Avance_Obra;
                Mi_SQL += ", " + Ope_Ort_Ficha_Revision_Depto.Campo_Documentos_Dictamen;
                Mi_SQL += ", " + Ope_Ort_Ficha_Revision_Depto.Campo_Cumplimiento_Norma;
                Mi_SQL += ", " + Ope_Ort_Ficha_Revision_Depto.Campo_Ubicacion_Construccion;
                Mi_SQL += ", " + Ope_Ort_Ficha_Revision_Depto.Campo_Tramite;
                Mi_SQL += ", " + Ope_Ort_Ficha_Revision_Depto.Campo_Solicitud_ID;
                Mi_SQL += ", " + Ope_Ort_Ficha_Revision_Depto.Campo_Inicio_Permiso;
                Mi_SQL += ", " + Ope_Ort_Ficha_Revision_Depto.Campo_Fin_Permiso;
                Mi_SQL += ", " + Ope_Ort_Ficha_Revision_Depto.Campo_Perito;
                Mi_SQL += ", " + Ope_Ort_Ficha_Revision_Depto.Campo_Usuario_Creo;
                Mi_SQL += ", " + Ope_Ort_Ficha_Revision_Depto.Campo_Fecha_Creo;
                Mi_SQL += ") VALUES ('" + Parametros.P_Ficha_Revision_ID;
                Mi_SQL += "', '" + Parametros.P_Tipo_Tramite;
                Mi_SQL += "', '" + Parametros.P_Nombre_Propietario;
                Mi_SQL += "', '" + Parametros.P_Calle_Ubicacion;
                Mi_SQL += "', '" + Parametros.P_Colonia_Ubicacion;
                Mi_SQL += "', '" + Parametros.P_Codigo_Postal;
                Mi_SQL += "', '" + Parametros.P_Ciudad_Ubicacion;
                Mi_SQL += "', '" + Parametros.P_Estado_Ubicacion;
                Mi_SQL += "', '" + Parametros.P_Documentos_Propiedad;
                Mi_SQL += "', '" + Parametros.P_Observacion_Juridica;
                Mi_SQL += "', '" + Parametros.P_Observacion_Tecnica;
                Mi_SQL += "', '" + Parametros.P_Avance_Obra;
                Mi_SQL += "', '" + Parametros.P_Documentos_Dictamen;
                Mi_SQL += "', '" + Parametros.P_Cumplimiento_Norma;
                Mi_SQL += "', '" + Parametros.P_Ubicacion_Construccion;
                Mi_SQL += "', '" + Parametros.P_Tramite;
                Mi_SQL += "', '" + Parametros.P_Solicitud_ID;
                //Mi_SQL += "', '" + Parametros.P_Inicio_Permiso;
                //Mi_SQL += "', '" + Parametros.P_Fin_Permiso;

                Mi_SQL += "', '";
                Mi_SQL += Parametros.P_Inicio_Permiso.ToString().Contains("01/01/0001") ? "" : Parametros.P_Inicio_Permiso.Day + "/" + Parametros.P_Inicio_Permiso.Month + "/" + Parametros.P_Inicio_Permiso.Year;
                Mi_SQL += "', '";
                Mi_SQL += Parametros.P_Fin_Permiso.ToString().Contains("01/01/0001") ? "" : Parametros.P_Fin_Permiso.Day + "/" + Parametros.P_Fin_Permiso.Month + "/" + Parametros.P_Fin_Permiso.Year;
                
                Mi_SQL += "', '" + Parametros.P_Perito;
                Mi_SQL += "', '" + Parametros.P_Usuario_Creo;
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
        ///PARAMETROS           : 1.Parametros. Contiene los parametros que se van hacer la
        ///                       Modificación en la Base de Datos.
        ///CREO                 : Salvador Vázquez Camacho
        ///FECHA_CREO           : 30/Julio/2010
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static void Modificar_Ficha_Revision_Depto(Cls_Ope_Ort_Ficha_Revision_Depto_Negocio Parametros)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try
            {
                String Mi_SQL = "UPDATE " + Ope_Ort_Ficha_Revision_Depto.Tabla_Ope_Ort_Ficha_Revision_Depto;
                Mi_SQL += " SET " + Ope_Ort_Ficha_Revision_Depto.Campo_Tramite + " = '" + Parametros.P_Tramite;
                Mi_SQL += "', " + Ope_Ort_Ficha_Revision_Depto.Campo_Nombre_Propietario + " = '" + Parametros.P_Nombre_Propietario;
                Mi_SQL += "', " + Ope_Ort_Ficha_Revision_Depto.Campo_Calle_Ubicacion + " = '" + Parametros.P_Calle_Ubicacion;
                Mi_SQL += "', " + Ope_Ort_Ficha_Revision_Depto.Campo_Colonia_Ubicacion + " = '" + Parametros.P_Colonia_Ubicacion;
                Mi_SQL += "', " + Ope_Ort_Ficha_Revision_Depto.Campo_Codigo_Postal + " = '" + Parametros.P_Codigo_Postal;
                Mi_SQL += "', " + Ope_Ort_Ficha_Revision_Depto.Campo_Ciudad_Ubicacion + " = '" + Parametros.P_Ciudad_Ubicacion;
                Mi_SQL += "', " + Ope_Ort_Ficha_Revision_Depto.Campo_Estado_Ubicacion + " = '" + Parametros.P_Estado_Ubicacion;
                Mi_SQL += "', " + Ope_Ort_Ficha_Revision_Depto.Campo_Documentos_Propiedad + " = '" + Parametros.P_Documentos_Propiedad;
                Mi_SQL += "', " + Ope_Ort_Ficha_Revision_Depto.Campo_Observacion_Juridica + " = '" + Parametros.P_Observacion_Juridica;
                Mi_SQL += "', " + Ope_Ort_Ficha_Revision_Depto.Campo_Observacion_Tecnica + " = '" + Parametros.P_Observacion_Tecnica;
                Mi_SQL += "', " + Ope_Ort_Ficha_Revision_Depto.Campo_Avance_Obra + " = '" + Parametros.P_Avance_Obra;
                Mi_SQL += "', " + Ope_Ort_Ficha_Revision_Depto.Campo_Documentos_Dictamen + " = '" + Parametros.P_Documentos_Dictamen;
                Mi_SQL += "', " + Ope_Ort_Ficha_Revision_Depto.Campo_Cumplimiento_Norma + " = '" + Parametros.P_Cumplimiento_Norma;
                Mi_SQL += "', " + Ope_Ort_Ficha_Revision_Depto.Campo_Ubicacion_Construccion + " = '" + Parametros.P_Ubicacion_Construccion;
                Mi_SQL += "', " + Ope_Ort_Ficha_Revision_Depto.Campo_Tipo_Tramite + " = '" + Parametros.P_Tramite;
                Mi_SQL += "', " + Ope_Ort_Ficha_Revision_Depto.Campo_Solicitud_ID + " = '" + Parametros.P_Solicitud_ID;
                //if (!Parametros.P_Inicio_Permiso.ToString().Contains("01/01/0001"))
                //    Mi_SQL += "', " + Ope_Ort_Ficha_Revision_Depto.Campo_Inicio_Permiso + " = '" + Parametros.P_Inicio_Permiso.Day + "/" + Parametros.P_Inicio_Permiso.Month + "/" + Parametros.P_Inicio_Permiso.Year;
                //if (!Parametros.P_Fin_Permiso.ToString().Contains("01/01/0001"))
                //    Mi_SQL += "', " + Ope_Ort_Ficha_Revision_Depto.Campo_Fin_Permiso + " = '" + Parametros.P_Fin_Permiso.Day + "/" + Parametros.P_Fin_Permiso.Month + "/" + Parametros.P_Fin_Permiso.Year;

                Mi_SQL += "', " + Ope_Ort_Ficha_Revision_Depto.Campo_Inicio_Permiso + " = '";
                Mi_SQL += Parametros.P_Inicio_Permiso.ToString().Contains("01/01/0001") ? "" : Parametros.P_Inicio_Permiso.Day + "/" + Parametros.P_Inicio_Permiso.Month + "/" + Parametros.P_Inicio_Permiso.Year;
                Mi_SQL += "', " + Ope_Ort_Ficha_Revision_Depto.Campo_Fin_Permiso + " = '";
                Mi_SQL += Parametros.P_Fin_Permiso.ToString().Contains("01/01/0001") ? "" : Parametros.P_Fin_Permiso.Day + "/" + Parametros.P_Fin_Permiso.Month + "/" + Parametros.P_Fin_Permiso.Year;
                
                Mi_SQL += "', " + Ope_Ort_Ficha_Revision_Depto.Campo_Perito + " = '" + Parametros.P_Perito;
                Mi_SQL += "', " + Ope_Ort_Ficha_Revision_Depto.Campo_Usuario_Modifico + " = '" + Parametros.P_Usuario_Modifico;
                Mi_SQL += "', " + Ope_Ort_Ficha_Revision_Depto.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL += " WHERE " + Ope_Ort_Ficha_Revision_Depto.Campo_Ficha_Revision_ID + " = '" + Parametros.P_Ficha_Revision_ID + "'";
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
        public static DataTable Consultar_Tabla_Ficha_Revision_Depto(Cls_Ope_Ort_Ficha_Revision_Depto_Negocio Parametros)
        {
            String Mi_SQL = null;
            DataSet Ds_Datos = null;
            DataTable Dt_Datos = new DataTable();
            Boolean Entro_Where = false;
            try
            {
                Mi_SQL = "SELECT FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Ficha_Revision_ID + " AS FICHA_REVISION_ID";
                Mi_SQL += ", FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Tipo_Tramite + " AS TIPO_TRAMITE";
                Mi_SQL += ", FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Nombre_Propietario + " AS NOMBRE_PROPIETARIO";
                Mi_SQL += ", FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Calle_Ubicacion + " AS CALLE_UBICACION";
                Mi_SQL += ", FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Colonia_Ubicacion + " AS COLONIA_UBICACION";
                Mi_SQL += ", FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Codigo_Postal + " AS CODIGO_POSTAL";
                Mi_SQL += ", FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Ciudad_Ubicacion + " AS CIUDAD_UBICACION";
                Mi_SQL += ", FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Estado_Ubicacion + " AS ESTADO_UBICACION";
                Mi_SQL += ", FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Documentos_Propiedad + " AS DOCUMENTOS_PROPIEDAD";
                Mi_SQL += ", FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Observacion_Juridica + " AS OBSERVACION_JURIDICA";
                Mi_SQL += ", FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Observacion_Tecnica + " AS OBSERVACION_TECNICA";
                Mi_SQL += ", FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Avance_Obra + " AS AVANCE_OBRA";
                Mi_SQL += ", FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Documentos_Dictamen + " AS DOCUMENTOS_DICTAMEN";
                Mi_SQL += ", FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Cumplimiento_Norma + " AS CUMPLIMIENTO_NORMAS";
                Mi_SQL += ", FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Ubicacion_Construccion + " AS UBICACION_CONSTRUCCION";
                Mi_SQL += ", FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Tramite + " AS TRAMITE";
                Mi_SQL += ", FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Solicitud_ID + " AS SOLICITUD_ID";
                Mi_SQL += ", FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Inicio_Permiso + " AS INICIO_PERMISO";
                Mi_SQL += ", FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Fin_Permiso + " AS FIN_PERMISO";
                Mi_SQL += ", FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Perito + " AS PERITO";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Ort_Ficha_Revision_Depto.Tabla_Ope_Ort_Ficha_Revision_Depto + " FICHA_REVISION_DEPTO";
                if (!String.IsNullOrEmpty(Parametros.P_Ficha_Revision_ID))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Ficha_Revision_ID + " = '" + Parametros.P_Ficha_Revision_ID + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Tipo_Tramite))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Tipo_Tramite + " = '" + Parametros.P_Tipo_Tramite + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Nombre_Propietario))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Nombre_Propietario + " = '" + Parametros.P_Nombre_Propietario + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Calle_Ubicacion))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Calle_Ubicacion + " = '" + Parametros.P_Calle_Ubicacion + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Colonia_Ubicacion))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Colonia_Ubicacion + " = '" + Parametros.P_Colonia_Ubicacion + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Codigo_Postal))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Codigo_Postal + " = '" + Parametros.P_Codigo_Postal + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Ciudad_Ubicacion))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Ciudad_Ubicacion + " = '" + Parametros.P_Ciudad_Ubicacion + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Estado_Ubicacion))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Estado_Ubicacion + " = '" + Parametros.P_Estado_Ubicacion + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Documentos_Propiedad))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Documentos_Propiedad + " = '" + Parametros.P_Documentos_Propiedad + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Observacion_Juridica))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Observacion_Juridica + " LIKE'%" + Parametros.P_Observacion_Juridica + "%'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Observacion_Tecnica))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Observacion_Tecnica + " LIKE'%" + Parametros.P_Observacion_Tecnica + "%'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Avance_Obra))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Avance_Obra + " = '" + Parametros.P_Avance_Obra + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Documentos_Dictamen))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Documentos_Dictamen + " = '" + Parametros.P_Documentos_Dictamen + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Cumplimiento_Norma))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Cumplimiento_Norma + " = '" + Parametros.P_Cumplimiento_Norma + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Ubicacion_Construccion))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Ubicacion_Construccion + " = '" + Parametros.P_Ubicacion_Construccion + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Tramite))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Tramite + " = '" + Parametros.P_Tramite + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Solicitud_ID))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Solicitud_ID + " = '" + Parametros.P_Solicitud_ID + "'";
                }
                //if (!String.IsNullOrEmpty(Parametros.P_Inicio_Permiso.ToString()))
                //{
                //    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                //    Mi_SQL += " FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Inicio_Permiso + " = '" + Parametros.P_Inicio_Permiso + "'";
                //}
                //if (!String.IsNullOrEmpty(Parametros.P_Fin_Permiso.ToString()))
                //{
                //    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                //    Mi_SQL += " FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Fin_Permiso + " = '" + Parametros.P_Fin_Permiso + "'";
                //}
                if (!String.IsNullOrEmpty(Parametros.P_Perito))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " FICHA_REVISION_DEPTO." + Ope_Ort_Ficha_Revision_Depto.Campo_Perito + " = '" + Parametros.P_Perito + "'";
                }
                Mi_SQL += " ORDER BY FICHA_REVISION_ID ASC";
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
        public static Cls_Ope_Ort_Ficha_Revision_Depto_Negocio Consultar_Ficha_Revision_Depto(Cls_Ope_Ort_Ficha_Revision_Depto_Negocio Parametros)
        {
            String Mi_SQL = null;
            Cls_Ope_Ort_Ficha_Revision_Depto_Negocio Obj_Cargado = new Cls_Ope_Ort_Ficha_Revision_Depto_Negocio();
            try
            {
                Mi_SQL = "SELECT * FROM " + Ope_Ort_Ficha_Revision_Depto.Tabla_Ope_Ort_Ficha_Revision_Depto + " WHERE " + Ope_Ort_Ficha_Revision_Depto.Campo_Ficha_Revision_ID + " = '" + Parametros.P_Ficha_Revision_ID + "'";
                OracleDataReader Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                while (Reader.Read())
                {
                    Obj_Cargado.P_Ficha_Revision_ID = (!String.IsNullOrEmpty(Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Ficha_Revision_ID].ToString())) ? Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Ficha_Revision_ID].ToString() : "";
                    Obj_Cargado.P_Tipo_Tramite = (!String.IsNullOrEmpty(Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Tipo_Tramite].ToString())) ? Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Tipo_Tramite].ToString() : "";
                    Obj_Cargado.P_Nombre_Propietario = (!String.IsNullOrEmpty(Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Nombre_Propietario].ToString())) ? Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Nombre_Propietario].ToString() : "";
                    Obj_Cargado.P_Calle_Ubicacion = (!String.IsNullOrEmpty(Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Calle_Ubicacion].ToString())) ? Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Calle_Ubicacion].ToString() : "";
                    Obj_Cargado.P_Colonia_Ubicacion = (!String.IsNullOrEmpty(Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Colonia_Ubicacion].ToString())) ? Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Colonia_Ubicacion].ToString() : "";
                    Obj_Cargado.P_Codigo_Postal = (!String.IsNullOrEmpty(Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Codigo_Postal].ToString())) ? Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Codigo_Postal].ToString() : "";
                    Obj_Cargado.P_Ciudad_Ubicacion = (!String.IsNullOrEmpty(Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Ciudad_Ubicacion].ToString())) ? Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Ciudad_Ubicacion].ToString() : "";
                    Obj_Cargado.P_Estado_Ubicacion = (!String.IsNullOrEmpty(Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Estado_Ubicacion].ToString())) ? Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Estado_Ubicacion].ToString() : "";
                    Obj_Cargado.P_Documentos_Propiedad = (!String.IsNullOrEmpty(Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Documentos_Propiedad].ToString())) ? Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Documentos_Propiedad].ToString() : "";
                    Obj_Cargado.P_Observacion_Juridica = (!String.IsNullOrEmpty(Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Observacion_Juridica].ToString())) ? Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Observacion_Juridica].ToString() : "";
                    Obj_Cargado.P_Observacion_Tecnica = (!String.IsNullOrEmpty(Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Observacion_Tecnica].ToString())) ? Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Observacion_Tecnica].ToString() : "";
                    Obj_Cargado.P_Avance_Obra = (!String.IsNullOrEmpty(Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Avance_Obra].ToString())) ? Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Avance_Obra].ToString() : "";
                    Obj_Cargado.P_Documentos_Dictamen = (!String.IsNullOrEmpty(Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Documentos_Dictamen].ToString())) ? Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Documentos_Dictamen].ToString() : "";
                    Obj_Cargado.P_Cumplimiento_Norma = (!String.IsNullOrEmpty(Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Cumplimiento_Norma].ToString())) ? Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Cumplimiento_Norma].ToString() : "";
                    Obj_Cargado.P_Ubicacion_Construccion = (!String.IsNullOrEmpty(Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Ubicacion_Construccion].ToString())) ? Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Ubicacion_Construccion].ToString() : "";
                    Obj_Cargado.P_Tramite = (!String.IsNullOrEmpty(Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Tramite].ToString())) ? Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Tramite].ToString() : "";
                    Obj_Cargado.P_Solicitud_ID = (!String.IsNullOrEmpty(Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Solicitud_ID].ToString())) ? Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Solicitud_ID].ToString() : "";
                    Obj_Cargado.P_Inicio_Permiso = (!String.IsNullOrEmpty(Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Inicio_Permiso].ToString())) ? Convert.ToDateTime(Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Inicio_Permiso]) : new DateTime();
                    Obj_Cargado.P_Fin_Permiso = (!String.IsNullOrEmpty(Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Fin_Permiso].ToString())) ? Convert.ToDateTime(Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Fin_Permiso]) : new DateTime();
                    Obj_Cargado.P_Perito = (!String.IsNullOrEmpty(Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Perito].ToString())) ? Reader[Ope_Ort_Ficha_Revision_Depto.Campo_Perito].ToString() : "";
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
        public static void Eliminar_Ficha_Revision_Depto(Cls_Ope_Ort_Ficha_Revision_Depto_Negocio Parametros)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try
            {
                String Mi_SQL = "DELETE FROM " + Ope_Ort_Ficha_Revision_Depto.Tabla_Ope_Ort_Ficha_Revision_Depto;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Ort_Ficha_Revision_Depto.Campo_Ficha_Revision_ID + " = '" + Parametros.P_Ficha_Revision_ID + "'";
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