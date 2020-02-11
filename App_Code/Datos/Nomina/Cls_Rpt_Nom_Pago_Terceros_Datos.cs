using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using System.Text;
using Presidencia.Pago_Terceros.Negocio;

namespace Presidencia.Pago_Terceros.Datos
{
    public class Cls_Rpt_Nom_Pago_Terceros_Datos
    {
        #region METODOS
            //*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Pago_Terceros
            ///DESCRIPCIÓN          : consulta para obtener los datos del reporte de pago a terceros
            ///PARAMETROS           1 Pagos_Negocio: conexion con la capa de negocios 
            ///CREO                 : Leslie Gonzalez Vázquez
            ///FECHA_CREO           : 17/Enero/2012
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            internal static DataTable Consultar_Pago_Terceros(Cls_Rpt_Nom_Pago_Terceros_Negocio Pagos_Negocio)
            {
                StringBuilder Mi_Sql = new StringBuilder();
                try
                {
                    Mi_Sql.Append("SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ");
                    Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                    Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                    Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " AS NOMBRE, ");
                    Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " || ' - ' ||");
                    Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS UNIDAD_RESPONSABLE,");
                    Mi_Sql.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina + " AS TIPO_NOMINA, ");
                    Mi_Sql.Append(Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Anio + " AS AÑO, ");
                    Mi_Sql.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina + " AS PERIODO, ");
                    Mi_Sql.Append(Cat_Nom_Terceros.Tabla_Cat_Nom_Terceros + "." + Cat_Nom_Terceros.Campo_Nombre + " AS NOMBRE_TERCEROS, ");
                    Mi_Sql.Append(Cat_Nom_Terceros.Tabla_Cat_Nom_Terceros + "." + Cat_Nom_Terceros.Campo_Porcentaje_Retencion + ", ");
                    Mi_Sql.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Clave + " || ' - ' ||");
                    Mi_Sql.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Nombre + " AS DEDUCCIÓN, ");
                    Mi_Sql.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_Monto);
                    Mi_Sql.Append(" FROM " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados);
                    Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados);
                    Mi_Sql.Append(" ON " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID);
                    Mi_Sql.Append(" = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);
                    Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias);
                    Mi_Sql.Append(" ON " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Dependencia_ID);
                    Mi_Sql.Append(" = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);
                    Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas);
                    Mi_Sql.Append(" ON " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Tipo_Nomina_ID);
                    Mi_Sql.Append(" = " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);
                    Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas);
                    Mi_Sql.Append(" ON " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID);
                    Mi_Sql.Append(" = " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Nomina_ID);
                    Mi_Sql.Append(" INNER JOIN " + Cat_Nom_Terceros.Tabla_Cat_Nom_Terceros);
                    Mi_Sql.Append(" ON " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Terceros_ID);
                    Mi_Sql.Append(" = " + Cat_Nom_Terceros.Tabla_Cat_Nom_Terceros + "." + Cat_Nom_Terceros.Campo_Tercero_ID);
                    Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion);
                    Mi_Sql.Append(" ON " + Cat_Nom_Terceros.Tabla_Cat_Nom_Terceros + "." + Cat_Nom_Terceros.Campo_Percepcion_Deduccion_ID);
                    Mi_Sql.Append(" = " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID);
                    Mi_Sql.Append(" LEFT OUTER JOIN " + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det);
                    Mi_Sql.Append(" ON " + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_Percepcion_Deduccion_ID);
                    Mi_Sql.Append(" = " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID);
                    Mi_Sql.Append(" AND " + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_No_Recibo);
                    Mi_Sql.Append(" = " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Recibo);

                    //filtro por numero de empleado
                    if (!String.IsNullOrEmpty(Pagos_Negocio.P_No_Empleado))
                    {
                        if (Mi_Sql.ToString().Contains("WHERE"))
                        {
                            Mi_Sql.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado);
                            Mi_Sql.Append(" = '" + Pagos_Negocio.P_No_Empleado + "'");
                        }
                        else
                        {
                            Mi_Sql.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado);
                            Mi_Sql.Append(" = '" + Pagos_Negocio.P_No_Empleado + "'");
                        }
                    }

                    //filtro por nombre de empleado
                    if (!String.IsNullOrEmpty(Pagos_Negocio.P_Nombre_Empleado))
                    {
                        if (Mi_Sql.ToString().Contains("WHERE"))
                        {
                            Mi_Sql.Append(" AND (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                            Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                            Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ")");
                            Mi_Sql.Append(" LIKE '%" + Pagos_Negocio.P_Nombre_Empleado.ToUpper() + "%'");
                        }
                        else
                        {
                            Mi_Sql.Append(" WHERE (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                            Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                            Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ")");
                            Mi_Sql.Append(" LIKE '%" + Pagos_Negocio.P_Nombre_Empleado.ToUpper() + "%'");
                        }
                    }

                    //filtro por año de la nomina
                    if (!String.IsNullOrEmpty(Pagos_Negocio.P_Nomina))
                    {
                        if (Mi_Sql.ToString().Contains("WHERE"))
                        {
                            Mi_Sql.Append(" AND " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID);
                            Mi_Sql.Append(" = '" + Pagos_Negocio.P_Nomina + "'");
                        }
                        else
                        {
                            Mi_Sql.Append(" WHERE " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID);
                            Mi_Sql.Append(" = '" + Pagos_Negocio.P_Nomina + "'");
                        }
                    }

                    //filtro por periodo
                    if (!String.IsNullOrEmpty(Pagos_Negocio.P_Periodo))
                    {
                        if (Mi_Sql.ToString().Contains("WHERE"))
                        {
                            Mi_Sql.Append(" AND " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina);
                            Mi_Sql.Append(" = '" + Pagos_Negocio.P_Periodo + "'");
                        }
                        else
                        {
                            Mi_Sql.Append(" WHERE " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina);
                            Mi_Sql.Append(" = '" + Pagos_Negocio.P_Periodo + "'");
                        }
                    }

                    Mi_Sql.Append(" ORDER BY " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + " ASC");

                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al intentar consultar los registros del reporte de pago a terceros. Error: [" + Ex.Message + "]");
                }
            }

            //*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Pago_Por_Deducciones_Diversas
            ///DESCRIPCIÓN          : consulta para obtener los datos del reporte de pago por deducciones diversas
            ///PARAMETROS           1 Pagos_Negocio: conexion con la capa de negocios 
            ///CREO                 : Leslie Gonzalez Vázquez
            ///FECHA_CREO           : 18/Enero/2012
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            internal static DataTable Consultar_Pago_Por_Deducciones_Diversas(Cls_Rpt_Nom_Pago_Terceros_Negocio Pagos_Negocio)
            {
                StringBuilder Mi_Sql = new StringBuilder();
                try
                {
                    Mi_Sql.Append("SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ");
                    Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                    Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                    Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " AS NOMBRE, ");
                    Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " || ' - ' ||");
                    Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS UNIDAD_RESPONSABLE,");
                    Mi_Sql.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina + " AS TIPO_NOMINA, ");
                    Mi_Sql.Append(Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Anio + " AS AÑO, ");
                    Mi_Sql.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina + " AS PERIODO, ");
                    Mi_Sql.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Clave + " || ' - ' ||");
                    Mi_Sql.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Nombre + " AS DEDUCCIÓN, ");
                    Mi_Sql.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_Monto);
                    Mi_Sql.Append(" FROM " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados);
                    Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados);
                    Mi_Sql.Append(" ON " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID);
                    Mi_Sql.Append(" = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);
                    Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias);
                    Mi_Sql.Append(" ON " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Dependencia_ID);
                    Mi_Sql.Append(" = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);
                    Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas);
                    Mi_Sql.Append(" ON " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Tipo_Nomina_ID);
                    Mi_Sql.Append(" = " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);
                    Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas);
                    Mi_Sql.Append(" ON " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID);
                    Mi_Sql.Append(" = " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Nomina_ID);
                    Mi_Sql.Append(" LEFT OUTER JOIN " + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det);
                    Mi_Sql.Append(" ON " + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_No_Recibo);
                    Mi_Sql.Append(" = " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Recibo);
                    Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion);
                    Mi_Sql.Append(" ON " + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_Percepcion_Deduccion_ID);
                    Mi_Sql.Append(" = " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID);

                    //filtro por numero de empleado
                    if (!String.IsNullOrEmpty(Pagos_Negocio.P_No_Empleado))
                    {
                        if (Mi_Sql.ToString().Contains("WHERE"))
                        {
                            Mi_Sql.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado);
                            Mi_Sql.Append(" = '" + Pagos_Negocio.P_No_Empleado + "'");
                        }
                        else
                        {
                            Mi_Sql.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado);
                            Mi_Sql.Append(" = '" + Pagos_Negocio.P_No_Empleado + "'");
                        }
                    }

                    //filtro por nombre de empleado
                    if (!String.IsNullOrEmpty(Pagos_Negocio.P_Nombre_Empleado))
                    {
                        if (Mi_Sql.ToString().Contains("WHERE"))
                        {
                            Mi_Sql.Append(" AND (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                            Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                            Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ")");
                            Mi_Sql.Append(" LIKE '%" + Pagos_Negocio.P_Nombre_Empleado.ToUpper() + "%'");
                        }
                        else
                        {
                            Mi_Sql.Append(" WHERE (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                            Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                            Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ")");
                            Mi_Sql.Append(" LIKE '%" + Pagos_Negocio.P_Nombre_Empleado.ToUpper() + "%'");
                        }
                    }

                    //filtro por año de la nomina
                    if (!String.IsNullOrEmpty(Pagos_Negocio.P_Nomina))
                    {
                        if (Mi_Sql.ToString().Contains("WHERE"))
                        {
                            Mi_Sql.Append(" AND " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID);
                            Mi_Sql.Append(" = '" + Pagos_Negocio.P_Nomina + "'");
                        }
                        else
                        {
                            Mi_Sql.Append(" WHERE " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID);
                            Mi_Sql.Append(" = '" + Pagos_Negocio.P_Nomina + "'");
                        }
                    }

                    //filtro por periodo
                    if (!String.IsNullOrEmpty(Pagos_Negocio.P_Periodo))
                    {
                        if (Mi_Sql.ToString().Contains("WHERE"))
                        {
                            Mi_Sql.Append(" AND " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina);
                            Mi_Sql.Append(" = '" + Pagos_Negocio.P_Periodo + "'");
                        }
                        else
                        {
                            Mi_Sql.Append(" WHERE " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina);
                            Mi_Sql.Append(" = '" + Pagos_Negocio.P_Periodo + "'");
                        }
                    }

                    //filtro por tipo de orden judicial
                    if (!String.IsNullOrEmpty(Pagos_Negocio.P_Tipo_Orden_Judicial))
                    {
                        if (Mi_Sql.ToString().Contains("WHERE"))
                        {
                            Mi_Sql.Append(" AND " + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_Percepcion_Deduccion_ID);
                            Mi_Sql.Append(" = '" + Pagos_Negocio.P_Tipo_Orden_Judicial + "'");
                        }
                        else
                        {
                            Mi_Sql.Append(" WHERE " + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_Percepcion_Deduccion_ID);
                            Mi_Sql.Append(" = '" + Pagos_Negocio.P_Tipo_Orden_Judicial + "'");
                        }
                    }

                    Mi_Sql.Append(" ORDER BY " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + " ASC");

                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al intentar consultar los registros del reporte de pago por deducciones diversas. Error: [" + Ex.Message + "]");
                }
            }

            //*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Tipos_Orden_Judicial
            ///DESCRIPCIÓN          : consulta para obtener los datos de los tipos de orden judicial
            ///PARAMETROS           :
            ///CREO                 : Leslie Gonzalez Vázquez
            ///FECHA_CREO           : 18/Enero/2012
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            internal static DataTable Consultar_Tipos_Orden_Judicial()
            {
                StringBuilder Mi_Sql = new StringBuilder();
                DataTable Dt_Orden_Judicial = new DataTable();
                DataTable Dt_Ordenes = new DataTable();
                DataRow Fila;
                try
                {
                    Mi_Sql.Append("SELECT " + Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Aguinaldo + " AS OJ_AGUINALDO, ");
                    Mi_Sql.Append(Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Indemnizacion + " AS OJ_INDEMNIZACION, ");
                    Mi_Sql.Append(Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Prima_Vacacional + " AS OJ_PRIMA_VACACIONAL, ");
                    Mi_Sql.Append(Cat_Nom_Parametros.Campo_Deduccion_Tipo_Desc_Orden_Judicial + " AS OJ_SUELDO");
                    Mi_Sql.Append(" FROM " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros);

                    Dt_Ordenes = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];

                    if(Dt_Ordenes != null)
                    {
                        if(Dt_Ordenes.Rows.Count > 0)
                        {
                            Dt_Orden_Judicial.Columns.Add("ID");
                            Dt_Orden_Judicial.Columns.Add("NOMBRE");

                            Fila = Dt_Orden_Judicial.NewRow();
                            Fila["ID"] = Dt_Ordenes.Rows[0]["OJ_AGUINALDO"].ToString();
                            Fila["NOMBRE"] = "ORDEN JUDICIAL AGUINALDO";
                            Dt_Orden_Judicial.Rows.Add(Fila);

                            Fila = Dt_Orden_Judicial.NewRow();
                            Fila["ID"] = Dt_Ordenes.Rows[0]["OJ_INDEMNIZACION"].ToString();
                            Fila["NOMBRE"] = "ORDEN JUDICIAL INDEMNIZACIÓN";
                            Dt_Orden_Judicial.Rows.Add(Fila);

                            Fila = Dt_Orden_Judicial.NewRow();
                            Fila["ID"] = Dt_Ordenes.Rows[0]["OJ_PRIMA_VACACIONAL"].ToString();
                            Fila["NOMBRE"] = "ORDEN JUDICIAL PRIMA VACACIONAL";
                            Dt_Orden_Judicial.Rows.Add(Fila);

                            Fila = Dt_Orden_Judicial.NewRow();
                            Fila["ID"] = Dt_Ordenes.Rows[0]["OJ_SUELDO"].ToString();
                            Fila["NOMBRE"] = "ORDEN JUDICIAL SUELDO";
                            Dt_Orden_Judicial.Rows.Add(Fila);
                        }
                    }

                    return Dt_Orden_Judicial;
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al intentar consultar los registros de los tipos de orden judicial. Error: [" + Ex.Message + "]");
                }
            }
        #endregion
    }
}
