using System;
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
using Presidencia.Recotizar.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Generar_Requisicion.Datos;


public partial class paginas_Compras_Frm_Ope_Com_Recotizar_Requisicion : System.Web.UI.Page
{
    ///*******************************************************************************
    ///PAGE_LOAD
    ///*******************************************************************************
    #region PAGE LOAD
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["SortDirection"] = "ASC";
            Configurar_Formulario("Inicio");
            Cls_Ope_Com_Recotizar_Requisicion_Negocio Clase_Negocio = new Cls_Ope_Com_Recotizar_Requisicion_Negocio(); 
            Llenar_Grid_Requisiciones(Clase_Negocio);
        }
    }
    #endregion
    ///*******************************************************************************
    ///METODOS
    ///*******************************************************************************
    #region Metodos

    public void Configurar_Formulario(String Estatus)
    {
        switch (Estatus)
        {
            case "Inicio":
                Div_Grid_Requisiciones.Visible = true;
                Grid_Requisiciones.Visible = true;
                Div_Detalle_Requisicion.Visible = false;
                Div_Busqueda.Visible = true;
                //Boton Modificar
                Btn_Modificar.Visible = true;
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Modificar.Enabled = true;
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Inicio";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                //
                Cmb_Estatus.Enabled = false;
                Cmb_Estatus.SelectedIndex = 0;


                break;
            case "General":

                Div_Detalle_Requisicion.Visible = true;
                Div_Grid_Requisiciones.Visible = false;
                Div_Busqueda.Visible = false;

                Btn_Modificar.Visible = true;
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Modificar.Enabled = true;
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Listado";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Cmb_Estatus.SelectedIndex = 0;
                Cmb_Estatus.Enabled = false;



                break;
            case "Modificar":

                Div_Detalle_Requisicion.Visible = true;
                Div_Grid_Requisiciones.Visible = false;
                Div_Busqueda.Visible = false;

                Btn_Modificar.Visible = true;
                Btn_Modificar.ToolTip = "Guardar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Modificar.Enabled = true;
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Cmb_Estatus.Enabled = false;
                Cmb_Estatus.SelectedIndex = 0;


                break;
        }
    }

    public void Limpiar_Componentes()
    {
        Txt_Folio.Text = "";
        Txt_Dependencia.Text = "";
        Txt_Fecha_Generacion.Text = "";
        Txt_Concepto.Text = "";
        Txt_Justificacion.Text = "";
        Txt_Tipo.Text = "";
        Txt_Tipo_Articulo.Text = "";
        Txt_Justificacion.Text = "";
        Txt_Subtotal.Text = "";
        Txt_Subtotal_Cotizado.Text = "";
        Txt_IVA.Text = "";
        Txt_IVA_Cotizado.Text = "";
        Txt_Total.Text = "";
        Txt_Total_Cotizado.Text = "";
        Session["No_Requisicion"] = null;
    }

    #endregion


    ///*******************************************************************************
    ///GRID
    ///*******************************************************************************
    #region Grid

    public void Llenar_Grid_Requisiciones(Cls_Ope_Com_Recotizar_Requisicion_Negocio Clase_Negocio)
    {
        DataTable Dt_Requisiciones = Clase_Negocio.Consultar_Requisiciones();
       
        if (Dt_Requisiciones.Rows.Count != 0)
        {
            Session["Ds_Requisiciones"] = Dt_Requisiciones;
            Grid_Requisiciones.DataSource = Dt_Requisiciones;
            Grid_Requisiciones.DataBind();
        }
        else
        {
            Grid_Requisiciones.EmptyDataText = "No se han encontrado registros.";
            //Lbl_Mensaje_Error.Text = "+ No se encontraron datos <br />";
            Grid_Requisiciones.DataSource = new DataTable();
            Grid_Requisiciones.DataBind();
        }
    }



    protected void Grid_Requisiciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        Cls_Ope_Com_Recotizar_Requisicion_Negocio Clase_Negocio = new Cls_Ope_Com_Recotizar_Requisicion_Negocio();

        GridViewRow Row = Grid_Requisiciones.SelectedRow;
        Clase_Negocio.P_No_Requisicion = Grid_Requisiciones.SelectedDataKey["No_Requisicion"].ToString();
        Session["No_Requisicion"] = Clase_Negocio.P_No_Requisicion;
        

        //Consultamos los detalles del producto seleccionado 
        DataTable Dt_Detalle_Requisicion = Clase_Negocio.Consultar_Requisiciones();
        //Mostramos el div de detalle y el grid de Requisiciones
        Configurar_Formulario("General");
        Txt_Dependencia.Text = Dt_Detalle_Requisicion.Rows[0]["DEPENDENCIA"].ToString().Trim();
        Txt_Folio.Text = Dt_Detalle_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Folio].ToString().Trim();
        Txt_Concepto.Text = Dt_Detalle_Requisicion.Rows[0]["CONCEPTO"].ToString().Trim();
        Txt_Tipo.Text = Dt_Detalle_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Tipo].ToString().Trim();
        Txt_Tipo_Articulo.Text = Dt_Detalle_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Tipo_Articulo].ToString().Trim();
        Txt_Justificacion.Text = Dt_Detalle_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Justificacion_Compra].ToString().Trim();
        if (Dt_Detalle_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Subtotal].ToString().Trim() != String.Empty)
            Txt_Subtotal.Text =String.Format("{0:C}",double.Parse( Dt_Detalle_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Subtotal].ToString().Trim()));
        if (Dt_Detalle_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Subtotal_Cotizado].ToString().Trim()!= String.Empty)
            Txt_Subtotal_Cotizado.Text = String.Format("{0:C}",double.Parse(Dt_Detalle_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Subtotal_Cotizado].ToString().Trim()));
        if(Dt_Detalle_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_IVA].ToString().Trim()!= String.Empty)
            Txt_IVA.Text = String.Format("{0:C}",double.Parse(Dt_Detalle_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_IVA].ToString().Trim()));
        if (Dt_Detalle_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_IVA_Cotizado].ToString().Trim() != String.Empty)
            Txt_IVA_Cotizado.Text = String.Format("{0:C}", double.Parse(Dt_Detalle_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_IVA_Cotizado].ToString().Trim()));
        if (Dt_Detalle_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Total].ToString().Trim() != String.Empty)
            Txt_Total.Text = String.Format("{0:C}", double.Parse(Dt_Detalle_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Total].ToString().Trim()));
        if (Dt_Detalle_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Total_Cotizado].ToString().Trim() != String.Empty)
            Txt_Total_Cotizado.Text = String.Format("{0:C}", double.Parse(Dt_Detalle_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Total_Cotizado].ToString().Trim()));
        //Consultamos los productos 
        DataTable Dt_Productos_Servicios = Clase_Negocio.Consultar_Productos_Servicios();
        if (Dt_Productos_Servicios.Rows.Count != 0)
        {
            Grid_Productos.DataSource = Dt_Productos_Servicios;
            Grid_Productos.DataBind();
        }
        else
        {
            Grid_Productos.EmptyDataText = "No se han encontrado Productos/Servicios.";
            //Lbl_Mensaje_Error.Text = "+ No se encontraron datos <br />";
            Grid_Productos.DataSource = new DataTable();
            Grid_Productos.DataBind();
        }



    }
    #endregion

    ///*******************************************************************************
    ///EVENTOS
    ///*******************************************************************************
    #region Eventos

    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        switch(Btn_Modificar.ToolTip)
        {
            case "Modificar":
                Configurar_Formulario("Modificar");
                Cmb_Estatus.Enabled = true;
                Cmb_Estatus.SelectedIndex = 0;

                break;

            case "Guardar":
                Cls_Ope_Com_Recotizar_Requisicion_Negocio Clase_Negocio = new Cls_Ope_Com_Recotizar_Requisicion_Negocio();
                String Mensaje = "";
                if (Cmb_Estatus.SelectedIndex == 0)
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario seleccionar un estatus.";
                }
                else
                {
                    Clase_Negocio.P_No_Requisicion = Session["No_Requisicion"].ToString().Trim();
                    Clase_Negocio.P_Estatus = Cmb_Estatus.SelectedValue;
                    Mensaje = Clase_Negocio.Modificar_Requisicion();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Recotización de Requisición", "alert('" + Mensaje + "');", true);
                    //Registramos el cambio de Estatus en la Requisicion
                    //REgistro de Estatus de Recotizacion
                    Cls_Ope_Com_Requisiciones_Datos.Registrar_Historial(Cmb_Estatus.SelectedValue,Clase_Negocio.P_No_Requisicion);
                    //Registro de estatus de FILTRADA
                    Cls_Ope_Com_Requisiciones_Datos.Registrar_Historial("FILTRADA", Clase_Negocio.P_No_Requisicion);
                    //Regresamos al estatus inicial del formulario

                    Configurar_Formulario("Inicio");
                    Clase_Negocio = new Cls_Ope_Com_Recotizar_Requisicion_Negocio();
                    Llenar_Grid_Requisiciones(Clase_Negocio);
                    Limpiar_Componentes();
                }


                break;
        }


    }
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Cls_Ope_Com_Recotizar_Requisicion_Negocio Clase_Negocio = new Cls_Ope_Com_Recotizar_Requisicion_Negocio();

        switch (Btn_Salir.ToolTip)
        {
            case "Cancelar":
                Configurar_Formulario("Inicio");
                Llenar_Grid_Requisiciones(Clase_Negocio);
                Limpiar_Componentes();


                break;
            case "Listado":
                Configurar_Formulario("Inicio");
                Llenar_Grid_Requisiciones(Clase_Negocio);
                Limpiar_Componentes();

                break;
            case "Inicio":
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                Limpiar_Componentes();

                break;

        }
    }

    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        if (Txt_Busqueda.Text == String.Empty)
        {
            Txt_Busqueda.Text = "";
            Lbl_Mensaje_Error.Text = "Es necesario ingresar el Folio de la requisicion";
            Div_Contenedor_Msj_Error.Visible = true;


        }
        else
        {
            Cls_Ope_Com_Recotizar_Requisicion_Negocio Clase_Negocio = new Cls_Ope_Com_Recotizar_Requisicion_Negocio();
            Clase_Negocio.P_Folio = Txt_Busqueda.Text.Trim();
            Llenar_Grid_Requisiciones(Clase_Negocio);
        }

    }

    #endregion





    
}
