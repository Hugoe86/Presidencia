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
using Presidencia.Operacion_Predial_Constancias.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using System.Text;
using Presidencia.Sessiones;
using Presidencia.Operacion_Pre_Caj_Detalles.Negocio;

/// <summary>
/// Summary description for Cls_Ope_Pre_Caj_Detalles_Datos
/// </summary>
/// 
namespace Presidencia.Operacion_Pre_Caj_Detalles.Datos
{
    public class Cls_Ope_Pre_Caj_Detalles_Datos
    {
        public Cls_Ope_Pre_Caj_Detalles_Datos()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consultar_Pagos
        ///DESCRIPCIÓN:consulta a detalle  los pagos que se realizaron en la caja dependiendo del dia y la caja que desea consultar
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 07/27/2011 07:25:55 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************* 
        internal static DataTable Consultar_Pagos(Cls_Ope_Pre_Caj_Detalles_Negocio Datos)
        {
            String Mi_SQL;
            DateTime fecha = Convert.ToDateTime(Datos.P_Fecha);
            int cont = 0;
            try
            {
                Mi_SQL = "SELECT Ca." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " AS CAJA, ";
                Mi_SQL = Mi_SQL + "Cuenta." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " AS PREDIO, ";
                Mi_SQL = Mi_SQL + "Pa." + Ope_Caj_Pagos.Campo_Fecha + ", ";
                //Mi_SQL = Mi_SQL + "CASE WHEN PA." + Ope_Caj_Pagos.Campo_Estatus + " = 'CANCELADO' THEN TO_CHAR(Pa." + Ope_Caj_Pagos.Campo_No_Operacion + ") || ' (' || (SELECT TO_CHAR(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Operacion + ") FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " WHERE " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Recibo + " = Pa." + Ope_Caj_Pagos.Campo_No_Recibo + " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " = Pa." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID + " = Pa." + Ope_Caj_Pagos.Campo_Caja_ID + " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Estatus + " = 'PAGADO') || ')' ELSE TO_CHAR(PA." + Ope_Caj_Pagos.Campo_No_Operacion + ") END AS NO_OPERACION, ";
                //Mi_SQL = Mi_SQL + "TO_CHAR(Pa." + Ope_Caj_Pagos.Campo_No_Operacion + ") as No_Operacion, ";
                Mi_SQL = Mi_SQL + "Pa." + Ope_Caj_Pagos.Campo_No_Operacion + " as NO_OPERACION_PAGO, ";
                Mi_SQL = Mi_SQL + "CASE WHEN PA." + Ope_Caj_Pagos.Campo_Estatus + " = 'CANCELADO' THEN (SELECT " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Operacion + " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " WHERE " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Recibo + " = Pa." + Ope_Caj_Pagos.Campo_No_Recibo + " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " = Pa." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID + " = Pa." + Ope_Caj_Pagos.Campo_Caja_ID + " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Estatus + " = 'PAGADO') ELSE NULL END AS NO_OPERACION_CANCELACION, ";
                Mi_SQL = Mi_SQL + "Pa." + Ope_Caj_Pagos.Campo_No_Recibo + ", ";
                Mi_SQL = Mi_SQL + "Pa." + Ope_Caj_Pagos.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + "Clave." + Cat_Pre_Claves_Ingreso.Campo_Clave + " AS CVE_Ingreso, ";
                Mi_SQL = Mi_SQL + "Clave." + Cat_Pre_Claves_Ingreso.Campo_Descripcion + " AS Concepto, ";
                Mi_SQL = Mi_SQL + "Pasivo." + Ope_Ing_Pasivo.Campo_Monto + " AS Importe ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " Pa";
                Mi_SQL = Mi_SQL + " INNER JOIN " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " Ca ON Pa." + Ope_Caj_Pagos.Campo_Caja_ID + " = Ca." + Cat_Pre_Cajas.Campo_Caja_ID;
                Mi_SQL = Mi_SQL + " INNER JOIN " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " Pasivo ON Pasivo." + Ope_Ing_Pasivo.Campo_No_Pago + " = Pa." + Ope_Caj_Pagos.Campo_No_Pago;
                Mi_SQL = Mi_SQL + " INNER JOIN " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + " Clave ON Pasivo." + Ope_Ing_Pasivo.Campo_Clave_Ingreso_ID + " = Clave." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID;
                Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " Cuenta ON Pa." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + " = Cuenta." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                if (Datos.P_Caja_Id != "")
                {
                    Mi_SQL = Mi_SQL + " WHERE Pa." + Ope_Caj_Pagos.Campo_Caja_ID + "='" + Datos.P_Caja_Id + "'";
                    if (Datos.P_Estatus == "CANCELADOS")
                    {
                        Mi_SQL = Mi_SQL + " AND Pa." + Ope_Caj_Pagos.Campo_Estatus + " = 'CANCELADO' ";
                        cont = 1;
                    }
                    else
                    {
                        Mi_SQL = Mi_SQL + " AND Pa." + Ope_Caj_Pagos.Campo_Estatus + " <>'INUTILIZADO' ";
                        cont = 1;
                    }
                }
                else
                {
                    if (Datos.P_Estatus == "CANCELADOS")
                    {
                        Mi_SQL = Mi_SQL + " WHERE Pa." + Ope_Caj_Pagos.Campo_Estatus + " = 'CANCELADO' ";
                        cont = 1;
                    }
                    else
                    {
                        Mi_SQL = Mi_SQL + " WHERE Pa." + Ope_Caj_Pagos.Campo_Estatus + " <>'INUTILIZADO' ";
                        cont = 1;
                    }
                }
                Mi_SQL = Mi_SQL + " AND Pa.Fecha BETWEEN TO_DATE('" + String.Format("{0:dd/MM/yyyy}", fecha) + " 00:00:00','DD/MM/YYYY HH24:MI:SS') and TO_DATE('" + String.Format("{0:dd/MM/yyyy}", fecha) + " 23:59:59','DD/MM/YYYY HH24:MI:SS')";
                Mi_SQL = Mi_SQL + " ORDER BY Clave." + Cat_Pre_Claves_Ingreso.Campo_Descripcion + " DESC";
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Caja
        ///DESCRIPCIÓN:consulta a detalle  los pagos que se realizaron en la caja dependiendo del dia y la caja que desea consultar
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 16/octubre/2011 07:25:55 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************* 
        internal static DataSet Consultar_Caja(Cls_Ope_Pre_Caj_Detalles_Negocio Datos)
        {
            String Mi_SQL;
            try
            {
                Mi_SQL = "SELECT CAJA." + Cat_Pre_Cajas.Campo_Numero_De_Caja + ", CAJA." + Cat_Pre_Cajas.Campo_Caja_ID + ",CAJA." + Cat_Pre_Cajas.Campo_Foranea + ",Turno." + Ope_Caj_Turnos.Campo_Aplicacion_Pago + " FROM " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + " TURNO, ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " CAJA WHERE CAJA." + Cat_Pre_Cajas.Campo_Caja_Id + " = TURNO." + Ope_Caj_Turnos.Campo_Caja_Id;
                Mi_SQL = Mi_SQL + " AND CAJA." + Cat_Pre_Cajas.Campo_Estatus + "='VIGENTE' AND TURNO." + Ope_Caj_Turnos.Campo_Estatus + "='ABIERTO' AND ";
                Mi_SQL = Mi_SQL + " TURNO." + Ope_Caj_Turnos.Campo_Empleado_ID + "= '" + Datos.P_Empleado_ID + "'";
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Pagos_General
        ///DESCRIPCIÓN: consulta los pagos que se realizaron en la caja dependiendo el periodo que desea consultar
        ///PARAMETROS: 
        ///CREO: Sergio Manuel Gallardo Andrade
        ///FECHA_CREO: 07/27/2011 07:25:55 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************* 
        internal static DataTable Consultar_Pagos_General(Cls_Ope_Pre_Caj_Detalles_Negocio Datos)
        {
            String Mi_SQL;
            DateTime Fecha = Convert.ToDateTime(Datos.P_Fecha);
            DateTime Fecha_Final = Convert.ToDateTime(Datos.P_Fecha_Final);
            try
            {
                Mi_SQL = "SELECT Pa." + Ope_Caj_Pagos.Campo_No_Recibo + ", Pa." + Ope_Caj_Pagos.Campo_No_Operacion + ", Ra." + Cat_Pre_Ramas.Campo_Nombre + ", Ra." + Cat_Pre_Ramas.Campo_Clave + " AS CLAVE_RAMA,";
                if (Datos.P_Caja_Id != "")
                {
                    Mi_SQL += " Ca." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " AS CAJA, Pa.";
                }
                else
                {
                    Mi_SQL += " 'GLOBAL' AS CAJA, Pa.";
                }
                Mi_SQL = Mi_SQL + Ope_Caj_Pagos.Campo_Fecha + ", Pa.";
                Mi_SQL = Mi_SQL + Ope_Caj_Pagos.Campo_Estatus + ",Clave." + Cat_Pre_Claves_Ingreso.Campo_Clave + " as CLAVE_INGRESO, Clave.";
                Mi_SQL = Mi_SQL + Cat_Pre_Claves_Ingreso.Campo_Descripcion + " as Concepto,SUM(Pasivo." + Ope_Ing_Pasivo.Campo_Monto + ") as Importe, '0' as FECHA_FINAL,'0' as TOTAL_RECIBOS, COUNT(CLAVE." + Cat_Pre_Claves_Ingreso.Campo_Clave + ") AS UNIDAD FROM ";
                Mi_SQL = Mi_SQL + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " Pa, " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " Ca, " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + " Clave,";
                Mi_SQL = Mi_SQL + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " Pasivo," + Cat_Pre_Ramas.Tabla_Cat_Pre_Ramas + " Ra," + Cat_Pre_Grupos.Tabla_Cat_Pre_Grupos + " Gru  WHERE Pa." + Ope_Caj_Pagos.Campo_No_Pago + "=Pasivo.";
                Mi_SQL = Mi_SQL + Ope_Ing_Pasivo.Campo_No_Pago + " AND Pa." + Ope_Caj_Pagos.Campo_Caja_ID + "=Ca." + Cat_Pre_Cajas.Campo_Caja_Id + " AND Pasivo." + Ope_Ing_Pasivo.Campo_Clave_Ingreso_ID + " = Clave." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID + " AND  Clave." + Cat_Pre_Claves_Ingreso.Campo_Grupo_ID + " = Gru." + Cat_Pre_Grupos.Campo_Grupo_ID + " AND Gru.";
                Mi_SQL = Mi_SQL + Cat_Pre_Grupos.Campo_Rama_ID + " = Ra." + Cat_Pre_Ramas.Campo_Rama_ID + " AND Pa." + Ope_Caj_Pagos.Campo_Estatus + " <> 'INUTILIZADO'";
                if (Datos.P_Modulo_Id != "")
                {
                    Mi_SQL = Mi_SQL + " AND Ca." + Cat_Pre_Cajas.Campo_Modulo_Id + "='" + Datos.P_Modulo_Id + "'";
                }
                if (Datos.P_Caja_Id != "")
                {
                    Mi_SQL = Mi_SQL + " AND Pa." + Ope_Caj_Pagos.Campo_Caja_ID + "='" + Datos.P_Caja_Id + "'";
                }
                Mi_SQL = Mi_SQL + " AND Pa.Fecha BETWEEN TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Fecha) + " 00:00:00','DD/MM/YYYY HH24:MI:SS') AND TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Fecha_Final) + " 23:59:59','DD/MM/YYYY HH24:MI:SS')";
                if (Datos.P_Caja_Id != "")
                {
                    Mi_SQL = Mi_SQL + " GROUP BY Clave." + Cat_Pre_Claves_Ingreso.Campo_Clave + ",Ra." + Cat_Pre_Ramas.Campo_Nombre + ", Ra." + Cat_Pre_Ramas.Campo_Clave + ", Ca." + Cat_Pre_Cajas.Campo_Numero_De_Caja + ", Pa." + Ope_Caj_Pagos.Campo_Estatus + ", Pa." + Ope_Caj_Pagos.Campo_Fecha + ",Clave." + Cat_Pre_Claves_Ingreso.Campo_Descripcion + ", Pa." + Ope_Caj_Pagos.Campo_No_Recibo + ", Pa." + Ope_Caj_Pagos.Campo_No_Operacion;
                }
                else
                {
                    Mi_SQL = Mi_SQL + " GROUP BY Clave." + Cat_Pre_Claves_Ingreso.Campo_Clave + ",Ra." + Cat_Pre_Ramas.Campo_Nombre + ", Ra." + Cat_Pre_Ramas.Campo_Clave + ", Pa." + Ope_Caj_Pagos.Campo_Estatus + ", Pa." + Ope_Caj_Pagos.Campo_Fecha + ",Clave." + Cat_Pre_Claves_Ingreso.Campo_Descripcion + ", Pa." + Ope_Caj_Pagos.Campo_No_Recibo + ", Pa." + Ope_Caj_Pagos.Campo_No_Operacion;
                }
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

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
        ///NOMBRE DE LA FUNCIÓN: Reporte_Desglosado_Tarjeta_Bancaria
        ///DESCRIPCIÓN: Consulta los pagos por fecha, cantidad e importe con tarjeta bancaria
        ///PARAMETROS: 
        ///CREO: Ismael Prieto Sánchez
        ///FECHA_CREO: 19/Noviembre/2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************* 
        internal static DataTable Reporte_Desglosado_Tarjeta_Bancaria(Cls_Ope_Pre_Caj_Detalles_Negocio Datos)
        {
            //StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            String Mi_SQL;
            DateTime Fecha = Convert.ToDateTime(Datos.P_Fecha);
            DateTime Fecha_Final = Convert.ToDateTime(Datos.P_Fecha_Final);
            try
            {
                Mi_SQL = "SELECT " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." + Cat_Nom_Bancos.Campo_Nombre + " AS BANCO, " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha + ", " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Plan_Pago + " AS PLAN,";
                Mi_SQL += " TO_CHAR(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha + ", 'MM') AS MES,";
                Mi_SQL += " NVL(COUNT(" + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_No_Pago + "),'0') AS CANTIDAD,";
                Mi_SQL += " NVL(SUM(" + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Monto + "),'0') AS IMPORTE";
                Mi_SQL += " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + ", " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + ", " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + ", " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + ", " + Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas;
                Mi_SQL += " WHERE " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago + " = " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_No_Pago;
                Mi_SQL += " AND " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Banco_ID + " = " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." + Cat_Nom_Bancos.Campo_Banco_ID;
                Mi_SQL += " AND " + Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas + "." + Cat_Pre_Cajas.Campo_Caja_Id + " = " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID;
                Mi_SQL += " AND " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Modulo_Id + " = " + Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas + "." + Cat_Pre_Cajas.Campo_Modulo_Id;
                Mi_SQL += " AND " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + " = 'BANCO'";
                if (Datos.P_Modulo_Id != "")
                {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Modulo_Id + " = '" + Datos.P_Modulo_Id + "'";
                }
                if (Datos.P_Caja_Id != "")
                {
                    Mi_SQL = Mi_SQL + " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID + " = '" + Datos.P_Caja_Id + "'";
                }
                Mi_SQL += " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha + " BETWEEN TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Fecha) + " 00:00:00','DD/MM/YYYY HH24:MI:SS') AND TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Fecha_Final) + " 23:59:59','DD/MM/YYYY HH24:MI:SS')";
                Mi_SQL += " GROUP BY " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." + Cat_Nom_Bancos.Campo_Nombre + ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha + ", " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Plan_Pago;
                Mi_SQL += " ORDER BY " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha + ", " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." + Cat_Nom_Bancos.Campo_Nombre + ", " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Plan_Pago + " DESC";
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

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
        ///NOMBRE DE LA FUNCIÓN: Reporte_Concentracion_Monetarea
        ///DESCRIPCIÓN:consulta los pagos entre fechas indicadas y en base a una caja.
        ///PARAMETROS: 
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 20/Noviembre/2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************* 
        internal static DataTable Reporte_Concentracion_Monetarea(Cls_Ope_Pre_Caj_Detalles_Negocio Datos)
        {
            String Mi_SQL;
            try
            {
                Mi_SQL = "SELECT PAGOS." + Ope_Caj_Pagos.Campo_Fecha + " AS FECHA, ";
                Mi_SQL += " TO_CHAR(PAGOS." + Ope_Caj_Pagos.Campo_Fecha + ",'DD') || TO_CHAR(PAGOS." + Ope_Caj_Pagos.Campo_Fecha + ",'MM') || TO_CHAR(PAGOS." + Ope_Caj_Pagos.Campo_Fecha + ",'YYYY') AS DIA, ";
                Mi_SQL += " CAJAS." + Cat_Pre_Cajas.Campo_Numero_De_Caja + "|| ' (' ||MODULOS." + Cat_Pre_Modulos.Campo_Ubicacion + "||')' AS CAJA, ";
                Mi_SQL += "SUM(PAGOS." + Ope_Caj_Pagos.Campo_Total + ") AS IMPORTE ";
                Mi_SQL += " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " PAGOS INNER JOIN ";
                Mi_SQL += Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " CAJAS ON PAGOS." + Ope_Caj_Pagos.Campo_Caja_ID + "=CAJAS." + Cat_Pre_Cajas.Campo_Caja_Id;
                Mi_SQL += " INNER JOIN " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + " MODULOS";
                Mi_SQL += " ON CAJAS." + Cat_Pre_Cajas.Campo_Modulo_Id + "=MODULOS." + Cat_Pre_Modulos.Campo_Modulo_Id;
                Mi_SQL += " WHERE PAGOS." + Ope_Caj_Pagos.Campo_Fecha + " BETWEEN TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha)) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS') AND ";
                Mi_SQL += "TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Final)) + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS')";
                Mi_SQL += " AND PAGOS." + Cat_Pre_Cajas.Campo_Estatus + " = 'PAGADO'";
                if (Datos.P_Modulo_Id != "")
                {
                    Mi_SQL += " AND MODULOS." + Cat_Pre_Cajas.Campo_Modulo_Id + " = '" + Datos.P_Modulo_Id + "'";
                }
                if (Datos.P_Caja_Id != "")
                {
                    Mi_SQL += " AND PAGOS." + Cat_Pre_Cajas.Campo_Caja_Id + " = '" + Datos.P_Caja_Id + "'";
                }
                Mi_SQL += " GROUP BY PAGOS." + Ope_Caj_Pagos.Campo_Fecha + ", ";
                Mi_SQL += " TO_CHAR(PAGOS." + Ope_Caj_Pagos.Campo_Fecha + ",'DD') || TO_CHAR(PAGOS." + Ope_Caj_Pagos.Campo_Fecha + ",'MM') || TO_CHAR(PAGOS." + Ope_Caj_Pagos.Campo_Fecha + ",'YYYY'), ";
                Mi_SQL += " CAJAS." + Cat_Pre_Cajas.Campo_Numero_De_Caja + ", MODULOS." + Cat_Pre_Modulos.Campo_Ubicacion;
                Mi_SQL += " ORDER BY PAGOS." + Ope_Caj_Pagos.Campo_Fecha + ", TO_CHAR(PAGOS." + Ope_Caj_Pagos.Campo_Fecha + ",'DD') || TO_CHAR(PAGOS." + Ope_Caj_Pagos.Campo_Fecha + ",'MM') || TO_CHAR(PAGOS." + Ope_Caj_Pagos.Campo_Fecha + ",'YYYY'), CAJAS." + Cat_Pre_Cajas.Campo_Numero_De_Caja + ", MODULOS." + Cat_Pre_Modulos.Campo_Ubicacion;
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

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
        ///NOMBRE DE LA FUNCIÓN: Reporte_Pagos_tarjetas
        ///DESCRIPCIÓN:consulta los pagos entre fechas indicadas que sean pagadas con tarjetas de credito.
        ///PARAMETROS: 
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 20/Noviembre/2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************* 
        internal static DataTable Reporte_Pagos_tarjetas(Cls_Ope_Pre_Caj_Detalles_Negocio Datos)
        {
            String Mi_SQL;
            try
            {
                Mi_SQL = "SELECT PAGOS." + Ope_Caj_Pagos.Campo_Fecha + " AS FECHA, ";
                Mi_SQL += "TO_CHAR(PAGOS." + Ope_Caj_Pagos.Campo_Fecha + ",'MM') || TO_CHAR(PAGOS." + Ope_Caj_Pagos.Campo_Fecha + ",'YYYY') AS MES, ";
                Mi_SQL += "COUNT(PAGOS." + Ope_Caj_Pagos.Campo_Fecha + ") AS CANTIDAD, ";
                Mi_SQL += "SUM(PAG_DET." + Ope_Caj_Pagos_Detalles.Campo_Monto + ") AS IMPORTE";
                //Mi_SQL += " (SELECT MODULO." + Cat_Pre_Modulos.Campo_Ubicacion + " FROM " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + " MODULO WHERE MODULO." + Cat_Pre_Modulos.Campo_Modulo_Id + " IN (SELECT CAJA." + Cat_Pre_Cajas.Campo_Modulo_Id + " FROM " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " CAJA WHERE CAJA." + Cat_Pre_Cajas.Campo_Caja_Id + "='" + Datos.P_Caja_Id + "')) AS UB_MODULO";
                Mi_SQL += " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " PAGOS INNER JOIN ";
                Mi_SQL += Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + " PAG_DET ";
                Mi_SQL += " ON PAGOS." + Ope_Caj_Pagos.Campo_No_Pago + "=PAG_DET." + Ope_Caj_Pagos_Detalles.Campo_No_Pago + " ";
                Mi_SQL += " INNER JOIN " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " CAJA ON PAGOS." + Ope_Caj_Pagos.Campo_Caja_ID + " = CAJA." + Cat_Pre_Cajas.Campo_Caja_Id;
                Mi_SQL += " INNER JOIN " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + " MODULO ON CAJA." + Cat_Pre_Cajas.Campo_Modulo_Id + " = MODULO." + Cat_Pre_Modulos.Campo_Modulo_Id;
                Mi_SQL += " WHERE PAGOS." + Ope_Caj_Pagos.Campo_Fecha + " BETWEEN TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha)) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS') AND ";
                Mi_SQL += "TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Final)) + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS') AND ";
                Mi_SQL += " PAG_DET." + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + "='BANCO'";

                if (Datos.P_Modulo_Id != "")
                {
                    Mi_SQL += " AND MODULO." + Cat_Pre_Modulos.Campo_Modulo_Id + " = '" + Datos.P_Modulo_Id + "'";
                }
                if (Datos.P_Caja_Id != "")
                {
                    Mi_SQL += " AND CAJA." + Cat_Pre_Cajas.Campo_Caja_Id + " = '" + Datos.P_Caja_Id + "'";
                }
                Mi_SQL += " GROUP BY PAGOS." + Ope_Caj_Pagos.Campo_Fecha;
                Mi_SQL += " ORDER BY PAGOS." + Ope_Caj_Pagos.Campo_Fecha;
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

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
        ///NOMBRE DE LA FUNCIÓN: Reporte_Detallado_Pagos_Tarjeta
        ///DESCRIPCIÓN:consulta los pagos entre fechas indicadas que sean pagadas con tarjetas de credito.
        ///PARAMETROS: 
        ///CREO: Miguel Ismael Prieto Sánchez
        ///FECHA_CREO: 20/Abril/2012
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************* 
        internal static DataTable Reporte_Detallado_Pagos_Tarjeta(Cls_Ope_Pre_Caj_Detalles_Negocio Datos)
        {
            String Mi_SQL;
            try
            {
                Mi_SQL = "SELECT " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." + Cat_Nom_Bancos.Campo_Nombre + " as Banco";
                Mi_SQL += ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Recibo;
                Mi_SQL += ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha;
                Mi_SQL += ", " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_No_Tarjeta_Bancaria;
                Mi_SQL += ", " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_No_Autorizacion;
                Mi_SQL += ", " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Plan_Pago;
                Mi_SQL += ", " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Meses;
                Mi_SQL += ", " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Monto;
                Mi_SQL += ", " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Descripcion + " as Modulo";
                Mi_SQL += ", " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " as Caja";
                Mi_SQL += " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + ", " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + ", " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + ", " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + ", " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo;
                Mi_SQL += " WHERE " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." +  Ope_Caj_Pagos.Campo_No_Pago + " = " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_No_Pago;
                Mi_SQL += " AND " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Banco_ID + " = " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." +  Cat_Nom_Bancos.Campo_Banco_ID;
                Mi_SQL += " AND " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Caja_Id + " = " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID;
                Mi_SQL += " AND " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Modulo_Id + " = " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Modulo_Id;
                Mi_SQL += " AND " + Ope_Caj_Pagos.Campo_Fecha + " BETWEEN TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha)) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')";
                Mi_SQL += " AND TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Final)) + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS')";
                Mi_SQL += " AND " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + " = 'BANCO'";

                if (Datos.P_Modulo_Id != "")
                {
                    Mi_SQL += " AND " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Modulo_Id + " = '" + Datos.P_Modulo_Id + "'";
                }
                if (Datos.P_Caja_Id != "")
                {
                    Mi_SQL += " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID + " = '" + Datos.P_Caja_Id + "'";
                }
                Mi_SQL += " ORDER BY " + Cat_Nom_Bancos.Campo_Nombre + ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha + ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Operacion;
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

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
        ///NOMBRE DE LA FUNCIÓN: Reporte_Detallado_Pagos_Cheque
        ///DESCRIPCIÓN:consulta los pagos entre fechas indicadas que sean pagadas con cheque
        ///PARAMETROS: 
        ///CREO: Miguel Ismael Prieto Sánchez
        ///FECHA_CREO: 20/Abril/2012
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************* 
        internal static DataTable Reporte_Detallado_Pagos_Cheque(Cls_Ope_Pre_Caj_Detalles_Negocio Datos)
        {
            String Mi_SQL;
            try
            {
                Mi_SQL = "SELECT " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." + Cat_Nom_Bancos.Campo_Nombre + " as Banco";
                Mi_SQL += ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Recibo;
                Mi_SQL += ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha;
                Mi_SQL += ", " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_No_Cheque + " as No_Tarjeta_Bancaria";
                Mi_SQL += ", '' as No_Autorizacion";
                Mi_SQL += ", '' as Plan_Pago";
                Mi_SQL += ", 0 as Meses";
                Mi_SQL += ", " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Monto;
                Mi_SQL += ", " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Descripcion + " as Modulo";
                Mi_SQL += ", " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " as Caja";
                Mi_SQL += " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + ", " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + ", " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + ", " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + ", " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo;
                Mi_SQL += " WHERE " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago + " = " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_No_Pago;
                Mi_SQL += " AND " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Banco_ID + " = " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." + Cat_Nom_Bancos.Campo_Banco_ID;
                Mi_SQL += " AND " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Caja_Id + " = " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID;
                Mi_SQL += " AND " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Modulo_Id + " = " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Modulo_Id;
                Mi_SQL += " AND " + Ope_Caj_Pagos.Campo_Fecha + " BETWEEN TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha)) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')";
                Mi_SQL += " AND TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Final)) + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS')";
                Mi_SQL += " AND " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + " = 'CHEQUE'";

                if (Datos.P_Modulo_Id != "")
                {
                    Mi_SQL += " AND " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Modulo_Id + " = '" + Datos.P_Modulo_Id + "'";
                }
                if (Datos.P_Caja_Id != "")
                {
                    Mi_SQL += " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID + " = '" + Datos.P_Caja_Id + "'";
                }
                Mi_SQL += " ORDER BY " + Cat_Nom_Bancos.Campo_Nombre + ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha + ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Operacion;
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

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
        ///NOMBRE DE LA FUNCIÓN: Reporte_Detallado_Pagos_Transferencia
        ///DESCRIPCIÓN:consulta los pagos entre fechas indicadas que sean pagadas con transferencia
        ///PARAMETROS: 
        ///CREO: Miguel Ismael Prieto Sánchez
        ///FECHA_CREO: 20/Abril/2012
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************* 
        internal static DataTable Reporte_Detallado_Pagos_Transferencia(Cls_Ope_Pre_Caj_Detalles_Negocio Datos)
        {
            String Mi_SQL;
            try
            {
                Mi_SQL = "SELECT " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." + Cat_Nom_Bancos.Campo_Nombre + " as Banco";
                Mi_SQL += ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Recibo;
                Mi_SQL += ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha;
                Mi_SQL += ", " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Referencia_Transferencia + " as No_Tarjeta_Bancaria";
                Mi_SQL += ", '' as No_Autorizacion";
                Mi_SQL += ", '' as Plan_Pago";
                Mi_SQL += ", 0 as Meses";
                Mi_SQL += ", " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Monto;
                Mi_SQL += ", " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Descripcion + " as Modulo";
                Mi_SQL += ", " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " as Caja";
                Mi_SQL += " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + ", " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + ", " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + ", " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + ", " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo;
                Mi_SQL += " WHERE " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago + " = " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_No_Pago;
                Mi_SQL += " AND " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Banco_ID + " = " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." + Cat_Nom_Bancos.Campo_Banco_ID;
                Mi_SQL += " AND " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Caja_Id + " = " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID;
                Mi_SQL += " AND " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Modulo_Id + " = " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Modulo_Id;
                Mi_SQL += " AND " + Ope_Caj_Pagos.Campo_Fecha + " BETWEEN TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha)) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')";
                Mi_SQL += " AND TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Final)) + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS')";
                Mi_SQL += " AND " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + " = 'TRANSFERENCIA'";

                if (Datos.P_Modulo_Id != "")
                {
                    Mi_SQL += " AND " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Modulo_Id + " = '" + Datos.P_Modulo_Id + "'";
                }
                if (Datos.P_Caja_Id != "")
                {
                    Mi_SQL += " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID + " = '" + Datos.P_Caja_Id + "'";
                }
                Mi_SQL += " ORDER BY " + Cat_Nom_Bancos.Campo_Nombre + ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha + ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Operacion;
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

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
        ///NOMBRE DE LA FUNCIÓN: Reporte_Concentracion_Ingreso
        ///DESCRIPCIÓN:consulta los pagos entre fechas indicadas por cajero y corte
        ///PARAMETROS: 
        ///CREO: Ismael Prieto Sanchez
        ///FECHA_CREO: 30/Noviembre/2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************* 
        internal static DataTable Reporte_Concentracion_Ingreso(Cls_Ope_Pre_Caj_Detalles_Negocio Datos)
        {
            String Mi_SQL;
            DateTime Fecha;
            try
            {
                Mi_SQL = "SELECT NO_CAJA, cat_pre_modulos.descripcion AS MODULO, (cat_empleados.nombre || ' ' || cat_empleados.apellido_paterno || ' ' || cat_empleados.apellido_materno) AS EMPLEADO,";
                Mi_SQL += "NVL(SUM(ope_caj_pagos.total),'0') AS TOTAL_ANALITICO,";
                Mi_SQL += "NVL(SUM(ope_caj_turnos.total_efectivo_caja),'0') AS TOTAL_FISICO,";
                Mi_SQL += "NVL(SUM(ope_caj_turnos.sobrante),'0'), NVL(SUM(ope_caj_turnos.faltante),'0')";
                Mi_SQL += "FROM cat_pre_cajas";
                Mi_SQL += "LEFT OUTER JOIN CAT_PRE_MODULOS ON cat_pre_modulos.modulo_id = cat_pre_cajas.modulo_id";
                Mi_SQL += "LEFT OUTER JOIN OPE_CAJ_PAGOS ON ope_caj_pagos.caja_id = CAT_PRE_CAJAS.CAJA_ID";
                DateTime.TryParseExact(Datos.P_Fecha, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out Fecha);
                Mi_SQL += "AND ope_caj_pagos.fecha > '" + Fecha.ToString("dd-MM-yyyy") + "'";
                Mi_SQL += "AND ope_caj_pagos.fecha <= '" + Fecha.AddDays(1).ToString("dd-MM-yyyy") + "'";
                Mi_SQL += "LEFT OUTER JOIN OPE_CAJ_TURNOS ON ope_caj_turnos.caja_id = ope_caj_pagos.caja_id";
                Mi_SQL += "AND ope_caj_turnos.no_turno = ope_caj_pagos.no_turno";
                Mi_SQL += "LEFT OUTER JOIN cat_empleados ON cat_empleados.empleado_id = ope_caj_pagos.empleado_id";
                //Mi_SQL += "WHERE CAT_PRE_CAJAS.ESTATUS = 'VIGENTE'";
                Mi_SQL += "GROUP BY cat_pre_modulos.descripcion, NO_CAJA, (cat_empleados.nombre || ' ' || cat_empleados.apellido_paterno || ' ' || cat_empleados.apellido_materno)";
                Mi_SQL += "ORDER BY cat_pre_modulos.descripcion, NO_CAJA, (cat_empleados.nombre || ' ' || cat_empleados.apellido_paterno || ' ' || cat_empleados.apellido_materno)";

                if (Datos.P_Modulo_Id != "")
                {
                    Mi_SQL += " AND MODULO." + Cat_Pre_Modulos.Campo_Modulo_Id + " = '" + Datos.P_Modulo_Id + "'";
                }
                if (Datos.P_Caja_Id != "")
                {
                    Mi_SQL += " AND CAJA." + Cat_Pre_Cajas.Campo_Caja_Id + " = '" + Datos.P_Caja_Id + "'";
                }
                Mi_SQL += " GROUP BY PAGOS." + Ope_Caj_Pagos.Campo_Fecha;
                Mi_SQL += " ORDER BY PAGOS." + Ope_Caj_Pagos.Campo_Fecha;
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cajas
        ///DESCRIPCIÓN: Obtiene todos las Cajas que estan dadas de 
        ///             alta en la Base de Datos
        ///PARAMENTROS:   
        ///             1.  Caja.           Parametro de donde se sacara si habra o no un filtro de busqueda, en este
        ///                                 caso el filtro es la Clave.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 22/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Resumen_Diario_Ingresos()
        {
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT (SELECT SUM(" + Ope_Ing_Pasivo.Campo_Monto + ") FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo +
                    " WHERE TO_CHAR(SYSDATE,'DD')=TO_CHAR(" + Ope_Ing_Pasivo.Campo_Fecha_Pago + ",'DD') AND TO_CHAR(SYSDATE,'MM')=TO_CHAR(" +
                    Ope_Ing_Pasivo.Campo_Fecha_Pago + ",'MM') AND TO_CHAR(SYSDATE,'YYYY')=TO_CHAR(" + Ope_Ing_Pasivo.Campo_Fecha_Pago + ",'YYYY') AND " +
                    Ope_Ing_Pasivo.Campo_Clave_Ingreso_ID + "=P." + Ope_Ing_Pasivo.Campo_Clave_Ingreso_ID + ") AS RECAUDACION_DIA, ";
                Mi_SQL += "(SELECT SUM(" + Ope_Ing_Pasivo.Campo_Monto + ") FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo +
                    " WHERE TO_CHAR(SYSDATE,'MM')=TO_CHAR(" + Ope_Ing_Pasivo.Campo_Fecha_Pago + ",'MM') AND TO_CHAR(SYSDATE,'YYYY')=TO_CHAR(" +
                    Ope_Ing_Pasivo.Campo_Fecha_Pago + ",'YYYY') AND " + Ope_Ing_Pasivo.Campo_Clave_Ingreso_ID + "=P." +
                    Ope_Ing_Pasivo.Campo_Clave_Ingreso_ID + ") AS RECAUDACION_MES, ";
                Mi_SQL += "(SELECT SUM(" + Ope_Ing_Pasivo.Campo_Monto + ") FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo +
                    " WHERE TO_CHAR(SYSDATE,'YYYY')=TO_CHAR(" + Ope_Ing_Pasivo.Campo_Fecha_Pago + ",'YYYY') AND " +
                    Ope_Ing_Pasivo.Campo_Clave_Ingreso_ID + "=P." + Ope_Ing_Pasivo.Campo_Clave_Ingreso_ID + ") AS RECAUDACION_ANIO, ";
                Mi_SQL += "(SELECT " + Cat_Pre_Clav_Ing_Presupuestos.Campo_Importe + " FROM " + Cat_Pre_Clav_Ing_Presupuestos.Tabla_Cat_Pre_Clav_Ing_Presupuestos
                    + " WHERE " + Cat_Pre_Clav_Ing_Presupuestos.Campo_Anio + "=TO_CHAR(SYSDATE,'YYYY') AND " + Cat_Pre_Clav_Ing_Presupuestos.Campo_Clave_Ingreso_Id + "=P." + Ope_Ing_Pasivo.Campo_Clave_Ingreso_ID + ") AS PRESUPUESTO_ANIO, ";
                Mi_SQL += "NULL AS PORCENTAJE, NULL AS PRESUPUESTO_EJERCER, C." + Cat_Pre_Claves_Ingreso.Campo_Descripcion + " FROM ";
                Mi_SQL += Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " P LEFT OUTER JOIN " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + " C ON ";
                Mi_SQL += "P." + Ope_Ing_Pasivo.Campo_Clave_Ingreso_ID + "=C." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID + " GROUP BY ";
                Mi_SQL += "C." + Cat_Pre_Claves_Ingreso.Campo_Descripcion + ", P." + Ope_Ing_Pasivo.Campo_Clave_Ingreso_ID;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Cajas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cajas
        ///DESCRIPCIÓN: Obtiene todos las Cajas que estan dadas de 
        ///             alta en la Base de Datos
        ///PARAMENTROS:   
        ///             1.  Caja.           Parametro de donde se sacara si habra o no un filtro de busqueda, en este
        ///                                 caso el filtro es la Clave.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 22/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Corte_Caja(Cls_Ope_Pre_Caj_Detalles_Negocio Caj_Det)
        {
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT C." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " AS NO_CAJA, P." + Ope_Caj_Pagos.Campo_No_Recibo + " AS NO_RECIBO, P." +
                    Ope_Caj_Pagos.Campo_No_Operacion + " AS NO_OPERACION, CON." + Cat_Pre_Contribuyentes.Campo_Nombre + "||' '||CON." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno +
                        "||' '||CON." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " AS NOMBRE, P." + Ope_Caj_Pagos.Campo_Monto_Corriente + " AS IMPORTE, P." +
                        Ope_Caj_Pagos.Campo_Monto_Rezago + " AS REZAGO, (P." + Ope_Caj_Pagos.Campo_Monto_Recargos + " + P." + Ope_Caj_Pagos.Campo_Monto_Recargos_Moratorios + ") AS RECARGOS, P."
                        + Ope_Caj_Pagos.Campo_Honorarios + " AS HONORARIOS, (P." + Ope_Caj_Pagos.Campo_Descuento_Honorarios + " + P." + Ope_Caj_Pagos.Campo_Descuento_Moratorios + " + P." + Ope_Caj_Pagos.Campo_Descuento_Recargos + " + P." + Ope_Caj_Pagos.Campo_Descuento_Pronto_Pago + ") AS DESCUENTOS, P."
                        + Ope_Caj_Pagos.Campo_Total + " AS TOTAL, 'CORTE_CAJA' AS TIPO FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " P LEFT OUTER JOIN "
                        + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " PROP ON P." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + "=PROP." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID +
                        " LEFT OUTER JOIN " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " C ON P." + Ope_Caj_Pagos.Campo_Caja_ID + "=C." + Cat_Pre_Cajas.Campo_Caja_Id +
                        " LEFT OUTER JOIN " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " CON ON PROP." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + "=CON." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID +
                        " WHERE PROP." + Cat_Pre_Propietarios.Campo_Tipo + " IN ('PROPIETARIO','POSEEDOR') AND P." + Ope_Caj_Pagos.Campo_Caja_ID + "='" + Caj_Det.P_Caja_Id + "' AND TO_CHAR(" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Caj_Det.P_Fecha)) + ",'DD')=TO_CHAR(P." + Ope_Caj_Pagos.Campo_Fecha + ",'DD') AND TO_CHAR(" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Caj_Det.P_Fecha)) + ",'MM')=TO_CHAR(P." +
                    Ope_Caj_Pagos.Campo_Fecha + ",'MM') AND TO_CHAR(" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Caj_Det.P_Fecha)) + ",'YYYY')=TO_CHAR(P." + Ope_Caj_Pagos.Campo_Fecha + ",'YYYY')";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Cajas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return tabla;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Analisis_Entrega_Dia
        ///DESCRIPCIÓN: Obtiene los datos del reporte de analisis de entrega por día.
        ///PARAMENTROS:   
        ///             1.  Caj_Det.        Contiene los filtros para la consulta
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 16/Enero/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Analisis_Entrega_Dia(Cls_Ope_Pre_Caj_Detalles_Negocio Caj_Det)
        {
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT CAJA." + Cat_Pre_Cajas.Campo_Numero_De_Caja + ", EMP." + Cat_Empleados.Campo_Nombre + "||' '||EMP." + Cat_Empleados.Campo_Apellido_Paterno + "||' '||EMP." + Cat_Empleados.Campo_Apellido_Materno + " AS NOMBRE_CAJERO, DEP." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA, ";
                Mi_SQL += "(TURNO." + Ope_Caj_Turnos.Campo_Total_Bancos + "+TURNO." + Ope_Caj_Turnos.Campo_Total_Cheques + "+TURNO." + Ope_Caj_Turnos.Campo_Total_Recolectado + "+TURNO." + Ope_Caj_Turnos.Campo_Total_Efectivo_Sistema + ") AS INGRESO_SEGUN_ANALITICO, ";
                Mi_SQL += "(TURNO." + Ope_Caj_Turnos.Campo_Total_Bancos + "+TURNO." + Ope_Caj_Turnos.Campo_Total_Cheques + "+TURNO." + Ope_Caj_Turnos.Campo_Total_Recolectado + "+TURNO." + Ope_Caj_Turnos.Campo_Total_Efectivo_Caja + ") AS INGRESO_FISICO, TURNO." + Ope_Caj_Turnos.Campo_Sobrante + ", TURNO." + Ope_Caj_Turnos.Campo_Faltante + ", 'ANALISIS_DIA' AS TIPO";
                Mi_SQL += " FROM " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " CAJA LEFT OUTER JOIN " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + " TURNO ON CAJA." + Cat_Pre_Cajas.Campo_Caja_Id + "=TURNO." + Ope_Caj_Turnos.Campo_Caja_Id;
                Mi_SQL += " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " EMP ON TURNO." + Ope_Caj_Turnos.Campo_Empleado_ID + "=EMP." + Cat_Empleados.Campo_Empleado_ID + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEP ON EMP." + Cat_Empleados.Campo_Dependencia_ID + "=DEP." + Cat_Dependencias.Campo_Dependencia_ID;
                Mi_SQL += " WHERE (TURNO." + Ope_Caj_Turnos.Campo_Fecha_Turno + " BETWEEN TO_DATE('" + Caj_Det.P_Fecha + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS') AND TO_DATE('" + Caj_Det.P_Fecha_Final + " 00:00:00', 'DD-MM-YYYY HH24-MI-SS')) AND CAJA." + Cat_Pre_Cajas.Campo_Caja_Id + " LIKE '%" + Caj_Det.P_Caja_Id + "%' ";
                Mi_SQL += " ORDER BY CAJA." + Cat_Pre_Cajas.Campo_Numero_De_Caja + ", TURNO." + Ope_Caj_Turnos.Campo_Fecha_Turno;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Cajas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return tabla;
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Reporte_Recibos_Cancelados_Empleado
        ///DESCRIPCIÓN:consulta los acumulados de los recibos cancelados por empleados por año
        ///PARAMETROS: 
        ///CREO: Ismael Prieto Sanchez
        ///FECHA_CREO: 24/Abril/2012
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************* 
        internal static DataTable Reporte_Recibos_Cancelados_Empleado(Cls_Ope_Pre_Caj_Detalles_Negocio Datos)
        {
            String Mi_SQL;
            try
            {
                Mi_SQL = "SELECT (" + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || " + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || " + Cat_Empleados.Campo_Nombre + ")  AS Empleado,";
                Mi_SQL+= " NVL(COUNT(" + Ope_Caj_Pagos.Campo_No_Pago + "),'0') as Cantidad_Cancelados";
                Mi_SQL+= " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + ", " + Cat_Empleados.Tabla_Cat_Empleados;
                Mi_SQL+= " WHERE " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Empleado_ID + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID;
                Mi_SQL+= " AND TO_CHAR(" + Ope_Caj_Pagos.Campo_Fecha + ",'YYYY') = " + Convert.ToDateTime(Datos.P_Fecha).Year;
                if (Datos.P_Empleado_ID != "")
                {
                    Mi_SQL += " AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'";
                }
                Mi_SQL+= " GROUP BY (" + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || " + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || " + Cat_Empleados.Campo_Nombre + ")";
                Mi_SQL += " ORDER BY (" + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || " + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || " + Cat_Empleados.Campo_Nombre + ")";
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

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

    }
}
