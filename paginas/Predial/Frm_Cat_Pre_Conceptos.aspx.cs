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
using Presidencia.Catalogo_Conceptos.Negocio;
using Presidencia.Sessiones;

public partial class paginas_predial_Frm_Cat_Pre_Conceptos : System.Web.UI.Page{

    #region Page_Load

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 27/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************        
        protected void Page_Load(object sender, EventArgs e){
            if (!IsPostBack) {
                Configuracion_Formulario(true);
                Llenar_Tabla_Conceptos(0);
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
        ///FECHA_CREO: 27/Agosto/2010 
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
            Cmb_Tipo_Concepto.Enabled = !estatus;
            Cmb_Estatus.Enabled = !estatus;
            Txt_Descripcion.Enabled = !estatus;
            Grid_Conceptos_Generales.Enabled = estatus;
            Grid_Conceptos_Generales.SelectedIndex = (-1);
            Txt_Imp_Predial_Anio.Enabled = !estatus;
            Txt_Imp_Predial_Tasa.Enabled = !estatus;
            Txt_Imp_Traslacion_Anio.Enabled = !estatus;
            Txt_Imp_Traslacion_Tasa.Enabled = !estatus;
            Txt_Imp_Trasl_Deducible_Normal.Enabled = !estatus;
            Txt_Imp_Trasl_Deducible_Int_Social.Enabled = !estatus;
            Btn_Agregar_Impuesto_Predial.Visible = !estatus;
            Btn_Quitar_Impuesto_Predial.Visible = !estatus;
            Btn_Modificar_Impuesto_Predial.Visible = !estatus;
            Btn_Agregar_Impuesto_Traslacion.Visible = !estatus;
            Btn_Quitar_Impuesto_Traslacion.Visible = !estatus;
            Btn_Modificar_Impuesto_Traslacion.Visible = !estatus;
            Grid_Conceptos_Impuesto_Predial.SelectedIndex = (-1);
            Grid_Conceptos_Impuesto_Predial.Columns[1].Visible = false;
            Grid_Conceptos_Impuesto_Traslacion.Columns[1].Visible = false;
            Btn_Buscar_Concepto.Enabled = estatus;
            Txt_Busqueda_Concepto.Enabled = estatus;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
        ///DESCRIPCIÓN: Limpia los controles del Formulario
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 27/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Catalogo() {
            Txt_Identificador.Text = "";
            Cmb_Estatus.SelectedIndex = 0;
            Cmb_Tipo_Concepto.SelectedIndex = 0;
            Txt_Descripcion.Text = "";
            Txt_Imp_Predial_Anio.Text = "";
            Txt_Imp_Predial_Tasa.Text = "";
            Txt_ID_Concepto.Text = "";
            Grid_Conceptos_Impuesto_Predial.DataSource = new DataTable();
            Grid_Conceptos_Impuesto_Predial.DataBind();
            Grid_Conceptos_Impuesto_Traslacion.DataSource = new DataTable();
            Grid_Conceptos_Impuesto_Traslacion.DataBind();
            Hdf_Concepto_ID.Value = "";
            Hdf_Concepto_Impuesto_Predial_ID.Value = "";
        }

        #region Validaciones

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_General
            ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
            ///             una operación en la Vantana de Conceptos.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 02/Septiembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private bool Validar_Componentes_General()
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
                if (Cmb_Tipo_Concepto.SelectedIndex == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Tipo de Concepto.";
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

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Predial
            ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
            ///             una operación en la pestaña de Conceptos - Impuestos Predial.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 02/Septiembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private bool Validar_Componentes_Predial()
            {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                String Mensaje_Error = "";
                Boolean Validacion = true;
                if (Txt_Imp_Predial_Anio.Text.Trim().Length == 0)
                {
                    Mensaje_Error = Mensaje_Error + "+ Introducir el Año (Pestaña 2 de 3).";
                    Validacion = false;
                }
                if (Txt_Imp_Predial_Tasa.Text.Trim().Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Tasa (Pestaña 2 de 3).";
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
            ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Traslacion
            ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
            ///             una operación en la pestaña de Conceptos - Impuestos Traslacion.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 02/Septiembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private bool Validar_Componentes_Traslacion()
            {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                String Mensaje_Error = "";
                Boolean Validacion = true;
                if (Txt_Imp_Traslacion_Anio.Text.Trim().Length == 0)
                {
                    Mensaje_Error = Mensaje_Error + "+ Introducir el Año (Pestaña 3 de 3).";
                    Validacion = false;
                }
                if (Txt_Imp_Traslacion_Tasa.Text.Trim().Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Tasa (Pestaña 3 de 3).";
                    Validacion = false;
                }
                if (Txt_Imp_Trasl_Deducible_Normal.Text.Trim().Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir el Deducible Normal (Pestaña 3 de 3).";
                    Validacion = false;
                }
                if (Txt_Imp_Trasl_Deducible_Int_Social.Text.Trim().Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir el Deducible a Interes Social (Pestaña 3 de 3).";
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
    
        #region Grids

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Conceptos
            ///DESCRIPCIÓN: Llena la tabla de Conceptos con una consulta que puede o no
            ///             tener Filtros.
            ///PROPIEDADES:     
            ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 27/Agosto/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Tabla_Conceptos(int Pagina) {
                try{
                    Cls_Cat_Pre_Conceptos_Negocio concepto = new Cls_Cat_Pre_Conceptos_Negocio();
                    concepto.P_Identificador = Txt_Busqueda_Concepto.Text.Trim().ToUpper();
                    Grid_Conceptos_Generales.DataSource = concepto.Consultar_Conceptos();
                    Grid_Conceptos_Generales.PageIndex = Pagina;
                    Grid_Conceptos_Generales.DataBind();
                }catch(Exception Ex){
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;                
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Conceptos_Impuestos_Predial
            ///DESCRIPCIÓN: Llena la tabla de Conceptos Impuestos Predial.
            ///PROPIEDADES:     
            ///             1.  Pagina. Pagina en la cual se mostrará el Grid_VIew
            ///             2.  tabla.  Tabla que se va a cargar en el Grid.    
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 27/Agosto/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Tabla_Conceptos_Impuestos_Predial(int Pagina, DataTable Tabla){
                Grid_Conceptos_Impuesto_Predial.Columns[1].Visible = true;
                Grid_Conceptos_Impuesto_Predial.DataSource = Tabla;
                Grid_Conceptos_Impuesto_Predial.PageIndex = Pagina;
                Grid_Conceptos_Impuesto_Predial.DataBind();
                Grid_Conceptos_Impuesto_Predial.Columns[1].Visible = false;
                Session["Dt_Conceptos_Impuestos_Predial"] = Tabla;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Conceptos_Impuestos_Traslacion
            ///DESCRIPCIÓN: Llena la tabla de Conceptos Impuestos Traslacion.
            ///PROPIEDADES:     
            ///             1.  Pagina. Pagina en la cual se mostrará el Grid_VIew
            ///             2.  tabla.  Tabla que se va a cargar en el Grid.    
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 30/Agosto/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Tabla_Conceptos_Impuestos_Traslacion(int Pagina, DataTable Tabla)
            {
                Grid_Conceptos_Impuesto_Traslacion.Columns[1].Visible = true;
                Grid_Conceptos_Impuesto_Traslacion.DataSource = Tabla;
                Grid_Conceptos_Impuesto_Traslacion.PageIndex = Pagina;
                Grid_Conceptos_Impuesto_Traslacion.DataBind();
                Grid_Conceptos_Impuesto_Traslacion.Columns[1].Visible = false;
                Session["Dt_Conceptos_Impuestos_Traslacion"] = Tabla;
            }

        #endregion
    
    #endregion
    
    #region Grids
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Conceptos_Generales_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView General de los Conceptos 
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 27/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Conceptos_Generales_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            Grid_Conceptos_Generales.SelectedIndex = (-1);
            Llenar_Tabla_Conceptos(e.NewPageIndex);
            Limpiar_Catalogo();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Conceptos_Generales_SelectedIndexChanged
        ///DESCRIPCIÓN: Obtiene los datos del Concepto Seleccionado para mostrarlos a detalle
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 27/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Conceptos_Generales_SelectedIndexChanged(object sender, EventArgs e) {
            try{
                if (Grid_Conceptos_Generales.SelectedIndex > (-1)) {
                    Limpiar_Catalogo();
                    Session["Dt_Conceptos_Impuestos_Predial"] = null;
                    Session["Dt_Conceptos_Impuestos_Traslacion"] = null;
                    String id_Seleccionado = Grid_Conceptos_Generales.SelectedRow.Cells[1].Text;
                    Cls_Cat_Pre_Conceptos_Negocio concepto = new Cls_Cat_Pre_Conceptos_Negocio();
                    concepto.P_Concepto_Predial_ID = id_Seleccionado;
                    concepto = concepto.Consultar_Datos_Concepto();
                    Hdf_Concepto_ID.Value = concepto.P_Concepto_Predial_ID;
                    Txt_ID_Concepto.Text = concepto.P_Concepto_Predial_ID;
                    Txt_Identificador.Text = concepto.P_Identificador;
                    Txt_Descripcion.Text = concepto.P_Descripcion;
                    Cmb_Tipo_Concepto.SelectedIndex = Cmb_Tipo_Concepto.Items.IndexOf(Cmb_Tipo_Concepto.Items.FindByValue(concepto.P_Tipo_Concepto));
                    Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(concepto.P_Estatus));
                    Llenar_Tabla_Conceptos_Impuestos_Predial(0, concepto.P_Conceptos_Impuestos_Predial);
                    Llenar_Tabla_Conceptos_Impuestos_Traslacion(0, concepto.P_Conceptos_Impuestos_Traslacion);
                    System.Threading.Thread.Sleep(1000);
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Conceptos_Impuesto_Predial_PageIndexChanging
        ///DESCRIPCIÓN: Maneja el Cambio de Pagina de la Tabla de Conceptos Impuestos Predial
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 27/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Conceptos_Impuesto_Predial_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (Session["Dt_Conceptos_Impuestos_Predial"] != null)
            {
                DataTable tabla = (DataTable)Session["Dt_Conceptos_Impuestos_Predial"];
                Llenar_Tabla_Conceptos_Impuestos_Predial(e.NewPageIndex, tabla);
                Grid_Conceptos_Impuesto_Predial.SelectedIndex = (-1);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Grid_Conceptos_Impuesto_Predial_SelectedIndexChanged
        ///DESCRIPCIÓN          : Evento SelectedIndexChanged del grid Grid_Conceptos_Impuesto_Predial
        ///PARAMETROS           : sender y e
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 23/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Conceptos_Impuesto_Predial_SelectedIndexChanged(object sender, EventArgs e)
        {
            Txt_Impuesto_Predial_ID.Text = Grid_Conceptos_Impuesto_Predial.Rows[Grid_Conceptos_Impuesto_Predial.SelectedIndex].Cells[1].Text;
            Txt_Imp_Predial_Anio.Text = Grid_Conceptos_Impuesto_Predial.Rows[Grid_Conceptos_Impuesto_Predial.SelectedIndex].Cells[2].Text;
            Txt_Imp_Predial_Tasa.Text = Grid_Conceptos_Impuesto_Predial.Rows[Grid_Conceptos_Impuesto_Predial.SelectedIndex].Cells[3].Text;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Conceptos_Impuesto_Traslacion_PageIndexChanging
        ///DESCRIPCIÓN: Maneja el Cambio de Pagina de la Tabla de Conceptos Impuestos Traslacion
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 30/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Conceptos_Impuesto_Traslacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (Session["Dt_Conceptos_Impuestos_Traslacion"] != null)
            {
                DataTable tabla = (DataTable)Session["Dt_Conceptos_Impuestos_Traslacion"];
                Llenar_Tabla_Conceptos_Impuestos_Traslacion(e.NewPageIndex, tabla);
                Grid_Conceptos_Impuesto_Traslacion.SelectedIndex = (-1);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Grid_Conceptos_Impuesto_Traslacion_SelectedIndexChanged
        ///DESCRIPCIÓN          : Evento SelectedIndexChanged del control grid Grid_Conceptos_Impuesto_Traslacion
        ///PARAMETROS           : sender y e
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 23/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Conceptos_Impuesto_Traslacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            Txt_Imp_Traslacion_ID.Text=Grid_Conceptos_Impuesto_Traslacion.Rows[Grid_Conceptos_Impuesto_Traslacion.SelectedIndex].Cells[1].Text;
            Txt_Imp_Traslacion_Anio.Text = Grid_Conceptos_Impuesto_Traslacion.Rows[Grid_Conceptos_Impuesto_Traslacion.SelectedIndex].Cells[2].Text;
            Txt_Imp_Trasl_Deducible_Normal.Text = Grid_Conceptos_Impuesto_Traslacion.Rows[Grid_Conceptos_Impuesto_Traslacion.SelectedIndex].Cells[3].Text;
            Txt_Imp_Traslacion_Tasa.Text = Grid_Conceptos_Impuesto_Traslacion.Rows[Grid_Conceptos_Impuesto_Traslacion.SelectedIndex].Cells[4].Text;
            Txt_Imp_Trasl_Deducible_Int_Social.Text = Grid_Conceptos_Impuesto_Traslacion.Rows[Grid_Conceptos_Impuesto_Traslacion.SelectedIndex].Cells[5].Text;
        }

    #endregion

    #region Eventos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
        ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta un nuevo Conceptos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 27/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Nuevo_Click(object sender, EventArgs e){
            try{
                if (Btn_Nuevo.AlternateText.Equals("Nuevo")){
                    Configuracion_Formulario(false);
                    Limpiar_Catalogo();
                    Session["Dt_Conceptos_Impuestos_Predial"] = null;
                    Session["Dt_Conceptos_Impuestos_Traslacion"] = null;
                    Btn_Nuevo.AlternateText = "Dar de Alta";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.Visible = false;
                }else {
                    if ( Validar_Componentes_General() ){
                        Cls_Cat_Pre_Conceptos_Negocio concepto = new Cls_Cat_Pre_Conceptos_Negocio();
                        concepto.P_Identificador = Txt_Identificador.Text.ToUpper();
                        concepto.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        concepto.P_Tipo_Concepto = Cmb_Tipo_Concepto.SelectedItem.Value;
                        concepto.P_Descripcion = Txt_Descripcion.Text.ToUpper();
                        concepto.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        concepto.P_Conceptos_Impuestos_Predial = (DataTable)Session["Dt_Conceptos_Impuestos_Predial"];
                        concepto.P_Conceptos_Impuestos_Traslacion = (DataTable)Session["Dt_Conceptos_Impuestos_Traslacion"];
                        concepto.Alta_Concepto();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Session["Dt_Conceptos_Impuestos_Predial"] = null;
                        Session["Dt_Conceptos_Impuestos_Traslacion"] = null;
                        Llenar_Tabla_Conceptos(Grid_Conceptos_Generales.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Conceptos", "alert('Alta de Concepto Exitosa');", true);
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Grid_Conceptos_Impuesto_Predial.Enabled = true;
                        Grid_Conceptos_Impuesto_Traslacion.Enabled = true;
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
        ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de un Concepto
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 27/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Modificar_Click(object sender, EventArgs e){
            try{
                if (Btn_Modificar.AlternateText.Equals("Modificar")){
                    if (Grid_Conceptos_Generales.Rows.Count > 0 && Grid_Conceptos_Generales.SelectedIndex > (-1)){
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
                    if ( Validar_Componentes_General() ){
                        Cls_Cat_Pre_Conceptos_Negocio concepto = new Cls_Cat_Pre_Conceptos_Negocio();
                        concepto.P_Concepto_Predial_ID = Hdf_Concepto_ID.Value;
                        concepto.P_Identificador = Txt_Identificador.Text.ToUpper();
                        concepto.P_Tipo_Concepto = Cmb_Tipo_Concepto.SelectedItem.Value;
                        concepto.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        concepto.P_Descripcion = Txt_Descripcion.Text.ToUpper();
                        concepto.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        concepto.P_Conceptos_Impuestos_Predial = (DataTable)Session["Dt_Conceptos_Impuestos_Predial"];
                        concepto.P_Conceptos_Impuestos_Traslacion = (DataTable)Session["Dt_Conceptos_Impuestos_Traslacion"];
                        concepto.Modifcar_Concepto();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Tabla_Conceptos(Grid_Conceptos_Generales.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Conceptos", "alert('Actualización de Concepto Exitosa');", true);
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Grid_Conceptos_Impuesto_Predial.Enabled = true;
                        Grid_Conceptos_Impuesto_Traslacion.Enabled = true;
                    }
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Concepto_Click
        ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 27/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Concepto_Click(object sender, ImageClickEventArgs e){
            Limpiar_Catalogo();
            Session["Dt_Conceptos_Impuestos_Predial"] = null;
            Session["Dt_Conceptos_Impuestos_Traslacion"] = null;
            Grid_Conceptos_Generales.SelectedIndex = (-1);
            Grid_Conceptos_Impuesto_Predial.SelectedIndex = (-1);
            Llenar_Tabla_Conceptos(0);
            if (Grid_Conceptos_Generales.Rows.Count == 0 && Txt_Busqueda_Concepto.Text.Trim().Length > 0) {
                Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el Identificador \"" + Txt_Busqueda_Concepto.Text + "\" no se encotrarón coincidencias";
                Lbl_Mensaje_Error.Text = "(Se cargaron todos los conceptos almacenados)";
                Div_Contenedor_Msj_Error.Visible = true;
                Txt_Busqueda_Concepto.Text = "";
                Llenar_Tabla_Conceptos(0);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
        ///DESCRIPCIÓN: Elimina un concepto de la Base de Datos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 27/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Eliminar_Click(object sender, EventArgs e){
            try{
                if (Grid_Conceptos_Generales.Rows.Count > 0 && Grid_Conceptos_Generales.SelectedIndex > (-1)){
                    Cls_Cat_Pre_Conceptos_Negocio concepto = new Cls_Cat_Pre_Conceptos_Negocio();
                    concepto.P_Concepto_Predial_ID = Grid_Conceptos_Generales.SelectedRow.Cells[1].Text;
                    concepto.Eliminar_Concepto();
                    Grid_Conceptos_Generales.SelectedIndex = (-1);
                    Llenar_Tabla_Conceptos(Grid_Conceptos_Generales.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Conceptos", "alert('El Concepto fue eliminado exitosamente');", true);
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
        ///FECHA_CREO: 27/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Salir_Click(object sender, EventArgs e){
            if (Btn_Salir.AlternateText.Equals("Salir")){
                Session["Dt_Conceptos_Impuestos_Predial"] = null;
                Session["Dt_Conceptos_Impuestos_Traslacion"] = null;
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }else {
                Configuracion_Formulario(true);
                Limpiar_Catalogo();
                Tab_Contenedor_Pestagnas.TabIndex = 0;
                Session.Remove("Dt_Conceptos_Impuestos_Predial");
                Session.Remove("Dt_Conceptos_Impuestos_Traslacion");
                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Grid_Conceptos_Impuesto_Predial.Enabled = true;
                Grid_Conceptos_Impuesto_Traslacion.Enabled = true;
            }
        }

        #region Impuestos Predial

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Impuesto_Predial_Click
            ///DESCRIPCIÓN: Agrega un nuevo impuesto a la tabla de Conceptos Impuestos Predial(Solo en Interfaz no en la Base de Datos)
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 27/Agosto/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Btn_Agregar_Impuesto_Predial_Click(object sender, EventArgs e)
            {
                if (Validar_Componentes_Predial())
                {
                    DataTable tabla = (DataTable)Grid_Conceptos_Impuesto_Predial.DataSource;
                    if (tabla == null)
                    {
                        if (Session["Dt_Conceptos_Impuestos_Predial"] == null)
                        {
                            tabla = new DataTable("con_imp_pre");
                            tabla.Columns.Add("IMPUESTO_ID_PREDIAL", Type.GetType("System.String"));
                            tabla.Columns.Add("ANIO", Type.GetType("System.String"));
                            tabla.Columns.Add("TASA", Type.GetType("System.String"));
                        }
                        else
                        {
                            tabla = (DataTable)Session["Dt_Conceptos_Impuestos_Predial"];
                        }
                    }
                    DataRow fila = tabla.NewRow();
                    fila["IMPUESTO_ID_PREDIAL"] = HttpUtility.HtmlDecode("");
                    fila["ANIO"] = HttpUtility.HtmlDecode(Txt_Imp_Predial_Anio.Text.Trim());
                    fila["TASA"] = HttpUtility.HtmlDecode(Txt_Imp_Predial_Tasa.Text.Trim());
                    tabla.Rows.Add(fila);
                    Grid_Conceptos_Impuesto_Predial.DataSource = tabla;
                    Session["Dt_Conceptos_Impuestos_Predial"] = tabla;
                    Grid_Conceptos_Impuesto_Predial.DataBind();
                    Grid_Conceptos_Impuesto_Predial.SelectedIndex = (-1);
                    Txt_Imp_Predial_Anio.Text = "";
                    Txt_Imp_Predial_Tasa.Text = "";
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Impuesto_Predial_Click
            ///DESCRIPCIÓN: Modifica un impuesto a la tabla de Conceptos Impuestos Predial(Solo en Interfaz no en la Base de Datos)
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 27/Agosto/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Btn_Modificar_Impuesto_Predial_Click(object sender, EventArgs e)
            {
                if (Btn_Modificar_Impuesto_Predial.AlternateText.Equals("Modificar"))
                {
                    if (Grid_Conceptos_Impuesto_Predial.Rows.Count > 0 && Grid_Conceptos_Impuesto_Predial.SelectedIndex > (-1))
                    {
                        Hdf_Concepto_Impuesto_Predial_ID.Value = Grid_Conceptos_Impuesto_Predial.SelectedRow.Cells[1].Text.Trim();
                        Txt_Impuesto_Predial_ID.Text = Grid_Conceptos_Impuesto_Predial.SelectedRow.Cells[1].Text.Trim();
                        Txt_Imp_Predial_Anio.Text = Grid_Conceptos_Impuesto_Predial.SelectedRow.Cells[2].Text.Trim();
                        Txt_Imp_Predial_Tasa.Text = Convert.ToDouble(Grid_Conceptos_Impuesto_Predial.SelectedRow.Cells[3].Text.Trim()).ToString("#,###,###.00");
                        Btn_Modificar_Impuesto_Predial.AlternateText = "Actualizar";
                        Btn_Quitar_Impuesto_Predial.Visible = false;
                        Btn_Agregar_Impuesto_Predial.Visible = false;
                        Grid_Conceptos_Impuesto_Predial.Enabled = false;
                    }
                    else
                    {
                        Lbl_Ecabezado_Mensaje.Text = "Selecciona el Registro que quieres Modificar.";
                        Lbl_Mensaje_Error.Text = "";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                }
                else
                {
                    if (Validar_Componentes_Predial())
                    {
                        int registro = ((Grid_Conceptos_Impuesto_Predial.PageIndex) * Grid_Conceptos_Impuesto_Predial.PageSize) + (Grid_Conceptos_Impuesto_Predial.SelectedIndex);
                        if (Session["Dt_Conceptos_Impuestos_Predial"] != null)
                        {
                            DataTable tabla = (DataTable)Session["Dt_Conceptos_Impuestos_Predial"];
                            tabla.DefaultView.AllowEdit = true;
                            tabla.Rows[registro].BeginEdit();
                            tabla.Rows[registro][1] = Txt_Imp_Predial_Anio.Text.Trim();
                            tabla.Rows[registro][2] = Txt_Imp_Predial_Tasa.Text.Trim();
                            tabla.Rows[registro].EndEdit();
                            Session["Dt_Conceptos_Impuestos_Predial"] = tabla;
                            Llenar_Tabla_Conceptos_Impuestos_Predial(Grid_Conceptos_Impuesto_Predial.PageIndex, tabla);
                            Grid_Conceptos_Impuesto_Predial.SelectedIndex = (-1);
                            Btn_Modificar_Impuesto_Predial.AlternateText = "Modificar";
                            Btn_Quitar_Impuesto_Predial.Visible = true;
                            Btn_Agregar_Impuesto_Predial.Visible = true;
                            Tab_Contenedor_Pestagnas.TabIndex = 0;
                            Grid_Conceptos_Impuesto_Predial.Enabled = true;
                            Hdf_Concepto_Impuesto_Predial_ID.Value = "";
                            Txt_Impuesto_Predial_ID.Text = "";
                            Txt_Imp_Predial_Anio.Text = "";
                            Txt_Imp_Predial_Tasa.Text = "";
                        }
                    }
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Quitar_Impuesto_Predial_Click
            ///DESCRIPCIÓN: Quita un impuesto a la tabla de Conceptos Impuestos Predial(Solo en Interfaz no en la Base de Datos)
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 27/Agosto/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Btn_Quitar_Impuesto_Predial_Click(object sender, EventArgs e)
            {
                if (Grid_Conceptos_Impuesto_Predial.Rows.Count > 0 && Grid_Conceptos_Impuesto_Predial.SelectedIndex > (-1))
                {
                    int registro = ((Grid_Conceptos_Impuesto_Predial.PageIndex) * Grid_Conceptos_Impuesto_Predial.PageSize) + (Grid_Conceptos_Impuesto_Predial.SelectedIndex);
                    if (Session["Dt_Conceptos_Impuestos_Predial"] != null)
                    {
                        DataTable tabla = (DataTable)Session["Dt_Conceptos_Impuestos_Predial"];
                        tabla.Rows.RemoveAt(registro);
                        Session["Dt_Conceptos_Impuestos_Predial"] = tabla;
                        Grid_Conceptos_Impuesto_Predial.SelectedIndex = (-1);
                        Llenar_Tabla_Conceptos_Impuestos_Predial(Grid_Conceptos_Impuesto_Predial.PageIndex, tabla);
                    }
                }
                else
                {
                    Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Quitar.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

        #endregion

        #region Impuestos Traslacion

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Impuesto_Traslacion_Click
            ///DESCRIPCIÓN: Agrega un nuevo impuesto a la tabla de Conceptos Impuestos Traslacion(Solo en Interfaz no en la Base de Datos)
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 30/Agosto/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Btn_Agregar_Impuesto_Traslacion_Click(object sender, EventArgs e)
            {
                if (Validar_Componentes_Traslacion())
                {
                    DataTable tabla = (DataTable)Grid_Conceptos_Impuesto_Traslacion.DataSource;
                    if (tabla == null)
                    {
                        if (Session["Dt_Conceptos_Impuestos_Traslacion"] == null)
                        {
                            tabla = new DataTable("con_imp_tras");
                            tabla.Columns.Add("IMPUESTO_ID_TRASLACION", Type.GetType("System.String"));
                            tabla.Columns.Add("ANIO", Type.GetType("System.String"));
                            tabla.Columns.Add("TASA", Type.GetType("System.String"));
                            tabla.Columns.Add("DEDUCIBLE_NORMAL", Type.GetType("System.String"));
                            tabla.Columns.Add("DEDUCIBLE_INTERES_SOCIAL", Type.GetType("System.String"));
                        }
                        else
                        {
                            tabla = (DataTable)Session["Dt_Conceptos_Impuestos_Traslacion"];
                        }
                    }
                    DataRow fila = tabla.NewRow();
                    fila["IMPUESTO_ID_TRASLACION"] = HttpUtility.HtmlDecode("");
                    fila["ANIO"] = HttpUtility.HtmlDecode(Txt_Imp_Traslacion_Anio.Text.Trim());
                    fila["TASA"] = HttpUtility.HtmlDecode(Txt_Imp_Traslacion_Tasa.Text.Trim());
                    fila["DEDUCIBLE_NORMAL"] = HttpUtility.HtmlDecode(Txt_Imp_Trasl_Deducible_Normal.Text.Trim());
                    fila["DEDUCIBLE_INTERES_SOCIAL"] = HttpUtility.HtmlDecode(Txt_Imp_Trasl_Deducible_Int_Social.Text.Trim());
                    tabla.Rows.Add(fila);
                    Grid_Conceptos_Impuesto_Traslacion.DataSource = tabla;
                    Session["Dt_Conceptos_Impuestos_Traslacion"] = tabla;
                    Grid_Conceptos_Impuesto_Traslacion.DataBind();
                    Grid_Conceptos_Impuesto_Traslacion.SelectedIndex = (-1);
                    Txt_Imp_Traslacion_Anio.Text = "";
                    Txt_Imp_Traslacion_Tasa.Text = "";
                    Txt_Imp_Trasl_Deducible_Normal.Text = "";
                    Txt_Imp_Trasl_Deducible_Int_Social.Text = "";
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Impuesto_Traslacion_Click
            ///DESCRIPCIÓN: Modifica un impuesto a la tabla de Conceptos Impuestos Traslacion(Solo en Interfaz no en la Base de Datos)
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 30/Agosto/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Btn_Modificar_Impuesto_Traslacion_Click(object sender, EventArgs e)
            {
                if (Btn_Modificar_Impuesto_Traslacion.AlternateText.Equals("Modificar"))
                {
                    if (Grid_Conceptos_Impuesto_Traslacion.Rows.Count > 0 && Grid_Conceptos_Impuesto_Traslacion.SelectedIndex > (-1))
                    {
                        Hdf_Concepto_Impuesto_Traslacion_ID.Value = Grid_Conceptos_Impuesto_Traslacion.SelectedRow.Cells[1].Text.Trim();
                        Txt_Imp_Traslacion_ID.Text = Grid_Conceptos_Impuesto_Traslacion.SelectedRow.Cells[1].Text.Trim();
                        Txt_Imp_Traslacion_Anio.Text = Grid_Conceptos_Impuesto_Traslacion.SelectedRow.Cells[2].Text.Trim();
                        Txt_Imp_Traslacion_Tasa.Text = Convert.ToDouble(Grid_Conceptos_Impuesto_Traslacion.SelectedRow.Cells[3].Text.Trim()).ToString("#,###,###.00");
                        Txt_Imp_Trasl_Deducible_Normal.Text = Convert.ToDouble(Grid_Conceptos_Impuesto_Traslacion.SelectedRow.Cells[4].Text.Trim()).ToString("#,###,###.00");
                        Txt_Imp_Trasl_Deducible_Int_Social.Text = Convert.ToDouble(Grid_Conceptos_Impuesto_Traslacion.SelectedRow.Cells[5].Text.Trim()).ToString("#,###,###.00");
                        Btn_Modificar_Impuesto_Traslacion.AlternateText = "Actualizar";
                        Btn_Quitar_Impuesto_Traslacion.Visible = false;
                        Btn_Agregar_Impuesto_Traslacion.Visible = false;
                        Grid_Conceptos_Impuesto_Traslacion.Enabled = false;
                    }
                    else
                    {
                        Lbl_Ecabezado_Mensaje.Text = "Selecciona el Registro que quieres Modificar.";
                        Lbl_Mensaje_Error.Text = "";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                }
                else
                {
                    if (Validar_Componentes_Traslacion())
                    {
                        int registro = ((Grid_Conceptos_Impuesto_Traslacion.PageIndex) * Grid_Conceptos_Impuesto_Traslacion.PageSize) + (Grid_Conceptos_Impuesto_Traslacion.SelectedIndex);
                        if (Session["Dt_Conceptos_Impuestos_Traslacion"] != null)
                        {
                            DataTable tabla = (DataTable)Session["Dt_Conceptos_Impuestos_Traslacion"];
                            tabla.DefaultView.AllowEdit = true;
                            tabla.Rows[registro].BeginEdit();
                            tabla.Rows[registro][1] = Txt_Imp_Traslacion_Anio.Text.Trim();
                            tabla.Rows[registro][2] = Txt_Imp_Traslacion_Tasa.Text.Trim();
                            tabla.Rows[registro][3] = Txt_Imp_Trasl_Deducible_Normal.Text.Trim();
                            tabla.Rows[registro][4] = Txt_Imp_Trasl_Deducible_Int_Social.Text.Trim();
                            tabla.Rows[registro].EndEdit();
                            Session["Dt_Conceptos_Impuestos_Traslacion"] = tabla;
                            Llenar_Tabla_Conceptos_Impuestos_Traslacion(Grid_Conceptos_Impuesto_Traslacion.PageIndex, tabla);
                            Grid_Conceptos_Impuesto_Traslacion.SelectedIndex = (-1);
                            Btn_Modificar_Impuesto_Traslacion.AlternateText = "Modificar";
                            Btn_Quitar_Impuesto_Traslacion.Visible = true;
                            Btn_Agregar_Impuesto_Traslacion.Visible = true;
                            Grid_Conceptos_Impuesto_Traslacion.Enabled = true;
                            Hdf_Concepto_Impuesto_Traslacion_ID.Value = "";
                            Txt_Imp_Traslacion_ID.Text = "";
                            Txt_Imp_Traslacion_Anio.Text = "";
                            Txt_Imp_Traslacion_Tasa.Text = "";
                            Txt_Imp_Trasl_Deducible_Normal.Text = "";
                            Txt_Imp_Trasl_Deducible_Int_Social.Text = "";
                        }
                    }
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Quitar_Impuesto_Traslacion_Click
            ///DESCRIPCIÓN: Quita un impuesto a la tabla de Conceptos Impuestos Traslacion(Solo en Interfaz no en la Base de Datos)
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 30/Agosto/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Btn_Quitar_Impuesto_Traslacion_Click(object sender, EventArgs e)
            {
                if (Grid_Conceptos_Impuesto_Traslacion.Rows.Count > 0 && Grid_Conceptos_Impuesto_Traslacion.SelectedIndex > (-1))
                {
                    int registro = ((Grid_Conceptos_Impuesto_Traslacion.PageIndex) * Grid_Conceptos_Impuesto_Traslacion.PageSize) + (Grid_Conceptos_Impuesto_Traslacion.SelectedIndex);
                    if (Session["Dt_Conceptos_Impuestos_Traslacion"] != null)
                    {
                        DataTable tabla = (DataTable)Session["Dt_Conceptos_Impuestos_Traslacion"];
                        tabla.Rows.RemoveAt(registro);
                        Session["Dt_Conceptos_Impuestos_Traslacion"] = tabla;
                        Grid_Conceptos_Impuesto_Traslacion.SelectedIndex = (-1);
                        Llenar_Tabla_Conceptos_Impuestos_Traslacion(Grid_Conceptos_Impuesto_Traslacion.PageIndex, tabla);
                    }
                }
                else
                {
                    Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode("<br /> <b>+</b> <b style='color:red;'>Selecciona el Impuesto que quieres Quitar.</b>");
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

        #endregion

    #endregion
}