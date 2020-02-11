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
using System.Collections.Generic;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Reporte_Presupuesto_Egresos.Negocio;
using Presidencia.Grupos_Dependencias.Negocio;

namespace Presidencia.Reporte_Presupuesto_Egresos.Datos
{
    public class Cls_Rpt_Presupuesto_Egresos_Datos
    {
        #region(Metodos)
        /// ********************************************************************************************************************
        /// NOMBRE:         Consultar_Dependencias
        /// COMENTARIOS:    Se realiza una consulta de las dependencias que tienen el cierto grupo de dependnecia id
        /// PARÁMETROS:     
        /// USUARIO CREÓ:   Hugo Enrique Ramirez Aguilera
        /// FECHA CREÓ:     05/Diciembre/2011 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static DataTable Consultar_Dependencias(Cls_Rpt_Presupuesto_Egresos_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Presupuesto = new DataTable();
            try
            {
                Mi_SQL.Append("Select * from " + Cat_Dependencias.Tabla_Cat_Dependencias);
                Mi_SQL.Append( " Where " +Cat_Dependencias.Campo_Grupo_Dependencia_ID +"='" +Datos.P_Grupo_Dependencia_ID +"'");
                Dt_Presupuesto = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los Movimientos Presupuestales que existen actualmente en el sistema. Error: [" + Ex.Message + "]");
            }
            return Dt_Presupuesto;
        }

        /// ********************************************************************************************************************
        /// NOMBRE:         Consultar_Presupuesto_Dependencias
        /// COMENTARIOS:    Se realiza una consulta de las dependencias que tienen en el presupuesto
        /// PARÁMETROS:     
        /// USUARIO CREÓ:   Hugo Enrique Ramirez Aguilera
        /// FECHA CREÓ:     05/Diciembre/2011 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static DataTable Consultar_Presupuesto_Dependencias(Cls_Rpt_Presupuesto_Egresos_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Presupuesto = new DataTable();
            try
            {
                Mi_SQL.Append("Select * from " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado);
                Mi_SQL.Append(" Where " + Ope_Psp_Presupuesto_Aprobado.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'");
                Mi_SQL.Append(" and " + Ope_Psp_Presupuesto_Aprobado.Campo_Anio + "=" + Datos.P_Anio);
                Dt_Presupuesto = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los Movimientos Presupuestales que existen actualmente en el sistema. Error: [" + Ex.Message + "]");
            }
            return Dt_Presupuesto;
        }
        /// ********************************************************************************************************************
        /// NOMBRE:         Consultar_Presupuesto_Programa
        /// COMENTARIOS:    Se realiza una consulta de los programas que tienen en el presupuesto
        /// PARÁMETROS:     
        /// USUARIO CREÓ:   Hugo Enrique Ramirez Aguilera
        /// FECHA CREÓ:     07/Diciembre/2011 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static DataTable Consultar_Presupuesto_Programa(Cls_Rpt_Presupuesto_Egresos_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Presupuesto = new DataTable();
            try
            {
                Mi_SQL.Append("Select * from " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado);
                Mi_SQL.Append(" Where " + Ope_Psp_Presupuesto_Aprobado.Campo_Proyecto_Programa_ID + "='" + Datos.P_Programa_ID + "'");
                Mi_SQL.Append(" and " + Ope_Psp_Presupuesto_Aprobado.Campo_Anio + "=" + Datos.P_Anio);
                Dt_Presupuesto = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los Movimientos Presupuestales que existen actualmente en el sistema. Error: [" + Ex.Message + "]");
            }
            return Dt_Presupuesto;
        }
        /// ********************************************************************************************************************
        /// NOMBRE:         Consultar_Presupuesto_Partida
        /// COMENTARIOS:    Se realiza una consulta de las PARTIDAS que tienen se encuentren dentro del presupuesto
        /// PARÁMETROS:     
        /// USUARIO CREÓ:   Hugo Enrique Ramirez Aguilera
        /// FECHA CREÓ:     07/Diciembre/2011 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static DataTable Consultar_Presupuesto_Partida(Cls_Rpt_Presupuesto_Egresos_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Presupuesto = new DataTable();
            try
            {
                Mi_SQL.Append("Select * from " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado);
                Mi_SQL.Append(" Where " + Ope_Psp_Presupuesto_Aprobado.Campo_Partida_ID + "='" + Datos.P_Partida_ID + "'");
                Mi_SQL.Append(" and " + Ope_Psp_Presupuesto_Aprobado.Campo_Anio + "=" + Datos.P_Anio);
                Dt_Presupuesto = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los Movimientos Presupuestales que existen actualmente en el sistema. Error: [" + Ex.Message + "]");
            }
            return Dt_Presupuesto;
        }

        /// ********************************************************************************************************************
        /// NOMBRE:         Consultar_Presupuesto_Partida
        /// COMENTARIOS:    Se realiza una consulta de las PARTIDAS que tienen se encuentren dentro del presupuesto
        /// PARÁMETROS:     
        /// USUARIO CREÓ:   Hugo Enrique Ramirez Aguilera
        /// FECHA CREÓ:     07/Diciembre/2011 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ********************************************************************************************************************
        public static DataTable Consultar_Presupuesto_Año(Cls_Rpt_Presupuesto_Egresos_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Presupuesto = new DataTable();
            try
            {
                Mi_SQL.Append("Select distinct " +Ope_Psp_Presupuesto_Aprobado.Campo_Anio);
                Mi_SQL.Append(" from " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado);
                Mi_SQL.Append(" Order by " + Ope_Psp_Presupuesto_Aprobado.Campo_Anio + " ASC");
                Dt_Presupuesto = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los Movimientos Presupuestales que existen actualmente en el sistema. Error: [" + Ex.Message + "]");
            }
            return Dt_Presupuesto;
        }
        #endregion
    }
}