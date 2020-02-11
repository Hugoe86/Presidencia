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
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Operacion_Predial_Quitar_Beneficios.Negocio;
using System.Data.OracleClient;

namespace Presidencia.Operacion_Predial_Quitar_Beneficios.Datos
{
    public class Cls_Ope_Pre_Quitar_Beneficios_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Beneficios
        ///DESCRIPCIÓN: Hace una consulta a la Base de Datos y obtiene el tipo de Predio
        ///PARAMETROS:     
        ///CREO: Jacqueline Ramirez Sierra
        ///FECHA_CREO: 16/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Datos_Beneficios(Cls_Ope_Pre_Quitar_Beneficios_Negocio Datos)
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = " SELECT " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " AS CUENTA_PREDIAL,";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " AS CUENTA_PREDIAL_ID,";
                Mi_SQL = Mi_SQL + Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales + "." + Cat_Pre_Casos_Especiales.Campo_Descripcion + " AS BENEFICIO,";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion + " AS PORCENTAJE,";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija + " AS NO_CUOTA_FIJA,";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija + " AS CUOTA_FIJA,";
                Mi_SQL = Mi_SQL + Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales + "." + Cat_Pre_Casos_Especiales.Campo_Fecha_Creo + " AS FECHA_TRAMITE,";
                Mi_SQL = Mi_SQL + Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales + "." + Cat_Pre_Casos_Especiales.Campo_Estatus + " AS ESTATUS,";
                Mi_SQL = Mi_SQL + Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales + "." + Cat_Pre_Casos_Especiales.Campo_Usuario_Creo;
                ////PROPIETARIO
                //Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || ";
                //Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || ";
                //Mi_SQL = Mi_SQL + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Nombre + " AS NOMBRE_PROPIETARIO";

                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales + " inner join ";
                Mi_SQL = Mi_SQL + Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + " on ";
                Mi_SQL = Mi_SQL + Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + "." + Ope_Pre_Cuotas_Fijas.Campo_Caso_Especial_Id + "=";
                Mi_SQL = Mi_SQL + Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales + "." + Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID + " inner join ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " on ";
                Mi_SQL = Mi_SQL + Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + "." + Ope_Pre_Cuotas_Fijas.Campo_No_Cuota_Fija + "=";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija + " = " + "'SI'";
                if (Datos.P_Beneficio != null)
                {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales + "." + Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID + " ='" + Datos.P_Beneficio + "'";
                }
                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + "." + Ope_Pre_Cuotas_Fijas.Campo_No_Cuota_Fija + " IN (SELECT " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " LIKE '%" + Datos.P_Cuenta_Predial + "%') ";

                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Contrarecibo
        ///DESCRIPCIÓN: Se actualiza de un Contrarecibo la Cuenta_Predial.
        ///PARÁMETROS:     
        ///             1.  Traslado_Dominio.   Contiene las propiedades (Cuenta_Predial y 
        ///                                     No_Contrarecibo) para actualizar el contrarecibo.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        public static void Modificar_Cuota_Fija(Cls_Ope_Pre_Quitar_Beneficios_Negocio Datos)
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
                for (int x = 0; x < Datos.P_Dt_Quitar_Beneficio.Rows.Count; x++)
                {
                    String Mi_SQL = "UPDATE " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas;
                    Mi_SQL = Mi_SQL + " SET " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija + " = " + "'NO'";
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija + " = " + "'SI'";
                    Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija + "='";
                    Mi_SQL = Mi_SQL + Datos.P_Beneficio + "'";

                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
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
                    Mensaje = "Existen datos demasiado extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
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
                    Mensaje = "Error al intentar modificar el Registro. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Modificar_Observaciones_Orden de Variacion
        ///DESCRIPCIÓN: Se concatenan observaciones de Quitar Beneficios 
        ///PARÁMETROS: 1.  Traslado_Dominio.   Contiene las propiedades (Cuenta_Predial y 
        ///            No_Contrarecibo) para actualizar el contrarecibo.
        ///CREO: Jacqueline Ramírez Sierra
        ///FECHA_CREO: 6/Septirmbre/2011 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        ////public static void Modificar_Cuota_Fija(Cls_Ope_Pre_Quitar_Beneficios_Negocio Datos)
        ////{
        ////    String Mensaje = "";
        ////    OracleConnection Cn = new OracleConnection();
        ////    OracleCommand Cmd = new OracleCommand();
        ////    OracleTransaction Trans;
        ////    Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
        ////    Cn.Open();
        ////    Trans = Cn.BeginTransaction();
        ////    Cmd.Connection = Cn;
        ////    Cmd.Transaction = Trans;
        ////    try
        ////    {
        ////        for (int x = 0; x < Datos.P_Dt_Quitar_Beneficio.Rows.Count; x++)
        ////        {
        //String Mi_SQL = "UPDATE " + Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion;
        ////            Mi_SQL = Mi_SQL + " SET " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija + " = " + "'NO'";
        ////            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija + " = " + "'SI'";
        ////            Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija + "='";
        ////            Mi_SQL = Mi_SQL + Datos.P_Beneficio + "'";

        ////            Cmd.CommandText = Mi_SQL;
        ////            Cmd.ExecuteNonQuery();
        ////        }
        ////        Trans.Commit();

        ////    }
        ////    catch (OracleException Ex)
        ////    {
        ////        Trans.Rollback();
        ////        //variable para el mensaje 
        ////        //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
        ////        if (Ex.Code == 8152)
        ////        {
        ////            Mensaje = "Existen datos demasiado extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
        ////        }
        ////        else if (Ex.Code == 2627)
        ////        {
        ////            if (Ex.Message.IndexOf("PRIMARY") != -1)
        ////            {
        ////                Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
        ////            }
        ////            else if (Ex.Message.IndexOf("UNIQUE") != -1)
        ////            {
        ////                Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]";
        ////            }
        ////            else
        ////            {
        ////                Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]";
        ////            }
        ////        }
        ////        else if (Ex.Code == 547)
        ////        {
        ////            Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]";
        ////        }
        ////        else if (Ex.Code == 515)
        ////        {
        ////            Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
        ////        }
        ////        else
        ////        {
        ////            Mensaje = "Error al intentar modificar el Registro. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
        ////        }
        ////        //Indicamos el mensaje 
        ////        throw new Exception(Mensaje);
        ////    }
        ////    finally
        ////    {
        ////        Cn.Close();
        ////    }
        ////}

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Quitar_Beneficio
        ///DESCRIPCIÓN: inserta el Registro de el movimiento de 
        ///             eliminacion del beneficio de una cuenta predial
        ///PARAMETROS:  Datos: instancia de la clase de negocio
        ///CREO:        Jesus Toledo Rodriguez
        ///FECHA_CREO:  02/Enero/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        internal static void Alta_Quitar_Beneficio(Cls_Ope_Pre_Quitar_Beneficios_Negocio Datos)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL
            String Consec = ""; //Variable para la consulta SQL
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Object Aux; //Variable auxiliar para las consultas
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try
            {
                //Formar Sentencia para obtener el consecutivo
                Mi_SQL = "";
                Mi_SQL = "SELECT NVL(MAX(";
                Mi_SQL = Mi_SQL + Ope_Pre_Quitar_Beneficio.Campo_No_Beneficio_ID + "),00000)";
                Mi_SQL = Mi_SQL + " FROM ";
                Mi_SQL = Mi_SQL + Ope_Pre_Quitar_Beneficio.Tabla_Ope_Pre_Quitar_Beneficio;

                //Ejecutar consulta del consecutivo               
                Aux = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0][0];

                //Verificar si no es nulo
                if (Convert.IsDBNull(Aux) == false)
                {
                    Consec = String.Format("{0:00000}", Convert.ToInt32(Aux) + 1);
                }
                else
                    Consec = "00001";

                foreach (DataRow Dr_Beneficio in Datos.P_Dt_Quitar_Beneficio.Rows)
                {                    

                    Mi_SQL = "INSERT INTO " + Ope_Pre_Quitar_Beneficio.Tabla_Ope_Pre_Quitar_Beneficio + " ( ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Quitar_Beneficio.Campo_No_Beneficio_ID + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Quitar_Beneficio.Campo_Caso_Especial_ID + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Quitar_Beneficio.Campo_Fecha_Hora + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Quitar_Beneficio.Campo_Cuenta_Predial_ID + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Quitar_Beneficio.Campo_Quitar_Beneficio + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Quitar_Beneficio.Campo_Observaciones + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Quitar_Beneficio.Campo_Usuario_Creo + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Quitar_Beneficio.Campo_Fecha_Creo + ") ";
                    Mi_SQL = Mi_SQL + " VALUES ('";
                    Mi_SQL = Mi_SQL + Consec + "', '";
                    Mi_SQL = Mi_SQL + Datos.P_Beneficio + "', '";
                    Mi_SQL = Mi_SQL + Datos.P_Fecha_Hora.ToString("dd/MM/yyyy HH:MM:ss") + "','";
                    Mi_SQL = Mi_SQL + Dr_Beneficio[Ope_Pre_Quitar_Beneficio.Campo_Cuenta_Predial_ID] + "', 'SI','";
                    Mi_SQL = Mi_SQL + Datos.P_Observaciones + "', '";
                    Mi_SQL = Mi_SQL + Datos.P_Usuario_Creo + "', sysdate)";

                     Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Consec = String.Format("{0:00000}", Convert.ToInt32(Consec) + 1);
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
                    Mensaje = "Existen datos demasiado extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
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
                    Mensaje = "Error al intentar modificar el Registro. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Consulta_Reporte
        ///DESCRIPCIÓN: Consulta de los datos para mostrar en reporte
        ///PARAMETROS:  Datos: instancia de la clase de negocio
        ///CREO:        Jesus Toledo Rodriguez
        ///FECHA_CREO:  02/Enero/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        internal static DataSet Consulta_Reporte(Cls_Ope_Pre_Quitar_Beneficios_Negocio Datos)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL
            DataSet Ds_Resultado = new DataSet();

            try
            {
                Mi_SQL = "";
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + "q." + Ope_Pre_Quitar_Beneficio.Campo_Cuenta_Predial_ID + ", ";
                Mi_SQL = Mi_SQL + "q." + Ope_Pre_Quitar_Beneficio.Campo_Fecha_Hora + ", '";
                Mi_SQL = Mi_SQL + "100%' AS PORCENTAJE, ";
                Mi_SQL = Mi_SQL + "q." + Ope_Pre_Quitar_Beneficio.Campo_Observaciones + ", ";
                Mi_SQL = Mi_SQL + "c." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ", ";
                Mi_SQL = Mi_SQL + "b." + Cat_Pre_Casos_Especiales.Campo_Descripcion + " AS BENEFICIO";
                Mi_SQL = Mi_SQL + " FROM ";
                Mi_SQL = Mi_SQL + Ope_Pre_Quitar_Beneficio.Tabla_Ope_Pre_Quitar_Beneficio + " q ";
                Mi_SQL = Mi_SQL + " JOIN ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas +" c ";
                Mi_SQL = Mi_SQL + " ON c.";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = q. ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                Mi_SQL = Mi_SQL + " JOIN ";
                Mi_SQL = Mi_SQL + Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales + " b ";
                Mi_SQL = Mi_SQL + " ON b.";
                Mi_SQL = Mi_SQL + Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID + " = q. ";
                Mi_SQL = Mi_SQL + Ope_Pre_Quitar_Beneficio.Campo_Caso_Especial_ID;
                Mi_SQL = Mi_SQL + " WHERE q.";
                Mi_SQL = Mi_SQL + Ope_Pre_Quitar_Beneficio.Campo_Fecha_Hora + " = '";
                Mi_SQL = Mi_SQL + Datos.P_Fecha_Hora.ToString("dd/MM/yyyy HH:MM:ss") + "'";
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Mi_SQL + Ex.Message);
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