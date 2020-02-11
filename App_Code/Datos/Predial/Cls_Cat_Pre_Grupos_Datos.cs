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
using Presidencia.Catalogo_Grupos.Negocio;
using Presidencia.Catalogo_Ramas.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pre_Grupos_Datos
/// </summary>

namespace Presidencia.Catalogo_Grupos.Datos
{
    public class Cls_Cat_Pre_Grupos_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Grupo
        ///DESCRIPCIÓN: Da de alta en la Base de Datos un nuevo Grupo
        ///PARAMENTROS:     
        ///             1. Grupo.           Instancia de la Clase de Negocio de Grupos 
        ///                                 con los datos del que van a ser
        ///                                 dados de Alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 07/Julio/2011 
        ///MODIFICO: Miguel Angel Bedolla Moreno
        ///FECHA_MODIFICO 
        ///CAUSA_MODIFICACIÓN 
        ///*******************************************************************************
        public static void Alta_Grupos(Cls_Cat_Pre_Grupos_Negocio Grupo)
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

                String Grupo_ID = Obtener_ID_Consecutivo(Cat_Pre_Grupos.Tabla_Cat_Pre_Grupos, Cat_Pre_Grupos.Campo_Grupo_ID, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Grupos.Tabla_Cat_Pre_Grupos;
                Mi_SQL = Mi_SQL + " (" + Cat_Pre_Grupos.Campo_Grupo_ID + ", " + Cat_Pre_Grupos.Campo_Clave;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Grupos.Campo_Nombre + ", " + Cat_Pre_Grupos.Campo_Descripcion;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Grupos.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Grupos.Campo_Usuario_Creo + ", " + Cat_Pre_Grupos.Campo_Fecha_Creo;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Grupos.Campo_Usuario_Modifico + ", " + Cat_Pre_Grupos.Campo_Fecha_Modifico;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Grupos.Campo_Rama_ID+")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Grupo_ID + "', '" + Grupo.P_Clave + "'";
                Mi_SQL = Mi_SQL + ",'" + Grupo.P_Nombre + "'";
                Mi_SQL = Mi_SQL + ",'" + Grupo.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + ",'" + Grupo.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ",'" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'";
                Mi_SQL = Mi_SQL + ",sysdate";
                Mi_SQL = Mi_SQL + ",''";
                Mi_SQL = Mi_SQL + ",''";
                Mi_SQL = Mi_SQL + ", '"+Grupo.P_Rama_ID + "')";
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Grupos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Gasto_Ejecucion
        ///DESCRIPCIÓN: Elimina un Gasto Ejecucion
        ///PARAMETROS:   
        ///             1. Gasto_Ejecucion.   Gasto de Ejecucion que se va eliminar.
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 19/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Grupo(Cls_Cat_Pre_Grupos_Negocio Grupo)
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Grupos.Tabla_Cat_Pre_Grupos + " SET ";
                Mi_SQL = Mi_SQL + "" + Cat_Pre_Grupos.Campo_Estatus + " = 'BAJA'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Grupos.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Grupos.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Grupos.Campo_Grupo_ID+ " = '" + Grupo.P_Grupo_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            }
            catch (OracleException Ex)
            {
                if (Ex.Code == 547)
                {
                    Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos. Error: [" + Ex.Message + "]"; ;
                }
                else
                {
                    Mensaje = "Error al intentar eliminar el registro de Gastos de Ejecución. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                throw new Exception(Mensaje);
            }
            catch (Exception Ex)
            {
                Mensaje = "Error al intentar eliminar el registro de Grupos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Grupo
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Grupo.
        ///PARAMENTROS:     
        ///             1. Grupo.            Instancia de la Clase de Grupos 
        ///                                 con los datos del Registro 
        ///                                 que va a ser Actualizado.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 07/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Grupos(Cls_Cat_Pre_Grupos_Negocio Grupo)
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Grupos.Tabla_Cat_Pre_Grupos;
                Mi_SQL = Mi_SQL + " SET " + Cat_Pre_Grupos.Campo_Clave + " = '" + Grupo.P_Clave + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Grupos.Campo_Nombre + " = '" + Grupo.P_Nombre + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Grupos.Campo_Descripcion + " = '" + Grupo.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Grupos.Campo_Estatus + " = '" + Grupo.P_Estatus + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Grupos.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Grupos.Campo_Fecha_Modifico + " = sysdate";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Grupos.Campo_Rama_ID + " = '" + Grupo.P_Rama_ID + "'";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Grupos.Campo_Grupo_ID + " = '" + Grupo.P_Grupo_ID + "'";
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
                    Mensaje = "Error al intentar modificar un Registro de Grupos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Grupos
        ///DESCRIPCIÓN: Obtiene todos los Grupos que estan dados de 
        ///             alta en la Base de Datos
        ///PARAMENTROS:
        ///                 Grupo       Contiene los campos necesarios para hacer un filtrado de 
        ///                             información en base a la descripción, si es que se
        ///                             introdujeron datos de busqueda.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 07/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Grupos(Cls_Cat_Pre_Grupos_Negocio Grupo)
        {
            DataTable tabla = new DataTable();
            if (Grupo.P_Filtro.Trim().Length == 0)
            {
                try
                {
                    String Mi_SQL = "SELECT " + Cat_Pre_Grupos.Campo_Grupo_ID;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Grupos.Campo_Clave + "";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Grupos.Campo_Nombre + "";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Grupos.Campo_Rama_ID + "";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Grupos.Campo_Estatus + "";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Grupos.Campo_Descripcion + "";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Grupos.Tabla_Cat_Pre_Grupos;
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Grupos.Campo_Grupo_ID;
                    DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (dataset != null)
                    {
                        tabla = dataset.Tables[0];
                    }
                }
                catch (Exception Ex)
                {
                    String Mensaje = "Error al intentar consultar los registros de Ramas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
            }
            else
            {
                try
                {
                    String Mi_SQL = "SELECT " + Cat_Pre_Grupos.Campo_Grupo_ID;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Grupos.Campo_Clave + "";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Grupos.Campo_Nombre + "";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Grupos.Campo_Rama_ID + "";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Grupos.Campo_Estatus + "";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Grupos.Campo_Descripcion + "";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Grupos.Tabla_Cat_Pre_Grupos;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Grupos.Campo_Descripcion + " like '%" + Grupo.P_Filtro + "%'";
                    Mi_SQL = Mi_SQL + " OR " + Cat_Pre_Grupos.Campo_Clave + " LIKE '%" + Grupo.P_Filtro + "%'";
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Grupos.Campo_Grupo_ID;
                    DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (dataset != null)
                    {
                        tabla = dataset.Tables[0];
                    }
                }
                catch (Exception Ex)
                {
                    String Mensaje = "Error al intentar consultar los registros de Grupos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
            }
            return tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Grupos
        ///DESCRIPCIÓN: Obtiene todos los Grupos que estan dados de 
        ///             alta en la Base de Datos
        ///PARAMENTROS:
        ///                 Grupo       Contiene los campos necesarios para hacer un filtrado de 
        ///                             información en base a la descripción, si es que se
        ///                             introdujeron datos de busqueda.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 07/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Id_Nombre_Grupos()
        {
            DataTable tabla = new DataTable();
                try
                {
                    String Mi_SQL = "SELECT " + Cat_Pre_Grupos.Campo_Grupo_ID;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Grupos.Campo_Nombre + "";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Grupos.Tabla_Cat_Pre_Grupos;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Grupos.Campo_Estatus+"='VIGENTE'";
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Grupos.Campo_Grupo_ID;
                    DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (dataset != null)
                    {
                        tabla = dataset.Tables[0];
                    }
                }
                catch (Exception Ex)
                {
                    String Mensaje = "Error al intentar consultar los registros de Grupos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
            return tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Grupos
        ///DESCRIPCIÓN: Obtiene a detalle un Grupo.
        ///PARAMENTROS:   
        ///             1. P_Grupo.   Grupo que se va ver a Detalle.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 07/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Pre_Grupos_Negocio Consultar_Datos_Grupos(Cls_Cat_Pre_Grupos_Negocio P_Grupo)
        {
            Cls_Cat_Pre_Grupos_Negocio R_Grupo = new Cls_Cat_Pre_Grupos_Negocio();
            String Mi_SQL = "SELECT " + Cat_Pre_Grupos.Campo_Grupo_ID + ", " + Cat_Pre_Ramas.Campo_Clave;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Grupos.Campo_Nombre;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Grupos.Campo_Descripcion;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Grupos.Campo_Estatus;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Grupos.Campo_Rama_ID;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Grupos.Tabla_Cat_Pre_Grupos;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Grupos.Campo_Grupo_ID + " = '" + P_Grupo.P_Grupo_ID + "'";
            OracleDataReader Data_Reader;
            try
            {
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Grupo.P_Grupo_ID = P_Grupo.P_Grupo_ID;
                while (Data_Reader.Read())
                {
                    R_Grupo.P_Grupo_ID = Data_Reader[Cat_Pre_Grupos.Campo_Grupo_ID].ToString();
                    R_Grupo.P_Clave = Data_Reader[Cat_Pre_Grupos.Campo_Clave].ToString();
                    R_Grupo.P_Nombre = Data_Reader[Cat_Pre_Grupos.Campo_Nombre].ToString();
                    R_Grupo.P_Descripcion = Data_Reader[Cat_Pre_Grupos.Campo_Descripcion].ToString();
                    R_Grupo.P_Estatus = Data_Reader[Cat_Pre_Grupos.Campo_Estatus].ToString();
                    R_Grupo.P_Rama_ID = Data_Reader[Cat_Pre_Grupos.Campo_Rama_ID].ToString();
                }
                Data_Reader.Close();
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el registro de Grupos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Grupo;
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