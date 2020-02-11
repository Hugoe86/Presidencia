using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Recibo_Pago;
using System.Data;
using Presidencia.Recibo_Pago.Negocio;
using System.Data.OracleClient;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Text;

namespace Presidencia.Recibo_Pago.Datos
{
    public class Cls_Ope_Nom_Recibo_Pago_Datos
    {
        #region (Operacion)
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Generar_Recibo
        ///DESCRIPCIÓN: se genera el recibo filtrado por los metodos 
        ///             de busqueda especificados en los datos 
        ///             del objeto de negocio
        ///PARAMETROS: Datos:   Objeto de la capa de negocios 
        ///                     que tiene los datos y filtros
        ///                     para realizar la consulta
        ///CREO: jtoledo
        ///FECHA_CREO: 21/Abril/2011 02:37:19 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        internal static DataTable Generar_Recibo(Cls_Ope_Nom_Recibo_Pago_Negocio Datos)
        {
            //Declaracion de Variables
            String Mi_SQL = String.Empty; //Variable para las consultas

            try
            {
                //Asignar consulta
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados +".";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados.Campo_No_Recibo + " AS Recibo_No, ";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados +".";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados +".";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados.Campo_Total_Nomina + " AS Neto_A_Pagar, ";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados +".";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados.Campo_Dias_Trabajados + " AS Dias_Trabajados, ";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados +".";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados.Campo_Puesto_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados +".";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados.Campo_Dependencia_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados +".";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados +".";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados.Campo_No_Nomina + ", ";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados +".";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados.Campo_Detalle_Nomina_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + ".";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_No_Empleado + " AS Empleado_No, ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados +".";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_RFC + " AS RFC, ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados +".";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Nombre + " || ' ' || " + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || " + Cat_Empleados.Campo_Apellido_Materno + " AS Nombre_Empleado, ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados +".";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Area_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados +".";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_CURP + " AS CURP, ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados +".";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_No_IMSS + " AS No_Afiliacion,";//Pendiente de definir
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados +".";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_SAP_Codigo_Programatico + " AS Codigo_Programatico,"; //Pendiente de definir
                Mi_SQL = Mi_SQL + Cat_Areas.Tabla_Cat_Areas +".";
                Mi_SQL = Mi_SQL + Cat_Areas.Campo_Nombre + " AS Categoria, ";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Tabla_Cat_Dependencias + ".";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Nombre + " AS Departamento, ";
                Mi_SQL = Mi_SQL + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + ".";
                Mi_SQL = Mi_SQL + Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio + " || ' - ' || " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin + " AS Periodo_Fechas, ";
                Mi_SQL = Mi_SQL + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + ".";
                Mi_SQL = Mi_SQL + Cat_Nom_Nominas_Detalles.Campo_No_Nomina + " AS Periodo ";

                Mi_SQL = Mi_SQL + " FROM ";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + ",";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + ",";
                Mi_SQL = Mi_SQL + Cat_Areas.Tabla_Cat_Areas + ",";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Tabla_Cat_Dependencias +", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles +" ";

                Mi_SQL = Mi_SQL + " WHERE ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados +".";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Empleado_ID +" = ";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados +".";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID +" AND ";

                Mi_SQL = Mi_SQL + Cat_Areas.Tabla_Cat_Areas +".";
                Mi_SQL = Mi_SQL + Cat_Areas.Campo_Area_ID + " = ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados +".";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Area_ID +" AND ";

                Mi_SQL = Mi_SQL + Cat_Dependencias.Tabla_Cat_Dependencias + ".";
                Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Dependencia_ID + " = ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + ".";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Dependencia_ID + " AND (";

                Mi_SQL = Mi_SQL + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + ".";
                Mi_SQL = Mi_SQL + Cat_Nom_Nominas_Detalles.Campo_Nomina_ID + " = ";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + ".";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + " AND ";

                Mi_SQL = Mi_SQL + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + ".";
                Mi_SQL = Mi_SQL + Cat_Nom_Nominas_Detalles.Campo_No_Nomina + " = ";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + ".";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados.Campo_No_Nomina + " ) ";

                if (Datos.P_Empleado_ID != "" && Datos.P_Empleado_ID != string.Empty && Datos.P_Empleado_ID != null)
                {
                    Mi_SQL = Mi_SQL + " AND ";
                    Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + ".";
                    Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + " IN (SELECT ";
                    Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + ".";
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE ";
                    Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + ".";
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_No_Empleado + " = ";
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Empleado_ID + "') ";

                    if (Datos.P_Periodo != "" && Datos.P_Periodo != string.Empty && Datos.P_Periodo != null)
                    {
                        Mi_SQL = Mi_SQL + " AND ";
                        Mi_SQL = Mi_SQL + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + ".";
                        Mi_SQL = Mi_SQL + Cat_Nom_Nominas_Detalles.Campo_No_Nomina + " = ";
                        Mi_SQL = Mi_SQL + Datos.P_Periodo + " ";
                    }

                    if (!String.IsNullOrEmpty(Datos.P_Nomina_ID))
                    {
                        Mi_SQL = Mi_SQL + " AND ";
                        Mi_SQL = Mi_SQL + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + ".";
                        Mi_SQL = Mi_SQL + Cat_Nom_Nominas_Detalles.Campo_Nomina_ID + " = '";
                        Mi_SQL = Mi_SQL + Datos.P_Nomina_ID + "' ";
                    }
                    if (!String.IsNullOrEmpty(Datos.P_Nomina_ID))
                    {
                        Mi_SQL = Mi_SQL + " AND ";
                        Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + ".";
                        Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Tipo_Nomina_ID + " = '";
                        Mi_SQL = Mi_SQL + Datos.P_Tipo_Nomina_ID + "'";
                    }
                }
                else
                {
                    if (Datos.P_Rfc != "" && Datos.P_Rfc != string.Empty && Datos.P_Rfc != null)
                    {
                        Mi_SQL = Mi_SQL + " AND ";
                        Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + ".";
                        Mi_SQL = Mi_SQL + Cat_Empleados.Campo_RFC + " = '";
                        Mi_SQL = Mi_SQL + Datos.P_Rfc + "'";
                    }
                    else if (Datos.P_Curp != "" && Datos.P_Curp != string.Empty && Datos.P_Curp != null)
                    {
                        Mi_SQL = Mi_SQL + " AND ";
                        Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + ".";
                        Mi_SQL = Mi_SQL + Cat_Empleados.Campo_CURP + " = '";
                        Mi_SQL = Mi_SQL + Datos.P_Curp + "'";
                    }
                    else if (Datos.P_Departamento != "" && Datos.P_Departamento != string.Empty && Datos.P_Departamento != null)
                    {
                        Mi_SQL = Mi_SQL + " AND ";
                        Mi_SQL = Mi_SQL + Cat_Dependencias.Tabla_Cat_Dependencias + ".";
                        Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Dependencia_ID + " = '";
                        Mi_SQL = Mi_SQL + Datos.P_Departamento + "'";
                    }
                    if (Datos.P_Periodo != "" && Datos.P_Periodo != string.Empty && Datos.P_Periodo != null )
                    {
                        Mi_SQL = Mi_SQL + " AND ";
                        Mi_SQL = Mi_SQL + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + ".";
                        Mi_SQL = Mi_SQL + Cat_Nom_Nominas_Detalles.Campo_No_Nomina + " = ";
                        Mi_SQL = Mi_SQL + Datos.P_Periodo +" ";                        
                    }
                    if (!String.IsNullOrEmpty(Datos.P_Nomina_ID))
                    {
                        Mi_SQL = Mi_SQL + " AND ";
                        Mi_SQL = Mi_SQL + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + ".";
                        Mi_SQL = Mi_SQL + Cat_Nom_Nominas_Detalles.Campo_Nomina_ID + " = '";
                        Mi_SQL = Mi_SQL + Datos.P_Nomina_ID + "' ";
                    }
                    if (!String.IsNullOrEmpty(Datos.P_Nomina_ID))
                    {
                        Mi_SQL = Mi_SQL + " AND ";
                        Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + ".";
                        Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Tipo_Nomina_ID + " = '";
                        Mi_SQL = Mi_SQL + Datos.P_Tipo_Nomina_ID + "'";
                    }
                }
                                
                Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados +"." + Ope_Nom_Recibos_Empleados.Campo_No_Recibo;

                //Entregar resultado
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }
        #endregion

        #region (Consulta)
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Percepciones_Recibo_Pago
        ///DESCRIPCIÓN: consulta las percepciones del empleado para el recibo a imprimir
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 04/23/2011 12:15:08 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        internal static DataTable Consulta_Percepciones_Recibo_Pago(Cls_Ope_Nom_Recibo_Pago_Negocio Datos)
        {
            //Declaracion de Variables
            String Mi_SQL = String.Empty; //Variable para las consultas

            try
            {
                Mi_SQL = "";
                Mi_SQL = Mi_SQL + "SELECT ";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + ".*, ";
                Mi_SQL = Mi_SQL + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + ".* ";
                Mi_SQL = Mi_SQL + " FROM ";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + " ";                
                Mi_SQL = Mi_SQL + "WHERE ";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + ".";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados_Det.Campo_No_Recibo + " = '";
                Mi_SQL = Mi_SQL + Datos.P_No_Recibo + "' ";
                Mi_SQL = Mi_SQL + " AND ";
                Mi_SQL = Mi_SQL + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + ".";
                Mi_SQL = Mi_SQL + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + " = ";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + ".";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados_Det.Campo_Percepcion_Deduccion_ID + " ";
                Mi_SQL = Mi_SQL + " AND ";
                Mi_SQL = Mi_SQL + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + ".";
                Mi_SQL = Mi_SQL + Cat_Nom_Percepcion_Deduccion.Campo_Tipo + " = 'PERCEPCION' ";

                //Entregar resultado
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Deducciones_Recibo_Pago
        ///DESCRIPCIÓN: consulta las deducciones del empleado para el recibo a imprimir
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 04/23/2011 12:15:08 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        internal static DataTable Consulta_Deducciones_Recibo_Pago(Cls_Ope_Nom_Recibo_Pago_Negocio Datos)
        {
            //Declaracion de Variables
            String Mi_SQL = String.Empty; //Variable para las consultas

            try
            {
                Mi_SQL = "";
                Mi_SQL = Mi_SQL + "SELECT ";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + ".*, ";
                Mi_SQL = Mi_SQL + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + ".* ";
                Mi_SQL = Mi_SQL + " FROM ";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + " ";
                Mi_SQL = Mi_SQL + "WHERE ";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + ".";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados_Det.Campo_No_Recibo + " = '";
                Mi_SQL = Mi_SQL + Datos.P_No_Recibo + "' ";
                Mi_SQL = Mi_SQL + " AND ";
                Mi_SQL = Mi_SQL + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + ".";
                Mi_SQL = Mi_SQL + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + " = ";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + ".";
                Mi_SQL = Mi_SQL + Ope_Nom_Recibos_Empleados_Det.Campo_Percepcion_Deduccion_ID + " ";
                Mi_SQL = Mi_SQL + " AND ";
                Mi_SQL = Mi_SQL + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + ".";           
                Mi_SQL = Mi_SQL + Cat_Nom_Percepcion_Deduccion.Campo_Tipo + " = 'DEDUCCION' ";

                //Entregar resultado
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }
        #endregion

        #region (Consulta Recibos)
        public static DataTable Consultar_Recibos_Empleados(Cls_Ope_Nom_Recibo_Pago_Negocio Datos)
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
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_CURP + ", ");
                Mi_SQL.Append("NVL(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_IMSS + ", ' ') AS NO_IMSS, ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Codigo_Programatico + ", ");

                Mi_SQL.Append("(SELECT " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre +
                    " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias +
                    " WHERE " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "=" +
                    Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + ") AS UNIDAD_RESPONSABLE, ");
                Mi_SQL.Append("(SELECT " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina +
                    " FROM " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas +
                    " WHERE " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + "=" +
                    Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID + ") AS NOMINA, ");

                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Recibo + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Dias_Trabajados + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Total_Nomina + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Total_Percepciones + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Total_Deducciones + ", ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio + ", ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin + ", ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Nombre + " AS PUESTO, " );
                Mi_SQL.Append(Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." + Cat_Nom_Bancos.Campo_Nombre + " AS BANCO");
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
                else 
                {
                    Mi_SQL.Append(" AND ");
                    Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Tipo_Nomina_ID + " NOT IN ('00003')");
                }

                if (!String.IsNullOrEmpty(Datos.P_Empleado_ID))
                {
                    Mi_SQL.Append(" AND ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_Empleado_ID + "'");
                }
                if (!String.IsNullOrEmpty(Datos.P_Rfc))
                {
                    Mi_SQL.Append(" AND ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_RFC + "='" + Datos.P_Rfc + "'");
                }
                if (!String.IsNullOrEmpty(Datos.P_Curp))
                {
                    Mi_SQL.Append(" AND ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_CURP + "='" + Datos.P_Curp + "'");
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

                Mi_SQL.Append(" ORDER BY " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + ", ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados  + "." + Cat_Empleados.Campo_Apellido_Paterno + " ASC");

                Dt_Recibos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los recibos de nómina de los empleados. Error: [" + Ex.Message + "]");
            }
            return Dt_Recibos;
        }

        public static DataTable Consultar_Percepciones_Recibo_Empleado(Cls_Ope_Nom_Recibo_Pago_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Percepiones_Recibo = null;

            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Clave + ", ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Nombre + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_Monto);

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

                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Tipo + "='PERCEPCION'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_No_Recibo + "='" + Datos.P_No_Recibo + "'");
                Mi_SQL.Append(" ORDER BY " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Clave + " ASC");

                Dt_Percepiones_Recibo = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las percepciones del recibo del empleado. Error: [" + Ex.Message + "]");
            }
            return Dt_Percepiones_Recibo;
        }

        public static DataTable Consultar_Deducciones_Recibo_Empleado(Cls_Ope_Nom_Recibo_Pago_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Deducciones_Recibo = null;

            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Clave + ", ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Nombre + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_Monto);

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

                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Tipo + "='DEDUCCION'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_No_Recibo + "='" + Datos.P_No_Recibo + "'");
                Mi_SQL.Append(" ORDER BY " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Clave + " ASC");

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