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
using Presidencia.Control_Patrimonial_Bienes_Sin_Inventario.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Bienes_Muebles.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Vehiculos.Negocio;
using Presidencia.Catalogo_Compras_Marcas.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Materiales.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Colores.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Tipos_Vehiculo.Negocio;
using Presidencia.Dependencias.Negocios;
using Presidencia.Constantes;
using Presidencia.Catalogo_Compras_Productos.Negocio;

public partial class paginas_Compras_Frm_Ope_Pat_Bienes_Sin_Inventario : System.Web.UI.Page {
    
    #region "Page Load"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Page_Load
        ///DESCRIPCIÓN          : Se carga por default en la pagina al iniciar.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 04/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Page_Load(object sender, EventArgs e){
            Div_Contenedor_Msj_Error.Visible = false;
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Trim().Length == 0) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!IsPostBack) {
                Llenar_Combos_Independientes();
                Llenar_Combos_Bienes_Muebles();
                Llenar_Combos_Vehiculos();
                Habilitacion_Componentes("");
                Cmb_Busqueda_BSI_Tipo_Parent_SelectedIndexChanged(Cmb_Busqueda_BSI_Tipo_Parent, null);
                Llenar_MPE_Listado_Productos(0);
            }
        }

    #endregion

    #region "Metodos"

        #region "Limpiar"

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Limpiar_Campos_Generales
            ///DESCRIPCIÓN          : Limpiar TODOS los campos en el Formulario.
            ///PARAMETROS           : 
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 04/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Limpiar_Campos_Generales() {
                Hdf_Bien_ID.Value = "";
                Hdf_Producto_ID.Value = "";
                Txt_Nombre_Bien.Text = "";
                Cmb_Tipo_Parent.SelectedIndex = 0;
                Cmb_Marca.SelectedIndex = 0;
                Txt_Modelo.Text = "";
                Cmb_Material.SelectedIndex = 0;
                Cmb_Color.SelectedIndex = 0;
                Txt_Costo.Text = "";
                Txt_Fecha_Adquisicion.Text = "";
                Txt_Numero_Serie.Text = "";
                Cmb_Estado.SelectedIndex = 0;
                Cmb_Estatus.SelectedIndex = 0;
                Txt_Comentarios.Text = "";
                Txt_Motivo_Baja.Text = "";
                Limpiar_Campos_Vehiculo_Parent();
                Limpiar_Campos_Bien_Mueble_Parent();
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Limpiar_Campos_Vehiculo_Parent
            ///DESCRIPCIÓN          : Limpia los campos de la capa de Vehiculo.
            ///PARAMETROS           :
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 04/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Limpiar_Campos_Vehiculo_Parent() {
                Hdf_Bien_Parent_ID.Value = "";
                Txt_Vehiculo_Nombre.Text = "";
                Txt_Vehiculo_Dependencia.Text = "";
                Txt_Vehiculo_No_Inventario.Text = "";
                Txt_Vehiculo_Numero_Serie.Text = "";
                Txt_Vehiculo_Marca.Text = "";
                Txt_Vehiculo_Tipo.Text = "";
                Txt_Vehiculo_Color.Text = "";
                Txt_Vehiculo_Modelo.Text = "";
                Txt_Vehiculo_Numero_Economico.Text = "";
                Txt_Vehiculo_Placas.Text = "";
                Grid_Listado_Resguardantes.DataSource = new DataTable();
                Grid_Listado_Resguardantes.DataBind();
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Limpiar_Campos_Vehiculo_Parent
            ///DESCRIPCIÓN          : Limpia los campos de la capa de Bien Mueble.
            ///PARAMETROS           : 
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 04/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Limpiar_Campos_Bien_Mueble_Parent() {
                Hdf_Bien_Parent_ID.Value = "";
                Txt_Bien_Mueble_Nombre.Text = "";
                Txt_Bien_Mueble_Dependencia.Text = "";
                Txt_Bien_Mueble_Inventario_Anterior.Text = "";
                Txt_Bien_Mueble_Inventario_SIAS.Text = "";
                Txt_Bien_Mueble_Numero_Serie.Text = "";
                Txt_Bien_Mueble_Marca.Text = "";
                Txt_Bien_Mueble_Modelo.Text = "";
                Txt_Bien_Mueble_Material.Text = "";
                Txt_Bien_Mueble_Color.Text = "";
                Grid_Listado_Resguardantes.DataSource = new DataTable();
                Grid_Listado_Resguardantes.DataBind();
            }

        #endregion

        #region "Interaccion con Base de Datos [Mostrar Datos y Llenado de Componentes]"
    
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Llenar_Combos_Independientes
            ///DESCRIPCIÓN          : Llena los Combos en el Formulario.
            ///PARAMETROS           : 
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 04/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Llenar_Combos_Independientes(){
                Cls_Ope_Pat_Bienes_Sin_Inv_Negocio Negocio = new Cls_Ope_Pat_Bienes_Sin_Inv_Negocio();
                
                //Combo de Marcas
                Negocio.P_Tipo_DataTable = "MARCAS";
                DataTable Dt_Marcas = Negocio.Consultar_DataTable();
                Cmb_Marca.DataSource = Dt_Marcas;
                Cmb_Marca.DataTextField = "TEXTO";
                Cmb_Marca.DataValueField = "IDENTIFICADOR";
                Cmb_Marca.DataBind();
                Cmb_Marca.Items.Insert(0, new ListItem("< SELECCIONE >", ""));
                Cmb_Busqueda_BSI_Marca.DataSource = Dt_Marcas;
                Cmb_Busqueda_BSI_Marca.DataTextField = "TEXTO";
                Cmb_Busqueda_BSI_Marca.DataValueField = "IDENTIFICADOR";
                Cmb_Busqueda_BSI_Marca.DataBind();
                Cmb_Busqueda_BSI_Marca.Items.Insert(0, new ListItem("< TODAS >", ""));

                //Combo de Materiales
                Negocio.P_Tipo_DataTable = "MATERIALES";
                DataTable Dt_Materiales = Negocio.Consultar_DataTable();
                Cmb_Material.DataSource = Dt_Materiales;
                Cmb_Material.DataTextField = "TEXTO";
                Cmb_Material.DataValueField = "IDENTIFICADOR";
                Cmb_Material.DataBind();
                Cmb_Material.Items.Insert(0, new ListItem("< SELECCIONE >", ""));
                Cmb_Busqueda_BSI_Material.DataSource = Dt_Materiales;
                Cmb_Busqueda_BSI_Material.DataTextField = "TEXTO";
                Cmb_Busqueda_BSI_Material.DataValueField = "IDENTIFICADOR";
                Cmb_Busqueda_BSI_Material.DataBind();
                Cmb_Busqueda_BSI_Material.Items.Insert(0, new ListItem("< TODAS >", ""));

                //Combo de Colores
                Negocio.P_Tipo_DataTable = "COLORES";
                DataTable Dt_Colores = Negocio.Consultar_DataTable();
                Cmb_Color.DataSource = Dt_Colores;
                Cmb_Color.DataTextField = "TEXTO";
                Cmb_Color.DataValueField = "IDENTIFICADOR";
                Cmb_Color.DataBind();
                Cmb_Color.Items.Insert(0, new ListItem("< SELECCIONE >", ""));
                Cmb_Busqueda_BSI_Color.DataSource = Dt_Colores;
                Cmb_Busqueda_BSI_Color.DataTextField = "TEXTO";
                Cmb_Busqueda_BSI_Color.DataValueField = "IDENTIFICADOR";
                Cmb_Busqueda_BSI_Color.DataBind();
                Cmb_Busqueda_BSI_Color.Items.Insert(0, new ListItem("< TODOS >", ""));

                //Combo de Dependencias
                Negocio.P_Tipo_DataTable = "DEPENDENCIAS";
                DataTable Dt_Dependencias = Negocio.Consultar_DataTable();
                Cmb_Busqueda_BSI_Dependencia_Resguardante.DataSource = Dt_Dependencias;
                Cmb_Busqueda_BSI_Dependencia_Resguardante.DataTextField = "TEXTO";
                Cmb_Busqueda_BSI_Dependencia_Resguardante.DataValueField = "IDENTIFICADOR";
                Cmb_Busqueda_BSI_Dependencia_Resguardante.DataBind();
                Cmb_Busqueda_BSI_Dependencia_Resguardante.Items.Insert(0, new ListItem("< TODAS >", ""));

            }
                
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Llenar_Combos_Bienes_Muebles
            ///DESCRIPCIÓN          : Llena los Combos de Bienes Muebles en el Formulario.
            ///PARAMETROS           : 
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 22/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Llenar_Combos_Bienes_Muebles(){
                Cls_Ope_Pat_Com_Bienes_Muebles_Negocio BM_Negocio = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();

                //Llenar Combo de Dependencias
                BM_Negocio.P_Tipo_DataTable = "DEPENDENCIAS";
                DataTable Dt_Dependencias = BM_Negocio.Consultar_DataTable();
                Cmb_Busqueda_Bien_Mueble_Dependencias.DataSource = Dt_Dependencias;
                Cmb_Busqueda_Bien_Mueble_Dependencias.DataValueField = "DEPENDENCIA_ID";
                Cmb_Busqueda_Bien_Mueble_Dependencias.DataTextField = "NOMBRE";
                Cmb_Busqueda_Bien_Mueble_Dependencias.DataBind();
                Cmb_Busqueda_Bien_Mueble_Dependencias.Items.Insert(0, new ListItem("< TODAS >", ""));
                Cmb_Busqueda_Bien_Mueble_Resguardantes_Dependencias.DataSource = Dt_Dependencias;
                Cmb_Busqueda_Bien_Mueble_Resguardantes_Dependencias.DataValueField = "DEPENDENCIA_ID";
                Cmb_Busqueda_Bien_Mueble_Resguardantes_Dependencias.DataTextField = "NOMBRE";
                Cmb_Busqueda_Bien_Mueble_Resguardantes_Dependencias.DataBind();
                Cmb_Busqueda_Bien_Mueble_Resguardantes_Dependencias.Items.Insert(0, new ListItem("< TODAS >", ""));

                //Llenar Combo de Marcas
                BM_Negocio.P_Tipo_DataTable = "MARCAS";
                DataTable Dt_Marcas = BM_Negocio.Consultar_DataTable();
                Cmb_Busqueda_Bien_Mueble_Marca.DataSource = Dt_Marcas;
                Cmb_Busqueda_Bien_Mueble_Marca.DataValueField = "MARCA_ID";
                Cmb_Busqueda_Bien_Mueble_Marca.DataTextField = "NOMBRE";
                Cmb_Busqueda_Bien_Mueble_Marca.DataBind();
                Cmb_Busqueda_Bien_Mueble_Marca.Items.Insert(0, new ListItem("< TODAS >", ""));
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Llenar_Combos_Vehiculos
            ///DESCRIPCIÓN          : Llena los Combos de Vehículos en el Formulario.
            ///PARAMETROS           : 
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 22/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Llenar_Combos_Vehiculos() {
                Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculos_Neogcio = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
                
                //Se llena el Combo de Marcas
                Vehiculos_Neogcio.P_Tipo_DataTable = "MARCAS";
                DataTable Dt_Marcas = Vehiculos_Neogcio.Consultar_DataTable();
                Cmb_Busqueda_Vehiculo_Marca.DataSource = Dt_Marcas;
                Cmb_Busqueda_Vehiculo_Marca.DataValueField = "MARCA_ID";
                Cmb_Busqueda_Vehiculo_Marca.DataTextField = "NOMBRE";
                Cmb_Busqueda_Vehiculo_Marca.DataBind();
                Cmb_Busqueda_Vehiculo_Marca.Items.Insert(0, new ListItem("< TODAS >", ""));

                //Se llena el Combo de Tipo de Vehículo
                Vehiculos_Neogcio.P_Tipo_DataTable = "TIPOS_VEHICULOS";
                DataTable Dt_Tipos_Vehiculos = Vehiculos_Neogcio.Consultar_DataTable();
                Cmb_Busqueda_Vehiculo_Tipo_Vehiculo.DataSource = Dt_Tipos_Vehiculos;
                Cmb_Busqueda_Vehiculo_Tipo_Vehiculo.DataValueField = "TIPO_VEHICULO_ID";
                Cmb_Busqueda_Vehiculo_Tipo_Vehiculo.DataTextField = "DESCRIPCION";
                Cmb_Busqueda_Vehiculo_Tipo_Vehiculo.DataBind();
                Cmb_Busqueda_Vehiculo_Tipo_Vehiculo.Items.Insert(0, new ListItem("< TODOS >", ""));


                //Se llena el Combo de Tipos de Combustible
                Vehiculos_Neogcio.P_Tipo_DataTable = "TIPOS_COMBUSTIBLE";
                DataTable Dt_Combustibles = Vehiculos_Neogcio.Consultar_DataTable();
                Cmb_Busqueda_Vehiculo_Tipo_Combustible.DataSource = Dt_Combustibles;
                Cmb_Busqueda_Vehiculo_Tipo_Combustible.DataValueField = "TIPO_COMBUSTIBLE_ID";
                Cmb_Busqueda_Vehiculo_Tipo_Combustible.DataTextField = "DESCRIPCION";
                Cmb_Busqueda_Vehiculo_Tipo_Combustible.DataBind();
                Cmb_Busqueda_Vehiculo_Tipo_Combustible.Items.Insert(0, new ListItem("< TODOS >", ""));

                //Se llena el Combo de Colores
                Vehiculos_Neogcio.P_Tipo_DataTable = "COLORES";
                DataTable Dt_Colores = Vehiculos_Neogcio.Consultar_DataTable();
                Cmb_Busqueda_Vehiculo_Color.DataSource = Dt_Colores;
                Cmb_Busqueda_Vehiculo_Color.DataValueField = "COLOR_ID";
                Cmb_Busqueda_Vehiculo_Color.DataTextField = "DESCRIPCION";
                Cmb_Busqueda_Vehiculo_Color.DataBind();
                Cmb_Busqueda_Vehiculo_Color.Items.Insert(0, new ListItem("< TODOS >", ""));

                //Se llena el Combo de Zonas
                Vehiculos_Neogcio.P_Tipo_DataTable = "ZONAS";
                DataTable Dt_Zonas= Vehiculos_Neogcio.Consultar_DataTable();
                Cmb_Busqueda_Vehiculo_Zonas.DataSource = Dt_Zonas;
                Cmb_Busqueda_Vehiculo_Zonas.DataValueField = "ZONA_ID";
                Cmb_Busqueda_Vehiculo_Zonas.DataTextField = "DESCRIPCION";
                Cmb_Busqueda_Vehiculo_Zonas.DataBind();
                Cmb_Busqueda_Vehiculo_Zonas.Items.Insert(0, new ListItem("< TODAS >", ""));


                //Se llena el Combo de Dependencias
                Vehiculos_Neogcio.P_Tipo_DataTable = "DEPENDENCIAS";
                DataTable Dt_Dependencias = Vehiculos_Neogcio.Consultar_DataTable();
                Cmb_Busqueda_Vehiculo_Dependencias.DataSource = Dt_Dependencias;
                Cmb_Busqueda_Vehiculo_Dependencias.DataValueField = "DEPENDENCIA_ID";
                Cmb_Busqueda_Vehiculo_Dependencias.DataTextField = "NOMBRE";
                Cmb_Busqueda_Vehiculo_Dependencias.DataBind();
                Cmb_Busqueda_Vehiculo_Dependencias.Items.Insert(0, new ListItem("< TODAS >", ""));
                Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias.DataSource = Dt_Dependencias;
                Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias.DataValueField = "DEPENDENCIA_ID";
                Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias.DataTextField = "NOMBRE";
                Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias.DataBind();
                Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias.Items.Insert(0, new ListItem("< TODAS >", ""));
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Llenar_Combo_Empleados
            ///DESCRIPCIÓN          : Llena el Combo con los empleados de una dependencia.
            ///PARAMETROS           : 
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 22/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Llenar_Combo_Empleados(String Dependencia_ID, ref DropDownList Combo_Empleados) {
                Combo_Empleados.Items.Clear();
                if (Dependencia_ID != null && Dependencia_ID.Trim().Length > 0) {
                    Cls_Ope_Pat_Bienes_Sin_Inv_Negocio BSI_Negocio = new Cls_Ope_Pat_Bienes_Sin_Inv_Negocio();
                    BSI_Negocio.P_Tipo_DataTable = "EMPLEADOS";
                    BSI_Negocio.P_Dependencia_ID = Dependencia_ID.Trim();
                    DataTable Dt_Datos = BSI_Negocio.Consultar_DataTable();
                    Combo_Empleados.DataSource = Dt_Datos;
                    Combo_Empleados.DataValueField = "EMPLEADO_ID";
                    Combo_Empleados.DataTextField = "EMPLEADO";
                    Combo_Empleados.DataBind();
                } 
                Combo_Empleados.Items.Insert(0, new ListItem("< TODOS >", ""));
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Llenar_Campos_Vehiculo_Parent
            ///DESCRIPCIÓN          : Llena los Campos de la capa de Vehiculos.
            ///PARAMETROS           : 
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 04/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Llenar_Campos_Vehiculo_Parent() {
                Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo_Negocio = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
                Vehiculo_Negocio.P_Vehiculo_ID = Hdf_Bien_Parent_ID.Value.Trim();
                Vehiculo_Negocio = Vehiculo_Negocio.Consultar_Detalles_Vehiculo();
                Limpiar_Campos_Vehiculo_Parent();
                if (Vehiculo_Negocio.P_Vehiculo_ID != null && Vehiculo_Negocio.P_Vehiculo_ID.Trim().Length > 0) {
                    Hdf_Bien_Parent_ID.Value = Vehiculo_Negocio.P_Vehiculo_ID;
                    Txt_Vehiculo_Nombre.Text = Vehiculo_Negocio.P_Nombre_Producto.Trim();
                    if (!String.IsNullOrEmpty(Vehiculo_Negocio.P_Dependencia_ID)) {
                        Txt_Vehiculo_Dependencia.Text = Obtener_Descripcion_Identificador("DEPENDENCIAS", Vehiculo_Negocio.P_Dependencia_ID);
                    }
                    Txt_Vehiculo_No_Inventario.Text = Vehiculo_Negocio.P_Numero_Inventario.ToString().Trim();
                    Txt_Vehiculo_Numero_Serie.Text = Vehiculo_Negocio.P_Serie_Carroceria.Trim();
                    if (!String.IsNullOrEmpty(Vehiculo_Negocio.P_Marca_ID)) {
                        Txt_Vehiculo_Marca.Text = Obtener_Descripcion_Identificador("MARCAS", Vehiculo_Negocio.P_Marca_ID);
                    }
                    if (!String.IsNullOrEmpty(Vehiculo_Negocio.P_Tipo_Vehiculo_ID)) {
                        Txt_Vehiculo_Tipo.Text = Obtener_Descripcion_Identificador("TIPOS_VEHICULO", Vehiculo_Negocio.P_Tipo_Vehiculo_ID);
                    }
                    if (!String.IsNullOrEmpty(Vehiculo_Negocio.P_Color_ID)) {
                        Txt_Vehiculo_Color.Text = Obtener_Descripcion_Identificador("COLORES", Vehiculo_Negocio.P_Color_ID);
                    }
                    Txt_Vehiculo_Modelo.Text = Vehiculo_Negocio.P_Modelo_ID.Trim();
                    Txt_Vehiculo_Numero_Economico.Text = Vehiculo_Negocio.P_Numero_Economico_.Trim();
                    Txt_Vehiculo_Placas.Text = Vehiculo_Negocio.P_Placas.Trim();
                    DataTable Dt_Resguardos = (Vehiculo_Negocio.P_Resguardantes != null && Vehiculo_Negocio.P_Resguardantes.Rows.Count > 0) ? Vehiculo_Negocio.P_Resguardantes : new DataTable();
                    Llenar_Grid_Resguardos(Dt_Resguardos);
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Llenar_Campos_Bien_Mueble_Parent
            ///DESCRIPCIÓN          : Llena los Campos de la capa de Bienes Muebles.
            ///PARAMETROS           : 
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 04/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Llenar_Campos_Bien_Mueble_Parent() {
                Cls_Ope_Pat_Com_Bienes_Muebles_Negocio BM_Negocio = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                BM_Negocio.P_Bien_Mueble_ID = Hdf_Bien_Parent_ID.Value.Trim();
                BM_Negocio = BM_Negocio.Consultar_Detalles_Bien_Mueble();
                Limpiar_Campos_Bien_Mueble_Parent();
                if (BM_Negocio.P_Bien_Mueble_ID != null && BM_Negocio.P_Bien_Mueble_ID.Trim().Length > 0) {
                    Hdf_Bien_Parent_ID.Value = BM_Negocio.P_Bien_Mueble_ID;
                    Txt_Bien_Mueble_Nombre.Text = BM_Negocio.P_Nombre_Producto;
                    if (!String.IsNullOrEmpty(BM_Negocio.P_Dependencia_ID)) {
                        Txt_Bien_Mueble_Dependencia.Text = Obtener_Descripcion_Identificador("DEPENDENCIAS", BM_Negocio.P_Dependencia_ID);
                    }
                    Txt_Bien_Mueble_Inventario_Anterior.Text = BM_Negocio.P_Numero_Inventario.Trim();
                    Txt_Bien_Mueble_Inventario_SIAS.Text = BM_Negocio.P_Numero_Inventario_Anterior.Trim();
                    Txt_Bien_Mueble_Numero_Serie.Text = BM_Negocio.P_Numero_Serie;
                    if (!String.IsNullOrEmpty(BM_Negocio.P_Marca_ID)) {
                        Txt_Bien_Mueble_Marca.Text = Obtener_Descripcion_Identificador("MARCAS", BM_Negocio.P_Marca_ID);
                    }
                    Txt_Bien_Mueble_Modelo.Text = BM_Negocio.P_Modelo;

                    if (!String.IsNullOrEmpty(BM_Negocio.P_Material_ID)) {
                        Txt_Bien_Mueble_Material.Text = Obtener_Descripcion_Identificador("MATERIALES", BM_Negocio.P_Material_ID);
                    }
                    if (!String.IsNullOrEmpty(BM_Negocio.P_Color_ID)) {
                        Txt_Bien_Mueble_Color.Text = Obtener_Descripcion_Identificador("COLORES", BM_Negocio.P_Color_ID);
                    }
                    DataTable Dt_Resguardos = (BM_Negocio.P_Resguardantes != null && BM_Negocio.P_Resguardantes.Rows.Count > 0) ? BM_Negocio.P_Resguardantes : new DataTable();
                    Llenar_Grid_Resguardos(Dt_Resguardos);
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Llenar_Grid_Resguardos
            ///DESCRIPCIÓN          : Llena el Grid de los Resguardos.
            ///PARAMETROS           : 1. Dt_Resguardos. Tabla de Resguardos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 04/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Llenar_Grid_Resguardos(DataTable Dt_Resguardos) {
                Grid_Listado_Resguardantes.Columns[0].Visible = true;
                Grid_Listado_Resguardantes.DataSource = Dt_Resguardos;
                Grid_Listado_Resguardantes.DataBind();
                Grid_Listado_Resguardantes.Columns[0].Visible = false;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Obtener_Descripcion_Identificador
            ///DESCRIPCIÓN          : Obtiene la Descripcion de un identificador.
            ///PARAMETROS           : 1.Tipo. Tipo de Busqueda.
            ///                       2.Identificador. Identificador a Buscar su Descripción.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 04/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private String Obtener_Descripcion_Identificador(String Tipo, String Identificador) {
                String Descripcion = null;
                Cls_Ope_Pat_Bienes_Sin_Inv_Negocio Negocio_Tmp = new Cls_Ope_Pat_Bienes_Sin_Inv_Negocio();
                Negocio_Tmp.P_Tipo_DataTable = Tipo;
                Negocio_Tmp.P_Identificador_Generico = Identificador;
                DataTable Dt_Datos = Negocio_Tmp.Consultar_DataTable();
                if (Dt_Datos != null && Dt_Datos.Rows.Count > 0) {
                    Descripcion = Dt_Datos.Rows[0]["TEXTO"].ToString();
                }
                return Descripcion;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Llenar_MPE_Listado_Productos
            ///DESCRIPCIÓN          : Llena el Grid de los Productos.
            ///PARAMETROS           : 
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 18/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Llenar_MPE_Listado_Productos(Int32 Pagina) {
                try {
                    Grid_Listado_Productos.SelectedIndex = (-1);
                    Grid_Listado_Productos.Columns[1].Visible = true;
                    Cls_Cat_Com_Productos_Negocio Productos_Negocio = new Cls_Cat_Com_Productos_Negocio();
                    if (Txt_Nombre_Producto_Buscar.Text.Trim() != "") {
                        Productos_Negocio.P_Nombre = Txt_Nombre_Producto_Buscar.Text.Trim();
                    }
                    Productos_Negocio.P_Estatus = "ACTIVO";
                    Productos_Negocio.P_Tipo = "BIEN_MUEBLE";
                    DataTable Dt_Productos = Productos_Negocio.Consulta_Datos_Producto();
                    Dt_Productos.Columns[Cat_Com_Productos.Campo_Producto_ID].ColumnName = "PRODUCTO_ID";
                    Dt_Productos.Columns[Cat_Com_Productos.Campo_Clave].ColumnName = "CLAVE_PRODUCTO";
                    Dt_Productos.Columns[Cat_Com_Productos.Campo_Nombre].ColumnName = "NOMBRE_PRODUCTO";
                    Dt_Productos.Columns[Cat_Com_Productos.Campo_Descripcion].ColumnName = "DESCRIPCION_PRODUCTO";
                    Grid_Listado_Productos.DataSource = Dt_Productos;
                    Grid_Listado_Productos.PageIndex = Pagina;
                    Grid_Listado_Productos.DataBind();
                    Grid_Listado_Productos.Columns[1].Visible = false;
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = "";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Llenar_Grid_Listado_Bienes_Bien_Mueble
            ///DESCRIPCIÓN          : Llena el Grid de los Bienes Muebles.
            ///PARAMETROS           : 
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 18/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Llenar_Grid_Listado_Bienes_Bien_Mueble(Int32 Pagina) { 
                try{
                    Grid_Listado_Bienes_Bien_Mueble.Columns[1].Visible = true;
                    Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bienes = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                    Bienes.P_Tipo_DataTable = "BIENES";
                    if (Session["FILTRO_BUSQUEDA"] != null) {
                        Bienes.P_Tipo_Filtro_Busqueda = Session["FILTRO_BUSQUEDA"].ToString();
                        if (Session["FILTRO_BUSQUEDA"].ToString().Trim().Equals("DATOS_GENERALES")) {
                            if (Txt_Busqueda_Bien_Mueble_Numero_Inventario.Text.Trim().Length > 0) {
                                Bienes.P_Numero_Inventario_Anterior = Txt_Busqueda_Bien_Mueble_Numero_Inventario.Text.Trim();
                            }
                            if (Txt_Busqueda_Bien_Mueble_Numero_Inventario_SIAS.Text.Trim().Length > 0) {
                                Bienes.P_Numero_Inventario = Txt_Busqueda_Bien_Mueble_Numero_Inventario_SIAS.Text.Trim();
                            }
                            if (Txt_Busqueda_Bien_Mueble_Producto.Text.Trim().Length > 0) {
                                Bienes.P_Nombre_Producto = Txt_Busqueda_Bien_Mueble_Producto.Text.Trim();
                            }
                            if (Cmb_Busqueda_Bien_Mueble_Dependencias.SelectedIndex > 0)
                            {
                                Bienes.P_Dependencia_ID = Cmb_Busqueda_Bien_Mueble_Dependencias.SelectedItem.Value;
                            }
                            if (Txt_Busqueda_Bien_Mueble_Modelo.Text.Trim().Length > 0) {
                                Bienes.P_Modelo = Txt_Busqueda_Bien_Mueble_Modelo.Text.Trim();
                            }
                            if (Cmb_Busqueda_Bien_Mueble_Marca.SelectedIndex > 0) {
                                Bienes.P_Marca_ID = Cmb_Busqueda_Bien_Mueble_Marca.SelectedItem.Value;
                            }
                            if (Cmb_Busqueda_Bien_Mueble_Estatus.SelectedIndex > 0) {
                                Bienes.P_Estatus = Cmb_Busqueda_Bien_Mueble_Estatus.SelectedItem.Value;
                            }
                            if (Txt_Busqueda_Bien_Mueble_Factura.Text.Trim().Length > 0) {
                                Bienes.P_Factura = Txt_Busqueda_Bien_Mueble_Factura.Text.Trim();
                            }
                            if (Txt_Busqueda_Bien_Mueble_Numero_Serie.Text.Trim().Length > 0) {
                                Bienes.P_Numero_Serie = Txt_Busqueda_Bien_Mueble_Numero_Serie.Text.Trim();
                            }
                        } else if (Session["FILTRO_BUSQUEDA"].ToString().Trim().Equals("RESGUARDANTES")) {
                            if (Txt_Busqueda_Bien_Mueble_RFC_Resguardante.Text.Trim().Length > 0) {
                                Bienes.P_RFC_Resguardante = Txt_Busqueda_Bien_Mueble_RFC_Resguardante.Text.Trim();
                            }
                            if (Txt_Busqueda_Bien_Mueble_No_Empleado.Text.Trim().Length > 0) {
                                Bienes.P_No_Empleado_Resguardante = Txt_Busqueda_Bien_Mueble_No_Empleado.Text.Trim();
                            }
                            if (Cmb_Busqueda_Bien_Mueble_Resguardantes_Dependencias.SelectedIndex > 0) {
                                Bienes.P_Dependencia_ID = Cmb_Busqueda_Bien_Mueble_Resguardantes_Dependencias.SelectedItem.Value.Trim();
                            }
                            if (Cmb_Busqueda_Bien_Mueble_Nombre_Resguardante.SelectedIndex > 0) {
                                Bienes.P_Resguardante_ID = Cmb_Busqueda_Bien_Mueble_Nombre_Resguardante.SelectedItem.Value.Trim();
                            }                    
                        }
                    }
                    Grid_Listado_Bienes_Bien_Mueble.DataSource = Bienes.Consultar_DataTable();
                    Grid_Listado_Bienes_Bien_Mueble.PageIndex = Pagina;
                    Grid_Listado_Bienes_Bien_Mueble.DataBind();
                    Grid_Listado_Bienes_Bien_Mueble.Columns[1].Visible = false;
                    MPE_Busqueda_Bien_Mueble.Show();
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Llenar_Grid_Listado_Vehiculos
            ///DESCRIPCIÓN          : Llena el Grid de los Vehiculos.
            ///PARAMETROS           : 
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 18/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Llenar_Grid_Listado_Bienes_Vehiculos(Int32 Pagina) { 
               try {
                    Grid_Listado_Busqueda_Vehiculo.Columns[1].Visible = true;
                    Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculos = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
                    Vehiculos.P_Tipo_DataTable = "VEHICULOS";
                    if (Session["FILTRO_BUSQUEDA"] != null) {
                        Vehiculos.P_Tipo_Filtro_Busqueda = Session["FILTRO_BUSQUEDA"].ToString();
                        if (Session["FILTRO_BUSQUEDA"].ToString().Trim().Equals("DATOS_GENERALES")) {
                            if (Txt_Busqueda_Vehiculo_Numero_Inventario.Text.Trim().Length > 0) { Vehiculos.P_Numero_Inventario = Convert.ToInt64(Txt_Busqueda_Vehiculo_Numero_Inventario.Text.Trim()); }
                            if (Txt_Busqueda_Vehiculo_Numero_Economico.Text.Trim().Length > 0) { Vehiculos.P_Numero_Economico_ = Txt_Busqueda_Vehiculo_Numero_Economico.Text.Trim(); }
                            if (Txt_Busqueda_Vehiculo_Anio_Fabricacion.Text.Trim().Length > 0) { Vehiculos.P_Anio_Fabricacion = Convert.ToInt32(Txt_Busqueda_Vehiculo_Anio_Fabricacion.Text.Trim()); }
                            Vehiculos.P_Modelo_ID = Txt_Busqueda_Vehiculo_Modelo.Text.Trim();
                            if (Cmb_Busqueda_Vehiculo_Marca.SelectedIndex > 0) {
                                Vehiculos.P_Marca_ID = Cmb_Busqueda_Vehiculo_Marca.SelectedItem.Value.Trim();
                            }
                            if (Cmb_Busqueda_Vehiculo_Tipo_Vehiculo.SelectedIndex > 0) {
                                Vehiculos.P_Tipo_Vehiculo_ID = Cmb_Busqueda_Vehiculo_Tipo_Vehiculo.SelectedItem.Value.Trim();
                            }
                            if (Cmb_Busqueda_Vehiculo_Tipo_Combustible.SelectedIndex > 0) {
                                Vehiculos.P_Tipo_Combustible_ID = Cmb_Busqueda_Vehiculo_Tipo_Combustible.SelectedItem.Value.Trim();
                            }
                            if (Cmb_Busqueda_Vehiculo_Color.SelectedIndex > 0) {
                                Vehiculos.P_Color_ID = Cmb_Busqueda_Vehiculo_Color.SelectedItem.Value.Trim();
                            }
                            if (Cmb_Busqueda_Vehiculo_Zonas.SelectedIndex > 0) {
                                Vehiculos.P_Zona_ID = Cmb_Busqueda_Vehiculo_Zonas.SelectedItem.Value.Trim();
                            }
                            if (Cmb_Busqueda_Vehiculo_Estatus.SelectedIndex > 0) {
                                Vehiculos.P_Estatus = Cmb_Busqueda_Vehiculo_Estatus.SelectedItem.Value.Trim();
                            }
                            if (Cmb_Busqueda_Vehiculo_Dependencias.SelectedIndex > 0) {
                                Vehiculos.P_Dependencia_ID = Cmb_Busqueda_Vehiculo_Dependencias.SelectedItem.Value.Trim();
                            }
                        } else if (Session["FILTRO_BUSQUEDA"].ToString().Trim().Equals("RESGUARDANTES")) {
                            Vehiculos.P_RFC_Resguardante = Txt_Busqueda_Vehiculo_RFC_Resguardante.Text.Trim();
                            Vehiculos.P_RFC_Resguardante = Txt_Busqueda_Vehiculo_No_Empleado.Text.Trim();
                            if (Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias.SelectedIndex > 0) {
                                Vehiculos.P_Dependencia_ID = Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias.SelectedItem.Value.Trim();
                            }
                            if (Cmb_Busqueda_Vehiculo_Nombre_Resguardante.SelectedIndex > 0) {
                                Vehiculos.P_Resguardante_ID = Cmb_Busqueda_Vehiculo_Nombre_Resguardante.SelectedItem.Value.Trim();
                            }
                        }
                    }
                    Grid_Listado_Busqueda_Vehiculo.DataSource = Vehiculos.Consultar_DataTable();
                    Grid_Listado_Busqueda_Vehiculo.PageIndex = Pagina;
                    Grid_Listado_Busqueda_Vehiculo.DataBind();
                    Grid_Listado_Busqueda_Vehiculo.Columns[1].Visible = false;
                    MPE_Busqueda_Vehiculo.Show();
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Llenar_Grid_Listado_Bienes_Sin_Inventario
            ///DESCRIPCIÓN          : Llena el Grid de los Bienes SIn Inventario.
            ///PARAMETROS           : 
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 22/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Llenar_Grid_Listado_Bienes_Sin_Inventario(Int32 Pagina) {
                try {
                    Grid_Listado_Bienes_Sin_Inventario.Columns[1].Visible = true;
                    Cls_Ope_Pat_Bienes_Sin_Inv_Negocio BSI_Negocio = new Cls_Ope_Pat_Bienes_Sin_Inv_Negocio();
                    if (Txt_Busqueda_BSI_Nombre.Text.Trim().Length > 0) { BSI_Negocio.P_Nombre = Txt_Busqueda_BSI_Nombre.Text.Trim(); } else { BSI_Negocio.P_Nombre = ""; }
                    if (Cmb_Busqueda_BSI_Marca.SelectedIndex > 0) { BSI_Negocio.P_Marca = Cmb_Busqueda_BSI_Marca.SelectedItem.Value.Trim(); }
                    if (Txt_Busqueda_BSI_Modelo.Text.Trim().Length > 0) { BSI_Negocio.P_Modelo = Txt_Busqueda_BSI_Modelo.Text.Trim(); }
                    if (Cmb_Busqueda_BSI_Material.SelectedIndex > 0) { BSI_Negocio.P_Material = Cmb_Busqueda_BSI_Material.SelectedItem.Value.Trim(); }
                    if (Cmb_Busqueda_BSI_Color.SelectedIndex > 0) { BSI_Negocio.P_Color = Cmb_Busqueda_BSI_Color.SelectedItem.Value.Trim(); }
                    if (Txt_Busqueda_BSI_Numero_Serie.Text.Trim().Length > 0) { BSI_Negocio.P_Numero_Serie = Txt_Busqueda_BSI_Numero_Serie.Text.Trim(); }
                    if (Cmb_Busqueda_BSI_Estado.SelectedIndex > 0) { BSI_Negocio.P_Estado = Cmb_Busqueda_BSI_Estado.SelectedItem.Value.Trim(); }
                    if (Cmb_Busqueda_BSI_Estatus.SelectedIndex > 0) { BSI_Negocio.P_Estatus = Cmb_Busqueda_BSI_Estatus.SelectedItem.Value.Trim(); }
                    if (Cmb_Busqueda_BSI_Tipo_Parent.SelectedIndex > 0) { BSI_Negocio.P_Tipo_Parent = Cmb_Busqueda_BSI_Tipo_Parent.SelectedItem.Value.Trim(); }
                    if (Txt_Busqueda_BSI_No_Inv_Parent.Text.Trim().Length > 0) { BSI_Negocio.P_No_Inventario_Parent = Txt_Busqueda_BSI_No_Inv_Parent.Text.Trim(); }
                    if (Txt_Busqueda_BSI_RFC_Resguardante.Text.Trim().Length > 0) { BSI_Negocio.P_RFC_Resguardante = Txt_Busqueda_BSI_RFC_Resguardante.Text.Trim(); }
                    if (Txt_Busqueda_BSI_No_Empleado_Resguardante.Text.Trim().Length > 0) { BSI_Negocio.P_No_Empleado_Resguardante = Txt_Busqueda_BSI_No_Empleado_Resguardante.Text.Trim(); }
                    if (Cmb_Busqueda_BSI_Dependencia_Resguardante.SelectedIndex > 0) { BSI_Negocio.P_Dependencia_ID = Cmb_Busqueda_BSI_Dependencia_Resguardante.SelectedItem.Value.Trim(); }
                    if (Cmb_Busqueda_BSI_Nombre_Resguardante.SelectedIndex > 0) { BSI_Negocio.P_Resguardante_ID = Cmb_Busqueda_BSI_Nombre_Resguardante.SelectedItem.Value.Trim(); }
                    DataTable Dt_Datos = BSI_Negocio.Consultar_Bienes_Sin_Inventario();
                    Grid_Listado_Bienes_Sin_Inventario.DataSource = Dt_Datos;
                    Grid_Listado_Bienes_Sin_Inventario.PageIndex = Pagina;
                    Grid_Listado_Bienes_Sin_Inventario.DataBind();
                    Grid_Listado_Bienes_Sin_Inventario.Columns[1].Visible = false;
                    MPE_Busqueda_BSI.Show();
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = "Verificar.";
                    Lbl_Mensaje_Error.Text = "Excepcion ['" + Ex.Message + "']";
                    Div_Contenedor_Msj_Error.Visible = true;
                    MPE_Busqueda_BSI.Hide();
                }
            }

        #endregion 

        #region "Interaccion con Base de Datos [Alteración de Datos]"

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Alta_Bien
            ///DESCRIPCIÓN          : Da de Alta la información de un Bien.
            ///PARAMETROS           : 
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 04/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private Cls_Ope_Pat_Bienes_Sin_Inv_Negocio Alta_Bien() {
                Cls_Ope_Pat_Bienes_Sin_Inv_Negocio Bien_Negocio = new Cls_Ope_Pat_Bienes_Sin_Inv_Negocio();
                Bien_Negocio.P_Bien_Parent_ID = Hdf_Bien_Parent_ID.Value;
                Bien_Negocio.P_Tipo_Parent = Cmb_Tipo_Parent.SelectedItem.Value;
                Bien_Negocio.P_Nombre = Txt_Nombre_Bien.Text.Trim();
                Bien_Negocio.P_Marca = Cmb_Marca.SelectedItem.Value;
                Bien_Negocio.P_Costo_Inicial = Convert.ToDouble(Txt_Costo.Text.Trim());
                Bien_Negocio.P_Material = Cmb_Material.SelectedItem.Value;
                Bien_Negocio.P_Color = Cmb_Color.SelectedItem.Value;
                Bien_Negocio.P_Estado = Cmb_Estado.SelectedItem.Value;
                Bien_Negocio.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                Bien_Negocio.P_Comentarios = Txt_Comentarios.Text.Trim();
                Bien_Negocio.P_Fecha_Adquisicion = Convert.ToDateTime(Txt_Fecha_Adquisicion.Text.Trim());
                Bien_Negocio.P_Usuario_Nombre = Cls_Sessiones.Nombre_Empleado;
                Bien_Negocio.P_Producto_ID = Hdf_Producto_ID.Value;
                Bien_Negocio.P_Modelo = Txt_Modelo.Text.Trim();
                Bien_Negocio.P_Numero_Serie = Txt_Numero_Serie.Text.Trim();
                Bien_Negocio = Bien_Negocio.Alta_Bien_Sin_Inventario();
                Limpiar_Campos_Generales();
                return Bien_Negocio;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Modificar_Bien
            ///DESCRIPCIÓN          : Actualiza la información de un Bien.
            ///PARAMETROS           : 
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 04/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Modificar_Bien() {
                Cls_Ope_Pat_Bienes_Sin_Inv_Negocio Bien_Negocio = new Cls_Ope_Pat_Bienes_Sin_Inv_Negocio();
                Bien_Negocio.P_Bien_ID = Convert.ToInt32(Hdf_Bien_ID.Value);
                Bien_Negocio.P_Bien_Parent_ID = Hdf_Bien_Parent_ID.Value;
                Bien_Negocio.P_Tipo_Parent = Cmb_Tipo_Parent.SelectedItem.Value;
                Bien_Negocio.P_Nombre = Txt_Nombre_Bien.Text.Trim();
                Bien_Negocio.P_Marca = Cmb_Marca.SelectedItem.Value;
                Bien_Negocio.P_Costo_Inicial = Convert.ToDouble(Txt_Costo.Text.Trim());
                Bien_Negocio.P_Material = Cmb_Material.SelectedItem.Value;
                Bien_Negocio.P_Color = Cmb_Color.SelectedItem.Value;
                Bien_Negocio.P_Estado = Cmb_Estado.SelectedItem.Value;
                Bien_Negocio.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                Bien_Negocio.P_Comentarios = Txt_Comentarios.Text.Trim();
                Bien_Negocio.P_Fecha_Adquisicion = Convert.ToDateTime(Txt_Fecha_Adquisicion.Text.Trim());
                Bien_Negocio.P_Usuario_Nombre = Cls_Sessiones.Nombre_Empleado;
                Bien_Negocio.P_Modelo = Txt_Modelo.Text.Trim();
                Bien_Negocio.P_Numero_Serie = Txt_Numero_Serie.Text.Trim();
                if (Txt_Motivo_Baja.Visible) {
                    Bien_Negocio.P_Motivo_Baja = Txt_Motivo_Baja.Text.Trim();
                }
                Bien_Negocio.Modifica_Bien_Sin_Inventario();
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Mostrar_Detalles_Bien
            ///DESCRIPCIÓN          : Muestra la información de un Bien.
            ///PARAMETROS           : Bien. Datos a Cargar.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 11/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Mostrar_Detalles_Bien(Cls_Ope_Pat_Bienes_Sin_Inv_Negocio Bien) {
                Hdf_Bien_ID.Value = Bien.P_Bien_ID.ToString();
                Hdf_Producto_ID.Value = Bien.P_Producto_ID.ToString();
                Txt_Nombre_Bien.Text = Bien.P_Nombre;
                Cmb_Tipo_Parent.SelectedIndex = Cmb_Tipo_Parent.Items.IndexOf(Cmb_Tipo_Parent.Items.FindByValue(Bien.P_Tipo_Parent));
                Cmb_Tipo_Parent_SelectedIndexChanged(Cmb_Tipo_Parent, null);
                Cmb_Marca.SelectedIndex = Cmb_Marca.Items.IndexOf(Cmb_Marca.Items.FindByValue(Bien.P_Marca));
                Txt_Modelo.Text = Bien.P_Modelo;
                Cmb_Material.SelectedIndex = Cmb_Material.Items.IndexOf(Cmb_Material.Items.FindByValue(Bien.P_Material));
                Cmb_Color.SelectedIndex = Cmb_Color.Items.IndexOf(Cmb_Color.Items.FindByValue(Bien.P_Color));
                Txt_Costo.Text = Bien.P_Costo_Inicial.ToString("#######0.00");
                Txt_Fecha_Adquisicion.Text = String.Format("{0:dd/MMM/yyyy}", Bien.P_Fecha_Adquisicion);
                Txt_Numero_Serie.Text = Bien.P_Numero_Serie;
                Cmb_Estado.SelectedIndex = Cmb_Estado.Items.IndexOf(Cmb_Estado.Items.FindByValue(Bien.P_Estado));
                Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Bien.P_Estatus));
                Cmb_Estatus_SelectedIndexChanged(Cmb_Estatus, null);
                if (Bien.P_Motivo_Baja != null && Bien.P_Motivo_Baja.Length>0) {
                    Txt_Motivo_Baja.Text = Bien.P_Motivo_Baja.Trim();
                }
                Txt_Comentarios.Text = Bien.P_Comentarios;
                if (Bien.P_Tipo_Parent.Trim().Equals("BIEN_MUEBLE")) {
                    Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Mueble = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                    Bien_Mueble.P_Bien_Mueble_ID = Bien.P_Bien_Parent_ID.Trim();
                    Bien_Mueble = Bien_Mueble.Consultar_Detalles_Bien_Mueble();
                    Mostrar_Detalles_Bien_Mueble(Bien_Mueble);

                } else if (Bien.P_Tipo_Parent.Trim().Equals("VEHICULO")) {
                    Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
                    Vehiculo.P_Vehiculo_ID = Bien.P_Bien_Parent_ID.Trim();
                    Vehiculo = Vehiculo.Consultar_Detalles_Vehiculo();
                    Mostrar_Detalles_Vehiculo(Vehiculo);
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Mostrar_Detalles_Vehiculo
            ///DESCRIPCIÓN          : Muestra la información de un Vehículo.
            ///PARAMETROS           : Vehiculo. Datos a Cargar.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 11/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Mostrar_Detalles_Vehiculo(Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo) {
                if (Vehiculo.P_Vehiculo_ID != null && Vehiculo.P_Vehiculo_ID.Trim().Length > 0) {
                    Hdf_Bien_Parent_ID.Value = Vehiculo.P_Vehiculo_ID;
                    Txt_Vehiculo_Nombre.Text = Vehiculo.P_Nombre_Producto;
                    Txt_Vehiculo_Dependencia.Text = Obtener_Texto_Dependencia(Vehiculo.P_Dependencia_ID);
                    Txt_Vehiculo_No_Inventario.Text = Vehiculo.P_Numero_Inventario.ToString();
                    Txt_Vehiculo_Numero_Serie.Text = Vehiculo.P_Serie_Carroceria.Trim();
                    if (Vehiculo.P_Marca_ID != null && Vehiculo.P_Marca_ID.Trim().Length > 0) {
                        Cls_Cat_Com_Marcas_Negocio Marca_Negocio = new Cls_Cat_Com_Marcas_Negocio();
                        Marca_Negocio.P_Marca_ID = Vehiculo.P_Marca_ID;
                        DataTable Dt_Marcas = Marca_Negocio.Consulta_Marcas();
                        if (Dt_Marcas != null && Dt_Marcas.Rows.Count > 0) {
                            Txt_Vehiculo_Marca.Text = Dt_Marcas.Rows[0][Cat_Com_Marcas.Campo_Nombre].ToString();
                        }
                    }
                    Txt_Vehiculo_Modelo.Text = Vehiculo.P_Modelo_ID;
                    if (Vehiculo.P_Tipo_Vehiculo_ID != null && Vehiculo.P_Tipo_Vehiculo_ID.Trim().Length > 0) {
                        Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio Tipo_Negocio = new Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio();
                        Tipo_Negocio.P_Tipo_Vehiculo_ID = Vehiculo.P_Tipo_Vehiculo_ID;
                        Tipo_Negocio = Tipo_Negocio.Consultar_Datos_Vehiculo();
                        if (Tipo_Negocio.P_Tipo_Vehiculo_ID != null && Tipo_Negocio.P_Tipo_Vehiculo_ID.Trim().Length > 0) {
                            Txt_Vehiculo_Tipo.Text = Tipo_Negocio.P_Descripcion;
                        }
                    }
                    if (Vehiculo.P_Color_ID != null && Vehiculo.P_Color_ID.Trim().Length > 0) {
                        Cls_Cat_Pat_Com_Colores_Negocio Color_Negocio = new Cls_Cat_Pat_Com_Colores_Negocio();
                        Color_Negocio.P_Color_ID = Vehiculo.P_Color_ID;
                        Color_Negocio.P_Tipo_DataTable = "COLORES";
                        DataTable Dt_Colores = Color_Negocio.Consultar_DataTable();
                        if (Dt_Colores != null && Dt_Colores.Rows.Count > 0) {
                            Txt_Vehiculo_Color.Text = Dt_Colores.Rows[0][Cat_Pat_Colores.Campo_Descripcion].ToString();
                        }
                    }
                    Txt_Vehiculo_Numero_Economico.Text = Vehiculo.P_Numero_Economico_;
                    Txt_Vehiculo_Placas.Text = Vehiculo.P_Placas;
                    Llenar_Grid_Resguardos(Vehiculo.P_Resguardantes);
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Mostrar_Detalles_Bien_Mueble
            ///DESCRIPCIÓN          : Muestra la información de un Bien Mueble.
            ///PARAMETROS           : Bien_Mueble. Datos a Cargar.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 11/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Mostrar_Detalles_Bien_Mueble(Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Mueble) {
                if (Bien_Mueble.P_Bien_Mueble_ID != null && Bien_Mueble.P_Bien_Mueble_ID.Trim().Length > 0) { 
                    Hdf_Bien_Parent_ID.Value = Bien_Mueble.P_Bien_Mueble_ID;
                    Txt_Bien_Mueble_Nombre.Text = Bien_Mueble.P_Nombre_Producto;
                    Txt_Bien_Mueble_Dependencia.Text = Obtener_Texto_Dependencia(Bien_Mueble.P_Dependencia_ID);
                    Txt_Bien_Mueble_Inventario_Anterior.Text = Bien_Mueble.P_Numero_Inventario_Anterior;
                    Txt_Bien_Mueble_Inventario_SIAS.Text = Bien_Mueble.P_Numero_Inventario;
                    Txt_Bien_Mueble_Numero_Serie.Text = Bien_Mueble.P_Numero_Serie;
                    if (Bien_Mueble.P_Marca_ID != null && Bien_Mueble.P_Marca_ID.Trim().Length > 0) {
                        Cls_Cat_Com_Marcas_Negocio Marca_Negocio = new Cls_Cat_Com_Marcas_Negocio();
                        Marca_Negocio.P_Marca_ID = Bien_Mueble.P_Marca_ID;
                        DataTable Dt_Marcas = Marca_Negocio.Consulta_Marcas();
                        if (Dt_Marcas != null && Dt_Marcas.Rows.Count > 0) {
                            Txt_Bien_Mueble_Marca.Text = Dt_Marcas.Rows[0][Cat_Com_Marcas.Campo_Nombre].ToString();
                        }
                    }
                    Txt_Bien_Mueble_Modelo.Text = Bien_Mueble.P_Modelo;
                    if (Bien_Mueble.P_Material_ID != null && Bien_Mueble.P_Material_ID.Trim().Length > 0) {
                        Cls_Cat_Pat_Com_Materiales_Negocio Material_Negocio = new Cls_Cat_Pat_Com_Materiales_Negocio();
                        Material_Negocio.P_Material_ID = Bien_Mueble.P_Material_ID;
                        Material_Negocio.P_Tipo_DataTable = "MATERIALES";
                        DataTable Dt_Materiales = Material_Negocio.Consultar_DataTable();
                        if (Dt_Materiales != null && Dt_Materiales.Rows.Count > 0) {
                            Txt_Bien_Mueble_Material.Text = Dt_Materiales.Rows[0][Cat_Pat_Materiales.Campo_Descripcion].ToString();
                        }
                    }
                    if (Bien_Mueble.P_Color_ID != null && Bien_Mueble.P_Color_ID.Trim().Length > 0) {
                        Cls_Cat_Pat_Com_Colores_Negocio Color_Negocio = new Cls_Cat_Pat_Com_Colores_Negocio();
                        Color_Negocio.P_Color_ID = Bien_Mueble.P_Color_ID;
                        Color_Negocio.P_Tipo_DataTable = "COLORES";
                        DataTable Dt_Colores = Color_Negocio.Consultar_DataTable();
                        if (Dt_Colores != null && Dt_Colores.Rows.Count > 0) {
                            Txt_Bien_Mueble_Color.Text = Dt_Colores.Rows[0][Cat_Pat_Colores.Campo_Descripcion].ToString();
                        }
                    }
                    Llenar_Grid_Resguardos(Bien_Mueble.P_Resguardantes);
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Obtener_Texto_Dependencia
            ///DESCRIPCIÓN          : Obtiene el Texto con clave y nombre de una dependencia
            ///PARAMETROS           : Dependencia_ID. Dependencia para obtener su texto.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 11/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private String Obtener_Texto_Dependencia(String Dependencia_ID) {
                String Texto = "";
                Cls_Cat_Dependencias_Negocio Dependencia_Negocio = new Cls_Cat_Dependencias_Negocio();
                Dependencia_Negocio.P_Dependencia_ID = Dependencia_ID;
                DataTable Dt_Datos = Dependencia_Negocio.Consulta_Dependencias();
                if (Dt_Datos != null && Dt_Datos.Rows.Count > 0) {
                    if (!String.IsNullOrEmpty(Dt_Datos.Rows[0][Cat_Dependencias.Campo_Clave].ToString())) {
                        Texto = Dt_Datos.Rows[0][Cat_Dependencias.Campo_Clave].ToString();
                    }
                    Texto = Texto + " - ";
                    if (!String.IsNullOrEmpty(Dt_Datos.Rows[0][Cat_Dependencias.Campo_Nombre].ToString())) {
                        Texto += Dt_Datos.Rows[0][Cat_Dependencias.Campo_Nombre].ToString();
                    }
                }
                return Texto;
            }

        #endregion

        #region "Configuración de Componentes"

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Habilitacion_Componentes
            ///DESCRIPCIÓN          : Habilita e Inhabilita los componentes del Formulario.
            ///PARAMETROS           : 
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 04/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Habilitacion_Componentes(String Tipo) {
                if (Tipo.Trim().Equals("NUEVO")) {
                    Btn_Nuevo.ToolTip = "Guardar";
                    Btn_Nuevo.AlternateText = "Guardar";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Modificar.Visible = false;
                    Btn_Salir.ToolTip = "Cancelar Operación de Alta";
                    Btn_Salir.AlternateText = "Cancelar Operación de Alta";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Div_Busqueda.Visible = false;
                    Btn_Lanzar_Buscar_Producto.Visible = true;
                    Btn_Buscar_Vehiculo.Visible = true;
                    Btn_Buscar_Bien_Mueble.Visible = true;
                    Cmb_Tipo_Parent.Enabled = true;
                    Cmb_Marca.Enabled = true;
                    Txt_Modelo.Enabled = true;
                    Cmb_Material.Enabled = true;
                    Cmb_Color.Enabled = true;
                    Txt_Costo.Enabled = true;
                    Btn_Fecha_Adquisicion.Enabled = true;
                    Txt_Numero_Serie.Enabled = true;
                    Cmb_Estado.Enabled = true;
                    Cmb_Estatus.Enabled = false;
                    Cmb_Estatus.SelectedIndex = 1;
                    Txt_Comentarios.Enabled = true;
                    Txt_Motivo_Baja.Enabled = true;
                    Cmb_Tipo_Parent_SelectedIndexChanged(Cmb_Tipo_Parent, null);
                    Cmb_Estatus_SelectedIndexChanged(Cmb_Estatus, null);
                } else if (Tipo.Trim().Equals("MODIFICAR")) {
                    Btn_Modificar.ToolTip = "Guardar";
                    Btn_Modificar.AlternateText = "Guardar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Nuevo.Visible = false;
                    Btn_Salir.ToolTip = "Cancelar Operación de Actualización";
                    Btn_Salir.AlternateText = "Cancelar Operación de Actualización";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Div_Busqueda.Visible = false;
                    Btn_Lanzar_Buscar_Producto.Visible = false;
                    Btn_Buscar_Vehiculo.Visible = true;
                    Btn_Buscar_Bien_Mueble.Visible = true;
                    Cmb_Tipo_Parent.Enabled = true;
                    Cmb_Marca.Enabled = true;
                    Txt_Modelo.Enabled = true;
                    Cmb_Material.Enabled = true;
                    Cmb_Color.Enabled = true;
                    Txt_Costo.Enabled = true;
                    Btn_Fecha_Adquisicion.Enabled = true;
                    Txt_Numero_Serie.Enabled = true;
                    Cmb_Estado.Enabled = true;
                    Cmb_Estatus.Enabled = true;
                    Txt_Comentarios.Enabled = true;
                    Txt_Motivo_Baja.Enabled = true;
                } else {
                    Btn_Nuevo.Visible = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Nuevo.AlternateText = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevO.png";
                    Btn_Modificar.Visible = true;
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Modificar.AlternateText = "Modificar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Salir.ToolTip = "Salir";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Div_Busqueda.Visible = true;
                    Btn_Lanzar_Buscar_Producto.Visible = false;
                    Btn_Buscar_Vehiculo.Visible = false;
                    Btn_Buscar_Bien_Mueble.Visible = false;
                    Cmb_Tipo_Parent.Enabled = false;
                    Cmb_Marca.Enabled = false;
                    Txt_Modelo.Enabled = false;
                    Cmb_Material.Enabled = false;
                    Cmb_Color.Enabled = false;
                    Txt_Costo.Enabled = false;
                    Btn_Fecha_Adquisicion.Enabled = false;
                    Txt_Numero_Serie.Enabled = false;
                    Cmb_Estado.Enabled = false;
                    Cmb_Estatus.Enabled = false;
                    Txt_Comentarios.Enabled = false;
                    Txt_Motivo_Baja.Enabled = false;
                    Cmb_Tipo_Parent_SelectedIndexChanged(Cmb_Tipo_Parent, null);
                    Cmb_Estatus_SelectedIndexChanged(Cmb_Estatus, null);
                }
            }

        #endregion

        #region "Validaciones"
            
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Validar_Bien
            ///DESCRIPCIÓN          : Valida el Bien que se dara de Alta.
            ///PARAMETROS           : 
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 11/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public Boolean Validar_Bien() { 
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                String Mensaje_Error = "";
                Boolean Validacion = true;
                if (Hdf_Producto_ID.Value.Trim().Length == 0) {
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar el Producto.";
                    Validacion = false;
                }
                if (Cmb_Tipo_Parent.SelectedIndex == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar un Tipo de Bien al que se Asignara.";
                    Validacion = false;
                }
                if (Cmb_Marca.SelectedIndex == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar una Marca.";
                    Validacion = false;
                }
                if (Txt_Modelo.Text.Trim().Length == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ Indroducir el Modelo.";
                    Validacion = false;
                }
                if (Cmb_Material.SelectedIndex == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar un Material.";
                    Validacion = false;
                }
                if (Cmb_Color.SelectedIndex == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar un Color.";
                    Validacion = false;
                }
                if (Txt_Costo.Text.Trim().Length == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ Indroducir el Costo.";
                    Validacion = false;
                }
                if (Txt_Fecha_Adquisicion.Text.Trim().Length == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ Indroducir la Fecha de Adquisición.";
                    Validacion = false;
                }
                if (Txt_Numero_Serie.Text.Trim().Length == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ Indroducir el Número de Serie.";
                    Validacion = false;
                }
                if (Cmb_Estado.SelectedIndex == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar un Estado.";
                    Validacion = false;
                } 
                if (Cmb_Estatus.SelectedIndex == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar un Estatus.";
                    Validacion = false;
                }
                if (Txt_Motivo_Baja.Visible && Txt_Motivo_Baja.Text.Trim().Length==0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir el Motivo de la Baja.";
                    Validacion = false;
                }
                if (Hdf_Bien_Parent_ID.Value.Trim().Length == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar el Bien al que se le Asignará.";
                    Validacion = false;
                }
                if (!Validacion) {
                    Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
                    Div_Contenedor_Msj_Error.Visible = true;
                }
                return Validacion;
            }
            
        #endregion

    #endregion

    #region "Grids"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Productos_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView de los Productos
        ///PROPIEDADES:     
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 18/Noviembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Listado_Productos_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                Llenar_MPE_Listado_Productos(e.NewPageIndex);
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Productos_SelectedIndexChanged
        ///DESCRIPCIÓN: Maneja el evento de cambio de Seleccion del GridView de Productos del
        ///             Modal de Busqueda.
        ///PROPIEDADES:     
        ///CREO     : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO   : 18/Noviembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Listado_Productos_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                if (Grid_Listado_Productos.SelectedIndex > (-1)) {
                    String Producto_ID = Grid_Listado_Productos.SelectedRow.Cells[1].Text.Trim();
                    Cls_Cat_Com_Productos_Negocio Producto_Negocio = new Cls_Cat_Com_Productos_Negocio();
                    Producto_Negocio.P_Producto_ID = Producto_ID;
                    Producto_Negocio.P_Estatus = "ACTIVO";
                    DataTable Dt_Productos = Producto_Negocio.Consulta_Datos_Producto();
                    if (Dt_Productos != null && Dt_Productos.Rows.Count > 0) {
                        Hdf_Producto_ID.Value = Dt_Productos.Rows[0][Cat_Com_Productos.Campo_Producto_ID].ToString();
                        Txt_Nombre_Bien.Text = Dt_Productos.Rows[0][Cat_Com_Productos.Campo_Nombre].ToString() + " [" + Dt_Productos.Rows[0][Cat_Com_Productos.Campo_Descripcion].ToString() + "]";
                    }
                    Mpe_Productos_Cabecera.Hide();
                    Grid_Listado_Productos.SelectedIndex = (-1);
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Productos_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView de los Vehiculos
        ///PROPIEDADES:     
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 18/Noviembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Listado_Busqueda_Vehiculo_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                Llenar_Grid_Listado_Bienes_Vehiculos(e.NewPageIndex);
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Busqueda_Vehiculo_SelectedIndexChanged
        ///DESCRIPCIÓN: Maneja el evento de cambio de Seleccion del GridView de Vehiculos del
        ///             Modal de Busqueda.
        ///PROPIEDADES:     
        ///CREO     : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO   : 18/Noviembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Listado_Busqueda_Vehiculo_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                Limpiar_Campos_Vehiculo_Parent();
                if (Grid_Listado_Busqueda_Vehiculo.SelectedIndex > (-1)) {
                    String Vehiculo_ID = Grid_Listado_Busqueda_Vehiculo.SelectedRow.Cells[1].Text.Trim();
                    Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo_Negocio = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
                    Vehiculo_Negocio.P_Vehiculo_ID = Vehiculo_ID;
                    Vehiculo_Negocio = Vehiculo_Negocio.Consultar_Detalles_Vehiculo();
                    Mostrar_Detalles_Vehiculo(Vehiculo_Negocio);
                    MPE_Busqueda_Vehiculo.Hide();
                    Grid_Listado_Busqueda_Vehiculo.SelectedIndex = (-1);
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Productos_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView de los Vehiculos
        ///PROPIEDADES:     
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 18/Noviembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Listado_Bienes_Bien_Mueble_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                Llenar_Grid_Listado_Bienes_Bien_Mueble(e.NewPageIndex);
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Busqueda_Vehiculo_SelectedIndexChanged
        ///DESCRIPCIÓN: Maneja el evento de cambio de Seleccion del GridView de Vehiculos del
        ///             Modal de Busqueda.
        ///PROPIEDADES:     
        ///CREO     : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO   : 18/Noviembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Listado_Bienes_Bien_Mueble_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                Limpiar_Campos_Bien_Mueble_Parent();
                if (Grid_Listado_Bienes_Bien_Mueble.SelectedIndex > (-1)) {
                    String Bien_Mueble_ID = Grid_Listado_Bienes_Bien_Mueble.SelectedRow.Cells[1].Text.Trim();
                    Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Mueble = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                    Bien_Mueble.P_Bien_Mueble_ID = Bien_Mueble_ID;
                    Bien_Mueble = Bien_Mueble.Consultar_Detalles_Bien_Mueble();
                    Mostrar_Detalles_Bien_Mueble(Bien_Mueble);
                    MPE_Busqueda_Bien_Mueble.Hide();
                    Grid_Listado_Bienes_Bien_Mueble.SelectedIndex = (-1);
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Bienes_Sin_Inventario_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView de los Bienes Sin Inventario.
        ///PROPIEDADES:     
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 22/Noviembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Listado_Bienes_Sin_Inventario_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                Llenar_Grid_Listado_Bienes_Sin_Inventario(e.NewPageIndex);
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Bienes_Sin_Inventario_SelectedIndexChanged
        ///DESCRIPCIÓN: Maneja el evento de cambio de Seleccion del GridView de Bienes 
        ///             Sin Inventario del Modal de Busqueda.
        ///PROPIEDADES:     
        ///CREO     : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO   : 22/Noviembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Listado_Bienes_Sin_Inventario_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                if (Grid_Listado_Bienes_Sin_Inventario.SelectedIndex > (-1)) {
                    String Bien_ID = Grid_Listado_Bienes_Sin_Inventario.SelectedRow.Cells[1].Text.Trim();
                    Cls_Ope_Pat_Bienes_Sin_Inv_Negocio BSI_Negocio = new Cls_Ope_Pat_Bienes_Sin_Inv_Negocio();
                    BSI_Negocio.P_Bien_ID = Convert.ToInt32(Bien_ID);
                    BSI_Negocio = BSI_Negocio.Consultar_Detalles_Bien_Sin_Inventario();
                    Mostrar_Detalles_Bien(BSI_Negocio);
                    MPE_Busqueda_BSI.Hide();
                    Grid_Listado_Bienes_Sin_Inventario.SelectedIndex = (-1);
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }


    #endregion

    #region "Eventos"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Nuevo_Click
        ///DESCRIPCIÓN          : Evento Click del Botón Nuevo.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 04/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e) {
            try { 
                if (Btn_Nuevo.AlternateText.Trim().Equals("Nuevo")) {
                    Limpiar_Campos_Generales();
                    Habilitacion_Componentes("NUEVO");
                } else {
                    if (Validar_Bien()) {
                        Cls_Ope_Pat_Bienes_Sin_Inv_Negocio Bien_Mueble = Alta_Bien();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('Alta Exitosa!!!');", true);
                        Habilitacion_Componentes("");
                        Mostrar_Detalles_Bien(Bien_Mueble);
                    }       
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Excepción al dar de alta el bien.";
                Lbl_Mensaje_Error.Text = "Ex:['" + Ex.Message + "']";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Modificar_Click
        ///DESCRIPCIÓN          : Evento Click del Botón Modificar.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 04/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e) {
            if (Btn_Modificar.AlternateText.Trim().Equals("Modificar")) {
                if (!String.IsNullOrEmpty(Hdf_Bien_ID.Value)) {
                    Habilitacion_Componentes("MODIFICAR");
                } else {
                    Lbl_Ecabezado_Mensaje.Text = "Verificar.";
                    Lbl_Mensaje_Error.Text = "Es necesario seleccionar el Bien a Modificar.";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            } else {
                if (Validar_Bien()) {
                    Modificar_Bien();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('Actualización Exitosa!!!');", true);
                    String Bien_ID = Hdf_Bien_ID.Value.Trim();
                    Limpiar_Campos_Generales();
                    Habilitacion_Componentes("");
                    Cls_Ope_Pat_Bienes_Sin_Inv_Negocio BSI_Negocio = new Cls_Ope_Pat_Bienes_Sin_Inv_Negocio();
                    BSI_Negocio.P_Bien_ID = Convert.ToInt32(Bien_ID);
                    BSI_Negocio = BSI_Negocio.Consultar_Detalles_Bien_Sin_Inventario();
                    Mostrar_Detalles_Bien(BSI_Negocio);
                }
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Salir_Click
        ///DESCRIPCIÓN          : Evento Click del Botón Salir.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 04/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Btn_Salir_Click(object sender, ImageClickEventArgs e) {
            if (Btn_Salir.AlternateText.Trim().Equals("Salir")) {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            } else {
                String Bien_Cargardo = null;
                if (Hdf_Bien_ID.Value != null && Hdf_Bien_ID.Value.Trim().Length > 0) {
                    Bien_Cargardo = Hdf_Bien_ID.Value.Trim();
                }
                Limpiar_Campos_Generales();
                Habilitacion_Componentes("");
                if (Bien_Cargardo != null) {
                    Cls_Ope_Pat_Bienes_Sin_Inv_Negocio BSI_Negocio = new Cls_Ope_Pat_Bienes_Sin_Inv_Negocio();
                    BSI_Negocio.P_Bien_ID = Convert.ToInt32(Bien_Cargardo);
                    BSI_Negocio = BSI_Negocio.Consultar_Detalles_Bien_Sin_Inventario();
                    Mostrar_Detalles_Bien(BSI_Negocio);
                }
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Cmb_Tipo_Parent_SelectedIndexChanged
        ///DESCRIPCIÓN          : Carga la Configuración a mostrar de las capas
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 04/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Cmb_Tipo_Parent_SelectedIndexChanged(object sender, EventArgs e) {
            String Tipo_Parent = Cmb_Tipo_Parent.SelectedItem.Value;
            if (Tipo_Parent.Equals("BIEN_MUEBLE")) {
                Limpiar_Campos_Vehiculo_Parent();
                Limpiar_Campos_Bien_Mueble_Parent();
                Div_Bien_Mueble_Parent.Visible = true;
                Div_Vehiculo_Parent.Visible = false;
                Div_Resguardos.Visible = true;
            } else if (Tipo_Parent.Equals("VEHICULO")) {
                Limpiar_Campos_Vehiculo_Parent();
                Limpiar_Campos_Bien_Mueble_Parent();
                Div_Bien_Mueble_Parent.Visible = false;
                Div_Vehiculo_Parent.Visible = true;
                Div_Resguardos.Visible = true;
            } else {
                Limpiar_Campos_Vehiculo_Parent();
                Limpiar_Campos_Bien_Mueble_Parent();
                Div_Bien_Mueble_Parent.Visible = false;
                Div_Vehiculo_Parent.Visible = false;
                Div_Resguardos.Visible = false;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Vehiculo_Click
        ///DESCRIPCIÓN          : Busca el Vehículo al cual se asignara el Bien Actual.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 11/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Btn_Buscar_Vehiculo_Click(object sender, ImageClickEventArgs e) {
            try {
                MPE_Busqueda_Vehiculo.Show();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Excepción.";
                Lbl_Mensaje_Error.Text = "Ex:['" + Ex.Message + "']";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Bien_Mueble_Click
        ///DESCRIPCIÓN          : Busca el Bien Mueble al cual se asignara el Bien Actual.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 11/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Btn_Buscar_Bien_Mueble_Click(object sender, ImageClickEventArgs e) {
            try {
                MPE_Busqueda_Bien_Mueble.Show();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Excepción.";
                Lbl_Mensaje_Error.Text = "Ex:['" + Ex.Message + "']";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Cmb_Estatus_SelectedIndexChanged
        ///DESCRIPCIÓN          : Maneja el evento del Combo de Estatus.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 11/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Cmb_Estatus_SelectedIndexChanged(object sender, EventArgs e) {
            if (Cmb_Estatus.SelectedIndex > 1) {
                Lbl_Motivo_Baja.Visible = true;
                Txt_Motivo_Baja.Visible = true;
            } else {
                Lbl_Motivo_Baja.Visible = false;
                Txt_Motivo_Baja.Visible = false;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Limpiar_Filtros_Buscar_Datos_Bien_Mueble_Click
        ///DESCRIPCIÓN          : Hace la limpieza de los campos.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 15/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Btn_Limpiar_Filtros_Buscar_Datos_Bien_Mueble_Click(object sender, ImageClickEventArgs e)
        {
            try {
                Txt_Busqueda_Bien_Mueble_Numero_Inventario.Text = "";
                Txt_Busqueda_Bien_Mueble_Numero_Inventario_SIAS.Text = "";
                Txt_Busqueda_Bien_Mueble_Producto.Text = "";
                Cmb_Busqueda_Bien_Mueble_Dependencias.SelectedIndex = 0;
                Txt_Busqueda_Bien_Mueble_Modelo.Text = "";
                Cmb_Busqueda_Bien_Mueble_Marca.SelectedIndex = 0;
                Txt_Busqueda_Bien_Mueble_Factura.Text = "";
                Cmb_Busqueda_Bien_Mueble_Estatus.SelectedIndex = 0;
                Txt_Busqueda_Bien_Mueble_Numero_Serie.Text = "";
                MPE_Busqueda_Bien_Mueble.Show();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Excepción.";
                Lbl_Mensaje_Error.Text = "Ex:['" + Ex.Message + "']";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Limpiar_Filtros_Buscar_Resguardante_Bien_Mueble_Click
        ///DESCRIPCIÓN          : Hace la limpieza de los campos.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 15/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Btn_Limpiar_Filtros_Buscar_Resguardante_Bien_Mueble_Click(object sender, ImageClickEventArgs e) {
            try {
                Txt_Busqueda_Bien_Mueble_RFC_Resguardante.Text = "";
                Txt_Busqueda_Bien_Mueble_No_Empleado.Text = "";
                Cmb_Busqueda_Bien_Mueble_Resguardantes_Dependencias.SelectedIndex = 0;
                Cmb_Busqueda_Bien_Mueble_Resguardantes_Dependencias_SelectedIndexChanged(Cmb_Busqueda_Bien_Mueble_Resguardantes_Dependencias, null);
                Cmb_Busqueda_Bien_Mueble_Nombre_Resguardante.SelectedIndex = 0;
                MPE_Busqueda_Bien_Mueble.Show();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Excepción.";
                Lbl_Mensaje_Error.Text = "Ex:['" + Ex.Message + "']";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Lanzar_Buscar_Producto_Click
        ///DESCRIPCIÓN          : Busca el Producto a agregar.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 18/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Btn_Lanzar_Buscar_Producto_Click(object sender, ImageClickEventArgs e)  {
            try {
                Mpe_Productos_Cabecera.Show();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Excepción.";
                Lbl_Mensaje_Error.Text = "Ex:['" + Ex.Message + "']";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Txt_Nombre_Producto_Buscar_TextChanged
        ///DESCRIPCIÓN: Maneja el evento de cambio de Texto del Nombre de Producto.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 18/Noviembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Txt_Nombre_Producto_Buscar_TextChanged(object sender, EventArgs e) {
            try {
                Llenar_MPE_Listado_Productos(0);
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Ejecutar_Busqueda_Productos_Click
        ///DESCRIPCIÓN: Ejecuta la Busqueda de los productos.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 18/Noviembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Ejecutar_Busqueda_Productos_Click(object sender, ImageClickEventArgs e) {
            try {
                Llenar_MPE_Listado_Productos(0);
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Resguardante_Bien_Mueble_Click
        ///DESCRIPCIÓN: Ejecuta la Busqueda de los bienes muebles por resguardos.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 18/Noviembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Resguardante_Bien_Mueble_Click(object sender, ImageClickEventArgs e) {
            try {
                Session["FILTRO_BUSQUEDA"] = "RESGUARDANTES";
                Llenar_Grid_Listado_Bienes_Bien_Mueble(0);
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Datos_Bien_Mueble_Click
        ///DESCRIPCIÓN: Ejecuta la Busqueda de los Bienes muebles por datos generales.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 18/Noviembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Datos_Bien_Mueble_Click(object sender, ImageClickEventArgs e) {
            try {
                Session["FILTRO_BUSQUEDA"] = "DATOS_GENERALES";
                Llenar_Grid_Listado_Bienes_Bien_Mueble(0);
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Limpiar_Filtros_Buscar_Datos_Vehiculo_Click
        ///DESCRIPCIÓN          : Hace la limpieza de los campos.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 18/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Btn_Limpiar_Filtros_Buscar_Datos_Vehiculo_Click(object sender, ImageClickEventArgs e) {
            try {
                Txt_Busqueda_Vehiculo_Numero_Inventario.Text = "";
                Txt_Busqueda_Vehiculo_Numero_Economico.Text = "";
                Txt_Busqueda_Vehiculo_Modelo.Text = "";
                Cmb_Busqueda_Vehiculo_Marca.SelectedIndex = 0;
                Cmb_Busqueda_Vehiculo_Tipo_Vehiculo.SelectedIndex = 0;
                Cmb_Busqueda_Vehiculo_Tipo_Combustible.SelectedIndex = 0;
                Txt_Busqueda_Vehiculo_Anio_Fabricacion.Text = "";
                Cmb_Busqueda_Vehiculo_Color.SelectedIndex = 0;
                Cmb_Busqueda_Vehiculo_Zonas.SelectedIndex = 0;
                Cmb_Busqueda_Vehiculo_Estatus.SelectedIndex = 0;
                Cmb_Busqueda_Vehiculo_Dependencias.SelectedIndex = 0;
                MPE_Busqueda_Vehiculo.Show();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Excepción.";
                Lbl_Mensaje_Error.Text = "Ex:['" + Ex.Message + "']";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Limpiar_Filtros_Buscar_Resguardante_Vehiculo_Click
        ///DESCRIPCIÓN          : Hace la limpieza de los campos.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 18/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Btn_Limpiar_Filtros_Buscar_Resguardante_Vehiculo_Click(object sender, ImageClickEventArgs e)  {
            try {
                Txt_Busqueda_Vehiculo_RFC_Resguardante.Text = "";
                Txt_Busqueda_Vehiculo_No_Empleado.Text = "";
                Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias.SelectedIndex = 0;
                Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias_SelectedIndexChanged(Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias, null);
                Cmb_Busqueda_Vehiculo_Nombre_Resguardante.SelectedIndex = 0;
                MPE_Busqueda_Vehiculo.Show();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Excepción.";
                Lbl_Mensaje_Error.Text = "Ex:['" + Ex.Message + "']";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Resguardante_Vehiculo_Click
        ///DESCRIPCIÓN: Ejecuta la Busqueda de los vehiculos por resguardos.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 18/Noviembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Resguardante_Vehiculo_Click(object sender, ImageClickEventArgs e) {
            try {
                Session["FILTRO_BUSQUEDA"] = "RESGUARDANTES";
                Llenar_Grid_Listado_Bienes_Vehiculos(0);
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Datos_Vehiculo_Click
        ///DESCRIPCIÓN: Ejecuta la Busqueda de los Vehiculos por datos generales.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 18/Noviembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Datos_Vehiculo_Click(object sender, ImageClickEventArgs e) {
            try {
                Session["FILTRO_BUSQUEDA"] = "DATOS_GENERALES";
                Llenar_Grid_Listado_Bienes_Vehiculos(0);
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Lnk_Busqueda_Avanzada_Click
        ///DESCRIPCIÓN: Lanza la Ventana de la busqueda de Bienes Sin Inventario.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 21/Noviembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Lnk_Busqueda_Avanzada_Click(object sender, EventArgs e) {
            try {
                MPE_Busqueda_BSI.Show();
                Grid_Listado_Bienes_Sin_Inventario.DataSource = new DataTable();
                Grid_Listado_Bienes_Sin_Inventario.DataBind();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Excepción.";
                Lbl_Mensaje_Error.Text = "Ex:['" + Ex.Message + "']";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Ejecutar_Busqueda_Bienes_Sin_Inventario_Click
        ///DESCRIPCIÓN: Ejecuta la Busqueda de los Bienes Sin Inventario.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 22/Noviembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Ejecutar_Busqueda_Bienes_Sin_Inventario_Click(object sender, ImageClickEventArgs e) {
            try { 
                Llenar_Grid_Listado_Bienes_Sin_Inventario(0);
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Limpiar_Campos_Busqueda_Bienes_Sin_Inventario_Click
        ///DESCRIPCIÓN          : Hace la limpieza de los campos.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 22/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Btn_Limpiar_Campos_Busqueda_Bienes_Sin_Inventario_Click(object sender, ImageClickEventArgs e)  {
            try {
                Txt_Busqueda_BSI_Nombre.Text = "";
                Cmb_Busqueda_BSI_Marca.SelectedIndex = 0;
                Txt_Busqueda_BSI_Modelo.Text = "";
                Cmb_Busqueda_BSI_Material.SelectedIndex = 0;
                Cmb_Busqueda_BSI_Color.SelectedIndex = 0;
                Txt_Busqueda_BSI_Numero_Serie.Text = "";
                Cmb_Busqueda_BSI_Estado.SelectedIndex = 0;
                Cmb_Busqueda_BSI_Estatus.SelectedIndex = 0;
                Cmb_Busqueda_BSI_Tipo_Parent.SelectedIndex = 0;
                Cmb_Busqueda_BSI_Tipo_Parent_SelectedIndexChanged(Cmb_Busqueda_BSI_Tipo_Parent, null);
                Txt_Busqueda_BSI_No_Inv_Parent.Text = "";
                Txt_Busqueda_BSI_RFC_Resguardante.Text = "";
                Txt_Busqueda_BSI_No_Empleado_Resguardante.Text = "";
                Cmb_Busqueda_BSI_Dependencia_Resguardante.SelectedIndex = 0;
                Cmb_Busqueda_BSI_Nombre_Resguardante.SelectedIndex = 0;
                Cmb_Busqueda_BSI_Dependencia_Resguardante_SelectedIndexChanged(Cmb_Busqueda_BSI_Dependencia_Resguardante, null);
                MPE_Busqueda_BSI.Show();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Excepción.";
                Lbl_Mensaje_Error.Text = "Ex:['" + Ex.Message + "']";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Cmb_Busqueda_Bien_Mueble_Resguardantes_Dependencias_SelectedIndexChanged
        ///DESCRIPCIÓN          : Maneja el evento del Combo de Dependencias.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 22/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Cmb_Busqueda_Bien_Mueble_Resguardantes_Dependencias_SelectedIndexChanged(object sender, EventArgs e) {
            if (Cmb_Busqueda_Bien_Mueble_Resguardantes_Dependencias.SelectedIndex > 0) {
                Llenar_Combo_Empleados(Cmb_Busqueda_Bien_Mueble_Resguardantes_Dependencias.SelectedItem.Value.Trim(), ref Cmb_Busqueda_Bien_Mueble_Nombre_Resguardante);
            } else {
                Llenar_Combo_Empleados(null, ref Cmb_Busqueda_Bien_Mueble_Nombre_Resguardante);
            }
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias_SelectedIndexChanged
        ///DESCRIPCIÓN          : Maneja el evento del Combo de Dependencias.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 22/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias_SelectedIndexChanged(object sender, EventArgs e) {
            if (Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias.SelectedIndex > 0)
            {
                Llenar_Combo_Empleados(Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias.SelectedItem.Value.Trim(), ref Cmb_Busqueda_Vehiculo_Nombre_Resguardante);
            } else {
                Llenar_Combo_Empleados(null, ref Cmb_Busqueda_Vehiculo_Nombre_Resguardante);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Cmb_Busqueda_BSI_Dependencia_Resguardante_SelectedIndexChanged
        ///DESCRIPCIÓN          : Maneja el evento del Combo de Estatus.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 22/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Cmb_Busqueda_BSI_Dependencia_Resguardante_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cmb_Busqueda_BSI_Dependencia_Resguardante.SelectedIndex > 0)
            {
                Llenar_Combo_Empleados(Cmb_Busqueda_BSI_Dependencia_Resguardante.SelectedItem.Value.Trim(), ref Cmb_Busqueda_BSI_Nombre_Resguardante);
            }
            else
            {
                Llenar_Combo_Empleados(null, ref Cmb_Busqueda_BSI_Nombre_Resguardante);
            }
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Cmb_Busqueda_BSI_Tipo_Parent_SelectedIndexChanged
        ///DESCRIPCIÓN          : Maneja el evento del Combo de Estatus.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 22/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Cmb_Busqueda_BSI_Tipo_Parent_SelectedIndexChanged(object sender, EventArgs e) {
            if (Cmb_Busqueda_BSI_Tipo_Parent.SelectedIndex > 0) {
                Txt_Busqueda_BSI_No_Inv_Parent.Enabled = true;
                Txt_Busqueda_BSI_RFC_Resguardante.Enabled = true;
                Txt_Busqueda_BSI_No_Empleado_Resguardante.Enabled = true;
                Cmb_Busqueda_BSI_Dependencia_Resguardante.Enabled = true;
                Cmb_Busqueda_BSI_Nombre_Resguardante.Enabled = true;
            } else {
                Txt_Busqueda_BSI_No_Inv_Parent.Text = "";
                Txt_Busqueda_BSI_No_Inv_Parent.Enabled = false;
                Txt_Busqueda_BSI_RFC_Resguardante.Text = "";
                Txt_Busqueda_BSI_RFC_Resguardante.Enabled = false;
                Txt_Busqueda_BSI_No_Empleado_Resguardante.Text = "";
                Txt_Busqueda_BSI_No_Empleado_Resguardante.Enabled = false;
                Cmb_Busqueda_BSI_Dependencia_Resguardante.Enabled = false;
                Cmb_Busqueda_BSI_Dependencia_Resguardante.SelectedIndex = 0;
                Llenar_Combo_Empleados(null, ref Cmb_Busqueda_BSI_Nombre_Resguardante);
                Cmb_Busqueda_BSI_Nombre_Resguardante.Enabled = false;
                Cmb_Busqueda_BSI_Nombre_Resguardante.SelectedIndex = 0;
            }
        }

    #endregion

}
