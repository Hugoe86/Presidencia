using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;

namespace Presidencia.Manejo_Presupuesto_SAP.Datos
{
    public class Cls_Ope_Psp_Manejo_Presupuesto_SAP
    {
        #region VARIABLES

        public static String DISPONIBLE = "MONTO_DISPONIBLE";
        public static String COMPROMETIDO = "MONTO_COMPROMETIDO";

        #endregion

        public Cls_Ope_Psp_Manejo_Presupuesto_SAP()
        {
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Presupuesto
        ///DESCRIPCIÓN: Retorna DataTable con todos los campos de la tabla de una partida
        ///PARAMETROS: Dependencia_ID, Fte_Financiamiento_ID, Programa_ID, Capitulo_ID, Partida_ID, Anio
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 02 MAY 2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Presupuesto_Aprobado(String Dependencia_ID, String Fte_Financiamiento_ID, String Programa_ID, String Capitulo_ID, String Partida_ID, int Anio)
        {
            String Mi_SQL = "";
            DataTable Dt_Presupuesto = null;
            try
            {
                Mi_SQL = "SELECT * FROM " + Ope_Sap_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto +
                " WHERE " + Ope_Sap_Dep_Presupuesto.Campo_Anio_Presupuesto + " = " + Anio;
                if (!String.IsNullOrEmpty(Dependencia_ID))
                {
                    Mi_SQL += " AND " + Ope_Sap_Dep_Presupuesto.Campo_Dependencia_ID + " = '" + Dependencia_ID + "'";
                }
                if (!String.IsNullOrEmpty(Fte_Financiamiento_ID))
                {
                    Mi_SQL += " AND " + Ope_Sap_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID + " = '" + Fte_Financiamiento_ID + "'";
                }
                if (!String.IsNullOrEmpty(Programa_ID))
                {
                    Mi_SQL += " AND " + Ope_Sap_Dep_Presupuesto.Campo_Proyecto_Programa_ID + " = '" + Programa_ID + "'";
                }
                if (!String.IsNullOrEmpty(Capitulo_ID))
                {
                    Mi_SQL += " AND " + Ope_Sap_Dep_Presupuesto.Campo_Capitulo_ID + " = '" + Capitulo_ID + "'";
                }
                if (!String.IsNullOrEmpty(Partida_ID))
                {
                    Mi_SQL += " AND " + Ope_Sap_Dep_Presupuesto.Campo_Partida_ID + " = '" + Partida_ID + "'";
                }
                Mi_SQL += " ORDER BY " + Ope_Sap_Dep_Presupuesto.Campo_Dependencia_ID + " ASC";
                Dt_Presupuesto = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            return Dt_Presupuesto;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Actualizar_Momentos_Presupuestales
        ///DESCRIPCIÓN: 
        ///PARAMETROS: Dependencia_ID, Fte_Financiamiento_ID, Programa_ID, Capitulo_ID, Partida_ID, Anio
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 15/Nov/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Actualizar_Momentos_Presupuestales
            (String Dependencia_ID, String Fte_Financiamiento_ID, String Programa_ID, String Partida_ID, int Anio,
            String Cargo, String Abono, double Importe)
        {
            int Registros_Afectados = 0;
            String Mi_SQL = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;           
            try
            {
                //Realizar el movimiento presupuestal solicitado
                Mi_SQL = "UPDATE " + Ope_Sap_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + " SET " +
                Cargo + " = " + Cargo + " + " + Importe + ", " +
                Abono + " = " + Abono + " - " + Importe +
                " WHERE " +
                Ope_Sap_Dep_Presupuesto.Campo_Dependencia_ID + " = '" + Dependencia_ID + "' AND " +
                Ope_Sap_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID + " = '" + Fte_Financiamiento_ID + "' AND " +
                Ope_Sap_Dep_Presupuesto.Campo_Proyecto_Programa_ID + " = '" + Programa_ID + "' AND " +
                Ope_Sap_Dep_Presupuesto.Campo_Partida_ID + " = '" + Partida_ID + "' AND " +
                Ope_Sap_Dep_Presupuesto.Campo_Anio_Presupuesto + " = " + Anio;
                //Registros_Afectados = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Cmd.CommandText = Mi_SQL;
                Registros_Afectados = Cmd.ExecuteNonQuery();
                Trans.Commit();
            }
            catch (Exception Ex)
            {
                Trans.Rollback();
                throw new Exception(Ex.ToString());
            }
            return Registros_Afectados;
        }
    }
}
