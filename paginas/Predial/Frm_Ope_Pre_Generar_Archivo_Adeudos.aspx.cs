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
using Operacion_Predial_Orden_Variacion.Negocio;
using Presidencia.Catalogo_Tipos_Predio.Negocio;
using Presidencia.Catalogo_Tabulador_Recargos.Negocio;
using System.Text.RegularExpressions;
using System.Data.OracleClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;

public partial class paginas_Predial_Frm_Ope_Pre_Generar_Archivo_Adeudos : System.Web.UI.Page
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
            Txt_Anio_Generar.Text = (DateTime.Now.Year + 1).ToString();
            Habilitar_Controles("Inicial"); // Habilita los controles de la forma
            //Limpiar_Controles(); // Limpia los controles del forma
            Consultar_Ordenes_Pendientes();
            Llenar_Combo_Anios_Tabulador();
            Cargar_Documentos_Anteriores();
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
        Match Numero_Encontrado = Rge_Decimal.Match(Texto.Replace("$", "").Replace(",", ""));

        return Numero_Encontrado.Value;
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
    private void Mostrar_Resultados_Generacion_Adeudos(Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Totales)
    {
        String Resultado = "";
        String Reporte_Errores = "";

        // mostrar resultados en el grids
        Pnl_Resultados.Visible = true;

        Resultado = "Cuentas en el padrón: " + Totales.p_Total_Padron.ToString("#,##0") + "<br /><br />";

        if (Chk_Urbano.Checked == true)
        {
            Resultado += "Archivo de predios Urbanos: " + Totales.p_Cuentas_Archivo_Urbano.ToString("#,##0") + " cuentas<br />"
                + " &nbsp; &nbsp; &nbsp; &nbsp; cuentas con cuota mínima: " + Totales.p_Total_CM_Urbano + "<br />"
                + " &nbsp; &nbsp; &nbsp; &nbsp; Total por impuesto predial: " + Totales.p_Total_Adeudo_Urbano.ToString("#,##0.00") + "<br />"
                + " &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Impuesto rezago: " + Totales.p_Total_Rezago_Urbano.ToString("#,##0.00") + "<br />"
                + " &nbsp; &nbsp; &nbsp; &nbsp; Total por recargos ordinarios: " + Totales.p_Total_Recargos_Urbano.ToString("#,##0.00") + "<br />"
                + " &nbsp; &nbsp; &nbsp; &nbsp; Total honorarios: " + Totales.p_Total_Honorarios_Urbano.ToString("#,##0.00") + "<br />"
                + " &nbsp; &nbsp; &nbsp; &nbsp; Descuento en enero: " + Totales.p_Total_Descuento_Enero_Urbano.ToString("#,##0.00") + "<br />"
                + " &nbsp; &nbsp; &nbsp; &nbsp; Descuento en febrero: " + Totales.p_Total_Descuento_Febrero_Urbano.ToString("#,##0.00") + "<br /><br />";
        }
        if (Chk_Rural.Checked == true)
        {
            Resultado += "Archivo de predios Rústicos: " + Totales.p_Cuentas_Archivo_Rural.ToString("#,##0") + " cuentas<br />";
            Resultado += " &nbsp; &nbsp; &nbsp; &nbsp; cuentas con cuota mínima: " + Totales.p_Total_CM_Rural + "<br />";
            Resultado += " &nbsp; &nbsp; &nbsp; &nbsp; Total por impuesto predial: " + Totales.p_Total_Adeudo_Rural.ToString("#,##0.00") + "<br />";
            Resultado += " &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Impuesto rezago: " + Totales.p_Total_Rezago_Rural.ToString("#,##0.00") + "<br />";
            Resultado += " &nbsp; &nbsp; &nbsp; &nbsp; Total por recargos ordinarios: " + Totales.p_Total_Recargos_Rural.ToString("#,##0.00") + "<br />";
            Resultado += " &nbsp; &nbsp; &nbsp; &nbsp; Total honorarios: " + Totales.p_Total_Honorarios_Rural.ToString("#,##0.00") + "<br />";
            Resultado += " &nbsp; &nbsp; &nbsp; &nbsp; Descuento en enero: " + Totales.p_Total_Descuento_Enero_Rural.ToString("#,##0.00") + "<br />";
            Resultado += " &nbsp; &nbsp; &nbsp; &nbsp; Descuento en febrero: " + Totales.p_Total_Descuento_Febrero_Rural.ToString("#,##0.00") + "<br /><br />";
        }
        if (Chk_Foraneos.Checked == true)
        {
            Resultado += "Archivo de predios Foráneos: " + Totales.p_Cuentas_Archivo_Foraneos.ToString("#,##0") + " cuentas<br />";
            Resultado += " &nbsp; &nbsp; &nbsp; &nbsp; cuentas con cuota mínima: " + Totales.p_Total_CM_Foraneos + "<br />";
            Resultado += " &nbsp; &nbsp; &nbsp; &nbsp; Total por impuesto predial: " + Totales.p_Total_Adeudo_Foraneo.ToString("#,##0.00") + "<br />";
            Resultado += " &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Impuesto rezago: " + Totales.p_Total_Rezago_Foraneo.ToString("#,##0.00") + "<br />";
            Resultado += " &nbsp; &nbsp; &nbsp; &nbsp; Total por recargos ordinarios: " + Totales.p_Total_Recargos_Foraneo.ToString("#,##0.00") + "<br />";
            Resultado += " &nbsp; &nbsp; &nbsp; &nbsp; Total honorarios: " + Totales.p_Total_Honorarios_Foraneo.ToString("#,##0.00") + "<br />";
            Resultado += " &nbsp; &nbsp; &nbsp; &nbsp; Descuento en enero: " + Totales.p_Total_Descuento_Enero_Foraneo.ToString("#,##0.00") + "<br />";
            Resultado += " &nbsp; &nbsp; &nbsp; &nbsp; Descuento en febrero: " + Totales.p_Total_Descuento_Febrero_Foraneo.ToString("#,##0.00") + "<br /><br />";
        }
        // escribir resultados en etiqueta de generacion
        Lbl_Resultado_Generacion.Text = Resultado;
        Lbl_Errores_Generacion.Text = Reporte_Errores;


        // *********** temporal
        using (StreamWriter w = File.AppendText(Server.MapPath("~/Reporte/generacion_adeudos.txt")))
        {

            Log("Archivo_Adeudos: Resultado", HttpUtility.HtmlDecode(Resultado), DateTime.Now, w);
            // Close the writer and underlying file.
            w.Close();
        }
        using (StreamWriter w = File.AppendText(Server.MapPath("~/Reporte/generacion_adeudos.txt")))
        {

            Log("Archivo_Adeudos: Reporte_Errores", HttpUtility.HtmlDecode(Reporte_Errores), DateTime.Now, w);
            // Close the writer and underlying file.
            w.Close();
        }
        // *********** temporal

    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Crear_Ds_Archivos_Adeudos
    /// DESCRIPCIÓN: Crea un dataset y lo guarda en una variable de sesion para el reporte 
    ///             con la misma informacion que se muestra en pantalla
    /// PARÁMETROS:
    /// 	1. Totales: Instancia de la capa de negocio con totales de la generacion
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 19-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Crear_Ds_Archivos_Adeudos(Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Totales)
    {
        Ds_Ope_Pre_Archivos_Cierre_Anual Ds_Archivos_Adeudos = new Ds_Ope_Pre_Archivos_Cierre_Anual();

        DataRow Dr_Datos_Generales = Ds_Archivos_Adeudos.Tables[0].NewRow();
        // llenar los datos generales
        Dr_Datos_Generales["Totales"] = "Cuentas en el padrón: " + Totales.p_Total_Padron.ToString("#,##0");
        Dr_Datos_Generales["Titulo"] = "GENERACION DE ARCHIVO DE ADEUDOS " + Txt_Anio_Generar.Text;
        Dr_Datos_Generales["Subtitulo"] = Lbl_Encabezado_Resultados.Text;
        Dr_Datos_Generales["Fecha"] = DateTime.Now.ToString("dd/MMM/yyyy");
        Ds_Archivos_Adeudos.Tables[0].Rows.Add(Dr_Datos_Generales);

        if (Totales != null)
        {
            // agregar detalles de archivo de adeudos seleccionados
            if (Chk_Urbano.Checked == true)
            {
                String Encabezado_Archivo_Urbano = "Archivo de predios Urbanos: " + Totales.p_Cuentas_Archivo_Urbano.ToString("#,##0");

                DataRow Dr_Total_CM_Urbano = Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].NewRow();
                Dr_Total_CM_Urbano["Archivo"] = Encabezado_Archivo_Urbano;
                Dr_Total_CM_Urbano["Total"] = "cuentas con cuota mínima: " + Totales.p_Total_CM_Urbano.ToString("#,##0");
                Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].Rows.Add(Dr_Total_CM_Urbano);

                DataRow Dr_Total_Adeudo_Urbano = Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].NewRow();
                Dr_Total_Adeudo_Urbano["Archivo"] = Encabezado_Archivo_Urbano;
                Dr_Total_Adeudo_Urbano["Total"] = "Total por impuesto predial: " + Totales.p_Total_Adeudo_Urbano.ToString("#,##0.00");
                Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].Rows.Add(Dr_Total_Adeudo_Urbano);

                DataRow Dr_Total_Rezago_Urbano = Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].NewRow();
                Dr_Total_Rezago_Urbano["Archivo"] = Encabezado_Archivo_Urbano;
                Dr_Total_Rezago_Urbano["Total"] = "Impuesto rezago: " + Totales.p_Total_Rezago_Urbano.ToString("#,##0.00");
                Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].Rows.Add(Dr_Total_Rezago_Urbano);

                DataRow Dr_Total_Recargos_Urbano = Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].NewRow();
                Dr_Total_Recargos_Urbano["Archivo"] = Encabezado_Archivo_Urbano;
                Dr_Total_Recargos_Urbano["Total"] = "Total por recargos ordinarios: " + Totales.p_Total_Recargos_Urbano.ToString("#,##0.00");
                Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].Rows.Add(Dr_Total_Recargos_Urbano);

                DataRow Dr_Total_Honorarios_Urbano = Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].NewRow();
                Dr_Total_Honorarios_Urbano["Archivo"] = Encabezado_Archivo_Urbano;
                Dr_Total_Honorarios_Urbano["Total"] = "Total honorarios: " + Totales.p_Total_Honorarios_Urbano.ToString("#,##0.00");
                Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].Rows.Add(Dr_Total_Honorarios_Urbano);

                DataRow Dr_Total_Descuento_Enero_Urbano = Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].NewRow();
                Dr_Total_Descuento_Enero_Urbano["Archivo"] = Encabezado_Archivo_Urbano;
                Dr_Total_Descuento_Enero_Urbano["Total"] = "Descuento en enero: " + Totales.p_Total_Descuento_Enero_Urbano.ToString("#,##0.00");
                Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].Rows.Add(Dr_Total_Descuento_Enero_Urbano);

                DataRow Dr_Total_Descuento_Febrero_Urbano = Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].NewRow();
                Dr_Total_Descuento_Febrero_Urbano["Archivo"] = Encabezado_Archivo_Urbano;
                Dr_Total_Descuento_Febrero_Urbano["Total"] = "Descuento en febrero: " + Totales.p_Total_Descuento_Febrero_Urbano.ToString("#,##0.00");
                Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].Rows.Add(Dr_Total_Descuento_Febrero_Urbano);
            }

            if (Chk_Rural.Checked == true)
            {
                String Encabezado_Archivo_Rusticos = "Archivo de predios Rústicos: " + Totales.p_Cuentas_Archivo_Rural.ToString("#,##0");

                DataRow Dr_Total_CM_Rustico = Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].NewRow();
                Dr_Total_CM_Rustico["Archivo"] = Encabezado_Archivo_Rusticos;
                Dr_Total_CM_Rustico["Total"] = "cuentas con cuota mínima: " + Totales.p_Total_CM_Rural.ToString("#,##0");
                Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].Rows.Add(Dr_Total_CM_Rustico);

                DataRow Dr_Total_Adeudo_Rustico = Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].NewRow();
                Dr_Total_Adeudo_Rustico["Archivo"] = Encabezado_Archivo_Rusticos;
                Dr_Total_Adeudo_Rustico["Total"] = "Total por impuesto predial: " + Totales.p_Total_Adeudo_Rural.ToString("#,##0.00");
                Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].Rows.Add(Dr_Total_Adeudo_Rustico);

                DataRow Dr_Total_Rezago_Rustico = Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].NewRow();
                Dr_Total_Rezago_Rustico["Archivo"] = Encabezado_Archivo_Rusticos;
                Dr_Total_Rezago_Rustico["Total"] = "Total por impuesto predial: " + Totales.p_Total_Adeudo_Rural.ToString("#,##0.00");
                Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].Rows.Add(Dr_Total_Rezago_Rustico);

                DataRow Dr_Total_Recargos_Rustico = Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].NewRow();
                Dr_Total_Recargos_Rustico["Archivo"] = Encabezado_Archivo_Rusticos;
                Dr_Total_Recargos_Rustico["Total"] = "Total por recargos ordinarios: " + Totales.p_Total_Recargos_Rural.ToString("#,##0.00");
                Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].Rows.Add(Dr_Total_Recargos_Rustico);

                DataRow Dr_Total_Honorarios_Rustico = Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].NewRow();
                Dr_Total_Honorarios_Rustico["Archivo"] = Encabezado_Archivo_Rusticos;
                Dr_Total_Honorarios_Rustico["Total"] = "Total honorarios: " + Totales.p_Total_Honorarios_Rural.ToString("#,##0.00");
                Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].Rows.Add(Dr_Total_Honorarios_Rustico);

                DataRow Dr_Total_Descuento_Enero_Rustico = Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].NewRow();
                Dr_Total_Descuento_Enero_Rustico["Archivo"] = Encabezado_Archivo_Rusticos;
                Dr_Total_Descuento_Enero_Rustico["Total"] = "Descuento en enero: " + Totales.p_Total_Descuento_Enero_Rural.ToString("#,##0.00");
                Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].Rows.Add(Dr_Total_Descuento_Enero_Rustico);

                DataRow Dr_Total_Descuento_Febrero_Rustico = Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].NewRow();
                Dr_Total_Descuento_Febrero_Rustico["Archivo"] = Encabezado_Archivo_Rusticos;
                Dr_Total_Descuento_Febrero_Rustico["Total"] = "Descuento en febrero: " + Totales.p_Total_Descuento_Febrero_Rural.ToString("#,##0.00");
                Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].Rows.Add(Dr_Total_Descuento_Febrero_Rustico);
            }

            if (Chk_Foraneos.Checked == true)
            {
                String Encabezado_Archivo_Foraneos = "Archivo de predios Foráneos: " + Totales.p_Cuentas_Archivo_Foraneos.ToString("#,##0");

                DataRow Dr_Total_CM_Foraneos = Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].NewRow();
                Dr_Total_CM_Foraneos["Archivo"] = Encabezado_Archivo_Foraneos;
                Dr_Total_CM_Foraneos["Total"] = "cuentas con cuota mínima: " + Totales.p_Total_CM_Foraneos.ToString("#,##0");
                Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].Rows.Add(Dr_Total_CM_Foraneos);

                DataRow Dr_Total_Adeudo_Foraneo = Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].NewRow();
                Dr_Total_Adeudo_Foraneo["Archivo"] = Encabezado_Archivo_Foraneos;
                Dr_Total_Adeudo_Foraneo["Total"] = "Total por impuesto predial: " + Totales.p_Total_Adeudo_Foraneo.ToString("#,##0.00");
                Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].Rows.Add(Dr_Total_Adeudo_Foraneo);

                DataRow Dr_Total_Rezago_Foraneo = Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].NewRow();
                Dr_Total_Rezago_Foraneo["Archivo"] = Encabezado_Archivo_Foraneos;
                Dr_Total_Rezago_Foraneo["Total"] = "Impuesto rezago: " + Totales.p_Total_Rezago_Foraneo.ToString("#,##0.00");
                Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].Rows.Add(Dr_Total_Rezago_Foraneo);

                DataRow Dr_Total_Recargos_Foraneo = Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].NewRow();
                Dr_Total_Recargos_Foraneo["Archivo"] = Encabezado_Archivo_Foraneos;
                Dr_Total_Recargos_Foraneo["Total"] = "Impuesto rezago: " + Totales.p_Total_Rezago_Foraneo.ToString("#,##0.00");
                Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].Rows.Add(Dr_Total_Recargos_Foraneo);

                DataRow Dr_Total_Honorarios_Foraneo = Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].NewRow();
                Dr_Total_Honorarios_Foraneo["Archivo"] = Encabezado_Archivo_Foraneos;
                Dr_Total_Honorarios_Foraneo["Total"] = "Total honorarios: " + Totales.p_Total_Honorarios_Foraneo.ToString("#,##0.00");
                Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].Rows.Add(Dr_Total_Honorarios_Foraneo);

                DataRow Dr_Total_Descuento_Enero_Foraneo = Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].NewRow();
                Dr_Total_Descuento_Enero_Foraneo["Archivo"] = Encabezado_Archivo_Foraneos;
                Dr_Total_Descuento_Enero_Foraneo["Total"] = "Descuento en enero: " + Totales.p_Total_Descuento_Enero_Foraneo.ToString("#,##0.00");
                Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].Rows.Add(Dr_Total_Descuento_Enero_Foraneo);

                DataRow Dr_Total_Descuento_Febrero_Foraneo = Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].NewRow();
                Dr_Total_Descuento_Febrero_Foraneo["Archivo"] = Encabezado_Archivo_Foraneos;
                Dr_Total_Descuento_Febrero_Foraneo["Total"] = "Descuento en febrero: " + Totales.p_Total_Descuento_Febrero_Foraneo.ToString("#,##0.00");
                Ds_Archivos_Adeudos.Tables["Dt_Totales_Archivos"].Rows.Add(Dr_Total_Descuento_Febrero_Foraneo);
            }

        }

        Session["Ds_Archivos_Adeudos"] = Ds_Archivos_Adeudos;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Anios_Tabulador
    ///DESCRIPCIÓN: Metodo que llena el Combo de Años Tabulador con los años existentes.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 13/Septiembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Anios_Tabulador()
    {
        try
        {
            Cls_Cat_Pre_Tabulador_Recargos_Negocio Anio = new Cls_Cat_Pre_Tabulador_Recargos_Negocio();
            DataTable Dt_Anios = Anio.Consultar_Anios();

            //DataRow fila = Dt_Anios.NewRow();
            //fila[Cat_Pre_Recargos.Campo_Anio_Tabulador] = HttpUtility.HtmlDecode("&SELECCIONE&gt;");
            //Dt_Anios.Rows.InsertAt(fila, 0);
            Cmb_Anio_Tabulador.DataTextField = Cat_Pre_Recargos.Campo_Anio_Tabulador;
            Cmb_Anio_Tabulador.DataValueField = Cat_Pre_Recargos.Campo_Anio_Tabulador;
            Cmb_Anio_Tabulador.DataSource = Dt_Anios;
            Cmb_Anio_Tabulador.DataBind();
            Cmb_Anio_Tabulador.Items.Insert(0, new ListItem("<SELECCIONE>", "SELECCIONE"));
        }
        catch (Exception Ex)
        {
            Lbl_Encabezado_Resultados.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Cargar_Documentos_Anteriores
    /// DESCRIPCIÓN: Buscar los los documentos de generación de adeudos y si se encuentran, mostrar enlace para descarga
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 16-nov-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Cargar_Documentos_Anteriores()
    {
        String Archivo_Urbanos = @"~/Reporte/Urbanos.xlsx";
        String Archivo_Rusticos = @"~/Reporte/Rusticos.xlsx";
        String Archivo_Foraneos = @"~/Reporte/Foraneos.xlsx";

        try
        {
            if (File.Exists(Server.MapPath(Archivo_Urbanos)))
            {
                // crear enlace para archivo
                HyperLink Hl_Enlace_Urbanos = new HyperLink();
                Hl_Enlace_Urbanos.Text = "Último archivo de adeudos urbanos generado";
                Hl_Enlace_Urbanos.NavigateUrl = Archivo_Urbanos;
                Hl_Enlace_Urbanos.CssClass = "Enlace_Archivo";
                Lbl_Enlace_Urbanos.Controls.Clear();
                Lbl_Enlace_Urbanos.Controls.Add(Hl_Enlace_Urbanos);
            }

            if (File.Exists(Server.MapPath(Archivo_Rusticos)))
            {
                // crear enlace para archivo
                HyperLink Hl_Enlace_Rusticos = new HyperLink();
                Hl_Enlace_Rusticos.Text = "Último archivo de adeudos Rústicos generado";
                Hl_Enlace_Rusticos.NavigateUrl = Archivo_Rusticos;
                Hl_Enlace_Rusticos.CssClass = "Enlace_Archivo";
                Lbl_Enlace_Rural.Controls.Clear();
                Lbl_Enlace_Rural.Controls.Add(Hl_Enlace_Rusticos);
            }

            if (File.Exists(Server.MapPath(Archivo_Foraneos)))
            {
                // crear enlace para archivo
                HyperLink Hl_Enlace_Foraneos = new HyperLink();
                Hl_Enlace_Foraneos.Text = "Último archivo de adeudos Foráneos generado";
                Hl_Enlace_Foraneos.NavigateUrl = Archivo_Foraneos;
                Hl_Enlace_Foraneos.CssClass = "Enlace_Archivo";
                Lbl_Enlace_Foraneo.Controls.Clear();
                Lbl_Enlace_Foraneo.Controls.Add(Hl_Enlace_Foraneos);
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
    /// NOMBRE_FUNCIÓN: Crear_Ds_Archivo_Adeudos
    /// DESCRIPCIÓN: Regresa un Dataset con los datos para imprimir el reporte
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 19-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private Ds_Ope_Pre_Archivos_Cierre_Anual Crear_Ds_Archivo_Adeudos()
    {
        Ds_Ope_Pre_Archivos_Cierre_Anual Ds_Cierre_Anual = null;

        // si se encuentra el dataset en variable de sesion, sustituir el que se creo y agregarle datos
        if (Session["Ds_Archivos_Adeudos"] != null)
        {
            Ds_Cierre_Anual = (Ds_Ope_Pre_Archivos_Cierre_Anual)Session["Ds_Archivos_Adeudos"];
        }
        return Ds_Cierre_Anual;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Imprimir_Reporte
    /// DESCRIPCIÓN: Genera el reporte de Crystal con los datos proporcionados en el DataTable
    /// PARÁMETROS:
    /// 		1. Ds_Datos: Dataset con datos a imprimir
    /// 		2. Nombre_Reporte: Nombre del archivo de reporte .rpt
    /// 		3. Nombre_Archivo: Nombre del archivo a generar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 19-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Imprimir_Reporte(Ds_Ope_Pre_Archivos_Cierre_Anual Ds_Datos, String Nombre_Reporte, String Nombre_Archivo)
    {
        ReportDocument Reporte = new ReportDocument();
        String Ruta = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte);

        try
        {
            Reporte.Load(Ruta);
            Reporte.SetDataSource(Ds_Datos);
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
            Imprimir_Reporte(Crear_Ds_Archivo_Adeudos(),
                "Rpt_Pre_Archivos_Cierre_Anual.rpt",
                "Archivo_Adeudos");
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
    /// NOMBRE_FUNCIÓN: Btn_Archivo_Adeudos_Click
    /// DESCRIPCIÓN: Manejo del evento Click para el control Btn_Archivo_Adeudos. Generar archivo de adeudos
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 07-sep-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Archivo_Adeudos_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Rs_Adeudos = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
        Cls_Cat_Pre_Tipos_Predio_Negocio Rs_Tipos_Predio = new Cls_Cat_Pre_Tipos_Predio_Negocio();
        DataTable Dt_Tipos_Predio;
        Int32 Anio_Generar = 0;

        String Archivo_Urbanos = @"~/Reporte/Urbanos.xlsx";
        String Archivo_Rusticos = @"~/Reporte/Rusticos.xlsx";
        String Archivo_Foraneos = @"~/Reporte/Foraneos.xlsx";

        String Filtros = "";
        String Tipo_Predio = "";

        Lbl_Mensaje_Error.Text = "";
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;

        // verificar que hay por lo menos un check activado
        if (Chk_Foraneos.Checked == false && Chk_Rural.Checked == false && Chk_Urbano.Checked == false)
        {
            Lbl_Mensaje_Error.Text = "Debe seleccionar por lo menos un archivo a generar.";
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            return;
        }

        // verificar que el año ingresado sea el siguiente o el actual
        if (Int32.TryParse(Txt_Anio_Generar.Text, out Anio_Generar))
        {
            if (Anio_Generar != DateTime.Now.Year && Anio_Generar != DateTime.Now.Year + 1)
            {
                Lbl_Mensaje_Error.Text = "Sólo se puede generar el archivo de adeudos para el año siguiente o el actual.";
                Lbl_Mensaje_Error.Visible = true;
                return;
            }
        }
        else
        {
            Txt_Anio_Generar.Text = (DateTime.Now.Year + 1).ToString();
        }

        try
        {
            switch (Cmb_Filtro_Uno.SelectedValue)
            {
                case "SECTOR":
                    Filtros += " SECTOR " + Cmb_Orden_Uno.SelectedValue + ", ";
                    break;
                case "CALLE":
                    Filtros += " NOMBRE_CALLE " + Cmb_Orden_Uno.SelectedValue + ", ";
                    break;
                case "COLONIA":
                    Filtros += Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + "." + Cat_Ate_Colonias.Campo_Nombre 
                        + " " + Cmb_Orden_Tres.SelectedValue + ", ";
                    break;
            }
            switch (Cmb_Filtro_Dos.SelectedValue)
            {
                case "SECTOR":
                    Filtros += " SECTOR " + Cmb_Orden_Dos.SelectedValue + ", ";
                    break;
                case "CALLE":
                    Filtros += " NOMBRE_CALLE " + Cmb_Orden_Dos.SelectedValue + ", ";
                    break;
                case "COLONIA":
                    Filtros += Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + "." + Cat_Ate_Colonias.Campo_Nombre 
                        + " " + Cmb_Orden_Tres.SelectedValue + ", ";
                    break;
            }
            switch (Cmb_Filtro_Tres.SelectedValue)
            {
                case "SECTOR":
                    Filtros += " SECTOR " + Cmb_Orden_Tres.SelectedValue + ", ";
                    break;
                case "CALLE":
                    Filtros += " NOMBRE_CALLE " + Cmb_Orden_Tres.SelectedValue + ", ";
                    break;
                case "COLONIA":
                    Filtros += Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + "." + Cat_Ate_Colonias.Campo_Nombre 
                        + " " + Cmb_Orden_Tres.SelectedValue + ", ";
                    break;
            }
            // quitar coma al final
            if (Filtros.EndsWith(", "))
                Filtros = Filtros.Substring(0, Filtros.Length - 2);

            Rs_Adeudos.p_Anio = Anio_Generar;

            // si se especifica tabla de recargos, usarla para el calculo
            if (Cmb_Anio_Tabulador.SelectedIndex > 0)
            {
                Rs_Adeudos.p_Anio_Tabulador_Utilizar = Cmb_Anio_Tabulador.SelectedValue;
            }
            if (Cmb_Tabulador_Enero.SelectedIndex > 0)
            {
                Rs_Adeudos.p_Tabulador_Enero_Utilizar = Cmb_Tabulador_Enero.SelectedValue;
            }
            if (Cmb_Tabulador_Febrero.SelectedIndex > 0)
            {
                Rs_Adeudos.p_Tabulador_Febrero_Utilizar = Cmb_Tabulador_Febrero.SelectedValue;
            }

        DateTime Hora_Inicio = DateTime.Now;
            // si está seleccionado urbano
            if (Chk_Urbano.Checked)
            {
                // iniciar contado de cuotas minimas
                Rs_Adeudos.p_Total_Cuotas_Minimas = 0;
                Rs_Adeudos.p_Total_Adeudo_Archivo = 0;
                Rs_Adeudos.p_Total_Descuento_Enero = 0;
                Rs_Adeudos.p_Total_Descuento_Febrero = 0;
                Rs_Adeudos.p_Total_Cuentas = 0;
                Rs_Adeudos.p_Total_Rezago_Acumulado = 0;
                Rs_Adeudos.p_Total_Honorarios_Acumulado = 0;
                Rs_Adeudos.p_Total_Recargos_Acumulado = 0;
                // consultar tipos de predio
                Rs_Tipos_Predio.P_Campos_Dinamicos = Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID;
                Rs_Tipos_Predio.P_Filtros_Dinamicos = Cat_Pre_Tipos_Predio.Campo_Descripcion + " = 'URBANO'";
                Dt_Tipos_Predio = Rs_Tipos_Predio.Consultar_Tipo_Predio();
                if (Dt_Tipos_Predio.Rows.Count > 0)
                {
                    Tipo_Predio = Dt_Tipos_Predio.Rows[0][0].ToString();
                    Rs_Adeudos.Generar_Archivo_Adeudos(Filtros, Tipo_Predio, "NO", "Urbanos.xlsx");
                    // crear enlace para archivo
                    HyperLink Hl_Enlace_Urbanos = new HyperLink();
                    Hl_Enlace_Urbanos.Text = "Archivo de adeudos urbanos";
                    Hl_Enlace_Urbanos.NavigateUrl = Archivo_Urbanos;
                    Hl_Enlace_Urbanos.CssClass = "Enlace_Archivo";
                    Lbl_Enlace_Urbanos.Controls.Clear();
                    Lbl_Enlace_Urbanos.Controls.Add(Hl_Enlace_Urbanos);
                }
                // guardar contador de cuotas minimas para predios urbanos
                Rs_Adeudos.p_Total_CM_Urbano = Rs_Adeudos.p_Total_Cuotas_Minimas;
                Rs_Adeudos.p_Cuentas_Archivo_Urbano = Rs_Adeudos.p_Total_Cuentas;
                Rs_Adeudos.p_Total_Adeudo_Urbano = Rs_Adeudos.p_Total_Adeudo_Archivo;
                Rs_Adeudos.p_Total_Descuento_Enero_Urbano = Rs_Adeudos.p_Total_Descuento_Enero;
                Rs_Adeudos.p_Total_Descuento_Febrero_Urbano = Rs_Adeudos.p_Total_Descuento_Febrero;
                Rs_Adeudos.p_Total_Recargos_Urbano = Rs_Adeudos.p_Total_Recargos_Acumulado;
                Rs_Adeudos.p_Total_Rezago_Urbano = Rs_Adeudos.p_Total_Rezago_Acumulado;
                Rs_Adeudos.p_Total_Honorarios_Urbano = Rs_Adeudos.p_Total_Honorarios_Acumulado;

            }
            else if (File.Exists(Server.MapPath(Archivo_Urbanos)))
            {
                // crear enlace para archivo
                HyperLink Hl_Enlace_Urbanos = new HyperLink();
                Hl_Enlace_Urbanos.Text = "Último archivo de adeudos urbanos generado";
                Hl_Enlace_Urbanos.NavigateUrl = Archivo_Urbanos;
                Hl_Enlace_Urbanos.CssClass = "Enlace_Archivo";
                Lbl_Enlace_Urbanos.Controls.Clear();
                Lbl_Enlace_Urbanos.Controls.Add(Hl_Enlace_Urbanos);
            }


            using (StreamWriter w = File.AppendText(Server.MapPath("~/Reporte/generacion_adeudos.txt")))
            {

                Log("Archivo_Adeudos: Urbanos", "Cuentas generadas: " + Rs_Adeudos.p_Cuentas_Archivo_Urbano, Hora_Inicio, w);
                // Close the writer and underlying file.
                w.Close();
            }
            Hora_Inicio = DateTime.Now;


            // si está seleccionado rural
            if (Chk_Rural.Checked)
            {
                // iniciar contado de cuotas minimas
                Rs_Adeudos.p_Total_Cuotas_Minimas = 0;
                Rs_Adeudos.p_Total_Adeudo_Archivo = 0;
                Rs_Adeudos.p_Total_Descuento_Enero = 0;
                Rs_Adeudos.p_Total_Descuento_Febrero = 0;
                Rs_Adeudos.p_Total_Cuentas = 0;
                Rs_Adeudos.p_Total_Rezago_Acumulado = 0;
                Rs_Adeudos.p_Total_Honorarios_Acumulado = 0;
                Rs_Adeudos.p_Total_Recargos_Acumulado = 0;
                // consultar tipos de predio
                Rs_Tipos_Predio.P_Campos_Dinamicos = Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID;
                Rs_Tipos_Predio.P_Filtros_Dinamicos = Cat_Pre_Tipos_Predio.Campo_Descripcion + " = 'RUSTICO'";
                Dt_Tipos_Predio = Rs_Tipos_Predio.Consultar_Tipo_Predio();
                if (Dt_Tipos_Predio.Rows.Count > 0)
                {
                    Tipo_Predio = Dt_Tipos_Predio.Rows[0][0].ToString();
                    Rs_Adeudos.Generar_Archivo_Adeudos(Filtros, Tipo_Predio, "NO", "Rusticos.xlsx");
                    // crear enlace para archivo
                    HyperLink Hl_Enlace_Rusticos = new HyperLink();
                    Hl_Enlace_Rusticos.Text = "Archivo de adeudos Rústicos";
                    Hl_Enlace_Rusticos.NavigateUrl = Archivo_Rusticos;
                    Hl_Enlace_Rusticos.CssClass = "Enlace_Archivo";
                    Lbl_Enlace_Rural.Controls.Clear();
                    Lbl_Enlace_Rural.Controls.Add(Hl_Enlace_Rusticos);
                }
                // guardar contador de cuotas minimas para predios urbanos
                Rs_Adeudos.p_Total_CM_Rural = Rs_Adeudos.p_Total_Cuotas_Minimas;
                Rs_Adeudos.p_Cuentas_Archivo_Rural = Rs_Adeudos.p_Total_Cuentas;
                Rs_Adeudos.p_Total_Adeudo_Rural = Rs_Adeudos.p_Total_Adeudo_Archivo;
                Rs_Adeudos.p_Total_Descuento_Enero_Rural = Rs_Adeudos.p_Total_Descuento_Enero;
                Rs_Adeudos.p_Total_Descuento_Febrero_Rural = Rs_Adeudos.p_Total_Descuento_Febrero;
                Rs_Adeudos.p_Total_Recargos_Rural = Rs_Adeudos.p_Total_Recargos_Acumulado;
                Rs_Adeudos.p_Total_Rezago_Rural = Rs_Adeudos.p_Total_Rezago_Acumulado;
                Rs_Adeudos.p_Total_Honorarios_Rural = Rs_Adeudos.p_Total_Honorarios_Acumulado;
            }
            else if (File.Exists(Server.MapPath(Archivo_Rusticos)))
            {
                // crear enlace para archivo
                HyperLink Hl_Enlace_Rusticos = new HyperLink();
                Hl_Enlace_Rusticos.Text = "Último archivo de adeudos Rústicos generado";
                Hl_Enlace_Rusticos.NavigateUrl = Archivo_Rusticos;
                Hl_Enlace_Rusticos.CssClass = "Enlace_Archivo";
                Lbl_Enlace_Rural.Controls.Clear();
                Lbl_Enlace_Rural.Controls.Add(Hl_Enlace_Rusticos);
            }


            using (StreamWriter w = File.AppendText(Server.MapPath("~/Reporte/generacion_adeudos.txt")))
            {

                Log("Archivo_Adeudos: Rusticos", "Cuentas generadas: " + Rs_Adeudos.p_Cuentas_Archivo_Rural, Hora_Inicio, w);
                // Close the writer and underlying file.
                w.Close();
            }
            Hora_Inicio = DateTime.Now;


            // si está seleccionado foraneos
            if (Chk_Foraneos.Checked)
            {
                // iniciar contado de cuotas minimas
                Rs_Adeudos.p_Total_Cuotas_Minimas = 0;
                Rs_Adeudos.p_Total_Adeudo_Archivo = 0;
                Rs_Adeudos.p_Total_Descuento_Enero = 0;
                Rs_Adeudos.p_Total_Descuento_Febrero = 0;
                Rs_Adeudos.p_Total_Cuentas = 0;
                Rs_Adeudos.p_Total_Rezago_Acumulado = 0;
                Rs_Adeudos.p_Total_Honorarios_Acumulado = 0;
                Rs_Adeudos.p_Total_Recargos_Acumulado = 0;
                Tipo_Predio = "";
                // generar archivo
                Rs_Adeudos.Generar_Archivo_Adeudos(Filtros, Tipo_Predio, "SI", "Foraneos.xlsx");
                // crear enlace para archivo
                HyperLink Hl_Enlace_Foraneos = new HyperLink();
                Hl_Enlace_Foraneos.Text = "Archivo de adeudos Foráneos";
                Hl_Enlace_Foraneos.NavigateUrl = Archivo_Foraneos;
                Hl_Enlace_Foraneos.CssClass = "Enlace_Archivo";
                Lbl_Enlace_Foraneo.Controls.Clear();
                Lbl_Enlace_Foraneo.Controls.Add(Hl_Enlace_Foraneos);

                // guardar contador de cuotas minimas para predios urbanos
                Rs_Adeudos.p_Total_CM_Rural = Rs_Adeudos.p_Total_Cuotas_Minimas;
                Rs_Adeudos.p_Cuentas_Archivo_Foraneos = Rs_Adeudos.p_Total_Cuentas;
                Rs_Adeudos.p_Total_Adeudo_Foraneo = Rs_Adeudos.p_Total_Adeudo_Archivo;
                Rs_Adeudos.p_Total_Descuento_Enero_Foraneo = Rs_Adeudos.p_Total_Descuento_Enero;
                Rs_Adeudos.p_Total_Descuento_Febrero_Foraneo = Rs_Adeudos.p_Total_Descuento_Febrero;
                Rs_Adeudos.p_Total_Recargos_Foraneo = Rs_Adeudos.p_Total_Recargos_Acumulado;
                Rs_Adeudos.p_Total_Rezago_Foraneo = Rs_Adeudos.p_Total_Rezago_Acumulado;
                Rs_Adeudos.p_Total_Honorarios_Foraneo = Rs_Adeudos.p_Total_Honorarios_Acumulado;
            }
            else if (File.Exists(Server.MapPath(Archivo_Foraneos)))
            {
                // crear enlace para archivo
                HyperLink Hl_Enlace_Foraneos = new HyperLink();
                Hl_Enlace_Foraneos.Text = "Último archivo de adeudos Foráneos generado";
                Hl_Enlace_Foraneos.NavigateUrl = Archivo_Foraneos;
                Hl_Enlace_Foraneos.CssClass = "Enlace_Archivo";
                Lbl_Enlace_Foraneo.Controls.Clear();
                Lbl_Enlace_Foraneo.Controls.Add(Hl_Enlace_Foraneos);
            }


            using (StreamWriter w = File.AppendText(Server.MapPath("~/Reporte/generacion_adeudos.txt")))
            {

                Log("Archivo_Adeudos: Foraneos", "Cuentas generadas: " + Rs_Adeudos.p_Cuentas_Archivo_Foraneos, Hora_Inicio, w);
                // Close the writer and underlying file.
                w.Close();
            }


            Mostrar_Resultados_Generacion_Adeudos(Rs_Adeudos);
            Crear_Ds_Archivos_Adeudos(Rs_Adeudos);

            Btn_Imprimir.Enabled = true;
        }
        catch (Exception ex)
        {
            using (StreamWriter w = File.AppendText(Server.MapPath("~/Reporte/generacion_adeudos.txt")))
            {

                Log("Excepcion", "Cuentas generadas: " + ex.Message, DateTime.Now, w);
                // Close the writer and underlying file.
                w.Close();
            }

            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
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
