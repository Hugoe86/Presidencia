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
using Presidencia.Constantes;
using Presidencia.Catalogo_Bloqueo_Cuentas_Predial.Negocio;
using Presidencia.Sessiones;

public partial class paginas_predial_Frm_Cat_Pre_Bloqueo_Cuentas_Predial : System.Web.UI.Page{
    
    #region Page_Load
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 08/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************        
        protected void Page_Load(object sender, EventArgs e){
            if (!IsPostBack) {
                Configuracion_Formulario(true);
                Llenar_Bloqueo_Cuentas_Predial(0);
            }
            Div_Contenedor_Msj_Error.Visible = false;
        }

    #endregion
        
    #region Metodos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
        ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
        ///PARAMETROS:     
        ///             1. estatus.    Estatus en el que se cargara la configuración de los
        ///                             controles.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 08/Septiembre/2010 
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
            Txt_Cuenta_Predial.Enabled = !estatus;
            Cmb_Estatus.Enabled = !estatus;
            Cmb_Tipo_Bloqueo.Enabled = !estatus;
            Grid_Bloqueo_Cuentas_Predial.Enabled = estatus;
            Grid_Bloqueo_Cuentas_Predial.SelectedIndex = (-1);
            Btn_Buscar_Bloqueo_Cuenta_Predial.Enabled = estatus;
            Txt_Busqueda_Bloqueo_Cuenta_Predial.Enabled = estatus;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
        ///DESCRIPCIÓN: Limpia los controles del Formulario
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 08/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Catalogo() {
        Txt_ID_Bloqueo_Cuenta_Predial.Text = "";
        Txt_Cuenta_Predial.Text = "";
        Cmb_Estatus.SelectedIndex = 0;
        Cmb_Tipo_Bloqueo.SelectedIndex = 0;
        Hdf_Bloqueo_Cuenta_Predial_ID.Value = "";
    }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Bloqueo_Cuentas_Predial
        ///DESCRIPCIÓN: Llena la tabla de Bloqueo de Cuentas Predial con una consulta que 
        ///             puede o no tener Filtros.
        ///PARAMETROS:     
        ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 08/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Bloqueo_Cuentas_Predial(int Pagina) {
            try {    
                Cls_Cat_Pre_Bloqueo_Cuentas_Predial_Negocio Cuenta_Predial = new Cls_Cat_Pre_Bloqueo_Cuentas_Predial_Negocio();
                Cuenta_Predial.P_Cuenta_Predial = Txt_Busqueda_Bloqueo_Cuenta_Predial.Text.Trim();
                Grid_Bloqueo_Cuentas_Predial.DataSource = Cuenta_Predial.Consultar_Bloqueo_Cuentas_Predial();
                Grid_Bloqueo_Cuentas_Predial.PageIndex = Pagina;
                Grid_Bloqueo_Cuentas_Predial.DataBind();
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
            ///PARAMETROS:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 08/Septiembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private bool Validar_Componentes()
            {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                String Mensaje_Error = "";
                Boolean Validacion = true;
                if (Txt_Cuenta_Predial.Text.Trim().Length == 0)
                {
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Cuenta de Predial.";
                    Validacion = false;
                }
                if (Cmb_Estatus.SelectedIndex == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Estatus.";
                    Validacion = false;
                }
                if (Cmb_Tipo_Bloqueo.SelectedIndex == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Tipo de Bloqueo.";
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
        ///NOMBRE DE LA FUNCIÓN: Grid_Bloqueo_Cuentas_Predial_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView de los Bloqueos de Cuentas Predial
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 08/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Bloqueo_Cuentas_Predial_PageIndexChanging(object sender, GridViewPageEventArgs e){
            Grid_Bloqueo_Cuentas_Predial.SelectedIndex = (-1);
            Llenar_Bloqueo_Cuentas_Predial(e.NewPageIndex);
            Limpiar_Catalogo();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Bloqueo_Cuentas_Predial_SelectedIndexChanged
        ///DESCRIPCIÓN: Obtiene los datos de un Bloqueo de Cuentas Predial Seleccionado 
        ///             para mostrarlos a detalle
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 08/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Bloqueo_Cuentas_Predial_SelectedIndexChanged(object sender, EventArgs e){
            try {
                if (Grid_Bloqueo_Cuentas_Predial.SelectedIndex > (-1)) {
                    Limpiar_Catalogo();
                    String ID_Seleccionado = Grid_Bloqueo_Cuentas_Predial.SelectedRow.Cells[1].Text;
                    Cls_Cat_Pre_Bloqueo_Cuentas_Predial_Negocio Bloqueo_Cuenta_Predial = new Cls_Cat_Pre_Bloqueo_Cuentas_Predial_Negocio();
                    Bloqueo_Cuenta_Predial.P_Bloque_Cuenta_Predial_ID = ID_Seleccionado;
                    Bloqueo_Cuenta_Predial = Bloqueo_Cuenta_Predial.Consultar_Datos_Bloqueo_Cuentas_Predial();
                    Hdf_Bloqueo_Cuenta_Predial_ID.Value = Bloqueo_Cuenta_Predial.P_Bloque_Cuenta_Predial_ID;
                    Txt_ID_Bloqueo_Cuenta_Predial.Text = Bloqueo_Cuenta_Predial.P_Bloque_Cuenta_Predial_ID;
                    Txt_Cuenta_Predial.Text = Bloqueo_Cuenta_Predial.P_Cuenta_Predial;
                    Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Bloqueo_Cuenta_Predial.P_Estatus));
                    Cmb_Tipo_Bloqueo.SelectedIndex = Cmb_Tipo_Bloqueo.Items.IndexOf(Cmb_Tipo_Bloqueo.Items.FindByValue(Bloqueo_Cuenta_Predial.P_Tipo_Bloqueo));
                    System.Threading.Thread.Sleep(1000);   
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
        ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta un nuevo Bloqueo
        ///             de Cuentas Predial
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 08/Septiembre/2010 
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
                }else{
                    if (Validar_Componentes()){
                        Cls_Cat_Pre_Bloqueo_Cuentas_Predial_Negocio Cuenta_Predial = new Cls_Cat_Pre_Bloqueo_Cuentas_Predial_Negocio();
                        Cuenta_Predial.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
                        Cuenta_Predial.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        Cuenta_Predial.P_Tipo_Bloqueo = Cmb_Tipo_Bloqueo.SelectedItem.Value;
                        Cuenta_Predial.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Cuenta_Predial.Alta_Bloqueo_Cuentas_Predial();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Bloqueo_Cuentas_Predial(Grid_Bloqueo_Cuentas_Predial.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Bloqueo de Cuentas Predial", "alert('Alta de Bloqueo de Cuentas Predial Exitosa');", true);
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
        ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de un 
        ///             Bloqueo de Cuentas Predial
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 08/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Modificar_Click(object sender, EventArgs e){
            try{
                if (Btn_Modificar.AlternateText.Equals("Modificar")){
                    if (Grid_Bloqueo_Cuentas_Predial.Rows.Count > 0 && Grid_Bloqueo_Cuentas_Predial.SelectedIndex > (-1)){
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
                        Cls_Cat_Pre_Bloqueo_Cuentas_Predial_Negocio Cuenta_Predial = new Cls_Cat_Pre_Bloqueo_Cuentas_Predial_Negocio();
                        Cuenta_Predial.P_Bloque_Cuenta_Predial_ID = Hdf_Bloqueo_Cuenta_Predial_ID.Value;
                        Cuenta_Predial.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
                        Cuenta_Predial.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        Cuenta_Predial.P_Tipo_Bloqueo = Cmb_Tipo_Bloqueo.SelectedItem.Value;
                        Cuenta_Predial.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Cuenta_Predial.Modificar_Bloqueo_Cuentas_Predial();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Bloqueo_Cuentas_Predial(Grid_Bloqueo_Cuentas_Predial.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Bloqueo de Cuentas Predial", "alert('Actualización de Bloqueo de Cuentas Predial Exitosa');", true);
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
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Bloqueo_Cuenta_Predial_Click
        ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 08/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Bloqueo_Cuenta_Predial_Click(object sender, ImageClickEventArgs e){
            Limpiar_Catalogo();
            Grid_Bloqueo_Cuentas_Predial.SelectedIndex = (-1);
            Llenar_Bloqueo_Cuentas_Predial(0);
            if (Grid_Bloqueo_Cuentas_Predial.Rows.Count == 0 && Txt_Busqueda_Bloqueo_Cuenta_Predial.Text.Trim().Length > 0) {
                Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el nombre \"" + Txt_Busqueda_Bloqueo_Cuenta_Predial.Text + "\" no se encotrarón coincidencias";
                Lbl_Mensaje_Error.Text = "(Se cargaron todos los bloqueos de cuenta predial almacenados)";
                Div_Contenedor_Msj_Error.Visible = true;
                Txt_Busqueda_Bloqueo_Cuenta_Predial.Text = "";
                Llenar_Bloqueo_Cuentas_Predial(0);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
        ///DESCRIPCIÓN: Elimina un Bloqueo de Cuenta Predial de la Base de Datos
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 08/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Eliminar_Click(object sender, EventArgs e){
            try{
                if (Grid_Bloqueo_Cuentas_Predial.Rows.Count > 0 && Grid_Bloqueo_Cuentas_Predial.SelectedIndex > (-1)){
                    Cls_Cat_Pre_Bloqueo_Cuentas_Predial_Negocio Cuenta_Predial = new Cls_Cat_Pre_Bloqueo_Cuentas_Predial_Negocio();
                    Cuenta_Predial.P_Bloque_Cuenta_Predial_ID = Grid_Bloqueo_Cuentas_Predial.SelectedRow.Cells[1].Text;
                    Cuenta_Predial.Eliminar_Bloqueo_Cuentas_Predial();
                    Grid_Bloqueo_Cuentas_Predial.SelectedIndex = (-1);
                    Llenar_Bloqueo_Cuentas_Predial(Grid_Bloqueo_Cuentas_Predial.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Bloqueo de Cuentas Predial", "alert('El Bloqueo de Cuentas Predial fue eliminado exitosamente');", true);
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
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 08/Septiembre/2010 
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