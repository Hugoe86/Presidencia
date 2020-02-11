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
using Presidencia.Catalogo_Recargos_Traslado.Negocio;

/// <summary>
/// Summary description for Cls_Cat_Pre_Recargos_Traslado_Datos
/// </summary>

namespace Presidencia.Catalogo_Recargos_Traslado.Datos
{
    public class Cls_Cat_Pre_Recargos_Traslado_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Recargo
        ///DESCRIPCIÓN: Da de alta en la Base de Datos un nuevo Recargo
        ///PARAMENTROS:     
        ///             1. Recargo.         Instancia de la Clase de Negocio de Recargos de Traslado 
        ///                                 con los datos del que van a ser
        ///                                 dados de Alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 05/Agosto/2011 
        ///MODIFICO: 
        ///FECHA_MODIFICO 
        ///CAUSA_MODIFICACIÓN 
        ///*******************************************************************************
        public static void Alta_Recargo(Cls_Cat_Pre_Recargos_Traslado_Negocio Recargo)
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

                String Recargo_ID = Obtener_ID_Consecutivo(Cat_Pre_Recargos_Traslado.Tabla_Cat_Pre_Recargos_Traslado, Cat_Pre_Recargos_Traslado.Campo_Recargo_Traslado_Id, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Recargos_Traslado.Tabla_Cat_Pre_Recargos_Traslado;
                Mi_SQL = Mi_SQL + " (" + Cat_Pre_Recargos_Traslado.Campo_Recargo_Traslado_Id + ", " + Cat_Pre_Recargos_Traslado.Campo_Anio;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Traslado.Campo_Cuota ;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Traslado.Campo_Usuario_Creo + ", " + Cat_Pre_Recargos_Traslado.Campo_Fecha_Creo+ ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Recargo_ID + "', " + Recargo.P_Anio ;
                Mi_SQL = Mi_SQL + "," + Recargo.P_Cuota + "";
                Mi_SQL = Mi_SQL + ",'" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'";
                Mi_SQL = Mi_SQL + ",sysdate)";
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Recargos de Traslado. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Modificar_Recargo
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Recargo.
        ///PARAMENTROS:     
        ///             1. Recargo.         Instancia de la Clase de Recargos de Traslado 
        ///                                 con los datos del Registro 
        ///                                 que va a ser Actualizado.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 05/Agosto/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Recargo(Cls_Cat_Pre_Recargos_Traslado_Negocio Recargo)
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Recargos_Traslado.Tabla_Cat_Pre_Recargos_Traslado;
                Mi_SQL = Mi_SQL + " SET " + Cat_Pre_Recargos_Traslado.Campo_Anio + " = " + Recargo.P_Anio + "";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Recargos_Traslado.Campo_Cuota + " = " + Recargo.P_Cuota + "";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Recargos_Traslado.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Recargos_Traslado.Campo_Fecha_Modifico + " = sysdate";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Recargos_Traslado.Campo_Recargo_Traslado_Id + " = '" + Recargo.P_Recargo_Traslado_ID + "'";
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
                    Mensaje = "Error al intentar modificar un Registro de Recargos de Traslado. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Recargos
        ///DESCRIPCIÓN: Obtiene todos los Recargos de Traslado que estan dados de 
        ///             alta en la Base de Datos
        ///PARAMENTROS:
        ///                 Traslado    Contiene los campos necesarios para hacer un filtrado de 
        ///                             información en base a la descripción, si es que se
        ///                             introdujeron datos de busqueda.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 05/Agosto/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Recargos(Cls_Cat_Pre_Recargos_Traslado_Negocio Recargo)
        {
            DataTable tabla = new DataTable();
                try
                {
                    String Mi_SQL = "SELECT " + Cat_Pre_Recargos_Traslado.Campo_Recargo_Traslado_Id;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Traslado.Campo_Anio + "";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Traslado.Campo_Cuota + "";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Recargos_Traslado.Tabla_Cat_Pre_Recargos_Traslado;
                    if(Recargo.P_Filtro.Length!=0)
                    {
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Recargos_Traslado.Campo_Anio + " like '%" + Recargo.P_Filtro + "%'";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Recargos_Traslado.Campo_Anio +" DESC";
                    DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (dataset != null)
                    {
                        tabla = dataset.Tables[0];
                    }
                }
                catch (Exception Ex)
                {
                    String Mensaje = "Error al intentar consultar los registros de Recargos de Traslado. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
            return tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Recargos
        ///DESCRIPCIÓN: Obtiene a detalle un Recargo de Traslado.
        ///PARAMENTROS:   
        ///             1. P_recargo.   Recargo de Traslado que se va ver a Detalle.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 05/agosto/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Pre_Recargos_Traslado_Negocio Consultar_Datos_Recargos(Cls_Cat_Pre_Recargos_Traslado_Negocio P_Recargo)
        {
            Cls_Cat_Pre_Recargos_Traslado_Negocio R_Recargo = new Cls_Cat_Pre_Recargos_Traslado_Negocio();
            String Mi_SQL = "SELECT " + Cat_Pre_Recargos_Traslado.Campo_Recargo_Traslado_Id ;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Traslado.Campo_Anio;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos_Traslado.Campo_Cuota;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Recargos_Traslado.Tabla_Cat_Pre_Recargos_Traslado;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Recargos_Traslado.Campo_Recargo_Traslado_Id + " = '" + P_Recargo.P_Recargo_Traslado_ID + "'";
            OracleDataReader Data_Reader;
            try
            {
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Recargo.P_Recargo_Traslado_ID = P_Recargo.P_Recargo_Traslado_ID;
                while (Data_Reader.Read())
                {
                    R_Recargo.P_Recargo_Traslado_ID = Data_Reader[Cat_Pre_Recargos_Traslado.Campo_Recargo_Traslado_Id].ToString();
                    R_Recargo.P_Anio = Data_Reader[Cat_Pre_Recargos_Traslado.Campo_Anio].ToString();
                    R_Recargo.P_Cuota = Data_Reader[Cat_Pre_Recargos_Traslado.Campo_Cuota].ToString();
                }
                Data_Reader.Close();
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el registro de Recargos de Traslado. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Recargo;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Anio_Existente
        ///DESCRIPCIÓN: Identifica si ya hay una cuata para ese año.
        ///PARAMENTROS:   
        ///             1.  Recargo.       Parametro de donde se sacará si existe la apertura de esa caja ese día
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 05/Agosto/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static bool Consultar_Anio_Existente(Cls_Cat_Pre_Recargos_Traslado_Negocio Recargo)
        {
            bool Fecha = false;
            try
            {
                String Mi_SQL = "SELECT " + Cat_Pre_Recargos_Traslado.Campo_Recargo_Traslado_Id;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Recargos_Traslado.Tabla_Cat_Pre_Recargos_Traslado;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Recargos_Traslado.Campo_Anio + " = '" + Recargo.P_Anio + "'";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset.Tables[0].Rows[0].ItemArray.Length > 0)
                {
                    Fecha = true;
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Recargos de Traslado. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Fecha;
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