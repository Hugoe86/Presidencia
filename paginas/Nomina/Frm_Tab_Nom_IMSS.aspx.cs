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
using Presidencia.IMSS.Negocios;
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;

public partial class paginas_Nomina_Frm_Tab_Nom_IMSS : System.Web.UI.Page
{
    #region (Page Load)
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
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

    #region (Metodos)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 02-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Habilitar_Controles("Inicial"); //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar
            Limpia_Controles();             //Limpia los controles del forma
            Consulta_IMSS();                //Consulta todas los datos del IMSS que fueron dadas de alta en la BD
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
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 02-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpia_Controles()
    {
        try
        {
            Txt_IMSS_ID.Text = "";
            Txt_Porcentaje_Enfermedad_Maternidad_Especie_IMSS.Text = "";
            Txt_Porcentaje_Enfermedad_Maternidad_Pesos_IMSS.Text = "";
            Txt_Porcentaje_Invalidez_Vida_IMSS.Text = "";
            Txt_Porcentaje_Cesantia_Vejez_IMSS.Text = "";
            Txt_Comentarios_IMSS.Text = "";
            Txt_Excedente_SMG_DF.Text = "";
            Txt_Prestaciones_Dinero.Text = "";
            Txt_Gastos_Medicos.Text = "";
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
    /// FECHA_CREO  : 02-Septiembre-2010
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
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                    Configuracion_Acceso("Frm_Tab_Nom_IMSS.aspx");
                    break;

                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
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
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    break;
            }
            Txt_Porcentaje_Enfermedad_Maternidad_Especie_IMSS.Enabled = Habilitado;
            Txt_Porcentaje_Enfermedad_Maternidad_Pesos_IMSS.Enabled = Habilitado;
            Txt_Porcentaje_Invalidez_Vida_IMSS.Enabled = Habilitado;
            Txt_Porcentaje_Cesantia_Vejez_IMSS.Enabled = Habilitado;
            Txt_Excedente_SMG_DF.Enabled = Habilitado;
            Txt_Prestaciones_Dinero.Enabled = Habilitado;
            Txt_Gastos_Medicos.Enabled = Habilitado;
            Txt_Comentarios_IMSS.Enabled = Habilitado;
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_IMSS
    /// DESCRIPCION : Consulta los IMSS que estan dados de alta en la BD
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 02-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_IMSS()
    {
        Cls_Tab_Nom_IMSS_Negocio Rs_Consulta_Tab_Nom_IMSS = new Cls_Tab_Nom_IMSS_Negocio(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_IMSS; //Variable que obtendra los datos de la consulta 

        try
        {
            Dt_IMSS = Rs_Consulta_Tab_Nom_IMSS.Consulta_Datos_IMSS(); //Consulta todos los IMSS con sus datos generales
            if (Dt_IMSS.Rows.Count > 0)
            {
                //Asigna los valores de los campos obtenidos de la consulta anterior a los controles de la forma
                foreach (DataRow Registro in Dt_IMSS.Rows)
                {
                    Txt_IMSS_ID.Text = Registro["IMSS_ID"].ToString();
                    Txt_Porcentaje_Enfermedad_Maternidad_Especie_IMSS.Text = Registro[Tab_Nom_IMSS.Campo_Porcentaje_Enf_Mat_Esp].ToString();
                    Txt_Porcentaje_Enfermedad_Maternidad_Pesos_IMSS.Text = Registro[Tab_Nom_IMSS.Campo_Porcentaje_Enf_Mat_Pes].ToString();
                    Txt_Porcentaje_Invalidez_Vida_IMSS.Text = Registro[Tab_Nom_IMSS.Campo_Porcentaje_Invalidez_Vida].ToString();
                    Txt_Porcentaje_Cesantia_Vejez_IMSS.Text = Registro[Tab_Nom_IMSS.Campo_Porcentaje_Cesantia_Vejez].ToString();
                    Txt_Excedente_SMG_DF.Text = Registro[Tab_Nom_IMSS.Campo_Excendete_3_SMG_DF].ToString();
                    Txt_Prestaciones_Dinero.Text = Registro[Tab_Nom_IMSS.Campo_Prestaciones_Dinero].ToString();
                    Txt_Gastos_Medicos.Text = Registro[Tab_Nom_IMSS.Campo_Gastos_Medicos].ToString();
                    Txt_Comentarios_IMSS.Text = Registro[Tab_Nom_IMSS.Campo_Comentarios].ToString();
                }
            }
            Btn_Nuevo.Visible = true;
            Btn_Modificar.Visible = true;
            if (Txt_IMSS_ID.Text != "")
            {
                Btn_Nuevo.Visible = false;
            }
            else
            {
                Btn_Modificar.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_IMSS " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta_IMSS
    /// DESCRIPCION : Da de Alta el IMSS con los datos proporcionados por el usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 02-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Alta_IMSS()
    {
        Cls_Tab_Nom_IMSS_Negocio Rs_Alta_Tab_Nom_IMSS = new Cls_Tab_Nom_IMSS_Negocio(); //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta
        try
        {
            Rs_Alta_Tab_Nom_IMSS.P_Porcentaje_Enfermedad_Maternidad_Especie = Convert.ToDouble(Txt_Porcentaje_Enfermedad_Maternidad_Especie_IMSS.Text);
            Rs_Alta_Tab_Nom_IMSS.P_Porcentaje_Enfermedad_Maternidad_Pesos = Convert.ToDouble(Txt_Porcentaje_Enfermedad_Maternidad_Pesos_IMSS.Text);
            Rs_Alta_Tab_Nom_IMSS.P_Porcentaje_Invalidez_Vida = Convert.ToDouble(Txt_Porcentaje_Invalidez_Vida_IMSS.Text);
            Rs_Alta_Tab_Nom_IMSS.P_Porcentaje_Cesantia_Vejez = Convert.ToDouble(Txt_Porcentaje_Cesantia_Vejez_IMSS.Text);
            Rs_Alta_Tab_Nom_IMSS.P_Comentarios = Convert.ToString(Txt_Comentarios_IMSS.Text);
            Rs_Alta_Tab_Nom_IMSS.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;

            Rs_Alta_Tab_Nom_IMSS.Alta_IMSS(); //Da de alta los datos de el IMSS proporcionados por el usuario en la BD
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de IMSS", "alert('El Alta del IMSS fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Alta_IMSS " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_IMSS
    /// DESCRIPCION : Modifica los datos del IMSS con los proporcionados por el usuario en la BD
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 02-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Modificar_IMSS()
    {
        Cls_Tab_Nom_IMSS_Negocio Rs_Modificar_Tab_Nom_IMSS = new Cls_Tab_Nom_IMSS_Negocio(); //Variable de conexión hacia la capa de Negocios para envio de datos a modificar
        try
        {
            Rs_Modificar_Tab_Nom_IMSS.P_IMSS_ID = Convert.ToString(Txt_IMSS_ID.Text);
            Rs_Modificar_Tab_Nom_IMSS.P_Porcentaje_Enfermedad_Maternidad_Especie = Convert.ToDouble(Txt_Porcentaje_Enfermedad_Maternidad_Especie_IMSS.Text);
            Rs_Modificar_Tab_Nom_IMSS.P_Porcentaje_Enfermedad_Maternidad_Pesos = Convert.ToDouble(Txt_Porcentaje_Enfermedad_Maternidad_Pesos_IMSS.Text);
            Rs_Modificar_Tab_Nom_IMSS.P_Porcentaje_Invalidez_Vida = Convert.ToDouble(Txt_Porcentaje_Invalidez_Vida_IMSS.Text);
            Rs_Modificar_Tab_Nom_IMSS.P_Porcentaje_Cesantia_Vejez = Convert.ToDouble(Txt_Porcentaje_Cesantia_Vejez_IMSS.Text);
            Rs_Modificar_Tab_Nom_IMSS.P_Excendente_SMG_DF = Convert.ToDouble(Txt_Excedente_SMG_DF.Text);
            Rs_Modificar_Tab_Nom_IMSS.P_Prestaciones_Dinero = Convert.ToDouble(Txt_Prestaciones_Dinero.Text);
            Rs_Modificar_Tab_Nom_IMSS.P_Gastos_Medicos = Convert.ToDouble(Txt_Gastos_Medicos.Text);
            Rs_Modificar_Tab_Nom_IMSS.P_Comentarios = Convert.ToString(Txt_Comentarios_IMSS.Text);
            Rs_Modificar_Tab_Nom_IMSS.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;

            Rs_Modificar_Tab_Nom_IMSS.Modificar_IMSS(); //Sustituye los datos que se encuentran en la BD por lo que introdujo el usuario
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de IMSS", "alert('La Modificación del IMSS fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar_IMSS " + ex.Message.ToString(), ex);
        }
    }
    #endregion

    #region (Eventos)
    protected void Btn_Nuevo_Click(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (Btn_Nuevo.ToolTip == "Nuevo")
            {
                Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                Limpia_Controles();           //Limpia los controles de la forma para poder introducir nuevos datos
            }
            else
            {
                //Si todos los campos requeridos fueron proporcionados por el usuario entonces da de alta los mismo en la base de datos
                if (Txt_Porcentaje_Enfermedad_Maternidad_Especie_IMSS.Text != "" & Txt_Porcentaje_Enfermedad_Maternidad_Pesos_IMSS.Text != "" & Txt_Porcentaje_Invalidez_Vida_IMSS.Text != "" & Txt_Porcentaje_Cesantia_Vejez_IMSS.Text != "" & Txt_Comentarios_IMSS.Text.Length <= 250)
                {
                    Alta_IMSS(); //Da de alta los datos proporcionados por el usuario
                }
                //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                    if (Txt_Porcentaje_Enfermedad_Maternidad_Especie_IMSS.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Porcentaje de Enfermedad (Maternidad en Especie) <br>";
                    }
                    if (Txt_Porcentaje_Enfermedad_Maternidad_Pesos_IMSS.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Porcentaje de Enfermedad (Maternidad en Pesos) <br>";
                    }
                    if (Txt_Porcentaje_Invalidez_Vida_IMSS.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Porcentaje de Invalidez de Vida <br>";
                    }
                    if (Txt_Porcentaje_Cesantia_Vejez_IMSS.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Porcentaje de Cesantia para la Vejez <br>";
                    }
                    if (Txt_Comentarios_IMSS.Text.Length > 250)
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Los comentarios proporcionados no deben ser mayor a 250 caracteres <br>";
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
    protected void Btn_Modificar_Click(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (Btn_Modificar.ToolTip == "Modificar")
            {
                if (Txt_IMSS_ID.Text != "")
                {
                    Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Debe dar de Alta los datos del IMSS antes de modificar <br>";
                }
            }
            else
            {
                //Si todos los campos requeridos fueron proporcionados por el usuario entonces modifica estos en la BD
                if (Txt_Porcentaje_Enfermedad_Maternidad_Especie_IMSS.Text != "" & Txt_Porcentaje_Enfermedad_Maternidad_Pesos_IMSS.Text != "" & Txt_Porcentaje_Invalidez_Vida_IMSS.Text != "" & Txt_Porcentaje_Cesantia_Vejez_IMSS.Text != "" & Txt_Comentarios_IMSS.Text.Length <= 250)
                {
                    Modificar_IMSS(); //Modifica los datos del IMSS con los datos proporcionados por el usuario
                }
                //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                    if (Txt_Porcentaje_Enfermedad_Maternidad_Especie_IMSS.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Porcentaje de Enfermedad (Maternidad en Especie) <br>";
                    }
                    if (Txt_Porcentaje_Enfermedad_Maternidad_Pesos_IMSS.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Porcentaje de Enfermedad (Maternidad en Pesos) <br>";
                    }
                    if (Txt_Porcentaje_Invalidez_Vida_IMSS.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Porcentaje de Invalidez de Vida <br>";
                    }
                    if (Txt_Porcentaje_Cesantia_Vejez_IMSS.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Porcentaje de Cesantia para la Vejez <br>";
                    }
                    if (Txt_Comentarios_IMSS.Text.Length > 250)
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Los comentarios proporcionados no deben ser mayor a 250 caracteres <br>";
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
    protected void Btn_Salir_Click(object sender, EventArgs e)
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
