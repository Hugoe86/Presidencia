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
using Presidencia.Catalogo_Recargos.Negocio;

public partial class paginas_predial_Frm_Cat_Pre_Recargos : System.Web.UI.Page{
    
    #region Page_Load

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 07/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        protected void Page_Load(object sender, EventArgs e){
            if (!IsPostBack) {
                Configuracion_Formulario(true);
                Llenar_Tabla_Recargos(0);
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
        ///FECHA_CREO: 07/Septiembre/2010 
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
            Grid_Recargos_Generales.Enabled = estatus;
            Grid_Recargos_Generales.SelectedIndex = (-1);
            Txt_No_Bimestro.Enabled = !estatus;
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
            Btn_Agregar_Tasa.Visible = !estatus;
            Btn_Quitar_Tasa.Visible = !estatus;
            Btn_Modificar_Tasa.Visible = !estatus;
            Btn_Ver_Tasas.Visible = !estatus;
            Grid_Recargos_Tasas.SelectedIndex = (-1);
            Grid_Recargos_Tasas.Columns[1].Visible = false;
            Btn_Buscar_Recargo.Enabled = estatus;
            Txt_Busqueda_Recargo.Enabled = estatus;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
        ///DESCRIPCIÓN: Limpia los controles del Formulario
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 07/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Catalogo() {
            Txt_Identificador.Text = "";
            Cmb_Estatus.SelectedIndex = 0;
            Txt_Descripcion.Text = "";
            Txt_No_Bimestro.Text = "";
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
            Txt_ID_Recargo.Text = "";
            Grid_Recargos_Tasas.DataSource = new DataTable();
            Grid_Recargos_Tasas.DataBind();
            Hdf_Recargo_ID.Value = "";
            Hdf_Recargo_Tasa_ID.Value = "";
            Txt_Tasa_ID.Text = "";
        }

        #region Grids

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Recargos_Tasas
            ///DESCRIPCIÓN: Llena la tabla de Recargos Tasas.
            ///PROPIEDADES:     
            ///             1.  Pagina. Pagina en la cual se mostrará el Grid_VIew
            ///             2.  tabla.  Tabla que se va a cargar en el Grid.    
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 07/Septiembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Tabla_Recargos_Tasas(int Pagina, DataTable Tabla)
            {
                Grid_Recargos_Tasas.Columns[1].Visible = true;
                Grid_Recargos_Tasas.DataSource = Tabla;
                Grid_Recargos_Tasas.PageIndex = Pagina;
                Grid_Recargos_Tasas.DataBind();
                Grid_Recargos_Tasas.Columns[1].Visible = false;
                Session["Dt_Recargos_Tasas"] = Tabla;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Recargos
            ///DESCRIPCIÓN: Llena la tabla de Recargos con una consulta que puede o no
            ///             tener Filtros.
            ///PROPIEDADES:     
            ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 07/Septiembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Tabla_Recargos(int Pagina) {
                try{
                    Btn_Ver_Tasas.Visible = false;
                    Cls_Cat_Pre_Recargos_Negocio Recargo = new Cls_Cat_Pre_Recargos_Negocio();
                    Recargo.P_Identificador = Txt_Busqueda_Recargo.Text.Trim().ToUpper();
                    Grid_Recargos_Generales.DataSource = Recargo.Consultar_Recargos();
                    Grid_Recargos_Generales.PageIndex = Pagina;
                    Grid_Recargos_Generales.DataBind();
                }catch(Exception Ex){
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;                
                }
            }        

        #endregion

        #region Validaciones

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Generales
            ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
            ///             una operación en la pestaña de Recargos.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 08/Septiembre/2010 
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
            ///             una operación en la pestaña de Recargos - Tasas.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 08/Septiembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private bool Validar_Componentes_Tasas()
            {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                String Mensaje_Error = "";
                Boolean Validacion = true;
                if (Txt_No_Bimestro.Text.Trim().Length == 0)
                {
                    Mensaje_Error = Mensaje_Error + "+ Introducir el N&uacute;mero de Bimestro (Pestaña 2 de 2).";
                    Validacion = false;
                }
                if (Txt_Enero.Text.Trim().Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Tasa de Enero (Pestaña 2 de 2).";
                    Validacion = false;
                }
                if (Txt_Febrero.Text.Trim().Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Tasa de Febrero (Pestaña 2 de 2).";
                    Validacion = false;
                }
                if (Txt_Marzo.Text.Trim().Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Tasa de Marzo (Pestaña 2 de 2).";
                    Validacion = false;
                }
                if (Txt_Abril.Text.Trim().Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Tasa de Abril (Pestaña 2 de 2).";
                    Validacion = false;
                }
                if (Txt_Mayo.Text.Trim().Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Tasa de Mayo (Pestaña 2 de 2).";
                    Validacion = false;
                }
                if (Txt_Junio.Text.Trim().Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Tasa de Junio (Pestaña 2 de 2).";
                    Validacion = false;
                }
                if (Txt_Julio.Text.Trim().Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Tasa de Julio (Pestaña 2 de 2).";
                    Validacion = false;
                }
                if (Txt_Agosto.Text.Trim().Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Tasa de Agosto (Pestaña 2 de 2).";
                    Validacion = false;
                }
                if (Txt_Septiembre.Text.Trim().Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Tasa de Septiembre (Pestaña 2 de 2).";
                    Validacion = false;
                }
                if (Txt_Octubre.Text.Trim().Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Tasa de Octubre (Pestaña 2 de 2).";
                    Validacion = false;
                }
                if (Txt_Noviembre.Text.Trim().Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Tasa de Noviembre (Pestaña 2 de 2).";
                    Validacion = false;
                }
                if (Txt_Diciembre.Text.Trim().Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Tasa de Diciembre (Pestaña 2 de 2).";
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
        ///NOMBRE DE LA FUNCIÓN: Grid_Recargos_Generales_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView General de las Recargos 
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 07/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Recargos_Generales_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                Grid_Recargos_Generales.SelectedIndex = (-1);
                Llenar_Tabla_Recargos(e.NewPageIndex);
                Limpiar_Catalogo();
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Recargos_Generales_SelectedIndexChanged
        ///DESCRIPCIÓN: Obtiene los datos del Recargo Seleccionada para mostrarlos a detalle
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 07/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Recargos_Generales_SelectedIndexChanged(object sender, EventArgs e){
            try{
                if (Grid_Recargos_Generales.SelectedIndex > (-1)){
                    Limpiar_Catalogo();
                    Session["Dt_Recargos_Tasas"] = null;
                    String ID_Seleccionado = Grid_Recargos_Generales.SelectedRow.Cells[1].Text;
                    Cls_Cat_Pre_Recargos_Negocio Recargo = new Cls_Cat_Pre_Recargos_Negocio();
                    Recargo.P_Recargo_ID = ID_Seleccionado;
                    Recargo = Recargo.Consultar_Datos_Recargo();
                    Hdf_Recargo_ID.Value = Recargo.P_Recargo_ID;
                    Txt_ID_Recargo.Text = Recargo.P_Recargo_ID;
                    Txt_Identificador.Text = Recargo.P_Identificador;
                    Txt_Descripcion.Text = Recargo.P_Descripcion;
                    Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Recargo.P_Estatus));
                    Llenar_Tabla_Recargos_Tasas(0, Recargo.P_Recargos_Tasas);
                    if (Grid_Recargos_Tasas.Rows.Count > 0) { Btn_Ver_Tasas.Visible = true; } else { Btn_Ver_Tasas.Visible = false; }//AQUI
                    System.Threading.Thread.Sleep(1000);
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Recargos_Tasas_PageIndexChanging
        ///DESCRIPCIÓN: Maneja el Cambio de Pagina de la Tabla de Recargos Tasas
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 08/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Recargos_Tasas_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                if (Session["Dt_Recargos_Tasas"] != null){
                    DataTable tabla = (DataTable)Session["Dt_Recargos_Tasas"];
                    Llenar_Tabla_Recargos_Tasas(e.NewPageIndex, tabla);
                    Grid_Recargos_Tasas.SelectedIndex = (-1);
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Recargos_Tasas_SelectedIndexChanged
        ///DESCRIPCIÓN: Limpia los controles al cambiar de seleccion.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 08/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Recargos_Tasas_SelectedIndexChanged(object sender, EventArgs e)
        {
            Hdf_Recargo_Tasa_ID.Value = "";
            Txt_Tasa_ID.Text = "";
            Txt_No_Bimestro.Text = "";
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
        }

    #endregion

    #region Eventos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
        ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta un nuevo Recargo
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 07/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Nuevo_Click(object sender, EventArgs e){
            try{
                if (Btn_Nuevo.AlternateText.Equals("Nuevo")){
                    Configuracion_Formulario(false);
                    Limpiar_Catalogo();
                    Session["Dt_Recargos_Tasas"] = null;
                    Btn_Nuevo.AlternateText = "Dar de Alta";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.Visible = false;
                    Hdf_Recargo_Tasa_ID.Value = "";
                    Txt_Tasa_ID.Text = "";
                    Txt_No_Bimestro.Text = "";
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
                }else {
                    if (Validar_Componentes_Generales()){
                        Cls_Cat_Pre_Recargos_Negocio Recargo = new Cls_Cat_Pre_Recargos_Negocio();
                        Recargo.P_Identificador = Txt_Identificador.Text.ToUpper();
                        Recargo.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        Recargo.P_Descripcion = Txt_Descripcion.Text.ToUpper();
                        Recargo.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Recargo.P_Recargos_Tasas = (DataTable)Session["Dt_Recargos_Tasas"];
                        Recargo.Alta_Recargo();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Session["Dt_Recargos_Tasas"] = null;
                        Llenar_Tabla_Recargos(Grid_Recargos_Generales.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Recargos", "alert('Alta de Recargo Exitosa');", true);
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Grid_Recargos_Tasas.Enabled = true;
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
        ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de un Recargo
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 07/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Modificar_Click(object sender, EventArgs e){
            try{
                if (Btn_Modificar.AlternateText.Equals("Modificar")){
                    if (Grid_Recargos_Generales.Rows.Count > 0 && Grid_Recargos_Generales.SelectedIndex > (-1)){
                        Configuracion_Formulario(false);
                        Btn_Modificar.AlternateText = "Actualizar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                        Btn_Salir.AlternateText = "Cancelar";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        Btn_Nuevo.Visible = false;
                        Hdf_Recargo_Tasa_ID.Value = "";
                        Txt_Tasa_ID.Text = "";
                        Txt_No_Bimestro.Text = "";
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
                    }else{
                        Lbl_Ecabezado_Mensaje.Text = "Selecciona el Registro que quieres Modificar.";
                        Lbl_Mensaje_Error.Text = "";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                } else {
                    if (Validar_Componentes_Generales()){
                        Cls_Cat_Pre_Recargos_Negocio Recargo = new Cls_Cat_Pre_Recargos_Negocio();
                        Recargo.P_Recargo_ID = Hdf_Recargo_ID.Value;
                        Recargo.P_Identificador = Txt_Identificador.Text.ToUpper();
                        Recargo.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        Recargo.P_Descripcion = Txt_Descripcion.Text.ToUpper();
                        Recargo.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Recargo.P_Recargos_Tasas = (DataTable)Session["Dt_Recargos_Tasas"];
                        Recargo.Modificar_Recargo();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Tabla_Recargos(Grid_Recargos_Generales.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Recargas", "alert('Actualización de Recargo Exitosa');", true);
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Tab_Contenedor_Pestagnas.TabIndex = 0;
                        Grid_Recargos_Tasas.Enabled = true;      
                    }
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Recargo_Click
        ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 07/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Recargo_Click(object sender, ImageClickEventArgs e){
            try{
                Limpiar_Catalogo();
                Session["Dt_Recargos_Tasas"] = null;
                Grid_Recargos_Generales.SelectedIndex = (-1);
                Grid_Recargos_Tasas.SelectedIndex = (-1);
                Llenar_Tabla_Recargos(0);
                if (Grid_Recargos_Generales.Rows.Count == 0 && Txt_Busqueda_Recargo.Text.Trim().Length > 0) {
                    Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el nombre \"" + Txt_Busqueda_Recargo.Text + "\" no se encotrarón coincidencias";
                    Lbl_Mensaje_Error.Text = "(Se cargaron todos los recargos almacenados)";
                    Div_Contenedor_Msj_Error.Visible = true;
                    Txt_Busqueda_Recargo.Text = "";
                    Llenar_Tabla_Recargos(0);
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
        ///DESCRIPCIÓN: Elimina un Recargo de la Base de Datos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 08/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Eliminar_Click(object sender, EventArgs e){
            try{
                if (Grid_Recargos_Generales.Rows.Count > 0 && Grid_Recargos_Generales.SelectedIndex > (-1)){
                    Cls_Cat_Pre_Recargos_Negocio Recargo = new Cls_Cat_Pre_Recargos_Negocio();
                    Recargo.P_Recargo_ID = Grid_Recargos_Generales.SelectedRow.Cells[1].Text;
                    Recargo.Eliminar_Recargo();
                    Grid_Recargos_Generales.SelectedIndex = (-1);
                    Llenar_Tabla_Recargos(Grid_Recargos_Generales.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Recargos", "alert('El Recargo fue eliminad0 exitosamente');", true);
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
        ///FECHA_CREO: 08/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Salir_Click(object sender, EventArgs e){
            if (Btn_Salir.AlternateText.Equals("Salir")){
                Session.Remove("Dt_Recargos_Tasas");
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }else {
                Configuracion_Formulario(true);
                Limpiar_Catalogo();
                Tab_Contenedor_Pestagnas.TabIndex = 0;
                Session["Dt_Recargos_Tasas"] = null;
                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Grid_Recargos_Tasas.Enabled = true;
            }
        }
    
        #region Recargos - Tasas

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Tasa_Click
            ///DESCRIPCIÓN: Agrega una nueva tasa a la tabla de Recargos Tasas(Solo en Interfaz
            ///             no en la Base de Datos)
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 08/Septiembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Btn_Agregar_Tasa_Click(object sender, EventArgs e){
                try{
                    if (Validar_Componentes_Tasas()){
                        DataTable tabla = (DataTable)Grid_Recargos_Tasas.DataSource;
                        if (tabla == null){
                            if (Session["Dt_Recargos_Tasas"] == null){
                                tabla = new DataTable("rec_tas");
                                tabla.Columns.Add("RECARGO_TASA_ID", Type.GetType("System.String"));
                                tabla.Columns.Add("NO_BIMESTRO", Type.GetType("System.String"));
                                tabla.Columns.Add("ENERO", Type.GetType("System.String"));
                                tabla.Columns.Add("FEBRERO", Type.GetType("System.String"));
                                tabla.Columns.Add("MARZO", Type.GetType("System.String"));
                                tabla.Columns.Add("ABRIL", Type.GetType("System.String"));
                                tabla.Columns.Add("MAYO", Type.GetType("System.String"));
                                tabla.Columns.Add("JUNIO", Type.GetType("System.String"));
                                tabla.Columns.Add("JULIO", Type.GetType("System.String"));
                                tabla.Columns.Add("AGOSTO", Type.GetType("System.String"));
                                tabla.Columns.Add("SEPTIEMBRE", Type.GetType("System.String"));
                                tabla.Columns.Add("OCTUBRE", Type.GetType("System.String"));
                                tabla.Columns.Add("NOVIEMBRE", Type.GetType("System.String"));
                                tabla.Columns.Add("DICIEMBRE", Type.GetType("System.String"));
                            }else{
                                tabla = (DataTable)Session["Dt_Recargos_Tasas"];
                            }
                        }
                        DataRow fila = tabla.NewRow();
                        fila["RECARGO_TASA_ID"] = HttpUtility.HtmlDecode("");
                        fila["NO_BIMESTRO"] = HttpUtility.HtmlDecode(Txt_No_Bimestro.Text.Trim());
                        fila["ENERO"] = HttpUtility.HtmlDecode(Txt_Enero.Text.Trim());
                        fila["FEBRERO"] = HttpUtility.HtmlDecode(Txt_Febrero.Text.Trim());
                        fila["MARZO"] = HttpUtility.HtmlDecode(Txt_Marzo.Text.Trim());
                        fila["ABRIL"] = HttpUtility.HtmlDecode(Txt_Abril.Text.Trim());
                        fila["MAYO"] = HttpUtility.HtmlDecode(Txt_Mayo.Text.Trim());
                        fila["JUNIO"] = HttpUtility.HtmlDecode(Txt_Junio.Text.Trim());
                        fila["JULIO"] = HttpUtility.HtmlDecode(Txt_Julio.Text.Trim());
                        fila["AGOSTO"] = HttpUtility.HtmlDecode(Txt_Agosto.Text.Trim());
                        fila["SEPTIEMBRE"] = HttpUtility.HtmlDecode(Txt_Septiembre.Text.Trim());
                        fila["OCTUBRE"] = HttpUtility.HtmlDecode(Txt_Octubre.Text.Trim());
                        fila["NOVIEMBRE"] = HttpUtility.HtmlDecode(Txt_Noviembre.Text.Trim());
                        fila["DICIEMBRE"] = HttpUtility.HtmlDecode(Txt_Diciembre.Text.Trim());
                        tabla.Rows.Add(fila);
                        Grid_Recargos_Tasas.DataSource = tabla;
                        Session["Dt_Recargos_Tasas"] = tabla;
                        Grid_Recargos_Tasas.DataBind();
                        Grid_Recargos_Tasas.SelectedIndex = (-1);
                        Txt_Tasa_ID.Text = "";
                        Txt_No_Bimestro.Text = "";
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
                    }
                }catch(Exception Ex){
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;                
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Tasa_Click
            ///DESCRIPCIÓN: Modifica una tasa a la tabla de Recargos Tasas (Solo en Interfaz no
            ///             en la Base de Datos)
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 08/Septiembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Btn_Modificar_Tasa_Click(object sender, EventArgs e){
                try{
                    if (Btn_Modificar_Tasa.AlternateText.Equals("Modificar")){
                        if (Grid_Recargos_Tasas.Rows.Count > 0 && Grid_Recargos_Tasas.SelectedIndex > (-1)){
                            int registro = ((Grid_Recargos_Tasas.PageIndex) * Grid_Recargos_Tasas.PageSize) + (Grid_Recargos_Tasas.SelectedIndex);
                            if (Session["Dt_Recargos_Tasas"] != null) {
                                DataTable tabla = (DataTable)Session["Dt_Recargos_Tasas"];
                                Hdf_Recargo_Tasa_ID.Value = tabla.Rows[registro][0].ToString().Trim();
                                Txt_Tasa_ID.Text = tabla.Rows[registro][0].ToString().Trim();
                                Txt_No_Bimestro.Text = tabla.Rows[registro][1].ToString().Trim();
                                Txt_Enero.Text = Convert.ToDouble(tabla.Rows[registro][2].ToString().Trim()).ToString("#,###,###.00");
                                Txt_Febrero.Text = Convert.ToDouble(tabla.Rows[registro][3].ToString().Trim()).ToString("#,###,###.00");
                                Txt_Marzo.Text = Convert.ToDouble(tabla.Rows[registro][4].ToString().Trim()).ToString("#,###,###.00");
                                Txt_Abril.Text = Convert.ToDouble(tabla.Rows[registro][5].ToString().Trim()).ToString("#,###,###.00");
                                Txt_Mayo.Text = Convert.ToDouble(tabla.Rows[registro][6].ToString().Trim()).ToString("#,###,###.00");
                                Txt_Junio.Text = Convert.ToDouble(tabla.Rows[registro][7].ToString().Trim()).ToString("#,###,###.00");
                                Txt_Julio.Text = Convert.ToDouble(tabla.Rows[registro][8].ToString().Trim()).ToString("#,###,###.00");
                                Txt_Agosto.Text = Convert.ToDouble(tabla.Rows[registro][9].ToString().Trim()).ToString("#,###,###.00");
                                Txt_Septiembre.Text = Convert.ToDouble(tabla.Rows[registro][10].ToString().Trim()).ToString("#,###,###.00");
                                Txt_Octubre.Text = Convert.ToDouble(tabla.Rows[registro][11].ToString().Trim()).ToString("#,###,###.00");
                                Txt_Noviembre.Text = Convert.ToDouble(tabla.Rows[registro][12].ToString().Trim()).ToString("#,###,###.00");
                                Txt_Diciembre.Text = Convert.ToDouble(tabla.Rows[registro][13].ToString().Trim()).ToString("#,###,###.00");
                                Btn_Modificar_Tasa.AlternateText = "Actualizar";
                                Btn_Quitar_Tasa.Visible = false;
                                Btn_Agregar_Tasa.Visible = false;
                                Btn_Ver_Tasas.Visible = false;
                                Grid_Recargos_Tasas.Enabled = false;
                            }
                        }else{
                            Lbl_Ecabezado_Mensaje.Text = "Selecciona el Registro que quieres Modificar.";
                            Lbl_Mensaje_Error.Text = "";
                            Div_Contenedor_Msj_Error.Visible = true;
                        }
                    }else {
                        if (Validar_Componentes_Tasas()){
                            int registro = ((Grid_Recargos_Tasas.PageIndex) * Grid_Recargos_Tasas.PageSize) + (Grid_Recargos_Tasas.SelectedIndex);
                            if (Session["Dt_Recargos_Tasas"] != null){
                                DataTable tabla = (DataTable)Session["Dt_Recargos_Tasas"];
                                tabla.DefaultView.AllowEdit = true;
                                tabla.Rows[registro].BeginEdit();
                                tabla.Rows[registro][1] = Txt_No_Bimestro.Text.Trim();
                                tabla.Rows[registro][2] = Txt_Enero.Text.Trim();
                                tabla.Rows[registro][3] = Txt_Febrero.Text.Trim();
                                tabla.Rows[registro][4] = Txt_Marzo.Text.Trim();
                                tabla.Rows[registro][5] = Txt_Abril.Text.Trim();
                                tabla.Rows[registro][6] = Txt_Mayo.Text.Trim();
                                tabla.Rows[registro][7] = Txt_Junio.Text.Trim();
                                tabla.Rows[registro][8] = Txt_Julio.Text.Trim();
                                tabla.Rows[registro][9] = Txt_Agosto.Text.Trim();
                                tabla.Rows[registro][10] = Txt_Septiembre.Text.Trim();
                                tabla.Rows[registro][11] = Txt_Octubre.Text.Trim();
                                tabla.Rows[registro][12] = Txt_Noviembre.Text.Trim();
                                tabla.Rows[registro][13] = Txt_Diciembre.Text.Trim();
                                tabla.Rows[registro].EndEdit();
                                Session["Dt_Recargos_Tasas"] = tabla;
                                Llenar_Tabla_Recargos_Tasas(Grid_Recargos_Tasas.PageIndex, tabla);
                                Grid_Recargos_Tasas.SelectedIndex = (-1);
                                Btn_Modificar_Tasa.AlternateText = "Modificar";
                                Btn_Quitar_Tasa.Visible  = true;
                                Btn_Agregar_Tasa.Visible = true;
                                Btn_Ver_Tasas.Visible = true;
                                Tab_Contenedor_Pestagnas.TabIndex = 0;
                                Grid_Recargos_Tasas.Enabled = true;
                                Hdf_Recargo_Tasa_ID.Value = "";
                                Txt_Tasa_ID.Text = "";
                                Txt_No_Bimestro.Text = "";
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
            ///NOMBRE DE LA FUNCIÓN: Btn_Quitar_Tasa_Click
            ///DESCRIPCIÓN: Quita una tasas a la tabla de Recargos Tasas(Solo en Interfaz no 
            ///             en la Base de Datos)
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 08/Septiembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Btn_Quitar_Tasa_Click(object sender, EventArgs e){
                try{
                    if (Grid_Recargos_Tasas.Rows.Count > 0 && Grid_Recargos_Tasas.SelectedIndex > (-1)){
                        int registro = ((Grid_Recargos_Tasas.PageIndex) * Grid_Recargos_Tasas.PageSize) + (Grid_Recargos_Tasas.SelectedIndex);
                        if (Session["Dt_Recargos_Tasas"] != null){
                            DataTable tabla = (DataTable)Session["Dt_Recargos_Tasas"];
                            tabla.Rows.RemoveAt(registro);
                            Session["Dt_Recargos_Tasas"] = tabla;
                            Grid_Recargos_Tasas.SelectedIndex = (-1);
                            Llenar_Tabla_Recargos_Tasas(Grid_Recargos_Tasas.PageIndex, tabla);
                        }
                    }else{
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

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Ver_Tasas_Click
            ///DESCRIPCIÓN: Visualiza a detalle la tasa seleccionada.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 08/Septiembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Btn_Ver_Tasas_Click(object sender, EventArgs e){
                try{
                    if (Grid_Recargos_Tasas.Rows.Count > 0 && Grid_Recargos_Tasas.SelectedIndex > (-1)){
                        int registro = ((Grid_Recargos_Tasas.PageIndex) * Grid_Recargos_Tasas.PageSize) + (Grid_Recargos_Tasas.SelectedIndex);
                        if (Session["Dt_Recargos_Tasas"] != null){
                            DataTable tabla = (DataTable)Session["Dt_Recargos_Tasas"];
                            Hdf_Recargo_Tasa_ID.Value = tabla.Rows[registro][0].ToString().Trim();
                            Txt_Tasa_ID.Text = tabla.Rows[registro][0].ToString().Trim();
                            Txt_No_Bimestro.Text = tabla.Rows[registro][1].ToString().Trim();
                            Txt_Enero.Text = Convert.ToDouble(tabla.Rows[registro][2].ToString().Trim()).ToString("#,###,###.00");
                            Txt_Febrero.Text = Convert.ToDouble(tabla.Rows[registro][3].ToString().Trim()).ToString("#,###,###.00");
                            Txt_Marzo.Text = Convert.ToDouble(tabla.Rows[registro][4].ToString().Trim()).ToString("#,###,###.00");
                            Txt_Abril.Text = Convert.ToDouble(tabla.Rows[registro][5].ToString().Trim()).ToString("#,###,###.00");
                            Txt_Mayo.Text = Convert.ToDouble(tabla.Rows[registro][6].ToString().Trim()).ToString("#,###,###.00");
                            Txt_Junio.Text = Convert.ToDouble(tabla.Rows[registro][7].ToString().Trim()).ToString("#,###,###.00");
                            Txt_Julio.Text = Convert.ToDouble(tabla.Rows[registro][8].ToString().Trim()).ToString("#,###,###.00");
                            Txt_Agosto.Text = Convert.ToDouble(tabla.Rows[registro][9].ToString().Trim()).ToString("#,###,###.00");
                            Txt_Septiembre.Text = Convert.ToDouble(tabla.Rows[registro][10].ToString().Trim()).ToString("#,###,###.00");
                            Txt_Octubre.Text = Convert.ToDouble(tabla.Rows[registro][11].ToString().Trim()).ToString("#,###,###.00");
                            Txt_Noviembre.Text = Convert.ToDouble(tabla.Rows[registro][12].ToString().Trim()).ToString("#,###,###.00");
                            Txt_Diciembre.Text = Convert.ToDouble(tabla.Rows[registro][13].ToString().Trim()).ToString("#,###,###.00");
                        }
                    } else {
                        Lbl_Ecabezado_Mensaje.Text = "Selecciona el Registro que quieres ver a Detalle.";
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

    #endregion

}