using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Operacion_Calculo_Impuesto_Traslado.Negocio;
using Presidencia.Constantes;

namespace Presidencia.Operacion_Calculo_Impuesto_Traslado.Datos
{
    public class Cls_Ope_Pre_Calculo_Impuesto_Traslado_Datos
    {
        public Cls_Ope_Pre_Calculo_Impuesto_Traslado_Datos()
        {
        }
 
        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Alta_Calculo
        /// DESCRIPCIÓN: 1.Consulta el último No_Calculo dado de alta para poder ingresar el siguiente
        ///                  2. Da de Alta el No_Calculo en la BD con los datos proporcionados por el usuario
        ///                  Regresa el número de filas insertadas
        /// PARÁMETROS:
        /// 	1. Datos: Instancia de la clase de negocio con los datos que serán insertados en la base de datos
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 11-ago-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static int Alta_Calculo(Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Datos)
        {
            String Mi_SQL;  //Obtiene la cadena de inserción hacía la base de datos
            Object No_Calculo; //Obtiene el ID con la cual se guardo los datos en la base de datos
            Int32 Filas_Afectadas = 0;
            Int64 Filas_Encontradas = 0;
            String Mensaje = "";
            DateTime Fecha_Escritura;

            // variables para transaccion
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;

            try
            {
                // abrir conexion con la base de datos
                Conexion.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Conexion.Open();
                Transaccion = Conexion.BeginTransaction();
                Comando.Connection = Conexion;
                Comando.Transaction = Transaccion;

                //Valida que no se ingrese un contrarecibo 2 veces
                Mi_SQL = "SELECT NVL(COUNT(" + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo + "),0)";
                Mi_SQL += " FROM " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado;
                Mi_SQL += " WHERE " + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion + " = '" + Datos.P_No_Orden_Variacion + "'";
                Mi_SQL += " AND " + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + " = " + Datos.P_Anio_Orden;
                Comando.CommandText = Mi_SQL;
                Comando.CommandType = CommandType.Text;
                Filas_Encontradas = Convert.ToInt64(Comando.ExecuteScalar());
                if (Filas_Encontradas <= 0)
                {
                    //Obtiene el consecutivo del calculo
                    Mi_SQL = "SELECT NVL(MAX(" + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo + "),'0000000000') ";
                    Mi_SQL += "FROM " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado;
                    Mi_SQL += " WHERE " + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + " = " + Datos.P_Anio_Calculo;
                    Comando.CommandText = Mi_SQL;
                    Comando.CommandType = CommandType.Text;
                    No_Calculo = Comando.ExecuteScalar();

                    if (Convert.IsDBNull(No_Calculo))
                    {
                        Datos.P_No_Calculo = "0000000001";
                    }
                    else
                    {
                        Datos.P_No_Calculo = String.Format("{0:0000000000}", Convert.ToInt32(No_Calculo) + 1);
                    }

                    DateTime.TryParse(Datos.P_Fecha_Escritura, out Fecha_Escritura);

                    //Consulta para la inserción del No_Calculo con los datos proporcionados por el usuario
                    Mi_SQL = "INSERT INTO " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + " (";
                    Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo + ", ";
                    Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + ", ";
                    Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Cuenta_Predial_Id + ", ";
                    Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion + ", ";
                    Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + ", ";
                    Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Predio_Colindante + ", ";
                    Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Base_Impuesto + ", ";
                    Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Minimo_Elevado_Anio + ", ";
                    Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Base_Impuesto_Division + ", ";
                    Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Tasa_ID + ", ";
                    Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Multa_ID + ", ";
                    Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Fecha_Escritura + ", ";
                    Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Impuesto_Division_Lot_Id + ", ";
                    Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Costo_Constancia + ", ";
                    Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Traslado + ", ";
                    Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Division + ", ";
                    Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Multa + ", ";
                    Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Recargos + ", ";
                    Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Total_Pagar + ", ";
                    Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Tipo + ", ";
                    Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Realizo_Calculo + ", ";
                    Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + ", ";
                    Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Usuario_Creo + ", ";
                    Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Fecha_Creo + ") VALUES ('";
                    Mi_SQL += Datos.P_No_Calculo + "', '";
                    Mi_SQL += Datos.P_Anio_Calculo + "', '";
                    Mi_SQL += Datos.P_Cuenta_Predial_ID + "', '";
                    Mi_SQL += Datos.P_No_Orden_Variacion + "', '";
                    Mi_SQL += Datos.P_Anio_Orden + "', '";
                    Mi_SQL += Datos.P_Predio_Colindante + "', '";
                    Mi_SQL += Datos.P_Base_Impuesto + "', '";
                    Mi_SQL += Datos.P_Minimo_Elevado_Anio + "', '";
                    Mi_SQL += Datos.P_Base_Impuesto_Division + "', '";
                    Mi_SQL += Datos.P_Tasas_ID + "', ";
                    if (!String.IsNullOrEmpty(Datos.P_Multa_ID))
                    {
                        Mi_SQL += "'" + Datos.P_Multa_ID + "', '";
                    }
                    else
                    {
                        Mi_SQL += "null, '";
                    }
                    Mi_SQL += Fecha_Escritura.ToString("dd/M/yy") + "', '";
                    Mi_SQL += Datos.P_Impuesto_Div_Lot + "', '";
                    Mi_SQL += Datos.P_Costo_Constancia + "', '";
                    Mi_SQL += Datos.P_Monto_Traslado + "', '";
                    Mi_SQL += Datos.P_Monto_Division + "', '";
                    Mi_SQL += Datos.P_Monto_Multa + "', '";
                    Mi_SQL += Datos.P_Monto_Recargos + "', '";
                    Mi_SQL += Datos.P_Monto_Total_Pagar + "', '";
                    Mi_SQL += Datos.P_Tipo + "', '";
                    Mi_SQL += Datos.P_Nombre_Usuario + "', '";
                    Mi_SQL += Datos.P_Estatus + "', '";
                    Mi_SQL += Datos.P_Nombre_Usuario + "', SYSDATE)";

                    Comando.CommandText = Mi_SQL;
                    Filas_Afectadas += Comando.ExecuteNonQuery();

                    // si hay observaciones, agregar detalle de la tabla 
                    if (!String.IsNullOrEmpty(Datos.P_Observaciones))
                    {
                        Filas_Afectadas += Alta_Detalle_Calculo(Datos, Comando);
                    }
                }
                // aplicar cambios en base de datos
                Transaccion.Commit();
                Conexion.Close();

            }
            catch (OracleException Ex)
            {
                Transaccion.Rollback();
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Cálculo. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
            }
            finally
            {
            }
                //regresar el número de inserciones realizadas
                return Filas_Afectadas;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Alta_Detalle_Calculo
        /// DESCRIPCIÓN: Insercion de observaciones de un calculo de impuesto de traslado
        /// PARÁMETROS:
        /// 		1. Datos: Instancia de la clase de negocio con informacion a insertar
        /// 		2. Cmd: Conexion a la base de datos
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 12-ago-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static Int32 Alta_Detalle_Calculo(Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Datos, OracleCommand Cmd)
        {
            String Mi_SQL;
            Object No_Detalle;
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;
            Int32 Filas_Afectadas = 0;

            Mi_SQL = "SELECT NVL(MAX(" + Ope_Pre_Calc_Imp_Tras_Det.Campo_No_Detalle_Calculo + "),'0000000000') ";
            Mi_SQL += "FROM " + Ope_Pre_Calc_Imp_Tras_Det.Tabla_Ope_Pre_Ope_Pre_Calc_Imp_Tras_Det;
            No_Detalle = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            if (Convert.IsDBNull(No_Detalle) || No_Detalle == null)
            {
                 No_Detalle = "0000000001";
            }
            else
            {
                No_Detalle = String.Format("{0:0000000000}", Convert.ToInt32(No_Detalle) + 1);
            }

            // si llego un Comando como parametro, utilizarlo
            if (Cmd != null)    // si la conexion llego como parametro, establecer como comando para utilizar
            {
                Comando = Cmd;
            }
            else    // si no, crear nueva conexion y transaccion
            {
                Conexion.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Conexion.Open();
                Transaccion = Conexion.BeginTransaction();
                Comando.Connection = Conexion;
                Comando.Transaction = Transaccion;
            }

            Mi_SQL = "INSERT INTO " + Ope_Pre_Calc_Imp_Tras_Det.Tabla_Ope_Pre_Ope_Pre_Calc_Imp_Tras_Det + " (";
            Mi_SQL += Ope_Pre_Calc_Imp_Tras_Det.Campo_No_Detalle_Calculo + ", ";
            Mi_SQL += Ope_Pre_Calc_Imp_Tras_Det.Campo_No_Calculo + ", ";
            Mi_SQL += Ope_Pre_Calc_Imp_Tras_Det.Campo_Anio_Calculo + ", ";
            Mi_SQL += Ope_Pre_Calc_Imp_Tras_Det.Campo_Realizo_Observacion + ", ";
            Mi_SQL += Ope_Pre_Calc_Imp_Tras_Det.Campo_Observaciones + ", ";
            Mi_SQL += Ope_Pre_Calc_Imp_Tras_Det.Campo_Fecha_Hora + ", ";
            Mi_SQL += Ope_Pre_Calc_Imp_Tras_Det.Campo_Usuario_Creo + ", ";
            Mi_SQL += Ope_Pre_Calc_Imp_Tras_Det.Campo_Fecha_Creo + ") ";
            Mi_SQL += " VALUES ('";
            Mi_SQL += No_Detalle + "', '";
            Mi_SQL += Datos.P_No_Calculo + "', ";
            Mi_SQL += Datos.P_Anio_Calculo + ", '";
            Mi_SQL += Datos.P_Nombre_Usuario + "', '";
            Mi_SQL += Datos.P_Observaciones + "', ";
            Mi_SQL += "SYSTIMESTAMP, '";
            Mi_SQL += Datos.P_Nombre_Usuario + "', SYSDATE)";
            try
            {
                Comando.CommandText = Mi_SQL;
                Filas_Afectadas = Comando.ExecuteNonQuery();

                if (Cmd == null)    // si la conexion no llego como parametro, aplicar consultas
                {
                    Transaccion.Commit();
                }

                return Filas_Afectadas;
            }
            catch (OracleException Ex)
            {
                if (Cmd == null)
                {
                    Transaccion.Rollback();
                }
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
                if (Cmd == null)
                {
                    Conexion.Close();
                }
            }
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Alta_Pasivo
        /// DESCRIPCIÓN: Insercion de adeudo en tabla Ope_Ing_Pasivo
        /// PARÁMETROS:
        /// 	1. Datos: Instancia de la clase de negocio con los datos que serán insertados en la base de datos
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 24-ago-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static int Alta_Pasivo(Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Datos)
        {
            String Mi_SQL;  //Obtiene la cadena de inserción hacía la base de datos
            Int32 Filas_Afectadas = 0;
            String Mensaje = "";

            // variables para transaccion
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;

            try
            {
                // abrir conexion con la base de datos si no llego una conexion como parametro 
                if (Datos.P_Cmd_Calculo != null)
                {
                    Comando = Datos.P_Cmd_Calculo;
                }
                else
                {
                    Conexion.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                    Conexion.Open();
                    Transaccion = Conexion.BeginTransaction();
                    Comando.Connection = Conexion;
                    Comando.Transaction = Transaccion;
                }

                //Consulta para la inserción del Adeudo como pasivo
                Mi_SQL = "INSERT INTO " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo
                    + "(" + Ope_Ing_Pasivo.Campo_No_Pasivo
                    + ", " + Ope_Ing_Pasivo.Campo_Referencia
                    + ", " + Ope_Ing_Pasivo.Campo_Clave_Ingreso_ID
                    + ", " + Ope_Ing_Pasivo.Campo_Cuenta_Predial_ID
                    + ", " + Ope_Ing_Pasivo.Campo_Descripcion
                    + ", " + Ope_Ing_Pasivo.Campo_Fecha_Ingreso
                    + ", " + Ope_Ing_Pasivo.Campo_Fecha_Vencimiento
                    + ", " + Ope_Ing_Pasivo.Campo_Monto
                    + ", " + Ope_Ing_Pasivo.Campo_Recargos
                    + ", " + Ope_Ing_Pasivo.Campo_Estatus
                    + ", " + Ope_Ing_Pasivo.Campo_SubConcepto_Ing_ID
                    + ", " + Ope_Ing_Pasivo.Campo_Concepto_Ing_ID
                    + ", " + Ope_Ing_Pasivo.Campo_Dependencia_ID
                    + ", " + Ope_Ing_Pasivo.Campo_Contribuyente
                    + ", " + Ope_Ing_Pasivo.Campo_Origen
                    + ", " + Ope_Ing_Pasivo.Campo_Fecha_Creo
                    + ", " + Ope_Ing_Pasivo.Campo_Usuario_Creo
                    + ") VALUES(" 
                    + Obtener_Consecutivo_Pasivos(Ope_Ing_Pasivo.Campo_No_Pasivo, Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo, Datos.P_Cmd_Calculo)
                    + ",'" + Datos.P_Referencia + "'"
                    + ",'" + Datos.P_Clave_Ingreso_ID + "'"
                    + ",'" + Datos.P_Cuenta_Predial_ID + "'"
                    + ",'" + Datos.P_Descripcion + "'"
                    + ",'" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Tramite.Trim())) + "'"
                    + ",'" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Vencimiento_Pasivo.Trim())) + "'"
                    + ",'" + Datos.P_Monto_Total_Pagar.Trim() + "',0"
                    + ",'" + Datos.P_Estatus + "'"
                    + ",'" + Datos.P_SubConcepto_ID + "'"
                    + ",'" + Datos.P_Concepto_ID + "'"
                    + ",'" + Datos.P_Dependencia_ID + "'"
                    + ",'" + Datos.P_Contribuyente 
                    + "','" + Datos.P_Origen
                    + "', SYSDATE, '" 
                    + Datos.P_Nombre_Usuario + "')";

