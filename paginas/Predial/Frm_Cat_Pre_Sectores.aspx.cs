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
using Presidencia.Catalogo_Sectores.Negocio;
using Presidencia.Constantes;

public partial class paginas_predial_Frm_Cat_Pre_Sectores : System.Web.UI.Page
{
    
    #region Page_Load

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 29/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///******************************************************************************* 
        protected void Page_Load(object sender, EventArgs e){
            try
            {
                Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
                if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

                if (!IsPostBack)
                {
                    Configuracion_Acceso("Frm_Cat_Pre_Sectores.aspx");
                    Configuracion_Formulario(true);
                    Llenar_Grid_Sectores(0);
                    Llenar_Combo_Colonias();
                    //Tab_Sectores_Colonias.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
            Div_Contenedor_Msj_Error.Visible = false;
        }

    #endregion
    
    #region Metodos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
        ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
        ///PROPIEDADES:     
        ///             1. Estatus.    Estatus en el que se cargara la configuración de los
        ///                             controles.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 29/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Configuracion_Formulario( Boolean Estatus ) {
            Btn_Nuevo.Visible = true;
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            Btn_Modificar.Visible = true;
            Btn_Modificar.AlternateText = "Modificar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            Btn_Eliminar.Visible = Estatus;
            Txt_Nombre.Enabled = !Estatus;
            Txt_Comentarios.Enabled = !Estatus;
            Txt_Clave_Sector.Enabled = !Estatus;
            Grid_Sectores.Enabled = Estatus;
            Grid_Sectores.SelectedIndex = (-1);
            Btn_Buscar_Sector.Enabled = Estatus;
            Txt_Busqueda_Sector.Enabled = Estatus;
            Cmb_Colonia.Enabled = !Estatus;
            Tab_Catalogo_Sectores.Enabled = !Estatus; 
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
        ///DESCRIPCIÓN: Limpia los controles del Formulario
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 29/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Catalogo() {
            Txt_ID_Sector.Text = "";
            Txt_Nombre.Text = "";
            Txt_Comentarios.Text = "";
            Txt_Clave_Sector.Text = "";
            Txt_Clave_Colonia.Text = "";
            Cmb_Colonia.SelectedIndex = 0;
            Tab_Catalogo_Sectores.ActiveTabIndex = 0;
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Sectores
        ///DESCRIPCIÓN: Llena la tabla de Sectores con una consulta que puede o no
        ///             tener Filtros.
        ///PROPIEDADES:     
        ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 29/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Grid_Sectores(int Pagina) {
            try
            {
                Grid_Sectores.Columns[1].Visible = true;
                Grid_Sectores.Columns[4].Visible = true;
                Cls_Cat_Pre_Sectores_Negocio Sector = new Cls_Cat_Pre_Sectores_Negocio();
                Sector.P_Tipo_DataTable = "SECTORES";
                Sector.P_Nombre = Txt_Busqueda_Sector.Text.Trim().ToUpper();
                Grid_Sectores.DataSource = Sector.Consultar_DataTable();
                Grid_Sectores.PageIndex = Pagina;
                Grid_Sectores.DataBind();
                Grid_Sectores.Columns[1].Visible = false;
                Grid_Sectores.Columns[4].Visible = false;
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Colonias
        ///DESCRIPCIÓN: Llena la tabla de Colonias.
        ///PROPIEDADES:     
        ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
        ///CREO: JOsé Alfredo García Pichardo.
        ///FECHA_CREO: 06/Agosto/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Grid_Colonias(int Pagina)
        {
            try
            {
                Grid_Colonias_Sectores.Columns[0].Visible = true;
                Grid_Colonias_Sectores.Columns[1].Visible = true;
                Cls_Cat_Pre_Sectores_Negocio Colonia = new Cls_Cat_Pre_Sectores_Negocio();
                Colonia.P_Sector_ID = Txt_ID_Sector.Text;
                DataTable Dt_Colonias= Colonia.Consultar_Colonias();
                Grid_Colonias_Sectores.DataSource = Dt_Colonias;
                Grid_Colonias_Sectores.PageIndex = Pagina;
                Grid_Colonias_Sectores.DataBind();
                Grid_Colonias_Sectores.Columns[1].Visible = false;
                Grid_Colonias_Sectores.Columns[0].Visible = false;
                Session["Dt_Colonias"] = Dt_Colonias;
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }    

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Colonias
        ///DESCRIPCIÓN: Metodo que llena el Combo de Colonias con las colonias existentes.
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 06/Agosto/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Combo_Colonias()
        {
            try
            {
                Cls_Cat_Pre_Sectores_Negocio Colonia = new Cls_Cat_Pre_Sectores_Negocio();
                DataTable Colonias = Colonia.Llenar_Combo_Colonias();
                DataRow fila = Colonias.NewRow();
                fila[Cat_Ate_Colonias.Campo_Nombre] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
                fila[Cat_Ate_Colonias.Campo_Colonia_ID] = "SELECCIONE";
                Colonias.Rows.InsertAt(fila, 0);
                Cmb_Colonia.DataTextField = Cat_Ate_Colonias.Campo_Nombre;
                Cmb_Colonia.DataValueField = Cat_Ate_Colonias.Campo_Colonia_ID;
                Cmb_Colonia.DataSource = Colonias;
                Cmb_Colonia.DataBind();
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Colonias
        ///DESCRIPCIÓN: Metodo que llena el Combo de Colonias con las colonias existentes.
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 06/Agosto/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Llenar_Clave_Colonia(object sender, EventArgs e)
        {
            try
            {
                Cls_Cat_Pre_Sectores_Negocio Colonia = new Cls_Cat_Pre_Sectores_Negocio();
                Colonia.P_Colonia_ID = Cmb_Colonia.SelectedItem.Value;
                DataSet Colonias = Colonia.Llenar_Clave_Colonia();
                Txt_Clave_Colonia.Text = Colonias.Tables[0].Rows[0]["CLAVE"].ToString();
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    
        #region Validaciones

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Validar_Componentes
            ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
            ///             una operación.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 29/Octubre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private bool Validar_Componentes() {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                String Mensaje_Error = "";
                Boolean Validacion = true;
                if (Txt_Nombre.Text.Trim().Length == 0) {
                    Mensaje_Error = Mensaje_Error + "+ Introducir el Nombre.";
                    Validacion = false;
                }
                if (Txt_Comentarios.Text.Trim().Length > 100) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ Ajustar los comentarios, ya que rebasa el limte establecido por " + (Txt_Comentarios.Text.Trim().Length-100).ToString() + " caracteres.";
                    Validacion = false;
                }
                if (!Validacion) {
                    Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
                    Div_Contenedor_Msj_Error.Visible = true;
                }
                return Validacion;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Encontrar_Cajero
            ///DESCRIPCIÓN: Permite verificar que una Asignacion de Cajas no sea repetida.
            ///PROPIEDADES:   
            ///             1. Empleado.    Numero de Empleado que se verificara.
            ///             2. Modulo.      Nombre del Modulo que se verificara.
            ///             3. Caja.        Numero de la Caja que se verificara.
            ///             4. Turno.       Nombre del turno que se verificara.
            ///             5. Tabla.       Tabla a la que se hara la verificación.
            ///CREO: José Alfredo García Pichardo.
            ///FECHA_CREO: 07/Julio/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private Boolean Encontrar_Detalle(String Clave, String Colonia_ID, DataTable Tabla)
            {
                Boolean Encontrada = false;
                if (Tabla != null && Tabla.Rows.Count > 0)
                {
                    for (int cnt = 0; cnt < Tabla.Rows.Count; cnt++)
                    {
                        if (Tabla.Rows[cnt][0].ToString().Trim().Equals(Clave) &&
                            Tabla.Rows[cnt][1].ToString().Trim().Equals(Colonia_ID))
                        {
                            Encontrada = true;
                            break;
                        }
                    }
                }
                return Encontrada;
            }

        #endregion

    #endregion

    #region Grids

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Sectores_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView de los Sectores
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 29/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Sectores_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try{
                Grid_Sectores.SelectedIndex = (-1);
                Llenar_Grid_Sectores(e.NewPageIndex);
                Limpiar_Catalogo();
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Sectores_SelectedIndexChanged
        ///DESCRIPCIÓN: Obtiene los datos de un Sector Seleccionado para mostrarlos a detalle
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 29/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Sectores_SelectedIndexChanged(object sender, EventArgs e) {
            try{
                if (Grid_Sectores.SelectedIndex > (-1)){
                    Limpiar_Catalogo();
                    Txt_ID_Sector.Text = HttpUtility.HtmlDecode(Grid_Sectores.SelectedRow.Cells[1].Text.Trim());
                    Txt_Clave_Sector.Text = HttpUtility.HtmlDecode(Grid_Sectores.SelectedRow.Cells[2].Text.Trim());
                    Txt_Nombre.Text = HttpUtility.HtmlDecode(Grid_Sectores.SelectedRow.Cells[3].Text.Trim());
                    Txt_Comentarios.Text = HttpUtility.HtmlDecode(Grid_Sectores.SelectedRow.Cells[4].Text.Trim());
                    Llenar_Grid_Colonias(0);
                    Grid_Colonias_Sectores.Visible = true;
                    //Tab_Sectores_Colonias.Enabled = true;
                    System.Threading.Thread.Sleep(500);
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Sectores_Colonias_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView de los Sectores-Colonias
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 06/Septiembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Sectores_Colonias_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Grid_Sectores.SelectedIndex = (-1);
                Llenar_Grid_Colonias(e.NewPageIndex);
                
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
        ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta una nuevo Sector.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 29/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Nuevo_Click(object sender, EventArgs e){
            try{
                if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
                {
                    Configuracion_Formulario(false);
                    Limpiar_Catalogo();
                    Btn_Nuevo.AlternateText = "Dar de Alta";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.Visible = false;
                    Cls_Cat_Pre_Sectores_Negocio Sector = new Cls_Cat_Pre_Sectores_Negocio();
                    Txt_Clave_Sector.Text = Sector.Obtener_Clave_Maxima();
                    Txt_Clave_Sector.Enabled = false;
                    Session["Dt_Colonias"] = null;
                    Session["Colonias_Sectores"] = null;
                    Grid_Colonias_Sectores.Visible = false;
                    //Tab_Sectores_Colonias.Enabled = false;
                }
                else
                {
                    DataTable Dt_Colonias_Sectores = (DataTable)Session["Colonias_Sectores"];
                    if (Validar_Componentes() && Btn_Nuevo.AlternateText.Equals("Dar de Alta"))
                    {
                        Cls_Cat_Pre_Sectores_Negocio Sector = new Cls_Cat_Pre_Sectores_Negocio();
                        Sector.P_Clave = Txt_Clave_Sector.Text.Trim();
                        Sector.P_Nombre = Txt_Nombre.Text.ToUpper().Trim();
                        Sector.P_Comentarios = Txt_Comentarios.Text.ToUpper().Trim();
                        Sector.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Sector.Alta_Sector();
                        if (Session["Colonias_Sectores"] != null && Dt_Colonias_Sectores.Rows.Count > 0)
                        {
                            //if (Btn_Nuevo.AlternateText.Equals("Colonias"))
                            //{
                            Cls_Cat_Pre_Sectores_Negocio Colonia = new Cls_Cat_Pre_Sectores_Negocio();
                            Colonia.P_Colonias = (DataTable)Session["Colonias_Sectores"];
                            //Colonia.P_Sector_ID = Txt_ID_Sector.Text.Trim();
                            Colonia.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                            Colonia.Alta_Colonias();
                            Limpiar_Catalogo();
                            Configuracion_Formulario(true);
                            Session["Colonias_Sectores"] = null;
                            Grid_Colonias_Sectores.Visible = false;
                            Btn_Salir.AlternateText = "Salir";
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                            Tab_Sectores.Enabled = true;
                            //Tab_Sectores_Colonias.Enabled = false;        
                            //}
                        }
                        Configuracion_Formulario(true);
                        Txt_Clave_Sector.Enabled = false;
                        Limpiar_Catalogo();
                        Grid_Colonias_Sectores.Visible = false;
                        Tab_Catalogo_Sectores.ActiveTabIndex = 0;
                        Llenar_Grid_Sectores(Grid_Sectores.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Sector", "alert('Alta de Sector Exitosa');", true);
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    }
                }
                
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
        ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de un Sector
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 29/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Modificar_Click(object sender, EventArgs e){
            try{
                if (Btn_Modificar.AlternateText.Equals("Modificar")){
                    if (Grid_Sectores.Rows.Count > 0 && Grid_Sectores.SelectedIndex > (-1)){
                        Configuracion_Formulario(false);
                        Btn_Modificar.AlternateText = "Actualizar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                        Btn_Salir.AlternateText = "Cancelar";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        Btn_Nuevo.Visible = false;
                        Txt_Clave_Sector.Enabled = false;
                    }else{
                        Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Modificar.";
                        Lbl_Mensaje_Error.Text = "";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                } else {
                    DataTable Dt_Colonias_Sectores = (DataTable)Session["Colonias_Sectores"];
                    if (Validar_Componentes()){
                        Cls_Cat_Pre_Sectores_Negocio Sector = new Cls_Cat_Pre_Sectores_Negocio();
                        String Sector_ID = Txt_ID_Sector.Text.Trim();
                        Sector.P_Sector_ID = Sector_ID;
                        Sector.P_Nombre = Txt_Nombre.Text.Trim().ToUpper();
                        Sector.P_Comentarios = Txt_Comentarios.Text.Trim().ToUpper();
                        Sector.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Sector.Modificar_Sector();
                        if (Session["Colonias_Sectores"] != null && Dt_Colonias_Sectores.Rows.Count > 0)
                        {
                            //if (Btn_Nuevo.AlternateText.Equals("Colonias"))
                            //{
                            Cls_Cat_Pre_Sectores_Negocio Colonia = new Cls_Cat_Pre_Sectores_Negocio();
                            Colonia.P_Colonias = (DataTable)Session["Colonias_Sectores"];
                            Colonia.P_Sector_ID = Sector_ID;
                            Colonia.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                            Colonia.Alta_Colonias();
                            Limpiar_Catalogo();
                            Configuracion_Formulario(true);
                            Session["Colonias_Sectores"] = null;
                            Btn_Salir.AlternateText = "Salir";
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                            Tab_Sectores.Enabled = true;
                            //Tab_Sectores_Colonias.Enabled = false;        
                            //}
                        }
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Grid_Colonias_Sectores.Visible = false;
                        Tab_Catalogo_Sectores.ActiveTabIndex = 0;
                        Llenar_Grid_Sectores(Grid_Sectores.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Sector", "alert('Actualización de Sector Exitosa');", true);
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    }
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Sector_Click
        ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 29/Octubre/2010
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Sector_Click(object sender, ImageClickEventArgs e) {
            try{
                Limpiar_Catalogo();
                Grid_Sectores.SelectedIndex = (-1);
                Grid_Sectores.SelectedIndex = (-1);
                Llenar_Grid_Sectores(0);
                if (Grid_Sectores.Rows.Count == 0 && Txt_Busqueda_Sector.Text.Trim().Length > 0) {
                    Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el nombre \"" + Txt_Busqueda_Sector.Text + "\" no se encontraron coincidencias";
                    Lbl_Mensaje_Error.Text = "(Se cargaron todos los sectores almacenados)";
                    Div_Contenedor_Msj_Error.Visible = true;
                    Txt_Busqueda_Sector.Text = "";
                    Llenar_Grid_Sectores(0);
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
        ///DESCRIPCIÓN: Elimina un Estado Predio de la Base de Datos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 29/Octubre/2010
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Eliminar_Click(object sender, EventArgs e){
            try{
                if (Grid_Sectores.Rows.Count > 0 && Grid_Sectores.SelectedIndex > (-1)){
                    Cls_Cat_Pre_Sectores_Negocio Sector = new Cls_Cat_Pre_Sectores_Negocio();
                    Sector.P_Sector_ID = Grid_Sectores.SelectedRow.Cells[1].Text;
                    Sector.Eliminar_Sector();
                    Grid_Sectores.SelectedIndex = (-1);
                    Llenar_Grid_Sectores(Grid_Sectores.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Sector", "alert('El Sector fue eliminado exitosamente');", true);
                    Limpiar_Catalogo();
                }else{
                    Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Eliminar.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
        ///DESCRIPCIÓN: Cancela la operación que esta en proceso (Alta o Actualizar) o Sale del Formulario.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 29/Octubre/2010
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Salir_Click(object sender, EventArgs e){
            if (Btn_Salir.AlternateText.Equals("Salir")){
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Configuracion_Formulario(true);
                Session["Colonias_Sectores"] = null;
                Limpiar_Catalogo();
                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Grid_Colonias_Sectores.Visible = false;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
        ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta una nueva colonia
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 19/Julio/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Agregar_Click(object sender, EventArgs e)
        {
            if (Cmb_Colonia.SelectedItem.Value != null && Cmb_Colonia.SelectedIndex != 0)
            {
                try
                {
                    Session["Dt_Colonias"] = null;
                    DataTable tabla;
                    //Tab_Colonias_Click();
                    if (Session["Colonias_Sectores"] == null)
                    {
                        tabla = (DataTable)Grid_Colonias_Sectores.DataSource;
                        Session["Colonias_Sectores"] = tabla;
                        tabla = new DataTable("Colonias_Sectores");
                        tabla.Columns.Add("COLONIA_ID", Type.GetType("System.String"));
                        tabla.Columns.Add("CLAVE", Type.GetType("System.String"));
                        tabla.Columns.Add("NOMBRE", Type.GetType("System.String"));
                    }
                    else
                    {
                        tabla = (DataTable)Session["Colonias_Sectores"];
                    }
                    if (!Encontrar_Detalle(Cmb_Colonia.SelectedItem.Value,
                        Txt_Clave_Colonia.Text, tabla))
                    {
                        DataRow fila = tabla.NewRow();
                        fila["COLONIA_ID"] = HttpUtility.HtmlDecode(Cmb_Colonia.SelectedItem.Value);
                        fila["CLAVE"] = HttpUtility.HtmlDecode(Txt_Clave_Colonia.Text);
                        fila["NOMBRE"] = HttpUtility.HtmlDecode(Cmb_Colonia.SelectedItem.Text);
                        Grid_Colonias_Sectores.Columns[0].Visible = true;
                        Grid_Colonias_Sectores.Columns[1].Visible = true;
                        //Grid_Movimientos.Columns[4].Visible = true;
                        tabla.Rows.Add(fila);
                        Session["Colonias_Sectores"] = tabla;
                        Grid_Colonias_Sectores.DataSource = tabla;
                        Grid_Colonias_Sectores.DataBind();
                        Grid_Colonias_Sectores.Columns[0].Visible = false;
                        Grid_Colonias_Sectores.Columns[1].Visible = false;
                        Grid_Colonias_Sectores.Visible = true;
                        //Grid_Movimientos.Columns[4].Visible = false;
                        //Limpiar_Catalogo();
                        //Grid_Colonias_Sectores.Enabled = false;
                        Cmb_Colonia.SelectedIndex = 0; 
                    }
                }
                catch (Exception Ex)
                {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Borrar_Registro
        ///DESCRIPCIÓN: Permite borrar un registro del grid y elimina el registro en la base de datos
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 06/Septiembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Borrar_Registro(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Erase"))
            {
                DataTable Dt_Colonias = (DataTable)Session["Dt_Colonias"];
                DataTable Dt_Colonias_Sectores = (DataTable)Session["Colonias_Sectores"];
                if (Session["Dt_Colonias"] != null)
                {
                    Int32 Registro = ((Grid_Colonias_Sectores.PageIndex) *
                    Grid_Colonias_Sectores.PageSize) + (Convert.ToInt32(e.CommandArgument));

                    if (Dt_Colonias.Rows.Count > 0)
                    {
                        DataTable Tabla = (DataTable)Session["Dt_Colonias"];
                        Cls_Cat_Pre_Sectores_Negocio Eliminar = new Cls_Cat_Pre_Sectores_Negocio();
                        Eliminar.P_Colonia_ID = Grid_Colonias_Sectores.Rows[Convert.ToInt32(e.CommandArgument)].Cells[1].Text;
                        Tabla.Rows.RemoveAt(Registro);
                        Eliminar.Eliminar_Colonia();
                        Cmb_Colonia.SelectedIndex = 0;                        
                        Llenar_Combo_Colonias();
                        Cmb_Colonia.DataBind();
                        Session["Dt_Colonias"] = Tabla;
                        Grid_Colonias_Sectores.Columns[0].Visible = true;
                        Grid_Colonias_Sectores.Columns[1].Visible = true;
                        Grid_Colonias_Sectores.PageIndex = 0;
                        Grid_Colonias_Sectores.DataSource = Tabla;
                        Grid_Colonias_Sectores.DataBind();
                        Grid_Colonias_Sectores.Columns[0].Visible = false;
                        Grid_Colonias_Sectores.Columns[1].Visible = false;
                    }
                }
                else if (Session["Colonias_Sectores"] != null)
                {
                    Int32 Registro = ((Grid_Colonias_Sectores.PageIndex) *
                    Grid_Colonias_Sectores.PageSize) + (Convert.ToInt32(e.CommandArgument));

                    if (Dt_Colonias_Sectores.Rows.Count > 0)
                    {
                        DataTable Tabla = (DataTable)Session["Colonias_Sectores"];
                        Tabla.Rows.RemoveAt(Registro);
                        Session["Colonias_Sectores"] = Tabla;
                        Grid_Colonias_Sectores.Columns[0].Visible = true;
                        Grid_Colonias_Sectores.Columns[1].Visible = true;
                        Grid_Colonias_Sectores.PageIndex = 0;
                        Grid_Colonias_Sectores.DataSource = Tabla;
                        Grid_Colonias_Sectores.DataBind();
                        Grid_Colonias_Sectores.Columns[0].Visible = false;
                        Grid_Colonias_Sectores.Columns[1].Visible = false;
                    }
                }             
            }
        }

    #endregion

    #region

        //protected void Tab_Colonias_Click()
        //{
        //    try
        //    {
        //        //Tab_Sectores.Enabled = false;
        //        Btn_Nuevo.AlternateText = "Colonias";
        //        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
        //        Btn_Modificar.Visible = false;
        //        Btn_Eliminar.Visible = false;
        //        Btn_Salir.AlternateText = "Cancelar_Colonia";
        //        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
        //    }
        //    catch (Exception Ex)
        //    {
        //        Lbl_Ecabezado_Mensaje.Text = Ex.Message;
        //        Lbl_Mensaje_Error.Text = "";
        //        Div_Contenedor_Msj_Error.Visible = true;
        //    }
        //}

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
            Botones.Add(Btn_Buscar_Sector);

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