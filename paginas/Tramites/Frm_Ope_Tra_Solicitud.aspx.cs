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
using Presidencia.Solicitud_Tramites.Negocios;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using AjaxControlToolkit;
using System.IO;
using System.Text.RegularExpressions;
using Presidencia.Ventanilla_Lista_Tramites.Negocio;
using Presidencia.Dependencias.Negocios;
using Presidencia.Cls_Cat_Ven_Registro_Usuarios.Negocio;
using Presidencia.Catalogo_Calles.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Bandeja_Solicitudes_Tramites.Negocio;
using Presidencia.Plantillas_Word;
using System.Drawing;
using Presidencia.Registro_Peticion.Datos;
using Presidencia.Operacion_Predial_Dias_Inhabiles.Negocio;
using Presidencia.Ordenamiento_Territorial_Inspectores.Negocio;
using Presidencia.Operacion_Predial_Pagos_Instit_Externas.Negocio;
using Presidencia.Catalogo_Ordenamiento_Territorial_Parametros.Negocio;
using Presidencia.Ayudante_Curp_Rfc;


public partial class paginas_Tramites_Frm_Ope_Tra_Solicitud : System.Web.UI.Page
{
    #region Variables

        //objeto de la clase de negocio de solicitudes_tramites para acceder a la clase de datos y realizar copnexion
        private Cls_Ope_Solicitud_Tramites_Negocio Solicitud_Negocio;
        //variable para guardar los datos de la consulta de datos de un tramite
        private static DataSet Ds_Datos;
        //ariable para guardar los datos de la consulta de documentos de un tramite
        private static DataSet Ds_Documentos;
        //variable para guardar los datos de la consulta de un tramite
        private static DataSet Ds_Tramites;

    #endregion

    #region Page Load Init

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: 
        ///PARAMETROS:
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 19/Octubre/2010
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Page_Load(object sender, EventArgs e)
        {
            Solicitud_Negocio = new Cls_Ope_Solicitud_Tramites_Negocio();

           
            try
            {
                if (!IsPostBack)
                {
                    Session["Activa"] = true;//Variable para mantener la session activa.

                    this.Form.Enctype = "multipart/form-data";

                    if (Cmb_Estatus.Items.Count == 0)
                    {
                        Cmb_Estatus.Items.Add("<<SELECCIONAR>>");
                        Cmb_Estatus.Items.Add("PENDIENTE");
                        Cmb_Estatus.Items.Add("PROCESO");
                        Cmb_Estatus.Items.Add("TERMINADO");
                        Cmb_Estatus.Items.Add("DETENIDO");
                        Cmb_Estatus.Items.Add("CANCELADO");

                        Cmb_Estatus.Items[0].Value = "0";
                        Cmb_Estatus.Items[0].Selected = true;
                    }
                    if (Cmb_Tramite.Items.Count == 0)
                    {
                        Ds_Tramites = new DataSet();
                        Ds_Tramites = Solicitud_Negocio.Consultar_Tramites();
                        Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Tramite, Ds_Tramites.Tables[0], 3, 0);
                        Cmb_Tramite.SelectedIndex = 0;
                    }
                    
                    LLenar_Combos();

                    Div_Grid_Datos_Tramite.Style.Value = "display:block";
                    Div_Grid_Documentos.Style.Value = "display:block";
                    Div_Grid_Documentos_Modificar.Style.Value = "display:block";

                    Deshabilitar_Forma();
                    Cargar_Combo_Unidad_Responsable();
                    Cargar_Combo_Estado();
                    //Div_Link_Busqueda_Tramite.Style.Value = "display:none";
                    //Div_Consultar_Tramite.Style.Value = "color:#5D7B9D; display:none";

                    // registro de scripts del lado del servidor para mostrar ventanas emergentes para búsqueda avanzada
                    string Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergente/Frm_Busqueda_Avanzada_Tramites.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                    Btn_Busqueda_Tramite.Attributes.Add("OnClick", Ventana_Modal);
                    Btn_Nuevo.Attributes.Add("OnClick", Ventana_Modal);

                    Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergente/Frm_Busqueda_Avanzada_Ciudadano.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                    Btn_Link_Busqueda_Ciudadano.Attributes.Add("onclick", Ventana_Modal);

                    Ventana_Modal = "Abrir_Ventana_Modal('../Predial/Ventanas_Emergentes/Frm_Busqueda_Avanzada_Colonias_Calles.aspx', 'center:yes;resizable:no;status:no;dialogWidth:580px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                    Btn_Buscar_Calle.Attributes.Add("onclick", Ventana_Modal);

                    Ventana_Modal = "Abrir_Ventana_Modal('../Predial/Ventanas_Emergentes/Frm_Busqueda_Avanzada_Colonias.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                    Btn_Buscar_Colonia.Attributes.Add("onclick", Ventana_Modal);

                    Ventana_Modal = "Abrir_Ventana_Modal('../Ventanilla/Ventanas_Emergentes/Frm_Ven_Busqueda_Avanzada_Peritos.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                    Btn_Buscar_Perito.Attributes.Add("onclick", Ventana_Modal);

                    Session["BUSQUEDA_TRAMITES"] = false;
                    Session["BUSQUEDA_CIUDADANO"] = false;
                    Manejar_Botones(3);
                }
            }// fin del try

            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

    #endregion

