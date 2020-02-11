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
using Presidencia.Administrar_Requisiciones.Negocios;
using Presidencia.Sessiones;
using Presidencia.Bitacora_Eventos;
using Presidencia.Constantes;
using System.Collections.Generic;
using Presidencia.Generar_Requisicion.Negocio;
public partial class paginas_Compras_Frm_Ope_Com_Administrar_Requisiciones_Especiales : System.Web.UI.Page
{
   
    #region Variables
    //Objeto de la clase de negocio
    Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio;
    //Variable que permitira validar el Estatus Inicial de la requisicion seleccionada
    private static String Estatus_Inicial;
    #endregion

    #region Page Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo que se ejecuta cuando se carga la pagina
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 8/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
        if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
        if (!IsPostBack)
        {
            ViewState["SortDirection"] = "ASC";
            Requisicion_Negocio = new Cls_Ope_Com_Administrar_Requisiciones_Negocio();
            
            Estado_Formulario("inicial");
            Requisicion_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
            Requisicion_Negocio.P_Empleado_ID = Cls_Sessiones.No_Empleado;
            Limpiar_Componentes();
            
            Carga_Componentes_Busqueda();
            Chk_Dependencia.Checked = true;
            Chk_Dependencia.Enabled = false;
            Cmb_Dependencia.SelectedValue = Cls_Sessiones.Dependencia_ID_Empleado;
            //Cmb_Dependencia.Enabled = false;
            if (Cmb_Dependencia.SelectedIndex != 0)
                Requisicion_Negocio.P_Dependencia_ID = Cmb_Dependencia.SelectedValue; //Cls_Sessiones.Dependencia_ID_Empleado;
            else
                Requisicion_Negocio.P_Dependencia_ID = "";
            Llenar_Grid_Requisiciones(Requisicion_Negocio);
            Cmb_Dependencia.Enabled = true;

            //Verificar si su rol es jefe de dependencia, admin de modulo o admin de sistema
            //DataTable Dt_Grupo_Rol = Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Rol_ID.ToString());
            //if (Dt_Grupo_Rol != null)
            //{
            //    String Grupo_Rol = Dt_Grupo_Rol.Rows[0][Apl_Cat_Roles.Campo_Grupo_Roles_ID].ToString();
            //    if (Grupo_Rol == "00001" || Grupo_Rol == "00002")
            //    {
            //        Cmb_Dependencia.Enabled = true;
            //    }
            //    else
            //    {
            //        DataTable Dt_URs = Cls_Util.Consultar_URs_De_Empleado(Cls_Sessiones.Empleado_ID);
            //        if (Dt_URs.Rows.Count > 1)
            //        {
            //            Cmb_Dependencia.Enabled = true;
            //            Cls_Util.Llenar_Combo_Con_DataTable_Generico
            //                (Cmb_Dependencia, Dt_URs, 1, 0);
            //            Cmb_Dependencia.SelectedValue = Cls_Sessiones.Dependencia_ID_Empleado;
            //        }
            //    }
            //}

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
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    protected void Page_Init(object sender, EventArgs e)
    {
       
    }


    #endregion

    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Componentes
    ///DESCRIPCIÓN: Metodo que limpia los componentes del catalogo
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public void Limpiar_Componentes()
    {
        Txt_Busqueda.Text = "";
        Txt_Dependencia.Text = "";
        Txt_Tipo.Text = "";
        Txt_Folio.Text = "";
        Txt_Fecha_Generacion.Text = "";              
        Txt_Total.Text = "";
        Chk_Verificacion.Checked = false;
        Txt_Tipo_Articulo.Text = "";
        Llenar_Combo();
        Txt_Justificacion.Text = "";
        Txt_Especificacion.Text = ""; 
        Div_Productos.Visible = false;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo
    ///DESCRIPCIÓN: Metodo que llena el combo Cmb_Estatus
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 14/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo()
    {
        Cmb_Estatus.Items.Clear();
        Cmb_Estatus.Items.Add("<<SELECCIONAR>>");
        Cmb_Estatus.Items.Add("AUTORIZADA");
        Cmb_Estatus.Items.Add("RECHAZADA");
        Cmb_Estatus.Items.Add("CANCELADA");
        Cmb_Estatus.Items[0].Value = "0";
        Cmb_Estatus.Items[0].Selected = true;
        
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo
    ///DESCRIPCIÓN: Metodo que llena el combo Cmb_Estatus
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 14/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Estatus_Cotizado()
    {
        Cmb_Estatus.Items.Clear();
        Cmb_Estatus.Items.Add("<<SELECCIONAR>>");
        Cmb_Estatus.Items.Add("CONFIRMADA");
        Cmb_Estatus.Items.Add("COTIZADA-RECHAZADA");
        Cmb_Estatus.Items[0].Value = "0";
        Cmb_Estatus.Items[0].Selected = true;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Estado_Formulario
    ///DESCRIPCIÓN: Metodo que indica el estado de los botones del formulario
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Estado_Formulario(String Estado)
    {
        switch (Estado)
        {
            case "inicial":
                //Boton Salir
                Btn_Salir.ToolTip = "Inicio";
                Btn_Salir.Enabled = true;
                Btn_Salir.Visible = true;
                Div_Busqueda.Visible = true;
                Div_Requisiciones.Visible = true;
                Div_Productos.Visible = false;
                Div_Productos_Cotizados.Visible = false;
                Div_Comentarios.Visible = false;
                Div_Busqueda_Avanzada.Visible = true;
                Configuracion_Acceso("Frm_Ope_Com_Administrar_Requisiciones_Especiales.aspx");
                Configuracion_Acceso_LinkButton("Frm_Ope_Com_Administrar_Requisiciones_Especiales.aspx");
                
                break;
            case "modificar":
                ////Boton Salir
                Btn_Salir.ToolTip = "Listado";
                Btn_Salir.Enabled = true;
                Btn_Salir.Visible = true;
                Div_Busqueda.Visible = false;
                Div_Requisiciones.Visible = false;
                Div_Productos.Visible = true;
                Div_Comentarios.Visible = true;
                Div_Busqueda_Avanzada.Visible = false;

                break;
        }
    }

    public void Limpiar_Busqueda_avanzada()
    {
        Txt_Fecha_Inicial.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        Txt_Fecha_Final.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        Btn_Fecha_Inicio.Enabled = false;
        Btn_Fecha_Fin.Enabled = false;
        //Ponemos por default la dependencia a la que pertenece el usuario
        Chk_Dependencia.Checked = false;
        Chk_Dependencia.Enabled = true;
        Cmb_Dependencia.Enabled = false;
        Chk_Estatus.Checked = false;
        Cmb_Estatus_Busqueda.Enabled = false;
        Cmb_Estatus_Busqueda.SelectedIndex = 0;
        Chk_Fecha.Checked = false;
        Txt_Fecha_Inicial.Enabled = false;
        Txt_Fecha_Final.Enabled = false;

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Estatus 
    ///DESCRIPCIÓN: Metodo que valida que seleccione un estatus de la requisicion seleccionada
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public void Validar_Estatus()
    {
       if (Cmb_Estatus.SelectedIndex != 0 )
       {
           if (Cmb_Estatus.SelectedValue == "RECHAZADA")
           {
               Requisicion_Negocio.P_Estatus = "EN CONSTRUCCION";
           } 
           else
            Requisicion_Negocio.P_Estatus = Cmb_Estatus.SelectedValue;
       }
       else
       {
             Lbl_Mensaje_Error.Text += "+ Debe seleccionar un Estatus <br />";
             Div_Contenedor_Msj_Error.Visible = true;
             Txt_Comentario.Text = "";
       }
       if (Cmb_Estatus.SelectedValue != Estatus_Inicial)
       {
           Requisicion_Negocio.P_Estatus = Cmb_Estatus.SelectedValue;
       }
       else
       {
           Lbl_Mensaje_Error.Text += "+ Debe seleccionar un Estatus diferente a " +Estatus_Inicial+ "<br />";
           Div_Contenedor_Msj_Error.Visible = true;
           Txt_Comentario.Text = "";
       }
    }

        #region Metodos ModalPopUp

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
            Cmb_Estatus_Busqueda.Items.Add("GENERADA");
            Cmb_Estatus_Busqueda.Items.Add("AUTORIZADA");
            Cmb_Estatus_Busqueda.Items.Add("CANCELADA");
            Cmb_Estatus_Busqueda.Items.Add("COTIZADA");
            Cmb_Estatus_Busqueda.Items[0].Value = "0";
            Cmb_Estatus_Busqueda.Items[0].Selected = true;
        }
    }

  

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Areas
    ///DESCRIPCIÓN: Metodo que llena el combo Cmb_Areas que esta dentro del ModalPopup
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Dependencias()
    {
        Cmb_Dependencia.Items.Clear();
        DataTable Data_Table = Requisicion_Negocio.Consulta_Dependencias_Con_Programas_Especiales_yRamo33();
        Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Dependencia, Data_Table);
        Cmb_Dependencia.SelectedIndex =0;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Areas
    ///DESCRIPCIÓN: Metodo que llena el combo Cmb_Areas que esta dentro del ModalPopup
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Areas()
    {
        //Cmb_Area.Items.Clear();
        //DataTable Data_Table = Requisicion_Negocio.Consulta_Areas();
        //Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Area, Data_Table);
        //Cmb_Area.SelectedIndex = 0;
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
    public Cls_Ope_Com_Administrar_Requisiciones_Negocio Verificar_Fecha(Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio)
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
                    Requisicion_Negocio.P_Fecha_Inicial = Formato_Fecha(Txt_Fecha_Inicial.Text);
                    Requisicion_Negocio.P_Fecha_Final = Formato_Fecha(Txt_Fecha_Final.Text);
                    Session["Descripcion"] = Session["Descripcion"].ToString() + ", De la Fecha " + Txt_Fecha_Inicial.Text + " a " + Txt_Fecha_Final.Text;
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
        }
        return Requisicion_Negocio;
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

    public Cls_Ope_Com_Administrar_Requisiciones_Negocio Validar_Estatus_Busqueda(Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio)
    {
        
        if (Chk_Estatus.Checked == true)
        {
            if (Cmb_Estatus_Busqueda.SelectedIndex != 0)
            {
                Requisicion_Negocio.P_Estatus_Busqueda = Cmb_Estatus_Busqueda.SelectedValue;
                Session["Descripcion"] = Session["Descripcion"].ToString() + ", Estatus =" + Cmb_Estatus_Busqueda.SelectedValue;
            }
            else
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text += "+ Debe seleccionar un estatus <br />";
            }

        }
        else
        {
            Requisicion_Negocio.P_Estatus_Busqueda = null;
        }
     return Requisicion_Negocio;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Area
    ///DESCRIPCIÓN: Metodo que valida que seleccione un area dentro del modalpopup
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 27/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public Cls_Ope_Com_Administrar_Requisiciones_Negocio Validar_Dependencia(Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio)
    {
        
        if (Chk_Dependencia.Checked == true)
        {
            if (Cmb_Dependencia.SelectedIndex != 0)
            {
                Requisicion_Negocio.P_Dependencia_ID = Cmb_Dependencia.SelectedValue;
                Session["Descripcion"] = Session["Descripcion"].ToString() + ", Dependencia =" + Cmb_Dependencia.SelectedItem;
            }
            else
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + "+ Debe seleccionar una Dependencia <br />";
            }
        }
        else
        {
            Requisicion_Negocio.P_Area_ID = null;
        }
        return Requisicion_Negocio;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Area
    ///DESCRIPCIÓN: Metodo que valida que seleccione un area dentro del modalpopup
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 27/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public void Validar_Area()
    {
        
        if (Chk_Area.Checked == true)
        {
            if (Cmb_Area.SelectedIndex != 0)
            {
                Requisicion_Negocio.P_Area_ID = Cmb_Area.SelectedValue;
                Session["Descripcion"] = Session["Descripcion"].ToString() + ", Area =" + Cmb_Area.SelectedItem;
            }
            else
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + "+ Debe seleccionar un area <br />";
            }
        }
        else
        {
            Requisicion_Negocio.P_Area_ID = null;
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

        Requisicion_Negocio = new Cls_Ope_Com_Administrar_Requisiciones_Negocio();

        Txt_Fecha_Inicial.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        Txt_Fecha_Final.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        Btn_Fecha_Inicio.Enabled = false;
        Btn_Fecha_Fin.Enabled = false;
        Llenar_Combo_Estatus_Busqueda();
        Llenar_Combo_Dependencias();
        //Ponemos por default la dependencia a la que pertenece el usuario
        Chk_Dependencia.Checked = false;
        Chk_Dependencia.Enabled = true;
        Cmb_Dependencia.Enabled = false;
        Chk_Estatus.Checked = false;
        Cmb_Estatus_Busqueda.Enabled = false;
        Cmb_Estatus_Busqueda.SelectedIndex = 0;
        Chk_Fecha.Checked = false;
        Txt_Fecha_Inicial.Enabled = false;
        Txt_Fecha_Final.Enabled = false;
        //limpiamos la clase de Negocio
        Requisicion_Negocio.P_Estatus_Busqueda = null;
        Requisicion_Negocio.P_Fecha_Inicial = null;
        Requisicion_Negocio.P_Fecha_Final = null;
        Requisicion_Negocio.P_Dependencia_ID = null;
        Requisicion_Negocio.P_Area_ID = null;
        //llenamos el combo de dependencia
        Llenar_Combo_Areas();
    }
    #endregion

        #region Observaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Div_Comentarios
    ///DESCRIPCIÓN: Metodo que carga e inicializa los componentes del ModalPopUp
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public void Cargar_Div_Comentarios(bool Visible)
    {
        Div_Comentarios.Visible = Visible;


        if (Visible == true)
        {
            Llenar_Grid_Comentarios();
            //Validaciones 
            Btn_Alta_Observacion.Visible = true;
            Btn_Alta_Observacion.ToolTip = "Evaluar";
            Btn_Alta_Observacion.ImageUrl = "~/paginas/imagenes/paginas/accept.png";
            Btn_Alta_Observacion.Enabled = true;
            Btn_Cancelar_Observacion.Visible = false;
            Txt_Comentario.Text = "";
            Txt_Comentario.Enabled = false;
        }
        else
        {
            Session["Ds_Comentarios"] = null;
            Grid_Comentarios.DataSource = new DataTable();
            Grid_Comentarios.DataBind();
        }
    }

    #endregion
    
    #endregion 

    #region Grid

        #region Grid Requisiciones
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Requisiciones_PageIndexChanging
    ///DESCRIPCIÓN: Metodo que permite la paginacion del Grid_Requisicion
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Requisiciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Requisiciones.PageIndex = e.NewPageIndex;
        DataSet Ds_Requisiciones = (DataSet)Session["Ds_Requisiciones"];
        Grid_Requisiciones.DataSource = Ds_Requisiciones;
        Grid_Requisiciones.DataBind();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Requisiciones_SelectedIndexChanged
    ///DESCRIPCIÓN: Metodo que permite seleccionar un registro dentro de un grid
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Requisiciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        Requisicion_Negocio = new Cls_Ope_Com_Administrar_Requisiciones_Negocio();
        //Validaciones de los botones
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        //Cambiamos el estado del formulario
        Estado_Formulario("modificar");
        //GridViewRow representa una fila individual de un control gridview
        GridViewRow selectedRow = Grid_Requisiciones.Rows[Grid_Requisiciones.SelectedIndex];
        String Requisicion_Seleccionada = Convert.ToString(selectedRow.Cells[1].Text.Trim());
        Requisicion_Negocio.P_Folio = Requisicion_Seleccionada;
        Requisicion_Negocio.P_Req_Especiales = "SI";
        DataSet Dato_Requisicion = Requisicion_Negocio.Consulta_Requisiciones();
        //Cargamos los valores con los datos de la requisicion seleccionada en las cajas de texto
        Txt_Dependencia.Text = Dato_Requisicion.Tables[0].Rows[0].ItemArray[0].ToString();
        Txt_Tipo.Text = Dato_Requisicion.Tables[0].Rows[0].ItemArray[1].ToString();
        Txt_Folio.Text = Dato_Requisicion.Tables[0].Rows[0].ItemArray[2].ToString();
        Txt_Fecha_Generacion.Text =  Dato_Requisicion.Tables[0].Rows[0].ItemArray[3].ToString();
        //llenado del Combo
        Estatus_Inicial = Dato_Requisicion.Tables[0].Rows[0].ItemArray[4].ToString();
        if (Estatus_Inicial == "COTIZADA")
        {
            //Llenamos el grid con los datos que cotizo el proveedor 
            Session["Requisicion_ID"] = Dato_Requisicion.Tables[0].Rows[0].ItemArray[9].ToString().Trim();
            Requisicion_Negocio.P_Requisicion_ID = Session["Requisicion_ID"].ToString();
            Requisicion_Negocio.P_Tipo_Articulo = Dato_Requisicion.Tables[0].Rows[0].ItemArray[13].ToString().Trim();
            //Llenamos el Grid de Productos Cotizados
            DataTable Dt_Cotizados = Requisicion_Negocio.Consulta_Productos_Cotizados();
            Session["Dt_Cotizados"] = Dt_Cotizados;
            Gri_Productos_Cotizados.DataSource = Dt_Cotizados;
            Gri_Productos_Cotizados.DataBind();
            //Cargamos el combo de estatus pero solo con los estatus de Confirmada, Cotizada-Rechazada
            Llenar_Estatus_Cotizado();
            Div_Productos_Cotizados.Visible = true;
            Div_Productos.Visible = true;
            //Asignamos el total Cotizado
            Txt_Total_Cotizado.Text = Dato_Requisicion.Tables[0].Rows[0].ItemArray[14].ToString();
        }
        else
        {
            Llenar_Combo();
            switch (Estatus_Inicial)
            {
                case "GENERADA":
                    Cmb_Estatus.SelectedIndex = 0;
                    break;
                case "AUTORIZADA":
                    Cmb_Estatus.SelectedIndex = 1;
                    break;
                case "RECHAZADA":
                    Cmb_Estatus.SelectedIndex = 2;
                    break;
                case "CANCELADA":
                    Cmb_Estatus.SelectedIndex = 3;
                    break;
            }
            
        }
        //Txt_Subtotal.Text = Dato_Requisicion.Tables[0].Rows[0].ItemArray[6].ToString();
        //Txt_IEPS.Text = Dato_Requisicion.Tables[0].Rows[0].ItemArray[7].ToString();
        //Txt_IVA.Text = Dato_Requisicion.Tables[0].Rows[0].ItemArray[8].ToString();
        Txt_Total.Text = Dato_Requisicion.Tables[0].Rows[0].ItemArray[8].ToString();
        Requisicion_Negocio.P_Requisicion_ID = Dato_Requisicion.Tables[0].Rows[0].ItemArray[9].ToString();
        Session["Requisicion_ID"] = Dato_Requisicion.Tables[0].Rows[0].ItemArray[9].ToString();
        Txt_Justificacion.Text = Dato_Requisicion.Tables[0].Rows[0].ItemArray[10].ToString();
        Txt_Especificacion.Text = Dato_Requisicion.Tables[0].Rows[0].ItemArray[11].ToString();
        //llenamos el grid de Productos
        Llenar_Grid_Productos();
        Div_Productos.Visible = true;
        //Seleccionamos el Check de Verificar
        if (Dato_Requisicion.Tables[0].Rows[0].ItemArray[12].ToString() == "SI")
        {
            Chk_Verificacion.Checked = true;
        }
        else
        {
            Chk_Verificacion.Checked = false;
        }
        Txt_Tipo_Articulo.Text = Dato_Requisicion.Tables[0].Rows[0].ItemArray[13].ToString();
        //Habilitamos el boton de Modificar
        //Llenamos el grid de productos de acuerdo a la requisicion seleccionada        
        Cargar_Div_Comentarios(true);
        Requisicion_Negocio.P_Folio = null;

        Configuracion_Acceso("Frm_Ope_Com_Administrar_Requisiciones_Especiales.aspx");
        Configuracion_Acceso_LinkButton("Frm_Ope_Com_Administrar_Requisiciones_Especiales.aspx");
    }

    protected void Grid_Requisiciones_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataSet Ds = (DataSet)Session["Ds_Requisiciones"];
        DataTable Dt_Requisiciones = Ds.Tables[0];

      

        if (Dt_Requisiciones != null)
        {
            DataView Dv_Requisiciones = new DataView(Dt_Requisiciones);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Requisiciones.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Requisiciones.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Requisiciones.DataSource = Dv_Requisiciones;
            Grid_Requisiciones.DataBind();
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Requisiciones
    ///DESCRIPCIÓN: Metodo que permite llenar el Grid_Requisiciones
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Grid_Requisiciones(Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Datos)
    {
        Requisicion_Datos.P_Req_Especiales = "SI";
        DataSet Data_Set = Requisicion_Datos.Consulta_Requisiciones();
        Div_Requisiciones.Visible = true;
        Session["Ds_Requisiciones"] = null;
        //dEJAMOS VACIO EL GRID DE REQUISICIONES
        Grid_Requisiciones.DataSource = new DataTable();
        Grid_Requisiciones.DataBind();
        if (Data_Set.Tables[0].Rows.Count != 0)
        {
            Session["Ds_Requisiciones"] = Data_Set;
            Grid_Requisiciones.DataSource = Data_Set;
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

    #endregion

        #region Grid Productos
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Productos
    ///DESCRIPCIÓN: Metodo que permite llenar el Grid_Productos
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Grid_Productos()
    {
        DataSet Data_Set = Requisicion_Negocio.Consulta_Productos();
        if (Data_Set.Tables[0].Rows.Count != 0)
        {
            Div_Productos.Visible = true;
            Session["Dt_Productos"] = Data_Set;
            Grid_Productos.DataSource = Data_Set;
            Grid_Productos.DataBind();
        }
        else
        {
            Div_Requisiciones.Visible = false;
 
        }
    }

    protected void Grid_Productos_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataSet Ds = (DataSet)Session["Dt_Productos"];
        DataTable Dt_Productos = Ds.Tables[0];

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

    protected void Grid_Productos_Cotizados_Sorting(object sender, GridViewSortEventArgs e)
    {

        DataTable Dt_Productos_Cot = (DataTable)Session["Dt_Cotizados"];

        if (Dt_Productos_Cot != null)
        {
            DataView Dv_Productos_Cot = new DataView(Dt_Productos_Cot);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Productos_Cot.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Productos_Cot.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Gri_Productos_Cotizados.DataSource = Dv_Productos_Cot;
            Gri_Productos_Cotizados.DataBind();
        }

    }


    #endregion



        #region Grid Observaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Comentarios_SelectedIndexChanged
    ///DESCRIPCIÓN: Metodo que permite paginar el Grid_Productos
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    
        protected void Grid_Comentarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GridViewRow representa una fila individual de un control gridview
            GridViewRow selectedRow = Grid_Comentarios.Rows[Grid_Comentarios.SelectedIndex];
            //Cargamos los valores con los datos de la requisicion seleccionada en las cajas de texto
            Txt_Comentario.Text = Convert.ToString(selectedRow.Cells[2].Text);
            Txt_Comentario.Enabled = false;
            Llenar_Grid_Comentarios();

        }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Comentarios_SelectedIndexChanged
    ///DESCRIPCIÓN: Metodo que permite seleccionar un registro en el Grid_Comentarios
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
        protected void Grid_Comentarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            
            //Requisicion_Negocio.P_Requisicion_ID = Session["Requisicion_ID"].ToString();
            //Llenar_Grid_Comentarios();
            Grid_Comentarios.PageIndex = e.NewPageIndex;
            Grid_Comentarios.DataSource = (DataSet)Session["Ds_Comentarios"];
            Grid_Comentarios.DataBind();
        }

     ///*******************************************************************************
     ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Comentarios
     ///DESCRIPCIÓN: Metodo que permite seleccionar un registro en el Grid_Comentarios
     ///PARAMETROS:   
     ///CREO: Susana Trigueros Armenta
     ///FECHA_CREO: 10/Noviembre/2010 
     ///MODIFICO:
     ///FECHA_MODIFICO:
     ///CAUSA_MODIFICACIÓN:
     ///*******************************************************************************
        public void Llenar_Grid_Comentarios()
        {
            DataSet Data_Set = Requisicion_Negocio.Consulta_Observaciones();
            Session["Ds_Comentarios"] = Data_Set;
            if (Data_Set.Tables[0].Rows.Count != 0)
            {
                Grid_Comentarios.DataSource = Data_Set;
                Grid_Comentarios.DataBind();
            }
        }
    #endregion

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Evento del boton salir que tiene tambien la funcionalidad de cancelar y salir 
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Requisicion_Negocio = new Cls_Ope_Com_Administrar_Requisiciones_Negocio();
        switch (Btn_Salir.ToolTip)
        {
            
            case "Listado":

                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = false;
                Estado_Formulario("inicial");
                Cmb_Estatus.Enabled = false;
                Cmb_Estatus.SelectedIndex = 0;
                Limpiar_Componentes();
                Llenar_Grid_Requisiciones(Requisicion_Negocio);
                Div_Productos.Visible = false;
                Div_Comentarios.Visible = false;
                Div_Busqueda.Visible = true;
                Requisicion_Negocio = new Cls_Ope_Com_Administrar_Requisiciones_Negocio();
                Llenar_Grid_Requisiciones(Requisicion_Negocio);
                break;
            case "Inicio":
                Limpiar_Componentes();
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                break;
        }//fin del switch
        
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
    ///DESCRIPCIÓN: Evento del boton Buscar
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Session["Descripcion"] = "";
        Requisicion_Negocio = new Cls_Ope_Com_Administrar_Requisiciones_Negocio();
        Cargar_Div_Comentarios(false);
        //Validamos Si esta seleccionada una Fecha y que esta sea valida
        Requisicion_Negocio = Validar_Estatus_Busqueda(Requisicion_Negocio);
        Requisicion_Negocio = Validar_Dependencia(Requisicion_Negocio);
        //Validar_Area();
        Requisicion_Negocio = Verificar_Fecha(Requisicion_Negocio);
        if (Txt_Busqueda.Text.Trim() != String.Empty)
        {
            Requisicion_Negocio.P_Campo_Busqueda = Txt_Busqueda.Text;
        }

        if(Div_Contenedor_Msj_Error.Visible == false)
        {    
            //Asignamos el nombre a buscar a la variable de negocio

            Requisicion_Negocio.P_Req_Especiales = "SI";
            DataSet Data_Set = Requisicion_Negocio.Consulta_Requisiciones();
            Div_Requisiciones.Visible = true;
            
            if (Data_Set.Tables[0].Rows.Count !=0)
            {
                Llenar_Grid_Requisiciones(Requisicion_Negocio);
            }
            else
            {
                Div_Requisiciones.Visible = true;
               // Div_Contenedor_Msj_Error.Visible = true;
               
                //Lbl_Mensaje_Error.Text = "+ No se encontraron Datos";
                Requisicion_Negocio.P_Estatus = null;
                Requisicion_Negocio.P_Folio = null;
                Requisicion_Negocio.P_Campo_Busqueda = null;
                Llenar_Grid_Requisiciones(Requisicion_Negocio);
            }
            Requisicion_Negocio.P_Campo_Busqueda = null;
        }
        Limpiar_Componentes();
    }

    #region Componentes ModalPopUp
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Fecha_CheckedChanged
    ///DESCRIPCIÓN: 
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Fecha_CheckedChanged(object sender, EventArgs e)
    {
        //Modal_Busqueda.Show();
        if (Chk_Fecha.Checked == false)
        {
            Txt_Fecha_Inicial.Enabled = false;
            Txt_Fecha_Final.Enabled = false;
            Btn_Fecha_Inicio.Enabled = false;
            Btn_Fecha_Inicio.Enabled = false;
        }
        else
        {
            Txt_Fecha_Inicial.Enabled = true;
            Txt_Fecha_Final.Enabled = true;
            Btn_Fecha_Inicio.Enabled = true;
            Btn_Fecha_Inicio.Enabled = true;
        }
        
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Estatus_CheckedChanged
    ///DESCRIPCIÓN: Evento del Check Estatus del ModalPOpUP
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Estatus_CheckedChanged(object sender, EventArgs e)
    {
     //   Modal_Busqueda.Show();
        if (Chk_Estatus.Checked == false)
        {
            Cmb_Estatus_Busqueda.Enabled = false;
            Cmb_Estatus_Busqueda.SelectedIndex = 0;
            Txt_Fecha_Inicial.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Txt_Fecha_Final.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        }
        else
        {
            Cmb_Estatus_Busqueda.Enabled = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Dependencia_CheckedChanged
    ///DESCRIPCIÓN: evento del Check Areas en el ModalPopUp
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Dependencia_CheckedChanged(object sender, EventArgs e)
    {
       // Modal_Busqueda.Show();
        if (Chk_Dependencia.Checked == false)
        {
            Cmb_Dependencia.Enabled = false;
            Cmb_Dependencia.SelectedIndex = 0;

        }
        else
        {
            Cmb_Dependencia.Enabled = true;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Dependencia_SelectedIndexChanged
    ///DESCRIPCIÓN: evento del Cmb_Dependencias en el ModalPopUp
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Dependencia_SelectedIndexChanged(object sender, EventArgs e)
    {
       // Modal_Busqueda.Show();
        //if (Cmb_Dependencia.SelectedIndex != 0)
        //{
        //    Requisicion_Negocio.P_Dependencia_ID = Cmb_Dependencia.SelectedValue;
        //    Chk_Area.Checked = false;
        //    Cmb_Area.Enabled = false;
        //    Llenar_Combo_Areas();
        //}

    }

   
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Area_CheckedChanged
    ///DESCRIPCIÓN: evento del Check Areas en el ModalPopUp
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Area_CheckedChanged(object sender, EventArgs e)
    {
        //Modal_Busqueda.Show();
        if (Chk_Area.Checked == false)
        {
            Cmb_Area.Enabled = false;
        }
        else 
        {
            Cmb_Area.Enabled = true;
        }
    }

   
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Avanzada_Click
    ///DESCRIPCIÓN: Evento del boton busqueda avanzada 
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Avanzada_Click(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Div_Productos.Visible = false;
        Div_Requisiciones.Visible = true;
        //Pnl_Busqueda.Visible = true;
        Cargar_Div_Comentarios(false);
        //Modal_Busqueda.Show();
        Carga_Componentes_Busqueda();

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
       
       // Modal_Busqueda.Hide();
    }
    #endregion

    #region Manejo Observaciones
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Alta_Observacion_Click
    ///DESCRIPCIÓN: Evento del boton dar de alta una observacion, que a su ves permitira modificar el estatus
    ///             de la Requisicion. 
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    
    protected void Btn_Alta_Observacion_Click(object sender, ImageClickEventArgs e)
    {
        Requisicion_Negocio = new Cls_Ope_Com_Administrar_Requisiciones_Negocio();
        //Cambiamos la imagen del boton 
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
        switch (Btn_Alta_Observacion.ToolTip)
        {
            case "Evaluar":
                Btn_Alta_Observacion.ToolTip = "Guardar";
                Btn_Alta_Observacion.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Alta_Observacion.Enabled = true;
                Btn_Cancelar_Observacion.Visible = true;
                Txt_Comentario.Enabled = true;
                Txt_Comentario.Text = "";
                Cmb_Estatus.Enabled = true;

                break;
            case "Guardar":
                //Validamos que seleccione un estatus
                Validar_Estatus();                
                //Validamos que ingrese un comentario, ya que es obligatorio al cambiar el estatus
                if (Cmb_Estatus.SelectedValue.Trim() != "AUTORIZADA" && Cmb_Estatus.SelectedValue.Trim() != "CONFIRMADA")
                {
                    if (Txt_Comentario.Text.Trim() == String.Empty)
                    {
                        Div_Contenedor_Msj_Error.Visible = true;
                        Lbl_Mensaje_Error.Text += " + Es necesario un comentario <br />";
                    }

                }
                //Consultamos si es de estatus CONFIRMADA se consulta si el presupuesto es suficiente
                if (Cmb_Estatus.SelectedValue == "CONFIRMADA")
                {
                    //Checamos si existe presupuesto
                    Requisicion_Negocio.P_Requisicion_ID = Session["Requisicion_ID"].ToString();
                    bool Existe_Presupuesto = Requisicion_Negocio.Consultamos_Presupuesto_Existente();
                    if (Existe_Presupuesto == false)
                    {
                        Div_Contenedor_Msj_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + "+ No existe presupuesto suficiente </br>";
                    }
                }

                //if (Cmb_Estatus.SelectedValue == "COTIZADA-RECHAZADA")
                //{
                //    //Consultamos si es una requisicion consolidad
                //    Requisicion_Negocio.P_Requisicion_ID = Session["Requisicion_ID"].ToString();
                //    bool Consolidada = Requisicion_Negocio.Consultar_Requisicion_Consolidada();
                //    if (Consolidada == true)
                //    {
                //        Div_Contenedor_Msj_Error.Visible = true;
                //        Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + "+ No puede rechazar una Requisicion que esta Consolidada, ACCION NO VALIDA </br>";
                //    }//fin del if  Se realizara una validación adicional de presupuesto por parte de la Coordinación de Contabilidad antes de Enviar su requisición a Almacen

                //}
                                
                if (Div_Contenedor_Msj_Error.Visible == false)
                {
                    try
                    {
                        //Asignamos los valores a la clase de negocio
                        //Generamos el Id de la nueva Observacion
                        Requisicion_Negocio.P_Observacion_ID = Requisicion_Negocio.Generar_ID();
                        Requisicion_Negocio.P_Folio = Txt_Folio.Text;
                        Requisicion_Negocio.P_Tipo = Txt_Tipo.Text.Trim();
                        Requisicion_Negocio.P_Tipo_Articulo = Txt_Tipo_Articulo.Text.Trim();
                        //Hacemos nulos los que no son necesarios
                        Requisicion_Negocio.P_Estatus_Busqueda = null;
                        Requisicion_Negocio.P_Campo_Busqueda = null;
                        Requisicion_Negocio.P_Fecha_Inicial = null;
                        //Obtenemos el Id de la requisicion seleccionada
                        Requisicion_Negocio.P_Req_Especiales = "SI";
                        DataSet Dato_Requisicion_ID = Requisicion_Negocio.Consulta_Requisiciones();
                        Requisicion_Negocio.P_Requisicion_ID = Dato_Requisicion_ID.Tables[0].Rows[0].ItemArray[9].ToString();
                        Requisicion_Negocio.P_Comentario = Txt_Comentario.Text;
                        Requisicion_Negocio.Modificar_Requisicion();
                        if (Txt_Comentario.Text.Trim().Length > 0)
                        {
                            Requisicion_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                            Requisicion_Negocio.Alta_Observaciones();
                        }                       
                        //Registramos el historial
                        Cls_Ope_Com_Requisiciones_Negocio Requisicion = new Cls_Ope_Com_Requisiciones_Negocio();
                        Requisicion.Registrar_Historial(Cmb_Estatus.SelectedValue.Trim(),Requisicion_Negocio.P_Requisicion_ID);
                        if (Cmb_Estatus.SelectedValue == "COTIZADA-RECHAZADA")
                        {
                            Requisicion.Registrar_Historial("FILTRADA", Requisicion_Negocio.P_Requisicion_ID);

                        }
                        //Registramos la accion en la bitacora de Eventos
                        String Descripcion = "La Requisición " + Requisicion_Negocio.P_Folio + " cambio al Estatus " + Requisicion_Negocio.P_Estatus;
                       // Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Ope_Com_Administrar_Requisiciones.aspx", Requisicion_Negocio.P_Folio, Descripcion);
                        
                            
                        if(Requisicion_Negocio.P_Tipo == "STOCK" && Requisicion_Negocio.P_Estatus =="AUTORIZADA")
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Requisiciones","alert('Requisicion Autorizada, Se realizará una validación adicional de presupuesto por parte de la Coordinación de Contabilidad antes de enviar su requisición a Almacén');",true);
                        else
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Requisiciones", "alert('Requisición modificada');", true);
                        Requisicion_Negocio = new Cls_Ope_Com_Administrar_Requisiciones_Negocio();
                        Estado_Formulario("inicial");
                        Llenar_Grid_Requisiciones(Requisicion_Negocio);
                        Txt_Comentario.Enabled = false;
                        Cmb_Estatus.Enabled = false;
                        //Modificamos los botones
                        Btn_Alta_Observacion.ToolTip = "Evaluar";
                        Btn_Alta_Observacion.ImageUrl = "~/paginas/imagenes/paginas/accept.png";
                        Btn_Alta_Observacion.Enabled = true;
                        Btn_Cancelar_Observacion.Visible = false;
                        
                    }
                    catch (Exception Ex)
                    {
                        Requisicion_Negocio = new Cls_Ope_Com_Administrar_Requisiciones_Negocio();
                        Estado_Formulario("inicial");
                        Requisicion_Negocio.P_Req_Especiales = "SI";
                        Llenar_Grid_Requisiciones(Requisicion_Negocio);
                        throw new Exception("Error al modificar la Requisicion. Error: [" + Ex.Message + "]");
                    }
                    Llenar_Grid_Comentarios();
                }
                break;


        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Cancelar_Observacion_Click
    ///DESCRIPCIÓN: Evento del boton Cancelar una observacion u comentario
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Cancelar_Observacion_Click(object sender, ImageClickEventArgs e)
    {
        Requisicion_Negocio = new Cls_Ope_Com_Administrar_Requisiciones_Negocio();
        Limpiar_Componentes();
        Cmb_Estatus.Enabled = false;
        Cargar_Div_Comentarios(false);
        Estado_Formulario("inicial");
        Llenar_Grid_Requisiciones(Requisicion_Negocio);
    }

    #endregion

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
            Botones.Add(Btn_Alta_Observacion);
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
                            Cls_Util.Configuracion_Acceso_Sistema_SIAS_AlternateText(Botones, Dr_Menus[0]);//Habilitamos la configuracón de los botones.
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
