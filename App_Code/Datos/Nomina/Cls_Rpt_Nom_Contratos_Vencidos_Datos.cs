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
using Presidencia.Contratos_Vencidos.Negocio;

namespace Presidencia.Contratos_Vencidos.Datos
{
    public class Cls_Rpt_Nom_Contratos_Vencidos_Datos
    {
        #region METODOS

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Contratos_Vencidos
            ///DESCRIPCIÓN          : consulta para obtener los datos de los contratos vencidos
            ///PARAMETROS           1 Contratos_Negocio: conexion con la capa de negocios 
            ///CREO                 : Leslie Gonzalez Vázquez
            ///FECHA_CREO           : 13/Enero/2012
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            internal static DataTable Consultar_Contratos_Vencidos(Cls_Rpt_Nom_Contratos_Vencidos_Negocio Contratos_Negocio)
            {
                StringBuilder Mi_Sql = new StringBuilder();
                try
                {
                    Mi_Sql.Append("SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ");
                    Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                    Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                    Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " AS NOMBRE, ");
                    Mi_Sql.Append("'' AS FECHA_TERMINO_CONTRATO, ");
                    Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Fecha_Termino_Contrato + " AS FECHA, ");
                    Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + "|| ' - ' ||");
                    Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS UNIDAD_RESPONSABLE,");
                    Mi_Sql.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina + " AS TIPO_NOMINA");
                    Mi_Sql.Append(" FROM " + Cat_Empleados.Tabla_Cat_Empleados);
                    Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias);
                    Mi_Sql.Append(" ON " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID);
                    Mi_Sql.Append(" = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);
                    Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas);
                    Mi_Sql.Append(" ON " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID);
                    Mi_Sql.Append(" = " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);

                    //filtro por numero de empleado
                    if (!String.IsNullOrEmpty(Contratos_Negocio.P_No_Empleado)) 
                    {
                        if (Mi_Sql.ToString().Contains("WHERE"))
                        {
                            Mi_Sql.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado);
                            Mi_Sql.Append(" = '" + Contratos_Negocio.P_No_Empleado + "'");
                        }
                        else 
                        {
                            Mi_Sql.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado);
                            Mi_Sql.Append(" = '" + Contratos_Negocio.P_No_Empleado + "'");
                        }
                    }

                    //filtro por nombre de empleado
                    if (!String.IsNullOrEmpty(Contratos_Negocio.P_Nombre_Empleado))
                    {
                        if (Mi_Sql.ToString().Contains("WHERE"))
                        {
                            Mi_Sql.Append(" AND (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                            Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                            Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ")");
                            Mi_Sql.Append(" LIKE '%" + Contratos_Negocio.P_Nombre_Empleado.ToUpper() + "%'");
                        }
                        else
                        {
                            Mi_Sql.Append(" WHERE (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                            Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                            Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ")");
                            Mi_Sql.Append(" LIKE '%" + Contratos_Negocio.P_Nombre_Empleado.ToUpper() + "%'");
                        }
                    }

                    //filtro por rangos de fechas
                    if (!String.IsNullOrEmpty(Contratos_Negocio.P_Fecha_Inicio) && !String.IsNullOrEmpty(Contratos_Negocio.P_Fecha_Fin))
                    {
                        if (Mi_Sql.ToString().Contains("WHERE"))
                        {
                            Mi_Sql.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados  + "." + Cat_Empleados.Campo_Fecha_Termino_Contrato);
                            Mi_Sql.Append(" BETWEEN TO_DATE ('" + Contratos_Negocio.P_Fecha_Inicio + " 00:00:00', 'DD/MM/YYYY HH24:MI:SS')");
                            Mi_Sql.Append(" AND TO_DATE('" + Contratos_Negocio.P_Fecha_Fin + " 23:59:00', 'DD/MM/YYYY HH24:MI:SS')");
                        }
                        else
                        {
                            Mi_Sql.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Fecha_Termino_Contrato);
                            Mi_Sql.Append(" BETWEEN TO_DATE ('" + Contratos_Negocio.P_Fecha_Inicio + " 00:00:00', 'DD/MM/YYYY HH24:MI:SS')");
                            Mi_Sql.Append(" AND TO_DATE('" + Contratos_Negocio.P_Fecha_Fin + " 23:59:00', 'DD/MM/YYYY HH24:MI:SS')");
                        }
                    }

                    Mi_Sql.Append(" ORDER BY " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Fecha_Termino_Contrato + " ASC");

                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al intentar consultar los registros de los contratos vencidos. Error: [" + Ex.Message + "]");
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Sueldos_Netos
            ///DESCRIPCIÓN          : consulta para obtener los datos de los sueldos netos
            ///PARAMETROS           1 Contratos_Negocio: conexion con la capa de negocios 
            ///CREO                 : Leslie Gonzalez Vázquez
            ///FECHA_CREO           : 14/Enero/2012
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            internal static DataTable Consultar_Sueldos_Netos(Cls_Rpt_Nom_Contratos_Vencidos_Negocio Contratos_Negocio)
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
                    Mi_Sql.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Total_Nomina + " AS TOTAL");
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


                    //filtro por numero de empleado
                    if (!String.IsNullOrEmpty(Contratos_Negocio.P_No_Empleado))
                    {
                        if (Mi_Sql.ToString().Contains("WHERE"))
                        {
                            Mi_Sql.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado);
                            Mi_Sql.Append(" = '" + Contratos_Negocio.P_No_Empleado + "'");
                        }
                        else
                        {
                            Mi_Sql.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado);
                            Mi_Sql.Append(" = '" + Contratos_Negocio.P_No_Empleado + "'");
                        }
                    }

                    //filtro por nombre de empleado
                    if (!String.IsNullOrEmpty(Contratos_Negocio.P_Nombre_Empleado))
                    {
                        if (Mi_Sql.ToString().Contains("WHERE"))
                        {
                            Mi_Sql.Append(" AND (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                            Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                            Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ")");
                            Mi_Sql.Append(" LIKE '%" + Contratos_Negocio.P_Nombre_Empleado.ToUpper() + "%'");
                        }
                        else
                        {
                            Mi_Sql.Append(" WHERE (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                            Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                            Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ")");
                            Mi_Sql.Append(" LIKE '%" + Contratos_Negocio.P_Nombre_Empleado.ToUpper() + "%'");
                        }
                    }

                    //filtro por año de la nomina
                    if (!String.IsNullOrEmpty(Contratos_Negocio.P_Nomina))
                    {
                        if (Mi_Sql.ToString().Contains("WHERE"))
                        {
                            Mi_Sql.Append(" AND " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID);
                            Mi_Sql.Append(" = '" + Contratos_Negocio.P_Nomina + "'");
                        }
                        else
                        {
                            Mi_Sql.Append(" WHERE " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID);
                            Mi_Sql.Append(" = '" + Contratos_Negocio.P_Nomina + "'");
                        }
                    }

                    //filtro por periodo
                    if (!String.IsNullOrEmpty(Contratos_Negocio.P_Periodo))
                    {
                        if (Mi_Sql.ToString().Contains("WHERE"))
                        {
                            Mi_Sql.Append(" AND " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina);
                            Mi_Sql.Append(" = '" + Contratos_Negocio.P_Periodo + "'");
                        }
                        else
                        {
                            Mi_Sql.Append(" WHERE " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina);
                            Mi_Sql.Append(" = '" + Contratos_Negocio.P_Periodo + "'");
                        }
                    }

                    //filtro por tipo_Nomina
                    if (!String.IsNullOrEmpty(Contratos_Negocio.P_Tipo_Nomina))
                    {
                        if (Mi_Sql.ToString().Contains("WHERE"))
                        {
                            Mi_Sql.Append(" AND " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Tipo_Nomina_ID);
                            Mi_Sql.Append(" = '" + Contratos_Negocio.P_Tipo_Nomina + "'");
                        }
                        else
                        {
                            Mi_Sql.Append(" WHERE " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Tipo_Nomina_ID);
                            Mi_Sql.Append(" = '" + Contratos_Negocio.P_Tipo_Nomina + "'");
                        }
                    }

                    Mi_Sql.Append(" ORDER BY " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + " ASC");

                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al intentar consultar los registros de los contratos vencidos. Error: [" + Ex.Message + "]");
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Totales_Efectivo
            ///DESCRIPCIÓN          : consulta para obtener los datos de los totales efectivos
            ///PARAMETROS           1 Contratos_Negocio: conexion con la capa de negocios 
            ///CREO                 : Leslie Gonzalez Vázquez
            ///FECHA_CREO           : 14/Enero/2012
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            internal static DataTable Consultar_Totales_Efectivo(Cls_Rpt_Nom_Contratos_Vencidos_Negocio Contratos_Negocio)
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
                    Mi_Sql.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Total_Percepciones + " AS TOTAL");
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


                    //filtro por numero de empleado
                    if (!String.IsNullOrEmpty(Contratos_Negocio.P_No_Empleado))
                    {
                        if (Mi_Sql.ToString().Contains("WHERE"))
                        {
                            Mi_Sql.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado);
                            Mi_Sql.Append(" = '" + Contratos_Negocio.P_No_Empleado+ "'");
                        }
                        else
                        {
                            Mi_Sql.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado);
                            Mi_Sql.Append(" = '" + Contratos_Negocio.P_No_Empleado + "'");
                        }
                    }

                    //filtro por nombre de empleado
                    if (!String.IsNullOrEmpty(Contratos_Negocio.P_Nombre_Empleado))
                    {
                        if (Mi_Sql.ToString().Contains("WHERE"))
                        {
                            Mi_Sql.Append(" AND (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                            Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                            Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ")");
                            Mi_Sql.Append(" LIKE '%" + Contratos_Negocio.P_Nombre_Empleado.ToUpper() + "%'");
                        }
                        else
                        {
                            Mi_Sql.Append(" WHERE (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                            Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                            Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ")");
                            Mi_Sql.Append(" LIKE '%" + Contratos_Negocio.P_Nombre_Empleado.ToUpper() + "%'");
                        }
                    }

                    //filtro por año de la nomina
                    if (!String.IsNullOrEmpty(Contratos_Negocio.P_Nomina))
                    {
                        if (Mi_Sql.ToString().Contains("WHERE"))
                        {
                            Mi_Sql.Append(" AND " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID);
                            Mi_Sql.Append(" = '" + Contratos_Negocio.P_Nomina + "'");
                        }
                        else
                        {
                            Mi_Sql.Append(" WHERE " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID);
                            Mi_Sql.Append(" = '" + Contratos_Negocio.P_Nomina + "'");
                        }
                    }

                    //filtro por periodo
                    if (!String.IsNullOrEmpty(Contratos_Negocio.P_Periodo))
                    {
                        if (Mi_Sql.ToString().Contains("WHERE"))
                        {
                            Mi_Sql.Append(" AND " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina);
                            Mi_Sql.Append(" = '" + Contratos_Negocio.P_Periodo + "'");
                        }
                        else
                        {
                            Mi_Sql.Append(" WHERE " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina);
                            Mi_Sql.Append(" = '" + Contratos_Negocio.P_Periodo + "'");
                        }
                    }

                    //filtro por tipo_Nomina
                    if (!String.IsNullOrEmpty(Contratos_Negocio.P_Tipo_Nomina))
                    {
                        if (Mi_Sql.ToString().Contains("WHERE"))
                        {
                            Mi_Sql.Append(" AND " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Tipo_Nomina_ID);
                            Mi_Sql.Append(" = '" + Contratos_Negocio.P_Tipo_Nomina + "'");
                        }
                        else
                        {
                            Mi_Sql.Append(" WHERE " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Tipo_Nomina_ID);
                            Mi_Sql.Append(" = '" + Contratos_Negocio.P_Tipo_Nomina + "'");
                        }
                    }

                    Mi_Sql.Append(" ORDER BY " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + " ASC");

                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al intentar consultar los registros de los contratos vencidos. Error: [" + Ex.Message + "]");
                }
            }
        #endregion
    }
}
