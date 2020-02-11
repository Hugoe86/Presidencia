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
using Presidencia.Empleados.Negocios;
using Presidencia.Dependencias.Negocios;
using Presidencia.Constantes;
using Presidencia.Control_Patrimonial_Operacion_Bienes_Muebles.Negocio;
using Presidencia.Control_Patrimonial_Reporte_Listado_Bienes.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Procedencias.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using System.Text;
using System.Reflection;
using CarlosAg.ExcelXmlWriter;
using Presidencia.Control_Patrimonial_Catalogo_Clasificaciones.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Clases_Activos.Negocio;

public partial class paginas_Compras_Frm_Rpt_Reporte_Activos_Fijos : System.Web.UI.Page
{
    #region "Page Load"

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Evento que se carga cuando la Pagina de Inicia.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 14/Diciembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
        Lbl_Ecabezado_Mensaje.Text = "";
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
        if (!IsPostBack)
        {
            Llenar_Combos();
            Txt_Fecha_Inicial.Text = "";
            Txt_Fecha_Final.Text = "";
        }
    }

    #endregion

    #region "Metodos"

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combos
    ///DESCRIPCIÓN: Llena los combos del Formulario.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 14/Diciembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Llenar_Combos()
    {
        try
        {
            Cls_Ope_Pat_Com_Bienes_Muebles_Negocio BM_Negocio = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
            DataTable Dt_Temporal = null;

            //Se llena combo de Dependencias
            BM_Negocio.P_Tipo_DataTable = "DEPENDENCIAS";
            Dt_Temporal = BM_Negocio.Consultar_DataTable();
            Cmb_Dependencia.DataSource = Dt_Temporal;
            Cmb_Dependencia.DataTextField = "NOMBRE";
            Cmb_Dependencia.DataValueField = "DEPENDENCIA_ID";
            Cmb_Dependencia.DataBind();
            Cmb_Dependencia.Items.Insert(0, new ListItem("< TODAS >", ""));


            Cls_Cat_Pat_Com_Clases_Activo_Negocio CA_Negocio = new Cls_Cat_Pat_Com_Clases_Activo_Negocio();
            CA_Negocio.P_Estatus = "VIGENTE";
            CA_Negocio.P_Tipo_DataTable = "CLASES_ACTIVOS";
            Cmb_Clase_Activo.DataSource = CA_Negocio.Consultar_DataTable();
            Cmb_Clase_Activo.DataValueField = "CLASE_ACTIVO_ID";
            Cmb_Clase_Activo.DataTextField = "CLAVE_DESCRIPCION";
            Cmb_Clase_Activo.DataBind();
            Cmb_Clase_Activo.Items.Insert(0, new ListItem("< TODAS >", ""));

            Cls_Cat_Pat_Com_Clasificaciones_Negocio Clasificaciones_Negocio = new Cls_Cat_Pat_Com_Clasificaciones_Negocio();
            Clasificaciones_Negocio.P_Estatus = "VIGENTE";
            Clasificaciones_Negocio.P_Tipo_DataTable = "CLASIFICACIONES";
            Cmb_Tipo_Activo.DataSource = Clasificaciones_Negocio.Consultar_DataTable();
            Cmb_Tipo_Activo.DataValueField = "CLASIFICACION_ID";
            Cmb_Tipo_Activo.DataTextField = "CLAVE_DESCRIPCION";
            Cmb_Tipo_Activo.DataBind();
            Cmb_Tipo_Activo.Items.Insert(0, new ListItem("< TODAS >", ""));

        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = "Verificar al Llenar los Combos.";
            Lbl_Mensaje_Error.Text = "'" + Ex.Message + "'";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN: Genera el Reporte
    ///PROPIEDADES:     
    ///CREO:                 
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 07/Febrero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Generar_Reporte(String Tipo) {
        try {
            Cls_Rpt_Pat_Listado_Bienes_Negocio Negocio = new Cls_Rpt_Pat_Listado_Bienes_Negocio();
            if (Cmb_Tipo_Bien.SelectedIndex > 0) { Negocio.P_Tipo_Bien = Cmb_Tipo_Bien.SelectedItem.Value.Trim(); } else { Negocio.P_Tipo_Bien = "TODOS"; }
            if (Cmb_Dependencia.SelectedIndex > 0) { Negocio.P_Dependencia_ID = Cmb_Dependencia.SelectedItem.Value.Trim(); }
            if (Cmb_Estatus.SelectedIndex > 0) { Negocio.P_Estatus = Cmb_Estatus.SelectedItem.Value.Trim(); }
            if (Txt_Fecha_Inicial.Text.Trim().Length > 0 && (!Txt_Fecha_Inicial.Text.Trim().Equals("__/___/____")))  {
                Negocio.P_Fecha_Adquisicion_Inicial = Convert.ToDateTime(Txt_Fecha_Inicial.Text.Trim());
                Negocio.P_Tomar_Fecha_Inicial = true;
            }
            if (Txt_Fecha_Final.Text.Trim().Length > 0 && (!Txt_Fecha_Final.Text.Trim().Equals("__/___/____"))) {
                Negocio.P_Fecha_Adquisicion_Final = Convert.ToDateTime(Txt_Fecha_Final.Text.Trim()); 
                Negocio.P_Tomar_Fecha_Final = true;
            }
            if (Txt_Costo.Text.Trim().Length > 0) {
                Negocio.P_Valor_Minimo = Convert.ToDouble(Txt_Costo.Text);
            }
            if (Cmb_Clase_Activo.SelectedIndex > 0) { Negocio.P_Clase_Activo_ID = Cmb_Clase_Activo.SelectedItem.Value.Trim(); }
            if (Cmb_Tipo_Activo.SelectedIndex > 0) { Negocio.P_Clasificacion_ID = Cmb_Tipo_Activo.SelectedItem.Value.Trim(); }
            DataTable Dt_Datos = new DataTable();
            Cargar_Estructura_DataTable_Poliza(ref Dt_Datos);
            if (Cmb_Tipo_Bien.SelectedIndex == 0) {
                DataTable Dt_Datos_Tmp = Negocio.Obtener_Listado_Activos_Fijos_Animales();
                Cargar_Registros(ref Dt_Datos, Dt_Datos_Tmp);
                Dt_Datos_Tmp = Negocio.Obtener_Listado_Activos_Fijos_Bienes_Muebles();
                Cargar_Registros(ref Dt_Datos, Dt_Datos_Tmp);
                Dt_Datos_Tmp = Negocio.Obtener_Listado_Activos_Fijos_Vehiculos();
                Cargar_Registros(ref Dt_Datos, Dt_Datos_Tmp);
            } else if (Cmb_Tipo_Bien.SelectedItem.Value.Trim().Equals("ANIMAL")) {
                DataTable Dt_Datos_Tmp = Negocio.Obtener_Listado_Activos_Fijos_Animales();
                Cargar_Registros(ref Dt_Datos, Dt_Datos_Tmp);
            } else if (Cmb_Tipo_Bien.SelectedItem.Value.Trim().Equals("BIEN_MUEBLE")) {
                DataTable Dt_Datos_Tmp = Negocio.Obtener_Listado_Activos_Fijos_Bienes_Muebles();
                Cargar_Registros(ref Dt_Datos, Dt_Datos_Tmp);
            } else if (Cmb_Tipo_Bien.SelectedItem.Value.Trim().Equals("VEHICULO")) {
                DataTable Dt_Datos_Tmp = Negocio.Obtener_Listado_Activos_Fijos_Vehiculos();
                Cargar_Registros(ref Dt_Datos, Dt_Datos_Tmp);
            }

            DataTable Dt_Cabecera = new DataTable();
            Cargar_Estructura_DataTable_Poliza(ref Dt_Cabecera);
            Cargar_DataTable_Cabecera(ref Dt_Cabecera);
            DataTable Dt_Detalles_Cabecera = new DataTable();
            Cargar_Estructura_DataTable_Poliza(ref Dt_Detalles_Cabecera);
            Cargar_DataTable_Detalles_Cabecera(ref Dt_Detalles_Cabecera);
            if (Tipo.Equals("DESCARGAR_EXCEL")) {
                Pasar_Datos_A_Excel(Dt_Cabecera, Dt_Detalles_Cabecera, Dt_Datos);
            }
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = "Verificar.";
            Lbl_Mensaje_Error.Text = "'" + Ex.Message + "'";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Registros
    ///DESCRIPCIÓN: Carga los registros del reporte.
    ///PARAMETROS: 
    ///CREO: Francisco A. Gallardo Castañeda.
    ///FECHA_CREO: 07/Febrero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Registros(ref DataTable Tabla, DataTable Datos) {
        foreach (DataRow Fila in Datos.Rows) {
            Tabla.ImportRow(Fila);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_DataTable_Cabecera
    ///DESCRIPCIÓN: Carga los datos de la cabecera del reporte
    ///PARAMETROS: 
    ///CREO: Francisco A. Gallardo Castañeda.
    ///FECHA_CREO: 07/Febrero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_DataTable_Cabecera(ref DataTable Dt_Datos) {
        DataRow Fila = Dt_Datos.NewRow();

        Fila["CAMPO"] = "Campo";
        Fila["CLASE_ACTIVO"] = "Clase de Activos";
        Fila["SOCIEDAD"] = "Sociedad";
        Fila["NOMBRE_PRODUCTO_1"] = "Denominacion 1er Linea";
        Fila["NOMBRE_PRODUCTO_2"] = "Denominacion 2da Linea";
        Fila["NO_PRINCIPAL_ACTIVO_FIJO"] = "No Principal Activo Fijo (Texto)";
        Fila["NO_SERIE"] = "No de Serie";
        Fila["NO_INVENTARIO_ANTERIOR"] = "No De Inventario";
        Fila["FECHA_ULTIMO_INVENTARIO"] = "Ultimo Inventario";
        Fila["NOTA_INVENTARIO"] = "Nota de Inventario";
        Fila["CAPITALIZADO_EL"] = "CAPITALIZADO EL";
        Fila["DIVISION"] = "Division";
        Fila["CENTRO_COSTE"] = "Centro de Coste";
        Fila["FONDO"] = "Fondo";
        Fila["AREA_FUNCIONAL"] = "Area Funcional";
        Fila["TIPO_ACTIVO"] = "Tipo Activo";
        Fila["CARACTERISTICAS"] = "Caracteristicas";
        Fila["MUNICIPIO"] = "Municipio";
        Fila["DESTINO_INVERSION"] = "Destino de Inversion";
        Fila["ACCION_LEGAL"] = "Acción Legal";
        Fila["CRTI_CLASIF_5"] = "Crit Clasif 5";
        Fila["CLAVE_PROVEEDOR"] = "Clave Proveedor";
        Fila["PROVEEDOR"] = "PROVEEDOR";
        Fila["FABRICANTE"] = "Fabricante";
        Fila["PAIS_ORIGEN"] = "Pais de origen";
        Fila["DENOMINACION_TIPO"] = "Denominacion de Tipo";
        Fila["FECHA_ALTA_INVENTARIO"] = "ALTA el ";
        Fila["VALOR_ORIGINAL"] = "Valor Original";
        Fila["CLAVE_AGRUPAMIENTO"] = "Clave de Agrupamiento";
        Fila["INDICADOR_PROPIEDAD"] = "Indicador de Propiedad";
        Fila["DELEGACION_HACIENDA"] = "Delegación Hacienda";
        Fila["NIF_VALOR_UNIT"] = "NIF Valor Unit";
        Fila["CARTILLA_DEL"] = "Cartilla del";
        Fila["MUNICIPIO_BI"] = "Municipio";
        Fila["CLASE"] = "Clase";
        Fila["SOCIEDAD_GL"] = "Sociedad GL";
        Fila["NO_POLIZA"] = "No de Poliza";
        Fila["COMENTARIO"] = "Comentario";
        Fila["INICIO"] = "Inicio";
        Fila["PRIMA"] = "Prima";
        Fila["VALOR_BASE"] = "Valor Base";
        Fila["VALOR_MANUAL"] = "Valor Manual";
        Fila["ACTIVO_FIJO_ORIGINAL"] = "Activo Fijo Original";
        Fila["SUBNUMERO_ACTIVO_FIJO_ORIGINAL"] = "Subnúmero de Activo Fijo Original";
        Fila["ANIO_ADQUISICION_ORIGINAL"] = "Año de Adquisición orig.";
        Fila["MOTIVO_MAL_M"] = "Motivo val.m";
        Fila["VAL_PATRIM_MA"] = "Val.patrim.ma";
        Fila["REG_PROPIEDAD_D"] = "Reg.propiedad d";
        Fila["INSCRIPCION"] = "Inscripción ";
        Fila["TOMO_REGISTRO_PROPIEDAD"] = "Tomo del registro de la propiedad";
        Fila["HOJA"] = "Hoja";
        Fila["NO_ACT"] = "/nº act";
        Fila["CES_PROPIEDA"] = "Ces.propieda";
        Fila["CARTILLA_PARCELARIA"] = "Cartilla parcelaria";
        Fila["SUPERFICIE"] = "Superficie";
        Fila["UNI_SUPERFICIE"] = "Uni Superficie";
        Fila["COMENTARIO_ARRENDAMIENTO"] = "Comentario (arrendamiento)";

        Dt_Datos.Rows.Add(Fila);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_DataTable_Detalles_Cabecera
    ///DESCRIPCIÓN: Carga los detalles de la cabecera del reporte
    ///PARAMETROS: 
    ///CREO: Francisco A. Gallardo Castañeda.
    ///FECHA_CREO: 14/Dic/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_DataTable_Detalles_Cabecera(ref DataTable Dt_Datos) {
        
        //Carga los Cadigos de las Cabeceras
        DataRow Fila_Codigos = Dt_Datos.NewRow();

        Fila_Codigos["CLASE_ACTIVO"] = "ANLA-ANLKL";
        Fila_Codigos["SOCIEDAD"] = "ANLA-BUKRS";
        Fila_Codigos["NOMBRE_PRODUCTO_1"] = "ANLA-TXT50";
        Fila_Codigos["NOMBRE_PRODUCTO_2"] = "ANLA-TXA50";
        Fila_Codigos["NO_PRINCIPAL_ACTIVO_FIJO"] = "ANLH-ANLHTXT";
        Fila_Codigos["NO_SERIE"] = "ANLA-SERNR";
        Fila_Codigos["NO_INVENTARIO_ANTERIOR"] = "ANLA-INVNR";
        Fila_Codigos["FECHA_ULTIMO_INVENTARIO"] = "ANLA-IVDAT";
        Fila_Codigos["NOTA_INVENTARIO"] = "ANLA-INVZU";
        Fila_Codigos["CAPITALIZADO_EL"] = "ANLA-AKTIV";
        Fila_Codigos["DIVISION"] = "ANLZ-GSBER";
        Fila_Codigos["CENTRO_COSTE"] = "ANLZ-KOSTL";
        Fila_Codigos["FONDO"] = "ANLZ-GEBER";
        Fila_Codigos["AREA_FUNCIONAL"] = "ANLZ-FKBER";
        Fila_Codigos["TIPO_ACTIVO"] = "ANLA-ORD41";
        Fila_Codigos["CARACTERISTICAS"] = "ANLA-ORD42";
        Fila_Codigos["MUNICIPIO"] = "ANLA-ORD43";
        Fila_Codigos["DESTINO_INVERSION"] = "ANLA-IZWEK";
        Fila_Codigos["ACCION_LEGAL"] = "ANLA-ORD44";
        Fila_Codigos["CRTI_CLASIF_5"] = "ANLA-GDLGRP";
        Fila_Codigos["CLAVE_PROVEEDOR"] = "ANLA-LIFNR";
        Fila_Codigos["PROVEEDOR"] = "ANLA-LIEFE";
        Fila_Codigos["FABRICANTE"] = "ANLA-HERST";
        Fila_Codigos["PAIS_ORIGEN"] = "ANLA-LAND1";
        Fila_Codigos["DENOMINACION_TIPO"] = "ANLA-TYPBZ";
        Fila_Codigos["FECHA_ALTA_INVENTARIO"] = "ANLA-AIBDT";
        Fila_Codigos["VALOR_ORIGINAL"] = "ANLA-URWRT";
        Fila_Codigos["CLAVE_AGRUPAMIENTO"] = "ANLA-VMGLI";
        Fila_Codigos["INDICADOR_PROPIEDAD"] = "ANLA-EIGKZ";
        Fila_Codigos["DELEGACION_HACIENDA"] = "ANLA-FIAMT";
        Fila_Codigos["NIF_VALOR_UNIT"] = "ANLA-EHWNR";
        Fila_Codigos["CARTILLA_DEL"] = "ANLA-EHWZU";
        Fila_Codigos["MUNICIPIO_BI"] = "ANLA-STADT";
        Fila_Codigos["CLASE"] = "ANLV-VSART";
        Fila_Codigos["SOCIEDAD_GL"] = "ANLV-VSGES";
        Fila_Codigos["NO_POLIZA"] = "ANLV-VSSTX";
        Fila_Codigos["COMENTARIO"] = "ANLV-VSZTX";
        Fila_Codigos["INICIO"] = "ANLV-VRSBG";
        Fila_Codigos["PRIMA"] = "ANLV-VSTAR";
        Fila_Codigos["VALOR_BASE"] = "ANLV-VRSBA";
        Fila_Codigos["VALOR_MANUAL"] = "ANLV-VRSMA";
        Fila_Codigos["ACTIVO_FIJO_ORIGINAL"] = "ANLA-AIBN1";
        Fila_Codigos["SUBNUMERO_ACTIVO_FIJO_ORIGINAL"] = "ANLA-AIBN2";
        Fila_Codigos["ANIO_ADQUISICION_ORIGINAL"] = "ANLA-URJHR";
        Fila_Codigos["MOTIVO_MAL_M"] = "ANLA-GRUND";
        Fila_Codigos["VAL_PATRIM_MA"] = "ANLA-WRTMA";
        Fila_Codigos["REG_PROPIEDAD_D"] = "ANLA-GRUVO";
        Fila_Codigos["INSCRIPCION"] = "ANLA-GREIN";
        Fila_Codigos["TOMO_REGISTRO_PROPIEDAD"] = "ANLA-GRBND";
        Fila_Codigos["HOJA"] = "ANLA-GRBLT";
        Fila_Codigos["NO_ACT"] = "ANLA-GRLFD";
        Fila_Codigos["CES_PROPIEDA"] = "ANLA-AUFLA";
        Fila_Codigos["CARTILLA_PARCELARIA"] = "ANLA-FLURK";
        Fila_Codigos["VACIO"] = "ANLA-FLURN";
        Fila_Codigos["SUPERFICIE"] = "ANLA-GRUFL";
        Fila_Codigos["UNI_SUPERFICIE"] = "ANLA-FEINS";
        Fila_Codigos["COMENTARIO_ARRENDAMIENTO"] = "ANLA-LETXT";

        Dt_Datos.Rows.Add(Fila_Codigos);

        //Carga los Cadigos de las Cabeceras
        DataRow Fila = Dt_Datos.NewRow();

        Fila["CAMPO"] = "Longitud";
        Fila["CLASE_ACTIVO"] = "8";
        Fila["SOCIEDAD"] = "4";
        Fila["NOMBRE_PRODUCTO_1"] = "50";
        Fila["NOMBRE_PRODUCTO_2"] = "50";
        Fila["NO_PRINCIPAL_ACTIVO_FIJO"] = "50";
        Fila["NO_SERIE"] = "18";
        Fila["NO_INVENTARIO_ANTERIOR"] = "25";
        Fila["FECHA_ULTIMO_INVENTARIO"] = "8";
        Fila["NOTA_INVENTARIO"] = "15";
        Fila["CAPITALIZADO_EL"] = "8";
        Fila["DIVISION"] = "4";
        Fila["CENTRO_COSTE"] = "10";
        Fila["FONDO"] = "10";
        Fila["AREA_FUNCIONAL"] = "4";
        Fila["TIPO_ACTIVO"] = "4";
        Fila["CARACTERISTICAS"] = "4";
        Fila["MUNICIPIO"] = "4";
        Fila["DESTINO_INVERSION"] = "2";
        Fila["ACCION_LEGAL"] = "4";
        Fila["CRTI_CLASIF_5"] = "8";
        Fila["CLAVE_PROVEEDOR"] = "10";
        Fila["PROVEEDOR"] = "30";
        Fila["FABRICANTE"] = "30";
        Fila["PAIS_ORIGEN"] = "3";
        Fila["DENOMINACION_TIPO"] = "15";
        Fila["FECHA_ALTA_INVENTARIO"] = "8";
        Fila["VALOR_ORIGINAL"] = "13";
        Fila["CLAVE_AGRUPAMIENTO"] = "4";
        Fila["INDICADOR_PROPIEDAD"] = "1";
        Fila["DELEGACION_HACIENDA"] = "25";
        Fila["NIF_VALOR_UNIT"] = "16";
        Fila["CARTILLA_DEL"] = "8";
        Fila["MUNICIPIO_BI"] = "25";
        Fila["CLASE"] = "2";
        Fila["SOCIEDAD_GL"] = "2";
        Fila["NO_POLIZA"] = "15";
        Fila["COMENTARIO"] = "50";
        Fila["INICIO"] = "8";
        Fila["PRIMA"] = "5";
        Fila["VALOR_BASE"] = "13";
        Fila["VALOR_MANUAL"] = "13";
        Fila["ACTIVO_FIJO_ORIGINAL"] = "8";
        Fila["SUBNUMERO_ACTIVO_FIJO_ORIGINAL"] = "4";
        Fila["ANIO_ADQUISICION_ORIGINAL"] = "4";
        Fila["MOTIVO_MAL_M"] = "3";
        Fila["VAL_PATRIM_MA"] = "13 (dec)";
        Fila["REG_PROPIEDAD_D"] = "8";
        Fila["INSCRIPCION"] = "8";
        Fila["TOMO_REGISTRO_PROPIEDAD"] = "5";
        Fila["HOJA"] = "5";
        Fila["NO_ACT"] = "4";
        Fila["CES_PROPIEDA"] = "8";
        Fila["CARTILLA_PARCELARIA"] = "4";
        Fila["SUPERFICIE"] = "13 (3 dec)";
        Fila["UNI_SUPERFICIE"] = "3";
        Fila["COMENTARIO_ARRENDAMIENTO"] = "50";

        Dt_Datos.Rows.Add(Fila);
    }

    /// *************************************************************************************************************************
    /// Nombre: Pasar_DataTable_A_Excel
    /// 
    /// Descripción: Pasa DataTable a Excel.
    /// 
    /// Parámetros: Dt_Reporte.- DataTable que se pasara a excel.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 18/Octubre/2011.
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    public void Pasar_Datos_A_Excel(System.Data.DataTable Dt_Cabecera, System.Data.DataTable Dt_Detalles_Cabecera, System.Data.DataTable Dt_Datos) {
        String Ruta = "Activos_Fijos_" + (DateTime.Now).ToString() + ".xls";
        try
        {
            //Creamos el libro de Excel.
            CarlosAg.ExcelXmlWriter.Workbook Libro = new CarlosAg.ExcelXmlWriter.Workbook();

            Libro.Properties.Title = "Reporte de Activos Fijos para Control Patrimonial";
            Libro.Properties.Created = DateTime.Now;
            Libro.Properties.Author = "Control Patrimonial - " + Cls_Sessiones.Nombre_Empleado;

            //Creamos una hoja que tendrá el libro.
            CarlosAg.ExcelXmlWriter.Worksheet Hoja = Libro.Worksheets.Add("Registros");
            //Agregamos un renglón a la hoja de excel.
            CarlosAg.ExcelXmlWriter.WorksheetRow Renglon = Hoja.Table.Rows.Add();
            //Creamos el estilo titulo para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Titulo = Libro.Styles.Add("TitleStyle");
            //Creamos el estilo cabecera para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Cabecera = Libro.Styles.Add("HeaderStyle");
            //Creamos el estilo cabecera para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Detalle_Cabecera = Libro.Styles.Add("HeaderDetailStyle");
            //Creamos el estilo contenido para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Contenido = Libro.Styles.Add("BodyStyle");

            Estilo_Titulo.Font.FontName = "Arial";
            Estilo_Titulo.Font.Size = 16;
            Estilo_Titulo.Font.Bold = true;
            Estilo_Titulo.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Estilo_Titulo.Alignment.Vertical = StyleVerticalAlignment.Center;
            Estilo_Titulo.Font.Color = "#FFFFFF";
            Estilo_Titulo.Interior.Color = "#808080";
            Estilo_Titulo.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Titulo.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Titulo.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Titulo.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Titulo.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");
            Estilo_Titulo.Alignment.WrapText = true;

            Estilo_Cabecera.Font.FontName = "Arial";
            Estilo_Cabecera.Font.Size = 10;
            Estilo_Cabecera.Font.Bold = true;
            Estilo_Cabecera.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Estilo_Cabecera.Alignment.Vertical = StyleVerticalAlignment.Center;
            Estilo_Cabecera.Font.Color = "#000000";
            Estilo_Cabecera.Interior.Color = "#969696";
            Estilo_Cabecera.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Cabecera.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera.Alignment.WrapText = true;

            Estilo_Detalle_Cabecera.Font.FontName = "Arial";
            Estilo_Detalle_Cabecera.Font.Size = 10;
            Estilo_Detalle_Cabecera.Font.Bold = false;
            Estilo_Detalle_Cabecera.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Estilo_Detalle_Cabecera.Alignment.Vertical = StyleVerticalAlignment.Center;
            Estilo_Detalle_Cabecera.Font.Color = "#000000";
            Estilo_Detalle_Cabecera.Interior.Color = "#BFBFBF";
            Estilo_Detalle_Cabecera.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Detalle_Cabecera.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Detalle_Cabecera.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Detalle_Cabecera.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Detalle_Cabecera.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");
            Estilo_Detalle_Cabecera.Alignment.WrapText = true;

            Estilo_Contenido.Font.FontName = "Arial";
            Estilo_Contenido.Font.Size = 10;
            Estilo_Contenido.Font.Bold = false;
            Estilo_Contenido.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Estilo_Contenido.Font.Color = "#000000";
            Estilo_Contenido.Interior.Color = "White";
            Estilo_Contenido.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Contenido.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido.Alignment.WrapText = true;

            for (Int32 Contador = 0; Contador < 58; Contador++) {
                if (Contador == 3 || Contador == 4 || Contador == 5) {
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(400));
                } else if (Contador == 22 || Contador == 7|| Contador == 6) {
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(250));
                }  else { 
                    Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(90));
                }
            }

            //SE CARGA LA CABECERA PRINCIPAL DEL ARCHIVO
            WorksheetCell cell = Renglon.Cells.Add("Creación/Moficación de Activos Fijos");
            cell.MergeAcross = 57;            // Merge two cells together
            cell.StyleID = "TitleStyle";
            Renglon.Height = 27;
            Renglon = Hoja.Table.Rows.Add();
            
            //SE CARGA LA CABECERA CON LOS NOMBRES DE LAS COLUMNAS
            foreach (System.Data.DataRow FILA in Dt_Cabecera.Rows) {
                if (FILA is System.Data.DataRow) {
                    foreach (System.Data.DataColumn COLUMNA in Dt_Cabecera.Columns) {
                        if (COLUMNA is System.Data.DataColumn) {
                            Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(FILA[COLUMNA.ColumnName].ToString(), DataType.String, "HeaderStyle"));
                        }
                    }
                    Renglon.Height = 39;
                    Renglon.AutoFitHeight = true;
                    Renglon = Hoja.Table.Rows.Add();
                }
            }
            
            //SE CARGAN LOS DETALLES DE LAS CABECERAS
            foreach (System.Data.DataRow FILA in Dt_Detalles_Cabecera.Rows) {
                if (FILA is System.Data.DataRow) {
                    foreach (System.Data.DataColumn COLUMNA in Dt_Detalles_Cabecera.Columns) {
                        String estilo = "HeaderDetailStyle";
                        if (COLUMNA.ColumnName.Trim().Equals(Dt_Detalles_Cabecera.Columns[0].ColumnName.Trim())) {
                            estilo = "HeaderStyle";
                        }
                        if (COLUMNA is System.Data.DataColumn) {
                            Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(FILA[COLUMNA.ColumnName].ToString(), DataType.String, estilo));
                        }
                    }
                    Renglon.Height = 13;
                    Renglon.AutoFitHeight = true;
                    Renglon = Hoja.Table.Rows.Add();
                }
            }
            
            //SE CARGAN LOS DATOS...
            foreach (System.Data.DataRow FILA in Dt_Datos.Rows) {
                if (FILA is System.Data.DataRow) {
                    foreach (System.Data.DataColumn COLUMNA in Dt_Datos.Columns) {
                        String estilo = "BodyStyle";
                        if (COLUMNA.ColumnName.Trim().Equals(Dt_Detalles_Cabecera.Columns[0].ColumnName.Trim())) {
                            estilo = "HeaderStyle";
                        }
                        if (COLUMNA is System.Data.DataColumn) {
                            Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(FILA[COLUMNA.ColumnName].ToString(), DataType.String, estilo));
                        }
                    }
                    Renglon.Height = 20;
                    Renglon.AutoFitHeight = true;
                    Renglon = Hoja.Table.Rows.Add();
                }
            }
               
            //Abre el archivo de excel
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + Ruta);
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Libro.Save(Response.OutputStream);
            Response.End();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    /// *************************************************************************************************************************
    /// Nombre: Cargar_Estructura_DataTable_Poliza
    /// Descripción: Carga la Estructura para la poliza en un DataTable
    /// Parámetros: Dt_Parametro.- DataTable que se le cargara la estructura
    /// Usuario Creo: Francisco Antonio Gallardo Castañeda.
    /// Fecha Creó: 07/Febrero/2012.
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    public void Cargar_Estructura_DataTable_Poliza(ref DataTable Dt_Parametro) {
        if (Dt_Parametro != null)  {
            Dt_Parametro = new DataTable();
        }
        //BIENES MUEBLES
        Dt_Parametro.Columns.Add("CAMPO", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("CLASE_ACTIVO", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("SOCIEDAD", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("NOMBRE_PRODUCTO_1", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("NOMBRE_PRODUCTO_2", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("NO_PRINCIPAL_ACTIVO_FIJO", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("NO_SERIE", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("NO_INVENTARIO_ANTERIOR", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("FECHA_ULTIMO_INVENTARIO", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("NOTA_INVENTARIO", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("CAPITALIZADO_EL", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("DIVISION", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("CENTRO_COSTE", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("FONDO", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("AREA_FUNCIONAL", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("TIPO_ACTIVO", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("CARACTERISTICAS", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("MUNICIPIO", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("DESTINO_INVERSION", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("ACCION_LEGAL", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("CRTI_CLASIF_5", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("CLAVE_PROVEEDOR", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("PROVEEDOR", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("FABRICANTE", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("PAIS_ORIGEN", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("DENOMINACION_TIPO", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("FECHA_ALTA_INVENTARIO", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("VALOR_ORIGINAL", Type.GetType("System.String"));

        //BIENES INMUEBLES
        Dt_Parametro.Columns.Add("CLAVE_AGRUPAMIENTO", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("INDICADOR_PROPIEDAD", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("DELEGACION_HACIENDA", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("NIF_VALOR_UNIT", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("CARTILLA_DEL", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("MUNICIPIO_BI", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("CLASE", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("SOCIEDAD_GL", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("NO_POLIZA", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("COMENTARIO", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("INICIO", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("PRIMA", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("VALOR_BASE", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("VALOR_MANUAL", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("ACTIVO_FIJO_ORIGINAL", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("SUBNUMERO_ACTIVO_FIJO_ORIGINAL", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("ANIO_ADQUISICION_ORIGINAL", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("MOTIVO_MAL_M", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("VAL_PATRIM_MA", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("REG_PROPIEDAD_D", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("INSCRIPCION", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("TOMO_REGISTRO_PROPIEDAD", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("HOJA", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("NO_ACT", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("CES_PROPIEDA", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("CARTILLA_PARCELARIA", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("VACIO", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("SUPERFICIE", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("UNI_SUPERFICIE", Type.GetType("System.String"));
        Dt_Parametro.Columns.Add("COMENTARIO_ARRENDAMIENTO", Type.GetType("System.String"));
    }

    #endregion

    #region "Eventos"

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Descargar_Excel_Click
    ///DESCRIPCIÓN: Descarga el Listado a Excel.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 21/Diciembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Descargar_Excel_Click(object sender, EventArgs e) {
        try {
            Mev_Mee_Txt_Fecha_Inicial.Validate();
            Mev_Txt_Fecha_Final.Validate();
            Generar_Reporte("DESCARGAR_EXCEL");
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    #endregion
}

