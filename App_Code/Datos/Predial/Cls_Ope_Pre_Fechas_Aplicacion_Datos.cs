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
using System.Collections.Generic;
using System.Text;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
using Presidencia.Operacion_Fechas_Aplicacion.Negocio;
using Presidencia.Sessiones;

/// <summary>
/// Summary description for Cls_Cat_Pre_Fechas_Aplicacion_Datos
/// </summary>

namespace Presidencia.Operacion_Fechas_Aplicacion.Datos
{
    public class Cls_Ope_Pre_Fechas_Aplicacion_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Fecha
        ///DESCRIPCIÓN: Da de alta la fecha de pago de la caja
        ///PARAMENTROS: Fecha_Aplicacion: Contiene los datos a dar de alta y que fueron
        ///                               enviados desde la capa de negocios
        ///CREO       : Miguel Angel Bedolla Moreno.
        ///FECHA_CREO : 22/Junio/2011 
        ///MODIFICO          :
        ///FECHA_MODIFICO    :
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static void Alta_Fecha(Cls_Ope_Pre_Fechas_Aplicacion_Negocio Fecha_Aplicacion)
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
                String Fecha_ID = Obtener_ID_Consecutivo(Cat_Pre_Fechas_Aplicacion.Tabla_Cat_Pre_Fechas_Aplicacion, Cat_Pre_Fechas_Aplicacion.Campo_Fecha_Aplicacion_ID, 5);

