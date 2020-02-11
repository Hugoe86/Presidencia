using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Recibo_Pago;
using System.Data;
using Presidencia.Rpt_Listado_Nomina_Empleado.Negocio;
using System.Data.OracleClient;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Text;
using Presidencia.Sessiones;

namespace Presidencia.Rpt_Listado_Nomina_Empleado.Datos
{
    public class Cls_Rpt_Nom_Listado_Nomina_Por_Empleado_Datos
    {
        #region (Consulta Recibos)
        public static DataTable Consultar_Recibos_Empleados(Cls_Rpt_Nom_Listado_Nomina_Por_Empleado_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            StringBuilder Mi_SQL_Aux = new StringBuilder();
            DataTable Dt_Recibos = null;

            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ");
                Mi_SQL.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") AS EMPLEADO, ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_RFC + ", ");
                Mi_SQL.Append("NVL(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Cuenta_Bancaria + ", ' ') AS NO_CUENTA, ");
                Mi_SQL.Append("NVL(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Tarjeta + ", ' ') AS NO_TARJETA, ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Codigo_Programatico + ", ");

                Mi_SQL.Append("(SELECT " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre +
                    " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias +
                    " WHERE " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "=" +
                    Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + ") AS UNIDAD_RESPONSABLE, ");
                Mi_SQL.Append("(SELECT " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina +
                    " FROM " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas +
                    " WHERE " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + "=" +
                    Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID + ") AS NOMINA, ");

