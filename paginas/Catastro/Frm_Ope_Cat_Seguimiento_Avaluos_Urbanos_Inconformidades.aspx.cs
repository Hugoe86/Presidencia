using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Operacion_Cat_Avaluo_Urbano_In.Negocio;
using System.Data;
using Presidencia.Catalogo_Cat_Motivos_Avaluo.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Catalogo_Cat_Parametros.Negocio;
using Presidencia.Catalogo_Cat_Tabla_Factores.Negocio;
using Presidencia.Operacion_Calculo_Impuesto_Traslado.Negocio;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Registro_Peticion.Datos;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Numalet;
using Presidencia.Catalogo_Cat_Peritos_Externos.Negocio;
using Presidencia.Catalogo_Cat_Motivos_Rechazo.Negocio;
using Presidencia.Catalogo_Cat_Peritos_Internos.Negocio;
using Presidencia.Operacion_Resumen_Predio.Negocio;
using Operacion_Predial_Orden_Variacion.Negocio;
using System.IO;
using Presidencia.Catalogo_Cat_Valores_Inpa.Negocio;
using Presidencia.Catalogo_Cat_Valores_Inpr.Negocio;




public partial class paginas_Catastro_Frm_Ope_Cat_Seguimiento_Avaluos_Urbanos_Inconformidades : System.Web.UI.Page
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
                Session.Remove("ESTATUS_CUENTAS");
                Session.Remove("TIPO_CONTRIBUYENTE");
                Session["ESTATUS_CUENTAS"] = "IN ('PENDIENTE','ACTIVA','VIGENTE','BLOQUEADA','SUSPENDIDA','CANCELADA')";
                String Ventana_Modal1 = "Abrir_Ventana_Modal('../Predial/Ventanas_Emergentes/Frm_Busqueda_Avanzada_Cuentas_Predial.aspx', 'center:yes;resizable:no;status:no;dialogWidth:700px;dialogHeight:420px;dialogHide:true;help:no;scroll:no');";
                Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial.Attributes.Add("onclick", Ventana_Modal1);
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
        Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial.Enabled = false;
        //Btn_Mostrar_Busqueda_Avanzada_Recepcion_Oficios.Enabled = false;
        Txt_Clave_Catastral.Enabled = false;
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
        Txt_Inpa.Enabled = false;
        Txt_Inpr.Enabled = false;
        Txt_Vr.Enabled = false;
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
        Btn_Agregar_Documento.Enabled = false;
        Cmb_Documento.Enabled = false;
        Fup_Documento.Enabled = false;
   
        Grid_Documentos.Enabled = true;
        Btn_Busqueda_Avaluos_Av.Enabled = !Enabled;
        Grid_Colindancias.Enabled = false;
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
        Txt_Clave_Catastral.Text = "";
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
        Cmb_Estatus.SelectedIndex = 0;
        Txt_Busqueda.Text = "";
        Txt_Avaluo_Av.Text = "";
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
            Cls_Ope_Cat_Avaluo_Urbano_In_Negocio Avaluo_Urb = new Cls_Ope_Cat_Avaluo_Urbano_In_Negocio();
            if (Txt_Busqueda.Text.Trim() != "")
            {
                Avaluo_Urb.P_Folio = Txt_Busqueda.Text.Trim();
            }
            Avaluo_Urb.P_Perito_Interno_Id = Hdf_Perito_Interno_Id.Value;
            Grid_Avaluos_Urbanos.Columns[1].Visible = true;
            Grid_Avaluos_Urbanos.Columns[2].Visible = true;
            Grid_Avaluos_Urbanos.DataSource = Avaluo_Urb.Consultar_Avaluo_Urbano_In();
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
            Cls_Ope_Cat_Avaluo_Urbano_In_Negocio Aval_Urb = new Cls_Ope_Cat_Avaluo_Urbano_In_Negocio();
            Aval_Urb.P_No_Avaluo = Hdf_No_Avaluo.Value;
            Aval_Urb.P_Anio_Avaluo = Hdf_Anio_Avaluo.Value;
            Session["Dt_Tabla_Valores_Construccion"] = Aval_Urb.Consultar_Tabla_Valores_Construccion_In();
            Dt_Avaluo = Aval_Urb.Consultar_Avaluo_Urbano_In();
            Cargar_Datos_Avaluo(Dt_Avaluo);
            Cargar_Caracteristicas_Construccion(Aval_Urb.P_Dt_Caracteristicas_Terreno_In);
            Cargar_Construccion(Aval_Urb.P_Dt_Construccion_In);
            Crear_Tabla_Clasificacion_Zona(Aval_Urb.P_Dt_Clasificacion_Zona_In);
            Crear_Tabla_Construccion_Dominante(Aval_Urb.P_Dt_Construccion_Dominante_In);
            Crear_Tabla_Servicios_Zona(Aval_Urb.P_Dt_Servicios_Zona_In);
            Session["Dt_Grid_Calculos"] = Aval_Urb.P_Dt_Calculo_Valor_Terreno_In.Copy();
            Grid_Calculos.Columns[3].Visible = true;
            Grid_Calculos.DataSource = Aval_Urb.P_Dt_Calculo_Valor_Terreno_In;
            Grid_Calculos.PageIndex = 0;
            Grid_Calculos.DataBind();
            Grid_Calculos.Columns[3].Visible = false;
            Session["Dt_Grid_Elementos_Construccion"] = Aval_Urb.P_Dt_Elementos_Construccion_In.Copy();
            Grid_Elementos_Construccion.Columns[0].Visible = true;
            for (Int16 i = 1; i < (columnas + 1); i++)
            {
                Grid_Elementos_Construccion.Columns[i + 1].Visible = true;
            }
            Grid_Elementos_Construccion.DataSource = Aval_Urb.P_Dt_Elementos_Construccion_In;
            Grid_Elementos_Construccion.PageIndex = 0;
            Grid_Elementos_Construccion.DataBind();
            Grid_Elementos_Construccion.Columns[0].Visible = false;
            for (int i = (columnas + 1); i < 16; i++)
            {
                Grid_Elementos_Construccion.Columns[i + 1].Visible = false;
            }
            Session["Dt_Grid_Valores_Construccion"] = Aval_Urb.P_Dt_Calculo_Valor_Construccion_In.Copy();
            Grid_Valores_Construccion.DataSource = Aval_Urb.P_Dt_Calculo_Valor_Construccion_In;
            Grid_Valores_Construccion.PageIndex = 0;
            Grid_Valores_Construccion.DataBind();

            DataTable Dt_Archivos = Aval_Urb.P_Dt_Archivos_In.Copy();
            Dt_Archivos.Columns.Add("BITS_ARCHIVO", Type.GetType("System.Byte[]"));
            Dt_Archivos.Columns.Add("EXTENSION_ARCHIVO", typeof(String));
            Session["Dt_Documentos"] = Dt_Archivos;
            Grid_Documentos.Columns[0].Visible = true;
            Grid_Documentos.Columns[1].Visible = true;
            Grid_Documentos.DataSource = Dt_Archivos;
            Grid_Documentos.DataBind();
            Grid_Documentos.Columns[0].Visible = false;
            Grid_Documentos.Columns[1].Visible = false;
            //Cargar los demás grids con las tablas que trae el objeto Aval_Urb.
            //Fin de cargar datos del avalúo
            Div_Grid_Avaluo.Visible = false;
            Div_Datos_Avaluo.Visible = true;

            DataTable Dt_Valores;
            Session["Anio"] = Hdf_Anio_Avaluo.Value;
            Cls_Cat_Cat_Valores_Inpa_Negocio Valor_Inpa = new Cls_Cat_Cat_Valores_Inpa_Negocio();
            Cls_Cat_Cat_Valores_Inpr_Negocio Valor_Inpr = new Cls_Cat_Cat_Valores_Inpr_Negocio();
            Valor_Inpa.P_Anio = DateTime.Now.Year.ToString();
            Dt_Valores = Valor_Inpa.Consultar_Valores_Inpa();
            Double Val_Inpr = 0;
            Double Val_Inpa = 0;
            if (Dt_Valores.Rows.Count > 0)
            {
                Val_Inpa = Convert.ToDouble(Dt_Valores.Rows[0][Cat_Cat_Tab_Val_Inpa.Campo_Valor_Inpa].ToString());
                Hdf_Valor_Inpa_Id.Value = Dt_Valores.Rows[0][Cat_Cat_Tab_Val_Inpa.Campo_Valor_Inpa_Id].ToString();
                Txt_Inpa.Text = Val_Inpa.ToString("###,###,###,##0.00");
            }
            Valor_Inpr.P_Anio = DateTime.Now.Year.ToString();
            Dt_Valores = Valor_Inpr.Consultar_Valores_Inpr();
            if (Dt_Valores.Rows.Count > 0)
            {
                Val_Inpr = Convert.ToDouble(Dt_Valores.Rows[0][Cat_Cat_Tab_Val_Inpr.Campo_Valor_Inpr].ToString());
                Hdf_Valor_Inpr_Id.Value = Dt_Valores.Rows[0][Cat_Cat_Tab_Val_Inpr.Campo_Valor_Inpr_Id].ToString();
                Txt_Inpr.Text = Val_Inpr.ToString("###,###,###,##0.00");
            }

            DataTable Dt_Medidas = Aval_Urb.P_Dt_Medidas_In.Copy();
            Grid_Colindancias.Columns[0].Visible = true;
            Grid_Colindancias.DataSource = Dt_Medidas;
            Grid_Colindancias.DataBind();
            Grid_Colindancias.Columns[0].Visible = false;
            Session["Dt_Medidas"] = Dt_Medidas.Copy();
            Calcular_Totales_Construccion();
            Calcular_Totales_Terreno();
            Calcular_Valor_Total_Predio();
            Btn_Salir.AlternateText = "Atras";
            Div_Observaciones.Visible = true;
            DataTable Dt_Motivos_Rechazo;
            Aval_Urb.P_Estatus = "= 'VIGENTE'";
            Dt_Motivos_Rechazo = Aval_Urb.Consultar_Motivos_Rechazo_Avaluo_In();
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
            Txt_Region.Text = Dt_Avaluo.Rows[0]["REGION"].ToString();
            Txt_Manzana.Text = Dt_Avaluo.Rows[0]["MANZANA"].ToString();
            Txt_Lote.Text = Dt_Avaluo.Rows[0]["LOTE"].ToString();
            Txt_Avaluo_Av.Text = Dt_Avaluo.Rows[0]["ANIO_AVALUO_AV"].ToString() + "/" + Convert.ToInt16(Dt_Avaluo.Rows[0]["NO_AVALUO_AV"].ToString());
            Hdf_No_Avaluo.Value = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano_Re.Campo_No_Avaluo].ToString();
            Hdf_Anio_Avaluo.Value = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano_Re.Campo_Anio_Avaluo].ToString();
            Cmb_Motivo_Avaluo.SelectedIndex = Cmb_Motivo_Avaluo.Items.IndexOf(Cmb_Motivo_Avaluo.Items.FindByValue(HttpUtility.HtmlDecode(Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano_Re.Campo_Motivo_Avaluo_Id].ToString())));
            Cmb_Estatus.SelectedValue = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano_Av.Campo_Estatus].ToString();
            Cargar_Datos();
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
                if (Grid_Avaluos_Urbanos.SelectedRow.Cells[4].Text == "POR VALIDAR" || Grid_Avaluos_Urbanos.SelectedRow.Cells[4].Text == "RECHAZADO")
                {
                    if (Btn_Modificar.AlternateText.Equals("Modificar"))
                    {
                        Configuracion_Formulario(false);
                        Btn_Modificar.AlternateText = "Actualizar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                        Btn_Salir.AlternateText = "Cancelar";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    }
                    else
                    {
                        Cls_Ope_Cat_Avaluo_Urbano_In_Negocio Avaluo = new Cls_Ope_Cat_Avaluo_Urbano_In_Negocio();
                        Avaluo.P_Dt_Observaciones_In = (DataTable)Session["Dt_Motivos_Rechazo"];
                        Avaluo.P_No_Avaluo = Hdf_No_Avaluo.Value;
                        Avaluo.P_Anio_Avaluo = Hdf_Anio_Avaluo.Value;
                        Cambiar_Estatus_Avaluo_Rechazado_Autorizada();
                        Avaluo.P_Estatus = Cmb_Estatus.SelectedValue;
                        if ((Avaluo.Modificar_Observaciones_Avaluo_In()))
                        {
                            if (Cmb_Estatus.SelectedValue == "RECHAZADO")
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Avalúo Urbano", "alert('Se ha rechazado el Avalúo Exitosamente');", true);
                            }
                            else if (Cmb_Estatus.SelectedValue == "AUTORIZADO")
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Avalúo Urbano", "alert('Se ha Autorizado el Avalúo Exitosamente');", true);
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
        //Mandar imprimir el reporte
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

    protected void Grid_Elementos_Construccion_DataBound(object sender, EventArgs e)
    {
        DataTable Dt_Elementos_Construccion = (DataTable)Session["Dt_Grid_Elementos_Construccion"];
        for (int i = 0; i < Dt_Elementos_Construccion.Rows.Count; i++)
        {
            TextBox Txt_A_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[1].FindControl("Txt_A");
            TextBox Txt_B_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[2].FindControl("Txt_B");
            TextBox Txt_C_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[3].FindControl("Txt_C");
            TextBox Txt_D_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[4].FindControl("Txt_D");
            TextBox Txt_E_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[5].FindControl("Txt_E");
            TextBox Txt_F_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[6].FindControl("Txt_F");
            TextBox Txt_G_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[7].FindControl("Txt_G");
            TextBox Txt_H_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[8].FindControl("Txt_H");
            TextBox Txt_I_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[9].FindControl("Txt_I");
            TextBox Txt_J_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[10].FindControl("Txt_J");
            TextBox Txt_K_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[11].FindControl("Txt_K");
            TextBox Txt_L_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[12].FindControl("Txt_L");
            TextBox Txt_M_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[13].FindControl("Txt_M");
            TextBox Txt_N_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[14].FindControl("Txt_N");
            TextBox Txt_O_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[15].FindControl("Txt_O");
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
    ///NOMBRE DE LA FUNCIÓN: Txt_A_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_A en el Grid de Elementos de Construcción
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 08/Ago/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Txt_A_TextChanged(object sender, EventArgs e)
    {
        int index = 0;
        try
        {
            DataTable Dt_Elementos_Construccion = (DataTable)Session["Dt_Grid_Elementos_Construccion"];
            TextBox Text_A_Temporal = sender as TextBox;
            GridViewRow gvr = Text_A_Temporal.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_A = gvr.FindControl("Txt_A") as TextBox;
            Dt_Elementos_Construccion.Rows[index]["ELEMENTO_CONSTRUCCION_A"] = Text_A.Text.ToUpper();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_D_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_D en el Grid de Elementos de Construcción
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Txt_D_TextChanged(object sender, EventArgs e)
    {
        int index = 0;
        try
        {
            DataTable Dt_Elementos_Construccion = (DataTable)Session["Dt_Grid_Elementos_Construccion"];
            TextBox Text_D_Temporal = sender as TextBox;
            GridViewRow gvr = Text_D_Temporal.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_D = gvr.FindControl("Txt_D") as TextBox;
            Dt_Elementos_Construccion.Rows[index]["ELEMENTO_CONSTRUCCION_D"] = Text_D.Text.ToUpper();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_B_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_B en el Grid de Elementos de Construcción
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Txt_B_TextChanged(object sender, EventArgs e)
    {
        int index = 0;
        try
        {
            DataTable Dt_Elementos_Construccion = (DataTable)Session["Dt_Grid_Elementos_Construccion"];
            TextBox Text_B_Temporal = sender as TextBox;
            GridViewRow gvr = Text_B_Temporal.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_B = gvr.FindControl("Txt_B") as TextBox;
            Dt_Elementos_Construccion.Rows[index]["ELEMENTO_CONSTRUCCION_B"] = Text_B.Text.ToUpper();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_C_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_C en el Grid de Elementos de Construcción
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Txt_C_TextChanged(object sender, EventArgs e)
    {
        int index = 0;
        try
        {
            DataTable Dt_Elementos_Construccion = (DataTable)Session["Dt_Grid_Elementos_Construccion"];
            TextBox Text_C_Temporal = sender as TextBox;
            GridViewRow gvr = Text_C_Temporal.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_C = gvr.FindControl("Txt_C") as TextBox;
            Dt_Elementos_Construccion.Rows[index]["ELEMENTO_CONSTRUCCION_C"] = Text_C.Text.ToUpper();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_E_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_C en el Grid de Elementos de Construcción
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Txt_E_TextChanged(object sender, EventArgs e)
    {
        int index = 0;
        try
        {
            DataTable Dt_Elementos_Construccion = (DataTable)Session["Dt_Grid_Elementos_Construccion"];
            TextBox Text_E_Temporal = sender as TextBox;
            GridViewRow gvr = Text_E_Temporal.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_E = gvr.FindControl("Txt_E") as TextBox;
            Dt_Elementos_Construccion.Rows[index]["ELEMENTO_CONSTRUCCION_E"] = Text_E.Text.ToUpper();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_C_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_C en el Grid de Elementos de Construcción
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Txt_F_TextChanged(object sender, EventArgs e)
    {
        int index = 0;
        try
        {
            DataTable Dt_Elementos_Construccion = (DataTable)Session["Dt_Grid_Elementos_Construccion"];
            TextBox Text_F_Temporal = sender as TextBox;
            GridViewRow gvr = Text_F_Temporal.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_F = gvr.FindControl("Txt_F") as TextBox;
            Dt_Elementos_Construccion.Rows[index]["ELEMENTO_CONSTRUCCION_F"] = Text_F.Text.ToUpper();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_C_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_C en el Grid de Elementos de Construcción
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Txt_G_TextChanged(object sender, EventArgs e)
    {
        int index = 0;
        try
        {
            DataTable Dt_Elementos_Construccion = (DataTable)Session["Dt_Grid_Elementos_Construccion"];
            TextBox Text_G_Temporal = sender as TextBox;
            GridViewRow gvr = Text_G_Temporal.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_G = gvr.FindControl("Txt_G") as TextBox;
            Dt_Elementos_Construccion.Rows[index]["ELEMENTO_CONSTRUCCION_G"] = Text_G.Text.ToUpper();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_C_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_C en el Grid de Elementos de Construcción
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Txt_H_TextChanged(object sender, EventArgs e)
    {
        int index = 0;
        try
        {
            DataTable Dt_Elementos_Construccion = (DataTable)Session["Dt_Grid_Elementos_Construccion"];
            TextBox Text_H_Temporal = sender as TextBox;
            GridViewRow gvr = Text_H_Temporal.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_H= gvr.FindControl("Txt_H") as TextBox;
            Dt_Elementos_Construccion.Rows[index]["ELEMENTO_CONSTRUCCION_H"] = Text_H.Text.ToUpper();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_C_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_C en el Grid de Elementos de Construcción
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Txt_I_TextChanged(object sender, EventArgs e)
    {
        int index = 0;
        try
        {
            DataTable Dt_Elementos_Construccion = (DataTable)Session["Dt_Grid_Elementos_Construccion"];
            TextBox Text_I_Temporal = sender as TextBox;
            GridViewRow gvr = Text_I_Temporal.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_I = gvr.FindControl("Txt_I") as TextBox;
            Dt_Elementos_Construccion.Rows[index]["ELEMENTO_CONSTRUCCION_I"] = Text_I.Text.ToUpper();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_C_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_C en el Grid de Elementos de Construcción
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Txt_J_TextChanged(object sender, EventArgs e)
    {
        int index = 0;
        try
        {
            DataTable Dt_Elementos_Construccion = (DataTable)Session["Dt_Grid_Elementos_Construccion"];
            TextBox Text_J_Temporal = sender as TextBox;
            GridViewRow gvr = Text_J_Temporal.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_J = gvr.FindControl("Txt_J") as TextBox;
            Dt_Elementos_Construccion.Rows[index]["ELEMENTO_CONSTRUCCION_J"] = Text_J.Text.ToUpper();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_C_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_C en el Grid de Elementos de Construcción
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Txt_K_TextChanged(object sender, EventArgs e)
    {
        int index = 0;
        try
        {
            DataTable Dt_Elementos_Construccion = (DataTable)Session["Dt_Grid_Elementos_Construccion"];
            TextBox Text_K_Temporal = sender as TextBox;
            GridViewRow gvr = Text_K_Temporal.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_K = gvr.FindControl("Txt_K") as TextBox;
            Dt_Elementos_Construccion.Rows[index]["ELEMENTO_CONSTRUCCION_K"] = Text_K.Text.ToUpper();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_C_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_C en el Grid de Elementos de Construcción
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Txt_L_TextChanged(object sender, EventArgs e)
    {
        int index = 0;
        try
        {
            DataTable Dt_Elementos_Construccion = (DataTable)Session["Dt_Grid_Elementos_Construccion"];
            TextBox Text_L_Temporal = sender as TextBox;
            GridViewRow gvr = Text_L_Temporal.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_L = gvr.FindControl("Txt_L") as TextBox;
            Dt_Elementos_Construccion.Rows[index]["ELEMENTO_CONSTRUCCION_L"] = Text_L.Text.ToUpper();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_C_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_C en el Grid de Elementos de Construcción
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Txt_M_TextChanged(object sender, EventArgs e)
    {
        int index = 0;
        try
        {
            DataTable Dt_Elementos_Construccion = (DataTable)Session["Dt_Grid_Elementos_Construccion"];
            TextBox Text_M_Temporal = sender as TextBox;
            GridViewRow gvr = Text_M_Temporal.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_M = gvr.FindControl("Txt_M") as TextBox;
            Dt_Elementos_Construccion.Rows[index]["ELEMENTO_CONSTRUCCION_M"] = Text_M.Text.ToUpper();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_C_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_C en el Grid de Elementos de Construcción
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Txt_N_TextChanged(object sender, EventArgs e)
    {
        int index = 0;
        try
        {
            DataTable Dt_Elementos_Construccion = (DataTable)Session["Dt_Grid_Elementos_Construccion"];
            TextBox Text_N_Temporal = sender as TextBox;
            GridViewRow gvr = Text_N_Temporal.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_N = gvr.FindControl("Txt_N") as TextBox;
            Dt_Elementos_Construccion.Rows[index]["ELEMENTO_CONSTRUCCION_N"] = Text_N.Text.ToUpper();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_C_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_C en el Grid de Elementos de Construcción
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Txt_O_TextChanged(object sender, EventArgs e)
    {
        int index = 0;
        try
        {
            DataTable Dt_Elementos_Construccion = (DataTable)Session["Dt_Grid_Elementos_Construccion"];
            TextBox Text_O_Temporal = sender as TextBox;
            GridViewRow gvr = Text_O_Temporal.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_O = gvr.FindControl("Txt_O") as TextBox;
            Dt_Elementos_Construccion.Rows[index]["ELEMENTO_CONSTRUCCION_O"] = Text_O.Text.ToUpper();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    private void Crear_Dt_Elementos_Construccion()
    {
        DataTable Dt_Elementos_Construccion = new DataTable();
        Dt_Elementos_Construccion.Columns.Add("REFERENCIA", typeof(String));
        Dt_Elementos_Construccion.Columns.Add("ELEMENTO_CONSTRUCCION_A", typeof(String));
        Dt_Elementos_Construccion.Columns.Add("ELEMENTO_CONSTRUCCION_B", typeof(String));
        Dt_Elementos_Construccion.Columns.Add("ELEMENTO_CONSTRUCCION_C", typeof(String));
        Dt_Elementos_Construccion.Columns.Add("ELEMENTO_CONSTRUCCION_D", typeof(String));
        Dt_Elementos_Construccion.Columns.Add("ELEMENTO_CONSTRUCCION_E", typeof(String));
        Dt_Elementos_Construccion.Columns.Add("ELEMENTO_CONSTRUCCION_F", typeof(String));
        Dt_Elementos_Construccion.Columns.Add("ELEMENTO_CONSTRUCCION_G", typeof(String));
        Dt_Elementos_Construccion.Columns.Add("ELEMENTO_CONSTRUCCION_H", typeof(String));
        Dt_Elementos_Construccion.Columns.Add("ELEMENTO_CONSTRUCCION_I", typeof(String));
        Dt_Elementos_Construccion.Columns.Add("ELEMENTO_CONSTRUCCION_J", typeof(String));
        Dt_Elementos_Construccion.Columns.Add("ELEMENTO_CONSTRUCCION_K", typeof(String));
        Dt_Elementos_Construccion.Columns.Add("ELEMENTO_CONSTRUCCION_L", typeof(String));
        Dt_Elementos_Construccion.Columns.Add("ELEMENTO_CONSTRUCCION_M", typeof(String));
        Dt_Elementos_Construccion.Columns.Add("ELEMENTO_CONSTRUCCION_N", typeof(String));
        Dt_Elementos_Construccion.Columns.Add("ELEMENTO_CONSTRUCCION_O", typeof(String));
        DataRow Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "EDAD ESTIM.";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_E"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_F"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_G"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_H"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_I"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_J"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_K"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_L"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_M"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_N"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_O"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "MUROS";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_E"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_F"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_G"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_H"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_I"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_J"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_K"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_L"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_M"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_N"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_O"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "ESTRUCTURA";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_E"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_F"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_G"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_H"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_I"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_J"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_K"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_L"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_M"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_N"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_O"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "ENTREPISOS";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_E"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_F"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_G"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_H"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_I"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_J"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_K"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_L"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_M"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_N"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_O"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "TECHOS";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_E"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_F"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_G"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_H"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_I"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_J"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_K"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_L"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_M"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_N"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_O"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "PISOS";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_E"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_F"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_G"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_H"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_I"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_J"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_K"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_L"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_M"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_N"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_O"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "PUERTAS";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_E"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_F"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_G"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_H"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_I"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_J"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_K"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_L"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_M"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_N"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_O"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "VENTANAS";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_E"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_F"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_G"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_H"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_I"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_J"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_K"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_L"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_M"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_N"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_O"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "CARPINTERIA";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_E"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_F"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_G"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_H"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_I"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_J"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_K"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_L"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_M"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_N"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_O"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "HERRERIA";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_E"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_F"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_G"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_H"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_I"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_J"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_K"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_L"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_M"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_N"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_O"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "INST. ELEC.";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_E"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_F"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_G"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_H"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_I"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_J"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_K"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_L"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_M"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_N"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_O"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "INST. SANIT.";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_E"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_F"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_G"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_H"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_I"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_J"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_K"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_L"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_M"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_N"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_O"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "INST. ESP.";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_E"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_F"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_G"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_H"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_I"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_J"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_K"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_L"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_M"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_N"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_O"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "APLANADO";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_E"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_F"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_G"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_H"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_I"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_J"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_K"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_L"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_M"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_N"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_O"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "AC. EXT.";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_E"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_F"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_G"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_H"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_I"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_J"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_K"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_L"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_M"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_N"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_O"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "PINTURA";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_E"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_F"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_G"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_H"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_I"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_J"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_K"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_L"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_M"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_N"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_O"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "M. DE BAÑO";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_E"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_F"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_G"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_H"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_I"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_J"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_K"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_L"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_M"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_N"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_O"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "FACHADA";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_E"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_F"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_G"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_H"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_I"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_J"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_K"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_L"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_M"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_N"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_O"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Session["Dt_Grid_Elementos_Construccion"] = Dt_Elementos_Construccion.Copy();
        Grid_Elementos_Construccion.DataSource = Dt_Elementos_Construccion;
        Grid_Elementos_Construccion.PageIndex = 0;
        Grid_Elementos_Construccion.DataBind();
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
            Superficie_Total += Convert.ToDouble(Dr_Renglon["SUPERFICIE_M2"].ToString());
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
            //DataTable Dt_Ultimo_Movimiento = Rs_Consulta_Ope_Resumen_Predio.Consultar_Ultimo_Movimiento();
            //if (Dt_Ultimo_Movimiento.Rows.Count > 0)
            //{
            //    if (Dt_Ultimo_Movimiento.Rows[0]["Identificador"].ToString() != string.Empty)
            //    {
            //        if (Dt_Ultimo_Movimiento.Rows[0]["descripcion"].ToString() != "APERTURA")
            //        {
            //            Txt_Ultimo_Movimiento_General.Text = Dt_Ultimo_Movimiento.Rows[0]["Identificador"].ToString().Trim();
            //        }
            //    }
            //}
            //DataTable Dt_Consultar_Beneficio = Rs_Consulta_Ope_Resumen_Predio.Consultar_Beneficio();
            //if (Dt_Consultar_Beneficio.Rows.Count > 0)
            //{
            //    if (Dt_Consultar_Beneficio.Rows[0].ToString() != String.Empty)
            //    {
            //        if (Dt_Consultar_Beneficio.Rows[0].ToString() != "NO")
            //        {
            //            Lbl_Estatus.Text = " Beneficio Retirado por opción Global" + " " + Lbl_Estatus.Text;
            //        }
            //    }
            //}
            //if (dataTable.Rows[0]["Estatus"].ToString().Trim() == "BLOQUEADA")
            //{
            //    Lbl_Estatus.Text = " BLOQUEADA" + " " + Lbl_Estatus.Text;
            //}
            //if (dataTable.Rows[0]["Estatus"].ToString().Trim() == "BAJA")
            //{
            //    Lbl_Estatus.Text = " BAJA" + " " + Lbl_Estatus.Text;
            //}
            //if (dataTable.Rows[0]["Estatus"].ToString().Trim() == "CANCELADA")
            //{
            //    Lbl_Estatus.Text = " CANCELADA" + " " + Lbl_Estatus.Text;
            //}
            //if (dataTable.Rows[0]["Estatus"].ToString().Trim() == "SUSPENDIDA")
            //{
            //    Lbl_Estatus.Text = "SUSPENDIDA" + " " + Lbl_Estatus.Text;
            //}
            //if (dataTable.Rows[0]["Estatus"].ToString().Trim() == "PENDIENTE")
            //{
            //    Lbl_Estatus.Text = " Cuenta No Generada";
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Resumen de Predio", "alert('Cuenta No Generada')", true);

            //    Bloquear_Controles();
            //}
            //Txt_Cuenta_Origen.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen].ToString();
            //M_Orden_Negocio.P_Cuenta_Origen = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen].ToString();
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID].ToString() != string.Empty)
            //{
            //    Rs_Consulta_Ope_Resumen_Predio.P_Tipo_Predio = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID].ToString();
            //    DataTable Dt_Tipo_Predio = Rs_Consulta_Ope_Resumen_Predio.Consultar_Tipo_Predio();
            //    Txt_Tipo_Periodo_Impuestos.Text = Dt_Tipo_Predio.Rows[0]["Descripcion"].ToString().Trim();
            //}
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID].ToString() != string.Empty)
            //{
            //    Rs_Consulta_Ope_Resumen_Predio.P_Uso_Suelo_ID = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID].ToString();
            //    DataTable Dt_Uso_Suelo = Rs_Consulta_Ope_Resumen_Predio.Consultar_Uso_Predio();
            //    Txt_Uso_Predio_General.Text = Dt_Uso_Suelo.Rows[0]["Descripcion"].ToString();
            //}
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
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString() != string.Empty)
            //{
            //    Txt_Estatus_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString();
            //    M_Orden_Negocio.P_Estatus_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString();
            //}

            //Txt_Supe_Construida_General.Text = dataTable.Rows[0]["Superficie_Construida"].ToString();
            //M_Orden_Negocio.P_Superficie_Construida = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida].ToString();
            //Txt_Super_Total_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Total].ToString();
            //M_Orden_Negocio.P_Superficie_Total = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Total].ToString();
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
            Txt_Clave_Catastral.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral].ToString();
            M_Orden_Negocio.P_Clave_Catastral = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral].ToString();
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos].ToString() != string.Empty)
            //{
            //    Txt_Efectos_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos].ToString();
            //}
            //Txt_Valor_Fiscal_Impuestos.Text = Convert.ToDecimal(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal].ToString()).ToString("$ #,###,###,##0.00");
            //M_Orden_Negocio.P_Valor_Fiscal = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal].ToString();
            //Txt_Periodo_Corriente_Impuestos.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente].ToString();
            //M_Orden_Negocio.P_Periodo_Corriente_Inicial = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente].ToString();
            //Txt_Periodo_Corriente_Impuestos.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente].ToString();
            //M_Orden_Negocio.P_Periodo_Corriente_Inicial = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente].ToString();
            //if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString()))
            //{
            //    Txt_Cuota_Anual_Impuestos.Text = Convert.ToDecimal(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString()).ToString("$ #,###,###,##0.00");
            //    Decimal Cuota_Bimestral = Convert.ToDecimal(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString()) / 6;
            //    Txt_Cuota_Bimestral_Impuestos.Text = Convert.ToDecimal(Cuota_Bimestral).ToString("$ #,###,###,##0.00");
            //}
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual] != null)
            //{
            //    M_Orden_Negocio.P_Cuota_Anual = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString());
            //    M_Orden_Negocio.P_Cuota_Bimestral = ((Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString())) / 6);
            //}
            //Txt_Porciento_Exencion_Impuestos.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion].ToString();
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion] != null)
            //{
            //    M_Orden_Negocio.P_Exencion = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion].ToString());
            //}
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo] != null)
            //{
            //    DateTime Fecha_Avaluo;
            //    DateTime.TryParse(dataTable.Rows[0]["Fecha_Avaluo"].ToString(), out Fecha_Avaluo);
            //    if (Fecha_Avaluo <= DateTime.MinValue)
            //    {
            //        Txt_Fecha_Avaluo_Impuestos.Text = "";
            //        M_Orden_Negocio.P_Fecha_Avaluo = Fecha_Avaluo;
            //    }
            //    else
            //    {
            //        Txt_Fecha_Avaluo_Impuestos.Text = Fecha_Avaluo.ToString("dd-MMM-yyyy");
            //        M_Orden_Negocio.P_Fecha_Avaluo = Fecha_Avaluo;
            //    }
            //}
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion] != null)
            //{
            //    if (dataTable.Rows[0]["Termino_Exencion"].ToString().Trim() == "01/01/0001 12:00:00 a.m.")
            //    {
            //        Txt_Fecha_Termino_Extencion.Text = "";
            //        M_Orden_Negocio.P_Fecha_Termina_Exencion = Convert.ToDateTime(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion].ToString());
            //    }
            //    else
            //    {
            //        Txt_Fecha_Termino_Extencion.Text = String.Format("{0:dd/MMM/yyyy}", dataTable.Rows[0]["Termino_Exencion"].ToString().Trim());
            //        M_Orden_Negocio.P_Fecha_Termina_Exencion = Convert.ToDateTime(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion].ToString());
            //    }
            //}
            //Txt_Dif_Construccion_Impuestos.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion].ToString();
            //M_Orden_Negocio.P_Diferencia_Construccion = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion].ToString();

            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID] != null)
            //{
            //    M_Orden_Negocio.P_Cuota_Minima = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID].ToString());
            //    //Cmb_Cuota_Minima.SelectedValue = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID].ToString();
            //}
            ////Z1 HAY KU07A FIJ4!!! Seccion de carga de datos de la cuota fija
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija] != null)
            //{
            //    if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "NO" || dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "no" || dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "No")
            //    {
            //        Chk_Cuota_Fija.Checked = false;
            //        M_Orden_Negocio.P_Cuota_Fija = "NO";
            //    }
            //    if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "SI" || dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "si" || dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "Si")
            //    {
            //        Chk_Cuota_Fija.Checked = true;
            //        M_Orden_Negocio.P_Cuota_Fija = "SI";

            //        //----K4RG4R D47OZ D3 14 CU0T4 F1J4!!!!!
            //        if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija].ToString() != "")
            //        {
            //            M_Orden_Negocio.P_No_Cuota_Fija = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija].ToString().Trim().PadLeft(10, '0');
            //            Cargar_Datos_Cuota_Fija(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija].ToString());
            //        }
            //    }
            //}
            //if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID].ToString()))
            //{
            //    M_Orden_Negocio.P_Tasa = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID].ToString());
            //    Hdn_Tasa_ID.Value = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID].ToString();
            //}

            //if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_ID].ToString()))
            //{
            //    Rs_Consulta_Ope_Resumen_Predio.P_Tasa_Predial_ID = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_ID].ToString();
            //    DataTable Dt_Tasa = Rs_Consulta_Ope_Resumen_Predio.Consultar_Tasa();
            //    if (Dt_Tasa.Rows.Count > 0)
            //    {
            //        Txt_Tasa_Impuestos.Text = Dt_Tasa.Rows[0]["Descripcion"].ToString();
            //    }
            //}
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo].ToString() != String.Empty)
            //{
            //    //Cmb_Domicilio_Foraneo.SelectedValue = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo].ToString();
            //    //M_Orden_Negocio.P_Domicilio_Foraneo = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo].ToString();
            //}
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

            //Txt_Numero_Exterior_Propietario.Text = dataTable.Rows[0]["No_Exterior_Notificacion"].ToString();
            //M_Orden_Negocio.P_Exterior_Propietario = dataTable.Rows[0]["No_Exterior_Notificacion"].ToString();
            //Txt_Numero_Interior_Propietario.Text = dataTable.Rows[0]["No_Interior_Notificacion"].ToString();
            //M_Orden_Negocio.P_Interior_Propietario = dataTable.Rows[0]["No_Interior_Notificacion"].ToString();
            //Txt_Cod_Postal_Propietario.Text = dataTable.Rows[0]["Codigo_Postal"].ToString();
            //M_Orden_Negocio.P_CP_Propietario = dataTable.Rows[0]["Codigo_Postal"].ToString();


            //M_Orden_Negocio.P_Cuenta_Predial_ID = dataTable.Rows[0]["Cuenta_Predial_ID"].ToString().Trim();
            //M_Orden_Negocio.P_Dt_Copropietarios = M_Orden_Negocio.Consulta_Co_Propietarios();
            //Dt_Agregar_Co_Propietarios = M_Orden_Negocio.P_Dt_Copropietarios;
            //if (Dt_Agregar_Co_Propietarios.Rows.Count - 1 >= 0)
            //{
            //    for (int x = 0; x <= Dt_Agregar_Co_Propietarios.Rows.Count - 1; x++)
            //    {
            //        if (Dt_Agregar_Co_Propietarios.Rows[x]["Tipo"].ToString().Trim() == "COPROPIETARIO")
            //        {
            //            Txt_Copropietarios_Propietario.Text += Dt_Agregar_Co_Propietarios.Rows[x]["Nombre_Contribuyente"].ToString().Trim() + " \t" + Dt_Agregar_Co_Propietarios.Rows[x]["Rfc"].ToString().Trim() + "\n";
            //        }
            //    }
            //}
            //Consultar_Identificadores_Predio();
        }
        catch (Exception Ex)
        {
            throw new Exception("Cargar_Datos_Cuenta: " + Ex.Message);
        }
    }

    /////******************************************************************************* 
    /////NOMBRE DE LA FUNCIÓN: Consultar_Identificadores_Predio
    /////DESCRIPCIÓN: Obtiene el lote, región y manzana.
    /////PARAMETROS: 
    /////CREO: Miguel Angel Bedolla Moreno
    /////FECHA_CREO: 22/May/2012
    /////MODIFICO: 
    /////FECHA_MODIFICO:
    /////CAUSA_MODIFICACIÓN:
    /////*******************************************************************************
    //private void Consultar_Identificadores_Predio()
    //{
    //    try
    //    {
    //        DataTable Dt_Identificadores;
    //        Cls_Cat_Cat_Identificadores_Predio_Negocio Identificadores = new Cls_Cat_Cat_Identificadores_Predio_Negocio();
    //        Identificadores.P_Cuenta_Predial_Id = Hdf_Cuenta_Predial_Id.Value;
    //        Dt_Identificadores = Identificadores.Consultar_Identificadores_Predio();
    //        if (Dt_Identificadores.Rows.Count > 0)
    //        {
    //            Txt_Region.Text = Dt_Identificadores.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Region].ToString().Trim();
    //            Txt_Manzana.Text = Dt_Identificadores.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Manzana].ToString().Trim();
    //            Txt_Lote.Text = Dt_Identificadores.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Lote].ToString().Trim();
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        throw new Exception("Consultar_Identificadores_Predio: " + Ex.Message);
    //    }
    //}

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
        Valor_Vr = Valor_Total_Predio * Valor_Inpa * Valor_Inpr;
        Txt_Valor_Total_Predio.Text = Valor_Total_Predio.ToString("###,###,###,###,##0.00");
        Txt_Vr.Text = Valor_Vr.ToString("###,###,###,###,##0.00");
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
                Dr_Nuevo_Registro["MOTIVO_DESCRIPCION"] = Cmb_Motivos_Rechazo.SelectedItem.Text;
                Dt_Motivos_Rechazo.Rows.Add(Dr_Nuevo_Registro);
                Session["Dt_Motivos_Rechazo"] = Dt_Motivos_Rechazo.Copy();
                Grid_Observaciones.Columns[1].Visible = true;
                Grid_Observaciones.Columns[2].Visible = true;
                Grid_Observaciones.DataSource = Dt_Motivos_Rechazo;
                Grid_Observaciones.PageIndex = Grid_Observaciones.PageIndex;
                Grid_Observaciones.DataBind();
                Grid_Observaciones.Columns[1].Visible = false;
                Grid_Observaciones.Columns[2].Visible = false;
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
                    Dr_Nuevo_Registro["MOTIVO_DESCRIPCION"] = Cmb_Motivos_Rechazo.SelectedItem.Text;
                    Dt_Motivos_Rechazo.Rows.Add(Dr_Nuevo_Registro);
                    Session["Dt_Motivos_Rechazo"] = Dt_Motivos_Rechazo.Copy();
                    Dt_Motivos_Rechazo.DefaultView.Sort = "ESTATUS DESC";
                    Grid_Observaciones.Columns[1].Visible = true;
                    Grid_Observaciones.Columns[2].Visible = true;
                    Grid_Observaciones.Columns[4].Visible = true;
                    Grid_Observaciones.DataSource = Dt_Motivos_Rechazo;
                    Grid_Observaciones.PageIndex = Grid_Observaciones.PageIndex;
                    Grid_Observaciones.DataBind();
                    Grid_Observaciones.Columns[0].Visible = false;
                    Grid_Observaciones.Columns[1].Visible = false;
                    Grid_Observaciones.Columns[4].Visible = false;
                    Grid_Observaciones.SelectedIndex = -1;
                }
            }
        }
    }

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
    }

    protected void Grid_Observaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            DataTable Dt_Observaciones = (DataTable)Session["Dt_Motivos_Rechazo"];
            Grid_Observaciones.SelectedIndex = -1;
            Grid_Observaciones.Columns[1].Visible = true;
            Grid_Observaciones.Columns[2].Visible = true;
            Dt_Observaciones.DefaultView.Sort = "ESTATUS DESC";
            Grid_Observaciones.DataSource = Dt_Observaciones;
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
            Cmb_Estatus.SelectedIndex = 1;
        }
        else
        {
            Cmb_Estatus.SelectedIndex = 2;
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
    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN: Grid_Calles_SelectedIndexChanged
    /////DESCRIPCIÓN: Selecciona un registro del grid y toma los datos de los mismos campos del componente
    /////PROPIEDADES:     
    /////            
    /////CREO: Miguel Angel Bedolla Moreno
    /////FECHA_CREO: 05/May_2012
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //protected void Grid_Oficios_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (Grid_Oficios.SelectedIndex > -1)
    //    {
    //        Hdf_No_Oficio.Value = Grid_Oficios.SelectedRow.Cells[1].Text;

    //        if (Grid_Oficios.SelectedRow.Cells[4].Text != "&nbsp;" && Grid_Oficios.SelectedRow.Cells[4].Text != "")
    //        {
    //            Txt_Oficio.Text = Grid_Oficios.SelectedRow.Cells[4].Text;
    //        }
    //        else
    //        {
    //            Txt_Oficio.Text = "";

    //        }
    //        Grid_Oficios.SelectedIndex = -1;
    //        Mpe_Busqueda_Oficios.Hide();
    //    }
    //}


    //protected void Btn_Busqueda_Oficios_Click(object sender, EventArgs e)
    //{
    //    Llenar_Tabla_Oficios(0);
    //}


    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    /////DESCRIPCIÓN: Establece la configuración del formulario
    /////PROPIEDADES:     Enabled: Especifica si estara habilitado o no el componente
    /////            
    /////CREO: Miguel Angel Bedolla Moreno
    /////FECHA_CREO: 05/May_2012
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //private void Llenar_Tabla_Oficios(int Pagina)
    //{
    //    try
    //    {
    //        Cls_Ope_Cat_Recepcion_Oficios_Negocio Oficios = new Cls_Ope_Cat_Recepcion_Oficios_Negocio();
    //        DataTable Dt_Oficios;
    //        Oficios.P_No_Oficio_Recepcion = Txt_No_Oficio.Text.ToUpper();
    //        Oficios.P_Dependencia = Txt_Dependencia.Text.ToUpper();

    //        Dt_Oficios = Oficios.Consultar_Oficios_Avaluos();
    //        Grid_Oficios.Columns[1].Visible = true;

    //        Grid_Oficios.DataSource = Dt_Oficios;
    //        Grid_Oficios.PageIndex = Pagina;
    //        Grid_Oficios.DataBind();
    //        Grid_Oficios.Columns[1].Visible = false;
    //        Txt_No_Oficio.Text = "";
    //        Txt_Dependencia.Text = "";

    //    }
    //    catch (Exception E)
    //    {
    //        Lbl_Ecabezado_Mensaje.Text = E.Message.ToString();
    //        Lbl_Ecabezado_Mensaje.Visible = true;
    //        Div_Contenedor_Msj_Error.Visible = true;
    //    }
    //}

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN: Grid_Calles_PageIndexChanging
    /////DESCRIPCIÓN: Cambia la página del grid
    /////PROPIEDADES:     
    /////            
    /////CREO: Miguel Angel Bedolla Moreno
    /////FECHA_CREO: 05/May_2012
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //protected void Grid_Oficios_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    try
    //    {
    //        Llenar_Tabla_Oficios(e.NewPageIndex);
    //    }
    //    catch (Exception E)
    //    {
    //        Lbl_Ecabezado_Mensaje.Text = E.Message.ToString();
    //        Lbl_Ecabezado_Mensaje.Visible = true;
    //        Div_Contenedor_Msj_Error.Visible = true;
    //    }
    //}

    //protected void Btn_Cerrar_Ventana_Click(object sender, ImageClickEventArgs e)
    //{
    //    Mpe_Busqueda_Oficios.Hide();
    //    Txt_Busqueda.Text = "";
    //    Grid_Oficios.DataSource = null;
    //    Grid_Oficios.DataBind();
    //}

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
        //Session["Dt_Documentos"] = Dt_Documentos.Copy();
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

    protected void Grid_Colindancias_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable Dt_Medidas = (DataTable)Session["Dt_Medidas"];
        if (Dt_Medidas != null)
        {
            Dt_Medidas.DefaultView.RowFilter = "ACCION <> 'BAJA'";
            Grid_Colindancias.Columns[0].Visible = true;
            Grid_Colindancias.DataSource = Dt_Medidas;
            Grid_Colindancias.DataBind();
            Grid_Colindancias.Columns[0].Visible = false;

        }
    }
    protected void Grid_Colindancias_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Colindancias.SelectedIndex > -1)
        {
            DataTable Dt_Medidas = (DataTable)Session["Dt_Medidas"];
            if (Dt_Medidas != null)
            {
                foreach (DataRow Dr_Renglon in Dt_Medidas.Rows)
                {
                    if (Dr_Renglon[Ope_Cat_Colindancias_Aui.Campo_Medida_Colindancia].ToString() == Grid_Colindancias.SelectedRow.Cells[1].Text.ToUpper() && Dr_Renglon["ACCION"].ToString() != "BAJA")
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

}


