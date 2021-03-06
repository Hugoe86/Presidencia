﻿using System;
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
using Presidencia.Cat_Nom_Percepciones_Deducciones_Opcional.Negocios;
using Presidencia.Empleados.Negocios;
using System.Text.RegularExpressions;
using Presidencia.Dependencias.Negocios;
using Presidencia.Faltas_Empleado.Negocio;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Prestamos.Negocio;
using System.Collections.Generic;
using System.IO;
using AjaxControlToolkit;
using System.Reflection;
using System.Text;
using Presidencia.Captura_Masiva_Perc_Deduc_Fijas.Negocio;

public partial class paginas_Nomina_Frm_Ope_Captura_Masiva_Percepciones_Fijas : System.Web.UI.Page
{
    #region (Page_Load)
    protected void Page_Load(object sender, EventArgs e)
    {
        //Estilo para agregar al boton que cuando el cursor este sobre de el.
        String Estilo_Raton_Sobre = "this.style.backgroundColor='#DFE8F6';this.style.cursor='hand';this.style.color='DarkBlue';" +
            "this.style.borderStyle='none';this.style.borderColor='Silver';";

        try
        {
            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Configuracion_Inicial();//Habilita la configuracion inicial de los controles de la pagina.
            }

        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
        Lbl_Mensaje_Error.Text = "";
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
    }
    #endregion

    #region (Metodos)

