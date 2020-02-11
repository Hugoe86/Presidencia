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
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Otros_Pagos.Negocio;
using Presidencia.Catalogo_Otros_Pagos.Negocio;

public partial class paginas_predial_Frm_Cat_Pre_Otros_Pagos : System.Web.UI.Page{

    #region Page_Load

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 14/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
                if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

                if (!IsPostBack)
                {
                    Configuracion_Acceso("Frm_Cat_Pre_Otros_Pagos.aspx");
                    Configuracion_Formulario(true);
                    Llenar_Tabla_Otros_Pagos(0);
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

    #region Metodos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
        ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
        ///PROPIEDADES:     
        ///             1. Estatus.    Estatus en el que se cargara la configuración de los controles.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 14/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Configuracion_Formulario( Boolean Estatus )
        {
            Btn_Nuevo.Visible = true;
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            Btn_Modificar.Visible = true;
            Btn_Modificar.AlternateText = "Modificar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            Btn_Eliminar.Visible = Estatus;
            Txt_Concepto.Enabled = !Estatus;
            Cmb_Estatus.Enabled = !Estatus;
            Txt_Descripcion.Enabled = !Estatus;
            Grid_Otros_Pagos.Enabled = Estatus;
            Grid_Otros_Pagos.SelectedIndex = (-1);
            Btn_Buscar_Otros_Pagos.Enabled = Estatus;
            Txt_Busqueda_Otros_Pagos.Enabled = Estatus;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
        ///DESCRIPCIÓN: Limpia los controles del Formulario
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 14/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Catalogo()
        {
            Txt_Concepto.Text = "";
            Cmb_Estatus.SelectedIndex = 0;
            Txt_Descripcion.Text = "";
            Txt_Otros_Pagos_ID.Text = "";
            Grid_Otros_Pagos.DataSource = new DataTable();
            Grid_Otros_Pagos.DataBind();
        }

        #region Validaciones

        ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Generales
            ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
            ///             una operación de la pestaña de Otros Pagos.
            ///PROPIEDADES:     
            ///CREO: José Alfredo García Pichardo.
            ///FECHA_CREO: 14/Julio/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
        private bool Validar_Componentes_Generales()
            {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                String Mensaje_Error = "";
                Boolean Validacion = true;
                if (Txt_Concepto.Text.Trim().Length == 0)
                {
                    Mensaje_Error = Mensaje_Error + "+ Introducir el Concepto de Pago.";
                    Validacion = false;
                }
                if (Cmb_Estatus.SelectedIndex == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Estatus.";
                    Validacion = false;
                }
                if (Txt_Descripcion.Text.Trim().Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Descripci&oacute;n.";
                    Validacion = false;
                }
                if (!Validacion)
                {
                    Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
                    Div_Contenedor_Msj_Error.Visible = true;
                }
                return Validacion;
            }
            
        #endregion

        #region Grid
    
        ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Otros_Pagos
            ///DESCRIPCIÓN: Llena la tabla de Otros Pagos
            ///PROPIEDADES:     
            ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
            ///CREO: José Alfredo García Pichardo.
            ///FECHA_CREO: 14/Julio/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
        private void Llenar_Tabla_Otros_Pagos(int Pagina) 
            {
                try
                {
                    Cls_Cat_Pre_Otros_Pagos_Negocio Otros_Pagos = new Cls_Cat_Pre_Otros_Pagos_Negocio();
                    Grid_Otros_Pagos.DataSource = Otros_Pagos.Consultar_Otros_Pagos();
                    Grid_Otros_Pagos.PageIndex = Pagina;
                    Grid_Otros_Pagos.Columns[1].Visible = true;
                    Grid_Otros_Pagos.DataBind();
                    Grid_Otros_Pagos.Columns[1].Visible = false;
                }
                catch(Exception Ex)
                {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;                
                }
            }

        ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Otros_Pagos_Busqueda
            ///DESCRIPCIÓN: Llena la tabla de Otros Pagos de auerdo a la busqueda introducida.
            ///PROPIEDADES:     
            ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
            ///CREO: José Alfredo García Pichardo.
            ///FECHA_CREO: 15/Julio/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
        private void Llenar_Tabla_Otros_Pagos_Busqueda(int Pagina)
            {
                try
                {
                    Cls_Cat_Pre_Otros_Pagos_Negocio Otros_Pagos = new Cls_Cat_Pre_Otros_Pagos_Negocio();
                    Otros_Pagos.P_Pago_ID = Txt_Concepto.Text.Trim();
                    Otros_Pagos.P_Concepto = Txt_Busqueda_Otros_Pagos.Text.ToUpper().Trim();
                    Grid_Otros_Pagos.DataSource = Otros_Pagos.Consultar_Otro_Pago();
                    Grid_Otros_Pagos.PageIndex = Pagina;
                    Grid_Otros_Pagos.Columns[1].Visible = true;
                    Grid_Otros_Pagos.DataBind();
                    Grid_Otros_Pagos.Columns[1].Visible = false;
                }
                catch (Exception Ex)
                {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

        #endregion
    
    #endregion

    #region Grids
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Otros_Pagos_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView General de Otros Pagos
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 14/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Otros_Pagos_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                Grid_Otros_Pagos.SelectedIndex = (-1);
                Llenar_Tabla_Otros_Pagos(e.NewPageIndex);
                Limpiar_Catalogo();
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Otros_Pagos_Generales_SelectedIndexChanged
        ///DESCRIPCIÓN: Obtiene los datos del Otro Pago Seleccionado para mostrarlos a detalle
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 14/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Otros_Pagos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Grid_Otros_Pagos.SelectedIndex > (-1))
                {
                    Txt_Otros_Pagos_ID.Text = Grid_Otros_Pagos.SelectedRow.Cells[1].Text;
                    Txt_Concepto.Text = Grid_Otros_Pagos.SelectedRow.Cells[2].Text;
                    Txt_Descripcion.Text = Grid_Otros_Pagos.SelectedRow.Cells[4].Text;
                    Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Grid_Otros_Pagos.SelectedRow.Cells[3].Text));
                    System.Threading.Thread.Sleep(1000);
                }
            }
            catch(Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }    

