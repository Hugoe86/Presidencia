using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Almacen_Requisiciones_Pendientes.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using AjaxControlToolkit;
using System.Data;
using Presidencia.Dependencias.Negocios;
using Presidencia.Areas.Negocios;

public partial class paginas_Compras_Frm_Ope_Com_Requisiciones_Pendientes : System.Web.UI.Page
{
    #region (Variables)
    Cls_Ope_Com_Requisiciones_Pendientes_Negocio Requisiciones_Pendientes_Negocio = new Cls_Ope_Com_Requisiciones_Pendientes_Negocio(); //variable para la capa de negocios
    #endregion

    #region (Page_Load)
    protected void Page_Load(object sender, EventArgs e)
    {
        //Declaracion de variables
        String Tipo_Requisicion = String.Empty; //Variable que indica el tipo de requisicion

        String Pagina_OS = Request.QueryString["?PAGINA"];
        Session["Pagina_OS"] = Pagina_OS;

        try
        {
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                //Colocar el tipo de requisicion en la variable de sesion
                Tipo_Requisicion = HttpUtility.HtmlDecode(Request.QueryString["Tipo_Requisicion"].ToString());

                //Colocar la pagina en el estatus inicial dependiendo del tipo de requisicion
                Estado_Inicial(Tipo_Requisicion);

                //Colocar el tipo de la requisicion en una variable de sesion
                Session["Tipo_Requisicion"] = Tipo_Requisicion;

                Txt_Busqueda.Text = "";
            }
            else
            {
                Lbl_Informacion.Visible = false;
                Img_Warning.Visible = false;
                Lbl_Informacion.Text = "";
            }
        }
        catch (Exception ex)
        {
            Lbl_Informacion.Text = "Error: (Page_Load)" + ex.ToString();
            Mostrar_Informacion(1);
        }
    }
    #endregion

    #region (Metodos)


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Mostrar_Informacion
    ///DESCRIPCION:             Habilita o deshabilita la muestra en pantalle del mensaje 
    ///                         de Mostrar_Informacion para el usuario
    ///PARAMETROS:              1.- Condicion, entero para saber si es 1 habilita para que se muestre mensaje si es cero
    ///                         deshabilita para que no se muestre el mensaje
    ///CREO:                    Silvia Morales Portuhondo
    ///FECHA_CREO:              23/Septiembre/2010 
    ///MODIFICO:                Noe Mosqueda Valadez
    ///FECHA_MODIFICO:          22/Octubre/2010 11:38
    ///CAUSA_MODIFICACION:      Agregar try-catch para el manejo de errores 
    ///MODIFICO:                Salvador Hernández Ramírez
    ///FECHA_MODIFICO:          08/Marzo/2011 
    ///CAUSA_MODIFICACION:      Se cambio la habilitación del "Lbl_Informacion.Enable", por "Div_Contenedor_Msj_Error.Visible"
    ///*******************************************************************************
    private void Mostrar_Informacion(int Condicion)
    {
        try
        {
            if (Condicion == 1)
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Img_Warning.Visible = true;
                Lbl_Informacion.Visible = true;
            }
            else
            {
                Lbl_Informacion.Text = "";
                Div_Contenedor_Msj_Error.Visible = false;
                Img_Warning.Visible = false;
            }
        }
        catch (Exception ex)
        {
            Lbl_Informacion.Enabled = true;
            Img_Warning.Visible = true;
            Lbl_Informacion.Text = "Error: " + ex.ToString();
        }
    }



    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Busqueda
    ///DESCRIPCIÓN:          Método utilizado para mostrar y ocultar los controles
    ///                      utilizados para realizar la búsqueda simble y abanzada
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           12/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Mostrar_Busqueda(Boolean Estatus)
    {
        Txt_Busqueda.Visible = Estatus;
        Btn_Buscar.Visible = Estatus;
        Btn_Busqueda_Avanzada.Visible = Estatus;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Estado_Inicial
    ///DESCRIPCION:             Colocar la pagina en el estado inicial (navegacion)
    ///PARAMETROS:              Tipo_Requisicion: Cadena de texto que indica el tipo de requisicion
    ///CREO:                    Noe Mosqueda Valadez
    ///FECHA_CREO:              22/Noviembre/2010 18:24
    ///MODIFICO:                Noe Mosqueda Valadez
    ///FECHA_MODIFICO:          24/Enero/2011 17:00
    ///CAUSA_MODIFICACION:      Se le agrego lo tipo de requisicion para separar stock y transitorias
    ///*******************************************************************************
    private void Estado_Inicial(String Tipo_Requisicion)
    {
        try
        {
            String Titulo = "";

            Div_Productos_Requisicion.Visible = false;
            Div_Requisiciones_Pendientes.Visible = false;

            if (Tipo_Requisicion == "Transitorio")
                Titulo = " Requisiciones Pendientes Transitorias ";
            else if (Tipo_Requisicion == "Stock")
                Titulo = " Requisiciones Pendientes Stock ";

            Lbl_Titulo.Text = Titulo.Trim();
            Session.Remove("Tipo_Requisicion");
            Session.Remove("Dt_Requisiciones_Pendientes");

            //Colocar el tipo de la requisicion en una variable de sesion
            Session["Tipo_Requisicion"] = Tipo_Requisicion;
            Llena_Grid_Requisiciones_Pendientes(Tipo_Requisicion, "", -1);
            Llena_Combo_Dependencias();

            if (Btn_Salir.Visible)
            Configuracion_Acceso("Frm_Ope_Com_Requisiciones_Pendientes.aspx");

            if (Btn_Salir.Visible)
            {
                Grid_Requisiciones_Pendientes.Enabled = true;
                Mostrar_Busqueda(true);
            }
            else
            {
                Grid_Requisiciones_Pendientes.Enabled = false;
                Mostrar_Busqueda(false);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Llena_Grid_Requisiciones_Pendientes
    ///DESCRIPCION:             Llenar el grid de las requisiciones de acuerdo a un 
    ///                         criterio de busqueda
    ///PARAMETROS:              1. Tipo_Requisicion: Cadena de texto que indica si es Stock o Transitorio
    ///                         2. Busqueda: Cadena de texto que tiene el elemento a buscar
    ///                         3. Pagina: Entero que indica la pagina del grid a visualizar
    ///CREO:                    Noe Mosqueda Valadez
    ///FECHA_CREO:              22/Noviembre/2010 17:43
    ///MODIFICO:                Noe Mosqueda Valadez
    ///FECHA_MODIFICO:          24/Enero/2011 17:00
    ///CAUSA_MODIFICACION:      Se le agrego el tipo de requisicion para separar las de stock y transitorias
    ///MODIFICO:                Salvador Hernández Ramírez
    ///FECHA_MODIFICO:          08/Marzo/2011 
    ///CAUSA_MODIFICACION:      Se le agregaron mensajes de excepciones que seran utilizados cuando no se encuentren requisiciones pendientes
    ///*******************************************************************************
    private void Llena_Grid_Requisiciones_Pendientes(String Tipo_Requisicion, String Busqueda, int Pagina)
    {
        //Declaracion de variables
        DataTable Dt_Requisiciones_Pendientes = new DataTable(); //Tabla para las requisiciones pendientes

        try
        {
            // Verificar si la busqueda no esta vacia
            if (Busqueda != String.Empty && Busqueda != "" && Busqueda != null)
            {
                // Verificar si es la busqueda de todos
                if (Busqueda == "Todos")
                    Busqueda = "";

                //Realizar consulta
                Requisiciones_Pendientes_Negocio.P_Busqueda = Busqueda;
                Requisiciones_Pendientes_Negocio.P_Empleado_Filtrado_ID = Cls_Sessiones.Empleado_ID;
                Requisiciones_Pendientes_Negocio.P_Tipo_Requisicion = Tipo_Requisicion;

                Dt_Requisiciones_Pendientes = Requisiciones_Pendientes_Negocio.Consulta_Requisiciones_Pendientes();

                if (Dt_Requisiciones_Pendientes.Rows.Count <= 0)
                {
                    Lbl_Error_Busqueda.Text = "No se encontraron requisiciones";
                    Img_Error_Busqueda.Visible = true;
                    Modal_Busqueda.Show();
                }
                else
                {
                    Modal_Busqueda.Hide();
                    Lbl_Error_Busqueda.Text = "";
                    Img_Error_Busqueda.Visible = false;
                }
            }
            else
            {
                //Verificar si existe la variable de sesion
                if (Session["Dt_Requisiciones_Pendientes"] != null)
                    Dt_Requisiciones_Pendientes = (DataTable)Session["Dt_Requisiciones_Pendientes"];
                else
                {
                    //Realizar consulta
                    Requisiciones_Pendientes_Negocio.P_Busqueda = "";

                    //  Se revisa que se tenga un numero de requisiciòn a consultar
                    if (Txt_Busqueda.Text.Trim() != "")
                    {
                        Requisiciones_Pendientes_Negocio.P_No_Requisicion = Convert.ToInt32(Txt_Busqueda.Text.Trim());
                    }
                    else
                    {
                        Requisiciones_Pendientes_Negocio.P_No_Requisicion = 0;
                    }
                    Requisiciones_Pendientes_Negocio.P_Empleado_Filtrado_ID = Cls_Sessiones.Empleado_ID;
                    Requisiciones_Pendientes_Negocio.P_Tipo_Requisicion = Tipo_Requisicion;
                    Dt_Requisiciones_Pendientes = Requisiciones_Pendientes_Negocio.Consulta_Requisiciones_Pendientes();
                }
            }

            if (Dt_Requisiciones_Pendientes.Rows.Count > 0)
            {
                Session["Dt_Requisiciones_Pendientes"] = Dt_Requisiciones_Pendientes;
                // Llenar el grid
                Grid_Requisiciones_Pendientes.DataSource = Dt_Requisiciones_Pendientes;

                // Verificar si hay pagina
                if (Pagina > -1)
                    Grid_Requisiciones_Pendientes.PageIndex = Pagina;

                // Mostrar columnas
                Grid_Requisiciones_Pendientes.Columns[7].Visible = true;
                Grid_Requisiciones_Pendientes.Columns[8].Visible = true;
                Grid_Requisiciones_Pendientes.Columns[9].Visible = true;
                Grid_Requisiciones_Pendientes.Columns[10].Visible = true;
                Grid_Requisiciones_Pendientes.Columns[11].Visible = true;
                Grid_Requisiciones_Pendientes.Columns[12].Visible = true;
                Grid_Requisiciones_Pendientes.DataBind();

                //Ocultar columnas
                Grid_Requisiciones_Pendientes.Columns[7].Visible = false;
                Grid_Requisiciones_Pendientes.Columns[8].Visible = false;
                Grid_Requisiciones_Pendientes.Columns[9].Visible = false;
                Grid_Requisiciones_Pendientes.Columns[10].Visible = false;
                Grid_Requisiciones_Pendientes.Columns[11].Visible = false;
                Grid_Requisiciones_Pendientes.Columns[12].Visible = false;


                //Colocar tabla en variable de sesion
                Session["Dt_Requisiciones_Pendientes"] = Dt_Requisiciones_Pendientes;
                Mostrar_Informacion(0);
                Div_Requisiciones_Pendientes.Visible = true;

            }
            else
            {
                Lbl_Informacion.Text = "No se encontraron requisiciones";
                Mostrar_Informacion(1);
                Div_Requisiciones_Pendientes.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #region (Busqueda Avanzada)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Estado_Inicial_Busqueda_Avanzada
    ///DESCRIPCION:             Colocar la ventama modal en un estado inicial
    ///PARAMETROS:              
    ///CREO:                    Noe Mosqueda Valadez
    ///FECHA_CREO:              05/Enero/2011 09:37
    ///MODIFICO:                
    ///FECHA_MODIFICO:          
    ///CAUSA_MODIFICACION:      
    ///*******************************************************************************
    private void Estado_Inicial_Busqueda_Avanzada()
    {
        try
        {
            //Limpiar check
            Chk_Area.Checked = false;
            Chk_Dependencia.Checked = false;
            Chk_Estatus.Checked = false;
            Chk_Fecha.Checked = false;

            //Limpiar cajas de texto
            Txt_Fecha_Final.Text = "";
            Txt_Fecha_Inicial.Text = "";

            //colocar combos en indice 0
            Cmb_Dependencia.SelectedIndex = 0;
            Cmb_Area.Items.Clear();
            Cmb_Estatus_Busqueda.SelectedIndex = 0;

            //Dehabilitar controles
            Img_Btn_Fecha_Final.Enabled = false;
            Img_Btn_Fecha_Inicial.Enabled = false;
            Cmb_Area.Enabled = false;
            Cmb_Dependencia.Enabled = false;
            Cmb_Estatus_Busqueda.Enabled = false;

            //Bloque del error
            Img_Error_Busqueda.Visible = false;
            Lbl_Error_Busqueda.Text = "";
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Llena_Combo_Dependencias
    ///DESCRIPCION:             Llenar el combo con las dependencias
    ///PARAMETROS:             
    ///CREO:                    Noe Mosqueda Valadez
    ///FECHA_CREO:              05/Enero/2011 11:45
    ///MODIFICO:                
    ///FECHA_MODIFICO:          
    ///CAUSA_MODIFICACION:      
    ///*******************************************************************************
    private void Llena_Combo_Dependencias()
    {
        //Declaracion de variables
        Cls_Cat_Dependencias_Negocio Dependencias_Negocio = new Cls_Cat_Dependencias_Negocio(); //Variable para la capa de negocios

        try
        {
            //Llenar el combo de las dependencias
            Cmb_Dependencia.DataSource = Dependencias_Negocio.Consulta_Dependencias();
            Cmb_Dependencia.DataTextField = Cat_Dependencias.Campo_Nombre;
            Cmb_Dependencia.DataValueField = Cat_Dependencias.Campo_Dependencia_ID;
            Cmb_Dependencia.DataBind();
            Cmb_Dependencia.Items.Insert(0, new ListItem("<SELECCIONE>", ""));
            Cmb_Dependencia.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Llena_Combo_Areas
    ///DESCRIPCION:             Llenar el combo con las areas
    ///PARAMETROS:              1. Dependencia_ID: Cadena de texto con el ID de la dependencia
    ///CREO:                    Noe Mosqueda Valadez
    ///FECHA_CREO:              07/Enero/2011 10:20
    ///MODIFICO:                
    ///FECHA_MODIFICO:          
    ///CAUSA_MODIFICACION:      
    ///*******************************************************************************
    private void Llena_Combo_Areas(String Dependencia_ID)
    {
        //Declaracion de variables
        Cls_Cat_Areas_Negocio Areas_Negocio = new Cls_Cat_Areas_Negocio(); //Variable para la capa de negocios

        try
        {
            //Llenar el combo de las areas
            Areas_Negocio.P_Dependencia_ID = Dependencia_ID;
            Cmb_Area.DataSource = Areas_Negocio.Consulta_Areas();
            Cmb_Area.DataTextField = Cat_Areas.Campo_Nombre;
            Cmb_Area.DataValueField = Cat_Areas.Campo_Area_ID;
            Cmb_Area.DataBind();
            Cmb_Area.Items.Insert(0, new ListItem("<SELECCIONE>", ""));
            Cmb_Area.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    #endregion

    #region (Filtrado)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Filtrar_Requisicion
    ///DESCRIPCION:             Colocar el estatus de la requisicion en filtrado.
    ///PARAMETROS:              1. No_Requisicion: Numero que tiene el numero de la requisicion
    ///                         2. Comentarios: Cadena de texto que contiene los comentarios para el filtrado
    ///CREO:                    Noe Mosqueda Valadez
    ///FECHA_CREO:              23/Diciembre/2010 18:43
    ///MODIFICO:                
    ///FECHA_MODIFICO:          
    ///CAUSA_MODIFICACION:      
    ///*******************************************************************************
    private void Filtrar_Requisicion(long No_Requisicion, String Comentarios)
    {
        ////Declaracion de variables
        //Cls_Ope_Com_Requisiciones_Pendientes_Negocio Requisiciones_Pendientes_Negocio = new Cls_Ope_Com_Requisiciones_Pendientes_Negocio(); //variable para la capa de negocios
        try
        {
            //Asignar propiedades
            Requisiciones_Pendientes_Negocio.P_Estatus_Requisicion = "FILTRADA";
            Requisiciones_Pendientes_Negocio.P_No_Requisicion = No_Requisicion;

            if (Comentarios.Length > 249)
            {
                Requisiciones_Pendientes_Negocio.P_Comentarios = Comentarios.Substring(0, 249);
            }
            else
            {
                Requisiciones_Pendientes_Negocio.P_Comentarios = Comentarios;
            }
            Requisiciones_Pendientes_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
            Requisiciones_Pendientes_Negocio.P_Empleado_Filtrado_ID = Cls_Sessiones.Empleado_ID;
            Requisiciones_Pendientes_Negocio.Cambiar_Estatus_Requisiciones();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    #endregion

    #region (Revisar)

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Revisar_Requisicion
    ///DESCRIPCION:             Colocar el estatus de la requisicion en REVISAR.
    ///PARAMETROS:              1. No_Requisicion: Numero que tiene el numero de la requisicion
    ///                         2. Comentarios: Cadena de texto que contiene los comentarios para el filtrado
    ///CREO:                    Salvador Hernández Ramírez
    ///FECHA_CREO:              14/Abril/2011 
    ///MODIFICO:                
    ///FECHA_MODIFICO:          
    ///CAUSA_MODIFICACION:      
    ///*******************************************************************************
    private void Revisar_Requisicion(long No_Requisicion, String Comentarios)
    {
        ////Declaracion de variables
        //Cls_Ope_Com_Requisiciones_Pendientes_Negocio Requisiciones_Pendientes_Negocio = new Cls_Ope_Com_Requisiciones_Pendientes_Negocio(); //variable para la capa de negocios
        try
        {
            //Asignar propiedades
            Requisiciones_Pendientes_Negocio.P_Estatus_Requisicion = "REVISAR";
            Requisiciones_Pendientes_Negocio.P_No_Requisicion = No_Requisicion;
            Requisiciones_Pendientes_Negocio.P_Comentarios = Comentarios;
            Requisiciones_Pendientes_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
            Requisiciones_Pendientes_Negocio.P_Empleado_Filtrado_ID = Cls_Sessiones.Empleado_ID;
            Requisiciones_Pendientes_Negocio.Cambiar_Estatus_Requisiciones();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    #endregion

    #endregion

    #region (Grid)

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Grid_Requisiciones_Pendientes_PageIndexChanging
    ///DESCRIPCION:             Se optiene el tipo de requisicion y se llena el Grid con las requisiciones
    ///PARAMETROS:              
    ///CREO:                    Noe Mosqueda Valadez
    ///FECHA_CREO:              22/Noviembre/2010
    ///MODIFICO:                
    ///FECHA_MODIFICO:          
    ///CAUSA_MODIFICACION:      
    ///*******************************************************************************
    protected void Grid_Requisiciones_Pendientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //Declaracion de Variables
        String Tipo_Requisicion = String.Empty; // Variable para el tipo de la requisicion

        try
        {
            // Obtener el tipo de la requisicion
            Tipo_Requisicion = Session["Tipo_Requisicion"].ToString().Trim();

            // Llenar el grid con la pagina correspondiente
            Llena_Grid_Requisiciones_Pendientes(Tipo_Requisicion, "", e.NewPageIndex);
        }
        catch (Exception ex)
        {
            Lbl_Informacion.Text = "Error: (Grid_Requisiciones_Pendientes_PageIndexChanging)" + ex.ToString();
            Mostrar_Informacion(1);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Grid_Requisiciones_Pendientes_RowDataBound
    ///DESCRIPCION:             Dependiendo de el tipo de requisición se configuran los
    ///                         botones, HyperLink
    ///PARAMETROS:              
    ///CREO:                    Noe Mosqueda Valadez
    ///FECHA_CREO:              22/Noviembre/2010
    ///MODIFICO:                
    ///FECHA_MODIFICO:          
    ///CAUSA_MODIFICACION:      
    ///*******************************************************************************
    protected void Grid_Requisiciones_Pendientes_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //Verificar si es el renglon
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Instanciar imagebutton y el hyperlink
                ImageButton Img_Btn_Filtrado_src = (ImageButton)e.Row.FindControl("Img_Btn_Filtrado");
                ImageButton Img_Btn_Revisar_src = (ImageButton)e.Row.FindControl("Img_Btn_Revisar");
                HyperLink Hyp_Lnk_Operacion_src = (HyperLink)e.Row.FindControl("Hyp_Lnk_Operacion");

                //Verificar el estatus de la requisicion
                switch (e.Row.Cells[6].Text.Trim())
                {
                    case "AUTORIZADA":
                        Img_Btn_Filtrado_src.Visible = true;
                        Img_Btn_Revisar_src.Visible = true;
                        Hyp_Lnk_Operacion_src.Visible = false;
                        break;

                    case "ALMACEN":
                        Img_Btn_Filtrado_src.Visible = false;
                        Hyp_Lnk_Operacion_src.ToolTip = "Orden de Salida";
                        Hyp_Lnk_Operacion_src.ImageUrl = "../imagenes/gridview/grid_docto2.png";
                        Hyp_Lnk_Operacion_src.Visible = true;
                        string Pagina = Session["Pagina_OS"].ToString().Trim();
                        Hyp_Lnk_Operacion_src.NavigateUrl = "Frm_Alm_Com_Orden_Salida.aspx?No_Requisicion=" + HttpUtility.HtmlEncode(e.Row.Cells[2].Text) + "&Pagina=" + Pagina;
                        break;

                    case "CONFIRMADA":
                        Img_Btn_Filtrado_src.Visible = false;
                        Hyp_Lnk_Operacion_src.Visible = true;

                        String Dato = HttpUtility.HtmlDecode(e.Row.Cells[8].Text.Trim());
                        //Verificar el tipo de operacion
                        switch (HttpUtility.HtmlDecode(e.Row.Cells[8].Text.Trim()))
                        {
                            case "RECIBO":
                                //Hyp_Lnk_Operacion_src.ToolTip = "Recibo";
                                //Hyp_Lnk_Operacion_src.ImageUrl = "../imagenes/gridview/grid_kardex.png";
                                //Hyp_Lnk_Operacion_src.NavigateUrl = "Frm_Alm_Com_Recibos.aspx?No_Requisicion=" + HttpUtility.HtmlEncode(e.Row.Cells[2].Text);
                                e.Row.Visible = false;
                                //Lbl_Informacion.Text = "No se encontraron productos a resguardar";
                                //Lbl_Informacion.Visible = true;
                                //Div_Contenedor_Msj_Error.Visible = true;
                                break;

                            case "RESGUARDO":
                                Hyp_Lnk_Operacion_src.ImageUrl = "../imagenes/gridview/grid_kardex3.png";
                                Grid_Requisiciones_Pendientes.Visible = true;
                                Div_Contenedor_Msj_Error.Visible = false;
                                String Menu_ID = "";

                                String Dato2 = HttpUtility.HtmlDecode(e.Row.Cells[10].Text.Trim());
                                    //Verificar el tipo de resguardo
                                    switch (e.Row.Cells[10].Text.Trim())
                                    {
                                        case "BIEN_MUEBLE":
                                            Requisiciones_Pendientes_Negocio.P_URL_LINK = "../Compras/Frm_Ope_Pat_Com_Alta_Bienes_Muebles.aspx";
                                            Menu_ID = Requisiciones_Pendientes_Negocio.Consulta_Menu_ID();
                                            Hyp_Lnk_Operacion_src.ToolTip = "Resguardo de Bien Mueble: " + " "+ e.Row.Cells[12].Text.Trim();
                                            Hyp_Lnk_Operacion_src.NavigateUrl = "Frm_Ope_Pat_Com_Alta_Bienes_Muebles.aspx?No_Requisicion=" + HttpUtility.HtmlEncode(e.Row.Cells[2].Text) + "&Producto_ID=" + HttpUtility.HtmlEncode(e.Row.Cells[9].Text) +  "&PAGINA=" + Menu_ID + "&Fecha_Adquisicion=" + HttpUtility.HtmlEncode(e.Row.Cells[11].Text);
                                            e.Row.Visible = true;
                                            break;

                                        case "VEHICULO":
                                            Requisiciones_Pendientes_Negocio.P_URL_LINK = "../Compras/Frm_Ope_Pat_Com_Alta_Vehiculos.aspx";
                                            Menu_ID = Requisiciones_Pendientes_Negocio.Consulta_Menu_ID();
                                            Hyp_Lnk_Operacion_src.ToolTip = "Resguardo de Vehículo: " + " "+ e.Row.Cells[12].Text.Trim();
                                            Hyp_Lnk_Operacion_src.NavigateUrl = "Frm_Ope_Pat_Com_Alta_Vehiculos.aspx?No_Requisicion=" + HttpUtility.HtmlEncode(e.Row.Cells[2].Text) + "&Producto_ID=" + HttpUtility.HtmlEncode(e.Row.Cells[9].Text) +  "&PAGINA=" + Menu_ID + "&Fecha_Adquisicion=" + HttpUtility.HtmlEncode(e.Row.Cells[11].Text);
                                            e.Row.Visible = true;
                                            break;

                                        case "CEMOVIENTE":
                                            Requisiciones_Pendientes_Negocio.P_URL_LINK = "../Compras/Frm_Ope_Pat_Com_Alta_Cemovientes.aspx";
                                            Menu_ID = Requisiciones_Pendientes_Negocio.Consulta_Menu_ID();
                                            Hyp_Lnk_Operacion_src.ToolTip = "Resguardo de Animal: " + " "+ e.Row.Cells[12].Text.Trim();
                                            Hyp_Lnk_Operacion_src.NavigateUrl = "Frm_Ope_Pat_Com_Alta_Cemovientes.aspx?No_Requisicion=" + HttpUtility.HtmlEncode(e.Row.Cells[2].Text) + "&Producto_ID=" + HttpUtility.HtmlEncode(e.Row.Cells[9].Text)  + "&PAGINA=" + Menu_ID + "&Fecha_Adquisicion=" + HttpUtility.HtmlEncode(e.Row.Cells[11].Text);
                                            e.Row.Visible = true;
                                            break;
                                        default: break;
                                    }
                                break;

                            default:
                                e.Row.Visible = false;
                             break;
                        }
                        break;
                    default:

                        e.Row.Visible = false;
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Lbl_Informacion.Text = "Error: (Grid_Requisiciones_Pendientes_PageIndexChanging)" + ex.ToString();
            Mostrar_Informacion(1);
        }
    }

    #endregion

    #region (Eventos)

    //*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Btn_Salir_Click
    ///DESCRIPCION:             Evento utilizado para salir de la página
    ///                         
    ///PARAMETROS:              
    ///CREO:                    Noe Mosqueda Valadez
    ///FECHA_CREO:              22/Noviembre/2010
    ///MODIFICO:                
    ///FECHA_MODIFICO:          
    ///CAUSA_MODIFICACION:      
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.AlternateText == "NUEVO")
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx"); //Salir a la pagina principal
            }
            else
            {
                String Tipo_Requisicion = Session["Tipo_Requisicion"].ToString().Trim();
                Estado_Inicial(Tipo_Requisicion);
                Configurar_Boton(true);
                Estatus_Componentes_Busqueda(true);
            }
        }
        catch (Exception ex)
        {
            Lbl_Informacion.Text = "Error: (Btn_Salir_Click)" + ex.ToString();
            Mostrar_Informacion(1);
        }
    }

    public void Estatus_Componentes_Busqueda(Boolean Estatus)
    {
        Btn_Buscar.Visible = Estatus;
        Txt_Busqueda.Visible = Estatus;
        Btn_Busqueda_Avanzada.Visible = Estatus;
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Configurar_Boton
    /// DESCRIPCION       :     Evento utilizado para cambiarle al AlternateText 
    ///                         al botón Btn_Salir
    /// PARAMETROS        :     
    /// CREO              :     Salvador Hernández Ramírez
    /// FECHA_CREO        :     20/Abril/2011 
    /// MODIFICO          :    
    /// FECHA_MODIFICO    :    
    /// CAUSA_MODIFICACION:    
    ///*******************************************************************************/
    public void Configurar_Boton(Boolean Estatus)
    {
        if (Estatus)
        {
            Btn_Salir.AlternateText = "NUEVO";
            Mostrar_Busqueda(true);
        }
        else
        {
            Btn_Salir.AlternateText = "Atras";
            Mostrar_Busqueda(false);
        }
    }


    //*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Btn_Salir_Click
    ///DESCRIPCION:             Evento utilizado realizar una busqueda simple de requisiciones                
    ///PARAMETROS:              
    ///CREO:                    Salvador Hernández Ramírez
    ///FECHA_CREO:              15/Noviembre/2010
    ///MODIFICO:                
    ///FECHA_MODIFICO:          
    ///CAUSA_MODIFICACION:      
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        String Tipo_Requisicion = Session["Tipo_Requisicion"].ToString().Trim();
        Estado_Inicial(Tipo_Requisicion);
    }

    //*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Btn_Cancelar_Filtrado_Click
    ///DESCRIPCION:             Evento utilizado ocultar el modal              
    ///PARAMETROS:              
    ///CREO:                    Salvador Hernández Ramírez
    ///FECHA_CREO:              15/Noviembre/2010
    ///MODIFICO:                
    ///FECHA_MODIFICO:          
    ///CAUSA_MODIFICACION:      
    ///*******************************************************************************
    protected void Btn_Cancelar_Filtrado_Click(object sender, EventArgs e)
    {
        Modal_Filtrado.Hide();
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Grid_Productos_Requisicion_SelectedIndexChanging
    /// DESCRIPCION       :     Evento  del grid para manejar la páginacion
    /// PARAMETROS        :     
    /// CREO              :     Salvador Hernández Ramírez
    /// FECHA_CREO        :     20/Abril/2011 
    /// MODIFICO          :    
    /// FECHA_MODIFICO    :    
    /// CAUSA_MODIFICACION:    
    ///*******************************************************************************/
    protected void Grid_Productos_Requisicion_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        Grid_Productos_Requisicion.PageIndex = e.NewSelectedIndex;
        Grid_Productos_Requisicion.DataSource = (DataTable)Session["Dt_Productos"];
        Grid_Productos_Requisicion.DataBind();
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Btn_Seleccionar_Requisicion_Click
    /// DESCRIPCION       :     Evento  del botón que se encuentra en el Grid, el cual es utilizado
    ///                         para obtener el Numero de Requisición.
    /// PARAMETROS        :     
    /// CREO              :     Salvador Hernández Ramírez
    /// FECHA_CREO        :     20/Abril/2011 
    /// MODIFICO          :    
    /// FECHA_MODIFICO    :    
    /// CAUSA_MODIFICACION:    
    ///*******************************************************************************/
    protected void Btn_Seleccionar_Requisicion_Click(object sender, ImageClickEventArgs e)
    {
        long No_Requisicion = 0;
        String Requisicion = "";
        ImageButton Btn_Seleccionar_Requisicion = null;
        Btn_Seleccionar_Requisicion = (ImageButton)sender;
        DataTable Dt_Productos = new DataTable();
        DataTable Dt_Requisiciones_Pendientes = new DataTable();
        DataTable Dt_Detalles_Requisicion = new DataTable();

        DataRow[] Dr_requisicion;

        String Folio = String.Empty;  // Variables que contendran la información de la requisiciòn
        String Estatus = String.Empty;
        String Area = String.Empty;
        DateTime Fecha_Convertida = new DateTime();

        Txt_Fecha_Generación.Text = ""; // Se asigna "" ya que esta puede contener valores de otra Ordend e compra
        try
        {
            Requisicion = Btn_Seleccionar_Requisicion.CommandArgument;
            No_Requisicion = Convert.ToInt64(Requisicion);
            Requisiciones_Pendientes_Negocio.P_No_Requisicion = No_Requisicion;

            Dt_Productos = Requisiciones_Pendientes_Negocio.Consulta_Productos_Requisicion();
            Dt_Detalles_Requisicion = Requisiciones_Pendientes_Negocio.Consulta_Detalles_Requisicion();

            if (Dt_Productos.Rows.Count > 0)
            {
                Session["Dt_Productos"] = Dt_Productos;
                Grid_Productos_Requisicion.DataSource = Dt_Productos;
                Grid_Productos_Requisicion.DataBind();

                // Se consulta los datos del registro seleccionado
                Dt_Requisiciones_Pendientes = (DataTable)Session["Dt_Requisiciones_Pendientes"];
                Dr_requisicion = Dt_Requisiciones_Pendientes.Select("NO_REQUISICION='" + Requisicion + "'");

                // Se agregan los datos en los Text_Boxt
                if (Dr_requisicion.Length > 0)
                {
                    if (!string.IsNullOrEmpty(Dr_requisicion[0]["FOLIO"].ToString()))
                        Folio = Dr_requisicion[0]["FOLIO"].ToString().Trim();
                    if (!string.IsNullOrEmpty(Dr_requisicion[0]["ESTATUS"].ToString()))
                        Estatus = Dr_requisicion[0]["ESTATUS"].ToString().Trim();
                    if (!string.IsNullOrEmpty(Dr_requisicion[0]["AREA"].ToString()))
                        Area = HttpUtility.HtmlDecode(Dr_requisicion[0]["AREA"].ToString().Trim());

                    Txt_Area.Text = Area.Trim();
                    Txt_Estatus.Text = Estatus.Trim();
                    Txt_Folio.Text = Folio.Trim();
                }

                if (Dt_Detalles_Requisicion.Rows.Count > 0)
                {
                    if (Dt_Detalles_Requisicion.Rows[0]["FECHA_GENERACION"].ToString() != "")
                    {
                        String Fecha = Dt_Detalles_Requisicion.Rows[0]["FECHA_GENERACION"].ToString();
                        Fecha_Convertida = Convert.ToDateTime(Fecha);
                        Txt_Fecha_Generación.Text = String.Format("{0:dd/MMM/yyyy}", Fecha_Convertida);
                    }
                    Txt_Usuario_Creo.Text = HttpUtility.HtmlDecode(Dt_Detalles_Requisicion.Rows[0]["USUARIO_CREO"].ToString());
                    Txt_Dependencia.Text = HttpUtility.HtmlDecode(Dt_Detalles_Requisicion.Rows[0]["DEPENDENCIA"].ToString());
                }
                Div_Productos_Requisicion.Visible = true;
                Div_Requisiciones_Pendientes.Visible = false;
                Div_Contenedor_Msj_Error.Visible = false;
                Configurar_Boton(false);
                Estatus_Componentes_Busqueda(false);
            }
            else
            {
                Lbl_Informacion.Text = "La requisición no contiene productos";
                Lbl_Informacion.Visible = true;
                Div_Contenedor_Msj_Error.Visible = true;
                Div_Productos_Requisicion.Visible = false;
                Div_Requisiciones_Pendientes.Visible = true;
            }
        }
        catch (Exception ex)
        {
            Lbl_Informacion.Text = "Error: (Img_Btn_Filtrado_Click)" + ex.ToString();
            Mostrar_Informacion(1);
        }
    }

    #region (Busqueda Avanzada)

    //*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Btn_Busqueda_Avanzada_Click
    ///DESCRIPCION:             Evento utilizado para realizar la busqueda abanzada        
    ///PARAMETROS:              
    ///CREO:                    Salvador Hernández Ramírez
    ///FECHA_CREO:              15/Noviembre/2010
    ///MODIFICO:                
    ///FECHA_MODIFICO:          
    ///CAUSA_MODIFICACION:      
    ///*******************************************************************************
    protected void Btn_Busqueda_Avanzada_Click(object sender, EventArgs e)
    {
        try
        {
            //Mostrar el panel de la busqueda
            Pnl_Busqueda.Visible = true;
            Modal_Busqueda.Show();
            Estado_Inicial_Busqueda_Avanzada();
            Txt_Busqueda.Text = ""; // Se asigna a "" el TextBox de la búsqueda simple 
        }
        catch (Exception ex)
        {
            Lbl_Informacion.Text = "Error: (Btn_Busqueda_Avanzada_Click)" + ex.ToString();
            Mostrar_Informacion(1);
        }
    }


    //*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Chk_Fecha_CheckedChanged
    ///DESCRIPCION:             Evento  para realizar configurar los componentes que se utilizan cuando se 
    ///                         realiza una busqueda por fecha
    ///PARAMETROS:              
    ///CREO:                    Salvador Hernández Ramírez
    ///FECHA_CREO:              15/Noviembre/2010
    ///MODIFICO:                
    ///FECHA_MODIFICO:          
    ///CAUSA_MODIFICACION:      
    ///*******************************************************************************
    protected void Chk_Fecha_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            Txt_Fecha_Inicial.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Txt_Fecha_Final.Text = DateTime.Now.ToString("dd/MMM/yyyy");

            //Habilitar los botones del calendario
            Img_Btn_Fecha_Final.Enabled = Chk_Fecha.Checked;
            Img_Btn_Fecha_Inicial.Enabled = Chk_Fecha.Checked;
            Pnl_Busqueda.Visible = true;
            Modal_Busqueda.Show();
        }
        catch (Exception ex)
        {
            Lbl_Error_Busqueda.Text = "Error: (Chk_Fecha_CheckedChanged)" + ex.ToString();
            Img_Error_Busqueda.Visible = true;
        }
    }

    //*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Chk_Area_CheckedChanged
    ///DESCRIPCION:             Evento  para realizar configurar los componentes que se utilizan cuando se 
    ///                         realiza una busqueda por  area
    ///PARAMETROS:              
    ///CREO:                    Salvador Hernández Ramírez
    ///FECHA_CREO:              15/Noviembre/2010
    ///MODIFICO:                
    ///FECHA_MODIFICO:          
    ///CAUSA_MODIFICACION:      
    ///*******************************************************************************
    protected void Chk_Area_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            //Habilitar el combo del area
            Cmb_Area.Enabled = Chk_Area.Checked;
            Pnl_Busqueda.Visible = true;
            Modal_Busqueda.Show();
        }
        catch (Exception ex)
        {
            Lbl_Error_Busqueda.Text = "Error: (Chk_Area_CheckedChanged)" + ex.ToString();
            Img_Error_Busqueda.Visible = true;
        }
    }

    //*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Chk_Dependencia_CheckedChanged
    ///DESCRIPCION:             Evento  para realizar configurar los componentes que se utilizan cuando se 
    ///                         realiza una busqueda por  dependencia
    ///PARAMETROS:              
    ///CREO:                    Salvador Hernández Ramírez
    ///FECHA_CREO:              15/Noviembre/2010
    ///MODIFICO:                
    ///FECHA_MODIFICO:          
    ///CAUSA_MODIFICACION:      
    ///*******************************************************************************
    protected void Chk_Dependencia_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            //Habilitar el combo de la dependencia
            Cmb_Dependencia.Enabled = Chk_Dependencia.Checked;
            Pnl_Busqueda.Visible = true;
            Modal_Busqueda.Show();
        }
        catch (Exception ex)
        {
            Lbl_Error_Busqueda.Text = "Error: (Chk_Dependencia_CheckedChanged)" + ex.ToString();
            Img_Error_Busqueda.Visible = true;
        }
    }

    //*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Chk_Estatus_CheckedChanged
    ///DESCRIPCION:             Evento  para realizar configurar los componentes que se utilizan cuando se 
    ///                         realiza una busqueda por  estatus
    ///PARAMETROS:              
    ///CREO:                    Salvador Hernández Ramírez
    ///FECHA_CREO:              15/Noviembre/2010
    ///MODIFICO:                
    ///FECHA_MODIFICO:          
    ///CAUSA_MODIFICACION:      
    ///*******************************************************************************
    protected void Chk_Estatus_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            //Habilitar el combo del estatus
            Cmb_Estatus_Busqueda.Enabled = Chk_Estatus.Checked;
            Pnl_Busqueda.Visible = true;
            Modal_Busqueda.Show();
        }
        catch (Exception ex)
        {
            Lbl_Error_Busqueda.Text = "Error: (Chk_Estatus_CheckedChanged)" + ex.ToString();
            Img_Error_Busqueda.Visible = true;
        }
    }


    //*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Cmb_Dependencia_SelectedIndexChanged
    ///DESCRIPCION:             Evento seleccionar la dpendencia que se va a buscar              
    ///PARAMETROS:              
    ///CREO:                    Salvador Hernández Ramírez
    ///FECHA_CREO:              15/Noviembre/2010
    ///MODIFICO:                
    ///FECHA_MODIFICO:          
    ///CAUSA_MODIFICACION:      
    ///*******************************************************************************
    protected void Cmb_Dependencia_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //Limpiar el combo de las areas
            Cmb_Area.Items.Clear();

            //Verificar el indice de la dependencia
            if (Cmb_Dependencia.SelectedIndex > 0)
                Llena_Combo_Areas(Cmb_Dependencia.SelectedItem.Value);

            //Mostrar el modal
            Modal_Busqueda.Show();
        }
        catch (Exception ex)
        {
            Lbl_Error_Busqueda.Text = "Error: (Cmb_Dependencia_SelectedIndexChanged)" + ex.ToString();
            Img_Error_Busqueda.Visible = true;
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Cancelar_Click
    ///DESCRIPCIÓN:     Evento utilizado para ocultar el modal de busqueda
    ///PARAMETROS:       
    ///CREO:            Salvador Hernández Ramírez
    ///FECHA_CREO:      08/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Cancelar_Click(object sender, EventArgs e)
    {
        Modal_Busqueda.Hide();
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Formato_Fecha
    ///DESCRIPCIÓN:     Metodo que cambia el mes dic a dec para que oracle lo acepte
    ///PARAMETROS:      1.- String Fecha, es la fecha a la cual se le cambiara el formato 
    ///                     en caso de que cumpla la condicion del if
    ///CREO:            Salvador Hernández Ramírez
    ///FECHA_CREO:      19/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public String Formato_Fecha(String Fecha)
    {
        String Fecha_Valida = Fecha;
        //Se le aplica un split a la fecha 
        String[] aux = Fecha.Split('/');
        //Se modifica a mayusculas para que oracle acepte el formato. 
        switch (aux[1])
        {
            case "dic":
                aux[1] = "DEC";
                break;
        }
        //Concatenamos la fecha, y se cambia el orden a DD-MMM-YYYY para que sea una fecha valida para oracle
        Fecha_Valida = aux[0] + "-" + aux[1] + "-" + aux[2];
        return Fecha_Valida;
    }



    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Formato_Fecha
    ///DESCRIPCIÓN:     Evento que se utiliza para realizar la consulta en base a los 
    ///                 criterios de busqueda.
    ///CREO:            Salvador Hernández Ramírez
    ///FECHA_CREO:      08/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Aceptar_Click(object sender, EventArgs e)
    {
        //Declaracion de variables
        String Busqueda = String.Empty; //Variable para la cadena de busqueda
        String Tipo_Requisicion = String.Empty; //Variable para el tipo de requisicion
        Boolean Combo_Seleccionado = false;
        try
        {
            //Obtener el tipo de la requisicion
            Tipo_Requisicion = Session["Tipo_Requisicion"].ToString().Trim();

            //Verificar si se ha seleccionado una fecha
            if (Chk_Fecha.Checked == true)
            {
                //Verificar si ha ingresado fechas
                if (Txt_Fecha_Inicial.Text.Trim() != null && Txt_Fecha_Inicial.Text.Trim() != "" && Txt_Fecha_Inicial.Text.Trim() != String.Empty)
                {
                    Busqueda = Formato_Fecha(Txt_Fecha_Inicial.Text.Trim()) + ",";

                    //Verificar si hay fecha final
                    if (Txt_Fecha_Final.Text.Trim() != null && Txt_Fecha_Final.Text.Trim() != "" && Txt_Fecha_Final.Text.Trim() != String.Empty)
                        Busqueda = Busqueda + Formato_Fecha(Txt_Fecha_Final.Text.Trim()) + ",";
                    else
                        Busqueda = Busqueda + "0,";
                }
                else
                {
                    Busqueda = "0,0,";
                }
                Combo_Seleccionado = true;
            }
            else
                Busqueda = "0,0,";

            //Verificar si hay un estatus
            if (Chk_Estatus.Checked == true)
            {
                //Verificar si se ha seleccionado un elementos
                if (Cmb_Estatus_Busqueda.SelectedIndex > 0)
                    Busqueda = Busqueda + Cmb_Estatus_Busqueda.SelectedItem.Value + ",";
                else
                {
                    Busqueda = Busqueda + "0,";
                }
                Combo_Seleccionado = true;
            }
            else
                Busqueda = Busqueda + "0,";

            //Verificar si hay una dependencia
            if (Chk_Dependencia.Checked == true)
            {
                //Verificar si se ha seleccionado una del combo
                if (Cmb_Dependencia.SelectedIndex > 0)
                    Busqueda = Busqueda + Cmb_Dependencia.SelectedItem.Value + ",";
                else
                {
                    Busqueda = Busqueda + "0,";
                }
                Combo_Seleccionado = true;
            }
            else
                Busqueda = Busqueda + "0,";

            // Verificar si hay un área
            if (Chk_Area.Checked == true)
            {
                //Verificar si se ha seleccionado un elemento
                if (Cmb_Area.SelectedIndex > 0)
                    Busqueda = Busqueda + Cmb_Area.SelectedItem.Value;
                else
                {
                    Busqueda = Busqueda + "0";
                }
                Combo_Seleccionado = true;
            }
            else
                Busqueda = Busqueda + "0";

            if (Combo_Seleccionado == true)
            {
                //Llenar el grid con la busqueda
                Llena_Grid_Requisiciones_Pendientes(Tipo_Requisicion, Busqueda, -1);
            }
            else
            {
                Lbl_Error_Busqueda.Text = "Seleccionar un criterio de búsqueda";
                Img_Error_Busqueda.Visible = true;
                Modal_Busqueda.Show();
            }
        }
        catch (Exception ex)
        {
            Lbl_Error_Busqueda.Text = "Error: (Btn_Aceptar_Click)" + ex.ToString();
            Img_Error_Busqueda.Visible = true;
            Modal_Busqueda.Show();
        }
    }



    #endregion

    #region (Filtrado)

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Img_Btn_Filtrado_Click
    ///DESCRIPCIÓN:     Evento que se utiliza para mostrar el modal cuando se
    ///                 requiere filtrar una requisicion
    ///                 
    ///CREO:            Salvador Hernández Ramírez
    ///FECHA_CREO:      08/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Img_Btn_Filtrado_Click(object sender, ImageClickEventArgs e)
    {
        //Declaracion de variables
        GridViewRow Renglon; //Renglon para obtener la informacion del grid
        Session["Operacion"] = "FILTRADA";

        try
        {
            //Instanciar variables
            Renglon = ((GridViewRow)((ImageButton)sender).NamingContainer);

            //Colocar el numero de la requisicion en variable de sesion
            Session["No_Requisicion"] = Renglon.Cells[2].Text.Trim();
            Txt_No_Requisicion_Filtrado.Enabled = true;
            Txt_No_Requisicion_Filtrado.Text = Renglon.Cells[2].Text;
            Txt_No_Requisicion_Filtrado.Enabled = false;

            //Mostar el modal
            Txt_Comentarios_Filtrado.Text = "";
            Lbl_Error_Filtrado.Visible = false;
            Img_Error_Filtrado.Visible = false;
            Lbl_Error_Filtrado.Text = "";
            Lbl_Comentario.Text = "Comentario";
            Lbl_Titulo_Operacion.Text = "Filtrar Requisición";
            Upd_Filtrado.Update();
            Modal_Filtrado.Show();
        }
        catch (Exception ex)
        {
            Lbl_Informacion.Text = "Error: (Img_Btn_Filtrado_Click)" + ex.ToString();
            Mostrar_Informacion(1);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Btn_Aceptar_Filtrado_Click
    /// DESCRIPCION       :     Evento clic del boton Btn_Aceptar_Filtrado del modal, el cual
    ///                         es utilizado para aceptar el filtrado de la requisición.
    /// PARAMETROS        :     
    /// CREO              :     Salvador Hernández Ramírez
    /// FECHA_CREO        :     14/Febrero/2011
    /// MODIFICO          :     
    /// FECHA_MODIFICO    :     
    /// CAUSA_MODIFICACION:    
    ///*******************************************************************************/
    protected void Btn_Aceptar_Filtrado_Click(object sender, EventArgs e)
    {
        //Declaracion de variables
        String Tipo_Requisicion = String.Empty; // Variable para el tipo de requisicion

        try
        {
            if (Session["Operacion"].ToString() == "REVISAR") // Si se va a cambiar la requisición al Estatus "REVISAR"
            {
                // Se Obtiene el tipo de requisicion 
                Tipo_Requisicion = HttpUtility.HtmlDecode(Request.QueryString["Tipo_Requisicion"].ToString());

                // Verificar si se escribio el comentario.
                if (Txt_Comentarios_Filtrado.Text.Trim() != null && Txt_Comentarios_Filtrado.Text.Trim() != String.Empty && Txt_Comentarios_Filtrado.Text.Trim() != "")
                {
                    Revisar_Requisicion(Convert.ToInt64(Txt_No_Requisicion_Filtrado.Text.Trim()), Txt_Comentarios_Filtrado.Text.Trim());
                    Modal_Filtrado.Hide();

                    Txt_Busqueda.Text = "";

                    // Colocar la pagina en el estatus inicial dependiendo del tipo de requisicion
                    Estado_Inicial(Tipo_Requisicion);

                    //Colocar el tipo de la requisicion en una variable de sesion
                    Session["Tipo_Requisicion"] = Tipo_Requisicion;

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Requisición", "alert('Requisición  " + Txt_No_Requisicion_Filtrado.Text + " REVISADA" + "');", true);
                }
                else
                {
                    Modal_Filtrado.Show();
                    Lbl_Error_Filtrado.Visible = true;
                    Img_Error_Filtrado.Visible = true;
                    Lbl_Error_Filtrado.Text = "Favor de proporcionar los comentarios.";
                }
            }
            else if (Session["Operacion"].ToString() == "FILTRADA") // Si se va a cambiar la requisición al Estatus "FILTRADA"
            {
                //Obtener eL tipo de requisicion 
                Tipo_Requisicion = HttpUtility.HtmlDecode(Request.QueryString["Tipo_Requisicion"].ToString());

                Filtrar_Requisicion(Convert.ToInt64(Txt_No_Requisicion_Filtrado.Text.Trim()), Txt_Comentarios_Filtrado.Text.Trim());
                Modal_Filtrado.Hide();

                Txt_Busqueda.Text = "";

                //Colocar la pagina en el estatus inicial dependiendo del tipo de requisicion
                Estado_Inicial(Tipo_Requisicion);

                // Colocar el tipo de la requisicion en una variable de sesion
                Session["Tipo_Requisicion"] = Tipo_Requisicion;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Requisición", "alert('Requisición  " + Txt_No_Requisicion_Filtrado.Text + " FILTRADA" + "');", true);

            }
        }
        catch (Exception ex)
        {
            Lbl_Error_Filtrado.Text = "Error: (Btn_Aceptar_Filtrado_Click)" + ex.ToString();
            Lbl_Error_Filtrado.Enabled = true;
            Img_Error_Filtrado.Visible = true;
        }
    }
    #endregion

    # region (Revisar)

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Img_Btn_Revisar_Click
    /// DESCRIPCION       :     Evento utilizado para poner la requisición en  
    ///                         Estatus "REVISAR"
    /// PARAMETROS        :     
    ///                         
    /// CREO              :     Salvador Hernández Ramírez
    /// FECHA_CREO        :     14/Abril/2011 
    /// MODIFICO          :    
    /// FECHA_MODIFICO    :    
    /// CAUSA_MODIFICACION:    
    ///*******************************************************************************/
    protected void Img_Btn_Revisar_Click(object sender, ImageClickEventArgs e)
    {
        //Declaracion de variables
        GridViewRow Renglon; //Renglon para obtener la informacion del Grid
        Session["Operacion"] = "REVISAR";

        try
        {
            // Instanciar variables
            Renglon = ((GridViewRow)((ImageButton)sender).NamingContainer);

            // Colocar el numero de la requisicion en variable de sesion
            Session["No_Requisicion"] = Renglon.Cells[2].Text.Trim();
            Txt_No_Requisicion_Filtrado.Enabled = true;
            Txt_No_Requisicion_Filtrado.Text = Renglon.Cells[2].Text;
            Txt_No_Requisicion_Filtrado.Enabled = false;

            // Mostar el modal
            Txt_Comentarios_Filtrado.Text = "";
            Lbl_Error_Filtrado.Visible = false;
            Img_Error_Filtrado.Visible = false;
            Lbl_Error_Filtrado.Text = "";
            Lbl_Comentario.Text = "*Comentario";
            Lbl_Titulo_Operacion.Text = "Revisar Requisición";

            Upd_Filtrado.Update();
            Modal_Filtrado.Show();
        }
        catch (Exception ex)
        {
            Lbl_Informacion.Text = "Error: (Img_Btn_Filtrado_Click)" + ex.ToString();
            Mostrar_Informacion(1);
        }
    }
    # endregion
    #endregion

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
            Botones.Add(Btn_Salir);

            if (!String.IsNullOrEmpty(Request.QueryString["?PAGINA"]))
            {
                if (Es_Numero(Request.QueryString["?PAGINA"].Trim()))
                {
                    //Consultamos el menu de la página.
                    Dr_Menus = Cls_Sessiones.Menu_Control_Acceso.Select("MENU_ID=" + Request.QueryString["?PAGINA"]);

                    if (Dr_Menus.Length > 0)
                    {
                        //Validamos que el menu consultado corresponda a la página a validar.
                        if (Dr_Menus[0][Apl_Cat_Menus.Campo_URL_Link].ToString().Contains(URL_Pagina))
                        {
                            Cls_Util.Configuracion_Acceso_Sistema_SIAS_AlternateText(Botones, Dr_Menus[0]); // Habilitamos la configuracón de los botones.
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
