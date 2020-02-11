using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Operacion_Predial_Parametros.Negocio;
using Operacion_Predial_Orden_Variacion.Negocio;
using System.Text.RegularExpressions;
using System.Data.OracleClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using System.Text;

public partial class paginas_Predial_Frm_Ope_Pre_Cierre_Anual : System.Web.UI.Page
{

    ///********************************************************************************
    ///                                 METODOS
#region METODOS

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Roberto González Oseguera
    /// FECHA_CREO  : 23-jul-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Txt_Anio_Aplicar.Text = (DateTime.Now.Year + 1).ToString();
            Habilitar_Controles("Inicial"); // Habilita los controles de la forma
            //Limpiar_Controles(); // Limpia los controles del forma
            Obtener_Parametros_Proyeccion(); // Consulta los parametros de la BD
            Consultar_Ordenes_Pendientes();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION: Limpia los controles que se encuentran en la forma
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 25-jul-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            // ocultar y limpiar mensaje de error
            Lbl_Mensaje_Error.Text = "";
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            // limpiar parametros
            Txt_Cuota_Minima.Text = "";
            Txt_Salario_Minimo.Text = "";
            Txt_Total_Cuentas.Text = "";
            Txt_Tope_Valor_Fiscal_Pensionados.Text = "";
            // llamar metodo para limpiar etiquetas con mensajes de error
            Limpiar_Mensajes_Parametros();

            // limpiar grid
            Grid_Errores.DataSource = null;
            Grid_Errores.DataBind();

