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
using Presidencia.Compra_Directa.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Orden_Compra.Datos;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Generar_Requisicion.Negocio;
namespace Presidencia.Compra_Directa.Datos
{
    public class Cls_Ope_Com_Compra_Directa_Datos
    {
        #region MÉTODOS
        public Cls_Ope_Com_Compra_Directa_Datos()
        {
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Requisicion
        ///DESCRIPCIÓN: Obtiene un DataTable de una consulta de la tabla de Ope_Com_Requisiciones
        ///PARAMETROS: 1.-Objeto de negocio con datos
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 11/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Requisiciones_Filtradas(Cls_Ope_Com_Compra_Directa_Negocio Negocio)
        {
            double Monto_Inicio = 0;
            double Monto_Fin = 0;
            String Mi_Sql = "";
            Mi_Sql = "SELECT " +
                Cat_Com_Monto_Proceso_Compra.Campo_Monto_Compra_Directa_Ini + ", " +
                Cat_Com_Monto_Proceso_Compra.Campo_Monto_Compra_Directa_Fin +
                " FROM " +
                Cat_Com_Monto_Proceso_Compra.Tabla_Cat_Com_Monto_Proceso_Compra +
                " WHERE " +
                Cat_Com_Monto_Proceso_Compra.Campo_Tipo + " = '" + Negocio.P_Tipo_Articulo + "'";
            DataSet _DSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);             
            DataTable Dt_Tmp = null;
            if (_DSet != null && _DSet.Tables.Count > 0 && _DSet.Tables[0].Rows.Count > 0)
            {
                Dt_Tmp = _DSet.Tables[0];
                Monto_Inicio = Convert.ToDouble(Dt_Tmp.Rows[0]["MONTO_COMPRA_DIRECTA_INI"].ToString().Trim());
                Monto_Fin = Convert.ToDouble(Dt_Tmp.Rows[0]["MONTO_COMPRA_DIRECTA_FIN"].ToString().Trim());
            }
            Mi_Sql = "SELECT " +
            Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + ".*," +
                "(SELECT " + Cat_Dependencias.Campo_Nombre +
                " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias +
                " WHERE " + Cat_Dependencias.Campo_Dependencia_ID +
                "=" + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID + ") AS NOMBRE_DEPENDENCIA " +
            " FROM " +
            Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
            " WHERE " +
            Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Estatus +
            " IN ('" + Negocio.P_Estatus_Requisicion + "')" + 
            
            //" AND " +            
            //Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Tipo_Articulo +
            //" = '" + Negocio.P_Tipo_Articulo +             
            //"' AND " +
            //Ope_Com_Requisiciones.Campo_Total + " >= " + Monto_Inicio + " AND " +
            //Ope_Com_Requisiciones.Campo_Total + " <= " + Monto_Fin +

            //Ope_Com_Requisiciones.Campo_Total + " <= 10000" +

            " ORDER BY " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " DESC";
            DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            DataTable _DataTable = null;
            if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
            {
                _DataTable = _DataSet.Tables[0];
            }
            return _DataTable;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Guardar_Actualizacion_Precios_Proveedor
        ///DESCRIPCIÓN: ACtualiza a estatus de Cotizada la REquisicion
        ///PARAMETROS: 1.-Objeto de negocio con datos
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 11/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Guardar_Actualizacion_Precios_Proveedor(Cls_Ope_Com_Compra_Directa_Negocio Negocio)
        {
            //Actualizar Requisicion
            //Cls_Ope_Com_Orden_Compra_Datos.
            //Obtener_Consecutivo(Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra, Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra);
            String Mi_Sql = "";
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;                                                  //Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        
            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos
            try
            {
                Mi_Sql = "UPDATE " +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                " SET " +
                Ope_Com_Requisiciones.Campo_Fecha_Cotizacion + " = SYSDATE, " +
                Ope_Com_Requisiciones.Campo_Empleado_Cotizacion_ID + " = '" + Cls_Sessiones.Empleado_ID + "', " +
                Ope_Com_Requisiciones.Campo_Cotizador_ID + " = '" + Cls_Sessiones.Empleado_ID + "', " +
                Ope_Com_Requisiciones.Campo_Estatus + " = '" + "CONFIRMADA" + "', " +
                Ope_Com_Requisiciones.Campo_Tipo_Compra + " = 'COMPRA DIRECTA', " +
                Ope_Com_Requisiciones.Campo_Subtotal_Cotizado + " = " + Negocio.P_Subtotal_Cotizado + ", " +
                Ope_Com_Requisiciones.Campo_IEPS_Cotizado + " = " + Negocio.P_IEPS_Cotizado + ", " +
                Ope_Com_Requisiciones.Campo_IVA_Cotizado + " = " + Negocio.P_IVA_Cotizado + ", " +
                Ope_Com_Requisiciones.Campo_Total_Cotizado + " = " + Negocio.P_Total_Cotizado + 
                " WHERE " +
                Ope_Com_Requisiciones.Campo_Requisicion_ID + " = '" + Negocio.P_No_Requisicion + "'";
                Comando_SQL.CommandText = Mi_Sql;
                Comando_SQL.ExecuteNonQuery();
                //OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                //Actualizar detalles
                foreach (DataRow Renglon in Negocio.P_Dt_Detalles_Orden_Compra.Rows)
                {
                    Mi_Sql = "UPDATE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " SET " +
                    Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado + " = '" + Renglon["Precio_U_Sin_Imp_Cotizado"].ToString().Trim() + "', " +
                    Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado + " = '" + Renglon["Precio_U_Con_Imp_Cotizado"].ToString().Trim() + "', " +
                    Ope_Com_Req_Producto.Campo_Subtota_Cotizado + " = '" + Renglon["Subtotal_Cotizado"].ToString().Trim() + "', " +
                    Ope_Com_Req_Producto.Campo_IEPS_Cotizado + " = '" + Renglon["IEPS_Cotizado"].ToString().Trim() + "', " +
                    Ope_Com_Req_Producto.Campo_IVA_Cotizado + " = '" + Renglon["IVA_Cotizado"].ToString().Trim() + "', " +
                    Ope_Com_Req_Producto.Campo_Total_Cotizado + " = '" + Renglon["Total_Cotizado"].ToString().Trim() + "', " +
                    Ope_Com_Req_Producto.Campo_Proveedor_ID + " = '" + Renglon["Proveedor_ID"].ToString().Trim() + "', " +
                    Ope_Com_Req_Producto.Campo_Nombre_Proveedor + " = '" + Renglon["Nombre_Proveedor"].ToString().Trim() + "' " +
                    " WHERE " +
                    Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID + " = '" + Renglon["Ope_Com_Req_Producto_ID"].ToString().Trim() + "' ";
                    Comando_SQL.CommandText = Mi_Sql;
                    Comando_SQL.ExecuteNonQuery();
                }            
                Transaccion_SQL.Commit();
            }
            catch (OracleException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }          
                throw new Exception("Información: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Los datos fueron actualizados por otro Usuario. Información: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Información: " + Ex.Message);
            }
            finally
            {
                Conexion_Base.Close();
            }
            return 0;
        }

