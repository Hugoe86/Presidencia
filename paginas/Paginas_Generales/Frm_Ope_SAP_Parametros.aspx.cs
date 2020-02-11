using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Data;
using Presidencia.Operacion_SAP_Parametros.Negocio;

public partial class paginas_Paginas_Generales_Frm_Ope_SAP_Parametros : System.Web.UI.Page
{
    #region Variables
    private const int Const_Estado_Inicial = 0;
    private const int Const_Estado_Nuevo = 1;
    private const int Const_Grid_Cotizador = 2;
    private const int Const_Estado_Modificar = 3;
    
    private static DataTable Dt_Parametros = new DataTable();

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
                Cargar_Datos();
            }
            Mensaje_Error();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
            Estado_Botones(Const_Estado_Inicial);
        }
    }

    #endregion

    #region Metodos

    #region Metodos ABC
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Modificar_Parametros
    ///DESCRIPCIÓN: Se modifican los parametros SAP en la base de datos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 20/Abril/2011 01:57:55 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private void Modificar_Parametros()
    {
        try
        {
            
            Cls_Ope_SAP_Parametros_Negocio Parametros_Negocio = new Cls_Ope_SAP_Parametros_Negocio();
            if (Validar_Campos())
            {
                Parametros_Negocio.P_Fondo = Txt_Fondo.Text.Trim();
                Parametros_Negocio.P_Division = Txt_Division.Text.Trim();
                Parametros_Negocio.P_Sociedad = Txt_Sociedad.Text.Trim();
                Parametros_Negocio.Modificar_Parametros();                
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Parametros SAP", "alert('La Modificación de los parametros fue Exitosa');", true);
                Estado_Botones(Const_Estado_Inicial);
                Cargar_Datos();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Modificar Parametros: " + Ex.Message);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos
    ///DESCRIPCIÓN: consulta los parametros SAP y los asigna a los controles
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 04/20/2011 04:37:26 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Datos()
    {
        try
        {

            Cls_Ope_SAP_Parametros_Negocio Parametros_Negocio = new Cls_Ope_SAP_Parametros_Negocio();
            Dt_Parametros = Parametros_Negocio.Consultar_Parametros();
            if (Dt_Parametros.Rows.Count > 0) 
            {
                foreach (DataRow Dt in Dt_Parametros.Rows)
                {
                    Txt_Division.Text = Dt[Ope_SAP_Parametros.Campo_Division].ToString();
                    Txt_Fondo.Text = Dt[Ope_SAP_Parametros.Campo_Fondo].ToString();
                    Txt_Sociedad.Text = Dt[Ope_SAP_Parametros.Campo_Sociedad].ToString();
                }
            }

        }
        catch(Exception Ex)
        {
            Mensaje_Error(Ex.Message);

        }

    }

    #endregion

    #region Metodos Generales
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
        Boolean Habilitado = false;
        switch (P_Estado)
        {
            case 0: //Estado inicial
                Habilitado = false;               

                
                Btn_Modificar.AlternateText = "Modificar";                
                Btn_Salir.AlternateText = "Inicio";
                
                Btn_Modificar.ToolTip = "Modificar";                
                Btn_Salir.ToolTip = "Inicio";
                
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";                
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                Configuracion_Acceso("Frm_Ope_SAP_Parametros.aspx");
                break;            

            case 3: //Modificar
                Habilitado = true;
                Btn_Modificar.Visible = true;               
                
                Btn_Modificar.AlternateText = "Actualizar";                
                Btn_Salir.AlternateText = "Cancelar";
                
                Btn_Modificar.ToolTip = "Actualizar";                
                Btn_Salir.ToolTip = "Cancelar";
                
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";                
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";

                break;
        }
        Txt_Division.Enabled = Habilitado;
        Txt_Fondo.Enabled = Habilitado;
        Txt_Sociedad.Enabled = Habilitado;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Validar_Campos
    ///DESCRIPCIÓN: revisa que los controles hallan sido llenados con los datos necesarios
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 04/20/2011 04:50:54 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Boolean Validar_Campos()
    {
        Boolean Resultado = true;

        if (Txt_Division.Text.Trim() == String.Empty || Txt_Division.Text.Trim() == "" || Txt_Division.Text.Trim()==null)
        {
            Mensaje_Error("El campo Division es necesario");
            Resultado = false;
        } if (Txt_Fondo.Text.Trim() == String.Empty || Txt_Fondo.Text.Trim() == "" || Txt_Fondo.Text.Trim() == null)
        {
            Mensaje_Error("El campo Fondo es necesario");
            Resultado = false;
        }
        if (Txt_Sociedad.Text.Trim() == String.Empty || Txt_Sociedad.Text.Trim() == "" || Txt_Sociedad.Text.Trim() == null)
        {
            Mensaje_Error("El campo Sociedad es necesario");
            Resultado = false;
        }
        return Resultado;
    }
    #endregion

    

    #endregion

    #region Eventos ABC
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Evento para modificar los parametros SAP en la base de datos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 20/Abril/2011 01:59:32 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

            if (Btn_Modificar.AlternateText == "Modificar")
            {
                Estado_Botones(Const_Estado_Modificar);
            }
            else if (Btn_Modificar.AlternateText == "Actualizar")
            {
                Modificar_Parametros();
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message.ToString());
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Evento para salir o cancelar la accion
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 20/Abril/2011 02:00:04 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Btn_Salir_Click()
    {

    }
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText == "Inicio")
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");

        }
        else
        {
            Estado_Botones(Const_Estado_Inicial);
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
            Botones.Add(Btn_Modificar);

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
