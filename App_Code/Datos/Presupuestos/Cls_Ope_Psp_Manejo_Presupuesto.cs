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
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;

namespace Presidencia.Manejo_Presupuesto.Datos
{
    public class Cls_Ope_Psp_Manejo_Presupuesto
    {
        #region VARIABLES

        public static String CONSULTA_ANUAL = "ANUAL";
        public static String CONSULTA_MENSUAL = "MENSUAL";
        public static String CONSULTA_ACUMULADO_MENSUAL = "ACUMULADO";

        public static String DISPONIBLE = Ope_Psp_Presupuesto_Aprobado.Campo_Disponible;// "DISPONIBLE";
        public static String PRE_COMPROMETIDO = Ope_Psp_Presupuesto_Aprobado.Campo_Pre_Comprometido;// "PRE_COMPROMETIDO";
        public static String COMPROMETIDO = Ope_Psp_Presupuesto_Aprobado.Campo_Comprometido; //"COMPROMETIDO";
        public static String DEVENGADO = Ope_Psp_Presupuesto_Aprobado.Campo_Devengado; //"DEVENGADO";
        public static String EJERCIDO = Ope_Psp_Presupuesto_Aprobado.Campo_Ejercido;// "EJERCIDO";
        public static String PAGADO = Ope_Psp_Presupuesto_Aprobado.Campo_Pagado; //"PAGADO";

        public static String OPERACION_TRASPASO = "TRASPASO";
        public static String OPERACION_INCREMENTAR = "INCREMENTAR";
        public static String OPERACION_DECREMENTAR = "DECREMENTAR";

        #endregion

        #region METODOS

