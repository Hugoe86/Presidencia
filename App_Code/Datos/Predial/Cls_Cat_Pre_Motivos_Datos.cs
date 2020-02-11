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
using Presidencia.Catalogo_Motivos.Negocio;

/// <summary>
/// Summary description for Cls_Cat_Pre_Motivos
/// </summary>

namespace Presidencia.Catalogo_Motivos.Datos
{
    public class Cls_Cat_Pre_Motivos_Datos
    {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Motivo
        ///DESCRIPCIÓN: Da de alta en la Base de Datos un nuevo Motivo
        ///PARAMENTROS:     
        ///             1. Motivo.          Instancia de la Clase de Negocio de Motivos 
        ///                                 con los datos del que van a ser
        ///                                 dados de Alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 27/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO 
        ///CAUSA_MODIFICACIÓN 
        ///*******************************************************************************
        public static void Alta_Motivos(Cls_Cat_Pre_Motivos_Negocio Motivo)
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

                String Motivo_ID = Obtener_ID_Consecutivo(Cat_Pre_Motivos.Tabla_Cat_Pre_Motivos, Cat_Pre_Motivos.Campo_Motivo_ID, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Motivos.Tabla_Cat_Pre_Motivos;
                Mi_SQL = Mi_SQL + " (" + Cat_Pre_Motivos.Campo_Motivo_ID ;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Motivos.Campo_Nombre ;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Motivos.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Motivos.Campo_Usuario_Creo + ", " + Cat_Pre_Ramas.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Motivo_ID + "'";
                Mi_SQL = Mi_SQL + ",'" + Motivo.P_Nombre + "'";
                Mi_SQL = Mi_SQL + ",'" + Motivo.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ",'" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'";
                Mi_SQL = Mi_SQL + ",sysdate";
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Motivos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Modificar_Motivo
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Motivo
        ///PARAMENTROS:     
        ///             1. Motivo.          Instancia de la Clase de Motivos 
        ///                                 con los datos del Registro 
        ///                                 que va a ser Actualizado.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 27/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Motivo(Cls_Cat_Pre_Motivos_Negocio Motivo)
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Motivos.Tabla_Cat_Pre_Motivos;
                Mi_SQL = Mi_SQL + " SET " + Cat_Pre_Motivos.Campo_Nombre + " = '" + Motivo.P_Nombre + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Motivos.Campo_Estatus + " = '" + Motivo.P_Estatus + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Motivos.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Motivos.Campo_Fecha_Modifico + " = sysdate";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Motivos.Campo_Motivo_ID + " = '" + Motivo.P_Motivo_ID + "'";
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
                    Mensaje = "Error al intentar modificar un Registro de Motivos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Motivo
        ///DESCRIPCIÓN: Elimina un motivo de la Base de Datos.
        ///PARAMENTROS:   
        ///             1. Caja.   Registro que se va a eliminar de la Base de Datos.
        ///CREO: Ismael Prieto Sánchez.
        ///FECHA_CREO: 19/Octubre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Motivo(Cls_Cat_Pre_Motivos_Negocio Motivo)
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
                String Mi_SQL = "DELETE FROM " + Cat_Pre_Motivos.Tabla_Cat_Pre_Motivos;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Motivos.Campo_Motivo_ID + " = '" + Motivo.P_Motivo_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            }
            catch (OracleException Ex)
            {
                Trans.Rollback();
                if (Ex.Code == 547)
                {
                    Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos. Error: [" + Ex.Message + "]"; ;
                }
                else
                {
                    Mensaje = "Error al intentar eliminar el registro de Cajas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                throw new Exception(Mensaje);
            }
            catch (Exception Ex)
            {
                Trans.Rollback();
                Mensaje = "Error al intentar eliminar el registro de Cajas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Motivos
        ///DESCRIPCIÓN: Obtiene todos los Motivos que estan dadas de 
        ///             alta en la Base de Datos
        ///PARAMENTROS:
        ///                 Motivo      Contiene los campos necesarios para hacer un filtrado de 
        ///                             información en base a la descripción, si es que se
        ///                             introdujeron datos de busqueda.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 27/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Motivos(Cls_Cat_Pre_Motivos_Negocio Motivo)
        {
            DataTable tabla = new DataTable();
                try
                {
                    String Mi_SQL = "SELECT " + Cat_Pre_Motivos.Campo_Motivo_ID;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Motivos.Campo_Nombre + "";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Motivos.Campo_Estatus + "";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Motivos.Tabla_Cat_Pre_Motivos;
                    if(Motivo.P_Filtro.Length!=0)
                    {
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Motivos.Campo_Nombre + " like '%" + Motivo.P_Filtro + "%'";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Motivos.Campo_Motivo_ID;
                    DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (dataset != null)
                    {
                        tabla = dataset.Tables[0];
                    }
                }
                catch (Exception Ex)
                {
                    String Mensaje = "Error al intentar consultar los registros de Motivos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
            return tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Ramas_Nombre_Id
        ///DESCRIPCIÓN: Obtiene todos los Motivos que estan dadas de 
        ///             alta en la Base de Datos. Solo regresa el nombre y el id para
        ///             llenar Combos.
        ///PARAMENTROS:
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 27/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Motivos_Nombre_Id()
        {
            DataTable tabla = new DataTable();
            {
                try
                {
                    String Mi_SQL = "SELECT " + Cat_Pre_Motivos.Campo_Motivo_ID;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Motivos.Campo_Nombre + "";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Motivos.Tabla_Cat_Pre_Motivos;
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Motivos.Campo_Motivo_ID;
                    DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (dataset != null)
                    {
                        tabla = dataset.Tables[0];
                    }
                }
                catch (Exception Ex)
                {
                    String Mensaje = "Error al intentar consultar los registros de Motivos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
            }
            return tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Motivo
        ///DESCRIPCIÓN: Obtiene a detalle un Motivo.
        ///PARAMENTROS:   
        ///             1. P_Rama.   Rama que se va ver a Detalle.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 07/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Pre_Motivos_Negocio Consultar_Datos_Motivo(Cls_Cat_Pre_Motivos_Negocio P_Motivo)
        {
            Cls_Cat_Pre_Motivos_Negocio R_Motivo = new Cls_Cat_Pre_Motivos_Negocio();
            String Mi_SQL = "SELECT " + Cat_Pre_Motivos.Campo_Motivo_ID;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Motivos.Campo_Estatus;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Motivos.Campo_Nombre;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Motivos.Tabla_Cat_Pre_Motivos;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Motivos.Campo_Motivo_ID + " = '" + P_Motivo.P_Motivo_ID + "'";
            OracleDataReader Data_Reader;
            try
            {
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Motivo.P_Motivo_ID = P_Motivo.P_Motivo_ID;
                while (Data_Reader.Read())
                {
                    R_Motivo.P_Motivo_ID = Data_Reader[Cat_Pre_Motivos.Campo_Motivo_ID].ToString();
                    R_Motivo.P_Estatus = Data_Reader[Cat_Pre_Motivos.Campo_Estatus].ToString();
                    R_Motivo.P_Nombre = Data_Reader[Cat_Pre_Motivos.Campo_Nombre].ToString();
                }
                Data_Reader.Close();
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el registro de Motivos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Motivo;
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