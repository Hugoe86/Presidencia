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
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Reporte_Cat_Catastrales.Negocio;
/// <summary>
/// Summary description for Cls_Rpt_Cat_Catastrales_Datos
/// </summary>
/// 
namespace Presidencia.Reporte_Cat_Catastrales.Datos
{
    public class Cls_Rpt_Cat_Catastrales_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Asignacion_Modificacion_Claves
        ///DESCRIPCIÓN: Obtiene las claves catastrales que se agregaron y/modificaron
        ///PARAMETROS:
        ///CREO: David Herrera Rincon
        ///FECHA_CREO: 25/Octubre/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Asignacion_Modificacion_Claves(Cls_Rpt_Cat_Catastrales_Negocio Datos)
        {
            DataTable Dt_Claves = new DataTable();
            String My_Sql = "";
            try
            {
                My_Sql = "SELECT T." + Cat_Tra_Tramites.Campo_Nombre + ",";
                My_Sql += " (SELECT " + Ope_Tra_Datos.Campo_Valor + " FROM " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos + " WHERE " + Ope_Tra_Datos.Campo_Dato_ID + " = Dt." + Cat_Tra_Datos_Tramite.Campo_Dato_ID + "";
                My_Sql += " AND " + Ope_Tra_Datos.Campo_Tramite_ID + " = T." + Cat_Tra_Tramites.Campo_Tramite_ID + " AND " + Ope_Tra_Datos.Campo_Solicitud_ID + " = S." + Ope_Tra_Solicitud.Campo_Solicitud_ID + ") AS Perito,";
                My_Sql += " S." + Ope_Tra_Solicitud.Campo_Fecha_Creo + " AS Fecha_Ingreso, S." + Ope_Tra_Solicitud.Campo_Fecha_Entrega + ", S." + Ope_Tra_Solicitud.Campo_Cuenta_Predial + ",";
                My_Sql += " S." + Ope_Tra_Solicitud.Campo_Cantidad +" AS Total";
                My_Sql += " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " S LEFT OUTER JOIN " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " T";
                My_Sql += " ON S." + Ope_Tra_Solicitud.Campo_Tramite_ID + " = T." + Cat_Tra_Tramites.Campo_Tramite_ID + " LEFT OUTER JOIN " + Cat_Tra_Datos_Tramite.Tabla_Cat_Tra_Datos_Tramite + " Dt";
                My_Sql += " ON T." + Cat_Tra_Tramites.Campo_Tramite_ID + " = Dt." + Cat_Tra_Datos_Tramite.Campo_Tramite_ID + "";

                if (Datos.P_Tipo_Consulta == "CLAVE")
                {
                    My_Sql += " WHERE UPPER(T." + Cat_Tra_Tramites.Campo_Nombre + ") IN ('ASIGNACION DE CLAVES CATASTRALES', 'MODIFICACION DE CLAVES CATASTRALES')";
                    My_Sql += " AND UPPER(T." + Cat_Tra_Tramites.Campo_Clave_Tramite + ") IN ('ACC','MCC')";
                }
                else if (Datos.P_Tipo_Consulta == "CEDULA")
                {
                    My_Sql += " WHERE UPPER(T." + Cat_Tra_Tramites.Campo_Nombre + ") IN ('ALTA DE CEDULA CATASTRAL', 'MODIFICACION DE CEDULA CATASTRAL')";
                    My_Sql += " AND UPPER(T." + Cat_Tra_Tramites.Campo_Clave_Tramite + ") IN ('AC','MC')";
                }

                My_Sql += " AND (S." + Ope_Tra_Solicitud.Campo_Fecha_Creo + " BETWEEN TO_DATE ('" + String.Format("{0:dd-MM-yyy}", Datos.P_Fecha_Inicio) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')  AND TO_DATE ('" + String.Format("{0:dd-MM-yyy}", Datos.P_Fecha_Fin) + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS'))";

                DataSet Ds_Tabla = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, My_Sql);
                if (Ds_Tabla != null)
                {
                    Dt_Claves = Ds_Tabla.Tables[0];
                }
            }
            catch (Exception E)
            {
                throw new Exception("Consultar_Asignacion_Modificacion_Claves: Error al consultar las asignaciones.");
            }
            return Dt_Claves;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Areas_Privativas
        ///DESCRIPCIÓN: Obtiene las areas privativas
        ///PARAMETROS:
        ///CREO: David Herrera Rincon
        ///FECHA_CREO: 25/Octubre/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Areas_Privativas(Cls_Rpt_Cat_Catastrales_Negocio Datos)
        {
            DataTable Dt_Claves = new DataTable();
            String My_Sql = "";
            try
            {
                My_Sql = "SELECT S." + Ope_Tra_Solicitud.Campo_Fecha_Creo + " AS FECHA, S." + Ope_Tra_Solicitud.Campo_Cuenta_Predial + ", S." + Ope_Tra_Solicitud.Campo_Costo_Total + " AS Cantidad,";
                My_Sql += " (Co." + Cat_Pre_Contribuyentes.Campo_Nombre + " ||' '|| Co." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " ||' '|| Co." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + ") AS Propietario,";
                My_Sql += " (SELECT " + Ope_Tra_Datos.Campo_Valor + " FROM " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos + " WHERE S." + Ope_Tra_Solicitud.Campo_Solicitud_ID + " = " + Ope_Tra_Datos.Campo_Solicitud_ID + " AND T." + Cat_Tra_Tramites.Campo_Tramite_ID + " = " + Ope_Tra_Datos.Campo_Tramite_ID + " AND Dt." + Cat_Tra_Datos_Tramite.Campo_Dato_ID + " = " + Ope_Tra_Datos.Campo_Dato_ID + " AND UPPER(Dt." + Cat_Tra_Datos_Tramite.Campo_Nombre + ") IN ('TIPO DE CONDOMINIO')) AS Tipo_Condominio,";
                My_Sql += " (SELECT " + Ope_Tra_Datos.Campo_Valor + " FROM " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos + " WHERE S." + Ope_Tra_Solicitud.Campo_Solicitud_ID + " = " + Ope_Tra_Datos.Campo_Solicitud_ID + " AND T." + Cat_Tra_Tramites.Campo_Tramite_ID + " = " + Ope_Tra_Datos.Campo_Tramite_ID + " AND Dt." + Cat_Tra_Datos_Tramite.Campo_Dato_ID + " = " + Ope_Tra_Datos.Campo_Dato_ID + " AND UPPER(Dt." + Cat_Tra_Datos_Tramite.Campo_Nombre + ") IN ('DENOMINACION')) AS Denominacion,";
                My_Sql += " (SELECT " + Ope_Tra_Datos.Campo_Valor + " FROM " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos + " WHERE S." + Ope_Tra_Solicitud.Campo_Solicitud_ID + " = " + Ope_Tra_Datos.Campo_Solicitud_ID + " AND T." + Cat_Tra_Tramites.Campo_Tramite_ID + " = " + Ope_Tra_Datos.Campo_Tramite_ID + " AND Dt." + Cat_Tra_Datos_Tramite.Campo_Dato_ID + " = " + Ope_Tra_Datos.Campo_Dato_ID + " AND UPPER(Dt." + Cat_Tra_Datos_Tramite.Campo_Nombre + ") IN ('UBICACION')) AS Ubicacion,";
                My_Sql += " (SELECT " + Ope_Tra_Datos.Campo_Valor + " FROM " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos + " WHERE S." + Ope_Tra_Solicitud.Campo_Solicitud_ID + " = " + Ope_Tra_Datos.Campo_Solicitud_ID + " AND T." + Cat_Tra_Tramites.Campo_Tramite_ID + " = " + Ope_Tra_Datos.Campo_Tramite_ID + " AND Dt." + Cat_Tra_Datos_Tramite.Campo_Dato_ID + " = " + Ope_Tra_Datos.Campo_Dato_ID + " AND UPPER(Dt." + Cat_Tra_Datos_Tramite.Campo_Nombre + ") IN ('AREAS PRIVATIVAS')) AS Areas_Privativas";
                My_Sql += " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " Co JOIN " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " P";
                My_Sql += " ON Co." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = P." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " Cp";
                My_Sql += " ON P." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " = Cp." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " JOIN " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " S";
                My_Sql += " ON Cp." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " = S." + Ope_Tra_Solicitud.Campo_Cuenta_Predial + " LEFT OUTER JOIN " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " T";
                My_Sql += " ON S." + Ope_Tra_Solicitud.Campo_Tramite_ID + " = T." + Cat_Tra_Tramites.Campo_Tramite_ID + " LEFT OUTER JOIN " + Cat_Tra_Datos_Tramite.Tabla_Cat_Tra_Datos_Tramite + " Dt";
                My_Sql += " ON T." + Cat_Tra_Tramites.Campo_Tramite_ID + " = Dt." + Cat_Tra_Tramites.Campo_Tramite_ID + "";
                My_Sql += " WHERE UPPER(Co." + Cat_Pre_Contribuyentes.Campo_Tipo_Propietario + ") IN ('PROPIETARIO','POSEEDOR') AND UPPER(S." + Ope_Tra_Solicitud.Campo_Estatus + ") IN ('TERMINADO')";
                My_Sql += " AND UPPER(T." + Cat_Tra_Tramites.Campo_Nombre + ") IN ('AUTORIZACION DE REGIMEN EN ONDOMINIO', 'MODIFICACION DE REGIMEN EN CONDOMINIO', 'REGIMEN DE CONDOMINIO DE 24 UNIDADES')";
                My_Sql += " AND (S." + Ope_Tra_Solicitud.Campo_Fecha_Creo + " BETWEEN TO_DATE ('" + String.Format("{0:dd-MM-yyy}", Datos.P_Fecha_Inicio) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')  AND TO_DATE ('" + String.Format("{0:dd-MM-yyy}", Datos.P_Fecha_Fin) + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS'))";

                DataSet Ds_Tabla = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, My_Sql);
                if (Ds_Tabla != null)
                {
                    Dt_Claves = Ds_Tabla.Tables[0];
                }
            }
            catch (Exception E)
            {
                throw new Exception("Consultar_Areas_Privativas: Error al consultar las areas privativas.");
            }
            return Dt_Claves;
        }
    }
}
