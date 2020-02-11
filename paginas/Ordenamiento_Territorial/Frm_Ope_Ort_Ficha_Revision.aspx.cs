using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Ordenamiento_Territorial_Ficha_Revision.Negocio;
using Presidencia.Ordenamiento_Territorial_Zonas.Negocio;
using Presidencia.Areas.Negocios;
using System.Drawing.Drawing2D;

public partial class paginas_Ordenamiento_Territorial_Frm_Ope_Ort_Ficha_Revision : System.Web.UI.Page
{
    #region Page_Load

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN         : Metodo que se carga cada que ocurre un PostBack de la Página
        ///PARÁMETROS          :
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///*******************************************************************************
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) 
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Cls_Sessiones.Ciudadano_ID = "";
                Llenar_Combo_Areas("");
                Llenar_Combo_Zonas();
                Configuracion_Formulario(true);

                if (Session["Opinion_Solicitud_Id"] != null && Session["Opinion_Solicitud_Id"].ToString() != "")
                {
                    Llenar_Grid_Opiniones();
                    Session.Remove("Opinion_Solicitud_Id");
                }
                else
                {
                    Llenar_Grid_Listado(0);
                }
                // registro de scripts del lado del servidor para mostrar ventanas emergentes para búsqueda avanzada
                string Ventana_Modal = "Abrir_Ventana_Modal('../Tramites/Ventanas_Emergente/Frm_Busqueda_Avanzada_Solicitud_Tramite.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                Btn_Buscar_Solicitud.Attributes.Add("OnClick", Ventana_Modal);

            }
            Div_Contenedor_Msj_Error.Visible = false;
        }

    #endregion

    #region Metodos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Zonas
        ///DESCRIPCIÓN         : Se llena el Combo de las Areas.
        ///PARÁMETROS          :
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///******************************************************************************* 
        private void Llenar_Combo_Areas(String Area_ID)
        {
            Cls_Cat_Areas_Negocio Negocio = new Cls_Cat_Areas_Negocio();
            Negocio.P_Area_ID = Area_ID;
            DataTable Dt_Areas = Negocio.Consulta_Areas();
            Cmb_Area.DataSource = Dt_Areas;
            Cmb_Area.DataTextField = "NOMBRE";
            Cmb_Area.DataValueField = "AREA_ID";
            Cmb_Area.DataBind();
            Cmb_Area.Items.Insert(0, new ListItem("< - - SELECCIONE - - >", ""));
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Zonas
        ///DESCRIPCIÓN         : Se llena el Combo de las Zonas.
        ///PARÁMETROS          :
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///******************************************************************************* 
        private void Llenar_Combo_Zonas()
        {
            Cls_Cat_Ort_Zona_Negocio Negocio = new Cls_Cat_Ort_Zona_Negocio();
            DataTable Dt_Zonas = Negocio.Consultar_Zonas();
            Cmb_Zona.DataSource = Dt_Zonas;
            Cmb_Zona.DataTextField = "NOMBRE";
            Cmb_Zona.DataValueField = "ZONA_ID";
            Cmb_Zona.DataBind();
            Cmb_Zona.Items.Insert(0, new ListItem("< - - SELECCIONE - - >", ""));
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
        ///DESCRIPCIÓN         : Carga una configuracion de los controles del Formulario
        ///PARÁMETROS          : 1. Estatus. Estatus en el que se cargara la configuración de los
        ///                      controles.
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///*******************************************************************************
        private void Configuracion_Formulario(Boolean Estatus)
        {
            Btn_Nuevo.Visible = true;
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            Btn_Modificar.Visible = true;
            Btn_Modificar.AlternateText = "Modificar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            Cmb_Area.Enabled = !Estatus;
            Cmb_Zona.Enabled = !Estatus;
            Txt_Observacion.Enabled = !Estatus;
            Txt_Respuesta.Enabled = false;
            Grid_Listado.Enabled = Estatus;
            Grid_Listado.SelectedIndex = (-1);
            Btn_Busqueda_Directa.Visible = !Estatus;
            Btn_Buscar_Solicitud.Visible = !Estatus;
            Btn_Reporte.Visible = false;
            Btn_Link_Datos_Inspeccion.Visible = false;
            Btn_Link_Datos_Solicitud.Visible = false;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
        ///DESCRIPCIÓN         : Limpia los controles del Formulario
        ///PARÁMETROS          :
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///*******************************************************************************
        private void Limpiar_Catalogo()
        {
            Hdf_Solicitud_ID.Value = "";
            Hdf_Solicitud_Interna_ID.Value = "";
            Hdf_Subproceso_ID.Value = "";
            Txt_Solicitud_ID.Text = "";
            Txt_Solicitud_Interna_ID.Text = "";
            Txt_Fecha_Respuesta.Text = "";
            Txt_Fecha_solicitud.Text = "";
            Txt_Observacion.Text = "";
            Txt_Respuesta.Text = "";
            Cmb_Area.SelectedIndex = 0;
            Cmb_Zona.SelectedIndex = 0;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Listado
        ///DESCRIPCIÓN         : Llena el Listado con una consulta que puede o no
        ///                      tener Filtros.
        ///PARÁMETROS          : 1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///*******************************************************************************
        private void Llenar_Grid_Listado(int Pagina)
        {
            DataTable Dt_Consulta = new DataTable();
            try
            {
                Grid_Listado.SelectedIndex = (-1);
                Cls_Ope_Ort_Ficha_Revision_Negocio Negocio = new Cls_Ope_Ort_Ficha_Revision_Negocio();
                Negocio.P_Solicitud_Interna_ID = Txt_Busqueda.Text.Trim();
                Dt_Consulta = Negocio.Consultar_Tabla_Ficha_Revision();
                
                //  se ordenara la tabla por solicitud desc
                DataView Dv_Ordenar = new DataView(Dt_Consulta);
                Dv_Ordenar.Sort = Ope_Ort_Ficha_Revision.Campo_Solicitud_Interna_ID + " desc ";
                Dt_Consulta = Dv_Ordenar.ToTable();

                Grid_Listado.Columns[1].Visible = true;
                Grid_Listado.DataSource = Dt_Consulta;
                Grid_Listado.PageIndex = Pagina;
                Grid_Listado.DataBind();
                Grid_Listado.Columns[1].Visible = false;
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Opiniones
        ///DESCRIPCIÓN         : Llena el Listado con una consulta que puede o no
        ///                      tener Filtros.
        ///PARÁMETROS          : 
        ///CREO                : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO          : 26/Septiembre/2012
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///*******************************************************************************
        private void Llenar_Grid_Opiniones()
        {
            DataTable Dt_Consulta = new DataTable();
            try
            {
                Grid_Listado.SelectedIndex = (-1);
                Cls_Ope_Ort_Ficha_Revision_Negocio Negocio = new Cls_Ope_Ort_Ficha_Revision_Negocio();
                Negocio.P_Solicitud_ID = Session["Opinion_Solicitud_Id"].ToString();
                Dt_Consulta = Negocio.Consultar_Tabla_Ficha_Revision();

                //  se ordenara la tabla por solicitud desc
                DataView Dv_Ordenar = new DataView(Dt_Consulta);
                Dv_Ordenar.Sort = Ope_Ort_Ficha_Revision.Campo_Solicitud_Interna_ID + " desc ";
                Dt_Consulta = Dv_Ordenar.ToTable();

                Grid_Listado.Columns[1].Visible = true;
                Grid_Listado.DataSource = Dt_Consulta;
                Grid_Listado.DataBind();
                Grid_Listado.Columns[1].Visible = false;
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Listado
        ///DESCRIPCIÓN         : Llena el Listado con una consulta que puede o no
        ///                      tener Filtros.
        ///PARÁMETROS          : 1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///*******************************************************************************
        private void Mostrar_Datos(String Solicitud_interna_ID)
        {
            Cls_Ope_Ort_Ficha_Revision_Negocio Negocio = new Cls_Ope_Ort_Ficha_Revision_Negocio();
            Negocio.P_Solicitud_Interna_ID = Solicitud_interna_ID;
            Negocio = Negocio.Consultar_Ficha_Revision();
            Hdf_Solicitud_ID.Value = Negocio.P_Solicitud_ID;
            Txt_Solicitud_Interna_ID.Text = Negocio.P_Solicitud_Interna_ID;
            Txt_Solicitud_ID.Text = Negocio.P_Solicitud_ID;
            if (!Negocio.P_Fecha_Respuesta.ToString().Contains("01/01/0001"))
                Txt_Fecha_Respuesta.Text = String.Format("{0:D}", Negocio.P_Fecha_Respuesta, new CultureInfo("es-MX"));
            if (!Negocio.P_Fecha_Solicitud.ToString().Contains("01/01/0001"))
                Txt_Fecha_solicitud.Text = String.Format("{0:D}", Negocio.P_Fecha_Solicitud, new CultureInfo("es-MX"));

            Llenar_Combo_Zonas();
            Cmb_Zona.SelectedIndex = Cmb_Zona.Items.IndexOf(Cmb_Zona.Items.FindByValue(Negocio.P_Zona_ID));
            Llenar_Combo_Areas(Negocio.P_Area_ID);
            Cmb_Area.SelectedIndex = Cmb_Area.Items.IndexOf(Cmb_Area.Items.FindByValue(Negocio.P_Area_ID));

            Txt_Observacion.Text = Negocio.P_Observacion;
            Txt_Respuesta.Text = Negocio.P_Respuesta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
        ///DESCRIPCIÓN         : genera el reporte de pdf
        ///PARÁMETROS          : 1. Ds_Reporte: Dataset con datos a imprimir
        ///                      2. Nombre_Reporte: Nombre del archivo de reporte .rpt
        ///                      3. Nombre_Archivo: Nombre del archivo a generar
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///******************************************************************************
        public void Generar_Reporte(DataSet Ds_Reporte, String Extension_Archivo, String Tipo, string Ruta_Archivo_Rpt)
        {
            String Nombre_Archivo = "Reporte_Ficha_Revision" + Convert.ToString(String.Format("{0:ddMMMyyy}", DateTime.Today)); //Obtiene el nombre del archivo que sera asignado al documento
            String Ruta_Archivo = @Server.MapPath(Ruta_Archivo_Rpt);//Obtiene la ruta en la cual será guardada el archivo
            ReportDocument Reporte = new ReportDocument();

            try
            {
                Reporte.Load(Ruta_Archivo);
                Reporte.SetDataSource(Ds_Reporte);

                DiskFileDestinationOptions m_crDiskFileDestinationOptions = new DiskFileDestinationOptions();
                //  para el tipo de archivo
                if (Extension_Archivo == "PDF")
                    Nombre_Archivo += ".pdf";
                else if (Extension_Archivo == "EXCEL")
                    Nombre_Archivo += ".xls";

                Ruta_Archivo = @Server.MapPath("../../Reporte/");
                m_crDiskFileDestinationOptions.DiskFileName = Ruta_Archivo + Nombre_Archivo;

                ExportOptions Opciones_Exportacion = new ExportOptions();
                Opciones_Exportacion.ExportDestinationOptions = m_crDiskFileDestinationOptions;
                Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;

                //  para el tipo de archivo
                if (Extension_Archivo == "PDF")
                    Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
                else if (Extension_Archivo == "EXCEL")
                    Opciones_Exportacion.ExportFormatType = ExportFormatType.Excel;

                Reporte.Export(Opciones_Exportacion);

                if (Extension_Archivo == "PDF")
                    Abrir_Ventana(Nombre_Archivo, Tipo);

                else if (Extension_Archivo == "EXCEL")
                {
                    String Ruta_Destino = "../../Reporte/" + Nombre_Archivo;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta_Destino + "', '" + Tipo + "','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Abrir_Ventana
        ///DESCRIPCIÓN         : Abre en otra ventana el archivo pdf
        ///PARÁMETROS          : Nombre_Archivo: Guarda el nombre del archivo que se desea abrir
        ///                      para mostrar los datos al usuario
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///******************************************************************************
        private void Abrir_Ventana(String Nombre_Archivo, String Tipo)
        {
            String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";
            try
            {
                Pagina = Pagina + Nombre_Archivo;
                ScriptManager.RegisterStartupScript(this, this.GetType(), Tipo,
                "window.open('" + Pagina + "', '" + Tipo + "','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
            catch (Exception ex)
            {
                throw new Exception("Abrir_Ventana " + ex.Message.ToString(), ex);
            }
        }

        #region Validaciones

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Validar_Componentes
            ///DESCRIPCIÓN         : Hace una validacion de que haya datos en los componentes antes de hacer
            ///                      una operación.
            ///PARÁMETROS          :
            ///CREO                : Salvador Vázquez Camacho
            ///FECHA_CREO          : 30/Julio/2010 
            ///MODIFICO            :
            ///FECHA_MODIFICO      :
            ///CAUSA_MODIFICACIÓN  :
            ///*******************************************************************************
            private Boolean Validar_Componentes()
            {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                String Mensaje_Error = "";
                Boolean Validacion = true;
                if (Hdf_Solicitud_ID.Value.Trim().Length == 0)
                {
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar la Solicitud.";
                    Validacion = false;
                }
                //if (Hdf_Subproceso_ID.Value.Trim().Length == 0)
                //{
                //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                //    Mensaje_Error = Mensaje_Error + "+ Seleccionar un Subproceso.";
                //    Validacion = false;
                //}
                if (Cmb_Area.SelectedIndex == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar una Área.";
                    Validacion = false;
                }
                if (Cmb_Zona.SelectedIndex == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar una Zona";
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

    #region Eventos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
        ///DESCRIPCIÓN         : Deja los componentes listos para dar de Alta un registro.
        ///PARÁMETROS          :     
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///*******************************************************************************
        protected void Btn_Nuevo_Click(object sender, EventArgs e)
        {
            try
            {
                if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
                {
                    Configuracion_Formulario(false);
                    Limpiar_Catalogo();
                    Btn_Nuevo.AlternateText = "Dar de Alta";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.Visible = false;
                    Txt_Observacion.Enabled = true;
                    Txt_Fecha_solicitud.Text = String.Format("{0:D}", DateTime.Parse(DateTime.Now.ToString(), new CultureInfo("es-MX")));
                }
                else
                {
                    if (Validar_Componentes())
                    {
                        if (Txt_Observacion.Text.Length != 0)
                        {
                            Cls_Ope_Ort_Ficha_Revision_Negocio Negocio = new Cls_Ope_Ort_Ficha_Revision_Negocio();
                            Negocio.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                            Negocio.P_Area_ID = Cmb_Area.SelectedValue;
                            Negocio.P_Zona_ID = Cmb_Zona.SelectedValue;
                            Negocio.P_Observacion = Txt_Observacion.Text.Trim();
                            Negocio.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
                            Negocio.Alta_Ficha_Revision();
                            Configuracion_Formulario(true);
                            Limpiar_Catalogo();
                            Llenar_Grid_Listado(Grid_Listado.PageIndex);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo", "alert('Alta Exitosa');", true);
                            Btn_Nuevo.AlternateText = "Nuevo";
                            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                            Btn_Salir.AlternateText = "Salir";
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        }
                        else
                        {
                            Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                            Lbl_Mensaje_Error.Text = "+ Introducir la Observacion.";
                            Div_Contenedor_Msj_Error.Visible = true;
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
        ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
        ///DESCRIPCIÓN         : Deja los componentes listos para hacer la modificacion.
        ///PARÁMETROS          :     
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///*******************************************************************************
        protected void Btn_Modificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Btn_Modificar.AlternateText.Equals("Modificar"))
                {
                    if (Grid_Listado.Rows.Count > 0 && Grid_Listado.SelectedIndex > (-1))
                    {
                        if (Txt_Respuesta.Text.Length == 0)
                        {
                            Cls_Ope_Ort_Ficha_Revision_Negocio Negocio = new Cls_Ope_Ort_Ficha_Revision_Negocio();
                            Negocio.P_Solicitud_Interna_ID = Hdf_Solicitud_Interna_ID.Value;
                            Negocio = Negocio.Consultar_Ficha_Revision();

                            if (Negocio.P_Usuario_Creo == Cls_Sessiones.Nombre_Empleado)
                            {
                                Configuracion_Formulario(false);
                                Btn_Modificar.AlternateText = "Actualizar";
                                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                                Btn_Salir.AlternateText = "Cancelar";
                                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                                Btn_Nuevo.Visible = false;
                            }
                            else
                            {
                                Cls_Cat_Ort_Zona_Negocio Zona_Negocio = new Cls_Cat_Ort_Zona_Negocio();
                                Zona_Negocio.P_Zona_ID = Negocio.P_Zona_ID;
                                DataTable Dt_Zonas = Zona_Negocio.Consultar_Zonas();


                                if (Dt_Zonas.Rows.Count>=0)
                                {
                                    if (Dt_Zonas.Rows[0][Cat_Ort_Zona.Campo_Empleado_ID].ToString() == Cls_Sessiones.Empleado_ID)
                                    {
                                        Txt_Respuesta.Enabled = true;
                                        Btn_Link_Datos_Solicitud.Visible = true;
                                        Btn_Link_Datos_Inspeccion.Visible = true;
                                        Btn_Modificar.AlternateText = "Actualizar";
                                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                                        Btn_Salir.AlternateText = "Cancelar";
                                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                                        Btn_Nuevo.Visible = false;
                                        Grid_Listado.Enabled = false;

                                        String Ventana_Modal = "Abrir_Resumen('../Tramites/Frm_Bandeja_Tramites.aspx";
                                        String Propiedades = ", 'center:yes,resizable=no,status=no,width=750,scrollbars=yes,')";
                                        Btn_Link_Datos_Solicitud.Attributes.Add("OnClick", Ventana_Modal + "?Solicitud=" + Hdf_Solicitud_ID.Value + "'" + Propiedades);

                                        Ventana_Modal = "Abrir_Resumen('../Ordenamiento_Territorial/Frm_Ope_Ort_Administracion_Urbana.aspx";
                                        Propiedades = ", 'center:yes,resizable=no,status=no,width=750,scrollbars=yes,')";
                                        Btn_Link_Datos_Inspeccion.Attributes.Add("OnClick", Ventana_Modal + "?Solicitud=" + Hdf_Solicitud_ID.Value + "'" + Propiedades);

                                        //"?Cuenta_Predial="
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                        Lbl_Mensaje_Error.Text = "+ Seleccionar la Solicitud que desea Modificar.";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                }
                else
                {
                    if (Validar_Componentes())
                    {
                        Cls_Ope_Ort_Ficha_Revision_Negocio Negocio = new Cls_Ope_Ort_Ficha_Revision_Negocio();
                        Negocio.P_Solicitud_Interna_ID = Hdf_Solicitud_Interna_ID.Value;
                        Negocio = Negocio.Consultar_Ficha_Revision();
                        Negocio.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                        Negocio.P_Area_ID = Cmb_Area.SelectedValue;
                        Negocio.P_Zona_ID = Cmb_Zona.SelectedValue;
                        Negocio.P_Observacion = Txt_Observacion.Text.Trim();
                        Negocio.P_Respuesta = Txt_Respuesta.Text.Trim();
                        if (Txt_Respuesta.Enabled)
                            Negocio.P_Fecha_Respuesta = DateTime.Now;
                        Negocio.P_Usuario_Modifico = Cls_Sessiones.Nombre_Empleado;
                        Negocio.Modificar_Ficha_Revision();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Grid_Listado(Grid_Listado.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo", "alert('Actualización Exitosa');", true);
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Grid_Listado.Enabled = true;
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
        ///NOMBRE DE LA FUNCIÓN: Btn_Link_Datos_Solicitud_Click
        ///DESCRIPCIÓN: mostrara la informacion de la solicitud
        ///PARAMETROS: 
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  10/Septiembre/2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Btn_Link_Datos_Solicitud_Click(object sender, EventArgs e)
        {
            try
            {
                //Session["BUSQUEDA_SOLICITUD"] = null;

                //Cls_Sessiones.Ciudadano_ID = Hdf_Solicitud_ID.Value;

                ////Response.Redirect("../Tramites/Frm_Bandeja_Tramites.aspx");
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Link_Datos_Inspeccion_Click
        ///DESCRIPCIÓN: mostrara la informacion de la solicitud
        ///PARAMETROS: 
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  10/Septiembre/2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Btn_Link_Datos_Inspeccion_Click(object sender, EventArgs e)
        {
            try
            {
                //Session["BUSQUEDA_SOLICITUD"] = null;

                //Cls_Sessiones.Ciudadano_ID = Hdf_Solicitud_ID.Value;
                ////Response.Redirect("../Ordenamiento_Territorial/Frm_Ope_Ort_Administracion_Urbana.aspx");
                
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
        ///DESCRIPCIÓN         : Cancela la operación que esta en proceso (Alta o Actualizar) o Sale del Formulario.
        ///PARÁMETROS          :     
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///*******************************************************************************
        protected void Btn_Salir_Click(object sender, EventArgs e)
        {
            if (Btn_Salir.AlternateText.Equals("Salir"))
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                Cls_Sessiones.Ciudadano_ID = "";
            }
            else
            {
                Configuracion_Formulario(true);
                Limpiar_Catalogo();
                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Cls_Sessiones.Ciudadano_ID = "";
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Cmb_Zona_SelectedIndexChanged
        ///DESCRIPCIÓN         : Realiza la busqueda deacuerdo al elemento seleccionado.
        ///PARÁMETROS          :     
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///*******************************************************************************
        protected void Cmb_Zona_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cls_Cat_Ort_Zona_Negocio Negocio = new Cls_Cat_Ort_Zona_Negocio();
            Negocio.P_Zona_ID = Cmb_Zona.SelectedValue;
            DataTable Dt_Zonas = Negocio.Consultar_Zonas();

            if (Dt_Zonas.Rows.Count >= 0)
            {
                Llenar_Combo_Areas(Dt_Zonas.Rows[0][Cat_Ort_Zona.Campo_Area].ToString());
                Cmb_Area.SelectedIndex = 1;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Solicitud_Click
        ///DESCRIPCIÓN         : Carga los datos del elemento seleccionado.
        ///PARÁMETROS          :     
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///*******************************************************************************
        protected void Btn_Buscar_Solicitud_Click(object sender, ImageClickEventArgs e)
        {
            Txt_Solicitud_ID.Text = Session["SOLICITUD_ID"].ToString();
            Hdf_Solicitud_ID.Value = Txt_Solicitud_ID.Text.Trim();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Reporte_Click
        ///DESCRIPCIÓN         : Consulta los datos del elemento seleccionado para generar
        ///                      el reporte.
        ///PARÁMETROS          :     
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///*******************************************************************************
        protected void Btn_Reporte_Click(object sender, ImageClickEventArgs e)
        {
            if (Hdf_Solicitud_Interna_ID.Value.ToString().Length != 0)
            {
                Cls_Ope_Ort_Ficha_Revision_Negocio Negocio = new Cls_Ope_Ort_Ficha_Revision_Negocio();
                DataSet Ds_Reporte = new DataSet();
                DataTable Dt_Ficha_Revision = new DataTable();
                try
                {
                    Negocio.P_Solicitud_Interna_ID = Hdf_Solicitud_Interna_ID.Value;
                    Dt_Ficha_Revision = Negocio.Consultar_Tabla_Ficha_Revision();

                    Dt_Ficha_Revision.TableName = "Dt_Ficha_Revision";
                    Ds_Reporte.Tables.Add(Dt_Ficha_Revision.Copy());

                    Generar_Reporte(Ds_Reporte, "PDF", "Ficha_Revision", "../Rpt/Ordenamiento_Territorial/Rpt_Ort_Ficha_Revision.rpt");

                }
                catch (Exception ex)
                {
                    throw new Exception("Generar_Reporte_Ficha_Revision: " + ex.Message.ToString(), ex);
                }

            }
        }

    #endregion

    #region Grids

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Listado_SelectedIndexChanged
        ///DESCRIPCIÓN         : Obtiene el elemento seleccionado.
        ///PARÁMETROS          :     
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///*******************************************************************************
        protected void Grid_Listado_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Grid_Listado.SelectedIndex > (-1))
                {
                    Limpiar_Catalogo();
                    String ID = HttpUtility.HtmlDecode(Grid_Listado.SelectedRow.Cells[1].Text.Trim());
                    Hdf_Solicitud_Interna_ID.Value = ID;
                    Mostrar_Datos(ID);
                    Cls_Sessiones.Ciudadano_ID = Hdf_Solicitud_ID.Value;
                    Btn_Reporte.Visible = true;
                    System.Threading.Thread.Sleep(500);
                }
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Grid_Listado_RowDataBound
        ///DESCRIPCIÓN          : cargara los botones
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramirez Aguilera
        /// FECHA_CREO          : 08/Mayo/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        protected void Grid_Listado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Cls_Ope_Ort_Ficha_Revision_Negocio Negocio = new Cls_Ope_Ort_Ficha_Revision_Negocio();
            String Mensaje = "";
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Negocio.P_Solicitud_Interna_ID = e.Row.Cells[1].Text;
                    Negocio = Negocio.Consultar_Ficha_Revision();

                    //  fecha de respuesta
                    Mensaje = e.Row.Cells[5].Text;

                    if (Negocio.P_Usuario_Creo == Cls_Sessiones.Nombre_Empleado)
                    {
                        if (Mensaje != "-")
                        {
                            //  color para los que tienen respuesta
                            e.Row.Cells[0].BackColor = System.Drawing.Color.DeepSkyBlue;
                            //e.Row.Cells[1].BackColor = System.Drawing.Color.SkyBlue;
                            //e.Row.Cells[2].BackColor = System.Drawing.Color.SkyBlue;
                            //e.Row.Cells[3].BackColor = System.Drawing.Color.SkyBlue;
                            //e.Row.Cells[4].BackColor = System.Drawing.Color.SkyBlue;
                            //e.Row.Cells[5].BackColor = System.Drawing.Color.SkyBlue;
                        }
                        else
                        {
                            //  color para los que no tienen respuesta
                            e.Row.Cells[0].BackColor = System.Drawing.Color.LightSteelBlue;
                            //e.Row.Cells[1].BackColor = System.Drawing.Color.LightSteelBlue;
                            //e.Row.Cells[2].BackColor = System.Drawing.Color.LightSteelBlue;
                            //e.Row.Cells[3].BackColor = System.Drawing.Color.LightSteelBlue;
                            //e.Row.Cells[4].BackColor = System.Drawing.Color.LightSteelBlue;
                            //e.Row.Cells[5].BackColor = System.Drawing.Color.LightSteelBlue;
                        }
                    }
                    else
                    {
                        Cls_Cat_Ort_Zona_Negocio Zona_Negocio = new Cls_Cat_Ort_Zona_Negocio();
                        Zona_Negocio.P_Zona_ID = Negocio.P_Zona_ID;
                        DataTable Dt_Zonas = Zona_Negocio.Consultar_Zonas();


                        if (Dt_Zonas.Rows.Count >= 0 && Dt_Zonas != null)
                        {
                            if (Dt_Zonas.Rows[0][Cat_Ort_Zona.Campo_Empleado_ID].ToString() == Cls_Sessiones.Empleado_ID)
                            {
                                if (Negocio.P_Respuesta == "")
                                {
                                    e.Row.Cells[0].BackColor = System.Drawing.Color.Yellow;
                                    e.Row.Cells[1].BackColor = System.Drawing.Color.Yellow;
                                    e.Row.Cells[2].BackColor = System.Drawing.Color.Yellow;
                                    e.Row.Cells[3].BackColor = System.Drawing.Color.Yellow;
                                    e.Row.Cells[4].BackColor = System.Drawing.Color.Yellow;
                                    e.Row.Cells[5].BackColor = System.Drawing.Color.Yellow;
                                }
                                else
                                {
                                    e.Row.Cells[0].BackColor = System.Drawing.Color.LightGreen;
                                    e.Row.Cells[1].BackColor = System.Drawing.Color.LightGreen;
                                    //e.Row.Cells[2].BackColor = System.Drawing.Color.LightGreen;
                                    //e.Row.Cells[3].BackColor = System.Drawing.Color.LightGreen;
                                }
                            }
                        }

                    }

                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }
    #endregion

}
