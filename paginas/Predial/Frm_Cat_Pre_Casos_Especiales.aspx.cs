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
using Presidencia.Catalogo_Casos_Especiales.Negocio;

public partial class paginas_predial_Frm_Cat_Pre_Casos_Especiales : System.Web.UI.Page{

    #region Page_Load

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 02/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************        
        protected void Page_Load(object sender, EventArgs e){
            if (!IsPostBack) {
                Configuracion_Formulario(true);
                Llenar_Casos_Especiales(0);
                Txt_Observaciones.Attributes.Add("OnKeyPress", " ValidarCaracteres(this,45)");
                Txt_Observaciones.Attributes.Add("OnKeyUp", " ValidarCaracteres(this,45)");
                Txt_Descripcion.Attributes.Add("OnKeyPress", " ValidarCaracteres(this,45)");
                Txt_Descripcion.Attributes.Add("OnKeyUp", " ValidarCaracteres(this,45)");
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
        ///FECHA_CREO: 02/Septiembre/2010 
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
            Txt_Articulo.Enabled = !estatus;
            Txt_Inciso.Enabled = !estatus;
            Txt_Observaciones.Enabled = !estatus;
            Grid_Casos_Especiales.Enabled = estatus;
            Grid_Casos_Especiales.SelectedIndex = (-1);
            Btn_Buscar_Caso_Especial.Enabled = estatus;
            Txt_Busqueda_Caso_Especial.Enabled = estatus;
            Cmb_Tipo.Enabled = !estatus;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
        ///DESCRIPCIÓN: Limpia los controles del Formulario
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 02/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Catalogo() {
            Txt_Identificador.Text = "";
            Cmb_Estatus.SelectedIndex = 0;
            Txt_Descripcion.Text = "";
            Txt_Articulo.Text = "";
            Txt_Inciso.Text = "";
            Txt_Porcentaje.Text = "";
            Txt_Observaciones.Text = "";
            Txt_ID_Caso_Especial.Text = "";
            Hdf_Caso_Especial_ID.Value = "";
            Cmb_Tipo.SelectedIndex = 0;
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Casos_Especiales
        ///DESCRIPCIÓN: Llena la tabla de Casos Especiales con una consulta que puede o no
        ///             tener Filtros.
        ///PROPIEDADES:     
        ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 02/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Casos_Especiales(Int32 Pagina) {
            try{
                Cls_Cat_Pre_Casos_Especiales_Negocio Caso_Especial = new Cls_Cat_Pre_Casos_Especiales_Negocio();
                Caso_Especial.P_Identificador = Txt_Busqueda_Caso_Especial.Text.Trim().ToUpper();
                Grid_Casos_Especiales.DataSource = Caso_Especial.Consultar_Casos_Especiales();
                Grid_Casos_Especiales.PageIndex = Pagina;
                Grid_Casos_Especiales.Columns[1].Visible = true;
                Grid_Casos_Especiales.DataBind();
                Grid_Casos_Especiales.Columns[1].Visible = false;
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
        ///FECHA_CREO: 02/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private bool Validar_Componentes()
        {
            Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
            String Mensaje_Error = "";
            Boolean Validacion = true;
            if (Txt_Identificador.Text.Trim().Length == 0)
            {
                Mensaje_Error = Mensaje_Error + "+ Introducir el Identificador.";
                Validacion = false;
            }
            else if (Txt_Identificador.Text.Length > 45)
            {
                Mensaje_Error = Mensaje_Error + "+ El Identificador debe de ser de 45 caracteres o menos.";
                Validacion = false;
            }
            if (Cmb_Estatus.SelectedIndex == 0)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Estatus.";
                Validacion = false;
            }
            if (Cmb_Tipo.SelectedIndex == 0)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Tipo.";
                Validacion = false;
            }
            if (Cmb_Tipo.SelectedValue == "ESCUELAS")
            {
                if (Txt_Porcentaje.Text.Trim() == "")
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introduzca un porcentaje.";
                    Validacion = false;
                }
            }
            if (Txt_Descripcion.Text.Trim().Length == 0)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir la Descripci&oacute;n.";
                Validacion = false;
            }
            if (Txt_Articulo.Text.Trim().Length == 0)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir el Articulo.";
                Validacion = false;
            }
            if (Txt_Inciso.Text.Trim().Length == 0)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir el Inciso.";
                Validacion = false;
            }
            if (Txt_Observaciones.Text.Trim().Length == 0)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir las Observaciones.";
                Validacion = false;
            }
            else if (Txt_Observaciones.Text.Length > 45)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Las Observaciones deben de ser de 45 caracteres o menos.";
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
        ///NOMBRE DE LA FUNCIÓN: Grid_Casos_Especialess_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView de los Casos Especiales 
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 02/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Casos_Especiales_PageIndexChanging(object sender, GridViewPageEventArgs e){
            Grid_Casos_Especiales.SelectedIndex = (-1);
            Llenar_Casos_Especiales(e.NewPageIndex);
            Limpiar_Catalogo();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Casos_Especiales_SelectedIndexChanged
        ///DESCRIPCIÓN: Obtiene los datos de un Caso Especial Seleccionado para mostrarlos a detalle
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 02/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Casos_Especiales_SelectedIndexChanged(object sender, EventArgs e){
            try{
                if (Grid_Casos_Especiales.SelectedIndex > (-1)) {
                    Limpiar_Catalogo();
                    String ID_Seleccionado = Grid_Casos_Especiales.SelectedRow.Cells[1].Text;
                    Cls_Cat_Pre_Casos_Especiales_Negocio Caso_Especial = new Cls_Cat_Pre_Casos_Especiales_Negocio();
                    Caso_Especial.P_Caso_Especial_ID = ID_Seleccionado;
                    Caso_Especial = Caso_Especial.Consultar_Datos_Caso_Especial();
                    Hdf_Caso_Especial_ID.Value = Caso_Especial.P_Caso_Especial_ID;
                    Txt_ID_Caso_Especial.Text = Caso_Especial.P_Caso_Especial_ID;
                    Txt_Identificador.Text = Caso_Especial.P_Identificador;
                    Txt_Descripcion.Text = Caso_Especial.P_Descripcion;
                    Txt_Articulo.Text = Caso_Especial.P_Articulo;
                    Txt_Inciso.Text = Caso_Especial.P_Inciso;
                    Cmb_Tipo.SelectedIndex = Cmb_Tipo.Items.IndexOf(Cmb_Tipo.Items.FindByValue(Caso_Especial.P_Tipo));
                    if (Cmb_Tipo.SelectedValue == "ESCUELAS")
                    {
                        Pnl_Porcentaje.Visible = true;
                        Txt_Porcentaje.Text = Convert.ToDouble(Caso_Especial.P_Porcentaje).ToString("##0.00");
                    }
                    else
                    {
                        Pnl_Porcentaje.Visible = false;
                        Txt_Porcentaje.Enabled = false;
                    }
                    Txt_Observaciones.Text = Caso_Especial.P_Observaciones;
                    Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Caso_Especial.P_Estatus));
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
        ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
        ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta un nuevo Caso Especial
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 02/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Nuevo_Click(object sender, EventArgs e){
            try {
                if (Btn_Nuevo.AlternateText.Equals("Nuevo")){
                    Configuracion_Formulario(false);
                    Limpiar_Catalogo();
                    Btn_Nuevo.AlternateText = "Dar de Alta";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.Visible = false;
                    Pnl_Porcentaje.Visible = false;
                    Txt_Porcentaje.Enabled = false;
                } else {
                    if (Validar_Componentes()) {
                        Cls_Cat_Pre_Casos_Especiales_Negocio Caso_Especial = new Cls_Cat_Pre_Casos_Especiales_Negocio();
                        Caso_Especial.P_Identificador = Txt_Identificador.Text.Trim().ToUpper();
                        Caso_Especial.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        if (Cmb_Tipo.SelectedValue == "ESCUELAS")
                        {
                            Caso_Especial.P_Porcentaje = Txt_Porcentaje.Text;
                        }
                        Caso_Especial.P_Descripcion = Txt_Descripcion.Text.ToUpper();
                        Caso_Especial.P_Articulo = Txt_Articulo.Text.Trim();
                        Caso_Especial.P_Inciso = Txt_Inciso.Text.Trim();
                        Caso_Especial.P_Tipo = Cmb_Tipo.SelectedItem.Value;
                        Caso_Especial.P_Observaciones = Txt_Observaciones.Text.ToUpper();
                        Caso_Especial.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Grid_Casos_Especiales.Columns[1].Visible = true;
                        Caso_Especial.Alta_Caso_Especial();
                        Grid_Casos_Especiales.Columns[1].Visible = false;
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Casos_Especiales(Grid_Casos_Especiales.PageIndex);
                        Pnl_Porcentaje.Visible = false;
                        Txt_Porcentaje.Enabled = false;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Casos Especiales", "alert('Alta de Caso Especial Exitosa');", true);
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
        ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de un Caso Especial
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 02/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Modificar_Click(object sender, EventArgs e){
            try{
                if (Btn_Modificar.AlternateText.Equals("Modificar")){
                    if (Grid_Casos_Especiales.Rows.Count > 0 && Grid_Casos_Especiales.SelectedIndex > (-1)){
                        Configuracion_Formulario(false);
                        Btn_Modificar.AlternateText = "Actualizar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                        Btn_Salir.AlternateText = "Cancelar";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        Btn_Nuevo.Visible = false;
                        if (Pnl_Porcentaje.Visible == true)
                        {
                            Txt_Porcentaje.Enabled = true;
                        }
                    }else{
                        Lbl_Ecabezado_Mensaje.Text = "Selecciona el Registro que quieres Modificar.";
                        Lbl_Mensaje_Error.Text = "";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                } else {
                    if (Validar_Componentes()){
                        Cls_Cat_Pre_Casos_Especiales_Negocio Caso_Especial = new Cls_Cat_Pre_Casos_Especiales_Negocio();
                        Caso_Especial.P_Caso_Especial_ID = Hdf_Caso_Especial_ID.Value;
                        Caso_Especial.P_Identificador = Txt_Identificador.Text.Trim().ToUpper();
                        Caso_Especial.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        Caso_Especial.P_Descripcion = Txt_Descripcion.Text.ToUpper();
                        Caso_Especial.P_Articulo = Txt_Articulo.Text.Trim();
                        Caso_Especial.P_Tipo = Cmb_Tipo.SelectedItem.Value;
                        if (Cmb_Tipo.SelectedValue == "ESCUELAS")
                        {
                            Caso_Especial.P_Porcentaje = Txt_Porcentaje.Text;
                        }
                        Caso_Especial.P_Inciso = Txt_Inciso.Text.Trim();
                        Caso_Especial.P_Observaciones = Txt_Observaciones.Text.ToUpper();
                        Caso_Especial.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Grid_Casos_Especiales.Columns[1].Visible = true;
                        Caso_Especial.Modificar_Caso_Especial();
                        Grid_Casos_Especiales.Columns[1].Visible = false;
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Casos_Especiales(Grid_Casos_Especiales.PageIndex);
                        Pnl_Porcentaje.Visible = false;
                        Txt_Porcentaje.Enabled = false;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Casos Especiales", "alert('Actualización Caso Especial Exitosa');", true);
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
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Caso_Especial_Click
        ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 02/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Caso_Especial_Click(object sender, ImageClickEventArgs e){
            Limpiar_Catalogo();
            Grid_Casos_Especiales.SelectedIndex = (-1);
            Llenar_Casos_Especiales(0);
            if (Grid_Casos_Especiales.Rows.Count == 0 && Txt_Busqueda_Caso_Especial.Text.Trim().Length > 0) {
                Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el Identificador \"" + Txt_Busqueda_Caso_Especial.Text + "\" no se encotrarón coincidencias";
                Lbl_Mensaje_Error.Text = "(Se cargaron todos los casos especiales almacenados)";
                Div_Contenedor_Msj_Error.Visible = true;
                Txt_Busqueda_Caso_Especial.Text = "";
                Llenar_Casos_Especiales(0);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
        ///DESCRIPCIÓN: Elimina un Caso Especial de la Base de Datos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 02/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Eliminar_Click(object sender, EventArgs e){
            try{
                if (Grid_Casos_Especiales.Rows.Count > 0 && Grid_Casos_Especiales.SelectedIndex > (-1)){
                    Cls_Cat_Pre_Casos_Especiales_Negocio Caso_Especial = new Cls_Cat_Pre_Casos_Especiales_Negocio();
                    Caso_Especial.P_Caso_Especial_ID = Grid_Casos_Especiales.SelectedRow.Cells[1].Text;
                    Caso_Especial.Eliminar_Caso_Especial();
                    Grid_Casos_Especiales.SelectedIndex = (-1);
                    Llenar_Casos_Especiales(Grid_Casos_Especiales.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Casos Especiales", "alert('El Caso Especial fue eliminado exitosamente');", true);
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
        ///FECHA_CREO: 02/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Salir_Click(object sender, EventArgs e){
            if (Btn_Salir.AlternateText.Equals("Salir")){
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }else {
                Configuracion_Formulario(true);
                Pnl_Porcentaje.Visible = false;
                Txt_Porcentaje.Enabled = false;
                Limpiar_Catalogo();
                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            }
        }

    #endregion

        protected void Cmb_Tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cmb_Tipo.SelectedValue == "ESCUELAS")
            {
                Pnl_Porcentaje.Visible = true;
                Txt_Porcentaje.Enabled = true;
            }
            else
            {
                Pnl_Porcentaje.Visible = false;
                Txt_Porcentaje.Enabled = false;
            }
        }

        protected void Txt_Porcentaje_TextChanged(object sender, EventArgs e)
        {
            if (Txt_Porcentaje.Text.Trim() == "")
            {
                Txt_Porcentaje.Text = "0.00";
            }
            else
            {
                try
                {
                    Txt_Porcentaje.Text = Convert.ToDouble(Txt_Porcentaje.Text).ToString("##0.00");
                }
                catch
                {
                    Txt_Porcentaje.Text = "0.00";
                }
            }
        }

}