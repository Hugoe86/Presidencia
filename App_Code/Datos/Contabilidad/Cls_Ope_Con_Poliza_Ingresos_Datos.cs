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
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using System.Text;
using Presidencia.Sessiones;

namespace Presidencia.Ope_Con_Poliza_Ingresos.Datos
{
    public class Cls_Ope_Con_Poliza_Ingresos_Datos
    {
        #region Metodos
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Poliza_Ingresos
        ///DESCRIPCIÓN          : consulta para modificar los datos de la poliza de ingresos
        ///PARAMETROS           1 Dt_Datos: datos a afectar en la poliza de ingresos
        ///CREO                 : Leslie Gonzalez Vázquez
        ///FECHA_CREO           : 20/Abril/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Alta_Poliza_Ingresos(DataTable Dt_Datos,OracleCommand Cmmd, String Referencia,
            String No_Poliza, String Tipo_Poliza_ID, String Mes_Anio, String Tipo_Poliza_Ing)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta.
            Boolean Operacion_Completa = false;//Estado de la operacion.
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans = null;
            if (Cmmd != null)
            {
                Cmd = Cmmd;
            }
            else
            {
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Trans.Connection;
                Cmd.Transaction = Trans;
            }
            String Rubro = String.Empty;
            String Tipo = String.Empty;
            String Clase = String.Empty;
            String Concepto = String.Empty;
            String Cuenta = String.Empty;
            String Partida = String.Empty;
            DataTable Dt_Registro = new DataTable();
            Int32 Registro_Ingreso = 0;
            DateTime Fecha = DateTime.Now;
            String MES = String.Format("{0:MM}", Fecha).ToUpper();
            String Cargo = String.Empty;
            String Abono = String.Empty;
            Double Importe = 0.00;
            Int32 No_Movimiento = 0;

            //obtenemos el numero de movimiento
            Mi_SQL.Append("SELECT NVL(MAX(" + Ope_Psp_Registro_Mov_Ingreso.Campo_No_Movimiento + "),0)");
            Mi_SQL.Append(" FROM " + Ope_Psp_Registro_Mov_Ingreso.Tabla_Ope_Psp_Registro_Mov_Ingreso);

