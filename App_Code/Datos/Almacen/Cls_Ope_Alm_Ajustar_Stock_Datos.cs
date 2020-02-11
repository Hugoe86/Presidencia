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

using Presidencia.Ajustar_Stock.Negocio;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
/// <summary>
/// Summary description for Cls_Ope_Alm_Ajustar_Stock_Datos
/// </summary>
/// 
namespace Presidencia.Ajustar_Stock.Datos
{
    public class Cls_Ope_Alm_Ajustar_Stock_Datos
    {
        public Cls_Ope_Alm_Ajustar_Stock_Datos()
        {
        }
    #region MÉTODOS

        public static DataTable Consultar_Productos(Cls_Ope_Alm_Ajustar_Stock_Negocio Negocio)
        {
            String Mi_SQL = " SELECT * FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos +
                            " WHERE UPPER(" + Cat_Com_Productos.Campo_Nombre +
                            ") LIKE UPPER('%" + Negocio.P_Producto + "%') AND " +
                            Cat_Com_Productos.Campo_Stock + " ='SI'" ;
            if (!String.IsNullOrEmpty(Negocio.P_Clave)) 
            {
                Mi_SQL += " AND " + Cat_Com_Productos.Campo_Clave + " = '" + Negocio.P_Clave + "'";
            }
            DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            DataTable Dt_Table = null;
            if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
            {
                Dt_Table = _DataSet.Tables[0];
            }
            return Dt_Table;
        }
        public static int Actualizar_Productos(Cls_Ope_Alm_Ajustar_Stock_Negocio Negocio)
        {
            try
            {
                String Mi_SQL = "UPDATE " + Cat_Com_Productos.Tabla_Cat_Com_Productos +
                                " SET " + Cat_Com_Productos.Campo_Existencia + " = " + Negocio.P_Existencia + "," +
                                Cat_Com_Productos.Campo_Disponible + " = " + Negocio.P_Disponible +
                                " WHERE " + Cat_Com_Productos.Campo_Clave + " = '" + Negocio.P_Clave + "'";
                int Rows = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                return Rows;
            }
            catch(Exception Ex)
            {
                Ex.ToString();
                return 0;
            }
        }
        public static DataTable Consultar_Ajustes_Inventario(Cls_Ope_Alm_Ajustar_Stock_Negocio Negocio)
        {
            DataTable Dt_Tabla = null;
            try
            {
                String Mi_SQL = "SELECT * FROM " + Ope_Alm_Ajustes_Inv_Stock.Tabla_Ope_Alm_Ajustes_Inv_Stock +
                               " WHERE " + 
                               " TO_DATE(TO_CHAR(" + Ope_Alm_Ajustes_Inv_Stock.Campo_Fecha_Creo + ",'DD-MM-YYYY'))" +
                               " >= '" + Negocio.P_Fecha_Inicial + "' AND " +
                               "TO_DATE(TO_CHAR(" + Ope_Alm_Ajustes_Inv_Stock.Campo_Fecha_Creo + ",'DD-MM-YYYY'))" +
                                            " <= '" + Negocio.P_Fecha_Final + "'";
                if (!String.IsNullOrEmpty(Negocio.P_No_Ajuste))
                {
                    Mi_SQL = "SELECT * FROM " + Ope_Alm_Ajustes_Inv_Stock.Tabla_Ope_Alm_Ajustes_Inv_Stock +
                                   " WHERE " + Ope_Alm_Ajustes_Inv_Stock.Campo_No_Ajuste + " = " + Negocio.P_No_Ajuste;
                }
                Dt_Tabla = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch(Exception Ex)
            {
                Ex.ToString();
                Dt_Tabla = null;
            }
            return Dt_Tabla;
        }

        public static int Guardar_Ajustes_Inventario(Cls_Ope_Alm_Ajustar_Stock_Negocio Negocio)
        {
            int Registros_Afectados = 0;
            int No_Ajuste = 0;
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
                No_Ajuste =
                    Obtener_Consecutivo(Ope_Alm_Ajustes_Inv_Stock.Campo_No_Ajuste,
                    Ope_Alm_Ajustes_Inv_Stock.Tabla_Ope_Alm_Ajustes_Inv_Stock);
                String Mi_SQL = "INSERT INTO " + Ope_Alm_Ajustes_Inv_Stock.Tabla_Ope_Alm_Ajustes_Inv_Stock +
                "(" + Ope_Alm_Ajustes_Inv_Stock.Campo_No_Ajuste + "," +
                Ope_Alm_Ajustes_Inv_Stock.Campo_Fecha_Hora + "," +
                Ope_Alm_Ajustes_Inv_Stock.Campo_Motivo_Ajuste_Coor + "," +
                Ope_Alm_Ajustes_Inv_Stock.Campo_Empleado_Elaboro_ID + "," +
                Ope_Alm_Ajustes_Inv_Stock.Campo_Fecha_Elaboro + "," +
                Ope_Alm_Ajustes_Inv_Stock.Campo_Estatus + "," +
                Ope_Alm_Ajustes_Inv_Stock.Campo_Usuario_Creo + "," +
                Ope_Alm_Ajustes_Inv_Stock.Campo_Fecha_Creo + ") VALUES (" +
                No_Ajuste + ",SYSTIMESTAMP,'" + Negocio.P_Motivo_Ajuste_Coordinador + "','" +
                Cls_Sessiones.Empleado_ID + "',SYSDATE,'" + Negocio.P_Estatus + "','" +
                Cls_Sessiones.Nombre_Empleado + "',SYSDATE)";
                Cmd.CommandText = Mi_SQL;
                Registros_Afectados = Cmd.ExecuteNonQuery();

                foreach (DataRow Producto in Negocio.P_Dt_Productos_Ajustados.Rows)
                {
                    Mi_SQL = "INSERT INTO " + Ope_Alm_Ajustes_Detalles.Tabla_Ope_Alm_Ajustes_Almacen +
                    "(" + Ope_Alm_Ajustes_Detalles.Campo_No_Ajuste + "," +
                    Ope_Alm_Ajustes_Detalles.Campo_Producto_ID + "," +
                    Ope_Alm_Ajustes_Detalles.Campo_Existencia_Sistema + "," +
                    Ope_Alm_Ajustes_Detalles.Campo_Conteo_Fisico + "," +
                    Ope_Alm_Ajustes_Detalles.Campo_Diferencia + "," +
                    Ope_Alm_Ajustes_Detalles.Campo_Tipo_Movimiento + "," +
                    Ope_Alm_Ajustes_Detalles.Campo_Importe_Diferencia + "," +
                    Ope_Alm_Ajustes_Detalles.Campo_Precio_Promedio + "," +
                    Ope_Alm_Ajustes_Detalles.Campo_Nombre_Descipcion + "," +
                    Ope_Alm_Ajustes_Detalles.Campo_Usuario_Creo + "," +
                    Ope_Alm_Ajustes_Detalles.Campo_Fecha_Creo + ") VALUES (" +
                    No_Ajuste + "," +
                    "'" + Producto[Ope_Alm_Ajustes_Detalles.Campo_Producto_ID].ToString() + "'," +
                    "" + Producto[Ope_Alm_Ajustes_Detalles.Campo_Existencia_Sistema].ToString() + "," +
                    "" + Producto[Ope_Alm_Ajustes_Detalles.Campo_Conteo_Fisico].ToString() + "," +
                    "" + Producto[Ope_Alm_Ajustes_Detalles.Campo_Diferencia].ToString() + "," +
                    "'" + Producto[Ope_Alm_Ajustes_Detalles.Campo_Tipo_Movimiento].ToString() + "'," +
                    "" + Producto[Ope_Alm_Ajustes_Detalles.Campo_Importe_Diferencia].ToString() + "," +
                    "" + Producto[Ope_Alm_Ajustes_Detalles.Campo_Precio_Promedio].ToString() + "," +
                    "'" + Producto[Ope_Alm_Ajustes_Detalles.Campo_Nombre_Descipcion].ToString() + "'," +
                    "'" + Cls_Sessiones.Nombre_Empleado + "',SYSDATE)";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                }
                Trans.Commit();
            }
            catch (Exception Ex)
            {
                Trans.Rollback();               
                No_Ajuste = 0;
                throw new Exception(Ex.ToString());
            }
            finally
            {
                Cn.Close();
            }
            return No_Ajuste;
        }


