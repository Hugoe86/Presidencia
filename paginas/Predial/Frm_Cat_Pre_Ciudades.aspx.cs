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
using Presidencia.Catalogo_Ciudades.Negocio;

public partial class paginas_predial_Frm_Cat_Pre_Ciudades : System.Web.UI.Page{

        #region Page_Load

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011 
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
                    Configuracion_Acceso("Frm_Cat_Pre_Ciudades.aspx");
                    Configuracion_Formulario(true);
                    Llenar_Tabla_Ciudades(0);
                    Llenar_Combo_Estados();
                   
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

        ///****************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
        ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
        ///PROPIEDADES:     
        ///             1. Estatus.    Estatus en el que se cargara la configuración de los controles.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Configuracion_Formulario(Boolean Estatus)
        {
             Btn_Nuevo.Visible = true;
             Btn_Nuevo.AlternateText = "Nuevo";
             Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
             Btn_Modificar.Visible = true;
             Btn_Modificar.AlternateText = "Modificar";
             Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
             Btn_Eliminar.Visible = Estatus;
             Txt_Clave.Enabled = !Estatus;
             Txt_Nombre.Enabled = !Estatus;
             Cmb_Estados.Enabled = !Estatus;
             Cmb_Estatus.Enabled = !Estatus;
             Grid_Ciudades.Enabled = Estatus;
             Grid_Ciudades.SelectedIndex = (-1);
             Btn_Buscar_Ciudades.Enabled = Estatus;
             Txt_Busqueda_Ciudades.Enabled = Estatus; 
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
        ///DESCRIPCIÓN: Limpia los controles del Formulario
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Catalogo()
        {
            Txt_Ciudad_ID.Text = "";
            Txt_Clave.Text = "";
            Cmb_Estados.SelectedIndex = 0;
            Cmb_Estatus.SelectedIndex = 0;
            Txt_Nombre.Text = "";
            Grid_Ciudades.DataSource = new DataTable();
            Grid_Ciudades.DataBind();
        }

        #region Validaciones

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Generales
        ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
        ///             una operación que se vea afectada en la basae de datos.
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private bool Validar_Componentes_Generales()
        {
            Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
            String Mensaje_Error = "";
            Boolean Validacion = true;
            if (Txt_Clave.Text.Trim().Length == 0)
            {
                Mensaje_Error = Mensaje_Error + "+ Introducir el la Clave de la Ciudad.";
                Validacion = false;
            }
            if (Cmb_Estatus.SelectedIndex == 0)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Estatus.";
                Validacion = false;
            }
            if (Cmb_Estados.SelectedIndex == 0)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Estados.";
                Validacion = false;
            }
            if (Txt_Nombre.Text.Trim().Length == 0)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir el Nombre.";
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
        ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Ciudades
        ///DESCRIPCIÓN: Llena la tabla de Ciudades
        ///PROPIEDADES:     
        ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Tabla_Ciudades(int Pagina)
        {
            try
            {
                Cls_Cat_Pre_Ciudades_Negocio Ciudad = new Cls_Cat_Pre_Ciudades_Negocio();
                Grid_Ciudades.DataSource = Ciudad.Consultar_Ciudades();
                Grid_Ciudades.PageIndex = Pagina;
                Grid_Ciudades.Columns[1].Visible = true;
                Grid_Ciudades.Columns[2].Visible = true;
                Grid_Ciudades.DataBind();
                Grid_Ciudades.Columns[1].Visible = false;
                Grid_Ciudades.Columns[2].Visible = false;
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Ciudades_Busqueda
        ///DESCRIPCIÓN: Llena la tabla de Ciudades de acuerdo a la busqueda introducida.
        ///PROPIEDADES:     
        ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Tabla_Ciudades_Busqueda(int Pagina)
        {
            try
            {
                Cls_Cat_Pre_Ciudades_Negocio Ciudades = new Cls_Cat_Pre_Ciudades_Negocio();
                Ciudades.P_Ciudad_ID = Txt_Ciudad_ID.Text.Trim();
                Ciudades.P_Nombre = Txt_Busqueda_Ciudades.Text.ToUpper().Trim();
                Grid_Ciudades.DataSource = Ciudades.Consultar_Nombre();
                Grid_Ciudades.PageIndex = Pagina;
                Grid_Ciudades.Columns[2].Visible = true;
                Grid_Ciudades.Columns[1].Visible = true;
                Grid_Ciudades.DataBind();
                Grid_Ciudades.Columns[1].Visible = false;
                Grid_Ciudades.Columns[2].Visible = false;
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Estados
        ///DESCRIPCIÓN: Metodo que llena el Combo de Estados con los estados existentes.
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Combo_Estados()
        {
            try
            {
                Cls_Cat_Pre_Ciudades_Negocio Ciudad = new Cls_Cat_Pre_Ciudades_Negocio();
                DataTable Ciudades = Ciudad.Llenar_Combo_Estados();
                DataRow fila = Ciudades.NewRow();
                fila[Cat_Pre_Estados.Campo_Nombre] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
                fila[Cat_Pre_Estados.Campo_Estado_ID] = "SELECCIONE";
                Ciudades.Rows.InsertAt(fila, 0);
                Cmb_Estados.DataTextField = Cat_Pre_Estados.Campo_Nombre;
                Cmb_Estados.DataValueField = Cat_Pre_Estados.Campo_Estado_ID;
                Cmb_Estados.DataSource = Ciudades;
                Cmb_Estados.DataBind();
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
        ///NOMBRE DE LA FUNCIÓN: Grid_Ciudades_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView General de Ciudades
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Ciudades_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Limpiar_Catalogo();
                Grid_Ciudades.SelectedIndex = (-1);
                Llenar_Tabla_Ciudades(e.NewPageIndex);
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Ciudades_SelectedIndexChanged
        ///DESCRIPCIÓN: Obtiene los datos de la Ciudad Seleccionada para mostrarlos a detalle
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Ciudades_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Grid_Ciudades.SelectedIndex > (-1))
                {
                    Cmb_Estados.SelectedIndex = Cmb_Estados.Items.IndexOf(Cmb_Estados.Items.FindByText(Grid_Ciudades.SelectedRow.Cells[6].Text));
                    Cmb_Estados.SelectedIndex = Cmb_Estados.Items.IndexOf(Cmb_Estados.Items.FindByValue(Grid_Ciudades.SelectedRow.Cells[2].Text));
                    Txt_Ciudad_ID.Text = Grid_Ciudades.SelectedRow.Cells[1].Text;
                    Txt_Clave.Text = Grid_Ciudades.SelectedRow.Cells[3].Text;
                    Txt_Nombre.Text = Grid_Ciudades.SelectedRow.Cells[4].Text;
                    Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByText(Grid_Ciudades.SelectedRow.Cells[5].Text));
                    System.Threading.Thread.Sleep(1000);
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

        #region Eventos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
        ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta una nueva Ciudad
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011
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
                    Txt_Clave.Enabled = false;
                    Cls_Cat_Pre_Ciudades_Negocio Ciudades = new Cls_Cat_Pre_Ciudades_Negocio();
                    Ciudades.P_Ciudad_ID = Txt_Ciudad_ID.Text.Trim();
                    Txt_Clave.Text = Ciudades.Ultima_Clave();

                }
                else
                {
                    if (Validar_Componentes_Generales())
                    {

                        Cls_Cat_Pre_Ciudades_Negocio Ciudades = new Cls_Cat_Pre_Ciudades_Negocio();
                        Ciudades.P_Estado_ID = Cmb_Estados.SelectedItem.Value;
                        Ciudades.P_Clave = Txt_Clave.Text.Trim().ToUpper();
                        Ciudades.P_Nombre = Txt_Nombre.Text.ToUpper().Trim();
                        Ciudades.P_Estatus = Cmb_Estatus.SelectedItem.Text.ToUpper().Trim();
                        Limpiar_Catalogo();
                        Ciudades.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Grid_Ciudades.Columns[1].Visible = true;
                        Grid_Ciudades.Columns[2].Visible = true;
                        Ciudades.Alta_Ciudad();
                        Grid_Ciudades.Columns[1].Visible = false;
                        Grid_Ciudades.Columns[2].Visible = false;
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Tabla_Ciudades(Grid_Ciudades.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Ciudades", "alert('Alta de Ciudad Exitosa');", true);
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Grid_Ciudades.Enabled = true;
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
        ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de una Ciudad
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011 
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
                    if (Grid_Ciudades.Rows.Count > 0 && Grid_Ciudades.SelectedIndex > (-1))
                    {
                        Configuracion_Formulario(false);
                        Btn_Modificar.AlternateText = "Actualizar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                        Btn_Salir.AlternateText = "Cancelar";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        Btn_Nuevo.Visible = false;
                        Txt_Clave.Enabled = false;
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
                        Cls_Cat_Pre_Ciudades_Negocio Ciudades = new Cls_Cat_Pre_Ciudades_Negocio();
                        Ciudades.P_Ciudad_ID = Txt_Ciudad_ID.Text.Trim();
                        Ciudades.P_Estado_ID = Cmb_Estados.SelectedItem.Value;
                        Ciudades.P_Nombre = Txt_Nombre.Text.ToUpper().Trim();
                        Ciudades.P_Estatus = Cmb_Estatus.SelectedItem.Text.ToUpper().Trim();
                        Ciudades.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Grid_Ciudades.Columns[1].Visible = true;
                        Grid_Ciudades.Columns[2].Visible = true;
                        Ciudades.Modificar_Ciudad();
                        Grid_Ciudades.Columns[1].Visible = false;
                        Grid_Ciudades.Columns[2].Visible = false;
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Tabla_Ciudades(Grid_Ciudades.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Ciudades", "alert('Actualización de Ciudad Exitosa');", true);
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Grid_Ciudades.Enabled = true;
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
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
        ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Limpiar_Catalogo();
                Llenar_Tabla_Ciudades_Busqueda(0);
                if (Grid_Ciudades.Rows.Count == 0 && Txt_Busqueda_Ciudades.Text.Trim().Length > 0)
                {
                    Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el Concepto\"" + Txt_Busqueda_Ciudades.Text + "\" no se encotrarón coincidencias";
                    Lbl_Mensaje_Error.Text = "(Se cargaron  todos las Ciudades almacenadas)";
                    Div_Contenedor_Msj_Error.Visible = true;
                    Txt_Busqueda_Ciudades.Text = "";
                    Llenar_Tabla_Ciudades_Busqueda(0);
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
        ///DESCRIPCIÓN: Elimina una Ciudad de la Base de Datos
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Eliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Grid_Ciudades.Rows.Count > 0 && Grid_Ciudades.SelectedIndex > (-1))
                {
                    Cls_Cat_Pre_Ciudades_Negocio Ciudades = new Cls_Cat_Pre_Ciudades_Negocio();
                    Ciudades.P_Ciudad_ID = Grid_Ciudades.SelectedRow.Cells[1].Text;
                    Ciudades.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                    Grid_Ciudades.Columns[1].Visible = true;
                    Grid_Ciudades.Columns[2].Visible = true;
                    Ciudades.Eliminar_Ciudad();
                    Grid_Ciudades.Columns[1].Visible = false;
                    Grid_Ciudades.Columns[2].Visible = false;
                    Grid_Ciudades.SelectedIndex = 0;
                    Llenar_Tabla_Ciudades(Grid_Ciudades.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Ciudades", "alert('La Ciudad fue eliminado exitosamente');", true);
                    Limpiar_Catalogo();
                    Llenar_Tabla_Ciudades(0);
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
        ///FECHA_CREO: 21/Julio/2011 
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
                    Llenar_Tabla_Ciudades(0);
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Grid_Ciudades.Enabled = true;
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
                Botones.Add(Btn_Buscar_Ciudades);

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