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
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Folios_Inutilizados_General_Negocio;
using Presidencia;
using Presidencia.Empleados.Negocios;
using System.Text;

public partial class paginas_Predial_Frm_Ope_Caj_Folios_Inutilizados_General : System.Web.UI.Page
{
    #region PAGELOAD
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
                if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

                if (!IsPostBack)
                {
                    //Configuracion_Acceso("Frm_Ope_Caj_Folios_Inutilizados_General.aspx");
                    //Response.Redirect("..//Predial/Impresion_Recibos/Frm_Ope_Pre_Impresion_Recibo.aspx?Referencia=TD642011");
                    Folios_Inutilizados_Inicio();
                }
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
            Div_Contenedor_Msj_Error.Visible = false;
        }
    #endregion

    #region METODOS
        //*******************************************************************************
        //NOMBRE DE LA FUNCIÓN : Folios_Inutilizados_Inicio
        //DESCRIPCIÓN          : Metodo para configurar el inicio de la pagina
        //PARAMETROS           :   
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 21/octubre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //*******************************************************************************
        private void Folios_Inutilizados_Inicio() {
            Limpiar_Controles();
            Cmb_Caja.Enabled = true;
            Llenar_Combo_Modulo();
        }

        //*******************************************************************************
        //NOMBRE DE LA FUNCIÓN : Limpiar_Controles
        //DESCRIPCIÓN          : Metodo para limpiar los controles de la pagina
        //PARAMETROS           :   
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 21/octubre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //*******************************************************************************
        private void Limpiar_Controles()
        {
            Txt_Empleado.Text = "";
            Txt_Fecha_Fin.Text = "";
            Txt_Fecha_Inicio.Text = "";
            Cmb_Modulo.SelectedIndex = -1;
            Cmb_Caja.SelectedIndex = -1;
        }

        //*******************************************************************************
        //NOMBRE DE LA FUNCIÓN : Llenar_Combo_Modulo
        //DESCRIPCIÓN          : Metodo para llenar el combo con los modulos existentes
        //PARAMETROS           :   
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 21/octubre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //*******************************************************************************
        private void Llenar_Combo_Modulo()
        {
            Cls_Ope_Caj_Folios_Inutilizados_General_Negocio Folios_Negocio = new Cls_Ope_Caj_Folios_Inutilizados_General_Negocio();
            DataTable Dt_Modulos = new DataTable();
            try
            {
                Dt_Modulos = Folios_Negocio.Consultar_Modulo();
                DataRow Fila_Modulo = Dt_Modulos.NewRow();
                Fila_Modulo["MODULO_ID"] = HttpUtility.HtmlDecode("00000");
                Fila_Modulo["DESCRIPCION"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
                Dt_Modulos.Rows.InsertAt(Fila_Modulo, 0);
                Cmb_Modulo.DataValueField = "MODULO_ID";
                Cmb_Modulo.DataTextField = "DESCRIPCION";
                Cmb_Modulo.DataSource = Dt_Modulos;
                Cmb_Modulo.DataBind();
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al tratar de llenar el combo de modulos Error:[" + Ex.Message + "]");
            }
        }

        //*******************************************************************************
        //NOMBRE DE LA FUNCIÓN : Llenar_Combo_Caja
        //DESCRIPCIÓN          : Metodo para llenar el combo con las cajas existentes de un modulo
        //PARAMETROS           :   
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 21/octubre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //*******************************************************************************
        private void Llenar_Combo_Caja(String Modulo_ID)
        {
            Cls_Ope_Caj_Folios_Inutilizados_General_Negocio Folios_Negocio = new Cls_Ope_Caj_Folios_Inutilizados_General_Negocio();
            DataTable Dt_Cajas = new DataTable();
            try
            {
                Folios_Negocio.P_Modulo_ID = Modulo_ID;
                Dt_Cajas = Folios_Negocio.Consultar_Caja();
                if (Dt_Cajas.Rows.Count > 0){
                    Cmb_Caja.DataValueField = "CAJA_ID";
                    Cmb_Caja.DataTextField = "NO_CAJA";
                    Cmb_Caja.DataSource = Dt_Cajas;
                    Cmb_Caja.DataBind();
                    Cmb_Caja.Items.Insert(0, new ListItem("<-- Seleccione -->", String.Empty));
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al tratar de llenar el combo de cajas Error:[" + Ex.Message + "]");
            }
        }

        //*******************************************************************************
        //NOMBRE DE LA FUNCIÓN : Llena_Grid_Empleados
        //DESCRIPCIÓN          : Metodo para llenar el grid con los empleados encontrados en la busqueda
        //PARAMETROS           :   
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 26/octubre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //*******************************************************************************
        private void Llena_Grid_Empleados()
        {
            DataTable Dt_Empleados; //Variable que obtendra los datos de la consulta 
            try
            {
                Grid_Empleados.Columns[1].Visible = true;
                Grid_Empleados.DataBind();
                Dt_Empleados = (DataTable)Session["Consulta_Empleados"];
                Grid_Empleados.DataSource = Dt_Empleados;
                Grid_Empleados.DataBind();
                Grid_Empleados.Columns[1].Visible = false;
                Grid_Empleados.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                throw new Exception("Llena_Grid_Empleados " + ex.Message.ToString(), ex);
            }
        }

        //*******************************************************************************
        //NOMBRE DE LA FUNCIÓN : Validar_Busqueda
        //DESCRIPCIÓN          : Metodo para validar los datos de la busqueda del empleado
        //PARAMETROS           :   
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 26/octubre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //*******************************************************************************
        private Boolean Validar_Busqueda() {
            Boolean Datos_Validos;
            Datos_Validos = true;
            try {
                if (string.IsNullOrEmpty(Txt_Busqueda_No_Empleado.Text) && string.IsNullOrEmpty(Txt_Busqueda_Nombre_Empleado.Text))
                {
                    Lbl_Error_Busqueda.Text = "Por favor introduce un No. Empleado o el nombre del Empleado para hacer la busqueda";
                    Lbl_Error_Busqueda.Style.Add("display", "block");
                    Img_Error_Busqueda.Style.Add("display", "block");
                    Datos_Validos = false;
                }
            }catch(Exception Ex){
                throw new Exception("Error al validar los datos de la busqueda Error:[" + Ex.Message + "]");
            }
            return Datos_Validos;
        }
    #endregion

    #region EVENTOS
        //*******************************************************************************
        //NOMBRE DE LA FUNCIÓN : Cmb_Modulo_SelectedIndexChanged
        //DESCRIPCIÓN          : Evento del combo de modulos
        //PARAMETROS           :   
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 21/octubre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //*******************************************************************************
        protected void Cmb_Modulo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Modulo_Id;
            try 
            {
                Modulo_Id = Cmb_Modulo.SelectedValue;
                Llenar_Combo_Caja(Modulo_Id);
            }
            catch(Exception Ex){
                throw new Exception("Error al generar el evento del combo de modulos Error:["+Ex.Message +"]");
            }
        }

        //*******************************************************************************
        //NOMBRE DE LA FUNCIÓN : Btn_Imprimir_Click
        //DESCRIPCIÓN          : Evento del boton de imprimir
        //PARAMETROS           :   
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 21/octubre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //*******************************************************************************
        protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
        {
            Cls_Ope_Caj_Folios_Inutilizados_General_Negocio Folios_Negocio = new Cls_Ope_Caj_Folios_Inutilizados_General_Negocio();
            Ds_Ope_Caj_Folios_Inutilizados_General Ds_Folios = new Ds_Ope_Caj_Folios_Inutilizados_General();
            DataTable Dt_Folios = new DataTable();
            try
            {
                //obtenemos los valores de las cajas de texto
                if (Cmb_Modulo.SelectedIndex > 0) {
                    Folios_Negocio.P_Modulo_ID = Cmb_Modulo.SelectedValue;
                }
                if (Cmb_Caja.SelectedIndex > 0) {
                    Folios_Negocio.P_Caja_ID = Cmb_Caja.SelectedValue;
                }
                if (!string.IsNullOrEmpty(Txt_Empleado.Text)) {
                    Folios_Negocio.P_Empleado_ID = HF_Empleado_ID.Value;
                }
                if (!string.IsNullOrEmpty(Txt_Fecha_Inicio.Text))
                {
                    Folios_Negocio.P_Fecha_Inicio = string.Format("{0:dd/MM/yyyy}", Txt_Fecha_Inicio.Text);
                }
                if (!string.IsNullOrEmpty(Txt_Fecha_Fin.Text))
                {
                    Folios_Negocio.P_Fecha_Fin = string.Format("{0:dd/MM/yyyy}", Txt_Fecha_Fin.Text);
                }
                Dt_Folios = Folios_Negocio.Consultar_Folio();

                if (Dt_Folios.Rows.Count > 0)
                {
                    Dt_Folios.TableName = "Dt_Folios_Inutilizados";
                    Ds_Folios.Clear();
                    Ds_Folios.Tables.Clear();
                    Ds_Folios.Tables.Add(Dt_Folios.Copy());
                    Imprimir_Reporte(Ds_Folios, "Rpt_Ope_Caj_Folios_Inutilizados_General.rpt", "Reporte de Folios Inutilizados");
                }
                else {
                    Lbl_Mensaje_Error.Text = "No se encontraron registros";
                    Lbl_Mensaje_Error.Visible = true;
                }
            }
            catch(Exception Ex) {
                throw new Exception("Error en el evento del boton de imprimir Error:["+Ex.Message +"]");
            }
        }
        
        //*******************************************************************************
        //NOMBRE DE LA FUNCIÓN : Btn_Salir_Click
        //DESCRIPCIÓN          : Evento del boton de salir
        //PARAMETROS           :   
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 21/octubre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //*******************************************************************************
        protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
        {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }

        //*******************************************************************************
        //NOMBRE DE LA FUNCIÓN : Btn_Busqueda_Empleados_Click
        //DESCRIPCIÓN          : Evento del boton de busquedas de empleados
        //PARAMETROS           :   
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 26/octubre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //*******************************************************************************
        protected void Btn_Busqueda_Empleados_Click(object sender, EventArgs e)
        {
            Cls_Cat_Empleados_Negocios Rs_Consulta_Ca_Empleados = new Cls_Cat_Empleados_Negocios(); //Variable de conexión hacia la capa de Negocios
            DataTable Dt_Empleados; //Variable que obtendra los datos de la consulta 
            Int32 No_Letras = 0;
            String No_Empleado = String.Empty;

            try
            {
                if (Validar_Busqueda())
                {
                    if (!string.IsNullOrEmpty(Txt_Busqueda_No_Empleado.Text.Trim()))
                    {
                        No_Letras = Txt_Busqueda_No_Empleado.Text.Trim().Length; //obtenemos el no de letras que tiene el numero de empleado 

                        if (No_Letras < 6)
                        {
                            for (Int32 i = 0; i < 6 - No_Letras; i++)
                            {
                                No_Empleado += "0";
                            }
                        }
                        Rs_Consulta_Ca_Empleados.P_No_Empleado = No_Empleado + Txt_Busqueda_No_Empleado.Text.Trim();
                    }
                    if (!string.IsNullOrEmpty(Txt_Busqueda_Nombre_Empleado.Text.Trim())) Rs_Consulta_Ca_Empleados.P_Nombre = Txt_Busqueda_Nombre_Empleado.Text.Trim();
                    Rs_Consulta_Ca_Empleados.P_Estatus = "ACTIVO";
                    Dt_Empleados = Rs_Consulta_Ca_Empleados.Consulta_Empleados_General(); //Consulta todos los Empleados que coindican con lo proporcionado por el usuario
                    Session["Consulta_Empleados"] = Dt_Empleados;
                    Llena_Grid_Empleados();
                    Mpe_Busqueda_Empleados.Show();
                    Lbl_Error_Busqueda.Style.Add("display", "none");
                    Img_Error_Busqueda.Style.Add("display", "none");

                    if (Dt_Empleados is DataTable)
                        Lbl_Numero_Registros.Text = "Registros Encontrados: [" + Dt_Empleados.Rows.Count + "]";
                    else
                        Lbl_Numero_Registros.Text = "Registros Encontrados: [0]";
                }
                else {
                    Mpe_Busqueda_Empleados.Show();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Consulta_Empleados " + ex.Message.ToString(), ex);
            }
        }
    #endregion

    #region REPORTE
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN : Imprimir_Reporte
        ///DESCRIPCIÓN          : Genera el reporte de Crystal con los datos proporcionados en el DataTable y lo manda a la impresora default
        ///PARAMETROS: 
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 23/Julio/2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        private void Imprimir_Reporte(DataSet Ds, String Nombre_Reporte, String Nombre_Archivo)
        {
            ReportDocument Reporte = new ReportDocument();
            String File_Path = Server.MapPath("../Rpt/Cajas/" + Nombre_Reporte);
            try
            {
                Reporte.Load(File_Path);
                Reporte.SetDataSource(Ds);
            }
            catch
            {
                Lbl_Mensaje_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No se pudo cargar el reporte para su impresión";
            }

            String Archivo_PDF = Nombre_Archivo + ".pdf";  // Es el nombre del PDF que se va a generar    
            try
            {
                ExportOptions Export_Options = new ExportOptions();
                DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
                Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Archivo_PDF);
                Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
                Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
                Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
                Reporte.Export(Export_Options);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error " + Ex.Message);
            }

            try
            {
                Mostrar_Reporte(Archivo_PDF);
            }
            catch (Exception Ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Ex.Message.ToString();
            }
        }

        //*******************************************************************************
        //NOMBRE DE LA FUNCIÓN : Mostrar_Reporte
        //DESCRIPCIÓN          : Metodo para mostrar el reporte
        //PARAMETROS           1 Nombre_Reporte: Nombre que tiene el reporte que se mostrara en pantalla.   
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 25/octubre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //*******************************************************************************
        protected void Mostrar_Reporte(String Nombre_Reporte)
        {
            String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

            try
            {
                Pagina = Pagina + Nombre_Reporte;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt_Folios_Inutilizados",
                    "window.open('" + Pagina + "', 'Reportes','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
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
                Botones.Add(Btn_Imprimir);
                Botones.Add(Btn_Salir);
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
                                Cls_Util.Configuracion_Acceso_Sistema_SIAS(Botones, Dr_Menus[0]);//Habilitamos la configuracón de los botones.
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
                throw new Exception("Error al Validar si es un dato numerico. Error [" + Ex.Message + "]");
            }
            return Resultado;
        }
    #endregion

    #region GRID
        //*******************************************************************************
        //NOMBRE DE LA FUNCIÓN : Grid_Empleados_SelectedIndexChanged
        //DESCRIPCIÓN          : Evento de seleccion de un registro del del grid
        //PARAMETROS           :   
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 26/octubre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //*******************************************************************************
        protected void Grid_Empleados_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                HF_Empleado_ID.Value = Grid_Empleados.SelectedRow.Cells[1].Text;
                Txt_Empleado.Text  = Grid_Empleados.SelectedRow.Cells[3].Text;
                Mpe_Busqueda_Empleados.Hide();
                Lbl_Error_Busqueda.Text = "";
                Lbl_Error_Busqueda.Style.Add("display", "none");
                Img_Error_Busqueda.Style.Add("display", "none");
                Txt_Busqueda_Nombre_Empleado.Text = "";
                Txt_Busqueda_No_Empleado.Text = "";
                Grid_Empleados.DataBind();
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
    #endregion
}
