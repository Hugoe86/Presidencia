using System;
using System.Collections.Generic;
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
using Presidencia.Sessiones;
using Presidencia.Operacion_Cat_Avaluo_Urbano_Av.Negocio;
using Presidencia.Catalogo_Cat_Motivos_Avaluo.Negocio;
using Presidencia.Constantes;
using Presidencia.Operacion_Resumen_Predio.Negocio;
using Operacion_Predial_Orden_Variacion.Negocio;
using Presidencia.Catalogo_Cat_Identificadores_Predio.Negocio;
using Presidencia.Catalogo_Cat_Valores_Inpa.Negocio;
using Presidencia.Catalogo_Cat_Valores_Inpr.Negocio;
using Presidencia.Catalogo_Cat_Peritos_Internos.Negocio;
using Presidencia.Catalogo_Cat_Motivos_Rechazo.Negocio;
using Presidencia.Catalogo_Cat_Parametros.Negocio;
using Presidencia.Catalogo_Cat_Tasas.Negocio;
using System.IO;
using Presidencia.Catalogo_Cat_Peritos_Externos.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Catalogo_Cuotas_Minimas.Negocio;

public partial class paginas_Catastro_Frm_Ope_Cat_Seguimiento_Avaluos_Urbanos_Av : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Cls_Cat_Cat_Peritos_Internos_Negocio Perito_Interno = new Cls_Cat_Cat_Peritos_Internos_Negocio();
                Perito_Interno.P_Empleado_Id = Cls_Sessiones.Empleado_ID;
                DataTable Dt_Perito_Interno = Perito_Interno.Consultar_Peritos_Internos();
                if (Dt_Perito_Interno.Rows.Count > 0)
                {
                    Hdf_Perito_Interno_Id.Value = Dt_Perito_Interno.Rows[0][Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id].ToString();
                }
                Session["Activa"] = true;//Variable para mantener la session activa.
                Configuracion_Formulario(true);
                Llenar_Tabla_Avaluos_Urbanos(0);
                Llenar_Combo_Motivos_Avaluo();
                Llenar_Combo_Motivos_Rechazo();
                Llenar_Combo_Firmante();
                Session.Remove("ESTATUS_CUENTAS");
                Session.Remove("TIPO_CONTRIBUYENTE");
                Session["ESTATUS_CUENTAS"] = "IN ('PENDIENTE','ACTIVA','VIGENTE','BLOQUEADA','SUSPENDIDA','CANCELADA')";
                String Ventana_Modal1 = "Abrir_Ventana_Modal('../Predial/Ventanas_Emergentes/Frm_Busqueda_Avanzada_Cuentas_Predial.aspx', 'center:yes;resizable:no;status:no;dialogWidth:700px;dialogHeight:420px;dialogHide:true;help:no;scroll:no');";
                //Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial.Attributes.Add("onclick", Ventana_Modal1);
                Txt_Observaciones.Attributes.Add("OnKeyPress", " ValidarCaracteres(this,250)");
                Txt_Observaciones.Attributes.Add("OnKeyUp", " ValidarCaracteres(this,250)");
                Txt_Observaciones.Attributes.Add("OnKeyPress", " ValidarCaracteres(this,250)");
                Txt_Observaciones.Attributes.Add("OnKeyUp", " ValidarCaracteres(this,250)");

                Txt_Uso.Attributes.Add("OnKeyPress", " ValidarCaracteres(this,250)");
                Txt_Uso.Attributes.Add("OnKeyUp", " ValidarCaracteres(this,250)");
                Txt_Uso.Attributes.Add("OnKeyPress", " ValidarCaracteres(this,250)");
                Txt_Uso.Attributes.Add("OnKeyUp", " ValidarCaracteres(this,250)");
                Limpiar_Formulario();
            }
        }
        catch (Exception ex)
        {
            Lbl_Ecabezado_Mensaje.Text = ex.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
        Div_Contenedor_Msj_Error.Visible = false;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Establece la configuración del formulario
    ///PROPIEDADES:     Enabled: Especifica si estara habilitado o no el componente
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean Enabled)
    {
        Cmb_Motivo_Avaluo.Enabled = false;
        Txt_Uso.Enabled = false;
        Txt_No_Avaluo.Enabled = false;
        Txt_Ubicacion_Predio.Enabled = false;
        Txt_Colonia.Enabled = false;
        Txt_Localidad.Enabled = false;
        Txt_Municipio.Enabled = false;
        Txt_Propietario.Enabled = false;
        Txt_Domicilio_Not.Enabled = false;
        Txt_Colonia_Not.Enabled = false;
        Txt_Localidad_Not.Enabled = false;
        Txt_Municipio_Not.Enabled = false;
        Txt_Cuenta_Predial.Enabled = false;
        //Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial.Enabled = false;
        Txt_Region.Enabled = false;
        Txt_Manzana.Enabled = false;
        Txt_Lote.Enabled = false;
        Txt_Solicitante.Enabled = false;
        Txt_Dens_Construccion.Enabled = false;
        Txt_Observaciones.Enabled = false;
        Txt_Terreno_Superficie_Total.Enabled = false;
        Txt_Terreno_Valor_Total.Enabled = false;
        Txt_Construccion_Superficie_Total.Enabled = false;
        Txt_Construccion_Valor_Total.Enabled = false;
        Txt_Valor_Total_Predio.Enabled = false;
        Cmb_Estatus.Enabled = false;
        Grid_Elementos_Construccion.Enabled = false;
        Grid_Calculos.Enabled = false;
        Grid_Valores_Construccion.Enabled = false;
        Grid_Clasificacion_Zona.Enabled = false;
        Grid_Construccion_Dominante.Enabled = false;
        Grid_Servicios_Zona.Enabled = false;
        Cmb_Motivos_Rechazo.Enabled = !Enabled;
        Grid_Observaciones.Enabled = !Enabled;
        Btn_Agregar_Observacion.Enabled = !Enabled;
        Btn_Eliminar_Observacion.Enabled = !Enabled;
        Grid_Avaluos_Urbanos.Enabled = Enabled;
        Txt_Busqueda.Enabled = Enabled;
        Btn_Buscar.Enabled = Enabled;
        Rdb_Ampliacion.Enabled = false;
        Rdb_Buenas.Enabled = false;
        Rdb_Malas.Enabled = false;
        Rdb_Nueva.Enabled = false;
        Rdb_Pendiente.Enabled = false;
        Rdb_Plana.Enabled = false;
        Rdb_Calidad_Buena.Enabled = false;
        Rdb_Calidad_Mala.Enabled = false;
        Rdb_Calidad_Regular.Enabled = false;
        Rdb_Regulares.Enabled = false;
        Rdb_Remodelacion.Enabled = false;
        Rdb_Rentada.Enabled = false;
        Txt_Construccion_Valor_Total.Style["text-align"] = "Right";
        Txt_Construccion_Superficie_Total.Style["text-align"] = "Right";
        Txt_Terreno_Superficie_Total.Style["text-align"] = "Right";
        Txt_Terreno_Valor_Total.Style["text-align"] = "Right";
        Txt_Dens_Construccion.Style["text-align"] = "Right";
        Txt_Valor_Total_Predio.Style["text-align"] = "Right";
        Txt_Cuota_Anual_Actual.Style["text-align"] = "Right";
        Txt_Valor_Total_Predio_Anterior.Style["text-align"] = "Right";
        Txt_Cuota_Anual_Anterior.Style["text-align"] = "Right";
        Txt_Diferencia_Anual.Style["text-align"] = "Right";
        Chk_Fotogrametria.Enabled = false;
    }

    private void Crear_Mascara(Int16 Cantidad_Decimales)
    {
        Mascara_Caracteres = "";
        if (Cantidad_Decimales > 0)
        {
            for (int i = 0; i < Cantidad_Decimales; i++)
            {
                Mascara_Caracteres += "0";
            }
        }
    }

    String Mascara_Caracteres;

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Establece la configuración del formulario
    ///PROPIEDADES:     Enabled: Especifica si estara habilitado o no el componente
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Formulario()
    {
        Cmb_Motivo_Avaluo.SelectedIndex = 0;
        Txt_No_Avaluo.Text = "";
        Txt_Ubicacion_Predio.Text = "";
        Txt_Colonia.Text = "";
        Txt_Localidad.Text = "";
        Txt_Municipio.Text = "";
        Txt_Propietario.Text = "";
        Txt_Domicilio_Not.Text = "";
        Txt_Colonia_Not.Text = "";
        Txt_Localidad_Not.Text = "";
        Txt_Municipio_Not.Text = "";
        Txt_Cuenta_Predial.Text = "";
        Txt_Region.Text = "";
        Txt_Manzana.Text = "";
        Txt_Lote.Text = "";
        Txt_Solicitante.Text = "";
        Txt_Dens_Construccion.Text = "0.00";
        Txt_Observaciones.Text = "";
        Txt_Terreno_Superficie_Total.Text = "0.00";
        Txt_Terreno_Valor_Total.Text = "0.00";
        Txt_Construccion_Superficie_Total.Text = "0.00";
        Txt_Construccion_Valor_Total.Text = "0.00";
        Txt_Valor_Total_Predio.Text = "0.00";
        Cmb_Estatus.SelectedIndex = 0;
        Txt_Busqueda.Text = "";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Calidad
    ///DESCRIPCIÓN: Llena la tabla de los datos de calidad
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 07/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Avaluos_Urbanos(int Pagina)
    {
        try
        {
            Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio Avaluo_Urb = new Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio();
            if (Txt_Busqueda.Text.Trim() != "")
            {
                Avaluo_Urb.P_Folio = Txt_Busqueda.Text.Trim();
            }
            Avaluo_Urb.P_Estatus = "='POR VALIDAR'";
            Avaluo_Urb.P_Permitir_Revision = "SI";
            Grid_Avaluos_Urbanos.Columns[1].Visible = true;
            Grid_Avaluos_Urbanos.Columns[2].Visible = true;
            Grid_Avaluos_Urbanos.DataSource = Avaluo_Urb.Consultar_Avaluo_Urbano_Av();
            Grid_Avaluos_Urbanos.PageIndex = Pagina;
            Grid_Avaluos_Urbanos.DataBind();
            Grid_Avaluos_Urbanos.Columns[1].Visible = false;
            Grid_Avaluos_Urbanos.Columns[2].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Motivos_Avaluo
    ///DESCRIPCIÓN: Llena la el combo con los datos de los motivos de avalúo
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 07/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Motivos_Avaluo()
    {
        try
        {
            DataTable Dt_Motivos_Avaluo;
            DataRow Dr_Renglon_Nuevo;
            Cls_Cat_Cat_Motivos_Avaluo_Negocio Motivos = new Cls_Cat_Cat_Motivos_Avaluo_Negocio();
            Dt_Motivos_Avaluo = Motivos.Consultar_Motivos_Avaluo();
            Dr_Renglon_Nuevo = Dt_Motivos_Avaluo.NewRow();
            Dr_Renglon_Nuevo[Cat_Cat_Motivos_Avaluo.Campo_Motivo_Avaluo_Id] = "SELECCIONE";
            Dr_Renglon_Nuevo[Cat_Cat_Motivos_Avaluo.Campo_Motivo_Avaluo_Descripcion] = "<SELECCIONE>";
            Dt_Motivos_Avaluo.Rows.InsertAt(Dr_Renglon_Nuevo, 0);
            Cmb_Motivo_Avaluo.DataSource = Dt_Motivos_Avaluo;
            Cmb_Motivo_Avaluo.DataTextField = Cat_Cat_Motivos_Avaluo.Campo_Motivo_Avaluo_Descripcion;
            Cmb_Motivo_Avaluo.DataValueField = Cat_Cat_Motivos_Avaluo.Campo_Motivo_Avaluo_Id;
            Cmb_Motivo_Avaluo.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Motivos_Rechazo
    ///DESCRIPCIÓN: Llena la el combo con los datos de los motivos de rechazo
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 07/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Motivos_Rechazo()
    {
        try
        {
            DataTable Dt_Motivos_Rechazo;
            DataRow Dr_Renglon_Nuevo;
            Cls_Cat_Cat_Motivos_Rechazo_Negocio Motivos = new Cls_Cat_Cat_Motivos_Rechazo_Negocio();
            Dt_Motivos_Rechazo = Motivos.Consultar_Motivos_Rechazo();
            Dr_Renglon_Nuevo = Dt_Motivos_Rechazo.NewRow();
            Dr_Renglon_Nuevo[Cat_Cat_Motivos_Rechazo.Campo_Motivo_Id] = "SELECCIONE";
            Dr_Renglon_Nuevo[Cat_Cat_Motivos_Rechazo.Campo_Motivo_Descripcion] = "<SELECCIONE>";
            Dt_Motivos_Rechazo.Rows.InsertAt(Dr_Renglon_Nuevo, 0);
            Cmb_Motivos_Rechazo.DataSource = Dt_Motivos_Rechazo;
            Cmb_Motivos_Rechazo.DataTextField = Cat_Cat_Motivos_Rechazo.Campo_Motivo_Descripcion;
            Cmb_Motivos_Rechazo.DataValueField = Cat_Cat_Motivos_Rechazo.Campo_Motivo_Id;
            Cmb_Motivos_Rechazo.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Avaluos_Urbanos_PageIndexChanging
    ///DESCRIPCIÓN: Cambia la página del grid
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Avaluos_Urbanos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Llenar_Tabla_Avaluos_Urbanos(e.NewPageIndex);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Avaluos_Urbanos_SelectedIndexChanged
    ///DESCRIPCIÓN: Selecciona un registro del grid y toma los datos de los mismos campos del componente
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Avaluos_Urbanos_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Avaluos_Urbanos.SelectedIndex > -1)
        {
            Cls_Cat_Cat_Parametros_Negocio Par = new Cls_Cat_Cat_Parametros_Negocio();
            Int16 columnas = Convert.ToInt16(Par.Consultar_Parametros().Rows[0][Cat_Cat_Parametros.Campo_Columnas_Calc_Construccion].ToString());
            DataTable Dt_Avaluo;
            Hdf_Anio_Avaluo.Value = Grid_Avaluos_Urbanos.SelectedRow.Cells[2].Text;
            Hdf_No_Avaluo.Value = Grid_Avaluos_Urbanos.SelectedRow.Cells[1].Text;
            Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio Aval_Urb = new Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio();
            Aval_Urb.P_No_Avaluo = Hdf_No_Avaluo.Value;
            Aval_Urb.P_Anio_Avaluo = Hdf_Anio_Avaluo.Value;
            Session["Dt_Tabla_Valores_Construccion"] = Aval_Urb.Consultar_Tabla_Valores_Construccion_Av();
            Dt_Avaluo = Aval_Urb.Consultar_Avaluo_Urbano_Av();
            Cargar_Datos_Avaluo(Dt_Avaluo);
            Cargar_Caracteristicas_Construccion(Aval_Urb.P_Dt_Caracteristicas_Terreno_Av);
            Cargar_Construccion(Aval_Urb.P_Dt_Construccion_Av);
            Crear_Tabla_Clasificacion_Zona(Aval_Urb.P_Dt_Clasificacion_Zona_Av);
            Crear_Tabla_Construccion_Dominante(Aval_Urb.P_Dt_Construccion_Dominante_Av);
            Crear_Tabla_Servicios_Zona(Aval_Urb.P_Dt_Servicios_Zona_Av);
            Session["Dt_Grid_Calculos"] = Aval_Urb.P_Dt_Calculo_Valor_Terreno_Av.Copy();
            Grid_Calculos.Columns[3].Visible = true;
            Grid_Calculos.DataSource = Aval_Urb.P_Dt_Calculo_Valor_Terreno_Av;
            Grid_Calculos.PageIndex = 0;
            Grid_Calculos.DataBind();
            Grid_Calculos.Columns[3].Visible = false;
            columnas = Convert.ToInt16(Txt_Renglones_Col.Text);
            Session["Dt_Grid_Elementos_Construccion"] = Aval_Urb.P_Dt_Elementos_Construccion_Av.Copy();
            Grid_Elementos_Construccion.Columns[0].Visible = true;
            for (Int16 i = 1; i < (columnas + 1); i++)
            {
                Grid_Elementos_Construccion.Columns[i + 1].Visible = true;
            }
            Grid_Elementos_Construccion.DataSource = Aval_Urb.P_Dt_Elementos_Construccion_Av;
            Grid_Elementos_Construccion.PageIndex = 0;
            Grid_Elementos_Construccion.DataBind();
            Grid_Elementos_Construccion.Columns[0].Visible = false;
            for (int i = (columnas + 1); i < 16; i++)
            {
                Grid_Elementos_Construccion.Columns[i + 1].Visible = false;
            }
            Session["Dt_Grid_Valores_Construccion"] = Aval_Urb.P_Dt_Calculo_Valor_Construccion_Av.Copy();
            Grid_Valores_Construccion.DataSource = Aval_Urb.P_Dt_Calculo_Valor_Construccion_Av;
            Grid_Valores_Construccion.PageIndex = 0;
            Grid_Valores_Construccion.DataBind();
            //Cargar los demás grids con las tablas que trae el objeto Aval_Urb.
            //Fin de cargar datos del avalúo
            Div_Grid_Avaluo.Visible = false;
            Div_Datos_Avaluo.Visible = true;
            Session["Anio"] = Hdf_Anio_Avaluo.Value;
            Calcular_Totales_Construccion();
            Calcular_Totales_Terreno();
            Calcular_Valor_Total_Predio();
            Btn_Salir.AlternateText = "Atras";
            Div_Observaciones.Visible = true;
            DataTable Dt_Motivos_Rechazo;
            Aval_Urb.P_Estatus = "= 'VIGENTE'";
            Dt_Motivos_Rechazo = Aval_Urb.Consultar_Motivos_Rechazo_Avaluo_Av();
            Session["Dt_Motivos_Rechazo"] = Dt_Motivos_Rechazo.Copy();
            Grid_Observaciones.Columns[1].Visible = true;
            Grid_Observaciones.Columns[2].Visible = true;
            Grid_Observaciones.Columns[4].Visible = true;
            Grid_Observaciones.DataSource = Dt_Motivos_Rechazo;
            Grid_Observaciones.PageIndex = 0;
            Grid_Observaciones.DataBind();
            Grid_Observaciones.Columns[1].Visible = false;
            Grid_Observaciones.Columns[2].Visible = false;
            Grid_Observaciones.Columns[4].Visible = false;

            DataTable Dt_Medidas = Aval_Urb.P_Dt_Colindancias.Copy();
            //Grid_Colindancias.Columns[0].Visible = true;
            //Grid_Colindancias.DataSource = Dt_Medidas;
            //Grid_Colindancias.DataBind();
            //Grid_Colindancias.Columns[0].Visible = false;
            Session["Dt_Medidas"] = Dt_Medidas.Copy();

            DataTable Dt_Archivos = Aval_Urb.P_Dt_Documentos.Copy();
            Dt_Archivos.Columns.Add("BITS_ARCHIVO", Type.GetType("System.Byte[]"));
            Dt_Archivos.Columns.Add("EXTENSION_ARCHIVO", typeof(String));
            Session["Dt_Documentos"] = Dt_Archivos;
            Grid_Documentos.Columns[0].Visible = true;
            Grid_Documentos.Columns[1].Visible = true;
            Grid_Documentos.DataSource = Dt_Archivos;
            Grid_Documentos.DataBind();
            Grid_Documentos.Columns[0].Visible = false;
            Grid_Documentos.Columns[1].Visible = false;

            Aval_Urb.P_Estatus = "<> 'VIGENTE'";
            Dt_Motivos_Rechazo = Aval_Urb.Consultar_Motivos_Rechazo_Avaluo_Av();
            Session["Dt_Motivos_Rechazo_Corregidas"] = Dt_Motivos_Rechazo.Copy();
            Grid_Observaciones_Corregidas.DataSource = Dt_Motivos_Rechazo;
            Grid_Observaciones_Corregidas.PageIndex = 0;
            Grid_Observaciones_Corregidas.DataBind();
            Cargar_Ventana_Emergente_Resumen_Predio();
        }
    }


    private void Cargar_Datos_Avaluo(DataTable Dt_Avaluo)
    {
        if (Dt_Avaluo.Rows.Count > 0)
        {
            Hdf_Cuenta_Predial_Id.Value = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano_Re.Campo_Cuenta_Predial_Id].ToString();
            Txt_Observaciones.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano_Re.Campo_Observaciones].ToString();
            Txt_Solicitante.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano_Re.Campo_Solicitante].ToString();
            Txt_No_Avaluo.Text = Dt_Avaluo.Rows[0]["AVALUO"].ToString();
            Txt_Cuenta_Predial.Text = Dt_Avaluo.Rows[0]["CUENTA_PREDIAL"].ToString();
            Hdf_No_Avaluo.Value = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano_Re.Campo_No_Avaluo].ToString();
            Hdf_Anio_Avaluo.Value = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano_Re.Campo_Anio_Avaluo].ToString();
            Cmb_Motivo_Avaluo.SelectedIndex = Cmb_Motivo_Avaluo.Items.IndexOf(Cmb_Motivo_Avaluo.Items.FindByValue(HttpUtility.HtmlDecode(Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano_Re.Campo_Motivo_Avaluo_Id].ToString())));
            Cmb_Estatus.SelectedValue = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano_Av.Campo_Estatus].ToString();
            Txt_Comentarios_Perito.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano_Av.Campo_Comentarios_Revisor].ToString();
            Txt_Lote.Text = Dt_Avaluo.Rows[0]["LOTE"].ToString();
            Txt_Manzana.Text = Dt_Avaluo.Rows[0]["MANZANA"].ToString();
            Txt_Region.Text = Dt_Avaluo.Rows[0]["REGION"].ToString();
            Txt_Renglones_Col.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano_Av.Campo_No_Renglones].ToString();
            Cargar_Datos();
            if (Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano_Av.Campo_Tipo_Avaluo].ToString().Trim() == "")
            {
                Chk_Fotogrametria.Checked = false;
                Div_Completo.Visible = true;
            }
            else
            {
                Chk_Fotogrametria.Checked = true;
                Div_Completo.Visible = false;
            }
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Observaciones_Corregidas_PageIndexChanging
    ///DESCRIPCIÓN: Cambia la página del grid
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Grid_Observaciones_Corregidas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            DataTable Dt_Observaciones = (DataTable)Session["Dt_Motivos_Rechazo_Corregidas"];
            Grid_Observaciones_Corregidas.DataSource = Dt_Observaciones;
            Grid_Observaciones_Corregidas.PageIndex = e.NewPageIndex;
            Grid_Observaciones_Corregidas.DataBind();
        }
        catch (Exception ex)
        {
            Lbl_Ecabezado_Mensaje.Text = ex.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    private void Cargar_Caracteristicas_Construccion(DataTable Dt_Car_Construccion)
    {
        if (Dt_Car_Construccion.Rows.Count > 0)
        {
            String Vias_Acceso = Dt_Car_Construccion.Rows[0][Ope_Cat_Caract_Terreno_Re.Campo_Vias_Acceso].ToString();
            String Fotografia = Dt_Car_Construccion.Rows[0][Ope_Cat_Caract_Terreno_Re.Campo_Fotografia].ToString();
            String Dens_Construccion = Convert.ToDouble(Dt_Car_Construccion.Rows[0][Ope_Cat_Caract_Terreno_Re.Campo_Dens_Const].ToString()).ToString("##0.00");
            if (Vias_Acceso.Trim() == "")
            {
                Rdb_Buenas.Checked = false;
                Rdb_Regulares.Checked = false;
                Rdb_Malas.Checked = false;
            }
            else
            {
                switch (Vias_Acceso.Trim())
                {
                    case "BUENA":
                        Rdb_Buenas.Checked = true;
                        break;
                    case "REGULAR":
                        Rdb_Regulares.Checked = true;
                        break;
                    case "MALA":
                        Rdb_Malas.Checked = true;
                        break;
                }
            }
            if (Fotografia.Trim() == "")
            {
                Rdb_Plana.Checked = false;
                Rdb_Pendiente.Checked = false;
            }
            else
            {
                switch (Fotografia.Trim())
                {
                    case "PLANA":
                        Rdb_Plana.Checked = true;
                        break;
                    case "PENDIENTE":
                        Rdb_Pendiente.Checked = true;
                        break;
                }
            }
            Txt_Dens_Construccion.Text = Dens_Construccion;
        }
    }

    private void Cargar_Construccion(DataTable Dt_Construccion)
    {
        if (Dt_Construccion.Rows.Count > 0)
        {
            String Tipo_Construccion = Dt_Construccion.Rows[0][Ope_Cat_Construccion_Re.Campo_Tipo_Construccion].ToString();
            String Calidad_Proyecto = Dt_Construccion.Rows[0][Ope_Cat_Construccion_Re.Campo_Calidad_Proyecto].ToString();
            String Uso_Construccion = Dt_Construccion.Rows[0][Ope_Cat_Construccion_Re.Campo_Uso_Construccion].ToString();
            if (Tipo_Construccion.Trim() == "")
            {
                Rdb_Nueva.Checked = false;
                Rdb_Ampliacion.Checked = false;
                Rdb_Remodelacion.Checked = false;
                Rdb_Rentada.Checked = false;
            }
            else
            {
                switch (Tipo_Construccion.Trim())
                {
                    case "NUEVA":
                        Rdb_Nueva.Checked = true;
                        break;
                    case "AMPLIACION":
                        Rdb_Ampliacion.Checked = true;
                        break;
                    case "REMODELACION":
                        Rdb_Remodelacion.Checked = true;
                        break;
                    case "RENTADA":
                        Rdb_Rentada.Checked = true;
                        break;
                }
            }
            if (Calidad_Proyecto.Trim() == "")
            {
                Rdb_Calidad_Buena.Checked = false;
                Rdb_Calidad_Regular.Checked = false;
                Rdb_Calidad_Mala.Checked = false;
            }
            else
            {
                switch (Calidad_Proyecto.Trim())
                {
                    case "NUEVA":
                        Rdb_Calidad_Buena.Checked = true;
                        break;
                    case "REGULAR":
                        Rdb_Calidad_Regular.Checked = true;
                        break;
                    case "MALA":
                        Rdb_Calidad_Mala.Checked = true;
                        break;
                }
            }
            Txt_Uso.Text = Uso_Construccion;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Evento del botón modificar
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Grid_Avaluos_Urbanos.SelectedIndex > -1)
            {
                if (Grid_Avaluos_Urbanos.SelectedRow.Cells[8].Text == "POR VALIDAR" || Grid_Avaluos_Urbanos.SelectedRow.Cells[8].Text == "RECHAZADO")
                {
                    if (Btn_Modificar.AlternateText.Equals("Modificar"))
                    {
                        Configuracion_Formulario(false);
                        Btn_Modificar.AlternateText = "Actualizar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                        Btn_Salir.AlternateText = "Cancelar";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        Btn_Imprimir.Visible = false;
                        Grid_Documentos.Columns[0].Visible = true;
                        Grid_Documentos.Columns[1].Visible = true;
                        Grid_Documentos.DataSource = (DataTable)Session["Dt_Documentos"];
                        Grid_Documentos.DataBind();
                        Grid_Documentos.Columns[0].Visible = false;
                        Grid_Documentos.Columns[1].Visible = false;
                    }
                    else
                    {
                        Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio Avaluo = new Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio();
                        Avaluo.P_Dt_Observaciones_Av = (DataTable)Session["Dt_Motivos_Rechazo"];
                        Avaluo.P_No_Avaluo = Hdf_No_Avaluo.Value;
                        Avaluo.P_Anio_Avaluo = Hdf_Anio_Avaluo.Value;
                        Cambiar_Estatus_Avaluo_Rechazado_Autorizada();
                        Avaluo.P_Estatus = Cmb_Estatus.SelectedValue;
                        if (Avaluo.P_Estatus == "POR AUTORIZAR")
                        {
                            DataTable Dt_Motivos_Rechazo = (DataTable)Session["Dt_Motivos_Rechazo"];
                            Cmb_Motivos_Rechazo.SelectedIndex = Cmb_Motivos_Rechazo.Items.IndexOf(Cmb_Motivos_Rechazo.Items.FindByText("SE AUTORIZO PARA SEGUNDA REVISION"));
                            DataRow Dr_Nuevo_Registro = Dt_Motivos_Rechazo.NewRow();
                            Dr_Nuevo_Registro["NO_SEGUIMIENTO"] = " ";
                            Dr_Nuevo_Registro["MOTIVO_ID"] = Cmb_Motivos_Rechazo.SelectedValue;
                            Dr_Nuevo_Registro["ESTATUS"] = "BAJA";
                            Dr_Nuevo_Registro["USUARIO_CREO"] = Cls_Sessiones.Nombre_Empleado;
                            Dr_Nuevo_Registro["FECHA_CREO"] = DateTime.Now;
                            Dr_Nuevo_Registro["ACCION"] = "ALTA";
                            Dr_Nuevo_Registro["MOTIVO_DESCRIPCION"] = Cmb_Motivos_Rechazo.SelectedItem.Text;
                            Dt_Motivos_Rechazo.Rows.Add(Dr_Nuevo_Registro);
                        }
                        Avaluo.P_Veces_Rechazo = (Convert.ToInt16(Grid_Avaluos_Urbanos.SelectedRow.Cells[5].Text) + 1).ToString();
                        Avaluo.P_Observaciones_Rechazo = "";
                        if ((Avaluo.Modificar_Observaciones_Avaluo_Av()))
                        {
                            if (Cmb_Estatus.SelectedValue == "RECHAZADO")
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Avalúo Urbano", "alert('Se ha rechazado el Avalúo Exitosamente');", true);
                            }
                            else if (Cmb_Estatus.SelectedValue == "POR AUTORIZAR")
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Avalúo Urbano", "alert('El avalúo paso a la siguiente fase de revisiones.');", true);
                            }
                            Btn_Salir_Click(null, null);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Avalúo Urbano", "alert('Error');", true);
                        }
                    }
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Seleccione el Avalúo.";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Imprimir_Click
    ///DESCRIPCIÓN: Evento del botón Imprimir
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        if (Grid_Avaluos_Urbanos.SelectedIndex > -1)
        {
            if (!Chk_Fotogrametria.Checked)
            {
                Imprimir_Reporte(Crear_Ds_Avaluo_Urbano(), "Rpt_Ope_Cat_Avaluo_Urbano_Av.rpt", "Reporte_Avaluo_Urbano", "Window_Frm", "Avaluo_Urbano");
            }
            else
            {
                Imprimir_Reporte(Crear_Ds_Avaluo_Urbano(), "Rpt_Ope_Cat_Avaluo_Urbano_Fotogrametria.rpt", "Reporte_Avaluo_Urbano", "Window_Frm", "Avaluo_Urbano");
            }
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Evento del botón salir
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText.Equals("Salir"))
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Limpiar_Formulario();
            Btn_Modificar.Visible = true;
            Btn_Modificar.AlternateText = "Modificar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            Configuracion_Formulario(true);
            Llenar_Tabla_Avaluos_Urbanos(Grid_Avaluos_Urbanos.PageIndex);
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Grid_Avaluos_Urbanos.SelectedIndex = -1;
            Div_Datos_Avaluo.Visible = false;
            Div_Grid_Avaluo.Visible = true;
            Div_Observaciones.Visible = false;
            Session["Dt_Grid_Valores_Construccion"] = null;
            Session["Dt_Grid_Calculos"] = null;
            Session["Dt_Grid_Elementos_Construccion"] = null;
            Session["Dt_Grid_Valores_Construccion"] = null;
            Session["Anio"] = null;
            Session["Dt_Tabla_Valores_Construccion"] = null;
            Btn_Imprimir.Visible = true;
            Chk_Fotogrametria.Checked = false;
            Div_Completo.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
    ///DESCRIPCIÓN: Evento del botón buscar
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 24/May/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        if (Txt_Busqueda.Text.Trim() != "")
        {
            Llenar_Tabla_Avaluos_Urbanos(0);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Elementos_Construccion_DataBound
    ///DESCRIPCIÓN: Carga los datos en los componentes del grid
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Grid_Elementos_Construccion_DataBound(object sender, EventArgs e)
    {
        DataTable Dt_Elementos_Construccion = (DataTable)Session["Dt_Grid_Elementos_Construccion"];
        for (int i = 0; i < Dt_Elementos_Construccion.Rows.Count; i++)
        {
            TextBox Txt_A_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[2].FindControl("Txt_A");
            TextBox Txt_B_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[3].FindControl("Txt_B");
            TextBox Txt_C_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[4].FindControl("Txt_C");
            TextBox Txt_D_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[5].FindControl("Txt_D");
            TextBox Txt_E_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[6].FindControl("Txt_E");
            TextBox Txt_F_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[7].FindControl("Txt_F");
            TextBox Txt_G_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[8].FindControl("Txt_G");
            TextBox Txt_H_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[9].FindControl("Txt_H");
            TextBox Txt_I_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[10].FindControl("Txt_I");
            TextBox Txt_J_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[11].FindControl("Txt_J");
            TextBox Txt_K_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[12].FindControl("Txt_K");
            TextBox Txt_L_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[13].FindControl("Txt_L");
            TextBox Txt_M_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[14].FindControl("Txt_M");
            TextBox Txt_N_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[15].FindControl("Txt_N");
            TextBox Txt_O_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[16].FindControl("Txt_O");
            Txt_A_Temporal.Text = Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_A"].ToString();
            Txt_B_Temporal.Text = Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_B"].ToString();
            Txt_C_Temporal.Text = Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_C"].ToString();
            Txt_D_Temporal.Text = Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_D"].ToString();
            Txt_E_Temporal.Text = Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_E"].ToString();
            Txt_F_Temporal.Text = Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_F"].ToString();
            Txt_G_Temporal.Text = Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_G"].ToString();
            Txt_H_Temporal.Text = Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_H"].ToString();
            Txt_I_Temporal.Text = Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_I"].ToString();
            Txt_J_Temporal.Text = Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_J"].ToString();
            Txt_K_Temporal.Text = Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_K"].ToString();
            Txt_L_Temporal.Text = Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_L"].ToString();
            Txt_M_Temporal.Text = Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_M"].ToString();
            Txt_N_Temporal.Text = Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_N"].ToString();
            Txt_O_Temporal.Text = Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_O"].ToString();
        }
    }

    protected void Grid_Calculos_DataBound(object sender, EventArgs e)
    {
        Cls_Cat_Cat_Parametros_Negocio Parametros = new Cls_Cat_Cat_Parametros_Negocio();
        DataTable Dt_Parametros = Parametros.Consultar_Parametros();
        Crear_Mascara(Convert.ToInt16(Dt_Parametros.Rows[0][Cat_Cat_Parametros.Campo_Decimales_Redondeo].ToString()));
        DataTable Dt_Calculos = (DataTable)Session["Dt_Grid_Calculos"];
        for (int i = 0; i < Dt_Calculos.Rows.Count; i++)
        {
            TextBox Txt_Superficie_M2_Temporal = (TextBox)Grid_Calculos.Rows[i].Cells[1].FindControl("Txt_Superficie_M2");
            TextBox Txt_Valor_M2_Temporal = (TextBox)Grid_Calculos.Rows[i].Cells[2].FindControl("Txt_Valor_M2");
            TextBox Txt_Tramo_Id_Temporal = (TextBox)Grid_Calculos.Rows[i].Cells[3].FindControl("Txt_Tramo_Id");
            ImageButton Btn_Valor_Tramo_Temporal = (ImageButton)Grid_Calculos.Rows[i].Cells[4].FindControl("Btn_Valor_Tramo");
            TextBox Txt_Factor_Temporal = (TextBox)Grid_Calculos.Rows[i].Cells[5].FindControl("Txt_Factor");
            TextBox Txt_Factor_Ef_Temporal = (TextBox)Grid_Calculos.Rows[i].Cells[6].FindControl("Txt_Factor_Ef");
            TextBox Txt_Total_Temporal = (TextBox)Grid_Calculos.Rows[i].Cells[7].FindControl("Txt_Total");
            Txt_Superficie_M2_Temporal.Text = Convert.ToDouble(Dt_Calculos.Rows[i]["SUPERFICIE_M2"].ToString()).ToString("###,###,###,##0." + Mascara_Caracteres);
            Txt_Valor_M2_Temporal.Text = Convert.ToDouble(Dt_Calculos.Rows[i]["VALOR_TRAMO"].ToString()).ToString("###,###,###,##0.00");
            Txt_Tramo_Id_Temporal.Text = Dt_Calculos.Rows[i]["VALOR_TRAMO_ID"].ToString();
            Txt_Factor_Temporal.Text = Convert.ToDouble(Dt_Calculos.Rows[i]["FACTOR"].ToString()).ToString("###,###,###,##0." + Mascara_Caracteres);
            Txt_Factor_Ef_Temporal.Text = Convert.ToDouble(Dt_Calculos.Rows[i]["FACTOR_EF"].ToString()).ToString("###,###,###,##0." + Mascara_Caracteres);
            Txt_Total_Temporal.Text = Convert.ToDouble(Dt_Calculos.Rows[i]["VALOR_PARCIAL"].ToString()).ToString("###,###,###,###,###,##0.00");
            String Ventana = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Tabla_Valores_Tramo.aspx";
            String Propiedades = ", 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHide:true;help:no;scroll:no');";
            Btn_Valor_Tramo_Temporal.Attributes.Add("OnClick", Ventana + "?Fecha=False'" + Propiedades);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_Superficie_M2_Cal_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_Superficie_M2 en el Grid de Calculos
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Txt_Superficie_M2_Cal_TextChanged(object sender, EventArgs e)
    {
        int index = 0;
        try
        {
            DataTable Dt_Calidad = (DataTable)Session["Dt_Grid_Calculos"];
            TextBox Txt_Superficie_M2_Temporal = sender as TextBox;
            GridViewRow gvr = Txt_Superficie_M2_Temporal.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_Txt_Superficie_M2 = gvr.FindControl("Txt_Superficie_M2") as TextBox;
            try
            {
                if (Text_Txt_Superficie_M2.Text.Trim() != "")
                {
                    Dt_Calidad.Rows[index]["SUPERFICIE_M2"] = Convert.ToDouble(Text_Txt_Superficie_M2.Text);
                    Text_Txt_Superficie_M2.Text = Convert.ToDouble(Text_Txt_Superficie_M2.Text).ToString("###,###,###,###,##0.00");
                }
                else
                {
                    Dt_Calidad.Rows[index]["SUPERFICIE_M2"] = 0;
                    Text_Txt_Superficie_M2.Text = "0.00";
                }
            }
            catch (Exception Exc)
            {
                Dt_Calidad.Rows[index]["SUPERFICIE_M2"] = 0;
                Text_Txt_Superficie_M2.Text = "0.00";
            }
            Calcular_Valor_Parcial_Terreno(index);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_Factor_Cal_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_Factor en el Grid de Calculos
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Txt_Factor_Cal_TextChanged(object sender, EventArgs e)
    {
        int index = 0;
        try
        {
            DataTable Dt_Calidad = (DataTable)Session["Dt_Grid_Calculos"];
            TextBox Txt_Factor_Temporal = sender as TextBox;
            GridViewRow gvr = Txt_Factor_Temporal.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_Txt_Factor = gvr.FindControl("Txt_Factor") as TextBox;
            try
            {
                if (Text_Txt_Factor.Text.Trim() != "")
                {
                    Dt_Calidad.Rows[index]["FACTOR"] = Convert.ToDouble(Text_Txt_Factor.Text);
                    Text_Txt_Factor.Text = Convert.ToDouble(Text_Txt_Factor.Text).ToString("###,###,###,###,##0.00");
                }
                else
                {
                    Dt_Calidad.Rows[index]["FACTOR"] = 0;
                    Text_Txt_Factor.Text = "0.00";
                }
            }
            catch (Exception Exc)
            {
                Dt_Calidad.Rows[index]["FACTOR"] = 0;
                Text_Txt_Factor.Text = "0.00";
            }
            Calcular_Valor_Parcial_Terreno(index);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_Factor_Ef_Cal_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_Factor_Ef en el Grid de Calculos
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Txt_Factor_Ef_Cal_TextChanged(object sender, EventArgs e)
    {
        int index = 0;
        try
        {
            DataTable Dt_Calidad = (DataTable)Session["Dt_Grid_Calculos"];
            TextBox Txt_Factor_Ef_Temporal = sender as TextBox;
            GridViewRow gvr = Txt_Factor_Ef_Temporal.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_Txt_Factor_Ef = gvr.FindControl("Txt_Factor_Ef") as TextBox;
            try
            {
                if (Text_Txt_Factor_Ef.Text.Trim() != "")
                {
                    Dt_Calidad.Rows[index]["FACTOR_EF"] = Convert.ToDouble(Text_Txt_Factor_Ef.Text);
                    Text_Txt_Factor_Ef.Text = Convert.ToDouble(Text_Txt_Factor_Ef.Text).ToString("###,###,###,###,##0.00");
                }
                else
                {
                    Dt_Calidad.Rows[index]["FACTOR_EF"] = 0;
                    Text_Txt_Factor_Ef.Text = "0.00";
                }
            }
            catch (Exception Exc)
            {
                Dt_Calidad.Rows[index]["FACTOR_EF"] = 0;
                Text_Txt_Factor_Ef.Text = "0.00";
            }
            Calcular_Valor_Parcial_Terreno(index);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    private void Calcular_Valor_Parcial_Terreno(int Index)
    {
        DataTable Dt_Calculos = (DataTable)Session["Dt_Grid_Calculos"];
        Dt_Calculos.Rows[Index]["VALOR_PARCIAL"] = Convert.ToDouble(Dt_Calculos.Rows[Index]["SUPERFICIE_M2"].ToString()) * Convert.ToDouble(Dt_Calculos.Rows[Index]["VALOR_TRAMO"].ToString()) * Convert.ToDouble(Dt_Calculos.Rows[Index]["FACTOR"].ToString()) * Convert.ToDouble(Dt_Calculos.Rows[Index]["FACTOR_EF"].ToString());
        TextBox Text_Txt_Valor_Parcial = (TextBox)Grid_Calculos.Rows[Index].Cells[7].FindControl("Txt_Total");
        Text_Txt_Valor_Parcial.Text = Convert.ToDouble(Dt_Calculos.Rows[Index]["VALOR_PARCIAL"].ToString()).ToString("###,###,###,###,###,##0.00");
        Calcular_Totales_Terreno();
    }

    private void Calcular_Totales_Terreno()
    {
        DataTable Dt_Calculos = (DataTable)Session["Dt_Grid_Calculos"];
        Double Superficie_Total = 0;
        Double Valor_Total = 0;
        foreach (DataRow Dr_Renglon in Dt_Calculos.Rows)
        {
            if (Dr_Renglon["SECCION"].ToString() != "INC. ESQ.")
            {
                Superficie_Total += Convert.ToDouble(Dr_Renglon["SUPERFICIE_M2"].ToString());
            }
            Valor_Total += Convert.ToDouble(Dr_Renglon["VALOR_PARCIAL"].ToString());
        }
        Txt_Terreno_Superficie_Total.Text = Superficie_Total.ToString("###,###,###,###,###,##0.00");
        Txt_Terreno_Valor_Total.Text = Valor_Total.ToString("###,###,###,###,###,##0.00");

        Calcular_Valor_Total_Predio();
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Crear_Dt_Calculos
    ///DESCRIPCIÓN: Crea la tabla inicial de calculos para el grid Grid_Calculos
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 24/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    private void Crear_Dt_Calculos()
    {
        DataTable Dt_Calculos = new DataTable();
        Dt_Calculos.Columns.Add("SECCION", typeof(String));
        Dt_Calculos.Columns.Add("SUPERFICIE_M2", typeof(Double));
        Dt_Calculos.Columns.Add("VALOR_TRAMO", typeof(Double));
        Dt_Calculos.Columns.Add("VALOR_TRAMO_ID", typeof(String));
        Dt_Calculos.Columns.Add("FACTOR", typeof(Double));
        Dt_Calculos.Columns.Add("FACTOR_EF", typeof(Double));
        Dt_Calculos.Columns.Add("VALOR_PARCIAL", typeof(Double));
        DataRow Dr_renglon = Dt_Calculos.NewRow();
        Dr_renglon["SECCION"] = "I";
        Dr_renglon["SUPERFICIE_M2"] = 0;
        Dr_renglon["VALOR_TRAMO"] = 0;
        Dr_renglon["VALOR_TRAMO_ID"] = "";
        Dr_renglon["FACTOR"] = 1;
        Dr_renglon["FACTOR_EF"] = 1;
        Dr_renglon["VALOR_PARCIAL"] = 0;
        Dt_Calculos.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Calculos.NewRow();
        Dr_renglon["SECCION"] = "II";
        Dr_renglon["SUPERFICIE_M2"] = 0;
        Dr_renglon["VALOR_TRAMO"] = 0;
        Dr_renglon["VALOR_TRAMO_ID"] = "";
        Dr_renglon["FACTOR"] = 1;
        Dr_renglon["FACTOR_EF"] = 1;
        Dr_renglon["VALOR_PARCIAL"] = 0;
        Dt_Calculos.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Calculos.NewRow();
        Dr_renglon["SECCION"] = "III";
        Dr_renglon["SUPERFICIE_M2"] = 0;
        Dr_renglon["VALOR_TRAMO"] = 0;
        Dr_renglon["VALOR_TRAMO_ID"] = "";
        Dr_renglon["FACTOR"] = 1;
        Dr_renglon["FACTOR_EF"] = 1;
        Dr_renglon["VALOR_PARCIAL"] = 0;
        Dt_Calculos.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Calculos.NewRow();
        Dr_renglon["SECCION"] = "IV";
        Dr_renglon["SUPERFICIE_M2"] = 0;
        Dr_renglon["VALOR_TRAMO"] = 0;
        Dr_renglon["VALOR_TRAMO_ID"] = "";
        Dr_renglon["FACTOR"] = 1;
        Dr_renglon["FACTOR_EF"] = 1;
        Dr_renglon["VALOR_PARCIAL"] = 0;
        Dt_Calculos.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Calculos.NewRow();
        Dr_renglon["SECCION"] = "INC. ESQ.";
        Dr_renglon["SUPERFICIE_M2"] = 0;
        Dr_renglon["VALOR_TRAMO"] = 0;
        Dr_renglon["VALOR_TRAMO_ID"] = "";
        Dr_renglon["FACTOR"] = 1;
        Dr_renglon["FACTOR_EF"] = 1;
        Dr_renglon["VALOR_PARCIAL"] = 0;
        Dt_Calculos.Rows.Add(Dr_renglon);
        Session["Dt_Grid_Calculos"] = Dt_Calculos.Copy();
        Grid_Calculos.DataSource = Dt_Calculos;
        Grid_Calculos.PageIndex = 0;
        Grid_Calculos.DataBind();
    }

    //Pendiente
    protected void Grid_Valores_Construccion_DataBound(object sender, EventArgs e)
    {
        DataTable Dt_Valores_Construccion = (DataTable)Session["Dt_Grid_Valores_Construccion"];
        for (int i = 0; i < Dt_Valores_Construccion.Rows.Count; i++)
        {
            TextBox Txt_Tipo_Temporal = (TextBox)Grid_Valores_Construccion.Rows[i].Cells[1].FindControl("Txt_Tipo");
            TextBox Txt_Con_Serv_Temporal = (TextBox)Grid_Valores_Construccion.Rows[i].Cells[2].FindControl("Txt_Con_Serv");
            TextBox Txt_Superficie_M2_Temporal = (TextBox)Grid_Valores_Construccion.Rows[i].Cells[3].FindControl("Txt_Superficie_M2");
            TextBox Txt_Valor_M2_Temporal = (TextBox)Grid_Valores_Construccion.Rows[i].Cells[4].FindControl("Txt_Valor_X_M2");
            TextBox Txt_Valor_Construccion_Id_Temporal = (TextBox)Grid_Valores_Construccion.Rows[i].Cells[5].FindControl("Txt_Valor_Construccion_Id");
            TextBox Txt_Factor_Temporal = (TextBox)Grid_Valores_Construccion.Rows[i].Cells[6].FindControl("Txt_Factor");
            TextBox Txt_Total_Temporal = (TextBox)Grid_Valores_Construccion.Rows[i].Cells[7].FindControl("Txt_Total");
            Txt_Tipo_Temporal.Text = Dt_Valores_Construccion.Rows[i]["TIPO"].ToString();
            Txt_Con_Serv_Temporal.Text = Dt_Valores_Construccion.Rows[i]["CON_SERV"].ToString();
            Txt_Superficie_M2_Temporal.Text = Convert.ToDouble(Dt_Valores_Construccion.Rows[i]["SUPERFICIE_M2"].ToString()).ToString("###,###,###,##0.00");
            Txt_Valor_M2_Temporal.Text = Convert.ToDouble(Dt_Valores_Construccion.Rows[i]["VALOR_M2"].ToString()).ToString("###,###,###,##0.00");
            Txt_Valor_Construccion_Id_Temporal.Text = Dt_Valores_Construccion.Rows[i]["VALOR_CONSTRUCCION_ID"].ToString();
            Txt_Factor_Temporal.Text = Convert.ToDouble(Dt_Valores_Construccion.Rows[i]["FACTOR"].ToString()).ToString("###,###,###,##0.00");
            Txt_Total_Temporal.Text = Convert.ToDouble(Dt_Valores_Construccion.Rows[i]["VALOR_PARCIAL"].ToString()).ToString("###,###,###,###,###,##0.00");
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_Factor_Ef_Cal_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_Factor_Ef en el Grid de Calculos
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Txt_Tipo_Constru_TextChanged(object sender, EventArgs e)
    {
        int index = 0;
        try
        {
            DataTable Dt_Valores_Construccion = (DataTable)Session["Dt_Grid_Valores_Construccion"];
            TextBox Txt_Tipo_Temporal = sender as TextBox;
            GridViewRow gvr = Txt_Tipo_Temporal.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_Txt_Tipo = gvr.FindControl("Txt_Tipo") as TextBox;
            TextBox Text_Txt_Con_Serv = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[2].FindControl("Txt_Con_Serv");
            try
            {
                if (Text_Txt_Tipo.Text.Trim() != "")
                {
                    Dt_Valores_Construccion.Rows[index]["TIPO"] = Convert.ToInt16(Text_Txt_Tipo.Text);
                    Text_Txt_Tipo.Text = Text_Txt_Tipo.Text.Trim();
                    if (Text_Txt_Con_Serv.Text.Trim() != "")
                    {
                        DataTable Dt_Tabla_Valores = (DataTable)Session["Dt_Tabla_Valores_Construccion"];
                        Boolean Coinciden_Tipo_Con_Serv = false;
                        String Valor_Construccion_Id = " ";
                        Double Valor_M2 = 0;
                        foreach (DataRow Dr_Renglon in Dt_Tabla_Valores.Rows)
                        {
                            if (Text_Txt_Con_Serv.Text.Trim() == Dr_Renglon["CON_SERV"].ToString() && Text_Txt_Tipo.Text.Trim() == Dr_Renglon["TIPO"].ToString())
                            {
                                Coinciden_Tipo_Con_Serv = true;
                                Valor_Construccion_Id = Dr_Renglon["VALOR_CONSTRUCCION_ID"].ToString();
                                Valor_M2 = Convert.ToDouble(Dr_Renglon["VALOR_M2"].ToString());
                                break;
                            }
                        }
                        if (Coinciden_Tipo_Con_Serv)
                        {
                            TextBox Txt_Temporal_Val_Const_Id = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[5].FindControl("Txt_Valor_Construccion_Id");
                            TextBox Txt_Temporal_Valor_M2 = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[4].FindControl("Txt_Valor_X_M2");
                            Txt_Temporal_Val_Const_Id.Text = Valor_Construccion_Id;
                            Txt_Temporal_Valor_M2.Text = Valor_M2.ToString("###,###,###,###,##0.00");
                            Dt_Valores_Construccion.Rows[index]["VALOR_CONSTRUCCION_ID"] = Valor_Construccion_Id;
                            Dt_Valores_Construccion.Rows[index]["VALOR_M2"] = Valor_M2;
                        }
                        else
                        {
                            TextBox Txt_Temporal_Val_Const_Id = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[5].FindControl("Txt_Valor_Construccion_Id");
                            TextBox Txt_Temporal_Valor_M2 = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[4].FindControl("Txt_Valor_X_M2");
                            Txt_Temporal_Val_Const_Id.Text = "";
                            Txt_Temporal_Valor_M2.Text = Valor_M2.ToString("###,###,###,###,##0.00");
                            Dt_Valores_Construccion.Rows[index]["VALOR_CONSTRUCCION_ID"] = " ";
                            Dt_Valores_Construccion.Rows[index]["VALOR_M2"] = Valor_M2;
                        }
                    }
                }
                else
                {
                    Dt_Valores_Construccion.Rows[index]["TIPO"] = 0;
                    Text_Txt_Tipo.Text = "0";
                    TextBox Txt_Temporal_Val_Const_Id = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[5].FindControl("Txt_Valor_Construccion_Id");
                    TextBox Txt_Temporal_Valor_M2 = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[4].FindControl("Txt_Valor_X_M2");
                    Txt_Temporal_Val_Const_Id.Text = "";
                    Txt_Temporal_Valor_M2.Text = "0.00";
                    Dt_Valores_Construccion.Rows[index]["VALOR_CONSTRUCCION_ID"] = " ";
                    Dt_Valores_Construccion.Rows[index]["VALOR_M2"] = 0;
                }
            }
            catch (Exception Exc)
            {
                Dt_Valores_Construccion.Rows[index]["TIPO"] = 0;
                Text_Txt_Tipo.Text = "0";
                TextBox Txt_Temporal_Val_Const_Id = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[5].FindControl("Txt_Valor_Construccion_Id");
                TextBox Txt_Temporal_Valor_M2 = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[4].FindControl("Txt_Valor_X_M2");
                Txt_Temporal_Val_Const_Id.Text = "";
                Txt_Temporal_Valor_M2.Text = "0.00";
                Dt_Valores_Construccion.Rows[index]["VALOR_CONSTRUCCION_ID"] = " ";
                Dt_Valores_Construccion.Rows[index]["VALOR_M2"] = 0;
            }
            Calcular_Valor_Parcial_Construccion(index);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_Con_Serv_Constru_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_Factor_Ef en el Grid de Calculos
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Txt_Con_Serv_Constru_TextChanged(object sender, EventArgs e)
    {
        int index = 0;
        try
        {
            DataTable Dt_Valores_Construccion = (DataTable)Session["Dt_Grid_Valores_Construccion"];
            TextBox Txt_Con_Serv_Temporal = sender as TextBox;
            GridViewRow gvr = Txt_Con_Serv_Temporal.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_Txt_Con_Serv = gvr.FindControl("Txt_Con_Serv") as TextBox;
            TextBox Text_Txt_Tipo = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[1].FindControl("Txt_Tipo");
            try
            {
                if (Text_Txt_Con_Serv.Text.Trim() != "")
                {
                    Dt_Valores_Construccion.Rows[index]["CON_SERV"] = Convert.ToDouble(Text_Txt_Con_Serv.Text);
                    Text_Txt_Con_Serv.Text = Convert.ToInt16(Text_Txt_Con_Serv.Text).ToString();

                    if (Text_Txt_Tipo.Text.Trim() != "")
                    {
                        DataTable Dt_Tabla_Valores = (DataTable)Session["Dt_Tabla_Valores_Construccion"];
                        Boolean Coinciden_Tipo_Con_Serv = false;
                        String Valor_Construccion_Id = " ";
                        Double Valor_M2 = 0;
                        foreach (DataRow Dr_Renglon in Dt_Tabla_Valores.Rows)
                        {
                            if (Text_Txt_Con_Serv.Text.Trim() == Dr_Renglon["CON_SERV"].ToString() && Text_Txt_Tipo.Text.Trim() == Dr_Renglon["TIPO"].ToString())
                            {
                                Coinciden_Tipo_Con_Serv = true;
                                Valor_Construccion_Id = Dr_Renglon["VALOR_CONSTRUCCION_ID"].ToString();
                                Valor_M2 = Convert.ToDouble(Dr_Renglon["VALOR_M2"].ToString());
                                break;
                            }
                        }
                        if (Coinciden_Tipo_Con_Serv)
                        {
                            TextBox Txt_Temporal_Val_Const_Id = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[5].FindControl("Txt_Valor_Construccion_Id");
                            TextBox Txt_Temporal_Valor_M2 = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[4].FindControl("Txt_Valor_X_M2");
                            Txt_Temporal_Val_Const_Id.Text = Valor_Construccion_Id;
                            Txt_Temporal_Valor_M2.Text = Valor_M2.ToString("###,###,###,###,##0.00");
                            Dt_Valores_Construccion.Rows[index]["VALOR_CONSTRUCCION_ID"] = Valor_Construccion_Id;
                            Dt_Valores_Construccion.Rows[index]["VALOR_M2"] = Valor_M2;
                        }
                        else
                        {
                            TextBox Txt_Temporal_Val_Const_Id = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[5].FindControl("Txt_Valor_Construccion_Id");
                            TextBox Txt_Temporal_Valor_M2 = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[4].FindControl("Txt_Valor_X_M2");
                            Txt_Temporal_Val_Const_Id.Text = "";
                            Txt_Temporal_Valor_M2.Text = Valor_M2.ToString("###,###,###,###,##0.00");
                            Dt_Valores_Construccion.Rows[index]["VALOR_CONSTRUCCION_ID"] = " ";
                            Dt_Valores_Construccion.Rows[index]["VALOR_M2"] = Valor_M2;
                        }
                    }
                }
                else
                {
                    Dt_Valores_Construccion.Rows[index]["CON_SERV"] = 0;
                    Text_Txt_Con_Serv.Text = "0";
                    TextBox Txt_Temporal_Val_Const_Id = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[5].FindControl("Txt_Valor_Construccion_Id");
                    TextBox Txt_Temporal_Valor_M2 = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[4].FindControl("Txt_Valor_X_M2");
                    Txt_Temporal_Val_Const_Id.Text = "";
                    Txt_Temporal_Valor_M2.Text = "0.00";
                    Dt_Valores_Construccion.Rows[index]["VALOR_CONSTRUCCION_ID"] = " ";
                    Dt_Valores_Construccion.Rows[index]["VALOR_M2"] = 0;
                }
            }
            catch (Exception Exc)
            {
                Dt_Valores_Construccion.Rows[index]["CON_SERV"] = 0;
                Text_Txt_Con_Serv.Text = "0";
                TextBox Txt_Temporal_Val_Const_Id = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[5].FindControl("Txt_Valor_Construccion_Id");
                TextBox Txt_Temporal_Valor_M2 = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[4].FindControl("Txt_Valor_X_M2");
                Txt_Temporal_Val_Const_Id.Text = "";
                Txt_Temporal_Valor_M2.Text = "0.00";
                Dt_Valores_Construccion.Rows[index]["VALOR_CONSTRUCCION_ID"] = " ";
                Dt_Valores_Construccion.Rows[index]["VALOR_M2"] = 0;
            }
            Calcular_Valor_Parcial_Construccion(index);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_Superficie_M2_Constru_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_Factor_Ef en el Grid de Calculos
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Txt_Superficie_M2_Constru_TextChanged(object sender, EventArgs e)
    {
        int index = 0;
        try
        {
            DataTable Dt_Valores_Construccion = (DataTable)Session["Dt_Grid_Valores_Construccion"];
            TextBox Txt_Superficie_M2_Temporal = sender as TextBox;
            GridViewRow gvr = Txt_Superficie_M2_Temporal.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_Txt_Superficie_M2 = gvr.FindControl("Txt_Superficie_M2") as TextBox;
            try
            {
                if (Text_Txt_Superficie_M2.Text.Trim() != "")
                {
                    Dt_Valores_Construccion.Rows[index]["SUPERFICIE_M2"] = Convert.ToDouble(Text_Txt_Superficie_M2.Text);
                    Text_Txt_Superficie_M2.Text = Convert.ToDouble(Text_Txt_Superficie_M2.Text).ToString("###,###,###,###,##0.00");
                }
                else
                {
                    Dt_Valores_Construccion.Rows[index]["SUPERFICIE_M2"] = 0;
                    Text_Txt_Superficie_M2.Text = "0.00";
                }
            }
            catch (Exception Exc)
            {
                Dt_Valores_Construccion.Rows[index]["SUPERFICIE_M2"] = 0;
                Text_Txt_Superficie_M2.Text = "0.00";
            }
            Calcular_Valor_Parcial_Construccion(index);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_Factor_Constru_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_Factor_Ef en el Grid de Calculos
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Txt_Factor_Constru_TextChanged(object sender, EventArgs e)
    {
        int index = 0;
        try
        {
            DataTable Dt_Valores_Construccion = (DataTable)Session["Dt_Grid_Valores_Construccion"];
            TextBox Txt_Factor_Temporal = sender as TextBox;
            GridViewRow gvr = Txt_Factor_Temporal.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_Txt_Factor = gvr.FindControl("Txt_Factor") as TextBox;
            try
            {
                if (Text_Txt_Factor.Text.Trim() != "")
                {
                    Dt_Valores_Construccion.Rows[index]["FACTOR"] = Convert.ToDouble(Text_Txt_Factor.Text);
                    Text_Txt_Factor.Text = Convert.ToDouble(Text_Txt_Factor.Text).ToString("###,###,###,###,##0.00");
                }
                else
                {
                    Dt_Valores_Construccion.Rows[index]["FACTOR"] = 0;
                    Text_Txt_Factor.Text = "0.00";
                }
            }
            catch (Exception Exc)
            {
                Dt_Valores_Construccion.Rows[index]["FACTOR"] = 0;
                Text_Txt_Factor.Text = "0.00";
            }
            Calcular_Valor_Parcial_Construccion(index);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    private void Calcular_Valor_Parcial_Construccion(int Index)
    {
        DataTable Dt_Valores_Construccion = (DataTable)Session["Dt_Grid_Valores_Construccion"];
        Dt_Valores_Construccion.Rows[Index]["VALOR_PARCIAL"] = Convert.ToDouble(Dt_Valores_Construccion.Rows[Index]["SUPERFICIE_M2"].ToString()) * Convert.ToDouble(Dt_Valores_Construccion.Rows[Index]["VALOR_M2"].ToString()) * Convert.ToDouble(Dt_Valores_Construccion.Rows[Index]["FACTOR"].ToString());
        TextBox Text_Txt_Valor_Parcial = (TextBox)Grid_Valores_Construccion.Rows[Index].Cells[7].FindControl("Txt_Total");
        Text_Txt_Valor_Parcial.Text = Convert.ToDouble(Dt_Valores_Construccion.Rows[Index]["VALOR_PARCIAL"].ToString()).ToString("###,###,###,###,###,##0.00");
        Calcular_Totales_Construccion();
    }

    private void Calcular_Totales_Construccion()
    {
        DataTable Dt_Valores_Construccion = (DataTable)Session["Dt_Grid_Valores_Construccion"];
        Double Superficie_Total = 0;
        Double Valor_Total = 0;
        foreach (DataRow Dr_Renglon in Dt_Valores_Construccion.Rows)
        {
            Superficie_Total += Convert.ToDouble(Dr_Renglon["SUPERFICIE_M2"].ToString());
            Valor_Total += Convert.ToDouble(Dr_Renglon["VALOR_PARCIAL"].ToString());
        }
        Txt_Construccion_Superficie_Total.Text = Superficie_Total.ToString("###,###,###,###,###,##0.00");
        Txt_Construccion_Valor_Total.Text = Valor_Total.ToString("###,###,###,###,###,##0.00");

        Calcular_Valor_Total_Predio();
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Crear_Dt_Valores_Construccion
    ///DESCRIPCIÓN: Crea la tabla inicial de calculos para el grid Grid_Valores_Construccion
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 24/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    private void Crear_Dt_Valores_Construccion()
    {
        DataTable Dt_Valores_Construccion = new DataTable();
        Dt_Valores_Construccion.Columns.Add("REFERENCIA", typeof(String));
        Dt_Valores_Construccion.Columns.Add("TIPO", typeof(Int16));
        Dt_Valores_Construccion.Columns.Add("CON_SERV", typeof(Int16));
        Dt_Valores_Construccion.Columns.Add("SUPERFICIE_M2", typeof(Double));
        Dt_Valores_Construccion.Columns.Add("VALOR_M2", typeof(Double));
        Dt_Valores_Construccion.Columns.Add("VALOR_CONSTRUCCION_ID", typeof(String));
        Dt_Valores_Construccion.Columns.Add("FACTOR", typeof(Double));
        Dt_Valores_Construccion.Columns.Add("VALOR_PARCIAL", typeof(Double));
        DataRow Dr_renglon = Dt_Valores_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "A";
        Dr_renglon["TIPO"] = 0;
        Dr_renglon["CON_SERV"] = 0;
        Dr_renglon["SUPERFICIE_M2"] = 0;
        Dr_renglon["VALOR_M2"] = 0;
        Dr_renglon["VALOR_CONSTRUCCION_ID"] = " ";
        Dr_renglon["FACTOR"] = 1;
        Dr_renglon["VALOR_PARCIAL"] = 0;
        Dt_Valores_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Valores_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "B";
        Dr_renglon["TIPO"] = 0;
        Dr_renglon["CON_SERV"] = 0;
        Dr_renglon["SUPERFICIE_M2"] = 0;
        Dr_renglon["VALOR_M2"] = 0;
        Dr_renglon["VALOR_CONSTRUCCION_ID"] = " ";
        Dr_renglon["FACTOR"] = 1;
        Dr_renglon["VALOR_PARCIAL"] = 0;
        Dt_Valores_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Valores_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "C";
        Dr_renglon["TIPO"] = 0;
        Dr_renglon["CON_SERV"] = 0;
        Dr_renglon["SUPERFICIE_M2"] = 0;
        Dr_renglon["VALOR_M2"] = 0;
        Dr_renglon["VALOR_CONSTRUCCION_ID"] = " ";
        Dr_renglon["FACTOR"] = 1;
        Dr_renglon["VALOR_PARCIAL"] = 0;
        Dt_Valores_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Valores_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "D";
        Dr_renglon["TIPO"] = 0;
        Dr_renglon["CON_SERV"] = 0;
        Dr_renglon["SUPERFICIE_M2"] = 0;
        Dr_renglon["VALOR_M2"] = 0;
        Dr_renglon["VALOR_CONSTRUCCION_ID"] = " ";
        Dr_renglon["FACTOR"] = 1;
        Dr_renglon["VALOR_PARCIAL"] = 0;
        Dt_Valores_Construccion.Rows.Add(Dr_renglon);
        Session["Dt_Grid_Valores_Construccion"] = Dt_Valores_Construccion.Copy();
        Grid_Valores_Construccion.DataSource = Dt_Valores_Construccion;
        Grid_Valores_Construccion.PageIndex = 0;
        Grid_Valores_Construccion.DataBind();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Mostrar_Busqueda_Avanzada_Click
    ///DESCRIPCIÓN          : Muestra los datos de la consulta
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial_Click(object sender, ImageClickEventArgs e)
    {
        Boolean Busqueda_Ubicaciones;
        String Cuenta_Predial_ID;
        String Cuenta_Predial;

        Busqueda_Ubicaciones = Convert.ToBoolean(Session["BUSQUEDA_CUENTAS_PREDIAL"]);
        if (Busqueda_Ubicaciones)
        {
            if (Session["CUENTA_PREDIAL_ID"] != null)
            {
                //Limpiar_Formulario();
                Cuenta_Predial_ID = Convert.ToString(Session["CUENTA_PREDIAL_ID"]);
                Hdf_Cuenta_Predial_Id.Value = Cuenta_Predial_ID;
                Cuenta_Predial = Convert.ToString(Session["CUENTA_PREDIAL"]);
                Txt_Cuenta_Predial.Text = Cuenta_Predial;
                //Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Cuenta_Pendiente = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
                //Cuenta_Pendiente.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_Id.Value;
                //if (!Cuenta_Pendiente.Consultar_Cuenta_Pendiente())
                //{
                Cargar_Datos();
                //}
                //else
                //{
                //    Txt_Cuenta_Predial_TextChanged();
                //}
                //Consultar_Identificadores_Predio();
            }
        }
        Session.Remove("BUSQUEDA_CUENTAS_PREDIAL");
        Session.Remove("CUENTA_PREDIAL_ID");
        //Session.Remove("CUENTA_PREDIAL");
    }

    protected void Grid_Calculos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (Session["VALOR_TRAMO_ID"] != null && Session["VALOR_M2"] != null)
        {
            DataTable Dt_Calculos = (DataTable)Session["Dt_Grid_Calculos"];
            TextBox Txt_Valor_M2_Temporal = (TextBox)Grid_Calculos.Rows[Convert.ToInt16(e.CommandArgument)].Cells[2].FindControl("Txt_Valor_M2");
            TextBox Txt_Tramo_Id_Temporal = (TextBox)Grid_Calculos.Rows[Convert.ToInt16(e.CommandArgument)].Cells[3].FindControl("Txt_Tramo_Id");
            Txt_Tramo_Id_Temporal.Text = Session["VALOR_TRAMO_ID"].ToString();
            Txt_Valor_M2_Temporal.Text = Convert.ToDouble(Session["VALOR_M2"].ToString()).ToString("###,###,###,###,##0.00");
            Dt_Calculos.Rows[Convert.ToInt16(e.CommandArgument)]["VALOR_TRAMO_ID"] = Session["VALOR_TRAMO_ID"].ToString();
            Dt_Calculos.Rows[Convert.ToInt16(e.CommandArgument)]["VALOR_TRAMO"] = Convert.ToDouble(Session["VALOR_M2"].ToString());
            Session["VALOR_TRAMO_ID"] = null;
            Session["VALOR_M2"] = null;
            Calcular_Valor_Parcial_Terreno(Convert.ToInt16(e.CommandArgument));
        }
    }



    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Generales_Cuenta
    ///DESCRIPCIÓN: asignar datos generales de cuenta a los controles y objeto de negocio
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/Ago/2011 06:31:08 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    private void Cargar_Generales_Cuenta(DataTable dataTable)
    {
        Cls_Ope_Pre_Resumen_Predio_Negocio Rs_Consulta_Ope_Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio(); //Variable de conexión hacia la capa de Negocios
        try
        {
            Txt_Cuenta_Predial.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial].ToString();
            Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial_ID = dataTable.Rows[0]["Cuenta_Predial_ID"].ToString().Trim();

            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID].ToString() != string.Empty)
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Estado_Predio = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID].ToString();
                DataTable Dt_Estado_Predio = Rs_Consulta_Ope_Resumen_Predio.Consultar_Estado_Predio();
                Txt_Municipio.Text = Dt_Estado_Predio.Rows[0]["Descripcion"].ToString();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString() != string.Empty)
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Calle_ID = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
                DataTable Dt_Calles = Rs_Consulta_Ope_Resumen_Predio.Consultar_Calle_Generales();
                Txt_Ubicacion_Predio.Text = Dt_Calles.Rows[0]["Nombre"].ToString();
                Rs_Consulta_Ope_Resumen_Predio.P_Colonia_ID = Dt_Calles.Rows[0]["Colonia_ID"].ToString();
                DataTable Dt_Colonia = Rs_Consulta_Ope_Resumen_Predio.Consultar_Colonia_Generales();
                Txt_Colonia.Text = Dt_Colonia.Rows[0]["Nombre"].ToString();
            }

            if (dataTable.Rows[0][Cat_Pre_Calles.Campo_Colonia_ID].ToString() != "")
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Colonia_ID = dataTable.Rows[0][Cat_Pre_Calles.Campo_Colonia_ID].ToString();
                DataTable Dt_Colonia = Rs_Consulta_Ope_Resumen_Predio.Consultar_Colonia_Generales();
                Txt_Colonia.Text = Dt_Colonia.Rows[0][Cat_Ate_Colonias.Campo_Nombre].ToString();
                M_Orden_Negocio.P_Ubicacion_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString().Trim() != "")
            {
                Txt_Ubicacion_Predio.Text = Txt_Ubicacion_Predio.Text + " NO. EXT. " + dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString();
            }
            M_Orden_Negocio.P_Exterior_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString();
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString().Trim() != "")
            {
                Txt_Ubicacion_Predio.Text = Txt_Ubicacion_Predio.Text + " NO. INT. " + dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString();
            }
            M_Orden_Negocio.P_Interior_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString();

            M_Orden_Negocio.P_Clave_Catastral = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral].ToString();
            //Txt_Clave_Catastral.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral].ToString();
            Txt_Valor_Total_Predio_Anterior.Text = Convert.ToDecimal(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal].ToString()).ToString("###,###,###,###,###,##0.00");

            Txt_Cuota_Anual_Anterior.Text = Convert.ToDecimal(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString()).ToString("###,###,###,###,###,###,##0.00");

            if (!String.IsNullOrEmpty(dataTable.Rows[0]["Estado_ID_Notificacion"].ToString()))
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Estado_Predio = (dataTable.Rows[0]["Estado_ID_Notificacion"].ToString());
                DataTable Dt_Estado_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Estado_Predio_Propietario();
                if (Dt_Estado_Propietario.Rows.Count > 0)
                {
                    Txt_Localidad_Not.Text = (Dt_Estado_Propietario.Rows[0]["Descripcion"].ToString());
                    Txt_Localidad.Text = (Dt_Estado_Propietario.Rows[0]["Descripcion"].ToString());
                    M_Orden_Negocio.P_Estado_Propietario = (Dt_Estado_Propietario.Rows[0]["Descripcion"].ToString());
                }
            }
            else if (!String.IsNullOrEmpty(dataTable.Rows[0]["Estado_Notificacion"].ToString()))
            {
                Txt_Localidad_Not.Text = dataTable.Rows[0]["Estado_Notificacion"].ToString();
            }
            if (!String.IsNullOrEmpty(dataTable.Rows[0]["Ciudad_ID_Notificacion"].ToString()))
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Ciudad_ID = dataTable.Rows[0]["Ciudad_ID_Notificacion"].ToString();
                DataTable Dt_Ciudad_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Ciudad();
                Txt_Municipio_Not.Text = Dt_Ciudad_Propietario.Rows[0]["Nombre"].ToString();
                M_Orden_Negocio.P_Ciudad_Propietario = dataTable.Rows[0]["Ciudad_ID_Notificacion"].ToString();
            }
            else if (!String.IsNullOrEmpty(dataTable.Rows[0]["Ciudad_Notificacion"].ToString()))
            {
                Txt_Municipio_Not.Text = dataTable.Rows[0]["Ciudad_Notificacion"].ToString();
            }
            Txt_Localidad.Text = "GUANAJUATO";
            Txt_Localidad_Not.Text = "GUANAJUATO";
            Txt_Municipio_Not.Text = "IRAPUATO";
            Txt_Municipio.Text = "IRAPUATO";
            if (dataTable.Rows[0]["Domicilio_Foraneo"].ToString().Trim() == "SI")
            {
                if (!String.IsNullOrEmpty(dataTable.Rows[0]["Colonia_Notificacion"].ToString()))
                {
                    Txt_Colonia_Not.Text = dataTable.Rows[0]["Colonia_Notificacion"].ToString();
                }
                if (!String.IsNullOrEmpty(dataTable.Rows[0]["Calle_Notificacion"].ToString()))
                {
                    Txt_Domicilio_Not.Text = dataTable.Rows[0]["Calle_Notificacion"].ToString();
                    if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString().Trim() != "")
                    {
                        Txt_Domicilio_Not.Text = Txt_Domicilio_Not.Text + " NO. EXT. " + dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString();
                    }
                    if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString().Trim() != "")
                    {
                        Txt_Domicilio_Not.Text = Txt_Domicilio_Not.Text + " NO. INT. " + dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString();
                    }
                }
            }
            else
            {

                if (!String.IsNullOrEmpty(dataTable.Rows[0]["Colonia_ID_Notificacion"].ToString()))
                {
                    Rs_Consulta_Ope_Resumen_Predio.P_Colonia_ID = dataTable.Rows[0]["Colonia_ID_Notificacion"].ToString();
                    DataTable DT_Colonia_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Colonia_Generales();
                    Txt_Colonia_Not.Text = DT_Colonia_Propietario.Rows[0]["Nombre"].ToString();
                }

                if (!String.IsNullOrEmpty(dataTable.Rows[0]["Calle_ID_Notificacion"].ToString()))
                {
                    Rs_Consulta_Ope_Resumen_Predio.P_Calle_ID = dataTable.Rows[0]["Calle_ID_Notificacion"].ToString();//*
                    DataTable Dt_Calle_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Calle_Generales();
                    Txt_Domicilio_Not.Text = Dt_Calle_Propietario.Rows[0]["Nombre"].ToString();
                    if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion].ToString().Trim() != "")
                    {
                        Txt_Domicilio_Not.Text = Txt_Domicilio_Not.Text + ", NO. EXT. " + dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion].ToString();
                    }
                    if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString().Trim() != "")
                    {
                        Txt_Domicilio_Not.Text = Txt_Domicilio_Not.Text + ", NO. INT. " + dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion].ToString();
                    }
                }
            }
            Txt_Domicilio_Not.Text = Txt_Domicilio_Not.Text.Replace("   ", "");
            Txt_Ubicacion_Predio.Text = Txt_Ubicacion_Predio.Text.Replace("   ", "");
        }
        catch (Exception Ex)
        {
            throw new Exception("Cargar_Datos_Cuenta: " + Ex.Message);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Consultar_Identificadores_Predio
    ///DESCRIPCIÓN: Obtiene el lote, región y manzana.
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 22/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Consultar_Identificadores_Predio()
    {
        try
        {
            DataTable Dt_Identificadores;
            Cls_Cat_Cat_Identificadores_Predio_Negocio Identificadores = new Cls_Cat_Cat_Identificadores_Predio_Negocio();
            Identificadores.P_Cuenta_Predial_Id = Hdf_Cuenta_Predial_Id.Value;
            Dt_Identificadores = Identificadores.Consultar_Identificadores_Predio();
            if (Dt_Identificadores.Rows.Count > 0)
            {
                Txt_Region.Text = Dt_Identificadores.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Region].ToString().Trim();
                Txt_Manzana.Text = Dt_Identificadores.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Manzana].ToString().Trim();
                Txt_Lote.Text = Dt_Identificadores.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Lote].ToString().Trim();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Consultar_Identificadores_Predio: " + Ex.Message);
        }
    }

    private void Busqueda_Propietario()
    {
        DataSet Ds_Prop;
        String Cuenta_Predial_ID = Session["Cuenta_Predial_ID"].ToString().Trim();
        try
        {
            M_Orden_Negocio.P_Cuenta_Predial_ID = Cuenta_Predial_ID;
            Ds_Prop = M_Orden_Negocio.Consulta_Datos_Propietario();
            if (Ds_Prop.Tables[0].Rows.Count > 0)
            {
                Session.Remove("Ds_Prop_Datos");
                Session["Ds_Prop_Datos"] = Ds_Prop;
                Cargar_Datos_Propietario(((DataSet)Session["Ds_Prop_Datos"]).Tables["Dt_Propietarios"]);
            }
        }
        catch (Exception Ex)
        {
            //Mensaje_Error(Ex.Message);
        }

    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos_Propietario
    ///DESCRIPCIÓN: asignar datos de propietario de la cuenta a los controles
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/Ago/2011 06:44:08 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Datos_Propietario(DataTable dataTable)
    {
        try
        {
            if (dataTable.Rows.Count > 0 && dataTable != null)
            {
                //Hdn_Propietario_ID.Value = dataTable.Rows[0]["PROPIETARIO"].ToString();
                M_Orden_Negocio.P_Propietario_ID = dataTable.Rows[0]["PROPIETARIO"].ToString(); ;

                Txt_Propietario.Text = dataTable.Rows[0]["NOMBRE_PROPIETARIO"].ToString();
                M_Orden_Negocio.P_Nombre_Propietario = dataTable.Rows[0]["NOMBRE_PROPIETARIO"].ToString();
                //if (dataTable.Rows[0]["TIPO_PROPIETARIO"].ToString() != "")
                //{
                //    Txt_Propietario_Poseedor_Propietario.Text = dataTable.Rows[0]["TIPO_PROPIETARIO"].ToString();
                //    M_Orden_Negocio.P_Tipo_Propietario = dataTable.Rows[0]["TIPO_PROPIETARIO"].ToString();
                //}

                //Txt_Rfc_Propietario.Text = dataTable.Rows[0]["RFC"].ToString();
                M_Orden_Negocio.P_RFC_Propietario = dataTable.Rows[0]["RFC"].ToString();
            }

        }
        catch (Exception Ex)
        {
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos
    ///DESCRIPCIÓN: asignar datos de cuenta a los controles
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/Ago/2011 06:31:08 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Datos()
    {
        try
        {
            if (!String.IsNullOrEmpty(Txt_Cuenta_Predial.Text.Trim()))
            {
                //KONSULTA DATOS CUENTA HACER DS
                Busqueda_Cuentas();
                //LLENAR CAJAS
                if (Session["Ds_Cuenta_Datos"] != null)
                {
                    Cargar_Generales_Cuenta(((DataSet)Session["Ds_Cuenta_Datos"]).Tables["Dt_Generales"]);
                    Busqueda_Propietario();
                }

            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }

    private void Busqueda_Cuentas()
    {
        Cls_Ope_Pre_Resumen_Predio_Negocio Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        DataSet Ds_Cuenta;
        try
        {
            Resumen_Predio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            Ds_Cuenta = Resumen_Predio.Consulta_Datos_Cuenta_Generales();
            if (Ds_Cuenta.Tables[0].Rows.Count - 1 >= 0)
            {
                if (Ds_Cuenta.Tables[0].Rows[0]["Cuenta_Predial_Id"].ToString().Trim() != string.Empty)
                {
                    Session["Cuenta_Predial_ID"] = Ds_Cuenta.Tables[0].Rows[0]["Cuenta_Predial_Id"].ToString().Trim();
                }
            }
            if (Ds_Cuenta.Tables[0].Rows.Count > 0)
            {
                Session.Remove("Ds_Cuenta_Datos");
                M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
                Session["Ds_Cuenta_Datos"] = Ds_Cuenta;
            }
            else
            {
                //Mensaje_Error("No se encontraron los datos necesarios para la consulta de la cuenta");
                //Lbl_Mensaje_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            //Mensaje_Error(Ex.Message);
        }

    }

    Cls_Ope_Pre_Orden_Variacion_Negocio M_Orden_Negocio;

    private DataTable Crear_Tabla_Caracteristicas_Terreno()
    {
        String Vias_Acceso = "";
        String Fotografia = "";
        String Dens_Construccion = "";
        DataTable Dt_Caracteristicas_Terreno = new DataTable();
        Dt_Caracteristicas_Terreno.Columns.Add("VIAS_ACCESO", typeof(String));
        Dt_Caracteristicas_Terreno.Columns.Add("FOTOGRAFIA", typeof(String));
        Dt_Caracteristicas_Terreno.Columns.Add("DENS_CONST", typeof(String));
        DataRow Dr_Renglon_Nuevo = Dt_Caracteristicas_Terreno.NewRow();

        if (Rdb_Buenas.Checked)
        {
            Vias_Acceso = "BUENA";
        }
        else if (Rdb_Regulares.Checked)
        {
            Vias_Acceso = "REGULAR";
        }
        else if (Rdb_Malas.Checked)
        {
            Vias_Acceso = "MALA";
        }

        if (Rdb_Plana.Checked)
        {
            Fotografia = "PLANA";
        }
        else if (Rdb_Pendiente.Checked)
        {
            Fotografia = "PENDIENTE";
        }
        Dens_Construccion = Txt_Dens_Construccion.Text.Trim();
        Dr_Renglon_Nuevo["VIAS_ACCESO"] = Vias_Acceso;
        Dr_Renglon_Nuevo["FOTOGRAFIA"] = Fotografia;
        Dr_Renglon_Nuevo["DENS_CONST"] = Dens_Construccion;
        Dt_Caracteristicas_Terreno.Rows.Add(Dr_Renglon_Nuevo);
        return Dt_Caracteristicas_Terreno;
    }

    private DataTable Crear_Tabla_Construccion_Terreno()
    {
        String Tipo_Construccion = "";
        String Calidad_Proyecto = "";
        String Uso_Construccion = "";
        DataTable Dt_Construccion_Terreno = new DataTable();
        Dt_Construccion_Terreno.Columns.Add("TIPO_CONSTRUCCION", typeof(String));
        Dt_Construccion_Terreno.Columns.Add("CALIDAD_PROYECTO", typeof(String));
        Dt_Construccion_Terreno.Columns.Add("USO_CONSTRUCCION", typeof(String));
        DataRow Dr_Renglon_Nuevo = Dt_Construccion_Terreno.NewRow();
        if (Rdb_Nueva.Checked)
        {
            Tipo_Construccion = "NUEVA";
        }
        if (Rdb_Ampliacion.Checked)
        {
            Tipo_Construccion = "AMPLIACION";
        }
        if (Rdb_Remodelacion.Checked)
        {
            Tipo_Construccion = "REMODELACION";
        }
        if (Rdb_Rentada.Checked)
        {
            Tipo_Construccion = "RENTADA";
        }

        if (Rdb_Calidad_Buena.Checked)
        {
            Calidad_Proyecto += "BUENA";
        }
        if (Rdb_Calidad_Mala.Checked)
        {
            Calidad_Proyecto += "MALA";
        }
        if (Rdb_Calidad_Regular.Checked)
        {
            Calidad_Proyecto += "REGULAR";
        }

        Uso_Construccion = Txt_Uso.Text.ToUpper();
        Dr_Renglon_Nuevo["TIPO_CONSTRUCCION"] = Tipo_Construccion;
        Dr_Renglon_Nuevo["CALIDAD_PROYECTO"] = Calidad_Proyecto;
        Dr_Renglon_Nuevo["USO_CONSTRUCCION"] = Uso_Construccion;
        Dt_Construccion_Terreno.Rows.Add(Dr_Renglon_Nuevo);
        return Dt_Construccion_Terreno;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Calcular_Valor_Total_Predio
    ///DESCRIPCIÓN: Cálcula el valor total del predio
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    private void Calcular_Valor_Total_Predio()
    {
        try
        {
            Double Diferencia_Anual = 0;
            Double Couta_Anual_Minima = 0;
            Double Cuota_Anual_Anterior = 0;
            Double Cuota_Anual_Actual = 0;
            Cls_Cat_Pre_Cuotas_Minimas_Negocio CuentasMin = new Cls_Cat_Pre_Cuotas_Minimas_Negocio();
            DataTable Cuotas = new DataTable();
            CuentasMin.P_Anio = DateTime.Now.Year.ToString();
            Cuotas = CuentasMin.Consultar_Cuotas_Minimas();
            Couta_Anual_Minima = Convert.ToDouble(Cuotas.Rows[0]["CUOTA"].ToString().Trim());
            Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio Tasas = new Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio();
            Tasas.P_Anio_Tasa = DateTime.Now.Year.ToString();
            Double Valor_Construccion = 0;
            Double Valor_Terreno = 0;
            Double Valor_Total_Predio = 0;
            Valor_Construccion = Convert.ToDouble(Txt_Construccion_Valor_Total.Text);
            Valor_Terreno = Convert.ToDouble(Txt_Terreno_Valor_Total.Text);
            Valor_Total_Predio = Valor_Construccion + Valor_Terreno;
            Txt_Valor_Total_Predio.Text = Valor_Total_Predio.ToString("###,###,###,###,##0.00");
            if (Txt_Construccion_Superficie_Total.Text.Trim() != "0.00" && Txt_Construccion_Superficie_Total.Text.Trim() != "")
            {
                Tasas.P_Identificador_Tasa = "UR";
                Txt_Cuota_Anual_Actual.Text = (Valor_Total_Predio * (Convert.ToDouble(Tasas.Consultar_Tasas_Anuales().Rows[0]
                    ["TASA_ANUAL"].ToString()) / 1000)).ToString("###,###,###,###,##0.00");
            }
            else
            {
                Tasas.P_Identificador_Tasa = "USE";
                Txt_Cuota_Anual_Actual.Text = (Valor_Total_Predio * (Convert.ToDouble(Tasas.Consultar_Tasas_Anuales().Rows[0]
                    ["TASA_ANUAL"].ToString()) / 1000)).ToString("###,###,###,###,##0.00");
            }
            Cuota_Anual_Actual = Convert.ToDouble(Txt_Cuota_Anual_Actual.Text);
            if (Cuota_Anual_Actual < Couta_Anual_Minima)
            {
                Txt_Cuota_Anual_Actual.Text = Couta_Anual_Minima.ToString("###,###,###,###,##0.00");
                Cuota_Anual_Actual = Couta_Anual_Minima;
            }
            else
            {
                Txt_Cuota_Anual_Actual.Text = Cuota_Anual_Actual.ToString("###,###,###,###,##0.00");
            }
            Cuota_Anual_Anterior = Convert.ToDouble(Txt_Cuota_Anual_Anterior.Text);
            Diferencia_Anual = Cuota_Anual_Actual - Cuota_Anual_Anterior;
            if (Diferencia_Anual < 0)
            {
                // this.Txt_Diferencia_Anual.Enabled = !true;
                this.Txt_Diferencia_Anual.BackColor = System.Drawing.Color.Red;
            }
            if (Diferencia_Anual > 0)
            {
                this.Txt_Diferencia_Anual.BackColor = System.Drawing.Color.White;
            }
            Txt_Diferencia_Anual.Text = Diferencia_Anual.ToString("###,###,###,###,##0.00");
        }
        catch
        {
            throw new Exception("Error al consultar las tasas y/o las cuotas mínimas del año " + DateTime.Now.Year + ", faltan datos.");
        }
    }


    protected void Txt_Dens_Construccion_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Txt_Dens_Construccion.Text.Trim() == "")
            {
                Txt_Dens_Construccion.Text = "0.00";
            }
            else
            {
                Txt_Dens_Construccion.Text = Convert.ToDouble(Txt_Dens_Construccion.Text).ToString("##0.00");
            }
        }
        catch
        {
            Txt_Dens_Construccion.Text = "0.00";
        }
    }

    protected void Btn_Agregar_Observacion_Click(object sender, ImageClickEventArgs e)
    {
        if (Cmb_Motivos_Rechazo.SelectedValue != "SELECCIONE")
        {
            DataTable Dt_Motivos_Rechazo = (DataTable)Session["Dt_Motivos_Rechazo"];
            Boolean Existe_Registro = false;
            if (Dt_Motivos_Rechazo.Rows.Count == 0)
            {
                DataRow Dr_Nuevo_Registro = Dt_Motivos_Rechazo.NewRow();
                Dr_Nuevo_Registro["NO_SEGUIMIENTO"] = " ";
                Dr_Nuevo_Registro["MOTIVO_ID"] = Cmb_Motivos_Rechazo.SelectedValue;
                Dr_Nuevo_Registro["ESTATUS"] = "VIGENTE";
                Dr_Nuevo_Registro["ACCION"] = "ALTA";
                Dr_Nuevo_Registro["USUARIO_CREO"] = Cls_Sessiones.Nombre_Empleado;
                Dr_Nuevo_Registro["FECHA_CREO"] = DateTime.Now;
                Dr_Nuevo_Registro["MOTIVO_DESCRIPCION"] = Cmb_Motivos_Rechazo.SelectedItem.Text;
                Dt_Motivos_Rechazo.Rows.Add(Dr_Nuevo_Registro);
                Session["Dt_Motivos_Rechazo"] = Dt_Motivos_Rechazo.Copy();
                Dt_Motivos_Rechazo.DefaultView.RowFilter = "ESTATUS='VIGENTE'";
                Grid_Observaciones.Columns[1].Visible = true;
                Grid_Observaciones.Columns[2].Visible = true;
                Grid_Observaciones.Columns[4].Visible = true;
                Grid_Observaciones.DataSource = Dt_Motivos_Rechazo;
                Grid_Observaciones.PageIndex = Grid_Observaciones.PageIndex;
                Grid_Observaciones.DataBind();
                Grid_Observaciones.Columns[1].Visible = false;
                Grid_Observaciones.Columns[2].Visible = false;
                Grid_Observaciones.Columns[4].Visible = false;
                Grid_Observaciones.SelectedIndex = -1;
            }
            else
            {
                foreach (DataRow Dr_Renglon in Dt_Motivos_Rechazo.Rows)
                {
                    if (Dr_Renglon["MOTIVO_ID"].ToString() == Cmb_Motivos_Rechazo.SelectedValue && Dr_Renglon["ESTATUS"].ToString() != "BAJA")
                    {
                        Existe_Registro = true;
                    }
                }
                if (!Existe_Registro)
                {
                    DataRow Dr_Nuevo_Registro = Dt_Motivos_Rechazo.NewRow();
                    Dr_Nuevo_Registro["NO_SEGUIMIENTO"] = " ";
                    Dr_Nuevo_Registro["MOTIVO_ID"] = Cmb_Motivos_Rechazo.SelectedValue;
                    Dr_Nuevo_Registro["ESTATUS"] = "VIGENTE";
                    Dr_Nuevo_Registro["ACCION"] = "ALTA";
                    Dr_Nuevo_Registro["USUARIO_CREO"] = Cls_Sessiones.Nombre_Empleado;
                    Dr_Nuevo_Registro["FECHA_CREO"] = DateTime.Now;
                    Dr_Nuevo_Registro["MOTIVO_DESCRIPCION"] = Cmb_Motivos_Rechazo.SelectedItem.Text;
                    Dt_Motivos_Rechazo.Rows.Add(Dr_Nuevo_Registro);
                    Session["Dt_Motivos_Rechazo"] = Dt_Motivos_Rechazo.Copy();
                    Dt_Motivos_Rechazo.DefaultView.RowFilter = "ESTATUS='VIGENTE'";
                    Dt_Motivos_Rechazo.DefaultView.Sort = "ESTATUS DESC";
                    Grid_Observaciones.Columns[1].Visible = true;
                    Grid_Observaciones.Columns[2].Visible = true;
                    Grid_Observaciones.Columns[4].Visible = true;
                    Grid_Observaciones.DataSource = Dt_Motivos_Rechazo;
                    Grid_Observaciones.PageIndex = Grid_Observaciones.PageIndex;
                    Grid_Observaciones.DataBind();
                    Grid_Observaciones.Columns[1].Visible = false;
                    Grid_Observaciones.Columns[2].Visible = false;
                    Grid_Observaciones.Columns[4].Visible = false;
                    Grid_Observaciones.SelectedIndex = -1;
                }
            }
        }
        Grid_Documentos.Columns[0].Visible = true;
        Grid_Documentos.Columns[1].Visible = true;
        Grid_Documentos.DataSource = (DataTable)Session["Dt_Documentos"];
        Grid_Documentos.DataBind();
        Grid_Documentos.Columns[0].Visible = false;
        Grid_Documentos.Columns[1].Visible = false;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Documentos_DataBound
    ///DESCRIPCIÓN: Carga los componentes del grid
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Documentos_DataBound(object sender, EventArgs e)
    {
        Int16 i = 0;
        DataTable Dt_Documentos = (DataTable)Session["Dt_Documentos"];
        foreach (DataRow Dr_Renglon in Dt_Documentos.Rows)
        {
            if (Dr_Renglon["ACCION"].ToString() == "NADA")
            {
                //Label Lbl_Url_Temporal = (Label)Grid_Documentos.Rows[i].Cells[3].FindControl("Lbl_Url");
                if (File.Exists(Server.MapPath(Dr_Renglon["RUTA_DOCUMENTO"].ToString())))
                {
                    HyperLink Hlk_Enlace = new HyperLink();
                    Hlk_Enlace.Text = Path.GetFileName(Dr_Renglon["RUTA_DOCUMENTO"].ToString());
                    Hlk_Enlace.NavigateUrl = Dr_Renglon["RUTA_DOCUMENTO"].ToString();
                    Hlk_Enlace.CssClass = "enlace_fotografia";
                    Hlk_Enlace.Target = "blank";
                    //e.Row.Cells[3].Controls.Add(Hlk_Enlace);
                    Grid_Documentos.Rows[i].Cells[3].Controls.Add(Hlk_Enlace);
                    i++;
                }
            }
            else if (Dr_Renglon["ACCION"].ToString() == "ALTA")
            {
                Label Lbl_Guardar = new Label();
                Lbl_Guardar.Text = "Guardar para poder visualizar este archivo";
                Grid_Documentos.Rows[i].Cells[3].Controls.Add(Lbl_Guardar);
                i++;
            }
        }
    }

    //protected void Grid_Colindancias_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    DataTable Dt_Medidas = (DataTable)Session["Dt_Medidas"];
    //    if (Dt_Medidas != null)
    //    {
    //        Dt_Medidas.DefaultView.RowFilter = "ACCION <> 'BAJA'";
    //        Grid_Colindancias.Columns[0].Visible = true;
    //        Grid_Colindancias.DataSource = Dt_Medidas;
    //        Grid_Colindancias.DataBind();
    //        Grid_Colindancias.Columns[0].Visible = false;

    //    }
    //}

    protected void Btn_Eliminar_Observacion_Click(object sender, ImageClickEventArgs e)
    {
        if (Grid_Observaciones.SelectedIndex > -1)
        {
            DataTable Dt_Motivos_Rechazo = (DataTable)Session["Dt_Motivos_Rechazo"];
            int Indice = (Grid_Observaciones.PageIndex * 10) + Grid_Observaciones.SelectedIndex;
            Dt_Motivos_Rechazo.Rows[Indice]["ESTATUS"] = "BAJA";
            if (Dt_Motivos_Rechazo.Rows[Indice]["NO_SEGUIMIENTO"].ToString().Trim() == "")
            {
                Dt_Motivos_Rechazo.Rows[Indice]["ACCION"] = "ALTA";
            }
            else
            {
                Dt_Motivos_Rechazo.Rows[Indice]["ACCION"] = "ACTUALIZAR";
            }
            Session["Dt_Motivos_Rechazo"] = Dt_Motivos_Rechazo.Copy();
            Dt_Motivos_Rechazo.DefaultView.Sort = "ESTATUS DESC";
            Dt_Motivos_Rechazo.DefaultView.RowFilter = "ESTATUS='VIGENTE'";
            Grid_Observaciones.Columns[1].Visible = true;
            Grid_Observaciones.Columns[2].Visible = true;
            Grid_Observaciones.DataSource = Dt_Motivos_Rechazo;
            Grid_Observaciones.PageIndex = Grid_Observaciones.PageIndex;
            Grid_Observaciones.DataBind();
            Grid_Observaciones.Columns[1].Visible = false;
            Grid_Observaciones.Columns[2].Visible = false;
            Grid_Observaciones.SelectedIndex = -1;
            if (Cmb_Estatus.SelectedValue == "POR VALIDAR")
            {
                Cambiar_Estatus_Avaluo_Rechazado_Autorizada();
            }
        }
        Grid_Documentos.Columns[0].Visible = true;
        Grid_Documentos.Columns[1].Visible = true;
        Grid_Documentos.DataSource = (DataTable)Session["Dt_Documentos"];
        Grid_Documentos.DataBind();
        Grid_Documentos.Columns[0].Visible = false;
        Grid_Documentos.Columns[1].Visible = false;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Observaciones_PageIndexChanging
    ///DESCRIPCIÓN: Cambia la página del grid
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Grid_Observaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            DataTable Dt_Observaciones = (DataTable)Session["Dt_Motivos_Rechazo"];
            Grid_Observaciones.SelectedIndex = -1;
            Grid_Observaciones.Columns[1].Visible = true;
            Grid_Observaciones.Columns[2].Visible = true;
            Grid_Observaciones.Columns[4].Visible = true;
            Dt_Observaciones.DefaultView.Sort = "ESTATUS DESC";
            Grid_Observaciones.DataSource = Dt_Observaciones;
            Grid_Observaciones.PageIndex = e.NewPageIndex;
            Grid_Observaciones.DataBind();
            Grid_Observaciones.Columns[1].Visible = false;
            Grid_Observaciones.Columns[2].Visible = false;
            Grid_Observaciones.Columns[4].Visible = false;
        }
        catch (Exception ex)
        {
            Lbl_Ecabezado_Mensaje.Text = ex.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    protected void Grid_Observaciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Observaciones.SelectedIndex > -1)
        {
            try
            {
                //Grid_Observaciones.DataSource = (DataTable)Session["Dt_Motivos_Rechazo"];
                //Grid_Observaciones.PageIndex = e.NewPageIndex;
                //Grid_Observaciones.DataBind();
                Hdf_Motivo_Id.Value = Grid_Observaciones.SelectedRow.Cells[2].Text;
                Cmb_Motivos_Rechazo.SelectedIndex = Cmb_Motivos_Rechazo.Items.IndexOf(Cmb_Motivos_Rechazo.Items.FindByValue(HttpUtility.HtmlDecode(Grid_Observaciones.SelectedRow.Cells[2].Text)));
            }
            catch (Exception ex)
            {
                Lbl_Ecabezado_Mensaje.Text = ex.Message.ToString();
                Lbl_Ecabezado_Mensaje.Visible = true;
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    }

    private void Cambiar_Estatus_Avaluo_Rechazado_Autorizada()
    {
        DataTable Dt_Motivos_Rechazo = (DataTable)Session["Dt_Motivos_Rechazo"];
        Int16 Registros = 0;
        foreach (DataRow Dr_Renglon in Dt_Motivos_Rechazo.Rows)
        {
            if (Dr_Renglon["ESTATUS"].ToString() == "VIGENTE")
            {
                Registros++;
            }
        }
        if (Registros > 0)
        {
            Cmb_Estatus.SelectedValue = "RECHAZADO";
        }
        else
        {
            Cmb_Estatus.SelectedValue = "POR AUTORIZAR";
        }
    }

    protected void Grid_Clasificacion_Zona_DataBound(object sender, EventArgs e)
    {
        DataTable Dt_Clasificacion_Zona = (DataTable)Session["Dt_Grid_Clasificacion_Zona"];
        for (int i = 0; i < Dt_Clasificacion_Zona.Rows.Count; i++)
        {
            CheckBox Chk_Columna_A_Temporal = (CheckBox)Grid_Clasificacion_Zona.Rows[i].Cells[1].FindControl("Chk_Columna_A");
            CheckBox Chk_Columna_B_Temporal = (CheckBox)Grid_Clasificacion_Zona.Rows[i].Cells[3].FindControl("Chk_Columna_B");
            CheckBox Chk_Columna_C_Temporal = (CheckBox)Grid_Clasificacion_Zona.Rows[i].Cells[5].FindControl("Chk_Columna_C");
            CheckBox Chk_Columna_D_Temporal = (CheckBox)Grid_Clasificacion_Zona.Rows[i].Cells[7].FindControl("Chk_Columna_D");
            if (Dt_Clasificacion_Zona.Rows[i]["COLUMNA_A_ID"].ToString().Trim() != "")
            {
                if (Dt_Clasificacion_Zona.Rows[i]["COLUMNA_A_VALOR"].ToString().Trim() != "")
                {
                    Chk_Columna_A_Temporal.Checked = true;
                }
                else
                {
                    Chk_Columna_A_Temporal.Checked = false;
                }
            }
            else
            {
                Chk_Columna_A_Temporal.Visible = false;
            }
            if (Dt_Clasificacion_Zona.Rows[i]["COLUMNA_B_ID"].ToString().Trim() != "")
            {
                if (Dt_Clasificacion_Zona.Rows[i]["COLUMNA_B_VALOR"].ToString().Trim() != "")
                {
                    Chk_Columna_B_Temporal.Checked = true;
                }
                else
                {
                    Chk_Columna_B_Temporal.Checked = false;
                }
            }
            else
            {
                Chk_Columna_B_Temporal.Visible = false;
            }
            if (Dt_Clasificacion_Zona.Rows[i]["COLUMNA_C_ID"].ToString().Trim() != "")
            {
                if (Dt_Clasificacion_Zona.Rows[i]["COLUMNA_C_VALOR"].ToString().Trim() != "")
                {
                    Chk_Columna_C_Temporal.Checked = true;
                }
                else
                {
                    Chk_Columna_C_Temporal.Checked = false;
                }
            }
            else
            {
                Chk_Columna_C_Temporal.Visible = false;
            }
            if (Dt_Clasificacion_Zona.Rows[i]["COLUMNA_D_ID"].ToString().Trim() != "")
            {
                if (Dt_Clasificacion_Zona.Rows[i]["COLUMNA_D_VALOR"].ToString().Trim() != "")
                {
                    Chk_Columna_D_Temporal.Checked = true;
                }
                else
                {
                    Chk_Columna_D_Temporal.Checked = false;
                }
            }
            else
            {
                Chk_Columna_D_Temporal.Visible = false;
            }
        }
    }

    private void Guardar_Grid_Clasificacion_Zona()
    {
        DataTable Dt_Clasificacion_Zona = (DataTable)Session["Dt_Grid_Clasificacion_Zona"];
        for (int i = 0; i < Dt_Clasificacion_Zona.Rows.Count; i++)
        {
            CheckBox Chk_Columna_A_Temporal = (CheckBox)Grid_Clasificacion_Zona.Rows[i].Cells[1].FindControl("Chk_Columna_A");
            CheckBox Chk_Columna_B_Temporal = (CheckBox)Grid_Clasificacion_Zona.Rows[i].Cells[3].FindControl("Chk_Columna_B");
            CheckBox Chk_Columna_C_Temporal = (CheckBox)Grid_Clasificacion_Zona.Rows[i].Cells[5].FindControl("Chk_Columna_C");
            CheckBox Chk_Columna_D_Temporal = (CheckBox)Grid_Clasificacion_Zona.Rows[i].Cells[7].FindControl("Chk_Columna_D");
            if (Dt_Clasificacion_Zona.Rows[i]["COLUMNA_A_ID"].ToString().Trim() != "")
            {
                if (Chk_Columna_A_Temporal.Checked == true)
                {
                    Dt_Clasificacion_Zona.Rows[i]["COLUMNA_A_VALOR"] = "X";
                }
                else
                {
                    Dt_Clasificacion_Zona.Rows[i]["COLUMNA_A_VALOR"] = "";
                }
            }
            if (Dt_Clasificacion_Zona.Rows[i]["COLUMNA_B_ID"].ToString().Trim() != "")
            {
                if (Chk_Columna_B_Temporal.Checked == true)
                {
                    Dt_Clasificacion_Zona.Rows[i]["COLUMNA_B_VALOR"] = "X";
                }
                else
                {
                    Dt_Clasificacion_Zona.Rows[i]["COLUMNA_B_VALOR"] = "";
                }
            }
            if (Dt_Clasificacion_Zona.Rows[i]["COLUMNA_C_ID"].ToString().Trim() != "")
            {
                if (Chk_Columna_C_Temporal.Checked == true)
                {
                    Dt_Clasificacion_Zona.Rows[i]["COLUMNA_C_VALOR"] = "X";
                }
                else
                {
                    Dt_Clasificacion_Zona.Rows[i]["COLUMNA_C_VALOR"] = "";
                }
            }
            if (Dt_Clasificacion_Zona.Rows[i]["COLUMNA_D_ID"].ToString().Trim() != "")
            {
                if (Chk_Columna_D_Temporal.Checked == true)
                {
                    Dt_Clasificacion_Zona.Rows[i]["COLUMNA_D_VALOR"] = "X";
                }
                else
                {
                    Dt_Clasificacion_Zona.Rows[i]["COLUMNA_D_VALOR"] = "";
                }
            }
        }
    }

    protected void Grid_Servicios_Zona_DataBound(object sender, EventArgs e)
    {
        DataTable Dt_Servicios_Zona = (DataTable)Session["Dt_Grid_Servicios_Zona"];
        for (int i = 0; i < Dt_Servicios_Zona.Rows.Count; i++)
        {
            CheckBox Chk_Columna_A_Temporal = (CheckBox)Grid_Servicios_Zona.Rows[i].Cells[1].FindControl("Chk_Columna_A");
            CheckBox Chk_Columna_B_Temporal = (CheckBox)Grid_Servicios_Zona.Rows[i].Cells[3].FindControl("Chk_Columna_B");
            CheckBox Chk_Columna_C_Temporal = (CheckBox)Grid_Servicios_Zona.Rows[i].Cells[5].FindControl("Chk_Columna_C");
            CheckBox Chk_Columna_D_Temporal = (CheckBox)Grid_Servicios_Zona.Rows[i].Cells[7].FindControl("Chk_Columna_D");
            if (Dt_Servicios_Zona.Rows[i]["COLUMNA_A_ID"].ToString().Trim() != "")
            {
                if (Dt_Servicios_Zona.Rows[i]["COLUMNA_A_VALOR"].ToString().Trim() != "")
                {
                    Chk_Columna_A_Temporal.Checked = true;
                }
                else
                {
                    Chk_Columna_A_Temporal.Checked = false;
                }
            }
            else
            {
                Chk_Columna_A_Temporal.Visible = false;
            }
            if (Dt_Servicios_Zona.Rows[i]["COLUMNA_B_ID"].ToString().Trim() != "")
            {
                if (Dt_Servicios_Zona.Rows[i]["COLUMNA_B_VALOR"].ToString().Trim() != "")
                {
                    Chk_Columna_B_Temporal.Checked = true;
                }
                else
                {
                    Chk_Columna_B_Temporal.Checked = false;
                }
            }
            else
            {
                Chk_Columna_B_Temporal.Visible = false;
            }
            if (Dt_Servicios_Zona.Rows[i]["COLUMNA_C_ID"].ToString().Trim() != "")
            {
                if (Dt_Servicios_Zona.Rows[i]["COLUMNA_C_VALOR"].ToString().Trim() != "")
                {
                    Chk_Columna_C_Temporal.Checked = true;
                }
                else
                {
                    Chk_Columna_C_Temporal.Checked = false;
                }
            }
            else
            {
                Chk_Columna_C_Temporal.Visible = false;
            }
            if (Dt_Servicios_Zona.Rows[i]["COLUMNA_D_ID"].ToString().Trim() != "")
            {
                if (Dt_Servicios_Zona.Rows[i]["COLUMNA_D_VALOR"].ToString().Trim() != "")
                {
                    Chk_Columna_D_Temporal.Checked = true;
                }
                else
                {
                    Chk_Columna_D_Temporal.Checked = false;
                }
            }
            else
            {
                Chk_Columna_D_Temporal.Visible = false;
            }
        }
    }

    private void Guardar_Grid_Servicios_Zona()
    {
        DataTable Dt_Servicios_Zona = (DataTable)Session["Dt_Grid_Servicios_Zona"];
        for (int i = 0; i < Dt_Servicios_Zona.Rows.Count; i++)
        {
            CheckBox Chk_Columna_A_Temporal = (CheckBox)Grid_Servicios_Zona.Rows[i].Cells[1].FindControl("Chk_Columna_A");
            CheckBox Chk_Columna_B_Temporal = (CheckBox)Grid_Servicios_Zona.Rows[i].Cells[3].FindControl("Chk_Columna_B");
            CheckBox Chk_Columna_C_Temporal = (CheckBox)Grid_Servicios_Zona.Rows[i].Cells[5].FindControl("Chk_Columna_C");
            CheckBox Chk_Columna_D_Temporal = (CheckBox)Grid_Servicios_Zona.Rows[i].Cells[7].FindControl("Chk_Columna_D");
            if (Dt_Servicios_Zona.Rows[i]["COLUMNA_A_ID"].ToString().Trim() != "")
            {
                if (Chk_Columna_A_Temporal.Checked == true)
                {
                    Dt_Servicios_Zona.Rows[i]["COLUMNA_A_VALOR"] = "X";
                }
                else
                {
                    Dt_Servicios_Zona.Rows[i]["COLUMNA_A_VALOR"] = "";
                }
            }
            if (Dt_Servicios_Zona.Rows[i]["COLUMNA_B_ID"].ToString().Trim() != "")
            {
                if (Chk_Columna_B_Temporal.Checked == true)
                {
                    Dt_Servicios_Zona.Rows[i]["COLUMNA_B_VALOR"] = "X";
                }
                else
                {
                    Dt_Servicios_Zona.Rows[i]["COLUMNA_B_VALOR"] = "";
                }
            }
            if (Dt_Servicios_Zona.Rows[i]["COLUMNA_C_ID"].ToString().Trim() != "")
            {
                if (Chk_Columna_C_Temporal.Checked == true)
                {
                    Dt_Servicios_Zona.Rows[i]["COLUMNA_C_VALOR"] = "X";
                }
                else
                {
                    Dt_Servicios_Zona.Rows[i]["COLUMNA_C_VALOR"] = "";
                }
            }
            if (Dt_Servicios_Zona.Rows[i]["COLUMNA_D_ID"].ToString().Trim() != "")
            {
                if (Chk_Columna_D_Temporal.Checked == true)
                {
                    Dt_Servicios_Zona.Rows[i]["COLUMNA_D_VALOR"] = "X";
                }
                else
                {
                    Dt_Servicios_Zona.Rows[i]["COLUMNA_D_VALOR"] = "";
                }
            }
        }
    }

    protected void Grid_Construccion_Dominante_DataBound(object sender, EventArgs e)
    {
        DataTable Dt_Construccion_Dominante = (DataTable)Session["Dt_Grid_Construccion_Dominante"];
        for (int i = 0; i < Dt_Construccion_Dominante.Rows.Count; i++)
        {
            RadioButton Rdb_Columna_A_Temporal = (RadioButton)Grid_Construccion_Dominante.Rows[i].Cells[1].FindControl("Rdb_Columna_A");
            RadioButton Rdb_Columna_B_Temporal = (RadioButton)Grid_Construccion_Dominante.Rows[i].Cells[3].FindControl("Rdb_Columna_B");
            RadioButton Rdb_Columna_C_Temporal = (RadioButton)Grid_Construccion_Dominante.Rows[i].Cells[5].FindControl("Rdb_Columna_C");
            if (Dt_Construccion_Dominante.Rows[i]["COLUMNA_A_ID"].ToString().Trim() != "")
            {
                if (Dt_Construccion_Dominante.Rows[i]["COLUMNA_A_VALOR"].ToString().Trim() != "")
                {
                    Rdb_Columna_A_Temporal.Checked = true;
                }
                else
                {
                    Rdb_Columna_A_Temporal.Checked = false;
                }
            }
            else
            {
                Rdb_Columna_A_Temporal.Visible = false;
            }
            if (Dt_Construccion_Dominante.Rows[i]["COLUMNA_B_ID"].ToString().Trim() != "")
            {
                if (Dt_Construccion_Dominante.Rows[i]["COLUMNA_B_VALOR"].ToString().Trim() != "")
                {
                    Rdb_Columna_B_Temporal.Checked = true;
                }
                else
                {
                    Rdb_Columna_B_Temporal.Checked = false;
                }
            }
            else
            {
                Rdb_Columna_B_Temporal.Visible = false;
            }
            if (Dt_Construccion_Dominante.Rows[i]["COLUMNA_C_ID"].ToString().Trim() != "")
            {
                if (Dt_Construccion_Dominante.Rows[i]["COLUMNA_C_VALOR"].ToString().Trim() != "")
                {
                    Rdb_Columna_C_Temporal.Checked = true;
                }
                else
                {
                    Rdb_Columna_C_Temporal.Checked = false;
                }
            }
            else
            {
                Rdb_Columna_C_Temporal.Visible = false;
            }
        }
    }

    protected void Guardar_Grid_Construccion_Dominante()
    {
        DataTable Dt_Construccion_Dominante = (DataTable)Session["Dt_Grid_Construccion_Dominante"];
        for (int i = 0; i < Dt_Construccion_Dominante.Rows.Count; i++)
        {
            RadioButton Rdb_Columna_A_Temporal = (RadioButton)Grid_Construccion_Dominante.Rows[i].Cells[1].FindControl("Rdb_Columna_A");
            RadioButton Rdb_Columna_B_Temporal = (RadioButton)Grid_Construccion_Dominante.Rows[i].Cells[3].FindControl("Rdb_Columna_B");
            RadioButton Rdb_Columna_C_Temporal = (RadioButton)Grid_Construccion_Dominante.Rows[i].Cells[5].FindControl("Rdb_Columna_C");
            if (Dt_Construccion_Dominante.Rows[i]["COLUMNA_A_ID"].ToString().Trim() != "")
            {
                if (Rdb_Columna_A_Temporal.Checked == true)
                {
                    Dt_Construccion_Dominante.Rows[i]["COLUMNA_A_VALOR"] = "X";
                }
                else
                {
                    Dt_Construccion_Dominante.Rows[i]["COLUMNA_A_VALOR"] = "";
                }
            }
            if (Dt_Construccion_Dominante.Rows[i]["COLUMNA_B_ID"].ToString().Trim() != "")
            {
                if (Rdb_Columna_B_Temporal.Checked == true)
                {
                    Dt_Construccion_Dominante.Rows[i]["COLUMNA_B_VALOR"] = "X";
                }
                else
                {
                    Dt_Construccion_Dominante.Rows[i]["COLUMNA_B_VALOR"] = "";
                }
            }
            if (Dt_Construccion_Dominante.Rows[i]["COLUMNA_C_ID"].ToString().Trim() != "")
            {
                if (Rdb_Columna_C_Temporal.Checked == true)
                {
                    Dt_Construccion_Dominante.Rows[i]["COLUMNA_C_VALOR"] = "X";
                }
                else
                {
                    Dt_Construccion_Dominante.Rows[i]["COLUMNA_C_VALOR"] = "";
                }
            }
        }
    }

    private void Crear_Tabla_Clasificacion_Zona(DataTable Dt_Clasificacion_Zona)
    {
        DataTable Dt_Clasificacion_Zona_Avaluo = new DataTable();
        DataRow Dr_Renglon_Nuevo;
        Int16 i = 0;
        int Contador_Renglones = 1;
        Dt_Clasificacion_Zona_Avaluo.Columns.Add("COLUMNA_A", typeof(String));
        Dt_Clasificacion_Zona_Avaluo.Columns.Add("COLUMNA_A_ID", typeof(String));
        Dt_Clasificacion_Zona_Avaluo.Columns.Add("COLUMNA_A_VALOR", typeof(String));

        Dt_Clasificacion_Zona_Avaluo.Columns.Add("COLUMNA_B", typeof(String));
        Dt_Clasificacion_Zona_Avaluo.Columns.Add("COLUMNA_B_ID", typeof(String));
        Dt_Clasificacion_Zona_Avaluo.Columns.Add("COLUMNA_B_VALOR", typeof(String));

        Dt_Clasificacion_Zona_Avaluo.Columns.Add("COLUMNA_C", typeof(String));
        Dt_Clasificacion_Zona_Avaluo.Columns.Add("COLUMNA_C_ID", typeof(String));
        Dt_Clasificacion_Zona_Avaluo.Columns.Add("COLUMNA_C_VALOR", typeof(String));

        Dt_Clasificacion_Zona_Avaluo.Columns.Add("COLUMNA_D", typeof(String));
        Dt_Clasificacion_Zona_Avaluo.Columns.Add("COLUMNA_D_ID", typeof(String));
        Dt_Clasificacion_Zona_Avaluo.Columns.Add("COLUMNA_D_VALOR", typeof(String));
        Dr_Renglon_Nuevo = Dt_Clasificacion_Zona_Avaluo.NewRow();
        foreach (DataRow Dr_Renglon_Actual in Dt_Clasificacion_Zona.Rows)
        {
            if (i == 0)
            {
                Dr_Renglon_Nuevo["COLUMNA_A"] = Dr_Renglon_Actual[Cat_Cat_Clasificacion_Zona.Campo_Clasificacion_Zona].ToString();
                Dr_Renglon_Nuevo["COLUMNA_A_ID"] = Dr_Renglon_Actual[Cat_Cat_Clasificacion_Zona.Campo_Clasificacion_Zona_Id].ToString();
                Dr_Renglon_Nuevo["COLUMNA_A_VALOR"] = Dr_Renglon_Actual["VALOR_CLASIFICACION_ZONA"].ToString();
                if (Contador_Renglones == Dt_Clasificacion_Zona.Rows.Count)
                {
                    Dr_Renglon_Nuevo["COLUMNA_B"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_B_ID"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_B_VALOR"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_C"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_C_ID"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_C_VALOR"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_D"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_D_ID"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_D_VALOR"] = "";
                    Dt_Clasificacion_Zona_Avaluo.Rows.Add(Dr_Renglon_Nuevo);
                    break;
                }
                i++;
            }
            else if (i == 1)
            {
                Dr_Renglon_Nuevo["COLUMNA_B"] = Dr_Renglon_Actual[Cat_Cat_Clasificacion_Zona.Campo_Clasificacion_Zona].ToString();
                Dr_Renglon_Nuevo["COLUMNA_B_ID"] = Dr_Renglon_Actual[Cat_Cat_Clasificacion_Zona.Campo_Clasificacion_Zona_Id].ToString();
                Dr_Renglon_Nuevo["COLUMNA_B_VALOR"] = Dr_Renglon_Actual["VALOR_CLASIFICACION_ZONA"].ToString();
                if (Contador_Renglones == Dt_Clasificacion_Zona.Rows.Count)
                {
                    Dr_Renglon_Nuevo["COLUMNA_C"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_C_ID"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_C_VALOR"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_D"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_D_ID"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_D_VALOR"] = "";
                    Dt_Clasificacion_Zona_Avaluo.Rows.Add(Dr_Renglon_Nuevo);
                    break;
                }
                i++;
            }
            else if (i == 2)
            {
                Dr_Renglon_Nuevo["COLUMNA_C"] = Dr_Renglon_Actual[Cat_Cat_Clasificacion_Zona.Campo_Clasificacion_Zona].ToString();
                Dr_Renglon_Nuevo["COLUMNA_C_ID"] = Dr_Renglon_Actual[Cat_Cat_Clasificacion_Zona.Campo_Clasificacion_Zona_Id].ToString();
                Dr_Renglon_Nuevo["COLUMNA_C_VALOR"] = Dr_Renglon_Actual["VALOR_CLASIFICACION_ZONA"].ToString();
                if (Contador_Renglones == Dt_Clasificacion_Zona.Rows.Count)
                {
                    Dr_Renglon_Nuevo["COLUMNA_D"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_D_ID"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_D_VALOR"] = "";
                    Dt_Clasificacion_Zona_Avaluo.Rows.Add(Dr_Renglon_Nuevo);
                    break;
                }
                i++;
            }
            else if (i == 3)
            {
                Dr_Renglon_Nuevo["COLUMNA_D"] = Dr_Renglon_Actual[Cat_Cat_Clasificacion_Zona.Campo_Clasificacion_Zona].ToString();
                Dr_Renglon_Nuevo["COLUMNA_D_ID"] = Dr_Renglon_Actual[Cat_Cat_Clasificacion_Zona.Campo_Clasificacion_Zona_Id].ToString();
                Dr_Renglon_Nuevo["COLUMNA_D_VALOR"] = Dr_Renglon_Actual["VALOR_CLASIFICACION_ZONA"].ToString();
                Dt_Clasificacion_Zona_Avaluo.Rows.Add(Dr_Renglon_Nuevo);
                Dr_Renglon_Nuevo = Dt_Clasificacion_Zona_Avaluo.NewRow();
                i = 0;
            }
            Contador_Renglones++;
        }
        Session["Dt_Grid_Clasificacion_Zona"] = Dt_Clasificacion_Zona_Avaluo.Copy();
        Grid_Clasificacion_Zona.DataSource = Dt_Clasificacion_Zona_Avaluo;
        Grid_Clasificacion_Zona.DataBind();
    }

    private void Crear_Tabla_Servicios_Zona(DataTable Dt_Servicios_Zona)
    {
        DataTable Dt_Servicios_Zona_Avaluo = new DataTable();
        DataRow Dr_Renglon_Nuevo;
        Int16 i = 0;
        int Contador_Renglones = 1;
        Dt_Servicios_Zona_Avaluo.Columns.Add("COLUMNA_A", typeof(String));
        Dt_Servicios_Zona_Avaluo.Columns.Add("COLUMNA_A_ID", typeof(String));
        Dt_Servicios_Zona_Avaluo.Columns.Add("COLUMNA_A_VALOR", typeof(String));

        Dt_Servicios_Zona_Avaluo.Columns.Add("COLUMNA_B", typeof(String));
        Dt_Servicios_Zona_Avaluo.Columns.Add("COLUMNA_B_ID", typeof(String));
        Dt_Servicios_Zona_Avaluo.Columns.Add("COLUMNA_B_VALOR", typeof(String));

        Dt_Servicios_Zona_Avaluo.Columns.Add("COLUMNA_C", typeof(String));
        Dt_Servicios_Zona_Avaluo.Columns.Add("COLUMNA_C_ID", typeof(String));
        Dt_Servicios_Zona_Avaluo.Columns.Add("COLUMNA_C_VALOR", typeof(String));

        Dt_Servicios_Zona_Avaluo.Columns.Add("COLUMNA_D", typeof(String));
        Dt_Servicios_Zona_Avaluo.Columns.Add("COLUMNA_D_ID", typeof(String));
        Dt_Servicios_Zona_Avaluo.Columns.Add("COLUMNA_D_VALOR", typeof(String));
        Dr_Renglon_Nuevo = Dt_Servicios_Zona_Avaluo.NewRow();
        foreach (DataRow Dr_Renglon_Actual in Dt_Servicios_Zona.Rows)
        {
            if (i == 0)
            {
                Dr_Renglon_Nuevo["COLUMNA_A"] = Dr_Renglon_Actual[Cat_Cat_Servicios_Zona.Campo_Servicio_Zona].ToString();
                Dr_Renglon_Nuevo["COLUMNA_A_ID"] = Dr_Renglon_Actual[Cat_Cat_Servicios_Zona.Campo_Servicio_Zona_Id].ToString();
                Dr_Renglon_Nuevo["COLUMNA_A_VALOR"] = Dr_Renglon_Actual["VALOR_SERVICIO_ZONA"].ToString();
                if (Contador_Renglones == Dt_Servicios_Zona.Rows.Count)
                {
                    Dr_Renglon_Nuevo["COLUMNA_B"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_B_ID"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_B_VALOR"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_C"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_C_ID"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_C_VALOR"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_D"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_D_ID"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_D_VALOR"] = "";
                    Dt_Servicios_Zona_Avaluo.Rows.Add(Dr_Renglon_Nuevo);
                    break;
                }
                i++;
            }
            else if (i == 1)
            {
                Dr_Renglon_Nuevo["COLUMNA_B"] = Dr_Renglon_Actual[Cat_Cat_Servicios_Zona.Campo_Servicio_Zona].ToString();
                Dr_Renglon_Nuevo["COLUMNA_B_ID"] = Dr_Renglon_Actual[Cat_Cat_Servicios_Zona.Campo_Servicio_Zona_Id].ToString();
                Dr_Renglon_Nuevo["COLUMNA_B_VALOR"] = Dr_Renglon_Actual["VALOR_SERVICIO_ZONA"].ToString();
                if (Contador_Renglones == Dt_Servicios_Zona.Rows.Count)
                {
                    Dr_Renglon_Nuevo["COLUMNA_C"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_C_ID"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_C_VALOR"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_D"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_D_ID"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_D_VALOR"] = "";
                    Dt_Servicios_Zona_Avaluo.Rows.Add(Dr_Renglon_Nuevo);
                    break;
                }
                i++;
            }
            else if (i == 2)
            {
                Dr_Renglon_Nuevo["COLUMNA_C"] = Dr_Renglon_Actual[Cat_Cat_Servicios_Zona.Campo_Servicio_Zona].ToString();
                Dr_Renglon_Nuevo["COLUMNA_C_ID"] = Dr_Renglon_Actual[Cat_Cat_Servicios_Zona.Campo_Servicio_Zona_Id].ToString();
                Dr_Renglon_Nuevo["COLUMNA_C_VALOR"] = Dr_Renglon_Actual["VALOR_SERVICIO_ZONA"].ToString();
                if (Contador_Renglones == Dt_Servicios_Zona.Rows.Count)
                {
                    Dr_Renglon_Nuevo["COLUMNA_D"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_D_ID"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_D_VALOR"] = "";
                    Dt_Servicios_Zona_Avaluo.Rows.Add(Dr_Renglon_Nuevo);
                    break;
                }
                i++;
            }
            else if (i == 3)
            {
                Dr_Renglon_Nuevo["COLUMNA_D"] = Dr_Renglon_Actual[Cat_Cat_Servicios_Zona.Campo_Servicio_Zona].ToString();
                Dr_Renglon_Nuevo["COLUMNA_D_ID"] = Dr_Renglon_Actual[Cat_Cat_Servicios_Zona.Campo_Servicio_Zona_Id].ToString();
                Dr_Renglon_Nuevo["COLUMNA_D_VALOR"] = Dr_Renglon_Actual["VALOR_SERVICIO_ZONA"].ToString();
                Dt_Servicios_Zona_Avaluo.Rows.Add(Dr_Renglon_Nuevo);
                Dr_Renglon_Nuevo = Dt_Servicios_Zona_Avaluo.NewRow();
                i = 0;
            }
            Contador_Renglones++;
        }

        Session["Dt_Grid_Servicios_Zona"] = Dt_Servicios_Zona_Avaluo.Copy();
        Grid_Servicios_Zona.DataSource = Dt_Servicios_Zona_Avaluo;
        Grid_Servicios_Zona.DataBind();
    }

    private void Crear_Tabla_Construccion_Dominante(DataTable Dt_Construccion_Dominante)
    {
        DataTable Dt_Construccion_Dominante_Avaluo = new DataTable();
        DataRow Dr_Renglon_Nuevo;
        Int16 i = 0;
        int Contador_Renglones = 1;
        Dt_Construccion_Dominante_Avaluo.Columns.Add("COLUMNA_A", typeof(String));
        Dt_Construccion_Dominante_Avaluo.Columns.Add("COLUMNA_A_ID", typeof(String));
        Dt_Construccion_Dominante_Avaluo.Columns.Add("COLUMNA_A_VALOR", typeof(String));

        Dt_Construccion_Dominante_Avaluo.Columns.Add("COLUMNA_B", typeof(String));
        Dt_Construccion_Dominante_Avaluo.Columns.Add("COLUMNA_B_ID", typeof(String));
        Dt_Construccion_Dominante_Avaluo.Columns.Add("COLUMNA_B_VALOR", typeof(String));

        Dt_Construccion_Dominante_Avaluo.Columns.Add("COLUMNA_C", typeof(String));
        Dt_Construccion_Dominante_Avaluo.Columns.Add("COLUMNA_C_ID", typeof(String));
        Dt_Construccion_Dominante_Avaluo.Columns.Add("COLUMNA_C_VALOR", typeof(String));

        Dr_Renglon_Nuevo = Dt_Construccion_Dominante_Avaluo.NewRow();
        foreach (DataRow Dr_Renglon_Actual in Dt_Construccion_Dominante.Rows)
        {
            if (i == 0)
            {
                Dr_Renglon_Nuevo["COLUMNA_A"] = Dr_Renglon_Actual[Cat_Cat_Construccion_Dominante.Campo_Construccion_Dominante].ToString();
                Dr_Renglon_Nuevo["COLUMNA_A_ID"] = Dr_Renglon_Actual[Cat_Cat_Construccion_Dominante.Campo_Construccion_Dominante_Id].ToString();
                Dr_Renglon_Nuevo["COLUMNA_A_VALOR"] = Dr_Renglon_Actual["VALOR_CONST_DOMINANTE"].ToString();
                if (Contador_Renglones == Dt_Construccion_Dominante.Rows.Count)
                {
                    Dr_Renglon_Nuevo["COLUMNA_B"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_B_ID"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_B_VALOR"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_C"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_C_ID"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_C_VALOR"] = "";
                    Dt_Construccion_Dominante_Avaluo.Rows.Add(Dr_Renglon_Nuevo);
                    break;
                }
                i++;
            }
            else if (i == 1)
            {
                Dr_Renglon_Nuevo["COLUMNA_B"] = Dr_Renglon_Actual[Cat_Cat_Construccion_Dominante.Campo_Construccion_Dominante].ToString();
                Dr_Renglon_Nuevo["COLUMNA_B_ID"] = Dr_Renglon_Actual[Cat_Cat_Construccion_Dominante.Campo_Construccion_Dominante_Id].ToString();
                Dr_Renglon_Nuevo["COLUMNA_B_VALOR"] = Dr_Renglon_Actual["VALOR_CONST_DOMINANTE"].ToString();
                if (Contador_Renglones == Dt_Construccion_Dominante.Rows.Count)
                {
                    Dr_Renglon_Nuevo["COLUMNA_C"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_C_ID"] = "";
                    Dr_Renglon_Nuevo["COLUMNA_C_VALOR"] = "";
                    Dt_Construccion_Dominante_Avaluo.Rows.Add(Dr_Renglon_Nuevo);
                    break;
                }
                i++;
            }
            else if (i == 2)
            {
                Dr_Renglon_Nuevo["COLUMNA_C"] = Dr_Renglon_Actual[Cat_Cat_Construccion_Dominante.Campo_Construccion_Dominante].ToString();
                Dr_Renglon_Nuevo["COLUMNA_C_ID"] = Dr_Renglon_Actual[Cat_Cat_Construccion_Dominante.Campo_Construccion_Dominante_Id].ToString();
                Dr_Renglon_Nuevo["COLUMNA_C_VALOR"] = Dr_Renglon_Actual["VALOR_CONST_DOMINANTE"].ToString();
                Dt_Construccion_Dominante_Avaluo.Rows.Add(Dr_Renglon_Nuevo);
                Dr_Renglon_Nuevo = Dt_Construccion_Dominante_Avaluo.NewRow();
                i = 0;
            }
            Contador_Renglones++;
        }
        Session["Dt_Grid_Construccion_Dominante"] = Dt_Construccion_Dominante_Avaluo.Copy();
        Grid_Construccion_Dominante.DataSource = Dt_Construccion_Dominante_Avaluo;
        Grid_Construccion_Dominante.DataBind();
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Avaluo_Urbano
    ///DESCRIPCIÓN          : Crea un DataSet para el reporte del avalúo urbano
    ///PARAMETROS: 
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 22/Junio/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Avaluo_Urbano()
    {
        String Valuador = "";
        Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio Avaluo = new Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio();
        Cls_Cat_Cat_Peritos_Externos_Negocio Perito_Externo = new Cls_Cat_Cat_Peritos_Externos_Negocio();
        DataTable Dt_Perito_Externo;
        DataTable Dt_Elementos_Construccion;

        DataTable Dt_Calculo_Valor_Construccion;
        DataTable Dt_Calculo_Valor_Terreno;
        DataTable Dt_Clasificacion_Zona;
        DataTable Dt_Servicios_Zona;
        DataTable Dt_Construccion_Dominante;
        Avaluo.P_Anio_Avaluo = Hdf_Anio_Avaluo.Value;
        Avaluo.P_No_Avaluo = Hdf_No_Avaluo.Value;
        DataTable Dt_Avaluo = Avaluo.Consultar_Avaluo_Urbano_Av();
        Ds_Ope_Cat_Avaluo_Urbano Ds_Avaluo_Urbano = new Ds_Ope_Cat_Avaluo_Urbano();
        Perito_Externo.P_Perito_Externo_Id = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Perito_Externo_Id].ToString();
        Dt_Perito_Externo = Perito_Externo.Consultar_Peritos_Externos();
        if (Dt_Perito_Externo.Rows.Count > 0)
        {
            if (Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Nombre].ToString().Trim() != "")
                Valuador += Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Nombre].ToString() + " ";
            if (Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Apellido_Paterno].ToString().Trim() != "")
                Valuador += Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Apellido_Paterno].ToString() + " ";
            if (Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Apellido_Materno].ToString().Trim() != "")
                Valuador += Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Apellido_Materno].ToString() + " ";
            if (Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Apellido_Materno].ToString().Trim() != "" || Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Apellido_Materno].ToString().Trim() != "")
            {
                Valuador += "(";
                if (Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Calle].ToString().Trim() != "")
                    Valuador += Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Calle].ToString() + " ";
                Valuador += ") ";
                if (Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Colonia].ToString().Trim() != "")
                    Valuador += Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Colonia].ToString() + " ";
            }
            if (Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Telefono].ToString().Trim() != "")
                Valuador += "TEL. " + Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Telefono].ToString();
        }
        DataTable Dt_Datos_Generales = Ds_Avaluo_Urbano.Tables["DT_DATOS_GENERALES"];
        DataTable Dt_Valor_Total_Predio = Ds_Avaluo_Urbano.Tables["DT_VALOR_TOTAL_PREDIO"];
        DataTable Dt_Caracteristicas_Terreno = Ds_Avaluo_Urbano.Tables["DT_CARACTERISTICAS_TERRENO"];
        DataTable Dt_Construccion = Ds_Avaluo_Urbano.Tables["DT_CONSTRUCCION"];
        DataTable Dt_Totales = Ds_Avaluo_Urbano.Tables["DT_TOTALES"];
        DataTable Dt_Colindancias = Ds_Avaluo_Urbano.Tables["DT_COLINDANCIAS"];
        DataRow Dr_Avaluo;
        Dr_Avaluo = Dt_Datos_Generales.NewRow();
        Dr_Avaluo["MOTIVO_AVALUO"] = Cmb_Motivo_Avaluo.SelectedItem.Text;
        Dr_Avaluo["UBICACION"] = Txt_Ubicacion_Predio.Text;
        Dr_Avaluo["LOCALIDAD"] = Txt_Localidad.Text;
        Dr_Avaluo["PROPIEDAD"] = Txt_Propietario.Text;
        Dr_Avaluo["COLONIA"] = Txt_Colonia.Text;
        Dr_Avaluo["MUNICIPIO"] = Txt_Municipio.Text;
        Dr_Avaluo["DOMICILIO_NOTIFICAR"] = Txt_Domicilio_Not.Text;
        Dr_Avaluo["LOCALIDAD_NOTIFICAR"] = Txt_Localidad_Not.Text;
        Dr_Avaluo["FIRMANTE"] =
        Dr_Avaluo["CUENTA_PREDIAL"] = Txt_Cuenta_Predial.Text;
        Dr_Avaluo["SOLICITANTE"] = Txt_Solicitante.Text.ToUpper();
        Dr_Avaluo["COLONIA_NOTIFICAR"] = Txt_Colonia_Not.Text;
        Dr_Avaluo["MUNICIPIO_NOTIFICAR"] = Txt_Municipio_Not.Text;
        Dr_Avaluo["CLAVE_CATASTRAL"] = "";
        Dr_Avaluo["OBSERVACIONES"] = Txt_Observaciones.Text.ToUpper();
        Dr_Avaluo["AVALUO"] = Txt_No_Avaluo.Text;
        Dr_Avaluo["REG"] = Txt_Region.Text;
        Dr_Avaluo["MZNA"] = Txt_Manzana.Text;
        Dr_Avaluo["LOTE"] = Txt_Lote.Text;
        if (Grid_Elementos_Construccion.Columns[6].Visible == true)
        {
            Dr_Avaluo["ANEXO"] = "SI";
        }
        else
        {
            Dr_Avaluo["ANEXO"] = "";
        }
        Dr_Avaluo["VALUADOR"] = Valuador.ToUpper();
        Dr_Avaluo["FIRMANTE"] = Cmb_Firmante.SelectedItem.Text.Split('-')[0].ToString().Trim();
        Dr_Avaluo["PUESTO"] = Cmb_Firmante.SelectedItem.Text.Split('-')[1].ToString().Trim();
        Dr_Avaluo["LEYENDAS"] = Leyenda.ToString();
        Dr_Avaluo["FECHA_ELABORACION"] = Convert.ToDateTime(Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Fecha_Creo].ToString());
        Dr_Avaluo["FECHA_AUTORIZACION"] = DateTime.Now;
        //Convert.ToDateTime(Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Fecha_Autorizo].ToString());
        Dr_Avaluo["NO_PROGRESIVO"] = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_No_Avaluo].ToString();
        Dr_Avaluo["NO_VALUADOR"] = Convert.ToInt16(Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Perito_Interno_Id].ToString()).ToString();
        Dt_Datos_Generales.Rows.Add(Dr_Avaluo);
        DataTable Dt_Elementos_Construccion_A = Ds_Avaluo_Urbano.Tables["DT_ELEMENTOS_CONSTRUCCION_A"];
        DataTable Dt_Elementos_Construccion_B = Ds_Avaluo_Urbano.Tables["DT_ELEMENTOS_CONSTRUCCION_B"];

        DataRow Dr_C_Elementos_A;
        DataRow Dr_C_Elementos_B;
        Dr_C_Elementos_A = Dt_Elementos_Construccion_A.NewRow();
        Dr_C_Elementos_B = Dt_Elementos_Construccion_B.NewRow();
        if (Grid_Elementos_Construccion.Columns[6].Visible == true)
        {
            Dr_C_Elementos_A["ELEMENTO_CONSTRUCCION_E"] = "E";
        }
        if (Grid_Elementos_Construccion.Columns[7].Visible == true)
        {
            Dr_C_Elementos_A["ELEMENTO_CONSTRUCCION_F"] = "F";
        }
        if (Grid_Elementos_Construccion.Columns[8].Visible == true)
        {
            Dr_C_Elementos_A["ELEMENTO_CONSTRUCCION_G"] = "G";
        }
        if (Grid_Elementos_Construccion.Columns[9].Visible == true)
        {
            Dr_C_Elementos_A["ELEMENTO_CONSTRUCCION_H"] = "H";
        }
        if (Grid_Elementos_Construccion.Columns[10].Visible == true)
        {
            Dr_C_Elementos_A["ELEMENTO_CONSTRUCCION_I"] = "I";
        }
        if (Grid_Elementos_Construccion.Columns[11].Visible == true)
        {
            Dr_C_Elementos_A["ELEMENTO_CONSTRUCCION_J"] = "J";
        }
        Dt_Elementos_Construccion_A.Rows.Add(Dr_C_Elementos_A);

        if (Grid_Elementos_Construccion.Columns[12].Visible == true)
        {
            Dr_C_Elementos_B["ELEMENTO_CONSTRUCCION_K"] = "K";
        }
        if (Grid_Elementos_Construccion.Columns[13].Visible == true)
        {
            Dr_C_Elementos_B["ELEMENTO_CONSTRUCCION_L"] = "L";
        }
        if (Grid_Elementos_Construccion.Columns[14].Visible == true)
        {
            Dr_C_Elementos_B["ELEMENTO_CONSTRUCCION_M"] = "M";
        }
        if (Grid_Elementos_Construccion.Columns[15].Visible == true)
        {
            Dr_C_Elementos_B["ELEMENTO_CONSTRUCCION_N"] = "N";
        }
        if (Grid_Elementos_Construccion.Columns[16].Visible == true)
        {
            Dr_C_Elementos_B["ELEMENTO_CONSTRUCCION_O"] = "O";
        }
        Dt_Elementos_Construccion_B.Rows.Add(Dr_C_Elementos_B);


        int cuenta = 0;
        for (int i = 6; i < 16; i++)
        {
            if (Grid_Elementos_Construccion.Columns[i].Visible == true)
            {


                for (int f = cuenta; f < Grid_Elementos_Construccion.Rows.Count; f++)
                {

                    DataRow Dr_Elementos_A;
                    Dr_Elementos_A = Dt_Elementos_Construccion_A.NewRow();
                    Dr_Elementos_A["ELEMENTO_CONSTRUCCION_E"] = ((TextBox)Grid_Elementos_Construccion.Rows[f].Cells[6].FindControl("Txt_E")).Text.ToUpper();
                    Dr_Elementos_A["ELEMENTO_CONSTRUCCION_F"] = ((TextBox)Grid_Elementos_Construccion.Rows[f].Cells[7].FindControl("Txt_F")).Text.ToUpper();
                    Dr_Elementos_A["ELEMENTO_CONSTRUCCION_G"] = ((TextBox)Grid_Elementos_Construccion.Rows[f].Cells[8].FindControl("Txt_G")).Text.ToUpper();
                    Dr_Elementos_A["ELEMENTO_CONSTRUCCION_H"] = ((TextBox)Grid_Elementos_Construccion.Rows[f].Cells[9].FindControl("Txt_H")).Text.ToUpper();
                    Dr_Elementos_A["ELEMENTO_CONSTRUCCION_I"] = ((TextBox)Grid_Elementos_Construccion.Rows[f].Cells[10].FindControl("Txt_I")).Text.ToUpper();
                    Dr_Elementos_A["ELEMENTO_CONSTRUCCION_J"] = ((TextBox)Grid_Elementos_Construccion.Rows[f].Cells[11].FindControl("Txt_J")).Text.ToUpper();
                    Dt_Elementos_Construccion_A.Rows.Add(Dr_Elementos_A);



                    DataRow Dr_Elementos_B;
                    Dr_Elementos_B = Dt_Elementos_Construccion_B.NewRow();
                    Dr_Elementos_B["ELEMENTO_CONSTRUCCION_K"] = ((TextBox)Grid_Elementos_Construccion.Rows[f].Cells[12].FindControl("Txt_K")).Text.ToUpper();
                    Dr_Elementos_B["ELEMENTO_CONSTRUCCION_L"] = ((TextBox)Grid_Elementos_Construccion.Rows[f].Cells[13].FindControl("Txt_L")).Text.ToUpper();
                    Dr_Elementos_B["ELEMENTO_CONSTRUCCION_M"] = ((TextBox)Grid_Elementos_Construccion.Rows[f].Cells[14].FindControl("Txt_M")).Text.ToUpper();
                    Dr_Elementos_B["ELEMENTO_CONSTRUCCION_N"] = ((TextBox)Grid_Elementos_Construccion.Rows[f].Cells[15].FindControl("Txt_N")).Text.ToUpper();
                    Dr_Elementos_B["ELEMENTO_CONSTRUCCION_O"] = ((TextBox)Grid_Elementos_Construccion.Rows[f].Cells[16].FindControl("Txt_O")).Text.ToUpper();
                    Dt_Elementos_Construccion_B.Rows.Add(Dr_Elementos_B);
                    cuenta = f;
                }
            }

        }
        Dt_Elementos_Construccion_A.TableName = "DT_ELEMENTOS_CONSTRUCCION_A";
        Dt_Elementos_Construccion_B.TableName = "DT_ELEMENTOS_CONSTRUCCION_B";



        Dr_Avaluo = Dt_Valor_Total_Predio.NewRow();
        Dr_Avaluo["VALOR_INPA"] = 0.00;
        Dr_Avaluo["VALOR_INPR"] = 0.00;
        Dr_Avaluo["VALOR_VR"] = 0.00;
        Dr_Avaluo["VALOR_TOTAL_PREDIO"] = Convert.ToDouble(Txt_Valor_Total_Predio.Text);
        Dt_Valor_Total_Predio.Rows.Add(Dr_Avaluo);

        Dr_Avaluo = Dt_Caracteristicas_Terreno.NewRow();
        if (Rdb_Buenas.Checked)
            Dr_Avaluo["BUENAS"] = "X";
        if (Rdb_Regulares.Checked)
            Dr_Avaluo["REGULARES"] = "X";
        if (Rdb_Malas.Checked)
            Dr_Avaluo["MALAS"] = "X";
        if (Rdb_Plana.Checked)
            Dr_Avaluo["PLANA"] = "X";
        if (Rdb_Pendiente.Checked)
            Dr_Avaluo["PENDIENTE"] = "X";
        if (Txt_Dens_Construccion.Text.Trim() != "")
            Dr_Avaluo["DENSIDAD_CONSTRUCCION"] = Convert.ToDouble(Txt_Dens_Construccion.Text);
        else
            Dr_Avaluo["DENSIDAD_CONSTRUCCION"] = Convert.ToDouble(Txt_Dens_Construccion.Text);
        Dt_Caracteristicas_Terreno.Rows.Add(Dr_Avaluo);

        Dr_Avaluo = Dt_Construccion.NewRow();
        if (Rdb_Nueva.Checked)
            Dr_Avaluo["NUEVA"] = "X";
        if (Rdb_Ampliacion.Checked)
            Dr_Avaluo["AMPLIACION"] = "X";
        if (Rdb_Remodelacion.Checked)
            Dr_Avaluo["REMODELACION"] = "X";
        if (Rdb_Rentada.Checked)
            Dr_Avaluo["RENTADA"] = "X";
        if (Rdb_Calidad_Buena.Checked)
            Dr_Avaluo["CALIDAD_B"] = "X";
        if (Rdb_Calidad_Regular.Checked)
            Dr_Avaluo["CALIDAD_R"] = "X";
        if (Rdb_Calidad_Mala.Checked)
            Dr_Avaluo["CALIDAD_M"] = "X";
        Dr_Avaluo["USO"] = Txt_Uso.Text.ToUpper();
        Dt_Construccion.Rows.Add(Dr_Avaluo);

        Dr_Avaluo = Dt_Totales.NewRow();
        Dr_Avaluo["SUP_TOTALES_TERRENO"] = Convert.ToDouble(Txt_Terreno_Superficie_Total.Text);
        Dr_Avaluo["VALOR_TOTAL_TERRENO"] = Convert.ToDouble(Txt_Terreno_Valor_Total.Text);
        Dr_Avaluo["SUP_TOTAL_CONST"] = Convert.ToDouble(Txt_Construccion_Superficie_Total.Text);
        Dr_Avaluo["VALOR_TOTAL_CONST"] = Convert.ToDouble(Txt_Construccion_Valor_Total.Text);
        Dt_Totales.Rows.Add(Dr_Avaluo);

        Guardar_Dt_Elementos_Construccion();
        Guardar_Grid_Clasificacion_Zona();
        Guardar_Grid_Construccion_Dominante();
        Guardar_Grid_Servicios_Zona();

        Guardar_Dt_Calculos();
        Guardar_Dt_Valores_Construccion();
        Dt_Calculo_Valor_Construccion = ((DataTable)Session["Dt_Grid_Valores_Construccion"]).Copy();
        Dt_Calculo_Valor_Construccion.TableName = "DT_CALCULO_VALOR_CONSTRUCCION";
        Dt_Calculo_Valor_Construccion.Columns.Add("GROUP", typeof(String));
        Dt_Calculo_Valor_Terreno = ((DataTable)Session["Dt_Grid_Calculos"]).Copy();
        Dt_Calculo_Valor_Terreno.TableName = "DT_CALCULO_VALOR_TERRENO";
        Dt_Calculo_Valor_Terreno.Columns.Add("GROUP", typeof(String));
        Dt_Elementos_Construccion = (DataTable)Session["Dt_Grid_Elementos_Construccion"];
        Dt_Elementos_Construccion.TableName = "DT_ELEMENTOS_CONSTRUCCION";

        Guardar_Grid_Clasificacion_Zona();
        Guardar_Grid_Construccion_Dominante();
        Guardar_Grid_Servicios_Zona();
        Dt_Clasificacion_Zona = (DataTable)Session["Dt_Grid_Clasificacion_Zona"];
        Dt_Clasificacion_Zona.TableName = "DT_CLASIFICACION_ZONA";
        Dt_Servicios_Zona = (DataTable)Session["Dt_Grid_Servicios_Zona"];
        Dt_Servicios_Zona.TableName = "DT_SERVICIOS_ZONA";
        Dt_Construccion_Dominante = (DataTable)Session["Dt_Grid_Construccion_Dominante"];
        Dt_Construccion_Dominante.TableName = "DT_CONSTRUCCION_DOMINANTE";
        Ds_Avaluo_Urbano = new Ds_Ope_Cat_Avaluo_Urbano();
        Ds_Avaluo_Urbano.Clear();
        Ds_Avaluo_Urbano.Tables.RemoveAt(0);
        Ds_Avaluo_Urbano.Tables.RemoveAt(0);
        Ds_Avaluo_Urbano.Tables.RemoveAt(0);
        Ds_Avaluo_Urbano.Tables.RemoveAt(0);
        Ds_Avaluo_Urbano.Tables.RemoveAt(0);
        Ds_Avaluo_Urbano.Tables.RemoveAt(0);
        Ds_Avaluo_Urbano.Tables.RemoveAt(0);
        Ds_Avaluo_Urbano.Tables.RemoveAt(0);
        Ds_Avaluo_Urbano.Tables.RemoveAt(0);
        Ds_Avaluo_Urbano.Tables.RemoveAt(0);
        Ds_Avaluo_Urbano.Tables.RemoveAt(0);
        Ds_Avaluo_Urbano.Tables.RemoveAt(0);
        Ds_Avaluo_Urbano.Tables.RemoveAt(0);
        Ds_Avaluo_Urbano.Tables.RemoveAt(0);

        Ds_Avaluo_Urbano.Tables.Add(Dt_Datos_Generales.Copy());
        Ds_Avaluo_Urbano.Tables.Add(Dt_Caracteristicas_Terreno.Copy());
        Ds_Avaluo_Urbano.Tables.Add(Dt_Construccion.Copy());
        Ds_Avaluo_Urbano.Tables.Add(Dt_Valor_Total_Predio.Copy());
        Ds_Avaluo_Urbano.Tables.Add(Dt_Calculo_Valor_Construccion.Copy());
        Ds_Avaluo_Urbano.Tables.Add(Dt_Calculo_Valor_Terreno.Copy());
        Ds_Avaluo_Urbano.Tables.Add(Dt_Elementos_Construccion.Copy());
        Ds_Avaluo_Urbano.Tables.Add(Dt_Elementos_Construccion_A.Copy());
        Ds_Avaluo_Urbano.Tables.Add(Dt_Elementos_Construccion_B.Copy());
        Ds_Avaluo_Urbano.Tables.Add(Dt_Clasificacion_Zona.Copy());
        Ds_Avaluo_Urbano.Tables.Add(Dt_Construccion_Dominante.Copy());
        Ds_Avaluo_Urbano.Tables.Add(Dt_Servicios_Zona.Copy());
        Ds_Avaluo_Urbano.Tables.Add(Dt_Totales.Copy());
        //Ds_Avaluo_Urbano.Tables.Add(Dt_Colindancias.Copy());



        return Ds_Avaluo_Urbano;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Imprimir_Reporte
    ///DESCRIPCIÓN          : Genera el reporte de Crystal con los datos proporcionados en el DataTable y lo manda a la impresora default
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Imprimir_Reporte(DataSet Ds_Convenios, String Nombre_Reporte, String Nombre_Archivo, String Formato, String Tipo)
    {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Catastro/" + Nombre_Reporte);
        try
        {
            Reporte.Load(File_Path);
            Reporte.SetDataSource(Ds_Convenios);
        }
        catch
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "No se pudo cargar el reporte para su impresión";
        }

        String Archivo_PDF = Nombre_Archivo + ".pdf";  // Es el nombre del PDF que se va a generar    
        try
        {
            ExportOptions Export_Options = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
            Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Archivo_PDF);
            Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
            Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
            Reporte.Export(Export_Options);
        }
        catch (Exception Ex)
        {
            //Lbl_Mensaje_Error.Visible = true;
            //Lbl_Mensaje_Error.Text = "No se pudo exportar el reporte";
        }

        try
        {
            Mostrar_Reporte(Archivo_PDF, Tipo, Formato);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Mostrar_Reporte
    ///DESCRIPCIÓN          : Visualiza en pantalla el reporte indicado
    ///PARAMETROS           : Nombre_Reporte: cadena con el nombre del archivo.
    ///                     : Formato: Exensión del archivo a visualizar.
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Mostrar_Reporte(String Nombre_Reporte, String Formato, String Frm_Formato)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            //if (Formato == "PDF")
            //{
            Pagina = Pagina + Nombre_Reporte;
            ScriptManager.RegisterStartupScript(this, this.GetType(), Frm_Formato, "window.open('" + Pagina + "', '" + Formato + "','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            //}
            //else if (Formato == "Excel")
            //{
            //    String Ruta = "../../Reporte/" + Nombre_Reporte;
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            //}
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Guardar_Dt_Calculos
    ///DESCRIPCIÓN: Guarda los cambios del grid
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 24/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Guardar_Dt_Calculos()
    {
        DataTable Dt_Calculos = (DataTable)Session["Dt_Grid_Calculos"];
        Grid_Calculos.Columns[3].Visible = true;
        for (int i = 0; i < Dt_Calculos.Rows.Count; i++)
        {
            TextBox Txt_Superficie_M2_Temporal = (TextBox)Grid_Calculos.Rows[i].Cells[1].FindControl("Txt_Superficie_M2");
            TextBox Txt_Valor_M2_Temporal = (TextBox)Grid_Calculos.Rows[i].Cells[2].FindControl("Txt_Valor_M2");
            TextBox Txt_Tramo_Id_Temporal = (TextBox)Grid_Calculos.Rows[i].Cells[3].FindControl("Txt_Tramo_Id");
            TextBox Txt_Factor_Temporal = (TextBox)Grid_Calculos.Rows[i].Cells[5].FindControl("Txt_Factor");
            TextBox Txt_Factor_Ef_Temporal = (TextBox)Grid_Calculos.Rows[i].Cells[6].FindControl("Txt_Factor_Ef");
            TextBox Txt_Total_Temporal = (TextBox)Grid_Calculos.Rows[i].Cells[7].FindControl("Txt_Total");


            Dt_Calculos.Rows[i]["SUPERFICIE_M2"] = Convert.ToDouble(Txt_Superficie_M2_Temporal.Text);
            Dt_Calculos.Rows[i]["VALOR_TRAMO"] = Convert.ToDouble(Txt_Valor_M2_Temporal.Text);
            //Dt_Calculos.Rows[i]["VALOR_TRAMO_ID"] = Txt_Tramo_Id_Temporal.Text;
            Dt_Calculos.Rows[i]["FACTOR"] = Convert.ToDouble(Txt_Factor_Temporal.Text);
            Dt_Calculos.Rows[i]["FACTOR_EF"] = Convert.ToDouble(Txt_Factor_Ef_Temporal.Text);
            Dt_Calculos.Rows[i]["VALOR_PARCIAL"] = Convert.ToDouble(Txt_Total_Temporal.Text);
        }
        Grid_Calculos.Columns[3].Visible = false;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Guardar_Dt_Elementos_Construccion
    ///DESCRIPCIÓN: Guarda los cambios en el grid de elementos de la construccion
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Guardar_Dt_Elementos_Construccion()
    {
        DataTable Dt_Elementos_Construccion = (DataTable)Session["Dt_Grid_Elementos_Construccion"];
        for (int i = 0; i < Dt_Elementos_Construccion.Rows.Count; i++)
        {
            TextBox Txt_A_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[2].FindControl("Txt_A");
            TextBox Txt_B_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[3].FindControl("Txt_B");
            TextBox Txt_C_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[4].FindControl("Txt_C");
            TextBox Txt_D_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[5].FindControl("Txt_D");
            TextBox Txt_E_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[6].FindControl("Txt_E");
            TextBox Txt_F_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[7].FindControl("Txt_F");
            TextBox Txt_G_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[8].FindControl("Txt_G");
            TextBox Txt_H_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[9].FindControl("Txt_H");
            TextBox Txt_I_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[10].FindControl("Txt_I");
            TextBox Txt_J_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[11].FindControl("Txt_J");
            TextBox Txt_K_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[12].FindControl("Txt_K");
            TextBox Txt_L_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[13].FindControl("Txt_L");
            TextBox Txt_M_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[14].FindControl("Txt_M");
            TextBox Txt_N_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[15].FindControl("Txt_N");
            TextBox Txt_O_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[16].FindControl("Txt_O");
            Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_A"] = Txt_A_Temporal.Text.ToUpper();
            Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_B"] = Txt_B_Temporal.Text.ToUpper();
            Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_C"] = Txt_C_Temporal.Text.ToUpper();
            Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_D"] = Txt_D_Temporal.Text.ToUpper();
            Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_E"] = Txt_E_Temporal.Text.ToUpper();
            Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_F"] = Txt_F_Temporal.Text.ToUpper();
            Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_G"] = Txt_G_Temporal.Text.ToUpper();
            Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_H"] = Txt_H_Temporal.Text.ToUpper();
            Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_I"] = Txt_I_Temporal.Text.ToUpper();
            Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_J"] = Txt_J_Temporal.Text.ToUpper();
            Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_K"] = Txt_K_Temporal.Text.ToUpper();
            Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_L"] = Txt_L_Temporal.Text.ToUpper();
            Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_M"] = Txt_M_Temporal.Text.ToUpper();
            Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_N"] = Txt_N_Temporal.Text.ToUpper();
            Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_O"] = Txt_O_Temporal.Text.ToUpper();
        }
    }

    protected void Guardar_Dt_Valores_Construccion()
    {
        DataTable Dt_Valores_Construccion = (DataTable)Session["Dt_Grid_Valores_Construccion"];
        for (int i = 0; i < Dt_Valores_Construccion.Rows.Count; i++)
        {
            TextBox Txt_Tipo_Temporal = (TextBox)Grid_Valores_Construccion.Rows[i].Cells[1].FindControl("Txt_Tipo");
            TextBox Txt_Con_Serv_Temporal = (TextBox)Grid_Valores_Construccion.Rows[i].Cells[2].FindControl("Txt_Con_Serv");
            TextBox Txt_Superficie_M2_Temporal = (TextBox)Grid_Valores_Construccion.Rows[i].Cells[3].FindControl("Txt_Superficie_M2");
            TextBox Txt_Valor_M2_Temporal = (TextBox)Grid_Valores_Construccion.Rows[i].Cells[4].FindControl("Txt_Valor_X_M2");
            TextBox Txt_Valor_Construccion_Id_Temporal = (TextBox)Grid_Valores_Construccion.Rows[i].Cells[5].FindControl("Txt_Valor_Construccion_Id");
            TextBox Txt_Factor_Temporal = (TextBox)Grid_Valores_Construccion.Rows[i].Cells[6].FindControl("Txt_Factor");
            TextBox Txt_Total_Temporal = (TextBox)Grid_Valores_Construccion.Rows[i].Cells[7].FindControl("Txt_Total");


            Dt_Valores_Construccion.Rows[i]["TIPO"] = Txt_Tipo_Temporal.Text;
            Dt_Valores_Construccion.Rows[i]["CON_SERV"] = Txt_Con_Serv_Temporal.Text;
            Dt_Valores_Construccion.Rows[i]["SUPERFICIE_M2"] = Convert.ToDouble(Txt_Superficie_M2_Temporal.Text);
            Dt_Valores_Construccion.Rows[i]["VALOR_M2"] = Convert.ToDouble(Txt_Valor_M2_Temporal.Text);
            Dt_Valores_Construccion.Rows[i]["FACTOR"] = Convert.ToDouble(Txt_Factor_Temporal.Text);
            Dt_Valores_Construccion.Rows[i]["VALOR_PARCIAL"] = Convert.ToDouble(Txt_Total_Temporal.Text);
        }
    }

    public static String Leyenda = " ";

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Firmante
    ///DESCRIPCIÓN: Llena la el combo con los datos de los firmantes
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 07/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Firmante()
    {

        try
        {
            DataTable Dt_Firmantes_Combo = new DataTable();
            Dt_Firmantes_Combo.Columns.Add("FIRMANTE", typeof(String));
            DataTable Dt_Firmante = new DataTable();
            DataRow Dr_Renglon_Nuevo;
            Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio Firmante = new Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio();
            Dt_Firmante = Firmante.Consulta_Firmante();
            Dr_Renglon_Nuevo = Dt_Firmantes_Combo.NewRow();
            Dr_Renglon_Nuevo["FIRMANTE"] = Dt_Firmante.Rows[0]["FIRMANTE_1"].ToString();
            Dt_Firmantes_Combo.Rows.InsertAt(Dr_Renglon_Nuevo, 0);
            Dr_Renglon_Nuevo = Dt_Firmantes_Combo.NewRow();
            Dr_Renglon_Nuevo["FIRMANTE"] = Dt_Firmante.Rows[0]["FIRMANTE_2"].ToString();
            Dt_Firmantes_Combo.Rows.InsertAt(Dr_Renglon_Nuevo, 1);
            Leyenda = Dt_Firmante.Rows[0]["LEYENDA"].ToString();

            Cmb_Firmante.DataTextField = "FIRMANTE";
            Cmb_Firmante.DataValueField = "FIRMANTE";
            Cmb_Firmante.DataSource = Dt_Firmantes_Combo;
            Cmb_Firmante.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Ventana_Emergente_Resumen_Predio
    ///DESCRIPCIÓN          : Establece el evento onclik del control para abrir la ventana emergente del Resumen de Predial con la ruta y parámtros necesarios
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 21/Otubre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Ventana_Emergente_Resumen_Predio()
    {
        String Ventana_Modal = "Abrir_Resumen('../Predial/Ventanas_Emergentes/Frm_Resumen_Predio.aspx";
        String Propiedades = ",'height=600,width=800,scrollbars=1');";
        Btn_Detalles_Cuenta_Predial.Attributes.Add("onclick", Ventana_Modal + "?Cuenta_Predial=" + Txt_Cuenta_Predial.Text.Trim() + "'" + Propiedades);
    }
}