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
using Presidencia.Constantes;
using Presidencia.Sessiones;
using AjaxControlToolkit;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using Presidencia.Cierre_Anual.Negocio;
using Presidencia.Polizas.Negocios;

public partial class paginas_Contabilidad_Frm_Ope_Con_Cierre_Anual : System.Web.UI.Page
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
    /// DESCRIPCIÓN: Habilita las operaciones que podrá realizar el usuario en la página.
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

    #region (Metodos Generales)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 24/Octubre/2011
    /// MODIFICO          : 
    /// FECHA_MODIFICO    : 
    /// CAUSA_MODIFICACION: 
    ///*******************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Limpia_Controles();             //Limpia los controles de la forma
            Habilitar_Controles("Inicial"); //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar
            Llenar_Cmb_Cuenta_Inicial();
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
    /// FECHA_CREO  : 24/Octubre/2011
    /// MODIFICO          : 
    /// FECHA_MODIFICO    : 
    /// CAUSA_MODIFICACION: 
    ///*******************************************************************************
    private void Limpia_Controles()
    {
        try
        {
            Cmb_Cuenta_Final.Items.Clear();
            Cmb_Cuenta_Inicial.Items.Clear();
            Txt_Anio.Text = "";
            Txt_Descripcion.Text = "";
            Txt_Nueva_Cuenta.Text = "";
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
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 11/Julio/2011
    /// MODIFICO          : Salvador L. Rea Ayala
    /// FECHA_MODIFICO    : 10/Octubre/2011
    /// CAUSA_MODIFICACION: Se agregaron los nuevos controles.
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
                    Habilitado = true;
                    Btn_Salir.ToolTip = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Configuracion_Acceso("Frm_Ope_Con_Cierre_Anual.aspx");
                    break;
            }

            Cmb_Cuenta_Inicial.Enabled = Habilitado;
            Cmb_Cuenta_Final.Enabled = !Habilitado;
            Txt_Nueva_Cuenta.Enabled = Habilitado;
            Txt_Anio.Text = "";
            Txt_Descripcion.Text = "";

            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos_Cierre_Anual
    /// DESCRIPCION : Valida que los datos necesarios se encuentren presentes
    /// PARAMETROS  : 
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 25/Octubre/2011
    /// MODIFICO          : 
    /// FECHA_MODIFICO    : 
    /// CAUSA_MODIFICACION: 
    ///*******************************************************************************
    private Boolean Validar_Datos_Cierre_Anual()
    {
        Boolean Datos_Validos = true;
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

        if (Cmb_Cuenta_Inicial.SelectedIndex == 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Cuenta Inicial. <br>";
            Datos_Validos = false;
        }
        if (Cmb_Cuenta_Final.SelectedIndex == 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Cuenta Final. <br>";
            Datos_Validos = false;
        }
        if (string.IsNullOrEmpty(Txt_Nueva_Cuenta.Text))
        {
            Lbl_Mensaje_Error.Text += "+ Nueva Cuenta. <br>";
            Datos_Validos = false;
        }
        if (string.IsNullOrEmpty(Txt_Anio.Text))
        {
            Lbl_Mensaje_Error.Text += "+ Año. <br>";
            Datos_Validos = false;
        }
        if (string.IsNullOrEmpty(Txt_Descripcion.Text))
        {
            Lbl_Mensaje_Error.Text += "+ Descripcion. <br>";
            Datos_Validos = false;
        }

        return Datos_Validos;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Obtener_Debe_Haber_Cuentas
    /// DESCRIPCION : Obtiene el total del debe y el haber del rango de cuentas seleccionado
    /// PARAMETROS  : 
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 26/Octubre/2011
    /// MODIFICO          : 
    /// FECHA_MODIFICO    : 
    /// CAUSA_MODIFICACION: 
    ///*******************************************************************************
    private double[] Obtener_Debe_Haber_Cuentas(DataRow Registro)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            double[] Totales = new double[2];   //Almacena el total del debe y el haber de las cuentas seleccionadas.
            Cls_Ope_Con_Polizas_Negocio Rs_Polizas = new Cls_Ope_Con_Polizas_Negocio(); //Variable de conexion con la capa de negocios.
            DataTable Dt_Polizas = null;    //Almacena los datos de la consulta.

            Rs_Polizas.P_Cuenta_Contable_ID = Registro["CUENTA_CONTABLE_ID"].ToString();
            Rs_Polizas.P_Mes_Inicio = "01" + Txt_Anio.Text.Substring(2, 2);
            Rs_Polizas.P_Mes_Fin = "12" + Txt_Anio.Text.Substring(2, 2);
            Dt_Polizas = Rs_Polizas.Consulta_Detalles_Poliza_Cuenta_Contable();
            Totales[0] = 0;
            Totales[1] = 0;

            foreach (DataRow Reg_Polizas in Dt_Polizas.Rows)
            {
                Totales[0] += Convert.ToDouble(Reg_Polizas["DEBE"].ToString());
                Totales[1] += Convert.ToDouble(Reg_Polizas["HABER"].ToString());
            }

            return Totales;
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
            return null;
        }
    }
    #endregion

    #region (Metodos)
    private void Llenar_Cmb_Cuenta_Inicial()
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Cls_Ope_Con_Cierre_Anual_Negocio Rs_Cuentas_Contables = new Cls_Ope_Con_Cierre_Anual_Negocio();   //Variable de conexion con la capa de negocios.
            DataTable Dt_Cuentas_Contables = null;
            int Items = 1;

            Dt_Cuentas_Contables = Rs_Cuentas_Contables.Consulta_Datos_Cuentas_Contables();
            Cmb_Cuenta_Inicial.Items.Clear();
            Cmb_Cuenta_Inicial.Items.Insert(0, new ListItem("<- SELECCIONE ->", ""));

            foreach (DataRow Registro in Dt_Cuentas_Contables.Rows)
            {
                Cmb_Cuenta_Inicial.Items.Insert(Items, new ListItem(Registro["CUENTA"].ToString() + " - " + Registro["DESCRIPCION"].ToString(), Registro["CUENTA"].ToString()));
                Items++;
            }
            Cmb_Cuenta_Inicial.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Llenar_Cmb_Cuenta_Inicial: " + ex.Message.ToString();
        }
    }
    #endregion

    #region (Eventos)
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (Btn_Salir.ToolTip == "Salir")
            {
                Session.Remove("Dt_Partidas_Poliza");
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
    protected void Cmb_Cuenta_Inicial_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Cls_Ope_Con_Cierre_Anual_Negocio Rs_Cuentas_Contables = new Cls_Ope_Con_Cierre_Anual_Negocio(); //Variable de conexcion con la capa de negocios.
            DataTable Dt_Cuentas = null;    //Almacena los datos de la consulta.
            int Items = 1;

            Cmb_Cuenta_Final.Enabled = true;
            Cmb_Cuenta_Final.Items.Clear();
            Rs_Cuentas_Contables.P_Cuenta_Inicial = Cmb_Cuenta_Inicial.SelectedValue.ToString();
            Rs_Cuentas_Contables.P_Cuenta_Contable_Rango = true;
            Dt_Cuentas = Rs_Cuentas_Contables.Consulta_Datos_Cuentas_Contables();
            Cmb_Cuenta_Final.Items.Insert(0, new ListItem("<- SELECCIONE ->", ""));

            foreach (DataRow Registro in Dt_Cuentas.Rows)
            {
                Cmb_Cuenta_Final.Items.Insert(Items, new ListItem(Registro["CUENTA"].ToString() + " - " + Registro["DESCRIPCION"].ToString(), Registro["CUENTA"].ToString()));
                Items++;
            }
            Cmb_Cuenta_Final.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Cmb_Cuenta_Inicial_SelectedIndexChanged: " + ex.Message.ToString();
        }
    }
    protected void Btn_Cierre_Anual_Click(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (Validar_Datos_Cierre_Anual())
            {
                Cls_Ope_Con_Cierre_Anual_Negocio Rs_Alta_Cierre = new Cls_Ope_Con_Cierre_Anual_Negocio();   //Variable de conexion con la capa de negocios.
                double[] Totales = new double[2];   //Recibe el total del debe y el haber del rango de cuentas seleccionadas.
                Cls_Ope_Con_Cierre_Anual_Negocio Rs_Cuentas_Contables = new Cls_Ope_Con_Cierre_Anual_Negocio(); //Variable de conexion con la capa de negocios.
                DataTable Dt_Cuentas = null;    //Almacena los datos de la consulta;

                Rs_Cuentas_Contables.P_Cuenta_Inicial = Cmb_Cuenta_Inicial.SelectedValue.ToString();
                Rs_Cuentas_Contables.P_Cuenta_Final = Cmb_Cuenta_Final.SelectedValue.ToString();
                Rs_Cuentas_Contables.P_Cuenta_Contable_Rango = true;
                Dt_Cuentas = Rs_Cuentas_Contables.Consulta_Datos_Cuentas_Contables();

                foreach (DataRow Registro in Dt_Cuentas.Rows)
                {
                    Totales = Obtener_Debe_Haber_Cuentas(Registro);
                    Rs_Alta_Cierre.P_Anio = Txt_Anio.Text;
                    Rs_Alta_Cierre.P_Cuenta_Contable_ID_Fin = Dt_Cuentas.Rows[Dt_Cuentas.Rows.Count - 1]["CUENTA_CONTABLE_ID"].ToString();
                    Rs_Alta_Cierre.P_Cuenta_Contable_ID_Inicio = Dt_Cuentas.Rows[0]["CUENTA_CONTABLE_ID"].ToString();
                    Rs_Alta_Cierre.P_Cuenta_Contable_ID = Registro["CUENTA_CONTABLE_ID"].ToString();
                    Rs_Alta_Cierre.P_Descripcion = Txt_Descripcion.Text;
                    Rs_Alta_Cierre.P_Diferencia = Totales[1] - Totales[0];
                    Rs_Alta_Cierre.P_Total_Debe = Totales[0];
                    Rs_Alta_Cierre.P_Total_Haber = Totales[1];
                    Rs_Alta_Cierre.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;

                    Rs_Alta_Cierre.Alta_Cierre_Mensual();
                }

                Inicializa_Controles(); //Inicializa los controles
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Cierre Anual", "alert('El Alta del Cierre Anual fue Exitosa');", true);
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Btn_Cierre_Anual_Click: " + ex.Message.ToString();
        }
    }
    protected void Txt_Nueva_Cuenta_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Cls_Ope_Con_Cierre_Anual_Negocio Rs_Cuentas_Contables = new Cls_Ope_Con_Cierre_Anual_Negocio(); //Variable de conexion con la capa de negocios.
            DataTable Dt_Cuentas = null;    //Almacena los datos de la consulta.

            Rs_Cuentas_Contables.P_Cuenta_Inicial = Txt_Nueva_Cuenta.Text;
            Dt_Cuentas = Rs_Cuentas_Contables.Consulta_Datos_Cuentas_Contables();

            if (Dt_Cuentas.Rows.Count == 0)
            {
                Txt_Nueva_Cuenta.Focus();
                Txt_Nueva_Cuenta.Text = "";
                throw new Exception("La nueva cuenta contable no existe, ingrese otra.");
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Txt_Nueva_Cuenta_TextChanged: " + Ex.Message.ToString();
        }
    }
    #endregion
}