            //Limpiar_Controles_Tasas();
        }
        catch (Exception ex)
        {
            throw new Exception("Limpiar_Controles: " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION: Elimina los controles de las tasas (creados dinamicamente)
    ///                 Tomando el diccionario de tasas de la sesion
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 25-jul-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles_Tasas()
    {
        // ocultar y limpiar mensaje de error
        Lbl_Mensaje_Error.Text = "";
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;

        try
        {
            Dictionary<String, String> Dicc_IDs_Conceptos = (Dictionary<String, String>)Session["Dicc_Conceptos"];
            // ocultar y limpiar mensaje de error

            // validar que se recupero el diccionario de la sesion
            if (Dicc_IDs_Conceptos != null)
            {
                foreach (String ID in Dicc_IDs_Conceptos.Keys)
                {
                    // si existen controles para estas tasas, eliminar
                    Control Control_Temporal = FindControl("Lbl_Etiqueta_" + ID);
                    if (Control_Temporal != null)
                    {
                        Control_Temporal.Dispose();
                    }
                    Control_Temporal = FindControl("Txt_Campo_" + ID);
                    if (Control_Temporal != null)
                    {
                        Control_Temporal.Dispose();
                    }
                    Control_Temporal = FindControl("Lbl_Msg_Campo_" + ID);
                    if (Control_Temporal != null)
                    {
                        Control_Temporal.Dispose();
                    }
                    Control_Temporal = FindControl("sep_" + ID);
                    if (Control_Temporal != null)
                    {
                        Control_Temporal.Dispose();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Limpiar_Controles_Tasas: " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Habilitar_Controles
    /// DESCRIPCIÓN: Habilita o Deshabilita los controles de la forma según se requiera 
    ///             para la siguiente operación
    /// PARÁMETROS:
    ///         1. Operacion: Indica la operación que se desea realizar por parte del usuario
    /// 	             (inicial)
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 23-jul-2011 
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        try
        {
            switch (Operacion)
            {
                case "Inicial":
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    break;

                case "Nuevo":
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    break;

                case "Modificar":
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    break;
                case "PROYECCION":
                    Txt_Anio_Aplicar.ReadOnly = false;
                    Txt_Cuota_Minima.ReadOnly = false;
                    Txt_Salario_Minimo.ReadOnly = false;
                    Txt_Tope_Valor_Fiscal_Pensionados.ReadOnly = false;
                    foreach (GridViewRow Tasas in Grid_Tasas.Rows)
                    {
                        TextBox ct = (TextBox)Tasas.Cells[1].Controls[1];
                        ct.ReadOnly = false;
                    }
                    break;
                case "CIERRE ANUAL":
                    Txt_Anio_Aplicar.ReadOnly = false;
                    Txt_Cuota_Minima.ReadOnly = true;
                    Txt_Salario_Minimo.ReadOnly = true;
                    Txt_Tope_Valor_Fiscal_Pensionados.ReadOnly = true;
                    foreach (GridViewRow Tasas in Grid_Tasas.Rows)
                    {
                        TextBox ct = (TextBox)Tasas.Cells[1].Controls[1];
                        ct.ReadOnly = true;
                    }
                    break;
            }

            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }// termina metodo Habilitar_Controles

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Obtener_Parametros_Proyeccion
    /// DESCRIPCION: Consulta los parametros (salario y cuota minima, tasas, etc) de la BD 
    ///             mediante la clase de negocio Generar adeudos predial
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 24-jul-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Obtener_Parametros_Proyeccion()
    {
        Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio RS_Consulta_Parametros = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
        Int32 Anio = 0;     // anio siguiente
        Dictionary<String, String> Dicc_IDs_Conceptos;
        Dictionary<String, Decimal> Dicc_IDs_Tasas;
        DataTable Dt_Tasas = new DataTable();

        Lbl_Mensaje_Error.Text = "";
        Lbl_Mensaje_Error.Visible = false;  
        Img_Error.Visible = false;

        // obtener el año de la caja de texto
        Int32.TryParse(Txt_Anio_Aplicar.Text, out Anio);

        try
        {
            RS_Consulta_Parametros.p_Anio = Anio;
            RS_Consulta_Parametros.Obtener_Parametros(); //Consulta los parametros
            
            // obtener salario minimo
            if (RS_Consulta_Parametros.p_Salario_Minimo > 0)
            {
                Txt_Salario_Minimo.Text = RS_Consulta_Parametros.p_Salario_Minimo.ToString();
            }
            else        // tratar de obtener el salario minimo de un anio anterior
            {
                Decimal Salario_Minimo_Anterior = RS_Consulta_Parametros.Obtener_Salario_Minimo(Anio - 1); // Consultar salario minimo
                if (Salario_Minimo_Anterior > 0)
                {
                    Txt_Salario_Minimo.Text = Salario_Minimo_Anterior.ToString();
                    Lbl_Msg_Salario_Minimo.Text = "No se encontró salario mínimo para " + Anio + " se muestra el de " + (Anio - 1).ToString();
                    Lbl_Msg_Salario_Minimo.Style.Value += "color:red;";
                }
                else            // mostrar mensaje de error
                {
                    Lbl_Msg_Salario_Minimo.Text = "No se encontró salario mínimo";
                    Lbl_Msg_Salario_Minimo.Style.Value += "color:red;";
                }
            }

            // mostrar Anio
            if (Anio > 0)
            {
                Txt_Anio_Aplicar.Text = Anio.ToString();
            }

            // obtener cuota minima
            if (RS_Consulta_Parametros.p_Cuota_Minima > 0)
            {
                Txt_Cuota_Minima.Text = RS_Consulta_Parametros.p_Cuota_Minima.ToString("#,##0.00");
            }
            else            // mostrar mensaje de error
            {
                Decimal Cuota_Minima_Anterior = RS_Consulta_Parametros.Obtener_Cuota_Minima(Anio - 1); // Consultar cuota minima
                if (Cuota_Minima_Anterior > 0)
                {
                    Txt_Cuota_Minima.Text = Cuota_Minima_Anterior.ToString();
                    Lbl_Msg_Cuota_Minima.Text = "No se encontró Cuota mínima para " + Anio + " se muestra la de " + (Anio - 1).ToString();
                    Lbl_Msg_Cuota_Minima.Style.Value += "color:red;";
                }
                else
                {
                    Lbl_Msg_Cuota_Minima.Text = "No se encontró Cuota mínima";
                    Lbl_Msg_Cuota_Minima.Style.Value += "color:red;";
                }
            }

            // mostrar valor fiscal tope
            if (RS_Consulta_Parametros.p_Tope_Salarios_Minimos > 0)
            {
                Txt_Tope_Valor_Fiscal_Pensionados.Text = RS_Consulta_Parametros.p_Tope_Salarios_Minimos.ToString("#,##0.00");
            }
            else            // mostrar mensaje de error
            {
                Lbl_Msg_Tope_Valor_Fiscal_Pensionados.Text = "No se encontró el tope de valor fiscal";
                Lbl_Msg_Tope_Valor_Fiscal_Pensionados.Style.Value += "color:red;";
            }

            // recuperar tasas predial
            RS_Consulta_Parametros.p_Anio = Anio;
            RS_Consulta_Parametros.Obtener_Tasas_Predial(out Dicc_IDs_Conceptos, out Dicc_IDs_Tasas);
            // si no se encontraron valores, intentar con el anio anterior
            if (Dicc_IDs_Conceptos.Count <= 0 || Dicc_IDs_Tasas.Count <= 0)
            {
                // consultar para el anio actual, si se encuentran registros, mostrar mensaje
                RS_Consulta_Parametros.p_Anio = Anio - 1;
                Dt_Tasas = RS_Consulta_Parametros.Obtener_Tasas_Predial(out Dicc_IDs_Conceptos, out Dicc_IDs_Tasas);
                RS_Consulta_Parametros.p_Anio = Anio;
                //if (Dicc_IDs_Tasas.Count > 0 && Dicc_IDs_Conceptos.Count == Dicc_IDs_Tasas.Count)
                //{
                //    Lbl_Mensaje_Error.Text = "No se encontraron tasas para " + Anio + ", se muestran las de " + (Anio - 1).ToString();
                //    Lbl_Mensaje_Error.Visible = true;
                //    Img_Error.Visible = true;
                //}
                //else
                //{
                //    Lbl_Mensaje_Error.Text = "No se encontraron tasas.";
                //    Lbl_Mensaje_Error.Visible = true;
                //    Img_Error.Visible = true;
                //}
            }
            // si se obtuvieron tasas de la consulta, cargar en grid
            if (RS_Consulta_Parametros.p_Dt_Tasas != null)
            {
                if (RS_Consulta_Parametros.p_Dt_Tasas.Rows.Count > 0)
                {
                    Grid_Tasas.Columns[2].Visible = true;
                    Grid_Tasas.DataSource = RS_Consulta_Parametros.p_Dt_Tasas;
                    Grid_Tasas.DataBind();
                    Grid_Tasas.Columns[2].Visible = false;
                }
            }
            else
            {

            }
            // si se obtuvieron tasas de la consulta, cargar en grid
            if (Dt_Tasas.Rows.Count > 0)
            {
                Grid_Tasas.Columns[2].Visible = true;
                Grid_Tasas.DataSource = Dt_Tasas;
                Grid_Tasas.DataBind();
                Grid_Tasas.Columns[2].Visible = false;
            }


            // obtener total de cuentas a generar
            Txt_Total_Cuentas.Text = RS_Consulta_Parametros.p_Total_Cuentas.ToString("#,##0");

        }
        catch (Exception ex)
        {
            throw new Exception("Obtener_Parametros: " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Obtener_Parametros_Cierre_Anual
    /// DESCRIPCION: Muestra los parametros de la instancia de la clase de negocio
    ///             recibida como parametro
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 26-jul-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Obtener_Parametros_Cierre_Anual(Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Parametros)
    {
        Int32 Anio;

        Lbl_Mensaje_Error.Text = "";
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;

        try
        {
            Anio = Parametros.p_Anio;

            // mostrar Anio
            if (Anio == DateTime.Now.Year || Anio == DateTime.Now.Year + 1)
            {
                Txt_Anio_Aplicar.Text = Anio.ToString();
            }
            else
            {
                Anio = DateTime.Now.Year + 1;
                Txt_Anio_Aplicar.Text = Anio.ToString();
            }

            Parametros.Obtener_Parametros(); //Consulta los parametros
            Parametros.p_Total_Padron = Parametros.p_Total_Cuentas;
            // si hay un salario minimo, mostrarlo
            if (Parametros.p_Salario_Minimo > 0)
            {
                Txt_Salario_Minimo.Text = Parametros.p_Salario_Minimo.ToString();
                Lbl_Msg_Salario_Minimo.Text = "";
            }
            else        // si no mostrar mensaje
            {
                Txt_Salario_Minimo.Text = "";
                Lbl_Msg_Salario_Minimo.Text = "No se encontró salario mínimo para el año " + Anio;
                Lbl_Msg_Salario_Minimo.Style.Value += "color:red;";
            }

            // mostrar cuota minima
            if (Parametros.p_Cuota_Minima > 0)
            {
                Txt_Cuota_Minima.Text = Parametros.p_Cuota_Minima.ToString("#,##0.00");
                Lbl_Msg_Cuota_Minima.Text = "";
            }
            else            // mostrar mensaje de error
            {
                Txt_Cuota_Minima.Text = "";
                Lbl_Msg_Cuota_Minima.Text = "No se encontró cuota mínima para el año " + Anio;
                Lbl_Msg_Cuota_Minima.Style.Value += "color:red;";
            }
            
            // mostrar valor fiscal tope
            if (Parametros.p_Tope_Salarios_Minimos > 0)
            {
                Txt_Tope_Valor_Fiscal_Pensionados.Text = Parametros.p_Tope_Salarios_Minimos.ToString("#,##0.00");
                Lbl_Msg_Tope_Valor_Fiscal_Pensionados.Text = "";
            }
            else            // mostrar mensaje de error
            {
                Txt_Tope_Valor_Fiscal_Pensionados.Text = ""; 
                Lbl_Msg_Tope_Valor_Fiscal_Pensionados.Text = "No se encontró el tope de valor fiscal para el año " + Anio;
                Lbl_Msg_Tope_Valor_Fiscal_Pensionados.Style.Value += "color:red;";
            }

            // si se obtuvieron tasas de la consulta, cargar en grid
            if (Parametros.p_Dt_Tasas != null)
            {
                if (Parametros.p_Dt_Tasas.Rows.Count > 0)
                {
                    Grid_Tasas.Columns[2].Visible = true;
                    Grid_Tasas.DataSource = Parametros.p_Dt_Tasas;
                    Grid_Tasas.DataBind();
                    Grid_Tasas.Columns[2].Visible = false;
                }
            }
            //else
            //{
            //    foreach (GridViewRow Fila in Grid_Tasas.Rows)
            //    {
            //        TextBox txt_ = (TextBox)Fila.Cells[1].Controls[1];
            //        txt_.Text = "";
            //    }
            //    Lbl_Mensaje_Error.Text = "No se encontraron tasas de predial para el " + Anio;
            //}

            // obtener total de cuentas a generar
            Txt_Total_Cuentas.Text = Parametros.p_Total_Padron.ToString("#,##0");

        }
        catch (Exception ex)
        {
            throw new Exception("Obtener_Parametros_Cierre_Anual: " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Cuentas_Exencion_Vigente
    /// DESCRIPCION: Metodo para recuperar los valores de las cuentas con porcentaje de 
    ///             exencion vigente
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 25-jul-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Cuentas_Exencion_Vigente()
    {
        Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Rs_Exenciones_Vigentes = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
        DataTable Dt_Exenciones;

        // Consultar_Cuentas_Exencion_Vigente cuentas activas al 1 de enero del siguiente anio
        Dt_Exenciones = Rs_Exenciones_Vigentes.Consultar_Cuentas_Exencion_Vigente("ACTIVA", "01/01/" + DateTime.Now.AddYears(1).Year.ToString());

        if (Dt_Exenciones.Rows.Count <= 0)
        {
            Lbl_Mensaje_Error.Text = "Existen " + Dt_Exenciones.Rows.Count + " cuentas con exención vigente.<br />" + Lbl_Mensaje_Error.Text;
        }

    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Ordenes_Pendientes
    /// DESCRIPCION: Metodo para consultar si existen ordenes de variacion pendientes 
    ///             (POR VALIDAR)
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 12-sep-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Ordenes_Pendientes()
    {
        Cls_Ope_Pre_Orden_Variacion_Negocio Rs_Ordenes = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        DataTable Dt_Ordenes;

        // consultar ordenes de variacion por validar
        Rs_Ordenes.P_Incluir_Campos_Foraneos = false;
        Rs_Ordenes.P_Generar_Orden_Estatus = "POR VALIDAR";
        Dt_Ordenes = Rs_Ordenes.Consultar_Ordenes_Variacion();

        if (Dt_Ordenes.Rows.Count > 0)
        {
            Lbl_Mensaje_Error.Text += "<br /> &nbsp; &nbsp; &nbsp; &nbsp; Existen " + Dt_Ordenes.Rows.Count + " órdenes de variación POR VALIDAR.<br />";
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }

    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Obtener_Proyeccion
    /// DESCRIPCION: Metodo para recuperar los valores de los parametros y obtener una proyeccion
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 25-jul-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private String Obtener_Proyeccion()
    {
        Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio RS_Genera_Adeudos = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
        Int32 Anio;
        Dictionary<String, Decimal> IDs_Tasas = new Dictionary<String, Decimal>();
        Dictionary<String, String> IDs_Conceptos;
        DataTable Sumatoria_Adeudos;

        Decimal Cuota_Minima = 0;
        Decimal Salario_Minimo = 0;
        Decimal valor_Fiscal_Tope = 0;
        Decimal Monto =0;

        Int32.TryParse(Txt_Anio_Aplicar.Text, out Anio);
        Decimal.TryParse(Txt_Cuota_Minima.Text, out Cuota_Minima);
        Decimal.TryParse(Txt_Salario_Minimo.Text, out Salario_Minimo);
        Decimal.TryParse(Txt_Tope_Valor_Fiscal_Pensionados.Text, out valor_Fiscal_Tope);
        // recorrer el grid en busca para leer las tasas
        foreach(GridViewRow Tasa in Grid_Tasas.Rows)
        {
            TextBox ct = (TextBox)Tasa.Cells[1].Controls[1];
            if (!IDs_Tasas.ContainsKey(Tasa.Cells[2].Text) && decimal.TryParse(ct.Text, out Monto))
            {
                IDs_Tasas.Add(Tasa.Cells[2].Text, Monto);
            }
        }
        DateTime Hora_Inicio = DateTime.Now;

        String Mensajes_Error = "";

        Lbl_Mensaje_Error.Text = "";
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;

        try
        {
            // validar que el año sea el actual o el siguiente, si no, regresar mensaje de error
            if (Anio == DateTime.Now.Year || Anio == DateTime.Now.Year + 1)
            {
                RS_Genera_Adeudos.p_Anio = Anio;
            }
            else
            {
                Limpiar_Controles();
                return ("Sólo se puede hacer la generación para el año actual o el siguiente.");
            }
            // si hay tasas pasarlas como parametro, si no, consultarlas
            if (IDs_Tasas.Count > 0)
            {
                RS_Genera_Adeudos.p_Dicc_IDs_Tasas = IDs_Tasas;
            }
            else
            {
                RS_Genera_Adeudos.Obtener_Tasas_Predial(out IDs_Conceptos, out IDs_Tasas);
                if (IDs_Tasas.Count > 0)
                {
                    RS_Genera_Adeudos.p_Dicc_IDs_Tasas = IDs_Tasas;
                }
            }

            RS_Genera_Adeudos.p_Cuota_Minima = Cuota_Minima;
            RS_Genera_Adeudos.p_Salario_Minimo = Salario_Minimo;
            RS_Genera_Adeudos.p_Tope_Salarios_Minimos = valor_Fiscal_Tope;
            RS_Genera_Adeudos.Obtener_Tasas_Predial(out IDs_Conceptos, out IDs_Tasas);

            if (RS_Genera_Adeudos.p_Tope_Salarios_Minimos <= 0)
            {
                RS_Genera_Adeudos.Obtener_Tope_Salarios_Minimos();
            }

            // establecer el tabulador a utilizar
            RS_Genera_Adeudos.p_Anio_Tabulador_Utilizar = Anio.ToString();
            RS_Genera_Adeudos.p_Tabulador_Enero_Utilizar = "ENERO";
            RS_Genera_Adeudos.p_Tabulador_Febrero_Utilizar = "FEBRERO";

            // obtener adeudos
            Mensajes_Error = RS_Genera_Adeudos.Generar_Proyeccion(out Sumatoria_Adeudos);

            // Mostrar resultados generacion
            Mostrar_Resultados_Generacion_Adeudos(Sumatoria_Adeudos, RS_Genera_Adeudos);
            if (Cmb_Tipo_Generacion.SelectedValue == "CIERRE ANUAL")
            {
                Lbl_Encabezado_Resultados.Text = "Adeudos a generar (CIERRE ANUAL)";
            }
            else
            {
                Lbl_Encabezado_Resultados.Text = "Adeudos generados (PROYECCIÓN)";
            }

            // Mostrar errores ocurridos
            Cargar_Listado_Errores_Generacion(RS_Genera_Adeudos.p_Errores_Cuentas);
            // generar el dataset para la impresion
            Crear_Ds_Generacion_Adeudos(Sumatoria_Adeudos, RS_Genera_Adeudos);


            if (!Directory.Exists(Server.MapPath("~/Reporte/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Reporte/"));
            }
            using (StreamWriter w = File.AppendText(Server.MapPath("~/Reporte/generacion_adeudos.txt")))
            {

                Log("Obtener_Proyeccion", "Cuentas generadas: " + RS_Genera_Adeudos.p_Total_Adeudos_Generados, Hora_Inicio, w);
                // Close the writer and underlying file.
                w.Close();
            }


            return Mensajes_Error;
        }
        catch (Exception ex)
        {
            throw new Exception("Obtener_Proyeccion: " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Aplicar_Adeudos_Cierre_Anual
    /// DESCRIPCION: Metodo para aplicar los adeudos del cierre anual
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 02-dic-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private String Aplicar_Adeudos_Cierre_Anual()
    {
        Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio RS_Genera_Adeudos = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
        Cls_Ope_Pre_Parametros_Negocio Parametro = new Cls_Ope_Pre_Parametros_Negocio();
        Int32 Anio = DateTime.Now.Year + 1;     // anio siguiente
        DataTable Sumatoria_Adeudos;

        DateTime Hora_Inicio = DateTime.Now;

        String Mensajes_Error = "";

        Int32.TryParse(Txt_Anio_Aplicar.Text, out Anio);
        // si el año en la caja de texto no es el siguiente o el actual, regresar mensaje
        if (Anio != DateTime.Now.Year && Anio != DateTime.Now.Year + 1)
        {
            return "Sólo se pueden generar adeudos para el año siguiente o el actual.";
        }
        RS_Genera_Adeudos.p_Anio = Anio;

        // eliminar adeudos del año a generar
        RS_Genera_Adeudos.Eliminar_Adeudos_Predial();
        // metodo que genera los adeudos
        Mensajes_Error = RS_Genera_Adeudos.Generar_Impuesto_Cierre_Anual(out Sumatoria_Adeudos);

        // actualizar el anio corriente
        if (Mensajes_Error == "" && Parametro.Consultar_Anio_Corriente() != Anio)
        {
            Parametro.Modificar_Anio_Corriente(Anio);
        }

        // Mostrar resultados generacion
        Mostrar_Resultados_Aplicacion_Adeudos(Sumatoria_Adeudos, RS_Genera_Adeudos);
        Lbl_Encabezado_Resultados.Text = "Adeudos generados";

        // Mostrar errores ocurridos
        Cargar_Listado_Errores_Generacion(RS_Genera_Adeudos.p_Errores_Cuentas);


        if (!Directory.Exists(Server.MapPath("~/Reporte/")))
        {
            Directory.CreateDirectory(Server.MapPath("~/Reporte/"));
        }
        using (StreamWriter w = File.AppendText(Server.MapPath("~/Reporte/generacion_adeudos.txt")))
        {

            Log("Generar_Adeudos_Cierre", "Cuentas generadas: " + RS_Genera_Adeudos.p_Total_Adeudos_Generados, Hora_Inicio, w);
            // Close the writer and underlying file.
            w.Close();
        }



        return Mensajes_Error;
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Tomar_Parametros_De_Usuario
    /// DESCRIPCION: Metodo que lee los valores en las cajas de texto para tomar como parametros
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 27-jul-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Tomar_Parametros_De_Usuario(Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio RS_Genera_Adeudos)
    {
        Int32 Anio = DateTime.Now.Year;     // anio siguiente
        Decimal Cuota_Minima = 0;
        Decimal Salario_Minimo = 0;


        /// **** Obtener parametros de las cajas de texto
        if (!Int32.TryParse(Txt_Anio_Aplicar.Text, out Anio))
        {
            Anio = DateTime.Now.Year;     // anio actual
        }
        RS_Genera_Adeudos.p_Anio = Anio;
        if (Decimal.TryParse(Txt_Cuota_Minima.Text, out Cuota_Minima))
        {
            RS_Genera_Adeudos.p_Cuota_Minima = Cuota_Minima;
        }
        else
        {
            RS_Genera_Adeudos.Obtener_Cuota_Minima(Anio);
        }
        if (decimal.TryParse(Txt_Salario_Minimo.Text, out Salario_Minimo))
        {
            RS_Genera_Adeudos.p_Salario_Minimo = Salario_Minimo;
        }
        else
        {
            RS_Genera_Adeudos.Obtener_Salario_Minimo(Anio);
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Cargar_Listado_Errores_Generacion
    /// DESCRIPCIÓN: Muestra el listado de errores ocurridos durante la generacion de adeudos
    /// PARÁMETROS:
    /// 	1. Errores_Cuentas: Diccionario con cuenta predial y errores ocurridos
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 26-jul-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Cargar_Listado_Errores_Generacion(Dictionary<String, String> Errores_Cuentas)
    {
        // si contiene registros, mostrar panel y cargar en datagrid
        if (Errores_Cuentas != null)
        {
            if (Errores_Cuentas.Count > 0)
            {
                Pnl_Errores_Generacion.Visible = true;
                Grid_Errores.DataSource = Errores_Cuentas;
                Grid_Errores.DataBind();
            }
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Mostrar_Resultados_Generacion_Adeudos
    /// DESCRIPCIÓN: Muestra el resultado de la generacion de adeudos
    /// PARÁMETROS:
    /// 	1. Sumatoria_Adeudos: Sumatoria de cuota anual y bimestres 
    /// 	2. Totales: Instancia de la capa de negocio con totales de la generacion
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 26-jul-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Mostrar_Resultados_Generacion_Adeudos(DataTable Sumatoria_Adeudos, Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Totales)
    {
        String Resultado = "";
        String Reporte_Errores = "";
        HyperLink Hl_Enlace_Urbanos = new HyperLink();
        String Propiedades_Ventana_Emergente;

        Propiedades_Ventana_Emergente = ", 'toolbar=0,location=0,status=0,menubar=0,scrollbars=1,resizable=0,width=550,height=450,left=300,top=200');";
        // si contiene registros, mostrar panel y cargar en datagrid
        if (Sumatoria_Adeudos != null && Sumatoria_Adeudos.Rows.Count > 0)
        {
            // mostrar resultados en el grid5
            Pnl_Resultados.Visible = true;
            Grid_Resultados_Generacion.DataSource = Sumatoria_Adeudos;
            Grid_Resultados_Generacion.DataBind();

            Resultado = "Cuentas en el padrón: " + Totales.p_Total_Padron.ToString("#,##0") + "<br />";
            Resultado += "Se excluyeron " +
                (Totales.p_Total_Bloqueadas + Totales.p_Total_Canceladas + Totales.p_Total_Suspendidas + Totales.p_Total_Pendientes).ToString() + " cuentas.<br />";
            // generar enlace al resumen de cuentas excluidas y concatenar con el mensaje
            //if (Totales.p_Total_Bloqueadas > 0)
            //{
            //    StringBuilder Sb_Enlace_Bloqueadas = new StringBuilder();
            //    HtmlTextWriter Htw_Enlace_Bloqueadas = new HtmlTextWriter(new System.IO.StringWriter(Sb_Enlace_Bloqueadas, System.Globalization.CultureInfo.InvariantCulture));
            //    Hl_Enlace_Urbanos.Text = Totales.p_Total_Bloqueadas + " bloqueadas<br />";
            //    Htw_Enlace_Bloqueadas.AddAttribute(HtmlTextWriterAttribute.Onclick, 
            //        "window.open('Ventanas_Emergentes/Frm_Reporte_Cuentas_Excluidas.aspx?Tipo_Estatus=BLOQUEADA', 'Cuentas_Bloqueadas'" + Propiedades_Ventana_Emergente);
            //    Htw_Enlace_Bloqueadas.AddAttribute(HtmlTextWriterAttribute.Class, "Enlace_Archivo");
            //    Hl_Enlace_Urbanos.RenderControl(Htw_Enlace_Bloqueadas);
            //    Resultado += " &nbsp; &nbsp; &nbsp; &nbsp; " + Sb_Enlace_Bloqueadas.ToString();
            //}
            if (Totales.p_Total_Canceladas > 0)
            {
                StringBuilder Sb_Enlace_Canceladas = new StringBuilder();
                HtmlTextWriter Htw_Enlace_Canceladas = new HtmlTextWriter(new System.IO.StringWriter(Sb_Enlace_Canceladas, System.Globalization.CultureInfo.InvariantCulture));
                Hl_Enlace_Urbanos.Text = Totales.p_Total_Canceladas + " canceladas<br />";
                Htw_Enlace_Canceladas.AddAttribute(HtmlTextWriterAttribute.Onclick, 
                    "window.open('Ventanas_Emergentes/Frm_Reporte_Cuentas_Excluidas.aspx?Tipo_Estatus=CANCELADA&Tipo_Suspension=CANCELADA,SUSPENDIDA', 'Cuentas_Canceladas'" + Propiedades_Ventana_Emergente);
                Htw_Enlace_Canceladas.AddAttribute(HtmlTextWriterAttribute.Class, "Enlace_Archivo");
                Hl_Enlace_Urbanos.RenderControl(Htw_Enlace_Canceladas);
                Resultado += " &nbsp; &nbsp; &nbsp; &nbsp; " + Sb_Enlace_Canceladas.ToString();
            }
            //if (Totales.p_Total_Suspendidas > 0)
            //{
            //    StringBuilder Sb_Enlace_Suspendidas = new StringBuilder();
            //    HtmlTextWriter Htw_Enlace_Suspendidas = new HtmlTextWriter(new System.IO.StringWriter(Sb_Enlace_Suspendidas, System.Globalization.CultureInfo.InvariantCulture));
            //    Hl_Enlace_Urbanos.Text = Totales.p_Total_Suspendidas + " suspendidas<br />";
            //    Htw_Enlace_Suspendidas.AddAttribute(HtmlTextWriterAttribute.Onclick,
            //        "window.open('Ventanas_Emergentes/Frm_Reporte_Cuentas_Excluidas.aspx?Tipo_Suspension=CANCELADA,SUSPENDIDA', 'Cuentas_Suspendidas'" + Propiedades_Ventana_Emergente);
            //    Htw_Enlace_Suspendidas.AddAttribute(HtmlTextWriterAttribute.Class, "Enlace_Archivo");
            //    Hl_Enlace_Urbanos.RenderControl(Htw_Enlace_Suspendidas);
            //    Resultado += " &nbsp; &nbsp; &nbsp; &nbsp; " + Sb_Enlace_Suspendidas.ToString();
            //}
            if (Totales.p_Total_Pendientes > 0)
            {
                StringBuilder Sb_Enlace_Pendientes = new StringBuilder();
                HtmlTextWriter Htw_Enlace_Pendientes = new HtmlTextWriter(new System.IO.StringWriter(Sb_Enlace_Pendientes, System.Globalization.CultureInfo.InvariantCulture));
                Hl_Enlace_Urbanos.Text = Totales.p_Total_Pendientes + " pendientes<br /><br />";
                Htw_Enlace_Pendientes.AddAttribute(HtmlTextWriterAttribute.Onclick, 
                    "window.open('Ventanas_Emergentes/Frm_Reporte_Cuentas_Excluidas.aspx?Tipo_Estatus=PENDIENTE', 'Cuentas_Pendientes'" + Propiedades_Ventana_Emergente);
                Htw_Enlace_Pendientes.AddAttribute(HtmlTextWriterAttribute.Class, "Enlace_Archivo");
                Hl_Enlace_Urbanos.RenderControl(Htw_Enlace_Pendientes);
                Resultado += " &nbsp; &nbsp; &nbsp; &nbsp; " + Sb_Enlace_Pendientes.ToString();
            }

            Resultado += "Se generaron adeudos para " + Totales.p_Total_Adeudos_Generados + " cuentas<br />";
            Resultado += " &nbsp; &nbsp; &nbsp; &nbsp; cuentas con cuota mínima: " + Totales.p_Total_Cuotas_Minimas + "<br />";
            if (Totales.p_Total_Exenciones > 0)
            {
                Resultado += " &nbsp; &nbsp; &nbsp; &nbsp; cuentas con exención: " + Totales.p_Total_Exenciones + "<br /><br />";
            }

            // Ordenes de variacion por cuota minima
            if (Totales.p_Total_Cuotas_Minimas > 0)
            {
                Resultado += " Se van a generar " + Totales.p_Total_Cuotas_Minimas + " órdenes de variación por cuota mínima <br /><br />";
            }


            Resultado += "Total de montos adeudo corriente<br />";

            if (!(Totales.p_Errores_Cuentas == null))
            {
                Reporte_Errores += "Cuentas de las que no se pudo generar adeudo: " + Totales.p_Errores_Cuentas.Count + "<br />";
            }
            // escribir resultados en etiqueta de generacion
            Lbl_Resultado_Generacion.Text = Resultado;
            Lbl_Errores_Generacion.Text = Reporte_Errores;
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Crear_Ds_Generacion_Adeudos
    /// DESCRIPCIÓN: Crea un dataset y lo guarda en una variable de sesion para el reporte 
    ///             con la misma informacion que se muestra en pantalla
    /// PARÁMETROS:
    /// 	1. Sumatoria_Adeudos: Sumatoria de cuota anual y bimestres 
    /// 	2. Totales: Instancia de la capa de negocio con totales de la generacion
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 19-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Crear_Ds_Generacion_Adeudos(DataTable Sumatoria_Adeudos, Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Totales)
    {
        Ds_Ope_Pre_Cierre_Anual Ds_Cierre = new Ds_Ope_Pre_Cierre_Anual();


        if (Sumatoria_Adeudos != null && Sumatoria_Adeudos.Rows.Count > 0)
        {
            DataRow Dr_Datos_Generales = Ds_Cierre.Tables[0].NewRow();

            // copiar los valores de la sumatoria de adeudos generados
            DataRow Dr_Total_Adeudos = Ds_Cierre.Tables["Dt_Total_Bimestres"].NewRow();
            Dr_Total_Adeudos[0] = Sumatoria_Adeudos.Rows[0][0];
            Dr_Total_Adeudos[1] = Sumatoria_Adeudos.Rows[0][1];
            Dr_Total_Adeudos[2] = Sumatoria_Adeudos.Rows[0][2];
            Dr_Total_Adeudos[3] = Sumatoria_Adeudos.Rows[0][3];
            Dr_Total_Adeudos[4] = Sumatoria_Adeudos.Rows[0][4];
            Dr_Total_Adeudos[5] = Sumatoria_Adeudos.Rows[0][5];
            Dr_Total_Adeudos[6] = Sumatoria_Adeudos.Rows[0][6];
            // agregar la nueva fila a la tabla
            Ds_Cierre.Tables["Dt_Total_Bimestres"].Rows.Add(Dr_Total_Adeudos);

            // llenar los datos de adeudos generados
            Dr_Datos_Generales["Total_Cuentas_Padron"] = "Cuentas en el padrón: " + Totales.p_Total_Padron.ToString("#,##0");

            Dr_Datos_Generales["Cuentas_Omitidas"] = "Se excluyeron " +
                (Totales.p_Total_Bloqueadas + Totales.p_Total_Canceladas + Totales.p_Total_Suspendidas + Totales.p_Total_Pendientes).ToString("#,##0") + " cuentas.";

            // para cada tipo de cuenta omitida agregar un nuevo renglon a la tabla Dt_Cuentas_Omitidas (indice 1)
            //if (Totales.p_Total_Bloqueadas > 0)
            //{
            //    DataRow Dr_Bloqueadas = Ds_Cierre.Tables["Dt_Cuentas_Omitidas"].NewRow();
            //    Dr_Bloqueadas["Cuenta_Omitida"] = Totales.p_Total_Bloqueadas.ToString("#,##0") + " bloqueadas";
            //    Ds_Cierre.Tables["Dt_Cuentas_Omitidas"].Rows.Add(Dr_Bloqueadas);
            //}
            if (Totales.p_Total_Canceladas > 0)
            {
                DataRow Dr_Canceladas = Ds_Cierre.Tables["Dt_Cuentas_Omitidas"].NewRow();
                Dr_Canceladas["Cuenta_Omitida"] = Totales.p_Total_Canceladas.ToString("#,##0") + " canceladas";
                Ds_Cierre.Tables["Dt_Cuentas_Omitidas"].Rows.Add(Dr_Canceladas);
            }
            //if (Totales.p_Total_Suspendidas > 0)
            //{
            //    DataRow Dr_Suspendidas = Ds_Cierre.Tables["Dt_Cuentas_Omitidas"].NewRow();
            //    Dr_Suspendidas["Cuenta_Omitida"] = Totales.p_Total_Suspendidas.ToString("#,##0") + " suspendidas";
            //    Ds_Cierre.Tables["Dt_Cuentas_Omitidas"].Rows.Add(Dr_Suspendidas);
            //}
            if (Totales.p_Total_Pendientes > 0)
            {
                DataRow Dr_Pendientes = Ds_Cierre.Tables["Dt_Cuentas_Omitidas"].NewRow();
                Dr_Pendientes["Cuenta_Omitida"] = Totales.p_Total_Pendientes.ToString("#,##0") + " pendientes";
                Ds_Cierre.Tables["Dt_Cuentas_Omitidas"].Rows.Add(Dr_Pendientes);
            }

            Dr_Datos_Generales["Adeudos_Generados"] = "Se generaron adeudos para " + Totales.p_Total_Adeudos_Generados.ToString("#,##0") + " cuentas";
            // agregar renglones con detalles a la tabla Dt_Adeudos_Generados
            DataRow Dr_Adeudos_Generados = Ds_Cierre.Tables["Dt_Adeudos_Generados"].NewRow();
            Dr_Adeudos_Generados["Adeudo_Generado"] = "cuentas con cuota mínima: " + Totales.p_Total_Cuotas_Minimas.ToString("#,##0");
            Ds_Cierre.Tables["Dt_Adeudos_Generados"].Rows.Add(Dr_Adeudos_Generados);
            if (Totales.p_Total_Exenciones > 0)
            {
                Dr_Adeudos_Generados = Ds_Cierre.Tables["Dt_Adeudos_Generados"].NewRow();
                Dr_Adeudos_Generados["Adeudo_Generado"] = "cuentas con exención: " + Totales.p_Total_Exenciones.ToString("#,##0");
                Ds_Cierre.Tables["Dt_Adeudos_Generados"].Rows.Add(Dr_Adeudos_Generados);
            }

            // Ordenes de variacion por cuota minima
            if (Totales.p_Total_Cuotas_Minimas > 0)
            {
                Dr_Datos_Generales["Ordenes_Generadas"] = " Se van a generar " + Totales.p_Total_Cuotas_Minimas.ToString("#,##0") + " órdenes de variación por cuota mínima";
            }

            // "Total de montos adeudo corriente<br />";
            Dr_Datos_Generales["Totales"] = "Total de adeudo corriente generado:";

            if (!(Totales.p_Errores_Cuentas == null))
            {
                Dr_Datos_Generales["Errores"] = "Cuentas de las que no se pudo generar adeudo: " + Totales.p_Errores_Cuentas.Count;
                foreach (KeyValuePair<string, string> Error in Totales.p_Errores_Cuentas)
                {
                    DataRow Dr_Error = Ds_Cierre.Tables["Dt_Errores"].NewRow();
                    Dr_Error["Cuenta_Predial"] = Error.Key;
                    Dr_Error["Error"] = Error.Value;
                    Ds_Cierre.Tables["Dt_Errores"].Rows.Add(Dr_Error);
                }
            }

            Ds_Cierre.Tables["Dt_Datos_Generales"].Rows.Add(Dr_Datos_Generales);
        }
        Session["Ds_Generacion_Adeudos"] = Ds_Cierre;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Mostrar_Resultados_Generacion_Adeudos
    /// DESCRIPCIÓN: Muestra el resultado de los adeudos aplicados
    /// PARÁMETROS:
    /// 	1. Sumatoria_Adeudos: Sumatoria de cuota anual y bimestres 
    /// 	2. Totales: Instancia de la capa de negocio con totales de la generacion
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 02-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Mostrar_Resultados_Aplicacion_Adeudos(DataTable Sumatoria_Adeudos, Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Totales)
    {
        String Resultado = "";
        String Reporte_Errores = Lbl_Resultado_Generacion.Text;
        HyperLink Hl_Enlace_Urbanos = new HyperLink();
        //String Propiedades_Ventana_Emergente;

        // quitar ultima linea del resultado anterior
        if (Resultado.IndexOf("Total de montos ") > 0)
        {
            Resultado = Resultado.Substring(0, Resultado.IndexOf("Total de montos "));
        }

        //Propiedades_Ventana_Emergente = ", 'toolbar=0,location=0,status=0,menubar=0,scrollbars=1,resizable=0,width=550,height=450,left=300,top=200');";
        // si contiene registros, mostrar panel y cargar en datagrid
        if (Sumatoria_Adeudos != null && Sumatoria_Adeudos.Rows.Count > 0)
        {
            // mostrar resultados en el grid
            Pnl_Resultados.Visible = true;
            Grid_Resultados_Generacion.DataSource = Sumatoria_Adeudos;
            Grid_Resultados_Generacion.DataBind();

            // Ordenes de variacion por cuota minima
            //if (Totales.p_Total_Ordenes_Cuota_Minima > 0)
            //{
            //    String Fecha_Generacion = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy");
            //    StringBuilder Sb_Enlace_Ordenes = new StringBuilder();
            //    HtmlTextWriter Htw_Enlace_Ordenes = new HtmlTextWriter(new System.IO.StringWriter(Sb_Enlace_Ordenes, System.Globalization.CultureInfo.InvariantCulture));
            //    Hl_Enlace_Urbanos.Text = Totales.p_Total_Ordenes_Cuota_Minima + " órdenes de variación por cuota mínima";
            //    Htw_Enlace_Ordenes.AddAttribute(HtmlTextWriterAttribute.Onclick,
            //        "window.open('Ventanas_Emergentes/Frm_Reporte_Ordenes_Cuota_Minima.aspx?Fecha=" + Fecha_Generacion
            //        + "&Anio=" + DateTime.Now.AddYears(1).Year
            //        + "', 'Cuentas_Pendientes'" + Propiedades_Ventana_Emergente);
            //    Htw_Enlace_Ordenes.AddAttribute(HtmlTextWriterAttribute.Class, "Enlace_Archivo");
            //    Hl_Enlace_Urbanos.RenderControl(Htw_Enlace_Ordenes);
            //    Resultado += " Se generaron " + Sb_Enlace_Ordenes.ToString() + " <br /><br />";
            //}

            // Ordenes de variacion por cuota minima
            if (Totales.p_Total_Ordenes_Cuota_Minima > 0)
            {
                Resultado += " Faltan por generar " + Totales.p_Total_Ordenes_Cuota_Minima + " órdenes de variación por cuota mínima <br /><br />";
            }

            // escribir resultados en etiqueta de generacion
            Lbl_Resultado_Generacion.Text = Resultado;
            Lbl_Errores_Generacion.Text = Reporte_Errores;
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Extraer_Numero
    /// DESCRIPCIÓN: Mediante una expresión regular encuentra números en el texto
    /// PARÁMETROS:
    /// 	1. Texto: Texto en el que se va a buscar un número
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 25-jul-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private String Extraer_Numero(String Texto)
    {
        Regex Rge_Decimal = new Regex(@"(?<entero>[0-9]{1,12})(?:\.[0-9]{0,4})?");
        Match Numero_Encontrado = Rge_Decimal.Match(Texto);

        return Numero_Encontrado.Value;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Crear_Ds_Cierre_Anual
    /// DESCRIPCIÓN: Regresa un Dataset con los datos para imprimir el reporte
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 19-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private DataSet Crear_Ds_Cierre_Anual()
    {
        Ds_Ope_Pre_Cierre_Anual Ds_Cierre_Anual = new Ds_Ope_Pre_Cierre_Anual();
        
        // si se encuentra el dataset en variable de sesion, sustituir el que se creo y agregarle datos
        if (Session["Ds_Generacion_Adeudos"] != null)
        {
            Ds_Cierre_Anual = (Ds_Ope_Pre_Cierre_Anual)Session["Ds_Generacion_Adeudos"];
            // verificar que contenga tablas y que la primera tabla contenga renglones
            if (Ds_Cierre_Anual.Tables.Count > 0 && Ds_Cierre_Anual.Tables[0].Rows.Count > 0)
            {
                Ds_Cierre_Anual.Tables[0].Rows[0]["Titulo"] = "CIERRE ANUAL " + (DateTime.Now.Year + 1).ToString();
                Ds_Cierre_Anual.Tables[0].Rows[0]["Subtitulo"] = Lbl_Encabezado_Resultados.Text;
                Ds_Cierre_Anual.Tables[0].Rows[0]["Fecha"] = DateTime.Now.ToString("dd/MMM/yyyy");
            }
        }
        return Ds_Cierre_Anual;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Imprimir_Reporte
    /// DESCRIPCIÓN: Genera el reporte de Crystal con los datos proporcionados en el DataTable
    /// PARÁMETROS:
    /// 		1. Ds_Convenio: Dataset con datos a imprimir
    /// 		2. Nombre_Reporte: Nombre del archivo de reporte .rpt
    /// 		3. Nombre_Archivo: Nombre del archivo a generar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 19-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Imprimir_Reporte(DataSet Ds_Convenio, String Nombre_Reporte, String Nombre_Archivo)
    {
        ReportDocument Reporte = new ReportDocument();
        String Ruta = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte);

        try
        {
            Reporte.Load(Ruta);
            Reporte.SetDataSource(Ds_Convenio);
        }
        catch
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "No se pudo cargar el reporte para su impresión";
        }

        String PDF_Convenio = Nombre_Archivo + ".pdf";  // Es el nombre del PDF que se va a generar 
        try
        {
            ExportOptions Export_Options_Calculo = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options_Calculo = new DiskFileDestinationOptions();
            Disk_File_Destination_Options_Calculo.DiskFileName = Server.MapPath("../../Reporte/" + PDF_Convenio);
            Export_Options_Calculo.ExportDestinationOptions = Disk_File_Destination_Options_Calculo;
            Export_Options_Calculo.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options_Calculo.ExportFormatType = ExportFormatType.PortableDocFormat;
            Reporte.Export(Export_Options_Calculo);

            Mostrar_Reporte(PDF_Convenio, "Cierre_Anual", "Window_Fmt");
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Mostrar_Reporte
    /// DESCRIPCIÓN: Visualiza en pantalla el reporte indicado
    /// PARÁMETROS:
    /// 		1. Nombre_Reporte: Nombre del reporte a generar
    /// 		2. Tipo: Parametro tipo de reporte
    /// 		3. Window_Fmt: Parametros para la ventana en la que se muestra el reporte
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 20-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Mostrar_Reporte(String Nombre_Reporte, String Tipo, String Window_Fmt)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            Pagina = Pagina + Nombre_Reporte;
            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                Window_Fmt,
                "window.open('" + Pagina +
                "', '" + Tipo + "','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')",
                true
                );
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Mostrar_Reporte
    /// DESCRIPCIÓN: Visualiza en pantalla el reporte indicado
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 20-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Limpiar_Mensajes_Parametros()
    {
        Lbl_Msg_Cuota_Minima.Text = "";
        Lbl_Msg_Salario_Minimo.Text = "";
        Lbl_Msg_Total_Cuentas.Text = "";
        Lbl_Msg_Tope_Valor_Fiscal_Pensionados.Text = "";
    }

#endregion METODOS


    ///********************************************************************************
    ///                                 EVENTOS
#region EVENTOS

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Page_Load
    /// DESCRIPCIÓN:  Manejador del evento Carga de pagina, 
    /// PARÁMETROS:
    /// 	1. sender: Objeto que  llama al evento
    /// 	2. e: Argumentos del evento
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 22-jul-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            //if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!Page.IsPostBack)
            {
                Session["Activa"] = true;
                //Recuperar_Sesion();       // recuperar el valor de cookies con el estado de los paneles
                Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                //Llenar_Grid();

                Btn_Imprimir.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Cierre_Anual_Click
    /// DESCRIPCIÓN: Manejo del evento Click para el control Btn_Salir. Ir a la página principal
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 23-jul-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Cierre_Anual_Click(object sender, ImageClickEventArgs e)
    {
        String Mensaje = "";

        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        Lbl_Msg_Salario_Minimo.Text = "";
        Lbl_Msg_Cuota_Minima.Text = "";
        Lbl_Msg_Tope_Valor_Fiscal_Pensionados.Text = "";

        try
        {
            if (Cmb_Tipo_Generacion.SelectedValue == "CIERRE ANUAL")
            {
                Mensaje = Obtener_Proyeccion();
                Btn_Aplicar_Adeudos.Visible = true;
                Btn_Imprimir.Enabled = true;
            }
            else if (Cmb_Tipo_Generacion.SelectedValue == "PROYECCION")
            {
                Mensaje = Obtener_Proyeccion();
                Btn_Imprimir.Enabled = true;
                Btn_Aplicar_Adeudos.Visible = false;
                // volver a mostrar adeudos
                //Obtener_Parametros_Proyeccion(); // Consulta los parametros de la BD
            }
            else
            {
                Obtener_Parametros_Proyeccion();
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = " Debe seleccionar el tipo de generación de adeudo.";
            }

            // si ocurrio algun error, mostrarlo
            if (Mensaje.Length > 0)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Mensaje;
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Imprimir_Click
    /// DESCRIPCIÓN: Manejo del evento clic en el boton imprimir (imprimir reporte de cierre)
    /// PARÁMETROS: sender, e
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 19-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        // verificar que hay texto para imprimir cuenta seleccionada
        if (Lbl_Resultado_Generacion.Text != "" || Lbl_Errores_Generacion.Text != "")
        {
            // llamar metodo impresion de reporte
            Imprimir_Reporte(Crear_Ds_Cierre_Anual(),
                "Rpt_Pre_Cierre_Anual.rpt",
                "Cierre_Anual");
        }
        else
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "No hay resultados para generar reporte.<br />";
        }
    }
    
    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Salir_Click
    /// DESCRIPCIÓN: Manejo del evento Click para el control Btn_Salir. Ir a la página principal
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 23-jul-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip == "Inicio")
            {
                Session.Remove("Consulta_Partidas");
                Session.Remove("Dicc_Tasas");
                Session.Remove("Dicc_Conceptos");
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Inicializa_Controles();//Habilita los controles para la siguiente operación del usuario en el catálogo
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Cmb_Tipo_Generacion_SelectedIndexChanged
    /// DESCRIPCIÓN: Manejo del evento SelectedIndexChanged para el combo tipo generacion
    ///             llama al evento habilitar controles
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 07-sep-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Cmb_Tipo_Generacion_SelectedIndexChanged(object sender, EventArgs e)
    {
        // ocultar y limpiar mensaje de error
        Lbl_Mensaje_Error.Text = "";
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Int32 Anio = 0;

        // obtener el año de la caja de texto
        Int32.TryParse(Txt_Anio_Aplicar.Text, out Anio);
        // validar que si no es el año actual o el siguiente, se establece con el año siguiente
        if (Anio != DateTime.Now.Year && Anio != DateTime.Now.Year + 1)
        {
            Anio = DateTime.Now.Year + 1;
            Txt_Anio_Aplicar.Text = Anio.ToString();
        }

        Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio DS_Consulta = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
        // si hay un valor seleccionado
        if (Cmb_Tipo_Generacion.SelectedIndex > 0)
        {


            // llamar metodo habilitar controles para que habilite o deshabilite las cajas de texto
            Habilitar_Controles(Cmb_Tipo_Generacion.SelectedValue);
            // si el tipo de generacion es cierre anual, obtener los parametros para el cierre
            if (Cmb_Tipo_Generacion.SelectedValue == "CIERRE ANUAL")
            {
                DS_Consulta.p_Anio = Anio;
                Obtener_Parametros_Cierre_Anual(DS_Consulta);
            }
            else
            {
                Obtener_Parametros_Proyeccion();
                Btn_Aplicar_Adeudos.Visible = false;
            }
            Consultar_Ordenes_Pendientes();
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Aplicar_Adeudos_Click
    /// DESCRIPCIÓN: Aplicacion de adeudos
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 14-sep-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Aplicar_Adeudos_Click(object sender, EventArgs e)
    {
        String Mensaje = "";

        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        try
        {
            if (Cmb_Tipo_Generacion.SelectedValue == "CIERRE ANUAL")
            {
                Mensaje = Aplicar_Adeudos_Cierre_Anual();
                Btn_Aplicar_Adeudos.Visible = false;
                if (Mensaje != "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType()
                        , "Cierre anual", "alert('Generación de adeudos Exitosa');", true);
                }
            }

            // si ocurrio algun error, mostrarlo
            if (Mensaje.Length > 0)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Mensaje;
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
        
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Txt_Anio_Aplicar_Changed
    /// DESCRIPCIÓN: Manejar el cambio de texto en la caja Anio_Aplicar
    ///             validar el año
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 13-ene-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Txt_Anio_Aplicar_Changed(object sender, EventArgs e)
    {
        Int32 Anio;

        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        Limpiar_Mensajes_Parametros();
        Int32.TryParse(Txt_Anio_Aplicar.Text, out Anio);
        // validar que el año sea el actual o el siguiente
        if (Anio == DateTime.Now.Year || Anio == DateTime.Now.Year + 1)
        {
            Obtener_Parametros_Proyeccion();
        }
        else
        {
            Limpiar_Controles();
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Sólo se puede hacer la generación para el año actual o el siguiente";
        }
    }

#endregion EVENTOS

    public static void Log(string Funcion, string Mensaje, DateTime Hora_Inicial, TextWriter w)
    {
        w.WriteLine("\r\nFuncion : " + Funcion);
        w.WriteLine("Iniciado   {0} {1}", Hora_Inicial.ToLongTimeString(), Hora_Inicial.ToLongDateString());
        w.WriteLine("Finalizado {0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
        w.WriteLine("  :");
        w.WriteLine("  :{0}", Mensaje);
        w.WriteLine("-------------------------------");
        // Update the underlying file.
        w.Flush();
    }

}
