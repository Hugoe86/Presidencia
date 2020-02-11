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
using Presidencia.Catalogo_Estados.Negocio;

public partial class paginas_predial_Frm_Cat_Pre_Estados : System.Web.UI.Page{

        #region Page_Load

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 20/Julio/2011 
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
                    Configuracion_Acceso("Frm_Cat_Pre_Estados.aspx");
                    Configuracion_Formulario(true);
                    Llenar_Tabla_Estados(0);
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
        ///FECHA_CREO: 20/Julio/2011 
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
             Cmb_Estatus.Enabled = !Estatus;
             Grid_Estados.Enabled = Estatus;
             Grid_Estados.SelectedIndex = (-1);
             Btn_Buscar_Estados.Enabled = Estatus;
             Txt_Busqueda_Estados.Enabled = Estatus; 
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
        ///DESCRIPCIÓN: Limpia los controles del Formulario
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 20/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Catalogo()
        {
            Txt_Estado_ID.Text = "";
            Txt_Clave.Text = "";
            Cmb_Estatus.SelectedIndex = 0;
            Txt_Nombre.Text = "";
            Grid_Estados.DataSource = new DataTable();
            Grid_Estados.DataBind();
        }

        #region Validaciones

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Generales
        ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
        ///             una operación que se vea afectada en la basae de datos.
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 20/Julio/2011 
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
                Mensaje_Error = Mensaje_Error + "+ Introducir el la Clave del Estado.";
                Validacion = false;
            }
            if (Cmb_Estatus.SelectedIndex == 0)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Estatus.";
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
        ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Estados
        ///DESCRIPCIÓN: Llena la tabla de Estadoss
        ///PROPIEDADES:     
        ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 20/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Tabla_Estados(int Pagina)
        {
            try
            {
                Cls_Cat_Pre_Estados_Negocio Estado= new Cls_Cat_Pre_Estados_Negocio();
                Grid_Estados.DataSource = Estado.Consultar_Estados();
                Grid_Estados.PageIndex = Pagina;
                Grid_Estados.Columns[1].Visible = true;
                Grid_Estados.DataBind();
                Grid_Estados.Columns[1].Visible = false;
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Estados_Busqueda
        ///DESCRIPCIÓN: Llena la tabla de Estados de acuerdo a la busqueda introducida.
        ///PROPIEDADES:     
        ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 20/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Tabla_Estados_Busqueda(int Pagina)
        {
            try
            {
                Cls_Cat_Pre_Estados_Negocio Estados = new Cls_Cat_Pre_Estados_Negocio();
                Estados.P_Estado_ID = Txt_Estado_ID.Text.Trim();
                Estados.P_Nombre = Txt_Busqueda_Estados.Text.ToUpper().Trim();
                Grid_Estados.DataSource = Estados.Consultar_Nombre();
                Grid_Estados.PageIndex = Pagina;
                Grid_Estados.Columns[1].Visible = true;
                Grid_Estados.DataBind();
                Grid_Estados.Columns[1].Visible = false;
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
            Div_Contenedor_Msj_Error.Visible = false;
        }

        #endregion

        #endregion

        #region Grids

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Estados_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView General de Estados
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 20/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Estados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Limpiar_Catalogo();
                Grid_Estados.SelectedIndex = (-1);
                Llenar_Tabla_Estados(e.NewPageIndex);
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Estados_SelectedIndexChanged
        ///DESCRIPCIÓN: Obtiene los datos del Estado Seleccionada para mostrarlos a detalle
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 20/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Estados_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Grid_Estados.SelectedIndex > (-1))
                {
                    Txt_Estado_ID.Text = Grid_Estados.SelectedRow.Cells[1].Text;
                    Txt_Clave.Text = Grid_Estados.SelectedRow.Cells[2].Text;
                    Txt_Nombre.Text = Grid_Estados.SelectedRow.Cells[3].Text;
                    Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByText(Grid_Estados.SelectedRow.Cells[4].Text));
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
        ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta un nuevo Estado
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 20/Julio/2011
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
                    Cls_Cat_Pre_Estados_Negocio Estados = new Cls_Cat_Pre_Estados_Negocio();
                    Estados.P_Estado_ID = Txt_Estado_ID.Text.Trim();
                    Txt_Clave.Text = Estados.Ultima_Clave();
                }
                else
                {
                    if (Validar_Componentes_Generales())
                    {
                        
                        Cls_Cat_Pre_Estados_Negocio Estados = new Cls_Cat_Pre_Estados_Negocio();
                        Estados.P_Estado_ID = Txt_Estado_ID.Text.Trim();
                        Estados.P_Clave = Txt_Clave.Text.Trim();
                        Estados.P_Nombre = Txt_Nombre.Text.ToUpper().Trim();
                        Estados.P_Estatus = Cmb_Estatus.SelectedItem.Text;
                        Limpiar_Catalogo();
                        Estados.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                        Grid_Estados.Columns[1].Visible = true;
                        Estados.Alta_Estado();
                        Grid_Estados.Columns[1].Visible = false;
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Tabla_Estados(Grid_Estados.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Estados", "alert('Alta de Estado Exitosa');", true);
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Grid_Estados.Enabled = true;
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
        ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de un Estado
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 20/Julio/2011 
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
                    if (Grid_Estados.Rows.Count > 0 && Grid_Estados.SelectedIndex > (-1))
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
                        Cls_Cat_Pre_Estados_Negocio Estados= new Cls_Cat_Pre_Estados_Negocio();
                        Estados.P_Estado_ID = Txt_Estado_ID.Text.Trim();
                        Estados.P_Nombre = Txt_Nombre.Text.ToUpper().Trim();
                        Estados.P_Estatus = Cmb_Estatus.SelectedValue;
                        Estados.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                        Grid_Estados.Columns[1].Visible = true;
                        Estados.Modificar_Estado();
                        Grid_Estados.Columns[1].Visible = false;
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Tabla_Estados(Grid_Estados.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Estados", "alert('Actualización de Estado Exitosa');", true);
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Grid_Estados.Enabled = true;
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
        ///FECHA_CREO: 20/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Limpiar_Catalogo();
                Llenar_Tabla_Estados_Busqueda(0);
                if (Grid_Estados.Rows.Count == 0 && Txt_Busqueda_Estados.Text.Trim().Length > 0)
                {
                    Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el Concepto\"" + Txt_Busqueda_Estados.Text + "\" no se encotrarón coincidencias";
                    Lbl_Mensaje_Error.Text = "(Se cargaron  todos los Estados almacenadas)";
                    Div_Contenedor_Msj_Error.Visible = true;
                    Txt_Busqueda_Estados.Text = "";
                    Llenar_Tabla_Estados_Busqueda(0);
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
        ///DESCRIPCIÓN: Elimina un Estado de la Base de Datos
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 20/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Eliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Grid_Estados.Rows.Count > 0 && Grid_Estados.SelectedIndex > (-1))
                {
                    Cls_Cat_Pre_Estados_Negocio Estados = new Cls_Cat_Pre_Estados_Negocio();
                    Estados.P_Estado_ID = Grid_Estados.SelectedRow.Cells[1].Text;
                    Estados.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                    Grid_Estados.Columns[1].Visible = true;
                    Estados.Eliminar_Estado();
                    Grid_Estados.Columns[1].Visible = false;
                    Grid_Estados.SelectedIndex = 0;
                    Llenar_Tabla_Estados(Grid_Estados.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Estados", "alert('El Estado fue eliminado exitosamente');", true);
                    Limpiar_Catalogo();
                    Llenar_Tabla_Estados(0);
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
        ///FECHA_CREO: 20/Julio/2011 
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
                    Llenar_Tabla_Estados(0);
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Grid_Estados.Enabled = true;
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
                Botones.Add(Btn_Buscar_Estados);

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