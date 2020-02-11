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
using Presidencia.Control_Patrimonial_Operacion_Bienes_Muebles.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Vehiculos.Negocio;
using Presidencia.Catalogo_Compras_Marcas.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Materiales.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Colores.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Tipos_Vehiculo.Negocio;
using Presidencia.Dependencias.Negocios;
using Presidencia.Constantes;
using Presidencia.Catalogo_Compras_Proveedores.Negocio;

public partial class paginas_Control_Patrimonial_Frm_Ope_Pat_Asignacion_Liberacion_Bienes_Muebles : System.Web.UI.Page {

    #region "Page Load"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Page_Load
        ///DESCRIPCIÓN          : Se ejecuta al iniciar la página.
        ///PARAMETROS           :
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : Marzo/2012 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Page_Load(object sender, EventArgs e) {
            Div_Contenedor_Msj_Error.Visible = false;
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Trim().Length == 0) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!IsPostBack) {
                Llenar_Combos_Independientes();
                Llenar_Combos_Bienes_Muebles();
                Llenar_Combos_Vehiculos();
                Habilitar_Campos_Formulario("NORMAL");
            }
        }

    #endregion

    #region "Metodos"

        #region "Limpiar"

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Limpiar_Campos_Vehiculo_Parent
            ///DESCRIPCIÓN          : Limpia los campos de la capa de Vehiculo.
            ///PARAMETROS           :
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : Marzo/2012 
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
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Limpiar_Campos_Vehiculo_Parent
            ///DESCRIPCIÓN          : Limpia los campos de la capa de Bien Mueble.
            ///PARAMETROS           : 
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : Marzo/2012 
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
            }
            
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Limpiar_Generales
            ///DESCRIPCIÓN: Se Limpian los campos Generales de los Bienes Muebles.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO           : Marzo/2012 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Limpiar_Generales() {
                try {
                    Hdf_Bien_Mueble_ID.Value = "";
                    Hdf_Lanza_Busqueda.Value = "";
                    Hdf_Bien_Parent_ID.Value = "";
                    Txt_Nombre_Producto.Text = "";
                    Txt_Resguardo_Recibo.Text = "";
                    Cmb_Dependencias.SelectedIndex = 0;
                    Cmb_Zonas.SelectedIndex = 0;
                    Txt_Modelo.Text = "";
                    Cmb_Marca.SelectedIndex = 0;
                    Cmb_Materiales.SelectedIndex = 0;
                    Cmb_Colores.SelectedIndex = 0;
                    Txt_Numero_Serie.Text = "";
                    Txt_Observaciones.Text = "";
                    Txt_Numero_Inventario.Text = "";
                    Txt_Invenario_Anterior.Text = "";
                    Grid_Listado_Resguardantes.DataSource = new DataTable();
                    Grid_Listado_Resguardantes.DataBind();
                    Grid_Listado_Resguardantes.SelectedIndex = (-1);
                    Limpiar_Campos_Bien_Mueble_Parent();
                    Limpiar_Campos_Vehiculo_Parent();
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
        
        #endregion

        #region "Llenar Datos"
        
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Resultados_Multiples
            ///DESCRIPCIÓN: Se llenan el Grid de Resultados Multiples del Modal de Busqueda 
            ///             dependiendo de los filtros pasados.
            ///PROPIEDADES:  Datos.  Fuente de donde se llenara el Grid.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: Marzo/2012 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Grid_Resultados_Multiples(DataTable Datos) {
                try{
                    Grid_Resultados_Multiples.Columns[1].Visible = true;
                    Grid_Resultados_Multiples.DataSource = Datos;
                    Grid_Resultados_Multiples.DataBind();
                    Grid_Resultados_Multiples.Columns[1].Visible = false;
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Llenar_Grid_Listado_Bienes_Bien_Mueble
            ///DESCRIPCIÓN          : Llena el Grid de los Bienes Muebles.
            ///PARAMETROS           : 
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : Marzo/2012 
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
            ///FECHA_CREO           : Marzo/2012 
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
            ///NOMBRE DE LA FUNCIÓN : Llenar_Grid_Resguardos
            ///DESCRIPCIÓN          : Llena el Grid de los Resguardos.
            ///PARAMETROS           : 1. Dt_Resguardos. Tabla de Resguardos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : Marzo/2012 
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
            ///NOMBRE DE LA FUNCIÓN : Llenar_Combo_Empleados
            ///DESCRIPCIÓN          : Llena el Combo con los empleados de una dependencia.
            ///PARAMETROS           : 
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : Marzo/2012 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Llenar_Combo_Empleados(String Dependencia_ID, ref DropDownList Combo_Empleados) {
                Combo_Empleados.Items.Clear();
                if (Dependencia_ID != null && Dependencia_ID.Trim().Length > 0) {
                    Cls_Ope_Pat_Com_Bienes_Muebles_Negocio BSI_Negocio = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                    BSI_Negocio.P_Tipo_DataTable = "EMPLEADOS";
                    BSI_Negocio.P_Dependencia_ID = Dependencia_ID.Trim();
                    DataTable Dt_Datos = BSI_Negocio.Consultar_DataTable();
                    Combo_Empleados.DataSource = Dt_Datos;
                    Combo_Empleados.DataValueField = "EMPLEADO_ID";
                    Combo_Empleados.DataTextField = "NOMBRE";
                    Combo_Empleados.DataBind();
                } 
                Combo_Empleados.Items.Insert(0, new ListItem("< TODOS >", ""));
            }
    
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Llenar_Combos_Vehiculos
            ///DESCRIPCIÓN          : Llena los Combos de Vehículos en el Formulario.
            ///PARAMETROS           : 
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : Marzo/2012 
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
            ///NOMBRE DE LA FUNCIÓN : Llenar_Combos_Bienes_Muebles
            ///DESCRIPCIÓN          : Llena los Combos de Bienes Muebles en el Formulario.
            ///PARAMETROS           : 
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : Marzo/2012 
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
            ///NOMBRE DE LA FUNCIÓN: Llenar_Combos
            ///DESCRIPCIÓN: Se llenan los Combos Generales Independientes.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: Marzo/2012 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Combos_Independientes() {
                try {
                    Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Combos = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();

                    //SE LLENA EL COMBO DE MATERIALES
                    Combos.P_Tipo_DataTable = "MATERIALES";
                    DataTable Materiales = Combos.Consultar_DataTable();
                    Cmb_Materiales.DataSource = Materiales.Copy();
                    Cmb_Materiales.DataValueField = "MATERIAL_ID";
                    Cmb_Materiales.DataTextField = "DESCRIPCION";
                    Cmb_Materiales.DataBind();
                    Cmb_Materiales.Items.Insert(0, new ListItem("-----------", ""));

                    Combos.P_Tipo_DataTable = "COLORES";
                    DataTable Colores = Combos.Consultar_DataTable();
                    DataRow Fila_Color = Colores.NewRow();
                    Fila_Color["COLOR_ID"] = "SELECCIONE";
                    Fila_Color["DESCRIPCION"] = HttpUtility.HtmlDecode("-----------");
                    Colores.Rows.InsertAt(Fila_Color, 0);
                    Cmb_Colores.DataSource = Colores.Copy();
                    Cmb_Colores.DataValueField = "COLOR_ID";
                    Cmb_Colores.DataTextField = "DESCRIPCION";
                    Cmb_Colores.DataBind();

                    //SE LLENA EL COMBO DE MARCAS
                    Combos.P_Tipo_DataTable = "MARCAS";
                    DataTable Marcas = Combos.Consultar_DataTable();
                    Cmb_Marca.DataSource = Marcas.Copy();
                    Cmb_Marca.DataValueField = "MARCA_ID";
                    Cmb_Marca.DataTextField = "NOMBRE";
                    Cmb_Marca.DataBind();
                    Cmb_Marca.Items.Insert(0, new ListItem("-----------", ""));

                    //SE LLENA EL COMBO DE DEPENDENCIAS
                    Combos.P_Tipo_DataTable = "DEPENDENCIAS";
                    DataTable Dependencias = Combos.Consultar_DataTable();
                    Cmb_Dependencias.DataSource = Dependencias.Copy();
                    Cmb_Dependencias.DataValueField = "DEPENDENCIA_ID";
                    Cmb_Dependencias.DataTextField = "NOMBRE";
                    Cmb_Dependencias.DataBind();
                    Cmb_Dependencias.Items.Insert(0, new ListItem("-----------", ""));

                    Combos.P_Tipo_DataTable = "ZONAS";
                    DataTable Zonas = Combos.Consultar_DataTable();
                    Zonas.DefaultView.Sort = "DESCRIPCION";
                    Cmb_Zonas.DataSource = Zonas.Copy();
                    Cmb_Zonas.DataTextField = "DESCRIPCION";
                    Cmb_Zonas.DataValueField = "ZONA_ID";
                    Cmb_Zonas.DataBind();
                    Cmb_Zonas.Items.Insert(0, new ListItem("-----------", ""));

                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

        #endregion

        #region "Mostrar Datos"
    
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Mostrar_Detalles_Vehiculo
            ///DESCRIPCIÓN          : Muestra la información de un Vehículo Parent.
            ///PARAMETROS           : Vehiculo. Datos a Cargar.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : Marzo/2012 
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
            ///DESCRIPCIÓN          : Muestra la información de un Bien Mueble Parent.
            ///PARAMETROS           : Bien_Mueble. Datos a Cargar.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : Marzo/2012 
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
            ///FECHA_CREO           : Marzo/2012 
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
    
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Mostrar_Detalles_Bien_Mueble
            ///DESCRIPCIÓN: Muestra los datos del Bien Mueble a Cargar.
            ///PROPIEDADES:     
            ///             1. Bien_Mueble. Contiene los parametros que se desean mostrar.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: Marzo/2012 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Mostrar_Detalles_Bien_Mueble_Principal(Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Mueble){
                try {
                    Hdf_Bien_Mueble_ID.Value = Bien_Mueble.P_Bien_Mueble_ID;
                    Txt_Nombre_Producto.Text = Bien_Mueble.P_Nombre_Producto;
                    for (Int32 Contador = 0; Contador < Bien_Mueble.P_Resguardantes.Rows.Count; Contador++) {
                        Cmb_Dependencias.SelectedIndex = Cmb_Dependencias.Items.IndexOf(Cmb_Dependencias.Items.FindByValue(Bien_Mueble.P_Resguardantes.Rows[Contador]["DEPENDENCIA_ID"].ToString().Trim()));
                    }
                    //Cmb_Dependencias_SelectedIndexChanged(Cmb_Dependencias, null);
                    Cmb_Marca.SelectedIndex = Cmb_Marca.Items.IndexOf(Cmb_Marca.Items.FindByValue(Bien_Mueble.P_Marca_ID));
                    Txt_Modelo.Text = Bien_Mueble.P_Modelo;
                    Cmb_Materiales.SelectedIndex = Cmb_Materiales.Items.IndexOf(Cmb_Materiales.Items.FindByValue(Bien_Mueble.P_Material_ID));
                    Cmb_Colores.SelectedIndex = Cmb_Colores.Items.IndexOf(Cmb_Colores.Items.FindByValue(Bien_Mueble.P_Color_ID));
                    Txt_Numero_Serie.Text = Bien_Mueble.P_Numero_Serie;
                    Cmb_Zonas.SelectedIndex = Cmb_Zonas.Items.IndexOf(Cmb_Zonas.Items.FindByValue(Bien_Mueble.P_Zona));
                    Txt_Observaciones.Text = Bien_Mueble.P_Observaciones;
                    Txt_Numero_Inventario.Text = Bien_Mueble.P_Numero_Inventario;
                    Txt_Invenario_Anterior.Text = Bien_Mueble.P_Numero_Inventario_Anterior;
                    Txt_Resguardo_Recibo.Text = Bien_Mueble.P_Operacion.Trim();
                    Llenar_Grid_Resguardos(Bien_Mueble.P_Resguardantes);
                    Cmb_Tipo_Parent.SelectedIndex = Cmb_Tipo_Parent.Items.IndexOf(Cmb_Tipo_Parent.Items.FindByValue(Bien_Mueble.P_Proveniente));
                    Cmb_Tipo_Parent_SelectedIndexChanged(Cmb_Tipo_Parent, null);
                    if (Bien_Mueble.P_Ascencendia != null && Bien_Mueble.P_Ascencendia.Trim().Length > 0) {
                        if (Bien_Mueble.P_Proveniente != null && Bien_Mueble.P_Proveniente.Trim().Equals("BIEN_MUEBLE")) {
                            Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Parent = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                            Bien_Parent.P_Bien_Mueble_ID = Bien_Mueble.P_Ascencendia;
                            Bien_Parent = Bien_Parent.Consultar_Detalles_Bien_Mueble();
                            Mostrar_Detalles_Bien_Mueble(Bien_Parent);
                            Div_Bien_Mueble_Parent.Visible = true;
                        } else if (Bien_Mueble.P_Proveniente != null && Bien_Mueble.P_Proveniente.Trim().Equals("VEHICULO")) {
                            Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
                            Vehiculo.P_Vehiculo_ID = Bien_Mueble.P_Ascencendia;
                            Vehiculo = Vehiculo.Consultar_Detalles_Vehiculo();
                            Mostrar_Detalles_Vehiculo(Vehiculo);
                            Div_Vehiculo_Parent.Visible = true;
                        }
                    } else {
                        Limpiar_Campos_Bien_Mueble_Parent();
                        Limpiar_Campos_Vehiculo_Parent();
                        Div_Bien_Mueble_Parent.Visible = false;
                        Div_Vehiculo_Parent.Visible = false;
                    }
                    System.Threading.Thread.Sleep(1000);
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

        #endregion

    #endregion

    #region "Grids"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Productos_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView de los Vehiculos
        ///PROPIEDADES:     
        ///CREO : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
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
        ///CREO : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
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
        ///CREO : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
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
        ///CREO : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
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
                    if (Hdf_Lanza_Busqueda.Value.Equals("PARENT")) { 
                        Mostrar_Detalles_Bien_Mueble(Bien_Mueble);
                    } else if (Hdf_Lanza_Busqueda.Value.Equals("ACTUAL")) {
                        Mostrar_Detalles_Bien_Mueble_Principal(Bien_Mueble);
                    }
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
        ///NOMBRE DE LA FUNCIÓN: Grid_Resultados_Multiples_SelectedIndexChanged
        ///DESCRIPCIÓN: Maneja el evento de cambio de Seleccion del GridView de Bienes del
        ///             Modal de Resultados Multiples.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************
        protected void Grid_Resultados_Multiples_SelectedIndexChanged(object sender, EventArgs e) {
            try{
                if (Grid_Resultados_Multiples.SelectedIndex > (-1)) {
                    Limpiar_Generales();
                    String Bien_Mueble_Seleccionado_ID = Grid_Resultados_Multiples.SelectedRow.Cells[1].Text.Trim();
                    Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Mueble = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                    Bien_Mueble.P_Bien_Mueble_ID = Bien_Mueble_Seleccionado_ID;
                    Bien_Mueble = Bien_Mueble.Consultar_Detalles_Bien_Mueble();
                    Mostrar_Detalles_Bien_Mueble_Principal(Bien_Mueble);
                    Grid_Resultados_Multiples.SelectedIndex = -1;
                    Mpe_Multiples_Resultados.Hide();
                    System.Threading.Thread.Sleep(500);
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }            
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Resultados_Multiples_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView de Resultados Multiples.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Resultados_Multiples_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                Grid_Resultados_Multiples.PageIndex = e.NewPageIndex;
                Btn_Buscar_Click(Btn_Buscar, null);
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

    #endregion

    #region "Eventos"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Lnk_Busqueda_Avanzada_Click
        ///DESCRIPCIÓN: Lanza la Ventana de la busqueda de Bienes Sin Inventario.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO : Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Lnk_Busqueda_Avanzada_Click(object sender, EventArgs e) { 
            try {
                Hdf_Lanza_Busqueda.Value = "ACTUAL";
                MPE_Busqueda_Bien_Mueble.Show();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Excepción.";
                Lbl_Mensaje_Error.Text = "Ex:['" + Ex.Message + "']";
                Div_Contenedor_Msj_Error.Visible = true;
            } 
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Vehiculo_Click
        ///DESCRIPCIÓN: Busca el Vehiculo a Asignar.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO : Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
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
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Bien_Mueble_Click
        ///DESCRIPCIÓN: Busca el Bien Mueble a Asignar.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO : Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Bien_Mueble_Click(object sender, ImageClickEventArgs e) { 
            try {
                Hdf_Lanza_Busqueda.Value = "PARENT";
                MPE_Busqueda_Bien_Mueble.Show();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Excepción.";
                Lbl_Mensaje_Error.Text = "Ex:['" + Ex.Message + "']";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Resguardante_Bien_Mueble_Click
        ///DESCRIPCIÓN: Ejecuta la Busqueda de los bienes muebles por resguardos.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO : Marzo/2012 
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
        ///FECHA_CREO : Marzo/2012 
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
        ///FECHA_CREO           : Marzo/2012 
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
        ///FECHA_CREO           : Marzo/2012 
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
        ///FECHA_CREO: Marzo/2012 
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
        ///FECHA_CREO: Marzo/2012 
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
        ///NOMBRE DE LA FUNCIÓN : Cmb_Busqueda_Bien_Mueble_Resguardantes_Dependencias_SelectedIndexChanged
        ///DESCRIPCIÓN          : Maneja el evento del Combo de Dependencias.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : Marzo/2012 
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
        ///FECHA_CREO           : Marzo/2012 
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
        ///NOMBRE DE LA FUNCIÓN : Btn_Limpiar_Filtros_Buscar_Datos_Bien_Mueble_Click
        ///DESCRIPCIÓN          : Hace la limpieza de los campos.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : Marzo/2012 
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
        ///FECHA_CREO           : Marzo/2012 
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
        ///NOMBRE DE LA FUNCIÓN : Cmb_Tipo_Parent_SelectedIndexChanged
        ///DESCRIPCIÓN          : Carga la Configuración a mostrar de las capas
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : Marzo/2012 
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
                Div_Resguardos.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
        ///DESCRIPCIÓN: Ejecuta la modificacion.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e) {
            if (Btn_Modificar.AlternateText.Equals("Modificar")) { 
                if (Hdf_Bien_Mueble_ID.Value.Length > 0) {
                    Habilitar_Campos_Formulario("MODIFICADO");
                } else {
                    Lbl_Ecabezado_Mensaje.Text = "VERIFICAR.";
                    Lbl_Mensaje_Error.Text = "NO SE TIENE SELECCIONADO EL BIEN A MODIFICAR";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            } else {
                Cls_Ope_Pat_Com_Bienes_Muebles_Negocio BI_Negocio = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                BI_Negocio.P_Bien_Mueble_ID = Hdf_Bien_Mueble_ID.Value;
                BI_Negocio = BI_Negocio.Consultar_Detalles_Bien_Mueble();
                BI_Negocio.P_Proveniente = Cmb_Tipo_Parent.SelectedItem.Value;
                BI_Negocio.P_Ascencendia = Hdf_Bien_Parent_ID.Value.Trim();
                BI_Negocio.P_Resguardantes = Obtener_Resguardantes_Actuales();
                BI_Negocio.P_Usuario_Nombre = Cls_Sessiones.Nombre_Empleado;
                BI_Negocio.P_Usuario_ID = Cls_Sessiones.Empleado_ID;
                BI_Negocio.Modificar_Bien_Mueble_Secundarios();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "a", "alert('Actualización Exitosa');", true);
                BI_Negocio = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                BI_Negocio.P_Bien_Mueble_ID = Hdf_Bien_Mueble_ID.Value;
                BI_Negocio = BI_Negocio.Consultar_Detalles_Bien_Mueble();
                Habilitar_Campos_Formulario("NORMAL");
                Limpiar_Generales();
                Mostrar_Detalles_Bien_Mueble_Principal(BI_Negocio);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Habilitar_Campos_Formulario
        ///DESCRIPCIÓN: Controla la habilitacion e inhabilitacion de los campos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Habilitar_Campos_Formulario(String Estatus) {
            if (Estatus.Trim().Equals("NORMAL")) {
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Modificar.AlternateText = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Salir.ToolTip = "Salir";
                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Div_Busqueda.Visible = true;
                Cmb_Tipo_Parent.Enabled = false;
                Cmb_Tipo_Parent_SelectedIndexChanged(Cmb_Tipo_Parent, null);
            } else if (Estatus.Trim().Equals("MODIFICADO")) { 
                Btn_Modificar.ToolTip = "Guardar";
                Btn_Modificar.AlternateText = "Guardar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.ToolTip = "Cancelar Operación de Actualización";
                Btn_Salir.AlternateText = "Cancelar Operación de Actualización";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Div_Busqueda.Visible = false;
                Cmb_Tipo_Parent.Enabled = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
        ///DESCRIPCIÓN: Carga el Modal Popup de Busqueda Directa.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Salir_Click(object sender, ImageClickEventArgs e) {
            if (Btn_Salir.AlternateText.Equals("Salir")) {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            } else {
                Habilitar_Campos_Formulario("NORMAL");
                Limpiar_Generales();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
        ///DESCRIPCIÓN: Carga el Modal Popup de Busqueda Directa.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private DataTable Obtener_Resguardantes_Actuales() {
            DataTable Tabla = new DataTable();
            Tabla.Columns.Add("BIEN_RESGUARDO_ID", Type.GetType("System.String"));
            Tabla.Columns.Add("EMPLEADO_ID", Type.GetType("System.String"));
            Tabla.Columns.Add("NO_EMPLEADO", Type.GetType("System.String"));
            Tabla.Columns.Add("NOMBRE_EMPLEADO", Type.GetType("System.String"));
            Tabla.Columns.Add("COMENTARIOS", Type.GetType("System.String"));
            foreach (GridViewRow Fila_Grid in Grid_Listado_Resguardantes.Rows) {
                DataRow Fila = Tabla.NewRow();
                Fila["BIEN_RESGUARDO_ID"] = 0;
                Fila["EMPLEADO_ID"] = HttpUtility.HtmlDecode(Fila_Grid.Cells[0].Text.Trim());
                Fila["NO_EMPLEADO"] = HttpUtility.HtmlDecode(Fila_Grid.Cells[1].Text.Trim());
                Fila["NOMBRE_EMPLEADO"] = HttpUtility.HtmlDecode(Fila_Grid.Cells[2].Text.Trim());
                Fila["COMENTARIOS"] = HttpUtility.HtmlDecode("");
                Tabla.Rows.Add(Fila);
            }
            return Tabla;
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
        ///DESCRIPCIÓN: Carga el Modal Popup de Busqueda Directa.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e) {
            try {
                if (Txt_Busqueda.Text.Trim().Length > 0 || Txt_Busqueda_Anterior.Text.Trim().Length > 0) {
                    Limpiar_Generales();
                    Boolean Multiples_Resultados = false;
                    String No_Inventario_SIAS = Txt_Busqueda.Text.Trim();
                    String No_Inventario_Anterior = Txt_Busqueda_Anterior.Text.Trim();
                    if (No_Inventario_Anterior.Trim().Length > 0 || No_Inventario_SIAS.Trim().Length > 0) {
                        Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Mueble_Negocio = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                        Bien_Mueble_Negocio.P_Tipo_DataTable = "BIENES";
                        Bien_Mueble_Negocio.P_Numero_Inventario_Anterior = No_Inventario_Anterior.Trim();
                        Bien_Mueble_Negocio.P_Numero_Inventario = No_Inventario_SIAS.Trim();
                        DataTable Dt_Temporal = Bien_Mueble_Negocio.Consultar_DataTable();
                        if (Dt_Temporal.Rows.Count > 1) {
                            Multiples_Resultados = true;
                            Llenar_Grid_Resultados_Multiples(Dt_Temporal);
                            Mpe_Multiples_Resultados.Show();
                            return;
                        } else if (Dt_Temporal.Rows.Count == 1) {
                            No_Inventario_Anterior = Dt_Temporal.Rows[0]["NO_INVENTARIO_ANTERIOR"].ToString().Trim();
                        }
                    }
                    if (!Multiples_Resultados) {
                        Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Mueble = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                        Bien_Mueble.P_Numero_Inventario = No_Inventario_SIAS;
                        Bien_Mueble.P_Numero_Inventario_Anterior = No_Inventario_Anterior;
                        Bien_Mueble.P_Buscar_Numero_Inventario = true;
                        Bien_Mueble = Bien_Mueble.Consultar_Detalles_Bien_Mueble();

                        String Operacion = Bien_Mueble.P_Operacion;
                        Txt_Resguardo_Recibo.Text = Operacion;

                        if (Bien_Mueble.P_Bien_Mueble_ID != null && Bien_Mueble.P_Bien_Mueble_ID.Trim().Length > 0) {
                            Mostrar_Detalles_Bien_Mueble_Principal(Bien_Mueble);
                        } else {
                            Lbl_Ecabezado_Mensaje.Text = HttpUtility.HtmlDecode("No se encontro un Bien Mueble con el Número de Inventario.");
                            Lbl_Mensaje_Error.Text = "";
                            Div_Contenedor_Msj_Error.Visible = true;
                        } 
                    } 
                } else {
                    Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                    Lbl_Mensaje_Error.Text = "Introducir el Número de Inventario a Buscar";
                    Div_Contenedor_Msj_Error.Visible = true;                    
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