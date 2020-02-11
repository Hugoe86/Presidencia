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
using Presidencia.Polizas.Negocios;
using Presidencia.Tipo_Polizas.Negocios;
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
using Presidencia.Bitacora_Polizas.Negocio;
using Presidencia.Cierre_Mensual.Negocio;

public partial class paginas_Contabilidad_Frm_Ope_Con_Polizas : System.Web.UI.Page
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
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 11-Julio-2011
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
            Consultar_Cuentas_Contables_Tipo_Polizas(); //Consulta todas las Cuentas Contables y los tipos de pólizas que fueron dadas de alta en la BD
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
    /// FECHA_CREO  : 11-Julio-2011
    /// MODIFICO          : Salvador L. Rea Ayala
    /// FECHA_MODIFICO    : 10/Octubre/2011
    /// CAUSA_MODIFICACION: Se agregaron los nuevos controles para su correcto
    ///                     funcionamiento.
    ///*******************************************************************************
    private void Limpia_Controles()
    {
        try
        {
            Txt_No_Poliza.Text = "";
            Txt_Concepto_Poliza.Text = "";
            Txt_Fecha_Poliza.Text = "";
            Txt_Concepto_Partida.Text = "";
            Txt_Cuenta_Contable.Text = "";
            Txt_Debe_Partida.Text = "";
            Txt_Haber_Partida.Text = "";
            Txt_Total_Debe.Text = "";
            Txt_Total_Haber.Text = "";
            Txt_No_Partidas.Text = "";
            //Txt_Clave_Presupuestal.Text = "";
            //Txt_Nombre_Presupuestal.Text = "";
            Txt_Empleado_Autorizo.Text = "";
            Cmb_Descripcion.SelectedIndex = -1;
            Cmb_Tipo_Poliza.SelectedIndex = -1;
           // Cmb_Unidad_Responsable.Items.Clear();
           // Cmb_Programa.Items.Clear();
           // Cmb_Fuente_Financiamiento.Items.Clear();
           // Cmb_Area_Funcional.Items.Clear();
            Cmb_Nombre_Empleado.Items.Clear();

            Grid_Detalles_Poliza.DataSource = new DataTable();
            Grid_Detalles_Poliza.DataBind();
            Grid_Polizas.DataSource = new DataTable();
            Grid_Polizas.DataBind();

            if (Session["Dt_Partidas_Poliza"] != null)
            {
                Session.Remove("Dt_Partidas_Poliza");
            }
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
                    Habilitado = false;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Salir";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = true;
                    Btn_Copiar.Visible = true;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Copiar.CausesValidation = false;
                    Txt_Empleado_Creo.Enabled = false;
                    Btn_Password.Visible = false;
                   // Cmb_Empleado_Creo.Enabled = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Configuracion_Acceso("Frm_Ope_Con_Polizas.aspx");
                    break;

                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Eliminar.Visible = false;
                    Btn_Copiar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Txt_Empleado_Creo.Enabled  = false;    
                //Cmb_Empleado_Creo.Enabled = false;
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
                    Btn_Copiar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Txt_Empleado_Creo.Enabled  = false;    
                //Cmb_Empleado_Creo.Enabled = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    break;

                case "Carga":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Eliminar.Visible = false;
                    Btn_Copiar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Txt_Empleado_Creo.Enabled  = false;    
                //Cmb_Empleado_Creo.Enabled = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    break;
            }
            Txt_Concepto_Partida.Enabled = Habilitado;
            Txt_Concepto_Poliza.Enabled = Habilitado;
            Txt_Cuenta_Contable.Enabled = Habilitado;
            Txt_Debe_Partida.Enabled = Habilitado;
            Txt_Fecha_Poliza.Enabled = Habilitado;
            Txt_Haber_Partida.Enabled = Habilitado;
            //Txt_Beneficiario.Enabled = Habilitado;
            Txt_Empleado_Autorizo.Enabled = Habilitado;
            Cmb_Nombre_Empleado.Enabled = Habilitado;
            Btn_Agregar_Partida.Enabled = Habilitado;
            Cmb_Tipo_Poliza.Enabled = Habilitado;
            Cmb_Descripcion.Enabled = Habilitado;
            //Cmb_Area_Funcional.Enabled = Habilitado;
            //Cmb_Fuente_Financiamiento.Enabled = Habilitado;
            //Cmb_Programa.Enabled = Habilitado;
            //Cmb_Unidad_Responsable.Enabled = Habilitado;
            Grid_Detalles_Poliza.Enabled = Habilitado;

            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Montos_Debe_Haber_Poliza
    /// DESCRIPCION : Obtiene los montos totales del Debe y Haber de la póliza de acuerdo
    ///               a las partidas
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 11/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Montos_Debe_Haber_Poliza()
    {
        DataTable Dt_Partidas = (DataTable)Session["Dt_Partidas_Poliza"];
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Txt_Total_Haber.Text = "0";
            Txt_Total_Debe.Text = "0";
            Txt_No_Partidas.Text = Dt_Partidas.Rows.Count.ToString();

            //Suma todos los Debe y Haber de la póliza
            foreach (DataRow Registro in Dt_Partidas.Rows)
            {
                Txt_Total_Debe.Text = (Convert.ToDouble(Txt_Total_Debe.Text.ToString()) + Convert.ToDouble(Registro[Ope_Con_Polizas_Detalles.Campo_Debe].ToString())).ToString();
                Txt_Total_Haber.Text = (Convert.ToDouble(Txt_Total_Haber.Text.ToString()) + Convert.ToDouble(Registro[Ope_Con_Polizas_Detalles.Campo_Haber].ToString())).ToString();
            }
            Txt_Total_Debe.Text = String.Format("{0:#,###,##0.00}", Convert.ToDouble((String.IsNullOrEmpty(Txt_Total_Debe.Text.ToString()) ? "0" : Txt_Total_Debe.Text.ToString())));
            Txt_Total_Haber.Text = String.Format("{0:#,###,##0.00}", Convert.ToDouble((String.IsNullOrEmpty(Txt_Total_Haber.Text.ToString()) ? "0" : Txt_Total_Haber.Text.ToString())));
        }
        catch (Exception ex)
        {
            throw new Exception("Montos_Debe_Haber_Poliza " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos_Poliza
    /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 11/Julio/2011
    /// MODIFICO          : Salvador L. Rea Ayala
    /// FECHA_MODIFICO    : 10/Octubre/2011
    /// CAUSA_MODIFICACION: Se valido que la cuenta contable este en el formato
    ///                     correcto de acuerdo a la BD.
    ///*******************************************************************************
    private Boolean Validar_Datos_Poliza()
    {
        Boolean Datos_Validos = true;
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

        if (Cmb_Tipo_Poliza.SelectedIndex == 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Tipo Poliza <br>";
            Datos_Validos = false;
        }
        if (string.IsNullOrEmpty(Txt_Fecha_Poliza.Text) || (Txt_Fecha_Poliza.Text.Trim().Equals("__/___/____")))
        {
            Lbl_Mensaje_Error.Text += "+ La Fecha de la Póliza <br>";
            Datos_Validos = false;
        }
        if (String.IsNullOrEmpty(Txt_Concepto_Poliza.Text))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Concepto de la Poliza <br>";
            Datos_Validos = false;
        }
        if (String.IsNullOrEmpty(Txt_Empleado_Autorizo.Text))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Empleado que autoriza <br>";
            Datos_Validos = false;
        }
        if (String.IsNullOrEmpty(Txt_No_Partidas.Text))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Las Partidas de la Poliza <br>";
            Datos_Validos = false;
        }
        else
        {
            if (Convert.ToInt32(Txt_No_Partidas.Text.ToString()) == 0)
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Las Partidas de la Poliza <br>";
                Datos_Validos = false;
            }
        }
      // if (Txt_Clave_Presupuestal.Text != "")
       // {
        //    if (Cmb_Area_Funcional.Items.Count == 0)
         //   {
         //       Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Debe ingresar el codigo programatico. <br>";
         //       Datos_Validos = false;
         //   }
       // }

        //***************************************************
        string Mascara_Cuenta_Contable = Consulta_Parametros();
        string Texto = Txt_Cuenta_Contable.Text;
        if (Texto.Length == Mascara_Cuenta_Contable.Length)
        {
            for (int Cont_Caracteres = 0; Cont_Caracteres < Mascara_Cuenta_Contable.Length; Cont_Caracteres++)
            {
                if (Texto.Substring(Cont_Caracteres, 1) == "-")
                {
                    if (Mascara_Cuenta_Contable.Substring(Cont_Caracteres, 1) != Texto.Substring(Cont_Caracteres, 1))
                        Datos_Validos = false;
                }
                else
                {
                    if (Mascara_Cuenta_Contable.Substring(Cont_Caracteres, 1) != "#")
                        Datos_Validos = false;
                }
            }
        }
        //***************************************************
        return Datos_Validos;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Formato_Fecha
    /// DESCRIPCION : Valida el formato de las fechas.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 23/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Formato_Fecha(String Fecha)
    {
        String Cadena_Fecha = @"^(([0-9])|([0-2][0-9])|([3][0-1]))\/(ene|feb|mar|abr|may|jun|jul|ago|sep|oct|nov|dic)\/\d{4}$";
        if (Fecha != null)
        {
            return Regex.IsMatch(Fecha, Cadena_Fecha);
        }
        else
        {
            return false;
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Mascara_Cuenta_Contable
    /// DESCRIPCION : Validar que el formato de la mascara sea el correcto.
    /// PARAMETROS  : TEXTO: Recibe el valor contenido en la propiedad Text de la Txt_Cuenta_Contable
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 19/Septiembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Mascara_Cuenta_Contable(string Texto)
    {
        try
        {
            string Mascara_Cuenta_Contable = Consulta_Parametros(); //Almacena el formato actual que debe tener la cuenta contable.
            int Cont_Guiones = 0;   //Almacenara la cantidad de guiones presentes en la mascara.
            for (int Cont_Caracter = 0; Cont_Caracter < Mascara_Cuenta_Contable.Length; Cont_Caracter++)    //Ciclo para contar los guiones
            {
                if (Mascara_Cuenta_Contable.Substring(Cont_Caracter, 1) == "-")
                    Cont_Guiones++;
            }
            if (Texto.Length == Mascara_Cuenta_Contable.Length) //Valida que Texto y la Mascara sean del mismo tamaño
            {
                for (int Cont_Caracteres = 0; Cont_Caracteres < Mascara_Cuenta_Contable.Length; Cont_Caracteres++)  //Recorre Texto para compararlo con cada caracter de la mascara contable
                {
                    if (Texto.Substring(Cont_Caracteres, 1) == "-")
                    {
                        if (Mascara_Cuenta_Contable.Substring(Cont_Caracteres, 1) != Texto.Substring(Cont_Caracteres, 1))
                            throw new Exception("El formato de entrada de la cuenta contable no coincide con lo establecido en la mascara.");
                    }
                    else
                    {
                        if (Mascara_Cuenta_Contable.Substring(Cont_Caracteres, 1) != "#")
                            throw new Exception("El formato de entrada de la cuenta contable no coincide con lo establecido en la mascara.");
                    }
                }
                return (Boolean)true;
            }
            else if (Texto.Length == (Mascara_Cuenta_Contable.Length - Cont_Guiones))
            {
                return (Boolean)true;
            }
            else
            {
                throw new Exception("La Cuenta Contable no cumple con lo requerido.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Validar_Mascara_Cuenta_Contable " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Aplicar_Mascara_Cuenta_Contable
    /// DESCRIPCION : Aplica la Mascara a la Cuenta Contable
    /// PARAMETROS  : Cuenta_Contable: Recibe el numero de cuenta contable3613  1
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 20/Septiembre/2011
    /// MODIFICO          : sergio manuel gallardo andrade
    /// FECHA_MODIFICO    :3-noviembre-2011
    /// CAUSA_MODIFICACION:no funciona correctamente al aplicarle la mascara 
    ///*******************************************************************************
    private string Aplicar_Mascara_Cuenta_Contable(string Cuenta_Contable)
    {
        try
        {
            string Mascara_Cuenta_Contable = Consulta_Parametros(); //Consulta y almacena la mascara contable actual.
            string Cuenta_Contable_Con_Formato = "";    //Almacenara la cuenta contable ya estandarizada de acuerdo al formato.
            Boolean Primer_Numero = true;   //Detecta si el primer caracter es un numero.
            int Caracteres_Extraidos_Cuenta_Contable = 0;  //Variable que almacena la cantidad de caracteres extraidos de la cuenta.
            int contador_nuevo = 0;
            int Inicio_Extraccion = 0; //Variable que almacena el inicio de la cadena a extraer.
            int Fin_Extraccion = 0;    //Variable que almacena el fin de la cadena a extraer.
            for (int Cont_Desplazamiento = 0; Cont_Desplazamiento < Mascara_Cuenta_Contable.Length; Cont_Desplazamiento++)  //Ciclo de desplazamiento
            {
                if (Primer_Numero == true && Mascara_Cuenta_Contable.Substring(Cont_Desplazamiento, 1) == "#")  //Detecta el primer numero dentro de la mascara contable
                {
                    if (Cont_Desplazamiento == 0)
                        Inicio_Extraccion = Cont_Desplazamiento;
                    else
                        Inicio_Extraccion = contador_nuevo;
                    Primer_Numero = false;
                }
                if (Mascara_Cuenta_Contable.Substring(Cont_Desplazamiento, 1) != "#") //Detecta si el caracter es diferente de un numero en la mascara contable.
                {
                    Fin_Extraccion = Cont_Desplazamiento;
                    if (Inicio_Extraccion == 0)
                    {
                        Cuenta_Contable_Con_Formato += Cuenta_Contable.Substring(Inicio_Extraccion, Fin_Extraccion - Inicio_Extraccion);
                        Caracteres_Extraidos_Cuenta_Contable = Fin_Extraccion - Inicio_Extraccion;
                    }
                    else
                    {
                        Cuenta_Contable_Con_Formato += Cuenta_Contable.Substring(Inicio_Extraccion, Fin_Extraccion - Inicio_Extraccion - contador_nuevo);
                        Caracteres_Extraidos_Cuenta_Contable += Fin_Extraccion - Inicio_Extraccion - contador_nuevo;
                    }
                    if (contador_nuevo <Cuenta_Contable.Length)
                        {
                            contador_nuevo = contador_nuevo + 1;
                    }
                    Primer_Numero = true;
                    Cuenta_Contable_Con_Formato += "-";
                    
                }
            }
            if (Caracteres_Extraidos_Cuenta_Contable != Cuenta_Contable.Length) //Concatena los caracteres sobrantes en la cuenta contable.
            {
                Cuenta_Contable_Con_Formato += Cuenta_Contable.Substring(Caracteres_Extraidos_Cuenta_Contable, Cuenta_Contable.Length - Caracteres_Extraidos_Cuenta_Contable);
            }
            return Cuenta_Contable_Con_Formato;
        }
        catch (Exception ex)
        {
            throw new Exception("Aplicar_Mascara_Cuenta_Contable " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llenar_Cmb_Dependencia
    /// DESCRIPCION : Llena el ComboBox de acuerdo a la Partida
    /// PARAMETROS  : Dependencias_ID almacena los IDs
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 20/Septiembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    //private void Llenar_Cmb_Dependencia(string[] Dependencias_ID, int Cont_Dependencias)
    //{
    //    Cls_Cat_Dependencias_Negocio Rs_Consulta_Dependencias_Negocio = new Cls_Cat_Dependencias_Negocio(); //Variable de conexión hacia la capa de Negocios
    //    DataTable Dt_Dependencias; //Variable que obtendra los datos de la consulta 
    //    try
    //    {
    //        Cmb_Unidad_Responsable.Items.Clear();   //Elimina los Items dentro del Combo.
    //        Cmb_Unidad_Responsable.Items.Insert(0, new ListItem("<- Seleccione ->", ""));   //Inserta la opcion Seleccione en el Combo.
    //        for (int Cont_Desplazamiento = 0; Cont_Desplazamiento < Cont_Dependencias; Cont_Desplazamiento++)   //Ciclo de desplazamiento a traves del los IDs de las Dependencias.
    //        {
    //            Rs_Consulta_Dependencias_Negocio.P_Dependencia_ID = Dependencias_ID[Cont_Desplazamiento];
    //            Dt_Dependencias = Rs_Consulta_Dependencias_Negocio.Consulta_Dependencias();
    //            foreach (DataRow Registro in Dt_Dependencias.Rows)  //Inserta las dependencias encontradas al Combo.
    //                Cmb_Unidad_Responsable.Items.Insert(Cont_Desplazamiento + 1, new ListItem(Registro[Cat_Dependencias.Campo_Clave].ToString() + " - " + Registro[Cat_Dependencias.Campo_Nombre].ToString(), Registro[Cat_Dependencias.Campo_Dependencia_ID].ToString()));
    //        }
    //        Cmb_Unidad_Responsable.SelectedIndex = -1;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception("Llenar_Cmb_Dependencia " + ex.Message.ToString(), ex);
    //    }
    //}
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llenar_Cmb_Area_Funcional
    /// DESCRIPCION : Llena el ComboBox de acuerdo a la Fuente de Financiamiento
    /// PARAMETROS  : Areas_ID: almacena los IDs
    ///               Cont_Areas: almacena la cantidad de IDs que son
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 6/Octubre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    //private void Llenar_Cmb_Area_Funcional(string[] Areas_ID, int Cont_Areas)
    //{
    //    Cls_Ope_Con_Polizas_Negocio Rs_Consulta_Areas = new Cls_Ope_Con_Polizas_Negocio(); //Variable de conexión hacia la capa de Negocios
    //    DataTable Dt_Areas; //Variable que obtendra los datos de la consulta 
    //    try
    //    {
    //        Cmb_Area_Funcional.Items.Clear();   //Elimina los Items dentro del Combo.
    //        Cmb_Area_Funcional.Items.Insert(0, new ListItem("<- Seleccione ->", ""));   //Inserta la opcion Seleccione en el Combo.
    //        for (int Cont_Desplazamiento = 0; Cont_Desplazamiento < Cont_Areas; Cont_Desplazamiento++)  //Ciclo de desplazamiento a traves del los IDs de las Areas Funcionales
    //        {
    //            Rs_Consulta_Areas.P_Area_Funcional_ID = Areas_ID[Cont_Desplazamiento];
    //            Dt_Areas = Rs_Consulta_Areas.Consulta_Area_Funcional_Especial();
    //            foreach (DataRow Registro in Dt_Areas.Rows) //Inserta las areas funcionales encontradas al Combo.
    //                Cmb_Area_Funcional.Items.Insert(Cont_Desplazamiento + 1, new ListItem(Registro["CLAVE_NOMBRE"].ToString(), Registro[Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID].ToString()));
    //        }
    //        Cmb_Area_Funcional.SelectedIndex = -1;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception("Llenar_Cmb_Area_Funcional " + ex.Message.ToString(), ex);
    //    }
    //}
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llenar_Cmb_Proyectos_Programas
    /// DESCRIPCION : Llena el ComboBox de acuerdo a la Fuente de Financiamiento
    /// PARAMETROS  : Programas_ID: almacena los IDs
    ///               Cont_Programas: almacena la cantidad de IDs que son
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 6/Octubre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    //private void Llenar_Cmb_Proyectos_Programas(string[] Programas_ID, int Cont_Programas)
    //{
    //    Cls_Cat_Com_Proyectos_Programas_Negocio Rs_Consulta_Programas = new Cls_Cat_Com_Proyectos_Programas_Negocio(); //Variable de conexión hacia la capa de Negocios
    //    DataTable Dt_Programas; //Variable que obtendra los datos de la consulta 
    //    try
    //    {
    //        Cmb_Programa.Items.Clear(); //Elimina los Items dentro del Combo.
    //        Cmb_Programa.Items.Insert(0, new ListItem("<- Seleccione ->", "")); //Inserta la opcion Seleccione en el Combo.
    //        for (int Cont_Desplazamiento = 0; Cont_Desplazamiento < Cont_Programas; Cont_Desplazamiento++)  //Ciclo de desplazamiento a traves del los IDs de los Proyectos y Programas
    //        {
    //            Rs_Consulta_Programas.P_Proyecto_Programa_ID = Programas_ID[Cont_Desplazamiento];
    //            Dt_Programas = Rs_Consulta_Programas.Consulta_Programas_Proyectos();
    //            foreach (DataRow Registro in Dt_Programas.Rows) //Inserta los programas encontrados al Combo.
    //                Cmb_Programa.Items.Insert(Cont_Desplazamiento + 1, new ListItem(Registro[Cat_Com_Proyectos_Programas.Campo_Clave].ToString() + " - " + Registro[Cat_Com_Proyectos_Programas.Campo_Descripcion].ToString(), Registro[Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID].ToString()));
    //        }
    //        Cmb_Programa.SelectedIndex = -1;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception("Llenar_Cmb_Area_Funcional " + ex.Message.ToString(), ex);
    //    }
    //}
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llenar_Cmb_Fuentes_Financiamiento
    /// DESCRIPCION : Llena el ComboBox de acuerdo al Programa
    /// PARAMETROS  : Fuentes_ID: almacena los IDs
    ///               Fuentes_ID: almacena la cantidad de IDs que son
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 6/Octubre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    //private void Llenar_Cmb_Fuentes_Financiamiento(string[] Fuentes_ID, int Cont_Fuentes)
    //{
    //    Cls_Cat_SAP_Fuente_Financiamiento_Negocio Rs_Consulta_Fuentes = new Cls_Cat_SAP_Fuente_Financiamiento_Negocio(); //Variable de conexión hacia la capa de Negocios
    //    DataTable Dt_Fuentes; //Variable que obtendra los datos de la consulta 
    //    try
    //    {
    //        Cmb_Fuente_Financiamiento.Items.Clear();   //Elimina los Items dentro del Combo.
    //        Cmb_Fuente_Financiamiento.Items.Insert(0, new ListItem("<- Seleccione ->", ""));   //Inserta la opcion Seleccione en el Combo.
    //        for (int Cont_Desplazamiento = 0; Cont_Desplazamiento < Cont_Fuentes; Cont_Desplazamiento++)    //Ciclo de desplazamiento a traves del los IDs de las Fuentes de Financiamiento
    //        {
    //            Rs_Consulta_Fuentes.P_Fuente_Financiamiento_ID = Fuentes_ID[Cont_Desplazamiento];
    //            Dt_Fuentes = Rs_Consulta_Fuentes.Consulta_Datos_Fuente_Financiamiento();
    //            foreach (DataRow Registro in Dt_Fuentes.Rows)   //Inserta las fuentes de financiamiento encontrados al Combo.
    //                Cmb_Fuente_Financiamiento.Items.Insert(Cont_Desplazamiento + 1, new ListItem(Registro[Cat_SAP_Fuente_Financiamiento.Campo_Clave].ToString() + " - " + Registro[Cat_SAP_Fuente_Financiamiento.Campo_Descripcion].ToString(), Registro[Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID].ToString()));
    //        }
    //        Cmb_Fuente_Financiamiento.SelectedIndex = -1;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception("Llenar_Cmb_Fuentes_Financiamiento " + ex.Message.ToString(), ex);
    //    }
    //}
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Convertir_Datos_Poliza
    /// DESCRIPCION : Busca los IDs correspondientes para formar el codigo programatico
    /// PARAMETROS  : Dt_Poliza_Detalles: Almacena los datos capturados desde el Excel.
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 10/Octubre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Convertir_Datos_Poliza(DataTable Dt_Poliza_Detalles)
    {
        Cls_Cat_Dependencias_Negocio Rs_Dependencias = new Cls_Cat_Dependencias_Negocio();  //Variable para Dependencias.
        Cls_Cat_SAP_Fuente_Financiamiento_Negocio Rs_Financiamiento = new Cls_Cat_SAP_Fuente_Financiamiento_Negocio();  //Variable para Financiamiento.
        Cls_Ope_Con_Polizas_Negocio Rs_Area = new Cls_Ope_Con_Polizas_Negocio();  //Variable para el Area Funcional.
        Cls_Cat_Com_Proyectos_Programas_Negocio Rs_Proyectos = new Cls_Cat_Com_Proyectos_Programas_Negocio();   //Variable para los Proyectos.
        Cls_Ope_Con_Polizas_Negocio Rs_Partidas = new Cls_Ope_Con_Polizas_Negocio();  //Variable para las Partidas.
        Cls_Cat_Con_Cuentas_Contables_Negocio Rs_Cuentas = new Cls_Cat_Con_Cuentas_Contables_Negocio(); //Variable para las Cuentas Contables.
        DataTable Dt_Partidas_Polizas = new DataTable(); //Obtiene los datos de la póliza que fueron proporcionados por el usuario
        DataTable Dt_Uso_Multiple = null;
       // string Codigo_Programatico = "";
        int Cont_Rows = 1;
        double Total_Debe = 0;
        double Total_Haber = 0;

        try
        {
            //Agrega los campos que va a contener el DataTable
            Dt_Partidas_Polizas.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Partida, typeof(System.Int32));
            //Dt_Partidas_Polizas.Columns.Add("CODIGO_PROGRAMATICO", typeof(System.String));
            //Dt_Partidas_Polizas.Columns.Add("DEPENDENCIA_ID", typeof(System.String));
            //Dt_Partidas_Polizas.Columns.Add("FUENTE_FINANCIAMIENTO_ID", typeof(System.String));
            //Dt_Partidas_Polizas.Columns.Add("AREA_FUNCIONAL_ID", typeof(System.String));
            //Dt_Partidas_Polizas.Columns.Add("PROYECTO_PROGRAMA_ID", typeof(System.String));
            //Dt_Partidas_Polizas.Columns.Add("PARTIDA_ID", typeof(System.String));
            //Dt_Partidas_Polizas.Columns.Add("COMPROMISO_ID", typeof(System.String));
            Dt_Partidas_Polizas.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID, typeof(System.String));
            Dt_Partidas_Polizas.Columns.Add(Cat_Con_Cuentas_Contables.Campo_Cuenta, typeof(System.String));
            Dt_Partidas_Polizas.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Concepto, typeof(System.String));
            Dt_Partidas_Polizas.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Debe, typeof(System.Double));
            Dt_Partidas_Polizas.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Haber, typeof(System.Double));

            DataRow row;

            foreach (DataRow Registro in Dt_Poliza_Detalles.Rows)
            {
                row = Dt_Partidas_Polizas.NewRow(); //Crea un nuevo registro a la tabla
           //     Codigo_Programatico = "";
                #region (Codigo_Programatico)
                //Rs_Dependencias.P_Clave = Registro["UNIDAD_RESPONSABLE"].ToString();
                //Codigo_Programatico += Registro["UNIDAD_RESPONSABLE"].ToString();
                //Dt_Uso_Multiple = Rs_Dependencias.Consulta_Dependencias();
                //if (Dt_Uso_Multiple.Rows.Count > 0)
                //{
                //    row["DEPENDENCIA_ID"] = Dt_Uso_Multiple.Rows[0][0].ToString();
                //    Dt_Uso_Multiple = null;
                //}
                //Rs_Proyectos.P_Clave = Registro["PROGRAMA"].ToString();
                //Codigo_Programatico += "-" + Registro["PROGRAMA"].ToString();
                //Dt_Uso_Multiple = Rs_Proyectos.Consulta_Programas_Proyectos();
                //if (Dt_Uso_Multiple.Rows.Count > 0)
                //{
                //    row["PROYECTO_PROGRAMA_ID"] = Dt_Uso_Multiple.Rows[0][0].ToString();
                //    Dt_Uso_Multiple = null;
                //}
                //Rs_Financiamiento.P_Clave = Registro["FTE_FINANCIAMIENTO"].ToString();
                //Codigo_Programatico += "-" + Registro["FTE_FINANCIAMIENTO"].ToString();
                //Dt_Uso_Multiple = Rs_Financiamiento.Consulta_Fuente_Financiamiento();
                //if (Dt_Uso_Multiple.Rows.Count > 0)
                //{
                //    row["FUENTE_FINANCIAMIENTO_ID"] = Dt_Uso_Multiple.Rows[0][0].ToString();                   
                //    Dt_Uso_Multiple = null;
                //}
                //Rs_Area.P_Clave = Registro["AREA_FUNCIONAL"].ToString();
                //Codigo_Programatico += "-" + Registro["AREA_FUNCIONAL"].ToString();
                //Dt_Uso_Multiple = Rs_Area.Consulta_Area_Funcional_Especial();
                //if (Dt_Uso_Multiple.Rows.Count > 0)
                //{
                //    row["AREA_FUNCIONAL_ID"] = Dt_Uso_Multiple.Rows[0][1].ToString().Substring(0, 5);
                //    Dt_Uso_Multiple = null;
                //}
                //Rs_Partidas.P_Clave = Registro["PARTIDA"].ToString();
                //Codigo_Programatico += "-" + Registro["PARTIDA"].ToString();
                //Dt_Uso_Multiple = Rs_Partidas.Consulta_Partida_Especifica();
                //if (Dt_Uso_Multiple.Rows.Count > 0)
                //{
                //    row["PARTIDA_ID"] = Dt_Uso_Multiple.Rows[0][1].ToString();
                //    Dt_Uso_Multiple = null;
                //}
                Rs_Cuentas.P_Cuenta = Registro["CUENTA_CONTABLE"].ToString();
                Dt_Uso_Multiple = Rs_Cuentas.Consulta_Existencia_Cuenta_Contable();
                if (Dt_Uso_Multiple.Rows.Count > 0)
                {
                    row[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID] = Dt_Uso_Multiple.Rows[0][0].ToString();
                    Dt_Uso_Multiple = null;
                }
                #endregion   //Obtiene los IDs del codigo programatico
                row[Ope_Con_Polizas_Detalles.Campo_Partida] = Cont_Rows.ToString();
                row[Ope_Con_Polizas_Detalles.Campo_Debe] = Registro["DEBE"].ToString();
                Total_Debe += Convert.ToDouble(Registro["DEBE"].ToString());
                row[Ope_Con_Polizas_Detalles.Campo_Haber] = Registro["HABER"].ToString();
                Total_Haber += Convert.ToDouble(Registro["HABER"].ToString());
                row[Ope_Con_Polizas_Detalles.Campo_Concepto] = Registro["CONCEPTO"].ToString();
                row[Cat_Con_Cuentas_Contables.Campo_Cuenta] = Aplicar_Mascara_Cuenta_Contable(Registro["CUENTA_CONTABLE"].ToString());
                //row["CODIGO_PROGRAMATICO"] = Codigo_Programatico;
                //row["COMPROMISO_ID"] = Registro["NO_COMPROMISO"].ToString();
                Cont_Rows++;
                Dt_Partidas_Polizas.Rows.Add(row);
                Dt_Partidas_Polizas.AcceptChanges();
            }
            Session["Dt_Partidas_Poliza"] = Dt_Partidas_Polizas;//Agrega los valores del registro a la sesión
            Txt_No_Partidas.Text = (Cont_Rows - 1).ToString(); //Inserta el numero de partidas
            Txt_Total_Debe.Text = Total_Debe.ToString();    //Inserta el total del debe
            Txt_Total_Haber.Text = Total_Haber.ToString();  //Inserta el tottal de haber

            Grid_Detalles_Poliza.Columns[1].Visible = true;
            Grid_Detalles_Poliza.Columns[2].Visible = true;
            Grid_Detalles_Poliza.Columns[3].Visible = true;
            Grid_Detalles_Poliza.Columns[4].Visible = true;
            Grid_Detalles_Poliza.Columns[5].Visible = true;
            Grid_Detalles_Poliza.Columns[6].Visible = true;
            Grid_Detalles_Poliza.DataSource = Dt_Partidas_Polizas; //Agrega los valores de todas las partidas que se tienen al grid
            Grid_Detalles_Poliza.DataBind();
            Grid_Detalles_Poliza.Columns[1].Visible = false;
            //Grid_Detalles_Poliza.Columns[2].Visible = false;
            //Grid_Detalles_Poliza.Columns[3].Visible = false;
            //Grid_Detalles_Poliza.Columns[4].Visible = false;
            //Grid_Detalles_Poliza.Columns[5].Visible = false;
            //Grid_Detalles_Poliza.Columns[6].Visible = false;
            Habilitar_Controles("Carga");
        }
        catch (Exception ex)
        {
            throw new Exception("Convertir_Datos_Poliza " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Interpretar_Excel
    /// DESCRIPCION : Interpreta los datos contenidos por el archivo de Excel.
    /// PARAMETROS  : 
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 13/Octubre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Interpretar_Excel()
    {
        string connString = ""; //Cadena de conexion para Excel.
        string Nom_Archivo = "";    //Almacenara el nombre del archivo
        bool xls = Path.GetExtension(AFU_Archivo_Excel.PostedFile.FileName).Contains(".xls"); //Almacena si el archivo es 2003
        bool xlsx = Path.GetExtension(AFU_Archivo_Excel.PostedFile.FileName).Contains(".xlsx"); //Almacena si el archivo es 2007
        DataTable Dt_Poliza_Detalles = null;    //Almacena los detalles de la poliza
        int Num_Detalles_Poliza = 0;    //Almacena el numero de movimientos para esa poliza.
        int Caracter_Inicio = 0;    //Posicion del caracter de inicio del nombre del archivo.

        for (int Cont_Caracter = 0; Cont_Caracter < AFU_Archivo_Excel.PostedFile.FileName.Length; Cont_Caracter++)
        {
            if (AFU_Archivo_Excel.PostedFile.FileName[Cont_Caracter] == Convert.ToChar(92))
            {
                Caracter_Inicio = Cont_Caracter + 1;
            }
        }
        Nom_Archivo = AFU_Archivo_Excel.PostedFile.FileName.Substring(Caracter_Inicio, AFU_Archivo_Excel.PostedFile.FileName.Length - Caracter_Inicio);
        if (xls == true)
            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:/Polizas/" + Nom_Archivo + ";Extended Properties=Excel 8.0";
        if (xlsx == true)
            connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:/Polizas/" + Nom_Archivo + ";Extended Properties=Excel 12.0";
        OleDbConnection oledbConn = new OleDbConnection(connString);    //Crea el objeto de conexion
        try
        {
            Num_Detalles_Poliza = Convert.ToInt16(Txt_Num_Polizas.Text);
            DataSet ds = new DataSet(); // Crea el dataset que almacenara los datos extraidos
            OleDbCommand cmd;   // Almacenara el comando a ejecutar.
            oledbConn.Open();   // Abre la conexion
            cmd = new OleDbCommand("SELECT * FROM [Hoja1$A1:J" + (Num_Detalles_Poliza + 1) + "]", oledbConn);  //Crea el comando y extrae los datos.
            OleDbDataAdapter oleda = new OleDbDataAdapter();    // Crea el nuevo adaptador Ole
            oleda.SelectCommand = cmd;
            oleda.Fill(ds, "Poliza");    // Llena el dataset con los datos extraidos
            Dt_Poliza_Detalles = ds.Tables[0];    // Liga los datos con el DataTable
            Convertir_Datos_Poliza(Dt_Poliza_Detalles); //Convierte los datos extraidos para que sean visualizados de manera correcta.
        }
        catch (Exception ex)
        {
            throw new Exception("Error  Btn_Cargar_Click: " + ex.Message.ToString(), ex);
        }
        finally
        {
            oledbConn.Close();  // Close connection
        }      
    }
    #endregion


    #region (Metodos Consulta)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Cuenta_Contable
    /// DESCRIPCION : Consulta las cuentas contables
    /// PARAMETROS  : 
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 10/OCtubre/2011
    /// MODIFICO          : 
    /// FECHA_MODIFICO    : 
    /// CAUSA_MODIFICACION: 
    ///*******************************************************************************
    private void Consulta_Cuenta_Contable()
    {
        Cls_Cat_Con_Cuentas_Contables_Negocio Rs_Consulta_Con_Cuentas_Contables = new Cls_Cat_Con_Cuentas_Contables_Negocio(); //Variable de conexión a la capa de negocios
        DataTable Dt_Cuenta_Contable = null; //Obtiene la cuenta contable de la descripción que fue seleccionada por el usuario

        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Rs_Consulta_Con_Cuentas_Contables.P_Cuenta_Contable_ID = Cmb_Descripcion.SelectedValue;

            Dt_Cuenta_Contable = Rs_Consulta_Con_Cuentas_Contables.Consulta_Datos_Cuentas_Contables();
            if (Dt_Cuenta_Contable.Rows.Count > 0)
            {
                //Agrega la cuenta contable a la caja de texto correspondiente
                foreach (DataRow Registro in Dt_Cuenta_Contable.Rows)
                {
                    Txt_Cuenta_Contable.Text = Registro[Cat_Con_Cuentas_Contables.Campo_Cuenta].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Cuenta_Contable " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Cuentas_Contables_Tipo_Polizas
    /// DESCRIPCION : Carga las Percepciones Deducciones Fijas o Variables que no son calculadas
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Cuentas_Contables_Tipo_Polizas()
    {
        DataTable Dt_Cuentas_Contables = null;  //Almacenara los datos de las cuentas contables.
        DataTable Dt_Tipo_Polizas = null;   //Almacenara los tipos de polizas.
        Cls_Cat_Con_Cuentas_Contables_Negocio Rs_Consulta_Con_Cuentas_Contables = new Cls_Cat_Con_Cuentas_Contables_Negocio(); //Variable de conexion con la capa de Datos
        Cls_Cat_Con_Tipo_Polizas_Negocio Rs_Consulta_Cat_Con_Tipo_Polizas = new Cls_Cat_Con_Tipo_Polizas_Negocio(); //Variable de conexion con la capa de Datos

        try
        {
            Rs_Consulta_Con_Cuentas_Contables.P_Afectable = "SI";
            Dt_Cuentas_Contables = Rs_Consulta_Con_Cuentas_Contables.Consulta_Cuentas_Contables(); //Consulta las cuentas contables
            Cmb_Descripcion.DataSource = Dt_Cuentas_Contables; //Liga los datos con el combo
            Cmb_Descripcion.DataTextField = Cat_Con_Cuentas_Contables.Campo_Descripcion;    //Asigna el campo de la tabla que se visualizara
            Cmb_Descripcion.DataValueField = Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID;    //Asigna el campo de la tabla que sera usado como valor
            Cmb_Descripcion.DataBind();
            Cmb_Descripcion.Items.Insert(0, new ListItem("< Seleccione >", ""));
            Cmb_Descripcion.SelectedIndex = -1;

            Dt_Tipo_Polizas = Rs_Consulta_Cat_Con_Tipo_Polizas.Consulta_Tipos_Poliza(); //Consulta los tipos de polizas
            Cmb_Tipo_Poliza.DataSource = Dt_Tipo_Polizas;   //Liga los datos con el combo
            Cmb_Tipo_Poliza.DataTextField = Cat_Con_Tipo_Polizas.Campo_Descripcion; //Asigna el campo de la tabla que se visualizara
            Cmb_Tipo_Poliza.DataValueField = Cat_Con_Tipo_Polizas.Campo_Tipo_Poliza_ID; //Asigna el campo de la tabla que sera usado como valor
            Cmb_Tipo_Poliza.DataBind();
            Cmb_Tipo_Poliza.Items.Insert(0, new ListItem("< Seleccione >", ""));
            Cmb_Tipo_Poliza.SelectedIndex = -1;
            Cmb_Busqueda_Tipo_Poliza.DataSource = Dt_Tipo_Polizas;
            Cmb_Busqueda_Tipo_Poliza.DataTextField = Cat_Con_Tipo_Polizas.Campo_Descripcion; //Asigna el campo de la tabla que se visualizara
            Cmb_Busqueda_Tipo_Poliza.DataValueField = Cat_Con_Tipo_Polizas.Campo_Tipo_Poliza_ID; //Asigna el campo de la tabla que sera usado como valor
            Cmb_Busqueda_Tipo_Poliza.DataBind();
            Cmb_Busqueda_Tipo_Poliza.Items.Insert(0, new ListItem("< Seleccione >", ""));
        }
        catch (Exception Ex)
        {
            throw new Exception("Error generado al consultar las Percepciones Deducciones. Error: [" + Ex.Message + "]");
        }
    }
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
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Poliza_Avanzada
    /// DESCRIPCION : Ejecuta la busqueda de Poliza
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 22/Septiembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Poliza_Avanzada()
    {
        Cls_Ope_Con_Polizas_Negocio Rs_Consulta_Ca_Poliza = new Cls_Ope_Con_Polizas_Negocio(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Poliza; //Variable que obtendra los datos de la consulta 
        Double bandera = 0;
        Session["Consulta_Poliza_Avanzada"] = null;
        Lbl_Error_Busqueda.Visible = false;
        Img_Error_Busqueda.Visible = false;
        try
        {
            if (!string.IsNullOrEmpty(Txt_No_Poliza_PopUp.Text))
            {
                Rs_Consulta_Ca_Poliza.P_No_Poliza = String.Format("{0:0000000000}", Convert.ToInt16(Txt_No_Poliza_PopUp.Text.ToString()));
                bandera = 1;
            }
            if (Cmb_Busqueda_Tipo_Poliza.SelectedIndex > 0)
            {
                Rs_Consulta_Ca_Poliza.P_Tipo_Poliza_ID = Consulta_Tipo_Poliza();
                bandera = 1;
            }
            if (Cmb_Busqueda_Mes_Poliza.SelectedIndex > 0 && Cmb_Busqueda_Anio_Poliza.SelectedIndex > 0)
            {
                #region MES
                switch (Cmb_Busqueda_Mes_Poliza.SelectedItem.Text)
                {
                    case "ENERO":
                        Rs_Consulta_Ca_Poliza.P_Mes_Ano = "01";
                        break;
                    case "FEBRERO":
                        Rs_Consulta_Ca_Poliza.P_Mes_Ano = "02";
                        break;
                    case "MARZO":
                        Rs_Consulta_Ca_Poliza.P_Mes_Ano = "03";
                        break;
                    case "ABRIL":
                        Rs_Consulta_Ca_Poliza.P_Mes_Ano = "04";
                        break;
                    case "MAYO":
                        Rs_Consulta_Ca_Poliza.P_Mes_Ano = "05";
                        break;
                    case "JUNIO":
                        Rs_Consulta_Ca_Poliza.P_Mes_Ano = "06";
                        break;
                    case "JULIO":
                        Rs_Consulta_Ca_Poliza.P_Mes_Ano = "07";
                        break;
                    case "AGOSTO":
                        Rs_Consulta_Ca_Poliza.P_Mes_Ano = "08";
                        break;
                    case "SEPTIEMBRE":
                        Rs_Consulta_Ca_Poliza.P_Mes_Ano = "09";
                        break;
                    case "OCTUBRE":
                        Rs_Consulta_Ca_Poliza.P_Mes_Ano = "10";
                        break;
                    case "NOVIEMBRE":
                        Rs_Consulta_Ca_Poliza.P_Mes_Ano = "11";
                        break;
                    case "DICIEMBRE":
                        Rs_Consulta_Ca_Poliza.P_Mes_Ano = "12";
                        break;
                    default:
                        Rs_Consulta_Ca_Poliza.P_Mes_Ano = "";
                        break;
                }
                #endregion
                #region ANIO
                if (Cmb_Busqueda_Anio_Poliza.SelectedIndex > 0)
                    Rs_Consulta_Ca_Poliza.P_Mes_Ano += Cmb_Busqueda_Anio_Poliza.SelectedItem.Text.Substring(2, 2);
                #endregion
                bandera = 1;
            }
            if (Cmb_Busqueda_Mes_Poliza.SelectedIndex > 0 && Cmb_Busqueda_Anio_Poliza.SelectedIndex == 0) 
            {
                bandera = 2;
                Lbl_Error_Busqueda.Text = "<br> Debes Seleccionar un año para poder realizar la busqueda por año y mes  <br>";
            }
            if (Cmb_Busqueda_Anio_Poliza.SelectedIndex > 0 && Cmb_Busqueda_Anio_Poliza.SelectedIndex == 0)
            {
                bandera = 2;
                Lbl_Error_Busqueda.Text = "<br> Debes Seleccionar un Mes para poder realizar la busqueda por año y mes   <br>";
            }
                 if (bandera == 1)
                 {
                     Dt_Poliza = Rs_Consulta_Ca_Poliza.Consulta_Poliza_Popup();
                     Session["Consulta_Poliza_Avanzada"] = Dt_Poliza;
                     Llena_Grid_Polizas_Avanzada();
                     Mpe_Busqueda_Polizas.Show();
                 }
                 else
                 {
                     if (bandera == 0) Lbl_Error_Busqueda.Text = "<br> Debes ingresar un filtro para poder realizar la busqueda  <br>";
                     Lbl_Error_Busqueda.Visible = true;
                     Img_Error_Busqueda.Visible = true;
                     Mpe_Busqueda_Polizas.Show();
                 }
                 
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Poliza_Avanzada " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Tipo_Poliza
    /// DESCRIPCION : Ejecuta la busqueda del Tipo de Poliza
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 22/Septiembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private string Consulta_Tipo_Poliza()
    {
        Cls_Cat_Con_Tipo_Polizas_Negocio Rs_Consulta_Tipo_Poliza = new Cls_Cat_Con_Tipo_Polizas_Negocio(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Tipo_Poliza; //Variable que obtendra los datos de la consulta 

        try
        {
            Rs_Consulta_Tipo_Poliza.P_Descripcion = Cmb_Busqueda_Tipo_Poliza.SelectedItem.Text;
            Dt_Tipo_Poliza = Rs_Consulta_Tipo_Poliza.Consulta_Tipos_Poliza(); //Consulta el ID de acuerdo a la Descripcion
            Session["Consulta_Tipo_Poliza"] = Dt_Tipo_Poliza;
            return Dt_Tipo_Poliza.Rows[0][0].ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Tipo_Poliza " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Partida_Presupuestal
    /// DESCRIPCION : Realiza la busqueda de la partida especifica asociada a la cuenta contable.
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 4/Octubre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private string Consulta_Partida_Presupuestal()
    {
        Cls_Cat_Con_Tipo_Polizas_Negocio Rs_Consulta_Tipo_Poliza = new Cls_Cat_Con_Tipo_Polizas_Negocio(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Tipo_Poliza; //Variable que obtendra los datos de la consulta 

        try
        {
            Rs_Consulta_Tipo_Poliza.P_Descripcion = Cmb_Busqueda_Tipo_Poliza.SelectedItem.Text;
            Dt_Tipo_Poliza = Rs_Consulta_Tipo_Poliza.Consulta_Tipos_Poliza(); //Consulta el ID de acuerdo a la Descripcion
            Session["Consulta_Tipo_Poliza"] = Dt_Tipo_Poliza;
            return Dt_Tipo_Poliza.Rows[0][0].ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Tipo_Poliza " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Programa
    /// DESCRIPCION : Realiza la busqueda de todas las dependencias y las carga al ComboBox
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 4/Octubre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    //private void Consulta_Programa()
    //{
    //    Cls_Cat_Com_Proyectos_Programas_Negocio Rs_Consulta_Programas = new Cls_Cat_Com_Proyectos_Programas_Negocio(); //Variable de conexión hacia la capa de Negocios
    //    DataTable Dt_Partidas_Especificas; //Variable que obtendra los datos de la consulta 

    //    try
    //    {
    //        Dt_Partidas_Especificas = Rs_Consulta_Programas.Consulta_Programas_Especial(); //Consulta los programas existentes.
    //        Cmb_Programa.DataSource = Dt_Partidas_Especificas;
    //        Cmb_Programa.DataTextField = "CLAVE_NOMBRE";
    //        Cmb_Programa.DataValueField = Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID;
    //        Cmb_Programa.DataBind();
    //        Cmb_Programa.Items.Insert(0, new ListItem("< Seleccione >", ""));
    //        Cmb_Programa.SelectedIndex = -1;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception("Consulta_Dependencia " + ex.Message.ToString(), ex);
    //    }
    //}
    #endregion

    #region (Metodos de Operacion [Alta - Modificar - Eliminar])
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta_Poliza
    /// DESCRIPCION : Da de Alta la poliza con los datos proporcionados por el usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 11-Julio-2011
    /// MODIFICO          : Salvador L. Rea Ayala
    /// FECHA_MODIFICO    : 10/Octubre/2011
    /// CAUSA_MODIFICACION: Se agregaron los nuevos campos al metodo para su correcto
    ///                     funcionamiento.
    ///*******************************************************************************
    private void Alta_Poliza()
    {
        Cls_Ope_Con_Polizas_Negocio Rs_Alta_Ope_Con_Polizas; //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta
        Cls_Ope_Con_Compromisos_Negocio Rs_Compromisos = new Cls_Ope_Con_Compromisos_Negocio(); //Variable de conexion con la capa de negocios.
       // DataTable Dt_Compromisos = null;    //Almacena el compromiso de acuerdo a los datos proporcionados.
        Cls_Ope_SAP_Dep_Presupuesto_Negocio Rs_Presupuesto = new Cls_Ope_SAP_Dep_Presupuesto_Negocio(); //Variable de conexion con la capa de Negocios.
        DataTable Dt_Partidas_Poliza = null;    //Almacenara los registros encontrados en la tabla de Presupuestos        
       // DataTable Dt_Presupuesto = null;
        DataTable Dt_Jefe_Dependencia = null;
        try
        { 
            Rs_Alta_Ope_Con_Polizas = new Cls_Ope_Con_Polizas_Negocio();
            Rs_Alta_Ope_Con_Polizas.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
            Dt_Jefe_Dependencia = Rs_Alta_Ope_Con_Polizas.Consulta_Empleado_Jefe_Dependencia();
            Rs_Alta_Ope_Con_Polizas = new Cls_Ope_Con_Polizas_Negocio();
            Rs_Alta_Ope_Con_Polizas.P_Tipo_Poliza_ID = Cmb_Tipo_Poliza.SelectedValue;
            Rs_Alta_Ope_Con_Polizas.P_Mes_Ano = String.Format("{0:MMyy}", Convert.ToDateTime(Txt_Fecha_Poliza.Text));
            Rs_Alta_Ope_Con_Polizas.P_Fecha_Poliza = Convert.ToDateTime(Txt_Fecha_Poliza.Text);
            Rs_Alta_Ope_Con_Polizas.P_Concepto = Convert.ToString(Txt_Concepto_Poliza.Text.ToString());
            Rs_Alta_Ope_Con_Polizas.P_Total_Debe = Convert.ToDouble(Convert.ToString(Txt_Total_Debe.Text.ToString()).Replace(",", ""));
            Rs_Alta_Ope_Con_Polizas.P_Total_Haber = Convert.ToDouble(Convert.ToString(Txt_Total_Haber.Text.ToString()).Replace(",", ""));
            Rs_Alta_Ope_Con_Polizas.P_No_Partida = Convert.ToInt32(Txt_No_Partidas.Text.ToString());
            Rs_Alta_Ope_Con_Polizas.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
            Rs_Alta_Ope_Con_Polizas.P_Dt_Detalles_Polizas = (DataTable)Session["Dt_Partidas_Poliza"];
            Rs_Alta_Ope_Con_Polizas.P_Empleado_ID_Creo = Cls_Sessiones.Empleado_ID;
            Rs_Alta_Ope_Con_Polizas.P_Empleado_ID_Autorizo = Cmb_Nombre_Empleado.SelectedValue.ToString();

            string[] Datos_Poliza = Rs_Alta_Ope_Con_Polizas.Alta_Poliza(); //Da de alta los datos de la Póliza proporcionados por el usuario en la BD
            Dt_Partidas_Poliza = (DataTable)Session["Dt_Partidas_Poliza"];

            //foreach (DataRow Registro in Dt_Partidas_Poliza.Rows)
            //{
                //if (!String.IsNullOrEmpty(Registro["PARTIDA_ID"].ToString()))
                //{
                //    Rs_Presupuesto.P_Partida_ID = Registro["PARTIDA_ID"].ToString();
                //    Rs_Presupuesto.P_Dependencia_ID = Registro["DEPENDENCIA_ID"].ToString();
                //    Rs_Presupuesto.P_Fuente_Financiamiento_ID = Registro["FUENTE_FINANCIAMIENTO_ID"].ToString();
                //    Rs_Presupuesto.P_Programa_ID = Registro["PROYECTO_PROGRAMA_ID"].ToString();
                //    Dt_Presupuesto = Rs_Presupuesto.Consulta_Datos_Presupuestos();

                //    Rs_Compromisos.P_Dependencia_ID = Registro["DEPENDENCIA_ID"].ToString();
                //    Rs_Compromisos.P_Fuente_Financiamiento_ID = Registro["FUENTE_FINANCIAMIENTO_ID"].ToString();
                //    Rs_Compromisos.P_Proyecto_Programa_ID = Registro["PROYECTO_PROGRAMA_ID"].ToString();
                //    Rs_Compromisos.P_Area_Funcional_ID = Registro["AREA_FUNCIONAL_ID"].ToString();
                //    Rs_Compromisos.P_Partida_ID = Registro["PARTIDA_ID"].ToString();
                //    Dt_Compromisos = Rs_Compromisos.Consulta_Compromisos();

                //    foreach (DataRow Registro_Presupuesto in Dt_Presupuesto.Rows)
                //    {
                //        if (!String.IsNullOrEmpty(Registro["COMPROMISO_ID"].ToString()))
                //        {
                //            Rs_Presupuesto.P_Comprometido = (Convert.ToDouble(Registro_Presupuesto[Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido].ToString()) - (Convert.ToDouble(Registro["DEBE"].ToString()) - Convert.ToDouble(Registro["HABER"].ToString()))).ToString();
                //            Rs_Presupuesto.P_Disponible = Registro_Presupuesto[Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible].ToString();
                //            Rs_Presupuesto.Actualizar_Montos_Presupuesto();
                //        }
                //        else
                //        {
                //            Rs_Presupuesto.P_Comprometido = Registro_Presupuesto[Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido].ToString();
                //            Rs_Presupuesto.P_Disponible = (Convert.ToDouble(Registro_Presupuesto[Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible].ToString()) - (Convert.ToDouble(Registro["DEBE"].ToString()) - Convert.ToDouble(Registro["HABER"].ToString()))).ToString();
                //            Rs_Presupuesto.Actualizar_Montos_Presupuesto();
                //        }
                //    }

                //    foreach (DataRow Registro_Compromisos in Dt_Compromisos.Rows)
                //    {
                //        if (!String.IsNullOrEmpty(Registro["COMPROMISO_ID"].ToString()))
                //        {
                //            Rs_Compromisos.P_No_Compromiso = Registro["COMPROMISO_ID"].ToString();
                //            Rs_Compromisos.P_Monto_Comprometido = (Convert.ToDouble(Registro_Compromisos[Ope_Con_Compromisos.Campo_Monto_Comprometido].ToString()) - (Convert.ToDouble(Registro["DEBE"].ToString()) - Convert.ToDouble(Registro["HABER"].ToString()))).ToString();
                //            Rs_Compromisos.Modificar_Montos();
                //        }
                //    }
                //}
            //}
            if (Convert.ToInt16(String.Format("{0:MM}", Convert.ToDateTime(Txt_Fecha_Poliza.Text))) < Convert.ToInt16(String.Format("{0:MM}", DateTime.Now)))
                Alta_Poliza_Desfasada(Datos_Poliza, (DataTable)Session["Dt_Partidas_Poliza"]);

            Limpia_Controles();
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            Txt_No_Poliza.Text = "";
            Cmb_Tipo_Poliza.SelectedIndex = -1;
            //Cmb_Afectable.SelectedIndex = 0;
            //Cmb_No_Compromiso.Items.Clear();
            //Cmb_No_Compromiso.Enabled = false;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Polizas", "alert('El Alta de la Póliza fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Alta_Poliza " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Poliza
    /// DESCRIPCION : Modifica la poliza con los datos proporcionados por el usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 11-Julio-2011
    /// MODIFICO          : Salvador L. Rea Ayala
    /// FECHA_MODIFICO    : 10/Octubre/2011
    /// CAUSA_MODIFICACION: Se agregaron los nuevos campos al metodo para su correcto
    ///                     funcionamiento.
    ///*******************************************************************************
    private void Modificar_Poliza()
    {
        Cls_Ope_Con_Polizas_Negocio Rs_Modificar_Ope_Con_Polizas = new Cls_Ope_Con_Polizas_Negocio(); //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta
        DataTable Dt_Polizas;
        Cls_Ope_Con_Compromisos_Negocio Rs_Compromisos = new Cls_Ope_Con_Compromisos_Negocio(); //Variable de conexion con la capa de negocios.
        DataTable Dt_Compromisos = null;    //Almacena el compromiso de acuerdo a los datos proporcionados.
        Cls_Ope_SAP_Dep_Presupuesto_Negocio Rs_Presupuesto = new Cls_Ope_SAP_Dep_Presupuesto_Negocio(); //Variable de conexion con la capa de Negocios.
        DataTable Dt_Partidas_Poliza = null;    //Almacenara los registros encontrados en la tabla de Presupuestos        
        DataTable Dt_Presupuesto = null;
        Rs_Modificar_Ope_Con_Polizas.P_No_Poliza = Txt_No_Poliza.Text;
        Rs_Modificar_Ope_Con_Polizas.P_Fecha_Poliza = Convert.ToDateTime(Txt_Fecha_Poliza.Text);
        try
        {
            Rs_Modificar_Ope_Con_Polizas.P_No_Poliza = Convert.ToString(Txt_No_Poliza.Text);
            Rs_Modificar_Ope_Con_Polizas.P_Tipo_Poliza_ID = Cmb_Tipo_Poliza.SelectedValue;
            Rs_Modificar_Ope_Con_Polizas.P_Mes_Ano = Convert.ToString(String.Format("{0:MMyy}", Convert.ToDateTime(Txt_Fecha_Poliza.Text)));
            Rs_Modificar_Ope_Con_Polizas.P_Fecha_Poliza = Convert.ToDateTime(Txt_Fecha_Poliza.Text);
            Rs_Modificar_Ope_Con_Polizas.P_Concepto = Convert.ToString(Txt_Concepto_Poliza.Text.ToString());
            Rs_Modificar_Ope_Con_Polizas.P_Total_Debe = Convert.ToDouble(Convert.ToString(Txt_Total_Debe.Text.ToString()).Replace(",", ""));
            Rs_Modificar_Ope_Con_Polizas.P_Total_Haber = Convert.ToDouble(Convert.ToString(Txt_Total_Haber.Text.ToString()).Replace(",", ""));
            Rs_Modificar_Ope_Con_Polizas.P_No_Partida = Convert.ToInt32(Txt_No_Partidas.Text.ToString());
            Rs_Modificar_Ope_Con_Polizas.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
            Rs_Modificar_Ope_Con_Polizas.P_Dt_Detalles_Polizas = (DataTable)Session["Dt_Partidas_Poliza"];

            Rs_Modificar_Ope_Con_Polizas.Modificar_Polizas(); //Da de alta los datos de el Sindicato proporcionados por el usuario en la BD
            Dt_Partidas_Poliza = (DataTable)Session["Dt_Partidas_Poliza"];

            //foreach (DataRow Registro in Dt_Partidas_Poliza.Rows)
            //{
            //    if (!String.IsNullOrEmpty(Registro["PARTIDA_ID"].ToString()))
            //    {
            //        Rs_Presupuesto.P_Partida_ID = Registro["PARTIDA_ID"].ToString();
            //        Rs_Presupuesto.P_Dependencia_ID = Registro["DEPENDENCIA_ID"].ToString();
            //        Rs_Presupuesto.P_Fuente_Financiamiento_ID = Registro["FUENTE_FINANCIAMIENTO_ID"].ToString();
            //        Rs_Presupuesto.P_Programa_ID = Registro["PROYECTO_PROGRAMA_ID"].ToString();
            //        Dt_Presupuesto = Rs_Presupuesto.Consulta_Datos_Presupuestos();

            //        Rs_Compromisos.P_Dependencia_ID = Registro["DEPENDENCIA_ID"].ToString();
            //        Rs_Compromisos.P_Fuente_Financiamiento_ID = Registro["FUENTE_FINANCIAMIENTO_ID"].ToString();
            //        Rs_Compromisos.P_Proyecto_Programa_ID = Registro["PROYECTO_PROGRAMA_ID"].ToString();
            //        Rs_Compromisos.P_Area_Funcional_ID = Registro["AREA_FUNCIONAL_ID"].ToString();
            //        Rs_Compromisos.P_Partida_ID = Registro["PARTIDA_ID"].ToString();
            //        Dt_Compromisos = Rs_Compromisos.Consulta_Compromisos();

            //        foreach (DataRow Registro_Presupuesto in Dt_Presupuesto.Rows)
            //        {
            //            if (!String.IsNullOrEmpty(Registro["COMPROMISO_ID"].ToString()))
            //            {
            //                Rs_Presupuesto.P_Comprometido = (Convert.ToDouble(Registro_Presupuesto[Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido].ToString()) - (Convert.ToDouble(Registro["DEBE"].ToString()) - Convert.ToDouble(Registro["HABER"].ToString()))).ToString();
            //                Rs_Presupuesto.P_Disponible = Registro_Presupuesto[Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible].ToString();
            //                Rs_Presupuesto.Actualizar_Montos_Presupuesto();
            //            }
            //            else
            //            {
            //                Rs_Presupuesto.P_Comprometido = Registro_Presupuesto[Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido].ToString();
            //                Rs_Presupuesto.P_Disponible = (Convert.ToDouble(Registro_Presupuesto[Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible].ToString()) - (Convert.ToDouble(Registro["DEBE"].ToString()) - Convert.ToDouble(Registro["HABER"].ToString()))).ToString();
            //                Rs_Presupuesto.Actualizar_Montos_Presupuesto();
            //            }
            //        }

            //        foreach (DataRow Registro_Compromisos in Dt_Compromisos.Rows)
            //        {
            //            if (!String.IsNullOrEmpty(Registro["COMPROMISO_ID"].ToString()))
            //            {
            //                Rs_Compromisos.P_No_Compromiso = Registro["COMPROMISO_ID"].ToString();
            //                Rs_Compromisos.P_Monto_Comprometido = (Convert.ToDouble(Registro_Compromisos[Ope_Con_Compromisos.Campo_Monto_Comprometido].ToString()) - (Convert.ToDouble(Registro["DEBE"].ToString()) - Convert.ToDouble(Registro["HABER"].ToString()))).ToString();
            //                Rs_Compromisos.Modificar_Montos();
            //            }
            //        }
            //    }
            //}

            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pólizas", "alert('La modificación de la Póliza fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar_Poliza " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Eliminar_Poliza
    /// DESCRIPCION : Elimina los datos de la Póliza que fue seleccionada por el Usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 11-Julio-2011
    /// MODIFICO          : Salvador L. Rea Ayala
    /// FECHA_MODIFICO    : 10/Octubre/2011
    /// CAUSA_MODIFICACION: Se agregaron los nuevos campos al metodo para su correcto
    ///                     funcionamiento.
    ///*******************************************************************************
    private void Eliminar_Poliza()
    {
        Cls_Ope_Con_Polizas_Negocio Rs_Eliminar_Ope_Con_Polizas = new Cls_Ope_Con_Polizas_Negocio(); //Variable de conexión hacia la capa de Negocios para la eliminación de los datos
        Cls_Ope_Con_Polizas_Negocio Rs_Consulta_Ope_Con_Polizas = new Cls_Ope_Con_Polizas_Negocio();
        DataTable Dt_Polizas = null;
        string Fecha_Poliza;
        try
        {
            Rs_Eliminar_Ope_Con_Polizas.P_No_Poliza = Convert.ToString(Txt_No_Poliza.Text);
            Rs_Eliminar_Ope_Con_Polizas.P_Tipo_Poliza_ID = Cmb_Tipo_Poliza.SelectedIndex > 0 ? Cmb_Tipo_Poliza.SelectedItem.Text : "";
            Rs_Eliminar_Ope_Con_Polizas.P_Mes_Ano = String.Format("{0:MMyy}", Convert.ToDateTime(Txt_Fecha_Poliza.Text));
            Rs_Eliminar_Ope_Con_Polizas.Eliminar_Poliza(); //Elimina la póliza que selecciono el usuario de la BD
            Fecha_Poliza = String.Format("{0:MMyy}", Convert.ToDateTime(Txt_Fecha_Poliza.Text));
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            Limpia_Controles();
            Rs_Consulta_Ope_Con_Polizas.P_Mes_Ano = Fecha_Poliza;
            Dt_Polizas = Rs_Consulta_Ope_Con_Polizas.Consulta_Poliza_Popup();
            Session.Remove("Consulta_Poliza_Avanzada");
            Session["Consulta_Poliza_Avanzada"] = Dt_Polizas;
            Llena_Grid_Polizas_Avanzada();
        }
        catch (Exception ex)
        {
            throw new Exception("Eliminar_Poliza " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta_Poliza_Desfasada
    /// DESCRIPCION : Da de alta los datos de la poliza desfasada
    /// PARAMETROS  : 
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 24/Octubre/2011
    /// MODIFICO          : 
    /// FECHA_MODIFICO    : 
    /// CAUSA_MODIFICACION: 
    ///*******************************************************************************
    private void Alta_Poliza_Desfasada(string[] Datos_Poliza, DataTable Dt_Movimientos)
    {
        Cls_Ope_Con_Bitacora_Polizas_Negocio Rs_Bitacora_Polizas = new Cls_Ope_Con_Bitacora_Polizas_Negocio();  //Variable de conexion con la capa de negocios.
        Cls_Ope_Con_Cierre_Mensual_Negocio Rs_Cierre_Polizas = new Cls_Ope_Con_Cierre_Mensual_Negocio();
        string mes;
        DateTime fecha;
        string anio;
        try
        {
            Rs_Bitacora_Polizas.P_No_Poliza = Datos_Poliza[0];
            Rs_Bitacora_Polizas.P_Tipo_Poliza_ID = Datos_Poliza[1];
            Rs_Bitacora_Polizas.P_Mes_Ano = Datos_Poliza[2];
            Rs_Bitacora_Polizas.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
            foreach (DataRow Registro in Dt_Movimientos.Rows)
            {
                Rs_Bitacora_Polizas.P_Cuenta_Contable_ID = Registro["CUENTA_CONTABLE_ID"].ToString();
                Rs_Bitacora_Polizas.P_Debe = Registro["DEBE"].ToString();
                Rs_Bitacora_Polizas.P_Haber = Registro["HABER"].ToString();
                Rs_Bitacora_Polizas.Alta_Bitacora();
            }
            fecha = Convert.ToDateTime(String.Format("{0:MM/dd/yy}",Convert.ToDateTime(Datos_Poliza[2].Substring(0, 2) + "/01/" + Datos_Poliza[2].Substring(2, 2))));
            mes = String.Format("{0:MMMM}", fecha).ToUpper(); 
            anio= String.Format("{0:yyyy}",fecha).ToUpper();
            Rs_Cierre_Polizas.P_Mes=mes;
            Rs_Cierre_Polizas.P_Anio = anio;
            Rs_Cierre_Polizas.P_Estatus ="AFECTADO";
            Rs_Cierre_Polizas.Modifica_Cierre_Mensual();
        }
        catch (Exception ex)
        {
            throw new Exception("Alta_Poliza_Desfasada " + ex.Message.ToString(), ex);
        }
    }
    #endregion
    #endregion
    #region (Grid)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Polizas_SelectedIndexChanged
    /// DESCRIPCION : Consulta los datos de la Poliza seleccionada
    /// PARAMETROS  : 
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 12-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Polizas_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_Con_Polizas_Negocio Rs_Consulta_Cat_Polizas = new Cls_Ope_Con_Polizas_Negocio(); //Variable de conexión a la capa de Negocios para la consulta de los datos del empleado
        DataTable Dt_Polizas; //Variable que obtendra los datos de la consulta
        Cls_Ope_Con_Polizas_Negocio Rs_Consulta_Detalles_Poliza = new Cls_Ope_Con_Polizas_Negocio();
        DataTable Dt_Detalles_Poliza;
        Cls_Ope_Con_Polizas_Negocio Rs_Consulta_Empleados = new Cls_Ope_Con_Polizas_Negocio();

        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Rs_Consulta_Cat_Polizas.P_No_Poliza = Grid_Polizas.SelectedRow.Cells[1].Text;
            Rs_Consulta_Cat_Polizas.P_Tipo_Poliza_ID = Grid_Polizas.SelectedRow.Cells[2].Text;
            Rs_Consulta_Cat_Polizas.P_Concepto  = Grid_Polizas.SelectedRow.Cells[5].Text;
            Dt_Polizas = Rs_Consulta_Cat_Polizas.Consulta_Poliza(); //Consulta los datos de la poliza que fue seleccionada
            if (Dt_Polizas.Rows.Count > 0)
            {
                //Agrega los valores de los campos a los controles correspondientes de la forma
                foreach (DataRow Registro in Dt_Polizas.Rows)
                {
                    if (!string.IsNullOrEmpty(Registro[Ope_Con_Polizas.Campo_No_Poliza].ToString()))
                        Txt_No_Poliza.Text = Registro[Ope_Con_Polizas.Campo_No_Poliza].ToString();
                    if (!string.IsNullOrEmpty(Registro[Ope_Con_Polizas.Campo_Tipo_Poliza_ID].ToString()))
                        Cmb_Tipo_Poliza.SelectedIndex = Cmb_Tipo_Poliza.Items.IndexOf(Cmb_Tipo_Poliza.Items.FindByValue(Registro[Ope_Con_Polizas.Campo_Tipo_Poliza_ID].ToString()));
                    if (!string.IsNullOrEmpty(Registro[Ope_Con_Polizas.Campo_Fecha_Poliza].ToString()))
                        Txt_Fecha_Poliza.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro[Ope_Con_Polizas.Campo_Fecha_Poliza].ToString()));
                    if (!string.IsNullOrEmpty(Registro[Ope_Con_Polizas.Campo_Concepto].ToString()))
                        Txt_Concepto_Poliza.Text = Registro[Ope_Con_Polizas.Campo_Concepto].ToString();
                    if (!string.IsNullOrEmpty(Registro[Ope_Con_Polizas.Campo_No_Partidas].ToString()))
                        Txt_No_Partidas.Text = Registro[Ope_Con_Polizas.Campo_No_Partidas].ToString();
                    if (!string.IsNullOrEmpty(Registro[Ope_Con_Polizas.Campo_Total_Debe].ToString()))
                        Txt_Total_Debe.Text = Registro[Ope_Con_Polizas.Campo_Total_Debe].ToString();
                    if (!string.IsNullOrEmpty(Registro[Ope_Con_Polizas.Campo_Total_Haber].ToString()))
                        Txt_Total_Haber.Text = Registro[Ope_Con_Polizas.Campo_Total_Haber].ToString();

                    Rs_Consulta_Detalles_Poliza.P_No_Poliza = Txt_No_Poliza.Text;
                    Rs_Consulta_Empleados.P_No_Poliza = Txt_No_Poliza.Text;
                    Rs_Consulta_Detalles_Poliza.P_Tipo_Poliza_ID = String.Format("{0:00000}", Registro[Ope_Con_Polizas.Campo_Tipo_Poliza_ID].ToString());
                    Rs_Consulta_Detalles_Poliza.P_Mes_Ano = Registro[Ope_Con_Polizas.Campo_Mes_Ano].ToString();
                }
                Dt_Detalles_Poliza = Rs_Consulta_Detalles_Poliza.Consulta_Detalles_Poliza_Seleccionada();
                Session["Consulta_Poliza"] = Dt_Detalles_Poliza;
                Session["Dt_Partidas_Poliza"] = Dt_Detalles_Poliza;
                Llena_Grid_Polizas();
                Dt_Polizas = Rs_Consulta_Empleados.Consulta_Detalles_Empleado_Aprobo();
                foreach (DataRow Registro in Dt_Polizas.Rows)
                {
                    Cmb_Nombre_Empleado.Items.Clear();
                    Cmb_Nombre_Empleado.Items.Insert(0, new ListItem(Registro["EMPLEADO_AUTORIZO"].ToString(), Registro[Cat_Empleados.Campo_Empleado_ID].ToString()));
                }
                Dt_Polizas = Rs_Consulta_Empleados.Consulta_Detalles_Empleado_Creo();
                foreach (DataRow Registro in Dt_Polizas.Rows)
                {
                    Txt_Empleado_Creo.Text = Registro["EMPLEADO_CREO"].ToString();
                    //Cmb_Empleado_Creo.Items.Clear();
                    //Cmb_Empleado_Creo.Items.Insert(0, new ListItem(Registro["EMPLEADO_CREO"].ToString(), Registro[Cat_Empleados.Campo_Empleado_ID].ToString()));
                }
            }
            Grid_Polizas.DataSource = null;
            Grid_Polizas.DataBind();
            Txt_No_Poliza_PopUp.Text="";  
          
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Polizas_PageIndexChanging
    /// DESCRIPCION : Cambia la pagina de la tabla de Polizas
    ///               
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Polizas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Limpia_Controles();                        //Limpia todos los controles de la forma
            Grid_Polizas.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
            Llena_Grid_Polizas_Avanzada();                    //Carga los Polizas que estan asignados a la página seleccionada
            Grid_Polizas.SelectedIndex = -1;
            Mpe_Busqueda_Polizas.Show();
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llena_Grid_Polizas
    /// DESCRIPCION : Llena el grid con las Polizas encontradas
    /// PARAMETROS  : 
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 22/Septiembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llena_Grid_Polizas()
    {
        DataTable Dt_Polizas; //Variable que obtendra los datos de la consulta 
        try
        {
            Grid_Detalles_Poliza.Columns[1].Visible = true;
            Grid_Detalles_Poliza.Columns[2].Visible = true;
            Grid_Detalles_Poliza.Columns[3].Visible = true;
            Grid_Detalles_Poliza.Columns[4].Visible = true;
            Grid_Detalles_Poliza.Columns[5].Visible = true;
            Grid_Detalles_Poliza.Columns[6].Visible = true;
            Dt_Polizas = (DataTable)Session["Consulta_Poliza"]; //Se obtienen los datos de la sesion.
            Grid_Detalles_Poliza.DataSource = Dt_Polizas;   // Se iguala el DataTable con el Grid.
            Grid_Detalles_Poliza.DataBind();    // Se ligan los datos.
            Grid_Detalles_Poliza.Columns[1].Visible = false;
            //Grid_Detalles_Poliza.Columns[2].Visible = false;
            //Grid_Detalles_Poliza.Columns[3].Visible = false;
            //Grid_Detalles_Poliza.Columns[4].Visible = false;
            //Grid_Detalles_Poliza.Columns[5].Visible = false;
            //Grid_Detalles_Poliza.Columns[6].Visible = false;
            Grid_Detalles_Poliza.Visible = true;
            Grid_Detalles_Poliza.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_Polizas " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llena_Grid_Polizas_Avanzada
    /// DESCRIPCION : Llena el grid con las Polizas encontradas
    /// PARAMETROS  : 
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 27/Septiembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llena_Grid_Polizas_Avanzada()
    {
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        DataTable Dt_Polizas; //Variable que obtendra los datos de la consulta 
        try
        {
            Grid_Polizas.Columns[1].Visible = true;
            Grid_Polizas.Columns[2].Visible = true;
            Grid_Polizas.DataSource = null;   // Se iguala el DataTable con el Grid
            Grid_Polizas.DataBind();    // Se ligan los datos.
            Dt_Polizas = (DataTable)Session["Consulta_Poliza_Avanzada"];    //Se obtienen los datos de la sesion.
            if (Dt_Polizas.Rows.Count == 0) {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No se encontrarón registros en la busqueda";
            }
            Grid_Polizas.DataSource = Dt_Polizas;   // Se iguala el DataTable con el Grid
            Grid_Polizas.DataBind();    // Se ligan los datos.
            Grid_Polizas.Visible = true;
            Grid_Polizas.Columns[2].Visible = false;
            Grid_Polizas.SelectedIndex = -1;

        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_Polizas_Avanzada " + ex.Message.ToString(), ex);
        }
    }
    
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Detalles_Poliza_RowDataBound
    /// DESCRIPCION : Agrega un identificador al boton de cancelar de la tabla
    ///               para identicar la fila seleccionada de tabla.
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 11/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Detalles_Poliza_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                ((ImageButton)e.Row.Cells[6].FindControl("Btn_Eliminar_Partida")).CommandArgument = e.Row.Cells[0].Text.Trim();
                ((ImageButton)e.Row.Cells[6].FindControl("Btn_Eliminar_Partida")).ToolTip = "Quitar la Partida " + e.Row.Cells[2].Text + " a la Poliza";
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    #endregion

    #region (Eventos)
    protected void Btn_Agregar_Partida_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Dt_Partidas_Polizas = new DataTable(); //Obtiene los datos de la póliza que fueron proporcionados por el usuario
        String Espacios = "";
        //string Codigo_Programatico = "";
        int error = 0;
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        //Valida que todos los datos requeridos los haya proporcionado el usuario
        if ((!String.IsNullOrEmpty(Txt_Debe_Partida.Text) || !String.IsNullOrEmpty(Txt_Haber_Partida.Text)) && !String.IsNullOrEmpty(Txt_Concepto_Partida.Text))
        {
            //if (Txt_Partida_ID.Text != "" && Cmb_Area_Funcional.Items.Count == 0)
            //{
            //    error = 1;
            //    Lbl_Mensaje_Error.Visible = true;
            //    Img_Error.Visible = true;
            //    Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
            //    Lbl_Mensaje_Error.Text += Espacios + " + El codigo programatico completo <br>";
            //}
            //else
            //{
                //Si no se a agregado ninguna partida entonces crea la Estructura necesaria para poder visualizar la información al usuario
                if (Session["Dt_Partidas_Poliza"] == null)
                {
                    //Agrega los campos que va a contener el DataTable
                    Dt_Partidas_Polizas.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Partida, typeof(System.Int32));
                    //Dt_Partidas_Polizas.Columns.Add("CODIGO_PROGRAMATICO", typeof(System.String));
                    //Dt_Partidas_Polizas.Columns.Add("DEPENDENCIA_ID", typeof(System.String));
                   // Dt_Partidas_Polizas.Columns.Add("FUENTE_FINANCIAMIENTO_ID", typeof(System.String));
                   // Dt_Partidas_Polizas.Columns.Add("AREA_FUNCIONAL_ID", typeof(System.String));
                   // Dt_Partidas_Polizas.Columns.Add("PROYECTO_PROGRAMA_ID", typeof(System.String));
                   // Dt_Partidas_Polizas.Columns.Add("PARTIDA_ID", typeof(System.String));
                    //Dt_Partidas_Polizas.Columns.Add("COMPROMISO_ID", typeof(System.String));
                    Dt_Partidas_Polizas.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID, typeof(System.String));
                    Dt_Partidas_Polizas.Columns.Add(Cat_Con_Cuentas_Contables.Campo_Cuenta, typeof(System.String));
                    Dt_Partidas_Polizas.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Concepto, typeof(System.String));
                    Dt_Partidas_Polizas.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Debe, typeof(System.Double));
                    Dt_Partidas_Polizas.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Haber, typeof(System.Double));
                }
                //Si se tiene agregada ya partidas a la póliza entonces agrega la estructura a la tabla para poder insertar
                //los datos que el usuario introdujo a la póliza
                else
                {
                    Dt_Partidas_Polizas = (DataTable)Session["Dt_Partidas_Poliza"];
                    Session.Remove("Dt_Partidas_Poliza");
                }
                DataRow row = Dt_Partidas_Polizas.NewRow(); //Crea un nuevo registro a la tabla

                if (String.IsNullOrEmpty(Txt_Debe_Partida.Text))
                {
                    Txt_Debe_Partida.Text = "0";
                }
                if (String.IsNullOrEmpty(Txt_Haber_Partida.Text))
                {
                    Txt_Haber_Partida.Text = "0";
                }

                //if (Cmb_Unidad_Responsable.SelectedIndex > 0)
                //{
                //    row["DEPENDENCIA_ID"] = Cmb_Unidad_Responsable.SelectedValue.ToString();
                //    Codigo_Programatico = Cmb_Unidad_Responsable.SelectedItem.ToString().Substring(0, 5) + "-";
                //}
                //else
                //{
                //    row["DEPENDENCIA_ID"] = "";
                //}
                //if (Cmb_Programa.SelectedIndex > 0)
                //{
                //    row["PROYECTO_PROGRAMA_ID"] = Cmb_Programa.SelectedValue.ToString();
                //    Codigo_Programatico += Cmb_Programa.SelectedItem.ToString().Substring(0, 5) + "-";
                //}
                //else
                //{
                //    row["PROYECTO_PROGRAMA_ID"] = "";
                //}
                //if (Cmb_Fuente_Financiamiento.SelectedIndex > 0)
                //{
                //    row["FUENTE_FINANCIAMIENTO_ID"] = Cmb_Fuente_Financiamiento.SelectedValue.ToString();
                //    Codigo_Programatico += Cmb_Fuente_Financiamiento.SelectedItem.ToString().Substring(0, 4) + "-";
                //}
                //else
                //{
                //    row["FUENTE_FINANCIAMIENTO_ID"] = "";
                //}
                //if (Cmb_Area_Funcional.SelectedIndex > 0)
                //{
                //    row["AREA_FUNCIONAL_ID"] = Cmb_Area_Funcional.SelectedValue.ToString();
                //    Codigo_Programatico += Cmb_Area_Funcional.SelectedItem.ToString().Substring(0, 5) + "-";
                //}
                //else
                //{
                //    row["AREA_FUNCIONAL_ID"] = "";
                //}
                //if (!String.IsNullOrEmpty(Txt_Clave_Presupuestal.Text))
                //{
                //    Codigo_Programatico += Txt_Clave_Presupuestal.Text;
                //}
                //else
                //{
                //    Codigo_Programatico = "N/A";
                //}
                //if (Cmb_Afectable.SelectedIndex == 2)
                //{
                //    row["COMPROMISO_ID"] = Cmb_No_Compromiso.SelectedValue.ToString(); //Si el monto se reducira de lo comprometido
                //}
                //Asigna los valores al nuevo registro creado a la tabla
                //row["PARTIDA_ID"] = Txt_Partida_ID.Text;
                row[Ope_Con_Polizas_Detalles.Campo_Partida] = Dt_Partidas_Polizas.Rows.Count + 1;
               // row["CODIGO_PROGRAMATICO"] = Codigo_Programatico;
                row[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID] = Cmb_Descripcion.SelectedValue;
                row[Cat_Con_Cuentas_Contables.Campo_Cuenta] = Convert.ToString(Txt_Cuenta_Contable.Text.ToString());
                row[Ope_Con_Polizas_Detalles.Campo_Concepto] = Txt_Concepto_Partida.Text.ToString();
                row[Ope_Con_Polizas_Detalles.Campo_Debe] = Convert.ToDouble(Txt_Debe_Partida.Text.ToString());
                row[Ope_Con_Polizas_Detalles.Campo_Haber] = Convert.ToDouble(Txt_Haber_Partida.Text.ToString());

                Dt_Partidas_Polizas.Rows.Add(row); //Agrega el registro creado con todos sus valores a la tabla
                Dt_Partidas_Polizas.AcceptChanges();
                Session["Dt_Partidas_Poliza"] = Dt_Partidas_Polizas;//Agrega los valores del registro a la sesión

                Grid_Detalles_Poliza.Columns[1].Visible = true;

                Grid_Detalles_Poliza.DataSource = Dt_Partidas_Polizas; //Agrega los valores de todas las partidas que se tienen al grid
                Grid_Detalles_Poliza.DataBind();
                Grid_Detalles_Poliza.Columns[1].Visible = false;
                Txt_Cuenta_Contable.Text = "";
                Txt_Debe_Partida.Text = "";
                Txt_Haber_Partida.Text = "";
                Txt_Debe_Partida.Enabled = true;
                Txt_Haber_Partida.Enabled = true;
                Txt_Concepto_Partida.Text = "";
               // Txt_Nombre_Presupuestal.Text = "";
                ///Txt_Clave_Presupuestal.Text = "";
                //Txt_No_Compromiso.Text = "";
               // Txt_Monto_Comprometido.Text = "";
               // Txt_Partida_ID.Text = "";
               // Txt_Monto_Disponible.Text = "";
                Txt_No_Poliza.Text = "";
              //  Cmb_Unidad_Responsable.Items.Clear();
               // Cmb_Programa.Items.Clear();
               // Cmb_Fuente_Financiamiento.Items.Clear();
               // Cmb_Area_Funcional.Items.Clear();
                Cmb_Descripcion.SelectedIndex = -1;
              //  Cmb_Afectable.SelectedIndex = 0;
                //Cmb_Afectable_SelectedIndexChanged(sender, e);
              //  Cmb_Afectable.Enabled = false;
               // Cmb_No_Compromiso.Items.Clear();

                Montos_Debe_Haber_Poliza(); //Obtiene el monto Total del Debe y Haber de la Poliza
            
        }

        //Indica al usuario que datos son los que falta por proporcionar para poder agregar la partida a la poliza
        else
        {
            error = 1;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
            if (Cmb_Descripcion.SelectedIndex == -1)
            {
                Lbl_Mensaje_Error.Text += Espacios + " + Numero de Cuenta <br>";
            }
            if (String.IsNullOrEmpty(Txt_Concepto_Partida.Text))
            {
                Lbl_Mensaje_Error.Text += Espacios + " + Concepto de la Partida de la Cuenta Contable <br>";
            }
            if (String.IsNullOrEmpty(Txt_Debe_Partida.Text) && String.IsNullOrEmpty(Txt_Haber_Partida.Text))
            {
                Lbl_Mensaje_Error.Text += Espacios + " + El Monto del Debe o Haber de la partida <br>";
            }
            //if (Txt_Partida_ID.Text != "")
            //{
            //    Lbl_Mensaje_Error.Text += Espacios + " + Debe ingresar el codigo programatico completo. <br>";
            //}
        }
        Txt_Debe_Partida_TextChanged(this, new EventArgs());
        Txt_Haber_Partida_TextChanged(this, new EventArgs());
        if (error==1){
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
        //Upnl_Partidas_Polizas.Update();
        //Cbx_Comprometido.Checked = false;
    }
    protected void Btn_Eliminar_Partida(object sender, EventArgs e)
    {
        Int32 Count_Fila = 1;
        ImageButton Btn_Eliminar_Partida_Poliza = (ImageButton)sender;
        DataTable Dt_Partidas = (DataTable)Session["Dt_Partidas_Poliza"];
        DataRow[] Filas = Dt_Partidas.Select(Ope_Con_Polizas_Detalles.Campo_Partida +
                "='" + Btn_Eliminar_Partida_Poliza.CommandArgument + "'");

        if (!(Filas == null))
        {
            if (Filas.Length > 0)
            {
                Dt_Partidas.Rows.Remove(Filas[0]);

                //Suma todos los Debe y Haber de la póliza
                foreach (DataRow Registro in Dt_Partidas.Rows)
                {
                    Registro[Ope_Con_Polizas_Detalles.Campo_Partida] = Count_Fila;
                    Count_Fila = Convert.ToInt32(Count_Fila) + 1;
                }
                Session["Dt_Partidas_Poliza"] = Dt_Partidas;
                Grid_Detalles_Poliza.Columns[1].Visible = true;
                Grid_Detalles_Poliza.Columns[2].Visible = true;
                Grid_Detalles_Poliza.Columns[3].Visible = true;
                Grid_Detalles_Poliza.Columns[4].Visible = true;
                Grid_Detalles_Poliza.Columns[5].Visible = true;
                Grid_Detalles_Poliza.Columns[6].Visible = true;
                Grid_Detalles_Poliza.DataSource = (DataTable)Session["Dt_Partidas_Poliza"];
                Grid_Detalles_Poliza.DataBind();
                Grid_Detalles_Poliza.Columns[1].Visible = false;
                //Grid_Detalles_Poliza.Columns[2].Visible = false;
                //Grid_Detalles_Poliza.Columns[3].Visible = false;
                //Grid_Detalles_Poliza.Columns[4].Visible = false;
                //Grid_Detalles_Poliza.Columns[5].Visible = false;
                //Grid_Detalles_Poliza.Columns[6].Visible = false;

                Montos_Debe_Haber_Poliza(); //Obtiwene el monto Total del Debe y Haber de la Poliza
            }
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cmb_Descripcion_SelectedIndexChanged
    /// DESCRIPCION : Consulta la cuenta contable de la descripción de la cuentan
    ///               contable que fue seleccionada por el usuario
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 11/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Cmb_Descripcion_SelectedIndexChanged(object sender, EventArgs e)
        {
        Cls_Cat_Con_Cuentas_Contables_Negocio Rs_Consulta_Con_Cuentas_Contables = new Cls_Cat_Con_Cuentas_Contables_Negocio(); //Variable de conexión a la capa de negocios
        DataTable Dt_Cuenta_Contable = null; //Obtiene la cuenta contable de la descripción que fue seleccionada por el usuario
        Cls_Ope_Con_Polizas_Negocio Rs_Consulta_Partidas_Especificas = new Cls_Ope_Con_Polizas_Negocio(); //Variable de conexión a la capa de negocios
       // DataTable Dt_Partidas_Especificas = null;   //Almacena la partida especifica asociada
        Cls_Ope_Con_Polizas_Negocio Rs_Consulta_Presupuesto = new Cls_Ope_Con_Polizas_Negocio();    //Variable de conexión a la capa de negocios
       // DataTable Dt_Presupuesto = null;    //Almacena el ID de la Dependencia asociada
        Cls_Ope_Con_Compromisos_Negocio Rs_Compromisos = new Cls_Ope_Con_Compromisos_Negocio(); //Variable de conexion con la capa de negocio
      //  DataTable Dt_Compromisos = null;    //Almacena los compromisos encontrados.
        //string[] Dependencias_ID;   //Almacena los IDs de las Dependencias
       // int Cont_Dependencias = 0;  //Almacena el numero de IDs en la variable Dependencias_ID
       // Double Monto_Comprometido = 0;  //Almacena el Monto Total comprometido de la Cuenta Contable ingresada.

        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
           // Cmb_Area_Funcional.Items.Clear();
            //Cmb_Fuente_Financiamiento.Items.Clear();
            //Cmb_Programa.Items.Clear();
            //Cmb_Unidad_Responsable.Items.Clear();
            //Cmb_Area_Funcional.Enabled = false;
           // Cmb_Fuente_Financiamiento.Enabled = false;
           // Cmb_Programa.Enabled = false;
           // Cmb_Afectable.SelectedIndex = 0;
           // Cmb_Afectable_SelectedIndexChanged(sender, e);
           // Cmb_Afectable.Enabled = false;
           // Cmb_No_Compromiso.Items.Clear();
           // Cmb_No_Compromiso.Enabled = false;
            //Txt_No_Compromiso.Text = "";
          //  Txt_Monto_Comprometido.Text = "";
          //  Txt_Monto_Disponible.Text = "";

            Rs_Consulta_Con_Cuentas_Contables.P_Cuenta_Contable_ID = Cmb_Descripcion.SelectedValue;
            Dt_Cuenta_Contable = Rs_Consulta_Con_Cuentas_Contables.Consulta_Datos_Cuentas_Contables();  //Consulta los datos de la cuenta contable seleccionada
            if (Dt_Cuenta_Contable.Rows.Count > 0)
            {
                //Agrega la cuenta contable a la caja de texto correspondiente
                foreach (DataRow Registro in Dt_Cuenta_Contable.Rows)
                {
                    //Rs_Consulta_Partidas_Especificas.P_Cuenta = Registro[Cat_Con_Cuentas_Contables.Campo_Cuenta].ToString();
                    //Dt_Partidas_Especificas = Rs_Consulta_Partidas_Especificas.Consulta_Partida_Especifica();   //Consulta la partida especifica ligada a la cuenta
                    Txt_Cuenta_Contable.Text = Aplicar_Mascara_Cuenta_Contable(Registro[Cat_Con_Cuentas_Contables.Campo_Cuenta].ToString()).ToString();
                    //Rs_Compromisos.P_Cuenta_Contable_ID = Registro[Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID].ToString();
                    //Rs_Compromisos.P_Estatus = "COMPROMETIDO";
                    //Dt_Compromisos = Rs_Compromisos.Consulta_Compromisos();

                    if (String.IsNullOrEmpty(Txt_Concepto_Poliza.Text))
                        Txt_Concepto_Partida.Text = Cmb_Descripcion.SelectedItem.Text.ToString();
                    else
                        Txt_Concepto_Partida.Text = Txt_Concepto_Poliza.Text;
                }
                //if (Dt_Partidas_Especificas.Rows.Count > 0) //Verifica si existe al menos una partida especifica
                //{
                //    foreach (DataRow Registro in Dt_Partidas_Especificas.Rows)  //Asigna los valores correspondientes a los controles
                //    {
                //      //  Txt_Clave_Presupuestal.Text = Registro[Cat_Sap_Partidas_Especificas.Campo_Clave].ToString().Substring(0, 4);
                //      //  Txt_Nombre_Presupuestal.Text = Registro[Cat_Sap_Partidas_Especificas.Campo_Nombre].ToString();
                //      //  Txt_Partida_ID.Text = Registro[Cat_Sap_Partidas_Especificas.Campo_Partida_ID].ToString();
                //        Rs_Consulta_Presupuesto.P_Partida_ID = Registro[Cat_Com_Dep_Presupuesto.Campo_Partida_ID].ToString();
                //        Dt_Presupuesto = Rs_Consulta_Presupuesto.Consulta_Dependencia_Partida_ID(); //Consulta las dependencias ligadas a esa partida.
                //    }
                //    if (Dt_Presupuesto.Rows.Count > 0)
                //    {
                //        Dependencias_ID = new string[Dt_Presupuesto.Rows.Count];
                //        foreach (DataRow Registro in Dt_Presupuesto.Rows)
                //        {
                //            Dependencias_ID[Cont_Dependencias] = Registro[Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID].ToString();
                //            Cont_Dependencias++;
                //        }
                //    //    Llenar_Cmb_Dependencia(Dependencias_ID, Cont_Dependencias); //Manda los datos para llenar el combo Dependencias.
                //     //   if (Cmb_Unidad_Responsable.Items.Count == 0)
                //     //   {
                //       //     Txt_Debe_Partida.Enabled = true;
                //      //      Txt_Haber_Partida.Enabled = true;
                //      //  }
                //    }
                //}
                //else
                //{
                //  //  Txt_Clave_Presupuestal.Text = "";
                //  //  Txt_Nombre_Presupuestal.Text = "";
                //  //  Txt_Partida_ID.Text = "";
                //}
            }

            Upnl_Partidas_Polizas.Update();
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Txt_Cuenta_Contable_TextChanged
    /// DESCRIPCION : Consulta la Descripción más cercana de la cuenta que esta 
    ///               proporcionando el usuario
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 11/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Txt_Cuenta_Contable_TextChanged(object sender, EventArgs e)
    {
        Cls_Cat_Con_Cuentas_Contables_Negocio Rs_Consulta_Cat_Con_Cuentas_Contables = new Cls_Cat_Con_Cuentas_Contables_Negocio();  //Variable de conexion con la capa de negocio
        Cls_Ope_Con_Polizas_Negocio Rs_Consulta_SAP_Partidas_Especificas = new Cls_Ope_Con_Polizas_Negocio(); //Variable de conexion con la capa de negocio
        Cls_Ope_Con_Polizas_Negocio Rs_Consulta_Presupuesto = new Cls_Ope_Con_Polizas_Negocio();    //Variable de conexión a la capa de negocios
        //DataTable Dt_Presupuesto = null;    //Almacena el ID de la Dependencia asociada
        DataTable Dt_Descripcion_Cuenta_Contable;   //Almacena la descripcion de la cuenta contable proporcionada.
        //DataTable Dt_Partidas_Especificas = null;   //Almacena las partidas especificas.
        //string[] Dependencias_ID;   //Almacena los IDs de las Dependencias
        //int Cont_Dependencias = 0;  //Almacena el numero de IDs en la variable Dependencias_ID
       // Double Monto_Comprometido = 0;  //Almacena el Monto Total comprometido de la Cuenta Contable ingresada.

        try
        {
            if (Validar_Mascara_Cuenta_Contable(Txt_Cuenta_Contable.Text))
            {
                Rs_Consulta_Cat_Con_Cuentas_Contables.P_Descripcion = Txt_Cuenta_Contable.Text;
                //Rs_Consulta_SAP_Partidas_Especificas.P_Cuenta = Txt_Cuenta_Contable.Text;
                //Dt_Partidas_Especificas = Rs_Consulta_SAP_Partidas_Especificas.Consulta_Partida_Especifica();   //Consulta las partidas especificas
                Dt_Descripcion_Cuenta_Contable = Rs_Consulta_Cat_Con_Cuentas_Contables.Consulta_Datos_Cuentas_Contables();  //Consulta las cuentas contables

                //if (Dt_Partidas_Especificas.Rows.Count > 0)
                //{
                //    foreach (DataRow Registro in Dt_Partidas_Especificas.Rows)
                //    {
                //      //  Txt_Clave_Presupuestal.Text = Registro[Cat_Sap_Partidas_Especificas.Campo_Clave].ToString();
                //      //  Txt_Nombre_Presupuestal.Text = Registro[Cat_Sap_Partidas_Especificas.Campo_Nombre].ToString();
                //      //  Txt_Partida_ID.Text = Registro[Cat_Sap_Partidas_Especificas.Campo_Partida_ID].ToString();
                //        Rs_Consulta_Presupuesto.P_Partida_ID = Registro[Cat_Com_Dep_Presupuesto.Campo_Partida_ID].ToString();
                //        Dt_Presupuesto = Rs_Consulta_Presupuesto.Consulta_Dependencia_Partida_ID(); //Consulta las dependencias ligadas a esa partida.
                //    }
                //    if (Dt_Presupuesto.Rows.Count > 0)
                //    {
                //        Dependencias_ID = new string[Dt_Presupuesto.Rows.Count];
                //        foreach (DataRow Registro in Dt_Presupuesto.Rows)
                //        {
                //            Dependencias_ID[Cont_Dependencias] = Registro[Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID].ToString();
                //            Cont_Dependencias++;
                //        }
                //     //   Llenar_Cmb_Dependencia(Dependencias_ID, Cont_Dependencias); //Manda los datos para llenar el combo Dependencias.
                //    }
                //}
                
                //Indica en donde se encuentra posiblemente la cuenta contable que fue agregada por el usuario
                foreach (DataRow Registro in Dt_Descripcion_Cuenta_Contable.Rows)
                {
                    Cmb_Descripcion.SelectedValue = Registro[Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID].ToString();
                    Consulta_Cuenta_Contable();
                    if (String.IsNullOrEmpty(Txt_Concepto_Partida.Text))
                    {
                        Txt_Concepto_Partida.Text = Cmb_Descripcion.SelectedItem.Text.ToString();
                    }
                }
                Txt_Cuenta_Contable.Text = Aplicar_Mascara_Cuenta_Contable(Txt_Cuenta_Contable.Text);
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
       // upd_panel
    }
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Ope_Con_Cierre_Mensual_Negocio Rs_Cierre = new Cls_Ope_Con_Cierre_Mensual_Negocio();  //Variable de conexion con la capa de negocio
        DataTable Dt_Cierre = null;
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (Btn_Nuevo.ToolTip == "Nuevo")
            {
                Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                Limpia_Controles();           //Limpia los controles de la forma para poder introducir nuevos datos
                Txt_Fecha_Poliza.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Now).ToString();
                Txt_Empleado_Creo.Text  = Cls_Sessiones.Nombre_Empleado;
                //Cmb_Empleado_Creo.Items.Insert(0, new ListItem(Cls_Sessiones.Nombre_Empleado, ""));
            }
            else
            {
                //Valida los datos ingresados por el usuario.
                if (Validar_Datos_Poliza())
                {
                    if (Convert.ToDouble(Txt_Total_Debe.Text.ToString().Replace(",", "")) == Convert.ToDouble(Txt_Total_Haber.Text.ToString().Replace(",", "")))
                    {
                        if (Convert.ToInt16(String.Format("{0:MM}", Convert.ToDateTime(Txt_Fecha_Poliza.Text))) < Convert.ToInt16(String.Format("{0:MM}", DateTime.Now)))
                        {
                            Rs_Cierre.P_Anio = String.Format("{0:yyyy}", Convert.ToDateTime(Txt_Fecha_Poliza.Text));
                            Rs_Cierre.P_Mes = String.Format("{0:MMMM}", Convert.ToDateTime(Txt_Fecha_Poliza.Text)).ToUpper();
                            Dt_Cierre = Rs_Cierre.Consulta_Cierre_General();
                            if (Dt_Cierre.Rows.Count > 0)
                            {
                                if (Dt_Cierre.Rows[0]["Estatus"].ToString() == "CERRADO")
                                {
                                    Btn_Password.Visible = true;
                                    Btn_Password_Click(sender, e);
                                }
                            }
                        }
                        if (Convert.ToDateTime(Txt_Fecha_Poliza.Text) > DateTime.Now)
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + No puede capturar una poliza posterior a esta fecha Actual. <br>";
                        }
                        if (Convert.ToInt16(String.Format("{0:MM}", Convert.ToDateTime(Txt_Fecha_Poliza.Text))) == Convert.ToInt16(String.Format("{0:MM}", DateTime.Now)))
                        {
                            Alta_Poliza(); //Da de Alta el detalles de la póliza y sus características
                        }
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El total Debe y el Total Haber deben ser iguales <br>";
                    }
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
                if (Txt_No_Poliza.Text != "")
                {
                    Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione la Poliza que desea modificar <br>";
                }
            }
            else
            {
                //Valida los datos ingresados por el usuario.
                if (Validar_Datos_Poliza())
                {
                    if (Convert.ToDouble(Txt_Total_Debe.Text.ToString().Replace(",", "")) == Convert.ToDouble(Txt_Total_Haber.Text.ToString().Replace(",", "")))
                    {
                        Modificar_Poliza(); //Modifica el detalles de la póliza y sus características
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El total Debe y el Total Haber deben ser iguales <br>";
                    }
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
    protected void Btn_Copiar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Habilitar_Controles("Carga");
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
            //Si el usuario selecciono un Sindicato entonces lo elimina de la base de datos
            if (Txt_No_Poliza.Text != "")
            {
                Eliminar_Poliza(); //Elimina la Poliza y sus Detalles que fue seleccionado por el usuario
            }
            //Si el usuario no selecciono alguna póliza manda un mensaje indicando que es necesario que seleccione algun para
            //poder eliminar
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Seleccione la Póliza que desea eliminar <br>";
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
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Txt_Debe_Partida_TextChanged
    /// DESCRIPCION : Verfica si el contenido de la caja de texto cambio.
    /// CREO        : Salvador L. Rea Ayala
    /// FEHA_CREO  : 20/Septiembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Txt_Debe_Partida_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            //Si el control Txt_Debe cambio su contenido bloquea el control Txt_Haber
            if (Txt_Debe_Partida.Text != "")
            {
                Txt_Haber_Partida.ReadOnly = true;
               // if (Txt_Monto_Comprometido.Text != "")
                //{
                  //  if (Convert.ToDouble(Txt_Monto_Comprometido.Text) > 0.0)
                  //  {
                   //     if (Convert.ToDouble(Txt_Debe_Partida.Text) > Convert.ToDouble(Txt_Monto_Disponible.Text))
                    //    {
                    //        Lbl_Mensaje_Error.Visible = true;
                   //         Img_Error.Visible = true;
                     //       Lbl_Mensaje_Error.Text = "El monto de la poliza no debe exceder al disponible.";
                    //        Txt_Debe_Partida.Text = "";
                    //    }
                   // }
             //   }
            }
            else
                Txt_Haber_Partida.ReadOnly = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Txt_Debe_Partida_TextChanged " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Txt_Haber_Partida_TextChanged
    /// DESCRIPCION : Verfica si el contenido de la caja de texto cambio.
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 20/Septiembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Txt_Haber_Partida_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            //Si el control Txt_Haber cambio su contenido bloquea el control Txt_Debe
            if (Txt_Haber_Partida.Text != "")
            {
                Txt_Debe_Partida.ReadOnly = true;
                //if (Txt_Monto_Comprometido.Text != "")
                //{
                //    if (Convert.ToDouble(Txt_Monto_Comprometido.Text) > 0.0)
                //    {
                //        if (Convert.ToDouble(Txt_Haber_Partida.Text) > Convert.ToDouble(Txt_Monto_Disponible.Text))
                //        {
                //            Lbl_Mensaje_Error.Visible = true;
                //            Img_Error.Visible = true;
                //            Lbl_Mensaje_Error.Text = "El monto de la poliza no debe exceder al disponible.";
                //            Txt_Haber_Partida.Text = "";
                //        }
                //    }
                //}
            }
            else
                Txt_Debe_Partida.ReadOnly = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Txt_Haber_Partida_TextChanged " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cmb_Unidad_Responsable_SelectedIndexChanged
    /// DESCRIPCION : Al seleccionar un item automaticamente busca las Ftes. de Financiamiento
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 3/Octubre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    //protected void Cmb_Unidad_Responsable_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    Cls_Ope_Con_Polizas_Negocio Rs_Consulta_Fuentes = new Cls_Ope_Con_Polizas_Negocio(); //Variable de conexión hacia la capa de Negocios
    //    DataTable Dt_Fuentes; //Variable que obtendra los datos de la consulta 
    //    string[] Fuentes_ID; //Almacena los IDs de las Fuentes de Financiamiento encontradas.
    //    int Cont_Fuentes = 0;   //Almacena el nunmero de IDs de las Fuentes de Financiamiento encotradas
    //    try
    //    {
    //        Cmb_Afectable.Enabled = false;
    //        Cmb_Afectable.SelectedIndex = 0;
    //        Cmb_Afectable_SelectedIndexChanged(sender, e);
    //        Cmb_Area_Funcional.Items.Clear();
    //        Cmb_Programa.Items.Clear();
    //        Cmb_Fuente_Financiamiento.Items.Clear();
    //        Cmb_Area_Funcional.Enabled = false;
    //        Cmb_Programa.Enabled = false;
    //        Cmb_Fuente_Financiamiento.Enabled = false;
    //        Cmb_Programa.Enabled = true;
    //        Rs_Consulta_Fuentes.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue.ToString(); //Consulta las Fuentes de Financiamiento de acuerdo a la Dependencia seleccionada.
    //        Rs_Consulta_Fuentes.P_Partida_ID = Txt_Partida_ID.Text;
    //        Dt_Fuentes = Rs_Consulta_Fuentes.Consulta_Dependencia_Programa_ID();
    //        Fuentes_ID = new string[Dt_Fuentes.Rows.Count];
    //        foreach (DataRow Registro in Dt_Fuentes.Rows)
    //        {
    //            Fuentes_ID[Cont_Fuentes] = Registro[Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID].ToString();
    //            Cont_Fuentes++;
    //        }
    //        Llenar_Cmb_Proyectos_Programas(Fuentes_ID, Cont_Fuentes);   //Manda los datos para llenar el Combo de Proyectos y Programas

    //        if (Cmb_Programa.Items.Count == 2)
    //        {
    //            Cmb_Programa.SelectedIndex = 1;
    //            Cmb_Programa_SelectedIndexChanged(sender, e);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception("Cmb_Unidad_Responsable_SelectedIndexChanged " + ex.Message.ToString(), ex);
    //    }
    //}
    //protected void Cmb_Fuente_Financiamiento_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    Cls_Ope_Con_Polizas_Negocio Rs_Area_Funcional = new Cls_Ope_Con_Polizas_Negocio();
    //    DataTable Dt_Area_Funcional = null; //Variable que obtendra los datos de la consulta 
    //    string[] Area_Funcional_ID; //Almacena los IDs de las Areas Funcionales encontradas
    //    int Cont_IDs = 0;   //Almacena el numero de IDs de las Areas Funcionales encontradas

    //    try
    //    {
    //        Cmb_Afectable.Enabled = false;
    //        Cmb_Afectable.SelectedIndex = 0;
    //        Cmb_Afectable_SelectedIndexChanged(sender, e);
    //        Cmb_Area_Funcional.Items.Clear();
    //        Cmb_Area_Funcional.Enabled = true;
    //        Rs_Area_Funcional.P_Fuente_Financiamiento_ID = Cmb_Fuente_Financiamiento.SelectedValue.ToString();  //Consulta las Areas Funcionales de acuerdo a la Fuente de Financiamiento seleccionada.
    //        Rs_Area_Funcional.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue.ToString();
    //        Rs_Area_Funcional.P_Programa_ID = Cmb_Programa.SelectedValue.ToString();
    //        Dt_Area_Funcional = Rs_Area_Funcional.Consulta_Fte_Area_Funcional_ID();
    //        Area_Funcional_ID = new string[Dt_Area_Funcional.Rows.Count];
    //        foreach (DataRow Registro in Dt_Area_Funcional.Rows)
    //        {
    //            Area_Funcional_ID[Cont_IDs] = Registro[Cat_Com_Dep_Presupuesto.Campo_Area_Funcional_ID].ToString();
    //            Cont_IDs++;
    //        }

    //        Llenar_Cmb_Area_Funcional(Area_Funcional_ID, Cont_IDs); //Manda los datos para llenar el Combo del Area Funcional

    //        if (Cmb_Area_Funcional.Items.Count == 2)
    //        {
    //            Cmb_Area_Funcional.SelectedIndex = 1;
    //            Cmb_Area_Funcional_SelectedIndexChanged(sender, e);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception("Cmb_Unidad_Responsable_SelectedIndexChanged " + ex.Message.ToString(), ex);
    //    }
    //}
    //protected void Cmb_Programa_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    Cls_Ope_Con_Polizas_Negocio Rs_Consulta_Fuente = new Cls_Ope_Con_Polizas_Negocio();
    //    DataTable Dt_Fuentes = null; //Variable que obtendra los datos de la consulta 
    //    string[] Fuentes_ID;    //Almacena los IDs de las Fuentes de Financiamiento encontradas.
    //    int Cont_IDs = 0;   //Almacena el numero de Fuentes de Financiamiento encontradas.

    //    try
    //    {
    //        Cmb_Afectable.Enabled = false;
    //        Cmb_Afectable.SelectedIndex = 0;
    //        Cmb_Afectable_SelectedIndexChanged(sender, e);
    //        Cmb_Area_Funcional.Items.Clear();
    //        Cmb_Fuente_Financiamiento.Items.Clear();
    //        Cmb_Area_Funcional.Enabled = false;
    //        Cmb_Fuente_Financiamiento.Enabled = true;
    //        Rs_Consulta_Fuente.P_Programa_ID = Cmb_Programa.SelectedValue.ToString();   //Consulta las fuentes de financiamiento de acuerdo al programa seleccionado.
    //        Rs_Consulta_Fuente.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue.ToString();
    //        Dt_Fuentes = Rs_Consulta_Fuente.Consulta_Programa_Fuente_ID();
    //        Fuentes_ID = new string[Dt_Fuentes.Rows.Count];
    //        foreach (DataRow Registro in Dt_Fuentes.Rows)
    //        {
    //            Fuentes_ID[Cont_IDs] = Registro[Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID].ToString();
    //            Cont_IDs++;
    //        }

    //        Llenar_Cmb_Fuentes_Financiamiento(Fuentes_ID, Cont_IDs); //Manda los datos para llenar el combo.

    //        if (Cmb_Fuente_Financiamiento.Items.Count == 2)
    //        {
    //            Cmb_Fuente_Financiamiento.SelectedIndex = 1;
    //            Cmb_Fuente_Financiamiento_SelectedIndexChanged(sender, e);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception("Cmb_Unidad_Responsable_SelectedIndexChanged " + ex.Message.ToString(), ex);
    //    }
    //}
    //protected void Cmb_Area_Funcional_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    Cls_Ope_Con_Compromisos_Negocio Rs_Compromisos = new Cls_Ope_Con_Compromisos_Negocio(); //Variable de conexion con la capa de negocios.
    //    DataTable Dt_Compromisos = null;    //Almacena el compromiso de acuerdo a los datos proporcionados.
    //    try
    //    {
    //        //Cbx_Comprometido.Enabled = true;
    //        if(Txt_Partida_ID.Text != "")
    //        {
    //            Rs_Compromisos.P_Partida_ID = Txt_Partida_ID.Text;
    //        }

    //        Rs_Compromisos.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue.ToString();
    //        Rs_Compromisos.P_Fuente_Financiamiento_ID = Cmb_Fuente_Financiamiento.SelectedValue.ToString();
    //        Rs_Compromisos.P_Proyecto_Programa_ID = Cmb_Programa.SelectedValue.ToString();
    //        Rs_Compromisos.P_Area_Funcional_ID = Cmb_Area_Funcional.SelectedValue.ToString();
    //        Dt_Compromisos = Rs_Compromisos.Consulta_Compromisos();
            
    //        //********************
    //        Cmb_Afectable.Enabled = true;
    //        Cmb_No_Compromiso.Items.Clear();
    //        Cmb_No_Compromiso.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
    //        int Cont_Compromisos = 1;

    //        if (Dt_Compromisos.Rows.Count > 0)
    //        {
    //            foreach (DataRow Registro in Dt_Compromisos.Rows)
    //            {
    //                Cmb_No_Compromiso.Items.Insert(Cont_Compromisos, new ListItem(Registro["NO_COMPROMISO"].ToString(), Registro["NO_COMPROMISO"].ToString()));
    //                Cont_Compromisos++;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception("Cmb_Area_Funcional_SelectedIndexChanged " + ex.Message.ToString(), ex);
    //    }
    //}
    protected void Txt_Empleado_Autorizo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Cls_Ope_Con_Polizas_Negocio Rs_Cat_Empleados = new Cls_Ope_Con_Polizas_Negocio(); //Variable de conexion con la capa de negocios.
            DataTable Dt_Empleados = null;  //Almacena los datos de los empleados encontrados.
            int Cont_Empleados = 1; //Contador para cambiar la posicion de insercion al combo.
           if (Txt_Empleado_Autorizo.Text == "")
           {
            Txt_Empleado_Autorizo.Text="0";
           }

           if (Es_Numero(Txt_Empleado_Autorizo.Text.Trim()))
            { 
                Rs_Cat_Empleados.P_Empleado_ID  = String.Format("{0:000000}", Txt_Empleado_Autorizo.Text).ToString();
                Rs_Cat_Empleados.P_Nombre = "";
                Cmb_Nombre_Empleado.Items.Clear();
            }else
            {
                Rs_Cat_Empleados.P_Empleado_ID = "";
                Rs_Cat_Empleados.P_Nombre = Txt_Empleado_Autorizo.Text.ToString();
                Cmb_Nombre_Empleado.Items.Clear();
            }
            Dt_Empleados = Rs_Cat_Empleados.Consulta_Empleados_Especial();
            Cmb_Nombre_Empleado.Items.Insert(0, new ListItem("<- Seleccione ->", ""));

            foreach (DataRow Registro in Dt_Empleados.Rows) //Agrega los Items al combo.
            {
                Cmb_Nombre_Empleado.Items.Insert(Cont_Empleados, new ListItem(Registro["EMPLEADO"].ToString(), Registro[Cat_Empleados.Campo_Empleado_ID].ToString()));
                Cont_Empleados++;
            }
            if (Cont_Empleados > 2) //Si existe mas de un empleado da la opcion de seleccionar
            {
                Cmb_Nombre_Empleado.SelectedIndex = 0;
            }
            else    //De lo contrario selecciona el unico registro encontrado.
            {
                Cmb_Nombre_Empleado.SelectedIndex = 1;
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    //protected void Cmb_Afectable_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        Lbl_Mensaje_Error.Visible = false;
    //        Img_Error.Visible = false;

    //        switch (Cmb_Afectable.SelectedIndex)
    //        {
    //            case 0:
    //                Cmb_No_Compromiso.Items.Clear();
    //                Cmb_No_Compromiso.Enabled = false;
    //                break;
    //            case 1:
    //                Cls_Ope_SAP_Dep_Presupuesto_Negocio Rs_Presupuesto = new Cls_Ope_SAP_Dep_Presupuesto_Negocio(); //Variable de conexion con la capa de Negocios.
    //                DataTable Dt_Presupuesto = null;    //Almacenara los registros encontrados en la tabla de Presupuestos.

    //                Cmb_No_Compromiso.SelectedIndex = -1;
    //                Cmb_No_Compromiso.Enabled = false;
    //                Cmb_No_Compromiso.Visible = false;
    //                Lbl_compromiso.Visible = false;
    //                Rs_Presupuesto.P_Partida_ID = Txt_Partida_ID.Text;
    //                Rs_Presupuesto.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue.ToString();
    //                Rs_Presupuesto.P_Fuente_Financiamiento_ID = Cmb_Fuente_Financiamiento.SelectedValue.ToString();
    //                Rs_Presupuesto.P_Programa_ID = Cmb_Programa.SelectedValue.ToString();
    //                Dt_Presupuesto = Rs_Presupuesto.Consulta_Datos_Presupuestos();
    //                foreach (DataRow Registro in Dt_Presupuesto.Rows)
    //                {
    //                    Txt_Monto_Disponible.Text = Convert.ToDouble(Registro[Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible].ToString()).ToString();
    //                    Txt_Monto_Comprometido.Text = Txt_Monto_Disponible.Text;
    //                }
    //                break;
    //            case 2:
    //                if (Cmb_No_Compromiso.Items.Count > 1)
    //                {
    //                    Cmb_No_Compromiso.Visible = true;
    //                    Lbl_compromiso.Visible = true;
    //                    Cmb_No_Compromiso.Enabled = true;
    //                    Txt_Monto_Comprometido.Text = "";
    //                }
    //                else
    //                {
    //                    Lbl_Mensaje_Error.Visible = true;
    //                    Img_Error.Visible = true;
    //                    Lbl_Mensaje_Error.Text = "Esta partida especifica no tiene compromisos";
    //                    Cmb_No_Compromiso.Enabled = false;
    //                    Cmb_Afectable.SelectedIndex = 1;
    //                    Cmb_Afectable_SelectedIndexChanged(sender, e);
    //                }
    //                break;
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        Lbl_Mensaje_Error.Visible = true;
    //        Img_Error.Visible = true;
    //        Lbl_Mensaje_Error.Text = Ex.Message.ToString();
    //    }
    //}
    //protected void Cmb_No_Compromiso_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        Lbl_Mensaje_Error.Visible = false;
    //        Img_Error.Visible = false;

    //        Cls_Ope_Con_Compromisos_Negocio Rs_Compromisos = new Cls_Ope_Con_Compromisos_Negocio(); //Variable de conexion con la capa de negocios.
    //        DataTable Dt_Compromisos = null;    //Almacena el compromiso de acuerdo a los datos proporcionados.

    //        if (Cmb_No_Compromiso.Items.Count > 0)
    //        {
    //            Rs_Compromisos.P_Compromiso_ID = Cmb_No_Compromiso.SelectedValue.ToString();
    //            Dt_Compromisos = Rs_Compromisos.Consulta_Compromisos();

    //            foreach (DataRow Registro in Dt_Compromisos.Rows)
    //            {
    //                Txt_Monto_Comprometido.Text = Registro[Ope_Con_Compromisos.Campo_Monto_Comprometido].ToString();
    //                Txt_Monto_Disponible.Text = Registro[Ope_Con_Compromisos.Campo_Monto_Comprometido].ToString();
    //            }
    //            Txt_Debe_Partida.Enabled = true;
    //            Txt_Haber_Partida.Enabled = false;
    //        }
    //        else
    //        {
    //            Txt_Monto_Comprometido.Text = "0";
    //            Txt_Monto_Disponible.Text = "0";
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        Lbl_Mensaje_Error.Visible = true;
    //        Img_Error.Visible = true;
    //        Lbl_Mensaje_Error.Text = Ex.Message.ToString();
    //    }
    //}
    protected void Btn_Password_Click(object sender, EventArgs e)
    {
        Txt_No_Empleado_Popup.Text = "";
        Txt_Password_Popup.Text = "";
        Mpe_Autorizar_Password.Show();
    }
    #endregion

    #region (ModalPopup)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Cerrar_Ventana_Click
    /// DESCRIPCION : Cierra la ventana de busqueda de Polizas.
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 22/Septiembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Cerrar_Ventana_Click(object sender, ImageClickEventArgs e)
    {
        Mpe_Busqueda_Polizas.Hide();    //Oculta el ModalPopUp
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Cerrar_Ventana_Carga_Click
    /// DESCRIPCION : Cierra la ventana de busqueda de Polizas.
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 22/Septiembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Cerrar_Ventana_Carga_Click(object sender, ImageClickEventArgs e)
    {
        Mpe_Carga_Masiva.Hide();
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Cerrar_Ventana_Password_Click
    /// DESCRIPCION : Cierra la ventana de Autorizacion de Polizas
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 29/Octubre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Cerrar_Ventana_Password_Click(object sender, ImageClickEventArgs e)
    {
        Mpe_Autorizar_Password.Hide();   
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Busqueda_Poliza_Popup_Click
    /// DESCRIPCION : Ejecuta la busqueda de Polizas
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 22/Septiembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Busqueda_Poliza_Popup_Click(object sender, EventArgs e)
    {
        try
        {
            Consulta_Poliza_Avanzada(); //Consulta las polizas de acuerdo a la informacion proporcionada.
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    protected void Btn_Carga_Masiva_Popup_Click(object sender, EventArgs e)
    {
        try
        {
            Interpretar_Excel();    //Interpreta el archivo de Excel cargado.
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    protected void AFU_Archivo_Excel_UploadedComplete(object sender, AsyncFileUploadEventArgs e)
    {
        //****************************************
        //CUANDO LA CARGA DEL ARCHIVO TERMINA LO
        //GUARDA EN LA UBICACION PRESTABLECIDA
        //****************************************
        try
        {
            if (AFU_Archivo_Excel.HasFile)
            {
                string currentDateTime = String.Format("{0:yyyy-MM-dd_HH.mm.sstt}", DateTime.Now);
                string fileNameOnServer = System.IO.Path.GetFileName(AFU_Archivo_Excel.FileName).Replace(" ", "_");

                bool xls = Path.GetExtension(AFU_Archivo_Excel.PostedFile.FileName).Contains(".xls");
                bool xlsx = Path.GetExtension(AFU_Archivo_Excel.PostedFile.FileName).Contains(".xlsx");

                if (xls || xlsx)
                    AFU_Archivo_Excel.SaveAs(@"C:/Polizas/" + fileNameOnServer);
                else
                    return;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void Btn_Autorizar_Poliza_Popup_Click(object sender, EventArgs e)
    {
        try
        {
            Lbl_Error_Password.Visible = false;
            Img_Error_Password.Visible = false;
            if (Txt_Password_Popup.Text != "" || Txt_No_Empleado_Popup.Text != "")
            {
                Cls_Cat_Empleados_Negocios Rs_Empleados = new Cls_Cat_Empleados_Negocios();
                DataTable Dt_Empleados = null;
                DataTable Dt_EmpleadosRol = null;
                Rs_Empleados.P_No_Empleado = String.Format("{0:000000}", Convert.ToInt16(Txt_No_Empleado_Popup.Text));
                Rs_Empleados.P_Password = Txt_Password_Popup.Text;
                Dt_Empleados = Rs_Empleados.Consulta_Usuario_Password();
                if (Dt_Empleados.Rows.Count == 0)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "El Numero de Empleado o la contraseña no coinciden.";
                }
                else
                {
                    Cls_Ope_Con_Polizas_Negocio Rs_Cat_Empleados = new Cls_Ope_Con_Polizas_Negocio();
                    Rs_Cat_Empleados.P_Empleado_ID = String.Format("{0:000000}", Convert.ToInt16(Txt_No_Empleado_Popup.Text));
                    Dt_EmpleadosRol = Rs_Cat_Empleados.Consulta_GrupoRol();
                    if (Dt_EmpleadosRol.Rows[0]["Grupo_Roles_ID"].ToString() == "00003")
                    {
                        Alta_Poliza();
                    }
                    else
                    {
                        Mpe_Autorizar_Password.Hide();
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "El numero de empleado no pertenece a los administradores.";
                    }
                }
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Debes proporcionar el password y la Clave de Empleado.";
            }
            
        }
        catch (Exception Ex)
        {
            Lbl_Error_Password.Visible = true;
            Img_Error_Password.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    #endregion
}