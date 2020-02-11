using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Tipos_Nominas.Negocios;
using Presidencia.Dependencias.Negocios;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Reportes_Nomina_Nomina_Por_Dependencia.Negocio;
using Presidencia.Sessiones;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Prestamos.Negocio;

public partial class paginas_Nomina_Reporte_Frm_Rpt_Nom_Nomina_Por_Dependencia : System.Web.UI.Page
{
    #region (Load/Init)
    /// *************************************************************************************
    /// NOMBRE: Page_Load
    /// 
    /// DESCRIPCIÓN: Habilita la configuración inicial de la página.
    ///              
    /// PARÁMETROS: No Aplica
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 18:25 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Estado_Inicial();
            }

            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    #endregion

    #region (Métodos)

    #region (Métodos Generales)
    /// *************************************************************************************
    /// NOMBRE: Estado_Inicial
    /// 
    /// DESCRIPCIÓN: Método que carga y habilita los controles a un estado inicial
    ///              para comenzar las operaciones.
    ///              
    /// PARÁMETROS: No Aplica
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 11:34 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Estado_Inicial()
    {
        try
        {
            Consultar_Tipos_Nominas();          //Carga los tipos de nómina registradas en sistema.
            Consultar_Unidades_Responsables();  //Carga la unidades responsables registrdas en sistema.
            Consultar_Calendarios_Nomina();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error generado al habilitar el estado inicial de los controles de la página. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Consultar)

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
                Cmb_Calendario_Nomina.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));
                Cmb_Calendario_Nomina.SelectedIndex = -1;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los calendarios de nómina que existen actualmente registrados en el sistema. Error: [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Consultar_Nomina
    /// DESCRIPCIÓN: Consulta los datos para el reporte 
    /// PARÁMETROS:
    /// 		1. Tipo_Percepcion_Deduccion: tipo que se va a consultar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 09-abr-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private DataTable Consultar_Nomina(string Tipo_Percepcion_Deduccion)
    {
        var Consulta_Nomina = new Cls_Rpt_Nom_Nomina_Por_Dependencia_Negocio();
        DataTable Dt_Nomina;

        // filtrar por unidad responsable si hay selección en el combo
        if (Cmb_Busqueda_Unidad_Responsable.SelectedIndex > -1)
        {
            Consulta_Nomina.P_Dependencia_ID = Cmb_Busqueda_Unidad_Responsable.SelectedValue;
        }

        // filtrar por tipo de nomina si hay selección en el combo
        if (Cmb_Busqueda_Tipo_Nomina.SelectedIndex > -1)
        {
            Consulta_Nomina.P_Tipo_Nomina_ID = Cmb_Busqueda_Tipo_Nomina.SelectedValue;
        }

        // filtrar por nomina (año y periodo) si hay selección en el combo
        if (Cmb_Periodos_Catorcenales_Nomina.SelectedIndex > -1 && Cmb_Calendario_Nomina.SelectedIndex > -1)
        {
            Consulta_Nomina.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue;
            Consulta_Nomina.P_No_Nomina = Cmb_Periodos_Catorcenales_Nomina.SelectedValue;
        }

        // filtrar tipo PERCEPCION o DEDUCCION
        if (!string.IsNullOrEmpty(Tipo_Percepcion_Deduccion))
        {
            Consulta_Nomina.P_Tipo_Percepcion_Deduccion = Tipo_Percepcion_Deduccion;
        }

        Dt_Nomina = Consulta_Nomina.Consultar_Nomina();

        return Dt_Nomina;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Formar_Tabla_Percepciones_Deducciones
    /// DESCRIPCIÓN: Consulta los datos para el reporte 
    /// PARÁMETROS:
    /// 		1. Dt_Percepciones_Deducciones: tabla de percepciones y deducciones
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 09-abr-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private DataTable Formar_Tabla_Percepciones_Deducciones(DataTable Dt_Percepciones_Deducciones)
    {
        List<string> Lista_Tipos_Nomina = new List<string>();
        List<string> Lista_Claves_Dependencia = new List<string>();
        DataTable Dt_Reporte = Crear_Tabla_Percepciones_Deducciones();
        DataTable Dt_Percepciones;
        DataTable Dt_Deducciones;
        DataRow Dr_Percepcion_Deduccion;
        int Ultima_Fila = 0;

        for (int i = 0; i < Dt_Percepciones_Deducciones.Rows.Count; i++)
        {
            // si hay un tipo de nómina y no está en la lista, agregarlo
            if (Dt_Percepciones_Deducciones.Rows[i]["TIPO_NOMINA_ID"].ToString() != "" && !Lista_Tipos_Nomina.Contains(Dt_Percepciones_Deducciones.Rows[i]["TIPO_NOMINA_ID"].ToString()))
            {
                Lista_Tipos_Nomina.Add(Dt_Percepciones_Deducciones.Rows[i]["TIPO_NOMINA_ID"].ToString());
            }
            // si hay una dependencia y no está en la lista, agregarla
            if (Dt_Percepciones_Deducciones.Rows[i]["CLAVE_DEPENDENCIA"].ToString() != "" && !Lista_Claves_Dependencia.Contains(Dt_Percepciones_Deducciones.Rows[i]["CLAVE_DEPENDENCIA"].ToString()))
            {
                Lista_Claves_Dependencia.Add(Dt_Percepciones_Deducciones.Rows[i]["CLAVE_DEPENDENCIA"].ToString());
            }
        }

        // recorrer todos los tipos de nomina y todas las claves para obtener los registros
        foreach (string Tipo_Nomina in Lista_Tipos_Nomina)
        {
            foreach (string Clave_Dependencia in Lista_Claves_Dependencia)
            {
                Dt_Deducciones = (from DataRow Fila in Dt_Percepciones_Deducciones.AsEnumerable()
                                  where Fila.Field<string>("TIPO") == "DEDUCCION"
                                  && Fila.Field<decimal>("TIPO_NOMINA_ID") == Convert.ToDecimal(Tipo_Nomina)
                                  && Fila.Field<string>("CLAVE_DEPENDENCIA") == Clave_Dependencia
                                  select Fila).AsDataView().ToTable();

                Dt_Percepciones = (from DataRow Fila in Dt_Percepciones_Deducciones.AsEnumerable()
                                   where Fila.Field<string>("TIPO") == "PERCEPCION"
                                   && Fila.Field<decimal>("TIPO_NOMINA_ID") == Convert.ToDecimal(Tipo_Nomina)
                                   && Fila.Field<string>("CLAVE_DEPENDENCIA") == Clave_Dependencia
                                   select Fila).AsDataView().ToTable();

                // agregar registros a la tabla para la consulta
                // agregar registros comenzando con la tabla con más filas
                if (Dt_Deducciones.Rows.Count > Dt_Percepciones.Rows.Count)
                {
                    Ultima_Fila = Dt_Reporte.Rows.Count;
                    // agregar filas con deducciones
                    foreach (DataRow Fila in Dt_Deducciones.Rows)
                    {
                        Dr_Percepcion_Deduccion = Dt_Reporte.NewRow();
                        Dr_Percepcion_Deduccion["TIPO_NOMINA_ID"] = Fila["TIPO_NOMINA_ID"];
                        Dr_Percepcion_Deduccion["CLAVE_DEPENDENCIA"] = Fila["CLAVE_DEPENDENCIA"];
                        Dr_Percepcion_Deduccion["NOMBRE_DEPENDENCIA"] = Fila["NOMBRE_DEPENDENCIA"];
                        Dr_Percepcion_Deduccion["NOMINA"] = Fila["NOMINA"];
                        Dr_Percepcion_Deduccion["CLAVE_DEDUCCION"] = Fila["CLAVE_PERCEPCION_DEDUCCION"];
                        Dr_Percepcion_Deduccion["NOMBRE_DEDUCCION"] = Fila["NOMBRE_PERCEPCION_DEDUCCION"];
                        Dr_Percepcion_Deduccion["MONTO_DEDUCCION"] = Fila["MONTO"];
                        Dt_Reporte.Rows.Add(Dr_Percepcion_Deduccion);
                    }

                    // agregar percepciones a las filas
                    for (int i = 0; i < Dt_Percepciones.Rows.Count; i++)
                    {
                        Dt_Reporte.Rows[Ultima_Fila + i]["CLAVE_PERCEPCION"] = Dt_Percepciones.Rows[i]["CLAVE_PERCEPCION_DEDUCCION"];
                        Dt_Reporte.Rows[Ultima_Fila + i]["NOMBRE_PERCEPCION"] = Dt_Percepciones.Rows[i]["NOMBRE_PERCEPCION_DEDUCCION"];
                        Dt_Reporte.Rows[Ultima_Fila + i]["MONTO_PERCEPCION"] = Dt_Percepciones.Rows[i]["MONTO"];
                        Dt_Reporte.AcceptChanges();
                    }
                }
                else
                {
                    Ultima_Fila = Dt_Reporte.Rows.Count;
                    // agregar filas con percepciones
                    foreach (DataRow Fila in Dt_Percepciones.Rows)
                    {
                        Dr_Percepcion_Deduccion = Dt_Reporte.NewRow();
                        Dr_Percepcion_Deduccion["TIPO_NOMINA_ID"] = Fila["TIPO_NOMINA_ID"];
                        Dr_Percepcion_Deduccion["CLAVE_DEPENDENCIA"] = Fila["CLAVE_DEPENDENCIA"];
                        Dr_Percepcion_Deduccion["NOMBRE_DEPENDENCIA"] = Fila["NOMBRE_DEPENDENCIA"];
                        Dr_Percepcion_Deduccion["NOMINA"] = Fila["NOMINA"];
                        Dr_Percepcion_Deduccion["CLAVE_PERCEPCION"] = Fila["CLAVE_PERCEPCION_DEDUCCION"];
                        Dr_Percepcion_Deduccion["NOMBRE_PERCEPCION"] = Fila["NOMBRE_PERCEPCION_DEDUCCION"];
                        Dr_Percepcion_Deduccion["MONTO_PERCEPCION"] = Fila["MONTO"];
                        Dt_Reporte.Rows.Add(Dr_Percepcion_Deduccion);
                    }

                    // agregar deducciones a las filas
                    for (int i = 0; i < Dt_Deducciones.Rows.Count; i++)
                    {
                        Dt_Reporte.Rows[Ultima_Fila + i]["CLAVE_DEDUCCION"] = Dt_Deducciones.Rows[i]["CLAVE_PERCEPCION_DEDUCCION"];
                        Dt_Reporte.Rows[Ultima_Fila + i]["NOMBRE_DEDUCCION"] = Dt_Deducciones.Rows[i]["NOMBRE_PERCEPCION_DEDUCCION"];
                        Dt_Reporte.Rows[Ultima_Fila + i]["MONTO_DEDUCCION"] = Dt_Deducciones.Rows[i]["MONTO"];
                        Dt_Reporte.AcceptChanges();
                    }
                }
            }
        }

        return Dt_Reporte;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Totales_Generales
    /// DESCRIPCIÓN: Recorre la tabla de la nómina y genera una sumatoria por clave
    /// PARÁMETROS:
    /// 		1. Dt_Percepciones_Deducciones: tabla de percepciones y deducciones
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 10-abr-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private DataTable Calcular_Totales_Generales(DataTable Dt_Percepciones_Deducciones)
    {
        var Dic_Clave_Total_Percepcion = new Dictionary<string, decimal>();
        var Dic_Clave_Total_Deduccion = new Dictionary<string, decimal>();
        var Dic_Clave_Nombre_Percepcion = new Dictionary<string, string>();
        var Dic_Clave_Nombre_Deduccion = new Dictionary<string, string>();
        DataTable Dt_Totales = Crear_Tabla_Percepciones_Deducciones();
        DataRow Dr_Percepcion_Deduccion;
        string Clave_Percepcion_Deduccion;
        string Nombre_Clave;
        int Contador_Fila;

        foreach (DataRow Fila_Percepcion_Deduccion in Dt_Percepciones_Deducciones.Rows)
        {
            Clave_Percepcion_Deduccion = Fila_Percepcion_Deduccion["CLAVE_PERCEPCION_DEDUCCION"].ToString();
            // si es una PERCEPCION
            if (Fila_Percepcion_Deduccion["TIPO"].ToString() == "PERCEPCION")
            {
                // si hay una clave de percepcion-deduccion y no está en el diccionario, agregar entrada en el diccionario
                if (!string.IsNullOrEmpty(Clave_Percepcion_Deduccion) && !Dic_Clave_Total_Percepcion.ContainsKey(Clave_Percepcion_Deduccion))
                {
                    Dic_Clave_Total_Percepcion.Add(Clave_Percepcion_Deduccion, (decimal)Fila_Percepcion_Deduccion["MONTO"]);
                    Dic_Clave_Nombre_Percepcion.Add(Clave_Percepcion_Deduccion, (string)Fila_Percepcion_Deduccion["NOMBRE_PERCEPCION_DEDUCCION"]);
                }
                else // si la clave ya está en el diccionario, sumar al registro ya existente
                {
                    Dic_Clave_Total_Percepcion[Clave_Percepcion_Deduccion] += (decimal)Fila_Percepcion_Deduccion["MONTO"];
                }
            }
            else if (Fila_Percepcion_Deduccion["TIPO"].ToString() == "DEDUCCION") // si es una DEDUCCION
            {
                // si hay una clave de percepcion-deduccion y no está en el diccionario, agregar entrada en el diccionario
                if (!string.IsNullOrEmpty(Clave_Percepcion_Deduccion) && !Dic_Clave_Total_Deduccion.ContainsKey(Clave_Percepcion_Deduccion))
                {
                    Dic_Clave_Total_Deduccion.Add(Clave_Percepcion_Deduccion, (decimal)Fila_Percepcion_Deduccion["MONTO"]);
                    Dic_Clave_Nombre_Deduccion.Add(Clave_Percepcion_Deduccion, (string)Fila_Percepcion_Deduccion["NOMBRE_PERCEPCION_DEDUCCION"]);
                }
                else // si la clave ya está en el diccionario, sumar al registro ya existente
                {
                    Dic_Clave_Total_Deduccion[Clave_Percepcion_Deduccion] += (decimal)Fila_Percepcion_Deduccion["MONTO"];
                }
            }
        }

        // agregar registros a la tabla para la consulta
        // agregar registros comenzando con la tabla con más filas
        if (Dic_Clave_Nombre_Deduccion.Count > Dic_Clave_Nombre_Percepcion.Count)
        {
            Contador_Fila = 0;
            // agregar filas con deducciones
            foreach (KeyValuePair<string, decimal> Deduccion in Dic_Clave_Total_Deduccion)
            {
                Dr_Percepcion_Deduccion = Dt_Totales.NewRow();
                Dr_Percepcion_Deduccion["CLAVE_DEDUCCION"] = Deduccion.Key;
                Dic_Clave_Nombre_Deduccion.TryGetValue(Deduccion.Key, out Nombre_Clave);
                Dr_Percepcion_Deduccion["NOMBRE_DEDUCCION"] = Nombre_Clave;
                Dr_Percepcion_Deduccion["MONTO_DEDUCCION"] = Deduccion.Value;
                Dt_Totales.Rows.Add(Dr_Percepcion_Deduccion);
            }

            // agregar percepciones a las filas
            foreach (KeyValuePair<string, decimal> Percepcion in Dic_Clave_Total_Percepcion)
            {
                Dt_Totales.Rows[Contador_Fila]["CLAVE_PERCEPCION"] = Percepcion.Key;
                Dic_Clave_Nombre_Percepcion.TryGetValue(Percepcion.Key, out Nombre_Clave);
                Dt_Totales.Rows[Contador_Fila]["NOMBRE_PERCEPCION"] = Nombre_Clave;
                Dt_Totales.Rows[Contador_Fila]["MONTO_PERCEPCION"] = Percepcion.Value;
                Dt_Totales.AcceptChanges();
                Contador_Fila++;
            }
        }
        else  // si el número de registros de deducciones a agregar no es mayor al número de percepciones, agregar primero percepciones
        {
            Contador_Fila = 0;
            // agregar filas con PERCEPCIONES
            foreach (KeyValuePair<string, decimal> Percepcion in Dic_Clave_Total_Percepcion)
            {
                Dr_Percepcion_Deduccion = Dt_Totales.NewRow();
                Dr_Percepcion_Deduccion["CLAVE_PERCEPCION"] = Percepcion.Key;
                Dic_Clave_Nombre_Percepcion.TryGetValue(Percepcion.Key, out Nombre_Clave);
                Dr_Percepcion_Deduccion["NOMBRE_PERCEPCION"] = Nombre_Clave;
                Dr_Percepcion_Deduccion["MONTO_PERCEPCION"] = Percepcion.Value;
                Dt_Totales.Rows.Add(Dr_Percepcion_Deduccion);
            }

            // agregar DEDUCCIONES a las filas existentes
            foreach (KeyValuePair<string, decimal> Deduccion in Dic_Clave_Total_Deduccion)
            {
                Dt_Totales.Rows[Contador_Fila]["CLAVE_DEDUCCION"] = Deduccion.Key;
                Dic_Clave_Nombre_Deduccion.TryGetValue(Deduccion.Key, out Nombre_Clave);
                Dt_Totales.Rows[Contador_Fila]["NOMBRE_DEDUCCION"] = Nombre_Clave;
                Dt_Totales.Rows[Contador_Fila]["MONTO_DEDUCCION"] = Deduccion.Value;
                Dt_Totales.AcceptChanges();
                Contador_Fila++;
            }
        }

        return Dt_Totales;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Crear_Tabla_Percepciones_Deducciones
    /// DESCRIPCIÓN: Crea la Tabla con los campos para percepciones y deducciones
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 09-abr-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private DataTable Crear_Tabla_Percepciones_Deducciones()
    {
        DataTable Dt_Percepciones_Deducciones = new DataTable();

        Dt_Percepciones_Deducciones.Columns.Add("CLAVE_PERCEPCION", Type.GetType("System.String"));
        Dt_Percepciones_Deducciones.Columns.Add("NOMBRE_PERCEPCION", Type.GetType("System.String"));
        Dt_Percepciones_Deducciones.Columns.Add("CLAVE_DEPENDENCIA", Type.GetType("System.String"));
        Dt_Percepciones_Deducciones.Columns.Add("NOMBRE_DEPENDENCIA", Type.GetType("System.String"));
        Dt_Percepciones_Deducciones.Columns.Add("TIPO_NOMINA_ID", Type.GetType("System.Int32"));
        Dt_Percepciones_Deducciones.Columns.Add("NOMINA", Type.GetType("System.String"));
        Dt_Percepciones_Deducciones.Columns.Add("MONTO_PERCEPCION", Type.GetType("System.Decimal"));
        Dt_Percepciones_Deducciones.Columns.Add("CLAVE_DEDUCCION", Type.GetType("System.String"));
        Dt_Percepciones_Deducciones.Columns.Add("NOMBRE_DEDUCCION", Type.GetType("System.String"));
        Dt_Percepciones_Deducciones.Columns.Add("MONTO_DEDUCCION", Type.GetType("System.Decimal"));

        return Dt_Percepciones_Deducciones;
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
                    Cmb_Periodos_Catorcenales_Nomina.Items.Insert(0, new ListItem("< Seleccione >", ""));
                    Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = -1;

                    Validar_Periodos_Pago(Cmb_Periodos_Catorcenales_Nomina);
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

    /// *************************************************************************************
    /// NOMBRE: Consultar_Parametros_Reporte
    /// DESCRIPCIÓN: Forma una tabla con el nombre del empleado en la sesión
    /// PARÁMETROS: No Aplica
    /// USUARIO CREO: Roberto González Oseguera
    /// FECHA CREO: 05-abr-2012
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected DataTable Consultar_Parametros_Reporte()
    {
        DataTable Dt_Parametros = new DataTable();
        DataRow Dr_Parametro;

        Dt_Parametros.Columns.Add("Elaboro", typeof(string));
        Dt_Parametros.Columns.Add("Fecha_Periodo", typeof(string));
        Dt_Parametros.Columns.Add("Periodo", typeof(string));

        Dr_Parametro = Dt_Parametros.NewRow();
        Dr_Parametro["Elaboro"] = Cls_Sessiones.Nombre_Empleado.ToUpper();
        Dr_Parametro["Fecha_Periodo"] = "DEL " + Txt_Inicia_Catorcena.Text.ToUpper() + " AL " + Txt_Fin_Catorcena.Text.ToUpper();
        Dr_Parametro["Periodo"] = Cmb_Calendario_Nomina.SelectedItem.Text + "-" + Cmb_Periodos_Catorcenales_Nomina.SelectedItem.Text;

        Dt_Parametros.Rows.Add(Dr_Parametro);

        return Dt_Parametros;
    }
    #endregion (Consultar)

    #region (Consultas Combos)
    /// *************************************************************************************
    /// NOMBRE: Consultar_Tipos_Nominas
    /// 
    /// DESCRIPCIÓN: Consulta los tipos de nómina que se encuantran dadas de alta 
    ///              actualmente en sistema.
    ///              
    /// PARÁMETROS: No Aplica
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 10:52 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Consultar_Tipos_Nominas()
    {
        Cls_Cat_Tipos_Nominas_Negocio Obj_Tipos_Nominas = new Cls_Cat_Tipos_Nominas_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Tipos_Nominas = null;//Variable que almacena la lista de tipos de nominas. 
        try
        {
            Dt_Tipos_Nominas = Obj_Tipos_Nominas.Consulta_Tipos_Nominas();//Consulta los tipos de nominas.
            Cargar_Combos(Cmb_Busqueda_Tipo_Nomina, Dt_Tipos_Nominas, Cat_Nom_Tipos_Nominas.Campo_Nomina,
                Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID, 0);//Carga el combo de tipos de nómina.
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los tipos de nomina que existen actualemte en sistema. Error: [" + Ex.Message + "]");
        }
    }

    /// *************************************************************************************
    /// NOMBRE: Consultar_Unidades_Responsables
    /// 
    /// DESCRIPCIÓN: Consulta las Unidades responsables que se encuentran registrados actualmente
    ///              en sistema.
    ///              
    /// PARÁMETROS: No Aplica.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 11:12 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Consultar_Unidades_Responsables()
    {
        Cls_Cat_Dependencias_Negocio Obj_Unidades_Responsables = new Cls_Cat_Dependencias_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Unidades_Responsables = null;//Variable que almacena una lista de las unidades resposables en sistema.

        try
        {
            Dt_Unidades_Responsables = Obj_Unidades_Responsables.Consulta_Dependencias();//Consulta las unidades responsables registradas en  sistema.
            Cargar_Combos(Cmb_Busqueda_Unidad_Responsable, Dt_Unidades_Responsables, Cat_Dependencias.Campo_Nombre,
                Cat_Dependencias.Campo_Dependencia_ID, 0);//Se carga el control que almacena las unidades responsables.
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las unidades responsables registradas en sistema. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************
    /// NOMBRE: Cargar_Combos
    /// 
    /// DESCRIPCIÓN: Carga cualquier ctlr DropDownList que se le pase como parámetro.
    ///              
    /// PARÁMETROS: Combo.- Ctlr que se va a cargar.
    ///             Dt_Datos.- Informacion que se cargara en el combo.
    ///             Text.- Texto que será la parte visible de la lista de tipos de nómina.
    ///             Value.- Valor que será el que almacenará el elemnto seleccionado.
    ///             Index.- Indice el cuál será el que se mostrara inicialmente. 
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 11:12 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    private void Cargar_Combos(DropDownList Combo, DataTable Dt_Datos, String Text, String Value, Int32 Index)
    {
        try
        {
            Combo.DataSource = Dt_Datos;
            Combo.DataTextField = Text;
            Combo.DataValueField = Value;
            Combo.DataBind();
            Combo.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Combo.SelectedIndex = Index;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cargar el Ctlr de Tipo DropDownList. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Metodos Validacion)

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
                    }
                }
            }
        }
    }

    /// *************************************************************************************
    /// NOMBRE: Validar_Datos_Formulario
    /// 
    /// DESCRIPCIÓN: Valida que se hallan ingresado los datos requeridos para realizar la 
    ///              operación.
    ///              
    /// PARÁMETROS: No Aplica
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 11:34 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected Boolean Validar_Datos_Formulario()
    {
        Lbl_Mensaje_Error.Text = "Es necesario ingresar: <br />";
        Boolean Es_Valido = true;

        try
        {
            if (Cmb_Calendario_Nomina.SelectedIndex <= 0)
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;+ El año de la nómina es un dato requerido. <br />";
                Es_Valido = false;
            }

            if (Cmb_Periodos_Catorcenales_Nomina.SelectedIndex <= 0)
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;+ El periodo es un dato requerido. <br />";
                Es_Valido = false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al validar la información del formulario. Error: [" + Ex.Message + "]");
        }
        return Es_Valido;
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: IsNumeric
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 3/Mayo/2011 12:20p.m.
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

    #endregion (Metodos Validacion)

    #region (Reportes)
    /// *************************************************************************************
    /// NOMBRE: Generar_Reporte
    /// 
    /// DESCRIPCIÓN: Método que invoca la generación del reporte.
    ///              
    /// PARÁMETROS: Nombre_Plantilla_Reporte.- Nombre del archivo del Crystal Report.
    ///             Nombre_Reporte_Generar.- Nombre que tendrá el reporte generado.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 18:15 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Generar_Reporte(ref DataSet Ds_Datos, String Nombre_Plantilla_Reporte, String Nombre_Reporte_Generar)
    {
        ReportDocument Reporte = new ReportDocument();//Variable de tipo reporte.
        String Ruta = String.Empty;//Variable que almacenara la ruta del archivo del crystal report. 

        try
        {
            Ruta = @Server.MapPath("../Rpt/Nomina/" + Nombre_Plantilla_Reporte);
            Reporte.Load(Ruta);

            if (Ds_Datos is DataSet)
            {
                if (Ds_Datos.Tables.Count > 0)
                {
                    Reporte.SetDataSource(Ds_Datos);
                    Exportar_Reporte_PDF(Reporte, Nombre_Reporte_Generar);
                    Mostrar_Reporte(Nombre_Reporte_Generar);
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************
    /// NOMBRE: Exportar_Reporte_PDF
    /// 
    /// DESCRIPCIÓN: Método que guarda el reporte generado en formato PDF en la ruta
    ///              especificada.
    ///              
    /// PARÁMETROS: Reporte.- Objeto de tipo documento que contiene el reporte a guardar.
    ///             Nombre_Reporte.- Nombre que se le dará al reporte.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 18:19 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Exportar_Reporte_PDF(ReportDocument Reporte, String Nombre_Reporte)
    {
        ExportOptions Opciones_Exportacion = new ExportOptions();
        DiskFileDestinationOptions Direccion_Guardar_Disco = new DiskFileDestinationOptions();
        PdfRtfWordFormatOptions Opciones_Formato_PDF = new PdfRtfWordFormatOptions();

        try
        {
            if (Reporte is ReportDocument)
            {
                Direccion_Guardar_Disco.DiskFileName = @Server.MapPath("../../Reporte/" + Nombre_Reporte);
                Opciones_Exportacion.ExportDestinationOptions = Direccion_Guardar_Disco;
                Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
                Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
                Reporte.Export(Opciones_Exportacion);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al exportar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************
    /// NOMBRE: Mostrar_Reporte
    /// 
    /// DESCRIPCIÓN: Muestra el reporte en pantalla.
    ///              
    /// PARÁMETROS: Nombre_Reporte.- Nombre que tiene el reporte que se mostrara en pantalla.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 18:20 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Mostrar_Reporte(String Nombre_Reporte)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            Pagina = Pagina + Nombre_Reporte;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt_Empleados",
                "window.open('" + Pagina + "', 'Busqueda_Empleados','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Exportar_Reporte
    /// DESCRIPCIÓN: Genera el reporte de Crystal con los datos proporcionados en el DataTable 
    /// PARÁMETROS:
    /// 		1. Ds_Reporte: Dataset con datos a imprimir
    /// 		2. Nombre_Reporte: Nombre del archivo de reporte .rpt
    /// 		3. Nombre_Archivo: Nombre del archivo a generar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 04-sep-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Exportar_Reporte(DataSet Ds_Reporte, String Nombre_Reporte, String Nombre_Archivo, String Extension_Archivo, ExportFormatType Formato)
    {
        ReportDocument Reporte = new ReportDocument();
        String Ruta = Server.MapPath("../Rpt/Nomina/" + Nombre_Reporte);

        try
        {
            Reporte.Load(Ruta);
            Reporte.SetDataSource(Ds_Reporte);
        }
        catch
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "No se pudo cargar el reporte";
        }

        String Archivo_Reporte = Nombre_Archivo + "." + Extension_Archivo;  // formar el nombre del archivo a generar 
        try
        {
            ExportOptions Export_Options_Calculo = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options_Calculo = new DiskFileDestinationOptions();
            Disk_File_Destination_Options_Calculo.DiskFileName = Server.MapPath("../../Reporte/" + Archivo_Reporte);
            Export_Options_Calculo.ExportDestinationOptions = Disk_File_Destination_Options_Calculo;
            Export_Options_Calculo.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options_Calculo.ExportFormatType = Formato;
            Reporte.Export(Export_Options_Calculo);

            if (Formato == ExportFormatType.Excel)
            {
                Mostrar_Excel(Server.MapPath("../../Reporte/" + Archivo_Reporte), "application/vnd.ms-excel");
            }
            else if (Formato == ExportFormatType.WordForWindows)
            {
                Mostrar_Excel(Server.MapPath("../../Reporte/" + Archivo_Reporte), "application/vnd.ms-word");
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    /// *************************************************************************************
    /// NOMBRE: Mostrar_Excel
    /// 
    /// DESCRIPCIÓN: Muestra el reporte en excel.
    ///              
    /// PARÁMETROS: No Aplica
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 10/Diciembre/2011.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    private void Mostrar_Excel(string Ruta_Archivo, string Contenido)
    {
        try
        {
            System.IO.FileInfo ArchivoExcel = new System.IO.FileInfo(Ruta_Archivo);
            if (ArchivoExcel.Exists)
            {
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = Contenido;
                Response.AddHeader("Content-Disposition", "attachment;filename=" + ArchivoExcel.Name);
                Response.Charset = "UTF-8";
                Response.ContentEncoding = Encoding.Default;
                Response.WriteFile(ArchivoExcel.FullName);
                Response.End();
            }
        }
        catch (Exception Ex)
        {
            //// Response.End(); siempre genera una excepción (http://support.microsoft.com/kb/312629/EN-US/)
            throw new Exception("Error al mostrar el reporte en excel. Error: [" + Ex.Message + "]");
        }
    }

    #endregion

    #endregion

    #region (Eventos)

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
        }
        else
        {
            Cmb_Periodos_Catorcenales_Nomina.DataSource = new DataTable();
            Cmb_Periodos_Catorcenales_Nomina.DataBind();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Periodo_SelectedIndexChanged
    ///DESCRIPCIÓN: Carga la fecha de inicio y fin del periodo catorcenal seleccionado.
    ///
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Periodo_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nomina = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Ope_Nom_Pestamos_Negocio Prestamos = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Detalles_Nomina = null;//Variable que almacenra los detalles del periodo seleccionado.
        DateTime Fecha_Inicio = new DateTime();//Fecha de inicio de la catorcena a generar la nómina.
        DateTime Fecha_Fin = new DateTime();//Fecha de fin de la catorcena a generar la nómina.

        try
        {
            if (Cmb_Calendario_Nomina.SelectedIndex > 0)
            {
                Prestamos.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();

                Prestamos.P_No_Nomina = Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedItem.Text.Trim());
                Dt_Detalles_Nomina = Prestamos.Consultar_Fechas_Periodo();

                if (Dt_Detalles_Nomina != null)
                {
                    if (Dt_Detalles_Nomina.Rows.Count > 0)
                    {
                        Fecha_Inicio = Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString());
                        Fecha_Fin = Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString());

                        Txt_Inicia_Catorcena.Text = string.Format("{0:dd/MMM/yyyy}", Fecha_Inicio);
                        Txt_Fin_Catorcena.Text = string.Format("{0:dd/MMM/yyyy}", Fecha_Fin);
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    #region (Botones)
    /// *************************************************************************************
    /// NOMBRE: Btn_Generar_Reporte_Click
    /// 
    /// DESCRIPCIÓN: Consulta los Empleados de acuerdo a los filtros establecidos para
    ///              ejecutar la búsqueda, Genera y muestra el reporte. 
    ///              
    /// PARÁMETROS: No Aplica
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 12:19 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Btn_Generar_Reporte_Click(object sender, ImageClickEventArgs e)
    {
        DataSet Ds_Reporte;
        DataTable Dt_Percepciones;
        DataTable Dt_Parametros;
        DataTable Dt_Reporte;
        DataTable Dt_Totales;

        Lbl_Mensaje_Error.Text = "";

        try
        {
            if (Validar_Datos_Formulario())
            {
                Ds_Reporte = new DataSet();
                Dt_Percepciones = Consultar_Nomina(null);
                Dt_Parametros = Consultar_Parametros_Reporte();

                Dt_Reporte = Formar_Tabla_Percepciones_Deducciones(Dt_Percepciones);
                Dt_Totales = Calcular_Totales_Generales(Dt_Percepciones);

                Dt_Reporte.TableName = "Dt_Percepciones_Deducciones";
                Dt_Parametros.TableName = "Dt_Parametros";
                Dt_Totales.TableName = "Dt_Totales";

                Ds_Reporte.Tables.Add(Dt_Reporte.Copy());
                Ds_Reporte.Tables.Add(Dt_Parametros.Copy());
                Ds_Reporte.Tables.Add(Dt_Totales.Copy());

                //Se llama al método que ejecuta la operación de generar el reporte.
                Generar_Reporte(ref Ds_Reporte, "Cr_Rpt_Nom_Nomina_Por_Dependencia.rpt", "Reporte_Nomina_Por_Dependencia" + Session.SessionID + ".pdf");
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    /// *************************************************************************************
    /// NOMBRE: Btn_Generar_Reporte_Excel_Click
    /// 
    /// DESCRIPCIÓN: Consulta los Empleados de acuerdo a los filtros establecidos para
    ///              ejecutar la búsqueda, Genera y muestra el reporte. 
    ///              
    /// PARÁMETROS: No Aplica
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 10/Diciembre/2011.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Btn_Generar_Reporte_Excel_Click(object sender, ImageClickEventArgs e)
    {
        DataSet Ds_Reporte;
        DataTable Dt_Percepciones;
        DataTable Dt_Parametros;
        DataTable Dt_Reporte;

        Lbl_Mensaje_Error.Text = "";

        try
        {
            if (Validar_Datos_Formulario())
            {
                Ds_Reporte = new DataSet();
                Dt_Percepciones = Consultar_Nomina(null);
                Dt_Parametros = Consultar_Parametros_Reporte();

                Dt_Reporte = Formar_Tabla_Percepciones_Deducciones(Dt_Percepciones);

                Dt_Reporte.TableName = "Dt_Percepciones_Deducciones";
                Dt_Parametros.TableName = "Dt_Parametros";
                Ds_Reporte.Tables.Add(Dt_Reporte.Copy());
                Ds_Reporte.Tables.Add(Dt_Parametros.Copy());

                //Se llama al método que ejecuta la operación de generar el reporte.
                Exportar_Reporte(Ds_Reporte, "Cr_Rpt_Nom_Nomina_Por_Dependencia.rpt", "Reporte_Nomina_Por_Dependencia" + Session.SessionID, "xls", ExportFormatType.Excel);
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }
        }
        catch (System.Threading.ThreadAbortException)
        { }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte Catálogo de empleados. Error: [" + Ex.Message + "]");
        }
    }

    /// *************************************************************************************
    /// NOMBRE: Btn_Generar_Reporte_Word_Click
    /// 
    /// DESCRIPCIÓN: Consulta los Empleados de acuerdo a los filtros establecidos para
    ///              ejecutar la búsqueda, Genera y ofrece para descarga reporte en formato MS Word. 
    /// PARÁMETROS: No Aplica
    /// USUARIO CREO: Roberto González Oseguera
    /// FECHA CREO: 05-abr-2012
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Btn_Generar_Reporte_Word_Click(object sender, ImageClickEventArgs e)
    {
        DataSet Ds_Reporte;
        DataTable Dt_Percepciones;
        DataTable Dt_Parametros;
        DataTable Dt_Reporte;

        Lbl_Mensaje_Error.Text = "";

        try
        {
            if (Validar_Datos_Formulario())
            {
                Ds_Reporte = new DataSet();
                Dt_Percepciones = Consultar_Nomina(null);
                Dt_Parametros = Consultar_Parametros_Reporte();

                Dt_Reporte = Formar_Tabla_Percepciones_Deducciones(Dt_Percepciones);

                Dt_Reporte.TableName = "Dt_Percepciones_Deducciones";
                Dt_Parametros.TableName = "Dt_Parametros";
                Ds_Reporte.Tables.Add(Dt_Reporte.Copy());
                Ds_Reporte.Tables.Add(Dt_Parametros.Copy());

                //Se llama al método que ejecuta la operación de generar el reporte.
                Exportar_Reporte(Ds_Reporte, "Cr_Rpt_Nom_Nomina_Por_Dependencia.rpt", "Reporte_Nomina_Por_Dependencia" + Session.SessionID, "doc", ExportFormatType.WordForWindows);
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }
        }
        catch (System.Threading.ThreadAbortException)
        { }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte Catálogo de empleados. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #endregion
}
