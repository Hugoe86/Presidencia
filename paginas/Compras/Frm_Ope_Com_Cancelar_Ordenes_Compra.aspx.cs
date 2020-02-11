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
using Presidencia.Cancelar_Ordenes.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;

public partial class paginas_Compras_Frm_Ope_Com_Cancelar_Ordenes_Compra : System.Web.UI.Page
{

    ///*******************************************************************************
    ///PAGE_LOAD
    ///*******************************************************************************
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            ViewState["SortDirection"] = "ASC";
            Cls_Ope_Com_Cancelar_Ordenes_Compra_Negocio Clase_Negocio = new Cls_Ope_Com_Cancelar_Ordenes_Compra_Negocio();
            Configurar_Formulario("Inicio");
            Llenar_Grid_Ordenes_Compra(Clase_Negocio);
        }
    }

    #endregion

    ///*******************************************************************************
    ///METODOS
    ///*******************************************************************************
    #region Metodos

    public void Busqueda()
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Cls_Ope_Com_Cancelar_Ordenes_Compra_Negocio Clase_Negocio = new Cls_Ope_Com_Cancelar_Ordenes_Compra_Negocio();
        if (Txt_Fecha_Inicio.Text != Txt_Fecha_Final.Text)
        {
            Verificar_Fecha(Clase_Negocio);
        }
        if (Div_Contenedor_Msj_Error.Visible == false)
        {
            //En caso de no existir Errores realizamos la consulta de la busqueda Avanzada
            if (Cmb_Estatus_Busqueda.SelectedIndex != 0)
            {
                Clase_Negocio.P_Estatus = Cmb_Estatus_Busqueda.SelectedValue;
            }
            if (Txt_Orden_Compra_Busqueda.Text.Trim() != String.Empty)
            {
                Clase_Negocio.P_Folio_Busqueda = Txt_Orden_Compra_Busqueda.Text.Trim();
            }
            if (Txt_Req_Busqueda.Text.Trim() != String.Empty)
            {
                Clase_Negocio.P_No_Requisicio = Txt_Req_Busqueda.Text.Trim();
            }
            //llamamos el metodo de llenar el grid de ordenes compra pra que realice la busqueda
            Llenar_Grid_Ordenes_Compra(Clase_Negocio);
            //Ingresamos la fecha inicial 
            Txt_Fecha_Inicio.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Txt_Fecha_Final.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Cmb_Estatus_Busqueda.SelectedIndex = 0;
            Txt_Orden_Compra_Busqueda.Text = "";
            Txt_Req_Busqueda.Text = "";
        }
    }

    public void Configurar_Formulario(String Estatus)
    {
        switch (Estatus)
        {
            case "Inicio":
                Div_Busquedas_Avanzadas.Visible = true;
                Div_Grid_Ordenes_Compra.Visible = true;
                Div_Contenido_Orden_Compra.Visible = false;
                //Boton Modificar
                Btn_Modificar.Visible = false;
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Modificar.Enabled = true;
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Inicio";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Txt_Fecha_Inicio.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                Txt_Fecha_Final.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                Txt_Req_Busqueda.Text = "";
                Txt_Orden_Compra_Busqueda.Text = "";

                break;

            case "General":
                Div_Busquedas_Avanzadas.Visible = false;
                Div_Grid_Ordenes_Compra.Visible = false;
                Div_Contenido_Orden_Compra.Visible = true;
                //Boton Modificar
                Btn_Modificar.Visible = true;
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Modificar.Enabled = true;
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Listado";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                
                break;

            case "Modificar":
                Div_Busquedas_Avanzadas.Visible = false;
                Div_Grid_Ordenes_Compra.Visible = false;
                Div_Contenido_Orden_Compra.Visible = true;
                //Boton Modificar
                Btn_Modificar.Visible = true;
                Btn_Modificar.ToolTip = "Guardar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Modificar.Enabled = true;
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";

                break;

        }
    }

    public void Limpiar_Componentes()
    {
        Txt_No_Orden_Compra.Text = "";
        Txt_No_Requisicion.Text = "";
        Txt_No_Reserva.Text = "";
        Cmb_Estatus.SelectedIndex = 0;
        Txt_Fecha_Cancelacion.Text = "";
        Txt_Proveedor.Text = "";
        Txt_Codigo_Programatico.Text = "";
        Txt_Justificacion.Text = "";
        Txt_Motivo_Cancelacion.Text = "";

    }

    public void Habilitar_Componentes(bool Estatus)
    {
        Txt_No_Orden_Compra.Enabled = false;
        Txt_No_Requisicion.Enabled = false;
        Txt_No_Reserva.Enabled = false;
        Cmb_Estatus.Enabled = Estatus;
        Txt_Fecha_Cancelacion.Enabled = false;
        Txt_Proveedor.Enabled = false;
        Txt_Codigo_Programatico.Enabled = false;
        Txt_Justificacion.Enabled = false;
        Txt_Motivo_Cancelacion.Enabled = Estatus;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN: Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///en la busqueda del Modalpopup
    ///PARAMETROS: 1.-TextBox Fecha_Inicial 
    ///            2.-TextBox Fecha_Final
    ///            3.-Label Mensaje_Error
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public bool Verificar_Fecha(Cls_Ope_Com_Cancelar_Ordenes_Compra_Negocio Clase_Negocio)
    {

        //Variables que serviran para hacer la convecion a datetime las fechas y poder validarlas 
        DateTime Date1 = new DateTime();
        DateTime Date2 = new DateTime();
        bool Fecha_Valida = true;

        if ((Txt_Fecha_Inicio.Text.Length == 11) && (Txt_Fecha_Final.Text.Length == 11))
        {
            //Convertimos el Texto de los TextBox fecha a dateTime
            Date1 = DateTime.Parse(Txt_Fecha_Inicio.Text);
            Date2 = DateTime.Parse(Txt_Fecha_Final.Text);
            //Validamos que las fechas sean iguales o la final sea mayor que la inicias, si no se manda un mensaje de error 
            if ((Date1 < Date2) | (Date1 == Date2))
            {
                //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                Clase_Negocio.P_Fecha_Inicio = Formato_Fecha(Txt_Fecha_Inicio.Text);
                Clase_Negocio.P_Fecha_Fin = Formato_Fecha(Txt_Fecha_Final.Text);

            }
            else
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text += "+ Fecha no valida <br />";
            }
        }
        else
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text += "+ Fecha no valida <br />";
        }

        return Fecha_Valida;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Formato_Fecha
    ///DESCRIPCIÓN: Metodo que cambia el mes dic a dec para que oracle lo acepte
    ///PARAMETROS:  1.- String Fecha, es la fecha a la cual se le cambiara el formato 
    ///                     en caso de que cumpla la condicion del if
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 2/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public String Formato_Fecha(String Fecha)
    {

        String Fecha_Valida = Fecha;
        //Se le aplica un split a la fecha 
        String[] aux = Fecha.Split('/');
        //Se modifica el es a solo mayusculas para que oracle acepte el formato. 
        switch (aux[1])
        {
            case "dic":
                aux[1] = "DEC";
                break;
        }
        //Concatenamos la fecha, y se cambia el orden a DD-MMM-YYYY para que sea una fecha valida para oracle
        Fecha_Valida = aux[0] + "-" + aux[1] + "-" + aux[2];
        return Fecha_Valida;
    }// fin de Formato_Fecha



    #endregion

    ///*******************************************************************************
    ///GRID
    ///*******************************************************************************
    #region Grid

    public void Llenar_Grid_Ordenes_Compra(Cls_Ope_Com_Cancelar_Ordenes_Compra_Negocio Clase_Negocio)
    {
        DataTable Dt_Ordenes_Compra= Clase_Negocio.Consultar_Ordenes_Compra();
        if(Dt_Ordenes_Compra.Rows.Count != 0)
        {
            Grid_Ordenes_Compra.DataSource = Dt_Ordenes_Compra;
            Grid_Ordenes_Compra.DataBind();
            Session["Dt_Ordenes_Compra"] = Dt_Ordenes_Compra;

        }
        else
        {
            Grid_Ordenes_Compra.EmptyDataText = "No se encontrado registros.";
            //Lbl_Mensaje_Error.Text = "+ No se encontraron datos <br />";
            Grid_Ordenes_Compra.DataSource = new DataTable();
            Grid_Ordenes_Compra.DataBind();
        }

    }

    protected void Grid_Ordenes_Compra_SelectedIndexChanged(object sender, EventArgs e)
    {
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
        Cls_Ope_Com_Cancelar_Ordenes_Compra_Negocio Clase_Negocio = new Cls_Ope_Com_Cancelar_Ordenes_Compra_Negocio();
        Clase_Negocio.P_No_Orden_Compra = Grid_Ordenes_Compra.SelectedDataKey["NO_ORDEN_COMPRA"].ToString();
        Session["NO_ORDEN_COMPRA"] = Clase_Negocio.P_No_Orden_Compra.Trim();
        
        //Realizamos la consulta de la Orden de Compra
        DataTable Dt_Orden_Compra_Detalle = Clase_Negocio.Consultar_Ordenes_Compra();
        Txt_No_Orden_Compra.Text = Dt_Orden_Compra_Detalle.Rows[0][Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra].ToString().Trim();
        Txt_No_Requisicion.Text = Dt_Orden_Compra_Detalle.Rows[0]["FOLIO_REQUISICION"].ToString().Trim();
        Txt_No_Reserva.Text = Dt_Orden_Compra_Detalle.Rows[0][Ope_Com_Ordenes_Compra.Campo_No_Reserva].ToString().Trim();
        Txt_Proveedor.Text = Dt_Orden_Compra_Detalle.Rows[0]["PROVEEDOR"].ToString().Trim();
        Txt_Codigo_Programatico.Text = Dt_Orden_Compra_Detalle.Rows[0][Ope_Com_Requisiciones.Campo_Codigo_Programatico].ToString().Trim();
        Txt_Justificacion.Text = Dt_Orden_Compra_Detalle.Rows[0][Ope_Com_Requisiciones.Campo_Justificacion_Compra].ToString().Trim();
        Txt_Motivo_Cancelacion.Text = Dt_Orden_Compra_Detalle.Rows[0][Ope_Com_Ordenes_Compra.Campo_Motivo_Cancelacion].ToString().Trim();
        Txt_Fecha_Cancelacion.Text = Dt_Orden_Compra_Detalle.Rows[0][Ope_Com_Ordenes_Compra.Campo_Fecha_Cancelacion].ToString().Trim();
        Txt_Subtotal.Text = String.Format("{0:C}",double.Parse(Dt_Orden_Compra_Detalle.Rows[0][Ope_Com_Ordenes_Compra.Campo_Subtotal].ToString().Trim()));
        Txt_Total_IVA.Text = String.Format("{0:C}",double.Parse(Dt_Orden_Compra_Detalle.Rows[0][Ope_Com_Ordenes_Compra.Campo_Total_IVA].ToString().Trim()));
        Txt_Total.Text = String.Format("{0:C}",double.Parse(Dt_Orden_Compra_Detalle.Rows[0][Ope_Com_Ordenes_Compra.Campo_Total].ToString().Trim()));
        Session["NO_REQUISICION"] = Dt_Orden_Compra_Detalle.Rows[0][Ope_Com_Requisiciones.Campo_Requisicion_ID].ToString().Trim();
        Session["LISTADO_ALMACEN"] = Dt_Orden_Compra_Detalle.Rows[0][Ope_Com_Requisiciones.Campo_Listado_Almacen].ToString().Trim();
        

        if ((Dt_Orden_Compra_Detalle.Rows[0][Ope_Com_Ordenes_Compra.Campo_Estatus].ToString().Trim() == "CANCELACION PARCIAL") || (Dt_Orden_Compra_Detalle.Rows[0][Ope_Com_Ordenes_Compra.Campo_Estatus].ToString().Trim() == "CANCELACION TOTAL"))
        {
            Cmb_Estatus.SelectedValue = Dt_Orden_Compra_Detalle.Rows[0][Ope_Com_Ordenes_Compra.Campo_Estatus].ToString().Trim();
        }
        else
        {
            Cmb_Estatus.SelectedIndex = 0;
        }
        Clase_Negocio.P_Estatus = Dt_Orden_Compra_Detalle.Rows[0][Ope_Com_Ordenes_Compra.Campo_Estatus].ToString().Trim();
        //Consultamos los Productos de la Requisicion
        DataTable Dt_Prod_Serv = Clase_Negocio.Consultar_Productos_Servicios();

        if (Dt_Prod_Serv.Rows.Count != 0)
        {
            Grid_Detalles_Compra.DataSource = Dt_Prod_Serv;
            Grid_Detalles_Compra.DataBind();

        }
        else
        {
            Grid_Detalles_Compra.EmptyDataText = "No se encontrado registros.";
            //Lbl_Mensaje_Error.Text = "+ No se encontraron datos <br />";
            Grid_Detalles_Compra.DataSource = new DataTable();
            Grid_Detalles_Compra.DataBind();
        }


        Configurar_Formulario("General");
        Habilitar_Componentes(false);
    }


    protected void Grid_Ordenes_Compra_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable Dt_Ordenes_Compra = (DataTable)Session["Dt_Ordenes_Compra"];

        if (Dt_Ordenes_Compra != null)
        {
            DataView Dv_Ordenes_Compras = new DataView(Dt_Ordenes_Compra);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Ordenes_Compras.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Ordenes_Compras.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Ordenes_Compra.DataSource = Dv_Ordenes_Compras;
            Grid_Ordenes_Compra.DataBind();
            Dt_Ordenes_Compra = Dv_Ordenes_Compras.Table.Copy();
            Session["Dt_Ordenes_Compra"] = Dt_Ordenes_Compra;

        }

    }
    #endregion

    ///*******************************************************************************
    ///EVENTOS
    ///*******************************************************************************
    #region Eventos


    protected void Cmb_Estatus_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (Cmb_Estatus.SelectedValue != "RECHAZADA")
        {
            Txt_Fecha_Cancelacion.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        }
        else
        {
            Txt_Fecha_Cancelacion.Text = "";
        }

    }

    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        switch (Btn_Modificar.ToolTip)
        {
            case "Modificar":

                if ((Cmb_Estatus.SelectedValue != "CANCELACION PARCIAL") && (Cmb_Estatus.SelectedValue != "CANCELACION TOTAL"))
                {
                    Configurar_Formulario("Modificar");
                    Habilitar_Componentes(true);
                }
                else
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Esta Orden de Compra no puede ser Cancelada";

                }

                break;
            case "Guardar":



                if (((Cmb_Estatus.SelectedValue == "CANCELACION PARCIAL") || (Cmb_Estatus.SelectedValue == "CANCELACION TOTAL"))&&(Txt_Motivo_Cancelacion.Text.Trim() == String.Empty))
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario indicar el Motivo de la cancelación.";
                }

                if (Div_Contenedor_Msj_Error.Visible == false)
                {
                    Cls_Ope_Com_Cancelar_Ordenes_Compra_Negocio Clase_Negocio = new Cls_Ope_Com_Cancelar_Ordenes_Compra_Negocio();
                    String Mensaje= "";
                    Clase_Negocio.P_No_Orden_Compra = Session["NO_ORDEN_COMPRA"].ToString().Trim();
                    Clase_Negocio.P_No_Requisicio = Session["NO_REQUISICION"].ToString().Trim();
                    Clase_Negocio.P_Estatus = Cmb_Estatus.SelectedValue;
                    Clase_Negocio.P_Motivo_Cancelacion = Txt_Motivo_Cancelacion.Text;
                    if(Session["LISTADO_ALMACEN"] != null)
                    {
                        Clase_Negocio.P_Listado_Almacen = Session["LISTADO_ALMACEN"].ToString().Trim();
                    }

                    if (Cmb_Estatus.SelectedValue == "CANCELACION TOTAL")
                    {
                        Txt_Total.Text = Txt_Total.Text.Replace("$", "");
                        Txt_Total.Text = Txt_Total.Text.Replace(",", "");
                        Clase_Negocio.P_Monto_Total = String.Format("{0:0.00}", double.Parse(Txt_Total.Text.Trim()));
                        Mensaje = Clase_Negocio.Modificar_Orden_Compra();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Cancelar Orden de Compra", "alert('" + Mensaje + "');", true);
                    }//Fin del IF

                    if (Cmb_Estatus.SelectedValue == "CANCELACION PARCIAL")
                    {
                        Mensaje = Clase_Negocio.Modificar_Orden_Compra();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Cancelar Orden de Compra", "alert('" + Mensaje + "');", true);
                    }
                    Configurar_Formulario("Inicio");
                    Habilitar_Componentes(false);
                    Limpiar_Componentes();

                    //Limpiamos las variables de session 
                    Session["NO_ORDEN_COMPRA"] = null;
                    Session["NO_REQUISICION"] = null;
                    Session["LISTADO_ALMACEN"] = null;
                    Clase_Negocio = new Cls_Ope_Com_Cancelar_Ordenes_Compra_Negocio();
                    Llenar_Grid_Ordenes_Compra(Clase_Negocio);
                }
                
                break;
        }

    }
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        switch (Btn_Salir.ToolTip)
        {
            case "Inicio":
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                Limpiar_Componentes();
                break;
            case "Cancelar":
                Configurar_Formulario("Inicio");
                Limpiar_Componentes();
                break;
            case "Listado":
                Configurar_Formulario("Inicio");
                Limpiar_Componentes();
                break;
        }

    }

    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {

        Busqueda();

    }

    protected void Txt_Orden_Compra_Busqueda_TextChanged(object sender, EventArgs e)
    {
        Busqueda();
    }

    protected void Txt_Req_Busqueda_TextChanged(object sender, EventArgs e)
    {
        Busqueda();
    }
  
    #endregion


  
}
