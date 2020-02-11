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
using Presidencia.Asignar_Cotizadores.Negocio;
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;

public partial class paginas_Compras_Frm_Ope_Com_Asignar_Cotizadores : System.Web.UI.Page
{
    ///*******************************************************************************
    /// VARIABLES
    ///*******************************************************************************
    #region Variables
    Cls_Ope_Com_Asignar_Cotizadores_Negocio Cotizadores_Datos;
    #endregion Fin Variables

    ///*******************************************************************************
    /// REGION PAGE_LOAD
    ///*******************************************************************************
    #region Page_Load

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Init
    ///DESCRIPCIÓN: 
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 31/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Page_Init(object sender, EventArgs e)
    {
        Cotizadores_Datos = new Cls_Ope_Com_Asignar_Cotizadores_Negocio();
        Estatus_Formulario("Inicial");
        Llenar_Grid_Cotizaciones();    
        //llenamos el combo de estatus;
        Llenar_Combo_Estatus();
    }

    #endregion Fin Page_Load
    ///*******************************************************************************
    /// REGION METODOS
    ///*******************************************************************************

    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Estatus_Formulario
    ///DESCRIPCIÓN: 
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 11/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Estatus_Formulario(String Estatus)
    {
        switch (Estatus)
        {

            case "Inicial":
                //Manejo de los Div
                Div_Busqueda.Visible = true;
                Div_Listado_Cotizaciones.Visible = true;
                Div_Datos_Cotizaciones.Visible = false;
                Div_Busqueda.Visible = true;
                //Boton Modificar
                Btn_Modificar.Visible = false;
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Agregar_Cotizador.Enabled = false;
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Inicio";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";

                Configuracion_Acceso("Frm_Ope_Com_Asignar_Cotizadores.aspx");
                break;
            case "General":
                //Manejo de los Div
                Div_Busqueda.Visible = false;
                Div_Listado_Cotizaciones.Visible = false;
                Div_Datos_Cotizaciones.Visible = true;
                Div_Busqueda.Visible = false;
                //Boton Modificar
                Btn_Modificar.Visible = true;
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Agregar_Cotizador.Enabled = false;
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Listado";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Grid_Concepto_Cotizadores.Enabled = false;
                break;
            case "Modificar":
                //Manejo de los Div
                Div_Busqueda.Visible = false;
                Div_Listado_Cotizaciones.Visible = false;
                Div_Datos_Cotizaciones.Visible = true;
                Div_Busqueda.Visible = false;
                Btn_Agregar_Cotizador.Enabled = true;
                //Boton Modificar
                Btn_Modificar.Visible = true;
                Btn_Modificar.ToolTip = "Actualizar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Grid_Concepto_Cotizadores.Enabled = true;
                break;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Forma()
    ///DESCRIPCIÓN: Metodo que habilitar o deshabilitar los componentes
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 15/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Habilitar_Componentes(bool Estatus)
    {
        Txt_Folio.Enabled = false;
        Txt_Fecha.Enabled = false;
        Txt_Total.Enabled = false;
        Txt_Condiciones.Enabled = false;
        Txt_Tipo.Enabled = false;
        Cmb_Cotizadores.Enabled = false;
        Cmb_Concepto.Enabled = Estatus;
        Cmb_Estatus.Enabled = Estatus;
        Grid_Concepto_Cotizadores.Enabled = Estatus;
        Btn_Agregar_Cotizador.Enabled = Estatus;

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Forma()
    ///DESCRIPCIÓN: Metodo que permite limpiar los componentes
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 15/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Limpiar_Forma()
    {
        Txt_Folio.Text = "";
        Txt_Fecha.Text = "";
        Txt_Total.Text = "";
        Txt_Condiciones.Text = "";
        Txt_Tipo.Text = "";
        Cmb_Concepto.SelectedIndex = 0;
        if(Cmb_Cotizadores.Items.Count != 0)
            Cmb_Cotizadores.SelectedIndex = 0;
        Cmb_Estatus.SelectedIndex = 0;
        Grid_Concepto_Cotizadores.DataBind();
        Session["Dt_Cotizaciones"] = null;
        Session["Dt_Cotizadores"] = null;
        Session["No_Cotizacion"] = null;
        Cotizadores_Datos = new Cls_Ope_Com_Asignar_Cotizadores_Negocio();
        
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Estatus()
    ///DESCRIPCIÓN: Metodo que permite cargar los el combo de estatus
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 15/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Estatus()
    {
        Cmb_Estatus.Items.Clear();
        Cmb_Estatus.Items.Add("<<SELECCIONAR>>");
        Cmb_Estatus.Items.Add("ASIGNAR COTIZADOR");
        Cmb_Estatus.Items.Add("COTIZADOR DEFINIDO");
        Cmb_Estatus.Items[0].Value = "0";
        Cmb_Estatus.Items[0].Selected = true;
    }

    public void LLenar_Combo_Concepto()
    {
        Cmb_Concepto.Items.Clear();
        DataTable Data_Table = Cotizadores_Datos.Consultar_Conceptos();
        Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Concepto, Data_Table);
        Cmb_Concepto.SelectedIndex = 0;
    }

    public void Agregar_Cotizadores()
    {
        String Concepto_Id = Cmb_Concepto.SelectedValue;
        DataRow[] Filas;
        DataTable Dt = (DataTable)Session["Dt_Cotizadores"];
        Filas = Dt.Select("Concepto_ID='" + Concepto_Id + "'");
        if (Filas.Length > 0)
        {
            //al usuario que elemento ha agregar ya existe en la tabla de grupos.
            Lbl_Mensaje_Error.Text += "+ No se puede agregar el Concepto " + Cmb_Concepto.SelectedItem.Text + " ya que esta ya se ha agregado <br/>";
            Div_Contenedor_Msj_Error.Visible = true;
        }
        else
        {
            DataRow Fila_Nueva = Dt.NewRow();
            //Asignamos los valores a la fila
            Fila_Nueva["Concepto_ID"] = Cmb_Concepto.SelectedValue.Trim();
            Fila_Nueva["Clave_Concepto"] = Cmb_Concepto.SelectedItem.Text.Trim().Substring(0,4);
            Fila_Nueva["Descripcion_Concepto"] = Cmb_Concepto.SelectedItem.Text.Trim().Substring(5);
            Fila_Nueva["Empleado_ID"] = Cmb_Cotizadores.SelectedValue.Trim();
            Fila_Nueva["Nombre_Empleado"] = Cmb_Cotizadores.SelectedItem.Text.Trim();
            Dt.Rows.Add(Fila_Nueva);
            Dt.AcceptChanges();
            Session["Dt_Cotizadores"] = Dt;
            //cargamos el grid de cotizadores
            Grid_Concepto_Cotizadores.DataSource = Dt;
            Grid_Concepto_Cotizadores.DataBind();
            Div_Grid_Concepto_Cotizadores.Visible = true;
        }

    }
    #endregion Fin Metodos

    ///*******************************************************************************
    /// REGION GRID
    ///*******************************************************************************
    #region Grid

    #region Grid_Cotizaciones
    protected void Grid_Cotizaciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        Estatus_Formulario("General");
        GridViewRow Row = Grid_Cotizaciones.SelectedRow;
        Cotizadores_Datos.P_No_Cotizacion = Grid_Cotizaciones.SelectedDataKey["No_Cotizacion"].ToString();
        //Consultamos los datos de lal proceso de comite de compras seleccionado 
        DataTable Dt_Cotizacion = Cotizadores_Datos.Consultar_Cotizaciones();
        Session["No_Cotizacion"] = Dt_Cotizacion.Rows[0][Ope_Com_Cotizaciones.Campo_No_Cotizacion].ToString().Trim();
        Txt_Folio.Text = Dt_Cotizacion.Rows[0][Ope_Com_Cotizaciones.Campo_Folio].ToString();
        Txt_Fecha.Text = Dt_Cotizacion.Rows[0]["FECHA"].ToString();
        //Cmb_Tipo.SelectedValue = Dt_Comite_Compras.Rows[0][Ope_Com_Comite_Compras.Campo_Tipo].ToString().Trim();
        Txt_Tipo.Text = Dt_Cotizacion.Rows[0][Ope_Com_Cotizaciones.Campo_Tipo].ToString();
        Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByText(Dt_Cotizacion.Rows[0][Ope_Com_Cotizaciones.Campo_Estatus].ToString().Trim()));
        Txt_Condiciones.Text = Dt_Cotizacion.Rows[0][Ope_Com_Cotizaciones.Campo_Condiciones].ToString();
        Txt_Total.Text = Dt_Cotizacion.Rows[0][Ope_Com_Cotizaciones.Campo_Total].ToString();
        //Llenamos Combo de Giros;
        LLenar_Combo_Concepto();
        //Consultamos los cotizadores ya dados de alta para esta cotizacion
        DataTable Dt_Cotizadores = Cotizadores_Datos.Consultar_Detalle_Cotizaciones();
        if (Dt_Cotizacion.Rows.Count != 0)
        {
            Div_Grid_Concepto_Cotizadores.Visible = true;
            Grid_Concepto_Cotizadores.DataSource = Dt_Cotizadores;
            Grid_Concepto_Cotizadores.DataBind();
            Session["Dt_Cotizadores"] = Dt_Cotizadores;
        }

        
        
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Requisiciones_PageIndexChanging
    ///DESCRIPCIÓN: Metodo que permite la paginacion del Grid_Requisicion
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 11/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Cotizaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Cotizaciones.PageIndex = e.NewPageIndex;
        Grid_Cotizaciones.DataSource = Session["Dt_Cotizaciones"];
        Grid_Cotizaciones.DataBind();

    }
    public void Llenar_Grid_Cotizaciones()
    {
        DataTable Dt_Cotizaciones = Cotizadores_Datos.Consultar_Cotizaciones();
        if (Dt_Cotizaciones.Rows.Count != 0)
        {
            Grid_Cotizaciones.DataSource = Dt_Cotizaciones;
            Grid_Cotizaciones.DataBind();
            Session["Dt_Cotizaciones"] = Dt_Cotizaciones;
        }
        else
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "No se encontraron Cotizaciones ";
            Grid_Cotizaciones.DataSource = new DataTable();
            Grid_Cotizaciones.DataBind();
        }
    }
    #endregion Fin_Grid_Cotizaciones
    #region Grid_Concepto_Cot

