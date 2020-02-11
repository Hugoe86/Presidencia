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
using Presidencia.Catalogo_Compras_Proveedores.Negocio;

public partial class paginas_Contabilidad_Frm_Ope_Con_Registro_Contratos : System.Web.UI.Page
{
    #region (Page Load)
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
        if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

        try
        {
            if (!IsPostBack)
            {
                Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                Limpia_Controles();     //Limpia los controles del forma
                ViewState["SortDirection"] = "ASC";
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
            Botones.Add(Btn_Eliminar);
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
            Txt_Cuenta_Contable.Text = "";
            Txt_Monto_Comprometido.Text = "";
            Txt_No_Compromiso.Text = "";
            Txt_Concepto.Text = "";
            Txt_Cuenta_Contable_ID.Text = "";
            Cmb_Area_Funcional.Items.Clear();
            Cmb_Fte_Financiamiento.Items.Clear();
            Cmb_Programa.Items.Clear();
            Cmb_Unidad_Responsable.Items.Clear();
            Cmb_Partida.Items.Clear();
            Cmb_Estatus.SelectedIndex = -1;

            Grid_Compromisos.DataSource = new DataTable();
            Grid_Compromisos.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception("Limpiar_Controles " + ex.Message.ToString(), ex);
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
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = true;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Configuracion_Acceso("Frm_Ope_Con_Registro_Contratos.aspx");
                    break;

                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Eliminar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    break;

                case "Modificar":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    break;
            }

            Txt_Cuenta_Contable.Enabled = Habilitado;
            Txt_Concepto.Enabled = Habilitado;
            Cmb_Beneficiario_Tipo.Enabled = Habilitado;
            Cmb_Area_Funcional.Enabled = false;
            Cmb_Fte_Financiamiento.Enabled = false;
            Cmb_Partida.Enabled = false;
            Cmb_Programa.Enabled = false;
            Cmb_Unidad_Responsable.Enabled = false;
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos_Compromiso
    /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 11/Octubre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos_Compromiso()
    {
        Boolean Datos_Validos = true;
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

        if (Cmb_Estatus.SelectedIndex <= -1)
        {
            Lbl_Mensaje_Error.Text += " + Estatus <br>";
            Datos_Validos = false;
        }
        if (string.IsNullOrEmpty(Txt_Cuenta_Contable.Text))
        {
            Lbl_Mensaje_Error.Text += " + La Cuenta Contable <br>";
            Datos_Validos = false;
        }
        if (String.IsNullOrEmpty(Txt_Monto_Comprometido.Text))
        {
            Lbl_Mensaje_Error.Text += " + El Monto Comprometido <br>";
            Datos_Validos = false;
        }
        if (Cmb_Area_Funcional.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += " + Area Funcionar <br>";
            Datos_Validos = false;
        }
        if (Cmb_Fte_Financiamiento.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += " + Fuente de Financiamiento <br>";
            Datos_Validos = false;
        }
        if (Cmb_Partida.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += " + Partida <br>";
            Datos_Validos = false;
        }
        if (Cmb_Programa.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += " + Programas y Proyectos <br>";
            Datos_Validos = false;
        }
        if (Cmb_Unidad_Responsable.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += " + Dependencia <br>";
            Datos_Validos = false;
        }
        return Datos_Validos;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llenar_Cmb_Dependencia
    /// DESCRIPCION : Llena el ComboBox de acuerdo a la Partida
    /// PARAMETROS  : Dependencias_ID almacena los IDs
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 11/Octubre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llenar_Cmb_Dependencia(string[] Dependencias_ID, int Cont_Dependencias, Boolean PopUp)
    {
        Cls_Cat_Dependencias_Negocio Rs_Consulta_Dependencias_Negocio = new Cls_Cat_Dependencias_Negocio(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Dependencias; //Variable que obtendra los datos de la consulta 
        try
        {
            Cmb_Unidad_Responsable.Items.Clear();
            Cmb_Unidad_Responsable.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Busqueda_Dependencia.Items.Clear();
            Cmb_Busqueda_Dependencia.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            for (int Cont_Desplazamiento = 0; Cont_Desplazamiento < Cont_Dependencias; Cont_Desplazamiento++)
            {
                Rs_Consulta_Dependencias_Negocio.P_Dependencia_ID = Dependencias_ID[Cont_Desplazamiento];
                Dt_Dependencias = Rs_Consulta_Dependencias_Negocio.Consulta_Dependencias();
                foreach (DataRow Registro in Dt_Dependencias.Rows)
                {
                    if (PopUp == false)
                    {
                        Cmb_Unidad_Responsable.Items.Insert(Cont_Desplazamiento + 1, new ListItem(Registro[Cat_Dependencias.Campo_Clave].ToString() + " - " + Registro[Cat_Dependencias.Campo_Nombre].ToString(), Registro[Cat_Dependencias.Campo_Dependencia_ID].ToString()));
                        Cmb_Unidad_Responsable.SelectedIndex = -1;
                    }
                    else
                    {
                        Cmb_Busqueda_Dependencia.Items.Insert(Cont_Desplazamiento + 1, new ListItem(Registro[Cat_Dependencias.Campo_Clave].ToString() + " - " + Registro[Cat_Dependencias.Campo_Nombre].ToString(), Registro[Cat_Dependencias.Campo_Dependencia_ID].ToString()));
                        Cmb_Busqueda_Dependencia.SelectedIndex = -1;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Llenar_Cmb_Dependencia " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llenar_Cmb_Area_Funcional
    /// DESCRIPCION : Llena el ComboBox de acuerdo a la Fuente de Financiamiento
    /// PARAMETROS  : Areas_ID: almacena los IDs
    ///               Cont_Areas: almacena la cantidad de IDs que son
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 11/Octubre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llenar_Cmb_Area_Funcional(string[] Areas_ID, int Cont_Areas, Boolean Popup)
    {
        Cls_Cat_SAP_Area_Funcional_Negocio Rs_Consulta_Areas = new Cls_Cat_SAP_Area_Funcional_Negocio(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Areas; //Variable que obtendra los datos de la consulta 
        try
        {
            Cmb_Area_Funcional.Items.Clear();
            Cmb_Area_Funcional.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Busqueda_Area_Funcional.Items.Clear();
            Cmb_Busqueda_Area_Funcional.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            for (int Cont_Desplazamiento = 0; Cont_Desplazamiento < Cont_Areas; Cont_Desplazamiento++)
            {
                Rs_Consulta_Areas.P_Area_Funcional_ID = Areas_ID[Cont_Desplazamiento];
                Dt_Areas = Rs_Consulta_Areas.Consulta_Area_Funcional_Especial();
                foreach (DataRow Registro in Dt_Areas.Rows)
                {
                    if (Popup == false)
                    {
                        Cmb_Area_Funcional.Items.Insert(Cont_Desplazamiento + 1, new ListItem(Registro["CLAVE_NOMBRE"].ToString(), Registro[Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID].ToString()));
                        Cmb_Area_Funcional.SelectedIndex = -1;
                    }
                    else
                    {
                        Cmb_Busqueda_Area_Funcional.Items.Insert(Cont_Desplazamiento + 1, new ListItem(Registro["CLAVE_NOMBRE"].ToString(), Registro[Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID].ToString()));
                        Cmb_Busqueda_Area_Funcional.SelectedIndex = -1;
                    }
                }
            }
            Cmb_Area_Funcional.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            throw new Exception("Llenar_Cmb_Area_Funcional " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llenar_Cmb_Proy_Prog
    /// DESCRIPCION : Llena el ComboBox de acuerdo a la Fuente de Financiamiento
    /// PARAMETROS  : Programas_ID: almacena los IDs
    ///               Cont_Programas: almacena la cantidad de IDs que son
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 11/Octubre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llenar_Cmb_Proy_Prog(string[] Programas_ID, int Cont_Programas, Boolean Popup)
    {
        Cls_Cat_Com_Proyectos_Programas_Negocio Rs_Consulta_Programas = new Cls_Cat_Com_Proyectos_Programas_Negocio(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Programas; //Variable que obtendra los datos de la consulta 
        try
        {
            Cmb_Programa.Items.Clear();
            Cmb_Programa.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Busqueda_Proyectos.Items.Clear();
            Cmb_Busqueda_Proyectos.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            for (int Cont_Desplazamiento = 0; Cont_Desplazamiento < Cont_Programas; Cont_Desplazamiento++)
            {
                Rs_Consulta_Programas.P_Proyecto_Programa_ID = Programas_ID[Cont_Desplazamiento];
                Dt_Programas = Rs_Consulta_Programas.Consulta_Programas_Proyectos();
                foreach (DataRow Registro in Dt_Programas.Rows)
                {
                    if (Popup == false)
                    {
                        Cmb_Programa.Items.Insert(Cont_Desplazamiento + 1, new ListItem(Registro[Cat_Com_Proyectos_Programas.Campo_Clave].ToString() + " - " + Registro[Cat_Com_Proyectos_Programas.Campo_Descripcion].ToString(), Registro[Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID].ToString()));
                        Cmb_Programa.SelectedIndex = -1;
                    }
                    else
                    {
                        Cmb_Busqueda_Proyectos.Items.Insert(Cont_Desplazamiento + 1, new ListItem(Registro[Cat_Com_Proyectos_Programas.Campo_Clave].ToString() + " - " + Registro[Cat_Com_Proyectos_Programas.Campo_Descripcion].ToString(), Registro[Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID].ToString()));
                        Cmb_Busqueda_Proyectos.SelectedIndex = -1;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Llenar_Cmb_Area_Funcional " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llenar_Cmb_Fuentes_Financiamiento
    /// DESCRIPCION : Llena el ComboBox de acuerdo al Programa
    /// PARAMETROS  : Fuentes_ID: almacena los IDs
    ///               Fuentes_ID: almacena la cantidad de IDs que son
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 11/Octubre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llenar_Cmb_Fuentes_Financiamiento(string[] Fuentes_ID, int Cont_Fuentes, Boolean PopUp)
    {
        Cls_Cat_SAP_Fuente_Financiamiento_Negocio Rs_Consulta_Fuentes = new Cls_Cat_SAP_Fuente_Financiamiento_Negocio(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Fuentes; //Variable que obtendra los datos de la consulta 
        try
        {
            Cmb_Fte_Financiamiento.Items.Clear();
            Cmb_Fte_Financiamiento.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Busqueda_Financiamiento.Items.Clear();
            Cmb_Busqueda_Financiamiento.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            for (int Cont_Desplazamiento = 0; Cont_Desplazamiento < Cont_Fuentes; Cont_Desplazamiento++)
            {
                Rs_Consulta_Fuentes.P_Fuente_Financiamiento_ID = Fuentes_ID[Cont_Desplazamiento];
                Dt_Fuentes = Rs_Consulta_Fuentes.Consulta_Datos_Fuente_Financiamiento();
                foreach (DataRow Registro in Dt_Fuentes.Rows)
                {
                    if (PopUp == false)
                    {
                        Cmb_Fte_Financiamiento.Items.Insert(Cont_Desplazamiento + 1, new ListItem(Registro[Cat_SAP_Fuente_Financiamiento.Campo_Clave].ToString() + " - " + Registro[Cat_SAP_Fuente_Financiamiento.Campo_Descripcion].ToString(), Registro[Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID].ToString()));
                        Cmb_Fte_Financiamiento.SelectedIndex = -1;
                    }
                    else
                    {
                        Cmb_Busqueda_Financiamiento.Items.Insert(Cont_Desplazamiento + 1, new ListItem(Registro[Cat_SAP_Fuente_Financiamiento.Campo_Clave].ToString() + " - " + Registro[Cat_SAP_Fuente_Financiamiento.Campo_Descripcion].ToString(), Registro[Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID].ToString()));
                        Cmb_Busqueda_Financiamiento.SelectedIndex = -1;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Llenar_Cmb_Fuentes_Financiamiento " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Aplicar_Mascara_Cuenta_Contable
    /// DESCRIPCION : Aplica la Mascara a la Cuenta Contable
    /// PARAMETROS  : Cuenta_Contable: Recibe el numero de cuenta contable
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 20/Septiembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private string Aplicar_Mascara_Cuenta_Contable(string Cuenta_Contable)
    {
        try
        {
            string Mascara_Cuenta_Contable = Consulta_Parametros();
            string Cuenta_Contable_Con_Formato = "";
            Boolean Primer_Numero = true;
            int Caracteres_Extraidos_Cuenta_Contable = 0;  //Variable que almacena la cantidad de caracteres extraidos de la cuenta.
            int Inicio_Extraccion = 0; //Variable que almacena el inicio de la cadena a extraer.
            int Fin_Extraccion = 0;    //Variable que almacena el fin de la cadena a extraer.
            for (int Cont_Desplazamiento = 0; Cont_Desplazamiento < Mascara_Cuenta_Contable.Length; Cont_Desplazamiento++)
            {
                if (Primer_Numero == true && Mascara_Cuenta_Contable.Substring(Cont_Desplazamiento, 1) == "#")
                {
                    if (Cont_Desplazamiento == 0)
                        Inicio_Extraccion = Cont_Desplazamiento;
                    else
                        Inicio_Extraccion = Cont_Desplazamiento - 1;
                    Primer_Numero = false;
                }
                if (Mascara_Cuenta_Contable.Substring(Cont_Desplazamiento, 1) != "#")
                {
                    Fin_Extraccion = Cont_Desplazamiento;
                    if (Inicio_Extraccion == 0)
                    {
                        Cuenta_Contable_Con_Formato += Cuenta_Contable.Substring(Inicio_Extraccion, Fin_Extraccion - Inicio_Extraccion);
                        Caracteres_Extraidos_Cuenta_Contable = Fin_Extraccion - Inicio_Extraccion;
                    }
                    else
                    {
                        Cuenta_Contable_Con_Formato += Cuenta_Contable.Substring(Inicio_Extraccion, Fin_Extraccion - Inicio_Extraccion - 1);
                        Caracteres_Extraidos_Cuenta_Contable += Fin_Extraccion - Inicio_Extraccion - 1;
                    }
                    Primer_Numero = true;
                    Cuenta_Contable_Con_Formato += "-";
                }
            }
            if (Caracteres_Extraidos_Cuenta_Contable != Cuenta_Contable.Length)
            {
                Cuenta_Contable_Con_Formato += Cuenta_Contable.Substring(Caracteres_Extraidos_Cuenta_Contable, Cuenta_Contable.Length - Caracteres_Extraidos_Cuenta_Contable);
            }
            return Cuenta_Contable_Con_Formato;
        }
        catch (Exception ex)
        {
            throw new Exception("Validar_Mascara_Cuenta_Contable " + ex.Message.ToString(), ex);
        }
    }
    #endregion

    #region (Metodos Consulta)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Parametros
    /// DESCRIPCION : Consulta la mascara para la cuenta contable actual.
    /// PARAMETROS  : 
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 19/Septiembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private string Consulta_Parametros()
    {
        Cls_Cat_Con_Parametros_Negocio Rs_Consulta_Cat_Con_Parametros_Negocio = new Cls_Cat_Con_Parametros_Negocio(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Parametros; //Variable que obtendra los datos de la consulta 
        string Mascara_Cuenta_Contable; //Recibe la mascara contable actual.
        try
        {
            Session.Remove("Consulta_Parametros");
            Dt_Parametros = Rs_Consulta_Cat_Con_Parametros_Negocio.Consulta_Parametros();//Consulta los datos generales de las Cuentas Contables dados de alta en la BD
            Session["Consulta_Parametros"] = Dt_Parametros;
            Mascara_Cuenta_Contable = Dt_Parametros.Rows[0][0].ToString();
            return Mascara_Cuenta_Contable;
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Parametros" + ex.Message.ToString(), ex);
        }
    }
    #endregion

    #region (Metodos de Operacion [Alta - Modificar - Eliminar])
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta_PreCompromiso
    /// DESCRIPCION : Da de Alta el Compromiso con los datos proporcionados
    /// PARAMETROS  : 
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 13/Octubre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Alta_Compromiso()
    {
        try
        {
            Cls_Ope_Con_Compromisos_Negocio Rs_Ope_Con_Compromisos = new Cls_Ope_Con_Compromisos_Negocio(); //Variable de conexion con la capa de datos

            Rs_Ope_Con_Compromisos.P_Area_Funcional_ID = Cmb_Area_Funcional.SelectedValue.ToString();
            Rs_Ope_Con_Compromisos.P_Concepto = Txt_Concepto.Text;

            Rs_Ope_Con_Compromisos.P_Cuenta_Contable_ID = Txt_Cuenta_Contable_ID.Text;
            Rs_Ope_Con_Compromisos.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue.ToString();
            Rs_Ope_Con_Compromisos.P_Estatus = Cmb_Estatus.SelectedValue.ToString();
            Rs_Ope_Con_Compromisos.P_Fecha_Creo = String.Format("{0:dd/MMM/yy}", DateTime.Now).ToString();
            Rs_Ope_Con_Compromisos.P_Fuente_Financiamiento_ID = Cmb_Fte_Financiamiento.SelectedValue.ToString();
            Rs_Ope_Con_Compromisos.P_Monto_Comprometido = Txt_Monto_Comprometido.Text;
            Rs_Ope_Con_Compromisos.P_Partida_ID = Cmb_Partida.SelectedValue.ToString();
            Rs_Ope_Con_Compromisos.P_Proyecto_Programa_ID = Cmb_Programa.SelectedValue.ToString();
            Rs_Ope_Con_Compromisos.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
            switch (Cmb_Beneficiario_Tipo.SelectedIndex)
            {
                case 1:
                    Rs_Ope_Con_Compromisos.P_Contratista_ID = Cmb_Beneficiario_Nombre.SelectedValue.ToString();
                    break;
                case 2:
                    Rs_Ope_Con_Compromisos.P_Empleado_ID = Cmb_Beneficiario_Nombre.SelectedValue.ToString();
                    break;
                case 3:
                    Rs_Ope_Con_Compromisos.P_Proveedor_ID = Cmb_Beneficiario_Nombre.SelectedValue.ToString();
                    break;
                case 4:
                    Rs_Ope_Con_Compromisos.P_Nombre_Beneficiario = Txt_Beneficiario_Nombre.Text;
                    break;
            }

            Rs_Ope_Con_Compromisos.Alta_Compromisos();

            Limpia_Controles();
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Compromisos", "alert('El Alta del Compromiso fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Alta_Compromiso " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Compromiso
    /// DESCRIPCION : Modifica el Compromiso con los datos proporcionados por el usuario
    /// PARAMETROS  : 
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 17/Octubre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Modificar_Compromiso()
    {
        Cls_Ope_Con_Compromisos_Negocio Rs_Compromisos = new Cls_Ope_Con_Compromisos_Negocio(); //Variable de conexion hacia la capa de negocios
        try
        {
            Rs_Compromisos.P_Area_Funcional_ID = Cmb_Area_Funcional.SelectedValue.ToString();
            Rs_Compromisos.P_No_Compromiso  = Txt_No_Compromiso.Text;
            Rs_Compromisos.P_Concepto = Txt_Concepto.Text;
            Rs_Compromisos.P_Cuenta_Contable_ID = Txt_Cuenta_Contable_ID.Text;
            Rs_Compromisos.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue.ToString();
            Rs_Compromisos.P_Estatus = Cmb_Estatus.SelectedItem.Text.ToString();
            Rs_Compromisos.P_Fuente_Financiamiento_ID = Cmb_Fte_Financiamiento.SelectedValue.ToString();
            Rs_Compromisos.P_Monto_Comprometido = Txt_Monto_Comprometido.Text;
            Rs_Compromisos.P_Partida_ID = Cmb_Partida.SelectedValue.ToString();
            Rs_Compromisos.P_Proyecto_Programa_ID = Cmb_Programa.SelectedValue.ToString();
            Rs_Compromisos.P_Usuario_Modifico = Cls_Sessiones.Nombre_Empleado;

            Rs_Compromisos.Modificar_Compromisos(); //Modifica el registro en base a los datos proporcionados
            Limpia_Controles(); //Limpia los controles del modulo
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Compromisos", "alert('La modificación del Compromiso fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar_Poliza " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Eliminar_Compromiso
    /// DESCRIPCION : Elimina los datos de la Póliza que fue seleccionada por el Usuario
    /// PARAMETROS  : 
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 17/Octubre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Eliminar_Compromiso()
    {
        Cls_Ope_Con_Compromisos_Negocio Rs_Compromisos = new Cls_Ope_Con_Compromisos_Negocio(); //Variable de conexion hacia la capa de negocios
        try
        {
            Rs_Compromisos.P_No_Compromiso = Txt_No_Compromiso.Text;
            Rs_Compromisos.Eliminar_Compromisos();

            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            Limpia_Controles();

            Session.Remove("Consulta_Compromisos");
        }
        catch (Exception ex)
        {
            throw new Exception("Eliminar_Compromiso " + ex.Message.ToString(), ex);
        }
    }
    #endregion
    #endregion

    #region (Eventos)
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (Btn_Nuevo.ToolTip == "Nuevo")
            {
                Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                Limpia_Controles();           //Limpia los controles de la forma para poder introducir nuevos datos
                Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByText("PENDIENTE"));
            }
            else
            {
                //Valida los datos ingresados por el usuario.
                if (Validar_Datos_Compromiso())
                {
                    Alta_Compromiso();
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
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (Btn_Modificar.ToolTip == "Modificar")
            {
                if (Txt_No_Compromiso.Text != "")
                {
                    Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                    Cmb_Unidad_Responsable.Enabled = true;
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el Compromiso que desea modificar <br>";
                }
            }
            else
            {
                //Valida los datos ingresados por el usuario.
                if (Validar_Datos_Compromiso())
                {
                    Modificar_Compromiso();
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
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            if (Txt_No_Compromiso.Text != "")
            {
                Eliminar_Compromiso();
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Seleccione el Compromiso que desea eliminar. <br>";
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
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
    protected void Cmb_Unidad_Responsable_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_SAP_Dep_Presupuesto_Negocio Rs_Consulta_Fuentes = new Cls_Ope_SAP_Dep_Presupuesto_Negocio(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Fuentes; //Variable que obtendra los datos de la consulta 
        string[] Fuentes_ID;
        int Cont_Fuentes = 0;
        try
        {
            Rs_Consulta_Fuentes.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue.ToString();
            Rs_Consulta_Fuentes.P_Partida_ID = Cmb_Partida.SelectedValue.ToString();
            Dt_Fuentes = Rs_Consulta_Fuentes.Consulta_Dependencia_Programa_ID();
            Fuentes_ID = new string[Dt_Fuentes.Rows.Count];
            foreach (DataRow Registro in Dt_Fuentes.Rows)
            {
                Fuentes_ID[Cont_Fuentes] = Registro[Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID].ToString();
                Cont_Fuentes++;
            }
            Llenar_Cmb_Proy_Prog(Fuentes_ID, Cont_Fuentes, false);
            Cmb_Programa.Enabled = true;
        }
        catch (Exception ex)
        {
            throw new Exception("Cmb_Unidad_Responsable_SelectedIndexChanged " + ex.Message.ToString(), ex);
        }
    }
    protected void Cmb_Fte_Financiamiento_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_SAP_Dep_Presupuesto_Negocio Rs_Area_Funcional = new Cls_Ope_SAP_Dep_Presupuesto_Negocio();
        DataTable Dt_Area_Funcional = null; //Variable que obtendra los datos de la consulta 
        string[] Area_Funcional_ID;
        int Cont_IDs = 0;

        try
        {
            Rs_Area_Funcional.P_Fuente_Financiamiento_ID = Cmb_Fte_Financiamiento.SelectedValue.ToString();
            Rs_Area_Funcional.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue.ToString();
            Rs_Area_Funcional.P_Programa_ID = Cmb_Programa.SelectedValue.ToString();
            Dt_Area_Funcional = Rs_Area_Funcional.Consulta_Fte_Area_Funcional_ID();
            Area_Funcional_ID = new string[Dt_Area_Funcional.Rows.Count];
            foreach (DataRow Registro in Dt_Area_Funcional.Rows)
            {
                Area_Funcional_ID[Cont_IDs] = Registro[Cat_Com_Dep_Presupuesto.Campo_Area_Funcional_ID].ToString();
                Cont_IDs++;
            }

            Llenar_Cmb_Area_Funcional(Area_Funcional_ID, Cont_IDs, false);
            Cmb_Area_Funcional.Enabled = true;
        }
        catch (Exception ex)
        {
            throw new Exception("Cmb_Unidad_Responsable_SelectedIndexChanged " + ex.Message.ToString(), ex);
        }
    }
    protected void Cmb_Programa_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_SAP_Dep_Presupuesto_Negocio Rs_Consulta_Fuente = new Cls_Ope_SAP_Dep_Presupuesto_Negocio();
        DataTable Dt_Fuentes = null; //Variable que obtendra los datos de la consulta 
        string[] Fuentes_ID;
        int Cont_IDs = 0;

        try
        {
            Rs_Consulta_Fuente.P_Programa_ID = Cmb_Programa.SelectedValue.ToString();
            Rs_Consulta_Fuente.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue.ToString();
            Dt_Fuentes = Rs_Consulta_Fuente.Consulta_Programa_Fuente_ID();
            Fuentes_ID = new string[Dt_Fuentes.Rows.Count];
            foreach (DataRow Registro in Dt_Fuentes.Rows)
            {
                Fuentes_ID[Cont_IDs] = Registro[Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID].ToString();
                Cont_IDs++;
            }

            Llenar_Cmb_Fuentes_Financiamiento(Fuentes_ID, Cont_IDs, false);
            Cmb_Fte_Financiamiento.Enabled = true;
        }
        catch (Exception ex)
        {
            throw new Exception("Cmb_Unidad_Responsable_SelectedIndexChanged " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Txt_Cuenta_Contable_TextChanged
    /// DESCRIPCION : consulta la informacion de la cuenta contable ingresada
    ///               proporcionando el usuario
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 11/Octubre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Txt_Cuenta_Contable_TextChanged(object sender, EventArgs e)
    {
        Cls_Cat_Con_Cuentas_Contables_Negocio Rs_Consulta_Con_Cuentas_Contables = new Cls_Cat_Con_Cuentas_Contables_Negocio(); //Variable de conexión a la capa de negocios
        DataTable Dt_Cuenta_Contable = null;    //Almacena los datos de la consulta a las cuentas contables.
        Cls_Cat_SAP_Partidas_Especificas_Negocio Rs_Consulta_Partidas_Especificas = new Cls_Cat_SAP_Partidas_Especificas_Negocio();
        DataTable Dt_Partidas_Especificas = null;   //Almacena la partida especifica asociada
        Cls_Ope_SAP_Dep_Presupuesto_Negocio Rs_Consulta_Presupuesto = new Cls_Ope_SAP_Dep_Presupuesto_Negocio();
        DataTable Dt_Presupuesto = null;    //Almacena el ID de la Dependencia asociada
        DataTable Dt_Montos_Presupuesto = null;
        string[] Dependencias_ID;
        int Cont_Dependencias = 0;

        try
        {
            Cmb_Area_Funcional.Items.Clear();
            Cmb_Fte_Financiamiento.Items.Clear();
            Cmb_Partida.Items.Clear();
            Cmb_Programa.Items.Clear();
            Cmb_Unidad_Responsable.Items.Clear();

            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Rs_Consulta_Con_Cuentas_Contables.P_Cuenta = Txt_Cuenta_Contable.Text;
            Dt_Cuenta_Contable = Rs_Consulta_Con_Cuentas_Contables.Consulta_Existencia_Cuenta_Contable();

            if (Dt_Cuenta_Contable.Rows.Count > 0)
                Txt_Cuenta_Contable_ID.Text = Dt_Cuenta_Contable.Rows[0][0].ToString();

            Rs_Consulta_Partidas_Especificas.P_Cuenta = Txt_Cuenta_Contable.Text;
            Dt_Partidas_Especificas = Rs_Consulta_Partidas_Especificas.Consulta_Partida_Especifica();
            Cmb_Partida.Items.Insert(0, new ListItem("<- Seleccione ->", ""));

            if (Dt_Partidas_Especificas.Rows.Count > 0)
            {
                foreach (DataRow Registro in Dt_Partidas_Especificas.Rows)
                {
                    Cmb_Partida.Items.Insert(1, new ListItem(Registro[Cat_Sap_Partidas_Especificas.Campo_Clave].ToString() + " - " + Registro[Cat_Sap_Partidas_Especificas.Campo_Nombre].ToString(), Registro[Cat_Sap_Partidas_Especificas.Campo_Partida_ID].ToString()));
                    Cmb_Partida.SelectedIndex = 1;
                    Rs_Consulta_Presupuesto.P_Partida_ID = Registro[Cat_Com_Dep_Presupuesto.Campo_Partida_ID].ToString();
                    Dt_Presupuesto = Rs_Consulta_Presupuesto.Consulta_Dependencia_Partida_ID();
                    Dt_Montos_Presupuesto = Rs_Consulta_Presupuesto.Consulta_Datos_Presupuestos();
                }
            }
            if (Dt_Presupuesto.Rows.Count > 0)
            {
                Dependencias_ID = new string[Dt_Presupuesto.Rows.Count];
                foreach (DataRow Registro in Dt_Presupuesto.Rows)
                {
                    Dependencias_ID[Cont_Dependencias] = Registro[Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID].ToString();
                    Cont_Dependencias++;
                }
                Llenar_Cmb_Dependencia(Dependencias_ID, Cont_Dependencias, false);
            }
            if (Dt_Montos_Presupuesto.Rows.Count > 0)
            {
                double Monto_Disponible = 0.0;
                foreach (DataRow Registro in Dt_Montos_Presupuesto.Rows)
                {
                    Monto_Disponible += Convert.ToDouble(Registro["MONTO_DISPONIBLE"].ToString());
                }
                Txt_Monto_Disponible.Text = "" + Monto_Disponible;
            }

            Cmb_Unidad_Responsable.Enabled = true;
            Cmb_Partida.Enabled = true;
            Txt_Monto_Comprometido.Focus();
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    protected void Cmb_Busqueda_Dependencia_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_SAP_Dep_Presupuesto_Negocio Rs_Consulta_Presupuesto = new Cls_Ope_SAP_Dep_Presupuesto_Negocio(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Presupuesto; //Variable que obtendra los datos de la consulta 
        string[] Presupuesto_ID;
        int Cont_Presupuesto = 0;
        try
        {
            Rs_Consulta_Presupuesto.P_Dependencia_ID = Cmb_Busqueda_Dependencia.SelectedValue.ToString();
            Rs_Consulta_Presupuesto.P_Partida_ID = Cmb_Busqueda_Partida.SelectedValue.ToString();
            Dt_Presupuesto = Rs_Consulta_Presupuesto.Consulta_Dependencia_Programa_ID();
            Presupuesto_ID = new string[Dt_Presupuesto.Rows.Count];
            foreach (DataRow Registro in Dt_Presupuesto.Rows)
            {
                Presupuesto_ID[Cont_Presupuesto] = Registro[Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID].ToString();
                Cont_Presupuesto++;
            }
            Llenar_Cmb_Proy_Prog(Presupuesto_ID, Cont_Presupuesto, true);
            Mpe_Busqueda_Compromisos.Show();
        }
        catch (Exception ex)
        {
            throw new Exception("Cmb_Unidad_Responsable_SelectedIndexChanged " + ex.Message.ToString(), ex);
        }
    }
    protected void Cmb_Busqueda_Financiamiento_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_SAP_Dep_Presupuesto_Negocio Rs_Area_Funcional = new Cls_Ope_SAP_Dep_Presupuesto_Negocio();
        DataTable Dt_Area_Funcional = null; //Variable que obtendra los datos de la consulta 
        string[] Area_Funcional_ID;
        int Cont_IDs = 0;

        try
        {
            Rs_Area_Funcional.P_Fuente_Financiamiento_ID = Cmb_Busqueda_Financiamiento.SelectedValue.ToString();
            Rs_Area_Funcional.P_Dependencia_ID = Cmb_Busqueda_Dependencia.SelectedValue.ToString();
            Rs_Area_Funcional.P_Programa_ID = Cmb_Busqueda_Proyectos.SelectedValue.ToString();
            Dt_Area_Funcional = Rs_Area_Funcional.Consulta_Fte_Area_Funcional_ID();
            Area_Funcional_ID = new string[Dt_Area_Funcional.Rows.Count];
            foreach (DataRow Registro in Dt_Area_Funcional.Rows)
            {
                Area_Funcional_ID[Cont_IDs] = Registro[Cat_Com_Dep_Presupuesto.Campo_Area_Funcional_ID].ToString();
                Cont_IDs++;
            }

            Llenar_Cmb_Area_Funcional(Area_Funcional_ID, Cont_IDs, true);
            Mpe_Busqueda_Compromisos.Show();
        }
        catch (Exception ex)
        {
            throw new Exception("Cmb_Unidad_Responsable_SelectedIndexChanged " + ex.Message.ToString(), ex);
        }
    }
    protected void Cmb_Busqueda_Proyectos_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_SAP_Dep_Presupuesto_Negocio Rs_Consulta_Fuente = new Cls_Ope_SAP_Dep_Presupuesto_Negocio();
        DataTable Dt_Fuentes = null; //Variable que obtendra los datos de la consulta 
        string[] Fuentes_ID;
        int Cont_IDs = 0;

        try
        {
            Rs_Consulta_Fuente.P_Programa_ID = Cmb_Busqueda_Proyectos.SelectedValue.ToString();
            Rs_Consulta_Fuente.P_Dependencia_ID = Cmb_Busqueda_Dependencia.SelectedValue.ToString();
            Dt_Fuentes = Rs_Consulta_Fuente.Consulta_Programa_Fuente_ID();
            Fuentes_ID = new string[Dt_Fuentes.Rows.Count];
            foreach (DataRow Registro in Dt_Fuentes.Rows)
            {
                Fuentes_ID[Cont_IDs] = Registro[Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID].ToString();
                Cont_IDs++;
            }

            Llenar_Cmb_Fuentes_Financiamiento(Fuentes_ID, Cont_IDs, true);
            Mpe_Busqueda_Compromisos.Show();
        }
        catch (Exception ex)
        {
            throw new Exception("Cmb_Unidad_Responsable_SelectedIndexChanged " + ex.Message.ToString(), ex);
        }
    }
    protected void Txt_Cuenta_Contable_PopUp_TextChanged(object sender, EventArgs e)
    {
        Cls_Cat_Con_Cuentas_Contables_Negocio Rs_Consulta_Con_Cuentas_Contables = new Cls_Cat_Con_Cuentas_Contables_Negocio(); //Variable de conexión a la capa de negocios
        Cls_Cat_SAP_Partidas_Especificas_Negocio Rs_Consulta_Partidas_Especificas = new Cls_Cat_SAP_Partidas_Especificas_Negocio();
        DataTable Dt_Partidas_Especificas = null;   //Almacena la partida especifica asociada
        Cls_Ope_SAP_Dep_Presupuesto_Negocio Rs_Consulta_Presupuesto = new Cls_Ope_SAP_Dep_Presupuesto_Negocio();
        DataTable Dt_Presupuesto = null;    //Almacena el ID de la Dependencia asociada
        string[] Dependencias_ID;
        int Cont_Dependencias = 0;

        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Rs_Consulta_Partidas_Especificas.P_Cuenta = Txt_Cuenta_Contable_PopUp.Text;
            Dt_Partidas_Especificas = Rs_Consulta_Partidas_Especificas.Consulta_Partida_Especifica();
            Cmb_Busqueda_Partida.Items.Insert(0, new ListItem("<- Seleccione ->", ""));

            if (Dt_Partidas_Especificas.Rows.Count > 0)
            {
                foreach (DataRow Registro in Dt_Partidas_Especificas.Rows)
                {
                    Cmb_Busqueda_Partida.Items.Insert(1, new ListItem(Registro[Cat_Sap_Partidas_Especificas.Campo_Clave].ToString() + " - " + Registro[Cat_Sap_Partidas_Especificas.Campo_Nombre].ToString(), Registro[Cat_Sap_Partidas_Especificas.Campo_Partida_ID].ToString()));
                    Cmb_Busqueda_Partida.SelectedIndex = 1;
                    Rs_Consulta_Presupuesto.P_Partida_ID = Registro[Cat_Com_Dep_Presupuesto.Campo_Partida_ID].ToString();
                    Dt_Presupuesto = Rs_Consulta_Presupuesto.Consulta_Dependencia_Partida_ID();
                }
            }
            if (Dt_Presupuesto.Rows.Count > 0)
            {
                Dependencias_ID = new string[Dt_Presupuesto.Rows.Count];
                foreach (DataRow Registro in Dt_Presupuesto.Rows)
                {
                    Dependencias_ID[Cont_Dependencias] = Registro[Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID].ToString();
                    Cont_Dependencias++;
                }
                Llenar_Cmb_Dependencia(Dependencias_ID, Cont_Dependencias, true);
            }
            Mpe_Busqueda_Compromisos.Show();
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    protected void Cmb_Beneficiario_Tipo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            if (Cmb_Beneficiario_Tipo.SelectedIndex == 4)
            {
                Cmb_Beneficiario_Nombre.Visible = false;
                Txt_Beneficiario_Nombre.Visible = true;
            }
            else
            {
                Cmb_Beneficiario_Nombre.Visible = true;
                Txt_Beneficiario_Nombre.Visible = false;
                Cmb_Beneficiario_Nombre.Enabled = true;

                if (Cmb_Beneficiario_Tipo.SelectedIndex == 3)
                {
                    Cls_Ope_Con_Compromisos_Negocio Rs_Proveedores = new Cls_Ope_Con_Compromisos_Negocio(); //Variable de conexion con la capa de negocios.
                    DataTable Dt_Proveedores = Rs_Proveedores.Consulta_Proveedores();   //Almacena los datos de la consulta.
                    int Items = 1;  //Contador para ingresar los items al DropDownList

                    Cmb_Beneficiario_Nombre.Items.Clear();
                    Cmb_Beneficiario_Nombre.Items.Insert(0, new ListItem("<- SELECCIONE ->", ""));
                    foreach (DataRow Registro in Dt_Proveedores.Rows)
                    {
                        Cmb_Beneficiario_Nombre.Items.Insert(Items, new ListItem(Registro["NOMBRE"].ToString(), Registro["PROVEEDOR_ID"].ToString()));
                        Items++;
                    }
                    Cmb_Beneficiario_Nombre.SelectedIndex = 0;
                }
                if (Cmb_Beneficiario_Tipo.SelectedIndex == 2)
                {
                    Cls_Cat_Empleados_Negocios Rs_Empleados = new Cls_Cat_Empleados_Negocios(); //Variable de conexion con la capa de negocios.
                    DataTable Dt_Empleados = Rs_Empleados.Consulta_Empleados();
                    int Items = 1;  //Contador para ingresar los items al DropDownList

                    Cmb_Beneficiario_Nombre.Items.Clear();
                    Cmb_Beneficiario_Nombre.Items.Insert(0, new ListItem("<- SELECCIONE ->", ""));
                    foreach (DataRow Registro in Dt_Empleados.Rows)
                    {
                        Cmb_Beneficiario_Nombre.Items.Insert(Items, new ListItem(Registro[Cat_Empleados.Campo_No_Empleado].ToString() + " - " + Registro["Empleado"].ToString(), Registro[Cat_Empleados.Campo_Empleado_ID].ToString()));
                        Items++;
                    }
                    Cmb_Beneficiario_Nombre.SelectedIndex = 0;
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
    protected void Txt_Monto_Comprometido_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            if (Convert.ToDouble(Txt_Monto_Comprometido.Text) > Convert.ToDouble(Txt_Monto_Disponible.Text))
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Txt_Monto_Comprometido.Text = "";
                Lbl_Mensaje_Error.Text = "El monto comprometido no puede exceder al monto disponible.";
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    protected void Cmb_Area_Funcional_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Cls_Ope_SAP_Dep_Presupuesto_Negocio Rs_Presupuesto = new Cls_Ope_SAP_Dep_Presupuesto_Negocio(); //Variable de conexion con la capa de Negocios.
            DataTable Dt_Presupuesto = null;    //Almacenara los registros encontrados en la tabla de Presupuestos.
            double Monto_Disponible = 0.0;

            Rs_Presupuesto.P_Partida_ID = Cmb_Partida.SelectedValue.ToString();
            Rs_Presupuesto.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue.ToString();
            Rs_Presupuesto.P_Fuente_Financiamiento_ID = Cmb_Fte_Financiamiento.SelectedValue.ToString();
            Rs_Presupuesto.P_Programa_ID = Cmb_Programa.SelectedValue.ToString();
            Dt_Presupuesto = Rs_Presupuesto.Consulta_Datos_Presupuestos();

            foreach (DataRow Registro in Dt_Presupuesto.Rows)
            {
                Monto_Disponible += Convert.ToDouble(Registro["MONTO_DISPONIBLE"].ToString());
            }
            Txt_Monto_Disponible.Text = "" + Monto_Disponible;
            Txt_Monto_Comprometido.Enabled = true;
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    #endregion

    #region (ModalPopup)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Cerrar_Ventana_Click
    /// DESCRIPCION : Cierra la ventana de busqueda de Compromisos.
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 22/Septiembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Cerrar_Ventana_Click(object sender, ImageClickEventArgs e)
    {
        Mpe_Busqueda_Compromisos.Hide();
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Busqueda_Poliza_Popup_Click
    /// DESCRIPCION : Ejecuta la busqueda de Compromisos
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 22/Septiembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Busqueda_Compromiso_Popup_Click(object sender, EventArgs e)
    {
        try
        {
            Cls_Ope_Con_Compromisos_Negocio Rs_Ope_Con_Compromisos = new Cls_Ope_Con_Compromisos_Negocio(); //Variable de conexion con la capa de datos
            DataTable Dt_Compromisos = null;    //Almacenara los datos arrojados por la consulta

            if (Cmb_Busqueda_Area_Funcional.SelectedIndex > 0)
                Rs_Ope_Con_Compromisos.P_Area_Funcional_ID = Cmb_Busqueda_Area_Funcional.SelectedValue.ToString();
            if (Cmb_Busqueda_Dependencia.SelectedIndex > 0)
                Rs_Ope_Con_Compromisos.P_Dependencia_ID = Cmb_Busqueda_Dependencia.SelectedValue.ToString();
            if (Cmb_Busqueda_Estatus.SelectedIndex > 0)
                Rs_Ope_Con_Compromisos.P_Estatus = Cmb_Busqueda_Estatus.SelectedValue.ToString();
            if (Cmb_Busqueda_Financiamiento.SelectedIndex > 0)
                Rs_Ope_Con_Compromisos.P_Fuente_Financiamiento_ID = Cmb_Busqueda_Financiamiento.SelectedValue.ToString();
            if (Cmb_Busqueda_Partida.SelectedIndex > 0)
                Rs_Ope_Con_Compromisos.P_Partida_ID = Cmb_Busqueda_Partida.SelectedValue.ToString();
            if (Cmb_Busqueda_Proyectos.SelectedIndex > 0)
                Rs_Ope_Con_Compromisos.P_Proyecto_Programa_ID = Cmb_Busqueda_Proyectos.SelectedValue.ToString();
            if (!String.IsNullOrEmpty(Txt_Cuenta_Contable_PopUp.Text))
            {
                Cls_Cat_Con_Cuentas_Contables_Negocio Rs_Cuenta_Contable = new Cls_Cat_Con_Cuentas_Contables_Negocio();
                DataTable Dt_Cuenta_Contable = null;   //Almacena la cuenta contable asociada
                Rs_Cuenta_Contable.P_Cuenta = Txt_Cuenta_Contable_PopUp.Text;
                Dt_Cuenta_Contable = Rs_Cuenta_Contable.Consulta_Datos_Cuentas_Contables();
                if (Dt_Cuenta_Contable.Rows.Count > 0)
                {
                    foreach (DataRow Registro in Dt_Cuenta_Contable.Rows)
                    {
                        Rs_Ope_Con_Compromisos.P_Cuenta_Contable_ID = Registro[Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID].ToString();
                    }
                }
            }

            Dt_Compromisos = Rs_Ope_Con_Compromisos.Consulta_Compromisos();
            if (Dt_Compromisos.Rows.Count > 0)
            {
                Session["Consulta_Compromisos"] = Dt_Compromisos;
                Llena_Grid_Compromisos();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    #endregion

    #region (Grid)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Compromisos_PageIndexChanging
    /// DESCRIPCION : Cambia la pagina de la tabla de Compromisos
    ///               
    /// PARAMETROS  : 
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 13/Octubre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Compromisos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Limpia_Controles();                        //Limpia todos los controles de la forma
            Grid_Compromisos.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
            Llena_Grid_Compromisos();                    //Carga los Compromisos que estan asignados a la página seleccionada
            Grid_Compromisos.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llena_Grid_Compromisos
    /// DESCRIPCION : Llena el grid con los Compromisos encontrados
    /// PARAMETROS  : 
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 13/Octubre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llena_Grid_Compromisos()
    {
        Cls_Cat_Dependencias_Negocio Rs_Dependencias = new Cls_Cat_Dependencias_Negocio();  //Variable para Dependencias.
        Cls_Cat_SAP_Fuente_Financiamiento_Negocio Rs_Financiamiento = new Cls_Cat_SAP_Fuente_Financiamiento_Negocio();  //Variable para Financiamiento.
        Cls_Cat_SAP_Area_Funcional_Negocio Rs_Area = new Cls_Cat_SAP_Area_Funcional_Negocio();  //Variable para el Area Funcional.
        Cls_Cat_Com_Proyectos_Programas_Negocio Rs_Proyectos = new Cls_Cat_Com_Proyectos_Programas_Negocio();   //Variable para los Proyectos.
        Cls_Cat_SAP_Partidas_Especificas_Negocio Rs_Partidas = new Cls_Cat_SAP_Partidas_Especificas_Negocio();  //Variable para las Partidas.
        Cls_Cat_Con_Cuentas_Contables_Negocio Rs_Cuentas = new Cls_Cat_Con_Cuentas_Contables_Negocio(); //Variable para las Cuentas Contables.
        DataTable Dt_Uso_Multiple = null;
        DataTable Dt_Compromisos; //Variable que obtendra los datos de la consulta 
        DataTable Dt_Busqueda = new DataTable();
        string Codigo_Programatico = "";

        try
        {
            Dt_Compromisos = (DataTable)Session["Consulta_Compromisos"];
            Dt_Busqueda.Columns.Add("NO_COMPROMISO", typeof(String));
            Dt_Busqueda.Columns.Add("CODIGO_PROGRAMATICO", typeof(String));
            Dt_Busqueda.Columns.Add(Ope_Con_Compromisos.Campo_Estatus, typeof(String));
            Dt_Busqueda.Columns.Add("CUENTA_CONTABLE", typeof(String));
            Dt_Busqueda.Columns.Add("MONTO", typeof(Double));

            foreach (DataRow Registro in Dt_Compromisos.Rows)
            {
                DataRow Agregar = Dt_Busqueda.NewRow();
                Agregar["NO_COMPROMISO"] = Registro["NO_COMPROMISO"].ToString();
                Agregar["MONTO"] = Registro["MONTO_COMPROMETIDO"].ToString();
                Agregar[Ope_Con_Compromisos.Campo_Estatus] = Registro[Ope_Con_Compromisos.Campo_Estatus].ToString();

                #region (Codigo_Programatico)
                Rs_Dependencias.P_Dependencia_ID = Registro[Cat_Dependencias.Campo_Dependencia_ID].ToString();
                Dt_Uso_Multiple = Rs_Dependencias.Consulta_Dependencias();
                if (Dt_Uso_Multiple.Rows.Count > 0)
                {
                    Codigo_Programatico += Dt_Uso_Multiple.Rows[0]["CLAVE"].ToString() + "-";
                    Dt_Uso_Multiple = null;
                }
                Rs_Proyectos.P_Proyecto_Programa_ID = Registro[Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID].ToString();
                Dt_Uso_Multiple = Rs_Proyectos.Consulta_Programas_Proyectos();
                if (Dt_Uso_Multiple.Rows.Count > 0)
                {
                    Codigo_Programatico += Dt_Uso_Multiple.Rows[0]["CLAVE"].ToString() + "-";
                    Dt_Uso_Multiple = null;
                }
                Rs_Financiamiento.P_Fuente_Financiamiento_ID = Registro[Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID].ToString();
                Dt_Uso_Multiple = Rs_Financiamiento.Consulta_Fuente_Financiamiento();
                if (Dt_Uso_Multiple.Rows.Count > 0)
                {
                    Codigo_Programatico += Dt_Uso_Multiple.Rows[0]["CLAVE"].ToString() + "-";
                    Dt_Uso_Multiple = null;
                }
                Rs_Area.P_Area_Funcional_ID = Registro[Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID].ToString();
                Dt_Uso_Multiple = Rs_Area.Consulta_Area_Funcional_Especial();
                if (Dt_Uso_Multiple.Rows.Count > 0)
                {
                    Codigo_Programatico += Dt_Uso_Multiple.Rows[0][0].ToString().Substring(0, 5) + "-";
                    Dt_Uso_Multiple = null;
                }
                Rs_Partidas.P_Partida_ID = Registro[Cat_Sap_Partidas_Especificas.Campo_Partida_ID].ToString();
                Dt_Uso_Multiple = Rs_Partidas.Consulta_Partida_Especifica();
                if (Dt_Uso_Multiple.Rows.Count > 0)
                {
                    Codigo_Programatico += Dt_Uso_Multiple.Rows[0][0].ToString();
                    Dt_Uso_Multiple = null;
                }
                //***************************************************************
                Rs_Cuentas.P_Cuenta_Contable_ID = Registro[Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID].ToString();
                Dt_Uso_Multiple = Rs_Cuentas.Consulta_Existencia_Cuenta_Contable();
                if (Dt_Uso_Multiple.Rows.Count > 0)
                {
                    Txt_Cuenta_Contable_ID.Text = Dt_Uso_Multiple.Rows[0]["CUENTA_CONTABLE_ID"].ToString();
                    if (Dt_Uso_Multiple.Rows[0]["CUENTA"].ToString().Contains("-"))
                        Agregar["CUENTA_CONTABLE"] = Dt_Uso_Multiple.Rows[0]["CUENTA"].ToString();
                    else
                        Agregar["CUENTA_CONTABLE"] = Aplicar_Mascara_Cuenta_Contable(Dt_Uso_Multiple.Rows[0]["CUENTA"].ToString());
                    Dt_Uso_Multiple = null;
                }
                #endregion
                Agregar["CODIGO_PROGRAMATICO"] = Codigo_Programatico;
                Dt_Busqueda.Rows.Add(Agregar);
                Dt_Busqueda.AcceptChanges();
                Codigo_Programatico = "";
            }
            Grid_Compromisos.DataSource = Dt_Busqueda;
            Grid_Compromisos.DataBind();
            Grid_Compromisos.Visible = true;
            Grid_Compromisos.SelectedIndex = -1;
            Mpe_Busqueda_Compromisos.Hide();
        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_Compromisos " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Compromisos_SelectedIndexChanged1
    /// DESCRIPCION : Consulta los datos del Compromiso seleccionado
    /// PARAMETROS  : 
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 13/Octubre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Compromisos_SelectedIndexChanged1(object sender, EventArgs e)
    {
        Cls_Ope_Con_Compromisos_Negocio Rs_Compromisos = new Cls_Ope_Con_Compromisos_Negocio(); //Variable de conexion con la capa de negocios.
        DataTable Dt_Compromisos_Detalles; //Variable que obtendra los datos de la consulta.
        Cls_Cat_Dependencias_Negocio Rs_Dependencias = new Cls_Cat_Dependencias_Negocio();  //Variable para Dependencias.
        Cls_Cat_SAP_Fuente_Financiamiento_Negocio Rs_Financiamiento = new Cls_Cat_SAP_Fuente_Financiamiento_Negocio();  //Variable para Financiamiento.
        Cls_Cat_SAP_Area_Funcional_Negocio Rs_Area = new Cls_Cat_SAP_Area_Funcional_Negocio();  //Variable para el Area Funcional.
        Cls_Cat_Com_Proyectos_Programas_Negocio Rs_Proyectos = new Cls_Cat_Com_Proyectos_Programas_Negocio();   //Variable para los Proyectos.
        Cls_Cat_SAP_Partidas_Especificas_Negocio Rs_Partidas = new Cls_Cat_SAP_Partidas_Especificas_Negocio();  //Variable para las Partidas.
        Cls_Cat_Con_Cuentas_Contables_Negocio Rs_Cuentas = new Cls_Cat_Con_Cuentas_Contables_Negocio(); //Variable para las Cuentas Contables.
        DataTable Dt_Uso_Multiple = null;   //Almacenara los datos de las diferentes consultas.
        string Codigo_Programatico = "";

        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Rs_Compromisos.P_No_Compromiso = Grid_Compromisos.SelectedRow.Cells[1].Text;
            Dt_Compromisos_Detalles = Rs_Compromisos.Consulta_Compromisos();

            if (Dt_Compromisos_Detalles.Rows.Count > 0)
            {
                foreach (DataRow Registro in Dt_Compromisos_Detalles.Rows)
                {
                    Txt_No_Compromiso.Text = Registro["NO_COMPROMISO"].ToString();
                    Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByText(Registro[Ope_Con_Compromisos.Campo_Estatus].ToString()));
                    Txt_Monto_Comprometido.Text = Registro[Ope_Con_Compromisos.Campo_Monto_Comprometido].ToString();
                    Txt_Concepto.Text = Registro[Ope_Con_Compromisos.Campo_Concepto].ToString();


                    //*****CUENTA CONTABLE******//
                    Rs_Cuentas.P_Cuenta_Contable_ID = Registro[Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID].ToString();
                    Dt_Uso_Multiple = Rs_Cuentas.Consulta_Existencia_Cuenta_Contable();
                    if (Dt_Uso_Multiple.Rows.Count > 0)
                    {
                        if (Dt_Uso_Multiple.Rows[0]["CUENTA"].ToString().Contains("-"))
                            Txt_Cuenta_Contable.Text = Dt_Uso_Multiple.Rows[0]["CUENTA"].ToString();
                        else
                            Txt_Cuenta_Contable.Text = Aplicar_Mascara_Cuenta_Contable(Dt_Uso_Multiple.Rows[0]["CUENTA"].ToString());
                    }

                    //******COMBOS*******//
                    Rs_Dependencias.P_Dependencia_ID = Registro[Cat_Dependencias.Campo_Dependencia_ID].ToString();
                    Dt_Uso_Multiple = Rs_Dependencias.Consulta_Dependencias();
                    if (Dt_Uso_Multiple.Rows.Count > 0)
                    {
                        Codigo_Programatico += Dt_Uso_Multiple.Rows[0]["CLAVE"].ToString() + "-";
                        Cmb_Unidad_Responsable.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
                        Cmb_Unidad_Responsable.Items.Insert(1, new ListItem(Dt_Uso_Multiple.Rows[0]["CLAVE_NOMBRE"].ToString(), Dt_Uso_Multiple.Rows[0]["DEPENDENCIA_ID"].ToString()));
                        Cmb_Unidad_Responsable.SelectedIndex = 1;
                        Dt_Uso_Multiple = null;
                    }
                    Rs_Proyectos.P_Proyecto_Programa_ID = Registro[Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID].ToString();
                    Dt_Uso_Multiple = Rs_Proyectos.Consulta_Programas_Proyectos();
                    if (Dt_Uso_Multiple.Rows.Count > 0)
                    {
                        Codigo_Programatico += Dt_Uso_Multiple.Rows[0]["CLAVE"].ToString() + "-";
                        Cmb_Programa.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
                        Cmb_Programa.Items.Insert(1, new ListItem(Dt_Uso_Multiple.Rows[0]["CLAVE"].ToString() + " - " + Dt_Uso_Multiple.Rows[0]["DESCRIPCION"].ToString(), Dt_Uso_Multiple.Rows[0]["PROYECTO_PROGRAMA_ID"].ToString()));
                        Cmb_Programa.SelectedIndex = 1;
                        Dt_Uso_Multiple = null;
                    }
                    Rs_Financiamiento.P_Fuente_Financiamiento_ID = Registro[Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID].ToString();
                    Dt_Uso_Multiple = Rs_Financiamiento.Consulta_Fuente_Financiamiento();
                    if (Dt_Uso_Multiple.Rows.Count > 0)
                    {
                        Codigo_Programatico += Dt_Uso_Multiple.Rows[0]["CLAVE"].ToString() + "-";
                        Cmb_Fte_Financiamiento.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
                        Cmb_Fte_Financiamiento.Items.Insert(1, new ListItem(Dt_Uso_Multiple.Rows[0]["CLAVE"].ToString() + " - " + Dt_Uso_Multiple.Rows[0]["DESCRIPCION"].ToString(), Dt_Uso_Multiple.Rows[0]["FUENTE_FINANCIAMIENTO_ID"].ToString()));
                        Cmb_Fte_Financiamiento.SelectedIndex = 1;
                        Dt_Uso_Multiple = null;
                    }
                    Rs_Area.P_Area_Funcional_ID = Registro[Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID].ToString();
                    Dt_Uso_Multiple = Rs_Area.Consulta_Area_Funcional_Especial();
                    if (Dt_Uso_Multiple.Rows.Count > 0)
                    {
                        Codigo_Programatico += Dt_Uso_Multiple.Rows[0][0].ToString().Substring(0, 5) + "-";
                        Cmb_Area_Funcional.Items.Insert(0, new ListItem("<- Seleccione ->"));
                        Cmb_Area_Funcional.Items.Insert(1, new ListItem(Dt_Uso_Multiple.Rows[0]["CLAVE_NOMBRE"].ToString(), Dt_Uso_Multiple.Rows[0]["AREA_FUNCIONAL_ID"].ToString()));
                        Cmb_Area_Funcional.SelectedIndex = 1;
                        Dt_Uso_Multiple = null;
                    }
                    Rs_Partidas.P_Partida_ID = Registro[Cat_Sap_Partidas_Especificas.Campo_Partida_ID].ToString();
                    Dt_Uso_Multiple = Rs_Partidas.Consulta_Partida_Especifica();
                    if (Dt_Uso_Multiple.Rows.Count > 0)
                    {
                        Codigo_Programatico += Dt_Uso_Multiple.Rows[0][0].ToString();
                        Cmb_Partida.Items.Insert(0, new ListItem("<- Seleccione ->"));
                        Cmb_Partida.Items.Insert(1, new ListItem(Dt_Uso_Multiple.Rows[0]["CLAVE"].ToString() + " - " + Dt_Uso_Multiple.Rows[0]["NOMBRE"].ToString(), Dt_Uso_Multiple.Rows[0]["PARTIDA_ID"].ToString()));
                        Cmb_Partida.SelectedIndex = 1;
                        Dt_Uso_Multiple = null;
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
    
}