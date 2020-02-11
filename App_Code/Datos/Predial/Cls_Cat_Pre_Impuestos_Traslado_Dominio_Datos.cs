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
using Presidencia.Catalogo_Impuestos_Traslado_Dominio.Negocio;

namespace Presidencia.Catalogo_Impuestos_Traslado_Dominio.Datos
{

    public class Cls_Cat_Pre_Impuestos_Traslado_Dominio_Datos
    {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Impuestos_Traslado_Dominio
        ///DESCRIPCIÓN: Obtiene todos los Impuestos que estan dados de alta en la Base de Datos
        ///PARAMETROS:
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Impuestos_Traslado_Dominio()
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Tasa_ID;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Anio;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Valor_Inicial;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Valor_Final;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Tasa;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Deducible_Uno;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Deducible_Dos;
                Mi_SQL = Mi_SQL + ", NVL(" + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Deducible_Tres + ", 0)" + 
                    Cat_Pre_Impuestos_Traslado_Dominio.Campo_Deducible_Tres;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Comentarios;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Impuestos_Traslado_Dominio.Tabla_Cat_Pre_Impuestos_Traslado_Dominio;
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Anio + " DESC ";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Impuestos de Traslado de Dominio. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Impuesto_Traslado_Dominio
        ///DESCRIPCIÓN: Da de alta en la Base de Datos un nuevo Impuesto
        ///PARAMETROS:     
        ///             1. Impuesto. Instancia de la Clase de Negocio de Impuestos con los datos 
        ///                          del Impuesto que va a ser dado de Alta.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Impuesto_Traslado_Dominio(Cls_Cat_Pre_Impuestos_Traslado_Dominio_Negocio Impuesto)
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
                String Impuesto_ID = Obtener_ID_Consecutivo(Cat_Pre_Impuestos_Traslado_Dominio.Tabla_Cat_Pre_Impuestos_Traslado_Dominio, Cat_Pre_Impuestos_Traslado_Dominio.Campo_Tasa_ID, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Impuestos_Traslado_Dominio.Tabla_Cat_Pre_Impuestos_Traslado_Dominio;
                Mi_SQL = Mi_SQL + " (" + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Tasa_ID;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Anio;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Valor_Inicial;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Valor_Final;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Tasa;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Deducible_Uno;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Deducible_Dos;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Deducible_Tres;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Comentarios;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Usuario_Creo;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Fecha_Modifico;
                Mi_SQL = Mi_SQL + ") VALUES ('" + Impuesto_ID + "' ";
                Mi_SQL = Mi_SQL + ",'" + Impuesto.P_Anio + "'";
                Mi_SQL = Mi_SQL + ",'" + Impuesto.P_Valor_Inicial + "'";
                Mi_SQL = Mi_SQL + ",'" + Impuesto.P_Valor_Final + "'";
                Mi_SQL = Mi_SQL + ",'" + Impuesto.P_Tasa + "'";
                Mi_SQL = Mi_SQL + ",'" + Impuesto.P_Deducible_Uno + "'";
                Mi_SQL = Mi_SQL + ",'" + Impuesto.P_Deducible_Dos + "'";
                Mi_SQL = Mi_SQL + ",'" + Impuesto.P_Deducible_Tres + "'";
                Mi_SQL = Mi_SQL + ",'" + Impuesto.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ",'" + Impuesto.P_Comentarios + "'";
                Mi_SQL = Mi_SQL + ",'" + Impuesto.P_Usuario + "'";
                Mi_SQL = Mi_SQL + ", SYSDATE";
                Mi_SQL = Mi_SQL + ")";
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Impuestos de Traslado de Dominio. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Modificar_Impuesto_Traslado_Dominio
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Impuesto
        ///PARAMETROS:     
        ///             1. Impuesto. Instancia de la Clase de Impuestos con 
        ///                          los datos del Impuesto que va a ser Actualizado.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Impuesto_Traslado_Dominio(Cls_Cat_Pre_Impuestos_Traslado_Dominio_Negocio Impuesto)
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Impuestos_Traslado_Dominio.Tabla_Cat_Pre_Impuestos_Traslado_Dominio+ " SET ";
                Mi_SQL = Mi_SQL + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Anio + " = '" + Impuesto.P_Anio + "' ,";
                Mi_SQL = Mi_SQL + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Valor_Inicial + " = '" + Impuesto.P_Valor_Inicial + "' ";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Valor_Final + " = '" + Impuesto.P_Valor_Final + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Tasa + " = '" + Impuesto.P_Tasa + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Deducible_Uno + " = '" + Impuesto.P_Deducible_Uno + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Deducible_Dos + " = '" + Impuesto.P_Deducible_Dos + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Deducible_Tres + " = '" + Impuesto.P_Deducible_Tres + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Comentarios + " = '" + Impuesto.P_Comentarios + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Estatus + " = '" + Impuesto.P_Estatus + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Usuario_Modifico + " = '" + Impuesto.P_Usuario + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Tasa_ID + " = '" + Impuesto.P_Tasa_ID+ "'";
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
                    Mensaje = "Error al intentar modificar un Registro de Impuestos de Traslado de Dominio. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Anio
        ///DESCRIPCIÓN: Obtiene los datos del Impuesto solicitado.
        ///PARAMETROS:   
        ///             1. Impuesto.   Impuesto que se va ver a Detalle.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Anio(Cls_Cat_Pre_Impuestos_Traslado_Dominio_Negocio Impuesto) //Busqueda
        {

            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Tasa_ID;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Anio;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Valor_Inicial;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Valor_Final;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Tasa;
                Mi_SQL = Mi_SQL + ", NVL(" + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Deducible_Uno + ", '0') AS DEDUCIBLE_UNO";
                Mi_SQL = Mi_SQL + ", NVL(" + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Deducible_Dos + ", '0') AS DEDUCIBLE_DOS";
                Mi_SQL = Mi_SQL + ", NVL(" + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Deducible_Tres + ", '0') AS DEDUCIBLE_TRES";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Comentarios;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Impuestos_Traslado_Dominio.Tabla_Cat_Pre_Impuestos_Traslado_Dominio;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Anio + " =" + Impuesto.P_Anio + " ";  
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Anio + " DESC";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Impuesto de Traslado de Dominio. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Tasas_ID
        ///DESCRIPCIÓN: Obtiene los datos del Impuesto filtrados por ID.
        ///PARAMETROS:   
        ///             1. Impuesto.   ID del impuesto que se va a consultar.
        ///CREO: Roberto Gonzalez Oseguera
        ///FECHA_CREO: 15/ago/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Tasas_ID(Cls_Cat_Pre_Impuestos_Traslado_Dominio_Negocio Impuesto) 
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Tasa_ID;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Anio;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Valor_Inicial;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Valor_Final;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Tasa;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Deducible_Uno;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Deducible_Dos;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Deducible_Tres;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Comentarios;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Impuestos_Traslado_Dominio.Tabla_Cat_Pre_Impuestos_Traslado_Dominio;
                // si se proporciono el dato, filtrar por ID
                if (!String.IsNullOrEmpty(Impuesto.P_Tasa_ID))
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Tasa_ID + " = '" + Impuesto.P_Tasa_ID + "' ";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Anio + " DESC";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Impuesto de Traslado de Dominio. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Impuesto_Traslado_Dominio
        ///DESCRIPCIÓN: Elimina un Impuesto
        ///PARAMETROS:   
        ///             1. Impuesto.   Impuesto que se va eliminar.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Impuesto_Traslado_Dominio(Cls_Cat_Pre_Impuestos_Traslado_Dominio_Negocio Impuesto)
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
                String Mi_SQL = "DELETE " + Cat_Pre_Impuestos_Traslado_Dominio.Tabla_Cat_Pre_Impuestos_Traslado_Dominio;
                //Mi_SQL = Mi_SQL + " SET " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Estatus + " = 'BAJA'";
                //Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Usuario_Modifico + " = '" + Impuesto.P_Usuario + "'";
                //Mi_SQL = Mi_SQL + ", " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Impuestos_Traslado_Dominio.Campo_Tasa_ID + " = '" + Impuesto.P_Tasa_ID + "'";
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
                    Mensaje = "Error al intentar modificar un Registro de Impuestos de Traslado de Dominio. Error: [" + Ex.Message + "]";
                }
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }

        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARÁMETROS:     
        ///             1. Tabla: Tabla de referencia a la que se sacara el ultimo ID
        ///             2. Campo: Compo al que se le asignara la ultima ID
        ///             3. Longitud_ID. Longitud que tendra el ID. 
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
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
        ///DESCRIPCIÓN: Pasa un numero entero a Formato de ID.
        ///PARÁMETROS:     
        ///             1. Dato_ID. Dato que se desea pasar al Formato de ID.
        ///             2. Longitud_ID. Longitud que tendra el ID. 
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
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