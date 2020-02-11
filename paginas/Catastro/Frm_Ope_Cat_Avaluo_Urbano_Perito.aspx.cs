using System;

using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Operacion_Cat_Avaluo_Urbano.Negocio;
using System.Data;
using Presidencia.Catalogo_Cat_Motivos_Avaluo.Negocio;
using Presidencia.Constantes;
using Presidencia.Operacion_Resumen_Predio.Negocio;
using Operacion_Predial_Orden_Variacion.Negocio;
using Presidencia.Catalogo_Cat_Identificadores_Predio.Negocio;
using Presidencia.Catalogo_Cat_Valores_Inpa.Negocio;
using Presidencia.Catalogo_Cat_Valores_Inpr.Negocio;
using Presidencia.Catalogo_Cat_Peritos_Internos.Negocio;
using Presidencia.Catalogo_Cat_Tabla_Factores.Negocio;
using Presidencia.Catalogo_Cat_Peritos_Externos.Negocio;
using Presidencia.Catalogo_Cat_Parametros.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using System.IO;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Operacion_Calculo_Impuesto_Traslado.Negocio;
using Presidencia.Numalet;
using Presidencia.Registro_Peticion.Datos;
using Presidencia.Catalogo_Cat_Tipos_Construccion.Negocio;

public partial class paginas_Catastro_Frm_Ope_Cat_Avaluo_Urbano_Perito : System.Web.UI.Page
{

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Establece la configuración principal del formulario
    ///PROPIEDADES:   
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        Tsm_Generar_Nomina.RegisterPostBackControl(Btn_Agregar_Documento);
        Btn_Agregar_Documento.Attributes["onclick"] = "$get('" + Uprg_Reporte.ClientID + "').style.display = 'block'; return true;";
        try
        {
            if (Cls_Sessiones.Nombre_Ciudadano == null || Cls_Sessiones.Nombre_Ciudadano.Equals(String.Empty)) Response.Redirect("../Ventanilla/Frm_Apl_Login_Ventanilla.aspx");

            if (!IsPostBack)
            {
                Llenar_Combo_Motivos_Avaluo();
                Session["Activa"] = true;//Variable para mantener la session activa.
                if (Session["Tramite_Id"] != null)
                {
                    //Validar que el trámite exista o no... =P.
                    Hdf_Solicitud_Id.Value = Session["Tramite_Id"].ToString();
                    if (Obtener_Dato_Consulta("SELECT " + Ope_Cat_Avaluo_Rustico.Campo_No_Avaluo + " FROM " + Ope_Cat_Avaluo_Rustico.Tabla_Ope_Cat_Avaluo_Rustico + " WHERE " + Ope_Cat_Avaluo_Rustico.Campo_Solicitud_Id + " = '" + Session["Tramite_Id"] + "'").Trim() != "")
                    {
                        Btn_Nuevo.Visible = false;
                        Btn_Modificar.Visible = false;
                        Btn_Imprimir.Visible = false;
                        Btn_Salir.Visible = true;
                        Configuracion_Formulario(true);
                        return;
                    }
                    String Consulta = "SELECT " + Ope_Cat_Avaluo_Urbano.Campo_No_Avaluo + " FROM " + Ope_Cat_Avaluo_Urbano.Tabla_Ope_Cat_Avaluo_Urbano + " WHERE " + Ope_Cat_Avaluo_Urbano.Campo_Solicitud_Id + " = '" + Session["Tramite_Id"] + "'";
                    if (Obtener_Dato_Consulta(Consulta).Trim() == "" && Session["Postback_grid"] == null)
                    {
                        Btn_Nuevo_Click(null, null);
                        Session["Postback_grid"] = "TRUE";
                        Llenar_Combo_Tipos_Construccion();
                    }
                    else if (Obtener_Dato_Consulta(Consulta).Trim() != "" && Session["Postback_grid"] == null)
                    {
                        Limpiar_Formulario();
                        Llenar_Combo_Tipos_Construccion();
                        Cls_Cat_Cat_Parametros_Negocio Par = new Cls_Cat_Cat_Parametros_Negocio();
                        Int16 columnas = Convert.ToInt16(Par.Consultar_Parametros().Rows[0][Cat_Cat_Parametros.Campo_Columnas_Calc_Construccion].ToString());
                        Cls_Cat_Cat_Tabla_Factores_Negocio Tabla_Factores = new Cls_Cat_Cat_Tabla_Factores_Negocio();
                        Tabla_Factores.P_Anio = DateTime.Now.Year.ToString();
                        DataTable Dt_Factores_Cobro = Tabla_Factores.Consultar_Tabla_Factores_Cobro_Avaluos();
                        if (Dt_Factores_Cobro.Rows.Count > 0)
                        {
                            Hdf_Base_Cobro.Value = Dt_Factores_Cobro.Rows[0][Cat_Cat_Factores_Cobro_Avaluos.Campo_Base_Cobro].ToString();
                            Hdf_Factor_Cobro2.Value = Dt_Factores_Cobro.Rows[0][Cat_Cat_Factores_Cobro_Avaluos.Campo_Factor_Cobro_2].ToString();
                            Hdf_Porcentaje_Cobro.Value = Dt_Factores_Cobro.Rows[0][Cat_Cat_Factores_Cobro_Avaluos.Campo_Porcentaje_PE].ToString();
                        }
                        else
                        {
                            Hdf_Base_Cobro.Value = "0.00";
                            Hdf_Factor_Cobro2.Value = "0.00";
                            Hdf_Porcentaje_Cobro.Value = "0.00";
                        }
                        Cls_Ope_Cat_Avaluo_Urbano_Negocio Aval_Urb = new Cls_Ope_Cat_Avaluo_Urbano_Negocio();
                        Aval_Urb.P_Solicitud_Id = Session["Tramite_Id"].ToString();
                        DataTable Dt_Avaluo;
                        Dt_Avaluo = Aval_Urb.Consultar_Avaluo_Urbano();
                        Hdf_Anio_Avaluo.Value = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Anio_Avaluo].ToString();
                        Hdf_No_Avaluo.Value = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_No_Avaluo].ToString();
                        Aval_Urb.P_No_Avaluo = Hdf_No_Avaluo.Value;
                        Aval_Urb.P_Anio_Avaluo = Hdf_Anio_Avaluo.Value;
                        Session["Dt_Tabla_Valores_Construccion"] = Aval_Urb.Consultar_Tabla_Valores_Construccion();
                        Dt_Avaluo = Aval_Urb.Consultar_Avaluo_Urbano();
                        Cargar_Datos_Avaluo(Dt_Avaluo);
                        Cargar_Caracteristicas_Construccion(Aval_Urb.P_Dt_Caracteristicas_Terreno);
                        Cargar_Construccion(Aval_Urb.P_Dt_Construccion);
                        Crear_Tabla_Clasificacion_Zona(Aval_Urb.P_Dt_Clasificacion_Zona);
                        Crear_Tabla_Construccion_Dominante(Aval_Urb.P_Dt_Construccion_Dominante);
                        Crear_Tabla_Servicios_Zona(Aval_Urb.P_Dt_Servicios_Zona);
                        Session["Dt_Grid_Calculos"] = Aval_Urb.P_Dt_Calculo_Valor_Terreno.Copy();
                        Grid_Calculos.Columns[3].Visible = true;
                        Grid_Calculos.DataSource = Aval_Urb.P_Dt_Calculo_Valor_Terreno;
                        Grid_Calculos.PageIndex = 0;
                        Grid_Calculos.DataBind();
                        Grid_Calculos.Columns[3].Visible = false;
                        Session["Dt_Grid_Elementos_Construccion"] = Aval_Urb.P_Dt_Elementos_Construccion.Copy();
                        Grid_Elementos_Construccion.Columns[0].Visible = true;
                        for (Int16 i = 1; i < (columnas + 1); i++)
                        {
                            Grid_Elementos_Construccion.Columns[i + 1].Visible = true;
                        }
                        Grid_Elementos_Construccion.DataSource = Aval_Urb.P_Dt_Elementos_Construccion;
                        Grid_Elementos_Construccion.PageIndex = 0;
                        Grid_Elementos_Construccion.DataBind();
                        Grid_Elementos_Construccion.Columns[0].Visible = false;
                        for (int i = (columnas + 1); i < 16; i++)
                        {
                            Grid_Elementos_Construccion.Columns[i + 1].Visible = false;
                        }
                        Session["Dt_Grid_Valores_Construccion"] = Aval_Urb.P_Dt_Calculo_Valor_Construccion.Copy();
                        Grid_Valores_Construccion.DataSource = Aval_Urb.P_Dt_Calculo_Valor_Construccion;
                        Grid_Valores_Construccion.PageIndex = 0;
                        Grid_Valores_Construccion.DataBind();
                        DataTable Dt_Archivos = Aval_Urb.P_Dt_Archivos.Copy();
                        Dt_Archivos.Columns.Add("BITS_ARCHIVO", Type.GetType("System.Byte[]"));
                        Dt_Archivos.Columns.Add("EXTENSION_ARCHIVO", typeof(String));
                        Session["Dt_Documentos"] = Dt_Archivos;
                        Grid_Documentos.Columns[0].Visible = true;
                        Grid_Documentos.Columns[1].Visible = true;
                        Grid_Documentos.DataSource = Dt_Archivos;
                        Grid_Documentos.DataBind();
                        Grid_Documentos.Columns[0].Visible = false;
                        Grid_Documentos.Columns[1].Visible = false;
                        Div_Grid_Avaluo.Visible = false;
                        Div_Datos_Avaluo.Visible = true;
                        Session["Anio"] = Hdf_Anio_Avaluo.Value;
                        DataTable Dt_Medidas = Aval_Urb.P_Dt_Medidas.Copy();
                        Grid_Colindancias.Columns[0].Visible = true;
                        Grid_Colindancias.DataSource = Dt_Medidas;
                        Grid_Colindancias.DataBind();
                        Grid_Colindancias.Columns[0].Visible = false;
                        Session["Dt_Medidas"] = Dt_Medidas.Copy();
                        Calcular_Totales_Construccion();
                        Calcular_Totales_Terreno();
                        Calcular_Valor_Total_Predio();
                        Btn_Salir.AlternateText = "Atras";
                        if (Cmb_Estatus.SelectedValue == "RECHAZADO")
                        {
                            Div_Observaciones.Visible = true;
                        }
                        else
                        {
                            Div_Observaciones.Visible = false;
                        }
                        DataTable Dt_Motivos_Rechazo;
                        Aval_Urb.P_Estatus = "= 'VIGENTE'";
                        Dt_Motivos_Rechazo = Aval_Urb.Consultar_Motivos_Rechazo_Avaluo();
                        Session["Dt_Motivos_Rechazo"] = Dt_Motivos_Rechazo.Copy();
                        Grid_Observaciones.Columns[0].Visible = true;
                        Grid_Observaciones.Columns[1].Visible = true;
                        Grid_Observaciones.Columns[3].Visible = true;
                        Grid_Observaciones.DataSource = Dt_Motivos_Rechazo;
                        Grid_Observaciones.PageIndex = 0;
                        Grid_Observaciones.DataBind();
                        Grid_Observaciones.Columns[0].Visible = false;
                        Grid_Observaciones.Columns[1].Visible = false;
                        Grid_Observaciones.Columns[3].Visible = false;
                        if (Cmb_Revision.SelectedValue == "NO")
                        {
                            if (Cmb_Estatus.SelectedValue != "PAGADO")
                            {
                                Btn_Modificar_Click(null, null);
                            }
                            else
                            {
                                Btn_Nuevo.Visible = false;
                                Btn_Salir.Visible = true;
                                Btn_Modificar.Visible = true;
                                Btn_Imprimir.Visible = false;
                                Configuracion_Formulario(true);
                            }

                        }
                        else
                        {
                            Btn_Modificar.Visible = false;
                            Btn_Nuevo.Visible = false;
                            Btn_Imprimir.Visible = false;
                            Configuracion_Formulario(true);
                        }
                        Session["Postback_grid"] = "TRUE";
                    }
                }
                Txt_Observaciones.Attributes.Add("OnKeyPress", " ValidarCaracteres(this,350)");
                Txt_Observaciones.Attributes.Add("OnKeyUp", " ValidarCaracteres(this,350)");
                Txt_Observaciones.Attributes.Add("OnKeyPress", " ValidarCaracteres(this,350)");
                Txt_Observaciones.Attributes.Add("OnKeyUp", " ValidarCaracteres(this,350)");
                Txt_Uso.Attributes.Add("OnKeyPress", " ValidarCaracteres(this,30)");
                Txt_Uso.Attributes.Add("OnKeyUp", " ValidarCaracteres(this,30)");
                Txt_Uso.Attributes.Add("OnKeyPress", " ValidarCaracteres(this,30)");
                Txt_Uso.Attributes.Add("OnKeyUp", " ValidarCaracteres(this,30)");
                Txt_Comentarios_Perito.Attributes.Add("OnKeyPress", " ValidarCaracteres(this,250)");
                Txt_Comentarios_Perito.Attributes.Add("OnKeyUp", " ValidarCaracteres(this,250)");
                Txt_Comentarios_Perito.Attributes.Add("OnKeyPress", " ValidarCaracteres(this,250)");
                Txt_Comentarios_Perito.Attributes.Add("OnKeyUp", " ValidarCaracteres(this,250)");
                Txt_Medida_Colindancia.Attributes.Add("OnKeyPress", " ValidarCaracteres(this,400)");
                Txt_Medida_Colindancia.Attributes.Add("OnKeyUp", " ValidarCaracteres(this,400)");
                Txt_Medida_Colindancia.Attributes.Add("OnKeyPress", " ValidarCaracteres(this,400)");
                Txt_Medida_Colindancia.Attributes.Add("OnKeyUp", " ValidarCaracteres(this,400)");
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
    ///NOMBRE DE LA FUNCIÓN: Crear_Mascara
    ///DESCRIPCIÓN: Establece mascara
    ///PROPIEDADES:   
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
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
    private void Configuracion_Formulario(Boolean Enabled)
    {
        Txt_Comentarios_Perito.Enabled = !Enabled;
        Cmb_Motivo_Avaluo.Enabled = !Enabled;
        Txt_Uso.Enabled = !Enabled;
        Txt_No_Avaluo.Enabled = false;
        Txt_Ubicacion_Predio.Enabled = !Enabled;
        Txt_Colonia.Enabled = !Enabled;
        Txt_Localidad.Enabled = !Enabled;
        Txt_Municipio.Enabled = !Enabled;
        Txt_Propietario.Enabled = !Enabled;
        Txt_Domicilio_Not.Enabled = !Enabled;
        Txt_Colonia_Not.Enabled = !Enabled;
        Txt_Localidad_Not.Enabled = !Enabled;
        Txt_Municipio_Not.Enabled = !Enabled;
        Txt_Cuenta_Predial.Enabled = !Enabled;
        Txt_Region.Enabled = !Enabled;
        Txt_Manzana.Enabled = !Enabled;
        Txt_Lote.Enabled = !Enabled;
        Txt_Solicitante.Enabled = !Enabled;
        Txt_Dens_Construccion.Enabled = !Enabled;
        Txt_Observaciones.Enabled = !Enabled;
        Txt_Terreno_Superficie_Total.Enabled = false;
        Txt_Terreno_Valor_Total.Enabled = false;
        Txt_Construccion_Superficie_Total.Enabled = false;
        Txt_Construccion_Valor_Total.Enabled = false;
        Txt_Valor_Total_Predio.Enabled = false;
        Txt_Inpa.Enabled = false;
        Txt_Inpr.Enabled = false;
        Txt_Vr.Enabled = false;
        Cmb_Estatus.Enabled = false;
        Rdb_Ampliacion.Enabled = !Enabled;
        Rdb_Buenas.Enabled = !Enabled;
        Rdb_Malas.Enabled = !Enabled;
        Rdb_Nueva.Enabled = !Enabled;
        Rdb_Pendiente.Enabled = !Enabled;
        Rdb_Plana.Enabled = !Enabled;
        Rdb_Calidad_Buena.Enabled = !Enabled;
        Rdb_Calidad_Mala.Enabled = !Enabled;
        Rdb_Calidad_Regular.Enabled = !Enabled;
        Rdb_Regulares.Enabled = !Enabled;
        Rdb_Remodelacion.Enabled = !Enabled;
        Rdb_Rentada.Enabled = !Enabled;
        Rdb_Misma.Enabled = !Enabled;
        Grid_Elementos_Construccion.Enabled = !Enabled;
        Grid_Calculos.Enabled = !Enabled;
        Grid_Valores_Construccion.Enabled = !Enabled;
        Grid_Avaluos_Urbanos.Enabled = Enabled;
        Grid_Clasificacion_Zona.Enabled = !Enabled;
        Grid_Servicios_Zona.Enabled = !Enabled;
        Grid_Construccion_Dominante.Enabled = !Enabled;
        Txt_Busqueda.Enabled = Enabled;
        Btn_Buscar.Enabled = Enabled;
        Txt_Inpa.Enabled = !Enabled;
        Txt_Inpr.Enabled = !Enabled;
        Txt_Observaciones_Rechazo.Enabled = false;
        Txt_Construccion_Valor_Total.Style["text-align"] = "Right";
        Txt_Construccion_Superficie_Total.Style["text-align"] = "Right";
        Txt_Terreno_Superficie_Total.Style["text-align"] = "Right";
        Txt_Terreno_Valor_Total.Style["text-align"] = "Right";
        Txt_Dens_Construccion.Style["text-align"] = "Right";
        Txt_Valor_Total_Predio.Style["text-align"] = "Right";
        Txt_Inpa.Style["text-align"] = "Right";
        Txt_Inpr.Style["text-align"] = "Right";
        Txt_Vr.Style["text-align"] = "Right";
        Txt_Precio_Avaluo.Style["text-align"] = "Right";
        Cmb_Documento.Enabled = !Enabled;
        Btn_Agregar_Documento.Enabled = !Enabled;
        Fup_Documento.Enabled = !Enabled;
        Grid_Documentos.Enabled = !Enabled;
        Grid_Colindancias.Enabled = !Enabled;
        Btn_Agregar_Med_Col.Enabled = !Enabled;
        Txt_Medida_Colindancia.Enabled = !Enabled;
        Cmb_Revision.Enabled = !Enabled;
        Cmb_Tipo_Construccion.Enabled = !Enabled;
        Txt_Uso.Enabled = false;
        if (Enabled == false && Cmb_Tipo_Construccion.SelectedValue == "OTRO")
        {
            Txt_Uso.Enabled = !Enabled;
        }
        else if (Enabled == false && Cmb_Tipo_Construccion.SelectedValue != "OTRO")
        {
            Txt_Uso.Enabled = Enabled;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Formulario
    ///DESCRIPCIÓN: elimina los datos contenidos en los componenetes del formulario
    ///PROPIEDADES:    
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Formulario()
    {
        Txt_Comentarios_Perito.Text = "";
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
        Txt_Inpa.Text = "0.00";
        Txt_Inpr.Text = "0.00";
        Txt_Vr.Text = "0.00";
        Txt_Precio_Avaluo.Text = "0.00";
        Cmb_Estatus.SelectedIndex = 0;
        Rdb_Ampliacion.Checked = false;
        Rdb_Buenas.Checked = true;
        Rdb_Malas.Checked = false;
        Rdb_Nueva.Checked = true;
        Rdb_Pendiente.Checked = true;
        Rdb_Plana.Checked = false;
        Rdb_Calidad_Buena.Checked = false;
        Rdb_Calidad_Mala.Checked = false;
        Rdb_Calidad_Regular.Checked = false;
        Rdb_Regulares.Checked = false;
        Rdb_Remodelacion.Checked = false;
        Rdb_Rentada.Checked = false;
        Rdb_Misma.Checked = true;
        Txt_Busqueda.Text = "";
        Txt_Observaciones_Rechazo.Text = "";
        Txt_Medida_Colindancia.Text = "";
        Cmb_Revision.SelectedValue = "NO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Avaluos_Urbanos
    ///DESCRIPCIÓN: Llena la tabla de los datos de los avaluos urbanos
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
            Cls_Ope_Cat_Avaluo_Urbano_Negocio Avaluo_Urb = new Cls_Ope_Cat_Avaluo_Urbano_Negocio();
            if (Txt_Busqueda.Text.Trim() != "")
            {
                Avaluo_Urb.P_Folio = Txt_Busqueda.Text.Trim();
            }
            Avaluo_Urb.P_Perito_Externo_Id = Cls_Sessiones.Empleado_ID;
            Grid_Avaluos_Urbanos.Columns[1].Visible = true;
            Grid_Avaluos_Urbanos.Columns[2].Visible = true;
            Grid_Avaluos_Urbanos.DataSource = Avaluo_Urb.Consultar_Avaluo_Urbano();
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
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Motivos_Avaluo
    ///DESCRIPCIÓN: Llena la el combo con los datos de los motivos de avalúo
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 07/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Tipos_Construccion()
    {
        try
        {
            DataTable Dt_Tipos_Construccion;
            DataRow Dr_Renglon_Nuevo;
            Cls_Cat_Cat_Tipos_Construccion_Negocio Tipo_Construccion = new Cls_Cat_Cat_Tipos_Construccion_Negocio();
            Tipo_Construccion.P_Identificador = "ANTIGUO";
            Tipo_Construccion.P_Estatus = " = 'VIGENTE' ";
            Dt_Tipos_Construccion = Tipo_Construccion.Consultar_Tipos_Construccion_Uso();
            Dr_Renglon_Nuevo = Dt_Tipos_Construccion.NewRow();
            Dr_Renglon_Nuevo[Cat_Cat_Tipos_Construccion.Campo_Identificador] = "OTRO";
            Dr_Renglon_Nuevo[Cat_Cat_Tipos_Construccion.Campo_Identificador] = "OTRO";
            Dt_Tipos_Construccion.Rows.Add(Dr_Renglon_Nuevo);
            Cmb_Tipo_Construccion.DataSource = Dt_Tipos_Construccion;
            Cmb_Tipo_Construccion.DataTextField = Cat_Cat_Tipos_Construccion.Campo_Identificador;
            Cmb_Tipo_Construccion.DataValueField = Cat_Cat_Tipos_Construccion.Campo_Identificador;
            Cmb_Tipo_Construccion.DataBind();
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
            Cls_Cat_Cat_Tabla_Factores_Negocio Tabla_Factores = new Cls_Cat_Cat_Tabla_Factores_Negocio();
            Tabla_Factores.P_Anio = DateTime.Now.Year.ToString();
            DataTable Dt_Factores_Cobro = Tabla_Factores.Consultar_Tabla_Factores_Cobro_Avaluos();
            if (Dt_Factores_Cobro.Rows.Count > 0)
            {
                Hdf_Base_Cobro.Value = Dt_Factores_Cobro.Rows[0][Cat_Cat_Factores_Cobro_Avaluos.Campo_Base_Cobro].ToString();
                Hdf_Factor_Cobro2.Value = Dt_Factores_Cobro.Rows[0][Cat_Cat_Factores_Cobro_Avaluos.Campo_Factor_Cobro_2].ToString();
                Hdf_Porcentaje_Cobro.Value = Dt_Factores_Cobro.Rows[0][Cat_Cat_Factores_Cobro_Avaluos.Campo_Porcentaje_PE].ToString();
            }
            else
            {
                Hdf_Base_Cobro.Value = "0.00";
                Hdf_Factor_Cobro2.Value = "0.00";
                Hdf_Porcentaje_Cobro.Value = "0.00";
            }
            DataTable Dt_Avaluo;
            Hdf_Anio_Avaluo.Value = Grid_Avaluos_Urbanos.SelectedRow.Cells[2].Text;
            Hdf_No_Avaluo.Value = Grid_Avaluos_Urbanos.SelectedRow.Cells[1].Text;
            Cls_Ope_Cat_Avaluo_Urbano_Negocio Aval_Urb = new Cls_Ope_Cat_Avaluo_Urbano_Negocio();
            Aval_Urb.P_No_Avaluo = Hdf_No_Avaluo.Value;
            Aval_Urb.P_Anio_Avaluo = Hdf_Anio_Avaluo.Value;
            Session["Dt_Tabla_Valores_Construccion"] = Aval_Urb.Consultar_Tabla_Valores_Construccion();
            Dt_Avaluo = Aval_Urb.Consultar_Avaluo_Urbano();
            Cargar_Datos_Avaluo(Dt_Avaluo);
            Cargar_Caracteristicas_Construccion(Aval_Urb.P_Dt_Caracteristicas_Terreno);
            Cargar_Construccion(Aval_Urb.P_Dt_Construccion);
            Crear_Tabla_Clasificacion_Zona(Aval_Urb.P_Dt_Clasificacion_Zona);
            Crear_Tabla_Construccion_Dominante(Aval_Urb.P_Dt_Construccion_Dominante);
            Crear_Tabla_Servicios_Zona(Aval_Urb.P_Dt_Servicios_Zona);
            Session["Dt_Grid_Calculos"] = Aval_Urb.P_Dt_Calculo_Valor_Terreno.Copy();
            Grid_Calculos.Columns[3].Visible = true;
            Grid_Calculos.DataSource = Aval_Urb.P_Dt_Calculo_Valor_Terreno;
            Grid_Calculos.PageIndex = 0;
            Grid_Calculos.DataBind();
            Grid_Calculos.Columns[3].Visible = false;
            Session["Dt_Grid_Elementos_Construccion"] = Aval_Urb.P_Dt_Elementos_Construccion.Copy();
            Grid_Elementos_Construccion.Columns[0].Visible = true;
            for (Int16 i = 1; i < (columnas + 1); i++)
            {
                Grid_Elementos_Construccion.Columns[i + 1].Visible = true;
            }
            Grid_Elementos_Construccion.DataSource = Aval_Urb.P_Dt_Elementos_Construccion;
            Grid_Elementos_Construccion.PageIndex = 0;
            Grid_Elementos_Construccion.DataBind();
            Grid_Elementos_Construccion.Columns[0].Visible = false;
            for (int i = (columnas + 1); i < 16; i++)
            {
                Grid_Elementos_Construccion.Columns[i + 1].Visible = false;
            }
            Session["Dt_Grid_Valores_Construccion"] = Aval_Urb.P_Dt_Calculo_Valor_Construccion.Copy();
            Grid_Valores_Construccion.DataSource = Aval_Urb.P_Dt_Calculo_Valor_Construccion;
            Grid_Valores_Construccion.PageIndex = 0;
            Grid_Valores_Construccion.DataBind();
            DataTable Dt_Archivos = Aval_Urb.P_Dt_Archivos.Copy();
            Dt_Archivos.Columns.Add("BITS_ARCHIVO", Type.GetType("System.Byte[]"));
            Dt_Archivos.Columns.Add("EXTENSION_ARCHIVO", typeof(String));
            Session["Dt_Documentos"] = Dt_Archivos;
            Grid_Documentos.Columns[0].Visible = true;
            Grid_Documentos.Columns[1].Visible = true;
            Grid_Documentos.DataSource = Dt_Archivos;
            Grid_Documentos.DataBind();
            Grid_Documentos.Columns[0].Visible = false;
            Grid_Documentos.Columns[1].Visible = false;
            Div_Grid_Avaluo.Visible = false;
            Div_Datos_Avaluo.Visible = true;
            Session["Anio"] = Hdf_Anio_Avaluo.Value;
            DataTable Dt_Medidas = Aval_Urb.P_Dt_Medidas.Copy();
            Grid_Colindancias.Columns[0].Visible = true;
            Grid_Colindancias.DataSource = Dt_Medidas;
            Grid_Colindancias.DataBind();
            Grid_Colindancias.Columns[0].Visible = false;
            Session["Dt_Medidas"] = Dt_Medidas.Copy();
            Calcular_Totales_Construccion();
            Calcular_Totales_Terreno();
            Calcular_Valor_Total_Predio();
            Btn_Salir.AlternateText = "Atras";
            if (Cmb_Estatus.SelectedValue == "RECHAZADO")
            {
                Div_Observaciones.Visible = true;
            }
            else
            {
                Div_Observaciones.Visible = false;
            }
            DataTable Dt_Motivos_Rechazo;
            Aval_Urb.P_Estatus = "= 'VIGENTE'";
            Dt_Motivos_Rechazo = Aval_Urb.Consultar_Motivos_Rechazo_Avaluo();
            Session["Dt_Motivos_Rechazo"] = Dt_Motivos_Rechazo.Copy();
            Grid_Observaciones.Columns[0].Visible = true;
            Grid_Observaciones.Columns[1].Visible = true;
            Grid_Observaciones.Columns[3].Visible = true;
            Grid_Observaciones.DataSource = Dt_Motivos_Rechazo;
            Grid_Observaciones.PageIndex = 0;
            Grid_Observaciones.DataBind();
            Grid_Observaciones.Columns[0].Visible = false;
            Grid_Observaciones.Columns[1].Visible = false;
            Grid_Observaciones.Columns[3].Visible = false;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos_Avaluo
    ///DESCRIPCIÓN: carga los datos que viene en el datatable y asigna a su componente.
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Datos_Avaluo(DataTable Dt_Avaluo)
    {
        if (Dt_Avaluo.Rows.Count > 0)
        {
            Hdf_Cuenta_Predial_Id.Value = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Cuenta_Predial_Id].ToString();
            Txt_Observaciones.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Observaciones].ToString();
            Txt_Solicitante.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Solicitante].ToString();
            Txt_No_Avaluo.Text = Dt_Avaluo.Rows[0]["AVALUO"].ToString();
            Txt_Cuenta_Predial.Text = Dt_Avaluo.Rows[0]["CUENTA_PREDIAL"].ToString();
            Hdf_No_Avaluo.Value = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_No_Avaluo].ToString();
            Hdf_Anio_Avaluo.Value = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Anio_Avaluo].ToString();
            Cmb_Motivo_Avaluo.SelectedIndex = Cmb_Motivo_Avaluo.Items.IndexOf(Cmb_Motivo_Avaluo.Items.FindByValue(HttpUtility.HtmlDecode(Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Motivo_Avaluo_Id].ToString())));
            Cmb_Estatus.SelectedValue = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Estatus].ToString();
            Txt_Region.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Region].ToString();
            Txt_Manzana.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Manzana].ToString();
            Txt_Lote.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Lote].ToString();
            Txt_Ubicacion_Predio.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Ubicacion].ToString();
            Txt_Colonia.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Colonia].ToString();
            Txt_Localidad.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Localidad].ToString();
            Txt_Municipio.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Municipio].ToString();
            Txt_Propietario.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Propietario].ToString();
            Txt_Domicilio_Not.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Domicilio_Notificacion].ToString();
            Txt_Colonia_Not.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Colonia_Notificacion].ToString();
            Txt_Localidad_Not.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Localidad_Notificacion].ToString();
            Txt_Municipio_Not.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Municipio_Notificacion].ToString();
            Txt_Inpa.Text = Convert.ToDouble(Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Inpa].ToString()).ToString("##0.00");
            Txt_Inpr.Text = Convert.ToDouble(Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Inpr].ToString()).ToString("##0.00");
            Txt_Observaciones_Rechazo.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Observaciones_Perito].ToString();
            Txt_Comentarios_Perito.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Comentarios_Perito].ToString();
            Cmb_Revision.SelectedValue = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Permitir_Revision].ToString();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Caracteristicas_Construccion
    ///DESCRIPCIÓN: toma los valores del data_table y los asigna
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Caracteristicas_Construccion(DataTable Dt_Car_Construccion)
    {
        if (Dt_Car_Construccion.Rows.Count > 0)
        {
            String Vias_Acceso = Dt_Car_Construccion.Rows[0][Ope_Cat_Caract_Terreno_Au.Campo_Vias_Acceso].ToString();
            String Fotografia = Dt_Car_Construccion.Rows[0][Ope_Cat_Caract_Terreno_Au.Campo_Fotografia].ToString();
            String Dens_Construccion = Convert.ToDouble(Dt_Car_Construccion.Rows[0][Ope_Cat_Caract_Terreno_Au.Campo_Dens_Const].ToString()).ToString("##0.00");
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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Construccion
    ///DESCRIPCIÓN: toma los valores del data_table y los asigna
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Construccion(DataTable Dt_Construccion)
    {
        if (Dt_Construccion.Rows.Count > 0)
        {
            String Tipo_Construccion = Dt_Construccion.Rows[0][Ope_Cat_Construccion_Au.Campo_Tipo_Construccion].ToString();
            String Calidad_Proyecto = Dt_Construccion.Rows[0][Ope_Cat_Construccion_Au.Campo_Calidad_Proyecto].ToString();
            String Uso_Construccion = Dt_Construccion.Rows[0][Ope_Cat_Construccion_Au.Campo_Uso_Construccion].ToString();
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
                    case "MISMA":
                        Rdb_Misma.Checked = true;
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

            try
            {
                Cmb_Tipo_Construccion.SelectedValue = Uso_Construccion;
                Txt_Uso.Enabled = false;
                if (Cmb_Tipo_Construccion.SelectedValue != Uso_Construccion)
                {
                    Cmb_Tipo_Construccion.SelectedValue = "OTRO";
                    Txt_Uso.Text = Uso_Construccion;
                    Txt_Uso.Enabled = false;
                }
            }
            catch
            {
                Txt_Uso.Text = Uso_Construccion;
                Cmb_Tipo_Construccion.SelectedValue = "OTRO";
                Txt_Uso.Enabled = true;
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Evento del botón nuevo
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
            {
                Cls_Cat_Cat_Parametros_Negocio Par = new Cls_Cat_Cat_Parametros_Negocio();
                Int16 columnas = Convert.ToInt16(Par.Consultar_Parametros().Rows[0][Cat_Cat_Parametros.Campo_Columnas_Calc_Construccion].ToString());
                DataTable Dt_Elementos_Construccion;
                Configuracion_Formulario(false);
                Limpiar_Formulario();
                Btn_Nuevo.AlternateText = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Modificar.Visible = false;
                Btn_Imprimir.Visible = false;
                Div_Datos_Avaluo.Visible = true;
                Div_Grid_Avaluo.Visible = false;
                Crear_Dt_Valores_Construccion();
                Crear_Dt_Calculos();
                Cls_Ope_Cat_Avaluo_Urbano_Negocio Avaluo = new Cls_Ope_Cat_Avaluo_Urbano_Negocio();
                Dt_Elementos_Construccion = Avaluo.Consultar_Tabla_Elementos_Construccion();
                Session["Dt_Grid_Elementos_Construccion"] = Dt_Elementos_Construccion.Copy();
                Grid_Elementos_Construccion.Columns[0].Visible = true;
                for (Int16 i = 1; i < (columnas + 1); i++)
                {
                    Grid_Elementos_Construccion.Columns[i + 1].Visible = true;
                }
                Grid_Elementos_Construccion.DataSource = Dt_Elementos_Construccion;
                Grid_Elementos_Construccion.DataBind();
                Grid_Elementos_Construccion.Columns[0].Visible = false;
                for (int i = (columnas + 1); i < 16; i++)
                {
                    Grid_Elementos_Construccion.Columns[i + 1].Visible = false;
                }
                Dt_Elementos_Construccion = Avaluo.Consultar_Tabla_Clasificacion_Zona();
                Crear_Tabla_Clasificacion_Zona(Dt_Elementos_Construccion);
                Dt_Elementos_Construccion = Avaluo.Consultar_Tabla_Servicios_Zona();
                Crear_Tabla_Servicios_Zona(Dt_Elementos_Construccion);
                Dt_Elementos_Construccion = Avaluo.Consultar_Tabla_Const_Dominante();
                Crear_Tabla_Construccion_Dominante(Dt_Elementos_Construccion);
                Avaluo.P_Anio_Avaluo = DateTime.Now.Year.ToString();
                Session["Dt_Tabla_Valores_Construccion"] = Avaluo.Consultar_Tabla_Valores_Construccion();
                Session["Anio"] = DateTime.Now.Year.ToString();
                Txt_Inpa.Text = "0.00";
                Txt_Inpr.Text = "0.00";
                Txt_Vr.Text = "0.00";
                Cls_Cat_Cat_Tabla_Factores_Negocio Tabla_Factores = new Cls_Cat_Cat_Tabla_Factores_Negocio();
                Tabla_Factores.P_Anio = DateTime.Now.Year.ToString();
                DataTable Dt_Factores_Cobro = Tabla_Factores.Consultar_Tabla_Factores_Cobro_Avaluos();
                if (Dt_Factores_Cobro.Rows.Count > 0)
                {
                    Hdf_Base_Cobro.Value = Dt_Factores_Cobro.Rows[0][Cat_Cat_Factores_Cobro_Avaluos.Campo_Base_Cobro].ToString();
                    Hdf_Factor_Cobro2.Value = Dt_Factores_Cobro.Rows[0][Cat_Cat_Factores_Cobro_Avaluos.Campo_Factor_Cobro_2].ToString();
                    Hdf_Porcentaje_Cobro.Value = Dt_Factores_Cobro.Rows[0][Cat_Cat_Factores_Cobro_Avaluos.Campo_Porcentaje_PE].ToString();
                }
                else
                {
                    Hdf_Base_Cobro.Value = "0.00";
                    Hdf_Factor_Cobro2.Value = "0.00";
                    Hdf_Porcentaje_Cobro.Value = "0.00";
                }
                DataTable Dt_Documentos = new DataTable();
                Dt_Documentos.Columns.Add("NO_DOCUMENTO", typeof(String));
                Dt_Documentos.Columns.Add("ANIO_DOCUMENTO", typeof(int));
                Dt_Documentos.Columns.Add("DOCUMENTO", typeof(String));
                Dt_Documentos.Columns.Add("RUTA_DOCUMENTO", typeof(String));
                Dt_Documentos.Columns.Add("BITS_ARCHIVO", Type.GetType("System.Byte[]"));
                Dt_Documentos.Columns.Add("EXTENSION_ARCHIVO", typeof(String));
                Dt_Documentos.Columns.Add("ACCION", typeof(String));
                Session["Dt_Documentos"] = Dt_Documentos;
                DataTable Dt_Medidas = new DataTable();
                Dt_Medidas.Columns.Add(Ope_Cat_Colindancias_Aua.Campo_No_Colindancia, typeof(String));
                Dt_Medidas.Columns.Add(Ope_Cat_Colindancias_Aua.Campo_Medida_Colindancia, typeof(String));
                Dt_Medidas.Columns.Add("ACCION", typeof(String));
                Session["Dt_Medidas"] = Dt_Medidas;
                Grid_Colindancias.Columns[0].Visible = true;
                Grid_Colindancias.DataSource = Dt_Medidas;
                Grid_Colindancias.DataBind();
                Grid_Colindancias.Columns[0].Visible = false;
                Div_Observaciones.Visible = false;
                Cmb_Tipo_Construccion.SelectedIndex = 0;
                Txt_Uso.Enabled = false;
            }
            else
            {
                if (Validar_Componentes())
                {
                    Cls_Ope_Cat_Avaluo_Urbano_Negocio Avaluo = new Cls_Ope_Cat_Avaluo_Urbano_Negocio();
                    Avaluo.P_Anio_Avaluo = DateTime.Now.Year.ToString();
                    Avaluo.P_Cuenta_Predial_Id = Hdf_Cuenta_Predial_Id.Value;
                    Avaluo.P_Estatus = Cmb_Estatus.SelectedValue;
                    Avaluo.P_Motivo_Avaluo_Id = Cmb_Motivo_Avaluo.SelectedValue;
                    Avaluo.P_Observaciones = Txt_Observaciones.Text.ToUpper();
                    String Perito_Id = Obtener_Dato_Consulta("SELECT " + Ope_Tra_Solicitud.Campo_Inspector_ID + " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " WHERE " + Ope_Tra_Solicitud.Campo_Solicitud_ID + " = '" + Hdf_Solicitud_Id.Value + "'");
                    Avaluo.P_Perito_Externo_Id = Perito_Id;
                    Avaluo.P_Solicitante = Txt_Solicitante.Text.ToUpper();
                    Avaluo.P_Ruta_Fachada_Inmueble = "";
                    Avaluo.P_Valor_Inpa = Txt_Inpa.Text;
                    Avaluo.P_Valor_Inpr = Txt_Inpr.Text;
                    Avaluo.P_Valor_Total_Predio = Convert.ToDouble(Txt_Valor_Total_Predio.Text);
                    Avaluo.P_Valor_Vr = Convert.ToDouble(Txt_Vr.Text);
                    Guardar_Dt_Elementos_Construccion();
                    Guardar_Dt_Calculos();
                    Guardar_Dt_Valores_Construccion();
                    Avaluo.P_Dt_Calculo_Valor_Construccion = (DataTable)Session["Dt_Grid_Valores_Construccion"];
                    Avaluo.P_Dt_Calculo_Valor_Terreno = (DataTable)Session["Dt_Grid_Calculos"];
                    Avaluo.P_Dt_Caracteristicas_Terreno = Crear_Tabla_Caracteristicas_Terreno1();
                    Avaluo.P_Dt_Construccion = Crear_Tabla_Construccion_Terreno();
                    Avaluo.P_Dt_Elementos_Construccion = (DataTable)Session["Dt_Grid_Elementos_Construccion"];
                    Guardar_Grid_Clasificacion_Zona();
                    Guardar_Grid_Construccion_Dominante();
                    Guardar_Grid_Servicios_Zona();
                    Avaluo.P_Dt_Clasificacion_Zona = (DataTable)Session["Dt_Grid_Clasificacion_Zona"];
                    Avaluo.P_Dt_Servicios_Zona = (DataTable)Session["Dt_Grid_Servicios_Zona"];
                    Avaluo.P_Dt_Construccion_Dominante = (DataTable)Session["Dt_Grid_Construccion_Dominante"];
                    Avaluo.P_Dt_Archivos = (DataTable)Session["Dt_Documentos"];
                    Avaluo.P_Dt_Medidas = (DataTable)Session["Dt_Medidas"];
                    Avaluo.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.ToUpper();
                    Avaluo.P_Region = Txt_Region.Text.ToUpper();
                    Avaluo.P_Manzana = Txt_Manzana.Text.ToUpper();
                    Avaluo.P_Lote = Txt_Lote.Text.ToUpper();
                    Avaluo.P_Ubicacion = Txt_Ubicacion_Predio.Text.ToUpper();
                    Avaluo.P_Colonia = Txt_Colonia.Text.ToUpper();
                    Avaluo.P_Localidad = Txt_Localidad.Text.ToUpper();
                    Avaluo.P_Municipio = Txt_Municipio.Text.ToUpper();
                    Avaluo.P_Propietario = Txt_Propietario.Text.ToUpper();
                    Avaluo.P_Domicilio_Notificar = Txt_Domicilio_Not.Text.ToUpper();
                    Avaluo.P_Colonia_Notificar = Txt_Colonia_Not.Text.ToUpper();
                    Avaluo.P_Localidad_Notificar = Txt_Localidad_Not.Text.ToUpper();
                    Avaluo.P_Municipio_Notificar = Txt_Municipio_Not.Text.ToUpper();
                    Avaluo.P_Coord_Norte = "";
                    Avaluo.P_Coord_Oriente = "";
                    Avaluo.P_Coord_Poniente = "";
                    Avaluo.P_Coord_Sur = "";
                    Avaluo.P_Solicitud_Id = Hdf_Solicitud_Id.Value;
                    Avaluo.P_Comentarios_Perito = Txt_Comentarios_Perito.Text.ToUpper();
                    Avaluo.P_Permitir_Revision = Cmb_Revision.SelectedValue;
                    if ((Avaluo.Alta_Valor_Construccion()))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Avalúo Urbano", "alert('Alta Exitosa');", true);
                        Hdf_No_Avaluo.Value = Avaluo.P_No_Avaluo;
                        Hdf_Anio_Avaluo.Value = Avaluo.P_Anio_Avaluo;
                        Guardar_Imagenes(Avaluo.P_Dt_Archivos);
                        Btn_Salir_Click(null, null);
                        Response.Redirect("../Ventanilla/Frm_Apl_Ventanilla.aspx");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Avalúo Urbano", "alert('Alta Errónea');", true);
                    }
                }
            }
        }
        catch (Exception E)
        {
            Lbl_Ecabezado_Mensaje.Text = E.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Med_Col_Click
    ///DESCRIPCIÓN: Evento para el boton agregar med
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Agregar_Med_Col_Click(object sender, ImageClickEventArgs e)
    {
        if (Txt_Medida_Colindancia.Text.Trim() != "")
        {
            DataTable Dt_Medidas = (DataTable)Session["Dt_Medidas"];
            Boolean Entro = false;
            foreach (DataRow Dr_Renglon in Dt_Medidas.Rows)
            {
                if (Dr_Renglon[Ope_Cat_Colindancias_Ara.Campo_Medida_Colindancia].ToString() == Txt_Medida_Colindancia.Text.ToUpper() && Dr_Renglon["ACCION"].ToString() != "BAJA")
                {
                    Entro = true;
                    break;
                }
            }
            if (!Entro)
            {
                DataRow Dr_Nuevo = Dt_Medidas.NewRow();
                Dr_Nuevo[Ope_Cat_Colindancias_Ara.Campo_No_Colindancia] = " ";
                Dr_Nuevo["ACCION"] = "ALTA";
                Dr_Nuevo[Ope_Cat_Colindancias_Ara.Campo_Medida_Colindancia] = Txt_Medida_Colindancia.Text.ToUpper();
                Dt_Medidas.Rows.Add(Dr_Nuevo);
                Dt_Medidas.DefaultView.RowFilter = "ACCION <> 'BAJA'";
                Grid_Colindancias.Columns[0].Visible = true;
                Grid_Colindancias.DataSource = Dt_Medidas;
                Grid_Colindancias.DataBind();
                Grid_Colindancias.Columns[0].Visible = false;
            }
            Txt_Medida_Colindancia.Text = "";
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Colindancias_PageIndexChanging
    ///DESCRIPCIÓN: cambia la pagina
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Colindancias_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable Dt_Medidas = (DataTable)Session["Dt_Medidas"];
        if (Dt_Medidas != null)
        {
            Dt_Medidas.DefaultView.RowFilter = "ACCION <> 'BAJA'";
            Grid_Colindancias.Columns[0].Visible = true;
            Grid_Colindancias.DataSource = Dt_Medidas;
            Grid_Colindancias.PageIndex = e.NewPageIndex;
            Grid_Colindancias.DataBind();
            Grid_Colindancias.Columns[0].Visible = false;

        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Colindancias_SelectedIndexChanged
    ///DESCRIPCIÓN: cambia la pagina
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Colindancias_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Colindancias.SelectedIndex > -1)
        {
            DataTable Dt_Medidas = (DataTable)Session["Dt_Medidas"];
            if (Dt_Medidas != null)
            {
                foreach (DataRow Dr_Renglon in Dt_Medidas.Rows)
                {
                    if (Dr_Renglon[Ope_Cat_Colindancias_Ara.Campo_Medida_Colindancia].ToString() == Grid_Colindancias.SelectedRow.Cells[1].Text.ToUpper() && Dr_Renglon["ACCION"].ToString() != "BAJA")
                    {
                        Dr_Renglon["ACCION"] = "BAJA";
                    }
                }
                Dt_Medidas.DefaultView.RowFilter = "ACCION <> 'BAJA'";
                Grid_Colindancias.Columns[0].Visible = true;
                Grid_Colindancias.DataSource = Dt_Medidas;
                Grid_Colindancias.DataBind();
                Grid_Colindancias.Columns[0].Visible = false;
                Grid_Colindancias.SelectedIndex = -1;
            }
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
            if (Cmb_Estatus.SelectedValue == "POR VALIDAR" || Cmb_Estatus.SelectedValue == "RECHAZADO" || Cmb_Estatus.SelectedValue == "PAGADO")
            {
                if (Btn_Modificar.AlternateText.Equals("Modificar"))
                {
                    if (Cmb_Estatus.SelectedValue == "PAGADO")
                    {
                        Cls_Ope_Cat_Avaluo_Urbano_Negocio Avaluo = new Cls_Ope_Cat_Avaluo_Urbano_Negocio();
                        Avaluo.P_Anio_Avaluo = Hdf_Anio_Avaluo.Value;
                        Avaluo.P_No_Avaluo = Hdf_No_Avaluo.Value;
                        DateTime DaTi = Convert.ToDateTime(Avaluo.Consultar_Avaluo_Urbano().Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Fecha_Autorizo].ToString());
                        DateTime DaTi_New = DaTi.AddDays(30);
                        if (DateTime.Compare(DaTi, DaTi_New) == 1)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Avalúo Urbano", "alert('No es posible actualizar el Avalúo');", true);
                            return;
                        }
                        Hdf_Cobro_Anterior.Value = Txt_Precio_Avaluo.Text;
                    }
                    Configuracion_Formulario(false);
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.Visible = false;
                    Btn_Imprimir.Visible = false;
                    Cls_Cat_Cat_Tabla_Factores_Negocio Tabla_Factores = new Cls_Cat_Cat_Tabla_Factores_Negocio();
                    Tabla_Factores.P_Anio = DateTime.Now.Year.ToString();
                    DataTable Dt_Factores_Cobro = Tabla_Factores.Consultar_Tabla_Factores_Cobro_Avaluos();
                    if (Dt_Factores_Cobro.Rows.Count > 0)
                    {
                        Hdf_Base_Cobro.Value = Dt_Factores_Cobro.Rows[0][Cat_Cat_Factores_Cobro_Avaluos.Campo_Base_Cobro].ToString();
                        Hdf_Factor_Cobro2.Value = Dt_Factores_Cobro.Rows[0][Cat_Cat_Factores_Cobro_Avaluos.Campo_Factor_Cobro_2].ToString();
                        Hdf_Porcentaje_Cobro.Value = Dt_Factores_Cobro.Rows[0][Cat_Cat_Factores_Cobro_Avaluos.Campo_Porcentaje_PE].ToString();
                    }
                    else
                    {
                        Hdf_Base_Cobro.Value = "0.00";
                        Hdf_Factor_Cobro1.Value = "0.00";
                        Hdf_Porcentaje_Cobro.Value = "0.00";
                    }
                    DataTable Dt_Archivos = (DataTable)Session["Dt_Documentos"];
                    Grid_Documentos.Columns[0].Visible = true;
                    Grid_Documentos.Columns[1].Visible = true;
                    Grid_Documentos.DataSource = Dt_Archivos;
                    Grid_Documentos.DataBind();
                    Grid_Documentos.Columns[0].Visible = false;
                    Grid_Documentos.Columns[1].Visible = false;
                }
                else
                {
                    if (Validar_Componentes())
                    {
                        Cls_Ope_Cat_Avaluo_Urbano_Negocio Avaluo = new Cls_Ope_Cat_Avaluo_Urbano_Negocio();
                        Avaluo.P_Anio_Avaluo = Hdf_Anio_Avaluo.Value;
                        Avaluo.P_No_Avaluo = Hdf_No_Avaluo.Value;
                        Avaluo.P_Cuenta_Predial_Id = Hdf_Cuenta_Predial_Id.Value;
                        Avaluo.P_Estatus = Cmb_Estatus.SelectedValue;
                        Avaluo.P_Motivo_Avaluo_Id = Cmb_Motivo_Avaluo.SelectedValue;
                        Avaluo.P_Observaciones = Txt_Observaciones.Text.ToUpper();
                        Avaluo.P_Perito_Externo_Id = Cls_Sessiones.Empleado_ID;
                        Avaluo.P_Solicitante = Txt_Solicitante.Text.ToUpper();
                        Avaluo.P_Ruta_Fachada_Inmueble = "";
                        Avaluo.P_Valor_Inpa = Txt_Inpa.Text;
                        Avaluo.P_Valor_Inpr = Txt_Inpr.Text;
                        Avaluo.P_Valor_Total_Predio = Convert.ToDouble(Txt_Valor_Total_Predio.Text);
                        Avaluo.P_Valor_Vr = Convert.ToDouble(Txt_Vr.Text);
                        Guardar_Dt_Elementos_Construccion();
                        Guardar_Dt_Calculos();
                        Guardar_Dt_Valores_Construccion();
                        Avaluo.P_Dt_Calculo_Valor_Construccion = (DataTable)Session["Dt_Grid_Valores_Construccion"];
                        Avaluo.P_Dt_Calculo_Valor_Terreno = (DataTable)Session["Dt_Grid_Calculos"];
                        Avaluo.P_Dt_Caracteristicas_Terreno = Crear_Tabla_Caracteristicas_Terreno();
                        Avaluo.P_Dt_Construccion = Crear_Tabla_Construccion_Terreno();
                        Avaluo.P_Dt_Elementos_Construccion = (DataTable)Session["Dt_Grid_Elementos_Construccion"];
                        Guardar_Grid_Clasificacion_Zona();
                        Guardar_Grid_Construccion_Dominante();
                        Guardar_Grid_Servicios_Zona();
                        Avaluo.P_Dt_Clasificacion_Zona = (DataTable)Session["Dt_Grid_Clasificacion_Zona"];
                        Avaluo.P_Dt_Servicios_Zona = (DataTable)Session["Dt_Grid_Servicios_Zona"];
                        Avaluo.P_Dt_Construccion_Dominante = (DataTable)Session["Dt_Grid_Construccion_Dominante"];
                        Avaluo.P_Dt_Observaciones = (DataTable)Session["Dt_Motivos_Rechazo"];
                        Avaluo.P_Dt_Archivos = (DataTable)Session["Dt_Documentos"];
                        Avaluo.P_Dt_Medidas = (DataTable)Session["Dt_Medidas"];
                        Avaluo.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.ToUpper();
                        Avaluo.P_Region = Txt_Region.Text.ToUpper();
                        Avaluo.P_Manzana = Txt_Manzana.Text.ToUpper();
                        Avaluo.P_Lote = Txt_Lote.Text.ToUpper();
                        Avaluo.P_Ubicacion = Txt_Ubicacion_Predio.Text.ToUpper();
                        Avaluo.P_Colonia = Txt_Colonia.Text.ToUpper();
                        Avaluo.P_Localidad = Txt_Localidad.Text.ToUpper();
                        Avaluo.P_Municipio = Txt_Municipio.Text.ToUpper();
                        Avaluo.P_Propietario = Txt_Propietario.Text.ToUpper();
                        Avaluo.P_Domicilio_Notificar = Txt_Domicilio_Not.Text.ToUpper();
                        Avaluo.P_Colonia_Notificar = Txt_Colonia_Not.Text.ToUpper();
                        Avaluo.P_Localidad_Notificar = Txt_Localidad_Not.Text.ToUpper();
                        Avaluo.P_Municipio_Notificar = Txt_Municipio_Not.Text.ToUpper();
                        Avaluo.P_Coord_Norte = "";
                        Avaluo.P_Coord_Oriente = "";
                        Avaluo.P_Coord_Poniente = "";
                        Avaluo.P_Coord_Sur = "";
                        Avaluo.P_Comentarios_Perito = Txt_Comentarios_Perito.Text.ToUpper();
                        Avaluo.P_Permitir_Revision = Cmb_Revision.SelectedValue;
                        if (Cmb_Estatus.SelectedValue != "PAGADO")
                        {
                            if (Avaluo.P_Dt_Observaciones != null && Avaluo.P_Dt_Observaciones.Rows.Count > 0)
                            {
                                Avaluo.P_Estatus = "POR VALIDAR";
                            }
                        }
                        else if (Cmb_Estatus.SelectedValue == "PAGADO")
                        {
                            if ((Convert.ToDouble(Txt_Precio_Avaluo.Text) - Convert.ToDouble(Hdf_Cobro_Anterior.Value)) > 0)
                            {
                                Avaluo.P_Estatus = "POR VALIDAR";
                            }
                        }
                        if ((Avaluo.Modificar_Valor_Construccion()))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Avalúo Urbano", "alert('Actualizacion Exitosa');", true);
                            Hdf_No_Avaluo.Value = Avaluo.P_No_Avaluo;
                            Hdf_Anio_Avaluo.Value = Avaluo.P_Anio_Avaluo;
                            Eliminar_Imagenes(Avaluo.P_Dt_Archivos);
                            Guardar_Imagenes(Avaluo.P_Dt_Archivos);
                            Btn_Salir_Click(null, null);
                            Response.Redirect("../Ventanilla/Frm_Apl_Ventanilla.aspx");
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Avalúo Urbano", "alert('Actualización Errónea');", true);
                        }
                    }
                }
            }
            else
            {
                Btn_Nuevo.Visible = false;
                Btn_Modificar.Visible = false;

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
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Avaluo_Urbano_Folio_Pago
    ///DESCRIPCIÓN          : Crea un DataSet para imprimir el folio del pago
    ///PARAMETROS: 
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 22/Junio/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Avaluo_Urbano_Folio_Pago()
    {
        Cls_Ope_Cat_Avaluo_Urbano_Negocio Avaluo = new Cls_Ope_Cat_Avaluo_Urbano_Negocio();
        Cls_Cat_Cat_Peritos_Externos_Negocio Perito_Interno = new Cls_Cat_Cat_Peritos_Externos_Negocio();
        Ds_Ope_Cat_Folio_Pago_Avaluo Folio_Avaluo = new Ds_Ope_Cat_Folio_Pago_Avaluo();
        Avaluo.P_No_Avaluo = Hdf_No_Avaluo.Value;
        Avaluo.P_Anio_Avaluo = Hdf_Anio_Avaluo.Value;
        DataTable Dt_Avaluo = Avaluo.Consultar_Avaluo_Urbano();
        Perito_Interno.P_Perito_Externo_Id = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Perito_Externo_Id].ToString();
        String Cantidad_Cobro = "";
        if ((Convert.ToDouble(Txt_Precio_Avaluo.Text) - Convert.ToDouble(Hdf_Cobro_Anterior.Value) > 0))
        {
            Cantidad_Cobro = (Convert.ToDouble(Txt_Precio_Avaluo.Text) - Convert.ToDouble(Hdf_Cobro_Anterior.Value)).ToString("0.00");
        }
        else
        {
            Cantidad_Cobro = "0.00";
        }
        Numalet Cantidad = new Numalet();
        Cantidad.MascaraSalidaDecimal = "00/100 M.N.";
        Cantidad.SeparadorDecimalSalida = "Pesos";
        Cantidad.ApocoparUnoParteEntera = true;
        Cantidad.LetraCapital = true;
        Dt_Avaluo = Perito_Interno.Consultar_Peritos_Externos();
        DataTable Dt_Folio_Avaluo = Folio_Avaluo.Tables["DT_PAGO_AVALUO"];
        DataRow Dr_Renglon_Nuevo = Dt_Folio_Avaluo.NewRow();
        Dr_Renglon_Nuevo["NOMBRE"] = Dt_Avaluo.Rows[0]["PERITO_EXTERNO"].ToString();
        Dr_Renglon_Nuevo["RFC"] = Dt_Avaluo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Observaciones].ToString();
        Dr_Renglon_Nuevo["NUMERO_PERITO"] = Convert.ToInt16(Dt_Avaluo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Perito_Externo_Id].ToString());
        Dr_Renglon_Nuevo["NUMERO_AVALUO"] = "AU" + Txt_No_Avaluo.Text;
        Dr_Renglon_Nuevo["PAGO_PRESIDENCIA"] = Convert.ToDouble(Cantidad_Cobro);
        Dr_Renglon_Nuevo["DIRECCION"] = Txt_Domicilio_Not.Text + " COL. " + Txt_Colonia_Not.Text;
        Dr_Renglon_Nuevo["CUENTA_PREDIAL"] = Txt_Cuenta_Predial.Text;
        Dr_Renglon_Nuevo["IMPORTE_AVALUO_LETRAS"] = Cantidad.ToCustomCardinal(Cantidad_Cobro); ;
        Dr_Renglon_Nuevo["IMPORTE_AVALUO"] = Convert.ToDouble(Cantidad_Cobro);
        Session["E_Mail"] = Dt_Avaluo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Usuario].ToString();
        Dt_Folio_Avaluo.Rows.Add(Dr_Renglon_Nuevo);
        return Folio_Avaluo;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Imprimir_Reporte_Folio_Pago
    ///DESCRIPCIÓN          : Genera el reporte de Crystal con los datos proporcionados en el DataTable y lo manda a la impresora default
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Imprimir_Reporte_Folio_Pago(DataSet Ds_Convenios, String Nombre_Reporte, String Nombre_Archivo, String Formato, String Tipo)
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
        { }
        try
        {
            Enviar_Correo_Cuenta((String)Session["E_Mail"], Server.MapPath("../../Reporte/" + Archivo_PDF));
            Session.Remove("E_Mail");
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Enviar_Correo_Cuenta
    ///DESCRIPCIÓN: Envia un correo al correo del perito externo
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Enviar_Correo_Cuenta(String E_Mail, String Url_Adjunto)
    {
        String Contenido = "";
        if ((Convert.ToDouble(Txt_Precio_Avaluo.Text) - Convert.ToDouble(Hdf_Cobro_Anterior.Value)) > 0)
        {
            Contenido = "Su avalúo urbano ha sido modificado. Favor de pasar a pagar en las cajas de Presidencia de Irapuato, su folio de pago se encuentra adjunto a este correo. Favor de imprimirlo dos veces";
        }
        else
        {
            Contenido = "El avalúo se ha modificado, sin embargo, no le generará un cobro extra.";
        }
        try
        {
            if (E_Mail.Trim().Length > 0)
            {
                Cls_Mail mail = new Cls_Mail();
                mail.P_Servidor = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Servidor_Correo].ToString();
                mail.P_Envia = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Correo_Saliente].ToString();
                mail.P_Password = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Password_Correo].ToString();
                mail.P_Recibe = E_Mail.Trim();
                mail.P_Subject = "Avalúo Modificado";
                mail.P_Texto = Contenido;
                mail.P_Adjunto = Url_Adjunto;//Hacer_Pdf();
                mail.Enviar_Correo();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("No se pudo enviar el Correo.");
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
        //Mandar imprimir el reporte
        //if (Grid_Avaluos_Urbanos.SelectedIndex > -1)
        //{
        if (Cmb_Estatus.SelectedValue == "PAGADO")
        {
            Imprimir_Reporte(Crear_Ds_Avaluo_Urbano(), "Rpt_Ope_Cat_Avaluo_Urbano.rpt", "Reporte_Avaluo_Urbano", "Window_Frm", "Avaluo_Urbano");
        }
        //}
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Guardar_Imagenes
    ///DESCRIPCIÓN: Crea las imagenes en la carpeta deseada
    ///PROPIEDADES:     Dt_Documentos:      Tabla que contiene todos los datos para ser creados como imagenes.
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/Jun/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Guardar_Imagenes(DataTable Dt_Documentos)
    {
        if (!Directory.Exists(Server.MapPath("../Catastro/Archivos_Avaluo_Au/" + Hdf_Anio_Avaluo.Value + "_" + Hdf_No_Avaluo.Value + "/")))
        {
            Directory.CreateDirectory(Server.MapPath("../Catastro/Archivos_Avaluo_Au/" + Hdf_Anio_Avaluo.Value + "_" + Hdf_No_Avaluo.Value + "/"));
        }
        foreach (DataRow Dr_Renglon in Dt_Documentos.Rows)
        {
            if (Dr_Renglon["ACCION"].ToString() == "ALTA")
            {
                //crear filestream y binarywriter para guardar archivo
                FileStream Escribir_Archivo = new FileStream(Server.MapPath("../Catastro/Archivos_Avaluo_Au/" + Hdf_Anio_Avaluo.Value + "_" + Hdf_No_Avaluo.Value + "/" + Dr_Renglon["RUTA_DOCUMENTO"].ToString()), FileMode.Create, FileAccess.Write);
                BinaryWriter Datos_Archivo = new BinaryWriter(Escribir_Archivo);
                Datos_Archivo.Write((Byte[])Dr_Renglon["BITS_ARCHIVO"]);
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Eliminar_Imagenes
    ///DESCRIPCIÓN: Elimina las imagenes en la carpeta designada
    ///PROPIEDADES:     Dt_Documentos:      Tabla que contiene todos los datos para ser Eliminados de la carpeta del perito.
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 06/Jun/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Eliminar_Imagenes(DataTable Dt_Documentos)
    {
        foreach (DataRow Dr_Renglon in Dt_Documentos.Rows)
        {
            if (Dr_Renglon["ACCION"].ToString() == "BAJA" && Dr_Renglon["NO_DOCUMENTO"].ToString().Trim().Replace("&nbsp;", "") != "")
            {
                //Elimina el archivo con la ruta asignadaen la columna RUTA_DOCUMENTO
                File.Delete(Server.MapPath(Dr_Renglon["RUTA_DOCUMENTO"].ToString()));
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Insertar_Pasivo
    ///DESCRIPCIÓN          : Genera el Pasivo
    ///PARAMETROS:     
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 22/Septiembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Insertar_Pasivo(String Referencia)
    {
        try
        {
            Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Calculo_Impuesto_Traslado = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio();
            String Clave_Ingreso_Id = "";
            String Dependencia_Id = "";
            String Consulta = "SELECT " + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID + " FROM " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + " WHERE " + Cat_Pre_Claves_Ingreso.Campo_Descripcion + " LIKE '%AUTORIZACION AVALUOS PERITOS FISCALES%'";
            Clave_Ingreso_Id = Obtener_Dato_Consulta(Consulta);
            if (Clave_Ingreso_Id.Trim() != "")
            {
                Consulta = "SELECT " + Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID + " FROM " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + " WHERE " + Cat_Pre_Claves_Ingreso.Campo_Descripcion + " LIKE '%AUTORIZACION AVALUOS PERITOS FISCALES%'";
                Dependencia_Id = Obtener_Dato_Consulta(Consulta);
                if (Dependencia_Id.Trim() != "")
                {
                    Cls_Cat_Cat_Parametros_Negocio Dias = new Cls_Cat_Cat_Parametros_Negocio();
                    Calculo_Impuesto_Traslado.P_Referencia = Referencia;
                    Calculo_Impuesto_Traslado.P_Descripcion = "AUTORIZACION DE AVALUO URBANO";
                    if (Hdf_Cobro_Anterior.Value.Trim() == "")
                    {
                        Hdf_Cobro_Anterior.Value = "0.00";
                    }
                    if ((Convert.ToDouble(Txt_Precio_Avaluo.Text) - Convert.ToDouble(Hdf_Cobro_Anterior.Value)) > 0)
                    {
                        Calculo_Impuesto_Traslado.P_Estatus = "POR PAGAR";
                        Calculo_Impuesto_Traslado.P_Monto_Total_Pagar = (Convert.ToDouble(Txt_Precio_Avaluo.Text) - Convert.ToDouble(Hdf_Cobro_Anterior.Value)).ToString("0.00");
                    }
                    else
                    {
                        Calculo_Impuesto_Traslado.P_Estatus = "PAGADO";
                        Calculo_Impuesto_Traslado.P_Monto_Total_Pagar = "0.00";
                    }
                    Calculo_Impuesto_Traslado.P_Clave_Ingreso_ID = Clave_Ingreso_Id;
                    Calculo_Impuesto_Traslado.P_Dependencia_ID = Dependencia_Id;
                    Calculo_Impuesto_Traslado.P_Fecha_Tramite = DateTime.Now.ToString("dd/MMM/yyyy");
                    Calculo_Impuesto_Traslado.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_Id.Value;
                    Calculo_Impuesto_Traslado.P_Contribuyente = Txt_Propietario.Text;
                    Calculo_Impuesto_Traslado.P_Fecha_Vencimiento_Pasivo = DateTime.Now.AddDays(Convert.ToInt16(Dias.Consultar_Parametros().Rows[0][Cat_Cat_Parametros.Campo_Dias_Vigencia].ToString())).ToString("dd/MMM/yyyy");
                    Calculo_Impuesto_Traslado.Alta_Pasivo();
                }
                else
                { }
            }
            else
            { }
        }
        catch (Exception Ex)
        { }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Obtener_Dato_Consulta
    ///DESCRIPCIÓN          : Consulta el Campo dado de la Tabla Indicada
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benvides Guardado
    ///FECHA_CREO           : 24/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private String Obtener_Dato_Consulta(String Consulta)
    {
        String Dato_Consulta = "";

        try
        {
            OracleDataReader Dr_Dato = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Consulta);
            if (Dr_Dato.Read())
            {
                if (Dr_Dato[0] != null)
                {
                    Dato_Consulta = Dr_Dato[0].ToString();
                }
                else
                {
                    Dato_Consulta = "";
                }
                Dr_Dato.Close();
            }
            else
            {
                Dato_Consulta = "";
            }
            Dr_Dato = null;
        }
        catch
        { }
        finally
        { }
        return Dato_Consulta;
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
        { }
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
            Pagina = Pagina + Nombre_Reporte;
            ScriptManager.RegisterStartupScript(this, this.GetType(), Frm_Formato, "window.open('" + Pagina + "', '" + Formato + "','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
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
        Cls_Ope_Cat_Avaluo_Urbano_Negocio Avaluo = new Cls_Ope_Cat_Avaluo_Urbano_Negocio();
        Cls_Cat_Cat_Peritos_Externos_Negocio Perito_Externo = new Cls_Cat_Cat_Peritos_Externos_Negocio();
        DataTable Dt_Elementos_Construccion;
        DataTable Dt_Calculo_Valor_Construccion;
        DataTable Dt_Calculo_Valor_Terreno;
        DataTable Dt_Clasificacion_Zona;
        DataTable Dt_Servicios_Zona;
        DataTable Dt_Construccion_Dominante;
        Avaluo.P_Anio_Avaluo = Hdf_Anio_Avaluo.Value;
        Avaluo.P_No_Avaluo = Hdf_No_Avaluo.Value;
        DataTable Dt_Avaluo = Avaluo.Consultar_Avaluo_Urbano();
        Ds_Ope_Cat_Avaluo_Urbano Ds_Avaluo_Urbano = new Ds_Ope_Cat_Avaluo_Urbano();
        String Perito_Id = Obtener_Dato_Consulta("SELECT " + Ope_Tra_Solicitud.Campo_Inspector_ID + " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " WHERE " + Ope_Tra_Solicitud.Campo_Solicitud_ID + " = '" + Hdf_Solicitud_Id.Value + "'");
        String Nombre_Perito = Obtener_Dato_Consulta("SELECT " + Cat_Cat_Peritos_Externos.Campo_Nombre + "||' '||" + Cat_Cat_Peritos_Externos.Campo_Apellido_Paterno + "||' '|| " + Cat_Cat_Peritos_Externos.Campo_Apellido_Materno + " FROM " + Cat_Cat_Peritos_Externos.Tabla_Cat_Cat_Peritos_Externos + " WHERE " + Cat_Cat_Peritos_Externos.Campo_Perito_Externo_Id + " = '" + Perito_Id.Trim() + "'").ToUpper();
        String E_Mail = Obtener_Dato_Consulta("SELECT " + Cat_Cat_Peritos_Externos.Campo_Usuario + " FROM " + Cat_Cat_Peritos_Externos.Tabla_Cat_Cat_Peritos_Externos + " WHERE " + Cat_Cat_Peritos_Externos.Campo_Perito_Externo_Id + " = '" + Perito_Id + "'").ToUpper();
        DataTable Dt_Datos_Generales = Ds_Avaluo_Urbano.Tables["DT_DATOS_GENERALES"];
        DataTable Dt_Valor_Total_Predio = Ds_Avaluo_Urbano.Tables["DT_VALOR_TOTAL_PREDIO"];
        DataTable Dt_Caracteristicas_Terreno = Ds_Avaluo_Urbano.Tables["DT_CARACTERISTICAS_TERRENO"];
        DataTable Dt_Construccion = Ds_Avaluo_Urbano.Tables["DT_CONSTRUCCION"];
        DataTable Dt_Totales = Ds_Avaluo_Urbano.Tables["DT_TOTALES"];
        DataRow Dr_Avaluo;
        DataTable Dt_Documentos = (DataTable)Session["Dt_Documentos"];
        Dr_Avaluo = Dt_Datos_Generales.NewRow();
        Dr_Avaluo["MOTIVO_AVALUO"] = Cmb_Motivo_Avaluo.SelectedItem.Text;
        Dr_Avaluo["UBICACION"] = Txt_Ubicacion_Predio.Text;
        Dr_Avaluo["LOCALIDAD"] = Txt_Localidad.Text;
        Dr_Avaluo["PROPIEDAD"] = Txt_Propietario.Text;
        Dr_Avaluo["COLONIA"] = Txt_Colonia.Text;
        Dr_Avaluo["MUNICIPIO"] = Txt_Municipio.Text;
        Dr_Avaluo["DOMICILIO_NOTIFICAR"] = Txt_Domicilio_Not.Text;
        Dr_Avaluo["LOCALIDAD_NOTIFICAR"] = Txt_Localidad_Not.Text;
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
        Dr_Avaluo["VALUADOR"] = Nombre_Perito.ToUpper();
        if (Grid_Elementos_Construccion.Columns[6].Visible == true)
        {
            Dr_Avaluo["ANEXO"] = "SI";
        }
        else
        {
            Dr_Avaluo["ANEXO"] = "";
        }
        Dr_Avaluo["LEYENDAS"] = "";
        Dr_Avaluo["FECHA_ELABORACION"] = Convert.ToDateTime(Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Fecha_Creo].ToString());
        Dr_Avaluo["FECHA_AUTORIZACION"] = Convert.ToDateTime(Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Fecha_Autorizo].ToString());
        Dr_Avaluo["NO_PROGRESIVO"] = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_No_Avaluo].ToString();
        Dr_Avaluo["NO_VALUADOR"] = Convert.ToInt16(Perito_Id).ToString();
        Dr_Avaluo["FIRMANTE"] = "";
        Dr_Avaluo["PUESTO"] = "";
        foreach (DataRow Dr_Renglon_Actual in Dt_Documentos.Rows)
        {
            if (Dr_Renglon_Actual[Ope_Cat_Documentos_Avaluo_Au.Campo_Documento].ToString().Trim() == "FACHADA")
            {
                if (File.Exists(Server.MapPath(Dr_Renglon_Actual[Ope_Cat_Documentos_Avaluo_Au.Campo_Ruta_Documento].ToString())))
                    Dr_Avaluo["FACHADA"] = Convertir_Imagen_A_Cadena_Bytes(System.Drawing.Image.FromFile(Server.MapPath(Dr_Renglon_Actual[Ope_Cat_Documentos_Avaluo_Au.Campo_Ruta_Documento].ToString())));
            }
            else if (Dr_Renglon_Actual[Ope_Cat_Documentos_Avaluo_Au.Campo_Documento].ToString().Trim() == "LOCALIZACION")
            {
                if (File.Exists(Server.MapPath(Dr_Renglon_Actual[Ope_Cat_Documentos_Avaluo_Au.Campo_Ruta_Documento].ToString())))
                    Dr_Avaluo["CROQUIS"] = Convertir_Imagen_A_Cadena_Bytes(System.Drawing.Image.FromFile(Server.MapPath(Dr_Renglon_Actual[Ope_Cat_Documentos_Avaluo_Au.Campo_Ruta_Documento].ToString()))); ;
            }
        }
        Dt_Datos_Generales.Rows.Add(Dr_Avaluo);

        Dr_Avaluo = Dt_Valor_Total_Predio.NewRow();
        Dr_Avaluo["VALOR_INPA"] = Convert.ToDouble(Txt_Inpa.Text);
        Dr_Avaluo["VALOR_INPR"] = Convert.ToDouble(Txt_Inpr.Text);
        Dr_Avaluo["VALOR_VR"] = Convert.ToDouble(Txt_Vr.Text);
        Dr_Avaluo["VALOR_TOTAL_PREDIO"] = Convert.ToDouble(Txt_Valor_Total_Predio.Text);
        Dt_Valor_Total_Predio.Rows.Add(Dr_Avaluo);

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
            Dr_Avaluo["DENSIDAD_CONSTRUCCION"] = 0;
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

        if (Cmb_Tipo_Construccion.SelectedValue == "OTRO")
        {
            Dr_Avaluo["USO"] = Txt_Uso.Text.ToUpper();
        }
        else
        {
            Dr_Avaluo["USO"] = Cmb_Tipo_Construccion.SelectedValue;
        }
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
        DataTable Dt_Medidas = (DataTable)Session["Dt_Medidas"];
        DataTable Dt_MedColindancias = Ds_Avaluo_Urbano.Tables["DT_COLINDANCIAS"];
        foreach (DataRow Dr_Renglon in Dt_Medidas.Rows)
        {
            Dr_Avaluo = Dt_MedColindancias.NewRow();
            Dr_Avaluo["COLINDANCIA"] = Dr_Renglon[Ope_Cat_Colindancias_Ara.Campo_Medida_Colindancia].ToString();
            Dr_Avaluo["GRUPO"] = "A";
            Dt_MedColindancias.Rows.Add(Dr_Avaluo);
        }
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
        Ds_Avaluo_Urbano.Tables.Add(Dt_MedColindancias.Copy());
        return Ds_Avaluo_Urbano;
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
            Session["Postback_grid"] = null;
            Response.Redirect("../Ventanilla/Frm_Apl_Ventanilla.aspx");
        }
        else
        {
            Session["Postback_grid"] = null;
            Limpiar_Formulario();
            Btn_Nuevo.Visible = true;
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            Btn_Modificar.Visible = true;
            Btn_Modificar.AlternateText = "Modificar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            Btn_Imprimir.Visible = true;
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
            Session["Dt_Motivos_Rechazo"] = null;
            Session["Dt_Documentos"] = null;
            Response.Redirect("../Ventanilla/Frm_Apl_Ventanilla.aspx");
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

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Calculos_DataBound
    ///DESCRIPCIÓN: carga los datos en los componentes del grid
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 24/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
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
            Dt_Calculos.Rows[i]["FACTOR"] = Convert.ToDouble(Txt_Factor_Temporal.Text);
            Dt_Calculos.Rows[i]["FACTOR_EF"] = Convert.ToDouble(Txt_Factor_Ef_Temporal.Text);
            Dt_Calculos.Rows[i]["VALOR_PARCIAL"] = Convert.ToDouble(Txt_Total_Temporal.Text);
        }
        Grid_Calculos.Columns[3].Visible = false;
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
            Cls_Cat_Cat_Parametros_Negocio Parametros = new Cls_Cat_Cat_Parametros_Negocio();
            DataTable Dt_Parametros = Parametros.Consultar_Parametros();
            Crear_Mascara(Convert.ToInt16(Dt_Parametros.Rows[0][Cat_Cat_Parametros.Campo_Decimales_Redondeo].ToString()));
            TextBox Txt_Superficie_M2_Temporal = sender as TextBox;
            GridViewRow gvr = Txt_Superficie_M2_Temporal.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_Txt_Superficie_M2 = gvr.FindControl("Txt_Superficie_M2") as TextBox;
            try
            {
                if (Text_Txt_Superficie_M2.Text.Trim() != "")
                {
                    Text_Txt_Superficie_M2.Text = Convert.ToDouble(Text_Txt_Superficie_M2.Text).ToString("###,###,###,###,##0." + Mascara_Caracteres);
                }
                else
                {
                    Text_Txt_Superficie_M2.Text = "0." + Mascara_Caracteres;
                }
            }
            catch (Exception Exc)
            {
                Text_Txt_Superficie_M2.Text = "0." + Mascara_Caracteres;
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
            Cls_Cat_Cat_Parametros_Negocio Parametros = new Cls_Cat_Cat_Parametros_Negocio();
            DataTable Dt_Parametros = Parametros.Consultar_Parametros();
            Crear_Mascara(Convert.ToInt16(Dt_Parametros.Rows[0][Cat_Cat_Parametros.Campo_Decimales_Redondeo].ToString()));
            TextBox Txt_Factor_Temporal = sender as TextBox;
            GridViewRow gvr = Txt_Factor_Temporal.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_Txt_Factor = gvr.FindControl("Txt_Factor") as TextBox;
            try
            {
                if (Text_Txt_Factor.Text.Trim() != "")
                {
                    Text_Txt_Factor.Text = Convert.ToDouble(Text_Txt_Factor.Text).ToString("###,###,###,###,##0." + Mascara_Caracteres);
                }
                else
                {
                    Text_Txt_Factor.Text = "0." + Mascara_Caracteres;
                }
            }
            catch (Exception Exc)
            {
                Text_Txt_Factor.Text = "0." + Mascara_Caracteres;
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
            Cls_Cat_Cat_Parametros_Negocio Parametros = new Cls_Cat_Cat_Parametros_Negocio();
            DataTable Dt_Parametros = Parametros.Consultar_Parametros();
            Crear_Mascara(Convert.ToInt16(Dt_Parametros.Rows[0][Cat_Cat_Parametros.Campo_Decimales_Redondeo].ToString()));
            TextBox Txt_Factor_Ef_Temporal = sender as TextBox;
            GridViewRow gvr = Txt_Factor_Ef_Temporal.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_Txt_Factor_Ef = gvr.FindControl("Txt_Factor_Ef") as TextBox;
            try
            {
                if (Text_Txt_Factor_Ef.Text.Trim() != "")
                {
                    Text_Txt_Factor_Ef.Text = Convert.ToDouble(Text_Txt_Factor_Ef.Text).ToString("###,###,###,###,##0." + Mascara_Caracteres);
                }
                else
                {
                    Text_Txt_Factor_Ef.Text = "0." + Mascara_Caracteres;
                }
            }
            catch (Exception Exc)
            {
                Text_Txt_Factor_Ef.Text = "0." + Mascara_Caracteres;
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
    ///NOMBRE DE LA FUNCIÓN: Calcular_Valor_Parcial_Terreno
    ///DESCRIPCIÓN: Cálcula el valor parcial del grid_calculos y lo inserta en la caja de texto del grid
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 24/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    private void Calcular_Valor_Parcial_Terreno(int Index)
    {
        TextBox Txt_Superficie_M2 = (TextBox)Grid_Calculos.Rows[Index].Cells[1].FindControl("Txt_Superficie_M2");
        TextBox Txt_Valor_M2 = (TextBox)Grid_Calculos.Rows[Index].Cells[2].FindControl("Txt_Valor_M2");
        TextBox Txt_Factor = (TextBox)Grid_Calculos.Rows[Index].Cells[5].FindControl("Txt_Factor");
        TextBox Txt_Factor_Ef = (TextBox)Grid_Calculos.Rows[Index].Cells[7].FindControl("Txt_Factor_Ef");
        TextBox Text_Txt_Valor_Parcial = (TextBox)Grid_Calculos.Rows[Index].Cells[7].FindControl("Txt_Total");
        Text_Txt_Valor_Parcial.Text = (Convert.ToDouble(Txt_Superficie_M2.Text) * Convert.ToDouble(Txt_Valor_M2.Text) * Convert.ToDouble(Txt_Factor.Text) * Convert.ToDouble(Txt_Factor_Ef.Text)).ToString("###,###,###,###,###,##0.00");
        Calcular_Totales_Terreno();
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Calcular_Totales_Terreno
    ///DESCRIPCIÓN: Cálcula el total de la superficie de m2 y total del grid_calculos
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 24/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    private void Calcular_Totales_Terreno()
    {
        Cls_Cat_Cat_Parametros_Negocio Parametros = new Cls_Cat_Cat_Parametros_Negocio();
        DataTable Dt_Parametros = Parametros.Consultar_Parametros();
        Crear_Mascara(Convert.ToInt16(Dt_Parametros.Rows[0][Cat_Cat_Parametros.Campo_Decimales_Redondeo].ToString()));
        Double Superficie_Total = 0;
        Double Valor_Total = 0;
        foreach (GridViewRow Renglon_Grid in Grid_Calculos.Rows)
        {
            if (Renglon_Grid.Cells[0].Text != "INC. ESQ.")
            {
                Superficie_Total += Convert.ToDouble(((TextBox)Renglon_Grid.Cells[1].FindControl("Txt_Superficie_M2")).Text);
            }
            Valor_Total += Convert.ToDouble(((TextBox)Renglon_Grid.Cells[7].FindControl("Txt_Total")).Text);
        }
        Txt_Terreno_Superficie_Total.Text = Superficie_Total.ToString("###,###,###,###,###,##0." + Mascara_Caracteres);
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
        Cls_Cat_Cat_Parametros_Negocio Parametros = new Cls_Cat_Cat_Parametros_Negocio();
        Int16 renglones = Convert.ToInt16(Parametros.Consultar_Parametros().Rows[0][Cat_Cat_Parametros.Campo_Renglones_Calc_Construccion].ToString());
        Double Inc_Esq = Convert.ToDouble(Parametros.Consultar_Parametros().Rows[0][Cat_Cat_Parametros.Campo_Factor_Ef].ToString());
        DataTable Dt_Calculos = new DataTable();
        Dt_Calculos.Columns.Add("SECCION", typeof(String));
        Dt_Calculos.Columns.Add("SUPERFICIE_M2", typeof(Double));
        Dt_Calculos.Columns.Add("VALOR_TRAMO", typeof(Double));
        Dt_Calculos.Columns.Add("VALOR_TRAMO_ID", typeof(String));
        Dt_Calculos.Columns.Add("FACTOR", typeof(Double));
        Dt_Calculos.Columns.Add("FACTOR_EF", typeof(Double));
        Dt_Calculos.Columns.Add("VALOR_PARCIAL", typeof(Double));
        DataRow Dr_renglon;
        for (int i = 0; i < (renglones - 0); i++)
        {
            Dr_renglon = Dt_Calculos.NewRow();
            switch (i)
            {
                case 0:
                    Dr_renglon["SECCION"] = "I";
                    break;
                case 1:
                    Dr_renglon["SECCION"] = "II";
                    break;
                case 2:
                    Dr_renglon["SECCION"] = "III";
                    break;
                case 3:
                    Dr_renglon["SECCION"] = "IV";
                    break;
                case 4:
                    Dr_renglon["SECCION"] = "V";
                    break;
                case 5:
                    Dr_renglon["SECCION"] = "VI";
                    break;
                case 6:
                    Dr_renglon["SECCION"] = "VII";
                    break;
                case 7:
                    Dr_renglon["SECCION"] = "VIII";
                    break;
                case 8:
                    Dr_renglon["SECCION"] = "IX";
                    break;
                case 9:
                    Dr_renglon["SECCION"] = "X";
                    break;
                case 10:
                    Dr_renglon["SECCION"] = "XI";
                    break;
            }
            Dr_renglon["SUPERFICIE_M2"] = 0;
            Dr_renglon["VALOR_TRAMO"] = 0;
            Dr_renglon["VALOR_TRAMO_ID"] = "";
            Dr_renglon["FACTOR"] = 1;
            Dr_renglon["FACTOR_EF"] = 1;
            Dr_renglon["VALOR_PARCIAL"] = 0;
            Dt_Calculos.Rows.Add(Dr_renglon);
        }
        Dr_renglon = Dt_Calculos.NewRow();
        Dr_renglon["SECCION"] = "INC. ESQ.";
        Dr_renglon["SUPERFICIE_M2"] = 0;
        Dr_renglon["VALOR_TRAMO"] = 0;
        Dr_renglon["VALOR_TRAMO_ID"] = "";
        Dr_renglon["FACTOR"] = 1;
        Dr_renglon["FACTOR_EF"] = Inc_Esq;
        Dr_renglon["VALOR_PARCIAL"] = 0;
        Dt_Calculos.Rows.Add(Dr_renglon);
        Session["Dt_Grid_Calculos"] = Dt_Calculos.Copy();
        Grid_Calculos.Columns[3].Visible = true;
        Grid_Calculos.DataSource = Dt_Calculos;
        Grid_Calculos.PageIndex = 0;
        Grid_Calculos.DataBind();
        Grid_Calculos.Columns[3].Visible = false;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Valores_Construccion_DataBound
    ///DESCRIPCIÓN: Toma los valores de la tabla y los asigna directamente al componente
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 24/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
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
    ///NOMBRE DE LA FUNCIÓN: Guardar_Dt_Valores_Construccion
    ///DESCRIPCIÓN: guarda los datos directamente al componente y los asigna a la tabla  
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 24/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
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

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_Tipo_Constru_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_Tipo_Constru_TextChanged en el Grid
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
                            Dt_Valores_Construccion.Rows[index]["VALOR_CONSTRUCCION_ID"] = Valor_Construccion_Id;
                            Txt_Temporal_Val_Const_Id.Text = Valor_Construccion_Id;
                            Txt_Temporal_Valor_M2.Text = Valor_M2.ToString("###,###,###,###,##0.00");
                        }
                        else
                        {
                            TextBox Txt_Temporal_Val_Const_Id = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[5].FindControl("Txt_Valor_Construccion_Id");
                            TextBox Txt_Temporal_Valor_M2 = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[4].FindControl("Txt_Valor_X_M2");
                            Dt_Valores_Construccion.Rows[index]["VALOR_CONSTRUCCION_ID"] = "";
                            Txt_Temporal_Val_Const_Id.Text = "";
                            Txt_Temporal_Valor_M2.Text = Valor_M2.ToString("###,###,###,###,##0.00");
                        }
                    }
                }
                else
                {
                    Text_Txt_Tipo.Text = "0";
                    TextBox Txt_Temporal_Val_Const_Id = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[5].FindControl("Txt_Valor_Construccion_Id");
                    TextBox Txt_Temporal_Valor_M2 = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[4].FindControl("Txt_Valor_X_M2");
                    Dt_Valores_Construccion.Rows[index]["VALOR_CONSTRUCCION_ID"] = "";
                    Txt_Temporal_Val_Const_Id.Text = "";
                    Txt_Temporal_Valor_M2.Text = "0.00";
                }
            }
            catch (Exception Exc)
            {
                Text_Txt_Tipo.Text = "0";
                TextBox Txt_Temporal_Val_Const_Id = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[5].FindControl("Txt_Valor_Construccion_Id");
                TextBox Txt_Temporal_Valor_M2 = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[4].FindControl("Txt_Valor_X_M2");
                Dt_Valores_Construccion.Rows[index]["VALOR_CONSTRUCCION_ID"] = "";
                Txt_Temporal_Val_Const_Id.Text = "";
                Txt_Temporal_Valor_M2.Text = "0.00";
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
                            Dt_Valores_Construccion.Rows[index]["VALOR_CONSTRUCCION_ID"] = Valor_Construccion_Id;
                            Txt_Temporal_Val_Const_Id.Text = Valor_Construccion_Id;
                            Txt_Temporal_Valor_M2.Text = Valor_M2.ToString("###,###,###,###,##0.00");
                        }
                        else
                        {
                            TextBox Txt_Temporal_Val_Const_Id = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[5].FindControl("Txt_Valor_Construccion_Id");
                            TextBox Txt_Temporal_Valor_M2 = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[4].FindControl("Txt_Valor_X_M2");
                            Dt_Valores_Construccion.Rows[index]["VALOR_CONSTRUCCION_ID"] = "";
                            Txt_Temporal_Val_Const_Id.Text = "";
                            Txt_Temporal_Valor_M2.Text = Valor_M2.ToString("###,###,###,###,##0.00");
                        }
                    }
                }
                else
                {
                    Text_Txt_Con_Serv.Text = "0";
                    TextBox Txt_Temporal_Val_Const_Id = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[5].FindControl("Txt_Valor_Construccion_Id");
                    TextBox Txt_Temporal_Valor_M2 = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[4].FindControl("Txt_Valor_X_M2");
                    Dt_Valores_Construccion.Rows[index]["VALOR_CONSTRUCCION_ID"] = "";
                    Txt_Temporal_Val_Const_Id.Text = "";
                    Txt_Temporal_Valor_M2.Text = "0.00";
                }
            }
            catch (Exception Exc)
            {
                Text_Txt_Con_Serv.Text = "0";
                TextBox Txt_Temporal_Val_Const_Id = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[5].FindControl("Txt_Valor_Construccion_Id");
                TextBox Txt_Temporal_Valor_M2 = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[4].FindControl("Txt_Valor_X_M2");
                Dt_Valores_Construccion.Rows[index]["VALOR_CONSTRUCCION_ID"] = "";
                Txt_Temporal_Val_Const_Id.Text = "";
                Txt_Temporal_Valor_M2.Text = "0.00";
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
            TextBox Txt_Superficie_M2_Temporal = sender as TextBox;
            GridViewRow gvr = Txt_Superficie_M2_Temporal.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_Txt_Superficie_M2 = gvr.FindControl("Txt_Superficie_M2") as TextBox;
            try
            {
                if (Text_Txt_Superficie_M2.Text.Trim() != "")
                {
                    Text_Txt_Superficie_M2.Text = Convert.ToDouble(Text_Txt_Superficie_M2.Text).ToString("###,###,###,###,##0.00");
                }
                else
                {
                    Text_Txt_Superficie_M2.Text = "0.00";
                }
            }
            catch (Exception Exc)
            {
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
            TextBox Txt_Factor_Temporal = sender as TextBox;
            GridViewRow gvr = Txt_Factor_Temporal.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_Txt_Factor = gvr.FindControl("Txt_Factor") as TextBox;
            try
            {
                if (Text_Txt_Factor.Text.Trim() != "")
                {
                    Text_Txt_Factor.Text = Convert.ToDouble(Text_Txt_Factor.Text).ToString("###,###,###,###,##0.00");
                }
                else
                {
                    Text_Txt_Factor.Text = "0.00";
                }
            }
            catch (Exception Exc)
            {
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

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Calcular_Valor_Parcial_Construccion
    ///DESCRIPCIÓN: recorre el grid de valores de construcción, suma la superficie_m2 
    ///y el valor parcial y los visualiza en las cajas de texto correspondientes.
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 24/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    private void Calcular_Valor_Parcial_Construccion(int Index)
    {
        Double Valor_Parcial = 0;
        Valor_Parcial = Convert.ToDouble(((TextBox)Grid_Valores_Construccion.Rows[Index].Cells[3].FindControl("Txt_Superficie_M2")).Text) * Convert.ToDouble(((TextBox)Grid_Valores_Construccion.Rows[Index].Cells[4].FindControl("Txt_Valor_X_M2")).Text) * Convert.ToDouble(((TextBox)Grid_Valores_Construccion.Rows[Index].Cells[6].FindControl("Txt_Factor")).Text);
        TextBox Text_Txt_Valor_Parcial = (TextBox)Grid_Valores_Construccion.Rows[Index].Cells[7].FindControl("Txt_Total");
        Text_Txt_Valor_Parcial.Text = Valor_Parcial.ToString("###,###,###,###,###,##0.00");
        Calcular_Totales_Construccion();
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Calcular_Totales_Construccion
    ///DESCRIPCIÓN: recorre el grid de valores de construcción, suma la superficie_m2 y el valor parcial y los visualiza en las cajas de texto correspondientes.
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 24/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    private void Calcular_Totales_Construccion()
    {
        Double Superficie_Total = 0;
        Double Valor_Total = 0;
        foreach (GridViewRow Renglon_Grid in Grid_Valores_Construccion.Rows)
        {
            Superficie_Total += Convert.ToDouble(((TextBox)Renglon_Grid.Cells[3].FindControl("Txt_Superficie_M2")).Text);
            Valor_Total += Convert.ToDouble(((TextBox)Renglon_Grid.Cells[7].FindControl("Txt_Total")).Text);
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
        Cls_Cat_Cat_Parametros_Negocio Parametros = new Cls_Cat_Cat_Parametros_Negocio();
        Int16 renglones = Convert.ToInt16(Parametros.Consultar_Parametros().Rows[0][Cat_Cat_Parametros.Campo_Renglones_Calc_Construccion].ToString());
        DataTable Dt_Valores_Construccion = new DataTable();
        Dt_Valores_Construccion.Columns.Add("REFERENCIA", typeof(String));
        Dt_Valores_Construccion.Columns.Add("TIPO", typeof(Int16));
        Dt_Valores_Construccion.Columns.Add("CON_SERV", typeof(Int16));
        Dt_Valores_Construccion.Columns.Add("SUPERFICIE_M2", typeof(Double));
        Dt_Valores_Construccion.Columns.Add("VALOR_M2", typeof(Double));
        Dt_Valores_Construccion.Columns.Add("VALOR_CONSTRUCCION_ID", typeof(String));
        Dt_Valores_Construccion.Columns.Add("FACTOR", typeof(Double));
        Dt_Valores_Construccion.Columns.Add("VALOR_PARCIAL", typeof(Double));
        DataRow Dr_renglon;
        for (int i = 0; i < renglones; i++)
        {
            Dr_renglon = Dt_Valores_Construccion.NewRow();
            switch (i)
            {
                case 0:
                    Dr_renglon["REFERENCIA"] = "A";
                    break;
                case 1:
                    Dr_renglon["REFERENCIA"] = "B";
                    break;
                case 2:
                    Dr_renglon["REFERENCIA"] = "C";
                    break;
                case 3:
                    Dr_renglon["REFERENCIA"] = "D";
                    break;
                case 4:
                    Dr_renglon["REFERENCIA"] = "E";
                    break;
                case 5:
                    Dr_renglon["REFERENCIA"] = "F";
                    break;
                case 6:
                    Dr_renglon["REFERENCIA"] = "G";
                    break;
                case 7:
                    Dr_renglon["REFERENCIA"] = "H";
                    break;
                case 8:
                    Dr_renglon["REFERENCIA"] = "I";
                    break;
                case 9:
                    Dr_renglon["REFERENCIA"] = "J";
                    break;
                case 10:
                    Dr_renglon["REFERENCIA"] = "K";
                    break;
            }
            Dr_renglon["TIPO"] = 0;
            Dr_renglon["CON_SERV"] = 0;
            Dr_renglon["SUPERFICIE_M2"] = 0;
            Dr_renglon["VALOR_M2"] = 0;
            Dr_renglon["VALOR_CONSTRUCCION_ID"] = " ";
            Dr_renglon["FACTOR"] = 1;
            Dr_renglon["VALOR_PARCIAL"] = 0;
            Dt_Valores_Construccion.Rows.Add(Dr_renglon);
        }
        Session["Dt_Grid_Valores_Construccion"] = Dt_Valores_Construccion.Copy();
        Grid_Valores_Construccion.Columns[5].Visible = true;
        Grid_Valores_Construccion.DataSource = Dt_Valores_Construccion;
        Grid_Valores_Construccion.PageIndex = 0;
        Grid_Valores_Construccion.DataBind();
        Grid_Valores_Construccion.Columns[5].Visible = false;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial_Click
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
                Cuenta_Predial_ID = Convert.ToString(Session["CUENTA_PREDIAL_ID"]);
                Hdf_Cuenta_Predial_Id.Value = Cuenta_Predial_ID;
                Cuenta_Predial = Convert.ToString(Session["CUENTA_PREDIAL"]);
                Txt_Cuenta_Predial.Text = Cuenta_Predial;
                Cargar_Datos();
            }
        }
        Session.Remove("BUSQUEDA_CUENTAS_PREDIAL");
        Session.Remove("CUENTA_PREDIAL_ID");
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Calculos_RowCommand
    ///DESCRIPCIÓN          : Muestra los datos de la consulta
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Calculos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataTable Dt_Calculos = (DataTable)Session["Dt_Grid_Calculos"];
        Grid_Calculos.Columns[3].Visible = true;
        if (Session["VALOR_TRAMO_ID"] != null && Session["VALOR_M2"] != null)
        {
            TextBox Txt_Valor_M2_Temporal = (TextBox)Grid_Calculos.Rows[Convert.ToInt16(e.CommandArgument)].Cells[2].FindControl("Txt_Valor_M2");
            TextBox Txt_Tramo_Id_Temporal = (TextBox)Grid_Calculos.Rows[Convert.ToInt16(e.CommandArgument)].Cells[3].FindControl("Txt_Tramo_Id");
            Dt_Calculos.Rows[Convert.ToInt16(e.CommandArgument)]["VALOR_TRAMO_ID"] = Session["VALOR_TRAMO_ID"].ToString();
            Txt_Tramo_Id_Temporal.Text = Session["VALOR_TRAMO_ID"].ToString();
            Txt_Valor_M2_Temporal.Text = Convert.ToDouble(Session["VALOR_M2"].ToString()).ToString("###,###,###,###,##0.00");
            Session["VALOR_TRAMO_ID"] = null;
            Session["VALOR_M2"] = null;
            Calcular_Valor_Parcial_Terreno(Convert.ToInt16(e.CommandArgument));
        }
        Grid_Calculos.Columns[3].Visible = false;
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
            Consultar_Identificadores_Predio();
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

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Busqueda_Propietario
    ///DESCRIPCIÓN: Obtiene el propietario del la cuenta predial.
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 22/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
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
        { }

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
                M_Orden_Negocio.P_Propietario_ID = dataTable.Rows[0]["PROPIETARIO"].ToString();
                Txt_Propietario.Text = dataTable.Rows[0]["NOMBRE_PROPIETARIO"].ToString();
                M_Orden_Negocio.P_Nombre_Propietario = dataTable.Rows[0]["NOMBRE_PROPIETARIO"].ToString();
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
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Busqueda_Cuentas
    ///DESCRIPCIÓN: realiza una busqueda de las cuentas existentes
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/Ago/2011 06:31:08 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
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
            { }
        }
        catch (Exception Ex)
        { }
    }

    Cls_Ope_Pre_Orden_Variacion_Negocio M_Orden_Negocio;

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Crear_Tabla_Caracteristicas_Terreno1
    ///DESCRIPCIÓN: En base a las caracteristicas del terreno crea una tabla
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/Ago/2011 06:31:08 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    private DataTable Crear_Tabla_Caracteristicas_Terreno1()
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

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Crear_Tabla_Construccion_Terreno
    ///DESCRIPCIÓN: Crea la tabla de constricción de terreno para guardarla en la BD
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
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
        if (Rdb_Misma.Checked)
        {
            Tipo_Construccion = "MISMA";
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
        Dr_Renglon_Nuevo["TIPO_CONSTRUCCION"] = Tipo_Construccion;
        Dr_Renglon_Nuevo["CALIDAD_PROYECTO"] = Calidad_Proyecto;

        if (Cmb_Tipo_Construccion.SelectedValue == "OTRO")
        {
            Dr_Renglon_Nuevo["USO_CONSTRUCCION"] = Txt_Uso.Text.ToUpper();
        }
        else
        {
            Dr_Renglon_Nuevo["USO_CONSTRUCCION"] = Cmb_Tipo_Construccion.SelectedValue;
        }


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
        Double Valor_Construccion = 0;
        Double Valor_Terreno = 0;
        Double Valor_Inpa = 0;
        Double Valor_Inpr = 0;
        Double Valor_Total_Predio = 0;
        Double Valor_Vr = 0;
        Valor_Construccion = Convert.ToDouble(Txt_Construccion_Valor_Total.Text);
        Valor_Terreno = Convert.ToDouble(Txt_Terreno_Valor_Total.Text);
        Valor_Inpa = Convert.ToDouble(Txt_Inpa.Text);
        Valor_Inpr = Convert.ToDouble(Txt_Inpr.Text);
        Valor_Total_Predio = Valor_Construccion + Valor_Terreno;
        if (Valor_Inpr != 0)
        {
            Valor_Vr = Valor_Total_Predio * Valor_Inpa / Valor_Inpr;
        }
        else
        {
            Valor_Vr = 0;
        }
        Txt_Valor_Total_Predio.Text = Valor_Total_Predio.ToString("###,###,###,###,##0.00");
        Txt_Vr.Text = Valor_Vr.ToString("###,###,###,###,##0.00");
        Calcular_Precio_Avaluo();
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Calcular_Precio_Avaluo
    ///DESCRIPCIÓN: Cálcula el importe del avalúo
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    private void Calcular_Precio_Avaluo()
    {
        Double Valor_Total_Predio = 0;
        Double Precio_Avaluo = 0;
        Valor_Total_Predio = Convert.ToDouble(Txt_Valor_Total_Predio.Text);
        Precio_Avaluo = ((Valor_Total_Predio * Convert.ToDouble(Hdf_Factor_Cobro2.Value)) + Convert.ToDouble(Hdf_Base_Cobro.Value)) * (Convert.ToDouble(Hdf_Porcentaje_Cobro.Value) / 100);
        Txt_Precio_Avaluo.Text = Precio_Avaluo.ToString("###,###,###,###,##0.00");
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_Dens_Construccion_TextChanged
    ///DESCRIPCIÓN: Evento de la caja de texto Txt_Dens_Construccion
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
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
            Grid_Observaciones.SelectedIndex = -1;
            Grid_Observaciones.Columns[1].Visible = true;
            Grid_Observaciones.Columns[2].Visible = true;
            Grid_Observaciones.DataSource = (DataTable)Session["Dt_Motivos_Rechazo"];
            Grid_Observaciones.PageIndex = e.NewPageIndex;
            Grid_Observaciones.DataBind();
            Grid_Observaciones.Columns[1].Visible = false;
            Grid_Observaciones.Columns[2].Visible = false;
        }
        catch (Exception ex)
        {
            Lbl_Ecabezado_Mensaje.Text = ex.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Clasificacion_Zona_DataBound
    ///DESCRIPCIÓN: Cambia la página del grid
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
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

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Guardar_Grid_Clasificacion_Zona
    ///DESCRIPCIÓN: toma los datos de la tabla y los muestra en el componente.
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
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

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Servicios_Zona_DataBound
    ///DESCRIPCIÓN: toma los datos de la tabla y los muestra en el componente.
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
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

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Guardar_Grid_Servicios_Zona
    ///DESCRIPCIÓN: toma los datos de la tabla y los muestra en el componente.
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
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

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Construccion_Dominante_DataBound
    ///DESCRIPCIÓN: toma los datos de la tabla y los muestra en el componente.
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
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

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Guardar_Grid_Construccion_Dominante
    ///DESCRIPCIÓN: toma los datos de la tabla y los muestra en el componente.
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
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

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Crear_Tabla_Clasificacion_Zona
    ///DESCRIPCIÓN: toma los datos de la tabla y los muestra en el componente.
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
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

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Crear_Tabla_Servicios_Zona
    ///DESCRIPCIÓN: toma los datos de la tabla y los muestra en el componente.
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
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

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Crear_Tabla_Construccion_Dominante
    ///DESCRIPCIÓN: toma los datos de la tabla y los muestra en el componente.
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
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
    ///NOMBRE DE LA FUNCIÓN: Crear_Tabla_Caracteristicas_Terreno
    ///DESCRIPCIÓN: toma los datos de la tabla y los muestra en el componente.
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes
    ///DESCRIPCIÓN: Valida los datos ingresados
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Componentes()
    {
        Boolean Valido = true;
        String Msj_Error = "Error: ";

        if (Cmb_Motivo_Avaluo.SelectedValue == "SELECCIONE")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Seleccione un motivo para el avalúo.";
            Valido = false;
        }
        if (Txt_Cuenta_Predial.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese una Cuenta Predial.";
            Valido = false;
        }

        if (Txt_Region.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese la Región.";
            Valido = false;
        }
        if (Txt_Manzana.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese la Manzana.";
            Valido = false;
        }
        if (Txt_Lote.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese el Lote.";
            Valido = false;
        }
        if (Txt_Ubicacion_Predio.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese la ubicación del predio.";
            Valido = false;
        }
        if (Txt_Colonia.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese la colonia.";
            Valido = false;
        }
        if (Txt_Localidad.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese la localidad.";
            Valido = false;
        }
        if (Txt_Municipio.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese el Municipio.";
            Valido = false;
        }
        if (Txt_Propietario.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese el Propietario.";
            Valido = false;
        }
        if (Txt_Domicilio_Not.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese el domicilio para notificar.";
            Valido = false;
        }
        if (Txt_Colonia_Not.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese la colonia para notificar.";
            Valido = false;
        }
        if (Txt_Localidad_Not.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese la localidad a notificar.";
            Valido = false;
        }
        if (Txt_Municipio_Not.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese el municipio para notificar.";
            Valido = false;
        }

        if (Txt_Solicitante.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese el solicitante del Avalúo.";
            Valido = false;
        }

        if (Cmb_Tipo_Construccion.SelectedValue == "OTRO")
        {
            if (Txt_Uso.Text.Trim() == "")
            {
                Msj_Error += "<br/>";
                Msj_Error += "+ Ingrese el Uso.";
                Valido = false;
            }
        }

        if (Txt_Observaciones.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese las observaciones.";
            Valido = false;
        }
        if (Grid_Documentos.Rows.Count != 3)
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingresar las imágenes solicitadas.";
            Valido = false;
        }
        int Filas_R = Grid_Elementos_Construccion.Rows.Count;
        int Filas_LLenas = 0;
        for (int c = 2; c < Grid_Elementos_Construccion.Columns.Count; c++)
        {
            for (int f = 0; f < Grid_Elementos_Construccion.Rows.Count; f++)
            {
                TextBox Txt_A_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[f].Cells[c].FindControl("Txt_A");
                TextBox Txt_B_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[f].Cells[c].FindControl("Txt_B");
                TextBox Txt_C_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[f].Cells[c].FindControl("Txt_C");
                TextBox Txt_D_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[f].Cells[c].FindControl("Txt_D");
                TextBox Txt_E_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[f].Cells[c].FindControl("Txt_E");
                TextBox Txt_F_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[f].Cells[c].FindControl("Txt_F");
                TextBox Txt_G_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[f].Cells[c].FindControl("Txt_G");
                TextBox Txt_H_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[f].Cells[c].FindControl("Txt_H");
                TextBox Txt_I_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[f].Cells[c].FindControl("Txt_I");
                TextBox Txt_J_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[f].Cells[c].FindControl("Txt_J");
                TextBox Txt_K_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[f].Cells[c].FindControl("Txt_K");
                TextBox Txt_L_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[f].Cells[c].FindControl("Txt_L");
                TextBox Txt_M_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[f].Cells[c].FindControl("Txt_M");
                TextBox Txt_N_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[f].Cells[c].FindControl("Txt_N");
                TextBox Txt_O_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[f].Cells[c].FindControl("Txt_O");
                if (Txt_A_Temporal.Text != "" && c == 2)
                {
                    Filas_LLenas++;
                }

                if (Txt_B_Temporal.Text != "" && c == 3)
                {
                    Filas_LLenas++;
                }
                if (Txt_C_Temporal.Text != "" && c == 4)
                {
                    Filas_LLenas++;
                }
                if (Txt_D_Temporal.Text != "" && c == 5)
                {
                    Filas_LLenas++;
                }
                if (Txt_E_Temporal.Text != "" && c == 6)
                {
                    Filas_LLenas++;
                }
                if (Txt_F_Temporal.Text != "" && c == 7)
                {
                    Filas_LLenas++;
                }
                if (Txt_G_Temporal.Text != "" && c == 8)
                {
                    Filas_LLenas++;
                }
                if (Txt_H_Temporal.Text != "" && c == 9)
                {
                    Filas_LLenas++;
                }
                if (Txt_I_Temporal.Text != "" && c == 10)
                {
                    Filas_LLenas++;
                }
                if (Txt_J_Temporal.Text != "" && c == 11)
                {
                    Filas_LLenas++;
                }
                if (Txt_K_Temporal.Text != "" && c == 12)
                {
                    Filas_LLenas++;
                }
                if (Txt_L_Temporal.Text != "" && c == 13)
                {
                    Filas_LLenas++;
                }
                if (Txt_M_Temporal.Text != "" && c == 14)
                {
                    Filas_LLenas++;
                }
                if (Txt_N_Temporal.Text != "" && c == 15)
                {
                    Filas_LLenas++;
                }
                if (Txt_O_Temporal.Text != "" && c == 16)
                {
                    Filas_LLenas++;
                }
            }
            if (Filas_R != Filas_LLenas && Filas_LLenas != 0)
            {
                switch (c)
                {
                    case 2:
                        Msj_Error += "<br/>";
                        Msj_Error += "+ Ingresa todos los campos de la columna A.";
                        Msj_Error += "<br/>";
                        break;
                    case 3:
                        Msj_Error += "<br/>";
                        Msj_Error += "+ Ingresa todos los campos de la columna B.";
                        Msj_Error += "<br/>";
                        break;
                    case 4:
                        Msj_Error += "<br/>";
                        Msj_Error += "+ Ingresa todos los campos de la columna C.";
                        Msj_Error += "<br/>";
                        break;
                    case 5:
                        Msj_Error += "<br/>";
                        Msj_Error += "+ Ingresa todos los campos de la columna D.";
                        Msj_Error += "<br/>";
                        break;
                    case 6:
                        Msj_Error += "<br/>";
                        Msj_Error += "+ Ingresa todos los campos de la columna E.";
                        Msj_Error += "<br/>";
                        break;
                    case 7:
                        Msj_Error += "<br/>";
                        Msj_Error += "+ Ingresa todos los campos de la columna F.";
                        Msj_Error += "<br/>";
                        break;
                    case 8:
                        Msj_Error += "<br/>";
                        Msj_Error += "+ Ingresa todos los campos de la columna G.";
                        Msj_Error += "<br/>";
                        break;
                    case 9:
                        Msj_Error += "<br/>";
                        Msj_Error += "+ Ingresa todos los campos de la columna H.";
                        Msj_Error += "<br/>";
                        break;
                    case 10:
                        Msj_Error += "<br/>";
                        Msj_Error += "+ Ingresa todos los campos de la columna I.";
                        Msj_Error += "<br/>";
                        break;
                    case 11:
                        Msj_Error += "<br/>";
                        Msj_Error += "+ Ingresa todos los campos de la columna J.";
                        Msj_Error += "<br/>";
                        break;
                    case 12:
                        Msj_Error += "<br/>";
                        Msj_Error += "+ Ingresa todos los campos de la columna K.";
                        Msj_Error += "<br/>";
                        break;
                    case 13:
                        Msj_Error += "<br/>";
                        Msj_Error += "+ Ingresa todos los campos de la columna L.";
                        Msj_Error += "<br/>";
                        break;
                    case 14:
                        Msj_Error += "<br/>";
                        Msj_Error += "+ Ingresa todos los campos de la columna M.";
                        Msj_Error += "<br/>";
                        break;
                    case 15:
                        Msj_Error += "<br/>";
                        Msj_Error += "+ Ingresa todos los campos de la columna N.";
                        Msj_Error += "<br/>";
                        break;
                    case 16:
                        Msj_Error += "<br/>";
                        Msj_Error += "+ Ingresa todos los campos de la columna O.";
                        Msj_Error += "<br/>";
                        break;
                    default:
                        break;
                }
                Valido = false;
            }
            Filas_LLenas = 0;
        }
        if (!Valido)
        {
            Lbl_Ecabezado_Mensaje.Text = Msj_Error;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Valido;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_Cuenta_Predial_TextChanged
    ///DESCRIPCIÓN: evento de la caja de texto Txt_Cuenta_Predial_TextChanged
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_Cuenta_Predial_TextChanged(object sender, EventArgs e)
    {
        Cls_Cat_Pre_Cuentas_Predial_Negocio Cuenta_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        DataTable Dt_Cuenta;
        Cuenta_Predial.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.ToUpper();
        Dt_Cuenta = Cuenta_Predial.Consultar_Cuenta();
        if (Dt_Cuenta.Rows.Count > 0)
        {
            Hdf_Cuenta_Predial_Id.Value = Dt_Cuenta.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
        }
        else
        {
            Hdf_Cuenta_Predial_Id.Value = "";
            Txt_Cuenta_Predial.Text = "APERTURA";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Avalúo Urbano", "alert('La Cuenta Predial ingresada no existe actualmente');", true);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_Inpa_TextChanged
    ///DESCRIPCIÓN: evento de la caja de texto Txt_Inpa_TextChanged
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_Inpa_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Txt_Inpa.Text.Trim() == "")
            {
                Txt_Inpa.Text = "0.00";
            }
            else
            {
                Txt_Inpa.Text = Convert.ToDouble(Txt_Inpa.Text).ToString("##0.00");
            }
        }
        catch (Exception exc)
        {
            Txt_Inpa.Text = "0.00";
        }
        Calcular_Valor_Total_Predio();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_Inpr_TextChanged
    ///DESCRIPCIÓN: evento de la caja de texto Txt_Inpr_TextChanged
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_Inpr_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Txt_Inpr.Text.Trim() == "")
            {
                Txt_Inpr.Text = "0.00";
            }
            else
            {
                Txt_Inpr.Text = Convert.ToDouble(Txt_Inpr.Text).ToString("##0.00");
            }
        }
        catch (Exception exc)
        {
            Txt_Inpr.Text = "0.00";
        }
        Calcular_Valor_Total_Predio();
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
                if (File.Exists(Server.MapPath(Dr_Renglon["RUTA_DOCUMENTO"].ToString())))
                {
                    HyperLink Hlk_Enlace = new HyperLink();
                    Hlk_Enlace.Text = Path.GetFileName(Dr_Renglon["RUTA_DOCUMENTO"].ToString());
                    Hlk_Enlace.NavigateUrl = Dr_Renglon["RUTA_DOCUMENTO"].ToString();
                    Hlk_Enlace.CssClass = "enlace_fotografia";
                    Hlk_Enlace.Target = "blank";
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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Documentos_SelectedIndexChanged
    ///DESCRIPCIÓN: Cambia la acción a BAJA para eliminarlo del sistema.
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 04/Jun/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Documentos_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Eliminar registro y archivo en caso de tenerlo
        DataTable Dt_Documentos = (DataTable)Session["Dt_Documentos"];
        foreach (DataRow Dr_Documento in Dt_Documentos.Rows)
        {
            if (Dr_Documento["DOCUMENTO"].ToString() == Grid_Documentos.SelectedRow.Cells[2].Text && Dr_Documento["ACCION"].ToString() != "BAJA")
            {
                Dr_Documento["ACCION"] = "BAJA";
                break;
            }
        }
        Dt_Documentos.DefaultView.RowFilter = "ACCION <> 'BAJA'";
        Grid_Documentos.Columns[0].Visible = true;
        Grid_Documentos.Columns[1].Visible = true;
        Grid_Documentos.DataSource = Dt_Documentos;
        Grid_Documentos.PageIndex = 0;
        Grid_Documentos.DataBind();
        Grid_Documentos.Columns[0].Visible = false;
        Grid_Documentos.Columns[1].Visible = false;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Documento_Click
    ///DESCRIPCIÓN: Agrega el documento al grid.
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 04/Jun/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Agregar_Documento_Click(object sender, ImageClickEventArgs e)
    {
        if (Fup_Documento.FileName.Trim() != "")
        {
            DataTable Dt_Documentos = (DataTable)Session["Dt_Documentos"];
            Boolean Entro = false;
            foreach (DataRow Dr_Renglon in Dt_Documentos.Rows)
            {
                if (Dr_Renglon["DOCUMENTO"].ToString() == Cmb_Documento.SelectedValue && Dr_Renglon["ACCION"].ToString() != "BAJA")
                {
                    Entro = true;
                    break;
                }
            }
            if (!Entro)
            {
                DataRow Dr_Nuevo = Dt_Documentos.NewRow();
                Dr_Nuevo["NO_DOCUMENTO"] = " ";
                Dr_Nuevo["ACCION"] = "ALTA";
                Dr_Nuevo["EXTENSION_ARCHIVO"] = Path.GetExtension(Fup_Documento.FileName).ToLower();
                Dr_Nuevo["DOCUMENTO"] = Cmb_Documento.SelectedValue;
                Dr_Nuevo["BITS_ARCHIVO"] = Fup_Documento.FileBytes;
                Dr_Nuevo["ANIO_DOCUMENTO"] = DateTime.Now.Year;
                Dr_Nuevo["RUTA_DOCUMENTO"] = Cmb_Documento.SelectedValue.Replace(' ', '_') + Path.GetExtension(Fup_Documento.FileName).ToLower();
                Dt_Documentos.Rows.Add(Dr_Nuevo);
                Dt_Documentos.DefaultView.RowFilter = "ACCION <> 'BAJA'";
                Grid_Documentos.Columns[0].Visible = true;
                Grid_Documentos.Columns[1].Visible = true;
                Grid_Documentos.DataSource = Dt_Documentos;
                Grid_Documentos.DataBind();
                Grid_Documentos.Columns[0].Visible = false;
                Grid_Documentos.Columns[1].Visible = false;
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Convertir_Imagen_A_Cadena_Bytes
    ///DESCRIPCIÓN: Convierte la Imagen a una Cadena de Bytes.
    ///PROPIEDADES:   1.  P_Imagen.  Imagen a Convertir.    
    ///CREO: Francisco Antonio Gallardo Castañeda
    ///FECHA_CREO: Marzo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Byte[] Convertir_Imagen_A_Cadena_Bytes(System.Drawing.Image P_Imagen)
    {
        Byte[] Img_Bytes = null;
        try
        {
            if (P_Imagen != null)
            {
                MemoryStream MS_Tmp = new MemoryStream();
                P_Imagen.Save(MS_Tmp, P_Imagen.RawFormat);
                Img_Bytes = MS_Tmp.GetBuffer();
                MS_Tmp.Close();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = "Verificar.";
            Lbl_Mensaje_Error.Text = "Verificar.";
            Div_Contenedor_Msj_Error.Visible = false;
        }
        return Img_Bytes;
    }
}