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
using Presidencia.Reporte_Credito_Fonacot.Negocio;

namespace Presidencia.Reporte_Credito_Fonacot.Datos
{
    public class Cls_Rpt_Nom_Reporte_Credito_Fonacot_Datos
    {
        public Cls_Rpt_Nom_Reporte_Credito_Fonacot_Datos()
        {
        }
        #region(Métodos)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_CreditoS_Fonacot_Empleado
        /// DESCRIPCION: se obtienen los creditos  fonacot por empleado
        /// PARAMETROS : Datos: Valores que son pasados desde la capa de negocios
        /// CREO       : Sergio Manuel Gallardo Andrade
        /// FECHA_CREO : 10-Abril-2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_CreditoS_Fonacot_Empleado(Cls_Rpt_Nom_Reporte_Credito_Fonacot_Negocio Datos)
                {
                    StringBuilder Mi_SQL= new StringBuilder(); //Variable que tendra la consulta a realizar a la base de datos
                    try
                    {
                        //Consulta el balance del mes y año que se proporciono por parte del usuario filtrando por rango de cuentas o mostrando todas
                        Mi_SQL.Append("SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ");
                        Mi_SQL.Append("("+Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + "||' '||");
                        Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "||' '||");
                        Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ") as Nombre_Empleado, ");
                        Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_No_Fonacot + " as Folio_Fonacot, ");
                        Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_No_Credito + ", ");
                        Mi_SQL.Append(Ope_Nom_Proveedores.Tabla_Ope_Nom_Proveedores + "." + Ope_Nom_Proveedores.Campo_Fecha_Autorizacion + " As Fecha_Autoriza, ");
                        Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_No_Movimiento + " as otro, ");
                        Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_Plazo + ", ");
                        Mi_SQL.Append(Ope_Nom_Proveedores.Tabla_Ope_Nom_Proveedores + "." + Ope_Nom_Proveedores.Campo_No_Periodos + " As Pagos, ");
                        Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_Cantidad + " As Abono,(");
                        Mi_SQL.Append(Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Anio + "||'-'||"+Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles +"."+Ope_Nom_Proveedores_Detalles.Campo_Periodo+") As Periodo , ");
                        Mi_SQL.Append(Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Fecha_Fin + " as Fecha_Pago , (SELECT " );
                        Mi_SQL.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + "||' '||");
                        Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "||' '||");
                        Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ") as Elaboro From ");
                        Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_Empleado_ID+"='" +Datos.P_Usuario_Creo+ "') as Nombre_Elaboro");
                        Mi_SQL.Append(" FROM " + Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + ", " + Ope_Nom_Proveedores.Tabla_Ope_Nom_Proveedores +", "+ Cat_Empleados.Tabla_Cat_Empleados+" , "+ Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas );
                        Mi_SQL.Append(" WHERE " + Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_No_Movimiento );
                        Mi_SQL.Append(" = " + Ope_Nom_Proveedores.Tabla_Ope_Nom_Proveedores  + "." + Ope_Nom_Proveedores.Campo_No_Movimiento +" AND "+ Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_Empleado_ID );
                        Mi_SQL.Append(" = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID +" AND "+Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_Nomina_ID);
                        Mi_SQL.Append(" = " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Nomina_ID);
                        Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "=" + Datos.P_Empleado_ID);
                        //Si se esta pidiendo filtrar entre rango de cuentas contables
                        if (!String.IsNullOrEmpty(Datos.P_Folio_Fonacot))
                        {
                            Mi_SQL.Append(" AND " + Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_No_Fonacot + "=" + Datos.P_Folio_Fonacot);
                        }
                        if (!String.IsNullOrEmpty(Datos.P_No_Credito))
                        {
                            Mi_SQL.Append(" AND " + Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_No_Credito + "=" + Datos.P_No_Credito);
                        }
                        Mi_SQL.Append(" ORDER BY " + Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_No_Credito);
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
        /// NOMBRE DE LA FUNCION: Consulta_Credito_Fonacot
        /// DESCRIPCION: Consulta los datos del periodo de los creditos fonacot para poder tener los datos
        /// PARAMETROS : Datos: Valores que son pasados desde la capa de negocios
        /// CREO       : sergio Manuel Gallardo Andrade
        /// FECHA_CREO : 10-Abril-2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Credito_Fonacot(Cls_Rpt_Nom_Reporte_Credito_Fonacot_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable que tendra la consulta a realizar a la base de datos
            try
            {
                //Consulta el balance del mes y año que se proporciono por parte del usuario filtrando por rango de cuentas o mostrando todas
                Mi_SQL.Append("SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ");
                Mi_SQL.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "||' '||");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "||' '||");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") as Nombre_Empleado, ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Codigo_Programatico + " as Codigo_Programatico, ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Clave  + " as concepto, ");
                Mi_SQL.Append(Cat_Nom_Calendario_Reloj.Tabla_Cat_Nom_Calendario_Reloj + "." + Cat_Nom_Calendario_Reloj.Campo_Fecha_Inicio  + " as Fecha_inicial, ");
                Mi_SQL.Append(Cat_Nom_Calendario_Reloj.Tabla_Cat_Nom_Calendario_Reloj  + "." + Cat_Nom_Calendario_Reloj.Campo_Fecha_Fin + " as Fecha_Final, ");
                Mi_SQL.Append(Cat_Grupos_Dependencias.Tabla_Cat_Grupos_Dependencias + "." + Cat_Grupos_Dependencias.Campo_Registro_Patronal  + " as Registro_Patronal, ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_No_Fonacot + " as Folio_Fonacot, ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_No_Credito + ", ");
                Mi_SQL.Append(Ope_Nom_Proveedores.Tabla_Ope_Nom_Proveedores + "." + Ope_Nom_Proveedores.Campo_Fecha_Autorizacion + " As Fecha_Autoriza, ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_No_Movimiento + " as otro, ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_Plazo + ", ");
                Mi_SQL.Append(Ope_Nom_Proveedores.Tabla_Ope_Nom_Proveedores + "." + Ope_Nom_Proveedores.Campo_No_Periodos + " As Pagos, ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_Cantidad + " As Abono,(");
                Mi_SQL.Append(Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Anio + "||'-'||" + Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_Periodo + ") As Periodo , ");
                Mi_SQL.Append(Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Fecha_Fin + " as Fecha_Pago , (SELECT ");
                Mi_SQL.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + "||' '||");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "||' '||");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ") as Elaboro From ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_Empleado_ID + "='" + Datos.P_Usuario_Creo + "') as Nombre_Elaboro FROM ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles );
                Mi_SQL.Append(" LEFT OUTER JOIN " + Ope_Nom_Proveedores.Tabla_Ope_Nom_Proveedores + " ON " + Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_No_Movimiento);
                Mi_SQL.Append(" = " + Ope_Nom_Proveedores.Tabla_Ope_Nom_Proveedores + "." + Ope_Nom_Proveedores.Campo_No_Movimiento);
                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Nom_Calendario_Reloj.Tabla_Cat_Nom_Calendario_Reloj  + " ON " + Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_Periodo);
                Mi_SQL.Append(" = " + Cat_Nom_Calendario_Reloj.Tabla_Cat_Nom_Calendario_Reloj + "." + Cat_Nom_Calendario_Reloj.Campo_No_Nomina + " AND " + Ope_Nom_Proveedores.Tabla_Ope_Nom_Proveedores + "." + Ope_Nom_Proveedores.Campo_Nomina_ID + "=" + Cat_Nom_Calendario_Reloj.Tabla_Cat_Nom_Calendario_Reloj + "." + Cat_Nom_Calendario_Reloj.Campo_Nomina_ID );
                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " ON " + Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles  + "." + Ope_Nom_Proveedores_Detalles.Campo_Empleado_ID);
                Mi_SQL.Append(" = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID );
                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + " ON " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Fuente_Financiamiento_ID);
                Mi_SQL.Append(" = " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID);
                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_SAP_Det_Fte_Dependencia.Tabla_Cat_SAP_Det_Fte_Financiamiento_Dependencia + " ON " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID);
                Mi_SQL.Append(" = " + Cat_SAP_Det_Fte_Dependencia.Tabla_Cat_SAP_Det_Fte_Financiamiento_Dependencia + "." + Cat_SAP_Det_Fte_Dependencia.Campo_Fuente_Financiamiento_ID);
                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " ON " + Cat_SAP_Det_Fte_Dependencia.Tabla_Cat_SAP_Det_Fte_Financiamiento_Dependencia + "." + Cat_SAP_Det_Fte_Dependencia.Campo_Dependencia_ID);
                Mi_SQL.Append(" = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);
                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Grupos_Dependencias.Tabla_Cat_Grupos_Dependencias + " ON " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Grupo_Dependencia_ID);
                Mi_SQL.Append(" = " + Cat_Grupos_Dependencias.Tabla_Cat_Grupos_Dependencias + "." + Cat_Grupos_Dependencias.Campo_Grupo_Dependencia_ID);
                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + " ON " + Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_Nomina_ID);
                Mi_SQL.Append(" = " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Nomina_ID);
                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion  + " ON " + Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_Percepcion_Deduccion_ID);
                Mi_SQL.Append(" = " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID);
                if (!String.IsNullOrEmpty(Datos.P_Periodo) && !String.IsNullOrEmpty(Datos.P_Nomina))
                {
                    Mi_SQL.Append(" WHERE " + Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_Periodo + "='" + Datos.P_Periodo + "'");
                    Mi_SQL.Append(" AND " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Anio + "='" + Datos.P_Nomina + "'");
                }
                if (!String.IsNullOrEmpty(Datos.P_Empleado_ID))
                {
                    Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "=" + Datos.P_Empleado_ID);
                }
                //Si se esta pidiendo filtrar entre rango de cuentas contables
                if (!String.IsNullOrEmpty(Datos.P_Folio_Fonacot))
                {
                    Mi_SQL.Append(" AND " + Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_No_Fonacot + "=" + Datos.P_Folio_Fonacot);
                }
                if (!String.IsNullOrEmpty(Datos.P_No_Credito))
                {
                    Mi_SQL.Append(" AND " + Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_No_Credito + "=" + Datos.P_No_Credito);
                }

                Mi_SQL.Append(" ORDER BY " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno);
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
    }
}