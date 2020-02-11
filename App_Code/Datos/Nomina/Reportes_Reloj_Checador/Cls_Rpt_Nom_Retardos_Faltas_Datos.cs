using System;
using System.IO;
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
using System.Collections.Generic;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Nomina_Reporte_Retardos_Faltas.Negocio;

namespace Presidencia.Nomina_Reporte_Retardos_Faltas.Datos
{
    public class Cls_Rpt_Nom_Retardos_Faltas_Datos
    {
        #region Metodos
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Faltas_Reporte
        /// DESCRIPCION : Consulta todos los datos del Empleado que selecciono el usuario
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        :Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 13/Marzo/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Faltas_Reporte(Cls_Rpt_Nom_Retardos_Faltas_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para la consulta para del Empleado
            DataTable Dt_Consulta = new DataTable();
            try
            {
                Mi_SQL.Append("Select * From " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado);
                Mi_SQL.Append(" Where TO_DATE(TO_CHAR(" + Ope_Nom_Faltas_Empleado.Campo_Fecha + ", 'DD-MM-YY')) >=' " + Datos.P_Fecha_Inicial + "'");
                Mi_SQL.Append(" AND TO_DATE(TO_CHAR(" + Ope_Nom_Faltas_Empleado.Campo_Fecha + ", 'DD-MM-YY')) <= '" + Datos.P_Fecha_Final + "'");
                //Mi_SQL.Append(" And " + Ope_Nom_Faltas_Empleado.Campo_Comentarios + "='GENERACION DE LISTA DE FALTAS Y RETARDOS POR RELOJ'");

                if (!string.IsNullOrEmpty(Datos.P_Empleado_ID))
                {
                    Mi_SQL.Append(" And " + Ope_Nom_Faltas_Empleado.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "' ");
                }

                if (!string.IsNullOrEmpty(Datos.P_Dependencia_ID))
                {
                    Mi_SQL.Append(" And " + Ope_Nom_Faltas_Empleado.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "' ");
                }

                Mi_SQL.Append(" ORDER BY " + Ope_Nom_Faltas_Empleado.Campo_Empleado_ID + "," + Ope_Nom_Faltas_Empleado.Campo_Fecha);
                Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                return Dt_Consulta;
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
        /// NOMBRE DE LA FUNCION: Consultar_Personal_Checa
        /// DESCRIPCION : Consulta todos los datos del Empleado 
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        :Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 13/Marzo/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Personal_Checa(Cls_Rpt_Nom_Retardos_Faltas_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para la consulta para del Empleado
            DataTable Dt_Consulta = new DataTable();
            try
            {
                Mi_SQL.Append("SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " , ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " , ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " , ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " , ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Reloj_Checador + " , ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + ", ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " as Nombre_Dependencia, ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Nombre + " as Nombre_Puesto,");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Salario_Mensual + ",");
                Mi_SQL.Append(Cat_Nom_Reloj_Checador.Tabla_Cat_Nom_Reloj_Checador + "." + Cat_Nom_Reloj_Checador.Campo_Clave + " as Clave_Reloj,");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Fecha_Inicio);
                Mi_SQL.Append(" From " + Cat_Empleados.Tabla_Cat_Empleados);

                Mi_SQL.Append(" left outer join " + Cat_Puestos.Tabla_Cat_Puestos + " on ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Puesto_ID);
                Mi_SQL.Append("=" + Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Puesto_ID);

                Mi_SQL.Append(" left outer join " + Cat_Dependencias.Tabla_Cat_Dependencias + " on ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID);
                Mi_SQL.Append("=" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);

                Mi_SQL.Append(" left outer join " + Cat_Nom_Reloj_Checador.Tabla_Cat_Nom_Reloj_Checador + " on ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Reloj_Checador);
                Mi_SQL.Append("=" + Cat_Nom_Reloj_Checador.Tabla_Cat_Nom_Reloj_Checador + "." + Cat_Nom_Reloj_Checador.Campo_Reloj_Checador_ID);

                Mi_SQL.Append(" Where ");

                if (!String.IsNullOrEmpty(Datos.P_No_Empleado))
                {
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "' ");
                }
                else
                {
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Reloj_Checador + "='" + Datos.P_Reloj_checador + "' ");
                }
                Mi_SQL.Append(" and " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Estatus + "='ACTIVO'");
                Mi_SQL.Append(" order by " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado +
                                ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Reloj_Checador);
                
                Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                return Dt_Consulta;
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
        /// NOMBRE DE LA FUNCION: Consultar_Informacion_Empleado
        /// DESCRIPCION : Consulta todos los datos del Empleado que selecciono el usuario
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        :Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 13/Marzo/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Informacion_Empleado(Cls_Rpt_Nom_Retardos_Faltas_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para la consulta para del Empleado
            DataTable Dt_Consulta = new DataTable();
            try
            {
                Mi_SQL.Append("Select " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno);
                Mi_SQL.Append(" || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno);
                Mi_SQL.Append(" || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " as Nombre_Empleado");
                Mi_SQL.Append(", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID);
                Mi_SQL.Append(", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Turno_ID);
                Mi_SQL.Append(", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);
                Mi_SQL.Append(", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado);
                Mi_SQL.Append(", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID);
                Mi_SQL.Append(", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Salario_Diario);
                Mi_SQL.Append(", " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre);
                Mi_SQL.Append(", " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave);
                Mi_SQL.Append(", " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina);
                Mi_SQL.Append(", " + Cat_Turnos.Tabla_Cat_Turnos + "." + Cat_Turnos.Campo_Descripcion);
                Mi_SQL.Append(", " + Cat_Turnos.Tabla_Cat_Turnos + "." + Cat_Turnos.Campo_Hora_Entrada);
                Mi_SQL.Append(", " + Cat_Turnos.Tabla_Cat_Turnos + "." + Cat_Turnos.Campo_Hora_Salida);
                Mi_SQL.Append(" From " + Cat_Empleados.Tabla_Cat_Empleados);
                Mi_SQL.Append(" INNER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " on " );
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID);
                Mi_SQL.Append("=" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);
                Mi_SQL.Append(" INNER JOIN " + Cat_Turnos.Tabla_Cat_Turnos);
                Mi_SQL.Append(" ON " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Turno_ID);
                Mi_SQL.Append("=" + Cat_Turnos.Tabla_Cat_Turnos + "." + Cat_Turnos.Campo_Turno_ID);
                Mi_SQL.Append(" INNER JOIN " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas);
                Mi_SQL.Append(" ON " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID);
                Mi_SQL.Append("=" + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);

                if (!string.IsNullOrEmpty(Datos.P_Empleado_ID))
                {
                    Mi_SQL.Append(" Where " + Cat_Empleados.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'");
                }

                if (!string.IsNullOrEmpty(Datos.P_No_Empleado))
                {
                    Mi_SQL.Append(" Where " + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "'");
                }
                
                Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                return Dt_Consulta;
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
        /// NOMBRE DE LA FUNCION: Consultar_Informacion_Asistencia
        /// DESCRIPCION : Consulta todos los datos del Empleado que selecciono el usuario
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        :Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 13/Marzo/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Informacion_Asistencia(Cls_Rpt_Nom_Retardos_Faltas_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para la consulta para del Empleado
            DataTable Dt_Consulta = new DataTable();
            try
            {
                Mi_SQL.Append("Select " + Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias + "." + Ope_Nom_Asistencias.Campo_Empleado_ID);
                Mi_SQL.Append("," + Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias + "." + Ope_Nom_Asistencias.Campo_Fecha_Hora_Entrada);
                Mi_SQL.Append("," + Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias + "." + Ope_Nom_Asistencias.Campo_Fecha_Hora_Salida);
                Mi_SQL.Append("," + Cat_Nom_Reloj_Checador.Tabla_Cat_Nom_Reloj_Checador + "." + Cat_Nom_Reloj_Checador.Campo_Clave);
                Mi_SQL.Append(" from " + Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias);
                Mi_SQL.Append(" inner join " + Cat_Nom_Reloj_Checador.Tabla_Cat_Nom_Reloj_Checador);
                Mi_SQL.Append(" on " + Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias + "." + Ope_Nom_Asistencias.Campo_Reloj_Checador_ID );
                Mi_SQL.Append("=" + Cat_Nom_Reloj_Checador.Tabla_Cat_Nom_Reloj_Checador + "." + Cat_Nom_Reloj_Checador.Campo_Reloj_Checador_ID);
                Mi_SQL.Append(" Where " + Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias + "." + Ope_Nom_Asistencias.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'");
                Mi_SQL.Append(" and TO_DATE(TO_CHAR(" + Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias + "." + Ope_Nom_Asistencias.Campo_Fecha_Hora_Entrada + ",'DD-MM-YY')) >= '" + Datos.P_Fecha_Inicial + "'");
                Mi_SQL.Append(" and TO_DATE(TO_CHAR(" + Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias + "." + Ope_Nom_Asistencias.Campo_Fecha_Hora_Entrada + ",'DD-MM-YY')) <= '" + Datos.P_Fecha_Final + "'");
                Mi_SQL.Append(" order by " + Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias + "." + Ope_Nom_Asistencias.Campo_Fecha_Hora_Entrada);
                Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                return Dt_Consulta;
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
        /// NOMBRE DE LA FUNCION: Consultar_Historico_Reloj
        /// DESCRIPCION : Consulta todos los datos del Empleado que selecciono el usuario
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        :Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 13/Marzo/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Historico_Reloj(Cls_Rpt_Nom_Retardos_Faltas_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para la consulta para del Empleado
            DataTable Dt_Consulta = new DataTable();
            try
            {
                Mi_SQL.Append("Select " + Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias + "." + Ope_Nom_Asistencias.Campo_Empleado_ID);
                Mi_SQL.Append("," + Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias + "." + Ope_Nom_Asistencias.Campo_Fecha_Hora_Entrada);
                Mi_SQL.Append("," + Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias + "." + Ope_Nom_Asistencias.Campo_Fecha_Hora_Salida);
                Mi_SQL.Append("," + Cat_Nom_Reloj_Checador.Tabla_Cat_Nom_Reloj_Checador + "." + Cat_Nom_Reloj_Checador.Campo_Clave);
                Mi_SQL.Append(" from " + Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias);
                Mi_SQL.Append(" inner join " + Cat_Nom_Reloj_Checador.Tabla_Cat_Nom_Reloj_Checador);
                Mi_SQL.Append(" on " + Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias + "." + Ope_Nom_Asistencias.Campo_Reloj_Checador_ID);
                Mi_SQL.Append("=" + Cat_Nom_Reloj_Checador.Tabla_Cat_Nom_Reloj_Checador + "." + Cat_Nom_Reloj_Checador.Campo_Reloj_Checador_ID);
                Mi_SQL.Append(" Where ");
                if(!String.IsNullOrEmpty(Datos.P_Empleado_ID))
                {
                    Mi_SQL.Append(Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias + "." + Ope_Nom_Asistencias.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "' and");
                }
                Mi_SQL.Append(" TO_DATE(TO_CHAR(" + Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias + "." + Ope_Nom_Asistencias.Campo_Fecha_Hora_Entrada + ",'DD-MM-YY')) >= '" + Datos.P_Fecha_Inicial + "'");
                Mi_SQL.Append(" and TO_DATE(TO_CHAR(" + Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias + "." + Ope_Nom_Asistencias.Campo_Fecha_Hora_Entrada + ",'DD-MM-YY')) <= '" + Datos.P_Fecha_Final + "'");

                Mi_SQL.Append(" or ");
                if (!String.IsNullOrEmpty(Datos.P_Empleado_ID))
                {
                    Mi_SQL.Append(Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias + "." + Ope_Nom_Asistencias.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "' and");
                }
                Mi_SQL.Append(" (TO_DATE(TO_CHAR(" + Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias + "." + Ope_Nom_Asistencias.Campo_Fecha_Hora_Salida + ",'DD-MM-YY')) >= '" + Datos.P_Fecha_Inicial + "'");
                Mi_SQL.Append(" and TO_DATE(TO_CHAR(" + Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias + "." + Ope_Nom_Asistencias.Campo_Fecha_Hora_Salida + ",'DD-MM-YY')) <= '" + Datos.P_Fecha_Final + "')");
                
                Mi_SQL.Append(" order by " + Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias + "." + Ope_Nom_Asistencias.Campo_Empleado_ID);
                Mi_SQL.Append(" , " + Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias + "." + Ope_Nom_Asistencias.Campo_Fecha_Hora_Entrada);
                Mi_SQL.Append(" , " + Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias + "." + Ope_Nom_Asistencias.Campo_Fecha_Hora_Salida);
                Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                return Dt_Consulta;
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
        /// NOMBRE DE LA FUNCION: Consultar_Informacion_Horario_Empleado
        /// DESCRIPCION : Consulta todos los datos del Empleado que selecciono el usuario
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        :Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 13/Marzo/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Informacion_Horario_Empleado(Cls_Rpt_Nom_Retardos_Faltas_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para la consulta para del Empleado
            DataTable Dt_Consulta = new DataTable();
            try
            {
                Mi_SQL.Append("Select * from " + Ope_Nom_Horarios_Empleados.Tabla_Ope_Nom_Horarios_Empleados);
                Mi_SQL.Append(" where " + Ope_Nom_Horarios_Empleados.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'");
                Mi_SQL.Append(" and (TO_DATE(TO_CHAR(" + Ope_Nom_Horarios_Empleados.Campo_Fecha_Inicio + ",'DD-MM-YY'))>='" + Datos.P_Fecha 
                            + "' or TO_DATE(TO_CHAR(");
                Mi_SQL.Append(Ope_Nom_Horarios_Empleados.Campo_Fecha_Termino + ",'DD-MM-YY'))<='" + Datos.P_Fecha + "')");
                Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                return Dt_Consulta;
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

    }
}