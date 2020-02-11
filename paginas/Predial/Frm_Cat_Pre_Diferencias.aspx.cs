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
using Presidencia.Catalogo_Diferencias.Negocio;

public partial class paginas_predial_Frm_Cat_Pre_Diferencias : System.Web.UI.Page{

    #region Page_Load

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
        ///PROPIEDADES:
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Septiembre/2010 
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
                    Configuracion_Acceso("Frm_Cat_Pre_Diferencias.aspx");
                    Configuracion_Formulario(true);
                    Llenar_Tabla_Tasas_Predial(0);
                }
            }
            catch (Exception ex)
            {
                Lbl_Ecabezado_Mensaje.Text = ex.Message.ToString();
                Lbl_Ecabezado_Mensaje.Visible = true;
                Lbl_Mensaje_Error.Visible = true;
            }
            Div_Contenedor_Msj_Error.Visible = false;
        }

    #endregion

    #region Metodos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
        ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
        ///PROPIEDADES:     
        ///             1. estatus.    Estatus en el que se cargara la configuración de los
        ///                             controles.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Configuracion_Formulario( Boolean estatus ) {
            Btn_Nuevo.Visible = true;
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            Btn_Modificar.Visible = true;
            Btn_Modificar.AlternateText = "Modificar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            Btn_Eliminar.Visible = estatus;
            Txt_Identificador.Enabled = !estatus;
            Cmb_Estatus.Enabled = !estatus;
            Txt_Descripcion.Enabled = !estatus;
            Grid_Tasas_Generales.Enabled = estatus;
            Grid_Tasas_Generales.SelectedIndex = (-1);
            Txt_Anio.Enabled = !estatus;
            Txt_Tasa_Anual.Enabled = !estatus;
            Btn_Agregar_Tasa.Visible = !estatus;
            Btn_Quitar_Tasa.Visible = !estatus;
            Btn_Modificar_Tasa.Visible = !estatus;
            Grid_Tasas.SelectedIndex = (-1);
            Grid_Tasas.Columns[1].Visible = false;
            Btn_Buscar_Diferencia.Enabled = estatus;
            Txt_Busqueda_Diferencia.Enabled = estatus;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
        ///DESCRIPCIÓN: Limpia los controles del Formulario
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Catalogo() {
            Txt_Identificador.Text = "";
            Cmb_Estatus.SelectedIndex = 0;
            Txt_Descripcion.Text = "";
            Txt_Anio.Text = "";
            Txt_Tasa_Anual.Text = "";
            Txt_ID_Diferencia.Text = "";
            Grid_Tasas.DataSource = new DataTable();
            Grid_Tasas.DataBind();
            Hdf_Diferencia_ID.Value = "";
            Hdf_Diferencia_Tasa_ID.Value = "";
        }

        #region Grids

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Tasas
            ///DESCRIPCIÓN: Llena la tabla de Tasas.
            ///PROPIEDADES:     
            ///             1.  Pagina. Pagina en la cual se mostrará el Grid_VIew
            ///             2.  tabla.  Tabla que se va a cargar en el Grid.    
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 01/Septiembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Tabla_Tasas(int Pagina, DataTable Tabla) {
                Grid_Tasas.Columns[1].Visible = true;
                Tabla.DefaultView.Sort = "ANIO DESC, TASA_ANUAL DESC";
                Grid_Tasas.DataSource = Tabla;
                Grid_Tasas.PageIndex = Pagina;
                Grid_Tasas.DataBind();
                Grid_Tasas.Columns[1].Visible = false;
                Session["Dt_Tasas"] = Tabla;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Tasas_Predial
            ///DESCRIPCIÓN: Llena la tabla de tasas predial con una consulta que puede o no
            ///             tener Filtros.
            ///PROPIEDADES:     
            ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 01/Septiembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Tabla_Tasas_Predial(int Pagina) {
                try{
                    Cls_Cat_Pre_Diferencias_Negocio Tasa_Predial = new Cls_Cat_Pre_Diferencias_Negocio();
                    Tasa_Predial.P_Identificador = Txt_Busqueda_Diferencia.Text.Trim().ToUpper();
                    Grid_Tasas_Generales.DataSource = Tasa_Predial.Consultar_Diferencias();
                    Grid_Tasas_Generales.PageIndex = Pagina;
                    Grid_Tasas_Generales.DataBind();
                }catch(Exception Ex){
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;                
                }
            }

        #endregion

        #region Validaciones

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Generales
            ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
            ///             una operación en la pestaña de Diferencias.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 02/Septiembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private bool Validar_Componentes_Generales()
            {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                String Mensaje_Error = "";
                Boolean Validacion = true;
                if (Txt_Identificador.Text.Trim().Length == 0)
                {
                    Mensaje_Error = Mensaje_Error + "+ Introducir el Identificador (Pestaña 1 de 2).";
                    Validacion = false;
                }
                if (Cmb_Estatus.SelectedIndex == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Estatus (Pestaña 1 de 2).";
                    Validacion = false;
                }
                if (Txt_Descripcion.Text.Trim().Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Descripci&oacute;n (Pestaña 1 de 2).";
                    Validacion = false;
                }
                if (!Validacion)
                {
                    Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
                    Div_Contenedor_Msj_Error.Visible = true;
                }
                return Validacion;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Tasas
            ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
            ///             una operación en la pestaña de Diferencias - Tasas.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 02/Septiembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private bool Validar_Componentes_Tasas()
            {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                String Mensaje_Error = "";
                Boolean Validacion = true;
                if (Txt_Anio.Text.Trim().Length == 0)
                {
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Tasa (Pestaña 2 de 2).";
                    Validacion = false;
                }
                if (Txt_Tasa_Anual.Text.Trim().Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Tasa Anual (Pestaña 2 de 2).";
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

    #endregion

    #region Grids
    
        ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Grid_Tasas_Prediales_Generales_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView General de las Tasas prediales 
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Tasas_Prediales_Generales_PageIndexChanging(object sender, GridViewPageEventArgs e){
            Grid_Tasas_Generales.SelectedIndex = (-1);
            Llenar_Tabla_Tasas_Predial(e.NewPageIndex);
            Limpiar_Catalogo();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Tasas_Prediales_Generales_SelectedIndexChanged
        ///DESCRIPCIÓN: Obtiene los datos de la tasa predial Seleccionada para mostrarlos a detalle
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Tasas_Prediales_Generales_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Grid_Tasas_Generales.SelectedIndex > (-1))
                {
                    Limpiar_Catalogo();
                    Session["Dt_Tasas_Generales"] = null;
                    String ID_Seleccionado = Grid_Tasas_Generales.SelectedRow.Cells[1].Text;
                    Cls_Cat_Pre_Diferencias_Negocio Tasa_General = new Cls_Cat_Pre_Diferencias_Negocio();
                    Tasa_General.P_Tasa_Predial_ID = ID_Seleccionado;
                    Tasa_General = Tasa_General.Consultar_Datos_Diferencia();
                    Hdf_Diferencia_ID.Value = Tasa_General.P_Tasa_Predial_ID;
                    Txt_ID_Diferencia.Text = Tasa_General.P_Tasa_Predial_ID;
                    Txt_Identificador.Text = Tasa_General.P_Identificador;
                    Txt_Descripcion.Text = Tasa_General.P_Descripcion;
                    Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Tasa_General.P_Estatus));
                    Llenar_Tabla_Tasas(0, Tasa_General.P_Diferencias_Tasas);
                    System.Threading.Thread.Sleep(1000);
                }
            }
            catch { }
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Tasas_PageIndexChanging
        ///DESCRIPCIÓN: Maneja el Cambio de Pagina de la Tabla de Tasas
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Tasas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (Session["Dt_Tasas"] != null){
                DataTable tabla = (DataTable)Session["Dt_Tasas"];
                Llenar_Tabla_Tasas(e.NewPageIndex, tabla);
                Grid_Tasas.SelectedIndex = (-1);
            }
        }

    #endregion

    #region Eventos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
        ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta una nueva Tasa predial
        ///PROPIEDADES:
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Nuevo_Click(object sender, EventArgs e){
            try{
                if (Btn_Nuevo.AlternateText.Equals("Nuevo")){
                    Configuracion_Formulario(false);
                    Limpiar_Catalogo();
                    Session["Dt_Tasas"] = null;
                    Btn_Nuevo.AlternateText = "Dar de Alta";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.Visible = false;
                }else {
                    if (Validar_Componentes_Generales()){
                        Cls_Cat_Pre_Diferencias_Negocio Tasa_Predial = new Cls_Cat_Pre_Diferencias_Negocio();
                        Tasa_Predial.P_Identificador = Txt_Identificador.Text.Trim().ToUpper();
                        Tasa_Predial.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        Tasa_Predial.P_Descripcion = Txt_Descripcion.Text.Trim().ToUpper();
                        Tasa_Predial.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Tasa_Predial.P_Diferencias_Tasas = (DataTable)Session["Dt_Tasas"];
                        Tasa_Predial.Alta_Diferencia();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Session["Dt_Tasas"] = null;
                        Llenar_Tabla_Tasas_Predial(Grid_Tasas_Generales.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tasas Prediales", "alert('Alta de Tasa predial Exitosa');", true);
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Grid_Tasas.Enabled = true;
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
        ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de una Tasa Predial
        ///PROPIEDADES:
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Modificar_Click(object sender, EventArgs e){
            try{
                if (Btn_Modificar.AlternateText.Equals("Modificar")){
                    if (Grid_Tasas_Generales.Rows.Count > 0 && Grid_Tasas_Generales.SelectedIndex > (-1)){
                        Configuracion_Formulario(false);
                        Btn_Modificar.AlternateText = "Actualizar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                        Btn_Salir.AlternateText = "Cancelar";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        Btn_Nuevo.Visible = false;
                    }else{
                        Lbl_Ecabezado_Mensaje.Text = "Selecciona el Registro que quieres Modificar.";
                        Lbl_Mensaje_Error.Text = "";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                } else {
                    if (Validar_Componentes_Generales()){
                        Cls_Cat_Pre_Diferencias_Negocio Tasa_Predial = new Cls_Cat_Pre_Diferencias_Negocio();
                        Tasa_Predial.P_Tasa_Predial_ID = Hdf_Diferencia_ID.Value;
                        Tasa_Predial.P_Identificador = Txt_Identificador.Text.Trim().ToUpper();
                        Tasa_Predial.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        Tasa_Predial.P_Descripcion = Txt_Descripcion.Text.ToUpper();
                        Tasa_Predial.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Tasa_Predial.P_Diferencias_Tasas = (DataTable)Session["Dt_Tasas"];
                        Tasa_Predial.Modificar_Diferencia();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Tabla_Tasas_Predial(Grid_Tasas_Generales.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tasas Prediales", "alert('Actualización de Tasa predial Exitosa');", true);
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Tab_Contenedor_Pestagnas.TabIndex = 0;
                        Grid_Tasas.Enabled = true;
                    }
                }
            }catch(Exception Ex){
                Lbl_Mensaje_Error.Text = "No se puede guardar el registro ya que se elimino una tasa predial que se encontraba en uso. Cancele los cambios por favor.";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Tasa_Predial_Click
        ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Tasa_Predial_Click(object sender, ImageClickEventArgs e)
        {
            try{
                Limpiar_Catalogo();
                Session["Dt_Tasas"] = null;
                Grid_Tasas_Generales.SelectedIndex = (-1);
                Grid_Tasas.SelectedIndex = (-1);
                Llenar_Tabla_Tasas_Predial(0);
                if (Grid_Tasas_Generales.Rows.Count == 0 && Txt_Busqueda_Diferencia.Text.Trim().Length > 0) {
                    Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el nombre \"" + Txt_Busqueda_Diferencia.Text + "\" no se encotrarón coincidencias";
                    Lbl_Mensaje_Error.Text = "(Se cargaron todas las tasas prediales almacenadas)";
                    Div_Contenedor_Msj_Error.Visible = true;
                    Txt_Busqueda_Diferencia.Text = "";
                    Llenar_Tabla_Tasas_Predial(0);
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
        ///DESCRIPCIÓN: Elimina una tasa predial de la Base de Datos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Eliminar_Click(object sender, EventArgs e){
            try{
                if (Grid_Tasas_Generales.Rows.Count > 0 && Grid_Tasas_Generales.SelectedIndex > (-1)){
                    Cls_Cat_Pre_Diferencias_Negocio Tasa_Predial = new Cls_Cat_Pre_Diferencias_Negocio();
                    Tasa_Predial.P_Tasa_Predial_ID = Grid_Tasas_Generales.SelectedRow.Cells[1].Text;
                    Tasa_Predial.Eliminar_Diferencia();
                    Grid_Tasas_Generales.SelectedIndex = (-1);
                    Llenar_Tabla_Tasas_Predial(Grid_Tasas_Generales.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tasas Prediales", "alert('La Tasa predial fue eliminada exitosamente');", true);
                    Tab_Contenedor_Pestagnas.TabIndex = 0;
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
        ///FECHA_CREO: 01/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Salir_Click(object sender, EventArgs e){
            if (Btn_Salir.AlternateText.Equals("Salir")){
                Session.Remove("Dt_Tasas");
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }else {
                Configuracion_Formulario(true);
                Limpiar_Catalogo();
                Tab_Contenedor_Pestagnas.TabIndex = 0;
                Session["Dt_Tasas"] = null;
                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Grid_Tasas.Enabled = true;
            }
        }

        #region Diferencias - Tasas

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Tasa_Click
            ///DESCRIPCIÓN: Agrega una nueva tasa a la tabla de Tasas(Solo en Interfaz no en la Base de Datos)
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 01/Septiembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Btn_Agregar_Tasa_Click(object sender, EventArgs e){
                try{
                    if (Validar_Componentes_Tasas()) {
                        DataTable tabla = (DataTable)Grid_Tasas.DataSource;
                        if (tabla == null){
                            if (Session["Dt_Tasas"] == null){
                                tabla = new DataTable("dif_tas");
                                tabla.Columns.Add("TASA_ID", Type.GetType("System.String"));
                                tabla.Columns.Add("ANIO", Type.GetType("System.String"));
                                tabla.Columns.Add("TASA_ANUAL", Type.GetType("System.String"));
                            }else{
                                tabla = (DataTable)Session["Dt_Tasas"];
                            }
                        }
                        foreach (DataRow Dr_Registro in tabla.Rows)
                        {
                            if (Dr_Registro["ANIO"].ToString() == Txt_Anio.Text.Trim() && Convert.ToDouble(Dr_Registro["TASA_ANUAL"].ToString()) == Convert.ToDouble(Txt_Tasa_Anual.Text.Trim()))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tasas Prediales", "alert('Año y tasa ya existentes. Registro no agregado.');", true);
                                return;
                            }
                        }
                        DataRow fila = tabla.NewRow();
                        fila["TASA_ID"] = HttpUtility.HtmlDecode("");
                        fila["ANIO"] = HttpUtility.HtmlDecode(Txt_Anio.Text.Trim());
                        fila["TASA_ANUAL"] = HttpUtility.HtmlDecode(Txt_Tasa_Anual.Text.Trim());
                        tabla.Rows.Add(fila);
                        tabla.DefaultView.Sort="ANIO DESC, TASA_ANUAL DESC";
                        Grid_Tasas.DataSource = tabla;
                        Session["Dt_Tasas"] = tabla;
                        Grid_Tasas.DataBind();
                        Grid_Tasas.SelectedIndex = (-1);
                        Txt_Anio.Text = "";
                        Txt_Tasa_Anual.Text = "";
                    }
                }catch(Exception Ex){
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;                
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Tasa_Click
            ///DESCRIPCIÓN: Modifica una tasa a la tabla de Tasas(Solo en Interfaz no en la Base de Datos)
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 01/Septiembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Btn_Modificar_Tasa_Click(object sender, EventArgs e){
                try{
                    if (Btn_Modificar_Tasa.AlternateText.Equals("Modificar")){
                        if (Grid_Tasas.Rows.Count > 0 && Grid_Tasas.SelectedIndex > (-1)){
                            Hdf_Diferencia_Tasa_ID.Value = Grid_Tasas.SelectedRow.Cells[1].Text.Trim();
                            Txt_Tasa_ID.Text = Grid_Tasas.SelectedRow.Cells[1].Text.Trim();
                            Txt_Anio.Text = Grid_Tasas.SelectedRow.Cells[2].Text.Trim();
                            Txt_Tasa_Anual.Text = Convert.ToDouble(Grid_Tasas.SelectedRow.Cells[3].Text.Trim()).ToString("#,###,###.00");
                            Btn_Modificar_Tasa.AlternateText = "Actualizar";
                            Btn_Quitar_Tasa.Visible = false;
                            Btn_Agregar_Tasa.Visible = false;
                            Grid_Tasas.Enabled = false;
                        }else{
                            Lbl_Ecabezado_Mensaje.Text = "Selecciona el Registro que quieres Modificar.";
                            Lbl_Mensaje_Error.Text = "";
                            Div_Contenedor_Msj_Error.Visible = true;
                        }
                    }else {
                        if (Validar_Componentes_Tasas()){
                            int registro = ((Grid_Tasas.PageIndex) * Grid_Tasas.PageSize) + (Grid_Tasas.SelectedIndex);
                            if (Session["Dt_Tasas"] != null){
                                DataTable tabla = (DataTable)Session["Dt_Tasas"];
                                int indice = 0;
                                foreach (DataRow Dr_Registro in tabla.Rows)
                                {
                                    if (Dr_Registro["ANIO"].ToString() == Txt_Anio.Text.Trim() && Convert.ToDouble(Dr_Registro["TASA_ANUAL"].ToString()) == Convert.ToDouble(Txt_Tasa_Anual.Text.Trim()) && indice!=registro)
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tasas Prediales", "alert('Año y tasa ya existentes. Registro no Modificado.');", true);
                                        return;
                                    }
                                    indice++;
                                }
                                tabla.DefaultView.AllowEdit = true;
                                tabla.Rows[registro].BeginEdit();
                                tabla.Rows[registro][1] = Txt_Anio.Text.Trim();
                                tabla.Rows[registro][2] = Txt_Tasa_Anual.Text.Trim();
                                tabla.Rows[registro].EndEdit();
                                Session["Dt_Tasas"] = tabla;
                                Llenar_Tabla_Tasas(Grid_Tasas.PageIndex, tabla);
                                Grid_Tasas.SelectedIndex = (-1);
                                Btn_Modificar_Tasa.AlternateText = "Modificar";
                                Btn_Quitar_Tasa.Visible = true;
                                Btn_Agregar_Tasa.Visible = true;
                                Grid_Tasas.Enabled = true;
                                Hdf_Diferencia_Tasa_ID.Value = "";
                                Txt_Tasa_ID.Text = "";
                                Txt_Anio.Text = "";
                                Txt_Tasa_Anual.Text = "";
                            }   
                         }
                    }
                }catch(Exception Ex){
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;                
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Quitar_Tasa_Click
            ///DESCRIPCIÓN: Quita una tasa a la tabla de Tasas(Solo en Interfaz no en la Base de Datos)
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 01/Septiembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Btn_Quitar_Tasa_Click(object sender, EventArgs e){
                try{
                    if (Grid_Tasas.Rows.Count > 0 && Grid_Tasas.SelectedIndex > (-1)){
                        int registro = ((Grid_Tasas.PageIndex) * Grid_Tasas.PageSize) + (Grid_Tasas.SelectedIndex);
                        if (Session["Dt_Tasas"] != null){
                            DataTable tabla = (DataTable)Session["Dt_Tasas"];
                            tabla.Rows.RemoveAt(registro);
                            tabla.DefaultView.Sort = "ANIO DESC, TASA_ANUAL DESC";
                            Session["Dt_Tasas"] = tabla;
                            Grid_Tasas.SelectedIndex = (-1);
                            Llenar_Tabla_Tasas(Grid_Tasas.PageIndex, tabla);
                        }
                    } else {
                        Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Quitar.";
                        Lbl_Mensaje_Error.Text = "";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                }catch(Exception Ex){
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
        
        #endregion

    #endregion

            protected void Txt_Tasa_Anual_TextChanged(object sender, EventArgs e)
            {
                if (Txt_Tasa_Anual.Text.Trim() == "")
                {
                    Txt_Tasa_Anual.Text = "0.00";
                }
                else
                {
                    try
                    {
                        Txt_Tasa_Anual.Text = Convert.ToDouble(Txt_Tasa_Anual.Text).ToString("#,###,###,###,###,###,###,###,##0.00");
                    }
                    catch
                    {
                        Txt_Tasa_Anual.Text = "0.00";
                    }
                }
            }

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
                    Botones.Add(Btn_Buscar_Diferencia);
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