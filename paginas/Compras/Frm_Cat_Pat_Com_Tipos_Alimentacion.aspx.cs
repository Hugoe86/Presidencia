﻿using System;
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
using Presidencia.Constantes;
using System.Collections.Generic;
using Presidencia.Control_Patrimonial_Catalogo_Tipos_Alimentacion.Negocio;

public partial class paginas_predial_Frm_Cat_Pat_Com_Tipos_Alimentacion : System.Web.UI.Page
{
    #region Page_Load
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 15/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///******************************************************************************* 
        protected void Page_Load(object sender, EventArgs e){
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!IsPostBack)
            {
                Configuracion_Formulario(true);
                Llenar_Grid_Tipos_Alimentacion(0);
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
        ///FECHA_CREO: 15/Diciembre/2010 
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
            Cmb_Estatus.Enabled = !Estatus;
            Grid_Tipos_Alimentacion.Enabled = Estatus;
            Grid_Tipos_Alimentacion.SelectedIndex = (-1);
            Btn_Buscar.Enabled = Estatus;
            Txt_Busqueda.Enabled = Estatus;
            //Configuracion_Acceso("Frm_Cat_Pat_Com_Tipos_Alimentacion.aspx");
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
        ///DESCRIPCIÓN: Limpia los controles del Formulario
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 15/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Catalogo() {
            Hdf_Tipo_Alimentacion_ID.Value = "";
            Txt_Tipo_Alimentacion_ID.Text = "";
            Txt_Nombre.Text = "";
            Txt_Comentarios.Text = "";
            Cmb_Estatus.SelectedIndex = 0;
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Tipos_Alimentacion
        ///DESCRIPCIÓN: Llena la tabla de Tipos Alimentacion con una consulta que puede o no
        ///             tener Filtros.
        ///PROPIEDADES:     
        ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 15/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Grid_Tipos_Alimentacion(int Pagina) {
            try {
                Cls_Cat_Pat_Com_Tipos_Alimentacion_Negocio Tipo_Alimentacion = new Cls_Cat_Pat_Com_Tipos_Alimentacion_Negocio();
                Tipo_Alimentacion.P_Tipo_DataTable = "TIPOS_ALIMENTACION";
                Tipo_Alimentacion.P_Nombre = Txt_Busqueda.Text.Trim();
                Grid_Tipos_Alimentacion.DataSource = Tipo_Alimentacion.Consultar_DataTable();
                Grid_Tipos_Alimentacion.PageIndex = Pagina;
                Grid_Tipos_Alimentacion.DataBind();
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
            ///FECHA_CREO: 15/Diciembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private bool Validar_Componentes() {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                String Mensaje_Error = "";
                Boolean Validacion = true;
                if (Txt_Nombre.Text.Trim().Length == 0) {
                    Mensaje_Error = Mensaje_Error + "+ Introducir el Nombre del Tipo de Alimentación.";
                    Validacion = false;
                }
                if (Cmb_Estatus.SelectedIndex == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar una opcion del Combo de Estatus.";
                    Validacion = false;
                }
                if (Txt_Comentarios.Text.Trim().Length > 0) {
                    if (Txt_Comentarios.Text.Trim().Length > 150) {
                        Mensaje_Error = Mensaje_Error + "+ Que los comentarios no sobrepasen los 150 Carácteres (Se pasa por " + (Txt_Comentarios.Text.Trim().Length-150).ToString() + ").";
                        Validacion = false;
                    }                    
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
        ///NOMBRE DE LA FUNCIÓN: Grid_Tipos_Alimentacion_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView de los Tipos Alimentacion
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 15/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
            protected void Grid_Tipos_Alimentacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
            {
            try {
                Grid_Tipos_Alimentacion.SelectedIndex = (-1);
                Llenar_Grid_Tipos_Alimentacion(e.NewPageIndex);
                Limpiar_Catalogo();
            } catch(Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Tipos_Alimentacion_SelectedIndexChanged
        ///DESCRIPCIÓN: Obtiene los datos de un Tipos Alimentacion para mostrarlos a detalle
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 15/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Tipos_Alimentacion_SelectedIndexChanged(object sender, EventArgs e) {
            try{
                if (Grid_Tipos_Alimentacion.SelectedIndex > (-1)){
                    Limpiar_Catalogo();
                    String Tipo_Alimento_ID = Grid_Tipos_Alimentacion.SelectedRow.Cells[1].Text.Trim();
                    Cls_Cat_Pat_Com_Tipos_Alimentacion_Negocio Tipo_Alimento = new Cls_Cat_Pat_Com_Tipos_Alimentacion_Negocio();
                    Tipo_Alimento.P_Tipo_Alimentacion_ID = Tipo_Alimento_ID;
                    Tipo_Alimento = Tipo_Alimento.Consultar_Datos_Tipo_Alimentacion();
                    Hdf_Tipo_Alimentacion_ID.Value = Tipo_Alimento.P_Tipo_Alimentacion_ID;
                    Txt_Tipo_Alimentacion_ID.Text = Tipo_Alimento.P_Tipo_Alimentacion_ID;
                    Txt_Nombre.Text = Tipo_Alimento.P_Nombre;
                    Txt_Comentarios.Text = Tipo_Alimento.P_Comentarios;
                    Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Tipo_Alimento.P_Estatus));
                    System.Threading.Thread.Sleep(500);
                }
            } catch(Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

    #endregion

    #region Eventos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
        ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta un nuevo Tipo de 
        ///             Alimentacion.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 15/Diciembre/2010 
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
                        Cls_Cat_Pat_Com_Tipos_Alimentacion_Negocio Tipo_Alimentacion = new Cls_Cat_Pat_Com_Tipos_Alimentacion_Negocio();
                        Tipo_Alimentacion.P_Nombre = Txt_Nombre.Text.Trim();
                        Tipo_Alimentacion.P_Comentarios = Txt_Comentarios.Text.Trim();
                        Tipo_Alimentacion.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        Tipo_Alimentacion.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                        Tipo_Alimentacion.Alta_Tipo_Alimentacion();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Grid_Tipos_Alimentacion(Grid_Tipos_Alimentacion.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tipos de Alimentos", "alert('Alta de Tipo de Alimento Exitosa');", true);
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
        ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de un Tipo de 
        ///             Alimentacion.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 15/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Modificar_Click(object sender, EventArgs e){
            try{
                if (Btn_Modificar.AlternateText.Equals("Modificar")){
                    if (Grid_Tipos_Alimentacion.Rows.Count > 0 && Grid_Tipos_Alimentacion.SelectedIndex > (-1)){
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
                        Cls_Cat_Pat_Com_Tipos_Alimentacion_Negocio Tipo_Alimentacion = new Cls_Cat_Pat_Com_Tipos_Alimentacion_Negocio();
                        Tipo_Alimentacion.P_Tipo_Alimentacion_ID = Hdf_Tipo_Alimentacion_ID.Value;
                        Tipo_Alimentacion.P_Nombre = Txt_Nombre.Text.Trim();
                        Tipo_Alimentacion.P_Comentarios = Txt_Comentarios.Text.Trim();
                        Tipo_Alimentacion.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        Tipo_Alimentacion.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                        Tipo_Alimentacion.Modificar_Tipo_Alimentacion();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Grid_Tipos_Alimentacion(Grid_Tipos_Alimentacion.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tipos de Alimentos", "alert('Actualización de Tipo de Alimento Exitosa');", true);
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
        ///FECHA_CREO: 15/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e) {
            try{
                Limpiar_Catalogo();
                Grid_Tipos_Alimentacion.SelectedIndex = (-1);
                Llenar_Grid_Tipos_Alimentacion(0);
                if (Grid_Tipos_Alimentacion.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0) {
                    Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el nombre \"" + Txt_Busqueda.Text + "\" no se encontrarón coincidencias";
                    Lbl_Mensaje_Error.Text = "(Se cargarón todos los Tipos de Alimentos almacenados)";
                    Div_Contenedor_Msj_Error.Visible = true;
                    Txt_Busqueda.Text = "";
                    Llenar_Grid_Tipos_Alimentacion(0);
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
        ///DESCRIPCIÓN: Elimina un Tipo de Alimentacion de la Base de Datos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 15/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Eliminar_Click(object sender, EventArgs e){
            try{
                if (Grid_Tipos_Alimentacion.Rows.Count > 0 && Grid_Tipos_Alimentacion.SelectedIndex > (-1)){
                    Cls_Cat_Pat_Com_Tipos_Alimentacion_Negocio Tipo_Alimentacion = new Cls_Cat_Pat_Com_Tipos_Alimentacion_Negocio();
                    Tipo_Alimentacion.P_Tipo_Alimentacion_ID = Hdf_Tipo_Alimentacion_ID.Value;
                    Tipo_Alimentacion.Eliminar_Tipo_Alimentacion();
                    Grid_Tipos_Alimentacion.SelectedIndex = (-1);
                    Llenar_Grid_Tipos_Alimentacion(Grid_Tipos_Alimentacion.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tipos de Alimentos", "alert('El Tipo de Alimento fue eliminado exitosamente');", true);
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
        ///FECHA_CREO: 15/Diciembre/2010 
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
                Botones.Add(Btn_Buscar);

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
                                Cls_Util.Configuracion_Acceso_Sistema_SIAS_AlternateText(Botones, Dr_Menus[0]); // Habilitamos la configuracón de los botones.
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
                throw new Exception("Error al validar si es un dato numerico. Error [" + Ex.Message + "]");
            }
            return Resultado;
        }
        #endregion
    
}