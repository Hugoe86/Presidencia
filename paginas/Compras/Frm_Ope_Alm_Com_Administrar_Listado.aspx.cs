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
using Presidencia.Administrar_Listado.Negocio;
using Presidencia.Listado_Almacen.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Collections.Generic;


public partial class paginas_Compras_Frm_Ope_Alm_Com_Administrar_Listado : System.Web.UI.Page
{
    #region Variables 
    Cls_Ope_Alm_Com_Administrar_Listado_Negocio Listado_Negocio;
    Cls_Ope_Com_Listado_Negocio Listado_Busqueda;
    #endregion 

    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
        if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
        if (!IsPostBack)
        {
            ViewState["SortDirection"] = "ASC";
        }
        
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Init
    ///DESCRIPCIÓN: Metodo de la pagina 
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 8/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN: s
    ///*******************************************************************************

    protected void Page_Init(object sender, EventArgs e)
    {
        Listado_Negocio = new Cls_Ope_Alm_Com_Administrar_Listado_Negocio();
        Llenar_Grid_Listado();
        Estatus_Formulario("Inicial");
        Llenar_Combo_Estatus();
        Listado_Busqueda = new Cls_Ope_Com_Listado_Negocio();

    }

    #endregion

    #region Metodos

    public void Estatus_Formulario(String Estatus)
    {
        switch (Estatus)
        {
            case "Inicial":
                Div_Contenedor_Msj_Error.Visible = false;
                Lbl_Mensaje_Error.Text = "";
                Div_Datos_Generales.Visible = false;
                Div_Grid_Listado.Visible = true;
                Div_Busqueda.Visible = true;
                Btn_Salir.ToolTip = "Inicio";
                //limpiamos los grid
                
                Configuracion_Acceso("Frm_Ope_Alm_Com_Administrar_Listado.aspx");
                Configuracion_Acceso_LinkButton("Frm_Ope_Alm_Com_Administrar_Listado.aspx");
                break;
            case "General":
                Div_Contenedor_Msj_Error.Visible = false;
                Lbl_Mensaje_Error.Text = "";
                Div_Datos_Generales.Visible = true;
                Div_Grid_Listado.Visible = false;
                Div_Busqueda.Visible = false;
                Txt_Busqueda.Text = "";
                Txt_Folio.Enabled = false;
                Txt_Fecha.Enabled = false;
                Txt_Tipo.Enabled = false;
                Txt_Total.Enabled = false;
                Cmb_Estatus.Enabled = false;
                Txt_Comentario.Enabled = false;
                Btn_Alta_Observacion.ToolTip = "Nuevo";
                Btn_Alta_Observacion.ImageUrl = "~/paginas/imagenes/paginas/sias_add.png";
                Btn_Alta_Observacion.Enabled = true;
                Btn_Cancelar_Observacion.Visible = false;
                Btn_Salir.ToolTip = "Listado";

                Configuracion_Acceso("Frm_Ope_Alm_Com_Administrar_Listado.aspx");
                Configuracion_Acceso_LinkButton("Frm_Ope_Alm_Com_Administrar_Listado.aspx");
                break;
            case "Modificar":
                Div_Contenedor_Msj_Error.Visible = false;
                Lbl_Mensaje_Error.Text = "";
                Div_Datos_Generales.Visible = true;
                Div_Grid_Listado.Visible = false;
                Div_Busqueda.Visible = false;
                Txt_Busqueda.Text = "";
                Txt_Folio.Enabled = false;
                Txt_Fecha.Enabled = false;
                Txt_Tipo.Enabled = false;
                Txt_Total.Enabled = false;
                Cmb_Estatus.Enabled = true;
                Txt_Comentario.Enabled = true;
                Btn_Alta_Observacion.ToolTip = "Guardar";
                Btn_Alta_Observacion.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Alta_Observacion.Enabled = true;
                Btn_Cancelar_Observacion.Visible = true;
                Btn_Salir.ToolTip = "Listado";
                break;

        }
    }

