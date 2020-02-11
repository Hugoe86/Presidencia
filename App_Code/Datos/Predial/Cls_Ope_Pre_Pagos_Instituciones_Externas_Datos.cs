using System;
using System.Data;
using Presidencia.Operacion_Predial_Pagos_Instit_Externas.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using System.Collections.Generic;
using Presidencia.Caja_Pagos.Negocio;

namespace Presidencia.Operacion_Predial_Pagos_Instit_Externas.Datos
{

    public class Cls_Ope_Pre_Pagos_Instituciones_Externas_Datos
    {

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Alta_Captura_Pagos
        /// DESCRIPCIÓN: dar de alta los dato de captura de pagos en instituciones externas
        /// PARÁMETROS:
        /// 	1. Datos: Instancia de la clase de negocio con los datos que serán insertados en la base de datos
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 19-ene-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static Int32 Alta_Captura_Pagos(Cls_Ope_Pre_Pagos_Instituciones_Externas_Negocio Datos)
        {
            String Mi_SQL;
            Object Obj_No_Captura;
            String No_Captura;
            int Consecutivo_Detalle = 0;
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;
            Int32 Filas_Afectadas = 0;
            List<string> Lista_Cuentas = new List<string>();
            var Aplicar_Pagos = new Cls_Ope_Caj_Pagos_Negocio();

            // obtener el no_captura para la insercion
            Mi_SQL = "SELECT NVL(MAX(" + Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_No_Captura + "),'0000000000') ";
            Mi_SQL += "FROM " + Ope_Pre_Capturas_Pagos_Instituciones_Externas.Tabla_Ope_Pre_Capturas_Pag_Ins_Ext;
            Obj_No_Captura = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            if (Convert.IsDBNull(Obj_No_Captura) || Obj_No_Captura == null)
            {
                No_Captura = "0000000001";
            }
            else
            {
                No_Captura = String.Format("{0:0000000000}", Convert.ToInt32(Obj_No_Captura) + 1);
            }
            // consulta para obtener el maximo valor de la tabla detalles de capturas
            Mi_SQL = "SELECT NVL(MAX(" + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_No_Detalle_Captura + "),'0000000000') ";
            Mi_SQL += "FROM " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Tabla_Ope_Pre_Det_Capt_Pag_Ins_Ext;
            Obj_No_Captura = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            if (Convert.IsDBNull(Obj_No_Captura) || Obj_No_Captura == null)
            {
                Consecutivo_Detalle = 1;
            }
            else
            {
                int.TryParse(Obj_No_Captura.ToString(), out Consecutivo_Detalle);
            }

            // si llego un Comando como parametro, utilizarlo
            if (Datos.P_Comando_Oracle != null)    // si la conexion llego como parametro, establecer como comando para utilizar
            {
                Comando = Datos.P_Comando_Oracle;
            }
            else    // si no, crear nueva conexion y transaccion
            {
                Conexion.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Conexion.Open();
                Transaccion = Conexion.BeginTransaction();
                Comando.Connection = Conexion;
                Comando.Transaction = Transaccion;
            }

            // solo si llegan datos en la tabla para insertar
            if (Datos.P_Dt_Capturas != null && Datos.P_Dt_Capturas.Rows.Count > 0)
            {
                Mi_SQL = "INSERT INTO " + Ope_Pre_Capturas_Pagos_Instituciones_Externas.Tabla_Ope_Pre_Capturas_Pag_Ins_Ext + " ("
                    + Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_No_Captura
                    + ", " + Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Institucion_Id
                    + ", " + Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Fecha_Captura
                    + ", " + Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Nombre_Archivo
                    + ", " + Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Fecha_Corte
                    + ", " + Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Usuario_Creo
                    + ", " + Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Fecha_Creo;
                Mi_SQL += ") VALUES ("
                    + "'" + No_Captura + "'"
                    + ", '" + Datos.P_Dt_Capturas.Rows[0][Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Institucion_Id] + "'"
                    + ", '" + Datos.P_Dt_Capturas.Rows[0].Field<DateTime>(Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Fecha_Captura).ToString("dd/MM/yyyy") + "'"
                    + ", '" + Datos.P_Dt_Capturas.Rows[0][Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Nombre_Archivo] + "'"
                    + ", '" + Datos.P_Dt_Capturas.Rows[0].Field<DateTime>(Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Fecha_Corte).ToString("dd/MM/yyyy") + "'"
                    + ", '" + Datos.P_Usuario + "', SYSDATE)";
                try
                {
                    Comando.CommandText = Mi_SQL;
                    Filas_Afectadas = Comando.ExecuteNonQuery();

                    // recorrer la tabla de detalles para insertar cada dato
                    foreach (DataRow Detalle in Datos.P_Dt_Detalles_Captura.Rows)
                    {

                        Mi_SQL = "INSERT INTO " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Tabla_Ope_Pre_Det_Capt_Pag_Ins_Ext + " ("
                            + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_No_Detalle_Captura
                            + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_No_Captura
                            + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial_Id
                            + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial
                            + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial_Original
                            + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Linea_Captura
                            + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Fecha_Pago
                            + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Pagado
                            + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Pagado_Original
                            + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Total_Adeudo
                            + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Rezago
                            + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Corriente
                            + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Recargos
                            + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Honorarios
                            + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Descuento
                            + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Diferencia
                            + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Sucursal
                            + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Tipo_Pago
                            + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Guia_Cie
                            + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cajero
                            + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Autorizacion
                            + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Incluido
                            + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Descripcion
                            + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Usuario_Creo
                            + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Fecha_Creo;
                        Mi_SQL += ") VALUES ("
                            + "'" + (++Consecutivo_Detalle).ToString().PadLeft(10, '0') + "'"
                            + ", '" + No_Captura + "'"
                            + ", '" + Detalle[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial_Id] + "'"
                            + ", '" + Detalle[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial] + "'"
                            + ", '" + Detalle[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial_Original] + "'"
                            + ", '" + Detalle[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Linea_Captura] + "'"
                            + ", '" + Detalle.Field<DateTime>(Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Fecha_Pago).ToString("dd/MM/yyyy") + "'"
                            + ", '" + Detalle[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Pagado] + "'"
                            + ", '" + Detalle[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Pagado_Original] + "'"
                            + ", '" + Detalle[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Total_Adeudo] + "'"
                            + ", '" + Detalle[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Rezago] + "'"
                            + ", '" + Detalle[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Corriente] + "'"
                            + ", '" + Detalle[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Recargos] + "'"
                            + ", '" + Detalle[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Honorarios] + "'"
                            + ", '" + Detalle[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Descuento] + "'"
                            + ", '" + Detalle[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Diferencia] + "'"
                            + ", '" + Detalle[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Sucursal] + "'"
                            + ", '" + Detalle[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Tipo_Pago] + "'"
                            + ", '" + Detalle[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Guia_Cie] + "'"
                            + ", '" + Detalle[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cajero] + "'"
                            + ", '" + Detalle[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Autorizacion] + "'"
                            + ", '" + Detalle[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Incluido] + "'"
                            + ", '" + Detalle[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Descripcion] + "'"
                            + ", '" + Datos.P_Usuario + "', SYSDATE)";

                        Comando.CommandText = Mi_SQL;
                        Filas_Afectadas = Comando.ExecuteNonQuery();

                        // agregar a la lista de cuentas a las que se aplica pago
                        if (Detalle[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Incluido].ToString() == "SI")
                        {
                            Lista_Cuentas.Add(Detalle[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial_Id].ToString());
                        }
                    }

                    // llamar método para aplicar pagos pasando como parámetro las tablas y la lista de cuentas
                    Aplicar_Pagos.P_Comando_Oracle = Comando;
                    Aplicar_Pagos.P_Nombre_Usuario = Datos.P_Usuario;
                    Aplicar_Pagos.P_Dt_Adeudos_Predial_Cajas = Datos.P_Dt_Adeudo_Detallado_Predial;
                    Aplicar_Pagos.P_Dt_Adeudos_Predial_Cajas_Detalle = Datos.P_Dt_Adeudos_Totales;
                    Aplicar_Pagos.P_Dt_Pasivos = Datos.P_Dt_Pasivos_Pago;
                    Aplicar_Pagos.P_Caja_ID = Datos.P_Caja_ID;
                    Aplicar_Pagos.Alta_Pago_Instituciones_Externas(Lista_Cuentas);

                    if (Datos.P_Comando_Oracle == null)    // si la conexion no llego como parametro, aplicar consultas
                    {
                        Transaccion.Commit();
                    }

                }
                catch (OracleException Ex)
                {
                    if (Datos.P_Comando_Oracle == null)
                    {
                        Transaccion.Rollback();
                    }
                    throw new Exception("Error: " + Ex.Message);
                }
                catch (DBConcurrencyException Ex)
                {
                    throw new Exception("Los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error: " + Ex.Message);
                }
                finally
                {
                    if (Datos.P_Comando_Oracle == null)
                    {
                        Conexion.Close();
                    }
                }

            }
            return Filas_Afectadas;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Cuenta_Predial_ID
        /// DESCRIPCIÓN: Consulta el ID de la cuenta predial y del tipo de predio para una cuenta dada
        /// PARÁMETROS:
        /// 		1. Datos: instancia de la clase de negocio con los datos para la consulta
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 19-ene-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Cuenta_Predial_ID(Cls_Ope_Pre_Pagos_Instituciones_Externas_Negocio Datos)
        {
            DataSet Ds_Resultado = new DataSet();
            String Mi_SQL;

            try
            {
                Mi_SQL = "SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID
                    + "," + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID
                    + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas
                    + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial
                    + " = '" + Datos.P_Cuenta_Predial + "'";


                Ds_Resultado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Resultado != null && Ds_Resultado.Tables.Count > 0)
                {
                    return Ds_Resultado.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Cuenta predial. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return null;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Propietario
        /// DESCRIPCIÓN: Consulta el nombre del propietario de la cuenta predial y el id de propietario y contribuyente
        /// PARÁMETROS:
        /// 		1. Datos: instancia de la clase de negocio con los datos para la consulta
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 02-abr-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Propietario(Cls_Ope_Pre_Pagos_Instituciones_Externas_Negocio Datos)
        {
            DataSet Ds_Resultado = new DataSet();
            String Mi_SQL;

            try
            {
                Mi_SQL = "SELECT " + Cat_Pre_Propietarios.Campo_Propietario_ID
                    + "," + Cat_Pre_Propietarios.Campo_Contribuyente_ID
                    + ",(SELECT " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || "
                    + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || "
                    + Cat_Pre_Contribuyentes.Campo_Nombre + " FROM "
                    + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " WHERE "
                    + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + "="
                    + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + ") NOMBRE_PROPIETARIO"
                    + " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios
                    + " WHERE " + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID
                    + " = '" + Datos.P_Cuenta_Predial_Id + "' AND "
                    + Cat_Pre_Propietarios.Campo_Tipo + " IN ('PROPIETARIO','POSEEDOR') AND ROWNUM=1";

                Ds_Resultado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Resultado != null && Ds_Resultado.Tables.Count > 0)
                {
                    return Ds_Resultado.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Cuenta predial. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return null;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Linea_Captura
        /// DESCRIPCIÓN: Consulta la cuenta predial, su id y el id del tipo de predio para la linea de captura dada
        /// PARÁMETROS:
        /// 		1. Datos: instancia de la clase de negocio con los datos para la consulta
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 19-ene-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Linea_Captura(Cls_Ope_Pre_Pagos_Instituciones_Externas_Negocio Datos)
        {
            DataTable Dt_Cuenta = new DataTable();
            String Mi_SQL;
            DataSet Ds_Resultado;

            try
            {
                Mi_SQL = "SELECT " + Ope_Pre_Lineas_Captura.Campo_Cuenta_Predial_Id
                    + ", " + Ope_Pre_Lineas_Captura.Campo_Linea_Captura_Enero
                    + ", " + Ope_Pre_Lineas_Captura.Campo_Linea_Captura_Febrero
                    + ", (SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial
                    + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas
                    + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID
                    + " = " + Ope_Pre_Lineas_Captura.Tabla_Ope_Pre_Lineas_Captura + "."
                    + Ope_Pre_Lineas_Captura.Campo_Cuenta_Predial_Id
                    + ") " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial
                    + ", (SELECT " + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID
                    + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas
                    + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID
                    + " = " + Ope_Pre_Lineas_Captura.Tabla_Ope_Pre_Lineas_Captura + "."
                    + Ope_Pre_Lineas_Captura.Campo_Cuenta_Predial_Id
                    + ") " + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID
                    + " FROM " + Ope_Pre_Lineas_Captura.Tabla_Ope_Pre_Lineas_Captura + " WHERE ";
                if (!string.IsNullOrEmpty(Datos.P_Institucion_Id))
                {
                    Mi_SQL += Ope_Pre_Lineas_Captura.Campo_Institucion_Id
                        + " = '" + Datos.P_Institucion_Id + "' AND ";
                }
                if (!String.IsNullOrEmpty(Datos.P_Linea_Captura))
                {
                    Mi_SQL += "(" + Ope_Pre_Lineas_Captura.Campo_Linea_Captura_Enero
                        + " = '" + Datos.P_Linea_Captura + "'"
                        + " OR " + Ope_Pre_Lineas_Captura.Campo_Linea_Captura_Febrero
                        + " = '" + Datos.P_Linea_Captura + "')";
                }

                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                else if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }

                Ds_Resultado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Resultado != null && Ds_Resultado.Tables.Count > 0)
                {
                    return Ds_Resultado.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de linea de captura. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return null;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Claves_Ingreso
        ///DESCRIPCIÓN: Realiza una consulta para obtener un detalle especifico de la Clave de Ingreso
        ///PARAMETROS:
        ///             1. Datos. Instancia de la Clase de negocio con información para forma consulta
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 02-mar-2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Claves_Ingreso(Cls_Ope_Pre_Pagos_Instituciones_Externas_Negocio Datos)
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT  C." + Cat_Pre_Claves_Ingreso.Campo_Clave
                    + ", C." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID
                    + ", C." + Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID
                    + ", D." + Cat_Pre_Claves_Ingreso_Det.Campo_Tipo_Predial_Traslado
                    + " FROM " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + " C LEFT JOIN "
                    + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det + " D ON "
                    + " C." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID + " = D." + Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID;

                // filtrar
                if (!String.IsNullOrEmpty(Datos.P_Filtro_Dinamico))
                {
                    Mi_SQL += " WHERE " + Datos.P_Filtro_Dinamico;
                }
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Detalles de Claves de Ingreso. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Capturas_Pagos
        /// DESCRIPCIÓN: Consulta los datos generales de capturas
        /// PARÁMETROS:
        /// 		1. Datos: instancia de la clase de negocio con los datos para la consulta
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 19-ene-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Capturas_Pagos(Cls_Ope_Pre_Pagos_Instituciones_Externas_Negocio Datos)
        {
            DataTable Dt_Cuenta = new DataTable();
            String Mi_SQL;
            DataSet Ds_Resultado;

            try
            {
                Mi_SQL = "SELECT " + Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_No_Captura
                    + ", " + Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Institucion_Id
                    + ", " + Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Fecha_Captura
                    + ", " + Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Fecha_Corte
                    + ", " + Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Nombre_Archivo
                    + ", (SELECT " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion
                    + " FROM " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Tabla_Cat_Pre_Instituciones_Que_Reciben_Pagos
                    + " WHERE " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion_Id
                    + " = " + Ope_Pre_Capturas_Pagos_Instituciones_Externas.Tabla_Ope_Pre_Capturas_Pag_Ins_Ext + "."
                    + Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Institucion_Id
                    + ") " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion
                    + ", (SELECT (SELECT " + Cat_Pre_Cajas.Campo_Numero_De_Caja + " FROM "
                    + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " WHERE "
                    + Cat_Pre_Cajas.Campo_Caja_ID + " = "
                    + Cat_Pre_Instituciones_Que_Reciben_Pagos.Tabla_Cat_Pre_Instituciones_Que_Reciben_Pagos + "."
                    + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Caja + ")"
                    + " FROM " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Tabla_Cat_Pre_Instituciones_Que_Reciben_Pagos
                    + " WHERE " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion_Id
                    + " = " + Ope_Pre_Capturas_Pagos_Instituciones_Externas.Tabla_Ope_Pre_Capturas_Pag_Ins_Ext + "."
                    + Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Institucion_Id
                    + ") " + Cat_Pre_Cajas.Campo_Numero_De_Caja
                    + ", (SELECT COUNT(" + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_No_Captura
                    + ") FROM " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Tabla_Ope_Pre_Det_Capt_Pag_Ins_Ext
                    + " WHERE " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_No_Captura
                    + " = " + Ope_Pre_Capturas_Pagos_Instituciones_Externas.Tabla_Ope_Pre_Capturas_Pag_Ins_Ext + "."
                    + Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_No_Captura
                    + ") MOVIMIENTOS"
                    + ", (SELECT COUNT(" + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_No_Captura
                    + ") FROM " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Tabla_Ope_Pre_Det_Capt_Pag_Ins_Ext
                    + " WHERE " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_No_Captura
                    + " = " + Ope_Pre_Capturas_Pagos_Instituciones_Externas.Tabla_Ope_Pre_Capturas_Pag_Ins_Ext + "."
                    + Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_No_Captura
                    + " AND " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Incluido + "='SI'"
                    + ") INCLUIDOS"
                    + ", (SELECT COUNT(" + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_No_Captura
                    + ") FROM " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Tabla_Ope_Pre_Det_Capt_Pag_Ins_Ext
                    + " WHERE " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_No_Captura
                    + " = " + Ope_Pre_Capturas_Pagos_Instituciones_Externas.Tabla_Ope_Pre_Capturas_Pag_Ins_Ext + "."
                    + Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_No_Captura
                    + " AND " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Incluido + "='NO'"
                    + ") EXCLUIDOS"
                    + " FROM " + Ope_Pre_Capturas_Pagos_Instituciones_Externas.Tabla_Ope_Pre_Capturas_Pag_Ins_Ext + " WHERE ";
                if (!string.IsNullOrEmpty(Datos.P_Institucion_Id))
                {
                    Mi_SQL += Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Institucion_Id
                        + " = '" + Datos.P_Institucion_Id + "' AND ";
                }
                if (!String.IsNullOrEmpty(Datos.P_Anio))
                {
                    // TO_CHAR(FECHA_CAPTURA, 'yyyy', 'NLS_LANGUAGE = "MEXICAN SPANISH"')
                    Mi_SQL += "TO_CHAR(" + Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Fecha_Captura
                        + ", 'yyyy', 'NLS_LANGUAGE = \"MEXICAN SPANISH\"') = '" + Datos.P_Anio + "'";
                }

                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                else if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }

                Ds_Resultado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Resultado != null && Ds_Resultado.Tables.Count > 0)
                {
                    return Ds_Resultado.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return null;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Detalles_Captura
        /// DESCRIPCIÓN: Consulta los detalles de capturas
        /// PARÁMETROS:
        /// 		1. Datos: instancia de la clase de negocio con los datos para la consulta
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 21-ene-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Detalles_Captura(Cls_Ope_Pre_Pagos_Instituciones_Externas_Negocio Datos)
        {
            DataTable Dt_Cuenta = new DataTable();
            String Mi_SQL;
            DataSet Ds_Resultado;

            try
            {
                Mi_SQL = "SELECT " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial
                    + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial_Id
                    + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial_Original
                    + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Linea_Captura
                    + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Fecha_Pago
                    + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Pagado
                    + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Total_Adeudo
                    + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Diferencia
                    + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Sucursal
                    + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Tipo_Pago
                    + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Guia_Cie
                    + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cajero
                    + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Autorizacion
                    + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Incluido
                    + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Descripcion
                    + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Pagado_Original
                    + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Corriente
                    + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Rezago
                    + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Recargos
                    + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Honorarios
                    + ", " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Descuento
                    + ", ROWNUM CONSECUTIVO" 
                    + " FROM " + Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Tabla_Ope_Pre_Det_Capt_Pag_Ins_Ext + " WHERE ";
                if (!string.IsNullOrEmpty(Datos.P_No_Captura))
                {
                    Mi_SQL += Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_No_Captura
                        + " = '" + Datos.P_No_Captura + "' AND ";
                }
                if (!String.IsNullOrEmpty(Datos.P_Filtro_Dinamico))
                {
                    Mi_SQL += Datos.P_Filtro_Dinamico;
                }

                // quitar WHERE o AND del final de la consulta
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                else if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }

                Ds_Resultado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Resultado != null && Ds_Resultado.Tables.Count > 0)
                {
                    return Ds_Resultado.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return null;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Anio_Captura
        /// DESCRIPCIÓN: Consulta los años en los que se han realizado capturas de pagos externos
        /// PARÁMETROS:
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 21-ene-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Anio_Captura()
        {
            String Mi_SQL;
            DataSet Ds_Resultado;

            try
            {
                // SELECT DISTINCT(TO_CHAR(FECHA_CAPTURA, 'yyyy', 'NLS_LANGUAGE = "MEXICAN SPANISH"')) ANIO FROM OPE_PRE_CAPTURAS_PAG_INS_EXT;
                Mi_SQL = "SELECT DISTINCT(TO_CHAR(" + Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Fecha_Captura
                    + ", 'yyyy', 'NLS_LANGUAGE = \"MEXICAN SPANISH\"')) ANIO FROM "
                    + Ope_Pre_Capturas_Pagos_Instituciones_Externas.Tabla_Ope_Pre_Capturas_Pag_Ins_Ext;

                Ds_Resultado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                // regresar valores obtenidos de la consulta
                if (Ds_Resultado != null && Ds_Resultado.Tables.Count > 0)
                {
                    return Ds_Resultado.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return null;
        }

    }

}