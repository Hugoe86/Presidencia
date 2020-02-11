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
using Presidencia.Catalogo_Fraccionamientos.Negocio;
using Presidencia.Sessiones;

public partial class paginas_predial_Frm_Cat_Pre_Fraccionamientos : System.Web.UI.Page{

    #region Page_Load

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 20/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        protected void Page_Load(object sender, EventArgs e){
            if (!IsPostBack) {
                Configuracion_Formulario(true);
                Llenar_Tabla_Fraccionamientos(0);
                Tab_Contenedor_Pestagnas.ActiveTabIndex = 0;
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
        ///FECHA_CREO: 20/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Configuracion_Formulario(Boolean estatus)
        {
            String Opcion_Boton = "";
            if (Session["Opcion_Boton"] != null)
            {
                Opcion_Boton = Convert.ToString(Session["Opcion_Boton"]);
            }
            switch (Opcion_Boton)
            {
                case "Nuevo":
                    Btn_Nuevo.Visible = true;
                    Btn_Nuevo.AlternateText = "Dar de Alta";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Modificar.Visible = false;
                    Btn_Eliminar.Visible = false;
                    Btn_Salir.Visible = true;
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Agregar_Impuesto.Visible = true;
                    Btn_Quitar_Impuesto.Visible = true;
                    Btn_Modificar_Impuesto.Visible = true;
                    break;
                case "Modificar":
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Eliminar.Visible = false;
                    Btn_Salir.Visible = true;
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Agregar_Impuesto.Visible = true;
                    Btn_Quitar_Impuesto.Visible = true;
                    Btn_Modificar_Impuesto.Visible = true;
                    break;
                case "Eliminar":
                    break;
                case "Cancelar":
                    Btn_Nuevo.Visible = true;
                    Btn_Nuevo.AlternateText = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.Visible = true;
                    Btn_Modificar.AlternateText = "Modificar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Eliminar.Visible = true;
                    Btn_Salir.Visible = true;
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Agregar_Impuesto.Visible = false;
                    Btn_Quitar_Impuesto.Visible = false;
                    Btn_Modificar_Impuesto.Visible = false;
                    break;
                default:
                    Btn_Agregar_Impuesto.Visible = false;
                    Btn_Quitar_Impuesto.Visible = false;
                    Btn_Modificar_Impuesto.Visible = false;
                    break;
            }

            Txt_Identificador.Enabled = !estatus;
            Cmb_Estatus.Enabled = !estatus;
            Txt_Descripcion.Enabled = !estatus;
            Grid_Fraccionamientos_Generales.Enabled = estatus;
            Grid_Fraccionamientos_Generales.SelectedIndex = (-1);

            switch (Opcion_Boton)
            {
                case "Nuevo":
                    Txt_Anio.Enabled = true;
                    Txt_Monto.Enabled = true;
                    break;
                case "Modificar":
                    Txt_Anio.Enabled = true;
                    Txt_Monto.Enabled = true;
                    break;
                case "Cancelar":
                    Txt_Anio.Enabled = false;
                    Txt_Monto.Enabled = false;
                    break;
            }
            Grid_Fraccionamientos_Impuesto.SelectedIndex = (-1);
            Grid_Fraccionamientos_Impuesto.Columns[1].Visible = false;

            Btn_Buscar_Fraccionamiento.Enabled = estatus;
            Txt_Busqueda_Fraccionamiento.Enabled = estatus;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
        ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
        ///PARAMETROS:     
        ///             1. estatus.    Estatus en el que se cargara la configuración de los
        ///                             controles.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 20/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Configurar_Botones(Boolean estatus)
        {
            String Opcion_Boton = "";
            if (Session["Opcion_Boton"] != null)
            {
                Opcion_Boton = Convert.ToString(Session["Opcion_Boton"]);
            }
            switch (Opcion_Boton)
            {
                case "Nuevo":
                    Btn_Nuevo.Visible = true;
                    Btn_Nuevo.AlternateText = "Dar de Alta";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Modificar.Visible = false;
                    Btn_Eliminar.Visible = false;
                    Btn_Salir.Visible = true;
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Agregar_Impuesto.Visible = true;
                    Btn_Quitar_Impuesto.Visible = true;
                    Btn_Modificar_Impuesto.Visible = true;
                    break;
                case "Modificar":
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Eliminar.Visible = false;
                    Btn_Salir.Visible = true;
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Agregar_Impuesto.Visible = true;
                    Btn_Quitar_Impuesto.Visible = true;
                    Btn_Modificar_Impuesto.Visible = true;
                    break;
                case "Eliminar":
                    break;
                case "Cancelar":
                    Btn_Nuevo.Visible = true;
                    Btn_Nuevo.AlternateText = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.Visible = true;
                    Btn_Modificar.AlternateText = "Modificar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Eliminar.Visible = true;
                    Btn_Salir.Visible = true;
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Agregar_Impuesto.Visible = false;
                    Btn_Quitar_Impuesto.Visible = false;
                    Btn_Modificar_Impuesto.Visible = false;
                    break;
                default:
                    Btn_Agregar_Impuesto.Visible = false;
                    Btn_Quitar_Impuesto.Visible = false;
                    Btn_Modificar_Impuesto.Visible = false;
                    break;
            }

            Txt_Identificador.Enabled = !estatus;
            Cmb_Estatus.Enabled = !estatus;
            Txt_Descripcion.Enabled = !estatus;
            Grid_Fraccionamientos_Generales.Enabled = estatus;
            Grid_Fraccionamientos_Generales.SelectedIndex = (-1);

            switch (Opcion_Boton)
            {
                case "Nuevo":
                    Txt_Anio.Enabled = true;
                    Txt_Monto.Enabled = true;
                    break;
                case "Modificar":
                    Txt_Anio.Enabled = true;
                    Txt_Monto.Enabled = true;
                    break;
                case "Cancelar":
                    Txt_Anio.Enabled = false;
                    Txt_Monto.Enabled = false;
                    break;
            }
            Grid_Fraccionamientos_Impuesto.SelectedIndex = (-1);
            Grid_Fraccionamientos_Impuesto.Columns[1].Visible = false;

            Btn_Buscar_Fraccionamiento.Enabled = estatus;
            Txt_Busqueda_Fraccionamiento.Enabled = estatus;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
        ///DESCRIPCIÓN: Limpia los controles del Formulario
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Catalogo() {
            Txt_Identificador.Text = "";
            Cmb_Estatus.SelectedIndex = 0;
            Txt_Descripcion.Text = "";
            Txt_Anio.Text = "";
            Txt_Monto.Text = "";
            Txt_Impuesto_ID.Text = "";
            Txt_ID_Fraccionamiento.Text = "";
            Grid_Fraccionamientos_Impuesto.DataSource = new DataTable();
            Grid_Fraccionamientos_Impuesto.DataBind();
            Hdf_Fraccionamiento_ID.Value = "";
            Hdf_Fraccionamiento_Impuesto_ID.Value = "";
        }

        #region Validaciones

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Generales
            ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
            ///             una operación de la Pestaña de Fraccionamientos.
            ///PARAMETROS:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 03/Septiembre/2010 
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

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Impuestos
            ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
            ///             una operación de la Pestaña de Fraccionamientos - Impuestos.
            ///PARAMETROS:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 03/Septiembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private bool Validar_Componentes_Impuestos()
            {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                String Mensaje_Error = "";
                Boolean Validacion = true;
                decimal Monto_Impuesto;
                if (Txt_Anio.Text.Trim().Length == 0)
                {
                    Mensaje_Error = Mensaje_Error + "+ Introducir el Año.";
                    Validacion = false;
                }
                if (!decimal.TryParse(Txt_Monto.Text.Trim(), out Monto_Impuesto) || Monto_Impuesto <= 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir el Monto.";
                    Validacion = false;
                }
                Txt_Monto.Text = Monto_Impuesto.ToString("0.00");
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
            ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Fraccionamientos_Impuestos
            ///DESCRIPCIÓN: Llena la tabla de Fraccionacionamientos Impuestos
            ///PARAMETROS:     
            ///             1.  Pagina. Pagina en la cual se mostrará el Grid_VIew
            ///             2.  tabla.  Tabla que se va a cargar en el Grid.    
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 23/Agosto/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Tabla_Fraccionamientos_Impuestos(int Pagina, DataTable Tabla) {
                Grid_Fraccionamientos_Impuesto.Columns[1].Visible = true;
                Tabla.DefaultView.Sort = "ANIO DESC, MONTO DESC";
                Grid_Fraccionamientos_Impuesto.DataSource = Tabla;
                Grid_Fraccionamientos_Impuesto.PageIndex = Pagina;
                Grid_Fraccionamientos_Impuesto.DataBind();
                Grid_Fraccionamientos_Impuesto.Columns[1].Visible = false;
                Session["Dt_Fraccionamientos_Impuestos"] = Tabla;
            }
    
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Fraccionamientos
            ///DESCRIPCIÓN: Llena la tabla de Fraccionacionamientos
            ///PARAMETROS:     
            ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 21/Agosto/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Tabla_Fraccionamientos(int Pagina) {
                try{
                    Cls_Cat_Pre_Fraccionamientos_Negocio fraccionamientos = new Cls_Cat_Pre_Fraccionamientos_Negocio();
                    fraccionamientos.P_Identificador = Txt_Busqueda_Fraccionamiento.Text.Trim().ToUpper();
                    Grid_Fraccionamientos_Generales.DataSource = fraccionamientos.Consultar_Fraccionamientos();
                    Grid_Fraccionamientos_Generales.PageIndex = Pagina;
                    Grid_Fraccionamientos_Generales.DataBind();
                }catch(Exception Ex){
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;                
                }
            }

        #endregion

    #endregion

    #region Grid
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Fraccionamientos_Generales_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView General de los Fraccionamientos 
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Fraccionamientos_Generales_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                Grid_Fraccionamientos_Generales.SelectedIndex = (-1);
                Llenar_Tabla_Fraccionamientos(e.NewPageIndex);
                Limpiar_Catalogo();
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Fraccionamientos_Generales_SelectedIndexChanged
        ///DESCRIPCIÓN: Obtiene los datos del Fraccionamiento Seleccionado para mostrarlos a detalle
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Fraccionamientos_Generales_SelectedIndexChanged(object sender, EventArgs e){
            try{
                if (Grid_Fraccionamientos_Generales.SelectedIndex > (-1)){
                    Limpiar_Catalogo();
                    Session["Dt_Fraccionamientos_Impuestos"] = null;
                    String id_Seleccionado = Grid_Fraccionamientos_Generales.SelectedRow.Cells[1].Text;
                    Cls_Cat_Pre_Fraccionamientos_Negocio fraccionamiento = new Cls_Cat_Pre_Fraccionamientos_Negocio();
                    fraccionamiento.P_Fraccionamiento_ID = id_Seleccionado;
                    fraccionamiento = fraccionamiento.Consultar_Datos_Fraccionamiento();
                    Hdf_Fraccionamiento_ID.Value = fraccionamiento.P_Fraccionamiento_ID;
                    Txt_ID_Fraccionamiento.Text = fraccionamiento.P_Fraccionamiento_ID;
                    Txt_Identificador.Text = fraccionamiento.P_Identificador;
                    Txt_Descripcion.Text = fraccionamiento.P_Descripcion;
                    Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(fraccionamiento.P_Estatus));
                    Llenar_Tabla_Fraccionamientos_Impuestos(0, fraccionamiento.P_Fraccionamientos_Impuestos);
                    System.Threading.Thread.Sleep(1000);
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Fraccionamientos_Impuesto_PageIndexChanging
        ///DESCRIPCIÓN: Maneja el Cambio de Pagina de la Tabla de Fraccionamiento Impuesto
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Fraccionamientos_Impuesto_PageIndexChanging(object sender, GridViewPageEventArgs e){
            if (Session["Dt_Fraccionamientos_Impuestos"] != null){
                DataTable tabla = (DataTable)Session["Dt_Fraccionamientos_Impuestos"];
                Llenar_Tabla_Fraccionamientos_Impuestos(e.NewPageIndex, tabla);
                Grid_Fraccionamientos_Impuesto.SelectedIndex = (-1);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Grid_Fraccionamientos_Impuesto_SelectedIndexChanged
        ///DESCRIPCIÓN          : Evento SelectedIndexChanged del control grid Grid_Fraccionamientos_Impuesto_SelectedIndexChanged
        ///PARAMETROS           : sender y e
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 23/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Fraccionamientos_Impuesto_SelectedIndexChanged(object sender, EventArgs e)
        {
            Txt_Impuesto_ID.Text = HttpUtility.HtmlDecode(Grid_Fraccionamientos_Impuesto.Rows[Grid_Fraccionamientos_Impuesto.SelectedIndex].Cells[1].Text);
            Txt_Anio.Text = Grid_Fraccionamientos_Impuesto.Rows[Grid_Fraccionamientos_Impuesto.SelectedIndex].Cells[2].Text;
            Txt_Monto.Text = Grid_Fraccionamientos_Impuesto.Rows[Grid_Fraccionamientos_Impuesto.SelectedIndex].Cells[3].Text;
        }

    #endregion

    #region Eventos
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
        ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta un nuevo Fraccionamiento
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 20/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Nuevo_Click(object sender, EventArgs e){
            try{
                if (Btn_Nuevo.AlternateText.Equals("Nuevo")) {
                    Session["Dt_Fraccionamientos_Impuestos"] = null;
                    Session["Opcion_Boton"] = "Nuevo";
                    Configuracion_Formulario(false);
                    Limpiar_Catalogo();
                    Btn_Modificar.Visible = false;
                }else {
                    if (Validar_Componentes_Generales()){
                        Cls_Cat_Pre_Fraccionamientos_Negocio fraccionamientos = new Cls_Cat_Pre_Fraccionamientos_Negocio();
                        fraccionamientos.P_Identificador = Txt_Identificador.Text.ToUpper();
                        fraccionamientos.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        fraccionamientos.P_Descripcion = Txt_Descripcion.Text.ToUpper();
                        fraccionamientos.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        fraccionamientos.P_Fraccionamientos_Impuestos = (DataTable)Session["Dt_Fraccionamientos_Impuestos"];
                        fraccionamientos.Alta_Fraccionamiento();
                        Session["Opcion_Boton"] = "Cancelar";
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Session["Dt_Fraccionamientos_Impuestos"] = null;
                        Llenar_Tabla_Fraccionamientos(Grid_Fraccionamientos_Generales.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Fraccionamientos", "alert('Alta de Fraccionamiento Exitosa');", true);
                        Grid_Fraccionamientos_Impuesto.Enabled = true;
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
        ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de un Fraccionamiento
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 20/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Modificar_Click(object sender, EventArgs e){
            try{
                if (Btn_Modificar.AlternateText.Equals("Modificar")){
                    if (Grid_Fraccionamientos_Generales.Rows.Count > 0 && Grid_Fraccionamientos_Generales.SelectedIndex > (-1)){
                        Session["Opcion_Boton"] = "Modificar";
                        Configuracion_Formulario(false);
                    }else{
                        Lbl_Ecabezado_Mensaje.Text = "Selecciona el Registro que quieres Modificar.";
                        Lbl_Mensaje_Error.Text = "";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                } else {
                    if (Validar_Componentes_Generales()){
                        Cls_Cat_Pre_Fraccionamientos_Negocio fraccionamientos = new Cls_Cat_Pre_Fraccionamientos_Negocio();
                        fraccionamientos.P_Fraccionamiento_ID = Hdf_Fraccionamiento_ID.Value;
                        fraccionamientos.P_Identificador = Txt_Identificador.Text.ToUpper();
                        fraccionamientos.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        fraccionamientos.P_Descripcion = Txt_Descripcion.Text.ToUpper();
                        fraccionamientos.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        fraccionamientos.P_Fraccionamientos_Impuestos = (DataTable)Session["Dt_Fraccionamientos_Impuestos"];
                        fraccionamientos.Modificar_Fraccionamiento();
                        Session["Opcion_Boton"] = "Cancelar";
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Tabla_Fraccionamientos(Grid_Fraccionamientos_Generales.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Fraccionamientos", "alert('Actualización de Fraccionamiento Exitosa');", true);
                        Tab_Contenedor_Pestagnas.TabIndex = 0;
                        Grid_Fraccionamientos_Impuesto.Enabled = true;
                    }
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Fraccionamiento_Click
        ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Fraccionamiento_Click(object sender, ImageClickEventArgs e){
            try{
                Limpiar_Catalogo();
                Session["Dt_Fraccionamientos_Impuestos"] = null;
                Grid_Fraccionamientos_Generales.SelectedIndex = (-1);
                Grid_Fraccionamientos_Impuesto.SelectedIndex = (-1);
                Llenar_Tabla_Fraccionamientos(0);
                if (Grid_Fraccionamientos_Generales.Rows.Count == 0 && Txt_Busqueda_Fraccionamiento.Text.Trim().Length > 0) {
                    Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el identificador \"" + Txt_Busqueda_Fraccionamiento.Text + "\" no se encotrarón coincidencias";
                    Lbl_Mensaje_Error.Text = "(Se cargaron todos los fraccionamientos almacenados)";
                    Div_Contenedor_Msj_Error.Visible = true;
                    Txt_Busqueda_Fraccionamiento.Text = "";
                    Llenar_Tabla_Fraccionamientos(0);
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
        ///DESCRIPCIÓN: Elimina un fraccionamiento de la Base de Datos
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Eliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Grid_Fraccionamientos_Generales.Rows.Count > 0 && Grid_Fraccionamientos_Generales.SelectedIndex > (-1))
                {
                    Cls_Cat_Pre_Fraccionamientos_Negocio fraccionamientos = new Cls_Cat_Pre_Fraccionamientos_Negocio();
                    fraccionamientos.P_Fraccionamiento_ID = Grid_Fraccionamientos_Generales.SelectedRow.Cells[1].Text;
                    fraccionamientos.P_Estatus = "BAJA";
                    fraccionamientos.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                    fraccionamientos.Eliminar_Fraccionamiento();
                    Grid_Fraccionamientos_Generales.SelectedIndex = (-1);
                    Llenar_Tabla_Fraccionamientos(Grid_Fraccionamientos_Generales.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Fraccionamientos", "alert('El Fraccionamiento fue actualizado exitosamente');", true);
                    Tab_Contenedor_Pestagnas.TabIndex = 0;
                    Limpiar_Catalogo();
                }
                else
                {
                    Lbl_Ecabezado_Mensaje.Text = "Selecciona el Registro que quieres Eliminar.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            catch (Exception Ex)
            {
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
        ///FECHA_CREO: 23/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
        {
            if (Btn_Salir.AlternateText.Equals("Salir")){
                Session["Dt_Fraccionamientos_Impuestos"] = null;
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }else {
                Session["Opcion_Boton"] = "Cancelar";
                Configuracion_Formulario(true);
                Limpiar_Catalogo();
                Tab_Contenedor_Pestagnas.TabIndex = 0;
                Session.Remove("Dt_Fraccionamientos_Impuestos");
                Grid_Fraccionamientos_Impuesto.Enabled = true;
            }
        }

        #region Impuestos
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Impuesto_Click
        ///DESCRIPCIÓN: Agrega un nuevo impuesto a la tabla de Fraccionamientos Impuestos(Solo en Interfaz no en la Base de Datos)
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Agregar_Impuesto_Click(object sender, EventArgs e)
        {
            try
            {
                if (Validar_Componentes_Impuestos())
                {
                    DataTable tabla = (DataTable)Session["Dt_Fraccionamientos_Impuestos"];
                    if (tabla == null)
                    {
                        if (Session["Dt_Fraccionamientos_Impuestos"] == null)
                        {
                            tabla = new DataTable("fracc_imp");
                            tabla.Columns.Add("IMPUESTO_FRACCIONAMIENTO_ID", Type.GetType("System.String"));
                            tabla.Columns.Add("ANIO", Type.GetType("System.String"));
                            tabla.Columns.Add("MONTO", Type.GetType("System.String"));
                        }
                        else
                        {
                            tabla = (DataTable)Session["Dt_Fraccionamientos_Impuestos"];
                        }
                    }
                    // validar que el año no esté ya en la tabla
                    foreach (DataRow Impuesto in tabla.Rows)
                    {
                        int Anio_Impuesto;
                        int Anio_Agregar;
                        int.TryParse(Impuesto["ANIO"].ToString(), out Anio_Impuesto);
                        int.TryParse(HttpUtility.HtmlDecode(Txt_Anio.Text.Trim()), out Anio_Agregar);
                        if (Anio_Impuesto == Anio_Agregar)
                        {
                            Lbl_Ecabezado_Mensaje.Text = "El impuesto ya tiene un valor para el año " + Anio_Agregar;
                            Lbl_Mensaje_Error.Text = "";
                            Div_Contenedor_Msj_Error.Visible = true;
                            return;
                        }
                    }
                    // agregar registro a la tabla
                    foreach (DataRow Dr_Registro in tabla.Rows)
                    {
                        if (Dr_Registro["ANIO"].ToString() == Txt_Anio.Text.Trim() && Convert.ToDouble(Dr_Registro["MONTO"].ToString()) == Convert.ToDouble(Txt_Monto.Text.Trim()))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Fraccionamientos", "alert('Año y monto ya existentes. Registro no agregado.');", true);
                            return;
                        }
                    }
                    DataRow fila = tabla.NewRow();
                    fila["IMPUESTO_FRACCIONAMIENTO_ID"] = HttpUtility.HtmlDecode("");
                    fila["ANIO"] = HttpUtility.HtmlDecode(Txt_Anio.Text.Trim());
                    fila["MONTO"] = HttpUtility.HtmlDecode(Txt_Monto.Text.Trim());
                    tabla.Rows.Add(fila);
                    // cargar en grid
                    Grid_Fraccionamientos_Impuesto.Columns[1].Visible = true;
                    tabla.DefaultView.Sort = "ANIO DESC, MONTO DESC";
                    Grid_Fraccionamientos_Impuesto.DataSource = tabla;
                    Session["Dt_Fraccionamientos_Impuestos"] = tabla;
                    Grid_Fraccionamientos_Impuesto.DataBind();
                    Grid_Fraccionamientos_Impuesto.SelectedIndex = (-1);
                    Grid_Fraccionamientos_Impuesto.Columns[1].Visible = false;
                    Txt_Anio.Text = "";
                    Txt_Monto.Text = "";
                    Grid_Fraccionamientos_Impuesto.Enabled = true;
                }
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Quitar_Impuesto_Click
        ///DESCRIPCIÓN: Quita un impuesto a la tabla de Fraccionamientos Impuestos(Solo en Interfaz no en la Base de Datos)
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Quitar_Impuesto_Click(object sender, EventArgs e)
        {
            try
            {
                if (Grid_Fraccionamientos_Impuesto.Rows.Count > 0 && Grid_Fraccionamientos_Impuesto.SelectedIndex > (-1))
                {
                    int registro = ((Grid_Fraccionamientos_Impuesto.PageIndex) * Grid_Fraccionamientos_Impuesto.PageSize) + (Grid_Fraccionamientos_Impuesto.SelectedIndex);
                    if (Session["Dt_Fraccionamientos_Impuestos"] != null)
                    {
                        DataTable tabla = (DataTable)Session["Dt_Fraccionamientos_Impuestos"];
                        tabla.Rows.RemoveAt(registro);
                        tabla.DefaultView.Sort = "ANIO DESC, MONTO DESC";
                        Session["Dt_Fraccionamientos_Impuestos"] = tabla;
                        Grid_Fraccionamientos_Impuesto.SelectedIndex = (-1);
                        Llenar_Tabla_Fraccionamientos_Impuestos(Grid_Fraccionamientos_Impuesto.PageIndex, tabla);
                        Grid_Fraccionamientos_Impuesto.Enabled = true;
                        Txt_Impuesto_ID.Text = "";
                        Txt_Anio.Text = "";
                        Txt_Monto.Text = "";
                    }
                }
                else
                {
                    Lbl_Ecabezado_Mensaje.Text = "Selecciona el Registro que quieres Quitar.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Impuesto_Click
        ///DESCRIPCIÓN: Modifica un impuesto a la tabla de Fraccionamientos Impuestos(Solo en Interfaz no en la Base de Datos)
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Modificar_Impuesto_Click(object sender, EventArgs e)
        {
            decimal Monto_Impuesto;
            try
            {
                if (Btn_Modificar_Impuesto.AlternateText.Equals("Modificar"))
                {
                    if (Grid_Fraccionamientos_Impuesto.Rows.Count > 0 && Grid_Fraccionamientos_Impuesto.SelectedIndex > (-1))
                    {
                        Hdf_Fraccionamiento_Impuesto_ID.Value = HttpUtility.HtmlDecode(Grid_Fraccionamientos_Impuesto.SelectedRow.Cells[1].Text.Trim());
                        Txt_Impuesto_ID.Text = HttpUtility.HtmlDecode(Grid_Fraccionamientos_Impuesto.SelectedRow.Cells[1].Text.Trim());
                        Txt_Anio.Text = Grid_Fraccionamientos_Impuesto.SelectedRow.Cells[2].Text.Trim();
                        decimal.TryParse(Grid_Fraccionamientos_Impuesto.SelectedRow.Cells[3].Text.Trim(), out Monto_Impuesto);
                        Txt_Monto.Text = Monto_Impuesto.ToString("#,##0.00");
                        Btn_Modificar_Impuesto.AlternateText = "Actualizar";
                        Btn_Quitar_Impuesto.Visible = false;
                        Btn_Agregar_Impuesto.Visible = false;
                        Grid_Fraccionamientos_Impuesto.Enabled = false;
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
                    if (Validar_Componentes_Impuestos())
                    {
                        int registro = ((Grid_Fraccionamientos_Impuesto.PageIndex) * Grid_Fraccionamientos_Impuesto.PageSize) + (Grid_Fraccionamientos_Impuesto.SelectedIndex);
                        if (Session["Dt_Fraccionamientos_Impuestos"] != null)
                        {
                            DataTable tabla = (DataTable)Session["Dt_Fraccionamientos_Impuestos"];
                            int indice = 0;
                            foreach (DataRow Dr_Registro in tabla.Rows)
                            {
                                if (Dr_Registro["ANIO"].ToString() == Txt_Anio.Text.Trim() && Convert.ToDouble(Dr_Registro["MONTO"].ToString()) == Convert.ToDouble(Txt_Monto.Text.Trim()) && indice != registro)
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Fraccionamientos", "alert('Año y monto ya existentes. Registro no Modificado.');", true);
                                    return;
                                }
                                indice++;
                            }
                            tabla.DefaultView.AllowEdit = true;
                            tabla.Rows[registro].BeginEdit();
                            tabla.Rows[registro]["ANIO"] = Txt_Anio.Text.Trim();
                            tabla.Rows[registro]["MONTO"] = Txt_Monto.Text.Trim();
                            tabla.Rows[registro].EndEdit();
                            tabla.AcceptChanges();
                            tabla.DefaultView.Sort = "ANIO DESC, MONTO DESC";
                            Session["Dt_Fraccionamientos_Impuestos"] = tabla;
                            Llenar_Tabla_Fraccionamientos_Impuestos(Grid_Fraccionamientos_Impuesto.PageIndex, tabla);
                            Grid_Fraccionamientos_Impuesto.SelectedIndex = (-1);
                            Btn_Modificar_Impuesto.AlternateText = "Modificar";
                            Btn_Quitar_Impuesto.Visible = true;
                            Btn_Agregar_Impuesto.Visible = true;
                            Tab_Contenedor_Pestagnas.TabIndex = 0;
                            Grid_Fraccionamientos_Impuesto.Visible = true;
                            Hdf_Fraccionamiento_Impuesto_ID.Value = "";
                            Txt_Impuesto_ID.Text = "";
                            Txt_Anio.Text = "";
                            Txt_Monto.Text = "";
                            Grid_Fraccionamientos_Impuesto.Enabled = true;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        #endregion

        protected void Txt_Monto_TextChanged(object sender, EventArgs e)
        {
            if (Txt_Monto.Text.Trim() == "")
            {
                Txt_Monto.Text = "0.00";
            }
            else
            {
                try
                {
                    Txt_Monto.Text = Convert.ToDouble(Txt_Monto.Text).ToString("#,###,###,###,###,###,###,###,##0.00");
                }
                catch
                {
                    Txt_Monto.Text = "0.00";
                }
            }
        }

    #endregion
}