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
using Presidencia.Catalogo_Descuentos_Generales.Negocio;

/// <summary>
/// Summary description for Cls_Cat_Pre_Descuentos_Generales_Datos
/// </summary>

namespace Presidencia.Catalogo_Descuentos_Generales.Datos
{
    public class Cls_Cat_Pre_Descuentos_Generales_Datos
    {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Descuentos_Generales
        ///DESCRIPCIÓN: Da de alta en la Base de Datos un nuevo Descuento General
        ///PARAMENTROS:     
        ///             1. Descuento_General.           Instancia de la Clase de Negocio Descuentos Generales 
        ///                                             con los datos del que van a ser
        ///                                             dados de Alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 12/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static void Alta_Descuentos_Generales(Cls_Cat_Pre_Descuentos_Generales_Negocio Descuento_General)
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

                String Descuento_General_ID = Obtener_ID_Consecutivo(Cat_Pre_Descuentos_Generales.Tabla_Cat_Pre_Descuentos_Generales, Cat_Pre_Descuentos_Generales.Campo_Descuentos_Generales_Id, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Descuentos_Generales.Tabla_Cat_Pre_Descuentos_Generales;
                Mi_SQL = Mi_SQL + " (" + Cat_Pre_Descuentos_Generales.Campo_Descuentos_Generales_Id + ", " + Cat_Pre_Descuentos_Generales.Campo_Tipo_De_Descuento;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Generales.Campo_Estatus + ", " + Cat_Pre_Descuentos_Generales.Campo_Vigencia_Desde;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Generales.Campo_Vigencia_Hasta + ", " + Cat_Pre_Descuentos_Generales.Campo_Porcentaje_Descuento;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Generales.Campo_Motivo + ", " + Cat_Pre_Descuentos_Generales.Campo_Comentarios;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Generales.Campo_Usuario_Creo + ", " + Cat_Pre_Descuentos_Generales.Campo_Usuario_Modifico;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Generales.Campo_Fecha_Creo + ", " + Cat_Pre_Descuentos_Generales.Campo_Fecha_Modifico + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Descuento_General_ID + "', '" + Descuento_General.P_Tipo_Impuesto + "'";
                Mi_SQL = Mi_SQL + ",'" + Descuento_General.P_Estatus + "', '" + Descuento_General.P_Vigencia_Desde + "'";
                Mi_SQL = Mi_SQL + ",'" + Descuento_General.P_Vigencia_Hasta + "', " + Descuento_General.P_Porcentaje_Descuento + "";
                Mi_SQL = Mi_SQL + ",'" + Descuento_General.P_Motivo + "', '" + Descuento_General.P_Comentarios + "'";
                Mi_SQL = Mi_SQL + ",'" + Cls_Sessiones.Nombre_Empleado + "'";
                Mi_SQL = Mi_SQL + ",''";
                Mi_SQL = Mi_SQL + ", sysdate";
                Mi_SQL = Mi_SQL + ",''";
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
                    Mensaje = "Error al intentar dar de Alta un Descuento General. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Modificar_Descuentos_Generales
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Descuento General,
        ///PARAMENTROS:     
        ///             1. Descuento_General.           Instancia de la Clase de Descuentos Generales 
        ///                                 con los datos del Registro 
        ///                                 que va a ser Actualizado.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 13/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Descuentos_Generales(Cls_Cat_Pre_Descuentos_Generales_Negocio Descuento_General)
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Descuentos_Generales.Tabla_Cat_Pre_Descuentos_Generales;
                Mi_SQL = Mi_SQL + " SET " + Cat_Pre_Descuentos_Generales.Campo_Descuentos_Generales_Id + " = '" + Descuento_General.P_Descuentos_Generales_Id + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Descuentos_Generales.Campo_Tipo_De_Descuento + " = '" + Descuento_General.P_Tipo_Impuesto + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Descuentos_Generales.Campo_Porcentaje_Descuento + " = " + Descuento_General.P_Porcentaje_Descuento + "";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Descuentos_Generales.Campo_Estatus + " = '" + Descuento_General.P_Estatus + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Descuentos_Generales.Campo_Vigencia_Desde + " = '" + Descuento_General.P_Vigencia_Desde + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Descuentos_Generales.Campo_Vigencia_Hasta + " = '" + Descuento_General.P_Vigencia_Hasta + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Descuentos_Generales.Campo_Comentarios + " = '" + Descuento_General.P_Comentarios + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Descuentos_Generales.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Descuentos_Generales.Campo_Fecha_Modifico + " = sysdate";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Descuentos_Generales.Campo_Descuentos_Generales_Id + " = '" + Descuento_General.P_Descuentos_Generales_Id + "'";
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
                    Mensaje = "Error al intentar modificar un Descuento General. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Descuentos_Generales
        ///DESCRIPCIÓN: Obtiene todos los descuentos generales que estan dados de 
        ///             alta en la Base de Datos
        ///PARAMENTROS:   
        ///             1.  Descuentos_Generales            Parametro de donde se sacara si habra o no un filtro de busqueda, en este
        ///                                                 caso el filtro es el tipo de impuesto y comentario..
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 13/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Descuentos_Generales(Cls_Cat_Pre_Descuentos_Generales_Negocio Descuentos_Generales)
        {
            DataTable tabla = new DataTable();
            if (Descuentos_Generales.P_Filtro.Trim().Length == 0)
            {
                try
                {
                    String Mi_SQL = "SELECT " + Cat_Pre_Descuentos_Generales.Campo_Descuentos_Generales_Id;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Generales.Campo_Tipo_De_Descuento + "";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Generales.Campo_Porcentaje_Descuento + "";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Generales.Campo_Estatus + "";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Generales.Campo_Vigencia_Desde + "";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Generales.Campo_Vigencia_Hasta + "";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Generales.Campo_Comentarios + "";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Descuentos_Generales.Tabla_Cat_Pre_Descuentos_Generales;
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Descuentos_Generales.Campo_Descuentos_Generales_Id;
                    DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (dataset != null)
                    {
                        tabla = dataset.Tables[0];
                    }
                }
                catch (Exception Ex)
                {
                    String Mensaje = "Error al intentar consultar los registros de Descuentos Generales. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
            }
            else
            {
                try
                {
                    String Mi_SQL = "SELECT " + Cat_Pre_Descuentos_Generales.Campo_Descuentos_Generales_Id;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Generales.Campo_Tipo_De_Descuento + "";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Generales.Campo_Porcentaje_Descuento + "";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Generales.Campo_Estatus + "";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Generales.Campo_Vigencia_Desde + "";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Generales.Campo_Vigencia_Hasta + "";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Generales.Campo_Comentarios + "";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Descuentos_Generales.Tabla_Cat_Pre_Descuentos_Generales;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Descuentos_Generales.Campo_Tipo_De_Descuento + " LIKE '%" + Descuentos_Generales.P_Filtro + "%' OR " + Cat_Pre_Descuentos_Generales.Campo_Comentarios + " LIKE '%" + Descuentos_Generales.P_Filtro + "%'";
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Descuentos_Generales.Campo_Descuentos_Generales_Id;
                    DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (dataset != null)
                    {
                        tabla = dataset.Tables[0];
                    }
                }
                catch (Exception Ex)
                {
                    String Mensaje = "Error al intentar consultar los registros de Descuentos Generales. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
            }
            return tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Descuentos_Generales
        ///DESCRIPCIÓN: Obtiene a detalle una Rango de descuento por rol.
        ///PARAMENTROS:   
        ///             1. P_Descuento_General.   descuento General que se va a ver a Detalle.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 13/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Pre_Descuentos_Generales_Negocio Consultar_Datos_Descuentos_Generales(Cls_Cat_Pre_Descuentos_Generales_Negocio P_Descuentos_Generales)
        {
            Cls_Cat_Pre_Descuentos_Generales_Negocio R_Descuento_General = new Cls_Cat_Pre_Descuentos_Generales_Negocio();
            String Mi_SQL = "SELECT " + Cat_Pre_Descuentos_Generales.Campo_Descuentos_Generales_Id + ", " + Cat_Pre_Descuentos_Generales.Campo_Tipo_De_Descuento;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Generales.Campo_Porcentaje_Descuento;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Generales.Campo_Estatus;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Generales.Campo_Vigencia_Desde;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Generales.Campo_Vigencia_Hasta;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Generales.Campo_Comentarios;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Generales.Campo_Motivo;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Descuentos_Generales.Tabla_Cat_Pre_Descuentos_Generales;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Descuentos_Generales.Campo_Descuentos_Generales_Id + " = '" + P_Descuentos_Generales.P_Descuentos_Generales_Id + "'";
            OracleDataReader Data_Reader;
            try
            {
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Descuento_General.P_Descuentos_Generales_Id = P_Descuentos_Generales.P_Descuentos_Generales_Id;
                while (Data_Reader.Read())
                {
                    R_Descuento_General.P_Descuentos_Generales_Id = Data_Reader[Cat_Pre_Descuentos_Generales.Campo_Descuentos_Generales_Id].ToString();
                    R_Descuento_General.P_Tipo_Impuesto = Data_Reader[Cat_Pre_Descuentos_Generales.Campo_Tipo_De_Descuento].ToString();
                    String ayudante = "" + Data_Reader[Cat_Pre_Descuentos_Generales.Campo_Porcentaje_Descuento];
                    R_Descuento_General.P_Porcentaje_Descuento = Convert.ToInt32(ayudante);
                    R_Descuento_General.P_Estatus = Data_Reader[Cat_Pre_Descuentos_Generales.Campo_Estatus].ToString();
                    R_Descuento_General.P_Vigencia_Desde = "" + Data_Reader[Cat_Pre_Descuentos_Generales.Campo_Vigencia_Desde].ToString();
                    R_Descuento_General.P_Vigencia_Hasta = "" + Data_Reader[Cat_Pre_Descuentos_Generales.Campo_Vigencia_Hasta].ToString();
                    R_Descuento_General.P_Comentarios = Data_Reader[Cat_Pre_Descuentos_Generales.Campo_Comentarios].ToString();
                    R_Descuento_General.P_Motivo = Data_Reader[Cat_Pre_Descuentos_Generales.Campo_Motivo].ToString();
                }
                Data_Reader.Close();
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el registro de descuentos Generales. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Descuento_General;
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