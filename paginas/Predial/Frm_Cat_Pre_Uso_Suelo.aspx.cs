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
using Presidencia.Catalogo_Uso_Suelo.Negocio;

public partial class paginas_predial_Frm_Cat_Pre_Uso_Suelo : System.Web.UI.Page{
    
    #region Page_Load

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 30/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        protected void Page_Load(object sender, EventArgs e){
            if (!IsPostBack) {
                Configuracion_Formulario(true);
                Llenar_Tabla_Uso_Suelo(0);
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
        ///FECHA_CREO: 30/Agosto/2010 
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
            Grid_Uso_Suelo.Enabled = estatus;
            Grid_Uso_Suelo.SelectedIndex = (-1);
            Btn_Buscar_Uso_Suelo.Enabled = estatus;
            Txt_Busqueda_Uso_Suelo.Enabled = estatus;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
        ///DESCRIPCIÓN: Limpia los controles del Formulario
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 30/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Catalogo() {
            Txt_Identificador.Text = "";
            Cmb_Estatus.SelectedIndex = 0;
            Txt_Descripcion.Text = "";
            Txt_ID_Uso_Suelo.Text = "";
            Hdf_Uso_Suelo_ID.Value = "";
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Uso_Suelo
        ///DESCRIPCIÓN: Llena la tabla de Registros de Uso de Suelo con una consulta que puede o no
        ///             tener Filtros.
        ///PROPIEDADES:     
        ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 30/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Tabla_Uso_Suelo(int Pagina)
        {
            try{
                Cls_Cat_Pre_Uso_Suelo_Negocio Uso_Suelo = new Cls_Cat_Pre_Uso_Suelo_Negocio();
                Uso_Suelo.P_Identificador = Txt_Busqueda_Uso_Suelo.Text.Trim().ToUpper();
                Grid_Uso_Suelo.DataSource = Uso_Suelo.Consultar_Uso_Suelo();
                Grid_Uso_Suelo.PageIndex = Pagina;
                Grid_Uso_Suelo.DataBind();
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        #region Validacion

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
                if (Cmb_Estatus.SelectedIndex == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Estatus.";
                    Validacion = false;
                }
                if (Txt_Descripcion.Text.Trim().Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Descripci&oacute;n.";
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
        ///NOMBRE DE LA FUNCIÓN: Grid_Uso_Suelo_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView General de los Registros de Uso de Suelo 
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 30/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Uso_Suelo_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                Grid_Uso_Suelo.SelectedIndex = (-1);
                Llenar_Tabla_Uso_Suelo(e.NewPageIndex);
                Limpiar_Catalogo();
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Uso_Suelo_SelectedIndexChanged
        ///DESCRIPCIÓN: Obtiene los datos de un Registro de Uso de Suelo Seleccionada para mostrarlos a detalle
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 30/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Uso_Suelo_SelectedIndexChanged(object sender, EventArgs e){
            try{
                if (Grid_Uso_Suelo.SelectedIndex > (-1)){
                    Limpiar_Catalogo();
                    String ID_Seleccionado = Grid_Uso_Suelo.SelectedRow.Cells[1].Text;
                    Cls_Cat_Pre_Uso_Suelo_Negocio Uso_Suelo = new Cls_Cat_Pre_Uso_Suelo_Negocio();
                    Uso_Suelo.P_Uso_Suelo_ID = ID_Seleccionado;
                    Uso_Suelo = Uso_Suelo.Consultar_Datos_Uso_Suelo();
                    Hdf_Uso_Suelo_ID.Value = Uso_Suelo.P_Uso_Suelo_ID;
                    Txt_ID_Uso_Suelo.Text = Uso_Suelo.P_Uso_Suelo_ID;
                    Txt_Identificador.Text = Uso_Suelo.P_Identificador;
                    Txt_Descripcion.Text = Uso_Suelo.P_Descripcion;
                    Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Uso_Suelo.P_Estatus));
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
        ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta una nuevo registro de 
        ///             Uso de Suelo
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 30/Agosto/2010 
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
                }else {
                    if (Validar_Componentes()){
                        Cls_Cat_Pre_Uso_Suelo_Negocio Uso_Suelo = new Cls_Cat_Pre_Uso_Suelo_Negocio();
                        Uso_Suelo.P_Identificador = Txt_Identificador.Text.ToUpper();
                        Uso_Suelo.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        Uso_Suelo.P_Descripcion = Txt_Descripcion.Text.ToUpper();
                        Uso_Suelo.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Uso_Suelo.Alta_Uso_Suelo();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Tabla_Uso_Suelo(Grid_Uso_Suelo.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Uso de Predio", "alert('Alta de Registro de Uso de Predio Exitosa');", true);
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
        ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de un Registro de Uso de Suelo
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 30/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Modificar_Click(object sender, EventArgs e){
            try{
                if (Btn_Modificar.AlternateText.Equals("Modificar")){
                    if (Grid_Uso_Suelo.Rows.Count > 0 && Grid_Uso_Suelo.SelectedIndex > (-1)){
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
                    if(Validar_Componentes()){
                        Cls_Cat_Pre_Uso_Suelo_Negocio Uso_Suelo = new Cls_Cat_Pre_Uso_Suelo_Negocio();
                        Uso_Suelo.P_Uso_Suelo_ID = Hdf_Uso_Suelo_ID.Value;
                        Uso_Suelo.P_Identificador = Txt_Identificador.Text.ToUpper();
                        Uso_Suelo.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        Uso_Suelo.P_Descripcion = Txt_Descripcion.Text.ToUpper();
                        Uso_Suelo.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Uso_Suelo.Modificar_Uso_Suelo();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Tabla_Uso_Suelo(Grid_Uso_Suelo.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Uso de Predio", "alert('Actualización Registro de Uso de Predio Exitosa');", true);
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
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Uso_Suelo_Click
        ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 30/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Uso_Suelo_Click(object sender, ImageClickEventArgs e){
            try{
                Limpiar_Catalogo();
                Grid_Uso_Suelo.SelectedIndex = (-1);
                Grid_Uso_Suelo.SelectedIndex = (-1);
                Llenar_Tabla_Uso_Suelo(0);
                if (Grid_Uso_Suelo.Rows.Count == 0 && Txt_Busqueda_Uso_Suelo.Text.Trim().Length > 0) {
                    Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el Identificador \"" + Txt_Busqueda_Uso_Suelo.Text + "\" no se encotrarón coincidencias";
                    Lbl_Mensaje_Error.Text = "(Se cargaron todos los registros de Uso de Predio almacenados)";
                    Div_Contenedor_Msj_Error.Visible = true;
                    Txt_Busqueda_Uso_Suelo.Text = "";
                    Llenar_Tabla_Uso_Suelo(0);
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
        ///DESCRIPCIÓN: Elimina un registro de Uso de Suelo de la Base de Datos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 30/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Eliminar_Click(object sender, EventArgs e){
            try{
                if (Grid_Uso_Suelo.Rows.Count > 0 && Grid_Uso_Suelo.SelectedIndex > (-1)){
                    Cls_Cat_Pre_Uso_Suelo_Negocio Uso_Suelo = new Cls_Cat_Pre_Uso_Suelo_Negocio();
                    Uso_Suelo.P_Uso_Suelo_ID = Grid_Uso_Suelo.SelectedRow.Cells[1].Text;
                    Uso_Suelo.Eliminar_Uso_Suelo();
                    Grid_Uso_Suelo.SelectedIndex = (-1);
                    Llenar_Tabla_Uso_Suelo(Grid_Uso_Suelo.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Uso de Predio", "alert('El registro de Uso de Predio fue eliminado exitosamente');", true);
                    Limpiar_Catalogo();
                }else{
                    Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Eliminar.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }catch(Exception Ex){
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Uso de Suelo", "alert('No se puede eliminar el Uso de Predio ya que se encuentra en uso.');", true);           
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
        ///DESCRIPCIÓN: Cancela la operación que esta en proceso (Alta o Actualizar) o Sale del Formulario.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 31/Agosto/2010 
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