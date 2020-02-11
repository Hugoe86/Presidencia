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
using Presidencia.Catalogo_Tipos_Bienes.Negocio;

/// <summary>
/// Summary description for Cls_Cat_Pre_Tipos_Bienes_Datos
/// </summary>

namespace Presidencia.Catalogo_Tipos_Bienes.Datos
{
    public class Cls_Cat_Pre_Tipos_Bienes_Datos
    {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Bien
        ///DESCRIPCIÓN: Da de alta en la Base de Datos un nuevo bien.
        ///PARAMENTROS:     
        ///             1. Bien.            Instancia de la Clase de Negocio de Tipos de Bienes 
        ///                                 con los datos del que van a ser
        ///                                 dados de Alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 23/Julio/2011 
        ///MODIFICO: 
        ///FECHA_MODIFICO 
        ///CAUSA_MODIFICACIÓN 
        ///*******************************************************************************
        public static void Alta_Bien(Cls_Cat_Pre_Tipos_Bienes_Negocio Bien)
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

                String Bien_ID = Obtener_ID_Consecutivo(Cat_Pre_Tipos_Bienes.Tabla_Cat_Pre_Tipos_Bienes, Cat_Pre_Tipos_Bienes.Campo_Tipo_Bien_Id, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Tipos_Bienes.Tabla_Cat_Pre_Tipos_Bienes;
                Mi_SQL = Mi_SQL + " (" + Cat_Pre_Tipos_Bienes.Campo_Tipo_Bien_Id + ", " + Cat_Pre_Tipos_Bienes.Campo_Nombre;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Tipos_Bienes.Campo_Estatus + ", " + Cat_Pre_Tipos_Bienes.Campo_Descripcion;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Tipos_Bienes.Campo_Usuario_Creo + ", " + Cat_Pre_Tipos_Bienes.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Bien_ID + "', '" + Bien.P_Nombre + "'";
                Mi_SQL = Mi_SQL + ",'" + Bien.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ",'" + Bien.P_Descripcion + "'";
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Tipos de Bienes. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Modificar_Bien
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Bien
        ///PARAMENTROS:     
        ///             1. Bien.            Instancia de la Clase de Tipos de Bienes 
        ///                                 con los datos del Registro 
        ///                                 que va a ser Actualizado.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 26/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Bien(Cls_Cat_Pre_Tipos_Bienes_Negocio Bien)
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Tipos_Bienes.Tabla_Cat_Pre_Tipos_Bienes;
                Mi_SQL = Mi_SQL + " SET " + Cat_Pre_Tipos_Bienes.Campo_Nombre + " = '" + Bien.P_Nombre + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Tipos_Bienes.Campo_Estatus + " = '" + Bien.P_Estatus + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Tipos_Bienes.Campo_Descripcion + " = '" + Bien.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Tipos_Bienes.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Tipos_Bienes.Campo_Fecha_Modifico + " = sysdate";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Tipos_Bienes.Campo_Tipo_Bien_Id + " = '" + Bien.P_Tipo_Bien_ID + "'";
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
                    Mensaje = "Error al intentar modificar un Registro de Tipos de bienes. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Bienes
        ///DESCRIPCIÓN: Obtiene todos los Bienes que estan dadas de 
        ///             alta en la Base de Datos
        ///PARAMENTROS:   
        ///             1.  Bien.           Parametro de donde se sacara si habra o no un filtro de busqueda, en este
        ///                                 caso el filtro es la Nombre.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 22/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Bienes(Cls_Cat_Pre_Tipos_Bienes_Negocio Bien)
        {
            DataTable tabla = new DataTable();
                try
                {
                    String Mi_SQL = "SELECT " + Cat_Pre_Tipos_Bienes.Campo_Tipo_Bien_Id;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Tipos_Bienes.Campo_Nombre + "";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Tipos_Bienes.Campo_Descripcion + "";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Tipos_Bienes.Campo_Estatus + "";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Tipos_Bienes.Tabla_Cat_Pre_Tipos_Bienes + "";
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Tipos_Bienes.Campo_Nombre + " LIKE '%"+Bien.P_Filtro+"%'";
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Tipos_Bienes.Campo_Tipo_Bien_Id;
                    DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (dataset != null)
                    {
                        tabla = dataset.Tables[0];
                    }
                }
                catch (Exception Ex)
                {
                    String Mensaje = "Error al intentar consultar los registros de Tipos de Bienes. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
            return tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Bienes
        ///DESCRIPCIÓN: Obtiene a detalle un Bien.
        ///PARAMENTROS:   
        ///             1. P_Bien.   Bien que se va a ver a Detalle.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 26/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Pre_Tipos_Bienes_Negocio Consultar_Datos_Bienes(Cls_Cat_Pre_Tipos_Bienes_Negocio P_Bien)
        {
            Cls_Cat_Pre_Tipos_Bienes_Negocio R_Bien = new Cls_Cat_Pre_Tipos_Bienes_Negocio();
            String Mi_SQL = "SELECT " + Cat_Pre_Tipos_Bienes.Campo_Tipo_Bien_Id;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Tipos_Bienes.Campo_Nombre;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Tipos_Bienes.Campo_Descripcion;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Tipos_Bienes.Campo_Estatus;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Tipos_Bienes.Tabla_Cat_Pre_Tipos_Bienes;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Tipos_Bienes.Campo_Tipo_Bien_Id + " = '" + P_Bien.P_Tipo_Bien_ID + "'";
            OracleDataReader Data_Reader;
            try
            {
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Bien.P_Tipo_Bien_ID = P_Bien.P_Tipo_Bien_ID;
                while (Data_Reader.Read())
                {
                    R_Bien.P_Tipo_Bien_ID = Data_Reader[Cat_Pre_Tipos_Bienes.Campo_Tipo_Bien_Id].ToString();
                    R_Bien.P_Nombre = Data_Reader[Cat_Pre_Tipos_Bienes.Campo_Nombre].ToString();
                    R_Bien.P_Descripcion = Data_Reader[Cat_Pre_Tipos_Bienes.Campo_Descripcion].ToString();
                    R_Bien.P_Estatus = Data_Reader[Cat_Pre_Tipos_Bienes.Campo_Estatus].ToString();
                }
                Data_Reader.Close();
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el registro de Tipos de Bienes. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Bien;
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