using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Montos_Proceso_Compra.Negocio;
using System.Text;

namespace Presidencia.Montos_Proceso_Compra.Datos
{
    public class Cls_Cat_Com_Montos_Proceso_Compra_Datos
    {
        #region METODOS
            //*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Montos_Proceso_Compra
            ///DESCRIPCIÓN: Consultar Montos del proceso de compra en la base de datos
            ///PARAMETROS:  1.- Cls_Cat_Com_Monto_Proceso_Compra_Negocios
            ///CREO: Leslie Gonzalez Vazquez
            ///FECHA_CREO: 15/Febrero/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************/
            public DataTable Consultar_Montos_Proceso_Compra(Cls_Cat_Com_Montos_Proceso_Compra_Negocio Montos_Proceso_Compra)
            {
                StringBuilder MI_SQL = new StringBuilder();//Variable que almacenara la consulta.
                DataTable Dt_Montos_Proceso_Compra = null;//Variable que almacenara un listado de [Montos Proceso Compra].
                try
                {
                    MI_SQL.Append(" SELECT " + Cat_Com_Monto_Proceso_Compra.Tabla_Cat_Com_Monto_Proceso_Compra  + ".* ");
                    MI_SQL.Append(" FROM " + Cat_Com_Monto_Proceso_Compra.Tabla_Cat_Com_Monto_Proceso_Compra);
                    MI_SQL.Append(" WHERE " + Cat_Com_Monto_Proceso_Compra.Campo_Tipo + " = '" + Montos_Proceso_Compra.P_Tipo + "'");

                    Dt_Montos_Proceso_Compra = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, 
                                                CommandType.Text, MI_SQL.ToString()).Tables[0];
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al consultar Consultar_Montos_Proceso_Compra. Error: [" + Ex.Message + "]");
                }
                return (Dt_Montos_Proceso_Compra is DataTable) ? Dt_Montos_Proceso_Compra : new DataTable();
            }

            //*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Modificar_Montos_Proceso_Compra
            ///DESCRIPCIÓN: Modificar el giro de un proveedor en la base de datos
            ///PARAMETROS:  1.- Cls_Cat_Com_Monto_Proceso_Compra_Negocios
            ///CREO: Leslie Gonzalez Vazquez
            ///FECHA_CREO: 15/Febrero/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************/
            public static Boolean Modificar_Montos_Proceso_Compra(Cls_Cat_Com_Montos_Proceso_Compra_Negocio Montos_Proceso_Compra)
            {
                StringBuilder MI_SQL = new StringBuilder();//variable que almacenará la consulta
                Boolean Operacion_Completa = false;//Estado de la operacion.
                try
                {
                    MI_SQL.Append(" UPDATE " + Cat_Com_Monto_Proceso_Compra.Tabla_Cat_Com_Monto_Proceso_Compra);
                    MI_SQL.Append(" SET " + Cat_Com_Monto_Proceso_Compra.Campo_Monto_Compra_Directa_Ini + " = '" + Montos_Proceso_Compra.P_Compra_Directa_Inicio + "', ");
                    MI_SQL.Append(Cat_Com_Monto_Proceso_Compra.Campo_Monto_Compra_Directa_Fin + " = '" + Montos_Proceso_Compra.P_Compra_Directa_Fin + "', ");
                    MI_SQL.Append(Cat_Com_Monto_Proceso_Compra.Campo_Monto_Cotizacion_Ini + " = '" + Montos_Proceso_Compra.P_Cotizacion_Inicio + "', " );
                    MI_SQL.Append(Cat_Com_Monto_Proceso_Compra.Campo_Monto_Cotizacion_Fin + " = '" + Montos_Proceso_Compra.P_Cotizacion_Fin + "', ");
                    MI_SQL.Append(Cat_Com_Monto_Proceso_Compra.Campo_Monto_Comite_Ini + " = '" + Montos_Proceso_Compra.P_Comite_Inicio + "', ");
                    MI_SQL.Append(Cat_Com_Monto_Proceso_Compra.Campo_Monto_Comite_Fin + " = '" + Montos_Proceso_Compra.P_Comite_Fin + "', ");
                    MI_SQL.Append(Cat_Com_Monto_Proceso_Compra.Campo_Monto_Licitacion_R_Ini + " = '" + Montos_Proceso_Compra.P_Licitacion_Restringida_Inicio + "', ");
                    MI_SQL.Append(Cat_Com_Monto_Proceso_Compra.Campo_Monto_Licitacion_R_Fin + " = '" + Montos_Proceso_Compra.P_Licitacion_Restringida_Fin + "', ");
                    MI_SQL.Append(Cat_Com_Monto_Proceso_Compra.Campo_Monto_Licitacion_P_Ini + " = '" + Montos_Proceso_Compra.P_Licitacion_Publica_Inicio + "', ");
                    MI_SQL.Append(Cat_Com_Monto_Proceso_Compra.Campo_Monto_Licitacion_P_Fin + " = '" + Montos_Proceso_Compra.P_Licitacion_Publica_Fin + "', ");
                    MI_SQL.Append(Cat_Com_Monto_Proceso_Compra.Campo_Fondo_Fijo_Ini + " = '" + Montos_Proceso_Compra.P_Fondo_Fijo_Inicio + "', ");
                    MI_SQL.Append(Cat_Com_Monto_Proceso_Compra.Campo_Fondo_Fijo_Fin + " = '" + Montos_Proceso_Compra.P_Fondo_Fijo_Fin + "', ");
                    MI_SQL.Append(Cat_Com_Monto_Proceso_Compra.Campo_Usuario_Modifico + " = '" + Montos_Proceso_Compra.P_Usuario + "', ");
                    MI_SQL.Append(Cat_Com_Monto_Proceso_Compra.Campo_Fecha_Modifico + " = SYSDATE");
                    MI_SQL.Append(" WHERE " + Cat_Com_Monto_Proceso_Compra.Campo_Tipo + " = '" + Montos_Proceso_Compra.P_Tipo + "'");

                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, MI_SQL.ToString());
                    Operacion_Completa = true;
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al tratar de modificar Modidificar_Montos_Proceso_Compra. Error: [" + Ex.Message + "]");
                }
                return Operacion_Completa;
            }
        #endregion
    }
}