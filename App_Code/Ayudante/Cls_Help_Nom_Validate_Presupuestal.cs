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
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;

namespace Presidencia.Informacion_Presupuestal
{
    public class Cls_Help_Nom_Validate_Presupuestal
    {
        #region (Variables Privadas)
        private String Fte_Financiamiento;
        private String Proyecto_Programa;
        private String Area_Funcional;
        private String Unidad_Responsable;
        private String Partida_Especifica;
        #endregion

        #region (Variables Públicas)
        public string P_Fte_Financiamiento
        {
            get { return Fte_Financiamiento; }
            set { Fte_Financiamiento = value; }
        }

        public string P_Proyecto_Programa
        {
            get { return Proyecto_Programa; }
            set { Proyecto_Programa = value; }
        }

        public string P_Area_Funcional
        {
            get { return Area_Funcional; }
            set { Area_Funcional = value; }
        }

        public string P_Unidad_Responsable
        {
            get { return Unidad_Responsable; }
            set { Unidad_Responsable = value; }
        }

        public string P_Partida_Especifica
        {
            get { return Partida_Especifica; }
            set { Partida_Especifica = value; }
        } 
        #endregion

        #region (Métodos)
        ///************************************************************************************
        /// Nombre Método: Consultar_Disponible
        /// 
        /// Descripción: Método que consulta el presupuesto disponible en la partida y dependencia
        ///              que son pasadas como párametro a este método.
        /// 
        /// Parámetros: UR.- Unidad responsable a la cual se le consultara el presupuesto disponible.
        ///             PE.- Partida especifica a la cuál se le consultara el disponible.
        ///
        /// Usuario creó: Juan Alberto Hernandez Negrete 
        /// Fecha Creó: Marzo/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa modificación:
        ///************************************************************************************
        public Double Consultar_Disponible(String UR, String PE)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Informacion = null;//Variable que almacenara el resultado de la consulta.
            Object Aux = null;//Variable auxiliar. 
            Double Disponible = 0.0;//Variable que almacenara el monto disponible en la unidad responsable y partida consultada.

