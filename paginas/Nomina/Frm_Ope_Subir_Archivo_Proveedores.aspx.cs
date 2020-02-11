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
using System.IO;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Nomina_Operacion_Proveedores.Negocio;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Prestamos.Negocio;
using System.Collections.Generic;
using Presidencia.Proveedores.Negocios;
using Presidencia.Empleados.Negocios;
using Presidencia.Deducciones_Variables.Negocio;

public partial class paginas_Nomina_Frm_Ope_Subir_Archivo_Proveedores : System.Web.UI.Page
{

    #region Page Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Evento de Entrada de la Página
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 21/Abril/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN 
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        try
        {
            if (!IsPostBack)
            {
                Consultar_Calendarios_Nomina();
                Llenar_Combo_Proveedores();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = "Ocurrio un Error al Cargar.";
            Lbl_Mensaje_Error.Text = "Error: [ " + Ex.Message + " ]";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    #endregion

    #region Metodos

    #region Cargar Archivos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Carga el Archivo en El Grid
    ///PARAMETROS: 
    ///             1. Archivo. Ruta del Archivo del Cual se cargarán los datos.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 21/Abril/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN 
    ///*******************************************************************************
    private DataTable Cargar_Datos_Archivo(String Archivo)
    {
        DataTable Dt_Datos = null;
        StreamReader Lector_Archivo = new StreamReader(Archivo);
        String Fila = String.Empty;
        Int32 Cantidad = 0;
        while (true)
        {
            Fila = Lector_Archivo.ReadLine();
            if (Fila == null)
            {
                break;
            }
            if (0 == Cantidad++)
            {
                Dt_Datos = Crear_Columnas(Fila);
            }
            else
            {
                Agregar_Fila(Fila, Dt_Datos);
            }
        }
        return Dt_Datos;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Crear_Tabla
    ///DESCRIPCIÓN: Crea el DataTable a Cargar en el Grid
    ///PARAMETROS: 
    ///             1. Fila. Fila Inicial de donde se sacaran los nombres de las columnas.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 21/Abril/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN 
    ///*******************************************************************************
    private DataTable Crear_Columnas(String Fila)
    {
        Int32 Cant_Columnas;
        DataTable Dt_Datos = new DataTable("Datos");
        String[] Valores = Fila.Split(new char[] { ',' });
        Cant_Columnas = Valores.Length;
        for (Int32 Contador = 0; Contador < Cant_Columnas; Contador++)
        {
            String Nombre_Columna = Valores[Contador].ToString();
            Dt_Datos.Columns.Add(Nombre_Columna, Type.GetType("System.String"));
        }
        return Dt_Datos;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Agregar_Fila
    ///DESCRIPCIÓN: Crea el DataTable a Cargar en el Grid
    ///PARAMETROS: 
    ///             1. Fila. Datos para cargar en la Fila.
    ///             2. Dt_Datos.  Tabla en la que se cargarán los datos.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 21/Abril/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN 
    ///*******************************************************************************
    private void Agregar_Fila(String Fila, DataTable Dt_Datos)
    {
        Int32 Cant_Columnas = Dt_Datos.Columns.Count;
        String[] Valores = Fila.Split(new char[] { ',' });
        Int32 No_Datos_Fila = Valores.Length;
        if (No_Datos_Fila > Cant_Columnas)
        {
            Int32 diferencia = No_Datos_Fila - Cant_Columnas;
            Cant_Columnas = No_Datos_Fila;
        }
        DataRow Fila_Tabla = Dt_Datos.NewRow();
        for (Int32 Contador = 0; Contador < Dt_Datos.Columns.Count; Contador++)
        {
            String Nombre_Columna = Dt_Datos.Columns[Contador].ColumnName;
            Fila_Tabla[Nombre_Columna] = Valores[Contador].ToString();
        }
        Dt_Datos.Rows.Add(Fila_Tabla);
    }

    #endregion

    #region Llenar Combos

    #region Calendario Nomina

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
        DataTable Dt_Calendarios_Nominales = null;//Variable que almacena los calendarios nominales que existén actualmente en el sistema.
        try
        {
            Dt_Calendarios_Nominales = Obj_Calendario_Nominales.Consultar_Calendario_Nominas();
            Dt_Calendarios_Nominales = Formato_Fecha_Calendario_Nomina(Dt_Calendarios_Nominales);

            if (Dt_Calendarios_Nominales is DataTable)
            {
                Cmb_Calendario_Nomina.DataSource = Dt_Calendarios_Nominales;
                Cmb_Calendario_Nomina.DataTextField = "Nomina";
                Cmb_Calendario_Nomina.DataValueField = Cat_Nom_Calendario_Nominas.Campo_Nomina_ID;
                Cmb_Calendario_Nomina.DataBind();
                Cmb_Calendario_Nomina.Items.Insert(0, new ListItem("<-- SELECCIONE -->", ""));
                Cmb_Calendario_Nomina.SelectedIndex = -1;
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
        DataTable Dt_Periodos_Catorcenales = null;//Variable que almacenra unaa lista de los periodos catorcenales que le correspónden a la nomina seleccionada.

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
                    Cmb_Periodos_Catorcenales_Nomina.Items.Insert(0, new ListItem("<-- SELECCIONE -->", ""));
                    Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = -1;

                    Validar_Periodos_Pago(Cmb_Periodos_Catorcenales_Nomina);
                }
                else
                {
                    Lbl_Ecabezado_Mensaje.Text = "No se encontraron periodos catorcenales para la nomina seleccionada.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
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
    private DataTable Formato_Fecha_Calendario_Nomina(DataTable Dt_Calendario_Nominas)
    {
        DataTable Dt_Nominas = new DataTable();
        DataRow Renglon_Dt_Clon = null;
        Dt_Nominas.Columns.Add("Nomina", typeof(System.String));
        Dt_Nominas.Columns.Add(Cat_Nom_Calendario_Nominas.Campo_Nomina_ID, typeof(System.String));

        if (Dt_Calendario_Nominas is DataTable)
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
        DataTable Dt_Detalles_Nomina = null;//Variable que almacenra los detalles del periodo seleccionado.
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

                        if (Fecha_Fin >= Fecha_Actual)
                        {
                            Elemento.Enabled = true;
                        }
                        else
                        {
                            Elemento.Enabled = false;
                        }
                    }
                }
            }
        }
    }

    #endregion

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Proveedores
    ///DESCRIPCIÓN: Llenar el Combo de los Proveedores
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 22/Abril/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN 
    ///*******************************************************************************
    private void Llenar_Combo_Proveedores()
    {
        try
        {
            Cls_Cat_Nom_Proveedores_Negocio Proveedores_Negocio = new Cls_Cat_Nom_Proveedores_Negocio();
            Proveedores_Negocio.P_Estatus = "ACTIVO";
            Cmb_Proveedores.DataSource = Proveedores_Negocio.Consultar_Proveedores();
            Cmb_Proveedores.DataTextField = Cat_Nom_Proveedores.Campo_Nombre;
            Cmb_Proveedores.DataValueField = Cat_Nom_Proveedores.Campo_Proveedor_ID;
            Cmb_Proveedores.DataBind();
            Cmb_Proveedores.Items.Insert(0, new ListItem("<-- SELECCIONE -- >", ""));
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = "Ocurrio un Error al Cargar los Proveedores.";
            Lbl_Mensaje_Error.Text = "Error: [ " + Ex.Message + " ]";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    #endregion

    #region Validaciones

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: IsNumeric
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Abril/2011
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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Carga_Archivo
    ///DESCRIPCIÓN: Valida que los campos para cargar los archivos de los proveedores.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 22/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Carga_Archivo()
    {
        Lbl_Ecabezado_Mensaje.Text = "Para hacer la Actualización es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Cmb_Proveedores.SelectedIndex == 0)
        {
            Mensaje_Error = Mensaje_Error + "+ Seleccionar el Proveedor.";
            Validacion = false;
        }
        if (Cmb_Calendario_Nomina.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br />"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar el Año de la Nomina.";
            Validacion = false;
        }
        if (Cmb_Periodos_Catorcenales_Nomina.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br />"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar el Periodo la Nomina a partir de donde se hará la deducción.";
            Validacion = false;
        }
        if (!AFU_Archivo.HasFile)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br />"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar el Archivo.";
            Validacion = false;
        }
        if (Txt_No_Periodos.Text.Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br />"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el número de Periodos en que se harán las deducciones.";
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

    #region Limpiar

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Campos
    ///DESCRIPCIÓN: Se limpian los campos del Formulario.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 22/Abril/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN 
    ///*******************************************************************************
    private void Limpiar_Campos()
    {
        Cmb_Proveedores.SelectedIndex = 0;
        Cmb_Calendario_Nomina.SelectedIndex = 0;
        Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = 0;
        Cmb_Periodos_Catorcenales_Nomina.Enabled = false;
        Txt_No_Periodos.Text = "";
        Remover_Sesiones_Ctlr_AsyncFileUpload(AFU_Archivo.ClientID);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Remover_Sesiones_Ctlr_AsyncFileUpload
    ///DESCRIPCIÓN: Metodo para limpiar un AsyncFileUpload.
    ///PARAMETROS:    
    ///             1. clientId.    Nombre del control en cliente.
    ///CREO: Juan Alberto Hernandez Negrete.
    ///FECHA_CREO: 24/Abril/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN 
    ///*******************************************************************************
    private void Remover_Sesiones_Ctlr_AsyncFileUpload(String clientId)
    {
        HttpContext currentContext;
        if (HttpContext.Current != null && HttpContext.Current.Session != null)
        {
            currentContext = HttpContext.Current;
        }
        else
        {
            currentContext = null;
        }

        if (currentContext != null)
        {
            foreach (String key in currentContext.Session.Keys)
            {
                if (key.Contains(clientId))
                {
                    currentContext.Session.Remove(key);
                    break;
                }
            }
        }
    }

    #endregion

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Calendario_Nomina_SelectedIndexChanged
    ///DESCRIPCIÓN: Consulta los periodos catorcenales de la nomina seleccionada.
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
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
            Cmb_Periodos_Catorcenales_Nomina.Enabled = true;
        }
        else
        {
            Cmb_Periodos_Catorcenales_Nomina.DataSource = new DataTable();
            Cmb_Periodos_Catorcenales_Nomina.DataBind();
            Cmb_Periodos_Catorcenales_Nomina.Items.Insert(0, new ListItem("<-- SELECCIONE -->", ""));
            Cmb_Periodos_Catorcenales_Nomina.Enabled = false;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Calendario_Nomina_SelectedIndexChanged
    ///DESCRIPCIÓN: Consulta los periodos catorcenales de la nomina seleccionada.
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Cargar_Archivo_Click(object sender, EventArgs e)
    {
        try
        {
            if (Validar_Carga_Archivo())
            {
                Cls_Ope_Nom_Proveedores_Negocio Proveedores_Negocio = new Cls_Ope_Nom_Proveedores_Negocio();
                Proveedores_Negocio.P_Proveedor_ID = Cmb_Proveedores.SelectedItem.Value;
                Proveedores_Negocio.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedItem.Value;
                Proveedores_Negocio.P_No_Nomina = Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedItem.Text.Trim());
                Proveedores_Negocio.P_No_Periodos = Convert.ToInt32(Txt_No_Periodos.Text.Trim());
                String Nombre_Archivo = @MapPath("Archivo_Proveedor.csv");
                AFU_Archivo.SaveAs(Nombre_Archivo);
                Proveedores_Negocio.P_Dt_Datos_Archivo = Cargar_Datos_Archivo(@MapPath("Archivo_Proveedor.csv"));
                Proveedores_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                Proveedores_Negocio.Subir_Informacion();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Subir Archivo Proveedores", "alert('Operación Exitosa [Subir Archivo de Proveedor: " + Cmb_Proveedores.SelectedItem.Text + "]');", true);
                Limpiar_Campos();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = "Ocurrio un Error al Subir la Información.";
            Lbl_Mensaje_Error.Text = "Error: [ " + Ex.Message + " ]";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Limpiar_Datos_Click
    ///DESCRIPCIÓN: Ejecuta el metodo de limpiar los campos del Formulario.
    ///CREO: Francisco Antonio Gallardo Castañeda
    ///FECHA_CREO: 22/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Limpiar_Datos_Click(object sender, EventArgs e)
    {
        Limpiar_Campos();
    }

    #endregion

}
