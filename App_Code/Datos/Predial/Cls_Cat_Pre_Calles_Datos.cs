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
using Presidencia.Catalogo_Calles.Negocio;

namespace Presidencia.Catalogo_Calles.Datos
{

    public class Cls_Cat_Pre_Calles_Datos
    {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Calles
        ///DESCRIPCIÓN: Obtiene todas las Calles que estan dados de alta en la Base de Datos
        ///PARAMETROS:
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 20/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Calles(Cls_Cat_Pre_Calles_Negocio Calle)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";
            String Filtro_SQL = "";

            try
            {
                Mi_SQL = "SELECT C." + Cat_Pre_Calles.Campo_Calle_ID
                    + ", CC." + Cat_Ate_Colonias.Campo_Nombre + " AS COLONIA"
                    + ", CC." + Cat_Ate_Colonias.Campo_Colonia_ID
                    + ", C." + Cat_Pre_Calles.Campo_Clave
                    + ", C." + Cat_Pre_Calles.Campo_Estatus
                    + ", C." + Cat_Pre_Calles.Campo_Nombre
                    + ", NVL(C." + Cat_Pre_Calles.Campo_Comentarios + ", ' ') AS COMENTARIOS"
                    + " FROM  " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles
                    + " C JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias
                    + " CC ON " + "C." + Cat_Pre_Calles.Campo_Colonia_ID + " = CC." + Cat_Ate_Colonias.Campo_Colonia_ID;
                // si se especifica un nombre de calle, filtrar las calles por nombre
                if (!String.IsNullOrEmpty(Calle.P_Nombre_Calle))
                {
                    Filtro_SQL = " WHERE C." + Cat_Pre_Calles.Campo_Nombre + " = '"
                        + Calle.P_Nombre_Calle + "'";
                }
                // si se especifica un ID de colonia, filtrar por ese ID
                if (!String.IsNullOrEmpty(Calle.P_Colonia_ID))
                {
                    // si ya hay un filtro agregar otro con AND
                    if (Filtro_SQL.Length > 0)
                    {
                        Filtro_SQL += " AND CC." + Cat_Ate_Colonias.Campo_Colonia_ID + " = '"
                            + Calle.P_Colonia_ID + "'";
                    }
                    else    // si no hay filtro, agregar filtro con WHERE
                    {
                        Filtro_SQL = " WHERE CC." + Cat_Ate_Colonias.Campo_Colonia_ID + " = '"
                            + Calle.P_Colonia_ID + "'";
                    }
                }

                // agregar filtro y orden a la consulta
                Mi_SQL += Filtro_SQL + " ORDER BY C." + Cat_Pre_Calles.Campo_Clave;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Calles. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Colonias
        ///DESCRIPCIÓN: Llena el combo de Colonias con las colonias existentes en la base de datos
        ///PARAMETROS:
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 20/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Colonias() //Llenar el combo de sectores
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Ate_Colonias.Campo_Colonia_ID;
                Mi_SQL = Mi_SQL + ", " + Cat_Ate_Colonias.Campo_Nombre;
                Mi_SQL = Mi_SQL + " FROM  " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias;
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Ate_Colonias.Campo_Nombre;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Calles. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Calle
        ///DESCRIPCIÓN: Da de alta en la Base de Datos una nueva Calle
        ///PARAMETROS:     
        ///             1. Calle.  Instancia de la Clase de Negocio de Calles con los datos 
        ///                          de la Calle que va a ser dada de Alta.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 18/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Calle(Cls_Cat_Pre_Calles_Negocio Calle)
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
                String Calle_ID = Obtener_ID_Consecutivo(Cat_Pre_Calles.Tabla_Cat_Pre_Calles, Cat_Pre_Calles.Campo_Calle_ID, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles;
                Mi_SQL = Mi_SQL + " (" + Cat_Pre_Calles.Campo_Calle_ID + ", " + Cat_Pre_Calles.Campo_Colonia_ID;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Calles.Campo_Clave + ", " + Cat_Pre_Calles.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Calles.Campo_Nombre + ", " + Cat_Pre_Calles.Campo_Comentarios;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Calles.Campo_Usuario_Creo + ", " + Cat_Pre_Calles.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Calle_ID + "' ";
                Mi_SQL = Mi_SQL + ",'" + Calle.P_Colonia_ID + "'";
                Mi_SQL = Mi_SQL + ",'" + Calle.P_Clave + "'";
                Mi_SQL = Mi_SQL + ",'" + Calle.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ",'" + Calle.P_Nombre + "'";
                Mi_SQL = Mi_SQL + ",'" + Calle.P_Comentarios + "'";
                Mi_SQL = Mi_SQL + ",'" + Calle.P_Usuario + "'";
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Calles. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Modificar_Calle
        ///DESCRIPCIÓN: Actualiza en la Base de Datos una Calle
        ///PARAMETROS:     
        ///             1. Calle. Instancia de la Clase de Calles  con 
        ///                       los datos de la Calle que va a ser Actualizada.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 20/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Calle(Cls_Cat_Pre_Calles_Negocio Calle)
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " SET ";
                Mi_SQL = Mi_SQL + Cat_Pre_Calles.Campo_Nombre + " = '" + Calle.P_Nombre + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Calles.Campo_Comentarios + " = '" + Calle.P_Comentarios + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Calles.Campo_Colonia_ID + " = '" + Calle.P_Colonia_ID + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Calles.Campo_Estatus + " = '" + Calle.P_Estatus + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Calles.Campo_Usuario_Modifico + " = '" + Calle.P_Usuario + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Calles.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Calles.Campo_Calle_ID + " = '" + Calle.P_Calle_ID + "'";
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
                    Mensaje = "Error al intentar modificar un Registro de Calles. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///DESCRIPCIÓN: Obtiene el nombre de la Calle solicitada.
        ///PARAMETROS:   
        ///             1. Calle.   Nombre que se va ver a Detalle.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 20/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Nombre(Cls_Cat_Pre_Calles_Negocio Calle) //Busqueda
        {

            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT C." + Cat_Pre_Calles.Campo_Calle_ID;
                Mi_SQL = Mi_SQL + ", CC." + Cat_Ate_Colonias.Campo_Nombre + " AS COLONIA";
                Mi_SQL = Mi_SQL + ", CC." + Cat_Ate_Colonias.Campo_Colonia_ID;
                Mi_SQL = Mi_SQL + ", C." + Cat_Pre_Calles.Campo_Clave;
                Mi_SQL = Mi_SQL + ", C." + Cat_Pre_Calles.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", C." + Cat_Pre_Calles.Campo_Nombre;
                Mi_SQL = Mi_SQL + ", NVL(C." + Cat_Pre_Calles.Campo_Comentarios + ", ' ') AS COMENTARIOS";
                Mi_SQL = Mi_SQL + " FROM  " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles;
                Mi_SQL = Mi_SQL + " C LEFT JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias;
                Mi_SQL = Mi_SQL + " CC ON " + "C." + Cat_Pre_Calles.Campo_Colonia_ID + " = CC." + Cat_Ate_Colonias.Campo_Colonia_ID;


                if (Calle.P_Nombre_Calle != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE C." + Cat_Pre_Calles.Campo_Nombre + " LIKE '%" + Calle.P_Nombre_Calle + "%' ";
                    Mi_SQL = Mi_SQL + " ORDER BY C." + Cat_Pre_Calles.Campo_Nombre;
                }
                else if (Calle.P_Nombre_Colonia != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE CC." + Cat_Ate_Colonias.Campo_Nombre + " LIKE '%" + Calle.P_Nombre_Colonia + "%' ";
                    Mi_SQL = Mi_SQL + " ORDER BY CC." + Cat_Ate_Colonias.Campo_Nombre;
                }
                else
                {
                    Mi_SQL = Mi_SQL + " WHERE C." + Cat_Ate_Colonias.Campo_Clave + " = '" + Calle.P_Clave + "' ";
                    Mi_SQL = Mi_SQL + " ORDER BY C." + Cat_Ate_Colonias.Campo_Clave;
                }
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Calles. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Calle
        ///DESCRIPCIÓN: Elimina una Calle
        ///PARAMETROS:   
        ///             1. Calle.   Calle que se va eliminar.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 20/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Calle(Cls_Cat_Pre_Calles_Negocio Calle)
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles;
                Mi_SQL = Mi_SQL + " SET " + Cat_Pre_Calles.Campo_Estatus + " = 'BAJA'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Calles.Campo_Usuario_Modifico + " = '" + Calle.P_Usuario + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Calles.Campo_Fecha_Modifico + " = SYSDATE";
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
                    Mensaje = "Error al intentar modificar un Registro de Calles. Error: [" + Ex.Message + "]";
                }
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }

        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Nombre_Calles
        ///DESCRIPCIÓN: Obtiene las Calles para asignarselos
        ///             a un combo.
        ///PARAMENTROS:
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 29/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Nombre_Calles(Cls_Cat_Pre_Calles_Negocio calle)
        {
            String Operador;
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL_Temp = "";
                String Mi_SQL = "SELECT " + Cat_Pre_Calles.Campo_Calle_ID + ", ";
                if (calle.P_Mostrar_Nombre_Calle_Nombre_Colonia)
                {
                    Mi_SQL = Mi_SQL + Cat_Pre_Calles.Campo_Nombre + " || ' - ' || " +
                        "(SELECT " + Cat_Ate_Colonias.Campo_Nombre + " FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " WHERE " + Cat_Ate_Colonias.Campo_Colonia_ID + " = " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + "." + Cat_Pre_Calles.Campo_Colonia_ID + ") AS " + Cat_Pre_Calles.Campo_Nombre;
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Pre_Calles.Campo_Nombre;
                }
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles;
                Mi_SQL = Mi_SQL + " WHERE ";
                if ((calle.P_Calle_ID != null && calle.P_Calle_ID != "")
                    && (calle.P_Nombre != null && calle.P_Nombre != ""))
                {
                    Operador = " OR ";
                }
                else
                {
                    Operador = " AND ";
                }
                if (calle.P_Colonia_ID != null && calle.P_Colonia_ID != "")
                {
                    Mi_SQL = Mi_SQL + Cat_Pre_Calles.Campo_Colonia_ID + Validar_Operador_Comparacion(calle.P_Colonia_ID) + " AND ";
                }
                if ((calle.P_Calle_ID != null && calle.P_Calle_ID != "")
                    && (calle.P_Nombre != null && calle.P_Nombre != ""))
                {
                    Mi_SQL_Temp += "(";
                }
                if (calle.P_Calle_ID != null && calle.P_Calle_ID != "")
                {
                    Mi_SQL_Temp += Cat_Pre_Calles.Campo_Calle_ID + Validar_Operador_Comparacion(calle.P_Calle_ID) + Operador;
                }
                if (calle.P_Nombre != null && calle.P_Nombre != "")
                {
                    Mi_SQL_Temp += " UPPER(" + Cat_Pre_Calles.Campo_Nombre + ") " + Validar_Operador_Comparacion(calle.P_Nombre) + Operador;
                }
                if (Mi_SQL_Temp.EndsWith(" AND "))
                {
                    Mi_SQL_Temp = Mi_SQL_Temp.Substring(0, Mi_SQL_Temp.Length - 5);
                }
                if (Mi_SQL_Temp.EndsWith(" OR "))
                {
                    Mi_SQL_Temp = Mi_SQL_Temp.Substring(0, Mi_SQL_Temp.Length - 4);
                }
                if ((calle.P_Calle_ID != null && calle.P_Calle_ID != "")
                    && (calle.P_Nombre != null && calle.P_Nombre != ""))
                {
                    Mi_SQL_Temp += ")";
                }
                Mi_SQL += Mi_SQL_Temp;
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
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Calles.Campo_Nombre;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Calles. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Ultima_Clave
        ///DESCRIPCIÓN: Da de alta en la Base de Datos una nueva Calle
        ///PARAMETROS:     
        ///             1. Calle.  Instancia de la Clase de Negocio de Calles con los datos.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 20/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static String Ultima_Clave()
        {
            return Obtener_Clave_Maxima(Cat_Pre_Calles.Tabla_Cat_Pre_Calles, Cat_Pre_Calles.Campo_Clave);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Clave_Maxima
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARÁMETROS:   
        ///             1. Tabla: Tabla a la que hace referencia el campo.
        ///             2. Campo: Campo que se examinara para obtener el ultimo valor ingresado.
        ///             3. Id:    ID del campo que se quiere obtener la clave siguiente
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 20/Julio/2011 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        public static String Obtener_Clave_Maxima(String Tabla, String Campo)
        {
            String Id = "";
            try
            {
                String Mi_SQL = "SELECT MAX(" + Campo + ") FROM " + Tabla;
                Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals(""))
                {
                    Int32 Numero = Convert.ToInt32(Obj_Temp);
                    Numero = Numero + 1;
                    Id = Numero.ToString();
                }
                else
                {
                    return "1";
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
        ///FECHA_CREO: 20/Julio/2011 
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
        ///FECHA_CREO: 20/Julio/2011 
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
        ///NOMBRE DE LA FUNCIÓN     : Consultar_Colonias_Calles
        ///DESCRIPCIÓN              : Obtiene los Id's y nombres de las Colonias y Calles relacioandas en los catálogos
        ///PARAMETROS:
        ///CREO                     : Antonio Salvador Benavides Guardado
        ///FECHA_CREO               : 14/Septiembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Colonias_Calles(Cls_Cat_Pre_Calles_Negocio Colonias_Calles)
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT * FROM (";
                Mi_SQL = Mi_SQL + "SELECT DISTINCT ";
                Mi_SQL = Mi_SQL + " " + Cat_Ate_Colonias.Campo_Colonia_ID;
                Mi_SQL = Mi_SQL + ", " + Cat_Ate_Colonias.Campo_Nombre + " AS COLONIA_NOMBRE";
                Mi_SQL = Mi_SQL + ", " + Cat_Ate_Colonias.Campo_Clave + " AS COLONIA_CLAVE";
                Mi_SQL = Mi_SQL + ", NULL AS " + Cat_Pre_Calles.Campo_Calle_ID;
                Mi_SQL = Mi_SQL + ", NULL AS CALLE_NOMBRE";
                Mi_SQL = Mi_SQL + ", NULL AS CALLE_CLAVE";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias;
                Mi_SQL = Mi_SQL + " WHERE ";
                if (Colonias_Calles.P_Nombre_Colonia != null && Colonias_Calles.P_Nombre_Colonia != "")
                {
                    Mi_SQL = Mi_SQL + " UPPER(" + Cat_Ate_Colonias.Campo_Nombre + ")" + Validar_Operador_Comparacion(Colonias_Calles.P_Nombre_Colonia) + " AND ";
                }
                if (Colonias_Calles.P_Clave_Colonia != null && Colonias_Calles.P_Clave_Colonia != "")
                {
                    Mi_SQL = Mi_SQL + " UPPER(" + Cat_Ate_Colonias.Campo_Clave + ")" + Validar_Operador_Comparacion(Colonias_Calles.P_Clave_Colonia) + " AND ";
                }
                if (Colonias_Calles.P_Nombre_Calle != null && Colonias_Calles.P_Nombre_Calle != "")
                {
                    Mi_SQL = Mi_SQL + " UPPER(" + Cat_Pre_Calles.Campo_Nombre + ")" + Validar_Operador_Comparacion(Colonias_Calles.P_Nombre_Calle) + " AND ";
                }
                if (Colonias_Calles.P_Clave_Calle != null && Colonias_Calles.P_Clave_Calle != "")
                {
                    Mi_SQL = Mi_SQL + " UPPER(" + Cat_Pre_Calles.Campo_Clave + ")" + Validar_Operador_Comparacion(Colonias_Calles.P_Clave_Calle) + " AND ";
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
                Mi_SQL = Mi_SQL + " UNION ";
                Mi_SQL = Mi_SQL + " SELECT ";
                Mi_SQL = Mi_SQL + " Colonias." + Cat_Ate_Colonias.Campo_Colonia_ID;
                Mi_SQL = Mi_SQL + ", Colonias." + Cat_Ate_Colonias.Campo_Nombre + " AS COLONIA_NOMBRE";
                Mi_SQL = Mi_SQL + ", Colonias." + Cat_Ate_Colonias.Campo_Clave + " AS COLONIA_CLAVE";
                Mi_SQL = Mi_SQL + ", Calles." + Cat_Pre_Calles.Campo_Calle_ID;
                Mi_SQL = Mi_SQL + ", Calles." + Cat_Pre_Calles.Campo_Nombre + " AS CALLE_NOMBRE";
                Mi_SQL = Mi_SQL + ", Calles." + Cat_Pre_Calles.Campo_Clave + " AS CALLE_CLAVE";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias;
                Mi_SQL = Mi_SQL + " Colonias JOIN " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles;
                Mi_SQL = Mi_SQL + " Calles ON " + "Colonias." + Cat_Ate_Colonias.Campo_Colonia_ID + " = Calles." + Cat_Pre_Calles.Campo_Colonia_ID;
                Mi_SQL = Mi_SQL + " WHERE ";
                if (Colonias_Calles.P_Nombre_Colonia != null && Colonias_Calles.P_Nombre_Colonia != "")
                {
                    Mi_SQL = Mi_SQL + " UPPER(Colonias." + Cat_Ate_Colonias.Campo_Nombre + ")" + Validar_Operador_Comparacion(Colonias_Calles.P_Nombre_Colonia) + " AND ";
                }
                if (Colonias_Calles.P_Clave_Colonia != null && Colonias_Calles.P_Clave_Colonia != "")
                {
                    Mi_SQL = Mi_SQL + " UPPER(Colonias." + Cat_Ate_Colonias.Campo_Clave + ")" + Validar_Operador_Comparacion(Colonias_Calles.P_Clave_Colonia) + " AND ";
                }
                if (Colonias_Calles.P_Nombre_Calle != null && Colonias_Calles.P_Nombre_Calle != "")
                {
                    Mi_SQL = Mi_SQL + " UPPER(Calles." + Cat_Pre_Calles.Campo_Nombre + ")" + Validar_Operador_Comparacion(Colonias_Calles.P_Nombre_Calle) + " AND ";
                }
                if (Colonias_Calles.P_Clave_Calle != null && Colonias_Calles.P_Clave_Calle != "")
                {
                    Mi_SQL = Mi_SQL + " UPPER(Calles." + Cat_Pre_Calles.Campo_Clave + ")" + Validar_Operador_Comparacion(Colonias_Calles.P_Clave_Calle) + " AND ";
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
                Mi_SQL = Mi_SQL + ")";
                Mi_SQL = Mi_SQL + " ORDER BY NVL(COLONIA_NOMBRE, ' '), NVL(CALLE_NOMBRE, ' ')";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Colonias y Calles. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
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
    }
}