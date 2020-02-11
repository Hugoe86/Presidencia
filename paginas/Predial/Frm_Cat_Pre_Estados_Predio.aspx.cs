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
using Presidencia.Catalogo_Estados_Predio.Negocio;

public partial class paginas_predial_Frm_Cat_Pre_Estados_Predio : System.Web.UI.Page
{
    
    #region Page_Load

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 28/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///******************************************************************************* 
        protected void Page_Load(object sender, EventArgs e){
            if (!IsPostBack) {
                Configuracion_Formulario(true);
                Llenar_Grid_Estados_Predio(0);
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
        ///FECHA_CREO: 28/Octubre/2010 
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
            Txt_Descripcion_Estado_Predio.Enabled = !Estatus;
            Grid_Estados_Predio.Enabled = Estatus;
            Grid_Estados_Predio.SelectedIndex = (-1);
            Btn_Buscar_Estado_Predio.Enabled = Estatus;
            Txt_Busqueda_Estado_Predio.Enabled = Estatus;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
        ///DESCRIPCIÓN: Limpia los controles del Formulario
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 28/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Catalogo() {
            Txt_Descripcion_Estado_Predio.Text = "";
            Txt_ID_Estado_Predio.Text = "";
            Hdf_Estado_Predio_ID.Value = "";
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Estados_Predio
        ///DESCRIPCIÓN: Llena la tabla de Estados Predio con una consulta que puede o no
        ///             tener Filtros.
        ///PROPIEDADES:     
        ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 28/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Grid_Estados_Predio(int Pagina) {
            try{
                Cls_Cat_Pre_Estados_Predio_Negocio Estados_Predio = new Cls_Cat_Pre_Estados_Predio_Negocio();
                //Estados_Predio.P_Tipo_DataTable = "ESTADOS_PREDIO";
                Estados_Predio.P_Descripcion = Txt_Busqueda_Estado_Predio.Text.Trim().ToUpper();
                Grid_Estados_Predio.DataSource = Estados_Predio.Consultar_Estado_Predio();
                Grid_Estados_Predio.PageIndex = Pagina;
                Grid_Estados_Predio.DataBind();
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
            ///FECHA_CREO: 28/Octubre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private bool Validar_Componentes()
            {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                String Mensaje_Error = "";
                Boolean Validacion = true;
                if (Txt_Descripcion_Estado_Predio.Text.Trim().Length == 0)
                {
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
        ///NOMBRE DE LA FUNCIÓN: Grid_Estados_Predio_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView de los Estados Predio
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 28/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Estados_Predio_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            Grid_Estados_Predio.SelectedIndex = (-1);
            Llenar_Grid_Estados_Predio(e.NewPageIndex);
            Limpiar_Catalogo();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Estados_Predio_SelectedIndexChanged
        ///DESCRIPCIÓN: Obtiene los datos de un Estado Predio Seleccionado para mostrarlos a detalle
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 28/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Estados_Predio_SelectedIndexChanged(object sender, EventArgs e) {
            if (Grid_Estados_Predio.SelectedIndex > (-1)){
                Limpiar_Catalogo();
                Hdf_Estado_Predio_ID.Value = Grid_Estados_Predio.SelectedRow.Cells[1].Text.Trim();
                Txt_ID_Estado_Predio.Text = Grid_Estados_Predio.SelectedRow.Cells[1].Text.Trim();
                Txt_Descripcion_Estado_Predio.Text = Grid_Estados_Predio.SelectedRow.Cells[2].Text.Trim();
                System.Threading.Thread.Sleep(500);
            }
        }

    #endregion

    #region Eventos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
        ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta una nuevo Estado Predio
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 28/Octubre/2010 
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
                        Cls_Cat_Pre_Estados_Predio_Negocio Estado_Predio = new Cls_Cat_Pre_Estados_Predio_Negocio();
                        Estado_Predio.P_Descripcion = Txt_Descripcion_Estado_Predio.Text.ToUpper();
                        Estado_Predio.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Estado_Predio.Alta_Estado_Predio();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Grid_Estados_Predio(Grid_Estados_Predio.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Estados Predio", "alert('Alta de Estado Predio Exitosa');", true);
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
        ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de un Estado
        ///             Predio.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 28/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Modificar_Click(object sender, EventArgs e){
            try{
                if (Btn_Modificar.AlternateText.Equals("Modificar")){
                    if (Grid_Estados_Predio.Rows.Count > 0 && Grid_Estados_Predio.SelectedIndex > (-1)){
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
                        Cls_Cat_Pre_Estados_Predio_Negocio Estado_Predio = new Cls_Cat_Pre_Estados_Predio_Negocio();
                        Estado_Predio.P_Estado_Predio_ID = Hdf_Estado_Predio_ID.Value;
                        Estado_Predio.P_Descripcion = Txt_Descripcion_Estado_Predio.Text.ToUpper();
                        Estado_Predio.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Estado_Predio.Modificar_Estado_Predio();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Grid_Estados_Predio(Grid_Estados_Predio.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Estados Predio", "alert('Actualización de Estado Predio Exitosa');", true);
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
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Estado_Predio_Click
        ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 28/Octubre/2010
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Estado_Predio_Click(object sender, ImageClickEventArgs e) {
            try{
                Limpiar_Catalogo();
                Grid_Estados_Predio.SelectedIndex = (-1);
                Grid_Estados_Predio.SelectedIndex = (-1);
                Llenar_Grid_Estados_Predio(0);
                if (Grid_Estados_Predio.Rows.Count == 0 && Txt_Busqueda_Estado_Predio.Text.Trim().Length > 0) {
                    Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con la descripción \"" + Txt_Busqueda_Estado_Predio.Text + "\" no se encotrarón coincidencias";
                    Lbl_Mensaje_Error.Text = "(Se cargaron todos los estados predio almacenados)";
                    Div_Contenedor_Msj_Error.Visible = true;
                    Txt_Busqueda_Estado_Predio.Text = "";
                    Llenar_Grid_Estados_Predio(0);
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
        ///FECHA_CREO: 28/Octubre/2010
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Eliminar_Click(object sender, EventArgs e){
            try{
                if (Grid_Estados_Predio.Rows.Count > 0 && Grid_Estados_Predio.SelectedIndex > (-1)){
                    Cls_Cat_Pre_Estados_Predio_Negocio Estado_Predio = new Cls_Cat_Pre_Estados_Predio_Negocio();
                    Estado_Predio.P_Estado_Predio_ID = Grid_Estados_Predio.SelectedRow.Cells[1].Text;
                    Estado_Predio.Eliminar_Estado_Predio();
                    Grid_Estados_Predio.SelectedIndex = (-1);
                    Llenar_Grid_Estados_Predio(Grid_Estados_Predio.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Estado Predio", "alert('El Estado Predio fue eliminado exitosamente');", true);
                    Limpiar_Catalogo();
                }else{
                    Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Eliminar.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }catch(Exception Ex){
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Estado Predio", "alert('No se puede eliminar el Estado de Predio ya que se encuentra en uso.');", true);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
        ///DESCRIPCIÓN: Cancela la operación que esta en proceso (Alta o Actualizar) o Sale del Formulario.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 28/Octubre/2010
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