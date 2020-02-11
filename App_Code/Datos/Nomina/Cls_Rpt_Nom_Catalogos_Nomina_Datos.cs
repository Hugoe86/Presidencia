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
using Presidencia.Rpt_Cat_Nomina.Negocio;
using System.Text;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;

namespace Presidencia.Rpt_Cat_Nomina.Datos
{
    public class Cls_Rpt_Nom_Catalogos_Nomina_Datos
    {
        #region (Métodos)
        /// *************************************************************************************************************************
        /// Nombre Método: Consultar_Tablas_Nomina
        /// 
        /// Descripción: Método que consulta las tablas de nómina de tipo catálogo.
        /// 
        /// Parámetros: No Aplica.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: Diciembre/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa Modificación:
        /// *************************************************************************************************************************
        public static DataTable Consultar_Tablas_Nomina()
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Tablas_Nomina = null;//Variable que listara las tablas de nomina.

            try
            {
                Mi_SQL.Append(" SELECT ");
                Mi_SQL.Append(" TABLE_NAME AS NOMBRE_CATALOGO ");
                Mi_SQL.Append(" FROM ALL_TABLES ");
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(" UPPER(TABLE_NAME) LIKE UPPER('%CAT_NOM%') ");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(" (");
                Mi_SQL.Append(" TABLE_NAME NOT  LIKE '%DET' ");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(" TABLE_NAME NOT  LIKE '%DETA' ");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(" TABLE_NAME NOT  LIKE '%DETALLES' ");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(" TABLE_NAME NOT IN ('CAT_NOM_PERIODO_TIPO_NOM', 'CAT_NOM_TAB_ORDEN_JUDICIAL')");
                Mi_SQL.Append(") ");
                Mi_SQL.Append(" OR ");
                Mi_SQL.Append(" TABLE_NAME IN ('CAT_EMPLEADOS', 'CAT_PUESTOS', 'CAT_AREAS', 'CAT_DEPENDENCIAS', 'TAB_NOM_DIAS_FESTIVOS', 'TAB_NOM_IMSS', 'TAB_NOM_ISR', 'TAB_NOM_SUBSIDIO')");
                Mi_SQL.Append(" ORDER BY TABLE_NAME ASC ");

                Dt_Tablas_Nomina = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Clase: [Cls_Rpt_Nom_Catalogos_Nomina_Datos] --> Metodo [Consultar_Tablas_Nomina] --> Error: [" + Ex.Message + "]");
            }
            return Dt_Tablas_Nomina;
        }
        /// *************************************************************************************************************************
        /// Nombre Método: Consultar_Campos_Por_Tabla
        /// 
        /// Descripción: Método que consulta todos los campos de la tabla seleccionada.
        /// 
        /// Parámetros: Tabla.- Tabla de la cuál se consultaran los campos.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: Diciembre/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa Modificación:
        /// *************************************************************************************************************************
        public static DataTable Consultar_Campos_Por_Tabla(Cls_Rpt_Nom_Catalogos_Nomina_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Campos_Por_Tabla = null;//Variable que listara los campos de la tabla seleccionada.

            try
            {
                Mi_SQL.Append("select column_name as NOMBRE_CAMPO from all_tab_columns where table_name = UPPER('" + Datos.P_Tabla + "')");
                Mi_SQL.Append(" and column_name <> 'USUARIO_CREO'");
                Mi_SQL.Append(" and column_name <> 'USUARIO_MODIFICO'");
                Mi_SQL.Append(" and column_name <> 'FECHA_CREO'");
                Mi_SQL.Append(" and column_name <> 'FECHA_MODIFICO' ");
                Mi_SQL.Append(" and column_name not in (select restricciones_columnas.column_name ");
                Mi_SQL.Append(" from user_cons_columns restricciones_columnas join user_constraints restricciones_tablas ");
                Mi_SQL.Append(" on restricciones_columnas.constraint_name=restricciones_tablas.constraint_name");
                Mi_SQL.Append(" where restricciones_tablas.constraint_type='P' ");
                Mi_SQL.Append(" and restricciones_tablas.table_name='" + Datos.P_Tabla + "') ");

                Mi_SQL.Append(" ORDER BY column_name ASC");

                Dt_Campos_Por_Tabla = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Clase: [Cls_Rpt_Nom_Catalogos_Nomina_Datos] --> Metodo [Consultar_Tablas_Nomina] --> Error: [" + Ex.Message + "]");
            }
            return Dt_Campos_Por_Tabla;
        }
        /// *************************************************************************************************************************
        /// Nombre Método: Ejecutar_Consulta
        /// 
        /// Descripción: Método que ejecuta la consulta que se le manda como parametro.
        /// 
        /// Parámetros: Datos.- Objeto con los datos a filtrar.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: Diciembre/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa Modificación:
        /// *************************************************************************************************************************
        public static DataTable Ejecutar_Consulta(Cls_Rpt_Nom_Catalogos_Nomina_Negocio Datos)
        {
            DataTable Dt_Datos = null;//Variable que almacenara el listado devuelto por la consulta.

            try
            {
                Dt_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Datos.P_Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al ejecutar la consulta. Error: [" + Ex.Message + "]");
            }
            return Dt_Datos;
        }
        #endregion
    }
}