                Mi_SQL.Append("('AÑO-PER   ' || (SELECT " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Anio +
                    " FROM " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas +
                    " WHERE " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Nomina_ID +
                    "=" + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + ") || '-' || ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados +"."+Ope_Nom_Recibos_Empleados.Campo_No_Nomina + ") AS PERIODO, ");

                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Recibo + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Dias_Trabajados + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Total_Nomina + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Total_Percepciones + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Total_Deducciones + ", ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio + ", ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin + ", ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Nombre + " AS PUESTO, ");
                Mi_SQL.Append(Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." + Cat_Nom_Bancos.Campo_Nombre + " AS BANCO, ");
                Mi_SQL.Append("('Del  ' || TO_CHAR(" + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio + ", 'DD-Mon-YYYY')");
                Mi_SQL.Append(" || '  Al  ' || ");
                Mi_SQL.Append("TO_CHAR(" + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin + ", 'DD-Mon-YYYY')");
                Mi_SQL.Append(") AS FECHAS ");
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados);
                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " ON ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);
                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + " ON ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Detalle_Nomina_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Detalle_Nomina_ID);
                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Puestos.Tabla_Cat_Puestos + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Puesto_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Puesto_ID);
                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Banco_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." + Cat_Nom_Bancos.Campo_Banco_ID);
                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);
                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);

                Mi_SQL.Append(" WHERE ");

                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + "= '" + Datos.P_Nomina_ID + "'");

                if (!String.IsNullOrEmpty(Datos.P_Periodo))
                {
                    Mi_SQL.Append(" AND ");
                    Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina + "= " + Datos.P_Periodo);
                }

                if (!String.IsNullOrEmpty(Datos.P_Tipo_Nomina_ID))
                {
                    Mi_SQL.Append(" AND ");
                    Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Tipo_Nomina_ID + "= '" + Datos.P_Tipo_Nomina_ID + "'");
                }

                if (!String.IsNullOrEmpty(Datos.P_No_Empleado))
                {
                    Mi_SQL.Append(" AND ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "'");
                }

                if (!String.IsNullOrEmpty(Datos.P_Nombre_Empleado))
                {
                    Mi_SQL.Append(" AND (");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") LIKE '%" + Datos.P_Nombre_Empleado + "%' ");

                    Mi_SQL.Append(" OR (");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre  + " || ' ' || ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno  + ") LIKE '%" + Datos.P_Nombre_Empleado + "%' ");
                }

                if (!String.IsNullOrEmpty(Datos.P_Departamento))
                {
                    Mi_SQL.Append(" AND ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + "='" + Datos.P_Departamento + "'");
                }

                if (!String.IsNullOrEmpty(Datos.P_Banco_ID))
                {
                    Mi_SQL.Append(" AND ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Banco_ID + "='" + Datos.P_Banco_ID + "'");
                }

                Mi_SQL.Append(" ORDER BY " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Banco_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + ", ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " ASC");

                Dt_Recibos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];


                Dt_Recibos.Columns.Add("USUARIO", typeof(String));

                if (Dt_Recibos is DataTable)
                {
                    if (Dt_Recibos.Rows.Count > 0)
                    {
                        foreach (DataRow INCAPACIDAD in Dt_Recibos.Rows)
                        {
                            if (INCAPACIDAD is DataRow)
                            {
                                INCAPACIDAD["USUARIO"] = "Elaboro: " + Cls_Sessiones.Nombre_Empleado;
                            }
                        }
                    }
                }

            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los recibos de nómina de los empleados. Error: [" + Ex.Message + "]");
            }
            return Dt_Recibos;
        }

        public static DataTable Consultar_Percepciones_Recibo_Empleado(Cls_Rpt_Nom_Listado_Nomina_Por_Empleado_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Percepiones_Recibo = null;

            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Clave + ", ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Nombre + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_Monto + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_No_Recibo);

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados);

                Mi_SQL.Append(" RIGHT OUTER JOIN " + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + " ON ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Recibo);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_No_Recibo);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + " ON ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_Percepcion_Deduccion_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " ON ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);

                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Tipo + "='PERCEPCION'");

                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + "= '" + Datos.P_Nomina_ID + "'");

                if (!String.IsNullOrEmpty(Datos.P_Periodo))
                {
                    Mi_SQL.Append(" AND ");
                    Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina + "= " + Datos.P_Periodo);
                }

                if (!String.IsNullOrEmpty(Datos.P_Tipo_Nomina_ID))
                {
                    Mi_SQL.Append(" AND ");
                    Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Tipo_Nomina_ID + "= '" + Datos.P_Tipo_Nomina_ID + "'");
                }

                if (!String.IsNullOrEmpty(Datos.P_No_Empleado))
                {
                    Mi_SQL.Append(" AND ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "'");
                }

                if (!String.IsNullOrEmpty(Datos.P_Nombre_Empleado))
                {
                    Mi_SQL.Append(" AND (");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") LIKE '%" + Datos.P_Nombre_Empleado + "%' ");

                    Mi_SQL.Append(" OR (");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " || ' ' || ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ") LIKE '%" + Datos.P_Nombre_Empleado + "%' ");
                }

                if (!String.IsNullOrEmpty(Datos.P_Departamento))
                {
                    Mi_SQL.Append(" AND ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + "='" + Datos.P_Departamento + "'");
                }

                if (!String.IsNullOrEmpty(Datos.P_Banco_ID))
                {
                    Mi_SQL.Append(" AND ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Banco_ID + "='" + Datos.P_Banco_ID + "'");
                }

                Mi_SQL.Append(" ORDER BY " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Banco_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + ", ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + ", ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Clave + " ASC");

                Dt_Percepiones_Recibo = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las percepciones del recibo del empleado. Error: [" + Ex.Message + "]");
            }
            return Dt_Percepiones_Recibo;
        }

        public static DataTable Consultar_Deducciones_Recibo_Empleado(Cls_Rpt_Nom_Listado_Nomina_Por_Empleado_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Deducciones_Recibo = null;

            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Clave + ", ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Nombre + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_Monto + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_No_Recibo);

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados);

                Mi_SQL.Append(" RIGHT OUTER JOIN " + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + " ON ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Recibo);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_No_Recibo);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + " ON ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_Percepcion_Deduccion_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " ON ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);

                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Tipo + "='DEDUCCION'");


                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + "= '" + Datos.P_Nomina_ID + "'");

                if (!String.IsNullOrEmpty(Datos.P_Periodo))
                {
                    Mi_SQL.Append(" AND ");
                    Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina + "= " + Datos.P_Periodo);
                }

                if (!String.IsNullOrEmpty(Datos.P_Tipo_Nomina_ID))
                {
                    Mi_SQL.Append(" AND ");
                    Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Tipo_Nomina_ID + "= '" + Datos.P_Tipo_Nomina_ID + "'");
                }

                if (!String.IsNullOrEmpty(Datos.P_No_Empleado))
                {
                    Mi_SQL.Append(" AND ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "'");
                }

                if (!String.IsNullOrEmpty(Datos.P_Nombre_Empleado))
                {
                    Mi_SQL.Append(" AND (");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") LIKE '%" + Datos.P_Nombre_Empleado + "%' ");

                    Mi_SQL.Append(" OR (");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " || ' ' || ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ") LIKE '%" + Datos.P_Nombre_Empleado + "%' ");
                }

                if (!String.IsNullOrEmpty(Datos.P_Departamento))
                {
                    Mi_SQL.Append(" AND ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + "='" + Datos.P_Departamento + "'");
                }

                if (!String.IsNullOrEmpty(Datos.P_Banco_ID))
                {
                    Mi_SQL.Append(" AND ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Banco_ID + "='" + Datos.P_Banco_ID + "'");
                }

                Mi_SQL.Append(" ORDER BY " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Banco_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + ", ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + ", ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Clave + " ASC");

                Dt_Deducciones_Recibo = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las percepciones del recibo del empleado. Error: [" + Ex.Message + "]");
            }
            return Dt_Deducciones_Recibo;
        }
        #endregion
    }
}
