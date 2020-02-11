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
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Operacion_Predial_Orden_Variacion.Negocio;
using Presidencia.Operacion_Predial_Parametros.Negocio;

namespace Operacion_Predial_Orden_Variacion.Datos
{
    public class Cls_Ope_Orden_Variacion_Datos
    {

        #region Metodos Consutla sobre cuenta
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Dato_Anterior
        ///DESCRIPCIÓN: se consulta el dato anterior del campo y cuenta especificado
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 11/Ago/2011 11:51:41 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public static String Consulta_Dato_Anterior(String Cuenta, String Campo)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL
            DataSet Ds_Resultado = new DataSet();

            try
            {
                Mi_SQL = "";
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Campo;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas;
                Mi_SQL = Mi_SQL + " WHERE ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = '" + Cuenta + "'";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0][0].ToString();
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


        #endregion

        

        #region Metodos Consultas Externas
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Cuenta_Existente
        ///DESCRIPCIÓN          : Obtiene Si hay una cuenta en la tabla con la misma cuenta predial.
        ///PARAMETROS           : Cuenta, instancia de Cls_Cat_Pre_Cuentas_Predial_Negocio
        ///CREO                 : Miguel Angel Bedolla Moreno
        ///FECHA_CREO           : 22/Septiembre/2011 
        ///MODIFICO:            : Jesus Toledo
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN   :Regregar ID
        ///*******************************************************************************
        public static String Consultar_Cuenta_Existente_ID(String Cuenta_Predial)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL;
            String ID = "";
            try
            {
                ////////////////////////////////////////////////////////////////
                Mi_SQL = "SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "," + Cat_Pre_Cuentas_Predial.Campo_Estatus + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE UPPER(" + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ") = UPPER('" + Cuenta_Predial + "')";
                ////////////////////////////////////////////////////////////////
                DataSet dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataSet != null)
                {
                    Tabla = dataSet.Tables[0];
                    if (Tabla.Rows.Count != 0)
                    {
                        ID = Tabla.Rows[0][0].ToString() + "-" + Tabla.Rows[0][1].ToString();
                    }
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de la Cuentas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return ID;
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consultar_Grupo_Mov
        ///DESCRIPCIÓN: consulta el id del movimiento de cancelacion
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 16/Ago/2011 11:50:12 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************    
        public static DataTable Consultar_Grupo_Mov(String ID)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL;
            String Mi_SQL_Campos_Foraneos = "";

            try
            {
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Cat_Pre_Movimientos.Campo_Movimiento_ID + " AS MOVIMIENTO_ID, ";
                Mi_SQL = Mi_SQL + Cat_Pre_Movimientos.Campo_Identificador + " AS IDENTIFICADOR, ";
                Mi_SQL = Mi_SQL + Cat_Pre_Movimientos.Campo_Estatus + " AS ESTATUS, ";
                Mi_SQL = Mi_SQL + Cat_Pre_Movimientos.Campo_Grupo_Id + " AS GRUPO_ID, ";
                Mi_SQL = Mi_SQL + Cat_Pre_Movimientos.Campo_Descripcion + " AS DESCRIPCION";

                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = '" + ID + "'";

                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Movimientos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Id_Movimiento
        ///DESCRIPCIÓN: consulta el id del movimiento de cancelacion
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 16/Ago/2011 11:50:12 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************              
        internal static String Consulta_Id_Movimiento(String Filtro)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL            

            try
            {
                Mi_SQL = "";
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Cat_Pre_Movimientos.Campo_Movimiento_ID + " AS ID ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos;
                Mi_SQL = Mi_SQL + " WHERE UPPER(" + Cat_Pre_Movimientos.Campo_Descripcion + ") LIKE UPPER('%CORRECIÓN DEL PADRÓN%')";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0][0].ToString();

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


        #endregion

        

        #region Diferencias
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN     : Consultar_Ordenes_Bajas_Directas
        ///DESCRIPCIÓN              : Obtiene las Órdenes para las Bajas Directas
        ///PARAMENTROS              : 1.  Orden. Instancia de la Clase con los Parámetros de Consulta
        ///CREO                     : Antonio Salvador Benavides Guardado
        ///FECHA_CREO               : 18/Noviembre/2011 
        ///MODIFICO: 
        ///FECHA_MODIFICO: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************
        public static DataTable Consultar_Ordenes_Bajas_Directas(Cls_Ope_Pre_Orden_Variacion_Negocio Orden)
        {
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL = "";


                Mi_SQL = "SELECT ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ", ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + ", ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Anio + ", ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + ", ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ", ";
                Mi_SQL += Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + "." + Cat_Pre_Movimientos.Campo_Descripcion + ", ";
                Mi_SQL += Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + "." + Cat_Pre_Movimientos.Campo_Identificador + ", ";
                Mi_SQL += Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + "." + Cat_Pre_Movimientos.Campo_Identificador + " || ' - ' || ";
                Mi_SQL += Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + "." + Cat_Pre_Movimientos.Campo_Descripcion + " AS IDENTIFICADOR_DESCRIPCION, ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + ", ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Observaciones;
                Mi_SQL += " FROM " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + ", ";
                Mi_SQL += Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos;
                Mi_SQL += " WHERE " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " = " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                Mi_SQL += " AND " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + " = " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + "." + Cat_Pre_Movimientos.Campo_Movimiento_ID;
                Mi_SQL += " AND " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + " IN (SELECT " + Cat_Pre_Movimientos.Campo_Movimiento_ID + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " WHERE " + Cat_Pre_Movimientos.Campo_Cargar_Modulos + " LIKE '%BAJAS_DIRECTAS%')";
                Mi_SQL += " AND " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " = 'ACEPTADA'";
                Mi_SQL += " ORDER BY " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " DESC, " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + " DESC, " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion;


                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Bajas Directas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return tabla;
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Datos_Cuenta
        ///DESCRIPCIÓN: se consultan los datos generales de la cuenta predial
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 19/Ago/2011 11:49:25 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************* 
        //Insertar detalles de las diferencias almacenadas en el datatable de la capa de negocio
        public static void Alta_Diferencias(Cls_Ope_Pre_Orden_Variacion_Negocio Datos)
        {
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion;
            OracleCommand Obj_Comando;
            String Mi_SQL = ""; //Variable para la consulta SQL            
            DataSet Ds_Resultado = new DataSet();
            string Diferencia_ID = "";
            string Diferencia_Detalle_ID = "";
            object Aux;
            if (Datos.P_Cmmd != null)
            {
                Obj_Comando = Datos.P_Cmmd;
            }
            else
            {
                Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
                Obj_Comando = new OracleCommand();
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;
            }

            try
            {
                //Formar Sentencia de consulta de consecutivo de la tabla diferencias o rezago
                Mi_SQL = "";
                Mi_SQL = "SELECT NVL(MAX(";
                Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Campo_No_Diferencia + "),0000000000)";
                Mi_SQL = Mi_SQL + " FROM ";
                Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias;

                //Ejecutar consulta

                Aux = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0][0];

                //Verificar si no es nulo
                if (Convert.IsDBNull(Aux) == false)
                {
                    Diferencia_ID = String.Format("{0:0000000000}", Convert.ToInt32(Aux) + 1);
                }
                else
                    Diferencia_ID = "0000000001";

                //Insertar Registro de Diferencias
                Mi_SQL = "";
                Mi_SQL = "INSERT INTO ";
                Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + " ( ";
                Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Campo_No_Diferencia + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Campo_Cuenta_Predial_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Campo_Anio + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Campo_No_Orden_Variacion + ") ";
                Mi_SQL = Mi_SQL + "VALUES('";
                Mi_SQL = Mi_SQL + Diferencia_ID + "','";
                Mi_SQL = Mi_SQL + Datos.P_Generar_Orden_Cuenta_ID + "', ";
                Mi_SQL = Mi_SQL + Datos.P_Generar_Orden_Anio + ", '";
                Mi_SQL = Mi_SQL + Datos.P_Generar_Orden_No_Orden + "') ";
                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();

                //Formar Sentencia de consulta de consecutivo de la tabla de detalles diferencias o rezago
                Mi_SQL = "";
                Mi_SQL = "SELECT NVL(MAX(";
                Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_No_Diferencias_Detalles + "),0000000000)";
                Mi_SQL = Mi_SQL + " FROM ";
                Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Tabla_Ope_Pre_Diferencias_Detalle;

                //Ejecutar consulta

                Aux = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0][0];

                //Verificar si no es nulo
                if (Convert.IsDBNull(Aux) == false)
                {
                    Diferencia_Detalle_ID = String.Format("{0:0000000000}", Convert.ToInt32(Aux) + 1);
                }
                else
                    Diferencia_Detalle_ID = "0000000001";

                foreach (DataRow Diferencia in Datos.P_Dt_Diferencias.Rows)
                {
                    Mi_SQL = "";
                    Mi_SQL = "INSERT INTO ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Tabla_Ope_Pre_Diferencias_Detalle + " ( ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_No_Diferencias_Detalles + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_No_Diferencia + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Valor_Fiscal + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Tasa_Predial_ID + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Tipo_Diferencia + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Tipo_Periodo + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Importe + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Cuota_Bimestral + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Periodo + ") VALUES('";
                    Mi_SQL = Mi_SQL + Diferencia_Detalle_ID + "','";
                    Mi_SQL = Mi_SQL + Diferencia_ID + "','" + Diferencia["VALOR_FISCAL"].ToString().Replace(",", "") + "','";
                    Mi_SQL = Mi_SQL + Diferencia["TASA_ID"].ToString() + "','" + Diferencia["TIPO"].ToString() + "','" + Diferencia["TIPO_PERIODO"].ToString() + "','";
                    Mi_SQL = Mi_SQL + Diferencia["IMPORTE"].ToString().Replace("$", "").Replace(",", "") + "','" + Diferencia["CUOTA_BIMESTRAL"].ToString().Replace("$", "").Replace(",", "") + "','" + Diferencia["PERIODO"].ToString() + "')";
                    Diferencia_Detalle_ID = String.Format("{0:0000000000}", Convert.ToInt32(Diferencia_Detalle_ID) + 1);
                    //Ejecutar consulta
                    Obj_Comando.CommandText = Mi_SQL;
                    Obj_Comando.ExecuteNonQuery();
                }
                //Agregar Variacion para ingresar el movimineto
                Datos.Agregar_Variacion(Cat_Pre_Cuentas_Predial.Campo_No_Diferencia, Diferencia_ID);

                if (Datos.P_Cmmd == null)
                {
                    Obj_Transaccion.Commit();
                }
            }
            catch (OracleException Ex)
            {
                if (Datos.P_Cmmd == null)
                {
                    Obj_Transaccion.Rollback();
                }
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

        #endregion

        public static DataTable Consulta_Diferencias(Cls_Ope_Pre_Orden_Variacion_Negocio Datos)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL
            DataSet Ds_Resultado = new DataSet();

            try
            {
                Mi_SQL = "";
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + " di." + Ope_Pre_Diferencias.Campo_No_Diferencia + ",";
                Mi_SQL = Mi_SQL + " di." + Ope_Pre_Diferencias.Campo_Cuenta_Predial_ID + ",";
                Mi_SQL = Mi_SQL + " de." + Ope_Pre_Diferencias_Detalle.Campo_Importe + ",";
                Mi_SQL = Mi_SQL + " de." + Ope_Pre_Diferencias_Detalle.Campo_Periodo + ",";
                Mi_SQL = Mi_SQL + " de." + Ope_Pre_Diferencias_Detalle.Campo_Tasa_Predial_ID + " AS TASA_ID,";
                Mi_SQL = Mi_SQL + " de." + Ope_Pre_Diferencias_Detalle.Campo_Tipo_Diferencia + " AS TIPO,";
                Mi_SQL = Mi_SQL + " de." + Ope_Pre_Diferencias_Detalle.Campo_Tipo_Periodo + ",";
                Mi_SQL = Mi_SQL + " de." + Ope_Pre_Diferencias_Detalle.Campo_Valor_Fiscal + ",";
                Mi_SQL = Mi_SQL + " ta." + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Anual + " AS TASA, ";
                Mi_SQL = Mi_SQL + " ta_P." + Cat_Pre_Tasas_Predial.Campo_Descripcion + " AS DESCRIPCION_TASA, ";
                Mi_SQL = Mi_SQL + " de." + Ope_Pre_Diferencias_Detalle.Campo_Cuota_Bimestral;

                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + " di ";
                Mi_SQL = Mi_SQL + " JOIN " + Ope_Pre_Diferencias_Detalle.Tabla_Ope_Pre_Diferencias_Detalle + " de ON de.";
                Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_No_Diferencia + " = di.";
                Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Campo_No_Diferencia + " LEFT OUTER JOIN ";
                Mi_SQL = Mi_SQL + Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual + " ta on ta.";
                Mi_SQL = Mi_SQL + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_ID + " = de." + Ope_Pre_Diferencias_Detalle.Campo_Tasa_Predial_ID + " LEFT OUTER JOIN ";
                Mi_SQL = Mi_SQL + Cat_Pre_Tasas_Predial.Tabla_Cat_Pre_Tasas_Predial + " ta_P on ta_P.";
                Mi_SQL = Mi_SQL + Cat_Pre_Tasas_Predial.Campo_Tasa_Predial_ID + " = ta." + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Predial_ID + " ";

                Mi_SQL = Mi_SQL + " WHERE ";
                Mi_SQL = Mi_SQL + "di." + Ope_Pre_Diferencias.Campo_Cuenta_Predial_ID + " = '" + Datos.P_Cuenta_Predial_ID + "'";
                Mi_SQL = Mi_SQL + " AND ";
                Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Campo_No_Orden_Variacion + " = '" + Datos.P_Generar_Orden_No_Orden + "'";
                Mi_SQL = Mi_SQL + " AND ";
                Mi_SQL = Mi_SQL + "di." + Ope_Pre_Diferencias.Campo_Anio + " = '" + Datos.P_Generar_Orden_Anio + "'";

                Mi_SQL = Mi_SQL + " ORDER BY SUBSTR(de." + Ope_Pre_Diferencias_Detalle.Campo_Periodo + ", LENGTH(de." + Ope_Pre_Diferencias_Detalle.Campo_Periodo + ") - 3, 4), ";
                Mi_SQL = Mi_SQL + "de." + Ope_Pre_Diferencias_Detalle.Campo_Periodo;
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

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

        #region Consulta_Datos_Cuenta
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Datos_Propietario
        ///DESCRIPCIÓN: se consulta con el ID de la cuenta los datos del propietario
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 08/23/2011 09:21:18 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        internal static DataSet Consulta_Datos_Propietario(Cls_Ope_Pre_Orden_Variacion_Negocio Datos)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL            
            DataSet Ds_Resultado = new DataSet();
            string Propietario_ID;
            try
            {
                Mi_SQL = "";
                Mi_SQL += "SELECT ";
                Mi_SQL += " PROP. " + Cat_Pre_Propietarios.Campo_Propietario_ID + " AS PROPIETARIO, ";
                Mi_SQL += " PROP. " + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " AS CONTRIBUYENTE, ";
                Mi_SQL += " PROP. " + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " AS CUENTA_ID, ";
                Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " ||' '|| ";
                Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " ||' '|| ";
                Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Nombre + " AS NOMBRE_PROPIETARIO, ";
                //Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Tipo_Propietario + " AS TIPO_PROPIETARIO, ";
                Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_RFC + " AS RFC, ";
                //
                Mi_SQL += " COL. " + Cat_Ate_Colonias.Campo_Nombre + " AS NOMBRE_COLONIA, ";
                Mi_SQL += " CALL. " + Cat_Pre_Calles.Campo_Nombre + " AS NOMBRE_CALLE, ";
                Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion + ", ";
                Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion + ", ";
                Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion + ", ";
                Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion + ", ";
                Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo + ", ";
                Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Codigo_Postal + ", ";
                Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Ciudad_Notificacion + ", ";
                Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Estado_Notificacion + ", ";
                Mi_SQL += " PROP. " + Cat_Pre_Propietarios.Campo_Tipo + " AS TIPO_PROPIETARIO, ";
                Mi_SQL += " EDO. " + Cat_Pre_Estados.Campo_Nombre + " AS NOMBRE_ESTADO, ";
                Mi_SQL += " EDO. " + Cat_Pre_Ciudades.Campo_Nombre + " AS NOMBRE_CIUDAD ";

                Mi_SQL += " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " PROP LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " CON ON CON.";
                Mi_SQL += Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = PROP." + Cat_Pre_Propietarios.Campo_Contribuyente_ID;
                Mi_SQL += " LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CUEN ON CUEN.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = PROP." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID;
                Mi_SQL += " LEFT OUTER JOIN ";
                Mi_SQL += Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COL ON CUEN.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion + " = COL." + Cat_Ate_Colonias.Campo_Colonia_ID;
                Mi_SQL += " LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CALL ON CUEN.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion + " = CALL." + Cat_Pre_Calles.Campo_Calle_ID;
                Mi_SQL += " LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Estados.Tabla_Cat_Pre_Estados + " EDO ON CUEN.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion + " = EDO." + Cat_Pre_Estados.Campo_Estado_ID;
                Mi_SQL += " LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Ciudades.Tabla_Cat_Pre_Ciudades + " CD ON CUEN.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion + " = CD." + Cat_Pre_Ciudades.Campo_Ciudad_ID;

                Mi_SQL += " WHERE ";
                Mi_SQL += "PROP." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " = '" + Datos.P_Cuenta_Predial_ID + "'";
                Mi_SQL += "AND (PROP." + Cat_Pre_Propietarios.Campo_Tipo + " = 'PROPIETARIO'";
                Mi_SQL += "OR PROP." + Cat_Pre_Propietarios.Campo_Tipo + " = 'POSEEDOR')";

                Ds_Resultado.Tables.Add(OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Copy());
                Ds_Resultado.Tables[0].TableName = "Dt_Propietarios";

                return Ds_Resultado;
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

        /////******************************************************************************* 
        /////NOMBRE DE LA FUNCIÓN     : Consulta_Datos_Copropietario
        /////DESCRIPCIÓN              : Se consultan con el ID de la cuenta los datos de los Copropietarios
        /////PARAMETROS: 
        /////CREO                     : Antonio Salvador Benavides Guardado
        /////FECHA_CREO               : 31/Agosto/2011
        /////MODIFICO: 
        /////FECHA_MODIFICO:
        /////CAUSA_MODIFICACIÓN:
        /////*******************************************************************************        
        //internal static DataSet Consulta_Datos_Copropietarios(Cls_Ope_Pre_Orden_Variacion_Negocio Datos)
        //{
        //    String Mi_SQL = ""; //Variable para la consulta SQL            
        //    DataSet Ds_Resultado = new DataSet();
        //    try
        //    {
        //        Mi_SQL = "";
        //        Mi_SQL += "SELECT ";
        //        Mi_SQL += " PROP. " + Cat_Pre_Propietarios.Campo_Propietario_ID + ", ";
        //        Mi_SQL += " PROP. " + Cat_Pre_Propietarios.Campo_Contribuyente_ID + ", ";
        //        Mi_SQL += " PROP. " + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " AS CUENTA_ID, ";
        //        Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " ||' '|| ";
        //        Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " ||' '|| ";
        //        Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Nombre + " AS NOMBRE_PROPIETARIO, ";
        //        //Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Tipo_Propietario + " AS TIPO_PROPIETARIO, ";
        //        Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_RFC + " AS RFC, ";
        //        //
        //        Mi_SQL += " COL. " + Cat_Ate_Colonias.Campo_Nombre + " AS NOMBRE_COLONIA, ";
        //        Mi_SQL += " CALL. " + Cat_Pre_Calles.Campo_Nombre + " AS NOMBRE_CALLE, ";
        //        Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion + ", ";
        //        Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion + ", ";
        //        Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo + ", ";
        //        Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Codigo_Postal + ", ";
        //        Mi_SQL += " PROP. " + Cat_Pre_Propietarios.Campo_Tipo + " AS TIPO_PROPIETARIO, ";
        //        Mi_SQL += " EDO. " + Cat_Pre_Estados.Campo_Nombre + " AS NOMBRE_ESTADO, ";
        //        Mi_SQL += " EDO. " + Cat_Pre_Ciudades.Campo_Nombre + " AS NOMBRE_CIUDAD ";

        //        Mi_SQL += " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " PROP LEFT OUTER JOIN ";
        //        Mi_SQL += Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " CON ON CON.";
        //        Mi_SQL += Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = PROP." + Cat_Pre_Propietarios.Campo_Contribuyente_ID;
        //        Mi_SQL += " LEFT OUTER JOIN ";
        //        Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CUEN ON CUEN.";
        //        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = PROP." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID;
        //        Mi_SQL += " LEFT OUTER JOIN ";
        //        Mi_SQL += Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COL ON CUEN.";
        //        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion + " = COL." + Cat_Ate_Colonias.Campo_Colonia_ID;
        //        Mi_SQL += " LEFT OUTER JOIN ";
        //        Mi_SQL += Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CALL ON CUEN.";
        //        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion + " = CALL." + Cat_Pre_Calles.Campo_Calle_ID;
        //        Mi_SQL += " LEFT OUTER JOIN ";
        //        Mi_SQL += Cat_Pre_Estados.Tabla_Cat_Pre_Estados + " EDO ON CUEN.";
        //        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion + " = EDO." + Cat_Pre_Estados.Campo_Estado_ID;
        //        Mi_SQL += " LEFT OUTER JOIN ";
        //        Mi_SQL += Cat_Pre_Ciudades.Tabla_Cat_Pre_Ciudades + " CD ON CUEN.";
        //        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion + " = CD." + Cat_Pre_Ciudades.Campo_Ciudad_ID;

        //        Mi_SQL += " WHERE ";
        //        Mi_SQL += "PROP." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " = '" + Datos.P_Cuenta_Predial_ID + "'";
        //        Mi_SQL += "AND PROP." + Cat_Pre_Propietarios.Campo_Tipo + " = 'COPROPIETARIO'";

        //        Ds_Resultado.Tables.Add(OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Copy());
        //        Ds_Resultado.Tables[0].TableName = "Dt_Propietarios";

        //        return Ds_Resultado;
        //    }
        //    catch (OracleException Ex)
        //    {
        //        throw new Exception("Error: " + Mi_SQL + Ex.Message);
        //    }

        //    catch (DBConcurrencyException Ex)
        //    {
        //        throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
        //    }
        //    catch (Exception Ex)
        //    {
        //        throw new Exception("Error: " + Ex.Message);
        //    }
        //    finally
        //    {
        //    }
        //}

       
       

        #endregion

        #region Copropietarios
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Co_Propietarios
        ///DESCRIPCIÓN: consulta los copropietarios que tiene la cuenta
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 08/11/2011 09:17:42 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        internal static DataTable Consulta_Co_Propietarios(Cls_Ope_Pre_Orden_Variacion_Negocio Datos)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL           

            try
            {
                Mi_SQL = "";
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + " CONT." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " AS CONTRIBUYENTE_ID,";
                Mi_SQL = Mi_SQL + " CONT." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " ||' '|| ";
                Mi_SQL = Mi_SQL + " CONT." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " ||' '|| ";
                Mi_SQL = Mi_SQL + " CONT." + Cat_Pre_Contribuyentes.Campo_Nombre + " AS NOMBRE_CONTRIBUYENTE, ";
                Mi_SQL = Mi_SQL + " CONT." + Cat_Pre_Contribuyentes.Campo_RFC + " AS RFC, ";
                Mi_SQL = Mi_SQL + " PROP." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + ", ";
                Mi_SQL = Mi_SQL + " PROP." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + ", ";
                Mi_SQL = Mi_SQL + " PROP." + Cat_Pre_Propietarios.Campo_Tipo + ", ";
                Mi_SQL = Mi_SQL + " 'ACTUAL' ESTATUS_VARIACION ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " CONT";
                Mi_SQL = Mi_SQL + " JOIN " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " PROP ON PROP.";
                Mi_SQL = Mi_SQL + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " = CONT." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID;
                Mi_SQL = Mi_SQL + " WHERE  PROP." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " = '" + Datos.P_Cuenta_Predial_ID + "'";
                Mi_SQL = Mi_SQL + " AND  PROP." + Cat_Pre_Propietarios.Campo_Tipo + " = 'COPROPIETARIO'";
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

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

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN     : Aplicar_Variacion_Propietarios
        ///DESCRIPCIÓN              : Afecta la variación de Propietarios
        ///PARAMETROS: 
        ///CREO                     : Antonio Salvador Benavides Guardado
        ///FECHA_CREO               : 06/Septiembre/2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************              
        internal static Boolean Aplicar_Variacion_Propietarios(Cls_Ope_Pre_Orden_Variacion_Negocio Propietario)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans = null;
            Boolean Alta = false;

            if (Propietario.P_Cmmd != null)
            {
                //Cmd.Connection = Propietario.P_Trans.Connection;
                //Cmd.Transaction = Propietario.P_Trans;
                Cmd = Propietario.P_Cmmd;
            }
            else
            {
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
            }
            try
            {
                String Mi_SQL = "";

                Mi_SQL = "DELETE FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios;
                Mi_SQL += " WHERE ";
                //if (Propietario.P_Copropietario_Cuenta_Predial_ID != "" && Propietario.P_Copropietario_Cuenta_Predial_ID != null)
                {
                    Mi_SQL += Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " = '" + Propietario.P_Copropietario_Cuenta_Predial_ID + "' AND ( ";
                    Mi_SQL += Cat_Pre_Propietarios.Campo_Tipo + " = 'PROPIETARIO' OR ";
                    Mi_SQL += Cat_Pre_Propietarios.Campo_Tipo + " = 'POSEEDOR')";
                }
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                //Int32 Propietario_ID = 0;
                if ((Propietario.P_Propietario_Cuenta_Predial_ID != "" && Propietario.P_Propietario_Cuenta_Predial_ID != null)
                    && (Propietario.P_Propietario_Propietario_ID != "" && Propietario.P_Propietario_Propietario_ID != null))
                {
                    Int32 Propietario_ID = Propietario_ID = Convert.ToInt32(Obtener_ID_Consecutivo(Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios, Cat_Pre_Propietarios.Campo_Propietario_ID, "", 10));
                    Mi_SQL = "INSERT INTO " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " (";
                    Mi_SQL += Cat_Pre_Propietarios.Campo_Propietario_ID + ", ";
                    Mi_SQL += Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + ", ";
                    Mi_SQL += Cat_Pre_Propietarios.Campo_Contribuyente_ID + ", ";
                    Mi_SQL += Cat_Pre_Propietarios.Campo_Tipo + ", ";
                    Mi_SQL += Cat_Pre_Propietarios.Campo_Usuario_Creo + ", ";
                    Mi_SQL += Cat_Pre_Propietarios.Campo_Fecha_Creo + ") ";
                    Mi_SQL += "VALUES (";
                    Mi_SQL += "'" + Propietario_ID.ToString("0000000000") + "', ";
                    if (Propietario.P_Propietario_Cuenta_Predial_ID != null)
                    {
                        Mi_SQL += "'" + Propietario.P_Propietario_Cuenta_Predial_ID + "', ";
                    }
                    else
                    {
                        Mi_SQL += "NULL, ";
                    }
                    if (Propietario.P_Propietario_Propietario_ID != null)
                    {
                        Mi_SQL += "'" + Propietario.P_Propietario_Propietario_ID + "', ";
                    }
                    else
                    {
                        Mi_SQL += "NULL, ";
                    }
                    if (Propietario.P_Propietario_Tipo != null)
                    {
                        Mi_SQL += "'" + Propietario.P_Propietario_Tipo + "', ";
                    }
                    else
                    {
                        Mi_SQL += "NULL, ";
                    }
                    Mi_SQL += "'" + Propietario.P_Usuario + "', SYSDATE)";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();

                    Propietario.P_Propietario_Propietario_ID = Propietario_ID.ToString("0000000000");
                }

                if (Propietario.P_Cmmd == null)
                {
                    Trans.Commit();
                }
                Alta = true;
            }
            catch (OracleException Ex)
            {
                if (Propietario.P_Cmmd == null)
                {
                    Trans.Rollback();
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Impuesto de Fraccionamiento. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Propietario.P_Cmmd == null)
                {
                    Cn.Close();
                }
            }
            return Alta;
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN     : Aplicar_Variacion_Copropietarios
        ///DESCRIPCIÓN              : Afecta la variación de Copropietarios
        ///PARAMETROS: 
        ///CREO                     : Antonio Salvador Benavides Guardado
        ///FECHA_CREO               : 31/Agosto/2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************              
        internal static Boolean Aplicar_Variacion_Copropietarios(Cls_Ope_Pre_Orden_Variacion_Negocio Copropietario)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans = null;
            Boolean Alta = false;

            if (Copropietario.P_Cmmd != null)
            {
                //Cmd.Connection = Copropietario.P_Trans.Connection;
                //Cmd.Transaction = Copropietario.P_Trans;
                Cmd = Copropietario.P_Cmmd;
            }
            else
            {
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
            }
            try
            {
                String Mi_SQL = "";

                Mi_SQL = "DELETE FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios;
                Mi_SQL += " WHERE ";
                //if (Copropietario.P_Copropietario_Cuenta_Predial_ID != "" && Copropietario.P_Copropietario_Cuenta_Predial_ID != null)
                {
                    Mi_SQL += Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " = '" + Copropietario.P_Copropietario_Cuenta_Predial_ID + "' AND ";
                    Mi_SQL += Cat_Pre_Propietarios.Campo_Tipo + " = 'COPROPIETARIO'";
                }
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                Int32 Propietario_ID;
                if (Copropietario.P_Propietario_Propietario_ID != null && Copropietario.P_Propietario_Propietario_ID.Trim() != "")
                {
                    Propietario_ID = Convert.ToInt32(Copropietario.P_Propietario_Propietario_ID.Trim()) + 1;
                }
                else
                {
                    Propietario_ID = Convert.ToInt32(Obtener_ID_Consecutivo(Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios, Cat_Pre_Propietarios.Campo_Propietario_ID, "", 10));
                }
                foreach (DataRow Dr_Copropietario in Copropietario.P_Dt_Copropietarios.Rows)
                {
                    Mi_SQL = "INSERT INTO " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " (";
                    Mi_SQL += Cat_Pre_Propietarios.Campo_Propietario_ID + ", ";
                    Mi_SQL += Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + ", ";
                    Mi_SQL += Cat_Pre_Propietarios.Campo_Contribuyente_ID + ", ";
                    Mi_SQL += Cat_Pre_Propietarios.Campo_Tipo + ", ";
                    Mi_SQL += Cat_Pre_Propietarios.Campo_Usuario_Creo + ", ";
                    Mi_SQL += Cat_Pre_Propietarios.Campo_Fecha_Creo + ") ";
                    Mi_SQL += "VALUES (";
                    Mi_SQL += "'" + Propietario_ID.ToString("0000000000") + "', ";
                    if (Copropietario.P_Copropietario_Cuenta_Predial_ID != null)
                    {
                        Mi_SQL += "'" + Copropietario.P_Copropietario_Cuenta_Predial_ID + "', ";
                    }
                    else
                    {
                        Mi_SQL += "NULL, ";
                    }
                    if (Dr_Copropietario["CONTRIBUYENTE_ID"] != null)
                    {
                        Mi_SQL += "'" + Dr_Copropietario["CONTRIBUYENTE_ID"].ToString() + "', ";
                    }
                    else
                    {
                        Mi_SQL += "NULL, ";
                    }
                    if (Copropietario.P_Copropietario_Tipo != null)
                    {
                        Mi_SQL += "'" + Copropietario.P_Copropietario_Tipo + "', ";
                    }
                    else
                    {
                        Mi_SQL += "NULL, ";
                    }
                    Mi_SQL += "'" + Copropietario.P_Usuario + "', SYSDATE)";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();

                    Propietario_ID += 1;
                }

                if (Copropietario.P_Cmmd == null)
                {
                    Trans.Commit();
                }
                Alta = true;
            }
            catch (OracleException Ex)
            {
                if (Copropietario.P_Cmmd == null)
                {
                    Trans.Rollback();
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Impuesto de Fraccionamiento. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Copropietario.P_Cmmd == null)
                {
                    Cn.Close();
                }
            }
            return Alta;
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN     : Aplicar_Variacion_Diferencias
        ///DESCRIPCIÓN              : Afecta los Adeudos con las Diferencias
        ///PARAMETROS: 
        ///CREO                     : Antonio Salvador Benavides Guardado
        ///FECHA_CREO               : 05/Septiembre/2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************              
        internal static Boolean Aplicar_Variacion_Diferencias(Cls_Ope_Pre_Orden_Variacion_Negocio Diferencias)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans = null;
            Boolean Alta = false;
            Boolean Suma_Primer_Bimestre = true;

            if (Diferencias.P_Cmmd != null)
            {
                //Cmd.Connection = Diferencias.P_Trans.Connection;
                //Cmd.Transaction = Diferencias.P_Trans;
                Cmd = Diferencias.P_Cmmd;
            }
            else
            {
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
            }
            try
            {
                String Mi_SQL = "";

                int Cont_Bimestres = 0;
                Int32 No_Adeudo = Convert.ToInt32(Obtener_ID_Consecutivo(Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial, Ope_Pre_Adeudos_Predial.Campo_No_Adeudo, "", 10));
                foreach (DataRow Dr_Diferencias in Diferencias.P_Dt_Diferencias.Rows)
                {
                    Mi_SQL = "SELECT " + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID;
                    Mi_SQL += " FROM " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial;
                    Mi_SQL += " WHERE ";
                    Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + " = '" + Diferencias.P_Cuenta_Predial_ID + "' AND ";
                    Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Anio + " = " + Dr_Diferencias["AÑO"];
                    Cmd.CommandText = Mi_SQL;
                    //Cmd.ExecuteReader();
                    //if (OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows.Count > 0)
                    if (!Cmd.ExecuteReader().Read())
                    {
                        Mi_SQL = "INSERT INTO " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + " (";
                        Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_No_Adeudo + ", ";
                        Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Fecha + ", ";
                        Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Cuota_Anual + ", ";
                        Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_1 + ", ";
                        Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_2 + ", ";
                        Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_3 + ", ";
                        Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_4 + ", ";
                        Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_5 + ", ";
                        Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_6 + ", ";
                        Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Anio + ", ";
                        Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Estatus + ", ";
                        Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + ", ";
                        Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Usuario_Creo + ", ";
                        Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Fecha_Creo + ") VALUES ('";
                        Mi_SQL += No_Adeudo.ToString("0000000000") + "', ";
                        Mi_SQL += "'" + DateTime.Now.ToString("d-M-yyyy") + "', ";
                        if (Dr_Diferencias["CUOTA_ANUAL"] != null)
                        {
                            Mi_SQL += Dr_Diferencias["CUOTA_ANUAL"].ToString() + ", ";
                        }
                        for (Cont_Bimestres = 1; Cont_Bimestres <= 6; Cont_Bimestres++)
                        {
                            if (Dr_Diferencias["BIMESTRE_" + Cont_Bimestres.ToString()] != null)
                            {
                                if (Dr_Diferencias["BIMESTRE_" + Cont_Bimestres.ToString()].ToString() != "")
                                {
                                    Mi_SQL += Dr_Diferencias["BIMESTRE_" + Cont_Bimestres.ToString()].ToString() + ", ";
                                }
                                else
                                {
                                    Mi_SQL += "NULL, ";
                                }
                            }
                        }
                        Mi_SQL += Dr_Diferencias["AÑO"] + ", ";
                        if (Diferencias.P_Diferencias_Estatus != null
                            && Diferencias.P_Diferencias_Estatus != "")
                        {
                            Mi_SQL += "'" + Diferencias.P_Diferencias_Estatus + "', ";
                        }
                        else
                        {
                            Mi_SQL += "'POR PAGAR', ";
                        }
                        Mi_SQL += "'" + Diferencias.P_Cuenta_Predial_ID + "', ";
                        Mi_SQL += "'" + Diferencias.P_Usuario + "', SYSDATE)";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                        No_Adeudo++;
                    }
                    else
                    {
                        Mi_SQL = "UPDATE " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + " SET ";
                        if (!Diferencias.P_Reactivando_Cuenta)
                        {
                            if (Dr_Diferencias["CUOTA_ANUAL"] != null)
                            {
                                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Cuota_Anual + " = " + Dr_Diferencias["CUOTA_ANUAL"].ToString() + ", ";
                            }
                        }
                        for (Cont_Bimestres = 1; Cont_Bimestres <= 6; Cont_Bimestres++)
                        {
                            if (Dr_Diferencias["BIMESTRE_" + Cont_Bimestres.ToString()] != null)
                            {
                                if (Dr_Diferencias["BIMESTRE_" + Cont_Bimestres.ToString()].ToString() != "")
                                {
                                    if (Diferencias.P_Suma_Variacion_Diferencias)
                                    {
                                        if (Dr_Diferencias["ALTA_BAJA"] != null)
                                        {
                                            if (Dr_Diferencias["ALTA_BAJA"].ToString() != "SOB" && Suma_Primer_Bimestre)
                                            {
                                                Mi_SQL += "BIMESTRE_" + Cont_Bimestres.ToString() + " = NVL(" + "BIMESTRE_" + Cont_Bimestres.ToString() + ", 0) + " + Dr_Diferencias["BIMESTRE_" + Cont_Bimestres.ToString()].ToString() + ", ";
                                                if (Dr_Diferencias["ALTA_BAJA"].ToString() == "SOB1")
                                                {
                                                    Suma_Primer_Bimestre = false;
                                                }
                                            }
                                            else
                                            {
                                                Mi_SQL += "BIMESTRE_" + Cont_Bimestres.ToString() + " = " + Dr_Diferencias["BIMESTRE_" + Cont_Bimestres.ToString()].ToString() + ", ";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Mi_SQL += "BIMESTRE_" + Cont_Bimestres.ToString() + " = " + Dr_Diferencias["BIMESTRE_" + Cont_Bimestres.ToString()].ToString() + ", ";
                                    }
                                }
                            }
                        }
                        if (Diferencias.P_Diferencias_Estatus != null
                            && Diferencias.P_Diferencias_Estatus != "")
                        {
                            Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Estatus + " = '" + Diferencias.P_Diferencias_Estatus + "', ";
                        }

                        Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Usuario_Modifico + " = '" + Diferencias.P_Usuario + "', ";
                        Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_SQL += " WHERE ";
                        Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + " = '" + Diferencias.P_Cuenta_Predial_ID + "' AND ";
                        Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Anio + " = " + Dr_Diferencias["AÑO"];
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }
                }

                if (Diferencias.P_Cmmd == null)
                {
                    Trans.Commit();
                }
                Alta = true;
            }
            catch (OracleException Ex)
            {
                if (Diferencias.P_Cmmd == null)
                {
                    Trans.Rollback();
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Impuesto de Fraccionamiento. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Diferencias.P_Cmmd == null)
                {
                    Cn.Close();
                }
            }
            return Alta;
        }

        #endregion

        #region Metodos
        public Cls_Ope_Orden_Variacion_Datos()
        {
        }

        #region Alta cuenta

        internal static String Alta_Cuenta(Cls_Ope_Pre_Orden_Variacion_Negocio Datos)
        {
            //Declaracion de variables
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion;
            OracleCommand Obj_Comando;
            String Mi_SQL = String.Empty;  //Variable para la sentencia SQL
            String Mensaje = String.Empty; //Variable para el mensaje de error
            String Diferencia_ID;
            int Consecutivo;
            Object Aux; //Variable auxiliar para las consultas            
            String No_Detalle;
            try
            {
                Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
                Obj_Comando = new OracleCommand();
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;


                Mi_SQL = "";
                Mi_SQL = "SELECT NVL(MAX(";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "),0000000000)";
                Mi_SQL = Mi_SQL + " FROM ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas;

                //Ejecutar consulta      '          
                Aux = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0][0];

                //Verificar si no es nulo
                if (Convert.IsDBNull(Aux) == false)
                {
                    No_Detalle = String.Format("{0:0000000000}", Convert.ToInt32(Aux) + 1);
                }
                else
                    No_Detalle = "0000000001";

                //Asignar consulta para la insercion de los datos generales de la orden a insertar
                Mi_SQL = "";
                Mi_SQL = "INSERT INTO " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "(";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ",";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Orden_Variacion.Campo_Fecha_Creo + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Orden_Variacion.Campo_Usuario_Creo + " )";
                Mi_SQL = Mi_SQL + " VALUES('" + No_Detalle + "','";
                Mi_SQL = Mi_SQL + Datos.P_Generar_Orden_Cuenta_ID + "',";
                Mi_SQL = Mi_SQL + " SYSDATE, '";
                Mi_SQL = Mi_SQL + Cls_Sessiones.Nombre_Empleado + "')";




                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();

                //Formar Sentencia para obtener el consecutivo de la orden

                Mi_SQL = "";
                Mi_SQL = "SELECT NVL(MAX(";
                Mi_SQL = Mi_SQL + Ope_Pre_Orden_Variacion.Campo_No_Orden_Variacion + "),0000000000)";
                Mi_SQL = Mi_SQL + " FROM ";
                Mi_SQL = Mi_SQL + Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion;
                Mi_SQL = Mi_SQL + " WHERE ";
                Mi_SQL = Mi_SQL + Ope_Pre_Orden_Variacion.Campo_Anio + " = '";
                Mi_SQL = Mi_SQL + DateTime.Today.Year.ToString() + "' ";

                //Ejecutar consulta                
                Aux = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0][0];

                //Verificar si no es nulo
                if (Convert.IsDBNull(Aux) == false)
                {
                    Datos.P_Generar_Orden_No_Orden = String.Format("{0:0000000000}", Convert.ToInt32(Aux) + 1);
                }
                else
                    Datos.P_Generar_Orden_No_Orden = "0000000001";

                //Asignar consulta para la insercion de los datos generales de la orden a insertar
                Mi_SQL = "";
                Mi_SQL = "INSERT INTO ";
                Mi_SQL = Mi_SQL + Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion + " ( ";
                Mi_SQL = Mi_SQL + Ope_Pre_Orden_Variacion.Campo_No_Orden_Variacion + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Orden_Variacion.Campo_Anio + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Orden_Variacion.Campo_Cuenta_Predial_ID + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Orden_Variacion.Campo_Movimiento_ID + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Orden_Variacion.Campo_Estatus + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Orden_Variacion.Campo_Observaciones + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Orden_Variacion.Campo_No_Contrarecibo + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Orden_Variacion.Campo_Fecha_Creo + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Orden_Variacion.Campo_Usuario_Creo + " )";
                Mi_SQL = Mi_SQL + " VALUES('" + Datos.P_Generar_Orden_No_Orden + "',";
                Mi_SQL = Mi_SQL + DateTime.Today.Year.ToString() + ",'";
                Mi_SQL = Mi_SQL + No_Detalle + "','";
                Mi_SQL = Mi_SQL + Datos.P_Generar_Orden_Movimiento_ID + "','";
                Mi_SQL = Mi_SQL + Datos.P_Generar_Orden_Estatus + "',UPPER('";
                Mi_SQL = Mi_SQL + Datos.P_Generar_Orden_Obserbaciones + "'),'";
                Mi_SQL = Mi_SQL + Datos.P_Contrarecibo;
                Mi_SQL = Mi_SQL + "',SYSDATE, '" + Cls_Sessiones.Nombre_Empleado + "')";


                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();
                //Ejecutar transaccion
                Obj_Transaccion.Commit();
                Obj_Conexion.Close();

                return Datos.P_Generar_Orden_No_Orden;

            }
            catch (OracleException Ex)
            {
                if (Obj_Transaccion != null)
                {
                    Obj_Transaccion.Rollback();
                }
                switch (Ex.Code.ToString())
                {
                    case "2291":
                        Mensaje = "Error: No existe un registro relacionado con esta operacion [" + Ex.Message + "]";
                        break;
                    case "923":
                        Mensaje = "Error: Consulta SQL [" + Ex.Message + "]";
                        break;
                    case "12170":
                        Mensaje = "Error: Conexion con el Servidor [" + Ex.Message + "]";
                        break;
                    default:
                        Mensaje = "Error:  [" + Mi_SQL + "   ]" + Ex.Message + "]";
                        break;
                }

                throw new Exception(Mensaje, Ex);
            }
            finally
            {
                Obj_Comando = null;
                Obj_Conexion = null;
                Obj_Transaccion = null;

            }
        }
        #endregion


        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Cambio_Propietarios
        ///DESCRIPCIÓN: se lee el datatable de las bajas y altas de los propietarios
        ///             y se insertan en los detalles de los propietarios tabla que se 
        ///             leera cuando se valide la orden de variacion
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 08/23/2011 09:26:47 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        internal static void Cambio_Propietarios(Cls_Ope_Pre_Orden_Variacion_Negocio Datos)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL
            String Consec = ""; //Variable para la consulta SQL
            Object Aux; //Variable auxiliar para las consultas 
            DataTable Dt_Copropietarios_Variacion;
            Boolean Propietario_Poseedor_Eliminados;
            try
            {
                //Formar Sentencia para obtener el consecutivo de la orden                
                Mi_SQL = "";
                Mi_SQL = "SELECT NVL(MAX(";
                Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Copropietario_Orden_Variacion_ID + "),0000000000)";
                Mi_SQL = Mi_SQL + " FROM ";
                Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion;
                //Ejecutar consulta del consecutivo
                Aux = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0][0];
                //Verificar si no es nulo
                if (Convert.IsDBNull(Aux) == false)
                {
                    Consec = String.Format("{0:0000000000}", Convert.ToInt32(Aux) + 1);
                }
                else
                {
                    Consec = "0000000001";
                }
                Propietario_Poseedor_Eliminados = false;
                foreach (DataRow Dr_Prop in Datos.P_Dt_Contribuyentes.Rows)
                {
                    if ((Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo].ToString() == "PROPIETARIO"
                         || Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo].ToString() == "POSEEDOR")
                         && Propietario_Poseedor_Eliminados == false)
                    {
                        Mi_SQL = "";
                        Mi_SQL = "DELETE FROM " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_No_Orden_Variacion + " = '" + Datos.P_Orden_Variacion_ID + "' ";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Anio + " = " + Datos.P_Generar_Orden_Anio + " ";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Cuenta_Predial_ID + " = '" + Datos.P_Generar_Orden_Cuenta_ID + "' ";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo + " IN ('PROPIETARIO', 'POSEEDOR')";
                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        Propietario_Poseedor_Eliminados = true;
                    }
                    Mi_SQL = "";
                    Mi_SQL = "SELECT " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Copropietario_Orden_Variacion_ID;
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_No_Orden_Variacion + " = '" + Datos.P_Orden_Variacion_ID + "' ";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Anio + " = " + Datos.P_Generar_Orden_Anio + " ";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Cuenta_Predial_ID + " = '" + Datos.P_Generar_Orden_Cuenta_ID + "' ";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID + " = '" + Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID].ToString() + "'";
                    if (Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo].ToString() == "COPROPIETARIO")
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo + " = '" + Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo].ToString() + "'";
                    Dt_Copropietarios_Variacion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                    if (Dt_Copropietarios_Variacion.Rows.Count > 0)
                    {
                        Mi_SQL = "";
                        Mi_SQL = "UPDATE " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion;
                        Mi_SQL = Mi_SQL + " SET " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Estatus + " = '" + Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Estatus].ToString() + "', ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo + " = '" + Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo].ToString() + "'";
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_No_Orden_Variacion + " = '" + Datos.P_Orden_Variacion_ID + "' ";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Anio + " = " + Datos.P_Generar_Orden_Anio + " ";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Cuenta_Predial_ID + " = '" + Datos.P_Generar_Orden_Cuenta_ID + "' ";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID + " = '" + Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID].ToString() + "'";
                        if (Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo].ToString() == "COPROPIETARIO")
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo + " = '" + Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo].ToString() + "'";
                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        Consec = String.Format("{0:0000000000}", Convert.ToInt32(Consec) + 1);
                    }
                    else
                    {
                        Mi_SQL = "";
                        Mi_SQL = "INSERT INTO ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + " ( ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Copropietario_Orden_Variacion_ID + ", ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Campo_No_Orden_Variacion + ", ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Cuenta_Predial_ID + ", ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID + ", ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Anio + ", ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Estatus + ", ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo + ", ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Usuario_Creo + ", ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Fecha_Creo + ") VALUES('";
                        Mi_SQL = Mi_SQL + Consec + "','";
                        Mi_SQL = Mi_SQL + Datos.P_Orden_Variacion_ID + "',";
                        if (String.IsNullOrEmpty(Datos.P_Generar_Orden_Cuenta_ID))
                            Mi_SQL = Mi_SQL + "NULL,'";
                        else
                            Mi_SQL = Mi_SQL + "'" + Datos.P_Generar_Orden_Cuenta_ID + "','";
                        Mi_SQL = Mi_SQL + Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID].ToString() + "',";
                        Mi_SQL = Mi_SQL + Datos.P_Generar_Orden_Anio + ",'";
                        Mi_SQL = Mi_SQL + Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Estatus].ToString() + "','";
                        Mi_SQL = Mi_SQL + Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo].ToString() + "','";
                        Mi_SQL = Mi_SQL + Presidencia.Sessiones.Cls_Sessiones.Nombre_Empleado + "',SYSDATE )";
                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        Consec = String.Format("{0:0000000000}", Convert.ToInt32(Consec) + 1);
                    }
                }
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

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cuentas_Canceladas
        ///DESCRIPCIÓN: Obtiene todos las Cajas que estan dadas de 
        ///             alta en la Base de Datos
        ///PARAMENTROS:   
        ///             1.  Orden.           Parametro de donde se sacara si habra o no un filtro de busqueda, en este
        ///                                 caso el filtro es la Clave.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 22/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Cuentas_Canceladas(Cls_Ope_Pre_Orden_Variacion_Negocio Orden)
        {
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT o." + Ope_Pre_Orden_Variacion.Campo_Cuenta_Predial_ID + " AS " + Ope_Pre_Orden_Variacion.Campo_Cuenta_Predial_ID;
                Mi_SQL = Mi_SQL + ", (SELECT c." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " c WHERE c." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "=o." + Ope_Pre_Orden_Variacion.Campo_Cuenta_Predial_ID;
                if (Orden.P_Cuenta_Predial.Length != 0)
                {
                    Mi_SQL = Mi_SQL + " AND c." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " LIKE '%" + Orden.P_Cuenta_Predial + "%'";
                    if (Orden.P_Generar_Orden_Estatus == "VIGENTE")
                    {
                        Mi_SQL = Mi_SQL + " AND c." + Ope_Pre_Orden_Variacion.Campo_Estatus + "!='VIGENTE'";
                    }
                    else
                    {
                        Mi_SQL = Mi_SQL + " AND c." + Ope_Pre_Orden_Variacion.Campo_Estatus + "='" + Orden.P_Generar_Orden_Estatus + "'";
                    }
                }
                Mi_SQL = Mi_SQL + ") AS " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial;
                Mi_SQL = Mi_SQL + ", o." + Ope_Pre_Orden_Variacion.Campo_Fecha_Creo + " AS " + Ope_Pre_Orden_Variacion.Campo_Fecha_Creo;
                Mi_SQL = Mi_SQL + ", (SELECT SUM(a." + Ope_Pre_Adeudos_Predial.Campo_Monto_Por_Pagar + ") FROM " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + " a WHERE a." + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + "=o." + Ope_Pre_Orden_Variacion.Campo_Cuenta_Predial_ID + ") AS ADEUDO_CANCELADO";
                Mi_SQL = Mi_SQL + ", o." + Ope_Pre_Orden_Variacion.Campo_Movimiento_ID + "";
                Mi_SQL = Mi_SQL + ", (SELECT m." + Cat_Pre_Movimientos.Campo_Identificador + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " m WHERE m." + Cat_Pre_Movimientos.Campo_Movimiento_ID + "=o." + Ope_Pre_Orden_Variacion.Campo_Movimiento_ID + ") AS TIPO_MOVIMIENTO";
                Mi_SQL = Mi_SQL + ", o." + Ope_Pre_Orden_Variacion.Campo_Observaciones + " AS MOTIVO_CANCELACION";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion + " o";

                //JOIN CAT_PRE_CUENTAS_PREDIAL c ON c.CUENTA_PREDIAL_ID = o.CUENTA_PREDIAL_ID WHERE c.ESTATUS = 'CANCELADA' 
                Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " c ON c." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = o." + Ope_Pre_Orden_Variacion.Campo_Cuenta_Predial_ID;
                //Mi_SQL = Mi_SQL + " WHERE c." + Cat_Pre_Cuentas_Predial.Campo_Estatus + " = 'CANCELADA' ";
                Mi_SQL = Mi_SQL + " WHERE o." + Ope_Pre_Orden_Variacion.Campo_Estatus + " = 'POR VALIDAR' ";
                //Añadir filtro
                if (Orden.P_Generar_Orden_Fecha_Inicial.Length != 0)
                {
                    Mi_SQL = Mi_SQL + " AND (o." + Ope_Pre_Orden_Variacion.Campo_Fecha_Creo + ">='" + Orden.P_Generar_Orden_Fecha_Inicial + "'";
                    if (Orden.P_Generar_Orden_Fecha_Final.Length != 0)
                    {
                        Mi_SQL = Mi_SQL + " AND o." + Ope_Pre_Orden_Variacion.Campo_Fecha_Creo + "<='" + Orden.P_Generar_Orden_Fecha_Final + "')";
                    }
                    else
                    {
                        Mi_SQL = Mi_SQL + ")";
                    }
                }
                else if (Orden.P_Generar_Orden_Fecha_Final.Length != 0)
                {
                    Mi_SQL = Mi_SQL + " AND (o." + Ope_Pre_Orden_Variacion.Campo_Fecha_Creo + "<='" + Orden.P_Generar_Orden_Fecha_Final + "')";
                }
                Mi_SQL = Mi_SQL + " ORDER BY o." + Ope_Pre_Orden_Variacion.Campo_Fecha_Creo + " DESC";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Cancelación de Cuenta Predial. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return tabla;
        }


        

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Adeudos
        ///DESCRIPCIÓN: Obitiene los adeudos por año.
        ///PARAMENTROS:   
        ///             1. P_Adeudo.   Adeudo los 
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 16/Agosto/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Adeudos(Cls_Ope_Pre_Orden_Variacion_Negocio Adeudo)
        {
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Ope_Pre_Adeudos_Predial.Campo_Anio + " AS ANIO";
                Mi_SQL += ", " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_1 + "-" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_1 + " AS ADEUDO_BIMESTRE_1";
                Mi_SQL += ", " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_2 + "-" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_2 + " AS ADEUDO_BIMESTRE_2";
                Mi_SQL += ", " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_3 + "-" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_3 + " AS ADEUDO_BIMESTRE_3";
                Mi_SQL += ", " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_4 + "-" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_4 + " AS ADEUDO_BIMESTRE_4";
                Mi_SQL += ", " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_5 + "-" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_5 + " AS ADEUDO_BIMESTRE_5";
                Mi_SQL += ", " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_6 + "-" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_6 + " AS ADEUDO_BIMESTRE_6";
                Mi_SQL += ", " + Ope_Pre_Adeudos_Predial.Campo_Monto_Por_Pagar;
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + " = '" + Adeudo.P_Cuenta_Predial_ID + "'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Adeudos_Predial.Campo_Monto_Por_Pagar + " != 0";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Adeudos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return tabla;
        }


        public static DataTable Consultar_Adeudos_Predial(Cls_Ope_Pre_Orden_Variacion_Negocio Adeudo)
        {
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Ope_Pre_Adeudos_Predial.Campo_Cuota_Anual;
                Mi_SQL += ", " + Ope_Pre_Adeudos_Predial.Campo_Bimestre_1;
                Mi_SQL += ", " + Ope_Pre_Adeudos_Predial.Campo_Bimestre_2;
                Mi_SQL += ", " + Ope_Pre_Adeudos_Predial.Campo_Bimestre_3;
                Mi_SQL += ", " + Ope_Pre_Adeudos_Predial.Campo_Bimestre_4;
                Mi_SQL += ", " + Ope_Pre_Adeudos_Predial.Campo_Bimestre_5;
                Mi_SQL += ", " + Ope_Pre_Adeudos_Predial.Campo_Bimestre_6;
                Mi_SQL += ", " + Ope_Pre_Adeudos_Predial.Campo_Anio + " AS AÑO";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + " = '" + Adeudo.P_Cuenta_Predial_ID + "'";
                //Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Adeudos_Predial.Campo_Monto_Por_Pagar + " != 0";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Adeudos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return tabla;
        }

        


        #endregion


        
        
        #region Metodos de Consulta Ordenes de Variacion
                

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Historial_Estatus_Ordenes_Variacion_Cuenta
        ///DESCRIPCIÓN          : Devuelve un DataTable con los registros de las Órdenes de Variación
        ///PARAMETROS           : Ordenes_Variacion, instancia de la capa de Negocios
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 20/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Historial_Estatus_Ordenes_Variacion_Cuenta(Cls_Ope_Pre_Orden_Variacion_Negocio Ordenes_Variacion)
        {
            DataTable Dt_Ordenes_Variacion = new DataTable();
            String Mi_SQL = "";
            try
            {
                Mi_SQL += "SELECT ";
                Mi_SQL += Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion + "." + Ope_Pre_Orden_Variacion.Campo_Cuenta_Predial_ID + ", ";
                Mi_SQL += Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion + "." + Ope_Pre_Orden_Variacion.Campo_Anio + ", ";
                Mi_SQL += Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion + "." + Ope_Pre_Orden_Variacion.Campo_No_Orden_Variacion + ", ";
                Mi_SQL += Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion + "." + Ope_Pre_Orden_Variacion.Campo_Fecha_Creo + ", ";
                Mi_SQL += Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion + "." + Ope_Pre_Orden_Variacion.Campo_Movimiento_ID + ", ";
                Mi_SQL += "(SELECT " + Cat_Pre_Movimientos.Campo_Identificador + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " WHERE " + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = " + Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion + "." + Ope_Pre_Orden_Variacion.Campo_Movimiento_ID + ") AS " + Cat_Pre_Movimientos.Campo_Identificador + ", ";
                Mi_SQL += "(SELECT " + Cat_Pre_Movimientos.Campo_Descripcion + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " WHERE " + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = " + Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion + "." + Ope_Pre_Orden_Variacion.Campo_Movimiento_ID + ") AS " + Cat_Pre_Movimientos.Campo_Descripcion + ", ";
                Mi_SQL += "(SELECT " + Cat_Pre_Movimientos.Campo_Identificador + " || ' - ' || " + Cat_Pre_Movimientos.Campo_Descripcion + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " WHERE " + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = " + Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion + "." + Ope_Pre_Orden_Variacion.Campo_Movimiento_ID + ") AS IDENTIFICADOR_DESCRIPCION, ";
                Mi_SQL += "NVL((SELECT " + Cat_Pre_Contribuyentes.Campo_Nombre + " || ' ' || " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || " + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes;
                Mi_SQL += " WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + "), ";
                Mi_SQL += "(SELECT " + Cat_Pre_Contribuyentes.Campo_Nombre + " || ' ' || " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || " + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes;
                Mi_SQL += " WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + "." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID + ")) AS NOMBRE_PROPIETARIO, ";
                Mi_SQL += Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion + "." + Ope_Pre_Orden_Variacion.Campo_Estatus;
                Mi_SQL += " FROM " + Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion;
                Mi_SQL += ", " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios;
                Mi_SQL += ", " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion;
                Mi_SQL += " WHERE ";
                Mi_SQL += Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion + "." + Ope_Pre_Orden_Variacion.Campo_Cuenta_Predial_ID + " = " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " AND ";
                Mi_SQL += Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion + "." + Ope_Pre_Orden_Variacion.Campo_Cuenta_Predial_ID + " = " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + "." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Cuenta_Predial_ID + " AND ";
                Mi_SQL += Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion + "." + Ope_Pre_Orden_Variacion.Campo_No_Orden_Variacion + " = " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + "." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_No_Orden_Variacion + " AND ";
                Mi_SQL += Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion + "." + Ope_Pre_Orden_Variacion.Campo_Anio + " = " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + "." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Anio + " AND ";
                if (Ordenes_Variacion.P_Cuenta_Predial_ID != "" && Ordenes_Variacion.P_Cuenta_Predial_ID != null)
                {
                    Mi_SQL += Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion + "." + Ope_Pre_Orden_Variacion.Campo_Cuenta_Predial_ID + " = '" + Ordenes_Variacion.P_Cuenta_Predial_ID + "' AND ";
                }
                if (Ordenes_Variacion.P_Cuenta_Predial != "" && Ordenes_Variacion.P_Cuenta_Predial != null)
                {
                    Mi_SQL += Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion + "." + Ope_Pre_Orden_Variacion.Campo_Cuenta_Predial_ID + " IN (SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE UPPER(" + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ")" + Validar_Operador_Comparacion(Ordenes_Variacion.P_Cuenta_Predial) + ") AND ";
                }
                if (Ordenes_Variacion.P_Contrarecibo != "" && Ordenes_Variacion.P_Contrarecibo != null)
                {
                    Mi_SQL += Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion + "." + Ope_Pre_Orden_Variacion.Campo_No_Contrarecibo + Validar_Operador_Comparacion(Ordenes_Variacion.P_Contrarecibo) + " AND ";
                }
                if (Ordenes_Variacion.P_Año != 0)
                {
                    Mi_SQL += Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion + "." + Ope_Pre_Orden_Variacion.Campo_Anio + " = " + Ordenes_Variacion.P_Año + " AND ";
                }
                Mi_SQL += Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion + "." + Ope_Pre_Orden_Variacion.Campo_Estatus + Validar_Operador_Comparacion(Ordenes_Variacion.P_Generar_Orden_Estatus) + " AND ";
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                Mi_SQL += " ORDER BY " + Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion + "." + Ope_Pre_Orden_Variacion.Campo_Anio + ", " + Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion + "." + Ope_Pre_Orden_Variacion.Campo_No_Orden_Variacion;

                DataSet Ds_Ordenes_Variacion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Ordenes_Variacion != null)
                {
                    Dt_Ordenes_Variacion = Ds_Ordenes_Variacion.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Órdenes de Variación. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Ordenes_Variacion;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Ultima_Orden_Con_Adeudos
        ///DESCRIPCIÓN          : Devuelve un DataTable con los registros de las Órdenes de Variación
        ///PARAMETROS           : Ordenes_Variacion, instancia de la capa de Negocios
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 13/Noviembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Ultima_Orden_Con_Adeudos(Cls_Ope_Pre_Orden_Variacion_Negocio Ordenes_Variacion)
        {
            DataTable Dt_Ordenes_Variacion = new DataTable();
            String Mi_SQL = "";
            try
            {
                Mi_SQL += "SELECT * FROM (";
                Mi_SQL += "SELECT ";
                Mi_SQL += Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_Anio + ", ";
                Mi_SQL += Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_No_Orden_Variacion;
                Mi_SQL += " FROM " + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + ", " + Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion;
                Mi_SQL += " WHERE ";
                Mi_SQL += Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_Cuenta_Predial_ID + " = " + Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion + "." + Ope_Pre_Orden_Variacion.Campo_Cuenta_Predial_ID + " AND ";
                Mi_SQL += Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_Anio + " = " + Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion + "." + Ope_Pre_Orden_Variacion.Campo_Anio + " AND ";
                Mi_SQL += Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_No_Orden_Variacion + " = " + Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion + "." + Ope_Pre_Orden_Variacion.Campo_No_Orden_Variacion + " AND ";
                Mi_SQL += Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_Cuenta_Predial_ID + " = '" + Ordenes_Variacion.P_Cuenta_Predial_ID + "' AND ";
                Mi_SQL += Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion + "." + Ope_Pre_Orden_Variacion.Campo_Estatus + " = 'ACEPTADA' AND ";
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                Mi_SQL += " ORDER BY " + Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion + "." + Ope_Pre_Orden_Variacion.Campo_Anio + " DESC, " + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_No_Orden_Variacion + " DESC";
                Mi_SQL += ") WHERE ROWNUM = 1";

                DataSet Ds_Ordenes_Variacion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Ordenes_Variacion != null)
                {
                    Dt_Ordenes_Variacion = Ds_Ordenes_Variacion.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Órdenes de Variación. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Ordenes_Variacion;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Ordene_Variacion
        ///DESCRIPCIÓN          : Actualiza el resitro indicado de la Orden de Variación
        ///PARAMETROS           : Ordenes_Variacion, instancia de la capa de Negocios
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 21/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Modificar_Orden_Variacion(Cls_Ope_Pre_Orden_Variacion_Negocio Ordenes_Variacion)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans = null;
            Boolean Actualizar = false;

            if (Ordenes_Variacion.P_Cmmd != null)
            {
                //Trans = Ordenes_Variacion.P_Trans;
                Cmd = Ordenes_Variacion.P_Cmmd;
            }
            else
            {
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Trans.Connection;
                Cmd.Transaction = Trans;
            }
            //Cmd.Connection = Trans.Connection;
            //Cmd.Transaction = Trans;
            try
            {
                String Mi_SQL = "UPDATE " + Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion + " SET ";
                if (Ordenes_Variacion.P_Generar_Orden_Anio != "" && Ordenes_Variacion.P_Generar_Orden_Anio != null)
                {
                    Mi_SQL += Ope_Pre_Orden_Variacion.Campo_Anio + " = " + Ordenes_Variacion.P_Generar_Orden_Anio + ", ";
                }
                if (Ordenes_Variacion.P_Generar_Orden_Estatus != "" && Ordenes_Variacion.P_Generar_Orden_Estatus != null)
                {
                    Mi_SQL += Ope_Pre_Orden_Variacion.Campo_Estatus + " = '" + Ordenes_Variacion.P_Generar_Orden_Estatus + "', ";
                }
                if (Ordenes_Variacion.P_Generar_Orden_Obserbaciones != "" && Ordenes_Variacion.P_Generar_Orden_Obserbaciones != null)
                {
                    Mi_SQL += Ope_Pre_Orden_Variacion.Campo_Observaciones + " = UPPER('" + Ordenes_Variacion.P_Generar_Orden_Obserbaciones + "'), ";
                }
                if (Ordenes_Variacion.P_Contrarecibo != "" && Ordenes_Variacion.P_Contrarecibo != null)
                {
                    Mi_SQL += Ope_Pre_Orden_Variacion.Campo_No_Contrarecibo + " = '" + Ordenes_Variacion.P_Contrarecibo + "', ";
                }
                if (Ordenes_Variacion.P_No_Nota != null)
                {
                    Mi_SQL += Ope_Pre_Orden_Variacion.Campo_No_Nota + " = " + Ordenes_Variacion.P_No_Nota + ", ";
                }
                if (Ordenes_Variacion.P_Fecha_Nota != null)
                {
                    Mi_SQL += Ope_Pre_Orden_Variacion.Campo_Fecha_Nota + " = '" + Ordenes_Variacion.P_Fecha_Nota.ToString("d-M-yyyy") + "', ";
                }
                if (Ordenes_Variacion.P_Grupo_Movimiento_ID != "" && Ordenes_Variacion.P_Grupo_Movimiento_ID != null)
                {
                    Mi_SQL += Ope_Pre_Orden_Variacion.Campo_Grupo_Movimiento_ID + " = '" + Ordenes_Variacion.P_Grupo_Movimiento_ID + "', ";
                }
                if (Ordenes_Variacion.P_Tipo_Predio_ID != "" && Ordenes_Variacion.P_Tipo_Predio_ID != null)
                {
                    Mi_SQL += Ope_Pre_Orden_Variacion.Campo_Tipo_Predio_ID + " = '" + Ordenes_Variacion.P_Tipo_Predio_ID + "', ";
                }
                if (Ordenes_Variacion.P_No_Nota_Impreso != "" && Ordenes_Variacion.P_No_Nota_Impreso != null)
                {
                    Mi_SQL += Ope_Pre_Orden_Variacion.Campo_Numero_Nota_Impreso + " = '" + Ordenes_Variacion.P_No_Nota_Impreso + "', ";
                }
                Mi_SQL += Ope_Pre_Orden_Variacion.Campo_Usuario_Modifico + " = '" + Ordenes_Variacion.P_Usuario + "', ";
                Mi_SQL += Ope_Pre_Orden_Variacion.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_SQL += "WHERE " + Ope_Pre_Orden_Variacion.Campo_No_Orden_Variacion + " = '" + Ordenes_Variacion.P_Orden_Variacion_ID + "' AND ";
                Mi_SQL += Ope_Pre_Orden_Variacion.Campo_Anio + "='" + Ordenes_Variacion.P_Año + "' AND ";
                Mi_SQL += Ope_Pre_Orden_Variacion.Campo_Cuenta_Predial_ID + "='" + Ordenes_Variacion.P_Cuenta_Predial_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                if (Ordenes_Variacion.P_Cmmd == null)
                {
                    Trans.Commit();
                }
                Actualizar = true;
            }
            catch (OracleException Ex)
            {
                if (Ordenes_Variacion.P_Cmmd == null)
                {
                    Trans.Rollback();
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
                    Mensaje = "Error al intentar modificar un Registro de Impuesto de Fraccionamiento. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Ordenes_Variacion.P_Cmmd == null)
                {
                    Cn.Close();
                }
            }
            return Actualizar;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Ordene_Variacion
        ///DESCRIPCIÓN          : Actualiza el resitro indicado de la Orden de Variación
        ///PARAMETROS           : Ordenes_Variacion, instancia de la capa de Negocios
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 21/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Insertar_Observaciones_Variacion(Cls_Ope_Pre_Orden_Variacion_Negocio Ordenes_Variacion)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans = null;
            Boolean Actualizar = false;

            if (Ordenes_Variacion.P_Cmmd != null)
            {
                //Trans = Ordenes_Variacion.P_Trans;
                Cmd = Ordenes_Variacion.P_Cmmd;
            }
            else
            {
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Trans.Connection;
                Cmd.Transaction = Trans;
            }
            //Cmd.Connection = Trans.Connection;
            //Cmd.Transaction = Trans;
            try
            {
                //Inserta las Observaciones de la Validación
                String Mi_SQL = "UPDATE " + Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion + " SET ";
                String Observacion_ID = Obtener_ID_Consecutivo(Ope_Pre_Observaciones.Tabla_Ope_Pre_Observaciones_Orden_Variacion, Ope_Pre_Observaciones.Campo_Observaciones_ID, "", 5);
                Mi_SQL = "INSERT INTO " + Ope_Pre_Observaciones.Tabla_Ope_Pre_Observaciones_Orden_Variacion + " (";
                Mi_SQL += Ope_Pre_Observaciones.Campo_Observaciones_ID + ", ";
                Mi_SQL += Ope_Pre_Observaciones.Campo_No_Orden_Variacion + ", ";
                Mi_SQL += Ope_Pre_Observaciones.Campo_Año + ", ";
                Mi_SQL += Ope_Pre_Observaciones.Campo_Descripcion + ", ";
                Mi_SQL += Ope_Pre_Observaciones.Campo_Usuario_Creo + ", ";
                Mi_SQL += Ope_Pre_Observaciones.Campo_Fecha_Creo + ") ";
                Mi_SQL += "VALUES (";
                Mi_SQL += "'" + Observacion_ID + "', ";
                Mi_SQL += "'" + Ordenes_Variacion.P_Observaciones_No_Orden_Variacion + "', ";
                Mi_SQL += Ordenes_Variacion.P_Observaciones_Año + ", ";
                if (Ordenes_Variacion.P_Observaciones_Descripcion != null && Ordenes_Variacion.P_Observaciones_Descripcion != "")
                {
                    Mi_SQL += " UPPER('" + Ordenes_Variacion.P_Observaciones_Descripcion + "'), ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                Mi_SQL += "'" + Ordenes_Variacion.P_Observaciones_Usuraio + "', SYSDATE) ";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                if (Ordenes_Variacion.P_Cmmd == null)
                {
                    Trans.Commit();
                }
                Actualizar = true;
            }
            catch (OracleException Ex)
            {
                if (Ordenes_Variacion.P_Cmmd == null)
                {
                    Trans.Rollback();
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
                    Mensaje = "Error al intentar modificar un Registro de Impuesto de Fraccionamiento. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Ordenes_Variacion.P_Cmmd == null)
                {
                    Cn.Close();
                }
            }
            return Actualizar;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Contrarecibo
        ///DESCRIPCIÓN          : Actualiza el resitro indicado del Contrarecibo
        ///PARAMETROS           : Ordenes_Variacion, instancia de la capa de Negocios
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 21/Septiembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Modificar_Contrarecibo(Cls_Ope_Pre_Orden_Variacion_Negocio Ordenes_Variacion)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans = null;
            Boolean Actualizar = false;

            if (Ordenes_Variacion.P_Cmmd != null)
            {
                Cmd = Ordenes_Variacion.P_Cmmd;
            }
            else
            {
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Trans.Connection;
                Cmd.Transaction = Trans;
            }
            try
            {
                String Mi_SQL = "UPDATE " + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + " SET ";
                if (Ordenes_Variacion.P_Contrarecibo_Cuenta_Predial_ID != "" && Ordenes_Variacion.P_Contrarecibo_Cuenta_Predial_ID != null)
                {
                    Mi_SQL += Ope_Pre_Contrarecibos.Campo_Cuenta_Predial_ID + " = '" + Ordenes_Variacion.P_Contrarecibo_Cuenta_Predial_ID + "', ";
                }
                if (Ordenes_Variacion.P_Contrarecibo_No_Escritoria != 0)
                {
                    Mi_SQL += Ope_Pre_Contrarecibos.Campo_No_Escritura + " = " + Ordenes_Variacion.P_Contrarecibo_No_Escritoria + ", ";
                }
                if (Ordenes_Variacion.P_Contrarecibo_Fecha_Escritura > DateTime.MinValue && Ordenes_Variacion.P_Contrarecibo_Fecha_Escritura != null)
                {
                    Mi_SQL += Ope_Pre_Contrarecibos.Campo_Fecha_Escritura + " = '" + Ordenes_Variacion.P_Contrarecibo_Fecha_Escritura.ToString("d-M-yyyy") + "', ";
                }
                if (Ordenes_Variacion.P_Contrarecibo_Fecha_Liberacion > DateTime.MinValue && Ordenes_Variacion.P_Contrarecibo_Fecha_Liberacion != null)
                {
                    Mi_SQL += Ope_Pre_Contrarecibos.Campo_Fecha_Liberacion + " = '" + Ordenes_Variacion.P_Contrarecibo_Fecha_Liberacion.ToString("d-M-yyyy") + "', ";
                }
                if (Ordenes_Variacion.P_Contrarecibo_Fecha_Pago > DateTime.MinValue && Ordenes_Variacion.P_Contrarecibo_Fecha_Pago != null)
                {
                    Mi_SQL += Ope_Pre_Contrarecibos.Campo_Fecha_Pago + " = '" + Ordenes_Variacion.P_Contrarecibo_Fecha_Pago.ToString("d-M-yyyy") + "', ";
                }
                if (Ordenes_Variacion.P_Contrarecibo_Estatus != "" && Ordenes_Variacion.P_Contrarecibo_Estatus != null)
                {
                    Mi_SQL += Ope_Pre_Contrarecibos.Campo_Estatus + " = '" + Ordenes_Variacion.P_Contrarecibo_Estatus + "', ";
                }
                if (Ordenes_Variacion.P_Contrarecibo_Notario_ID != "" && Ordenes_Variacion.P_Contrarecibo_Notario_ID != null)
                {
                    Mi_SQL += Ope_Pre_Contrarecibos.Campo_Notario_ID + " = '" + Ordenes_Variacion.P_Contrarecibo_Notario_ID + "', ";
                }
                if (Ordenes_Variacion.P_Contrarecibo_Listado_ID != "" && Ordenes_Variacion.P_Contrarecibo_Listado_ID != null)
                {
                    Mi_SQL += Ope_Pre_Contrarecibos.Campo_Listado_ID + " = '" + Ordenes_Variacion.P_Contrarecibo_Listado_ID + "', ";
                }
                //if (Ordenes_Variacion.P_Contrarecibo_Anio != 0)
                //{
                //    Mi_SQL += Ope_Pre_Contrarecibos.Campo_Anio + " = " + Ordenes_Variacion.P_Contrarecibo_Anio + ", ";
                //}
                Mi_SQL += Ope_Pre_Contrarecibos.Campo_Usuario_Modifico + " = '" + Ordenes_Variacion.P_Contrarecibo_Usuario + "', ";
                Mi_SQL += Ope_Pre_Contrarecibos.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_SQL += "WHERE " + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " = '" + Ordenes_Variacion.P_Contrarecibo_No_Contrarecibo + "' ";
                Mi_SQL += "AND " + Ope_Pre_Contrarecibos.Campo_Anio + " = " + Ordenes_Variacion.P_Contrarecibo_Anio;
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                if (Ordenes_Variacion.P_Cmmd == null)
                {
                    Trans.Commit();
                }
                Actualizar = true;
            }
            catch (OracleException Ex)
            {
                if (Ordenes_Variacion.P_Cmmd == null)
                {
                    Trans.Rollback();
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
                    Mensaje = "Error al intentar modificar un Registro de Impuesto de Fraccionamiento. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Ordenes_Variacion.P_Cmmd == null)
                {
                    Cn.Close();
                }
            }
            return Actualizar;
        }

        //PONER EN LA CAPA DE DATOS
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Calculo_Traslado
        ///DESCRIPCIÓN          : Actualiza el registro indicado del calculo
        ///PARAMETROS           : Ordenes_Variacion, instancia de la capa de Negocios
        ///CREO                 : Ismael Prieto Sanchez
        ///FECHA_CREO           : 6/Octubre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Modificar_Calculo_Traslado(Cls_Ope_Pre_Orden_Variacion_Negocio Ordenes_Variacion)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans = null;
            Boolean Actualizar = false;

            if (Ordenes_Variacion.P_Cmmd != null)
            {
                Cmd = Ordenes_Variacion.P_Cmmd;
            }
            else
            {
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Trans.Connection;
                Cmd.Transaction = Trans;
            }
            try
            {
                String Mi_SQL = "UPDATE " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + " SET ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + " = '" + Ordenes_Variacion.P_Contrarecibo_Estatus + "', ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Usuario_Modifico + " = '" + Ordenes_Variacion.P_Contrarecibo_Usuario + "', ";
                Mi_SQL += Ope_Pre_Calculo_Imp_Traslado.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_SQL += "WHERE " + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion + " = '" + Ordenes_Variacion.P_Orden_Variacion_ID + "' ";
                Mi_SQL += "AND " + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + " = " + Ordenes_Variacion.P_Año;
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                if (Ordenes_Variacion.P_Cmmd == null)
                {
                    Trans.Commit();
                }
                Actualizar = true;
            }
            catch (OracleException Ex)
            {
                if (Ordenes_Variacion.P_Cmmd == null)
                {
                    Trans.Rollback();
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
                    Mensaje = "Error al intentar modificar un Registro de Impuesto de Fraccionamiento. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Ordenes_Variacion.P_Cmmd == null)
                {
                    Cn.Close();
                }
            }
            return Actualizar;
        }        

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
                if (Filtro.Trim().ToUpper().StartsWith("NULL")
                    || Filtro.Trim().ToUpper().StartsWith("NOT NULL"))
                {
                    Cadena_Validada = " IS " + Filtro + " ";
                }
                else
                {
                    Cadena_Validada = " = '" + Filtro + "' ";
                }
            }
            return Cadena_Validada;
        }



        #endregion

        
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Alta_Beneficio_Couta_Fija
        ///DESCRIPCIÓN: Genera un beneficio
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 22/Ago/2011 8:03:39 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************* 
        internal static string Alta_Beneficio_Couta_Fija(Cls_Ope_Pre_Orden_Variacion_Negocio Datos)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL
            String Consec = ""; //Variable para la consulta SQL
            Object Aux; //Variable auxiliar para las consultas  

            try
            {
                //Formar Sentencia para obtener el consecutivo de la orden                
                Mi_SQL = "";
                Mi_SQL = "SELECT NVL(MAX(";
                Mi_SQL = Mi_SQL + Ope_Pre_Cuotas_Fijas.Campo_No_Cuota_Fija + "),0000000000)";
                Mi_SQL = Mi_SQL + " FROM ";
                Mi_SQL = Mi_SQL + Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas;

                //Ejecutar consulta del consecutivo               
                Aux = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0][0];

                //Verificar si no es nulo
                if (Convert.IsDBNull(Aux) == false)
                {
                    Consec = String.Format("{0:0000000000}", Convert.ToInt32(Aux) + 1);
                }
                else
                    Consec = "0000000001";

                Mi_SQL = "";
                Mi_SQL = "INSERT INTO ";
                Mi_SQL = Mi_SQL + Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + " ( ";
                Mi_SQL = Mi_SQL + Ope_Pre_Cuotas_Fijas.Campo_No_Cuota_Fija + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Cuotas_Fijas.Campo_Plazo_Financiamiento + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Cuotas_Fijas.Campo_Exedente_Construccion + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Cuotas_Fijas.Campo_Exedente_Valor + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Cuotas_Fijas.Campo_Total_Cuota_Fija + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Cuotas_Fijas.Campo_Caso_Especial_Id + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Cuotas_Fijas.Campo_Tasa_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Cuotas_Fijas.Campo_Tasa_Valor + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Cuotas_Fijas.Campo_Cuota_Minima + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Cuotas_Fijas.Campo_Excedente_Construccion_Total + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Cuotas_Fijas.Campo_Excedente_Valor_Total + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Cuotas_Fijas.Campo_Cuenta_Predial_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Cuotas_Fijas.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Cuotas_Fijas.Campo_Fecha_Creo + ") VALUES('";
                Mi_SQL = Mi_SQL + Consec;
                Mi_SQL = Mi_SQL + "','" + Datos.P_Cuota_Fija_Plazo + "";
                Mi_SQL = Mi_SQL + "','" + Datos.P_Cuota_Fija_Excedente_Cons.Replace(",", "") + "";
                Mi_SQL = Mi_SQL + "','" + Datos.P_Cuota_Fija_Excedente_Valor.Replace(",", "") + "";
                Mi_SQL = Mi_SQL + "','" + Datos.P_Cuota_Fija_Total.Replace(",", "") + "";
                Mi_SQL = Mi_SQL + "','" + Datos.P_Cuota_Fija_Caso_Especial + "";
                Mi_SQL = Mi_SQL + "','" + Datos.P_Cuota_Fija_Tasa_ID + "";
                Mi_SQL = Mi_SQL + "','" + Datos.P_Cuota_Fija_Tasa_Valor.Replace(",", "") + "";
                Mi_SQL = Mi_SQL + "','" + Datos.P_Cuota_Fija_Cuota_Minima.Replace(",", "") + "";
                Mi_SQL = Mi_SQL + "','" + Datos.P_Cuota_Fija_Excedente_Cons_Total.Replace(",", "") + "";
                Mi_SQL = Mi_SQL + "','" + Datos.P_Cuota_Fija_Excedente_Valor_Total.Replace(",", "") + "";
                if (!String.IsNullOrEmpty( Datos.P_Cuenta_Predial_ID))
                    Mi_SQL = Mi_SQL + "','" + Datos.P_Cuenta_Predial_ID + "";
                else
                    Mi_SQL = Mi_SQL + "', NULL " + ",'";
                Mi_SQL = Mi_SQL + "','" + Presidencia.Sessiones.Cls_Sessiones.Nombre_Empleado + "',SYSDATE )";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                return Consec;

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

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : Antonio Salvador Benavides Guardado
        ///FECHA_MODIFICO       : 26/Octubre/2010
        ///CAUSA_MODIFICACIÓN   : Estandarizar variables usadas
        ///*******************************************************************************
        public static String Obtener_ID_Consecutivo(String Tabla, String Campo, String Filtro, Int32 Longitud_ID)
        {
            String Id = Convert.ToString(1).PadLeft(Longitud_ID, '0');
            try
            {
                String Mi_SQL = "SELECT MAX(" + Campo + ") FROM " + Tabla;
                if (Filtro != "" && Filtro != null)
                {
                    Mi_SQL += " WHERE " + Filtro;
                }
                Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals(""))
                {
                    Id = Convert.ToString((Convert.ToInt32(Obj_Temp) + 1)).PadLeft(Longitud_ID, '0');
                }
            }
            catch (OracleException Ex)
            {
                new Exception(Ex.Message);
            }
            return Id;
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Calles_Sin_Colonia
        ///DESCRIPCIÓN: cosnulta el nombre de la calle
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 22/Ago/2011 8:03:39 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************* 
        internal static DataTable Consulta_Calles_Sin_Colonia(string Calle_ID)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL            

            try
            {
                Mi_SQL = "";
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Cat_Pre_Calles.Campo_Calle_ID + " AS ID, ";
                Mi_SQL = Mi_SQL + Cat_Pre_Calles.Campo_Nombre + " AS DESCRIPCION";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Calles.Campo_Colonia_ID + " in (SELECT ";
                Mi_SQL = Mi_SQL + Cat_Pre_Calles.Campo_Colonia_ID + " FROM " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " WHERE ";
                Mi_SQL = Mi_SQL + Cat_Pre_Calles.Campo_Calle_ID + " = '" + Calle_ID + "')";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

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
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consultar_Valor_Tasa
        ///DESCRIPCIÓN: Consulta el valor de una tasa
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 22/Ago/2011 8:03:39 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************* 
        internal static string Consultar_Valor_Tasa(string p)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL            

            try
            {
                Mi_SQL = "";
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Anual + " ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_ID + " = '" + p + "'";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0][0].ToString();

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


        internal static string Consultar_Historial(string p)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL            
            String Res = "";
            DataTable Dt_Res;
            try
            {
                Mi_SQL = "";
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Ope_Pre_Observaciones.Campo_Descripcion + " ";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Observaciones.Tabla_Ope_Pre_Observaciones_Orden_Variacion;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Observaciones.Campo_No_Orden_Variacion + " = '" + p + "'";

                Dt_Res = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                if (Dt_Res.Rows.Count > 0)
                    Res = Dt_Res.Rows[0][0].ToString();
                else
                    Res = "";
                return Res;
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
