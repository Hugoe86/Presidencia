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
using System.Collections.Generic;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Cierre_Mensual.Negocio;

public partial class paginas_Contabilidad_Frm_Cat_Con_Auxiliar_Cuentas : System.Web.UI.Page
{
    #region (Page_Load)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Page_Load
        /// DESCRIPCION : Carga la configuración inicial de los controles de la página.
        /// CREO        : Salvador L. Rea Ayala
        /// FECHA_CREO  : 15/Septiembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Refresca la session del usuario lagueado al sistema.
                Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
                //Valida que exista algun usuario logueado al sistema.
                if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
                if (!IsPostBack)
                {
                    Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
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

    #region (Metodos)
        #region (Metodos Generales)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Inicializa_Controles
        /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
        ///               diferentes operaciones
        /// PARAMETROS  : 
        /// CREO        : Salvador L. Rea Ayala
        /// FECHA_CREO  : 15/Septiembre/2011
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
        /// FECHA_CREO  : 15/Septiembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Limpia_Controles()
        {
            try
            {
                Txt_Buscar_Cuentas.Text = "";
            }
            catch (Exception ex)
            {
                throw new Exception("Limpia_Controles " + ex.Message.ToString(), ex);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Habilitar_Controles
        /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
        ///                para a siguiente operación
        /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
        ///                           si es una alta, modificacion
        ///                           
        /// CREO        : Salvador L. Rea Ayala
        /// FECHA_CREO  : 15/Septiembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Habilitar_Controles(String Operacion)
        {
            try
            {
                switch (Operacion)
                {
                    case "Inicial":
                        Cls_Ope_Con_Cierre_Mensual_Negocio Rs_Cierre_Mensual = new Cls_Ope_Con_Cierre_Mensual_Negocio();    //Variable de conexion con la capa de Negocios.
                        DataTable Dt_Datos = null;  //Almacenara los datos encontrados.
                        Btn_Salir.ToolTip = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Configuracion_Acceso("Frm_Cat_Con_Auxiliar_Cuentas.aspx");
                        Cmb_Anio.SelectedValue  = DateTime.Now.ToString("yyyy").ToUpper();
                        Rs_Cierre_Mensual.P_Mes_Anio = (Tbc_Meses_Contables.ActiveTabIndex + 1).ToString() + "" + Cmb_Anio.SelectedItem.ToString().Substring(2, 2); //Toma los valores del año y el mes para concatenarlos y almacenarlos.
                        Rs_Cierre_Mensual.P_Mes_Anio = String.Format("{0:0000}", Convert.ToInt16(Rs_Cierre_Mensual.P_Mes_Anio));    //Da el formato correcto para el campo Mes_Anio
                        Dt_Datos = Rs_Cierre_Mensual.Consulta_Cierre_Mensual_Auxiliar();    //Almacena los datos de la consulta
                            Grid_Cuentas_Enero.DataSource = Dt_Datos;
                            Grid_Cuentas_Enero.DataBind();
                            Grid_Cuentas_Enero.Visible = true;
                            break;
                }
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
            }
            catch (Exception ex)
            {
                throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
            }
        }
        #endregion

        #region (Control Acceso Pagina)
        ///*******************************************************************************
        /// NOMBRE: Configuracion_Acceso
        /// DESCRIPCIÓN: Habilita las operaciones que podrá realizar el usuario en la página.
        /// PARÁMETROS  :
        /// USUARIO CREÓ: Salvador L. Rea Ayala
        /// FECHA CREÓ  : 15/Septiembre/2011
        /// USUARIO MODIFICO  :
        /// FECHA MODIFICO    :
        /// CAUSA MODIFICACIÓN:
        ///*******************************************************************************
        protected void Configuracion_Acceso(String URL_Pagina)
        {
            List<ImageButton> Botones = new List<ImageButton>();//Variable que almacenara una lista de los botones de la página.
            DataRow[] Dr_Menus = null;//Variable que guardara los menus consultados.

            try
            {
                //Agregamos los botones a la lista de botones de la página.

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
        /// USUARIO CREÓ: Salvador L. Rea Ayala
        /// FECHA CREÓ  : 15/Septiembre/2011
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
    #endregion

    #region (Eventos)
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
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
        protected void Tbc_Meses_Contables_ActiveTabChanged(object sender, EventArgs e)
        {
            try
            {
                Txt_Buscar_Cuentas.Text = "";
                Cls_Ope_Con_Cierre_Mensual_Negocio Rs_Cierre_Mensual = new Cls_Ope_Con_Cierre_Mensual_Negocio();    //Variable de conexion con la capa de Negocios.
                DataTable Dt_Datos = null;  //Almacenara los datos encontrados.
                if (Cmb_Anio.SelectedIndex > 0) //Verifica que se haya seleccionado un año.
                {
                    Rs_Cierre_Mensual.P_Mes_Anio = (Tbc_Meses_Contables.ActiveTabIndex + 1).ToString() + "" + Cmb_Anio.SelectedItem.ToString().Substring(2, 2); //Toma los valores del año y el mes para concatenarlos y almacenarlos.
                    Rs_Cierre_Mensual.P_Mes_Anio = String.Format("{0:0000}", Convert.ToInt16(Rs_Cierre_Mensual.P_Mes_Anio));    //Da el formato correcto para el campo Mes_Anio
                    Dt_Datos = Rs_Cierre_Mensual.Consulta_Cierre_Mensual_Auxiliar();    //Almacena los datos de la consulta
                    #region Llenar Grids
                    switch (Tbc_Meses_Contables.ActiveTabIndex)
                    {
                        case 0:
                            Grid_Cuentas_Enero.DataSource = Dt_Datos;
                            Grid_Cuentas_Enero.DataBind();
                            Grid_Cuentas_Enero.Visible = true;
                            break;
                        case 1:
                            Grid_Cuentas_Febrero.DataSource = Dt_Datos;
                            Grid_Cuentas_Febrero.DataBind();
                            Grid_Cuentas_Febrero.Visible = true;
                            break;
                        case 2:
                            Grid_Cuentas_Marzo.DataSource = Dt_Datos;
                            Grid_Cuentas_Marzo.DataBind();
                            Grid_Cuentas_Marzo.Visible = true;
                            break;
                        case 3:
                            Grid_Cuentas_Abril.DataSource = Dt_Datos;
                            Grid_Cuentas_Abril.DataBind();
                            Grid_Cuentas_Abril.Visible = true;
                            break;
                        case 4:
                            Grid_Cuentas_Mayo.DataSource = Dt_Datos;
                            Grid_Cuentas_Mayo.DataBind();
                            Grid_Cuentas_Mayo.Visible = true;
                            break;
                        case 5:
                            Grid_Cuentas_Junio.DataSource = Dt_Datos;
                            Grid_Cuentas_Junio.DataBind();
                            Grid_Cuentas_Junio.Visible = true;
                            break;
                        case 6:
                            Grid_Cuentas_Julio.DataSource = Dt_Datos;
                            Grid_Cuentas_Julio.DataBind();
                            Grid_Cuentas_Julio.Visible = true;
                            break;
                        case 7:
                            Grid_Cuentas_Agosto.DataSource = Dt_Datos;
                            Grid_Cuentas_Agosto.DataBind();
                            Grid_Cuentas_Agosto.Visible = true;
                            break;
                        case 8:
                            Grid_Cuentas_Septiembre.DataSource = Dt_Datos;
                            Grid_Cuentas_Septiembre.DataBind();
                            Grid_Cuentas_Septiembre.Visible = true;
                            break;
                        case 9:
                            Grid_Cuentas_Octubre.DataSource = Dt_Datos;
                            Grid_Cuentas_Octubre.DataBind();
                            Grid_Cuentas_Octubre.Visible = true;
                            break;
                        case 10:
                            Grid_Cuentas_Noviembre.DataSource = Dt_Datos;
                            Grid_Cuentas_Noviembre.DataBind();
                            Grid_Cuentas_Noviembre.Visible = true;
                            break;
                        case 11:
                            Grid_Cuentas_Diciembre.DataSource = Dt_Datos;
                            Grid_Cuentas_Diciembre.DataBind();
                            Grid_Cuentas_Diciembre.Visible = true;
                            break;
                        case 12:
                            Grid_Cuentas_Trece.DataSource = Dt_Datos;
                            Grid_Cuentas_Trece.DataBind();
                            Grid_Cuentas_Trece.Visible = true;
                            break;
                    }
                    #endregion
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Tbc_Meses_Contables_ActiveTabChanged: " + Ex.Message);
            }
        }
        protected void Cmb_Anio_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cls_Ope_Con_Cierre_Mensual_Negocio Rs_Cierre_Mensual = new Cls_Ope_Con_Cierre_Mensual_Negocio();    //Variable de conexion con la capa de Negocios.
            DataTable Dt_Datos = null;  //Almacenara los datos encontrados.
            try
            {
                Txt_Buscar_Cuentas.Text = "";
                if (Cmb_Anio.SelectedIndex > 0) //Verifica que se haya seleccionado un año.
                {
                    Rs_Cierre_Mensual.P_Mes_Anio = (Tbc_Meses_Contables.ActiveTabIndex + 1).ToString() + "" + Cmb_Anio.SelectedItem.ToString().Substring(2, 2); //Toma los valores del año y el mes para concatenarlos y almacenarlos.
                    Rs_Cierre_Mensual.P_Mes_Anio = String.Format("{0:0000}", Convert.ToInt16(Rs_Cierre_Mensual.P_Mes_Anio));    //Da el formato correcto para el campo Mes_Anio
                    Dt_Datos = Rs_Cierre_Mensual.Consulta_Cierre_Mensual_Auxiliar();    //Almacena los datos de la consulta
                    #region Llenar Grids
                    switch (Tbc_Meses_Contables.ActiveTabIndex)
                    {
                        case 0:
                            Grid_Cuentas_Enero.DataSource = Dt_Datos;
                            Grid_Cuentas_Enero.DataBind();
                            Grid_Cuentas_Enero.Visible = true;
                            break;
                        case 1:
                            Grid_Cuentas_Febrero.DataSource = Dt_Datos;
                            Grid_Cuentas_Febrero.DataBind();
                            Grid_Cuentas_Febrero.Visible = true;
                            break;
                        case 2:
                            Grid_Cuentas_Marzo.DataSource = Dt_Datos;
                            Grid_Cuentas_Marzo.DataBind();
                            Grid_Cuentas_Marzo.Visible = true;
                            break;
                        case 3:
                            Grid_Cuentas_Abril.DataSource = Dt_Datos;
                            Grid_Cuentas_Abril.DataBind();
                            Grid_Cuentas_Abril.Visible = true;
                            break;
                        case 4:
                            Grid_Cuentas_Mayo.DataSource = Dt_Datos;
                            Grid_Cuentas_Mayo.DataBind();
                            Grid_Cuentas_Mayo.Visible = true;
                            break;
                        case 5:
                            Grid_Cuentas_Junio.DataSource = Dt_Datos;
                            Grid_Cuentas_Junio.DataBind();
                            Grid_Cuentas_Junio.Visible = true;
                            break;
                        case 6:
                            Grid_Cuentas_Julio.DataSource = Dt_Datos;
                            Grid_Cuentas_Julio.DataBind();
                            Grid_Cuentas_Julio.Visible = true;
                            break;
                        case 7:
                            Grid_Cuentas_Agosto.DataSource = Dt_Datos;
                            Grid_Cuentas_Agosto.DataBind();
                            Grid_Cuentas_Agosto.Visible = true;
                            break;
                        case 8:
                            Grid_Cuentas_Septiembre.DataSource = Dt_Datos;
                            Grid_Cuentas_Septiembre.DataBind();
                            Grid_Cuentas_Septiembre.Visible = true;
                            break;
                        case 9:
                            Grid_Cuentas_Octubre.DataSource = Dt_Datos;
                            Grid_Cuentas_Octubre.DataBind();
                            Grid_Cuentas_Octubre.Visible = true;
                            break;
                        case 10:
                            Grid_Cuentas_Noviembre.DataSource = Dt_Datos;
                            Grid_Cuentas_Noviembre.DataBind();
                            Grid_Cuentas_Noviembre.Visible = true;
                            break;
                        case 11:
                            Grid_Cuentas_Diciembre.DataSource = Dt_Datos;
                            Grid_Cuentas_Diciembre.DataBind();
                            Grid_Cuentas_Diciembre.Visible = true;
                            break;
                        case 12:
                            Grid_Cuentas_Trece.DataSource = Dt_Datos;
                            Grid_Cuentas_Trece.DataBind();
                            Grid_Cuentas_Trece.Visible = true;
                            break;
                    }
                    #endregion
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Cmb_Anio_SelectedIndexChanged: " + Ex.Message);
            }
        }
        #region "GRID PAGEINDEX"
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Grid_Cuentas_Enero_PageIndexChanging
        /// DESCRIPCION : Cambia la pagina de la tabla
        ///               
        /// PARAMETROS  : 
        /// CREO        : Sergio Manuel Gallardo Andrade
        /// FECHA_CREO  : 11/noviembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Grid_Cuentas_Enero_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Cls_Ope_Con_Cierre_Mensual_Negocio Rs_Cierre_Mensual = new Cls_Ope_Con_Cierre_Mensual_Negocio();    //Variable de conexion con la capa de Negocios.
            DataTable Dt_Datos = null;  //Almacenara los datos encontrados.
            try
            {
                Limpia_Controles();                        //Limpia todos los controles de la forma
                Grid_Cuentas_Enero.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
                Rs_Cierre_Mensual.P_Mes_Anio = (Tbc_Meses_Contables.ActiveTabIndex + 1).ToString() + "" + Cmb_Anio.SelectedItem.ToString().Substring(2, 2); //Toma los valores del año y el mes para concatenarlos y almacenarlos.
                Rs_Cierre_Mensual.P_Mes_Anio = String.Format("{0:0000}", Convert.ToInt16(Rs_Cierre_Mensual.P_Mes_Anio));    //Da el formato correcto para el campo Mes_Anio
                Dt_Datos = Rs_Cierre_Mensual.Consulta_Cierre_Mensual_Auxiliar();
                Grid_Cuentas_Enero.DataSource = Dt_Datos;
                Grid_Cuentas_Enero.DataBind();
                Grid_Cuentas_Enero.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Grid_Cuentas_Febrero_PageIndexChanging
        /// DESCRIPCION : Cambia la pagina de la tabla
        ///               
        /// PARAMETROS  : 
        /// CREO        : Sergio Manuel Gallardo Andrade
        /// FECHA_CREO  : 11/noviembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Grid_Cuentas_Febrero_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Cls_Ope_Con_Cierre_Mensual_Negocio Rs_Cierre_Mensual = new Cls_Ope_Con_Cierre_Mensual_Negocio();    //Variable de conexion con la capa de Negocios.
            DataTable Dt_Datos = null;  //Almacenara los datos encontrados.
            try
            {
                Limpia_Controles();                        //Limpia todos los controles de la forma
                Grid_Cuentas_Febrero.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
                Rs_Cierre_Mensual.P_Mes_Anio = (Tbc_Meses_Contables.ActiveTabIndex + 1).ToString() + "" + Cmb_Anio.SelectedItem.ToString().Substring(2, 2); //Toma los valores del año y el mes para concatenarlos y almacenarlos.
                Rs_Cierre_Mensual.P_Mes_Anio = String.Format("{0:0000}", Convert.ToInt16(Rs_Cierre_Mensual.P_Mes_Anio));    //Da el formato correcto para el campo Mes_Anio
                Dt_Datos = Rs_Cierre_Mensual.Consulta_Cierre_Mensual_Auxiliar();
                Grid_Cuentas_Febrero.DataSource = Dt_Datos;
                Grid_Cuentas_Febrero.DataBind();
                Grid_Cuentas_Febrero.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Grid_Cuentas_Marzo_PageIndexChanging
        /// DESCRIPCION : Cambia la pagina de la tabla
        ///               
        /// PARAMETROS  : 
        /// CREO        : Sergio Manuel Gallardo Andrade
        /// FECHA_CREO  : 11/noviembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Grid_Cuentas_Marzo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Cls_Ope_Con_Cierre_Mensual_Negocio Rs_Cierre_Mensual = new Cls_Ope_Con_Cierre_Mensual_Negocio();    //Variable de conexion con la capa de Negocios.
            DataTable Dt_Datos = null;  //Almacenara los datos encontrados.
            try
            {
                Limpia_Controles();                        //Limpia todos los controles de la forma
                Grid_Cuentas_Marzo.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
                Rs_Cierre_Mensual.P_Mes_Anio = (Tbc_Meses_Contables.ActiveTabIndex + 1).ToString() + "" + Cmb_Anio.SelectedItem.ToString().Substring(2, 2); //Toma los valores del año y el mes para concatenarlos y almacenarlos.
                Rs_Cierre_Mensual.P_Mes_Anio = String.Format("{0:0000}", Convert.ToInt16(Rs_Cierre_Mensual.P_Mes_Anio));    //Da el formato correcto para el campo Mes_Anio
                Dt_Datos = Rs_Cierre_Mensual.Consulta_Cierre_Mensual_Auxiliar();
                Grid_Cuentas_Marzo.DataSource = Dt_Datos;
                Grid_Cuentas_Marzo.DataBind();
                Grid_Cuentas_Marzo.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Grid_Cuentas_Abril_PageIndexChanging
        /// DESCRIPCION : Cambia la pagina de la tabla
        ///               
        /// PARAMETROS  : 
        /// CREO        : Sergio Manuel Gallardo Andrade
        /// FECHA_CREO  : 11/noviembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Grid_Cuentas_Abril_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Cls_Ope_Con_Cierre_Mensual_Negocio Rs_Cierre_Mensual = new Cls_Ope_Con_Cierre_Mensual_Negocio();    //Variable de conexion con la capa de Negocios.
            DataTable Dt_Datos = null;  //Almacenara los datos encontrados.
            try
            {
                Limpia_Controles();                        //Limpia todos los controles de la forma
                Grid_Cuentas_Abril.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
                Rs_Cierre_Mensual.P_Mes_Anio = (Tbc_Meses_Contables.ActiveTabIndex + 1).ToString() + "" + Cmb_Anio.SelectedItem.ToString().Substring(2, 2); //Toma los valores del año y el mes para concatenarlos y almacenarlos.
                Rs_Cierre_Mensual.P_Mes_Anio = String.Format("{0:0000}", Convert.ToInt16(Rs_Cierre_Mensual.P_Mes_Anio));    //Da el formato correcto para el campo Mes_Anio
                Dt_Datos = Rs_Cierre_Mensual.Consulta_Cierre_Mensual_Auxiliar();
                Grid_Cuentas_Abril.DataSource = Dt_Datos;
                Grid_Cuentas_Abril.DataBind();
                Grid_Cuentas_Abril.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Grid_Cuentas_Mayo_PageIndexChanging
        /// DESCRIPCION : Cambia la pagina de la tabla
        ///               
        /// PARAMETROS  : 
        /// CREO        : Sergio Manuel Gallardo Andrade
        /// FECHA_CREO  : 11/noviembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Grid_Cuentas_Mayo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Cls_Ope_Con_Cierre_Mensual_Negocio Rs_Cierre_Mensual = new Cls_Ope_Con_Cierre_Mensual_Negocio();    //Variable de conexion con la capa de Negocios.
            DataTable Dt_Datos = null;  //Almacenara los datos encontrados.
            try
            {
                Limpia_Controles();                        //Limpia todos los controles de la forma
                Grid_Cuentas_Mayo.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
                Rs_Cierre_Mensual.P_Mes_Anio = (Tbc_Meses_Contables.ActiveTabIndex + 1).ToString() + "" + Cmb_Anio.SelectedItem.ToString().Substring(2, 2); //Toma los valores del año y el mes para concatenarlos y almacenarlos.
                Rs_Cierre_Mensual.P_Mes_Anio = String.Format("{0:0000}", Convert.ToInt16(Rs_Cierre_Mensual.P_Mes_Anio));    //Da el formato correcto para el campo Mes_Anio
                Dt_Datos = Rs_Cierre_Mensual.Consulta_Cierre_Mensual_Auxiliar();
                Grid_Cuentas_Mayo.DataSource = Dt_Datos;
                Grid_Cuentas_Mayo.DataBind();
                Grid_Cuentas_Mayo.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Grid_Cuentas_Junio_PageIndexChanging
        /// DESCRIPCION : Cambia la pagina de la tabla
        ///               
        /// PARAMETROS  : 
        /// CREO        : Sergio Manuel Gallardo Andrade
        /// FECHA_CREO  : 11/noviembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Grid_Cuentas_Junio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Cls_Ope_Con_Cierre_Mensual_Negocio Rs_Cierre_Mensual = new Cls_Ope_Con_Cierre_Mensual_Negocio();    //Variable de conexion con la capa de Negocios.
            DataTable Dt_Datos = null;  //Almacenara los datos encontrados.
            try
            {
                Limpia_Controles();                        //Limpia todos los controles de la forma
                Grid_Cuentas_Junio.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
                Rs_Cierre_Mensual.P_Mes_Anio = (Tbc_Meses_Contables.ActiveTabIndex + 1).ToString() + "" + Cmb_Anio.SelectedItem.ToString().Substring(2, 2); //Toma los valores del año y el mes para concatenarlos y almacenarlos.
                Rs_Cierre_Mensual.P_Mes_Anio = String.Format("{0:0000}", Convert.ToInt16(Rs_Cierre_Mensual.P_Mes_Anio));    //Da el formato correcto para el campo Mes_Anio
                Dt_Datos = Rs_Cierre_Mensual.Consulta_Cierre_Mensual_Auxiliar();
                Grid_Cuentas_Junio.DataSource = Dt_Datos;
                Grid_Cuentas_Junio.DataBind();
                Grid_Cuentas_Junio.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Grid_Cuentas_Julio_PageIndexChanging
        /// DESCRIPCION : Cambia la pagina de la tabla
        ///               
        /// PARAMETROS  : 
        /// CREO        : Sergio Manuel Gallardo Andrade
        /// FECHA_CREO  : 11/noviembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Grid_Cuentas_Julio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Cls_Ope_Con_Cierre_Mensual_Negocio Rs_Cierre_Mensual = new Cls_Ope_Con_Cierre_Mensual_Negocio();    //Variable de conexion con la capa de Negocios.
            DataTable Dt_Datos = null;  //Almacenara los datos encontrados.
            try
            {
                Limpia_Controles();                        //Limpia todos los controles de la forma
                Grid_Cuentas_Julio.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
                Rs_Cierre_Mensual.P_Mes_Anio = (Tbc_Meses_Contables.ActiveTabIndex + 1).ToString() + "" + Cmb_Anio.SelectedItem.ToString().Substring(2, 2); //Toma los valores del año y el mes para concatenarlos y almacenarlos.
                Rs_Cierre_Mensual.P_Mes_Anio = String.Format("{0:0000}", Convert.ToInt16(Rs_Cierre_Mensual.P_Mes_Anio));    //Da el formato correcto para el campo Mes_Anio
                Dt_Datos = Rs_Cierre_Mensual.Consulta_Cierre_Mensual_Auxiliar();
                Grid_Cuentas_Julio.DataSource = Dt_Datos;
                Grid_Cuentas_Julio.DataBind();
                Grid_Cuentas_Julio.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Grid_Cuentas_Agosto_PageIndexChanging
        /// DESCRIPCION : Cambia la pagina de la tabla
        ///               
        /// PARAMETROS  : 
        /// CREO        : Sergio Manuel Gallardo Andrade
        /// FECHA_CREO  : 11/noviembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Grid_Cuentas_Agosto_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Cls_Ope_Con_Cierre_Mensual_Negocio Rs_Cierre_Mensual = new Cls_Ope_Con_Cierre_Mensual_Negocio();    //Variable de conexion con la capa de Negocios.
            DataTable Dt_Datos = null;  //Almacenara los datos encontrados.
            try
            {
                Limpia_Controles();                        //Limpia todos los controles de la forma
                Grid_Cuentas_Agosto.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
                Rs_Cierre_Mensual.P_Mes_Anio = (Tbc_Meses_Contables.ActiveTabIndex + 1).ToString() + "" + Cmb_Anio.SelectedItem.ToString().Substring(2, 2); //Toma los valores del año y el mes para concatenarlos y almacenarlos.
                Rs_Cierre_Mensual.P_Mes_Anio = String.Format("{0:0000}", Convert.ToInt16(Rs_Cierre_Mensual.P_Mes_Anio));    //Da el formato correcto para el campo Mes_Anio
                Dt_Datos = Rs_Cierre_Mensual.Consulta_Cierre_Mensual_Auxiliar();
                Grid_Cuentas_Agosto.DataSource = Dt_Datos;
                Grid_Cuentas_Agosto.DataBind();
                Grid_Cuentas_Agosto.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Grid_Cuentas_Septiembre_PageIndexChanging
        /// DESCRIPCION : Cambia la pagina de la tabla
        ///               
        /// PARAMETROS  : 
        /// CREO        : Sergio Manuel Gallardo Andrade
        /// FECHA_CREO  : 11/noviembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Grid_Cuentas_Septiembre_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Cls_Ope_Con_Cierre_Mensual_Negocio Rs_Cierre_Mensual = new Cls_Ope_Con_Cierre_Mensual_Negocio();    //Variable de conexion con la capa de Negocios.
            DataTable Dt_Datos = null;  //Almacenara los datos encontrados.
            try
            {
                Limpia_Controles();                        //Limpia todos los controles de la forma
                Grid_Cuentas_Septiembre.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
                Rs_Cierre_Mensual.P_Mes_Anio = (Tbc_Meses_Contables.ActiveTabIndex + 1).ToString() + "" + Cmb_Anio.SelectedItem.ToString().Substring(2, 2); //Toma los valores del año y el mes para concatenarlos y almacenarlos.
                Rs_Cierre_Mensual.P_Mes_Anio = String.Format("{0:0000}", Convert.ToInt16(Rs_Cierre_Mensual.P_Mes_Anio));    //Da el formato correcto para el campo Mes_Anio
                Dt_Datos = Rs_Cierre_Mensual.Consulta_Cierre_Mensual_Auxiliar();
                Grid_Cuentas_Septiembre.DataSource = Dt_Datos;
                Grid_Cuentas_Septiembre.DataBind();
                Grid_Cuentas_Septiembre.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Grid_Cuentas_Octubre_PageIndexChanging
        /// DESCRIPCION : Cambia la pagina de la tabla
        ///               
        /// PARAMETROS  : 
        /// CREO        : Sergio Manuel Gallardo Andrade
        /// FECHA_CREO  : 11/noviembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Grid_Cuentas_Octubre_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Cls_Ope_Con_Cierre_Mensual_Negocio Rs_Cierre_Mensual = new Cls_Ope_Con_Cierre_Mensual_Negocio();    //Variable de conexion con la capa de Negocios.
            DataTable Dt_Datos = null;  //Almacenara los datos encontrados.
            try
            {
                Limpia_Controles();                        //Limpia todos los controles de la forma
                Grid_Cuentas_Octubre.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
                Rs_Cierre_Mensual.P_Mes_Anio = (Tbc_Meses_Contables.ActiveTabIndex + 1).ToString() + "" + Cmb_Anio.SelectedItem.ToString().Substring(2, 2); //Toma los valores del año y el mes para concatenarlos y almacenarlos.
                Rs_Cierre_Mensual.P_Mes_Anio = String.Format("{0:0000}", Convert.ToInt16(Rs_Cierre_Mensual.P_Mes_Anio));    //Da el formato correcto para el campo Mes_Anio
                Dt_Datos = Rs_Cierre_Mensual.Consulta_Cierre_Mensual_Auxiliar();
                Grid_Cuentas_Octubre.DataSource = Dt_Datos;
                Grid_Cuentas_Octubre.DataBind();
                Grid_Cuentas_Octubre.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Grid_Cuentas_Noviembre_PageIndexChanging
        /// DESCRIPCION : Cambia la pagina de la tabla
        ///               
        /// PARAMETROS  : 
        /// CREO        : Sergio Manuel Gallardo Andrade
        /// FECHA_CREO  : 11/noviembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Grid_Cuentas_Noviembre_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Cls_Ope_Con_Cierre_Mensual_Negocio Rs_Cierre_Mensual = new Cls_Ope_Con_Cierre_Mensual_Negocio();    //Variable de conexion con la capa de Negocios.
            DataTable Dt_Datos = null;  //Almacenara los datos encontrados.
            try
            {
                Limpia_Controles();                        //Limpia todos los controles de la forma
                Grid_Cuentas_Noviembre.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
                Rs_Cierre_Mensual.P_Mes_Anio = (Tbc_Meses_Contables.ActiveTabIndex + 1).ToString() + "" + Cmb_Anio.SelectedItem.ToString().Substring(2, 2); //Toma los valores del año y el mes para concatenarlos y almacenarlos.
                Rs_Cierre_Mensual.P_Mes_Anio = String.Format("{0:0000}", Convert.ToInt16(Rs_Cierre_Mensual.P_Mes_Anio));    //Da el formato correcto para el campo Mes_Anio
                Dt_Datos = Rs_Cierre_Mensual.Consulta_Cierre_Mensual_Auxiliar();
                Grid_Cuentas_Noviembre.DataSource = Dt_Datos;
                Grid_Cuentas_Noviembre.DataBind();
                Grid_Cuentas_Noviembre.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Grid_Cuentas_Diciembre_PageIndexChanging
        /// DESCRIPCION : Cambia la pagina de la tabla
        ///               
        /// PARAMETROS  : 
        /// CREO        : Sergio Manuel Gallardo Andrade
        /// FECHA_CREO  : 11/noviembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Grid_Cuentas_Diciembre_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Cls_Ope_Con_Cierre_Mensual_Negocio Rs_Cierre_Mensual = new Cls_Ope_Con_Cierre_Mensual_Negocio();    //Variable de conexion con la capa de Negocios.
            DataTable Dt_Datos = null;  //Almacenara los datos encontrados.
            try
            {
                Limpia_Controles();                        //Limpia todos los controles de la forma
                Grid_Cuentas_Diciembre.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
                Rs_Cierre_Mensual.P_Mes_Anio = (Tbc_Meses_Contables.ActiveTabIndex + 1).ToString() + "" + Cmb_Anio.SelectedItem.ToString().Substring(2, 2); //Toma los valores del año y el mes para concatenarlos y almacenarlos.
                Rs_Cierre_Mensual.P_Mes_Anio = String.Format("{0:0000}", Convert.ToInt16(Rs_Cierre_Mensual.P_Mes_Anio));    //Da el formato correcto para el campo Mes_Anio
                Dt_Datos = Rs_Cierre_Mensual.Consulta_Cierre_Mensual_Auxiliar();
                Grid_Cuentas_Diciembre.DataSource = Dt_Datos;
                Grid_Cuentas_Diciembre.DataBind();
                Grid_Cuentas_Diciembre.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Grid_Cuentas_Trece_PageIndexChanging
        /// DESCRIPCION : Cambia la pagina de la tabla
        ///               
        /// PARAMETROS  : 
        /// CREO        : Sergio Manuel Gallardo Andrade
        /// FECHA_CREO  : 11/noviembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Grid_Cuentas_Trece_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Cls_Ope_Con_Cierre_Mensual_Negocio Rs_Cierre_Mensual = new Cls_Ope_Con_Cierre_Mensual_Negocio();    //Variable de conexion con la capa de Negocios.
            DataTable Dt_Datos = null;  //Almacenara los datos encontrados.
            try
            {
                Limpia_Controles();                        //Limpia todos los controles de la forma
                Grid_Cuentas_Trece.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
                Rs_Cierre_Mensual.P_Mes_Anio = (Tbc_Meses_Contables.ActiveTabIndex + 1).ToString() + "" + Cmb_Anio.SelectedItem.ToString().Substring(2, 2); //Toma los valores del año y el mes para concatenarlos y almacenarlos.
                Rs_Cierre_Mensual.P_Mes_Anio = String.Format("{0:0000}", Convert.ToInt16(Rs_Cierre_Mensual.P_Mes_Anio));    //Da el formato correcto para el campo Mes_Anio
                Dt_Datos = Rs_Cierre_Mensual.Consulta_Cierre_Mensual_Auxiliar();
                Grid_Cuentas_Trece.DataSource = Dt_Datos;
                Grid_Cuentas_Trece.DataBind();
                Grid_Cuentas_Trece.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        #endregion

        protected void Txt_Buscar_Cuentas_TextChanged(object sender, EventArgs e)
        {
            Cls_Ope_Con_Cierre_Mensual_Negocio Rs_Cierre_Mensual = new Cls_Ope_Con_Cierre_Mensual_Negocio();    //Variable de conexion con la capa de Negocios.
            DataTable Dt_Datos = null;  //Almacenara los datos encontrados.
            try
            {
                if (Cmb_Anio.SelectedIndex > 0) //Verifica que se haya seleccionado un año.
                {
                    Rs_Cierre_Mensual.P_Mes_Anio = (Tbc_Meses_Contables.ActiveTabIndex + 1).ToString() + "" + Cmb_Anio.SelectedItem.ToString().Substring(2, 2); //Toma los valores del año y el mes para concatenarlos y almacenarlos.
                    Rs_Cierre_Mensual.P_Mes_Anio = String.Format("{0:0000}", Convert.ToInt16(Rs_Cierre_Mensual.P_Mes_Anio));    //Da el formato correcto para el campo Mes_Anio
                    Rs_Cierre_Mensual.P_Descripcion = Txt_Buscar_Cuentas.Text;  //Almacena el texto introducido en la caja de texto
                    Dt_Datos = Rs_Cierre_Mensual.Consulta_Cierre_Mensual_Auxiliar();    //Almacena los datos de la consulta
                    #region Llenar Grids
                    switch (Tbc_Meses_Contables.ActiveTabIndex)
                    {
                        case 0:
                            Grid_Cuentas_Enero.DataSource = Dt_Datos;
                            Grid_Cuentas_Enero.DataBind();
                            Grid_Cuentas_Enero.Visible = true;
                            break;
                        case 1:
                            Grid_Cuentas_Febrero.DataSource = Dt_Datos;
                            Grid_Cuentas_Febrero.DataBind();
                            Grid_Cuentas_Febrero.Visible = true;
                            break;
                        case 2:
                            Grid_Cuentas_Marzo.DataSource = Dt_Datos;
                            Grid_Cuentas_Marzo.DataBind();
                            Grid_Cuentas_Marzo.Visible = true;
                            break;
                        case 3:
                            Grid_Cuentas_Abril.DataSource = Dt_Datos;
                            Grid_Cuentas_Abril.DataBind();
                            Grid_Cuentas_Abril.Visible = true;
                            break;
                        case 4:
                            Grid_Cuentas_Mayo.DataSource = Dt_Datos;
                            Grid_Cuentas_Mayo.DataBind();
                            Grid_Cuentas_Mayo.Visible = true;
                            break;
                        case 5:
                            Grid_Cuentas_Junio.DataSource = Dt_Datos;
                            Grid_Cuentas_Junio.DataBind();
                            Grid_Cuentas_Junio.Visible = true;
                            break;
                        case 6:
                            Grid_Cuentas_Julio.DataSource = Dt_Datos;
                            Grid_Cuentas_Julio.DataBind();
                            Grid_Cuentas_Julio.Visible = true;
                            break;
                        case 7:
                            Grid_Cuentas_Agosto.DataSource = Dt_Datos;
                            Grid_Cuentas_Agosto.DataBind();
                            Grid_Cuentas_Agosto.Visible = true;
                            break;
                        case 8:
                            Grid_Cuentas_Septiembre.DataSource = Dt_Datos;
                            Grid_Cuentas_Septiembre.DataBind();
                            Grid_Cuentas_Septiembre.Visible = true;
                            break;
                        case 9:
                            Grid_Cuentas_Octubre.DataSource = Dt_Datos;
                            Grid_Cuentas_Octubre.DataBind();
                            Grid_Cuentas_Octubre.Visible = true;
                            break;
                        case 10:
                            Grid_Cuentas_Noviembre.DataSource = Dt_Datos;
                            Grid_Cuentas_Noviembre.DataBind();
                            Grid_Cuentas_Noviembre.Visible = true;
                            break;
                        case 11:
                            Grid_Cuentas_Diciembre.DataSource = Dt_Datos;
                            Grid_Cuentas_Diciembre.DataBind();
                            Grid_Cuentas_Diciembre.Visible = true;
                            break;
                        case 12:
                            Grid_Cuentas_Trece.DataSource = Dt_Datos;
                            Grid_Cuentas_Trece.DataBind();
                            Grid_Cuentas_Trece.Visible = true;
                            break;
                    }
                    #endregion
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Cmb_Anio_SelectedIndexChanged: " + Ex.Message);
            }
        }
    #endregion
}
