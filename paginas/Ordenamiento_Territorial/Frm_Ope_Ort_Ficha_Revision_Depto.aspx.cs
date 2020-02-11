using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Solicitud_Tramites.Negocios;
using Presidencia.Ordenamiento_Territorial_Ficha_Revision_Depto.Negocio;
using Presidencia.Ordenamiento_Territorial_Inspectores.Negocio;
using Presidencia.Cls_Cat_Ven_Registro_Usuarios.Negocio;

public partial class paginas_Ordenamiento_Territorial_Frm_Ope_Ort_Ficha_Revision_Depto : System.Web.UI.Page
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
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!IsPostBack)
            {
                Configuracion_Formulario(false);
                Llenar_Grid_Listado(0);

                // registro de scripts del lado del servidor para mostrar ventanas emergentes para búsqueda avanzada
                string Ventana_Modal = "Abrir_Ventana_Modal('../Tramites/Ventanas_Emergente/Frm_Busqueda_Avanzada_Solicitud_Tramite.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                Btn_Buscar_Solicitud.Attributes.Add("OnClick", Ventana_Modal);

                Ventana_Modal = "Abrir_Ventana_Modal('../Tramites/Ventanas_Emergente/Frm_Busqueda_Avanzada_Ciudadano.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                Btn_Buscar_Propietario.Attributes.Add("OnClick", Ventana_Modal);

                Ventana_Modal = "Abrir_Ventana_Modal('../Ventanilla/Ventanas_Emergentes/Frm_Ven_Busqueda_Avanzada_Peritos.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                Btn_Buscar_Perito.Attributes.Add("OnClick", Ventana_Modal);
            }
            Div_Contenedor_Msj_Error.Visible = false;
        }

    #endregion

    #region Metodos

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
            
            Txt_Nombre_Propietario.Enabled = Estatus;
            Txt_Codigo_Postal.Enabled = Estatus;
            Txt_Calle_Ubicacion.Enabled = Estatus;
            Txt_Colonia_Ubicacion.Enabled = Estatus;
            Txt_Ciudad_Ubicacion.Enabled = Estatus;
            Txt_Estado_Ubicacion.Enabled = Estatus;

            Txt_Solicitud_ID.Enabled = false;
            Txt_Tipo_Tramite.Enabled = false;
            Txt_Tramite.Enabled = false;

            Txt_Avance_Obra.Enabled = Estatus;
            Cmb_Cumplimiento_Normas.Enabled = Estatus;
            Cmb_Documentos_Dictamen.Enabled = Estatus;
            Cmb_Documentos_Propiedad.Enabled = Estatus;
            Txt_Observacion_Juridica.Enabled = Estatus;
            Txt_Observacion_Tecnica.Enabled = Estatus;
            Txt_Perito.Enabled = false;
            Txt_Ubicacion_Construccion.Enabled = Estatus;
            Txt_Inicio_Permiso.Enabled = false;
            Txt_Fin_Permiso.Enabled = false;

            Grid_Listado.SelectedIndex = (-1);
            Btn_Buscar_Solicitud.Visible = Estatus;
            Btn_Buscar_Propietario.Visible = Estatus;
            Btn_Buscar_Perito.Visible = Estatus;
            Btn_Inicio_Permiso.Visible = Estatus;
            Btn_Fin_Permiso.Visible = Estatus;

            Cls_Sessiones.Ciudadano_ID = "Ciudadano_ID";
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
            try
            {
                Grid_Listado.SelectedIndex = (-1);
                Cls_Ope_Ort_Ficha_Revision_Depto_Negocio Negocio = new Cls_Ope_Ort_Ficha_Revision_Depto_Negocio();
                Grid_Listado.Columns[1].Visible = true;
                Grid_Listado.DataSource = Negocio.Consultar_Tabla_Ficha_Revision_Depto();
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
            Hdf_Ficha_Revision_ID.Value = "";
            Hdf_Solicitud_ID.Value = "";
            Txt_Avance_Obra.Text = "";
            Txt_Calle_Ubicacion.Text = "";
            Txt_Ciudad_Ubicacion.Text = "";
            Txt_Codigo_Postal.Text = "";
            Txt_Colonia_Ubicacion.Text = "";
            Txt_Estado_Ubicacion.Text = "";
            Txt_Fin_Permiso.Text = "";
            Txt_Inicio_Permiso.Text = "";
            Txt_Nombre_Propietario.Text = "";
            Txt_Observacion_Juridica.Text = "";
            Txt_Observacion_Tecnica.Text = "";
            Txt_Perito.Text = "";
            Txt_Solicitud_ID.Text = "";
            Txt_Tipo_Tramite.Text = "";
            Txt_Tramite.Text = "";
            Txt_Ubicacion_Construccion.Text = "";
            Cmb_Cumplimiento_Normas.SelectedIndex = 0;
            Cmb_Documentos_Dictamen.SelectedIndex = 0;
            Cmb_Documentos_Propiedad.SelectedIndex = 0;
            Session["PERITO_ID"] = "";
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Cargar_Tramite
        ///DESCRIPCIÓN         : Carga los datos del Tramite
        ///PARÁMETROS          :
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///*******************************************************************************
        private void Cargar_Tramite(String Solicitud_ID)
        {
            Cls_Ope_Solicitud_Tramites_Negocio Negocio = new Cls_Ope_Solicitud_Tramites_Negocio();
            Negocio.P_Solicitud_ID = Solicitud_ID;

            DataSet Ds_Solicitud = Negocio.Consultar_Solicitud();
            if (Ds_Solicitud.Tables.Count >= 0)
            {
                if (Ds_Solicitud.Tables[0].Rows.Count >= 0)
                {
                    Negocio.P_Tramite_ID = Ds_Solicitud.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Tramite_ID].ToString();
                    Hdf_Solicitud_ID.Value = Ds_Solicitud.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Solicitud_ID].ToString();
                    DataSet Ds_Tramite = Negocio.Consultar_Tramites();
                    if (Ds_Tramite.Tables.Count >= 0)
                    {
                        if (Ds_Tramite.Tables[0].Rows.Count >= 0)
                        {
                            Txt_Tipo_Tramite.Text = Ds_Tramite.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Clave_Tramite].ToString();
                            Txt_Tramite.Text = Ds_Tramite.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Nombre].ToString();
                        }
                    }
                }
            }
            Txt_Solicitud_ID.Text = Hdf_Solicitud_ID.Value;
            Session["SOLICITUD_ID"] = "";
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
        private void Mostrar_Datos(String Ficha_Revision_ID)
        {
            Cls_Ope_Ort_Ficha_Revision_Depto_Negocio Negocio = new Cls_Ope_Ort_Ficha_Revision_Depto_Negocio();
            Negocio.P_Ficha_Revision_ID = Ficha_Revision_ID;
            Negocio = Negocio.Consultar_Ficha_Revision_Depto();

            Txt_Nombre_Propietario.Text = Negocio.P_Nombre_Propietario;
            Txt_Codigo_Postal.Text = Negocio.P_Codigo_Postal;
            Txt_Calle_Ubicacion.Text = Negocio.P_Calle_Ubicacion;
            Txt_Colonia_Ubicacion.Text = Negocio.P_Colonia_Ubicacion;
            Txt_Ciudad_Ubicacion.Text = Negocio.P_Ciudad_Ubicacion;
            Txt_Estado_Ubicacion.Text = Negocio.P_Estado_Ubicacion;

            Hdf_Solicitud_ID.Value = Negocio.P_Solicitud_ID;
            Cargar_Tramite(Hdf_Solicitud_ID.Value);

            Txt_Avance_Obra.Text = Negocio.P_Avance_Obra;
            Cmb_Cumplimiento_Normas.SelectedIndex = Cmb_Cumplimiento_Normas.Items.IndexOf(Cmb_Cumplimiento_Normas.Items.FindByValue(Negocio.P_Cumplimiento_Norma));
            Cmb_Documentos_Dictamen.SelectedIndex = Cmb_Documentos_Dictamen.Items.IndexOf(Cmb_Documentos_Dictamen.Items.FindByValue(Negocio.P_Documentos_Dictamen));
            Cmb_Documentos_Propiedad.SelectedIndex = Cmb_Documentos_Propiedad.Items.IndexOf(Cmb_Documentos_Propiedad.Items.FindByValue(Negocio.P_Documentos_Propiedad));
            Txt_Observacion_Juridica.Text = Negocio.P_Observacion_Juridica;
            Txt_Observacion_Tecnica.Text = Negocio.P_Observacion_Tecnica;
            Txt_Perito.Text = Negocio.P_Perito;
            Txt_Ubicacion_Construccion.Text = Negocio.P_Ubicacion_Construccion;
            Txt_Inicio_Permiso.Text = String.Format("{0:dd/MMM/yyyy}", Negocio.P_Inicio_Permiso, new CultureInfo("es-MX"));
            Txt_Fin_Permiso.Text = String.Format("{0:dd/MMM/yyyy}", Negocio.P_Fin_Permiso, new CultureInfo("es-MX"));
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
                if (Txt_Nombre_Propietario.Text.Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir un valor para el campo Nombre Propietario.";
                    Validacion = false;
                }
                if (Txt_Codigo_Postal.Text.Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir un valor para el campo Codigo Postal.";
                    Validacion = false;
                }
                if (Txt_Calle_Ubicacion.Text.Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir un valor para el campo Calle.";
                    Validacion = false;
                }
                if (Txt_Colonia_Ubicacion.Text.Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir un valor para el campo Colonia.";
                    Validacion = false;
                }
                if (Txt_Ciudad_Ubicacion.Text.Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir un valor para el campo Ciudad.";
                    Validacion = false;
                }
                if (Txt_Estado_Ubicacion.Text.Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir un valor para el campo Estado.";
                    Validacion = false;
                }
                if (Txt_Observacion_Juridica.Text.Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir un valor para el campo Observación Juridica.";
                    Validacion = false;
                }
                if (Txt_Observacion_Tecnica.Text.Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir un valor para el campo Observación Técnica.";
                    Validacion = false;
                }
                if (Txt_Avance_Obra.Text.Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir un valor para el campo Avance Obra.";
                    Validacion = false;
                }
                if (Txt_Perito.Text.Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir un valor para el campo Perito.";
                    Validacion = false;
                }
                if (Txt_Ubicacion_Construccion.Text.Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir un valor para el campo Ubicación Construcción.";
                    Validacion = false;
                }
                if (Txt_Inicio_Permiso.Text.Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir un valor para el campo Inicio Permiso.";
                    Validacion = false;
                }
                if (Txt_Fin_Permiso.Text.Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir un valor para el campo Fin Permiso.";
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
                    Configuracion_Formulario(true);
                    Limpiar_Catalogo();
                    Btn_Nuevo.AlternateText = "Dar de Alta";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.Visible = false;
                }
                else
                {
                    if (Validar_Componentes())
                    {
                        Cls_Ope_Ort_Ficha_Revision_Depto_Negocio Negocio = new Cls_Ope_Ort_Ficha_Revision_Depto_Negocio();
                        Negocio.P_Nombre_Propietario = Txt_Nombre_Propietario.Text.Trim();
                        Negocio.P_Codigo_Postal = Txt_Codigo_Postal.Text.Trim();
                        Negocio.P_Calle_Ubicacion = Txt_Calle_Ubicacion.Text.Trim();
                        Negocio.P_Colonia_Ubicacion = Txt_Colonia_Ubicacion.Text.Trim();
                        Negocio.P_Ciudad_Ubicacion = Txt_Ciudad_Ubicacion.Text.Trim();
                        Negocio.P_Estado_Ubicacion = Txt_Estado_Ubicacion.Text.Trim();

                        Negocio.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                        Negocio.P_Tipo_Tramite = Txt_Tipo_Tramite.Text.Trim();
                        Negocio.P_Tramite = Txt_Tramite.Text.Trim();

                        Negocio.P_Documentos_Propiedad = Cmb_Documentos_Propiedad.SelectedValue;
                        Negocio.P_Documentos_Dictamen = Cmb_Documentos_Dictamen.SelectedValue;
                        Negocio.P_Observacion_Juridica = Txt_Observacion_Juridica.Text.Trim();
                        Negocio.P_Observacion_Tecnica = Txt_Observacion_Tecnica.Text.Trim();
                        Negocio.P_Avance_Obra = Txt_Avance_Obra.Text.Trim();
                        Negocio.P_Cumplimiento_Norma = Cmb_Cumplimiento_Normas.SelectedValue;
                        Negocio.P_Perito = Txt_Perito.Text.Trim();
                        Negocio.P_Ubicacion_Construccion = Txt_Ubicacion_Construccion.Text.Trim();
                        Negocio.P_Inicio_Permiso = DateTime.Parse(Txt_Inicio_Permiso.Text);
                        Negocio.P_Fin_Permiso = DateTime.Parse(Txt_Fin_Permiso.Text);

                        Negocio.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
                        Negocio.Alta_Ficha_Revision_Depto();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Grid_Listado(Grid_Listado.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo", "alert('Alta Exitosa');", true);
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
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
                        Configuracion_Formulario(true);
                        Btn_Modificar.AlternateText = "Actualizar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                        Btn_Salir.AlternateText = "Cancelar";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        Btn_Nuevo.Visible = false;
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
                    if (Validar_Componentes())
                    {
                        Cls_Ope_Ort_Ficha_Revision_Depto_Negocio Negocio = new Cls_Ope_Ort_Ficha_Revision_Depto_Negocio();
                        Negocio.P_Ficha_Revision_ID = Hdf_Ficha_Revision_ID.Value;
                        Negocio.P_Nombre_Propietario = Txt_Nombre_Propietario.Text.Trim();
                        Negocio.P_Codigo_Postal = Txt_Codigo_Postal.Text.Trim();
                        Negocio.P_Calle_Ubicacion = Txt_Calle_Ubicacion.Text.Trim();
                        Negocio.P_Colonia_Ubicacion = Txt_Colonia_Ubicacion.Text.Trim();
                        Negocio.P_Ciudad_Ubicacion = Txt_Ciudad_Ubicacion.Text.Trim();
                        Negocio.P_Estado_Ubicacion = Txt_Estado_Ubicacion.Text.Trim();

                        Negocio.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                        Negocio.P_Tipo_Tramite = Txt_Tipo_Tramite.Text.Trim();
                        Negocio.P_Tramite = Txt_Tramite.Text.Trim();

                        Negocio.P_Documentos_Propiedad = Cmb_Documentos_Propiedad.SelectedValue;
                        Negocio.P_Documentos_Dictamen = Cmb_Documentos_Dictamen.SelectedValue;
                        Negocio.P_Observacion_Juridica = Txt_Observacion_Juridica.Text.Trim();
                        Negocio.P_Observacion_Tecnica = Txt_Observacion_Tecnica.Text.Trim();
                        Negocio.P_Avance_Obra = Txt_Avance_Obra.Text.Trim();
                        Negocio.P_Cumplimiento_Norma = Cmb_Cumplimiento_Normas.SelectedValue;
                        Negocio.P_Perito = Txt_Perito.Text.Trim();
                        Negocio.P_Ubicacion_Construccion = Txt_Ubicacion_Construccion.Text.Trim();
                        Negocio.P_Inicio_Permiso = DateTime.Parse(Txt_Inicio_Permiso.Text, new CultureInfo("es-MX"));
                        Negocio.P_Fin_Permiso = DateTime.Parse(Txt_Fin_Permiso.Text, new CultureInfo("es-MX"));
                        Negocio.P_Usuario_Modifico = Cls_Sessiones.Nombre_Empleado;
                        Negocio.Modificar_Ficha_Revision_Depto();
                        Configuracion_Formulario(false);
                        Limpiar_Catalogo();
                        Llenar_Grid_Listado(Grid_Listado.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo", "alert('Actualización Exitosa');", true);
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
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
            }
            else
            {
                Configuracion_Formulario(false);
                Limpiar_Catalogo();
                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
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
            if (Session["SOLICITUD_ID"].ToString() != "")
                Cargar_Tramite(Session["SOLICITUD_ID"].ToString());
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Perito_Click
        ///DESCRIPCIÓN         : Carga los datos del elemento seleccionado.
        ///PARÁMETROS          :     
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///*******************************************************************************
        protected void Btn_Buscar_Perito_Click(object sender, ImageClickEventArgs e)
        {
            if (Session["PERITO_ID"].ToString() != "")
            {
                Cls_Cat_Ort_Inspectores_Negocio Neg_Peritos = new Cls_Cat_Ort_Inspectores_Negocio();
                Neg_Peritos.P_Inspector_ID = Session["PERITO_ID"].ToString();
                DataTable Dt_Perito = Neg_Peritos.Consultar_Inspectores();

                if (Dt_Perito.Rows.Count >= 0)
                {
                    Txt_Perito.Text = Dt_Perito.Rows[0][Cat_Ort_Inspectores.Campo_Nombre].ToString();
                }
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Propietario_Click
        ///DESCRIPCIÓN         : Carga los datos del elemento seleccionado.
        ///PARÁMETROS          :     
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///*******************************************************************************
        protected void Btn_Buscar_Propietario_Click(object sender, ImageClickEventArgs e)
        {
            Cls_Cat_Ven_Registro_Usuarios_Negocio Negocio = new Cls_Cat_Ven_Registro_Usuarios_Negocio();
            Negocio.P_Ciudadano_ID = Session["CIUDADANO_ID"].ToString();

            DataTable Dt_Consulta = Negocio.Consultar_Usuario_Soliucitante();

            if (Dt_Consulta != null)
            {
                if (Dt_Consulta is DataTable)
                {
                    if (Dt_Consulta.Rows.Count > 0)
                    {
                        Txt_Nombre_Propietario.Text = Dt_Consulta.Rows[0][Cat_Pre_Contribuyentes.Campo_Nombre_Completo].ToString();
                        Txt_Codigo_Postal.Text = Dt_Consulta.Rows[0][Cat_Pre_Contribuyentes.Campo_Codigo_Postal].ToString();
                        Txt_Calle_Ubicacion.Text = Dt_Consulta.Rows[0][Cat_Pre_Contribuyentes.Campo_Calle_Ubicacion].ToString();
                        Txt_Colonia_Ubicacion.Text = Dt_Consulta.Rows[0][Cat_Pre_Contribuyentes.Campo_Colonia_Ubicacion].ToString();
                        Txt_Ciudad_Ubicacion.Text = Dt_Consulta.Rows[0][Cat_Pre_Contribuyentes.Campo_Ciudad_Ubicacion].ToString();
                        Txt_Estado_Ubicacion.Text = Dt_Consulta.Rows[0][Cat_Pre_Contribuyentes.Campo_Estado_Ubicacion].ToString();
                    }
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
                    Hdf_Ficha_Revision_ID.Value = ID;
                    Mostrar_Datos(ID);
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

    #endregion

}
