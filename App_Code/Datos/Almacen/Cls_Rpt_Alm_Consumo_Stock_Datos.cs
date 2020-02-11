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
using Presidencia.Consumo_Stock.Negocio;
using System.Text;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;

namespace Presidencia.Consumo_Stock.Datos
{
    public class Cls_Rpt_Alm_Consumo_Stock_Datos
    {
        #region (Metodos)

        #region (Consultas)
        /// <summary>
        /// Nombre: Consultar_Departamentos
        /// 
        /// Descripción: Método que consulta los departamentos que tienen registros
        ///              de alguna salida de almacén.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creo: 05 Diciembre 2013 16:13 Hrs.
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// </summary>
        /// <returns>Tabla con los regitros encontrados según los filtros de búsqueda que se ingresaron</returns>
        public static DataTable Consultar_Departamentos(Cls_Rpt_Alm_Consumo_Stock_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Resultado = null;

            try
            {
                Mi_SQL.Append(" select distinct ");
                Mi_SQL.Append(" departamento." + Cat_Dependencias.Campo_Dependencia_ID + " ");
                Mi_SQL.Append(" , (trim(nvl(departamento." + Cat_Dependencias.Campo_Clave + ", '')) || '-' || nvl(departamento." + Cat_Dependencias.Campo_Nombre + ", '')) as UR ");
                Mi_SQL.Append(" from ");
                Mi_SQL.Append(" " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + " salidas ");
                Mi_SQL.Append(" left outer join " + Cat_Dependencias.Tabla_Cat_Dependencias + " departamento on ");
                Mi_SQL.Append(" salidas." + Alm_Com_Salidas.Campo_Dependencia_ID + " = departamento." + Cat_Dependencias.Campo_Dependencia_ID + " ");
                Mi_SQL.Append(" order by departamento." + Cat_Dependencias.Campo_Dependencia_ID + "  asc ");

                Dt_Resultado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                Mi_SQL.Remove(0, Mi_SQL.Length);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al realizar la consulta de departamentos. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }
        /// <summary>
        /// Nombre: Consultar_Productos
        /// 
        /// Descripción: Método que consulta los productos que tienen registros
        ///              de alguna salida de almacén.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creo: 05 Diciembre 2013 16:24 Hrs.
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// </summary>
        /// <returns>Tabla con los regitros encontrados según los filtros de búsqueda que se ingresaron</returns>
        public static DataTable Consultar_Productos(Cls_Rpt_Alm_Consumo_Stock_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Resultado = null;

            try
            {
                Mi_SQL.Append(" select distinct ");
                Mi_SQL.Append(" productos." + Cat_Com_Productos.Campo_Producto_ID + " ");
                Mi_SQL.Append(" , (trim(nvl(productos." + Cat_Com_Productos.Campo_Clave + ", '')) || '-' || nvl(productos." + Cat_Com_Productos.Campo_Nombre + ", '')) as Producto ");
                Mi_SQL.Append(" from  ");
                Mi_SQL.Append(" " + Alm_Com_Salidas_Detalles.Tabla_Alm_Com_Salidas_Detalles + " detalle  ");
                Mi_SQL.Append(" left outer join " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " productos on ");
                Mi_SQL.Append(" detalle." + Alm_Com_Salidas_Detalles.Campo_Producto_ID + " = productos." + Cat_Com_Productos.Campo_Producto_ID + " ");
                Mi_SQL.Append(" order by productos." + Cat_Com_Productos.Campo_Producto_ID + " asc ");

                Dt_Resultado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                Mi_SQL.Remove(0, Mi_SQL.Length);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al realizar la consulta de productos. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }
        /// <summary>
        /// Nombre: Consultar_Partidas_Presupuestales
        /// 
        /// Descripción: Método que consulta las partidas presupuestales que tienen registros
        ///              de alguna salida de almacén.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creo: 05 Diciembre 2013 16:29 Hrs.
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// </summary>
        /// <returns>Tabla con los regitros encontrados según los filtros de búsqueda que se ingresaron</returns>
        public static DataTable Consultar_Partidas_Presupuestales(Cls_Rpt_Alm_Consumo_Stock_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Resultado = null;

            try
            {
                Mi_SQL.Append(" select distinct ");
                Mi_SQL.Append(" partidas." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + " ");
                Mi_SQL.Append(" , (trim(nvl(partidas." + Cat_Sap_Partidas_Especificas.Campo_Clave + ", '')) || '-' || nvl(partidas." + Cat_Sap_Partidas_Especificas.Campo_Nombre + ", '')) as Partida ");
                Mi_SQL.Append(" from  ");
                Mi_SQL.Append(" " + Alm_Com_Salidas_Detalles.Tabla_Alm_Com_Salidas_Detalles + " detalle  ");
                Mi_SQL.Append(" left outer join " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " productos on ");
                Mi_SQL.Append(" detalle." + Alm_Com_Salidas_Detalles.Campo_Producto_ID + " = productos." + Cat_Com_Productos.Campo_Producto_ID + " ");
                Mi_SQL.Append(" left outer join " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + " partidas on ");
                Mi_SQL.Append(" productos." + Cat_Com_Productos.Campo_Partida_ID + " = partidas." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + " ");
                Mi_SQL.Append(" order by partidas." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + " asc ");

                Dt_Resultado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                Mi_SQL.Remove(0, Mi_SQL.Length);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al realizar la consulta de partidas presupuestales. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }
        /// <summary>
        /// Nombre: Consultar_Consumo_Stock
        /// 
        /// Descripción: Método que consulta el consumo de Stock
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creo: 05 Diciembre 2013 16:57 Hrs.
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// </summary>
        /// <returns>Tabla con los regitros encontrados según los filtros de búsqueda que se ingresaron</returns>
        public static DataTable Consultar_Consumo_Stock(Cls_Rpt_Alm_Consumo_Stock_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Resultado = null;

            try
            {

                #region (Campos a Mostrar)
                Mi_SQL.Append(" select ");
                Mi_SQL.Append(" productos." + Cat_Com_Productos.Campo_Clave + " ");
                Mi_SQL.Append(" , productos." + Cat_Com_Productos.Campo_Nombre + " as Producto ");
                Mi_SQL.Append(" , unidad." + Cat_Com_Unidades.Campo_Nombre + " as Unidad ");
                Mi_SQL.Append(" , requisicion." + Ope_Com_Requisiciones.Campo_Codigo_Programatico + " as Codigo_Programatico ");
                Mi_SQL.Append(" , (trim(nvl(departamento." + Cat_Dependencias.Campo_Clave + ", '')) || '-' || nvl(departamento." + Cat_Dependencias.Campo_Nombre + ", '')) as Departamento ");
                Mi_SQL.Append(" , sum(detalle_salida." + Alm_Com_Salidas_Detalles.Campo_Cantidad + ") as Cantidad ");
                Mi_SQL.Append(" , sum(detalle_salida." + Alm_Com_Salidas_Detalles.Campo_Importe + ") as Precio "); 
                #endregion

                #region (Tablas)
                Mi_SQL.Append(" from ");
                Mi_SQL.Append(" " + Alm_Com_Salidas_Detalles.Tabla_Alm_Com_Salidas_Detalles + " detalle_salida  ");

                Mi_SQL.Append(" left outer join " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + " salidas on ");
                Mi_SQL.Append(" detalle_salida." + Alm_Com_Salidas_Detalles.Campo_No_Salida + " = salidas." + Alm_Com_Salidas.Campo_No_Salida + " ");

                Mi_SQL.Append(" left outer join " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " requisicion on ");
                Mi_SQL.Append(" salidas." + Alm_Com_Salidas.Campo_Requisicion_ID + " = requisicion." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " ");

                Mi_SQL.Append(" left outer join " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " productos on ");
                Mi_SQL.Append(" detalle_salida." + Alm_Com_Salidas_Detalles.Campo_Producto_ID + " = productos." + Cat_Com_Productos.Campo_Producto_ID + " ");

                Mi_SQL.Append(" left outer join " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " unidad on ");
                Mi_SQL.Append(" productos." + Cat_Com_Productos.Campo_Unidad_ID + " = unidad." + Cat_Com_Unidades.Campo_Unidad_ID + " ");

                Mi_SQL.Append(" left outer join " + Cat_Dependencias.Tabla_Cat_Dependencias + " departamento on ");
                Mi_SQL.Append(" salidas." + Alm_Com_Salidas.Campo_Dependencia_ID + " = departamento." + Cat_Dependencias.Campo_Dependencia_ID + " ");

                Mi_SQL.Append(" left outer join " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + " partidas on ");
                Mi_SQL.Append(" productos." + Cat_Com_Productos.Campo_Partida_ID + " = partidas." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + " ");  
                #endregion

                #region (Filtros)
                Mi_SQL.Append(" where ");
                Mi_SQL.Append(" productos." + Cat_Com_Productos.Campo_Stock + " = 'SI' ");

                if (!string.IsNullOrEmpty(Datos.P_Dependencia_ID))
                    Mi_SQL.Append(" and departamento." + Cat_Dependencias.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "'");

                if (!string.IsNullOrEmpty(Datos.P_Producto_ID))
                    Mi_SQL.Append(" and productos." + Cat_Com_Productos.Campo_Producto_ID + " = '" + Datos.P_Producto_ID + "'");

                if (!string.IsNullOrEmpty(Datos.P_Partida_ID))
                    Mi_SQL.Append(" and partidas." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + " = '" + Datos.P_Partida_ID + "'");

                if (!string.IsNullOrEmpty(Datos.P_Fecha_Inicio) && !string.IsNullOrEmpty(Datos.P_Fecha_Fin))
                {
                    Mi_SQL.Append(" and (salidas." + Alm_Com_Salidas.Campo_Fecha_Creo + " between ");
                    Mi_SQL.Append(" TO_DATE ('" + Datos.P_Fecha_Inicio + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS') and ");
                    Mi_SQL.Append(" TO_DATE ('" + Datos.P_Fecha_Fin + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS'))");
                } 
                #endregion

                #region (Agrupamiento)
                Mi_SQL.Append(" group by ");
                Mi_SQL.Append(" productos." + Cat_Com_Productos.Campo_Clave + " ");
                Mi_SQL.Append(" , productos." + Cat_Com_Productos.Campo_Nombre + " ");
                Mi_SQL.Append(" , unidad." + Cat_Com_Unidades.Campo_Nombre + " ");
                Mi_SQL.Append(" , requisicion." + Ope_Com_Requisiciones.Campo_Codigo_Programatico + " ");
                Mi_SQL.Append(" , departamento." + Cat_Dependencias.Campo_Clave + " ");
                Mi_SQL.Append(" , departamento." + Cat_Dependencias.Campo_Nombre + " "); 
                #endregion

                Dt_Resultado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                Mi_SQL.Remove(0, Mi_SQL.Length);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al realizar la consulta del consumo de Stock. Error: [" + Ex.Message + "]");
            }
            return Dt_Resultado;
        }
        #endregion

        #endregion
    }
}
