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

public partial class paginas_Catastro_Frm_Ope_Cat_Avaluo_Urbano : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Configuracion_Formulario(true);
                Llenar_Tabla_Avaluos_Urbanos(0);
                Llenar_Combo_Motivos_Avaluo();
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
        Cmb_Motivo_Avaluo.Enabled = !Enabled;
        Txt_Uso.Enabled = !Enabled;
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
        Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial.Enabled = !Enabled;
        Txt_Clave_Catastral.Enabled = false;
        Txt_Region.Enabled = false;
        Txt_Manzana.Enabled = false;
        Txt_Lote.Enabled = false;
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
        Chk_Agua.Enabled = !Enabled;
        Rdb_Ampliacion.Enabled = !Enabled;
        Rdb_Antiguas.Enabled = !Enabled;
        Chk_Banquetas.Enabled = !Enabled;
        Rdb_Buenas.Enabled = !Enabled;
        Chk_Campestre.Enabled = !Enabled;
        Chk_Comercial.Enabled = !Enabled;
        Chk_Drenaje.Enabled = !Enabled;
        Chk_Economica.Enabled = !Enabled;
        Chk_Hab_1a.Enabled = !Enabled;
        Chk_Industrial.Enabled = !Enabled;
        Chk_Luz.Enabled = !Enabled;
        Rdb_Malas.Enabled = !Enabled;
        Chk_Media.Enabled = !Enabled;
        Rdb_Mixtas.Enabled = !Enabled;
        Rdb_Modernas.Enabled = !Enabled;
        Rdb_Nueva.Enabled = !Enabled;
        Chk_Pavimentos.Enabled = !Enabled;
        Rdb_Pendiente.Enabled = !Enabled;
        Rdb_Plana.Enabled = !Enabled;
        Rdb_Calidad_Buena.Enabled = !Enabled;
        Rdb_Calidad_Mala.Enabled = !Enabled;
        Rdb_Calidad_Regular.Enabled = !Enabled;
        Rdb_Regulares.Enabled = !Enabled;
        Rdb_Remodelacion.Enabled = !Enabled;
        Rdb_Rentada.Enabled = !Enabled;
        Chk_Telefono.Enabled = !Enabled;
        Chk_Agua.Enabled = !Enabled;
        Chk_Agua.Enabled = !Enabled;
        Chk_Agua.Enabled = !Enabled;
        Chk_Agua.Enabled = !Enabled;
        Grid_Elementos_Construccion.Enabled = !Enabled;
        Grid_Calculos.Enabled = !Enabled;
        Grid_Valores_Construccion.Enabled = !Enabled;
        Grid_Avaluos_Urbanos.Enabled = Enabled;
        Txt_Busqueda.Enabled = Enabled;
        Btn_Buscar.Enabled = Enabled;
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
        Chk_Agua.Checked = false;
        Rdb_Ampliacion.Checked = false;
        Rdb_Antiguas.Checked = false;
        Chk_Banquetas.Checked = false;
        Rdb_Buenas.Checked = true;
        Chk_Campestre.Checked = false;
        Chk_Comercial.Checked = false;
        Chk_Drenaje.Checked = false;
        Chk_Economica.Checked = false;
        Chk_Hab_1a.Checked = false;
        Chk_Industrial.Checked = false;
        Chk_Luz.Checked = false;
        Rdb_Malas.Checked = false;
        Chk_Media.Checked = false;
        Rdb_Mixtas.Checked = true;
        Rdb_Modernas.Checked = false;
        Rdb_Nueva.Checked = true;
        Chk_Pavimentos.Checked = false;
        Rdb_Pendiente.Checked = true;
        Rdb_Plana.Checked = false;
        Rdb_Calidad_Buena.Checked = false;
        Rdb_Calidad_Mala.Checked = false;
        Rdb_Calidad_Regular.Checked = true;
        Rdb_Regulares.Checked = false;
        Rdb_Remodelacion.Checked = false;
        Rdb_Rentada.Checked = false;
        Chk_Telefono.Checked = false;
        Chk_Agua.Checked = false;
        Chk_Agua.Checked = false;
        Chk_Agua.Checked = false;
        Chk_Agua.Checked = false;
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
            Cls_Ope_Cat_Avaluo_Urbano_Negocio Avaluo_Urb = new Cls_Ope_Cat_Avaluo_Urbano_Negocio();
            if (Txt_Busqueda.Text.Trim() != "")
            {
                Avaluo_Urb.P_Folio = Txt_Busqueda.Text.Trim();
            }
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
            Session["Dt_Grid_Calculos"] = Aval_Urb.P_Dt_Calculo_Valor_Terreno.Copy();
            Grid_Calculos.DataSource = Aval_Urb.P_Dt_Calculo_Valor_Terreno;
            Grid_Calculos.PageIndex = 0;
            Grid_Calculos.DataBind();
            Session["Dt_Grid_Elementos_Construccion"] = Aval_Urb.P_Dt_Elementos_Construccion.Copy();
            Grid_Elementos_Construccion.DataSource = Aval_Urb.P_Dt_Elementos_Construccion;
            Grid_Elementos_Construccion.PageIndex = 0;
            Grid_Elementos_Construccion.DataBind();
            Session["Dt_Grid_Valores_Construccion"] = Aval_Urb.P_Dt_Calculo_Valor_Construccion.Copy();
            Grid_Valores_Construccion.DataSource = Aval_Urb.P_Dt_Calculo_Valor_Construccion;
            Grid_Valores_Construccion.PageIndex = 0;
            Grid_Valores_Construccion.DataBind();
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
            Calcular_Totales_Construccion();
            Calcular_Totales_Terreno();
            Calcular_Valor_Total_Predio();
            Btn_Salir.AlternateText = "Atras";
            Div_Observaciones.Visible = true;
            DataTable Dt_Motivos_Rechazo;
            Aval_Urb.P_Estatus = "= 'VIGENTE'";
            Dt_Motivos_Rechazo = Aval_Urb.Consultar_Motivos_Rechazo_Avaluo();
            Session["Dt_Motivos_Rechazo"] = Dt_Motivos_Rechazo.Copy();
            Grid_Observaciones.Columns[0].Visible = true;
            Grid_Observaciones.Columns[1].Visible = true;
            Grid_Observaciones.DataSource = Dt_Motivos_Rechazo;
            Grid_Observaciones.PageIndex = 0;
            Grid_Observaciones.DataBind();
            Grid_Observaciones.Columns[0].Visible = false;
            Grid_Observaciones.Columns[1].Visible = false;
        }
    }


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
            Cargar_Datos();
        }
    }

    private void Cargar_Caracteristicas_Construccion(DataTable Dt_Car_Construccion)
    {
        //if (Dt_Car_Construccion.Rows.Count > 0)
        //{
        //    String[] Clasificacion_Zona = Dt_Car_Construccion.Rows[0][Ope_Cat_Caract_Terreno_Au.Campo_Clasificacion_Zona].ToString().Split(',');
        //    String[] Servicios_Zona = Dt_Car_Construccion.Rows[0][Ope_Cat_Caract_Terreno_Au.Campo_Servicios_Zona].ToString().Split(',');
        //    String Construccion_Dominante = Dt_Car_Construccion.Rows[0][Ope_Cat_Caract_Terreno_Au.Campo_Const_Dominante].ToString();
        //    String Vias_Acceso = Dt_Car_Construccion.Rows[0][Ope_Cat_Caract_Terreno_Au.Campo_Vias_Acceso].ToString();
        //    String Fotografia = Dt_Car_Construccion.Rows[0][Ope_Cat_Caract_Terreno_Au.Campo_Fotografia].ToString();
        //    String Dens_Construccion = Convert.ToDouble(Dt_Car_Construccion.Rows[0][Ope_Cat_Caract_Terreno_Au.Campo_Dens_Const].ToString()).ToString("##0.00");
        //    if (Clasificacion_Zona.Length == 0 || Clasificacion_Zona.Length == 1)
        //    {
        //        Chk_Hab_1a.Checked = false;
        //        Chk_Media.Checked = false;
        //        Chk_Economica.Checked = false;
        //        Chk_Industrial.Checked = false;
        //        Chk_Comercial.Checked = false;
        //        Chk_Campestre.Checked = false;
        //    }
        //    else
        //    {
        //        for (Int16 i = 0; i < Clasificacion_Zona.Length; i++)
        //        {
        //            switch (Clasificacion_Zona[i].Trim())
        //            {
        //                case "HAB_DE_1A":
        //                    Chk_Hab_1a.Checked = true;
        //                    break;
        //                case "MEDIA":
        //                    Chk_Media.Checked = true;
        //                    break;
        //                case "ECONOMICA":
        //                    Chk_Economica.Checked = true;
        //                    break;
        //                case "INDUSTRIAL":
        //                    Chk_Industrial.Checked = true;
        //                    break;
        //                case "COMERCIAL":
        //                    Chk_Comercial.Checked = true;
        //                    break;
        //                case "CAMPESTRE":
        //                    Chk_Campestre.Checked = true;
        //                    break;
        //            }
        //        }
        //    }
        //    if (Servicios_Zona.Length == 0 || Servicios_Zona.Length == 1)
        //    {
        //        Chk_Agua.Checked = false;
        //        Chk_Luz.Checked = false;
        //        Chk_Drenaje.Checked = false;
        //        Chk_Telefono.Checked = false;
        //        Chk_Pavimentos.Checked = false;
        //        Chk_Banquetas.Checked = false;
        //    }
        //    else
        //    {
        //        for (Int16 i = 0; i < Clasificacion_Zona.Length; i++)
        //        {
        //            switch (Servicios_Zona[i].Trim())
        //            {
        //                case "AGUA":
        //                    Chk_Agua.Checked = true;
        //                    break;
        //                case "LUZ":
        //                    Chk_Luz.Checked = true;
        //                    break;
        //                case "DRENAJE":
        //                    Chk_Drenaje.Checked = true;
        //                    break;
        //                case "TELEFONO":
        //                    Chk_Telefono.Checked = true;
        //                    break;
        //                case "PAVIMENTOS":
        //                    Chk_Pavimentos.Checked = true;
        //                    break;
        //                case "BANQUETAS":
        //                    Chk_Banquetas.Checked = true;
        //                    break;
        //            }
        //        }
        //    }
        //    if (Construccion_Dominante.Trim() == "")
        //    {
        //        Rdb_Mixtas.Checked = false;
        //        Rdb_Antiguas.Checked = false;
        //        Rdb_Modernas.Checked = false;
        //    }
        //    else
        //    {
        //        switch (Construccion_Dominante.Trim())
        //        {
        //            case "MIXTA":
        //                Rdb_Mixtas.Checked = true;
        //                break;
        //            case "ANTIGUA":
        //                Rdb_Antiguas.Checked = true;
        //                break;
        //            case "MODERNA":
        //                Rdb_Modernas.Checked = true;
        //                break;
        //        }
        //    }
        //    if (Vias_Acceso.Trim() == "")
        //    {
        //        Rdb_Buenas.Checked = false;
        //        Rdb_Regulares.Checked = false;
        //        Rdb_Malas.Checked = false;
        //    }
        //    else
        //    {
        //        switch (Construccion_Dominante.Trim())
        //        {
        //            case "BUENA":
        //                Rdb_Buenas.Checked = true;
        //                break;
        //            case "REGULAR":
        //                Rdb_Regulares.Checked = true;
        //                break;
        //            case "MALA":
        //                Rdb_Malas.Checked = true;
        //                break;
        //        }
        //    }
        //    if (Fotografia.Trim() == "")
        //    {
        //        Rdb_Plana.Checked = false;
        //        Rdb_Pendiente.Checked = false;
        //    }
        //    else
        //    {
        //        switch (Construccion_Dominante.Trim())
        //        {
        //            case "PLANA":
        //                Rdb_Plana.Checked = true;
        //                break;
        //            case "PENDIENTE":
        //                Rdb_Pendiente.Checked = true;
        //                break;
        //        }
        //    }
        //}
    }

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
                    case "AMPLIACION":
                        Rdb_Calidad_Regular.Checked = true;
                        break;
                    case "REMODELACION":
                        Rdb_Calidad_Mala.Checked = true;
                        break;
                }
            }
            Txt_Uso.Text = Uso_Construccion;
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
                DataTable Dt_Valores;
                Configuracion_Formulario(false);
                Btn_Nuevo.AlternateText = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Modificar.Visible = false;
                Btn_Imprimir.Visible = false;
                Div_Datos_Avaluo.Visible = true;
                Div_Grid_Avaluo.Visible = false;
                Crear_Dt_Elementos_Construccion();
                Crear_Dt_Valores_Construccion();
                Crear_Dt_Calculos();
                Cls_Ope_Cat_Avaluo_Urbano_Negocio Avaluo = new Cls_Ope_Cat_Avaluo_Urbano_Negocio();
                Avaluo.P_Anio_Avaluo = DateTime.Now.Year.ToString();
                Session["Dt_Tabla_Valores_Construccion"] = Avaluo.Consultar_Tabla_Valores_Construccion();
                Session["Anio"] = DateTime.Now.Year.ToString();
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
                Txt_Vr.Text = "0.00";
            }
            else
            {
                Cls_Ope_Cat_Avaluo_Urbano_Negocio Avaluo = new Cls_Ope_Cat_Avaluo_Urbano_Negocio();
                Avaluo.P_Anio_Avaluo = DateTime.Now.Year.ToString();
                Avaluo.P_Cuenta_Predial_Id = Hdf_Cuenta_Predial_Id.Value;
                Avaluo.P_Estatus = Cmb_Estatus.SelectedValue;
                Avaluo.P_Motivo_Avaluo_Id = Cmb_Motivo_Avaluo.SelectedValue;
                Avaluo.P_Observaciones = Txt_Observaciones.Text.ToUpper();
                Cls_Cat_Cat_Peritos_Internos_Negocio Perito = new Cls_Cat_Cat_Peritos_Internos_Negocio();
                Perito.P_Empleado_Id = Cls_Sessiones.Empleado_ID;
                Avaluo.P_Perito_Externo_Id = Perito.Consultar_Peritos_Internos().Rows[0][Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id].ToString();
                Avaluo.P_Solicitante = Txt_Solicitante.Text.ToUpper();
                Avaluo.P_Ruta_Fachada_Inmueble = "";
                Avaluo.P_Valor_Inpa = Hdf_Valor_Inpa_Id.Value;
                Avaluo.P_Valor_Inpr = Hdf_Valor_Inpr_Id.Value;
                Avaluo.P_Valor_Total_Predio = Convert.ToDouble(Txt_Valor_Total_Predio.Text);
                Avaluo.P_Valor_Vr = Convert.ToDouble(Txt_Vr.Text);
                Avaluo.P_Dt_Calculo_Valor_Construccion = (DataTable)Session["Dt_Grid_Valores_Construccion"];
                Avaluo.P_Dt_Calculo_Valor_Terreno = (DataTable)Session["Dt_Grid_Calculos"];
                Avaluo.P_Dt_Caracteristicas_Terreno = Crear_Tabla_Caracteristicas_Terreno();
                Avaluo.P_Dt_Construccion = Crear_Tabla_Construccion_Terreno();
                Avaluo.P_Dt_Elementos_Construccion = (DataTable)Session["Dt_Grid_Elementos_Construccion"];
                if ((Avaluo.Alta_Valor_Construccion()))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Avalúo Urbano", "alert('Alta Exitosa');", true);
                    Btn_Salir_Click(null, null);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Avalúo Urbano", "alert('Alta Errónea');", true);
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
                if (Btn_Modificar.AlternateText.Equals("Modificar"))
                {
                    Configuracion_Formulario(false);
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.Visible = false;
                    Btn_Imprimir.Visible = false;
                    Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial.Enabled = false;
                }
                else
                {
                    //if (Validar_Componentes())
                    //{
                    Cls_Ope_Cat_Avaluo_Urbano_Negocio Avaluo = new Cls_Ope_Cat_Avaluo_Urbano_Negocio();
                    Avaluo.P_Anio_Avaluo = Hdf_Anio_Avaluo.Value;
                    Avaluo.P_No_Avaluo = Hdf_No_Avaluo.Value;
                    Avaluo.P_Cuenta_Predial_Id = Hdf_Cuenta_Predial_Id.Value;
                    Avaluo.P_Estatus = Cmb_Estatus.SelectedValue;
                    Avaluo.P_Motivo_Avaluo_Id = Cmb_Motivo_Avaluo.SelectedValue;
                    Avaluo.P_Observaciones = Txt_Observaciones.Text.ToUpper();
                    Cls_Cat_Cat_Peritos_Internos_Negocio Perito = new Cls_Cat_Cat_Peritos_Internos_Negocio();
                    Perito.P_Empleado_Id = Cls_Sessiones.Empleado_ID;
                    Avaluo.P_Perito_Externo_Id = Perito.Consultar_Peritos_Internos().Rows[0][Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id].ToString();
                    Avaluo.P_Solicitante = Txt_Solicitante.Text.ToUpper();
                    Avaluo.P_Ruta_Fachada_Inmueble = "";
                    Avaluo.P_Valor_Inpa = Hdf_Valor_Inpa_Id.Value;
                    Avaluo.P_Valor_Inpr = Hdf_Valor_Inpr_Id.Value;
                    Avaluo.P_Valor_Total_Predio = Convert.ToDouble(Txt_Valor_Total_Predio.Text);
                    Avaluo.P_Valor_Vr = Convert.ToDouble(Txt_Vr.Text);
                    Avaluo.P_Dt_Calculo_Valor_Construccion = (DataTable)Session["Dt_Grid_Valores_Construccion"];
                    Avaluo.P_Dt_Calculo_Valor_Terreno = (DataTable)Session["Dt_Grid_Calculos"];
                    Avaluo.P_Dt_Caracteristicas_Terreno = Crear_Tabla_Caracteristicas_Terreno();
                    Avaluo.P_Dt_Construccion = Crear_Tabla_Construccion_Terreno();
                    Avaluo.P_Dt_Elementos_Construccion = (DataTable)Session["Dt_Grid_Elementos_Construccion"];
                    Avaluo.P_Dt_Observaciones = (DataTable)Session["Dt_Motivos_Rechazo"];
                    if (Avaluo.P_Dt_Observaciones != null && Avaluo.P_Dt_Observaciones.Rows.Count > 0)
                    {
                        Avaluo.P_Estatus = "POR VALIDAR";
                    }
                    if ((Avaluo.Modificar_Valor_Construccion()))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Avalúo Urbano", "alert('Actualizacion Exitosa');", true);
                        Btn_Salir_Click(null, null);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Avalúo Urbano", "alert('Actualización Errónea');", true);
                    }
                    //}
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Seleccione el Avalúo a modificar.";
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
            Btn_Nuevo.Visible = true;
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
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
            Session["Dt_Motivos_Rechazo"] = null;
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
            Txt_A_Temporal.Text = Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_A"].ToString();
            Txt_B_Temporal.Text = Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_B"].ToString();
            Txt_C_Temporal.Text = Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_C"].ToString();
            Txt_D_Temporal.Text = Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_D"].ToString();
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

    private void Crear_Dt_Elementos_Construccion()
    {
        DataTable Dt_Elementos_Construccion = new DataTable();
        Dt_Elementos_Construccion.Columns.Add("REFERENCIA", typeof(String));
        Dt_Elementos_Construccion.Columns.Add("ELEMENTO_CONSTRUCCION_A", typeof(String));
        Dt_Elementos_Construccion.Columns.Add("ELEMENTO_CONSTRUCCION_B", typeof(String));
        Dt_Elementos_Construccion.Columns.Add("ELEMENTO_CONSTRUCCION_C", typeof(String));
        Dt_Elementos_Construccion.Columns.Add("ELEMENTO_CONSTRUCCION_D", typeof(String));
        DataRow Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "EDAD ESTIM.";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "MUROS";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "ESTRUCTURA";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "ENTREPISOS";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "TECHOS";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "PISOS";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "PUERTAS";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "VENTANAS";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "CARPINTERIA";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "HERRERIA";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "INST. ELEC.";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "INST. SANIT.";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "INST. ESP.";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "APLANADO";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "AC. EXT.";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "PINTURA";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "M. DE BAÑO";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Dr_renglon = Dt_Elementos_Construccion.NewRow();
        Dr_renglon["REFERENCIA"] = "FACHADA";
        Dr_renglon["ELEMENTO_CONSTRUCCION_A"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_B"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_C"] = "";
        Dr_renglon["ELEMENTO_CONSTRUCCION_D"] = "";
        Dt_Elementos_Construccion.Rows.Add(Dr_renglon);
        Session["Dt_Grid_Elementos_Construccion"] = Dt_Elementos_Construccion.Copy();
        Grid_Elementos_Construccion.DataSource = Dt_Elementos_Construccion;
        Grid_Elementos_Construccion.PageIndex = 0;
        Grid_Elementos_Construccion.DataBind();
    }

    protected void Grid_Calculos_DataBound(object sender, EventArgs e)
    {
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
            Txt_Superficie_M2_Temporal.Text = Convert.ToDouble(Dt_Calculos.Rows[i]["SUPERFICIE_M2"].ToString()).ToString("###,###,###,##0.00");
            Txt_Valor_M2_Temporal.Text = Convert.ToDouble(Dt_Calculos.Rows[i]["VALOR_TRAMO"].ToString()).ToString("###,###,###,##0.00");
            Txt_Tramo_Id_Temporal.Text = Dt_Calculos.Rows[i]["VALOR_TRAMO_ID"].ToString();
            Txt_Factor_Temporal.Text = Convert.ToDouble(Dt_Calculos.Rows[i]["FACTOR"].ToString()).ToString("###,###,###,##0.00");
            Txt_Factor_Ef_Temporal.Text = Convert.ToDouble(Dt_Calculos.Rows[i]["FACTOR_EF"].ToString()).ToString("###,###,###,##0.00");
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

    //protected void Construccion_Dominante_CheckedChanged(object sender, EventArgs e)
    //{
    //    CheckBox Chk_B_Temporal = (CheckBox)sender;
    //    if (Chk_B_Temporal == Chk_Antigua)
    //    {
    //        Chk_Moderna.Checked = false;
    //        Chk_Mix.Checked = false;
    //    }
    //    else if (Chk_B_Temporal == Chk_Moderna)
    //    {
    //        Chk_Antigua.Checked = false;
    //        Chk_Mix.Checked = false;
    //    }
    //    else if (Chk_B_Temporal == Chk_Mix)
    //    {
    //        Chk_Moderna.Checked = false;
    //        Chk_Antigua.Checked = false;
    //    }
    //}

    //protected void Construccion_Tipo_CheckedChanged(object sender, EventArgs e)
    //{
    //    CheckBox Chk_B_Temporal = (CheckBox)sender;
    //    if (Chk_B_Temporal == Chk_Nueva)
    //    {
    //        Chk_Ampliacion.Checked = false;
    //        Chk_Remodelacion.Checked = false;
    //        Chk_Rentada.Checked = false;
    //    }
    //    else if (Chk_B_Temporal == Chk_Ampliacion)
    //    {
    //        Chk_Nueva.Checked = false;
    //        Chk_Remodelacion.Checked = false;
    //        Chk_Rentada.Checked = false;
    //    }
    //    else if (Chk_B_Temporal == Chk_Remodelacion)
    //    {
    //        Chk_Nueva.Checked = false;
    //        Chk_Ampliacion.Checked = false;
    //        Chk_Rentada.Checked = false;
    //    }
    //    else if (Chk_B_Temporal == Chk_Rentada)
    //    {
    //        Chk_Nueva.Checked = false;
    //        Chk_Ampliacion.Checked = false;
    //        Chk_Remodelacion.Checked = false;
    //    }
    //}

    //protected void Calidad_Proyecto_CheckedChanged(object sender, EventArgs e)
    //{
    //    CheckBox Chk_B_Temporal = (CheckBox)sender;
    //    if (Chk_B_Temporal == Chk_Proy_Buena)
    //    {
    //        Chk_Proy_Mala.Checked = false;
    //        Chk_Proy_Regular.Checked = false;
    //    }
    //    else if (Chk_B_Temporal == Chk_Proy_Mala)
    //    {
    //        Chk_Proy_Buena.Checked = false;
    //        Chk_Proy_Regular.Checked = false;
    //    }
    //    else if (Chk_B_Temporal == Chk_Proy_Regular)
    //    {
    //        Chk_Proy_Buena.Checked = false;
    //        Chk_Proy_Mala.Checked = false;
    //    }
    //}

    //protected void Vias_Acceso_CheckedChanged(object sender, EventArgs e)
    //{
    //    CheckBox Chk_B_Temporal = (CheckBox)sender;
    //    if (Chk_B_Temporal == Chk_Buena)
    //    {
    //        Chk_Mala.Checked = false;
    //        Chk_Regular.Checked = false;
    //    }
    //    else if (Chk_B_Temporal == Chk_Mala)
    //    {
    //        Chk_Buena.Checked = false;
    //        Chk_Regular.Checked = false;
    //    }
    //    else if (Chk_B_Temporal == Chk_Regular)
    //    {
    //        Chk_Buena.Checked = false;
    //        Chk_Mala.Checked = false;
    //    }
    //}

    //protected void Fotografia_CheckedChanged(object sender, EventArgs e)
    //{
    //    CheckBox Chk_B_Temporal = (CheckBox)sender;
    //    if (Chk_B_Temporal == Chk_Plana)
    //    {
    //        Chk_Pendiente.Checked = false;
    //    }
    //    else if (Chk_B_Temporal == Chk_Pendiente)
    //    {
    //        Chk_Plana.Checked = false;
    //    }
    //}

    private DataTable Crear_Tabla_Caracteristicas_Terreno()
    {
        String Clasificacion_Zona = "";
        String Servicios_Zona = "";
        String Construccion_Dominante = "";
        String Vias_Acceso = "";
        String Fotografia = "";
        String Dens_Construccion = "";
        DataTable Dt_Caracteristicas_Terreno = new DataTable();
        Dt_Caracteristicas_Terreno.Columns.Add("CLASIFICACION_ZONA", typeof(String));
        Dt_Caracteristicas_Terreno.Columns.Add("SERVICIOS_ZONA", typeof(String));
        Dt_Caracteristicas_Terreno.Columns.Add("CONST_DOMINANTE", typeof(String));
        Dt_Caracteristicas_Terreno.Columns.Add("VIAS_ACCESO", typeof(String));
        Dt_Caracteristicas_Terreno.Columns.Add("FOTOGRAFIA", typeof(String));
        Dt_Caracteristicas_Terreno.Columns.Add("DENS_CONST", typeof(String));
        DataRow Dr_Renglon_Nuevo = Dt_Caracteristicas_Terreno.NewRow();
        if (Chk_Hab_1a.Checked)
        {
            Clasificacion_Zona += "HAB_DE_1A, ";
        }
        if (Chk_Media.Checked)
        {
            Clasificacion_Zona += "MEDIA, ";
        }
        if (Chk_Economica.Checked)
        {
            Clasificacion_Zona += "ECONOMICA, ";
        }
        if (Chk_Industrial.Checked)
        {
            Clasificacion_Zona += "INDUSTRIAL, ";
        }
        if (Chk_Comercial.Checked)
        {
            Clasificacion_Zona += "COMERCIAL, ";
        }
        if (Chk_Campestre.Checked)
        {
            Clasificacion_Zona += "CAMPESTRE, ";
        }

        if (Chk_Agua.Checked)
        {
            Servicios_Zona += "AGUA,";
        }
        if (Chk_Drenaje.Checked)
        {
            Servicios_Zona += "DRENAJE,";
        }
        if (Chk_Luz.Checked)
        {
            Servicios_Zona += "LUZ,";
        }
        if (Chk_Telefono.Checked)
        {
            Servicios_Zona += "TELEFONO,";
        }
        if (Chk_Pavimentos.Checked)
        {
            Servicios_Zona += "PAVIMENTOS,";
        }
        if (Chk_Banquetas.Checked)
        {
            Servicios_Zona += "BANQUETAS,";
        }

        if (Rdb_Antiguas.Checked)
        {
            Construccion_Dominante = "ANTIGUA";
        }
        else if (Rdb_Modernas.Checked)
        {
            Construccion_Dominante = "MODERNA";
        }
        else if (Rdb_Mixtas.Checked)
        {
            Construccion_Dominante = "MIXTA";
        }

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
        Dr_Renglon_Nuevo["CLASIFICACION_ZONA"] = Clasificacion_Zona;
        Dr_Renglon_Nuevo["SERVICIOS_ZONA"] = Servicios_Zona;
        Dr_Renglon_Nuevo["CONST_DOMINANTE"] = Construccion_Dominante;
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
}