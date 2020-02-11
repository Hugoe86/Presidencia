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
using Presidencia.Catalogo_Tipos_Colonias.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;

public partial class paginas_predial_Frm_Cat_Pre_Tipos_Colonias : System.Web.UI.Page
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
                Llenar_Tabla_Tipos_Colonias(0);
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
        ///FECHA_CREO           : 26/Octubre/2010 
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
            Grid_Tipos_Colonias.Enabled = Estatus;
            Grid_Tipos_Colonias.SelectedIndex = (-1);
            Cmb_Estatus.Enabled = !Estatus;
            Txt_Busqueda.Enabled = Estatus;
            Txt_Descripcion.Enabled = !Estatus;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Limpiar_Catálogo
        ///DESCRIPCIÓN          : Limpia los controles del Formulario
        ///PROPIEDADES          :     
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 26/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Catalogo() {
            Hdf_Tipo_Colonia.Value = "";
            Cmb_Estatus.SelectedIndex = 0;
            Txt_Tipo_Colonia_ID.Text = "";
            Txt_Descripcion.Text = "";
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Llenar_Tabla_Tipos_Colonias
        ///DESCRIPCIÓN          : Llena la tabla de Tipos_Colonias con una consulta que puede o no tener Filtros.
        ///PROPIEDADES          : 1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 26/Ocubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Tabla_Tipos_Colonias(int Pagina) {
            try{
                Cls_Cat_Pre_Tipos_Colonias_Negocio Tipo_Colonia = new Cls_Cat_Pre_Tipos_Colonias_Negocio();
                Tipo_Colonia.P_Descripcion = Txt_Busqueda.Text.Trim().ToUpper();
                Grid_Tipos_Colonias.DataSource = Tipo_Colonia.Consultar_Tipos_Colonias();
                Grid_Tipos_Colonias.PageIndex = Pagina;
                Grid_Tipos_Colonias.DataBind();
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
            ///FECHA_CREO           : 26/Octubre/2010 
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
                if (Txt_Descripcion.Text.Trim().Equals("")) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Descripción.";
                    Validacion = false;
                }
                if (Txt_Descripcion.Text.Trim().Length > 50) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Que la Descripción tenga un máximo de 50 Carácteres (Sobrepasa por " + (Txt_Descripcion.Text.Trim().Length - 50) + ").";
                    Validacion = false;
                }
                if (Validar_Comentarios())
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Esa Descripción ya Existe";
                    Validacion = false;
                }
                if (!Validacion) {
                    Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
                    Div_Contenedor_Msj_Error.Visible = true;
                }
                return Validacion;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Validar_Comentarios
            ///DESCRIPCIÓN: Hace una validacion de que el campo de Comentarios no se repita en 
            ///             la base de datos
            ///PROPIEDADES:     
            ///CREO: José Alfredo García Pichardo.
            ///FECHA_CREO: 18/Agosto/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private bool Validar_Comentarios()
            {
                Cls_Cat_Pre_Tipos_Colonias_Negocio Comentarios = new Cls_Cat_Pre_Tipos_Colonias_Negocio();
                Comentarios.P_Descripcion = Cat_Pre_Tipos_Colonias.Campo_Descripcion;
                Comentarios.P_Tabla = Cat_Pre_Tipos_Colonias.Tabla_Cat_Pre_Tipos_Colonias;
                Comentarios.P_ID = Cat_Pre_Tipos_Colonias.Campo_Tipo_Colonia_ID;
                Comentarios.P_Campo_ID = Txt_Tipo_Colonia_ID.Text.Trim();
                DataSet Tabla = Comentarios.Validar_Descripcion();
                String Descripcion = Txt_Descripcion.Text.ToUpper().Trim();
                for (int i = 0; i < Tabla.Tables[0].Rows.Count; i++)
                {
                    if (Descripcion.Equals(Tabla.Tables[0].Rows[i]["DESCRIPCION"].ToString().Trim()))
                    {
                        return true;
                    }
                }
                return false;
            }

        #endregion

    #endregion

    #region Grids

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Grid_Tipos_Colonias_PageIndexChanging
        ///DESCRIPCIÓN          : Maneja la paginación del GridView de los Tipos_Colonias
        ///PROPIEDADES:
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 26/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Tipos_Colonias_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                Grid_Tipos_Colonias.SelectedIndex = (-1);
                Llenar_Tabla_Tipos_Colonias(e.NewPageIndex);
                Limpiar_Catalogo();
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }

        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Grid_Tipos_Colonias_SelectedIndexChanged
        ///DESCRIPCIÓN          : Obtiene los datos de un Tipo_Colonia Seleccionado para mostrarlos a detalle
        ///PROPIEDADES:     
        ///CREO                 : Antonio Benavides Guardado
        ///FECHA_CREO           : 26/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Tipos_Colonias_SelectedIndexChanged(object sender, EventArgs e) {
            try{
                if (Grid_Tipos_Colonias.SelectedIndex > (-1)) {
                    Limpiar_Catalogo();
                    String ID_Seleccionado = Grid_Tipos_Colonias.SelectedRow.Cells[1].Text;
                    Cls_Cat_Pre_Tipos_Colonias_Negocio Tipo_Colonia = new Cls_Cat_Pre_Tipos_Colonias_Negocio();
                    Tipo_Colonia.P_Tipo_Colonia_ID = ID_Seleccionado;
                    Tipo_Colonia = Tipo_Colonia.Consultar_Datos_Tipo_Colonia();
                    Hdf_Tipo_Colonia.Value = Tipo_Colonia.P_Tipo_Colonia_ID;
                    Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Tipo_Colonia.P_Estatus));
                    Txt_Descripcion.Text = Tipo_Colonia.P_Descripcion;
                    Txt_Tipo_Colonia_ID.Text = Tipo_Colonia.P_Tipo_Colonia_ID;
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
        ///DESCRIPCIÓN          : Deja los componentes listos para dar de Alta un nuevo Tipo_Colonia
        ///PROPIEDADES:     
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 26/Octubre/2010 
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
                    Grid_Tipos_Colonias.Visible = false;
                } else {
                    if (Validar_Componentes()) {
                        Cls_Cat_Pre_Tipos_Colonias_Negocio Tipo_Colonia = new Cls_Cat_Pre_Tipos_Colonias_Negocio();
                        Tipo_Colonia.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        Tipo_Colonia.P_Descripcion = Txt_Descripcion.Text.Trim().ToUpper();
                        Tipo_Colonia.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        if (Tipo_Colonia.Alta_Tipo_Colonia()) {
                            Configuracion_Formulario(true);
                            Limpiar_Catalogo();
                            Llenar_Tabla_Tipos_Colonias(Grid_Tipos_Colonias.PageIndex);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tipos Colonias", "alert('Alta de Tipo Colonia Exitosa');", true);
                            Btn_Nuevo.AlternateText = "Nuevo";
                            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                            Btn_Salir.AlternateText = "Salir";
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                            Grid_Tipos_Colonias.Visible = true;
                        }
                        else {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tipos Colonias", "alert('Alta de Tipo Colonia No fue Exitosa');", true);
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
        ///DESCRIPCIÓN          : Deja los componentes listos para hacer la modificacion de un Tipo_Colonia.
        ///PROPIEDADES:     
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 26/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Modificar_Click(object sender, EventArgs e){
            try{
                if (Btn_Modificar.AlternateText.Equals("Modificar")){
                    if (Grid_Tipos_Colonias.Rows.Count > 0 && Grid_Tipos_Colonias.SelectedIndex > (-1)){
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
                        Cls_Cat_Pre_Tipos_Colonias_Negocio Tipo_Colonia = new Cls_Cat_Pre_Tipos_Colonias_Negocio();
                        Tipo_Colonia.P_Tipo_Colonia_ID = Hdf_Tipo_Colonia.Value;
                        Tipo_Colonia.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        Tipo_Colonia.P_Descripcion = Txt_Descripcion.Text.Trim().ToUpper();
                        Tipo_Colonia.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        if (Tipo_Colonia.Modificar_Tipo_Colonia()){
                            Configuracion_Formulario(true);
                            Limpiar_Catalogo();
                            Llenar_Tabla_Tipos_Colonias(Grid_Tipos_Colonias.PageIndex);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tipos Colonias", "alert('Actualización Tipo Colonia Exitosa');", true);
                            Btn_Modificar.AlternateText = "Modificar";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                            Btn_Salir.AlternateText = "Salir";
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        }
                        else{
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tipos Colonias", "alert('Actualización Tipo Colonia No fue Exitosa');", true);
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
        ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Tipo_Colonia_Click
        ///DESCRIPCIÓN          : Llena la Tabla con la opcion buscada
        ///PROPIEDADES          :     
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 26/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e){
            try{
                Limpiar_Catalogo();
                Grid_Tipos_Colonias.SelectedIndex = (-1);
                Llenar_Tabla_Tipos_Colonias(0);
                if (Grid_Tipos_Colonias.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0) {
                    Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con la descripción \"" + Txt_Busqueda.Text + "\" no se encotrarón coincidencias";
                    Lbl_Mensaje_Error.Text = "(Se cargaron todos los tipos de colonia almacenados)";
                    Div_Contenedor_Msj_Error.Visible = true;
                    Txt_Busqueda.Text = "";
                    Llenar_Tabla_Tipos_Colonias(0);
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }

        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Eliminar_Click
        ///DESCRIPCIÓN          : Elimina un Tipo_Colonia de la Base de Datos
        ///PROPIEDADES          :     
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 26/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Eliminar_Click(object sender, EventArgs e){
            try{
                if (Grid_Tipos_Colonias.Rows.Count > 0 && Grid_Tipos_Colonias.SelectedIndex > (-1)){
                    Cls_Cat_Pre_Tipos_Colonias_Negocio Tipo_Colonia = new Cls_Cat_Pre_Tipos_Colonias_Negocio();
                    Tipo_Colonia.P_Tipo_Colonia_ID = Grid_Tipos_Colonias.SelectedRow.Cells[1].Text;
                    if (Tipo_Colonia.Eliminar_Tipo_Colonia()) {
                        Grid_Tipos_Colonias.SelectedIndex = (-1);
                        Llenar_Tabla_Tipos_Colonias(Grid_Tipos_Colonias.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tipos Colonias", "alert('El Tipo Colonia fue Eliminado Exitosamente');", true);
                        Limpiar_Catalogo();
                    }else{
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tipos Colonias", "alert('El Tipo Colonia No fue Eliminado');", true);
                    }
                }else{
                    Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Eliminar.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }catch(Exception Ex){
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tipos Colonias", "alert('No se puede eliminar el tipo de colonia ya que se encuentra en uso.');", true);
            }

        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Salir_Click
        ///DESCRIPCIÓN          : Cancela la operación que esta en proceso (Alta o Actualizar) o Sale del Formulario.
        ///PROPIEDADES:     
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 26/Octubre/2010 
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
                Grid_Tipos_Colonias.Visible = true;
                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            }
        }

    #endregion

}