                String Mi_SQL = "INSERT INTO " + Cat_Pre_Fechas_Aplicacion.Tabla_Cat_Pre_Fechas_Aplicacion;
                Mi_SQL = Mi_SQL + " (" + Cat_Pre_Fechas_Aplicacion.Campo_Fecha_Aplicacion_ID + ", " + Cat_Pre_Fechas_Aplicacion.Campo_Fecha_Alta;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Fechas_Aplicacion.Campo_Estatus + ", " + Cat_Pre_Fechas_Aplicacion.Campo_Fecha_Movimiento;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Fechas_Aplicacion.Campo_Fecha_Aplicacion + ", " + Cat_Pre_Fechas_Aplicacion.Campo_Motivo;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Fechas_Aplicacion.Campo_Usuario_Creo + ", " + Cat_Pre_Fechas_Aplicacion.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Fecha_ID + "', '" + Fecha_Aplicacion.P_Fecha_Alta + "'";
                Mi_SQL = Mi_SQL + ",'" + Fecha_Aplicacion.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ",'" + Fecha_Aplicacion.P_Fecha_Movimiento + "'";
                Mi_SQL = Mi_SQL + ",'" + Fecha_Aplicacion.P_Fecha_Aplicacion + "'";
                Mi_SQL = Mi_SQL + ",'" + Fecha_Aplicacion.P_Motivo + "'";
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
                    Mensaje = "Error al intentar dar de Alta un Registro de una Fechas de Aplicación. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Modificar_Fecha
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
        public static void Modificar_Fecha(Cls_Ope_Pre_Fechas_Aplicacion_Negocio Fecha_Aplicacion)
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Fechas_Aplicacion.Tabla_Cat_Pre_Fechas_Aplicacion;
                Mi_SQL = Mi_SQL + " SET " + Cat_Pre_Fechas_Aplicacion.Campo_Fecha_Alta + " = '" + Fecha_Aplicacion.P_Fecha_Alta + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Fechas_Aplicacion.Campo_Estatus + " = '" + Fecha_Aplicacion.P_Estatus + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Fechas_Aplicacion.Campo_Fecha_Movimiento + " = '" + Fecha_Aplicacion.P_Fecha_Movimiento + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Fechas_Aplicacion.Campo_Fecha_Aplicacion + " = '" + Fecha_Aplicacion.P_Fecha_Aplicacion + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Fechas_Aplicacion.Campo_Motivo + " = '" + Fecha_Aplicacion.P_Motivo + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Fechas_Aplicacion.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Fechas_Aplicacion.Campo_Fecha_Modifico + " = sysdate";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Fechas_Aplicacion.Campo_Fecha_Aplicacion_ID + " = '" + Fecha_Aplicacion.P_Fecha_Aplicacion_ID + "'";
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
                    Mensaje = "Error al intentar modificar un Registro de Fechas de Aplicación. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Fechas
        ///DESCRIPCIÓN: Consulta los valores que estan dados de alta de acuerdo a la fecha
        ///             proporcionada por el usurio
        ///PARAMENTROS: Fecha_Aplicacion: Obtiene la fecha que fue proporcionada por el
        ///                               usuario y que viene de la capa de negocios
        ///CREO       : Miguel Angel Bedolla Moreno.
        ///FECHA_CREO : 22/Junio/2011 
        ///MODIFICO          : 
        ///FECHA_MODIFICO    : 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************
        public static DataTable Consultar_Fechas(Cls_Ope_Pre_Fechas_Aplicacion_Negocio Fecha_Aplicacion)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la consulta a realizar a la base de datos
                try
                {
                    //Consulta los datos generales de la fecha que fue proporcionada por el usuario
                    Mi_SQL.Append("SELECT " + Cat_Pre_Fechas_Aplicacion.Campo_Fecha_Aplicacion_ID + ", ");
                    Mi_SQL.Append(Cat_Pre_Fechas_Aplicacion.Campo_Fecha_Movimiento + ", ");
                    Mi_SQL.Append(Cat_Pre_Fechas_Aplicacion.Campo_Fecha_Aplicacion + ", ");
                    Mi_SQL.Append(Cat_Pre_Fechas_Aplicacion.Campo_Motivo + ",");
                    Mi_SQL.Append(Cat_Pre_Fechas_Aplicacion.Campo_Estatus);
                    Mi_SQL.Append(" FROM " + Cat_Pre_Fechas_Aplicacion.Tabla_Cat_Pre_Fechas_Aplicacion);
                    if(!String.IsNullOrEmpty(Fecha_Aplicacion.P_Filtro))
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Pre_Fechas_Aplicacion.Campo_Fecha_Aplicacion);
                        Mi_SQL.Append(" BETWEEN TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Fecha_Aplicacion.P_Filtro)) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')");
                        Mi_SQL.Append(" AND TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Fecha_Aplicacion.P_Filtro)) + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS')");
                    }
                    Mi_SQL.Append(" ORDER BY " + Cat_Pre_Fechas_Aplicacion.Campo_Fecha_Movimiento + " DESC ");
                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                }
                catch (OracleException Ex)
                {
                    throw new Exception("Error: " + Ex.Message);
                }
                catch (DBConcurrencyException Ex)
                {
                    throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error: " + Ex.Message);
                }
                finally
                {
                }
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
        public static Cls_Ope_Pre_Fechas_Aplicacion_Negocio Consultar_Datos_Fechas(Cls_Ope_Pre_Fechas_Aplicacion_Negocio P_Fechas)
        {
            Cls_Ope_Pre_Fechas_Aplicacion_Negocio R_Fechas = new Cls_Ope_Pre_Fechas_Aplicacion_Negocio();
            String Mi_SQL = "SELECT " + Cat_Pre_Fechas_Aplicacion.Campo_Fecha_Aplicacion_ID + ", " + Cat_Pre_Fechas_Aplicacion.Campo_Fecha_Alta;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Fechas_Aplicacion.Campo_Fecha_Aplicacion;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Fechas_Aplicacion.Campo_Fecha_Movimiento;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Fechas_Aplicacion.Campo_Estatus;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Fechas_Aplicacion.Campo_Motivo;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Fechas_Aplicacion.Tabla_Cat_Pre_Fechas_Aplicacion;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Fechas_Aplicacion.Campo_Fecha_Aplicacion_ID + " = '" + P_Fechas.P_Fecha_Aplicacion_ID + "'";
            OracleDataReader Data_Reader;
            try
            {
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Fechas.P_Fecha_Aplicacion_ID = P_Fechas.P_Fecha_Aplicacion_ID;
                while (Data_Reader.Read())
                {
                    R_Fechas.P_Fecha_Aplicacion_ID = Data_Reader[Cat_Pre_Fechas_Aplicacion.Campo_Fecha_Aplicacion_ID].ToString();
                    R_Fechas.P_Fecha_Alta = Data_Reader[Cat_Pre_Fechas_Aplicacion.Campo_Fecha_Alta].ToString();
                    R_Fechas.P_Fecha_Aplicacion = Data_Reader[Cat_Pre_Fechas_Aplicacion.Campo_Fecha_Aplicacion].ToString();
                    R_Fechas.P_Fecha_Movimiento = Data_Reader[Cat_Pre_Fechas_Aplicacion.Campo_Fecha_Movimiento].ToString();
                    R_Fechas.P_Estatus = Data_Reader[Cat_Pre_Fechas_Aplicacion.Campo_Estatus].ToString();
                    R_Fechas.P_Motivo = Data_Reader[Cat_Pre_Fechas_Aplicacion.Campo_Motivo].ToString();
                }
                Data_Reader.Close();
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el registro de Fechas de Aplicacion. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Fechas;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Fechas_Inhabiles
        ///DESCRIPCIÓN: Consulta que el día proporcionado por el usuario es un día inhabil
        ///             si lo es retorna un valor de verdadero, si el día proporcionado no
        ///             esta como día inhabil entonces retorna un valor de False
        ///PARAMENTROS: Fecha_Aplicacion: Indica el día a consultar como día inhabil
        ///CREO       : Miguel Angel Bedolla Moreno.
        ///FECHA_CREO : 22/Junio/2011 
        ///MODIFICO          :
        ///FECHA_MODIFICO    :
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Consultar_Fechas_Inhabiles(String Fecha_Aplicacion)
        {
            Boolean Fecha_Inhabil = false;//Obtiene un valor true si es un día inhabil, si es false es un día habil para el usuario
            StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la consulta a realizar a la base de datos

            try
            {
                //Consulta si el día proporcionado por el usuario es un día inhabil
                Mi_SQL.Append("SELECT " + Ope_Pre_Dias_Inhabiles.Campo_Dia_ID);
                Mi_SQL.Append(" FROM " + Ope_Pre_Dias_Inhabiles.Tabla_Ope_Pre_Dias_Inhabiles);
                Mi_SQL.Append(" WHERE " + Ope_Pre_Dias_Inhabiles.Campo_Fecha);
                Mi_SQL.Append(" BETWEEN TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Fecha_Aplicacion)) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')");
                Mi_SQL.Append(" AND TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Fecha_Aplicacion)) + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS')");
                DataTable Dt_Dia_Inhabil = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                if (Dt_Dia_Inhabil.Rows.Count > 0) Fecha_Inhabil = true;
                return Fecha_Inhabil;
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Fecha_Aplicacion
        ///DESCRIPCIÓN: Obtiene La fecha de aplicación del día inhabil.
        ///PARAMENTROS: 1. P_Fechas: Fecha de movimiento que se va a obtener para reemplazar un día inhabil.
        ///CREO       : Miguel Angel Bedolla Moreno.
        ///FECHA_CREO : 01/Agosto/2011 
        ///MODIFICO          :
        ///FECHA_MODIFICO    :
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Cls_Ope_Pre_Fechas_Aplicacion_Negocio Obtener_Fecha_Aplicacion(Cls_Ope_Pre_Fechas_Aplicacion_Negocio P_Fechas)
        {
            Cls_Ope_Pre_Fechas_Aplicacion_Negocio R_Fecha = new Cls_Ope_Pre_Fechas_Aplicacion_Negocio(); //Regresa la fecha de aplicación a la capa de negocios
            try
            {
                //Consulta la fecha de aplicación que va a tener la fecha del movimiento que se pretende dar de alta
                String Mi_SQL = "SELECT " + Cat_Pre_Fechas_Aplicacion.Campo_Fecha_Aplicacion;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Fechas_Aplicacion.Tabla_Cat_Pre_Fechas_Aplicacion;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Fechas_Aplicacion.Campo_Fecha_Movimiento + " BETWEEN TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(P_Fechas.P_Fecha_Movimiento)) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')";
                Mi_SQL = Mi_SQL + " AND TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(P_Fechas.P_Fecha_Movimiento)) + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS')";
                OracleDataReader Data_Reader;

                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                while (Data_Reader.Read())
                {
                    R_Fecha.P_Fecha_Aplicacion = Data_Reader[Cat_Pre_Fechas_Aplicacion.Campo_Fecha_Aplicacion].ToString();
                }
                Data_Reader.Close();
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el registro de Fechas de Aplicacion. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Fecha;
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
        /// NOMBRE DE LA FUNCION: Consulta_Fecha_Repetida
        /// DESCRIPCION : Consulta si la fecha proporcionada por el usuario se encuentra
        ///               dada de alta con aterioridad
        /// PARAMETROS  : Datos: Obtiene los parámetros de consulta a realizar
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 09-Octubre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Fecha_Repetida(Cls_Ope_Pre_Fechas_Aplicacion_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la consulta a realizar para la obtención de los datos

            try
            {
                //Consulta todos los datos de los días inhabiles de acuerdo a los parámetros proporcionados por el usuario
                Mi_SQL.Append("SELECT * FROM " + Cat_Pre_Fechas_Aplicacion.Tabla_Cat_Pre_Fechas_Aplicacion);                
                if (!String.IsNullOrEmpty(Datos.P_Fecha_Aplicacion))
                {
                    Mi_SQL.Append(" WHERE " + Cat_Pre_Fechas_Aplicacion.Campo_Fecha_Aplicacion + " BETWEEN TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Aplicacion)) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')");
                    Mi_SQL.Append(" AND TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Aplicacion)) + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS')");
                }
                if (!String.IsNullOrEmpty(Datos.P_Fecha_Movimiento))
                {
                    Mi_SQL.Append(" WHERE " + Cat_Pre_Fechas_Aplicacion.Campo_Fecha_Movimiento + " BETWEEN TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Movimiento)) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')");
                    Mi_SQL.Append(" AND TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Movimiento)) + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS')");
                }
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }
    }
}