    public void Limpiar_Formulario()
    {
        //limpiamos los datos de las cajas de texto
        Txt_Busqueda.Text = "";
        Txt_Folio.Text = "";
        Txt_Fecha.Text = "";
        Txt_Tipo.Text = "";
        Txt_Total.Text = "";
        Cmb_Estatus.SelectedIndex = 0;
        Txt_Comentario.Text = "";



    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Estatus
    ///DESCRIPCIÓN: Metodo que carga el combo Cmb_Estatus
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 8/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Estatus()
    {
        Cmb_Estatus.Items.Clear();
        Cmb_Estatus.Items.Add("<<SELECCIONAR>>");
        Cmb_Estatus.Items.Add("AUTORIZADA");
        Cmb_Estatus.Items.Add("CANCELADA");
        Cmb_Estatus.Items.Add("RECHAZADA");
        Cmb_Estatus.Items[0].Value = "0";
        Cmb_Estatus.Items[0].Selected = true;

    }

    public void Validar_Comentario()
    {
        if (Txt_Comentario.Text.Trim() == String.Empty && Cmb_Estatus.SelectedItem.Text.Trim()=="RECHAZADA")
        {
            Lbl_Mensaje_Error.Text += "+ Es obligatorio un comentario <br />";
            Div_Contenedor_Msj_Error.Visible = true;
        }
        else
        {
            Listado_Negocio.P_Comentario = Txt_Comentario.Text;
        }
    }

    public void Validar_Estatus()
    {
        
        if (Cmb_Estatus.SelectedIndex == 0)
        {
            Lbl_Mensaje_Error.Text += "+ Es necesario seleccionar un estatus <br/>";
            Div_Contenedor_Msj_Error.Visible = true;
        }
        if (Cmb_Estatus.SelectedValue == Session["Estatus_Inicial"].ToString())
        {
            Lbl_Mensaje_Error.Text += "+ Es necesario seleccionar un estatus diferente a  " + Cmb_Estatus.SelectedValue.ToString()+ "<br/>";
            Div_Contenedor_Msj_Error.Visible = true;
        }

    }
        


    #region Modal Busqueda Avanzada

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Estatus_Busqueda
    ///DESCRIPCIÓN: Metodo que llena el combo Cmb_Estatus que esta dentro del ModalPopup
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Estatus_Busqueda()
    {
        if (Cmb_Estatus_Busqueda.Items.Count == 0)
        {
            Cmb_Estatus_Busqueda.Items.Add("<<SELECCIONAR>>");
            Cmb_Estatus_Busqueda.Items.Add("AUTORIZADA");
            Cmb_Estatus_Busqueda.Items.Add("GENERADA");
            Cmb_Estatus_Busqueda.Items.Add("CANCELADA");
            Cmb_Estatus_Busqueda.Items[0].Value = "0";
            Cmb_Estatus_Busqueda.Items[0].Selected = true;
        }
    }
   

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Carga_Componentes_Busqueda
    ///DESCRIPCIÓN: Metodo que carga e inicializa los componentes del ModalPopUp
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Carga_Componentes_Busqueda()
    {
        
        Llenar_Combo_Estatus_Busqueda();
        Img_Error_Busqueda.Visible = false;
        Lbl_Error_Busqueda.Text = "";
        Chk_Estatus.Checked = false;
        Cmb_Estatus_Busqueda.Enabled = false;
        Cmb_Estatus_Busqueda.SelectedIndex = 0;
        Chk_Fecha.Checked = false;
        Txt_Fecha_Inicial.Enabled = false;
        Txt_Fecha_Final.Enabled = false;
        //limpiamos la clase de Negocio
        Listado_Negocio.P_Estatus_Busqueda = null;
        Listado_Negocio.P_Fecha_Inicial = null;
        Listado_Negocio.P_Fecha_Final = null;
        Listado_Negocio.P_Giro_ID = null;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Estatus_Busqueda
    ///DESCRIPCIÓN: Metodo que valida que seleccione un estatus dentro del modalpopup
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 27/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public void Validar_Estatus_Busqueda()
    {

        if (Chk_Estatus.Checked == true)
        {
            if (Cmb_Estatus_Busqueda.SelectedIndex != 0)
            {
                Listado_Negocio.P_Estatus_Busqueda = Cmb_Estatus_Busqueda.SelectedValue;

            }
            else
            {
                Img_Error_Busqueda.Visible = true;
                Lbl_Error_Busqueda.Text += "+ Debe seleccionar un estatus <br />";
            }

        }
        else
        {
            Listado_Negocio.P_Estatus_Busqueda = null;
        }

    }

   

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN: Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///en la busqueda del Modalpopup
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Verificar_Fecha()
    {
        //Variables que serviran para hacer la convecion a datetime las fechas y poder validarlas 
        DateTime Date1 = new DateTime();
        DateTime Date2 = new DateTime();
        if (Chk_Fecha.Checked == true)
        {
            if ((Txt_Fecha_Inicial.Text.Length == 11) && (Txt_Fecha_Final.Text.Length == 11))
            {
                //Convertimos el Texto de los TextBox fecha a dateTime
                Date1 = DateTime.Parse(Txt_Fecha_Inicial.Text);
                Date2 = DateTime.Parse(Txt_Fecha_Final.Text);
                //Validamos que las fechas sean iguales o la final sea mayor que la inicias, si no se manda un mensaje de error 
                if ((Date1 < Date2) | (Date1 == Date2))
                {
                    //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                    Listado_Negocio.P_Fecha_Inicial = Formato_Fecha(Txt_Fecha_Inicial.Text);
                    Listado_Negocio.P_Fecha_Final = Formato_Fecha(Txt_Fecha_Final.Text);

                }
                else
                {
                    Img_Error_Busqueda.Visible = true;
                    Lbl_Error_Busqueda.Text += "+ Fecha no valida <br />";
                }
            }
            else
            {
                Img_Error_Busqueda.Visible = true;
                Lbl_Error_Busqueda.Text += "+ Fecha no valida <br />";
            }
        }
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



    #endregion Fin Modal Busqueda Avanzada

    #endregion Fin Metodos
    
    #region Grid

    #region Grid Listado

    protected void Grid_Listado_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Cargamos el combo de Estatus
        Llenar_Combo_Estatus();
        //OBTENEMOS LA FILA SELECCIONADA
        GridViewRow selectedRow = Grid_Listado.Rows[Grid_Listado.SelectedIndex];
        Listado_Negocio.P_Folio = Convert.ToString(selectedRow.Cells[1].Text);
        DataTable Dt_Lista = Listado_Negocio.Consulta_Listado_Almacen();
        //Cargamos los datos en las cajas de texto
        Txt_Folio.Text = Dt_Lista.Rows[0][Ope_Com_Listado.Campo_Folio].ToString();
        Txt_Fecha.Text = Dt_Lista.Rows[0][Ope_Com_Listado.Campo_Fecha_Creo].ToString();
        Txt_Tipo.Text = Dt_Lista.Rows[0][Ope_Com_Listado.Campo_Tipo].ToString();
        Txt_Total.Text = Dt_Lista.Rows[0][Ope_Com_Listado.Campo_Total].ToString();
        Div_Datos_Generales.Visible = true;
        Div_Grid_Listado.Visible = false;
        Estatus_Formulario("General");
        if (Dt_Lista.Rows[0][Ope_Com_Listado.Campo_Estatus].ToString() == "GENERADA")
            Cmb_Estatus.SelectedIndex = 0;
        else
            Cmb_Estatus.SelectedValue = Dt_Lista.Rows[0][Ope_Com_Listado.Campo_Estatus].ToString();
        Session["Estatus_Inicial"] = Dt_Lista.Rows[0][Ope_Com_Listado.Campo_Estatus].ToString();
        //CONSULTAMOS LOS PRODUCTOS PERTENECIENTES A ESTE LISTADO
        Listado_Negocio.P_Listado_ID = Dt_Lista.Rows[0][Ope_Com_Listado.Campo_Listado_ID].ToString();
        Session["Listado_ID"] = Listado_Negocio.P_Listado_ID;
        Llenar_Grid_Productos();
        //CONSULTAMOS LOS COMENTARIOS CORRESPONDIENTES A ESTE LISTADO
        Llenar_Grid_Comentarios();
        
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Listado_PageIndexChanging
    ///DESCRIPCIÓN: Metodo que permite la paginacion del Grid_Listado
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 28/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Listado_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Listado.PageIndex = e.NewPageIndex;
        Grid_Listado.DataSource = (DataTable)Session["Dt_Listado"];
        Grid_Listado.DataBind();
    }

    public void Llenar_Grid_Listado()
    {
        DataTable Data_Table = Listado_Negocio.Consulta_Listado_Almacen();
        Session["Dt_Listado"] = Data_Table;
        if (Data_Table.Rows.Count != 0)
        {
            Grid_Listado.DataSource = Data_Table;
            Grid_Listado.DataBind();
        }
        else
        {
            Grid_Listado.DataSource = null;
            Grid_Listado.DataBind();
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "+ No se encontraron datos <br />";
            
        }
    }

    protected void Grid_Listado_Sorting(object sender, GridViewSortEventArgs e)
    {

        DataTable Dt_Listado = (DataTable)Session["Dt_Listado"];

        if (Dt_Listado != null)
        {
            DataView Dv_Listado = new DataView(Dt_Listado);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Listado.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Listado.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Listado.DataSource = Dv_Listado;
            Grid_Listado.DataBind();
        }

    }

    #endregion Fin Grid Listado

    #region Grid Productos

    public void Llenar_Grid_Productos()
    {
        DataTable Data_Table = Listado_Negocio.Consulta_Listado_Detalle();
        if (Data_Table.Rows.Count != 0)
        {
            Session["Dt_Productos"] = Data_Table;
            Grid_Productos.DataSource = Data_Table;
            Grid_Productos.DataBind();
            Grid_Productos.Enabled = true;
        }
        
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Productos_PageIndexChanging
    ///DESCRIPCIÓN: Metodo que permite la paginacion del Grid_Listado
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 28/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Productos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Productos.PageIndex = e.NewPageIndex;
        Grid_Productos.DataBind();
        Grid_Productos.DataSource = (DataTable)Session["Dt_Productos"];
        Grid_Productos.DataBind();

    }

    protected void Grid_Productos_Sorting(object sender, GridViewSortEventArgs e)
    {

        DataTable Dt_Productos = (DataTable)Session["Dt_Productos"];

        if (Dt_Productos != null)
        {
            DataView Dv_Productos = new DataView(Dt_Productos);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Productos.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Productos.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Productos.DataSource = Dv_Productos;
            Grid_Productos.DataBind();
        }

    }
    #endregion Fin Grid Productos

    #region Grid Comentarios
    public void Llenar_Grid_Comentarios()
    {
        DataTable Data_Table = Listado_Negocio.Consultar_Observaciones_Listado();
        Session["Dt_Comentarios"] = Data_Table;
        if (Data_Table.Rows.Count != 0)
        {
            Grid_Comentarios.DataSource = Data_Table;
            Grid_Comentarios.DataBind();
            Grid_Comentarios.Visible = true;
        }
       
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Listado_PageIndexChanging
    ///DESCRIPCIÓN: Metodo que permite la paginacion del Grid_Listado
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 28/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Comentarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Comentarios.PageIndex = e.NewPageIndex;
        Grid_Comentarios.DataBind();
        Listado_Negocio.P_Listado_ID = Session["Listado_ID"].ToString();
        Llenar_Grid_Comentarios();
        
    }


    protected void Grid_Comentarios_SelectedIndexChanged(object sender, EventArgs e)
    {
        int Renglon = Grid_Comentarios.SelectedIndex;
        DataTable Dt_Aux = (DataTable)Session["Dt_Comentarios"];
        Txt_Comentario.Text = Dt_Aux.Rows[Renglon][0].ToString();
        Listado_Negocio.P_Listado_ID = Session["Listado_ID"].ToString();
        Llenar_Grid_Comentarios();
        Llenar_Grid_Productos();
    }


    protected void Grid_Comentarios_Sorting(object sender, GridViewSortEventArgs e)
    {
        Grid_Productos.Visible = true;
        Grid_Productos.DataSource = (DataTable)Session["Dt_Productos"];
        Grid_Productos.DataBind();
        DataTable Dt_Comentarios = (DataTable)Session["Dt_Comentarios"];

        if (Dt_Comentarios != null)
        {
            DataView Dv_Comentarios = new DataView(Dt_Comentarios);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Comentarios.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Comentarios.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Comentarios.DataSource = Dv_Comentarios;
            Grid_Comentarios.DataBind();
        }
    }

    #endregion Grid Comentarios


    
    #endregion Fin Grid

    #region Eventos


    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        switch (Btn_Salir.ToolTip)
        {
            case "Listado":
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = false;
                //Limpiamos el objeto de clase de negocios
                Listado_Negocio = new Cls_Ope_Alm_Com_Administrar_Listado_Negocio();
                Estatus_Formulario("Inicial");
                break;
            case "Inicio":
                //Limpiamos el objeto de clase de negocios
                Listado_Negocio = new Cls_Ope_Alm_Com_Administrar_Listado_Negocio();
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                break;
        }//fin del switch
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Cerrar_Click
    ///DESCRIPCIÓN: Evento del Boton de Cerrar, el cual oculta el div de busueda de productos y muestra el 
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Cerrar_Click(object sender, EventArgs e)
    {
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
        

    }
  


    protected void Btn_Alta_Observacion_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        switch(Btn_Alta_Observacion.ToolTip)
        {
            case "Nuevo":
                Estatus_Formulario("Modificar");
                //Habilitamos los componentes necesarios 
                Txt_Comentario.Enabled = true;
                Txt_Comentario.Text = "";
                Cmb_Estatus.Enabled = true; 
                //llenamos nuevamente los grid
                Listado_Negocio.P_Listado_ID = Session["Listado_ID"].ToString();
                Llenar_Grid_Comentarios();
                Llenar_Grid_Productos();
                Grid_Productos.Enabled = true;
                break;
            case "Guardar":
                Validar_Estatus();
                Validar_Comentario();
                try
                {
                    if (Div_Contenedor_Msj_Error.Visible == false)
                    {
                        Listado_Negocio.P_Folio = Txt_Folio.Text;
                        Listado_Negocio.P_Estatus = Cmb_Estatus.SelectedValue;
                        Listado_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                        Listado_Negocio.P_Usuario_ID = Cls_Sessiones.Empleado_ID;
                        Listado_Negocio.P_Listado_ID = Session["Listado_ID"].ToString();
                        Listado_Negocio.P_Total = Txt_Total.Text;
                        Listado_Negocio.P_Dt_Productos = (DataTable)Session["Dt_Productos"];
                        String Requisicion_Trancitoria = Listado_Negocio.Modificar_Listado();
                        Estatus_Formulario("Inicial");
                        
                        if (Requisicion_Trancitoria.Trim() != String.Empty)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Listado Almacen", "alert('La Actualizacion del listado fue exitosa y se creo la requisicion transitoria RQ-"+Requisicion_Trancitoria +"');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Listado Almacen", "alert('La Actualizacion del listado fue exitosa');", true);
                        }
                        //Limpiamos la clase de Negocio
                        Listado_Negocio = new Cls_Ope_Alm_Com_Administrar_Listado_Negocio();
                        Llenar_Grid_Listado();
                        Limpiar_Formulario();
                        Grid_Productos.DataBind();
                        Grid_Comentarios.DataBind();
                    }
                }
                catch(Exception Ex)
                {
                    throw new Exception("Error al modificar la Requisicion. Error: [" + Ex.Message + "]");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Listado Almacen", "alert('Falla al modificar el listado');", true);
                }
                
                break;

        }        

    }

