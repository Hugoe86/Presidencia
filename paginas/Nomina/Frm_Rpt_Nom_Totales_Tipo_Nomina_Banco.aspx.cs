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
using Presidencia.Tipos_Nominas.Negocios;
using Presidencia.Empleados.Negocios;
using Presidencia.Bancos_Nomina.Negocio;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Prestamos.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using System.Text;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using CarlosAg.ExcelXmlWriter;
using System.Collections.Generic;

public partial class paginas_Nomina_Frm_Rpt_Nom_Totales_Tipo_Nomina_Banco : System.Web.UI.Page
{
    #region (Init/Load)
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Configuracion_Inicial();
            }

            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";

            ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(Btn_Generar_Reporte);

        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }
    #endregion

    #region (Métodos)

    #region (Generales)
    /// *************************************************************************************
    /// NOMBRE: Configuracion_Inicial
    /// 
    /// DESCRIPCIÓN: Carga y habilita la configuracón inicial de la página.
    ///              
    /// PARÁMETROS: No Aplicá
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 9/Mayo/2011 17:45 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Configuracion_Inicial() {
        Consultar_Calendarios_Nomina();//Consultamos los calendarios  de nómina que existen actualmente en sistema.  
        Cmb_Calendario_Nomina.Focus();
    }

    protected void Ejecutar_Generacion_Reporte() {
        System.Data.DataTable Dt_Totales = null;
 
        try
        {
            Dt_Totales = Crear_Estructura_Tabla_Reporte();
            //Generar_Excel_Desde_DataTable(Dt_Totales);
            Generar_Rpt_Total_Tipos_Nomina_Bancos(Dt_Totales);
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    #endregion

    #region (Consulta)
    /// ***********************************************************************************************************************************
    /// NOMBRE: Consultar_Tipos_Nominas
    /// 
    /// DESCRIPCIÓN: Consulta los tipos de nómina que se encuentran registradas en sistema.
    /// PARÁMETROS: No Áplica.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 18/Mayo/2011 09:53 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// ***********************************************************************************************************************************
    protected System.Data.DataTable Consultar_Tipos_Nominas()
    {
        Cls_Cat_Tipos_Nominas_Negocio Obj_Tipos_Nomina = new Cls_Cat_Tipos_Nominas_Negocio();//Variable de conexión a la capa de negocios.
        System.Data.DataTable Dt_Tipos_Nomina = null;//Variable que lista los tipos de nómina.
        var Nominas_Vigentes = new HashSet<String> { "00001", "00002", "00003", "00004", "00005", "00009" };

        try
        {            
            Dt_Tipos_Nomina = Obj_Tipos_Nomina.Consulta_Tipos_Nominas();//Consultamos los tipos de nomina registradas en sistema.

            if (Dt_Tipos_Nomina != null)
            {
                IEnumerable<DataRow> dt = from item in Dt_Tipos_Nomina.AsEnumerable()
                                          where Nominas_Vigentes.Contains(item.Field<String>(Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID))
                                          orderby item.Field<String>(Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID) ascending
                                          select item;

                if (dt != null)
                    Dt_Tipos_Nomina = dt.CopyToDataTable();
            }
            else {
                Lbl_Mensaje_Error.Text = "No se encontraron resultados en la búsqueda de tipos de nómina.";
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los tipos de nómina registradas en sistema. Error: [" + Ex.Message + "]");
        }
        return Dt_Tipos_Nomina;
    }
    /// ***********************************************************************************************************************************
    /// NOMBRE: Consultar_Bancos
    /// 
    /// DESCRIPCIÓN: Consulta los bancos que se encuentran registradas en sistema.
    /// PARÁMETROS: No Áplica.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 18/Mayo/2011 09:59 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// ***********************************************************************************************************************************
    protected System.Data.DataTable Consultar_Bancos()
    {
        Cls_Cat_Nom_Bancos_Negocio Obj_Bancos = new Cls_Cat_Nom_Bancos_Negocio();//Variable de conexion con la capa de negocios.
        System.Data.DataTable Dt_Bancos = null;//Variable que lista los bancos que se encuentran registrados en sistema.

        try
        {
            Dt_Bancos = Obj_Bancos.Consulta_Bancos();//Consultamos los bancos registrados en sistema.

            if (Dt_Bancos != null)
            {
                IEnumerable<DataRow> items = from item in Dt_Bancos.AsEnumerable()
                                             where item.Field<String>(Cat_Nom_Bancos.Campo_Tipo) == "NOMINA"
                                             select item;
                if (items != null)
                    Dt_Bancos = items.CopyToDataTable();
            }
            else
            {
                Lbl_Mensaje_Error.Text = "No se encontraron resultados en la búsqueda de bancos.";
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los bancos registrados en sistema. Error: [" + Ex.Message + "]");
        }
        return Dt_Bancos;
    }
    #endregion 

    #region (Tabla Reporte)
    /// ***********************************************************************************************************************************
    /// NOMBRE: Crear_Estructura_Tabla_Reporte
    /// 
    /// DESCRIPCIÓN: Crea la tabla que almacenara los totales por tipo de nomina y por cada banco.
    /// 
    /// PARÁMETROS: No Áplica.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 18/Mayo/2011 12:49 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// ***********************************************************************************************************************************
    protected System.Data.DataTable Crear_Estructura_Tabla_Reporte()
    {
        Cls_Cat_Empleados_Negocios Obj_Empleados = null;                        //Variable de conexión con la capa de negocios.
        Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nominas = null;   //Variable de conexión con la capa de negocios.
        System.Data.DataTable Dt_Totales_Tipo_Nomina_Bancos = null;             //Variable que almacenara los totales por tipo de nomina de cada banco.
        System.Data.DataTable Dt_Tipos_Nomina = null;                           //Variable que listara los tipos de nómina registradas en sistema.
        System.Data.DataTable Dt_Bancos = null;                                 //Variable que listara los bancos registrados en sistema.
        System.Data.DataTable Dt_Aux = null;                                    //Variable que almacenara registros de manera temporal.
        System.Data.DataTable Dt_Detalle_Calendario_Nomina = null;              //Variable que almacenara los detalles de la nómina consultada. 
        DataRow FILA_INSERTAR = null;                                           //Variable que guaradara el registro a insertar al tabla de totales de nomina.        
        String Valor_Celda = String.Empty;                                      //Variable que almacenara el total de acuerdo al tipo de nómina y banco consultado.
        String Nombre_Columna = String.Empty;                                   //Variable que almacenara el nombre de la columna de la tabla de totales.
        String Nomina_ID = String.Empty;                                        //Variable que almacenara el identificador de la nómina consultada.
        String No_Nomina = String.Empty;                                        //Variable que almacenara el periodo a consultar.
        String Fecha_Termino_Periodo = String.Empty;                            //Variable que almacena la fecha de termino del periodo consultado.
        Double Total = 0;                                                       //Variable que almacena el total por cada banco.

        try
        {
            Obj_Empleados = new Cls_Cat_Empleados_Negocios();
            Obj_Calendario_Nominas = new Cls_Cat_Nom_Calendario_Nominas_Negocio();
            Dt_Totales_Tipo_Nomina_Bancos = new System.Data.DataTable("Totales_Tipo_Nomina_Banco");

            Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
            No_Nomina = Cmb_Periodos_Catorcenales_Nomina.SelectedValue.Trim();

            Obj_Calendario_Nominas.P_Nomina_ID = Nomina_ID;
            Obj_Calendario_Nominas.P_No_Nomina = Convert.ToInt32(No_Nomina); ;
            Dt_Detalle_Calendario_Nomina =Obj_Calendario_Nominas.Consultar_Periodos_Por_Nomina_Periodo();

            if (Dt_Detalle_Calendario_Nomina is System.Data.DataTable) {
                if (Dt_Detalle_Calendario_Nomina.Rows.Count > 0) {
                    foreach (DataRow CALENDARIO in Dt_Detalle_Calendario_Nomina.Rows) {
                        if (CALENDARIO is DataRow) {
                            if (!String.IsNullOrEmpty(CALENDARIO[Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString())) {
                                Fecha_Termino_Periodo = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(CALENDARIO[Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString()));
                            }
                        }
                    }
                }
            }

            Dt_Tipos_Nomina = Consultar_Tipos_Nominas();//Consultamos los tipos de nómina.

            Dt_Totales_Tipo_Nomina_Bancos.Columns.Add("NO_CUENTA", typeof(String));
            Dt_Totales_Tipo_Nomina_Bancos.Columns.Add("BANCO", typeof(String));

            if (Dt_Tipos_Nomina is System.Data.DataTable) {
                if (Dt_Tipos_Nomina.Rows.Count > 0) { 
                    foreach(DataRow TIPO_NOMINA in Dt_Tipos_Nomina.Rows){
                        if (TIPO_NOMINA is DataRow)
                        {
                            if (!String.IsNullOrEmpty(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID].ToString()))
                            {
                                Nombre_Columna = TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Nomina].ToString();
                                Dt_Totales_Tipo_Nomina_Bancos.Columns.Add(Nombre_Columna, typeof(String));
                                Dt_Totales_Tipo_Nomina_Bancos.Columns[Nombre_Columna].Caption = TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID].ToString();
                            }
                        }
                    }
                }
            }
            Dt_Totales_Tipo_Nomina_Bancos.Columns.Add("TOTAL", typeof(String));
            Dt_Totales_Tipo_Nomina_Bancos.Columns.Add("APLICACION_RECURSO", typeof(String));

            Dt_Bancos = Consultar_Bancos();//Cosnultamos los bancos que se encuentran registrados en sistema.

            if (Dt_Bancos is System.Data.DataTable)
            {
                if (Dt_Bancos.Rows.Count > 0)
                {                    
                    foreach (DataRow BANCO in Dt_Bancos.Rows)
                    {
                        FILA_INSERTAR = Dt_Totales_Tipo_Nomina_Bancos.NewRow();//Creamos la estructura de la fila.
                        if (BANCO is DataRow)
                        {
                            if (!String.IsNullOrEmpty(BANCO[Cat_Nom_Bancos.Campo_Banco_ID].ToString()))
                            {
                                if (Dt_Totales_Tipo_Nomina_Bancos is System.Data.DataTable)
                                {
                                    if (Dt_Totales_Tipo_Nomina_Bancos.Columns.Count > 0)
                                    {
                                        foreach (DataColumn COLUMNA in Dt_Totales_Tipo_Nomina_Bancos.Columns)
                                        {
                                            if (COLUMNA is DataColumn)
                                            {
                                                if (IsNumeric(COLUMNA.Caption.Trim()))
                                                {
                                                    Obj_Empleados.P_Nomina_ID = Nomina_ID;
                                                    Obj_Empleados.P_No_Nomina = No_Nomina;
                                                    Obj_Empleados.P_Tipo_Nomina_ID = COLUMNA.Caption.Trim();
                                                    Obj_Empleados.P_Banco_ID = BANCO[Cat_Nom_Bancos.Campo_Banco_ID].ToString();
                                                    Dt_Aux = Obj_Empleados.Consultar_Total_Pago_Bancos_Tipo_Nomina();

                                                    if (Dt_Aux is System.Data.DataTable)
                                                    {
                                                        if (Dt_Aux.Rows.Count > 0)
                                                        {
                                                            if (!String.IsNullOrEmpty(Dt_Aux.Rows[0]["TOTAL"].ToString()))
                                                            {
                                                                Valor_Celda = Dt_Aux.Rows[0]["TOTAL"].ToString();
                                                                Total += Convert.ToDouble(Valor_Celda);
                                                            }
                                                        }
                                                    }
                                                }
                                                else if (COLUMNA.ColumnName.ToUpper().Trim().Contains("NO_CUENTA"))
                                                {
                                                    Valor_Celda = BANCO[Cat_Nom_Bancos.Campo_No_Cuenta].ToString();
                                                }
                                                else if (COLUMNA.ColumnName.ToUpper().Trim().Contains("BANCO")) {
                                                    Valor_Celda = BANCO[Cat_Nom_Bancos.Campo_Nombre].ToString();
                                                }

                                                if (IsNumeric(Valor_Celda.Replace(".", "")) && !COLUMNA.ColumnName.Equals("NO_CUENTA") && !COLUMNA.ColumnName.Equals("BANCO"))
                                                    FILA_INSERTAR[COLUMNA.ColumnName] = String.Format("{0:c}",
                                                        Convert.ToDouble(String.IsNullOrEmpty(Valor_Celda) ? "0" : Valor_Celda.Trim()));
                                                else
                                                    FILA_INSERTAR[COLUMNA.ColumnName] = Valor_Celda; 

                                                Valor_Celda = String.Empty;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        FILA_INSERTAR["TOTAL"] = String.Format("{0:c}", Total);
                        FILA_INSERTAR["APLICACION_RECURSO"] = Fecha_Termino_Periodo;
                        Total = 0;
                        Dt_Totales_Tipo_Nomina_Bancos.Rows.Add(FILA_INSERTAR);//Insertamos la fila a la tabla de totales.
                    }
                }
            }          
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al crear la estructura de la tabla que se creara para el reporte. Error: [" + Ex.Message + "]");
        }
        return Dt_Totales_Tipo_Nomina_Bancos;
    }
    #endregion

    #region (Calendario Nomina)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Calendarios_Nomina
    /// DESCRIPCION : 
    /// 
    /// PARAMETROS:
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Calendarios_Nomina()
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nominales = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexión con la capa de negocios.
        System.Data.DataTable Dt_Calendarios_Nominales = null;//Variable que almacena los calendarios nominales que existén actualmente en el sistema.
        try
        {
            Dt_Calendarios_Nominales = Obj_Calendario_Nominales.Consultar_Calendario_Nominas();
            Dt_Calendarios_Nominales = Formato_Fecha_Calendario_Nomina(Dt_Calendarios_Nominales);

            if (Dt_Calendarios_Nominales is System.Data.DataTable)
            {
                Cmb_Calendario_Nomina.DataSource = Dt_Calendarios_Nominales;
                Cmb_Calendario_Nomina.DataTextField = "Nomina";
                Cmb_Calendario_Nomina.DataValueField = Cat_Nom_Calendario_Nominas.Campo_Nomina_ID;
                Cmb_Calendario_Nomina.DataBind();
                Cmb_Calendario_Nomina.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));

                Cmb_Calendario_Nomina.SelectedIndex = Cmb_Calendario_Nomina.Items.IndexOf
                    (Cmb_Calendario_Nomina.Items.FindByText(new Presidencia.Ayudante_Calendario_Nomina.Cls_Ayudante_Calendario_Nomina().P_Anyo));

                if (Cmb_Calendario_Nomina.SelectedIndex > 0)
                {
                    Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario_Nomina.SelectedValue.Trim());
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los calendarios de nómina que existen actualmente registrados en el sistema. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Periodos_Catorcenales_Nomina
    ///DESCRIPCIÓN: Consulta los periodos catorcenales para el 
    ///calendario de nomina seleccionado.
    ///PARAMETROS: Nomina_ID.- Indica el calendario de nomina del cuál se desea consultar
    ///                        los periodos catorcenales.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Periodos_Catorcenales_Nomina(String Nomina_ID)
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Consulta_Calendario_Nomina_Periodos = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Clase de conexion con la capa de negocios
        System.Data.DataTable Dt_Periodos_Catorcenales = null;//Variable que almacenra unaa lista de los periodos catorcenales que le correspónden a la nomina seleccionada.

        try
        {
            Consulta_Calendario_Nomina_Periodos.P_Nomina_ID = Nomina_ID;
            Dt_Periodos_Catorcenales = Consulta_Calendario_Nomina_Periodos.Consulta_Detalles_Nomina();
            if (Dt_Periodos_Catorcenales != null)
            {
                if (Dt_Periodos_Catorcenales.Rows.Count > 0)
                {
                    Cmb_Periodos_Catorcenales_Nomina.DataSource = Dt_Periodos_Catorcenales;
                    Cmb_Periodos_Catorcenales_Nomina.DataTextField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                    Cmb_Periodos_Catorcenales_Nomina.DataValueField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                    Cmb_Periodos_Catorcenales_Nomina.DataBind();
                    Cmb_Periodos_Catorcenales_Nomina.Items.Insert(0, new ListItem("< Seleccione >", ""));
                    Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = -1;

                    Validar_Periodos_Pago(Cmb_Periodos_Catorcenales_Nomina);

                    Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = Cmb_Periodos_Catorcenales_Nomina.Items.IndexOf(Cmb_Periodos_Catorcenales_Nomina.Items.FindByText(new Presidencia.Ayudante_Calendario_Nomina.Cls_Ayudante_Calendario_Nomina().P_Periodo));
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No se encontraron periodos catorcenales para la nomina seleccionada.";
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los periodos catorcenales del  calendario de nomina seleccionado. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Formato_Fecha_Calendario_Nomina
    /// DESCRIPCION : Crea el DataTable con la consulta de las nomina vigentes en el 
    /// sistema.
    /// PARAMETROS: Dt_Calendario_Nominas.- Lista de las nominas vigentes actualmente 
    ///             en el sistema.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private System.Data.DataTable Formato_Fecha_Calendario_Nomina(System.Data.DataTable Dt_Calendario_Nominas)
    {
        System.Data.DataTable Dt_Nominas = new System.Data.DataTable();
        DataRow Renglon_Dt_Clon = null;
        Dt_Nominas.Columns.Add("Nomina", typeof(System.String));
        Dt_Nominas.Columns.Add(Cat_Nom_Calendario_Nominas.Campo_Nomina_ID, typeof(System.String));

        if (Dt_Calendario_Nominas is System.Data.DataTable)
        {
            foreach (DataRow Renglon in Dt_Calendario_Nominas.Rows)
            {
                if (Renglon is DataRow)
                {
                    Renglon_Dt_Clon = Dt_Nominas.NewRow();
                    Renglon_Dt_Clon["Nomina"] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Fecha_Fin].ToString().Split(new char[] { ' ' })[0].Split(new char[] { '/' })[2];
                    Renglon_Dt_Clon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID];
                    Dt_Nominas.Rows.Add(Renglon_Dt_Clon);
                }
            }
        }
        return Dt_Nominas;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Periodos_Pago
    ///DESCRIPCIÓN: Valida que el empleado solo puedan comenzar a descontar la deduccion 
    ///a partir del periodo actual.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Validar_Periodos_Pago(DropDownList Combo)
    {
        Cls_Ope_Nom_Pestamos_Negocio Prestamos = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocios.
        System.Data.DataTable Dt_Detalles_Nomina = null;//Variable que almacenra los detalles del periodo seleccionado.
        DateTime Fecha_Actual = DateTime.Now;
        DateTime Fecha_Inicio = new DateTime();
        DateTime Fecha_Fin = new DateTime();

        Prestamos.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();

        foreach (ListItem Elemento in Combo.Items)
        {
            if (IsNumeric(Elemento.Text.Trim()))
            {
                Prestamos.P_No_Nomina = Convert.ToInt32(Elemento.Text.Trim());
                Dt_Detalles_Nomina = Prestamos.Consultar_Fechas_Periodo();

                if (Dt_Detalles_Nomina != null)
                {
                    if (Dt_Detalles_Nomina.Rows.Count > 0)
                    {
                        Fecha_Inicio = Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString());
                        Fecha_Fin = Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString());
                    }
                }
            }
        }
    }
    #endregion

    #region (Validaciones)
    ///*********************************************************************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos_Operacion
    /// 
    /// DESCRIPCION : Validar datos requeridos para realizar la operación.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 18/Mayo/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*********************************************************************************************************************************
    private Boolean Validar_Datos()
    {
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

        try
        {
            if (Cmb_Calendario_Nomina.SelectedIndex <= 0)
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Es necesario seleccionar un calendario de nómina para generar el reporte. <br>";
                Datos_Validos = false;
            }

            if (Cmb_Periodos_Catorcenales_Nomina.SelectedIndex <= 0)
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Es necesario seleccionar el periodo para generar el reporte. <br>";
                Datos_Validos = false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al validar los datos de la incapacidad del empleado. Error: [" + Ex.Message + "]");
        }
        return Datos_Validos;
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
    private Boolean IsNumeric(String Cadena)
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

    #region (Generar Documento Excel)
    /// ***********************************************************************************************************************************
    /// NOMBRE: Generar_Excel_Desde_DataTable
    /// 
    /// DESCRIPCIÓN: Genera el archivo de excel a partir de la tabla que se le pasa como parámetro al método.
    /// 
    /// PARÁMETROS: Dt_Datos.- Tabla que almacena los datos que serán mostrados en el documento de excel.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 18/Mayo/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// ***********************************************************************************************************************************
    private void Generar_Excel_Desde_DataTable(System.Data.DataTable Dt_Datos)
    {
        Int32 Contador_Columnas = 0;                                        //Variable que contara las columnas de la tabla. 
        Int32 Contador_Filas = 0;                                           //Variable que contara las filas de la tabla. 
        Int32 CABECERA = 1;                                                 //Establecemos como constante la posision de la cabecera.
        Microsoft.Office.Interop.Excel.ApplicationClass Obj_Excel = new Microsoft.Office.Interop.Excel.ApplicationClass();                //Se  crea el objeto excel.
        Microsoft.Office.Interop.Excel.Workbook Libro_Trabajo = Obj_Excel.Application.Workbooks.Add(true); //Se crea el libro de trabajo.
        Object Valor_Desconocido = System.Reflection.Missing.Value;         //Variable almacena el valor para los parámetros desconocidos.
        String Ruta_Servidor = @Server.MapPath("../../Reporte") + 
                @"\Rpt_Nom_Totales_Tipo_Nomina_Banco.xls";                  //Variable que almacenara la ruta y el nombre del archivo a generar.
        
        Obj_Excel.Columns.ColumnWidth = 25;//Establecemos el tamaño que tendrá cada columna.
           
        if (Dt_Datos is System.Data.DataTable)
        {
            if (Dt_Datos.Rows.Count > 0)
            {
                foreach (DataColumn COLUMNA in Dt_Datos.Columns)
                {
                    if (COLUMNA is DataColumn)
                    {
                        Contador_Columnas++;//Incrementamos el contador de columnas.

                        Obj_Excel.Cells[1, Contador_Columnas] = COLUMNA.ColumnName;//Establecemos la cabecera de la columna.
                        Microsoft.Office.Interop.Excel.Range Obj_Range_Celda_Cabecera = (Microsoft.Office.Interop.Excel.Range)Obj_Excel.Cells[CABECERA, Contador_Columnas];//Obtenemos el objeto range.

                        //Establecemos el tipo de fuente.
                        Obj_Range_Celda_Cabecera.Font.Name = "Verdana";
                        Obj_Range_Celda_Cabecera.Font.Size = 10;
                        Obj_Range_Celda_Cabecera.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                        Obj_Range_Celda_Cabecera.Font.Bold = true;
                                                
                        //Establecemos bordes a la celdas.
                        Obj_Range_Celda_Cabecera.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Silver);
                        Obj_Range_Celda_Cabecera.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)Microsoft.Office.Interop.Excel.Constants.xlTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                        Obj_Range_Celda_Cabecera.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)Microsoft.Office.Interop.Excel.Constants.xlBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                        Obj_Range_Celda_Cabecera.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)Microsoft.Office.Interop.Excel.Constants.xlLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                        Obj_Range_Celda_Cabecera.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)Microsoft.Office.Interop.Excel.Constants.xlRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                        //Alineamos el contenido de la celda.
                        Obj_Range_Celda_Cabecera.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                        Obj_Range_Celda_Cabecera.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    }
                }
            }
        }

        //Devolvemos los contadores a ceros.
        Contador_Filas = 0;
        Contador_Columnas = 0;

        if (Dt_Datos is System.Data.DataTable) {
            if (Dt_Datos.Rows.Count > 0) {
                foreach (DataRow FILA in Dt_Datos.Rows) {
                    if (FILA is DataRow) {
                        Contador_Filas++;//Incrementamos el contador de filas.
                        if (Dt_Datos.Columns.Count > 0) {
                            foreach (DataColumn COLUMNA in Dt_Datos.Columns) {
                                if (COLUMNA is DataColumn) {
                                    Contador_Columnas++;//Incrementamos el contador de columnas.
                                    Obj_Excel.Cells[(Contador_Filas + CABECERA), Contador_Columnas] = FILA[COLUMNA.ColumnName];//Establecemos el valor que almacenara la celda.

                                    Microsoft.Office.Interop.Excel.Range Obj_Range_Celda_Contenido = (Microsoft.Office.Interop.Excel.Range)Obj_Excel.Cells[(Contador_Filas + CABECERA), Contador_Columnas];//Obtenemos el objeto range.

                                    //Establecemos el tipo de fuente.
                                    Obj_Range_Celda_Contenido.Font.Name = "Verdana";
                                    Obj_Range_Celda_Contenido.Font.Size = 9;
                                    Obj_Range_Celda_Contenido.Font.Bold = false;
                                    Obj_Range_Celda_Contenido.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                                    
                                    //Establecemos un color de fondo para las celdas.
                                    Obj_Range_Celda_Contenido.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);

                                    //Establecemos los bordes de las celdas.
                                    Obj_Range_Celda_Contenido.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)Microsoft.Office.Interop.Excel.Constants.xlTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                                    Obj_Range_Celda_Contenido.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)Microsoft.Office.Interop.Excel.Constants.xlBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                                    Obj_Range_Celda_Contenido.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)Microsoft.Office.Interop.Excel.Constants.xlLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                                    Obj_Range_Celda_Contenido.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)Microsoft.Office.Interop.Excel.Constants.xlRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                                    //Establecemos la alineacion del contenido de las celdas.
                                    Obj_Range_Celda_Contenido.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                                    Obj_Range_Celda_Contenido.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                                    //Establecemos el formato para los datos de tipo cantidad.
                                    if (Contador_Columnas > 1 && Contador_Columnas < Dt_Datos.Columns.Count) Obj_Range_Celda_Contenido.NumberFormat = "$0.00";
                                }
                            }
                            Contador_Columnas = 0;
                        }
                    }
                }
            }
        }

        //Guardamos el documento que se genero a partir de la tabla.
        Libro_Trabajo.SaveAs(Ruta_Servidor, Microsoft.Office.Interop.Excel.XlFileFormat.xlXMLSpreadsheet, Valor_Desconocido, Valor_Desconocido, false, false,
                             Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Valor_Desconocido, Valor_Desconocido, Valor_Desconocido, Valor_Desconocido, 
                             Valor_Desconocido);

        //Para abrir el documento de excel.               
        Obj_Excel.Visible = true;

        Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)Obj_Excel.ActiveSheet;
        ((Microsoft.Office.Interop.Excel._Worksheet)worksheet).Activate();

        //Si no quieres mostrar el documento de excel.                
        //((_Application)Obj_Excel).Quit();
    }
    /// *************************************************************************************************************************
    /// Nombre: Generar_Rpt_Caja_Libertad_Excel
    /// 
    /// Descripción: Este método genera y muestra el archivo de dispersión a Caja Libertad. [Reporte_Caja_Libertad.xls]
    /// 
    /// Parámetros: Dt_Reporte.- Información que se mostrara en el reporte de Caja Libertad.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 18/Octubre/2011.
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    public void Generar_Rpt_Total_Tipos_Nomina_Bancos(System.Data.DataTable Dt_Reporte)
    {
        String Ruta = "Rpt_Nom_Totales_Tipo_Nomina_Banco.xls";//Variable que almacenara el nombre del archivo. 

        try
        {
            //Creamos el libro de Excel.
            CarlosAg.ExcelXmlWriter.Workbook Libro = new CarlosAg.ExcelXmlWriter.Workbook();

            Libro.Properties.Title = "Totales_Tipo_Nomina_Banco";
            Libro.Properties.Created = DateTime.Now;
            Libro.Properties.Author = "Presidencia_RH";

            //Creamos una hoja que tendrá el libro.
            CarlosAg.ExcelXmlWriter.Worksheet Hoja = Libro.Worksheets.Add("Totales_Tipo_Nomina_Banco");
            //Agregamos un renglón a la hoja de excel.
            CarlosAg.ExcelXmlWriter.WorksheetRow Renglon = Hoja.Table.Rows.Add();
            //Creamos el estilo cabecera para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Cabecera = Libro.Styles.Add("HeaderStyle");
            //Creamos el estilo contenido para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Contenido = Libro.Styles.Add("BodyStyle");

            Estilo_Cabecera.Font.FontName = "Tahoma";
            Estilo_Cabecera.Font.Size = 10;
            Estilo_Cabecera.Font.Bold = true;
            Estilo_Cabecera.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Estilo_Cabecera.Font.Color = "#FFFFFF";
            Estilo_Cabecera.Interior.Color = "#193d61";
            Estilo_Cabecera.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Cabecera.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

            Estilo_Contenido.Font.FontName = "Tahoma";
            Estilo_Contenido.Font.Size = 9;
            Estilo_Contenido.Font.Bold = true;
            Estilo_Contenido.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Estilo_Contenido.Font.Color = "#000000";
            Estilo_Contenido.Interior.Color = "White";
            Estilo_Contenido.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Contenido.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

            //Agregamos las columnas que tendrá la hoja de excel.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(190));//No Cuenta
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));//Banco

            if (Dt_Reporte is System.Data.DataTable)
            {
                if (Dt_Reporte.Rows.Count > 0)
                {
                    foreach (System.Data.DataColumn COLUMNA in Dt_Reporte.Columns)
                    {
                        if (COLUMNA is System.Data.DataColumn)
                        {
                            Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(COLUMNA.ColumnName, DataType.String, "HeaderStyle"));
                        }
                    }

                    foreach (System.Data.DataRow FILA in Dt_Reporte.Rows)
                    {
                        if (FILA is System.Data.DataRow)
                        {
                            Renglon = Hoja.Table.Rows.Add();

                            foreach (System.Data.DataColumn COLUMNA in Dt_Reporte.Columns)
                            {
                                if (COLUMNA is System.Data.DataColumn)
                                {
                                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(FILA[COLUMNA.ColumnName].ToString(), DataType.String, "BodyStyle"));
                                }
                            }
                        }
                    }
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
            Response.Flush();
            Response.Close();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte de Caja Libertad. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #endregion

    #region (Eventos)

    #region (Eventos Botones)
    /// ***********************************************************************************************************************************
    /// NOMBRE: Btn_Generar_Reporte_Click
    /// 
    /// DESCRIPCIÓN: Genera el archivo de excel a partir de la tabla que se le pasa como parámetro al método.
    /// 
    /// PARÁMETROS: No Áplica.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 18/Mayo/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// ***********************************************************************************************************************************
    protected void Btn_Generar_Reporte_Click(Object sender, EventArgs e) {
        try
        {
            if (Validar_Datos())
            {
                Ejecutar_Generacion_Reporte();//Ejecuta la generación del documento de excel.
            }
            else {
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Visible = true;                
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }
    #endregion

    #region (Eventos Combos)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Calendario_Nomina_SelectedIndexChanged
    ///
    ///DESCRIPCIÓN: Consulta los periodos catorcenales de la nomina seleccionada.
    ///
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: 18/Mayo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Calendario_Nomina_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 index = Cmb_Calendario_Nomina.SelectedIndex;
        if (index > 0)
        {
            Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario_Nomina.SelectedValue.Trim());
        }
        else
        {
            Cmb_Periodos_Catorcenales_Nomina.DataSource = new System.Data.DataTable();
            Cmb_Periodos_Catorcenales_Nomina.DataBind();
        }
        Cmb_Calendario_Nomina.Focus();
    }
    #endregion

    #endregion
}
