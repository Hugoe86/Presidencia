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
using Presidencia.Catalogo_Perfiles.Negocio;
using System.Text;

/// <summary>
/// Summary description for Cls_Cat_Tra_Perfiles_Datos
/// </summary>

namespace Presidencia.Catalogo_Perfiles.Datos {

    public class Cls_Cat_Tra_Perfiles_Datos
    {

        #region Metodos

        ///****************************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Perfil
        /// DESCRIPCION : Da de Alta un Perfil en la Base de Datos.
        /// PARAMETROS  : 
        ///               1. Perfil.    Objeto de la Clase de Negocio de Perfil con las propiedades
        ///                             del Perfil que va a ser dado de Alta.
        /// CREO        : Francisco Antonio Gallardo Castañeda.
        /// FECHA_CREO  : 06-Octubre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///****************************************************************************************
        public static void Alta_Perfil(Cls_Cat_Tra_Perfiles_Negocio Perfil)
        {
            String Perfil_ID = ""; 
            String Mi_SQL = "";
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
                Perfil_ID = Obtener_ID_Consecutivo(Cat_Tra_Perfiles.Tabla_Cat_Tra_Perfiles, Cat_Tra_Perfiles.Campo_Perfil_ID, 5);

                Mi_SQL = "INSERT INTO " + Cat_Tra_Perfiles.Tabla_Cat_Tra_Perfiles + " (";
                Mi_SQL += Cat_Tra_Perfiles.Campo_Perfil_ID;//1
                Mi_SQL += ", " + Cat_Tra_Perfiles.Campo_Nombre;//2
                Mi_SQL += ", " + Cat_Tra_Perfiles.Campo_Descripcion;//3
                Mi_SQL += ", " + Cat_Tra_Perfiles.Campo_Usuario_Creo;//4
                Mi_SQL += ", " + Cat_Tra_Perfiles.Campo_Fecha_Creo;//5
                
                Mi_SQL += ") VALUES ( ";

                Mi_SQL += "'" + Perfil_ID + "'";//1
                Mi_SQL += ", '" + Perfil.P_Nombre + "'";//2
                Mi_SQL += ", '" + Perfil.P_Descripcion + "'";//3
                Mi_SQL += ", '" + Perfil.P_Usuario + "'";//4
                Mi_SQL += ", SYSDATE";//5
                Mi_SQL += ")";

                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                //SE CARGAN LOS SUBPROCESOS DEL PERFIL
                if (Perfil.P_Detalles_Subproceso != null && Perfil.P_Detalles_Subproceso.Rows.Count > 0)
                {
                    for (int cnt = 0; cnt < Perfil.P_Detalles_Subproceso.Rows.Count; cnt++)
                    {
                        Mi_SQL = "INSERT INTO " + Tra_Subprocesos_Perfiles.Tabla_Tra_Subprocesos_Perfiles + " (";
                        Mi_SQL += Tra_Subprocesos_Perfiles.Campo_Subproceso_ID;//1
                        Mi_SQL += ", " + Tra_Subprocesos_Perfiles.Campo_Perfil_ID;//2
                        Mi_SQL += ", " + Tra_Subprocesos_Perfiles.Campo_Usuario_Creo;//3
                        Mi_SQL += ", " + Tra_Subprocesos_Perfiles.Campo_Fecha_Creo;//4
                        
                        Mi_SQL += ") VALUES (";
                        
                        Mi_SQL += "'" + Perfil.P_Detalles_Subproceso.Rows[cnt][0] + "'";//1
                        Mi_SQL += ", '" + Perfil_ID + "'";//2
                        Mi_SQL += ", '" + Perfil.P_Usuario + "'";//3
                        Mi_SQL += ", SYSDATE";//4
                        Mi_SQL += ")";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }
                }

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
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar";
                    }
                    else if (Ex.Message.IndexOf("UNIQUE") != -1)
                    {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor";
                    }
                    else
                    {
                        Mensaje = "Error general en la base de datos";
                    }
                }
                else if (Ex.Code == 547)
                {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla";
                }
                else if (Ex.Code == 515)
                {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar";
                }
                else
                {
                    Mensaje = Ex.Message; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Cn.State == ConnectionState.Open)
                {
                    Cn.Close();
                }
            }
        }

        ///****************************************************************************************
        /// NOMBRE DE LA FUNCION: Modificar_Perfil
        /// DESCRIPCION : Actualiza los datos un Perfil en la Base de Datos.
        /// PARAMETROS  : 
        ///               1. Perfil.    Objeto de la Clase de Negocio de Perfil con las propiedades
        ///                             del Perfil que va a ser Actualizado.
        /// CREO        : Francisco Antonio Gallardo Castañeda.
        /// FECHA_CREO  : 06-Octubre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///****************************************************************************************        
        public static void Modificar_Perfil(Cls_Cat_Tra_Perfiles_Negocio Perfil)
        {
            String Mi_SQL = "";
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
                //SE ACTUALIZAN LOS DATOS GENERALES DEL PERFIL
                Mi_SQL = "UPDATE " + Cat_Tra_Perfiles.Tabla_Cat_Tra_Perfiles;
                Mi_SQL += " SET " + Cat_Tra_Perfiles.Campo_Nombre + " = '" + Perfil.P_Nombre + "'";
                Mi_SQL += ", " + Cat_Tra_Perfiles.Campo_Descripcion + " = '" + Perfil.P_Descripcion + "'";
                Mi_SQL += ", " + Cat_Tra_Perfiles.Campo_Usuario_Modifico + " = '" + Perfil.P_Usuario + "'";
                Mi_SQL += ", " + Cat_Tra_Perfiles.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_SQL += " WHERE " + Cat_Tra_Perfiles.Campo_Perfil_ID + " = '" + Perfil.P_Perfil_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                //SE ELIMINA LA CONFIGURACION ANTERIOR DEL PERFIL
                Mi_SQL = "DELETE FROM " + Tra_Subprocesos_Perfiles.Tabla_Tra_Subprocesos_Perfiles;
                Mi_SQL += " WHERE " + Tra_Subprocesos_Perfiles.Campo_Perfil_ID;
                Mi_SQL += " = '" + Perfil.P_Perfil_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                //SE CARGAN LOS SUBPROCESOS DEL PERFIL
                if (Perfil.P_Detalles_Subproceso != null && Perfil.P_Detalles_Subproceso.Rows.Count > 0)
                {
                    for (int cnt = 0; cnt < Perfil.P_Detalles_Subproceso.Rows.Count; cnt++)
                    {
                        Mi_SQL = "INSERT INTO " + Tra_Subprocesos_Perfiles.Tabla_Tra_Subprocesos_Perfiles + " (";
                        Mi_SQL += Tra_Subprocesos_Perfiles.Campo_Subproceso_ID;//1
                        Mi_SQL += ", " + Tra_Subprocesos_Perfiles.Campo_Perfil_ID;//2
                        Mi_SQL += ", " + Tra_Subprocesos_Perfiles.Campo_Usuario_Creo;//3
                        Mi_SQL += ", " + Tra_Subprocesos_Perfiles.Campo_Fecha_Creo;//4
                        Mi_SQL += ", " + Tra_Subprocesos_Perfiles.Campo_Usuario_Modifico;//5
                        Mi_SQL += ", " + Tra_Subprocesos_Perfiles.Campo_Fecha_Modifico;//6
                        
                        Mi_SQL += ") VALUES (";
                       
                        Mi_SQL += "'" + Perfil.P_Detalles_Subproceso.Rows[cnt][0] + "'";//1
                        Mi_SQL += ", '" + Perfil.P_Perfil_ID + "'";//2
                        Mi_SQL += ", '" + Perfil.P_Usuario + "'";//3
                        Mi_SQL +=", SYSDATE";//4
                        Mi_SQL += ", '" + Perfil.P_Usuario + "'"; //5
                        Mi_SQL += ", SYSDATE";//6
                        Mi_SQL += ")";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }
                }

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
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar";
                    }
                    else if (Ex.Message.IndexOf("UNIQUE") != -1)
                    {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor";
                    }
                    else
                    {
                        Mensaje = "Error general en la base de datos";
                    }
                }
                else if (Ex.Code == 547)
                {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla";
                }
                else if (Ex.Code == 515)
                {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar";
                }
                else
                {
                    Mensaje = Ex.Message; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_DataTable
        ///DESCRIPCIÓN: Hace una consulta dependiendo del parametro que tenga y genera un 
        ///             DataTable de esa consulta
        ///PARAMETROS:     
        ///             1. Perfil.  Perfil con los datos para realizar la cosulta y extraer
        ///                         el DataTable
        ///CREO: Francisco Antonio Gallardo Castañeda.
        /// FECHA_CREO  : 06-Octubre-2010
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_DataTable(Cls_Cat_Tra_Perfiles_Negocio Perfil)
        {
            String Mi_SQL = "";
            DataTable Tabla = null;
            DataSet dataSet = null;
            if (Perfil.P_Tipo_DataTable.Equals("PERFILES"))
            {
                Mi_SQL = "SELECT " + Cat_Tra_Perfiles.Campo_Perfil_ID + " AS PERFIL_ID, " + Cat_Tra_Perfiles.Campo_Nombre + " AS NOMBRE";
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Perfiles.Campo_Descripcion + " AS DESCRIPCION FROM " + Cat_Tra_Perfiles.Tabla_Cat_Tra_Perfiles;
                Mi_SQL = Mi_SQL + " WHERE upper(" + Cat_Tra_Perfiles.Campo_Nombre + ") LIKE upper('%" + Perfil.P_Nombre + "%')";
            }
            else if (Perfil.P_Tipo_DataTable.Equals("SUBPROCESOS"))
            {
                Mi_SQL = "SELECT " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tramite_ID + " AS TRAMITE_ID";
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Nombre + " AS NOMBRE_TRAMITE"; ;
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Subproceso_ID + " AS SUBPROCESO_ID"; ;
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Nombre + " AS NOMBRE_SUBPROCESO"; ;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites;
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Tramite_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tramite_ID;
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Nombre;
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Nombre;
            }
            if (Mi_SQL != null && Mi_SQL.Trim().Length > 0)
            {
                dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            if (dataSet == null)
            {
                Tabla = new DataTable();
            }
            else
            {
                Tabla = dataSet.Tables[0];
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Perfil
        ///DESCRIPCIÓN: Obtiene todos los Detalles de un Perfil basandose en el ID pasado como propiedad
        ///PARAMETROS:     
        ///             1.P_Perfil.    Perfil que contiene el ID del cual se extraeran los datos.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 29/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Tra_Perfiles_Negocio Consultar_Datos_Perfil(Cls_Cat_Tra_Perfiles_Negocio P_Perfil)
        {
            Cls_Cat_Tra_Perfiles_Negocio R_Perfil = new Cls_Cat_Tra_Perfiles_Negocio();
            OracleDataReader Data_Reader = null;
            try
            {
                String Mi_SQL = "SELECT " + Cat_Tra_Perfiles.Campo_Nombre + ", " + Cat_Tra_Perfiles.Campo_Descripcion;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Tra_Perfiles.Tabla_Cat_Tra_Perfiles + " WHERE " + Cat_Tra_Perfiles.Campo_Perfil_ID;
                Mi_SQL = Mi_SQL + " = '" + P_Perfil.P_Perfil_ID + "'";
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Perfil.P_Perfil_ID = P_Perfil.P_Perfil_ID;
                while (Data_Reader.Read())
                {
                    R_Perfil.P_Nombre = Data_Reader.GetString(0);
                    R_Perfil.P_Descripcion = Data_Reader.GetString(1);
                }
                Data_Reader.Close();
                Mi_SQL = "SELECT " + Tra_Subprocesos_Perfiles.Campo_Subproceso_ID;
                Mi_SQL = Mi_SQL + " FROM " + Tra_Subprocesos_Perfiles.Tabla_Tra_Subprocesos_Perfiles;
                Mi_SQL = Mi_SQL + " WHERE " + Tra_Subprocesos_Perfiles.Campo_Perfil_ID;
                Mi_SQL = Mi_SQL + " = '" + P_Perfil.P_Perfil_ID + "'";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    R_Perfil.P_Detalles_Subproceso = dataset.Tables[0];
                }
                else
                {
                    R_Perfil.P_Detalles_Subproceso = new DataTable();
                }
            }
            catch (OracleException Ex)
            {
                new Exception(Ex.Message);
            }
            finally
            {
                if (!Data_Reader.IsClosed)
                {
                    Data_Reader.Close();
                }
            }
            return R_Perfil;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Perfil
        ///DESCRIPCIÓN: Elimina una Perfil de la Base de Datos, ademas las relaciones con
        ///             la Tabla de Subprocesos-Perfiles
        ///PROPIEDADES:   
        ///             1. Perfil.   Perfil que se va Eliminar.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        /// FECHA_CREO  : 06-Octubre-2010
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Perfil(Cls_Cat_Tra_Perfiles_Negocio Perfil)
        {
            String Mi_SQL = "";
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
                Mi_SQL = "DELETE FROM " + Tra_Subprocesos_Perfiles.Tabla_Tra_Subprocesos_Perfiles;
                Mi_SQL += " WHERE " + Tra_Subprocesos_Perfiles.Campo_Perfil_ID + " = '" + Perfil.P_Perfil_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                Mi_SQL = "DELETE FROM " + Cat_Tra_Perfiles.Tabla_Cat_Tra_Perfiles;
                Mi_SQL += " WHERE " + Cat_Tra_Perfiles.Campo_Perfil_ID + " = '" + Perfil.P_Perfil_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                Trans.Commit();
            }
            catch (OracleException ex)
            {
                if (ex.Code == 547)
                {
                    Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos";
                }
                else
                {
                    Mensaje = ex.Message;
                }
                throw new Exception(Mensaje);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
                Cn.Close();
            }
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
        public static string Obtener_ID_Consecutivo(string tabla, string campo, int longitud_id)
        {
            String id = Convertir_A_Formato_ID(1, longitud_id); ;
            try
            {
                String Mi_SQL = "SELECT MAX(" + campo + ") FROM " + tabla;
                Object tmp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (!(tmp is Nullable) && !tmp.ToString().Equals(""))
                {
                    id = Convertir_A_Formato_ID((Convert.ToInt32(tmp) + 1), longitud_id);
                }
            }
            catch (OracleException Ex)
            {
                new Exception(Ex.Message);
            }
            return id;
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
        private static string Convertir_A_Formato_ID(int Dato_ID, int Longitud_ID)
        {
            String retornar = "";
            String Dato = "" + Dato_ID;
            for (int tmp = Dato.Length; tmp < Longitud_ID; tmp++)
            {
                retornar = retornar + "0";
            }
            retornar = retornar + Dato;
            return retornar;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Tramites
        ///DESCRIPCIÓN          : Metodo para consultar los datos de las zonas
        ///PARAMETROS           :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 15/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Tramites(Cls_Cat_Tra_Perfiles_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                Mi_Sql.Append("SELECT * " + " FROM " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites);
                Mi_Sql.Append(" ORDER BY " + Cat_Tra_Tramites.Campo_Nombre);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Tramites_Dependencia
        ///DESCRIPCIÓN          : Metodo para consultar los datos de los tramties ordenados por dependencia
        ///PARAMETROS           :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 06/Diciembre/2012 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Tramites_Dependencia(Cls_Cat_Tra_Perfiles_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                Mi_Sql.Append("SELECT " + Cat_Tra_Tramites.Campo_Tramite_ID);
                Mi_Sql.Append(", " + Cat_Tra_Tramites.Campo_Nombre + " as NOMBRE_TRAMITE");
                Mi_Sql.Append(", (SELECT " + Cat_Dependencias.Campo_Nombre +
                                " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias +
                                " WHERE " + Cat_Dependencias.Campo_Dependencia_ID + 
                                "=" + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Dependencia_ID + ") As NOMBRE_DEPENDENCIA");
                Mi_Sql.Append(" FROM " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites);
                Mi_Sql.Append(" ORDER BY " + Cat_Tra_Tramites.Campo_Nombre);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Actividades_Tramites
        ///DESCRIPCIÓN          : Metodo para consultar los datos de las zonas
        ///PARAMETROS           :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 15/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Actividades_Tramites(Cls_Cat_Tra_Perfiles_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                Mi_Sql.Append("SELECT " + Cat_Tra_Subprocesos.Campo_Subproceso_ID + " as SUBPROCESO_ID");
                Mi_Sql.Append(", " + Cat_Tra_Subprocesos.Campo_Nombre + " as NOMBRE_SUBPROCESO");
                Mi_Sql.Append(", " + Cat_Tra_Subprocesos.Campo_Orden+ " as ORDEN_SUBPROCESO");
                Mi_Sql.Append(" FROM " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos);
                Mi_Sql.Append(" where " + Cat_Tra_Subprocesos.Campo_Tramite_ID + "='" + Negocio.P_Tramite_id + "' ");
                Mi_Sql.Append(" ORDER BY " + Cat_Tra_Subprocesos.Campo_Orden);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Actividades_Perfil
        ///DESCRIPCIÓN          : Metodo para consultar los datos de las las actividades
        ///PARAMETROS           :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 15/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Actividades_Perfil(Cls_Cat_Tra_Perfiles_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                Mi_Sql.Append("SELECT * from " + Tra_Subprocesos_Perfiles.Tabla_Tra_Subprocesos_Perfiles);
                Mi_Sql.Append(" where " + Tra_Subprocesos_Perfiles.Campo_Perfil_ID + "='" + Negocio.P_Perfil_ID + "' ");
                Mi_Sql.Append(" ORDER BY " + Tra_Subprocesos_Perfiles.Campo_Subproceso_ID);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }
        #endregion
    }

}