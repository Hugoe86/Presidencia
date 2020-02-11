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
using Presidencia.Proveedores.Negocios;
using Presidencia.Cap_Masiva_Prov_Fijas.Negocio;
using Presidencia.Nomina_Operacion_Proveedores.Negocio;
using Presidencia.Cat_Parametros_Nomina.Negocio;
using Presidencia.Ayudante_Informacion;

public partial class paginas_Nomina_Frm_Ope_Nom_Fonacot_Individual : System.Web.UI.Page
{
    #region (Page_Load)
    /// ***************************************************************************************************
    /// Nombre: Page_Load
    /// 
    /// Descripción: Carga la configuración inicial de la página.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario creo: Juan ALberto Hernández Negrete.
    /// Fecha Creó: 14/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ***************************************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
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
        Consultar_Proveedores();
        Consultar_Deducciones_Fonacot();
        Consultar_SAP_Unidades_Responsables();
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
            Txt_No_Periodos.Text = String.Empty;
            Txt_Busqueda_RFC.Text = String.Empty;
            Txt_Importe.Text = String.Empty;
            Txt_No_Credito.Text = String.Empty;
            Txt_No_Fonacot.Text = String.Empty;
            Txt_No_Periodos.Text = String.Empty;
            Txt_Plazo.Text = String.Empty;
            Txt_Retension_Mensual.Text = String.Empty;
            Txt_Retension_Real.Text = String.Empty;
            Txt_Cuotas_Pagadas.Text = String.Empty;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al limpiar los controles del formulario. Error: [" + Ex.Message.ToString() + "]");
        }
    }    
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles_Despues_Operacion
    /// DESCRIPCION : Limpia los Controles de la pagina.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 29/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles_Despues_Operacion()
    {
        try
        {
            Txt_No_Periodos.Text = String.Empty;
            Txt_Busqueda_No_Empleado.Text = String.Empty;
            Txt_Busqueda_Nombre_Empleado.Text = String.Empty;
            Txt_Busqueda_RFC.Text = String.Empty;
            Txt_Importe.Text = String.Empty;
            Txt_No_Credito.Text = String.Empty;
            Txt_No_Empleado_Fonacot.Text = String.Empty;
            Txt_No_Fonacot.Text = String.Empty;
            Txt_No_Periodos.Text = String.Empty;
            Txt_Nombre_Empleado.Text = String.Empty;
            Txt_Plazo.Text = String.Empty;
            Txt_Retension_Mensual.Text = String.Empty;
            Txt_Retension_Real.Text = String.Empty;
            HTxt_Empleado_ID.Value = String.Empty;
            Txt_Cuotas_Pagadas.Text = String.Empty;
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
                    Cmb_Calendario_Nomina.Focus();

                    break;
                case "Modificar":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";

                    break;
            }

            Cmb_Calendario_Nomina.Enabled = Habilitado;
            Cmb_Periodos_Catorcenales_Nomina.Enabled = Habilitado;
            Cmb_Proveedores.Enabled = false;
            Txt_No_Periodos.Enabled = Habilitado;

            Txt_Importe.Enabled = Habilitado;
            Txt_No_Credito.Enabled = Habilitado;
            Txt_No_Empleado_Fonacot.Enabled = false;
            Txt_Nombre_Empleado.Enabled = false;
            Txt_No_Credito.Enabled = Habilitado;
            Txt_No_Fonacot.Enabled = Habilitado;
            Txt_No_Periodos.Enabled = Habilitado;
            Txt_Plazo.Enabled = Habilitado;
            Txt_Retension_Mensual.Enabled = false;
            Txt_Retension_Real.Enabled = false;
            Txt_RFC_Proveedor.Enabled = false;
            Txt_Cuotas_Pagadas.Enabled = Habilitado;

            Txt_Fecha_Autorizacion.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Now);
            Btn_Fecha_Autorizacion.Enabled = false;
            Txt_Fecha_Autorizacion.Enabled = false;
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

        if (Cmb_Calendario_Nomina.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione un calendario de nómina. <br />";
            Datos_Validos = false;
        }

        if (Cmb_Periodos_Catorcenales_Nomina.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione un periodo de pago. <br />";
            Datos_Validos = false;
        }

        if (Cmb_Proveedores.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione un proveedor. <br />";
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
    /// *****************************************************************************************
    /// Nombre: Consultar_Nombre_Deduccion
    /// 
    /// Descripción: Consulta el nombre y la clave de la deducción por el identificador de la
    ///              la misma.
    /// 
    /// Parámetros: Deduccion_ID .- Identificador de la deducción a consultar.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 13/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *****************************************************************************************
    protected String Consultar_Nombre_Deduccion(String Deduccion_ID)
    {
        Cls_Cat_Nom_Percepciones_Deducciones_Business Obj_Percepcion_Deduccion =
            new Cls_Cat_Nom_Percepciones_Deducciones_Business();//Variable de conexion con la capa de negocios.
        DataTable Dt_Percepcion_Deduccion = null;//Variable que gusrada un listado de las percepciones y deducciones que tiene asiganadas el empleado.
        String Nombre_Deduccion = String.Empty;//Variable que almacenara el identificador del concepto.

        try
        {
            Obj_Percepcion_Deduccion.P_PERCEPCION_DEDUCCION_ID = Deduccion_ID;
            Dt_Percepcion_Deduccion = Obj_Percepcion_Deduccion.Consultar_Percepciones_Deducciones_General();

            if (Dt_Percepcion_Deduccion is DataTable)
            {
                if (Dt_Percepcion_Deduccion.Rows.Count > 0)
                {
                    foreach (DataRow DEDUCCION in Dt_Percepcion_Deduccion.Rows)
                    {
                        if (DEDUCCION is DataRow)
                        {
                            Nombre_Deduccion = "[" + DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Clave] + "] -- " +
                                DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Nombre];
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar el nombre de la deducción. Error: [" + Ex.Message + "]");
        }
        return Nombre_Deduccion;
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
                                        FILA[COLUMNA.ColumnName.Trim()] = FILA[COLUMNA.ColumnName.Trim()].ToString().Trim().Replace(",", "");
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
    /// ***********************************************************************************************************************************************
    /// Nombre: OBTENER_DEDUCCION
    /// 
    /// Descripción: Método que obtiene la deducción consecutiva que tiene disponible el empleado por el proveedor FONACOT.
    /// 
    /// Parámetros: Proveedor_ID.- FONACOT.
    ///             Empleado_ID.- Identificador del empleado que es utilizado para el control interno del sistema.
    ///             Deducciones_Asignadas.- Tabla que almacena las deducciones que ya fueron ocupadas en la actual carga masiva. Por lo tanto
    ///             tambien se descartara de la deducciones disponibles.
    /// 
    /// Usuario creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Diciembre/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ***********************************************************************************************************************************************
    private static String Obtener_Deduccion(String Proveedor_ID, String Empleado_ID, 
        String Nomina_ID, String No_Nomina)
    {
        Cls_Cat_Nom_Proveedores_Negocio Obj_Proveedores = new Cls_Cat_Nom_Proveedores_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Ope_Nom_Proveedores_Negocio Obj_Ope_Proveedores = new Cls_Ope_Nom_Proveedores_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Proveedores = null;//Variable que almacenara la lista de proveedores.
        DataTable Dt_Deducciones_Ocupadas = null;//Variable que almacenara las deducciones que actualmente ya no se encuentran disponibles.
        String Deduccion_Consecutiva = String.Empty;//Variable que almacenara la deducción consecutiva.

        try
        {
            //Consultamos las deducciones que tiene asignadas el proveedor.
            Obj_Proveedores.P_Proveedor_ID = Proveedor_ID;
            Dt_Proveedores = Obj_Proveedores.Consultar_Deducciones_Proveedor();

            //Consultamos las deducciones que actualmente se encuentran ocupadas.
            Obj_Ope_Proveedores.P_Empleado_ID = Empleado_ID;
            Obj_Ope_Proveedores.P_Nomina_ID = Nomina_ID;
            Obj_Ope_Proveedores.P_No_Nomina = Convert.ToInt32((!String.IsNullOrEmpty(No_Nomina)) ? No_Nomina : "0");
            Dt_Deducciones_Ocupadas = Obj_Ope_Proveedores.Consultar_Deduccion();

            //Codigo que descarta las deducciones que actulmente se ecuentran ocupadas.
            if (Dt_Deducciones_Ocupadas is DataTable)
            {
                if (Dt_Deducciones_Ocupadas.Rows.Count > 0)
                {
                    foreach (DataRow DEDUCCIONES in Dt_Deducciones_Ocupadas.Rows)
                    {
                        if (DEDUCCIONES is DataRow)
                        {
                            if (!String.IsNullOrEmpty(DEDUCCIONES[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString()))
                            {
                                DataRow[] Dr = Dt_Proveedores.Select(Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + "=" +
                                    DEDUCCIONES[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString());

                                if (Dr.Length > 0)
                                    Dt_Proveedores.Rows.Remove(Dr[0]);
                            }
                        }
                    }
                }
            }

            //Obtenemos la deduccion consecutiva.
            DataRow[] Dr_Min_Value = Dt_Proveedores.Select(Ope_Nom_Proveedores_Detalles.Campo_Percepcion_Deduccion_ID + " = MIN(" + Ope_Nom_Proveedores_Detalles.Campo_Percepcion_Deduccion_ID + ")");
            if (Dr_Min_Value.Length > 0)
                Deduccion_Consecutiva = Dr_Min_Value[0][Ope_Nom_Proveedores_Detalles.Campo_Percepcion_Deduccion_ID].ToString();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar la deduccion consecutiva. Error: [" + Ex.Message + "'");
        }
        return Deduccion_Consecutiva;
    }
    /// ***********************************************************************************************************************************************
    /// Nombre: Crear_Estructura_Para_Subir_Credito_Fonacot
    /// 
    /// Descripción: Método que obtiene la deducción consecutiva que tiene disponible el empleado por el proveedor FONACOT. Y crea la estructura
    ///              de la tabla para dar el alta del crédito fonacot.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Abril/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ***********************************************************************************************************************************************
    private DataTable Crear_Estructura_Para_Subir_Credito_Fonacot()
    {
        DataTable Dt_Empleados_Out = new DataTable();//Variable que almacenara un listado de empleados.
        Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nomina = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Calendario_Nomina = null;//Variable que almacenara el calendario de nomina consultado.
        Int32 No_Catorcenas_Mes = 0;//Numero de catorcenas del mes.
        String Anio_Nomina = String.Empty;//Año de la nómina actual.
        String Nomina_ID = String.Empty;//Identificador del calendario de nómina.
        String No_Nomina = String.Empty;//Periodo del cuál se generara la nómina.

        try
        {
            No_Catorcenas_Mes = Convert.ToInt32((String.IsNullOrEmpty(Txt_No_Periodos.Text)) ? "0" : Txt_No_Periodos.Text);
            Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
            No_Nomina = Cmb_Periodos_Catorcenales_Nomina.SelectedItem.Text;

            //Consultamos los periodos de la nomina actual para determinar a partir de cuando se comenzaran a realizar los descuentos.
            Obj_Calendario_Nomina.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
            Dt_Calendario_Nomina = Obj_Calendario_Nomina.Consultar_Calendario_Nominas();

            //Obtenemos el año del calendario de la nomina actual.
            if (Dt_Calendario_Nomina is DataTable)
            {
                if (Dt_Calendario_Nomina.Rows.Count > 0)
                {
                    foreach (DataRow CALENDARIO in Dt_Calendario_Nomina.Rows)
                    {
                        if (CALENDARIO is DataRow)
                        {
                            if (!String.IsNullOrEmpty(CALENDARIO[Cat_Nom_Calendario_Nominas.Campo_Anio].ToString()))
                                Anio_Nomina = CALENDARIO[Cat_Nom_Calendario_Nominas.Campo_Anio].ToString().Trim();
                        }
                    }
                }
            }

            //Creamos la estructura de la tabla que almacenara la información del archivo de excel.
            Dt_Empleados_Out.Columns.Add("ELIMINAR", typeof(String));
            Dt_Empleados_Out.Columns.Add(Ope_Nom_Proveedores_Detalles.Campo_Empleado_ID, typeof(String));
            Dt_Empleados_Out.Columns.Add(Ope_Nom_Proveedores_Detalles.Campo_Nombre, typeof(String));
            Dt_Empleados_Out.Columns.Add(Ope_Nom_Proveedores_Detalles.Campo_RFC, typeof(String));
            Dt_Empleados_Out.Columns.Add("NOMBRE_DEDUCCION", typeof(String));
            Dt_Empleados_Out.Columns.Add(Ope_Nom_Proveedores_Detalles.Campo_Percepcion_Deduccion_ID, typeof(String));
            Dt_Empleados_Out.Columns.Add(Ope_Nom_Proveedores_Detalles.Campo_No_Fonacot, typeof(String));
            Dt_Empleados_Out.Columns.Add(Ope_Nom_Proveedores_Detalles.Campo_No_Credito, typeof(String));
            Dt_Empleados_Out.Columns.Add(Ope_Nom_Proveedores_Detalles.Campo_Cantidad, typeof(String));
            Dt_Empleados_Out.Columns.Add(Ope_Nom_Proveedores_Detalles.Campo_Cuotas_Pagadas, typeof(String));
            Dt_Empleados_Out.Columns.Add(Ope_Nom_Proveedores_Detalles.Campo_Plazo, typeof(String));
            Dt_Empleados_Out.Columns.Add(Ope_Nom_Proveedores_Detalles.Campo_Retencion_Mensual, typeof(String));
            Dt_Empleados_Out.Columns.Add(Ope_Nom_Proveedores_Detalles.Campo_Retencion_Real, typeof(String));
            Dt_Empleados_Out.Columns.Add(Ope_Nom_Proveedores_Detalles.Campo_Nomina_ID, typeof(String));
            Dt_Empleados_Out.Columns.Add(Ope_Nom_Proveedores_Detalles.Campo_Periodo, typeof(String));
            Dt_Empleados_Out.Columns.Add(Ope_Nom_Proveedores_Detalles.Campo_Estatus, typeof(String));
            Dt_Empleados_Out.Columns.Add(Cat_Nom_Calendario_Nominas.Campo_Anio, typeof(String));


            Int32 Contador_Periodos = 1;//Creamos y establecemos el valor inicial del contador de periodos.

            while (Contador_Periodos <= No_Catorcenas_Mes)
            {
                //Cargamos la tabla que almacenara la información del archivo de excel.
                DataRow RENGLON = Dt_Empleados_Out.NewRow();

                RENGLON["ELIMINAR"] = String.Empty;
                RENGLON[Ope_Nom_Proveedores_Detalles.Campo_Empleado_ID] = HTxt_Empleado_ID.Value;
                RENGLON[Ope_Nom_Proveedores_Detalles.Campo_Nombre] = Txt_Nombre_Empleado.Text;
                RENGLON[Ope_Nom_Proveedores_Detalles.Campo_RFC] = Txt_RFC_Proveedor.Text;
                RENGLON[Ope_Nom_Proveedores_Detalles.Campo_Percepcion_Deduccion_ID] = Cmb_Deducciones_Fonacot.SelectedValue.Trim();
                RENGLON["NOMBRE_DEDUCCION"] = String.Empty;
                RENGLON[Ope_Nom_Proveedores_Detalles.Campo_No_Fonacot] = Txt_No_Fonacot.Text;
                RENGLON[Ope_Nom_Proveedores_Detalles.Campo_No_Credito] = Txt_No_Credito.Text;
                RENGLON[Ope_Nom_Proveedores_Detalles.Campo_Cantidad] = System.String.Format("{0:c}", Convert.ToDouble((!String.IsNullOrEmpty(Txt_Retension_Real.Text)) ? Txt_Retension_Real.Text : "0"));
                RENGLON[Ope_Nom_Proveedores_Detalles.Campo_Cuotas_Pagadas] = String.IsNullOrEmpty(Txt_Cuotas_Pagadas.Text) ? "0" : Txt_Cuotas_Pagadas.Text.Trim();
                RENGLON[Ope_Nom_Proveedores_Detalles.Campo_Plazo] = Txt_Plazo.Text;
                RENGLON[Ope_Nom_Proveedores_Detalles.Campo_Retencion_Mensual] = System.String.Format("{0:c}", Convert.ToDouble((!String.IsNullOrEmpty(Txt_Retension_Mensual.Text)) ? Txt_Retension_Mensual.Text : "0"));
                RENGLON[Ope_Nom_Proveedores_Detalles.Campo_Retencion_Real] = System.String.Format("{0:c}", Convert.ToDouble((!String.IsNullOrEmpty(Txt_Retension_Real.Text)) ? Txt_Retension_Real.Text : "0"));
                RENGLON[Ope_Nom_Proveedores_Detalles.Campo_Nomina_ID] = Nomina_ID;
                RENGLON[Ope_Nom_Proveedores_Detalles.Campo_Periodo] = No_Nomina;
                RENGLON[Ope_Nom_Proveedores_Detalles.Campo_Estatus] = "ACEPTADO";
                RENGLON[Cat_Nom_Calendario_Nominas.Campo_Anio] = Anio_Nomina;

                Dt_Empleados_Out.Rows.Add(RENGLON);

                String Resultado = Obtener_Nomina_Periodo_Consecutivo(Nomina_ID, No_Nomina);
                String[] Nomina_Periodo = null;

                if (!String.IsNullOrEmpty(Resultado))
                {
                    Nomina_Periodo = Resultado.Split(new Char[] { ',' });//Obtenemos la Nomina_ID y el No_Nomina consecutivos.

                    if (Nomina_Periodo.Length > 0)
                    {
                        Nomina_ID = Nomina_Periodo[0];
                        No_Nomina = Nomina_Periodo[1];
                    }
                }
                ++Contador_Periodos;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error Crear_Estructura_Para_Subir_Credito_Fonacot. Error: [" + Ex.Message + "]");
        }
        return Dt_Empleados_Out;
    }
    /// ***********************************************************************************************************************************************
    /// Nombre: OBTENER_NOMINA_PERIODO_CONSECUTIVO
    /// 
    /// Descripción: Método que obtiene la nómina y el periodo consecutivo.
    /// 
    /// Parámetros: Nomina_ID.- Nomina vigente.
    ///             No_Nomina.- Periodo nominal.
    /// 
    /// Usuario creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Diciembre/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ***********************************************************************************************************************************************
    private static String Obtener_Nomina_Periodo_Consecutivo(String Nomina_ID, String No_Nomina)
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nomina = new Cls_Cat_Nom_Calendario_Nominas_Negocio();
        DataTable Dt_Calendario_Nomina = null;
        String Resultado = String.Empty;

        try
        {
            Obj_Calendario_Nomina.P_Nomina_ID = Nomina_ID;
            Dt_Calendario_Nomina = Obj_Calendario_Nomina.Consulta_Detalles_Nomina();

            DataRow[] Dr_Periodos = Dt_Calendario_Nomina.Select("NO_NOMINA=" + (Convert.ToInt32(No_Nomina) + 1));

            if (Dr_Periodos.Length > 0)
            {
                foreach (DataRow Fila in Dr_Periodos)
                {
                    if (!String.IsNullOrEmpty(Fila[Cat_Nom_Nominas_Detalles.Campo_No_Nomina].ToString()))
                    {
                        Resultado = Fila[Cat_Nom_Nominas_Detalles.Campo_Nomina_ID].ToString();
                        Resultado += ",";
                        Resultado += Fila[Cat_Nom_Nominas_Detalles.Campo_No_Nomina].ToString();
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar la nomina y el periodo en el que aplicaran los pagos de fonacot. Error: [" + Ex.Message + "]");
        }
        return Resultado;
    }
    #endregion

    #region (Guardar)
    /// ***************************************************************************************************
    /// Nombre: Guardar_Deducciones_Fijas
    /// 
    /// Descripción: Guarda las deducciones que le aplicaran  al empleado por concepto de algún proveedor.
    /// 
    /// Parámetros: Datos.- Objeto que almacena los datos que se usaran en la consulta.
    /// 
    /// Usuario creo: Juan ALberto Hernández Negrete.
    /// Fecha Creó: 14/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ***************************************************************************************************
    protected void Guardar_Percepciones_Deducciones_Fijas()
    {
        Cls_Ope_Nom_Proveedores_Negocio Obj_Proveedores = new Cls_Ope_Nom_Proveedores_Negocio();

        try
        {
            Obj_Proveedores.P_Proveedor_ID = Cmb_Proveedores.SelectedValue.Trim();
            Obj_Proveedores.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
            Obj_Proveedores.P_No_Nomina = Convert.ToInt32((Cmb_Periodos_Catorcenales_Nomina.SelectedIndex <= 0) ? "0" : Cmb_Periodos_Catorcenales_Nomina.SelectedValue.Trim());
            Obj_Proveedores.P_No_Periodos = Convert.ToInt32((!String.IsNullOrEmpty(Txt_No_Periodos.Text.Trim())) ? Txt_No_Periodos.Text.Trim() : "0");
            Obj_Proveedores.P_Usuario = Cls_Sessiones.Nombre_Empleado;
            Obj_Proveedores.P_Dt_Datos_Archivo = Quitar_Caracteres_Cantidad(((DataTable)Session["Dt_Empleados"]));
            if (!String.IsNullOrEmpty(Txt_Fecha_Autorizacion.Text)) { Obj_Proveedores.P_Fecha_Autorizacion = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Autorizacion.Text.Trim())); }

            Obj_Proveedores.Subir_Informacion();
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
    /// ***************************************************************************************************
    /// Nombre: Consultar_Proveedores
    /// 
    /// Descripción: Consulta los proveedores que estan activos actualmente en el sistema.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario creo: Juan ALberto Hernández Negrete.
    /// Fecha Creó: 14/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ***************************************************************************************************
    protected void Consultar_Proveedores()
    {
        Cls_Cat_Nom_Proveedores_Negocio Obj_Proveedores = new Cls_Cat_Nom_Proveedores_Negocio();//Variable de conexión con la capa de negocios.
        Cls_Cat_Nom_Parametros_Negocio INF_PARAMETROS = null;//Variable de conexion con la capd e negocios.
        DataTable Dt_Proveedores = null;//Variable que almacena un listado de los proveedores del sistema.

        try
        {
            INF_PARAMETROS = Presidencia.Ayudante_Informacion.Cls_Ayudante_Nom_Informacion._Informacion_Parametros_Nomina();

            Dt_Proveedores = Obj_Proveedores.Consultar_Proveedores();
            Cmb_Proveedores.DataSource = Dt_Proveedores;
            Cmb_Proveedores.DataTextField = Cat_Nom_Proveedores.Campo_Nombre;
            Cmb_Proveedores.DataValueField = Cat_Nom_Proveedores.Campo_Proveedor_ID;
            Cmb_Proveedores.DataBind();

            Cmb_Proveedores.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));

            Cmb_Proveedores.SelectedIndex = Cmb_Proveedores.Items.IndexOf(
                Cmb_Proveedores.Items.FindByValue(INF_PARAMETROS.P_Proveedor_Fonacot));

            if (Cmb_Proveedores.SelectedIndex > 0) {

                if (Dt_Proveedores != null) {
                    var Provedores = from proveedor in Dt_Proveedores.AsEnumerable()
                                     where proveedor.Field<String>(Cat_Nom_Proveedores.Campo_Proveedor_ID) == INF_PARAMETROS.P_Proveedor_Fonacot
                                     select new { RFC = proveedor.Field<String>(Cat_Nom_Proveedores.Campo_RFC) };

                    foreach (var item_proveedor in Provedores) {
                        Txt_RFC_Proveedor.Text = item_proveedor.RFC;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los proveedores. Error: [" + Ex.Message + "]");
        }
    }
    /// ***************************************************************************************************
    /// Nombre: Consultar_Deducciones_Fonacot
    /// 
    /// Descripción: Consulta las deducciones del proveedor fonacot que estan activas actualmente en el sistema.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario creo: Juan ALberto Hernández Negrete.
    /// Fecha Creó: Abril/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ***************************************************************************************************
    protected void Consultar_Deducciones_Fonacot()
    {
        Cls_Cat_Nom_Proveedores_Negocio Obj_Proveedores = new Cls_Cat_Nom_Proveedores_Negocio();//Variable de conexion con la capa de  negocios.       
        DataTable Dt_Deducciones_Fonacot = null;//Listados de deducciones que aplicaran por fonacot.
        Cls_Cat_Nom_Parametros_Negocio INF_PARAMETROS = null;//Variable que almacena los parametros de la nomina.

        try
        {
            INF_PARAMETROS = Presidencia.Ayudante_Informacion.Cls_Ayudante_Nom_Informacion._Informacion_Parametros_Nomina();

            Obj_Proveedores.P_Proveedor_ID = INF_PARAMETROS.P_Proveedor_Fonacot;
            Dt_Deducciones_Fonacot = Obj_Proveedores.Consultar_Deducciones_Proveedor();
            Dt_Deducciones_Fonacot = Juntar_Clave_Percepcion_Deduccion(Dt_Deducciones_Fonacot);

            Cmb_Deducciones_Fonacot.DataSource = Dt_Deducciones_Fonacot;
            Cmb_Deducciones_Fonacot.DataTextField = Cat_Nom_Percepcion_Deduccion.Campo_Nombre;
            Cmb_Deducciones_Fonacot.DataValueField = Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID;
            Cmb_Deducciones_Fonacot.DataBind();

            Cmb_Deducciones_Fonacot.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Deducciones_Fonacot.SelectedIndex = -1; 
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las deducciones de fonacot. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_SAP_Unidades_Responsables
    /// 
    /// DESCRIPCION : Consulta los unidades responsables que existen actualmente 
    ///               registrados en el sistema.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 08/Diciembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_SAP_Unidades_Responsables()
    {
        Cls_Cat_Dependencias_Negocio Obj_Unidades_Responsables = new Cls_Cat_Dependencias_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Unidades_Responsables = null;//Variable que lista las unidades responsables registrdas en sistema.

        try
        {
            Dt_Unidades_Responsables = Obj_Unidades_Responsables.Consulta_Dependencias();
            Cmb_Busqueda_Dependencia.DataSource = Dt_Unidades_Responsables;
            Cmb_Busqueda_Dependencia.DataTextField = "CLAVE_NOMBRE";
            Cmb_Busqueda_Dependencia.DataValueField = Cat_Dependencias.Campo_Dependencia_ID;
            Cmb_Busqueda_Dependencia.DataBind();
            Cmb_Busqueda_Dependencia.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Busqueda_Dependencia.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las unidades responsables registradas en sistema. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #endregion

    #region (GridView)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llenar_Grid_Busqueda_Empleados
    /// 
    /// DESCRIPCION : Consulta y carga el grid de los empleados.
    ///               
    /// PARAMETROS  : No Aplica.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 08/Diciembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llenar_Grid_Busqueda_Empleados()
    {
        Cls_Cat_Empleados_Negocios Negocio = new Cls_Cat_Empleados_Negocios();

        Grid_Busqueda_Empleados.SelectedIndex = (-1);
        Grid_Busqueda_Empleados.Columns[1].Visible = true;

        if (Txt_Busqueda_No_Empleado.Text.Trim().Length > 0) { Negocio.P_No_Empleado = Txt_Busqueda_No_Empleado.Text.Trim(); }
        if (Txt_Busqueda_RFC.Text.Trim().Length > 0) { Negocio.P_RFC = Txt_Busqueda_RFC.Text.Trim(); }
        if (Txt_Busqueda_Nombre_Empleado.Text.Trim().Length > 0) { Negocio.P_Nombre = Txt_Busqueda_Nombre_Empleado.Text.Trim(); }
        if (Cmb_Busqueda_Dependencia.SelectedIndex > 0) { Negocio.P_Dependencia_ID = Cmb_Busqueda_Dependencia.SelectedItem.Value; }


        Grid_Busqueda_Empleados.DataSource = Negocio.Consultar_Empleados_Resguardos();
        Grid_Busqueda_Empleados.DataBind();
        Grid_Busqueda_Empleados.Columns[1].Visible = false;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Busqueda_Empleados_PageIndexChanging
    /// 
    /// DESCRIPCION : Cambia la pagina del grid del empleados.
    ///               
    /// PARAMETROS  : No Aplica.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 08/Diciembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Busqueda_Empleados_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Busqueda_Empleados.PageIndex = e.NewPageIndex;
            Llenar_Grid_Busqueda_Empleados();
            MPE_Empleados.Show();
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Busqueda_Empleados_SelectedIndexChanged
    /// 
    /// DESCRIPCION : Carga la información del registro seleccionado.
    ///               
    /// PARAMETROS  : No Aplica.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 08/Diciembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Busqueda_Empleados_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;

        try
        {
            if (Grid_Busqueda_Empleados.SelectedIndex > (-1))
            {
                INF_EMPLEADO = Cls_Ayudante_Nom_Informacion._Informacion_Empleado(HttpUtility.HtmlDecode(Grid_Busqueda_Empleados.Rows[Grid_Busqueda_Empleados.SelectedIndex].Cells[2].Text));

                Txt_No_Empleado_Fonacot.Text = HttpUtility.HtmlDecode(Grid_Busqueda_Empleados.Rows[Grid_Busqueda_Empleados.SelectedIndex].Cells[2].Text);
                Txt_Nombre_Empleado.Text = HttpUtility.HtmlDecode(Grid_Busqueda_Empleados.Rows[Grid_Busqueda_Empleados.SelectedIndex].Cells[3].Text);
                HTxt_Empleado_ID.Value = INF_EMPLEADO.P_Empleado_ID;

                Cmb_Deducciones_Fonacot.SelectedIndex = Cmb_Deducciones_Fonacot.Items.IndexOf(
                    Cmb_Deducciones_Fonacot.Items.FindByValue(Obtener_Deduccion(Cmb_Proveedores.SelectedValue, HTxt_Empleado_ID.Value, Cmb_Calendario_Nomina.SelectedValue, Cmb_Periodos_Catorcenales_Nomina.SelectedItem.Text)));

                MPE_Empleados.Hide();
                UPnl_Captura_Masiva_Proveedores_Fijas.Update();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }
    #endregion

    #region (Eventos)

    #region (Eventos Botones)
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
                if (!String.IsNullOrEmpty(HTxt_Empleado_ID.Value))
                {
                    Limpiar_Controles();//limpia los controles de la pagina.
                    Habilitar_Controles("Nuevo");//Habilita la configuracion de para ejecutar el alta.  
                }
                else {
                    Lbl_Mensaje_Error.Text = "Es necesario seleccionar el empleado al cual se le capturara el crédito fonacot.";
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
            else
            {
                if (Validar_Datos_Asignacion_Deducciones_Variables())
                {
                    Session["Dt_Empleados"] = Crear_Estructura_Para_Subir_Credito_Fonacot();
                    Guardar_Percepciones_Deducciones_Fijas();

                    Configuracion_Inicial();
                    Limpiar_Controles_Despues_Operacion();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Información", "alert('Operación Completa');", true);
                }
                else
                {
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
        String Nomina_ID = String.Empty;
        String No_Nomina = String.Empty;
        Int32 Numero_Catorcenas_Mes = 0;

        try
        {
            Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
            No_Nomina = Cmb_Periodos_Catorcenales_Nomina.SelectedItem.Text.Trim();
            Numero_Catorcenas_Mes = Convert.ToInt32((!String.IsNullOrEmpty(Txt_No_Periodos.Text.Trim())) ? Txt_No_Periodos.Text.Trim() : "0");

            Session["Dt_Empleados"] = Dt_Empleados;
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Empleados_Click
    ///DESCRIPCIÓN: Ejecuta la busqueda de empleados.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 08/Diciembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Busqueda_Empleados_Click(object sender, EventArgs e)
    {
        try
        {
            Grid_Busqueda_Empleados.PageIndex = 0;
            Llenar_Grid_Busqueda_Empleados();
            MPE_Empleados.Show();
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
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
        Cmb_Calendario_Nomina.Focus();
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Periodos_Selected
    ///DESCRIPCIÓN: Consulta el la deduccion disponible de fonacot del empleado.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: Abril/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Periodos_Selected(object sender, EventArgs e)
    {
        try
        {
            Cmb_Deducciones_Fonacot.SelectedIndex = Cmb_Deducciones_Fonacot.Items.IndexOf(
                Cmb_Deducciones_Fonacot.Items.FindByValue(Obtener_Deduccion(Cmb_Proveedores.SelectedValue, HTxt_Empleado_ID.Value, 
                    Cmb_Calendario_Nomina.SelectedValue, Cmb_Periodos_Catorcenales_Nomina.SelectedItem.Text)));
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al seleccionar un periodo del calendario. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #endregion
}