    protected void Btn_Cancelar_Observacion_Click(object sender, ImageClickEventArgs e)
    {
        Estatus_Formulario("Inicial");
        Limpiar_Formulario();
        Grid_Productos.DataBind();
        Grid_Comentarios.DataBind();
        //limpiamos la clase de negocio
        Listado_Negocio = new Cls_Ope_Alm_Com_Administrar_Listado_Negocio();
    }

    #region Check de Busqueda Avanzada

    protected void Chk_Fecha_CheckedChanged(object sender, EventArgs e)
    {
        Modal_Busqueda.Show();
    }

    protected void Chk_Estatus_CheckedChanged(object sender, EventArgs e)
    {
        Modal_Busqueda.Show();
        if (Chk_Estatus.Checked == true)
        {
            Txt_Fecha_Inicial.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Txt_Fecha_Final.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Cmb_Estatus_Busqueda.Enabled = true;
            Cmb_Estatus_Busqueda.SelectedIndex = 0;
        }
        else
        {
            Txt_Fecha_Inicial.Text = "";
            Txt_Fecha_Final.Text = "";
            Cmb_Estatus_Busqueda.Enabled = false;
            Cmb_Estatus_Busqueda.SelectedIndex = 0;
        }
    }
    
   

    #endregion

    #region Eventos Botones Busqueda
    protected void Btn_Aceptar_Click(object sender, EventArgs e)
    {
        Img_Error_Busqueda.Visible = false;
        Lbl_Error_Busqueda.Text = "";
        Validar_Estatus_Busqueda();
        Verificar_Fecha();
        if ((Chk_Fecha.Checked == false) && (Chk_Estatus.Checked == false))
        {
            Img_Error_Busqueda.Visible = true;
            Lbl_Error_Busqueda.Text += "+ Debe seleccionar una opcion <br />";
        }
        if (Img_Error_Busqueda.Visible == false)
        {
            Modal_Busqueda.Hide();
            DataTable Dt_Listado = Listado_Negocio.Consulta_Listado_Almacen();
            Session["Dt_Listado"] = Dt_Listado;
            // Se llena el grid de listado
            Grid_Listado.DataSource = Dt_Listado;
            Grid_Listado.DataBind();           
        }
        else
        {
            Modal_Busqueda.Show();
        }
    }

