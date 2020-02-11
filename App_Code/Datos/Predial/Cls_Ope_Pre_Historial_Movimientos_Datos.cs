using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using Presidencia.Constantes;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Operacion_Predial_Historial_Movimientos.Negocio;

/// <summary>
/// Summary description for Cls_Ope_Pre_Historial_Movimientos_Datos
/// </summary>
namespace Presidencia.Operacion_Predial_Historial_Movimientos.Datos
{
    public class Cls_Ope_Pre_Historial_Movimientos_Datos
    {
       


             ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Historial_Movimientos
            ///DESCRIPCIÓN: Hace una consulta a la Base de Datos y obtiene los datos en un
            ///             DataTable.
            ///PARAMETROS:     
            ///             1.  Cuenta Predial ID
            ///CREO: Christian Perez Ibarra.
            ///FECHA_CREO: 08/Agosto/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            public static DataTable Consultar_Historial_Movimientos(Cls_Ope_Pre_Historial_Movimientos_Negocio Datos)
            {
                DataTable Dt_Traslado_Dominio = new DataTable();
                DataTable Tabla = new DataTable();
                try
                {
                    String Mi_SQL = "SELECT  ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + "." + Cat_Pre_Movimientos.Campo_Movimiento_ID  + ",";
                    Mi_SQL = Mi_SQL + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + "." + Cat_Pre_Movimientos.Campo_Fecha_Creo  + " as Fecha,";
                    Mi_SQL = Mi_SQL + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + "." + Cat_Pre_Movimientos.Campo_Identificador + " ||' - '|| ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + "." + Cat_Pre_Movimientos.Campo_Descripcion  + " as Clave_Movimiento,";
                    Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " ";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " inner join ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " on ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + "=";
                    Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " left join ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " on ";
                    Mi_SQL = Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + " = ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + "." + Cat_Pre_Movimientos.Campo_Movimiento_ID;
                    if ((Datos.P_Entre_Fecha != "" && Datos.P_Entre_Fecha != null) && (Datos.P_Y_Fecha != "" && Datos.P_Y_Fecha!=null))
                    {
                        if (Mi_SQL.Contains(" Where "))
                        {
                            Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + "." + Cat_Pre_Movimientos.Campo_Fecha_Creo + " >='" + Datos.P_Entre_Fecha + "' AND " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + "." + Cat_Pre_Movimientos.Campo_Fecha_Creo + " <= '" + Datos.P_Y_Fecha + "' ";
                        }
                        else
                        {
                            Mi_SQL = Mi_SQL + " Where " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + "." + Cat_Pre_Movimientos.Campo_Fecha_Creo + " >='" + Datos.P_Entre_Fecha + "' AND " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + "." + Cat_Pre_Movimientos.Campo_Fecha_Creo + " <= '" + Datos.P_Y_Fecha + "' ";
                        }
                    }
                    if (!string.IsNullOrEmpty(Datos.P_Entre_Movimiento  ) && string.IsNullOrEmpty(Datos.P_Y_Movimiento  ))
                    {
                        if (Mi_SQL.Contains(" Where "))
                        {
                            Mi_SQL = Mi_SQL + " AND '" + Datos.P_Entre_Movimiento + "' >= " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + "." + Cat_Pre_Movimientos.Campo_Movimiento_ID + " AND '" + Datos.P_Y_Movimiento + "' <=" + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + "." + Cat_Pre_Movimientos.Campo_Movimiento_ID + " ";
                        }
                        else
                        {
                            Mi_SQL = Mi_SQL + " Where '" + Datos.P_Entre_Movimiento + "' >= " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + "." + Cat_Pre_Movimientos.Campo_Movimiento_ID + " AND '" + Datos.P_Y_Movimiento + "' <=" + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + "." + Cat_Pre_Movimientos.Campo_Movimiento_ID + " ";
                        }
                    }
                    if (Datos.P_Tipo != null )
                    {
                        if (Mi_SQL.Contains(" Where "))
                        {
                            Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + "." + Cat_Pre_Movimientos.Campo_Movimiento_ID + "='" + Datos.P_Tipo + "' ";
                        }
                        else
                        {
                            Mi_SQL = Mi_SQL + " Where " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + "." + Cat_Pre_Movimientos.Campo_Movimiento_ID  + "='" + Datos.P_Tipo + "' ";
                        }
                    }
                    if (!string.IsNullOrEmpty(Datos.P_Cuenta_Predial))
                    {
                        if (Mi_SQL.Contains(" Where "))
                        {
                            Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + "='" + Datos.P_Cuenta_Predial + "'";
                        }
                        else
                        {
                            Mi_SQL = Mi_SQL + " Where " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + "='" + Datos.P_Cuenta_Predial + "'";
                        }
                    }
                    DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (dataset != null)
                    {
                        Tabla = dataset.Tables[0];
                    }
                }
                catch (Exception Ex)
                {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Tabla;
            }
    
        
    }
}
