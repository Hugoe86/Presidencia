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
using Presidencia.Catalogo_Sectores.Negocio;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;

/// <summary>
/// Summary description for Cls_Cat_Pre_Estados_Predio_Datos
/// </summary>

namespace Presidencia.Catalogo_Sectores.Datos{

    public class Cls_Cat_Pre_Sectores_Datos {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Sector
        ///DESCRIPCIÓN: Da de alta en la Base de Datos una nuevo registro de Sector
        ///PARAMETROS:     
        ///             1. Sector.  Objeto con las propiedades necesarias para dar
        ///                         de alta el Sector.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 29/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Sector(Cls_Cat_Pre_Sectores_Negocio Sector) {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try{
                String Sector_ID = Obtener_ID_Consecutivo(Cat_Pre_Sectores.Tabla_Cat_Pre_Sectores, Cat_Pre_Sectores.Campo_Sector_ID, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Sectores.Tabla_Cat_Pre_Sectores;
                Mi_SQL = Mi_SQL + "( " + Cat_Pre_Sectores.Campo_Sector_ID + ", " + Cat_Pre_Sectores.Campo_Clave + ", " + Cat_Pre_Sectores.Campo_Nombre+ ", " + Cat_Pre_Sectores.Campo_Comentarios;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Sectores.Campo_Usuario_Creo + ", " + Cat_Pre_Sectores.Campo_Fecha_Creo;
                Mi_SQL = Mi_SQL + ") VALUES ( '" + Sector_ID + "', '" + Sector.P_Clave + "', '" +Sector.P_Nombre + "', '" + Sector.P_Comentarios + "'";
                Mi_SQL = Mi_SQL + ", '" + Sector.P_Usuario + "', SYSDATE )";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            }catch (OracleException Ex){
                Trans.Rollback();
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152){
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                } else if (Ex.Code == 2627){
                    if (Ex.Message.IndexOf("PRIMARY") != -1){
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                    } else if (Ex.Message.IndexOf("UNIQUE") != -1) {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]";
                    } else {
                        Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]";
                    }
                } else if (Ex.Code == 547) {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]";
                } else if (Ex.Code == 515) {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                } else {
                    Mensaje = "Error al intentar dar de Alta un Registro de Sector. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } finally{
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Sector
        ///DESCRIPCIÓN: Da de alta en la Base de Datos una nuevo registro de Sector
        ///PARAMETROS:     
        ///             1. Sector.  Objeto con las propiedades necesarias para dar
        ///                         de alta el Sector.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 29/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Colonias(Cls_Cat_Pre_Sectores_Negocio Sector)
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
                if (Sector.P_Sector_ID != null)
                {
                    for (int cont = 0; cont < Sector.P_Colonias.Rows.Count; cont++)
                    {
                        String Mi_SQL = "UPDATE " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias;
                        Mi_SQL = Mi_SQL + " SET " + Cat_Ate_Colonias.Campo_Sector_ID + " = '" + Sector.P_Sector_ID + "'";
                        Mi_SQL = Mi_SQL + ", " + Cat_Ate_Colonias.Campo_Usuario_Modifico + " = '" + Sector.P_Usuario + "'";
                        Mi_SQL = Mi_SQL + ", " + Cat_Ate_Colonias.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Ate_Colonias.Campo_Colonia_ID + " = '" + Sector.P_Colonias.Rows[cont]["COLONIA_ID"].ToString().Trim() + "'";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }
                    Trans.Commit();
                }
                else
                {
                    String Sector_ID = Obtener_Ultimo_Sector();
                    for (int cont = 0; cont < Sector.P_Colonias.Rows.Count; cont++)
                    {
                        String Mi_SQL = "UPDATE " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias;
                        Mi_SQL = Mi_SQL + " SET " + Cat_Ate_Colonias.Campo_Sector_ID + " = '" + Sector_ID + "'";
                        Mi_SQL = Mi_SQL + ", " + Cat_Ate_Colonias.Campo_Usuario_Modifico + " = '" + Sector.P_Usuario + "'";
                        Mi_SQL = Mi_SQL + ", " + Cat_Ate_Colonias.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Ate_Colonias.Campo_Colonia_ID + " = '" + Sector.P_Colonias.Rows[cont]["COLONIA_ID"].ToString().Trim() + "'";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }
                    Trans.Commit();
                }
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
                    Mensaje = "Error al intentar Agregar un Registro de Colonias. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Obtener_Clave_Maxima
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARÁMETROS:   
        ///             1. Tabla: Tabla a la que hace referencia el campo.
        ///             2. Campo: Campo que se examinara para obtener el ultimo valor ingresado.
        ///             3. Id:    ID del campo que se quiere obtener la clave siguiente
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 15/Julio/2011 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        public static String Obtener_Ultimo_Sector()
        {
            String Id = "";
            try
            {
                String Mi_SQL = "SELECT MAX(" + Cat_Pre_Sectores.Campo_Sector_ID + ") FROM " + Cat_Pre_Sectores.Tabla_Cat_Pre_Sectores;
                Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals(""))
                {
                    Id = Obj_Temp.ToString();
                }
            }
            catch (OracleException Ex)
            {
                new Exception(Ex.Message);
            }
            return Id;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Sector
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Registro de Sector
        ///PARAMETROS:     
        ///             1. Sector.  Objeto con las propiedades necesarias para dar
        ///                         de actualizar el Sector.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 29/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Sector(Cls_Cat_Pre_Sectores_Negocio Sector)
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
            try {
                String Mi_SQL = "UPDATE " + Cat_Pre_Sectores.Tabla_Cat_Pre_Sectores;
                Mi_SQL = Mi_SQL + " SET " + Cat_Pre_Sectores.Campo_Nombre + " = '" + Sector.P_Nombre + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Sectores.Campo_Comentarios + " = '" + Sector.P_Comentarios + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Sectores.Campo_Usuario_Modifico + " = '" + Sector.P_Usuario + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Sectores.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Sectores.Campo_Sector_ID + " = '" + Sector.P_Sector_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            } catch (OracleException Ex) {
                Trans.Rollback();
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152){
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                } else if (Ex.Code == 2627) {
                    if (Ex.Message.IndexOf("PRIMARY") != -1) {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                    } else if (Ex.Message.IndexOf("UNIQUE") != -1) {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]";
                    } else {
                        Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]";
                    }
                } else if (Ex.Code == 547) {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]";
                } else if (Ex.Code == 515) {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                } else {
                    Mensaje = "Error al intentar modificar un Registro de Sector. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);             
            } finally{
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_DataTable
        ///DESCRIPCIÓN: Obtiene todos Registros de un tipo de consulta y las devueve en 
        ///             un DataTable.
        ///PARAMETROS:   
        ///             1. Sector.  Objeto con las propiedades necesarias para 
        ///                         hacer la consulta.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 29/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_DataTable(Cls_Cat_Pre_Sectores_Negocio Tipo_Predio){
            String Mi_SQL = "";
            DataTable Tabla = new DataTable();
            try{
                DataSet dataSet = null;
                if (Tipo_Predio.P_Tipo_DataTable.Equals("SECTORES")) {
                    Mi_SQL = "SELECT " + Cat_Pre_Sectores.Campo_Sector_ID + " AS SECTOR_ID";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Sectores.Campo_Clave + " AS CLAVE";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Sectores.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Sectores.Campo_Comentarios + " AS COMENTARIOS";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Sectores.Tabla_Cat_Pre_Sectores;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Sectores.Campo_Clave + " LIKE '%" + Tipo_Predio.P_Nombre + "%'";
                    Mi_SQL = Mi_SQL + " OR " + Cat_Pre_Sectores.Campo_Nombre + " LIKE '%" + Tipo_Predio.P_Nombre +"%'";
                } 
                if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) {
                    dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (dataSet != null) {
                    Tabla = dataSet.Tables[0];
                }
            }catch(Exception Ex){
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Colonias
        ///DESCRIPCIÓN: Obtiene todas las Colonias almacenadas en la base de datos.
        ///PARAMETROS:   
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 06/Agosto/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Llenar_Combo_Colonias()
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Ate_Colonias.Campo_Colonia_ID;
                Mi_SQL = Mi_SQL + ", " + Cat_Ate_Colonias.Campo_Nombre;
                Mi_SQL = Mi_SQL + " FROM  " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Ate_Colonias.Campo_Sector_ID + " IS NULL";
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Ate_Colonias.Campo_Nombre;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Colonias. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Colonias
        ///DESCRIPCIÓN: Obtiene todas las Colonias almacenadas en la base de datos.
        ///PARAMETROS:   
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 06/Agosto/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Colonias(Cls_Cat_Pre_Sectores_Negocio Colonia)
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Ate_Colonias.Campo_Colonia_ID;
                Mi_SQL = Mi_SQL + ", " + Cat_Ate_Colonias.Campo_Clave;
                Mi_SQL = Mi_SQL + ", " + Cat_Ate_Colonias.Campo_Nombre;
                Mi_SQL = Mi_SQL + " FROM  " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Ate_Colonias.Campo_Sector_ID + " = '" + Colonia.P_Sector_ID +"'";
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Ate_Colonias.Campo_Colonia_ID;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Colonias. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Clave_Colonia
        ///DESCRIPCIÓN: Obtiene todas las Colonias almacenadas en la base de datos.
        ///PARAMETROS:   
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 08/Agosto/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataSet Consultar_Clave_Colonia(Cls_Cat_Pre_Sectores_Negocio Colonia)
        {
            DataSet dataset;
            try
            {
                String Mi_SQL = "SELECT " + Cat_Ate_Colonias.Campo_Clave;
                Mi_SQL = Mi_SQL + " FROM  " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Ate_Colonias.Campo_Colonia_ID + " = '" + Colonia.P_Colonia_ID + "'";
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Ate_Colonias.Campo_Colonia_ID;
                dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Colonias. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return dataset;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Sector
        ///DESCRIPCIÓN: Elimina un Registro de Sectores de la Base de Datos
        ///PARAMETROS:    
        ///             1. Sector.  Objeto con las propiedades necesarias para
        ///                         eliminar el Sector.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 06/Agosto/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Sector(Cls_Cat_Pre_Sectores_Negocio Sector)
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
                String Mi_SQL = "DELETE FROM " + Cat_Pre_Sectores.Tabla_Cat_Pre_Sectores;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Sectores.Campo_Sector_ID;
                Mi_SQL = Mi_SQL + " = '" + Sector.P_Sector_ID + "'";
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
                    Mensaje = "Error al intentar eliminar el registro de Sector. Error: [" + Ex.Message + "]";
                }
                throw new Exception(Mensaje);
            }
            catch (Exception Ex)
            {
                Mensaje = "Error al intentar eliminar el registro de Sector. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Sector
        ///DESCRIPCIÓN: Elimina un Registro de Sectores de la Base de Datos
        ///PARAMETROS:    
        ///             1. Sector.  Objeto con las propiedades necesarias para
        ///                         eliminar el Sector.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 06/Agosto/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Colonia(Cls_Cat_Pre_Sectores_Negocio Colonia)
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
                String Mi_SQL = "UPDATE " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias;
                Mi_SQL = Mi_SQL + " SET " + Cat_Ate_Colonias.Campo_Sector_ID + " = NULL";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Ate_Colonias.Campo_Colonia_ID;
                Mi_SQL = Mi_SQL + " = '" + Colonia.P_Colonia_ID + "'";
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
                    Mensaje = "Error al intentar eliminar el registro de Sector. Error: [" + Ex.Message + "]";
                }
                throw new Exception(Mensaje);
            }
            catch (Exception Ex)
            {
                Mensaje = "Error al intentar eliminar el registro de Colonia. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Clave_Maxima
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARÁMETROS:   
        ///             1. Tabla: Tabla a la que hace referencia el campo.
        ///             2. Campo: Campo que se examinara para obtener el ultimo valor ingresado.
        ///             3. Id:    ID del campo que se quiere obtener la clave siguiente
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 15/Julio/2011 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        public static String Obtener_Clave_Maxima()
        {
            String Id = "";
            try
            {
                String Mi_SQL = "SELECT MAX(" + Cat_Pre_Sectores.Campo_Clave+ ") FROM " + Cat_Pre_Sectores.Tabla_Cat_Pre_Sectores;
                Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals(""))
                {
                    Id = (Convert.ToInt32(Obj_Temp) + 1).ToString();
                }
            }
            catch (OracleException Ex)
            {
                new Exception(Ex.Message);
            }
            return Id;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
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
        ///PARAMETROS:     
        ///             1. Dato_ID. Dato que se desea pasar al Formato de ID.
        ///             2. Longitud_ID. Longitud que tendra el ID. 
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
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