        public static int Aplicar_Ajuste_Inventario(Cls_Ope_Alm_Ajustar_Stock_Negocio Negocio)
        {
            int Registros_Afectados = 0;
            int No_Ajuste = 0;
            int Existencia = 0;
            int Disponible = 0;
            int Comprometido = 0;
            int Diferencia = 0;
            DataTable Dt_Existencia = null;
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
                String Mi_SQL = "SELECT * FROM " + Ope_Alm_Ajustes_Detalles.Tabla_Ope_Alm_Ajustes_Almacen +
                " WHERE " + Ope_Alm_Ajustes_Detalles.Campo_No_Ajuste + " = " + Negocio.P_No_Ajuste;
                DataTable Dt_Productos_De_Ajuste = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                foreach (DataRow Producto in Dt_Productos_De_Ajuste.Rows)
                {
                    Mi_SQL = "SELECT " + Cat_Com_Productos.Campo_Existencia + "," +
                    Cat_Com_Productos.Campo_Comprometido + "," + Cat_Com_Productos.Campo_Disponible +
                    " FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos +
                    " WHERE " + Cat_Com_Productos.Campo_Producto_ID + " ='" + Producto[Cat_Com_Productos.Campo_Producto_ID].ToString().Trim() + "'";
                    Dt_Existencia = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                    Existencia = int.Parse(Dt_Existencia.Rows[0][Cat_Com_Productos.Campo_Existencia].ToString().Trim());
                    Disponible = int.Parse(Dt_Existencia.Rows[0][Cat_Com_Productos.Campo_Disponible].ToString().Trim());
                    Comprometido = int.Parse(Dt_Existencia.Rows[0][Cat_Com_Productos.Campo_Comprometido].ToString().Trim());
                    Diferencia = int.Parse(Producto["DIFERENCIA"].ToString().Trim());
                    //Existencia = Producto["TIPO_MOVIMIENTO"].ToString().Trim() == "ENTRADA" ? Existencia + Diferencia : Existencia - Diferencia;
                    //Disponible = Producto["TIPO_MOVIMIENTO"].ToString().Trim() == "ENTRADA" ? Disponible + Diferencia : Disponible - Diferencia;
                    //hacer update
                    Mi_SQL = "UPDATE " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " SET ";
                    if (Producto["TIPO_MOVIMIENTO"].ToString().Trim() == "ENTRADA")
                    {                        
                        Mi_SQL += Cat_Com_Productos.Campo_Existencia + " = " + Cat_Com_Productos.Campo_Existencia + " + " + Diferencia + ",";
                        Mi_SQL += Cat_Com_Productos.Campo_Disponible + " = " + Cat_Com_Productos.Campo_Disponible + " + " + Diferencia;
                    }
                    else if (Producto["TIPO_MOVIMIENTO"].ToString().Trim() == "SALIDA")
                    {
                        Mi_SQL += Cat_Com_Productos.Campo_Existencia + " = " + Cat_Com_Productos.Campo_Existencia + " - " + Diferencia + ",";
                        Mi_SQL += Cat_Com_Productos.Campo_Comprometido + " = " + Cat_Com_Productos.Campo_Comprometido + " - " + Diferencia;
                        //Mi_SQL += ", " + Cat_Com_Productos.Campo_Comprometido + " = " + (Comprometido - Diferencia);
                    }

                    Mi_SQL += " WHERE " + Cat_Com_Productos.Campo_Producto_ID + " = '";
                    Mi_SQL += Producto[Cat_Com_Productos.Campo_Producto_ID].ToString().Trim() + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                }
                Trans.Commit();
                Registros_Afectados = 1;
            }
            catch (Exception Ex)
            {
                Trans.Rollback();
                Ex.ToString();
                Registros_Afectados = 0;
                throw new Exception(Ex.ToString());
            }
            finally
            {
                Cn.Close();
            }
            return Registros_Afectados;
        }

        public static DataTable Consultar_Productos_De_Ajuste(Cls_Ope_Alm_Ajustar_Stock_Negocio Negocio)
        {
            DataTable Dt_tabla = null;
            try
            {
                String Mi_SQL = "SELECT * FROM " + Ope_Alm_Ajustes_Detalles.Tabla_Ope_Alm_Ajustes_Almacen +
                " WHERE " + Ope_Alm_Ajustes_Detalles.Campo_No_Ajuste + " = " + Negocio.P_No_Ajuste;
                Dt_tabla = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                Dt_tabla = null;
                 throw new Exception(Ex.ToString());
            }
            return Dt_tabla;
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
