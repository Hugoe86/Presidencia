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
using Presidencia.Catalogo_Gastos_Ejecucion.Negocio;

/// <summary>
/// Summary description for Cls_Cat_Pre_Gastos_Ejecucion_Datos
/// </summary>

namespace Presidencia.Catalogo_Gastos_Ejecucion.Datos{

    public class Cls_Cat_Pre_Gastos_Ejecucion_Datos {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Gasto_Ejecucion
        ///DESCRIPCIÓN: Da de alta en la Base de Datos un nuevo Gasto de Ejecucion
        ///PARAMETROS:     
        ///             1. Gasto_Ejecucion. Instancia de la Clase de Negocio de 
        ///                                     Cls_Cat_Pre_Gastos_Ejecucion_Negocio
        ///                                     con los datos del Gasto Ejecucion que 
        ///                                     va a ser dado de Alta.
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 19/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Gasto_Ejecucion(Cls_Cat_Pre_Gastos_Ejecucion_Negocio Gasto_Ejecucion)
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
                String Gasto_Ejecucion_ID = Obtener_ID_Consecutivo(Cat_Pre_Gastos_Ejecucion.Tabla_Cat_Pre_Gastos_Ejecucion, Cat_Pre_Gastos_Ejecucion.Campo_Gasto_Ejecucion_ID, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Gastos_Ejecucion.Tabla_Cat_Pre_Gastos_Ejecucion;
                Mi_SQL = Mi_SQL + " (" + Cat_Pre_Gastos_Ejecucion.Campo_Gasto_Ejecucion_ID + ", " + Cat_Pre_Gastos_Ejecucion.Campo_Clave;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Gastos_Ejecucion.Campo_Descripcion + ", " + Cat_Pre_Gastos_Ejecucion.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Gastos_Ejecucion.Campo_Nombre;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Gastos_Ejecucion.Campo_Usuario_Creo + ", " + Cat_Pre_Gastos_Ejecucion.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Gasto_Ejecucion_ID + "', '" + Gasto_Ejecucion.P_Clave + "'";
                Mi_SQL = Mi_SQL + ",'" + Gasto_Ejecucion.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + ",'" + Gasto_Ejecucion.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ",'" + Gasto_Ejecucion.P_Nombre + "'";
                Mi_SQL = Mi_SQL + ",'" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'";
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Gasto de Ejecución. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Modificar_Gasto_Ejecucion
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Gasto de Ejecución
        ///PARAMETROS:     
        ///             1. Gasto_Ejecucion.     Instancia de la Clase de Negocio de Gasto de 
        ///                                     Ejecución con los datos del Gastos de Ejecución 
        ///                                     que va a ser Actualizada.
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 19/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Gasto_Ejecucion(Cls_Cat_Pre_Gastos_Ejecucion_Negocio Gasto_Ejecucion){
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Gastos_Ejecucion.Tabla_Cat_Pre_Gastos_Ejecucion + " SET ";
                Mi_SQL = Mi_SQL + "" + Cat_Pre_Gastos_Ejecucion.Campo_Estatus + " = '" + Gasto_Ejecucion.P_Estatus + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Gastos_Ejecucion.Campo_Nombre + " = '" + Gasto_Ejecucion.P_Nombre + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Gastos_Ejecucion.Campo_Descripcion + " = '" + Gasto_Ejecucion.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Gastos_Ejecucion.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Gastos_Ejecucion.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Gastos_Ejecucion.Campo_Gasto_Ejecucion_ID + " = '" + Gasto_Ejecucion.P_Gastos_Ejecucion_ID + "'";
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
                    Mensaje = "Error al intentar modificar un Registro de Gastos de Ejecución. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Gastos_Ejecucion
        ///DESCRIPCIÓN: Obtiene todos los Gastos de Ejecución que estan dadas de 
        ///             alta en la Base de Datos
        ///PARAMENTROS:   
        ///             1.  Gasto_Ejecucion.            Parametro de donde se sacara si habra o no un filtro de busqueda, en este
        ///                                             caso el filtro es la Clave.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 22/Junio/2011 
        ///MODIFICO: Angel Antonio Escamilla Trejo
        ///FECHA_MODIFICO: 21/Marzo/2012
        ///CAUSA_MODIFICACIÓN: Faltava Consultar el costo
        ///*******************************************************************************
        public static DataTable Consultar_Gastos_Ejecucion(Cls_Cat_Pre_Gastos_Ejecucion_Negocio Gasto_Ejecucion)
        {
            DataTable tabla = new DataTable();
                try
                {
                    String Mi_SQL = "SELECT " + Cat_Pre_Gastos_Ejecucion.Campo_Gasto_Ejecucion_ID;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Gastos_Ejecucion.Campo_Clave + "";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Gastos_Ejecucion.Campo_Nombre + "";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Gastos_Ejecucion.Campo_Estatus + "";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Gastos_Ejecucion.Campo_Descripcion + "";
                    Mi_SQL = Mi_SQL + ", ( SELECT " + Cat_Pre_Gastos_Ejec_Tasas.Campo_Tasa + " FROM " + 
                        Cat_Pre_Gastos_Ejec_Tasas.Tabla_Cat_Pre_Gastos_Ejec_Tasas;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Gastos_Ejecucion.Tabla_Cat_Pre_Gastos_Ejecucion + "." + 
                        Cat_Pre_Gastos_Ejecucion.Campo_Gasto_Ejecucion_ID 
                        + " = " + Cat_Pre_Gastos_Ejec_Tasas.Tabla_Cat_Pre_Gastos_Ejec_Tasas + "." + 
                        Cat_Pre_Gastos_Ejec_Tasas.Campo_Gasto_Ejecucion_ID;
                    Mi_SQL = Mi_SQL + " ) COSTO";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Gastos_Ejecucion.Tabla_Cat_Pre_Gastos_Ejecucion;
                    if(Gasto_Ejecucion.P_Filtro.Length!=(0))
                    {
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Gastos_Ejecucion.Campo_Clave + " LIKE '%" + Gasto_Ejecucion.P_Filtro + "%' OR " + Cat_Pre_Gastos_Ejecucion.Campo_Nombre+" LIKE '%"+Gasto_Ejecucion.P_Filtro+"%'";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Gastos_Ejecucion.Campo_Gasto_Ejecucion_ID;
                    DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (dataset != null)
                    {
                        tabla = dataset.Tables[0];
                    }
                }
                catch (Exception Ex)
                {
                    String Mensaje = "Error al intentar consultar los registros de Cajas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
            return tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Gasto_Ejecucion
        ///DESCRIPCIÓN: Obtiene a detalle un Gasto de ejecución.
        ///PARAMENTROS:   
        ///             1. P_Gasto_Ejecucion.   Gasto de Ejecución que se va a ver a Detalle.
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 19/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Pre_Gastos_Ejecucion_Negocio Consultar_Datos_Gasto_Ejecucion(Cls_Cat_Pre_Gastos_Ejecucion_Negocio P_Gasto_Ejecucion)
        {
            Cls_Cat_Pre_Gastos_Ejecucion_Negocio R_Gasto_Ejecucion = new Cls_Cat_Pre_Gastos_Ejecucion_Negocio();
            String Mi_SQL = "SELECT " + Cat_Pre_Gastos_Ejecucion.Campo_Gasto_Ejecucion_ID + ", " + Cat_Pre_Gastos_Ejecucion.Campo_Clave;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Gastos_Ejecucion.Campo_Descripcion;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Gastos_Ejecucion.Campo_Estatus;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Gastos_Ejecucion.Campo_Nombre;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Gastos_Ejecucion.Tabla_Cat_Pre_Gastos_Ejecucion;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Gastos_Ejecucion.Campo_Gasto_Ejecucion_ID + " = '" + P_Gasto_Ejecucion.P_Gastos_Ejecucion_ID + "'";
            OracleDataReader Data_Reader;
            try
            {
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Gasto_Ejecucion.P_Gastos_Ejecucion_ID = P_Gasto_Ejecucion.P_Gastos_Ejecucion_ID;
                while (Data_Reader.Read())
                {
                    R_Gasto_Ejecucion.P_Gastos_Ejecucion_ID = Data_Reader[Cat_Pre_Gastos_Ejecucion.Campo_Gasto_Ejecucion_ID].ToString();
                    R_Gasto_Ejecucion.P_Clave = Data_Reader[Cat_Pre_Gastos_Ejecucion.Campo_Clave].ToString();
                    R_Gasto_Ejecucion.P_Descripcion = Data_Reader[Cat_Pre_Gastos_Ejecucion.Campo_Descripcion].ToString();
                    R_Gasto_Ejecucion.P_Estatus = Data_Reader[Cat_Pre_Gastos_Ejecucion.Campo_Estatus].ToString();
                    R_Gasto_Ejecucion.P_Nombre = Data_Reader[Cat_Pre_Gastos_Ejecucion.Campo_Nombre].ToString();
                }
                Data_Reader.Close();
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el registro de Gastos de Ejecución. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Gasto_Ejecucion;
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
        public static void Eliminar_Gasto_Ejecucion(Cls_Cat_Pre_Gastos_Ejecucion_Negocio Gasto_Ejecucion)
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Gastos_Ejecucion.Tabla_Cat_Pre_Gastos_Ejecucion + " SET ";
                Mi_SQL = Mi_SQL + "" + Cat_Pre_Gastos_Ejecucion.Campo_Estatus + " = 'BAJA'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Gastos_Ejecucion.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Gastos_Ejecucion.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Gastos_Ejecucion.Campo_Gasto_Ejecucion_ID + " = '" + Gasto_Ejecucion.P_Gastos_Ejecucion_ID + "'";
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
                Mensaje = "Error al intentar eliminar el registro de Gastos de Ejecución. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        public static String Obtener_ID_Consecutivo(String Tabla, String Campo, Int32 Longitud_ID) {
            String Id = Convertir_A_Formato_ID(1, Longitud_ID); ;
            try {
                String Mi_SQL = "SELECT MAX(" + Campo + ") FROM " + Tabla;
                Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals("")) {
                    Id = Convertir_A_Formato_ID((Convert.ToInt32(Obj_Temp) + 1), Longitud_ID);
                }
            } catch (OracleException Ex) {
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
        private static String Convertir_A_Formato_ID(Int32 Dato_ID, Int32 Longitud_ID) {
            String Retornar = "";
            String Dato = "" + Dato_ID;
            for (int Cont_Temp = Dato.Length; Cont_Temp < Longitud_ID; Cont_Temp++) {
                Retornar = Retornar + "0";
            }
            Retornar = Retornar + Dato;
            return Retornar;
        }

    }

}