                Comando.CommandText = Mi_SQL;
                Filas_Afectadas += Comando.ExecuteNonQuery();

                // aplicar cambios en base de datos
                if (Datos.P_Cmd_Calculo == null)
                {
                    Transaccion.Commit();
                }

            }
            catch (OracleException Ex)
            {
                if (Datos.P_Cmd_Calculo == null)
                {
                    Transaccion.Rollback();
                }
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
                    Mensaje = "Error al intentar actualizar un Registro de Cálculo. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Datos.P_Cmd_Calculo == null)
                {
                    Conexion.Close();
                }
            }
            //regresar el número de inserciones realizadas
            return Filas_Afectadas;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Eliminar_Referencias_Pasivo
        /// DESCRIPCIÓN: Eliminar pasivos por referencia con estatus POR PAGAR
        /// PARÁMETROS:
        /// 	1. Datos: Instancia de la clase de negocio con los datos que serán insertados en la base de datos
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 24-ago-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static int Eliminar_Referencias_Pasivo(Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Datos)
        {
            String Mi_SQL;  //Obtiene la cadena de inserción hacía la base de datos
            Int32 Filas_Afectadas = 0;
            String Mensaje = "";

            // variables para transaccion
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;

            try
            {
                if (!String.IsNullOrEmpty(Datos.P_Referencia))
                {
                    // abrir conexion con la base de datos si no llego una conexion como parametro 
                    if (Datos.P_Cmd_Calculo != null)
                    {
                        Comando = Datos.P_Cmd_Calculo;
                    }
                    else
                    {
                        Conexion.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                        Conexion.Open();
                        Transaccion = Conexion.BeginTransaction();
                        Comando.Connection = Conexion;
                        Comando.Transaction = Transaccion;
                    }

                    //Consulta para la inserción del Adeudo como pasivo
                    Mi_SQL = "DELETE FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo
                        + " WHERE " + Ope_Ing_Pasivo.Campo_Referencia
                        + " = '" + Datos.P_Referencia
                        + "' AND " + Ope_Ing_Pasivo.Campo_Estatus
                        + " = 'POR PAGAR'";

                    // filtros dinamicos
                    if (!String.IsNullOrEmpty(Datos.P_Filtro_Dinamico))
                    {
                        Mi_SQL += Datos.P_Filtro_Dinamico;
                    }

                    Comando.CommandText = Mi_SQL;
                    Filas_Afectadas += Comando.ExecuteNonQuery();

                    // aplicar cambios en base de datos
                    if (Datos.P_Cmd_Calculo == null)
                    {
                        Transaccion.Commit();
                    }
                }
            }
            catch (OracleException Ex)
            {
                if (Datos.P_Cmd_Calculo == null)
                {
                    Transaccion.Rollback();
                }
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
                    Mensaje = "Error al intentar actualizar un Registro de Cálculo. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Datos.P_Cmd_Calculo == null)
                {
                    Conexion.Close();
                }
            }
            //regresar el número de inserciones realizadas
            return Filas_Afectadas;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Eliminar_Referencias_Pasivo
        /// DESCRIPCIÓN: Eliminar pasivos por referencia con estatus POR PAGAR
        /// PARÁMETROS:
        /// 	1. Datos: Instancia de la clase de negocio con los datos que serán insertados en la base de datos
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 24-ago-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static int Cancelar_Descuentos_Caducados(Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Datos)
        {
            String Mi_SQL;  //Obtiene la cadena de inserción hacía la base de datos
            Int32 Filas_Afectadas = 0;
            String Mensaje = "";

            // variables para transaccion
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;

            try
            {
                Conexion.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Conexion.Open();
                Transaccion = Conexion.BeginTransaction();
                Comando.Connection = Conexion;
                Comando.Transaction = Transaccion;

                //Consulta para la inserción del Adeudo como pasivo
                Mi_SQL = "UPDATE " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo;
                Mi_SQL += " SET " + Ope_Ing_Pasivo.Campo_Estatus + "='CANCELADO'";
                Mi_SQL += " WHERE " + Ope_Ing_Pasivo.Campo_Referencia;
                Mi_SQL += " = '" + Datos.P_Referencia;
                Mi_SQL += "' AND " + Ope_Ing_Pasivo.Campo_Estatus;
                Mi_SQL += " = 'POR PAGAR' AND "+Ope_Ing_Pasivo.Campo_Descripcion+" LIKE '%DESCUENTO%'";


                Comando.CommandText = Mi_SQL;
                Filas_Afectadas += Comando.ExecuteNonQuery();

                // aplicar cambios en base de datos

                Transaccion.Commit();
            }
            catch (OracleException Ex)
            {

                Transaccion.Rollback();
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
                    Mensaje = "Error al intentar actualizar un Registro de Pasivos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {

                Conexion.Close();
            }
            //regresar el número de inserciones realizadas
            return Filas_Afectadas;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Modificar_Descuentos_Caducados
        /// DESCRIPCIÓN: Eliminar los pasivos de descuentos vencidos
        /// PARÁMETROS:
        /// CREO: Miguel Angel Bedolla Moreno
        /// FECHA_CREO: 22-Marzo-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static int Modificar_Descuentos_Caducados()
        {
            String Mi_SQL;  //Obtiene la cadena de inserción hacía la base de datos
            Int32 Filas_Afectadas = 0;
            String Mensaje = "";

            // variables para transaccion
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;

            try
            {
                Conexion.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Conexion.Open();
                Transaccion = Conexion.BeginTransaction();
                Comando.Connection = Conexion;
                Comando.Transaction = Transaccion;

                //Consulta para la inserción del Adeudo como pasivo
                Mi_SQL = "UPDATE " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo;
                Mi_SQL += " SET " + Ope_Ing_Pasivo.Campo_Estatus + "='CANCELADO'";
                Mi_SQL += " WHERE " + Ope_Ing_Pasivo.Campo_Estatus;
                Mi_SQL += " = 'POR PAGAR' AND " + Ope_Ing_Pasivo.Campo_Descripcion + " LIKE '%DESCUENTO%' AND ";
                Mi_SQL += Ope_Ing_Pasivo.Campo_Fecha_Vencimiento + "<TO_DATE(SYSDATE)";


                Comando.CommandText = Mi_SQL;
                Filas_Afectadas += Comando.ExecuteNonQuery();

                Mi_SQL = "UPDATE " + Ope_Pre_Descuento_Der_Sup.Tabla_Ope_Pre_Descuento_Der_Sup;
                Mi_SQL += " SET " + Ope_Pre_Descuento_Der_Sup.Campo_Estatus + "='CANCELADO'";
                Mi_SQL += " WHERE " + Ope_Pre_Descuento_Der_Sup.Campo_Estatus;
                Mi_SQL += " IN ('VIGENTE','BAJA') AND " + Ope_Pre_Descuento_Der_Sup.Campo_Fecha_Vencimiento + "<TO_DATE(SYSDATE)";

                Comando.CommandText = Mi_SQL;
                Filas_Afectadas += Comando.ExecuteNonQuery();

                Mi_SQL = "UPDATE " + Ope_Pre_Descuento_Fracc.Tabla_Ope_Pre_Descuento_Fracc;
                Mi_SQL += " SET " + Ope_Pre_Descuento_Fracc.Campo_Estatus + "='CANCELADO'";
                Mi_SQL += " WHERE " + Ope_Pre_Descuento_Fracc.Campo_Estatus;
                Mi_SQL += " IN ('VIGENTE','BAJA') AND " + Ope_Pre_Descuento_Fracc.Campo_Fecha_Vencimiento + "<TO_DATE(SYSDATE)";

                Comando.CommandText = Mi_SQL;
                Filas_Afectadas += Comando.ExecuteNonQuery();

                Mi_SQL = "UPDATE " + Ope_Pre_Descuento_Traslado.Tabla_Ope_Pre_Descuento_Traslado;
                Mi_SQL += " SET " + Ope_Pre_Descuento_Traslado.Campo_Estatus + "='CANCELADO'";
                Mi_SQL += " WHERE " + Ope_Pre_Descuento_Traslado.Campo_Estatus;
                Mi_SQL += " IN ('VIGENTE','BAJA') AND " + Ope_Pre_Descuento_Traslado.Campo_Fecha_Vencimiento + "<TO_DATE(SYSDATE)";

                Comando.CommandText = Mi_SQL;
                Filas_Afectadas += Comando.ExecuteNonQuery();

                Mi_SQL = "UPDATE " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial;
                Mi_SQL += " SET " + Ope_Pre_Descuentos_Predial.Campo_Estatus + "='CANCELADO'";
                Mi_SQL += " WHERE " + Ope_Pre_Descuentos_Predial.Campo_Estatus;
                Mi_SQL += " IN ('VIGENTE','PENDIENTE') AND " + Ope_Pre_Descuentos_Predial.Campo_Fecha_Vencimiento + "<TO_DATE(SYSDATE)";

                Comando.CommandText = Mi_SQL;
                Filas_Afectadas += Comando.ExecuteNonQuery();

                // aplicar cambios en base de datos

                Transaccion.Commit();
            }
            catch (OracleException Ex)
            {

                Transaccion.Rollback();
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
                    Mensaje = "Error al intentar actualizar un Registro de Pasivos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {

                Conexion.Close();
            }
            //regresar el número de inserciones realizadas
            return Filas_Afectadas;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consulta_Calculos
        /// DESCRIPCIÓN: Consulta los campos No_Calculo, Anio_Calculo, Fecha_Creo y Estatus de los calculos de traslado
        /// PARÁMETROS:
        /// 	1. Datos: Indica qué registro se desea consultar a la base de datos mediante propiedades 
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 11-ago-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Calculos(Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL
            String Filtro_SQL = ""; //Variable para el filtro de la consulta SQL

            try
            {
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Calculo_Imp_Traslado.Campo_Fecha_Creo + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus;
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado;

                if (Datos.P_No_Calculo != null)   // Si se recibió un ID de documento filtrar por ese ID
                {
                    Filtro_SQL = " WHERE " + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo + " = '" + Datos.P_No_Calculo + "'";
                }
                if (Datos.P_Estatus != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar OR y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL += " AND " + Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + " = '" + Datos.P_Estatus + "'";
                    else
                        Filtro_SQL = " WHERE " + Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + " = '" + Datos.P_Estatus + "'";
                }
                if (Datos.P_Cuenta_Predial != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL += " AND " + Ope_Pre_Calculo_Imp_Traslado.Campo_Cuenta_Predial_Id + " LIKE '" + Datos.P_Cuenta_Predial + "'";
                    else
                        Filtro_SQL = " WHERE " + Ope_Pre_Calculo_Imp_Traslado.Campo_Cuenta_Predial_Id + " LIKE '" + Datos.P_Cuenta_Predial + "'";
                }
                if (Datos.P_Anio_Calculo > 0)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL += " AND " + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + " = " + Datos.P_Anio_Calculo;
                    else
                        Filtro_SQL = " WHERE " + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + " = " + Datos.P_Anio_Calculo;
                }
                if (Datos.P_No_Orden_Variacion != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL += " AND " + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion + " = " + Datos.P_No_Orden_Variacion;
                    else
                        Filtro_SQL = " WHERE " + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion + " = " + Datos.P_No_Orden_Variacion;
                }
                if (Datos.P_Anio_Orden > 0)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL += " AND " + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + " = " + Datos.P_Anio_Orden;
                    else
                        Filtro_SQL = " WHERE " + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + " = " + Datos.P_Anio_Orden;
                }
                Filtro_SQL += " ORDER BY " + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo;
                Mi_SQL = Mi_SQL + Filtro_SQL; ;

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
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
        }   //FUNCIÓN: Consulta_Calculos

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consulta_Folio_Orden_Contrarecibo
        /// DESCRIPCIÓN: Consulta la orden de variacion para traer el folio de orden y contrarecibo
        /// PARÁMETROS:
        /// 	1. Datos: Instancia de la clase de negocio con datos a considerar
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 20-ago-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Folio_Orden_Contrarecibo(String No_Orden, Int32 Anio)
        {
            String Mi_SQL; //Variable para la consulta SQL
            String Filtro_SQL = ""; //Variable para el filtro de la consulta SQL

            try
            {
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + ", TO_NUMBER(";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + ") CONTRARECIBO, ( SELECT ";
                Mi_SQL = Mi_SQL + Cat_Pre_Movimientos.Campo_Identificador + " FROM ";
                Mi_SQL = Mi_SQL + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " WHERE ";
                Mi_SQL = Mi_SQL + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = ";
                Mi_SQL = Mi_SQL + "ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ") || TO_NUMBER(" +
                    Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + ") || '/' || " + 
                    Ope_Pre_Ordenes_Variacion.Campo_Anio + " FOLIO_ORDEN, ";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden;
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " ORDEN";
                
                Filtro_SQL = " WHERE " + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " = " + No_Orden;
                Filtro_SQL += " AND " + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = " + Anio;

                Mi_SQL = Mi_SQL + Filtro_SQL; ;

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
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
        }   //FUNCIÓN: Consulta_Folio_Orden_Contrarecibo

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Ordenes_Variacion
        /// DESCRIPCIÓN: Formar y ejecutar una consulta de las ordenes de variacion
        /// PARÁMETROS:
        /// 		1. Datos: Instancia de la clases de negocio con parametros para filtrar consulta
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 03-feb-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Ordenes_Variacion(Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Datos)
        {
            DataTable Dt_Ordenes_Variacion = new DataTable();
            String Mi_SQL = "";
            try
            {
                // formar subconsulta para obtener el propietario id de la tabla copropietarios de la OV o del catalogo de propietarios
                if (Datos.P_Incluir_Campos_Foraneos)
                {
                    Mi_SQL+= "WITH CONTRIBUYENTE AS (SELECT  NVL((SELECT "
                        + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID
                        + " FROM " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion
                        + " WHERE " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_No_Orden_Variacion
                        + " = OV." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion
                        + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Anio 
                        + " = OV." + Ope_Pre_Ordenes_Variacion.Campo_Anio
                        + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Estatus + " = 'ALTA' "
                        + " AND ( " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + "."
                        + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo + " ='PROPIETARIO' OR " 
                        + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + "."
                        + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo + " ='POSEEDOR') AND ROWNUM=1)" 
                        + ", (SELECT " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID 
                        + " FROM " +  Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios 
                        + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID
                        + " = OV." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID
                        + " AND ( " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "."
                        + Cat_Pre_Propietarios.Campo_Tipo + " ='PROPIETARIO' OR " 
                        + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "."
                        + Cat_Pre_Propietarios.Campo_Tipo + " ='POSEEDOR') AND ROWNUM=1)) "
                        + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID
                        + " FROM " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " OV"
                        + " WHERE ";

                    if (!string.IsNullOrEmpty(Datos.P_No_Orden_Variacion))
                    {
                        Mi_SQL += "OV." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion +
                                  Validar_Operador_Comparacion(Datos.P_No_Orden_Variacion) + " AND ";
                    }
                    if (Datos.P_Anio_Orden > 0)
                    {
                        Mi_SQL += "OV." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = " + Datos.P_Anio_Orden;
                    }
                    // eliminar AND o WHERE al final de la consulta
                    if (Mi_SQL.EndsWith(" AND "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                    }
                    if (Mi_SQL.EndsWith(" WHERE "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                    }
                    Mi_SQL += ")";
                }
                // formar consulta principal
                Mi_SQL += "SELECT " + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion
                          + ", " + Ope_Pre_Ordenes_Variacion.Campo_Anio;
                if (Datos.P_Incluir_Campos_Foraneos)
                {
                    Mi_SQL += ", (SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial
                              + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas
                              + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = "
                              + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                              + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + ") AS Cuenta_Predial ";
                    // movimiento
                    Mi_SQL += ", (SELECT " + Cat_Pre_Movimientos.Campo_Identificador
                              + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos
                              + " WHERE " + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = "
                              + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                              + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ") AS Identificador_Movimiento ";
                    Mi_SQL += ", (SELECT " + Cat_Pre_Movimientos.Campo_Descripcion
                              + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos
                              + " WHERE " + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = "
                              + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                              + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ") AS Descripcion_Movimiento ";
                    // propietario
                    Mi_SQL += ", (SELECT " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || "
                              + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || "
                              + Cat_Pre_Contribuyentes.Campo_Nombre
                              + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes
                              + " WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = "
                              + "CONTRIBUYENTE." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID
                              + ") AS NOMBRE_PROPIETARIO ";
                    Mi_SQL += ", (SELECT " + Cat_Pre_Contribuyentes.Campo_RFC
                              + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes
                              + " WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = "
                              + "CONTRIBUYENTE." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID
                              + ") AS RFC_PROPIETARIO ";
                    // ubicacion y notificacion
                    Mi_SQL += ", (SELECT " + Cat_Pre_Calles.Campo_Nombre
                              + " FROM " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles
                              + " WHERE " + Cat_Pre_Calles.Campo_Calle_ID + " = "
                              + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                              + Ope_Pre_Ordenes_Variacion.Campo_Calle_ID + ") AS NOMBRE_CALLE_UBICACION ";
                    Mi_SQL += ", (SELECT " + Cat_Ate_Colonias.Campo_Nombre
                              + " FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias
                              + " WHERE " + Cat_Ate_Colonias.Campo_Colonia_ID + " = "
                              + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                              + Ope_Pre_Ordenes_Variacion.Campo_Colonia_ID + ") AS NOMBRE_COLONIA_UBICACION ";
                    // calle y colonia notificacion
                    Mi_SQL += ", CASE WHEN NVL(" + Ope_Pre_Ordenes_Variacion.Campo_Domicilio_Foraneo + ", 'NO') ='SI' THEN "
                              + Ope_Pre_Ordenes_Variacion.Campo_Calle_Notificacion 
                              + " ELSE (SELECT " 
                              + Cat_Pre_Calles.Campo_Nombre
                              + " FROM " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles
                              + " WHERE " + Cat_Pre_Calles.Campo_Calle_ID + " = "
                              + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                              + Ope_Pre_Ordenes_Variacion.Campo_Calle_ID_Notificacion + ") END AS "
                              + Ope_Pre_Ordenes_Variacion.Campo_Calle_Notificacion;   // CALLE_NOTIFICACION
                    Mi_SQL += ", CASE WHEN NVL(domicilio_foraneo, 'NO') = 'SI' THEN " 
                              + Ope_Pre_Ordenes_Variacion.Campo_Colonia_Notificacion
                              + " ELSE (SELECT " + Cat_Ate_Colonias.Campo_Nombre
                              + " FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias
                              + " WHERE " + Cat_Ate_Colonias.Campo_Colonia_ID + " = "
                              + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                              + Ope_Pre_Ordenes_Variacion.Campo_Colonia_ID_Notificacion + ") END AS "
                              + Ope_Pre_Ordenes_Variacion.Campo_Colonia_Notificacion;   // COLONIA_NOTIFICACION
                    Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Ciudad_Notificacion
                              + ", " + Ope_Pre_Ordenes_Variacion.Campo_Estado_Notificacion
                              + ", " + Ope_Pre_Ordenes_Variacion.Campo_No_Exterior
                              + ", " + Ope_Pre_Ordenes_Variacion.Campo_No_Interior
                              + ", " + Ope_Pre_Ordenes_Variacion.Campo_No_Exterior_Notificacion
                              + ", " + Ope_Pre_Ordenes_Variacion.Campo_No_Interior_Notificacion;
                    // tipo predio
                    Mi_SQL += ", (SELECT " + Cat_Pre_Tipos_Predio.Campo_Descripcion
                              + " FROM " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio
                              + " WHERE " + Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + " = "
                              + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                              + Ope_Pre_Ordenes_Variacion.Campo_Tipo_Predio_ID + ") AS TIPO_PREDIO ";
                    // uso suelo
                    Mi_SQL += ", (SELECT " + Cat_Pre_Uso_Suelo.Campo_Descripcion
                              + " FROM " + Cat_Pre_Uso_Suelo.Tabla_Cat_Pre_Uso_Suelo
                              + " WHERE " + Cat_Pre_Uso_Suelo.Campo_Uso_Suelo_ID + " = "
                              + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                              + Ope_Pre_Ordenes_Variacion.Campo_Uso_Suelo_ID + ") AS USO_SUELO ";
                    // tasa
                    Mi_SQL += ", (SELECT " + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Anual
                              + " FROM " + Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual
                              + " WHERE " + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_ID + " = "
                              + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                              + Ope_Pre_Ordenes_Variacion.Campo_Tasa_ID + ") AS " 
                              + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Anual;
                }
                if (!string.IsNullOrEmpty(Datos.P_Campos_Dinamicos))
                {
                    Mi_SQL += Datos.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID
                    + ", " + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID
                    + ", " + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo
                    + ", " + Ope_Pre_Ordenes_Variacion.Campo_No_Nota
                    + ", " + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Nota
                    + ", " + Ope_Pre_Ordenes_Variacion.Campo_Grupo_Movimiento_ID
                    + ", " + Ope_Pre_Ordenes_Variacion.Campo_Tipo_Predio_ID
                    + ", " + Ope_Pre_Ordenes_Variacion.Campo_Numero_Nota_Impreso
                    + ", " + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden
                    + ", " + Ope_Pre_Ordenes_Variacion.Campo_Observaciones
                    + ", " + Ope_Pre_Ordenes_Variacion.Campo_Usuario_Creo
                    + ", " + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + ", TO_CHAR(";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + ", 'DD/Mon/YYYY HH:MI:SS PM') AS FECHA_ORDEN";
                }

                // otros datos de la cuenta
                if (Datos.P_Incluir_Generales_Cuenta)
                {
                    Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Valor_Fiscal
                              + ", " + Ope_Pre_Ordenes_Variacion.Campo_Efectos
                              + ", " + Ope_Pre_Ordenes_Variacion.Campo_Domicilio_Foraneo
                              + ", " + Ope_Pre_Ordenes_Variacion.Campo_Codigo_Postal
                              + ", " + Ope_Pre_Ordenes_Variacion.Campo_Cuota_Anual
                              + ", " + Ope_Pre_Ordenes_Variacion.Campo_Tasa_ID
                              + ", " + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Cuenta;

                }
                if (Datos.P_Incluir_Campos_Foraneos)
                {
                    Mi_SQL += ", CONTRIBUYENTE." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID;
                }
                // FROM
                Mi_SQL += " FROM " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion;

                if (Datos.P_Incluir_Campos_Foraneos)
                {
                    Mi_SQL += ", CONTRIBUYENTE";
                }
                // filtros de la consulta
                if (!string.IsNullOrEmpty(Datos.P_Filtro_Dinamico))
                {
                    Mi_SQL += " WHERE " + Datos.P_Filtro_Dinamico;
                }
                else
                {
                    Mi_SQL += " WHERE ";
                    if (!string.IsNullOrEmpty(Datos.P_No_Orden_Variacion))
                    {
                        Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion +
                                  Validar_Operador_Comparacion(Datos.P_No_Orden_Variacion) + " AND ";
                    }
                    if (Datos.P_Anio_Orden > 0)
                    {
                        Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Anio + " = " + Datos.P_Anio_Orden + " AND ";
                    }
                    if (!string.IsNullOrEmpty(Datos.P_Estatus_Orden))
                    {
                        Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " = '" + Datos.P_Estatus_Orden +
                                  "' AND ";
                    }
                    if (Datos.P_Fecha_Creo != DateTime.MinValue)
                    {
                        Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo +
                                  Validar_Operador_Comparacion(Datos.P_Fecha_Creo.ToString("dd-MM-yyyy")) + " AND ";
                    }

                    // eliminar AND o WHERE al final de la consulta
                    if (Mi_SQL.EndsWith(" AND "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                    }
                    if (Mi_SQL.EndsWith(" WHERE "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                    }
                }
                // agregar ordenamiento dinamico o por defecto
                if (!string.IsNullOrEmpty(Datos.P_Ordenar_Dinamico))
                {
                    Mi_SQL += " ORDER BY " + Datos.P_Ordenar_Dinamico;
                }
                else
                {
                    Mi_SQL += " ORDER BY " + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " DESC";
                }

                DataSet Ds_Ordenes_Variacion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text,
                                                                           Mi_SQL);
                if (Ds_Ordenes_Variacion != null)
                {
                    Dt_Ordenes_Variacion = Ds_Ordenes_Variacion.Tables[0];

                    if (Dt_Ordenes_Variacion.Rows.Count > 0 && Datos.P_Incluir_Observaciones)
                    {
                        //Consulta las Observaciones de la Orden
                        Mi_SQL = "SELECT ";
                        Mi_SQL += Ope_Pre_Observaciones.Campo_Observaciones_ID + ", ";
                        Mi_SQL += Ope_Pre_Observaciones.Campo_No_Orden_Variacion + ", ";
                        Mi_SQL += Ope_Pre_Observaciones.Campo_Usuario_Creo + ", ";
                        Mi_SQL += Ope_Pre_Observaciones.Campo_Año + ", ";
                        Mi_SQL += Ope_Pre_Observaciones.Campo_Descripcion;
                        Mi_SQL += " FROM ";
                        Mi_SQL += Ope_Pre_Observaciones.Tabla_Ope_Pre_Observaciones_Orden_Variacion;
                        Mi_SQL += " WHERE ";
                        if (Datos.P_Anio_Orden > 0)
                        {
                            Mi_SQL += Ope_Pre_Observaciones.Campo_Año + " = " + Datos.P_Anio_Orden + " AND ";
                        }
                        // filtrar por el numero de orden en la primera fila de la tabla de ordenes
                        Mi_SQL += Ope_Pre_Observaciones.Campo_No_Orden_Variacion + " = '" +
                                  Ds_Ordenes_Variacion.Tables[0].Rows[0][
                                      Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion] + "' AND ";

                        if (Mi_SQL.EndsWith(" AND "))
                        {
                            Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                        }
                        if (Mi_SQL.EndsWith(" WHERE "))
                        {
                            Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                        }
                        Mi_SQL += " ORDER BY " + Ope_Pre_Observaciones.Campo_Observaciones_ID + " DESC";
                        Datos.P_Dt_Observaciones =
                            OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                    }
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Ordenes_Variacion;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Datos_Cuenta_Predial
        /// DESCRIPCIÓN: Formar y ejecutar una consulta de los datos de cuentas predial
        /// PARÁMETROS:
        /// 		1. Datos: Instancia de la clases de negocio con parametros para filtrar consulta
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 29-feb-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Datos_Cuenta_Predial(Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Datos)
        {
            DataTable Dt_Cuenta = new DataTable();
            String Mi_SQL = "";
            try
            {
                // formar subconsulta para obtener el propietario id de la tabla copropietarios de la OV o del catalogo de propietarios
                if (Datos.P_Incluir_Campos_Foraneos)
                {
                    Mi_SQL += "WITH CONTRIBUYENTE AS (SELECT CP."
                        + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID
                        + ", (SELECT " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID
                        + " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios
                        + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID
                        + " = CP." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID
                        + " AND ( " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "."
                        + Cat_Pre_Propietarios.Campo_Tipo + " ='PROPIETARIO' OR "
                        + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "."
                        + Cat_Pre_Propietarios.Campo_Tipo + " ='POSEEDOR') AND ROWNUM=1) "
                        + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID
                        + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CP"
                        + " WHERE ";

                    // filtrar por cuenta predial ID
                    if (!string.IsNullOrEmpty(Datos.P_Cuenta_Predial_ID))
                    {
                        Mi_SQL += "CP." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "='"
                            + Datos.P_Cuenta_Predial_ID + "' AND ";
                    }
                    // filtrar por cuenta predial
                    if (!string.IsNullOrEmpty(Datos.P_Cuenta_Predial))
                    {
                        Mi_SQL += "CP." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " LIKE '%"
                            + Datos.P_Cuenta_Predial + "%' AND ";
                    }
                    // filtrar por estatus
                    if (!string.IsNullOrEmpty(Datos.P_Estatus))
                    {
                        Mi_SQL += "CP." + Cat_Pre_Cuentas_Predial.Campo_Estatus + " = '"
                            + Datos.P_Estatus + "' AND ";
                    }
                    // filtrar por estatus
                    if (!string.IsNullOrEmpty(Datos.P_Filtro_Estatus))
                    {
                        Mi_SQL += "CP." + Cat_Pre_Cuentas_Predial.Campo_Estatus
                            + Datos.P_Filtro_Estatus + " AND ";
                    }
                    // eliminar AND o WHERE al final de la consulta
                    if (Mi_SQL.EndsWith(" AND "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                    }
                    if (Mi_SQL.EndsWith(" WHERE "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                    }
                    Mi_SQL += ")";
                }
                // formar consulta principal
                Mi_SQL += "SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial;

                if (Datos.P_Incluir_Campos_Foraneos)
                {
                    // propietario
                    Mi_SQL += ", (SELECT " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || "
                              + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || "
                              + Cat_Pre_Contribuyentes.Campo_Nombre
                              + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes
                              + " WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = "
                              + "CONTRIBUYENTE." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID
                              + ") AS NOMBRE_PROPIETARIO ";
                    Mi_SQL += ", (SELECT " + Cat_Pre_Contribuyentes.Campo_RFC
                              + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes
                              + " WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = "
                              + "CONTRIBUYENTE." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID
                              + ") AS RFC_PROPIETARIO ";
                    // ubicacion y notificacion
                    Mi_SQL += ", (SELECT " + Cat_Pre_Calles.Campo_Nombre
                              + " FROM " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles
                              + " WHERE " + Cat_Pre_Calles.Campo_Calle_ID + " = "
                              + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "."
                              + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + ") AS NOMBRE_CALLE_UBICACION ";
                    Mi_SQL += ", (SELECT " + Cat_Ate_Colonias.Campo_Nombre
                              + " FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias
                              + " WHERE " + Cat_Ate_Colonias.Campo_Colonia_ID + " = "
                              + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "."
                              + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + ") AS NOMBRE_COLONIA_UBICACION ";
                    // calle y colonia notificacion
                    Mi_SQL += ", CASE WHEN NVL(" + Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo + ", 'NO') ='SI' THEN "
                              + Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion
                              + " ELSE (SELECT "
                              + Cat_Pre_Calles.Campo_Nombre
                              + " FROM " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles
                              + " WHERE " + Cat_Pre_Calles.Campo_Calle_ID + " = "
                              + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "."
                              + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion + ") END AS "
                              + Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion;   // CALLE_NOTIFICACION
                    Mi_SQL += ", CASE WHEN NVL(domicilio_foraneo, 'NO') = 'SI' THEN "
                              + Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion
                              + " ELSE (SELECT " + Cat_Ate_Colonias.Campo_Nombre
                              + " FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias
                              + " WHERE " + Cat_Ate_Colonias.Campo_Colonia_ID + " = "
                              + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "."
                              + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion + ") END AS "
                              + Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion;   // COLONIA_NOTIFICACION
                    Mi_SQL += ", " + Cat_Pre_Cuentas_Predial.Campo_Ciudad_Notificacion
                              + ", " + Cat_Pre_Cuentas_Predial.Campo_Estado_Notificacion
                              + ", " + Cat_Pre_Cuentas_Predial.Campo_No_Exterior
                              + ", " + Cat_Pre_Cuentas_Predial.Campo_No_Interior
                              + ", " + Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion
                              + ", " + Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion;
                    // tipo predio
                    Mi_SQL += ", (SELECT " + Cat_Pre_Tipos_Predio.Campo_Descripcion
                              + " FROM " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio
                              + " WHERE " + Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + " = "
                              + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "."
                              + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + ") AS TIPO_PREDIO ";
                    // uso suelo
                    Mi_SQL += ", (SELECT " + Cat_Pre_Uso_Suelo.Campo_Descripcion
                              + " FROM " + Cat_Pre_Uso_Suelo.Tabla_Cat_Pre_Uso_Suelo
                              + " WHERE " + Cat_Pre_Uso_Suelo.Campo_Uso_Suelo_ID + " = "
                              + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "."
                              + Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID + ") AS USO_SUELO ";
                    // tasa
                    Mi_SQL += ", (SELECT " + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Anual
                              + " FROM " + Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual
                              + " WHERE " + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_ID + " = "
                              + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "."
                              + Cat_Pre_Cuentas_Predial.Campo_Tasa_ID + ") AS "
                              + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Anual;
                }
                if (!string.IsNullOrEmpty(Datos.P_Campos_Dinamicos))
                {
                    Mi_SQL += Datos.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL += ", " + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID
                    + ", " + Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal
                    + ", " + Cat_Pre_Cuentas_Predial.Campo_Efectos
                    + ", " + Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo
                    + ", " + Cat_Pre_Cuentas_Predial.Campo_Codigo_Postal
                    + ", " + Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual
                    + ", " + Cat_Pre_Cuentas_Predial.Campo_Tasa_ID
                    + ", " + Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo
                    + ", " + Cat_Pre_Cuentas_Predial.Campo_Estatus;
                }


                if (Datos.P_Incluir_Campos_Foraneos)
                {
                    Mi_SQL += ", CONTRIBUYENTE." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID;
                }
                // FROM
                Mi_SQL += " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas;

                if (Datos.P_Incluir_Campos_Foraneos)
                {
                    Mi_SQL += " left join CONTRIBUYENTE on "
                        + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "."
                        + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "= CONTRIBUYENTE."
                        + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                }
                // filtros de la consulta
                if (!string.IsNullOrEmpty(Datos.P_Filtro_Dinamico))
                {
                    Mi_SQL += " WHERE " + Datos.P_Filtro_Dinamico;
                }
                else
                {
                    Mi_SQL += " WHERE ";

                    // filtrar por cuenta predial ID
                    if (!string.IsNullOrEmpty(Datos.P_Cuenta_Predial_ID))
                    {
                        Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "."
                            + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "='"
                            + Datos.P_Cuenta_Predial_ID + "' AND ";
                    }
                    // filtrar por cuenta predial
                    if (!string.IsNullOrEmpty(Datos.P_Cuenta_Predial))
                    {
                        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " LIKE '%"
                            + Datos.P_Cuenta_Predial + "%' AND ";
                    }
                    // filtrar por estatus
                    if (!string.IsNullOrEmpty(Datos.P_Estatus))
                    {
                        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Estatus + " = '"
                            + Datos.P_Estatus + "' AND ";
                    }
                    // filtrar por estatus
                    if (!string.IsNullOrEmpty(Datos.P_Filtro_Estatus))
                    {
                        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Estatus
                            + Datos.P_Filtro_Estatus + " AND ";
                    }
                }

                // eliminar AND o WHERE al final de la consulta
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }

                // agregar ordenamiento dinamico o por defecto
                if (!string.IsNullOrEmpty(Datos.P_Ordenar_Dinamico))
                {
                    Mi_SQL += " ORDER BY " + Datos.P_Ordenar_Dinamico;
                }
                else
                {
                    Mi_SQL += " ORDER BY "
                        + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "."
                        + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                }

                DataSet Ds_Ordenes_Variacion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text,
                                                                           Mi_SQL);
                if (Ds_Ordenes_Variacion != null)
                {
                    Dt_Cuenta = Ds_Ordenes_Variacion.Tables[0];

                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Cuenta;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consulta_Detalles_Orden_Variacion
        /// DESCRIPCIÓN: Consulta la fecha de escritura a traves  del numero y anio de la orden
        /// PARÁMETROS:
        /// 	1. Datos: Indica qué registro se desea consultar a la base de datos mediante propiedades 
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 11-ago-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Detalles_Orden_Variacion(Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL
            String Filtro_SQL = ""; //Variable para el filtro de la consulta SQL

            try
            {
                Mi_SQL = "SELECT ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion 
                    + ", " + Ope_Pre_Ordenes_Variacion.Campo_Anio 
                    + ", (SELECT " + Ope_Pre_Contrarecibos.Campo_Fecha_Escritura 
                    + " FROM " + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos
                    + " WHERE " + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " = ORDEN."
                    + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + ") FECHA_ESCRITURA"
                    + ", (SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial
                    + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas
                    + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = ORDEN."
                    + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + ") CUENTA_PREDIAL"
                    + ", (SELECT " + Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal 
                    + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas
                    + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = ORDEN."
                    + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + ") VALOR_FISCAL ";
                Mi_SQL += " FROM " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " ORDEN ";

                if (Datos.P_No_Orden_Variacion != null)   // Si se recibió un ID de documento filtrar por ese ID
                {
                    Filtro_SQL = " WHERE " + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " = '" + Datos.P_No_Orden_Variacion + "'";
                }
                if (Datos.P_Anio_Orden > 0)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL += " AND " + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = " + Datos.P_Anio_Orden;
                    else
                        Filtro_SQL = " WHERE " + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = " + Datos.P_Anio_Orden;
                }
                Filtro_SQL += " ORDER BY " + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + " DESC";
                Mi_SQL = Mi_SQL + Filtro_SQL; ;

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
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
        }   //FUNCIÓN: Consulta_Detalles_Orden_Variacion


        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consulta_Calculos_Contrarecibo
        /// DESCRIPCIÓN: Consulta los campos con los datos de calculos de traslado y mediante joins los datos 
        ///             de la orden, descuentos y adeudos
        /// PARÁMETROS:
        /// 	1. Datos: Indica qué registro se desea consultar a la base de datos mediante propiedades 
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 14-ago-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Calculos_Contrarecibo(Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL
            String Filtro_SQL = ""; //Variable para el filtro de la consulta SQL

            try
            {
                Mi_SQL = "SELECT";
                if (Datos.P_Campos_Dinamicos != null && Datos.P_Campos_Dinamicos != "")
                {
                    Mi_SQL += Datos.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo + ",";
                    Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + ",";
                    Mi_SQL += " TO_NUMBER(CONTRARECIBO." + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + ") AS CONTRARECIBO,";
                    Mi_SQL += " CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ",";
                    Mi_SQL += " NVL(CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ", 'SIN REGISTRO') AS " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ",";
                    Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Fecha_Creo + " AS FECHA_CALCULO,";
                    Mi_SQL += " ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + " AS FECHA_ORDEN,";
                    Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Realizo_Calculo + " AS REALIZO_CALCULO,";
                    Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion + ",";
                    Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + ",";
                    Mi_SQL += " ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ",";
                    Mi_SQL += "  (SELECT " + Cat_Pre_Movimientos.Campo_Descripcion + " FROM "
                        + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " WHERE "
                        + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = ORDEN."
                        + Cat_Pre_Movimientos.Campo_Movimiento_ID + ") MOVIMIENTO, ";
                    Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + " AS ESTATUS_CALCULO,";
                    Mi_SQL += " NVL(REPLACE(CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus
                        + ", 'RECHAZADO', 'RECHAZO CALCULO'), REPLACE(CONTRARECIBO."
                        + Ope_Pre_Contrarecibos.Campo_Estatus + ", 'RECHAZADO', 'TRASLADO')) AS ESTATUS_CONTRARECIBO_CALCULO,";
                    Mi_SQL += " ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " AS ESTATUS_ORDEN,";
                    Mi_SQL += " DESCUENTO." + Ope_Pre_Descuento_Traslado.Campo_Estatus + " AS ESTATUS_DESCUENTO,";
                    Mi_SQL += " ADEUDO." + Ope_Pre_Adeudos_Folio.Campo_Estatus + " AS ESTATUS_ADEUDO,";
                    Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Recargos + ",";
                    Mi_SQL += " TO_NUMBER(ADEUDO." + Ope_Pre_Adeudos_Folio.Campo_Desc_Recargo + ") AS DESC_RECARGO,";
                    Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Multa + ",";
                    Mi_SQL += " TO_NUMBER(ADEUDO." + Ope_Pre_Adeudos_Folio.Campo_Desc_Multa + ") AS DESC_MULTA,";
                    Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Total_Pagar + " AS MONTO_TOTAL_CALCULO,";
                    Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Costo_Constancia + " AS COSTO_CONSTANCIA,";
                    Mi_SQL += " ADEUDO." + Ope_Pre_Adeudos_Folio.Campo_Monto + ",";
                    Mi_SQL += " TO_NUMBER(ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion;
                    Mi_SQL += ") || '/' || ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " AS FOLIO_ORDEN, ";
                    Mi_SQL += " TO_NUMBER(CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo;
                    Mi_SQL += ") || '/' || CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + " AS FOLIO_CALCULO";
                    Mi_SQL += " FROM ";
                    Mi_SQL += Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + " CONTRARECIBO";
                    Mi_SQL += " LEFT JOIN " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " ORDEN"
                        + " ON CONTRARECIBO." + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " = ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo;
                    Mi_SQL += " LEFT JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CUENTA"
                        + " ON CONTRARECIBO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Cuenta_Predial_Id + " = CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                    Mi_SQL += " LEFT JOIN " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + " CALCULO"
                        + " ON ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " = CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion
                        + " AND ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden;
                    Mi_SQL += " LEFT JOIN " + Ope_Pre_Descuento_Traslado.Tabla_Ope_Pre_Descuento_Traslado + " DESCUENTO"
                        + " ON CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo + " = DESCUENTO." + Ope_Pre_Descuento_Traslado.Campo_No_Calculo
                        + " AND CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + " = DESCUENTO." + Ope_Pre_Descuento_Traslado.Campo_Anio_Calculo
                        + " AND DESCUENTO." + Ope_Pre_Descuento_Traslado.Campo_No_Calculo + "!='CANCELADO'";
                    Mi_SQL += " LEFT JOIN " + Ope_Pre_Adeudos_Folio.Tabla_Ope_Pre_Adeudos_Folio + " ADEUDO"
                        + " ON DESCUENTO." + Ope_Pre_Descuento_Traslado.Campo_No_Descuento + " = ADEUDO." + Ope_Pre_Adeudos_Folio.Campo_No_Descuento
                        + " AND CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Cuenta_Predial_Id + " = ADEUDO." + Ope_Pre_Adeudos_Folio.Campo_Cuenta_Predial_ID;
                }
                if (!String.IsNullOrEmpty(Datos.P_No_Calculo))   // Si se recibió un ID de documento filtrar por ese ID
                {
                    Filtro_SQL = " WHERE TO_NUMBER(CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo + ") =  TO_NUMBER('" + Datos.P_No_Calculo + "')";
                }
                if (Datos.P_Estatus != null && Datos.P_Estatus_Orden != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar OR y el siguinte filtro, si no, iniciar con WHERE
                    {
                        Filtro_SQL += " AND (CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + " = '" + Datos.P_Estatus + "'";
                        Filtro_SQL += " OR ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " = '" + Datos.P_Estatus_Orden + "')";
                    }
                    else
                    {
                        Filtro_SQL = " WHERE (CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + " = '" + Datos.P_Estatus + "'";
                        Filtro_SQL += " OR ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " = '" + Datos.P_Estatus_Orden + "')";
                    }
                }
                else if (Datos.P_Estatus != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar OR y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL += " AND CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + " = '" + Datos.P_Estatus + "'";
                    else
                        Filtro_SQL = " WHERE (CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + " ='" + Datos.P_Estatus + "')";
                }
                else if (Datos.P_Estatus_Orden != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar OR y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL += " AND ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " = '" + Datos.P_Estatus_Orden + "'";
                    else
                        Filtro_SQL = " WHERE ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " = '" + Datos.P_Estatus_Orden + "'";
                }
                if (Datos.P_Estatus_Descuento != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar OR y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL += " AND DESCUENTO." + Ope_Pre_Descuento_Traslado.Campo_Estatus + " = '" + Datos.P_Estatus_Descuento + "'";
                    else
                        Filtro_SQL = " WHERE DESCUENTO." + Ope_Pre_Descuento_Traslado.Campo_Estatus + " = '" + Datos.P_Estatus_Descuento + "'";
                }
                // filtrar por CUENTA PREDIAL ID
                if (Datos.P_Cuenta_Predial_ID != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL += " AND CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + Validar_Operador_Comparacion(Datos.P_Cuenta_Predial_ID);
                    else
                        Filtro_SQL = " WHERE CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + Validar_Operador_Comparacion(Datos.P_Cuenta_Predial_ID);
                }
                // filtrar por CUENTA PREDIAL
                if (Datos.P_Cuenta_Predial != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL += " AND CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " LIKE '%" + Datos.P_Cuenta_Predial + "%'";
                    else
                        Filtro_SQL = " WHERE CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " LIKE '%" + Datos.P_Cuenta_Predial + "%'";
                }
                if (Datos.P_No_Orden_Variacion != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL += " AND ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " = " + Datos.P_No_Orden_Variacion;
                    else
                        Filtro_SQL = " WHERE ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " = " + Datos.P_No_Orden_Variacion;
                }
                if (Datos.P_Anio_Calculo > 0)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL += " AND CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + " = " + Datos.P_Anio_Calculo;
                    else
                        Filtro_SQL = " WHERE CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + " = " + Datos.P_Anio_Calculo;
                }
                if (Datos.P_Anio_Orden > 0)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL += " AND ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = " + Datos.P_Anio_Orden;
                    else
                        Filtro_SQL = " WHERE ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = " + Datos.P_Anio_Orden;
                }
                 // para busqueda por contrarecibo
                if (!String.IsNullOrEmpty(Datos.P_No_Contrarecibo))
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL += " AND TO_NUMBER(CONTRARECIBO." + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + ") " + Validar_Operador_Comparacion(Datos.P_No_Contrarecibo);
                    else
                        Filtro_SQL = " WHERE TO_NUMBER(CONTRARECIBO." + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + ") " + Validar_Operador_Comparacion(Datos.P_No_Contrarecibo);
                }
                if (Datos.P_Anio_Contrarecibo > 0)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL += " AND CONTRARECIBO." + Ope_Pre_Contrarecibos.Campo_Anio + " = " + Datos.P_Anio_Contrarecibo;
                    else
                        Filtro_SQL = " WHERE CONTRARECIBO." + Ope_Pre_Contrarecibos.Campo_Anio + " = " + Datos.P_Anio_Contrarecibo;
                }
                if (Datos.P_Mostrar_Contrarecibos_Sin_Calculo)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL += " AND CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + " = 'LISTO'";
                    else
                        Filtro_SQL = " WHERE CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + " = 'LISTO'";
                }
                // filtro dinamico
                if (!String.IsNullOrEmpty(Datos.P_Filtro_Dinamico))
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL += " AND " + Datos.P_Filtro_Dinamico;
                    else
                        Filtro_SQL = " WHERE " + Datos.P_Filtro_Dinamico;
                }
                Filtro_SQL += " AND ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " != 'CANCELADA'";
                Filtro_SQL += " ORDER BY CASE WHEN CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " IS NULL THEN 1 ELSE 2 END, CONTRARECIBO." + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo;
                Mi_SQL = Mi_SQL + Filtro_SQL;

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
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
        }   //FUNCIÓN: Consulta_Calculos

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consulta_Calculos_Contrarecibo_Cancelado
        /// DESCRIPCIÓN: Consulta los campos los datos de calculos de traslado y mediante joins los datos 
        ///             de la orden, descuentos y adeudos
        /// PARÁMETROS:
        /// 	1. Datos: Indica qué registro se desea consultar a la base de datos mediante propiedades 
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 14-ago-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Calculos_Contrarecibo_Cancelado(Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL
            String Filtro_SQL = ""; //Variable para el filtro de la consulta SQL

            try
            {
                Mi_SQL = "SELECT";
                Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo + ",";
                Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + ",";
                Mi_SQL += " TO_NUMBER(ORDEN." + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + ") AS CONTRARECIBO,";
                Mi_SQL += " CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ",";
                Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Fecha_Creo + " AS FECHA_CALCULO,";
                Mi_SQL += " ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + " AS FECHA_ORDEN,";
                Mi_SQL += " ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ",";
                Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + " AS ESTATUS_CALCULO,";
                Mi_SQL += " ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " AS ESTATUS_ORDEN,";
                Mi_SQL += " DESCUENTO." + Ope_Pre_Descuento_Traslado.Campo_Estatus + " AS ESTATUS_DESCUENTO,";
                Mi_SQL += " ADEUDO." + Ope_Pre_Adeudos_Folio.Campo_Estatus + " AS ESTATUS_ADEUDO,";
                Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Recargos + ",";
                Mi_SQL += " TO_NUMBER(ADEUDO." + Ope_Pre_Adeudos_Folio.Campo_Desc_Recargo + ") AS DESC_RECARGO,";
                Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Multa + ",";
                Mi_SQL += " TO_NUMBER(ADEUDO." + Ope_Pre_Adeudos_Folio.Campo_Desc_Multa + ") AS DESC_MULTA,";
                Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Total_Pagar + " AS MONTO_TOTAL_CALCULO,";
                Mi_SQL += " ADEUDO." + Ope_Pre_Adeudos_Folio.Campo_Monto + ",";
                Mi_SQL += " TO_NUMBER(ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion;
                Mi_SQL += ") || '/' || ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " AS FOLIO_ORDEN, ";
                Mi_SQL += " TO_NUMBER(CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo;
                Mi_SQL += ") || '/' || CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + " AS FOLIO_CALCULO";
                Mi_SQL += " FROM ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + " CALCULO";
                Mi_SQL += " LEFT JOIN " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " ORDEN";
                Mi_SQL += " ON CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion;
                Mi_SQL += " = ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion;
                Mi_SQL += " AND CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden;
                Mi_SQL += " = ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " LEFT JOIN  ";
                Mi_SQL += Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + " CONTRARECIBO";
                Mi_SQL += " ON ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo;
                Mi_SQL += " = CONTRARECIBO." + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " LEFT JOIN  ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CUENTA";
                Mi_SQL += " ON CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Cuenta_Predial_Id;
                Mi_SQL += " = CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " LEFT JOIN  ";
                Mi_SQL += Ope_Pre_Descuento_Traslado.Tabla_Ope_Pre_Descuento_Traslado + " DESCUENTO";
                Mi_SQL += " ON CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo;
                Mi_SQL += " = DESCUENTO." + Ope_Pre_Descuento_Traslado.Campo_No_Calculo;
                Mi_SQL += " AND CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo;
                Mi_SQL += " = DESCUENTO." + Ope_Pre_Descuento_Traslado.Campo_Anio_Calculo + " LEFT JOIN  ";
                Mi_SQL += Ope_Pre_Adeudos_Folio.Tabla_Ope_Pre_Adeudos_Folio + " ADEUDO";
                Mi_SQL += " ON DESCUENTO." + Ope_Pre_Descuento_Traslado.Campo_No_Descuento;
                Mi_SQL += " = ADEUDO." + Ope_Pre_Adeudos_Folio.Campo_No_Descuento;
                Mi_SQL += " WHERE CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + "='CALCULADO'";
                Filtro_SQL += " ORDER BY " + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo;
                Mi_SQL = Mi_SQL + Filtro_SQL; ;

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
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
        }   //FUNCIÓN: Consulta_Calculos


        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consulta_Detalles_Calculo
        /// DESCRIPCIÓN: Consulta todos los campos del calculo especificado
        /// PARÁMETROS:
        /// 	1. Datos: Indica qué registro se desea consultar a la base de datos mediante propiedades 
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 14-ago-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Detalles_Calculo(Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL
            String Filtro_SQL = ""; //Variable para el filtro de la consulta SQL

            try
            {
                Mi_SQL = "SELECT";
                Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo + ", ";
                Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + ", ";
                Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Cuenta_Predial_Id + ", ";
                Mi_SQL += " (SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial 
                    + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas 
                    + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID 
                    + " = CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Cuenta_Predial_Id + ") CUENTA_PREDIAL, ";
                Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion + ", ";
                Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + ", ";
                Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Tasa_ID + ", ";
                Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Multa_ID + ", ";
                Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Impuesto_Division_Lot_Id + ", ";
                Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Predio_Colindante + ", ";
                Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Base_Impuesto + ", ";
                Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Fecha_Escritura + ", ";
                Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Fecha_Creo + ", ";
                Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Costo_Constancia + ", ";
                Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Traslado + ", ";
                Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Division + ", ";
                Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Multa + ", ";
                Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Recargos + ", ";
                Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Total_Pagar + ", ";
                Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Tipo + ", ";
                Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Realizo_Calculo + ", ";
                Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Minimo_Elevado_Anio + ", ";
                Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Base_Impuesto_Division + ", ";
                Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Fundamento + ", ";
                Mi_SQL += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + ", ";
                Mi_SQL += " DETALLES." + Ope_Pre_Calc_Imp_Tras_Det.Campo_Realizo_Observacion + ", ";
                Mi_SQL += " DETALLES." + Ope_Pre_Calc_Imp_Tras_Det.Campo_Observaciones + ", ";
                Mi_SQL += " DETALLES." + Ope_Pre_Calc_Imp_Tras_Det.Campo_Fecha_Hora;
                Mi_SQL += " FROM ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + " CALCULO";
                Mi_SQL += " LEFT JOIN " + Ope_Pre_Calc_Imp_Tras_Det.Tabla_Ope_Pre_Ope_Pre_Calc_Imp_Tras_Det + " DETALLES";
                Mi_SQL += " ON CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo;
                Mi_SQL += " = DETALLES." + Ope_Pre_Calc_Imp_Tras_Det.Campo_No_Calculo;
                Mi_SQL += " AND CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo;
                Mi_SQL += " = DETALLES." + Ope_Pre_Calc_Imp_Tras_Det.Campo_Anio_Calculo;

                if (Datos.P_No_Calculo != null)   // Si se recibió un ID de documento filtrar por ese ID
                {
                    Filtro_SQL = " WHERE CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo + " = '" + Datos.P_No_Calculo + "'";
                }
                if (Datos.P_Estatus != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar OR y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL += " AND UPPER(CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + ") = UPPER('" + Datos.P_Estatus + "')";
                    else
                        Filtro_SQL = " WHERE UPPER(CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + ") = UPPER('" + Datos.P_Estatus + "')";
                }
                if (Datos.P_Cuenta_Predial != null)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL += " AND UPPER(CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Cuenta_Predial_Id + ") LIKE '" + Datos.P_Cuenta_Predial + "'";
                    else
                        Filtro_SQL = " WHERE UPPER(CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Cuenta_Predial_Id + ") LIKE '" + Datos.P_Cuenta_Predial + "'";
                }
                if (Datos.P_Anio_Calculo > 0)
                {
                    if (Filtro_SQL.Length > 0)  //Si ya hay un filtro agregar AND y el siguinte filtro, si no, iniciar con WHERE
                        Filtro_SQL += " AND CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + " = " + Datos.P_Anio_Calculo;
                    else
                        Filtro_SQL = " WHERE CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + " = " + Datos.P_Anio_Calculo;
                }
                Filtro_SQL += " ORDER BY DETALLES." + Ope_Pre_Calc_Imp_Tras_Det.Campo_Fecha_Hora + " DESC";
                Mi_SQL = Mi_SQL + Filtro_SQL; ;

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
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
        }   //FUNCIÓN: Consulta_Calculos

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consulta_Pendientes_Calcular_Rechazados
        /// DESCRIPCIÓN: Consulta los calculos rechazados pendientes de revisar y las ordenes 
        ///             con estatus de aceptadas
        /// PARÁMETROS:
        /// 	1. Datos: Indica qué registro se desea consultar de la base de datos mediante propiedades 
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 15-ago-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Pendientes_Calcular_Rechazados(Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL

            try
            {
                Mi_SQL = "SELECT";
                Mi_SQL += " ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + ", ";
                Mi_SQL += " ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " ANIO_ORDEN, ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + ", ";
                // primera subconsulta
                Mi_SQL += "  (SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " FROM "
                + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE "
                    + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = ORDEN."
                    + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ") CUENTA_PREDIAL, ";
                Mi_SQL += "    (SELECT " + Cat_Pre_Movimientos.Campo_Descripcion + " FROM "
                    + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " WHERE "
                    + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = ORDEN."
                    + Cat_Pre_Movimientos.Campo_Movimiento_ID + ") MOVIMIENTO, ";
                Mi_SQL += " TO_NUMBER(ORDEN." + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + ") CONTRARECIBO, ";
                Mi_SQL += "ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + " FECHA_ORDEN, ";
                Mi_SQL += "ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " ESTATUS_ORDEN, ";
                Mi_SQL += "NULL  FOLIO_CALCULO, ";
                Mi_SQL += "NULL FECHA_CALCULO, ";
                Mi_SQL += "NULL ESTATUS_CALCULO ";
                Mi_SQL += " FROM ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " ORDEN ";
                Mi_SQL += " WHERE ";
                Mi_SQL += "NOT " + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion 
                    + " || " + Ope_Pre_Ordenes_Variacion.Campo_Anio + " IN "
                    + "(select " + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion
                    + " || " + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + " from "
                    + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + ") AND "
                    + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " = 'ACEPTADA' AND TO_NUMBER("
                    + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + ") > 0";
                Mi_SQL += " UNION ";
                Mi_SQL += "SELECT ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion + ", ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + ", ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Cuenta_Predial_Id + ", ";
                Mi_SQL += "(SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " FROM "
                    + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE "
                    + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = CALCULO."
                    + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ") CUENTA_PREDIAL, ";
                Mi_SQL += "(SELECT (SELECT " + Cat_Pre_Movimientos.Campo_Descripcion + " FROM "
                    + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " WHERE " +
                    Cat_Pre_Movimientos.Campo_Movimiento_ID + " = ORD."
                    + Cat_Pre_Movimientos.Campo_Movimiento_ID + ") FROM "
                    + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " ORD WHERE "
                    + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " = CALCULO."
                    + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion + " AND "
                    + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = CALCULO."
                    + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + ") MOVIMIENTO, ";
                Mi_SQL += "(SELECT TO_NUMBER(" + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + ") FROM "
                    + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " WHERE "
                    + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " = CALCULO."
                    + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion + " AND "
                    + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = CALCULO."
                    + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + ") CONTRARECIBO, ";
                Mi_SQL += "(SELECT " + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + " FROM "
                    + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " WHERE "
                    + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " = CALCULO."
                    + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion + " AND "
                    + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = CALCULO."
                    + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + ") FECHA_ORDEN, ";
                Mi_SQL += "(SELECT " + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " FROM "
                    + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " WHERE "
                    + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " = CALCULO."
                    + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion + " AND "
                    + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = CALCULO."
                    + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + ") ESTATUS_ORDEN, ";
                Mi_SQL += "TO_NUMBER(" + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo
                    + ") || '/' || " + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + " FOLIO_CALCULO, ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Fecha_Creo + " FECHA_CALCULO, ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + " ESTATUS_CALCULO ";
                Mi_SQL += " FROM ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + " CALCULO ";
                Mi_SQL += " WHERE ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + " = 'RECHAZADO'";
                Mi_SQL += " ORDER BY CONTRARECIBO";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
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
        }   //FUNCIÓN: Consulta_Pendientes_Calcular_Rechazados

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Calculos_Ordenes
        /// DESCRIPCIÓN: Consulta los calculos rechazados pendientes de revisar y las ordenes 
        ///             con estatus de aceptadas (se puede filtrar por Cuenta_Predial_ID)
        /// PARÁMETROS:
        /// 	1. Datos: Indica qué registro se desea consultar de la base de datos mediante propiedades 
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 21-sep-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Calculos_Ordenes(Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL

            try
            {
                Mi_SQL = "SELECT";
                Mi_SQL += " ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + ", ";
                Mi_SQL += " ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " ANIO_ORDEN, ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + ", ";
                // primera subconsulta
                Mi_SQL += "  (SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " FROM "
                + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE "
                    + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = ORDEN."
                    + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ") CUENTA_PREDIAL, ";
                Mi_SQL += "    (SELECT " + Cat_Pre_Movimientos.Campo_Descripcion + " FROM "
                    + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " WHERE "
                    + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = ORDEN."
                    + Cat_Pre_Movimientos.Campo_Movimiento_ID + ") MOVIMIENTO, ";
                Mi_SQL += " TO_NUMBER(ORDEN." + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + ") CONTRARECIBO, ";
                Mi_SQL += "ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + " FECHA_ORDEN, ";
                Mi_SQL += "ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " ESTATUS_ORDEN, ";
                Mi_SQL += "NULL  FOLIO_CALCULO, ";
                Mi_SQL += "NULL FECHA_CALCULO, ";
                Mi_SQL += "NULL ESTATUS_CALCULO ";
                Mi_SQL += " FROM ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " ORDEN ";
                Mi_SQL += " WHERE ";
                Mi_SQL += "NOT " + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion
                    + " || " + Ope_Pre_Ordenes_Variacion.Campo_Anio + " IN "
                    + "(select " + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion
                    + " || " + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + " from "
                    + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + ") AND "
                    + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " = 'ACEPTADA' AND TO_NUMBER("
                    + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + ") > 0";
                if (!String.IsNullOrEmpty(Datos.P_Cuenta_Predial_ID))
                {
                    Mi_SQL += " AND " + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " = '"
                        + Datos.P_Cuenta_Predial_ID + "'";
                }
                Mi_SQL += " UNION ";
                Mi_SQL += "SELECT ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion + ", ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + ", ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Cuenta_Predial_Id + ", ";
                Mi_SQL += "(SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " FROM "
                    + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE "
                    + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = CALCULO."
                    + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ") CUENTA_PREDIAL, ";
                Mi_SQL += "(SELECT (SELECT " + Cat_Pre_Movimientos.Campo_Descripcion + " FROM "
                    + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " WHERE " +
                    Cat_Pre_Movimientos.Campo_Movimiento_ID + " = ORD."
                    + Cat_Pre_Movimientos.Campo_Movimiento_ID + ") FROM "
                    + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " ORD WHERE "
                    + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " = CALCULO."
                    + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion + " AND "
                    + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = CALCULO."
                    + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + ") MOVIMIENTO, ";
                Mi_SQL += "(SELECT TO_NUMBER(" + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + ") FROM "
                    + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " WHERE "
                    + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " = CALCULO."
                    + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion + " AND "
                    + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = CALCULO."
                    + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + ") CONTRARECIBO, ";
                Mi_SQL += "(SELECT " + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + " FROM "
                    + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " WHERE "
                    + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " = CALCULO."
                    + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion + " AND "
                    + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = CALCULO."
                    + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + ") FECHA_ORDEN, ";
                Mi_SQL += "(SELECT " + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " FROM "
                    + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " WHERE "
                    + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " = CALCULO."
                    + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion + " AND "
                    + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = CALCULO."
                    + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + ") ESTATUS_ORDEN, ";
                Mi_SQL += "TO_NUMBER(" + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo
                    + ") || '/' || " + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + " FOLIO_CALCULO, ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Fecha_Creo + " FECHA_CALCULO, ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + " ESTATUS_CALCULO ";
                Mi_SQL += " FROM ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + " CALCULO ";
                Mi_SQL += " WHERE ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + " = 'RECHAZADO'";
                // filtrar por cuenta predial si se proporciona
                if (!String.IsNullOrEmpty(Datos.P_Cuenta_Predial_ID))
                {
                    Mi_SQL += " AND " + Ope_Pre_Calculo_Imp_Traslado.Campo_Cuenta_Predial_Id + " = '"
                        + Datos.P_Cuenta_Predial_ID + "'";
                }

                Mi_SQL += " ORDER BY CONTRARECIBO";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
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
        }   //FUNCIÓN: Consultar_Calculos_Ordenes

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Calculos_Ordenes_Solo_Contrarecibo
        /// DESCRIPCIÓN: Consulta los calculos rechazados pendientes de revisar y las ordenes 
        ///             con estatus de aceptadas (se puede filtrar solo por contrarecibo)
        /// PARÁMETROS:
        /// 	1. Datos: Indica qué registro se desea consultar de la base de datos mediante propiedades 
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 21-sep-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Calculos_Ordenes_Solo_Contrarecibo(Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL

            try
            {
                Mi_SQL = "SELECT";
                Mi_SQL += " ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + ", ";
                Mi_SQL += " ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " ANIO_ORDEN, ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + ", ";
                // primera subconsulta
                Mi_SQL += "  (SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " FROM "
                          + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE "
                          + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = ORDEN."
                          + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ") CUENTA_PREDIAL, ";
                Mi_SQL += "    (SELECT " + Cat_Pre_Movimientos.Campo_Descripcion + " FROM "
                          + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " WHERE "
                          + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = ORDEN."
                          + Cat_Pre_Movimientos.Campo_Movimiento_ID + ") MOVIMIENTO, ";
                Mi_SQL += " TO_NUMBER(ORDEN." + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + ") CONTRARECIBO, ";
                Mi_SQL += "ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + " FECHA_ORDEN, ";
                Mi_SQL += "ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " ESTATUS_ORDEN, ";
                Mi_SQL += "NULL  FOLIO_CALCULO, ";
                Mi_SQL += "NULL FECHA_CALCULO, ";
                Mi_SQL += "NULL ESTATUS_CALCULO ";
                Mi_SQL += " FROM ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " ORDEN ";
                Mi_SQL += " WHERE ";
                Mi_SQL += "NOT " + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion
                          + " || " + Ope_Pre_Ordenes_Variacion.Campo_Anio + " IN "
                          + "(select " + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion
                          + " || " + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + " from "
                          + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + ") AND "
                          + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " = 'ACEPTADA' ";
                if (!String.IsNullOrEmpty(Datos.P_No_Contrarecibo))
                {
                    Mi_SQL += " AND TO_NUMBER("
                              + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + ") = " + Datos.P_No_Contrarecibo;
                }
                if (Datos.P_Anio_Contrarecibo > 0)
                {
                    Mi_SQL += " AND " + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = "
                              + Datos.P_Anio_Contrarecibo;
                }
                else
                {
                    Mi_SQL += " AND " + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = "
                              + DateTime.Now.Year;
                }
                Mi_SQL += " UNION ";
                Mi_SQL += "SELECT CALCULO.";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion + ", ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + ", ";
                Mi_SQL += "CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Cuenta_Predial_Id + ", ";
                Mi_SQL += "(SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " FROM "
                          + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE "
                          + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = CALCULO."
                          + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ") CUENTA_PREDIAL, ";
                Mi_SQL += "(SELECT (SELECT " + Cat_Pre_Movimientos.Campo_Descripcion + " FROM "
                          + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " WHERE " +
                          Cat_Pre_Movimientos.Campo_Movimiento_ID + " = ORD."
                          + Cat_Pre_Movimientos.Campo_Movimiento_ID + ") FROM "
                          + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " ORD WHERE "
                          + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " = CALCULO."
                          + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion + " AND "
                          + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = CALCULO."
                          + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + ") MOVIMIENTO, ";
                Mi_SQL += "(SELECT TO_NUMBER(" + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + ") FROM "
                          + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " WHERE "
                          + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " = CALCULO."
                          + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion + " AND "
                          + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = CALCULO."
                          + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + ") CONTRARECIBO, ";
                Mi_SQL += "(SELECT " + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + " FROM "
                          + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " WHERE "
                          + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " = CALCULO."
                          + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion + " AND "
                          + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = CALCULO."
                          + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + ") FECHA_ORDEN, ";
                Mi_SQL += "(SELECT " + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " FROM "
                          + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " WHERE "
                          + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " = CALCULO."
                          + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion + " AND "
                          + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = CALCULO."
                          + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + ") ESTATUS_ORDEN, ";
                Mi_SQL += "TO_NUMBER(" + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo
                          + ") || '/' || " + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + " FOLIO_CALCULO, ";
                Mi_SQL += "CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Fecha_Creo + " FECHA_CALCULO, ";
                Mi_SQL += "CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + " ESTATUS_CALCULO ";
                Mi_SQL += " FROM ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + " CALCULO ";
                // filtrar por contrarecibo si se proporciona
                if (!String.IsNullOrEmpty(Datos.P_No_Contrarecibo))
                {
                    Mi_SQL += " JOIN " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " OV ON CALCULO."
                              + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion + " = OV."
                              + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " AND CALCULO."
                              + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + " = OV."
                              + Ope_Pre_Ordenes_Variacion.Campo_Anio + " AND TO_NUMBER(OV."
                              + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + ") = "
                              + Datos.P_No_Contrarecibo;
                    if (Datos.P_Anio_Contrarecibo > 0)
                    {
                        Mi_SQL += " AND OV." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = "
                                  + Datos.P_Anio_Contrarecibo;
                    }
                    else
                    {
                        Mi_SQL += " AND OV." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = "
                                  + DateTime.Now.Year;
                    }
                }
                Mi_SQL += " WHERE CALCULO.";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + " = 'RECHAZADO'";

                Mi_SQL += " ORDER BY CONTRARECIBO";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message +
                                    "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
        }

        //FUNCIÓN: Consultar_Calculos_Ordenes_Solo_Contrarecibo

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Actualizar_Calculo
        /// DESCRIPCIÓN: Actualizar los datos de un calculo dado
        /// PARÁMETROS:
        /// 	1. Datos: Instancia de la clase de negocio con los datos que serán insertados en la base de datos
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 14-ago-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static int Actualizar_Calculo(Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Datos)
        {
            String Mi_SQL;  //Obtiene la cadena de inserción hacía la base de datos
            Int32 Filas_Afectadas = 0;
            String Mensaje;
            DateTime Fecha_Escritura;

            // variables para transaccion
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;

            try
            {
                // abrir conexion con la base de datos
                Conexion.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Conexion.Open();
                Transaccion = Conexion.BeginTransaction();
                Comando.Connection = Conexion;
                Comando.Transaction = Transaccion;

                DateTime.TryParse(Datos.P_Fecha_Escritura, out Fecha_Escritura);
                //Consulta para la inserción del No_Calculo con los datos proporcionados por el usuario
                Mi_SQL = "UPDATE " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + " SET ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + " = " + Datos.P_Anio_Calculo + ", ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Cuenta_Predial_Id + " = '" + Datos.P_Cuenta_Predial_ID + "', ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion + " = '" + Datos.P_No_Orden_Variacion + "', ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + " = " + Datos.P_Anio_Orden + ", ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Predio_Colindante + " = '" + Datos.P_Predio_Colindante + "', ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Base_Impuesto + " = '" + Datos.P_Base_Impuesto + "', ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Minimo_Elevado_Anio + " = '" + Datos.P_Minimo_Elevado_Anio + "', ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Base_Impuesto_Division + " = '" + Datos.P_Base_Impuesto_Division + "', ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Tasa_ID + " = '" + Datos.P_Tasas_ID + "', ";
                if (!String.IsNullOrEmpty(Datos.P_Multa_ID))
                {
                    Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Multa_ID + " = '" + Datos.P_Multa_ID + "', ";
                }
                else
                {
                    Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Multa_ID + " = null, ";
                }
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Fecha_Escritura + " = '" + Fecha_Escritura.ToString("dd/M/yy") + "', ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Impuesto_Division_Lot_Id + " = '" + Datos.P_Impuesto_Div_Lot + "', ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Costo_Constancia + " = '" + Datos.P_Costo_Constancia + "', ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Traslado + " = '" + Datos.P_Monto_Traslado + "', ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Division + " = '" + Datos.P_Monto_Division + "', ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Multa + " = '" + Datos.P_Monto_Multa + "', ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Recargos + " = '" + Datos.P_Monto_Recargos + "', ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Total_Pagar + " = '" + Datos.P_Monto_Total_Pagar + "', ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Tipo + " = '" + Datos.P_Tipo + "', ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Realizo_Calculo + " = '" + Datos.P_Nombre_Usuario + "', ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + " = '" + Datos.P_Estatus + "', ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_SQL += " WHERE " + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo + " = '" + Datos.P_No_Calculo + "'";
                Mi_SQL += " AND " + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + " = " + Datos.P_Anio_Calculo;
                
                Comando.CommandText = Mi_SQL;
                Filas_Afectadas += Comando.ExecuteNonQuery();

                // si hay observaciones, agregar detalle de la tabla 
                if (!String.IsNullOrEmpty(Datos.P_Observaciones))
                    Filas_Afectadas += Alta_Detalle_Calculo(Datos, Comando);

                // aplicar cambios en base de datos
                Transaccion.Commit();
                Conexion.Close();

                //regresar el número de inserciones realizadas
                return Filas_Afectadas;
            }
            catch (OracleException Ex)
            {
                    Transaccion.Rollback();
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
                    Mensaje = "Error al intentar actualizar un Registro de Cálculo. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
            }
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Actualizar_Estatus_Calculo
        /// DESCRIPCIÓN: Actualizar el estatus de un calculo dado
        /// PARÁMETROS:
        /// 	1. Datos: Instancia de la clase de negocio con los datos que serán insertados en la base de datos
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 20-ago-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static int Actualizar_Estatus_Calculo(Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Datos)
        {
            String Mi_SQL;  //Obtiene la cadena de inserción hacía la base de datos
            Int32 Filas_Afectadas = 0;
            String Mensaje = "";

            // variables para transaccion
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;

            try
            {
                // abrir conexion con la base de datos si no llego una conexion como parametro 
                if (Datos.P_Cmd_Calculo != null)
                {
                    Comando = Datos.P_Cmd_Calculo;
                }
                else
                {
                    Conexion.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                    Conexion.Open();
                    Transaccion = Conexion.BeginTransaction();
                    Comando.Connection = Conexion;
                    Comando.Transaction = Transaccion;
                }

                //Consulta para la inserción del No_Calculo con los datos proporcionados por el usuario
                Mi_SQL = "UPDATE " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + " SET ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + " = '" + Datos.P_Estatus + "', ";
                if (!String.IsNullOrEmpty(Datos.P_Fundamento))
                {
                    Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Fundamento + " = '" + Datos.P_Fundamento + "', ";
                }
                // insertar numero de adeudo si se proporciono
                if (!String.IsNullOrEmpty(Datos.P_Numero_Adeudo))
                {
                    Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_No_Adeudo + " = '" + Datos.P_Numero_Adeudo + "', ";
                }
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_SQL += " WHERE " + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo + " = '" + Datos.P_No_Calculo + "'";
                Mi_SQL += " AND " + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + " = " + Datos.P_Anio_Calculo;

                Comando.CommandText = Mi_SQL;
                Filas_Afectadas += Comando.ExecuteNonQuery();

                // si hay observaciones, agregar detalle de la tabla 
                if (!String.IsNullOrEmpty(Datos.P_Observaciones))
                    Filas_Afectadas += Alta_Detalle_Calculo(Datos, Comando);

                // aplicar cambios en base de datos
                if (Datos.P_Cmd_Calculo == null)
                {
                    Transaccion.Commit();
                }

            }

            catch (OracleException Ex)
            {
                if (Datos.P_Cmd_Calculo == null)
                {
                    Transaccion.Rollback();
                }
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
                    Mensaje = "Error al intentar actualizar un Registro de Cálculo. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Datos.P_Cmd_Calculo == null)
                {
                    Conexion.Close();
                }
            }
            //regresar el número de inserciones realizadas
            return Filas_Afectadas;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Actualizar_Estatus_Contrarecibo
        /// DESCRIPCIÓN: Actualizar el estatus de un contrarecibo, primero se consulta 
        ///             el numero de contrarecibo para el calculo y despues se actualiza
        /// PARÁMETROS:
        /// 	1. Datos: Instancia de la clase de negocio con los datos que serán insertados en la base de datos
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 09-ene-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static int Actualizar_Estatus_Contrarecibo(Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Datos)
        {
            String Mi_SQL; //Obtiene la cadena de inserción hacía la base de datos
            Int32 Filas_Afectadas = 0;
            String Mensaje = "";
            DataTable Dt_Contrarecibo;

            // variables para transaccion
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;

            try
            {
                // abrir conexion con la base de datos si no llego una conexion como parametro 
                if (Datos.P_Cmd_Calculo != null)
                {
                    Comando = Datos.P_Cmd_Calculo;
                }
                else
                {
                    Conexion.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                    Conexion.Open();
                    Transaccion = Conexion.BeginTransaction();
                    Comando.Connection = Conexion;
                    Comando.Transaction = Transaccion;
                }
                // consulta para obtener el contrarecibo del calculo
                Mi_SQL = "SELECT CR." + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo
                         + ", CR." + Ope_Pre_Contrarecibos.Campo_Anio
                         + " FROM " + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + " CR "
                         + " LEFT JOIN " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " OV "
                         + " ON OV." + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + " ="
                         + " CR." + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo
                         + " AND OV." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = "
                         + "CR." + Ope_Pre_Contrarecibos.Campo_Anio;
                Mi_SQL += " LEFT JOIN "
                          + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + " CALC "
                          + " ON CALC." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion + " = "
                          + " OV." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion
                          + " AND CALC." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + " = "
                          + " OV." + Ope_Pre_Ordenes_Variacion.Campo_Anio;
                Mi_SQL += " WHERE "
                          + "CALC." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo + " = '" + Datos.P_No_Calculo + "'"
                          + " AND CALC." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + " = " +
                          Datos.P_Anio_Calculo;
                // ejecutar la consulta
                Dt_Contrarecibo = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                // solo que se hayan recibido datos de la consulta (se encontro el numero de contrarecibo) actualizar estatus
                if (Dt_Contrarecibo != null && Dt_Contrarecibo.Rows.Count > 0)
                {
                    // formar la consulta para actualizar el estatus del contrarecibo
                    Mi_SQL = "UPDATE " + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + " SET "
                             + Ope_Pre_Contrarecibos.Campo_Estatus + " = '" + Datos.P_Estatus + "', ";
                    Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Usuario_Modifico
                              + " = '" + Datos.P_Nombre_Usuario + "', ";
                    Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Fecha_Modifico + " = SYSDATE ";
                    Mi_SQL += " WHERE " + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo 
                            + " = '" + Dt_Contrarecibo.Rows[0][0].ToString() + "'";
                    Mi_SQL += " AND " + Ope_Pre_Contrarecibos.Campo_Anio
                        + " = " + Dt_Contrarecibo.Rows[0][1].ToString();

                    Comando.CommandText = Mi_SQL;
                    Filas_Afectadas += Comando.ExecuteNonQuery();

                    // aplicar cambios en base de datos (si la transaccion es local)
                    if (Datos.P_Cmd_Calculo == null)
                    {
                        Transaccion.Commit();
                    }
                }
            }

            catch (OracleException Ex)
            {
                if (Datos.P_Cmd_Calculo == null)
                {
                    Transaccion.Rollback();
                }
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152)
                {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" +
                              Ex.Message + "]";
                }
                else if (Ex.Code == 2627)
                {
                    if (Ex.Message.IndexOf("PRIMARY") != -1)
                    {
                        Mensaje =
                            "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" +
                            Ex.Message + "]";
                    }
                    else if (Ex.Message.IndexOf("UNIQUE") != -1)
                    {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" +
                                  Ex.Message + "]";
                    }
                    else
                    {
                        Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]";
                    }
                }
                else if (Ex.Code == 547)
                {
                    Mensaje =
                        "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" +
                        Ex.Message + "]";
                }
                else if (Ex.Code == 515)
                {
                    Mensaje =
                        "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" +
                        Ex.Message + "]";
                }
                else
                {
                    Mensaje = "Error al intentar actualizar un Registro de Cálculo. Error: [" + Ex.Message + "]";
                    //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Datos.P_Cmd_Calculo == null)
                {
                    Conexion.Close();
                }
            }
            //regresar el número de inserciones realizadas
            return Filas_Afectadas;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Obtener_Consecutivo_Pasivos
        /// DESCRIPCIÓN: Obtiene el numero consecutivo para la trabla de pasivos
        /// PARÁMETROS:
        /// 	1. Campo_ID: Nombre del campo del que se obtendra consecutivo
        /// 	2. Tabla: Nombre de la tabla
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 24-ago-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static int Obtener_Consecutivo_Pasivos(String Campo_ID, String Tabla, OracleCommand Cmd)
        {
            int Consecutivo = 0;
            String Mi_Sql;
            Object Obj; //Obtiene el ID con la cual se guardo los datos en la base de datos
            Mi_Sql = "SELECT NVL(MAX (" + Campo_ID + "),0) FROM " + Tabla;
            if (Cmd != null)
            {
                Cmd.CommandText = Mi_Sql;
                int.TryParse( Cmd.ExecuteOracleScalar().ToString(), out Consecutivo);
            }
            else
            {
                Obj = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                int.TryParse(Obj.ToString(), out Consecutivo);
            }

            return Consecutivo + 1;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Descuentos_Traslado
        /// DESCRIPCIÓN: Consulta los descuentos que apliquen para el calculo
        /// PARÁMETROS:
        /// 	1. Datos: Indica qué registro se desea consultar a la base de datos mediante propiedades 
        /// CREO: Ismael Prieto S{anchez
        /// FECHA_CREO: 10-oct-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Descuentos_Traslado(Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Datos)
        {
//            OracleCommand Comando = new OracleCommand(); //variable del command
            String Mi_SQL; //Variable para la consulta SQL
            DataTable Dt_Descuentos = new DataTable();
  //          DataRow Registro;

            try
            {
                //asigna el command 
    //            Comando = Datos.P_Cmd_Calculo;

                //crea la estructura de la tabla
                //Dt_Descuentos.Columns.Add("No_Descuento");
                //Dt_Descuentos.Columns.Add("Anio_Calculo");
                //Dt_Descuentos.Columns.Add("No_Calculo");
                //Dt_Descuentos.Columns.Add(new DataColumn("Monto_Multas", typeof(double)));
                //Dt_Descuentos.Columns.Add(new DataColumn("Monto_Recargos", typeof(double)));

                //realiza la consulta
                Mi_SQL = "SELECT ";
                Mi_SQL += Ope_Pre_Descuento_Traslado.Campo_No_Descuento + ", ";
                Mi_SQL += Ope_Pre_Descuento_Traslado.Campo_Anio_Calculo + ", ";
                Mi_SQL += Ope_Pre_Descuento_Traslado.Campo_No_Calculo + ", ";
                Mi_SQL += Ope_Pre_Descuento_Traslado.Campo_Monto_Multas + ", ";
                Mi_SQL += Ope_Pre_Descuento_Traslado.Campo_Monto_Recargos;
                Mi_SQL += " FROM " + Ope_Pre_Descuento_Traslado.Tabla_Ope_Pre_Descuento_Traslado;
                Mi_SQL += " WHERE " + Ope_Pre_Descuento_Traslado.Campo_No_Calculo + " = '" + Datos.P_No_Calculo + "'";
                Mi_SQL += " AND " + Ope_Pre_Descuento_Traslado.Campo_Anio_Calculo + " = " + Datos.P_Anio_Calculo;
                Dt_Descuentos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                //Comando.CommandText = Mi_SQL;
                //OracleDataReader Dr = Comando.ExecuteReader();
                //if (Dr.Read()) 
                //{
                //    Registro=Dt_Descuentos.NewRow();
                //    Registro["No_Descuento"] = Dr[Ope_Pre_Descuento_Traslado.Campo_No_Descuento];
                //    Registro["Anio_Calculo"] = Dr[Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo];
                //    Registro["No_Calculo"] = Dr[Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo];
                //    Registro["Monto_Multa"] = Dr[Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Multa];
                //    Registro["Monto_Recargos"] = Dr[Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Recargos];
                //    Dt_Descuentos.Rows.Add(Registro);
                //}
                //Dr.Close();

                return Dt_Descuentos;
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
        }   //FUNCIÓN: Consultar_Descuentos_Traslado

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consulta_Calculos_Convenio
        /// DESCRIPCIÓN: Consulta los impuestos y los descuentos, asi como el costo de la constancia.
        /// PARÁMETROS:
        /// 	1. Datos: Indica qué registro se desea consultar a la base de datos mediante propiedades 
        /// CREO: Miguel Angel Bedolla Moreno
        /// FECHA_CREO: 14-ago-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Calculos_Convenio(Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL

            try
            {
                Mi_SQL = "SELECT CAL.";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Division + " AS MONTO_DIVISION, CAL.";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Costo_Constancia + " AS COSTO_CONSTANCIA, CAL.";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Traslado + " AS MONTO_TRASLADO, NVL((DES.";
                Mi_SQL += Ope_Pre_Descuento_Traslado.Campo_Desc_Multa + "),0) AS DESC_MULTA , NVL((CAL.";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Recargos + "),0) AS MONTO_RECARGOS , NVL((CAL.";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Multa + "),0) AS MONTO_MULTAS , NVL((DES.";
                Mi_SQL += Ope_Pre_Descuento_Traslado.Campo_Desc_Recargo + "),0) AS DESC_RECARGO, CAL.";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + " AS ESTATUS, CAL.";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Fecha_Creo + " AS FECHA_CALCULO, CAL.";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Realizo_Calculo + " AS REALIZO_CALCULO ";
                Mi_SQL += " FROM ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + " CAL ";
                Mi_SQL += "LEFT OUTER JOIN " + Ope_Pre_Descuento_Traslado.Tabla_Ope_Pre_Descuento_Traslado + " DES ON CAL.";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo + " = DES." + Ope_Pre_Descuento_Traslado.Campo_No_Calculo;
                Mi_SQL += " WHERE CAL." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo + "='" + Datos.P_No_Calculo + "'";
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
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
        }   //FUNCIÓN: Consulta_Calculos_Convenio

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Validar_Operador_Comparacion
        ///DESCRIPCIÓN          : Devuelve una cadena adecuada al operador indicado en la capa de Negocios
        ///PARAMETROS           : 
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 20/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static String Validar_Operador_Comparacion(String Filtro)
        {
            String Cadena_Validada;
            if (Filtro.Trim().StartsWith("<")
               || Filtro.Trim().StartsWith(">")
               || Filtro.Trim().StartsWith("<>")
               || Filtro.Trim().StartsWith("<=")
               || Filtro.Trim().StartsWith(">=")
               || Filtro.Trim().StartsWith("=")
               || Filtro.Trim().ToUpper().StartsWith("BETWEEN")
               || Filtro.Trim().ToUpper().StartsWith("LIKE")
               || Filtro.Trim().ToUpper().StartsWith("IN"))
            {
                Cadena_Validada = " " + Filtro + " ";
            }
            else
            {
                if (Filtro.Trim().ToUpper().StartsWith("NULL"))
                {
                    Cadena_Validada = " IS " + Filtro + " ";
                }
                else
                {
                    if (Filtro.Trim().StartsWith("'") && Filtro.Trim().EndsWith("'"))
                    {
                        Cadena_Validada = " = " + Filtro;
                    }
                    else
                    {
                        Cadena_Validada = " = '" + Filtro + "' ";
                    }
                }
            }
            return Cadena_Validada;
        }
    }

}
