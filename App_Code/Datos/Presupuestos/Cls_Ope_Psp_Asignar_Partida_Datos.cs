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
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using System.Text;
using Presidencia.Ope_Psp_Asignar_Partida.Negocio;

namespace Presidencia.Ope_Psp_Asignar_Partida.Datos
{
    public class Cls_Ope_Psp_Asignar_Partida_Datos
    {
        #region METODOS
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Unidad_Responsable
        ///DESCRIPCIÓN          : consulta para obtener los datos de las unidad responsable
        ///PARAMETROS           : 
        ///CREO                 : Leslie Gonzalez Vázquez
        ///FECHA_CREO           : 24/Noviembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static DataTable Consultar_Unidad_Responsable()
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                //OBTENEMOS LAS DEPENDENCIAS DEL CATALOGO
                Mi_Sql.Append("SELECT " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " ||' '|| ");
                Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS NOMBRE, ");
                Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);
                Mi_Sql.Append(" FROM " + Cat_Dependencias.Tabla_Cat_Dependencias);
                Mi_Sql.Append(" WHERE " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Estatus + " = 'ACTIVO'");
                Mi_Sql.Append(" ORDER BY " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " ASC");

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros de las unidades responsables. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Capitulos
        ///DESCRIPCIÓN          : consulta para obtener los capitulos de una unidad responsable
        ///PARAMETROS           1 Negocio conexion con la capa de negocio 
        ///CREO                 : Leslie Gonzalez Vázquez
        ///FECHA_CREO           : 24/Noviembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static DataTable Consultar_Capitulos()
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                //OBTENEMOS LAS DEPENDENCIAS DEL CATALOGO
                Mi_Sql.Append("SELECT " + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + "." + Cat_SAP_Capitulos.Campo_Clave + " ||' '|| ");
                Mi_Sql.Append(Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + "." + Cat_SAP_Capitulos.Campo_Descripcion + " AS NOMBRE, ");
                Mi_Sql.Append(Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + "." + Cat_SAP_Capitulos.Campo_Capitulo_ID);
                Mi_Sql.Append(" FROM " + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos);
                Mi_Sql.Append(" WHERE " + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + "." + Cat_SAP_Capitulos.Campo_Estatus + " = 'ACTIVO'");
                Mi_Sql.Append(" ORDER BY " + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + "." + Cat_SAP_Capitulos.Campo_Clave + " ASC");



                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros de los capitulos. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Estatus_Partidas
        ///DESCRIPCIÓN          : consulta para obtener el estatus de la partida
        ///PARAMETROS           1 Negocio conexion con la capa de negocio 
        ///CREO                 : Leslie Gonzalez Vázquez
        ///FECHA_CREO           : 24/Noviembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static DataTable Consultar_Estatus_Partidas(Cls_Ope_Psp_Asignar_Partida_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            try
            {
                Mi_SQL.Append("SELECT " + Ope_Psp_Calendarizacion_Presu.Campo_Estatus);
                Mi_SQL.Append(" FROM " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu);
                Mi_SQL.Append(" WHERE " + Ope_Psp_Calendarizacion_Presu.Campo_Estatus + " NOT IN('AUTORIZADO')");
                Mi_SQL.Append(" AND " + Ope_Psp_Calendarizacion_Presu.Campo_Anio + " = '" + Negocio.P_Anio_Presupuesto + "'");
                Mi_SQL.Append(" AND " + Ope_Psp_Calendarizacion_Presu.Campo_Dependencia_ID + " = '" + Negocio.P_Dependencia_ID + "'");

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los datos del año presupuestal. Error: [" + Ex.Message + "]");
            }
        }

        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Partida_Stock
        ///DESCRIPCIÓN          : consulta para obtener si la partida es de stock o no
        ///PARAMETROS           1 Negocio conexion con la capa de negocio 
        ///CREO                 : Leslie Gonzalez Vázquez
        ///FECHA_CREO           : 24/Noviembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static Boolean Guardar_Partidas_Asignadas(Cls_Ope_Psp_Asignar_Partida_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            Boolean Operacion_Exitosa = false;
            Double Ene = 0.00;
            Double Feb = 0.00;
            Double Mar = 0.00;
            Double Abr = 0.00;
            Double May = 0.00;
            Double Jun = 0.00;
            Double Jul = 0.00;
            Double Ago = 0.00;
            Double Sep = 0.00;
            Double Oct = 0.00;
            Double Nov = 0.00;
            Double Dic = 0.00;
            Double Precio = 0.00;

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
                Mi_SQL.Append("DELETE " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu);
                Mi_SQL.Append(" WHERE " + Ope_Psp_Calendarizacion_Presu.Campo_Dependencia_ID + " = '" + Negocio.P_Dependencia_ID + "'");
                Mi_SQL.Append(" AND " + Ope_Psp_Calendarizacion_Presu.Campo_Anio + " = '" + Negocio.P_Anio_Presupuesto + "'");
                Mi_SQL.Append(" AND " + Ope_Psp_Calendarizacion_Presu.Campo_Estatus + " NOT IN('AUTORIZADO')");
                Cmd.CommandText = Mi_SQL.ToString();
                Cmd.ExecuteNonQuery();

                foreach (DataRow Renglon in Negocio.P_Dt_Datos.Rows)
                {
                    Mi_SQL = new StringBuilder();
                    Ene = 0.00;
                    Feb = 0.00;
                    Mar = 0.00;
                    Abr = 0.00;
                    May = 0.00;
                    Jun = 0.00;
                    Jul = 0.00;
                    Ago = 0.00;
                    Sep = 0.00;
                    Oct = 0.00;
                    Nov = 0.00;
                    Dic = 0.00;
                    Precio = 0.00;

                    Precio = Convert.ToDouble(String.IsNullOrEmpty(Renglon["PRECIO"].ToString().Trim()) ? "0" : Renglon["PRECIO"].ToString().Trim());

                    if (Precio > 0)
                    {
                        Ene = Convert.ToDouble(String.IsNullOrEmpty(Renglon["ENERO"].ToString().Trim()) ? "0" : Renglon["ENERO"].ToString().Trim()) / Precio;
                        Feb = Convert.ToDouble(String.IsNullOrEmpty(Renglon["FEBRERO"].ToString().Trim()) ? "0" : Renglon["FEBRERO"].ToString().Trim()) / Precio;
                        Mar = Convert.ToDouble(String.IsNullOrEmpty(Renglon["MARZO"].ToString().Trim()) ? "0" : Renglon["MARZO"].ToString().Trim()) / Precio;
                        Abr = Convert.ToDouble(String.IsNullOrEmpty(Renglon["ABRIL"].ToString().Trim()) ? "0" : Renglon["ABRIL"].ToString().Trim()) / Precio;
                        May = Convert.ToDouble(String.IsNullOrEmpty(Renglon["MAYO"].ToString().Trim()) ? "0" : Renglon["MAYO"].ToString().Trim()) / Precio;
                        Jun = Convert.ToDouble(String.IsNullOrEmpty(Renglon["JUNIO"].ToString().Trim()) ? "0" : Renglon["JUNIO"].ToString().Trim()) / Precio;
                        Jul = Convert.ToDouble(String.IsNullOrEmpty(Renglon["JULIO"].ToString().Trim()) ? "0" : Renglon["JULIO"].ToString().Trim()) / Precio;
                        Ago = Convert.ToDouble(String.IsNullOrEmpty(Renglon["AGOSTO"].ToString().Trim()) ? "0" : Renglon["AGOSTO"].ToString().Trim()) / Precio;
                        Sep = Convert.ToDouble(String.IsNullOrEmpty(Renglon["SEPTIEMBRE"].ToString().Trim()) ? "0" : Renglon["SEPTIEMBRE"].ToString().Trim()) / Precio;
                        Oct = Convert.ToDouble(String.IsNullOrEmpty(Renglon["OCTUBRE"].ToString().Trim()) ? "0" : Renglon["OCTUBRE"].ToString().Trim()) / Precio;
                        Nov = Convert.ToDouble(String.IsNullOrEmpty(Renglon["NOVIEMBRE"].ToString().Trim()) ? "0" : Renglon["NOVIEMBRE"].ToString().Trim()) / Precio;
                        Dic = Convert.ToDouble(String.IsNullOrEmpty(Renglon["DICIEMBRE"].ToString().Trim()) ? "0" : Renglon["DICIEMBRE"].ToString().Trim()) / Precio;
                    }

                    Mi_SQL.Append("INSERT INTO " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + " (" + Ope_Psp_Calendarizacion_Presu.Campo_Dependencia_ID + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Fte_Financiamiento_ID + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Proyecto_Programa_ID + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Capitulo_ID + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Partida_ID + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Producto_ID + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Anio + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Justificacion + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Enero + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Febrero + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Marzo + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Abril + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Mayo + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Junio + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Julio + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Agosto + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Septiembre + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Octubre + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Noviembre + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Diciembre + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Importe_Enero + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Importe_Febrero + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Importe_Marzo + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Importe_Abril + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Importe_Mayo + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Importe_Junio + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Importe_Julio + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Importe_Agosto + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Importe_Septiembre + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Importe_Octubre + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Importe_Noviembre + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Importe_Diciembre + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Importe_Total + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Estatus + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Usuario_Creo + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Fecha_Creo + ") VALUES (");
                    Mi_SQL.Append("'" + Renglon["DEPENDENCIA_ID"].ToString() + "', ");
                    Mi_SQL.Append("'" + Renglon["FUENTE_FINANCIAMIENTO_ID"].ToString() + "',");
                    Mi_SQL.Append("'" + Renglon["PROYECTO_ID"].ToString() + "', ");
                    Mi_SQL.Append("'" + Renglon["CAPITULO_ID"].ToString() + "', ");
                    Mi_SQL.Append("'" + Renglon["PARTIDA_ID"].ToString() + "', ");
                    Mi_SQL.Append("'" + Renglon["PRODUCTO_ID"].ToString() + "', ");
                    Mi_SQL.Append("'" + Negocio.P_Anio_Presupuesto + "',");
                    Mi_SQL.Append("'" + Renglon["JUSTIFICACION"].ToString() + "', ");
                    Mi_SQL.Append(Convert.ToString(Ene) + ", ");
                    Mi_SQL.Append(Convert.ToString(Feb) + ", ");
                    Mi_SQL.Append(Convert.ToString(Mar) + ", ");
                    Mi_SQL.Append(Convert.ToString(Abr) + ", ");
                    Mi_SQL.Append(Convert.ToString(May) + ", ");
                    Mi_SQL.Append(Convert.ToString(Jun) + ", ");
                    Mi_SQL.Append(Convert.ToString(Jul) + ", ");
                    Mi_SQL.Append(Convert.ToString(Ago) + ", ");
                    Mi_SQL.Append(Convert.ToString(Sep) + ", ");
                    Mi_SQL.Append(Convert.ToString(Oct) + ", ");
                    Mi_SQL.Append(Convert.ToString(Nov) + ", ");
                    Mi_SQL.Append(Convert.ToString(Dic) + ", ");
                    Mi_SQL.Append(Renglon["ENERO"].ToString().Replace(",", "") + ", ");
                    Mi_SQL.Append(Renglon["FEBRERO"].ToString().Replace(",", "") + ", ");
                    Mi_SQL.Append(Renglon["MARZO"].ToString().Replace(",", "") + ", ");
                    Mi_SQL.Append(Renglon["ABRIL"].ToString().Replace(",", "") + ", ");
                    Mi_SQL.Append(Renglon["MAYO"].ToString().Replace(",", "") + ", ");
                    Mi_SQL.Append(Renglon["JUNIO"].ToString().Replace(",", "") + ", ");
                    Mi_SQL.Append(Renglon["JULIO"].ToString().Replace(",", "") + ", ");
                    Mi_SQL.Append(Renglon["AGOSTO"].ToString().Replace(",", "") + ", ");
                    Mi_SQL.Append(Renglon["SEPTIEMBRE"].ToString().Replace(",", "") + ", ");
                    Mi_SQL.Append(Renglon["OCTUBRE"].ToString().Replace(",", "") + ", ");
                    Mi_SQL.Append(Renglon["NOVIEMBRE"].ToString().Replace(",", "") + ", ");
                    Mi_SQL.Append(Renglon["DICIEMBRE"].ToString().Replace(",", "") + ", ");
                    Mi_SQL.Append(Renglon["IMPORTE_TOTAL"].ToString().Replace(",", "") + ", ");
                    Mi_SQL.Append("'" + Negocio.P_Estatus + "',");
                    Mi_SQL.Append("'" + Negocio.P_Usuario_Creo + "',");
                    Mi_SQL.Append("SYSDATE)");
                    Cmd.CommandText = Mi_SQL.ToString();
                    Cmd.ExecuteNonQuery();
                }
                Trans.Commit();
                Operacion_Exitosa = true;
            }
            catch (Exception Ex)
            {
                Operacion_Exitosa = false;
                Trans.Rollback();
                throw new Exception("Error al intentar insertar los registros. Error: [" + Ex.Message + "]");
            }
            return Operacion_Exitosa;
        }

        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Partida_Asignadas
        ///DESCRIPCIÓN          : consulta para obtener las partidas asignadas
        ///PARAMETROS           1 Negocio conexion con la capa de negocio 
        ///CREO                 : Leslie Gonzalez Vázquez
        ///FECHA_CREO           : 24/Noviembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static DataTable Consultar_Partida_Asignadas(Cls_Ope_Psp_Asignar_Partida_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            try
            {
                Mi_SQL.Append(" SELECT " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Dependencia_ID + ", ");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Proyecto_Programa_ID + " AS PROYECTO_ID, ");
                Mi_SQL.Append(Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + "." + Cat_SAP_Capitulos.Campo_Capitulo_ID + ", ");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Partida_ID + ", ");
                Mi_SQL.Append("NVL(" + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Producto_ID + ", '') AS PRODUCTO_ID, ");
                //obtenemos el precio
                Mi_SQL.Append("(CASE ");
                Mi_SQL.Append(" WHEN " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Enero + " > 0 THEN ");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Importe_Enero + "/");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Enero);
                Mi_SQL.Append(" WHEN " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Febrero + " > 0 THEN ");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Importe_Febrero + "/");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Febrero);
                Mi_SQL.Append(" WHEN " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Marzo + " > 0 THEN ");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Importe_Marzo + "/");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Marzo);
                Mi_SQL.Append(" WHEN " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Abril + " > 0 THEN ");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Importe_Abril + "/");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Abril);
                Mi_SQL.Append(" WHEN " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Mayo + " > 0 THEN ");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Importe_Mayo + "/");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Mayo);
                Mi_SQL.Append(" WHEN " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Junio + " > 0 THEN ");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Importe_Junio + "/");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Junio);
                Mi_SQL.Append(" WHEN " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Julio + " > 0 THEN ");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Importe_Julio + "/");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Julio);
                Mi_SQL.Append(" WHEN " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Agosto + " > 0 THEN ");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Importe_Agosto + "/");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Agosto);
                Mi_SQL.Append(" WHEN " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Septiembre + " > 0 THEN ");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Importe_Septiembre + "/");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Septiembre);
                Mi_SQL.Append(" WHEN " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Octubre + " > 0 THEN ");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Importe_Octubre + "/");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Octubre);
                Mi_SQL.Append(" WHEN " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Noviembre + " > 0 THEN ");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Importe_Noviembre + "/");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Noviembre);
                Mi_SQL.Append(" WHEN " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Diciembre + " > 0 THEN ");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Importe_Diciembre + "/");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Diciembre);
                Mi_SQL.Append(" ELSE 0 END) AS PRECIO, ");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Justificacion + ", ");
                Mi_SQL.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Clave + " AS CLAVE_PARTIDA, ");
                Mi_SQL.Append("NVL(" + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Clave + ", '') AS CLAVE_PRODUCTO, ");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Importe_Enero + " AS ENERO, ");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Importe_Febrero + " AS FEBRERO, ");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Importe_Marzo + " AS MARZO , ");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Importe_Abril + " AS ABRIL, ");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Importe_Mayo + " AS MAYO, ");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Importe_Junio + " AS JUNIO, ");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Importe_Julio + " AS JULIO, ");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Importe_Agosto + " AS AGOSTO, ");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Importe_Septiembre + " AS SEPTIEMBRE, ");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Importe_Octubre + " AS OCTUBRE, ");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Importe_Noviembre + " AS NOVIEMBRE, ");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Importe_Diciembre + " AS DICIEMBRE, ");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Importe_Total + " AS IMPORTE_TOTAL, '' AS ID, ");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Fte_Financiamiento_ID + " AS FUENTE_FINANCIAMIENTO_ID ");
                Mi_SQL.Append(" FROM " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu);
                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Com_Productos.Tabla_Cat_Com_Productos);
                Mi_SQL.Append(" ON " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Producto_ID);
                Mi_SQL.Append(" = " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Producto_ID);
                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas);
                Mi_SQL.Append(" ON " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Partida_ID);
                Mi_SQL.Append(" = " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID);
                Mi_SQL.Append(" INNER JOIN " + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica);
                Mi_SQL.Append(" ON " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID);
                Mi_SQL.Append(" = " + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica + "." + Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID);
                Mi_SQL.Append(" INNER JOIN " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto);
                Mi_SQL.Append(" ON " + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica + "." + Cat_SAP_Partida_Generica.Campo_Concepto_ID);
                Mi_SQL.Append(" = " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Concepto_ID);
                Mi_SQL.Append(" INNER JOIN " + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos);
                Mi_SQL.Append(" ON " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Capitulo_ID);
                Mi_SQL.Append(" = " + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + "." + Cat_SAP_Capitulos.Campo_Capitulo_ID);
                Mi_SQL.Append(" WHERE " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Estatus + " NOT IN('AUTORIZADO')");
                Mi_SQL.Append(" AND " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Anio + " = '" + Negocio.P_Anio_Presupuesto + "'");

                if (!string.IsNullOrEmpty(Negocio.P_Dependencia_ID))
                {
                    Mi_SQL.Append(" AND " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Dependencia_ID);
                    Mi_SQL.Append(" = '" + Negocio.P_Dependencia_ID + "'");
                }

                if (!string.IsNullOrEmpty(Negocio.P_Estatus))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" AND " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Estatus);
                        Mi_SQL.Append(" = '" + Negocio.P_Estatus + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Estatus);
                        Mi_SQL.Append(" = '" + Negocio.P_Estatus + "'");
                    }
                }

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros de las partidas asignadas. Error: [" + Ex.Message + "]");
            }
        }

        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Partida_Asignadas
        ///DESCRIPCIÓN          : Consulta para modificar los datos de las partidas asignadas
        ///PARAMETROS           1 Negocio conexion con la capa de negocio 
        ///CREO                 : Leslie Gonzalez Vázquez
        ///FECHA_CREO           : 24/Noviembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static Boolean Modificar_Partida_Asignadas(Cls_Ope_Psp_Asignar_Partida_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            Boolean Operacion_Exitosa = false;
            Double Ene = 0.00;
            Double Feb = 0.00;
            Double Mar = 0.00;
            Double Abr = 0.00;
            Double May = 0.00;
            Double Jun = 0.00;
            Double Jul = 0.00;
            Double Ago = 0.00;
            Double Sep = 0.00;
            Double Oct = 0.00;
            Double Nov = 0.00;
            Double Dic = 0.00;
            Double Precio = 0.00;

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
                Mi_SQL.Append("DELETE FROM " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu);
                Mi_SQL.Append(" WHERE " + Ope_Psp_Calendarizacion_Presu.Campo_Dependencia_ID + " = '" + Negocio.P_Dependencia_ID + "'");
                Mi_SQL.Append(" AND " + Ope_Psp_Calendarizacion_Presu.Campo_Anio + " = '" + Negocio.P_Anio_Presupuesto + "'");
                Cmd.CommandText = Mi_SQL.ToString();
                Cmd.ExecuteNonQuery();

                foreach (DataRow Renglon in Negocio.P_Dt_Datos.Rows)
                {
                    Mi_SQL = new StringBuilder();
                    Ene = 0.00;
                    Feb = 0.00;
                    Mar = 0.00;
                    Abr = 0.00;
                    May = 0.00;
                    Jun = 0.00;
                    Jul = 0.00;
                    Ago = 0.00;
                    Sep = 0.00;
                    Oct = 0.00;
                    Nov = 0.00;
                    Dic = 0.00;
                    Precio = 0.00;

                    Precio = Convert.ToDouble(String.IsNullOrEmpty(Renglon["PRECIO"].ToString().Trim()) ? "0" : Renglon["PRECIO"].ToString().Trim());

                    if (Precio > 0)
                    {
                        Ene = Convert.ToDouble(String.IsNullOrEmpty(Renglon["ENERO"].ToString().Trim()) ? "0" : Renglon["ENERO"].ToString().Trim()) / Precio;
                        Feb = Convert.ToDouble(String.IsNullOrEmpty(Renglon["FEBRERO"].ToString().Trim()) ? "0" : Renglon["FEBRERO"].ToString().Trim()) / Precio;
                        Mar = Convert.ToDouble(String.IsNullOrEmpty(Renglon["MARZO"].ToString().Trim()) ? "0" : Renglon["MARZO"].ToString().Trim()) / Precio;
                        Abr = Convert.ToDouble(String.IsNullOrEmpty(Renglon["ABRIL"].ToString().Trim()) ? "0" : Renglon["ABRIL"].ToString().Trim()) / Precio;
                        May = Convert.ToDouble(String.IsNullOrEmpty(Renglon["MAYO"].ToString().Trim()) ? "0" : Renglon["MAYO"].ToString().Trim()) / Precio;
                        Jun = Convert.ToDouble(String.IsNullOrEmpty(Renglon["JUNIO"].ToString().Trim()) ? "0" : Renglon["JUNIO"].ToString().Trim()) / Precio;
                        Jul = Convert.ToDouble(String.IsNullOrEmpty(Renglon["JULIO"].ToString().Trim()) ? "0" : Renglon["JULIO"].ToString().Trim()) / Precio;
                        Ago = Convert.ToDouble(String.IsNullOrEmpty(Renglon["AGOSTO"].ToString().Trim()) ? "0" : Renglon["AGOSTO"].ToString().Trim()) / Precio;
                        Sep = Convert.ToDouble(String.IsNullOrEmpty(Renglon["SEPTIEMBRE"].ToString().Trim()) ? "0" : Renglon["SEPTIEMBRE"].ToString().Trim()) / Precio;
                        Oct = Convert.ToDouble(String.IsNullOrEmpty(Renglon["OCTUBRE"].ToString().Trim()) ? "0" : Renglon["OCTUBRE"].ToString().Trim()) / Precio;
                        Nov = Convert.ToDouble(String.IsNullOrEmpty(Renglon["NOVIEMBRE"].ToString().Trim()) ? "0" : Renglon["NOVIEMBRE"].ToString().Trim()) / Precio;
                        Dic = Convert.ToDouble(String.IsNullOrEmpty(Renglon["DICIEMBRE"].ToString().Trim()) ? "0" : Renglon["DICIEMBRE"].ToString().Trim()) / Precio;
                    }

                    Mi_SQL.Append("INSERT INTO " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + " (" + Ope_Psp_Calendarizacion_Presu.Campo_Dependencia_ID + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Fte_Financiamiento_ID + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Proyecto_Programa_ID + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Capitulo_ID + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Partida_ID + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Producto_ID + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Anio + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Justificacion + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Enero + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Febrero + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Marzo + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Abril + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Mayo + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Junio + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Julio + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Agosto + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Septiembre + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Octubre + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Noviembre + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Cantidad_Diciembre + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Importe_Enero + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Importe_Febrero + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Importe_Marzo + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Importe_Abril + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Importe_Mayo + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Importe_Junio + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Importe_Julio + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Importe_Agosto + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Importe_Septiembre + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Importe_Octubre + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Importe_Noviembre + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Importe_Diciembre + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Importe_Total + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Estatus + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Usuario_Modifico + ", ");
                    Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Campo_Fecha_Modifico + ") VALUES (");
                    Mi_SQL.Append("'" + Renglon["DEPENDENCIA_ID"].ToString() + "', ");
                    Mi_SQL.Append("'" + Renglon["FUENTE_FINANCIAMIENTO_ID"].ToString() + "',");
                    Mi_SQL.Append("'" + Renglon["PROYECTO_ID"].ToString() + "', ");
                    Mi_SQL.Append("'" + Renglon["CAPITULO_ID"].ToString() + "', ");
                    Mi_SQL.Append("'" + Renglon["PARTIDA_ID"].ToString() + "', ");
                    Mi_SQL.Append("'" + Renglon["PRODUCTO_ID"].ToString() + "', ");
                    Mi_SQL.Append("'" + Negocio.P_Anio_Presupuesto + "',");
                    Mi_SQL.Append("'" + Renglon["JUSTIFICACION"].ToString() + "', ");
                    Mi_SQL.Append(Convert.ToString(Ene) + ", ");
                    Mi_SQL.Append(Convert.ToString(Feb) + ", ");
                    Mi_SQL.Append(Convert.ToString(Mar) + ", ");
                    Mi_SQL.Append(Convert.ToString(Abr) + ", ");
                    Mi_SQL.Append(Convert.ToString(May) + ", ");
                    Mi_SQL.Append(Convert.ToString(Jun) + ", ");
                    Mi_SQL.Append(Convert.ToString(Jul) + ", ");
                    Mi_SQL.Append(Convert.ToString(Ago) + ", ");
                    Mi_SQL.Append(Convert.ToString(Sep) + ", ");
                    Mi_SQL.Append(Convert.ToString(Oct) + ", ");
                    Mi_SQL.Append(Convert.ToString(Nov) + ", ");
                    Mi_SQL.Append(Convert.ToString(Dic) + ", ");
                    Mi_SQL.Append(Renglon["ENERO"].ToString().Replace(",", "") + ", ");
                    Mi_SQL.Append(Renglon["FEBRERO"].ToString().Replace(",", "") + ", ");
                    Mi_SQL.Append(Renglon["MARZO"].ToString().Replace(",", "") + ", ");
                    Mi_SQL.Append(Renglon["ABRIL"].ToString().Replace(",", "") + ", ");
                    Mi_SQL.Append(Renglon["MAYO"].ToString().Replace(",", "") + ", ");
                    Mi_SQL.Append(Renglon["JUNIO"].ToString().Replace(",", "") + ", ");
                    Mi_SQL.Append(Renglon["JULIO"].ToString().Replace(",", "") + ", ");
                    Mi_SQL.Append(Renglon["AGOSTO"].ToString().Replace(",", "") + ", ");
                    Mi_SQL.Append(Renglon["SEPTIEMBRE"].ToString().Replace(",", "") + ", ");
                    Mi_SQL.Append(Renglon["OCTUBRE"].ToString().Replace(",", "") + ", ");
                    Mi_SQL.Append(Renglon["NOVIEMBRE"].ToString().Replace(",", "") + ", ");
                    Mi_SQL.Append(Renglon["DICIEMBRE"].ToString().Replace(",", "") + ", ");
                    Mi_SQL.Append(Renglon["IMPORTE_TOTAL"].ToString().Replace(",", "") + ", ");
                    Mi_SQL.Append("'" + Negocio.P_Estatus + "',");
                    Mi_SQL.Append("'" + Negocio.P_Usuario_Modifico + "',");
                    Mi_SQL.Append("SYSDATE)");

                    Cmd.CommandText = Mi_SQL.ToString();
                    Cmd.ExecuteNonQuery();
                }
                Trans.Commit();
                Operacion_Exitosa = true;
            }
            catch (Exception Ex)
            {
                Operacion_Exitosa = false;
                Trans.Rollback();
                throw new Exception("Error al intentar modificar los registros. Error: [" + Ex.Message + "]");
            }
            return Operacion_Exitosa;
        }

        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Partida_Autorizadas
        ///DESCRIPCIÓN          : consulta para obtener las partidas autorizadas
        ///PARAMETROS           1 Negocio conexion con la capa de negocio 
        ///CREO                 : Leslie Gonzalez Vázquez
        ///FECHA_CREO           : 25/Noviembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static DataTable Consultar_Partida_Autorizadas(Cls_Ope_Psp_Asignar_Partida_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            try
            {
                Mi_SQL.Append("SELECT " + Ope_Psp_Presupuesto_Aprobado.Campo_Dependencia_ID + ", ");
                Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID + ", ");
                Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Proyecto_Programa_ID + ", ");
                Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Capitulo_ID + ", ");
                Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Partida_ID + ", ");
                Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Anio);
                Mi_SQL.Append(" FROM " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado);
                Mi_SQL.Append(" WHERE " + Ope_Psp_Presupuesto_Aprobado.Campo_Anio + " = '" + Negocio.P_Anio_Presupuesto + "'");
                Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Aprobado.Campo_Dependencia_ID + " = '" + Negocio.P_Dependencia_ID + "'");

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros de las partidas asignadas. Error: [" + Ex.Message + "]");
            }
        }

        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Dependencias_Presupuestadas
        ///DESCRIPCIÓN          : consulta para obtener las dependencias presupuestadas
        ///PARAMETROS           :
        ///CREO                 : Leslie Gonzalez Vázquez
        ///FECHA_CREO           : 28/Noviembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static DataTable Consultar_Dependencias_Presupuestadas()
        {
            StringBuilder Mi_SQL = new StringBuilder();
            try
            {
                Mi_SQL.Append(" SELECT " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Dependencia_ID + ", ");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Anio + ", ");
                Mi_SQL.Append("SUM(" + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Importe_Total + ") AS TOTAL, ");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Estatus + ", ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " || ' ' || ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA ");
                Mi_SQL.Append(" FROM " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu);
                Mi_SQL.Append(" INNER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias);
                Mi_SQL.Append(" ON " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Dependencia_ID);
                Mi_SQL.Append(" = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);
                Mi_SQL.Append(" WHERE " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Estatus + " = 'GENERADO'");
                Mi_SQL.Append(" GROUP BY " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Dependencia_ID + ", ");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Anio + ", ");
                Mi_SQL.Append(Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu + "." + Ope_Psp_Calendarizacion_Presu.Campo_Estatus + ", ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + ", ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre);
                Mi_SQL.Append(" ORDER BY " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros de las partidas asignadas. Error: [" + Ex.Message + "]");
            }
        }

        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Guardar_Historial_Calendario
        ///DESCRIPCIÓN          : Consulta para guardar los comentarios del presupuesto
        ///PARAMETROS           1 Negocio conexion con la capa de negocio 
        ///CREO                 : Leslie Gonzalez Vázquez
        ///FECHA_CREO           : 29/Noviembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static Boolean Guardar_Historial_Calendario(Cls_Ope_Psp_Asignar_Partida_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            Boolean Operacion_Exitosa = false;

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
                Mi_SQL.Append("UPDATE " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu);
                Mi_SQL.Append(" SET " + Ope_Psp_Calendarizacion_Presu.Campo_Estatus + " = '" + Negocio.P_Estatus + "'");
                Mi_SQL.Append(" WHERE " + Ope_Psp_Calendarizacion_Presu.Campo_Anio + " = '" + Negocio.P_Anio_Presupuesto + "'");
                Mi_SQL.Append(" AND " + Ope_Psp_Calendarizacion_Presu.Campo_Dependencia_ID + " = '" + Negocio.P_Dependencia_ID + "'");
                Mi_SQL.Append(" AND " + Ope_Psp_Calendarizacion_Presu.Campo_Estatus + " = 'GENERADO'");
                Cmd.CommandText = Mi_SQL.ToString();
                Cmd.ExecuteNonQuery();

                Mi_SQL = new StringBuilder();

                Mi_SQL.Append("INSERT INTO " + Ope_Psp_Hist_Calendar_Presu.Tabla_Ope_Psp_Hist_Calendar_Presu + "(");
                Mi_SQL.Append(Ope_Psp_Hist_Calendar_Presu.Campo_Anio + ", ");
                Mi_SQL.Append(Ope_Psp_Hist_Calendar_Presu.Campo_Dependencia_ID + ", ");
                Mi_SQL.Append(Ope_Psp_Hist_Calendar_Presu.Campo_Comentario + ", ");
                Mi_SQL.Append(Ope_Psp_Hist_Calendar_Presu.Campo_Usuario_Creo + ", ");
                Mi_SQL.Append(Ope_Psp_Hist_Calendar_Presu.Campo_Fecha_Creo + ") VALUES(");
                Mi_SQL.Append("'" + Negocio.P_Anio_Presupuesto + "', ");
                Mi_SQL.Append("'" + Negocio.P_Dependencia_ID + "', ");
                Mi_SQL.Append("'" + Negocio.P_Comentario + "', ");
                Mi_SQL.Append("'" + Negocio.P_Usuario_Creo + "', ");
                Mi_SQL.Append("SYSDATE)");
                Cmd.CommandText = Mi_SQL.ToString();
                Cmd.ExecuteNonQuery();
                
                if(Negocio.P_Estatus.Equals("AUTORIZADO"))
                {
                    foreach (DataRow Dr in Negocio.P_Dt_Datos.Rows) 
                    {
                        Mi_SQL = new StringBuilder();
                        Mi_SQL.Append("INSERT INTO " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "(");
                        Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Dependencia_ID + ", ");
                        Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID + ", ");
                        Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Proyecto_Programa_ID + ", ");
                        Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Capitulo_ID + ", ");
                        Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Partida_ID + ", ");
                        Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Anio + ", ");
                        Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Aprobado + ", ");
                        Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Ampliacion + ", ");
                        Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Reduccion + ", ");
                        Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Modificado + ", ");
                        Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Devengado + ", ");
                        Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Ejercido + ", ");
                        Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Pagado + ", ");
                        Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Pre_Comprometido + ", ");
                        Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Comprometido + ", ");
                        Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Disponible + ", ");
                        Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Saldo + ", ");
                        Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Enero + ", ");
                        Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Febrero + ", ");
                        Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Marzo + ", ");
                        Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Abril + ", ");
                        Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Mayo + ", ");
                        Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Junio + ", ");
                        Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Julio + ", ");
                        Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Agosto + ", ");
                        Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Septiembre + ", ");
                        Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Octubre + ", ");
                        Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Noviembre + ", ");
                        Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Diciembre + ", ");
                        Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Total + ", ");
                        Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Usuario_Creo + ", ");
                        Mi_SQL.Append(Ope_Psp_Presupuesto_Aprobado.Campo_Fecha_Creo + ") VALUES(");
                        Mi_SQL.Append("'" + Dr["DEPENDENCIA_ID"].ToString() + "', ");
                        Mi_SQL.Append("'" + Dr["FUENTE_FINANCIAMIENTO_ID"].ToString() + "', ");
                        Mi_SQL.Append("'" + Dr["PROYECTO_ID"].ToString() + "', ");
                        Mi_SQL.Append("'" + Dr["CAPITULO_ID"].ToString() + "', ");
                        Mi_SQL.Append("'" + Dr["PARTIDA_ID"].ToString() + "', ");
                        Mi_SQL.Append("'" + Negocio.P_Anio_Presupuesto + "', ");
                        Mi_SQL.Append("" + Dr["TOTAL"].ToString().Replace(",", "") + ", ");
                        Mi_SQL.Append("0.00, ");
                        Mi_SQL.Append("0.00, ");
                        Mi_SQL.Append("0.00, ");
                        Mi_SQL.Append("0.00, ");
                        Mi_SQL.Append("0.00, ");
                        Mi_SQL.Append("0.00, ");
                        Mi_SQL.Append("0.00, ");
                        Mi_SQL.Append("0.00, ");
                        Mi_SQL.Append(Dr["TOTAL"].ToString().Replace(",", "") + ", ");
                        Mi_SQL.Append(Dr["TOTAL"].ToString().Replace(",", "") + ", ");
                        Mi_SQL.Append(Dr["TOTAL_ENE"].ToString().Replace(",", "") + ", ");
                        Mi_SQL.Append(Dr["TOTAL_FEB"].ToString().Replace(",", "") + ", ");
                        Mi_SQL.Append(Dr["TOTAL_MAR"].ToString().Replace(",", "") + ", ");
                        Mi_SQL.Append(Dr["TOTAL_ABR"].ToString().Replace(",", "") + ", ");
                        Mi_SQL.Append(Dr["TOTAL_MAY"].ToString().Replace(",", "") + ", ");
                        Mi_SQL.Append(Dr["TOTAL_JUN"].ToString().Replace(",", "") + ", ");
                        Mi_SQL.Append(Dr["TOTAL_JUL"].ToString().Replace(",", "") + ", ");
                        Mi_SQL.Append(Dr["TOTAL_AGO"].ToString().Replace(",", "") + ", ");
                        Mi_SQL.Append(Dr["TOTAL_SEP"].ToString().Replace(",", "") + ", ");
                        Mi_SQL.Append(Dr["TOTAL_OCT"].ToString().Replace(",", "") + ", ");
                        Mi_SQL.Append(Dr["TOTAL_NOV"].ToString().Replace(",", "") + ", ");
                        Mi_SQL.Append(Dr["TOTAL_DIC"].ToString().Replace(",", "") + ", ");
                        Mi_SQL.Append(Dr["TOTAL"].ToString().Replace(",", "") + ", ");
                        Mi_SQL.Append("'" + Negocio.P_Usuario_Creo + "', ");
                        Mi_SQL.Append("SYSDATE)");

                        Cmd.CommandText = Mi_SQL.ToString();
                        Cmd.ExecuteNonQuery();
                    }
                }
                Trans.Commit();
                Operacion_Exitosa = true;
            }
            catch (Exception Ex)
            {
                Operacion_Exitosa = false;
                Trans.Rollback();
                throw new Exception("Error al intentar guardar el historial de los registros. Error: [" + Ex.Message + "]");
            }
            return Operacion_Exitosa;
        }
        #endregion
    }
}
