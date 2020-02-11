using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Linq;
using System.Windows.Forms;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using Presidencia.Sindicatos.Negocios;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Cuentas_Contables.Negocio;
using AjaxControlToolkit;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using Presidencia.Parametros_Contabilidad.Negocio;
using Presidencia.SAP_Partidas_Especificas.Negocio;
using Presidencia.Catalogo_SAP_Fuente_Financiamiento.Negocio;
using Presidencia.Dependencias.Negocios;
using Presidencia.Area_Funcional.Negocio;
using Presidencia.Catalogo_Compras_Proyectos_Programas.Negocio;
using Presidencia.SAP_Operacion_Departamento_Presupuesto.Negocio;
using Presidencia.Empleados.Negocios;
using Presidencia.Compromisos_Contabilidad.Negocios;
using Presidencia.Generar_Reservas.Negocio;
using Presidencia.Manejo_Presupuesto.Datos;

public partial class paginas_Contabilidad_Frm_Ope_Con_Reservas : System.Web.UI.Page
{
    private static String P_Dt_Reservas = "P_Dt_Reservas";
    private static String P_Dt_Programas = "P_Dt_Programas";
    private static String P_Dt_Partidas = "P_Dt_Partidas";
    private static String Importe = "Importe";
    
    #region (Page Load)
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
        if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

