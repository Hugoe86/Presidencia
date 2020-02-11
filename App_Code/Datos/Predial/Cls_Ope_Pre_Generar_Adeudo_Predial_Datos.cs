using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio;
using System.Text;

namespace Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Datos
{
    public class Cls_Ope_Pre_Generar_Adeudo_Predial_Datos
    {

        private static Int32 Contador_Insercion = 0;

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consulta_Numeros_Cuenta_Predial
        /// DESCRIPCIÓN: Obtiene un listado de IDs de cuentas predial (filtrar por estatus)
        ///             Utilizando un DataReader
        /// PARÁMETROS:
        /// 	1. Datos: Indica qué registros se desea consultar de la base de datos
        /// 	        Para filtrar por estatus, se debe especificar entre comilla simple y pueden 
        /// 	        especificarse varios valores separados por comas
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 22-jul-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static List<String> Consulta_Numeros_Cuenta_Predial(Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL
            String Filtro_SQL = "";
            List<String> IDs_Cuentas = new List<string>();

            // Variables para conexion a la base de datos con un datareader
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleDataReader Reader;

            try
            {
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas;

                if (Datos.p_Estatus_Excluir != null)   // Si se recibió un ESTATUS filtrar 
                {
                    Filtro_SQL += " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Estatus + Datos.p_Estatus_Excluir;
                }
                if (!String.IsNullOrEmpty(Datos.p_Tipo_Suspension_Excluir))
                {
                    if (Filtro_SQL.Length > 0)
                    {
                        Filtro_SQL += " AND NVL(" + Cat_Pre_Cuentas_Predial.Campo_Tipo_Suspencion + ",' ') " + Datos.p_Tipo_Suspension_Excluir;
                    }
                    else
                    {
                        Filtro_SQL += " WHERE NVL(" + Cat_Pre_Cuentas_Predial.Campo_Tipo_Suspencion + ",' ') " + Datos.p_Tipo_Suspension_Excluir;
                    }
                }
                Mi_SQL += Filtro_SQL;
                // ejecutar la conexion a la base de datos con un datareader
                Conexion.ConnectionString = Cls_Constantes.Str_Conexion;        // string de conexion
                Comando.CommandText = Mi_SQL;                                   // consulta SQL
                Comando.Connection = Conexion;                                  // Conexion
                Conexion.Open();
                Reader = Comando.ExecuteReader(CommandBehavior.CloseConnection);

                while (Reader.Read())       // mientras se sigan leyendo registros, agregar a la lista
                {
                    IDs_Cuentas.Add(Reader[Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString());
                }

                return IDs_Cuentas;
                //return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
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
        }   //FUNCIÓN: Consulta_Numeros_Cuenta_Predial


        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Total_Cuentas
        /// DESCRIPCIÓN: Consulta el total de cuentas filtradas por estatus
        ///             Regresa un valor entero
        /// PARÁMETROS:
        /// 	1. Estatus: Lista entre comillas separada por comas de los estatus a excluir 'ACTIVA','CANCELADA'
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 23-jul-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static Int32 Consultar_Total_Cuentas(String Excluir, String Estatus, String Excluir_Tipo_Suspension)
        {
            String Mi_SQL; //Variable para la consulta SQL
            String Filtro_SQL = "";
            Int32 Total_Cuentas;
            object Total;

            try
            {
                Mi_SQL = "SELECT COUNT(" + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                Mi_SQL += ") FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas;
                // filtrar por estatus
                if (!String.IsNullOrEmpty(Excluir))  // si el parametro no es nulo, excluir cuentas
                {
                    Filtro_SQL = " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Estatus + Excluir;
                }
                else if (!String.IsNullOrEmpty(Estatus))                                // si no, solo contar las cuentas con estatus
                {
                    Filtro_SQL = " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Estatus + " = '" + Estatus + "'";
                }
                if (!String.IsNullOrEmpty(Excluir_Tipo_Suspension))
                {
                    if (Filtro_SQL.Length > 0)
                    {
                        Filtro_SQL += " AND NVL(" + Cat_Pre_Cuentas_Predial.Campo_Tipo_Suspencion + ",' ') " + Excluir_Tipo_Suspension;
                    }
                    else
                    {
                        Filtro_SQL += " WHERE NVL(" + Cat_Pre_Cuentas_Predial.Campo_Tipo_Suspencion + ",' ') " + Excluir_Tipo_Suspension;
                    }
                }

                Mi_SQL += Filtro_SQL;

                Total = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(Total))    // si no se obtuvo resultado de la consulta
                {
                    return (Int32)0;              // regresar 0
                }
                else
                {
                    if (Int32.TryParse(Total.ToString(), out Total_Cuentas))    //si se obtiene un valor entero de la consulta, regresar ese valor
                        return Total_Cuentas;
                    else
                        return (Int32)0;           // si no, regresar 0
                }
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

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Datos_Cuentas
        /// DESCRIPCIÓN: Consulta detalles de cuentas filtradas por estatus
        ///             Regresa un datatable con la consulta
        /// PARÁMETROS:
        /// 	1. Estatus: Lista entre comillas separada por comas de los estatus a excluir 'ACTIVA','CANCELADA'
        /// 	2. Numero_Cuenta_Predial: Si se especifica, se filtra por cuenta
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 25-jul-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Datos_Cuentas(String Estatus, String Numero_Cuenta_Predial, String Filtros_Dinamicos)
        {
            String Mi_SQL; //Variable para la consulta SQL

            try
            {
                Mi_SQL = "SELECT c.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ", c.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ", c.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal + ", c.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija + ", e.";
                Mi_SQL += Cat_Pre_Casos_Especiales.Campo_Tipo + ", t.";
                Mi_SQL += Cat_Pre_Tasas_Predial.Campo_Tasa_Predial_ID + ", c.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Tasa_ID + ", c.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo + ", c.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + ", c.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion + ", c.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual + ", c.";
                //Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Tipo_Beneficio + ", ";
                //Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Costo_m2 + ", c.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion + ", c.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion + ", s.";
                Mi_SQL += Cat_Pre_Sectores.Campo_Nombre + ", (SELECT ";
                Mi_SQL += Cat_Pre_Calles.Campo_Nombre + " FROM ";
                Mi_SQL += Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " WHERE ";
                Mi_SQL += Cat_Pre_Calles.Campo_Calle_ID + " = c." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID;
                Mi_SQL += ") NOMBRE_CALLE ";
                Mi_SQL += " FROM ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " c";
                Mi_SQL += " LEFT JOIN ";
                Mi_SQL += Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + " f";
                Mi_SQL += " ON c.";
                Mi_SQL += Ope_Pre_Cuotas_Fijas.Campo_No_Cuota_Fija + " = f." + Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija;
                Mi_SQL += " LEFT JOIN ";
                Mi_SQL += Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales + " e";
                Mi_SQL += " ON f.";
                Mi_SQL += Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID + " = e." + Ope_Pre_Cuotas_Fijas.Campo_Caso_Especial_Id;
                Mi_SQL += " LEFT JOIN ";
                Mi_SQL += Cat_Pre_Tasas_Predial.Tabla_Cat_Pre_Tasas_Predial + " t";
                Mi_SQL += " ON c.";
                Mi_SQL += Cat_Pre_Tasas_Predial.Campo_Tasa_Predial_ID + " = t." + Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID;
                Mi_SQL += " LEFT JOIN ";
                Mi_SQL += Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " o";
                Mi_SQL += " ON c.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion + " = o." + Cat_Ate_Colonias.Campo_Colonia_ID;
                Mi_SQL += " LEFT JOIN ";
                Mi_SQL += Cat_Pre_Sectores.Tabla_Cat_Pre_Sectores + " s";
                Mi_SQL += " ON o.";
                Mi_SQL += Cat_Ate_Colonias.Campo_Sector_ID + " = s." + Cat_Pre_Sectores.Campo_Sector_ID;
                Mi_SQL += " WHERE ";
                if (String.IsNullOrEmpty(Numero_Cuenta_Predial))            // si no se especifica cuenta predial
                {                                                           // filtrar por estatus
                    Mi_SQL += " c." + Cat_Pre_Cuentas_Predial.Campo_Estatus + Estatus;
                }
                else
                {                                                           // si hay cuenta, filtrar por numero de cuenta
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " = " + Numero_Cuenta_Predial + ")";
                }
                if (!String.IsNullOrEmpty(Filtros_Dinamicos))
                {
                    if (Mi_SQL.EndsWith("WHERE "))
                    {
                        Mi_SQL += Filtros_Dinamicos;
                    }
                    else
                    {
                        Mi_SQL += " AND " + Filtros_Dinamicos;
                    }
                }

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
        }   //FUNCIÓN: Consultar_Datos_Cuentas

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Cuentas_Con_Cuota_Minima
        /// DESCRIPCIÓN: Regresa un datatable con el ID de las cuentas predial que en la tabla temporal de adeudos
        ///             tengan cuota minima en el primer bimestre y sin adeudo en el segundo bimestre
        ///             y en una subconsulta, se obtiene la cuota minima de la cuenta
        /// PARÁMETROS:
        /// 	1. Cuota_Minima: Cuota minima a comparar con adeudo del primer Bimestre
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 02-dic-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Cuentas_Con_Cuota_Minima(Decimal Cuota_Minima)
        {
            String Mi_SQL; //Variable para la consulta SQL

            //Formar consulta
            Mi_SQL = "SELECT "
                + Tmp_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID
                + ", (SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual
                    + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas
                    + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID
                    + " = " + Tmp_Pre_Adeudos_Predial.Tabla_Tmp_Pre_Adeudos_Predial
                    + "." + Tmp_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID
                    + ") " + Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual
                + " FROM "
                + Tmp_Pre_Adeudos_Predial.Tabla_Tmp_Pre_Adeudos_Predial
                + " WHERE "
                + Tmp_Pre_Adeudos_Predial.Campo_Bimestre_1 + " = " + Cuota_Minima
                + " AND "
                + Tmp_Pre_Adeudos_Predial.Campo_Bimestre_2 + " = 0";


            try
            {
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
        }   //FUNCIÓN: Consultar_Tasas_Conceptos

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Cuentas_Adeudo_Menor_Cuota_Minima
        /// DESCRIPCIÓN: Regresa un datatable con el ID de las cuentas predial con una cuota anual menor a 
        ///             la cuota minima dada
        /// PARÁMETROS:
        /// 	1. Cuota_Minima: Cuota minima a tomar
        /// 	2. Estatus: filtro a aplicar para estatus
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 06-mar-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Cuentas_Adeudo_Menor_Cuota_Minima(Decimal Cuota_Minima, string Estatus)
        {
            String Mi_SQL; //Variable para la consulta SQL

            //Formar consulta
            Mi_SQL = "SELECT "
                + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID
                + "," + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID
                + " FROM "
                + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas
                + " WHERE "
                + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual + " < " + Cuota_Minima;

            // filtrar estatus
            if (!string.IsNullOrEmpty(Estatus))
            {
                Mi_SQL += " AND "
                    + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estatus + Estatus;
            }

            try
            {
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
        }   //FUNCIÓN: Consultar_Tasas_Conceptos

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Tasas_Conceptos
        /// DESCRIPCIÓN: Consulta las tasas y conceptos (incluye ID) para calculo de impuesto predial
        ///             Regresa un datatable con el resultado de la consulta
        /// PARÁMETROS:
        /// 	1. Datos: Instancia de la capa de negocio con datos para la consulta (anio y tipo de impuesto)
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 24-jul-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Tasas_Conceptos(Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta SQL

            //Formar consulta
            Mi_SQL = "SELECT ";
            Mi_SQL += "Conceptos." + Cat_Pre_Tasas_Predial.Campo_Tasa_Predial_ID + ", ";
            Mi_SQL += "Conceptos." + Cat_Pre_Tasas_Predial.Campo_Descripcion + ", ";
            Mi_SQL += "Conceptos." + Cat_Pre_Tasas_Predial.Campo_Identificador + ", ";
            Mi_SQL += "Tasas." + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Anual;
            Mi_SQL += " FROM ";
            Mi_SQL += Cat_Pre_Tasas_Predial.Tabla_Cat_Pre_Tasas_Predial + " Conceptos, ";
            Mi_SQL += Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual + " Tasas ";
            Mi_SQL += " WHERE ";
            Mi_SQL += "Conceptos." + Cat_Pre_Tasas_Predial.Campo_Tasa_Predial_ID + " = ";
            Mi_SQL += "Tasas." + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Predial_ID;
            if (Datos.p_Anio > 0)
            {
                Mi_SQL += " AND Tasas." + Cat_Pre_Tasas_Predial_Anual.Campo_Año + " = " + Datos.p_Anio;
            }
            if (!String.IsNullOrEmpty(Datos.p_Estatus))
            {
                Mi_SQL += " AND Conceptos." + Cat_Pre_Tasas_Predial.Campo_Estatus + " = " + Datos.p_Estatus;
            }
            Mi_SQL += " ORDER BY " + Cat_Pre_Tasas_Predial.Campo_Identificador;

            try
            {
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
        }   //FUNCIÓN: Consultar_Tasas_Conceptos

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Tasas_Anuales
        /// DESCRIPCIÓN: Consulta las tasas para calculo de impuesto predial
        ///             Regresa un datatable con el resultado de la consulta
        /// PARÁMETROS:
        /// 	1. Datos: Instancia de la capa de negocio con datos para la consulta (anio y tipo de impuesto)
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 14-sep-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Tasas_Anuales()
        {
            String Mi_SQL; //Variable para la consulta SQL

            //Formar consulta
            Mi_SQL = "SELECT ";
            Mi_SQL += Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_ID + ", ";
            Mi_SQL += Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Anual;
            Mi_SQL += " FROM ";
            Mi_SQL += Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual;
            Mi_SQL += " ORDER BY " + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_ID;

            try
            {
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
        }   //FUNCIÓN: Consultar_Tasas_Anuales

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Alta_Adeudos
        /// DESCRIPCIÓN: Insercion de adeudos de una cuenta utilizando conexión existente
        /// PARÁMETROS:
        /// 		1. Tabla: Nombre de la tabla a afectar (operacion o temporal)
        /// 		2. Usuario: Nombre del usuario para el campo USUARIO_CREO
        /// 		3. Estatus: Estatus de cuenta generada
        /// 		4. Cuenta_Predial_ID: ID de la cuenta predial a insertar
        /// 		5. Cuota_Anual: Valor decimal con la cuota anual
        /// 		6. Cuota_Bimestral: Arreglo de valores decimal con las cuotas bimestral
        /// 		7. Anio: PAra el adeudo
        /// 		8. No_Adeudo_Siguiente: Contador del id a insertar
        /// 		9. Cmd: Conexion a la base de datos
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 24-jul-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static Int32 Alta_Adeudos(String Tabla, String Usuario, String Estatus, String Cuenta_Predial_ID,
            Decimal Cuota_Anual, Decimal[] Cuota_Bimestral, Int32 Anio, Int32 No_Adeudo_Siguiente, OracleCommand Cmd)
        {
            String Mi_SQL;
            String No_Adeudo;
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;
            Int32 Filas_Afectadas = 0;

            // verificar numero de adeudo a insertar
            if (No_Adeudo_Siguiente > 0)
            {
                // si el numero de adeudo que llega es mayor que cero, darle formato
                No_Adeudo = String.Format("{0:0000000000}", No_Adeudo_Siguiente + 1);
            }
            else
            {
                // si no, obtener numero de la base de datos
                No_Adeudo = String.Format("{0:0000000000}", Consultar_Consecutivo_No_Adeudo(Tabla) + 1);
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

            Mi_SQL = "INSERT INTO " + Tabla + " ("
                + Ope_Pre_Adeudos_Predial.Campo_No_Adeudo + ", "
                + Ope_Pre_Adeudos_Predial.Campo_Fecha + ", "
                + Ope_Pre_Adeudos_Predial.Campo_Cuota_Anual + ", "
                + Ope_Pre_Adeudos_Predial.Campo_Bimestre_1 + ", "
                + Ope_Pre_Adeudos_Predial.Campo_Bimestre_2 + ", "
                + Ope_Pre_Adeudos_Predial.Campo_Bimestre_3 + ", "
                + Ope_Pre_Adeudos_Predial.Campo_Bimestre_4 + ", "
                + Ope_Pre_Adeudos_Predial.Campo_Bimestre_5 + ", "
                + Ope_Pre_Adeudos_Predial.Campo_Bimestre_6 + ", "
                + Ope_Pre_Adeudos_Predial.Campo_Anio + ", "
                + Ope_Pre_Adeudos_Predial.Campo_Estatus + ", "
                + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + ", "
                + Ope_Pre_Adeudos_Predial.Campo_Usuario_Creo + ", "
                + Ope_Pre_Adeudos_Predial.Campo_Fecha_Creo + ") "
                + " VALUES ('"
                + No_Adeudo + "', SYSDATE, "
                + Cuota_Anual + ", "
                + Cuota_Bimestral[0] + ", "
                + Cuota_Bimestral[1] + ", "
                + Cuota_Bimestral[2] + ", "
                + Cuota_Bimestral[3] + ", "
                + Cuota_Bimestral[4] + ", "
                + Cuota_Bimestral[5] + ", "
                + Anio + ", '"
                + Estatus + "', '"
                + Cuenta_Predial_ID + "', '"
                + Usuario + "', SYSDATE)";
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
        /// NOMBRE_FUNCIÓN: Alta_Variacion
        /// DESCRIPCIÓN: Insercion de orden de variacion despues de actualizar cuenta
        ///             regresa el número de nota asignado a la orden
        /// PARÁMETROS:
        /// 		1. Cmd: omando de oracle para transacción
        /// 		2. Cuenta_ID: ID de la cuenta predial a considerar
        /// 		3. Anio_Variacion: Año para la orden de variación
        /// 		4. Anio_Generacion: Año de la generación de adeudos (observaciones)
        /// 		5. Grupo_Movimiento_ID: id del grupo de movimiento de la orden
        /// 		6. Movimiento_ID: id del movimiento de la orden
        /// 		7. Tipo_Predio_ID: id del tipo de predio para consultar el número de nota 
        /// 		8. Cuota_Anual: Cuota anual que se va a ingresar
        /// 		9. Usuario: usuario que modifica
        /// 		10. Ultimo_Numero_Nota: Número de nota que se va a utilizar para la orden de variación si se proporciona
        /// 		11. Ultimo_Numero_Orden: Número de la última orden de variación
        /// 		12. out Numero_Orden_Generada: Número de la orden de variación generada
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 24-jul-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static Int32 Alta_Variacion(
            OracleCommand Cmd,
            string Cuenta_ID,
            int Anio_Variacion,
            int Anio_Generacion,
            string Grupo_Movimiento_ID,
            string Movimiento_ID,
            string Tipo_Predio_ID,
            decimal Cuota_Anual,
            string Usuario,
            Int32 Ultimo_Numero_Nota,
            Int32 Ultimo_Numero_Orden,
            out Int32 Numero_Orden_Generada
            )
        {
            String Mi_SQL;
            int Numero_Nota;
            int Numero_Orden;
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;
            Object Obj_Temp;

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

            try
            {
                // si en los parámetros se indicó un número de nota, tomarlo como el siguiente
                if (Ultimo_Numero_Nota > 0)
                {
                    Numero_Nota = Ultimo_Numero_Nota + 1;
                }
                else
                {
                    // obtener el numero de nota para el movimiento
                    Mi_SQL = "SELECT NVL(MAX(" + Ope_Pre_Ordenes_Variacion.Campo_No_Nota
                        + "), 0) + 1"
                        + " FROM " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion
                        + " WHERE " + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = " + Anio_Variacion
                        + " AND " + Ope_Pre_Ordenes_Variacion.Campo_Grupo_Movimiento_ID + " = '" + Grupo_Movimiento_ID + "'"
                        + " AND " + Ope_Pre_Ordenes_Variacion.Campo_Tipo_Predio_ID + " = '" + Tipo_Predio_ID + "'";

                    Comando.CommandType = CommandType.Text;
                    Comando.CommandText = Mi_SQL;
                    Obj_Temp = Comando.ExecuteScalar();
                    int.TryParse(Obj_Temp.ToString(), out Numero_Nota);
                }

                // si se proporciona un último número de orden, tomar para siguiente orde, si no, consultar
                if (Ultimo_Numero_Orden > 0)
                {
                    Numero_Orden = Ultimo_Numero_Orden + 1;
                }
                else
                {
                    // obtener el siguiente numero de orden de variacion a insertar
                    Mi_SQL = "SELECT NVL(MAX(" + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion
                        + "), 0) + 1"
                        + " FROM " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion
                        + " WHERE " + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = " + Anio_Variacion.ToString();

                    Comando.CommandText = Mi_SQL;
                    Obj_Temp = Comando.ExecuteScalar();
                    int.TryParse(Obj_Temp.ToString(), out Numero_Orden);
                }

                // actualizar cuota anual de la cuenta predial
                Mi_SQL = "UPDATE " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " SET "
                    + Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual + " = " + Cuota_Anual.ToString("0.00") + ", "
                    + Ope_Pre_Adeudos_Predial.Campo_Usuario_Modifico + " = '" + Usuario + "', "
                    + Ope_Pre_Adeudos_Predial.Campo_Fecha_Modifico + " = SYSDATE"
                    + " WHERE "
                    + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = '"
                    + Cuenta_ID + "'";
                Comando.CommandText = Mi_SQL;
                Comando.ExecuteNonQuery();

                // insertar orden variacion
                Mi_SQL = "INSERT INTO " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " ("
                    + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Anio + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Tipo_Predio_ID + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Estado_Predio_ID + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Tasa_ID + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Tasa_Predial_ID + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Uso_Suelo_ID + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Cuota_Minima_ID + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Grupo_Movimiento_ID + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_No_Cuota_Fija + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Origen + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Clave_Catastral + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Cuenta + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Valor_Fiscal + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Porcentaje_Exencion + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Superficie_Construida + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Superficie_Total + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Termino_Exencion + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Avaluo + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Domicilio_Foraneo + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Calle_ID + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Calle_ID_Notificacion + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Calle_Notificacion + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Colonia_ID + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Colonia_ID_Notificacion + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Colonia_Notificacion + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Ciudad_ID_Notificacion + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Ciudad_Notificacion + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Estado_ID_Notificacion + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Estado_Notificacion + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_No_Exterior + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_No_Exterior_Notificacion + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_No_Interior + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_No_Interior_Notificacion + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Codigo_Postal + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Efectos + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Periodo_Corriente + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Costo_M2 + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Cuota_Anual + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Cuota_Fija + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Diferencia_Construccion + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Tipo_Suspencion + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_No_Nota + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Nota + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Numero_Nota_Impreso + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Observaciones + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Usuario_Creo + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Usuario_Modifico + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Modifico + ") ";
                Mi_SQL += " SELECT '" + Numero_Orden.ToString().PadLeft(10, '0') + "',"
                    + Anio_Variacion.ToString() + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Tasa_ID + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID + ", '"
                    + Grupo_Movimiento_ID + "', '"
                    + Movimiento_ID + "', "
                    + "NULL,"
                    + Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral + ", "
                    + "'ACEPTADA',"
                    + Cat_Pre_Cuentas_Predial.Campo_Estatus + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Superficie_Total + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Ciudad_Notificacion + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Estado_Notificacion + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_No_Interior + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Codigo_Postal + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Efectos + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Costo_m2 + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion + ", "
                    + Cat_Pre_Cuentas_Predial.Campo_Tipo_Suspencion + ", "
                    + Numero_Nota.ToString() + ", "
                    + "SYSDATE,"
                    + "'NO',"
                    + "'GENERACION DE ADEUDOS " + Anio_Generacion.ToString()
                    + "','" + Usuario + "'," + "SYSDATE,'" + Usuario + "'," + "SYSDATE"
                    + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas
                    + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "= '" + Cuenta_ID + "'";
                Comando.CommandText = Mi_SQL;
                Comando.ExecuteNonQuery();

                if (Cmd == null)    // si la conexion no llego como parametro, aplicar consultas
                {
                    Transaccion.Commit();
                }

                Numero_Orden_Generada = Numero_Orden;
                return Numero_Nota;
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
        /// NOMBRE_FUNCIÓN: Aplicacion_Adeudos_Proyeccion
        /// DESCRIPCIÓN: Insercion de adeudos generados en la tabla TMP_PRE_ADEUDOS_PREDIAL a OPE_PRE_ADEUDOS_PREDIAL
        /// PARÁMETROS:
        /// 		1. Cmd: Conexion a la base de datos
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 02-dic-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static Int32 Aplicacion_Adeudos_Proyeccion(OracleCommand Cmd)
        {
            String Mi_SQL;
            Int32 No_Adeudo;
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;
            Int32 Filas_Afectadas = 0;

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

            try
            {

                Comando.CommandText = "SELECT NVL(MAX(" + Ope_Pre_Adeudos_Predial.Campo_No_Adeudo
                    + "), 0) FROM " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial;
                Int32.TryParse(Comando.ExecuteScalar().ToString(), out No_Adeudo);

                Mi_SQL = "INSERT INTO " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + " ("
                    + Ope_Pre_Adeudos_Predial.Campo_No_Adeudo + ", "
                    + Ope_Pre_Adeudos_Predial.Campo_Fecha + ", "
                    + Ope_Pre_Adeudos_Predial.Campo_Cuota_Anual + ", "
                    + Ope_Pre_Adeudos_Predial.Campo_Bimestre_1 + ", "
                    + Ope_Pre_Adeudos_Predial.Campo_Bimestre_2 + ", "
                    + Ope_Pre_Adeudos_Predial.Campo_Bimestre_3 + ", "
                    + Ope_Pre_Adeudos_Predial.Campo_Bimestre_4 + ", "
                    + Ope_Pre_Adeudos_Predial.Campo_Bimestre_5 + ", "
                    + Ope_Pre_Adeudos_Predial.Campo_Bimestre_6 + ", "
                    + Ope_Pre_Adeudos_Predial.Campo_Anio + ", "
                    + Ope_Pre_Adeudos_Predial.Campo_Estatus + ", "
                    + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + ", "
                    + Ope_Pre_Adeudos_Predial.Campo_Usuario_Creo + ", "
                    + Ope_Pre_Adeudos_Predial.Campo_Fecha_Creo + ") "
                    + " SELECT SUBSTR(TO_CHAR(TO_NUMBER(" + Ope_Pre_Adeudos_Predial.Campo_No_Adeudo + ") + " + No_Adeudo + " , '0000000000'), - 10)"
                    + " ," + Tmp_Pre_Adeudos_Predial.Campo_Fecha
                    + " ," + Tmp_Pre_Adeudos_Predial.Campo_Cuota_Anual
                    + " ," + Tmp_Pre_Adeudos_Predial.Campo_Bimestre_1
                    + " ," + Tmp_Pre_Adeudos_Predial.Campo_Bimestre_2
                    + " ," + Tmp_Pre_Adeudos_Predial.Campo_Bimestre_3
                    + " ," + Tmp_Pre_Adeudos_Predial.Campo_Bimestre_4
                    + " ," + Tmp_Pre_Adeudos_Predial.Campo_Bimestre_5
                    + " ," + Tmp_Pre_Adeudos_Predial.Campo_Bimestre_6
                    + " ," + Tmp_Pre_Adeudos_Predial.Campo_Anio
                    + " ," + Tmp_Pre_Adeudos_Predial.Campo_Estatus
                    + " ," + Tmp_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID
                    + " ," + Tmp_Pre_Adeudos_Predial.Campo_Usuario_Creo
                    + " ," + Tmp_Pre_Adeudos_Predial.Campo_Fecha_Creo
                    + " FROM " + Tmp_Pre_Adeudos_Predial.Tabla_Tmp_Pre_Adeudos_Predial;

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
        /// NOMBRE_FUNCIÓN: Alta_Lineas_Captura
        /// DESCRIPCIÓN: Insercion de líneas de captura, regresa el número de inserciones realizadas y elimina por año y tipo de predio id
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 15-mar-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static Int32 Alta_Lineas_Captura(int Anio, string Tipo_Predio_ID, DataTable Dt_Lineas_Captura, string Usuario)
        {
            String Mi_SQL;
            Int32 No_Linea_Captura;
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;
            Int32 Filas_Afectadas = 0;

            Conexion.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Conexion.Open();
            Transaccion = Conexion.BeginTransaction();
            Comando.Connection = Conexion;
            Comando.Transaction = Transaccion;

            try
            {
                // si se especifico Anio y tipo de predio, eliminar
                if (Anio > 0 && !string.IsNullOrEmpty(Tipo_Predio_ID))
                {
                    // Consulta para eliminar todos los adeudos de la tabla
                    Mi_SQL = "DELETE FROM " + Ope_Pre_Lineas_Captura.Tabla_Ope_Pre_Lineas_Captura
                        + " WHERE " + Ope_Pre_Lineas_Captura.Campo_Anio + " = '" + Anio.ToString() + "'"
                        + " AND " + Ope_Pre_Lineas_Captura.Campo_Tipo_Predio_ID + " = '" + Tipo_Predio_ID + "'";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }

                Comando.CommandText = "SELECT NVL(MAX(" + Ope_Pre_Lineas_Captura.Campo_No_Linea_Captura
                    + "), 0) FROM " + Ope_Pre_Lineas_Captura.Tabla_Ope_Pre_Lineas_Captura;
                Int32.TryParse(Comando.ExecuteScalar().ToString(), out No_Linea_Captura);
                for (int Linea = 0; Linea < Dt_Lineas_Captura.Rows.Count; Linea++)
                {
                    Mi_SQL = "INSERT INTO " + Ope_Pre_Lineas_Captura.Tabla_Ope_Pre_Lineas_Captura + " ("
                        + Ope_Pre_Lineas_Captura.Campo_No_Linea_Captura + ", "
                        + Ope_Pre_Lineas_Captura.Campo_Cuenta_Predial_Id + ", "
                        + Ope_Pre_Lineas_Captura.Campo_Institucion_Id + ", "
                        + Ope_Pre_Lineas_Captura.Campo_Tipo_Predio_ID + ", "
                        + Ope_Pre_Lineas_Captura.Campo_Anio + ", "
                        + Ope_Pre_Lineas_Captura.Campo_Linea_Captura_Enero + ", "
                        + Ope_Pre_Lineas_Captura.Campo_Linea_Captura_Febrero + ", "
                        + Ope_Pre_Adeudos_Predial.Campo_Usuario_Creo + ", "
                        + Ope_Pre_Adeudos_Predial.Campo_Fecha_Creo + ") "
                        + " VALUES('" + (++No_Linea_Captura).ToString().PadLeft(10, '0')
                        + "' ,'" + Dt_Lineas_Captura.Rows[Linea]["CUENTA_PREDIAL_ID"].ToString()
                        + "' ,'" + Dt_Lineas_Captura.Rows[Linea]["INSTITUCION_ID"].ToString()
                        + "' ,'" + Dt_Lineas_Captura.Rows[Linea]["TIPO_PREDIO_ID"].ToString()
                        + "' ,'" + Dt_Lineas_Captura.Rows[Linea]["ANIO"].ToString()
                        + "' ,'" + Dt_Lineas_Captura.Rows[Linea]["LINEA_CAPTURA_ENERO"].ToString()
                        + "' ,'" + Dt_Lineas_Captura.Rows[Linea]["LINEA_CAPTURA_FEBRERO"].ToString()
                        + "' ,'" + Usuario
                        + "' ,SYSDATE)";

                    Comando.CommandText = Mi_SQL;
                    Filas_Afectadas = Comando.ExecuteNonQuery();
                }
                Transaccion.Commit();

                return Filas_Afectadas;
            }
            catch (OracleException Ex)
            {
                Transaccion.Rollback();
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                Transaccion.Rollback();
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                Transaccion.Rollback();
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                Conexion.Close();
            }
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Alta_Recargos
        /// DESCRIPCIÓN: Alta o modificación de Recargos en la tabla de recargos utilizando una tranasacción existente
        /// PARÁMETROS:
        /// 	    1. Usuario: Nombre del usuario para el campo USUARIO_CREO o USUARIO_MODIFICO
        /// 		2. Estatus: Estatus de adeudo a generar
        /// 		3. Cuenta_Predial_ID: ID de la cuenta predial a la que se agrega el recargo
        /// 		4. Anio: Anio en que se calculan recargos
        /// 		5. Bimestre_Final: hasta que periodo se calculan los recargos
        /// 		6. Monto: Valor decimal con el monto total de recargos calculados
        /// 		7. Tipo: Tipo de recargo ORDINARIO o MORATORIO
        /// 		8. Rezago: Monto de rezago sobre el que se calcularon recargos
        /// 		9. Cmd: Conexion a la base de datos
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 05-ago-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static Int32 Alta_Recargos(String Usuario, String Estatus,
            String Cuenta_Predial_ID, Int32 Anio,
            Int32 Bimestre_Final, Decimal Monto,
            String Tipo, Decimal Rezago, OracleCommand Cmd)
        {
            String Mi_SQL = "";
            String No_Adeudo = "";
            String No_Adeudo_Actualiza = "";
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;
            Int32 Filas_Afectadas = 0;

            // buscar recargos pendientes de pago
            No_Adeudo_Actualiza = Consultar_No_Recargo_Sin_Pagar(Cuenta_Predial_ID, "POR PAGAR");
            if (No_Adeudo_Actualiza == "")
            {
                // si no existe, obtener numero de la base de datos
                No_Adeudo = String.Format("{0:0000000000}", Consultar_Consecutivo_No_Recargos());
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
            if (!String.IsNullOrEmpty(No_Adeudo_Actualiza))
            {
                Mi_SQL = "UPDATE " + Ope_Pre_Recargos.Tabla_Ope_Pre_Recargos + " SET "
                    + Ope_Pre_Recargos.Campo_Anio + " = " + Anio + ", "
                    + Ope_Pre_Recargos.Campo_Bimestre_Final + " =  " + Bimestre_Final + ", "
                    + Ope_Pre_Recargos.Campo_Monto + " = " + Monto + ", "
                    + Ope_Pre_Recargos.Campo_Tipo + " = '" + Tipo + "', "
                    + Ope_Pre_Recargos.Campo_Rezago + " = '" + Rezago + "', "
                    + Ope_Pre_Adeudos_Predial.Campo_Estatus + " = '" + Estatus + "', "
                    + Ope_Pre_Adeudos_Predial.Campo_Usuario_Modifico + " = '" + Usuario + "', "
                    + Ope_Pre_Adeudos_Predial.Campo_Fecha_Modifico + " = SYSDATE"
                    + " WHERE "
                    + Ope_Pre_Recargos.Campo_No_Recargo + " = "
                    + No_Adeudo_Actualiza;
            }
            else if (!String.IsNullOrEmpty(No_Adeudo))
            {
                Mi_SQL = "INSERT INTO " + Ope_Pre_Recargos.Tabla_Ope_Pre_Recargos + " ("
                    + Ope_Pre_Recargos.Campo_No_Recargo + ", "
                    + Ope_Pre_Recargos.Campo_Cuenta_Predial_ID + ", "
                    + Ope_Pre_Recargos.Campo_Anio + ", "
                    + Ope_Pre_Recargos.Campo_Bimestre_Final + ", "
                    + Ope_Pre_Recargos.Campo_Monto + ", "
                    + Ope_Pre_Recargos.Campo_Tipo + ", "
                    + Ope_Pre_Recargos.Campo_Rezago + ", "
                    + Ope_Pre_Adeudos_Predial.Campo_Estatus + ", "
                    + Ope_Pre_Adeudos_Predial.Campo_Usuario_Creo + ", "
                    + Ope_Pre_Adeudos_Predial.Campo_Fecha_Creo + ") "
                    + " VALUES ('"
                    + No_Adeudo + "', '"
                    + Cuenta_Predial_ID + "', "
                    + Anio + ", '"
                    + Bimestre_Final + "', '"
                    + Monto + "', '"
                    + Tipo + "', '"
                    + Rezago + "', '"
                    + Estatus + "', '"
                    + Usuario + "', SYSDATE)";
            }

            try
            {
                if (!String.IsNullOrEmpty(Mi_SQL))
                {
                    Comando.CommandText = Mi_SQL;
                    Filas_Afectadas = Comando.ExecuteNonQuery();

                    if (Cmd == null)    // si la conexion no llego como parametro, aplicar consultas
                    {
                        Transaccion.Commit();
                    }
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
        /// NOMBRE_FUNCIÓN: Eliminar_Adeudos_Temporales
        /// DESCRIPCIÓN: Elimina todos los registros de la tabla 
        /// PARÁMETROS:
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 25-jul-2011 
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static void Eliminar_Adeudos_Temporales()
        {
            String Mi_SQL; //Obtiene la cadena de modificación hacía la base de datos

            try
            {
                // Consulta para eliminar todos los adeudos de la tabla
                Mi_SQL = "DELETE FROM " + Tmp_Pre_Adeudos_Predial.Tabla_Tmp_Pre_Adeudos_Predial;

                // regresar el número de filas modificadas
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
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

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Eliminar_Adeudos_Predial
        /// DESCRIPCIÓN: Elimina todos los registros de la tabla de adeudos predial para el año especificado
        /// PARÁMETROS:
        /// 		1. Anio: Año de adeudos a eliminar
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 18-oct-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static int Eliminar_Adeudos_Predial(String Anio)
        {
            String Mi_SQL; //Obtiene la cadena de modificación hacía la base de datos

            try
            {
                // validar que el campo anio no viene vacio
                if (!String.IsNullOrEmpty(Anio))
                {
                    // Consulta para eliminar todos los adeudos de la tabla
                    Mi_SQL = "DELETE FROM " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial
                        + " WHERE " + Ope_Pre_Adeudos_Predial.Campo_Anio + " = " + Anio;

                    // regresar el número de filas modificadas
                    return OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                else
                {
                    return 0;
                }
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

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Consecutivo_No_Adeudo
        /// DESCRIPCIÓN: Consulta el valor maximo del consecutivo de adeudos NO_ADEUDO
        ///             Regresa entero con este valor
        /// PARÁMETROS:
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 26-jul-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static Int32 Consultar_Consecutivo_No_Adeudo(String Tabla)
        {
            String Mi_SQL;
            Object No_Adeudo;
            // consulta para obtener el último registro insertado
            Mi_SQL = "SELECT NVL(MAX(" + Ope_Pre_Adeudos_Predial.Campo_No_Adeudo + "), 0) ";
            Mi_SQL += "FROM " + Tabla;

            try
            {
                No_Adeudo = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                // verificar valor o asignar por defecto
                if (Convert.IsDBNull(No_Adeudo) || No_Adeudo == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(No_Adeudo);
                }
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
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Consecutivo_No_Recargo
        /// DESCRIPCIÓN: Consulta el valor maximo del consecutivo de recargos No_Recargo
        ///             Regresa entero con este valor
        /// PARÁMETROS:
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 05-ago-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static Int32 Consultar_Consecutivo_No_Recargos()
        {
            String Mi_SQL;
            Object No_Recargo;
            // consulta para obtener el último registro insertado
            Mi_SQL = "SELECT NVL(MAX(" + Ope_Pre_Recargos.Campo_No_Recargo + "),'0000000000') ";
            Mi_SQL += "FROM " + Ope_Pre_Recargos.Tabla_Ope_Pre_Recargos;

            try
            {
                // si la variable global contiene valor, tomar ese valor en lugar de consultar
                if (Contador_Insercion <= 0)
                {
                    No_Recargo = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                    // verificar valor o asignar por defecto
                    if (Convert.IsDBNull(No_Recargo) || No_Recargo == null)
                    {
                        Contador_Insercion = 1;
                        return 1;
                    }
                    else
                    {
                        Contador_Insercion = Convert.ToInt32(No_Recargo);
                        return ++Contador_Insercion;
                    }
                }
                else
                {
                    return ++Contador_Insercion;
                }
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
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_No_Recargo_Sin_Pagar
        /// DESCRIPCIÓN: Consulta los recargos en busca por numero de cuenta filtrando por estatus
        ///             Regresa No_Recargo si lo encuentra
        /// PARÁMETROS:
        ///             1. Cuenta_Predial_ID: número de cuenta a buscar
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 05-ago-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static String Consultar_No_Recargo_Sin_Pagar(String Cuenta_Predial_ID, String Estatus)
        {
            String Mi_SQL;
            String Filtro_SQL = "";
            Object No_Recargo;
            // consulta para obtener el último registro insertado
            Mi_SQL = "SELECT " + Ope_Pre_Recargos.Campo_No_Recargo + " ";
            Mi_SQL += "FROM " + Ope_Pre_Recargos.Tabla_Ope_Pre_Recargos;
            if (!String.IsNullOrEmpty(Cuenta_Predial_ID))
            {
                Filtro_SQL += " WHERE " + Ope_Pre_Recargos.Campo_Cuenta_Predial_ID;
                Filtro_SQL += " = '" + Cuenta_Predial_ID + "'";
            }
            if (!String.IsNullOrEmpty(Estatus))
            {
                if (Filtro_SQL == "")
                {
                    Filtro_SQL += " WHERE " + Ope_Pre_Recargos.Campo_Estatus + " = '";
                    Filtro_SQL += Estatus + "'";
                }
                else
                {
                    Filtro_SQL += " AND " + Ope_Pre_Recargos.Campo_Estatus + " = '";
                    Filtro_SQL += Estatus + "'";
                }
            }
            Mi_SQL += Filtro_SQL;
            try
            {
                No_Recargo = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                // verificar valor o asignar por defecto
                if (Convert.IsDBNull(No_Recargo) || No_Recargo == null)
                {
                    return String.Empty;
                }
                else
                {
                    return No_Recargo.ToString();
                }
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
        }


        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Adeudos
        /// DESCRIPCIÓN: Consulta el total de adeudos generados
        /// PARÁMETROS:
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 26-jul-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static Int32 Consultar_Adeudos(String Tabla)
        {
            String Mi_SQL;
            Object No_Adeudo;
            // consulta para obtener el último registro insertado
            Mi_SQL = "SELECT " + Ope_Pre_Adeudos_Predial.Campo_No_Adeudo + ", ";
            Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_No_Adeudo + ", ";
            Mi_SQL += "FROM " + Tabla;

            try
            {
                No_Adeudo = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                // verificar valor o asignar por defecto
                if (Convert.IsDBNull(No_Adeudo) || No_Adeudo == null)
                {
                    return 1;
                }
                else
                {
                    return Convert.ToInt32(No_Adeudo);
                }
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
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Total_Adeudos
        /// DESCRIPCIÓN: Consulta el total de adeudos generados
        /// PARÁMETROS:
        /// 	1. Tabla: Tabla que se va a consultar (adeudos predial o temporal)
        /// 	2. Estatus: Filtro por estatus de la cuenta
        /// 	3. Fecha_Adeudo_Desde: Adeudos mayores a esta fecha
        /// 	4. Fecha_Adeudo_Hasta: Adeudos menores a esta fecha
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 25-jul-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static Int32 Consultar_Total_Adeudos(String Tabla, String Estatus, String Fecha_Adeudo_Desde, String Fecha_Adeudo_Hasta)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL
            String Filtros_SQL = "";
            Int32 Total_Adeudos;
            object Total;

            try
            {
                Mi_SQL = "SELECT COUNT(" + Ope_Pre_Adeudos_Predial.Campo_No_Adeudo;
                Mi_SQL += ") FROM " + Tabla;
                if (!String.IsNullOrEmpty(Estatus))   // filtrar desde fecha si llego parametro
                {
                    Filtros_SQL += " WHERE " + Ope_Pre_Adeudos_Predial.Campo_Estatus + " LIKE '" + Estatus + "'";
                }
                if (!String.IsNullOrEmpty(Fecha_Adeudo_Desde))   // filtrar desde fecha si llego parametro
                {
                    if (Filtros_SQL.Length == 0)        // si no hay filtro, agregar nuevos filtros con WHERE
                    {
                        Filtros_SQL += " WHERE TO_DATE(TO_CHAR(" + Ope_Pre_Adeudos_Predial.Campo_Fecha + ",'DD/MM/YY'))";
                        Filtros_SQL += " >= '" + Fecha_Adeudo_Desde + "' ";
                    }
                    else
                    {
                        Filtros_SQL += " AND TO_DATE(TO_CHAR(" + Ope_Pre_Adeudos_Predial.Campo_Fecha + ",'DD/MM/YY'))";
                        Filtros_SQL += " >= '" + Fecha_Adeudo_Desde + "' ";
                    }
                }
                if (!String.IsNullOrEmpty(Fecha_Adeudo_Hasta))   // filtrar hasta fehca, si se recibio fecha final
                {
                    if (Filtros_SQL.Length == 0)        // si no hay filtro, agregar nuevos filtros con WHERE
                    {
                        Filtros_SQL += " WHERE TO_DATE(TO_CHAR(" + Ope_Pre_Adeudos_Predial.Campo_Fecha + ",'DD/MM/YY'))";
                        Filtros_SQL += " <= '" + Fecha_Adeudo_Hasta + "' ";
                    }
                    else                // agregar filtros con AND
                    {
                        Filtros_SQL += " AND TO_DATE(TO_CHAR(" + Ope_Pre_Adeudos_Predial.Campo_Fecha + ",'DD/MM/YY'))";
                        Filtros_SQL += " <= '" + Fecha_Adeudo_Hasta + "' ";
                    }
                }
                Mi_SQL += Filtros_SQL;

                Total = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(Total) || Total == null)    // si no se obtuvo resultado de la consulta
                {
                    return (Int32)0;              // regresar 0
                }
                else
                {
                    if (Int32.TryParse(Total.ToString(), out Total_Adeudos))    //si se obtiene un valor entero de la consulta, regresar ese valor
                        return Total_Adeudos;
                    else
                        return (Int32)0;           // si no, regresar 0
                }
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
        }   //FUNCIÓN: Consultar_Total_Adeudos_Generados

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Monto_Adeudos_Generados
        /// DESCRIPCIÓN: Consulta el monto total de adeudos generados por cuota anual y bimestres
        /// PARÁMETROS:
        /// 	1. Tabla: Tabla que se va a consultar (adeudos predial o temporal)
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 25-jul-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static System.Data.DataTable Consultar_Monto_Adeudos_Generados(String Tabla, String Fecha_Adeudo_Desde, String Fecha_Adeudo_Hasta)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL
            String Filtros_SQL = "";
            DataTable Dt_Adeudos;

            try
            {
                Mi_SQL = "SELECT"
                + " SUM (" + Ope_Pre_Adeudos_Predial.Campo_Cuota_Anual
                + ") AS " + Ope_Pre_Adeudos_Predial.Campo_Cuota_Anual + ", "
                + " SUM (" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_1
                + ") AS " + Ope_Pre_Adeudos_Predial.Campo_Bimestre_1 + ", "
                + " SUM (" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_2
                + ") AS " + Ope_Pre_Adeudos_Predial.Campo_Bimestre_2 + ", "
                + " SUM (" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_3
                + ") AS " + Ope_Pre_Adeudos_Predial.Campo_Bimestre_3 + ", "
                + " SUM (" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_4
                + ") AS " + Ope_Pre_Adeudos_Predial.Campo_Bimestre_4 + ", "
                + " SUM (" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_5
                + ") AS " + Ope_Pre_Adeudos_Predial.Campo_Bimestre_5 + ", "
                + " SUM (" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_6
                + ") AS " + Ope_Pre_Adeudos_Predial.Campo_Bimestre_6
                + " FROM " + Tabla;
                if (!String.IsNullOrEmpty(Fecha_Adeudo_Desde))   // filtrar desde fecha si llego parametro
                {
                    Filtros_SQL += " WHERE TO_DATE(TO_CHAR(" + Ope_Pre_Adeudos_Predial.Campo_Fecha + ",'DD/MM/YY'))";
                    Filtros_SQL += " >= '" + Fecha_Adeudo_Desde + "' ";
                }
                if (!String.IsNullOrEmpty(Fecha_Adeudo_Hasta))   // filtrar hasta fehca, si se recibio fecha final
                {
                    if (Filtros_SQL.Length == 0)        // si no hay filtro, agregar nuevos filtros con WHERE
                    {
                        Filtros_SQL += " WHERE TO_DATE(TO_CHAR(" + Ope_Pre_Adeudos_Predial.Campo_Fecha + ",'DD/MM/YY'))";
                        Filtros_SQL += " <= '" + Fecha_Adeudo_Hasta + "' ";
                    }
                    else                // agregar filtros con AND
                    {
                        Filtros_SQL += " AND TO_DATE(TO_CHAR(" + Ope_Pre_Adeudos_Predial.Campo_Fecha + ",'DD/MM/YY'))";
                        Filtros_SQL += " <= '" + Fecha_Adeudo_Hasta + "'";
                    }
                }
                Mi_SQL += Filtros_SQL;

                Dt_Adeudos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                if (Dt_Adeudos.Rows.Count > 0)      // Si la consulta arrojo resultados, regresar el datatable con los resultados
                {
                    return Dt_Adeudos;
                }
                return null;
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
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Cuentas_Exencion_Vigente
        /// DESCRIPCIÓN: Consulta las cuentas con exencion vigente
        /// PARÁMETROS:
        /// 	1. Estatus: Tabla que se va a consultar (adeudos predial o temporal)
        /// 	2. Fecha_Vigencia: Fecha de vigencia a consultar
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 25-jul-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Cuentas_Exencion_Vigente(String Estatus, String Fecha_Vigencia)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL

            try
            {
                Mi_SQL = "SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion;
                Mi_SQL += " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas;
                Mi_SQL += " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Estatus + " LIKE '" + Estatus + "'";
                Mi_SQL += " AND TO_DATE(TO_CHAR(" + Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion + ",'DD/MM/YY'))";
                Mi_SQL += " <= '" + Fecha_Vigencia + "' ";

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
        }   //FUNCIÓN: Consultar_Cuentas_Exencion_Vigente

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Adeudos_Cuenta_Predial
        /// DESCRIPCIÓN: Consulta los adeudos de una cuenta predial dada (regresa datatable con adeudos)
        /// PARÁMETROS:
        /// 	1. Cuenta_Predial: Cuenta predial de la que se obtendrán los adeudos
        /// 	2. Estatus: si se especifica, se agrega filtro a la contulta
        /// 	3. Desde_Anio: si se especifica, se agrega filtro a la contulta
        /// 	4. Hasta_Anio: si se especifica, se agrega filtro a la contulta
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 25-jul-2011
        /// MODIFICÓ: Roberto González Oseguera
        /// FECHA_MODIFICÓ: 29-mar-2012
        /// CAUSA_MODIFICACIÓN: Se quita el agrupamiento por ANIO y se agrega el campo NO_ADEUDO a la conulta
        ///*******************************************************************************************************
        public static DataTable Consultar_Adeudos_Cuenta_Predial(String Cuenta_Predial, String Estatus, Int32 Desde_Anio, Int32 Hasta_Anio)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL
            String Filtros_SQL = "";

            try
            {
                Mi_SQL = "WITH ADEUDOS AS (SELECT "
                + Ope_Pre_Adeudos_Predial.Campo_No_Adeudo + ","
                + Ope_Pre_Adeudos_Predial.Campo_Anio + ", NVL("
                + Ope_Pre_Adeudos_Predial.Campo_Bimestre_1 + ",0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_1 + ",0)  ADEUDO_BIMESTRE_1, NVL("
                + Ope_Pre_Adeudos_Predial.Campo_Bimestre_2 + ",0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_2 + ",0)  ADEUDO_BIMESTRE_2, NVL("
                + Ope_Pre_Adeudos_Predial.Campo_Bimestre_3 + ",0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_3 + ",0)  ADEUDO_BIMESTRE_3, NVL("
                + Ope_Pre_Adeudos_Predial.Campo_Bimestre_4 + ",0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_4 + ",0)  ADEUDO_BIMESTRE_4, NVL("
                + Ope_Pre_Adeudos_Predial.Campo_Bimestre_5 + ",0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_5 + ",0)  ADEUDO_BIMESTRE_5, NVL("
                + Ope_Pre_Adeudos_Predial.Campo_Bimestre_6 + ",0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_6 + ",0)  ADEUDO_BIMESTRE_6"
                + " FROM " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial;
                if (!String.IsNullOrEmpty(Estatus))   // filtrar desde fecha si llego parametro
                {
                    Filtros_SQL += " WHERE " + Ope_Pre_Adeudos_Predial.Campo_Estatus + " LIKE '" + Estatus + "'";
                }
                if (!String.IsNullOrEmpty(Cuenta_Predial))   // filtrar por cuenta predial si llego como parametro
                {
                    if (Filtros_SQL.Length == 0)        // si no hay filtro, agregar nuevos filtros con WHERE
                    {
                        Filtros_SQL += " WHERE " + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID;
                        Filtros_SQL += " = '" + Cuenta_Predial + "'";
                    }
                    else
                    {
                        Filtros_SQL += " AND " + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID;
                        Filtros_SQL += " = '" + Cuenta_Predial + "'";
                    }
                }
                if (Desde_Anio > 0)   // filtrar desde anio si llego parametro
                {
                    if (Filtros_SQL.Length == 0)        // si no hay filtro, agregar nuevos filtros con WHERE
                    {
                        Filtros_SQL += " WHERE " + Ope_Pre_Adeudos_Predial.Campo_Anio;
                        Filtros_SQL += " >= " + Desde_Anio;
                    }
                    else
                    {
                        Filtros_SQL += " AND " + Ope_Pre_Adeudos_Predial.Campo_Anio;
                        Filtros_SQL += " >= " + Desde_Anio;
                    }
                }
                if (Hasta_Anio > 0)   // filtrar desde anio si llego parametro
                {
                    if (Filtros_SQL.Length == 0)        // si no hay filtro, agregar nuevos filtros con WHERE
                    {
                        Filtros_SQL += " WHERE " + Ope_Pre_Adeudos_Predial.Campo_Anio;
                        Filtros_SQL += " <= " + Hasta_Anio;
                    }
                    else
                    {
                        Filtros_SQL += " AND " + Ope_Pre_Adeudos_Predial.Campo_Anio;
                        Filtros_SQL += " <= " + Hasta_Anio;
                    }
                }

                Filtros_SQL += " ORDER BY " + Ope_Pre_Adeudos_Predial.Campo_Anio;
                Mi_SQL += Filtros_SQL
                    + " ) SELECT * FROM ADEUDOS "
                    + " WHERE (ADEUDO_BIMESTRE_1 + ADEUDO_BIMESTRE_2 + ADEUDO_BIMESTRE_3 + ADEUDO_BIMESTRE_4 + ADEUDO_BIMESTRE_5 + ADEUDO_BIMESTRE_6) <> 0 ";

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
        }   //FUNCIÓN: Consultar_Adeudos_Cuenta_Predial

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Adeudos_Cuenta_Predial_Con_Totales
        /// DESCRIPCIÓN: Consulta los adeudos de una cuenta predial dada (regresa datatable con adeudos), 
        ///             incluye un campo con el total por anio
        /// PARÁMETROS:
        /// 	1. Cuenta_Predial: Cuenta predial de la que se obtendrán los adeudos
        /// 	2. Estatus: si se especifica, se agrega filtro a la contulta
        /// 	3. Desde_Anio: si se especifica, se agrega filtro a la contulta
        /// 	4. Hasta_Anio: si se especifica, se agrega filtro a la contulta
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 25-sep-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Adeudos_Cuenta_Predial_Con_Totales(String Cuenta_Predial, String Estatus, Int32 Desde_Anio, Int32 Hasta_Anio)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL
            String Filtros_SQL = "";

            try
            {
                Mi_SQL = "WITH ADEUDOS AS ( SELECT ";
                //Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_No_Adeudo + ", ";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Anio + ", SUM(NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_1 + ",0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_1 + ",0)) ADEUDO_BIMESTRE_1, SUM(NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_2 + ",0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_2 + ",0))  ADEUDO_BIMESTRE_2, SUM(NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_3 + ",0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_3 + ",0))  ADEUDO_BIMESTRE_3, SUM(NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_4 + ",0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_4 + ",0))  ADEUDO_BIMESTRE_4, SUM(NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_5 + ",0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_5 + ",0))  ADEUDO_BIMESTRE_5, SUM(NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_6 + ",0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_6 + ",0))  ADEUDO_BIMESTRE_6, SUM((NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_1 + ",0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_1 + ",0)) + (NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_2 + ",0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_2 + ",0)) + (NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_3 + ",0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_3 + ",0))  + (NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_4 + ",0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_4 + ",0)) + (NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_5 + ",0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_5 + ",0)) + (NVL(";
                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_6 + ",0) - NVL("
                    + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_6 + ",0))) ADEUDO_TOTAL_ANIO";
                Mi_SQL += " FROM " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial;
                if (!String.IsNullOrEmpty(Estatus))   // filtrar desde fecha si llego parametro
                {
                    Filtros_SQL += " WHERE " + Ope_Pre_Adeudos_Predial.Campo_Estatus + " LIKE '" + Estatus + "'";
                }
                if (!String.IsNullOrEmpty(Cuenta_Predial))   // filtrar por cuenta predial si llego como parametro
                {
                    if (Filtros_SQL.Length == 0)        // si no hay filtro, agregar nuevos filtros con WHERE
                    {
                        Filtros_SQL += " WHERE " + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID;
                        Filtros_SQL += " = '" + Cuenta_Predial + "'";
                    }
                    else
                    {
                        Filtros_SQL += " AND " + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID;
                        Filtros_SQL += " = '" + Cuenta_Predial + "'";
                    }
                }
                if (Desde_Anio > 0)   // filtrar desde anio si llego parametro
                {
                    if (Filtros_SQL.Length == 0)        // si no hay filtro, agregar nuevos filtros con WHERE
                    {
                        Filtros_SQL += " WHERE " + Ope_Pre_Adeudos_Predial.Campo_Anio;
                        Filtros_SQL += " >= " + Desde_Anio;
                    }
                    else
                    {
                        Filtros_SQL += " AND " + Ope_Pre_Adeudos_Predial.Campo_Anio;
                        Filtros_SQL += " >= " + Desde_Anio;
                    }
                }
                if (Hasta_Anio > 0)   // filtrar desde anio si llego parametro
                {
                    if (Filtros_SQL.Length == 0)        // si no hay filtro, agregar nuevos filtros con WHERE
                    {
                        Filtros_SQL += " WHERE " + Ope_Pre_Adeudos_Predial.Campo_Anio;
                        Filtros_SQL += " <= " + Hasta_Anio;
                    }
                    else
                    {
                        Filtros_SQL += " AND " + Ope_Pre_Adeudos_Predial.Campo_Anio;
                        Filtros_SQL += " <= " + Hasta_Anio;
                    }
                }
                Filtros_SQL += " GROUP BY " + Ope_Pre_Adeudos_Predial.Campo_Anio;
                Filtros_SQL += " ORDER BY " + Ope_Pre_Adeudos_Predial.Campo_Anio;
                Mi_SQL += Filtros_SQL
                    + " ) SELECT * FROM ADEUDOS "
                    +" WHERE ADEUDO_TOTAL_ANIO <> 0 ";

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
        }   //FUNCIÓN: Consultar_Adeudos_Cuenta_Predial_Con_Totales

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Ordenes_Cuota_Minima
        ///DESCRIPCIÓN: Consulta los datos de las ordenes de variacion generadas a partir de una fecha dada
        ///PARAMETROS:     
        ///             1. Cuenta_Predial_ID: ID de la cuenta predial a consultar
        ///CREO: Roberto Gonzáles Oseguera
        ///FECHA_CREO: 15-nov-2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Ordenes_Cuota_Minima(String Fecha, String Observaciones)
        {
            DataTable Dt_Ultimo_Movimiento = new DataTable();
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";

            try
            {
                Mi_SQL = "SELECT (SELECT "
                    + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " FROM "
                    + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE "
                    + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = "
                    + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion
                    + "." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + ") CUENTA_PREDIAL, "
                    + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_No_Nota
                    + " FROM " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion
                    + " WHERE " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo
                    + " > '" + Fecha + "'"
                    + " AND " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden
                    + " = 'ACEPTADA'"
                    + " AND " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Observaciones
                    + " = '" + Observaciones + "'"
                    + " ORDER BY " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " DESC";

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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Movimientos_Aceptados_Cuentas
        ///DESCRIPCIÓN: Regresa un datatable con el id de las cuentas predial y sus movimientos
        ///PARAMETROS: 
        ///CREO: Roberto Gonzáles Oseguera
        ///FECHA_CREO: 23-dic-2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Movimientos_Aceptados_Cuentas()
        {
            DataTable Dt_Ultimo_Movimiento = new DataTable();
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";

            try
            {
                Mi_SQL = "SELECT "
                    + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID
                    + ", (SELECT "
                    + Cat_Pre_Movimientos.Campo_Identificador
                    + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos
                    + " WHERE "
                    + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = "
                    + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID
                    + ") ULTIMO_MOVIMIENTO"
                    + "," + Cat_Pre_Movimientos.Campo_Movimiento_ID
                    + " FROM " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion
                    + " WHERE " + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " = 'ACEPTADA'"
                    + " ORDER BY " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ", "
                    + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " DESC";

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
        ///NOMBRE DE LA FUNCIÓN : Consultar_Cuentas
        ///DESCRIPCIÓN          : Obtiene las cuentas de acuerdo a los filtros establecidos en la interfaz
        ///PARAMETROS           : 
        ///                     1. Estatus: Lista entre comillas separada por comas de los estatus a excluir 'ACTIVA','CANCELADA'
        ///                     2. Filtros_Dinamicos: filtros para la consulta (tipo de predio y domicilio foraneo)
        ///CREO                 : Roberto González Oseguera
        ///FECHA_CREO           : 07/Diciembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Datos_Cuentas(String Estatus, String Filtros_Dinamicos)
        {
            String Mi_SQL;

            Mi_SQL = "SELECT "
                + "UNIQUE("
                + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ")"
                + ", " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + "." + Cat_Pre_Calles.Campo_Nombre + " AS NOMBRE_CALLE"
                + ", " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + "." + Cat_Ate_Colonias.Campo_Nombre + " AS NOMBRE_COLONIA";
            Mi_SQL += ", (SELECT TRIM(" + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " | | ' ' | | "
                + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " | | ' ' | | "
                + Cat_Pre_Contribuyentes.Campo_Nombre + ") FROM "
                + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " WHERE "
                + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = "
                + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Contribuyente_ID
                + ") AS PROPIETARIO_CTA";
            Mi_SQL += ", " + Cat_Pre_Estados_Predio.Tabla_Cat_Pre_Estados_Predio + "." + Cat_Pre_Estados_Predio.Campo_Descripcion + " AS DESCRIPCION_ESTADO_PREDIO"
                + ", " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio + "." + Cat_Pre_Tipos_Predio.Campo_Descripcion + " AS DESCRIPCION_TIPO_PREDIO"
                + ", " + Cat_Pre_Uso_Suelo.Tabla_Cat_Pre_Uso_Suelo + "." + Cat_Pre_Uso_Suelo.Campo_Descripcion + " AS DESCRIPCION_USO_SUELO"
                + ", " + Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual + "." + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Anual + " AS IMPUESTO_PREDIAL"
                + ", " + Cat_Pre_Cuotas_Minimas.Tabla_Cat_Pre_Cuotas_Minimas + "." + Cat_Pre_Cuotas_Minimas.Campo_Cuota + " AS CUOTA_MINIMA"
                + ", " + Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual
                + ", " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial
                + ", " + Cat_Pre_Cuentas_Predial.Campo_No_Exterior
                + ", " + Cat_Pre_Cuentas_Predial.Campo_No_Interior
                + ", " + Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion
                + ", " + Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion
                + ", " + Cat_Pre_Cuentas_Predial.Campo_Efectos
                + ", " + Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal
                + ", " + Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion
                + ", " + Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion
                + ", " + Cat_Pre_Cuentas_Predial.Campo_Ciudad_Notificacion
                + ", " + Cat_Pre_Cuentas_Predial.Campo_Estado_Notificacion
                + ", " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID
                + ", NVL(TRIM(" + Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo + "),'NO') " + Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo;
            Mi_SQL += ", ( "
                + "SELECT " + Cat_Ate_Colonias.Campo_Nombre
                + " FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias
                + " WHERE " + Cat_Ate_Colonias.Campo_Colonia_ID + " = "
                + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID
                + " ) COLONIA_UBICACION ";
            Mi_SQL += ", ( "
                + "SELECT " + Cat_Ate_Colonias.Campo_Nombre
                + " FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias
                + " WHERE " + Cat_Ate_Colonias.Campo_Colonia_ID + " = "
                + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion
                + " ) COLONIA_LOCAL_NOTIFICACION ";
            Mi_SQL += ", ( "
                + "SELECT " + Cat_Pre_Calles.Campo_Nombre
                + " FROM " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles
                + " WHERE " + Cat_Pre_Calles.Campo_Calle_ID + " = "
                + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion
                + " ) CALLE_LOCAL_NOTIFICACION ";
            Mi_SQL += ", ( "
                + "SELECT " + Cat_Pre_Sectores.Campo_Clave
                + " FROM " + Cat_Pre_Sectores.Tabla_Cat_Pre_Sectores
                + " WHERE " + Cat_Pre_Sectores.Campo_Sector_ID
                + " = ( "
                + "SELECT " + Cat_Ate_Colonias.Campo_Colonia_ID
                + " FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias
                + " WHERE " + Cat_Ate_Colonias.Campo_Colonia_ID + " = "
                + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID
                + ") ) SECTOR ";
            Mi_SQL += ", ( "
                + "SELECT SUBSTR(" + Cat_Pre_Tipos_Predio.Campo_Descripcion
                + ", 1, 1) FROM " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio
                + " WHERE " + Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + " = "
                + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID
                + " ) TIPO_PREDIO";

            Mi_SQL += " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas;

            Mi_SQL += " LEFT OUTER JOIN " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " ON "
                + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = "
                + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " AND "
                + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Tipo + " IN ('PROPIETARIO','POSEEDOR')";
            Mi_SQL += " LEFT OUTER JOIN " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " ON "
                + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + " = "
                + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + "." + Cat_Pre_Calles.Campo_Calle_ID;
            Mi_SQL += " LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " ON "
                + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + " = "
                + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + "." + Cat_Ate_Colonias.Campo_Colonia_ID;
            Mi_SQL += " LEFT OUTER JOIN " + Cat_Pre_Estados_Predio.Tabla_Cat_Pre_Estados_Predio + " ON "
                + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID + " = "
                + Cat_Pre_Estados_Predio.Tabla_Cat_Pre_Estados_Predio + "." + Cat_Pre_Estados_Predio.Campo_Estado_Predio_ID + " ";
            Mi_SQL += " LEFT OUTER JOIN " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio + " ON "
                + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + " = "
                + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio + "." + Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + " ";
            Mi_SQL += " LEFT OUTER JOIN " + Cat_Pre_Uso_Suelo.Tabla_Cat_Pre_Uso_Suelo + " ON "
                + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID + " = "
                + Cat_Pre_Uso_Suelo.Tabla_Cat_Pre_Uso_Suelo + "." + Cat_Pre_Uso_Suelo.Campo_Uso_Suelo_ID + " ";
            Mi_SQL += " LEFT OUTER JOIN " + Cat_Pre_Tasas_Predial.Tabla_Cat_Pre_Tasas_Predial + " ON "
                + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID + " = "
                + Cat_Pre_Tasas_Predial.Tabla_Cat_Pre_Tasas_Predial + "." + Cat_Pre_Tasas_Predial.Campo_Tasa_Predial_ID + " ";
            Mi_SQL += " LEFT OUTER JOIN " + Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual + " ON "
                + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tasa_ID + " = "
                + Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual + "." + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_ID;
            Mi_SQL += " LEFT OUTER JOIN " + Cat_Pre_Cuotas_Minimas.Tabla_Cat_Pre_Cuotas_Minimas + " ON "
                + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID + " = "
                + Cat_Pre_Cuotas_Minimas.Tabla_Cat_Pre_Cuotas_Minimas + "." + Cat_Pre_Cuotas_Minimas.Campo_Cuota_Minima_ID + " ";

            Mi_SQL += " WHERE ";
            if (!String.IsNullOrEmpty(Estatus))            // si Estatus no es nulo ni vacio
            {                                                           // filtrar por estatus
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estatus + Estatus;
            }
            if (!String.IsNullOrEmpty(Filtros_Dinamicos))
            {
                if (Mi_SQL.EndsWith("WHERE "))
                {
                    Mi_SQL += Filtros_Dinamicos;
                }
                else
                {
                    Mi_SQL += " AND " + Filtros_Dinamicos;
                }
            }

            try
            {
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de la Cuentas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
        }

    }
}