    protected void Btn_Avanzada_Click(object sender, EventArgs e)
    {
        //Hacemos visible el modal
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Carga_Componentes_Busqueda();
        Modal_Busqueda.Show();
    }
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        if(Txt_Busqueda.Text.Trim() != "")
            Listado_Negocio.P_Folio = Txt_Busqueda.Text.Trim();

        DataTable Dt_Listado = Listado_Negocio.Consulta_Listado_Almacen();

        if (Dt_Listado.Rows.Count > 0)
        {
            Session["Dt_Listado"] = Dt_Listado;
            // Se llena el grid de listado
            Grid_Listado.DataSource = Dt_Listado;
            Grid_Listado.DataBind();
            Div_Contenedor_Msj_Error.Visible = false;
            Grid_Listado.Visible = true;
        }
        else
        {
            Lbl_Mensaje_Error.Text = "No se Encontraron Listados";
            Lbl_Mensaje_Error.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
            Grid_Listado.Visible = true;
        }
    }

    #endregion Fin Eventos Busqueda
        
    #endregion  Fin de Eventos

    protected void Btn_Cerrar_Click(object sender, ImageClickEventArgs e)
    {

    }

    #region (Control Acceso Pagina)
    /// *****************************************************************************************************************************
    /// NOMBRE: Configuracion_Acceso
    /// 
    /// DESCRIPCIÓN: Habilita las operaciones que podrá realizar el usuario en la página.
    /// 
    /// PARÁMETROS: No Áplica.
    /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 23/Mayo/2011 10:43 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *****************************************************************************************************************************
    protected void Configuracion_Acceso(String URL_Pagina)
    {
        List<ImageButton> Botones = new List<ImageButton>();//Variable que almacenara una lista de los botones de la página.
        DataRow[] Dr_Menus = null;//Variable que guardara los menus consultados.

        try
        {
            //Agregamos los botones a la lista de botones de la página.
            Botones.Add(Btn_Buscar);

            if (!String.IsNullOrEmpty(Request.QueryString["PAGINA"]))
            {
                if (Es_Numero(Request.QueryString["PAGINA"].Trim()))
                {
                    //Consultamos el menu de la página.
                    Dr_Menus = Cls_Sessiones.Menu_Control_Acceso.Select("MENU_ID=" + Request.QueryString["PAGINA"]);

                    if (Dr_Menus.Length > 0)
                    {
                        //Validamos que el menu consultado corresponda a la página a validar.
                        if (Dr_Menus[0][Apl_Cat_Menus.Campo_URL_Link].ToString().Contains(URL_Pagina))
                        {
                            Cls_Util.Configuracion_Acceso_Sistema_SIAS(Botones, Dr_Menus[0]);//Habilitamos la configuracón de los botones.
                        }
                        else
                        {
                            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                    }
                }
                else
                {
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }
            }
            else
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al habilitar la configuración de accesos a la página. Error: [" + Ex.Message + "]");
        }
    }
    /// *****************************************************************************************************************************
    /// NOMBRE: Configuracion_Acceso
    /// 
    /// DESCRIPCIÓN: Habilita las operaciones que podrá realizar el usuario en la página.
    /// 
    /// PARÁMETROS: No Áplica.
    /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 23/Mayo/2011 10:43 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *****************************************************************************************************************************
    protected void Configuracion_Acceso_LinkButton(String URL_Pagina)
    {
        List<LinkButton> Botones = new List<LinkButton>();//Variable que almacenara una lista de los botones de la página.
        DataRow[] Dr_Menus = null;//Variable que guardara los menus consultados.

        try
        {
            //Agregamos los botones a la lista de botones de la página.
            Botones.Add(Btn_Busqueda_Avanzada);

            if (!String.IsNullOrEmpty(Request.QueryString["PAGINA"]))
            {
                if (Es_Numero(Request.QueryString["PAGINA"].Trim()))
                {
                    //Consultamos el menu de la página.
                    Dr_Menus = Cls_Sessiones.Menu_Control_Acceso.Select("MENU_ID=" + Request.QueryString["PAGINA"]);

                    if (Dr_Menus.Length > 0)
                    {
                        //Validamos que el menu consultado corresponda a la página a validar.
                        if (Dr_Menus[0][Apl_Cat_Menus.Campo_URL_Link].ToString().Contains(URL_Pagina))
                        {
                            Cls_Util.Configuracion_Acceso_Sistema_SIAS(Botones, Dr_Menus[0]);//Habilitamos la configuracón de los botones.
                        }
                        else
                        {
                            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                    }
                }
                else
                {
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }
            }
            else
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al habilitar la configuración de accesos a la página. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: IsNumeric
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 29/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Es_Numero(String Cadena)
    {
        Boolean Resultado = true;
        Char[] Array = Cadena.ToCharArray();
        try
        {
            for (int index = 0; index < Array.Length; index++)
            {
                if (!Char.IsDigit(Array[index])) return false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al Validar si es un dato numerico. Error [" + Ex.Message + "]");
        }
        return Resultado;
    }
    #endregion
}