        public Cls_Ope_Psp_Manejo_Presupuesto()
        {            
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Encontrar_Duplicado
        ///DESCRIPCIÓN: Busca en la tabla si un presupuesta ya existe
        ///PARAMETROS: Dependencia_ID, Fte_Financiamiento_ID, Programa_ID, Capitulo_ID, Partida_ID, Anio
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 15/Nov/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static bool Encontrar_Duplicado(String Dependencia_ID, String Fte_Financiamiento_ID, String Programa_ID, String Capitulo_ID, String Partida_ID, int Anio)
        {
            String Mi_SQL = "";
            bool Duplicado = false;
            try
            {
                DataTable _DataTable = Consultar_Presupuesto_Aprobado(Dependencia_ID, Fte_Financiamiento_ID, Programa_ID, Capitulo_ID, Partida_ID, Anio);
                if (_DataTable != null && _DataTable.Rows.Count > 0)
                {
                    Duplicado = true;
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString() + "[NO SE PUDO VERIFICAR PRESUPUESTO DUPLICADO]");
            }
            return Duplicado;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Presupuesto_Calendarizado
        ///DESCRIPCIÓN: Guarda el presupuesto aprobado
        ///PARAMETROS: DataTable con el presupuesto calendarizado
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 15/Nov/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        ///String Dependencia_ID, String Fte_Financiamiento_ID, String Programa_ID, String Capitulo_ID, String Partida_ID, int Anio, String Estatus)
        public static DataTable Consultar_Presupuesto_Calendarizado(int Anio, String Estatus)
        {
            String Mi_SQL = "";
            DataTable Dt_Presupuesto_Calendarizado = null;
            try
            {
                Mi_SQL = "SELECT * FROM " + Ope_Psp_Calendarizacion_Presu.Tabla_Ope_Psp_Calendarizacion_Presu +
                 " WHERE " + Ope_Psp_Calendarizacion_Presu.Campo_Anio + " = " + Anio;
                if (!String.IsNullOrEmpty(Estatus))
                {
                    Mi_SQL += " AND " + Ope_Psp_Calendarizacion_Presu.Campo_Estatus + " = '" + Estatus + "'"; 
                }
                Mi_SQL += " ORDER BY " + Ope_Psp_Calendarizacion_Presu.Campo_Dependencia_ID + " ASC"; 
                Dt_Presupuesto_Calendarizado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString() + "[NO SE PUDO CONSULTAR PRESUPUESTO CALENDARIZADO]");
            }
            return Dt_Presupuesto_Calendarizado;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Guardar_Presupuesto_Aprobado
        ///DESCRIPCIÓN: Guarda el presupuesto aprobado
        ///PARAMETROS: DataTable con el presupuesto calendarizado
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 15/Nov/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Guardar_Presupuesto_Aprobado(DataTable Dt_Presupuesto)
        {
            int Registros_Afectados = 0;
            int No_Ajuste = 0;
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
                foreach (DataRow Dr_Presupuesto in Dt_Presupuesto.Rows)
                {
                    Mi_SQL = "INSERT INTO " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado +
                    "(" +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Dependencia_ID + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Proyecto_Programa_ID + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Capitulo_ID + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Partida_ID + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Anio + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Enero + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Febrero + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Marzo + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Abril + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Mayo + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Junio + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Julio + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Agosto + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Septiembre + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Octubre + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Noviembre + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Diciembre + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Total + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Usuario_Creo + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Fecha_Creo +
                    ") VALUES (" +
                    "'" + Dr_Presupuesto[Ope_Psp_Presupuesto_Aprobado.Campo_Dependencia_ID].ToString() + "'," +
                    "'" + Dr_Presupuesto[Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID].ToString() + "'," +
                    "'" + Dr_Presupuesto[Ope_Psp_Presupuesto_Aprobado.Campo_Proyecto_Programa_ID].ToString() + "'," +
                    "'" + Dr_Presupuesto[Ope_Psp_Presupuesto_Aprobado.Campo_Capitulo_ID].ToString() + "'," +
                    "'" + Dr_Presupuesto[Ope_Psp_Presupuesto_Aprobado.Campo_Partida_ID].ToString() + "'," +
                    Dr_Presupuesto[Ope_Psp_Presupuesto_Aprobado.Campo_Anio].ToString() + "," +
                    Dr_Presupuesto[Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Enero].ToString() + "," +
                    Dr_Presupuesto[Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Febrero].ToString() + "," +
                    Dr_Presupuesto[Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Marzo].ToString() + "," +
                    Dr_Presupuesto[Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Abril].ToString() + "," +
                    Dr_Presupuesto[Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Mayo].ToString() + "," +
                    Dr_Presupuesto[Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Junio].ToString() + "," +
                    Dr_Presupuesto[Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Julio].ToString() + "," +
                    Dr_Presupuesto[Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Agosto].ToString() + "," +
                    Dr_Presupuesto[Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Septiembre].ToString() + "," +
                    Dr_Presupuesto[Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Octubre].ToString() + "," +
                    Dr_Presupuesto[Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Noviembre].ToString() + "," +
                    Dr_Presupuesto[Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Diciembre].ToString() + "," +
                    Dr_Presupuesto[Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Total].ToString() + "," +
                    "'" + Cls_Sessiones.Nombre_Empleado + "'," +
                    "SYSDATE)";
                    if (!Encontrar_Duplicado(
                            Dr_Presupuesto[Ope_Psp_Presupuesto_Aprobado.Campo_Dependencia_ID].ToString(),
                            Dr_Presupuesto[Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID].ToString(),
                            Dr_Presupuesto[Ope_Psp_Presupuesto_Aprobado.Campo_Proyecto_Programa_ID].ToString(),
                            Dr_Presupuesto[Ope_Psp_Presupuesto_Aprobado.Campo_Capitulo_ID].ToString(),
                            Dr_Presupuesto[Ope_Psp_Presupuesto_Aprobado.Campo_Partida_ID].ToString(),
                            int.Parse( Dr_Presupuesto[Ope_Psp_Presupuesto_Aprobado.Campo_Anio].ToString())
                        ))
                    {
                        Cmd.CommandText = Mi_SQL;
                        Registros_Afectados += Cmd.ExecuteNonQuery();
                    }
                }
                Trans.Commit();
            }
            catch (Exception Ex)
            {
                Trans.Rollback();               
                throw new Exception(Ex.ToString());
            }
            finally
            {
                Cn.Close();
            }
            return Registros_Afectados;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Guardar_Presupuesto_Aprobado
        ///DESCRIPCIÓN: Guarda el presupuesto aprobado solo de una partida
        ///PARAMETROS: DataTable con el presupuesto calendarizado
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 15/Nov/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Guardar_Presupuesto_Aprobado(
            String Dependencia_ID, String Fte_Financiamiento_ID, String Programa_ID, String Capitulo_ID, String Partida_ID, int Anio,
            double Importe_Enero, double Importe_Febrero, double Importe_Marzo, double Importe_Abril,
            double Importe_Mayo, double Importe_Junio, double Importe_Julio, double Importe_Agosto,
            double Importe_Septiembre, double Importe_Octubre, double Importe_Noviembre, double Importe_Diciembre, double Importe_Total
            )
        {
            int Registros_Afectados = 0;
            int No_Ajuste = 0;
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
                    Mi_SQL = "INSERT INTO " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado +
                    "(" +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Dependencia_ID + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Proyecto_Programa_ID + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Capitulo_ID + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Partida_ID + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Anio + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Enero + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Febrero + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Marzo + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Abril + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Mayo + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Junio + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Julio + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Agosto + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Septiembre + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Octubre + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Noviembre + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Diciembre + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Total + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Usuario_Creo + "," +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Fecha_Creo +
                    ") VALUES (" +
                    "'" + Dependencia_ID + "'," +
                    "'" + Fte_Financiamiento_ID + "'," +
                    "'" + Programa_ID + "'," +
                    "'" + Capitulo_ID + "'," +
                    "'" + Partida_ID + "'," +
                    Anio + "," +
                    Importe_Enero + "," +
                    Importe_Febrero + "," +
                    Importe_Marzo + "," +
                    Importe_Abril + "," +
                    Importe_Mayo + "," +
                    Importe_Junio + "," +
                    Importe_Julio + "," +
                    Importe_Agosto + "," +
                    Importe_Septiembre + "," +
                    Importe_Octubre + "," +
                    Importe_Noviembre + "," +
                    Importe_Diciembre + "," +
                    Importe_Total + "," +
                    "'" + Cls_Sessiones.Nombre_Empleado + "'," +
                    "SYSDATE";
                    if (!Encontrar_Duplicado(
                            Dependencia_ID,
                            Fte_Financiamiento_ID,
                            Programa_ID,
                            Capitulo_ID,
                            Partida_ID,
                            Anio)
                        )
                    {
                        Cmd.CommandText = Mi_SQL;
                        Registros_Afectados += Cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        throw new Exception("LA PARTIDA PRESUPUESTAL YA SE ENCUENTRA REGISTRADA ");
                    }
                
                Trans.Commit();
            }
            catch (Exception Ex)
            {
                Trans.Rollback();
                throw new Exception(Ex.ToString());
            }
            finally
            {
                Cn.Close();
            }
            return Registros_Afectados;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Presupuesto_Aprobado
        ///DESCRIPCIÓN: Elimina el presupuesto de un año
        ///PARAMETROS: Año a eliminar
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 15/Nov/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Eliminar_Presupuesto_Aprobado(int Anio)
        {
            String Mi_SQL = "";
            int Registros_Afectados = 0;

            try
            {
                Mi_SQL = "DELETE " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado +
                " WHERE " + Ope_Psp_Presupuesto_Aprobado.Campo_Anio + " = " + Anio;
                Registros_Afectados = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            return Registros_Afectados;
        }        

        //
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Presupuesto
        ///DESCRIPCIÓN: Retorna DataTable con todos los campos de la tabla de una partida
        ///PARAMETROS: Dependencia_ID, Fte_Financiamiento_ID, Programa_ID, Capitulo_ID, Partida_ID, Anio
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 15/Nov/2011
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
                Mi_SQL = "SELECT * FROM " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado +
                " WHERE " + Ope_Psp_Presupuesto_Aprobado.Campo_Anio + " = " + Anio;
                if (!String.IsNullOrEmpty(Dependencia_ID))
                {
                    Mi_SQL += " AND " + Ope_Psp_Presupuesto_Aprobado.Campo_Dependencia_ID + " = '" + Dependencia_ID + "'";
                }
                if (!String.IsNullOrEmpty(Fte_Financiamiento_ID))
                {
                    Mi_SQL += " AND " + Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID + " = '" + Fte_Financiamiento_ID + "'";
                }
                if (!String.IsNullOrEmpty(Programa_ID))
                {
                    Mi_SQL += " AND " + Ope_Psp_Presupuesto_Aprobado.Campo_Proyecto_Programa_ID + " = '" + Programa_ID + "'";
                }
                if (!String.IsNullOrEmpty(Capitulo_ID))
                {
                    Mi_SQL += " AND " + Ope_Psp_Presupuesto_Aprobado.Campo_Capitulo_ID + " = '" + Capitulo_ID + "'";
                }
                if (!String.IsNullOrEmpty(Partida_ID))
                {
                    Mi_SQL += " AND " + Ope_Psp_Presupuesto_Aprobado.Campo_Partida_ID + " = '" + Partida_ID + "'";
                }
                Mi_SQL += " ORDER BY " + Ope_Psp_Presupuesto_Aprobado.Campo_Dependencia_ID + " ASC";               
                Dt_Presupuesto = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            return Dt_Presupuesto;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Importes_Presupuesto_Aprobado
        ///DESCRIPCIÓN: Se pueden consultar APROBADO, REDUCCION, AMPLIACION, 
        ///PARAMETROS: Dependencia_ID, Fte_Financiamiento_ID, Programa_ID, Capitulo_ID, Partida_ID, Anio
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 15/Nov/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static double Consultar_Importes_Presupuesto_Aprobado(String Dependencia_ID, String Fte_Financiamiento_ID, String Programa_ID, String Capitulo_ID, String Partida_ID, int Anio, String Campo)
        {
            String Mi_SQL = "";
            double Importe = 0;
            if (Campo == DISPONIBLE || Campo == PRE_COMPROMETIDO || Campo == COMPROMETIDO || Campo == DEVENGADO || Campo == EJERCIDO || Campo == PAGADO)
            {
                try
                {
                    Mi_SQL = "SELECT " + Campo + " FROM " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado +
                    " WHERE " + Ope_Psp_Presupuesto_Aprobado.Campo_Anio + " = " + Anio;
                    if (!String.IsNullOrEmpty(Dependencia_ID))
                    {
                        Mi_SQL += " AND " + Ope_Psp_Presupuesto_Aprobado.Campo_Dependencia_ID + " = '" + Dependencia_ID + "'";
                    }
                    if (!String.IsNullOrEmpty(Fte_Financiamiento_ID))
                    {
                        Mi_SQL += " AND " + Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID + " = '" + Fte_Financiamiento_ID + "'";
                    }
                    if (!String.IsNullOrEmpty(Programa_ID))
                    {
                        Mi_SQL += " AND " + Ope_Psp_Presupuesto_Aprobado.Campo_Proyecto_Programa_ID + " = '" + Programa_ID + "'";
                    }
                    if (!String.IsNullOrEmpty(Capitulo_ID))
                    {
                        Mi_SQL += " AND " + Ope_Psp_Presupuesto_Aprobado.Campo_Capitulo_ID + " = '" + Capitulo_ID + "'";
                    }
                    if (!String.IsNullOrEmpty(Partida_ID))
                    {
                        Mi_SQL += " AND " + Ope_Psp_Presupuesto_Aprobado.Campo_Partida_ID + " = '" + Partida_ID + "'";
                    }
                    Mi_SQL += " ORDER BY " + Ope_Psp_Presupuesto_Aprobado.Campo_Dependencia_ID + " ASC";
                    Importe = Convert.ToDouble( OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL));
                }
                catch (Exception Ex)
                {
                    throw new Exception(Ex.ToString() + "[NO SE PUDO OBTENER EL IMPORTE SOLICITADO, VERIFIQUE CON SU ADMINISTRADOR]");
                }
            }
            else
            {
                throw new Exception("EL VALOR DEL PARAMETRO CAMPO NO ES VALIDO");
            }
            return Importe;
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
            try
            {
                    //Realizar el movimiento presupuestal solicitado
                    Mi_SQL = "UPDATE " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + " SET " +
                    Cargo + " = " + Cargo + " + " + Importe + ", " +
                    Abono + " = " + Abono + " - " + Importe +
                    " WHERE " +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Dependencia_ID + " = '" + Dependencia_ID + "' AND " +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID + " = '" + Fte_Financiamiento_ID + "' AND " +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Proyecto_Programa_ID + " = '" + Programa_ID + "' AND " +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Partida_ID + " = '" + Partida_ID + "' AND " +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Anio + " = " + Anio;  
                    Registros_Afectados = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch(Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            return Registros_Afectados;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Actualizar_Momentos_Presupuestales
        ///DESCRIPCIÓN: 
        ///PARAMETROS: No_Reserva
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 15/Nov/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Actualizar_Momentos_Presupuestales(String No_Reserva, String Cargo, String Abono, double Importe)
        {
            int Registros_Afectados = 0;
            DataTable Dt_Codigo_Programatico = null;
            String Mi_SQL = "";
            try
            {
                Dt_Codigo_Programatico = Consultar_Reserva(No_Reserva);
                if (Dt_Codigo_Programatico != null && Dt_Codigo_Programatico.Rows.Count > 0)
                {
                    //Realizar el movimiento presupuestal solicitado
                    Mi_SQL = "UPDATE " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + " SET " +
                    Cargo + " = " + Cargo + " + " + Importe + ", " +
                    Abono + " = " + Abono + " - " + Importe +
                    " WHERE " +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Dependencia_ID + " = '" + Dt_Codigo_Programatico.Rows[0]["DEPENDENCIA_ID"].ToString() + "' AND " +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID + " = '" + Dt_Codigo_Programatico.Rows[0]["FTE_FINANCIAMIENTO_ID"].ToString() + "' AND " +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Proyecto_Programa_ID + " = '" + Dt_Codigo_Programatico.Rows[0]["PROYECTO_PROGRAMA_ID"].ToString() + "' AND " +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Partida_ID + " = '" + Dt_Codigo_Programatico.Rows[0]["PARTIDA_ID"].ToString() + "' AND " +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Anio + " = " + Dt_Codigo_Programatico.Rows[0]["ANIO"].ToString() + "";
                    Registros_Afectados = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);                                     
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            return Registros_Afectados;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Traspaso_Presupuestal
        ///DESCRIPCIÓN: Se realizan traspasos de presupuesto 
        ///PARAMETROS: codigo origen y destino + importe
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 15/Nov/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Traspaso_Presupuestal
            (String Origen_Dependencia_ID, String Origen_Fte_Financiamiento_ID, String Origen_Programa_ID, String Origen_Partida_ID, int Origen_Anio,
             String Destino_Dependencia_ID, String Destino_Fte_Financiamiento_ID, String Destino_Programa_ID, String Destino_Partida_ID, int Destino_Anio,
             double Importe)
        {
            int Registros_Afectados = 0;
            String Mi_SQL = "";
            try
            {
                //Realizar el movimiento presupuestal solicitado para la partida ORIGEN
                Mi_SQL = "UPDATE " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + " SET " +
                Ope_Psp_Presupuesto_Aprobado.Campo_Reduccion + " = " + Ope_Psp_Presupuesto_Aprobado.Campo_Reduccion + " + " + Importe + ", " +
                Ope_Psp_Presupuesto_Aprobado.Campo_Modificado + " = " + 
                    Ope_Psp_Presupuesto_Aprobado.Campo_Aprobado + " + " + 
                    Ope_Psp_Presupuesto_Aprobado.Campo_Ampliacion +  " - " + 
                    Ope_Psp_Presupuesto_Aprobado.Campo_Reduccion + ", " +
                Ope_Psp_Presupuesto_Aprobado.Campo_Disponible + " = " + Ope_Psp_Presupuesto_Aprobado.Campo_Disponible + " - " + Importe +  
                " WHERE " +
                Ope_Psp_Presupuesto_Aprobado.Campo_Dependencia_ID + " = '" + Origen_Dependencia_ID + "' AND " +
                Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID + " = '" + Origen_Fte_Financiamiento_ID + "' AND " +
                Ope_Psp_Presupuesto_Aprobado.Campo_Proyecto_Programa_ID + " = '" + Origen_Programa_ID + "' AND " +
                Ope_Psp_Presupuesto_Aprobado.Campo_Partida_ID + " = '" + Origen_Partida_ID + "' AND " +
                Ope_Psp_Presupuesto_Aprobado.Campo_Anio + " = " + Origen_Anio;
                Registros_Afectados += OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                //Realizar el movimiento presupuestal solicitado para la partida DESTINO
                Mi_SQL = "UPDATE " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + " SET " +
                Ope_Psp_Presupuesto_Aprobado.Campo_Ampliacion + " = " + Ope_Psp_Presupuesto_Aprobado.Campo_Ampliacion + " + " + Importe + ", " +
                Ope_Psp_Presupuesto_Aprobado.Campo_Modificado + " = " +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Aprobado + " + " +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Ampliacion + " - " +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Reduccion + ", " +
                Ope_Psp_Presupuesto_Aprobado.Campo_Disponible + " = " + Ope_Psp_Presupuesto_Aprobado.Campo_Disponible + " + " + Importe +
                " WHERE " +
                Ope_Psp_Presupuesto_Aprobado.Campo_Dependencia_ID + " = '" + Destino_Dependencia_ID + "' AND " +
                Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID + " = '" + Destino_Fte_Financiamiento_ID + "' AND " +
                Ope_Psp_Presupuesto_Aprobado.Campo_Proyecto_Programa_ID + " = '" + Destino_Programa_ID + "' AND " +
                Ope_Psp_Presupuesto_Aprobado.Campo_Partida_ID + " = '" + Destino_Partida_ID + "' AND " +
                Ope_Psp_Presupuesto_Aprobado.Campo_Anio + " = " + Destino_Anio;
                Registros_Afectados += OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            return Registros_Afectados;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Incremento_Presupuestal
        ///DESCRIPCIÓN: Se realizan traspasos de presupuesto 
        ///PARAMETROS: codigo origen y destino + importe
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 15/Nov/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Incremento_Presupuestal
            (String Dependencia_ID, String Fte_Financiamiento_ID, String Programa_ID, String Partida_ID, int Anio, double Importe)
        {
            int Registros_Afectados = 0;
            String Mi_SQL = "";
            try
            {
                //Realizar el movimiento presupuestal solicitado para la partida 
                Mi_SQL = "UPDATE " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + " SET " +
                Ope_Psp_Presupuesto_Aprobado.Campo_Ampliacion + " = " + Ope_Psp_Presupuesto_Aprobado.Campo_Ampliacion + " + " + Importe + ", " +
                Ope_Psp_Presupuesto_Aprobado.Campo_Modificado + " = " +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Aprobado + " + " +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Ampliacion + " - " +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Reduccion + ", " +
                Ope_Psp_Presupuesto_Aprobado.Campo_Disponible + " = " + Ope_Psp_Presupuesto_Aprobado.Campo_Disponible + " + " + Importe +
                " WHERE " +
                Ope_Psp_Presupuesto_Aprobado.Campo_Dependencia_ID + " = '" + Dependencia_ID + "' AND " +
                Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID + " = '" + Fte_Financiamiento_ID + "' AND " +
                Ope_Psp_Presupuesto_Aprobado.Campo_Proyecto_Programa_ID + " = '" + Programa_ID + "' AND " +
                Ope_Psp_Presupuesto_Aprobado.Campo_Partida_ID + " = '" + Partida_ID + "' AND " +
                Ope_Psp_Presupuesto_Aprobado.Campo_Anio + " = " + Anio;
                Registros_Afectados += OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            return Registros_Afectados;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Reduccion_Presupuestal
        ///DESCRIPCIÓN: Se realizan traspasos de presupuesto 
        ///PARAMETROS: codigo origen y destino + importe
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 15/Nov/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Reduccion_Presupuestal
            (String Dependencia_ID, String Fte_Financiamiento_ID, String Programa_ID, String Partida_ID, int Anio, double Importe)
        {
            int Registros_Afectados = 0;
            String Mi_SQL = "";
            try
            {
                //Realizar el movimiento presupuestal solicitado para la partida 
                Mi_SQL = "UPDATE " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + " SET " +
                Ope_Psp_Presupuesto_Aprobado.Campo_Reduccion + " = " + Ope_Psp_Presupuesto_Aprobado.Campo_Reduccion + " + " + Importe + ", " +
                Ope_Psp_Presupuesto_Aprobado.Campo_Modificado + " = " +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Aprobado + " + " +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Ampliacion + " - " +
                    Ope_Psp_Presupuesto_Aprobado.Campo_Reduccion + ", " +
                Ope_Psp_Presupuesto_Aprobado.Campo_Disponible + " = " + Ope_Psp_Presupuesto_Aprobado.Campo_Disponible + " - " + Importe +
                " WHERE " +
                Ope_Psp_Presupuesto_Aprobado.Campo_Dependencia_ID + " = '" + Dependencia_ID + "' AND " +
                Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID + " = '" + Fte_Financiamiento_ID + "' AND " +
                Ope_Psp_Presupuesto_Aprobado.Campo_Proyecto_Programa_ID + " = '" + Programa_ID + "' AND " +
                Ope_Psp_Presupuesto_Aprobado.Campo_Partida_ID + " = '" + Partida_ID + "' AND " +
                Ope_Psp_Presupuesto_Aprobado.Campo_Anio + " = " + Anio;
                Registros_Afectados += OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            return Registros_Afectados;
        }

//##########################################################

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Inicializar_Presupuesto_Mensual
        ///DESCRIPCIÓN: Metodo que Inicializa el disponible segun sea 
        ///PARAMETROS: String No_Reserva
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 16/Nov/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Inicializar_Presupuesto_Mensual()
        {
            //Verificamos Si es Anual o Mensual
            String Mi_SQL = "";
            DataTable Dt_Tipo_Consulta = null;
            String Tipo_Consulta = "";
            //Consultamos el tipo de presupuesto que se aplicara en el año en curso, puede ser MENSUAL, ANUAL o ACUMULADO
            Mi_SQL = "SELECT * FROM " + Cat_Parametros_Ejercer_Psp.Tabla_Cat_Parametros_Ejercer_Psp;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Parametros_Ejercer_Psp.Campo_Anio;
            Mi_SQL = Mi_SQL + "=EXTRACT(YEAR FROM SYSDATE)";
            Dt_Tipo_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            if (Dt_Tipo_Consulta != null)
                Tipo_Consulta = Dt_Tipo_Consulta.Rows[0][Cat_Parametros_Ejercer_Psp.Campo_Tipo_De_Consulta].ToString().Trim();
            Mi_SQL = "";
            //Obtenemos la Fecha Actual
            DateTime Fecha_Actual = DateTime.Now;
            //Obtenemos el mes Actual
            int Mes_Actual = int.Parse(Fecha_Actual.Month.ToString());
            switch (Tipo_Consulta)
            {
                case "MENSUAL":
                    //Actualizamos el presupuesto

                    //Realizamos la Actualizacion del Disponible de acuerdo al mes en el que se encuentre
                    switch (Mes_Actual)
                    {
                        case 1:
                            //MES ENERO
                            Actualizar_Disponible_Mensual(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Enero);
                            break;
                        case 2:
                            //MES FEBRERO
                            Actualizar_Disponible_Mensual(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Febrero);
                            break;
                        case 3:
                            //MES MARZO
                            Actualizar_Disponible_Mensual(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Marzo);
                            break;
                        case 4:
                            //MES ABRIL
                            Actualizar_Disponible_Mensual(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Abril);
                            break;
                        case 5:
                            //MES MAYO
                            Actualizar_Disponible_Mensual(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Mayo);
                            break;
                        case 6:
                            //MES JUNIO
                            Actualizar_Disponible_Mensual(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Junio);
                            break;
                        case 7:
                            //MES JULIO
                            Actualizar_Disponible_Mensual(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Julio);
                            break;
                        case 8:
                            //MES AGOSTO
                            Actualizar_Disponible_Mensual(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Agosto);
                            break;
                        case 9:
                            //MES SEPTIEMBRE
                            Actualizar_Disponible_Mensual(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Septiembre);
                            break;
                        case 10:
                            //MES OCTUBRE
                            Actualizar_Disponible_Mensual(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Octubre);
                            break;
                        case 11:
                            //MES NOVIEMBRE
                            Actualizar_Disponible_Mensual(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Noviembre);
                            break;
                        case 12:
                            //MES DICIEMBRE
                            Actualizar_Disponible_Mensual(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Diciembre);
                            break;
                    }



                    break;
                case "ACUMULADO":

                    //Realizamos la Actualizacion del Disponible de acuerdo al mes en el que se encuentre
                    switch (Mes_Actual)
                    {
                        case 1:
                            //MES ENERO
                            //Este no se actualiza ya que automaticamente se asigna el valor al seleccionar el tipo de concepto 
                            //que se aplicara ya sea Anual, Mensual o Acumulado

                            break;
                        case 2:
                            //MES FEBRERO
                            Actualizar_Disponible_Acumulado(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Febrero);
                            break;
                        case 3:
                            //MES MARZO
                            Actualizar_Disponible_Acumulado(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Marzo);
                            break;
                        case 4:
                            //MES ABRIL
                            Actualizar_Disponible_Acumulado(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Abril);
                            break;
                        case 5:
                            //MES MAYO
                            Actualizar_Disponible_Acumulado(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Mayo);
                            break;
                        case 6:
                            //MES JUNIO
                            Actualizar_Disponible_Acumulado(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Junio);
                            break;
                        case 7:
                            //MES JULIO
                            Actualizar_Disponible_Acumulado(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Julio);
                            break;
                        case 8:
                            //MES AGOSTO
                            Actualizar_Disponible_Acumulado(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Agosto);
                            break;
                        case 9:
                            //MES SEPTIEMBRE
                            Actualizar_Disponible_Acumulado(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Septiembre);
                            break;
                        case 10:
                            //MES OCTUBRE
                            Actualizar_Disponible_Acumulado(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Octubre);
                            break;
                        case 11:
                            //MES NOVIEMBRE
                            Actualizar_Disponible_Acumulado(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Noviembre);
                            break;
                        case 12:
                            //MES DICIEMBRE
                            Actualizar_Disponible_Acumulado(Ope_Psp_Presupuesto_Aprobado.Campo_Importe_Diciembre);
                            break;
                    }

                    break;
            }



        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Actualizar_Disponible
        ///DESCRIPCIÓN: Metodo que Inicializa el disponible segun sea el mes actual 
        ///PARAMETROS: String Campo:Nombre del campo al que se le asignara el disponible.
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 16/Nov/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static void Actualizar_Disponible_Mensual(String Campo)
        {
            String Mi_SQL = "";
            //aCTUALIZAMOS EL DISPONIBLE EN CASO DE QUE SE APLIQUE LA AFECTACION PRESUPUESTAL MENSUAL
            Mi_SQL = "UPDATE " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado;
            Mi_SQL = Mi_SQL + " SET " + Ope_Psp_Presupuesto_Aprobado.Campo_Disponible;
            Mi_SQL = Mi_SQL + "=" + Campo;
            Mi_SQL = Mi_SQL + ", ACTUALIZADO ='SI'";
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Psp_Presupuesto_Aprobado.Campo_Anio;
            Mi_SQL = Mi_SQL + "=EXTRACT(YEAR FROM SYSDATE)";
            Mi_SQL = Mi_SQL + " AND EXTRACT(DAY FROM SYSDATE)=1";
            Mi_SQL = Mi_SQL + " AND ACTUALIZADO='NO'";
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            //ACTUALIZAMOS EL CAMPO DE ACTUALIZADO A NO SOLO CUANOD ES EL DIA 28 DE CADA MES PARA QUE NOS 
            //DEJE MODIFICAR AL SIGUIENTE MES EL DISPONIBLE QUE CORRESPONDE AL MES EN CURSO
            Mi_SQL = "UPDATE " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado;
            Mi_SQL = Mi_SQL + " SET ACTUALIZADO ='NO'";
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Psp_Presupuesto_Aprobado.Campo_Anio;
            Mi_SQL = Mi_SQL + "=EXTRACT(YEAR FROM SYSDATE)";
            Mi_SQL = Mi_SQL + " AND EXTRACT(DAY FROM SYSDATE)=28";

            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Actualizar_Disponible
        ///DESCRIPCIÓN: Metodo que Inicializa el disponible segun sea el mes actual 
        ///PARAMETROS: String Campo:Nombre del campo al que se le asignara el disponible.
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 16/Nov/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static void Actualizar_Disponible_Acumulado(String Campo)
        {
            String Mi_SQL = "";
            //aCTUALIZAMOS EL DISPONIBLE EN CASO DE QUE SE APLIQUE LA AFECTACION PRESUPUESTAL MENSUAL
            Mi_SQL = "UPDATE " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado;
            Mi_SQL = Mi_SQL + " SET " + Ope_Psp_Presupuesto_Aprobado.Campo_Disponible;
            Mi_SQL = Mi_SQL + "=" + Ope_Psp_Presupuesto_Aprobado.Campo_Disponible;
            Mi_SQL = Mi_SQL + Campo;
            Mi_SQL = Mi_SQL + ", ACTUALIZADO ='SI'";
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Psp_Presupuesto_Aprobado.Campo_Anio;
            Mi_SQL = Mi_SQL + "=EXTRACT(YEAR FROM SYSDATE)";
            Mi_SQL = Mi_SQL + " AND EXTRACT(DAY FROM SYSDATE)=1";
            Mi_SQL = Mi_SQL + " AND ACTUALIZADO='NO'";

            //ACTUALIZAMOS EL CAMPO DE ACTUALIZADO A NO SOLO CUANOD ES EL DIA 28 DE CADA MES PARA QUE NOS 
            //DEJE MODIFICAR AL SIGUIENTE MES EL DISPONIBLE QUE CORRESPONDE AL MES EN CURSO
            Mi_SQL = "UPDATE " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado;
            Mi_SQL = Mi_SQL + " SET ACTUALIZADO ='NO'";
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Psp_Presupuesto_Aprobado.Campo_Anio;
            Mi_SQL = Mi_SQL + "=EXTRACT(YEAR FROM SYSDATE)";
            Mi_SQL = Mi_SQL + " AND EXTRACT(DAY FROM SYSDATE)=28";

            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Codigo_Programatico_De_Reserva
        ///DESCRIPCIÓN: Metodo que consulta todo el Codigo Programatico de la reserva
        ///PARAMETROS: String No_Reserva
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 15/Nov/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Reserva(String No_Reserva)
        {
            //COnsultar tabla de Reservas
            DataTable Dt_Codigo_Programatico = null;
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "SELECT * FROM " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Psp_Reservas.Campo_No_Reserva;
                Mi_SQL = Mi_SQL + "=" + No_Reserva.Trim();
                Dt_Codigo_Programatico = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {

                throw new Exception(Ex.ToString());
            }
            return Dt_Codigo_Programatico;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Registro_Movimiento_Presupuestal
        ///DESCRIPCIÓN: Metodo que da de algta el movimiento presupuestal
        ///PARAMETROS: String No_Reserva, String Cargo, String Abono, double Importe
        ///CREO: Susana Trigueros Armenta 
        ///FECHA_CREO: 15/Nov/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Registro_Movimiento_Presupuestal(String No_Reserva, String Cargo, String Abono, double Importe, String No_Poliza, String Tipo_Poliza_ID, String Mes_Ano, String Partida)
        {
            int Registros_Guardados = 0;
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "INSERT INTO " + Ope_Psp_Registro_Movimientos.Tabla_Ope_Psp_Registro_Movimientos;
                Mi_SQL = Mi_SQL + " (" + Ope_Psp_Registro_Movimientos.Campo_No_Reserva;
                Mi_SQL = Mi_SQL + ", " + Ope_Psp_Registro_Movimientos.Campo_Cargo;
                Mi_SQL = Mi_SQL + ", " + Ope_Psp_Registro_Movimientos.Campo_Abono;
                Mi_SQL = Mi_SQL + ", " + Ope_Psp_Registro_Movimientos.Campo_Importe;
                Mi_SQL = Mi_SQL + ", " + Ope_Psp_Registro_Movimientos.Campo_Fecha;
                Mi_SQL = Mi_SQL + ", " + Ope_Psp_Registro_Movimientos.Campo_Usuario;
                Mi_SQL = Mi_SQL + ", " + Ope_Psp_Registro_Movimientos.Campo_Usuario_Creo;
                Mi_SQL = Mi_SQL + ", " + Ope_Psp_Registro_Movimientos.Campo_Fecha_Creo;
                if (No_Poliza.Trim() != String.Empty)
                    Mi_SQL = Mi_SQL + ", " + Ope_Psp_Registro_Movimientos.Campo_No_Poliza;
                if (Tipo_Poliza_ID.Trim() != String.Empty)
                    Mi_SQL = Mi_SQL + ", " + Ope_Psp_Registro_Movimientos.Campo_Tipo_Poliza_ID;
                if (Mes_Ano.Trim() != String.Empty)
                    Mi_SQL = Mi_SQL + ", " + Ope_Psp_Registro_Movimientos.Campo_Mes_Ano;
                if (Partida.Trim() != String.Empty)
                    Mi_SQL = Mi_SQL + ", " + Ope_Psp_Registro_Movimientos.Campo_Partida;

    
                Mi_SQL = Mi_SQL + ") VALUES(";
                Mi_SQL = Mi_SQL + No_Reserva;
                Mi_SQL = Mi_SQL + ",'" + Cargo + "'";
                Mi_SQL = Mi_SQL + ",'" + Abono + "'";
                Mi_SQL = Mi_SQL + ",'" + Importe.ToString() + "'";
                Mi_SQL = Mi_SQL + ",SYSDATE";
                Mi_SQL = Mi_SQL + ",'" + Cls_Sessiones.Nombre_Empleado + "'";
                Mi_SQL = Mi_SQL + ",'" + Cls_Sessiones.Nombre_Empleado + "'";
                Mi_SQL = Mi_SQL + ",SYSDATE";
                if (No_Poliza.Trim() != String.Empty)
                    Mi_SQL = Mi_SQL + ", '" + No_Poliza.Trim() + "'";
                if (Tipo_Poliza_ID.Trim() != String.Empty)
                    Mi_SQL = Mi_SQL + ", '" + Tipo_Poliza_ID.Trim() + "'";
                if (Mes_Ano.Trim() != String.Empty)
                    Mi_SQL = Mi_SQL + ", '" + Mes_Ano.Trim() + "'";
                if (Partida.Trim() != String.Empty)
                    Mi_SQL = Mi_SQL + "," + Partida.Trim();
                Mi_SQL = Mi_SQL + ")";


                Registros_Guardados = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            return Registros_Guardados;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Crear_Reserva
        ///DESCRIPCIÓN: Metodo que crea la reserva para un proximo gasto 
        ///PARAMETROS: String Dependencia_ID, String Fte_Financiamiento_ID, String Programa_ID, String Partida_ID,String Concepto,String Anio, double Importe
        ///CREO: Susana Trigueros Armenta 
        ///FECHA_CREO: 16/Nov/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Crear_Reserva(String Dependencia_ID, String Fte_Financiamiento_ID, String Programa_ID, String Partida_ID, String Concepto, String Anio, double Importe)
        {
            int No_Reserva = Obtener_Consecutivo(Ope_Psp_Reservas.Campo_No_Reserva, Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas);
            String Mi_SQL = "";

            Mi_SQL = "INSERT INTO " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas;
            Mi_SQL = Mi_SQL + "(" + Ope_Psp_Reservas.Campo_No_Reserva;
            Mi_SQL = Mi_SQL + ", " + Ope_Psp_Reservas.Campo_Concepto;
            Mi_SQL = Mi_SQL + ", " + Ope_Psp_Reservas.Campo_Fecha;
            Mi_SQL = Mi_SQL + ", " + Ope_Psp_Reservas.Campo_Dependencia_ID;
            Mi_SQL = Mi_SQL + ", " + Ope_Psp_Reservas.Campo_Fte_Financimiento_ID;
            Mi_SQL = Mi_SQL + ", " + Ope_Psp_Reservas.Campo_Proyecto_Programa_ID;
            Mi_SQL = Mi_SQL + ", " + Ope_Psp_Reservas.Campo_Partida_ID;
            Mi_SQL = Mi_SQL + ", " + Ope_Psp_Reservas.Campo_Capitulo_ID;
            Mi_SQL = Mi_SQL + ", " + Ope_Psp_Reservas.Campo_Anio;
            Mi_SQL = Mi_SQL + ", " + Ope_Psp_Reservas.Campo_Importe_Inicial;
            Mi_SQL = Mi_SQL + ", " + Ope_Psp_Reservas.Campo_Saldo;
            Mi_SQL = Mi_SQL + ", " + Ope_Psp_Reservas.Campo_Usuario_Creo;
            Mi_SQL = Mi_SQL + ", " + Ope_Psp_Reservas.Campo_Fecha_Creo;
            //Mi_SQL = Mi_SQL + ", " + "BENEFICIARIO";
            Mi_SQL = Mi_SQL + ") VALUES(";
            Mi_SQL = Mi_SQL + No_Reserva;
            Mi_SQL = Mi_SQL + ",'" + Concepto + "'";
            Mi_SQL = Mi_SQL + ",SYSDATE";
            Mi_SQL = Mi_SQL + ",'" + Dependencia_ID.Trim() + "'";
            Mi_SQL = Mi_SQL + ",'" + Fte_Financiamiento_ID.Trim() + "'";
            Mi_SQL = Mi_SQL + ",'" + Programa_ID.Trim() + "'";
            Mi_SQL = Mi_SQL + ",'" + Partida_ID.Trim() + "'";
            Mi_SQL = Mi_SQL + ",(SELECT " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Capitulo_ID;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto;
            Mi_SQL = Mi_SQL + " JOIN " + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas;
            Mi_SQL = Mi_SQL + " ON " + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + "." + Cat_Sap_Partidas_Genericas.Campo_Concepto_ID;
            Mi_SQL = Mi_SQL + "=" + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Concepto_ID;
            Mi_SQL = Mi_SQL + " JOIN " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas;
            Mi_SQL = Mi_SQL + " ON " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID;
            Mi_SQL = Mi_SQL + "=" + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + "." + Cat_Sap_Partidas_Genericas.Campo_Partida_Generica_ID;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID;
            Mi_SQL = Mi_SQL + "='" + Partida_ID.Trim() + "')";
            Mi_SQL = Mi_SQL + ", " + Anio;
            Mi_SQL = Mi_SQL + ", " + Importe;
            Mi_SQL = Mi_SQL + ", " + Importe;
            Mi_SQL = Mi_SQL + ",'" + Cls_Sessiones.Nombre_Empleado + "'";
            Mi_SQL = Mi_SQL + ",SYSDATE)";
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return No_Reserva;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Crear_Reserva
        ///DESCRIPCIÓN: Metodo que crea la reserva para un proximo gasto 
        ///PARAMETROS: String Dependencia_ID,String Estatus, String Beneficiario, String Fte_Financiamiento_ID, String P_Programa_ID, String Partida_ID,String Concepto,String Anio, double Importe
        ///CREO: Susana Trigueros Armenta 
        ///FECHA_CREO: 30/Nov/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Crear_Reserva(String Dependencia_ID, String Estatus, String Beneficiario, String Fte_Financiamiento_ID, String Programa_ID, String Partida_ID, String Concepto, String Anio, double Importe)
        {
            int No_Reserva = Obtener_Consecutivo(Ope_Psp_Reservas.Campo_No_Reserva, Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas);
            String Mi_SQL = "";
            Mi_SQL = "INSERT INTO " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas;
            Mi_SQL = Mi_SQL + "(" + Ope_Psp_Reservas.Campo_No_Reserva;
            Mi_SQL = Mi_SQL + ", " + Ope_Psp_Reservas.Campo_Estatus;
            Mi_SQL = Mi_SQL + ", " + Ope_Psp_Reservas.Campo_Beneficiario;
            Mi_SQL = Mi_SQL + ", " + Ope_Psp_Reservas.Campo_Concepto;
            Mi_SQL = Mi_SQL + ", " + Ope_Psp_Reservas.Campo_Fecha;
            Mi_SQL = Mi_SQL + ", " + Ope_Psp_Reservas.Campo_Dependencia_ID;
            Mi_SQL = Mi_SQL + ", " + Ope_Psp_Reservas.Campo_Fte_Financimiento_ID;
            Mi_SQL = Mi_SQL + ", " + Ope_Psp_Reservas.Campo_Proyecto_Programa_ID;
            Mi_SQL = Mi_SQL + ", " + Ope_Psp_Reservas.Campo_Partida_ID;
            Mi_SQL = Mi_SQL + ", " + Ope_Psp_Reservas.Campo_Capitulo_ID;
            Mi_SQL = Mi_SQL + ", " + Ope_Psp_Reservas.Campo_Anio;
            Mi_SQL = Mi_SQL + ", " + Ope_Psp_Reservas.Campo_Importe_Inicial;
            Mi_SQL = Mi_SQL + ", " + Ope_Psp_Reservas.Campo_Saldo;
            Mi_SQL = Mi_SQL + ", " + Ope_Psp_Reservas.Campo_Usuario_Creo;
            Mi_SQL = Mi_SQL + ", " + Ope_Psp_Reservas.Campo_Fecha_Creo;
            Mi_SQL = Mi_SQL + ") VALUES(";
            Mi_SQL = Mi_SQL + No_Reserva;
            Mi_SQL = Mi_SQL + ",'" + Estatus + "'";
            Mi_SQL = Mi_SQL + ",'" + Beneficiario + "'";
            Mi_SQL = Mi_SQL + ",'" + Concepto + "'";
            Mi_SQL = Mi_SQL + ",SYSDATE";
            Mi_SQL = Mi_SQL + ",'" + Dependencia_ID.Trim() + "'";
            Mi_SQL = Mi_SQL + ",'" + Fte_Financiamiento_ID.Trim() + "'";
            Mi_SQL = Mi_SQL + ",'" + Programa_ID.Trim() + "'";
            Mi_SQL = Mi_SQL + ",'" + Partida_ID.Trim() + "'";
            Mi_SQL = Mi_SQL + ",(SELECT " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Capitulo_ID;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto;
            Mi_SQL = Mi_SQL + " JOIN " + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas;
            Mi_SQL = Mi_SQL + " ON " + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + "." + Cat_Sap_Partidas_Genericas.Campo_Concepto_ID;
            Mi_SQL = Mi_SQL + "=" + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Concepto_ID;
            Mi_SQL = Mi_SQL + " JOIN " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas;
            Mi_SQL = Mi_SQL + " ON " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID;
            Mi_SQL = Mi_SQL + "=" + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + "." + Cat_Sap_Partidas_Genericas.Campo_Partida_Generica_ID;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID;
            Mi_SQL = Mi_SQL + "='" + Partida_ID.Trim() + "')";
            Mi_SQL = Mi_SQL + ", " + Anio;
            Mi_SQL = Mi_SQL + ", " + Importe;
            Mi_SQL = Mi_SQL + ", " + Importe;
            Mi_SQL = Mi_SQL + ",'" + Cls_Sessiones.Nombre_Empleado + "'";
            Mi_SQL = Mi_SQL + ",SYSDATE)";
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return No_Reserva;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Consecutivo
        ///DESCRIPCIÓN: Obtiene el numero consecutivo para las tablas ocupadas en esta clase
        ///PARAMETROS: 1.-Campo del cual se obtendra el consecutivo
        ///            2.-Nombre de la tabla
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 10/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Obtener_Consecutivo(String Campo_ID, String Tabla)
        {
            int Consecutivo = 0;
            String Mi_Sql;
            Object Obj; //Obtiene el ID con la cual se guardo los datos en la base de datos
            Mi_Sql = "SELECT NVL(MAX (" + Campo_ID + "),'00000') FROM " + Tabla;
            Obj = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            Consecutivo = (Convert.ToInt32(Obj) + 1);
            return Consecutivo;
        }

        public static DataTable Consultar_Reservas(String Fecha_Inico, String Fecha_Fin, int No_Reserva)
        {
            DataTable Dt_Historial = null;
            String Mi_SQL = "SELECT * FROM " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas;
            Mi_SQL += " WHERE TO_DATE(TO_CHAR(" + Ope_Psp_Reservas.Campo_Fecha + ",'DD-MM-YYYY'))" +
                        " >= '" + Fecha_Inico + "' AND " +
                "TO_DATE(TO_CHAR(" + Ope_Psp_Reservas.Campo_Fecha + ",'DD-MM-YYYY'))" +
                        " <= '" + Fecha_Fin + "'";
            if (No_Reserva > 0)
            {
                Mi_SQL = "SELECT * FROM " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas;
                Mi_SQL += " WHERE " + Ope_Psp_Reservas.Campo_No_Reserva + " = " + No_Reserva;
            }
            Mi_SQL += "ORDER BY " + Ope_Psp_Reservas.Campo_No_Reserva + " DESC";
            try
            {
                Dt_Historial = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            return Dt_Historial;
        }

        public static DataTable Consultar_Reservas_De_Requisicion(String No_Requisicion)
        {
            DataTable Dt_Reserva = null;
            String Mi_SQL = "SELECT " +
            Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + ".*" +
            " FROM " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + ", " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
            " WHERE " +
            Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + "." + Ope_Psp_Reservas.Campo_No_Reserva + " = " +
            Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + ".NUM_RESERVA";
            try
            {
                Dt_Reserva = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];             
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            return Dt_Reserva;
        }

        public static DataTable Consultar_Historial_Reservas(int No_Reserva)
        {
            DataTable Dt_Historial = null;
            String Mi_SQL = "SELECT * FROM " + Ope_Psp_Registro_Movimientos.Tabla_Ope_Psp_Registro_Movimientos;
            if (No_Reserva > 0)
            {
                Mi_SQL += " WHERE " + Ope_Psp_Registro_Movimientos.Campo_No_Reserva + " = " + No_Reserva;
            }
            try
            {
                Dt_Historial = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            return Dt_Historial;
        }
        #endregion
    }
}
