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
using Presidencia.Control_Patrimonial_Operacion_Partes_Vehiculos.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Vehiculos.Negocio;
using Presidencia.Catalogo_Compras_Productos.Negocio;
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Compras_Marcas.Negocio;

public partial class paginas_Compras_Ope_Pat_Partes_Vehiculos : System.Web.UI.Page {

    #region Page_Load

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Evento que se carga al cargar la pagina
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 26/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Page_Load(object sender, EventArgs e) {
            Div_Contenedor_Msj_Error.Visible = false;
            if (!IsPostBack) {
                Llenar_Combos();
                Llenar_Combos_MPE_Busqueda_Vehiculos();
                Llenar_Combos_MPE_Busqueda_Partes();
                Configuracion_Formulario(true, "");
                Grid_Listado_Vehiculos.Columns[1].Visible = false;
            }
        }

    #endregion

    #region Metodos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combos
        ///DESCRIPCIÓN: Se llenan los combos generales del Formulario.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 28/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public void Llenar_Combos() {
            try {
                Cls_Ope_Pat_Com_Vehiculos_Negocio Combos = new Cls_Ope_Pat_Com_Vehiculos_Negocio();

                //SE LLENA EL COMBO DE MARCAS
                Combos.P_Tipo_DataTable = "MARCAS";
                DataTable Marcas = Combos.Consultar_DataTable();
                DataRow Fila_Marca = Marcas.NewRow();
                Fila_Marca["MARCA_ID"] = "SELECCIONE";
                Fila_Marca["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
                Marcas.Rows.InsertAt(Fila_Marca, 0);
                Cmb_Marca_Parte.DataSource = Marcas;
                Cmb_Marca_Parte.DataTextField = "NOMBRE";
                Cmb_Marca_Parte.DataValueField = "MARCA_ID";
                Cmb_Marca_Parte.DataBind();
                Marcas.Rows.RemoveAt(0);
                Fila_Marca["MARCA_ID"] = "TODAS";
                Fila_Marca["NOMBRE"] = HttpUtility.HtmlDecode("&lt;TODAS&gt;");
                Marcas.Rows.InsertAt(Fila_Marca, 0);
                Cmb_Marca_Producto.DataSource = Marcas;
                Cmb_Marca_Producto.DataTextField = "NOMBRE";
                Cmb_Marca_Producto.DataValueField = "MARCA_ID";
                Cmb_Marca_Producto.DataBind();

                //SE LLENA EL COMBO DE MODELOS
                Combos.P_Tipo_DataTable = "MODELOS";
                DataTable Modelos = Combos.Consultar_DataTable();
                DataRow Fila_Modelo = Modelos.NewRow();
                Fila_Modelo["MODELO_ID"] = "TODOS";
                Fila_Modelo["NOMBRE"] = HttpUtility.HtmlDecode("&lt;TODOS&gt;");
                Modelos.Rows.InsertAt(Fila_Modelo, 0);
                Cmb_Modelo_Producto.DataSource = Modelos;
                Cmb_Modelo_Producto.DataTextField = "NOMBRE";
                Cmb_Modelo_Producto.DataValueField = "MODELO_ID";
                Cmb_Modelo_Producto.DataBind();

                //SE LLENA EL COMBO DE MATERIALES
                Combos.P_Tipo_DataTable = "MATERIALES";
                DataTable Materiales = Combos.Consultar_DataTable();
                DataRow Fila_Material = Materiales.NewRow();
                Fila_Material["MATERIAL_ID"] = "SELECCIONE";
                Fila_Material["DESCRIPCION"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
                Materiales.Rows.InsertAt(Fila_Material, 0);
                Cmb_Material_Parte.DataSource = Materiales;
                Cmb_Material_Parte.DataTextField = "DESCRIPCION";
                Cmb_Material_Parte.DataValueField = "MATERIAL_ID";
                Cmb_Material_Parte.DataBind();

                //SE LLENA EL COMBO DE COLORES
                Combos.P_Tipo_DataTable = "COLORES";
                DataTable Colores = Combos.Consultar_DataTable();
                DataRow Fila_Color = Colores.NewRow();
                Fila_Color["COLOR_ID"] = "SELECCIONE";
                Fila_Color["DESCRIPCION"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
                Colores.Rows.InsertAt(Fila_Color, 0);
                Cmb_Color_Parte.DataSource = Colores;
                Cmb_Color_Parte.DataValueField = "COLOR_ID";
                Cmb_Color_Parte.DataTextField = "DESCRIPCION";
                Cmb_Color_Parte.DataBind();

                //SE LLENA EL COMBO DE DEPENDENCIAS
                Combos.P_Tipo_DataTable = "DEPENDENCIAS";
                DataTable Dependencias = Combos.Consultar_DataTable();
                DataRow Fila_Dependencia = Dependencias.NewRow();
                Fila_Dependencia["DEPENDENCIA_ID"] = "SELECCIONE";
                Fila_Dependencia["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
                Dependencias.Rows.InsertAt(Fila_Dependencia, 0);
                Cmb_Dependencia_Vehiculo.DataSource = Dependencias;
                Cmb_Dependencia_Vehiculo.DataValueField = "DEPENDENCIA_ID";
                Cmb_Dependencia_Vehiculo.DataTextField = "NOMBRE";
                Cmb_Dependencia_Vehiculo.DataBind();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Verificar";
                Lbl_Mensaje_Error.Text = "+ " + Ex.Message;
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Generales
        ///DESCRIPCIÓN: Limpia la parte general de las partes.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 26/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public void Limpiar_Producto() {
            Hdf_Producto_ID.Value = "";
            Txt_Nombre_Parte.Text = "";
            Cmb_Marca_Parte.SelectedIndex = 0;
            Txt_Modelo_Parte.Text = "";
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Vehiculo
        ///DESCRIPCIÓN: Limpia la parte general del Vehículo de la parte.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 05/Marzo/20101
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public void Limpiar_Vehiculo() {
            Hdf_Vehiculo_ID.Value = "";
            Txt_Nombre_Vehiculo.Text = "";
            Cmb_Dependencia_Vehiculo.SelectedIndex = 0;
            Txt_Numero_Inventario_Vehiculo.Text = "";
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Generales
        ///DESCRIPCIÓN: Limpia la parte general de las partes.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 26/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public void Limpiar_Generales() { 
            Hdf_Parte_ID.Value = "";
            Txt_Numero_Inventario_Parte.Text = "";
            Cmb_Material_Parte.SelectedIndex = 0;
            Cmb_Color_Parte.SelectedIndex = 0;
            Txt_Costo_Parte.Text = "";
            Txt_Fecha_Adquisicion_Parte.Text = "";
            Cmb_Estado_Parte.SelectedIndex = 0;
            Cmb_Estatus_Parte.SelectedIndex = 0;
            Txt_Comentarios_Parte.Text = "";
            Txt_Motivo_Baja_Parte.Text = "";
            Grid_Resguardos.DataSource = new DataTable();
            Grid_Resguardos.DataBind();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Historial_Resguardantes
        ///DESCRIPCIÓN: Se Limpian los campos de Historial de los Resguardantes de las 
        ///             Partes de Vehiculos.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 14/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Historial_Resguardantes() {
            try {
                Txt_Historial_Empleado_Resguardo.Text = "";
                Txt_Historial_Fecha_Inicial_Resguardo.Text = "";
                Txt_Historial_Fecha_Final_Resguardo.Text = "";
                Txt_Historial_Comentarios_Resguardo.Text = "";
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
        ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
        ///PROPIEDADES:     
        ///             1. Estatus. Estatus en el que se cargara la configuración de los 
        ///                         controles.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 26/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Configuracion_Formulario(Boolean Estatus, String Operacion) {
            if (Operacion.Trim().Equals("Nuevo")) {
                Btn_Nuevo.AlternateText = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Modificar.Visible = false;
                Cmb_Estatus_Parte.Enabled = false;
                Btn_Lanzar_Buscar_Producto.Visible = true;
                Btn_Lanzar_Buscar_Vehiculo.Visible = true;
                Lbl_Motivo_Baja_Parte.Visible = false;
                Txt_Motivo_Baja_Parte.Visible = false;
            } else if (Operacion.Trim().Equals("Modificar")) {
                Btn_Modificar.AlternateText = "Actualizar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Nuevo.Visible = false;
                Cmb_Estatus_Parte.Enabled = true;
                Btn_Lanzar_Buscar_Producto.Visible = false;
                Btn_Lanzar_Buscar_Vehiculo.Visible = true;
                Lbl_Motivo_Baja_Parte.Visible = true;
                Txt_Motivo_Baja_Parte.Visible = true;
            } else {
                Btn_Nuevo.AlternateText = "Nuevo";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                Btn_Modificar.AlternateText = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Cmb_Estatus_Parte.Enabled = false;
                Btn_Nuevo.Visible = true;
                Btn_Modificar.Visible = true;
                Btn_Lanzar_Buscar_Producto.Visible = false;
                Btn_Lanzar_Buscar_Vehiculo.Visible = false;
                Lbl_Motivo_Baja_Parte.Visible = false;
                Txt_Motivo_Baja_Parte.Visible = false;

                Configuracion_Acceso("Ope_Pat_Partes_Vehiculos.aspx");
                Configuracion_Acceso_LinkButton("Ope_Pat_Partes_Vehiculos.aspx");
            }
            Cmb_Marca_Parte.Enabled = !Estatus;
            Txt_Modelo_Parte.Enabled = !Estatus;
            Div_Busqueda.Visible = Estatus;
            Cmb_Material_Parte.Enabled = !Estatus;
            Cmb_Color_Parte.Enabled = !Estatus;
            Txt_Costo_Parte.Enabled = !Estatus;
            Btn_Fecha_Adquisicion_Parte.Enabled = !Estatus;
            Txt_Comentarios_Parte.Enabled = !Estatus;
            Cmb_Estado_Parte.Enabled = !Estatus;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Resguardos_Partes
        ///DESCRIPCIÓN: Llena la tabla de Resguardantes de las Partes
        ///PROPIEDADES:     
        ///             1.  Pagina. Pagina en la cual se mostrará el Grid_VIew
        ///             2.  Tabla.  Tabla que se va a cargar en el Grid.    
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 25/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Grid_Resguardos(Int32 Pagina, DataTable Tabla)
        {
            Grid_Resguardos.Columns[0].Visible = true;
            Grid_Resguardos.DataSource = Tabla;
            Grid_Resguardos.PageIndex = Pagina;
            Grid_Resguardos.DataBind();
            Grid_Resguardos.Columns[0].Visible = false;
            Session["Dt_Resguardados"] = Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Historial_Resguardos
        ///DESCRIPCIÓN: Llena la tabla de Historial de Resguardantes
        ///PROPIEDADES:     
        ///             1.  Pagina. Pagina en la cual se mostrará el Grid_VIew
        ///             2.  Tabla.  Tabla que se va a cargar en el Grid.    
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 14/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Grid_Historial_Resguardos(Int32 Pagina, DataTable Tabla) {
            Grid_Historial_Resguardantes.Columns[1].Visible = true;
            Grid_Historial_Resguardantes.DataSource = Tabla;
            Grid_Historial_Resguardantes.PageIndex = Pagina;
            Grid_Historial_Resguardantes.DataBind();
            Grid_Historial_Resguardantes.Columns[1].Visible = false;
            Session["Dt_Historial_Resguardos"] = Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Buscar_Clave_DataTable
        ///DESCRIPCIÓN: Busca una Clave en un DataTable, si la encuentra Retorna 'true'
        ///             en caso contrario 'false'.
        ///PROPIEDADES:  
        ///             1.  Clave.  Clave que se buscara en el DataTable
        ///             2.  Tabla.  Datatable donde se va a buscar la clave.
        ///             3.  Columna.Columna del DataTable donde se va a buscar la clave.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 03/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private Boolean Buscar_Clave_DataTable(String Clave, DataTable Tabla, Int32 Columna) {
            Boolean Resultado_Busqueda = false;
            if (Tabla != null && Tabla.Rows.Count > 0 && Tabla.Columns.Count > 0) {
                if (Tabla.Columns.Count > Columna) {
                    for (Int32 Contador = 0; Contador < Tabla.Rows.Count; Contador++) {
                        if (Tabla.Rows[Contador][Columna].ToString().Trim().Equals(Clave.Trim())) {
                            Resultado_Busqueda = true;
                            break;
                        }
                    }
                }
            }
            return Resultado_Busqueda;
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Mostrar_Detalles_Vehiculo
        ///DESCRIPCIÓN: Muestra a detalle el Vehiculo que se pasa como paremetro.
        ///PROPIEDADES:     
        ///             1. Vehiculo.    Contiene los Parametros y detalles que se desean 
        ///                             mostrar.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 05/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Mostrar_Detalles_Vehiculo(Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo) {
            try {
                Limpiar_Vehiculo();
                Hdf_Vehiculo_ID.Value = Vehiculo.P_Vehiculo_ID;
                Cls_Cat_Com_Marcas_Negocio Marca_Negocio = new Cls_Cat_Com_Marcas_Negocio();
                Marca_Negocio.P_Marca_ID = Vehiculo.P_Marca_ID;
                DataTable Dt_Detalles_Marca = Marca_Negocio.Consulta_Marcas();
                Txt_Nombre_Vehiculo.Text = "[" + Vehiculo.P_Serie_Carroceria.ToString() + "] " + Vehiculo.P_Nombre_Producto + ((Dt_Detalles_Marca.Rows.Count > 0) ? " " + Dt_Detalles_Marca.Rows[0][Cat_Com_Marcas.Campo_Nombre].ToString() : "") + " - " + Vehiculo.P_Modelo_ID;
                Cmb_Dependencia_Vehiculo.SelectedIndex = Cmb_Dependencia_Vehiculo.Items.IndexOf(Cmb_Dependencia_Vehiculo.Items.FindByValue(Vehiculo.P_Dependencia_ID));
                Txt_Numero_Inventario_Vehiculo.Text = Vehiculo.P_Numero_Inventario.ToString();
                System.Threading.Thread.Sleep(1000);
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Mostrar_Detalles_Parte
        ///DESCRIPCIÓN: Muestra los detalles de la parte en sus campos.
        ///PROPIEDADES:  
        ///             1.  Parte.  Instancia con las propiedades cargadas.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 26/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Mostrar_Detalles_Parte(Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio Parte) {
            try {
                Limpiar_Producto();
                Limpiar_Vehiculo();
                Limpiar_Generales();
                Session.Remove("Dt_Resguardados");
                Session.Remove("Dt_Historial_Resguardados");
                Hdf_Producto_ID.Value = Parte.P_Producto_ID;
                Hdf_Parte_ID.Value = Parte.P_Parte_ID.ToString();
                Txt_Nombre_Parte.Text = Parte.P_Nombre;
                Txt_Numero_Inventario_Parte.Text = Parte.P_Numero_Inventario;
                Cmb_Material_Parte.SelectedIndex = Cmb_Material_Parte.Items.IndexOf(Cmb_Material_Parte.Items.FindByValue(Parte.P_Material));
                Cmb_Color_Parte.SelectedIndex = Cmb_Color_Parte.Items.IndexOf(Cmb_Color_Parte.Items.FindByValue(Parte.P_Color));
                Cmb_Marca_Parte.SelectedIndex = Cmb_Marca_Parte.Items.IndexOf(Cmb_Marca_Parte.Items.FindByValue(Parte.P_Marca));
                Txt_Modelo_Parte.Text = Parte.P_Modelo;
                Txt_Costo_Parte.Text = Parte.P_Costo.ToString("#,###,###.00");
                Txt_Fecha_Adquisicion_Parte.Text = Parte.P_Fecha_Adquisicion.ToString("dd/MMM/yyyy");
                Cmb_Estado_Parte.SelectedIndex = Cmb_Estado_Parte.Items.IndexOf(Cmb_Estado_Parte.Items.FindByValue(Parte.P_Estado));
                Cmb_Estatus_Parte.SelectedIndex = Cmb_Estatus_Parte.Items.IndexOf(Cmb_Estatus_Parte.Items.FindByValue(Parte.P_Estatus));
                Txt_Comentarios_Parte.Text = Parte.P_Comentarios;
                Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
                Vehiculo.P_Vehiculo_ID = Parte.P_Vehiculo_ID;
                Vehiculo = Vehiculo.Consultar_Detalles_Vehiculo();
                Mostrar_Detalles_Vehiculo(Vehiculo);
                Llenar_Grid_Resguardos(0, Parte.P_Resguardantes);
                Llenar_Grid_Historial_Resguardos(0, Parte.P_Historial_Resguardos);
                System.Threading.Thread.Sleep(1000);
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        #region MPE_Busqueda_Productos

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Limpiar_Campos_MPE_Busqueda_Productos
            ///DESCRIPCIÓN: Limpia los filtros para la Busqueda de los productos.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 01/Marzo/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Limpiar_Campos_MPE_Busqueda_Productos() {
                Txt_Clave_Producto.Text = "";
                Txt_Nombre_Producto.Text = "";
                Cmb_Marca_Producto.SelectedIndex = 0;
                Cmb_Modelo_Producto.SelectedIndex = 0;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_MPE_Busqueda_Productos
            ///DESCRIPCIÓN: Hace una consulta a la Base de Datos y llena el Grid con el 
            ///             resultado.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 01/Marzo/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Grid_MPE_Busqueda_Productos(Int32 Pagina) {
                try {
                    Grid_Listado_Productos.SelectedIndex = -1;
                    Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio Negocio = new Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio();
                    Negocio.P_Clave_Interna = Txt_Clave_Producto.Text.Trim();
                    Negocio.P_Nombre = Txt_Nombre_Producto.Text.Trim();
                    Negocio.P_Marca = (Cmb_Marca_Producto.SelectedIndex > 0) ? Cmb_Marca_Producto.SelectedItem.Value : "";
                    Negocio.P_Modelo = (Cmb_Modelo_Producto.SelectedIndex > 0) ? Cmb_Modelo_Producto.SelectedItem.Value : "";
                    Grid_Listado_Productos.DataSource = Negocio.Listar_Productos_Partes();
                    Grid_Listado_Productos.PageIndex = Pagina;
                    Grid_Listado_Productos.DataBind();
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

        #endregion

        #region  MPE_Busqueda_Vehiculos

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Combos_MPE_Busqueda_Vehiculos
            ///DESCRIPCIÓN: Se llenan los Combos del Modal de Busqueda de Vehiculos.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 05/Mazo/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Combos_MPE_Busqueda_Vehiculos() {
                try {
                    Cls_Ope_Pat_Com_Vehiculos_Negocio Combos = new Cls_Ope_Pat_Com_Vehiculos_Negocio();

                    //SE LLENA EL COMBO DE DEPENDENCIAS DE LAS BUSQUEDAS
                    Combos.P_Tipo_DataTable = "DEPENDENCIAS";
                    DataTable Dependencias = Combos.Consultar_DataTable();
                    DataRow Fila_Dependencia = Dependencias.NewRow();
                    Fila_Dependencia["DEPENDENCIA_ID"] = "TODAS";
                    Fila_Dependencia["NOMBRE"] = HttpUtility.HtmlDecode("&lt;TODAS&gt;");
                    Dependencias.Rows.InsertAt(Fila_Dependencia, 0);
                    Cmb_Busqueda_Vehiculo_Dependencias.DataSource = Dependencias;
                    Cmb_Busqueda_Vehiculo_Dependencias.DataValueField = "DEPENDENCIA_ID";
                    Cmb_Busqueda_Vehiculo_Dependencias.DataTextField = "NOMBRE";
                    Cmb_Busqueda_Vehiculo_Dependencias.DataBind();
                    Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias.DataSource = Dependencias;
                    Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias.DataValueField = "DEPENDENCIA_ID";
                    Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias.DataTextField = "NOMBRE";
                    Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias.DataBind();

                    //SE LLENA EL COMBO DE MARCAS DE LAS BUSQUEDAS
                    Combos.P_Tipo_DataTable = "MARCAS";
                    DataTable Marcas = Combos.Consultar_DataTable();
                    DataRow Fila_Marca = Marcas.NewRow();
                    Fila_Marca["MARCA_ID"] = "TODAS";
                    Fila_Marca["NOMBRE"] = HttpUtility.HtmlDecode("&lt;TODAS&gt;");
                    Marcas.Rows.InsertAt(Fila_Marca, 0);
                    Cmb_Busqueda_Vehiculo_Marca.DataSource = Marcas;
                    Cmb_Busqueda_Vehiculo_Marca.DataTextField = "NOMBRE";
                    Cmb_Busqueda_Vehiculo_Marca.DataValueField = "MARCA_ID";
                    Cmb_Busqueda_Vehiculo_Marca.DataBind();

                    ////SE LLENA EL COMBO DE MODELOS DE LAS BUSQUEDAS
                    //Combos.P_Tipo_DataTable = "MODELOS";
                    //DataTable Modelos = Combos.Consultar_DataTable();
                    //DataRow Fila_Modelo = Modelos.NewRow();
                    //Fila_Modelo["MODELO_ID"] = "TODAS";
                    //Fila_Modelo["NOMBRE"] = HttpUtility.HtmlDecode("&lt;TODAS&gt;");
                    //Modelos.Rows.InsertAt(Fila_Modelo, 0);
                    //Cmb_Busqueda_Vehiculo_Modelo.DataSource = Modelos;
                    //Cmb_Busqueda_Vehiculo_Modelo.DataTextField = "NOMBRE";
                    //Cmb_Busqueda_Vehiculo_Modelo.DataValueField = "MODELO_ID";
                    //Cmb_Busqueda_Vehiculo_Modelo.DataBind();

                    //SE LLENA EL COMBO DE TIPO DE VEHICULOS DE LAS BUSQUEDAS
                    Combos.P_Tipo_DataTable = "TIPOS_VEHICULOS";
                    DataTable Tipos_Vehiculos = Combos.Consultar_DataTable();
                    DataRow Fila_Tipo_Vehiculo = Tipos_Vehiculos.NewRow();
                    Fila_Tipo_Vehiculo["TIPO_VEHICULO_ID"] = "TODOS";
                    Fila_Tipo_Vehiculo["DESCRIPCION"] = HttpUtility.HtmlDecode("&lt;TODOS&gt;");
                    Tipos_Vehiculos.Rows.InsertAt(Fila_Tipo_Vehiculo, 0);
                    Cmb_Busqueda_Vehiculo_Tipo_Vehiculo.DataSource = Tipos_Vehiculos;
                    Cmb_Busqueda_Vehiculo_Tipo_Vehiculo.DataTextField = "DESCRIPCION";
                    Cmb_Busqueda_Vehiculo_Tipo_Vehiculo.DataValueField = "TIPO_VEHICULO_ID";
                    Cmb_Busqueda_Vehiculo_Tipo_Vehiculo.DataBind();  


                    //SE LLENA EL COMBO DE TIPO DE COMBUSTIBLE DE LAS BUSQUEDAS
                    Combos.P_Tipo_DataTable = "TIPOS_COMBUSTIBLE";
                    DataTable Tipos_Combustible = Combos.Consultar_DataTable();
                    DataRow Fila_Tipos_Combustible = Tipos_Combustible.NewRow();
                    Fila_Tipos_Combustible["TIPO_COMBUSTIBLE_ID"] = "TODOS";
                    Fila_Tipos_Combustible["DESCRIPCION"] = HttpUtility.HtmlDecode("&lt;TODOS&gt;");
                    Tipos_Combustible.Rows.InsertAt(Fila_Tipos_Combustible, 0);
                    Cmb_Busqueda_Vehiculo_Tipo_Combustible.DataSource = Tipos_Combustible;
                    Cmb_Busqueda_Vehiculo_Tipo_Combustible.DataTextField = "DESCRIPCION";
                    Cmb_Busqueda_Vehiculo_Tipo_Combustible.DataValueField = "TIPO_COMBUSTIBLE_ID";
                    Cmb_Busqueda_Vehiculo_Tipo_Combustible.DataBind();

                    //SE LLENA EL COMBO DE COLORES DE LAS BUSQUEDAS
                    Combos.P_Tipo_DataTable = "COLORES";
                    DataTable Tipos_Colores = Combos.Consultar_DataTable();
                    DataRow Fila_Color = Tipos_Colores.NewRow();
                    Fila_Color["COLOR_ID"] = "TODOS";
                    Fila_Color["DESCRIPCION"] = HttpUtility.HtmlDecode("&lt;TODOS&gt;");
                    Tipos_Colores.Rows.InsertAt(Fila_Color, 0);
                    Cmb_Busqueda_Vehiculo_Color.DataSource = Tipos_Colores;
                    Cmb_Busqueda_Vehiculo_Color.DataTextField = "DESCRIPCION";
                    Cmb_Busqueda_Vehiculo_Color.DataValueField = "COLOR_ID";
                    Cmb_Busqueda_Vehiculo_Color.DataBind();

                    //SE LLENA EL COMBO DE ZONAS DE LAS BUSQUEDAS
                    Combos.P_Tipo_DataTable = "ZONAS";
                    DataTable Zonas = Combos.Consultar_DataTable();
                    DataRow Fila_Zona = Zonas.NewRow();
                    Fila_Zona["ZONA_ID"] = "TODOS";
                    Fila_Zona["DESCRIPCION"] = HttpUtility.HtmlDecode("&lt;TODOS&gt;");
                    Zonas.Rows.InsertAt(Fila_Zona, 0);
                    Cmb_Busqueda_Vehiculo_Zonas.DataSource = Zonas;
                    Cmb_Busqueda_Vehiculo_Zonas.DataTextField = "DESCRIPCION";
                    Cmb_Busqueda_Vehiculo_Zonas.DataValueField = "ZONA_ID";
                    Cmb_Busqueda_Vehiculo_Zonas.DataBind();

                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Empleados_Busqueda_Vehiculos
            ///DESCRIPCIÓN: Llena el combo de Empleados del Modal de Busqueda de Vehiculos.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 05/Marzo/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Combo_Empleados_Busqueda_Vehiculos(DataTable Tabla) {
                try {
                    DataRow Fila_Empleado = Tabla.NewRow();
                    Fila_Empleado["EMPLEADO_ID"] = "TODOS";
                    Fila_Empleado["NOMBRE"] = HttpUtility.HtmlDecode("&lt;TODOS&gt;");
                    Tabla.Rows.InsertAt(Fila_Empleado, 0);
                    Cmb_Busqueda_Vehiculo_Nombre_Resguardante.DataSource = Tabla;
                    Cmb_Busqueda_Vehiculo_Nombre_Resguardante.DataValueField = "EMPLEADO_ID";
                    Cmb_Busqueda_Vehiculo_Nombre_Resguardante.DataTextField = "NOMBRE";
                    Cmb_Busqueda_Vehiculo_Nombre_Resguardante.DataBind();
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Listado_Vehiculos
            ///DESCRIPCIÓN: Se llenan el Grid de Vehiculos del Modal de Busqueda dependiendo de 
            ///             los filtros pasados.
            ///PROPIEDADES:     
            ///             1. Pagina.  Pagina en donde aparecerá el Grid.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 05/Marzo/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Grid_Listado_Vehiculos(Int32 Pagina) {
                try {
                    Grid_Listado_Vehiculos.Columns[1].Visible = true;
                    Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculos = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
                    Vehiculos.P_Tipo_DataTable = "VEHICULOS";
                    if (Session["FILTRO_BUSQUEDA_VEHICULO"] != null) {
                        Vehiculos.P_Tipo_Filtro_Busqueda = Session["FILTRO_BUSQUEDA_VEHICULO"].ToString();
                        if (Session["FILTRO_BUSQUEDA_VEHICULO"].ToString().Trim().Equals("DATOS_GENERALES")) {
                            if (Txt_Busqueda_Vehiculo_Numero_Inventario.Text.Trim().Length > 0) { Vehiculos.P_Numero_Inventario = Convert.ToInt64(Txt_Busqueda_Vehiculo_Numero_Inventario.Text.Trim()); }
                            if (Txt_Busqueda_Vehiculo_Numero_Economico.Text.Trim().Length > 0) { Vehiculos.P_Numero_Economico_ = Txt_Busqueda_Vehiculo_Numero_Economico.Text.Trim(); }
                            if (Txt_Busqueda_Vehiculo_Anio_Fabricacion.Text.Trim().Length > 0) { Vehiculos.P_Anio_Fabricacion = Convert.ToInt32(Txt_Busqueda_Vehiculo_Anio_Fabricacion.Text.Trim()); }
                            Vehiculos.P_Modelo_ID = Txt_Modelo_Busqueda.Text.Trim();
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
                        } else if (Session["FILTRO_BUSQUEDA_VEHICULO"].ToString().Trim().Equals("RESGUARDANTES")) {
                            Vehiculos.P_RFC_Resguardante = Txt_Busqueda_Vehiculo_RFC_Resguardante.Text.Trim();
                            if (Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias.SelectedIndex > 0) {
                                Vehiculos.P_Dependencia_ID = Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias.SelectedItem.Value.Trim();
                            }
                            if (Cmb_Busqueda_Vehiculo_Nombre_Resguardante.SelectedIndex > 0) {
                                Vehiculos.P_Resguardante_ID = Cmb_Busqueda_Vehiculo_Nombre_Resguardante.SelectedItem.Value.Trim();
                            }
                        }
                    }
                    Grid_Listado_Vehiculos.DataSource = Vehiculos.Consultar_DataTable();
                    Grid_Listado_Vehiculos.PageIndex = Pagina;
                    Grid_Listado_Vehiculos.DataBind();
                    Grid_Listado_Vehiculos.Columns[1].Visible = false;
                    MPE_Busqueda_Vehiculo.Show();
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            
        #endregion

        #region  MPE_Busqueda_Partes

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Combos_MPE_Busqueda_Partes
            ///DESCRIPCIÓN: Se llenan los Combos del Modal de Busqueda de Partes.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 07/Mazo/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Combos_MPE_Busqueda_Partes() {
                try {
                    Cls_Ope_Pat_Com_Vehiculos_Negocio Combos = new Cls_Ope_Pat_Com_Vehiculos_Negocio();

                    //SE LLENA EL COMBO DE MARCAS
                    Combos.P_Tipo_DataTable = "MARCAS";
                    DataTable Marcas = Combos.Consultar_DataTable();
                    DataRow Fila_Marca = Marcas.NewRow();
                    Fila_Marca["MARCA_ID"] = "TODAS";
                    Fila_Marca["NOMBRE"] = HttpUtility.HtmlDecode("&lt;TODAS&gt;");
                    Marcas.Rows.InsertAt(Fila_Marca, 0);
                    Cmb_MPE_Busqueda_Partes_Marca.DataSource = Marcas;
                    Cmb_MPE_Busqueda_Partes_Marca.DataTextField = "NOMBRE";
                    Cmb_MPE_Busqueda_Partes_Marca.DataValueField = "MARCA_ID";
                    Cmb_MPE_Busqueda_Partes_Marca.DataBind();

                    //SE LLENA EL COMBO DE MATERIALES
                    Combos.P_Tipo_DataTable = "MATERIALES";
                    DataTable Materiales = Combos.Consultar_DataTable();
                    DataRow Fila_Material = Materiales.NewRow();
                    Fila_Material["MATERIAL_ID"] = "TODOS";
                    Fila_Material["DESCRIPCION"] = HttpUtility.HtmlDecode("&lt;TODOS&gt;");
                    Materiales.Rows.InsertAt(Fila_Material, 0);
                    Cmb_MPE_Busqueda_Partes_Material.DataSource = Materiales;
                    Cmb_MPE_Busqueda_Partes_Material.DataTextField = "DESCRIPCION";
                    Cmb_MPE_Busqueda_Partes_Material.DataValueField = "MATERIAL_ID";
                    Cmb_MPE_Busqueda_Partes_Material.DataBind();

                    //SE LLENA EL COMBO DE COLORES
                    Combos.P_Tipo_DataTable = "COLORES";
                    DataTable Colores = Combos.Consultar_DataTable();
                    DataRow Fila_Color = Colores.NewRow();
                    Fila_Color["COLOR_ID"] = "TODOS";
                    Fila_Color["DESCRIPCION"] = HttpUtility.HtmlDecode("&lt;TODOS&gt;");
                    Colores.Rows.InsertAt(Fila_Color, 0);
                    Cmb_MPE_Busqueda_Partes_Color.DataSource = Colores;
                    Cmb_MPE_Busqueda_Partes_Color.DataValueField = "COLOR_ID";
                    Cmb_MPE_Busqueda_Partes_Color.DataTextField = "DESCRIPCION";
                    Cmb_MPE_Busqueda_Partes_Color.DataBind();

                    //SE LLENA EL COMBO DE DEPENDENCIAS
                    Combos.P_Tipo_DataTable = "DEPENDENCIAS";
                    DataTable Dependencias = Combos.Consultar_DataTable();
                    DataRow Fila_Dependencia = Dependencias.NewRow();
                    Fila_Dependencia["DEPENDENCIA_ID"] = "TODAS";
                    Fila_Dependencia["NOMBRE"] = HttpUtility.HtmlDecode("&lt;TODAS&gt;");
                    Dependencias.Rows.InsertAt(Fila_Dependencia, 0);
                    Cmb_MPE_Busqueda_Partes_Dependencia.DataSource = Dependencias;
                    Cmb_MPE_Busqueda_Partes_Dependencia.DataValueField = "DEPENDENCIA_ID";
                    Cmb_MPE_Busqueda_Partes_Dependencia.DataTextField = "NOMBRE";
                    Cmb_MPE_Busqueda_Partes_Dependencia.DataBind();
                    Cmb_MPE_Busqueda_Partes_Dependencia_Resguardante.DataSource = Dependencias;
                    Cmb_MPE_Busqueda_Partes_Dependencia_Resguardante.DataValueField = "DEPENDENCIA_ID";
                    Cmb_MPE_Busqueda_Partes_Dependencia_Resguardante.DataTextField = "NOMBRE";
                    Cmb_MPE_Busqueda_Partes_Dependencia_Resguardante.DataBind();
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Empleados_Busqueda_Partes
            ///DESCRIPCIÓN: Llena el combo de Empleados del Modal de Busqueda de Partes.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 07/Marzo/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Combo_Empleados_Busqueda_Partes(DataTable Tabla) {
                try {
                    DataRow Fila_Empleado = Tabla.NewRow();
                    Fila_Empleado["EMPLEADO_ID"] = "TODOS";
                    Fila_Empleado["NOMBRE"] = HttpUtility.HtmlDecode("&lt;TODOS&gt;");
                    Tabla.Rows.InsertAt(Fila_Empleado, 0);
                    Cmb_MPE_Busqueda_Partes_Nombre_Resguardante.DataSource = Tabla;
                    Cmb_MPE_Busqueda_Partes_Nombre_Resguardante.DataValueField = "EMPLEADO_ID";
                    Cmb_MPE_Busqueda_Partes_Nombre_Resguardante.DataTextField = "NOMBRE";
                    Cmb_MPE_Busqueda_Partes_Nombre_Resguardante.DataBind();
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Listado_Partes
            ///DESCRIPCIÓN: Se llenan el Grid de Vehiculos del Modal de Busqueda dependiendo de 
            ///             los filtros pasados.
            ///PROPIEDADES:     
            ///             1. Pagina.  Pagina en donde aparecerá el Grid.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 07/Marzo/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Grid_Listado_Partes(Int32 Pagina) {
                try {
                    Grid_Listado_Partes.Columns[1].Visible = true;
                    Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio Parte = new Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio();
                    Parte.P_Tipo_DataTable = "PARTES";
                    if (Session["FILTRO_BUSQUEDA_PARTE"] != null) {
                        Parte.P_Tipo_Filtro_Busqueda = Session["FILTRO_BUSQUEDA_PARTE"].ToString();
                        if (Session["FILTRO_BUSQUEDA_PARTE"].ToString().Trim().Equals("DATOS_GENERALES")) {
                            if (Txt_MPE_Busqueda_Partes_No_Inventario_Parte.Text.Trim().Length > 0) { Parte.P_Numero_Inventario = Txt_MPE_Busqueda_Partes_No_Inventario_Parte.Text.Trim(); }
                            if (Txt_MPE_Busqueda_Partes_No_Inventario_Vehiculo.Text.Trim().Length > 0) { Parte.P_Numero_Inventario_Vehiculo = Txt_MPE_Busqueda_Partes_No_Inventario_Vehiculo.Text.Trim(); }
                            if (Cmb_MPE_Busqueda_Partes_Marca.SelectedIndex > 0) { Parte.P_Marca = Cmb_MPE_Busqueda_Partes_Marca.SelectedItem.Value; }
                            if (Txt_MPE_Busqueda_Partes_Modelo.Text.Trim().Length > 0) { Parte.P_Modelo = Txt_MPE_Busqueda_Partes_Modelo.Text.Trim(); }
                            if (Cmb_MPE_Busqueda_Partes_Material.SelectedIndex > 0) { Parte.P_Marca = Cmb_MPE_Busqueda_Partes_Material.SelectedItem.Value; }
                            if (Cmb_MPE_Busqueda_Partes_Color.SelectedIndex > 0) { Parte.P_Marca = Cmb_MPE_Busqueda_Partes_Color.SelectedItem.Value; }
                            if (Txt_MPE_Busqueda_Partes_Fecha_Adquisicion.Text.Trim().Length > 0) { 
                                Parte.P_Fecha_Adquisicion = Convert.ToDateTime(Txt_MPE_Busqueda_Partes_Fecha_Adquisicion.Text.Trim()); 
                                Parte.P_Buscar_Fecha_Adquisicion = true;
                            }
                            if (Cmb_MPE_Busqueda_Partes_Estatus.SelectedIndex > 0) { Parte.P_Estatus = Cmb_MPE_Busqueda_Partes_Estatus.SelectedItem.Value; }
                            if (Cmb_MPE_Busqueda_Partes_Dependencia.SelectedIndex > 0) { Parte.P_Dependencia_ID = Cmb_MPE_Busqueda_Partes_Dependencia.SelectedItem.Value; }
                        } else if (Session["FILTRO_BUSQUEDA_PARTE"].ToString().Trim().Equals("RESGUARDANTES")) {
                            if (Txt_MPE_Busqueda_Partes_RFC_Resguardante.Text.Trim().Length > 0) { Parte.P_RFC_Resguardante = Txt_MPE_Busqueda_Partes_RFC_Resguardante.Text.Trim(); }
                            if (Cmb_MPE_Busqueda_Partes_Dependencia_Resguardante.SelectedIndex > 0) { Parte.P_Dependencia_ID = Cmb_MPE_Busqueda_Partes_Dependencia_Resguardante.SelectedItem.Value; }
                            if (Cmb_MPE_Busqueda_Partes_Nombre_Resguardante.SelectedIndex > 0) { Parte.P_Resguardante_ID = Cmb_MPE_Busqueda_Partes_Nombre_Resguardante.SelectedItem.Value; }
                        }
                    }
                    Grid_Listado_Partes.DataSource = Parte.Listado_Partes_Vehiculos();
                    Grid_Listado_Partes.PageIndex = Pagina;
                    Grid_Listado_Partes.DataBind();
                    Grid_Listado_Partes.Columns[1].Visible = false;
                    MPE_Busqueda_Partes.Show();
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

            #endregion

        #region Validaciones

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Validar_Generales
            ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
            ///             una operación para dar de alta una parte.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 25/Febrero/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private Boolean Validar_Generales()
            {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                String Mensaje_Error = "";
                Boolean Validacion = true;
                if (Hdf_Producto_ID.Value.Trim().Length == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar una Parte (Producto).";
                    Validacion = false;
                }
                if (Hdf_Vehiculo_ID.Value.Trim().Length == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar una Vehiculo (Al que se va a agregar la parte).";
                    Validacion = false;
                }
                if (Txt_Modelo_Parte.Text.Trim().Length == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir el Modelo.";
                    Validacion = false;
                }
                if (Cmb_Marca_Parte.SelectedIndex == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar una opción del Combo de Marcas de la Parte.";
                    Validacion = false;
                }
                if (Cmb_Material_Parte.SelectedIndex == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar una opción del Combo de Material de la Parte.";
                    Validacion = false;
                }
                if (Cmb_Color_Parte.SelectedIndex == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar una opción del Combo de Color de la Parte.";
                    Validacion = false;
                }
                if (Txt_Costo_Parte.Text.Trim().Length == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir el Costo de la Parte.";
                    Validacion = false;
                }
                if (Txt_Fecha_Adquisicion_Parte.Text.Trim().Length == 0){
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Fecha de Adquisición de la Parte.";
                    Validacion = false;
                }
                if (Cmb_Estado_Parte.SelectedIndex == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Estado de la Parte.";
                    Validacion = false;
                }
                if (Cmb_Estatus_Parte.SelectedIndex == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Estatus de la Parte.";
                    Validacion = false;
                }
                if (Txt_Comentarios_Parte.Text.Trim().Length > 500) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Los Comentarios de la Parte no pueden exceder los 500 carácteres (Sobrepasa por " + (Txt_Comentarios_Parte.Text.Trim().Length - 500) + ").";
                    Validacion = false;
                }
                if (Grid_Resguardos.Rows.Count == 0 || Session["Dt_Resguardados"] == null) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Debe haber como minimo un empleado para resguardo.";
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

    #region Grids
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Resguardos_Partes_PageIndexChanging
        ///DESCRIPCIÓN: Maneja el evento de cambio de página de el Grid de Resguardos de
        ///             la parte.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 03/Febrero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Resguardos_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                if (Session["Dt_Resguardados"] != null) {
                    DataTable Tabla = (DataTable)Session["Dt_Resguardados"];
                    Llenar_Grid_Resguardos(e.NewPageIndex, Tabla);
                    Grid_Resguardos.SelectedIndex = (-1);
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Historial_Resguardantes_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView de Historial de Resguardos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 14/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Historial_Resguardantes_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                if (Session["Dt_Historial_Resguardos"] != null) {
                    Grid_Historial_Resguardantes.SelectedIndex = (-1);
                    Llenar_Grid_Historial_Resguardos(e.NewPageIndex, (DataTable)Session["Dt_Historial_Resguardos"]);
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Historial_Resguardantes_SelectedIndexChanged
        ///DESCRIPCIÓN: Maneja el evento de cambio de Seleccion del GridView de Historial
        ///             de Resguardos.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 14/Marzo/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Historial_Resguardantes_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                if (Grid_Historial_Resguardantes.SelectedIndex > (-1)) {
                    Limpiar_Historial_Resguardantes();
                    if (Session["Dt_Historial_Resguardos"] != null) {
                        Int32 Registro = ((Grid_Historial_Resguardantes.PageIndex) * Grid_Historial_Resguardantes.PageSize) + (Grid_Historial_Resguardantes.SelectedIndex);
                        DataTable Tabla = (DataTable)Session["Dt_Historial_Resguardos"];
                        Txt_Historial_Empleado_Resguardo.Text = Tabla.Rows[Registro][2].ToString().Trim();
                        Txt_Historial_Comentarios_Resguardo.Text = Tabla.Rows[Registro][3].ToString().Trim();
                        Txt_Historial_Fecha_Inicial_Resguardo.Text = String.Format("{0:dd 'de' MMMMMMMMMMMMM 'de' yyyy}", Tabla.Rows[Registro][4]);
                        Txt_Historial_Fecha_Final_Resguardo.Text = String.Format("{0:dd 'de' MMMMMMMMMMMMM 'de' yyyy}", Tabla.Rows[Registro][5]);
                    }
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        #region MPE_Busqueda_Productos

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Productos_PageIndexChanging
            ///DESCRIPCIÓN: Maneja el evento de cambio de página de el Grid de Productos de
            ///             la parte.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 04/Marzo/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Grid_Listado_Productos_PageIndexChanging(object sender, GridViewPageEventArgs e) {
                try {
                    Llenar_Grid_MPE_Busqueda_Productos(e.NewPageIndex);
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Productos_SelectedIndexChanged
            ///DESCRIPCIÓN: Maneja el evento de cambio de página de el Grid de Productos de
            ///             la parte.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 04/Marzo/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Grid_Listado_Productos_SelectedIndexChanged(object sender, EventArgs e) {
                try {
                    if (Grid_Listado_Productos.SelectedIndex > (-1)) {
                        Limpiar_Producto();
                        String Producto_ID = Grid_Listado_Productos.SelectedRow.Cells[1].Text.Trim();
                        if (Producto_ID.Trim().Length > 0) {
                            Cls_Cat_Com_Productos_Negocio Producto = new Cls_Cat_Com_Productos_Negocio();
                            Producto.P_Producto_ID = Producto_ID;
                            DataTable Dt_Productos = Producto.Consulta_Datos_Producto(); //Consulta los datos del Producto que fue seleccionada por el usuario
                            if (Dt_Productos.Rows.Count > 0) {
                                //Agrega los valores de los campos a los controles correspondientes de la forma
                                foreach (DataRow Registro in Dt_Productos.Rows) {
                                    Hdf_Producto_ID.Value = Producto_ID;
                                    Txt_Nombre_Parte.Text = Registro[Cat_Com_Productos.Campo_Nombre].ToString();
                                    //Cmb_Marca_Parte.SelectedValue = Registro[Cat_Com_Marcas.Campo_Marca_ID].ToString();
                                    //Cmb_Modelo_Parte.SelectedValue = Registro[Cat_Com_Productos.Campo_Modelo_ID].ToString();
                                }
                            }
                        }
                    }
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

        #endregion

        #region MPE_Busqueda_Vehiculos

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Vehiculos_PageIndexChanging
            ///DESCRIPCIÓN: Maneja la paginación del GridView de Vehiculos del Modal de Busqueda
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 05/Marzo/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Grid_Listado_Vehiculos_PageIndexChanging(object sender, GridViewPageEventArgs e) {
                try {
                    Grid_Listado_Vehiculos.SelectedIndex = (-1);
                    Llenar_Grid_Listado_Vehiculos(e.NewPageIndex);
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Vehiculos_SelectedIndexChanged
            ///DESCRIPCIÓN: Maneja el evento de cambio de Seleccion del GridView de Vehiculos del
            ///             Modal de Busqueda.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 05/Marzo/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Grid_Listado_Vehiculos_SelectedIndexChanged(object sender, EventArgs e) {
                try {
                    if (Grid_Listado_Vehiculos.SelectedIndex > (-1)) {
                        String Vehiculo_Seleccionado_ID = Grid_Listado_Vehiculos.SelectedRow.Cells[1].Text.Trim();
                        Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
                        Vehiculo.P_Vehiculo_ID = Vehiculo_Seleccionado_ID;
                        Vehiculo = Vehiculo.Consultar_Detalles_Vehiculo();
                        Mostrar_Detalles_Vehiculo(Vehiculo);
                        Grid_Listado_Vehiculos.SelectedIndex = -1;
                        Llenar_Grid_Resguardos(0, Vehiculo.P_Resguardantes);
                        MPE_Busqueda_Vehiculo.Hide();
                        System.Threading.Thread.Sleep(500);
                    }
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
        
        #endregion

        #region MPE_Busqueda_Partes

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Partes_PageIndexChanging
            ///DESCRIPCIÓN: Maneja la paginación del GridView de Parte del Modal de Busqueda
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 07/Marzo/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Grid_Listado_Partes_PageIndexChanging(object sender, GridViewPageEventArgs e) {
                try {
                    Grid_Listado_Partes.SelectedIndex = (-1);
                    Llenar_Grid_Listado_Partes(e.NewPageIndex);
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Partes_SelectedIndexChanged
            ///DESCRIPCIÓN: Maneja el evento de cambio de Seleccion del GridView de Partes del
            ///             Modal de Busqueda.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 07/Marzo/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Grid_Listado_Partes_SelectedIndexChanged(object sender, EventArgs e) {
                try {
                    if (Grid_Listado_Partes.SelectedIndex > (-1)) {
                        String Parte_ID = Grid_Listado_Partes.SelectedRow.Cells[1].Text.Trim();
                        Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio Parte = new Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio();
                        Parte.P_Parte_ID = Convert.ToInt32(Parte_ID);
                        Parte = Parte.Consultar_Datos_Parte_Vehiculo();
                        Mostrar_Detalles_Parte(Parte);
                        System.Threading.Thread.Sleep(500);
                    }
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
        
        #endregion

    #endregion

    #region Eventos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
        ///DESCRIPCIÓN: Maneja el Evento del Boton de Nuevo y guarda los datos de una nueva 
        ///             parte.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 26/Febrero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e) {
            if (Btn_Nuevo.AlternateText.Trim().Equals("Nuevo")) {
                Limpiar_Producto();
                Limpiar_Vehiculo();
                Limpiar_Generales();
                Configuracion_Formulario(false, "Nuevo");
                Cmb_Estatus_Parte.SelectedIndex = 1;
            } else {
                if (Validar_Generales()) {
                    Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio Parte = new Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio();
                    Parte.P_Producto_ID = Hdf_Producto_ID.Value;
                    Parte.P_Nombre = Txt_Nombre_Parte.Text.Trim();
                    Parte.P_Marca = Cmb_Marca_Parte.SelectedItem.Value;
                    Parte.P_Modelo = Txt_Modelo_Parte.Text.Trim();
                    Parte.P_Vehiculo_ID = Hdf_Vehiculo_ID.Value;
                    Parte.P_Costo = Convert.ToDouble(Txt_Costo_Parte.Text.Trim());
                    Parte.P_Material = Cmb_Material_Parte.SelectedItem.Value;
                    Parte.P_Color = Cmb_Color_Parte.SelectedItem.Value;
                    Parte.P_Numero_Inventario = Txt_Numero_Inventario_Parte.Text.Trim();
                    Parte.P_Cantidad = 1;
                    Parte.P_Estado = Cmb_Estado_Parte.SelectedItem.Value;
                    Parte.P_Estatus = Cmb_Estatus_Parte.SelectedItem.Value;
                    Parte.P_Comentarios = Txt_Comentarios_Parte.Text.Trim();
                    Parte.P_Fecha_Adquisicion = Convert.ToDateTime(Txt_Fecha_Adquisicion_Parte.Text.Trim());
                    Parte.P_Resguardantes = (Session["Dt_Resguardados"] != null) ? (DataTable)Session["Dt_Resguardados"] : new DataTable();
                    Parte.P_Usuario_Nombre = Cls_Sessiones.Nombre_Empleado;
                    Parte.P_Usuario_ID = Cls_Sessiones.Empleado_ID;
                    Parte = Parte.Alta_Parte();
                    Limpiar_Producto();
                    Limpiar_Vehiculo();
                    Limpiar_Generales();
                    Session.Remove("Dt_Resguardados");
                    Configuracion_Formulario(true, "");
                    ClientScript.RegisterStartupScript(this.GetType(), "Alta de Parte Vehiculos", "alert('Alta de Parte Exitosa');");
                    Parte = Parte.Consultar_Datos_Parte_Vehiculo();
                    Mostrar_Detalles_Parte(Parte);
                }
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
        ///DESCRIPCIÓN: Maneja el Evento del Boton de Modificar y guarda los datos de la 
        ///             parte seleccionada.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 26/Febrero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e) {
            if (Hdf_Parte_ID.Value != null && Hdf_Parte_ID.Value.Trim().Length > 0) {
                if (Btn_Modificar.AlternateText.Trim().Equals("Modificar")) {
                    Configuracion_Formulario(false, "Modificar");
                } else {
                    if (Validar_Generales()) {
                        Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio Parte = new Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio();
                        Parte.P_Parte_ID = (Hdf_Parte_ID.Value != null && Hdf_Parte_ID.Value.Trim().Length>0)?Convert.ToInt32(Hdf_Parte_ID.Value.Trim()):(-1);
                        Parte.P_Vehiculo_ID = Hdf_Vehiculo_ID.Value;
                        Parte.P_Numero_Inventario = Txt_Numero_Inventario_Parte.Text.Trim();
                        Parte.P_Cantidad = 1;
                        Parte.P_Material = Cmb_Material_Parte.SelectedItem.Value;
                        Parte.P_Color = Cmb_Color_Parte.SelectedItem.Value;
                        Parte.P_Costo = Convert.ToDouble(Txt_Costo_Parte.Text.Trim());
                        Parte.P_Fecha_Adquisicion = Convert.ToDateTime(Txt_Fecha_Adquisicion_Parte.Text.Trim());
                        Parte.P_Estado = Cmb_Estado_Parte.SelectedItem.Value;
                        Parte.P_Estatus = Cmb_Estatus_Parte.SelectedItem.Value;
                        Parte.P_Comentarios = Txt_Comentarios_Parte.Text.Trim();
                        Parte.P_Resguardantes = (Session["Dt_Resguardados"] != null) ? (DataTable)Session["Dt_Resguardados"] : new DataTable();
                        Parte.P_Usuario_Nombre = Cls_Sessiones.Nombre_Empleado;
                        Parte.P_Usuario_ID = Cls_Sessiones.Empleado_ID;
                        Parte.Modificar_Parte();
                        Limpiar_Producto();
                        Limpiar_Vehiculo();
                        Limpiar_Generales();
                        Session.Remove("Dt_Resguardados");
                        Configuracion_Formulario(true, "");
                        ClientScript.RegisterStartupScript(this.GetType(), "Actualización de Parte Vehiculos", "alert('Actualización de Parte Exitosa');", true);
                        Parte = Parte.Consultar_Datos_Parte_Vehiculo();
                        Mostrar_Detalles_Parte(Parte);
                    }
                }
            } else {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                Lbl_Mensaje_Error.Text = "+ Seleccionar la parte que se desea Modificar";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
        ///DESCRIPCIÓN: Maneja el Evento del Boton de Salir y Cancelar.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 26/Febrero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Salir_Click(object sender, ImageClickEventArgs e) {
            if (Btn_Salir.AlternateText.Trim().Equals("Salir")) {
                Session.Remove("Dt_Resguardados");
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            } else {
                Limpiar_Producto();
                Limpiar_Vehiculo();
                Limpiar_Generales();
                Session.Remove("Dt_Resguardados");
                Configuracion_Formulario(true, "");
            }
        }

        #region MPE_Busqueda_Productos

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Lanzar_Buscar_Producto_Click
            ///DESCRIPCIÓN: Lanza el Modal de Busqueda de Productos.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 04/Marzo/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Btn_Lanzar_Buscar_Producto_Click(object sender, ImageClickEventArgs e) {
                try {
                    MPE_Busqueda_Productos.Show();
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_MPE_Productos_Buscar_Click
            ///DESCRIPCIÓN: Hace la Busqueda de los productos.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 01/Marzo/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Btn_MPE_Productos_Buscar_Click(object sender, ImageClickEventArgs e) {
                try {
                    Llenar_Grid_MPE_Busqueda_Productos(0);
                    MPE_Busqueda_Productos.Show();
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_MPE_Productos_Limpiar_Click
            ///DESCRIPCIÓN: Limpia los filtros para la Busqueda de los productos.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 01/Marzo/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Btn_MPE_Productos_Limpiar_Click(object sender, ImageClickEventArgs e) {
                try {
                    Limpiar_Campos_MPE_Busqueda_Productos();
                    MPE_Busqueda_Productos.Show();
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

        #endregion

        #region  MPE_Busqueda_Vehiculos

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Lanzar_Buscar_Vehiculo_Click
            ///DESCRIPCIÓN: Lanza el Modal de Busqueda de Vehículos.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 05/Marzo/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Btn_Lanzar_Buscar_Vehiculo_Click(object sender, ImageClickEventArgs e) {
                try {
                    Div_Contenedor_Msj_Error.Visible = false;
                    Lbl_Mensaje_Error.Text = "";
                    MPE_Busqueda_Vehiculo.Show();
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Cmb_Busqueda_Resguardantes_Dependencias_SelectedIndexChanged
            ///DESCRIPCIÓN: Maneja el evento de cambio de Selección del Combo de Dependencias
            ///             del Modal de Busqueda (Parte de Resguardantes).
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 05/Marzo/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias_SelectedIndexChanged(object sender, EventArgs e) {
                try {
                    if (Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias.SelectedIndex > 0) {
                        Cls_Ope_Pat_Com_Vehiculos_Negocio Combo = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
                        Combo.P_Tipo_DataTable = "EMPLEADOS";
                        Combo.P_Dependencia_ID = Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias.SelectedItem.Value.Trim();
                        DataTable Tabla = Combo.Consultar_DataTable();
                        Llenar_Combo_Empleados_Busqueda_Vehiculos(Tabla);
                    } else {
                        DataTable Tabla = new DataTable();
                        Tabla.Columns.Add("EMPLEADO_ID", Type.GetType("System.String"));
                        Tabla.Columns.Add("NOMBRE", Type.GetType("System.String"));
                        Llenar_Combo_Empleados_Busqueda_Vehiculos(Tabla);
                    }
                    MPE_Busqueda_Vehiculo.Show();
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_MPE_Vehiculos_Limpiar_Filtros_Buscar_Datos_Click
            ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Limpieza de los filtros
            ///             para la busqueda por parte de los Datos Generales.
            ///PARAMETROS:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 05/Marzo/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************    
            protected void Btn_MPE_Vehiculos_Limpiar_Filtros_Buscar_Datos_Click(object sender, ImageClickEventArgs e) {
                try {
                    Txt_Busqueda_Vehiculo_Numero_Inventario.Text = "";
                    Txt_Busqueda_Vehiculo_Numero_Economico.Text = "";
                    Txt_Modelo_Busqueda.Text = "";
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
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_MPE_Vehiculos_Buscar_Datos_Click
            ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Busqueda de los
            ///             Datos Generales.
            ///PARAMETROS:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 05/Marzo/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************    
            protected void Btn_MPE_Vehiculos_Buscar_Datos_Click(object sender, ImageClickEventArgs e) {
                try {
                    Session["FILTRO_BUSQUEDA_VEHICULO"] = "DATOS_GENERALES";
                    Llenar_Grid_Listado_Vehiculos(0);
                }
                catch (Exception Ex)
                {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_MPE_Vehiculos_Limpiar_Filtros_Buscar_Resguardante_Click
            ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Limpieza de los filtros
            ///             para la busqueda por parte de los Listados.
            ///PARAMETROS:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 05/Marzo/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************  
            protected void Btn_MPE_Vehiculos_Limpiar_Filtros_Buscar_Resguardante_Click(object sender, ImageClickEventArgs e) {
                try {
                    Txt_Busqueda_Vehiculo_RFC_Resguardante.Text = "";
                    Cmb_Busqueda_Vehiculo_Nombre_Resguardante.SelectedIndex = 0;
                    Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias.SelectedIndex = 0;
                    DataTable Tabla = new DataTable();
                    Tabla.Columns.Add("EMPLEADO_ID", Type.GetType("System.String"));
                    Tabla.Columns.Add("NOMBRE", Type.GetType("System.String"));
                    Llenar_Combo_Empleados_Busqueda_Vehiculos(Tabla);
                    MPE_Busqueda_Vehiculo.Show();
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_MPE_Vehiculos_Buscar_Resguardante_Click
            ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Busqueda de los
            ///             Reguardante
            ///PARAMETROS:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 05/Marzo/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************    
            protected void Btn_MPE_Vehiculos_Buscar_Resguardante_Click(object sender, ImageClickEventArgs e) {
                try {
                    Session["FILTRO_BUSQUEDA_VEHICULO"] = "RESGUARDANTES";
                    Llenar_Grid_Listado_Vehiculos(0);
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }


        #endregion

        #region  MPE_Busqueda_Partes

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Lnk_Busqueda_Avanzada_Click
            ///DESCRIPCIÓN: Lanza el Modal de Busqueda de Partes.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 07/Marzo/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Lnk_Busqueda_Avanzada_Click(object sender, EventArgs e) {
                try {
                    Div_Contenedor_Msj_Error.Visible = false;
                    Lbl_Mensaje_Error.Text = "";
                    MPE_Busqueda_Partes.Show();
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Cmb_MPE_Busqueda_Partes_Dependencia_Resguardante_SelectedIndexChanged
            ///DESCRIPCIÓN: Maneja el evento de cambio de Selección del Combo de Dependencias
            ///             del Modal de Busqueda (Parte de Resguardantes).
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 07/Marzo/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Cmb_MPE_Busqueda_Partes_Dependencia_Resguardante_SelectedIndexChanged(object sender, EventArgs e) {
                try {
                    if (Cmb_MPE_Busqueda_Partes_Dependencia_Resguardante.SelectedIndex > 0) {
                        Cls_Ope_Pat_Com_Vehiculos_Negocio Combo = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
                        Combo.P_Tipo_DataTable = "EMPLEADOS";
                        Combo.P_Dependencia_ID = Cmb_MPE_Busqueda_Partes_Dependencia_Resguardante.SelectedItem.Value.Trim();
                        DataTable Tabla = Combo.Consultar_DataTable();
                        Llenar_Combo_Empleados_Busqueda_Partes(Tabla);
                    } else {
                        DataTable Tabla = new DataTable();
                        Tabla.Columns.Add("EMPLEADO_ID", Type.GetType("System.String"));
                        Tabla.Columns.Add("NOMBRE", Type.GetType("System.String"));
                        Llenar_Combo_Empleados_Busqueda_Partes(Tabla);
                    }
                    MPE_Busqueda_Partes.Show();
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_MPE_Partes_Limpiar_Filtros_Buscar_Datos_Click
            ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Limpieza de los filtros
            ///             para la busqueda por parte de los Datos Generales.
            ///PARAMETROS:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 07/Marzo/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************    
            protected void Btn_MPE_Partes_Limpiar_Filtros_Buscar_Datos_Click(object sender, ImageClickEventArgs e) {
                try {
                    Txt_MPE_Busqueda_Partes_No_Inventario_Parte.Text = "";
                    Txt_MPE_Busqueda_Partes_No_Inventario_Vehiculo.Text = "";
                    Cmb_MPE_Busqueda_Partes_Marca.SelectedIndex = 0;
                    Txt_MPE_Busqueda_Partes_Modelo.Text = "";
                    Cmb_MPE_Busqueda_Partes_Material.SelectedIndex = 0;
                    Cmb_MPE_Busqueda_Partes_Color.SelectedIndex = 0;
                    Txt_MPE_Busqueda_Partes_Fecha_Adquisicion.Text = "";
                    Cmb_MPE_Busqueda_Partes_Estatus.SelectedIndex = 0;
                    Cmb_MPE_Busqueda_Partes_Dependencia.SelectedIndex = 0;
                    MPE_Busqueda_Partes.Show();
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_MPE_Partes_Buscar_Datos_Click
            ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Busqueda de los
            ///             Datos Generales.
            ///PARAMETROS:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 07/Marzo/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************    
            protected void Btn_MPE_Partes_Buscar_Datos_Click(object sender, ImageClickEventArgs e) {
                try {
                    Session["FILTRO_BUSQUEDA_PARTE"] = "DATOS_GENERALES";
                    Llenar_Grid_Listado_Partes(0);
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_MPE_Partes_Limpiar_Filtros_Buscar_Resguardante_Click
            ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Limpieza de los filtros
            ///             para la busqueda por parte de los Listados.
            ///PARAMETROS:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 07/Marzo/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************  
            protected void Btn_MPE_Partes_Limpiar_Filtros_Buscar_Resguardante_Click(object sender, ImageClickEventArgs e) {
                try {
                    Txt_MPE_Busqueda_Partes_RFC_Resguardante.Text = "";
                    Cmb_MPE_Busqueda_Partes_Nombre_Resguardante.SelectedIndex = 0;
                    Cmb_MPE_Busqueda_Partes_Dependencia_Resguardante.SelectedIndex = 0;
                    DataTable Tabla = new DataTable();
                    Tabla.Columns.Add("EMPLEADO_ID", Type.GetType("System.String"));
                    Tabla.Columns.Add("NOMBRE", Type.GetType("System.String"));
                    Llenar_Combo_Empleados_Busqueda_Partes(Tabla);
                    MPE_Busqueda_Partes.Show();
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_MPE_Partes_Buscar_Resguardante_Click
            ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Busqueda de los
            ///             Reguardante
            ///PARAMETROS:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 05/Marzo/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************    
            protected void Btn_MPE_Partes_Buscar_Resguardante_Click(object sender, ImageClickEventArgs e) {
                try {
                    Session["FILTRO_BUSQUEDA_PARTE"] = "RESGUARDANTES";
                    Llenar_Grid_Listado_Partes(0);
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
            protected void Configuracion_Acceso_LinkButton(String URL_Pagina)
            {
                List<LinkButton> Botones = new List<LinkButton>();//Variable que almacenara una lista de los botones de la página.
                DataRow[] Dr_Menus = null;//Variable que guardara los menus consultados.

                try
                {
                    //Agregamos los botones a la lista de botones de la página.
                    Botones.Add(Lnk_Busqueda_Avanzada);

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
}