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
using System.Text;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
using Presidencia.Reporte_Basicos_Contabilidad.Negocio;

namespace Presidencia.Reporte_Basicos_Contabilidad.Datos
{
    public class Cls_Rpt_Con_Reporte_Basicos_Contabilidad_Datos
    {
        public Cls_Rpt_Con_Reporte_Basicos_Contabilidad_Datos()
        {
        }
        #region(Métodos)
            #region (Rpt_Balanza_Mensual)
                ///*******************************************************************************
                /// NOMBRE DE LA FUNCION: Consulta_Balanza_Mensual
                /// DESCRIPCION: Consulta los datos del Cierre mensual para poder tener los datos
                ///               de la balanza mensual
                /// PARAMETROS : Datos: Valores que son pasados desde la capa de negocios
                /// CREO       : Yazmin A Delgado Gómez
                /// FECHA_CREO : 18-Octubre-2011
                /// MODIFICO          :
                /// FECHA_MODIFICO    :
                /// CAUSA_MODIFICACION:
                ///*******************************************************************************
                public static DataTable Consulta_Balanza_Mensual(Cls_Rpt_Con_Reporte_Basicos_Contabilidad_Negocio Datos)
                {
                    StringBuilder Mi_SQL= new StringBuilder(); //Variable que tendra la consulta a realizar a la base de datos
                    try
                    {
                        //Consulta el balance del mes y año que se proporciono por parte del usuario filtrando por rango de cuentas o mostrando todas
                        Mi_SQL.Append("SELECT " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta + ", ");
                        Mi_SQL.Append(Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Descripcion + ", ");
                        Mi_SQL.Append(Ope_Con_Cierre_Mensual.Tabla_Ope_Con_Cierre_Mensual + "." + Ope_Con_Cierre_Mensual.Campo_Saldo_Inicial + ", ");
                        Mi_SQL.Append(Ope_Con_Cierre_Mensual.Tabla_Ope_Con_Cierre_Mensual + "." + Ope_Con_Cierre_Mensual.Campo_Saldo_Final + ", ");
                        Mi_SQL.Append(Ope_Con_Cierre_Mensual.Tabla_Ope_Con_Cierre_Mensual + "." + Ope_Con_Cierre_Mensual.Campo_Total_Debe + ", ");
                        Mi_SQL.Append(Ope_Con_Cierre_Mensual.Tabla_Ope_Con_Cierre_Mensual + "." + Ope_Con_Cierre_Mensual.Campo_Total_Haber);
                        Mi_SQL.Append(" FROM " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + ", " + Ope_Con_Cierre_Mensual.Tabla_Ope_Con_Cierre_Mensual);
                        Mi_SQL.Append(" WHERE " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID);
                        Mi_SQL.Append(" = " + Ope_Con_Cierre_Mensual.Tabla_Ope_Con_Cierre_Mensual + "." + Ope_Con_Cierre_Mensual.Campo_Cuenta_Contable_ID);
                        Mi_SQL.Append(" AND " + Ope_Con_Cierre_Mensual.Campo_Mes_Anio + " = '" + Datos.P_Mes_Anio + "'");
                        //Si se esta pidiendo filtrar entre rango de cuentas contables
                        if (!String.IsNullOrEmpty(Datos.P_Cuenta_Inicial) && !String.IsNullOrEmpty(Datos.P_Cuenta_Final))
                        {
                            Mi_SQL.Append(" AND (" + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta + " >= '" + Datos.P_Cuenta_Inicial + "'");
                            Mi_SQL.Append(" AND " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta + " <= '" + Datos.P_Cuenta_Final + "')");
                        }
                        else
                        {
                            //Si se esta pidiendo consulta cuentas contables mayores o iguales a la indico el usuario
                            if (!String.IsNullOrEmpty(Datos.P_Cuenta_Inicial) && String.IsNullOrEmpty(Datos.P_Cuenta_Final))
                            {
                                Mi_SQL.Append(" AND " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta + " >= '" + Datos.P_Cuenta_Inicial + "'");
                            }
                            else
                            {
                                //Si se esta pidiendo consultar cuentas contables menores o iguales a la que indico el usuario
                                if (String.IsNullOrEmpty(Datos.P_Cuenta_Inicial) && !String.IsNullOrEmpty(Datos.P_Cuenta_Final))
                                {
                                    Mi_SQL.Append(" AND " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta + " <= '" + Datos.P_Cuenta_Final + "'");
                                }   
                            }
                        }
                        //Solo mostrara saldos que no esten en ceros
                        if (Datos.P_Montos_Cero == "SI")
                        {
                            Mi_SQL.Append(" AND (" + Ope_Con_Cierre_Mensual.Campo_Saldo_Inicial + " <> 0");
                            Mi_SQL.Append(" OR " + Ope_Con_Cierre_Mensual.Campo_Saldo_Final + " <> 0");
                            Mi_SQL.Append(" OR " + Ope_Con_Cierre_Mensual.Campo_Total_Debe + " <> 0");
                            Mi_SQL.Append(" OR " + Ope_Con_Cierre_Mensual.Campo_Total_Haber + " <> 0)");
                        }
                        Mi_SQL.Append(" ORDER BY " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta);
                        return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                    }
                    catch (OracleException Ex)
                    {
                        throw new Exception("Error: " + Ex.Message);
                    }

                    catch (DBConcurrencyException Ex)
                    {
                        throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
                    }
                    catch (Exception Ex)
                    {
                        throw new Exception("Error: " + Ex.Message);
                    }
                    finally
                    {
                    }
                }
                ///*******************************************************************************
                /// NOMBRE DE LA FUNCION: Consulta_Detalles_Poliza_Balanza_Mensual
                /// DESCRIPCION: Consulta el total Haber y Debe de las cuentas contables del Mes
                ///              y Año proporcionado por el usuario
                /// PARAMETROS : Datos: Valores que son pasados desde la capa de negocios
                /// CREO       : Yazmin A Delgado Gómez
                /// FECHA_CREO : 18-Octubre-2011
                /// MODIFICO          :
                /// FECHA_MODIFICO    :
                /// CAUSA_MODIFICACION:
                ///*******************************************************************************
                public static DataTable Consulta_Balanza_Mensual_Debe_Haber(Cls_Rpt_Con_Reporte_Basicos_Contabilidad_Negocio Datos)
                {
                    StringBuilder Mi_SQL= new StringBuilder(); //Variable que tendra la consulta a realizar a la base de datos
                    try
                    {
                        //Consulta el balance del mes y año que se proporciono por parte del usuario filtrando por rango de cuentas o mostrando todas
                        Mi_SQL.Append("SELECT NVL(SUM(" + Ope_Con_Polizas_Detalles.Campo_Debe + "), 0) AS Total_Debe, ");
                        Mi_SQL.Append("NVL(SUM(" + Ope_Con_Polizas_Detalles.Campo_Haber + "), 0) AS Total_Haber");
                        Mi_SQL.Append(" FROM " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + ", " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles);
                        Mi_SQL.Append(" WHERE " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID);
                        Mi_SQL.Append(" = " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID);
                        Mi_SQL.Append(" AND " + Ope_Con_Polizas_Detalles.Campo_Mes_Ano + " = '" + Datos.P_Mes_Anio + "'");
                        //Si se esta pidiendo filtrar entre rango de cuentas contables
                        if (!String.IsNullOrEmpty(Datos.P_Cuenta_Inicial) && !String.IsNullOrEmpty(Datos.P_Cuenta_Final))
                        {
                            Mi_SQL.Append(" AND (" + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta + " >= '" + Datos.P_Cuenta_Inicial + "'");
                            Mi_SQL.Append(" AND " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta + " <= '" + Datos.P_Cuenta_Final + "')");
                        }
                        else
                        {
                            //Si se esta pidiendo consulta cuentas contables mayores o iguales a la indico el usuario
                            if (!String.IsNullOrEmpty(Datos.P_Cuenta_Inicial) && String.IsNullOrEmpty(Datos.P_Cuenta_Final))
                            {
                                Mi_SQL.Append(" AND " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta + " >= '" + Datos.P_Cuenta_Inicial + "'");
                            }
                            else
                            {
                                //Si se esta pidiendo consultar cuentas contables menores o iguales a la que indico el usuario
                                if (String.IsNullOrEmpty(Datos.P_Cuenta_Inicial) && !String.IsNullOrEmpty(Datos.P_Cuenta_Final))
                                {
                                    Mi_SQL.Append(" AND " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta + " <= '" + Datos.P_Cuenta_Final + "'");
                                }
                            }
                        }
                        return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                    }
                    catch (OracleException Ex)
                    {
                        throw new Exception("Error: " + Ex.Message);
                    }

                    catch (DBConcurrencyException Ex)
                    {
                        throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
                    }
                    catch (Exception Ex)
                    {
                        throw new Exception("Error: " + Ex.Message);
                    }
                    finally
                    {
                    }
                }
            #endregion
            #region (Rpt_Diario_General_CONAC)
                ///*******************************************************************************
                /// NOMBRE DE LA FUNCION: Consulta_Diario_General
                /// DESCRIPCION: Consulta todos los movimientos de pólizas realizados durante las
                ///              fechas proporcionadas por el usuario
                /// PARAMETROS : Datos: Valores que son pasados desde la capa de negocios
                /// CREO       : Yazmin A Delgado Gómez
                /// FECHA_CREO : 19-Octubre-2011
                /// MODIFICO          :
                /// FECHA_MODIFICO    :
                /// CAUSA_MODIFICACION:
                ///*******************************************************************************
                public static DataTable Consulta_Diario_General(Cls_Rpt_Con_Reporte_Basicos_Contabilidad_Negocio Datos)
                {
                    StringBuilder Mi_SQL = new StringBuilder(); //Variable que tendra la consulta a realizar a la base de datos

                    try
                    {
                        Mi_SQL.Append("SELECT (" + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_No_Poliza + ") AS No_Poliza, ");
                        Mi_SQL.Append("(" + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_Tipo_Poliza_ID + ") AS Tipo_Poliza_ID, ");
                        Mi_SQL.Append("(" + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_Mes_Ano + ") AS Mes_Anio, ");
                        Mi_SQL.Append("(" + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_Fecha_Poliza + ") AS Fecha_Poliza, ");
                        Mi_SQL.Append("(" + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_Concepto + ") AS Concepto, ");
                        Mi_SQL.Append("(" + Cat_Con_Tipo_Polizas.Tabla_Cat_Con_Tipo_Polizas + "." + Cat_Con_Tipo_Polizas.Campo_Abreviatura + ") AS Tipo_Poliza");
                        Mi_SQL.Append(" FROM " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + ", " + Cat_Con_Tipo_Polizas.Tabla_Cat_Con_Tipo_Polizas);
                        Mi_SQL.Append(" WHERE " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_Tipo_Poliza_ID);
                        Mi_SQL.Append(" = " + Cat_Con_Tipo_Polizas.Tabla_Cat_Con_Tipo_Polizas + "." + Cat_Con_Tipo_Polizas.Campo_Tipo_Poliza_ID);
                        Mi_SQL.Append(" AND " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_Fecha_Poliza);
                        Mi_SQL.Append(" BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicial + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')");
                        Mi_SQL.Append(" AND TO_DATE ('" + Datos.P_Fecha_Final + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS')");
                        Mi_SQL.Append(" ORDER BY " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_Fecha_Poliza);
                        return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                    }
                    catch (OracleException Ex)
                    {
                        throw new Exception("Error: " + Ex.Message);
                    }

                    catch (DBConcurrencyException Ex)
                    {
                        throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
                    }
                    catch (Exception Ex)
                    {
                        throw new Exception("Error: " + Ex.Message);
                    }
                    finally
                    {
                    }
                }
                ///*******************************************************************************
                /// NOMBRE DE LA FUNCION: Consulta_Diario_General_Detalles
                /// DESCRIPCION: Consulta todos los movimientos de pólizas realizados durante las
                ///              fechas proporcionadas por el usuario con respecto a su cuenta
                /// PARAMETROS : Datos: Valores que son pasados desde la capa de negocios
                /// CREO       : Yazmin A Delgado Gómez
                /// FECHA_CREO : 19-Octubre-2011
                /// MODIFICO          :
                /// FECHA_MODIFICO    :
                /// CAUSA_MODIFICACION:
                ///*******************************************************************************
                public static DataTable Consulta_Diario_General_Detalles(Cls_Rpt_Con_Reporte_Basicos_Contabilidad_Negocio Datos)
                {
                    StringBuilder Mi_SQL = new StringBuilder(); //Variable que tendra la consulta a realizar a la base de datos
                    try
                    {
                        Mi_SQL.Append("SELECT (" + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_No_Poliza + ") AS No_Poliza, ");
                        Mi_SQL.Append("(" + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_Tipo_Poliza_ID + ") AS Tipo_Poliza_ID, ");
                        Mi_SQL.Append("(" + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_Mes_Ano + ") AS Mes_Anio, ");
                        Mi_SQL.Append("(" + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta + ") AS Cuenta, ");
                        Mi_SQL.Append("(" + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Concepto + ") AS Concepto, ");
                        Mi_SQL.Append("(" + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Debe + ") AS Debe, ");
                        Mi_SQL.Append("(" + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Haber + ") AS Haber");
                        Mi_SQL.Append(" FROM " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + ", " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "," + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables);
                        Mi_SQL.Append(" WHERE " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID);
                        Mi_SQL.Append(" = " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID);
                        Mi_SQL.Append(" AND " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_No_Poliza);
                        Mi_SQL.Append(" = " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_No_Poliza);
                        Mi_SQL.Append(" AND " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_Tipo_Poliza_ID);
                        Mi_SQL.Append(" = " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Tipo_Poliza_ID);
                        Mi_SQL.Append(" AND " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_Mes_Ano);
                        Mi_SQL.Append(" = " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Mes_Ano);
                        Mi_SQL.Append(" AND " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_Fecha_Poliza);
                        Mi_SQL.Append(" BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicial + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')");
                        Mi_SQL.Append(" AND TO_DATE ('" + Datos.P_Fecha_Final + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS')");
                        Mi_SQL.Append(" ORDER BY " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_Fecha_Poliza);
                        return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                    }
                    catch (OracleException Ex)
                    {
                        throw new Exception("Error: " + Ex.Message);
                    }

                    catch (DBConcurrencyException Ex)
                    {
                        throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
                    }
                    catch (Exception Ex)
                    {
                        throw new Exception("Error: " + Ex.Message);
                    }
                    finally
                    {
                    }
                }
                ///*******************************************************************************
                /// NOMBRE DE LA FUNCION: Consulta_Libro_Diario
                /// DESCRIPCION: Consulta todos los movimientos de pólizas realizados durante las
                ///              fechas proporcionadas por el usuario con respecto a su cuenta
                /// PARAMETROS : Datos: Valores que son pasados desde la capa de negocios
                /// CREO       : Yazmin A Delgado Gómez
                /// FECHA_CREO : 14-Noviembre-2011
                /// MODIFICO          :
                /// FECHA_MODIFICO    :
                /// CAUSA_MODIFICACION:
                ///*******************************************************************************
                public static DataTable Consulta_Libro_Diario(Cls_Rpt_Con_Reporte_Basicos_Contabilidad_Negocio Datos)
                {
                    StringBuilder Mi_SQL = new StringBuilder(); //Consulta los datos del detalle 
                    try
                    {
                        Mi_SQL.Append("SELECT " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_Fecha_Poliza + " AS Fecha, ");
                        Mi_SQL.Append(Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_No_Poliza + " AS No_Poliza, 'POLIZAS' AS Documento_Fuente, ");
                        Mi_SQL.Append(Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Partida + " AS No_Asiento, ");
                        Mi_SQL.Append(Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Concepto + " AS Concepto_Detalle_Poliza, ");
                        Mi_SQL.Append(Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Debe + " AS Debe, ");
                        Mi_SQL.Append(Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Haber + " AS Haber, ");
                        Mi_SQL.Append(Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta + " AS Cuenta_Contable, ");
                        Mi_SQL.Append(Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Descripcion + " AS Concepto_Cuenta_Contable, ");
                        Mi_SQL.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Clave + " AS Cuenta_Presupuestal, ");
                        Mi_SQL.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Descripcion + " AS Concepto_Cuenta_Presupuestal");
                        Mi_SQL.Append(" FROM " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas);
                        Mi_SQL.Append(" LEFT OUTER JOIN " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles);
                        Mi_SQL.Append(" ON " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_No_Poliza);
                        Mi_SQL.Append(" = " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_No_Poliza);
                        Mi_SQL.Append(" AND " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_Fecha_Poliza);
                        Mi_SQL.Append(" BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicial + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')");
                        Mi_SQL.Append(" AND TO_DATE ('" + Datos.P_Fecha_Final + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS')");
                        Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables);
                        Mi_SQL.Append(" ON " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID);
                        Mi_SQL.Append(" = " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID);
                        Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas);
                        Mi_SQL.Append(" ON " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID);
                        Mi_SQL.Append(" = " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Partida_ID);
                        Mi_SQL.Append(" ORDER BY " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_Fecha_Poliza + ", ");
                        Mi_SQL.Append(Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_No_Poliza);
                        return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                    }
                    catch (OracleException Ex)
                    {
                        throw new Exception("Error: " + Ex.Message);
                    }

