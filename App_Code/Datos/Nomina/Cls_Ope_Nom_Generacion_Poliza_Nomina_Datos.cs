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
using System.Text;
using Presidencia.Generacion_Poliza_Nomina.Negocio;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Cat_Nom_Percepciones_Deducciones_Opcional.Negocios;

namespace Presidencia.Generacion_Poliza_Nomina.Datos
{
    public class Cls_Ope_Nom_Generacion_Poliza_Nomina_Datos
    {
        #region (Consulta)
        public static DataTable Consultar_Cuentas_Contables_Nomina(Cls_Nom_Generacion_Poliza_Nomina_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Cuentas_Contables_Nomina = null;

            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Campo_Cuenta_Contable_ID + ", ");

                Mi_SQL.Append("(SELECT ");
                Mi_SQL.Append(Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta);
                Mi_SQL.Append(" FROM "); 
                Mi_SQL.Append(Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Cuenta_Contable_ID);
                Mi_SQL.Append(") AS CUENTA, ");

                Mi_SQL.Append("(SELECT ");
                Mi_SQL.Append(Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Partida_ID);
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Cuenta_Contable_ID);
                Mi_SQL.Append(") AS PARTIDA_ID, ");

                Mi_SQL.Append("NVL(SUM((");
                Mi_SQL.Append("SELECT SUM(");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_Monto);
                Mi_SQL.Append(") ");
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados);
                Mi_SQL.Append(" RIGHT OUTER JOIN ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det);
                Mi_SQL.Append(" ON ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Recibo);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_No_Recibo);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID);
                Mi_SQL.Append("='" +  Datos.P_Nomina_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina);
                Mi_SQL.Append("='" + Datos.P_No_Nomina + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_Percepcion_Deduccion_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID);
                Mi_SQL.Append(")), 0) AS MONTO ");

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion);

                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID);
                Mi_SQL.Append(" IN (");
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append("SUBSTR(COLUMN_NAME, 6, 6) AS PERCEPCION_DEDUCCION_ID ");
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(" ALL_TAB_COLUMNS ");
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(" TABLE_NAME = '" + Ope_Nom_Totales_Nomina.Tabla_Ope_Nom_Totales_Nomina + "' ");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(" COLUMN_NAME LIKE '%0%' ");
                Mi_SQL.Append(")");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Campo_Cuenta_Contable_ID + " IS NOT NULL ");
                Mi_SQL.Append(" GROUP BY " + Cat_Nom_Percepcion_Deduccion.Campo_Cuenta_Contable_ID);

                Dt_Cuentas_Contables_Nomina = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las cuentas contables de nomina. Error: [" + Ex.Message + "]");
            }
            return Dt_Cuentas_Contables_Nomina;
        }
        ///******************************************************************************
        /// NOMBRE DE LA FUNCIÓN: Consultar_Percepciones_Deducciones_General
        /// 
        /// DESCRIPCIÓN: Consulta la Tabla de Percepciones y Deducciones y se trae una lista de
        ///              todos las percepciones deducciones.
        ///              
        /// CREO: Juan Alberto Hernandez Negrete
        /// FECHA_CREO: 22/Agosto/2010 14:20 p.m.
        /// MODIFICO:
        /// FECHA_MODIFICO
        /// CAUSA_MODIFICACIÓN   
        ///******************************************************************************
        internal static DataTable Consultar_Percepciones_Deducciones_General(Cls_Nom_Generacion_Poliza_Nomina_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Percepciones_Deducciones = null;

            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + ".*, (" + Cat_Nom_Percepcion_Deduccion.Campo_Clave + " || ' ' || " + Cat_Nom_Percepcion_Deduccion.Campo_Nombre + ") AS NOMBRE_CONCEPTO");
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion);

                if (!String.IsNullOrEmpty(Datos.P_Cuenta_Contable_ID))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                        Mi_SQL.Append(" AND " + Cat_Nom_Percepcion_Deduccion.Campo_Cuenta_Contable_ID + "='" + Datos.P_Cuenta_Contable_ID + "'");
                    else
                        Mi_SQL.Append(" WHERE " + Cat_Nom_Percepcion_Deduccion.Campo_Cuenta_Contable_ID + "='" + Datos.P_Cuenta_Contable_ID + "'");
                }

                Dt_Percepciones_Deducciones = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las percepciones y deducciones. Error: [" + Ex.Message + "]");
            }
            return Dt_Percepciones_Deducciones;
        }

        public static String Consultar_Cuenta_Contable_Proveedor(Cls_Nom_Generacion_Poliza_Nomina_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Proveedores = null;
            String Cuenta_Contable_ID = String.Empty;

            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Nom_Proveedores.Campo_Cuenta_Contable_ID);
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Nom_Proveedores.Tabla_Cat_Nom_Proveedores);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Nom_Proveedores.Campo_Proveedor_ID); 
                Mi_SQL.Append(" IN ");
                Mi_SQL.Append("(SELECT ");
                Mi_SQL.Append(Cat_Nom_Proveedores_Detalles.Campo_Proveedor_ID);
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Nom_Proveedores_Detalles.Tabla_Cat_Nom_Proveedores_Detalles);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Nom_Proveedores_Detalles.Campo_Percepcion_Deduccion_ID);
                Mi_SQL.Append(" = '");
                Mi_SQL.Append(Datos.P_Percepcion_Deduccion_ID);
                Mi_SQL.Append("')");

                Dt_Proveedores = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

                if (Dt_Proveedores is DataTable) {
                    if (Dt_Proveedores.Rows.Count > 0) {
                        foreach (DataRow PROVEEDOR in Dt_Proveedores.Rows) {
                            if (PROVEEDOR is DataRow) {
                                if (!String.IsNullOrEmpty(PROVEEDOR[Cat_Nom_Proveedores.Campo_Cuenta_Contable_ID].ToString())) {
                                    Cuenta_Contable_ID = PROVEEDOR[Cat_Nom_Proveedores.Campo_Cuenta_Contable_ID].ToString().Trim();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar la cuenta contable del proveedor. Error: [" + Ex.Message + "]");
            }
            return Cuenta_Contable_ID;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Cuentas_Contables
        /// DESCRIPCION : Consulta los Tipos de Poliza de las cuentas contables que estan 
        ///              dados de alta en la BD 
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 21-Junio-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Cuentas_Contables(Cls_Nom_Generacion_Poliza_Nomina_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de las cuentas contables
            DataTable Dt_Cuenta_Contable = null;

            try
            {
                //Consulta las Cuentas Contables que estan dados de alta en la base de datos
                Mi_SQL = "SELECT " + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID + ", " + Cat_Con_Cuentas_Contables.Campo_Cuenta;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables;

                if (Datos.P_Cuenta_Contable_ID != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID + " = '" + Datos.P_Cuenta_Contable_ID + "'";
                }

                Dt_Cuenta_Contable = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            return Dt_Cuenta_Contable;
        }
        #endregion
    }
}
