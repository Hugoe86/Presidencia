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
using Presidencia.Inventarios_De_Stock.Negocio;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;

/// <summary>
/// Summary description for Cls_Ope_Com_Cuadro_Comparativo_Datos
/// </summary>
/// 
namespace Presidencia.Inventarios_De_Stock.Datos
{
    public class Cls_Ope_Alm_Inventario_Stock_Datos
    {
        #region METODOS 
        public Cls_Ope_Alm_Inventario_Stock_Datos()
        {

        }
        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consultar_Costeo_Inventario
        /// DESCRIPCION:            
        /// PARAMETROS :           
        /// CREO       :            Gustavo Angeles Cruz
        /// FECHA_CREO :            15/Julio/2011  
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static double Consultar_Costeo_Inventario(Cls_Ope_Alm_Inventarios_Stock_Negocio Negocio)
        {
            DataTable Dt_Tabla = null;
            double Acumulado = 0.0;
            String Mi_SQL = "SELECT SUM(EXISTENCIA*COSTO_PROMEDIO) ACUMULADO FROM CAT_COM_PRODUCTOS  WHERE STOCK = 'SI'";
            DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            if (_DataSet != null && _DataSet.Tables.Count > 0)
            {
                Dt_Tabla = _DataSet.Tables[0];
                try
                {
                    Acumulado = double.Parse(Dt_Tabla.Rows[0]["ACUMULADO"].ToString());                   
                }
                catch (Exception Ex) 
                {
                    Ex.ToString();
                    Acumulado = 0.0;
                }
            }
            return Acumulado;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consultar_Inventario_Stock
        /// DESCRIPCION:            
        /// PARAMETROS :           
        /// CREO       :            Gustavo Angeles Cruz
        /// FECHA_CREO :            7/Julio/2011  
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consultar_Inventario_Stock(Cls_Ope_Alm_Inventarios_Stock_Negocio Negocio)
        {
            DataTable Dt_Tabla = null;         
            String Mi_SQL = "SELECT PRODUCTO_ID,CLAVE,NOMBRE,DESCRIPCION,EXISTENCIA,COMPROMETIDO,DISPONIBLE,MAXIMO,MINIMO,REORDEN,COSTO_PROMEDIO,0 AS NUEVO_MAXIMO, 0 AS NUEVO_MINIMO,0 AS NUEVO_REORDEN," + 
                "(EXISTENCIA*COSTO_PROMEDIO) ACUMULADO FROM CAT_COM_PRODUCTOS WHERE STOCK = 'SI' AND ESTATUS = 'ACTIVO' " ;
            if (!string.IsNullOrEmpty(Negocio.P_Partida_ID))
            {
                Mi_SQL += " AND " + Cat_Com_Productos.Campo_Partida_Especifica_ID + " = '" + Negocio.P_Partida_ID + "'";
            }
            if (!string.IsNullOrEmpty(Negocio.P_Nombre_Producto))
            {
                Mi_SQL += " AND UPPER(" + Cat_Com_Productos.Campo_Nombre + ") LIKE UPPER('%" +
                Negocio.P_Nombre_Producto + "%') ";
            }
            if (!string.IsNullOrEmpty(Negocio.P_Clave_Producto))
            {

                Mi_SQL = "SELECT PRODUCTO_ID,CLAVE,NOMBRE,DESCRIPCION,EXISTENCIA,COMPROMETIDO,DISPONIBLE,MAXIMO,MINIMO,REORDEN,COSTO_PROMEDIO,0 AS NUEVO_MAXIMO, 0 AS NUEVO_MINIMO,0 AS NUEVO_REORDEN," +
                    "(EXISTENCIA*COSTO_PROMEDIO) ACUMULADO FROM CAT_COM_PRODUCTOS WHERE UPPER(CLAVE) = UPPER('" + Negocio.P_Clave_Producto + "') AND STOCK = 'SI' AND ESTATUS = 'ACTIVO' ";
            }
            Mi_SQL += " ORDER BY " + Cat_Com_Productos.Campo_Nombre + " ASC";
                //Negocio.P_No_Requisicion + " GROUP BY (PROVEEDOR_ID)";
            DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            if (_DataSet != null && _DataSet.Tables.Count > 0)
            {
                Dt_Tabla = _DataSet.Tables[0];
            }
            return Dt_Tabla;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consultar_Precios_Cotizados
        /// DESCRIPCION:            
        /// PARAMETROS :           
        /// CREO       :            Gustavo Angeles Cruz
        /// FECHA_CREO :            6/Julio/2011  
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consultar_Partidas_Stock(Cls_Ope_Alm_Inventarios_Stock_Negocio Negocio)
        {

            DataTable Dt_Tabla = null;
            String Mi_SQL = "SELECT " + 
                Cat_Com_Partidas.Campo_Partida_ID + "," +
                Cat_Com_Partidas.Campo_Clave + " ||' '|| " + Cat_Com_Partidas.Campo_Descripcion + " CLAVE_NOMBRE" +
                " FROM " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas + " WHERE " + Cat_Com_Partidas.Campo_Partida_ID +
                " IN (" +
                "SELECT DISTINCT(" + Cat_Com_Productos.Campo_Partida_ID + ")" +
                " FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " WHERE " + Cat_Com_Productos.Campo_Stock + " = 'SI'" +                
                ")";
            DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);                                 
            if (_DataSet != null && _DataSet.Tables.Count > 0)
            {
                Dt_Tabla = _DataSet.Tables[0];
            }
            return Dt_Tabla;
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Modificar_Producto
        /// DESCRIPCION:            
        /// PARAMETROS :           
        /// CREO       :            Gustavo Angeles Cruz
        /// FECHA_CREO :            6/Julio/2011  
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static bool Modificar_Producto(Cls_Ope_Alm_Inventarios_Stock_Negocio Negocio)
        {
            int afectados = 0;
            try
            {            
                String Mi_SQL = "UPDATE " +
                Cat_Com_Productos.Tabla_Cat_Com_Productos +
                " SET " +
                Cat_Com_Productos.Campo_Minimo + " = " + Negocio.P_Minimo + "," +
                Cat_Com_Productos.Campo_Maximo + " = " + Negocio.P_Maximo + "," +
                Cat_Com_Productos.Campo_Reorden + " = " + Negocio.P_Reorden +
                " WHERE " + Cat_Com_Productos.Campo_Producto_ID + " ='" + Negocio.P_Producto_ID + "'";
                afectados =  OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (Exception Ex)
            {
                String Str = Ex.ToString();
                afectados = 0;
            }
            if (afectados > 0)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consultar_Salidas
        /// DESCRIPCION:            
        /// PARAMETROS :           
        /// CREO       :            Gustavo Angeles Cruz
        /// FECHA_CREO :            21 Marzo 2012  
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consultar_Inventario_Para_Calculo_Max_Min_RO(Cls_Ope_Alm_Inventarios_Stock_Negocio Negocio)
        {           
            DataTable Dt_Tabla = null;
            DateTime _DateTime = DateTime.Now;
            int dias = _DateTime.Day;
            dias = dias * -1;
            dias++;
            _DateTime = _DateTime.AddDays(dias);
            _DateTime = _DateTime.AddMonths(-1);
            
            String Inicial = _DateTime.ToString("dd/MM/yyyy").ToUpper();
            _DateTime = _DateTime.AddMonths(1);
            _DateTime = _DateTime.AddDays(-1);
            String Final = _DateTime.ToString("dd/MM/yyyy").ToUpper();

            try
            {
                String Mi_SQL = "" +
                "SELECT PRODUCTO.PRODUCTO_ID,PRODUCTO.CLAVE,PRODUCTO.NOMBRE,PRODUCTO.DESCRIPCION, " +
                "PRODUCTO.MAXIMO,PRODUCTO.MINIMO,PRODUCTO.REORDEN, SUM(SALIDA.CANTIDAD) SUMA, " +
                "MAX(SALIDA.CANTIDAD) MAYOR, MIN(SALIDA.CANTIDAD) MENOR FROM  ALM_COM_SALIDAS_DETALLES SALIDA " +
                "LEFT JOIN CAT_COM_PRODUCTOS  PRODUCTO " +
                "ON SALIDA.PRODUCTO_ID = PRODUCTO.PRODUCTO_ID " +
                " LEFT JOIN ALM_COM_SALIDAS ON " +
                " SALIDA.NO_SALIDA = ALM_COM_SALIDAS.NO_SALIDA " +
                "WHERE PRODUCTO.PRODUCTO_ID = '" + Negocio.P_Producto_ID + "' " +

                " AND TO_DATE(TO_CHAR(ALM_COM_SALIDAS.FECHA_CREO,'dd/MM/yyyy')) BETWEEN '" + Inicial + "' AND '" + Final + "' " +
                
                "GROUP BY (PRODUCTO.PRODUCTO_ID,PRODUCTO.NOMBRE,PRODUCTO.DESCRIPCION,PRODUCTO.MAXIMO,PRODUCTO.MINIMO,PRODUCTO.REORDEN,PRODUCTO.CLAVE) " +
                "ORDER BY PRODUCTO.NOMBRE"; 
                Dt_Tabla = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());             
            }
            return Dt_Tabla;
        }


        private static DateTime [] Fecha_Inicial_Final_Mes(DateTime Fecha_Inicio) 
        {
            DateTime[] Fechas = new DateTime[2];
            DateTime Fecha_Fin = Fecha_Inicio;            
            while(Fecha_Fin.Month == Fecha_Inicio.Month)
            {
                Fecha_Fin = Fecha_Fin.AddDays(1);
            }
            Fecha_Fin = Fecha_Fin.AddDays(-1);
            Fechas[0] = Fecha_Inicio;
            Fechas[1] = Fecha_Fin;
            return Fechas;
        } 

        public static DataTable Consultar_Salidas_Stock_Por_Periodo(Cls_Ope_Alm_Inventarios_Stock_Negocio Negocio)
        {
            int Meses = Negocio.P_No_Meses_Para_Calculo; //Num de meses a tomar en cuanta para la consulta           
            bool Tomar_Mes_Actual = Negocio.P_Tomar_Mes_Actual;
            DateTime _DateTime = DateTime.Now;

            DateTime [] Dtime_Fechas;

            int dias = _DateTime.Day;
            String Inicial = "";
            String Final = "";
            String Mi_SQL = "";

            dias = dias * -1;
            dias++;            
            _DateTime = _DateTime.AddDays(dias);

            DataTable Dt_Tabla = null;
            try
            {
                if (Tomar_Mes_Actual)
                {
                    _DateTime = _DateTime.AddMonths(-(Meses - 1));
                }
                else 
                {
                    _DateTime = _DateTime.AddMonths(-Meses);
                }
                
                for (int i = 1; i <= Meses; i++)
                {
                    Dtime_Fechas = Fecha_Inicial_Final_Mes(_DateTime);
                    Inicial = Dtime_Fechas[0].ToString("dd/MM/yyyy").ToUpper();
                    Final = Dtime_Fechas[1].ToString("dd/MM/yyyy").ToUpper();
                    _DateTime = Dtime_Fechas[1];
                    _DateTime = _DateTime.AddDays(1);

                    Mi_SQL +=
                    "SELECT PRODUCTO.PRODUCTO_ID, " +
                    "SUM(SALIDA.CANTIDAD) SUMA, " +
                    " COUNT(SALIDA.CANTIDAD) CONTEO " +
                    "FROM  ALM_COM_SALIDAS_DETALLES SALIDA " +
                    "LEFT JOIN CAT_COM_PRODUCTOS  PRODUCTO " +
                    "ON SALIDA.PRODUCTO_ID = PRODUCTO.PRODUCTO_ID " +
                    "LEFT JOIN ALM_COM_SALIDAS ON  SALIDA.NO_SALIDA = ALM_COM_SALIDAS.NO_SALIDA " +
                    "WHERE " +
                    "TO_DATE(TO_CHAR(ALM_COM_SALIDAS.FECHA_CREO,'dd/MM/yyyy')) " +
                    "BETWEEN '" + Inicial + "' AND '" + Final + "' ";
                    if (!String.IsNullOrEmpty(Negocio.P_Producto_ID))
                    {
                        Mi_SQL += " AND PRODUCTO.PRODUCTO_ID = '" + Negocio.P_Producto_ID + "' ";
                    }
                    Mi_SQL +=  "GROUP BY (PRODUCTO.PRODUCTO_ID) ";
                    if (i < Meses)
                    {
                        Mi_SQL += "UNION ALL ";
                    }
                }
                Dt_Tabla = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            return Dt_Tabla;
        }


        #endregion
    }        
}
