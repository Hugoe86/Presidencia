using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.OracleClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Catalogo_Cuotas_Minimas.Negocio;
using Presidencia.Catalogo_Instituciones_Recepcion_Pago_Predial.Negocio;
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio;
using Presidencia.Operacion_Predial_Parametros.Negocio;
using Presidencia.Operacion_Predial_Pagos_Instit_Externas.Negocio;
using Presidencia.Catalogo_Descuentos_Predial.Negocio;
using Presidencia.Catalogo_Claves_Ingreso.Negocio;
using Presidencia.Catalogo_Tipos_Predio.Negocio;
using Presidencia.Operacion_Predial_Recepcion_Pagos.Negocio;

public partial class paginas_Predial_Frm_Ope_Pre_Pagos_Instit_Externas : System.Web.UI.Page
{

    private Dictionary<int, string> Dic_Meses = new Dictionary<int, string>() 
        { 
            { 1, "ene" }, { 2, "feb" }, 
            { 3, "mar" }, { 4, "abr" }, 
            { 5, "may" }, { 6, "jun" }, 
            { 7, "jul" }, { 8, "ago" }, 
            { 9, "sep" }, { 10, "oct" }, 
            { 11, "nov" }, { 12, "dic" } 
        };

    #region METODOS

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Inicializa_Controles
    /// DESCRIPCIÓN: Prepara los controles en la forma para que el usuario pueda realizar 
    ///          diferentes operaciones
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 16-ene-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Habilitar_Controles("Inicial");
            Limpiar_Controles();
            Cargar_Combos_Instituciones();
            Cargar_Combo_Anios_Captura();
            Consultar_Capturas();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Habilitar_Controles
    /// DESCRIPCIÓN: Habilita o Deshabilita los controles de la forma según se requiera para la 
    ///              siguiente operación
    /// PARÁMETROS:
    ///             1. Operacion: Indica el tipo de operacion para la que se preparan los controles
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 16-ene-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado;

