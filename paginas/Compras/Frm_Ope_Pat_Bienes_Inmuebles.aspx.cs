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
using Presidencia.Control_Patrimonial_Catalogo_Usos_Inmuebles.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Destinos_Inmuebles.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Bienes_Inmuebles.Negocio;
using Presidencia.Catalogo_Tipos_Predio.Negocio;
using Presidencia.Catalogo_Calles.Negocio;
using Presidencia.Catalogo_Colonias.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Clasificaciones_Zonas_Inmuebles.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Origenes_Inmuebles.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Orientaciones_Inmuebles.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Areas_Donacion.Negocio;
using Presidencia.Catalogo_Notarios.Negocio;
using Presidencia.Colonias.Negocios; 
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Presidencia.Control_Patrimonial_Catalogo_Clases_Activos.Negocio;
using Presidencia.Control_Patrimonial_Reporte_Listado_Bienes.Negocio;
using System.Drawing;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;

public partial class paginas_Control_Patrimonial_Frm_Ope_Pat_Bienes_Inmuebles : System.Web.UI.Page {

    #region "Page Load"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Evento que se ejecuta cuando se carga inicialmente la pagina
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Page_Load(object sender, EventArgs e) {
            Lbl_Ecabezado_Mensaje.Text = "";
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = false;
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Trim().Length == 0) {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            }
            if (!IsPostBack) {
                Llenar_Combo_Uso();
                Llenar_Combo_Destino();
                Llenar_Combo_Tipo_Predio();
                Llenar_Combo_Sectores();
                Llenar_Combo_Clasificaciones_Zonas();
                Llenar_Combo_Origenes();
                Llenar_Combo_Orientaciones();
                Llenar_Combo_Areas_Donacion();
                Llenar_Combo_Clase_Activo();
                Habiliar_Generales_Formulario(false);
                Limpiar_Generales_Formulario();
                if (Session["Operacion_Inicial"] != null) {
                    String Operacion_Inicial = Session["Operacion_Inicial"].ToString();
                    Session.Remove("Operacion_Inicial");
                    if (Operacion_Inicial.Trim().Equals("NUEVO")) {
                        Btn_Nuevo_Click(Btn_Nuevo, null);
                    } else if (Operacion_Inicial.Trim().Equals("VER_BIEN_INMUEBLE")) {
                        if (Session["Bien_Inmueble_ID"] != null) {
                            String Bien_Inmueble_ID = Session["Bien_Inmueble_ID"].ToString();
                            Session.Remove("Bien_Mueble_ID");
                            Hdf_Bien_Inmueble_ID.Value = Bien_Inmueble_ID;
                            Mostrar_Detalles_Bien_Inmueble();
                        } else {
                            Response.Redirect("Frm_Ope_Pat_Entrada_Bienes_Inmuebles.aspx");
                        }
                    } else {
                        Response.Redirect("Frm_Ope_Pat_Entrada_Bienes_Inmuebles.aspx");
                    }
                } else {
                    Response.Redirect("Frm_Ope_Pat_Entrada_Bienes_Inmuebles.aspx");
                }
            }
        }

    #endregion

    #region "Metodos"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Afectaciones
        ///DESCRIPCIÓN: Limpia los campos de la Seccion de Afectaciones
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Contabilidad() {
            Txt_Cont_Hoja.Text = "";
            Txt_Cont_Tomo.Text = "";
            Txt_Cont_Numero_Acta.Text = "";
            Txt_Cont_Cartilla_Parcelaria.Text = "";
            Txt_Cont_Superficie.Text = "";
            Txt_Cont_Unidad_Superficie.Text = "";
            Cmb_Clase_Activo.SelectedIndex = 0;
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Habilitar_Contabilidad
        ///DESCRIPCIÓN: Habilita los campos de la Seccion de Contabilidad
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Habilitar_Contabilidad(Boolean Habilitar) {
            Txt_Cont_Hoja.Enabled = Habilitar;
            Txt_Cont_Tomo.Enabled = Habilitar;
            Txt_Cont_Numero_Acta.Enabled = Habilitar;
            Txt_Cont_Cartilla_Parcelaria.Enabled = Habilitar;
            Txt_Cont_Superficie.Enabled = Habilitar;
            Txt_Cont_Unidad_Superficie.Enabled = Habilitar;
            Cmb_Clase_Activo.Enabled = Habilitar;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Afectaciones
        ///DESCRIPCIÓN: Limpia los campos de la Seccion de Afectaciones
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Afectaciones() {
            Txt_No_Contrato.Text = "";
            Txt_Fecha_Afectacion.Text = "";
            Txt_Nuevo_Propietario.Text = "";
            Txt_Session_Ayuntamiento.Text = "";
            Txt_Tramo.Text = "";
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Habilitar_Afectaciones
        ///DESCRIPCIÓN: Habilita los campos de la Seccion de Afectaciones
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Habilitar_Afectaciones(Boolean Habilitar) {
            Txt_No_Contrato.Enabled = Habilitar;
            Txt_Fecha_Afectacion.Enabled = false;
            Btn_Fecha_Afectacion.Enabled = Habilitar;
            Txt_Nuevo_Propietario.Enabled = Habilitar;
            Txt_Session_Ayuntamiento.Enabled = Habilitar;
            Txt_Tramo.Enabled = Habilitar;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Anexos
        ///DESCRIPCIÓN: Limpia los campos de la Seccion de Anexos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Anexos() {
            Cmb_Tipo_Archivo.SelectedIndex = 0;
            Txt_Descripcion_Archivo.Text = "";
            Remover_Sesiones_Control_AsyncFileUpload(AFU_Ruta_Archivo.ClientID);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Habilitar_Anexos
        ///DESCRIPCIÓN: Habilita los campos de la Seccion de Anexos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Habilitar_Anexos(Boolean Habilitar) {
            Cmb_Tipo_Archivo.Enabled = Habilitar;
            AFU_Ruta_Archivo.Enabled = Habilitar;
            Txt_Descripcion_Archivo.Enabled = Habilitar;
            Grid_Listado_Anexos.Columns[7].Visible = Habilitar;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Medidas_Colindancias
        ///DESCRIPCIÓN: Limpia los campos de la Seccion de Medidas y Colindancias
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Medidas_Colindancias() {
            Cmb_Orientacion.SelectedIndex = 0;
            Txt_Medida.Text = "";
            Txt_Colindancia.Text = "";
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Habilitar_Medidas_Colindancias
        ///DESCRIPCIÓN: Habilita los campos de la Seccion de Medidas y Colindancias
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Habilitar_Medidas_Colindancias(Boolean Habilitar) {
            Cmb_Orientacion.Enabled = Habilitar;
            Txt_Medida.Enabled = Habilitar;
            Txt_Colindancia.Enabled = Habilitar;
            Btn_Agregar_Medida_Colindancia.Visible = Habilitar;
            Grid_Listado_Medidas_Colindancias.Columns[3].Visible = Habilitar;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Sustento_Juridico
        ///DESCRIPCIÓN: Limpia los campos de la Seccion de Juridico
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Sustento_Juridico() {
            //Alta
            Hdf_No_Registro_Juridico_Alta.Value = "";
            Txt_Escritura.Text = "";
            Txt_Fecha_Escritura.Text = "";
            Txt_No_Notario.Text = "";
            Txt_Constacia_Registral.Text = "";
            Txt_Nombre_Notario.Text = "";
            Txt_Folio_Real.Text = "";
            Cmb_Libertad_Gravament.SelectedIndex = 0;
            Txt_Antecedente_Registral.Text = "";
            Txt_Proveedor.Text = "";
            Txt_No_Contrato_Juridico.Text = "";

            //Baja
            Txt_Fecha_Baja.Text = "";
            Hdf_No_Registro_Juridico_Baja.Value = "";
            Txt_Baja_No_Escritura.Text = "";
            Txt_Baja_Fecha_Escritura.Text = "";
            Txt_Baja_No_Notario.Text = "";
            Txt_Baja_Constancia_Registral.Text = "";
            Txt_Baja_Nombre_Notario.Text = "";
            Txt_Baja_Folio_Real.Text = "";
            Txt_Baja_Nuevo_Propietario.Text = "";
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Habilitar_Sustento_Juridico
        ///DESCRIPCIÓN: Habilita los campos de la Seccion de Juridico
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Habilitar_Sustento_Juridico(Boolean Habilitar) {
            //Alta
            Txt_Escritura.Enabled = Habilitar;
            Txt_Fecha_Escritura.Enabled = false;
            Txt_No_Notario.Enabled = Habilitar;
            Txt_Constacia_Registral.Enabled = Habilitar;
            Txt_Nombre_Notario.Enabled = Habilitar;
            Txt_Folio_Real.Enabled = Habilitar;
            Cmb_Libertad_Gravament.Enabled = Habilitar;
            Txt_Antecedente_Registral.Enabled = Habilitar;
            Txt_Proveedor.Enabled = Habilitar;
            Btn_Fecha_Escritura.Enabled = Habilitar;
            Txt_No_Contrato_Juridico.Enabled = Habilitar;

            //Baja
            Txt_Fecha_Baja.Enabled = false;
            Btn_Fecha_Baja.Enabled = Habilitar;
            Txt_Baja_No_Escritura.Enabled = Habilitar;
            Txt_Baja_Fecha_Escritura.Enabled = false;
            Btn_Baja_Fecha_Escritura.Enabled = Habilitar;
            Txt_Baja_No_Notario.Enabled = Habilitar;
            Txt_Baja_Constancia_Registral.Enabled = Habilitar;
            Txt_Baja_Nombre_Notario.Enabled = Habilitar;
            Txt_Baja_Folio_Real.Enabled = Habilitar;
            Txt_Baja_Nuevo_Propietario.Enabled = Habilitar;
            Txt_Baja_No_Contrato.Enabled = Habilitar;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Generales_Formulario
        ///DESCRIPCIÓN: Limpia los campos de todo el formulario
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Generales_Formulario() {
            Hdf_Bien_Inmueble_ID.Value = "";
            Txt_Bien_Inmueble_ID.Text = "";
            Txt_Fecha_Alta_Cta_Pub.Text = "";
            Txt_Registro_Propiedad.Text = "";
            Hdf_Calle_ID.Value = "";
            Hdf_Colonia_ID.Value = "";
            Hdf_Cuenta_Predial_ID.Value = "";
            Txt_Calle.Text = "";
            Txt_Numero_Exterior.Text = "";
            Txt_Numero_Interior.Text = "";
            Txt_Colonia.Text = "";
            Cmb_Uso.SelectedIndex = 0;
            Cmb_Destino.SelectedIndex = 0;
            Cmb_Origen.SelectedIndex = 0;
            Cmb_Estatus.SelectedIndex = 0;
            Txt_Superficie.Text = "";
            Txt_Construccion_Resgistrada.Text = "";
            Txt_Fecha_Registro.Text = "";
            Txt_Manzana.Text = "";
            Txt_Lote.Text = "";
            Txt_Porcentaje_Ocupacion.Text = "";
            Txt_Numero_Cuenta_Predial.Text = "";
            Txt_Propietario.Text = "";
            Txt_Valor_Catastral.Text = "";
            Txt_Valor_Comercial.Text = "";
            Txt_Efectos_Fiscales.Text = "";
            Cmb_Sector.SelectedIndex = 0;
            Cmb_Clasificacion_Zona.SelectedIndex = 0;
            Cmb_Tipo_Predio.SelectedIndex = 0;
            Txt_Vias_Aceso.Text = "";
            Cmb_Estado.SelectedIndex = 0;
            Cmb_Area_Donacion.SelectedIndex = 0;
            Cmb_Tipo_Bien.SelectedIndex = 0;
            Txt_Densidad_Construccion.Text = "";
            Txt_Observaciones.Text = "";
            Txt_Expropiacion.Text = "";
            Limpiar_Medidas_Colindancias();
            Limpiar_Sustento_Juridico();
            Limpiar_Anexos();
            Grid_Observaciones.DataSource = new DataTable();
            Grid_Observaciones.DataBind();
            Llenar_Listado_Medidas_Colindancias(new DataTable());
            Llenar_Listado_Anexos(new DataTable());
            Limpiar_Afectaciones();
            Grid_Afectaciones.DataSource = new DataTable();
            Grid_Afectaciones.DataBind();
            Limpiar_Contabilidad();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Habiliar_Generales_Formulario
        ///DESCRIPCIÓN: Habilita los campos de todo el formulario
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Habiliar_Generales_Formulario(Boolean Habilitar) {
            if (!Habilitar) {
                Btn_Nuevo.Visible = false;
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Nuevo.AlternateText = "Nuevo";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                Btn_Modificar.Visible = true;
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Modificar.AlternateText = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Salir.ToolTip = "Salir";
                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Ver_Ficha_Tecnica_PDF.Visible = true;
                Btn_Ver_Ficha_Tecnica_Excel.Visible = true;
            } else { 
                Btn_Nuevo.ToolTip = "Dar de Alta";
                Btn_Nuevo.AlternateText = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Modificar.ToolTip = "Actualizar Cambios";
                Btn_Modificar.AlternateText = "Actualizar Cambios";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Ver_Ficha_Tecnica_PDF.Visible = false;
                Btn_Ver_Ficha_Tecnica_Excel.Visible = false;
            }
            Btn_Buscar_Calle.Visible = Habilitar;
            Btn_Buscar_Colonia.Visible = Habilitar;
            Btn_Buscar_Numero_Cuenta_Predial.Visible = Habilitar;
            Btn_Fecha_Alta_Cta_Pub.Enabled = Habilitar;
            Txt_Calle.Enabled = false;
            Txt_Registro_Propiedad.Enabled = Habilitar;
            Txt_Numero_Exterior.Enabled = Habilitar;
            Txt_Numero_Interior.Enabled = Habilitar;
            Txt_Colonia.Enabled = false;
            Cmb_Uso.Enabled = Habilitar;
            Cmb_Destino.Enabled = Habilitar;
            Cmb_Origen.Enabled = Habilitar;
            Cmb_Estatus.Enabled = Habilitar;
            Txt_Superficie.Enabled = Habilitar;
            Txt_Construccion_Resgistrada.Enabled = Habilitar;
            Btn_Fecha_Registro.Enabled = Habilitar;
            Txt_Manzana.Enabled = Habilitar;
            Txt_Lote.Enabled = Habilitar;
            Txt_Porcentaje_Ocupacion.Enabled = Habilitar;
            Txt_Numero_Cuenta_Predial.Enabled = false;
            Txt_Propietario.Enabled = false;
            Txt_Efectos_Fiscales.Enabled = Habilitar;
            Cmb_Sector.Enabled = Habilitar;
            Cmb_Clasificacion_Zona.Enabled = Habilitar;
            Cmb_Tipo_Predio.Enabled = Habilitar;
            Txt_Valor_Catastral.Enabled = false;
            Txt_Valor_Comercial.Enabled = Habilitar;
            Txt_Vias_Aceso.Enabled = Habilitar;
            if (Cmb_Estado.SelectedItem.Value == "BAJA") { Cmb_Estado.Enabled = false; } else { Cmb_Estado.Enabled = Habilitar; }
            Txt_Densidad_Construccion.Enabled = Habilitar;
            Txt_Observaciones.Enabled = Habilitar;
            Txt_Expropiacion.Enabled = Habilitar;
            Cmb_Area_Donacion.Enabled = Habilitar;
            Cmb_Tipo_Bien.Enabled = Habilitar;
            Habilitar_Medidas_Colindancias(Habilitar);
            Habilitar_Sustento_Juridico(Habilitar);
            Habilitar_Anexos(Habilitar);
            Habilitar_Afectaciones(Habilitar);
            Habilitar_Contabilidad(Habilitar);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Uso
        ///DESCRIPCIÓN: Llena el Combo de Usos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Combo_Uso() {
            Cls_Cat_Pat_Com_Usos_Inmuebles_Negocio Uso_Suelo = new Cls_Cat_Pat_Com_Usos_Inmuebles_Negocio();
            Uso_Suelo.P_Estatus = "VIGENTE";
            Cmb_Uso.DataSource = Uso_Suelo.Consultar_Usos();
            Cmb_Uso.DataTextField = "DESCRIPCION";
            Cmb_Uso.DataValueField = "USO_ID";
            Cmb_Uso.DataBind();
            Cmb_Uso.Items.Insert(0, new ListItem("<SELECCIONE>", ""));
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Destino
        ///DESCRIPCIÓN: Llena el Combo de Destinos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Llenar_Combo_Destino() {
            Cls_Cat_Pat_Com_Destinos_Inmuebles_Negocio Destino_Suelo = new Cls_Cat_Pat_Com_Destinos_Inmuebles_Negocio();
            Destino_Suelo.P_Estatus = "VIGENTE";
            Cmb_Destino.DataSource = Destino_Suelo.Consultar_Destinos();
            Cmb_Destino.DataTextField = "DESCRIPCION";
            Cmb_Destino.DataValueField = "DESTINO_ID";
            Cmb_Destino.DataBind();
            Cmb_Destino.Items.Insert(0, new ListItem("<SELECCIONE>", ""));
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Clase_Activo
        ///DESCRIPCIÓN: Llena el Combo de Clases de Activos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Llenar_Combo_Clase_Activo() {
            Cls_Cat_Pat_Com_Clases_Activo_Negocio CA_Negocio = new Cls_Cat_Pat_Com_Clases_Activo_Negocio();
            CA_Negocio.P_Estatus = "VIGENTE";
            CA_Negocio.P_Tipo_DataTable = "CLASES_ACTIVOS";
            Cmb_Clase_Activo.DataSource = CA_Negocio.Consultar_DataTable();
            Cmb_Clase_Activo.DataValueField = "CLASE_ACTIVO_ID";
            Cmb_Clase_Activo.DataTextField = "CLAVE_DESCRIPCION";
            Cmb_Clase_Activo.DataBind();
            Cmb_Clase_Activo.Items.Insert(0, new ListItem("<- SELECCIONE ->", ""));
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Origenes
        ///DESCRIPCIÓN: Llena el Combo de Origenes
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************   
        private void Llenar_Combo_Origenes() {
            Cls_Cat_Pat_Com_Origenes_Inmuebles_Negocio Destino_Suelo = new Cls_Cat_Pat_Com_Origenes_Inmuebles_Negocio();
            Destino_Suelo.P_Estatus = "VIGENTE";
            Cmb_Origen.DataSource = Destino_Suelo.Consultar_Origenes();
            Cmb_Origen.DataTextField = "NOMBRE";
            Cmb_Origen.DataValueField = "ORIGEN_ID";
            Cmb_Origen.DataBind();
            Cmb_Origen.Items.Insert(0, new ListItem("<SELECCIONE>", ""));
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Tipo_Predio
        ///DESCRIPCIÓN: Llena el Combo de Tipos de Predio
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************   
        private void Llenar_Combo_Tipo_Predio() {
            Cls_Cat_Pre_Tipos_Predio_Negocio Tipo_Predio = new Cls_Cat_Pre_Tipos_Predio_Negocio();
            Cmb_Tipo_Predio.DataSource = Tipo_Predio.Consultar_Tipo_Predio();
            Cmb_Tipo_Predio.DataTextField = "DESCRIPCION";
            Cmb_Tipo_Predio.DataValueField = "TIPO_PREDIO_ID";
            Cmb_Tipo_Predio.DataBind();
            Cmb_Tipo_Predio.Items.Insert(0, new ListItem("<SELECCIONE>", ""));
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Sectores
        ///DESCRIPCIÓN: Llena el Combo de Sectores
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Llenar_Combo_Sectores() {
            Cls_Cat_Pat_Com_Orientaciones_Inmuebles_Negocio Sectores = new Cls_Cat_Pat_Com_Orientaciones_Inmuebles_Negocio();
            Sectores.P_Estatus = "VIGENTE";
            Cmb_Sector.DataSource = Sectores.Consultar_Orientaciones();
            Cmb_Sector.DataTextField = "DESCRIPCION";
            Cmb_Sector.DataValueField = "ORIENTACION_ID";
            Cmb_Sector.DataBind();
            Cmb_Sector.Items.Insert(0, new ListItem("<SELECCIONE>", ""));
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Clasificaciones_Zonas
        ///DESCRIPCIÓN: Llena el Combo de Clasificaciones de Zonas
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Llenar_Combo_Clasificaciones_Zonas() {
            Cls_Cat_Pat_Com_Clasificaciones_Zonas_Inmuebles_Negocio Negocio = new Cls_Cat_Pat_Com_Clasificaciones_Zonas_Inmuebles_Negocio();
            Negocio.P_Estatus = "VIGENTE";
            Cmb_Clasificacion_Zona.DataSource = Negocio.Consultar_Clasificaciones();
            Cmb_Clasificacion_Zona.DataTextField = "DESCRIPCION";
            Cmb_Clasificacion_Zona.DataValueField = "CLASIFICACION_ID";
            Cmb_Clasificacion_Zona.DataBind();
            Cmb_Clasificacion_Zona.Items.Insert(0, new ListItem("<SELECCIONE>", ""));
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Orientaciones
        ///DESCRIPCIÓN: Llena el Combo de Orientaciones
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Llenar_Combo_Orientaciones() {
            Cls_Cat_Pat_Com_Orientaciones_Inmuebles_Negocio Orientaciones = new Cls_Cat_Pat_Com_Orientaciones_Inmuebles_Negocio();
            Orientaciones.P_Estatus = "VIGENTE";
            Cmb_Orientacion.DataSource = Orientaciones.Consultar_Orientaciones();
            Cmb_Orientacion.DataTextField = "DESCRIPCION";
            Cmb_Orientacion.DataValueField = "ORIENTACION_ID";
            Cmb_Orientacion.DataBind();
            Cmb_Orientacion.Items.Insert(0, new ListItem("<SELECCIONE>", ""));
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Areas_Donacion
        ///DESCRIPCIÓN: Llena el Combo de Areas de Donación
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************   
        private void Llenar_Combo_Areas_Donacion() {
            Cls_Cat_Pat_Com_Areas_Donacion_Negocio Area_Negocio = new Cls_Cat_Pat_Com_Areas_Donacion_Negocio();
            Area_Negocio.P_Estatus = "VIGENTE";
            Cmb_Area_Donacion.DataSource = Area_Negocio.Consultar_Areas();
            Cmb_Area_Donacion.DataTextField = "DESCRIPCION";
            Cmb_Area_Donacion.DataValueField = "AREA_ID";
            Cmb_Area_Donacion.DataBind();
            Cmb_Area_Donacion.Items.Insert(0, new ListItem("<SELECCIONE>", ""));
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta
        ///DESCRIPCIÓN: Registra en la Base de Datos el Bien Inmueble
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private Cls_Ope_Pat_Bienes_Inmuebles_Negocio Alta() {
            Cls_Ope_Pat_Bienes_Inmuebles_Negocio BI_Negocio = new Cls_Ope_Pat_Bienes_Inmuebles_Negocio();
            BI_Negocio.P_Registro_Propiedad = Txt_Registro_Propiedad.Text.Trim();
            if (Txt_Fecha_Alta_Cta_Pub.Text.Trim().Length > 0) { BI_Negocio.P_Fecha_Alta_Cuenta_Publica = Convert.ToDateTime(Txt_Fecha_Alta_Cta_Pub.Text); }
            BI_Negocio.P_Calle = Hdf_Calle_ID.Value;
            BI_Negocio.P_Colonia = Hdf_Colonia_ID.Value;
            if (Cmb_Uso.SelectedIndex > 0) { BI_Negocio.P_Uso_ID = Cmb_Uso.SelectedItem.Value; }
            if (Cmb_Origen.SelectedIndex > 0) { BI_Negocio.P_Origen_ID = Cmb_Origen.SelectedItem.Value; }
            if (Cmb_Estatus.SelectedIndex > 0) { BI_Negocio.P_Estatus = Cmb_Estatus.SelectedItem.Value; }
            if (Txt_Superficie.Text.Trim().Length > 0) { BI_Negocio.P_Superficie = Convert.ToDouble(Txt_Superficie.Text); }
            if (Txt_Construccion_Resgistrada.Text.Trim().Length > 0) { BI_Negocio.P_Construccion_Construida = Convert.ToDouble(Txt_Construccion_Resgistrada.Text); }
            BI_Negocio.P_Manzana = Txt_Manzana.Text.Trim();
            BI_Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value.Trim();
            BI_Negocio.P_Lote = Txt_Lote.Text.Trim();
            if (Txt_Porcentaje_Ocupacion.Text.Trim().Length > 0) { BI_Negocio.P_Ocupacion = Convert.ToDouble(Txt_Porcentaje_Ocupacion.Text); }
            BI_Negocio.P_Efectos_Fiscales = Txt_Efectos_Fiscales.Text.Trim();
            if (Cmb_Sector.SelectedIndex > 0) { BI_Negocio.P_Sector_ID = Cmb_Sector.SelectedItem.Value; }
            if (Cmb_Clasificacion_Zona.SelectedIndex > 0) { BI_Negocio.P_Clasificacion_Zona_ID = Cmb_Clasificacion_Zona.SelectedItem.Value; }
            if (Cmb_Tipo_Predio.SelectedIndex > 0) { BI_Negocio.P_Tipo_Predio_ID = Cmb_Tipo_Predio.SelectedItem.Value; }
            if (Txt_Valor_Comercial.Text.Trim().Length > 0) { BI_Negocio.P_Valor_Comercial = Convert.ToDouble(Txt_Valor_Comercial.Text); }
            BI_Negocio.P_Vias_Acceso = Txt_Vias_Aceso.Text.Trim();
            BI_Negocio.P_Estado = Cmb_Estado.SelectedItem.Value;
            if (Txt_Densidad_Construccion.Text.Trim().Length > 0) { BI_Negocio.P_Densidad_Construccion = Convert.ToDouble(Txt_Densidad_Construccion.Text); }
            BI_Negocio.P_No_Exterior = Txt_Numero_Exterior.Text.Trim();
            BI_Negocio.P_No_Interior = Txt_Numero_Interior.Text.Trim();
            if (Cmb_Destino.SelectedIndex > 0) { BI_Negocio.P_Destino_ID = Cmb_Destino.SelectedItem.Value; }
            if (Cmb_Area_Donacion.SelectedIndex > 0) { BI_Negocio.P_Area_ID = Cmb_Area_Donacion.SelectedItem.Value; }
            if (Cmb_Tipo_Bien.SelectedIndex > 0) { BI_Negocio.P_Tipo_Bien = Cmb_Tipo_Bien.SelectedItem.Value; }
            BI_Negocio.P_Observaciones = Txt_Observaciones.Text.Trim();
            BI_Negocio.P_Fecha_Registro = Convert.ToDateTime(Txt_Fecha_Registro.Text);
            BI_Negocio.P_Dt_Medidas_Colindancias = (Session["Dt_Medidas_Colindancias"] != null) ? ((DataTable)Session["Dt_Medidas_Colindancias"]) : new DataTable();
            
            BI_Negocio.P_Hoja = Txt_Cont_Hoja.Text.Trim();
            BI_Negocio.P_Tomo = Txt_Cont_Tomo.Text.Trim();
            BI_Negocio.P_Numero_Acta = Txt_Cont_Numero_Acta.Text.Trim();
            BI_Negocio.P_Cartilla_Parcelaria = Txt_Cont_Cartilla_Parcelaria.Text.Trim();
            if (Txt_Cont_Superficie.Text.Trim().Length > 0) { BI_Negocio.P_Superficie_Contable = Convert.ToDouble(Txt_Cont_Superficie.Text); }
            BI_Negocio.P_Unidad_Superficie = Txt_Cont_Unidad_Superficie.Text.Trim();
            BI_Negocio.P_Clase_Activo_ID = Cmb_Clase_Activo.SelectedItem.Value;

            //Juridico
            if (Txt_Escritura.Text.Trim().Length > 0) {
                BI_Negocio.P_No_Escritura = Txt_Escritura.Text;
                BI_Negocio.P_Fecha_Escritura = Convert.ToDateTime(Txt_Fecha_Escritura.Text);
                BI_Negocio.P_No_Notario = Txt_No_Notario.Text.Trim();
                BI_Negocio.P_Notario_Nombre = Txt_Nombre_Notario.Text.Trim();
                BI_Negocio.P_Constancia_Registral = Txt_Constacia_Registral.Text;
                BI_Negocio.P_Folio_Real = Txt_Folio_Real.Text;
                BI_Negocio.P_Libre_Gravament = Cmb_Libertad_Gravament.SelectedItem.Value;
                BI_Negocio.P_Antecedente = Txt_Antecedente_Registral.Text;
                BI_Negocio.P_Proveedor = Txt_Proveedor.Text;
                BI_Negocio.P_No_Contrato_Juridico = Txt_No_Contrato_Juridico.Text;
            }

            String Nombre_Archivo = (DateTime.Now).ToString().Replace(".", "").Replace(" ", "").Replace("/", "").Replace("-", "").Replace(":", ""); 
            if (AFU_Ruta_Archivo.HasFile) {
                BI_Negocio.P_Tipo_Anexo = Cmb_Tipo_Archivo.SelectedItem.Value.Trim();
                BI_Negocio.P_Archivo = Nombre_Archivo + "_"+ AFU_Ruta_Archivo.FileName;
                BI_Negocio.P_Descripcion_Anexo = Txt_Descripcion_Archivo.Text.Trim();
            }

            //Expropiaciones
            BI_Negocio.P_Expropiacion = Txt_Expropiacion.Text.Trim();

            //Afectaciones
            if (Txt_No_Contrato.Text.Length > 0) {
                BI_Negocio.P_No_Contrato = Txt_No_Contrato.Text.Trim();
                BI_Negocio.P_Fecha_Afectacion = Convert.ToDateTime(Txt_Fecha_Afectacion.Text.Trim());
                BI_Negocio.P_Nuevo_Propietario = Txt_Nuevo_Propietario.Text.Trim();
                BI_Negocio.P_Session_Ayuntamiento = Txt_Session_Ayuntamiento.Text.Trim();
                BI_Negocio.P_Tramo = Txt_Tramo.Text.Trim();
                BI_Negocio.P_Agregar_Afectacion = true;
            }

            BI_Negocio.P_Usuario_ID = Cls_Sessiones.Empleado_ID;
            BI_Negocio.P_Usuario_Nombre = Cls_Sessiones.Nombre_Empleado;
            BI_Negocio = BI_Negocio.Alta_Bien_Inmueble();
            return BI_Negocio;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar
        ///DESCRIPCIÓN: Actualiza en la Base de Datos el Bien Inmueble
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private Cls_Ope_Pat_Bienes_Inmuebles_Negocio Modificar() {
            Cls_Ope_Pat_Bienes_Inmuebles_Negocio BI_Negocio = new Cls_Ope_Pat_Bienes_Inmuebles_Negocio();
            BI_Negocio.P_Registro_Propiedad = Txt_Registro_Propiedad.Text.Trim();
            if (Txt_Fecha_Alta_Cta_Pub.Text.Trim().Length > 0) { BI_Negocio.P_Fecha_Alta_Cuenta_Publica = Convert.ToDateTime(Txt_Fecha_Alta_Cta_Pub.Text); }
            BI_Negocio.P_Bien_Inmueble_ID = Hdf_Bien_Inmueble_ID.Value;
            BI_Negocio.P_Calle = Hdf_Calle_ID.Value;
            BI_Negocio.P_Colonia = Hdf_Colonia_ID.Value;
            BI_Negocio.P_Uso_ID = Cmb_Uso.SelectedItem.Value;
            BI_Negocio.P_Origen_ID = Cmb_Origen.SelectedItem.Value;
            BI_Negocio.P_Estatus = Cmb_Estatus.SelectedItem.Value;
            if (Txt_Superficie.Text.Trim().Length > 0) { BI_Negocio.P_Superficie = Convert.ToDouble(Txt_Superficie.Text); }
            if (Txt_Construccion_Resgistrada.Text.Trim().Length > 0) { BI_Negocio.P_Construccion_Construida = Convert.ToDouble(Txt_Construccion_Resgistrada.Text); }
            BI_Negocio.P_Manzana = Txt_Manzana.Text.Trim();
            BI_Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value.Trim();
            BI_Negocio.P_Lote = Txt_Lote.Text.Trim();
            if (Txt_Porcentaje_Ocupacion.Text.Trim().Length > 0) { BI_Negocio.P_Ocupacion = Convert.ToDouble(Txt_Porcentaje_Ocupacion.Text); }
            BI_Negocio.P_Efectos_Fiscales = Txt_Efectos_Fiscales.Text.Trim();
            BI_Negocio.P_Sector_ID = Cmb_Sector.SelectedItem.Value;
            BI_Negocio.P_Clasificacion_Zona_ID = Cmb_Clasificacion_Zona.SelectedItem.Value;
            BI_Negocio.P_Tipo_Predio_ID = Cmb_Tipo_Predio.SelectedItem.Value;
            if (Txt_Valor_Comercial.Text.Trim().Length > 0) { BI_Negocio.P_Valor_Comercial = Convert.ToDouble(Txt_Valor_Comercial.Text); }
            BI_Negocio.P_Vias_Acceso = Txt_Vias_Aceso.Text.Trim();
            BI_Negocio.P_Estado = Cmb_Estado.SelectedItem.Value;
            if (Txt_Densidad_Construccion.Text.Trim().Length > 0) { BI_Negocio.P_Densidad_Construccion = Convert.ToDouble(Txt_Densidad_Construccion.Text); }
            BI_Negocio.P_No_Exterior = Txt_Numero_Exterior.Text.Trim();
            BI_Negocio.P_No_Interior = Txt_Numero_Interior.Text.Trim();
            BI_Negocio.P_Destino_ID = Cmb_Destino.SelectedItem.Value;
            BI_Negocio.P_Observaciones = Txt_Observaciones.Text.Trim();
            BI_Negocio.P_Area_ID = Cmb_Area_Donacion.SelectedItem.Value;
            BI_Negocio.P_Tipo_Bien = Cmb_Tipo_Bien.SelectedItem.Value;
            BI_Negocio.P_Fecha_Registro = Convert.ToDateTime(Txt_Fecha_Registro.Text);
            BI_Negocio.P_Dt_Medidas_Colindancias = (Session["Dt_Medidas_Colindancias"] != null) ? ((DataTable)Session["Dt_Medidas_Colindancias"]) : new DataTable();
            BI_Negocio.P_Dt_Anexos_Bajas = (Session["Dt_Anexos_Baja"] != null) ? ((DataTable)Session["Dt_Anexos_Baja"]) : new DataTable(); 
            //Juridico
            if (Txt_Escritura.Text.Trim().Length > 0) {
                BI_Negocio.P_No_Escritura = Txt_Escritura.Text;
                BI_Negocio.P_Fecha_Escritura = Convert.ToDateTime(Txt_Fecha_Escritura.Text);
                BI_Negocio.P_No_Notario = Txt_No_Notario.Text.Trim();
                BI_Negocio.P_Notario_Nombre = Txt_Nombre_Notario.Text.Trim();
                BI_Negocio.P_Constancia_Registral = Txt_Constacia_Registral.Text;
                BI_Negocio.P_Folio_Real = Txt_Folio_Real.Text;
                BI_Negocio.P_Libre_Gravament = Cmb_Libertad_Gravament.SelectedItem.Value;
                BI_Negocio.P_Antecedente = Txt_Antecedente_Registral.Text;
                BI_Negocio.P_Proveedor = Txt_Proveedor.Text;
                BI_Negocio.P_No_Contrato_Juridico = Txt_No_Contrato_Juridico.Text;
                BI_Negocio.P_No_Registro_Alta_Juridico = Hdf_No_Registro_Juridico_Alta.Value.Trim();
            }
            if (Cmb_Estado.SelectedItem.Value == "BAJA") {
                BI_Negocio.P_Fecha_Baja = Convert.ToDateTime(Txt_Fecha_Baja.Text);
                BI_Negocio.P_No_Escritura_Baja = Txt_Baja_No_Escritura.Text;
                BI_Negocio.P_Fecha_Escritura_Baja = Convert.ToDateTime(Txt_Baja_Fecha_Escritura.Text);
                BI_Negocio.P_No_Notario_Baja = Txt_Baja_No_Notario.Text.Trim();
                BI_Negocio.P_Notario_Nombre_Baja = Txt_Baja_Nombre_Notario.Text.Trim();
                BI_Negocio.P_Constancia_Registral_Baja = Txt_Baja_Constancia_Registral.Text;
                BI_Negocio.P_Folio_Real_Baja = Txt_Baja_Folio_Real.Text;
                BI_Negocio.P_Nuevo_Propietario_Juridico = Txt_Baja_Nuevo_Propietario.Text;
                BI_Negocio.P_No_Contrato_Baja = Txt_Baja_No_Contrato.Text;
                BI_Negocio.P_No_Registro_Baja_Juridico = Hdf_No_Registro_Juridico_Baja.Value.Trim();
            }

            String Nombre_Archivo = (DateTime.Now).ToString().Replace(".", "").Replace(" ", "").Replace("/", "").Replace("-", "").Replace(":", ""); 
            if (AFU_Ruta_Archivo.HasFile) {
                BI_Negocio.P_Tipo_Anexo = Cmb_Tipo_Archivo.SelectedItem.Value.Trim();
                BI_Negocio.P_Archivo = Nombre_Archivo + "_"+ AFU_Ruta_Archivo.FileName;
                BI_Negocio.P_Descripcion_Anexo = Txt_Descripcion_Archivo.Text.Trim();
            }

            //Expropiaciones
            BI_Negocio.P_Expropiacion = Txt_Expropiacion.Text.Trim();

            //Afectaciones
            if (Txt_No_Contrato.Text.Length > 0) {
                BI_Negocio.P_No_Contrato = Txt_No_Contrato.Text.Trim();
                BI_Negocio.P_Fecha_Afectacion = Convert.ToDateTime(Txt_Fecha_Afectacion.Text.Trim());
                BI_Negocio.P_Nuevo_Propietario = Txt_Nuevo_Propietario.Text.Trim();
                BI_Negocio.P_Session_Ayuntamiento = Txt_Session_Ayuntamiento.Text.Trim();
                BI_Negocio.P_Tramo = Txt_Tramo.Text.Trim();
                BI_Negocio.P_Agregar_Afectacion = true;
            }

            BI_Negocio.P_Hoja = Txt_Cont_Hoja.Text.Trim();
            BI_Negocio.P_Tomo = Txt_Cont_Tomo.Text.Trim();
            BI_Negocio.P_Numero_Acta = Txt_Cont_Numero_Acta.Text.Trim();
            BI_Negocio.P_Cartilla_Parcelaria = Txt_Cont_Cartilla_Parcelaria.Text.Trim();
            if (Txt_Cont_Superficie.Text.Trim().Length > 0) { BI_Negocio.P_Superficie_Contable = Convert.ToDouble(Txt_Cont_Superficie.Text); }
            BI_Negocio.P_Unidad_Superficie = Txt_Cont_Unidad_Superficie.Text.Trim();
            BI_Negocio.P_Clase_Activo_ID = Cmb_Clase_Activo.SelectedItem.Value;

            BI_Negocio.P_Usuario_ID = Cls_Sessiones.Empleado_ID;
            BI_Negocio.P_Usuario_Nombre = Cls_Sessiones.Nombre_Empleado;
            BI_Negocio.Modifica_Bien_Inmueble();
            return BI_Negocio;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Mostrar_Detalles_Bien_Inmueble
        ///DESCRIPCIÓN: Muestra detalles del Bien inmueble
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Mostrar_Detalles_Bien_Inmueble() {
            Cls_Ope_Pat_Bienes_Inmuebles_Negocio BI_Negocio = new Cls_Ope_Pat_Bienes_Inmuebles_Negocio();
            BI_Negocio.P_Bien_Inmueble_ID = Hdf_Bien_Inmueble_ID.Value;
            BI_Negocio = BI_Negocio.Consultar_Detalles_Bien_Inmueble();
            Txt_Bien_Inmueble_ID.Text = BI_Negocio.P_Bien_Inmueble_ID;
            Txt_Registro_Propiedad.Text = BI_Negocio.P_Registro_Propiedad;
            Hdf_Calle_ID.Value = BI_Negocio.P_Calle;
            Hdf_Colonia_ID.Value = BI_Negocio.P_Colonia;
            if (Hdf_Calle_ID.Value.Trim().Length > 0) { 
                Cls_Cat_Pre_Calles_Negocio Calle_Negocio = new Cls_Cat_Pre_Calles_Negocio();
                Calle_Negocio.P_Calle_ID = Hdf_Calle_ID.Value.Trim();
                DataTable Dt_Datos = Calle_Negocio.Consultar_Nombre_Id_Calles();
                if (Dt_Datos != null && Dt_Datos.Rows.Count > 0) {
                    Txt_Calle.Text = Dt_Datos.Rows[0][Cat_Pre_Calles.Campo_Nombre].ToString();
                }
            }
            if (Hdf_Colonia_ID.Value.Trim().Length > 0) {
                Cls_Ate_Colonias_Negocio Colonia_Negocio = new Cls_Ate_Colonias_Negocio();
                Colonia_Negocio.P_Filtros_Dinamicos = Cat_Ate_Colonias.Campo_Colonia_ID + " = '" + Hdf_Colonia_ID.Value.Trim() + "'";
                DataTable Dt_Datos = Colonia_Negocio.Consultar_Colonias();
                if (Dt_Datos != null && Dt_Datos.Rows.Count > 0) {
                    Txt_Colonia.Text = Dt_Datos.Rows[0][Cat_Ate_Colonias.Campo_Nombre].ToString();
                }
            }
            if (BI_Negocio.P_Uso_ID != null && BI_Negocio.P_Uso_ID.Trim().Length > 0) { Cmb_Uso.SelectedIndex = Cmb_Uso.Items.IndexOf(Cmb_Uso.Items.FindByValue(BI_Negocio.P_Uso_ID)); }
            if (BI_Negocio.P_Origen_ID != null && BI_Negocio.P_Origen_ID.Trim().Length > 0) { Cmb_Origen.SelectedIndex = Cmb_Origen.Items.IndexOf(Cmb_Origen.Items.FindByValue(BI_Negocio.P_Origen_ID)); }
            if (BI_Negocio.P_Estatus != null && BI_Negocio.P_Estatus.Trim().Length > 0) { Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(BI_Negocio.P_Estatus)); }
            if (BI_Negocio.P_Superficie > (-1)) { Txt_Superficie.Text = BI_Negocio.P_Superficie.ToString(); }
            if (BI_Negocio.P_Construccion_Construida > (-1)) { Txt_Construccion_Resgistrada.Text = BI_Negocio.P_Construccion_Construida.ToString(); }
            if (BI_Negocio.P_Manzana != null && BI_Negocio.P_Manzana.Trim().Length > 0) { Txt_Manzana.Text = BI_Negocio.P_Manzana.Trim(); }
            if (BI_Negocio.P_Tipo_Bien != null && BI_Negocio.P_Tipo_Bien.Trim().Length > 0) { Cmb_Tipo_Bien.SelectedIndex = Cmb_Tipo_Bien.Items.IndexOf(Cmb_Tipo_Bien.Items.FindByValue(BI_Negocio.P_Tipo_Bien)); }
            if (BI_Negocio.P_Area_ID != null && BI_Negocio.P_Area_ID.Trim().Length > 0) { Cmb_Area_Donacion.SelectedIndex = Cmb_Area_Donacion.Items.IndexOf(Cmb_Area_Donacion.Items.FindByValue(BI_Negocio.P_Area_ID)); }
            Hdf_Cuenta_Predial_ID.Value = BI_Negocio.P_Cuenta_Predial_ID;
            if (Hdf_Cuenta_Predial_ID.Value.Trim().Length > 0) {
                Cls_Cat_Pre_Cuentas_Predial_Negocio CP_Negocio = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
                CP_Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value.Trim();
                CP_Negocio.P_Incluir_Campos_Foraneos = true;
                DataTable DT_Cuentas_Predial = CP_Negocio.Consultar_Cuenta();
                if (DT_Cuentas_Predial != null && DT_Cuentas_Predial.Rows.Count > 0) {
                    Txt_Numero_Cuenta_Predial.Text = DT_Cuentas_Predial.Rows[0]["CUENTA_PREDIAL"].ToString().Trim();
                    Txt_Propietario.Text = DT_Cuentas_Predial.Rows[0]["NOMBRE_PROPIETARIO"].ToString().Trim();
                    if (!String.IsNullOrEmpty(DT_Cuentas_Predial.Rows[0]["VALOR_FISCAL"].ToString())) {
                        Txt_Valor_Catastral.Text = String.Format("{0:#,###,##0.00}", Convert.ToDouble(DT_Cuentas_Predial.Rows[0]["VALOR_FISCAL"].ToString()));
                    }
                }
            }
            if (BI_Negocio.P_Lote != null && BI_Negocio.P_Lote.Trim().Length > 0) { Txt_Lote.Text = BI_Negocio.P_Lote.Trim(); }
            if (BI_Negocio.P_Ocupacion > (-1)) { Txt_Porcentaje_Ocupacion.Text = BI_Negocio.P_Ocupacion.ToString(); }
            if (BI_Negocio.P_Efectos_Fiscales != null && BI_Negocio.P_Efectos_Fiscales.Trim().Length > 0) { Txt_Efectos_Fiscales.Text = BI_Negocio.P_Efectos_Fiscales.Trim(); }
            if (BI_Negocio.P_Sector_ID != null && BI_Negocio.P_Sector_ID.Trim().Length > 0) { Cmb_Sector.SelectedIndex = Cmb_Sector.Items.IndexOf(Cmb_Sector.Items.FindByValue(BI_Negocio.P_Sector_ID)); }
            if (BI_Negocio.P_Clasificacion_Zona_ID != null && BI_Negocio.P_Clasificacion_Zona_ID.Trim().Length > 0) { Cmb_Clasificacion_Zona.SelectedIndex = Cmb_Clasificacion_Zona.Items.IndexOf(Cmb_Clasificacion_Zona.Items.FindByValue(BI_Negocio.P_Clasificacion_Zona_ID)); }
            if (BI_Negocio.P_Tipo_Predio_ID != null && BI_Negocio.P_Tipo_Predio_ID.Trim().Length > 0) { Cmb_Tipo_Predio.SelectedIndex = Cmb_Tipo_Predio.Items.IndexOf(Cmb_Tipo_Predio.Items.FindByValue(BI_Negocio.P_Tipo_Predio_ID)); }
            if (BI_Negocio.P_Valor_Comercial > (-1)) { Txt_Valor_Comercial.Text = BI_Negocio.P_Valor_Comercial.ToString(); }
            if (BI_Negocio.P_Vias_Acceso != null && BI_Negocio.P_Vias_Acceso.Trim().Length > 0) { Txt_Vias_Aceso.Text = BI_Negocio.P_Vias_Acceso.Trim(); }
            Cmb_Estado.SelectedIndex = Cmb_Estado.Items.IndexOf(Cmb_Estado.Items.FindByValue(BI_Negocio.P_Estado));
            Cmb_Estado_SelectedIndexChanged(Cmb_Estado, null);
            if (BI_Negocio.P_Densidad_Construccion > (-1)) { Txt_Densidad_Construccion.Text = BI_Negocio.P_Densidad_Construccion.ToString().Trim(); }
            if (BI_Negocio.P_No_Exterior != null && BI_Negocio.P_No_Exterior.Trim().Length > 0) { Txt_Numero_Exterior.Text = BI_Negocio.P_No_Exterior.Trim(); }
            if (BI_Negocio.P_No_Interior != null && BI_Negocio.P_No_Interior.Trim().Length > 0) { Txt_Numero_Interior.Text = BI_Negocio.P_No_Interior.Trim(); }
            if (BI_Negocio.P_Destino_ID != null && BI_Negocio.P_Destino_ID.Trim().Length > 0) { Cmb_Destino.SelectedIndex = Cmb_Destino.Items.IndexOf(Cmb_Destino.Items.FindByValue(BI_Negocio.P_Destino_ID)); }
            if (!String.Format("{0:ddMMyyyy}", BI_Negocio.P_Fecha_Registro).Trim().Equals(String.Format("{0:ddMMyyyy}", new DateTime()))) { Txt_Fecha_Registro.Text = String.Format("{0:dd/MMM/yyyy}", BI_Negocio.P_Fecha_Registro); } else { Txt_Fecha_Registro.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Today); }
            if (!String.Format("{0:ddMMyyyy}", BI_Negocio.P_Fecha_Baja).Trim().Equals(String.Format("{0:ddMMyyyy}", new DateTime()))) { Txt_Fecha_Baja.Text = String.Format("{0:dd/MMM/yyyy}", BI_Negocio.P_Fecha_Baja); } else { Txt_Fecha_Baja.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Today); }
            if (!String.Format("{0:ddMMyyyy}", BI_Negocio.P_Fecha_Alta_Cuenta_Publica).Trim().Equals(String.Format("{0:ddMMyyyy}", new DateTime()))) { Txt_Fecha_Alta_Cta_Pub.Text = String.Format("{0:dd/MMM/yyyy}", BI_Negocio.P_Fecha_Alta_Cuenta_Publica); } 

            Txt_Cont_Hoja.Text = BI_Negocio.P_Hoja;
            Txt_Cont_Tomo.Text = BI_Negocio.P_Tomo;
            Txt_Cont_Numero_Acta.Text = BI_Negocio.P_Numero_Acta;
            Txt_Cont_Cartilla_Parcelaria.Text = BI_Negocio.P_Cartilla_Parcelaria;
            if (BI_Negocio.P_Superficie_Contable > (-1)) { Txt_Cont_Superficie.Text = BI_Negocio.P_Superficie_Contable.ToString().Trim(); }
            Txt_Cont_Unidad_Superficie.Text = BI_Negocio.P_Unidad_Superficie;
            Cmb_Clase_Activo.SelectedIndex = Cmb_Clase_Activo.Items.IndexOf(Cmb_Clase_Activo.Items.FindByValue(BI_Negocio.P_Clase_Activo_ID));
            
            Grid_Observaciones.PageIndex = 0;
            Llenar_Listado_Observaciones(BI_Negocio.P_Dt_Observaciones);
            Grid_Listado_Medidas_Colindancias.PageIndex = 0;
            Llenar_Listado_Medidas_Colindancias(BI_Negocio.P_Dt_Medidas_Colindancias);
            Mostrar_Detalles_Juridico(BI_Negocio.P_Dt_Historico_Juridico);
            Grid_Listado_Anexos.PageIndex = 0;
            Llenar_Listado_Anexos(BI_Negocio.P_Dt_Anexos);
            Grid_Expropiaciones.PageIndex = 0;
            Llenar_Listado_Expropiaciones(BI_Negocio.P_Dt_Expropiaciones);
            Grid_Afectaciones.PageIndex = 0;
            Llenar_Listado_Afectaciones(BI_Negocio.P_Dt_Afectaciones);
            Caracteres_Permitidos(ref FTE_Txt_Superficie, "AGREGAR", ",");
            Caracteres_Permitidos(ref FTE_Txt_Valor_Comercial, "AGREGAR", ",");
            Caracteres_Permitidos(ref FTE_Txt_Densidad_Construccion, "AGREGAR", ",");
            Caracteres_Permitidos(ref FTE_Txt_Construccion_Resgistrada, "AGREGAR", ",");
            Caracteres_Permitidos(ref FTE_Txt_Cont_Superficie, "AGREGAR", ",");
            Formato_Numerico(ref Txt_Superficie, "DISTANCIA");
            Formato_Numerico(ref Txt_Valor_Comercial, "DISTANCIA");
            Formato_Numerico(ref Txt_Densidad_Construccion, "DISTANCIA");
            Formato_Numerico(ref Txt_Construccion_Resgistrada, "DISTANCIA");
            Formato_Numerico(ref Txt_Cont_Superficie, "DISTANCIA");
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Mostrar_Detalles_Juridico
        ///DESCRIPCIÓN: Muestra detalles del Bien inmueble Parte de Juridico
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Mostrar_Detalles_Juridico(DataTable Dt_Juridico) {
            if (Dt_Juridico != null && Dt_Juridico.Rows.Count > 0) {
                DataRow[] Filas_Alta = Dt_Juridico.Select("MOVIMIENTO = 'ALTA'");
                foreach (DataRow Fila_Actual in Filas_Alta) {
                    Hdf_No_Registro_Juridico_Alta.Value = (Fila_Actual["NO_REGISTRO"] != null) ? Fila_Actual["NO_REGISTRO"].ToString() : "";
                    Txt_Escritura.Text = (Fila_Actual["ESCRITURA"] != null) ? Fila_Actual["ESCRITURA"].ToString() : "";
                    Txt_Fecha_Escritura.Text = (Fila_Actual["FECHA_ESCRITURA"] != null) ? String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Fila_Actual["FECHA_ESCRITURA"])) : "";
                    Txt_No_Notario.Text = (Fila_Actual["NO_NOTARIO"] != null) ? Fila_Actual["NO_NOTARIO"].ToString() : "";
                    Txt_Constacia_Registral.Text = (Fila_Actual["CONSTANCIA_REGISTRAL"] != null) ? Fila_Actual["CONSTANCIA_REGISTRAL"].ToString() : "";
                    Txt_Nombre_Notario.Text = (Fila_Actual["NOMBRE_COMPLETO_NOTARIO"] != null) ? Fila_Actual["NOMBRE_COMPLETO_NOTARIO"].ToString() : "";
                    Txt_Folio_Real.Text = (Fila_Actual["FOLIO_REAL"] != null) ? Fila_Actual["FOLIO_REAL"].ToString() : "";
                    if (Fila_Actual["LIBERTAD_GRAVAMEN"] != null) {
                        Cmb_Libertad_Gravament.SelectedIndex = Cmb_Libertad_Gravament.Items.IndexOf(Cmb_Libertad_Gravament.Items.FindByValue(Fila_Actual["LIBERTAD_GRAVAMEN"].ToString()));
                    }
                    Txt_Antecedente_Registral.Text = (Fila_Actual["ANTECEDENTE_REGISTRAL"] != null) ? Fila_Actual["ANTECEDENTE_REGISTRAL"].ToString() : "";
                    Txt_No_Contrato_Juridico.Text = (Fila_Actual["NO_CONTRATO"] != null) ? Fila_Actual["NO_CONTRATO"].ToString() : "";
                    Txt_Proveedor.Text = (Fila_Actual["PROVEEDOR"] != null) ? Fila_Actual["PROVEEDOR"].ToString() : "";
                }
                DataRow[] Filas_Baja = Dt_Juridico.Select("MOVIMIENTO = 'BAJA'");
                foreach (DataRow Fila_Actual in Filas_Baja) {
                    Hdf_No_Registro_Juridico_Baja.Value = (Fila_Actual["NO_REGISTRO"] != null) ? Fila_Actual["NO_REGISTRO"].ToString() : "";
                    Txt_Baja_No_Escritura.Text = (Fila_Actual["ESCRITURA"] != null) ? Fila_Actual["ESCRITURA"].ToString() : "";
                    Txt_Baja_Fecha_Escritura.Text = (Fila_Actual["FECHA_ESCRITURA"] != null) ? String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Fila_Actual["FECHA_ESCRITURA"])) : "";
                    Txt_Baja_No_Notario.Text = (Fila_Actual["NO_NOTARIO"] != null) ? Fila_Actual["NO_NOTARIO"].ToString() : "";
                    Txt_Baja_Constancia_Registral.Text = (Fila_Actual["CONSTANCIA_REGISTRAL"] != null) ? Fila_Actual["CONSTANCIA_REGISTRAL"].ToString() : "";
                    Txt_Baja_Nombre_Notario.Text = (Fila_Actual["NOMBRE_COMPLETO_NOTARIO"] != null) ? Fila_Actual["NOMBRE_COMPLETO_NOTARIO"].ToString() : "";
                    Txt_Baja_Folio_Real.Text = (Fila_Actual["FOLIO_REAL"] != null) ? Fila_Actual["FOLIO_REAL"].ToString() : "";
                    Txt_Baja_No_Contrato.Text = (Fila_Actual["NO_CONTRATO"] != null) ? Fila_Actual["NO_CONTRATO"].ToString() : "";
                    Txt_Baja_Nuevo_Propietario.Text = (Fila_Actual["NUEVO_PROPIETARIO"] != null) ? Fila_Actual["NUEVO_PROPIETARIO"].ToString() : "";
                    Cmb_Estado.Enabled = false;
                }
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Listado_Calles
        ///DESCRIPCIÓN: Llena el Grid de Busqueda de Calles
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Llenar_Listado_Calles() {
            Grid_Listado_Calles.SelectedIndex = (-1);
            Cls_Cat_Pre_Calles_Negocio Calles = new Cls_Cat_Pre_Calles_Negocio();
            Calles.P_Nombre_Calle = Txt_Nombre_Calles_Buscar.Text.Trim().ToUpper();
            DataTable Resultados_Calles = Calles.Consultar_Nombre();
            Resultados_Calles.Columns[Cat_Pre_Calles.Campo_Calle_ID].ColumnName = "CALLE_ID";
            Resultados_Calles.Columns[Cat_Pre_Calles.Campo_Nombre].ColumnName = "NOMBRE_CALLE";
            Grid_Listado_Calles.Columns[1].Visible = true;
            Grid_Listado_Calles.DataSource = Resultados_Calles;
            Grid_Listado_Calles.DataBind();
            Grid_Listado_Calles.Columns[1].Visible = false;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Listado_Cuentas_Predial
        ///DESCRIPCIÓN: Llena el Grid de Busqueda de Cuentas Predial
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Llenar_Listado_Cuentas_Predial() {
            Grid_Listado_Cuentas_Predial.SelectedIndex = (-1);
            Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
            Cuentas_Predial.P_Cuenta_Predial = Txt_Nombre_Cuenta_Predial_Buscar.Text.Trim().ToUpper();
            Cuentas_Predial.P_Incluir_Campos_Foraneos = true;
            DataTable Resultados_Cuentas_Predial = Cuentas_Predial.Consultar_Cuenta();
            Grid_Listado_Cuentas_Predial.Columns[1].Visible = true;
            Grid_Listado_Cuentas_Predial.DataSource = Resultados_Cuentas_Predial;
            Grid_Listado_Cuentas_Predial.DataBind();
            Grid_Listado_Cuentas_Predial.Columns[1].Visible = false;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Listado_Observaciones
        ///DESCRIPCIÓN: Llena el Grid de Historial de Observaciones
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Llenar_Listado_Observaciones(DataTable Dt_Observaciones) {
            Grid_Observaciones.Columns[0].Visible = true;
            Dt_Observaciones.DefaultView.Sort = "NO_OBSERVACION DESC";
            Grid_Observaciones.DataSource = Dt_Observaciones;
            Grid_Observaciones.DataBind();
            Grid_Observaciones.Columns[0].Visible = false;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Listado_Medidas_Colindancias
        ///DESCRIPCIÓN: Llena el Grid de Medidas y Colindancias
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Llenar_Listado_Medidas_Colindancias(DataTable Dt_Medidas_Colindancias) {
            if (Dt_Medidas_Colindancias != null && Dt_Medidas_Colindancias.Rows.Count > 0) {
                Session["Dt_Medidas_Colindancias"] = Dt_Medidas_Colindancias;
            } else {
                Session.Remove("Dt_Medidas_Colindancias");
            }
            Grid_Listado_Medidas_Colindancias.DataSource = Dt_Medidas_Colindancias;
            Grid_Listado_Medidas_Colindancias.DataBind();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Listado_Expropiaciones
        ///DESCRIPCIÓN: Llena el Grid de Historial de Observaciones
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Llenar_Listado_Expropiaciones(DataTable Dt_Expropiaciones) {
            Grid_Expropiaciones.Columns[0].Visible = true;
            Dt_Expropiaciones.DefaultView.Sort = "NO_EXPROPIACION DESC";
            Grid_Expropiaciones.DataSource = Dt_Expropiaciones;
            Grid_Expropiaciones.DataBind();
            Grid_Expropiaciones.Columns[0].Visible = false;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Listado_Afectaciones
        ///DESCRIPCIÓN: Llena el Grid de Historial de Afectaciones
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Llenar_Listado_Afectaciones(DataTable Dt_Afectaciones) {
            Grid_Afectaciones.Columns[0].Visible = true;
            Grid_Afectaciones.Columns[5].Visible = true;
            Grid_Afectaciones.Columns[6].Visible = true;
            Grid_Afectaciones.Columns[7].Visible = true;
            Grid_Afectaciones.Columns[8].Visible = true;
            Dt_Afectaciones.DefaultView.Sort = "NO_REGISTRO DESC";
            Grid_Afectaciones.DataSource = Dt_Afectaciones;
            Grid_Afectaciones.DataBind();
            Grid_Afectaciones.Columns[0].Visible = false;
            Grid_Afectaciones.Columns[5].Visible = false;
            Grid_Afectaciones.Columns[6].Visible = false;
            Grid_Afectaciones.Columns[7].Visible = false;
            Grid_Afectaciones.Columns[8].Visible = false;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Listado_Anexos
        ///DESCRIPCIÓN: Llena el Grid de Anexos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Llenar_Listado_Anexos(DataTable Dt_Anexos) {
            Session.Remove("Dt_Anexos_Baja");
            Grid_Listado_Anexos.Columns[0].Visible = true;
            Grid_Listado_Anexos.Columns[1].Visible = true;
            if (Dt_Anexos.Rows.Count > 0) { Dt_Anexos.DefaultView.Sort = "TIPO_ARCHIVO ASC"; }
            Grid_Listado_Anexos.DataSource = Dt_Anexos;
            Grid_Listado_Anexos.DataBind();
            Grid_Listado_Anexos.Columns[0].Visible = false;
            Grid_Listado_Anexos.Columns[1].Visible = false;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Remover_Sesiones_Control_AsyncFileUpload
        ///DESCRIPCIÓN: Limpia un control de AsyncFileUpload
        ///PROPIEDADES:     
        ///CREO: Juan Alberto Hernandez Negrete
        ///FECHA_CREO: 16/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Remover_Sesiones_Control_AsyncFileUpload(String Cliente_ID) {
            HttpContext Contexto;
            if (HttpContext.Current != null && HttpContext.Current.Session != null) {
                Contexto = HttpContext.Current;
            }  else {
                Contexto = null;
            }
            if (Contexto != null) {
                foreach (String key in Contexto.Session.Keys) {
                    if (key.Contains(Cliente_ID)) {
                        Contexto.Session.Remove(key);
                        break;
                    }
                }
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Validar_Medidas_Colindancias
        ///DESCRIPCIÓN: Validar los datos de Medidas y Colindancias
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private Boolean Validar_Medidas_Colindancias() { 
            Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
            String Mensaje_Error = "";
            Boolean Validacion = true;
            if (Cmb_Orientacion.SelectedIndex == 0) {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Seleccionar la Orientación.";
                Validacion = false;
            }
            if (Txt_Medida.Text.Trim().Length == 0) {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir la Medida.";
                Validacion = false;
            } else {
                if (!Validar_Valores_Decimales(Txt_Medida.Text.Trim())) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ La Medida tiene un Formato Incorrecto.";
                    Validacion = false;
                }
            }
            if (Txt_Colindancia.Text.Trim().Length == 0) {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir la Colindancia.";
                Validacion = false;
            }
            if (!Validacion) {
                Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
                Div_Contenedor_Msj_Error.Visible = true;
            }
            return Validacion;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Validar_Juridico
        ///DESCRIPCIÓN: Valida los datos para datos Juridicos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private Boolean Validar_Juridico() { 
            Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
            String Mensaje_Error = "";
            Boolean Validacion = true;
            if (Txt_Escritura.Text.Trim().Length == 0) {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir el No. de Escritura.";
                Validacion = false;
            }
            if (Txt_Fecha_Escritura.Text.Trim().Length == 0) {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir la Fecha de Escritura.";
                Validacion = false;
            }
            if (Txt_No_Notario.Text.Trim().Length == 0) {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir el No. Notario.";
                Validacion = false;
            }
            if (Txt_Nombre_Notario.Text.Trim().Length == 0) {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir el Nombre Notario.";
                Validacion = false;
            }
            if (Txt_Antecedente_Registral.Text.Length > 0) { 
                if (Txt_Antecedente_Registral.Text.Trim().Length > 4000) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Int32 Sobrecarga = Txt_Antecedente_Registral.Text.Trim().Length - 4000;
                    Mensaje_Error = Mensaje_Error + "+ El Antecedente Registral tiene como longitud Max. 4000 Caracteres [Sobrecarga de " + Sobrecarga;
                    if (Sobrecarga > 1) { Mensaje_Error += " Carácteres]."; } else { Mensaje_Error += " Carácter]."; }
                    Validacion = false;
                }
            }
            if (!Validacion) {
                Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
                Div_Contenedor_Msj_Error.Visible = true;
            }
            return Validacion;
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Validar_Juridico_Baja
        ///DESCRIPCIÓN: Valida los datos para datos Juridicos cuando hay baja
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private Boolean Validar_Juridico_Baja() { 
            Lbl_Ecabezado_Mensaje.Text = "Es necesario completar los campos de la Sección de Baja de Registros.";
            String Mensaje_Error = "";
            Boolean Validacion = true;
            if (Txt_Baja_No_Escritura.Text.Trim().Length == 0) {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir el No. de Escritura.";
                Validacion = false;
            }
            if (Txt_Baja_Fecha_Escritura.Text.Trim().Length == 0) {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir la Fecha de Escritura.";
                Validacion = false;
            }
            if (Txt_Baja_No_Notario.Text.Trim().Length == 0) {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir el No. Notario.";
                Validacion = false;
            }
            if (Txt_Baja_Nombre_Notario.Text.Trim().Length == 0) {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir el Nombre Notario.";
                Validacion = false;
            }
            if (!Validacion) {
                Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
                Div_Contenedor_Msj_Error.Visible = true;
            }
            return Validacion;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Validar_Anexo
        ///DESCRIPCIÓN: Valida los datos para cargar un anexo.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private Boolean Validar_Anexo() { 
            Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
            String Mensaje_Error = "";
            Boolean Validacion = true;
            if (Cmb_Tipo_Archivo.SelectedIndex == 0) {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Seleccionar el Tipo de Archivo.";
                Validacion = false;
            }
            if (!AFU_Ruta_Archivo.HasFile) {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Seleccionar el Archivo.";
                Validacion = false;
            }
            if (Txt_Descripcion_Archivo.Text.Trim().Length == 0) {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir la Descripción del Archivo.";
                Validacion = false;
            }
            if (!Validacion) {
                Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
                Div_Contenedor_Msj_Error.Visible = true;
            }
            return Validacion;
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Validar_Afectaciones
        ///DESCRIPCIÓN: Valida los datos para datos Afectaciones
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private Boolean Validar_Afectaciones() { 
            Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
            String Mensaje_Error = "";
            Boolean Validacion = true;
            if (Txt_No_Contrato.Text.Trim().Length == 0) {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir el No. de Contrato.";
                Validacion = false;
            }
            if (Txt_Fecha_Afectacion.Text.Trim().Length == 0) {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir la Fecha de Afectación.";
                Validacion = false;
            }
            if (Txt_Nuevo_Propietario.Text.Trim().Length == 0) {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir el Nuevo Propietario.";
                Validacion = false;
            }
            if (Txt_Session_Ayuntamiento.Text.Trim().Length == 0) {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir la Sessión de Ayuntamiento de la Afectación.";
                Validacion = false;
            }
            if (Txt_Tramo.Text.Trim().Length == 0) {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir el Tramo de la Afectación.";
                Validacion = false;
            }
            if (!Validacion) {
                Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
                Div_Contenedor_Msj_Error.Visible = true;
            }
            return Validacion;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Agregar_Dt_Anexos_Baja
        ///DESCRIPCIÓN: Lista los Anexos que se daran de Baja
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Agregar_Dt_Anexos_Baja(Int32 No_Registro) {
            DataTable Dt_Anexos_Baja = new DataTable();
            if (Session["Dt_Anexos_Baja"] == null) {
                Dt_Anexos_Baja.Columns.Add("No_Registro", Type.GetType("System.Int32"));
            } else {
                Dt_Anexos_Baja = (DataTable)Session["Dt_Anexos_Baja"];
            }
            DataRow Fila = Dt_Anexos_Baja.NewRow();
            Fila["No_Registro"] = No_Registro;
            Dt_Anexos_Baja.Rows.Add(Fila);
            Session["Dt_Anexos_Baja"] = Dt_Anexos_Baja;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Validar_Valores_Decimales
        ///DESCRIPCIÓN: Valida los valores decimales para un Anexo.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private Boolean Validar_Valores_Decimales(String Valor) {
            Boolean Validacion = true;
            Regex Expresion_Regular = new Regex(@"^[0-9]{1,50}(\.[0-9]{0,4})?$");
            Validacion = Expresion_Regular.IsMatch(Valor);
            return Validacion;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Listado_Colonias
        ///DESCRIPCIÓN: Llena la Busqueda de las Colonias.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Listado_Colonias() {
            Grid_Listado_Colonias.SelectedIndex = (-1);
            Cls_Cat_Ate_Colonias_Negocio Colonias_Negocio = new Cls_Cat_Ate_Colonias_Negocio();
            Colonias_Negocio.P_Nombre = Txt_Nombre_Colonia_Buscar.Text.Trim().ToUpper();
            DataTable Resultados_Colonias = Colonias_Negocio.Consulta_Datos().Tables[0];
            Resultados_Colonias.Columns[Cat_Ate_Colonias.Campo_Nombre].ColumnName = "NOMBRE_COLONIA";
            Resultados_Colonias.DefaultView.Sort = "NOMBRE_COLONIA";
            Grid_Listado_Colonias.Columns[1].Visible = true;
            Grid_Listado_Colonias.DataSource = Resultados_Colonias;
            Grid_Listado_Colonias.DataBind();
            Grid_Listado_Colonias.Columns[1].Visible = false;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Formato_Numerico
        ///DESCRIPCIÓN: Pasa un valor decimal a un formato.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Abril/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Formato_Numerico(ref TextBox Txt_Campo, String Tipo) {
            if (Txt_Campo.Text.Trim().Length > 0) {
                Double Valor = Convert.ToDouble(Txt_Campo.Text.Trim().Replace("$", "").Replace("%", "").Replace(",", ""));
                if (Tipo.Equals("DISTANCIA")) {
                    Txt_Campo.Text = String.Format("{0:#,###,##0.####}", Valor);
                } else if (Tipo.Equals("PORCENTUAL")) {
                    Txt_Campo.Text = String.Format("{0:##0.####}", Valor);
                } else if (Tipo.Equals("NUMERICO")) {
                    Txt_Campo.Text = Valor.ToString();
                }
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Formato_Numerico
        ///DESCRIPCIÓN: Pasa un valor decimal a un formato.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Abril/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Caracteres_Permitidos(ref AjaxControlToolkit.FilteredTextBoxExtender FTE_Txt_Campo, String Operacion, String Caracter) {
            if (Operacion.Equals("AGREGAR")) {
                FTE_Txt_Campo.ValidChars = FTE_Txt_Campo.ValidChars + Caracter;
            } else if (Operacion.Equals("QUITAR")) {
                FTE_Txt_Campo.ValidChars = FTE_Txt_Campo.ValidChars.Replace(Caracter, "");
            }
        }

    #endregion

    #region "Grids"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Calles_PageIndexChanging
        ///DESCRIPCIÓN: Evento de Cambio de Pagina del Grid
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Listado_Calles_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            Grid_Listado_Calles.PageIndex = e.NewPageIndex;
            Llenar_Listado_Calles();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Cuentas_Predial_PageIndexChanging
        ///DESCRIPCIÓN: Evento de Cambio de Pagina del Grid
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Listado_Cuentas_Predial_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            Grid_Listado_Cuentas_Predial.PageIndex = e.NewPageIndex;
            Llenar_Listado_Cuentas_Predial();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Calles_SelectedIndexChanged
        ///DESCRIPCIÓN: Evento de Cambio de Seleccion del Grid
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Listado_Calles_SelectedIndexChanged(object sender, EventArgs e) {
            if (Grid_Listado_Calles.SelectedIndex > (-1)) {
                Hdf_Calle_ID.Value = HttpUtility.HtmlDecode(Grid_Listado_Calles.SelectedRow.Cells[1].Text.Trim());
                Txt_Calle.Text = HttpUtility.HtmlDecode(Grid_Listado_Calles.SelectedRow.Cells[2].Text.Trim());
                Mpe_Calles_Cabecera.Hide();
                Cls_Ope_Pat_Bienes_Inmuebles_Negocio BI_Negocio = new Cls_Ope_Pat_Bienes_Inmuebles_Negocio();
                BI_Negocio.P_Calle = Hdf_Calle_ID.Value;
                BI_Negocio.P_Tipo_Complento = "COLONIA_DE_CALLE";
                BI_Negocio = BI_Negocio.Obtener_Complemento();
                Txt_Colonia.Text = BI_Negocio.P_Tipo_Complento;
                Hdf_Colonia_ID.Value = BI_Negocio.P_Colonia;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Cuentas_Predial_SelectedIndexChanged
        ///DESCRIPCIÓN: Evento de Cambio de Seleccion del Grid
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Listado_Cuentas_Predial_SelectedIndexChanged(object sender, EventArgs e) {
            if (Grid_Listado_Cuentas_Predial.SelectedIndex > (-1)) {
                Hdf_Cuenta_Predial_ID.Value = HttpUtility.HtmlDecode(Grid_Listado_Cuentas_Predial.SelectedRow.Cells[1].Text.Trim());
                if (Hdf_Cuenta_Predial_ID.Value.Trim().Length > 0) {
                    Cls_Cat_Pre_Cuentas_Predial_Negocio CP_Negocio = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
                    CP_Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value.Trim();
                    CP_Negocio.P_Incluir_Campos_Foraneos = true;
                    DataTable DT_Cuentas_Predial = CP_Negocio.Consultar_Cuenta();
                    if (DT_Cuentas_Predial != null && DT_Cuentas_Predial.Rows.Count > 0) {
                        Txt_Numero_Cuenta_Predial.Text = DT_Cuentas_Predial.Rows[0]["CUENTA_PREDIAL"].ToString().Trim();
                        Txt_Propietario.Text = DT_Cuentas_Predial.Rows[0]["NOMBRE_PROPIETARIO"].ToString().Trim();
                        if (!String.IsNullOrEmpty(DT_Cuentas_Predial.Rows[0]["VALOR_FISCAL"].ToString())) {
                            Txt_Valor_Catastral.Text = String.Format("{0:#,###,##0.00}", Convert.ToDouble(DT_Cuentas_Predial.Rows[0]["VALOR_FISCAL"].ToString()));
                        }
                    }
                }
                Mpe_Cuentas_Predial_Cabecera.Hide();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Medidas_Colindancias_RowDataBound
        ///DESCRIPCIÓN: Evento de llenado de datos en el Grid
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Listado_Medidas_Colindancias_RowDataBound(object sender, GridViewRowEventArgs e) {
            try {
                Int32 No_Fila = e.Row.RowIndex;
                if (e.Row.FindControl("Btn_Quitar_Medida_Colindancia") != null) {
                    ImageButton Btn_Quitar_Medida_Colindancia = (ImageButton)e.Row.FindControl("Btn_Quitar_Medida_Colindancia");
                    Btn_Quitar_Medida_Colindancia.CommandArgument = No_Fila.ToString();
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Verificar.";
                Lbl_Mensaje_Error.Text = "[" + Ex.Message + "]";
                Div_Contenedor_Msj_Error.Visible = true;
            } 
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Anexos_RowDataBound
        ///DESCRIPCIÓN: Evento de llenado de datos en el Grid
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Listado_Anexos_RowDataBound(object sender, GridViewRowEventArgs e) { 
            try {
                if (e.Row.FindControl("Btn_Ver_Anexo") != null) {
                    ImageButton Btn_Ver_Anexo = (ImageButton)e.Row.FindControl("Btn_Ver_Anexo");
                    Btn_Ver_Anexo.CommandArgument = HttpUtility.HtmlDecode(Hdf_Bien_Inmueble_ID.Value + "/" + e.Row.Cells[3].Text.Trim() + "/" + e.Row.Cells[1].Text.Trim());
                }
                if (e.Row.FindControl("Btn_Eliminar_Anexo") != null) {
                    ImageButton Btn_Eliminar_Anexo = (ImageButton)e.Row.FindControl("Btn_Eliminar_Anexo");
                    Btn_Eliminar_Anexo.CommandArgument = HttpUtility.HtmlDecode(e.Row.Cells[0].Text.Trim());
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Verificar.";
                Lbl_Mensaje_Error.Text = "[" + Ex.Message + "]";
                Div_Contenedor_Msj_Error.Visible = true;
            } 
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Afectaciones_RowDataBound
        ///DESCRIPCIÓN: Evento de llenado de datos en el Grid
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Afectaciones_RowDataBound(object sender, GridViewRowEventArgs e) {
            try {
                Int32 No_Fila = e.Row.RowIndex;
                if (e.Row.FindControl("Btn_Ver_Detalle_Afectaciones") != null) {
                    ImageButton Btn_Ver_Detalle_Afectaciones = (ImageButton)e.Row.FindControl("Btn_Ver_Detalle_Afectaciones");
                    Btn_Ver_Detalle_Afectaciones.CommandArgument = No_Fila.ToString();
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Verificar.";
                Lbl_Mensaje_Error.Text = "[" + Ex.Message + "]";
                Div_Contenedor_Msj_Error.Visible = true;
            } 
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Colonias_PageIndexChanging
        ///DESCRIPCIÓN: Evento de Cambio de Pagina de Listado Colonias
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Listado_Colonias_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            Grid_Listado_Colonias.PageIndex = e.NewPageIndex;
            Llenar_Listado_Colonias();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Colonias_SelectedIndexChanged
        ///DESCRIPCIÓN: Evento de Cambio de Seleccion de Listado Colonias
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Listado_Colonias_SelectedIndexChanged(object sender, EventArgs e) {
            if (Grid_Listado_Colonias.SelectedIndex > (-1)) {
                Hdf_Colonia_ID.Value = HttpUtility.HtmlDecode(Grid_Listado_Colonias.SelectedRow.Cells[1].Text.Trim());
                Txt_Colonia.Text = HttpUtility.HtmlDecode(Grid_Listado_Colonias.SelectedRow.Cells[2].Text.Trim());
                Mpe_Colonias.Hide();
            }
        }

    #endregion

    #region "Eventos"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
        ///DESCRIPCIÓN: Ejecuta el evento para generar un nuevo registro de Bienees Inmuebles
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e) {
            if (Btn_Nuevo.AlternateText.Trim().Equals("Nuevo")) {
                Limpiar_Generales_Formulario();
                Habiliar_Generales_Formulario(true);
                Btn_Modificar.Visible = false;
                Btn_Nuevo.Visible = true;
                Txt_Fecha_Registro.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Today);
                Cmb_Estado.SelectedIndex = Cmb_Estado.Items.IndexOf(Cmb_Estado.Items.FindByValue("ALTA"));
                Cmb_Estado.Enabled = false;
                Cmb_Estado_SelectedIndexChanged(Cmb_Estado, null);
                Txt_Bien_Inmueble_ID.Text = "<< AUTOMATICO >>";
            } else {
                Boolean Validacion = true;
                if (Txt_Escritura.Text.Trim().Length > 0) { Validacion = Validar_Juridico(); }
                if (Validacion) {
                    if (AFU_Ruta_Archivo.HasFile) { Validacion = Validar_Anexo(); }
                    if (Validacion) {
                        if (Txt_No_Contrato.Text.Trim().Length > 0) { Validacion = Validar_Afectaciones(); }
                        if (Validacion) { 
                            Cls_Ope_Pat_Bienes_Inmuebles_Negocio BI_Negocio = Alta();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "GACO", "alert('ALTA EXITOSA!!!');", true);
                            Habiliar_Generales_Formulario(false);
                            if (AFU_Ruta_Archivo.HasFile) {
                                String Ruta = Server.MapPath(Ope_Pat_B_Inm_Archivos.Ruta_Archivos_Inmuebles + BI_Negocio.P_Bien_Inmueble_ID + "/" + Cmb_Tipo_Archivo.SelectedItem.Value);
                                if (!Directory.Exists(Ruta)) {
                                    Directory.CreateDirectory(Ruta);
                                }
                                Ruta = Ruta + "/" + BI_Negocio.P_Archivo;
                                AFU_Ruta_Archivo.SaveAs(Ruta);
                            }
                            //Hdf_Bien_Inmueble_ID.Value = BI_Negocio.P_Bien_Inmueble_ID;
                            //String BI_ID = null;
                            //BI_ID = Hdf_Bien_Inmueble_ID.Value;
                            Limpiar_Generales_Formulario();
                            if (BI_Negocio.P_Bien_Inmueble_ID != null || BI_Negocio.P_Bien_Inmueble_ID.Trim().Length > 0) {
                                Hdf_Bien_Inmueble_ID.Value = BI_Negocio.P_Bien_Inmueble_ID;
                                Mostrar_Detalles_Bien_Inmueble();
                            }
                        } 
                    }
                }
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
        ///DESCRIPCIÓN: Ejecuta el evento para actualizar un registro de Bienees Inmuebles
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e) {
            if (Btn_Modificar.AlternateText.Trim().Equals("Modificar")) {
                if (Hdf_Bien_Inmueble_ID.Value.Trim().Length > 0) {
                    Habiliar_Generales_Formulario(true);
                    Btn_Nuevo.Visible = false;
                    Caracteres_Permitidos(ref FTE_Txt_Superficie, "QUITAR", ",");
                    Caracteres_Permitidos(ref FTE_Txt_Valor_Comercial, "QUITAR", ",");
                    Caracteres_Permitidos(ref FTE_Txt_Densidad_Construccion, "QUITAR", ",");
                    Caracteres_Permitidos(ref FTE_Txt_Construccion_Resgistrada, "QUITAR", ",");
                    Caracteres_Permitidos(ref FTE_Txt_Cont_Superficie, "QUITAR", ",");
                    Formato_Numerico(ref Txt_Superficie, "NUMERICO");
                    Formato_Numerico(ref Txt_Valor_Comercial, "NUMERICO");
                    Formato_Numerico(ref Txt_Densidad_Construccion, "NUMERICO");
                    Formato_Numerico(ref Txt_Construccion_Resgistrada, "NUMERICO");
                    Formato_Numerico(ref Txt_Cont_Superficie, "NUMERICO");
                } else {
                    Lbl_Ecabezado_Mensaje.Text = "Verificar.";
                    Lbl_Mensaje_Error.Text = "Se debe seleccionar el Bien Inmueble a Actualizar su información.";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            } else {
                Boolean Validacion = true;
                if (Txt_Escritura.Text.Trim().Length > 0) { Validacion = Validar_Juridico(); }
                if (Validacion) {
                    if (AFU_Ruta_Archivo.HasFile) { Validacion = Validar_Anexo(); }
                    if (Validacion) {
                        if (Txt_No_Contrato.Text.Trim().Length > 0) { Validacion = Validar_Afectaciones(); }
                        if (Validacion) {
                            if (Cmb_Estado.SelectedItem.Value == "BAJA") { Validacion = Validar_Juridico_Baja(); }
                            if (Validacion) { 
                                Cls_Ope_Pat_Bienes_Inmuebles_Negocio BI_Negocio = Modificar();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "GACO", "alert('ACTUALIZACION EXITOSA!!!');", true);
                                Habiliar_Generales_Formulario(false);
                                if (AFU_Ruta_Archivo.HasFile) {
                                    String Ruta = Server.MapPath(Ope_Pat_B_Inm_Archivos.Ruta_Archivos_Inmuebles + BI_Negocio.P_Bien_Inmueble_ID + "/" + Cmb_Tipo_Archivo.SelectedItem.Value);
                                    if (!Directory.Exists(Ruta)) {
                                        Directory.CreateDirectory(Ruta);
                                    }
                                    Ruta = Ruta + "/" + BI_Negocio.P_Archivo;
                                    AFU_Ruta_Archivo.SaveAs(Ruta);
                                }
                                //Hdf_Bien_Inmueble_ID.Value = BI_Negocio.P_Bien_Inmueble_ID;
                                //String BI_ID = null;
                                //BI_ID = Hdf_Bien_Inmueble_ID.Value;
                                Limpiar_Generales_Formulario();
                                if (BI_Negocio.P_Bien_Inmueble_ID != null || BI_Negocio.P_Bien_Inmueble_ID.Trim().Length > 0) {
                                    Hdf_Bien_Inmueble_ID.Value = BI_Negocio.P_Bien_Inmueble_ID;
                                    Mostrar_Detalles_Bien_Inmueble();
                                }
                            }
                        }
                    }
                }
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
        ///DESCRIPCIÓN: Ejecuta el evento para salir del formulario
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Salir_Click(object sender, ImageClickEventArgs e) {
            if (Btn_Salir.AlternateText.Trim().Equals("Salir")) {
                Response.Redirect("Frm_Ope_Pat_Entrada_Bienes_Inmuebles.aspx");
            } else {
                if (Btn_Nuevo.Visible) {
                    Limpiar_Generales_Formulario();
                    Habiliar_Generales_Formulario(false);
                    Response.Redirect("Frm_Ope_Pat_Entrada_Bienes_Inmuebles.aspx");
                } else {
                    String BI_ID = null;
                    BI_ID = Hdf_Bien_Inmueble_ID.Value;
                    Limpiar_Generales_Formulario();
                    Habiliar_Generales_Formulario(false);
                    if (BI_ID != null) {
                        Hdf_Bien_Inmueble_ID.Value = BI_ID;
                        Mostrar_Detalles_Bien_Inmueble();
                    } else {
                        Response.Redirect("Frm_Ope_Pat_Entrada_Bienes_Inmuebles.aspx");
                    }
                }
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Ejecutar_Busqueda_Calles_Click
        ///DESCRIPCIÓN: Ejecuta el evento para lanzar el buscador de calles
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Ejecutar_Busqueda_Calles_Click(object sender, ImageClickEventArgs e) {
            Grid_Listado_Calles.PageIndex = 0;
            Llenar_Listado_Calles();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Txt_Nombre_Calles_Buscar_TextChanged
        ///DESCRIPCIÓN: Ejecuta el evento buscar carlles por el nombre
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Txt_Nombre_Calles_Buscar_TextChanged(object sender, EventArgs e) {
            try {
                Grid_Listado_Calles.PageIndex = 0;
                Llenar_Listado_Calles();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Calle_Click
        ///DESCRIPCIÓN: Ejecuta el evento buscar carlles.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Calle_Click(object sender, ImageClickEventArgs e) {
            Mpe_Calles_Cabecera.Show();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Ejecutar_Busqueda_Cuenta_Predial_Click
        ///DESCRIPCIÓN: Ejecuta el evento para lanzar el buscador de Cuentas Predial
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Ejecutar_Busqueda_Cuenta_Predial_Click(object sender, ImageClickEventArgs e) {
            Grid_Listado_Cuentas_Predial.PageIndex = 0;
            Llenar_Listado_Cuentas_Predial();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Txt_Nombre_Cuenta_Predial_Buscar_TextChanged
        ///DESCRIPCIÓN: Ejecuta el evento para buscar las cuentas predial por el no. de cuenta
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Txt_Nombre_Cuenta_Predial_Buscar_TextChanged(object sender, EventArgs e) {
            try {
                Grid_Listado_Cuentas_Predial.PageIndex = 0;
                Llenar_Listado_Cuentas_Predial();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Numero_Cuenta_Predial_Click
        ///DESCRIPCIÓN: Ejecuta el evento para buscar las cuentas predial.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Numero_Cuenta_Predial_Click(object sender, ImageClickEventArgs e) {
            Mpe_Cuentas_Predial_Cabecera.Show();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Medida_Colindancia_Click
        ///DESCRIPCIÓN: Agrada la medida como detalle del Bien Inmueble
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Agregar_Medida_Colindancia_Click(object sender, EventArgs e) {
            if (Validar_Medidas_Colindancias()) { 
                DataTable Tabla = (DataTable) Grid_Listado_Medidas_Colindancias.DataSource;
                if (Tabla == null) {
                    if (Session["Dt_Medidas_Colindancias"] == null) {
                        Tabla = new DataTable("Dt_Medidas_Colindancias");
                        Tabla.Columns.Add("ORIENTACION_ID", Type.GetType("System.String"));
                        Tabla.Columns.Add("ORIENTACION", Type.GetType("System.String"));
                        Tabla.Columns.Add("MEDIDA", Type.GetType("System.Double"));
                        Tabla.Columns.Add("COLINDANCIA", Type.GetType("System.String"));
                    } else {
                        Tabla = (DataTable)Session["Dt_Medidas_Colindancias"];
                    }
                }
                DataRow Fila = Tabla.NewRow();
                Fila["ORIENTACION_ID"] = Cmb_Orientacion.SelectedItem.Value;
                Fila["ORIENTACION"] = Cmb_Orientacion.SelectedItem.Text;
                Fila["MEDIDA"] = Convert.ToDouble(Txt_Medida.Text);
                Fila["COLINDANCIA"] = Txt_Colindancia.Text.Trim();
                Tabla.Rows.Add(Fila);
                Llenar_Listado_Medidas_Colindancias(Tabla);
                Limpiar_Medidas_Colindancias();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Quitar_Medida_Colindancia_Click
        ///DESCRIPCIÓN: Quita la medida como detalle del Bien Inmueble
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Quitar_Medida_Colindancia_Click(object sender, ImageClickEventArgs e) {
            try {
                ImageButton Btn_Quitar_Medida_Colindancia = (ImageButton) sender;
                if (!String.IsNullOrEmpty(Btn_Quitar_Medida_Colindancia.CommandArgument)) {
                    Int32 No_Fila = Convert.ToInt32(Btn_Quitar_Medida_Colindancia.CommandArgument);
                    DataTable Dt_Medidas_Colindancias = (Session["Dt_Medidas_Colindancias"] != null) ? ((DataTable)Session["Dt_Medidas_Colindancias"]) : new DataTable();
                    if (Dt_Medidas_Colindancias.Rows.Count > 0) {
                        Dt_Medidas_Colindancias.Rows.RemoveAt(No_Fila);
                        Llenar_Listado_Medidas_Colindancias(Dt_Medidas_Colindancias);
                    }
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Verificar.";
                Lbl_Mensaje_Error.Text = "[" + Ex.Message + "]";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Ver_Anexo_Click
        ///DESCRIPCIÓN: Muestra el Anexo del Bien Inmueble
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Ver_Anexo_Click(object sender, ImageClickEventArgs e) {
            try {
                ImageButton Btn_Ver_Anexo = (ImageButton)sender;
                if (!String.IsNullOrEmpty(Btn_Ver_Anexo.CommandArgument)) {
                    String Nombre_Archivo = Ope_Pat_B_Inm_Archivos.Ruta_Archivos_Inmuebles + Btn_Ver_Anexo.CommandArgument;
                    if (File.Exists(Server.MapPath(Nombre_Archivo))) {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Archivo_Archivos", "window.open('" + Nombre_Archivo + "','Window_Archivo','left=0,top=0')", true);
                    } else {
                        Lbl_Ecabezado_Mensaje.Text = "El Archivo no esta disponible o fue eliminado";
                        Lbl_Mensaje_Error.Text = "";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Verificar.";
                Lbl_Mensaje_Error.Text = "[" + Ex.Message + "]";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Anexo_Click
        ///DESCRIPCIÓN: Hace una eliminacion Logica del Anexo
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Eliminar_Anexo_Click(object sender, ImageClickEventArgs e) {
            try {
                ImageButton Btn_Eliminar_Anexo = (ImageButton)sender;
                if (!String.IsNullOrEmpty(Btn_Eliminar_Anexo.CommandArgument)) {
                    Int32 No_Registro = Convert.ToInt32(Btn_Eliminar_Anexo.CommandArgument);
                    Agregar_Dt_Anexos_Baja(No_Registro);
                    Btn_Eliminar_Anexo.Visible = false;
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Verificar.";
                Lbl_Mensaje_Error.Text = "[" + Ex.Message + "]";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Ver_Detalle_Afectaciones_Click
        ///DESCRIPCIÓN: Muestra el Detalle de Afectaciones como parte del Bien Inmueble
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Ver_Detalle_Afectaciones_Click(object sender, ImageClickEventArgs e) {
            try {
                ImageButton Btn_Ver_Detalle_Afectaciones = (ImageButton)sender;
                if (!String.IsNullOrEmpty(Btn_Ver_Detalle_Afectaciones.CommandArgument)) {
                    Int32 No_Fila = Convert.ToInt32(Btn_Ver_Detalle_Afectaciones.CommandArgument);
                    GridViewRow Fila_Grid = Grid_Afectaciones.Rows[No_Fila];
                    if (Fila_Grid != null) {
                        Txt_Mpe_No_Contrato.Text = HttpUtility.HtmlDecode(Fila_Grid.Cells[1].Text);
                        Txt_Mpe_Fecha_Afectacion.Text = HttpUtility.HtmlDecode(Fila_Grid.Cells[2].Text);
                        Txt_Mpe_Nuevo_Propietario.Text = HttpUtility.HtmlDecode(Fila_Grid.Cells[3].Text);
                        Txt_Mpe_Session_Ayuntamiento.Text = HttpUtility.HtmlDecode(Fila_Grid.Cells[5].Text);
                        Txt_Mpe_Tramo.Text = HttpUtility.HtmlDecode(Fila_Grid.Cells[6].Text);
                        String Fecha = HttpUtility.HtmlDecode(Fila_Grid.Cells[8].Text);
                        Txt_Mpe_Datos_Auditoria.Text = HttpUtility.HtmlDecode(Fila_Grid.Cells[7].Text + " [" + ((Fecha.Trim().Length > 0) ? String.Format("{0:dd/MMM/yyyy, hh:mm:ss tt}", Convert.ToDateTime(Fecha)) : "") + "]");
                        UpPnl_Mpe_Detalles_Afectaciones.Update();
                    }
                }
                Mpe_Detalles_Afectaciones.Show();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Verificar.";
                Lbl_Mensaje_Error.Text = "[" + Ex.Message + "]";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Cmb_Estado_SelectedIndexChanged
        ///DESCRIPCIÓN: Maneja el Evento de Cambio de Estado.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Cmb_Estado_SelectedIndexChanged(object sender, EventArgs e) {
            if (Cmb_Estado.SelectedItem.Value == "ALTA") {
                Div_Campos_Juridico_Baja.Visible = false;
            } else {
                Txt_Fecha_Baja.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Today);
                Div_Campos_Juridico_Baja.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Ver_Ficha_Tecnica_PDF_Click
        ///DESCRIPCIÓN: Lanza el Reporte de Ficha Tecnica del Bien Actual
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Ver_Ficha_Tecnica_PDF_Click(object sender, ImageClickEventArgs e) {
            try{
                Cargar_Tablas_Reporte("PDF");
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Verificar.";
                Lbl_Mensaje_Error.Text = Ex.Message;
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Ver_Ficha_Tecnica_Excel_Click
        ///DESCRIPCIÓN: Lanza el Reporte de Ficha Tecnica del Bien Actual
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Ver_Ficha_Tecnica_Excel_Click(object sender, ImageClickEventArgs e) {
            try{
                Cargar_Tablas_Reporte("EXCEL");
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Verificar.";
                Lbl_Mensaje_Error.Text = Ex.Message;
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Cargar_Tablas_Reporte
        ///DESCRIPCIÓN: Maneja las tablas del Reporte de Ficha Tecnica
        ///PROPIEDADES:   1.  P_Imagen.  Imagen a Convertir.    
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Cargar_Tablas_Reporte(String Tipo) {
            Cls_Rpt_Pat_Listado_Bienes_Negocio Reporte_Negocio = new Cls_Rpt_Pat_Listado_Bienes_Negocio();
            Reporte_Negocio.P_Bien_ID = Hdf_Bien_Inmueble_ID.Value;
            DataTable Dt_Datos_Generales_Reporte = Reporte_Negocio.Consultar_Datos_Generales_BI_Ficha_Tecnica();
            DataTable Dt_Datos_Medidas_Colindancias_Reporte = Reporte_Negocio.Consultar_Datos_Medidas_Colindancias_BI_Ficha_Tecnica();
            Dt_Datos_Generales_Reporte.Columns.Add("FOTO", Type.GetType("System.Byte[]"));
            Dt_Datos_Generales_Reporte.Columns.Add("MAPA", Type.GetType("System.Byte[]"));
            Dt_Datos_Generales_Reporte.Columns.Add("COMPLEMENTOS", Type.GetType("System.String"));
            Dt_Datos_Generales_Reporte.Columns.Add("LEVANTAMIENTO_TOPOGRAFICO", Type.GetType("System.Byte[]"));
            StringBuilder Bienes_Inmuebles_ID = new StringBuilder();
            for (Int32 Contador = 0; Contador < Dt_Datos_Generales_Reporte.Rows.Count; Contador++) {
                if (Contador > 0) {
                    Bienes_Inmuebles_ID.Append(",'");
                }
                Bienes_Inmuebles_ID.Append(Dt_Datos_Generales_Reporte.Rows[Contador]["BIEN_INMUEBLE_ID"].ToString().Trim() + "'");
                //Se carga la foto
                Dt_Datos_Generales_Reporte.DefaultView.AllowEdit = true;
                Reporte_Negocio = new Cls_Rpt_Pat_Listado_Bienes_Negocio();
                Reporte_Negocio.P_Bien_ID = Dt_Datos_Generales_Reporte.Rows[Contador]["BIEN_INMUEBLE_ID"].ToString().Trim();
                Reporte_Negocio.P_Tipo = "FOTOGRAFIA";
                DataTable Dt_Tmp = null;
                Dt_Tmp = Reporte_Negocio.Consultar_Datos_Archivos_BI_Ficha_Tecnica();
                if (Dt_Tmp != null && Dt_Tmp.Rows.Count > 0) {
                    Dt_Datos_Generales_Reporte.Rows[Contador].BeginEdit();
                    String Nombre_Archivo = Dt_Tmp.Rows[0]["RUTA_ARCHIVO"].ToString().Trim();
                    String Directorio = Server.MapPath(Ope_Pat_B_Inm_Archivos.Ruta_Archivos_Inmuebles + Dt_Tmp.Rows[0]["BIEN_INMUEBLE_ID"].ToString().Trim() + "/FOTOGRAFIA");
                    String Nombre_Completo_Archivo = Directorio + "/" + Nombre_Archivo;
                    if (File.Exists(Nombre_Completo_Archivo)) {
                        Dt_Datos_Generales_Reporte.Rows[Contador]["FOTO"] = Convertir_Imagen_A_Cadena_Bytes(System.Drawing.Image.FromFile(Nombre_Completo_Archivo));
                    }
                    Dt_Datos_Generales_Reporte.Rows[Contador].EndEdit();
                }
                Dt_Datos_Generales_Reporte.DefaultView.AllowEdit = false;

                //Se carga el mapa
                Dt_Datos_Generales_Reporte.DefaultView.AllowEdit = true;
                Reporte_Negocio = new Cls_Rpt_Pat_Listado_Bienes_Negocio();
                Reporte_Negocio.P_Bien_ID = Dt_Datos_Generales_Reporte.Rows[Contador]["BIEN_INMUEBLE_ID"].ToString().Trim();
                Reporte_Negocio.P_Tipo = "MAPA";
                Dt_Tmp = null;
                Dt_Tmp = Reporte_Negocio.Consultar_Datos_Archivos_BI_Ficha_Tecnica();
                if (Dt_Tmp != null && Dt_Tmp.Rows.Count > 0) {
                    Dt_Datos_Generales_Reporte.Rows[Contador].BeginEdit();
                    String Nombre_Archivo = Dt_Tmp.Rows[0]["RUTA_ARCHIVO"].ToString().Trim();
                    String Directorio = Server.MapPath(Ope_Pat_B_Inm_Archivos.Ruta_Archivos_Inmuebles + Dt_Tmp.Rows[0]["BIEN_INMUEBLE_ID"].ToString().Trim() + "/MAPA");
                    String Nombre_Completo_Archivo = Directorio + "/" + Nombre_Archivo;
                    if (File.Exists(Nombre_Completo_Archivo)) {
                        Dt_Datos_Generales_Reporte.Rows[Contador]["MAPA"] = Convertir_Imagen_A_Cadena_Bytes(System.Drawing.Image.FromFile(Nombre_Completo_Archivo));
                    }
                    Dt_Datos_Generales_Reporte.Rows[Contador].EndEdit();
                }
                Dt_Datos_Generales_Reporte.DefaultView.AllowEdit = false;

                //Se carga el Levantamiento Topografico
                Dt_Datos_Generales_Reporte.DefaultView.AllowEdit = true;
                Reporte_Negocio = new Cls_Rpt_Pat_Listado_Bienes_Negocio();
                Reporte_Negocio.P_Bien_ID = Dt_Datos_Generales_Reporte.Rows[Contador]["BIEN_INMUEBLE_ID"].ToString().Trim();
                Reporte_Negocio.P_Tipo = "LEVANTAMIENTO_TOPOGRAFICO";
                Dt_Tmp = null;
                Dt_Tmp = Reporte_Negocio.Consultar_Datos_Archivos_BI_Ficha_Tecnica();
                if (Dt_Tmp != null && Dt_Tmp.Rows.Count > 0)
                {
                    Dt_Datos_Generales_Reporte.Rows[Contador].BeginEdit();
                    String Nombre_Archivo = Dt_Tmp.Rows[0]["RUTA_ARCHIVO"].ToString().Trim();
                    String Directorio = Server.MapPath(Ope_Pat_B_Inm_Archivos.Ruta_Archivos_Inmuebles + Dt_Tmp.Rows[0]["BIEN_INMUEBLE_ID"].ToString().Trim() + "/LEVANTAMIENTO_TOPOGRAFICO");
                    String Nombre_Completo_Archivo = Directorio + "/" + Nombre_Archivo;
                    if (File.Exists(Nombre_Completo_Archivo))
                    {
                        Dt_Datos_Generales_Reporte.Rows[Contador]["LEVANTAMIENTO_TOPOGRAFICO"] = Convertir_Imagen_A_Cadena_Bytes(System.Drawing.Image.FromFile(Nombre_Completo_Archivo));
                    }
                    Dt_Datos_Generales_Reporte.Rows[Contador].EndEdit();
                }
                Dt_Datos_Generales_Reporte.DefaultView.AllowEdit = false;

                //Se carga el mapa
                Dt_Datos_Generales_Reporte.DefaultView.AllowEdit = true;
                Reporte_Negocio = new Cls_Rpt_Pat_Listado_Bienes_Negocio();
                Reporte_Negocio.P_Bien_ID = Dt_Datos_Generales_Reporte.Rows[Contador]["BIEN_INMUEBLE_ID"].ToString().Trim();
                Dt_Tmp = null;
                Dt_Tmp = Reporte_Negocio.Consultar_Datos_Observaciones_BI_Ficha_Tecnica();
                if (Dt_Tmp != null && Dt_Tmp.Rows.Count > 0) {
                    Dt_Datos_Generales_Reporte.Rows[Contador].BeginEdit();
                    Dt_Datos_Generales_Reporte.Rows[Contador]["COMPLEMENTOS"] = Dt_Tmp.Rows[0]["OBSERVACION"].ToString();
                    Dt_Datos_Generales_Reporte.Rows[Contador].EndEdit();
                }
                Dt_Datos_Generales_Reporte.DefaultView.AllowEdit = false;
            }

            //Se Consultan las Medidas y Colindancias
            Reporte_Negocio = new Cls_Rpt_Pat_Listado_Bienes_Negocio();
            Reporte_Negocio.P_Bien_ID = Bienes_Inmuebles_ID.ToString().Trim('\'');
            DataTable Dt_Tmp_ = null;
            Dt_Tmp_ = Reporte_Negocio.Consultar_Datos_Medidas_Colindancias_BI_Ficha_Tecnica();
            Dt_Datos_Generales_Reporte.TableName = "DT_GENERALES";
            Dt_Tmp_.TableName = "DT_MEDIAS_COLINDANCIAS";
            DataSet Ds_Consulta = new DataSet();
            Ds_Consulta.Tables.Add(Dt_Datos_Generales_Reporte.Copy());
            Ds_Consulta.Tables.Add(Dt_Tmp_.Copy());
            Ds_Rpt_Pat_Ficha_Tecnica_Bienes_Inmuebles Ds_Reporte = new Ds_Rpt_Pat_Ficha_Tecnica_Bienes_Inmuebles();
            if (Tipo.Equals("PDF")) { Generar_Reporte(Ds_Consulta, Ds_Reporte, "Rpt_Pat_Ficha_Tecnica_Bienes_Inmuebles.rpt"); }
            else if (Tipo.Equals("EXCEL")) { Generar_Reporte_Excel(Ds_Consulta, Ds_Reporte, "Rpt_Pat_Ficha_Tecnica_Bienes_Inmuebles.rpt"); }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
        ///DESCRIPCIÓN: caraga el data set fisico con el cual se genera el Reporte especificado
        ///PARAMETROS:  1.-Data_Set_Consulta_DB.- Contiene la informacion de la consulta a la base de datos
        ///             2.-Ds_Reporte, Objeto que contiene la instancia del Data set fisico del Reporte a generar
        ///             3.-Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
        ///CREO: Susana Trigueros Armenta.
        ///FECHA_CREO: 01/Mayo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        private void Generar_Reporte(DataSet Data_Set_Consulta_DB, DataSet Ds_Reporte, string Nombre_Reporte) {
            ReportDocument Reporte = new ReportDocument();
            String File_Path = Server.MapPath("../Rpt/Compras/" + Nombre_Reporte);
            Reporte.Load(File_Path);
            String Ruta = "../../Reporte/Ficha_Tecnica_" + Hdf_Bien_Inmueble_ID.Value + "" + Session.SessionID + String.Format("{0:ddMMyyyyhhmmss}", DateTime.Now) + ".pdf";
            Ds_Reporte = Data_Set_Consulta_DB;
            Reporte.SetDataSource(Ds_Reporte);
            ExportOptions Export_Options = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
            Disk_File_Destination_Options.DiskFileName = Server.MapPath(Ruta);
            Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
            Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
            Reporte.Export(Export_Options);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600');", true);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Generar_Reporte_Excel
        ///DESCRIPCIÓN: caraga el data set fisico con el cual se genera el Reporte especificado
        ///PARAMETROS:  1.-Data_Set_Consulta_DB.- Contiene la informacion de la consulta a la base de datos
        ///             2.-Ds_Reporte, Objeto que contiene la instancia del Data set fisico del Reporte a generar
        ///             3.-Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
        ///CREO: Susana Trigueros Armenta.
        ///FECHA_CREO: 01/Mayo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        private void Generar_Reporte_Excel(DataSet Data_Set_Consulta_DB, DataSet Ds_Reporte, string Nombre_Reporte) {
            ReportDocument Reporte = new ReportDocument();
            String File_Path = Server.MapPath("../Rpt/Compras/" + Nombre_Reporte);
            Reporte.Load(File_Path);
            String Ruta = "../../Reporte/Ficha_Tecnica_" + Hdf_Bien_Inmueble_ID.Value + "" + Session.SessionID + String.Format("{0:ddMMyyyyhhmmss}", DateTime.Now) + ".xls";
            Ds_Reporte = Data_Set_Consulta_DB;
            Reporte.SetDataSource(Ds_Reporte);
            ExportOptions Export_Options = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
            Disk_File_Destination_Options.DiskFileName = Server.MapPath(Ruta);
            Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
            Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options.ExportFormatType = ExportFormatType.Excel;
            Reporte.Export(Export_Options);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600');", true);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Convertir_Imagen_A_Cadena_Bytes
        ///DESCRIPCIÓN: Convierte la Imagen a una Cadena de Bytes.
        ///PROPIEDADES:   1.  P_Imagen.  Imagen a Convertir.    
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private Byte[] Convertir_Imagen_A_Cadena_Bytes(System.Drawing.Image P_Imagen) {
            Byte[] Img_Bytes = null;
            try {
                if (P_Imagen != null) {
                    MemoryStream MS_Tmp = new MemoryStream();
                    P_Imagen.Save(MS_Tmp, P_Imagen.RawFormat);
                    Img_Bytes = MS_Tmp.GetBuffer();
                    MS_Tmp.Close();
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Verificar.";
                Lbl_Mensaje_Error.Text = "Verificar.";
                Div_Contenedor_Msj_Error.Visible = false;
            }
            return Img_Bytes;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Colonia_Click
        ///DESCRIPCIÓN: Lanza el Buscador de las Colonias
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Colonia_Click(object sender, ImageClickEventArgs e) {
            Mpe_Colonias.Show();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Ejecutar_Busqueda_Colonia_Click
        ///DESCRIPCIÓN: Buscar Colonia
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Ejecutar_Busqueda_Colonia_Click(object sender, ImageClickEventArgs e) {
            Grid_Listado_Colonias.PageIndex = 0;
            Llenar_Listado_Colonias();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Txt_Nombre_Colonia_Buscar_TextChanged
        ///DESCRIPCIÓN: Buscar Colonia
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Txt_Nombre_Colonia_Buscar_TextChanged(object sender, EventArgs e) {
            try {
                Grid_Listado_Colonias.PageIndex = 0;
                Llenar_Listado_Colonias();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

    #endregion

}