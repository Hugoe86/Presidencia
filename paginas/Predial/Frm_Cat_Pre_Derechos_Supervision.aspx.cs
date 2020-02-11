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
using Presidencia.Catalogo_Derechos_Supervision.Negocio;
using Presidencia.Sessiones;

public partial class paginas_predial_Frm_Cat_Pre_Derechos_Supervision : System.Web.UI.Page{

    #region Page_Load

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 06/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        protected void Page_Load(object sender, EventArgs e){
            if (!IsPostBack) {
                Configuracion_Formulario(true);
                Llenar_Tabla_Derechos_Supervision(0);
            }
            Div_Contenedor_Msj_Error.Visible = false;
        }

    #endregion
    
    #region Metodos
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
        ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
        ///PROPIEDADES:     
        ///             1. estatus. Estatus en el que se cargara la configuración de los
        ///                         controles.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 06/Septiembre/2010 
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
            Grid_Derechos_Supervision_Generales.Enabled = estatus;
            Grid_Derechos_Supervision_Generales.SelectedIndex = (-1);
            Txt_Anio.Enabled = !estatus;
            Txt_Tasa.Enabled = !estatus;
            Btn_Agregar_Tasa.Visible = !estatus;
            Btn_Quitar_Tasa.Visible = !estatus;
            Btn_Modificar_Tasa.Visible = !estatus;
            Grid_Derechos_Supervision_Tasas.SelectedIndex = (-1);
            Grid_Derechos_Supervision_Tasas.Columns[1].Visible = false;
            Btn_Buscar_Derecho_Supervision.Enabled = estatus;
            Txt_Busqueda_Derecho_Supervision.Enabled = estatus;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
        ///DESCRIPCIÓN: Limpia los controles del Formulario
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 06/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Catalogo() {
            Txt_Identificador.Text = "";
            Cmb_Estatus.SelectedIndex = 0;
            Txt_Descripcion.Text = "";
            Txt_Anio.Text = "";
            Txt_Tasa.Text = "";
            Txt_ID_Derecho_Supervision.Text = "";
            Grid_Derechos_Supervision_Tasas.DataSource = new DataTable();
            Grid_Derechos_Supervision_Tasas.DataBind();
            Hdf_Derecho_Supervision_ID.Value = "";
            Hdf_Derecho_Supervision_Tasa_ID.Value = "";
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Derechos_Supervision
        ///DESCRIPCIÓN: Llena la tabla de Derechos Supervision con una consulta que puede o no
        ///             tener Filtros.
        ///PROPIEDADES:     
        ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 06/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Tabla_Derechos_Supervision(int Pagina) {
            try{
                Cls_Cat_Pre_Derechos_Supervision_Negocio Derecho_Supervision = new Cls_Cat_Pre_Derechos_Supervision_Negocio();
                Derecho_Supervision.P_Identificador = Txt_Busqueda_Derecho_Supervision.Text.Trim().ToUpper();
                Grid_Derechos_Supervision_Generales.DataSource = Derecho_Supervision.Consultar_Derechos_Supervision();
                Grid_Derechos_Supervision_Generales.PageIndex = Pagina;
                Grid_Derechos_Supervision_Generales.DataBind();
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Derechos_Supervision_Tasas
        ///DESCRIPCIÓN: Llena la tabla de Derechos Supervision Tasas.
        ///PROPIEDADES:     
        ///             1.  Pagina. Pagina en la cual se mostrará el Grid_VIew
        ///             2.  tabla.  Tabla que se va a cargar en el Grid.    
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 06/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Tabla_Derechos_Supervision_Tasas(int Pagina, DataTable Tabla)
        {
            Grid_Derechos_Supervision_Tasas.Columns[1].Visible = true;
            Tabla.DefaultView.Sort = "ANIO DESC, TASA DESC";
            Grid_Derechos_Supervision_Tasas.DataSource = Tabla;
            Grid_Derechos_Supervision_Tasas.PageIndex = Pagina;
            Grid_Derechos_Supervision_Tasas.DataBind();
            Grid_Derechos_Supervision_Tasas.Columns[1].Visible = false;
            Session["Dt_Derechos_Tasas"] = Tabla;
        }


        #region Validaciones

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Generales
            ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
            ///             una operación en la pestaña de Derechos Supervisión.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 06/Septiembre/2010 
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
            ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Impuestos
            ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
            ///             una operación en la pestaña de Derechos Supervisión - Tasas.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 06/Septiembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private bool Validar_Componentes_Impuestos()
            {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                String Mensaje_Error = "";
                Boolean Validacion = true;
                if (Txt_Anio.Text.Trim().Length == 0)
                {
                    Mensaje_Error = Mensaje_Error + "+ Introducir el Año (Pestaña 2 de 2).";
                    Validacion = false;
                }
                if (Txt_Tasa.Text.Trim().Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Tasa (Pestaña 2 de 2).";
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
        ///NOMBRE DE LA FUNCIÓN: Grid_Derechos_Supervision_Generales_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView General de las Derechos Supervision
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 06/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Derechos_Supervision_Generales_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                Grid_Derechos_Supervision_Generales.SelectedIndex = (-1);
                Llenar_Tabla_Derechos_Supervision(e.NewPageIndex);
                Limpiar_Catalogo();
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Derechos_Supervision_Generales_SelectedIndexChanged
        ///DESCRIPCIÓN: Obtiene los datos del Derecho Supervision Seleccionada para mostrarlos a detalle
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 06/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Derechos_Supervision_Generales_SelectedIndexChanged(object sender, EventArgs e){
            try{
                if (Grid_Derechos_Supervision_Generales.SelectedIndex > (-1)){
                    Limpiar_Catalogo();
                    Session["Dt_Derechos_Tasas"] = null;
                    String ID_Seleccionado = Grid_Derechos_Supervision_Generales.SelectedRow.Cells[1].Text;
                    Cls_Cat_Pre_Derechos_Supervision_Negocio Derecho_Supervision = new Cls_Cat_Pre_Derechos_Supervision_Negocio();
                    Derecho_Supervision.P_Derecho_Supervision_ID = ID_Seleccionado;
                    Derecho_Supervision = Derecho_Supervision.Consultar_Datos_Derecho_Supervision();
                    Hdf_Derecho_Supervision_ID.Value = Derecho_Supervision.P_Derecho_Supervision_ID;
                    Txt_ID_Derecho_Supervision.Text = Derecho_Supervision.P_Derecho_Supervision_ID;
                    Txt_Identificador.Text = Derecho_Supervision.P_Identificador;
                    Txt_Descripcion.Text = Derecho_Supervision.P_Descripcion;
                    Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Derecho_Supervision.P_Estatus));
                    Llenar_Tabla_Derechos_Supervision_Tasas(0, Derecho_Supervision.P_Derechos_Tasas);
                    System.Threading.Thread.Sleep(1000);
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Derechos_Supervision_Tasas_PageIndexChanging
        ///DESCRIPCIÓN: Maneja el Cambio de Pagina de la Tabla de Derechos Supervision Tasas
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 06/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Derechos_Supervision_Tasas_PageIndexChanging(object sender, GridViewPageEventArgs e){
            if (Session["Dt_Derechos_Tasas"] != null){
                DataTable tabla = (DataTable)Session["Dt_Derechos_Tasas"];
                Llenar_Tabla_Derechos_Supervision_Tasas(e.NewPageIndex, tabla);
                Grid_Derechos_Supervision_Tasas.SelectedIndex = (-1);
            }
        }

    #endregion

    #region Eventos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
        ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta una nuevo Derecho Supervision
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 06/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Nuevo_Click(object sender, EventArgs e){
            try{
                if (Btn_Nuevo.AlternateText.Equals("Nuevo")){
                    Configuracion_Formulario(false);
                    Limpiar_Catalogo();
                    Session["Dt_Derechos_Tasas"] = null;
                    Btn_Nuevo.AlternateText = "Dar de Alta";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.Visible = false;
                }else {
                    if (Validar_Componentes_Generales()){
                        Cls_Cat_Pre_Derechos_Supervision_Negocio Derecho_Supervision = new Cls_Cat_Pre_Derechos_Supervision_Negocio();
                        Derecho_Supervision.P_Identificador = Txt_Identificador.Text.ToUpper();
                        Derecho_Supervision.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        Derecho_Supervision.P_Descripcion = Txt_Descripcion.Text.ToUpper();
                        Derecho_Supervision.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Derecho_Supervision.P_Derechos_Tasas = (DataTable)Session["Dt_Derechos_Tasas"];
                        Derecho_Supervision.Alta_Derecho_Supervision();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Session["Dt_Derechos_Tasas"] = null;
                        Llenar_Tabla_Derechos_Supervision(Grid_Derechos_Supervision_Generales.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Derechos Supervision", "alert('Alta de Derecho Supervision Exitosa');", true);
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Grid_Derechos_Supervision_Tasas.Enabled = true;
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
        ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de un Derecho
        ///             Supervision.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 06/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Modificar_Click(object sender, EventArgs e){
            try{
                if (Btn_Modificar.AlternateText.Equals("Modificar")){
                    if (Grid_Derechos_Supervision_Generales.Rows.Count > 0 && Grid_Derechos_Supervision_Generales.SelectedIndex > (-1)){
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
                        Cls_Cat_Pre_Derechos_Supervision_Negocio Derecho_Supervision = new Cls_Cat_Pre_Derechos_Supervision_Negocio();
                        Derecho_Supervision.P_Derecho_Supervision_ID = Hdf_Derecho_Supervision_ID.Value;
                        Derecho_Supervision.P_Identificador = Txt_Identificador.Text.ToUpper();
                        Derecho_Supervision.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        Derecho_Supervision.P_Descripcion = Txt_Descripcion.Text.ToUpper();
                        Derecho_Supervision.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Derecho_Supervision.P_Derechos_Tasas = (DataTable)Session["Dt_Derechos_Tasas"];
                        Derecho_Supervision.Modificar_Derecho_Supervision();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Tabla_Derechos_Supervision(Grid_Derechos_Supervision_Generales.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Derechos Supervision", "alert('Actualización de Derecho de Supervisión Exitosa');", true);
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Tab_Contenedor_Pestagnas.TabIndex = 0;
                        Grid_Derechos_Supervision_Tasas.Enabled = true;
                    }
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Derecho_Supervision_Click
        ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 06/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Derecho_Supervision_Click(object sender, ImageClickEventArgs e){
            try{
                Limpiar_Catalogo();
                Session["Dt_Derechos_Tasas"] = null;
                Grid_Derechos_Supervision_Generales.SelectedIndex = (-1);
                Grid_Derechos_Supervision_Tasas.SelectedIndex = (-1);
                Llenar_Tabla_Derechos_Supervision(0);
                if (Grid_Derechos_Supervision_Generales.Rows.Count == 0 && Txt_Busqueda_Derecho_Supervision.Text.Trim().Length > 0) {
                    Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el Identificador \"" + Txt_Busqueda_Derecho_Supervision.Text + "\" no se encotrarón coincidencias";
                    Lbl_Mensaje_Error.Text = "(Se cargaron todos los derechos supervision almacenados)";
                    Div_Contenedor_Msj_Error.Visible = true;
                    Txt_Busqueda_Derecho_Supervision.Text = "";
                    Llenar_Tabla_Derechos_Supervision(0);
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
        ///DESCRIPCIÓN: Elimina un Derecho Supervision de la Base de Datos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 06/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Eliminar_Click(object sender, EventArgs e){
            try{
                if (Grid_Derechos_Supervision_Generales.Rows.Count > 0 && Grid_Derechos_Supervision_Generales.SelectedIndex > (-1)){
                    Cls_Cat_Pre_Derechos_Supervision_Negocio Derecho_Supervision = new Cls_Cat_Pre_Derechos_Supervision_Negocio();
                    Derecho_Supervision.P_Derecho_Supervision_ID = Grid_Derechos_Supervision_Generales.SelectedRow.Cells[1].Text;
                    Derecho_Supervision.Eliminar_Derecho_Supervision();
                    Grid_Derechos_Supervision_Generales.SelectedIndex = (-1);
                    Llenar_Tabla_Derechos_Supervision(Grid_Derechos_Supervision_Generales.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Derechos Supervisión", "alert('El Derecho de Supervisión fue eliminado exitosamente');", true);
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
        ///FECHA_CREO: 06/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Salir_Click(object sender, EventArgs e){
            if (Btn_Salir.AlternateText.Equals("Salir")){
                Session["Dt_Derechos_Tasas"] = null;
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }else {
                Configuracion_Formulario(true);
                Limpiar_Catalogo();
                Tab_Contenedor_Pestagnas.TabIndex = 0;
                Session.Remove("Dt_Derechos_Tasas");
                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Grid_Derechos_Supervision_Tasas.Enabled = true;
            }
        }

        #region Derechos Supervision - Tasas

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Tasa_Click
            ///DESCRIPCIÓN: Agrega una nueva tasa a la tabla de Derechos Supervision Tasa (Solo en
            ///             Interfaz no en la Base de Datos)
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 06/Septiembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Btn_Agregar_Tasa_Click(object sender, EventArgs e){
                try{
                    if (Validar_Componentes_Impuestos()){
                        DataTable tabla = (DataTable)Grid_Derechos_Supervision_Tasas.DataSource;
                        if (tabla == null){
                            if (Session["Dt_Derechos_Tasas"] == null){
                                tabla = new DataTable("der_tasas");
                                tabla.Columns.Add("DERECHO_SUPERVISION_TASA_ID", Type.GetType("System.String"));
                                tabla.Columns.Add("ANIO", Type.GetType("System.String"));
                                tabla.Columns.Add("TASA", Type.GetType("System.String"));
                            }else{
                                tabla = (DataTable)Session["Dt_Derechos_Tasas"];
                            }
                        }
                        DataRow fila = tabla.NewRow();
                        foreach (DataRow Dr_Registro in tabla.Rows)
                        {
                            if (Dr_Registro["ANIO"].ToString() == Txt_Anio.Text.Trim() && Convert.ToDouble(Dr_Registro["TASA"].ToString()) == Convert.ToDouble(Txt_Tasa.Text.Trim()))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Derechos de Supervisión", "alert('Año y tasa ya existentes. Registro no agregado.');", true);
                                return;
                            }
                        }
                        fila["DERECHO_SUPERVISION_TASA_ID"] = HttpUtility.HtmlDecode("");
                        fila["ANIO"] = HttpUtility.HtmlDecode(Txt_Anio.Text.Trim());
                        fila["TASA"] = HttpUtility.HtmlDecode(Txt_Tasa.Text.Trim());
                        tabla.Rows.Add(fila);
                        tabla.DefaultView.Sort = "ANIO DESC, TASA DESC";
                        Grid_Derechos_Supervision_Tasas.DataSource = tabla;
                        Session["Dt_Derechos_Tasas"] = tabla;
                        Grid_Derechos_Supervision_Tasas.DataBind();
                        Grid_Derechos_Supervision_Tasas.SelectedIndex = (-1);
                        Txt_Anio.Text = "";
                        Txt_Tasa.Text = "";
                    }
                }catch(Exception Ex){
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;                
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Tasa_Click
            ///DESCRIPCIÓN: Modifica un impuesto a la tabla de Derechos Supervision Tasas (Solo
            ///             en Interfaz no en la Base de Datos)
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 06/Septiembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Btn_Modificar_Tasa_Click(object sender, EventArgs e){
                try{
                    if (Btn_Modificar_Tasa.AlternateText.Equals("Modificar")){
                        if (Grid_Derechos_Supervision_Tasas.Rows.Count > 0 && Grid_Derechos_Supervision_Tasas.SelectedIndex > (-1)){
                            Hdf_Derecho_Supervision_Tasa_ID.Value = Grid_Derechos_Supervision_Tasas.SelectedRow.Cells[1].Text.Trim();
                            Txt_Impuesto_ID.Text = Grid_Derechos_Supervision_Tasas.SelectedRow.Cells[1].Text.Trim();
                            Txt_Anio.Text = Grid_Derechos_Supervision_Tasas.SelectedRow.Cells[2].Text.Trim();
                            Txt_Tasa.Text = Convert.ToDouble(Grid_Derechos_Supervision_Tasas.SelectedRow.Cells[3].Text.Trim()).ToString("#,###,###.00");
                            Btn_Modificar_Tasa.AlternateText = "Actualizar";
                            Btn_Quitar_Tasa.Visible = false;
                            Btn_Agregar_Tasa.Visible = false;
                            Grid_Derechos_Supervision_Tasas.Enabled = false;
                        }else{
                            Lbl_Ecabezado_Mensaje.Text = "Selecciona el Registro que quieres Modificar.";
                            Lbl_Mensaje_Error.Text = "";
                            Div_Contenedor_Msj_Error.Visible = true;
                        }
                    }else {
                        if (Validar_Componentes_Impuestos()){
                            int registro = ((Grid_Derechos_Supervision_Tasas.PageIndex) * Grid_Derechos_Supervision_Tasas.PageSize) + (Grid_Derechos_Supervision_Tasas.SelectedIndex);
                            if (Session["Dt_Derechos_Tasas"] != null){
                                DataTable tabla = (DataTable)Session["Dt_Derechos_Tasas"];
                                int indice = 0;
                                foreach (DataRow Dr_Registro in tabla.Rows)
                                {
                                    if (Dr_Registro["ANIO"].ToString() == Txt_Anio.Text.Trim() && Convert.ToDouble(Dr_Registro["TASA"].ToString()) == Convert.ToDouble(Txt_Tasa.Text.Trim()) && indice != registro)
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Derechos de Supervisión", "alert('Año y tasa ya existentes. Registro no Modificado.');", true);
                                        return;
                                    }
                                    indice++;
                                }
                                tabla.DefaultView.AllowEdit = true;
                                tabla.Rows[registro].BeginEdit();
                                if (Btn_Modificar.AlternateText == "Actualizar")
                                {
                                    tabla.Rows[registro][2] = Txt_Anio.Text.Trim();
                                    tabla.Rows[registro][3] = Txt_Tasa.Text.Trim();
                                }
                                else if (Btn_Nuevo.AlternateText == "Dar de Alta")
                                {
                                    tabla.Rows[registro][1] = Txt_Anio.Text.Trim();
                                    tabla.Rows[registro][2] = Txt_Tasa.Text.Trim();
                                }
                                tabla.Rows[registro].EndEdit();
                                tabla.DefaultView.Sort = "ANIO DESC, TASA DESC";
                                Session["Dt_Derechos_Tasas"] = tabla;
                                Llenar_Tabla_Derechos_Supervision_Tasas(Grid_Derechos_Supervision_Tasas.PageIndex, tabla);
                                Grid_Derechos_Supervision_Tasas.SelectedIndex = (-1);
                                Btn_Modificar_Tasa.AlternateText = "Modificar";
                                Btn_Quitar_Tasa.Visible = true;
                                Btn_Agregar_Tasa.Visible = true;
                                Grid_Derechos_Supervision_Tasas.Enabled = true;
                                Hdf_Derecho_Supervision_Tasa_ID.Value = "";
                                Txt_Impuesto_ID.Text = "";
                                Txt_Anio.Text = "";
                                Txt_Tasa.Text = "";
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
            ///NOMBRE DE LA FUNCIÓN: Btn_Quitar_Impuesto_Click
            ///DESCRIPCIÓN: Quita una tasa a la tabla de Derechos Supervision Tasas (Solo 
            ///             en Interfaz no en la Base de Datos)
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 06/Septiembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Btn_Quitar_Tasa_Click(object sender, EventArgs e) {
                 try{
                    if (Grid_Derechos_Supervision_Tasas.Rows.Count > 0 && Grid_Derechos_Supervision_Tasas.SelectedIndex > (-1)) {
                        int registro = ((Grid_Derechos_Supervision_Tasas.PageIndex) * Grid_Derechos_Supervision_Tasas.PageSize) + (Grid_Derechos_Supervision_Tasas.SelectedIndex);
                        if (Session["Dt_Derechos_Tasas"] != null) {
                            DataTable tabla = (DataTable)Session["Dt_Derechos_Tasas"];
                            tabla.Rows.RemoveAt(registro);
                            tabla.DefaultView.Sort = "ANIO DESC, TASA DESC";
                            Session["Dt_Derechos_Tasas"] = tabla;
                            Grid_Derechos_Supervision_Tasas.SelectedIndex = (-1);
                            Llenar_Tabla_Derechos_Supervision_Tasas(Grid_Derechos_Supervision_Tasas.PageIndex, tabla);
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

            protected void Txt_Tasa_TextChanged(object sender, EventArgs e)
            {
                if (Txt_Tasa.Text.Trim() == "")
                {
                    Txt_Tasa.Text = "0.00";
                }
                else
                {
                    try
                    {
                        Txt_Tasa.Text = Convert.ToDouble(Txt_Tasa.Text).ToString("#,###,###,###,###,###,###,###,##0.00");
                    }
                    catch
                    {
                        Txt_Tasa.Text = "0.00";
                    }
                }
            }

    #endregion
}