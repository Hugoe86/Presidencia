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
using Presidencia.Sessiones;
using Presidencia.Catalogo_Rangos_De_Descuentos_Por_Rol.Negocio;

/// <summary>
/// Summary description for Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Datos
/// </summary>

namespace Presidencia.Catalogo_Rangos_De_Descuentos_Por_Rol.Datos
{
    public class Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Rangos_De_Descuento_Por_Rol
        ///DESCRIPCIÓN: Da de alta en la Base de Datos un nuevo Rango de descuento po rol
        ///PARAMENTROS:     
        ///             1. rango.           Instancia de la Clase de Negocio de rangos de descuentos por rol 
        ///                                 con los datos del que van a ser
        ///                                 dados de Alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 11/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static void Alta_Rangos_De_Descuento_Por_Rol(Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio rango)
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

                String Rango_ID = Obtener_ID_Consecutivo(Cat_Pre_Rangos_De_Descuento_Por_Rol.Tabla_Cat_Pre_Rangos_De_Descuento_Por_Rol, Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Rangos_De_Descuento_Por_Rol_Id, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Rangos_De_Descuento_Por_Rol.Tabla_Cat_Pre_Rangos_De_Descuento_Por_Rol;
                Mi_SQL = Mi_SQL + " (" + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Rangos_De_Descuento_Por_Rol_Id + ", " + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Empleado_Id;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Estatus + ", " + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Porcentaje;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Comentarios + ", " + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Tipo;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Usuario_Creo ;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Fecha_Creo +")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Rango_ID + "', '" + rango.P_Empleado_Id + "'";
                Mi_SQL = Mi_SQL + ",'" + rango.P_Estatus + "'";
                Mi_SQL = Mi_SQL + "," + rango.P_Porcentaje_Maximo + "";
                Mi_SQL = Mi_SQL + ",'" + rango.P_Comentarios + "'";
                Mi_SQL = Mi_SQL + ",'" + rango.P_Tipo + "'";
                Mi_SQL = Mi_SQL + ",'" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'";
                Mi_SQL = Mi_SQL + ", sysdate";
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
                    Mensaje = "Error al intentar dar de Alta un Rango de descuento por rol. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Rangos_De_Descuento_Por_Rol
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Rango de descuento por rol
        ///PARAMENTROS:     
        ///             1. rango.           Instancia de la Clase de Rangos_De_Descuento_Por_Rol 
        ///                                 con los datos del Registro 
        ///                                 que va a ser Actualizado.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 11/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Rangos_De_Descuento_Por_Rol(Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio rango)
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Rangos_De_Descuento_Por_Rol.Tabla_Cat_Pre_Rangos_De_Descuento_Por_Rol;
                Mi_SQL = Mi_SQL + " SET " + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Empleado_Id + " = '" + rango.P_Empleado_Id + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Estatus + " = '" + rango.P_Estatus + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Porcentaje + " = " + rango.P_Porcentaje_Maximo + "";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Tipo + " = '" + rango.P_Tipo + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Comentarios + " = '" + rango.P_Comentarios + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Fecha_Modifico + " = sysdate";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Rangos_De_Descuento_Por_Rol_Id + " = '" + rango.P_Rangos_De_Descuento_Por_Rol_Id + "'";
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
                        Mensaje = "Error general en la base de datos";
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
                    Mensaje = "Error al intentar modificar un Rango de descuentos por rol. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Rangos_De_Descuento_Por_Rol
        ///DESCRIPCIÓN: Obtiene todos las Cajas que estan dadas de 
        ///             alta en la Base de Datos
        ///PARAMENTROS:   
        ///             1.  rango.          Parametro de donde se sacara si habra o no un filtro de busqueda, en este
        ///                                 caso el filtro es el comentario.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 11/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Rangos_De_Descuento_Por_Rol(Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio rango)
        {
            DataTable tabla = new DataTable();
                try
                {
                    String Mi_SQL = "SELECT r." + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Rangos_De_Descuento_Por_Rol_Id;
                    Mi_SQL = Mi_SQL + ", r." + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Empleado_Id + "";
                    Mi_SQL = Mi_SQL + ", r." + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Porcentaje + "";
                    Mi_SQL = Mi_SQL + ", r." + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Estatus + "";
                    Mi_SQL = Mi_SQL + ", r." + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Comentarios + "";
                    Mi_SQL = Mi_SQL + ", (select p." + Cat_Empleados.Campo_Nombre + "||' '||p." + Cat_Empleados.Campo_Apellido_Paterno + "||' '||p." + Cat_Empleados.Campo_Apellido_Materno + " from " + Cat_Empleados.Tabla_Cat_Empleados + " p WHERE r." + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Empleado_Id + "=p." + Cat_Empleados.Campo_Empleado_ID + ") as NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Rangos_De_Descuento_Por_Rol.Tabla_Cat_Pre_Rangos_De_Descuento_Por_Rol + " r";
                    if(rango.P_Filtro.Length!=0)
                    {
                        Mi_SQL = Mi_SQL + " WHERE r." + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Comentarios + " LIKE '%" + rango.P_Filtro + "%' OR r." + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Porcentaje + " LIKE '%" + rango.P_Filtro + "%'";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY r." + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Rangos_De_Descuento_Por_Rol_Id;
                    DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (dataset != null)
                    {
                        tabla = dataset.Tables[0];
                    }
                }
                catch (Exception Ex)
                {
                    String Mensaje = "Error al intentar consultar los registros de Rangos de descuentos por rol. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
            return tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Rangos_De_Descuento_Por_Rol_Completo
        ///DESCRIPCIÓN: Obtiene todos los descuentos vigentes para un empleado determinado.
        ///PARAMENTROS:   
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 12/Marzp/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Rangos_De_Descuento_Por_Rol_Completo(Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio rango)
        {
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Porcentaje + ", " + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Tipo;
                Mi_SQL += " FROM " + Cat_Pre_Rangos_De_Descuento_Por_Rol.Tabla_Cat_Pre_Rangos_De_Descuento_Por_Rol;
                Mi_SQL += " WHERE " + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Empleado_Id + "='" + rango.P_Empleado_Id + "' AND " + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Estatus + "='VIGENTE'";
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Rangos_De_Descuento_Por_Rol_Id;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Rangos de descuentos por rol. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Rangos_De_Descuento_Por_Rol
        ///DESCRIPCIÓN: Obtiene a detalle una Rango de descuento por rol.
        ///PARAMENTROS:   
        ///             1. P_Rango.   rango que se va ver a Detalle.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 11/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio Consultar_Datos_Rangos_De_Descuento_Por_Rol(Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio P_Rango)
        {
            Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio R_Rango = new Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio();
            String Mi_SQL = "SELECT r." + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Rangos_De_Descuento_Por_Rol_Id + " AS " + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Rangos_De_Descuento_Por_Rol_Id;
            Mi_SQL = Mi_SQL + ", r." + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Empleado_Id + " AS " + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Empleado_Id;
            Mi_SQL = Mi_SQL + ", r." + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Porcentaje + " AS " + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Porcentaje;
            Mi_SQL = Mi_SQL + ", r." + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Estatus + " AS " + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Estatus;
            Mi_SQL = Mi_SQL + ", r." + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Comentarios + " AS " + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Comentarios;
            Mi_SQL = Mi_SQL + ", r." + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Tipo + " AS " + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Tipo;
            Mi_SQL = Mi_SQL + ", (select p." + Cat_Empleados.Campo_Nombre + "||' '||p." + Cat_Empleados.Campo_Apellido_Paterno + "||' '||p." + Cat_Empleados.Campo_Apellido_Materno + " from " + Cat_Empleados.Tabla_Cat_Empleados + " p WHERE r." + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Empleado_Id + "=p." + Cat_Empleados.Campo_Empleado_ID + ") as NOMBRE";
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Rangos_De_Descuento_Por_Rol.Tabla_Cat_Pre_Rangos_De_Descuento_Por_Rol + " r";
            Mi_SQL = Mi_SQL + " WHERE r." + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Rangos_De_Descuento_Por_Rol_Id + " ='" + P_Rango.P_Rangos_De_Descuento_Por_Rol_Id + "'";
            Mi_SQL = Mi_SQL + " ORDER BY r." + Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Rangos_De_Descuento_Por_Rol_Id;
            OracleDataReader Data_Reader;
            try
            {
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Rango.P_Rangos_De_Descuento_Por_Rol_Id = P_Rango.P_Rangos_De_Descuento_Por_Rol_Id;
                while (Data_Reader.Read())
                {
                    R_Rango.P_Rangos_De_Descuento_Por_Rol_Id = Data_Reader[Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Rangos_De_Descuento_Por_Rol_Id].ToString();
                    R_Rango.P_Empleado_Id = Data_Reader[Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Empleado_Id].ToString();
                    String ayudante = "" + Data_Reader[Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Porcentaje];
                    R_Rango.P_Porcentaje_Maximo = Convert.ToInt32(ayudante);
                    R_Rango.P_Estatus = Data_Reader[Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Estatus].ToString();
                    R_Rango.P_Comentarios = Data_Reader[Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Comentarios].ToString();
                    R_Rango.P_Tipo = Data_Reader[Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Tipo].ToString();
                    R_Rango.P_Nombre_Empleado = Data_Reader["NOMBRE"].ToString();
                }
                Data_Reader.Close();
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el registro de Rango de Descuentos Por Rol. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Rango;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Empleado
        ///DESCRIPCIÓN: Obtiene a detalle un empleado
        ///PARAMENTROS:   
        ///             1. P_Rango.   rango que se va ver a Detalle.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 08/Agosto/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio Consultar_Datos_Empleado(Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio P_Rango)
        {
            Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio R_Rango = new Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio();
            String Mi_SQL = "SELECT " + Cat_Empleados.Campo_Empleado_ID;
            Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Campo_No_Empleado;
            Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" + Cat_Empleados.Campo_Apellido_Materno + " AS NOMBRE_EMPLEADO";
            Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Campo_RFC;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_Empleado_ID+"='"+P_Rango.P_Empleado_Id + "'";
            Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Empleados.Campo_Empleado_ID;
            OracleDataReader Data_Reader;
            try
            {
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Rango.P_Rangos_De_Descuento_Por_Rol_Id = P_Rango.P_Rangos_De_Descuento_Por_Rol_Id;
                while (Data_Reader.Read())
                {
                    R_Rango.P_Nombre_Empleado = Data_Reader["NOMBRE_EMPLEADO"].ToString();
                    R_Rango.P_Empleado_Id = Data_Reader[Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Empleado_Id].ToString();
                }
                Data_Reader.Close();
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el registro de Empleados. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Rango;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Empleado
        ///DESCRIPCIÓN: Obtiene todos los empleados que estan dadas de 
        ///             alta en la Base de Datos que coincidan con los parámetros de búsqueda.
        ///PARAMENTROS:
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 12/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Empleado(Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio rango)
        {
            DataTable tabla = new DataTable();
                try
                {
                    String Mi_SQL = "SELECT " + Cat_Empleados.Campo_Empleado_ID;
                    Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Campo_No_Empleado;
                    Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Campo_Nombre+"||' '||"+Cat_Empleados.Campo_Apellido_Paterno+"||' '||"+Cat_Empleados.Campo_Apellido_Materno+" AS NOMBRE_EMPLEADO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Campo_RFC;
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                    Mi_SQL = Mi_SQL + " WHERE " + rango.P_Filtro_Dinamico;
                    //+" AND "+Cat_Empleados.Campo_Empleado_ID+"='"+rango.P_Empleado_Id+"'";
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Empleados.Campo_Empleado_ID;
                    DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (dataset != null)
                    {
                        tabla = dataset.Tables[0];
                    }
                }
                catch (Exception Ex)
                {
                    String Mensaje = "Error al intentar consultar los registros de Empleados. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
            return tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARÁMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
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
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
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