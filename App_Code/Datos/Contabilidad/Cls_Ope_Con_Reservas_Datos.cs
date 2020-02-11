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
using Presidencia.Generar_Reservas.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Sessiones;
using Presidencia.Administrar_Requisiciones.Negocios;
using System.Data.OracleClient;

namespace Presidencia.Generar_Reservas.Datos
{
    public class Cls_Ope_Con_Reservas_Datos
    {
        public Cls_Ope_Con_Reservas_Datos()
        {
        }

        //*********************************************************************************************
        //*********************************************************************************************
        //*********************************************************************************************
        #region CONSULTAS / FTE FINANCIAMIENTO, PROYECTOS, PARTIDAS, PRESUPUESTOS
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Fuentes_Financiamiento
        ///DESCRIPCIÓN: crea una sentencia sql para Consultar_Fuentes_Financiamiento
        ///PARAMETROS: 1.-Clase de Negocio
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 25/Ene/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO: 
        ///CAUSA_MODIFICACIÓN
        ///******************************************************************************* 
        public static DataTable Consultar_Fuentes_Financiamiento(Cls_Ope_Con_Reservas_Negocio Reserva_Negocio)
        {
            try
            {
                String Mi_SQL = "";
                Mi_SQL =
                "SELECT FUENTE." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID + "," +
                " FUENTE." + Cat_SAP_Fuente_Financiamiento.Campo_Clave + " ||' '||" +
                " FUENTE." + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion +
                " FROM " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + " FUENTE" +
                " JOIN " + Cat_SAP_Det_Fte_Dependencia.Tabla_Cat_SAP_Det_Fte_Financiamiento_Dependencia + " DETALLE" +
                " ON FUENTE." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID + " = " +
                " DETALLE." + Cat_SAP_Det_Fte_Dependencia.Campo_Fuente_Financiamiento_ID +
                " WHERE DETALLE." + Cat_SAP_Det_Fte_Dependencia.Campo_Dependencia_ID + " = " +
                "'" + Reserva_Negocio.P_Dependencia_ID + "'" +
                " AND FUENTE." + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID + " IN " +
                "(SELECT " + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID + 
                " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto +
                " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + " = " +
                "'" + Reserva_Negocio.P_Dependencia_ID + "'" + ")" +
                " ORDER BY " + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion + " ASC";
                DataTable Data_Table =
                    OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                return Data_Table;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Proyectos_Programas
        ///DESCRIPCIÓN: crea una sentencia sql para Consultar_Proyectos_Programas
        ///PARAMETROS: 1.-Clase de Negocio
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 25/Ene/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO: 
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        public static DataTable Consultar_Proyectos_Programas(Cls_Ope_Con_Reservas_Negocio Reserva_Negocio)
        {
            String Mi_SQL = "";
            Mi_SQL =
            "SELECT PROGRAMA." + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID + "," +
            " PROGRAMA." + Cat_Com_Proyectos_Programas.Campo_Clave + " ||' '||" +
            " PROGRAMA." + Cat_Com_Proyectos_Programas.Campo_Nombre + "," +
            " PROGRAMA." + Cat_Com_Proyectos_Programas.Campo_Elemento_PEP +
            " FROM " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + " PROGRAMA" +            
            " JOIN " + Cat_SAP_Det_Prog_Dependencia.Tabla_Cat_SAP_Det_Programa_Dependencia + " DETALLE" +
            " ON PROGRAMA." + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID + " = " +
            " DETALLE." + Cat_SAP_Det_Prog_Dependencia.Campo_Proyecto_Programa_ID +
            " WHERE DETALLE." + Cat_SAP_Det_Prog_Dependencia.Campo_Dependencia_ID + " = " +
            "'" + Reserva_Negocio.P_Dependencia_ID + "' ORDER BY " + 
            Cat_Com_Proyectos_Programas.Campo_Descripcion + " ASC";
            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Partidas_De_Un_Programa
        ///DESCRIPCIÓN: crea una sentencia sql para Consultar_Partidas_De_Un_Programa
        ///PARAMETROS: 1.-Clase de Negocio
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 25/Ene/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO: 
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        //CONSULTAR PARTIDAS DE UN PROGRAMA
        public static DataTable Consultar_Partidas_De_Un_Programa(Cls_Ope_Con_Reservas_Negocio Reservas_Negocio) 
        {
            String Mi_SQL = "SELECT PARTIDA." + Cat_Com_Partidas.Campo_Partida_ID + ", " +
            " PARTIDA." + Cat_Com_Partidas.Campo_Clave +" ||' '||" +
            " PARTIDA." + Cat_Com_Partidas.Campo_Nombre +
            " FROM " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas + " PARTIDA" +
            " JOIN " + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas + " DETALLE" +
            " ON PARTIDA." + Cat_Com_Partidas.Campo_Partida_ID + " = " +
            " DETALLE." + Cat_Sap_Det_Prog_Partidas.Campo_Det_Partida_ID +
            " WHERE DETALLE." + Cat_Sap_Det_Prog_Partidas.Campo_Det_Proyecto_Programa_ID + " = " +
            "'" + Reservas_Negocio.P_Proyecto_Programa_ID + "'";
            Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Com_Partidas.Campo_Nombre + " ASC";
            DataTable Data_Table = 
                OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }

        //OBTINE PARTIDAS ESPECIFICAS CON PRESPUESTOS A PARTIR DE LA DEPENDENCIA Y EL PROYECTO
        public static DataTable Consultar_Presupuesto_Partidas(Cls_Ope_Con_Reservas_Negocio Requisicion_Negocio)
        {
            String Mi_SQL = "";

            Mi_SQL = 
            "SELECT " +
            Cat_Com_Dep_Presupuesto.Campo_Partida_ID + ", " +
                "(SELECT " + Cat_Com_Partidas.Campo_Nombre + " FROM " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas +
                " WHERE " + Cat_Com_Partidas.Campo_Partida_ID + " = " + 
                Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + 
                Cat_Com_Dep_Presupuesto.Campo_Partida_ID + ") NOMBRE, " +

                "(SELECT " + Cat_Com_Partidas.Campo_Clave + " FROM " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas +
                " WHERE " + Cat_Com_Partidas.Campo_Partida_ID + " = " +
                Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." +
                Cat_Com_Dep_Presupuesto.Campo_Partida_ID + ") CLAVE, " +

                "(SELECT " + Cat_Com_Partidas.Campo_Clave + " ||' '||" + 
                Cat_Com_Partidas.Campo_Nombre + " FROM " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas +
                " WHERE " + Cat_Com_Partidas.Campo_Partida_ID + " = " +
                Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." +
                Cat_Com_Dep_Presupuesto.Campo_Partida_ID + ") CLAVE_NOMBRE, " +
            Cat_Com_Dep_Presupuesto.Campo_Monto_Presupuestal + ", " +
            Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible + " MONTO_DISPONIBLE, " +
            Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido + ", " +
            Cat_Com_Dep_Presupuesto.Campo_Monto_Ejercido + ", " +
            Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto + ", " +
            Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + ", " +
            Cat_Com_Dep_Presupuesto.Campo_Fecha_Creo + 
            //" TO_CHAR(FECHA_CREO ,'DD/MM/YY') FECHA_CREO" + 
            " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto +
            " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID + 
            " = '" + Requisicion_Negocio.P_Proyecto_Programa_ID + "'" +
            " AND " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + 
            " = '" + Requisicion_Negocio.P_Dependencia_ID + "'" +
            " AND " + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID +
            " = '" + Requisicion_Negocio.P_Fuente_Financiamiento + "'" +
            " AND " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID +
            " IN (" + Requisicion_Negocio.P_Partida_ID + ")" +
            " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto +
            " = '" + Requisicion_Negocio.P_Anio_Presupuesto + "'" +
            " AND " + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + " = " +
                "(SELECT MAX(" + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + ")" +
                " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + 
                " WHERE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." +
                            Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID +
                            " = '" + Requisicion_Negocio.P_Proyecto_Programa_ID + "'" +
                            " AND " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." +
                            Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID +
                            " = '" + Requisicion_Negocio.P_Dependencia_ID + "'" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID + 
                            " = '" + Requisicion_Negocio.P_Fuente_Financiamiento + "'" +

                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto +
                            " = '" + Requisicion_Negocio.P_Anio_Presupuesto + "'" +

                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID +
                            " IN (" + Requisicion_Negocio.P_Partida_ID + "))";

            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }
        #endregion                             
        #region CONSULTA RESERVAS
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Reservas
        ///DESCRIPCIÓN: crea una sentencia sql para conultar una Requisa en la base de datos
        ///PARAMETROS: 1.-Clase de Negocio
        ///            2.-Usuario que crea la requisa
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: Noviembre/2010 
        ///MODIFICO:Gustavo Angeles Cruz
        ///FECHA_MODIFICO: 25/Ene/2011
        ///CAUSA_MODIFICACIÓN
        ///******************************************************************************* 
        public static DataTable Consultar_Reservas(Cls_Ope_Con_Reservas_Negocio Reserva_Negocio)
        {
          
            String Mi_Sql = "";
            Mi_Sql = "SELECT " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + ".*, " +
                "(SELECT " + Cat_Dependencias.Campo_Nombre + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias +
                " WHERE " + Cat_Dependencias.Campo_Dependencia_ID + " = " +
                Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + "." + Ope_Psp_Reservas.Campo_Dependencia_ID +
                ") NOMBRE_DEPENDENCIA " +
            " FROM " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas;
            if (!string.IsNullOrEmpty(Reserva_Negocio.P_Dependencia_ID) && Reserva_Negocio.P_Dependencia_ID != "0")
            {
                Mi_Sql += " WHERE " + Ope_Psp_Reservas.Campo_Dependencia_ID +
                " = '" + Reserva_Negocio.P_Dependencia_ID + "'";
                Mi_Sql += " AND TO_DATE(TO_CHAR(" + Ope_Psp_Reservas.Campo_Fecha + ",'DD-MM-YYYY'))" +
                        " >= '" + Reserva_Negocio.P_Fecha_Inicial + "' AND " +
                "TO_DATE(TO_CHAR(" + Ope_Psp_Reservas.Campo_Fecha + ",'DD-MM-YYYY'))" +
                        " <= '" + Reserva_Negocio.P_Fecha_Final + "'";
            }
            if (!string.IsNullOrEmpty(Reserva_Negocio.P_No_Reserva))
            {
                Mi_Sql +=
                " AND " + Ope_Psp_Reservas.Campo_No_Reserva  +
                " = " + Reserva_Negocio.P_No_Reserva ;
            }
            Mi_Sql = Mi_Sql + " ORDER BY " + Ope_Psp_Reservas.Campo_No_Reserva  + " DESC";
            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            if (Data_Set != null && Data_Set.Tables[0].Rows.Count > 0)
            {
                return (Data_Set.Tables[0]);
            }
            else
            {
                return null;
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Modificar_Reserva
        /// DESCRIPCION : Modifica la reserva seleccionado
        /// PARAMETROS  : Datos: Contiene los datos proporcionados
        /// CREO        : Sergio Manuel Gallardo Andrade
        /// FECHA_CREO  : 22/Noviembre/2011
        /// MODIFICO          : 
        /// FECHA_MODIFICO    : 
        /// CAUSA_MODIFICACION: 
        ///*******************************************************************************
        public static void Modificar_Reserva(Cls_Ope_Con_Reservas_Negocio  Datos)
        {
            try
            {
                String Mi_SQL;  //Almacena la sentencia de modificacion.

                Mi_SQL = "UPDATE " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + " SET ";
                Mi_SQL += Ope_Psp_Reservas.Campo_Estatus + " = '" + Datos.P_Estatus + "', ";
                Mi_SQL += Ope_Psp_Reservas.Campo_Saldo + " = '0', ";
                Mi_SQL += Ope_Psp_Reservas.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario_Modifico + "',";
                Mi_SQL += Ope_Psp_Reservas.Campo_Fecha_Modifico + " =SYSDATE";
                Mi_SQL += " WHERE " + Ope_Psp_Reservas.Campo_No_Reserva + " = '" + Datos.P_No_Reserva + "'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error : " + ex.Message, ex);
            }
        }
        #endregion

    }
}