        try
        {
            if (!IsPostBack)
            {
                Session["P_Dt_Reservas"] = null;
                Session["P_Dt_Programas"] = null;
                Session["P_Dt_Partidas"] = null;
                Session["Importe"] = null;
                Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                Limpia_Controles();     //Limpia los controles del forma
                ViewState["SortDirection"] = "ASC";
                //Llenar_Cmb_Unidad_Responsable_Busqueda();
                DateTime _DateTime = DateTime.Now;
                int dias = _DateTime.Day;
                dias = dias * -1;
                dias++;
                _DateTime = _DateTime.AddDays(dias);
                Txt_Fecha.Text = DateTime.Now.ToString("dd/MMM/yyyy").ToUpper();
                Txt_Fecha_Inicio.Text = _DateTime.ToString("dd/MMM/yyyy").ToUpper();
                Txt_Fecha_Final.Text = DateTime.Now.ToString("dd/MMM/yyyy").ToUpper();

                Cls_Cat_Dependencias_Negocio Dependencia_Negocio = new Cls_Cat_Dependencias_Negocio();
                DataTable Dt_Dependencias = Dependencia_Negocio.Consulta_Dependencias();
                Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Unidad_Responsable, Dt_Dependencias, "CLAVE_NOMBRE", "DEPENDENCIA_ID");
                Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Unidad_Responsable_Busqueda, Dt_Dependencias, 1, 0);
                Cmb_Unidad_Responsable.SelectedIndex = 0;
                Cmb_Unidad_Responsable_Busqueda.SelectedValue = Cls_Sessiones.Dependencia_ID_Empleado;
                //MOD
                Llenar_Grid_Reservas();
                Cmb_Unidad_Responsable_Busqueda.Enabled = false;
                //Verificar si su rol es jefe de dependencia, admin de modulo o admin de sistema
                DataTable Dt_Grupo_Rol = Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Rol_ID.ToString());
                if (Dt_Grupo_Rol != null)
                {
                    String Grupo_Rol = Dt_Grupo_Rol.Rows[0][Apl_Cat_Roles.Campo_Grupo_Roles_ID].ToString();
                    if (Grupo_Rol == "00001" || Grupo_Rol == "00002")
                    {
                        Cmb_Unidad_Responsable_Busqueda.Enabled = true;
                    }
                    else
                    {
                        DataTable Dt_URs = Cls_Util.Consultar_URs_De_Empleado(Cls_Sessiones.Empleado_ID);
                        if (Dt_URs.Rows.Count > 1)
                        {
                            Cmb_Unidad_Responsable_Busqueda.Enabled = true;
                            Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Unidad_Responsable_Busqueda, Dt_URs, 1, 0);
                            Cmb_Unidad_Responsable_Busqueda.SelectedValue = Cls_Sessiones.Dependencia_ID_Empleado;
                        }
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
            Botones.Add(Btn_Nuevo);
            Botones.Add(Btn_Modificar);
            //Botones.Add(Btn_Eliminar);
            //Botones.Add(Btn_Mostrar_Popup_Busqueda);

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

    #region (Metodos)
    #region(Metodos Generales)
    ///*******************************************************************************
    // NOMBRE DE LA FUNCIÓN: Llenar_Grid_Reservas
    // DESCRIPCIÓN: Llena el grid principal de reservas
    // RETORNA: 
    // CREO: Sergio Manuel Gallardo Andrade
    // FECHA_CREO: 17/noviembre/2011 
    // MODIFICO:
    // FECHA_MODIFICO:
    // CAUSA_MODIFICACIÓN:
    //********************************************************************************/
    public void Llenar_Grid_Reservas()
    {
        Cls_Ope_Con_Reservas_Negocio Reserva_Negocio = new Cls_Ope_Con_Reservas_Negocio();
        Reserva_Negocio.P_Dependencia_ID = Cmb_Unidad_Responsable_Busqueda.SelectedValue;//Cmb_Dependencia.SelectedValue;       
        //Requisicion_Negocio.P_Fecha_Inicial = Txt_Fecha_Inicial.Text;
        Reserva_Negocio.P_Fecha_Inicial = String.Format("{0:dd-MMM-yyyy}", Txt_Fecha_Inicio.Text);
        Reserva_Negocio.P_Fecha_Final = Txt_Fecha_Final.Text;
        Reserva_Negocio.P_Fecha_Final = String.Format("{0:dd-MMM-yyyy}", Txt_Fecha_Final.Text);
        if (Txt_No_Folio.Text.Trim().Length > 0)
        {
            String No_Reserva = Txt_No_Folio.Text;
            No_Reserva = No_Reserva.ToUpper();
            int Int_No_Reserva = 0;
            try
            {
                Int_No_Reserva = int.Parse(No_Reserva);
            }
            catch (Exception Ex)
            {
                String Str = Ex.ToString();
                No_Reserva = "0";
            }
            Reserva_Negocio.P_No_Reserva = No_Reserva;
        }
        Session[P_Dt_Reservas] = Reserva_Negocio.Consultar_Reservas();
        if (Session[P_Dt_Reservas] != null && ((DataTable)Session[P_Dt_Reservas]).Rows.Count > 0)
        {
            Grid_Reservas.Columns[2].Visible = true;
            Grid_Reservas.Columns[7].Visible = true;
            Grid_Reservas.DataSource = Session[P_Dt_Reservas] as DataTable;
            Grid_Reservas.DataBind();
            Grid_Reservas.Columns[2].Visible = false;
            Grid_Reservas.Columns[7].Visible = false;
        }
        else
        {
            Session[P_Dt_Reservas] = null;
            Grid_Reservas.DataSource = null;
            Grid_Reservas.DataBind();
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 13/Octubre/2011
    /// MODIFICO          : 
    /// FECHA_MODIFICO    : 
    /// CAUSA_MODIFICACION: 
    ///*******************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Limpia_Controles();
            Habilitar_Controles("Inicial"); //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION : Limpia los controles que se encuentran en la forma
    /// PARAMETROS  : 
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 13/Octubre/2011
    /// MODIFICO          : 
    /// FECHA_MODIFICO    : 
    /// CAUSA_MODIFICACION: 
    ///*******************************************************************************
    private void Limpia_Controles()
    {
        try
        {
            Txt_Beneficiario.Text = "";
            Txt_No_Folio.Text = "";
            Txt_No_Reserva.Text = "";
            Cmb_Fuente_Financiamiento.Items.Clear();
            Cmb_Programa.Items.Clear();
            Cmb_Partida.Items.Clear();
            Txt_Importe.Text = "";
            Txt_Conceptos.Text = "";
            Grid_Reservas.DataSource = new DataTable();
            Grid_Reservas.DataBind();
            Txt_Saldo.Text = "";
        }
        catch (Exception ex)
        {
            throw new Exception("Limpiar_Controles " + ex.Message.ToString(), ex);
        }
    }

    /// ******************************************************************************************
    /// NOMBRE: Grid_Requisiciones_Sorting
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// CREÓ: Gustavo Angeles Cruz
    /// FECHA CREÓ: 11/Junio/2011
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// ******************************************************************************************
    protected void Grid_Reservas_Sorting(object sender, GridViewSortEventArgs e)
    {
        Grid_Sorting(Grid_Reservas, ((DataTable)Session[P_Dt_Reservas]), e);
    }
    /// *****************************************************************************************
    /// NOMBRE: Grid_Sorting
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// CREÓ: Gustavo Angeles Cruz
    /// FECHA CREÓ: 11/Junio/2011
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// *****************************************************************************************
    private void Grid_Sorting(GridView Grid, DataTable Dt_Table, GridViewSortEventArgs e)
    {
        if (Dt_Table != null)
        {
            DataView Dv_Vista = new DataView(Dt_Table);
            String Orden = ViewState["SortDirection"].ToString();
            if (Orden.Equals("ASC"))
            {
                Dv_Vista.Sort = e.SortExpression + " DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Vista.Sort = e.SortExpression + " ASC";
                ViewState["SortDirection"] = "ASC";
            }
            Grid.DataSource = Dv_Vista;
            Grid.DataBind();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///               para a siguiente operación
    /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
    ///                          si es una alta, modificacion
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 11/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado; ///Indica si el control de la forma va hacer habilitado para utilización del usuario
        try
        {
            Habilitado = false;
            switch (Operacion)
            {
                case "Inicial":
                    Habilitado = false;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Salir";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    //Btn_Eliminar.Visible = true;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Div_Reservas_Presentacion.Style.Add("display", "block");
                    Div_Reserva_Datos.Style.Add("display", "none");
                    Configuracion_Acceso("Frm_Ope_Con_Reservas.aspx");
                    Btn_Modificar.Visible = false;
                    Cmb_Estatus.Enabled= false;
                    tr_Saldo.Style.Add("display","none"); 
                    break;

                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    //Btn_Eliminar.Visible = false;                    
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Div_Reservas_Presentacion.Style.Add("display", "none");
                    Div_Reserva_Datos.Style.Add("display", "block");
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Cmb_Estatus.Enabled= false;
                    tr_Saldo.Style.Add("display", "none"); 
                    break;

                case "Modificar":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    //Btn_Eliminar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Cmb_Estatus.Enabled= true;
                    tr_Saldo.Style.Add("display", "block"); 
                    break;
            }
            Btn_Fecha_Inicio.Enabled = !Habilitado;
            Btn_Fecha_Final.Enabled = !Habilitado;
            Txt_No_Folio.Enabled = !Habilitado;
            Cmb_Unidad_Responsable_Busqueda.Enabled=!Habilitado;
            Txt_Fecha_Inicio.Enabled = false;
            Txt_Fecha_Final.Enabled = false;
            Txt_Beneficiario.Enabled=Habilitado;
            Btn_Buscar_Reserva.Enabled = !Habilitado;
            Txt_No_Reserva.ReadOnly = true;
            Txt_Fecha.Enabled = false;
            Cmb_Unidad_Responsable.Enabled = false;
            Cmb_Fuente_Financiamiento.Enabled = Habilitado;
            Cmb_Programa.Enabled = Habilitado;
            Cmb_Partida.Enabled = false;
            Txt_Importe.Enabled = Habilitado;
            Txt_Conceptos.Enabled = Habilitado;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Seleccionar_Requisicion_Click
    ///DESCRIPCIÓN: Metodo para consultar la reserva
    ///CREO: Sergio Manuel Gallardo Andrade
    ///FECHA_CREO: 17/noviembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Seleccionar_Reserva_Click(object sender, ImageClickEventArgs e)
    {
        String No_Reserva = ((ImageButton)sender).CommandArgument;
        Evento_Grid_Requisiciones_Seleccionar(No_Reserva);
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Evento_Grid_Requisiciones_Seleccionar
    ///DESCRIPCIÓN:
    ///CREO: Sergio Manuel Gallardo Andrade
    ///FECHA_CREO: 17/Noviembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Evento_Grid_Requisiciones_Seleccionar(String Dato)
    {
        Habilitar_Controles("Inicial");
        Btn_Modificar.Visible = true;
        Btn_Modificar.ToolTip = "Cancelar reserva";
        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
        Txt_Conceptos.Visible = true;
        Txt_Saldo.Enabled = false;
        tr_Saldo.Style.Add("display", "block"); 
        Lbl_disponible.Text = "";
        Llenar_Combos_Generales();
        Div_Reserva_Datos.Style.Add("display", "block");
        Div_Reservas_Presentacion.Style.Add("display", "none");       
        Cls_Cat_Dependencias_Negocio Dependencia_Negocio = new Cls_Cat_Dependencias_Negocio();
        Cls_Ope_Con_Reservas_Negocio Reserva_Negocio = new Cls_Ope_Con_Reservas_Negocio();
        String No_Reserva = Dato;//Grid_Requisiciones.SelectedDataKey["No_Requisicion"].ToString();
        DataRow[] Reserva = ((DataTable)Session[P_Dt_Reservas]).Select("NO_RESERVA = '" + No_Reserva + "'");
        Txt_No_Reserva.Text = Reserva[0]["NO_RESERVA"].ToString();
        String Fecha = Reserva[0]["FECHA"].ToString();
        Fecha = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Fecha));
        Txt_Fecha.Text = Fecha.ToUpper();
        //Seleccionar combo dependencia
        Cmb_Unidad_Responsable.SelectedValue =
            Reserva[0]["DEPENDENCIA_ID"].ToString().Trim();
        Txt_Conceptos.Text =
            Reserva[0]["CONCEPTO"].ToString();
        //Total de la requisición
        Txt_Importe.Text =
            Reserva[0]["IMPORTE_INICIAL"].ToString();
        //Se llena el combo partidas
        Cmb_Programa.SelectedValue =
            Reserva[0]["PROYECTO_PROGRAMA_ID"].ToString();
        Cmb_Partida.Items.Clear();
        Cls_Ope_Con_Reservas_Negocio Reservas_Negocio = new Cls_Ope_Con_Reservas_Negocio();
        Reservas_Negocio.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue;
        Reservas_Negocio.P_Proyecto_Programa_ID = Cmb_Programa.SelectedValue;
        DataTable Data_Table = Reservas_Negocio.Consultar_Partidas_De_Un_Programa();
        Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Partida, Data_Table, 1, 0);
        Cmb_Partida.SelectedIndex = 0;
        Cmb_Partida.SelectedValue =
            Reserva[0]["PARTIDA_ID"].ToString();
        Cmb_Fuente_Financiamiento.SelectedValue =
            Reserva[0]["FTE_FINANCIAMIENTO_ID"].ToString();
       Txt_Beneficiario.Text =
            Reserva[0]["BENEFICIARIO"].ToString();
        Txt_Saldo.Text= 
           String.Format("{0:c}", Reserva[0]["SALDO"].ToString());
        //DataTable  Dt_Presupuesto;
        // Dt_Presupuesto = Cls_Ope_Psp_Manejo_Presupuesto.Consultar_Presupuesto_Aprobado(Cmb_Unidad_Responsable.SelectedValue.Trim(), Cmb_Fuente_Financiamiento.SelectedValue.Trim(), Cmb_Programa.SelectedValue.Trim(), "", Cmb_Partida.SelectedValue.Trim(), Convert.ToInt16(DateTime.Now.Year));
        // if (Dt_Presupuesto != null && Dt_Presupuesto.Rows.Count > 0)
        // {
        //     DataRow Renglon = Dt_Presupuesto.Rows[0];
        //     //Lbl_Partida.Text = Renglon["NOMBRE"].ToString();
        //     //Lbl_Clave.Text = Renglon["CLAVE"].ToString();
        //     Lbl_disponible.Style.Add("color", "#990000");
        //     String Mto_Disponible = String.Format("{0:n}", double.Parse(Renglon["DISPONIBLE"].ToString()));
        //     Lbl_disponible.Text = "Disponible $ " + Mto_Disponible;
        //     Session["Importe"] = Mto_Disponible;
        // }

    }
    ///*******************************************************************************
    // NOMBRE DE LA FUNCIÓN: Llenar_Combos_Generales()
    // DESCRIPCIÓN: Llena los combos principales de la interfaz de usuario
    // RETORNA: 
    // CREO: Sergio Manuel Gallardo Andrade
    // FECHA_CREO: 17/Noviembre/2011 
    // MODIFICO:
    // FECHA_MODIFICO:
    // CAUSA_MODIFICACIÓN:
    //********************************************************************************/
    public void Llenar_Combos_Generales()
    {
        Cmb_Unidad_Responsable.SelectedValue =Cmb_Unidad_Responsable_Busqueda.SelectedValue;//Cls_Sessiones.Dependencia_ID_Empleado.ToString();
        Cmb_Programa.Items.Clear();
        Cmb_Fuente_Financiamiento.Items.Clear();
        Cls_Ope_Con_Reservas_Negocio Reserva_Negocio = new Cls_Ope_Con_Reservas_Negocio();
        Reserva_Negocio.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue;//Cls_Sessiones.Dependencia_ID_Empleado.ToString();
        DataTable Dt_Fte_Financiamiento = Reserva_Negocio.Consultar_Fuentes_Financiamiento();
        Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Fuente_Financiamiento, Dt_Fte_Financiamiento, 1, 0);

        DataTable Data_Table_Proyectos = Reserva_Negocio.Consultar_Proyectos_Programas();
        Session[P_Dt_Programas] = Data_Table_Proyectos;
        Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Programa, Data_Table_Proyectos, 1, 0);

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Programa_SelectedIndexChanged
    ///DESCRIPCIÓN: Metodo que llena el combo de partidas dependiendo del programa seleccionado 
    ///CREO: Sergio Manuel Gallardo Andrade 
    ///FECHA_CREO: 17/Noviembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Programa_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cmb_Partida.Enabled = true;
        if (Cmb_Programa.SelectedIndex == 0)
        {
            Mostrar_Informacion("Seleccione un Programa", true);
        }
        else
        {
            Cmb_Partida.Items.Clear();
            Cls_Ope_Con_Reservas_Negocio Reservas_Negocio = new Cls_Ope_Con_Reservas_Negocio();
            Reservas_Negocio.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue;
            Reservas_Negocio.P_Proyecto_Programa_ID = Cmb_Programa.SelectedValue;
            DataTable Data_Table = Reservas_Negocio.Consultar_Partidas_De_Un_Programa();
            Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Partida, Data_Table, 1, 0);
            Cmb_Partida.SelectedIndex = 0;
        }
    }
    private void Mostrar_Informacion(String txt, Boolean mostrar)
    {
        Lbl_Mensaje_Error.Style.Add("color", "#990000");
        Lbl_Mensaje_Error.Visible = mostrar;
        Img_Error.Visible = mostrar;
        Lbl_Mensaje_Error.Text = txt;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Partida_SelectedIndexChanged
    ///DESCRIPCIÓN: Funcion que sllena las partidas dependiendo d
    ///en la busqueda del Modalpopup
    ///CREO: Gustavo Angeles
    ///FECHA_CREO: 9/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Partida_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["Importe"] = 0;
        //Div_Presupuesto.Visible = true;
        DataTable Dt_Presupuesto = null;
        DataRow[] Partidas = null;
        //Verificar si Dt_PArtidas ya contiene la partida seleccionada
        if (Session[P_Dt_Partidas] != null && ((DataTable)Session[P_Dt_Partidas]).Rows.Count > 0)
        {
            Partidas = ((DataTable)Session[P_Dt_Partidas]).Select("Partida_ID = '" + Cmb_Partida.SelectedValue.Trim() + "'");
        }

        if (Partidas != null && Partidas.Length > 0)
        {
            DataRow Renglon = Partidas[0];
            String Mto_Disponible = String.Format("{0:n}", double.Parse(Renglon["MONTO_DISPONIBLE"].ToString()));

          //  Lbl_Disponible_Partida.Text = " $ " + Mto_Disponible;
            String fecha = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Renglon["FECHA_CREO"].ToString()));
          //  Lbl_Fecha_Asignacion.Text = fecha;
        }
        else
        {

            Dt_Presupuesto = Cls_Ope_Psp_Manejo_Presupuesto.Consultar_Presupuesto_Aprobado(Cmb_Unidad_Responsable.SelectedValue.Trim(), Cmb_Fuente_Financiamiento.SelectedValue.Trim(), Cmb_Programa.SelectedValue.Trim(), "", Cmb_Partida.SelectedValue.Trim(), Convert.ToInt16(DateTime.Now.Year));
            if (Dt_Presupuesto != null && Dt_Presupuesto.Rows.Count > 0)
            {
                DataRow Renglon = Dt_Presupuesto.Rows[0];
                //Lbl_Partida.Text = Renglon["NOMBRE"].ToString();
                //Lbl_Clave.Text = Renglon["CLAVE"].ToString();
                Lbl_disponible.Style.Add("color", "#990000");
                String Mto_Disponible = String.Format("{0:n}", double.Parse(Renglon["DISPONIBLE"].ToString()));
                Lbl_disponible.Text = "Disponible $ " + Mto_Disponible;
                Txt_Importe.Text = "";
                Txt_Importe.Enabled = true;
                Session["Importe"] =Mto_Disponible;
            }
            else
            {
                Lbl_disponible.Style.Add("color", "#990000");
                Lbl_disponible.Text = "Disponible $ 0.00";
                Txt_Importe.Text = "";
                Session["Importe"] =0;
                Txt_Importe.Enabled = false;
            }
        }
    }
    ///*******************************************************************************
    // NOMBRE DE LA FUNCIÓN: Validaciones
    // DESCRIPCIÓN: Genera el String con la informacion que falta y ejecuta la 
    // operacion solicitada si las validaciones son positivas
    // RETORNA: 
    // CREO: Gustavo Angeles Cruz
    // FECHA_CREO: 24/Agosto/2010 
    // MODIFICO:
    // FECHA_MODIFICO:
    // CAUSA_MODIFICACIÓN:
    //********************************************************************************/
    private Boolean Validaciones(bool Validar_Completo)
    {
        Boolean Bln_Bandera;
        Bln_Bandera = true;
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
        //Verifica que campos esten seleccionados o tengan valor valor
        if (Cmb_Fuente_Financiamiento.SelectedIndex == 0)
        {
            Lbl_Mensaje_Error.Text += " Seleccionar La Fuente de Financiamiento. <br>";
            Bln_Bandera = false;
        }
        if (Cmb_Programa.SelectedIndex == 0 )
        {
            Lbl_Mensaje_Error.Text += " Seleccionar El Programa. <br>";
            Bln_Bandera = false;
        }
        if (Cmb_Unidad_Responsable.SelectedIndex == 0)
        {
            Lbl_Mensaje_Error.Text += " Seleccionar La Unidad Responsable <br>";
            Bln_Bandera = false;
        }
        if (Txt_Importe.Text == "")
        {
            Lbl_Mensaje_Error.Text += " Ingresa Una Cantidad en el importe <br>";
            Bln_Bandera = false;
        }
            if (Convert.ToDouble(Txt_Importe.Text) > Convert.ToDouble(Session["Importe"]))
            {
                Lbl_Mensaje_Error.Text += " El Importe no Puede ser Mayor al Disponible <br>";
                Bln_Bandera = false;
            }
        if (Txt_Conceptos.Text.Length >= 250)
        {            
            Lbl_Mensaje_Error.Text += " El concepto solo permite un maximo de 250 caracteres. <br>";
            Bln_Bandera = false;
        }
        if (!Bln_Bandera)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true; 
        }
        return Bln_Bandera;
    }
    #endregion

    #region (Metodos de Operacion [Alta - Modificar - Eliminar])
   ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Reserva
    /// DESCRIPCION : Modifica la reserva con los datos proporcionados por el usuario
    /// PARAMETROS  : 
    /// CREO        : Sergio Manuel Gallardo Andrade
    /// FECHA_CREO  : 22/Noviembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Modificar_Reserva()
    {
        Cls_Ope_Con_Reservas_Negocio  Rs_Reserva= new Cls_Ope_Con_Reservas_Negocio(); //Variable de conexion hacia la capa de negocios
        try
        {
            int Registro_Presupuestal;
            int Registro_Movimientos;
            Rs_Reserva.P_No_Reserva =Txt_No_Reserva.Text.Trim();
            //Rs_Reserva.P_Fuente_Financiamiento = Cmb_Fuente_Financiamiento.SelectedValue;
            //Rs_Reserva.P_Proyecto_Programa_ID = Cmb_Programa.SelectedValue;
            //Rs_Reserva.P_Partida_ID = Cmb_Partida.SelectedValue;
            Rs_Reserva.P_Estatus ="CANCELADA";
            //Rs_Reserva.P_Beneficiario = Txt_Beneficiario.Text.Trim();
            Rs_Reserva.P_Importe = Txt_Importe.Text;
            Rs_Reserva.P_Usuario_Modifico = Cls_Sessiones.Nombre_Empleado;
            Rs_Reserva.Modificar_Reserva(); //Modifica el registro en base a los datos proporcionados
            Registro_Presupuestal = Cls_Ope_Psp_Manejo_Presupuesto.Actualizar_Momentos_Presupuestales(Txt_No_Reserva.Text.Trim(), "DISPONIBLE", "COMPROMETIDO", Convert.ToDouble(Txt_Saldo.Text));
            Registro_Movimientos = Cls_Ope_Psp_Manejo_Presupuesto.Registro_Movimiento_Presupuestal(Txt_No_Reserva.Text.Trim(), "DISPONIBLE", "COMPROMETIDO", Convert.ToDouble(Txt_Saldo.Text), "", "", "", "");
            Limpia_Controles(); //Limpia los controles del modulo
            Inicializa_Controles();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "RESERVAS", "alert('La Cancelacion de la reserva fue exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar_Cheque " + ex.Message.ToString(), ex);
        }
    }
    #endregion
    #endregion


    #region (Eventos)
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            int No_reserva;
            int Registro_Afectado;
            int Registro_Movimiento;
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (Btn_Nuevo.ToolTip == "Nuevo")
            {
                Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                Limpia_Controles();           //Limpia los controles de la forma para poder introducir nuevos datos
                Llenar_Combos_Generales();
            }
            else
            {
                //Valida los datos ingresados por el usuario.
                if (Validaciones(true))
                {
                  // No_reserva=Cls_Ope_Psp_Manejo_Presupuesto.Crear_Reserva(Cmb_Unidad_Responsable.SelectedValue.Trim(), Cmb_Fuente_Financiamiento.SelectedValue.Trim(), Cmb_Programa.SelectedValue.Trim(), Cmb_Partida.SelectedValue.Trim(), Txt_Conceptos.Text, Convert.ToString(DateTime.Now.Year), Convert.ToDouble(Txt_Importe.Text), Txt_Beneficiario.Text);
                   //Registro_Afectado=Cls_Ope_Psp_Manejo_Presupuesto.Actualizar_Momentos_Presupuestales(Convert.ToString(No_reserva),"COMPROMETIDO","DISPONIBLE",Convert.ToDouble(Txt_Importe.Text));
                   //Registro_Movimiento = Cls_Ope_Psp_Manejo_Presupuesto.Registro_Movimiento_Presupuestal(Convert.ToString(No_reserva), "COMPROMETIDO", "DISPONIBLE", Convert.ToDouble(Txt_Importe.Text), "", "", "", "");
                   //ScriptManager.RegisterStartupScript(this, this.GetType(), "Reservas", "alert('El alta de la reserva No."+No_reserva+"  fue exitosa');", true);
                   //Inicializa_Controles();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
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
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Metodo que permite modificar la reserva
    ///CREO: Sergio Manuel Gallardo Andrade
    ///FECHA_CREO: 17/Noviembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
     {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Lbl_Mensaje_Error.Text = "";
                if (Btn_Modificar.ToolTip == "Modificar")
                {
                        Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                }
                else
                {
                       Modificar_Reserva(); //Modifica los datos de la Cuenta Contable con los datos proporcionados por el usuario
                }
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
       ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Reserva_Click
    ///DESCRIPCIÓN: Metodo que permite buscar la reserva
    ///CREO: Gustavo Angeles
    ///FECHA_CREO: 9/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Reserva_Click(object sender, ImageClickEventArgs e)
    {
        Habilitar_Controles("Inicial");
        Llenar_Grid_Reservas();
    }
    
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip == "Salir")
            {                
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Inicializa_Controles();//Habilita los controles para la siguiente operación del usuario en el catálogo
                Limpia_Controles();//Limpia los controles de la forma
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    #endregion
    #region (Grid)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN: Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///en la busqueda del Modalpopup
    ///CREO: Gustavo Angeles
    ///FECHA_CREO: 9/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Grid_Reservas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Reservas.DataSource = ((DataTable)Session[P_Dt_Reservas]);
        Grid_Reservas.PageIndex = e.NewPageIndex;
        Grid_Reservas.DataBind();
    }
    #endregion
}