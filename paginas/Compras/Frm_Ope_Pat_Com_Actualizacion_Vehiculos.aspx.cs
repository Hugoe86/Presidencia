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
using Presidencia.Control_Patrimonial_Operacion_Vehiculos.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Partes_Vehiculos.Negocio;
using Presidencia.Sessiones;
using System.IO;
using Presidencia.Constantes;
using System.Collections.Generic;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using Presidencia.Reportes;
using Presidencia.Almacen_Resguardos.Negocio;
using Presidencia.Catalogo_Compras_Marcas.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Detalles_Vehiculos.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Tipos_Vehiculo.Negocio;
using Presidencia.Catalogo_Compras_Proveedores.Negocio;
using Presidencia.Control_Patrimonial_Reporte_Completo_Vehiculos.Negocio;
using Presidencia.Empleados.Negocios;
using Presidencia.Control_Patrimonial_Catalogo_Clasificaciones.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Clases_Activos.Negocio;

public partial class paginas_Compras_Frm_Ope_Pat_Com_Actualizacion_Vehiculos : System.Web.UI.Page
{

    #region Page Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 06/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
        Div_Contenedor_Msj_Error.Visible = false;
        if (!IsPostBack)
        {
            Llenar_Combos_Independientes();
            Grid_Listado_Vehiculos.Columns[1].Visible = false;
            Configuracion_Formulario(true);
            Div_Partes_Vehiculos_Campos.Visible = false;
            Lbl_Capacidad_Carga.Visible = false;
            Txt_Capacidad_Carga.Visible = false;
        }
    }

    #endregion

    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
    ///PROPIEDADES:     
    ///             1. Estatus. Estatus en el que se cargara la configuración de los 
    ///                         controles.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 06/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean Estatus)
    {
        if (Estatus)
        {
            Btn_Modificar.AlternateText = "Modificar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Div_Busqueda.Visible = true;
            Btn_Generar_Reporte.Visible = true;
            Btn_Generar_Reporte_Completo.Visible = true;
        }
        else
        {
            Btn_Modificar.AlternateText = "Actualizar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
            Btn_Salir.AlternateText = "Cancelar";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
            Div_Busqueda.Visible = false;
            Btn_Generar_Reporte.Visible = false;
            Btn_Generar_Reporte_Completo.Visible = false;
        }
        Cmb_Tipo_Activo.Enabled = !Estatus;
        Cmb_Clase_Activo.Enabled = !Estatus;
        Cmb_Dependencias.Enabled = !Estatus;
        Cmb_Marca.Enabled = !Estatus;
        Txt_Modelo.Enabled = !Estatus;
        Txt_Numero_Economico.Enabled = !Estatus;
        Btn_Fecha_Adquisicion.Enabled = !Estatus;
        Cmb_Tipos_Vehiculos.Enabled = !Estatus;
        Cmb_Tipo_Combustible.Enabled = !Estatus;
        Cmb_Colores.Enabled = !Estatus;
        Cmb_Zonas.Enabled = !Estatus;
        Txt_Placas.Enabled = !Estatus;
        Txt_Capacidad_Carga.Enabled = !Estatus;
        Txt_Anio_Fabricacion.Enabled = !Estatus;
        Txt_Serie_Carroceria.Enabled = !Estatus;
        Txt_Numero_Cilindros.Enabled = !Estatus;
        Txt_Kilometraje.Enabled = !Estatus;
        Txt_No_Factura.Enabled = !Estatus;
        Cmb_Proveedor.Enabled = !Estatus;
        Cmb_Estatus.Enabled = !Estatus;
        Cmb_Odometro.Enabled = !Estatus;
        Txt_Motivo_Baja.Enabled = !Estatus;
        Txt_Observaciones.Enabled = !Estatus;
        Cmb_Aseguradoras.Enabled = !Estatus;
        Txt_Numero_Poliza_Seguro.Enabled = !Estatus;
        Txt_Numero_Inciso.Enabled = !Estatus;
        Txt_Cobertura_Seguro.Enabled = !Estatus;
        Cmb_Empleados.Enabled = !Estatus;
        Txt_Cometarios.Enabled = !Estatus;
        Btn_Agregar_Resguardante.Visible = !Estatus;
        Btn_Quitar_Resguardante.Visible = !Estatus;
        Grid_Resguardantes.Columns[0].Visible = !Estatus;
        AFU_Archivo.Enabled = !Estatus;
        Grid_Detalles_Vehiculo.Enabled = !Estatus;
        Btn_Busqueda_Avanzada_Resguardante.Visible = !Estatus;
        Btn_Resguardo_Completo_Operador.Visible = !Estatus;
        Btn_Resguardo_Completo_Funcionario_Recibe.Visible = !Estatus;
        Btn_Resguardo_Completo_Autorizo.Visible = !Estatus;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Generales
    ///DESCRIPCIÓN: Se Limpian los campos Generales de los Vehiculos.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 02/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Generales()
    {
        try
        {
            Hdf_Vehiculo_ID.Value = "";
            Txt_Numero_Inventario.Text = "";
            Txt_Nombre.Text = "";
            Txt_Numero_Economico.Text = "";
            Cmb_Tipos_Vehiculos.SelectedIndex = 0;
            Cmb_Tipo_Combustible.SelectedIndex = 0;
            Cmb_Colores.SelectedIndex = 0;
            Cmb_Dependencias.SelectedIndex = 0;
            Cmb_Marca.SelectedIndex = 0;
            Txt_Modelo.Text = "";
            Cmb_Zonas.SelectedIndex = 0;
            Txt_Placas.Text = "";
            Txt_Costo_Inicial.Text = "";
            Txt_Costo_Actual.Text = "";
            Txt_Capacidad_Carga.Text = "";
            Txt_Anio_Fabricacion.Text = "";
            Txt_Serie_Carroceria.Text = "";
            Txt_Numero_Cilindros.Text = "";
            Txt_Fecha_Adquisicion.Text = "";
            Txt_Kilometraje.Text = "";
            Cmb_Estatus.SelectedIndex = 0;
            Cmb_Odometro.SelectedIndex = 0;
            Txt_Motivo_Baja.Text = "";
            Txt_Observaciones.Text = "";
            Hdf_Vehiculo_Aseduradora_ID.Value = "";
            Cmb_Aseguradoras.SelectedIndex = 0;
            Txt_Numero_Poliza_Seguro.Text = "";
            Txt_Numero_Inciso.Text = "";
            Txt_Cobertura_Seguro.Text = "";
            Grid_Resguardantes.DataSource = new DataTable();
            Grid_Resguardantes.DataBind();
            Grid_Historial_Resguardantes.DataSource = new DataTable();
            Grid_Historial_Resguardantes.DataBind();
            Limpiar_Resguardantes();
            Limpiar_Historial_Resguardantes();
            Llenar_Combo_Empleados();
            Limpiar_Firmas();
            Remover_Sesiones_Control_AsyncFileUpload(AFU_Archivo.ClientID);
            Txt_Busqueda_No_Empleado.Text = "";
            Txt_Busqueda_RFC.Text = "";
            Txt_Busqueda_Nombre_Empleado.Text = "";
            Cmb_Busqueda_Dependencia.SelectedIndex = 0;
            Grid_Busqueda_Empleados_Resguardo.DataSource = new DataTable();
            Grid_Busqueda_Empleados_Resguardo.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Empleados
    ///DESCRIPCIÓN: Llena el combo de Empleados.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 03/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Empleados(DataTable Tabla)
    {
        try
        {
            DataRow Fila_Empleado = Tabla.NewRow();
            Fila_Empleado["EMPLEADO_ID"] = "SELECCIONE";
            Fila_Empleado["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Tabla.Rows.InsertAt(Fila_Empleado, 0);
            Cmb_Empleados.DataSource = Tabla;
            Cmb_Empleados.DataValueField = "EMPLEADO_ID";
            Cmb_Empleados.DataTextField = "NOMBRE";
            Cmb_Empleados.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Resguardantes
    ///DESCRIPCIÓN: Se Limpian los campos de Resguardantes de los Vehiculos.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 06/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Resguardantes()
    {
        try
        {
            Cmb_Empleados.SelectedIndex = 0;
            Txt_Cometarios.Text = "";
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Firmas
    ///DESCRIPCIÓN: Se Limpian los campos de la parte de Firmas.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 20/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Firmas() {
        Hdf_Resguardo_Completo_Operador.Value = "";
        Hdf_Resguardo_Completo_Funcionario_Recibe.Value = "";
        Hdf_Resguardo_Completo_Autorizo.Value = "";
        Txt_Resguardo_Completo_Operador.Text = "";
        Txt_Resguardo_Completo_Funcionario_Recibe.Text = "";
        Txt_Resguardo_Completo_Autorizo.Text = "";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Historial_Resguardantes
    ///DESCRIPCIÓN: Se Limpian los campos de Historial de los Resguardantes de los 
    ///             Bienes Muebles.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 13/Diciembre/2010 
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
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Empleados
    ///DESCRIPCIÓN: Llena el combo de Empleados.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 07/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Empleados()
    {
        try
        {
            DataTable Tabla = new DataTable();
            if (Hdf_Vehiculo_ID.Value.Trim().Length > 0)
            {
                Cls_Ope_Pat_Com_Vehiculos_Negocio Empleados = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
                Empleados.P_Tipo_DataTable = "EMPLEADOS_VEHICULO";
                Empleados.P_Vehiculo_ID = Hdf_Vehiculo_ID.Value.Trim();
                Tabla = Empleados.Consultar_DataTable();
            }
            else
            {
                Tabla.Columns.Add("EMPLEADO_ID", Type.GetType("System.String"));
                Tabla.Columns.Add("NOMBRE", Type.GetType("System.String"));
            }
            DataRow Fila_Empleado = Tabla.NewRow();
            Fila_Empleado["EMPLEADO_ID"] = "SELECCIONE";
            Fila_Empleado["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Tabla.Rows.InsertAt(Fila_Empleado, 0);
            Cmb_Empleados.DataSource = Tabla;
            Cmb_Empleados.DataValueField = "EMPLEADO_ID";
            Cmb_Empleados.DataTextField = "NOMBRE";
            Cmb_Empleados.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Resguardantes
    ///DESCRIPCIÓN: Llena la tabla de Resguardantes
    ///PROPIEDADES:     
    ///             1.  Pagina. Pagina en la cual se mostrará el Grid_VIew
    ///             2.  Tabla.  Tabla que se va a cargar en el Grid.    
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 07/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Resguardantes(Int32 Pagina, DataTable Tabla)
    {
        Session["Dt_Resguardantes"] = Tabla;
        Grid_Resguardantes.Columns[1].Visible = true;
        Grid_Resguardantes.Columns[2].Visible = true;
        Grid_Resguardantes.DataSource = Tabla;
        Grid_Resguardantes.PageIndex = Pagina;
        Grid_Resguardantes.DataBind();
        Grid_Resguardantes.Columns[1].Visible = false;
        Grid_Resguardantes.Columns[2].Visible = false;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Historial_Resguardos
    ///DESCRIPCIÓN: Llena la tabla de Historial de Resguardantes
    ///PROPIEDADES:     
    ///             1.  Pagina. Pagina en la cual se mostrará el Grid_VIew
    ///             2.  Tabla.  Tabla que se va a cargar en el Grid.    
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 14/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Historial_Resguardos(Int32 Pagina, DataTable Tabla)
    {
        Grid_Historial_Resguardantes.Columns[1].Visible = true;
        Grid_Historial_Resguardantes.Columns[2].Visible = true;
        Grid_Historial_Resguardantes.DataSource = Tabla;
        Grid_Historial_Resguardantes.PageIndex = Pagina;
        Grid_Historial_Resguardantes.DataBind();
        Grid_Historial_Resguardantes.Columns[1].Visible = false;
        Grid_Historial_Resguardantes.Columns[2].Visible = false;
        Session["Dt_Historial_Resguardos"] = Tabla;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Historial_Archivos
    ///DESCRIPCIÓN: Llena la tabla de Historial de Archivos
    ///PROPIEDADES:     
    ///             1.  Pagina. Pagina en la cual se mostrará el Grid_VIew
    ///             2.  Tabla.  Tabla que se va a cargar en el Grid.    
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 16/Febrero/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Historial_Archivos(Int32 Pagina, DataTable Tabla)
    {
        Grid_Archivos.Columns[0].Visible = true;
        Grid_Archivos.Columns[1].Visible = true;
        Grid_Archivos.DataSource = Tabla;
        Grid_Archivos.PageIndex = Pagina;
        Grid_Archivos.DataBind();
        Grid_Archivos.Columns[0].Visible = false;
        Grid_Archivos.Columns[1].Visible = false;
        Session["Dt_Historial_Archivos"] = Tabla;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Detalles_Vehiculo
    ///DESCRIPCIÓN: Llena la tabla de Detalles de Parte de Vehiculo.
    ///PARAMETROS:       
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 07/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Detalles_Vehiculo(DataTable Dt_Detalles) {
        Grid_Detalles_Vehiculo.Columns[0].Visible = true;
        Grid_Detalles_Vehiculo.DataSource = Dt_Detalles;
        Grid_Detalles_Vehiculo.DataBind();
        Grid_Detalles_Vehiculo.Columns[0].Visible = false;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Grid_Detalles_Vehiculo
    ///DESCRIPCIÓN: Carga los detalles de la tabla de Detalles de Parte de Vehiculo.
    ///PARAMETROS: Dt_Detalles. Detalles para cargar el grid.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 07/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Grid_Detalles_Vehiculo(DataTable Dt_Detalles) {
        for (Int32 Contador = 0; Contador < (Grid_Detalles_Vehiculo.Rows.Count); Contador++) {
            String Grid_Detalle_ID = Grid_Detalles_Vehiculo.Rows[Contador].Cells[0].Text.Trim();
            for (Int32 Contador_Dt = 0; Contador_Dt < (Dt_Detalles.Rows.Count); Contador_Dt++) {
                String Dt_Detalle_ID = Dt_Detalles.Rows[Contador_Dt]["DETALLE_ID"].ToString().Trim();
                if (Grid_Detalle_ID.Equals(Dt_Detalle_ID)) {
                    if (Grid_Detalles_Vehiculo.Rows[Contador].FindControl("Cmb_Estado_Detalle") != null) {
                        DropDownList Cmb_Estado_Detalle = (DropDownList)Grid_Detalles_Vehiculo.Rows[Contador].FindControl("Cmb_Estado_Detalle");
                        Cmb_Estado_Detalle.SelectedIndex = Cmb_Estado_Detalle.Items.IndexOf(Cmb_Estado_Detalle.Items.FindByValue(Dt_Detalles.Rows[Contador_Dt]["ESTADO"].ToString()));
                    }
                }
            }
        }
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
    private Boolean Buscar_Clave_DataTable(String Clave, DataTable Tabla, Int32 Columna)
    {
        Boolean Resultado_Busqueda = false;
        if (Tabla != null && Tabla.Rows.Count > 0 && Tabla.Columns.Count > 0)
        {
            if (Tabla.Columns.Count > Columna)
            {
                for (Int32 Contador = 0; Contador < Tabla.Rows.Count; Contador++)
                {
                    if (Tabla.Rows[Contador][Columna].ToString().Trim().Equals(Clave.Trim()))
                    {
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
    ///FECHA_CREO: 14/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Mostrar_Detalles_Vehiculo(Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo) {
        try {
            Limpiar_Generales();
            Hdf_Vehiculo_ID.Value = Vehiculo.P_Vehiculo_ID;
            Txt_Numero_Inventario.Text = Vehiculo.P_Numero_Inventario.ToString();
            Txt_Nombre.Text = Vehiculo.P_Nombre_Producto;
            Txt_Numero_Economico.Text = Vehiculo.P_Numero_Economico_.ToString();
            Cmb_Tipo_Activo.SelectedIndex = Cmb_Tipo_Activo.Items.IndexOf(Cmb_Tipo_Activo.Items.FindByValue(Vehiculo.P_Clasificacion_ID));
            Cmb_Clase_Activo.SelectedIndex = Cmb_Clase_Activo.Items.IndexOf(Cmb_Clase_Activo.Items.FindByValue(Vehiculo.P_Clase_Activo_ID));
            Cmb_Marca.SelectedIndex = Cmb_Marca.Items.IndexOf(Cmb_Marca.Items.FindByValue(Vehiculo.P_Marca_ID));
            Txt_Modelo.Text = Vehiculo.P_Modelo_ID;
            Cmb_Dependencias.SelectedIndex = Cmb_Dependencias.Items.IndexOf(Cmb_Dependencias.Items.FindByValue(Vehiculo.P_Dependencia_ID));
            Cmb_Tipos_Vehiculos.SelectedIndex = Cmb_Tipos_Vehiculos.Items.IndexOf(Cmb_Tipos_Vehiculos.Items.FindByValue(Vehiculo.P_Tipo_Vehiculo_ID));
            Cmb_Tipo_Combustible.SelectedIndex = Cmb_Tipo_Combustible.Items.IndexOf(Cmb_Tipo_Combustible.Items.FindByValue(Vehiculo.P_Tipo_Combustible_ID));
            Cmb_Colores.SelectedIndex = Cmb_Colores.Items.IndexOf(Cmb_Colores.Items.FindByValue(Vehiculo.P_Color_ID));
            Cmb_Zonas.SelectedIndex = Cmb_Zonas.Items.IndexOf(Cmb_Zonas.Items.FindByValue(Vehiculo.P_Zona_ID));
            Txt_Placas.Text = Vehiculo.P_Placas;
            Txt_Costo_Inicial.Text = Vehiculo.P_Costo_Inicial.ToString("#,###,##0.00");
            Txt_Costo_Actual.Text = Vehiculo.P_Costo_Actual.ToString("#,###,##0.00");
            Txt_Capacidad_Carga.Text = Vehiculo.P_Capacidad_Carga.ToString();
            Txt_Anio_Fabricacion.Text = Vehiculo.P_Anio_Fabricacion.ToString();
            Txt_Serie_Carroceria.Text = Vehiculo.P_Serie_Carroceria;
            Txt_Numero_Cilindros.Text = Vehiculo.P_Numero_Cilindros.ToString();
            Txt_Fecha_Adquisicion.Text = String.Format("{0:dd/MMM/yyyy}", Vehiculo.P_Fecha_Adquisicion);
            Txt_Kilometraje.Text = Vehiculo.P_Kilometraje.ToString("#,###,##0.00");
            Txt_No_Factura.Text = Vehiculo.P_No_Factura_.Trim();
            Cmb_Proveedor.SelectedIndex = Cmb_Proveedor.Items.IndexOf(Cmb_Proveedor.Items.FindByValue(Vehiculo.P_Proveedor_ID));
            Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Vehiculo.P_Estatus));
            Cmb_Odometro.SelectedIndex = Cmb_Odometro.Items.IndexOf(Cmb_Odometro.Items.FindByValue(Vehiculo.P_Odometro));
            if (Vehiculo.P_Motivo_Baja != null) { Txt_Motivo_Baja.Text = Vehiculo.P_Motivo_Baja; }
            Txt_Observaciones.Text = Vehiculo.P_Observaciones.Trim();
            Txt_Usuario_creo.Text = (Vehiculo.P_Dato_Creacion.Trim() != "[]") ? Vehiculo.P_Dato_Creacion : "";
            Txt_Usuario_Modifico.Text = (Vehiculo.P_Dato_Modificacion.Trim() != "[]") ? Vehiculo.P_Dato_Modificacion : "";
            Txt_Usuario_creo.Text = Vehiculo.P_Dato_Creacion;
            Hdf_Vehiculo_Aseduradora_ID.Value = Vehiculo.P_Vehiculo_Aseguradora_ID.ToString();
            Cmb_Aseguradoras.SelectedIndex = Cmb_Aseguradoras.Items.IndexOf(Cmb_Aseguradoras.Items.FindByValue(Vehiculo.P_Aseguradora_ID));
            Txt_Numero_Poliza_Seguro.Text = Vehiculo.P_No_Poliza_Seguro;
            Txt_Numero_Inciso.Text = Vehiculo.P_Descripcion_Seguro;
            Txt_Cobertura_Seguro.Text = Vehiculo.P_Cobertura_Seguro;
            Llenar_Grid_Resguardantes(0, Vehiculo.P_Resguardantes);
            Llenar_Combo_Empleados();
            Llenar_Grid_Historial_Resguardos(0, Vehiculo.P_Historial_Resguardos);
            Llenar_Grid_Historial_Archivos(0, Vehiculo.P_Dt_Historial_Archivos);
            Llenar_Grid_Partes(Vehiculo.P_Dt_Partes_Vehiculo, 0);
            Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio Tipo_Vehiculo_Negocio = new Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio();
            Tipo_Vehiculo_Negocio.P_Tipo_Vehiculo_ID = Vehiculo.P_Tipo_Vehiculo_ID;
            Tipo_Vehiculo_Negocio = Tipo_Vehiculo_Negocio.Consultar_Datos_Vehiculo();
            Llenar_Grid_Detalles_Vehiculo(Tipo_Vehiculo_Negocio.P_Dt_Detalles);
            if (Vehiculo.P_Dt_Detalles != null && Vehiculo.P_Dt_Detalles.Rows.Count > 0) {
                Cargar_Grid_Detalles_Vehiculo(Vehiculo.P_Dt_Detalles);
            }
            if (Vehiculo.P_Empleado_Operador != null && Vehiculo.P_Empleado_Operador.Trim().Length > 0) {
                Hdf_Resguardo_Completo_Operador.Value = Vehiculo.P_Empleado_Operador.Trim();
                Cls_Cat_Empleados_Negocios Empleado_Negocio = new Cls_Cat_Empleados_Negocios();
                Empleado_Negocio.P_Empleado_ID = Hdf_Resguardo_Completo_Operador.Value.Trim();
                DataTable Dt_Datos_Empleado = Empleado_Negocio.Consulta_Empleados_General();
                String Texto = "[" + ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_RFC].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_RFC].ToString() : "") + "]";
                Texto = Texto.Trim() + " " + ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Paterno].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Paterno].ToString() : "");
                Texto = Texto.Trim() + " " + ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Materno].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Materno].ToString() : "");
                Texto = Texto.Trim() + " " + ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Nombre].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Nombre].ToString() : "");
                Txt_Resguardo_Completo_Operador.Text = Texto.Trim();
            }
            if (Vehiculo.P_Empleado_Funcionario_Recibe != null && Vehiculo.P_Empleado_Funcionario_Recibe.Trim().Length > 0) {
                Hdf_Resguardo_Completo_Funcionario_Recibe.Value = Vehiculo.P_Empleado_Funcionario_Recibe.Trim();
                Cls_Cat_Empleados_Negocios Empleado_Negocio = new Cls_Cat_Empleados_Negocios();
                Empleado_Negocio.P_Empleado_ID = Hdf_Resguardo_Completo_Funcionario_Recibe.Value.Trim();
                DataTable Dt_Datos_Empleado = Empleado_Negocio.Consulta_Empleados_General();
                String Texto = "[" + ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_RFC].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_RFC].ToString() : "") + "]";
                Texto = Texto.Trim() + " " + ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Paterno].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Paterno].ToString() : "");
                Texto = Texto.Trim() + " " + ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Materno].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Materno].ToString() : "");
                Texto = Texto.Trim() + " " + ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Nombre].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Nombre].ToString() : "");
                Txt_Resguardo_Completo_Funcionario_Recibe.Text = Texto.Trim();
            }
            if (Vehiculo.P_Empleado_Autorizo != null && Vehiculo.P_Empleado_Autorizo.Trim().Length > 0) {
                Hdf_Resguardo_Completo_Autorizo.Value = Vehiculo.P_Empleado_Autorizo.Trim();
                Cls_Cat_Empleados_Negocios Empleado_Negocio = new Cls_Cat_Empleados_Negocios();
                Empleado_Negocio.P_Empleado_ID = Hdf_Resguardo_Completo_Autorizo.Value.Trim();
                DataTable Dt_Datos_Empleado = Empleado_Negocio.Consulta_Empleados_General();
                String Texto = "[" + ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_RFC].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_RFC].ToString() : "") + "]";
                Texto = Texto.Trim() + " " + ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Paterno].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Paterno].ToString() : "");
                Texto = Texto.Trim() + " " + ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Materno].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Materno].ToString() : "");
                Texto = Texto.Trim() + " " + ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Nombre].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Nombre].ToString() : "");
                Txt_Resguardo_Completo_Autorizo.Text = Texto.Trim();
            }
            Tab_Contenedor_Pestagnas.ActiveTabIndex = 0;
            System.Threading.Thread.Sleep(1000);
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
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
    private void Remover_Sesiones_Control_AsyncFileUpload(String Cliente_ID)
    {
        HttpContext Contexto;
        if (HttpContext.Current != null && HttpContext.Current.Session != null)
        {
            Contexto = HttpContext.Current;
        }
        else
        {
            Contexto = null;
        }
        if (Contexto != null)
        {
            foreach (String key in Contexto.Session.Keys)
            {
                if (key.Contains(Cliente_ID))
                {
                    Contexto.Session.Remove(key);
                    break;
                }
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Partes
    ///DESCRIPCIÓN: Llena la tabla de Resguardantes
    ///PROPIEDADES:     
    ///             1.  Lista_Partes. Lista de objetos de donde se llenará lista.
    ///             1.  Pagina. Pagina donde se establecera el Grid despues de llenarlo.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 16/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Partes(DataTable Dt_Partes, Int32 Pagina)
    {
        try
        {
            Grid_Partes.Columns[0].Visible = true;
            Grid_Partes.Columns[1].Visible = true;
            if (Dt_Partes != null && Dt_Partes.Rows.Count > 0) {
                Session["Partes_Vehiculo"] = Dt_Partes;
            } else {
                Session.Remove("Partes_Vehiculo");
            }
            Grid_Partes.SelectedIndex = (-1);
            Grid_Partes.DataSource = Dt_Partes;
            Grid_Partes.PageIndex = Pagina;
            Grid_Partes.DataBind();
            Grid_Partes.Columns[0].Visible = false;
            Grid_Partes.Columns[1].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }



    #region Modal Busqueda

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Empleados_Busqueda
    ///DESCRIPCIÓN: Llena el combo de Empleados del Modal de Busqueda.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 06/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Empleados_Busqueda(DataTable Tabla)
    {
        try
        {
            DataRow Fila_Empleado = Tabla.NewRow();
            Fila_Empleado["EMPLEADO_ID"] = "TODOS";
            Fila_Empleado["NOMBRE"] = HttpUtility.HtmlDecode("&lt;TODOS&gt;");
            Tabla.Rows.InsertAt(Fila_Empleado, 0);
            Cmb_Busqueda_Nombre_Resguardante.DataSource = Tabla;
            Cmb_Busqueda_Nombre_Resguardante.DataValueField = "EMPLEADO_ID";
            Cmb_Busqueda_Nombre_Resguardante.DataTextField = "NOMBRE";
            Cmb_Busqueda_Nombre_Resguardante.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combos_Independientes
    ///DESCRIPCIÓN: Se llenan los Combos Generales Independientes.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 06/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combos_Independientes()
    {
        try
        {
            Cls_Ope_Pat_Com_Vehiculos_Negocio Combos = new Cls_Ope_Pat_Com_Vehiculos_Negocio();

            //SE LLENA EL COMBO DE DEPENDENCIAS DE LAS BUSQUEDAS
            Combos.P_Tipo_DataTable = "DEPENDENCIAS";
            DataTable Dependencias = Combos.Consultar_DataTable();
            DataRow Fila_Dependencia = Dependencias.NewRow();
            Fila_Dependencia["DEPENDENCIA_ID"] = "TODAS";
            Fila_Dependencia["NOMBRE"] = HttpUtility.HtmlDecode("&lt;TODAS&gt;");
            Dependencias.Rows.InsertAt(Fila_Dependencia, 0);
            Cmb_Busqueda_Dependencias.DataSource = Dependencias;
            Cmb_Busqueda_Dependencias.DataValueField = "DEPENDENCIA_ID";
            Cmb_Busqueda_Dependencias.DataTextField = "NOMBRE";
            Cmb_Busqueda_Dependencias.DataBind();
            Cmb_Busqueda_Dependencia.DataSource = Dependencias;
            Cmb_Busqueda_Dependencia.DataValueField = "DEPENDENCIA_ID";
            Cmb_Busqueda_Dependencia.DataTextField = "NOMBRE";
            Cmb_Busqueda_Dependencia.DataBind();
            Cmb_Busqueda_Resguardantes_Dependencias.DataSource = Dependencias;
            Cmb_Busqueda_Resguardantes_Dependencias.DataValueField = "DEPENDENCIA_ID";
            Cmb_Busqueda_Resguardantes_Dependencias.DataTextField = "NOMBRE";
            Cmb_Busqueda_Resguardantes_Dependencias.DataBind();
            Cmb_Busqueda_Resguardantes_Dependencias.DataSource = Dependencias;
            Cmb_Busqueda_Resguardantes_Dependencias.DataValueField = "DEPENDENCIA_ID";
            Cmb_Busqueda_Resguardantes_Dependencias.DataTextField = "NOMBRE";
            Cmb_Busqueda_Resguardantes_Dependencias.DataBind();
            Dependencias.Rows.RemoveAt(0);
            Fila_Dependencia["DEPENDENCIA_ID"] = "SELECCIONE";
            Fila_Dependencia["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Dependencias.Rows.InsertAt(Fila_Dependencia, 0);
            Cmb_Dependencias.DataSource = Dependencias;
            Cmb_Dependencias.DataValueField = "DEPENDENCIA_ID";
            Cmb_Dependencias.DataTextField = "NOMBRE";
            Cmb_Dependencias.DataBind();

            //SE LLENA EL COMBO DE MARCAS DE LAS BUSQUEDAS
            Combos.P_Tipo_DataTable = "MARCAS";
            DataTable Marcas = Combos.Consultar_DataTable();
            DataRow Fila_Marca = Marcas.NewRow();
            Fila_Marca["MARCA_ID"] = "TODAS";
            Fila_Marca["NOMBRE"] = HttpUtility.HtmlDecode("&lt;TODAS&gt;");
            Marcas.Rows.InsertAt(Fila_Marca, 0);
            Cmb_Busqueda_Marca.DataSource = Marcas;
            Cmb_Busqueda_Marca.DataTextField = "NOMBRE";
            Cmb_Busqueda_Marca.DataValueField = "MARCA_ID";
            Cmb_Busqueda_Marca.DataBind();
            Marcas.Rows.RemoveAt(0);
            Fila_Marca["MARCA_ID"] = "SELECCIONE";
            Fila_Marca["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Marcas.Rows.InsertAt(Fila_Marca, 0);
            Cmb_Marca.DataSource = Marcas;
            Cmb_Marca.DataTextField = "NOMBRE";
            Cmb_Marca.DataValueField = "MARCA_ID";
            Cmb_Marca.DataBind();

            ////SE LLENA EL COMBO DE MODELOS DE LAS BUSQUEDAS
            //Combos.P_Tipo_DataTable = "MODELOS";
            //DataTable Modelos = Combos.Consultar_DataTable();
            //DataRow Fila_Modelo = Modelos.NewRow();
            //Fila_Modelo["MODELO_ID"] = "TODAS";
            //Fila_Modelo["NOMBRE"] = HttpUtility.HtmlDecode("&lt;TODAS&gt;");
            //Modelos.Rows.InsertAt(Fila_Modelo, 0);
            //Cmb_Busqueda_Modelo.DataSource = Modelos;
            //Cmb_Busqueda_Modelo.DataTextField = "NOMBRE";
            //Cmb_Busqueda_Modelo.DataValueField = "MODELO_ID";
            //Cmb_Busqueda_Modelo.DataBind();
            //Modelos.Rows.RemoveAt(0);
            //Fila_Modelo["MODELO_ID"] = "SELECCIONE";
            //Fila_Modelo["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            //Modelos.Rows.InsertAt(Fila_Modelo, 0);
            //Cmb_Modelo.DataSource = Modelos;
            //Cmb_Modelo.DataTextField = "NOMBRE";
            //Cmb_Modelo.DataValueField = "MODELO_ID";
            //Cmb_Modelo.DataBind();

            //SE LLENA EL COMBO DE TIPO DE VEHICULOS DE LAS BUSQUEDAS
            Combos.P_Tipo_DataTable = "TIPOS_VEHICULOS";
            DataTable Tipos_Vehiculos = Combos.Consultar_DataTable();
            DataRow Fila_Tipo_Vehiculo = Tipos_Vehiculos.NewRow();
            Fila_Tipo_Vehiculo["TIPO_VEHICULO_ID"] = "TODOS";
            Fila_Tipo_Vehiculo["DESCRIPCION"] = HttpUtility.HtmlDecode("&lt;TODOS&gt;");
            Tipos_Vehiculos.Rows.InsertAt(Fila_Tipo_Vehiculo, 0);
            Cmb_Busqueda_Tipo_Vehiculo.DataSource = Tipos_Vehiculos;
            Cmb_Busqueda_Tipo_Vehiculo.DataTextField = "DESCRIPCION";
            Cmb_Busqueda_Tipo_Vehiculo.DataValueField = "TIPO_VEHICULO_ID";
            Cmb_Busqueda_Tipo_Vehiculo.DataBind();
            Tipos_Vehiculos.Rows.RemoveAt(0);
            Fila_Tipo_Vehiculo["TIPO_VEHICULO_ID"] = "SELECCIONE";
            Fila_Tipo_Vehiculo["DESCRIPCION"] = HttpUtility.HtmlDecode("&lt;SELEECIONE&gt;");
            Tipos_Vehiculos.Rows.InsertAt(Fila_Tipo_Vehiculo, 0);
            Cmb_Tipos_Vehiculos.DataSource = Tipos_Vehiculos;
            Cmb_Tipos_Vehiculos.DataTextField = "DESCRIPCION";
            Cmb_Tipos_Vehiculos.DataValueField = "TIPO_VEHICULO_ID";
            Cmb_Tipos_Vehiculos.DataBind();


            //SE LLENA EL COMBO DE TIPO DE COMBUSTIBLE DE LAS BUSQUEDAS
            Combos.P_Tipo_DataTable = "TIPOS_COMBUSTIBLE";
            DataTable Tipos_Combustible = Combos.Consultar_DataTable();
            DataRow Fila_Tipos_Combustible = Tipos_Combustible.NewRow();
            Fila_Tipos_Combustible["TIPO_COMBUSTIBLE_ID"] = "TODOS";
            Fila_Tipos_Combustible["DESCRIPCION"] = HttpUtility.HtmlDecode("&lt;TODOS&gt;");
            Tipos_Combustible.Rows.InsertAt(Fila_Tipos_Combustible, 0);
            Cmb_Busqueda_Tipo_Combustible.DataSource = Tipos_Combustible;
            Cmb_Busqueda_Tipo_Combustible.DataTextField = "DESCRIPCION";
            Cmb_Busqueda_Tipo_Combustible.DataValueField = "TIPO_COMBUSTIBLE_ID";
            Cmb_Busqueda_Tipo_Combustible.DataBind();
            Tipos_Combustible.Rows.RemoveAt(0);
            Fila_Tipos_Combustible["TIPO_COMBUSTIBLE_ID"] = "SELECCIONE";
            Fila_Tipos_Combustible["DESCRIPCION"] = HttpUtility.HtmlDecode("&lt;SELEECIONE&gt;");
            Tipos_Combustible.Rows.InsertAt(Fila_Tipos_Combustible, 0);
            Cmb_Tipo_Combustible.DataSource = Tipos_Combustible;
            Cmb_Tipo_Combustible.DataTextField = "DESCRIPCION";
            Cmb_Tipo_Combustible.DataValueField = "TIPO_COMBUSTIBLE_ID";
            Cmb_Tipo_Combustible.DataBind();

            //SE LLENA EL COMBO DE COLORES DE LAS BUSQUEDAS
            Combos.P_Tipo_DataTable = "COLORES";
            DataTable Tipos_Colores = Combos.Consultar_DataTable();
            DataRow Fila_Color = Tipos_Colores.NewRow();
            Fila_Color["COLOR_ID"] = "TODOS";
            Fila_Color["DESCRIPCION"] = HttpUtility.HtmlDecode("&lt;TODOS&gt;");
            Tipos_Colores.Rows.InsertAt(Fila_Color, 0);
            Cmb_Busqueda_Color.DataSource = Tipos_Colores;
            Cmb_Busqueda_Color.DataTextField = "DESCRIPCION";
            Cmb_Busqueda_Color.DataValueField = "COLOR_ID";
            Cmb_Busqueda_Color.DataBind();
            Tipos_Colores.Rows.RemoveAt(0);
            Fila_Color["COLOR_ID"] = "SELECCIONE";
            Fila_Color["DESCRIPCION"] = HttpUtility.HtmlDecode("&lt;SELEECIONE&gt;");
            Tipos_Colores.Rows.InsertAt(Fila_Color, 0);
            Cmb_Colores.DataSource = Tipos_Colores;
            Cmb_Colores.DataTextField = "DESCRIPCION";
            Cmb_Colores.DataValueField = "COLOR_ID";
            Cmb_Colores.DataBind();
            Cmb_Color_Parte.DataSource = Tipos_Colores;
            Cmb_Color_Parte.DataTextField = "DESCRIPCION";
            Cmb_Color_Parte.DataValueField = "COLOR_ID";
            Cmb_Color_Parte.DataBind();

            //SE LLENA EL COMBO DE ZONAS DE LAS BUSQUEDAS
            Combos.P_Tipo_DataTable = "ZONAS";
            DataTable Zonas = Combos.Consultar_DataTable();
            DataRow Fila_Zona = Zonas.NewRow();
            Fila_Zona["ZONA_ID"] = "TODOS";
            Fila_Zona["DESCRIPCION"] = HttpUtility.HtmlDecode("&lt;TODOS&gt;");
            Zonas.Rows.InsertAt(Fila_Zona, 0);
            Cmb_Busqueda_Zonas.DataSource = Zonas;
            Cmb_Busqueda_Zonas.DataTextField = "DESCRIPCION";
            Cmb_Busqueda_Zonas.DataValueField = "ZONA_ID";
            Cmb_Busqueda_Zonas.DataBind();
            Zonas.Rows.RemoveAt(0);
            Fila_Zona["ZONA_ID"] = "SELECCIONE";
            Fila_Zona["DESCRIPCION"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Zonas.Rows.InsertAt(Fila_Zona, 0);
            Cmb_Zonas.DataSource = Zonas;
            Cmb_Zonas.DataTextField = "DESCRIPCION";
            Cmb_Zonas.DataValueField = "ZONA_ID";
            Cmb_Zonas.DataBind();

            //SE LLENA EL COMBO DE ASEGURADORAS
            Combos.P_Tipo_DataTable = "ASEGURADORAS";
            DataTable Aseguradoras = Combos.Consultar_DataTable();
            DataRow Fila_Aseguradora = Aseguradoras.NewRow();
            Fila_Aseguradora["ASEGURADORA_ID"] = "SELECCIONE";
            Fila_Aseguradora["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Aseguradoras.Rows.InsertAt(Fila_Aseguradora, 0);
            Cmb_Aseguradoras.DataSource = Aseguradoras;
            Cmb_Aseguradoras.DataValueField = "ASEGURADORA_ID";
            Cmb_Aseguradoras.DataTextField = "NOMBRE";
            Cmb_Aseguradoras.DataBind();


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

            Cls_Cat_Com_Proveedores_Negocio Proveedores = new Cls_Cat_Com_Proveedores_Negocio();
            Cmb_Proveedor.DataSource = Proveedores.Consulta_Datos_Proveedores();
            Cmb_Proveedor.DataTextField = Cat_Com_Proveedores.Campo_Nombre;
            Cmb_Proveedor.DataValueField = Cat_Com_Proveedores.Campo_Proveedor_ID;
            Cmb_Proveedor.DataBind();
            Cmb_Proveedor.Items.Insert(0, new ListItem("<-- SELECCIONE -->", ""));

            Cls_Cat_Pat_Com_Clases_Activo_Negocio CA_Negocio = new Cls_Cat_Pat_Com_Clases_Activo_Negocio();
            CA_Negocio.P_Estatus = "VIGENTE";
            CA_Negocio.P_Tipo_DataTable = "CLASES_ACTIVOS";
            Cmb_Clase_Activo.DataSource = CA_Negocio.Consultar_DataTable();
            Cmb_Clase_Activo.DataValueField = "CLASE_ACTIVO_ID";
            Cmb_Clase_Activo.DataTextField = "CLAVE_DESCRIPCION";
            Cmb_Clase_Activo.DataBind();
            Cmb_Clase_Activo.Items.Insert(0, new ListItem("<- SELECCIONE ->", ""));

            Cls_Cat_Pat_Com_Clasificaciones_Negocio Clasificaciones_Negocio = new Cls_Cat_Pat_Com_Clasificaciones_Negocio();
            Clasificaciones_Negocio.P_Estatus = "VIGENTE";
            Clasificaciones_Negocio.P_Tipo_DataTable = "CLASIFICACIONES";
            Cmb_Tipo_Activo.DataSource = Clasificaciones_Negocio.Consultar_DataTable();
            Cmb_Tipo_Activo.DataValueField = "CLASIFICACION_ID";
            Cmb_Tipo_Activo.DataTextField = "CLAVE_DESCRIPCION";
            Cmb_Tipo_Activo.DataBind();
            Cmb_Tipo_Activo.Items.Insert(0, new ListItem("<- SELECCIONE ->", ""));
        }
        catch (Exception Ex)
        {
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
    ///FECHA_CREO: 03/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Listado_Vehiculos(Int32 Pagina) {
        try {
            Grid_Listado_Vehiculos.Columns[1].Visible = true;
            Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculos = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
            Vehiculos.P_Tipo_DataTable = "VEHICULOS";
            if (Session["FILTRO_BUSQUEDA"] != null) {
                Vehiculos.P_Tipo_Filtro_Busqueda = Session["FILTRO_BUSQUEDA"].ToString();
                if (Session["FILTRO_BUSQUEDA"].ToString().Trim().Equals("DATOS_GENERALES")) {
                    if (Txt_Busqueda_Numero_Inventario.Text.Trim().Length > 0) { Vehiculos.P_Numero_Inventario = Convert.ToInt64(Txt_Busqueda_Numero_Inventario.Text.Trim()); }
                    if (Txt_Busqueda_Numero_Economico.Text.Trim().Length > 0) { Vehiculos.P_Numero_Economico_ = Txt_Busqueda_Numero_Economico.Text.Trim(); }
                    if (Txt_Busqueda_Anio_Fabricacion.Text.Trim().Length > 0) { Vehiculos.P_Anio_Fabricacion = Convert.ToInt32(Txt_Busqueda_Anio_Fabricacion.Text.Trim()); }
                    Vehiculos.P_Modelo_ID = Txt_Modelo_Busqueda.Text.Trim();
                    if (Cmb_Busqueda_Marca.SelectedIndex > 0) {
                        Vehiculos.P_Marca_ID = Cmb_Busqueda_Marca.SelectedItem.Value.Trim();
                    }
                    if (Cmb_Busqueda_Tipo_Vehiculo.SelectedIndex > 0) {
                        Vehiculos.P_Tipo_Vehiculo_ID = Cmb_Busqueda_Tipo_Vehiculo.SelectedItem.Value.Trim();
                    }
                    if (Cmb_Busqueda_Tipo_Combustible.SelectedIndex > 0) {
                        Vehiculos.P_Tipo_Combustible_ID = Cmb_Busqueda_Tipo_Combustible.SelectedItem.Value.Trim();
                    }
                    if (Cmb_Busqueda_Color.SelectedIndex > 0) {
                        Vehiculos.P_Color_ID = Cmb_Busqueda_Color.SelectedItem.Value.Trim();
                    }
                    if (Cmb_Busqueda_Zonas.SelectedIndex > 0) {
                        Vehiculos.P_Zona_ID = Cmb_Busqueda_Zonas.SelectedItem.Value.Trim();
                    }
                    if (Cmb_Busqueda_Estatus.SelectedIndex > 0) {
                        Vehiculos.P_Estatus = Cmb_Busqueda_Estatus.SelectedItem.Value.Trim();
                    }
                    if (Cmb_Busqueda_Dependencias.SelectedIndex > 0) {
                        Vehiculos.P_Dependencia_ID = Cmb_Busqueda_Dependencias.SelectedItem.Value.Trim();
                    }
                } else if (Session["FILTRO_BUSQUEDA"].ToString().Trim().Equals("RESGUARDANTES")) {
                    Vehiculos.P_RFC_Resguardante = Txt_Busqueda_RFC_Resguardante.Text.Trim();
                    if (Cmb_Busqueda_Resguardantes_Dependencias.SelectedIndex > 0) {
                        Vehiculos.P_Dependencia_ID = Cmb_Busqueda_Resguardantes_Dependencias.SelectedItem.Value.Trim();
                    }
                    if (Cmb_Busqueda_Nombre_Resguardante.SelectedIndex > 0) {
                        Vehiculos.P_Resguardante_ID = Cmb_Busqueda_Nombre_Resguardante.SelectedItem.Value.Trim();
                    }
                }
            }
            Grid_Listado_Vehiculos.DataSource = Vehiculos.Consultar_DataTable();
            Grid_Listado_Vehiculos.PageIndex = Pagina;
            Grid_Listado_Vehiculos.DataBind();
            Grid_Listado_Vehiculos.Columns[1].Visible = false;
            MPE_Busqueda_Vehiculo.Show();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Dt_Detalles
    ///DESCRIPCIÓN: Se carga un DataTable con los datos del Grid de Detalles.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 09/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private DataTable Cargar_Dt_Detalles() {
        DataTable Dt_Detalles = new DataTable();
        Dt_Detalles.Columns.Add("DETALLE_ID", Type.GetType("System.String"));
        Dt_Detalles.Columns.Add("NOMBRE", Type.GetType("System.String"));
        Dt_Detalles.Columns.Add("ESTADO", Type.GetType("System.String"));
        for (Int32 Contador = 0; Contador < Grid_Detalles_Vehiculo.Rows.Count; Contador++) {
            DataRow Fila = Dt_Detalles.NewRow();
            Fila["DETALLE_ID"] = Grid_Detalles_Vehiculo.Rows[Contador].Cells[0].Text.Trim();
            Fila["NOMBRE"] = Grid_Detalles_Vehiculo.Rows[Contador].Cells[1].Text.Trim();
            if (Grid_Detalles_Vehiculo.Rows[Contador].FindControl("Cmb_Estado_Detalle") != null) {
                DropDownList Combo = (DropDownList)Grid_Detalles_Vehiculo.Rows[Contador].FindControl("Cmb_Estado_Detalle");
                Fila["ESTADO"] = Combo.SelectedItem.Value;
            } else {
                Fila["ESTADO"] = "";
            }
            Dt_Detalles.Rows.Add(Fila);
        }
        return Dt_Detalles;
    }

    #endregion

    #region Validaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Generales
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación de la pestaña de Generales.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 07/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Componentes_Generales()
    {
        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Txt_Numero_Economico.Text.Trim().Length == 0)
        {
            Mensaje_Error = Mensaje_Error + "+ Introducir el Número Económico del Vehículo.";
            Validacion = false;
        }
        if (Cmb_Tipos_Vehiculos.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Tipos de Vehículos.";
            Validacion = false;
        }
        if (Cmb_Tipo_Combustible.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Tipos de Combustible.";
            Validacion = false;
        }
        if (Cmb_Colores.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Colores.";
            Validacion = false;
        }
        if (Cmb_Zonas.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Zonas.";
            Validacion = false;
        }
        if (Txt_Placas.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir las Placas del Vehículo.";
            Validacion = false;
        }
        if (Txt_Anio_Fabricacion.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Año de Fabricación del Vehículo.";
            Validacion = false;
        }
        if (Txt_Serie_Carroceria.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Número de Serie del Vehículo.";
            Validacion = false;
        }
        if (Txt_Numero_Cilindros.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Número de Cilindros del Vehículo.";
            Validacion = false;
        }
        if (Txt_Kilometraje.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Kilometraje del Vehículo.";
            Validacion = false;
        }
        if (Cmb_Estatus.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Estatus.";
            Validacion = false;
        }
        else
        {
            if (!Cmb_Estatus.SelectedItem.Value.Equals("VIGENTE"))
            {
                if (Txt_Motivo_Baja.Text.Trim().Length == 0)
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir el Motivo de la Baja del Vehículo.";
                    Validacion = false;
                }
            }
        }
        if (Cmb_Odometro.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del Combo de Odomentro.";
            Validacion = false;
        }
        if (Cmb_Estatus.SelectedItem.Value.Equals("VIGENTE"))
        {
            if (Grid_Resguardantes.Rows.Count == 0 || Session["Dt_Resguardantes"] == null)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Debe haber como minimo un empleado para resguardo del Vehículo.";
                Validacion = false;
            }
        }
        if (!Validacion)
        {
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Validacion;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Resguardos
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación de la pestaña de Resguardos.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 07/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Componentes_Resguardos()
    {
        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Cmb_Empleados.SelectedIndex == 0)
        {
            Mensaje_Error = Mensaje_Error + "+ Seleccionar el Empleado para Resguardo.";
            Validacion = false;
        }
        if (!Validacion)
        {
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Validacion;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Firmas
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 20/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Firmas()
    {
        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Hdf_Resguardo_Completo_Operador.Value.Trim().Length == 0) {
            Mensaje_Error = Mensaje_Error + "+ Introducir el Nombre y RFC del Operador.";
            Validacion = false;
        }
        if (Hdf_Resguardo_Completo_Funcionario_Recibe.Value.Trim().Length == 0) {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Nombre y RFC del Funcionario que Recibe.";
            Validacion = false;
        }
        if (Hdf_Resguardo_Completo_Autorizo.Value.Trim().Length == 0) {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Nombre y RFC de quien Autorizo.";
            Validacion = false;
        }
        if (!Validacion) {
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Validacion;
    }

    #endregion

    #region Reporte

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_DataSet_Resguardos_Vehiculos
    ///DESCRIPCIÓN: Llena el dataSet "Data_Set_Resguardos_Vehiculos" con las personas a las que se les asigno el
    ///vehiculo, sus detalles generales y especificos, para que con estos datos se genere el reporte.
    ///PARAMETROS:  
    ///CREO: Salvador Hernández Ramírez
    ///FECHA_CREO: 23/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_DataSet_Resguardos_Vehiculos(Cls_Ope_Pat_Com_Vehiculos_Negocio Id_Vehiculo)
    {
        try
        {

            String Formato = "PDF";
            Cls_Alm_Com_Resguardos_Negocio Consulta_Resguardos_Vehiculos = new Cls_Alm_Com_Resguardos_Negocio();
            DataSet Data_Set_Resguardos_Vehiculos, Data_Set_Vehiculos_Asegurados;
            Id_Vehiculo.P_Producto_Almacen = false;
            Data_Set_Resguardos_Vehiculos = Consulta_Resguardos_Vehiculos.Consulta_Resguardos_Vehiculos(Id_Vehiculo);
            Data_Set_Vehiculos_Asegurados = Consulta_Resguardos_Vehiculos.Consulta_Vehiculos_Asegurados(Id_Vehiculo);
            Ds_Alm_Com_Resguardos_Vehiculos Ds_Consulta_Resguardos_Vehiculos = new Ds_Alm_Com_Resguardos_Vehiculos();
            Generar_Reporte(Data_Set_Vehiculos_Asegurados, Data_Set_Resguardos_Vehiculos, Ds_Consulta_Resguardos_Vehiculos, Formato);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN: caraga el data set fisico con el cual se genera el Reporte especificado
    ///PARAMETROS:  1.-Data_Set_Consulta_DB.- Contiene la informacion de la consulta a la base de datos
    ///             2.-Ds_Reporte, Objeto que contiene la instancia del Data set fisico del Reporte a generar
    ///             3.-Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
    ///CREO: Salvador Hernández Ramírez
    ///FECHA_CREO: 15/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte(DataSet Data_Set_Consulta_Vehiculos_A, DataSet Data_Set_Consulta_Resguardos_V, DataSet Ds_Reporte, String Formato)
    {
        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";
        DataRow Renglon;

        try
        {
            if (Data_Set_Consulta_Resguardos_V.Tables[0].Rows.Count > 0)
            {
                String Cantidad = Data_Set_Consulta_Resguardos_V.Tables[0].Rows[0]["CANTIDAD"].ToString();
                String Costo = Data_Set_Consulta_Resguardos_V.Tables[0].Rows[0]["COSTO_UNITARIO"].ToString();
                Double Resultado = (Convert.ToDouble(Cantidad)) * (Convert.ToDouble(Costo));

                String Total = "" + Resultado;
                Renglon = Data_Set_Consulta_Resguardos_V.Tables[0].Rows[0];
                Ds_Reporte.Tables[1].ImportRow(Renglon);
                Ds_Reporte.Tables[1].Rows[0].SetField("COSTO_TOTAL", Total);

                for (int Cont_Elementos = 0; Cont_Elementos < Data_Set_Consulta_Resguardos_V.Tables[0].Rows.Count; Cont_Elementos++)
                {
                    Renglon = Data_Set_Consulta_Resguardos_V.Tables[0].Rows[Cont_Elementos]; //Instanciar renglon e importarlo
                    

                    String Nombre_E = Data_Set_Consulta_Resguardos_V.Tables[0].Rows[Cont_Elementos]["NOMBRE_E"].ToString();
                    String Apellido_Paterno_E = Data_Set_Consulta_Resguardos_V.Tables[0].Rows[Cont_Elementos]["APELLIDO_PATERNO_E"].ToString();
                    String Apellido_Materno_E = Data_Set_Consulta_Resguardos_V.Tables[0].Rows[Cont_Elementos]["APELLIDO_MATERNO_E"].ToString();
                    String RFC_E = Data_Set_Consulta_Resguardos_V.Tables[0].Rows[Cont_Elementos]["RFC_E"].ToString();
                    String Resguardante = Nombre_E + " " + Apellido_Paterno_E + " " + Apellido_Materno_E + " " + "(" + RFC_E + ")";
                    if (!Resguardante.Trim().Equals("()")) {
                        Ds_Reporte.Tables[0].ImportRow(Renglon);
                        Ds_Reporte.Tables[0].Rows[Cont_Elementos].SetField("RESGUARDANTES", Resguardante);
                    }
                }

                if (Data_Set_Consulta_Vehiculos_A.Tables[0].Rows.Count > 0)
                {
                    String Nombre_Aeguradora = Data_Set_Consulta_Vehiculos_A.Tables[0].Rows[0]["NOMBRE_ASEGURADORA"].ToString();
                    String No_Poliza = Data_Set_Consulta_Vehiculos_A.Tables[0].Rows[0]["NO_POLIZA"].ToString();
                    String Descripcion_Seguro = Data_Set_Consulta_Vehiculos_A.Tables[0].Rows[0]["DESCRIPCION_SEGURO"].ToString();
                    String Cobertura = Data_Set_Consulta_Vehiculos_A.Tables[0].Rows[0]["COBERTURA"].ToString();
                    Ds_Reporte.Tables[1].Rows[0].SetField("NOMBRE_ASEGURADORA", Nombre_Aeguradora);
                    Ds_Reporte.Tables[1].Rows[0].SetField("NO_POLIZA", No_Poliza);
                    Ds_Reporte.Tables[1].Rows[0].SetField("DESCRIPCION_SEGURO", Descripcion_Seguro);
                    Ds_Reporte.Tables[1].Rows[0].SetField("COBERTURA", Cobertura);
                }
            }

            // Ruta donde se encuentra el Reporte Crystal
            Ruta_Reporte_Crystal = "../Rpt/Compras/Rpt_Alm_Com_Resguardos_Vehiculos.rpt";

            // Se da el nombre del reporte que se va generar
            if (Formato == "PDF")
                Nombre_Reporte_Generar = "Rpt_Resguardo_Vehiculos" + Session.SessionID + ".pdf";  // Es el nombre del reporte PDF que se va a generar
            else if (Formato == "Excel")
                Nombre_Reporte_Generar = "Rpt_Resguardo_Vehiculos" + Session.SessionID + ".xls";  // Es el nombre del repote en Excel que se va a generar

            Cls_Reportes Reportes = new Cls_Reportes();
            Reportes.Generar_Reporte(ref Ds_Reporte, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, Formato);
            Mostrar_Reporte(Nombre_Reporte_Generar, Formato);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al llenar el DataSet. Error: [" + Ex.Message + "]");
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
    /// FECHA MODIFICO:      23-Mayo-2011
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
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte_Completo
    ///DESCRIPCIÓN: caraga el data set fisico con el cual se genera el Reporte especificado
    ///PARAMETROS:  1.-Data_Set_Consulta_DB.- Contiene la informacion de la consulta a la base de datos
    ///             2.-Ds_Reporte, Objeto que contiene la instancia del Data set fisico del Reporte a generar
    ///             3.-Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 12/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte_Completo(DataSet Data_Set_Consulta_DB, DataSet Ds_Reporte, string Nombre_Reporte) {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Compras/" + Nombre_Reporte);
        Reporte.Load(File_Path);
        Ds_Reporte = Data_Set_Consulta_DB;
        Reporte.SetDataSource(Ds_Reporte);
        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/Rpt_Pat_Completo_Vehiculos.pdf");
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
        Reporte.Export(Export_Options);
        String Ruta = "../../Reporte/Rpt_Pat_Completo_Vehiculos.pdf";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
    }

    #endregion

    private void Consultar_Empleados(String Dependencia_ID) { 
        try {
            Session.Remove("Dt_Resguardantes");
            Grid_Resguardantes.DataSource = new DataTable();
            Grid_Resguardantes.DataBind();
            if (Cmb_Dependencias.SelectedIndex > 0) {
                Cls_Ope_Pat_Com_Vehiculos_Negocio Combo = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
                Combo.P_Tipo_DataTable = "EMPLEADOS";
                Combo.P_Dependencia_ID = Dependencia_ID;
                DataTable Tabla = Combo.Consultar_DataTable();
                Llenar_Combo_Empleados(Tabla);
            } else {
                DataTable Tabla = new DataTable();
                Tabla.Columns.Add("EMPLEADO_ID", Type.GetType("System.String"));
                Tabla.Columns.Add("NOMBRE", Type.GetType("System.String"));
                Llenar_Combo_Empleados(Tabla);
            }
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    
    #region "Busqueda Resguardantes"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Busqueda_Empleados_Resguardo
        ///DESCRIPCIÓN: Llena el Grid con los empleados que cumplan el filtro
        ///PROPIEDADES:     
        ///CREO:                 
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 24/Octubre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Grid_Busqueda_Empleados_Resguardo() {
            Grid_Busqueda_Empleados_Resguardo.SelectedIndex = (-1);
            Grid_Busqueda_Empleados_Resguardo.Columns[1].Visible = true;
            Cls_Ope_Pat_Com_Vehiculos_Negocio Negocio = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
            if (Txt_Busqueda_No_Empleado.Text.Trim().Length > 0) { Negocio.P_No_Empleado_Resguardante = Txt_Busqueda_No_Empleado.Text.Trim(); }
            if (Txt_Busqueda_RFC.Text.Trim().Length > 0) { Negocio.P_RFC_Resguardante = Txt_Busqueda_RFC.Text.Trim(); }
            if (Txt_Busqueda_Nombre_Empleado.Text.Trim().Length > 0) { Negocio.P_Nombre_Resguardante = Txt_Busqueda_Nombre_Empleado.Text.Trim(); }
            if (Cmb_Busqueda_Dependencia.SelectedIndex > 0) { Negocio.P_Dependencia_ID = Cmb_Busqueda_Dependencia.SelectedItem.Value; }
            Grid_Busqueda_Empleados_Resguardo.DataSource = Negocio.Consultar_Empleados_Resguardos();
            Grid_Busqueda_Empleados_Resguardo.DataBind();
            Grid_Busqueda_Empleados_Resguardo.Columns[1].Visible = false;
        }

    #endregion
    #endregion

    #region Grid

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Vehiculos_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de Vehiculos del Modal de Busqueda
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 07/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Listado_Vehiculos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Listado_Vehiculos.SelectedIndex = (-1);
            Llenar_Grid_Listado_Vehiculos(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
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
    ///FECHA_CREO: 07/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Listado_Vehiculos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Listado_Vehiculos.SelectedIndex > (-1))
            {
                String Vehiculo_Seleccionado_ID = Grid_Listado_Vehiculos.SelectedRow.Cells[1].Text.Trim();
                Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
                Vehiculo.P_Vehiculo_ID = Vehiculo_Seleccionado_ID;
                Vehiculo = Vehiculo.Consultar_Detalles_Vehiculo();
                Mostrar_Detalles_Vehiculo(Vehiculo);
                Grid_Listado_Vehiculos.SelectedIndex = -1;
                MPE_Busqueda_Vehiculo.Hide();
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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Historial_Resguardantes_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de Historial de Resguardos
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 14/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Historial_Resguardantes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            if (Session["Dt_Historial_Resguardos"] != null)
            {
                Grid_Historial_Resguardantes.SelectedIndex = (-1);
                Llenar_Grid_Historial_Resguardos(e.NewPageIndex, (DataTable)Session["Dt_Historial_Resguardos"]);
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
    ///NOMBRE DE LA FUNCIÓN: Grid_Historial_Resguardantes_SelectedIndexChanged
    ///DESCRIPCIÓN: Maneja el evento de cambio de Seleccion del GridView de Historial
    ///             de Resguardos.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 14/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Historial_Resguardantes_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Historial_Resguardantes.SelectedIndex > (-1))
            {
                Limpiar_Historial_Resguardantes();
                if (Session["Dt_Historial_Resguardos"] != null)
                {
                    Int32 Registro = ((Grid_Historial_Resguardantes.PageIndex) * Grid_Historial_Resguardantes.PageSize) + (Grid_Historial_Resguardantes.SelectedIndex);
                    DataTable Tabla = (DataTable)Session["Dt_Historial_Resguardos"];
                    Txt_Historial_Empleado_Resguardo.Text = Tabla.Rows[Registro][2].ToString().Trim();
                    Txt_Historial_Comentarios_Resguardo.Text = Tabla.Rows[Registro][3].ToString().Trim();
                    Txt_Historial_Fecha_Inicial_Resguardo.Text = String.Format("{0:dd/MMM/yyyy}", Tabla.Rows[Registro][4]);
                    Txt_Historial_Fecha_Final_Resguardo.Text = String.Format("{0:dd/MMM/yyyy}", Tabla.Rows[Registro][5]);
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
    ///NOMBRE DE LA FUNCIÓN: Grid_Archivos_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de Historial de Archivos
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 16/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Archivos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            if (Session["Dt_Historial_Archivos"] != null)
            {
                Grid_Archivos.SelectedIndex = (-1);
                Llenar_Grid_Historial_Archivos(e.NewPageIndex, (DataTable)Session["Dt_Historial_Archivos"]);
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
    ///NOMBRE DE LA FUNCIÓN: Grid_Archivos_RowDataBound
    ///DESCRIPCIÓN: Maneja el evento de RowDataBound del Grid de Archivos
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 16/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Archivos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton Boton = (ImageButton)e.Row.FindControl("Btn_Ver_Archivo");
            Boton.CommandArgument = e.Row.Cells[0].Text.Trim();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Partes_PageIndexChanging
    ///DESCRIPCIÓN: Maneja el Cambio de Pagina de Grid_Partes
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 16/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Partes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            if (Session["Partes_Vehiculo"] != null) {
                Llenar_Grid_Partes(((DataTable)Session["Partes_Vehiculo"]), e.NewPageIndex);
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
    ///NOMBRE DE LA FUNCIÓN: Grid_Partes_SelectedIndexChanged
    ///DESCRIPCIÓN: Maneja el Cambio de Selección de Grid_Partes
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 16/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Partes_SelectedIndexChanged(object sender, EventArgs e){
        try{
            if (Grid_Partes.SelectedIndex > (-1)) {
                Int32 Parte_ID = Convert.ToInt32(Grid_Partes.SelectedRow.Cells[1].Text.Trim());
                Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio Parte = new Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio();
                Parte.P_Parte_ID = Parte_ID;
                Parte = Parte.Consultar_Datos_Parte_Vehiculo();
                Txt_Numero_Inventario_Parte.Text = Parte.P_Numero_Inventario;
                Txt_Cantidad_Parte.Text = Parte.P_Cantidad.ToString();
                Cmb_Material_Parte.SelectedIndex = Cmb_Material_Parte.Items.IndexOf(Cmb_Material_Parte.Items.FindByValue(Parte.P_Material));
                Cmb_Color_Parte.SelectedIndex = Cmb_Color_Parte.Items.IndexOf(Cmb_Color_Parte.Items.FindByValue(Parte.P_Color));
                Txt_Costo_Parte.Text = Parte.P_Costo.ToString();
                Txt_Fecha_Adquisicion_Parte.Text = String.Format("{0:dd 'de' MMMMMMMMMMMMMMM 'de' yyyy}", Parte.P_Fecha_Adquisicion);
                Cmb_Estatus_Parte.SelectedIndex = Cmb_Estatus_Parte.Items.IndexOf(Cmb_Estatus_Parte.Items.FindByValue(Parte.P_Estatus));
                Cmb_Estado_Parte.SelectedIndex = Cmb_Estado_Parte.Items.IndexOf(Cmb_Estado_Parte.Items.FindByValue(Parte.P_Estado));
                Txt_Comentarios_Parte.Text = Parte.P_Comentarios;
                System.Threading.Thread.Sleep(1000);
            }
        }  catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Resguardantes_RowDataBound
    ///DESCRIPCIÓN: Maneja el Evento RowDataBound del Grid de Resguardos.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 30/Septiembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Resguardantes_RowDataBound(object sender, GridViewRowEventArgs e) {
        try {
            if (e.Row.RowType == DataControlRowType.DataRow) { 
                if(e.Row.FindControl("Btn_Ver_Informacion_Resguardo") != null){
                    if(Session["Dt_Resguardantes"] != null){
                        ImageButton Btn_Informacion = (ImageButton) e.Row.FindControl("Btn_Ver_Informacion_Resguardo");
                        Btn_Informacion.CommandArgument = ((DataTable)Session["Dt_Resguardantes"]).Rows[e.Row.RowIndex]["COMENTARIOS"].ToString();
                    }
                }
            }
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = "Verificar.";
            Lbl_Ecabezado_Mensaje.Text = "[Excepción: '" + Ex.Message + "']";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
        
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Busqueda_Empleados_Resguardo_PageIndexChanging
    ///DESCRIPCIÓN: Maneja el evento de cambio de Página del GridView de Busqueda
    ///             de empleados.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 24/Octubre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Busqueda_Empleados_Resguardo_PageIndexChanging(object sender, GridViewPageEventArgs e) {
        try {
            Grid_Busqueda_Empleados_Resguardo.PageIndex = e.NewPageIndex;
            Llenar_Grid_Busqueda_Empleados_Resguardo();
            MPE_Resguardante.Show();
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Busqueda_Empleados_Resguardo_SelectedIndexChanged
    ///DESCRIPCIÓN: Maneja el evento de cambio de Selección del GridView de Busqueda
    ///             de empleados.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 24/Octubre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Busqueda_Empleados_Resguardo_SelectedIndexChanged(object sender, EventArgs e) { 
        try {
            if (Grid_Busqueda_Empleados_Resguardo.SelectedIndex > (-1)) {
                String Empleado_Seleccionado_ID = Grid_Busqueda_Empleados_Resguardo.SelectedRow.Cells[1].Text.Trim();
                Cls_Cat_Empleados_Negocios Empleado_Negocio = new Cls_Cat_Empleados_Negocios();
                Empleado_Negocio.P_Empleado_ID = Empleado_Seleccionado_ID.Trim();
                DataTable Dt_Datos_Empleado = Empleado_Negocio.Consulta_Empleados_General();
                if (Hdf_Tipo_Busqueda.Value == null || Hdf_Tipo_Busqueda.Value.Trim().Length == 0) { 
                    String Dependencia_ID = (!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Dependencias.Campo_Dependencia_ID].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Dependencias.Campo_Dependencia_ID].ToString() : null;
                    Int32 Index_Combo = (-1);
                    if (Dependencia_ID != null && Dependencia_ID.Trim().Length > 0) {
                        Index_Combo = Cmb_Dependencias.Items.IndexOf(Cmb_Dependencias.Items.FindByValue(Dependencia_ID));
                        if (Index_Combo > (-1)) {
                            if (Index_Combo == Cmb_Dependencias.SelectedIndex) {
                                Cmb_Empleados.SelectedIndex = Cmb_Empleados.Items.IndexOf(Cmb_Empleados.Items.FindByValue(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString()));
                            } else {
                                Cmb_Dependencias.SelectedIndex = Cmb_Dependencias.Items.IndexOf(Cmb_Dependencias.Items.FindByValue(Dependencia_ID));
                                Consultar_Empleados(Dependencia_ID);
                                Cmb_Empleados.SelectedIndex = Cmb_Empleados.Items.IndexOf(Cmb_Empleados.Items.FindByValue(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString()));
                            }
                        }
                    }
                } else if(Hdf_Tipo_Busqueda.Value.Trim().Equals("OPERADOR")) {
                    Hdf_Resguardo_Completo_Operador.Value = (!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString() : "";
                    String Texto = "[" + ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_RFC].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_RFC].ToString() : "") + "]";
                    Texto = Texto.Trim() + " " + ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Paterno].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Paterno].ToString() : "");
                    Texto = Texto.Trim() + " " + ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Materno].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Materno].ToString() : "");
                    Texto = Texto.Trim() + " " + ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Nombre].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Nombre].ToString() : "");
                    Txt_Resguardo_Completo_Operador.Text = Texto.Trim();
                } else if(Hdf_Tipo_Busqueda.Value.Trim().Equals("FUNCIONARIO_RECIBE")) {
                    Hdf_Resguardo_Completo_Funcionario_Recibe.Value = (!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString() : null;
                    String Texto = "[" + ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_RFC].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_RFC].ToString() : "") + "]";
                    Texto = Texto.Trim() + " " + ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Paterno].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Paterno].ToString() : "");
                    Texto = Texto.Trim() + " " + ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Materno].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Materno].ToString() : "");
                    Texto = Texto.Trim() + " " + ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Nombre].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Nombre].ToString() : "");
                    Txt_Resguardo_Completo_Funcionario_Recibe.Text = Texto.Trim();
                } else if(Hdf_Tipo_Busqueda.Value.Trim().Equals("AUTORIZO")) {
                    Hdf_Resguardo_Completo_Autorizo.Value = (!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString() : null;
                    String Texto = "[" + ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_RFC].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_RFC].ToString() : "") + "]";
                    Texto = Texto.Trim() + " " + ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Paterno].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Paterno].ToString() : "");
                    Texto = Texto.Trim() + " " + ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Materno].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Materno].ToString() : "");
                    Texto = Texto.Trim() + " " + ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Nombre].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Nombre].ToString() : "");
                    Txt_Resguardo_Completo_Autorizo.Text = Texto.Trim();
                }

                MPE_Resguardante.Hide();
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Prepara y Actualiza un Vehiculo con uno o mas resguardantes.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 07/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e) {
        try {
            if (Btn_Modificar.AlternateText.Equals("Modificar")) {
                if (Hdf_Vehiculo_ID.Value.Trim().Length > 0) {
                    if (!Cmb_Estatus.SelectedItem.Value.Equals("DEFINITIVA")) {
                        Configuracion_Formulario(false);
                    } else {
                        Lbl_Ecabezado_Mensaje.Text = "El Estatus del Vehículo es \"BAJA DEFINITIVA\" y no puede ser actualizado el Bien";
                        Lbl_Mensaje_Error.Text = "";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                } else {
                    Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                    Lbl_Mensaje_Error.Text = "Seleccionar el Vehículo a Modificar";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            } else {
                if (Validar_Componentes_Generales()) {
                    Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
                    Vehiculo.P_Vehiculo_ID = Hdf_Vehiculo_ID.Value.Trim();
                    Vehiculo.P_Clase_Activo_ID = Cmb_Clase_Activo.SelectedItem.Value.Trim();
                    Vehiculo.P_Clasificacion_ID = Cmb_Tipo_Activo.SelectedItem.Value.Trim();
                    Vehiculo.P_Dependencia_ID = Cmb_Dependencias.SelectedItem.Value.Trim();
                    Vehiculo.P_Numero_Economico_ = Txt_Numero_Economico.Text.Trim();
                    Vehiculo.P_Cantidad = 1;
                    Vehiculo.P_Tipo_Vehiculo_ID = Cmb_Tipos_Vehiculos.SelectedItem.Value.Trim();
                    Vehiculo.P_Tipo_Combustible_ID = Cmb_Tipo_Combustible.SelectedItem.Value.Trim();
                    Vehiculo.P_Color_ID = Cmb_Colores.SelectedItem.Value.Trim();
                    Vehiculo.P_Zona_ID = Cmb_Zonas.SelectedItem.Value.Trim();
                    Vehiculo.P_Placas = Txt_Placas.Text.Trim();
                    Vehiculo.P_Capacidad_Carga = Txt_Capacidad_Carga.Text.Trim();
                    Vehiculo.P_Anio_Fabricacion = Convert.ToInt32(Txt_Anio_Fabricacion.Text.Trim());
                    Vehiculo.P_Serie_Carroceria = Txt_Serie_Carroceria.Text.Trim();
                    Vehiculo.P_Numero_Cilindros = Convert.ToInt32(Txt_Numero_Cilindros.Text.Trim());
                    Vehiculo.P_Kilometraje = Convert.ToDouble(Txt_Kilometraje.Text.Trim());
                    Vehiculo.P_Estatus = Cmb_Estatus.SelectedItem.Value.Trim();
                    Vehiculo.P_Odometro = Cmb_Odometro.SelectedItem.Value.Trim();
                    Vehiculo.P_Observaciones = Txt_Observaciones.Text.Trim();
                    Vehiculo.P_Fecha_Adquisicion = Convert.ToDateTime(Txt_Fecha_Adquisicion.Text.Trim());
                    Vehiculo.P_No_Factura_ = Txt_No_Factura.Text.Trim();
                    if (!Cmb_Estatus.SelectedItem.Value.Equals("VIGENTE")) {
                        Vehiculo.P_Motivo_Baja = Txt_Motivo_Baja.Text.Trim();
                    }
                    if (AFU_Archivo.HasFile) {
                        Vehiculo.P_Archivo = AFU_Archivo.FileName;
                    }
                    Vehiculo.P_Vehiculo_Aseguradora_ID = (Hdf_Vehiculo_Aseduradora_ID.Value.Trim().Length > 0) ? Convert.ToInt32(Hdf_Vehiculo_Aseduradora_ID.Value) : 0;
                    Vehiculo.P_Aseguradora_ID = Cmb_Aseguradoras.SelectedItem.Value;
                    Vehiculo.P_No_Poliza_Seguro = Txt_Numero_Poliza_Seguro.Text.Trim();
                    Vehiculo.P_Descripcion_Seguro = Txt_Numero_Inciso.Text.Trim();
                    Vehiculo.P_Cobertura_Seguro = Txt_Cobertura_Seguro.Text.Trim();
                    Vehiculo.P_Resguardantes = (DataTable)Session["Dt_Resguardantes"];
                    Vehiculo.P_Dt_Detalles = Cargar_Dt_Detalles();
                    Vehiculo.P_No_Factura_ = Txt_No_Factura.Text.Trim();
                    if (Cmb_Proveedor.SelectedIndex > 0)  {
                        Vehiculo.P_Proveedor_ID = Cmb_Proveedor.SelectedItem.Value;
                    }
                    Vehiculo.P_Empleado_Operador = ((Hdf_Resguardo_Completo_Operador.Value.Trim().Length > 0) ? Hdf_Resguardo_Completo_Operador.Value.Trim() : "");
                    Vehiculo.P_Empleado_Funcionario_Recibe = ((Hdf_Resguardo_Completo_Funcionario_Recibe.Value.Trim().Length > 0) ? Hdf_Resguardo_Completo_Funcionario_Recibe.Value.Trim() : "");
                    Vehiculo.P_Empleado_Autorizo = ((Hdf_Resguardo_Completo_Autorizo.Value.Trim().Length > 0) ? Hdf_Resguardo_Completo_Autorizo.Value.Trim() : "");
                    Vehiculo.P_Usuario_Nombre = Cls_Sessiones.Nombre_Empleado;
                    Vehiculo.P_Usuario_ID = Cls_Sessiones.Empleado_ID;
                    Vehiculo.Modificar_Vehiculo();
                    if (AFU_Archivo.HasFile) {
                        String Ruta = Server.MapPath("../../" + Ope_Pat_Archivos_Bienes.Campo_Ruta_Fisica_Archivos + "/VEHICULOS/" + Vehiculo.P_Vehiculo_ID);
                        if (!Directory.Exists(Ruta)) {
                            Directory.CreateDirectory(Ruta);
                        }
                        String Archivo = Ruta + "/" + Vehiculo.P_Archivo;
                        AFU_Archivo.SaveAs(Archivo);
                    }
                    Configuracion_Formulario(true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Actualización de Vehículos", "alert('Actualización de Vehículo Exitosa');", true);
                    Vehiculo = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
                    Vehiculo.P_Vehiculo_ID = Hdf_Vehiculo_ID.Value.Trim();
                    Vehiculo = Vehiculo.Consultar_Detalles_Vehiculo();
                    Mostrar_Detalles_Vehiculo(Vehiculo);
                }
            }
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Cancela la operación que esta en proceso (Alta o Actualizar) o Sale
    ///             del Formulario.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 07/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText.Equals("Salir"))
        {
            Session["Dt_Resguardantes"] = null;
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Configuracion_Formulario(true);
            Tab_Contenedor_Pestagnas.TabIndex = 0;
            Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
            Vehiculo.P_Vehiculo_ID = Hdf_Vehiculo_ID.Value.Trim();
            Vehiculo = Vehiculo.Consultar_Detalles_Vehiculo();
            Mostrar_Detalles_Vehiculo(Vehiculo);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Avanzada_Click
    ///DESCRIPCIÓN: Carga el Modal Popup de Busqueda Avanzada.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 13/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Avanzada_Click(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Pnl_Busqueda_Vehiculo.Visible = true;
        MPE_Busqueda_Vehiculo.Show();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
    ///DESCRIPCIÓN: Carga el Modal Popup de Busqueda Directa.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 14/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Txt_Busqueda.Text.Trim().Length > 0)
            {
                Limpiar_Generales();
                String Clave_Inventario = Txt_Busqueda.Text.Trim();
                Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
                Vehiculo.P_Numero_Inventario = Convert.ToInt32(Clave_Inventario);
                Vehiculo.P_Buscar_Numero_Inventario = true;
                Vehiculo = Vehiculo.Consultar_Detalles_Vehiculo();
                if (Vehiculo.P_Vehiculo_ID != null && Vehiculo.P_Vehiculo_ID.Trim().Length > 0) {
                    Mostrar_Detalles_Vehiculo(Vehiculo);
                } else {
                    Lbl_Ecabezado_Mensaje.Text = HttpUtility.HtmlDecode("No se encontro un vehiculo con el Número de Inventario '" + Txt_Busqueda.Text.Trim() + "'.");
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            } else  {
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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Ver_Archivo_Click
    ///DESCRIPCIÓN: Limpia los componentes del MPE de Cancelación de Vacuna
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 16/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Ver_Archivo_Click(object sender, ImageClickEventArgs e)  {
        try {
            ImageButton Boton = (ImageButton)sender;
            String Archivo_Bien_ID = Boton.CommandArgument;
            for (Int32 Contador = 0; Contador < Grid_Archivos.Rows.Count; Contador++) {
                if (Grid_Archivos.Rows[Contador].Cells[0].Text.Trim().Equals(Archivo_Bien_ID))  {
                    String Archivo = "../../" + Ope_Pat_Archivos_Bienes.Campo_Ruta_Fisica_Archivos + "/VEHICULOS/" + Hdf_Vehiculo_ID.Value + "/" + Grid_Archivos.Rows[Contador].Cells[1].Text.Trim();
                    if (File.Exists(Server.MapPath(Archivo))) {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Archivo_Archivos", "window.open('" + Archivo + "','Window_Archivo','left=0,top=0')", true);
                        break;
                    }  else  {
                        Lbl_Ecabezado_Mensaje.Text = "El Archivo no esta disponible o fue eliminado";
                        Lbl_Mensaje_Error.Text = "";
                        Div_Contenedor_Msj_Error.Visible = true;
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Reporte_Click
    ///DESCRIPCIÓN: Genera el reporte simple.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 04/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Generar_Reporte_Click(object sender, ImageClickEventArgs e) {
        try {
            if (Btn_Modificar.AlternateText.Equals("Modificar")) {
                if (Hdf_Vehiculo_ID.Value.Trim().Length > 0) {
                    Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
                    Vehiculo.P_Vehiculo_ID = Hdf_Vehiculo_ID.Value;
                    Vehiculo = Vehiculo.Consultar_Detalles_Vehiculo();
                    Llenar_DataSet_Resguardos_Vehiculos(Vehiculo);
                } else {
                    Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                    Lbl_Mensaje_Error.Text = "Seleccionar el Vehículo a Generar el Reporte.";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Iniciar_Generacion_Reporte_Completo
    ///DESCRIPCIÓN: Genera el reporte completo.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 04/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Iniciar_Generacion_Reporte_Completo() { 
        try {
            if (Btn_Modificar.AlternateText.Equals("Modificar")) {
                if (Hdf_Vehiculo_ID.Value.Trim().Length > 0) {
                    Cls_Rpt_Pat_Completo_Vehiculos_Negocio Reporte_Negocio = new Cls_Rpt_Pat_Completo_Vehiculos_Negocio();
                    Reporte_Negocio.P_Vehiculo_ID = Hdf_Vehiculo_ID.Value;
                    DataTable Dt_Datos_Generales = Reporte_Negocio.Obtener_Datos_Generales();
                    DataTable Dt_Datos_Adquisicion = Reporte_Negocio.Obtener_Datos_Adquisicion();
                    DataTable Dt_Datos_Estado = Reporte_Negocio.Obtener_Datos_Estado_Detalles_Vehiculo();
                    DataTable Dt_Datos_Firmas = Obtener_Datos_Firmas_Reporte();

                    Dt_Datos_Generales.TableName = "DATOS_GENERALES";
                    Dt_Datos_Adquisicion.TableName = "DATOS_ADQUISICION";
                    Dt_Datos_Estado.TableName = "DATOS_DETALLES";
                    Dt_Datos_Firmas.TableName = "DT_FIRMAS";

                    DataSet Ds_Consulta = new DataSet();
                    Ds_Consulta.Tables.Add(Dt_Datos_Generales.Copy());
                    Ds_Consulta.Tables.Add(Dt_Datos_Adquisicion.Copy());
                    Ds_Consulta.Tables.Add(Dt_Datos_Estado.Copy());
                    Ds_Consulta.Tables.Add(Dt_Datos_Firmas.Copy());

                    Ds_Pat_Completo_Vehiculos Ds_Reporte = new Ds_Pat_Completo_Vehiculos();
                    Generar_Reporte_Completo(Ds_Consulta, Ds_Reporte, "Rpt_Pat_Completo_Vehiculos.rpt");

                } else {
                    Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                    Lbl_Mensaje_Error.Text = "Seleccionar el Vehículo a Generar el Reporte.";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Reporte_Completo_Click
    ///DESCRIPCIÓN: Genera el reporte completo.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 11/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Generar_Reporte_Completo_Click(object sender, ImageClickEventArgs e) {
        if (Hdf_Vehiculo_ID.Value.Trim().Length > 0) {
           if (Validar_Firmas()) {
                Iniciar_Generacion_Reporte_Completo();
            }
        } else {
            Lbl_Ecabezado_Mensaje.Text = "Verificar.";
            Lbl_Mensaje_Error.Text = "Se tiene que tener seleccionado el Vehiculo";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Obtener_Datos_Firmas_Reporte
    ///DESCRIPCIÓN: Obtiene los datos de las firmas del reporte de Vehiculos completo.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 10/Octubre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private DataTable Obtener_Datos_Firmas_Reporte() {
        DataTable Dt_Firmas = new DataTable("DT_FIRMAS");
        Dt_Firmas.Columns.Add("OPERADOR", Type.GetType("System.String"));
        Dt_Firmas.Columns.Add("FUNCIONARIO", Type.GetType("System.String"));
        Dt_Firmas.Columns.Add("AUTORIZO", Type.GetType("System.String"));
        DataRow Fila_Firmas = Dt_Firmas.NewRow();
        Fila_Firmas["OPERADOR"] = Txt_Resguardo_Completo_Operador.Text.Trim();
        Fila_Firmas["FUNCIONARIO"] = Txt_Resguardo_Completo_Funcionario_Recibe.Text.Trim();
        Fila_Firmas["AUTORIZO"] = Txt_Resguardo_Completo_Autorizo.Text.Trim();
        Dt_Firmas.Rows.Add(Fila_Firmas);
        return Dt_Firmas;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Dependencias_SelectedIndexChanged
    ///DESCRIPCIÓN: Maneja el evento de cambio de Selección del Combo de Dependencias
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 03/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Dependencias_SelectedIndexChanged(object sender, EventArgs e)
    {
        String Dependencia_ID = Cmb_Dependencias.SelectedItem.Value;
        Consultar_Empleados(Dependencia_ID);
    }

    #region Modal Busqueda

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Busqueda_Resguardantes_Dependencias_SelectedIndexChanged
    ///DESCRIPCIÓN: Maneja el evento de cambio de Selección del Combo de Dependencias
    ///             del Modal de Busqueda (Parte de Resguardantes).
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 06/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Busqueda_Resguardantes_Dependencias_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Cmb_Busqueda_Resguardantes_Dependencias.SelectedIndex > 0)
            {
                Cls_Ope_Pat_Com_Vehiculos_Negocio Combo = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
                Combo.P_Tipo_DataTable = "EMPLEADOS";
                Combo.P_Dependencia_ID = Cmb_Busqueda_Resguardantes_Dependencias.SelectedItem.Value.Trim();
                DataTable Tabla = Combo.Consultar_DataTable();
                Llenar_Combo_Empleados_Busqueda(Tabla);
            }
            else
            {
                DataTable Tabla = new DataTable();
                Tabla.Columns.Add("EMPLEADO_ID", Type.GetType("System.String"));
                Tabla.Columns.Add("NOMBRE", Type.GetType("System.String"));
                Llenar_Combo_Empleados_Busqueda(Tabla);
            }
            MPE_Busqueda_Vehiculo.Show();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Limpiar_Filtros_Buscar_Datos_Click
    ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Limpieza de los filtros
    ///             para la busqueda por parte de los Datos Generales.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 06/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************    
    protected void Btn_Limpiar_Filtros_Buscar_Datos_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Txt_Busqueda_Numero_Inventario.Text = "";
            Txt_Busqueda_Numero_Economico.Text = "";
            Txt_Modelo_Busqueda.Text = "";
            Cmb_Busqueda_Marca.SelectedIndex = 0;
            Cmb_Busqueda_Tipo_Vehiculo.SelectedIndex = 0;
            Cmb_Busqueda_Tipo_Combustible.SelectedIndex = 0;
            Txt_Busqueda_Anio_Fabricacion.Text = "";
            Cmb_Busqueda_Color.SelectedIndex = 0;
            Cmb_Busqueda_Zonas.SelectedIndex = 0;
            Cmb_Busqueda_Estatus.SelectedIndex = 0;
            Cmb_Busqueda_Dependencias.SelectedIndex = 0;
            MPE_Busqueda_Vehiculo.Show();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Datos_Click
    ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Busqueda de los
    ///             Datos Generales.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 06/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************    
    protected void Btn_Buscar_Datos_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Session["FILTRO_BUSQUEDA"] = "DATOS_GENERALES";
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Limpiar_Filtros_Buscar_Resguardante_Click
    ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Limpieza de los filtros
    ///             para la busqueda por parte de los Listados.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 06/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Limpiar_Filtros_Buscar_Resguardante_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Txt_Busqueda_RFC_Resguardante.Text = "";
            Cmb_Busqueda_Nombre_Resguardante.SelectedIndex = 0;
            Cmb_Busqueda_Resguardantes_Dependencias.SelectedIndex = 0;
            DataTable Tabla = new DataTable();
            Tabla.Columns.Add("EMPLEADO_ID", Type.GetType("System.String"));
            Tabla.Columns.Add("NOMBRE", Type.GetType("System.String"));
            Llenar_Combo_Empleados_Busqueda(Tabla);
            MPE_Busqueda_Vehiculo.Show();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Resguardante_Click
    ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Busqueda de los
    ///             Reguardante
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 06/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************    
    protected void Btn_Buscar_Resguardante_Click(object sender, ImageClickEventArgs e)
    {
        try {
            Session["FILTRO_BUSQUEDA"] = "RESGUARDANTES";
            Llenar_Grid_Listado_Vehiculos(0);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    #endregion

    #region Resguardos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Resguardante_Click
    ///DESCRIPCIÓN: Agrega una nuevo Empleado Resguardante para este Vehiculo.
    ///             (No aun en la Base de Datos)
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 07/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Agregar_Resguardante_Click(object sender, ImageClickEventArgs e) {
        if (Validar_Componentes_Resguardos()) {
            DataTable Tabla = (DataTable)Grid_Resguardantes.DataSource;
            if (Tabla == null) {
                if (Session["Dt_Resguardantes"] == null) {
                    Tabla = new DataTable("Resguardos");
                    Tabla.Columns.Add("BIEN_RESGUARDO_ID", Type.GetType("System.String"));
                    Tabla.Columns.Add("EMPLEADO_ID", Type.GetType("System.String"));
                    Tabla.Columns.Add("NO_EMPLEADO", Type.GetType("System.String"));
                    Tabla.Columns.Add("NOMBRE_EMPLEADO", Type.GetType("System.String"));
                    Tabla.Columns.Add("COMENTARIOS", Type.GetType("System.String"));
                } else {
                    Tabla = (DataTable)Session["Dt_Resguardantes"];
                }
            }
            if (!Buscar_Clave_DataTable(Cmb_Empleados.SelectedItem.Value, Tabla, 1)) {
                Cls_Cat_Empleados_Negocios Empleados_Negocio = new Cls_Cat_Empleados_Negocios();
                Empleados_Negocio.P_Empleado_ID = Cmb_Empleados.SelectedItem.Value;
                DataTable Dt_Empleado = Empleados_Negocio.Consulta_Datos_Empleado();
                if (Dt_Empleado != null && Dt_Empleado.Rows.Count > 0) {
                    DataRow Fila = Tabla.NewRow();
                    Fila["BIEN_RESGUARDO_ID"] = 0;
                    Fila["EMPLEADO_ID"] = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString().Trim();
                    Fila["NO_EMPLEADO"] = Dt_Empleado.Rows[0][Cat_Empleados.Campo_No_Empleado].ToString().Trim();
                    Fila["NOMBRE_EMPLEADO"] = HttpUtility.HtmlDecode(Cmb_Empleados.SelectedItem.Text);
                    Fila["COMENTARIOS"] = HttpUtility.HtmlDecode(Txt_Cometarios.Text.Trim());
                    Tabla.Rows.Add(Fila);
                }
                Llenar_Grid_Resguardantes(Grid_Resguardantes.PageIndex, Tabla);
                Grid_Resguardantes.SelectedIndex = (-1);
                Cmb_Empleados.SelectedIndex = 0;
                Txt_Cometarios.Text = "";
            } else {
                Lbl_Ecabezado_Mensaje.Text = "El Empleado ya esta Agregado.";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Quitar_Resguardante_Click
    ///DESCRIPCIÓN: Quita un Empleado resguardante para este Vehiculo (No en la Base de datos
    ///             aun).
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 07/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Quitar_Resguardante_Click(object sender, ImageClickEventArgs e)
    {
        if (Grid_Resguardantes.Rows.Count > 0 && Grid_Resguardantes.SelectedIndex > (-1))
        {
            Int32 Registro = ((Grid_Resguardantes.PageIndex) * Grid_Resguardantes.PageSize) + (Grid_Resguardantes.SelectedIndex);
            if (Session["Dt_Resguardantes"] != null)
            {
                DataTable Tabla = (DataTable)Session["Dt_Resguardantes"];
                Tabla.Rows.RemoveAt(Registro);
                Session["Dt_Resguardantes"] = Tabla;
                Grid_Resguardantes.SelectedIndex = (-1);
                Llenar_Grid_Resguardantes(Grid_Resguardantes.PageIndex, Tabla);
            }
        }
        else
        {
            Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Quitar.";
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Ver_Informacion_Resguardo_Click
    ///DESCRIPCIÓN: Manda Visualizar los Comentarios del Resguardo.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 30/Septiembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************
    protected void Btn_Ver_Informacion_Resguardo_Click(object sender, ImageClickEventArgs e) {
        ImageButton Btn_Ver_Informacion_Resguardo = (ImageButton)sender;
        String Comentarios = "Sin Comentarios";
        if (Btn_Ver_Informacion_Resguardo.CommandArgument.Trim().Length > 0) { Comentarios = "Comentarios: " + Btn_Ver_Informacion_Resguardo.CommandArgument.Trim(); }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "GACO", "alert('" + Comentarios + "');", true);
    }

    #endregion

    #region "Busqueda Resguardantes"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Avanzada_Resguardante_Click
        ///DESCRIPCIÓN: Lanza la Busqueda Avanzada para el Resguardante.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 24/Octubre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        protected void Btn_Busqueda_Avanzada_Resguardante_Click(object sender, ImageClickEventArgs e) {
            try {
                Hdf_Tipo_Busqueda.Value = "";
                MPE_Resguardante.Show();
            }catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Resguardo_Completo_Operador_Click
        ///DESCRIPCIÓN: Lanza la Busqueda Avanzada para el Resguardante.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 24/Octubre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        protected void Btn_Resguardo_Completo_Operador_Click(object sender, ImageClickEventArgs e) {
            try {
                Hdf_Tipo_Busqueda.Value = "OPERADOR";
                MPE_Resguardante.Show();
            }catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Resguardo_Completo_Operador_Click
        ///DESCRIPCIÓN: Lanza la Busqueda Avanzada para el Resguardante.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 24/Octubre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        protected void Btn_Resguardo_Completo_Funcionario_Recibe_Click(object sender, ImageClickEventArgs e) {
            try {
                Hdf_Tipo_Busqueda.Value = "FUNCIONARIO_RECIBE";
                MPE_Resguardante.Show();
            }catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Resguardo_Completo_Autorizo_Click
        ///DESCRIPCIÓN: Lanza la Busqueda Avanzada para el Resguardante.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 24/Octubre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        protected void Btn_Resguardo_Completo_Autorizo_Click(object sender, ImageClickEventArgs e) {
            try {
                Hdf_Tipo_Busqueda.Value = "AUTORIZO";
                MPE_Resguardante.Show();
            }catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Empleados_Click
        ///DESCRIPCIÓN: Ejecuta la Busqueda Avanzada para el Resguardante.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 24/Octubre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        protected void Btn_Busqueda_Empleados_Click(object sender, EventArgs e) {
            try {
                Grid_Busqueda_Empleados_Resguardo.PageIndex = 0;
                Llenar_Grid_Busqueda_Empleados_Resguardo();
                MPE_Resguardante.Show();
            }  catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

    #endregion

    #endregion

}