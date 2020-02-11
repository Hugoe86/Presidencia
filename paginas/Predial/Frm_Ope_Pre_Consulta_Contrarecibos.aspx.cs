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
using Presidencia.Operacion_Predial_Traslado.Negocio;
using Presidencia.Operacion_Predial_Traslado.Datos;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using System.IO;
using Operacion_Predial_Validacion_Recepcion.Negocio;
using Operacion_Predial_Orden_Variacion.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using Presidencia.Reportes;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;

public partial class paginas_Predial_Frm_Ope_Pre_Consulta_Contrarecibos : System.Web.UI.Page
{
    #region Variables
        private const int Const_Estado_Inicial = 0;
        private const int Const_Estado_Modificar = 1;
        private const int Const_Estado_Imprimir = 2;
    
    #endregion

    #region Page_Load

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 09/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        protected void Page_Load(object sender, EventArgs e){
            if (!IsPostBack) {
                Estado_Botones(Const_Estado_Inicial);
                Llenar_Combo_Notarios();
                Llenar_Grid_Contrarecibos(0);                
            }
            Div_Contenedor_Msj_Error.Visible = false;
        }

    #endregion

    #region Metodos

        #region Metodos/Generales
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Estado_Botones
        ///DESCRIPCIÓN: Metodo para establecer el estado de los botones y componentes del formulario
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 02/02/2011 05:49:53 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        private void Estado_Botones(int P_Estado)
        {
            bool Estado;
            Estado = true;
            switch (P_Estado)
            {
                case 0: //Estado inicial
                    Estado = true;
                    Btn_Actualizar.AlternateText = "Modificar";
                    Btn_Imprimir.AlternateText = "Imprimir";
                    Btn_Salir.AlternateText = "Inicio";
                    Btn_Actualizar.ToolTip = "Modificar";
                    Btn_Imprimir.ToolTip = "Imprimir";
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Actualizar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Actualizar.Visible = true;
                    Btn_Imprimir.Visible = true;
                    Btn_Salir.Visible = true;
                    Hdn_Estado_Guardar.Value = "Modificar";

                break;

                case 1: //Modificar

                    Estado = false;
                    Btn_Actualizar.AlternateText = "Guardar";
                    Btn_Imprimir.AlternateText = "Imprimir";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Actualizar.ToolTip = "Guardar";
                    Btn_Imprimir.ToolTip = "Imprimir";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Actualizar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Actualizar.Visible = true;
                    Btn_Imprimir.Visible = false;
                    Btn_Salir.Visible = true;
                    Hdn_Estado_Guardar.Value = "Guardar";

                break;

                case 2: //Imprimir

                break;

            }
            Cmb_Estatus.Enabled = true;
            Txt_Cuenta_Predial.Enabled = Estado;
            Txt_Fecha_Escritura.Enabled = Estado;
            Txt_No_Escritura.Enabled = Estado;
            Txt_No_Contrarecibo.Enabled = Estado;
        }
        #endregion

        #region Metodos Reporte
        public void Generar_Reporte(String No_Contrarecibo)
        {
            Cls_Ope_Pre_Traslado_Negocio Traslado = new Cls_Ope_Pre_Traslado_Negocio();
            DataSet Ds_Reporte_Contrarecibo;
            String No_Movimiento;
            String Recepcion = "";
            String Movimiento = "";
            Cls_Ope_Pre_Validacion_Recepcion_Negocio  Generar_Reporte_Contra_Recibo = new Cls_Ope_Pre_Validacion_Recepcion_Negocio();
            try
            {
                    if (!String.IsNullOrEmpty(No_Contrarecibo))
                {                    
                    No_Movimiento = Traslado.Consultar_Recepcion(No_Contrarecibo);
                    if (No_Movimiento.Contains('-'))
                    {
                        Recepcion = No_Movimiento.Split('-')[0].ToString();
                        Movimiento = No_Movimiento.Split('-')[1].ToString();
                    }
                    Generar_Reporte_Contra_Recibo.P_No_Recepcion_Documento = Recepcion;
                    Generar_Reporte_Contra_Recibo.P_No_Movimiento = Movimiento;
                    //Generar_Reporte_Contra_Recibo.P_No_Contrarecibo = No_Contrarecibo;
                    Ds_Reporte_Contrarecibo = Generar_Reporte_Contra_Recibo.Generar_Reporte_Contra_Recibo();
                    Ds_Reporte_Contrarecibo.Tables[0].Rows[0]["ESTATUS"] = Grid_Contrarecibos.SelectedDataKey["ESTATUS"].ToString();
                    Generar_Reporte(Ds_Reporte_Contrarecibo);
                }
                else
                {
                    throw new Exception("No hay contrarecibo seleccionado");
                }

            }
            catch (Exception Ex)
            {
                Div_Contenedor_Msj_Error.Visible = true;
                IBtn_Imagen_Error.Visible = true;
                Lbl_Ecabezado_Mensaje.Text = "";
                Lbl_Mensaje_Error.Text = "No se puede consultar la información del contrarecibo";
            }
        }
        #endregion
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
        ///DESCRIPCIÓN:          Carga el data set físico con el cual se genera el Reporte especificado
        ///PARAMETROS:           1.-Data_Set_Consulta_Inventario.- Contiene la informacion de la consulta a la base de datos
        ///                      2.-Ds_Reporte_Stock, Objeto que contiene la instancia del Data set fisico del Reporte a generar
        ///                      3.-Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           17/Febrero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        private void Generar_Reporte(DataSet Ds_Reporte_Ordenes_Salida)
        {
            ReportDocument Reporte = new ReportDocument();
            String Ruta_Reporte_Crystal = "";
            String Nombre_Reporte_Generar = "";

            try
            {
                // Ruta donde se encuentra el reporte Crystal
                Ruta_Reporte_Crystal = "../Rpt/Predial/Rpt_Pre_Contrarecibo.rpt";

                // Se crea el nombre del reporte
                String Nombre_Reporte = "Rpt_Contrarecibo_" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));

