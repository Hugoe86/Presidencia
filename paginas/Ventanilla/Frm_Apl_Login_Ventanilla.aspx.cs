using System;
using System.Data;
using System.Web.Security;
using System.Web.UI;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Ventanilla_Usarios.Negocio;

public partial class paginas_Ventanilla_Frm_Apl_Login_Ventanilla : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
        if (!Page.IsPostBack)
        {
            //Colocamos el foco en el primer control
            Txt_Usuario.Focus();
            Txt_Usuario.Text = "";
            Session.RemoveAll();
        }
    }

    #region Metodos
  

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Autentificar
    ///DESCRIPCIÓN: Metodo que permite validar el correo ingresado por el usuario
    ///PARAMETROS:  
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  01/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Autentificar()
    {
        Cls_Ven_Usuarios_Negocio Rs_Autentificar = new Cls_Ven_Usuarios_Negocio();
        DataTable Dt_Consulta = new DataTable();
        try
        {

            Rs_Autentificar.P_Email = Txt_Usuario.Text.Trim();
            Rs_Autentificar.P_Password = Txt_Password.Text.Trim();
            Dt_Consulta = Rs_Autentificar.Validar_Usuario();

            if (Dt_Consulta is DataTable)
            {
                if (Dt_Consulta.Rows.Count > 0)
                {
                    foreach (DataRow Registro in Dt_Consulta.Rows)
                    {
                        //Cls_Sessiones.Nombre_Empleado = Registro[Cat_Ven_Usuarios.Campo_Email].ToString();
                        //Cls_Sessiones.Rol_ID = "";
                        //Cls_Sessiones.Empleado_ID = Registro[Cat_Ven_Usuarios.Campo_Usuario_ID].ToString();                       
                        //Cls_Sessiones.No_Empleado = "";
                        //Cls_Sessiones.Dependencia_ID_Empleado = Registro[Cat_Ven_Usuarios.Campo_Usuario_ID].ToString();
                        //Cls_Sessiones.Area_ID_Empleado = "";

                        Cls_Sessiones.Nombre_Ciudadano = Registro[Cat_Pre_Contribuyentes.Campo_Nombre_Completo].ToString();
                        Cls_Sessiones.Rol_ID = "";
                        Cls_Sessiones.Ciudadano_ID = Registro[Cat_Pre_Contribuyentes.Campo_Contribuyente_ID].ToString();
                        Cls_Sessiones.Datos_Ciudadano = Dt_Consulta;                        
                    }

                    FormsAuthentication.Initialize();
                    FormsAuthentication.RedirectFromLoginPage(Convert.ToString(Session[Cls_Sessiones.Nombre_Ciudadano]), false);
                    Cls_Sessiones.Mostrar_Menu = true;
                    Response.Redirect("../Ventanilla/Frm_Apl_Ventanilla.aspx");

                }// fin del if .Rows.Count > 0

                else
                {
                    Txt_Usuario.Text = "";
                    Txt_Password.Text = "";
                    Txt_Usuario.Focus();
                    Lbl_Mensaje.Text = "El Usuario y Password no son correctos, favor de verificar";
                    Btn_Img_Mensaje.Style[HtmlTextWriterStyle.Visibility] = "Show";
                    Txt_Usuario.Focus();
                    return;
                }// fin del else
            }// fin del if principal
        }
        catch (Exception Ex)
        {
            String Mensaje = "Existe un problema con la conexión a la fuente de datos [" + Ex.ToString() + "]";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('" + Mensaje + "');", true);
        }
    }

    #endregion

    #region Eventos

    #region Botones
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Auntentificar_Login_Click
    ///DESCRIPCIÓN: Metodo que permite validar el correo ingresado por el usuario
    ///PARAMETROS:  
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  01/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Auntentificar_Login_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Txt_Usuario.Text != "" & Txt_Password.Text != "")
            {
                Autentificar();
            }
            else
            {
                if (Txt_Usuario.Text.Trim() == String.Empty)
                {
                    Lbl_Mensaje.Text = "Proporcione el Usuario para poder acceder al sistema";
                    Btn_Img_Mensaje.Style[HtmlTextWriterStyle.Visibility] = "Show";
                    Txt_Usuario.Focus();
                    return;
                }
                if (Txt_Password.Text.Trim() == String.Empty)
                {
                    Lbl_Mensaje.Text = "Proporcione la Contraseña para poder acceder al sistema";
                    Btn_Img_Mensaje.Style[HtmlTextWriterStyle.Visibility] = "Show";
                    Txt_Usuario.Focus();
                    return;
                }
            }    
        }
        catch (Exception Ex)
        {
            String Mensaje = "Existe un problema con la conexión a la fuente de datos [" + Ex.ToString() + "]";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('" + Mensaje + "');", true);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Folio_Click
    ///DESCRIPCIÓN: Metodo que permite buscar un tramite por numero de folio
    ///PARAMETROS:  
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  //2012 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_OnClick(object sender, EventArgs e)
    {
        try
        {
            Cls_Sessiones.Nombre_Empleado = "";
            Cls_Sessiones.Rol_ID = "";
            Cls_Sessiones.Empleado_ID = "";
            Cls_Sessiones.No_Empleado = "";
            Cls_Sessiones.Dependencia_ID_Empleado = "";
            Cls_Sessiones.Area_ID_Empleado = "";

            FormsAuthentication.Initialize();
            FormsAuthentication.RedirectFromLoginPage(Convert.ToString(Session[Cls_Sessiones.Nombre_Empleado]), false);
            Cls_Sessiones.Mostrar_Menu = true;
            Response.Redirect("../Ventanilla/Frm_Rpt_Ven_Consultar_Tramites.aspx");
        }
        catch (Exception Ex)
        {

        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Buscar_Folio_OnClick
    ///DESCRIPCIÓN: Abre la página para consulta de peticiones por folio
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 28-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Buscar_Folio_OnClick(object sender, EventArgs e)
    {
        try
        {
            Cls_Sessiones.Nombre_Empleado = "";
            Cls_Sessiones.Rol_ID = "";
            Cls_Sessiones.Empleado_ID = "";
            Cls_Sessiones.No_Empleado = "";
            Cls_Sessiones.Dependencia_ID_Empleado = "";
            Cls_Sessiones.Area_ID_Empleado = "";

            FormsAuthentication.Initialize();
            FormsAuthentication.RedirectFromLoginPage(Convert.ToString(Session[Cls_Sessiones.Nombre_Empleado]), false);
            Cls_Sessiones.Mostrar_Menu = true;
            Response.Redirect("../Ventanilla/Frm_Rpt_Ven_Consultar_Peticion.aspx");
        }
        catch
        {
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Tramites_Click
    ///DESCRIPCIÓN: Metodo que permite acceder a la pagina de tramites
    ///PARAMETROS:  
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  04/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Tramites_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Cls_Sessiones.Nombre_Empleado = "";
            Cls_Sessiones.Rol_ID = "";
            Cls_Sessiones.Empleado_ID = "";
            Cls_Sessiones.No_Empleado = "";
            Cls_Sessiones.Dependencia_ID_Empleado = "";
            Cls_Sessiones.Area_ID_Empleado = "";

            FormsAuthentication.Initialize();
            FormsAuthentication.RedirectFromLoginPage(Convert.ToString(Session[Cls_Sessiones.Nombre_Empleado]), false);
            Cls_Sessiones.Mostrar_Menu = true;
            Response.Redirect("../Ventanilla/Frm_Ope_Ven_Lista_Tramites.aspx");
        }
        catch (Exception Ex)
        {
            String Mensaje = "Existe un problema con la conexión a la fuente de datos [" + Ex.ToString() + "]";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('" + Mensaje + "');", true);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Atencion_Ciudadana_Click
    ///DESCRIPCIÓN: Metodo que permite acceder a la pagina de atencion ciudadana
    ///PARAMETROS:  
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  04/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Atencion_Ciudadana_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Sessiones.Nombre_Empleado = "";
        Cls_Sessiones.Rol_ID = "";
        Cls_Sessiones.Empleado_ID = "";
        Cls_Sessiones.No_Empleado = "";
        Cls_Sessiones.Dependencia_ID_Empleado = "";
        Cls_Sessiones.Area_ID_Empleado = "";

        FormsAuthentication.Initialize();
        FormsAuthentication.RedirectFromLoginPage(Convert.ToString(Session[Cls_Sessiones.Nombre_Empleado]), false);
        Cls_Sessiones.Mostrar_Menu = true;
        Response.Redirect("../Ventanilla/Frm_Ope_Ven_Registrar_Peticion.aspx");
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Listado_Vacantes_Click
    ///DESCRIPCIÓN: Metodo que permite acceder a la pagina de listado de vacantes
    ///PARAMETROS:  
    ///CREO:        Roberto González Oseguera
    ///FECHA_CREO:  04-jun-2012 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Listado_Vacantes_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("../Ventanilla/Frm_Ope_Ven_Listado_Vacantes.aspx");
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Pagos_OnClick
    ///DESCRIPCIÓN: Metodo que permite acceder a la pagina de listado de vacantes
    ///PARAMETROS:  
    ///CREO:        Roberto González Oseguera
    ///FECHA_CREO:  04-jun-2012 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Pagos_OnClick(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("../Ventanilla/Frm_Ope_Ven_Pagos_Internet.aspx");
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Notarios_OnClick
    ///DESCRIPCIÓN: Metodo que permite acceder a la pagina de listado de vacantes
    ///PARAMETROS:  
    ///CREO:        Antonio Salvador Benavides Guardado
    ///FECHA_CREO:  22/Septiembre/2015
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Notarios_OnClick(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("http://192.168.1.204:444/paginas/Paginas_Generales/Frm_Apl_Login_Notarios.aspx");
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Registrar_Ciudadano_OnClick
    ///DESCRIPCIÓN: Metodo que permite acceder a la pagina de registro de ciudadano
    ///PARAMETROS:  
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  31/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Registrar_Ciudadano_OnClick(object sender, EventArgs e)
    {
        try
        {
            Cls_Sessiones.Nombre_Ciudadano = "";
            Cls_Sessiones.Rol_ID = "";
            Cls_Sessiones.Ciudadano_ID = "";
            Cls_Sessiones.Datos_Ciudadano = new DataTable();

            FormsAuthentication.Initialize();
            FormsAuthentication.RedirectFromLoginPage(Convert.ToString(Session[Cls_Sessiones.Nombre_Empleado]), false);
            Cls_Sessiones.Mostrar_Menu = true;
            Response.Redirect("../Ventanilla/Frm_Cat_Ven_Registrar_Ciudadano.aspx");
        }
        catch (Exception Ex)
        {
            String Mensaje = "Existe un problema con la conexión a la fuente de datos [" + Ex.ToString() + "]";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('" + Mensaje + "');", true);
        }
    }
    #endregion

    #endregion



   
}