                    catch (DBConcurrencyException Ex)
                    {
                        throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
                    }
                    catch (Exception Ex)
                    {
                        throw new Exception("Error: " + Ex.Message);
                    }
                    finally
                    {
                    }
                }
            #endregion
            #region (Rpt_Libro_Mayor/Rpt_Tipo_Poliza)
                ///*******************************************************************************
                /// NOMBRE DE LA FUNCION: Consulta_Libro_Mayor
                /// DESCRIPCION: Consulta todos los movimientos que ha tenido la cuenta que fue
                ///              proporcionada por el usuario desde el primer mes hasta el mes
                ///              que proporciono el usuario
                /// PARAMETROS : Datos: Valores que son pasados desde la capa de negocios
                /// CREO       : Yazmin A Delgado Gómez
                /// FECHA_CREO : 07-Noviembre-2011
                /// MODIFICO          :
                /// FECHA_MODIFICO    :
                /// CAUSA_MODIFICACION:
                ///*******************************************************************************
                public static DataTable Consulta_Libro_Mayor(Cls_Rpt_Con_Reporte_Basicos_Contabilidad_Negocio Datos)
                {
                    StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la consulta a realizar a la base de datos
                    try
                    {
                        Mi_SQL.Append("SELECT " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_Fecha_Poliza + " AS Fecha, ");
                        Mi_SQL.Append(Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_No_Poliza + " AS No_Poliza, ");
                        Mi_SQL.Append(Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Haber + " AS Haber, ");
                        Mi_SQL.Append(Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Debe + " AS Debe, ");
                        Mi_SQL.Append(Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Concepto + " AS Concepto");
                        Mi_SQL.Append(" FROM " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + ", " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas);
                        Mi_SQL.Append(" WHERE " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_No_Poliza);
                        Mi_SQL.Append(" = " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_No_Poliza);
                        Mi_SQL.Append(" AND " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_Mes_Ano);
                        Mi_SQL.Append(" = " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Mes_Ano);
                        Mi_SQL.Append(" AND " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_Tipo_Poliza_ID);
                        Mi_SQL.Append(" = " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Tipo_Poliza_ID);
                        Mi_SQL.Append(" AND " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_Mes_Ano + " = '" + Datos.P_Mes_Anio + "'");
                        Mi_SQL.Append(" AND " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID + " = '" + Datos.P_Cuenta_Inicial + "'");
                        Mi_SQL.Append(" ORDER BY " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_Fecha_Poliza + ", ");
                        Mi_SQL.Append(Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_No_Poliza);
                        return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                    }
                    catch (OracleException Ex)
                    {
                        throw new Exception("Error: " + Ex.Message);
                    }

                    catch (DBConcurrencyException Ex)
                    {
                        throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
                    }
                    catch (Exception Ex)
                    {
                        throw new Exception("Error: " + Ex.Message);
                    }
                    finally
                    {
                    }
                }
                ///*******************************************************************************
                /// NOMBRE DE LA FUNCION: Consulta_Tipo_Poliza
                /// DESCRIPCION: Consulta los datos de las pólizas que fueron generadasde acuerdo
                ///              a los parametros seleccionados por el usuario
                /// PARAMETROS : Datos: Valores que son pasados desde la capa de negocios
                /// CREO       : José Antonio López Hernández
                /// FECHA_CREO : 09-Junio-2011
                /// MODIFICO          :
                /// FECHA_MODIFICO    :
                /// CAUSA_MODIFICACION:
                ///*******************************************************************************
                public static DataSet Consulta_Tipo_Poliza(Cls_Rpt_Con_Reporte_Basicos_Contabilidad_Negocio Datos)
                {
                    StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la consulta a realizar en la base de datos
                    try
                    {
                        Mi_SQL.Append("SELECT " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_No_Poliza + " AS No_Poliza, ");
                        Mi_SQL.Append(Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_Tipo_Poliza_ID + " AS Tipo_Poliza_ID, ");
                        Mi_SQL.Append(Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_Mes_Ano + " AS Mes_Ano, ");
                        Mi_SQL.Append(Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_Fecha_Poliza + " AS Fecha_Poliza, ");
                        Mi_SQL.Append(Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_Concepto + " AS Concepto_Poliza, ");
                        Mi_SQL.Append(Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Concepto + " AS Concepto_Partida, ");
                        Mi_SQL.Append("CAST(" + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Debe + " AS NUMERIC(12,2)) AS Debe, ");
                        Mi_SQL.Append("CAST(" + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Haber + " AS NUMERIC(12,2)) AS Haber, ");
                        Mi_SQL.Append("CAST(" + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Saldo + " AS NUMERIC(12,2)) AS Saldo, ");
                        Mi_SQL.Append(Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Fecha + " AS Fecha_Partida, ");
                        Mi_SQL.Append(Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta + " AS Cuenta_Contable, ");
                        Mi_SQL.Append(Cat_Con_Tipo_Polizas.Tabla_Cat_Con_Tipo_Polizas + "." + Cat_Con_Tipo_Polizas.Campo_Abreviatura + " AS Tipo_Poliza");
                        Mi_SQL.Append(" FROM " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + ", " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + ", ");
                        Mi_SQL.Append(Cat_Con_Tipo_Polizas.Tabla_Cat_Con_Tipo_Polizas + ", " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables);
                        Mi_SQL.Append(" WHERE " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_No_Poliza);
                        Mi_SQL.Append(" = " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_No_Poliza);
                        Mi_SQL.Append(" AND " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_Tipo_Poliza_ID);
                        Mi_SQL.Append(" = " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Tipo_Poliza_ID);
                        Mi_SQL.Append(" AND " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_Mes_Ano);
                        Mi_SQL.Append(" = " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Mes_Ano);
                        Mi_SQL.Append(" AND " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_Tipo_Poliza_ID); 
                        Mi_SQL.Append(" = " + Cat_Con_Tipo_Polizas.Tabla_Cat_Con_Tipo_Polizas + "." + Cat_Con_Tipo_Polizas.Campo_Tipo_Poliza_ID);
                        Mi_SQL.Append(" AND " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles + "." + Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID);
                        Mi_SQL.Append(" = " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID);
                        if (!String.IsNullOrEmpty(Datos.P_Tipo_Polizas)) Mi_SQL.Append(Datos.P_Tipo_Polizas);
                        if (!String.IsNullOrEmpty(Datos.P_Poliza_Inicial) && !String.IsNullOrEmpty(Datos.P_Poliza_Final))
                        {
                           Mi_SQL.Append(" AND CAST(" + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_No_Poliza + " AS NUMBER) >= " + int.Parse(Datos.P_Poliza_Inicial));
                           Mi_SQL.Append(" AND CAST(" + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_No_Poliza + " AS NUMBER) >= " + int.Parse(Datos.P_Poliza_Final));
                        }
                        Mi_SQL.Append(" AND " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_Fecha_Poliza);
                        Mi_SQL.Append(" BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicial + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')");
                        Mi_SQL.Append(" AND TO_DATE ('" + Datos.P_Fecha_Final + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS')");
                        DataSet Ds_Polizas = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                        return Ds_Polizas;
                    }
                    catch (OracleException Ex)
                    {
                        throw new Exception("Error: " + Ex.Message);
                    }

                    catch (DBConcurrencyException Ex)
                    {
                        throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
                    }
                    catch (Exception Ex)
                    {
                        throw new Exception("Error: " + Ex.Message);
                    }
                    finally
                    {
                    }
                }
            #endregion
        #endregion
    }
}