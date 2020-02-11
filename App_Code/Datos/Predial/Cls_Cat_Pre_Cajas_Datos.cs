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
using Presidencia.Catalogo_Cajas.Negocio;

/// <summary>
/// Summary description for Cls_Cat_Pre_Cajas_Datos
/// </summary>

namespace Presidencia.Catalogo_Cajas.Datos
{
    public class Cls_Cat_Pre_Cajas_Datos
    {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Caja
        ///DESCRIPCIÓN: Da de alta en la Base de Datos una nueva caja
        ///PARAMENTROS:     
        ///             1. Caja.            Instancia de la Clase de Negocio de Cajas 
        ///                                 con los datos del que van a ser
        ///                                 dados de Alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 22/Junio/2011 
        ///MODIFICO: Miguel Angel Bedolla Moreno
        ///FECHA_MODIFICO 22/Junio/2011
        ///CAUSA_MODIFICACIÓN Modificaciones en la base de datos.
        ///*******************************************************************************
        public static void Alta_Cajas(Cls_Cat_Pre_Cajas_Negocio Caja)
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
                
                String Caja_ID = Obtener_ID_Consecutivo(Cat_Pre_Cajas.Tabla_Cat_Pre_Caja, Cat_Pre_Cajas.Campo_Caja_Id, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja;
                Mi_SQL = Mi_SQL + " (" + Cat_Pre_Cajas.Campo_Caja_Id + ", " + Cat_Pre_Cajas.Campo_Clave;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Cajas.Campo_Estatus + ", " + Cat_Pre_Cajas.Campo_Comentario;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Cajas.Campo_Foranea;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Cajas.Campo_Usuario_Creo + ", " + Cat_Pre_Cajas.Campo_Usuario_Modifico;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Cajas.Campo_Fecha_Creo + ", " + Cat_Pre_Cajas.Campo_Fecha_Modifico;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Cajas.Campo_Numero_De_Caja + ", " + Cat_Pre_Cajas.Campo_Modulo_Id + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Caja_ID + "', '" + Caja.P_Clave + "'";
                Mi_SQL = Mi_SQL + ",'" + Caja.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ",'" + Caja.P_Comentarios + "'";
                Mi_SQL = Mi_SQL + ",'" + Caja.P_Foranea + "'";
                Mi_SQL = Mi_SQL + ",'" + Cls_Sessiones.Nombre_Empleado.ToUpper()+ "'";
                Mi_SQL = Mi_SQL + ",''";
                Mi_SQL = Mi_SQL + ", sysdate";
                Mi_SQL = Mi_SQL + ",''";
                Mi_SQL = Mi_SQL + ",'" + Caja.P_Numero_De_Caja + "'";
                Mi_SQL = Mi_SQL + ",'" + Caja.P_Modulo + "'";
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Caja. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Modificar_Caja
        ///DESCRIPCIÓN: Actualiza en la Base de Datos una Caja
        ///PARAMENTROS:     
        ///             1. Caja.            Instancia de la Clase de Cajas 
        ///                                 con los datos del Registro 
        ///                                 que va a ser Actualizado.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 22/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Cajas(Cls_Cat_Pre_Cajas_Negocio Caja)
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja;
                Mi_SQL = Mi_SQL + " SET " + Cat_Pre_Cajas.Campo_Clave + " = '" + Caja.P_Clave + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Cajas.Campo_Comentario + " = '" + Caja.P_Comentarios + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Cajas.Campo_Foranea + " = '" + Caja.P_Foranea + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Cajas.Campo_Estatus + " = '" + Caja.P_Estatus + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Cajas.Campo_Modulo_Id + " = '" + Caja.P_Modulo + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Cajas.Campo_Numero_De_Caja + " = '" + Caja.P_Numero_De_Caja + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Cajas.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Cajas.Campo_Fecha_Modifico + " = sysdate";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Cajas.Campo_Caja_Id + " = '" + Caja.P_Caja_ID + "'";
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
                    Mensaje = "Error al intentar modificar un Registro de Cajas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cajas
        ///DESCRIPCIÓN: Obtiene todos las Cajas que estan dadas de 
        ///             alta en la Base de Datos
        ///PARAMENTROS:   
        ///             1.  Caja.           Parametro de donde se sacara si habra o no un filtro de busqueda, en este
        ///                                 caso el filtro es la Clave.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 22/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Cajas(Cls_Cat_Pre_Cajas_Negocio Caja)
        {
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT c." + Cat_Pre_Cajas.Campo_Caja_Id;
                Mi_SQL = Mi_SQL + ", c." + Cat_Pre_Cajas.Campo_Clave + "";
                Mi_SQL = Mi_SQL + ", c." + Cat_Pre_Cajas.Campo_Numero_De_Caja + "";
                Mi_SQL = Mi_SQL + ", c." + Cat_Pre_Cajas.Campo_Estatus + "";
                Mi_SQL = Mi_SQL + ", c." + Cat_Pre_Cajas.Campo_Modulo_Id + "";
                Mi_SQL = Mi_SQL + ", c." + Cat_Pre_Cajas.Campo_Comentario + "";
                Mi_SQL = Mi_SQL + ", (select m." + Cat_Pre_Modulos.Campo_Descripcion + " from " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + " m where c." + Cat_Pre_Cajas.Campo_Modulo_Id + "=m." + Cat_Pre_Modulos.Campo_Modulo_Id + ") as MODULO";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " c";
                if (Caja.P_Filtro.Trim().Length == 0)
                {
                    Mi_SQL = Mi_SQL + " WHERE c." + Cat_Pre_Cajas.Campo_Clave + " LIKE '%" + Caja.P_Filtro + "%'";
                }
                Mi_SQL = Mi_SQL + " ORDER BY c." + Cat_Pre_Cajas.Campo_Caja_Id;
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Nombre_Id_Cajas
        ///DESCRIPCIÓN: Obtiene todos las Cajas que estan dadas de 
        ///             alta en la Base de Datos
        ///PARAMENTROS:   
        ///             1.  Caja.           Parametro de donde se sacara si habra o no un filtro de busqueda, en este
        ///                                 caso el filtro es la Clave.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 22/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Nombre_Id_Cajas()
        {
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Pre_Cajas.Campo_Caja_Id;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Cajas.Campo_Numero_De_Caja + "";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Cajas.Campo_Estatus+"='VIGENTE'";
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Cajas.Campo_Caja_Id;
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Cajas
        ///DESCRIPCIÓN: Obtiene a detalle una Caja.
        ///PARAMENTROS:   
        ///             1. P_Caja.   Caja que se va ver a Detalle.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 22/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Pre_Cajas_Negocio Consultar_Datos_Cajas(Cls_Cat_Pre_Cajas_Negocio P_Caja)
        {
            Cls_Cat_Pre_Cajas_Negocio R_Cajas = new Cls_Cat_Pre_Cajas_Negocio();
            String Mi_SQL = "SELECT " + Cat_Pre_Cajas.Campo_Caja_Id + ", " + Cat_Pre_Cajas.Campo_Clave;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Cajas.Campo_Numero_De_Caja;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Cajas.Campo_Modulo_Id;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Cajas.Campo_Estatus;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Cajas.Campo_Comentario;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Cajas.Campo_Foranea;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Cajas.Campo_Caja_Id + " = '" + P_Caja.P_Caja_ID + "'";
            OracleDataReader Data_Reader;
            try
            {
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Cajas.P_Caja_ID = P_Caja.P_Caja_ID;
                while (Data_Reader.Read())
                {
                    R_Cajas.P_Caja_ID = Data_Reader[Cat_Pre_Cajas.Campo_Caja_Id].ToString();
                    R_Cajas.P_Clave = Data_Reader[Cat_Pre_Cajas.Campo_Clave].ToString();
                    String ayudante = ""+Data_Reader[Cat_Pre_Cajas.Campo_Numero_De_Caja];
                    R_Cajas.P_Numero_De_Caja = Convert.ToInt32(ayudante);
                    R_Cajas.P_Modulo = Data_Reader[Cat_Pre_Cajas.Campo_Modulo_Id].ToString();
                    R_Cajas.P_Estatus = Data_Reader[Cat_Pre_Cajas.Campo_Estatus].ToString();
                    R_Cajas.P_Comentarios = Data_Reader[Cat_Pre_Cajas.Campo_Comentario].ToString();
                    R_Cajas.P_Foranea = Data_Reader[Cat_Pre_Cajas.Campo_Foranea].ToString();
                }
                Data_Reader.Close();
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el registro de Cajas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Cajas;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Caja
        ///DESCRIPCIÓN: Elimina una Caja de la Base de Datos.
        ///PARAMENTROS:   
        ///             1. Caja.   Registro que se va a eliminar de la Base de Datos.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 22/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Caja(Cls_Cat_Pre_Cajas_Negocio Caja)
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja;
                Mi_SQL = Mi_SQL + " SET " + Cat_Pre_Cajas.Campo_Estatus + " = 'BAJA'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Cajas.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Cajas.Campo_Fecha_Modifico + " = sysdate";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Cajas.Campo_Caja_Id + " = '" + Caja.P_Caja_ID + "'";
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
                    Mensaje = "Error al intentar eliminar el registro de Cajas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                throw new Exception(Mensaje);
            }
            catch (Exception Ex)
            {
                Mensaje = "Error al intentar eliminar el registro de Cajas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Caja_Foranea
        ///DESCRIPCIÓN: Obtiene Si una caja es foranea o no.
        ///PARAMENTROS:   
        ///             1. P_Caja.   Caja que será consultada para saber si es foranea o no
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 01/Agosto/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Pre_Cajas_Negocio Obtener_Caja_Foranea(Cls_Cat_Pre_Cajas_Negocio P_Caja)
        {
            Cls_Cat_Pre_Cajas_Negocio R_Caja = new Cls_Cat_Pre_Cajas_Negocio();
            String Mi_SQL = "SELECT " + Cat_Pre_Cajas.Campo_Foranea;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Cajas.Campo_Caja_Id + " = '" + P_Caja.P_Caja_ID + "'";
            OracleDataReader Data_Reader;
            try
            {
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                while (Data_Reader.Read())
                {
                    R_Caja.P_Filtro = Data_Reader[Cat_Pre_Cajas.Campo_Foranea].ToString();
                }
                Data_Reader.Close();
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el registro de Cajas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Caja;
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

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cajas_Modulo
        ///DESCRIPCIÓN: Obtiene todos las Cajas que estan dadas de 
        ///             alta en el modulo.
        ///PARAMENTROS:   
        ///             1.  Caja.           Parametro de donde se sacara si habra o no un filtro de busqueda, en este
        ///                                 caso el filtro es la Clave.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 22/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Cajas_Modulo(Cls_Cat_Pre_Cajas_Negocio P_Caja)
        {
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Pre_Cajas.Campo_Caja_Id;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Cajas.Campo_Numero_De_Caja + "";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Cajas.Campo_Estatus + "='VIGENTE' AND MODULO_ID='" + P_Caja.P_Modulo + "'";
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Cajas.Campo_Caja_Id;
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
    }
}