                Nombre_Reporte_Generar = Nombre_Reporte + ".pdf";  // Es el nombre del reporte PDF que se va a generar
                Cls_Reportes Reportes = new Cls_Reportes();
                Reportes.Generar_Reporte(ref Ds_Reporte_Ordenes_Salida, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, "PDF");
                Mostrar_Reporte(Nombre_Reporte_Generar, "PDF");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        /// *************************************************************************************
        /// NOMBRE:              Mostrar_Reporte
        /// DESCRIPCIÓN:         Muestra el reporte en pantalla.
        /// PARÁMETROS:          Nombre_Reporte_Generar.- Nombre que tiene el reporte que se mostrará en pantalla.
        ///                      Formato.- Variable que contiene el formato en el que se va a generar el reporte "PDF" O "Excel"
        /// USUARIO CREO:        Juan Alberto Hernández Negrete.
        /// FECHA CREO:          3/Mayo/2011 18:20 p.m.
        /// USUARIO MODIFICO:    Salvador Hernández Ramírez
        /// FECHA MODIFICO:      16-Mayo-2011
        /// CAUSA MODIFICACIÓN:  Se asigno la opción para que en el mismo método se muestre el reporte en excel
        /// *************************************************************************************
        protected void Mostrar_Reporte(String Nombre_Reporte_Generar, String Formato)
        {
            String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

            try
            {
                if (Formato == "PDF")
                {
                    Pagina = Pagina + Nombre_Reporte_Generar;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt",
                    "window.open('" + Pagina + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
                }
                else if (Formato == "Excel")
                {
                    String Ruta = "../../Reporte/" + Nombre_Reporte_Generar;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Notarios
        ///DESCRIPCIÓN: Llena el Combo de Notarios
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Combo_Notarios() {
            try{
                Cls_Ope_Pre_Traslado_Negocio Traslado_Dominio = new Cls_Ope_Pre_Traslado_Negocio();
                Traslado_Dominio.P_Tipo_DataTable = "LISTAR_NOTARIOS";
                DataTable Notarios = Traslado_Dominio.Consultar_DataTable();
                DataRow Fila_Notario = Notarios.NewRow();
                Fila_Notario["NOTARIO_ID"] = HttpUtility.HtmlDecode("00000");
                Fila_Notario["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
                Notarios.Rows.InsertAt(Fila_Notario, 0);

                Cmb_Estatus.Items.Clear();
                Cmb_Estatus.Items.Add(new ListItem("<SELECCIONE>", "0"));
                //Cmb_Estatus.Items.Add(new ListItem("PENDIENTE", "PENDIENTE"));
                Cmb_Estatus.Items.Add(new ListItem("CANCELADO", "CANCELADO"));
                Cmb_Estatus.Items.Add(new ListItem("GENERADO", "GENERADO"));
                Cmb_Estatus.Items.Add(new ListItem("POR VALIDAR", "POR VALIDAR"));
                Cmb_Estatus.Items.Add(new ListItem("RECHAZADO", "RECHAZADO"));
                Cmb_Estatus.Items.Add(new ListItem("VALIDADO", "VALIDADO"));
                Cmb_Estatus.Items.Add(new ListItem("CALCULADO", "CALCULADO"));
                Cmb_Estatus.Items.Add(new ListItem("LISTO", "LISTO"));
                Cmb_Estatus.Items.Add(new ListItem("POR PAGAR", "POR PAGAR"));
                Cmb_Estatus.Items.Add(new ListItem("PAGADO", "PAGADO"));
                //Cmb_Estatus.Items.Add(new ListItem("PAGADO", "PAGADO"));
                
                //SE QUITO PESTAÑA POR ESO SE ELIMINA COMBO
                //Cmb_Notarios.DataSource = Notarios;
                //Cmb_Notarios.DataValueField = "NOTARIO_ID";
                //Cmb_Notarios.DataTextField = "NOMBRE";
                //Cmb_Notarios.DataBind();
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }                    

        #region Grid
    
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Contrarecibos
            ///DESCRIPCIÓN: Llena el Grid de Contrarecibos
            ///PARAMETROS:     
            ///             1. Pagina.  Pagina en la cual se mostrará el Grid_View
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 10/Noviembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Grid_Contrarecibos(Int32 Pagina) {
                try{
                        Cls_Ope_Pre_Traslado_Negocio Traslado_Dominio = new Cls_Ope_Pre_Traslado_Negocio();
                        Traslado_Dominio.P_Tipo_DataTable = "LISTAR_CONTRARECIBOS";
                        Traslado_Dominio.P_Buscar_Estatus = true;
                        Traslado_Dominio.P_Estatus = "GENERADO";
                        if (Session["Tipo_Busqueda"] != null) {
                            String Tipo_Busqueda = Session["Tipo_Busqueda"].ToString();
                            if (Tipo_Busqueda.Trim().Equals("CONTRARECIBOS")) {
                                if (!String.IsNullOrEmpty (Txt_Cuenta_Predial.Text.Trim()))
                                {
                                    Traslado_Dominio.P_Cuenta_Predial_ID = Txt_Cuenta_Predial.Text.Trim();
                                    Traslado_Dominio.P_Buscar_Estatus = false;
                                }
                                
                                if (!String.IsNullOrEmpty(Txt_No_Contrarecibo.Text.Trim()))
                                {
                                    Traslado_Dominio.P_Buscar_Estatus = false;
                                    Int32 Contrarecibo = 0;
                                    Int32.TryParse(Txt_No_Contrarecibo.Text.Trim(), out Contrarecibo);
                                    Traslado_Dominio.P_No_Contrarecibo = String.Format("{0:0000000000}", Contrarecibo);
                                }
                                
                                if (Txt_Fecha_Escritura.Text.Trim().Length > 0) {
                                    Traslado_Dominio.P_Buscar_Estatus = false;
                                    Traslado_Dominio.P_Buscar_Fecha_Escritura = true;
                                    Traslado_Dominio.P_Fecha_Escritura = Convert.ToDateTime(Txt_Fecha_Escritura.Text.Trim());
                                }
                                if (Txt_No_Escritura.Text.Trim().Length > 0) {
                                    Traslado_Dominio.P_Buscar_Estatus = false;
                                    Traslado_Dominio.P_Buscar_No_Escritura = true;
                                    Traslado_Dominio.P_No_Escritura = Txt_No_Escritura.Text.Trim();
                                }
                                if (Cmb_Estatus.SelectedIndex > 0)
                                {
                                    Traslado_Dominio.P_Buscar_Estatus = true;
                                    Traslado_Dominio.P_Estatus = Cmb_Estatus.SelectedValue.ToString();
                                }

                                //SE ELIMINO POR HABER QUITADO LA PESTAÑE DE LISTADO
                            //} else if (Tipo_Busqueda.Trim().Equals("LISTADOS")) {
                            //    Traslado_Dominio.P_Listadoque_ID = Txt_No_Listado.Text.Trim();
                            //    if (Txt_Fecha_Generacion.Text.Trim().Length > 0) {
                            //        Traslado_Dominio.P_Buscar_Fecha_Generacion = true;
                            //        Traslado_Dominio.P_Fecha_Generacion = Convert.ToDateTime(Txt_Fecha_Generacion.Text.Trim());
                            //    }
                            //    if (Cmb_Notarios.SelectedIndex > 0) {
                            //        Traslado_Dominio.P_Notario_ID = Cmb_Notarios.SelectedItem.Value;
                            //    }
                            }
                        }
                        Grid_Contrarecibos.Columns[2].Visible = true;
                        Grid_Contrarecibos.DataSource = Traslado_Dominio.Consultar_DataTable();
                        Grid_Contrarecibos.PageIndex = Pagina;
                        Grid_Contrarecibos.DataBind();
                        Grid_Contrarecibos.Columns[2].Visible = false;
                }catch(Exception Ex){
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }            

        #endregion

    #endregion

    #region Grids
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Limpiar_Filtros_Buscar_Contrarecibos_Click
        ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Limpieza de los filtros
        ///             para la busqueda por parte de los Contrarecibos.
        ///PARAMETROS:     
        ///CREO: Jacqueline Ramírez Sierra
        ///FECHA_CREO: 26/Agosto/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************             
            protected void Grid_Contrarecibos_PageIndexChanging(object sender, GridViewPageEventArgs e)
            {
                Grid_Contrarecibos.SelectedIndex = (-1);
                Llenar_Grid_Contrarecibos(e.NewPageIndex);
            }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Contrarecibos_RowDataBound
        ///DESCRIPCIÓN: Evento de RowDataBound del Grid de Contrarecibos
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Contrarecibos_RowDataBound(object sender, GridViewRowEventArgs e) {
            try{
                if (e.Row.RowType == DataControlRowType.DataRow) {
                    if (e.Row.Cells[3].Text == null || e.Row.Cells[3].Text.Trim().Equals("") || e.Row.Cells[3].Text.Trim()!="SIN REGISTRO" ) {
                        TextBox Caja_Texto_Cuenta = (TextBox)e.Row.Cells[9].FindControl("Txt_Establecer_Cuenta_Predial");
                        Caja_Texto_Cuenta.Visible = true;
                        ImageButton Boton_Cuenta = (ImageButton)e.Row.Cells[9].FindControl("Btn_Establecer_Cuenta_Predial");
                        Boton_Cuenta.Visible = true;
                        Boton_Cuenta.CommandArgument = e.Row.Cells[1].Text.Trim();
                        //e.Row.Cells[0].Enabled = false;
                    } else {
                        TextBox Caja_Texto_Cuenta = (TextBox)e.Row.Cells[9].FindControl("Txt_Establecer_Cuenta_Predial");
                        Caja_Texto_Cuenta.Visible = false;
                        ImageButton Boton_Cuenta = (ImageButton)e.Row.Cells[9].FindControl("Btn_Establecer_Cuenta_Predial");
                        Boton_Cuenta.Visible = false;
                        e.Row.Cells[0].Enabled = true;
                    }
                    if (e.Row.Cells[8].Text.Trim().Equals("CANCELADO")) {
                        TextBox Caja_Texto_Cuenta = (TextBox)e.Row.Cells[9].FindControl("Txt_Establecer_Cuenta_Predial");
                        Caja_Texto_Cuenta.Visible = false;
                        ImageButton Boton_Cuenta = (ImageButton)e.Row.Cells[9].FindControl("Btn_Establecer_Cuenta_Predial");
                        Boton_Cuenta.Visible = false;
                        e.Row.Cells[0].Enabled = false; 
                    }
                    if (e.Row.Cells[3].Text == null || e.Row.Cells[3].Text.Trim().Equals("") || e.Row.Cells[3].Text.Trim() != "SIN REGISTRO")
                    {
                        if (!Obtener_Dato_Consulta("ESTATUS", Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas, Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " ='" + e.Row.Cells[3].Text.Trim() + "'").Trim().Equals("PENDIENTE"))
                        {
                            TextBox Caja_Texto_Cuenta = (TextBox)e.Row.Cells[9].FindControl("Txt_Establecer_Cuenta_Predial");
                            Caja_Texto_Cuenta.Visible = false;
                            ImageButton Boton_Cuenta = (ImageButton)e.Row.Cells[9].FindControl("Btn_Establecer_Cuenta_Predial");
                            Boton_Cuenta.Visible = false;
                        }
                    }
                    if (e.Row.Cells[8].Text == "PAGADO" )                                           
                        e.Row.Cells[0].Enabled = false;
                    if (e.Row.Cells[8].Text == "CANCELADO")
                    {
                        Btn_Actualizar.Visible = false;
                    }
                    else
                    {
                        Btn_Actualizar.Visible = true;
                    }

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
        ///NOMBRE DE LA FUNCIÓN: Btn_Limpiar_Filtros_Buscar_Contrarecibos_Click
        ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Limpieza de los filtros
        ///             para la busqueda por parte de los Contrarecibos.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        protected void Btn_Limpiar_Filtros_Buscar_Contrarecibos_Click(object sender, ImageClickEventArgs e) {
            Txt_Cuenta_Predial.Text = "";
            Txt_No_Contrarecibo.Text = "";
            Txt_Fecha_Escritura.Text = "";
            Txt_No_Escritura.Text = "";
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Contrarecibos_Click
        ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Busqueda de los
        ///             Contrarecibos
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        protected void Btn_Buscar_Contrarecibos_Click(object sender, ImageClickEventArgs e) {
            Btn_Actualizar.Visible = true;
            Session["Tipo_Busqueda"] = "CONTRARECIBOS";
            Llenar_Grid_Contrarecibos(0);            
            if (Grid_Contrarecibos.Rows.Count == 0 && (Txt_Cuenta_Predial.Text.Trim().Length > 0 || Txt_No_Contrarecibo.Text.Trim().Length > 0 || Txt_Fecha_Escritura.Text.Trim().Length > 0 || Txt_No_Escritura.Text.Trim().Length > 0)) {
                Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con los filtros establecidos no se encotrarón coincidencias";
                Lbl_Mensaje_Error.Text = "(Se cargaron todos los contrarecibos almacenados y se limpiaron los filtros)";
                Div_Contenedor_Msj_Error.Visible = true;
                Txt_Cuenta_Predial.Text = "";
                Txt_No_Contrarecibo.Text = "";
                Txt_Fecha_Escritura.Text = "";
                Txt_No_Escritura.Text = "";
                //Cmb_Estatus.SelectedIndex = 0;
                Session.Remove("Tipo_Busqueda");
                Llenar_Grid_Contrarecibos(0);
                //Cmb_Estatus.SelectedIndex = -1;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Establecer_Cuenta_Predial_Click
        ///DESCRIPCIÓN: Maneja el Evento del Boton para establecer la cuenta de Predial
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        protected void Btn_Establecer_Cuenta_Predial_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                String No_Contrarecibo = ((ImageButton)sender).CommandArgument.Trim();
                if (Grid_Contrarecibos.Rows.Count > 0 && No_Contrarecibo.Trim().Length > 0)
                {
                    for (Int32 Contador = 0; Contador < Grid_Contrarecibos.Rows.Count; Contador++)
                    {
                        if (Grid_Contrarecibos.Rows[Contador].Cells[1].Text.Trim().Equals(No_Contrarecibo))
                        {
                            TextBox Text_Temporal = (TextBox)Grid_Contrarecibos.Rows[Contador].Cells[9].FindControl("Txt_Establecer_Cuenta_Predial");
                            //String Cuenta_Predial_ID = Obtener_Cuenta_Predial_ID(Text_Temporal.Text.Trim());
                            if (!Validar_Cuenta_Existente(Text_Temporal.Text.Trim()))
                            {
                                String Cuenta_Predial_ID = Crear_Cuenta_Predial_ID(Text_Temporal.Text.Trim());
                                if (Cuenta_Predial_ID != "")
                                {
                                    if (Cuenta_Predial_ID.Trim().Length > 0)
                                    {
                                        Cls_Ope_Pre_Traslado_Negocio Traslado_Dominio = new Cls_Ope_Pre_Traslado_Negocio();
                                        Traslado_Dominio.P_No_Contrarecibo = No_Contrarecibo;
                                        Traslado_Dominio.P_Cuenta_Predial_ID = Cuenta_Predial_ID;
                                        Traslado_Dominio.P_Estatus = "PENDIENTE";
                                        Traslado_Dominio.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                                        Traslado_Dominio.Modificar_Contrarecibo();
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Operacion_Predial_Traslado", "alert('Cuenta Registrada Exitosamente!')", true);
                                        Llenar_Grid_Contrarecibos(Grid_Contrarecibos.PageIndex);
                                    }
                                    else
                                    {
                                        Lbl_Ecabezado_Mensaje.Text = "Para la Actualización de la cuenta del Contrarecibo '" + No_Contrarecibo + "' es necesario Introducir la Cuenta";
                                        Lbl_Mensaje_Error.Text = "";
                                        Div_Contenedor_Msj_Error.Visible = true;
                                    }
                                }
                                else
                                {
                                    Lbl_Ecabezado_Mensaje.Text = "La Cuenta Predial no pudo ser creada";
                                    //Lbl_Ecabezado_Mensaje.Text = "La Cuenta Predial indicada no existe";
                                    Lbl_Mensaje_Error.Text = "";
                                    Div_Contenedor_Msj_Error.Visible = true;
                                }
                            }
                            else 
                            {
                                Lbl_Ecabezado_Mensaje.Text = "La Cuenta Predial ya existe, introduzca otro número de Cuenta Predial";
                                //Lbl_Ecabezado_Mensaje.Text = "La Cuenta Predial indicada no existe";
                                Lbl_Mensaje_Error.Text = "";
                                Div_Contenedor_Msj_Error.Visible = true;
                            }
                            break;
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
        ///NOMBRE DE LA FUNCIÓN : Obtener_Cuenta_Predial_ID
        ///DESCRIPCIÓN          : En base al número de cuenta, obtiene el Id del catálogo
        ///PARAMETROS           : Cuenta_Predial, String con el valor del No. de Cuenta
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 10/Diciembre/2010
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        private String Obtener_Cuenta_Predial_ID(String Cuenta_Predial)
        {
            Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
            DataTable Dt_Cuentas_Predial;
            String Cuenta_Predial_ID = "";

            Cuentas_Predial.P_Campos_Dinamicos = Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
            Cuentas_Predial.P_Filtros_Dinamicos = Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " = '" + Cuenta_Predial + "'";
            Dt_Cuentas_Predial = Cuentas_Predial.Consultar_Cuenta();
            if (Dt_Cuentas_Predial.Rows.Count > 0)
            {
                Cuenta_Predial_ID = Convert.ToString(Dt_Cuentas_Predial.Rows[0][0]);
            }
            return Cuenta_Predial_ID;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Crear_Cuenta_Predial_ID
        ///DESCRIPCIÓN          : Se crea la cuenta en la tabla Cat_Pre_Cuentas_Predial
        ///PARAMETROS           : Cuenta_Predial, String con el valor del No. de Cuenta
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 15/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        private String Crear_Cuenta_Predial_ID(String Cuenta_Predial)
        {
            Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
            String Cuenta_Predial_ID = "";

            Cuentas_Predial.P_Cuenta_Predial = Cuenta_Predial;
            Cuentas_Predial.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
            Cuentas_Predial.P_Estatus = "PENDIENTE";
            if (Cuentas_Predial.Alta_Cuenta())
            {
                DataTable Dt_Cuentas_Predial;
                Cuentas_Predial.P_Campos_Dinamicos = Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                Cuentas_Predial.P_Cuenta_Predial = Cuenta_Predial.ToUpper();
                Dt_Cuentas_Predial = Cuentas_Predial.Consultar_Cuenta();
                if (Dt_Cuentas_Predial != null)
                {
                    if (Dt_Cuentas_Predial.Rows.Count > 0)
                    {
                        Cuenta_Predial_ID = Dt_Cuentas_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
                    }
                }
            }
            return Cuenta_Predial_ID;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Validar_Cuenta_Existente
        ///DESCRIPCIÓN          : Se crea la cuenta en la tabla Cat_Pre_Cuentas_Predial
        ///PARAMETROS           : Cuenta_Predial, String con el valor del No. de Cuenta
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 15/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        private Boolean Validar_Cuenta_Existente(String Cuenta_Predial)
        {
            Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
            String Cuenta_Predial_ID = "";
            Boolean Existe = false;

            Cuentas_Predial.P_Cuenta_Predial = Cuenta_Predial;
            Existe=Cuentas_Predial.Consultar_Cuenta_Existente();
            return Existe;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Listado_Click
        ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Busqueda de los
        ///             Listados
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        protected void Btn_Buscar_Listado_Click(object sender, ImageClickEventArgs e) {
            Session["Tipo_Busqueda"] = "LISTADOS";
            Llenar_Grid_Contrarecibos(0);
            //if (Grid_Contrarecibos.Rows.Count == 0 && (Txt_No_Listado.Text.Trim().Length > 0 || Txt_Fecha_Generacion.Text.Trim().Length > 0 || Cmb_Notarios.SelectedIndex > 0)) {
            //    Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con los filtros establecidos no se encontraron coincidencias";
            //    Lbl_Mensaje_Error.Text = "(Se cargaron todos los contrarecibos almacenados y se limpiaron los filtros)";
            //    Div_Contenedor_Msj_Error.Visible = true;
            //    Txt_No_Listado.Text = "";
            //    Txt_Fecha_Generacion.Text = "";
            //    Cmb_Notarios.SelectedIndex = 0;
            //    Session.Remove("Tipo_Busqueda");
            //    Llenar_Grid_Contrarecibos(0);
            //}
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Limpiar_Filtros_Buscar_Listado_Click
        ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Limpieza de los filtros
        ///             para la busqueda por parte de los Listados.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 09/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        //protected void Btn_Limpiar_Filtros_Buscar_Listado_Click(object sender, ImageClickEventArgs e) {
        //    Txt_No_Listado.Text = "";
        //    Txt_Fecha_Generacion.Text = "";
        //    Cmb_Notarios.SelectedIndex = 0;
        //}

    #endregion

            protected void Grid_Contrarecibos_SelectedIndexChanged(object sender, EventArgs e)
        {            
            Session["Contrarecibo_Traslado"] = Grid_Contrarecibos.Rows[Grid_Contrarecibos.SelectedIndex].Cells[1].Text;
            Session["Estatus_Traslado"] = Grid_Contrarecibos.Rows[Grid_Contrarecibos.SelectedIndex].Cells[8].Text;
            //if ( !String.IsNullOrEmpty( Session["Contrarecibo_Traslado"].ToString() ) )
            //Response.Redirect("../Predial/Frm_Ope_Pre_Orden_Variacion.aspx");
        }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Limpiar_Filtros_Buscar_Listado_Click
            ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Limpieza de los filtros
            ///             para la busqueda por parte de los Listados.
            ///PARAMETROS:     
            ///CREO: FJacqueline Ramirez Sierra
            ///FECHA_CREO: 29/Septiembre/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************  
            protected void Btn_Guardar_Click(object sender, ImageClickEventArgs e)
            {
                Cls_Ope_Pre_Orden_Variacion_Negocio Orden = new Cls_Ope_Pre_Orden_Variacion_Negocio();
                string Estatus_Contrarecibo;
                string Orden_Contrarecibo;
                string Orden_Contrarecibo_Anio;
                bool Cancelado = true;
                
                    if (Grid_Contrarecibos.SelectedIndex > (-1))
                    {
                        if (Btn_Actualizar.AlternateText == "Modificar")
                        {
                            Estado_Botones(Const_Estado_Modificar);
                        }
                        else if (Btn_Actualizar.AlternateText == "Guardar")
                        {
                            
                            if (!String.IsNullOrEmpty(Grid_Contrarecibos.SelectedDataKey["CUENTA_PREDIAL"].ToString()))
                            {
                                if (Hdn_Respuesta_Confirmacion.Value == "true")
                                {
                                Cls_Ope_Pre_Traslado_Negocio Estatus = new Cls_Ope_Pre_Traslado_Negocio();
                                Estatus.P_No_Contrarecibo = Session["Contrarecibo_Traslado"].ToString();
                                Estatus.P_Estatus_Anterior = Grid_Contrarecibos.SelectedDataKey["ESTATUS"].ToString();
                                Estatus.P_Estatus = Cmb_Estatus.SelectedItem.Text.Trim();
                                if (Valor_Estatus(Grid_Contrarecibos.SelectedDataKey["ESTATUS"].ToString()) - Valor_Estatus(Cmb_Estatus.SelectedValue.ToString()) > 0 && !Grid_Contrarecibos.SelectedDataKey["ESTATUS"].ToString().Contains("CANCELADO") && Grid_Contrarecibos.SelectedDataKey["ESTATUS"].ToString()!= "PAGADO")
                                {
                                    Estatus_Contrarecibo = Grid_Contrarecibos.SelectedDataKey["ESTATUS"].ToString();
                                    //SE COMENTO DEBIDO A QUE FUNCIONALIDAD DE  "DES-CANCELAR CR NO ESTA COMPLETA Y NO SE AH REVISADO CON CLIENTE"
                                    //if (Estatus.P_Estatus == "CANCELADO")
                                    //{
                                    //    if (!Estatus.P_Estatus_Anterior.Contains('/'))
                                    //    {
                                    //        Estatus.P_Estatus = Cmb_Estatus.SelectedItem.Text.Trim() + "/" + Estatus_Contrarecibo;
                                    //        //Estatus_Contrarecibo = (Estatus.P_Estatus_Anterior.Split('/'))[1].ToString();
                                    //    }                                        
                                    //}
                                    //else if (Estatus.P_Estatus_Anterior.Contains('/'))
                                    //{
                                    //    if (Estatus.P_Estatus != (Estatus.P_Estatus_Anterior.Split('/'))[1].ToString())
                                    //        Cancelado = false;
                                    //}
                                    if (Cancelado)
                                    {

                                        Estatus.Modificar_Contrarecibo_Estatus();
                                        //if (Estatus.P_No_Contrarecibo.Contains('/'))
                                        //{
                                            //Orden_Contrarecibo = Estatus.P_No_Contrarecibo.Split('/')[0].ToString();
                                            //Orden_Contrarecibo_Anio = Estatus.P_No_Contrarecibo.Split('/')[1].ToString();
                                            //Orden.P_Contrarecibo = Orden_Contrarecibo;
                                            //Orden.P_Contrarecibo_Anio = Convert.ToInt16 (Orden_Contrarecibo_Anio);
                                            //Orden.Eliminar_Orden();
                                        //}
                                        
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Operacion_Predial_Traslado", "alert('El Estatus fue Modificado Exitosamente!')", true);
                                        Estado_Botones(Const_Estado_Inicial);
                                        Llenar_Grid_Contrarecibos(Grid_Contrarecibos.PageIndex);
                                        //LIMPIA SESSION DE BUSQUEDA
                                        Cmb_Estatus.SelectedIndex = 0;
                                        Session.Remove("Tipo_Busqueda");
                                    }
                                    else
                                    {
                                        Estado_Botones(Const_Estado_Inicial);
                                        Div_Contenedor_Msj_Error.Visible = true;
                                        IBtn_Imagen_Error.Visible = true;
                                        Lbl_Ecabezado_Mensaje.Text = "";
                                        Lbl_Mensaje_Error.Text = "No es posible realizar el cambio de estatus de ";
                                        Lbl_Mensaje_Error.Text += Grid_Contrarecibos.SelectedDataKey["ESTATUS"].ToString() + " a " + Cmb_Estatus.SelectedValue.ToString();
                                    }
                                }
                                else
                                {
                                    Estado_Botones(Const_Estado_Inicial);
                                    Div_Contenedor_Msj_Error.Visible = true;
                                    IBtn_Imagen_Error.Visible = true;
                                    Lbl_Ecabezado_Mensaje.Text = "";
                                    Lbl_Mensaje_Error.Text = "No es posible realizar el cambio de estatus de ";
                                    Lbl_Mensaje_Error.Text += Grid_Contrarecibos.SelectedDataKey["ESTATUS"].ToString() + " a " + Cmb_Estatus.SelectedValue.ToString();
                                }
                            }
                        }
                            else
                            {
                                Estado_Botones(Const_Estado_Inicial);
                                Div_Contenedor_Msj_Error.Visible = true;
                                IBtn_Imagen_Error.Visible = true;
                                Lbl_Ecabezado_Mensaje.Text = "Es necesario:";
                                Lbl_Mensaje_Error.Text = "+ Seleccionar un contrarecibo con una cuenta generada";
                            }
                        }
                    }
                    else
                    {
                        Estado_Botones(Const_Estado_Inicial);
                        Div_Contenedor_Msj_Error.Visible = true;
                        IBtn_Imagen_Error.Visible = true;
                        Lbl_Ecabezado_Mensaje.Text = "Es necesario:";
                        Lbl_Mensaje_Error.Text = "+ Seleccionar un contrarecibo";
                    }
                

            }
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN:Btn_Salir_Click
            ///DESCRIPCIÓN: Salir del formulario
            ///PARAMETROS:     
            ///CREO: Jesus Toledo Rodriguez
            ///FECHA_CREO: 12/Nov/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************  
            protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
            {
                if (Btn_Salir.AlternateText == "Inicio")
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");   // ir a pagina principal
                else
                {                    
                    Estado_Botones(Const_Estado_Inicial);
                    Llenar_Grid_Contrarecibos(Grid_Contrarecibos.PageIndex);
                    //LIMPIA SESSION DE BUSQUEDA
                    Cmb_Estatus.SelectedIndex = 0;
                    Session.Remove("Tipo_Busqueda");
                }
            }
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Imprimir_Click
            ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la impresion del contrarecibo
            ///PARAMETROS:     
            ///CREO: Jesus Toledo Rodriguez
            ///FECHA_CREO: 12/Nov/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************  
            protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
            {               
                if (Grid_Contrarecibos.SelectedIndex > (-1))
                {
                    //Consulta de datos de Contrarecibo
                    Generar_Reporte(Grid_Contrarecibos.SelectedDataKey["NO_CONTRARECIBO"].ToString());
                }
                else
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    IBtn_Imagen_Error.Visible = true;
                    Lbl_Ecabezado_Mensaje.Text = "Es necesario:";
                    Lbl_Mensaje_Error.Text = "+ Seleccionar un contrarecibo";
                }
            }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Valor_Estatus
    ///DESCRIPCIÓN: Funcion que asigna valor a los estatus para validar el cambio de este
    ///PARAMETROS:     
    ///CREO: Jesus Toledo Rodriguez
    ///FECHA_CREO: 06/Dic/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
            public int Valor_Estatus(String P_Estatus)
            {
                int     Valor_Estatus = 0;
                string  Estatus;

                Estatus = P_Estatus;
                if ( P_Estatus.Contains('/') )
                    Estatus = (P_Estatus.Split('/'))[1].ToString();
                switch( P_Estatus )
                {
                    case "PENDIENTE":
                        Valor_Estatus = 0;
                    break;
                    case "CANCELADO":
                        Valor_Estatus = 0;
                    break;                    
                    case "GENERADO":
                    Valor_Estatus = 1;
                    break;
                    case "POR VALIDAR":
                    Valor_Estatus = 2;
                    break;
                    case "RECHAZADO":
                    Valor_Estatus = 3;
                    break;
                    case "VALIDADO":
                    Valor_Estatus = 4;
                    break;
                    case "CALCULADO":
                    Valor_Estatus = 5;
                    break;
                    case "LISTO":
                    Valor_Estatus = 6;
                    break;
                    case "POR PAGAR":
                    Valor_Estatus = 7;
                    break;
                    case "PAGADO":
                    Valor_Estatus = 8;
                    break;
                }
                return Valor_Estatus;
            }
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Obtener_Dato_Consulta
            ///DESCRIPCIÓN          : Consulta el Campo dado de la Tabla Indicada
            ///PARAMETROS:     
            ///CREO                 : Antonio Salvador Benvides Guardado
            ///FECHA_CREO           : 24/Agosto/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private String Obtener_Dato_Consulta(String Campo, String Tabla, String Condiciones)
            {
                String Mi_SQL;
                String Dato_Consulta = "";

                try
                {
                    Mi_SQL = "SELECT " + Campo;
                    if (Tabla != "")
                    {
                        Mi_SQL += " FROM " + Tabla;
                    }
                    if (Condiciones != "")
                    {
                        Mi_SQL += " WHERE " + Condiciones;
                    }

                    OracleDataReader Dr_Dato = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                    if (Dr_Dato.Read())
                    {
                        if (Dr_Dato[0] != null)
                        {
                            Dato_Consulta = Dr_Dato[0].ToString();
                        }
                        else
                        {
                            Dato_Consulta = "";
                        }
                        Dr_Dato.Close();
                    }
                    else
                    {
                        Dato_Consulta = "";
                    }
                    if (Dr_Dato != null)
                    {
                        Dr_Dato.Close();
                    }
                    Dr_Dato = null;
                }
                catch
                {
                }
                finally
                {
                }

                return Dato_Consulta;
            }
}