    #endregion

    #region Eventos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
        ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta un nuevo concepto de pago
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 15/Julio/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Nuevo_Click(object sender, EventArgs e)
        {
            try
            {
                if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
                {
                    Configuracion_Formulario(false);
                    Limpiar_Catalogo();
                    Btn_Nuevo.AlternateText = "Dar de Alta";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.Visible = false;
                }
                else
                {
                    if (Validar_Componentes_Generales())
                    {
                        Cls_Cat_Pre_Otros_Pagos_Negocio Otro_Pago = new Cls_Cat_Pre_Otros_Pagos_Negocio();
                        Otro_Pago.P_Pago_ID = Txt_Otros_Pagos_ID.Text.Trim();
                        Otro_Pago.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        Otro_Pago.P_Descripcion = Txt_Descripcion.Text.ToUpper();
                        Otro_Pago.P_Concepto = Txt_Concepto.Text.ToUpper();
                        Otro_Pago.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Grid_Otros_Pagos.Columns[1].Visible = true;
                        Otro_Pago.Alta_Otro_Pago();
                        Grid_Otros_Pagos.Columns[1].Visible = false;
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Tabla_Otros_Pagos(Grid_Otros_Pagos.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Otros Pagos", "alert('Alta de Otro Pago Exitosa');", true);
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Grid_Otros_Pagos.Enabled = true;
                    }
                }
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
        ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de un concepto de pago
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 15/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Modificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Btn_Modificar.AlternateText.Equals("Modificar"))
                {
                    if (Grid_Otros_Pagos.Rows.Count > 0 && Grid_Otros_Pagos.SelectedIndex > (-1))
                    {
                        Configuracion_Formulario(false);
                        Btn_Modificar.AlternateText = "Actualizar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                        Btn_Salir.AlternateText = "Cancelar";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        Btn_Nuevo.Visible = false;
                    }
                    else
                    {
                        Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Modificar.";
                        Lbl_Mensaje_Error.Text = "";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                }
                else
                {
                    if (Validar_Componentes_Generales())
                    {
                        Cls_Cat_Pre_Otros_Pagos_Negocio Otro_Pago = new Cls_Cat_Pre_Otros_Pagos_Negocio();
                        Otro_Pago.P_Pago_ID = Txt_Otros_Pagos_ID.Text.Trim();
                        Otro_Pago.P_Concepto = Txt_Concepto.Text.ToUpper();
                        Otro_Pago.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        Otro_Pago.P_Descripcion = Txt_Descripcion.Text.ToUpper();
                        Otro_Pago.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Grid_Otros_Pagos.Columns[1].Visible = true;
                        Otro_Pago.Modificar_Otro_Pago();
                        Grid_Otros_Pagos.Columns[1].Visible = false;
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Tabla_Otros_Pagos(Grid_Otros_Pagos.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Otros Pagos", "alert('Actualización de Otro Pago Exitosa');", true);
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Grid_Otros_Pagos.Enabled = true;
                    }

                }
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Otros_Pagos_Click
        ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 15/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Otros_Pagos_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Limpiar_Catalogo();
                Llenar_Tabla_Otros_Pagos_Busqueda(0);
                if (Grid_Otros_Pagos.Rows.Count == 0 && Txt_Busqueda_Otros_Pagos.Text.Trim().Length > 0)
                {
                    Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el Concepto\"" + Txt_Busqueda_Otros_Pagos.Text + "\" no se encotrarón coincidencias";
                    Lbl_Mensaje_Error.Text = "(Se cargaron  todos los Conceptos de Pago almacenados)";
                    Div_Contenedor_Msj_Error.Visible = true;
                    Txt_Busqueda_Otros_Pagos.Text = "";
                    Llenar_Tabla_Otros_Pagos(0);
                }
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
        ///DESCRIPCIÓN: Elimina otro pago de la Base de Datos
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 15/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Eliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Grid_Otros_Pagos.Rows.Count > 0 && Grid_Otros_Pagos.SelectedIndex > (-1))
                {
                    Cls_Cat_Pre_Otros_Pagos_Negocio Otro_Pago = new Cls_Cat_Pre_Otros_Pagos_Negocio();
                    Otro_Pago.P_Pago_ID = Grid_Otros_Pagos.SelectedRow.Cells[1].Text;
                    Grid_Otros_Pagos.Columns[1].Visible = true;
                    Otro_Pago.Eliminar_Otro_Pago();
                    Grid_Otros_Pagos.Columns[1].Visible = false;
                    Grid_Otros_Pagos.SelectedIndex = 0;
                    Llenar_Tabla_Otros_Pagos(Grid_Otros_Pagos.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Otros Pagos", "alert('El pago por otro concepto fue eliminado exitosamente');", true);
                    Limpiar_Catalogo();
                    Llenar_Tabla_Otros_Pagos(0);
                }
                else
                {
                    Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Eliminar.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
        ///DESCRIPCIÓN: Cancela la operación que esta en proceso (Alta o Actualizar) o Sale del Formulario.
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 15/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Salir_Click(object sender, EventArgs e)
        {
            try
            {
                if (Btn_Salir.AlternateText.Equals("Salir"))
                {
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }
                else
                {
                    Configuracion_Formulario(true);
                    Limpiar_Catalogo();
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Grid_Otros_Pagos.Enabled = true;
                }
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
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
                Botones.Add(Btn_Buscar_Otros_Pagos);

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