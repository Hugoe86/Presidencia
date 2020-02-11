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
using Presidencia.Catalogo_Multas.Negocio;
using Presidencia.Sessiones;

public partial class paginas_predial_Frm_Cat_Pre_Multas : System.Web.UI.Page{

    #region Page_Load

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        protected void Page_Load(object sender, EventArgs e){
            if (!IsPostBack) {
                Configuracion_Formulario(true);
                Llenar_Tabla_Multas(0);
            }
            Div_Contenedor_Msj_Error.Visible = false;
        }

    #endregion

    #region Metodos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
        ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
        ///PROPIEDADES:     
        ///             1. estatus.    Estatus en el que se cargara la configuración de los controles.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Septiembre/2010 
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
            Grid_Multas_Generales.Enabled = estatus;
            Grid_Multas_Generales.SelectedIndex = (-1);
            Txt_Anio.Enabled = !estatus;
            Txt_Monto.Enabled = !estatus;
            Btn_Agregar_Cuota.Visible = !estatus;
            Btn_Quitar_Cuota.Visible = !estatus;
            Btn_Modificar_Cuota.Visible = !estatus;
            Grid_Multas_Cuotas.SelectedIndex = (-1);
            Grid_Multas_Cuotas.Columns[1].Visible = false;
            Btn_Buscar_Multa.Enabled = estatus;
            Txt_Busqueda_Multa.Enabled = estatus;
            Txt_Desde.Enabled = !estatus;
            Txt_Hasta.Enabled = !estatus;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
        ///DESCRIPCIÓN: Limpia los controles del Formulario
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Septiembre/2010 
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
            Txt_ID_Multa.Text = "";
            Grid_Multas_Cuotas.DataSource = new DataTable();
            Grid_Multas_Cuotas.DataBind();
            Hdf_Multa_ID.Value = "";
            Hdf_Multa_Cuota_ID.Value = "";
            Txt_Hasta.Text = "";
            Txt_Desde.Text = "";
        }

        #region Validaciones

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Generales
            ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
            ///             una operación de la pestaña de Multas.
            ///PROPIEDADES:     
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
                if (Txt_Desde.Text.Trim().Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir un valor en el campo Desde.";
                    Validacion = false;
                }
                if (Txt_Hasta.Text.Trim().Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir un valor en el campo Hasta.";
                    Validacion = false;
                }
                if (Validar_Anios()) 
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ El Año Desde es Mayor que el Año Hasta.";
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
            ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Cuotas
            ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
            ///             una operación de la pestaña de Multas - Cuotas.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 03/Septiembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private bool Validar_Componentes_Cuotas()
            {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                String Mensaje_Error = "";
                Boolean Validacion = true;
                if (Txt_Anio.Text.Trim().Length == 0)
                {
                    Mensaje_Error = Mensaje_Error + "+ Introducir el Año.";
                    Validacion = false;
                }
                if (Txt_Monto.Text.Trim().Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir el Monto.";
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
            ///NOMBRE DE LA FUNCIÓN: Validar_Anios
            ///DESCRIPCIÓN: Hace una validacion de que el año desde que introduce el usuario
            ///             sea menor que el año hasta
            ///PROPIEDADES:     
            ///CREO: José Alfredo García Pichardo.
            ///FECHA_CREO: 18/Agosto/2011 
            ///MODIFICO: Roberto González Oseguera
            ///FECHA_MODIFICO: 20-feb-2012
            ///CAUSA_MODIFICACIÓN: se cambia Convert.ToInt32 por TryParse para evitar 
            ///                 excepción si los campos están vacíos
            ///*******************************************************************************
            private bool Validar_Anios()
            {
                Int32 Desde;
                Int32 Hasta;
                int.TryParse(Txt_Desde.Text.ToString().Trim(), out Desde);
                int.TryParse(Txt_Hasta.Text.Trim(), out Hasta);

                if (Desde > Hasta || Desde < 0 || Hasta < 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        #endregion

        #region Grid
    
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Multas
            ///DESCRIPCIÓN: Llena la tabla de Multas
            ///PROPIEDADES:     
            ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 01/Septiembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Tabla_Multas(int Pagina) {
                try{
                    Cls_Cat_Pre_Multas_Negocio Multa = new Cls_Cat_Pre_Multas_Negocio();
                    Multa.P_Identificador = Txt_Busqueda_Multa.Text.Trim().ToUpper();
                    Grid_Multas_Generales.DataSource = Multa.Consultar_Multas();
                    Grid_Multas_Generales.PageIndex = Pagina;
                    Grid_Multas_Generales.DataBind();
                }catch(Exception Ex){
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;                
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Multas_Cuotas
            ///DESCRIPCIÓN: Llena la tabla de Multas Cuotas
            ///PROPIEDADES:     
            ///             1.  Pagina. Pagina en la cual se mostrará el Grid_VIew
            ///             2.  tabla.  Tabla que se va a cargar en el Grid.    
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 02/Septiembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Tabla_Multas_Cuotas(int Pagina, DataTable Tabla) {
                Grid_Multas_Cuotas.Columns[1].Visible = true;
                Tabla.DefaultView.Sort = "ANIO DESC, MONTO DESC";
                Grid_Multas_Cuotas.DataSource = Tabla;
                Grid_Multas_Cuotas.PageIndex = Pagina;
                Grid_Multas_Cuotas.DataBind();
                Grid_Multas_Cuotas.Columns[1].Visible = false;
                Session["Dt_Multas_Cuotas_Traslado"] = Tabla;
            }

        #endregion
    
    #endregion

    #region Grids
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Multas_Generales_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView General de las Multas
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 02/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Multas_Generales_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                Grid_Multas_Generales.SelectedIndex = (-1);
                Llenar_Tabla_Multas(e.NewPageIndex);
                Limpiar_Catalogo();
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Multas_Generales_SelectedIndexChanged
        ///DESCRIPCIÓN: Obtiene los datos de la Multa Seleccionado para mostrarlos a detalle
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 02/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Multas_Generales_SelectedIndexChanged(object sender, EventArgs e){
            try{
                if (Grid_Multas_Generales.SelectedIndex > (-1)){
                    Limpiar_Catalogo();
                    Session["Dt_Multas_Cuotas_Traslado"] = null;
                    String ID_Seleccionado = Grid_Multas_Generales.SelectedRow.Cells[1].Text;
                    Cls_Cat_Pre_Multas_Negocio Multa = new Cls_Cat_Pre_Multas_Negocio();
                    Multa.P_Multa_ID = ID_Seleccionado;
                    Multa = Multa.Consultar_Datos_Multa();
                    Hdf_Multa_ID.Value = Multa.P_Multa_ID;
                    Txt_ID_Multa.Text = Multa.P_Multa_ID;
                    Txt_Desde.Text = Multa.P_Desde;
                    Txt_Hasta.Text = Multa.P_Hasta;
                    Txt_Identificador.Text = Multa.P_Identificador;
                    Txt_Descripcion.Text = Multa.P_Descripcion;
                    Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Multa.P_Estatus));
                    Llenar_Tabla_Multas_Cuotas(0, Multa.P_Multas_Cuotas);
                    System.Threading.Thread.Sleep(1000);
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Multas_Cuotas_PageIndexChanging
        ///DESCRIPCIÓN: Maneja el Cambio de Pagina de la Tabla de Multas Cuotas
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 02/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Multas_Cuotas_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                if (Session["Dt_Multas_Cuotas_Traslado"] != null){
                    DataTable tabla = (DataTable)Session["Dt_Multas_Cuotas_Traslado"];
                    Llenar_Tabla_Multas_Cuotas(e.NewPageIndex, tabla);
                    Grid_Multas_Cuotas.SelectedIndex = (-1);
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
        ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta una nueva Multa
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 02/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Nuevo_Click(object sender, EventArgs e){
            try{
                if (Btn_Nuevo.AlternateText.Equals("Nuevo")){
                    Configuracion_Formulario(false);
                    Limpiar_Catalogo();
                    Session["Dt_Multas_Cuotas_Traslado"] = null;
                    Btn_Nuevo.AlternateText = "Dar de Alta";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.Visible = false;
                }else {
                    if (Validar_Componentes_Generales()){
                        Cls_Cat_Pre_Multas_Negocio Multa = new Cls_Cat_Pre_Multas_Negocio();
                        Multa.P_Identificador = Txt_Identificador.Text.Trim().ToUpper();
                        Multa.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        Multa.P_Descripcion = Txt_Descripcion.Text.ToUpper();
                        Multa.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Multa.P_Desde = Txt_Desde.Text.Trim();
                        Multa.P_Hasta = Txt_Hasta.Text.Trim();
                        Multa.P_Multas_Cuotas = (DataTable)Session["Dt_Multas_Cuotas_Traslado"];
                        Multa.Alta_Multa();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Session["Dt_Multas_Cuotas_Traslado"] = null;
                        Llenar_Tabla_Multas(Grid_Multas_Generales.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Multas de Traslado de Dominio", "alert('Alta de Multa de Traslado de Dominio Exitosa');", true);
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Grid_Multas_Cuotas.Enabled = true;
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
        ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de una Multa
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
                    if (Grid_Multas_Generales.Rows.Count > 0 && Grid_Multas_Generales.SelectedIndex > (-1)){
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
                    if (Validar_Componentes_Generales()){
                        Cls_Cat_Pre_Multas_Negocio Multa = new Cls_Cat_Pre_Multas_Negocio();
                        Multa.P_Multa_ID = Hdf_Multa_ID.Value;
                        Multa.P_Identificador = Txt_Identificador.Text.ToUpper();
                        Multa.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        Multa.P_Descripcion = Txt_Descripcion.Text.ToUpper();
                        Multa.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Multa.P_Desde = Txt_Desde.Text.Trim();
                        Multa.P_Hasta = Txt_Hasta.Text.Trim();
                        Multa.P_Multas_Cuotas = (DataTable)Session["Dt_Multas_Cuotas_Traslado"];
                        Multa.Modificar_Multa();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Tabla_Multas(Grid_Multas_Generales.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Multas de Traslado de Dominio", "alert('Actualización de Multa de Traslado de Dominio Exitosa');", true);
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Grid_Multas_Cuotas.Enabled = true;
                    }
                    
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Multa_Click
        ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 02/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Multa_Click(object sender, ImageClickEventArgs e){
            try{
                Limpiar_Catalogo();
                Session["Dt_Multas_Cuotas_Traslado"] = null;
                Grid_Multas_Generales.SelectedIndex = (-1);
                Grid_Multas_Cuotas.SelectedIndex = (-1);
                Llenar_Tabla_Multas(0);
                if (Grid_Multas_Generales.Rows.Count == 0 && Txt_Busqueda_Multa.Text.Trim().Length > 0) {
                    Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el Identificador \"" + Txt_Busqueda_Multa.Text + "\" no se encotrarón coincidencias";
                    Lbl_Mensaje_Error.Text = "(Se cargaron todas las Multas de Traslado de Dominio almacenadas)";
                    Div_Contenedor_Msj_Error.Visible = true;
                    Txt_Busqueda_Multa.Text = "";
                    Llenar_Tabla_Multas(0);
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
        ///DESCRIPCIÓN: Elimina una Multa de la Base de Datos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 02/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Eliminar_Click(object sender, EventArgs e){
            try{
                if (Grid_Multas_Generales.Rows.Count > 0 && Grid_Multas_Generales.SelectedIndex > (-1)){
                    Cls_Cat_Pre_Multas_Negocio Multa = new Cls_Cat_Pre_Multas_Negocio();
                    Multa.P_Multa_ID = Grid_Multas_Generales.SelectedRow.Cells[1].Text;
                    Multa.P_Estatus = "BAJA";
                    Multa.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                    Multa.Eliminar_Multa();
                    Grid_Multas_Generales.SelectedIndex = (-1);
                    Llenar_Tabla_Multas(Grid_Multas_Generales.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Multas de Traslado de Dominio", "alert('La Multa fue dada de BAJA exitosamente');", true);
                    Tab_Contenedor_Pestagnas.TabIndex = -1;
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
            try{
                if (Btn_Salir.AlternateText.Equals("Salir")){
                    Session["Dt_Multas_Cuotas_Traslado"] = null;
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }else {
                    Configuracion_Formulario(true);
                    Limpiar_Catalogo();
                    Tab_Contenedor_Pestagnas.TabIndex = 0;
                    Session.Remove("Dt_Multas_Cuotas_Traslado");
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Grid_Multas_Cuotas.Enabled = true;
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        #region Multas - Cuotas

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Cuota_Click
            ///DESCRIPCIÓN: Agrega una nueva cuota a la tabla de Multas Cuotas(Solo en Interfaz no en la Base de Datos)
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 02/Septiembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Btn_Agregar_Cuota_Click(object sender, EventArgs e){
                try{
                    if (Validar_Componentes_Cuotas()){
                        DataTable tabla = (DataTable)Grid_Multas_Cuotas.DataSource;
                        if (tabla == null){
                            if (Session["Dt_Multas_Cuotas_Traslado"] == null){
                                tabla = new DataTable("multas_cuotas");
                                tabla.Columns.Add("MULTA_CUOTA_ID", Type.GetType("System.String"));
                                tabla.Columns.Add("ANIO", Type.GetType("System.String"));
                                tabla.Columns.Add("MONTO", Type.GetType("System.String"));
                            }else{
                                tabla = (DataTable)Session["Dt_Multas_Cuotas_Traslado"];
                            }
                        }
                        // validar que el año no esté ya en la tabla
                        foreach (DataRow Dr_Registro in tabla.Rows)
                        {
                            if (Dr_Registro["ANIO"].ToString() == Txt_Anio.Text.Trim() && Convert.ToDouble(Dr_Registro["MONTO"].ToString()) == Convert.ToDouble(Txt_Monto.Text.Trim()))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Multas de Traslado de Dominio", "alert('Año y monto ya existentes. Registro no agregado.');", true);
                                return;
                            }
                            else if (Dr_Registro["ANIO"].ToString() == Txt_Anio.Text.Trim())
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Multas de Traslado de Dominio", "alert('Año ya existentes. Registro no agregado.');", true);
                                return;
                            }
                        }
                        // agregar registro a la tabla
                        DataRow fila = tabla.NewRow();
                        fila["MULTA_CUOTA_ID"] = HttpUtility.HtmlDecode("");
                        fila["ANIO"] = HttpUtility.HtmlDecode(Txt_Anio.Text.Trim());
                        decimal Monto_Multa;
                        decimal.TryParse(HttpUtility.HtmlDecode(Txt_Monto.Text.Trim()), out Monto_Multa);
                        fila["MONTO"] = Monto_Multa.ToString("0.00");
                        tabla.Rows.Add(fila);
                        tabla.DefaultView.Sort = "ANIO DESC, MONTO DESC";
                        Grid_Multas_Cuotas.DataSource = tabla;
                        Session["Dt_Multas_Cuotas_Traslado"] = tabla;
                        Grid_Multas_Cuotas.DataBind();
                        Grid_Multas_Cuotas.SelectedIndex = (-1);
                        Txt_Anio.Text = "";
                        Txt_Monto.Text = "";
                    }
                }catch(Exception Ex){
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;                
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Cuota_Click
            ///DESCRIPCIÓN: Modifica un impuesto a la tabla de Fraccionamientos Impuestos(Solo en Interfaz no en la Base de Datos)
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 23/Agosto/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Btn_Modificar_Cuota_Click(object sender, EventArgs e)
            {
                try
                {
                    if (Btn_Modificar_Cuota.AlternateText.Equals("Modificar"))
                    {
                        if (Grid_Multas_Cuotas.Rows.Count > 0 && Grid_Multas_Cuotas.SelectedIndex > (-1))
                        {
                            Hdf_Multa_Cuota_ID.Value = Grid_Multas_Cuotas.SelectedRow.Cells[1].Text.Trim();
                            Txt_Multa_Cuota_ID.Text = Grid_Multas_Cuotas.SelectedRow.Cells[1].Text.Trim();
                            Txt_Anio.Text = Grid_Multas_Cuotas.SelectedRow.Cells[2].Text.Trim();
                            Txt_Monto.Text = Convert.ToDouble(Grid_Multas_Cuotas.SelectedRow.Cells[3].Text.Trim()).ToString("#,##0.00");
                            Btn_Modificar_Cuota.AlternateText = "Actualizar";
                            Btn_Quitar_Cuota.Visible = false;
                            Btn_Agregar_Cuota.Visible = false;
                            Grid_Multas_Cuotas.Enabled = false;
                        }
                        else
                        {
                            Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Modificar.";
                            Lbl_Mensaje_Error.Text = "";
                            Div_Contenedor_Msj_Error.Visible = true;
                        }
                    }
                    else
                    {
                        if (Validar_Componentes_Cuotas())
                        {
                            int registro = ((Grid_Multas_Cuotas.PageIndex) * Grid_Multas_Cuotas.PageSize) + (Grid_Multas_Cuotas.SelectedIndex);
                            if (Session["Dt_Multas_Cuotas_Traslado"] != null)
                            {
                                DataTable tabla = (DataTable)Session["Dt_Multas_Cuotas_Traslado"];
                                int indice = 0;
                                foreach (DataRow Dr_Registro in tabla.Rows)
                                {
                                    if (Dr_Registro["ANIO"].ToString() == Txt_Anio.Text.Trim() && Convert.ToDouble(Dr_Registro["MONTO"].ToString()) == Convert.ToDouble(Txt_Monto.Text.Trim()) && indice != registro)
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Multas de Traslado de Dominio", "alert('Año y monto ya existentes. Registro no Modificado.');", true);
                                        return;
                                    }
                                    else if (Dr_Registro["ANIO"].ToString() == Txt_Anio.Text.Trim() && indice != registro)
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Multas de Traslado de Dominio", "alert('Año ya existentes. Registro no Modificado.');", true);
                                        return;
                                    }
                                    indice++;
                                }
                                tabla.DefaultView.AllowEdit = true;
                                tabla.Rows[registro].BeginEdit();
                                tabla.Rows[registro]["ANIO"] = Txt_Anio.Text.Trim();
                                tabla.Rows[registro]["MONTO"] = Txt_Monto.Text.Trim();
                                tabla.Rows[registro].EndEdit();
                                tabla.DefaultView.Sort = "ANIO DESC, MONTO DESC";
                                Session["Dt_Multas_Cuotas_Traslado"] = tabla;
                                Llenar_Tabla_Multas_Cuotas(Grid_Multas_Cuotas.PageIndex, tabla);
                                Grid_Multas_Cuotas.SelectedIndex = (-1);
                                Btn_Modificar_Cuota.AlternateText = "Modificar";
                                Btn_Quitar_Cuota.Visible = true;
                                Btn_Agregar_Cuota.Visible = true;
                                Tab_Contenedor_Pestagnas.TabIndex = 0;
                                Grid_Multas_Cuotas.Enabled = true;
                                Hdf_Multa_Cuota_ID.Value = "";
                                Txt_Multa_Cuota_ID.Text = "";
                                Txt_Anio.Text = "";
                                Txt_Monto.Text = "";
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

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Quitar_Cuota_Click
            ///DESCRIPCIÓN: Quita un impuesto a la tabla de Multas Cuotas(Solo en Interfaz no en la Base de Datos)
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 02/Septiembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Btn_Quitar_Cuota_Click(object sender, EventArgs e){
                try{
                    if (Grid_Multas_Cuotas.Rows.Count > 0 && Grid_Multas_Cuotas.SelectedIndex > (-1)){
                        int registro = ((Grid_Multas_Cuotas.PageIndex) * Grid_Multas_Cuotas.PageSize) + (Grid_Multas_Cuotas.SelectedIndex);
                        if (Session["Dt_Multas_Cuotas_Traslado"] != null){
                            DataTable tabla = (DataTable)Session["Dt_Multas_Cuotas_Traslado"];
                            tabla.Rows.RemoveAt(registro);
                            tabla.DefaultView.Sort = "ANIO DESC, MONTO DESC";
                            Session["Dt_Multas_Cuotas_Traslado"] = tabla;
                            Grid_Multas_Cuotas.SelectedIndex = (-1);
                            Llenar_Tabla_Multas_Cuotas(Grid_Multas_Cuotas.PageIndex, tabla);
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
        
        #endregion
    
    #endregion

}