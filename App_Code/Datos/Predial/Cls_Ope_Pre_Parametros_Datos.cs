using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Operacion_Predial_Parametros.Negocio;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using System.Data;

namespace Presidencia.Operacion_Predial_Parametros.Datos
{
    public class Cls_Ope_Pre_Parametros_Datos
    {
        #region Metodos
        public Cls_Ope_Pre_Parametros_Datos()
        {
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Modificar_Parametros
        ///DESCRIPCIÓN: establece el valor de los parametros de predial
        ///PARAMETROS: 
        ///CREO: jesus toledo
        ///FECHA_CREO: 24/Junio/2011 10:51:24 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Parametros()
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Ope_Pre_Parametros.Tabla_Ope_Pre_Parametros + ".*";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Parametros.Tabla_Ope_Pre_Parametros;                                
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Parámetros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consultar_Parametro_Caja_Pagos_Internet
        ///DESCRIPCIÓN: Consulta el campo caja_id_pago_internet para hacer regerencia al id de la caja para pagos en internet.
        ///PARAMETROS: 
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 14/Diciembre/2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Parametro_Caja_Pagos_Internet()
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Ope_Pre_Parametros.Tabla_Ope_Pre_Parametros + "."+Ope_Pre_Parametros.Campo_Caja_Id_Pago_Internet;
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Parametros.Tabla_Ope_Pre_Parametros;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Parámetros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consultar_Parametro_Caja_Pagos_Pae
        ///DESCRIPCIÓN: Consulta el campo caja_id_pago_Pae para hacer regerencia al id de la caja para pagos en Pae.
        ///PARAMETROS: 
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 14/Diciembre/2011
        ///MODIFICO: Armando Zavala Moreno  
        ///FECHA_MODIFICO: 08&
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Parametro_Caja_Pagos_Pae()
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Ope_Pre_Parametros.Tabla_Ope_Pre_Parametros + "." + Ope_Pre_Parametros.Campo_Caja_Id_Pago_Pae;
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Parametros.Tabla_Ope_Pre_Parametros;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Parámetros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consultar_Dias_Vencimiento
        ///DESCRIPCIÓN: Devuelve el número de días que tiene de vencimiento.
        ///PARAMETROS: 
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 30/Agosto/2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Int32 Consultar_Dias_Vencimiento()
        {
            Int32 Resultado = 0;
            String Mi_SQL = "SELECT " + Ope_Pre_Parametros.Campo_Dias_Vencimiento + "";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Parametros.Tabla_Ope_Pre_Parametros;
            OracleDataReader Data_Reader;
            try
            {
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                while (Data_Reader.Read())
                {
                    Resultado = Convert.ToInt32(Data_Reader[Ope_Pre_Parametros.Campo_Dias_Vencimiento].ToString());
                }
                Data_Reader.Close();
            }
            catch (Exception Ex)
            {
                String Mensaje = "No hay días de vencimiento dados de alta en el sistema. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Resultado;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Anio_Corriente
        /// DESCRIPCIÓN: Regresa un entero con el anio corriente registrado en la tabla de parametros
        /// PARÁMETROS:
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 22-oct-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static Int32 Consultar_Anio_Corriente()
        {
            Int32 Anio_Corriente = 0;
            Object Resultado_Consulta;
            String Mi_SQL;

            Mi_SQL = "SELECT " + Ope_Pre_Parametros.Campo_Anio_Corriente;
            Mi_SQL += " FROM " + Ope_Pre_Parametros.Tabla_Ope_Pre_Parametros;

            Resultado_Consulta = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            Int32.TryParse(Resultado_Consulta.ToString(), out Anio_Corriente);

            return Anio_Corriente;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Modificar_Anio_Corriente
        /// DESCRIPCIÓN: Modificar
        /// PARÁMETROS:
        /// 	    1. Anio_Corriente: Entero con el año a guardar como parametro
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 02-nov-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static Int32 Modificar_Anio_Corriente(Int32 Anio_Corriente)
        {
            String Mi_SQL = "";

            if (Anio_Corriente > 0)
            {
                Mi_SQL = "UPDATE " + Ope_Pre_Parametros.Tabla_Ope_Pre_Parametros + " SET ";
                Mi_SQL += Ope_Pre_Parametros.Campo_Anio_Corriente + " = '" + Anio_Corriente + "'";
            }

            try
            {
                return OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Modificar_Parametros
        ///DESCRIPCIÓN: establece el valor de los parametros de predial
        ///PARAMETROS:
        ///CREO: jesus toledo
        ///FECHA_CREO: 24/Junio/2011 10:51:24 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static void Modificar_Parametros(Cls_Ope_Pre_Parametros_Negocio Datos)
        {
            //Declaracion de variables            
            OracleConnection Obj_Conexion = new OracleConnection();
            OracleCommand Obj_Comando = new OracleCommand();
            OracleTransaction Obj_Transaccion;
            Obj_Conexion.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Obj_Conexion.Open();
            Obj_Transaccion = Obj_Conexion.BeginTransaction();
            Obj_Comando.Connection = Obj_Conexion;
            Obj_Comando.Transaction = Obj_Transaccion;            
            String Mi_SQL = String.Empty;
            Object Aux; //Variable auxiliar para las consultas
            String Mensaje = String.Empty; //Variable para el mensaje de error
            try
            {
                Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
                Obj_Comando = new OracleCommand();
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;

                //Asignar consulta para el ID
                Mi_SQL = "SELECT COUNT(*) FROM " + Ope_Pre_Parametros.Tabla_Ope_Pre_Parametros;                

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Aux = Obj_Comando.ExecuteScalar();

                //Verificar si es nulo
                if (Convert.ToInt32(Aux) < 1)
                {
                    //Asignar consulta para la insercion
                    Mi_SQL = "INSERT INTO " + Ope_Pre_Parametros.Tabla_Ope_Pre_Parametros + " (";
                    Mi_SQL = Mi_SQL + Ope_Pre_Parametros.Campo_Recargas_Traslado + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Parametros.Campo_Constancia_No_Adeudo + "";
                    //Mi_SQL = Mi_SQL + Ope_Pre_Parametros.Campo_Respaldo_Descuento_Tras + ",";
                    //Mi_SQL = Mi_SQL + Ope_Pre_Parametros.Campo_Porcentaje_Cobro_Honorarios + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Parametros.Campo_Tope_Salario + ",";
                    //Mi_SQL = Mi_SQL + Ope_Pre_Parametros.Campo_Diferencia_Adeudo + ",";
                    //Mi_SQL = Mi_SQL + Ope_Pre_Parametros.Campo_Anio_Vigencia;
                    Mi_SQL = Mi_SQL + ") VALUES('" + Datos.P_Recargas_Traslado + "',";
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Constancia_No_Adeudo 
                        //+ "','" + Datos.P_Respaldo_Des_Tras
                        //+ "','" + Datos.P_Porcentaje_Cobro
                        + "','" + Datos.P_Tope_Salario + "')";
                        //+ "','" + Datos.P_Diferencia_Pago 
                        //+ "','" + Datos.P_Anio_Vigencia + "')";

                    //Ejecutar consulta
                    Obj_Comando.CommandText = Mi_SQL;
                    Obj_Comando.ExecuteNonQuery();

                }
                else
                {
                    //Asignar consulta para la insercion
                    Mi_SQL = "UPDATE " + Ope_Pre_Parametros.Tabla_Ope_Pre_Parametros + " SET ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Parametros.Campo_Recargas_Traslado + " = '" + Datos.P_Recargas_Traslado + "',";
                    Mi_SQL = Mi_SQL + Ope_Pre_Parametros.Campo_Constancia_No_Adeudo + " = '" + Datos.P_Constancia_No_Adeudo + "',";
                    //Mi_SQL = Mi_SQL + Ope_Pre_Parametros.Campo_Respaldo_Descuento_Tras + " = '" + Datos.P_Respaldo_Des_Tras + "',";

                    //Mi_SQL = Mi_SQL + Ope_Pre_Parametros.Campo_Porcentaje_Cobro_Honorarios + " = '" + Datos.P_Porcentaje_Cobro + "',";
                    Mi_SQL = Mi_SQL + Ope_Pre_Parametros.Campo_Tope_Salario + " = '" + Datos.P_Tope_Salario + "'";
                    //Mi_SQL = Mi_SQL + Ope_Pre_Parametros.Campo_Diferencia_Adeudo + " = '" + Datos.P_Diferencia_Pago + "',";
                    //Mi_SQL = Mi_SQL + Ope_Pre_Parametros.Campo_Anio_Vigencia + " = '" + Datos.P_Anio_Vigencia + "'";

                    //Ejecutar consulta
                    Obj_Comando.CommandText = Mi_SQL;
                    Obj_Comando.ExecuteNonQuery();
                }

                //Ejecutar transaccion
                Obj_Transaccion.Commit();
            }
            catch (OracleException Ex)
            {
                Obj_Transaccion.Rollback();
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
                    Mensaje = "Error al intentar dar de Alta Los Parámetros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                Obj_Conexion.Close();
                Obj_Comando = null;
                Obj_Conexion = null;
                Obj_Transaccion = null;
            }            
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Constancias
        ///DESCRIPCIÓN: Consulta de la tabla de constancias el costo, nombre e ID de estas
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 06/27/2011 04:21:27 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        internal static DataTable Consulta_Constancias(Cls_Ope_Pre_Parametros_Negocio Datos)
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Pre_Tipos_Constancias.Campo_Tipo_Constancia_ID + " AS ID, ";
                Mi_SQL = Mi_SQL + Cat_Pre_Tipos_Constancias.Campo_Nombre + " AS NOMBRE, ";
                Mi_SQL = Mi_SQL + Cat_Pre_Tipos_Constancias.Campo_Costo + " AS COSTO ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Tipos_Constancias.Tabla_Cat_Pre_Tipos_Constancias;
                if (Datos.P_Constancia_Nombre == "" || Datos.P_Constancia_Nombre == String.Empty || Datos.P_Constancia_Nombre == null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Tipos_Constancias.Campo_Nombre + " = 'CONSTANCIA DE NO ADEUDO'";  
                }
                else 
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Tipos_Constancias.Campo_Nombre + " LIKE '%" + Datos.P_Constancia_Nombre+ "%'";  
                }
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Constancias. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }
        
        #endregion

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Modificar_Parametros_Cajas
        ///DESCRIPCIÓN: establece el valor de los parametros de Cajas (Predial)
        ///PARAMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 10/Agosto/2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static void Modificar_Parametros_Cajas(Cls_Ope_Pre_Parametros_Negocio Datos)
        {
            //Declaracion de variables            
            OracleConnection Obj_Conexion = new OracleConnection();
            OracleCommand Obj_Comando = new OracleCommand();
            OracleTransaction Obj_Transaccion;
            Obj_Conexion.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Obj_Conexion.Open();
            Obj_Transaccion = Obj_Conexion.BeginTransaction();
            Obj_Comando.Connection = Obj_Conexion;
            Obj_Comando.Transaction = Obj_Transaccion;
            String Mi_SQL = String.Empty;
            Object Aux; //Variable auxiliar para las consultas
            String Mensaje = String.Empty; //Variable para el mensaje de error
            try
            {
                Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
                Obj_Comando = new OracleCommand();
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;

                //Asignar consulta para el ID
                Mi_SQL = "SELECT COUNT(*) FROM " + Ope_Pre_Parametros.Tabla_Ope_Pre_Parametros;

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Aux = Obj_Comando.ExecuteScalar();

                //Verificar si es nulo
                if (Convert.ToInt32(Aux) < 1)
                {
                    //Asignar consulta para la insercion
                    Mi_SQL = "INSERT INTO " + Ope_Pre_Parametros.Tabla_Ope_Pre_Parametros + " (";
                    Mi_SQL = Mi_SQL + Ope_Pre_Parametros.Campo_Tolerancia_Pago_Superior + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Parametros.Campo_Tolerancia_Pago_Inferior;
                    Mi_SQL = Mi_SQL + ") VALUES(" + Datos.P_Tolerancia_Pago_Superior + ", ";
                    Mi_SQL = Mi_SQL + Datos.P_Tolerancia_Pago_Inferior + ")";

                    //Ejecutar consulta
                    Obj_Comando.CommandText = Mi_SQL;
                    Obj_Comando.ExecuteNonQuery();

                }
                else
                {
                    //Asignar consulta para la insercion
                    Mi_SQL = "UPDATE " + Ope_Pre_Parametros.Tabla_Ope_Pre_Parametros + " SET ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Parametros.Campo_Tolerancia_Pago_Superior + " = " + Datos.P_Tolerancia_Pago_Superior;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pre_Parametros.Campo_Tolerancia_Pago_Inferior + " = " + Datos.P_Tolerancia_Pago_Inferior;

                    //Ejecutar consulta
                    Obj_Comando.CommandText = Mi_SQL;
                    Obj_Comando.ExecuteNonQuery();
                }

                //Ejecutar transaccion
                Obj_Transaccion.Commit();
            }
            catch (OracleException Ex)
            {
                Obj_Transaccion.Rollback();
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
                    Mensaje = "Error al intentar dar de Alta Los Parámetros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                Obj_Conexion.Close();
                Obj_Comando = null;
                Obj_Conexion = null;
                Obj_Transaccion = null;
            }
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consultar_Parametros_Cajas
        ///DESCRIPCIÓN: establece el valor de los parametros Cajas
        ///PARAMETROS: 
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 10/Agosto/2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Parametros_Cajas()
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "SELECT " + Ope_Pre_Parametros.Campo_Tolerancia_Pago_Superior + ", " + Ope_Pre_Parametros.Campo_Tolerancia_Pago_Inferior;
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Parametros.Tabla_Ope_Pre_Parametros;

            try
            {
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el registro de Parámetros Cajas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

    }
}