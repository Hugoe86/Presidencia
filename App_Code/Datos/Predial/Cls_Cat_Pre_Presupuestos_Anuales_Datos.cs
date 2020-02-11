using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Constantes;
using Presidencia.Cat_Pre_Presupuestos_Anuales.Negocio;
using System.Data.OracleClient;
using Presidencia.Sessiones;
using System.Data;
using SharpContent.ApplicationBlocks.Data;

/// <summary>
/// Summary description for Cls_Cat_Pre_Presupuestos_Anuales_Datos
/// </summary>

namespace Presidencia.Cat_Pre_Presupuestos_Anuales.Datos
{
public class Cls_Cat_Pre_Presupuestos_Anuales_Datos
{

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Presupuestos
        ///DESCRIPCIÓN: Da de alta en la Base de Datos los presupuestos agregados.
        ///PARAMENTROS:     
        ///             1. Presupuesto.             Instancia de la Clase de Negocio de Presupuestos 
        ///                                         con los datos del que van a ser
        ///                                         dados de Alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 10/Enero/2012
        ///MODIFICO: 
        ///FECHA_MODIFICO 
        ///CAUSA_MODIFICACIÓN 
        ///*******************************************************************************
    public static void Alta_Presupuestos(Cls_Cat_Pre_Presupuestos_Anuales_Negocio Presupuesto)
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
                DataTable Dt_Presupuestos = Presupuesto.P_Dt_Presupustos;
                String Presupuesto_Id = Obtener_ID_Consecutivo(Cat_Pre_Clav_Ing_Presupuestos.Tabla_Cat_Pre_Clav_Ing_Presupuestos, Cat_Pre_Clav_Ing_Presupuestos.Campo_Presupuesto_Id, 5);
                foreach (DataRow Dr_Renglon_Actual in Dt_Presupuestos.Rows)
                {
                    String Mi_SQL = "INSERT INTO " + Cat_Pre_Clav_Ing_Presupuestos.Tabla_Cat_Pre_Clav_Ing_Presupuestos;
                    Mi_SQL = Mi_SQL + " (" + Cat_Pre_Clav_Ing_Presupuestos.Campo_Clave_Ingreso_Id;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Clav_Ing_Presupuestos.Campo_Presupuesto_Id;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Clav_Ing_Presupuestos.Campo_Anio;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Clav_Ing_Presupuestos.Campo_Importe;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Clav_Ing_Presupuestos.Campo_Usuario_Creo + ", " + Cat_Pre_Clav_Ing_Presupuestos.Campo_Fecha_Creo + ")";
                    Mi_SQL = Mi_SQL + " VALUES ('" + Dr_Renglon_Actual[Cat_Pre_Clav_Ing_Presupuestos.Campo_Clave_Ingreso_Id].ToString() + "', '";
                    Mi_SQL = Mi_SQL + Presupuesto_Id + "', ";
                    Mi_SQL = Mi_SQL + Dr_Renglon_Actual[Cat_Pre_Clav_Ing_Presupuestos.Campo_Anio].ToString() + ", ";
                    Mi_SQL = Mi_SQL + Dr_Renglon_Actual[Cat_Pre_Clav_Ing_Presupuestos.Campo_Importe].ToString() + ", '";
                    Mi_SQL = Mi_SQL + Cls_Sessiones.Nombre_Empleado + "', SYSDATE";
                    Mi_SQL = Mi_SQL + ")";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Presupuesto_Id = (Convert.ToInt32(Presupuesto_Id) + 1).ToString("00000");
                }
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Presupuestos Anuales. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Modificar_Presupuestos
        ///DESCRIPCIÓN: Modifica, agrega o elimina los Presupuestos deseados.
        ///PARAMENTROS:     
        ///             1. Presupuestos.            Instancia de la Clase de Presupuestos 
        ///                                         con los datos de los Registros
        ///                                         que van a ser Actualizados.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 10/Enero/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Presupuestos(Cls_Cat_Pre_Presupuestos_Anuales_Negocio Presupuestos)
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
            String Mi_SQL = "";
            String Presupuesto_Id = "";
            try
            {
                DataTable Dt_Presupuestos= Presupuestos.P_Dt_Presupustos;
                if (Dt_Presupuestos != null)
                {
                    foreach (DataRow Dr_Presupuesto in Dt_Presupuestos.Rows)
                    {
                        if (Dr_Presupuesto["ACCION"].ToString() == "ACTUALIZAR")
                        {
                            Mi_SQL = "UPDATE " + Cat_Pre_Clav_Ing_Presupuestos.Tabla_Cat_Pre_Clav_Ing_Presupuestos;
                            Mi_SQL = Mi_SQL + " SET " + Cat_Pre_Clav_Ing_Presupuestos.Campo_Clave_Ingreso_Id + " = '" + Dr_Presupuesto[Cat_Pre_Clav_Ing_Presupuestos.Campo_Clave_Ingreso_Id].ToString() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Clav_Ing_Presupuestos.Campo_Anio + " = " + Dr_Presupuesto[Cat_Pre_Clav_Ing_Presupuestos.Campo_Anio].ToString() + "";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Clav_Ing_Presupuestos.Campo_Importe + " = " + Dr_Presupuesto[Cat_Pre_Clav_Ing_Presupuestos.Campo_Importe].ToString() + "";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Clav_Ing_Presupuestos.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Clav_Ing_Presupuestos.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Clav_Ing_Presupuestos.Campo_Presupuesto_Id + " = '" + Dr_Presupuesto[Cat_Pre_Clav_Ing_Presupuestos.Campo_Presupuesto_Id].ToString() + "'";
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                        }
                        else if (Dr_Presupuesto["ACCION"].ToString() == "AGREGAR")
                        {
                            if (Presupuesto_Id == "")
                            {
                                Presupuesto_Id = Obtener_ID_Consecutivo(Cat_Pre_Clav_Ing_Presupuestos.Tabla_Cat_Pre_Clav_Ing_Presupuestos, Cat_Pre_Clav_Ing_Presupuestos.Campo_Presupuesto_Id, 5);
                            }
                            Mi_SQL = "INSERT INTO " + Cat_Pre_Clav_Ing_Presupuestos.Tabla_Cat_Pre_Clav_Ing_Presupuestos;
                            Mi_SQL = Mi_SQL + " (" + Cat_Pre_Clav_Ing_Presupuestos.Campo_Clave_Ingreso_Id;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Clav_Ing_Presupuestos.Campo_Presupuesto_Id;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Clav_Ing_Presupuestos.Campo_Anio;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Clav_Ing_Presupuestos.Campo_Importe;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Clav_Ing_Presupuestos.Campo_Usuario_Creo + ", " + Cat_Pre_Clav_Ing_Presupuestos.Campo_Fecha_Creo + ")";
                            Mi_SQL = Mi_SQL + " VALUES ('" + Dr_Presupuesto[Cat_Pre_Clav_Ing_Presupuestos.Campo_Clave_Ingreso_Id].ToString() + "', '";
                            Mi_SQL = Mi_SQL + Presupuesto_Id + "', ";
                            Mi_SQL = Mi_SQL + Dr_Presupuesto[Cat_Pre_Clav_Ing_Presupuestos.Campo_Anio].ToString() + ", ";
                            Mi_SQL = Mi_SQL + Dr_Presupuesto[Cat_Pre_Clav_Ing_Presupuestos.Campo_Importe].ToString() + ", '";
                            Mi_SQL = Mi_SQL + Cls_Sessiones.Nombre_Empleado + "', SYSDATE";
                            Mi_SQL = Mi_SQL + ")";
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                            Presupuesto_Id = (Convert.ToInt32(Presupuesto_Id) + 1).ToString("00000");
                        }
                        else if (Dr_Presupuesto["ACCION"].ToString() == "BORRAR")
                        {
                            Mi_SQL = "DELETE " + Cat_Pre_Clav_Ing_Presupuestos.Tabla_Cat_Pre_Clav_Ing_Presupuestos;
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Clav_Ing_Presupuestos.Campo_Presupuesto_Id + "='" + Dr_Presupuesto[Cat_Pre_Clav_Ing_Presupuestos.Campo_Presupuesto_Id].ToString() + "'";
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                        }
                    }
                }
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
                    Mensaje = "Error al intentar modificar los registros de Presupuestos anuales. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Presupuestos
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
        public static DataTable Consultar_Presupuestos(Cls_Cat_Pre_Presupuestos_Anuales_Negocio Presupuestos)
        {
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Pre_Clav_Ing_Presupuestos.Campo_Clave_Ingreso_Id;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Clav_Ing_Presupuestos.Campo_Presupuesto_Id + "";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Clav_Ing_Presupuestos.Campo_Anio + "";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Clav_Ing_Presupuestos.Campo_Importe + "";
                Mi_SQL = Mi_SQL + ", 'NADA' AS ACCION";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Clav_Ing_Presupuestos.Tabla_Cat_Pre_Clav_Ing_Presupuestos;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Clav_Ing_Presupuestos.Campo_Clave_Ingreso_Id + "='"+Presupuestos.P_Clave_Ingreso_Id+"'";
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Clav_Ing_Presupuestos.Campo_Anio;
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