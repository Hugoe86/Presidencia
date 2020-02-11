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
using Presidencia.Catalogo_Descuentos_Predial.Negocio;
/// <summary>
/// Summary description for Cls_Cat_Pre_Descuentos_Predial__Datos
/// </summary>
/// 

namespace Presidencia.Catalogo_Descuentos_Predial.Datos
{

    public class Cls_Cat_Pre_Descuentos_Predial_Datos
    {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Descuento_Predial
        ///DESCRIPCIÓN: Da de alta en la Base de Datos un nuevo Descuento Predial
        ///PARAMETROS:     
        ///             1. Descuento_Predial.   Instancia de la Clase de Negocio de Cls_Cat_Pre_Descuento_Predial_Negocio
        ///                                     con los datos del Descuento Predial que va a ser dada de Alta.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 03/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Descuento_Predial(Cls_Cat_Pre_Descuentos_Predial_Negocio Descuento_Predial)
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
                String Descuento_Predial_ID = Obtener_ID_Consecutivo(Cat_Pre_Descuentos_Predial.Tabla_Cat_Pre_Descuentos_Predial, Cat_Pre_Descuentos_Predial.Campo_Descuento_ID, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Descuentos_Predial.Tabla_Cat_Pre_Descuentos_Predial;
                Mi_SQL = Mi_SQL + " (" + Cat_Pre_Descuentos_Predial.Campo_Descuento_ID + ", " + Cat_Pre_Descuentos_Predial.Campo_Año;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Predial.Campo_Enero + ", " + Cat_Pre_Descuentos_Predial.Campo_Febrero;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Predial.Campo_Marzo + ", " + Cat_Pre_Descuentos_Predial.Campo_Abril;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Predial.Campo_Mayo + ", " + Cat_Pre_Descuentos_Predial.Campo_Junio;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Predial.Campo_Julio + ", " + Cat_Pre_Descuentos_Predial.Campo_Agosto;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Predial.Campo_Septiembre + ", " + Cat_Pre_Descuentos_Predial.Campo_Octubre;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Predial.Campo_Noviembre + ", " + Cat_Pre_Descuentos_Predial.Campo_Diciembre;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Predial.Campo_Usuario_Creo + ", " + Cat_Pre_Descuentos_Predial.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Descuento_Predial_ID + "', " + Descuento_Predial.P_Anio;
                Mi_SQL = Mi_SQL + ", " + Descuento_Predial.P_Enero + ", " + Descuento_Predial.P_Febrero;
                Mi_SQL = Mi_SQL + ", " + Descuento_Predial.P_Marzo + ", " + Descuento_Predial.P_Abril;
                Mi_SQL = Mi_SQL + ", " + Descuento_Predial.P_Mayo + ", " + Descuento_Predial.P_Junio;
                Mi_SQL = Mi_SQL + ", " + Descuento_Predial.P_Julio + ", " + Descuento_Predial.P_Agosto;
                Mi_SQL = Mi_SQL + ", " + Descuento_Predial.P_Septiembre + ", " + Descuento_Predial.P_Octubre;
                Mi_SQL = Mi_SQL + ", " + Descuento_Predial.P_Noviembre + ", " + Descuento_Predial.P_Diciembre;
                Mi_SQL = Mi_SQL + ",'" + Descuento_Predial.P_Usuario + "'" + ", SYSDATE";
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Descuentos Predial. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Modificar_Descuento_Predial
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Descuento Predial
        ///PARAMETROS:     
        ///             1. Descuento_Predial.   Instancia de la Clase de Negocio de Descuentos Predial con los datos 
        ///                                     del Registro que va a ser Actualizado.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 03/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Descuento_Predial(Cls_Cat_Pre_Descuentos_Predial_Negocio Descuento_Predial)
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Descuentos_Predial.Tabla_Cat_Pre_Descuentos_Predial + " SET " + Cat_Pre_Descuentos_Predial.Campo_Año + " = " + Descuento_Predial.P_Anio;
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Descuentos_Predial.Campo_Enero + " = " + Descuento_Predial.P_Enero + "," + Cat_Pre_Descuentos_Predial.Campo_Febrero + " = " + Descuento_Predial.P_Febrero;
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Descuentos_Predial.Campo_Marzo + " = " + Descuento_Predial.P_Marzo + "," + Cat_Pre_Descuentos_Predial.Campo_Abril + " = " + Descuento_Predial.P_Abril;
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Descuentos_Predial.Campo_Mayo + " = " + Descuento_Predial.P_Mayo + "," + Cat_Pre_Descuentos_Predial.Campo_Junio + " = " + Descuento_Predial.P_Junio;
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Descuentos_Predial.Campo_Julio + " = " + Descuento_Predial.P_Julio + "," + Cat_Pre_Descuentos_Predial.Campo_Agosto + " = " + Descuento_Predial.P_Agosto;
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Descuentos_Predial.Campo_Septiembre + " = " + Descuento_Predial.P_Septiembre + "," + Cat_Pre_Descuentos_Predial.Campo_Octubre + " = " + Descuento_Predial.P_Octubre;
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Descuentos_Predial.Campo_Noviembre + " = " + Descuento_Predial.P_Noviembre + "," + Cat_Pre_Descuentos_Predial.Campo_Diciembre + " = " + Descuento_Predial.P_Diciembre;
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Descuentos_Predial.Campo_Usuario_Modifico + " = '" + Descuento_Predial.P_Usuario + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Descuentos_Predial.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Descuentos_Predial.Campo_Descuento_ID + " = '" + Descuento_Predial.P_Descuento_ID + "'";
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
                    Mensaje = "Error al intentar Modificar el Registro de Descuentos Predial. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Descuentos_Predial
        ///DESCRIPCIÓN: Obtiene todos los Descuentos Predial que estan dadas de alta en la Base de Datos
        ///PARAMETROS:   
        ///             1.  Descuentos_Predial.     Parametro de donde se sacara si habra o no un filtro de busqueda, en este
        ///                                         caso el filtro es el año.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 03/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Descuentos_Predial(Cls_Cat_Pre_Descuentos_Predial_Negocio Descuentos_Predial)
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Pre_Descuentos_Predial.Campo_Descuento_ID + ", " + Cat_Pre_Descuentos_Predial.Campo_Año + "";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Descuentos_Predial.Tabla_Cat_Pre_Descuentos_Predial;
                if (!Descuentos_Predial.P_Anio.ToString().Trim().Equals("") && Descuentos_Predial.P_Anio != -1)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Descuentos_Predial.Campo_Año + " = " + Descuentos_Predial.P_Anio;
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Descuentos_Predial.Campo_Año + " DESC";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los Registros de Descuentos Predial. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Descuento_Predial
        ///DESCRIPCIÓN: Obtiene a detalle una Descuento Predial.
        ///PARAMETROS:   
        ///             1. P_Descuento_Predial.   Descuento Predial que se va ver a Detalle.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 03/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Pre_Descuentos_Predial_Negocio Consultar_Datos_Descuento_Predial(Cls_Cat_Pre_Descuentos_Predial_Negocio P_Descuento_Predial)
        {
            String Mi_SQL = "SELECT " + Cat_Pre_Descuentos_Predial.Campo_Año;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Predial.Campo_Enero + ", " + Cat_Pre_Descuentos_Predial.Campo_Febrero;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Predial.Campo_Marzo + ", " + Cat_Pre_Descuentos_Predial.Campo_Abril;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Predial.Campo_Mayo + ", " + Cat_Pre_Descuentos_Predial.Campo_Junio;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Predial.Campo_Julio + ", " + Cat_Pre_Descuentos_Predial.Campo_Agosto;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Predial.Campo_Septiembre + ", " + Cat_Pre_Descuentos_Predial.Campo_Octubre;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Predial.Campo_Noviembre + ", " + Cat_Pre_Descuentos_Predial.Campo_Diciembre;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Descuentos_Predial.Tabla_Cat_Pre_Descuentos_Predial;
            if (!String.IsNullOrEmpty(P_Descuento_Predial.P_Descuento_ID))
            {
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Descuentos_Predial.Campo_Descuento_ID + " = '" + P_Descuento_Predial.P_Descuento_ID + "'";
            }
            else if (P_Descuento_Predial.P_Anio > 0)
            {
                Mi_SQL += " WHERE " + Cat_Pre_Descuentos_Predial.Campo_Año + " = '" + P_Descuento_Predial.P_Anio + "'";
            }
            Cls_Cat_Pre_Descuentos_Predial_Negocio R_Descuento_Predial = new Cls_Cat_Pre_Descuentos_Predial_Negocio();
            OracleDataReader Data_Reader;
            try
            {
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Descuento_Predial.P_Descuento_ID = P_Descuento_Predial.P_Descuento_ID;
                while (Data_Reader.Read())
                {
                    R_Descuento_Predial.P_Anio = Convert.ToInt32(Data_Reader[Cat_Pre_Descuentos_Predial.Campo_Año]);
                    R_Descuento_Predial.P_Enero = Convert.ToDouble(Data_Reader[Cat_Pre_Descuentos_Predial.Campo_Enero]);
                    R_Descuento_Predial.P_Febrero = Convert.ToDouble(Data_Reader[Cat_Pre_Descuentos_Predial.Campo_Febrero]);
                    R_Descuento_Predial.P_Marzo = Convert.ToDouble(Data_Reader[Cat_Pre_Descuentos_Predial.Campo_Marzo]);
                    R_Descuento_Predial.P_Abril = Convert.ToDouble(Data_Reader[Cat_Pre_Descuentos_Predial.Campo_Abril]);
                    R_Descuento_Predial.P_Mayo = Convert.ToDouble(Data_Reader[Cat_Pre_Descuentos_Predial.Campo_Mayo]);
                    R_Descuento_Predial.P_Junio = Convert.ToDouble(Data_Reader[Cat_Pre_Descuentos_Predial.Campo_Junio]);
                    R_Descuento_Predial.P_Julio = Convert.ToDouble(Data_Reader[Cat_Pre_Descuentos_Predial.Campo_Julio]);
                    R_Descuento_Predial.P_Agosto = Convert.ToDouble(Data_Reader[Cat_Pre_Descuentos_Predial.Campo_Agosto]);
                    R_Descuento_Predial.P_Septiembre = Convert.ToDouble(Data_Reader[Cat_Pre_Descuentos_Predial.Campo_Septiembre]);
                    R_Descuento_Predial.P_Octubre = Convert.ToDouble(Data_Reader[Cat_Pre_Descuentos_Predial.Campo_Octubre]);
                    R_Descuento_Predial.P_Noviembre = Convert.ToDouble(Data_Reader[Cat_Pre_Descuentos_Predial.Campo_Noviembre]);
                    R_Descuento_Predial.P_Diciembre = Convert.ToDouble(Data_Reader[Cat_Pre_Descuentos_Predial.Campo_Diciembre]);
                }
                Data_Reader.Close();
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el Registros de Descuentos Predial. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Descuento_Predial;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Descuento_Predial
        ///DESCRIPCIÓN: Elimina un Descuento Predial de la Base de Datos.
        ///PARAMETROS:   
        ///             1. Descuento_Predial.   Registro que se va a eliminar.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 03/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Descuento_Predial(Cls_Cat_Pre_Descuentos_Predial_Negocio Descuento_Predial)
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
                String Mi_SQL = "DELETE FROM " + Cat_Pre_Descuentos_Predial.Tabla_Cat_Pre_Descuentos_Predial;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Descuentos_Predial.Campo_Descuento_ID + " = '" + Descuento_Predial.P_Descuento_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            }
            catch (OracleException Ex)
            {
                if (Ex.Code == 547)
                {
                    Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos. Error: [" + Ex.Message + "]";
                }
                else
                {
                    Mensaje = "Error al intentar Eliminar los registros de Descuentos Predial. Error: [" + Ex.Message + "]";
                }
                throw new Exception(Mensaje);
            }
            catch (Exception Ex)
            {
                Mensaje = "Error al intentar Eliminar los registros de Descuentos Predial. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN     : Consultar_Descuento_Mes
        ///DESCRIPCIÓN              : Devuelve el valor del Descuento que aplica para el Año y Mes indicados
        ///PARÁMETROS               : Descuento_Predial instancia de la Clase para obtener los parámetros desdes el usuario
        ///CREO                     : Antonio Salvador Benavides Guardado
        ///FECHA_CREO               : 22/Noviembre/2011
        ///MODIFICO: 
        ///FECHA_MODIFICO: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************
        public static double Consultar_Descuento_Mes(Cls_Cat_Pre_Descuentos_Predial_Negocio Descuento_Predial)
        {
            DataTable Tabla = new DataTable();
            Double Descuento = 0;
            String Mi_SQL;
            try
            {
                Mi_SQL = "SELECT NVL(" + Descuento_Predial.P_Mes + ", 0) AS " + Descuento_Predial.P_Mes;
                Mi_SQL += " FROM " + Cat_Pre_Descuentos_Predial.Tabla_Cat_Pre_Descuentos_Predial;
                Mi_SQL += " WHERE " + Cat_Pre_Descuentos_Predial.Campo_Año + " = " + Descuento_Predial.P_Anio;
                DataSet dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataSet != null)
                {
                    Tabla = dataSet.Tables[0];
                    if (Tabla.Rows.Count > 0)
                    {
                        Descuento = Convert.ToDouble(Tabla.Rows[0][0]);
                    }
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el registro del Descuento. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Descuento;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Validar_Existe
        ///DESCRIPCIÓN          : Devuelve un DataTable si encontró registros con los parámetros proporcionados
        ///PARAMETROS           : 
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 15/Diciembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Validar_Existe(Cls_Cat_Pre_Descuentos_Predial_Negocio Descuento_Predial)
        {
            DataSet Ds_Descuentos_Predial = null;
            Boolean Existente = false;
            String Mi_SQL;

            try
            {
                Mi_SQL = "SELECT ";
                Mi_SQL += Cat_Pre_Descuentos_Predial.Campo_Descuento_ID + ", " + Cat_Pre_Descuentos_Predial.Campo_Año;
                Mi_SQL += " FROM " + Cat_Pre_Descuentos_Predial.Tabla_Cat_Pre_Descuentos_Predial;
                Mi_SQL += " WHERE ";
                if (Descuento_Predial.P_Descuento_ID != null)
                {
                    if (Descuento_Predial.P_Descuento_ID.Trim() != "")
                    {
                        Mi_SQL += Cat_Pre_Descuentos_Predial.Campo_Descuento_ID + " != '" + Descuento_Predial.P_Descuento_ID + "' AND ";
                    }
                }
                Mi_SQL += Cat_Pre_Descuentos_Predial.Campo_Año + " = '" + Descuento_Predial.P_Anio + "'";

                Ds_Descuentos_Predial = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Descuentos_Predial != null)
                {
                    if (Ds_Descuentos_Predial.Tables[0] != null)
                    {
                        if (Ds_Descuentos_Predial.Tables[0].Rows.Count > 0)
                        {
                            Existente = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Descuentos Predial. Error: [" + ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Existente;
        }
    }
}