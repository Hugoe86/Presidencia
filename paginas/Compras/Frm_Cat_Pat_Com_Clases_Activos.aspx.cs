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
using Presidencia.Sessiones;
using Presidencia.Control_Patrimonial_Catalogo_Clases_Activos.Negocio;
using System.Collections.Generic;
using Presidencia.Constantes;

public partial class paginas_Compras_Frm_Cat_Pat_Com_Clases_Activos : System.Web.UI.Page {
    
    #region Page_Load

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23/Enero/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///******************************************************************************* 
        protected void Page_Load(object sender, EventArgs e){
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            Div_Contenedor_Msj_Error.Visible = false;
            if (!IsPostBack) {
                Configuracion_Formulario(true);
                Llenar_Grid_Clases_Activos(0);
            }
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
        ///FECHA_CREO: 23/Enero/2012 
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
            Txt_Clave.Enabled = !Estatus;
            Txt_Descripcion.Enabled = !Estatus;
            Cmb_Estatus.Enabled = !Estatus;
            Grid_Clases_Activos.Enabled = Estatus;
            Grid_Clases_Activos.SelectedIndex = (-1);
            Btn_Buscar.Enabled = Estatus;
            Txt_Busqueda.Enabled = Estatus;

            //Configuracion_Acceso("Frm_Cat_Pat_Com_Clasificaciones.aspx");
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
        ///DESCRIPCIÓN: Limpia los controles del Formulario
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23/Enero/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Catalogo() {
            Hdf_Clase_Activo_ID.Value = "";
            Txt_Clase_Activo_ID.Text = "";
            Txt_Clave.Text = "";
            Txt_Descripcion.Text = "";
            Cmb_Estatus.SelectedIndex = 0;
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Clases_Activos
        ///DESCRIPCIÓN: Llena la tabla de Clases con una consulta que puede o no
        ///             tener Filtros.
        ///PROPIEDADES:     
        ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23/Enero/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Grid_Clases_Activos(int Pagina) {
            try{
                Cls_Cat_Pat_Com_Clases_Activo_Negocio Clases_Activo = new Cls_Cat_Pat_Com_Clases_Activo_Negocio();
                Clases_Activo.P_Tipo_DataTable = "CLASES_ACTIVOS";
                Clases_Activo.P_Clave = Txt_Busqueda.Text.Trim();
                Clases_Activo.P_Descripcion = Txt_Busqueda.Text.Trim();
                Grid_Clases_Activos.DataSource = Clases_Activo.Consultar_DataTable();
                Grid_Clases_Activos.PageIndex = Pagina;
                Grid_Clases_Activos.DataBind();
            }catch(Exception Ex){
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
            ///FECHA_CREO: 23/Enero/2012 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private bool Validar_Componentes() {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                String Mensaje_Error = "";
                Boolean Validacion = true;
                if (Txt_Clave.Text.Trim().Length == 0) {
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Clave de la Clase de Activo.<br />";
                    Validacion = false;
                }
                if (Txt_Descripcion.Text.Trim().Length == 0) {
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Descripción de la Clase de Activo.<br />";
                    Validacion = false;
                }
                if (Cmb_Estatus.SelectedIndex == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar una opcion del Combo de Estatus.<br />";
                    Validacion = false;
                }
                if (!Validacion) {
                    Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
                    Div_Contenedor_Msj_Error.Visible = true;
                }
                return Validacion;
            }

        #endregion

    #endregion

    #region Grids

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Clases_Activos_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView de las Clases de Activos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23/Enero/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Clases_Activos_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                Grid_Clases_Activos.SelectedIndex = (-1);
                Llenar_Grid_Clases_Activos(e.NewPageIndex);
                Limpiar_Catalogo();
            } catch(Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Clases_Activos_SelectedIndexChanged
        ///DESCRIPCIÓN: Obtiene los datos de una Clasificacion Seleccionada para mostrarlos a detalle
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23/Enero/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Clases_Activos_SelectedIndexChanged(object sender, EventArgs e) {
            try { 
                if (Grid_Clases_Activos.SelectedIndex > (-1)){
                    Limpiar_Catalogo();
                    Hdf_Clase_Activo_ID.Value = HttpUtility.HtmlDecode(Grid_Clases_Activos.SelectedRow.Cells[1].Text.Trim());
                    Txt_Clase_Activo_ID.Text = HttpUtility.HtmlDecode(Grid_Clases_Activos.SelectedRow.Cells[1].Text.Trim());
                    Txt_Clave.Text = HttpUtility.HtmlDecode(Grid_Clases_Activos.SelectedRow.Cells[2].Text.Trim());
                    Txt_Descripcion.Text = HttpUtility.HtmlDecode(Grid_Clases_Activos.SelectedRow.Cells[3].Text.Trim());
                    Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Grid_Clases_Activos.SelectedRow.Cells[4].Text.Trim()));
                    System.Threading.Thread.Sleep(500);
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

    #endregion

    #region Eventos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
        ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta una nueva Clasificacion.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23/Enero/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Nuevo_Click(object sender, EventArgs e){
            try{
                if (Btn_Nuevo.AlternateText.Equals("Nuevo")){
                    Configuracion_Formulario(false);
                    Limpiar_Catalogo();
                    Btn_Nuevo.AlternateText = "Dar de Alta";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.Visible = false;
                    Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue("VIGENTE"));
                }else {
                    if (Validar_Componentes()){
                        Cls_Cat_Pat_Com_Clases_Activo_Negocio Clase_Activo = new Cls_Cat_Pat_Com_Clases_Activo_Negocio();
                        Clase_Activo.P_Clave = Txt_Clave.Text.Trim();
                        Clase_Activo.P_Descripcion = Txt_Descripcion.Text.Trim();
                        Clase_Activo.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        Clase_Activo.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                        Clase_Activo.Alta_Clase_Activo();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Grid_Clases_Activos(Grid_Clases_Activos.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Clases de Activos", "alert('Alta de Clase de Activo Exitosa!');", true);
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
        ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de una Clasificacion
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23/Enero/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Modificar_Click(object sender, EventArgs e){
            try{
                if (Btn_Modificar.AlternateText.Equals("Modificar")){
                    if (Grid_Clases_Activos.Rows.Count > 0 && Grid_Clases_Activos.SelectedIndex > (-1)){
                        Configuracion_Formulario(false);
                        Btn_Modificar.AlternateText = "Actualizar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                        Btn_Salir.AlternateText = "Cancelar";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        Btn_Nuevo.Visible = false;
                    }else{
                        Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Modificar.";
                        Lbl_Mensaje_Error.Text = "";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                } else {
                    if (Validar_Componentes()){
                        Cls_Cat_Pat_Com_Clases_Activo_Negocio Clase_Activo = new Cls_Cat_Pat_Com_Clases_Activo_Negocio();
                        Clase_Activo.P_Clase_Activo_ID = Hdf_Clase_Activo_ID.Value;
                        Clase_Activo.P_Clave = Txt_Clave.Text.Trim();
                        Clase_Activo.P_Descripcion = Txt_Descripcion.Text.Trim();
                        Clase_Activo.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        Clase_Activo.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                        Clase_Activo.Modificar_Clase_Activo();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Grid_Clases_Activos(Grid_Clases_Activos.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Clases de Activo", "alert('Actualización de Clase de Activo Exitosa');", true);
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
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
        ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23/Enero/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e) {
            try{
                Limpiar_Catalogo();
                Grid_Clases_Activos.SelectedIndex = (-1);
                Grid_Clases_Activos.SelectedIndex = (-1);
                Llenar_Grid_Clases_Activos(0);
                if (Grid_Clases_Activos.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0) {
                    Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el nombre \"" + Txt_Busqueda.Text + "\" no se encontrarón coincidencias";
                    Lbl_Mensaje_Error.Text = "(Se cargarón todos las Clases de Activo almacenadas)";
                    Div_Contenedor_Msj_Error.Visible = true;
                    Txt_Busqueda.Text = "";
                    Llenar_Grid_Clases_Activos(0);
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
        ///DESCRIPCIÓN: Elimina una Clasificacion de la Base de Datos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23/Enero/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Eliminar_Click(object sender, EventArgs e){
            try{
                if (Grid_Clases_Activos.Rows.Count > 0 && Grid_Clases_Activos.SelectedIndex > (-1)){
                    Cls_Cat_Pat_Com_Clases_Activo_Negocio Clase_Activo = new Cls_Cat_Pat_Com_Clases_Activo_Negocio();
                    Clase_Activo.P_Clase_Activo_ID = Hdf_Clase_Activo_ID.Value;
                    Clase_Activo.Eliminar_Clase_Activo();
                    Grid_Clases_Activos.SelectedIndex = (-1);
                    Llenar_Grid_Clases_Activos(Grid_Clases_Activos.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Clases de Activo", "alert('La Clase de Activo fue eliminada exitosamente');", true);
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
        ///FECHA_CREO: 23/Enero/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Salir_Click(object sender, EventArgs e){
            if (Btn_Salir.AlternateText.Equals("Salir")){
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }else {
                Configuracion_Formulario(true);
                Limpiar_Catalogo();
                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            }
        }

    #endregion

}