    #region Métodos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Extension
        ///DESCRIPCIÓN: Maneja la extencion del archivo
        ///PROPIEDADES: String Ruta, direccion que 
        ///contiene el nombre del archivo al cual se le sacara la extension
        ///CREO: Francisco Gallardo
        ///FECHA_CREO: 16/Marzo/2010
        ///MODIFICO: Silvia Morales
        ///FECHA_MODIFICO: 19/Octubre/2010
        ///CAUSA_MODIFICACIÓN: Se adecuo al estandar
        ///*******************************************************************************
        private string Obtener_Extension(String Ruta)
        {

            String Extension = "";
            try
            {
                int index = Ruta.LastIndexOf(".");
                if (index < Ruta.Length)
                {
                    Extension = Ruta.Substring(index + 1);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return Extension;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: CRefrescar_Grid_Documentos
        ///DESCRIPCIÓN: Carga el grid de documentos 
        ///con los documentos aplicables al tramite q se escogio y hace vissible el grid
        ///PARAMETROS: String Tramite_ID:Contiene el id del tramite
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 15/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Refrescar_Grid_Documentos(String Tramite_ID)
        {
            try
            {
                Solicitud_Negocio.P_Tramite_ID = Tramite_ID;
                Ds_Documentos = new DataSet();
                Ds_Documentos = Solicitud_Negocio.Consultar_Documentos_Tramite();


                DataView Dv_Ordenar = new DataView(Ds_Documentos.Tables[0]);
                Dv_Ordenar.Sort = "DOCUMENTO_REQUERIDO desc, DOCUMENTO asc";
                DataTable Dt_Datos_Ordenados = Dv_Ordenar.ToTable();

                Ds_Documentos = new DataSet();
                Ds_Documentos.Tables.Add(Dt_Datos_Ordenados.Copy());

                if (Ds_Documentos.Tables[0].Rows.Count > 0 && Ds_Documentos.Tables[0] != null)
                {
                    Grid_Documentos.Columns[0].Visible = true;
                    Grid_Documentos.Columns[3].Visible = true;
                    Grid_Documentos.Visible = true;
                    Grid_Documentos.DataSource = Dt_Datos_Ordenados;
                    Grid_Documentos.DataBind();
                    Grid_Documentos.Columns[0].Visible = false;
                    Grid_Documentos.Columns[3].Visible = false;

                    Lbl_Documentos_Requeridos.Visible = true;
                    Lbl_Mensaje_Documentos.Visible = true;
                    Lbl_Mensaje_Documentos.Visible = true;
                    Div_Grid_Documentos.Style.Value = "overflow:auto;height:150px;width:99%;vertical-align:top;border-style:solid;border-color:Silver;display:block";
                }
                else
                {
                    Grid_Documentos.Columns[0].Visible = true;
                    Grid_Documentos.Visible = false;
                    Grid_Documentos.DataSource = new DataTable();
                    Grid_Documentos.DataBind();
                    Grid_Documentos.Columns[0].Visible = false;
                    Lbl_Mensaje_Documentos.Visible = false;
                    Lbl_Documentos_Requeridos.Visible = false;
                    Div_Grid_Documentos.Style.Value = "display:none";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        
        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Llenar_Combo_Con_DataTable
        ///DESCRIPCIÓN: Asigna los valores de la tabla en el combo recibidos como parámetros
        ///PARÁMETROS:
        /// 		1. Obj_Combo: control al que se van a asignar los datos en la tabla
        /// 		2. Dt_Temporal: tabla con los datos a mostrar en el control Obj_Combo
        /// 		3. Indice_Campo_Valor: entero con el número de columna de la tabla con el valor para el combo
        /// 		4. Indice_Campo_Texto: entero con el número de columna de la tabla con el texto para el combo
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 17-may-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public void Llenar_Combo_Con_DataTable(DropDownList Obj_Combo, DataTable Dt_Temporal, int Indice_Campo_Valor, int Indice_Campo_Texto)
        {
            Obj_Combo.Items.Clear();
            Obj_Combo.SelectedValue = null;
            Obj_Combo.DataSource = Dt_Temporal;
            Obj_Combo.DataTextField = Dt_Temporal.Columns[Indice_Campo_Texto].ToString();
            Obj_Combo.DataValueField = Dt_Temporal.Columns[Indice_Campo_Valor].ToString();
            Obj_Combo.DataBind();
            Obj_Combo.Items.Insert(0, new ListItem(HttpUtility.HtmlDecode("<SELECCIONAR>"), "0"));
            Obj_Combo.SelectedIndex = 0;
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Refrescar_Grid_Documentos_Modificar
        ///DESCRIPCIÓN: Carga el grid de documentos
        ///con los documentos aplicables al tramite q se escogio y hace vissible el grid
        ///este ocurre cuando se realiza una busqueda
        ///PARAMETROS:
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 15/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Refrescar_Grid_Documentos_Modificar()
        {
            try
            {
                Solicitud_Negocio.P_Tramite_ID = Cmb_Tramite.SelectedValue;
                Ds_Documentos = new DataSet();
                Ds_Documentos = Solicitud_Negocio.Consultar_Documentos_Tramite();

                DataView Dv_Ordenar = new DataView(Ds_Documentos.Tables[0]);
                Dv_Ordenar.Sort = "DOCUMENTO_REQUERIDO desc, DOCUMENTO asc";
                DataTable Dt_Datos_Ordenados = Dv_Ordenar.ToTable();

                Ds_Documentos = new DataSet();
                Ds_Documentos.Tables.Add(Dt_Datos_Ordenados.Copy());

                if (Ds_Documentos.Tables[0].Rows.Count > 0 && Ds_Documentos.Tables[0] != null)
                {
                    Grid_Documentos_Modificar.Visible = true;
                    Grid_Documentos_Modificar.DataSource = Ds_Documentos;
                    Grid_Documentos_Modificar.DataBind();

                    Lbl_Documentos_Requeridos.Visible = true;
                    Lbl_Mensaje_Documentos.Visible = true;
                    Div_Grid_Documentos_Modificar.Style.Value = "overflow:auto;height:150px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;display:block";
                }
                else
                {
                    Grid_Documentos_Modificar.Visible = false;
                    Grid_Documentos_Modificar.DataSource = new DataTable();
                    Grid_Documentos_Modificar.DataBind();

                    Lbl_Documentos_Requeridos.Visible = false;
                    Lbl_Mensaje_Documentos.Visible = false;
                    Div_Grid_Documentos_Modificar.Style.Value = "display:none";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: CRefrescar_Grid_Datos
        ///DESCRIPCIÓN: Carga el grid de datos 
        ///con los datos aplicables al tramite q se escogio y hace vissible el grid
        ///PARAMETROS: String Tramite_ID:contiene el id del tramite
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 15/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Refrescar_Grid_Datos(String Tramite_ID)
        {
            try
            {
                Solicitud_Negocio.P_Tramite_ID = Tramite_ID;
                Ds_Datos = new DataSet();
                Solicitud_Negocio.P_Tipo_Dato = "INICIAL";
                Ds_Datos = Solicitud_Negocio.Consultar_Datos_Tramite();
                
                if (Ds_Datos.Tables[0].Rows.Count > 0 && Ds_Datos.Tables[0] != null)
                {
                    Grid_Datos.Columns[0].Visible = true;
                    Grid_Datos.DataSource = Ds_Datos;
                    Grid_Datos.DataBind();
                    Grid_Datos.Visible = true;
                    Grid_Datos.Columns[0].Visible = false;
                    Lbl_Datos_Requeridos.Visible = true;
                    Div_Grid_Datos_Tramite.Style.Value = "overflow:auto;height:150px;width:99%;vertical-align:top;border-style:solid;border-color:Silver;display:block";

                }
                else
                {
                    Grid_Datos.Columns[0].Visible = true;
                    Grid_Datos.DataSource = new DataTable();
                    Grid_Datos.DataBind();
                    Grid_Datos.Columns[0].Visible = false;
                    Grid_Datos.Visible = true;
                    Lbl_Datos_Requeridos.Visible = false;
                    Div_Grid_Datos_Tramite.Style.Value = "display:none";

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Unidad_Responsable
        ///DESCRIPCIÓN: cargara la informacion de las unidades responsables
        ///PARAMETROS: 
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  17/Mayo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Cargar_Combo_Unidad_Responsable()
        {
            Cls_Cat_Dependencias_Negocio Rs_Responsable = new Cls_Cat_Dependencias_Negocio();
            DataTable Dt_Unidad_Responsable = new DataTable();
            try
            {
                //  para la unidad resposable
                Dt_Unidad_Responsable = Rs_Responsable.Consulta_Dependencias();

                DataView Dv_Ordenar = new DataView(Dt_Unidad_Responsable);
                Dv_Ordenar.Sort = Cat_Dependencias.Campo_Nombre;
                Dt_Unidad_Responsable = Dv_Ordenar.ToTable();

                //Cmb_Unidad_Responsable_Filtro.DataSource = Dt_Unidad_Responsable;
                //Cmb_Unidad_Responsable_Filtro.DataValueField = Cat_Dependencias.Campo_Dependencia_ID;
                //Cmb_Unidad_Responsable_Filtro.DataTextField = Cat_Dependencias.Campo_Nombre;
                //Cmb_Unidad_Responsable_Filtro.DataBind();
                //Cmb_Unidad_Responsable_Filtro.Items.Insert(0, "< SELECCIONE >");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Cuenta_Predial_ID
        /// DESCRIPCIÓN: Regresar el id de la cuenta predial, se busca mediante la cuenta predial 
        /// PARÁMETROS:
        /// 		1. Cuenta_Predial: cuenta predial a localizar
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 10-jun-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private String Consultar_Cuenta_Predial_ID(String Cuenta_Predial)
        {
            var Consulta_Cuenta = new Cls_Ope_Pre_Pagos_Instituciones_Externas_Negocio();
            String Cuenta_Predial_ID = "";
            DataTable Dt_Resultado_Consulta;

            try
            {
                if (!String.IsNullOrEmpty(Cuenta_Predial))
                {
                    // consultar cuenta predial
                    Consulta_Cuenta.P_Cuenta_Predial = Cuenta_Predial;
                    Dt_Resultado_Consulta = Consulta_Cuenta.Consultar_Cuenta_Predial_ID();
                    if (Dt_Resultado_Consulta != null && Dt_Resultado_Consulta.Rows.Count > 0)
                    {
                        Cuenta_Predial_ID = Dt_Resultado_Consulta.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Consultar_Cuenta_Predial_ID: " + ex.Message.ToString(), ex);
            }
            return Cuenta_Predial_ID;
        }
        
        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Propietario
        /// DESCRIPCIÓN: Regresar el nombre del propietario de la cuenta predial con el id proporcionado
        /// PARÁMETROS:
        /// 		1. Cuenta_Predial_ID: id de la cuenta predial a consultar
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 10-jun-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private String Consultar_Propietario(String Cuenta_Predial_ID)
        {
            String Propietario = "";
            DataTable Dt_Resultado_Consulta;
            var Consulta_Propietario_Negocio = new Cls_Ope_Pre_Pagos_Instituciones_Externas_Negocio();

            try
            {
                if (!String.IsNullOrEmpty(Cuenta_Predial_ID))
                {
                    // consultar cuenta predial
                    Consulta_Propietario_Negocio.P_Cuenta_Predial_Id = Cuenta_Predial_ID;
                    Dt_Resultado_Consulta = Consulta_Propietario_Negocio.Consultar_Propietario();
                    if (Dt_Resultado_Consulta != null && Dt_Resultado_Consulta.Rows.Count > 0)
                    {
                        Propietario = Dt_Resultado_Consulta.Rows[0]["NOMBRE_PROPIETARIO"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Lbl_Informacion.Visible = true;
                Img_Warning.Visible = true;
                Lbl_Informacion.Text = ex.Message;
            }
            return Propietario;
        }
        
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN : Cargar_Ventana_Emergente_Resumen_Predio
        ///DESCRIPCIÓN          : Establece el evento onclik del control para abrir la ventana emergente del Resumen de Predial con la ruta y parámtros necesarios
        ///PARAMETROS: 
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 09/Julio/2012
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        private void Cargar_Ventana_Emergente_Resumen_Predio()
        {
            String Ventana_Modal = "Abrir_Resumen('../Ventanilla/Ventanas_Emergentes/Frm_Resumen_Predio_Ventanilla.aspx";
            String Propiedades = ", 'center:yes,resizable=no,status=no,width=750,scrollbars=yes,');";
            //String Propiedades = ", 'center:yes;resizable:no;status:no;dialogWidth:800px;dialogHeight:600px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Cuenta_Predial.Attributes.Add("onclick", Ventana_Modal + "?Cuenta_Predial=" + Txt_Cuenta_Predial.Text.Trim().ToUpper() + "'" + Propiedades);
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Estado
        ///DESCRIPCIÓN: cargara la informacion de los estados de la republica
        ///PARAMETROS: 
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  23/Mayo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Cargar_Combo_Estado()
        {
            Cls_Cat_Ven_Registro_Usuarios_Negocio Negocio_Consulta = new Cls_Cat_Ven_Registro_Usuarios_Negocio();
            DataTable Dt_Estado = new DataTable();
            try
            {
                //  para las calles
                Dt_Estado = Negocio_Consulta.Consultar_Estados();

                Cmb_Estado.DataSource = Dt_Estado;
                Cmb_Estado.DataValueField = Cat_Pre_Estados.Campo_Nombre;
                Cmb_Estado.DataTextField = Cat_Pre_Estados.Campo_Nombre;
                Cmb_Estado.DataBind();
                Cmb_Estado.Items.Insert(0, "< SELECCIONE >");

                Cmb_Estado.SelectedValue = "GUANAJUATO";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: LLenar_Combos
        ///DESCRIPCIÓN: Consulta los datos para los combos y los asigna al combo correspondiente
        ///PARÁMETROS:
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 10-jun-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public void LLenar_Combos()
        {
            var Obj_Peritos = new Cls_Cat_Ort_Inspectores_Negocio();
            DataTable Dt_Peritos;

            Lbl_Informacion.Visible = false;
            Img_Warning.Visible = false;
            Lbl_Informacion.Text = "";

            try
            {
                // consultar peritos
                Dt_Peritos = Obj_Peritos.Consultar_Inspectores();
                // cargar datos en el combo
                Cmb_Perito.Items.Clear();
                Cmb_Perito.DataSource = Dt_Peritos;
                Cmb_Perito.DataTextField = Cat_Ort_Inspectores.Campo_Nombre;
                Cmb_Perito.DataValueField = Cat_Ort_Inspectores.Campo_Inspector_ID;
                Cmb_Perito.DataBind();
                Cmb_Perito.Items.Insert(0, new ListItem(HttpUtility.HtmlDecode("<SELECCIONAR>"), ""));
                Cmb_Perito.SelectedIndex = 0;
            }
            catch (Exception Ex)
            {
                Lbl_Informacion.Visible = true;
                Img_Warning.Visible = true;
                Lbl_Informacion.Text = Ex.Message;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
        ///DESCRIPCIÓN: genera el reporte de pdf
        ///PARÁMETROS : 	
        ///         1. Ds_Reporte: Dataset con datos a imprimir
        /// 		2. Nombre_Reporte: Nombre del archivo de reporte .rpt
        /// 		3. Nombre_Archivo: Nombre del archivo a generar
        ///CREO       : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO  : 02-Julio-2012
        ///MODIFICO          :
        ///FECHA_MODIFICO    :
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************
        public void Generar_Reporte(DataSet Ds_Reporte, String Extension_Archivo, String Tipo, string Ruta_Archivo_Rpt)
        {
            String Nombre_Archivo = "Reporte_Seguimiento_Solicitud_" + Convert.ToString(String.Format("{0:ddMMMyyy}", DateTime.Today)); //Obtiene el nombre del archivo que sera asignado al documento
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
        ///DESCRIPCIÓN: Abre en otra ventana el archivo pdf
        ///PARÁMETROS : Nombre_Archivo: Guarda el nombre del archivo que se desea abrir
        ///                             para mostrar los datos al usuario
        ///CREO       : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO  : 02-Julio-2012
        ///MODIFICO          :
        ///FECHA_MODIFICO    :
        ///CAUSA_MODIFICACIÓN:
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
        
        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Generar_Reporte_Folio_Solicitud
        ///DESCRIPCIÓN: Breve descripción de lo que hace la función.
        ///PARÁMETROS:
        /// 		1. Clave_Unica: folio de la solicitud
        /// 		2. Fecha_Solucion: fecha de solución probable para la solicitud
        /// 		3. Nombre_Completo: nombre del solicitante
        /// 		4. Email: correo electrónico del solicitante
        /// 		5. Nombre_Tramite: nombre del trámite que se pasa como parámetro
        /// 		6. Cuenta_Predial: cadena de texto con el número de cuenta predial de la solicitud
        /// 		7. Propietario: nombre del propietario de la cuenta predial en la solicitud
        /// 		7. Solicitud_ID: Id de la solicitud
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 11-jul-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private void Generar_Reporte_Folio_Solicitud(string Clave_Unica, DateTime Fecha_Solucion, string Nombre_Completo, string Email,
                string Nombre_Tramite, string Cuenta_Predial, string Propietario, String Solicitud_ID, String Dependencia, String Area)
        {
            Ds_Rpt_Ort_Seguimiento_Solicitud_Tramite Ds_Reporte = new Ds_Rpt_Ort_Seguimiento_Solicitud_Tramite();
            Cls_Cat_Dependencias_Negocio Negocio_Dependencia = new Cls_Cat_Dependencias_Negocio();
            DataTable Dt_Dependencia = new DataTable();
            try
            {
                Negocio_Dependencia.P_Dependencia_ID = Dependencia;
                Dt_Dependencia = Negocio_Dependencia.Consulta_Dependencias();

                DataRow Fila = Ds_Reporte.Tables["Dt_Datos_Solicitud"].NewRow();
                Fila["CLAVE_SOLICITUD"] = Clave_Unica;
                Fila["PORCENTAJE_AVANCE"] = "0";
                Fila["ESTATUS"] = "PENDIENTE";
                //Fila["FECHA_TRAMITE"] = DateTime.Today;
                //Fila["FECHA_ENTREGA"] = Fecha_Solucion;
                Fila["NOMBRE_COMPLETO"] = Nombre_Completo;
                Fila["CORREO_ELECTRONICO"] = Email;
                Fila["NOMBRE"] = "";
                Fila["NOMBRE_TRAMITE"] = Nombre_Tramite;
                Fila["CUENTA_PREDIAL"] = Cuenta_Predial;
                Fila["PROPIETARIO_CUENTA"] = Propietario;
                Fila["CONSECUTIVO"] = Solicitud_ID; 
                Fila["DEPENDENCIA"] = Dt_Dependencia.Rows[0][Cat_Dependencias.Campo_Nombre].ToString();
                Fila["AREA"] = Area;
                Ds_Reporte.Tables["Dt_Datos_Solicitud"].Rows.Add(Fila);


                Generar_Reporte(Ds_Reporte, "PDF", "Folio_Solicitud", "../Rpt/Ventanilla/Rpt_Ven_Folio_Solicitud_Tramite.rpt");

            }
            catch (Exception ex)
            {
                throw new Exception("Generar_Reporte_Folio_Solicitud: " + ex.Message.ToString(), ex);
            }
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Generar_Reporte_Solicitud_Formato
        ///DESCRIPCIÓN: genera el reporte de pdf
        ///PARÁMETROS : 	
        ///         1. Ds_Reporte: Dataset con datos a imprimir
        /// 		2. Nombre_Reporte: Nombre del archivo de reporte .rpt
        /// 		3. Nombre_Archivo: Nombre del archivo a generar
        /// 		4. Tipo: Parámetro para ventana emergente
        ///CREO       : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO  : 02-Julio-2012
        ///MODIFICO          :
        ///FECHA_MODIFICO    :
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************
        public void Generar_Reporte_Solicitud_Formato(DataSet Ds_Reporte, String Extension_Archivo, String Tipo)
        {
            String Nombre_Archivo = "Reporte_Formato_" + Convert.ToString(String.Format("{0:ddMMMyyy}", DateTime.Today)); //Obtiene el nombre del archivo que sera asignado al documento
            String Ruta_Archivo = @Server.MapPath("../Rpt/Ordenamiento_Territorial/");//Obtiene la ruta en la cual será guardada el archivo
            ReportDocument Reporte = new ReportDocument();

            try
            {
                Reporte.Load(Ruta_Archivo + "Rpt_Ort_Remodelacion_Ampliacion.rpt");
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
        ///NOMBRE DE LA FUNCIÓN: Generar_Reporte_Formato
        ///DESCRIPCIÓN:     Maneja la Extension del archivo
        ///PROPIEDADES:     String Ruta, direccion que 
        ///                 contiene el nombre del archivo al cual se le sacara la extension
        ///CREO:            Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:      03/Mayo/2012
        ///MODIFICO: 
        ///FECHA_MODIFICO: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************
        private void Generar_Reporte_Formato(String Clave_Unica, DateTime Fecha_Solucion, String Nombre_Completo, String Telefono, String Cuenta_Predial, String Perito_ID, String Solicitud_ID)
        {

            Cls_Ope_Solicitud_Tramites_Negocio Negocio_Actividades_Realizadas = new Cls_Ope_Solicitud_Tramites_Negocio();
            DataTable Dt_Datos_Inmueble = new DataTable();
            DataTable Dt_Datos_Propietario = new DataTable();
            DataTable Dt_Datos_Solictud = new DataTable();
            DataTable Dt_Perito = new DataTable();
            DataTable Dt_Ubicacion_Obra = new DataTable();
            DataRow Fila;
            DataTable Dt_Datos = (DataTable)(Session["Grid_Datos"]);
            String Valor_Dato = "";
            String[,] Datos;

            Ds_Rpt_Remodelacion_Ampliacion Ds_Reporte = new Ds_Rpt_Remodelacion_Ampliacion();
            DataSet Ds_Consulta = new DataSet();
            try
            {
                //  previsualizacion de la solicitud
                Dt_Datos_Inmueble = Ds_Reporte.Dt_Datos_Inmueble.Clone();
                Dt_Datos_Propietario = Ds_Reporte.Dt_Datos_Propietario.Clone();
                Dt_Datos_Solictud = Ds_Reporte.Dt_Datos_Solictud.Clone();
                Dt_Perito = Ds_Reporte.Dt_Perito.Clone();
                Dt_Ubicacion_Obra = Ds_Reporte.Dt_Ubicacion_Obra.Clone();


                //  para la ubicacion de la obra
                Negocio_Actividades_Realizadas.P_Cuenta_Predial = Cuenta_Predial;
                Dt_Ubicacion_Obra = Negocio_Actividades_Realizadas.Consultar_Datos_Obra();

                //  para los datos del perito
                Negocio_Actividades_Realizadas.P_Perito_ID = Perito_ID;
                Dt_Perito = Negocio_Actividades_Realizadas.Consultar_Inspectores();

                //  para los datos de la solicutd
                Fila = Dt_Datos_Solictud.NewRow();
                Fila["CLAVE_SOLICITUD"] = Clave_Unica;
                Fila["FECHA_TRAMITE"] = DateTime.Today;
                Fila["FECHA_ENTREGA"] = Convert.ToDateTime(Fecha_Solucion);
                Fila["SOLICITUD_ID"] = Solicitud_ID;
                Dt_Datos_Solictud.Rows.Add(Fila);


                if (Dt_Ubicacion_Obra != null && Dt_Ubicacion_Obra.Rows.Count > 0)
                {
                    foreach (DataRow Registro in Dt_Ubicacion_Obra.Rows)
                    {
                        //  para los datos del propietario
                        Fila = Dt_Datos_Propietario.NewRow();
                        Fila["NOMBRE"] = Nombre_Completo;
                        Fila["DOMICILIO"] = Registro["Calle"].ToString();
                        Fila["COLONIA"] = Registro["Colonia"].ToString();
                        Fila["TELEFONO"] = Telefono;
                        Dt_Datos_Propietario.Rows.Add(Fila);
                        break;
                    }
                }
                //para los datos de la solicitud
                if (Ds_Datos.Tables[0] != null && Ds_Datos.Tables[0].Rows.Count > 0)
                {
                    Datos = new String[Ds_Datos.Tables[0].Rows.Count, 2];
                    //  para obtener la informacion de los datos
                    for (int Contador_For = 0; Contador_For < Ds_Datos.Tables[0].Rows.Count; Contador_For++)
                    {
                        Datos[Contador_For, 0] = Ds_Datos.Tables[0].Rows[Contador_For].ItemArray[0].ToString();
                        String Nombre_Dato = Grid_Datos.Rows[Contador_For].Cells[1].Text;
                        Valor_Dato = ((TextBox)Grid_Datos.Rows[Contador_For].FindControl("Txt_Descripcion_Datos")).Text;
                        //  se agregan los campos a la tabla de datos del inmueble
                        Fila = Dt_Datos_Inmueble.NewRow();
                        Fila["NOMBRE_DATO"] = Nombre_Dato;
                        Fila["VALOR"] = Valor_Dato;
                        Dt_Datos_Inmueble.Rows.Add(Fila);
                    }
                }

                Dt_Ubicacion_Obra.TableName = "Dt_Ubicacion_Obra";
                Dt_Perito.TableName = "Dt_Perito";
                Dt_Datos_Solictud.TableName = "Dt_Datos_Solictud";
                Dt_Datos_Inmueble.TableName = "Dt_Datos_Inmueble";
                Dt_Datos_Propietario.TableName = "Dt_Datos_Propietario";

                Ds_Reporte.Clear();
                Ds_Reporte.Tables.Clear();
                Ds_Reporte.Tables.Add(Dt_Ubicacion_Obra.Copy());
                Ds_Reporte.Tables.Add(Dt_Perito.Copy());
                Ds_Reporte.Tables.Add(Dt_Datos_Solictud.Copy());
                Ds_Reporte.Tables.Add(Dt_Datos_Inmueble.Copy());
                Ds_Reporte.Tables.Add(Dt_Datos_Propietario.Copy());
                Generar_Reporte_Solicitud_Formato(Ds_Reporte, "PDF", "Formato_Solicitud");

            }
            catch (Exception ex)
            {
                throw new Exception("Alta_Tramite " + ex.Message.ToString(), ex);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Calles_Colonias
        ///DESCRIPCIÓN: cargara la informacion de las calles y colonias
        ///PARAMETROS: 
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  23/Mayo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Cargar_Combo_Calles_Colonias()
        {
            Cls_Cat_Ven_Registro_Usuarios_Negocio Negocio_Consulta = new Cls_Cat_Ven_Registro_Usuarios_Negocio(); 
            DataTable Dt_Calles = new DataTable();
            DataTable Dt_Colonias = new DataTable();
            try
            {
                //  para las calles
                Dt_Calles = Negocio_Consulta.Consultar_Calles();
                Dt_Colonias = Negocio_Consulta.Consultar_Colonia();


                //Cmb_Calle.DataSource = Dt_Calles;
                //Cmb_Calle.DataValueField = Cat_Pre_Calles.Campo_Nombre;
                //Cmb_Calle.DataTextField = Cat_Pre_Calles.Campo_Nombre;
                //Cmb_Calle.DataBind();
                //Cmb_Calle.Items.Insert(0, "< SELECCIONE >");

                Cmb_Colonias.DataSource = Dt_Colonias;
                Cmb_Colonias.DataValueField = Cat_Ate_Colonias.Campo_Colonia_ID;
                Cmb_Colonias.DataTextField = Cat_Ate_Colonias.Campo_Nombre;
                Cmb_Colonias.DataBind();
                Cmb_Colonias.Items.Insert(0, "< SELECCIONE >");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Mostrar_Archivo
        ///DESCRIPCIÓN:          Muestra un Archivo del cual se le pasa la ruta como parametro.
        ///PARAMETROS:           1.  Ruta.  Ruta del Archivo.
        ///CREO:                 Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:           24/Mayo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        public void Mostrar_Archivo(String Ruta)
        {
            try
            {
                if (System.IO.File.Exists(Ruta))
                {
                    String Archivo = "../../Portafolio/" + Cls_Sessiones.Ciudadano_ID + "/" + Path.GetFileName(Ruta);

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Archivo_Archivos", "window.open('" + Archivo + "','Window_Archivo','left=0,top=0')", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "nada", "alert('El archivo no existe');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "nada", "alert('" + ex.Message + "');", true);
            }
        }

        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Limpiar_Controles
        /// DESCRIPCION : Limpia los controles que se encuentran en la forma
        /// PARAMETROS  : 
        /// CREO        : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 18/Mayo/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Limpia_Controles()
        {
            try
            {
                Txt_Folio.Text = "";
                Txt_Apellido_Materno.Text = "";
                Txt_Apellido_Paterno.Text = "";
                Txt_Nombre.Text = "";
                Txt_Costo.Text = "";
                Txt_Tiempo_Estimado.Text = "";
                Txt_Nombre.Text = "";
                Txt_Apellido_Paterno.Text = "";
                Txt_Apellido_Materno.Text = "";
                Txt_Email.Text = "";
                Txt_Edad.Text = "";
                //Txt_Calle.Text = "";
                if (Cmb_Calle.SelectedIndex > 0) 
                    Cmb_Calle.SelectedIndex = 0;
                Txt_Colonia.Text = "";
                Txt_Numero.Text = "";
                if (Cmb_Colonias.SelectedIndex > 0) 
                    Cmb_Colonias.SelectedIndex = 0;
                Txt_Ciudad.Text = "IRAPUATO";
                Txt_Estado.Text = "GUANAJUATO";
                Txt_CP.Text = "";
                Cmb_Sexo.SelectedIndex = 0;
                Hdf_Usuario_ID.Value = "";
                Txt_Rfc.Text = "";
                Txt_Curp.Text = "";
                Txt_Fecha_Nacimiento.Text = "";
                Txt_Telefono_Casa.Text = "";
                Txt_Clave_Tramite.Text = "";
                Txt_Nombre_Tramite.Text = "";
                Txt_Descripcion.Text = "";
                Hdf_Tramite_ID.Value = "";
                Txt_Direccion_Predio.Text = "";
                Txt_Calle_Predio.Text = "";
                Txt_Numero_Predio.Text = "";
                Txt_Manzana_Predio.Text = "";
                Txt_Lote_Predio.Text = "";
                Txt_Otros_Predio.Text = "";
                Txt_Propietario_Cuenta_Predial.Text = "";
                Txt_Cuenta_Predial.Text = "";
                Cmb_Perito.SelectedIndex = 0;

                Grid_Datos.DataSource = new DataTable();
                Grid_Datos.DataBind();

                Grid_Documentos.DataSource = new DataTable();
                Grid_Documentos.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception("Limpia_Controles " + ex.Message.ToString());
            }
        }
        
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCIÓN: Alta_Usuario
        /// DESCRIPCION : Realizara el alta del usuario
        /// PARAMETROS  : 
        /// CREO        : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 18/Mayo/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public void Alta_Usuario()
        {
            Cls_Cat_Ven_Registro_Usuarios_Negocio Negocio_Alta_Usuario = new Cls_Cat_Ven_Registro_Usuarios_Negocio();
            String Sexo = "";
            DataTable Dt_Usuarios = new DataTable();
            String Nombre = "";
            String Email = "";
            String Password = "";
            try
            {
                Negocio_Alta_Usuario.P_Nombre = Txt_Nombre.Text.ToUpper().Trim();
                Negocio_Alta_Usuario.P_Apellido_Paterno = Txt_Apellido_Paterno.Text.ToUpper().Trim();
                Negocio_Alta_Usuario.P_Apellido_Materno = Txt_Apellido_Materno.Text.ToUpper().Trim();
                Negocio_Alta_Usuario.P_Nombre_Completo = Txt_Nombre.Text.ToUpper().Trim() + " " + Txt_Apellido_Paterno.Text.ToUpper().Trim() + " " + Txt_Apellido_Materno.Text.ToUpper().Trim();
                Negocio_Alta_Usuario.P_Edad = Txt_Edad.Text;
                Negocio_Alta_Usuario.P_Sexo = Cmb_Sexo.SelectedValue;
                Negocio_Alta_Usuario.P_Estatus = "ACTIVO";
                Negocio_Alta_Usuario.P_Calle = Cmb_Calle.SelectedItem.ToString() + " " + Txt_Numero.Text;
                Negocio_Alta_Usuario.P_Colonia = Cmb_Colonias.SelectedItem.ToString();
                Negocio_Alta_Usuario.P_Colonia_ID = Cmb_Colonias.SelectedValue;
                Negocio_Alta_Usuario.P_Calle_ID = Cmb_Calle.SelectedValue;
                Negocio_Alta_Usuario.P_Codigo_Postal = Txt_CP.Text;
                Negocio_Alta_Usuario.P_Ciudad = Txt_Ciudad.Text.ToUpper().Trim();
                Negocio_Alta_Usuario.P_Estado = Txt_Estado.Text.ToUpper().Trim();
                Negocio_Alta_Usuario.P_Rfc = Txt_Rfc.Text.ToUpper().Trim();
                Negocio_Alta_Usuario.P_Curp = Txt_Curp.Text.ToUpper().Trim();
                Negocio_Alta_Usuario.P_Email = Txt_Email.Text;
                Negocio_Alta_Usuario.P_Telefono_Casa = Txt_Telefono_Casa.Text;
                Negocio_Alta_Usuario.P_Fecha_Nacimiento = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Nacimiento.Text)); ;

                Dt_Usuarios = Negocio_Alta_Usuario.Consultar_Usuarios();

                if (Dt_Usuarios != null && Dt_Usuarios.Rows.Count > 0)
                {
                    if (Negocio_Alta_Usuario.P_Email != "")
                    {
                        Nombre = Dt_Usuarios.Rows[0][Cat_Ven_Usuarios.Campo_Nombre_Completo].ToString().Trim();
                        Email = Dt_Usuarios.Rows[0][Cat_Ven_Usuarios.Campo_Email].ToString().Trim();
                        Password = Dt_Usuarios.Rows[0][Cat_Ven_Usuarios.Campo_Password].ToString().Trim();
                        Negocio_Alta_Usuario.Enviar_Correo(Email, Password, Nombre);
                    }
                }
                else
                {
                    Negocio_Alta_Usuario.Guardar_Usuario();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Alta_Tramite " + ex.Message.ToString(), ex);
            }
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Habilitar_Forma
        ///DESCRIPCIÓN: es un metodo generico para habilitar todos los campos de la 
        ///forma que pueden ser editados
        ///PARAMETROS:
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 20/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Habilitar_Forma()
        {
            try
            {
                Txt_Folio.Text = "";
                Txt_Apellido_Materno.Text = "";
                Txt_Apellido_Paterno.Text = "";
                Txt_Nombre.Text = "";
                Txt_Costo.Text = "";
                Txt_Tiempo_Estimado.Text = "";
                Txt_Nombre.Text = "";
                Txt_Apellido_Paterno.Text = "";
                Txt_Apellido_Materno.Text = "";
                Txt_Email.Text = "";
                Txt_Edad.Text = "";
                //Txt_Calle.Text = "";
                if (Cmb_Calle.SelectedIndex > 0) 
                    Cmb_Calle.SelectedIndex = 0;
                Txt_Numero.Text = "";
                //Txt_Colonia.Text = "";
                if (Cmb_Colonias.SelectedIndex > 0) 
                    Cmb_Colonias.SelectedIndex = 0;
                Txt_Ciudad.Text = "IRAPUATO";
                Txt_Estado.Text = "GUANAJUATO";
                Txt_CP.Text = "";
                Cmb_Sexo.SelectedIndex = 0;
                Hdf_Usuario_ID.Value = "";
                //Txt_Filtro_Nombre.Text = "";
                Txt_Rfc.Text = "";
                Txt_Curp.Text = "";
                //  para las cajas de texto de los filtros del cuidadano
                //Txt_Filtro_Apellido_Materno.Text = "";
                //Txt_Filtro_Apellido_Paterno.Text = "";
                //Txt_Filtro_Curp.Text = "";
                //Txt_Filtro_Email.Text = "";
                //Txt_Filtro_Nombre.Text = "";
                //Txt_Filtro_RFC.Text = "";
                //Txt_Filtro_Telefono.Text = "";
                Txt_Fecha_Nacimiento.Text = "";

                Txt_Telefono_Casa.Text = "";
                Txt_Email.Enabled = true;
                Txt_Nombre.Enabled = true;
                Txt_Apellido_Paterno.Enabled = true;
                Txt_Apellido_Materno.Enabled = true;
                //Cmb_Tramite.SelectedIndex = 0;
                Cmb_Tramite.Enabled = true;
                Txt_Edad.Enabled = true;
                //Txt_Calle.Enabled = true;
                Txt_Numero.Enabled = true;
                Cmb_Calle.Enabled = true;
                //Txt_Colonia.Enabled = true;
                Cmb_Colonias.Enabled = true;
                Txt_Ciudad.Enabled = true;
                Txt_Estado.Enabled = true;
                Cmb_Estado.Enabled = true;
                Txt_CP.Enabled = true;
                Cmb_Sexo.Enabled = true;
                Txt_Direccion_Predio.Enabled = true;
                Txt_Calle_Predio.Enabled = true;
                Txt_Numero_Predio.Enabled = true;
                Txt_Manzana_Predio.Enabled = true;
                Txt_Lote_Predio.Enabled = true;
                Txt_Otros_Predio.Enabled = true;
                //Txt_Filtro_Email.Enabled = true;
                Txt_Propietario_Cuenta_Predial.Enabled = true;
                Txt_Rfc.Enabled = true;
                Txt_Curp.Enabled = true;
                Txt_Fecha_Nacimiento.Enabled = true;
                Txt_Telefono_Casa.Enabled = true;
                Btn_Busqueda_Tramite.Enabled = true;
                Txt_Colonia.Enabled = true;
                Btn_Link_Busqueda_Ciudadano.Enabled = true;

                Btn_Buscar_Colonia.Enabled = true;
                Btn_Buscar_Calle.Enabled = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Deshabilitar_Forma
        ///DESCRIPCIÓN: es un metodo generico para deshabilitar todos los campos de la 
        ///forma que pueden ser editados
        ///PARAMETROS:
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 20/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Deshabilitar_Forma()
        {
            try
            {
                Txt_Folio.Text = "";
                Cmb_Estatus.SelectedIndex = 0;
                Cmb_Tramite.SelectedIndex = 0;
                Cmb_Tramite.Enabled = false;
                Cmb_Estatus.Enabled = false;
                Txt_Email.Enabled = false;
                Txt_Email.Text = "";
                Txt_Apellido_Materno.Text = "";
                Txt_Apellido_Paterno.Text = "";
                Txt_Nombre.Text = "";
                Txt_Costo.Text = "";
                Txt_Tiempo_Estimado.Text = "";
                Txt_Avance.Text = "";
                Limpia_Controles();

                //  para las cajas de texto de los filtros del cuidadano
                //Txt_Filtro_Apellido_Materno.Text = "";
                //Txt_Filtro_Apellido_Paterno.Text = "";
                //Txt_Filtro_Curp.Text = "";
                //Txt_Filtro_Email.Text = "";
                //Txt_Filtro_Nombre.Text = "";
                //Txt_Filtro_RFC.Text = "";
                //Txt_Filtro_Telefono.Text = "";
                Txt_Colonia.Text = "";

                Txt_Nombre.Enabled = false;
                Txt_Apellido_Paterno.Enabled = false;
                Txt_Apellido_Materno.Enabled = true;
                Solicitud_Negocio.P_Clave_Solicitud = null;
                Solicitud_Negocio.P_Datos = null;
                Solicitud_Negocio.P_Documentos = null;
                Solicitud_Negocio.P_Estatus = null;
                Solicitud_Negocio.P_Porcentaje = null;
                Solicitud_Negocio.P_Solicitud_ID = null;
                Solicitud_Negocio.P_Tramite_ID = null;
                Solicitud_Negocio.P_Apellido_Materno = null;
                Solicitud_Negocio.P_Apellido_Paterno = null;
                Solicitud_Negocio.P_Nombre_Solicitante = null;
                Grid_Datos.Visible = false;
                Grid_Documentos.Visible = false;
                Grid_Documentos_Modificar.Visible = false;
                Lbl_Datos_Requeridos.Visible = false;
                Lbl_Documentos_Requeridos.Visible = false;
                Lbl_Mensaje_Documentos.Visible = false;
                Txt_Colonia.Enabled = false;
                Txt_Email.Enabled = false;
                Txt_Nombre.Enabled = false;
                Txt_Apellido_Paterno.Enabled = false;
                Txt_Apellido_Materno.Enabled = false;
                Cmb_Tramite.SelectedIndex = 0;
                Cmb_Tramite.Enabled = false;
                Txt_Edad.Enabled = false;
                Txt_Propietario_Cuenta_Predial.Enabled = false;
                //Txt_Calle.Enabled = false;
                Cmb_Calle.Enabled = false;
                Txt_Numero.Enabled = false;
                //Txt_Colonia.Enabled = false;
                Cmb_Colonias.Enabled = false;
                Txt_Ciudad.Enabled = false;
                Txt_Estado.Enabled = false;
                Txt_CP.Enabled = false;
                Cmb_Sexo.Enabled = false;
                //Txt_Filtro_Nombre.Enabled = false;
                Txt_Rfc.Enabled = false;
                Txt_Curp.Enabled = false;
                Txt_Fecha_Nacimiento.Enabled = false;
                Txt_Telefono_Casa.Enabled = false;
                Txt_Direccion_Predio.Enabled = false;
                Txt_Calle_Predio.Enabled = false;
                Txt_Numero_Predio.Enabled = false;
                Txt_Manzana_Predio.Enabled = false;
                Txt_Lote_Predio.Enabled = false;
                Txt_Otros_Predio.Enabled = false;
                //Txt_Filtro_Email.Enabled = false;
                Btn_Busqueda_Tramite.Enabled = false;
                Txt_Colonia.Enabled = false;
                Btn_Link_Busqueda_Ciudadano.Enabled = false;
                Btn_Buscar_Colonia.Enabled = false;
                Btn_Buscar_Calle.Enabled = false;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Mostrar_Informacion
        ///DESCRIPCIÓN: Habilita o deshabilita la muestra en pantalle del mensaje 
        ///de Mostrar_Informacion para el usuario
        ///PARAMETROS: 1.- Condicion, entero para saber si es 1 habilita para que se muestre mensaje si es cero
        ///deshabiñina para que no se muestre el mensaje
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 23/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Mostrar_Informacion(int Condicion)
        {
            try
            {
                if (Condicion == 1)
                {
                    //Lbl_Informacion.Enabled = true;
                    Img_Warning.Visible = true;
                }
                else
                {
                    Lbl_Informacion.Text = "";
                    //Lbl_Informacion.Enabled = false;
                    Img_Warning.Visible = false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Manejar_Botones
        ///DESCRIPCIÓN: es un metodo generico para habilitar y deshabilitar todos 
        ///los botones de la forma de acuerdo a sus eventos
        ///PARAMETROS: 1.- Modo, indica la forma en que se ´pondran los botones en 
        ///tanto a vidibilidad y tooltip
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 20/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        private void Manejar_Botones(int Modo)
        {
            try
            {
                switch (Modo)
                {
                    //Click en Nuevo
                    case 1:
                        Btn_Salir.ToolTip = "Cancelar";
                        Btn_Nuevo.Visible = false;
                        Btn_Guardar.Visible = true;
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        break;

                    //Click en Modificar
                    case 2:
                        Btn_Salir.ToolTip = "Cancelar";
                        Btn_Nuevo.Visible = false;
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        break;

                    //Estado Inicial
                    case 3:
                        Btn_Nuevo.Visible = true;
                        Btn_Salir.Visible = true;
                        Btn_Guardar.Visible = false;
                        Btn_Salir.ToolTip = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        break;
                    default: break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Validar_Expresion
        ///DESCRIPCIÓN: es un metodo generico para habilitar y deshabilitar todos 
        ///los botones de la forma de acuerdo a sus eventos
        ///PARAMETROS: 1.- Cadena, es la cadena de caracteres que se va a validar
        ///            2.- Tipo_Validacion.- que se requiere que la cadena contenga
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 19/Octubre/2010
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public Boolean Validar_Expresion(String Str_Cadena, String Str_Tipo_Validacion)
        {
            String Str_Expresion;
            Str_Expresion = String.Empty;
            //Se seleccion el tipo de valor a validar
            try
            {
                switch (Str_Tipo_Validacion)
                {
                    case "String":
                        Str_Expresion = "[^a-zA-Z.ÑÁÉÍÓñáéíóúü\\s]";
                        break;
                    case "Integer":
                        Str_Expresion = "[^0-9]";
                        break;
                    case "Varchar":
                        Str_Expresion = "[^a-zA-ZÑÁÉÍÓñáéíóúü\\/\\*,-.()0-9\\s]";
                        break;
                    case "Date":
                        Str_Expresion = "\\d{2}/\\d{2}/\\d{2}";
                        break;
                    case "Email":
                        Str_Expresion = "^\\w+([\\.-]?\\w+)*@\\w+([\\.-]?\\w+)*(\\.\\w{2,3})+$";
                        //@"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){2}(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5]))$";                        
                        break;
                    case "Password":
                        Str_Expresion = "[^a-zA-ZÑÁÉÍÓñáéíóúü.0-9\\s]";
                        break;
                    case "CURP":
                        Str_Expresion = "^[a-zA-Z]{4}(\\d{6})([a-zA-Z]{6})(\\d{2})?$";
                        break;
                }

                //Se revisa la expresion
                Regex Exp_Regular = new Regex(Str_Expresion);
                //Regresa un valor true o false segun se cumplan las condiciones
                if (Str_Tipo_Validacion == "Date" || Str_Tipo_Validacion == "Email" || Str_Tipo_Validacion == "CURP")
                    return !(Exp_Regular.IsMatch(Str_Cadena));
                else
                    return Exp_Regular.IsMatch(Str_Cadena);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }


        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Validar_Datos
        /// DESCRIPCION : Validar que se se encuentre todos los datos para continuar con el proceso
        /// CREO        : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 03/Mayo/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private Boolean Validar_Generacion_Rfc_Curp()
        {
            Boolean Estatus = true;

            try
            {
                if (Txt_Apellido_Paterno.Text != "" && Txt_Apellido_Materno.Text != "" && Txt_Nombre.Text != "" && Txt_Fecha_Nacimiento.Text != ""
                    && Cmb_Estado.SelectedIndex > 0)
                    Estatus = true;
              
                else
                    Estatus = false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return Estatus;
        }
           
        
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Validar_Datos
        /// DESCRIPCION : Validar que se se encuentre todos los datos para continuar con el proceso
        /// CREO        : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 03/Mayo/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private Boolean Validar_Datos()
        {
           

            String Espacios_Blanco = "";
            Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.

            Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            Lbl_Informacion.Text += Espacios_Blanco + "Es necesario Introducir: <br>";
            Lbl_Informacion.Visible = true;
            Img_Warning.Visible = true;

            if (String.IsNullOrEmpty(Hdf_Usuario_ID.Value))
            {
                if (Txt_Nombre.Text == "")
                {
                    Lbl_Informacion.Text += Espacios_Blanco + "*Ingrese el nombre.<br>";
                    Datos_Validos = false;
                }
                if (Txt_Apellido_Paterno.Text == "")
                {
                    Lbl_Informacion.Text += Espacios_Blanco + "*Ingrese el apellido paterno de quién solicita.<br>";
                    Datos_Validos = false;
                }

                if (Txt_Apellido_Materno.Text == "")
                {
                    Lbl_Informacion.Text += Espacios_Blanco + "*Ingrese el apellido materno de quién solicita.<br>";
                    Datos_Validos = false;
                }

                //if (Txt_Email.Text == "")
                //{
                //    Lbl_Informacion.Text += Espacios_Blanco + "*Ingrese el email de quién solicita.<br>";
                //    Datos_Validos = false;
                //}

                if (Txt_Edad.Text == "")
                {
                    Lbl_Informacion.Text += Espacios_Blanco + "*Ingrese la edad.<br>";
                    Datos_Validos = false;
                }
                if (Cmb_Calle.SelectedIndex == 0)
                {
                    Lbl_Informacion.Text += Espacios_Blanco + "*Ingrese la calle.<br>";
                    Datos_Validos = false;
                }
                if (Cmb_Colonias.SelectedIndex == 0)
                {
                    Lbl_Informacion.Text += Espacios_Blanco + "*Ingrese la colonia..<br>";
                    Datos_Validos = false;
                }
               
                if (Txt_CP.Text == "")
                {
                    Lbl_Informacion.Text += Espacios_Blanco + "*Ingrese el codigo postal.<br>";
                    Datos_Validos = false;
                } 
                if (Txt_Estado.Text == "")
                {
                    Lbl_Informacion.Text += Espacios_Blanco + "*Ingrese el estado.<br>";
                    Datos_Validos = false;
                }
                if (Txt_Rfc.Text == "")
                {
                    Lbl_Informacion.Text += Espacios_Blanco + "*Ingrese el rfc.<br>";
                    Datos_Validos = false;
                }
                if (Txt_Fecha_Nacimiento.Text == "")
                {
                    Lbl_Informacion.Text += Espacios_Blanco + "*Ingrese la fecha de nacimiento.<br>";
                    Datos_Validos = false;
                }
                if (Hdf_Tramite_ID.Value == "")
                {
                    Lbl_Informacion.Text += Espacios_Blanco + "*Seleccione un tramite.<br>";
                    Datos_Validos = false;
                }


                 if( Pnl_Cuenta_Predial.Visible == true)
                 {
                    if (Txt_Cuenta_Predial.Text == "")
                    {
                        if (Txt_Propietario_Cuenta_Predial.Text == "")
                        {
                            Lbl_Informacion.Text += Espacios_Blanco + "*Ingresa el propietario.<br>";
                            Datos_Validos = false;
                        }
                        if (Txt_Direccion_Predio.Text == "")
                        {
                            Lbl_Informacion.Text += Espacios_Blanco + "*Ingresa la direccion del predio.<br>";
                            Datos_Validos = false;
                        }
                        if (Txt_Calle_Predio.Text == "")
                        {
                            Lbl_Informacion.Text += Espacios_Blanco + "*Ingresa la calle del predio.<br>";
                            Datos_Validos = false;
                        }
                        if (Txt_Numero_Predio.Text == "")
                        {
                            Lbl_Informacion.Text += Espacios_Blanco + "*Ingresa el número del predio.<br>";
                            Datos_Validos = false;
                        }
                        //if (Txt_Manzana_Predio.Text == "")
                        //{
                        //    Lbl_Informacion.Text += Espacios_Blanco + "*Ingresa la manzana del predio.<br>";
                        //    Datos_Validos = false;
                        //}
                        //if (Txt_Lote_Predio.Text == "")
                        //{
                        //    Lbl_Informacion.Text += Espacios_Blanco + "*Ingresa el lote del predio.<br>";
                        //    Datos_Validos = false;
                        //}
                    }

                }
            }
            return Datos_Validos;
        }
        
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Validar_Datos_Grid_Datos
        /// DESCRIPCION : Validar que se se encuentre todos los datos para continuar con el proceso
        /// CREO        : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 23/Julio/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private Boolean Validar_Datos_Grid_Datos()
        {
            String Espacios_Blanco = "";
            Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
            String[,] Datos;
            try
            {

                Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                Lbl_Informacion.Text += Espacios_Blanco + "Es necesario Introducir: <br />";
                Lbl_Informacion.Visible = true;
                Img_Warning.Visible = true;


                if (Ds_Datos.Tables[0] == null)
                {
                    //  saca las dimenciones del arreglo
                    if (Ds_Datos.Tables[0].Rows.Count > 0)
                        Datos = new String[Ds_Datos.Tables[0].Rows.Count, 2];

                    //  para saber si cuenta con informacion 
                    for (int Contador_For = 0; Contador_For < Ds_Datos.Tables[0].Rows.Count; Contador_For++)
                    {
                        String Valor_Dato = ((TextBox)Grid_Datos.Rows[Contador_For].FindControl("Txt_Descripcion_Datos")).Text;

                        if (Valor_Dato != "" || (Ds_Datos.Tables[0].Rows[Contador_For][Cat_Tra_Datos_Tramite.Campo_Dato_Requerido].ToString()) == "N")
                        {
                        }

                        else
                        {
                            Lbl_Informacion.Text += Espacios_Blanco + "*Ingrese el dato de " +
                                    Ds_Datos.Tables[0].Rows[Contador_For][Cat_Tra_Datos_Tramite.Campo_Nombre] + ".<br />";
                            Datos_Validos = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Validar_Datos_Grid_Datos " + ex.Message.ToString(), ex);
            }

            return Datos_Validos;
        }
        
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Validar_Datos_Grid_Documentos
        /// DESCRIPCION : Validar que se se encuentre todos los datos para continuar con el proceso
        /// CREO        : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 03/Mayo/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private Boolean Validar_Datos_Grid_Documentos()
        {
            String Espacios_Blanco = "";
            Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.

            try
            {
                Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                Lbl_Informacion.Text += Espacios_Blanco + "Es necesario Introducir: <br />";
                Lbl_Informacion.Visible = true;
                Img_Warning.Visible = true;

                ////  para saber si cuenta con informacion (Validacion de documentos)
                if (Ds_Documentos.Tables[0] != null)
                {
                    for (int Contador_For = 0; Contador_For < Ds_Documentos.Tables[0].Rows.Count; Contador_For++)
                    {
                        AsyncFileUpload AsFileUp = (AsyncFileUpload)Grid_Documentos.Rows[Contador_For].Cells[1].FindControl("FileUp");
                        TextBox Txt_Url = (TextBox)Grid_Documentos.Rows[Contador_For].Cells[2].FindControl("Txt_Url");
                        FileUpload File_Acutalizar = (FileUpload)Grid_Documentos.Rows[Contador_For].Cells[1].FindControl("FileUp_Acutalizacion");

                        String Extension = Obtener_Extension(AsFileUp.FileName);


                        if (Ds_Documentos.Tables[0].Rows[Contador_For][Tra_Detalle_Documentos.Campo_Documento_Requerido].ToString() == "S")
                        {
                            if (Extension == "pdf" || Extension == "jpg" || Extension == "jpeg" || Extension == "rar" || Extension == "zip")
                            {

                            }
                            else
                            {
                                // para los documentos que se encuentra dentro del portafolio
                                if (Txt_Url.Text != "")
                                {

                                }
                                else
                                {
                                    Lbl_Informacion.Text += Espacios_Blanco + "*Ingrese el dato de " + Ds_Documentos.Tables[0].Rows[Contador_For]["DOCUMENTO"] + " ya que es un documento requerido.<br />";
                                    Datos_Validos = false;
                                }
                            }
                        }
                        else
                        {
                            if (Txt_Url.Text != "")
                            {

                            }
                            else if (Extension == "pdf" || Extension == "jpg" || Extension == "jpeg" || Extension == "rar" || Extension == "zip")
                            {

                            }
                            else if (Txt_Url.Text == "" && Extension == "")
                            {

                            }
                            else
                            {
                                Lbl_Informacion.Text += Espacios_Blanco + "*El formato del documento" + Ds_Documentos.Tables[0].Rows[Contador_For]["DOCUMENTO"] + " no es valido.<br />";
                                Datos_Validos = false;
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                throw new Exception("Validar_Datos_Grid_Documentos " + ex.Message.ToString(), ex);
            }

            return Datos_Validos;
        }

    #endregion

    #region Eventos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
        ///DESCRIPCIÓN: Habilita las cajas de texto necesarias para crear un Nueva solicitud de tramite
        ///y coloca un Asunto_Id de forma automatica en la caja de texto de Asunto_Id, se convierte en dar alta
        ///cuando oprimimos Nuevo y dar alta  Crea un registro de una solicitud en la base de datos 
        ///PARAMETROS: 
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 16/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Mostrar_Informacion(0);
                
                Limpia_Controles();
                Lbl_Informacion.Text = "";
                //Lbl_Informacion.Enabled = false;
                Img_Warning.Visible = false;
                Grid_Datos.Visible = false;
                Grid_Documentos.Visible = false;
                Grid_Documentos_Modificar.Visible = false;
                Lbl_Datos_Requeridos.Visible = false;
                Lbl_Documentos_Requeridos.Visible = false;
                Lbl_Mensaje_Documentos.Visible = false;
                Cargar_Combo_Calles_Colonias();
                Habilitar_Forma();
                Cmb_Estatus.SelectedIndex = 1;
                Txt_Avance.Text = "0";
              

                String Tramite_ID = "";

                if (Session["BUSQUEDA_TRAMITES"] != null)
                {
                    if (Session["BUSQUEDA_TRAMITES"].ToString() == "True")
                    {
                        Boolean Estado = Convert.ToBoolean(Session["BUSQUEDA_TRAMITES"].ToString());

                        if (Estado != false)
                        {
                            Hdf_Tramite_ID.Value = Session["TRAMITE_ID"].ToString();
                            Tramite_ID = Hdf_Tramite_ID.Value;
                            Refrescar_Grid_Datos(Tramite_ID);
                            Refrescar_Grid_Documentos(Tramite_ID);
                            Solicitud_Negocio.P_Tramite_ID = Tramite_ID;
                            Ds_Tramites = Solicitud_Negocio.Consultar_Tramites();
                            Txt_Clave_Tramite.Text = Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Clave_Tramite].ToString();
                            Txt_Nombre_Tramite.Text = Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Nombre].ToString();
                            Txt_Descripcion.Text = Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Descripcion].ToString();
                            Txt_Costo.Text = Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Costo].ToString();
                            Txt_Tiempo_Estimado.Text = Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Tiempo_Estimado].ToString();
                            Txt_Avance.Text = "0";
                            Cmb_Tramite.SelectedIndex = Cmb_Tramite.Items.IndexOf(Cmb_Tramite.Items.FindByValue(Tramite_ID));

                            Hdf_Dependencia_ID.Value = Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Dependencia_ID].ToString();
                            Solicitud_Negocio.P_Tramite_ID = Hdf_Tramite_ID.Value;
                            Ds_Tramites = Solicitud_Negocio.Consultar_Tramites();
                            if (Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Dependencia_ID].ToString() == "00069"
                               || Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Dependencia_ID].ToString() == "00070"
                               || Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Dependencia_ID].ToString() == "00071"
                               || Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Dependencia_ID].ToString() == "00072")
                            {
                                Pnl_Cuenta_Predial.Visible = true;
                                Hdf_Cuenta_Predial.Value = "Si";
                            }
                            else
                            {
                                Pnl_Cuenta_Predial.Visible = false;
                                Hdf_Cuenta_Predial.Value = "No";
                            }
                        }

                        Manejar_Botones(1);
                    }

                    else
                    {
                        Deshabilitar_Forma();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Guardar_Click
        ///DESCRIPCIÓN: Crea el nuevo registro en la base de datos
        ///PARAMETROS: 
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 16/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Btn_Guardar_Click(object sender, ImageClickEventArgs e)
        {
            Cls_Cat_Ven_Registro_Usuarios_Negocio Negocio_Consulta_ID = new Cls_Cat_Ven_Registro_Usuarios_Negocio();
            Boolean Error = true;
            String Consecutivo="";
            String[,] Datos = new String[0, 0];
            String[,] Documentos = new String[0, 0];
            DataTable Dt_Ciudadano_ID = new DataTable();
            String Directorio_Portafolio = "";
            String Extension = "";
            String URL = "";
            String Nombre_Archivo = "";
            String Direccion_Archivo = "";
            String Directorio_Expediente = "";
            String Raiz ="";
            try
            {
                Mostrar_Informacion(0);

                if (Validar_Datos())
                {
                    if (Validar_Datos_Grid_Datos())
                    {
                        Lbl_Informacion.Text = "";
                        Lbl_Informacion.Visible = false;
                        Img_Warning.Visible = false;

                        if (Validar_Datos_Grid_Documentos())
                        {
                            Error = false;
                            Lbl_Informacion.Text = "";
                            Lbl_Informacion.Visible = false;
                            Img_Warning.Visible = false;
                            //  se da de alta al usuario
                            if (Hdf_Usuario_ID.Value == "")
                            {
                                Alta_Usuario();
                            }


                            if (!String.IsNullOrEmpty(Hdf_Ciudadano_ID.Value))
                            {
                                Directorio_Portafolio = Hdf_Ciudadano_ID.Value;
                                Hdf_Usuario_ID.Value = Hdf_Ciudadano_ID.Value;
                            }

                            else if (!String.IsNullOrEmpty(Txt_Email.Text))
                            {
                                Negocio_Consulta_ID.P_Email = Txt_Email.Text;
                                Dt_Ciudadano_ID = Negocio_Consulta_ID.Consultar_Usuario_Soliucitante();

                                if (Dt_Ciudadano_ID != null && Dt_Ciudadano_ID is DataTable)
                                {
                                    if (Dt_Ciudadano_ID.Rows.Count > 0)
                                    {
                                        Directorio_Portafolio = Dt_Ciudadano_ID.Rows[0][Cat_Pre_Contribuyentes.Campo_Contribuyente_ID].ToString();
                                        Hdf_Usuario_ID.Value = Dt_Ciudadano_ID.Rows[0][Cat_Pre_Contribuyentes.Campo_Contribuyente_ID].ToString();
                                    }
                                }
                            }

                            Solicitud_Negocio.P_Porcentaje = "0";
                            Solicitud_Negocio.P_Tramite_ID = Hdf_Tramite_ID.Value;
                            Solicitud_Negocio.P_E_Mail = Txt_Email.Text;
                            DataSet Ds_Subproceso = Solicitud_Negocio.Consultar_Subproceso();
                            Solicitud_Negocio.P_Subproceso_ID = Ds_Subproceso.Tables[0].Rows[0].ItemArray[0].ToString();
                            Solicitud_Negocio.P_Estatus = Cmb_Estatus.SelectedValue;


                            if (Ds_Datos.Tables[0].Rows.Count > 0 && Ds_Datos.Tables[0] != null)
                                Datos = new String[Ds_Datos.Tables[0].Rows.Count, 2];

                            if (Ds_Documentos.Tables[0].Rows.Count > 0 && Ds_Documentos.Tables[0] != null)
                                Documentos = new String[Ds_Documentos.Tables[0].Rows.Count, 2];

                            //llenar matriz de datos
                            if (Ds_Datos.Tables[0].Rows.Count > 0 && Ds_Datos.Tables[0] != null)
                            {
                                for (int i = 0; i < Ds_Datos.Tables[0].Rows.Count; i++)
                                {
                                    for (int j = 0; j < 2; j++)
                                    {
                                        if (j == 0)
                                        {
                                            Datos[i, j] = Ds_Datos.Tables[0].Rows[i].ItemArray[0].ToString();
                                        }//fin if
                                        else
                                        {
                                            String Valor_Dato = ((TextBox)Grid_Datos.Rows[i].FindControl("Txt_Descripcion_Datos")).Text;
                                            if (Valor_Dato != "" ||
                                                (Ds_Datos.Tables[0].Rows[i][Cat_Tra_Datos_Tramite.Campo_Dato_Requerido].ToString()) == "N")
                                            {
                                                Datos[i, j] = Valor_Dato;
                                            }

                                        }//fin else
                                    }//fin for j
                                }//fin for i 
                            }// fin del if

                            //  infomracion que se enviara a la capa de negocios
                            if (Ds_Documentos.Tables[0].Rows.Count > 0 && Ds_Documentos.Tables[0] != null)
                            {
                                for (int Cnt_Documentos = 0; Cnt_Documentos < Ds_Documentos.Tables[0].Rows.Count; Cnt_Documentos++)
                                {

                                    for (int Cnt_Documentos_Celdas = 0; Cnt_Documentos_Celdas < 2; Cnt_Documentos_Celdas++)
                                    {
                                        AsyncFileUpload AsFileUp = (AsyncFileUpload)Grid_Documentos.Rows[Cnt_Documentos].Cells[2].FindControl("FileUp");
                                        String Nombre_Documento = Ds_Documentos.Tables[0].Rows[Cnt_Documentos][1].ToString();
                                        TextBox Txt_Url = (TextBox)Grid_Documentos.Rows[Cnt_Documentos].Cells[2].FindControl("Txt_Url");

                                        if (AsFileUp.FileName != "")
                                            Extension = Obtener_Extension(AsFileUp.FileName);
                                        else
                                            Extension = Obtener_Extension(Txt_Url.Text);

                                        if (Ds_Documentos.Tables[0].Rows[Cnt_Documentos][Tra_Detalle_Documentos.Campo_Documento_Requerido].ToString() == "S" ||
                                            AsFileUp.FileName != "" || Txt_Url.Text != "")
                                        {
                                            if (Cnt_Documentos_Celdas == 0)
                                                Documentos[Cnt_Documentos, Cnt_Documentos_Celdas] = Ds_Documentos.Tables[0].Rows[Cnt_Documentos].ItemArray[0].ToString();

                                            if (Cnt_Documentos_Celdas == 1)
                                            {
                                                Directorio_Expediente = "TR-";
                                                Raiz = @Server.MapPath("../../Archivos");

                                                Direccion_Archivo = Raiz + "/" + Directorio_Expediente +
                                                           "/" + Server.HtmlEncode(Ds_Documentos.Tables[0].Rows[Cnt_Documentos].ItemArray[3].ToString() +
                                                           "_" + Ds_Documentos.Tables[0].Rows[Cnt_Documentos].ItemArray[2].ToString() +
                                                           "." + Extension);

                                                Documentos[Cnt_Documentos, Cnt_Documentos_Celdas] = Direccion_Archivo;
                                            }
                                        }
                                    }
                                }
                            }

                            Solicitud_Negocio.P_Datos = Datos;
                            Solicitud_Negocio.P_Documentos = Documentos;
                            if (Txt_Apellido_Materno.Text.Length > 0)
                            {
                                Solicitud_Negocio.P_Apellido_Materno = Txt_Apellido_Materno.Text;
                            }
                            else
                            {
                                Solicitud_Negocio.P_Apellido_Materno = "X";
                            }
                            //  datos del perito
                            if (Cmb_Perito.SelectedIndex > 0)
                            {
                                Solicitud_Negocio.P_Perito_ID = Cmb_Perito.SelectedValue;
                            }
                            //  para la cuenta predial
                            Solicitud_Negocio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.ToUpper();
                            Solicitud_Negocio.P_Direccion_Predio = Txt_Direccion_Predio.Text.ToString().ToUpper();
                            if (Txt_Numero_Predio.Text.ToString().Length > 10)
                            {
                                Txt_Numero_Predio.Text = Txt_Numero_Predio.Text.Substring(0, 10);
                            }
                            Solicitud_Negocio.P_Nuemro_Predio = Txt_Numero_Predio.Text.ToString().ToUpper();
                            Solicitud_Negocio.P_Manzana_Predio = Txt_Manzana_Predio.Text.ToString().ToUpper();
                            Solicitud_Negocio.P_Lote_Predio = Txt_Lote_Predio.Text.ToString().ToUpper();
                            Solicitud_Negocio.P_Propietario_Predio = Txt_Propietario_Cuenta_Predial.Text.ToString().ToUpper();
                            Solicitud_Negocio.P_Calle_Predio = Txt_Calle_Predio.Text.ToString().ToUpper();
                            Solicitud_Negocio.P_Otros_Predio = Txt_Otros_Predio.Text.ToString().ToUpper();
                            DateTime Fecha_Solucion;
                            var Dias_Inhabilies = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();

                            Fecha_Solucion = Dias_Inhabilies.Calcular_Fecha("" + DateTime.Today, Txt_Tiempo_Estimado.Text);

                            Solicitud_Negocio.P_Fecha_Entrega = Fecha_Solucion;
                            Solicitud_Negocio.P_Apellido_Paterno = Txt_Apellido_Paterno.Text;
                            Solicitud_Negocio.P_Nombre_Solicitante = Txt_Nombre.Text;
                            Solicitud_Negocio.P_Contribuyente_ID = Hdf_Usuario_ID.Value;
                            String Clave_Unica = Cls_Util.Generar_Folio_Tramite();
                            Solicitud_Negocio.P_Clave_Solicitud = Clave_Unica;
                            Solicitud_Negocio.P_Folio = Txt_Clave_Tramite.Text.Trim();
                            Solicitud_Negocio.P_Contribuyente_ID = Hdf_Usuario_ID.Value;

                            //  se agregan los costos a la solicitud
                            Solicitud_Negocio.P_Costo_Base = Txt_Costo.Text;
                            Solicitud_Negocio.P_Cantidad = "1";
                            Solicitud_Negocio.P_Costo_Total = Txt_Costo.Text;

                            //   *********************** inicio alta la solicitud *******************************************************************
                            Consecutivo = Solicitud_Negocio.Alta_Solicitud(Cls_Sessiones.Nombre_Empleado);
                            //   *********************** fin alta la solicitud **********************************************************************


                            if (Ds_Documentos.Tables[0].Rows.Count > 0 && Ds_Documentos.Tables[0] != null)
                                Documentos = new String[Ds_Documentos.Tables[0].Rows.Count, 2];

                            //lenar matriz documentos
                            if (Ds_Documentos.Tables[0].Rows.Count > 0 && Ds_Documentos.Tables[0] != null)
                            {
                                for (int Cnt_Principal = 0; Cnt_Principal < Ds_Documentos.Tables[0].Rows.Count; Cnt_Principal++)
                                {
                                    for (int Cnt_Secundario = 0; Cnt_Secundario < 2; Cnt_Secundario++)
                                    {
                                        if (Cnt_Secundario == 0)
                                        {

                                            Documentos[Cnt_Principal, Cnt_Secundario] = Ds_Documentos.Tables[0].Rows[Cnt_Principal].ItemArray[0].ToString();

                                        }//fin if j= 0
                                        if (Cnt_Secundario == 1)
                                        {
                                            AsyncFileUpload AsFileUp = (AsyncFileUpload)Grid_Documentos.Rows[Cnt_Principal].Cells[2].FindControl("FileUp");
                                            String Nombre_Documento = Ds_Documentos.Tables[0].Rows[Cnt_Principal][2].ToString();
                                            TextBox Txt_Url = (TextBox)Grid_Documentos.Rows[Cnt_Principal].Cells[2].FindControl("Txt_Url");

                                            if (AsFileUp.FileName != "")
                                                Extension = Obtener_Extension(AsFileUp.FileName);
                                            else
                                                Extension = Obtener_Extension(Txt_Url.Text);


                                            if (Extension == "pdf" || Extension == "jpg" || Extension == "jpeg" || Extension == "zip" || Extension == "rar" || Txt_Url.Text != "")
                                            {
                                                //HttpPostedFile HttpFile = AsFileUp.PostedFile; 
                                                Directorio_Expediente = "TR-" + Consecutivo;
                                                Raiz = @Server.MapPath("../../Archivos");
                                                Direccion_Archivo = "";
                                                //verifica si existe el directorio donde se guardan los archivos
                                                // si no existe lo crea


                                                if (!Directory.Exists(Raiz))
                                                {
                                                    Directory.CreateDirectory(Raiz);
                                                }


                                                if (AsFileUp.FileName != "")
                                                {
                                                    URL = AsFileUp.FileName;
                                                }
                                                else
                                                {
                                                    URL = Txt_Url.Text;

                                                }
                                                Extension = Obtener_Extension(URL);


                                                //verifica que ya exista una url osea un archivo seleccionado para ser subido
                                                if (URL != "")
                                                {
                                                    //verifica si existe un directorio llamado con ese Nombre_Commando de expediente
                                                    if (!Directory.Exists(Raiz + Directorio_Expediente))
                                                    {
                                                        Directory.CreateDirectory(Raiz + "/" + Directorio_Expediente);
                                                    }//fin if si existe directorio expediente

                                                    //se crea el Nombre_Commando del archivo que se va a guardar
                                                    Direccion_Archivo = Raiz + "/" + Directorio_Expediente +
                                                        "/" + Server.HtmlEncode(Ds_Documentos.Tables[0].Rows[Cnt_Principal].ItemArray[3].ToString() +
                                                        "_" + Ds_Documentos.Tables[0].Rows[Cnt_Principal].ItemArray[2].ToString() +
                                                        "." + Extension);

                                                    //se valida que contega un archivo 
                                                    if (AsFileUp.HasFile)
                                                    {
                                                        if (System.IO.File.Exists(Direccion_Archivo))
                                                            System.IO.File.Delete(Direccion_Archivo);

                                                        //se guarda el archivo
                                                        AsFileUp.SaveAs(Direccion_Archivo);

                                                        // se subira el archivo al portafolio*************************
                                                        Raiz = @Server.MapPath("../../Portafolio");
                                                        URL = AsFileUp.FileName;


                                                        if (System.IO.File.Exists(MapPath("../../Portafolio/" + Directorio_Portafolio)))
                                                        {
                                                        }
                                                        else
                                                        {
                                                            Directory.CreateDirectory(MapPath("../../Portafolio/" + Directorio_Portafolio));

                                                        }
                                                        String[] Archivos = Directory.GetFiles(MapPath("../../Portafolio/" + Directorio_Portafolio + "/"));

                                                        //verifica si existe un directorio 
                                                        if (!Directory.Exists(Raiz + "/" + Directorio_Portafolio))
                                                        {
                                                            Directory.CreateDirectory(Raiz + "/" + Directorio_Portafolio);
                                                        }

                                                        //  se busca el archivo
                                                        for (Int32 Contador_For = 0; Contador_For < Archivos.Length; Contador_For++)
                                                        {
                                                            Nombre_Archivo = Path.GetFileName(Archivos[Contador_For].Trim());

                                                            if (Nombre_Archivo.Contains(Nombre_Documento))
                                                            {
                                                                System.IO.File.Delete(Archivos[Contador_For].Trim());
                                                                break;
                                                            }

                                                        }// fin del for



                                                        // ejemplo: Portafolio/0000000002/0000000003_
                                                        //          Ife.jpg
                                                        Direccion_Archivo = Raiz + "/" + Directorio_Portafolio + "/" + Server.HtmlEncode(Ds_Documentos.Tables[0].Rows[Cnt_Principal].ItemArray[3].ToString() +
                                                            "_" + Ds_Documentos.Tables[0].Rows[Cnt_Principal].ItemArray[2].ToString() + "." + Extension);

                                                        if (AsFileUp.HasFile)
                                                        {
                                                            //se guarda el archivo
                                                            AsFileUp.SaveAs(Direccion_Archivo);
                                                        }// fin del if (AFU_Subir_Archivo.HasFile)************************


                                                    }//fin if hasFile

                                                    else
                                                    {
                                                        System.IO.File.Copy(Txt_Url.Text, Direccion_Archivo);
                                                    }

                                                    Documentos[Cnt_Principal, Cnt_Secundario] = Direccion_Archivo;

                                                }//fin if url

                                            }//fin if extension

                                        }//fin if Cnt_Secundario == 1

                                    }//fin for Cnt_Secundario

                                }//fin for Cnt_Principal 

                            }// fin del if



                            String Cuenta_Predial_Visible = Hdf_Cuenta_Predial.Value;
                            String Nombre = Txt_Propietario_Cuenta_Predial.Text;
                            String Email = Txt_Email.Text;
                            String Perito_ID = Cmb_Perito.SelectedValue;
                            String Dependencia_ID = Hdf_Dependencia_ID.Value;
                            String Nombre_Propietario = Txt_Propietario_Cuenta_Predial.Text;
                            String Nombre_Tramite = "";
                            String Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim().ToUpper();

                            // si hay un trámite seleccionado, guardar dato para reporte
                            if (Cmb_Tramite.SelectedIndex > 0)
                            {
                                Nombre_Tramite = Cmb_Tramite.SelectedItem.Text;
                            }

                            if (Txt_Propietario_Cuenta_Predial.Text != "")
                                Nombre = Txt_Propietario_Cuenta_Predial.Text;

                            else
                                Nombre = Txt_Apellido_Paterno.Text + " " + Txt_Apellido_Materno.Text + " " + Txt_Nombre.Text;

                            //  se obtendra el id de la solicitud                    
                            DataTable Dt_Consutar_Por_Clave = Solicitud_Negocio.Consultar_Solicitud().Tables[0].Copy();
                            String Solicitud_ID = Consecutivo;


                            Cls_Ope_Bandeja_Tramites_Negocio Negocio_Datos_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
                            Negocio_Datos_Solicitud.P_Solicitud_ID = Solicitud_ID;
                            Negocio_Datos_Solicitud = Negocio_Datos_Solicitud.Consultar_Datos_Solicitud();



                            Generar_Reporte_Folio_Solicitud(Clave_Unica, Fecha_Solucion, Nombre, Email, Nombre_Tramite, Cuenta_Predial,
                                    Nombre_Propietario, Solicitud_ID, Negocio_Datos_Solicitud.P_Dependencia_ID, Negocio_Datos_Solicitud.P_Area_Dependencia);

                            Mostrar_Informacion(0);
                            Manejar_Botones(3);
                            Deshabilitar_Forma();
                            Limpia_Controles();
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Solicitud de Trámites", "alert('Solicitud registrada con el Folio: " + Clave_Unica + "');", true);

                            Div_Grid_Datos_Tramite.Style.Value = "display:block";
                            Div_Grid_Documentos.Style.Value = "display:block";
                            Div_Grid_Documentos_Modificar.Style.Value = "display:block";
                            Div_Direccion_Completa.Style.Value = "display:none";
                            Div_Direccion_Seperada.Style.Value = "display:block";
                            Pnl_Cuenta_Predial.Visible = false;

                        }
                    }
                }
                    
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
        ///DESCRIPCIÓN: Cancela la operacion actual qye se este realizando
        ///PARAMETROS: 
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 16/OCtubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
        {
            if (Btn_Salir.ToolTip == "Salir")
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                try
                {
                    Manejar_Botones(3);
                    Mostrar_Informacion(0);
                    Deshabilitar_Forma();
                    Limpia_Controles();
                    Div_Grid_Datos_Tramite.Style.Value = "display:block";
                    Div_Grid_Documentos.Style.Value = "display:block";
                    Div_Grid_Documentos_Modificar.Style.Value = "display:block";
                    Div_Link_Busqueda_Tramite.Style.Value = "display:block";
                    //Div_Consultar_Tramite.Style.Value = "color:#5D7B9D; display:none";
                    Pnl_Cuenta_Predial.Visible = false;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message.ToString());
                }
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Cmb_Tramite_SelectedIndexChanged
        ///DESCRIPCIÓN: Se interactua con los gris para cargagar en ellos los
        ///datos y documentops requeridos para el tramite seleccionado en el combo
        ///PARAMETROS:
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 4/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Cmb_Tramite_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Mostrar_Informacion(0);
                //if (Cmb_Tramite.SelectedIndex != 0)
                //{
                //    Refrescar_Grid_Datos();
                //    Refrescar_Grid_Documentos();
                //    Solicitud_Negocio.P_Tramite_ID = Cmb_Tramite.SelectedValue;
                //    Ds_Tramites = Solicitud_Negocio.Consultar_Tramites();
                //    Txt_Costo.Text = Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Costo].ToString();
                //    Txt_Tiempo_Estimado.Text = Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Tiempo_Estimado].ToString();
                //}
                //else
                //{
                //    Refrescar_Grid_Datos();
                //    Refrescar_Grid_Documentos();
                //    Txt_Costo.Text = "";
                //    Txt_Tiempo_Estimado.Text = "";
                //}
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Cmb_Colonia_SelectedIndexChanged
        ///DESCRIPCIÓN: Si se selecciona una colonia se actualiza el combo de calles de la colonia seleccionada
        ///PARÁMETROS: NO APLICA
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 16-may-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        protected void Cmb_Colonias_SelectedIndexChanged(object sender, EventArgs e)
        {
            var Obj_Calles = new Cls_Cat_Pre_Calles_Negocio();

            try
            {
                Cmb_Calle.Items.Clear();
                // cargar combo calles si hay una colonia seleccionada
                if (Cmb_Colonias.SelectedIndex > 0)
                {
                    Obj_Calles.P_Colonia_ID = Cmb_Colonias.SelectedValue;
                    Llenar_Combo_Con_DataTable(Cmb_Calle, Obj_Calles.Consultar_Calles(), 0, 5);
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message.ToString());
            }
        }
        ///*******************************************************************************
        ///NOMBRE:      Cmb_Estado_OnSelectedIndexChanged
        ///DESCRIPCIÓN: se cargara la colonia 
        ///PARAMETROS:
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  23/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Cmb_Estado_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Validar_Generacion_Rfc_Curp())
                {
                    Sugerir_RFC();
                }
                Txt_Fecha_Nacimiento.Focus();
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
        ///DESCRIPCIÓN: Busca un Asunto por medio del Nombre en la base de datos 
        ///y pone el resultado de las coincidencias de la busqueda en el grid
        ///PARAMETROS: 
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 23/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Mostrar_Informacion(0);
                //Solicitud_Negocio.P_Clave_Solicitud = Txt_Busqueda.Text;
                DataSet Ds_Solicitud = Solicitud_Negocio.Consultar_Solicitud();
                if (Ds_Solicitud.Tables[0].Rows.Count > 0)
                {
                    //colocar datos de la solicitud
                    Txt_Folio.Text = Ds_Solicitud.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Clave_Solicitud].ToString();
                    Txt_Nombre.Text = Ds_Solicitud.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Nombre_Solicitante].ToString();
                    Txt_Apellido_Materno.Text = Ds_Solicitud.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Apellido_Materno].ToString();
                    Txt_Apellido_Paterno.Text = Ds_Solicitud.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Apellido_Paterno].ToString();
                    Cmb_Tramite.SelectedValue = Ds_Solicitud.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Tramite_ID].ToString();
                    Cmb_Estatus.SelectedValue = Ds_Solicitud.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Estatus].ToString();
                    Txt_Avance.Text = Ds_Solicitud.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Porcentaje_Avance].ToString();
                    if (Ds_Solicitud.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Correo_Electronico].ToString() != "")
                        Txt_Email.Text = Ds_Solicitud.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Correo_Electronico].ToString();
                    else
                        Txt_Email.Text = "";
                    
                    Solicitud_Negocio.P_Tramite_ID = Cmb_Tramite.SelectedValue;
                    //colocar datos del tramite
                    Ds_Tramites = Solicitud_Negocio.Consultar_Tramites();
                    Txt_Costo.Text = Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Costo].ToString();
                    Txt_Tiempo_Estimado.Text = Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Tiempo_Estimado].ToString();
                    Solicitud_Negocio.P_Solicitud_ID = Ds_Solicitud.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Solicitud_ID].ToString();
                    Refrescar_Grid_Datos(Solicitud_Negocio.P_Tramite_ID);
                    Refrescar_Grid_Documentos_Modificar();
                    DataSet Ds_Datos_Solicitud = Solicitud_Negocio.Consultar_Datos_Solicitud();
                    DataSet Ds_Documentos_Solicitud = Solicitud_Negocio.Consultar_Documentos_Solicitud();
                    //            String Valor_Dato = "";

                    if (Ds_Datos.Tables[0] != null && Ds_Datos.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < Ds_Datos.Tables[0].Rows.Count; i++)
                        {
                            ((TextBox)Grid_Datos.Rows[i].FindControl("Txt_Descripcion_Datos")).Text = Ds_Datos_Solicitud.Tables[0].Rows[i][Ope_Tra_Datos.Campo_Valor].ToString();
                        }
                    }

                }
                else
                {
                    Lbl_Informacion.Text = "No se encontro registro de una solicitud con el número de folio proporcionado, <br/>" +
                        "sea tan amable de verificar";
                    Mostrar_Informacion(1);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Link_Busqueda_Tramite_Click
        ///DESCRIPCIÓN: habilita el div con los filtros para realizar la busqueda de los tramites
        ///PARAMETROS: 
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  17/Mayo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Btn_Link_Busqueda_Tramite_Click(object sender, EventArgs e)
        {
            String Tramite_ID = "";
            try
            {
                if (Session["BUSQUEDA_TRAMITES"] != null)
                {
                    if (Session["BUSQUEDA_TRAMITES"].ToString() == "True")
                    {
                        Boolean Estado = Convert.ToBoolean(Session["BUSQUEDA_TRAMITES"].ToString());

                        if (Estado != false)
                        {
                            Hdf_Tramite_ID.Value = Session["TRAMITE_ID"].ToString();
                            Tramite_ID = Hdf_Tramite_ID.Value;
                            Refrescar_Grid_Datos(Tramite_ID);
                            Refrescar_Grid_Documentos(Tramite_ID);
                            Solicitud_Negocio.P_Tramite_ID = Tramite_ID;
                            Ds_Tramites = Solicitud_Negocio.Consultar_Tramites();
                            Txt_Clave_Tramite.Text = Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Clave_Tramite].ToString();
                            Txt_Nombre_Tramite.Text = Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Nombre].ToString();
                            Txt_Descripcion.Text = Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Descripcion].ToString();
                            Txt_Costo.Text = Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Costo].ToString();
                            Txt_Tiempo_Estimado.Text = Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Tiempo_Estimado].ToString();
                            Txt_Avance.Text = "0";

                            Cmb_Tramite.SelectedIndex = Cmb_Tramite.Items.IndexOf(Cmb_Tramite.Items.FindByValue(Tramite_ID));

                            Hdf_Dependencia_ID.Value = Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Dependencia_ID].ToString();
                            Solicitud_Negocio.P_Tramite_ID = Hdf_Tramite_ID.Value;
                            Ds_Tramites = Solicitud_Negocio.Consultar_Tramites();
                            if (Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Dependencia_ID].ToString() == "00069"
                               || Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Dependencia_ID].ToString() == "00070"
                               || Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Dependencia_ID].ToString() == "00071"
                               || Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Dependencia_ID].ToString() == "00072")
                            {
                                Pnl_Cuenta_Predial.Visible = true;
                                Hdf_Cuenta_Predial.Value = "Si";
                            }
                            else
                            {
                                Pnl_Cuenta_Predial.Visible = false;
                                Hdf_Cuenta_Predial.Value = "No";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Cargar_Grid " + ex.Message.ToString());
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Link_Busqueda_Ciudadano_Click
        ///DESCRIPCIÓN: habilita el div con los filtros para realizar la busqueda de los tramites
        ///PARAMETROS: 
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  17/Mayo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Btn_Link_Busqueda_Ciudadano_Click(object sender, EventArgs e)
        {
            Cls_Cat_Ven_Registro_Usuarios_Negocio Negocio_Consulta_Usuario = new Cls_Cat_Ven_Registro_Usuarios_Negocio();
            String Usuario_ID = "";
            DataTable Dt_Consulta = new DataTable();
            int Valor = 0;
            try
            {
                if (Session["BUSQUEDA_CIUDADANO"] != null)
                {
                    Boolean Estado = Convert.ToBoolean(Session["BUSQUEDA_CIUDADANO"].ToString());

                    if (Estado != false)
                    {
                        Hdf_Ciudadano_ID.Value = Session["CIUDADANO_ID"].ToString();
                        Negocio_Consulta_Usuario.P_Ciudadano_ID = Session["CIUDADANO_ID"].ToString();
                        Dt_Consulta = Negocio_Consulta_Usuario.Consultar_Usuario_Soliucitante();
                        if (Dt_Consulta != null)
                        {

                            if (Dt_Consulta is DataTable)
                            {
                                //  se llenan los campos con la informacion
                                if (Dt_Consulta.Rows.Count == 1)
                                {
                                    Div_Direccion_Completa.Style.Value = "display:none";
                                    Div_Direccion_Seperada.Style.Value = "display:block";

                                    Usuario_ID = Dt_Consulta.Rows[0][Cat_Pre_Contribuyentes.Campo_Contribuyente_ID].ToString();
                                    Hdf_Usuario_ID.Value = Usuario_ID;
                                    Txt_Nombre.Text = Dt_Consulta.Rows[0][Cat_Pre_Contribuyentes.Campo_Nombre].ToString();
                                    Txt_Apellido_Paterno.Text = Dt_Consulta.Rows[0][Cat_Pre_Contribuyentes.Campo_Apellido_Paterno].ToString();
                                    Txt_Apellido_Materno.Text = Dt_Consulta.Rows[0][Cat_Pre_Contribuyentes.Campo_Apellido_Materno].ToString();
                                    Txt_Email.Text = Dt_Consulta.Rows[0][Cat_Pre_Contribuyentes.Campo_Email].ToString();
                                    Txt_Edad.Text = Dt_Consulta.Rows[0][Cat_Pre_Contribuyentes.Campo_Edad].ToString();

                                    Txt_Direccion_Completa.Text = Dt_Consulta.Rows[0][Cat_Pre_Contribuyentes.Campo_Calle_Ubicacion].ToString();
                                    Txt_Colonia.Text = Dt_Consulta.Rows[0][Cat_Pre_Contribuyentes.Campo_Colonia_Ubicacion].ToString();


                                    Cmb_Colonias.SelectedValue = Dt_Consulta.Rows[0][Cat_Pre_Contribuyentes.Campo_Colonia_ID].ToString();
                                    Cmb_Colonias_SelectedIndexChanged(sender, null);

                                    Cmb_Calle.SelectedValue = Dt_Consulta.Rows[0][Cat_Pre_Contribuyentes.Campo_Calle_ID].ToString();
                                    Txt_Numero.Text = Dt_Consulta.Rows[0][Cat_Pre_Contribuyentes.Campo_Calle_Ubicacion].ToString();

                                    Txt_Ciudad.Text = Dt_Consulta.Rows[0][Cat_Pre_Contribuyentes.Campo_Ciudad_Ubicacion].ToString();
                                    Txt_Estado.Text = Dt_Consulta.Rows[0][Cat_Pre_Contribuyentes.Campo_Estado_Ubicacion].ToString();
                                    Txt_CP.Text = Dt_Consulta.Rows[0][Cat_Pre_Contribuyentes.Campo_Codigo_Postal].ToString();
                                    Txt_Rfc.Text = Dt_Consulta.Rows[0][Cat_Pre_Contribuyentes.Campo_RFC].ToString();
                                    Txt_Curp.Text = Dt_Consulta.Rows[0][Cat_Pre_Contribuyentes.Campo_CURP].ToString();

                                    if (!String.IsNullOrEmpty(Dt_Consulta.Rows[0][Cat_Pre_Contribuyentes.Campo_Telefono_Casa].ToString()))
                                        Txt_Telefono_Casa.Text = Dt_Consulta.Rows[0][Cat_Pre_Contribuyentes.Campo_Telefono_Casa].ToString();

                                    else if (!String.IsNullOrEmpty(Dt_Consulta.Rows[0][Cat_Pre_Contribuyentes.Campo_Celular].ToString()))
                                        Txt_Telefono_Casa.Text = Dt_Consulta.Rows[0][Cat_Pre_Contribuyentes.Campo_Celular].ToString();

                                    Txt_Fecha_Nacimiento.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Dt_Consulta.Rows[0][Cat_Pre_Contribuyentes.Campo_Fecha_Nacimiento].ToString()));

                                    if (Dt_Consulta.Rows[0][Cat_Pre_Contribuyentes.Campo_Sexo].ToString() == "FEMENINO")
                                        Valor = 0;
                                    else
                                        Valor = 1;

                                    Cmb_Sexo.SelectedValue = Dt_Consulta.Rows[0][Cat_Pre_Contribuyentes.Campo_Sexo].ToString().ToUpper().Trim();
                                    Refrescar_Grid_Datos(Hdf_Tramite_ID.Value);
                                    Refrescar_Grid_Documentos(Hdf_Tramite_ID.Value);
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Cargar_Grid " + ex.Message.ToString());
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Ciudadano_Click
        ///DESCRIPCIÓN: habilita el div con los filtros para realizar la busqueda de los tramites
        ///PARAMETROS: 
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  17/Mayo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        //protected void Btn_Buscar_Ciudadano_Click(object sender, EventArgs e)
        //{
        //    //Div_Link_Buscar_Ciudadano.Style.Value = "display:none";
        //    //Div_Pnl_Buscar_Solicitante.Style.Value = "color:#5D7B9D; display:block";
        //}

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Colonia_Click
        ///DESCRIPCIÓN: Obtener de la variable de sesión el ID de la colonia seleccionada en la 
        ///             búsqueda avanzada
        ///PARAMETROS: 
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 17/may/2012
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************    
        protected void Btn_Buscar_Colonia_Click(object sender, ImageClickEventArgs e)
        {
            // validar que la variable de sesión existe
            if (Session["BUSQUEDA_COLONIAS"] != null)
            {
                // si el valor de la sesión es igual a true
                if (Convert.ToBoolean(Session["BUSQUEDA_COLONIAS"]) == true)
                {
                    try
                    {
                        string Colonia_ID = Session["COLONIA_ID"].ToString().Replace("&nbsp;", "");
                        // si el combo colonias contiene la colonia con el ID, seleccionar
                        if (Cmb_Colonias.Items.FindByValue(Colonia_ID) != null)
                        {
                            Cmb_Colonias.SelectedValue = Colonia_ID;
                            Cmb_Colonias_SelectedIndexChanged(null, null);
                        }
                    }
                    catch (Exception Ex)
                    {
                        throw new Exception(Ex.Message.ToString());
                        //Mostrar_Informacion(Ex.Message, true);
                    }

                    // limpiar variables de sesión
                    Session.Remove("COLONIA_ID");
                    Session.Remove("NOMBRE_COLONIA");
                }
                // limpiar variable de sesión
                Session.Remove("BUSQUEDA_COLONIAS");
            }
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Calles_Click
        ///DESCRIPCIÓN: Obtener de la variable de sesión el ID de la calle seleccionada en la 
        ///             búsqueda avanzada
        ///PARAMETROS: 
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 17/may/2012
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************    
        protected void Btn_Buscar_Calles_Click(object sender, ImageClickEventArgs e)
        {
            // validar que la variable de sesión existe
            if (Session["BUSQUEDA_COLONIAS_CALLES"] != null)
            {
                // si el valor de la sesión es igual a true
                if (Convert.ToBoolean(Session["BUSQUEDA_COLONIAS_CALLES"]) == true)
                {
                    var Obj_Calles = new Cls_Cat_Pre_Calles_Negocio();

                    try
                    {
                        string Calle_ID = Session["CALLE_ID"].ToString().Replace("&nbsp;", "");
                        string Colonia_ID = Session["COLONIA_ID"].ToString().Replace("&nbsp;", "");
                        // consultar las calles de la colonia a la que pertenece la calle seleccionada
                        Cmb_Calle.Items.Clear();
                        Obj_Calles.P_Colonia_ID = Colonia_ID;
                        Llenar_Combo_Con_DataTable(Cmb_Calle, Obj_Calles.Consultar_Calles(), 0, 5);
                        // si el combo colonias contiene la colonia con el ID, seleccionar
                        if (Cmb_Colonias.Items.FindByValue(Colonia_ID) != null)
                        {
                            Cmb_Colonias.SelectedValue = Colonia_ID;
                        }
                        // si el combo calles contiene un elemento con el ID, seleccionar
                        if (Cmb_Calle.Items.FindByValue(Calle_ID) != null)
                        {
                            Cmb_Calle.SelectedValue = Calle_ID;
                        }
                    }
                    catch (Exception Ex)
                    {
                        throw new Exception(Ex.Message.ToString());
                    }

                    // limpiar variables de sesión
                    Session.Remove("COLONIA_ID");
                    Session.Remove("CLAVE_COLONIA");
                    Session.Remove("CALLE_ID");
                    Session.Remove("CLAVE_CALLE");
                }
                // limpiar variable de sesión
                Session.Remove("BUSQUEDA_COLONIAS_CALLES");
            }

        }
        
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Perito_Click
        ///DESCRIPCIÓN: Obtener de la variable de sesión el ID del Perito seleccionado en la 
        ///             búsqueda avanzada
        ///PARAMETROS: 
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 10-jun-2012
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************    
        protected void Btn_Buscar_Perito_Click(object sender, ImageClickEventArgs e)
        {
            // limpiar mensaje de error
            Lbl_Informacion.Visible = false;
            Img_Warning.Visible = false;
            Lbl_Informacion.Text = "";

            // validar que la variable de sesión existe
            if (Session["BUSQUEDA_PERITOS"] != null)
            {
                // si el valor de la sesión es igual a true y el valor de la sesiones PERITO_ID no es nulo ni vacío
                if (Convert.ToBoolean(Session["BUSQUEDA_PERITOS"]) == true && Session["PERITO_ID"] != null && Session["PERITO_ID"].ToString().Length > 0)
                {
                    try
                    {
                        string Perito_ID = Session["PERITO_ID"].ToString().Replace("&nbsp;", "");
                        // si el combo colonias contiene la colonia con el ID, seleccionar
                        if (Cmb_Perito.Items.FindByValue(Perito_ID) != null)
                        {
                            Cmb_Perito.SelectedValue = Perito_ID;
                        }
                        else if (Session["NOMBRE_PERITO"] != null && Session["NOMBRE_PERITO"].ToString().Length > 0)
                        {
                            Cmb_Perito.Items.Add(new ListItem(HttpUtility.HtmlDecode(Session["NOMBRE_PERITO"].ToString()), Perito_ID));
                            Cmb_Perito.SelectedValue = Perito_ID;
                        }
                    }
                    catch (Exception Ex)
                    {
                        Lbl_Informacion.Visible = true;
                        Img_Warning.Visible = true;
                        Lbl_Informacion.Text = Ex.Message;
                    }

                    // limpiar variables de sesión
                    Session.Remove("PERITO_ID");
                    Session.Remove("NOMBRE_PERITO");
                }
                // limpiar variable de sesión
                Session.Remove("BUSQUEDA_PERITOS");
            }
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Avanzada_Click
        ///DESCRIPCIÓN: oculta el div con los filtros para realizar la busqueda de los tramites
        ///PARAMETROS: 
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  17/Mayo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Btn_Cerrar_Busqueda_Avanzada_Click(object sender, ImageClickEventArgs e)
        {
            Div_Link_Busqueda_Tramite.Style.Value = "display:block";
            //Div_Consultar_Tramite.Style.Value = "color:#5D7B9D; display:none";
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Actualizar_Documento_Click
        ///DESCRIPCIÓN: permitira actualizar el archivo
        ///PARAMETROS: 
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  24/Mayo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Btn_Actualizar_Documento_Click(object sender, ImageClickEventArgs e)
        {
            int Fila = 0;
            TableCell Celda = new TableCell();
            GridViewRow Renglon;
            ImageButton Boton = new ImageButton();
            try
            {
                Boton = (ImageButton)sender;
                Celda = (TableCell)Boton.Parent;
                Renglon = (GridViewRow)Celda.Parent;
                Grid_Documentos.SelectedIndex = Renglon.RowIndex;
                Fila = Renglon.RowIndex;

                ImageButton Btn_Acutalizar_Documento = (ImageButton)Grid_Documentos.Rows[Fila].Cells[1].FindControl("Btn_Acutalizar_Documento");
                ImageButton Btn_Ver_Documento = (ImageButton)Grid_Documentos.Rows[Fila].Cells[1].FindControl("Btn_Ver_Documento");
                AsyncFileUpload Afu_Subir_Archivo = (AsyncFileUpload)Grid_Documentos.Rows[Fila].Cells[1].FindControl("FileUp");
                TextBox Txt_Url = (TextBox)Grid_Documentos.Rows[Fila].Cells[1].FindControl("Txt_Url");

                //  se limpia la ruta del archivo
                Txt_Url.Text = "";
                //Txt_Url.Visible = true;
                //  se ocultan los botones de ver y actualizar
                Btn_Ver_Documento.Visible = false;
                Btn_Acutalizar_Documento.Visible = false;
                //  se muestra el boton de subir archivo
                Afu_Subir_Archivo.Visible = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Cargar_Grid " + ex.Message.ToString());
            }
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Ver_Documento_Click
        ///DESCRIPCIÓN: mostrara el documento
        ///PARAMETROS: 
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  24/Mayo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Btn_Ver_Documento_Click(object sender, ImageClickEventArgs e)
        {
            String URL = String.Empty;
            int Fila = 0;
            TableCell Celda = new TableCell();
            GridViewRow Renglon;
            ImageButton Boton = new ImageButton();
            String Nombre_Archivo = "";
            String Nombre_Documento = "";
            String Directorio_Portafolio = "";
            try
            {
                //  para obtener el id del documento 
                Boton = (ImageButton)sender;
                Celda = (TableCell)Boton.Parent;
                Renglon = (GridViewRow)Celda.Parent;
                Grid_Documentos.SelectedIndex = Renglon.RowIndex;
                Fila = Renglon.RowIndex;

                //  se obtiene el nombre del documento y el id del ciudadano
                Nombre_Documento = Grid_Documentos.Rows[Fila].Cells[1].Text.Trim();
                Directorio_Portafolio = Hdf_Usuario_ID.Value;

                //  se obtiene el nombre de los archivos existentes en la carpeta
                String[] Archivos = Directory.GetFiles(Server.MapPath("../../Portafolio/" + Directorio_Portafolio + "/"));

                //  se busca el archivo
                for (Int32 Contador = 0; Contador < Archivos.Length; Contador++)
                {
                    Nombre_Archivo = Path.GetFileName(Archivos[Contador].Trim());

                    if (Nombre_Archivo.Contains(Nombre_Documento))
                    {
                        URL = Archivos[Contador].Trim();
                        break;
                    }

                }// fin del for

                if (URL != null)
                {
                    Mostrar_Archivo(URL);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Cargar_Grid " + ex.Message.ToString());
            }
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Avanzada_Click
        ///DESCRIPCIÓN: oculta el div con los filtros para realizar la busqueda de los tramites
        ///PARAMETROS: 
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  22/Mayo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Btn_Cerrar_Busqueda_Ciudadano_Click(object sender, ImageClickEventArgs e)
        {
            Div_Link_Buscar_Ciudadano.Style.Value = "display:block";
            //Div_Pnl_Buscar_Solicitante.Style.Value = "color:#5D7B9D; display:none";
        }
    
    #endregion

    #region Grid
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Tramite_Filtro_Click
        ///DESCRIPCIÓN: cargara la informacion del tramite en las cajas de texto
        ///PARAMETROS: 
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  17/Mayo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Grid_Tramites_Generales_SelectedIndexChanged(object sender, EventArgs e)
        {
            String Tramite_ID = "";
            try
            {
                Mostrar_Informacion(0);
                Limpia_Controles();

                //Grid_Tramites_Generales.Columns[1].Visible = true;
                //GridViewRow Indice = Grid_Tramites_Generales.Rows[Grid_Tramites_Generales.SelectedIndex];
                //Tramite_ID = HttpUtility.HtmlDecode(Indice.Cells[1].Text).ToString();
                //Grid_Tramites_Generales.Columns[1].Visible = false;

                Hdf_Tramite_ID.Value = Tramite_ID;

                Refrescar_Grid_Datos(Tramite_ID);
                Refrescar_Grid_Documentos(Tramite_ID);
                Solicitud_Negocio.P_Tramite_ID = Tramite_ID;
                Ds_Tramites = Solicitud_Negocio.Consultar_Tramites();

                Txt_Costo.Text = Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Costo].ToString();
                Txt_Tiempo_Estimado.Text = Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Tiempo_Estimado].ToString();
                
                Txt_Avance.Text = "0";

                Div_Link_Busqueda_Tramite.Style.Value = "display:block";
                //Div_Consultar_Tramite.Style.Value = "color:#5D7B9D; display:none";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Grid_Documentos_RowDataBound
        ///DESCRIPCIÓN          :cargara los botones dentro del grid
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramirez Aguilera
        /// FECHA_CREO          : 24/Mayo/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        protected void Grid_Documentos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            String Nombre_Archivo = "";
            String Nombre_Documento = "";
            String Directorio_Portafolio = "";
            String Direccion_Archivo = "";
            Boolean Encontrado = false;
            String Raiz = ""; 
            Color Color_Requerido = Color.LightBlue;


            try
            {
                Directorio_Portafolio = Hdf_Usuario_ID.Value;
                Raiz = @Server.MapPath("../../Portafolio");

                if (Directorio_Portafolio != "")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        AsyncFileUpload Afu_Subir_Archivo = (AsyncFileUpload)e.Row.Cells[1].FindControl("FileUp");
                        TextBox Txt_Url = (TextBox)e.Row.Cells[1].FindControl("Txt_Url");
                        ImageButton Btn_Acutalizar_Documento = (ImageButton)e.Row.Cells[1].FindControl("Btn_Acutalizar_Documento");
                        ImageButton Btn_Ver_Documento = (ImageButton)e.Row.Cells[1].FindControl("Btn_Ver_Documento");


                        e.Row.Cells[3].Visible = true;
                        String Requerido = e.Row.Cells[3].Text;
                        e.Row.Cells[3].Visible = false;

                        if (!Directory.Exists(Raiz))
                        {
                            Directory.CreateDirectory(Raiz);
                        }

                        if (!Directory.Exists(Raiz + "/" + Directorio_Portafolio))
                        {
                            Directory.CreateDirectory(Raiz + "/" + Directorio_Portafolio);
                        }


                        String[] Archivos = Directory.GetFiles(MapPath("../../Portafolio/" + Directorio_Portafolio + "/"));
                        Nombre_Documento = e.Row.Cells[1].Text;

                        for (Int32 Contador = 0; Contador < Archivos.Length; Contador++)
                        {
                            Nombre_Archivo = Path.GetFileName(Archivos[Contador].Trim());

                            if (Nombre_Archivo.Contains(Nombre_Documento))
                            {
                                //  se carga la ruta del archivo
                                Txt_Url.Text = Archivos[Contador].Trim();

                                Btn_Acutalizar_Documento.Visible = true;
                                Btn_Ver_Documento.Visible = true;
                                Afu_Subir_Archivo.Visible = false;
                                Txt_Url.Visible = false;
                                Encontrado = true;
                                break;
                            }

                        }// fin del for

                        if (Encontrado == false)
                        {
                          

                            Afu_Subir_Archivo.Visible = true;
                            Txt_Url.Visible = false;
                            Btn_Acutalizar_Documento.Visible = false;
                            Btn_Ver_Documento.Visible = false;
                            Txt_Url.Visible = false;
                        }

                        if (Requerido == "N")
                        {
                            e.Row.Cells[2].BackColor = Color_Requerido;
                        }


                    }
                }

                else
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        e.Row.Cells[3].Visible = true;
                        String Requerido = e.Row.Cells[3].Text;
                        e.Row.Cells[3].Visible = false;

                        AsyncFileUpload Afu_Subir_Archivo = (AsyncFileUpload)e.Row.Cells[1].FindControl("FileUp");
                        TextBox Txt_Url = (TextBox)e.Row.Cells[1].FindControl("Txt_Url");
                        ImageButton Btn_Acutalizar_Documento = (ImageButton)e.Row.Cells[1].FindControl("Btn_Acutalizar_Documento");
                        ImageButton Btn_Ver_Documento = (ImageButton)e.Row.Cells[1].FindControl("Btn_Ver_Documento");

                        Afu_Subir_Archivo.Visible = true;
                        Txt_Url.Visible = false;
                        Btn_Acutalizar_Documento.Visible = false;
                        Btn_Ver_Documento.Visible = false;
                        Txt_Url.Visible = false;

                        if (Requerido == "N")
                        {
                            e.Row.Cells[2].BackColor = Color_Requerido;
                        }
                    }
                }

            }// fin del try
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }
    
    #endregion

    #region TextBox
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Txt_Apellido_Paterno_TextChanged
        ///DESCRIPCIÓN: Maneja el evento cambio de texto en la caja de texto Apellido paterno
        ///             llama al metodo para sugerir el RFC
        ///PROPIEDADES:     
        ///CREO: Roberto Gonzalez
        ///FECHA_CREO: 15-07-2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Txt_Apellido_Paterno_TextChanged(object sender, EventArgs e)
        {
            String RFC_Sugerido = "";

            try
            {
                if (Txt_Apellido_Paterno.Text != "" && Txt_Apellido_Materno.Text != "" && Txt_Nombre.Text != "" &&
                        Txt_Fecha_Nacimiento.Text != "")
                {
                    Sugerir_RFC();
                }

                Txt_Apellido_Materno.Focus();
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Txt_Apellido_Materno_TextChanged
        ///DESCRIPCIÓN: Maneja el evento cambio de texto en la caja de texto Apellido paterno
        ///             llama al metodo para sugerir el RFC
        ///PROPIEDADES:     
        ///CREO: Roberto Gonzalez
        ///FECHA_CREO: 15-07-2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Txt_Apellido_Materno_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Validar_Generacion_Rfc_Curp())
                {
                    Sugerir_RFC();
                }
                Txt_Fecha_Nacimiento.Focus();
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Txt_Nombre_TextChanged
        ///DESCRIPCIÓN: Maneja el evento cambio de texto en la caja de texto Apellido paterno
        ///             llama al metodo para sugerir el RFC
        ///PROPIEDADES:     
        ///CREO: Roberto Gonzalez
        ///FECHA_CREO: 15-07-2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Txt_Nombre_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Validar_Generacion_Rfc_Curp())
                {
                    Sugerir_RFC();
                }
                Txt_Apellido_Paterno.Focus();
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Txt_Fecha_Nacimiento_TextChanged
        ///DESCRIPCIÓN: Maneja el evento cambio de texto en la caja de texto Apellido paterno
        ///             llama al metodo para sugerir el RFC
        ///PROPIEDADES:     
        ///CREO: Roberto Gonzalez
        ///FECHA_CREO: 15-07-2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Txt_Fecha_Nacimiento_TextChanged(object sender, EventArgs e)
        {
            DateTime Dtime_Fecha;
            try
            {
                if (Txt_Fecha_Nacimiento.Text != "")
                {
                    Dtime_Fecha = Convert.ToDateTime(Txt_Fecha_Nacimiento.Text);
                    int Edad = (DateTime.Now.Year - Dtime_Fecha.Year);
                    Txt_Edad.Text = Edad.ToString();
                    Txt_Email.Focus();
                }

                if (Validar_Generacion_Rfc_Curp())
                {
                    Sugerir_RFC();
                }

            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }

        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Sugerir_RFC
        ///DESCRIPCIÓN: Si el campo no contiene 10 caracteres o mas y ya se llenaron los 
        ///         apellidos, el nombre y la fecha de nacimiento, se sugiere un RFC
        ///PROPIEDADES:     
        ///CREO: Roberto Gonzalez Oseguera
        ///FECHA_CREO: 15-07-2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected String Sugerir_RFC()
        {
            Cls_Ayudante_Curp_Rfc Rs_Curp_Rfc = new Cls_Ayudante_Curp_Rfc();
            String RFC = "";

            try
            {
                Rs_Curp_Rfc.P_Apellido_Paterno = Txt_Apellido_Paterno.Text;
                Rs_Curp_Rfc.P_Apellido_Materno = Txt_Apellido_Materno.Text;
                Rs_Curp_Rfc.P_Nombre = Txt_Nombre.Text;
                Rs_Curp_Rfc.P_Sexo = Cmb_Sexo.SelectedValue;
                Rs_Curp_Rfc.P_Entidad_Federativa = Cmb_Estado.SelectedValue;
                Rs_Curp_Rfc.P_Fecha_Nacimiento = Convert.ToDateTime(Txt_Fecha_Nacimiento.Text);
                Rs_Curp_Rfc.Calcular();

                Txt_Rfc.Text = Rs_Curp_Rfc.P_RFC;
                Txt_Curp.Text = Rs_Curp_Rfc.P_CURP;
            }
            catch (Exception ex)
            {
                Lbl_Informacion.Visible = true;
                Img_Warning.Visible = true;
                Lbl_Informacion.Text = ex.Message;
            }
            return RFC;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Es_Vocal
        ///DESCRIPCIÓN: Si el char que se recibe es vocal, regresa verdadero
        ///PROPIEDADES:     
        ///CREO: Roberto Gonzalez Oseguera
        ///FECHA_CREO: 15-07-2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private bool Es_Vocal(char letter)
        {
            return letter == 'a' ||
            letter == 'e' ||
            letter == 'i' ||
            letter == 'o' ||
            letter == 'u' ||
            letter == 'á' ||
            letter == 'é' ||
            letter == 'í' ||
            letter == 'ó' ||
            letter == 'ú';
        }

    #endregion

    #region TextBox
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Txt_Cuenta_Predial_TextChanged
        ///DESCRIPCIÓN: habilitara el boton de busqueda de cuenta predial
        ///PARAMETROS: 
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  09/Julio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Txt_Cuenta_Predial_TextChanged(object sender, EventArgs e)
        {
            string Cuenta_Predial_Id = "";

            // limpiar mensaje de error
            Lbl_Informacion.Visible = false;
            Img_Warning.Visible = false;
            Lbl_Informacion.Text = "";

            try
            {
                // limpiar y ocultar el campo propietario y el botón resumen predio
                Btn_Buscar_Cuenta_Predial.Visible = false;
                Txt_Propietario_Cuenta_Predial.Text = "";
                Txt_Propietario_Cuenta_Predial.Text = "";
                Txt_Direccion_Predio.Text = "";
                Txt_Calle_Predio.Text = "";
                Txt_Numero_Predio.Text = "";
                Txt_Manzana_Predio.Text = "";
                Txt_Lote_Predio.Text = "";
                // si el campo cuenta predial contiene texto, buscar el id de la cuenta
                if (Txt_Cuenta_Predial.Text.Length > 0)
                {
                    // llamar al método que consulta la cuenta predial id con la cuenta ingresada
                    if (!String.IsNullOrEmpty(Txt_Cuenta_Predial.Text))
                    {
                        Cls_Ope_Solicitud_Tramites_Negocio Negocio = new Cls_Ope_Solicitud_Tramites_Negocio();
                        Negocio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.ToUpper();
                        DataTable Dt_Predio = Negocio.Consultar_Cuenta_Predial();

                        if (Dt_Predio.Rows.Count > 0)
                        {
                            Txt_Propietario_Cuenta_Predial.Text = String.IsNullOrEmpty(Dt_Predio.Rows[0]["PROPIETARIO"].ToString()) ? ""
                                : Dt_Predio.Rows[0]["PROPIETARIO"].ToString();
                            Txt_Direccion_Predio.Text = String.IsNullOrEmpty(Dt_Predio.Rows[0]["COLONIA"].ToString()) ? ""
                                : Dt_Predio.Rows[0]["COLONIA"].ToString();
                            Txt_Calle_Predio.Text = String.IsNullOrEmpty(Dt_Predio.Rows[0]["CALLE"].ToString()) ? ""
                                : Dt_Predio.Rows[0]["CALLE"].ToString();
                            Txt_Numero_Predio.Text = String.IsNullOrEmpty(Dt_Predio.Rows[0]["NO_EXTERIOR"].ToString()) ? ""
                                : Dt_Predio.Rows[0]["NO_EXTERIOR"].ToString();
                            Txt_Manzana_Predio.Text = String.IsNullOrEmpty(Dt_Predio.Rows[0]["MANZANA"].ToString()) ? ""
                                : Dt_Predio.Rows[0]["MANZANA"].ToString();
                            Txt_Lote_Predio.Text = String.IsNullOrEmpty(Dt_Predio.Rows[0]["LOTE"].ToString()) ? ""
                                : Dt_Predio.Rows[0]["LOTE"].ToString();
                        }
                    }
                    else
                    {
                        // mostrar mensaje indicando que no se encontró la cuenta
                        Lbl_Informacion.Visible = true;
                        Img_Warning.Visible = true;
                        Lbl_Informacion.Text = "La Cuenta Predial proporcionada no se encuentra en el sistema.<br /><br />";
                    }
                    //Cuenta_Predial_Id = Consultar_Cuenta_Predial_ID(Txt_Cuenta_Predial.Text.Trim().ToUpper());
                    //// validar que la se haya obtenido una valor para la cuenta
                    //if (!string.IsNullOrEmpty(Cuenta_Predial_Id))
                    //{
                    //    Txt_Propietario_Cuenta_Predial.Text = Consultar_Propietario(Cuenta_Predial_Id);
                    //    Btn_Buscar_Cuenta_Predial.Visible = true;
                    //    Cargar_Ventana_Emergente_Resumen_Predio();
                    //}
                    //else
                    //{
                    //    // mostrar mensaje indicando que no se encontró la cuenta
                    //    Lbl_Informacion.Visible = true;
                    //    Img_Warning.Visible = true;
                    //    Lbl_Informacion.Text = "La Cuenta Predial proporcionada no se encuentra en el sistema.<br /><br />";
                    //}
                }
            }
            catch (Exception ex)
            {
                Lbl_Informacion.Visible = true;
                Img_Warning.Visible = true;
                Lbl_Informacion.Text = ex.Message;
            }
        }
        
    #endregion
}
