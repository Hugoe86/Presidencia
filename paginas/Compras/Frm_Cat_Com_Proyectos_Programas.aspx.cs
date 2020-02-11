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
using Presidencia.Constantes;
using Presidencia.Catalogo_Compras_Proyectos_Programas.Negocio;
using Presidencia.Catalogo_SAP_Conceptos.Negocio;
using Presidencia.Catalogo_SAP_Det_Prog_Partidas.Negocio;
using System.Collections.Generic;
using Presidencia.Sessiones;


public partial class paginas_Compras_Frm_Cat_Com_Proyectos_Programas : System.Web.UI.Page
{

    #region Variables
    private const int Const_Estado_Inicial = 0;
    private const int Const_Estado_Nuevo = 1;
    private const int Const_Estado_Modificar = 2;
    private const int Const_Estado_Buscar = 3;
    private const int Const_Estado_Deshabilitado = 4;
    private const int Const_Estado_Partidas = 5;
    private const int Const_Estado_Grid_Proy_Seleccionado = 6;
    private static DataTable Dt_Proyectos_Programas = new DataTable();
    private static DataTable Dt_Proyectos_Partidas = new DataTable();
    private static string M_Busqueda = "";
    #endregion

    #region Page Load / Init
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!Page.IsPostBack)
            {
                Estado_Botones(Const_Estado_Inicial);
                Cargar_Combos();
                M_Busqueda = "";
                Cargar_Grid(0);
               
                ViewState["SortDirection"] = "DESC";
            }
            Mensaje_Error();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
            Estado_Botones(Const_Estado_Deshabilitado);

        }
    }
    #endregion

    #region Metodos

    #region Metodos Generales
    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Inicializa_Controles
    /// 	DESCRIPCIÓN: Prepara los controles en la forma para que el usuario pueda realizar 
    /// 	            diferentes operaciones
    /// 	PARÁMETROS:
    /// 	CREO: Hugo Enrique Ramirez Aguilera
    /// 	FECHA_CREO: 05-Noviembre-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Estado_Botones(Const_Estado_Inicial);
            Cargar_Combos();
            M_Busqueda = "";
            Cargar_Grid(0);
            
            ViewState["SortDirection"] = "DESC";
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA METODO: LLenar_Combo_Id
    ///        DESCRIPCIÓN: llena todos los combos
    ///         PARAMETROS: 1.- Obj_DropDownList: Combo a llenar
    ///                     2.- Dt_Temporal: DataTable genarada por una consulta a la base de datos
    ///                     3.- Texto: nombre de la columna del dataTable que mostrara el texto en el combo
    ///                     3.- Valor: nombre de la columna del dataTable que mostrara el valor en el combo
    ///                     3.- Seleccion: Id del combo el cual aparecera como seleccionado por default
    ///               CREO: Jesus S. Toledo Rdz.
    ///         FECHA_CREO: 06/9/2010
    ///           MODIFICO:
    ///     FECHA_MODIFICO:
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_ID(DropDownList Obj_DropDownList, DataTable Dt_Temporal, String _Texto, String _Valor, String Seleccion)
    {
        String Texto = "";
        try
        {
            Obj_DropDownList.Items.Clear();
            Obj_DropDownList.Items.Add(new ListItem("< SELECCIONAR >", "0"));
            foreach (DataRow row in Dt_Temporal.Rows)
            {
                if (_Texto.Contains("+"))
                {
                    String[] Array_Texto = _Texto.Split('+');

                    foreach (String Campo in Array_Texto)
                    {
                        Texto = Texto + row[Campo].ToString();
                        Texto = Texto + "  ";
                    }
                }
                else
                {
                    Texto = row[_Texto].ToString();
                }
                Obj_DropDownList.Items.Add(new ListItem(Texto, row[_Valor].ToString()));
                Texto = "";
            }
            Obj_DropDownList.SelectedValue = Seleccion;
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }
    public void Llenar_Combo_ID(DropDownList Obj_DropDownList)
    {
        try
        {
            Obj_DropDownList.Items.Clear();
            Obj_DropDownList.Items.Add(new ListItem("< SELECCIONAR >", "0"));
            Obj_DropDownList.SelectedValue = "0";
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }
    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Mensaje_Error
    ///DESCRIPCION : Muestra el error
    ///PARAMETROS  : P_Texto: texto de un TextBox
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 04-Septiembre-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Mensaje_Error(String P_Mensaje)
    {
        Img_Error.Visible = true;
        Lbl_Error.Text += P_Mensaje + "</br>";
    }
    private void Mensaje_Error()
    {
        Img_Error.Visible = false;
        Lbl_Error.Text = "";
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Estado_Botones
    ///DESCRIPCIÓN: Metodo para establecer el estado de los botones y componentes del formulario
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/02/2011 05:49:53 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Estado_Botones(int P_Estado)
    {
        switch (P_Estado)
        {
            case 0: //Estado inicial

                Mensaje_Error();
                Txt_Busqueda.Text = String.Empty;
                Txt_Comentarios.Text = String.Empty;
                Txt_ID.Text = String.Empty;
                Txt_Clave.Text = String.Empty;
                Txt_Nombre.Text = String.Empty;
                Txt_Elemento_Pep.Text = String.Empty;

                Txt_Comentarios.Enabled = false;
                Txt_ID.Enabled = false;
                Txt_Clave.Enabled = false;
                Txt_Nombre.Enabled = false;
                Txt_Elemento_Pep.Enabled = false;

                Div_Partidas.Visible = false;
                Div_Grid_Programas_Proyectos.Visible = true;
                Div_SAP_Proyectos_Programas.Visible = false;
                Cmb_Estatus.SelectedIndex = 0;
                Btn_Eliminar.Visible = false;
                Btn_Modificar.Visible = false;

                Cmb_Estatus.Enabled = false;

                Grid_Proyectos_Programas.Enabled = true;
                Grid_Proyectos_Programas.SelectedIndex = (-1);

                Btn_Busqueda.Enabled = true;
                Btn_Eliminar.Enabled = true;
                Btn_Modificar.Enabled = true;
                Btn_Nuevo.Enabled = true;
                Btn_Salir.Enabled = true;
                Btn_Busqueda.AlternateText = "Buscar";
                Btn_Eliminar.AlternateText = "Eliminar";
                Btn_Modificar.AlternateText = "Modificar";
                Btn_Nuevo.AlternateText = "Nuevo";
                Btn_Salir.AlternateText = "Salir";

                Btn_Busqueda.ToolTip = "Consultar";
                Btn_Eliminar.ToolTip = "Eliminar";
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Salir.ToolTip = "Inicio";

                Btn_Busqueda.ImageUrl = "~/paginas/imagenes/paginas/busqueda.png";
                Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                Configuracion_Acceso("Frm_Cat_Com_Proyectos_Programas.aspx");
                break;

            case 1: //Nuevo
                Mensaje_Error();

                Txt_Comentarios.Text = String.Empty;
                Txt_Clave.Text = String.Empty;
                Txt_ID.Text = String.Empty;
                Txt_Elemento_Pep.Text = String.Empty;
                Txt_Nombre.Text = String.Empty;

                Cmb_Estatus.SelectedIndex = 0;

                Txt_Comentarios.Enabled = true;
                Txt_Clave.Enabled = true;
                Txt_Nombre.Enabled = true;
                Txt_Elemento_Pep.Enabled = true;
                Cmb_Estatus.Enabled = true;

                Btn_Eliminar.Enabled = false;
                Btn_Modificar.Enabled = false;
                Btn_Nuevo.Enabled = true;
                Btn_Salir.Enabled = true;

                Div_Partidas.Visible = false;
                Div_Grid_Programas_Proyectos.Visible = false;
                Div_SAP_Proyectos_Programas.Visible = true;
                Cmb_Estatus.SelectedIndex = 0;
                Btn_Eliminar.Visible = false;
                Btn_Modificar.Visible = false;

                Grid_Proyectos_Programas.SelectedIndex = (-1);
                Grid_Proyectos_Programas.Enabled = false;
                Grid_Partidas.SelectedIndex = (-1);
                Div_Partidas.Visible = false;

                Btn_Eliminar.AlternateText = "Eliminar";
                Btn_Modificar.AlternateText = "Modificar";
                Btn_Nuevo.AlternateText = "Guardar";
                Btn_Salir.AlternateText = "Cancelar";

                Btn_Eliminar.ToolTip = "Eliminar";
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Nuevo.ToolTip = "Guardar";
                Btn_Salir.ToolTip = "Cancelar";

                Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar_deshabilitado.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar_deshabilitado.png";

                break;

            case 2: //Modificar
                Mensaje_Error();

                Txt_Comentarios.Enabled = true;
                Txt_Clave.Enabled = true;
                Txt_Nombre.Enabled = true;
                Txt_Elemento_Pep.Enabled = true;

                Cmb_Estatus.Enabled = true;

                Btn_Eliminar.Enabled = false;
                Btn_Modificar.Enabled = true;
                Btn_Nuevo.Enabled = false;
                Btn_Salir.Enabled = true;

                Grid_Partidas.SelectedIndex = (-1);
                //Div_Partidas.Visible = false;

                Btn_Eliminar.AlternateText = "Eliminar";
                Btn_Modificar.AlternateText = "Guardar";
                Btn_Nuevo.AlternateText = "Nuevo";
                Btn_Salir.AlternateText = "Cancelar";

                Btn_Eliminar.ToolTip = "Eliminar";
                Btn_Modificar.ToolTip = "Guardar";
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Salir.ToolTip = "Cancelar";

                Grid_Proyectos_Programas.Enabled = false;

                Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar_deshabilitado.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo_deshabilitado.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";

                break;

            case 3: //Buscar
                Mensaje_Error();

                Txt_Comentarios.Text = String.Empty;
                Txt_ID.Text = String.Empty;
                Txt_Clave.Text = String.Empty;
                Txt_Elemento_Pep.Text = String.Empty;

                Cmb_Estatus.SelectedIndex = 0;

                break;

            case 4: //Desabilitar

                Txt_Busqueda.Text = String.Empty;
                Txt_Comentarios.Text = String.Empty;
                Txt_ID.Text = String.Empty;
                Txt_Clave.Text = String.Empty;
                Txt_Nombre.Text = String.Empty;
                Txt_Elemento_Pep.Text = String.Empty;

                Txt_Comentarios.Enabled = false;
                Txt_ID.Enabled = false;
                Txt_Clave.Enabled = false;
                Txt_Nombre.Enabled = false;
                Txt_Elemento_Pep.Enabled = false;

                Cmb_Estatus.Enabled = false;

                Grid_Proyectos_Programas.Enabled = true;
                Grid_Proyectos_Programas.SelectedIndex = (-1);
                Grid_Partidas.SelectedIndex = (-1);
                Div_Partidas.Visible = false;

                Btn_Busqueda.Enabled = false;
                Btn_Eliminar.Enabled = false;
                Btn_Modificar.Enabled = false;
                Btn_Nuevo.Enabled = false;
                Btn_Salir.Enabled = false;
                Btn_Busqueda.AlternateText = "Buscar";
                Btn_Eliminar.AlternateText = "Eliminar";
                Btn_Modificar.AlternateText = "Modificar";
                Btn_Nuevo.AlternateText = "Nuevo";
                Btn_Salir.AlternateText = "Salir";

                Btn_Busqueda.ToolTip = "Buscar";
                Btn_Eliminar.ToolTip = "Eliminar";
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Salir.ToolTip = "Inicio";

                Btn_Busqueda.ImageUrl = "~/paginas/imagenes/paginas/busqueda.png";
                Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                break;

            case 5: //Partidas
                Mensaje_Error();

                Cmb_Capitulo.SelectedIndex = 0;
                Llenar_Combo_ID(Cmb_Conceptos);
                Llenar_Combo_ID(Cmb_Partida_Especifica);
                Llenar_Combo_ID(Cmb_Partida_General);
                Grid_Partidas.SelectedIndex = (-1);

                break;
            case 6: //Grid Proyectos Seleccionado
                Mensaje_Error();

                Div_Partidas.Visible = true;
                Div_Grid_Programas_Proyectos.Visible = false;
                Div_SAP_Proyectos_Programas.Visible = true;
                Btn_Eliminar.Visible = true;
                Btn_Modificar.Visible = true;
                Btn_Salir.AlternateText = "Listado";
                Btn_Salir.ToolTip = "Listado";

                break;
        }
    }
    #endregion


    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Combos
    ///DESCRIPCIÓN: metodo usado para cargar la informacion de todos los combos del formulario con la respectiva consulta
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/03/2011 08:46:12 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Combos()
    {
        try
        {
            Cls_Cat_Com_Proyectos_Programas_Negocio Proyectos_Programas = new Cls_Cat_Com_Proyectos_Programas_Negocio();
            Llenar_Combo_ID(Cmb_Capitulo, Proyectos_Programas.Consulta_Capitulos(),Cat_SAP_Capitulos.Campo_Clave +"+"+ Cat_SAP_Capitulos.Campo_Descripcion, Cat_SAP_Capitulos.Campo_Capitulo_ID, "0");
            Cmb_Estatus.Items.Clear();
            Cmb_Partida_General.Items.Clear();
            Cmb_Partida_Especifica.Items.Clear();
            Cmb_Conceptos.Items.Clear();
            Cmb_Estatus.Items.Add(new ListItem("<SELECCIONE>", "0"));
            Cmb_Estatus.Items.Add(new ListItem("ACTIVO", "ACTIVO"));
            Cmb_Estatus.Items.Add(new ListItem("INACTIVO", "INACTIVO"));
        }
        catch (Exception Ex)
        {
            throw new Exception("No se pudieron cargar los datos necesarios" + "</br>" + Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Grid
    ///DESCRIPCIÓN: Realizar la consulta y llenar el grido con estos datos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/03/2011 12:14:35 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Grid(int Page_Index)
    {
        try
        {
            Cls_Cat_Com_Proyectos_Programas_Negocio Proyectos_Programas = new Cls_Cat_Com_Proyectos_Programas_Negocio();
            Proyectos_Programas.P_Clave = M_Busqueda;
            Dt_Proyectos_Programas = Proyectos_Programas.Consulta_Programas_Proyectos();
            Grid_Proyectos_Programas.PageIndex = Page_Index;
            Grid_Proyectos_Programas.DataSource = Dt_Proyectos_Programas;
            Grid_Proyectos_Programas.DataBind();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Grid_Partidas
    ///DESCRIPCIÓN: Realizar la consulta y llenar el grido con estos datos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/03/2011 12:14:35 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Grid_Partidas(int Page_Index)
    {
        try
        {
            Cls_Cat_Com_Proyectos_Programas_Negocio Proyectos_Programas = new Cls_Cat_Com_Proyectos_Programas_Negocio();
            Proyectos_Programas.P_Proyecto_Programa_ID = Txt_ID.Text.Trim();
            Dt_Proyectos_Partidas = Proyectos_Programas.Consulta_Programas_Partidas();
            Grid_Partidas.PageIndex = Page_Index;
            Grid_Partidas.DataSource = Dt_Proyectos_Partidas;
            Grid_Partidas.DataBind();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }    
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos
    ///DESCRIPCIÓN: metodo que recibe el DataRow seleccionado de la grilla y carga los datos en los componetes de l formulario
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/03/2011 02:07:54 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Datos(DataRow Dr_Proyectos_Programas)
    {
        try
        {
            Txt_Comentarios.Text = Dr_Proyectos_Programas[Cat_Com_Proyectos_Programas.Campo_Descripcion].ToString();
            Txt_ID.Text = Dr_Proyectos_Programas[Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID].ToString();
            Txt_Clave.Text = Dr_Proyectos_Programas[Cat_Com_Proyectos_Programas.Campo_Clave].ToString();
            Txt_Nombre.Text = Dr_Proyectos_Programas[Cat_Com_Proyectos_Programas.Campo_Nombre].ToString();
            Txt_Elemento_Pep.Text = Dr_Proyectos_Programas[Cat_Com_Proyectos_Programas.Campo_Elemento_PEP].ToString();
            Cmb_Estatus.SelectedValue = Dr_Proyectos_Programas[Cat_Com_Proyectos_Programas.Campo_Estatus].ToString();

            Cargar_Combos();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    
    
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Validar_Datos
    ///DESCRIPCIÓN: Guardar datos para dar de alta un nuevo registro de un servicio
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/03/2011 10:45:17 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Boolean Validar_Datos()
    {
        Boolean Resultado = true;
        try
        {
            if (Txt_Clave.Text.Trim() == "")
            {
                Resultado = false;
                Mensaje_Error("Favor de ingresar la Clave");
            }
            if (Txt_Nombre.Text.Trim() == "")
            {
                Resultado = false;
                Mensaje_Error("Favor de ingresar el Nombre");
            }
            if (Txt_Elemento_Pep.Text.Trim() == "")
            {
                Resultado = false;
                Mensaje_Error("Favor de ingresar el Elemento PEP");
            }            
            if (Cmb_Estatus.SelectedIndex <= 0)
            {
                Resultado = false;
                Mensaje_Error("Favor de especificar el Estatus");
            }
            if (!Txt_Comentarios.Text.Trim().Equals(""))
            {
                if (Txt_Comentarios.Text.Trim().Length >= 250)
                {
                    Txt_Comentarios.Text = Txt_Comentarios.Text.Trim().Substring(0, 250);
                }
            }

        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
        return Resultado;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Alta_Concepto
    ///DESCRIPCIÓN: Realiza el alta de un nuevo registro de un concepto
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 26/02/2011 11:09:45
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Alta()
    {
        try
        {
            if (Validar_Datos())
            {
                Cls_Cat_Com_Proyectos_Programas_Negocio Proyecto_Programa = new Cls_Cat_Com_Proyectos_Programas_Negocio();
                if (!Txt_Comentarios.Text.Equals(""))
                {
                    Proyecto_Programa.P_Comentarios = Txt_Comentarios.Text.Trim();
                }
                Proyecto_Programa.P_Estatus = Cmb_Estatus.SelectedValue;
                Proyecto_Programa.P_Nombre = Txt_Nombre.Text.Trim();
                Proyecto_Programa.P_Clave = Txt_Clave.Text.Trim();
                Proyecto_Programa.P_Elemento_Pep = Txt_Elemento_Pep.Text.Trim();
                Proyecto_Programa.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                Proyecto_Programa.Alta_Programas_Proyectos();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Proyectos y Programas", "alert('El Alta del registro fue Exitosa');", true);
                Estado_Botones(Const_Estado_Inicial);
                Txt_Busqueda.Text = "";
                Cargar_Grid(0);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message, Ex);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Baja_Concepto
    ///DESCRIPCIÓN: dar de baja un registro de la base de datos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/04/2011 11:25:30 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Baja()
    {
        try
        {
            Cls_Cat_Com_Proyectos_Programas_Negocio Proyecto_Programa = new Cls_Cat_Com_Proyectos_Programas_Negocio();
            Proyecto_Programa.P_Proyecto_Programa_ID = Txt_ID.Text.Trim();            
            Proyecto_Programa.Baja_Programas_Proyectos();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Proyectos y Programas", "alert('La Baja del registro fue Exitosa');", true);
            Estado_Botones(Const_Estado_Inicial);
            Txt_Busqueda.Text = "";
            Cargar_Grid(0);
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message, Ex);
        }

    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Modificar_Concepto
    ///DESCRIPCIÓN: Modifica un registro y lo guarda en la base de datos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/04/2011 11:58:30 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Modificar()
    {
        try
        {
            if (Validar_Datos())
            {
                Cls_Cat_Com_Proyectos_Programas_Negocio Proyecto_Programa = new Cls_Cat_Com_Proyectos_Programas_Negocio();
                if (!Txt_Comentarios.Text.Equals(""))
                {
                    Proyecto_Programa.P_Comentarios = Txt_Comentarios.Text.Trim();
                }
                Proyecto_Programa.P_Proyecto_Programa_ID = Txt_ID.Text.Trim();
                Proyecto_Programa.P_Estatus = Cmb_Estatus.SelectedValue;                
                Proyecto_Programa.P_Clave = Txt_Clave.Text.Trim();
                Proyecto_Programa.P_Nombre = Txt_Nombre.Text.Trim();
                Proyecto_Programa.P_Elemento_Pep = Txt_Elemento_Pep.Text.Trim();
                Proyecto_Programa.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                Proyecto_Programa.Cambio_Programas_Proyectos();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Proyectos y Programas", "alert('La modificación del Programa fue Exitosa');", true);
                Estado_Botones(Const_Estado_Inicial);
                Txt_Busqueda.Text = "";
                Cargar_Grid(0);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message, Ex);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Agregar_Partida
    ///DESCRIPCIÓN: Se asigna y se da de alta la partida al proyecto
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/02/2011 12:16:05 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Agregar_Partida()
    {
        try
        {
            Cls_Cat_SAP_Det_Prog_Partidas_Negocio Proyecto_Programa_Detalles = new Cls_Cat_SAP_Det_Prog_Partidas_Negocio();
                Proyecto_Programa_Detalles.P_Det_Proyecto_Programa_ID = Txt_ID.Text.Trim();
                Proyecto_Programa_Detalles.P_Det_Partida_ID = Cmb_Partida_Especifica.SelectedValue;
                Proyecto_Programa_Detalles.Alta_Partida();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Proyectos y Programas", "alert('La asignacion de la partida fue Exitosa');", true);
                Estado_Botones(Const_Estado_Partidas);
                Cargar_Grid_Partidas(0);            
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message, Ex);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Quitar_Partida
    ///DESCRIPCIÓN: Se borra la partida del proyecto seleccionado
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/02/2011 12:16:56 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Quitar_Partida()
    {
        try
        {
            Cls_Cat_SAP_Det_Prog_Partidas_Negocio Proyecto_Programa_Detalles = new Cls_Cat_SAP_Det_Prog_Partidas_Negocio();
            Proyecto_Programa_Detalles.P_Det_Prog_Partidas_ID = HttpUtility.HtmlDecode(Grid_Partidas.SelectedRow.Cells[1].Text.Trim());
            Proyecto_Programa_Detalles.Baja_Partida();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Proyectos y Programas", "alert('La Eliminacion de la partida fue Exitosa');", true);
            Estado_Botones(Const_Estado_Partidas);
            Cargar_Grid_Partidas(0);
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message, Ex);
        }
    }

    #endregion

    #region Eventos

        #region Eventos Programas

    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
            {
                Estado_Botones(Const_Estado_Nuevo);
            }
            else
            {
                Alta();
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Grid_Proyectos_Programas.SelectedIndex > (-1))
            {
                if (Btn_Modificar.AlternateText.Equals("Modificar"))
                {
                    Estado_Botones(Const_Estado_Modificar);
                }
                else
                {
                    Modificar();
                }
            }
            else
            {
                Mensaje_Error("Favor de seleccionar el registro a modificar");
            }

        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);

        }
    }
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Grid_Proyectos_Programas.SelectedIndex > (-1))
            {
                Baja();
            }
            else
            {
                Mensaje_Error("Favor de seleccionar el registro a eliminar");
            }
        }
        catch (Exception Ex)
        {            
            //Mensaje_Error(Ex.Message);
            String Msg = Ex.ToString();
            Mensaje_Error("Es posible que existan registros secuendarios del registro que desea eliminar");
        }
    }
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.AlternateText.Equals("Salir"))
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else if (Btn_Salir.AlternateText.Equals("Listado"))
            {
                Estado_Botones(Const_Estado_Inicial);
            }
            else
            {
                Estado_Botones(Const_Estado_Inicial);
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    protected void Btn_Busqueda_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Mensaje_Error();
            Estado_Botones(Const_Estado_Buscar);
            Grid_Proyectos_Programas.SelectedIndex = (-1);
            M_Busqueda = Txt_Busqueda.Text.Trim();
            Cargar_Grid(0);
            
            
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    #endregion

        #region Eventos Partidas

            #region Eventos Partidas_Combos
    protected void Cmb_Capitulo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cmb_Capitulo.SelectedIndex > 0)
        {            
            Cls_Cat_Com_Proyectos_Programas_Negocio Programas_Negocio = new Cls_Cat_Com_Proyectos_Programas_Negocio();
            Cmb_Capitulo.ToolTip = Cmb_Capitulo.SelectedItem.Text;
            Llenar_Combo_ID(Cmb_Partida_Especifica);
            Llenar_Combo_ID(Cmb_Partida_General);
            Llenar_Combo_ID(Cmb_Conceptos);
            Llenar_Combo_ID(Cmb_Conceptos, Programas_Negocio.Consulta_Conceptos(Cmb_Capitulo.SelectedValue.ToString()), Cat_Sap_Concepto.Campo_Clave +"+"+ Cat_Sap_Concepto.Campo_Descripcion, Cat_Sap_Concepto.Campo_Concepto_ID, "0");
        }
    }  

    protected void Cmb_Partida_General_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cmb_Partida_General.SelectedIndex > 0)
        {
            Cmb_Partida_General.ToolTip = Cmb_Partida_General.SelectedItem.Text;
            Cls_Cat_Com_Proyectos_Programas_Negocio Programas_Negocio = new Cls_Cat_Com_Proyectos_Programas_Negocio();
            Llenar_Combo_ID(Cmb_Partida_Especifica);
            Llenar_Combo_ID(Cmb_Partida_Especifica, Programas_Negocio.Consulta_Partidas_Especificas(Cmb_Partida_General.SelectedValue), Cat_Sap_Partidas_Especificas.Campo_Clave +"+"+ Cat_Sap_Partidas_Especificas.Campo_Nombre, Cat_Sap_Partidas_Especificas.Campo_Partida_ID, "0");
        }
    }

    protected void Cmb_Conceptos_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cmb_Conceptos.SelectedIndex > 0)
        {
            Cmb_Conceptos.ToolTip = Cmb_Conceptos.SelectedItem.Text;
            Cls_Cat_Com_Proyectos_Programas_Negocio Programas_Negocio = new Cls_Cat_Com_Proyectos_Programas_Negocio();
            Llenar_Combo_ID(Cmb_Partida_Especifica);
            Llenar_Combo_ID(Cmb_Partida_General);
            Llenar_Combo_ID(Cmb_Partida_General, Programas_Negocio.Consulta_Partidas_Genericas(Cmb_Conceptos.SelectedValue), Cat_SAP_Partida_Generica.Campo_Clave +"+"+ Cat_SAP_Partida_Generica.Campo_Descripcion, Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID, "0");
        }
    }
    protected void Cmb_Partida_Especifica_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cmb_Partida_Especifica.ToolTip = Cmb_Partida_Especifica.SelectedItem.Text;
    }
    #endregion

            protected void Btn_Agregar_Click(object sender, ImageClickEventArgs e)
            {
                try
                {
                    if ( Cmb_Partida_Especifica.SelectedIndex > 0 )
                    {
                        Agregar_Partida();
                    }
                    else
                    {
                        Mensaje_Error("Seleccione la partida que desea agregar");
                    }
                }
                catch (Exception Ex)
                {
                    Mensaje_Error(Ex.Message);
                }
            }
            protected void Btn_Quitar_Click(object sender, ImageClickEventArgs e)
            {
                try
                {
                    if (Grid_Partidas.SelectedIndex > (-1))
                    {
                        Quitar_Partida();
                    }
                    else
                    {
                        Mensaje_Error("Favor de seleccionar la partida que desea quitar");
                    }
                }
                catch (Exception Ex)
                {
                    Mensaje_Error(Ex.Message);
                }
            }

    #endregion

    #endregion

    #region Eventos Grid
    protected void Grid_Proyectos_Programas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Proyectos_Programas.SelectedIndex = (-1);
            Cargar_Grid(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    protected void Grid_Proyectos_Programas_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Com_Proyectos_Programas_Negocio Proyecto = new Cls_Cat_Com_Proyectos_Programas_Negocio();
        
        DataTable Dt_Programa = new DataTable();
        try
        {
            if (Grid_Proyectos_Programas.SelectedIndex > (-1))
            {
                Estado_Botones(Const_Estado_Grid_Proy_Seleccionado);
                GridViewRow selectedRow = Grid_Proyectos_Programas.Rows[Grid_Proyectos_Programas.SelectedIndex];
                String clave = HttpUtility.HtmlDecode(selectedRow.Cells[2].Text).ToString().Trim();
                Proyecto.P_Clave= clave;
                Dt_Programa = Proyecto.Consulta_Programas_Proyectos();

                foreach (DataRow Registro in Dt_Programa.Rows)
                {
                    Txt_Comentarios.Text = Registro[Cat_Com_Proyectos_Programas.Campo_Descripcion].ToString();
                    Txt_ID.Text = Registro[Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID].ToString();
                    Txt_Clave.Text = Registro[Cat_Com_Proyectos_Programas.Campo_Clave].ToString();
                    Txt_Nombre.Text = Registro[Cat_Com_Proyectos_Programas.Campo_Nombre].ToString();
                    Txt_Elemento_Pep.Text = Registro[Cat_Com_Proyectos_Programas.Campo_Elemento_PEP].ToString();
                    Cmb_Estatus.SelectedValue = Registro[Cat_Com_Proyectos_Programas.Campo_Estatus].ToString();
                }
                //Cargar_Datos(Dt_Programa);
                Grid_Partidas.SelectedIndex = (-1);
                Cargar_Grid_Partidas(0);
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    protected void Grid_Partidas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Partidas.SelectedIndex = (-1);
            Cargar_Grid_Partidas(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    protected void Grid_Proyectos_Programas_Sorting(object sender, GridViewSortEventArgs e)
    {
        //Se consultan los movimientos que actualmente se encuentran registradas en el sistema.
        Consultar_Grid_Proyecto();
        DataTable Dt_Proyectos_Programas = (Grid_Proyectos_Programas.DataSource as DataTable);

        if (Dt_Proyectos_Programas != null)
        {
            DataView Dv_Proyectos_Programas = new DataView(Dt_Proyectos_Programas);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Proyectos_Programas.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Proyectos_Programas.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Proyectos_Programas.DataSource = Dv_Proyectos_Programas;
            Grid_Proyectos_Programas.DataBind();
        }
    }

   
    protected void Grid_Partidas_Sorting(object sender, GridViewSortEventArgs e)
    {
        //Se consultan los movimientos que actualmente se encuentran registradas en el sistema.
        Cargar_Grid_Partidas(0);
        DataTable Dt_Partida = (Grid_Partidas.DataSource as DataTable);

        if (Dt_Partida != null)
        {
            DataView Dv_Partida = new DataView(Dt_Partida);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Partida.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Partida.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Partidas.DataSource = Dv_Partida;
            Grid_Partidas.DataBind();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Grid_Proyecto
    /// DESCRIPCION : Llena el grid con los programas que se encuentran en la 
    ///               base de datos
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramirez Aguilera
    /// FECHA_CREO  : 03-Noviembre-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Grid_Proyecto()
    {
        Cls_Cat_Com_Proyectos_Programas_Negocio Rs_Proyecto = new Cls_Cat_Com_Proyectos_Programas_Negocio();
        
        DataTable Dt_Programa = null; //Variable que obtendra los datos de la consulta 
        try
        {
            Dt_Programa = Rs_Proyecto.Consulta_Programas_Proyectos();
            Session["Consulta_Proyecto"] = Dt_Programa;
            Grid_Proyectos_Programas.DataSource = (DataTable)Session["Consulta_Proyecto"];
            Grid_Proyectos_Programas.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar Proyecto " + ex.Message.ToString(), ex);
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
            Botones.Add(Btn_Eliminar);
            Botones.Add(Btn_Busqueda);

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