        try
        {
            Habilitado = false;
            switch (Operacion)
            {
                case "Inicial":
                    Habilitado = false;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Nuevo.Visible = true;
                    Btn_Imprimir.Visible = false;
                    Pnl_Contenedor_Controles.Visible = false;
                    Pnl_Pagos_Registrados.Visible = true;
                    Div_Contenedor_Busqueda.Visible = true;
                    break;

                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Subir_Archivo.ToolTip = "Enviar archivo";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Nuevo.Visible = true;
                    Btn_Imprimir.Visible = false;
                    Pnl_Contenedor_Controles.Visible = true;
                    Pnl_Pagos_Registrados.Visible = false;
                    Div_Contenedor_Busqueda.Visible = false;
                    Txt_Nombre_Archivo.Visible = false;
                    Fup_Archivo_Adeudos.Visible = true;
                    Btn_Subir_Archivo.Visible = true;
                    break;

                case "Consulta":
                    Habilitado = false;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Salir.ToolTip = "Regresar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Nuevo.Visible = true;
                    Btn_Imprimir.Visible = true;
                    Pnl_Contenedor_Controles.Visible = true;
                    Pnl_Pagos_Registrados.Visible = false;
                    Div_Contenedor_Busqueda.Visible = true;
                    Txt_Nombre_Archivo.Visible = true;
                    Fup_Archivo_Adeudos.Visible = false;
                    Btn_Subir_Archivo.Visible = false;
                    break;
            }

            Cmb_Banco.Enabled = Habilitado;
            Cmb_Filtrar_Institucion.Enabled = !Habilitado;     //deshabilitar la búsqueda mientras se editan los datos
            Btn_Buscar.Enabled = !Habilitado;

            Txt_Fecha.Enabled = false;
            Txt_Caja.Enabled = false;
            Txt_Numero_Captura.Enabled = false;
            Txt_Nombre_Archivo.Enabled = false;
            Txt_Cantidad_Movimientos.Enabled = false;

            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Limpiar_Controles
    /// DESCRIPCIÓN: Limpia los controles que se encuentran en la forma
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 16-ene-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            Txt_Nombre_Archivo.Text = "";
            Txt_Cantidad_Movimientos.Text = "";
            Session.Remove("Archivo_Pagos");
            Session.Remove("Dt_Pagos");
            Session.Remove("Dt_Captura");
            Session.Remove("Dt_Adeudos_Totales");
            Session.Remove("Dt_Adeudo_Detallado_Predial");
            Session.Remove("Dt_Pasivos_Pago");
            Txt_Numero_Captura.Text = "";
            Txt_Caja.Text = "";
            Cmb_Banco.SelectedIndex = 0;

            Div_Encabezado_Pagos_Incluidos.InnerText = "Pagos Incluidos";
            Div_Encabezado_Pagos_Excluidos.InnerText = "Pagos Excluidos";
            Grid_Pagos_Incluidos.DataSource = null;
            Grid_Pagos_Incluidos.DataBind();
            Grid_Pagos_Excluidos.DataSource = null;
            Grid_Pagos_Excluidos.DataBind();
            Grid_Capturas.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            throw new Exception("Limpiar_Controles " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Llena_Grid_Capturas
    /// DESCRIPCIÓN: Llena el grid con las capturas de la base de datos
    /// PARÁMETROS:
    ///             1. Dt_Capturas: datatable con los datos a mostrar en el grid
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 16-ene-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Llena_Grid_Capturas(DataTable Dt_Capturas)
    {
        try
        {
            if (Dt_Capturas != null && Dt_Capturas.Rows.Count > 0)
            {
                Grid_Capturas.Columns[1].Visible = true;
                Grid_Capturas.Columns[10].Visible = true;
                Grid_Capturas.DataSource = Dt_Capturas;
                Grid_Capturas.DataBind();
                Grid_Capturas.Columns[1].Visible = false;
                Grid_Capturas.Columns[10].Visible = false;
            }
            else
            {
                Grid_Capturas.DataSource = null;
                Grid_Capturas.DataBind();

            }
        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_Capturas " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Consultar_Capturas
    /// DESCRIPCIÓN: Consulta las capturas en la base de datos con los filtros especificados
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 21-ene-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Consultar_Capturas()
    {
        var Consulta_Capturas = new Cls_Ope_Pre_Pagos_Instituciones_Externas_Negocio();
        DataTable Dt_Capturas;

        try
        {
            // si hay anio seleccionado, agregar parametro para la consulta
            if (Cmb_Filtrar_Anio.SelectedIndex > 0)
            {
                Consulta_Capturas.P_Anio = Cmb_Filtrar_Anio.SelectedValue;
            }
            // si hay institucion seleccionada, agregar para filtrar consulta
            if (Cmb_Filtrar_Institucion.SelectedIndex > 0)
            {
                Consulta_Capturas.P_Institucion_Id = Cmb_Filtrar_Institucion.SelectedValue;
            }

            Dt_Capturas = Consulta_Capturas.Consultar_Capturas_Pagos();
            // si la consulta regreso valores, llamar al metodo que los muestra en el grid
            if (Dt_Capturas != null)
            {
                Llena_Grid_Capturas(Dt_Capturas);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Capturas " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Cargar_Combo_Anios_Captura
    /// DESCRIPCIÓN: Llena el combo con los años en los que hay capturas
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 21-ene-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Cargar_Combo_Anios_Captura()
    {
        var Consulta_Capturas = new Cls_Ope_Pre_Pagos_Instituciones_Externas_Negocio();
        DataTable Dt_Capturas;

        try
        {
            Dt_Capturas = Consulta_Capturas.Consultar_Anio_Captura();
            // si la consulta regreso valores, llamar al metodo que los muestra en el combo
            if (Dt_Capturas != null && Dt_Capturas.Rows.Count > 0)
            {
            Cmb_Filtrar_Anio.DataSource = Dt_Capturas;
            Cmb_Filtrar_Anio.DataValueField = "ANIO";
            Cmb_Filtrar_Anio.DataTextField = "ANIO";
            Cmb_Filtrar_Anio.DataBind();
            Cmb_Filtrar_Anio.Items.Insert(0, "< SELECCIONE >");
            Cmb_Filtrar_Anio.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Combo_Anios_Captura: " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar_Combos_Instituciones
    /// DESCRIPCION: Consulta las instituciones que reciben pagos de predial en la 
    ///             base de datos con estatus VIGENTE y los carga en los combos Instituciones
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 16-ene-2012
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Combos_Instituciones()
    {
        var Consulta_Instituciones = new Cls_Cat_Pre_Instituciones_Recepcion_Pagos_Predial_Negocio();
        DataTable Dt_Instituciones;

        try
        {
            // consultar instituciones VIGENTES
            Consulta_Instituciones.P_Filtro = "";
            Consulta_Instituciones.P_Estatus = "VIGENTE";
            Dt_Instituciones = Consulta_Instituciones.Consultar_Institucion();
            Session["Dt_Instituciones"] = Dt_Instituciones;

            // cargar en los combos
            Cmb_Banco.DataSource = Dt_Instituciones;
            Cmb_Banco.DataValueField = Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion_Id;
            Cmb_Banco.DataTextField = Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion;
            Cmb_Banco.DataBind();
            Cmb_Banco.Items.Insert(0, "< SELECCIONE >");
            Cmb_Banco.SelectedIndex = 0;

            Cmb_Filtrar_Institucion.DataSource = Dt_Instituciones;
            Cmb_Filtrar_Institucion.DataValueField = Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion_Id;
            Cmb_Filtrar_Institucion.DataTextField = Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion;
            Cmb_Filtrar_Institucion.DataBind();
            Cmb_Filtrar_Institucion.Items.Insert(0, "< SELECCIONE >");
            Cmb_Filtrar_Institucion.SelectedIndex = 0;

        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Combos_Instituciones: " + ex.Message.ToString(), ex);
        }
    }

    ///******************************************************************************* 
    /// NOMBRE DE LA FUNCIÓN : Crear_Tabla_Pagos
    /// DESCRIPCIÓN          : Devuelve un DataTable con la estructura para los pagos
    /// PARAMETROS: 
    /// CREO                 : Roberto Gonzalez Oseguera
    /// FECHA_CREO           : 17-ene-2012
    /// MODIFICO: 
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Crear_Tabla_Pagos()
    {
        DataTable Dt_Pagos = new DataTable();
        Dt_Pagos.Columns.Add(new DataColumn(Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial, typeof(String)));
        Dt_Pagos.Columns.Add(new DataColumn(Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial_Id, typeof(String)));
        Dt_Pagos.Columns.Add(new DataColumn(Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Linea_Captura, typeof(String)));
        Dt_Pagos.Columns.Add(new DataColumn(Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Fecha_Pago, typeof(DateTime)));
        Dt_Pagos.Columns.Add(new DataColumn(Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Pagado, typeof(Decimal)));
        Dt_Pagos.Columns.Add(new DataColumn(Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Total_Adeudo, typeof(Decimal)));
        Dt_Pagos.Columns.Add(new DataColumn(Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Diferencia, typeof(Decimal)));
        Dt_Pagos.Columns.Add(new DataColumn(Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Sucursal, typeof(String)));
        Dt_Pagos.Columns.Add(new DataColumn(Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Tipo_Pago, typeof(String)));
        Dt_Pagos.Columns.Add(new DataColumn(Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Guia_Cie, typeof(String)));
        Dt_Pagos.Columns.Add(new DataColumn(Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cajero, typeof(Int32)));
        Dt_Pagos.Columns.Add(new DataColumn(Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Autorizacion, typeof(Int32)));
        Dt_Pagos.Columns.Add(new DataColumn(Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Incluido, typeof(String)));
        Dt_Pagos.Columns.Add(new DataColumn(Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Descripcion, typeof(String)));
        Dt_Pagos.Columns.Add(new DataColumn("CONSECUTIVO", typeof(Int32)));
        Dt_Pagos.Columns.Add(new DataColumn(Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Pagado_Original, typeof(Decimal)));
        Dt_Pagos.Columns.Add(new DataColumn(Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial_Original, typeof(String)));
        Dt_Pagos.Columns.Add(new DataColumn(Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Corriente, typeof(Decimal)));
        Dt_Pagos.Columns.Add(new DataColumn(Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Rezago, typeof(Decimal)));
        Dt_Pagos.Columns.Add(new DataColumn(Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Recargos, typeof(Decimal)));
        Dt_Pagos.Columns.Add(new DataColumn(Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Honorarios, typeof(Decimal)));
        Dt_Pagos.Columns.Add(new DataColumn(Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Descuento, typeof(Decimal)));

        return Dt_Pagos;
    }

    ///******************************************************************************* 
    /// NOMBRE DE LA FUNCIÓN : Crear_Tabla_Captura_Pagos
    /// DESCRIPCIÓN          : Devuelve un DataTable con la estructura para los datos 
    ///                     generales de carga de pagos
    /// PARAMETROS: 
    /// CREO                 : Roberto Gonzalez Oseguera
    /// FECHA_CREO           : 20-ene-2012
    /// MODIFICO: 
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Crear_Tabla_Captura_Pagos()
    {
        DataTable Dt_Pagos = new DataTable();
        Dt_Pagos.Columns.Add(new DataColumn(Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_No_Captura, typeof(String)));
        Dt_Pagos.Columns.Add(new DataColumn(Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Institucion_Id, typeof(String)));
        Dt_Pagos.Columns.Add(new DataColumn(Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Fecha_Captura, typeof(DateTime)));
        Dt_Pagos.Columns.Add(new DataColumn(Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Nombre_Archivo, typeof(String)));
        Dt_Pagos.Columns.Add(new DataColumn(Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Fecha_Corte, typeof(DateTime)));

        return Dt_Pagos;
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Leer_Datos_Archivo
    /// DESCRIPCION: Lee el archivo y llama a los metodos de cada banco para seprar los datos
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 16-ene-2012
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private String Leer_Datos_Archivo()
    {
        Cls_Ope_Pre_Parametros_Negocio Parametros = new Cls_Ope_Pre_Parametros_Negocio();
        DataTable Dt_Parametros = new DataTable();
        String Mensaje = "";
        StreamReader Sr_Archivo;
        Decimal Limite_Pago_Superior = 0;
        Decimal Limite_Pago_Inferior = 0;

        try
        {
            Cmb_Banco.Enabled = false;
            Dt_Parametros = Parametros.Consultar_Parametros_Cajas();
            if (Dt_Parametros != null && Dt_Parametros.Rows.Count > 0)
            {
                decimal.TryParse(Dt_Parametros.Rows[0][Ope_Pre_Parametros.Campo_Tolerancia_Pago_Superior].ToString(), out Limite_Pago_Superior);
                decimal.TryParse(Dt_Parametros.Rows[0][Ope_Pre_Parametros.Campo_Tolerancia_Pago_Inferior].ToString(), out Limite_Pago_Inferior);
            }

            Sr_Archivo = (StreamReader)Session["Archivo_Pagos"];

            // si no se pudo convertir el archivo, regresar mensaje
            if (Sr_Archivo == null)
            {
                return "No se pudo leer el archivo.";
            }

            // limpiar datos de captura
            Txt_Cantidad_Movimientos.Text = "";
            Div_Encabezado_Pagos_Incluidos.InnerText = "Pagos Incluidos";
            Div_Encabezado_Pagos_Excluidos.InnerText = "Pagos Excluidos";
            Grid_Pagos_Incluidos.DataSource = null;
            Grid_Pagos_Incluidos.DataBind();
            Grid_Pagos_Excluidos.DataSource = null;
            Grid_Pagos_Excluidos.DataBind();

            switch (Cmb_Banco.SelectedItem.Text)
            {
                case "BANAMEX":
                    Mensaje += Leer_Archivo_Banamex(Sr_Archivo, Limite_Pago_Inferior, Limite_Pago_Superior);
                    break;
                case "BANCOMER":
                    Mensaje += Leer_Archivo_Bancomer(Sr_Archivo, Limite_Pago_Inferior, Limite_Pago_Superior);
                    break;
                case "BANORTE":
                    Mensaje += Leer_Archivo_Banorte(Sr_Archivo, Limite_Pago_Inferior, Limite_Pago_Superior);
                    break;
                case "BAJIO":
                    Mensaje += Leer_Archivo_Bajio(Sr_Archivo, Limite_Pago_Inferior, Limite_Pago_Superior);
                    break;
                case "SANTANDER":
                    Mensaje += Leer_Archivo_Santander(Sr_Archivo, Limite_Pago_Inferior, Limite_Pago_Superior);
                    break;
                case "SCOTIABANK":
                    Mensaje += Leer_Archivo_Scotiabank(Sr_Archivo, Limite_Pago_Inferior, Limite_Pago_Superior);
                    break;
                case "HSBC":
                    Mensaje += Leer_Archivo_HSBC(Sr_Archivo, Limite_Pago_Inferior, Limite_Pago_Superior);
                    break;
                default:
                    if (Cmb_Banco.SelectedItem.Text.Contains("OXXO"))
                    {
                        Mensaje += Leer_Archivo_Oxxo(Sr_Archivo, Limite_Pago_Inferior, Limite_Pago_Superior);
                    }
                    else
                    {
                        Mensaje += "No existe algorítmo para leer los datos del archivo con la institución seleccionada.";
                    }
                    break;
            }

            // si regreso mensaje de error, mostrarlo
            if (Mensaje.Length > 0)
            {
                Cmb_Banco.Enabled = true;
            }
            else
            {
                // eliminar sesion con archivo de pagos
                Session.Remove("Archivo_Pagos");
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Leer_Datos_Archivo " + ex.Message.ToString(), ex);
        }
        return Mensaje;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Convertir_A_Fecha
    /// DESCRIPCIÓN: Trata de obtener una fecha a partir de los parametros recibidos y regresa un DateTime
    /// PARÁMETROS:
    /// 		1. Dia: cadena de caracteres con el dia del mes
    /// 		2. Mes: cadena de caracteres con el numero de mes
    /// 		3. Anio: cadena de caracteres con el año
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 17-ene-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private DateTime Convertir_A_Fecha(String Dia, String Mes, String Anio)
    {
        DateTime Fecha;
        if (Dic_Meses.ContainsKey(Convert.ToInt32(Mes)))
        {
            DateTime.TryParse(Dia + "-" + Dic_Meses[Convert.ToInt32(Mes)] + "-" + Anio, out Fecha);
        }
        else
        {
            Fecha = new DateTime();
        }
        return Fecha;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Cargar_Grids_Pagos
    /// DESCRIPCIÓN: Asigna el datasource de los grid de pagos incluidos y excluidos 
    ///             mediante linq, se obtienen del datatable Dt_Pagos en sesion, los 
    ///             pagos incluidos y excluidos en dos datatable para los grids
    /// PARÁMETROS:
    ///             1. Mostrar_Botones: boleano para mostrar u ocultar botones de los grids
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 17-ene-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Cargar_Grids_Pagos(bool Mostrar_Botones)
    {
        DataTable Dt_Pagos = null;

        try
        {
            // recuperar datatable de variable de sesion
            if (Session["Dt_Pagos"] != null)
            {
                Dt_Pagos = (DataTable)Session["Dt_Pagos"];
            }
            else
            {
                return;
            }

            // obtener las fila incluidas (campo INCLUIDO = "SI")
            var Pagos_Incluidos = (from Fila in Dt_Pagos.AsEnumerable()
                                   where Fila.Field<string>(Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Incluido) == "SI"
                                   select Fila).AsDataView().ToTable();
            // asignar al grid
            Div_Encabezado_Pagos_Incluidos.InnerText = "Pagos Incluidos " + Pagos_Incluidos.Rows.Count.ToString("#,##0");
            Grid_Pagos_Incluidos.Columns[11].Visible = true;
            Grid_Pagos_Incluidos.Columns[12].Visible = true;
            Grid_Pagos_Incluidos.Columns[13].Visible = true;
            Grid_Pagos_Incluidos.Columns[Grid_Pagos_Incluidos.Columns.Count - 1].Visible = Mostrar_Botones;
            Grid_Pagos_Incluidos.DataSource = Pagos_Incluidos;
            Grid_Pagos_Incluidos.DataBind();
            Grid_Pagos_Incluidos.Columns[11].Visible = false;
            Grid_Pagos_Incluidos.Columns[12].Visible = false;
            Grid_Pagos_Incluidos.Columns[13].Visible = false;

            // obtener las fila excluidas (campo INCLUIDO = "NO")
            var Pagos_Excluidos = (from Fila in Dt_Pagos.AsEnumerable()
                                   where Fila.Field<string>(Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Incluido) == "NO"
                                   select Fila).AsDataView().ToTable();
            // asignar al grid
            Div_Encabezado_Pagos_Excluidos.InnerText = "Pagos Excluidos " + Pagos_Excluidos.Rows.Count.ToString("#,##0");
            Grid_Pagos_Excluidos.Columns[11].Visible = true;
            Grid_Pagos_Excluidos.Columns[12].Visible = true;
            Grid_Pagos_Excluidos.Columns[13].Visible = true;
            Grid_Pagos_Excluidos.Columns[0].Visible = Mostrar_Botones;
            Grid_Pagos_Excluidos.Columns[Grid_Pagos_Excluidos.Columns.Count - 1].Visible = Mostrar_Botones;
            Grid_Pagos_Excluidos.DataSource = Pagos_Excluidos;
            Grid_Pagos_Excluidos.DataBind();
            Grid_Pagos_Excluidos.Columns[11].Visible = false;
            Grid_Pagos_Excluidos.Columns[12].Visible = false;
            Grid_Pagos_Excluidos.Columns[13].Visible = false;

        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Grids_Pagos " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Configurar_Visibilidad_Controles_Grid_Pagos_Excluidos
    /// DESCRIPCIÓN: Recupera los controles de la fila del control recibido como parámetro y establece la visibilidad de los mismos
    /// PARÁMETROS:
    /// 		1. sender: objeto origen para encontrar los controles a modificar
    /// 		2. Visible: boleano con el que se establecera la visibilidad de los controles
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 18-ene-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Configurar_Visibilidad_Controles_Grid_Pagos_Excluidos(object sender, bool Visible)
    {
        GridViewRow Fila_Grid;
        ImageButton Btn_Original = (ImageButton)sender;
        ImageButton Btn_Editar_Pago;
        ImageButton Btn_Incluir_Pago;
        Label Lbl_Grid_Monto_Pagado;
        TextBox Txt_Grid_Monto_Pagado;
        Label Lbl_Grid_Cuenta_Predial;
        TextBox Txt_Grid_Cuenta_Predial;
        ImageButton Btn_Aplicar_Cambios_Pago_Excluido;
        ImageButton Btn_Deshacer_Cambios_Pago_Excluido;

        try
        {
            // verificar que el objeto origen recibido no sea nulo
            if (Btn_Original!= null)
            {
                // ubicar el grid a partir del objeto que disparo el evento y validar que no sea nulo
                Fila_Grid = (GridViewRow)FindControl(Btn_Original.Parent.Parent.UniqueID);
                if (Fila_Grid != null)
                {
                    Lbl_Grid_Monto_Pagado = (Label)Fila_Grid.Cells[4].FindControl("Lbl_Grid_Monto_Pagado");
                    Txt_Grid_Monto_Pagado = (TextBox)Fila_Grid.Cells[4].FindControl("Txt_Grid_Monto_Pagado");
                    Lbl_Grid_Cuenta_Predial = (Label)Fila_Grid.Cells[9].FindControl("Lbl_Grid_Cuenta_Predial");
                    Txt_Grid_Cuenta_Predial = (TextBox)Fila_Grid.Cells[9].FindControl("Txt_Grid_Cuenta_Predial");
                    Btn_Editar_Pago = (ImageButton)Fila_Grid.Cells[0].FindControl("Btn_Editar_Pago_Excluido");
                    Btn_Incluir_Pago = (ImageButton)Fila_Grid.Cells[11].FindControl("Btn_Incluir_Pago");
                    Btn_Aplicar_Cambios_Pago_Excluido = (ImageButton)Fila_Grid.Cells[0].FindControl("Btn_Aplicar_Cambios_Pago_Excluido");
                    Btn_Deshacer_Cambios_Pago_Excluido = (ImageButton)Fila_Grid.Cells[11].FindControl("Btn_Deshacer_Cambios_Pago_Excluido");
                    // etiquetas y cajas de texto de monto pagado y cuenta predial
                    if (Lbl_Grid_Monto_Pagado != null)
                    {
                        Lbl_Grid_Monto_Pagado.Visible = !Visible;
                    }
                    if (Txt_Grid_Monto_Pagado != null)
                    {
                        Txt_Grid_Monto_Pagado.Visible = Visible;
                    }
                    if (Lbl_Grid_Cuenta_Predial != null)
                    {
                        Lbl_Grid_Cuenta_Predial.Visible = !Visible;
                    }
                    if (Txt_Grid_Cuenta_Predial != null)
                    {
                        Txt_Grid_Cuenta_Predial.Visible = Visible;
                    }
                    // botones edicion e incluir pago y aplicar y deshacer cambios
                    if (Btn_Editar_Pago != null)
                    {
                        Btn_Editar_Pago.Visible = !Visible;
                    }
                    if (Btn_Incluir_Pago != null)
                    {
                        Btn_Incluir_Pago.Visible = !Visible;
                    }
                    if (Btn_Aplicar_Cambios_Pago_Excluido != null)
                    {
                        Btn_Aplicar_Cambios_Pago_Excluido.Visible = Visible;
                    }
                    if (Btn_Deshacer_Cambios_Pago_Excluido != null)
                    {
                        Btn_Deshacer_Cambios_Pago_Excluido.Visible = Visible;
                    }
                }
            }

        }
        catch (Exception ex)
        {
            throw new Exception("Configurar_Visibilidad_Controles_Grid_Pagos_Excluidos " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Consultar_Adeudos_Cuenta_Predial
    /// DESCRIPCIÓN: Consulta el adeudo predial de una cuenta y regresa el adeudo desglosado en los mismos parámetros, o un mensaje de error
    /// PARÁMETROS:
    /// 		1. Cuenta_Predial_ID: ID de la cuenta predial a consultar
    /// 		2. Total_Adeudo: variable en la que se va a regresar el adeudo total
    /// 		3. Corriente: variable en la que se va a regresar el adeudo por concepto de impuesto predial corriente
    /// 		4. Rezago: variable en la que se va a regresar el adeudo por concepto de impuesto predial rezago
    /// 		5. Honorarios: variable en la que se va a regresar el adeudo por concepto de honorarios
    /// 		6. Recargos: variable en la que se va a regresar el adeudo por concepto de recargos
    /// 		7. Periodo_Corriente: bimestres incluidos en periodo corriente
    /// 		8. Periodo_Rezago: bimestres incluidos en periodo rezago
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 19-ene-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected DataTable Consultar_Adeudos_Cuenta_Predial(
        string Cuenta_Predial_ID, 
        DateTime Fecha_Pago, 
        out decimal Total_Adeudo, 
        out decimal Corriente, 
        out decimal Rezago, 
        out decimal Honorarios, 
        out decimal Recargos,
        out string Periodo_Corriente,
        out string Periodo_Rezago)
    {
        Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Consulta_Adeudos = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
        DataTable Dt_ADeudos;

        Honorarios = 0;
        
        try
        {
            Consulta_Adeudos.p_Mes_Tabulador_Utilizar = Fecha_Pago.ToString("MMMM").ToUpper();
            Dt_ADeudos = Consulta_Adeudos.Calcular_Recargos_Predial(Cuenta_Predial_ID);
            // tomar valores de las propiedades
            Corriente = Consulta_Adeudos.p_Total_Corriente;
            Rezago = Consulta_Adeudos.p_Total_Rezago;
            Recargos = Consulta_Adeudos.p_Total_Recargos_Generados;
            Total_Adeudo = Convert.ToDecimal((Corriente + Rezago + Recargos + Honorarios).ToString("#,##0.00"));
            Periodo_Corriente = Consulta_Adeudos.p_Periodo_Corriente;
            Periodo_Rezago = Consulta_Adeudos.p_Periodo_Rezago;
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Adeudos_Cuenta_Predial " + ex.Message.ToString(), ex);
        }

        return Dt_ADeudos;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Consultar_Cuenta_Predial_ID
    /// DESCRIPCIÓN: Regresar el id de la cuenta predial, se busca mediante la cuenta predial 
    /// PARÁMETROS:
    /// 		1. Cuenta_Predial: cuenta predial a localizar
    /// 		2. Tipo_Predio_ID: variable para asignar el tipo de predio
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 19-ene-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private String Consultar_Cuenta_Predial_ID(String Cuenta_Predial, out string Tipo_Predio_ID)
    {
        var Consulta_Cuenta = new Cls_Ope_Pre_Pagos_Instituciones_Externas_Negocio();
        String Cuenta_Predial_ID = "";
        Tipo_Predio_ID = "";
        DataTable Dt_Resultado_Consulta;

        try
        {
            if (!String.IsNullOrEmpty(Cuenta_Predial))
            {
                // consultar cuenta predial
                Consulta_Cuenta.P_Cuenta_Predial = Cuenta_Predial;
                Dt_Resultado_Consulta = Consulta_Cuenta.Consultar_Cuenta_Predial_ID();
                if (Dt_Resultado_Consulta != null && Dt_Resultado_Consulta.Rows.Count > 0)
                {
                    Cuenta_Predial_ID = Dt_Resultado_Consulta.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
                    Tipo_Predio_ID = Dt_Resultado_Consulta.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Cuenta_Predial_ID " + ex.Message.ToString(), ex);
        }
        return Cuenta_Predial_ID;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Consultar_Cuenta_Predial_ID
    /// DESCRIPCIÓN: Regresar el id de la cuenta predial, se busca mediante la cuenta predial 
    /// PARÁMETROS:
    /// 		1. Linea_Captura: Linea de captura a localizar
    /// 		2. Cuenta_Predial: cuenta predial a localizar
    /// 		3. Institucion_Id: ID de la institucion a consultar
    /// 		4. Tipo_Predio_ID: variable para asignar el tipo de predio
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 19-ene-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private String Consultar_Cuenta_Predial_ID(String Linea_Captura, out String Cuenta_Predial, String Institucion_Id, out string Tipo_Predio_ID)
    {
        var Consulta_Cuenta = new Cls_Ope_Pre_Pagos_Instituciones_Externas_Negocio();
        String Cuenta_Predial_ID = "";
        Cuenta_Predial = "";
        Tipo_Predio_ID = "";
        DataTable Dt_Cuenta_Predial;

        try
        {
            if (!String.IsNullOrEmpty(Linea_Captura))
            {
                // consultar cuenta predial
                Consulta_Cuenta.P_Linea_Captura = Linea_Captura;
                Consulta_Cuenta.P_Columna_Linea_Captura = "";
                Consulta_Cuenta.P_Institucion_Id = Institucion_Id;
                Dt_Cuenta_Predial = Consulta_Cuenta.Consultar_Linea_Captura();
                // si la consulta regreso resultados, recuperar cuenta predial y ID
                if (Dt_Cuenta_Predial != null && Dt_Cuenta_Predial.Rows.Count > 0)
                {
                    Cuenta_Predial = Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial].ToString();
                    Cuenta_Predial_ID = Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
                    Tipo_Predio_ID = Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Cuenta_Predial_ID " + ex.Message.ToString(), ex);
        }
        return Cuenta_Predial_ID;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Consultar_Propietario
    /// DESCRIPCIÓN: Regresar el nombre del propietario de la cuenta predial
    /// PARÁMETROS:
    /// 		1. Cuenta_Predial_ID: id de la cuenta predial a 
    /// 		2. Consulta_Propietario_Negocio: instancia de la clase de negocio para consultar 
    /// 		        propietario, si es null, se genera nueva instancia
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 02-abr-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private String Consultar_Propietario(String Cuenta_Predial_ID, Cls_Ope_Pre_Pagos_Instituciones_Externas_Negocio Consulta_Propietario_Negocio)
    {
        String Propietario = "";
        DataTable Dt_Resultado_Consulta;

        if (Consulta_Propietario_Negocio == null)
        {
            Consulta_Propietario_Negocio = new Cls_Ope_Pre_Pagos_Instituciones_Externas_Negocio();
        }
        try
        {
            if (!String.IsNullOrEmpty(Cuenta_Predial_ID))
            {
                // consultar cuenta predial
                Consulta_Propietario_Negocio.P_Cuenta_Predial_Id = Cuenta_Predial_ID;
                Dt_Resultado_Consulta = Consulta_Propietario_Negocio.Consultar_Propietario();
                if (Dt_Resultado_Consulta != null && Dt_Resultado_Consulta.Rows.Count > 0)
                {
                    Propietario = Dt_Resultado_Consulta.Rows[0]["NOMBRE_PROPIETARIO"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Cuenta_Predial_ID " + ex.Message.ToString(), ex);
        }
        return Cuenta_Predial_ID;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Consultar_Tipos_Predio
    /// DESCRIPCIÓN: Regresar un diccionario con el tipo de predio y su ID
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 31-mar-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private Dictionary<string, string> Consultar_Tipos_Predio()
    {
        var Consulta_Tipos_Predio = new Cls_Cat_Pre_Tipos_Predio_Negocio();
        var Dic_Tipos_Predio = new Dictionary<string, string>();
        DataTable Dt_Tipos_Predio;

        try
        {
                // consultar tipos de predio
                Dt_Tipos_Predio = Consulta_Tipos_Predio.Consultar_Tipo_Predio();
                // si la consulta regreso resultados, recuperar cuenta predial y ID
                if (Dt_Tipos_Predio != null)
                {
                    foreach (DataRow Dr_Tipo_Predio in Dt_Tipos_Predio.Rows)
                    {
                        if (!Dic_Tipos_Predio.ContainsKey(Dr_Tipo_Predio[Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID].ToString()))
                        {
                            Dic_Tipos_Predio.Add(Dr_Tipo_Predio[Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID].ToString(), Dr_Tipo_Predio[Cat_Pre_Tipos_Predio.Campo_Descripcion].ToString());
                        }
                    }
                }
            
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Tipos_Predio " + ex.Message.ToString(), ex);
        }
        return Dic_Tipos_Predio;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Obtener_Clave_Ingreso
    ///DESCRIPCIÓN: Obtiene la Clave de Ingreso para el tipo ingresado.
    ///PARAMETROS: 
    ///         1. Tipo: Tipo de la Clave que se buscara
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 02-mar-2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private String Obtener_Clave_Ingreso(String Tipo)
    {
        String Clave = null;
        var Consulta_Claves_Ingreso = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
        Consulta_Claves_Ingreso.P_Tipo = Tipo;
        Consulta_Claves_Ingreso.P_Tipo_Predial_Traslado = "PREDIAL";
        DataTable Dt_Temporal = Consulta_Claves_Ingreso.Consultar_Clave_Ingreso();
        if (Dt_Temporal != null && Dt_Temporal.Rows.Count > 0)
        {
            Clave = Dt_Temporal.Rows[0]["CLAVE"].ToString();
        }
        return Clave;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Obtener_Diccionario_Claves_Ingreso
    ///DESCRIPCIÓN: Obtiene la Clave de Ingreso para el tipo ingresado y lo regresa en dos diccionarios.
    ///PARAMETROS: 
    ///         1. Dic_Clave_Dependencia: diccionario en el que se van a regresar los valores de: Clave de ingreso-Dependencia
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 02-mar-2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Dictionary<string, string> Obtener_Diccionario_Claves_Ingreso(out Dictionary<string, string> Dic_Clave_Dependencia)
    {
        var Dic_Claves_Ingreso = new Dictionary<string, string>();
        Dic_Clave_Dependencia = new Dictionary<string, string>();
        var Consulta_Claves_Ingreso = new Cls_Ope_Pre_Pagos_Instituciones_Externas_Negocio();
        Consulta_Claves_Ingreso.P_Filtro_Dinamico = "D." + Cat_Pre_Claves_Ingreso_Det.Campo_Tipo + " = " + "'PREDIAL'";
        DataTable Dt_Claves = Consulta_Claves_Ingreso.Consultar_Claves_Ingreso();

        // si la consulta obtuvo resultados, recorrer el datatable
        if (Dt_Claves != null)
        {
            foreach (DataRow Dr_Clave in Dt_Claves.Rows)
            {
                // agregar al diccionario nuevo tipo-clave
                if (!Dic_Claves_Ingreso.ContainsKey(Dr_Clave[Cat_Pre_Claves_Ingreso_Det.Campo_Tipo_Predial_Traslado].ToString()))
                {
                    Dic_Claves_Ingreso.Add(Dr_Clave[Cat_Pre_Claves_Ingreso_Det.Campo_Tipo_Predial_Traslado].ToString(), Dr_Clave[Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID].ToString());
                }
                // agregar nueva clave-dependiencia al diccionario
                if (!Dic_Clave_Dependencia.ContainsKey(Dr_Clave[Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID].ToString()))
                {
                    Dic_Clave_Dependencia.Add(Dr_Clave[Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID].ToString(), Dr_Clave[Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID].ToString());
                }
            }
        }

        return Dic_Claves_Ingreso;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Alta_Captura_Pagos
    /// DESCRIPCIÓN: Dar de alta los datos de la captura desde las tablas en la variable de sesion
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 15-ene-20112
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private string Alta_Captura_Pagos()
    {
        DataTable Dt_Pagos;
        DataTable Dt_Captura;
        DataTable Dt_Adeudos_Totales;
        DataTable Dt_Adeudo_Detallado_Predial;
        DataTable Dt_Pasivos_Pago;
        var Alta_Captura_Pagos = new Cls_Ope_Pre_Pagos_Instituciones_Externas_Negocio();
        string Mensaje = "";

        OracleConnection Cn = new OracleConnection();
        OracleCommand Cmd = new OracleCommand();
        OracleTransaction Trans = null;

        Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
        Cn.Open();
        Trans = Cn.BeginTransaction();
        Cmd.Connection = Cn;
        Cmd.Transaction = Trans;

        try
        {

            Dt_Pagos = (DataTable)Session["Dt_Pagos"];
            Dt_Captura = (DataTable)Session["Dt_Captura"];
            Dt_Adeudos_Totales = (DataTable)Session["Dt_Adeudos_Totales"];
            Dt_Adeudo_Detallado_Predial = (DataTable)Session["Dt_Adeudo_Detallado_Predial"];
            Dt_Pasivos_Pago = (DataTable)Session["Dt_Pasivos_Pago"];

            // verificar que se recuperaron las tablas desde sesión y que contenga datos
            if (Dt_Pagos != null && Dt_Captura != null && Dt_Adeudos_Totales != null && Dt_Adeudo_Detallado_Predial != null && Dt_Pasivos_Pago != null
                && Dt_Pagos.Rows.Count > 0 && Dt_Captura.Rows.Count > 0 && Dt_Adeudos_Totales.Rows.Count > 0 && Dt_Adeudo_Detallado_Predial.Rows.Count > 0 && Dt_Pasivos_Pago.Rows.Count > 0)
            {
                Alta_Captura_Pagos.P_Dt_Capturas = Dt_Captura;
                Alta_Captura_Pagos.P_Dt_Detalles_Captura = Dt_Pagos;
                Alta_Captura_Pagos.P_Dt_Adeudos_Totales = Dt_Adeudos_Totales;
                Alta_Captura_Pagos.P_Dt_Adeudo_Detallado_Predial = Dt_Adeudo_Detallado_Predial;
                Alta_Captura_Pagos.P_Dt_Pasivos_Pago = Dt_Pasivos_Pago;
                Alta_Captura_Pagos.P_Comando_Oracle = Cmd;
                Alta_Captura_Pagos.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                Alta_Captura_Pagos.P_Caja_ID = Hdn_Caja_Id.Value;
                // llamar metodo Alta_Captura_Pagos y mostrar mensaje dependiendo de si hubo inserciones o no
                if (Alta_Captura_Pagos.Alta_Captura_Pagos() > 0)
                {
                    // aplicar cambios a la base de datos 
                    Trans.Commit();
                    Btn_Imprimir_Click(null, null);
                    //limpiar controles
                    Inicializa_Controles();
                    ScriptManager.RegisterStartupScript(this, this.GetType(),
                        "Pagos en instituciones externas", "alert('Los datos se dieron de alta con éxito');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pagos en instituciones externas", "alert('Ocurrió un error y los datos no se guardaron en la base de datos');", true);
                }
            }
            else
            {
                Mensaje = "No se encontraron datos para aplicar pagos, vuelva a intentar cargar el archivo de pagos.";
            }

        }
        catch (OracleException Ex)
        {
            Trans.Rollback();
            throw new Exception("Error: " + Ex.Message);
        }
        catch (Exception ex)
        {
            Trans.Rollback();
            throw new Exception("Alta_Captura_Pagos: " + ex.Message.ToString(), ex);
        }
        finally
        {
            Cn.Close();
        }
        return Mensaje;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Cargar_Detalles_Captura
    /// DESCRIPCIÓN: Consulta los detalles de la captura indicada como parametro y llama al 
    ///             metodo para que cargue en los controles los datos 
    /// PARÁMETROS:
    /// 		1. No_Captura: Numero de captura de la que se van a mostrar detalles
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 21-ene-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Cargar_Detalles_Captura(String No_Captura)
    {
        var Consulta_Detalles = new Cls_Ope_Pre_Pagos_Instituciones_Externas_Negocio();
        DataTable Dt_Detalles;
        int Numero_Captura;

        try
        {
            if (!String.IsNullOrEmpty(No_Captura))
            {
                int.TryParse(Grid_Capturas.SelectedRow.Cells[1].Text, out Numero_Captura);
                // cargar datos generales de da captura
                Txt_Caja.Text = Grid_Capturas.SelectedRow.Cells[3].Text;
                Txt_Cantidad_Movimientos.Text = Grid_Capturas.SelectedRow.Cells[7].Text;
                // si la institución no está en el combo, agrgarla
                if (Cmb_Banco.Items.FindByValue(Grid_Capturas.SelectedRow.Cells[10].Text) == null)
                {
                    Cmb_Banco.Items.Add(new ListItem(Grid_Capturas.SelectedRow.Cells[2].Text, Grid_Capturas.SelectedRow.Cells[10].Text));
                }
                Cmb_Banco.SelectedValue = Grid_Capturas.SelectedRow.Cells[10].Text;
                Txt_Fecha.Text = Grid_Capturas.SelectedRow.Cells[4].Text;
                Txt_Numero_Captura.Text = Numero_Captura.ToString("#,##0");
                Txt_Nombre_Archivo.Text = Grid_Capturas.SelectedRow.Cells[6].Text;
                // consultar detalles
                Consulta_Detalles.P_No_Captura = Grid_Capturas.SelectedRow.Cells[1].Text;
                Dt_Detalles = Consulta_Detalles.Consultar_Detalles_Captura();
                // validar si la consulta regreso resultados
                if (Dt_Detalles != null && Dt_Detalles.Rows.Count > 0)
                {
                    Session["Dt_Pagos"] = Dt_Detalles;
                    // cargar grids
                    Cargar_Grids_Pagos(false);
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Detalles_Captura " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Exportar_Reporte
    /// DESCRIPCIÓN: Genera el reporte de Crystal con los datos proporcionados en el DataTable 
    /// PARÁMETROS:
    /// 		1. Ds_Convenio: Dataset con datos a imprimir
    /// 		2. Nombre_Reporte: Nombre del archivo de reporte .rpt
    /// 		3. Nombre_Archivo: Nombre del archivo a generar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 04-sep-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Exportar_Reporte(DataSet Ds_Reporte, String Nombre_Reporte, String Nombre_Archivo, String Extension_Archivo, ExportFormatType Formato)
    {
        ReportDocument Reporte = new ReportDocument();
        String Ruta = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte);

        try
        {
            // si la tabla no trae datos, mostrar mensaje
            if (Ds_Reporte.Tables[1].Rows.Count <= 0)
            {
                Lbl_Mensaje_Error.Text = "No se encontraron registros con el criterio seleccionado.";
                Lbl_Mensaje_Error.Visible = true;
                return;
            }

            Reporte.Load(Ruta);
            Reporte.SetDataSource(Ds_Reporte);
        }
        catch
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "No se pudo cargar el reporte para su impresión";
        }

        String Archivo_Convenio = Nombre_Archivo + "." + Extension_Archivo;  // formar el nombre del archivo a generar 
        try
        {
            ExportOptions Export_Options_Calculo = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options_Calculo = new DiskFileDestinationOptions();
            Disk_File_Destination_Options_Calculo.DiskFileName = Server.MapPath("../../Reporte/" + Archivo_Convenio);
            Export_Options_Calculo.ExportDestinationOptions = Disk_File_Destination_Options_Calculo;
            Export_Options_Calculo.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options_Calculo.ExportFormatType = Formato;
            Reporte.Export(Export_Options_Calculo);

            Mostrar_Reporte(Archivo_Convenio, "Reporte");
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
    /// FECHA_CREO: 25-ene-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Mostrar_Reporte(String Nombre_Reporte, String Tipo)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            Pagina = Pagina + Nombre_Reporte;
            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "Formato",
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

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Exportar_Reporte
    ///DESCRIPCIÓN          : Crea un Dataset con los datos de la consulta de convenios
    ///PARAMETROS: 
    ///CREO                 : Roberto Gonzalez Oseguera
    ///FECHA_CREO           : 11-ene-2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Exportar_Reporte()
    {
        var Ds_Pagos = new Ds_Ope_Pre_Pagos_Instituciones_Externas();
        DataTable Dt_Pagos;

        // agregar datos generales al dataset
        var Dr_Datos_Generales = Ds_Pagos.Tables[0].NewRow();
        Dr_Datos_Generales["Titulo"] = "Pagos en instituciones externas";
        Dr_Datos_Generales["Subtitulo"] = Txt_Cantidad_Movimientos.Text + " movimientos capturados el " + Txt_Fecha.Text + ": " + Cmb_Banco.SelectedItem.Text + " (" + Txt_Nombre_Archivo.Text + ")";
        Dr_Datos_Generales["Fecha_Captura"] = Txt_Fecha.Text;
        Ds_Pagos.Tables[0].Rows.Add(Dr_Datos_Generales);

        // recuperar tabla pagos de la base de datos
        Dt_Pagos=(DataTable)Session["Dt_Pagos"];
        // agregar tabla obtenida de la consulta al dataset
        if (Dt_Pagos != null)
        {
            Dt_Pagos.TableName = "Dt_Pagos";
            Ds_Pagos.Tables.Remove("Dt_Pagos");
            Ds_Pagos.Tables.Add(Dt_Pagos.Copy());
        }

        return Ds_Pagos;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Obtener_Descuentos_Pronto_Pago
    /// DESCRIPCIÓN: Obtiene los descuentos del catalogo
    /// PARÁMETROS:
    /// 		1. Mes: cadena de caracteres con el nombre del mes a consultar
    /// 		2. Anio: entero con el año de los descuentos a consultar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 25-ene-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private decimal Obtener_Descuentos_Pronto_Pago(string Mes, int Anio)
    {
        decimal Descuento = 0;
        Cls_Cat_Pre_Descuentos_Predial_Negocio Consulta_Descuentos = new Cls_Cat_Pre_Descuentos_Predial_Negocio();

        // consulta adeudo
        Consulta_Descuentos.P_Anio = Anio;
        Consulta_Descuentos = Consulta_Descuentos.Consultar_Datos_Descuento_Predial();

        // recuperar monto descuento dependiendo del mes
        switch (Mes)
        {
            case "ENERO":
                Decimal.TryParse(Consulta_Descuentos.P_Enero.ToString(), out Descuento);
                break;
            case "FEBRERO":
                Decimal.TryParse(Consulta_Descuentos.P_Febrero.ToString(), out Descuento);
                break;
            case "MARZO":
                Decimal.TryParse(Consulta_Descuentos.P_Marzo.ToString(), out Descuento);
                break;
            case "ABRIL":
                Decimal.TryParse(Consulta_Descuentos.P_Abril.ToString(), out Descuento);
                break;
            case "MAYO":
                Decimal.TryParse(Consulta_Descuentos.P_Mayo.ToString(), out Descuento);
                break;
            case "JUNIO":
                Decimal.TryParse(Consulta_Descuentos.P_Junio.ToString(), out Descuento);
                break;
            case "JULIO":
                Decimal.TryParse(Consulta_Descuentos.P_Julio.ToString(), out Descuento);
                break;
            case "AGOSTO":
                Decimal.TryParse(Consulta_Descuentos.P_Agosto.ToString(), out Descuento);
                break;
            case "SEPTIEMBRE":
                Decimal.TryParse(Consulta_Descuentos.P_Septiembre.ToString(), out Descuento);
                break;
            case "OCTUBRE":
                Decimal.TryParse(Consulta_Descuentos.P_Octubre.ToString(), out Descuento);
                break;
            case "NOVIEMBRE":
                Decimal.TryParse(Consulta_Descuentos.P_Noviembre.ToString(), out Descuento);
                break;
            case "DICIEMBRE":
                Decimal.TryParse(Consulta_Descuentos.P_Diciembre.ToString(), out Descuento);
                break;
        }

        return Descuento;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Obtener_Cuota_Minima
    /// DESCRIPCIÓN: Consultar la cuota minima del anio proporcionado
    /// PARÁMETROS:
    ///         1. Anio: Entero que especifica el anio en el que se consulta la cuota minima
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 25-ene-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ:
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************************************
    public Decimal Obtener_Cuota_Minima(Int32 Anio)
    {
        DataTable Dt_Cuotas_Minimas;
        decimal Cuota_Minima = 0;
        var Cuotas_Minima = new Cls_Cat_Pre_Cuotas_Minimas_Negocio();

        try
        {
            Cuotas_Minima.P_Anio = Anio.ToString();
            Dt_Cuotas_Minimas = Cuotas_Minima.Consultar_Cuotas_Minimas();

            if (Dt_Cuotas_Minimas!=null && Dt_Cuotas_Minimas.Rows.Count>0)
            {
                // convertir y validar año y cuota
                Decimal.TryParse(Dt_Cuotas_Minimas.Rows[0]["CUOTA"].ToString(), out Cuota_Minima);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Obtener_Cuota_Minima: " + ex.Message.ToString(), ex);
        }
        return Cuota_Minima;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Crear_Tabla_Adeudos_Predial
    /// DESCRIPCIÓN: Crea la Tabla con el listado y los montos de los adeudos.
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 26-ene-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private DataTable Crear_Tabla_Adeudos_Predial()
    {
        DataTable Dt_Adeudos_Predial = new DataTable();

        Dt_Adeudos_Predial.Columns.Add("NO_CUENTA", Type.GetType("System.String"));
        Dt_Adeudos_Predial.Columns.Add("NO_ADEUDO", Type.GetType("System.String"));
        Dt_Adeudos_Predial.Columns.Add("NO_CONVENIO", Type.GetType("System.String"));
        Dt_Adeudos_Predial.Columns.Add("NO_PAGO", Type.GetType("System.String"));
        Dt_Adeudos_Predial.Columns.Add("BIMESTRE", Type.GetType("System.Int32"));
        Dt_Adeudos_Predial.Columns.Add("ANIO", Type.GetType("System.Int32"));
        Dt_Adeudos_Predial.Columns.Add("MONTO", Type.GetType("System.Decimal"));

        return Dt_Adeudos_Predial;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Crear_Tabla_Adeudos_Totales
    /// DESCRIPCIÓN: Crea la Tabla con los montos totales del adeudo
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 26-ene-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private DataTable Crear_Tabla_Adeudos_Totales()
    {
        DataTable Dt_Adeudos_Predial = new DataTable();

        Dt_Adeudos_Predial.Columns.Add("NO_CUENTA", Type.GetType("System.String"));
        Dt_Adeudos_Predial.Columns.Add("CONCEPTO", Type.GetType("System.String"));
        Dt_Adeudos_Predial.Columns.Add("MONTO", Type.GetType("System.Decimal"));
        Dt_Adeudos_Predial.Columns.Add("REFERENCIA", Type.GetType("System.String"));

        return Dt_Adeudos_Predial;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Crear_Tabla_Pasivos_Pago
    /// DESCRIPCIÓN: Crea la Tabla con las columnas para insertar pasivo del pago
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 30-mar-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private DataTable Crear_Tabla_Pasivos_Pago()
    {
        DataTable Dt_Pasivos = new DataTable();

        Dt_Pasivos.Columns.Add(new DataColumn("Cuenta_Predial", typeof(String)));
        Dt_Pasivos.Columns.Add(new DataColumn("Cuenta_Predial_Id", typeof(String)));
        Dt_Pasivos.Columns.Add(new DataColumn("Clave_Ingreso", typeof(String)));
        Dt_Pasivos.Columns.Add(new DataColumn("Descripcion", typeof(String)));
        Dt_Pasivos.Columns.Add(new DataColumn("Fecha_Tramite", typeof(DateTime)));
        Dt_Pasivos.Columns.Add(new DataColumn("Fecha_Vencimiento", typeof(DateTime)));
        Dt_Pasivos.Columns.Add(new DataColumn("Monto", typeof(Decimal)));
        Dt_Pasivos.Columns.Add(new DataColumn("Estatus", typeof(String)));
        Dt_Pasivos.Columns.Add(new DataColumn("Dependencia", typeof(String)));
        Dt_Pasivos.Columns.Add(new DataColumn("Contribuyente", typeof(String)));

        return Dt_Pasivos;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Agregar_Adeudos_Tablas
    /// DESCRIPCIÓN: Agregar filas a la tabla de adeudos de predial desde la tabla que 
    ///             se obtiene del calculo de recargos
    /// PARÁMETROS:
    /// 		1. Dt_Adeudos: Tabla de la que se toman los adeudos
    /// 		2. Dt_Adeudo_Detallado_Predial: tabla en la que se van a insertar nuevos registros
    /// 		3. Cuenta_Predial_ID: Id de la cuenta predial con la que se relacionan los adeudos
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 27-ene-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Agregar_Adeudos_Predial(DataTable Dt_Adeudos, DataTable Dt_Adeudo_Detallado_Predial, String Cuenta_Predial_ID)
    {
        DataRow Dr_Adeudo;
        int Anio;
        int Bimestre;

        if (Dt_Adeudos != null)
        {
            foreach (DataRow Fila_Adeudo in Dt_Adeudos.Rows)
            {
                Dr_Adeudo = Dt_Adeudo_Detallado_Predial.NewRow();

                int.TryParse(Fila_Adeudo["PERIODO"].ToString().Substring(0, 1), out Bimestre);
                int.TryParse(Fila_Adeudo["PERIODO"].ToString().Substring(1, 4), out Anio);

                Dr_Adeudo["NO_CUENTA"] = Cuenta_Predial_ID;
                Dr_Adeudo["BIMESTRE"] = Bimestre;
                Dr_Adeudo["ANIO"] = Anio;
                Dr_Adeudo["MONTO"] = (decimal)Fila_Adeudo["ADEUDO"];
                Dr_Adeudo["NO_ADEUDO"] = Fila_Adeudo["NO_ADEUDO"];

                Dt_Adeudo_Detallado_Predial.Rows.Add(Dr_Adeudo);
            } 
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Agregar_Pasivos_Adeudo
    /// DESCRIPCIÓN: Agregar filas a la tabla de adeudos de predial desde la tabla que 
    ///             se obtiene del calculo de recargos
    /// PARÁMETROS:
    /// 		1. Dt_Pasivos: tabla en la que se agregarán los pasivos
    /// 		2. Adeudo_Corriente: monto de adeudo predial corriente
    /// 		3. Adeudo_Rezago: monto de adeudo predial rezago
    /// 		4. Adeudo_Recargos: monto de adeudo por recargos
    /// 		5. Adeudo_Honorarios: monto de adeudo honorarios
    /// 		6. Descuento: descuento por pronto pago
    /// 		7. Cuenta_Predial: número de cuenta
    /// 		8. Cuenta_Predial_ID: id de cuenta predial
    /// 		9. Tipo_Predio: tipo de predio
    /// 		10. Periodo_Corriente: cadena de caracteres con el periodo incluido en el pago
    /// 		11. Periodo_Rezago: cadena de caracteres con el periodo incluido en el pago
    /// 		12. Fecha_Pago: variable datetime con la fecha de aplicación del pago
    /// 		13. Propietario: nombre del propietario de la cuenta
    /// 		14. Dic_Claves_Ingreso: diccionario con: Tipo ingreso-clave de ingreso
    /// 		15. Dic_Clave_Dependencia: diccionario con: Clave de ingreso-Dependencias
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 02-abr-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private string Agregar_Pasivos_Adeudo(
        DataTable Dt_Pasivos, 
        decimal Adeudo_Corriente, 
        decimal Adeudo_Rezago, 
        decimal Adeudo_Recargos, 
        decimal Adeudo_Honorarios, 
        decimal Descuento, 
        string Cuenta_Predial, 
        string Cuenta_Predial_ID, 
        string Tipo_Predio,
        string Periodo_Corriente,
        string Periodo_Rezago, 
        DateTime Fecha_Pago,
        string Propietario, 
        Dictionary<string, string> Dic_Claves_Ingreso, 
        Dictionary<string, string> Dic_Clave_Dependencia
        )
    {
        string Mensaje = "";
        DataRow Dr_Pasivo;
        // si el adeudo es mayor a cero, agregar pasivo
        if (Adeudo_Corriente > 0)
        {
            Dr_Pasivo = Dt_Pasivos.NewRow();
            Dr_Pasivo["Cuenta_Predial"] = Cuenta_Predial;
            Dr_Pasivo["Cuenta_Predial_Id"] = Cuenta_Predial_ID;
            // verificar que la clave de ingreso se encuentre en el diccionario
            if (!Dic_Claves_Ingreso.ContainsKey("IMPUESTO " + Tipo_Predio))
            {
                Mensaje = "No se encontró la clave de ingreso: IMPUESTO " + Tipo_Predio + " <br />";
            }
            else
            {
                Dr_Pasivo["Clave_Ingreso"] = Dic_Claves_Ingreso["IMPUESTO " + Tipo_Predio];
            }
            Dr_Pasivo["Descripcion"] = "IMPUESTO CORRIENTE [" + Periodo_Corriente + "]";
            Dr_Pasivo["Fecha_Tramite"] = Fecha_Pago;
            Dr_Pasivo["Fecha_Vencimiento"] = Fecha_Pago;
            Dr_Pasivo["Monto"] = Adeudo_Corriente;
            Dr_Pasivo["Estatus"] = "PAGADO";
            // verificar que la dependencia se encuentre en el diccionario
            if (!Dic_Clave_Dependencia.ContainsKey(Dic_Claves_Ingreso["IMPUESTO " + Tipo_Predio]))
            {
                Mensaje = "No se encontró la Dependencia para la clave de ingreso: " + Dic_Claves_Ingreso["IMPUESTO " + Tipo_Predio] + " <br />";
            }
            else
            {
                Dr_Pasivo["Dependencia"] = Dic_Clave_Dependencia[Dic_Claves_Ingreso["IMPUESTO " + Tipo_Predio]];
            }
            Dr_Pasivo["Contribuyente"] = Propietario;
            Dt_Pasivos.Rows.Add(Dr_Pasivo);
        }

        // si el adeudo es mayor a cero, agregar pasivo
        if (Adeudo_Rezago> 0)
        {
            Dr_Pasivo = Dt_Pasivos.NewRow();
            Dr_Pasivo["Cuenta_Predial"] = Cuenta_Predial;
            Dr_Pasivo["Cuenta_Predial_Id"] = Cuenta_Predial_ID;
            // verificar que la clave de ingreso se encuentre en el diccionario
            if (!Dic_Claves_Ingreso.ContainsKey("REZAGO " + Tipo_Predio))
            {
                Mensaje = "No se encontró la clave de ingreso: REZAGO " + Tipo_Predio + " <br />";
            }
            else
            {
                Dr_Pasivo["Clave_Ingreso"] = Dic_Claves_Ingreso["REZAGO " + Tipo_Predio];
            }
            Dr_Pasivo["Descripcion"] = "IMPUESTO REZAGO [" + Periodo_Rezago + "]";
            Dr_Pasivo["Fecha_Tramite"] = Fecha_Pago;
            Dr_Pasivo["Fecha_Vencimiento"] = Fecha_Pago;
            Dr_Pasivo["Monto"] = Adeudo_Rezago;
            Dr_Pasivo["Estatus"] = "PAGADO";
            // verificar que la dependencia se encuentre en el diccionario
            if (!Dic_Clave_Dependencia.ContainsKey(Dic_Claves_Ingreso["REZAGO " + Tipo_Predio]))
            {
                Mensaje = "No se encontró la Dependencia para la clave de ingreso: " + Dic_Claves_Ingreso["REZAGO " + Tipo_Predio] + " <br />";
            }
            else
            {
                Dr_Pasivo["Dependencia"] = Dic_Clave_Dependencia[Dic_Claves_Ingreso["REZAGO " + Tipo_Predio]];
            }
            Dr_Pasivo["Contribuyente"] = Propietario;
            Dt_Pasivos.Rows.Add(Dr_Pasivo);
        }

        // si el adeudo por honorarios es mayor a cero, agregar pasivo
        if (Adeudo_Honorarios > 0)
        {
            Dr_Pasivo = Dt_Pasivos.NewRow();
            Dr_Pasivo["Cuenta_Predial"] = Cuenta_Predial;
            Dr_Pasivo["Cuenta_Predial_Id"] = Cuenta_Predial_ID;
            // verificar que la clave de ingreso se encuentre en el diccionario
            if (!Dic_Claves_Ingreso.ContainsKey("HONORARIOS"))
            {
                Mensaje = "No se encontró la clave de ingreso: HONORARIOS <br />";
            }
            else
            {
                Dr_Pasivo["Clave_Ingreso"] = Dic_Claves_Ingreso["HONORARIOS"];
            }
            Dr_Pasivo["Descripcion"] = "HONORARIOS]";
            Dr_Pasivo["Fecha_Tramite"] = Fecha_Pago;
            Dr_Pasivo["Fecha_Vencimiento"] = Fecha_Pago;
            Dr_Pasivo["Monto"] = Adeudo_Honorarios;
            Dr_Pasivo["Estatus"] = "PAGADO";
            // verificar que la dependencia se encuentre en el diccionario
            if (!Dic_Clave_Dependencia.ContainsKey(Dic_Claves_Ingreso["HONORARIOS"]))
            {
                Mensaje = "No se encontró la Dependencia para la clave de ingreso: " + Dic_Claves_Ingreso["HONORARIOS"] + " <br />";
            }
            else
            {
                Dr_Pasivo["Dependencia"] = Dic_Clave_Dependencia[Dic_Claves_Ingreso["HONORARIOS"]];
            }
            Dr_Pasivo["Contribuyente"] = Propietario;
            Dt_Pasivos.Rows.Add(Dr_Pasivo);
        }

        // si el adeudo por RECARGOS ORDINARIOS es mayor a cero, agregar pasivo
        if (Adeudo_Recargos > 0)
        {
            Dr_Pasivo = Dt_Pasivos.NewRow();
            Dr_Pasivo["Cuenta_Predial"] = Cuenta_Predial;
            Dr_Pasivo["Cuenta_Predial_Id"] = Cuenta_Predial_ID;
            // verificar que la clave de ingreso se encuentre en el diccionario
            if (!Dic_Claves_Ingreso.ContainsKey("RECARGOS ORDINARIOS"))
            {
                Mensaje = "No se encontró la clave de ingreso: RECARGOS ORDINARIOS <br />";
            }
            else
            {
                Dr_Pasivo["Clave_Ingreso"] = Dic_Claves_Ingreso["RECARGOS ORDINARIOS"];
            }
            Dr_Pasivo["Descripcion"] = "RECARGOS ORDINARIOS";
            Dr_Pasivo["Fecha_Tramite"] = Fecha_Pago;
            Dr_Pasivo["Fecha_Vencimiento"] = Fecha_Pago;
            Dr_Pasivo["Monto"] = Adeudo_Recargos;
            Dr_Pasivo["Estatus"] = "PAGADO";
            // verificar que la dependencia se encuentre en el diccionario
            if (!Dic_Clave_Dependencia.ContainsKey(Dic_Claves_Ingreso["RECARGOS ORDINARIOS"]))
            {
                Mensaje = "No se encontró la Dependencia para la clave de ingreso: " + Dic_Claves_Ingreso["RECARGOS ORDINARIOS"] + " <br />";
            }
            else
            {
                Dr_Pasivo["Dependencia"] = Dic_Clave_Dependencia[Dic_Claves_Ingreso["RECARGOS ORDINARIOS"]];
            }
            Dr_Pasivo["Contribuyente"] = Propietario;
            Dt_Pasivos.Rows.Add(Dr_Pasivo);
        }

        return Mensaje;
    }

    #region METODOS_DATOS_BANCOS

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Leer_Archivo_Bajio
    /// DESCRIPCION: Lee los datos en el archivo recibido de acuerdo con el layout del banco
    /// PARAMETROS: 
    ///             1. Sr_Archivo: contenido del archivo subido por el usuario
    /// 		    2. Limite_Pago_Inferior: cantidad máxima que puede faltar del pago contra el adeudo
    /// 		    3. Limite_Pago_Superior: cantidad máxima que puede superar el pago al adeudo
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 22-mar-2012
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private String Leer_Archivo_Bajio(StreamReader Sr_Archivo, decimal Limite_Pago_Inferior, decimal Limite_Pago_Superior)
    {
        string Mensaje = "";
        string Linea_Archivo;
        DataTable Dt_Pagos;
        DataTable Dt_Captura;
        string Cuenta_Predial;
        string Cuenta_Predial_ID;
        DateTime Fecha_Actual = DateTime.Now;
        decimal Cuota_Minima;
        string Mes_Movimientos;
        string Tipo_Predio_ID;
        string Propietario;
        var Consulta_Propietario_Negocio = new Cls_Ope_Pre_Pagos_Instituciones_Externas_Negocio();
        // totales archivo
        DateTime Fecha_Movimientos = DateTime.Now;
        Int32 Total_Operaciones = 0;
        decimal Total_Importe = 0;
        Int32 Contador_Movimientos = 0;
        decimal Suma_Importes = 0;
        Int32 Contador_Lineas = 0;
        // detalles pagos
        DateTime Fecha_Pago;
        decimal Importe;
        string Tipo_Pago;
        string Sucursal;
        string Identificador_Pago;
        DataRow Fila_Pago;
        DataRow Fila_Carga;
        var Dicc_Porciento_Descuento = new Dictionary<string, decimal>();
        Dictionary<string, string> Dicc_Tipos_Predio;
        Dictionary<string, string> Dic_Claves_Ingreso;
        Dictionary<string, string> Dic_Clave_Dependencia;
        // adeudos
        decimal Adeudo_Corriente = 0;
        decimal Adeudo_Rezago = 0;
        decimal Adeudo_Recargos = 0;
        decimal Adeudo_Honorarios = 0;
        decimal Total_Adeudo = 0;
        decimal Descuento;
        string Periodo_Corriente;
        string Periodo_Rezago;
        // tablas para el pago
        DataTable Dt_Adeudo_Detallado_Predial = Crear_Tabla_Adeudos_Predial();
        DataTable Dt_Adeudos_Totales = Crear_Tabla_Adeudos_Totales();
        DataTable Dt_Pasivos_Pago = Crear_Tabla_Pasivos_Pago();
        DataTable Dt_Adeudos;
        string[] Arreglo_Parametros;

        // obtener cuota minima
        Cuota_Minima = Obtener_Cuota_Minima(Fecha_Actual.Year);
        // crear tabla para pagos
        Dt_Pagos = Crear_Tabla_Pagos();
        Dt_Captura = Crear_Tabla_Captura_Pagos();

        Dicc_Tipos_Predio = Consultar_Tipos_Predio();
        Dic_Claves_Ingreso = Obtener_Diccionario_Claves_Ingreso(out Dic_Clave_Dependencia);

        try
        {
            while (Sr_Archivo.Peek() != -1)
            {
                Linea_Archivo = Sr_Archivo.ReadLine();
                Contador_Lineas++;
                // validar longitud de la linea leida
                if (Linea_Archivo.Length < 32)
                {
                    Mensaje += "Se omite la línea " + Contador_Lineas + " debido a que no tiene la longitud mínima para extraer datos <br />";
                    continue;
                }
                Arreglo_Parametros = Linea_Archivo.Split(';');

                // procesar encabezado (primer caracter 0), movimientos (primer caracter 1) y totales (primer caracter 9)
                switch (Linea_Archivo.Substring(0, 4))
                {
                    case "0000": // encabezado (fecha)
                        // validar que tenga el número de parámetros necesarios (encabezado: 4)
                        if (Arreglo_Parametros.Length < 4)
                        {
                            Mensaje += "Se omite la línea " + Contador_Lineas + " debido a que no tiene el mínimo de parámetros para extraer datos <br />";
                            continue;
                        }
                        // recuperar valores de la fecha (formato: ddMMMaaaa)
                        DateTime.TryParseExact(Linea_Archivo[3].ToString(), "ddMMMyyyy", null, System.Globalization.DateTimeStyles.None, out Fecha_Movimientos);
                        // agregar porcentaje de descuento para el mes de los movimientos al diccionario de descuentos
                        Mes_Movimientos = Fecha_Movimientos.ToString("MMMM").ToUpper();
                        if (!Dicc_Porciento_Descuento.ContainsKey(Mes_Movimientos))
                        {
                            Dicc_Porciento_Descuento.Add(Mes_Movimientos, Obtener_Descuentos_Pronto_Pago(Mes_Movimientos, Fecha_Movimientos.Year));
                        }
                        break;
                    case "0001": // movimientos
                        // validar que tenga el número de parámetros necesarios (detalles: 11)
                        if (Arreglo_Parametros.Length < 11)
                        {
                            Mensaje += "Se omite la línea " + Contador_Lineas + " debido a que no tiene el mínimo de parámetros para extraer datos <br />";
                            continue;
                        }
                        String Incluido = "SI";
                        decimal Diferencia = 0;
                        Descuento = 0;
                        // recuperar valores de la fecha (último parámetro: ddMMMaaaa)
                        DateTime.TryParseExact(Arreglo_Parametros[10], "ddMMMyyyy", null, System.Globalization.DateTimeStyles.None, out Fecha_Pago);
                        // agregar porcentaje de descuento para el mes de los movimientos al diccionario de descuentos
                        Mes_Movimientos = Fecha_Pago.ToString("MMMM").ToUpper();
                        if (!Dicc_Porciento_Descuento.ContainsKey(Mes_Movimientos))
                        {
                            Dicc_Porciento_Descuento.Add(Mes_Movimientos, Obtener_Descuentos_Pronto_Pago(Mes_Movimientos, Fecha_Pago.Year));
                        }

                        // caracteres 8 a 20 parte entera del importe, 21 y 22 parte decimal del importe
                        Decimal.TryParse(Arreglo_Parametros[9].Trim(), out Importe);
                        Suma_Importes += Importe;
                        Tipo_Pago = Arreglo_Parametros[7].Trim().ToUpper();
                        Sucursal = Arreglo_Parametros[8].Split('-')[0];
                        Identificador_Pago = Arreglo_Parametros[5].Trim();
                        Cuenta_Predial_ID = Consultar_Cuenta_Predial_ID(Identificador_Pago, out Cuenta_Predial, Cmb_Banco.SelectedValue, out Tipo_Predio_ID);
                        Propietario = Consultar_Propietario(Cuenta_Predial_ID, Consulta_Propietario_Negocio);
                        // si falta la cuenta predial o el identificador del pago, marcar como no incluido
                        if (Cuenta_Predial == "" || Cuenta_Predial_ID == "" || Identificador_Pago == "")
                        {
                            Incluido = "NO";
                            Total_Adeudo = 0;
                            Adeudo_Corriente = 0;
                            Adeudo_Rezago = 0;
                            Adeudo_Honorarios = 0;
                            Adeudo_Recargos = 0;
                        }
                        else
                        {
                            Dt_Adeudos = Consultar_Adeudos_Cuenta_Predial(
                                Cuenta_Predial_ID,
                                Fecha_Pago,
                                out Total_Adeudo,
                                out Adeudo_Corriente,
                                out Adeudo_Rezago,
                                out Adeudo_Honorarios,
                                out Adeudo_Recargos,
                                out Periodo_Corriente,
                                out Periodo_Rezago);
                            // calcular descuento
                            string Mes_Pago = Fecha_Pago.ToString("MMMM").ToUpper();
                            if (Dicc_Porciento_Descuento.ContainsKey(Mes_Pago))
                            {
                                Descuento = Adeudo_Corriente * Dicc_Porciento_Descuento[Mes_Pago] / 100;
                            }
                            // validar que el descuento restado al adeudo corriente no sea menor que la cuota mínima
                            if (Cuota_Minima > 0 && Adeudo_Corriente - Descuento < Cuota_Minima)
                            {
                                Descuento = Adeudo_Corriente - Cuota_Minima;
                            }
                            // validar descuentos negativos
                            if (Descuento < 0)
                            {
                                Descuento = 0;
                            }
                            // agregar datos a las tablas para aplicar el pago
                            Agregar_Adeudos_Predial(Dt_Adeudos, Dt_Adeudo_Detallado_Predial, Cuenta_Predial_ID);
                            Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "CORRIENTE", Adeudo_Corriente, "");
                            Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "REZAGO", Adeudo_Rezago, "");
                            Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "RECARGOS", Adeudo_Recargos, "");
                            Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "HONORARIOS", Adeudo_Honorarios, "");
                            Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "DESCUENTOS_CORRIENTES", Descuento, "");
                            Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "MONTO_TOTAL_PAGADO", Importe, "");
                            // agregar a la tabla pasivos
                            if (!Dicc_Tipos_Predio.ContainsKey(Tipo_Predio_ID))
                            {
                                Mensaje += "No se encontró el tipo de predio para la cuenta " + Cuenta_Predial + "<br />";
                                Incluido = "NO";
                            }
                            else
                            {
                                string Mensaje_Pasivo = Agregar_Pasivos_Adeudo(
                                    Dt_Pasivos_Pago, 
                                    Adeudo_Corriente, 
                                    Adeudo_Rezago, 
                                    Adeudo_Recargos, 
                                    Adeudo_Honorarios, 
                                    Descuento, 
                                    Cuenta_Predial, 
                                    Cuenta_Predial_ID, 
                                    Dicc_Tipos_Predio[Tipo_Predio_ID], 
                                    Periodo_Corriente, 
                                    Periodo_Rezago, 
                                    Fecha_Pago, 
                                    Propietario, 
                                    Dic_Claves_Ingreso, 
                                    Dic_Clave_Dependencia);
                                if (Mensaje_Pasivo.Length > 0)
                                {
                                    Incluido = "NO";
                                }
                                Mensaje += Mensaje_Pasivo;
                            }

                            // Recalcular total adeudo
                            Total_Adeudo = Adeudo_Rezago + Adeudo_Corriente - Descuento + Adeudo_Recargos + Adeudo_Honorarios;

                            // si el importe pagado es diferente al adeudo total, marcar el pago como no incluido
                            if ((Importe > (Total_Adeudo + Limite_Pago_Superior)) || (Importe < (Total_Adeudo - Limite_Pago_Inferior)))
                            {
                                Incluido = "NO";
                            }
                        }
                        Diferencia = Importe - Total_Adeudo;
                        Contador_Movimientos++;
                        // agregar datos a la tabla
                        Fila_Pago = Dt_Pagos.NewRow();
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial] = Cuenta_Predial;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial_Id] = Cuenta_Predial_ID;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial_Original] = Cuenta_Predial;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Diferencia] = Diferencia;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Linea_Captura] = Identificador_Pago;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Fecha_Pago] = Fecha_Pago;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Pagado] = Importe;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Pagado_Original] = Importe;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Total_Adeudo] = Total_Adeudo;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Sucursal] = Sucursal;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Tipo_Pago] = Tipo_Pago;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Guia_Cie] = "";
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Descripcion] = "";
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Incluido] = Incluido;
                        Fila_Pago["CONSECUTIVO"] = Contador_Movimientos;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Rezago] = Adeudo_Rezago;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Corriente] = Adeudo_Corriente;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Recargos] = Adeudo_Recargos;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Honorarios] = Adeudo_Honorarios;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Descuento] = Descuento;
                        Dt_Pagos.Rows.Add(Fila_Pago);
                        break;
                    case "9999": // total de movimientos
                        // validar que tenga el número de parámetros necesarios (final: 3)
                        if (Arreglo_Parametros.Length < 3)
                        {
                            Mensaje += "Se omite la línea " + Contador_Lineas + " debido a que no tiene el mínimo de parámetros para extraer datos <br />";
                            continue;
                        }
                        // carcteres 3 a 11: total de movimientos en el archivo
                        Int32.TryParse(Arreglo_Parametros[1].Trim(), out Total_Operaciones);
                        // caracteres 12 a 26 parte entera y 27 y 28 parte decimal del importe total en el archivo
                        Decimal.TryParse(Arreglo_Parametros[2].Trim(), out Total_Importe);
                        break;
                }
            }
            // si no hay pagos en la tabla, mostrar mensaje de error en el formato del archivo
            if (Dt_Pagos.Rows.Count <= 0)
            {
                return "El archivo no tiene el formato correcto";
            }
            // validar que no haya inconsistencias entre los totales en el archivo y los totales leidos
            if (Suma_Importes != Total_Importe)
            {
                Mensaje += "El total en archivo: " + Total_Importe.ToString("#,##0.00")
                    + " es diferente al total calculado al leer el archivo: "
                    + Suma_Importes.ToString("#,##0.00") + ".<br />";
            }
            if (Contador_Movimientos != Total_Operaciones)
            {
                Mensaje += "El total de movimientos en el archivo: " + Total_Operaciones.ToString("#,##0")
                    + " es diferente al número de movimientos leídos: " + Contador_Movimientos.ToString("#,##0") + ".<br />";
            }
            // mostrar total de movimientos
            Txt_Cantidad_Movimientos.Text = Contador_Movimientos.ToString("#,##0");

            // datos generales de captura
            Fila_Carga = Dt_Captura.NewRow();
            Fila_Carga[Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Institucion_Id] = Cmb_Banco.SelectedValue;
            Fila_Carga[Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Fecha_Captura] = DateTime.Now;
            Fila_Carga[Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Nombre_Archivo] = Txt_Nombre_Archivo.Text;
            Fila_Carga[Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Fecha_Corte] = Fecha_Movimientos;
            Dt_Captura.Rows.Add(Fila_Carga);

            Session["Dt_Pagos"] = Dt_Pagos;
            Session["Dt_Captura"] = Dt_Captura;
            Session["Dt_Adeudos_Totales"] = Dt_Adeudos_Totales;
            Session["Dt_Adeudo_Detallado_Predial"] = Dt_Adeudo_Detallado_Predial;
            Session["Dt_Pasivos_Pago"] = Dt_Pasivos_Pago;
            // cargar grids
            Grid_Pagos_Incluidos.Columns[6].Visible = true;
            Grid_Pagos_Excluidos.Columns[7].Visible = true;
            Cargar_Grids_Pagos(true);
        }
        catch (Exception ex)
        {
            throw new Exception("Leer_Archivo_Bajio " + ex.Message.ToString(), ex);
        }
        return Mensaje;
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Leer_Archivo_Banamex
    /// DESCRIPCION: Lee los datos en el archivo recibido de acuerdo con el layout del banco
    /// PARAMETROS: 
    ///             1. Sr_Archivo: contenido del archivo subido por el usuario
    /// 		    2. Limite_Pago_Inferior: cantidad máxima que puede faltar del pago contra el adeudo
    /// 		    3. Limite_Pago_Superior: cantidad máxima que puede superar el pago al adeudo
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 16-ene-2012
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private String Leer_Archivo_Banamex(StreamReader Sr_Archivo, decimal Limite_Pago_Inferior, decimal Limite_Pago_Superior)
    {
        string Mensaje = "";
        string Linea_Archivo;
        DataTable Dt_Pagos;
        DataTable Dt_Captura;
        string Cuenta_Predial;
        string Cuenta_Predial_ID;
        DateTime Fecha_Actual = DateTime.Now;
        decimal Cuota_Minima;
        string Mes_Movimientos;
        string Tipo_Predio_ID;
        string Propietario;
        var Consulta_Propietario_Negocio = new Cls_Ope_Pre_Pagos_Instituciones_Externas_Negocio();
        // totales archivo
        DateTime Fecha_Movimientos = DateTime.Now;
        Int32 Total_Operaciones = 0;
        decimal Total_Importe = 0;
        Int32 Contador_Movimientos = 0;
        decimal Suma_Importes = 0;
        Int32 Contador_Lineas = 0;
        // detalles pagos
        DateTime Fecha_Pago;
        decimal Importe;
        string Tipo_Pago;
        Int32 Sucursal;
        Int32 Cajero;
        Int32 Autorizacion;
        string Identificador_Pago;
        DataRow Fila_Pago;
        DataRow Fila_Carga;
        var Dicc_Porciento_Descuento = new Dictionary<string, decimal>();
        Dictionary<string, string> Dicc_Tipos_Predio;
        Dictionary<string, string> Dic_Claves_Ingreso;
        Dictionary<string, string> Dic_Clave_Dependencia;
        // adeudos
        decimal Adeudo_Corriente = 0;
        decimal Adeudo_Rezago = 0;
        decimal Adeudo_Recargos = 0;
        decimal Adeudo_Honorarios = 0;
        decimal Total_Adeudo = 0;
        string Periodo_Corriente;
        string Periodo_Rezago;
        // tablas para el pago
        DataTable Dt_Adeudo_Detallado_Predial = Crear_Tabla_Adeudos_Predial();
        DataTable Dt_Adeudos_Totales = Crear_Tabla_Adeudos_Totales();
        DataTable Dt_Pasivos_Pago = Crear_Tabla_Pasivos_Pago();
        DataTable Dt_Adeudos;

        // obtener cuota minima
        Cuota_Minima = Obtener_Cuota_Minima(Fecha_Actual.Year);
        // crear tabla para pagos
        Dt_Pagos = Crear_Tabla_Pagos();
        Dt_Captura = Crear_Tabla_Captura_Pagos();

        Dicc_Tipos_Predio = Consultar_Tipos_Predio();
        Dic_Claves_Ingreso = Obtener_Diccionario_Claves_Ingreso(out Dic_Clave_Dependencia);

        try
        {
            while (Sr_Archivo.Peek() != -1)
            {
                Linea_Archivo = Sr_Archivo.ReadLine();
                Contador_Lineas++;
                // validar longitud de la linea leida
                if (Linea_Archivo.Length <= 77)
                {
                    Mensaje += "Se omite la línea " + Contador_Lineas + " debido a que no tiene la longitud mínima para extraer datos <br />";
                    continue;
                }

                // procesar encabezado (primer caracter 0), movimientos (primer caracter 1) y totales (primer caracter 9)
                switch (Linea_Archivo.Substring(0, 1))
                {
                    case "0": // encabezado (fecha)
                        String Numero_Establecimiento;
                        // recuperar valores de la fecha (caracteres 3 a 8 con formato: ddmmaa)
                        Fecha_Movimientos = Convertir_A_Fecha(
                            Linea_Archivo.Substring(2, 2)
                            , Linea_Archivo.Substring(4, 2)
                            , Linea_Archivo.Substring(6, 2));
                        // caracteres 8 a 18
                        Numero_Establecimiento = Linea_Archivo.Substring(8, 10).Trim();
                        // agregar porcentaje de descuento para el mes de los movimientos al diccionario de descuentos
                        Mes_Movimientos = Fecha_Movimientos.ToString("MMMM").ToUpper();
                        if (!Dicc_Porciento_Descuento.ContainsKey(Mes_Movimientos))
                        {
                            Dicc_Porciento_Descuento.Add(Mes_Movimientos, Obtener_Descuentos_Pronto_Pago(Mes_Movimientos, Fecha_Movimientos.Year));
                        }
                        break;
                    case "1": // movimientos
                        String Incluido = "SI";
                        decimal Diferencia = 0;
                        decimal Descuento = 0;
                        // recuperar valores de la fecha (caracteres 2 a 7 con formato: ddmmaa)
                        Fecha_Pago = Convertir_A_Fecha(
                            Linea_Archivo.Substring(5, 2)
                            , Linea_Archivo.Substring(3, 2)
                            , Linea_Archivo.Substring(1, 2));

                        // agregar porcentaje de descuento para el mes de los movimientos al diccionario de descuentos
                        Mes_Movimientos = Fecha_Pago.ToString("MMMM").ToUpper();
                        if (!Dicc_Porciento_Descuento.ContainsKey(Mes_Movimientos))
                        {
                            Dicc_Porciento_Descuento.Add(Mes_Movimientos, Obtener_Descuentos_Pronto_Pago(Mes_Movimientos, Fecha_Pago.Year));
                        }

                        // caracteres 8 a 20 parte entera del importe, 21 y 22 parte decimal del importe
                        Decimal.TryParse(Linea_Archivo.Substring(7, 15), out Importe);
                        Importe /= 100;
                        Suma_Importes += Importe;
                        Tipo_Pago = Linea_Archivo.Substring(28, 6);
                        Int32.TryParse(Linea_Archivo.Substring(34, 4), out Sucursal);
                        Int32.TryParse(Linea_Archivo.Substring(38, 2), out Cajero);
                        Int32.TryParse(Linea_Archivo.Substring(40, 7), out Autorizacion);
                        Identificador_Pago = Linea_Archivo.Substring(47, 30).Trim();
                        Cuenta_Predial_ID = Consultar_Cuenta_Predial_ID("B: " + Identificador_Pago, out Cuenta_Predial, Cmb_Banco.SelectedValue, out Tipo_Predio_ID);
                        Propietario = Consultar_Propietario(Cuenta_Predial_ID, Consulta_Propietario_Negocio);
                        // si falta la cuenta predial o el identificador del pago, marcar como no incluido
                        if (Cuenta_Predial == "" || Cuenta_Predial_ID == "" || Identificador_Pago == "")
                        {
                            Incluido = "NO";
                            Total_Adeudo = 0;
                            Adeudo_Corriente = 0;
                            Adeudo_Rezago = 0;
                            Adeudo_Honorarios = 0;
                            Adeudo_Recargos = 0;
                        }
                        else
                        {
                            Dt_Adeudos = Consultar_Adeudos_Cuenta_Predial(
                                Cuenta_Predial_ID,
                                Fecha_Pago,
                                out Total_Adeudo,
                                out Adeudo_Corriente,
                                out Adeudo_Rezago,
                                out Adeudo_Honorarios,
                                out Adeudo_Recargos,
                                out Periodo_Corriente,
                                out Periodo_Rezago);
                            // calcular descuento
                            string Mes_Pago = Fecha_Pago.ToString("MMMM").ToUpper();
                            if (Dicc_Porciento_Descuento.ContainsKey(Mes_Pago))
                            {
                                Descuento = Adeudo_Corriente * Dicc_Porciento_Descuento[Mes_Pago] / 100;
                            }
                            
                            // validar que el descuento restado al adeudo corriente no sea menor que la cuota mínima
                            if (Cuota_Minima > 0 && Adeudo_Corriente - Descuento < Cuota_Minima)
                            {
                                Descuento = Adeudo_Corriente - Cuota_Minima;
                            }
                            // validar descuentos negativos
                            if (Descuento < 0)
                            {
                                Descuento = 0;
                            }
                            // agregar datos a las tablas para aplicar el pago
                            Agregar_Adeudos_Predial(Dt_Adeudos, Dt_Adeudo_Detallado_Predial, Cuenta_Predial_ID);
                            Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "CORRIENTE", Adeudo_Corriente, "");
                            Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "REZAGO", Adeudo_Rezago, "");
                            Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "RECARGOS", Adeudo_Recargos, "");
                            Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "HONORARIOS", Adeudo_Honorarios, "");
                            Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "DESCUENTOS_CORRIENTES", Descuento, "");
                            Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "MONTO_TOTAL_PAGADO", Importe, "");
                            // agregar a la tabla pasivos
                            if (!Dicc_Tipos_Predio.ContainsKey(Tipo_Predio_ID))
                            {
                                Mensaje += "No se encontró el tipo de predio para la cuenta " + Cuenta_Predial + "<br />";
                                Incluido = "NO";
                            }
                            else
                            {
                                string Mensaje_Pasivo = Agregar_Pasivos_Adeudo(
                                    Dt_Pasivos_Pago,
                                    Adeudo_Corriente,
                                    Adeudo_Rezago,
                                    Adeudo_Recargos,
                                    Adeudo_Honorarios,
                                    Descuento,
                                    Cuenta_Predial,
                                    Cuenta_Predial_ID,
                                    Dicc_Tipos_Predio[Tipo_Predio_ID],
                                    Periodo_Corriente,
                                    Periodo_Rezago,
                                    Fecha_Pago,
                                    Propietario,
                                    Dic_Claves_Ingreso,
                                    Dic_Clave_Dependencia);
                                if (Mensaje_Pasivo.Length > 0)
                                {
                                    Incluido = "NO";
                                }
                                Mensaje += Mensaje_Pasivo;
                            }

                            // Recalcular total adeudo - Descuento
                            Total_Adeudo = Adeudo_Rezago + Adeudo_Corriente - Descuento + Adeudo_Recargos + Adeudo_Honorarios;

                            // si el importe pagado es diferente al adeudo total, marcar el pago como no incluido
                            if ((Importe > (Total_Adeudo + Limite_Pago_Superior)) || (Importe < (Total_Adeudo - Limite_Pago_Inferior)))
                            {
                                Incluido = "NO";
                            }
                        }
                        Diferencia = Importe - Total_Adeudo;
                        Contador_Movimientos++;
                        // agregar datos a la tabla
                        Fila_Pago = Dt_Pagos.NewRow();
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial] = Cuenta_Predial;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial_Id] = Cuenta_Predial_ID;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial_Original] = Cuenta_Predial;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Diferencia] = Diferencia;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Linea_Captura] = Identificador_Pago;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Fecha_Pago] = Fecha_Pago;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Pagado] = Importe;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Pagado_Original] = Importe;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Total_Adeudo] = Total_Adeudo;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Sucursal] = Sucursal.ToString();
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Tipo_Pago] = Tipo_Pago;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Guia_Cie] = "";
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cajero] = Cajero;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Autorizacion] = Autorizacion;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Descripcion] = "";
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Incluido] = Incluido;
                        Fila_Pago["CONSECUTIVO"] = Contador_Movimientos;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Rezago] = Adeudo_Rezago;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Corriente] = Adeudo_Corriente;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Recargos] = Adeudo_Recargos;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Honorarios] = Adeudo_Honorarios;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Descuento] = Descuento;
                        Dt_Pagos.Rows.Add(Fila_Pago);
                        break;
                    case "9": // total de movimientos
                        // carcteres 3 a 11: total de movimientos en el archivo
                        Int32.TryParse(Linea_Archivo.Substring(2, 9), out Total_Operaciones);
                        // caracteres 12 a 26 parte entera y 27 y 28 parte decimal del importe total en el archivo
                        Decimal.TryParse(Linea_Archivo.Substring(11, 17), out Total_Importe);
                        Total_Importe /= 100;
                        break;
                }
            }
            // si no hay pagos en la tabla, mostrar mensaje de error en el formato del archivo
            if (Dt_Pagos.Rows.Count <= 0)
            {
                return "El archivo no tiene el formato correcto";
            }
            // validar que no haya inconsistencias entre los totales en el archivo y los totales leidos
            if (Suma_Importes != Total_Importe)
            {
                Mensaje += "El total en archivo: " + Total_Importe.ToString("#,##0.00")
                    + " es diferente al total calculado al leer el archivo: "
                    + Suma_Importes.ToString("#,##0.00") + ".<br />";
            }
            if (Contador_Movimientos != Total_Operaciones)
            {
                Mensaje += "El total de movimientos en el archivo: " + Total_Operaciones.ToString("#,##0")
                    + " es diferente al número de movimientos leídos: " + Contador_Movimientos.ToString("#,##0") + ".<br />";
            }
            // mostrar total de movimientos
            Txt_Cantidad_Movimientos.Text = Contador_Movimientos.ToString("#,##0");

            // datos generales de captura
            Fila_Carga = Dt_Captura.NewRow();
            Fila_Carga[Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Institucion_Id] = Cmb_Banco.SelectedValue;
            Fila_Carga[Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Fecha_Captura] = DateTime.Now;
            Fila_Carga[Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Nombre_Archivo] = Txt_Nombre_Archivo.Text;
            Fila_Carga[Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Fecha_Corte] = Fecha_Movimientos;
            Dt_Captura.Rows.Add(Fila_Carga);

            Session["Dt_Pagos"] = Dt_Pagos;
            Session["Dt_Captura"] = Dt_Captura;
            Session["Dt_Adeudos_Totales"] = Dt_Adeudos_Totales;
            Session["Dt_Adeudo_Detallado_Predial"] = Dt_Adeudo_Detallado_Predial;
            Session["Dt_Pasivos_Pago"] = Dt_Pasivos_Pago;
            // cargar grids
            Grid_Pagos_Incluidos.Columns[6].Visible = true;
            Grid_Pagos_Excluidos.Columns[7].Visible = true;
            Cargar_Grids_Pagos(true);
        }
        catch (Exception ex)
        {
            throw new Exception("Leer_Archivo_Banamex " + ex.Message.ToString(), ex);
        }
        return Mensaje;
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Leer_Archivo_Bancomer
    /// DESCRIPCION: Lee los datos en el archivo recibido de acuerdo con el layout del banco
    /// PARAMETROS: 
    ///             1. Sr_Archivo: contenido del archivo subido por el usuario
    /// 		    2. Limite_Pago_Inferior: cantidad máxima que puede faltar del pago contra el adeudo
    /// 		    3. Limite_Pago_Superior: cantidad máxima que puede superar el pago al adeudo
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 16-ene-2012
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private String Leer_Archivo_Bancomer(StreamReader Sr_Archivo, decimal Limite_Pago_Inferior, decimal Limite_Pago_Superior)
    {
        String Mensaje = "";
        String Linea_Archivo;
        DataTable Dt_Pagos;
        DataTable Dt_Captura;
        String Cuenta_Predial;
        String Cuenta_Predial_ID;
        var Dicc_Porciento_Descuento = new Dictionary<string, decimal>();
        Dictionary<string, string> Dicc_Tipos_Predio;
        Dictionary<string, string> Dic_Claves_Ingreso;
        Dictionary<string, string> Dic_Clave_Dependencia;
        decimal Cuota_Minima;
        decimal Descuento;
        string Mes_Movimientos;
        string Tipo_Predio_ID;
        string Propietario;
        var Consulta_Propietario_Negocio = new Cls_Ope_Pre_Pagos_Instituciones_Externas_Negocio();
        // totales archivo
        Int32 Total_Operaciones = 0;
        Decimal Total_Importe = 0;
        Int32 Contador_Movimientos = 0;
        Decimal Suma_Importes = 0;
        Int32 Contador_Lineas = 0;
        // detalles pagos
        DateTime Fecha_Pago = DateTime.Now;
        Decimal Importe;
        string Sucursal;
        String Identificador_Pago;
        DataRow Fila_Pago;
        DataRow Fila_Carga;
        String Guia_Cie;
        String Cie;
        string Tipo_Pago;
        // tablas para aplicación de pagos
        DataTable Dt_Adeudo_Detallado_Predial = Crear_Tabla_Adeudos_Predial();
        DataTable Dt_Adeudos_Totales = Crear_Tabla_Adeudos_Totales();
        DataTable Dt_Pasivos_Pago = Crear_Tabla_Pasivos_Pago();
        DataTable Dt_Adeudos;
        // adeudos
        decimal Adeudo_Corriente = 0;
        decimal Adeudo_Rezago = 0;
        decimal Adeudo_Recargos = 0;
        decimal Adeudo_Honorarios = 0;
        decimal Total_Adeudo = 0;
        string Periodo_Corriente;
        string Periodo_Rezago;

        // crear tabla para pagos
        Dt_Pagos = Crear_Tabla_Pagos();
        Dt_Captura = Crear_Tabla_Captura_Pagos();

        // obtener cuota minima
        Cuota_Minima = Obtener_Cuota_Minima(Fecha_Pago.Year);

        Dicc_Tipos_Predio = Consultar_Tipos_Predio();
        Dic_Claves_Ingreso = Obtener_Diccionario_Claves_Ingreso(out Dic_Clave_Dependencia);

        try
        {
            while (Sr_Archivo.Peek() != -1)
            {
                Linea_Archivo = Sr_Archivo.ReadLine();
                Contador_Lineas++;
                // procesar movimientos (primer caracter 0) y totales (primer caracter T)
                switch (Linea_Archivo.Substring(0, 1))
                {
                    case "0": // movimientos
                        String Incluido = "SI";
                        decimal Diferencia = 0;
                        Descuento = 0;
                        // validar longitud de la linea leida
                        if (Linea_Archivo.Length < 141)
                        {
                            Mensaje += "Se omite la línea " + Contador_Lineas + " debido a que no tiene la longitud mínima para extraer datos <br />";
                            continue;
                        }

                        // linea de captura caracteres 1-7(6)
                        Cie = Linea_Archivo.Substring(1, 6).Trim();
                        // linea de captura caracteres 8-27(50)
                        Identificador_Pago = Linea_Archivo.Substring(7, 50).Trim();
                        // caracteres 90-105(16): importe
                        Decimal.TryParse(Linea_Archivo.Substring(89, 16), out Importe);
                        // caracteres 106-114(9): guia cie
                        Guia_Cie = Linea_Archivo.Substring(105, 9);
                        // recuperar valores de la fecha (caracteres 2 a 7 con formato: aaaa-mm-dd)
                        DateTime.TryParse(Linea_Archivo.Substring(114, 10), out Fecha_Pago);
                        // sucursal
                        Sucursal = Linea_Archivo.Substring(134, 4);
                        Tipo_Pago = Linea_Archivo.Substring(138, 3);

                        Cuenta_Predial_ID = Consultar_Cuenta_Predial_ID("Cie " + Cie + " Ref " + Identificador_Pago, out Cuenta_Predial, Cmb_Banco.SelectedValue, out Tipo_Predio_ID);
                        Propietario = Consultar_Propietario(Cuenta_Predial_ID, Consulta_Propietario_Negocio);

                        // agregar porcentaje de descuento para el mes de los movimientos al diccionario de descuentos
                        Mes_Movimientos = Fecha_Pago.ToString("MMMM").ToUpper();
                        if (!Dicc_Porciento_Descuento.ContainsKey(Mes_Movimientos))
                        {
                            Dicc_Porciento_Descuento.Add(Mes_Movimientos, Obtener_Descuentos_Pronto_Pago(Mes_Movimientos, Fecha_Pago.Year));
                        }

                        // si falta la cuenta predial o el identificador del pago, marcar como no incluido
                        if (Cuenta_Predial == "" || Cuenta_Predial_ID == "" || Identificador_Pago == "")
                        {
                            Incluido = "NO";
                            Total_Adeudo = 0;
                            Adeudo_Corriente = 0;
                            Adeudo_Rezago = 0;
                            Adeudo_Honorarios = 0;
                            Adeudo_Recargos = 0;
                        }
                        else
                        {
                            Dt_Adeudos = Consultar_Adeudos_Cuenta_Predial(
                                Cuenta_Predial_ID,
                                Fecha_Pago,
                                out Total_Adeudo,
                                out Adeudo_Corriente,
                                out Adeudo_Rezago,
                                out Adeudo_Honorarios,
                                out Adeudo_Recargos,
                                out Periodo_Corriente,
                                out Periodo_Rezago);
                            
                            // calcular descuento
                            string Mes_Pago = Fecha_Pago.ToString("MMMM").ToUpper();
                            if (Dicc_Porciento_Descuento.ContainsKey(Mes_Pago))
                            {
                                Descuento = Adeudo_Corriente * Dicc_Porciento_Descuento[Mes_Pago] / 100;
                            }

                            // validar que el descuento restado al adeudo corriente no sea menor que la cuota mínima
                            if (Cuota_Minima > 0 && Adeudo_Corriente - Descuento < Cuota_Minima)
                            {
                                Descuento = Adeudo_Corriente - Cuota_Minima;
                            }
                            // validar descuentos negativos
                            if (Descuento < 0)
                            {
                                Descuento = 0;
                            }
                            // agregar datos a las tablas para aplicar el pago
                            Agregar_Adeudos_Predial(Dt_Adeudos, Dt_Adeudo_Detallado_Predial, Cuenta_Predial_ID);
                            Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "CORRIENTE", Adeudo_Corriente, "");
                            Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "REZAGO", Adeudo_Rezago, "");
                            Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "RECARGOS", Adeudo_Recargos, "");
                            Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "HONORARIOS", Adeudo_Honorarios, "");
                            Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "DESCUENTOS_CORRIENTES", Descuento, "");
                            Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "MONTO_TOTAL_PAGADO", Importe, "");
                            // agregar a la tabla pasivos
                            if (!Dicc_Tipos_Predio.ContainsKey(Tipo_Predio_ID))
                            {
                                Mensaje += "No se encontró el tipo de predio para la cuenta " + Cuenta_Predial + "<br />";
                                Incluido = "NO";
                            }
                            else
                            {
                                string Mensaje_Pasivo = Agregar_Pasivos_Adeudo(
                                    Dt_Pasivos_Pago,
                                    Adeudo_Corriente,
                                    Adeudo_Rezago,
                                    Adeudo_Recargos,
                                    Adeudo_Honorarios,
                                    Descuento,
                                    Cuenta_Predial,
                                    Cuenta_Predial_ID,
                                    Dicc_Tipos_Predio[Tipo_Predio_ID],
                                    Periodo_Corriente,
                                    Periodo_Rezago,
                                    Fecha_Pago,
                                    Propietario,
                                    Dic_Claves_Ingreso,
                                    Dic_Clave_Dependencia);
                                if (Mensaje_Pasivo.Length > 0)
                                {
                                    Incluido = "NO";
                                }
                                Mensaje += Mensaje_Pasivo;
                            }

                            // Recalcular total adeudo
                            Total_Adeudo = Adeudo_Rezago + Adeudo_Corriente - Descuento + Adeudo_Recargos + Adeudo_Honorarios;

                            // si el importe pagado es diferente al adeudo total, marcar el pago como no incluido
                            if ((Importe > (Total_Adeudo + Limite_Pago_Superior)) || (Importe < (Total_Adeudo - Limite_Pago_Inferior)))
                            {
                                Incluido = "NO";
                            }
                        }
                        Diferencia = Importe - Total_Adeudo;
                        Suma_Importes += Importe;
                        Contador_Movimientos++;
                        // agregar datos a la tabla
                        Fila_Pago = Dt_Pagos.NewRow();
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial] = Cuenta_Predial;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial_Id] = Cuenta_Predial_ID;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial_Original] = Cuenta_Predial;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Diferencia] = Diferencia;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Linea_Captura] = Identificador_Pago;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Fecha_Pago] = Fecha_Pago;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Pagado] = Importe;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Pagado_Original] = Importe;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Total_Adeudo] = Total_Adeudo;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Sucursal] = Sucursal;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Guia_Cie] = Guia_Cie;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Incluido] = Incluido;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Tipo_Pago] = Tipo_Pago;
                        Fila_Pago["CONSECUTIVO"] = Contador_Movimientos;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Rezago] = Adeudo_Rezago;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Corriente] = Adeudo_Corriente;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Recargos] = Adeudo_Recargos;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Honorarios] = Adeudo_Honorarios;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Descuento] = Descuento;
                        Dt_Pagos.Rows.Add(Fila_Pago);
                        break;
                    case "T": // total de movimientos
                        if (Linea_Archivo.Contains("Total Transacciones"))
                        {
                            // carcteres 3 a 11: total de movimientos en el archivo
                            Int32.TryParse(Linea_Archivo.Substring(Linea_Archivo.Length - 16), out Total_Operaciones);
                        }
                        else if (Linea_Archivo.Contains("Total Abonado"))
                        {
                            // caracteres 12 a 26 parte entera y 27 y 28 parte decimal del importe total en el archivo
                            Decimal.TryParse(Linea_Archivo.Substring(16), out Total_Importe);
                        }
                        break;
                }
            }

            // si no hay pagos en la tabla, mostrar mensaje de error en el formato del archivo
            if (Dt_Pagos.Rows.Count <= 0)
            {
                return "El archivo no tiene el formato correcto";
            }
            // validar que no haya inconsistencias entre los totales en el archivo y los totales leidos
            if (Suma_Importes != Total_Importe)
            {
                Mensaje += "El total en archivo: " + Total_Importe.ToString("#,##0.00")
                    + " es diferente al total calculado al leer el archivo: "
                    + Suma_Importes.ToString("#,##0.00") + ".<br />";
            }
            if (Contador_Movimientos != Total_Operaciones)
            {
                Mensaje += "El total de movimientos en el archivo: " + Total_Operaciones.ToString("#,##0")
                    + " es diferente al número de movimientos leídos: " + Contador_Movimientos.ToString("#,##0") + ".<br />";
            }
            // mostrar total de movimientos
            Txt_Cantidad_Movimientos.Text = Contador_Movimientos.ToString("#,##0");

            // datos generales de captura
            Fila_Carga = Dt_Captura.NewRow();
            Fila_Carga[Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Institucion_Id] = Cmb_Banco.SelectedValue;
            Fila_Carga[Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Fecha_Captura] = DateTime.Now;
            Fila_Carga[Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Nombre_Archivo] = Txt_Nombre_Archivo.Text;
            Fila_Carga[Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Fecha_Corte] = Fecha_Pago;
            Dt_Captura.Rows.Add(Fila_Carga);

            Session["Dt_Pagos"] = Dt_Pagos;
            Session["Dt_Captura"] = Dt_Captura;
            Session["Dt_Adeudos_Totales"] = Dt_Adeudos_Totales;
            Session["Dt_Adeudo_Detallado_Predial"] = Dt_Adeudo_Detallado_Predial;
            Session["Dt_Pasivos_Pago"] = Dt_Pasivos_Pago;
            // cargar grids
            Grid_Pagos_Incluidos.Columns[7].Visible = true;
            Grid_Pagos_Incluidos.Columns[9].Visible = true;
            Grid_Pagos_Excluidos.Columns[8].Visible = true;
            Grid_Pagos_Excluidos.Columns[10].Visible = true;
            Cargar_Grids_Pagos(true);
        }
        catch (Exception ex)
        {
            throw new Exception("Leer_Archivo_Bancomer " + ex.Message.ToString(), ex);
        }
        return Mensaje;
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Leer_Archivo_Banorte
    /// DESCRIPCION: Lee los datos en el archivo recibido de acuerdo con el layout del banco
    /// PARAMETROS: 
    ///             1. Sr_Archivo: contenido del archivo subido por el usuario
    /// 		    2. Limite_Pago_Inferior: cantidad máxima que puede faltar del pago contra el adeudo
    /// 		    3. Limite_Pago_Superior: cantidad máxima que puede superar el pago al adeudo
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 16-ene-2012
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private String Leer_Archivo_Banorte(StreamReader Sr_Archivo, decimal Limite_Pago_Inferior, decimal Limite_Pago_Superior)
    {
        String Mensaje = "";
        String Linea_Archivo;
        DataTable Dt_Pagos;
        DataTable Dt_Captura;
        var Dicc_Porciento_Descuento = new Dictionary<string, decimal>();
        Dictionary<string, string> Dicc_Tipos_Predio;
        Dictionary<string, string> Dic_Claves_Ingreso;
        Dictionary<string, string> Dic_Clave_Dependencia;
        decimal Cuota_Minima;
        decimal Descuento;
        string Mes_Movimientos;
        string Tipo_Predio_ID;
        string Propietario;
        var Consulta_Propietario_Negocio = new Cls_Ope_Pre_Pagos_Instituciones_Externas_Negocio();
        // totales archivo
        Int32 Contador_Movimientos = 0;
        Decimal Suma_Importes = 0;
        Int32 Contador_Lineas = 0;
        DateTime Fecha_Corte = new DateTime();
        // detalles pagos
        DateTime Fecha_Pago = DateTime.Now;
        Decimal Importe;
        String Sucursal;
        String Identificador_Pago;
        DataRow Fila_Pago;
        String Descripcion;
        String Cuenta_Predial;
        String Cuenta_Predial_ID;
        string Tipo_Pago;
        // tablas para aplicación de pagos
        DataTable Dt_Adeudo_Detallado_Predial = Crear_Tabla_Adeudos_Predial();
        DataTable Dt_Adeudos_Totales = Crear_Tabla_Adeudos_Totales();
        DataTable Dt_Pasivos_Pago = Crear_Tabla_Pasivos_Pago();
        DataTable Dt_Adeudos;
        // adeudos
        decimal Adeudo_Corriente = 0;
        decimal Adeudo_Rezago = 0;
        decimal Adeudo_Recargos = 0;
        decimal Adeudo_Honorarios = 0;
        decimal Total_Adeudo = 0;
        string Periodo_Corriente;
        string Periodo_Rezago;
        Dictionary<string, string> Dic_Tipos_Pago = new Dictionary<string, string>()
        {
            {"01","Efectivo"},
            {"02","Cheque Banorte"},
            {"03","Cheque Otros Bancos"},
            {"04","Cargo Cta de Cheques"},
            {"05","Multipago"}, 
            {"06","Tarjeta de Crédito"},
        };

        // crear tabla para pagos
        Dt_Pagos = Crear_Tabla_Pagos();
        Dt_Captura = Crear_Tabla_Captura_Pagos();

        // obtener cuota minima
        Cuota_Minima = Obtener_Cuota_Minima(Fecha_Pago.Year);

        Dicc_Tipos_Predio = Consultar_Tipos_Predio();
        Dic_Claves_Ingreso = Obtener_Diccionario_Claves_Ingreso(out Dic_Clave_Dependencia);

        try
        {
            while (Sr_Archivo.Peek() != -1)
            {
                String Incluido = "SI";
                decimal Diferencia = 0;
                Descuento = 0;
                Linea_Archivo = Sr_Archivo.ReadLine();
                Contador_Lineas++;
                // solo contiene lineas con datos
                if (Linea_Archivo.Length < 128)
                {
                    Mensaje += "Se omite la línea " + Contador_Lineas + " debido a que no tiene la longitud mínima para extraer datos <br />";
                    continue;
                }

                // caracteres 1-30(30): cuenta predial
                Cuenta_Predial = Linea_Archivo.Substring(0, 30).Trim();
                Identificador_Pago = Linea_Archivo.Substring(30, 5) + " " + Cuenta_Predial;
                // caracteres 35-52(17): importe
                Decimal.TryParse(Linea_Archivo.Substring(35, 17), out Importe);
                Importe /= 100;
                // caracteres 53-92(40): concepto
                Descripcion = Linea_Archivo.Substring(53, 40).Trim();
                // recuperar valores de la fecha (caracteres 117 a 126 con formato: aaaa-mm-dd)
                DateTime.TryParseExact(Linea_Archivo.Substring(116, 10), "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out Fecha_Pago);
                // sucursal
                Sucursal = Linea_Archivo.Substring(112, 4);

                Tipo_Pago = Linea_Archivo.Substring(126, 2);
                if (Dic_Tipos_Pago.ContainsKey(Tipo_Pago))
                {
                    Tipo_Pago = Dic_Tipos_Pago[Tipo_Pago];
                }

                // agregar porcentaje de descuento para el mes de los movimientos al diccionario de descuentos
                Mes_Movimientos = Fecha_Pago.ToString("MMMM").ToUpper();
                if (!Dicc_Porciento_Descuento.ContainsKey(Mes_Movimientos))
                {
                    Dicc_Porciento_Descuento.Add(Mes_Movimientos, Obtener_Descuentos_Pronto_Pago(Mes_Movimientos, Fecha_Pago.Year));
                }

                // solo asignar la fecha de corte la primera vez
                if (Fecha_Corte == DateTime.MinValue)
                {
                    DateTime.TryParseExact(Linea_Archivo.Substring(102, 10), "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out Fecha_Corte);
                }
                Cuenta_Predial_ID = Consultar_Cuenta_Predial_ID(Cuenta_Predial, out Tipo_Predio_ID);
                Propietario = Consultar_Propietario(Cuenta_Predial_ID, Consulta_Propietario_Negocio);

                // si falta la cuenta predial o el identificador del pago, marcar como no incluido
                if (Cuenta_Predial == "" || Cuenta_Predial_ID == "" || Identificador_Pago == "")
                {
                    Incluido = "NO";
                    Total_Adeudo = 0;
                    Adeudo_Corriente = 0;
                    Adeudo_Rezago = 0;
                    Adeudo_Honorarios = 0;
                    Adeudo_Recargos = 0;
                }
                else
                {
                    Dt_Adeudos = Consultar_Adeudos_Cuenta_Predial(
                        Cuenta_Predial_ID,
                        Fecha_Pago,
                        out Total_Adeudo,
                        out Adeudo_Corriente,
                        out Adeudo_Rezago,
                        out Adeudo_Honorarios,
                        out Adeudo_Recargos,
                        out Periodo_Corriente,
                        out Periodo_Rezago);

                    // calcular descuento
                    string Mes_Pago = Fecha_Pago.ToString("MMMM").ToUpper();
                    if (Dicc_Porciento_Descuento.ContainsKey(Mes_Pago))
                    {
                        Descuento = Adeudo_Corriente * Dicc_Porciento_Descuento[Mes_Pago] / 100;
                    }

                    // validar que el descuento restado al adeudo corriente no sea menor que la cuota mínima
                    if (Cuota_Minima > 0 && Adeudo_Corriente - Descuento < Cuota_Minima)
                    {
                        Descuento = Adeudo_Corriente - Cuota_Minima;
                    }
                    // validar descuentos negativos
                    if (Descuento < 0)
                    {
                        Descuento = 0;
                    }
                    // agregar datos a las tablas para aplicar el pago
                    Agregar_Adeudos_Predial(Dt_Adeudos, Dt_Adeudo_Detallado_Predial, Cuenta_Predial_ID);
                    Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "CORRIENTE", Adeudo_Corriente, "");
                    Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "REZAGO", Adeudo_Rezago, "");
                    Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "RECARGOS", Adeudo_Recargos, "");
                    Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "HONORARIOS", Adeudo_Honorarios, "");
                    Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "DESCUENTOS_CORRIENTES", Descuento, "");
                    Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "MONTO_TOTAL_PAGADO", Importe, "");
                    // agregar a la tabla pasivos
                    if (!Dicc_Tipos_Predio.ContainsKey(Tipo_Predio_ID))
                    {
                        Mensaje += "No se encontró el tipo de predio para la cuenta " + Cuenta_Predial + "<br />";
                        Incluido = "NO";
                    }
                    else
                    {
                        string Mensaje_Pasivo = Agregar_Pasivos_Adeudo(
                            Dt_Pasivos_Pago,
                            Adeudo_Corriente,
                            Adeudo_Rezago,
                            Adeudo_Recargos,
                            Adeudo_Honorarios,
                            Descuento,
                            Cuenta_Predial,
                            Cuenta_Predial_ID,
                            Dicc_Tipos_Predio[Tipo_Predio_ID],
                            Periodo_Corriente,
                            Periodo_Rezago,
                            Fecha_Pago,
                            Propietario,
                            Dic_Claves_Ingreso,
                            Dic_Clave_Dependencia);
                        if (Mensaje_Pasivo.Length > 0)
                        {
                            Incluido = "NO";
                        }
                        Mensaje += Mensaje_Pasivo;
                    }

                    // Recalcular total adeudo
                    Total_Adeudo = Adeudo_Rezago + Adeudo_Corriente - Descuento + Adeudo_Recargos + Adeudo_Honorarios;

                    // si el importe pagado es diferente al adeudo total, marcar el pago como no incluido
                    if ((Importe > (Total_Adeudo + Limite_Pago_Superior)) || (Importe < (Total_Adeudo - Limite_Pago_Inferior)))
                    {
                        Incluido = "NO";
                    }
                }
                Diferencia = Importe - Total_Adeudo;
                Suma_Importes += Importe;
                Contador_Movimientos++;
                // agregar datos a la tabla
                Fila_Pago = Dt_Pagos.NewRow();
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial] = Cuenta_Predial;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial_Id] = Cuenta_Predial_ID;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial_Original] = Cuenta_Predial;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Diferencia] = Diferencia;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Linea_Captura] = Identificador_Pago;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Fecha_Pago] = Fecha_Pago;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Pagado] = Importe;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Pagado_Original] = Importe;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Total_Adeudo] = Total_Adeudo;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Sucursal] = Sucursal.ToString();
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Tipo_Pago] = Tipo_Pago;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Descripcion] = Descripcion;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Incluido] = Incluido;
                Fila_Pago["CONSECUTIVO"] = Contador_Movimientos;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Rezago] = Adeudo_Rezago;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Corriente] = Adeudo_Corriente;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Recargos] = Adeudo_Recargos;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Honorarios] = Adeudo_Honorarios;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Descuento] = Descuento;
                Dt_Pagos.Rows.Add(Fila_Pago);
            }

            // si no hay pagos en la tabla, mostrar mensaje de error en el formato del archivo
            if (Dt_Pagos.Rows.Count <= 0)
            {
                return "El archivo no tiene el formato correcto";
            }
            // mostrar total de movimientos
            Txt_Cantidad_Movimientos.Text = Contador_Movimientos.ToString("#,##0");

            // datos generales de captura
            DataRow Fila_Carga = Dt_Captura.NewRow();
            Fila_Carga[Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Institucion_Id] = Cmb_Banco.SelectedValue;
            Fila_Carga[Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Fecha_Captura] = DateTime.Now;
            Fila_Carga[Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Nombre_Archivo] = Txt_Nombre_Archivo.Text;
            Fila_Carga[Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Fecha_Corte] = Fecha_Corte;
            Dt_Captura.Rows.Add(Fila_Carga);

            Session["Dt_Pagos"] = Dt_Pagos;
            Session["Dt_Captura"] = Dt_Captura;
            Session["Dt_Adeudos_Totales"] = Dt_Adeudos_Totales;
            Session["Dt_Adeudo_Detallado_Predial"] = Dt_Adeudo_Detallado_Predial;
            Session["Dt_Pasivos_Pago"] = Dt_Pasivos_Pago;
            // cargar grids
            Cargar_Grids_Pagos(true);
        }
        catch (Exception ex)
        {
            throw new Exception("Leer_Archivo_Banorte " + ex.Message.ToString(), ex);
        }
        return Mensaje;
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Leer_Archivo_Oxxo
    /// DESCRIPCION: Lee los datos en el archivo recibido de acuerdo con el layout del banco
    /// PARAMETROS: 
    ///             1. Sr_Archivo: contenido del archivo subido por el usuario
    /// 		    2. Limite_Pago_Inferior: cantidad máxima que puede faltar del pago contra el adeudo
    /// 		    3. Limite_Pago_Superior: cantidad máxima que puede superar el pago al adeudo
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 23-mar-2012
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private String Leer_Archivo_Oxxo(StreamReader Sr_Archivo, decimal Limite_Pago_Inferior, decimal Limite_Pago_Superior)
    {
        string Mensaje = "";
        string Linea_Archivo;
        DataTable Dt_Pagos;
        DataTable Dt_Captura;
        string Cuenta_Predial;
        string Cuenta_Predial_ID;
        DateTime Fecha_Actual = DateTime.Now;
        decimal Cuota_Minima;
        string Mes_Movimientos;
        string Tipo_Predio_ID;
        string Propietario;
        var Consulta_Propietario_Negocio = new Cls_Ope_Pre_Pagos_Instituciones_Externas_Negocio();
        // totales archivo
        DateTime Fecha_Movimientos = DateTime.Now;
        Int32 Total_Operaciones = 0;
        decimal Total_Importe = 0;
        Int32 Contador_Movimientos = 0;
        decimal Suma_Importes = 0;
        Int32 Contador_Lineas = 0;
        // detalles pagos
        DateTime Fecha_Pago;
        decimal Importe;
        string Sucursal;
        string Identificador_Pago;
        DataRow Fila_Pago;
        DataRow Fila_Carga;
        var Dicc_Porciento_Descuento = new Dictionary<string, decimal>();
        Dictionary<string, string> Dicc_Tipos_Predio;
        Dictionary<string, string> Dic_Claves_Ingreso;
        Dictionary<string, string> Dic_Clave_Dependencia;
        // adeudos
        decimal Adeudo_Corriente = 0;
        decimal Adeudo_Rezago = 0;
        decimal Adeudo_Recargos = 0;
        decimal Adeudo_Honorarios = 0;
        decimal Total_Adeudo = 0;
        string Periodo_Corriente;
        string Periodo_Rezago;
        // tablas para el pago
        DataTable Dt_Adeudo_Detallado_Predial = Crear_Tabla_Adeudos_Predial();
        DataTable Dt_Adeudos_Totales = Crear_Tabla_Adeudos_Totales();
        DataTable Dt_Pasivos_Pago = Crear_Tabla_Pasivos_Pago();
        DataTable Dt_Adeudos;
        string[] Arreglo_Parametros;

        // obtener cuota minima
        Cuota_Minima = Obtener_Cuota_Minima(Fecha_Actual.Year);
        // crear tabla para pagos
        Dt_Pagos = Crear_Tabla_Pagos();
        Dt_Captura = Crear_Tabla_Captura_Pagos();

        Dicc_Tipos_Predio = Consultar_Tipos_Predio();
        Dic_Claves_Ingreso = Obtener_Diccionario_Claves_Ingreso(out Dic_Clave_Dependencia);

        try
        {
            while (Sr_Archivo.Peek() != -1)
            {
                Linea_Archivo = Sr_Archivo.ReadLine();
                Contador_Lineas++;
                Arreglo_Parametros = Linea_Archivo.Split(',');

                // procesar encabezado (primer caracter 0), movimientos (primer caracter 1) y totales (primer caracter 9)
                switch (Linea_Archivo.Substring(0, 1))
                {
                    case "1": // movimientos
                        // validar que tenga el número de parámetros necesarios (detalles: 11)
                        if (Arreglo_Parametros.Length < 7)
                        {
                            Mensaje += "Se omite la línea " + Contador_Lineas + " debido a que no tiene el mínimo de parámetros para extraer datos <br />";
                            continue;
                        }
                        String Incluido = "SI";
                        decimal Diferencia = 0;
                        decimal Descuento = 0;
                        // recuperar valores de la fecha (tercer y cuarto parámetro: yyyyMMdd)
                        DateTime.TryParseExact(Arreglo_Parametros[2] + Arreglo_Parametros[3], "yyyyMMddHH:mm", null, System.Globalization.DateTimeStyles.None, out Fecha_Pago);

                        // agregar porcentaje de descuento para el mes de los movimientos al diccionario de descuentos
                        Mes_Movimientos = Fecha_Pago.ToString("MMMM").ToUpper();
                        if (!Dicc_Porciento_Descuento.ContainsKey(Mes_Movimientos))
                        {
                            Dicc_Porciento_Descuento.Add(Mes_Movimientos, Obtener_Descuentos_Pronto_Pago(Mes_Movimientos, Fecha_Pago.Year));
                        }

                        // séptimo parámetro
                        Decimal.TryParse(Arreglo_Parametros[6].Trim(), out Importe);
                        Suma_Importes += Importe;
                        Sucursal = Arreglo_Parametros[1].Trim();
                        // recuperar la linea de captura (quitar ceros del lado derecho)
                        Identificador_Pago = Quitar_Caracteres_Derecha(Arreglo_Parametros[4].Trim(), "0");
                        // consultar cuenta
                        Cuenta_Predial_ID = Consultar_Cuenta_Predial_ID(Identificador_Pago, out Cuenta_Predial, Cmb_Banco.SelectedValue, out Tipo_Predio_ID);
                        Propietario = Consultar_Propietario(Cuenta_Predial_ID, Consulta_Propietario_Negocio);
                        // si falta la cuenta predial o el identificador del pago, marcar como no incluido
                        if (Cuenta_Predial == "" || Cuenta_Predial_ID == "" || Identificador_Pago == "")
                        {
                            Incluido = "NO";
                            Total_Adeudo = 0;
                            Adeudo_Corriente = 0;
                            Adeudo_Rezago = 0;
                            Adeudo_Honorarios = 0;
                            Adeudo_Recargos = 0;
                        }
                        else
                        {
                            Dt_Adeudos = Consultar_Adeudos_Cuenta_Predial(
                                Cuenta_Predial_ID,
                                Fecha_Pago,
                                out Total_Adeudo,
                                out Adeudo_Corriente,
                                out Adeudo_Rezago,
                                out Adeudo_Honorarios,
                                out Adeudo_Recargos,
                                out Periodo_Corriente,
                                out Periodo_Rezago);
                            // calcular descuento
                            string Mes_Pago = Fecha_Pago.ToString("MMMM").ToUpper();
                            if (Dicc_Porciento_Descuento.ContainsKey(Mes_Pago))
                            {
                                Descuento = Adeudo_Corriente * Dicc_Porciento_Descuento[Mes_Pago] / 100;
                            }
                            
                            // validar que el descuento restado al adeudo corriente no sea menor que la cuota mínima
                            if (Cuota_Minima > 0 && Adeudo_Corriente - Descuento < Cuota_Minima)
                            {
                                Descuento = Adeudo_Corriente - Cuota_Minima;
                            }
                            // validar descuentos negativos
                            if (Descuento < 0)
                            {
                                Descuento = 0;
                            }
                            // agregar datos a las tablas para aplicar el pago
                            Agregar_Adeudos_Predial(Dt_Adeudos, Dt_Adeudo_Detallado_Predial, Cuenta_Predial_ID);
                            Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "CORRIENTE", Adeudo_Corriente, "");
                            Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "REZAGO", Adeudo_Rezago, "");
                            Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "RECARGOS", Adeudo_Recargos, "");
                            Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "HONORARIOS", Adeudo_Honorarios, "");
                            Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "DESCUENTOS_CORRIENTES", Descuento, "");
                            Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "MONTO_TOTAL_PAGADO", Importe, "");
                            // agregar a la tabla pasivos
                            if (!Dicc_Tipos_Predio.ContainsKey(Tipo_Predio_ID))
                            {
                                Mensaje += "No se encontró el tipo de predio para la cuenta " + Cuenta_Predial + "<br />";
                                Incluido = "NO";
                            }
                            else
                            {
                                string Mensaje_Pasivo = Agregar_Pasivos_Adeudo(
                                    Dt_Pasivos_Pago,
                                    Adeudo_Corriente,
                                    Adeudo_Rezago,
                                    Adeudo_Recargos,
                                    Adeudo_Honorarios,
                                    Descuento,
                                    Cuenta_Predial,
                                    Cuenta_Predial_ID,
                                    Dicc_Tipos_Predio[Tipo_Predio_ID],
                                    Periodo_Corriente,
                                    Periodo_Rezago,
                                    Fecha_Pago,
                                    Propietario,
                                    Dic_Claves_Ingreso,
                                    Dic_Clave_Dependencia);
                                if (Mensaje_Pasivo.Length > 0)
                                {
                                    Incluido = "NO";
                                }
                                Mensaje += Mensaje_Pasivo;
                            }

                            // Recalcular total adeudo
                            Total_Adeudo = Adeudo_Rezago + Adeudo_Corriente - Descuento + Adeudo_Recargos + Adeudo_Honorarios;

                            // si el importe pagado es diferente al adeudo total, marcar el pago como no incluido
                            if ((Importe > (Total_Adeudo + Limite_Pago_Superior)) || (Importe < (Total_Adeudo - Limite_Pago_Inferior)))
                            {
                                Incluido = "NO";
                            }
                        }
                        Diferencia = Importe - Total_Adeudo;
                        Contador_Movimientos++;
                        // agregar datos a la tabla
                        Fila_Pago = Dt_Pagos.NewRow();
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial] = Cuenta_Predial;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial_Id] = Cuenta_Predial_ID;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial_Original] = Cuenta_Predial;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Diferencia] = Diferencia;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Linea_Captura] = Identificador_Pago;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Fecha_Pago] = Fecha_Pago;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Pagado] = Importe;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Pagado_Original] = Importe;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Total_Adeudo] = Total_Adeudo;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Sucursal] = Sucursal.ToString();
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Guia_Cie] = "";
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Descripcion] = "";
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Incluido] = Incluido;
                        Fila_Pago["CONSECUTIVO"] = Contador_Movimientos;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Rezago] = Adeudo_Rezago;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Corriente] = Adeudo_Corriente;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Recargos] = Adeudo_Recargos;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Honorarios] = Adeudo_Honorarios;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Descuento] = Descuento;
                        Dt_Pagos.Rows.Add(Fila_Pago);
                        break;
                    case "2": // total de movimientos
                        DateTime.TryParseExact(Arreglo_Parametros[2] + Arreglo_Parametros[3], "yyyyMMddHH:mm", null, System.Globalization.DateTimeStyles.None, out Fecha_Movimientos);
                        // validar que tenga el número de parámetros necesarios (final: 7)
                        if (Arreglo_Parametros.Length < 7)
                        {
                            Mensaje += "Se omite la línea de control (" + Contador_Lineas + ") debido a que no tiene el mínimo de parámetros para extraer datos <br />";
                            continue;
                        }
                        // quinto parámetro del arreglo
                        Int32.TryParse(Arreglo_Parametros[4].Trim(), out Total_Operaciones);
                        // séptimo parámetro del arreglo
                        Decimal.TryParse(Arreglo_Parametros[6].Trim(), out Total_Importe);
                        break;
                }
            }
            // si no hay pagos en la tabla, mostrar mensaje de error en el formato del archivo
            if (Dt_Pagos.Rows.Count <= 0)
            {
                return "El archivo no tiene el formato correcto";
            }
            // validar que no haya inconsistencias entre los totales en el archivo y los totales leidos
            if (Suma_Importes != Total_Importe)
            {
                Mensaje += "El total en archivo: " + Total_Importe.ToString("#,##0.00")
                    + " es diferente al total calculado al leer el archivo: "
                    + Suma_Importes.ToString("#,##0.00") + ".<br />";
            }
            if (Contador_Movimientos != Total_Operaciones)
            {
                Mensaje += "El total de movimientos en el archivo: " + Total_Operaciones.ToString("#,##0")
                    + " es diferente al número de movimientos leídos: " + Contador_Movimientos.ToString("#,##0") + ".<br />";
            }
            // mostrar total de movimientos
            Txt_Cantidad_Movimientos.Text = Contador_Movimientos.ToString("#,##0");

            // datos generales de captura
            Fila_Carga = Dt_Captura.NewRow();
            Fila_Carga[Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Institucion_Id] = Cmb_Banco.SelectedValue;
            Fila_Carga[Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Fecha_Captura] = DateTime.Now;
            Fila_Carga[Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Nombre_Archivo] = Txt_Nombre_Archivo.Text;
            Fila_Carga[Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Fecha_Corte] = Fecha_Movimientos;
            Dt_Captura.Rows.Add(Fila_Carga);

            Session["Dt_Pagos"] = Dt_Pagos;
            Session["Dt_Captura"] = Dt_Captura;
            Session["Dt_Adeudos_Totales"] = Dt_Adeudos_Totales;
            Session["Dt_Adeudo_Detallado_Predial"] = Dt_Adeudo_Detallado_Predial;
            Session["Dt_Pasivos_Pago"] = Dt_Pasivos_Pago;
            // cargar grids
            Grid_Pagos_Incluidos.Columns[6].Visible = true;
            Grid_Pagos_Excluidos.Columns[7].Visible = true;
            Cargar_Grids_Pagos(true);
        }
        catch (Exception ex)
        {
            throw new Exception("Leer_Archivo_Oxxo " + ex.Message.ToString(), ex);
        }
        return Mensaje;
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Leer_Archivo_Santander
    /// DESCRIPCION: Lee los datos en el archivo recibido de acuerdo con el layout del banco
    /// PARAMETROS: 
    ///             1. Sr_Archivo: contenido del archivo subido por el usuario
    /// 		    2. Limite_Pago_Inferior: cantidad máxima que puede faltar del pago contra el adeudo
    /// 		    3. Limite_Pago_Superior: cantidad máxima que puede superar el pago al adeudo
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 16-ene-2012
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private String Leer_Archivo_Santander(StreamReader Sr_Archivo, decimal Limite_Pago_Inferior, decimal Limite_Pago_Superior)
    {
        String Mensaje = "";
        String Linea_Archivo;
        DataTable Dt_Pagos;
        DataTable Dt_Captura;
        var Dicc_Porciento_Descuento = new Dictionary<string, decimal>();
        Dictionary<string, string> Dicc_Tipos_Predio;
        Dictionary<string, string> Dic_Claves_Ingreso;
        Dictionary<string, string> Dic_Clave_Dependencia;
        decimal Cuota_Minima;
        decimal Descuento;
        string Mes_Movimientos;
        string Tipo_Predio_ID;
        string Propietario;
        var Consulta_Propietario_Negocio = new Cls_Ope_Pre_Pagos_Instituciones_Externas_Negocio();
        // totales archivo
        Int32 Contador_Movimientos = 0;
        Decimal Suma_Importes = 0;
        Int32 Contador_Lineas = 0;
        // detalles pagos
        DateTime Fecha_Pago = DateTime.Now;
        Decimal Importe;
        String Sucursal;
        String Identificador_Pago;
        DataRow Fila_Pago;
        String Descripcion;
        String Cuenta_Predial;
        String Cuenta_Predial_ID;
        string Tipo_Pago;
        // tablas para aplicación de pagos
        DataTable Dt_Adeudo_Detallado_Predial = Crear_Tabla_Adeudos_Predial();
        DataTable Dt_Adeudos_Totales = Crear_Tabla_Adeudos_Totales();
        DataTable Dt_Pasivos_Pago = Crear_Tabla_Pasivos_Pago();
        DataTable Dt_Adeudos;
        // adeudos
        decimal Adeudo_Corriente = 0;
        decimal Adeudo_Rezago = 0;
        decimal Adeudo_Recargos = 0;
        decimal Adeudo_Honorarios = 0;
        decimal Total_Adeudo = 0;
        string Periodo_Corriente;
        string Periodo_Rezago;

        // crear tabla para pagos
        Dt_Pagos = Crear_Tabla_Pagos();
        Dt_Captura = Crear_Tabla_Captura_Pagos();

        // obtener cuota minima
        Cuota_Minima = Obtener_Cuota_Minima(Fecha_Pago.Year);

        Dicc_Tipos_Predio = Consultar_Tipos_Predio();
        Dic_Claves_Ingreso = Obtener_Diccionario_Claves_Ingreso(out Dic_Clave_Dependencia);

        try
        {
            while (Sr_Archivo.Peek() != -1)
            {
                String Incluido = "SI";
                decimal Diferencia = 0;
                Descuento = 0;

                Linea_Archivo = Sr_Archivo.ReadLine();
                Contador_Lineas++;
                // solo contiene lineas con datos
                if (Linea_Archivo.Length < 150)
                {
                    Mensaje += "Se omite la línea " + Contador_Lineas + " debido a que no tiene la longitud mínima para extraer datos <br />";
                    continue;
                }

                // caracteres 114-149(25): línea captura
                Identificador_Pago = Linea_Archivo.Substring(113, 25).Trim();
                // caracteres 77-91(15): importe con signo
                Decimal.TryParse(Linea_Archivo.Substring(76, 15), out Importe);
                Importe /= 100;
                // caracteres 1-16(16): cuenta para el pago
                Descripcion = Linea_Archivo.Substring(0, 16).Trim();
                // recuperar valores de la fecha
                DateTime.TryParseExact(Linea_Archivo.Substring(16, 12), "MMddyyyyHHmm", null, System.Globalization.DateTimeStyles.None, out Fecha_Pago);
                // sucursal
                Sucursal = Linea_Archivo.Substring(28, 4);

                Tipo_Pago = Linea_Archivo.Substring(36, 40).Trim();

                Cuenta_Predial_ID = Consultar_Cuenta_Predial_ID(Identificador_Pago, out Cuenta_Predial, Cmb_Banco.SelectedValue, out Tipo_Predio_ID);
                Propietario = Consultar_Propietario(Cuenta_Predial_ID, Consulta_Propietario_Negocio);

                // agregar porcentaje de descuento para el mes de los movimientos al diccionario de descuentos
                Mes_Movimientos = Fecha_Pago.ToString("MMMM").ToUpper();
                if (!Dicc_Porciento_Descuento.ContainsKey(Mes_Movimientos))
                {
                    Dicc_Porciento_Descuento.Add(Mes_Movimientos, Obtener_Descuentos_Pronto_Pago(Mes_Movimientos, Fecha_Pago.Year));
                }

                // si falta la cuenta predial o el identificador del pago, marcar como no incluido
                if (Cuenta_Predial == "" || Cuenta_Predial_ID == "" || Identificador_Pago == "")
                {
                    Incluido = "NO";
                    Total_Adeudo = 0;
                    Adeudo_Corriente = 0;
                    Adeudo_Rezago = 0;
                    Adeudo_Honorarios = 0;
                    Adeudo_Recargos = 0;
                }
                else
                {
                    Dt_Adeudos = Consultar_Adeudos_Cuenta_Predial(
                        Cuenta_Predial_ID,
                        Fecha_Pago,
                        out Total_Adeudo,
                        out Adeudo_Corriente,
                        out Adeudo_Rezago,
                        out Adeudo_Honorarios,
                        out Adeudo_Recargos,
                        out Periodo_Corriente,
                        out Periodo_Rezago);

                    // calcular descuento
                    string Mes_Pago = Fecha_Pago.ToString("MMMM").ToUpper();
                    if (Dicc_Porciento_Descuento.ContainsKey(Mes_Pago))
                    {
                        Descuento = Adeudo_Corriente * Dicc_Porciento_Descuento[Mes_Pago] / 100;
                    }

                    // validar que el descuento restado al adeudo corriente no sea menor que la cuota mínima
                    if (Cuota_Minima > 0 && Adeudo_Corriente - Descuento < Cuota_Minima)
                    {
                        Descuento = Adeudo_Corriente - Cuota_Minima;
                    }
                    // validar descuentos negativos
                    if (Descuento < 0)
                    {
                        Descuento = 0;
                    }
                    // agregar datos a las tablas para aplicar el pago
                    Agregar_Adeudos_Predial(Dt_Adeudos, Dt_Adeudo_Detallado_Predial, Cuenta_Predial_ID);
                    Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "CORRIENTE", Adeudo_Corriente, "");
                    Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "REZAGO", Adeudo_Rezago, "");
                    Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "RECARGOS", Adeudo_Recargos, "");
                    Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "HONORARIOS", Adeudo_Honorarios, "");
                    Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "DESCUENTOS_CORRIENTES", Descuento, "");
                    Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "MONTO_TOTAL_PAGADO", Importe, "");
                    // agregar a la tabla pasivos
                    if (!Dicc_Tipos_Predio.ContainsKey(Tipo_Predio_ID))
                    {
                        Mensaje += "No se encontró el tipo de predio para la cuenta " + Cuenta_Predial + "<br />";
                        Incluido = "NO";
                    }
                    else
                    {
                        string Mensaje_Pasivo = Agregar_Pasivos_Adeudo(
                            Dt_Pasivos_Pago,
                            Adeudo_Corriente,
                            Adeudo_Rezago,
                            Adeudo_Recargos,
                            Adeudo_Honorarios,
                            Descuento,
                            Cuenta_Predial,
                            Cuenta_Predial_ID,
                            Dicc_Tipos_Predio[Tipo_Predio_ID],
                            Periodo_Corriente,
                            Periodo_Rezago,
                            Fecha_Pago,
                            Propietario,
                            Dic_Claves_Ingreso,
                            Dic_Clave_Dependencia);
                        if (Mensaje_Pasivo.Length > 0)
                        {
                            Incluido = "NO";
                        }
                        Mensaje += Mensaje_Pasivo;
                    }

                    // Recalcular total adeudo
                    Total_Adeudo = Adeudo_Rezago + Adeudo_Corriente - Descuento + Adeudo_Recargos + Adeudo_Honorarios;

                    // si el importe pagado es diferente al adeudo total, marcar el pago como no incluido
                    if ((Importe > (Total_Adeudo + Limite_Pago_Superior)) || (Importe < (Total_Adeudo - Limite_Pago_Inferior)))
                    {
                        Incluido = "NO";
                    }
                }
                Diferencia = Importe - Total_Adeudo;
                Suma_Importes += Importe;
                Contador_Movimientos++;
                // agregar datos a la tabla
                Fila_Pago = Dt_Pagos.NewRow();
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial] = Cuenta_Predial;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial_Id] = Cuenta_Predial_ID;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial_Original] = Cuenta_Predial;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Diferencia] = Diferencia;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Linea_Captura] = Identificador_Pago;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Fecha_Pago] = Fecha_Pago;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Pagado] = Importe;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Pagado_Original] = Importe;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Total_Adeudo] = Total_Adeudo;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Sucursal] = Sucursal.ToString();
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Tipo_Pago] = Tipo_Pago;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Descripcion] = Descripcion;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Incluido] = Incluido;
                Fila_Pago["CONSECUTIVO"] = Contador_Movimientos;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Rezago] = Adeudo_Rezago;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Corriente] = Adeudo_Corriente;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Recargos] = Adeudo_Recargos;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Honorarios] = Adeudo_Honorarios;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Descuento] = Descuento;
                Dt_Pagos.Rows.Add(Fila_Pago);
            }

            // si no hay pagos en la tabla, mostrar mensaje de error en el formato del archivo
            if (Dt_Pagos.Rows.Count <= 0)
            {
                return "El archivo no tiene el formato correcto";
            }
            // mostrar total de movimientos
            Txt_Cantidad_Movimientos.Text = Contador_Movimientos.ToString("#,##0");

            // datos generales de captura
            DataRow Fila_Carga = Dt_Captura.NewRow();
            Fila_Carga[Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Institucion_Id] = Cmb_Banco.SelectedValue;
            Fila_Carga[Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Fecha_Captura] = DateTime.Now;
            Fila_Carga[Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Nombre_Archivo] = Txt_Nombre_Archivo.Text;
            Fila_Carga[Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Fecha_Corte] = Fecha_Pago;
            Dt_Captura.Rows.Add(Fila_Carga);

            Session["Dt_Pagos"] = Dt_Pagos;
            Session["Dt_Captura"] = Dt_Captura;
            Session["Dt_Adeudos_Totales"] = Dt_Adeudos_Totales;
            Session["Dt_Adeudo_Detallado_Predial"] = Dt_Adeudo_Detallado_Predial;
            Session["Dt_Pasivos_Pago"] = Dt_Pasivos_Pago;
            // cargar grids
            Cargar_Grids_Pagos(true);
        }
        catch (Exception ex)
        {
            throw new Exception("Leer_Archivo_Santander " + ex.Message.ToString(), ex);
        }
        return Mensaje;
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Leer_Archivo_Scotiabank
    /// DESCRIPCION: Lee los datos en el archivo recibido de acuerdo con el layout del banco
    /// PARAMETROS: 
    ///             1. Sr_Archivo: contenido del archivo subido por el usuario
    /// 		    2. Limite_Pago_Inferior: cantidad máxima que puede faltar del pago contra el adeudo
    /// 		    3. Limite_Pago_Superior: cantidad máxima que puede superar el pago al adeudo
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 16-ene-2012
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private String Leer_Archivo_Scotiabank(StreamReader Sr_Archivo, decimal Limite_Pago_Inferior, decimal Limite_Pago_Superior)
    {
        string Mensaje = "";
        string Linea_Archivo;
        DataTable Dt_Pagos;
        DataTable Dt_Captura;
        string Cuenta_Predial;
        string Cuenta_Predial_ID;
        DateTime Fecha_Actual = DateTime.Now;
        decimal Cuota_Minima;
        string Mes_Movimientos;
        string Tipo_Predio_ID;
        string Propietario;
        var Consulta_Propietario_Negocio = new Cls_Ope_Pre_Pagos_Instituciones_Externas_Negocio();
        // totales archivo
        DateTime Fecha_Movimientos = DateTime.Now;
        Int32 Total_Operaciones = 0;
        decimal Total_Importe = 0;
        Int32 Contador_Movimientos = 0;
        decimal Suma_Importes = 0;
        Int32 Contador_Lineas = 0;
        // detalles pagos
        DateTime Fecha_Pago;
        decimal Importe;
        string Tipo_Pago;
        string Sucursal;
        string Cajero;
        string Identificador_Pago;
        DataRow Fila_Pago;
        DataRow Fila_Carga;
        var Dicc_Porciento_Descuento = new Dictionary<string, decimal>();
        Dictionary<string, string> Dicc_Tipos_Predio;
        Dictionary<string, string> Dic_Claves_Ingreso;
        Dictionary<string, string> Dic_Clave_Dependencia;
        // adeudos
        decimal Adeudo_Corriente = 0;
        decimal Adeudo_Rezago = 0;
        decimal Adeudo_Recargos = 0;
        decimal Adeudo_Honorarios = 0;
        decimal Total_Adeudo = 0;
        string Periodo_Corriente;
        string Periodo_Rezago;
        // tablas para el pago
        DataTable Dt_Adeudo_Detallado_Predial = Crear_Tabla_Adeudos_Predial();
        DataTable Dt_Adeudos_Totales = Crear_Tabla_Adeudos_Totales();
        DataTable Dt_Pasivos_Pago = Crear_Tabla_Pasivos_Pago();
        DataTable Dt_Adeudos;

        // obtener cuota minima
        Cuota_Minima = Obtener_Cuota_Minima(Fecha_Actual.Year);
        // crear tabla para pagos
        Dt_Pagos = Crear_Tabla_Pagos();
        Dt_Captura = Crear_Tabla_Captura_Pagos();

        Dicc_Tipos_Predio = Consultar_Tipos_Predio();
        Dic_Claves_Ingreso = Obtener_Diccionario_Claves_Ingreso(out Dic_Clave_Dependencia);

        try
        {
            while (Sr_Archivo.Peek() != -1)
            {
                Linea_Archivo = Sr_Archivo.ReadLine();
                Contador_Lineas++;
                // validar longitud de la linea leida
                if (Linea_Archivo.Length < 60)
                {
                    Mensaje += "Se omite la línea " + Contador_Lineas + " debido a que no tiene la longitud mínima para extraer datos <br />";
                    continue;
                }

                // procesar encabezado (primer caracter 0), movimientos (primer caracter 1) y totales (primer caracter 9)
                switch (Linea_Archivo.Substring(0, 1))
                {
                    case "H": // encabezado (fecha)
                        // validar longitud de la linea leida
                        if (Linea_Archivo.Length < 61)
                        {
                            Mensaje += "Se omite la línea encabezado (" + Contador_Lineas + ") debido a que no tiene la longitud mínima para extraer datos <br />";
                            continue;
                        }

                        // recuperar valores de la fecha (caracteres 6 a 14 con formato: aaaammdd)
                        DateTime.TryParseExact(Linea_Archivo.Substring(5, 8), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out Fecha_Movimientos);
                        // agregar porcentaje de descuento para el mes de los movimientos al diccionario de descuentos
                        Mes_Movimientos = Fecha_Movimientos.ToString("MMMM").ToUpper();
                        if (!Dicc_Porciento_Descuento.ContainsKey(Mes_Movimientos))
                        {
                            Dicc_Porciento_Descuento.Add(Mes_Movimientos, Obtener_Descuentos_Pronto_Pago(Mes_Movimientos, Fecha_Movimientos.Year));
                        }
                        break;
                    case "D": // movimientos
                        // validar longitud de la linea leida
                        if (Linea_Archivo.Length < 237)
                        {
                            Mensaje += "Se omite la línea " + Contador_Lineas + " debido a que no tiene la longitud mínima para extraer datos <br />";
                            continue;
                        }

                        String Incluido = "SI";
                        decimal Diferencia = 0;
                        decimal Descuento = 0;
                        // recuperar valores de la fecha (caracteres 92 a 100 con formato: aaaammdd)
                        DateTime.TryParseExact(Linea_Archivo.Substring(91, 8) + Linea_Archivo.Substring(232, 4), "yyyyMMddHHmm", null, System.Globalization.DateTimeStyles.None, out Fecha_Pago);

                        // agregar porcentaje de descuento para el mes de los movimientos al diccionario de descuentos
                        Mes_Movimientos = Fecha_Pago.ToString("MMMM").ToUpper();
                        if (!Dicc_Porciento_Descuento.ContainsKey(Mes_Movimientos))
                        {
                            Dicc_Porciento_Descuento.Add(Mes_Movimientos, Obtener_Descuentos_Pronto_Pago(Mes_Movimientos, Fecha_Pago.Year));
                        }

                        // importe: caracteres 138 a 152 parte decimal del importe
                        Decimal.TryParse(Linea_Archivo.Substring(137, 15), out Importe);
                        Importe /= 100;
                        Suma_Importes += Importe;
                        Tipo_Pago = Linea_Archivo.Substring(167, 3);
                        Sucursal = Linea_Archivo.Substring(18, 3);
                        Cajero = Linea_Archivo.Substring(15, 3);
                        Identificador_Pago = Linea_Archivo.Substring(53, 30).Trim();
                        Cuenta_Predial_ID = Consultar_Cuenta_Predial_ID(Identificador_Pago, out Cuenta_Predial, Cmb_Banco.SelectedValue, out Tipo_Predio_ID);
                        Propietario = Consultar_Propietario(Cuenta_Predial_ID, Consulta_Propietario_Negocio);
                        // si falta la cuenta predial o el identificador del pago, marcar como no incluido
                        if (Cuenta_Predial == "" || Cuenta_Predial_ID == "" || Identificador_Pago == "")
                        {
                            Incluido = "NO";
                            Total_Adeudo = 0;
                            Adeudo_Corriente = 0;
                            Adeudo_Rezago = 0;
                            Adeudo_Honorarios = 0;
                            Adeudo_Recargos = 0;
                        }
                        else
                        {
                            Dt_Adeudos = Consultar_Adeudos_Cuenta_Predial(
                                Cuenta_Predial_ID,
                                Fecha_Pago,
                                out Total_Adeudo,
                                out Adeudo_Corriente,
                                out Adeudo_Rezago,
                                out Adeudo_Honorarios,
                                out Adeudo_Recargos,
                                out Periodo_Corriente,
                                out Periodo_Rezago);
                            // calcular descuento
                            string Mes_Pago = Fecha_Pago.ToString("MMMM").ToUpper();
                            if (Dicc_Porciento_Descuento.ContainsKey(Mes_Pago))
                            {
                                Descuento = Adeudo_Corriente * Dicc_Porciento_Descuento[Mes_Pago] / 100;
                            }

                            // validar que el descuento restado al adeudo corriente no sea menor que la cuota mínima
                            if (Cuota_Minima > 0 && Adeudo_Corriente - Descuento < Cuota_Minima)
                            {
                                Descuento = Adeudo_Corriente - Cuota_Minima;
                            }
                            // validar descuentos negativos
                            if (Descuento < 0)
                            {
                                Descuento = 0;
                            }
                            // agregar datos a las tablas para aplicar el pago
                            Agregar_Adeudos_Predial(Dt_Adeudos, Dt_Adeudo_Detallado_Predial, Cuenta_Predial_ID);
                            Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "CORRIENTE", Adeudo_Corriente, "");
                            Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "REZAGO", Adeudo_Rezago, "");
                            Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "RECARGOS", Adeudo_Recargos, "");
                            Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "HONORARIOS", Adeudo_Honorarios, "");
                            Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "DESCUENTOS_CORRIENTES", Descuento, "");
                            Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "MONTO_TOTAL_PAGADO", Importe, "");
                            // agregar a la tabla pasivos
                            if (!Dicc_Tipos_Predio.ContainsKey(Tipo_Predio_ID))
                            {
                                Mensaje += "No se encontró el tipo de predio para la cuenta " + Cuenta_Predial + "<br />";
                                Incluido = "NO";
                            }
                            else
                            {
                                string Mensaje_Pasivo = Agregar_Pasivos_Adeudo(
                                    Dt_Pasivos_Pago,
                                    Adeudo_Corriente,
                                    Adeudo_Rezago,
                                    Adeudo_Recargos,
                                    Adeudo_Honorarios,
                                    Descuento,
                                    Cuenta_Predial,
                                    Cuenta_Predial_ID,
                                    Dicc_Tipos_Predio[Tipo_Predio_ID],
                                    Periodo_Corriente,
                                    Periodo_Rezago,
                                    Fecha_Pago,
                                    Propietario,
                                    Dic_Claves_Ingreso,
                                    Dic_Clave_Dependencia);
                                if (Mensaje_Pasivo.Length > 0)
                                {
                                    Incluido = "NO";
                                }
                                Mensaje += Mensaje_Pasivo;
                            }

                            // Recalcular total adeudo - Descuento
                            Total_Adeudo = Adeudo_Rezago + Adeudo_Corriente - Descuento + Adeudo_Recargos + Adeudo_Honorarios;

                            // si el importe pagado es diferente al adeudo total, marcar el pago como no incluido
                            if ((Importe > (Total_Adeudo + Limite_Pago_Superior)) || (Importe < (Total_Adeudo - Limite_Pago_Inferior)))
                            {
                                Incluido = "NO";
                            }
                        }
                        Diferencia = Importe - Total_Adeudo;
                        Contador_Movimientos++;
                        // agregar datos a la tabla
                        Fila_Pago = Dt_Pagos.NewRow();
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial] = Cuenta_Predial;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial_Id] = Cuenta_Predial_ID;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial_Original] = Cuenta_Predial;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Diferencia] = Diferencia;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Linea_Captura] = Identificador_Pago;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Fecha_Pago] = Fecha_Pago;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Pagado] = Importe;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Pagado_Original] = Importe;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Total_Adeudo] = Total_Adeudo;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Sucursal] = Sucursal;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Tipo_Pago] = Tipo_Pago;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Guia_Cie] = "";
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cajero] = Cajero;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Descripcion] = "";
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Incluido] = Incluido;
                        Fila_Pago["CONSECUTIVO"] = Contador_Movimientos;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Rezago] = Adeudo_Rezago;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Corriente] = Adeudo_Corriente;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Recargos] = Adeudo_Recargos;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Honorarios] = Adeudo_Honorarios;
                        Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Descuento] = Descuento;
                        Dt_Pagos.Rows.Add(Fila_Pago);
                        break;
                    case "T": // total de movimientos
                        // validar longitud de la linea leida
                        if (Linea_Archivo.Length < 81)
                        {
                            Mensaje += "Se omite la línea de totales " + Contador_Lineas + " debido a que no tiene la longitud mínima para extraer datos <br />";
                            continue;
                        }
                        // carcteres 2 a 10: total de movimientos en el archivo
                        Int32.TryParse(Linea_Archivo.Substring(1, 9), out Total_Operaciones);
                        // caracteres 11 a 27 parte entera y 27 y 28 parte decimal del importe total en el archivo
                        Decimal.TryParse(Linea_Archivo.Substring(10, 17), out Total_Importe);
                        Total_Importe /= 100;
                        break;
                }
            }
            // si no hay pagos en la tabla, mostrar mensaje de error en el formato del archivo
            if (Dt_Pagos.Rows.Count <= 0)
            {
                return "El archivo no tiene el formato correcto";
            }
            // validar que no haya inconsistencias entre los totales en el archivo y los totales leidos
            if (Suma_Importes != Total_Importe)
            {
                Mensaje += "El total en archivo: " + Total_Importe.ToString("#,##0.00")
                    + " es diferente al total calculado al leer el archivo: "
                    + Suma_Importes.ToString("#,##0.00") + ".<br />";
            }
            if (Contador_Movimientos != Total_Operaciones)
            {
                Mensaje += "El total de movimientos en el archivo: " + Total_Operaciones.ToString("#,##0")
                    + " es diferente al número de movimientos leídos: " + Contador_Movimientos.ToString("#,##0") + ".<br />";
            }
            // mostrar total de movimientos
            Txt_Cantidad_Movimientos.Text = Contador_Movimientos.ToString("#,##0");

            // datos generales de captura
            Fila_Carga = Dt_Captura.NewRow();
            Fila_Carga[Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Institucion_Id] = Cmb_Banco.SelectedValue;
            Fila_Carga[Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Fecha_Captura] = DateTime.Now;
            Fila_Carga[Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Nombre_Archivo] = Txt_Nombre_Archivo.Text;
            Fila_Carga[Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Fecha_Corte] = Fecha_Movimientos;
            Dt_Captura.Rows.Add(Fila_Carga);

            Session["Dt_Pagos"] = Dt_Pagos;
            Session["Dt_Captura"] = Dt_Captura;
            Session["Dt_Adeudos_Totales"] = Dt_Adeudos_Totales;
            Session["Dt_Adeudo_Detallado_Predial"] = Dt_Adeudo_Detallado_Predial;
            Session["Dt_Pasivos_Pago"] = Dt_Pasivos_Pago;
            // cargar grids
            Grid_Pagos_Incluidos.Columns[7].Visible = true;
            Grid_Pagos_Incluidos.Columns[9].Visible = true;
            Grid_Pagos_Excluidos.Columns[8].Visible = true;
            Grid_Pagos_Excluidos.Columns[10].Visible = true;
            Cargar_Grids_Pagos(true);
        }
        catch (Exception ex)
        {
            throw new Exception("Leer_Archivo_Scotiabank " + ex.Message.ToString(), ex);
        }
        return Mensaje;
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Leer_Archivo_HSBC
    /// DESCRIPCION: Lee los datos en el archivo recibido de acuerdo con el layout del banco
    /// PARAMETROS: 
    ///             1. Sr_Archivo: contenido del archivo subido por el usuario
    /// 		    2. Limite_Pago_Inferior: cantidad máxima que puede faltar del pago contra el adeudo
    /// 		    3. Limite_Pago_Superior: cantidad máxima que puede superar el pago al adeudo
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 16-ene-2012
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private String Leer_Archivo_HSBC(StreamReader Sr_Archivo, decimal Limite_Pago_Inferior, decimal Limite_Pago_Superior)
    {
        String Mensaje = "";
        String Linea_Archivo;
        DataTable Dt_Pagos;
        DataTable Dt_Captura;
        var Dicc_Porciento_Descuento = new Dictionary<string, decimal>();
        Dictionary<string, string> Dicc_Tipos_Predio;
        Dictionary<string, string> Dic_Claves_Ingreso;
        Dictionary<string, string> Dic_Clave_Dependencia;
        decimal Cuota_Minima;
        decimal Descuento;
        string Mes_Movimientos;
        string Tipo_Predio_ID;
        string Propietario;
        var Consulta_Propietario_Negocio = new Cls_Ope_Pre_Pagos_Instituciones_Externas_Negocio();
        // totales archivo
        Int32 Contador_Movimientos = 0;
        Decimal Suma_Importes = 0;
        Int32 Contador_Lineas = 0;
        // detalles pagos
        DateTime Fecha_Pago = DateTime.Now;
        Decimal Importe;
        String Sucursal;
        String Identificador_Pago;
        DataRow Fila_Pago;
        String Descripcion;
        String Cuenta_Predial;
        String Cuenta_Predial_ID;
        //string Tipo_Pago;
        // tablas para aplicación de pagos
        DataTable Dt_Adeudo_Detallado_Predial = Crear_Tabla_Adeudos_Predial();
        DataTable Dt_Adeudos_Totales = Crear_Tabla_Adeudos_Totales();
        DataTable Dt_Pasivos_Pago = Crear_Tabla_Pasivos_Pago();
        DataTable Dt_Adeudos;
        // adeudos
        decimal Adeudo_Corriente = 0;
        decimal Adeudo_Rezago = 0;
        decimal Adeudo_Recargos = 0;
        decimal Adeudo_Honorarios = 0;
        decimal Total_Adeudo = 0;
        string Periodo_Corriente;
        string Periodo_Rezago;

        // crear tabla para pagos
        Dt_Pagos = Crear_Tabla_Pagos();
        Dt_Captura = Crear_Tabla_Captura_Pagos();

        // obtener cuota minima
        Cuota_Minima = Obtener_Cuota_Minima(Fecha_Pago.Year);

        Dicc_Tipos_Predio = Consultar_Tipos_Predio();
        Dic_Claves_Ingreso = Obtener_Diccionario_Claves_Ingreso(out Dic_Clave_Dependencia);

        try
        {
            while (Sr_Archivo.Peek() != -1)
            {
                String Incluido = "SI";
                decimal Diferencia = 0;
                Descuento = 0;

                Linea_Archivo = Sr_Archivo.ReadLine();
                Contador_Lineas++;
                // solo contiene lineas con datos
                if (Linea_Archivo.Length < 150)
                {
                    Mensaje += "Se omite la línea " + Contador_Lineas + " debido a que no tiene la longitud mínima para extraer datos <br />";
                    continue;
                }

                // caracteres 123-162(40): línea captura
                Identificador_Pago = Linea_Archivo.Substring(122, 40).Trim();
                // caracteres 90-102(13): importe
                Decimal.TryParse(Linea_Archivo.Substring(89, 13), out Importe);
                Importe /= 100;
                // si el importe es cero, ignorar línea
                if (Importe == 0)
                {
                    continue;
                }

                // caracteres 1-16(16): cuenta del pago
                Descripcion = Linea_Archivo.Substring(0, 18).Trim();
                // recuperar valores de la fecha
                DateTime.TryParseExact(Linea_Archivo.Substring(18, 18), "dd/MM/yyyyHH:mm:ss", null, System.Globalization.DateTimeStyles.None, out Fecha_Pago);
                // sucursal
                Sucursal = Linea_Archivo.Substring(162, 5);

                //Tipo_Pago = Linea_Archivo.Substring(36, 40).Trim();

                Cuenta_Predial_ID = Consultar_Cuenta_Predial_ID(Identificador_Pago, out Cuenta_Predial, Cmb_Banco.SelectedValue, out Tipo_Predio_ID);
                Propietario = Consultar_Propietario(Cuenta_Predial_ID, Consulta_Propietario_Negocio);

                // agregar porcentaje de descuento para el mes de los movimientos al diccionario de descuentos
                Mes_Movimientos = Fecha_Pago.ToString("MMMM").ToUpper();
                if (!Dicc_Porciento_Descuento.ContainsKey(Mes_Movimientos))
                {
                    Dicc_Porciento_Descuento.Add(Mes_Movimientos, Obtener_Descuentos_Pronto_Pago(Mes_Movimientos, Fecha_Pago.Year));
                }

                // si falta la cuenta predial o el identificador del pago, marcar como no incluido
                if (Cuenta_Predial == "" || Cuenta_Predial_ID == "" || Identificador_Pago == "")
                {
                    Incluido = "NO";
                    Total_Adeudo = 0;
                    Adeudo_Corriente = 0;
                    Adeudo_Rezago = 0;
                    Adeudo_Honorarios = 0;
                    Adeudo_Recargos = 0;
                }
                else
                {
                    Dt_Adeudos = Consultar_Adeudos_Cuenta_Predial(
                        Cuenta_Predial_ID,
                        Fecha_Pago,
                        out Total_Adeudo,
                        out Adeudo_Corriente,
                        out Adeudo_Rezago,
                        out Adeudo_Honorarios,
                        out Adeudo_Recargos,
                        out Periodo_Corriente,
                        out Periodo_Rezago);

                    // calcular descuento
                    string Mes_Pago = Fecha_Pago.ToString("MMMM").ToUpper();
                    if (Dicc_Porciento_Descuento.ContainsKey(Mes_Pago))
                    {
                        Descuento = Adeudo_Corriente * Dicc_Porciento_Descuento[Mes_Pago] / 100;
                    }

                    // validar que el descuento restado al adeudo corriente no sea menor que la cuota mínima
                    if (Cuota_Minima > 0 && Adeudo_Corriente - Descuento < Cuota_Minima)
                    {
                        Descuento = Adeudo_Corriente - Cuota_Minima;
                    }
                    // validar descuentos negativos
                    if (Descuento < 0)
                    {
                        Descuento = 0;
                    }
                    // agregar datos a las tablas para aplicar el pago
                    Agregar_Adeudos_Predial(Dt_Adeudos, Dt_Adeudo_Detallado_Predial, Cuenta_Predial_ID);
                    Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "CORRIENTE", Adeudo_Corriente, "");
                    Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "REZAGO", Adeudo_Rezago, "");
                    Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "RECARGOS", Adeudo_Recargos, "");
                    Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "HONORARIOS", Adeudo_Honorarios, "");
                    Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "DESCUENTOS_CORRIENTES", Descuento, "");
                    Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "MONTO_TOTAL_PAGADO", Importe, "");
                    // agregar a la tabla pasivos
                    if (!Dicc_Tipos_Predio.ContainsKey(Tipo_Predio_ID))
                    {
                        Mensaje += "No se encontró el tipo de predio para la cuenta " + Cuenta_Predial + "<br />";
                        Incluido = "NO";
                    }
                    else
                    {
                        string Mensaje_Pasivo = Agregar_Pasivos_Adeudo(
                            Dt_Pasivos_Pago,
                            Adeudo_Corriente,
                            Adeudo_Rezago,
                            Adeudo_Recargos,
                            Adeudo_Honorarios,
                            Descuento,
                            Cuenta_Predial,
                            Cuenta_Predial_ID,
                            Dicc_Tipos_Predio[Tipo_Predio_ID],
                            Periodo_Corriente,
                            Periodo_Rezago,
                            Fecha_Pago,
                            Propietario,
                            Dic_Claves_Ingreso,
                            Dic_Clave_Dependencia);
                        if (Mensaje_Pasivo.Length > 0)
                        {
                            Incluido = "NO";
                        }
                        Mensaje += Mensaje_Pasivo;
                    }

                    // Recalcular total adeudo
                    Total_Adeudo = Adeudo_Rezago + Adeudo_Corriente - Descuento + Adeudo_Recargos + Adeudo_Honorarios;

                    // si el importe pagado es diferente al adeudo total, marcar el pago como no incluido
                    if ((Importe > (Total_Adeudo + Limite_Pago_Superior)) || (Importe < (Total_Adeudo - Limite_Pago_Inferior)))
                    {
                        Incluido = "NO";
                    }
                }
                Diferencia = Importe - Total_Adeudo;
                Suma_Importes += Importe;
                Contador_Movimientos++;
                // agregar datos a la tabla
                Fila_Pago = Dt_Pagos.NewRow();
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial] = Cuenta_Predial;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial_Id] = Cuenta_Predial_ID;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial_Original] = Cuenta_Predial;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Diferencia] = Diferencia;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Linea_Captura] = Identificador_Pago;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Fecha_Pago] = Fecha_Pago;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Pagado] = Importe;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Pagado_Original] = Importe;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Total_Adeudo] = Total_Adeudo;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Sucursal] = Sucursal.ToString();
                //Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Tipo_Pago] = Tipo_Pago;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Descripcion] = Descripcion;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Incluido] = Incluido;
                Fila_Pago["CONSECUTIVO"] = Contador_Movimientos;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Rezago] = Adeudo_Rezago;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Corriente] = Adeudo_Corriente;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Recargos] = Adeudo_Recargos;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Honorarios] = Adeudo_Honorarios;
                Fila_Pago[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Descuento] = Descuento;
                Dt_Pagos.Rows.Add(Fila_Pago);
            }

            // si no hay pagos en la tabla, mostrar mensaje de error en el formato del archivo
            if (Dt_Pagos.Rows.Count <= 0)
            {
                return "El archivo no tiene el formato correcto";
            }
            // mostrar total de movimientos
            Txt_Cantidad_Movimientos.Text = Contador_Movimientos.ToString("#,##0");

            // datos generales de captura
            DataRow Fila_Carga = Dt_Captura.NewRow();
            Fila_Carga[Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Institucion_Id] = Cmb_Banco.SelectedValue;
            Fila_Carga[Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Fecha_Captura] = DateTime.Now;
            Fila_Carga[Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Nombre_Archivo] = Txt_Nombre_Archivo.Text;
            Fila_Carga[Ope_Pre_Capturas_Pagos_Instituciones_Externas.Campo_Fecha_Corte] = Fecha_Pago;
            Dt_Captura.Rows.Add(Fila_Carga);

            Session["Dt_Pagos"] = Dt_Pagos;
            Session["Dt_Captura"] = Dt_Captura;
            Session["Dt_Adeudos_Totales"] = Dt_Adeudos_Totales;
            Session["Dt_Adeudo_Detallado_Predial"] = Dt_Adeudo_Detallado_Predial;
            Session["Dt_Pasivos_Pago"] = Dt_Pasivos_Pago;
            // cargar grids
            Cargar_Grids_Pagos(true);
        }
        catch (Exception ex)
        {
            throw new Exception("Leer_Archivo_HSBC " + ex.Message.ToString(), ex);
        }
        return Mensaje;
    }

    #endregion METODOS_DATOS_BANCOS

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Quitar_Caracteres_Derecha
    /// DESCRIPCIÓN: Regresa el texto sin el caracter indicado a la derecha
    /// PARÁMETROS:
    /// 		1. Texto: al que se va a quitar el caracter
    /// 		2. Caracter: caracter a remover del texto
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 23-mar-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private string Quitar_Caracteres_Derecha(string Texto, string Caracter)
    {
        // validar que contiene texto y que termine con el caracter indicado
        if (Texto.Length > 0 && Texto.Substring(Texto.Length-1) == Caracter)
        {
            return Quitar_Caracteres_Derecha(Texto.Substring(0, Texto.Length - 1), Caracter);
        }
        else
        {
            return Texto;
        }
    }

    #endregion METODOS

    #region EVENTOS

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Page_Load
    /// DESCRIPCIÓN: Manejador del evento del servidor PageLoad
    /// PARÁMETROS: sender, e
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 16-ene-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        this.Form.Enctype = "multipart/form-data";
        Tsm_Script_Manager.RegisterPostBackControl(Btn_Subir_Archivo);
        //Tsm_Script_Manager.RegisterPostBackControl(Cmb_Banco);
        
        try
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty))
                Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
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
    /// NOMBRE_FUNCIÓN: Btn_Nuevo_Click
    /// DESCRIPCIÓN: Habilita la forma para ingresar datos y permitir guardar un nuevo registro
    ///             en caso de guardar, verifica la validez de los datos ingresados y reporta cualquier
    ///             error
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 16-ene-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        string Mensaje;

        try
        {
            if (Btn_Nuevo.ToolTip == "Nuevo")
            {
                Habilitar_Controles("Nuevo");
                Limpiar_Controles();
                Txt_Fecha.Text = DateTime.Now.ToString("dd-MMM-yyyy");

                Session.Remove("Archivo_Pagos");
            }
            else
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                //Validar_Campos();

                //Si faltaron campos por capturar envía un mensaje al usuario indicando cuáles
                if (Lbl_Mensaje_Error.Text.Length > 0)
                {
                }
                else
                {
                    Mensaje = Alta_Captura_Pagos();
                    if (Mensaje.Length > 0)
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = Mensaje;
                    }
                }
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
    /// NOMBRE_FUNCIÓN: Btn_Salir_Click
    /// DESCRIPCIÓN: Manejo del evento Click para el control Btn_Salir. Ir a la página principal o 
    ///         inicializar controles 
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 16-ene-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        try
        {
            if (Btn_Salir.ToolTip == "Inicio")
            {
                Session.Remove("Dt_Pagos");
                Session.Remove("Dt_Captura");
                Session.Remove("Dt_Instituciones");
                Session.Remove("Dt_Adeudos_Totales");
                Session.Remove("Dt_Adeudo_Detallado_Predial");
                Session.Remove("Dt_Pasivos_Pago");
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else if (Btn_Salir.ToolTip=="Regresar")
            {
                Habilitar_Controles("Inicial");
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
    /// NOMBRE_FUNCIÓN: Btn_Buscar_Click
    /// DESCRIPCIÓN: Manejo del evento Click para el control Btn_Buscar. Llamar al metodo 
    ///             de consulta de capturas
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 21-ene-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        try
        {
            Habilitar_Controles("Inicial");
            Consultar_Capturas();
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Cmb_Banco_SelectedIndexChanged
    /// DESCRIPCIÓN: Manejo del evento cambio de indice en el combo bancos, cambiar el dato de caja 
    ///             (numero e ID de caja), tomando el dato guardado en la variable de sesion Dt_Instituiones
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 16-ene-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Cmb_Banco_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable Dt_Instituciones;

        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        try
        {
            Dt_Instituciones = (DataTable)Session["Dt_Instituciones"];
            // obtener la fila con el id del banco seleccionado
            var Datos_Institucion = (from Fila in Dt_Instituciones.AsEnumerable()
                                     where Fila.Field<string>(Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion_Id) == Cmb_Banco.SelectedValue
                                     select Fila).AsDataView().ToTable();
            // si el resultado no es nulo, recuperar datos de caja (numero y ID)
            if (Datos_Institucion != null && Datos_Institucion.Rows.Count > 0)
            {
                Txt_Caja.Text = Datos_Institucion.Rows[0][Cat_Pre_Cajas.Campo_Clave].ToString();
                Hdn_Caja_Id.Value = Datos_Institucion.Rows[0][Cat_Pre_Cajas.Campo_Caja_Id].ToString();
            }
            else
            {
                Txt_Caja.Text = "";
                Hdn_Caja_Id.Value = "";
            }

            // si ya hay un archivo subido, llamar al metodo para cargar los datos del archivo
            if (Session["Archivo_Pagos"] != null && Txt_Nombre_Archivo.Text != "")
            {
                string Mensaje = Leer_Datos_Archivo();
                // si regreso mensaje de error, mostrarlo
                if (Mensaje.Length > 0)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = Mensaje;
                }
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
    /// NOMBRE_FUNCIÓN: Btn_Subir_Archivo_Click
    /// DESCRIPCIÓN: Manejo del evento clic en el boton subir archivo, valida que se haya recibido un 
    ///             archivo y llama al metodo que lo procesa
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 16-ene-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Subir_Archivo_Click(object sender, EventArgs e)
    {
        String Mensaje = "";
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        try
        {
            Cmb_Banco.Enabled = true;
            // si se envia archivo, validar que se recibe archivo
            if (Btn_Subir_Archivo.ToolTip == "Enviar archivo")
            {
                // validar que se subio archivo
                if (Fup_Archivo_Adeudos.HasFile)
                {
                    // configurar controles para subida de archivo
                    Fup_Archivo_Adeudos.Visible = false;
                    Txt_Nombre_Archivo.Visible = true;
                    Btn_Subir_Archivo.ToolTip = "Volver a subir archivo";

                    Txt_Nombre_Archivo.Text = Fup_Archivo_Adeudos.FileName;
                    Session["Archivo_Pagos"] = new StreamReader(Fup_Archivo_Adeudos.FileContent);
                    // validar que hay un banco seleccionado en el combo bancos
                    if (Cmb_Banco.SelectedIndex > 0)
                    {
                        Mensaje = Leer_Datos_Archivo();
                        // si regreso mensaje de error, mostrarlo
                        if (Mensaje.Length > 0)
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = Mensaje;
                        }
                    }
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Debe seleccionar el archivo a procesar.";
                }
            }
            else // si se va a volver a subir archivo, borrar datos de archivo anterior
            {
                Fup_Archivo_Adeudos.Visible = true;
                Txt_Nombre_Archivo.Visible = false;
                Txt_Nombre_Archivo.Text= "";
                Txt_Cantidad_Movimientos.Text = "";
                Session.Remove("Archivo_Pagos");
                Session.Remove("Dt_Pagos");
                Session.Remove("Dt_Captura");
                Session.Remove("Dt_Adeudos_Totales");
                Session.Remove("Dt_Adeudo_Detallado_Predial");
                Session.Remove("Dt_Pasivos_Pago");
                Btn_Subir_Archivo.ToolTip = "Enviar archivo";
                // limpiar grids pagos y encabezados
                Div_Encabezado_Pagos_Incluidos.InnerText = "Pagos Incluidos";
                Div_Encabezado_Pagos_Excluidos.InnerText = "Pagos Excluidos";
                Grid_Pagos_Incluidos.DataSource = null;
                Grid_Pagos_Incluidos.DataBind();
                Grid_Pagos_Excluidos.DataSource = null;
                Grid_Pagos_Excluidos.DataBind();
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
    /// NOMBRE_FUNCIÓN: Grid_Pagos_Incluidos_PageIndexChanging
    /// DESCRIPCIÓN: Manejo del evento de paginación del grid (cargar los datos de la página seleccionada)
    ///             desde Dt_Pagos en sesion
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 16-ene-2012
    /// MODIFICÓ: 
    ///	FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Pagos_Incluidos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        try
        {
            Grid_Pagos_Incluidos.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
            Cargar_Grids_Pagos(Grid_Pagos_Incluidos.Rows[0].Cells[0].Visible);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Grid_Pagos_Excluidos_PageIndexChanging
    /// DESCRIPCIÓN: Manejo del evento de paginación del grid (cargar los datos de la página seleccionada)
    ///             desde Dt_Pagos en sesion
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 16-ene-2012
    /// MODIFICÓ: 
    ///	FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Pagos_Excluidos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        try
        {
            Grid_Pagos_Excluidos.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
            Cargar_Grids_Pagos(Grid_Pagos_Excluidos.Rows[0].Cells[0].Visible);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Grid_Capturas_PageIndexChanging
    /// DESCRIPCIÓN: Manejo del evento de paginación del grid (cargar los datos de la página seleccionada)
    ///             desde Dt_Pagos en sesion
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 21-ene-2012
    /// MODIFICÓ: 
    ///	FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Capturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        try
        {
            Grid_Capturas.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
            Consultar_Capturas();
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Grid_Capturas_SelectedIndexChanged
    /// DESCRIPCIÓN: Manejo del evento de cambio de indice del grid: cargar detalles de la captura seleccionada
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 21-ene-2012
    /// MODIFICÓ: 
    ///	FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Capturas_SelectedIndexChanged(object sender, EventArgs e)
    {
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        try
        {
            if (Grid_Capturas.SelectedIndex >= 0)
            {
                Habilitar_Controles("Consulta");
                Cargar_Detalles_Captura(Grid_Capturas.SelectedRow.Cells[1].Text);
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
    /// 	NOMBRE_FUNCIÓN: Grid_Pagos_Incluidos_RowDataBound
    /// 	DESCRIPCIÓN: Manejo del evento RowDataBound en el grid pagos incluidos
    /// 	            al boton en cada fila agregar como argumento (propiedad CommandArgument del boton) 
    /// 	            el campo consecutivo de pagos
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 18-ene-2012
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Pagos_Incluidos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton Btn_Excluir_Pago = (ImageButton)e.Row.FindControl("Btn_Excluir_Pago");
                DataRowView Dr_Fila_Pago = (DataRowView)e.Row.DataItem;
                Btn_Excluir_Pago.CommandArgument = Dr_Fila_Pago["CONSECUTIVO"].ToString();
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
    /// 	NOMBRE_FUNCIÓN: Grid_Pagos_Excluidos_RowDataBound
    /// 	DESCRIPCIÓN: Manejo del evento RowDataBound en el grid pagos excluidos
    /// 	            al boton en cada fila agregar como argumento (propiedad CommandArgument del boton) 
    /// 	            el campo consecutivo de pagos para poder identificar la fila
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 18-ene-2012
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Pagos_Excluidos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton Btn_Incluir_Pago = (ImageButton)e.Row.FindControl("Btn_Incluir_Pago");
                var Btn_Aplicar_Cambios_Pago_Excluido = (ImageButton)e.Row.FindControl("Btn_Aplicar_Cambios_Pago_Excluido");
                var Btn_Deshacer_Cambios_Pago_Excluido = (ImageButton)e.Row.FindControl("Btn_Deshacer_Cambios_Pago_Excluido");

                DataRowView Dr_Fila_Pago = (DataRowView)e.Row.DataItem;
                Btn_Aplicar_Cambios_Pago_Excluido.CommandArgument = Dr_Fila_Pago["CONSECUTIVO"].ToString();
                Btn_Deshacer_Cambios_Pago_Excluido.CommandArgument = Dr_Fila_Pago["CONSECUTIVO"].ToString();
                Btn_Incluir_Pago.CommandArgument = Dr_Fila_Pago["CONSECUTIVO"].ToString();
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
    /// 	NOMBRE_FUNCIÓN: Btn_Excluir_Pago_Click
    /// 	DESCRIPCIÓN: Manejo del evento Click en el boton excluir pago, 
    /// 	            Cambiar el pago seleccionado a INCLUIDO="NO", se realiza el cambio en 
    /// 	            Dt_Pagos de sesion y una vez actualizado, se vuelven a cargar los grids
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 18-ene-2012
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Excluir_Pago_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Dt_Pagos;
        ImageButton Btn_Excluir_Pago = (ImageButton)sender;

        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        try
        {
            if (Session["Dt_Pagos"] != null)    //si hay datos en la tabla de documentos
            {
                Dt_Pagos = (DataTable)Session["Dt_Pagos"];
                foreach (DataRow Fila in Dt_Pagos.Rows)
                {
                    if (Btn_Excluir_Pago.CommandArgument == Fila["CONSECUTIVO"].ToString())  // encontrar fila por el consecutivo
                    {
                        Fila["INCLUIDO"] = "NO"; // actualizar Fila de la tabla
                        //Dt_Pagos.AcceptChanges();
                        break;
                    }
                }
                Session["Dt_Pagos"] = Dt_Pagos;     // guardar tabla
            }

            Cargar_Grids_Pagos(true);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Btn_Incluir_Pago_Click
    /// 	DESCRIPCIÓN: Manejo del evento Click en el boton incluir pago, 
    /// 	            Cambiar el pago seleccionado a INCLUIDO="SI", se realiza el cambio en 
    /// 	            Dt_Pagos de sesion y una vez actualizado, se vuelven a cargar los grids
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 18-ene-2012
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Incluir_Pago_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Dt_Pagos;
        ImageButton Btn_Incluir_Pago = (ImageButton)sender;
        string Mensaje_Error = "";

        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        try
        {
            if (Session["Dt_Pagos"] != null)    //si hay datos en la tabla de documentos
            {
                Dt_Pagos = (DataTable)Session["Dt_Pagos"];
                foreach (DataRow Fila in Dt_Pagos.Rows)
                {
                    if (Btn_Incluir_Pago.CommandArgument == Fila["CONSECUTIVO"].ToString())  // encontrar fila por el consecutivo
                    {
                        // validar que el pago tenga una cuenta predial
                        if (Fila[Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString().Trim() != "")
                        {
                            Fila["INCLUIDO"] = "SI"; // actualizar Fila de la tabla
                        }
                        else
                        {
                            Mensaje_Error = "Es necesario especificar una cuenta predial para el pago";
                        }
                        break;
                    }
                }
                Session["Dt_Pagos"] = Dt_Pagos;     // guardar tabla
            }

            Cargar_Grids_Pagos(true);

            // mostrar mensaje
            if (Mensaje_Error != "")
            {
                Lbl_Mensaje_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Mensaje_Error;
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
    /// 	NOMBRE_FUNCIÓN: Btn_Editar_Pago_Excluido_Click
    /// 	DESCRIPCIÓN: Manejo del evento Click en el boton editar pago excluido, 
    /// 	            llamar al metodo que localiza y establece la visibilidad de los 
    /// 	            controles en el grid de pagos excluidos
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 18-ene-2012
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Editar_Pago_Excluido_Click(object sender, ImageClickEventArgs e)
    {
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        try
        {
            Configurar_Visibilidad_Controles_Grid_Pagos_Excluidos(sender, true);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Btn_Aplicar_Cambios_Pago_Excluido_Click
    /// 	DESCRIPCIÓN: Manejo del evento Click en el boton aplicar cambios pago excluido, 
    /// 	            aplicar cambios a la tabla de pagos y volver a configurar visibilidad 
    /// 	            de controles y botones en el grid de pagos excluidos
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 18-ene-2012
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Aplicar_Cambios_Pago_Excluido_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow Fila_Grid;
        ImageButton Btn_Original = (ImageButton)sender;
        Label Lbl_Grid_Monto_Pagado;
        TextBox Txt_Grid_Monto_Pagado;
        Label Lbl_Grid_Cuenta_Predial;
        TextBox Txt_Grid_Cuenta_Predial;
        DataTable Dt_Pagos;
        Decimal Monto_Pagado;
        String Cuenta_Predial_ID;
        string Mes_Movimientos;
        string Tipo_Predio_ID;

        // tablas para el pago
        DataTable Dt_Adeudo_Detallado_Predial = Crear_Tabla_Adeudos_Predial();
        DataTable Dt_Adeudos_Totales = Crear_Tabla_Adeudos_Totales();
        DataTable Dt_Pasivos_Pago = Crear_Tabla_Pasivos_Pago();
        DataTable Dt_Adeudos;
        
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        try
        {
            Dt_Pagos = (DataTable)Session["Dt_Pagos"];
            // tratar de recuperar tablas para aplicar pagos
            if (Session["Dt_Adeudos_Totales"] != null)
            {
                Dt_Adeudos_Totales = (DataTable)Session["Dt_Adeudos_Totales"];
            }
            if (Session["Dt_Adeudo_Detallado_Predial"] != null)
            {
                Dt_Adeudo_Detallado_Predial = (DataTable)Session["Dt_Adeudo_Detallado_Predial"];
            }
            if (Session["Dt_Pasivos_Pago"] != null)
            {
                Dt_Pasivos_Pago = (DataTable)Session["Dt_Pasivos_Pago"];
            }

            // verificar que el objeto origen recibido no sea nulo
            if (Btn_Original != null && Dt_Pagos != null)
            {
                // ubicar el grid a partir del objeto que disparo el evento y validar que no sea nulo
                Fila_Grid = (GridViewRow)FindControl(Btn_Original.Parent.Parent.UniqueID);
                if (Fila_Grid != null)
                {
                    Lbl_Grid_Monto_Pagado = (Label)Fila_Grid.Cells[4].FindControl("Lbl_Grid_Monto_Pagado");
                    Txt_Grid_Monto_Pagado = (TextBox)Fila_Grid.Cells[4].FindControl("Txt_Grid_Monto_Pagado");
                    Lbl_Grid_Cuenta_Predial = (Label)Fila_Grid.Cells[9].FindControl("Lbl_Grid_Cuenta_Predial");
                    Txt_Grid_Cuenta_Predial = (TextBox)Fila_Grid.Cells[9].FindControl("Txt_Grid_Cuenta_Predial");
                    // verificar que ninguno de los controles sea nulo
                    if (Lbl_Grid_Monto_Pagado != null && Txt_Grid_Monto_Pagado != null && Lbl_Grid_Cuenta_Predial != null && Txt_Grid_Cuenta_Predial != null)
                    {
                        // recorrer la tabla hasta encontrar la fila a cambiar
                        foreach (DataRow Fila in Dt_Pagos.Rows)
                        {
                            if (Btn_Original.CommandArgument == Fila["CONSECUTIVO"].ToString())  // encontrar fila por el consecutivo
                            {
                                // validar que el campo cuenta predial no este vacio, si esta vacio, mostrar mensaje y abandonar evento
                                if (Txt_Grid_Cuenta_Predial.Text.Length > 0)
                                {
                                    Cuenta_Predial_ID = Consultar_Cuenta_Predial_ID(Txt_Grid_Cuenta_Predial.Text, out Tipo_Predio_ID);
                                    string Propietario = Consultar_Propietario(Cuenta_Predial_ID, null);
                                    if (Cuenta_Predial_ID != "")
                                    {
                                        // actualizar adeudos
                                        decimal Adeudo_Corriente = 0;
                                        decimal Adeudo_Rezago = 0;
                                        decimal Adeudo_Recargos = 0;
                                        decimal Adeudo_Honorarios = 0;
                                        decimal Total_Adeudo = 0;
                                        string Periodo_Corriente;
                                        string Periodo_Rezago;
                                        DateTime Fecha_Pago;
                                        var Dicc_Porciento_Descuento = new Dictionary<string, decimal>();
                                        Dictionary<string, string> Dicc_Tipos_Predio;
                                        Dictionary<string, string> Dic_Claves_Ingreso;
                                        Dictionary<string, string> Dic_Clave_Dependencia;
                                        decimal Descuento = 0;
                                        decimal Cuota_Minima = 0;

                                        Dicc_Tipos_Predio = Consultar_Tipos_Predio();
                                        Dic_Claves_Ingreso = Obtener_Diccionario_Claves_Ingreso(out Dic_Clave_Dependencia);

                                        // obtener cuota minima
                                        Cuota_Minima = Obtener_Cuota_Minima(DateTime.Now.Year);
                                        DateTime.TryParse(Fila[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Fecha_Pago].ToString(), out Fecha_Pago);

                                        // agregar porcentaje de descuento para el mes de los movimientos al diccionario de descuentos
                                        Mes_Movimientos = Fecha_Pago.ToString("MMMM").ToUpper();
                                        if (!Dicc_Porciento_Descuento.ContainsKey(Mes_Movimientos))
                                        {
                                            Dicc_Porciento_Descuento.Add(Mes_Movimientos, Obtener_Descuentos_Pronto_Pago(Mes_Movimientos, Fecha_Pago.Year));
                                        }
                                        
                                        Dt_Adeudos = Consultar_Adeudos_Cuenta_Predial(
                                            Cuenta_Predial_ID,
                                            Fecha_Pago,
                                            out Total_Adeudo,
                                            out Adeudo_Corriente,
                                            out Adeudo_Rezago,
                                            out Adeudo_Honorarios,
                                            out Adeudo_Recargos,
                                            out Periodo_Corriente,
                                            out Periodo_Rezago);

                                        // calcular descuento
                                        string Mes_Pago = Fecha_Pago.ToString("MMMM").ToUpper();
                                        if (Dicc_Porciento_Descuento.ContainsKey(Mes_Pago))
                                        {
                                            Descuento = Adeudo_Corriente * Dicc_Porciento_Descuento[Mes_Pago] / 100;
                                        }

                                        // validar que el descuento restado al adeudo corriente no sea menor que la cuota mínima
                                        if (Cuota_Minima > 0 && Adeudo_Corriente - Descuento < Cuota_Minima)
                                        {
                                            Descuento = Adeudo_Corriente - Cuota_Minima;
                                        }
                                        // validar descuentos negativos
                                        if (Descuento < 0)
                                        {
                                            Descuento = 0;
                                        }
                                        // si ya hay registro con el id de la cuenta predial en las tablas de aplicación de pago, eliminar antes de agregar nuevo registro
                                        for (int Registro_Adeudo = Dt_Adeudo_Detallado_Predial.Rows.Count - 1; Registro_Adeudo >= 0; Registro_Adeudo--)
                                        {
                                            if (Dt_Adeudo_Detallado_Predial.Rows[Registro_Adeudo]["NO_CUENTA"].ToString() == Cuenta_Predial_ID)
                                            {
                                                Dt_Adeudo_Detallado_Predial.Rows[Registro_Adeudo].Delete();
                                            }
                                        }
                                        Dt_Adeudo_Detallado_Predial.AcceptChanges();
                                        for (int Registro_Adeudo = Dt_Adeudos_Totales.Rows.Count - 1; Registro_Adeudo >= 0; Registro_Adeudo--)
                                        {
                                            if (Dt_Adeudos_Totales.Rows[Registro_Adeudo]["NO_CUENTA"].ToString() == Cuenta_Predial_ID)
                                            {
                                                Dt_Adeudos_Totales.Rows[Registro_Adeudo].Delete();
                                            }
                                        }
                                        Dt_Adeudos_Totales.AcceptChanges();
                                        for (int Registro_Adeudo = Dt_Pasivos_Pago.Rows.Count - 1; Registro_Adeudo >= 0; Registro_Adeudo--)
                                        {
                                            if (Dt_Pasivos_Pago.Rows[Registro_Adeudo]["Cuenta_Predial_Id"].ToString() == Cuenta_Predial_ID)
                                            {
                                                Dt_Pasivos_Pago.Rows[Registro_Adeudo].Delete();
                                            }
                                        }
                                        Dt_Pasivos_Pago.AcceptChanges();

                                        // agregar datos a las tablas para aplicar el pago
                                        Agregar_Adeudos_Predial(Dt_Adeudos, Dt_Adeudo_Detallado_Predial, Cuenta_Predial_ID);
                                        Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "CORRIENTE", Adeudo_Corriente, "");
                                        Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "REZAGO", Adeudo_Rezago, "");
                                        Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "RECARGOS", Adeudo_Recargos, "");
                                        Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "HONORARIOS", Adeudo_Honorarios, "");
                                        Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "DESCUENTOS_CORRIENTES", Descuento, "");
                                        // agregar a la tabla pasivos
                                        if (Dicc_Tipos_Predio.ContainsKey(Tipo_Predio_ID))
                                        {
                                            Agregar_Pasivos_Adeudo(
                                                Dt_Pasivos_Pago,
                                                Adeudo_Corriente,
                                                Adeudo_Rezago,
                                                Adeudo_Recargos,
                                                Adeudo_Honorarios,
                                                Descuento,
                                                Txt_Grid_Cuenta_Predial.Text,
                                                Cuenta_Predial_ID,
                                                Dicc_Tipos_Predio[Tipo_Predio_ID],
                                                Periodo_Corriente,
                                                Periodo_Rezago,
                                                Fecha_Pago,
                                                Propietario,
                                                Dic_Claves_Ingreso,
                                                Dic_Clave_Dependencia);
                                        }

                                        // Recalcular total adeudo
                                        Total_Adeudo = Adeudo_Rezago + Adeudo_Corriente - Descuento + Adeudo_Recargos + Adeudo_Honorarios;

                                        Fila[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Total_Adeudo] = Total_Adeudo;
                                        Fila[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Rezago] = Adeudo_Rezago;
                                        Fila[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Corriente] = Adeudo_Corriente;
                                        Fila[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Recargos] = Adeudo_Recargos;
                                        Fila[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Adeudo_Honorarios] = Adeudo_Honorarios;
                                        Fila[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Descuento] = Descuento;
                                        Fila[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial] = Txt_Grid_Cuenta_Predial.Text;
                                        Fila[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Cuenta_Predial_Id] = Cuenta_Predial_ID;
                                        // actualizar adeudos de la cuenta
                                        Dt_Pagos.AcceptChanges();
                                    }
                                    else
                                    {
                                        Img_Error.Visible = true;
                                        Lbl_Mensaje_Error.Visible = true;
                                        Lbl_Mensaje_Error.Text = "La cuenta predial ingresada no se encuentra en el sistema.";

                                        return;
                                    }
                                }
                                else
                                {
                                    Img_Error.Visible = true;
                                    Lbl_Mensaje_Error.Visible = true;
                                    Lbl_Mensaje_Error.Text = "Debe ingresar la cuenta predial.";

                                    return;
                                }
                                // si el monto pagado se puede convertir a decimal, actualizar el valor, si no, mostrar mensaje y abandonar evento
                                if (Decimal.TryParse(Txt_Grid_Monto_Pagado.Text.Replace("$", ""), out Monto_Pagado))
                                {
                                    decimal Adeudo;
                                    decimal.TryParse(Fila[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Total_Adeudo].ToString(), out Adeudo);
                                    Fila[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Monto_Pagado] = Monto_Pagado;
                                    Fila[Ope_Pre_Detalles_Captura_Pagos_Instit_Exter.Campo_Diferencia] = Monto_Pagado - Adeudo;
                                    Dt_Adeudos_Totales.Rows.Add(Cuenta_Predial_ID, "MONTO_TOTAL_PAGADO", Monto_Pagado, "");
                                    Dt_Pagos.AcceptChanges();
                                }
                                else
                                {
                                    Img_Error.Visible = true;
                                    Lbl_Mensaje_Error.Visible = true;
                                    Lbl_Mensaje_Error.Text = "El monto ingresado no es correcto.";

                                    return;
                                }
                                break;
                            }
                        }
                        Session["Dt_Pagos"] = Dt_Pagos;     // guardar tabla
                        Session["Dt_Adeudos_Totales"] = Dt_Adeudos_Totales;
                        Session["Dt_Adeudo_Detallado_Predial"] = Dt_Adeudo_Detallado_Predial;
                        Session["Dt_Pasivos_Pago"] = Dt_Pasivos_Pago;
                    }
                }
            }

            Configurar_Visibilidad_Controles_Grid_Pagos_Excluidos(sender, false);
            Cargar_Grids_Pagos(true);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Btn_Deshacer_Cambios_Pago_Excluido_Click
    /// 	DESCRIPCIÓN: Manejo del evento Click en el boton deshacer cambios pago excluido, 
    /// 	            recuperar el valor anterior en el control y volver a configurar visibilidad 
    /// 	            de controles y botones en el grid de pagos excluidos
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 18-ene-2012
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Deshacer_Cambios_Pago_Excluido_Click(object sender, ImageClickEventArgs e)
    {
        
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        try
        {
            Configurar_Visibilidad_Controles_Grid_Pagos_Excluidos(sender, false);
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
    /// DESCRIPCIÓN: Manejo del evento Click para el control Btn_Imprimir que genera el reporte en pdf
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 25-ene-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        try
        {
            Exportar_Reporte(Crear_Ds_Exportar_Reporte(), "Rpt_Pre_Pagos_Instituciones_Externas.rpt", "Pagos_Instituciones_Externas", "pdf", ExportFormatType.PortableDocFormat);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Exportar convenio a pdf: " + Ex.Message;
            Img_Error.Visible = true;
        }
    }

    #endregion EVENTOS
}