            try
            {
                Consultar_Programa_Por_Partida(UR, PE);

                Mi_SQL.Append("select ");
                Mi_SQL.Append(" SUM(" + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible + ")");
                Mi_SQL.Append(" from ");
                Mi_SQL.Append(Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto);
                Mi_SQL.Append(" where ");
                //Mi_SQL.Append(Cat_Com_Dep_Presupuesto.Campo_Area_Funcional_ID + "='" + P_Area_Funcional + "'");
                //Mi_SQL.Append(" and ");
                //Mi_SQL.Append(Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID + "='" + P_Proyecto_Programa + "'");
                //Mi_SQL.Append(" and ");
                Mi_SQL.Append(Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + "='" + P_Unidad_Responsable + "'");
                Mi_SQL.Append(" and ");
                Mi_SQL.Append(Cat_Com_Dep_Presupuesto.Campo_Partida_ID + "='" + P_Partida_Especifica + "'");
                Mi_SQL.Append("and ");
                Mi_SQL.Append(Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto + "=" + DateTime.Now.Year);

                Aux = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

                if (Aux != null)
                    if (!Convert.IsDBNull(Aux))
                        Disponible = Convert.ToDouble(Aux);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar el presupuesto disponible. Error: " + ex.Message + "]");
            }
            return Disponible;
        }
        ///************************************************************************************
        /// Nombre Método: Consultar_Comprometido_Sueldos
        /// 
        /// Descripción: Método que consulta el presupuesto disponible en la partida y dependencia
        ///              que son pasadas como párametro a este método.
        /// 
        /// Parámetros: UR.- Unidad responsable a la cual se le consultara el presupuesto disponible.
        ///             PE.- Partida especifica a la cuál se le consultara el disponible.
        ///
        /// Usuario creó: Juan Alberto Hernandez Negrete 
        /// Fecha Creó: Marzo/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa modificación:
        ///************************************************************************************
        public Double Consultar_Comprometido_Sueldos(String UR, String PE)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            Object Aux = null;//Variable auxiliar. 
            Double Disponible = 0.0;//Variable que almacenara el monto disponible en la unidad responsable y partida consultada.

            try
            {
                Consultar_Programa_Por_Partida(UR, PE);

                Mi_SQL.Append("select ");
                Mi_SQL.Append(" SUM(" + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido_Real + ")");
                Mi_SQL.Append(" from ");
                Mi_SQL.Append(Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto);
                Mi_SQL.Append(" where ");
                //Mi_SQL.Append(Cat_Com_Dep_Presupuesto.Campo_Area_Funcional_ID + "='" + P_Area_Funcional + "'");
                //Mi_SQL.Append(" and ");
                //Mi_SQL.Append(Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID + "='" + P_Proyecto_Programa + "'");
                //Mi_SQL.Append(" and ");
                Mi_SQL.Append(Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + "='" + P_Unidad_Responsable + "'");
                Mi_SQL.Append(" and ");
                Mi_SQL.Append(Cat_Com_Dep_Presupuesto.Campo_Partida_ID + "='" + P_Partida_Especifica + "'");
                Mi_SQL.Append("and ");
                Mi_SQL.Append(Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto + "=" + DateTime.Now.Year);

                Aux = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

                if (Aux != null)
                    if (!Convert.IsDBNull(Aux))
                        Disponible = Convert.ToDouble(Aux);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar el presupuesto comprometido. Error: " + ex.Message + "]");
            }
            return Disponible;
        }
        ///************************************************************************************
        /// Nombre Método: Consultar_Programa_Por_Partida
        /// 
        /// Descripción: Método que deacuerdo a la unidad responsable y a la partida consulta
        ///              el programa en el que se encuentra la partida especifica.
        /// 
        /// Parámetros: UR.- Unidad responsable a la cual se le consultara el presupuesto disponible.
        ///             PE.- Partida especifica a la cuál se le consultara el disponible.
        ///
        /// Usuario creó: Juan Alberto Hernández Negrete
        /// Fecha Creó: Marzo/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa modificación:
        ///************************************************************************************
        public void Consultar_Programa_Por_Partida(String UR, String PE)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacena la consulta.
            DataTable Dt_Informacion = null;//Variable que almacena el resultado de la consulta.
            P_Unidad_Responsable = UR;//Variable que almacena el identificador de la unidad responsable.
            P_Partida_Especifica = PE;//Variable que almacena el identificador de la partida especifica.

            try
            {
                Mi_SQL.Append("select ");

                Mi_SQL.Append("(select " + Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID);
                Mi_SQL.Append(" from " + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional);
                Mi_SQL.Append(" where " + Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID + " in ");
                Mi_SQL.Append("(select " + Cat_Dependencias.Campo_Area_Funcional_ID);
                Mi_SQL.Append(" from " + Cat_Dependencias.Tabla_Cat_Dependencias);
                Mi_SQL.Append(" where " + Cat_Dependencias.Campo_Dependencia_ID + "='" + P_Unidad_Responsable +
                              "')) as AREA_FUNCIONAL_ID, ");

                Mi_SQL.Append(Cat_SAP_Det_Prog_Dependencia.Campo_Proyecto_Programa_ID);

                Mi_SQL.Append(" from ");
                Mi_SQL.Append(Cat_SAP_Det_Prog_Dependencia.Tabla_Cat_SAP_Det_Programa_Dependencia);

                Mi_SQL.Append(" where ");
                Mi_SQL.Append(Cat_SAP_Det_Prog_Dependencia.Campo_Dependencia_ID + "='" + P_Unidad_Responsable + "'");
                Mi_SQL.Append(" and ");
                Mi_SQL.Append(Cat_SAP_Det_Prog_Dependencia.Campo_Proyecto_Programa_ID + " in ");
                Mi_SQL.Append("(select " + Cat_Sap_Det_Prog_Partidas.Campo_Det_Proyecto_Programa_ID);
                Mi_SQL.Append(" from " + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas);
                Mi_SQL.Append(" where " + Cat_Sap_Det_Prog_Partidas.Campo_Det_Partida_ID + "='" +
                              P_Partida_Especifica + "')");

                Mi_SQL.Append(" order by " + Cat_SAP_Det_Prog_Dependencia.Campo_Proyecto_Programa_ID + " asc ");

                Dt_Informacion =
                    OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables
                        [0];

                if (Dt_Informacion is DataTable)
                {
                    if (Dt_Informacion.Rows.Count > 0)
                    {
                        foreach (DataRow FILA in Dt_Informacion.Rows)
                        {
                            if (FILA is DataRow)
                            {

                                if (!String.IsNullOrEmpty(FILA["AREA_FUNCIONAL_ID"].ToString()))
                                    P_Area_Funcional = FILA["AREA_FUNCIONAL_ID"].ToString();

                                if (
                                    !String.IsNullOrEmpty(
                                        FILA[Cat_SAP_Det_Prog_Dependencia.Campo_Proyecto_Programa_ID].ToString()))
                                    P_Proyecto_Programa =
                                        FILA[Cat_SAP_Det_Prog_Dependencia.Campo_Proyecto_Programa_ID].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(
                    "Error al consultar el el programa que contiene la partida especifica y la UR pasada como parámetro. Error: [" +
                    Ex.Message + "]");
            }
        }
        ///************************************************************************************
        /// Nombre Método: Consultar_Partidas
        /// 
        /// Descripción: Método que consulta las partidas que actualmente se encuentran en el 
        ///              sistema.
        /// 
        /// Parámetros: No Aplica.
        ///
        /// Usuario creó: Juan Alberto Hernández Negrete
        /// Fecha Creó: Marzo/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa modificación:
        ///************************************************************************************
        public static DataTable Consultar_Partidas()
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Partidas = null;//Variable que almacenara el listado de partidas que se encuentran actualmente en el sistema.

            try
            {
                Mi_SQL.Append("select (" + Cat_Sap_Partidas_Especificas.Campo_Clave + " || '- ' || " +
                              Cat_Sap_Partidas_Especificas.Campo_Nombre + ") as NOMBRE, ");
                Mi_SQL.Append(Cat_Sap_Partidas_Especificas.Campo_Partida_ID);
                Mi_SQL.Append(" from " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas);
                Mi_SQL.Append(" where " + Cat_Sap_Partidas_Especificas.Campo_Clave + " between 1000 and 2000");

                Dt_Partidas =
                    OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables
                        [0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al cosultas las partidas. Error: [" + Ex.Message + "]");
            }
            return Dt_Partidas;
        }
        ///************************************************************************************
        /// Nombre Método: Consultar_Partidas_General
        /// 
        /// Descripción: Método que consulta las partidas generales esto para la excepción
        ///              de algunas partidas de nómina ue no pertencén al rango de las 1000. 
        /// 
        /// Parámetros:
        ///
        /// Usuario creó: Juan Alberto Hernández Negrete
        /// Fecha Creó: Marzo/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa modificación:
        ///************************************************************************************
        public static  DataTable Consultar_Partidas_General()
        {
            StringBuilder Mi_SQL = new StringBuilder();//variable que almacenara la consulta.
            DataTable Dt_Resultado = null;//Variable que almacenara el resultado de la consulta.

            try
            {
                Mi_SQL.Append("select (" + Cat_Sap_Partidas_Especificas.Campo_Clave + " || '- ' || " +
                              Cat_Sap_Partidas_Especificas.Campo_Nombre + ") as NOMBRE, ");
                Mi_SQL.Append(Cat_Sap_Partidas_Especificas.Campo_Partida_ID);
                Mi_SQL.Append(" from ");
                Mi_SQL.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + " order by " + Cat_Sap_Partidas_Especificas.Campo_Clave + " desc ");

                Dt_Resultado =
                    OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables
                        [0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error . Error: [" + ex.Message + "]");
            }
            return Dt_Resultado;
        }
        /// *******************************************************************************************
        /// Nombre: Consultar_Total_Nomina
        /// 
        /// Descripcion: Consulta los totales de nómina por nomina y por periodo.
        /// 
        /// Parámetros: Nomina_ID.- Año de la nomina a consultar el total. 
        ///             Periodo.- Periodo a consultar el total.
        /// 
        /// Ususario Creó: Juan Alberto Hernandez Negrete.
        /// Fecha Creó: Marzo/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa Modificación
        /// *******************************************************************************************
        public static DataTable Consultar_Total_Nomina(String Nomina_ID, String Periodo)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable que almacenara la consulta.
            DataTable Dt_Totales_Nomina = null; //Variable que almacenara el resultado de la consulta.

            try
            {
                Mi_SQL.Append("select ");
                Mi_SQL.Append(Ope_Nom_Totales_Nomina.Tabla_Ope_Nom_Totales_Nomina + ".* ");
                Mi_SQL.Append(" from ");
                Mi_SQL.Append(Ope_Nom_Totales_Nomina.Tabla_Ope_Nom_Totales_Nomina);
                Mi_SQL.Append(" where ");
                Mi_SQL.Append(Ope_Nom_Totales_Nomina.Campo_Nomina_ID + "='" + Nomina_ID + "'");
                Mi_SQL.Append(" and ");
                Mi_SQL.Append(Ope_Nom_Totales_Nomina.Campo_No_Nomina + "='" + Periodo + "'");

                Dt_Totales_Nomina =
                    OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables
                        [0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar el total de la nomina consultada. Error: [" + Ex.Message + "]");
            }
            return Dt_Totales_Nomina;
        }
        ///************************************************************************************
        /// Nombre Método: Consultar_Montos_Conceptos_Por_Unidad_Responsable
        /// 
        /// Descripción: Método que consulta los totales que se pago de un concepto en la catorcena 
        ///              en la cuál se genero la nómina.
        /// 
        /// Parámetros: Unidad_Responsable_ID.- Unidad responsable  en cuál se consultara el total
        ///                                     por concepto.
        ///
        /// Usuario creó: Juan Alberto Hernández Negrete
        /// Fecha Creó: Marzo/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa modificación:
        ///************************************************************************************
        public static double Consultar_Montos_Conceptos_Por_Unidad_Responsable(String Unidad_Responsable_ID, 
            String Nomina_ID, String Periodo, String Percepcion_Deduccion_ID)
        {
            object Aux = null;
            double Monto_por_Unidad_Responsable = 0.0;
            StringBuilder Mi_SQL = new StringBuilder();

            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;

            Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
            Conexion.Open();

            Transaccion = Conexion.BeginTransaction();
            Comando.Connection = Conexion;
            Comando.Transaction = Transaccion;

            try
            {
                Mi_SQL.Append("select ");
                Mi_SQL.Append(" sum(" + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." +
                              Ope_Nom_Recibos_Empleados_Det.Campo_Monto + ") as MONTO ");

                Mi_SQL.Append(" from ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + " right outer join ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + " on ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                              Ope_Nom_Recibos_Empleados.Campo_No_Recibo + "=");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." +
                              Ope_Nom_Recibos_Empleados_Det.Campo_No_Recibo);

                if (!String.IsNullOrEmpty(Unidad_Responsable_ID))
                {
                    if (Mi_SQL.ToString().ToUpper().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" and ");
                        Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                                      Ope_Nom_Recibos_Empleados.Campo_Dependencia_ID + "='" + Unidad_Responsable_ID +
                                      "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" where ");
                        Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                                      Ope_Nom_Recibos_Empleados.Campo_Dependencia_ID + "='" + Unidad_Responsable_ID +
                                      "'");
                    }
                }

                if (!String.IsNullOrEmpty(Nomina_ID))
                {
                    if (Mi_SQL.ToString().ToUpper().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" and ");
                        Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                                      Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + "='" + Nomina_ID + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" where ");
                        Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                                      Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + "='" + Nomina_ID + "'");
                    }
                }

                if (!String.IsNullOrEmpty(Periodo))
                {
                    if (Mi_SQL.ToString().ToUpper().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" and ");
                        Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                                      Ope_Nom_Recibos_Empleados.Campo_No_Nomina + "=" + Periodo);
                    }
                    else
                    {
                        Mi_SQL.Append(" where ");
                        Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                                      Ope_Nom_Recibos_Empleados.Campo_No_Nomina + "=" + Periodo);
                    }
                }

                if (!String.IsNullOrEmpty(Percepcion_Deduccion_ID))
                {
                    if (Mi_SQL.ToString().ToUpper().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" and ");
                        Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." +
                                      Ope_Nom_Recibos_Empleados_Det.Campo_Percepcion_Deduccion_ID + "='" +
                                      Percepcion_Deduccion_ID + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" where ");
                        Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." +
                                      Ope_Nom_Recibos_Empleados_Det.Campo_Percepcion_Deduccion_ID + "='" +
                                      Percepcion_Deduccion_ID + "'");
                    }
                }

                Comando.CommandText = Mi_SQL.ToString();
                Aux = Comando.ExecuteScalar();

                if (!Convert.IsDBNull(Aux))
                    Monto_por_Unidad_Responsable = Convert.ToDouble(Aux);

                Transaccion.Commit();
            }
            catch (Exception ex)
            {
                Transaccion.Rollback();
                throw new Exception("Error . Error: [" + ex.Message + "]");
            }finally
            {
                Conexion.Close();
            }
            return Monto_por_Unidad_Responsable;
        }
        ///************************************************************************************
        /// Nombre Método: Consultar_Montos_Conceptos_Por_Unidad_Responsable
        /// 
        /// Descripción: Método que consulta los totales que se pago de un concepto en la catorcena 
        ///              en la cuál se genero la nómina.
        /// 
        /// Parámetros: Unidad_Responsable_ID.- Unidad responsable  en cuál se consultara el total
        ///                                     por concepto.
        ///
        /// Usuario creó: Juan Alberto Hernández Negrete
        /// Fecha Creó: Marzo/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa modificación:
        ///************************************************************************************
        public static double Consultar_Montos_Conceptos_Por_Unidad_Responsable(String Unidad_Responsable_ID,
            String Nomina_ID, String Periodo, String Percepcion_Deduccion_ID, String Tipo_Plaza)
        {
            object Aux = null;
            double Monto_por_Unidad_Responsable = 0.0;
            StringBuilder Mi_SQL = new StringBuilder();

            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;

            Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
            Conexion.Open();

            Transaccion = Conexion.BeginTransaction();
            Comando.Connection = Conexion;
            Comando.Transaction = Transaccion;

            try
            {
                Mi_SQL.Append("select ");
                Mi_SQL.Append(" sum(" + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." +
                              Ope_Nom_Recibos_Empleados_Det.Campo_Monto + ") as MONTO ");

                Mi_SQL.Append(" from ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + " right outer join ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + " on ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                              Ope_Nom_Recibos_Empleados.Campo_No_Recibo + "=");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." +
                              Ope_Nom_Recibos_Empleados_Det.Campo_No_Recibo);

                if (!String.IsNullOrEmpty(Unidad_Responsable_ID))
                {
                    if (Mi_SQL.ToString().ToUpper().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" and ");
                        Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                                      Ope_Nom_Recibos_Empleados.Campo_Dependencia_ID + "='" + Unidad_Responsable_ID +
                                      "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" where ");
                        Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                                      Ope_Nom_Recibos_Empleados.Campo_Dependencia_ID + "='" + Unidad_Responsable_ID +
                                      "'");
                    }
                }

                if (!String.IsNullOrEmpty(Nomina_ID))
                {
                    if (Mi_SQL.ToString().ToUpper().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" and ");
                        Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                                      Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + "='" + Nomina_ID + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" where ");
                        Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                                      Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + "='" + Nomina_ID + "'");
                    }
                }

                if (!String.IsNullOrEmpty(Periodo))
                {
                    if (Mi_SQL.ToString().ToUpper().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" and ");
                        Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                                      Ope_Nom_Recibos_Empleados.Campo_No_Nomina + "=" + Periodo);
                    }
                    else
                    {
                        Mi_SQL.Append(" where ");
                        Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                                      Ope_Nom_Recibos_Empleados.Campo_No_Nomina + "=" + Periodo);
                    }
                }

                if (!String.IsNullOrEmpty(Tipo_Plaza))
                {
                    if (Mi_SQL.ToString().ToUpper().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" and ");
                        Mi_SQL.Append("(select ");
                        Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Tipo_Plaza);
                        Mi_SQL.Append(" from ");
                        Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det);
                        Mi_SQL.Append(" where ");
                        Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Empleado_ID + "=");
                        Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + ") = '" + Tipo_Plaza + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" where ");
                        Mi_SQL.Append("(select ");
                        Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Tipo_Plaza);
                        Mi_SQL.Append(" from ");
                        Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det);
                        Mi_SQL.Append(" where ");
                        Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Empleado_ID + "=");
                        Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + ") = '" + Tipo_Plaza + "'");
                    }
                }

                if (!String.IsNullOrEmpty(Percepcion_Deduccion_ID))
                {
                    if (Mi_SQL.ToString().ToUpper().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" and ");
                        Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." +
                                      Ope_Nom_Recibos_Empleados_Det.Campo_Percepcion_Deduccion_ID + "='" +
                                      Percepcion_Deduccion_ID + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" where ");
                        Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." +
                                      Ope_Nom_Recibos_Empleados_Det.Campo_Percepcion_Deduccion_ID + "='" +
                                      Percepcion_Deduccion_ID + "'");
                    }
                }

                Comando.CommandText = Mi_SQL.ToString();
                Aux = Comando.ExecuteScalar();

                if (!Convert.IsDBNull(Aux))
                    Monto_por_Unidad_Responsable = Convert.ToDouble(Aux);

                Transaccion.Commit();
            }
            catch (Exception ex)
            {
                Transaccion.Rollback();
                throw new Exception("Error . Error: [" + ex.Message + "]");
            }
            finally
            {
                Conexion.Close();
            }
            return Monto_por_Unidad_Responsable;
        }
        #endregion
    }
}