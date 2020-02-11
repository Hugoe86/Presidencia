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
using Presidencia.Catalogo_Colonias.Negocio;

namespace Presidencia.Catalogo_Colonias.Datos{
    
    public class Cls_Cat_Pre_Colonias_Datos {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Colonias
        ///DESCRIPCIÓN: Obtiene todas las Colonias que estan dados de alta en la Base de Datos
        ///PARAMETROS:
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 18/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Colonias()
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT C." + Cat_Ate_Colonias.Campo_Colonia_ID;
                Mi_SQL = Mi_SQL + ", C." + Cat_Ate_Colonias.Campo_Clave;
                Mi_SQL = Mi_SQL + ", C." + Cat_Ate_Colonias.Campo_Nombre;
                Mi_SQL = Mi_SQL + ", S." + Cat_Pre_Sectores.Campo_Nombre + " AS SECTOR";
                Mi_SQL = Mi_SQL + ", NVL(T." + Cat_Pre_Tipos_Colonias.Campo_Descripcion + ", ' ') AS DESCRIPCION";
                Mi_SQL = Mi_SQL + ", T." + Cat_Pre_Tipos_Colonias.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", NVL(C." + Cat_Ate_Colonias.Campo_Descripcion + ", ' ') AS COMENTARIOS";
                Mi_SQL = Mi_SQL + ", T." + Cat_Pre_Tipos_Colonias.Campo_Tipo_Colonia_ID ;
                Mi_SQL = Mi_SQL + " FROM  " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias;
                Mi_SQL = Mi_SQL + " C JOIN " + Cat_Pre_Sectores.Tabla_Cat_Pre_Sectores;
                Mi_SQL = Mi_SQL + " S ON " + "C." + Cat_Ate_Colonias.Campo_Sector_ID + " = S." + Cat_Pre_Sectores.Campo_Sector_ID;
                Mi_SQL = Mi_SQL + " JOIN " + Cat_Pre_Tipos_Colonias.Tabla_Cat_Pre_Tipos_Colonias;
                Mi_SQL = Mi_SQL + " T ON " + "C." + Cat_Ate_Colonias.Campo_Tipo_Colonia_ID + " = T." + Cat_Pre_Tipos_Colonias.Campo_Tipo_Colonia_ID;
                Mi_SQL = Mi_SQL + " ORDER BY C." + Cat_Ate_Colonias.Campo_Clave;
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Tipos
        ///DESCRIPCIÓN: Obtiene todos los tipos de colonias existentes en la base de datos
        ///PARAMETROS:
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 19/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Tipos() //Llenar el combo de tipos
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Pre_Tipos_Colonias.Campo_Tipo_Colonia_ID;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Tipos_Colonias.Campo_Descripcion;
                Mi_SQL = Mi_SQL + " FROM  " + Cat_Pre_Tipos_Colonias.Tabla_Cat_Pre_Tipos_Colonias;
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Tipos_Colonias.Campo_Tipo_Colonia_ID;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Tipos de Colonias. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Sectores
        ///DESCRIPCIÓN: Obtiene todos los sectores para las colonias existentes en la base de datos
        ///PARAMETROS:
        ///CREO: Armando Zavala Moreno.
        ///FECHA_CREO: 25/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Sectores(Cls_Cat_Pre_Colonias_Negocio Colonia) //Llenar el combo de tipos
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Pre_Sectores.Campo_Sector_ID;
                Mi_SQL += ", " + Cat_Pre_Sectores.Campo_Nombre;
                Mi_SQL += ", " + Cat_Pre_Sectores.Campo_Clave;
                Mi_SQL += " FROM  " + Cat_Pre_Sectores.Tabla_Cat_Pre_Sectores;
                if (!String.IsNullOrEmpty(Colonia.P_Sector))
                {
                    Mi_SQL += " WHERE " + Cat_Pre_Sectores.Campo_Nombre + "='" + Colonia.P_Sector + "'";
                }
                Mi_SQL += " ORDER BY " + Cat_Pre_Sectores.Campo_Sector_ID;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los Sectores de las Colonias. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Colonia
        ///DESCRIPCIÓN: Da de alta en la Base de Datos una nueva Colonia
        ///PARAMETROS:     
        ///             1. Colonia.  Instancia de la Clase de Negocio de Colonias con los datos 
        ///                          de la Colonia que va a ser dada de Alta.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 18/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Colonia(Cls_Cat_Pre_Colonias_Negocio Colonia)
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
                String Colonia_ID = Obtener_ID_Consecutivo(Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias, Cat_Ate_Colonias.Campo_Colonia_ID, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias;
                Mi_SQL = Mi_SQL + " (" + Cat_Ate_Colonias.Campo_Colonia_ID + ", " + Cat_Ate_Colonias.Campo_Nombre;
                Mi_SQL = Mi_SQL + ", " + Cat_Ate_Colonias.Campo_Descripcion + ", " + Cat_Ate_Colonias.Campo_Tipo_Colonia_ID;
                Mi_SQL = Mi_SQL + ", " + Cat_Ate_Colonias.Campo_Clave;
                Mi_SQL = Mi_SQL + ", " + Cat_Ate_Colonias.Campo_Usuario_Creo + ", " + Cat_Ate_Colonias.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Colonia_ID + "', '" + Colonia.P_Nombre + "'";
                Mi_SQL = Mi_SQL + ",'" + Colonia.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + ",'" + Colonia.P_Tipo + "'";
                Mi_SQL = Mi_SQL + ",'" + Colonia.P_Clave + "'";
                Mi_SQL = Mi_SQL + ",'" + Colonia.P_Usuario + "'";
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
                    Mensaje = "Error al intentar dar de Alta una Colonia. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Ultima Clave
        ///DESCRIPCIÓN: Da de alta en la Base de Datos una nueva Colonia
        ///PARAMETROS:     
        ///             1. Colonia.  Instancia de la Clase de Negocio de Colonias con los datos.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 19/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static String Ultima_Clave(Cls_Cat_Pre_Colonias_Negocio Colonia) 
        {
            return  Obtener_Clave_Maxima(Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias, Cat_Ate_Colonias.Campo_Clave);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Colonia
        ///DESCRIPCIÓN: Actualiza en la Base de Datos una Colonia
        ///PARAMETROS:     
        ///             1. Colonia. Instancia de la Clase de Colonias  con 
        ///                         los datos de la Colonia que va a ser Actualizada.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 19/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Colonia(Cls_Cat_Pre_Colonias_Negocio Colonia)
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
                String Mi_SQL = "UPDATE " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " SET ";
                Mi_SQL = Mi_SQL + Cat_Ate_Colonias.Campo_Nombre + " = '" + Colonia.P_Nombre + "', ";
                Mi_SQL = Mi_SQL + Cat_Ate_Colonias.Campo_Tipo_Colonia_ID + " = '" + Colonia.P_Tipo + "', ";
                Mi_SQL = Mi_SQL + Cat_Ate_Colonias.Campo_Descripcion + " = '" + Colonia.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Ate_Colonias.Campo_Usuario_Modifico + " = '" + Colonia.P_Usuario + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Ate_Colonias.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Ate_Colonias.Campo_Colonia_ID + " = '" + Colonia.P_Colonia_ID + "'";
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
                    Mensaje = "Error al intentar modificar un Registro de Colonias. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Nombre
        ///DESCRIPCIÓN: Obtiene el nombre de la Colonia solicitada.
        ///PARAMETROS:   
        ///             1. Colonia.   Nombre que se va ver a Detalle.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 19/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Nombre(Cls_Cat_Pre_Colonias_Negocio Colonia) //Busqueda
        {

            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT C." + Cat_Ate_Colonias.Campo_Colonia_ID;
                Mi_SQL = Mi_SQL + ", C." + Cat_Ate_Colonias.Campo_Clave;
                Mi_SQL = Mi_SQL + ", C." + Cat_Ate_Colonias.Campo_Nombre;
                Mi_SQL = Mi_SQL + ", S." + Cat_Pre_Sectores.Campo_Nombre + " AS SECTOR";
                Mi_SQL = Mi_SQL + ", NVL(T." + Cat_Pre_Tipos_Colonias.Campo_Descripcion + ", ' ') AS DESCRIPCION";
                Mi_SQL = Mi_SQL + ", T." + Cat_Pre_Tipos_Colonias.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", NVL(C." + Cat_Ate_Colonias.Campo_Descripcion + ", ' ') AS COMENTARIOS";
                Mi_SQL = Mi_SQL + ", T." + Cat_Pre_Tipos_Colonias.Campo_Tipo_Colonia_ID;
                Mi_SQL = Mi_SQL + " FROM  " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias;
                Mi_SQL = Mi_SQL + " C LEFT JOIN " + Cat_Pre_Sectores.Tabla_Cat_Pre_Sectores;
                Mi_SQL = Mi_SQL + " S ON " + "C." + Cat_Ate_Colonias.Campo_Sector_ID + " = S." 
                    + Cat_Pre_Sectores.Campo_Sector_ID;
                Mi_SQL = Mi_SQL + " JOIN " + Cat_Pre_Tipos_Colonias.Tabla_Cat_Pre_Tipos_Colonias;
                Mi_SQL = Mi_SQL + " T ON " + "C." + Cat_Ate_Colonias.Campo_Tipo_Colonia_ID + " = T." 
                    + Cat_Pre_Tipos_Colonias.Campo_Tipo_Colonia_ID;
                Mi_SQL = Mi_SQL + " WHERE " + "C." + Cat_Ate_Colonias.Campo_Nombre 
                    + " LIKE '%" + Colonia.P_Nombre + "%' ";
                Mi_SQL = Mi_SQL + " OR " + "C." + Cat_Ate_Colonias.Campo_Clave 
                    + " LIKE '%" + Colonia.P_Nombre + "%'";
                Mi_SQL = Mi_SQL + " ORDER BY C." + Cat_Ate_Colonias.Campo_Clave; 
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
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Colonia
        ///DESCRIPCIÓN: Elimina una Colonia
        ///PARAMETROS:   
        ///             1. Colonia.   Colonia que se va eliminar.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 19/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Colonia(Cls_Cat_Pre_Colonias_Negocio Colonia)
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Tipos_Colonias.Tabla_Cat_Pre_Tipos_Colonias;
                Mi_SQL = Mi_SQL + " SET " + Cat_Pre_Tipos_Colonias.Campo_Estatus + " = 'BAJA'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Tipos_Colonias.Campo_Usuario_Modifico + " = '" + Colonia.P_Usuario + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Tipos_Colonias.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Ate_Colonias.Campo_Tipo_Colonia_ID + " = '" + Colonia.P_Tipo + "'";
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
                    Mensaje = "Error al intentar modificar un Registro de Colonias. Error: [" + Ex.Message + "]";
                }
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }

        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Nombre_Colonias
        ///DESCRIPCIÓN: Obtiene las Colonias para asignarselos
        ///             a un combo.
        ///PARAMENTROS:
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 29/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Nombre_Colonias(Cls_Cat_Pre_Colonias_Negocio Colonias)
        {
            String Operador;
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Ate_Colonias.Campo_Colonia_ID + ", " + Cat_Ate_Colonias.Campo_Nombre;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias;
                Mi_SQL = Mi_SQL + " WHERE ";
                if ((Colonias.P_Colonia_ID != null && Colonias.P_Colonia_ID != "")
                    && (Colonias.P_Nombre != null && Colonias.P_Nombre != ""))
                {
                    Operador = " OR ";
                }
                else
                {
                    Operador = " AND ";
                }
                if (Colonias.P_Colonia_ID != null && Colonias.P_Colonia_ID != "")
                {
                    Mi_SQL = Mi_SQL + Cat_Pre_Calles.Campo_Colonia_ID + Validar_Operador_Comparacion(Colonias.P_Colonia_ID) + Operador;
                }
                if (Colonias.P_Nombre != null && Colonias.P_Nombre != "")
                {
                    Mi_SQL = Mi_SQL + " UPPER(" + Cat_Pre_Calles.Campo_Nombre + ") " + Validar_Operador_Comparacion(Colonias.P_Nombre) + Operador;
                }
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" OR "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 4);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Ate_Colonias.Campo_Nombre;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Colonias. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return tabla;
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
        public static String Obtener_Clave_Maxima(String Tabla, String Campo)
        {
            String Id = "";
            try
            {
                String Mi_SQL = "SELECT MAX(" + Campo + ") FROM " + Tabla ;
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
        ///PARÁMETROS:     
        ///             1. Tabla: Tabla de referencia a la que se sacara el ultimo ID
        ///             2. Campo: Compo al que se le asignara la ultima ID
        ///             3. Longitud_ID. Longitud que tendra el ID. 
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 15/Julio/2011 
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
        ///FECHA_CREO: 15/Julio/2011 
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

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Validar_Operador_Comparacion
        ///DESCRIPCIÓN          : Devuelve una cadena adecuada al operador indicado en la capa de Negocios
        ///PARAMETROS           : 
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 11/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static String Validar_Operador_Comparacion(String Filtro)
        {
            String Cadena_Validada;
            if (Filtro.Trim().StartsWith("<")
               || Filtro.Trim().StartsWith(">")
               || Filtro.Trim().StartsWith("<>")
               || Filtro.Trim().StartsWith("<=")
               || Filtro.Trim().StartsWith(">=")
               || Filtro.Trim().StartsWith("=")
               || Filtro.Trim().ToUpper().StartsWith("BETWEEN")
               || Filtro.Trim().ToUpper().StartsWith("LIKE")
               || Filtro.Trim().ToUpper().StartsWith("IN"))
            {
                Cadena_Validada = " " + Filtro + " ";
            }
            else
            {
                Cadena_Validada = " = '" + Filtro + "' ";
            }
            return Cadena_Validada;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Actualizar_Sector_Colonia
        ///DESCRIPCIÓN: Actualiza en la Base de Datos una el sector de una Colonia
        ///PARAMETROS:     
        ///             1. Colonia. Instancia de la Clase de Colonias  con 
        ///                         los datos de la Colonia que va a ser Actualizada.
        ///CREO: Armando Zavala Moreno.
        ///FECHA_CREO: 25/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Actualizar_Sector_Colonia(Cls_Cat_Pre_Colonias_Negocio Colonia)
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
                String Mi_SQL = "UPDATE " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " SET ";
                Mi_SQL = Mi_SQL + Cat_Ate_Colonias.Campo_Sector_ID + " = '" + Colonia.P_Sector + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Ate_Colonias.Campo_Usuario_Modifico + " = '" + Colonia.P_Usuario + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Ate_Colonias.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Ate_Colonias.Campo_Colonia_ID + " = '" + Colonia.P_Colonia_ID + "'";
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
                    Mensaje = "Error al intentar modificar un Registro de Colonias. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
        }
    }
}