        public static DataTable Consultar_Articulos_Requisiciones_Filtradas(Cls_Ope_Com_Compra_Directa_Negocio Negocio)
        {
            String Mi_Sql = "";
            Mi_Sql = "SELECT * FROM " +
            Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
            " WHERE " + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = '" + Negocio.P_No_Requisicion + "'";
            DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            DataTable _DataTable = null;
            if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
            {
                _DataTable = _DataSet.Tables[0];
            }
            return _DataTable;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Proveedores_Por_Giro()
        ///DESCRIPCIÓN: Obtiene un DataTable de una consulta con
        ///los servicios de las requisas
        ///PARAMETROS: 1.-Objeto de negocio con datos
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 10/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Proveedores_Por_Concepto(Cls_Ope_Com_Compra_Directa_Negocio Negocio)
        {
        //SELECT DISTINCT(PROVEEDOR_ID), (SELECT NOMBRE FROM CAT_COM_PROVEEDORES WHERE PROVEEDOR_ID = CAT_COM_GIRO_PROVEEDOR
        //.PROVEEDOR_ID) AS NOMBRE FROM CAT_COM_GIRO_PROVEEDOR WHERE GIRO_ID IN ('00001','00004')

            String Mi_Sql = "";
            Mi_Sql = "SELECT DISTINCT(" + Cat_Com_Giro_Proveedor.Campo_Proveedor_ID + "), " +
                "(SELECT " + Cat_Com_Proveedores.Campo_Nombre + " FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores +
                " WHERE " + Cat_Com_Proveedores.Campo_Proveedor_ID + " = " + Cat_Com_Giro_Proveedor.Tabla_Cat_Com_Giro_Proveedor +
                "." + Cat_Com_Giro_Proveedor.Campo_Proveedor_ID + ")" +
                " AS NOMBRE " +
            "FROM " + Cat_Com_Giro_Proveedor.Tabla_Cat_Com_Giro_Proveedor +
            " WHERE " + Cat_Com_Giro_Proveedor.Campo_Giro_ID + " IN (" + Negocio.P_Giro_ID + ")";
            
            DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            DataTable _DataTable = null;
            if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
            {
                _DataTable = _DataSet.Tables[0];
            }
            return _DataTable;

        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Conceptos_De_Partidas
        ///DESCRIPCIÓN: Obtiene un DataTable de una consulta con
        ///los conceptos de una partida especifica
        ///PARAMETROS: 1.-Objeto de negocio con datos
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 19/Abril/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Conceptos_De_Partidas(Cls_Ope_Com_Compra_Directa_Negocio Negocio) 
        {
            //SELECT DISTINCT (CAT_SAP_CONCEPTO.CONCEPTO_ID),CAT_SAP_CONCEPTO.CLAVE,CAT_SAP_CONCEPTO.DESCRIPCION AS NOMBRE, CAT_SAP_CONCEPTO.CLAVE 
            //||' '||CAT_SAP_CONCEPTO.DESCRIPCION AS CLAVE_NOMBRE FROM 
            //CAT_SAP_PARTIDAS_ESPECIFICAS JOIN CAT_SAP_PARTIDA_GENERICA
            //ON CAT_SAP_PARTIDAS_ESPECIFICAS.PARTIDA_GENERICA_ID = CAT_SAP_PARTIDA_GENERICA.PARTIDA_GENERICA_ID
            //JOIN CAT_SAP_CONCEPTO ON CAT_SAP_PARTIDA_GENERICA.CONCEPTO_ID = CAT_SAP_CONCEPTO.CONCEPTO_ID
            String Mi_Sql = "";
            Mi_Sql = "SELECT DISTINCT(" +
                Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Concepto_ID + "), " +
                Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Clave + ", " +
                Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Descripcion + ", " +
                Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Clave + " ||' '|| " +
                Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Descripcion + " AS CLAVE_NOMBRE" +
                " FROM " +
                Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas +
                " JOIN " +
                Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas +
                " ON " +
                Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." +
                Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID + " = " +
                Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + "." +
                Cat_Sap_Partidas_Genericas.Campo_Partida_Generica_ID +
                " JOIN " +
                Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto +
                " ON " +
                Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + "." +
                Cat_Sap_Partidas_Genericas.Campo_Concepto_ID + " = " +
                Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Concepto_ID +
                " WHERE " +
                Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." +
                Cat_Sap_Partidas_Especificas.Campo_Partida_ID + " IN (" + Negocio.P_Lista_Partidas +")";

            DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            DataTable _DataTable = null;
            if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
            {
                _DataTable = _DataSet.Tables[0];
            }
            return _DataTable;

        }

        #endregion
    }
}