            No_Movimiento = Convert.ToInt32(OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString().Trim()));
            No_Movimiento = No_Movimiento + 1;
            
            //obtenemos el no de presipuesto id por si se tiene q insertar un registro no existente
            Mi_SQL = new StringBuilder();
            Mi_SQL.Append("SELECT MAX (" + Ope_Psp_Presupuesto_Ingresos.Campo_Presupuesto_Ing_ID + ")");
            Mi_SQL.Append(" FROM " + Ope_Psp_Presupuesto_Ingresos.Tabla_Ope_Psp_Presupuesto_Ingresos);
            Registro_Ingreso = Convert.ToInt32(OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()));
            if (Convert.IsDBNull(Registro_Ingreso))
            {Registro_Ingreso = 1;}
            else{Registro_Ingreso = Registro_Ingreso + 1;}

            try
            {
                if (Dt_Datos != null)
                {
                    if (Dt_Datos.Rows.Count > 0)
                    {
                        if (!String.IsNullOrEmpty(No_Poliza.Trim()) && !String.IsNullOrEmpty(Tipo_Poliza_ID.Trim()) 
                            && !String.IsNullOrEmpty(Mes_Anio.Trim()) && !String.IsNullOrEmpty(Tipo_Poliza_Ing.Trim()))
                        {
                            foreach (DataRow Dr in Dt_Datos.Rows)
                            {
                                //validamos si la fuente de financiamiento tiene dato y la dependencia viene vacia, 
                                //el programa lo validaremos mas adelante porque puede o no venir
                                if (!String.IsNullOrEmpty(Dr["FUENTE_FINANCIAMIENTO_ID"].ToString().Trim()) && String.IsNullOrEmpty(Dr["DEPENDENCIA_ID"].ToString().Trim()))
                                {
                                    Cuenta = Dr[Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID].ToString().Trim();

                                    Mi_SQL = new StringBuilder();
                                    Mi_SQL.Append("SELECT " + Cat_Con_Cuentas_Contables.Campo_Partida_ID);
                                    Mi_SQL.Append(" FROM " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables);
                                    Mi_SQL.Append(" WHERE " + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID + " = '" + Cuenta + "'");

                                    Concepto = (String)OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

                                    Mi_SQL = new StringBuilder();
                                    Mi_SQL.Append("SELECT " + Cat_Psp_Concepto_Ing.Campo_Clase_Ing_ID);
                                    Mi_SQL.Append(" FROM " + Cat_Psp_Concepto_Ing.Tabla_Cat_Psp_Concepto_Ing);
                                    Mi_SQL.Append(" WHERE " + Cat_Psp_Concepto_Ing.Campo_Concepto_Ing_ID + " = '" + Concepto + "'");

                                    Clase = (String)OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

                                    Mi_SQL = new StringBuilder();
                                    Mi_SQL.Append("SELECT " + Cat_Psp_Clase_Ing.Campo_Tipo_ID);
                                    Mi_SQL.Append(" FROM " + Cat_Psp_Clase_Ing.Tabla_Cat_Psp_Clase_Ing);
                                    Mi_SQL.Append(" WHERE " + Cat_Psp_Clase_Ing.Campo_Clase_Ing_ID + " = '" + Clase + "'");

                                    Tipo = (String)OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

                                    Mi_SQL = new StringBuilder();
                                    Mi_SQL.Append("SELECT " + Cat_Psp_Tipo.Campo_Rubro_ID);
                                    Mi_SQL.Append(" FROM " + Cat_Psp_Tipo.Tabla_Cat_Psp_Tipo);
                                    Mi_SQL.Append(" WHERE " + Cat_Psp_Tipo.Campo_Tipo_ID + " = '" + Tipo + "'");

                                    Rubro = (String)OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

                                    if (Convert.ToDouble(String.IsNullOrEmpty(Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim()) ? "0" : Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", "")) > 0)
                                    {
                                        if(Tipo_Poliza_Ing.Trim().Equals("DEVENGADO"))
                                        {
                                            Cargo = Ope_Psp_Presupuesto_Ingresos.Campo_Devengado.Trim();
                                            Abono = Ope_Psp_Presupuesto_Ingresos.Campo_Por_Recaudar.Trim();
                                        }
                                        else if (Tipo_Poliza_Ing.Trim().Equals("RECAUDADO"))
                                        {
                                            Cargo = Ope_Psp_Presupuesto_Ingresos.Campo_Recaudado.Trim();
                                            Abono = Ope_Psp_Presupuesto_Ingresos.Campo_Devengado.Trim();
                                        }
                                        
                                        Importe = Convert.ToDouble(String.IsNullOrEmpty(Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim()) ? "0" : Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",",""));

                                        //Primero se consulta si existe un registro en el presupuesto de ingreso 
                                        Mi_SQL = new StringBuilder();
                                        Mi_SQL.Append("SELECT * FROM " + Ope_Psp_Presupuesto_Ingresos.Tabla_Ope_Psp_Presupuesto_Ingresos);
                                        Mi_SQL.Append(" WHERE " + Ope_Psp_Presupuesto_Ingresos.Campo_Fuente_Financiamiento_ID + " = '" + Dr["FUENTE_FINANCIAMIENTO_ID"].ToString().Trim() + "' ");
                                        if (!String.IsNullOrEmpty(Dr["PROYECTO_PROGRAMA_ID"].ToString().Trim()))
                                        {
                                            Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Proyecto_Programa_ID + " = '" + Dr["PROYECTO_PROGRAMA_ID"].ToString().Trim() + "' ");
                                        }
                                        Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Anio + " = " + String.Format("{0:yyyy}", DateTime.Now) + " ");
                                        Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Rubro_ID + " = '" + Rubro.Trim() + "' ");
                                        Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Tipo_ID + " = '" + Tipo.Trim() + "' ");
                                        Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Clase_Ing_ID + " = '" + Clase.Trim() + "' ");
                                        Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Concepto_Ing_ID + " = '" + Concepto.Trim() + "' ");

                                        Dt_Registro = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                                        if (Dt_Registro.Rows.Count > 0)
                                        {
                                            Mi_SQL = new StringBuilder();
                                            Mi_SQL.Append("UPDATE " + Ope_Psp_Presupuesto_Ingresos.Tabla_Ope_Psp_Presupuesto_Ingresos);
                                            if (Tipo_Poliza_Ing.Trim().Equals("DEVENGADO"))
                                            {
                                                Mi_SQL.Append(" SET " + Ope_Psp_Presupuesto_Ingresos.Campo_Devengado + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Devengado + " + ");
                                                Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", "") + ", ");
                                                Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Por_Recaudar + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Por_Recaudar + " - ");
                                                Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", ""));
                                            }
                                            else if (Tipo_Poliza_Ing.Trim().Equals("RECAUDADO"))
                                            {
                                                Mi_SQL.Append(" SET " + Ope_Psp_Presupuesto_Ingresos.Campo_Recaudado + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Recaudado + " + ");
                                                Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", "") + ", ");
                                                Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Devengado + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Devengado + " - ");
                                                Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", "") + ", ");

                                                if (Convert.ToInt32(MES) == 1)
                                                {
                                                    Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Enero + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Enero + " + ");
                                                    Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", ""));
                                                }
                                                else if (Convert.ToInt32(MES) == 2)
                                                {
                                                    Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Febrero + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Febrero + " + ");
                                                    Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", ""));
                                                }
                                                else if (Convert.ToInt32(MES) == 3)
                                                {
                                                    Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Marzo + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Marzo + " + ");
                                                    Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", ""));
                                                }
                                                else if (Convert.ToInt32(MES) == 4)
                                                {
                                                    Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Abril + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Abril + " + ");
                                                    Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", ""));
                                                }
                                                else if (Convert.ToInt32(MES) == 5)
                                                {
                                                    Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Mayo + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Mayo + " + ");
                                                    Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", ""));
                                                }
                                                else if (Convert.ToInt32(MES) == 6)
                                                {
                                                    Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Junio + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Junio + " + ");
                                                    Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", ""));
                                                }
                                                else if (Convert.ToInt32(MES) == 7)
                                                {
                                                    Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Julio + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Julio + " + ");
                                                    Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", ""));
                                                }
                                                else if (Convert.ToInt32(MES) == 8)
                                                {
                                                    Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Agosto + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Agosto + " + ");
                                                    Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", ""));
                                                }
                                                else if (Convert.ToInt32(MES) == 9)
                                                {
                                                    Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Septiembre + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Septiembre + " + ");
                                                    Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", ""));
                                                }
                                                else if (Convert.ToInt32(MES) == 10)
                                                {
                                                    Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Octubre + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Octubre + " + ");
                                                    Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", ""));
                                                }
                                                else if (Convert.ToInt32(MES) == 11)
                                                {
                                                    Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Noviembre + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Noviembre + " + ");
                                                    Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", ""));
                                                }
                                                else if (Convert.ToInt32(MES) == 12)
                                                {
                                                    Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Diciembre + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Diciembre + " + ");
                                                    Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", ""));
                                                }
                                            }

                                            Mi_SQL.Append(" WHERE " + Ope_Psp_Presupuesto_Ingresos.Campo_Fuente_Financiamiento_ID + " = '" + Dr["FUENTE_FINANCIAMIENTO_ID"].ToString().Trim() + "' ");
                                            if (!String.IsNullOrEmpty(Dr["PROYECTO_PROGRAMA_ID"].ToString().Trim()))
                                            {
                                                Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Proyecto_Programa_ID + " = '" + Dr["PROYECTO_PROGRAMA_ID"].ToString().Trim() + "' ");
                                            }
                                            Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Anio + " = " + String.Format("{0:yyyy}", DateTime.Now) + " ");
                                            Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Rubro_ID + " = '" + Rubro.Trim() + "' ");
                                            Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Tipo_ID + " = '" + Tipo.Trim() + "' ");
                                            Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Clase_Ing_ID + " = '" + Clase.Trim() + "' ");
                                            Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Concepto_Ing_ID + " = '" + Concepto.Trim() + "' ");
                                            Cmd.CommandText = Mi_SQL.ToString();
                                            Cmd.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            //Se inserta el registro ya que no existe en el presupuesto
                                            Mi_SQL = new StringBuilder();
                                            Mi_SQL.Append("INSERT INTO " + Ope_Psp_Presupuesto_Ingresos.Tabla_Ope_Psp_Presupuesto_Ingresos + " (");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Presupuesto_Ing_ID + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Fuente_Financiamiento_ID + ", ");
                                            if (!String.IsNullOrEmpty(Dr["PROYECTO_PROGRAMA_ID"].ToString().Trim()))
                                            { Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Proyecto_Programa_ID + ", ");}
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Rubro_ID + ", "+Ope_Psp_Presupuesto_Ingresos.Campo_Tipo_ID + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Clase_Ing_ID  + ", "+Ope_Psp_Presupuesto_Ingresos.Campo_Concepto_Ing_ID + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Anio + ", " + Ope_Psp_Presupuesto_Ingresos.Campo_Importe_Enero  + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Importe_Febrero  + ", "+ Ope_Psp_Presupuesto_Ingresos.Campo_Importe_Marzo  + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Importe_Abril+ ", "+Ope_Psp_Presupuesto_Ingresos.Campo_Importe_Mayo + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Importe_Junio + ", "+Ope_Psp_Presupuesto_Ingresos.Campo_Importe_Julio + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Importe_Agosto+ ", "+ Ope_Psp_Presupuesto_Ingresos.Campo_Importe_Septiembre+ ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Importe_Octubre + ", "+Ope_Psp_Presupuesto_Ingresos.Campo_Importe_Noviembre + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Importe_Diciembre + ", " + Ope_Psp_Presupuesto_Ingresos.Campo_Importe_Total + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Aprobado  + ", "+Ope_Psp_Presupuesto_Ingresos.Campo_Ampliacion  + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Reduccion + ", "+Ope_Psp_Presupuesto_Ingresos.Campo_Modificado + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Devengado + ", " + Ope_Psp_Presupuesto_Ingresos.Campo_Recaudado + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Por_Recaudar + ", " + Ope_Psp_Presupuesto_Ingresos.Campo_Saldo + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Enero + ", " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Febrero  + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Marzo  + ", " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Abril + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Mayo + ", " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Junio + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Julio + ", " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Agosto + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Septiembre + ", " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Octubre + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Noviembre + ", " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Diciembre + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Usuario_Creo + ", " + Ope_Psp_Presupuesto_Ingresos.Campo_Fecha_Creo + ") ");
                                            Mi_SQL.Append("VALUES (" + Registro_Ingreso + ", '" + Dr["FUENTE_FINANCIAMIENTO_ID"].ToString().Trim() + "', ");
                                            if (!String.IsNullOrEmpty(Dr["PROYECTO_PROGRAMA_ID"].ToString().Trim()))
                                            { Mi_SQL.Append("'" + Dr["PROYECTO_PROGRAMA_ID"].ToString().Trim() + "', "); }
                                            Mi_SQL.Append("'" + Rubro.Trim() + "', '" + Tipo.Trim() + "', '");
                                            Mi_SQL.Append(Clase.Trim() + "', '" + Concepto.Trim() + "', " + String.Format("{0:yyyy}", DateTime.Now) + ", ");
                                            Mi_SQL.Append("0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00,");
                                            if (Tipo_Poliza_Ing.Trim().Equals("DEVENGADO"))
                                            {
                                                Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", "") + ", 0.00, ");
                                                Mi_SQL.Append("-" + Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", "") + ", 0.00, ");
                                            }
                                            else if (Tipo_Poliza_Ing.Trim().Equals("RECAUDADO"))
                                            {
                                                Mi_SQL.Append(" 0.00, " + Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", "") + ", ");
                                                Mi_SQL.Append("-" + Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", "") + ", 0.00, ");
                                            }

                                            if (Convert.ToDouble(MES) == 1)
                                            {
                                                Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", "") + ",");
                                            }
                                            else
                                            {
                                                Mi_SQL.Append("0.00,");
                                            }
                                            if (Convert.ToDouble(MES) == 2)
                                            {
                                                Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", "") + ",");
                                            }
                                            else
                                            {
                                                Mi_SQL.Append("0.00,");
                                            }
                                            if (Convert.ToDouble(MES) == 3)
                                            {
                                                Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", "") + ",");
                                            }
                                            else
                                            {
                                                Mi_SQL.Append("0.00,");
                                            }
                                            if (Convert.ToDouble(MES) == 4)
                                            {
                                                Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", "") + ",");
                                            }
                                            else
                                            {
                                                Mi_SQL.Append("0.00,");
                                            }
                                            if (Convert.ToDouble(MES) == 5)
                                            {
                                                Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", "") + ",");
                                            }
                                            else
                                            {
                                                Mi_SQL.Append("0.00,");
                                            }
                                            if (Convert.ToDouble(MES) == 6)
                                            {
                                                Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", "") + ",");
                                            }
                                            else
                                            {
                                                Mi_SQL.Append("0.00,");
                                            }
                                            if (Convert.ToDouble(MES) == 7)
                                            {
                                                Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", "") + ",");
                                            }
                                            else
                                            {
                                                Mi_SQL.Append("0.00,");
                                            }
                                            if (Convert.ToDouble(MES) == 8)
                                            {
                                                Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", "") + ",");
                                            }
                                            else
                                            {
                                                Mi_SQL.Append("0.00,");
                                            }
                                            if (Convert.ToDouble(MES) == 9)
                                            {
                                                Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", "") + ",");
                                            }
                                            else
                                            {
                                                Mi_SQL.Append("0.00,");
                                            }
                                            if (Convert.ToDouble(MES) == 10)
                                            {
                                                Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", "") + ",");
                                            }
                                            else
                                            {
                                                Mi_SQL.Append("0.00,");
                                            }
                                            if (Convert.ToDouble(MES) == 11)
                                            {
                                                 Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", "") + ",");
                                            }
                                            else
                                            {
                                                Mi_SQL.Append("0.00,");
                                            }
                                            if (Convert.ToDouble(MES) == 12)
                                            {
                                                Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", "") + ",");
                                            }
                                            else
                                            {
                                                Mi_SQL.Append("0.00,");
                                            }
                                            Mi_SQL.Append("'"+Cls_Sessiones.Nombre_Empleado.ToString() + "',SYSDATE)");
                                            Cmd.CommandText = Mi_SQL.ToString();
                                            Cmd.ExecuteNonQuery();

                                            Registro_Ingreso++;
                                        }
                                        Operacion_Completa = true;
                                    }
                                    else if (Convert.ToDouble(String.IsNullOrEmpty(Dr[Ope_Con_Polizas_Detalles.Campo_Haber].ToString().Trim()) ? "0" : Dr[Ope_Con_Polizas_Detalles.Campo_Haber].ToString().Trim()) > 0)
                                    {
                                        if (Tipo_Poliza_Ing.Trim().Equals("DEVENGADO"))
                                        {
                                            Cargo = Ope_Psp_Presupuesto_Ingresos.Campo_Devengado.Trim();
                                            Abono = Ope_Psp_Presupuesto_Ingresos.Campo_Recaudado.Trim();
                                        }
                                        else if (Tipo_Poliza_Ing.Trim().Equals("RECAUDADO"))
                                        {
                                            Cargo = Ope_Psp_Presupuesto_Ingresos.Campo_Por_Recaudar.Trim();
                                            Abono = Ope_Psp_Presupuesto_Ingresos.Campo_Devengado.Trim();
                                        }
                                        else
                                        {
                                            break;
                                        }

                                        Importe = Convert.ToDouble(String.IsNullOrEmpty(Dr[Ope_Con_Polizas_Detalles.Campo_Haber].ToString().Trim()) ? "0" : Dr[Ope_Con_Polizas_Detalles.Campo_Haber].ToString().Trim().Replace(",",""));

                                        //Primero se consulta si existe un registro en el presupuesto de ingreso 
                                        Mi_SQL = new StringBuilder();
                                        Mi_SQL.Append("SELECT * FROM " + Ope_Psp_Presupuesto_Ingresos.Tabla_Ope_Psp_Presupuesto_Ingresos);
                                        Mi_SQL.Append(" WHERE " + Ope_Psp_Presupuesto_Ingresos.Campo_Fuente_Financiamiento_ID + " = '" + Dr["FUENTE_FINANCIAMIENTO_ID"].ToString().Trim() + "' ");
                                        if (!String.IsNullOrEmpty(Dr["PROYECTO_PROGRAMA_ID"].ToString().Trim()))
                                        {Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Proyecto_Programa_ID + " = '" + Dr["PROYECTO_PROGRAMA_ID"].ToString().Trim() + "' ");}
                                        Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Anio + " = " + String.Format("{0:yyyy}", DateTime.Now) + " ");
                                        Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Rubro_ID + " = '" + Rubro.Trim() + "' ");
                                        Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Tipo_ID + " = '" + Tipo.Trim() + "' ");
                                        Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Clase_Ing_ID + " = '" + Clase.Trim() + "' ");
                                        Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Concepto_Ing_ID + " = '" + Concepto.Trim() + "' ");
                                        Dt_Registro = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                                        if (Dt_Registro.Rows.Count > 0)
                                        {
                                            Mi_SQL = new StringBuilder();
                                            Mi_SQL.Append("UPDATE " + Ope_Psp_Presupuesto_Ingresos.Tabla_Ope_Psp_Presupuesto_Ingresos);
                                            if (Tipo_Poliza_Ing.Trim().Equals("DEVENGADO"))
                                            {
                                                Mi_SQL.Append(" SET " + Ope_Psp_Presupuesto_Ingresos.Campo_Recaudado + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Recaudado + " - ");
                                                Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Haber].ToString().Trim().Replace(",", "") + ", ");
                                                Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Devengado + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Devengado + " + ");
                                                Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Haber].ToString().Trim().Replace(",", ""));
                                            }
                                            else if (Tipo_Poliza_Ing.Trim().Equals("RECAUDADO"))
                                            {
                                                Mi_SQL.Append(" SET " + Ope_Psp_Presupuesto_Ingresos.Campo_Devengado + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Devengado + " - ");
                                                Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Haber].ToString().Trim().Replace(",", "") + ", ");
                                                Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Por_Recaudar + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Por_Recaudar + " + ");
                                                Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Haber].ToString().Trim().Replace(",", ""));
                                            }
                                            
                                            Mi_SQL.Append(" WHERE " + Ope_Psp_Presupuesto_Ingresos.Campo_Fuente_Financiamiento_ID + " = '" + Dr["FUENTE_FINANCIAMIENTO_ID"].ToString().Trim() + "' ");
                                            if (!String.IsNullOrEmpty(Dr["PROYECTO_PROGRAMA_ID"].ToString().Trim()))
                                            {Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Proyecto_Programa_ID + " = '" + Dr["PROYECTO_PROGRAMA_ID"].ToString().Trim() + "' ");}
                                            Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Anio + " = " + String.Format("{0:yyyy}", DateTime.Now) + " ");
                                            Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Rubro_ID + " = '" + Rubro.Trim() + "' ");
                                            Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Tipo_ID + " = '" + Tipo.Trim() + "' ");
                                            Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Clase_Ing_ID + " = '" + Clase.Trim() + "' ");
                                            Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Concepto_Ing_ID + " = '" + Concepto.Trim() + "' ");
                                            Cmd.CommandText = Mi_SQL.ToString();
                                            Cmd.ExecuteNonQuery();
                                        }
                                    }

                                    //insertamos los registros de los movimientos del presupuesto

                                    Insertar_Registro_Mov_Ing(Cargo.Trim(), Abono.Trim(), Importe.ToString().Trim(),
                                        Dr[Ope_Con_Polizas_Detalles.Campo_Concepto].ToString().Trim(), Referencia.Trim(), No_Poliza.Trim(), Tipo_Poliza_ID.Trim(),
                                        Mes_Anio.Trim(), Dr["FUENTE_FINANCIAMIENTO_ID"].ToString().Trim(), Dr["PROYECTO_PROGRAMA_ID"].ToString().Trim(), Rubro.Trim(),
                                        Tipo.Trim(), Clase.Trim(), Concepto.Trim(), String.Empty, Cls_Sessiones.Nombre_Empleado.Trim(), Cmd, No_Movimiento);

                                    No_Movimiento++;
                                }
                                else if (!String.IsNullOrEmpty(Dr["FUENTE_FINANCIAMIENTO_ID"].ToString().Trim()) && !String.IsNullOrEmpty(Dr["PROYECTO_PROGRAMA_ID"].ToString().Trim()) && !String.IsNullOrEmpty(Dr["DEPENDENCIA_ID"].ToString().Trim()))
                                {
                                    Cuenta = Dr[Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID].ToString().Trim();

                                    Mi_SQL = new StringBuilder();
                                    Mi_SQL.Append("SELECT " + Cat_Con_Cuentas_Contables.Campo_Partida_ID);
                                    Mi_SQL.Append(" FROM " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables);
                                    Mi_SQL.Append(" WHERE " + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID + " = '" + Cuenta + "'");

                                    Partida = (String)OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

                                    if (Convert.ToDouble(String.IsNullOrEmpty(Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim()) ? "0" : Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim()) > 0)
                                    {
                                        Mi_SQL = new StringBuilder();
                                        Mi_SQL.Append("UPDATE " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado);
                                        Mi_SQL.Append(" SET " + Dr["MOMENTO_INICIAL"].ToString().Trim() + " = " + Dr["MOMENTO_INICIAL"].ToString().Trim() + " - ");
                                        Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", "") + ", ");
                                        Mi_SQL.Append(Dr["MOMENTO_FINAL"].ToString().Trim() + " = " + Dr["MOMENTO_FINAL"].ToString().Trim() + " + ");
                                        Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Debe].ToString().Trim().Replace(",", "") + " ");
                                        Mi_SQL.Append(" WHERE " + Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID + " = '" + Dr["FUENTE_FINANCIAMIENTO_ID"].ToString().Trim() + "' ");
                                        Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Aprobado.Campo_Anio + " = " + String.Format("{0:yyyy}", DateTime.Now) + " ");
                                        Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Aprobado.Campo_Proyecto_Programa_ID + " = '" + Dr["PROYECTO_PROGRAMA_ID"].ToString().Trim() + "' ");
                                        Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Aprobado.Campo_Partida_ID + " = '" + Partida + "' ");
                                        Cmd.CommandText = Mi_SQL.ToString();
                                        Cmd.ExecuteNonQuery();

                                        Operacion_Completa = true;
                                    }
                                    else if (Convert.ToDouble(String.IsNullOrEmpty(Dr[Ope_Con_Polizas_Detalles.Campo_Haber].ToString().Trim()) ? "0" : Dr[Ope_Con_Polizas_Detalles.Campo_Haber].ToString().Trim()) > 0)
                                    {
                                        Mi_SQL = new StringBuilder();
                                        Mi_SQL.Append("UPDATE " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado);
                                        Mi_SQL.Append(" SET " + Dr["MOMENTO_INICIAL"].ToString().Trim() + " = " + Dr["MOMENTO_INICIAL"].ToString().Trim() + " + ");
                                        Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Haber].ToString().Trim().Replace(",", "") + ", ");
                                        Mi_SQL.Append(Dr["MOMENTO_FINAL"].ToString().Trim() + " = " + Dr["MOMENTO_FINAL"].ToString().Trim() + " - ");
                                        Mi_SQL.Append(Dr[Ope_Con_Polizas_Detalles.Campo_Haber].ToString().Trim().Replace(",", "") + " ");
                                        Mi_SQL.Append(" WHERE " + Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID + " = '" + Dr["FUENTE_FINANCIAMIENTO_ID"].ToString().Trim() + "' ");
                                        Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Aprobado.Campo_Anio + " = " + String.Format("{0:yyyy}", DateTime.Now) + " ");
                                        Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Aprobado.Campo_Proyecto_Programa_ID + " = '" + Dr["PROYECTO_PROGRAMA_ID"].ToString().Trim() + "' ");
                                        Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Aprobado.Campo_Partida_ID + " = '" + Partida + "' ");
                                        Cmd.CommandText = Mi_SQL.ToString();
                                        Cmd.ExecuteNonQuery(); ;
                                    }
                                }
                            }
                        }
                        if (Cmmd == null)
                        {
                            Trans.Commit();
                        }

                       Operacion_Completa = true;
                    }
                }
            }
            catch (Exception Ex)
            {
                if (Cmmd == null)
                {
                    Trans.Rollback();
                }
                throw new Exception("Error al ejecutar los el alta de la poliza de ingresos. Error: [" + Ex.Message + "]");
            }
            finally
            {
                if (Cmmd == null)
                {
                    Cn.Close();
                }
            }
            return Operacion_Completa;
        }

        ///*******************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Insertar_Registro_Mov_Ing
        ///DESCRIPCIÓN          : consulta para insertar los datos de los movimientos de ingresos
        ///PARAMETROS           1 Cargo: Nombre Columna de ingresos a la que se realizara el cargo (Obligatorio)
        ///                     2 Abono: Nombre Columna de ingresos a la que se realizara el abono (Obligatorio)
        ///                     3 Importe: monto que se afectara en el presupuesto de ingresos (Obligatorio)
        ///                     4 Descripcion: aqui puede ir el concepto, justificacion o descripcion del movimiento (Opcional)
        ///                     5 Referencia: aqui puede ir la referencia motivo del movimiento (Opcional)
        ///                     6 No_Poliza: no de poliza que se genero para afectar el presupuesto de ingresos (Obligatorio)
        ///                     7 Tipo_Poliza_ID: tipo de poliza que se genero para afectar el presupuesto de ingresos (Obligatorio)
        ///                     8 Mes_Anio: mes y año de la poliza que se genero para afectar el presupuesto de ingresos (Obligatorio)
        ///                     9 Fte_Financiamiento_ID: fuente de financiamiento al que se afectara el ingreso (Obligatorio)
        ///                     10 Programa_ID: programa al que se afectara el ingreso, si es que se cuenta con el nombre del programa  (Opcional)
        ///                     11 Rubro_ID: rubro al que pertenece el concepto que afectara el presupuesto(Obligatorio)
        ///                     12 Tipo_ID: tipo al que pertenece el concepto que afectara el presupuesto (Obligatorio)
        ///                     13 Clase_ID: clase al que pertenece el concepto que afectara el presupuesto (Obligatorio)
        ///                     14 Concepto_ID: concepto que afectara el presupuesto (Obligatorio)
        ///                     15 SubConcepto_ID: subconcepto que se afectara el presupuesto, si es que llega a este nivel  (Opcional)
        ///                     16 Usuario: usuario que solicito realizar la poliza (Obligatorio)
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 28/Agosto/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************************************
        public static Boolean Insertar_Registro_Mov_Ing(String Cargo, String Abono, String Importe, String Descripcion, String Referencia,
             String No_Poliza, String Tipo_Poliza_ID, String Mes_Anio, String Fte_Financiamiento_ID, String Programa_ID, String Rubro_ID,
             String Tipo_ID, String Clase_ID, String Concepto_ID, String SubConcepto_ID, String Usuario, OracleCommand Cmmd,
            Int32 No_Movimiento)
        {
            
            Boolean Operacion_Completa = false;

            StringBuilder Mi_SQL = new StringBuilder();
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans = null;

            if (Cmmd != null)
            {
                Cmd = Cmmd;
            }
            else
            {
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Trans.Connection;
                Cmd.Transaction = Trans;
            }

            try
            {
                Mi_SQL = new StringBuilder();
                Mi_SQL.Append("INSERT INTO " + Ope_Psp_Registro_Mov_Ingreso.Tabla_Ope_Psp_Registro_Mov_Ingreso + "(");
                Mi_SQL.Append(Ope_Psp_Registro_Mov_Ingreso.Campo_No_Movimiento + ", ");
                if (!String.IsNullOrEmpty(Cargo.Trim()))
                { Mi_SQL.Append(Ope_Psp_Registro_Mov_Ingreso.Campo_Cargo + ", "); }
                if (!String.IsNullOrEmpty(Abono.Trim()))
                { Mi_SQL.Append(Ope_Psp_Registro_Mov_Ingreso.Campo_Abono + ", "); }
                if (!String.IsNullOrEmpty(Importe.Trim()))
                { Mi_SQL.Append(Ope_Psp_Registro_Mov_Ingreso.Campo_Importe + ", "); }
                Mi_SQL.Append(Ope_Psp_Registro_Mov_Ingreso.Campo_Fecha + ", ");
                if (!String.IsNullOrEmpty(Descripcion.Trim()))
                { Mi_SQL.Append(Ope_Psp_Registro_Mov_Ingreso.Campo_Descripcion + ", "); }
                if (!String.IsNullOrEmpty(Referencia.Trim()))
                { Mi_SQL.Append(Ope_Psp_Registro_Mov_Ingreso.Campo_Referencia + ", "); }
                if (!String.IsNullOrEmpty(No_Poliza.Trim()))
                { Mi_SQL.Append(Ope_Psp_Registro_Mov_Ingreso.Campo_No_Poliza + ", "); }
                if (!String.IsNullOrEmpty(Tipo_Poliza_ID.Trim()))
                { Mi_SQL.Append(Ope_Psp_Registro_Mov_Ingreso.Campo_Tipo_Poliza_ID + ", "); }
                if (!String.IsNullOrEmpty(Mes_Anio.Trim()))
                { Mi_SQL.Append(Ope_Psp_Registro_Mov_Ingreso.Campo_Mes_Ano + ", "); }
                if (!String.IsNullOrEmpty(Fte_Financiamiento_ID.Trim()))
                { Mi_SQL.Append(Ope_Psp_Registro_Mov_Ingreso.Campo_Fte_Financiamiento_ID + ", "); }
                if (!String.IsNullOrEmpty(Programa_ID.Trim()))
                { Mi_SQL.Append(Ope_Psp_Registro_Mov_Ingreso.Campo_Proyecto_Programa_ID + ", "); }
                if (!String.IsNullOrEmpty(Rubro_ID.Trim()))
                { Mi_SQL.Append(Ope_Psp_Registro_Mov_Ingreso.Campo_Rubro_ID + ", "); }
                if (!String.IsNullOrEmpty(Tipo_ID.Trim()))
                { Mi_SQL.Append(Ope_Psp_Registro_Mov_Ingreso.Campo_Tipo_ID + ", "); }
                if (!String.IsNullOrEmpty(Clase_ID.Trim()))
                { Mi_SQL.Append(Ope_Psp_Registro_Mov_Ingreso.Campo_Clase_Ing_ID + ", "); }
                if (!String.IsNullOrEmpty(Concepto_ID.Trim()))
                { Mi_SQL.Append(Ope_Psp_Registro_Mov_Ingreso.Campo_Concepto_Ing_ID + ", "); }
                if (!String.IsNullOrEmpty(SubConcepto_ID.Trim()))
                { Mi_SQL.Append(Ope_Psp_Registro_Mov_Ingreso.Campo_SubConcepto_Ing_ID + ", "); }
                Mi_SQL.Append(Ope_Psp_Registro_Mov_Ingreso.Campo_Usuario_Creo + ", ");
                Mi_SQL.Append(Ope_Psp_Registro_Mov_Ingreso.Campo_Fecha_Creo + ") VALUES( ");
                Mi_SQL.Append(No_Movimiento + ", ");
                if (!String.IsNullOrEmpty(Cargo.Trim()))
                { Mi_SQL.Append("'" + Cargo.Trim() + "', "); }
                if (!String.IsNullOrEmpty(Abono.Trim()))
                { Mi_SQL.Append("'" + Abono.Trim() + "', "); }
                if (!String.IsNullOrEmpty(Importe.Trim()))
                { Mi_SQL.Append(Importe.Trim().Replace(",", "").Replace("$", "") + ", "); }
                Mi_SQL.Append("SYSDATE, ");
                if (!String.IsNullOrEmpty(Descripcion.Trim()))
                { Mi_SQL.Append("'" + Descripcion.Trim() + "', "); }
                if (!String.IsNullOrEmpty(Referencia.Trim()))
                { Mi_SQL.Append("'" + Referencia.Trim() + "', "); }
                if (!String.IsNullOrEmpty(No_Poliza.Trim()))
                { Mi_SQL.Append("'" + No_Poliza.Trim() + "', "); }
                if (!String.IsNullOrEmpty(Tipo_Poliza_ID.Trim()))
                { Mi_SQL.Append("'" + Tipo_Poliza_ID.Trim() + "', "); }
                if (!String.IsNullOrEmpty(Mes_Anio.Trim()))
                { Mi_SQL.Append("'" + Mes_Anio.Trim() + "', "); }
                if (!String.IsNullOrEmpty(Fte_Financiamiento_ID.Trim()))
                { Mi_SQL.Append("'" + Fte_Financiamiento_ID.Trim() + "', "); }
                if (!String.IsNullOrEmpty(Programa_ID.Trim()))
                { Mi_SQL.Append("'" + Programa_ID.Trim() + "', "); }
                if (!String.IsNullOrEmpty(Rubro_ID.Trim()))
                { Mi_SQL.Append("'" + Rubro_ID.Trim() + "', "); }
                if (!String.IsNullOrEmpty(Tipo_ID.Trim()))
                { Mi_SQL.Append("'" + Tipo_ID.Trim() + "', "); }
                if (!String.IsNullOrEmpty(Clase_ID.Trim()))
                { Mi_SQL.Append("'" + Clase_ID.Trim() + "', "); }
                if (!String.IsNullOrEmpty(Concepto_ID.Trim()))
                { Mi_SQL.Append("'" + Concepto_ID.Trim() + "', "); }
                if (!String.IsNullOrEmpty(SubConcepto_ID.Trim()))
                { Mi_SQL.Append("'" + SubConcepto_ID.Trim() + "', "); }
                Mi_SQL.Append("'" + Usuario.Trim() + "', ");
                Mi_SQL.Append("SYSDATE) ");

                Cmd.CommandText = Mi_SQL.ToString();
                Cmd.ExecuteNonQuery();

                if (Cmmd == null)
                {
                    Trans.Commit();
                }
            }
            catch (Exception Ex)
            {
                if (Cmmd == null)
                {
                    Trans.Rollback();
                }
                throw new Exception("Error al intentar insertat los datos del movimiento de ingresos. Error: [" + Ex.Message + "]");
            }
            finally
            {
                if (Cmmd == null)
                {
                    Cn.Close();
                }
            }
            return Operacion_Completa;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Movimientos_Presupuestales
        ///DESCRIPCIÓN          : consulta para modificar los datos del presupuesto de ingresos
        ///PARAMETROS           1 Dt_Datos: datos a afectar en la poliza de ingresos
        ///CREO                 : Leslie Gonzalez Vázquez
        ///FECHA_CREO           : 07/Septiembre/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Alta_Movimientos_Presupuestales(DataTable Dt_Datos, OracleCommand Cmmd, String Referencia,
            String No_Poliza, String Tipo_Poliza_ID, String Mes_Anio, String Momento_Inicial, String Momento_Final)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta.
            Boolean Operacion_Completa = false;//Estado de la operacion.
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans = null;
            if (Cmmd != null)
            {
                Cmd = Cmmd;
            }
            else
            {
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Trans.Connection;
                Cmd.Transaction = Trans;
            }
            String Rubro = String.Empty;
            String Tipo = String.Empty;
            String Clase = String.Empty;
            String Concepto = String.Empty;
            String SubConcepto = String.Empty;
            DataTable Dt_Registro = new DataTable();
            Int32 Registro_Ingreso = 0;
            DateTime Fecha = DateTime.Now;
            String MES = String.Format("{0:MM}", Fecha).ToUpper();
            Double Importe = 0.00;
            Int32 No_Movimiento = 0;
            Boolean Existe_Psp_SubConcepto = false;
            String Anio = String.Format("{0:yyyy}", DateTime.Now);
            Boolean Por_Recaudar = false;
            Boolean Suma = false;

            //obtenemos el numero de movimiento
            Mi_SQL.Append("SELECT NVL(MAX(" + Ope_Psp_Registro_Mov_Ingreso.Campo_No_Movimiento + "),0)");
            Mi_SQL.Append(" FROM " + Ope_Psp_Registro_Mov_Ingreso.Tabla_Ope_Psp_Registro_Mov_Ingreso);
            No_Movimiento = Convert.ToInt32(OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString().Trim()));
            No_Movimiento = No_Movimiento + 1;

            //obtenemos el no de presipuesto id por si se tiene q insertar un registro no existente
            Mi_SQL = new StringBuilder();
            Mi_SQL.Append("SELECT  NVL(MAX (" + Ope_Psp_Presupuesto_Ingresos.Campo_Presupuesto_Ing_ID + "),0)");
            Mi_SQL.Append(" FROM " + Ope_Psp_Presupuesto_Ingresos.Tabla_Ope_Psp_Presupuesto_Ingresos);
            Registro_Ingreso = Convert.ToInt32(OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()));
            Registro_Ingreso = Registro_Ingreso + 1;

            try
            {
                if (Dt_Datos != null)
                {
                    if (Dt_Datos.Rows.Count > 0)
                    {
                        if (!String.IsNullOrEmpty(No_Poliza.Trim()) && !String.IsNullOrEmpty(Tipo_Poliza_ID.Trim())
                            && !String.IsNullOrEmpty(Mes_Anio.Trim())
                            && !String.IsNullOrEmpty(Momento_Inicial.Trim()) && !String.IsNullOrEmpty(Momento_Final.Trim()))
                        {
                            if ((Momento_Inicial.Trim().Equals(Ope_Psp_Presupuesto_Ingresos.Campo_Devengado)
                                && Momento_Final.Trim().Equals(Ope_Psp_Presupuesto_Ingresos.Campo_Recaudado))
                                ||
                                (Momento_Inicial.Trim().Equals(Ope_Psp_Presupuesto_Ingresos.Campo_Por_Recaudar)
                                && Momento_Final.Trim().Equals(Ope_Psp_Presupuesto_Ingresos.Campo_Recaudado))) 
                            {
                                Por_Recaudar = true;
                                Suma = true;
                            }
                            else if ((Momento_Inicial.Trim().Equals(Ope_Psp_Presupuesto_Ingresos.Campo_Por_Recaudar)
                                && Momento_Final.Trim().Equals(Ope_Psp_Presupuesto_Ingresos.Campo_Devengado))
                                ||
                                (Momento_Inicial.Trim().Equals(Ope_Psp_Presupuesto_Ingresos.Campo_Recaudado)
                                && Momento_Final.Trim().Equals(Ope_Psp_Presupuesto_Ingresos.Campo_Devengado))) 
                            {
                                Por_Recaudar = false;
                                Suma = false;
                            }
                            else if ((Momento_Inicial.Trim().Equals(Ope_Psp_Presupuesto_Ingresos.Campo_Devengado)
                                && Momento_Final.Trim().Equals(Ope_Psp_Presupuesto_Ingresos.Campo_Por_Recaudar))
                                ||
                                (Momento_Inicial.Trim().Equals(Ope_Psp_Presupuesto_Ingresos.Campo_Recaudado)
                                && Momento_Final.Trim().Equals(Ope_Psp_Presupuesto_Ingresos.Campo_Por_Recaudar)))
                            {
                                Por_Recaudar = true;
                                Suma = false;
                            }

                            foreach (DataRow Dr in Dt_Datos.Rows)
                            {
                                if (!String.IsNullOrEmpty(Dr["Fte_Financiamiento_ID"].ToString().Trim()) &&
                                    !String.IsNullOrEmpty(Dr["Concepto_Ing_ID"].ToString().Trim()))
                                {
                                    if (!String.IsNullOrEmpty(Dr["Anio"].ToString().Trim()))
                                    { Anio = Dr["Anio"].ToString().Trim(); }

                                    Concepto = Dr["Concepto_Ing_ID"].ToString().Trim();

                                    Mi_SQL = new StringBuilder();
                                    Mi_SQL.Append("SELECT " + Cat_Psp_Concepto_Ing.Campo_Clase_Ing_ID);
                                    Mi_SQL.Append(" FROM " + Cat_Psp_Concepto_Ing.Tabla_Cat_Psp_Concepto_Ing);
                                    Mi_SQL.Append(" WHERE " + Cat_Psp_Concepto_Ing.Campo_Concepto_Ing_ID + " = '" + Concepto + "'");

                                    Clase = (String)OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

                                    Mi_SQL = new StringBuilder();
                                    Mi_SQL.Append("SELECT " + Cat_Psp_Clase_Ing.Campo_Tipo_ID);
                                    Mi_SQL.Append(" FROM " + Cat_Psp_Clase_Ing.Tabla_Cat_Psp_Clase_Ing);
                                    Mi_SQL.Append(" WHERE " + Cat_Psp_Clase_Ing.Campo_Clase_Ing_ID + " = '" + Clase + "'");

                                    Tipo = (String)OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

                                    Mi_SQL = new StringBuilder();
                                    Mi_SQL.Append("SELECT " + Cat_Psp_Tipo.Campo_Rubro_ID);
                                    Mi_SQL.Append(" FROM " + Cat_Psp_Tipo.Tabla_Cat_Psp_Tipo);
                                    Mi_SQL.Append(" WHERE " + Cat_Psp_Tipo.Campo_Tipo_ID + " = '" + Tipo + "'");

                                    Rubro = (String)OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

                                    if (Convert.ToDouble(String.IsNullOrEmpty(Dr["Importe"].ToString().Trim()) ? "0" : Dr["Importe"].ToString().Trim().Replace(",", "")) > 0)
                                    {
                                        Importe = Convert.ToDouble(String.IsNullOrEmpty(Dr["Importe"].ToString().Trim()) ? "0" : Dr["Importe"].ToString().Trim().Replace(",", ""));

                                        //Primero se consulta si existe un registro en el presupuesto de ingreso 
                                        Mi_SQL = new StringBuilder();
                                        Mi_SQL.Append("SELECT * FROM " + Ope_Psp_Presupuesto_Ingresos.Tabla_Ope_Psp_Presupuesto_Ingresos);
                                        Mi_SQL.Append(" WHERE " + Ope_Psp_Presupuesto_Ingresos.Campo_Fuente_Financiamiento_ID + " = '" + Dr["Fte_Financiamiento_ID"].ToString().Trim() + "' ");
                                        if (!String.IsNullOrEmpty(Dr["Proyecto_Programa_ID"].ToString().Trim()))
                                        {Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Proyecto_Programa_ID + " = '" + Dr["Proyecto_Programa_ID"].ToString().Trim() + "' ");}
                                        Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Anio + " = " + Anio);
                                        Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Rubro_ID + " = '" + Rubro.Trim() + "' ");
                                        Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Tipo_ID + " = '" + Tipo.Trim() + "' ");
                                        Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Clase_Ing_ID + " = '" + Clase.Trim() + "' ");
                                        Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Concepto_Ing_ID + " = '" + Concepto.Trim() + "' ");
                                        if (!String.IsNullOrEmpty(Dr["SubConcepto_Ing_ID"].ToString().Trim()))
                                        {Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_SubConcepto_Ing_ID+ " = '" + Dr["SubConcepto_Ing_ID"].ToString().Trim() + "' ");}
                                        
                                        Dt_Registro = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

                                        if (Dt_Registro != null && Dt_Registro.Rows.Count > 0)
                                        {
                                            Existe_Psp_SubConcepto = true;
                                            SubConcepto = Dr["SubConcepto_Ing_ID"].ToString().Trim();

                                            if (String.IsNullOrEmpty(Dr["SubConcepto_Ing_ID"].ToString().Trim()))
                                            {
                                                Existe_Psp_SubConcepto = false;
                                                SubConcepto = String.Empty;
                                            }
                                        }
                                        else {
                                            Dt_Registro = new DataTable();
                                            Mi_SQL = new StringBuilder();
                                            Existe_Psp_SubConcepto = false;
                                            SubConcepto = String.Empty;

                                            Mi_SQL.Append("SELECT * FROM " + Ope_Psp_Presupuesto_Ingresos.Tabla_Ope_Psp_Presupuesto_Ingresos);
                                            Mi_SQL.Append(" WHERE " + Ope_Psp_Presupuesto_Ingresos.Campo_Fuente_Financiamiento_ID + " = '" + Dr["Fte_Financiamiento_ID"].ToString().Trim() + "' ");
                                            if (!String.IsNullOrEmpty(Dr["Proyecto_Programa_ID"].ToString().Trim()))
                                            { Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Proyecto_Programa_ID + " = '" + Dr["Proyecto_Programa_ID"].ToString().Trim() + "' "); }
                                            Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Anio + " = " + Anio);
                                            Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Rubro_ID + " = '" + Rubro.Trim() + "' ");
                                            Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Tipo_ID + " = '" + Tipo.Trim() + "' ");
                                            Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Clase_Ing_ID + " = '" + Clase.Trim() + "' ");
                                            Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Concepto_Ing_ID + " = '" + Concepto.Trim() + "' ");

                                            Dt_Registro = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                                        }

                                        if (Dt_Registro.Rows.Count > 0)
                                        {
                                            Mi_SQL = new StringBuilder();
                                            Mi_SQL.Append("UPDATE " + Ope_Psp_Presupuesto_Ingresos.Tabla_Ope_Psp_Presupuesto_Ingresos);
                                            Mi_SQL.Append(" SET " + Momento_Final.Trim() + " = " + Momento_Final.Trim() + " + ");
                                            Mi_SQL.Append(Dr["Importe"].ToString().Trim().Replace(",", "") + ", ");
                                            Mi_SQL.Append(Momento_Inicial.Trim() + " = " + Momento_Inicial.Trim() + " - ");
                                            Mi_SQL.Append(Dr["Importe"].ToString().Trim().Replace(",", "") + ", ");

                                            if (Por_Recaudar)
                                            {
                                                switch (Convert.ToInt32(MES))
                                                {
                                                    case 1:
                                                        if (Suma)
                                                        { Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Enero + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Enero + " + "); }
                                                        else
                                                        { Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Enero + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Enero + " - "); }
                                                        break;
                                                    case 2:
                                                        if (Suma)
                                                        { Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Febrero + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Febrero + " + "); }
                                                        else
                                                        { Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Febrero + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Febrero + " - "); }
                                                        break;
                                                    case 3:
                                                        if (Suma)
                                                        { Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Marzo + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Marzo + " + "); }
                                                        else
                                                        { Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Marzo + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Marzo + " - "); }
                                                        break;
                                                    case 4:
                                                        if (Suma)
                                                        { Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Abril + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Abril + " + "); }
                                                        else
                                                        { Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Abril + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Abril + " - "); }
                                                        break;
                                                    case 5:
                                                        if (Suma)
                                                        { Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Mayo + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Mayo + " + "); }
                                                        else
                                                        { Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Mayo + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Mayo + " - "); }
                                                        break;
                                                    case 6:
                                                        if (Suma)
                                                        { Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Junio + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Junio + " + "); }
                                                        else
                                                        { Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Junio + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Junio + " - "); }
                                                        break;
                                                    case 7:
                                                        
                                                        if (Suma)
                                                        { Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Julio + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Julio + " + "); }
                                                        else
                                                        { Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Julio + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Julio + " - "); }
                                                        break;
                                                    case 8:
                                                        if (Suma)
                                                        { Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Agosto + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Agosto + " + "); }
                                                        else
                                                        { Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Agosto + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Agosto + " - "); }
                                                        break;
                                                    case 9:
                                                        if (Suma)
                                                        { Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Septiembre + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Septiembre + " + "); }
                                                        else
                                                        { Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Septiembre + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Septiembre + " - "); }
                                                        break;
                                                    case 10:
                                                        if (Suma)
                                                        { Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Octubre + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Octubre + " + "); }
                                                        else
                                                        { Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Octubre + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Octubre + " - "); }
                                                        break;
                                                    case 11:
                                                        if (Suma)
                                                        { Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Noviembre + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Noviembre + " + "); }
                                                        else
                                                        { Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Noviembre + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Noviembre + " - "); }
                                                        break;
                                                    case 12:
                                                        if (Suma)
                                                        { Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Diciembre + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Diciembre + " + "); }
                                                        else
                                                        { Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Diciembre + " = " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Diciembre + " - "); }
                                                        break;
                                                }
                                                Mi_SQL.Append(Dr["Importe"].ToString().Trim().Replace(",", "") + ", ");
                                            }

                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Fecha_Modifico + " = SYSDATE ");
                                            Mi_SQL.Append(" WHERE " + Ope_Psp_Presupuesto_Ingresos.Campo_Fuente_Financiamiento_ID + " = '" + Dr["Fte_Financiamiento_ID"].ToString().Trim() + "' ");
                                            if (!String.IsNullOrEmpty(Dr["Proyecto_Programa_ID"].ToString().Trim()))
                                            {Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Proyecto_Programa_ID + " = '" + Dr["Proyecto_Programa_ID"].ToString().Trim() + "' ");}
                                            Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Anio + " = " + Anio);
                                            Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Rubro_ID + " = '" + Rubro.Trim() + "' ");
                                            Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Tipo_ID + " = '" + Tipo.Trim() + "' ");
                                            Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Clase_Ing_ID + " = '" + Clase.Trim() + "' ");
                                            Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_Concepto_Ing_ID + " = '" + Concepto.Trim() + "' ");
                                            if (Existe_Psp_SubConcepto) 
                                            {Mi_SQL.Append(" AND " + Ope_Psp_Presupuesto_Ingresos.Campo_SubConcepto_Ing_ID + " = '" + Dr["SubConcepto_Ing_ID"].ToString().Trim() + "' ");}
                                            Cmd.CommandText = Mi_SQL.ToString();
                                            Cmd.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            //Se inserta el registro ya que no existe en el presupuesto
                                            Mi_SQL = new StringBuilder();
                                            Mi_SQL.Append("INSERT INTO " + Ope_Psp_Presupuesto_Ingresos.Tabla_Ope_Psp_Presupuesto_Ingresos + " (");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Presupuesto_Ing_ID + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Fuente_Financiamiento_ID + ", ");
                                            if (!String.IsNullOrEmpty(Dr["Proyecto_Programa_ID"].ToString().Trim()))
                                            { Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Proyecto_Programa_ID + ", "); }
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Rubro_ID + ", " + Ope_Psp_Presupuesto_Ingresos.Campo_Tipo_ID + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Clase_Ing_ID + ", " + Ope_Psp_Presupuesto_Ingresos.Campo_Concepto_Ing_ID + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_SubConcepto_Ing_ID + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Anio + ", " + Ope_Psp_Presupuesto_Ingresos.Campo_Importe_Enero + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Importe_Febrero + ", " + Ope_Psp_Presupuesto_Ingresos.Campo_Importe_Marzo + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Importe_Abril + ", " + Ope_Psp_Presupuesto_Ingresos.Campo_Importe_Mayo + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Importe_Junio + ", " + Ope_Psp_Presupuesto_Ingresos.Campo_Importe_Julio + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Importe_Agosto + ", " + Ope_Psp_Presupuesto_Ingresos.Campo_Importe_Septiembre + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Importe_Octubre + ", " + Ope_Psp_Presupuesto_Ingresos.Campo_Importe_Noviembre + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Importe_Diciembre + ", " + Ope_Psp_Presupuesto_Ingresos.Campo_Importe_Total + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Aprobado + ", " + Ope_Psp_Presupuesto_Ingresos.Campo_Ampliacion + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Reduccion + ", " + Ope_Psp_Presupuesto_Ingresos.Campo_Modificado + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Devengado + ", " + Ope_Psp_Presupuesto_Ingresos.Campo_Recaudado + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Por_Recaudar + ", " + Ope_Psp_Presupuesto_Ingresos.Campo_Saldo + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Enero + ", " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Febrero + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Marzo + ", " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Abril + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Mayo + ", " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Junio + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Julio + ", " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Agosto + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Septiembre + ", " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Octubre + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Noviembre + ", " + Ope_Psp_Presupuesto_Ingresos.Campo_Acumulado_Diciembre + ", ");
                                            Mi_SQL.Append(Ope_Psp_Presupuesto_Ingresos.Campo_Usuario_Creo + ", " + Ope_Psp_Presupuesto_Ingresos.Campo_Fecha_Creo + ") ");
                                            Mi_SQL.Append("VALUES (" + Registro_Ingreso + ", '" + Dr["Fte_Financiamiento_ID"].ToString().Trim() + "', ");
                                            if (!String.IsNullOrEmpty(Dr["Proyecto_Programa_ID"].ToString().Trim()))
                                            { Mi_SQL.Append("'" + Dr["Proyecto_Programa_ID"].ToString().Trim() + "', "); }
                                            Mi_SQL.Append("'" + Rubro.Trim() + "', '" + Tipo.Trim() + "', '");
                                            Mi_SQL.Append(Clase.Trim() + "', '" + Concepto.Trim() + "', NULL," + Anio + ", ");
                                            Mi_SQL.Append("0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00,");
                                            if (Momento_Inicial.Trim().Equals(Ope_Psp_Presupuesto_Ingresos.Campo_Por_Recaudar)
                                                && Momento_Final.Trim().Equals(Ope_Psp_Presupuesto_Ingresos.Campo_Recaudado))
                                            {
                                                Mi_SQL.Append(" 0.00, " + Dr["Importe"].ToString().Trim().Replace(",", "") + ", ");
                                                Mi_SQL.Append("-" + Dr["Importe"].ToString().Trim().Replace(",", "") + ", 0.00, ");
                                            }
                                            if (Momento_Inicial.Trim().Equals(Ope_Psp_Presupuesto_Ingresos.Campo_Devengado)
                                            && Momento_Final.Trim().Equals(Ope_Psp_Presupuesto_Ingresos.Campo_Recaudado))
                                            {
                                                Mi_SQL.Append("-" + Dr["Importe"].ToString().Trim().Replace(",", "") + ", " + Dr["Importe"].ToString().Trim().Replace(",", "") + ", ");
                                                Mi_SQL.Append(" 0.00, 0.00, ");
                                            }
                                            else if (Momento_Inicial.Trim().Equals(Ope_Psp_Presupuesto_Ingresos.Campo_Por_Recaudar)
                                                && Momento_Final.Trim().Equals(Ope_Psp_Presupuesto_Ingresos.Campo_Devengado))
                                            {
                                                Mi_SQL.Append(Dr["Importe"].ToString().Trim().Replace(",", "") + ", 0.00,");
                                                Mi_SQL.Append("-" + Dr["Importe"].ToString().Trim().Replace(",", "") + ", 0.00, ");
                                            }
                                            else if (Momento_Inicial.Trim().Equals(Ope_Psp_Presupuesto_Ingresos.Campo_Recaudado)
                                                && Momento_Final.Trim().Equals(Ope_Psp_Presupuesto_Ingresos.Campo_Devengado))
                                            {
                                                Mi_SQL.Append(Dr["Importe"].ToString().Trim().Replace(",", "") + ", -" + Dr["Importe"].ToString().Trim().Replace(",", "") + ", ");
                                                Mi_SQL.Append(" 0.00, 0.00, ");
                                            }
                                            if (Momento_Inicial.Trim().Equals(Ope_Psp_Presupuesto_Ingresos.Campo_Devengado)
                                                && Momento_Final.Trim().Equals(Ope_Psp_Presupuesto_Ingresos.Campo_Por_Recaudar))
                                            {
                                                Mi_SQL.Append("-" + Dr["Importe"].ToString().Trim().Replace(",", "") + ", 0.00,");
                                                Mi_SQL.Append( Dr["Importe"].ToString().Trim().Replace(",", "") + ", 0.00, ");
                                            }
                                            if (Momento_Inicial.Trim().Equals(Ope_Psp_Presupuesto_Ingresos.Campo_Recaudado)
                                                && Momento_Final.Trim().Equals(Ope_Psp_Presupuesto_Ingresos.Campo_Por_Recaudar))
                                            {
                                                Mi_SQL.Append(" 0.00, -" + Dr["Importe"].ToString().Trim().Replace(",", "") + ", ");
                                                Mi_SQL.Append(Dr["Importe"].ToString().Trim().Replace(",", "") + ", 0.00, ");
                                            }

                                            if (Por_Recaudar)
                                            {
                                                switch (Convert.ToInt32(MES))
                                                {
                                                    case 1:
                                                        if (Suma)
                                                        { Mi_SQL.Append(Dr["Importe"].ToString().Trim().Replace(",", "") + ", 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00,"); }
                                                        else
                                                        { Mi_SQL.Append("-" + Dr["Importe"].ToString().Trim().Replace(",", "") + ", 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00,"); }
                                                        break;
                                                    case 2:
                                                        if (Suma)
                                                        { Mi_SQL.Append("0.00," + Dr["Importe"].ToString().Trim().Replace(",", "") + ", 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00,"); }
                                                        else
                                                        { Mi_SQL.Append("0.00, -" + Dr["Importe"].ToString().Trim().Replace(",", "") + ", 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00,"); }
                                                        break;
                                                    case 3:
                                                        if (Suma)
                                                        { Mi_SQL.Append("0.00, 0.00," + Dr["Importe"].ToString().Trim().Replace(",", "") + ", 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00,"); }
                                                        else
                                                        { Mi_SQL.Append("0.00, 0.00, -" + Dr["Importe"].ToString().Trim().Replace(",", "") + ", 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00,"); }
                                                        break;
                                                    case 4:
                                                        if (Suma)
                                                        { Mi_SQL.Append("0.00, 0.00, 0.00," + Dr["Importe"].ToString().Trim().Replace(",", "") + ", 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00,"); }
                                                        else
                                                        { Mi_SQL.Append("0.00, 0.00, 0.00, -" + Dr["Importe"].ToString().Trim().Replace(",", "") + ", 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00,"); }
                                                        break;
                                                    case 5:
                                                        if (Suma)
                                                        { Mi_SQL.Append("0.00, 0.00, 0.00, 0.00, " + Dr["Importe"].ToString().Trim().Replace(",", "") + ", 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00,"); }
                                                        else
                                                        { Mi_SQL.Append("0.00,  0.00, 0.00, 0.00, -" + Dr["Importe"].ToString().Trim().Replace(",", "") + ", 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00,"); }
                                                        break;
                                                    case 6:
                                                        if (Suma)
                                                        { Mi_SQL.Append("0.00, 0.00, 0.00, 0.00, 0.00," + Dr["Importe"].ToString().Trim().Replace(",", "") + ", 0.00, 0.00, 0.00, 0.00, 0.00, 0.00,"); }
                                                        else
                                                        { Mi_SQL.Append("0.00, 0.00, 0.00, 0.00, 0.00, -" + Dr["Importe"].ToString().Trim().Replace(",", "") + ", 0.00, 0.00, 0.00, 0.00, 0.00, 0.00,"); }
                                                        break;
                                                    case 7:
                                                        if (Suma)
                                                        { Mi_SQL.Append("0.00, 0.00, 0.00, 0.00, 0.00, 0.00," + Dr["Importe"].ToString().Trim().Replace(",", "") + ", 0.00, 0.00, 0.00, 0.00, 0.00,"); }
                                                        else
                                                        { Mi_SQL.Append("0.00, 0.00, 0.00, 0.00, 0.00, 0.00, -" + Dr["Importe"].ToString().Trim().Replace(",", "") + ", 0.00, 0.00, 0.00, 0.00, 0.00,"); }
                                                        break;
                                                    case 8:
                                                        if (Suma)
                                                        { Mi_SQL.Append("0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00," + Dr["Importe"].ToString().Trim().Replace(",", "") + ", 0.00, 0.00, 0.00, 0.00,"); }
                                                        else
                                                        { Mi_SQL.Append("0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, -" + Dr["Importe"].ToString().Trim().Replace(",", "") + ", 0.00, 0.00, 0.00, 0.00,"); }
                                                        break;
                                                    case 9:
                                                        if (Suma)
                                                        { Mi_SQL.Append("0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00," + Dr["Importe"].ToString().Trim().Replace(",", "") + ", 0.00, 0.00, 0.00,"); }
                                                        else
                                                        { Mi_SQL.Append("0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, -" + Dr["Importe"].ToString().Trim().Replace(",", "") + ", 0.00, 0.00, 0.00,"); }
                                                        break;
                                                    case 10:
                                                        if (Suma)
                                                        { Mi_SQL.Append("0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00," + Dr["Importe"].ToString().Trim().Replace(",", "") + ", 0.00, 0.00,"); }
                                                        else
                                                        { Mi_SQL.Append("0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, -" + Dr["Importe"].ToString().Trim().Replace(",", "") + ", 0.00, 0.00,"); }
                                                        break;
                                                    case 11:
                                                        if (Suma)
                                                        { Mi_SQL.Append("0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00," + Dr["Importe"].ToString().Trim().Replace(",", "") + ", 0.00,"); }
                                                        else
                                                        { Mi_SQL.Append("0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, -" + Dr["Importe"].ToString().Trim().Replace(",", "") + ", 0.00,"); }
                                                        break;
                                                    case 12:
                                                        if (Suma)
                                                        { Mi_SQL.Append("0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00," + Dr["Importe"].ToString().Trim().Replace(",", "") + ", "); }
                                                        else
                                                        { Mi_SQL.Append("0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, -" + Dr["Importe"].ToString().Trim().Replace(",", "") + ", "); }
                                                        break;
                                                }
                                            }
                                            else {
                                                Mi_SQL.Append("0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00,");
                                            }
                                            Mi_SQL.Append("'" + Cls_Sessiones.Nombre_Empleado.ToString() + "',SYSDATE)");
                                            Cmd.CommandText = Mi_SQL.ToString();
                                            Cmd.ExecuteNonQuery();

                                            Registro_Ingreso++;
                                        }
                                        Operacion_Completa = true;
                                    }

                                    //insertamos los registros de los movimientos del presupuesto
                                    Insertar_Registro_Mov_Ing(Momento_Final.Trim(), Momento_Inicial.Trim(), Importe.ToString().Trim(),
                                        String.Empty, Referencia.Trim(), No_Poliza.Trim(), Tipo_Poliza_ID.Trim(),
                                        Mes_Anio.Trim(), Dr["Fte_Financiamiento_ID"].ToString().Trim(), Dr["Proyecto_Programa_ID"].ToString().Trim(), Rubro.Trim(),
                                        Tipo.Trim(), Clase.Trim(), Concepto.Trim(), SubConcepto.Trim(), Cls_Sessiones.Nombre_Empleado.Trim(), Cmd, No_Movimiento);

                                    No_Movimiento++;
                                }
                            }
                        }
                        if (Cmmd == null)
                        {
                            Trans.Commit();
                        }

                        Operacion_Completa = true;
                    }
                }
            }
            catch (Exception Ex)
            {
                if (Cmmd == null)
                {
                    Trans.Rollback();
                }
                throw new Exception("Error al ejecutar los el alta de la poliza de ingresos. Error: [" + Ex.Message + "]");
            }
            finally
            {
                if (Cmmd == null)
                {
                    Cn.Close();
                }
            }
            return Operacion_Completa;
        }
        #endregion
    }
}