    protected void Grid_Concepto_Cotizadores_SelectedIndexChanged(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        DataRow[] Renglones;
        DataRow Renglon;
        //Obtenemos el Id del producto seleccionado
        GridViewRow selectedRow = Grid_Concepto_Cotizadores.Rows[Grid_Concepto_Cotizadores.SelectedIndex];
        String Concepto_ID = Grid_Concepto_Cotizadores.SelectedDataKey["Concepto_ID"].ToString().Trim();
        Renglones = ((DataTable)Session["Dt_Cotizadores"]).Select(Ope_Com_Det_Cotizaciones.Campo_Giro_ID + "='" + Concepto_ID + "'");

        if (Renglones.Length > 0)
        {
            Renglon = Renglones[0];
            DataTable Tabla = (DataTable)Session["Dt_Cotizadores"];
            Tabla.Rows.Remove(Renglon);
            //Asignamos el nuevo valor al datatable de Session
            Session["Dt_Cotizadores"] = Tabla;
            Grid_Concepto_Cotizadores.SelectedIndex = (-1);
            Grid_Concepto_Cotizadores.DataSource = Tabla;
            Grid_Concepto_Cotizadores.DataBind();
            if (Tabla.Rows.Count == 0)
                Div_Grid_Concepto_Cotizadores.Visible = false;
        }
    }
    #endregion