    #region (Metodos Generales)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Inicial
    ///DESCRIPCIÓN: Configuracion Inicial de los controles del formulario.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 28/Noviembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Inicial()
    {
        Limpiar_Controles();
        Habilitar_Controles("Inicial");
        Consultar_Calendarios_Nomina();
        RBtn_Tipo_Percepcion_Deduccion.SelectedIndex = 0;
        Consultar_Percepciones_Fijas();
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Ctlr
    /// DESCRIPCION : Limpia los Controles de la pagina.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 29/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            Grid_Empleados.Columns[0].Visible = true;
            Grid_Empleados.SelectedIndex = -1;
            Grid_Empleados.DataSource = new DataTable();
            Grid_Empleados.DataBind();
            Grid_Empleados.Columns[0].Visible = false;

            Cmb_Calendario_Nomina.SelectedIndex = -1;
            Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = -1;
            Cmb_Percepciones.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al limpiar los controles del formulario. Error: [" + Ex.Message.ToString() + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///               para a siguiente operación
    /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
    ///                          si es una alta, modificacion
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 29/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado;

        try
        {
            Habilitado = false;
            switch (Operacion)
            {
                case "Inicial":
                    Habilitado = false;
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Nuevo.Visible = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";

                    Lbl_Mensaje_Error.Text = "";
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;

                    break;
                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    if (Session["Dt_Empleados"] != null) Session.Remove("Dt_Empleados");

                    break;
                case "Modificar":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";

                    break;
            }

            Grid_Empleados.Enabled = Habilitado;
            Cmb_Calendario_Nomina.Enabled = Habilitado;
            Cmb_Periodos_Catorcenales_Nomina.Enabled = Habilitado;
            AFU_Cargar_Archivo_Fijos.Enabled = Habilitado;
            Btn_Cargar_Empleados.Enabled = Habilitado;
            Btn_Limpiar_Empleados.Enabled = Habilitado;
            RBtn_Tipo_Percepcion_Deduccion.Enabled = Habilitado;
            Cmb_Percepciones.Enabled = Habilitado;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al Habilitar los Controles del formulario. Error:[" + ex.Message.ToString() + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos_Asignacion_Deducciones_Variables
    /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 29/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos_Asignacion_Deducciones_Variables()
    {
        Boolean Datos_Validos = true;
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

        if (Cmb_Calendario_Nomina.SelectedIndex <= 0) {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione un calendario de nómina. <br />";
            Datos_Validos = false;
        }

        if (Cmb_Periodos_Catorcenales_Nomina.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione un periodo de pago. <br />";
            Datos_Validos = false;
        }

        if (Cmb_Percepciones.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione una percepción ó deducción. <br />";
            Datos_Validos = false;
        }

        if (RBtn_Tipo_Percepcion_Deduccion.SelectedIndex < 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione un tipo de concepto. <br />";
            Datos_Validos = false;
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
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Formato_Fecha
    /// DESCRIPCION : Valida el formato de las fechas.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 23/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Formato_Fecha(String Fecha)
    {
        String Cadena_Fecha = @"^(([0-9])|([0-2][0-9])|([3][0-1]))\/(ene|feb|mar|abr|may|jun|jul|ago|sep|oct|nov|dic)\/\d{4}$";
        if (Fecha != null)
        {
            return Regex.IsMatch(Fecha, Cadena_Fecha);
        }
        else
        {
            return false;
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Calendarios_Nomina
    /// DESCRIPCION : 
    /// 
    /// PARAMETROS:
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 21/Febrero/2011
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
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Formato_Fecha_Calendario_Nomina
    /// DESCRIPCION : Crea el DataTable con la consulta de las nomina vigentes en el 
    /// sistema.
    /// PARAMETROS: Dt_Calendario_Nominas.- Lista de las nominas vigentes actualmente 
    ///             en el sistema.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 01/Diciembre/2010
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
    ///FECHA_CREO: 01/Diciembre/2010
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
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Periodos_Pago
    ///DESCRIPCIÓN: Valida que el empleado solo puedan comenzar a descontar la deduccion 
    ///a partir del periodo actual.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Diciembre/2010
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

                        //if (Fecha_Fin >= Fecha_Actual)
                        //{
                        //    Elemento.Enabled = true;
                        //}
                        //else
                        //{
                        //    Elemento.Enabled = false;
                        //}
                    }
                }
            }
        }
    }
    ///********************************************************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Leer_Archivo_Obtener_Historial_Nomina_Generada
    ///
    ///DESCRIPCIÓN: Lee los empleados del archivo para crear una tabla con la información del mismo.
    ///
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*********************************************************************************************************************************************************
    protected DataTable Leer_Archivo_Canceptos_Variables(String Ruta, String Nombre_Archivo, String Extencion)
    {
        StreamReader Lector = null;//Variable que leerá el archivo que almacena los registros que fueron afectados al Generar la Nómina que se desea Regenerar.
        String Linea_Leida = "";//Variable que leerá cada línea del archivo. para su tratamiento como cadena.
        String[] Columnas;//Variable que contendrá todos lo campos que se guardaron por cada registro que se almaceno en cada línea del archivo.
        DataRow Renglon_Insertar = null;//Variable que almacenrá cada renglon contruido, y que se insertara en una respectiva tabla
        DataTable Dt_Empleados = new DataTable("VARIABLES");//Tabla [PRESTAMOS] con los registros que fueron afectados al Generar la Nómina.

        try
        {


            //Se válida que exista un archivo.
            if (File.Exists(@"" + (Ruta + Nombre_Archivo + Extencion)))
            {
                //Si existe el archivo el siguiente paso, es crear el objeto que nos ayudará a leer el archivo.
                Lector = new StreamReader(@"" + (Ruta + Nombre_Archivo + Extencion));

                //Se crea la estructura de la tabla que alamcenará los registros que fueron afectados 
                Dt_Empleados.Columns.Add(Cat_Empleados.Campo_Empleado_ID, typeof(String));
                Dt_Empleados.Columns.Add(Cat_Empleados.Campo_Nombre, typeof(String));
                Dt_Empleados.Columns.Add("CANTIDAD", typeof(String));
                Dt_Empleados.Columns.Add(Cat_Dependencias.Campo_Dependencia_ID, typeof(String));

                //Leer el archivo, hasta llegar al final del mismo.
                while ((Linea_Leida = Lector.ReadLine()) != null)
                {
                    Renglon_Insertar = null;//Limpiamos el renglon, para que este disponible para cuando se realize el siguiente registro.
                    Linea_Leida = Linea_Leida.Replace("[", "");//Eliminamos el carácter [ de la cadena leida.
                    Linea_Leida = Linea_Leida.Replace("]", "");//Eliminamos el carácter ] de la cadena leida.
                    Columnas = Linea_Leida.Split(new Char[] { ',' });//Obtenemos un arreglo con un número de elementos igual, al número de

                    if (Columnas.Length == 4)
                    {

                        Renglon_Insertar = Dt_Empleados.NewRow();
                        Renglon_Insertar[Cat_Empleados.Campo_Empleado_ID] = Consultar_Empleado_ID(Columnas[0]);
                        Renglon_Insertar[Cat_Empleados.Campo_Nombre] = Columnas[1];
                        Renglon_Insertar["CANTIDAD"] = Columnas[2];
                        Renglon_Insertar[Cat_Dependencias.Campo_Dependencia_ID] = Columnas[3];
                        Dt_Empleados.Rows.Add(Renglon_Insertar);
                    }
                }
                Lector.Close();//Cerramos el lector del archivo.
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al escribir el Archivo " + Nombre_Archivo + ". Error: [" + Ex.Message + "]");
        }
        return Dt_Empleados;
    }
    /// ****************************************************************************************************************
    /// Nombre: Consultar_Empleado_ID
    /// 
    /// Descripción: Consulta la información del empleado y obtiene el identifacador del empleado.
    /// 
    /// Parámetros: No_Empleado.- Es el identificador del empleado para uso de recursos humanos.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete
    /// Fecha Creo: 6/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// ****************************************************************************************************************
    protected String Consultar_Empleado_ID(String No_Empleado)
    {
        Cls_Cat_Empleados_Negocios Obj_Empelado = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa d enegocios.
        DataTable Dt_Empleado = null;//Variable que guarda la lista de empleados que se consulto del archivo.
        String Empleado_ID = String.Empty;//Variable que almacenara el identificador del empleado.

        try
        {
            Obj_Empelado.P_No_Empleado = No_Empleado;
            Dt_Empleado = Obj_Empelado.Consulta_Empleados_General();

            if (Dt_Empleado is DataTable)
            {
                if (Dt_Empleado.Rows.Count > 0)
                {
                    foreach (DataRow EMPLEADO in Dt_Empleado.Rows)
                    {
                        if (EMPLEADO is DataRow)
                        {
                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Empleado_ID].ToString().Trim()))
                                Empleado_ID = EMPLEADO[Cat_Empleados.Campo_Empleado_ID].ToString().Trim();
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los ");
        }
        return Empleado_ID;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Remover_Sesiones_Control_Carga_Archivos
    /// DESCRIPCION : Remueve la sesion del Ctlr AsyncFileUpload que mantiene al archivo
    /// en memoria.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 27/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Remover_Sesiones_Control_Carga_Archivos(String Client_ID)
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
                if (key.Contains(Client_ID))
                {
                    currentContext.Session.Remove(key);
                    break;
                }
            }
        }
    }
    /// *****************************************************************************************************
    /// Nombre: Leer_Archivo_Excel
    /// 
    /// Descripción: Lee un archivo de excel y retorna una tabla con las columnas definidas en el doc.
    /// 
    /// Parámetros: Ruta.- Parámetro que almacena la ruta del archivo.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 06/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *****************************************************************************************************
    public DataTable Leer_Archivo_Excel(String Ruta)
    {
        Int32 Contador_Columnas = 1;
        Int32 Contador_Filas = 0;
        DataTable Dt_Empleados = new DataTable();
        DataRow[] Empleados = new DataRow[] { };

        //Declaro las variables necesarias
        Microsoft.Office.Interop.Excel._Application xlApp;
        Microsoft.Office.Interop.Excel._Workbook xlLibro;
        Microsoft.Office.Interop.Excel._Worksheet xlHoja1;
        Microsoft.Office.Interop.Excel.Sheets xlHojas;
        //asigno la ruta dónde se encuentra el archivo
        String Ruta_Archivo = Ruta;
        //inicializo la variable xlApp (referente a la aplicación)
        xlApp = new Microsoft.Office.Interop.Excel.Application();
        //Muestra la aplicación Excel si está en true
        xlApp.Visible = false;
        //Abrimos el libro a leer (documento excel)
        xlLibro = xlApp.Workbooks.Open(Ruta_Archivo, Missing.Value, Missing.Value,
            Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
            Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
            Missing.Value, Missing.Value);

        try
        {
            //Asignamos las hojas
            xlHojas = xlLibro.Sheets;
            //Asignamos la hoja con la que queremos trabajar: 
            xlHoja1 = (Microsoft.Office.Interop.Excel._Worksheet)xlHojas["Hoja1"];

            //recorremos las celdas que queremos y sacamos los datos 
            while (!((string)xlHoja1.get_Range("A" + Contador_Columnas, Missing.Value).Text).Trim().Equals(""))
            {
                if (Contador_Filas == 1)
                {
                    HTxt_Referencia.Value = (string)xlHoja1.get_Range("A" + Contador_Columnas, Missing.Value).Text;
                }

                if (Contador_Filas == 2)
                {
                    //Se crea la estructura de la tabla que alamcenará los registros que fueron afectados 
                    Dt_Empleados.Columns.Add(Cat_Empleados.Campo_Empleado_ID, typeof(String));
                    Dt_Empleados.Columns.Add("NOMBRE", typeof(String));
                    Dt_Empleados.Columns.Add("CANTIDAD", typeof(String));
                }
                else if(Contador_Filas > 2)
                {
                    string No_Empleado = Consultar_Empleado_ID(((string)xlHoja1.get_Range("A" + Contador_Columnas, Missing.Value).Text));

                    string Nombre = "[" + String.Format("{0:000000}", Convert.ToInt32((string)xlHoja1.get_Range("A" + Contador_Columnas, Missing.Value).Text)) + "] -- " +
                        (string)xlHoja1.get_Range("B" + Contador_Columnas, Missing.Value).Text;

                    string Cantidad = (string)xlHoja1.get_Range("C" + Contador_Columnas, Missing.Value).Text;

                    if (!String.IsNullOrEmpty(No_Empleado))
                    {
                        if (Dt_Empleados.Rows.Count > 0)
                            Empleados = Dt_Empleados.Select(Cat_Empleados.Campo_Empleado_ID + "=" + No_Empleado);

                        if (Empleados.Length <= 0)
                        {
                            DataRow Dr_Insertar = Dt_Empleados.NewRow();
                            Dr_Insertar[0] = No_Empleado;
                            Dr_Insertar[1] = Nombre;
                            Dr_Insertar[2] = String.Format("{0:c}", Convert.ToDouble(Cantidad));
                            Dt_Empleados.Rows.Add(Dr_Insertar);
                        }
                        else
                        {
                            Lbl_Mensaje_Error.Text = "Hay empleados duplicados.";
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;

                            Dt_Empleados = new DataTable();
                            break;
                        }
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Text = "Hay empleados que no existen en el sistema.";
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;

                        Dt_Empleados = new DataTable();
                        break;
                    }
                }
                Contador_Columnas++;
                Contador_Filas++;
            }

            return Dt_Empleados;
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = "Hay información en el archivo que no viene de forma correcta.";
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
        finally
        {
            //Cerrar el Libro
            xlLibro.Close(false, Missing.Value, Missing.Value);
            //Cerrar la Aplicación
            xlApp.Quit();
        }
        return Dt_Empleados;
    }
    /// ***************************************************************************************************
    /// Nombre: Quitar_Caracteres_Cantidad
    /// 
    /// Descripción: Recorrecorre todas las celdas de la tabla y revisa si en alguna celda hay algun 
    ///              caracter de simbolo de pesos y lo elimina.
    /// 
    /// Parámetros: Dt_Perc_Deduc_Empl.- Tabla la cuál se recorrera en para eliminar los caracteres
    ///             de simbolo de pesos.
    /// 
    /// Usuario creo: Juan ALberto Hernández Negrete.
    /// Fecha Creó: 14/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ***************************************************************************************************
    protected DataTable Quitar_Caracteres_Cantidad(DataTable Dt_Perc_Deduc_Empl)
    {
        try
        {
            if (Dt_Perc_Deduc_Empl is DataTable)
            {
                if (Dt_Perc_Deduc_Empl.Rows.Count > 0)
                {
                    foreach (DataRow FILA in Dt_Perc_Deduc_Empl.Rows)
                    {
                        if (FILA is DataRow)
                        {
                            if (Dt_Perc_Deduc_Empl.Columns.Count > 0)
                            {
                                foreach (DataColumn COLUMNA in Dt_Perc_Deduc_Empl.Columns)
                                {
                                    if (COLUMNA is DataColumn)
                                    {
                                        FILA[COLUMNA.ColumnName.Trim()] = FILA[COLUMNA.ColumnName.Trim()].ToString().Trim().Replace("$", "");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al quitar los signos de pesos. Error: [" + Ex.Message + "]");
        }
        return Dt_Perc_Deduc_Empl;
    }
    /// *************************************************************************************
    /// Nombre: Copiar_Archivo_Servidor
    /// 
    /// Descripción: Elimina y vuelve a copiar el archivo.
    /// 
    /// Parámetros: Ctrl que se uso para cargar el archivo.
    /// 
    /// Usuario Creo: Juan Alberto Hernandez Negrete.
    /// Fecha Creo: 06/Diciembre/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificacion:
    /// *************************************************************************************
    private String Copiar_Archivo_Servidor(AsyncFileUpload Ctlr_AFU_Archivo)
    {
        String Extension = String.Empty;
        String Ruta = String.Empty;
        String Full_Path = String.Empty;

        try
        {
            if (Ctlr_AFU_Archivo is AsyncFileUpload)
            {
                if (Ctlr_AFU_Archivo.HasFile)
                {
                    Extension = Path.GetExtension(Ctlr_AFU_Archivo.PostedFile.FileName);//Obtenemos la extensión del archivo.
                    Ruta = Server.MapPath("Archivos");
                    Full_Path = Ruta + "/Archivo" + Extension;

                    if (File.Exists(Full_Path))
                    {
                        File.Delete(Full_Path);
                    }

                    Ctlr_AFU_Archivo.SaveAs(Full_Path);
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al copiar los archivos al servidor. Error: [" + Ex.Message + "]");
        }
        return Full_Path;
    }
    #endregion

    #region (Guardar)
    protected void Guardar_Percepciones_Deducciones_Fijas()
    {
        Cls_Ope_Nom_Cap_Masiva_Perc_Dedu_Fijas_Negocio Obj_Captura_Masiva_Perc_Dedu_Fijas =
            new Cls_Ope_Nom_Cap_Masiva_Perc_Dedu_Fijas_Negocio();
        try
        {
            Obj_Captura_Masiva_Perc_Dedu_Fijas.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
            Obj_Captura_Masiva_Perc_Dedu_Fijas.P_No_Nomina = Cmb_Periodos_Catorcenales_Nomina.SelectedValue.Trim();
            Obj_Captura_Masiva_Perc_Dedu_Fijas.P_Dt_Empleados = Quitar_Caracteres_Cantidad((DataTable)Session["Dt_Empleados"]);
            Obj_Captura_Masiva_Perc_Dedu_Fijas.P_Percepcion_Deduccion_ID = Cmb_Percepciones.SelectedValue.Trim();
            Obj_Captura_Masiva_Perc_Dedu_Fijas.P_Referencia = HTxt_Referencia.Value.Trim();

            if (Obj_Captura_Masiva_Perc_Dedu_Fijas.Alta_Percepciones_Deducciones_Fijas()) {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Guardar Percepciones y Deducciones Fijas",
                    "alert('Operación Completa');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al guardar las percepciones y deducciones de forma masiva. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Metodos Cargar Combos)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Juntar_Clave_Percepcion_Deduccion
    /// 
    /// DESCRIPCION : Junta la clave de la percepcion y deduccion con el nombre.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 07/Julio/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected DataTable Juntar_Clave_Percepcion_Deduccion(DataTable Dt_Percepciones_Deducciones)
    {
        try
        {
            if (Dt_Percepciones_Deducciones is DataTable)
            {
                if (Dt_Percepciones_Deducciones.Rows.Count > 0)
                {
                    foreach (DataRow PERCEPCION_DEDUCCION in Dt_Percepciones_Deducciones.Rows)
                    {
                        PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Nombre] =
                            "[" + PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Clave] + "] -- " +
                            PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Nombre];
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al juntar el nombre de la percepcion deduccion con la clave. Error: [" + Ex.Message + "]");
        }
        return Dt_Percepciones_Deducciones;
    }

    protected void Consultar_Percepciones_Fijas()
    {
        Cls_Cat_Nom_Percepciones_Deducciones_Business Obj_Percepciones_Fijas =
            new Cls_Cat_Nom_Percepciones_Deducciones_Business();
        DataTable Dt_Percepciones_Fijas = null;

        try
        {
            Obj_Percepciones_Fijas.P_TIPO = RBtn_Tipo_Percepcion_Deduccion.SelectedItem.Text.Trim();
            Obj_Percepciones_Fijas.P_ESTATUS = "ACTIVO";
            Obj_Percepciones_Fijas.P_TIPO_ASIGNACION = "FIJA";
            Dt_Percepciones_Fijas = Obj_Percepciones_Fijas.Consultar_Percepciones_Deducciones_General();

            Dt_Percepciones_Fijas = Juntar_Clave_Percepcion_Deduccion(Dt_Percepciones_Fijas);
            Cmb_Percepciones.DataSource = Dt_Percepciones_Fijas;
            Cmb_Percepciones.DataTextField = Cat_Nom_Percepcion_Deduccion.Campo_Nombre;
            Cmb_Percepciones.DataValueField = Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID;
            Cmb_Percepciones.DataBind();

            Cmb_Percepciones.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));

            Cmb_Percepciones.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las percepciones fijas del empleado. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #endregion

    #region (Grid)

    #region (Grid_Empleados)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Empleados_PageIndexChanging
    ///DESCRIPCIÓN: Realiza el Cambio de la pagina de la tabla.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 29/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Empleados_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            if (Session["Dt_Empleados"] != null)
            {
                LLenar_Grid_Empleados(e.NewPageIndex, (DataTable)Session["Dt_Empleados"]);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error cambiar de un de la tabla. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: LLenar_Grid_Empleados
    ///DESCRIPCIÓN: LLena el grid de Empleados
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 28/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void LLenar_Grid_Empleados(Int32 Pagina, DataTable Tabla)
    {
        Grid_Empleados.Columns[0].Visible = true;
        Grid_Empleados.SelectedIndex = (-1);
        Grid_Empleados.DataSource = Tabla;
        Grid_Empleados.PageIndex = Pagina;
        Grid_Empleados.DataBind();
        Session["Dt_Empleados"] = Tabla;
        Grid_Empleados.Columns[0].Visible = false;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Empleados_RowDataBound
    ///DESCRIPCIÓN: Es el evento previo antes cargar el grid con informacion de 
    ///los empleados
    ///PARAMETROS:  
    ///CREO: Juan Alberto Hernandez Negrete
    ///FECHA_CREO: 28/Noviembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Empleados_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                ((ImageButton)e.Row.Cells[3].FindControl("Btn_Eliminar_Empleado")).CommandArgument = e.Row.Cells[0].Text.Trim();
                ((ImageButton)e.Row.Cells[3].FindControl("Btn_Eliminar_Empleado")).ToolTip = "Quitar al Empleado " + HttpUtility.HtmlDecode(e.Row.Cells[1].Text);
            }
            if (e.Row.RowType.Equals(DataControlRowType.Footer))
            {
                e.Row.Cells[1].Text = HttpUtility.HtmlDecode("<p Style='font-family=Courier New; font-size:11px' >Total de Registros Cargados&nbsp;=&nbsp;" + ((GridView)sender).Rows.Count.ToString() + "</p>");
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Obtener_Total
    ///DESCRIPCIÓN: Obtiene el total de la columna de Cantidad de la tabla de Empleados. 00000001458
    /// 
    ///PARAMETROS: No Aplica.
    ///
    ///CREO: Juan Alberto Hernandez Negrete
    ///FECHA_CREO: Abril/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public double Obtener_Total()
    {
        double Total = 0.0;//Almacenara el total de la columna de cantidad.

        try
        {
            foreach (GridViewRow Fila in Grid_Empleados.Rows)
                Total += Convert.ToDouble(String.IsNullOrEmpty(((Label)Fila.Cells[2].FindControl("Lbl_Cantidad")).Text) ? "0" :
                    ((Label)Fila.Cells[2].FindControl("Lbl_Cantidad")).Text.Replace("$", ""));
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al obtener el total de la columna de CANTIDAD. Error: [" + Ex.Message + "]");
        }
        return Total;
    }
    #endregion

    #endregion

    #region (Eventos)

    #region (Eventos Alta)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Alta de un Asignacion de una Deduccion Variable
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 29/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Nuevo.ToolTip.Equals("Nuevo"))
            {
                Limpiar_Controles();//limpia los controles de la pagina.
                Habilitar_Controles("Nuevo");//Habilita la configuracion de para ejecutar el alta.              
            }
            else
            {
                if (Validar_Datos_Asignacion_Deducciones_Variables())
                {
                    Guardar_Percepciones_Deducciones_Fijas();
                    Limpiar_Controles();
                }
                else {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
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
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Salir de la Operacion Actual
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 17/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip == "Inicio")
            {
                Session.Remove("Dt_Empleados");
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Configuracion_Inicial();//Habilita los controles para la siguiente operación del usuario en el catálogo
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Busqueda_Deducciones_Click
    /// DESCRIPCION : Ejecuta la Busqueda de Percepciones Variables Asignadas Dados de Alta en el Sistema
    /// por diferentes filtros. [No_Deduccion, Estatus, Dependencia]
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 28/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Busqueda_Deducciones_Click(object sender, EventArgs e)
    {

        try
        {
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Error producido al realizar la Busqueda. Error: [" + Ex.Message + "]";
        }
    }
    /// ***********************************************************************************************************
    /// Nombre: Btn_Cargar_Empleados_Click
    /// 
    /// Descripción: Carga los empleados del archivo de conceptos variables.
    /// 
    /// Parámetros: No Áplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 06/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// ***********************************************************************************************************
    protected void Btn_Cargar_Empleados_Click(object sender, EventArgs e)
    {
        DataTable Dt_Empleados = null;//Variable que almacenara la informacion del empleado.

        try
        {
            String Full_Path = Copiar_Archivo_Servidor(AFU_Cargar_Archivo_Fijos);

            if (AFU_Cargar_Archivo_Fijos is AsyncFileUpload)
            {
                if (AFU_Cargar_Archivo_Fijos.HasFile)
                {
                    String Ruta = Path.GetPathRoot(AFU_Cargar_Archivo_Fijos.PostedFile.FileName);//Obtenemos la ruta.
                    String Nombre = Path.GetFileNameWithoutExtension(AFU_Cargar_Archivo_Fijos.PostedFile.FileName);//Obtenemos el nombre del archivo.
                    String Extension = Path.GetExtension(AFU_Cargar_Archivo_Fijos.PostedFile.FileName);//Obtenemos la extensión del archivo.

                    if (Extension.Equals(".xlsx") || Extension.Equals(".xls"))
                    {
                        //Dt_Empleados = Leer_Archivo_Canceptos_Variables(Ruta, Nombre, Extension);//Leemos los empleados a lo que le aplicara el concepto variable.
                        //Dt_Empleados = Leer_Archivo_Excel(AFU_Cargar_Archivo_Fijos.PostedFile.FileName);

                        if (RBtn_Tipo_Percepcion_Deduccion.SelectedItem.Text.Trim().Equals("DEDUCCION"))
                        {
                            HTxt_Referencia.Value = Presidencia.Ayudante_Excel.Cls_Ayudante_Leer_Excel.Obtener_Referencia(
                                Presidencia.Ayudante_Excel.Cls_Ayudante_Leer_Excel.Leer_Tabla_Excel(Full_Path, "REFERENCIA"));
                        }

                        Dt_Empleados = Presidencia.Ayudante_Excel.Cls_Ayudante_Leer_Excel.Leer_Tabla_Excel(Full_Path, "FIJOS");
                        Dt_Empleados = Presidencia.Ayudante_Excel.Cls_Ayudante_Leer_Excel.Crear_Estructura_Carga_Masiva(Dt_Empleados);

                        Grid_Empleados.Columns[0].Visible = true;
                        Session["Dt_Empleados"] = Dt_Empleados;
                        Grid_Empleados.DataSource = (DataTable)Session["Dt_Empleados"];
                        Grid_Empleados.DataBind();
                        Grid_Empleados.Columns[0].Visible = false;
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Text = "Archivo Invalido";
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                    }

                    Remover_Sesiones_Control_Carga_Archivos(AFU_Cargar_Archivo_Fijos.ClientID);
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }
    /// ***********************************************************************************************************
    /// Nombre: Btn_Limpiar_Empleados_Click
    /// 
    /// Descripción: Limpia los empleados leidos del archivo de conceptos variables.
    /// 
    /// Parámetros: No Áplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 06/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// ***********************************************************************************************************
    protected void Btn_Limpiar_Empleados_Click(object sender, EventArgs e)
    {
        try
        {
            Session["Dt_Empleados"] = new DataTable();
            Grid_Empleados.Columns[0].Visible = true;
            Grid_Empleados.DataSource = (DataTable)Session["Dt_Empleados"];
            Grid_Empleados.DataBind();
            Grid_Empleados.SelectedIndex = -1;
            Grid_Empleados.Columns[0].Visible = false;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al leer los empleados del archivo de conceptos variables. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region(Eventos Agregar y Quitar Empleados)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Eliminar_Empleado_Click
    /// DESCRIPCION : Evento que genera la peticion para Quitar el Empleado seleccionado
    /// de la tabla de Empelados.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 17/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Eliminar_Empleado_Click(object sender, EventArgs e)
    {
        DataRow[] Renglones;//Variable que almacena una lista de DataRows del  Grid_Empleados
        DataRow Renglon;//Variable que almacenara un Renglon del Grid_Empleados
        ImageButton Btn_Eliminar_Empleado = (ImageButton)sender;//Variable que almacenra el control Btn_Eliminar_Empleado

        if (Session["Dt_Empleados"] != null)
        {
            Renglones = ((DataTable)Session["Dt_Empleados"]).Select(Cat_Empleados.Campo_Empleado_ID + "='" + Btn_Eliminar_Empleado.CommandArgument + "'");

            if (Renglones.Length > 0)
            {
                Renglon = Renglones[0];
                DataTable Tabla = (DataTable)Session["Dt_Empleados"];
                Tabla.Rows.Remove(Renglon);
                Session["Dt_Empleados"] = Tabla;
                Grid_Empleados.SelectedIndex = (-1);
                LLenar_Grid_Empleados(Grid_Empleados.PageIndex, Tabla);
            }
        }
        else
        {
            Lbl_Mensaje_Error.Text = "Se debe seleccionar de la tabla el Empleados a quitar";
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }
    #endregion

    #region (Eventos Combos)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Calendario_Nomina_SelectedIndexChanged
    ///DESCRIPCIÓN: Consulta los periodos catorcenales de la nomina seleccionada.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 01/Diciembre/2010
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
    ///NOMBRE DE LA FUNCIÓN: Cmb_Calendario_Nomina_SelectedIndexChanged
    ///DESCRIPCIÓN: Consulta los periodos catorcenales de la nomina seleccionada.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 01/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void RBtn_Tipo_Percepcion_Deduccion_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 index = RBtn_Tipo_Percepcion_Deduccion.SelectedIndex;

        if (index >= 0)
        {
            Consultar_Percepciones_Fijas();
            ScriptManager.RegisterClientScriptBlock(this ,this.GetType(), "", "javascript:refresh_tabla_empleados();", true);
        }
    }
    #endregion

    #endregion

}
