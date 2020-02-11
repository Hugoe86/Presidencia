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
using Presidencia.Catalogo_Rangos_Identificadores_Colonias.Negocio;
using Presidencia.Sessiones;

public partial class paginas_predial_Frm_Cat_Pre_Rangos_Identificadores_Colonias : System.Web.UI.Page
{

    #region Page_Load

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
        ///PROPIEDADES:     
        ///CREO: 
        ///FECHA_CREO: 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************        
        protected void Page_Load(object sender, EventArgs e){
            if (!IsPostBack) {
                Configuracion_Formulario(true);
                Llenar_Tabla_Rangos_Identificadores_Colonias(0);
                Llenar_Combo_Tipos_Colonias();
            }
            Div_Contenedor_Msj_Error.Visible = false;
        }

    #endregion

    #region Metodos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Configuracion_Formulario
        ///DESCRIPCIÓN          : Carga una configuracion de los controles del Formulario
        ///PROPIEDADES          : 1. Estatus.    Estatus en el que se cargara la configuración de los controles.
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 28/Octubre/2010 
        ///MODIFICO
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
            Grid_Rangos_Identificadores_Colonias.Enabled = Estatus;
            Grid_Rangos_Identificadores_Colonias.SelectedIndex = (-1);
            Cmb_Estatus.Enabled = !Estatus;
            Cmb_Tipo_Colonia.Enabled = !Estatus;
            Txt_Busqueda.Enabled = Estatus;
            Txt_Rango_Final.Enabled = !Estatus;
            Txt_Rango_Inicial.Enabled = !Estatus;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Limpiar_Catálogo
        ///DESCRIPCIÓN          : Limpia los controles del Formulario
        ///PROPIEDADES          :     
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 28/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Catálogo() {
            Hdf_Rango_Identificador_Colonia.Value = "";
            Cmb_Estatus.SelectedIndex = 0;
            Cmb_Tipo_Colonia.SelectedIndex = 0;
            Txt_Busqueda.Text = "";
            Txt_Rango_Final.Text = "";
            Txt_Rango_Identificador_Colonia_ID.Text = "";
            Txt_Rango_Inicial.Text = "";
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Llenar_Tabla_Rangos_Identificadores_Colonias
        ///DESCRIPCIÓN          : Llena la tabla de Rangos_Identificadores_Colonias con una consulta que puede o no tener Filtros.
        ///PROPIEDADES          : 1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 28/Ocubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Tabla_Rangos_Identificadores_Colonias(int Pagina){
            try{
                Cls_Cat_Pre_Rangos_Identificadores_Colonias_Negocio Rango_Identificador_Colonia = new Cls_Cat_Pre_Rangos_Identificadores_Colonias_Negocio();
                Rango_Identificador_Colonia.P_Tipo_DataTable = "RANGOS_IDENTIFICADORES";
                Rango_Identificador_Colonia.P_Tipo_Colonia_ID = Txt_Busqueda.Text.Trim().ToUpper();
                Grid_Rangos_Identificadores_Colonias.DataSource = Rango_Identificador_Colonia.Consultar_DataTable();
                Grid_Rangos_Identificadores_Colonias.PageIndex = Pagina;
                Grid_Rangos_Identificadores_Colonias.DataBind();
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Llenar_Combo_Tipos_Colonias
        ///DESCRIPCIÓN          : Llena el Combo de Tipos de Colonias con los existentes en la Base de Datos.
        ///PROPIEDADES          : 1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
        ///CREO                 : Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO           : 04/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Combo_Tipos_Colonias() {
            try{
                Cls_Cat_Pre_Rangos_Identificadores_Colonias_Negocio Rangos_Identificadores = new Cls_Cat_Pre_Rangos_Identificadores_Colonias_Negocio();
                Rangos_Identificadores.P_Tipo_DataTable = "TIPOS_COLONIAS";
                DataTable Tipos_Colonias = Rangos_Identificadores.Consultar_DataTable();
                DataRow Fila_Tipo_Colonia = Tipos_Colonias.NewRow();
                Fila_Tipo_Colonia["TIPO_COLONIA_ID"] = HttpUtility.HtmlDecode("00000");
                Fila_Tipo_Colonia["DESCRIPCION"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
                Tipos_Colonias.Rows.InsertAt(Fila_Tipo_Colonia, 0);
                Cmb_Tipo_Colonia.DataSource = Tipos_Colonias;
                Cmb_Tipo_Colonia.DataValueField = "TIPO_COLONIA_ID";
                Cmb_Tipo_Colonia.DataTextField = "DESCRIPCION";
                Cmb_Tipo_Colonia.DataBind();         
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }   
        }

        #region Validaciones

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Validar_Componentes
            ///DESCRIPCIÓN          : Hace una validacion de que haya datos en los componentes antes de hacer una operación.
            ///PROPIEDADES:     
            ///CREO                 : Antonio Salvador Benvides Guardado
            ///FECHA_CREO           : 28/Octubre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private bool Validar_Componentes() {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                String Mensaje_Error = "";
                Boolean Validacion = true;
                if (Cmb_Estatus.SelectedIndex == 0) {
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Estatus.";
                    Validacion = false;
                }
                if (Cmb_Tipo_Colonia.SelectedIndex == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Tipos de Colonias.";
                    Validacion = false;
                }
                if (Txt_Rango_Final.Text.Trim().Equals("")) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir el Rango Final.";
                    Validacion = false;
                }
                if (Txt_Rango_Inicial.Text.Trim().Equals("")) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir el Rango Inicial.";
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
        ///NOMBRE DE LA FUNCIÓN : Grid_Rangos_Identificadores_Colonias_PageIndexChanging
        ///DESCRIPCIÓN          : Maneja la paginación del GridView de los Rangos_Identificadores_Colonias
        ///PROPIEDADES:
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 28/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Rangos_Identificadores_Colonias_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                Grid_Rangos_Identificadores_Colonias.SelectedIndex = (-1);
                Llenar_Tabla_Rangos_Identificadores_Colonias(e.NewPageIndex);
                Limpiar_Catálogo();
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Grid_Rangos_Identificadores_Colonias_SelectedIndexChanged
        ///DESCRIPCIÓN          : Obtiene los datos de un Rango_Identificador_Colonia Seleccionado para mostrarlos a detalle
        ///PROPIEDADES:     
        ///CREO                 : Antonio Benavides Guardado
        ///FECHA_CREO           : 28/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Rangos_Identificadores_Colonias_SelectedIndexChanged(object sender, EventArgs e) {
            try{
                if (Grid_Rangos_Identificadores_Colonias.SelectedIndex > (-1)) {
                    Limpiar_Catálogo();
                    String ID_Seleccionado = Grid_Rangos_Identificadores_Colonias.SelectedRow.Cells[1].Text;
                    Cls_Cat_Pre_Rangos_Identificadores_Colonias_Negocio Rango_Identificador_Colonia = new Cls_Cat_Pre_Rangos_Identificadores_Colonias_Negocio();
                    Rango_Identificador_Colonia.P_Rango_Identificador_Colonia_ID = ID_Seleccionado;
                    Rango_Identificador_Colonia = Rango_Identificador_Colonia.Consultar_Datos_Rango_Identificador_Colonia();
                    Hdf_Rango_Identificador_Colonia.Value = Rango_Identificador_Colonia.P_Rango_Identificador_Colonia_ID;
                    Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Rango_Identificador_Colonia.P_Estatus));
                    Cmb_Tipo_Colonia.SelectedIndex = Cmb_Tipo_Colonia.Items.IndexOf(Cmb_Tipo_Colonia.Items.FindByValue(Rango_Identificador_Colonia.P_Tipo_Colonia_ID));
                    Txt_Rango_Final.Text = Rango_Identificador_Colonia.P_Rango_Final.ToString();
                    Txt_Rango_Identificador_Colonia_ID.Text = Rango_Identificador_Colonia.P_Rango_Identificador_Colonia_ID;
                    Txt_Rango_Inicial.Text = Rango_Identificador_Colonia.P_Rango_Inicial.ToString();
                    System.Threading.Thread.Sleep(1000);          
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
        ///NOMBRE DE LA FUNCIÓN : Btn_Nuevo_Click
        ///DESCRIPCIÓN          : Deja los componentes listos para dar de Alta un nuevo Rango_Identificador_Colonia
        ///PROPIEDADES:     
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 28/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Nuevo_Click(object sender, EventArgs e){
            try{
                if (Btn_Nuevo.AlternateText.Equals("Nuevo")){
                    Configuracion_Formulario(false);
                    Limpiar_Catálogo();
                    Btn_Nuevo.AlternateText = "Dar de Alta";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.Visible = false;
                } else {
                    if (Validar_Componentes()) {
                        Cls_Cat_Pre_Rangos_Identificadores_Colonias_Negocio Rango_Identificador_Colonia = new Cls_Cat_Pre_Rangos_Identificadores_Colonias_Negocio();
                        Rango_Identificador_Colonia.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        Rango_Identificador_Colonia.P_Tipo_Colonia_ID = Cmb_Tipo_Colonia.SelectedItem.Value;
                        Rango_Identificador_Colonia.P_Rango_Inicial = Convert.ToInt32(Txt_Rango_Inicial.Text.Trim());
                        Rango_Identificador_Colonia.P_Rango_Final = Convert.ToInt32(Txt_Rango_Final.Text.Trim());
                        Rango_Identificador_Colonia.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        if (Rango_Identificador_Colonia.Alta_Rango_Identificador_Colonia()) {
                            Configuracion_Formulario(true);
                            Limpiar_Catálogo();
                            Llenar_Tabla_Rangos_Identificadores_Colonias(Grid_Rangos_Identificadores_Colonias.PageIndex);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Rangos Identificadores de Colonias", "alert('Alta de Rango Identificador de Colonia Exitosa');", true);
                            Btn_Nuevo.AlternateText = "Nuevo";
                            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                            Btn_Salir.AlternateText = "Salir";
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        }
                        else {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Rangos Identificadores de Colonias", "alert('Alta de Rango Identificador de Colonia No fue Exitosa');", true);
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
        ///NOMBRE DE LA FUNCIÓN : Btn_Modificar_Click
        ///DESCRIPCIÓN          : Deja los componentes listos para hacer la modificacion de un Rango_Identificador_Colonia.
        ///PROPIEDADES:     
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 28/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Modificar_Click(object sender, EventArgs e){
            try{
                if (Btn_Modificar.AlternateText.Equals("Modificar")) {
                    if (Grid_Rangos_Identificadores_Colonias.Rows.Count > 0 && Grid_Rangos_Identificadores_Colonias.SelectedIndex > (-1)){
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
                    if (Validar_Componentes()){
                        Cls_Cat_Pre_Rangos_Identificadores_Colonias_Negocio Rango_Identificador_Colonia = new Cls_Cat_Pre_Rangos_Identificadores_Colonias_Negocio();
                        Rango_Identificador_Colonia.P_Rango_Identificador_Colonia_ID = Hdf_Rango_Identificador_Colonia.Value;
                        Rango_Identificador_Colonia.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        Rango_Identificador_Colonia.P_Tipo_Colonia_ID = Cmb_Tipo_Colonia.SelectedItem.Value;
                        Rango_Identificador_Colonia.P_Rango_Inicial = Convert.ToInt32(Txt_Rango_Inicial.Text.Trim());
                        Rango_Identificador_Colonia.P_Rango_Final = Convert.ToInt32(Txt_Rango_Final.Text.Trim());
                        Rango_Identificador_Colonia.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        if (Rango_Identificador_Colonia.Modificar_Rango_Identificador_Colonia()){
                            Configuracion_Formulario(true);
                            Limpiar_Catálogo();
                            Llenar_Tabla_Rangos_Identificadores_Colonias(Grid_Rangos_Identificadores_Colonias.PageIndex);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Rangos Identificadores de Colonias", "alert('Actualización Rango Identificador de Colonia Exitosa');", true);
                            Btn_Modificar.AlternateText = "Modificar";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                            Btn_Salir.AlternateText = "Salir";
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        }
                        else{
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Rangos Identificadores de Colonias", "alert('Actualización Rango Identificador de Colonia No fue Exitosa');", true);
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
        ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Rango_Identificador_Colonia_Click
        ///DESCRIPCIÓN          : Llena la Tabla con la opcion buscada
        ///PROPIEDADES          :     
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 28/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e){
            try{
                Grid_Rangos_Identificadores_Colonias.SelectedIndex = (-1);
                Llenar_Tabla_Rangos_Identificadores_Colonias(0);
                if (Grid_Rangos_Identificadores_Colonias.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0) {
                    Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el Tipo de Colonia \"" + Txt_Busqueda.Text + "\" no se encotrarón coincidencias";
                    Lbl_Mensaje_Error.Text = "(Se cargaron todos los Rangos Identificadores de Colonias almacenados)";
                    Div_Contenedor_Msj_Error.Visible = true;
                    Txt_Busqueda.Text = "";
                    Llenar_Tabla_Rangos_Identificadores_Colonias(0);
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Eliminar_Click
        ///DESCRIPCIÓN          : Elimina un Rango_Identificador_Colonia de la Base de Datos
        ///PROPIEDADES          :     
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 28/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Eliminar_Click(object sender, EventArgs e){
            try{
                if (Grid_Rangos_Identificadores_Colonias.Rows.Count > 0 && Grid_Rangos_Identificadores_Colonias.SelectedIndex > (-1)){
                    Cls_Cat_Pre_Rangos_Identificadores_Colonias_Negocio Rango_Identificador_Colonia = new Cls_Cat_Pre_Rangos_Identificadores_Colonias_Negocio();
                    Rango_Identificador_Colonia.P_Rango_Identificador_Colonia_ID = Grid_Rangos_Identificadores_Colonias.SelectedRow.Cells[1].Text;
                    if (Rango_Identificador_Colonia.Eliminar_Rango_Identificador_Colonia()) {
                        Grid_Rangos_Identificadores_Colonias.SelectedIndex = (-1);
                        Llenar_Tabla_Rangos_Identificadores_Colonias(Grid_Rangos_Identificadores_Colonias.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Rangos Identificadores de Colonias", "alert('El Rango Identificador de Colonia fue Eliminado Exitosamente');", true);
                        Limpiar_Catálogo();
                    }else{
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Rangos Identificadores de Colonias", "alert('El Rango Identificador de Colonia No fue Eliminado');", true);
                    }
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
        ///NOMBRE DE LA FUNCIÓN : Btn_Salir_Click
        ///DESCRIPCIÓN          : Cancela la operación que esta en proceso (Alta o Actualizar) o Sale del Formulario.
        ///PROPIEDADES:     
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 28/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Salir_Click(object sender, EventArgs e){
            if (Btn_Salir.AlternateText.Equals("Salir")) {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }else {
                Configuracion_Formulario(true);
                Limpiar_Catálogo();
                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            }
        }

    #endregion

}
