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
using Presidencia.Parametros.Negocios;
using Presidencia.Sessiones;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Presidencia.Constantes;

public partial class paginas_Paginas_Generales_Frm_Apl_Cat_Parametros : System.Web.UI.Page
{

    #region Variables

    private Cls_Apl_Cat_Parametros_Negocio Parametros_Negocio;

    #endregion

    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo que se ejecuta cuando se carga la pagina
    ///PARAMETROS: 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 05/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    protected void Page_Load(object sender, EventArgs e)
    {
        Txt_Password_Correo.Attributes.Add("value", Txt_Password_Correo.Text);
        Configuracion_Acceso("Frm_Apl_Cat_Parametros.aspx");
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Init
    ///DESCRIPCIÓN: Metodo que se ejecuta cuando se carga por primera vez el formulario
    ///PARAMETROS: 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 05/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Page_Init(object sender, EventArgs e)
    {
        Parametros_Negocio = new Cls_Apl_Cat_Parametros_Negocio();
        Habilitar_Componentes(false);
        Llenar_Cajas();
    }
    #endregion

    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Habilitar_Componentes
    ///DESCRIPCIÓN: Metodo que valida las cajas y los botones de acuerdo a su parametro Estado 
    ///PARAMETROS: 1.- Estado: es el estado para habilitar o no las cajas de texto 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 06/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public void Habilitar_Componentes(bool Estado) 
    {

        if (Estado == true)
        {
            Txt_Correo_Saliente.Enabled = Estado;
            Txt_Servidor_Correo.Enabled = Estado;
            Txt_Usuario_Correo.Enabled = Estado;
            Txt_Password_Correo.Enabled = Estado;
            //Boton de Modificar
            Btn_Modificar.ToolTip = "Actualizar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
            //Boton Salir
            Btn_Salir.ToolTip = "Cancelar";
            Btn_Salir.Enabled = Estado;
            Btn_Salir.Visible = Estado;
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
        }
        else
        {
            Txt_Correo_Saliente.Enabled = Estado;
            Txt_Servidor_Correo.Enabled = Estado;
            Txt_Usuario_Correo.Enabled = Estado;
            Txt_Password_Correo.Enabled = Estado;
            //Boton Modificar
            Btn_Modificar.ToolTip = "Modificar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            //Boton Salir
            Btn_Salir.ToolTip = "Inicio";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";

        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos
    ///DESCRIPCIÓN: Metodo que carga los datos ingresados en las cajas a la clase de negocios.
    ///PARAMETROS: 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 07/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Cargar_Datos()
    {
        Parametros_Negocio.P_Correo_Saliente = Txt_Correo_Saliente.Text;
        Parametros_Negocio.P_Servidor_Correo = Txt_Servidor_Correo.Text;
        Parametros_Negocio.P_Usuario_Correo = Txt_Usuario_Correo.Text;
        Parametros_Negocio.P_Password_Correo = Txt_Password_Correo.Text;
        Parametros_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Cajas
    ///DESCRIPCIÓN: Metodo que llena las cajas de acuerdo a los datos de la tabla Apl_Cat_Parametros
    ///PARAMETROS: 1.- Estado: es el estado para habilitar o no las cajas de texto 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 06/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    
    public void Llenar_Cajas()
    {
        DataSet Parametros = Parametros_Negocio.Consulta_Parametros();
        //Asignamos los valores a cada caja de texto 
        Txt_Correo_Saliente.Text = Parametros.Tables[0].Rows[0].ItemArray[0].ToString();
        Txt_Servidor_Correo.Text = Parametros.Tables[0].Rows[0].ItemArray[1].ToString();
        Txt_Usuario_Correo.Text = Parametros.Tables[0].Rows[0].ItemArray[2].ToString();
        Txt_Password_Correo.Text = Parametros.Tables[0].Rows[0].ItemArray[3].ToString();

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Cajas
    ///DESCRIPCIÓN: Metodo que valida que las cajas tengan contenido. 
    ///PARAMETROS: 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 06/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Validar_Cajas()
    {

        if (Txt_Usuario_Correo.Text.Trim() == String.Empty)
        {
            Lbl_Mensaje_Error.Text += "El Usuario de Correo es obligatorio <br />";
            Div_Contenedor_Msj_Error.Visible = true;
        }
        if (Txt_Password_Correo.Text.Trim() == String.Empty)
        {
            Lbl_Mensaje_Error.Text += "El Password de Correo es obligatorio <br />";
            Div_Contenedor_Msj_Error.Visible = true;
        }


    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Email
    ///DESCRIPCIÓN: 
    ///PARAMETROS: Metodo que permite validar el correo ingresado por el usuario
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 07/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public void Validar_Email()
    {
       
        Regex Exp_Regular = new Regex("^\\w+([\\.-]?\\w+)*@\\w+([\\.-]?\\w+)*(\\.\\w{2,3})+$");
        Match  Comparar= Exp_Regular.Match(Txt_Correo_Saliente.Text);

        if (!Comparar.Success)
        {
            Lbl_Mensaje_Error.Text += "+ El contenido del Correo Saliente es incorrecto <br />";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_IP
    ///DESCRIPCIÓN: 
    ///PARAMETROS: Metodo que permite validar el correo ingresado por el usuario
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 07/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public void Validar_IP()
    {
        Regex Mascara_IP = new Regex(@"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){2}(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5]))$");
       
        Match Comparar = Mascara_IP.Match(Txt_Servidor_Correo.Text);

        if (!Comparar.Success)
        {
            Lbl_Mensaje_Error.Text += "+ El contenido de la direccion del Servidor de Correo es incorrecto <br />";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Confirmar_Password
    ///DESCRIPCIÓN: 
    ///PARAMETROS: Metodo que permite validar que en caso de modificar el password coincida este 
    ///             con el campo de confirmar password
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 11/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Confirmar_Password()
    {
        DataSet Data_Set = Parametros_Negocio.Consulta_Parametros();
        String Password_Actual = Data_Set.Tables[0].Rows[0].ItemArray[3].ToString();

        if (Password_Actual != Txt_Password_Correo.Text)
        {
            if (Txt_Password_Correo.Text != Txt_Confirmar_Password.Text)
            {
                Lbl_Mensaje_Error.Text += "+ Los Password deben coincidir <br />";
                Div_Contenedor_Msj_Error.Visible = true;
            }

        }//fin del If

    }
    


    #endregion
    
    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: 
    ///PARAMETROS: Evento del boton Modificar que valida los botones y llama el metodo 
    ///             de modificar registro en la clase de datos. 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 06/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
        if (Btn_Modificar.ToolTip == "Modificar")
        {
            Habilitar_Componentes(true);
            Llenar_Cajas();

        }else{
            //esto se ejecutara en caso de ser actualizacion 
            Validar_Cajas();
            Cargar_Datos();
            Validar_Email();
            Validar_IP();
            Confirmar_Password();
            if (Div_Contenedor_Msj_Error.Visible == false)
            {
                Parametros_Negocio.Modificar_Parametros();
                //Deshabilita las cajas de texto
                Habilitar_Componentes(false);
            }//fin del if 
        }

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: 
    ///PARAMETROS: Evento del boton Salir que valida los botones y regresa a la pantalla de inicio. 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 07/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.ToolTip == "Cancelar")
        {
            Habilitar_Componentes(false);
            Llenar_Cajas();
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = false;
        }
        else
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
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
