using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Data;
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
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio;
using Presidencia.Caja_Pagos.Negocio;
using Presidencia.Operacion_Resumen_Predio.Negocio;
using Operacion_Predial_Orden_Variacion.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Catalogo_Cuotas_Minimas.Negocio;
using Presidencia.Catalogo_Tabulador_Recargos.Negocio;


public partial class paginas_Predial_Ventanas_Emergentes_Resumen_Predial_Frm_Adeudo_Diferencias : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Grid_Estado_Cuenta.DataBind();
            Cargar_Datos_Orden_Variacion();
        }
    }
    private void Cargar_Datos_Orden_Variacion()
    {
        try
        {
            Txt_Cuenta_Predial.Text = "";
            Txt_Cuenta_Predial.Text = Request.QueryString["Cuenta_Predial"].ToString();// (Session["Cuenta_Predial"] != null) ? Session["Cuenta_Predial"].ToString().Trim() : "";

            if (Request.QueryString["Consulta_Adeudos_Cancelados"] != null)
            {
                Cargar_Adeudos_Cancelados_Diferencias(0, (DataTable)Session["Dt_Agregar_Diferencias"]);
            }
            else
            {
                Cargar_Adeudos_Activos_Diferencias(0, (DataTable)Session["Dt_Agregar_Diferencias"]);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Encabezado_Error.Text = Ex.Message;
            Lbl_Encabezado_Error.Visible = true;
            Img_Error.Visible = true;

        }
    }

    private Double Importe_Adeudos_Tipo(DataTable Dt_Adeudos, String Tipo)
    {
        Double Adeudo_Total = 0.0;
        if (Dt_Adeudos != null && Dt_Adeudos.Rows.Count > 0)
        {
            for (Int32 Contador = 0; Contador < Dt_Adeudos.Rows.Count; Contador++)
            {
                if (Dt_Adeudos.Rows[Contador]["TIPO_PERIODO"].ToString().Trim().Equals(Tipo))
                {
                    if (Dt_Adeudos.Rows[Contador]["TIPO"].ToString().Trim().Equals("ALTA"))
                    {
                        Adeudo_Total = Adeudo_Total + Convert.ToDouble(Dt_Adeudos.Rows[Contador]["IMPORTE"].ToString());
                    }
                    else
                    {
                        Adeudo_Total = Adeudo_Total - Convert.ToDouble(Dt_Adeudos.Rows[Contador]["IMPORTE"].ToString());
                    }
                }
            }
        }
        return Adeudo_Total;
    }

    private DataTable Crear_Dt_Adeudos(DataTable Datos)
    {
        DataTable Dt_Datos = new DataTable();
        Dt_Datos.Columns.Add("Anio", Type.GetType("System.Int32"));
        Dt_Datos.Columns.Add("Adeudo_Bimestre_1", Type.GetType("System.Double"));
        Dt_Datos.Columns.Add("Adeudo_Bimestre_2", Type.GetType("System.Double"));
        Dt_Datos.Columns.Add("Adeudo_Bimestre_3", Type.GetType("System.Double"));
        Dt_Datos.Columns.Add("Adeudo_Bimestre_4", Type.GetType("System.Double"));
        Dt_Datos.Columns.Add("Adeudo_Bimestre_5", Type.GetType("System.Double"));
        Dt_Datos.Columns.Add("Adeudo_Bimestre_6", Type.GetType("System.Double"));
        Dt_Datos.Columns.Add("Adeudo_Total_Anio", Type.GetType("System.Double"));
        Dt_Datos.Columns.Add("Alta_Baja", Type.GetType("System.String"));
        for (Int32 Contador = 0; Contador < Datos.Rows.Count; Contador++)
        {
            DataTable Tmp = Crear_DataTable_DataRow(Datos.Rows[Contador]);
            for (Int32 C = 0; C < Tmp.Rows.Count; C++)
            {
                Int32 No_Fila = Obtener_Fila_Adeudo(Dt_Datos, Tmp.Rows[C]["ANIO"].ToString());
                if (No_Fila == (-1))
                {
                    DataRow Fila = Dt_Datos.NewRow();
                    Fila["Anio"] = Tmp.Rows[C]["ANIO"].ToString();
                    Fila["Adeudo_Bimestre_" + Tmp.Rows[C]["BIMESTRE"].ToString()] = Tmp.Rows[C]["ADEUDO"].ToString();
                    Fila["Alta_Baja"] = Datos.Rows[Contador]["TIPO"].ToString().Trim();
                    Dt_Datos.Rows.Add(Fila);
                }
                else
                {
                    Dt_Datos.DefaultView.AllowEdit = true;
                    Dt_Datos.Rows[No_Fila].BeginEdit();
                    Dt_Datos.Rows[No_Fila]["Adeudo_Bimestre_" + Tmp.Rows[C]["BIMESTRE"].ToString()] = Tmp.Rows[C]["ADEUDO"].ToString();
                    Dt_Datos.Rows[No_Fila].EndEdit();
                }
            }
        }
        return Dt_Datos;
    }

    private DataTable Crear_DataTable_DataRow(DataRow Fila)
    {
        //String Periodo_Inicial = Session["Periodo"].ToString().Trim();
        //String Anio = Session["Anio"].ToString().Trim();
        DataTable Dt_Datos = new DataTable();
        Dt_Datos.Columns.Add("ANIO", Type.GetType("System.Int32"));
        Dt_Datos.Columns.Add("BIMESTRE", Type.GetType("System.Int32"));
        Dt_Datos.Columns.Add("ADEUDO", Type.GetType("System.Double"));
        String Periodos = Fila["PERIODO"].ToString();
        String Cuota_Bimestral = Fila["Importe"].ToString();
        String Periodo_Inicial = Periodos.Split('-')[0].Trim();
        String Periodo_Final = Periodos.Split('-')[1].Trim();
        String Anio = Periodo_Inicial.Split('/')[1].Trim();
        String Bimestre_Inicial = Periodo_Inicial.Split('/')[0].Trim();
        String Bimestre_Final = Periodo_Final.Split('/')[0].Trim();
        for (Int32 Contador = Convert.ToInt32(Bimestre_Inicial); Contador <= Convert.ToInt32(Bimestre_Final); Contador++)
        {
            DataRow Dr_Datos = Dt_Datos.NewRow();
            Dr_Datos["ANIO"] = Anio;
            Dr_Datos["BIMESTRE"] = Contador;
            Dr_Datos["ADEUDO"] = Convert.ToDouble(Cuota_Bimestral) / 6;
            Dt_Datos.Rows.Add(Dr_Datos);
        }
        return Dt_Datos;
    }

    private Int32 Obtener_Fila_Adeudo(DataTable Dt_Datos, String Buscar)
    {
        Int32 Fila = (-1);
        for (Int32 Contador = 0; Contador < Dt_Datos.Rows.Count; Contador++)
        {
            if (Buscar.Trim().Equals(Dt_Datos.Rows[Contador]["Anio"].ToString().Trim()))
            {
                Fila = Contador;
                break;
            }
        }
        return Fila;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN      : Cargar_Adeudos_Activos_Diferencias
    /// DESCRIPCIÓN         : Lee adeudos de la cuenta y adeudos en analisis de rezago y los muestra según los casos que apliquen
    /// PARÁMETROS:
    /// CREO                : Antonio Salvador Benavides Guardado
    /// FECHA_CREO          : 23/Nov/2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Cargar_Adeudos_Activos_Diferencias(int Page_Index, DataTable Dt_Diferencias_Adeudos)
    {
        try
        {
            Cls_Cat_Pre_Cuotas_Minimas_Negocio Cuotas_Minimas = new Cls_Cat_Pre_Cuotas_Minimas_Negocio();
            Cls_Ope_Pre_Resumen_Predio_Negocio Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio();
            Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
            Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Adeudos_Predial = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
            Cls_Cat_Pre_Tabulador_Recargos_Negocio Recargos = new Cls_Cat_Pre_Tabulador_Recargos_Negocio();

            DataTable Dt_Adeudos_Cuenta = null;
            DataTable Dt_Adeudos = null;

            Dictionary<String, Decimal> Dic_Adeudos_Diferencias = new Dictionary<string, decimal>();
            Dictionary<String, Decimal> Dicc_Tabulador_Recargos = new Dictionary<String, Decimal>();

            String Cuenta_Predial_ID = "";
            String Orden_Variacion_ID = "";
            String Anio_Orden = "";
            String Periodo = "";
            Decimal Cuota_Bimestral = 0;
            Boolean Periodo_Corriente_Validado = false;
            Boolean Periodo_Rezago_Validado = false;
            //Boolean Cuotas_Minimas_Encontradas_Año = false;
            //Boolean Cuotas_Minimas_Encontradas_Periodo = false;
            Decimal Sum_Adeudos_Año = 0;
            Decimal Sum_Adeudos_Periodo = 0;
            int Cont_Adeudos_Año = 0;
            int Cont_Adeudos_Periodo = 0;
            int Cont_Cuotas_Minimas_Año = 0;
            int Cont_Cuotas_Minimas_Periodo = 0;
            int Desde_Bimestre = 0;
            int Hasta_Bimestre = 0;
            int Cont_Bimestres = 0;
            int Año_Periodo = 0;
            int Desde_Anio = DateTime.Now.Year + 1;
            int Hasta_Anio = 0;
            int Tmp_Desde_Anio = 0;
            int Tmp_Hasta_Anio = 0;
            int Mes_Actual = DateTime.Now.Month;
            int Anio_Actual = DateTime.Now.Year;
            int Signo = 1;
            Decimal Cuota_Minima_Año = 0;
            Decimal Cuota_Fija = 0;
            Boolean Nueva_Cuota_Fija = false;
            Decimal Importe_Rezago = 0;
            Decimal Total_Adeudo_Impuesto = 0;
            Decimal Valor_Fiscal = 0;
            Decimal Tasa_Diferencias = 0;
            String Periodo_Inicial = "-";
            String Periodo_Final = "-";
            Double Cuota_Fija_Nueva = 0;
            Double Cuota_Fija_Anterior = 0;

            if (Session["Cuenta_Predial_ID_Adeudos"] != null)
            {
                Cuenta_Predial_ID = Session["Cuenta_Predial_ID_Adeudos"].ToString().Trim();
            }

            if (Session["Orden_Variacion_ID_Adeudos"] != null)
            {
                Orden_Variacion_ID = Session["Orden_Variacion_ID_Adeudos"].ToString().Trim();
            }
            if (Request.QueryString["Anio_Orden"] != null)
            {
                Anio_Orden = Request.QueryString["Anio_Orden"].ToString();
            }
            else
            {
                if (Session["Anio_Orden_Adeudos"] != null)
                {
                    Anio_Orden = Session["Anio_Orden_Adeudos"].ToString().Trim();
                }
            }

            if (Session["Cuota_Fija_Nueva"] != null)
            {
                if (Session["Cuota_Fija_Nueva"].ToString().Trim() != "")
                {
                    Cuota_Fija_Nueva = Convert.ToDouble(Session["Cuota_Fija_Nueva"]);
                }
            }
            if (Session["Cuota_Fija_Anterior"] != null)
            {
                if (Session["Cuota_Fija_Anterior"].ToString().Trim() != "")
                {
                    Cuota_Fija_Anterior = Convert.ToDouble(Session["Cuota_Fija_Anterior"]);
                }
            }

            if (Cuota_Fija_Nueva != 0
                && Cuota_Fija_Nueva != Cuota_Fija_Anterior)
            {
                Nueva_Cuota_Fija = true;
            }

            if (Dt_Diferencias_Adeudos == null)
            {
                Orden_Variacion.P_Cuenta_Predial_ID = Cuenta_Predial_ID;
                Orden_Variacion.P_Generar_Orden_No_Orden = Orden_Variacion_ID;
                Orden_Variacion.P_Generar_Orden_Anio = Anio_Orden;
                //Orden_Variacion.P_Ordenar_Dinamico = "TIPO DESC, SUBSTR(de." + Ope_Pre_Diferencias_Detalle.Campo_Periodo + ", LENGTH(de." + Ope_Pre_Diferencias_Detalle.Campo_Periodo + ") - 3, 4), ";
                //Orden_Variacion.P_Ordenar_Dinamico += "de." + Ope_Pre_Diferencias_Detalle.Campo_Periodo;
                Dt_Diferencias_Adeudos = Orden_Variacion.Consulta_Diferencias();
            }
            if (Dt_Diferencias_Adeudos != null)
            {
                Dt_Diferencias_Adeudos.DefaultView.Sort = "TIPO DESC";
                Dt_Diferencias_Adeudos = Dt_Diferencias_Adeudos.DefaultView.ToTable();
            }
            Dt_Adeudos_Cuenta = Adeudos_Predial.Calcular_Recargos_Predial(Cuenta_Predial_ID);

            if (Dt_Adeudos_Cuenta != null)
            {
                //Procesa cada Adeudo de la Cuenta para agregarlo al Diccionario
                foreach (DataRow Fila_Adeudos in Dt_Adeudos_Cuenta.Rows)
                {
                    Periodo = Fila_Adeudos[Ope_Pre_Diferencias_Detalle.Campo_Periodo].ToString().Trim();
                    Cuota_Bimestral = Convert.ToDecimal(Fila_Adeudos["ADEUDO"]);
                    Dic_Adeudos_Diferencias.Add(Periodo, Cuota_Bimestral);
                    Desde_Anio = Convert.ToInt16(Dic_Adeudos_Diferencias.First().Key.Substring(Dic_Adeudos_Diferencias.First().Key.Length - 4));
                    Hasta_Anio = Convert.ToInt16(Dic_Adeudos_Diferencias.Last().Key.Substring(Dic_Adeudos_Diferencias.First().Key.Length - 4));
                }
            }

            if (Dt_Diferencias_Adeudos != null)
            {
                if (Obtener_Anio_Minimo_Maximo(out Tmp_Desde_Anio, out Tmp_Hasta_Anio, Dt_Diferencias_Adeudos, Ope_Pre_Diferencias_Detalle.Campo_Periodo, 2))
                {
                    // si el valor regresado por la funcion es menor, asignar, si no, dejar el mismo
                    if (Tmp_Desde_Anio < Desde_Anio)
                    {
                        Desde_Anio = Tmp_Desde_Anio;
                    }
                    // si el valor regresado por la funcion es mayor, asignarlo a la variable, si no, dejar el mismo
                    if (Tmp_Hasta_Anio > Hasta_Anio)
                    {
                        Hasta_Anio = Tmp_Hasta_Anio;
                    }
                }

                //Procesa cada Adeudo de la Orden para Sumarlos al Diccionario
                foreach (DataRow Fila_Adeudos in Dt_Diferencias_Adeudos.Rows)
                {
                    if (Fila_Adeudos["TIPO"] != null)
                    {
                        if (Fila_Adeudos["TIPO"].ToString().Trim() != "")
                        {
                            if (Fila_Adeudos["TIPO"].ToString().Trim() == "ALTA")
                            {
                                Signo = 1;
                            }
                            else
                            {
                                if (Fila_Adeudos["TIPO"].ToString().Trim() == "BAJA")
                                {
                                    Signo = -1;
                                }
                            }
                        }
                    }
                    Año_Periodo = Convert.ToInt16(Fila_Adeudos[Ope_Pre_Diferencias_Detalle.Campo_Periodo].ToString().Substring(Fila_Adeudos[Ope_Pre_Diferencias_Detalle.Campo_Periodo].ToString().Length - 4));
                    Cuota_Minima_Año = Convert.ToDecimal(Cuotas_Minimas.Consultar_Cuota_Minima_Anio(Año_Periodo.ToString()));
                    if (Fila_Adeudos[Ope_Pre_Diferencias_Detalle.Campo_Importe] != null)
                    {
                        if (Fila_Adeudos[Ope_Pre_Diferencias_Detalle.Campo_Importe].ToString() != "")
                        {
                            Importe_Rezago = Convert.ToDecimal(Fila_Adeudos[Ope_Pre_Diferencias_Detalle.Campo_Importe]);
                            Valor_Fiscal = Convert.ToDecimal(Fila_Adeudos[Ope_Pre_Diferencias_Detalle.Campo_Valor_Fiscal]);
                            Tasa_Diferencias = Convert.ToDecimal(Fila_Adeudos["TASA"]) / 1000;
                            Periodo = Obtener_Periodos_Bimestre(Fila_Adeudos[Ope_Pre_Diferencias_Detalle.Campo_Periodo].ToString(), out Periodo_Corriente_Validado, out Periodo_Rezago_Validado);
                            if (Periodo.Trim() != "")
                            {
                                Desde_Bimestre = Convert.ToInt32(Periodo.Split('-').GetValue(0));
                                Hasta_Bimestre = Convert.ToInt32(Periodo.Split('-').GetValue(1));

                                //Cuotas_Minimas_Encontradas_Año = false;
                                Cont_Cuotas_Minimas_Año = 0;
                                Cont_Adeudos_Año = 0;
                                Sum_Adeudos_Año = 0;
                                //Cuotas_Minimas_Encontradas_Periodo = false;
                                Cont_Cuotas_Minimas_Periodo = 0;
                                Cont_Adeudos_Periodo = 0;
                                Sum_Adeudos_Periodo = 0;

                                Dt_Adeudos_Cuenta = Resumen_Predio.Consultar_Adeudos_Cuenta_Predial_Con_Totales(Cuenta_Predial_ID, null, Año_Periodo, Año_Periodo);
                                if (Dt_Adeudos_Cuenta != null)
                                {
                                    if (Dt_Adeudos_Cuenta.Rows.Count > 0)
                                    {
                                        //Contador de los Adeudos/Cuotas del Año
                                        for (Cont_Bimestres = 1; Cont_Bimestres <= 6; Cont_Bimestres++)
                                        {
                                            if (Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres] != System.DBNull.Value)
                                            {
                                                if (Convert.ToDecimal(Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres]) == Cuota_Minima_Año)
                                                {
                                                    //Cuotas_Minimas_Encontradas_Año = true;
                                                    Cont_Cuotas_Minimas_Año += 1;
                                                }
                                                if (Convert.ToDecimal(Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres]) != 0)
                                                {
                                                    Cont_Adeudos_Año += 1;
                                                    Sum_Adeudos_Año += Convert.ToDecimal(Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres]);
                                                }
                                            }
                                        }
                                        //Contador de los Adeudos/Cuotas del Periodo indicado
                                        for (Cont_Bimestres = Desde_Bimestre; Cont_Bimestres <= Hasta_Bimestre; Cont_Bimestres++)
                                        {
                                            if (Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres] != System.DBNull.Value)
                                            {
                                                if (Convert.ToDecimal(Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres]) == Cuota_Minima_Año)
                                                {
                                                    //Cuotas_Minimas_Encontradas_Periodo = true;
                                                    Cont_Cuotas_Minimas_Periodo += 1;
                                                }
                                                if (Convert.ToDecimal(Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres]) != 0)
                                                {
                                                    Cont_Adeudos_Periodo += 1;
                                                    Sum_Adeudos_Periodo += Convert.ToDecimal(Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres]);
                                                }
                                            }
                                        }
                                    }
                                }

                                //VALIDACIONES PARA CASOS DE CUOTAS MÍNIMAS Y APLICACIÓN DE ADEUDOS
                                //if (Cont_Cuotas_Minimas_Periodo == 1 && Importe_Rezago != Cuota_Minima_Año && !Nueva_Cuota_Fija)
                                //{
                                //    //SUMA LA CUOTA MÍNIMA AL IMPORTE Y EL RESULTADO LO PRORRATEA EN EL PERIODO INDICADO
                                //    for (Cont_Bimestres = Desde_Bimestre; Cont_Bimestres <= Hasta_Bimestre; Cont_Bimestres++)
                                //    {
                                //        if (!Dic_Adeudos_Diferencias.ContainsKey(Cont_Bimestres.ToString() + Año_Periodo.ToString()))
                                //        {
                                //            Dic_Adeudos_Diferencias.Add(Cont_Bimestres.ToString() + Año_Periodo.ToString(), Math.Round((Importe_Rezago + Cuota_Minima_Año) / (Hasta_Bimestre - Desde_Bimestre + 1) * Signo, 2));
                                //        }
                                //        else
                                //        {
                                //            Dic_Adeudos_Diferencias[Cont_Bimestres.ToString() + Año_Periodo.ToString()] = Math.Round((Importe_Rezago + Cuota_Minima_Año) / (Hasta_Bimestre - Desde_Bimestre + 1) * Signo, 2);
                                //        }
                                //    }
                                //}
                                //else
                                {
                                    if (((Importe_Rezago == Cuota_Minima_Año)
                                            || (((Sum_Adeudos_Periodo - Importe_Rezago) == Cuota_Minima_Año && Signo < 0)))
                                        && !Nueva_Cuota_Fija
                                        && !(Importe_Rezago == Cuota_Minima_Año && (Hasta_Bimestre - Desde_Bimestre + 1) == 1))
                                    {
                                        //APLICA LA CUOTA MÍNIMA EN EL PRIMER BIMESTRE INDICADO, EL RESTO DE BIMESTRES LOS DEJA EN CEROS
                                        for (Cont_Bimestres = Desde_Bimestre; Cont_Bimestres <= Hasta_Bimestre; Cont_Bimestres++)
                                        {
                                            if (Cont_Bimestres > Desde_Bimestre && Cuota_Minima_Año != 0)
                                            {
                                                Cuota_Minima_Año = 0;
                                            }
                                            if (!Dic_Adeudos_Diferencias.ContainsKey(Cont_Bimestres.ToString() + Año_Periodo.ToString()))
                                            {
                                                Dic_Adeudos_Diferencias.Add(Cont_Bimestres.ToString() + Año_Periodo.ToString(), Math.Round(Cuota_Minima_Año, 2));
                                            }
                                            else
                                            {
                                                if (Importe_Rezago == Cuota_Minima_Año || Signo > 0)
                                                {
                                                    Dic_Adeudos_Diferencias[Cont_Bimestres.ToString() + Año_Periodo.ToString()] += Math.Round(Cuota_Minima_Año * Signo, 2);
                                                }
                                                else
                                                {
                                                    Dic_Adeudos_Diferencias[Cont_Bimestres.ToString() + Año_Periodo.ToString()] = Math.Round(Cuota_Minima_Año, 2);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if ((Valor_Fiscal * Tasa_Diferencias) <= Cuota_Minima_Año
                                            && (Sum_Adeudos_Periodo + Importe_Rezago) == Cuota_Minima_Año
                                            && Signo > 0)
                                        {
                                            //APLICA LA CUOTA MÍNIMA EN EL PRIMER BIMESTRE INDICADO, EL RESTO DE BIMESTRES LOS DEJA EN CEROS
                                            for (Cont_Bimestres = Desde_Bimestre; Cont_Bimestres <= Hasta_Bimestre; Cont_Bimestres++)
                                            {
                                                if (Cont_Bimestres > Desde_Bimestre && Cuota_Minima_Año != 0)
                                                {
                                                    Cuota_Minima_Año = 0;
                                                }
                                                if (!Dic_Adeudos_Diferencias.ContainsKey(Cont_Bimestres.ToString() + Año_Periodo.ToString()))
                                                {
                                                    Dic_Adeudos_Diferencias.Add(Cont_Bimestres.ToString() + Año_Periodo.ToString(), Math.Round(Cuota_Minima_Año, 2));
                                                }
                                                else
                                                {
                                                    Dic_Adeudos_Diferencias[Cont_Bimestres.ToString() + Año_Periodo.ToString()] = Math.Round(Cuota_Minima_Año, 2);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (Nueva_Cuota_Fija && Signo < 0)
                                            {
                                                //APLICA LA CUOTA FIJA EN EL PRIMER BIMESTRE INDICADO, EL RESTO DE BIMESTRES LOS DEJA EN CERO
                                                Cuota_Fija = Sum_Adeudos_Periodo - Importe_Rezago; //(Decimal)Cuota_Fija_Nueva;
                                                for (Cont_Bimestres = Desde_Bimestre; Cont_Bimestres <= Hasta_Bimestre; Cont_Bimestres++)
                                                {
                                                    if (Cont_Bimestres > Desde_Bimestre && Cuota_Minima_Año != 0)
                                                    {
                                                        Cuota_Fija = 0;
                                                    }
                                                    if (!Dic_Adeudos_Diferencias.ContainsKey(Cont_Bimestres.ToString() + Año_Periodo.ToString()))
                                                    {
                                                        Dic_Adeudos_Diferencias.Add(Cont_Bimestres.ToString() + Año_Periodo.ToString(), Math.Round(Cuota_Fija, 2));
                                                    }
                                                    else
                                                    {
                                                        Dic_Adeudos_Diferencias[Cont_Bimestres.ToString() + Año_Periodo.ToString()] = Math.Round(Cuota_Fija, 2);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //PRORRATEA EL IMPORTE EN EL PERIODO INDICADO
                                                for (Cont_Bimestres = Desde_Bimestre; Cont_Bimestres <= Hasta_Bimestre; Cont_Bimestres++)
                                                {
                                                    if (!Dic_Adeudos_Diferencias.ContainsKey(Cont_Bimestres.ToString() + Año_Periodo.ToString()))
                                                    {
                                                        Dic_Adeudos_Diferencias.Add(Cont_Bimestres.ToString() + Año_Periodo.ToString(), Convert.ToDecimal((Importe_Rezago / (Hasta_Bimestre - Desde_Bimestre + 1)).ToString("#,##0.00")) * Signo);
                                                        //Dic_Adeudos_Diferencias.Add(Cont_Bimestres.ToString() + Año_Periodo.ToString(), Math.Round(Importe_Rezago / (Hasta_Bimestre - Desde_Bimestre + 1) * Signo, 2));
                                                    }
                                                    else
                                                    {
                                                        //Dic_Adeudos_Diferencias[Cont_Bimestres.ToString() + Año_Periodo.ToString()] += Math.Round(Importe_Rezago / (Hasta_Bimestre - Desde_Bimestre + 1) * Signo, 2);
                                                        Dic_Adeudos_Diferencias[Cont_Bimestres.ToString() + Año_Periodo.ToString()] += Convert.ToDecimal((Importe_Rezago / (Hasta_Bimestre - Desde_Bimestre + 1)).ToString("#,##0.00")) * Signo;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Dicc_Tabulador_Recargos = Recargos.Consultar_Tabulador_Recargos_Diccionario(Mes_Actual, Anio_Actual);

            Dt_Adeudos = Crear_Tabla_Adeudos();
            DataRow Nuevo_Adeudo;
            Decimal Total_Adeudo_Anio;
            // formar la tabla de adeudos a partir de los adeudos en el diccionario
            for (Año_Periodo = Desde_Anio; Año_Periodo <= Hasta_Anio; Año_Periodo++)
            {
                Nuevo_Adeudo = Dt_Adeudos.NewRow();
                Total_Adeudo_Anio = 0;
                Nuevo_Adeudo[0] = Año_Periodo.ToString();
                // agregar Cont_Bimestres del diccionario
                for (Cont_Bimestres = 6; Cont_Bimestres >= 1; Cont_Bimestres--)
                {
                    if (Dic_Adeudos_Diferencias.ContainsKey(Cont_Bimestres.ToString() + Año_Periodo.ToString()))
                    {
                        Periodo = Cont_Bimestres.ToString() + Año_Periodo.ToString();
                        if (Convert.ToDecimal(Dic_Adeudos_Diferencias[Periodo]) != -1)
                        {
                            Nuevo_Adeudo[Cont_Bimestres] = Dic_Adeudos_Diferencias[Periodo].ToString("#,##0.00");

                            Total_Adeudo_Anio += Convert.ToDecimal(Nuevo_Adeudo[Cont_Bimestres]);
                            // identificar periodo inicial
                            if (Periodo_Final == "-" && Dic_Adeudos_Diferencias[Periodo] > 0)
                            {
                                Periodo_Final = "0" + Cont_Bimestres.ToString() + "/" + Año_Periodo.ToString();
                            }
                            Periodo_Inicial = "0" + Cont_Bimestres.ToString() + "/" + Año_Periodo.ToString();
                        }
                        else
                        {
                            Nuevo_Adeudo[Cont_Bimestres] = "0.00";
                        }
                    }
                    else
                    {
                        Nuevo_Adeudo[Cont_Bimestres] = "0.00";
                    }
                }
                Total_Adeudo_Impuesto += Total_Adeudo_Anio;
                Nuevo_Adeudo["Adeudo_Total_Año"] = Total_Adeudo_Anio.ToString("#,##0.00");
                Dt_Adeudos.Rows.Add(Nuevo_Adeudo);
            }
            Grid_Estado_Cuenta.DataSource = Dt_Adeudos;
            Grid_Estado_Cuenta.PageIndex = Page_Index;
            Grid_Estado_Cuenta.DataBind();

            //Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();

            //if (Session["Cuenta_Predial_ID_Adeudos"] != null)
            //{
            //    Orden_Variacion.P_Cuenta_Predial_ID = Session["Cuenta_Predial_ID_Adeudos"].ToString().Trim();
            //}

            //if (Session["Orden_Variacion_ID_Adeudos"] != null)
            //{
            //    Orden_Variacion.P_Generar_Orden_No_Orden = Session["Orden_Variacion_ID_Adeudos"].ToString().Trim();
            //}
            //if (Request.QueryString["Anio_Orden"] != null)
            //{
            //    Orden_Variacion.P_Generar_Orden_Anio = Request.QueryString["Anio_Orden"].ToString();
            //}
            //else
            //{
            //    if (Session["Anio_Orden_Adeudos"] != null)
            //    {
            //        Orden_Variacion.P_Generar_Orden_Anio = Session["Anio_Orden_Adeudos"].ToString().Trim();
            //    }
            //}

            //if (Session["Cuota_Fija_Nueva"] != null)
            //{
            //    if (Session["Cuota_Fija_Nueva"].ToString().Trim() != "")
            //    {
            //        Orden_Variacion.P_Cuota_Fija_Nueva = Convert.ToDecimal(Session["Cuota_Fija_Nueva"]);
            //    }
            //}
            //if (Session["Cuota_Fija_Anterior"] != null)
            //{
            //    if (Session["Cuota_Fija_Anterior"].ToString().Trim() != "")
            //    {
            //        Orden_Variacion.P_Cuota_Fija_Anterior = Convert.ToDecimal(Session["Cuota_Fija_Anterior"]);
            //    }
            //}
            //Orden_Variacion.P_Dt_Diferencias = Dt_Diferencias_Adeudos;
            //DataTable Dt_Adeudos = Orden_Variacion.Obtener_Adeudos_Cuenta_Aplicando_Diferencias_Orden();
            //Grid_Estado_Cuenta.DataSource = Dt_Adeudos;
            //Grid_Estado_Cuenta.PageIndex = Page_Index;
            //Grid_Estado_Cuenta.DataBind();
        }
        catch (Exception Ex)
        {
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN      : Cargar_Adeudos_Cancelados_Diferencias
    /// DESCRIPCIÓN         : Lee adeudos Cancelados de la cuenta y los muestra
    /// PARÁMETROS:
    /// CREO                : Antonio Salvador Benavides Guardado
    /// FECHA_CREO          : 02/Marzo/2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Cargar_Adeudos_Cancelados_Diferencias(int Page_Index, DataTable Dt_Diferencias_Adeudos)
    {
        try
        {
            Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
            Dictionary<String, Decimal> Dic_Adeudos_Diferencias = new Dictionary<string, decimal>();
            DataTable Dt_Adeudos = null;

            String Cuenta_Predial_ID = "";
            String Orden_Variacion_ID = "";
            String Anio_Orden = "";
            String Periodo = "";
            int Cont_Bimestres = 0;
            int Año_Periodo = 0;
            int Desde_Anio = DateTime.Now.Year + 1;
            int Hasta_Anio = 0;
            int Tmp_Desde_Anio = 0;
            int Tmp_Hasta_Anio = 0;
            int Mes_Actual = DateTime.Now.Month;
            int Anio_Actual = DateTime.Now.Year;
            Decimal Adeudo_Bimestre = 0;
            Decimal Total_Adeudo_Impuesto = 0;
            String Periodo_Inicial = "-";
            String Periodo_Final = "-";

            if (Session["Cuenta_Predial_ID_Adeudos"] != null)
            {
                Cuenta_Predial_ID = Session["Cuenta_Predial_ID_Adeudos"].ToString().Trim();
            }

            if (Session["Orden_Variacion_ID_Adeudos"] != null)
            {
                Orden_Variacion_ID = Session["Orden_Variacion_ID_Adeudos"].ToString().Trim();
            }
            if (Request.QueryString["Anio_Orden"] != null)
            {
                Anio_Orden = Request.QueryString["Anio_Orden"].ToString();
            }
            else
            {
                if (Session["Anio_Orden_Adeudos"] != null)
                {
                    Anio_Orden = Session["Anio_Orden_Adeudos"].ToString().Trim();
                }
            }

            if (Dt_Diferencias_Adeudos == null)
            {
                Orden_Variacion.P_Cuenta_Predial_ID = Cuenta_Predial_ID;
                Orden_Variacion.P_Generar_Orden_No_Orden = Orden_Variacion_ID;
                Orden_Variacion.P_Generar_Orden_Anio = Anio_Orden;
                Dt_Diferencias_Adeudos = Orden_Variacion.Consulta_Diferencias();
            }

            if (Dt_Diferencias_Adeudos != null)
            {
                if (Obtener_Anio_Minimo_Maximo(out Tmp_Desde_Anio, out Tmp_Hasta_Anio, Dt_Diferencias_Adeudos, Ope_Pre_Diferencias_Detalle.Campo_Periodo, 2))
                {
                    // si el valor regresado por la funcion es menor, asignar, si no, dejar el mismo
                    if (Tmp_Desde_Anio < Desde_Anio)
                    {
                        Desde_Anio = Tmp_Desde_Anio;
                    }
                    // si el valor regresado por la funcion es mayor, asignarlo a la variable, si no, dejar el mismo
                    if (Tmp_Hasta_Anio > Hasta_Anio)
                    {
                        Hasta_Anio = Tmp_Hasta_Anio;
                    }
                }

                //Procesa cada Adeudo de la Orden para Sumarlos al Diccionario
                foreach (DataRow Fila_Adeudos in Dt_Diferencias_Adeudos.Rows)
                {
                    Año_Periodo = Convert.ToInt16(Fila_Adeudos[Ope_Pre_Diferencias_Detalle.Campo_Periodo].ToString().Substring(Fila_Adeudos[Ope_Pre_Diferencias_Detalle.Campo_Periodo].ToString().Length - 4));

                    //PONE CADA IMPORTE EN EL PERIODO INDICADO
                    for (Cont_Bimestres = 1; Cont_Bimestres <= 6; Cont_Bimestres++)
                    {
                        Adeudo_Bimestre = 0;
                        if (Fila_Adeudos["BIMESTRE_" + Cont_Bimestres] != DBNull.Value)
                        {
                            Adeudo_Bimestre = Convert.ToDecimal(Fila_Adeudos["BIMESTRE_" + Cont_Bimestres]);
                        }
                        if (!Dic_Adeudos_Diferencias.ContainsKey(Cont_Bimestres.ToString() + Año_Periodo.ToString()))
                        {
                            Dic_Adeudos_Diferencias.Add(Cont_Bimestres.ToString() + Año_Periodo.ToString(), Adeudo_Bimestre);
                        }
                        else
                        {
                            Dic_Adeudos_Diferencias[Cont_Bimestres.ToString() + Año_Periodo.ToString()] = Adeudo_Bimestre;
                        }
                    }
                }
            }

            Dt_Adeudos = Crear_Tabla_Adeudos();
            DataRow Nuevo_Adeudo;
            Decimal Total_Adeudo_Anio;
            // formar la tabla de adeudos a partir de los adeudos en el diccionario
            for (Año_Periodo = Desde_Anio; Año_Periodo <= Hasta_Anio; Año_Periodo++)
            {
                Nuevo_Adeudo = Dt_Adeudos.NewRow();
                Total_Adeudo_Anio = 0;
                Nuevo_Adeudo[0] = Año_Periodo.ToString();
                // agregar Cont_Bimestres del diccionario
                for (Cont_Bimestres = 6; Cont_Bimestres >= 1; Cont_Bimestres--)
                {
                    if (Dic_Adeudos_Diferencias.ContainsKey(Cont_Bimestres.ToString() + Año_Periodo.ToString()))
                    {
                        Periodo = Cont_Bimestres.ToString() + Año_Periodo.ToString();
                        if (Convert.ToDecimal(Dic_Adeudos_Diferencias[Periodo]) != -1)
                        {
                            Nuevo_Adeudo[Cont_Bimestres] = Dic_Adeudos_Diferencias[Periodo].ToString("#,##0.00");

                            Total_Adeudo_Anio += Convert.ToDecimal(Nuevo_Adeudo[Cont_Bimestres]);
                            // identificar periodo inicial
                            if (Periodo_Final == "-" && Dic_Adeudos_Diferencias[Periodo] > 0)
                            {
                                Periodo_Final = "0" + Cont_Bimestres.ToString() + "/" + Año_Periodo.ToString();
                            }
                            Periodo_Inicial = "0" + Cont_Bimestres.ToString() + "/" + Año_Periodo.ToString();
                        }
                        else
                        {
                            Nuevo_Adeudo[Cont_Bimestres] = "0.00";
                        }
                    }
                    else
                    {
                        Nuevo_Adeudo[Cont_Bimestres] = "0.00";
                    }
                }
                Total_Adeudo_Impuesto += Total_Adeudo_Anio;
                Nuevo_Adeudo["Adeudo_Total_Anio"] = Total_Adeudo_Anio.ToString("#,##0.00");
                Dt_Adeudos.Rows.Add(Nuevo_Adeudo);
            }
            Grid_Estado_Cuenta.DataSource = Dt_Adeudos;
            Grid_Estado_Cuenta.PageIndex = Page_Index;
            Grid_Estado_Cuenta.DataBind();
        }
        catch (Exception Ex)
        {
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Agregar_Periodos
    /// DESCRIPCIÓN: Lee el periodo que recibe como parametro y agrega bimestres correspondientes
    ///             con la cuota especificada en el diccionario 
    /// PARÁMETROS:
    /// 		1. Periodo: Periodo a agregar (formato: 1/2011-6/2011)
    /// 		2. Cuota_Bimestral: Valor decimal con el que se agregaran las parcialidades
    /// 		3. Dic_Adeudos_Diferencias: Diccionario en el que se van a agregar los periodos (con el formato AñoBimestre: 20111)
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 27-sep-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private Dictionary<String, Decimal> Agregar_Periodos(String Periodo, Decimal Cuota_Bimestral, Dictionary<String, Decimal> Dic_Adeudos)
    {
        String[] Arr_Periodos;
        String[] Arr_Bimestres;
        Int32 Anio = 0;
        Int32 Bimestre_Inicial = 0;
        Int32 Bimestre_Final = 0;
        Dictionary<String, Decimal> Dic_Adeudos_Periodo = Dic_Adeudos;

        // separar periodo por guion medio
        Arr_Periodos = Periodo.Split('-');
        if (Arr_Periodos.Length >= 2)
        {
            // separar bimestre y año por diagonal
            Arr_Bimestres = Arr_Periodos[0].Trim().Split('/');
            if (Arr_Bimestres.Length >= 2)
            {
                Int32.TryParse(Arr_Bimestres[0].ToString().Trim(), out Bimestre_Inicial);
                Int32.TryParse(Arr_Bimestres[1].ToString().Trim(), out Anio);
            }
            // separar bimestre y año por diagonal
            Arr_Bimestres = Arr_Periodos[1].Trim().Split('/');
            if (Arr_Bimestres.Length >= 2)
            {
                Int32.TryParse(Arr_Bimestres[0].ToString().Trim(), out Bimestre_Final);
            }

            // si se obtuvieron valores, agregar al diccionario
            if (Bimestre_Final > 0 && Bimestre_Inicial > 0 && Anio > 0)
            {
                // desde bimestre inicial hasta final, agregar al diccionario
                for (int i = Bimestre_Inicial; i <= Bimestre_Final; i++)
                {
                    // si no existe en el diccionario, agregar
                    if (!Dic_Adeudos.ContainsKey(i.ToString() + Anio.ToString()))
                    {
                        Dic_Adeudos.Add(i.ToString() + Anio.ToString(), Cuota_Bimestral);
                    }
                    // si ya existe, sumar (por si los bimestres se traslapan)
                    else
                    {
                        Dic_Adeudos[i.ToString() + Anio.ToString()] += Cuota_Bimestral;
                    }
                }
            }
        }

        return Dic_Adeudos_Periodo;
    }
    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Obtener_Anio_Minimo_Maximo
    /// DESCRIPCIÓN: Breve descripción de lo que hace la función.
    /// PARÁMETROS:
    /// 		1. Desde_Anio: Variable que almacena el valor del año menor
    /// 		2. Hasta_Anio: Variable que almacena el valor del año mayor
    /// 		3. Dt_Adeudos: Datatable con periodos a procesar
    /// 		4. Nombre_Fila: Nombre de la fila con los periodos
    /// 		5. Indice: Valor entero desde el que se obtiene el año (formatos: 1/2011 y 12011)
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 27-sep-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private Boolean Obtener_Anio_Minimo_Maximo(out Int32 Desde_Anio, out Int32 Hasta_Anio, DataTable Dt_Adeudos, String Nombre_Fila, Int32 Indice)
    {
        Int32 Anio = 0;
        Desde_Anio = 99999;
        Hasta_Anio = 0;

        // para cada fila en la tabla
        foreach (DataRow Fila_Tabla in Dt_Adeudos.Rows)
        {
            // obtener y parsear el año
            String St_Anio = Fila_Tabla[Nombre_Fila].ToString().Substring(Indice, 4);
            if (Int32.TryParse(St_Anio, out Anio))
            {
                if (Anio < Desde_Anio)
                {
                    Desde_Anio = Anio;
                }
                if (Anio > Hasta_Anio)
                {
                    Hasta_Anio = Anio;
                }
            }
        }

        // regresar verdadero si se asignaron años a las variables desde y hasta anio
        if (Desde_Anio < 99999 && Hasta_Anio > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Tabla_Adeudos
    ///DESCRIPCIÓN          : Devuelve un DataTable con la estructura para las parcialidades
    ///PARAMETROS: 
    ///CREO                 : Roberto Gonzalez Oseguera
    ///FECHA_CREO           : 27-sep-2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Crear_Tabla_Adeudos()
    {
        DataTable Dt_Adeudos = new DataTable();
        Dt_Adeudos.Columns.Add(new DataColumn("Año", typeof(String)));
        Dt_Adeudos.Columns.Add(new DataColumn("Bimestre_1", typeof(String)));
        Dt_Adeudos.Columns.Add(new DataColumn("Bimestre_2", typeof(String)));
        Dt_Adeudos.Columns.Add(new DataColumn("Bimestre_3", typeof(String)));
        Dt_Adeudos.Columns.Add(new DataColumn("Bimestre_4", typeof(String)));
        Dt_Adeudos.Columns.Add(new DataColumn("Bimestre_5", typeof(String)));
        Dt_Adeudos.Columns.Add(new DataColumn("Bimestre_6", typeof(String)));
        Dt_Adeudos.Columns.Add(new DataColumn("Adeudo_Total_Año", typeof(String)));

        return Dt_Adeudos;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Obtener_Periodos_Bimestre
    ///DESCRIPCIÓN          : Valida la cadena indicada para obtener los periodos de la Bimestres quitando los Años
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 20/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private String Obtener_Periodos_Bimestre(String Periodos, out Boolean Periodo_Corriente_Validado, out Boolean Periodo_Rezago_Validado)
    {
        String Periodo = "";
        int Indice = 0;
        Periodo_Corriente_Validado = false;
        Periodo_Rezago_Validado = false;

        if (Periodos.IndexOf("-") >= 0)
        {
            if (Periodos.Split('-').Length == 2)
            {
                //Valida el segundo nodo del arreglo
                if (Periodos.Split('-').GetValue(1).ToString().IndexOf("/") >= 0)
                {
                    Periodo = Periodos.Split('-').GetValue(0).ToString().Trim().Substring(0, 1);
                    Periodo += "-";
                    Periodo += Periodos.Split('-').GetValue(1).ToString().Trim().Substring(0, 1);
                    Periodo_Rezago_Validado = true;
                }
                else
                {
                    Periodo = Periodos.Split('-').GetValue(0).ToString().Replace("/", "-").Trim();
                    Periodo_Corriente_Validado = true;
                }
            }
            else
            {
                if (Periodos.Contains("/"))
                {
                    Indice = Periodos.IndexOf("/");
                    Periodo = Periodos.Substring(Indice - 1, 1);
                    Periodo += "-";
                    Indice = Periodos.IndexOf("/", Indice + 1);
                    Periodo += Periodos.Substring(Indice - 1, 1);
                    Periodo_Rezago_Validado = true;
                }
                else
                {
                    Periodo = Periodos.Substring(0, 3);
                    Periodo_Corriente_Validado = true;
                }
            }
        }
        else
        {
            if (Periodos.Trim().IndexOf(" ") >= 0)
            {
                if (Periodos.Split(' ').GetValue(0).ToString().Contains("/"))
                {
                    Periodo = Periodos.Split(' ').GetValue(0).ToString().Replace("/", "-").Trim();
                    Periodo_Corriente_Validado = true;
                }
                else
                {
                    Periodo = Periodos.Substring(0, 3);
                    Periodo_Corriente_Validado = true;
                }
            }
        }
        return Periodo;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Grid_Estado_Cuenta_PageIndexChanging
    ///DESCRIPCIÓN          : Evento para cargar el grid al seleccionar una página del mismo
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 11/Noviembre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Grid_Estado_Cuenta_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (Request.QueryString["Consulta_Adeudos_Cancelados"] != null)
        {
            Cargar_Adeudos_Cancelados_Diferencias(e.NewPageIndex, (DataTable)Session["Dt_Agregar_Diferencias"]);
        }
        else
        {
            Cargar_Adeudos_Activos_Diferencias(e.NewPageIndex, (DataTable)Session["Dt_Agregar_Diferencias"]);
        }
    }
}
