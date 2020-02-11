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
using Presidencia.Constantes;
using System.Collections.Generic;
using Presidencia.Control_Patrimonial_Catalogo_Tipos_Vehiculo.Negocio;
using System.IO;
using Presidencia.Control_Patrimonial_Catalogo_Detalles_Vehiculos.Negocio;

public partial class paginas_Compras_Frm_Cat_Pat_Com_Tipos_Vehiculo : System.Web.UI.Page
{
    
    #region Page_Load

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///******************************************************************************* 
        protected void Page_Load(object sender, EventArgs e){
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!IsPostBack)
            {
                Llenar_Combo_Aseguradoras();
                Configuracion_Formulario(true);
                Llenar_Grid_Tipos_Vehiculo(0);
                Grid_Tipos_Vehiculo.Columns[4].Visible = false;
                Llenar_Grid_Detalles_Vehiculo();
            }
            Div_Contenedor_Msj_Error.Visible = false;
        }

    #endregion
    
    #region Metodos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Aseguradoras
        ///DESCRIPCIÓN: Se llena el combo de Aseguradoras.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 11/Marzo/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///******************************************************************************* 
        public void Llenar_Combo_Aseguradoras() {
            try {
                Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio Negocio = new Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio();
                Negocio.P_Tipo_DataTable = "ASEGURADORAS";
                DataTable Tabla = Negocio.Consultar_DataTable();
                DataRow Fila = Tabla.NewRow();
                Fila["ASEGURADORA_ID"] = "SELECCIONE";
                Fila["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
                Tabla.Rows.InsertAt(Fila, 0);
                Cmb_Aseguradoras.DataSource = Tabla;
                Cmb_Aseguradoras.DataTextField = "NOMBRE";
                Cmb_Aseguradoras.DataValueField = "ASEGURADORA_ID";
                Cmb_Aseguradoras.DataBind();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
        ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
        ///PARAMETROS:     
        ///             1. Estatus.    Estatus en el que se cargara la configuración de los
        ///                             controles.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23/Noviembre/2010 
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
            Txt_Descripcion.Enabled = !Estatus;
            Cmb_Estatus.Enabled = !Estatus;
            Grid_Tipos_Vehiculo.Enabled = Estatus;
            Grid_Tipos_Vehiculo.SelectedIndex = (-1);
            Btn_Buscar.Enabled = Estatus;
            Txt_Busqueda.Enabled = Estatus;
            Cmb_Aseguradoras.Enabled = !Estatus;
            Txt_Numero_Poliza_Seguro.Enabled = !Estatus;
            Txt_Numero_Inciso.Enabled = !Estatus;
            Txt_Cobertura_Seguro.Enabled = !Estatus;
            //Configuracion_Acceso("Frm_Cat_Pat_Com_Tipos_Vehiculo.aspx");
            Txt_Nombre_Archivo.Enabled = !Estatus;
            Txt_Comentarios_Archivo.Enabled = !Estatus;
            AFU_Archivo.Enabled = !Estatus;
            Btn_Limpiar_FileUpload.Visible = !Estatus;
            Grid_Detalles_Vehiculo.Enabled = !Estatus;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
        ///DESCRIPCIÓN: Limpia los controles del Formulario
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Catalogo() {
            Hdf_Tipo_Vehiculo_ID.Value = "";
            Txt_Tipo_Vehiculo_ID.Text = "";
            Txt_Descripcion.Text = "";
            Cmb_Estatus.SelectedIndex = 0;
            Cmb_Aseguradoras.SelectedIndex = 0;
            Txt_Numero_Poliza_Seguro.Text = "";
            Txt_Numero_Inciso.Text = "";
            Txt_Cobertura_Seguro.Text = "";
            Txt_Nombre_Archivo.Text = "";
            Txt_Comentarios_Archivo.Text = "";
            Remover_Sesiones_Control_AsyncFileUpload(AFU_Archivo.ClientID);
            Session.Remove("Dt_Historial_Archivos");
            Grid_Archivos.DataSource = new DataTable();
            Grid_Archivos.DataBind();
            for (Int32 Contador = 0; Contador < (Grid_Detalles_Vehiculo.Rows.Count); Contador++) {
                if (Grid_Detalles_Vehiculo.Rows[Contador].FindControl("Chk_Seleccion_Detalle") != null) {
                    CheckBox Chk_Temporal = (CheckBox)Grid_Detalles_Vehiculo.Rows[Contador].FindControl("Chk_Seleccion_Detalle");
                    Chk_Temporal.Checked = false;
                }
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Remover_Sesiones_Control_AsyncFileUpload
        ///DESCRIPCIÓN: Limpia un control de AsyncFileUpload
        ///PARAMETROS:     
        ///CREO: Juan Alberto Hernandez Negrete
        ///FECHA_CREO: 16/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Remover_Sesiones_Control_AsyncFileUpload(String Cliente_ID)
        {
            HttpContext Contexto;
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                Contexto = HttpContext.Current;
            }
            else
            {
                Contexto = null;
            }
            if (Contexto != null)
            {
                foreach (String key in Contexto.Session.Keys)
                {
                    if (key.Contains(Cliente_ID))
                    {
                        Contexto.Session.Remove(key);
                        break;
                    }
                }
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Tipos_Vehiculo
        ///DESCRIPCIÓN: Llena la tabla de Tipos de Vehiculo con una consulta que puede o no
        ///             tener Filtros.
        ///PARAMETROS:     
        ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Grid_Tipos_Vehiculo(Int32 Pagina) {
            try{
                Grid_Tipos_Vehiculo.Columns[4].Visible = true;
                Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio Tipo_Vehiculo = new Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio();
                Tipo_Vehiculo.P_Tipo_DataTable = "TIPOS_VEHICULOS";
                Tipo_Vehiculo.P_Descripcion = Txt_Busqueda.Text.Trim();
                Grid_Tipos_Vehiculo.DataSource = Tipo_Vehiculo.Consultar_DataTable();
                Grid_Tipos_Vehiculo.PageIndex = Pagina;
                Grid_Tipos_Vehiculo.DataBind();
                Grid_Tipos_Vehiculo.Columns[4].Visible = false;
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Historial_Archivos
        ///DESCRIPCIÓN: Llena la tabla de Historial de Archivos
        ///PARAMETROS:     
        ///             1.  Pagina. Pagina en la cual se mostrará el Grid_VIew
        ///             2.  Tabla.  Tabla que se va a cargar en el Grid.    
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Grid_Historial_Archivos(Int32 Pagina, DataTable Tabla)  {
            Grid_Archivos.Columns[0].Visible = true;
            Grid_Archivos.Columns[1].Visible = true;
            Grid_Archivos.Columns[2].Visible = true;
            Grid_Archivos.DataSource = Tabla;
            Grid_Archivos.PageIndex = Pagina;
            Grid_Archivos.DataBind();
            Grid_Archivos.Columns[0].Visible = false;
            Grid_Archivos.Columns[1].Visible = false;
            Grid_Archivos.Columns[2].Visible = false;
            Session["Dt_Historial_Archivos"] = Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Detalles_Vehiculo
        ///DESCRIPCIÓN: Llena la tabla de Detalles de Parte de Vehiculo.
        ///PARAMETROS:       
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 07/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Grid_Detalles_Vehiculo() {
            Cls_Cat_Pat_Com_Detalles_Vehiculos_Negocio Detalles = new Cls_Cat_Pat_Com_Detalles_Vehiculos_Negocio();
            Detalles.P_Estatus = "VIGENTE";
            Detalles.P_Tipo_DataTable = "DETALLES_VEHICULOS";
            Grid_Detalles_Vehiculo.Columns[1].Visible = true;
            Grid_Detalles_Vehiculo.DataSource = Detalles.Consultar_DataTable();
            Grid_Detalles_Vehiculo.DataBind();
            Grid_Detalles_Vehiculo.Columns[1].Visible = false;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Cargar_Grid_Detalles_Vehiculo
        ///DESCRIPCIÓN: Carga los detalles de la tabla de Detalles de Parte de Vehiculo.
        ///PARAMETROS: Dt_Detalles. Detalles para cargar el grid.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 07/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Cargar_Grid_Detalles_Vehiculo(DataTable Dt_Detalles) {
            for (Int32 Contador = 0; Contador < (Grid_Detalles_Vehiculo.Rows.Count); Contador++) {
                String Grid_Detalle_ID = Grid_Detalles_Vehiculo.Rows[Contador].Cells[1].Text.Trim();
                for (Int32 Contador_Dt = 0; Contador_Dt < (Dt_Detalles.Rows.Count); Contador_Dt++) {
                    String Dt_Detalle_ID = Dt_Detalles.Rows[Contador_Dt]["DETALLE_ID"].ToString().Trim();
                    if (Grid_Detalle_ID.Equals(Dt_Detalle_ID)) {
                        if (Grid_Detalles_Vehiculo.Rows[Contador].FindControl("Chk_Seleccion_Detalle") != null) {
                            CheckBox Chk_Temporal = (CheckBox)Grid_Detalles_Vehiculo.Rows[Contador].FindControl("Chk_Seleccion_Detalle");
                            Chk_Temporal.Checked = true;
                        }
                    }
                }
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Cargar_Dt_Detalles
        ///DESCRIPCIÓN: Carga los detalles de la tabla de Detalles en un DataTable.
        ///PARAMETROS:       
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 07/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private DataTable Cargar_Dt_Detalles() {
            DataTable Dt_Detalles = new DataTable();
            Dt_Detalles.Columns.Add("DETALLE_ID", Type.GetType("System.String"));
            Dt_Detalles.Columns.Add("NOMBRE", Type.GetType("System.String"));
            for (Int32 Contador = 0; Contador < (Grid_Detalles_Vehiculo.Rows.Count); Contador++) { 
                if (Grid_Detalles_Vehiculo.Rows[Contador].FindControl("Chk_Seleccion_Detalle") != null) {
                    CheckBox Chk_Temporal = (CheckBox)Grid_Detalles_Vehiculo.Rows[Contador].FindControl("Chk_Seleccion_Detalle");
                    if (Chk_Temporal.Checked) {
                        DataRow Fila = Dt_Detalles.NewRow();
                        Fila["DETALLE_ID"] = Grid_Detalles_Vehiculo.Rows[Contador].Cells[1].Text.ToString();
                        Fila["NOMBRE"] = Grid_Detalles_Vehiculo.Rows[Contador].Cells[2].Text.ToString();
                        Dt_Detalles.Rows.Add(Fila);
                    }
                }                
            }
            return Dt_Detalles;
        }

        #region Validaciones

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Validar_Componentes
            ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
            ///             una operación.
            ///PARAMETROS:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 23/Noviembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private bool Validar_Componentes() {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                String Mensaje_Error = "";
                Boolean Validacion = true;
                if (Txt_Descripcion.Text.Trim().Length == 0) {
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Descripción del Tipo de Vehiculo.";
                    Validacion = false;
                }
                if (Cmb_Estatus.SelectedIndex == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar una opcion del Combo de Estatus.";
                    Validacion = false;
                }
                if (Txt_Cobertura_Seguro.Text.Trim().Length > 150) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ La Cobertura es de Máximo 150 Caracteres.";
                    Validacion = false;
                }
                if (AFU_Archivo.HasFile) { 
                    if (Txt_Nombre_Archivo.Text.Trim().Length == 0) {
                        if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                        Mensaje_Error = Mensaje_Error + "+ Introducir el Nombre o Asunto del Archivo a subir.";
                        Validacion = false;
                    }
                    if (Txt_Comentarios_Archivo.Text.Trim().Length > 200) {
                        if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                        Mensaje_Error = Mensaje_Error + "+ Los Comentarios del Archivo deben ser de Máximo 200 Caracteres.";
                        Validacion = false;
                    }
                }

                if (!Validacion) {
                    Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
                    Div_Contenedor_Msj_Error.Visible = true;
                }
                return Validacion;
            }

        #endregion

    #endregion

    #region Grids

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Tipos_Vehiculo_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView de los Tipos de Vehiculo
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Tipos_Vehiculo_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                Grid_Tipos_Vehiculo.SelectedIndex = (-1);
                Llenar_Grid_Tipos_Vehiculo(e.NewPageIndex);
                Limpiar_Catalogo();
            } catch(Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Tipos_Vehiculo_SelectedIndexChanged
        ///DESCRIPCIÓN: Obtiene los datos de una Tipo de Vehiculo Seleccionada para mostrarlos
        ///             a detalle.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Tipos_Vehiculo_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                if (Grid_Tipos_Vehiculo.SelectedIndex > (-1)){
                    Limpiar_Catalogo();
                    String Tipo_ID = HttpUtility.HtmlDecode(Grid_Tipos_Vehiculo.SelectedRow.Cells[1].Text.Trim());
                    Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio Tipo_Vehiculo = new Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio();
                    Tipo_Vehiculo.P_Tipo_Vehiculo_ID = Tipo_ID;
                    Tipo_Vehiculo = Tipo_Vehiculo.Consultar_Datos_Vehiculo();
                    Hdf_Tipo_Vehiculo_ID.Value = Tipo_Vehiculo.P_Tipo_Vehiculo_ID;
                    Txt_Tipo_Vehiculo_ID.Text = Tipo_Vehiculo.P_Tipo_Vehiculo_ID;
                    Txt_Descripcion.Text = Tipo_Vehiculo.P_Descripcion;
                    Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Tipo_Vehiculo.P_Estatus));
                    if (Tipo_Vehiculo.P_Aseguradora_ID != null && Tipo_Vehiculo.P_Aseguradora_ID.Trim().Length > 0) { 
                        Cmb_Aseguradoras.SelectedIndex = Cmb_Aseguradoras.Items.IndexOf(Cmb_Aseguradoras.Items.FindByValue(Tipo_Vehiculo.P_Aseguradora_ID));
                        Txt_Numero_Poliza_Seguro.Text = Tipo_Vehiculo.P_No_Poliza_Seguro.ToString();
                        Txt_Numero_Inciso.Text = Tipo_Vehiculo.P_Descripcion_Seguro;
                        Txt_Cobertura_Seguro.Text = Tipo_Vehiculo.P_Cobertura_Seguro;
                    }
                    if (Tipo_Vehiculo.P_Dt_Archivos != null && Tipo_Vehiculo.P_Dt_Archivos.Rows.Count > 0){
                        Llenar_Grid_Historial_Archivos(0, Tipo_Vehiculo.P_Dt_Archivos);
                    } else {
                        Grid_Archivos.DataSource = new DataTable();
                        Grid_Archivos.DataBind();
                        Session.Remove("Dt_Historial_Archivos");
                    }
                    if (Tipo_Vehiculo.P_Dt_Detalles != null && Tipo_Vehiculo.P_Dt_Detalles.Rows.Count > 0) {
                        Cargar_Grid_Detalles_Vehiculo(Tipo_Vehiculo.P_Dt_Detalles);
                    }
                    System.Threading.Thread.Sleep(500);
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Archivos_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView de Historial de Archivos
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Julio/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Archivos_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                if (Session["Dt_Historial_Archivos"] != null) {
                    Grid_Archivos.SelectedIndex = (-1);
                    Llenar_Grid_Historial_Archivos(e.NewPageIndex, (DataTable)Session["Dt_Historial_Archivos"]);
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Archivos_RowDataBound
        ///DESCRIPCIÓN: Maneja el evento de RowDataBound del Grid de Archivos
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Julio/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Archivos_RowDataBound(object sender, GridViewRowEventArgs e) {
            if (e.Row.RowType == DataControlRowType.DataRow) {
                ImageButton Boton = (ImageButton)e.Row.FindControl("Btn_Ver_Archivo");
                Boton.CommandArgument = e.Row.Cells[0].Text.Trim();
            }
        }

    #endregion

    #region Eventos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
        ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta una nuevo Tipo de Vehiculo.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23/Noviembre/2010 
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
                    Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue("VIGENTE"));
                }else {
                    if (Validar_Componentes()){
                        Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio Tipo_Vehiculo = new Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio();
                        Tipo_Vehiculo.P_Descripcion = Txt_Descripcion.Text.Trim();
                        Tipo_Vehiculo.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        if (Cmb_Aseguradoras.SelectedIndex > 0) {
                            Tipo_Vehiculo.P_Aseguradora_ID = Cmb_Aseguradoras.SelectedItem.Value;
                            Tipo_Vehiculo.P_No_Poliza_Seguro = Txt_Numero_Poliza_Seguro.Text;
                            Tipo_Vehiculo.P_Descripcion_Seguro = Txt_Numero_Inciso.Text;
                            Tipo_Vehiculo.P_Cobertura_Seguro = Txt_Cobertura_Seguro.Text;
                        }
                        if (AFU_Archivo.HasFile) {
                            Tipo_Vehiculo.P_Descripcion_Archivo = Txt_Nombre_Archivo.Text.Trim();
                            Tipo_Vehiculo.P_Comentarios_Archivo = Txt_Comentarios_Archivo.Text.Trim();
                            Tipo_Vehiculo.P_Nombre_Fisico_Archivo = Path.GetExtension(AFU_Archivo.FileName);
                            Tipo_Vehiculo.P_Fecha = DateTime.Today;
                        }
                        Tipo_Vehiculo.P_Dt_Detalles = Cargar_Dt_Detalles();
                        Tipo_Vehiculo.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                        Tipo_Vehiculo = Tipo_Vehiculo.Alta_Tipo_Vehiculo();
                        if (AFU_Archivo.HasFile) {
                            String Archivo = Server.MapPath("../../" + Cat_Pat_Tipo_Vehiculo_Archivo.Campo_Ruta_Fisica_Archivos + "/TIPOS_VEHICULOS/" + Tipo_Vehiculo.P_Nombre_Fisico_Archivo);
                            AFU_Archivo.SaveAs(Archivo);
                        }
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Grid_Tipos_Vehiculo(Grid_Tipos_Vehiculo.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tipos de Vehiculo", "alert('Alta de Tipo de Vehiculo Exitosa');", true);
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
        ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de un Tipo
        ///             de Vehiculo.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Modificar_Click(object sender, EventArgs e){
            try{
                if (Btn_Modificar.AlternateText.Equals("Modificar")){
                    if (Grid_Tipos_Vehiculo.Rows.Count > 0 && Grid_Tipos_Vehiculo.SelectedIndex > (-1)){
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
                        Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio Tipo_Vehiculo = new Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio();
                        Tipo_Vehiculo.P_Tipo_Vehiculo_ID = Hdf_Tipo_Vehiculo_ID.Value;
                        Tipo_Vehiculo.P_Descripcion = Txt_Descripcion.Text.Trim();
                        Tipo_Vehiculo.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        if (Cmb_Aseguradoras.SelectedIndex > 0) {
                            Tipo_Vehiculo.P_Aseguradora_ID = Cmb_Aseguradoras.SelectedItem.Value;
                            Tipo_Vehiculo.P_No_Poliza_Seguro = Txt_Numero_Poliza_Seguro.Text;
                            Tipo_Vehiculo.P_Descripcion_Seguro = Txt_Numero_Inciso.Text;
                            Tipo_Vehiculo.P_Cobertura_Seguro = Txt_Cobertura_Seguro.Text;
                        }
                        if (AFU_Archivo.HasFile) {
                            Tipo_Vehiculo.P_Descripcion_Archivo = Txt_Nombre_Archivo.Text.Trim();
                            Tipo_Vehiculo.P_Comentarios_Archivo = Txt_Comentarios_Archivo.Text.Trim();
                            Tipo_Vehiculo.P_Nombre_Fisico_Archivo = Path.GetExtension(AFU_Archivo.FileName);
                            Tipo_Vehiculo.P_Fecha = DateTime.Today;
                        }
                        Tipo_Vehiculo.P_Dt_Detalles = Cargar_Dt_Detalles();
                        Tipo_Vehiculo.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                        Tipo_Vehiculo = Tipo_Vehiculo.Modificar_Tipo_Vehiculo();
                        if (AFU_Archivo.HasFile) {
                            String Directorio = Server.MapPath("../../" + Cat_Pat_Tipo_Vehiculo_Archivo.Campo_Ruta_Fisica_Archivos + "/TIPOS_VEHICULOS/");
                            if (!Directory.Exists(Directorio)) {
                                Directory.CreateDirectory(Directorio);
                            }
                            String Archivo = Directorio  + Tipo_Vehiculo.P_Nombre_Fisico_Archivo;
                            AFU_Archivo.SaveAs(Archivo);
                        }
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Grid_Tipos_Vehiculo(Grid_Tipos_Vehiculo.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tipos de Vehiculo", "alert('Actualización de Tipo de Vehiculo Exitosa');", true);
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
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
        ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e) {
            try{
                Limpiar_Catalogo();
                Grid_Tipos_Vehiculo.SelectedIndex = (-1);
                Grid_Tipos_Vehiculo.SelectedIndex = (-1);
                Llenar_Grid_Tipos_Vehiculo(0);
                if (Grid_Tipos_Vehiculo.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0) {
                    Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el nombre \"" + Txt_Busqueda.Text + "\" no se encontrarón coincidencias";
                    Lbl_Mensaje_Error.Text = "(Se cargarón todos los Tipos de Vehiculo almacenadas)";
                    Div_Contenedor_Msj_Error.Visible = true;
                    Txt_Busqueda.Text = "";
                    Llenar_Grid_Tipos_Vehiculo(0);
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
        ///DESCRIPCIÓN: Elimina un Tipo de Vehiculo de la Base de Datos
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 23/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Eliminar_Click(object sender, EventArgs e){
            try{
                if (Grid_Tipos_Vehiculo.Rows.Count > 0 && Grid_Tipos_Vehiculo.SelectedIndex > (-1)){
                    Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio Tipo_Vehiculo = new Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio();
                    Tipo_Vehiculo.P_Tipo_Vehiculo_ID = Hdf_Tipo_Vehiculo_ID.Value;
                    Tipo_Vehiculo.Eliminar_Tipo_Vehiculo();
                    Grid_Tipos_Vehiculo.SelectedIndex = (-1);
                    Llenar_Grid_Tipos_Vehiculo(Grid_Tipos_Vehiculo.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tipos de Vehiculo", "alert('El Tipo de Vehiculo fue eliminada exitosamente');", true);
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
        ///FECHA_CREO: 23/Noviembre/2010 
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

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Limpiar_FileUpload_Click
        ///DESCRIPCIÓN: Limpia el FileUpload que carga los archivos
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Julio/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Limpiar_FileUpload_Click(object sender, ImageClickEventArgs e) {
            try {
                Remover_Sesiones_Control_AsyncFileUpload(AFU_Archivo.ClientID);
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Ver_Archivo_Click
        ///DESCRIPCIÓN: Se ve el archivo del grid
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Julio/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Ver_Archivo_Click(object sender, ImageClickEventArgs e)  {
            try  {
                ImageButton Boton = (ImageButton)sender;
                String Archivo_Bien_ID = Boton.CommandArgument;
                for (Int32 Contador = 0; Contador < Grid_Archivos.Rows.Count; Contador++) {
                    if (Grid_Archivos.Rows[Contador].Cells[0].Text.Trim().Equals(Archivo_Bien_ID)) {
                        String Archivo = "../../" + Cat_Pat_Tipo_Vehiculo_Archivo.Campo_Ruta_Fisica_Archivos + "/TIPOS_VEHICULOS/" +  Grid_Archivos.Rows[Contador].Cells[1].Text.Trim();
                        if (File.Exists(Server.MapPath(Archivo))) {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Archivo_Archivos", "window.open('" + Archivo + "','Window_Archivo','left=0,top=0')", true);
                            break;
                        }  else  {
                            Lbl_Ecabezado_Mensaje.Text = "El Archivo no esta disponible o fue eliminado";
                            Lbl_Mensaje_Error.Text = "";
                            Div_Contenedor_Msj_Error.Visible = true;
                        }
                    }
                }
            }  catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

    #endregion

    #region (Control Acceso Pagina)
        /// *****************************************************************************************************************************
        /// NOMBRE: Configuracion_Acceso
        /// 
        /// DESCRIPCIÓN: Habilita las operaciones que podrá realizar el usuario en la página.
        /// 
        /// PARÁMETROS: No Áplica.
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 23/Mayo/2011 10:43 a.m.
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA MODIFICACIÓN:
        /// *****************************************************************************************************************************
        protected void Configuracion_Acceso(String URL_Pagina)
        {
            List<ImageButton> Botones = new List<ImageButton>();//Variable que almacenara una lista de los botones de la página.
            DataRow[] Dr_Menus = null;//Variable que guardara los menus consultados.

            try
            {
                //Agregamos los botones a la lista de botones de la página.
                Botones.Add(Btn_Nuevo);
                Botones.Add(Btn_Modificar);
                Botones.Add(Btn_Eliminar);
                Botones.Add(Btn_Buscar);

                if (!String.IsNullOrEmpty(Request.QueryString["PAGINA"]))
                {
                    if (Es_Numero(Request.QueryString["PAGINA"].Trim()))
                    {
                        //Consultamos el menu de la página.
                        Dr_Menus = Cls_Sessiones.Menu_Control_Acceso.Select("MENU_ID=" + Request.QueryString["PAGINA"]);

                        if (Dr_Menus.Length > 0)
                        {
                            //Validamos que el menu consultado corresponda a la página a validar.
                            if (Dr_Menus[0][Apl_Cat_Menus.Campo_URL_Link].ToString().Contains(URL_Pagina))
                            {
                                Cls_Util.Configuracion_Acceso_Sistema_SIAS_AlternateText(Botones, Dr_Menus[0]); // Habilitamos la configuracón de los botones.
                            }
                            else
                            {
                                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                            }
                        }
                        else
                        {
                            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                    }
                }
                else
                {
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al habilitar la configuración de accesos a la página. Error: [" + Ex.Message + "]");
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: IsNumeric
        /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
        /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 29/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private Boolean Es_Numero(String Cadena)
        {
            Boolean Resultado = true;
            Char[] Array = Cadena.ToCharArray();
            try
            {
                for (int index = 0; index < Array.Length; index++)
                {
                    if (!Char.IsDigit(Array[index])) return false;
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al validar si es un dato numerico. Error [" + Ex.Message + "]");
            }
            return Resultado;
        }
        #endregion
}