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
using Presidencia.Polizas_Stock.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
/// <summary>
/// Summary description for Cls_Ope_Com_Polizas_Stock_Datos
/// </summary>
namespace Presidencia.Polizas_Stock.Datos
{
    public class Cls_Ope_Com_Polizas_Stock_Datos
    {
        #region MÉTODOS
        public Cls_Ope_Com_Polizas_Stock_Datos()
        {
        }
        public static DataTable Consultar_Ordenes_Salida_Stock(Cls_Ope_Com_Polizas_Stock_Negocio Negocio) 
        {
            Negocio.P_Fecha_Inicio = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Negocio.P_Fecha_Inicio));
            Negocio.P_Fecha_Fin = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Negocio.P_Fecha_Fin));
            try
            {
                String Mi_SQL = "";
                Mi_SQL =
                "SELECT " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + ".*, " +
                "(SELECT " + Cat_Dependencias.Campo_Nombre + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias +
                " WHERE " + Cat_Dependencias.Campo_Dependencia_ID + " = " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + "." +
                Alm_Com_Salidas.Campo_Dependencia_ID + ") UNIDAD_RESP " +

                ",(SELECT " + Cat_Con_Cuentas_Contables.Campo_Cuenta + " FROM " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables +
                " WHERE " + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID + " = (SELECT CUENTA_CONTABLE_ID FROM " +
                Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + " WHERE " + Cat_Sap_Partidas_Especificas.Campo_Partida_ID +
                " = " +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Partida_ID
                + ")) AS CUENTA" +

                "," + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Codigo_Programatico +
                "," + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Elemento_PEP +
                "," + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + ".NO_RESERVA" +
                " FROM " +
                Alm_Com_Salidas.Tabla_Alm_Com_Salidas +
                " JOIN " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                " ON " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + "." + Alm_Com_Salidas.Campo_Requisicion_ID + " = " +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID +

                " WHERE " +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Tipo + " = 'STOCK' ";// +

                if (!String.IsNullOrEmpty(Negocio.P_Contabilizada))
                {
                    Mi_SQL += " AND " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + ".CONTABILIZADO = '" + Negocio.P_Contabilizada + "'"; //+
                }
                if (!String.IsNullOrEmpty(Negocio.P_Fecha_Inicio) && !String.IsNullOrEmpty(Negocio.P_Fecha_Fin))
                {
                    Mi_SQL += " AND TO_DATE(TO_CHAR(" + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + "." + Alm_Com_Salidas.Campo_Fecha_Creo + ",'DD-MM-YYYY'))" +
                    " >= '" + Negocio.P_Fecha_Inicio + "' AND " +
                    "TO_DATE(TO_CHAR(" + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + "." + Alm_Com_Salidas.Campo_Fecha_Creo + ",'DD-MM-YYYY'))" +
                    " <= '" + Negocio.P_Fecha_Fin + "'";
                }
                if (!String.IsNullOrEmpty(Negocio.P_No_Salida))
                {
                    Mi_SQL += " AND " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + "." + Alm_Com_Salidas.Campo_No_Salida + " = " + Negocio.P_No_Salida;
                }
                Mi_SQL += " ORDER BY " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + "." + Alm_Com_Salidas.Campo_No_Salida;
                
                DataTable Data_Table =
                    OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                return Data_Table;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }
        }
        //Consultar polizas 
        public static DataTable Consultar_Polizas_Stock_Generadas_Para_SAP(Cls_Ope_Com_Polizas_Stock_Negocio Negocio)
        {
            Negocio.P_Fecha_Inicio = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Negocio.P_Fecha_Inicio));
            Negocio.P_Fecha_Fin = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Negocio.P_Fecha_Fin));
            try
            {
                String Mi_SQL = "SELECT * FROM " + Poliza_Stock_Para_Sap.Tabla_Poliza_Stock_Para_Sap +
                    " WHERE " +
                    "TO_DATE(TO_CHAR(" + Poliza_Stock_Para_Sap.Campo_Fecha_Creo + ",'DD-MM-YYYY'))" +
                    " >= '" + Negocio.P_Fecha_Inicio + "' AND " +
                    "TO_DATE(TO_CHAR(" + Poliza_Stock_Para_Sap.Campo_Fecha_Creo + ",'DD-MM-YYYY'))" +
                    " <= '" + Negocio.P_Fecha_Fin + "' ORDER BY " + Poliza_Stock_Para_Sap.Campo_Fecha_Creo + " DESC";
                DataTable _DataTable =
                    OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                return _DataTable;
            }
            catch (Exception Ex)
            {
                Ex.ToString();
                return null;
            }            
        }

        //Para consultar salidas de un poliza ya generada, por páreametro debe traer seteado el no_poliza_stock
        public static DataTable Consultar_Ordenes_Salida_Stock_Por_Poliza_Generada(Cls_Ope_Com_Polizas_Stock_Negocio Negocio)
        {
            try
            {
                String Mi_SQL = "";
                Mi_SQL =
                "SELECT " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + ".*, " +
                "(SELECT " + Cat_Dependencias.Campo_Nombre + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias +
                " WHERE " + Cat_Dependencias.Campo_Dependencia_ID + " = " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + "." +
                Alm_Com_Salidas.Campo_Dependencia_ID + ") UNIDAD_RESP " +

                ",(SELECT " + Cat_Con_Cuentas_Contables.Campo_Cuenta + " FROM " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables +
                " WHERE " + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID + " = (SELECT CUENTA_CONTABLE_ID FROM " +
                Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + " WHERE " + Cat_Sap_Partidas_Especificas.Campo_Partida_ID +
                " = " +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Partida_ID
                + ")) AS CUENTA" +

                "," + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Codigo_Programatico +
                "," + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Elemento_PEP +
                "," + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + ".NO_RESERVA" +
                " FROM " +
                Alm_Com_Salidas.Tabla_Alm_Com_Salidas +
                " JOIN " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                " ON " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + "." + Alm_Com_Salidas.Campo_Requisicion_ID + " = " +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID +

                " WHERE " +
                Alm_Com_Salidas.Tabla_Alm_Com_Salidas + ".POLIZA_SAP = " + Negocio.P_No_Poliza_Stock +
                " ORDER BY " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + "." +
                Alm_Com_Salidas.Campo_No_Salida;

                DataTable Data_Table =
                    OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                return Data_Table;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }
        }

        public static int Guardar_Poliza_SAP_Stock(Cls_Ope_Com_Polizas_Stock_Negocio Negocio)
        {
            int Registros_Afectados = 0;
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
                int Consecutivo = Obtener_Consecutivo(Poliza_Stock_Para_Sap.Campo_No_Poliza_Stock,Poliza_Stock_Para_Sap.Tabla_Poliza_Stock_Para_Sap);
                String Mi_SQL = "";
                Mi_SQL =
                "INSERT INTO " + Poliza_Stock_Para_Sap.Tabla_Poliza_Stock_Para_Sap +
                " (" +
                Poliza_Stock_Para_Sap.Campo_No_Poliza_Stock + "," +
                Poliza_Stock_Para_Sap.Campo_Salidas + "," +
                Poliza_Stock_Para_Sap.Campo_Hora + "," +
                Poliza_Stock_Para_Sap.Campo_Importe + "," +
                Poliza_Stock_Para_Sap.Campo_Usuario_Creo + "," +
                Poliza_Stock_Para_Sap.Campo_Fecha_Creo + ") VALUES (" +
                Consecutivo +
                ",'" + Negocio.P_Lista_Salidas + "'," +
                "SYSTIMESTAMP," + Negocio.P_Importe + ",'" + Cls_Sessiones.Nombre_Empleado + "',SYSDATE)";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Mi_SQL = "UPDATE " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + " SET " + Alm_Com_Salidas.Campo_Poliza_Stock_SAP +
                    " = " + Consecutivo + "," + Alm_Com_Salidas.Campo_Contabilizado + " = 'SI'" +
                " WHERE " + Alm_Com_Salidas.Campo_No_Salida + " IN (" + Negocio.P_Lista_Salidas + ")";
                Cmd.CommandText = Mi_SQL;
                Registros_Afectados = Cmd.ExecuteNonQuery();
                Trans.Commit();
                //return Registros_Afectados;
            }
            catch (Exception ex)
            {
                Trans.Rollback();
                ex.ToString();
                Registros_Afectados = 0;
            }
            finally 
            {
                Cn.Close();
            }
            return Registros_Afectados;
        }
        public static DataTable Consultar_Poliza_SAP_Stock(Cls_Ope_Com_Polizas_Stock_Negocio Negocio)
        {
            try
            {
                int Consecutivo = Obtener_Consecutivo(Poliza_Stock_Para_Sap.Campo_No_Poliza_Stock,Poliza_Stock_Para_Sap.Tabla_Poliza_Stock_Para_Sap);
                String Mi_SQL = "";
                Mi_SQL =
                "SELECT * FROM" + Poliza_Stock_Para_Sap.Tabla_Poliza_Stock_Para_Sap;
                if (Negocio.P_No_Poliza_Stock != null && Negocio.P_No_Poliza_Stock != "")
                {
                    Mi_SQL += " WHERE " +
                    Poliza_Stock_Para_Sap.Campo_No_Poliza_Stock + " = " +
                    Negocio.P_No_Poliza_Stock;
                }
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Consecutivo
        ///DESCRIPCIÓN: Obtiene el numero consecutivo para las tablas ocupadas en esta clase
        ///PARAMETROS: 1.-Campo del cual se obtendra el consecutivo
        ///            2.-Nombre de la tabla
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 10/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Obtener_Consecutivo(String Campo_ID, String Tabla)
        {
            int Consecutivo = 0;
            String Mi_Sql;
            Object Obj; //Obtiene el ID con la cual se guardo los datos en la base de datos
            Mi_Sql = "SELECT NVL(MAX (" + Campo_ID + "),0) FROM " + Tabla;
            Obj = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            Consecutivo = (Convert.ToInt32(Obj) + 1);
            return Consecutivo;
        }
        #endregion

    }
}