    #endregion Fin Grid

    ///*******************************************************************************
    /// REGION EVENTOS
    ///*******************************************************************************
    #region Eventos

    protected void Cmb_Concepto_SelectedIndexChanged(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        if (Cmb_Concepto.SelectedIndex == 0)
        {
            Cmb_Cotizadores.Enabled = false;
            if (Cmb_Cotizadores.Items.Count != 0)
                Cmb_Cotizadores.SelectedIndex = 0;
        }
        else
        {
            Cmb_Cotizadores.Enabled = true;
            //Cargamos el combo de Cotizadores de acuerdo a lo seleccionado
            Cotizadores_Datos.P_Concepto_ID = Cmb_Concepto.SelectedValue.Trim();
            Cmb_Cotizadores.Items.Clear();
            DataTable Data_Table = Cotizadores_Datos.Consultar_Cotizadores();
            Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Cotizadores, Data_Table);
            Cmb_Cotizadores.SelectedIndex = 0;

        }
    }

    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        switch (Btn_Modificar.ToolTip)
        {
            case "Modificar":
                Div_Contenedor_Msj_Error.Visible = false;
                Lbl_Mensaje_Error.Text = "";

                if (Txt_Folio.Text.Trim() == String.Empty)
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "+Es necesario seleccionar una cotizacion </br>";
                }
                else
                {
                    Estatus_Formulario("Modificar");
                    Habilitar_Componentes(true);
                }
                break;
            case "Actualizar":
                Div_Contenedor_Msj_Error.Visible = false;
                Lbl_Mensaje_Error.Text = "";

                if (Cmb_Estatus.SelectedIndex == 0)
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text += "+ Es necesario seleccionar un estatus </br>";
                }
                if (Cmb_Estatus.SelectedIndex == 2)
                {
                    //EN caso de seleccionar el estatus de COTIZADOR DEFINIDO
                    //se obliga al usuario a insertar todos los cotizadores necesarios para el grid de 
                    if ((Cmb_Concepto.Items.Count-1) != Grid_Concepto_Cotizadores.Rows.Count)
                    {
                        Div_Contenedor_Msj_Error.Visible = true;
                        Lbl_Mensaje_Error.Text += "+ Debe agregar " +Cmb_Concepto.Items.Count + " Cotizadores </br>" ;
                    }
                }

                if (Grid_Concepto_Cotizadores.Rows.Count == 0)
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text += "+ No puede guardar si no ha agregado por lo menos 1 Cotizador </br>";
                }

                if (Div_Contenedor_Msj_Error.Visible == false)
                {
                    try
                    {
                        Cotizadores_Datos.P_No_Cotizacion = Session["No_Cotizacion"].ToString().Trim();
                        Cotizadores_Datos.P_Estatus = Cmb_Estatus.SelectedValue;
                        Cotizadores_Datos.P_Dt_Cotizadores = (DataTable)Session["Dt_Cotizadores"];
                        ////Damos de alta el registro de comite de compras
                        Cotizadores_Datos.Modificar_Cotizacion();
                        Estatus_Formulario("Inicial");
                        Limpiar_Forma();
                        Habilitar_Componentes(false);
                        Llenar_Grid_Cotizaciones();
                    }
                    catch (Exception Ex)
                    {
                        throw new Exception("Error al Modificar la cotizacion :Error[" + Ex.Message + "]");
                    }
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
            case "Cancelar":
                Estatus_Formulario("Inicial");
                Limpiar_Forma();
                Habilitar_Componentes(false);
                Llenar_Grid_Cotizaciones();
                break;
            case "Listado":
                Estatus_Formulario("Inicial");
                Limpiar_Forma();
                Habilitar_Componentes(false);
                Llenar_Grid_Cotizaciones();
                break;
            case "Inicio":
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                Limpiar_Forma();
                Habilitar_Componentes(false);
                break;
        }
    }

    protected void Btn_Agregar_Cotizador_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        if (Cmb_Cotizadores.SelectedIndex == 0)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "+ Debe seleccionar un Cotizador de acuerdo al giro";
        }
        else
        {
            if (Session["Dt_Cotizadores"] != null)
            {
                Agregar_Cotizadores();
            }
            else
            {
                //creamos la session por primera vez
                DataTable Dt_Cotizadores = new DataTable();
                Dt_Cotizadores.Columns.Add("Concepto_ID", typeof(System.String));
                Dt_Cotizadores.Columns.Add("Clave_Concepto", typeof(System.String));
                Dt_Cotizadores.Columns.Add("Descripcion_Concepto", typeof(System.String));
                Dt_Cotizadores.Columns.Add("Empleado_ID", typeof(System.String));    
                Dt_Cotizadores.Columns.Add("Nombre_Empleado", typeof(System.String));
                Session["Dt_Cotizadores"] = Dt_Cotizadores;
                //Agregamos el cotizador a la tabla
                Agregar_Cotizadores();
            }

        }//fin del else
    }

    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        if (Txt_Busqueda.Text.Trim() == String.Empty)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Es necesario ingresar el folio de la cotizacion a buscar";

        }
        else
        {
            Cotizadores_Datos.P_Folio = Txt_Busqueda.Text.Trim();
            Llenar_Grid_Cotizaciones();
        }
    }

    #endregion Fin Eventos

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
            Botones.Add(Btn_Modificar);
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
