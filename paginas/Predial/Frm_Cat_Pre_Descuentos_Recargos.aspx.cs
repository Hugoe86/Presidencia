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
using Presidencia.Catalogo_Descuentos_Recargos.Negocio;
using Presidencia.Sessiones;

public partial class paginas_predial_Frm_Cat_Pre_Descuentos_Recargos : System.Web.UI.Page{

    #region Page_Load

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 04/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        protected void Page_Load(object sender, EventArgs e){
            if (!IsPostBack) {
                Configuracion_Formulario(true);
                Llenar_Tabla_Descuentos(0);
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
        ///FECHA_CREO: 04/Septiembre/2010 
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
            Txt_Anio.Enabled = !estatus;
            Grid_Descuentos.Enabled = estatus;
            Grid_Descuentos.SelectedIndex = (-1);
            Txt_Enero.Enabled = !estatus;
            Txt_Febrero.Enabled = !estatus;
            Txt_Marzo.Enabled = !estatus;
            Txt_Abril.Enabled = !estatus;
            Txt_Mayo.Enabled = !estatus;
            Txt_Junio.Enabled = !estatus;
            Txt_Julio.Enabled = !estatus;
            Txt_Agosto.Enabled = !estatus;
            Txt_Septiembre.Enabled = !estatus;
            Txt_Octubre.Enabled = !estatus;
            Txt_Noviembre.Enabled = !estatus;
            Txt_Diciembre.Enabled = !estatus;
            Btn_Buscar_Descuento.Enabled = estatus;
            Txt_Busqueda_Descuento.Enabled = estatus;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
        ///DESCRIPCIÓN: Limpia los controles del Formulario
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 04/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Catalogo() {
            Txt_Anio.Text = "";
            Txt_Enero.Text = "";
            Txt_Febrero.Text = "";
            Txt_Marzo.Text = "";
            Txt_Abril.Text = "";
            Txt_Mayo.Text = "";
            Txt_Junio.Text = "";
            Txt_Julio.Text = "";
            Txt_Agosto.Text = "";
            Txt_Septiembre.Text = "";
            Txt_Octubre.Text = "";
            Txt_Noviembre.Text = "";
            Txt_Diciembre.Text = "";
            Txt_ID_Descuento.Text = "";
            Hdf_Descuento_ID.Value = "";
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Descuentos
        ///DESCRIPCIÓN: Llena la tabla de Descuentos Recargos con una consulta que puede o no
        ///             tener Filtros.
        ///PROPIEDADES:     
        ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 04/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Tabla_Descuentos(int Pagina) {
            try{
                Cls_Cat_Pre_Descuentos_Recargos_Negocio Descuento = new Cls_Cat_Pre_Descuentos_Recargos_Negocio();
                if (!Txt_Busqueda_Descuento.Text.Trim().Equals("")) {
                    String Elemento_Buscado = Txt_Busqueda_Descuento.Text.Trim();
                    Descuento.P_Anio = Convert.ToInt32(Elemento_Buscado);
                } else {
                    Descuento.P_Anio = -1;
                }
                Grid_Descuentos.DataSource = Descuento.Consultar_Descuentos_Recargos();
                Grid_Descuentos.PageIndex = Pagina;
                Grid_Descuentos.DataBind();
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
            ///             una operación en la pestaña de Generales y Meses.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 04/Septiembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private bool Validar_Componentes()
            {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                String Mensaje_Error = "";
                Boolean Validacion = true;
                if (Txt_Anio.Text.Trim().Length == 0)
                {
                    Mensaje_Error = Mensaje_Error + "+ Introducir el Año.";
                    Validacion = false;
                }
                if (Txt_Enero.Text.Trim().Length == 0 && Txt_Febrero.Text.Trim().Length == 0
                    && Txt_Marzo.Text.Trim().Length == 0 && Txt_Abril.Text.Trim().Length == 0
                    && Txt_Mayo.Text.Trim().Length == 0 && Txt_Junio.Text.Trim().Length == 0
                    && Txt_Julio.Text.Trim().Length == 0 && Txt_Agosto.Text.Trim().Length == 0
                    && Txt_Septiembre.Text.Trim().Length == 0 && Txt_Octubre.Text.Trim().Length == 0
                    && Txt_Noviembre.Text.Trim().Length == 0 && Txt_Diciembre.Text.Trim().Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir algun Porcentaje.";
                    Validacion = false;
                }
                if (Txt_Enero.Text.Trim().Length == 0)
                {
                    Txt_Enero.Text = "0";
                }
                if (Txt_Febrero.Text.Trim().Length == 0)
                {
                    Txt_Febrero.Text = "0";
                }
                if (Txt_Marzo.Text.Trim().Length == 0)
                {
                    Txt_Marzo.Text = "0";
                }
                if (Txt_Abril.Text.Trim().Length == 0)
                {
                    Txt_Abril.Text = "0";
                }
                if (Txt_Mayo.Text.Trim().Length == 0)
                {
                    Txt_Mayo.Text = "0";
                }
                if (Txt_Junio.Text.Trim().Length == 0)
                {
                    Txt_Junio.Text = "0";
                }
                if (Txt_Julio.Text.Trim().Length == 0)
                {
                    Txt_Julio.Text = "0";
                }
                if (Txt_Agosto.Text.Trim().Length == 0)
                {
                    Txt_Agosto.Text = "0";
                }
                if (Txt_Septiembre.Text.Trim().Length == 0)
                {
                    Txt_Septiembre.Text = "0";
                }
                if (Txt_Octubre.Text.Trim().Length == 0)
                {
                    Txt_Octubre.Text = "0";
                }
                if (Txt_Noviembre.Text.Trim().Length == 0)
                {
                    Txt_Noviembre.Text = "0";
                }
                if (Txt_Diciembre.Text.Trim().Length == 0)
                {
                    Txt_Diciembre.Text = "0";
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
        ///NOMBRE DE LA FUNCIÓN: Grid_Descuentos_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView General de los Descuentos 
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 04/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Descuentos_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                Grid_Descuentos.SelectedIndex = (-1);
                Llenar_Tabla_Descuentos(e.NewPageIndex);
                Limpiar_Catalogo();
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Descuentos_SelectedIndexChanged
        ///DESCRIPCIÓN: Obtiene los datos del Descuento Seleccionado para mostrarlos a detalle
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 04/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Descuentos_SelectedIndexChanged(object sender, EventArgs e){
            try{
                if (Grid_Descuentos.SelectedIndex > (-1)) {
                    Limpiar_Catalogo();
                    String ID_Seleccionado = Grid_Descuentos.SelectedRow.Cells[1].Text;
                    Cls_Cat_Pre_Descuentos_Recargos_Negocio Descuento_Recargos = new Cls_Cat_Pre_Descuentos_Recargos_Negocio();
                    Descuento_Recargos.P_Descuento_ID = ID_Seleccionado;
                    Descuento_Recargos = Descuento_Recargos.Consultar_Datos_Descuento_Recargos();
                    Hdf_Descuento_ID.Value = Descuento_Recargos.P_Descuento_ID;
                    Txt_ID_Descuento.Text = Descuento_Recargos.P_Descuento_ID;
                    Txt_Anio.Text = Descuento_Recargos.P_Anio.ToString();
                    Txt_Enero.Text = Descuento_Recargos.P_Enero.ToString("#,###,###.00");
                    Txt_Febrero.Text = Descuento_Recargos.P_Febrero.ToString("#,###,###.00");
                    Txt_Marzo.Text = Descuento_Recargos.P_Marzo.ToString("#,###,###.00");
                    Txt_Abril.Text = Descuento_Recargos.P_Abril.ToString("#,###,###.00");
                    Txt_Mayo.Text = Descuento_Recargos.P_Mayo.ToString("#,###,###.00");
                    Txt_Junio.Text = Descuento_Recargos.P_Junio.ToString("#,###,###.00");
                    Txt_Julio.Text = Descuento_Recargos.P_Julio.ToString("#,###,###.00");
                    Txt_Agosto.Text = Descuento_Recargos.P_Agosto.ToString("#,###,###.00");
                    Txt_Septiembre.Text = Descuento_Recargos.P_Septiembre.ToString("#,###,###.00");
                    Txt_Octubre.Text = Descuento_Recargos.P_Octubre.ToString("#,###,###.00");
                    Txt_Noviembre.Text = Descuento_Recargos.P_Noviembre.ToString("#,###,###.00");
                    Txt_Diciembre.Text = Descuento_Recargos.P_Diciembre.ToString("#,###,###.00");   
                    System.Threading.Thread.Sleep(1500);
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
        ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta un nuevo Descuento
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 04/Septiembre/2010 
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
                } else {
                    if (Validar_Componentes()) {
                        Cls_Cat_Pre_Descuentos_Recargos_Negocio Descuento_Recargos = new Cls_Cat_Pre_Descuentos_Recargos_Negocio();
                        Descuento_Recargos.P_Anio = Convert.ToInt32(Txt_Anio.Text);
                        Descuento_Recargos.P_Enero = Convert.ToDouble(Txt_Enero.Text);
                        Descuento_Recargos.P_Febrero = Convert.ToDouble(Txt_Febrero.Text);
                        Descuento_Recargos.P_Marzo = Convert.ToDouble(Txt_Marzo.Text);
                        Descuento_Recargos.P_Abril = Convert.ToDouble(Txt_Abril.Text);
                        Descuento_Recargos.P_Mayo = Convert.ToDouble(Txt_Mayo.Text);
                        Descuento_Recargos.P_Junio = Convert.ToDouble(Txt_Junio.Text);
                        Descuento_Recargos.P_Julio = Convert.ToDouble(Txt_Julio.Text);
                        Descuento_Recargos.P_Agosto = Convert.ToDouble(Txt_Agosto.Text);
                        Descuento_Recargos.P_Septiembre = Convert.ToDouble(Txt_Septiembre.Text);
                        Descuento_Recargos.P_Octubre = Convert.ToDouble(Txt_Octubre.Text);
                        Descuento_Recargos.P_Noviembre = Convert.ToDouble(Txt_Noviembre.Text);
                        Descuento_Recargos.P_Diciembre = Convert.ToDouble(Txt_Diciembre.Text);
                        Descuento_Recargos.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Descuento_Recargos.Alta_Descuento_Recargos();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Tabla_Descuentos(Grid_Descuentos.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Descuentos Recargos", "alert('Alta de Descuento Exitosa');", true);
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
        ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de un Descuento
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 04/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Modificar_Click(object sender, EventArgs e){
            try{
                if (Btn_Modificar.AlternateText.Equals("Modificar")){
                    if (Grid_Descuentos.Rows.Count > 0 && Grid_Descuentos.SelectedIndex > (-1)){
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
                        Cls_Cat_Pre_Descuentos_Recargos_Negocio Descuento_Recargos = new Cls_Cat_Pre_Descuentos_Recargos_Negocio();
                        Descuento_Recargos.P_Descuento_ID = Hdf_Descuento_ID.Value;
                        Descuento_Recargos.P_Anio = Convert.ToInt32(Txt_Anio.Text);
                        Descuento_Recargos.P_Enero = Convert.ToDouble(Txt_Enero.Text);
                        Descuento_Recargos.P_Febrero = Convert.ToDouble(Txt_Febrero.Text);
                        Descuento_Recargos.P_Marzo = Convert.ToDouble(Txt_Marzo.Text);
                        Descuento_Recargos.P_Abril = Convert.ToDouble(Txt_Abril.Text);
                        Descuento_Recargos.P_Mayo = Convert.ToDouble(Txt_Mayo.Text);
                        Descuento_Recargos.P_Junio = Convert.ToDouble(Txt_Junio.Text);
                        Descuento_Recargos.P_Julio = Convert.ToDouble(Txt_Julio.Text);
                        Descuento_Recargos.P_Agosto = Convert.ToDouble(Txt_Agosto.Text);
                        Descuento_Recargos.P_Septiembre = Convert.ToDouble(Txt_Septiembre.Text);
                        Descuento_Recargos.P_Octubre = Convert.ToDouble(Txt_Octubre.Text);
                        Descuento_Recargos.P_Noviembre = Convert.ToDouble(Txt_Noviembre.Text);
                        Descuento_Recargos.P_Diciembre = Convert.ToDouble(Txt_Diciembre.Text);
                        Descuento_Recargos.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Descuento_Recargos.Modificar_Descuento_Recargos();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Tabla_Descuentos(Grid_Descuentos.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Descuento Recargos", "alert('Actualización de Descuento Exitosa');", true);
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
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Descuento_Click
        ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 04/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Descuento_Click(object sender, ImageClickEventArgs e){
            try{
                Limpiar_Catalogo();
                Grid_Descuentos.SelectedIndex = (-1);
                Llenar_Tabla_Descuentos(0);
                if (Grid_Descuentos.Rows.Count == 0 && Txt_Busqueda_Descuento.Text.Trim().Length > 0) {
                    Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el año \"" + Txt_Busqueda_Descuento.Text + "\" no se encotrarón coincidencias";
                    Lbl_Mensaje_Error.Text = "(Se cargaron todos los descuentos de recargos almacenados)";
                    Div_Contenedor_Msj_Error.Visible = true;
                    Txt_Busqueda_Descuento.Text = "";
                    Llenar_Tabla_Descuentos(0);
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
        ///DESCRIPCIÓN: Elimina un Descuento de la Base de Datos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 04/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Eliminar_Click(object sender, EventArgs e){
            try{
                if (Grid_Descuentos.Rows.Count > 0 && Grid_Descuentos.SelectedIndex > (-1)){
                    Cls_Cat_Pre_Descuentos_Recargos_Negocio Descuento_Recargos = new Cls_Cat_Pre_Descuentos_Recargos_Negocio();
                    Descuento_Recargos.P_Descuento_ID = Grid_Descuentos.SelectedRow.Cells[1].Text;
                    Descuento_Recargos.Eliminar_Descuento_Recargo();
                    Grid_Descuentos.SelectedIndex = (-1);
                    Llenar_Tabla_Descuentos(Grid_Descuentos.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Descuentos Recargos", "alert('El Descuento fue eliminado exitosamente');", true);
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
        ///FECHA_CREO: 04/Septiembre/2010 
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