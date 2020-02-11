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
using Presidencia.Control_Patrimonial_Catalogo_Aseguradoras.Negocio;
using Presidencia.Sessiones;
using System.Collections.Generic;
using Presidencia.Constantes;

public partial class paginas_predial_Frm_Cat_Pat_Com_Aseguradoras : System.Web.UI.Page {

    #region Page_Load

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 25/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        protected void Page_Load(object sender, EventArgs e){
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!IsPostBack)
            {
                Configuracion_Formulario(true);
                Llenar_Grid_Aseguradoras(0);
                Grid_Aseguradora_Contacto.Columns[1].Visible = false;
                Grid_Aseguradora_Domicilio.Columns[1].Visible = false;
                Grid_Aseguradora_Bancos.Columns[1].Visible = false;
            }
            Div_Contenedor_Msj_Error.Visible = false;
        }

    #endregion

    #region Metodos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
        ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
        ///PROPIEDADES:     
        ///             1. Estatus. Estatus en el que se cargara la configuración de los controles.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 25/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Configuracion_Formulario( Boolean Estatus ) {
            //GENERALES
            Btn_Nuevo.Visible = true;
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            Btn_Modificar.Visible = true;
            Btn_Modificar.AlternateText = "Modificar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            Btn_Eliminar.Visible = Estatus;
            Txt_Nombre.Enabled = !Estatus;
            Txt_Nombre_Fiscal.Enabled = !Estatus;
            Txt_Nombre_Comercial.Enabled = !Estatus;
            Txt_RFC.Enabled = !Estatus;
            Txt_Cuenta_Contable.Enabled = !Estatus;
            Cmb_Estatus.Enabled = !Estatus;
            Grid_Aseguradoras_Generales.Enabled = Estatus;
            Grid_Aseguradoras_Generales.SelectedIndex = (-1);
            Btn_Buscar.Enabled = Estatus;
            Txt_Busqueda.Enabled = Estatus;
            
            //CONTACTOS
            Txt_Dato_Contacto.Enabled = !Estatus;
            Txt_Descripcion_Contacto.Enabled = !Estatus;
            Txt_Telefono_Contacto.Enabled = !Estatus;
            Txt_Fax_Contacto.Enabled = !Estatus;
            Txt_Celular_Contacto.Enabled = !Estatus;
            Txt_Correo_Electronico_Contacto.Enabled = !Estatus;
            Cmb_Estatus_Contacto.Enabled = !Estatus;
            Btn_Agregar_Contacto.Visible = !Estatus;
            Btn_Quitar_Contacto.Visible = !Estatus;
            Btn_Modificar_Contacto.Visible = !Estatus;
            Grid_Aseguradora_Contacto.SelectedIndex = (-1);
            Grid_Aseguradora_Contacto.Columns[1].Visible = false;

            //DOMICILIOS
            Txt_Nombre_Domicilio.Enabled = !Estatus;
            Txt_Descripcion_Domicilio.Enabled = !Estatus;
            Txt_Calle_Domicilio.Enabled = !Estatus;
            Txt_Numero_Exterior.Enabled = !Estatus;
            Txt_Numero_Interior.Enabled = !Estatus;
            Txt_Fax_Domicilio.Enabled = !Estatus;
            Txt_Colonia_Domicilio.Enabled = !Estatus;
            Txt_Codigo_Postal_Domicilio.Enabled = !Estatus;
            Txt_Ciudad_Domicilio.Enabled = !Estatus;
            Txt_Municipio_Domicilio.Enabled = !Estatus;
            Txt_Estado_Domicilio.Enabled = !Estatus;
            Txt_Pais_Domicilio.Enabled = !Estatus;
            Cmb_Estatus_Domicilio.Enabled = !Estatus;
            Btn_Agregar_Domicilio.Visible = !Estatus;
            Btn_Quitar_Domicilio.Visible = !Estatus;
            Btn_Modificar_Domicilio.Visible = !Estatus;
            Grid_Aseguradora_Domicilio.SelectedIndex = (-1);
            Grid_Aseguradora_Domicilio.Columns[1].Visible = false;

            //BANCOS
            Txt_Producto_Bancario.Enabled = !Estatus;
            Txt_Descripcion_Banco.Enabled = !Estatus;
            Txt_Institucion_Bancaria.Enabled = !Estatus;
            Txt_Cuenta_Banco.Enabled = !Estatus;
            Txt_Clabe_Institucion_Bancaria.Enabled = !Estatus;
            Txt_Clabe_Plaza.Enabled = !Estatus;
            Txt_Clabe_Cuenta.Enabled = !Estatus;
            Txt_Clabe_Digito_Verificador.Enabled = !Estatus;
            Txt_Clave_CIE.Enabled = !Estatus;
            Txt_Numero_Tarjeta.Enabled = !Estatus;
            Txt_Numero_Tarjeta_Reverso.Enabled = !Estatus;
            Btn_Fecha_Vigencia_Banco.Enabled = !Estatus;
            Cmb_Estatus_Banco.Enabled = !Estatus;
            Btn_Agregar_Banco.Visible = !Estatus;
            Btn_Quitar_Banco.Visible = !Estatus;
            Btn_Modificar_Banco.Visible = !Estatus;
            Grid_Aseguradora_Bancos.SelectedIndex = (-1);
            Grid_Aseguradora_Bancos.Columns[1].Visible = false;

            //Configuracion_Acceso("Frm_Cat_Pat_Com_Aseguradoras.aspx");
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo_Generales
        ///DESCRIPCIÓN: Limpia los controles del Formulario de la pestaña de Generales
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 25/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Catalogo_Generales() {
            Hdf_Aseguradora_ID.Value = "";
            Txt_Aseguradora_ID.Text = "";
            Txt_Nombre.Text = "";
            Txt_Nombre_Fiscal.Text = "";
            Txt_Nombre_Comercial.Text = "";
            Txt_RFC.Text = "";
            Txt_Cuenta_Contable.Text = "";
            Cmb_Estatus.SelectedIndex = 0;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo_Contactos
        ///DESCRIPCIÓN: Limpia los controles del Formulario de la Pestaña de Contactos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 25/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Catalogo_Contactos(){
            Hdf_Aseguradora_Contacto_ID.Value = "";
            Txt_Aseguradora_Contacto_ID.Text = "";
            Txt_Dato_Contacto.Text = "";
            Txt_Descripcion_Contacto.Text = "";
            Txt_Telefono_Contacto.Text = "";
            Txt_Fax_Contacto.Text = "";
            Txt_Celular_Contacto.Text = "";
            Txt_Correo_Electronico_Contacto.Text = "";
            Cmb_Estatus_Contacto.SelectedIndex = 0;
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo_Domicilios
        ///DESCRIPCIÓN: Limpia los controles del Formulario de la Pestaña de Domicilios
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 25/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Catalogo_Domicilios() {
            Hdf_Aseguradora_Domicilio_ID.Value = "";
            Txt_Aseguradora_Domicilio_ID.Text = "";
            Txt_Nombre_Domicilio.Text = "";
            Txt_Descripcion_Domicilio.Text = "";
            Txt_Calle_Domicilio.Text = "";
            Txt_Numero_Exterior.Text = "";
            Txt_Numero_Interior.Text = "";
            Txt_Fax_Domicilio.Text = "";
            Txt_Colonia_Domicilio.Text = "";
            Txt_Codigo_Postal_Domicilio.Text = "";
            Txt_Ciudad_Domicilio.Text = "";
            Txt_Municipio_Domicilio.Text = "";
            Txt_Estado_Domicilio.Text = "";
            Txt_Pais_Domicilio.Text = "";
            Cmb_Estatus_Domicilio.SelectedIndex = 0;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo_Bancos
        ///DESCRIPCIÓN: Limpia los controles del Formulario de la Pestaña de Bancos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 26/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Catalogo_Bancos() {
            Hdf_Aseguradora_Banco_ID.Value = "";
            Txt_Aseguradora_Banco_ID.Text = "";
            Txt_Producto_Bancario.Text = "";
            Txt_Descripcion_Banco.Text = "";
            Txt_Institucion_Bancaria.Text = "";
            Txt_Cuenta_Banco.Text = "";
            Txt_Clabe_Institucion_Bancaria.Text = "";
            Txt_Clabe_Plaza.Text = "";
            Txt_Clabe_Cuenta.Text = "";
            Txt_Clabe_Digito_Verificador.Text = "";
            Txt_Clave_CIE.Text = "";
            Txt_Numero_Tarjeta.Text = "";
            Txt_Numero_Tarjeta_Reverso.Text = "";
            Txt_Fecha_Vigencia_Banco.Text = "";
            Cmb_Estatus_Banco.SelectedIndex = 0;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_SubCatalogos
        ///DESCRIPCIÓN: Limpia los controles del Formulario de los SubCatalogos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 25/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_SubCatalogos() {
            //Contactos
            Limpiar_Catalogo_Contactos();
            Grid_Aseguradora_Contacto.DataSource = new DataTable();
            Grid_Aseguradora_Contacto.DataBind();
            Session.Remove("Dt_Contactos");

            //Domicilios
            Limpiar_Catalogo_Domicilios();
            Grid_Aseguradora_Domicilio.DataSource = new DataTable();
            Grid_Aseguradora_Domicilio.DataBind();
            Session.Remove("Dt_Domicilios");

            //Bancos
            Limpiar_Catalogo_Bancos();
            Grid_Aseguradora_Bancos.DataSource = new DataTable();
            Grid_Aseguradora_Bancos.DataBind();
            Session.Remove("Dt_Bancos");
        }        

        #region Validaciones
    
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Generales
            ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de 
            ///             hacer una operación de la pestaña de Datos Generales.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 25/Noviembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private bool Validar_Componentes_Generales() {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                String Mensaje_Error = "";
                Boolean Validacion = true;
                if (Txt_Nombre.Text.Trim().Length == 0) {
                    Mensaje_Error = Mensaje_Error + "+ Introducir el Nombre.";
                    Validacion = false;
                }
                if (Txt_Nombre_Fiscal.Text.Trim().Length == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir el Nombre Fiscal.";
                    Validacion = false;
                }
                if (Txt_RFC.Text.Trim().Length == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir el RFC.";
                    Validacion = false;
                }
                if (Cmb_Estatus.SelectedIndex == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Estatus.";
                    Validacion = false;
                }
                if (!Validacion) {
                    Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
                    Div_Contenedor_Msj_Error.Visible = true;
                }
                return Validacion;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Contactos
            ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
            ///             una operación de la pestaña de Contactos.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 25/Noviembre/2010  
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private bool Validar_Componentes_Contactos() {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                String Mensaje_Error = "";
                Boolean Validacion = true;
                if (Txt_Dato_Contacto.Text.Trim().Length == 0) {
                    Mensaje_Error = Mensaje_Error + "+ Introducir el Dato del Contacto.";
                    Validacion = false;
                }
                if (Txt_Descripcion_Contacto.Text.Trim().Length == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Descripción del Contacto.";
                    Validacion = false;
                }
                if (Txt_Telefono_Contacto.Text.Trim().Length == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir el Teléfono del Contacto.";
                    Validacion = false;
                }
                if (Cmb_Estatus_Contacto.SelectedIndex == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Estatus del Contacto.";
                    Validacion = false;
                }
                if (!Validacion) {
                    Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
                    Div_Contenedor_Msj_Error.Visible = true;
                }
                return Validacion;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Domicilios
            ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
            ///             una operación de la pestaña de Domicilios.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 25/Noviembre/2010  
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private bool Validar_Componentes_Domicilios() {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                String Mensaje_Error = "";
                Boolean Validacion = true;
                if (Txt_Nombre_Domicilio.Text.Trim().Length == 0) {
                    Mensaje_Error = Mensaje_Error + "+ Introducir el Nombre del Domicilio.";
                    Validacion = false;
                }
                if (Txt_Descripcion_Domicilio.Text.Trim().Length == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Descripción del Domicilio.";
                    Validacion = false;
                }
                if (Txt_Calle_Domicilio.Text.Trim().Length == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Calle del Domicilio.";
                    Validacion = false;
                }
                if (Cmb_Estatus_Domicilio.SelectedIndex == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Estatus del Domicilio.";
                    Validacion = false;
                }
                if (!Validacion) {
                    Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
                    Div_Contenedor_Msj_Error.Visible = true;
                }
                return Validacion;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Bancos
            ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
            ///             una operación de la pestaña de Bancos.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 26/Noviembre/2010  
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private bool Validar_Componentes_Bancos()
            {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                String Mensaje_Error = "";
                Boolean Validacion = true;
                if (Txt_Producto_Bancario.Text.Trim().Length == 0) {
                    Mensaje_Error = Mensaje_Error + "+ Introducir el Producto Bancario.";
                    Validacion = false;
                }
                if (Txt_Descripcion_Banco.Text.Trim().Length == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Descripción del Producto Bancario.";
                    Validacion = false;
                }
                if (Txt_Institucion_Bancaria.Text.Trim().Length == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Institución Bancaria.";
                    Validacion = false;
                }
                if (Cmb_Estatus_Banco.SelectedIndex == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Estatus del Banco.";
                    Validacion = false;
                }
                if (!Validacion) {
                    Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
                    Div_Contenedor_Msj_Error.Visible = true;
                }
                return Validacion;
            }

        #endregion

        #region Grid
    
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Aseguradoras
            ///DESCRIPCIÓN: Llena la tabla de Aseguradoras.
            ///PROPIEDADES:     
            ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 25/Noviembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Grid_Aseguradoras(Int32 Pagina) {
                try{
                    Cls_Cat_Pat_Com_Aseguradoras_Negocio Aseguradora = new Cls_Cat_Pat_Com_Aseguradoras_Negocio();
                    Aseguradora.P_Tipo_DataTable = "ASEGURADORAS";
                    Aseguradora.P_Nombre = Txt_Busqueda.Text.Trim();
                    Grid_Aseguradoras_Generales.DataSource = Aseguradora.Consultar_DataTable();
                    Grid_Aseguradoras_Generales.PageIndex = Pagina;
                    Grid_Aseguradoras_Generales.DataBind();
                }catch(Exception Ex){
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;                
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Contactos
            ///DESCRIPCIÓN: Llena la tabla de Contactos
            ///PROPIEDADES:     
            ///             1.  Pagina. Pagina en la cual se mostrará el Grid_VIew
            ///             2.  tabla.  Tabla que se va a cargar en el Grid.    
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 25/Noviembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Grid_Contactos(Int32 Pagina, DataTable Tabla) {
                Grid_Aseguradora_Contacto.Columns[1].Visible = true;
                Grid_Aseguradora_Contacto.DataSource = Tabla;
                Grid_Aseguradora_Contacto.PageIndex = Pagina;
                Grid_Aseguradora_Contacto.DataBind();
                Grid_Aseguradora_Contacto.Columns[1].Visible = false;
                Session["Dt_Contactos"] = Tabla;
                Checar_Contactos_Registrados();
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Domicilios
            ///DESCRIPCIÓN: Llena la tabla de Domicilios
            ///PROPIEDADES:     
            ///             1.  Pagina. Pagina en la cual se mostrará el Grid_VIew
            ///             2.  tabla.  Tabla que se va a cargar en el Grid.    
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 25/Noviembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Grid_Domicilios(Int32 Pagina, DataTable Tabla) {
                Grid_Aseguradora_Domicilio.Columns[1].Visible = true;
                Grid_Aseguradora_Domicilio.DataSource = Tabla;
                Grid_Aseguradora_Domicilio.PageIndex = Pagina;
                Grid_Aseguradora_Domicilio.DataBind();
                Grid_Aseguradora_Domicilio.Columns[1].Visible = false;
                Session["Dt_Domicilios"] = Tabla;
                Checar_Domicilios_Registrados();
            }
    
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Bancos
            ///DESCRIPCIÓN: Llena la tabla de Bancos
            ///PROPIEDADES:     
            ///             1.  Pagina. Pagina en la cual se mostrará el Grid_VIew
            ///             2.  tabla.  Tabla que se va a cargar en el Grid.    
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 26/Noviembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Grid_Bancos(Int32 Pagina, DataTable Tabla) {
                Grid_Aseguradora_Bancos.Columns[1].Visible = true;
                Grid_Aseguradora_Bancos.DataSource = Tabla;
                Grid_Aseguradora_Bancos.PageIndex = Pagina;
                Grid_Aseguradora_Bancos.DataBind();
                Grid_Aseguradora_Bancos.Columns[1].Visible = false;
                Session["Dt_Bancos"] = Tabla;
                Checar_Bancos_Registrados();
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Checar_Contactos_Registrados
            ///DESCRIPCIÓN: Verifica los contactos con el estatus de registrado.
            ///PROPIEDADES:       
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 26/Noviembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Checar_Contactos_Registrados() {
                if (Session["Dt_Contactos"] != null) {
                    DataTable Contactos = (DataTable)Session["Dt_Contactos"];
                    for(Int32 Contador_Grid = 0; Contador_Grid < Grid_Aseguradora_Contacto.Rows.Count; Contador_Grid++) {
                        Int32 Registro = ((Grid_Aseguradora_Contacto.PageIndex) * Grid_Aseguradora_Contacto.PageSize) + (Contador_Grid);
                        if (Contactos.Rows[Registro][0].ToString().Trim().Equals(Grid_Aseguradora_Contacto.Rows[Contador_Grid].Cells[1].Text.Trim())) {
                            CheckBox Check_Temporal = (CheckBox)Grid_Aseguradora_Contacto.Rows[Contador_Grid].FindControl("Chk_Contacto_Registrado");
                            if (Contactos.Rows[Registro][3].ToString().Trim().Equals("S")) {
                                Check_Temporal.Checked = true;
                            } else {
                                Check_Temporal.Checked = false;
                            }
                        }
                    }
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Checar_Domicilios_Registrados
            ///DESCRIPCIÓN: Verifica los domicilios con el estatus de registrado.
            ///PROPIEDADES:       
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 26/Noviembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Checar_Domicilios_Registrados() {
                if (Session["Dt_Domicilios"] != null) {
                    DataTable Domicilios = (DataTable)Session["Dt_Domicilios"];
                    for (Int32 Contador_Grid = 0; Contador_Grid < Grid_Aseguradora_Domicilio.Rows.Count; Contador_Grid++) {
                        Int32 Registro = ((Grid_Aseguradora_Domicilio.PageIndex) * Grid_Aseguradora_Domicilio.PageSize) + (Contador_Grid);
                        if (Domicilios.Rows[Registro][0].ToString().Trim().Equals(Grid_Aseguradora_Domicilio.Rows[Contador_Grid].Cells[1].Text.Trim())) {
                            CheckBox Check_Temporal = (CheckBox)Grid_Aseguradora_Domicilio.Rows[Contador_Grid].FindControl("Chk_Domicilio_Registrado");
                            if (Domicilios.Rows[Registro][3].ToString().Trim().Equals("S")) {
                                Check_Temporal.Checked = true;
                            } else {
                                Check_Temporal.Checked = false;
                            }
                        }
                    }
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Checar_Bancos_Registrados
            ///DESCRIPCIÓN: Verifica los bancos con el estatus de registrado.
            ///PROPIEDADES:       
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 26/Noviembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Checar_Bancos_Registrados() {
                if (Session["Dt_Bancos"] != null) {
                    DataTable Bancos = (DataTable)Session["Dt_Bancos"];
                    for (Int32 Contador_Grid = 0; Contador_Grid < Grid_Aseguradora_Bancos.Rows.Count; Contador_Grid++) {
                        Int32 Registro = ((Grid_Aseguradora_Bancos.PageIndex) * Grid_Aseguradora_Bancos.PageSize) + (Contador_Grid);
                        if (Bancos.Rows[Registro][0].ToString().Trim().Equals(Grid_Aseguradora_Bancos.Rows[Contador_Grid].Cells[1].Text.Trim())) {
                            CheckBox Check_Temporal = (CheckBox)Grid_Aseguradora_Bancos.Rows[Contador_Grid].FindControl("Chk_Banco_Registrado");
                            if (Bancos.Rows[Registro][3].ToString().Trim().Equals("S")) {
                                Check_Temporal.Checked = true;
                            } else {
                                Check_Temporal.Checked = false;
                            }
                        }
                    }
                }
            }

        #endregion
    
    #endregion

    #region Grids
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Aseguradoras_Generales_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView General de las Aseguradoras
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 25/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Aseguradoras_Generales_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try{
                Grid_Aseguradoras_Generales.SelectedIndex = (-1);
                Llenar_Grid_Aseguradoras(e.NewPageIndex);
                Limpiar_Catalogo_Generales();
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Aseguradoras_Generales_SelectedIndexChanged
        ///DESCRIPCIÓN: Obtiene los datos de la Aseguradora Seleccionado para mostrarlos a detalle
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 25/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Aseguradoras_Generales_SelectedIndexChanged(object sender, EventArgs e) {
            try{
                if (Grid_Aseguradoras_Generales.SelectedIndex > (-1)){
                    Limpiar_Catalogo_Generales();
                    Limpiar_SubCatalogos();
                    String ID_Seleccionado = Grid_Aseguradoras_Generales.SelectedRow.Cells[1].Text;
                    Cls_Cat_Pat_Com_Aseguradoras_Negocio Aseguradora = new Cls_Cat_Pat_Com_Aseguradoras_Negocio();
                    Aseguradora.P_Aseguradora_ID = ID_Seleccionado;
                    Aseguradora = Aseguradora.Consultar_Datos_Aseguradora();
                    Hdf_Aseguradora_ID.Value = Aseguradora.P_Aseguradora_ID;
                    Txt_Aseguradora_ID.Text = Aseguradora.P_Aseguradora_ID;
                    Txt_Nombre.Text = Aseguradora.P_Nombre;
                    Txt_Nombre_Fiscal.Text = Aseguradora.P_Nombre_Fiscal;
                    Txt_Nombre_Comercial.Text = Aseguradora.P_Nombre_Comercial;
                    Txt_RFC.Text = Aseguradora.P_RFC;
                    Txt_Cuenta_Contable.Text = Aseguradora.P_Cuenta_Contable;
                    Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Aseguradora.P_Estatus));
                    Llenar_Grid_Contactos(0, Aseguradora.P_Contactos);
                    Llenar_Grid_Domicilios(0, Aseguradora.P_Domicilios);
                    Llenar_Grid_Bancos(0, Aseguradora.P_Bancos);
                    System.Threading.Thread.Sleep(1000);
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Aseguradora_Contacto_PageIndexChanging
        ///DESCRIPCIÓN: Maneja el Cambio de Pagina de la Tabla de Contactos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 25/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Aseguradora_Contacto_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                if (Session["Dt_Contactos"] != null){
                    DataTable Tabla = (DataTable)Session["Dt_Contactos"];
                    Llenar_Grid_Contactos(e.NewPageIndex, Tabla);
                    Grid_Aseguradora_Contacto.SelectedIndex = (-1);
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Aseguradora_Domicilio_PageIndexChanging
        ///DESCRIPCIÓN: Maneja el Cambio de Pagina de la Tabla de Domicilios
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 25/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Aseguradora_Domicilio_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                if (Session["Dt_Domicilios"] != null) {
                    DataTable Tabla = (DataTable)Session["Dt_Domicilios"];
                    Llenar_Grid_Domicilios(e.NewPageIndex, Tabla);
                    Grid_Aseguradora_Domicilio.SelectedIndex = (-1);
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Aseguradora_Bancos_PageIndexChanging
        ///DESCRIPCIÓN: Maneja el Cambio de Pagina de la Tabla de Bancos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 26/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Aseguradora_Bancos_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                if (Session["Dt_Bancos"] != null) {
                    DataTable Tabla = (DataTable)Session["Dt_Bancos"];
                    Llenar_Grid_Bancos(e.NewPageIndex, Tabla);
                    Grid_Aseguradora_Bancos.SelectedIndex = (-1);
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

    #endregion

    #region Eventos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
        ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta una nueva Aseguradora
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 25/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Nuevo_Click(object sender, EventArgs e){
            try{
                if (Btn_Nuevo.AlternateText.Equals("Nuevo")){
                    Configuracion_Formulario(false);
                    Limpiar_Catalogo_Generales();
                    Limpiar_SubCatalogos();
                    Btn_Nuevo.AlternateText = "Dar de Alta";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.Visible = false;
                    Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue("VIGENTE"));
                }else {
                    if (Validar_Componentes_Generales()){
                        Cls_Cat_Pat_Com_Aseguradoras_Negocio Aseguradora = new Cls_Cat_Pat_Com_Aseguradoras_Negocio();
                        Aseguradora.P_Nombre = Txt_Nombre.Text.Trim();
                        Aseguradora.P_Nombre_Fiscal = Txt_Nombre_Fiscal.Text.Trim();
                        Aseguradora.P_Nombre_Comercial = Txt_Nombre_Comercial.Text.Trim();
                        Aseguradora.P_RFC = Txt_RFC.Text.Trim();
                        Aseguradora.P_Cuenta_Contable = Txt_Cuenta_Contable.Text.Trim();
                        Aseguradora.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        Aseguradora.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                        if (Session["Dt_Contactos"] != null) { Aseguradora.P_Contactos = (DataTable)Session["Dt_Contactos"]; }
                        if (Session["Dt_Domicilios"] != null) { Aseguradora.P_Domicilios = (DataTable)Session["Dt_Domicilios"]; }
                        if (Session["Dt_Bancos"] != null) { Aseguradora.P_Bancos = (DataTable)Session["Dt_Bancos"]; }
                        Aseguradora.Alta_Aseguradora();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo_Generales();
                        Limpiar_SubCatalogos();
                        Llenar_Grid_Aseguradoras(Grid_Aseguradoras_Generales.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Aseguradoras", "alert('Alta de Aseguradora Exitosa');", true);
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Grid_Aseguradora_Contacto.Enabled = true;
                        Tab_Contenedor_Pestagnas.ActiveTabIndex = 0;
                        Btn_Modificar_Contacto.AlternateText = "Modificar";
                        Btn_Modificar_Domicilio.AlternateText = "Modificar";
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
        ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de una Aseguradora.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 25/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Modificar_Click(object sender, EventArgs e){
            try{
                if (Btn_Modificar.AlternateText.Equals("Modificar")){
                    if (Grid_Aseguradoras_Generales.Rows.Count > 0 && Grid_Aseguradoras_Generales.SelectedIndex > (-1)){
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
                        Cls_Cat_Pat_Com_Aseguradoras_Negocio Aseguradora = new Cls_Cat_Pat_Com_Aseguradoras_Negocio();
                        Aseguradora.P_Aseguradora_ID = Hdf_Aseguradora_ID.Value;
                        Aseguradora.P_Nombre = Txt_Nombre.Text.Trim();
                        Aseguradora.P_Nombre_Fiscal = Txt_Nombre_Fiscal.Text.Trim();
                        Aseguradora.P_Nombre_Comercial = Txt_Nombre_Comercial.Text.Trim();
                        Aseguradora.P_RFC = Txt_RFC.Text.Trim();
                        Aseguradora.P_Cuenta_Contable = Txt_Cuenta_Contable.Text.Trim();
                        Aseguradora.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        Aseguradora.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                        if (Session["Dt_Contactos"] != null) { Aseguradora.P_Contactos = (DataTable)Session["Dt_Contactos"]; }
                        if (Session["Dt_Domicilios"] != null) { Aseguradora.P_Domicilios = (DataTable)Session["Dt_Domicilios"]; }
                        if (Session["Dt_Bancos"] != null) { Aseguradora.P_Bancos = (DataTable)Session["Dt_Bancos"]; }
                        Aseguradora.Modificar_Aseguradora();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo_Generales();
                        Limpiar_SubCatalogos();
                        Llenar_Grid_Aseguradoras(Grid_Aseguradoras_Generales.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Aseguradoras", "alert('Actualización de Aseguradora Exitosa');", true);
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Grid_Aseguradora_Contacto.Enabled = true;
                        Tab_Contenedor_Pestagnas.ActiveTabIndex = 0;
                        Btn_Modificar_Contacto.AlternateText = "Modificar";
                        Btn_Modificar_Domicilio.AlternateText = "Modificar";
                        Btn_Modificar_Banco.AlternateText = "Modificar";
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
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 25/Noviembre/2010
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e){
            try {
                Limpiar_Catalogo_Generales();
                Limpiar_SubCatalogos();
                Grid_Aseguradoras_Generales.SelectedIndex = (-1);
                Grid_Aseguradora_Contacto.SelectedIndex = (-1);
                Grid_Aseguradora_Domicilio.SelectedIndex = (-1);
                Grid_Aseguradora_Bancos.SelectedIndex = (-1);
                Llenar_Grid_Aseguradoras(0);
                if (Grid_Aseguradoras_Generales.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0) {
                    Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el Nombre ó RFC \"" + Txt_Busqueda.Text + "\" no se encotrarón coincidencias";
                    Lbl_Mensaje_Error.Text = "(Se cargarón todas las Aseguradoras almacenadas)";
                    Div_Contenedor_Msj_Error.Visible = true;
                    Txt_Busqueda.Text = "";
                    Llenar_Grid_Aseguradoras(0);
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
        ///DESCRIPCIÓN: Elimina una Aseguradora de la Base de Datos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 25/Noviembre/2010
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Eliminar_Click(object sender, EventArgs e){
            try{
                if (Grid_Aseguradoras_Generales.Rows.Count > 0 && Grid_Aseguradoras_Generales.SelectedIndex > (-1)){
                    Cls_Cat_Pat_Com_Aseguradoras_Negocio Aseguradora = new Cls_Cat_Pat_Com_Aseguradoras_Negocio();
                    Aseguradora.P_Aseguradora_ID = Grid_Aseguradoras_Generales.SelectedRow.Cells[1].Text;
                    Aseguradora.Eliminar_Aseguradora();
                    Grid_Aseguradoras_Generales.SelectedIndex = (-1);
                    Llenar_Grid_Aseguradoras(Grid_Aseguradoras_Generales.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Aseguradoras", "alert('La Aseguradora fue eliminada exitosamente');", true);
                    Tab_Contenedor_Pestagnas.TabIndex = 0;
                    Limpiar_Catalogo_Generales();
                    Limpiar_SubCatalogos();
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
        ///FECHA_CREO: 25/Noviembre/2010
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Salir_Click(object sender, EventArgs e){
            try{
                if (Btn_Salir.AlternateText.Equals("Salir")){
                    Limpiar_SubCatalogos();
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }else {
                    Configuracion_Formulario(true);
                    Limpiar_Catalogo_Generales();
                    Limpiar_SubCatalogos();
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Modificar_Contacto.AlternateText = "Modificar";
                    Btn_Modificar_Domicilio.AlternateText = "Modificar";
                    Btn_Modificar_Banco.AlternateText = "Modificar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Grid_Aseguradora_Contacto.Enabled = true;
                    Tab_Contenedor_Pestagnas.TabIndex = 0;
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        #region Contactos

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Contacto_Click
            ///DESCRIPCIÓN: Agrega un nuevo contacto a la tabla de Contactos(Solo en Interfaz 
            ///             no en la Base de Datos)
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 25/Noviembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Btn_Agregar_Contacto_Click(object sender, EventArgs e) {
                try{
                    if (Validar_Componentes_Contactos()){
                        DataTable Tabla = (DataTable)Grid_Aseguradora_Contacto.DataSource;
                        if (Tabla == null){
                            if (Session["Dt_Contactos"] == null){
                                Tabla = new DataTable("Contactos");
                                Tabla.Columns.Add("ASEGURADORA_CONTACTO_ID", Type.GetType("System.String"));
                                Tabla.Columns.Add("DATO_CONTACTO", Type.GetType("System.String"));
                                Tabla.Columns.Add("DESCRIPCION", Type.GetType("System.String"));
                                Tabla.Columns.Add("REGISTRADO", Type.GetType("System.String"));
                                Tabla.Columns.Add("TELEFONO", Type.GetType("System.String"));
                                Tabla.Columns.Add("FAX", Type.GetType("System.String"));
                                Tabla.Columns.Add("CELULAR", Type.GetType("System.String"));
                                Tabla.Columns.Add("CORREO_ELECTRONICO", Type.GetType("System.String"));
                                Tabla.Columns.Add("ESTATUS", Type.GetType("System.String"));
                            } else {
                                Tabla = (DataTable)Session["Dt_Contactos"];
                            }
                        }
                        DataRow Fila_Contacto = Tabla.NewRow();
                        Fila_Contacto["ASEGURADORA_CONTACTO_ID"] = HttpUtility.HtmlDecode("");
                        Fila_Contacto["DATO_CONTACTO"] = HttpUtility.HtmlDecode(Txt_Dato_Contacto.Text.Trim());
                        Fila_Contacto["DESCRIPCION"] = HttpUtility.HtmlDecode(Txt_Descripcion_Contacto.Text.Trim());
                        Fila_Contacto["REGISTRADO"] = (Txt_Telefono_Contacto.Text.Trim().Length>0)?"S":"N";
                        Fila_Contacto["TELEFONO"] = HttpUtility.HtmlDecode(Txt_Telefono_Contacto.Text.Trim());
                        Fila_Contacto["FAX"] = HttpUtility.HtmlDecode(Txt_Fax_Contacto.Text.Trim());
                        Fila_Contacto["CELULAR"] = HttpUtility.HtmlDecode(Txt_Celular_Contacto.Text.Trim());
                        Fila_Contacto["CORREO_ELECTRONICO"] = HttpUtility.HtmlDecode(Txt_Correo_Electronico_Contacto.Text.Trim());
                        Fila_Contacto["ESTATUS"] = HttpUtility.HtmlDecode(Cmb_Estatus_Contacto.SelectedItem.Value.Trim());
                        Tabla.Rows.Add(Fila_Contacto);
                        Session["Dt_Contactos"] = Tabla;
                        Grid_Aseguradora_Contacto.DataSource = Tabla;
                        Grid_Aseguradora_Contacto.DataBind();
                        Grid_Aseguradora_Contacto.SelectedIndex = (-1);
                        Limpiar_Catalogo_Contactos();
                    }
                } catch(Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;                
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Contacto_Click
            ///DESCRIPCIÓN: Modifica un Contacto a la tabla de Aseguradoras Contactos (Solo
            ///             en Interfaz no en la Base de Datos)
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 25/Noviembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Btn_Modificar_Contacto_Click(object sender, EventArgs e) {
                try{
                    if (Btn_Modificar_Contacto.AlternateText.Equals("Modificar")){
                        if (Grid_Aseguradora_Contacto.Rows.Count > 0 && Grid_Aseguradora_Contacto.SelectedIndex > (-1)){
                            Int32 Registro = ((Grid_Aseguradora_Contacto.PageIndex) * Grid_Aseguradora_Contacto.PageSize) + (Grid_Aseguradora_Contacto.SelectedIndex);
                            if (Session["Dt_Contactos"] != null) { 
                                DataTable Contactos = (DataTable) Session["Dt_Contactos"];
                                Hdf_Aseguradora_Contacto_ID.Value = Contactos.Rows[Registro][0].ToString().Trim();
                                Txt_Aseguradora_Contacto_ID.Text = Contactos.Rows[Registro][0].ToString().Trim();
                                Txt_Dato_Contacto.Text = Contactos.Rows[Registro][1].ToString().Trim();
                                Txt_Descripcion_Contacto.Text = Contactos.Rows[Registro][2].ToString().Trim();
                                Txt_Telefono_Contacto.Text = Contactos.Rows[Registro][4].ToString().Trim();
                                Txt_Fax_Contacto.Text = Contactos.Rows[Registro][5].ToString().Trim();
                                Txt_Celular_Contacto.Text = Contactos.Rows[Registro][6].ToString().Trim();
                                Txt_Correo_Electronico_Contacto.Text = Contactos.Rows[Registro][7].ToString().Trim();
                                Cmb_Estatus_Contacto.SelectedIndex = Cmb_Estatus_Contacto.Items.IndexOf(Cmb_Estatus_Contacto.Items.FindByValue(Contactos.Rows[Registro][8].ToString().Trim()));
                            }
                            Btn_Modificar_Contacto.AlternateText = "Actualizar";
                            Btn_Quitar_Contacto.Visible = false;
                            Btn_Agregar_Contacto.Visible = false;
                            Grid_Aseguradora_Contacto.Enabled = false;
                        }else{
                            Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Modificar.";
                            Lbl_Mensaje_Error.Text = "";
                            Div_Contenedor_Msj_Error.Visible = true;
                        }
                    }else {
                        if (Validar_Componentes_Contactos()){
                            Int32 Registro = ((Grid_Aseguradora_Contacto.PageIndex) * Grid_Aseguradora_Contacto.PageSize) + (Grid_Aseguradora_Contacto.SelectedIndex);
                            if (Session["Dt_Contactos"] != null){
                                DataTable Tabla = (DataTable)Session["Dt_Contactos"];
                                Tabla.DefaultView.AllowEdit = true;
                                Tabla.Rows[Registro].BeginEdit();
                                Tabla.Rows[Registro][1] = Txt_Dato_Contacto.Text.Trim();
                                Tabla.Rows[Registro][2] = Txt_Descripcion_Contacto.Text.Trim();
                                Tabla.Rows[Registro][3] = (Txt_Telefono_Contacto.Text.Trim().Length > 0) ? "S" : "N";
                                Tabla.Rows[Registro][4] = Txt_Telefono_Contacto.Text.Trim();
                                Tabla.Rows[Registro][5] = Txt_Fax_Contacto.Text.Trim();
                                Tabla.Rows[Registro][6] = Txt_Celular_Contacto.Text.Trim();
                                Tabla.Rows[Registro][7] = Txt_Correo_Electronico_Contacto.Text.Trim();
                                Tabla.Rows[Registro][8] = Cmb_Estatus_Contacto.SelectedItem.Value.Trim();
                                Tabla.Rows[Registro].EndEdit();
                                Session["Dt_Contactos"] = Tabla;
                                Llenar_Grid_Contactos(Grid_Aseguradora_Contacto.PageIndex, Tabla);
                                Grid_Aseguradora_Contacto.SelectedIndex = (-1);
                                Btn_Modificar_Contacto.AlternateText = "Modificar";
                                Btn_Quitar_Contacto.Visible = true;
                                Btn_Agregar_Contacto.Visible = true;
                                Tab_Contenedor_Pestagnas.TabIndex = 0;
                                Grid_Aseguradora_Contacto.Enabled = true;
                                Limpiar_Catalogo_Contactos();
                            } 
                        }  
                    }
                }catch(Exception Ex){
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;                
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Quitar_Contacto_Click
            ///DESCRIPCIÓN: Quita un contacto a la tabla de Aseguradora Contactos (Solo en 
            ///             Interfaz no en la Base de Datos)
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 25/Noviembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Btn_Quitar_Contacto_Click(object sender, EventArgs e) {
                try{
                    if (Grid_Aseguradora_Contacto.Rows.Count > 0 && Grid_Aseguradora_Contacto.SelectedIndex > (-1)){
                        Int32 Registro = ((Grid_Aseguradora_Contacto.PageIndex) * Grid_Aseguradora_Contacto.PageSize) + (Grid_Aseguradora_Contacto.SelectedIndex);
                        if (Session["Dt_Contactos"] != null){
                            DataTable Tabla = (DataTable)Session["Dt_Contactos"];
                            Tabla.Rows.RemoveAt(Registro);
                            Session["Dt_Contactos"] = Tabla;
                            Grid_Aseguradora_Contacto.SelectedIndex = (-1);
                            Llenar_Grid_Contactos(Grid_Aseguradora_Contacto.PageIndex, Tabla);
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

        #region Domicilios

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Domicilio_Click
            ///DESCRIPCIÓN: Agrega un Nuevo Domicilio a la tabla de Domicilios(Solo en Interfaz 
            ///             no en la Base de Datos)
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 25/Noviembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Btn_Agregar_Domicilio_Click(object sender, ImageClickEventArgs e){
                try {
                    if (Validar_Componentes_Domicilios()) {
                        DataTable Tabla = (DataTable)Grid_Aseguradora_Domicilio.DataSource;
                        if (Tabla == null) {
                            if (Session["Dt_Domicilios"] == null) {
                                Tabla = new DataTable("Domicilios");
                                Tabla.Columns.Add("ASEGURADORA_DOMICILIO_ID", Type.GetType("System.String"));
                                Tabla.Columns.Add("DOMICILIO", Type.GetType("System.String"));
                                Tabla.Columns.Add("DESCRIPCION", Type.GetType("System.String"));
                                Tabla.Columns.Add("REGISTRADO", Type.GetType("System.String"));
                                Tabla.Columns.Add("CALLE", Type.GetType("System.String"));
                                Tabla.Columns.Add("NUMERO_EXTERIOR", Type.GetType("System.String"));
                                Tabla.Columns.Add("NUMERO_INTERIOR", Type.GetType("System.String"));
                                Tabla.Columns.Add("FAX", Type.GetType("System.String"));
                                Tabla.Columns.Add("COLONIA", Type.GetType("System.String"));
                                Tabla.Columns.Add("CODIGO_POSTAL", Type.GetType("System.String"));
                                Tabla.Columns.Add("CUIDAD", Type.GetType("System.String"));
                                Tabla.Columns.Add("MUNICIPIO", Type.GetType("System.String"));
                                Tabla.Columns.Add("ESTADO", Type.GetType("System.String"));
                                Tabla.Columns.Add("PAIS", Type.GetType("System.String"));
                                Tabla.Columns.Add("ESTATUS", Type.GetType("System.String"));
                            } else {
                                Tabla = (DataTable)Session["Dt_Domicilios"];
                            }
                        }
                        DataRow Fila_Domicilio = Tabla.NewRow();
                        Fila_Domicilio["ASEGURADORA_DOMICILIO_ID"] = HttpUtility.HtmlDecode("");
                        Fila_Domicilio["DOMICILIO"] = HttpUtility.HtmlDecode(Txt_Nombre_Domicilio.Text.Trim());
                        Fila_Domicilio["DESCRIPCION"] = HttpUtility.HtmlDecode(Txt_Descripcion_Domicilio.Text.Trim());
                        Fila_Domicilio["REGISTRADO"] = (Txt_Calle_Domicilio.Text.Trim().Length > 0) ? "S" : "N";
                        Fila_Domicilio["CALLE"] = HttpUtility.HtmlDecode(Txt_Calle_Domicilio.Text.Trim());
                        Fila_Domicilio["NUMERO_EXTERIOR"] = HttpUtility.HtmlDecode(Txt_Numero_Exterior.Text.Trim());
                        Fila_Domicilio["NUMERO_INTERIOR"] = HttpUtility.HtmlDecode(Txt_Numero_Interior.Text.Trim());
                        Fila_Domicilio["FAX"] = HttpUtility.HtmlDecode(Txt_Fax_Domicilio.Text.Trim());
                        Fila_Domicilio["COLONIA"] = HttpUtility.HtmlDecode(Txt_Colonia_Domicilio.Text.Trim());
                        Fila_Domicilio["CODIGO_POSTAL"] = (Txt_Codigo_Postal_Domicilio.Text.Trim().Length>0)?HttpUtility.HtmlDecode(Txt_Codigo_Postal_Domicilio.Text.Trim()):"0";
                        Fila_Domicilio["CUIDAD"] = HttpUtility.HtmlDecode(Txt_Ciudad_Domicilio.Text.Trim());
                        Fila_Domicilio["MUNICIPIO"] = HttpUtility.HtmlDecode(Txt_Municipio_Domicilio.Text.Trim());
                        Fila_Domicilio["ESTADO"] = HttpUtility.HtmlDecode(Txt_Estado_Domicilio.Text.Trim());
                        Fila_Domicilio["PAIS"] = HttpUtility.HtmlDecode(Txt_Pais_Domicilio.Text.Trim());
                        Fila_Domicilio["ESTATUS"] = HttpUtility.HtmlDecode(Cmb_Estatus_Domicilio.SelectedItem.Value.Trim());
                        Tabla.Rows.Add(Fila_Domicilio);
                        Session["Dt_Domicilios"] = Tabla;
                        Grid_Aseguradora_Domicilio.DataSource = Tabla;
                        Grid_Aseguradora_Domicilio.DataBind();
                        Grid_Aseguradora_Domicilio.SelectedIndex = (-1);
                        Limpiar_Catalogo_Domicilios();
                    }
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Domicilio_Click
            ///DESCRIPCIÓN: Modifica un Domicilio a la tabla de Aseguradoras Domicilios (Solo
            ///             en Interfaz no en la Base de Datos)
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 25/Noviembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Btn_Modificar_Domicilio_Click(object sender, ImageClickEventArgs e){
                try {
                    if (Btn_Modificar_Domicilio.AlternateText.Equals("Modificar")) {
                        if (Grid_Aseguradora_Domicilio.Rows.Count > 0 && Grid_Aseguradora_Domicilio.SelectedIndex > (-1)) {
                            Int32 Registro = ((Grid_Aseguradora_Domicilio.PageIndex) * Grid_Aseguradora_Domicilio.PageSize) + (Grid_Aseguradora_Domicilio.SelectedIndex);
                            if (Session["Dt_Domicilios"] != null) {
                                DataTable Domicilios = (DataTable)Session["Dt_Domicilios"];
                                Hdf_Aseguradora_Domicilio_ID.Value = Domicilios.Rows[Registro][0].ToString().Trim();
                                Txt_Aseguradora_Domicilio_ID.Text = Domicilios.Rows[Registro][0].ToString().Trim();
                                Txt_Nombre_Domicilio.Text = Domicilios.Rows[Registro][1].ToString().Trim();
                                Txt_Descripcion_Domicilio.Text = Domicilios.Rows[Registro][2].ToString().Trim();
                                Txt_Calle_Domicilio.Text = Domicilios.Rows[Registro][4].ToString().Trim();
                                Txt_Numero_Exterior.Text = Domicilios.Rows[Registro][5].ToString().Trim();
                                Txt_Numero_Interior.Text = Domicilios.Rows[Registro][6].ToString().Trim();
                                Txt_Fax_Domicilio.Text = Domicilios.Rows[Registro][7].ToString().Trim();
                                Txt_Colonia_Domicilio.Text = Domicilios.Rows[Registro][8].ToString().Trim();
                                Txt_Codigo_Postal_Domicilio.Text = Domicilios.Rows[Registro][9].ToString().Trim();
                                Txt_Ciudad_Domicilio.Text = Domicilios.Rows[Registro][10].ToString().Trim();
                                Txt_Municipio_Domicilio.Text = Domicilios.Rows[Registro][11].ToString().Trim();
                                Txt_Estado_Domicilio.Text = Domicilios.Rows[Registro][12].ToString().Trim();
                                Txt_Pais_Domicilio.Text = Domicilios.Rows[Registro][13].ToString().Trim();
                                Cmb_Estatus_Domicilio.SelectedIndex = Cmb_Estatus_Domicilio.Items.IndexOf(Cmb_Estatus_Domicilio.Items.FindByValue(Domicilios.Rows[Registro][14].ToString().Trim()));
                            }
                            Btn_Modificar_Domicilio.AlternateText = "Actualizar";
                            Btn_Quitar_Domicilio.Visible = false;
                            Btn_Agregar_Domicilio.Visible = false;
                            Grid_Aseguradora_Domicilio.Enabled = false;
                        } else {
                            Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Modificar.";
                            Lbl_Mensaje_Error.Text = "";
                            Div_Contenedor_Msj_Error.Visible = true;
                        }
                    } else {
                        if (Validar_Componentes_Domicilios()) {
                            Int32 Registro = ((Grid_Aseguradora_Domicilio.PageIndex) * Grid_Aseguradora_Domicilio.PageSize) + (Grid_Aseguradora_Domicilio.SelectedIndex);
                            if (Session["Dt_Domicilios"] != null) {
                                DataTable Tabla = (DataTable)Session["Dt_Domicilios"];
                                Tabla.DefaultView.AllowEdit = true;
                                Tabla.Rows[Registro].BeginEdit();
                                Tabla.Rows[Registro][1] = Txt_Nombre_Domicilio.Text.Trim();
                                Tabla.Rows[Registro][2] = Txt_Descripcion_Domicilio.Text.Trim();
                                Tabla.Rows[Registro][3] = (Txt_Calle_Domicilio.Text.Trim().Length > 0) ? "S" : "N";
                                Tabla.Rows[Registro][4] = Txt_Calle_Domicilio.Text.Trim();
                                Tabla.Rows[Registro][5] = Txt_Numero_Exterior.Text.Trim();
                                Tabla.Rows[Registro][6] = Txt_Numero_Interior.Text.Trim();
                                Tabla.Rows[Registro][7] = Txt_Fax_Domicilio.Text.Trim();
                                Tabla.Rows[Registro][8] = Txt_Colonia_Domicilio.Text.Trim();
                                Tabla.Rows[Registro][9] = (Txt_Codigo_Postal_Domicilio.Text.Trim().Length > 0) ? HttpUtility.HtmlDecode(Txt_Codigo_Postal_Domicilio.Text.Trim()) : "0";
                                Tabla.Rows[Registro][10] = Txt_Ciudad_Domicilio.Text.Trim();
                                Tabla.Rows[Registro][11] = Txt_Municipio_Domicilio.Text.Trim();
                                Tabla.Rows[Registro][12] = Txt_Estado_Domicilio.Text.Trim();
                                Tabla.Rows[Registro][13] = Txt_Pais_Domicilio.Text.Trim();
                                Tabla.Rows[Registro][14] = Cmb_Estatus_Domicilio.SelectedItem.Value.Trim();
                                Tabla.Rows[Registro].EndEdit();
                                Session["Dt_Domicilios"] = Tabla;
                                Llenar_Grid_Domicilios(Grid_Aseguradora_Domicilio.PageIndex, Tabla);
                                Grid_Aseguradora_Domicilio.SelectedIndex = (-1);
                                Btn_Modificar_Domicilio.AlternateText = "Modificar";
                                Btn_Quitar_Domicilio.Visible = true;
                                Btn_Agregar_Domicilio.Visible = true;
                                Tab_Contenedor_Pestagnas.TabIndex = 0;
                                Grid_Aseguradora_Domicilio.Enabled = true;
                                Limpiar_Catalogo_Domicilios();
                            }
                        }
                    }
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Quitar_Domicilio_Click
            ///DESCRIPCIÓN: Quita un Domicilio a la tabla de Aseguradora Domicilio (Solo en 
            ///             Interfaz no en la Base de Datos)
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 25/Noviembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Btn_Quitar_Domicilio_Click(object sender, ImageClickEventArgs e){
                try {
                    if (Grid_Aseguradora_Domicilio.Rows.Count > 0 && Grid_Aseguradora_Domicilio.SelectedIndex > (-1)) {
                        Int32 Registro = ((Grid_Aseguradora_Domicilio.PageIndex) * Grid_Aseguradora_Domicilio.PageSize) + (Grid_Aseguradora_Domicilio.SelectedIndex);
                        if (Session["Dt_Domicilios"] != null) {
                            DataTable Tabla = (DataTable)Session["Dt_Domicilios"];
                            Tabla.Rows.RemoveAt(Registro);
                            Session["Dt_Domicilios"] = Tabla;
                            Grid_Aseguradora_Domicilio.SelectedIndex = (-1);
                            Llenar_Grid_Domicilios(Grid_Aseguradora_Domicilio.PageIndex, Tabla);
                        }
                    } else {
                        Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Quitar.";
                        Lbl_Mensaje_Error.Text = "";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

        #endregion

        #region Bancos

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Banco_Click
            ///DESCRIPCIÓN: Agrega un Nuevo Banco a la tabla de Bancos(Solo en Interfaz 
            ///             no en la Base de Datos)
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 26/Noviembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Btn_Agregar_Banco_Click(object sender, ImageClickEventArgs e) {
                try {
                    if (Validar_Componentes_Bancos()) {
                        DataTable Tabla = (DataTable)Grid_Aseguradora_Bancos.DataSource;
                        if (Tabla == null) {
                            if (Session["Dt_Bancos"] == null) {
                                Tabla = new DataTable("Bancos");
                                Tabla.Columns.Add("ASEGURADORA_BANCO_ID", Type.GetType("System.String"));
                                Tabla.Columns.Add("PRODUCTO_BANCARIO", Type.GetType("System.String"));
                                Tabla.Columns.Add("DESCRIPCION", Type.GetType("System.String"));
                                Tabla.Columns.Add("REGISTRADO", Type.GetType("System.String"));
                                Tabla.Columns.Add("INSTITUCION_BANCARIA", Type.GetType("System.String"));
                                Tabla.Columns.Add("CUENTA", Type.GetType("System.String"));
                                Tabla.Columns.Add("CLABE_INSTITUCION_BANCARIA", Type.GetType("System.String"));
                                Tabla.Columns.Add("CLABE_PLAZA", Type.GetType("System.String"));
                                Tabla.Columns.Add("CLABE_CUENTA", Type.GetType("System.String"));
                                Tabla.Columns.Add("CLABE_DIGITO_VERIFICADOR", Type.GetType("System.String"));
                                Tabla.Columns.Add("CLAVE_CIE", Type.GetType("System.String"));
                                Tabla.Columns.Add("NUMERO_TARJETA", Type.GetType("System.String"));
                                Tabla.Columns.Add("NUMERO_TARJETA_REVERSO", Type.GetType("System.String"));
                                Tabla.Columns.Add("FECHA_VIGENCIA", Type.GetType("System.DateTime"));
                                Tabla.Columns.Add("ESTATUS", Type.GetType("System.String"));
                            } else {
                                Tabla = (DataTable)Session["Dt_Bancos"];
                            }
                        }
                        DataRow Fila_Bancos = Tabla.NewRow();
                        Fila_Bancos["ASEGURADORA_BANCO_ID"] = HttpUtility.HtmlDecode("");
                        Fila_Bancos["PRODUCTO_BANCARIO"] = HttpUtility.HtmlDecode(Txt_Producto_Bancario.Text.Trim());
                        Fila_Bancos["DESCRIPCION"] = HttpUtility.HtmlDecode(Txt_Descripcion_Banco.Text.Trim());
                        Fila_Bancos["REGISTRADO"] = (Txt_Institucion_Bancaria.Text.Trim().Length > 0) ? "S" : "N";
                        Fila_Bancos["INSTITUCION_BANCARIA"] = HttpUtility.HtmlDecode(Txt_Institucion_Bancaria.Text.Trim());
                        Fila_Bancos["CUENTA"] = HttpUtility.HtmlDecode(Txt_Cuenta_Banco.Text.Trim());
                        Fila_Bancos["CLABE_INSTITUCION_BANCARIA"] = HttpUtility.HtmlDecode(Txt_Clabe_Institucion_Bancaria.Text.Trim());
                        Fila_Bancos["CLABE_PLAZA"] = HttpUtility.HtmlDecode(Txt_Clabe_Plaza.Text.Trim());
                        Fila_Bancos["CLABE_CUENTA"] = HttpUtility.HtmlDecode(Txt_Clabe_Cuenta.Text.Trim());
                        Fila_Bancos["CLABE_DIGITO_VERIFICADOR"] = HttpUtility.HtmlDecode(Txt_Clabe_Digito_Verificador.Text.Trim());
                        Fila_Bancos["CLAVE_CIE"] = HttpUtility.HtmlDecode(Txt_Clave_CIE.Text.Trim());
                        Fila_Bancos["NUMERO_TARJETA"] = HttpUtility.HtmlDecode(Txt_Numero_Tarjeta.Text.Trim());
                        Fila_Bancos["NUMERO_TARJETA_REVERSO"] = HttpUtility.HtmlDecode(Txt_Numero_Tarjeta_Reverso.Text.Trim());
                        if(Txt_Fecha_Vigencia_Banco.Text.Trim().Length>0){
                            Fila_Bancos["FECHA_VIGENCIA"] = Convert.ToDateTime(Txt_Fecha_Vigencia_Banco.Text.Trim());
                        }
                        Fila_Bancos["ESTATUS"] = HttpUtility.HtmlDecode(Cmb_Estatus_Banco.SelectedItem.Value.Trim());
                        Tabla.Rows.Add(Fila_Bancos);
                        Session["Dt_Bancos"] = Tabla;
                        Grid_Aseguradora_Bancos.DataSource = Tabla;
                        Grid_Aseguradora_Bancos.DataBind();
                        Grid_Aseguradora_Bancos.SelectedIndex = (-1);
                        Limpiar_Catalogo_Bancos();
                    }
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Banco_Click
            ///DESCRIPCIÓN: Modifica un Banco a la tabla de Aseguradoras Bancos (Solo
            ///             en Interfaz no en la Base de Datos)
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 26/Noviembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Btn_Modificar_Banco_Click(object sender, ImageClickEventArgs e) {
                try {
                    if (Btn_Modificar_Banco.AlternateText.Equals("Modificar")) {
                        if (Grid_Aseguradora_Bancos.Rows.Count > 0 && Grid_Aseguradora_Bancos.SelectedIndex > (-1)) {
                            Int32 Registro = ((Grid_Aseguradora_Bancos.PageIndex) * Grid_Aseguradora_Bancos.PageSize) + (Grid_Aseguradora_Bancos.SelectedIndex);
                            if (Session["Dt_Bancos"] != null) {
                                DataTable Bancos = (DataTable)Session["Dt_Bancos"];
                                Hdf_Aseguradora_Banco_ID.Value = Bancos.Rows[Registro][0].ToString().Trim();
                                Txt_Aseguradora_Banco_ID.Text = Bancos.Rows[Registro][0].ToString().Trim();
                                Txt_Producto_Bancario.Text = Bancos.Rows[Registro][1].ToString().Trim();
                                Txt_Descripcion_Banco.Text = Bancos.Rows[Registro][2].ToString().Trim();
                                Txt_Institucion_Bancaria.Text = Bancos.Rows[Registro][4].ToString().Trim();
                                Txt_Cuenta_Banco.Text = Bancos.Rows[Registro][5].ToString().Trim();
                                Txt_Clabe_Institucion_Bancaria.Text = Bancos.Rows[Registro][6].ToString().Trim();
                                Txt_Clabe_Plaza.Text = Bancos.Rows[Registro][7].ToString().Trim();
                                Txt_Clabe_Cuenta.Text = Bancos.Rows[Registro][8].ToString().Trim();
                                Txt_Clabe_Digito_Verificador.Text = Bancos.Rows[Registro][9].ToString().Trim();
                                Txt_Clave_CIE.Text = Bancos.Rows[Registro][10].ToString().Trim();
                                Txt_Numero_Tarjeta.Text = Bancos.Rows[Registro][11].ToString().Trim();
                                Txt_Numero_Tarjeta_Reverso.Text = Bancos.Rows[Registro][12].ToString().Trim();
                                if (Bancos.Rows[Registro][13] != null && Bancos.Rows[Registro][13].ToString().Trim().Length>0) { 
                                    Txt_Fecha_Vigencia_Banco.Text = String.Format("{0:dd/MMM/yyyy}", (DateTime)Bancos.Rows[Registro][13]);
                                }
                                Cmb_Estatus_Banco.SelectedIndex = Cmb_Estatus_Banco.Items.IndexOf(Cmb_Estatus_Banco.Items.FindByValue(Bancos.Rows[Registro][14].ToString().Trim()));
                            }
                            Btn_Modificar_Banco.AlternateText = "Actualizar";
                            Btn_Quitar_Banco.Visible = false;
                            Btn_Agregar_Banco.Visible = false;
                            Grid_Aseguradora_Bancos.Enabled = false;
                        } else {
                            Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Modificar.";
                            Lbl_Mensaje_Error.Text = "";
                            Div_Contenedor_Msj_Error.Visible = true;
                        }
                    } else {
                        if (Validar_Componentes_Bancos()) {
                            Int32 Registro = ((Grid_Aseguradora_Bancos.PageIndex) * Grid_Aseguradora_Bancos.PageSize) + (Grid_Aseguradora_Bancos.SelectedIndex);
                            if (Session["Dt_Bancos"] != null) {
                                DataTable Tabla = (DataTable)Session["Dt_Bancos"];
                                Tabla.DefaultView.AllowEdit = true;
                                Tabla.Rows[Registro].BeginEdit();
                                Tabla.Rows[Registro][1] = Txt_Producto_Bancario.Text.Trim();
                                Tabla.Rows[Registro][2] = Txt_Descripcion_Banco.Text.Trim();
                                Tabla.Rows[Registro][3] = (Txt_Institucion_Bancaria.Text.Trim().Length > 0) ? "S" : "N";
                                Tabla.Rows[Registro][4] = Txt_Institucion_Bancaria.Text.Trim();
                                Tabla.Rows[Registro][5] = Txt_Cuenta_Banco.Text.Trim();
                                Tabla.Rows[Registro][6] = Txt_Clabe_Institucion_Bancaria.Text.Trim();
                                Tabla.Rows[Registro][7] = Txt_Clabe_Plaza.Text.Trim();
                                Tabla.Rows[Registro][8] = Txt_Clabe_Cuenta.Text.Trim();
                                Tabla.Rows[Registro][9] = Txt_Clabe_Digito_Verificador.Text.Trim();
                                Tabla.Rows[Registro][10] = Txt_Clave_CIE.Text.Trim();
                                Tabla.Rows[Registro][11] = Txt_Numero_Tarjeta.Text.Trim();
                                Tabla.Rows[Registro][12] = Txt_Numero_Tarjeta_Reverso.Text.Trim();
                                if (Txt_Fecha_Vigencia_Banco.Text.Trim().Length > 0) {
                                    Tabla.Rows[Registro][13] = Convert.ToDateTime(Txt_Fecha_Vigencia_Banco.Text.Trim());
                                }
                                Tabla.Rows[Registro][14] = Cmb_Estatus_Banco.SelectedItem.Value.Trim();
                                Tabla.Rows[Registro].EndEdit();
                                Session["Dt_Bancos"] = Tabla;
                                Llenar_Grid_Bancos(Grid_Aseguradora_Bancos.PageIndex, Tabla);
                                Grid_Aseguradora_Bancos.SelectedIndex = (-1);
                                Btn_Modificar_Banco.AlternateText = "Modificar";
                                Btn_Quitar_Banco.Visible = true;
                                Btn_Agregar_Banco.Visible = true;
                                Tab_Contenedor_Pestagnas.TabIndex = 0;
                                Grid_Aseguradora_Bancos.Enabled = true;
                                Limpiar_Catalogo_Bancos();
                            }
                        }
                    }
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Quitar_Banco_Click
            ///DESCRIPCIÓN: Quita un Banco a la tabla de Aseguradora Banco (Solo en 
            ///             Interfaz no en la Base de Datos)
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 26/Noviembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************    
            protected void Btn_Quitar_Banco_Click(object sender, ImageClickEventArgs e) {
                try {
                    if (Grid_Aseguradora_Bancos.Rows.Count > 0 && Grid_Aseguradora_Bancos.SelectedIndex > (-1)) {
                        Int32 Registro = ((Grid_Aseguradora_Bancos.PageIndex) * Grid_Aseguradora_Bancos.PageSize) + (Grid_Aseguradora_Bancos.SelectedIndex);
                        if (Session["Dt_Bancos"] != null) {
                            DataTable Tabla = (DataTable)Session["Dt_Bancos"];
                            Tabla.Rows.RemoveAt(Registro);
                            Session["Dt_Bancos"] = Tabla;
                            Grid_Aseguradora_Bancos.SelectedIndex = (-1);
                            Llenar_Grid_Bancos(Grid_Aseguradora_Bancos.PageIndex, Tabla);
                        }
                    } else {
                        Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Quitar.";
                        Lbl_Mensaje_Error.Text = "";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

        #endregion

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
                                    Cls_Util.Configuracion_Acceso_Sistema_SIAS_AlternateText(Botones, Dr_Menus[0]);//Habilitamos la configuracón de los botones.
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
}