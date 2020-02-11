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
using System.Text.RegularExpressions;
using Presidencia.Empleados.Negocios;
using Presidencia.Dependencias.Negocios;
using Presidencia.Prestamos.Negocio;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Proveedores.Negocios;
using Presidencia.Sindicatos.Negocios;
using Presidencia.Tipos_Nominas.Negocios;
using Presidencia.Cat_Nom_Percepciones_Deducciones_Opcional.Negocios;
using System.Collections.Generic;
using Presidencia.Nomina_Percepciones_Deducciones;
using Presidencia.Finiquitos.Negocio;
using Presidencia.Recibos_Empleados.Negocio;
using System.Text;
using Presidencia.Archivos_Historial_Nomina_Generada;
using System.IO;
using Presidencia.Control_Patrimonial_Operacion_Bienes_Muebles.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Cemovientes.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Vehiculos.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Partes_Vehiculos.Negocio;
using Presidencia.Ajuste_ISR.Negocio;
using Presidencia.Cat_Parametros_Nomina.Negocio;
using Presidencia.Zona_Economica.Negocios;
using Presidencia.Ayudante_Exentos_Gravados;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Numalet;
using System.Globalization;
using Presidencia.Ayudante_Informacion;

public partial class paginas_Nomina_Frm_Ope_Nom_Generacion_Finiquitos : System.Web.UI.Page
{
    #region (Page Load)
    ///**************************************************************************************************************************************************************
    /// NOMBRE DE LA FUNCION: Page_Load
    /// DESCRIPCION : Ejecuta la carga inicial de la página. Habilita la configuración inicial de los controles de la página.
    ///               así como su carga inicial.
    ///               
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 05/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///**************************************************************************************************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //A este evento solo entrara la 1ra. vez que se carga la página, pero tambien es visitado cuando se selecciona
            //algún elemento de un GridView.
            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.

                //Configuración inicial de la página.
                Configuracion_Inicial();
            }
            //Limpiamos los mensajes de error generados por algun evento anterior.
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
        }
        catch (Exception Ex) {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }
    #endregion

    #region (Métodos)

    #region (Metodos Generales)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Inicial
    ///DESCRIPCIÓN: Configuracion Inicial de los controles del Formulario.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 03/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Inicial()
    {
        Limpiar_Controles();//Limpia los controles de la pagina.
        Habilitar_Controles("Inicial");//Habilita la configuracion inicial de los controles     
        Cargar_Combo_Tipos_Nomina();//Inicializa el combo que almacena los tipos de nómina que existe actualmente, en el sistema.
        Consultar_Calendario_Nominas();//Consultamos los calendarios de nómina que existen para dados de alta en el sistema.
    }
    ///*******************************************************************************
    /// NOMBRE DE LAFUNCION: Limpiar_Controles
    /// DESCRIPCION : Limpia los Controles de la pagina.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 03/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            //Se limpian los controles de la página.
            Txt_No_Empleado.Text = "";
            Txt_Nombre_Empleado.Text = "";
            Img_Foto_Empleado.ImageUrl = "~/paginas/imagenes/paginas/Sias_No_Disponible.JPG";

            Txt_Fecha_Elaboracion.Text = String.Empty;
            Txt_Puesto.Text = String.Empty;
            Txt_Sindicato_Empleado.Text = String.Empty;
            Txt_Dependencia_Empelado.Text = String.Empty;
            Txt_Concepto_Baja.Text = String.Empty;
            Txt_Fecha_Ingreso.Text = String.Empty;
            Txt_Fecha_Baja.Text = String.Empty;
            Txt_Salario_Diario.Text = String.Empty;
            Txt_Salario_Diario_Integrado.Text = String.Empty;
            Txt_Salario_Mensual.Text = String.Empty;
            Txt_Costo_Por_Hora.Text = String.Empty;
            Txt_Codigo_Programatico.Text = String.Empty;
            Cmb_Tipo_Nomina.SelectedIndex = -1;

            Cmb_Tipo_Nomina.SelectedIndex = -1;
            Cmb_Calendario_Nomina.SelectedIndex = -1;
            Cmb_Periodo.SelectedIndex = -1;
            Txt_Inicia_Catorcena.Text = "";
            Txt_Fin_Catorcena.Text = "";

            Grid_Percepciones.DataSource = new DataTable();
            Grid_Percepciones.DataBind();
            Grid_Deducciones.DataSource = new DataTable();
            Grid_Deducciones.DataBind();
            Grid_Resguardos_Empleado.DataSource = new DataTable();
            Grid_Resguardos_Empleado.DataBind();

            Lbl_Cantidad_Salario.Text = String.Empty;
            Cmb_Tipo_Salario.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al limpiar los controles de la pagina. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///               para a siguiente operación
    /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
    ///                          si es una alta, modificacion
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 03/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado;//Variable que sirve para almacenar el estatus de los controles habilitado o deshabilitado.

        try
        {
            Habilitado = false;
            switch (Operacion)
            {
                case "Inicial":
                    Habilitado = false;
                    //Mensajes de Error.
                    Lbl_Mensaje_Error.Text = "";
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    break;
                case "Nuevo":
                    Habilitado = true;
                    break;
                case "Modificar":
                    Habilitado = true;
                    break;
            }
            //Controles Datos Empleado a Finiquitar.
            Txt_No_Empleado.Enabled = true;
            Btn_Buscar_Empleado.Visible = false;
            Txt_Nombre_Empleado.Enabled = false;
            Img_Foto_Empleado.Enabled = false;

            //Txt_Fecha_Elaboracion.Enabled = false;
            Txt_Puesto.Enabled = false;
            Txt_Sindicato_Empleado.Enabled = false;
            Txt_Dependencia_Empelado.Enabled = false;
            Txt_Concepto_Baja.Enabled = false;
            Txt_Fecha_Ingreso.Enabled = false;
            Txt_Fecha_Baja.Enabled = false;
            Txt_Salario_Diario.Enabled = false;
            Txt_Salario_Diario_Integrado.Enabled = false;
            Txt_Salario_Mensual.Enabled = false;
            Txt_Costo_Por_Hora.Enabled = false;
            Txt_Codigo_Programatico.Enabled = false;
            Cmb_Tipo_Nomina.Enabled = false;
            Cmb_Calendario_Nomina.Enabled = false;
            Cmb_Periodo.Enabled = false;
            Btn_Generar_Pre_Recibo_Finiquito.Enabled = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Mostrar_Informacion_Empleado
    ///DESCRIPCIÓN: Consulta los datos del Empleado.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 03/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Consultar_Mostrar_Informacion_Empleado()
    {
        Cls_Cat_Empleados_Negocios Consulta_Empelado = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios.
        Cls_Cat_Nom_Sindicatos_Negocio Consulta_Sindicatos = new Cls_Cat_Nom_Sindicatos_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Cat_Dependencias_Negocio Consulta_Dependencias = new Cls_Cat_Dependencias_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Cat_Tipos_Nominas_Negocio Consulta_Tipos_Nominas = new Cls_Cat_Tipos_Nominas_Negocio();//Variable de conexion con la clase de negocios.
        Cls_Ope_Nom_Pestamos_Negocio Consulta_Empleados_Prestamos = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Empleados = null;//Variable que almacenara la informacion del empelado.
        String No_Empleado = "";//Variable que almacenara en nu del empleado.
        Boolean Busqueda_Exitosa = false;
        String Fecha_Baja_Empleado = String.Empty;
        Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nomina = new Cls_Cat_Nom_Calendario_Nominas_Negocio();
        DataTable Dt_Calendario_Nomina = null;
        DateTime? Fecha_Inicio = null;
        DateTime? Fecha_Fin = null;

        try
        {
            No_Empleado = Txt_No_Empleado.Text.Trim();
            Txt_No_Empleado.Text = No_Empleado;

            if (!string.IsNullOrEmpty(Txt_No_Empleado.Text.Trim()))
            {
                Consulta_Empelado.P_No_Empleado = Txt_No_Empleado.Text.Trim();
                Consulta_Empelado.P_Estatus = "INACTIVO";
                Dt_Empleados = Consulta_Empelado.Consultar_Informacion_Mostrar_Finiquitos();

                if (Dt_Empleados is DataTable)
                {
                    if (Dt_Empleados.Rows.Count > 0)
                    {
                        foreach (DataRow EMPLEADO in Dt_Empleados.Rows)
                        {
                            if (EMPLEADO is DataRow)
                            {
                                if (!String.IsNullOrEmpty(EMPLEADO["TIPO_NOMINA"].ToString().Trim()))
                                    Txt_Clase_Nomina_Empleado.Text = EMPLEADO["TIPO_NOMINA"].ToString().Trim().ToUpper();

                                if (!String.IsNullOrEmpty(EMPLEADO["EMPLEADO"].ToString().Trim()))
                                    Txt_Nombre_Empleado.Text = EMPLEADO["EMPLEADO"].ToString().Trim().ToUpper();

                                if (!String.IsNullOrEmpty(EMPLEADO["PUESTO"].ToString().Trim()))
                                    Txt_Puesto.Text = EMPLEADO["PUESTO"].ToString().Trim().ToUpper();

                                if (!String.IsNullOrEmpty(EMPLEADO["SINDICATO"].ToString().Trim()))
                                    Txt_Sindicato_Empleado.Text = EMPLEADO["SINDICATO"].ToString().Trim().ToUpper();

                                if (!String.IsNullOrEmpty(EMPLEADO["DEPENDENCIAS"].ToString().Trim()))
                                    Txt_Dependencia_Empelado.Text = EMPLEADO["DEPENDENCIAS"].ToString().Trim();

                                if (!String.IsNullOrEmpty(EMPLEADO["INDEMNIZACION"].ToString().Trim()))
                                    Txt_Concepto_Baja.Text = EMPLEADO["INDEMNIZACION"].ToString().Trim().ToUpper();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Fecha_Inicio].ToString().Trim()))
                                    Txt_Fecha_Ingreso.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(EMPLEADO[Cat_Empleados.Campo_Fecha_Inicio].ToString().Trim()));

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Fecha_Baja].ToString().Trim()))
                                    Txt_Fecha_Baja.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(EMPLEADO[Cat_Empleados.Campo_Fecha_Baja].ToString().Trim()));

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Salario_Diario].ToString().Trim()))
                                    Txt_Salario_Diario.Text = String.Format("{0:c}", Convert.ToDouble(EMPLEADO[Cat_Empleados.Campo_Salario_Diario].ToString().Trim()));

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Salario_Diario_Integrado].ToString().Trim()))
                                    Txt_Salario_Diario_Integrado.Text = String.Format("{0:c}", Convert.ToDouble(EMPLEADO[Cat_Empleados.Campo_Salario_Diario_Integrado].ToString().Trim()));

                                if (!String.IsNullOrEmpty(EMPLEADO["SALARIO_MENSUAL"].ToString().Trim()))
                                    Txt_Salario_Mensual.Text = String.Format("{0:c}", Convert.ToDouble(EMPLEADO["SALARIO_MENSUAL"].ToString().Trim()));

                                if (!String.IsNullOrEmpty(EMPLEADO["COSTO_HORA"].ToString().Trim()))
                                    Txt_Costo_Por_Hora.Text = String.Format("{0:c}", Convert.ToDouble(EMPLEADO["COSTO_HORA"].ToString().Trim()));

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_SAP_Codigo_Programatico].ToString().Trim()))
                                    Txt_Codigo_Programatico.Text = EMPLEADO[Cat_Empleados.Campo_SAP_Codigo_Programatico].ToString().Trim();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Tipo_Nomina_ID].ToString().Trim()))
                                    Cmb_Tipo_Nomina.SelectedIndex = Cmb_Tipo_Nomina.Items.IndexOf(Cmb_Tipo_Nomina.Items.FindByValue(EMPLEADO[Cat_Empleados.Campo_Tipo_Nomina_ID].ToString().Trim()));

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Fecha_Baja].ToString().Trim()))
                                {
                                    Obj_Calendario_Nomina.P_Fecha_Busqueda_Periodo = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime((EMPLEADO[Cat_Empleados.Campo_Fecha_Baja].ToString().Trim())));
                                    Dt_Calendario_Nomina = Obj_Calendario_Nomina.Consulta_Detalle_Periodo_Actual();

                                    if (Dt_Calendario_Nomina is DataTable)
                                    {
                                        if (Dt_Calendario_Nomina.Rows.Count > 0)
                                        {
                                            foreach (DataRow CALENDARIO_NOMINA in Dt_Calendario_Nomina.Rows)
                                            {
                                                if (CALENDARIO_NOMINA is DataRow)
                                                {
                                                    if (!String.IsNullOrEmpty(CALENDARIO_NOMINA[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID].ToString().Trim()))
                                                    {
                                                        Cmb_Calendario_Nomina.SelectedIndex = Cmb_Calendario_Nomina.Items.IndexOf(Cmb_Calendario_Nomina.Items.FindByValue(CALENDARIO_NOMINA[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID].ToString().Trim()));
                                                        Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario_Nomina.SelectedValue.Trim());

                                                        if (!String.IsNullOrEmpty(CALENDARIO_NOMINA[Cat_Nom_Nominas_Detalles.Campo_No_Nomina].ToString().Trim()))
                                                        {
                                                            Cmb_Periodo.SelectedIndex = Cmb_Periodo.Items.IndexOf(Cmb_Periodo.Items.FindByText(CALENDARIO_NOMINA[Cat_Nom_Nominas_Detalles.Campo_No_Nomina].ToString().Trim()));

                                                            if (Cmb_Periodo.SelectedIndex > 0)
                                                            {
                                                                Obj_Calendario_Nomina.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
                                                                Obj_Calendario_Nomina.P_No_Nomina = Convert.ToInt32(Cmb_Periodo.SelectedItem.Text.Trim());
                                                                DataTable Dt_Detalles_Nomina = Obj_Calendario_Nomina.Consultar_Periodos_Por_Nomina_Periodo();

                                                                if (Dt_Detalles_Nomina is DataTable)
                                                                {
                                                                    if (Dt_Detalles_Nomina.Rows.Count > 0)
                                                                    {
                                                                        foreach (DataRow DETALLE_NOMINA in Dt_Detalles_Nomina.Rows)
                                                                        {
                                                                            if (DETALLE_NOMINA is DataRow)
                                                                            {
                                                                                if (!String.IsNullOrEmpty(DETALLE_NOMINA[Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString().Trim()))
                                                                                    Fecha_Inicio = Convert.ToDateTime(DETALLE_NOMINA[Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString().Trim());
                                                                                if (!String.IsNullOrEmpty(DETALLE_NOMINA[Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString().Trim()))
                                                                                    Fecha_Fin = Convert.ToDateTime(DETALLE_NOMINA[Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString().Trim());

                                                                                Txt_Inicia_Catorcena.Text = string.Format("{0:dd/MMM/yyyy}", ((DateTime)Fecha_Inicio));
                                                                                Txt_Fin_Catorcena.Text = string.Format("{0:dd/MMM/yyyy}", ((DateTime)Fecha_Fin));

                                                                                Txt_No_Empleado.Enabled = true;
                                                                                Btn_Buscar_Empleado.Enabled = true;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                Txt_No_Empleado.Enabled = false;
                                                                Btn_Buscar_Empleado.Enabled = false;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Informacion", "alert('Información: El empleado a generar el finiquito aún se encuentra activo en la nómina. Ir al catalogo de baja de los empleados y hacer la baja del empleado');", true);
                                    return false; }

                                Txt_Fecha_Elaboracion.Text = String.Format("{0:dd/MMMM/yyyy}", DateTime.Today);

                                Img_Foto_Empleado.ImageUrl = (string.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Ruta_Foto].ToString().Trim())) ? "~/paginas/imagenes/paginas/Sias_No_Disponible.JPG" : @EMPLEADO[Cat_Empleados.Campo_Ruta_Foto].ToString().Trim();
                                Img_Foto_Empleado.DataBind();

                                Busqueda_Exitosa = true;
                            }
                        }
                    }
                    else {
                        Limpiar_Controles();
                        Habilitar_Controles("Inicial");
                    }
                }
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Ingrese el numero de empleado que desea buscar.";
            }
            return Busqueda_Exitosa;
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Percepciones_Deducciones_Aplican_Empleado
    ///
    ///DESCRIPCIÓN: Consulta ls todas las percepciones y/o deducciones que aplican para
    ///             el empleado. ya viene conm lo montos que le corresponden a cada
    ///             concepto.
    ///             
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 03/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Percepciones_Deducciones_Aplican_Empleado() {
        List<List<Cls_Percepciones_Deducciones>> Coleccion_Listas_Percepciones_Deducciones = new List<List<Cls_Percepciones_Deducciones>>();//Almacenar una lista de deducciones y/o percepciones.
        Cls_Ope_Nom_Finiquitos_Negocio Finiquitos_Negocio = new Cls_Ope_Nom_Finiquitos_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Cat_Empleados_Negocios Empleados_Negocio = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios.
        DataTable Dt_Empleados = null;//Registro con los datos del Empleado al cuál se re relizara su finiquito.
        DataSet Ds_Tablas_Percepciones_Deducciones = null;//Estructura que almacenera las tablas de Percepciones y/o Deducciones que aplicán al finiquito del empleado.
        DataTable Dt_Percepciones = null;//Lista de Percepciones que le aplican al empleado para sus finiquito.
        DataTable Dt_Deducciones = null;//Lista de Deducciones que le aplican al empleado para sus finiquito.        
        String Empleado_ID = "";//Identificador único del empleado, por el que se realizan todas las operaciones internas en el sistema.        
        DateTime Fecha;//Fecha para validar la generación del finiquito del empleado.
        DataTable Dt_Conceptos_Finiquitos = new DataTable();

        try
        {
            //Obtenemos la fecha para la generación del finiquito del empleado.
            Fecha = Convert.ToDateTime(Txt_Fin_Catorcena.Text.Trim());
            //Consultamos los datos del empleado.
            Empleados_Negocio.P_No_Empleado = Txt_No_Empleado.Text.Trim();
            Dt_Empleados = Empleados_Negocio.Consulta_Empleados_General();
            //Válidamos los resultados obtenidos de la búsqueda.
            if (Dt_Empleados != null)
                if (Dt_Empleados.Rows.Count > 0)
                    if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString()))
                        Empleado_ID = Dt_Empleados.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString();//Obtenemos el Identificador del empleado.

            //Obtenemos los valores que son necesarios para realizar la generación de la nómina.
            Finiquitos_Negocio.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
            Finiquitos_Negocio.P_Detalle_Nomina_ID = Cmb_Periodo.SelectedValue.Trim();
            Finiquitos_Negocio.P_No_Nomina = Convert.ToInt32(Cmb_Periodo.SelectedItem.Text.Trim());
            Finiquitos_Negocio.P_Tipo_Nomina_ID = Cmb_Tipo_Nomina.SelectedValue.Trim();
            Finiquitos_Negocio.P_Tipo_Salario = Cmb_Tipo_Salario.SelectedItem.Text.Trim();
           
            //Obtenemos un lista que almacena por cada elemento otra lista de percepciones y/o deducciones que le aplican al empleado actualmente.
            Coleccion_Listas_Percepciones_Deducciones = Finiquitos_Negocio.Obtener_Percepiones_IO_Deducciones(Empleado_ID, Fecha);
            //Apartir de la Estructura de listas, obtenemos una DataSet con las tablas divididas por el tipo [Percepciones y/o Deducciones].
            Ds_Tablas_Percepciones_Deducciones = Finiquitos_Negocio.Obtener_Tablas_Percepciones_Deducciones(Coleccion_Listas_Percepciones_Deducciones);

           Dt_Percepciones = Ds_Tablas_Percepciones_Deducciones.Tables["PERCEPCION"];//Obtenemos del DataSet la tabla de percepciones. Que podrán aplicarle al empleado.
           Dt_Deducciones = Ds_Tablas_Percepciones_Deducciones.Tables["DEDUCCION"];//Obtenemos del DataSet la tabla de deducciones. Que podrán aplicarle al empleado.

           Obtener_Percepciones_Deducciones_Propias_Finiquito(Dt_Percepciones, ref Dt_Conceptos_Finiquitos);
           Obtener_Percepciones_Deducciones_Propias_Finiquito(Dt_Deducciones, ref Dt_Conceptos_Finiquitos);

           //Validamos que por lo menos aplique alguna percepción para el finiquito del empleado.
           if (Dt_Percepciones != null) {
               if (Dt_Percepciones.Rows.Count > 0) {
                   Grid_Percepciones.Columns[1].Visible = true;
                   Grid_Percepciones.Columns[6].Visible = true;
                   Grid_Percepciones.Columns[7].Visible = true;
                   Dt_Percepciones.DefaultView.Sort = "TIPO_ASIGNACION ASC";
                   Consultar_Clave_Pecepcion_Deduccion(ref Dt_Percepciones);
                   //Cargamos el grid de percepciones con las percepciones que podrán aplicar para el finiquito del empleado.
                   Grid_Percepciones.DataSource = Dt_Percepciones;
                   Grid_Percepciones.DataBind();
                   Grid_Percepciones.Columns[1].Visible = false;
                   Grid_Percepciones.Columns[6].Visible = false;
                   Grid_Percepciones.Columns[7].Visible = false;
                   //Cargamos las cantidades en el TextBox interno dentro del GridView
                   Cargar_Cantidad_Grid_Percepciones_Deducciones(Grid_Percepciones, Dt_Percepciones, "Txt_Cantidad_Percepcion");
               }
           }

           //Validamos que por lo menos aplique alguna deducción para el finiquito del empleado.
           if (Dt_Deducciones != null)
           {
               if (Dt_Deducciones.Rows.Count > 0)
               {
                   Grid_Deducciones.Columns[1].Visible = true;
                   Dt_Deducciones.DefaultView.Sort = "TIPO_ASIGNACION ASC";
                   Consultar_Clave_Pecepcion_Deduccion(ref Dt_Deducciones);
                   //Cargamos el grid de deducciones con las deducciones que podrán aplicar para el finiquito del empleado
                   Grid_Deducciones.DataSource = Dt_Deducciones;
                   Grid_Deducciones.DataBind();
                   Grid_Deducciones.Columns[1].Visible = false;
                   //Cargamos las cantidades en el TextBox interno dentro del GridView
                   Cargar_Cantidad_Grid_Percepciones_Deducciones(Grid_Deducciones, Dt_Deducciones, "Txt_Cantidad_Deduccion");
               }
           }

           if (Dt_Conceptos_Finiquitos is DataTable) {
               if (Dt_Conceptos_Finiquitos.Rows.Count > 0) {
                   Grid_Conceptos_Exclusivos_Finiquitos.Columns[1].Visible = true;
                   Grid_Conceptos_Exclusivos_Finiquitos.Columns[6].Visible = true;
                   Grid_Conceptos_Exclusivos_Finiquitos.Columns[7].Visible = true;
                   Consultar_Clave_Pecepcion_Deduccion(ref Dt_Conceptos_Finiquitos);
                   Grid_Conceptos_Exclusivos_Finiquitos.DataSource = Dt_Conceptos_Finiquitos;
                   Grid_Conceptos_Exclusivos_Finiquitos.DataBind();
                   Grid_Conceptos_Exclusivos_Finiquitos.Columns[1].Visible = false;
                   Grid_Conceptos_Exclusivos_Finiquitos.Columns[6].Visible = false;
                   Grid_Conceptos_Exclusivos_Finiquitos.Columns[7].Visible = false;

                   Cargar_Cantidad_Grid_Percepciones_Deducciones(Grid_Conceptos_Exclusivos_Finiquitos, Dt_Conceptos_Finiquitos, "Txt_Cantidad_Deduccion_Percepcion");
               }
           }

           Ocultar_Percepciones_Exclusivas_Finiquito(Dt_Conceptos_Finiquitos, ref Grid_Percepciones);
           Ocultar_Percepciones_Exclusivas_Finiquito(Dt_Conceptos_Finiquitos, ref Grid_Deducciones);

            //************* PERCEPCIONES Y DEDUCCIONES POR SISTEMA ************************
           Session["TABLA_PERCEPCIONES"] = Crear_DataTable_Percepciones_Deducciones(Grid_Percepciones, "Txt_Cantidad_Percepcion", "Chk_Aplica_Deduccion_Finiquito"); ;
           Session["TABLA_DEDUCCIONES"] = Crear_DataTable_Percepciones_Deducciones(Grid_Deducciones, "Txt_Cantidad_Deduccion", "Chk_Aplica_Percepcion_Finiquito");
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las Percepciones y/o Deducciones que le aplican al empleado. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Resguardos_Empleado_Actualmente
    ///
    ///DESCRIPCIÓN: Consulta los resguardos que tiene el empleado actualmente, esta 
    ///             información nos será de utlidad, para determinar si el finiquito
    ///             debe hacerse, o cancelarse hasta que los resguardos sean devueltos.
    ///             
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 03/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Resguardos_Empleado_Actualmente()
    {
        Cls_Ope_Nom_Finiquitos_Negocio Obj_Generar_Finiquito = new Cls_Ope_Nom_Finiquitos_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Cat_Empleados_Negocios Obj_Empleados_Negocio = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios.
        Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Obj_Bienes_Muebles = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Ope_Pat_Com_Cemovientes_Negocio Obj_Cemoviente = new Cls_Ope_Pat_Com_Cemovientes_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Ope_Pat_Com_Vehiculos_Negocio Obj_Vehiculos = new Cls_Ope_Pat_Com_Vehiculos_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio Obj_Partes_Vehiculos = new Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio();//Variablde de conexion con la capa de negocios.

        DataTable Dt_Empleados = null;//Registro con los datos del Empleado al cuál se desae realizar el cálculo de su finiquito..
        DataTable Dt_Generico = null;//Variable que almacenara los resguardos que a tenido el empleado actualmente.
        DataTable Dt_Almacen_Resguardos_Empleado = null;//almacena todos los resguardos que tiene actualmente el empleado.
        String Empleado_ID = String.Empty;              //Clave únika del empleado.
        String Dependencia_ID = String.Empty;           //Clave única de la dependencia.
        String Tipo_DataTable = String.Empty;           //Tipo de reesguardo que tiene a su cargo el empleado.
        String Tipo_Filtro_Busqueda = String.Empty;     //Buscar por reeguardante.

        try
        {
            /**Inicializamos la tabla que almacenara las columnas con la
             * información que se le mostrara al usuario que tendra a su
             * cargo la responsabilidad de validar y verificar dicha inf.
            **/
            Dt_Almacen_Resguardos_Empleado = new DataTable("RESGUARDOS_EMPLEADO");
            Dt_Almacen_Resguardos_Empleado.Columns.Add("CLAVE_PRODUCTO", typeof(String));
            Dt_Almacen_Resguardos_Empleado.Columns.Add("NOMBRE_PRODUCTO", typeof(String));
            Dt_Almacen_Resguardos_Empleado.Columns.Add("ESTATUS", typeof(String));
            Dt_Almacen_Resguardos_Empleado.Columns.Add("URL", typeof(String));
            Dt_Almacen_Resguardos_Empleado.Columns.Add("TIPO", typeof(String));
            //Consultamos la información del empleado para obtener su identificador.
            Obj_Empleados_Negocio.P_No_Empleado = Txt_No_Empleado.Text.Trim();
            //Obj_Empleados_Negocio.P_Tipo_Nomina_ID = Cmb_Tipo_Nomina.SelectedValue.Trim();
            Dt_Empleados = Obj_Empleados_Negocio.Consulta_Empleados_General();
            //Obtener la clave del empleado.
            if (Dt_Empleados is DataTable)
            {
                if (Dt_Empleados.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString()))
                        Empleado_ID = Dt_Empleados.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString();

                    if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Dependencia_ID].ToString()))
                        Dependencia_ID = Dt_Empleados.Rows[0][Cat_Empleados.Campo_Dependencia_ID].ToString();

                    Tipo_DataTable = "BIENES";
                    Tipo_Filtro_Busqueda = "RESGUARDANTES";
                    Obj_Bienes_Muebles.P_Tipo_DataTable = Tipo_DataTable;
                    Obj_Bienes_Muebles.P_Tipo_Filtro_Busqueda = Tipo_Filtro_Busqueda;
                    Obj_Bienes_Muebles.P_Resguardante_ID = Empleado_ID;
                    Obj_Bienes_Muebles.P_Dependencia_ID = Dependencia_ID;
                    Dt_Generico = Obj_Bienes_Muebles.Consultar_DataTable();
                    Llenar_Tabla_Resguardos_Empleado(Dt_Generico, ref Dt_Almacen_Resguardos_Empleado, Tipo_DataTable);

                    Tipo_DataTable = "SEMOVIENTES";
                    Tipo_Filtro_Busqueda = "RESGUARDANTES";
                    Obj_Cemoviente.P_Tipo_DataTable = Tipo_DataTable;
                    Obj_Cemoviente.P_Tipo_Filtro_Busqueda = Tipo_Filtro_Busqueda;
                    Obj_Cemoviente.P_Resguardante_ID = Empleado_ID;
                    Obj_Cemoviente.P_Dependencia_ID = Dependencia_ID;
                    Dt_Generico = Obj_Cemoviente.Consultar_DataTable();
                    Llenar_Tabla_Resguardos_Empleado(Dt_Generico, ref Dt_Almacen_Resguardos_Empleado, Tipo_DataTable);

                    Tipo_DataTable = "VEHICULOS";
                    Tipo_Filtro_Busqueda = "RESGUARDANTES";
                    Obj_Vehiculos.P_Tipo_DataTable = Tipo_DataTable;
                    Obj_Vehiculos.P_Tipo_Filtro_Busqueda = Tipo_Filtro_Busqueda;
                    Obj_Vehiculos.P_Resguardante_ID = Empleado_ID;
                    Obj_Vehiculos.P_Dependencia_ID = Dependencia_ID;
                    Dt_Generico = Obj_Vehiculos.Consultar_DataTable();
                    Llenar_Tabla_Resguardos_Empleado(Dt_Generico, ref Dt_Almacen_Resguardos_Empleado, Tipo_DataTable);
                }
                else {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "El Empleado no existe o  no se encuentra en el tipo de nomina seleccionado";
                }
            }

            //Obtenemos los detalles de los resguardos que tiene el empleado actualmente.
            if (Dt_Almacen_Resguardos_Empleado != null)
            {
                //Cargamos el el grid de resguardos del empleado.
                Grid_Resguardos_Empleado.Columns[0].Visible = true;
                Grid_Resguardos_Empleado.DataSource = Dt_Almacen_Resguardos_Empleado;
                Grid_Resguardos_Empleado.DataBind();
                Grid_Resguardos_Empleado.Columns[0].Visible = false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los resguardos que tiene el empleado actualmete. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Resguardos_Empleado
    ///
    ///DESCRIPCIÓN: Carga una estructura del tipo DatTable con la informacion de los
    ///             los reesguardos que tiene el empleado bajo su reesponsabilidad.
    ///             
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 25/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Resguardos_Empleado(DataTable Dt_Generico, ref DataTable Dt_Almacen_Resguardos_Empleado, String Tipo_Data_Table)
    {
        DataRow Registro_Resguardo = null;//Variable que almacena un registro de resguardo que tiene el empleado.

        try
        {
            if (Dt_Generico is DataTable) {
                if (Dt_Generico.Rows.Count > 0) {
                    foreach (DataRow Fila in Dt_Generico.Rows) {
                        if (Fila is DataRow) {
                            Registro_Resguardo = Dt_Almacen_Resguardos_Empleado.NewRow();
                            switch (Tipo_Data_Table)
                            {
                                case "BIENES":
                                    if (!string.IsNullOrEmpty(Fila["BIEN_MUEBLE_ID"].ToString()))
                                        Registro_Resguardo["CLAVE_PRODUCTO"] = Fila["BIEN_MUEBLE_ID"].ToString();

                                    if (!string.IsNullOrEmpty(Fila["NOMBRE_PRODUCTO"].ToString()))
                                        Registro_Resguardo["NOMBRE_PRODUCTO"] = Fila["NOMBRE_PRODUCTO"].ToString();

                                    if (!string.IsNullOrEmpty(Fila["ESTADO"].ToString()))
                                        Registro_Resguardo["ESTATUS"] = Fila["ESTADO"].ToString();

                                    Registro_Resguardo["URL"] = "../Imagenes/paginas/Sias_Bien_Mueble.jpg";
                                    Registro_Resguardo["TIPO"] = "BIEN MUEBLE";
                                    break;
                                case "SEMOVIENTES":
                                    if (!string.IsNullOrEmpty(Fila["CEMOVIENTE_ID"].ToString()))
                                        Registro_Resguardo["CLAVE_PRODUCTO"] = Fila["CEMOVIENTE_ID"].ToString();

                                    if (!string.IsNullOrEmpty(Fila["RAZA"].ToString()))
                                        Registro_Resguardo["NOMBRE_PRODUCTO"] = Fila["RAZA"].ToString();

                                    if (!string.IsNullOrEmpty(Fila["ESTATUS"].ToString()))
                                        Registro_Resguardo["ESTATUS"] = Fila["ESTATUS"].ToString();

                                    Registro_Resguardo["URL"] = "../Imagenes/paginas/Sias_Semoviente.jpg";
                                    Registro_Resguardo["TIPO"] = "SEMOVIENTE";
                                    break;
                                case "VEHICULOS":
                                    if (!string.IsNullOrEmpty(Fila["VEHICULO_ID"].ToString()))
                                        Registro_Resguardo["CLAVE_PRODUCTO"] = Fila["VEHICULO_ID"].ToString();

                                    if (!string.IsNullOrEmpty(Fila["VEHICULO"].ToString()))
                                        Registro_Resguardo["NOMBRE_PRODUCTO"] = Fila["VEHICULO"].ToString();

                                    if (!string.IsNullOrEmpty(Fila["ESTATUS"].ToString()))
                                        Registro_Resguardo["ESTATUS"] = Fila["ESTATUS"].ToString();

                                    Registro_Resguardo["URL"] = "../Imagenes/paginas/Sias_Vehiculo.jpg";
                                    Registro_Resguardo["TIPO"] = "VEHICULO";
                                    break;
                                default:
                                    break;
                            }

                            Dt_Almacen_Resguardos_Empleado.Rows.Add(Registro_Resguardo);
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al llenar la tabla de con los reesguardos que tiene el empleado. Error: [" + Ex.Message + "]");
        }
    }
    ///**************************************************************************************************************************************************************
    /// NOMBRE DE LA FUNCION: Sumar_Percepciones_IO_Deducciones
    /// DESCRIPCION : Hace un barrido de todas las percepciones y/o deducciones para otener una cantidad o monto total de percepciones y/o deducciones
    ///               que le aplicaron al empleado para el cálculo del finiquito del empleado.
    ///               
    /// PARÁMETROS: Dt_Percepciones_Deducciones .- Conceptos de tipo Percepción y/o Deducción que le aplicán al empleado para el cálculo de su finiquito.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 05/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///**************************************************************************************************************************************************************
    private Double Sumar_Percepciones_IO_Deducciones(DataTable Dt_Percepciones_Deducciones, String Tipo)
    {
        Double Total = 0.0;//Variable que almacena el total de Percepiones y/o Deducciones.
        try
        {
            if (Dt_Percepciones_Deducciones is DataTable)
            {
                foreach (DataRow Percepcion_Deduccion in Dt_Percepciones_Deducciones.Rows)
                {
                    if (Percepcion_Deduccion is DataRow)
                    {
                        //Se valida si el monto que se desea sumar corresponde. [Monto, Grava ó Exento].
                        switch (Tipo)
                        {
                            case "Monto":
                                if (!string.IsNullOrEmpty(Percepcion_Deduccion["Monto"].ToString()))
                                {
                                    Total += Convert.ToDouble(Percepcion_Deduccion["Monto"].ToString());
                                }
                                break;
                            case "Grava":
                                if (!string.IsNullOrEmpty(Percepcion_Deduccion["Grava"].ToString()))
                                {
                                    Total += Convert.ToDouble(Percepcion_Deduccion["Grava"].ToString());
                                }
                                break;
                            case "Exenta":
                                if (!string.IsNullOrEmpty(Percepcion_Deduccion["Exenta"].ToString()))
                                {
                                    Total += Convert.ToDouble(Percepcion_Deduccion["Exenta"].ToString());
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al obtener el total de Percepciones a considerar para el finiquito del empleado. Error: [" + Ex.Message + "]");
        }
        return Total;
    }
    ///**************************************************************************************************************************************************************
    /// NOMBRE DE LA FUNCION: Integrar_Percepciones_Deducciones_Una_Tabla
    /// DESCRIPCION : Hace un barrido de todas las percepciones y/o deducciones para crear una tabla que las conjunta en una solo, para
    ///               posteriormente poder generar el recibo con los detalles que le aplicarán al empleado por concepto de su finiquito.
    ///               
    /// PARÁMETROS: Dt_Percepciones .- Conceptos de tipo Percepción que le aplicán al empleado para el cálculo de su finiquito.
    ///             Dt_Deducciones.- Conceptos de tipo Deducción que le aplicán al empleado para el cálculo de su finiquito.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 05/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///**************************************************************************************************************************************************************
    private DataTable Integrar_Percepciones_Deducciones_Una_Tabla(DataTable Dt_Percepciones, DataTable Dt_Deducciones)
    {
        DataTable Dt_Percepciones_Deducciones = new DataTable("CONCEPTOS_PERCEPCIONES_DEDUCCIONES");//Listado de Percepciones y/o Deducciones que se devolveran al empleado.
        //Se crean las columnas, de la tabla que almacenara todos los conceptos que aplican para
        //el finiquito del empleado.
        Dt_Percepciones_Deducciones.Columns.Add("Percepcion_Deduccion", typeof(System.String));//Columna para Almacenar el Identificador del concepto.
        Dt_Percepciones_Deducciones.Columns.Add("Monto", typeof(System.String));//Columna para almacenar los montos o cantidad.
        Dt_Percepciones_Deducciones.Columns.Add("Grava", typeof(System.String));//Columna para almacenar el gravante generado por el concepto.
        Dt_Percepciones_Deducciones.Columns.Add("Exenta", typeof(System.String));//Columna que almacenara el exento generado por el concepto.
        DataRow Concepto_Percepcion_IO_Deduccion = null;//Hace referencia a un fila o registro que almacenará los datos de la información de la Percepción y/o Deducción.

        try
        {

            ///Hacemos un barrido de todas las PERCEPCIONES que le aplicarán al empleado
            ///por concepto de su finiquito.
            if (Dt_Percepciones is DataTable)
            {
                foreach (DataRow Concepto_Temporal in Dt_Percepciones.Rows)
                {
                    if (Concepto_Temporal is DataRow)
                    {
                        //Obtenemos una fila clón, de la tabla de Percepciones_IO_ Deducciones.
                        Concepto_Percepcion_IO_Deduccion = Dt_Percepciones_Deducciones.NewRow();

                        //Identificador Concepto.
                        if (!string.IsNullOrEmpty(Concepto_Temporal["Percepcion_Deduccion"].ToString()))
                            Concepto_Percepcion_IO_Deduccion["Percepcion_Deduccion"] = Concepto_Temporal["Percepcion_Deduccion"].ToString();
                        //Monto Concepto
                        if (!string.IsNullOrEmpty(Concepto_Temporal["Monto"].ToString()))
                            Concepto_Percepcion_IO_Deduccion["Monto"] = Concepto_Temporal["Monto"].ToString();
                        //Grava Concepto
                        if (!string.IsNullOrEmpty(Concepto_Temporal["Grava"].ToString()))
                            Concepto_Percepcion_IO_Deduccion["Grava"] = Concepto_Temporal["Grava"].ToString();
                        //Exento Concepto
                        if (!string.IsNullOrEmpty(Concepto_Temporal["Exenta"].ToString()))
                            Concepto_Percepcion_IO_Deduccion["Exenta"] = Concepto_Temporal["Exenta"].ToString();

                        //Insertamos el registro o fila en la tabla de Percepciones y/o Deducciones.
                        Dt_Percepciones_Deducciones.Rows.Add(Concepto_Percepcion_IO_Deduccion);
                    }
                }
            }
            ///Hacemos un barrido de todas las DEDUCCIONES que le aplicarán al empleado
            ///por concepto de su finiquito.
            if (Dt_Deducciones is DataTable)
            {
                foreach (DataRow Concepto_Temporal in Dt_Deducciones.Rows)
                {
                    if (Concepto_Temporal is DataRow)
                    {
                        //Obtenemos una fila clón, de la tabla de Percepciones_IO_ Deducciones.
                        Concepto_Percepcion_IO_Deduccion = Dt_Percepciones_Deducciones.NewRow();

                        //Identificador Concepto.
                        if (!string.IsNullOrEmpty(Concepto_Temporal["Percepcion_Deduccion"].ToString()))
                            Concepto_Percepcion_IO_Deduccion["Percepcion_Deduccion"] = Concepto_Temporal["Percepcion_Deduccion"].ToString();
                        //Monto Concepto
                        if (!string.IsNullOrEmpty(Concepto_Temporal["Monto"].ToString()))
                            Concepto_Percepcion_IO_Deduccion["Monto"] = Concepto_Temporal["Monto"].ToString();

                        //Insertamos el registro o fila en la tabla de Percepciones y/o Deducciones.
                        Dt_Percepciones_Deducciones.Rows.Add(Concepto_Percepcion_IO_Deduccion);
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error generado al integrar la percepciones y/o deducciones en una sola tabla. Error: [" + Ex.Message + "]");
        }
        return Dt_Percepciones_Deducciones;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Es_Numerico
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 6/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Es_Numerico(String Cadena)
    {
        Boolean Resultado = true;//Almacena el resultado true si es numerica o false si no lo es.
        Char[] Array = Cadena.ToCharArray();//Obtenemos un arreglo de carácteres a partir de la cadena que se recibio como parámetro el método.

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
    /// NOMBRE DE LA FUNCION: Formato_Fecha_Calendario_Nomina
    /// DESCRIPCION : Crea el DataTable con la consulta de las nomina vigentes en el 
    /// sistema.
    /// PARAMETROS: Dt_Calendario_Nominas.- Lista de las nominas vigentes actualmente 
    ///             en el sistema.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 6/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private DataTable Formato_Fecha_Calendario_Nomina(DataTable Dt_Calendario_Nominas)
    {
        DataTable Dt_Nominas = new DataTable();//Variable que almacenara los calendarios de nóminas.
        DataRow Renglon_Dt_Clon = null;//Variable que almacenará un renglón del calendario de la nómina.

        //Creamos las columnas.
        Dt_Nominas.Columns.Add("Nomina", typeof(System.String));
        Dt_Nominas.Columns.Add(Cat_Nom_Calendario_Nominas.Campo_Nomina_ID, typeof(System.String));

        foreach (DataRow Renglon in Dt_Calendario_Nominas.Rows)
        {
            Renglon_Dt_Clon = Dt_Nominas.NewRow();
            Renglon_Dt_Clon["Nomina"] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Fecha_Fin].ToString().Split(new char[] { ' ' })[0].Split(new char[] { '/' })[2];
            Renglon_Dt_Clon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID];
            Dt_Nominas.Rows.Add(Renglon_Dt_Clon);
        }
        return Dt_Nominas;
    }

    protected Double Obtener_Cantidad(DataTable Dt_Conceptos)
    {
        Double Cantidad = 0.0;

        try
        {
            if (Dt_Conceptos is DataTable)
            {
                if (Dt_Conceptos.Rows.Count > 0)
                {
                    foreach (DataRow CONCEPTO in Dt_Conceptos.Rows)
                    {
                        if (CONCEPTO is DataRow)
                        {
                            if (!String.IsNullOrEmpty(CONCEPTO[0].ToString().Trim()))
                            {
                                Cantidad = Convert.ToDouble(CONCEPTO[0].ToString().Trim());
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al obtener el total del percepciones o deducciones. Error: [" + Ex.Message + "]");
        }
        return Cantidad;
    }

    protected String Convertir_Cantidad_Letras(Double Cantidad_Numero)
    {
        Numalet Obj_Numale = new Numalet();
        String Cantidad_Letra = String.Empty;

        try
        {
            Obj_Numale.MascaraSalidaDecimal = "centavos";
            Obj_Numale.SeparadorDecimalSalida = "pesos con";
            Obj_Numale.LetraCapital = true;
            Obj_Numale.ConvertirDecimales = true;
            Obj_Numale.Decimales = 2;
            Obj_Numale.CultureInfo = new CultureInfo("es-MX");
            Obj_Numale.ApocoparUnoParteEntera = true;
            Cantidad_Letra = Obj_Numale.ToCustomCardinal(Cantidad_Numero).Trim().ToUpper();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al convertir la cantidad a letras. Error:[" + Ex.Message + "]");
        }
        return Cantidad_Letra;
    }

    protected void Agregar_Total_Letra(ref DataTable Dt_Conceptos, Double Total)
    {
        try
        {
            if (Dt_Conceptos is DataTable)
            {
                if (Dt_Conceptos.Rows.Count > 0)
                {
                    foreach (DataRow CONCEPTO in Dt_Conceptos.Rows)
                    {
                        if (CONCEPTO is DataRow)
                        {
                            CONCEPTO["TOTAL_LETRA"] = Convertir_Cantidad_Letras(Total).Trim().ToUpper();
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al guardar el total del finiquito en letra. Error: [" + Ex.Message + "]");
        }
    }

    protected String Crear_Listado_Conceptos(DataTable Dt_Conceptos)
    {
        StringBuilder Lista_Percepciones_Deducciones = null;
        Int32 Contador_Filas = 0;

        try
        {
            Lista_Percepciones_Deducciones = new StringBuilder();

            if (Dt_Conceptos is DataTable)
            {
                if (Dt_Conceptos.Rows.Count > 0)
                {
                    foreach (DataRow CONCEPTO in Dt_Conceptos.Rows)
                    {
                        if (CONCEPTO is DataRow)
                        {
                            if (!String.IsNullOrEmpty(CONCEPTO["NOMBRE"].ToString().Trim().ToUpper()))
                            {

                                if (Contador_Filas == 0)
                                    Lista_Percepciones_Deducciones.Append(CONCEPTO["NOMBRE"].ToString().Trim().ToUpper());
                                if (Contador_Filas > 0)
                                    Lista_Percepciones_Deducciones.Append(", " + CONCEPTO["NOMBRE"].ToString().Trim().ToUpper());
                                if (Contador_Filas == (Dt_Conceptos.Rows.Count - 1))
                                    Lista_Percepciones_Deducciones.Append(" Y " + CONCEPTO["NOMBRE"].ToString().Trim().ToUpper());

                                Contador_Filas++;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al crear el listado de conceptos. Error: [" + Ex.Message + "]");
        }
        return Lista_Percepciones_Deducciones.ToString();
    }

    #endregion

    #region (Metodos Manejo Cantidades Grids)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Crear_DataTable_Percepciones_Deducciones
    /// DESCRIPCION : Crea un datatable con la informacion de del id de la percepcion
    ///               y la cantidad asignada para la percepcion.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 04/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private DataTable Crear_DataTable_Percepciones_Deducciones(GridView _GridView, String TextBox_ID, String CheckBox_ID)
    {
        DataTable Dt_Percepciones_Deducciones = new DataTable("PERCEPCIONES_IO_DEDUCCIONES");//Estructura que almacenará la tabla de percepciones y/o deducciones.
        //Se crea la estructura de columnas que tendrá la tabla [PERCEPCIONES_IO_DEDUCCIONES].
        Dt_Percepciones_Deducciones.Columns.Add("Percepcion_Deduccion", typeof(System.String));//Columna que almacenará el identificador único del concepto aplicar.
        Dt_Percepciones_Deducciones.Columns.Add("Monto", typeof(System.String));//Columna que almacenará la cantidad por motivo del concepto aplicado [Percepción y/ Deducción].
        Dt_Percepciones_Deducciones.Columns.Add("Grava", typeof(System.String));//Columna que almacena la cantidad que grava el concepto, si se trata de una percepción.
        Dt_Percepciones_Deducciones.Columns.Add("Exenta", typeof(System.String));//Columna que almacena la cantidad que exenta el concepto, si se trata de una percepción.
        DataRow Concepto_Percepcion_IO_Deduccion;//Fila o registro que almacena la cantidad a otorgar o descontar al realizar el finiquito del empleado.
        Boolean Aplica = false;//Variable que almacena el estatus si la percepción y/o deducción aplicara para el cálculo del finiquito del empleado.

        try
        {
            //Recorremos el Grid, que fue pasado como parámetro al método. y itera tantas veces como filas 
            //contienen el mismo.
            for (int index = 0; index < _GridView.Rows.Count; index++)
            {
                //Obtenemos el control CheckBox de columna cero del GridView, y obtenemos el estatus que 
                //almacena. si es true, significa que el concepto aplicará al finiquito, por lo tanto, obtenemos
                //el monto a otorgar o a descontar. Si el estatus es false, el concepto no es tomado en cuenta
                //para el cálculo del finiquito al empleado.
                if (((CheckBox)_GridView.Rows[index].Cells[0].FindControl(CheckBox_ID)).Checked)
                {
                    //Las columnas de Percepcion_Deduccion & Monto, aplican tanto para las percepciones como para las deducciones.
                    Concepto_Percepcion_IO_Deduccion = Dt_Percepciones_Deducciones.NewRow();
                    Concepto_Percepcion_IO_Deduccion["Percepcion_Deduccion"] = _GridView.Rows[index].Cells[1].Text;
                    Concepto_Percepcion_IO_Deduccion["Monto"] =
                        ((TextBox)_GridView.Rows[index].Cells[5].FindControl(TextBox_ID + "_Real")).Text.Trim().Replace("$", "").Replace("&nbsp;", "");

                    if (_GridView.Columns.Count > 6)
                    {
                        //Las columnas de Grava & Exenta, solo aplicaran para las percepciones.
                        Concepto_Percepcion_IO_Deduccion["Grava"] = (_GridView.Rows[index].Cells[6].Text.Trim().Replace("$", "").Replace("&nbsp;", ""));
                        Concepto_Percepcion_IO_Deduccion["Exenta"] = (_GridView.Rows[index].Cells[7].Text.Trim().Replace("$", "").Replace("&nbsp;", ""));
                    }
                    //Por último se inserta el concepto capturado en la tabla de percepciones y/o deducciones. 
                    Dt_Percepciones_Deducciones.Rows.Add(Concepto_Percepcion_IO_Deduccion);
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al obtener las cantidades de Percepciones y/o Deducciones con su respectiva cantidad. Error: [" + Ex.Message + "]");
        }
        return Dt_Percepciones_Deducciones;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar_Cantidad_Grid_Percepciones_Deducciones
    /// DESCRIPCION : Carga la cantidad correspodiente a la percepcion o deduccion 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 04/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Cantidad_Grid_Percepciones_Deducciones(GridView Grid_Percepcion_Deduccion, DataTable Dt_Datos_Consultados, String TextBox_ID)
    {
        String Percepcion_Deduccion_ID = String.Empty;
        DataRow []Dr_Registros_Encontrados = null  ;

        try
        {
            if (Grid_Percepcion_Deduccion is GridView) {
                if (Grid_Percepcion_Deduccion.Rows.Count > 0) {
                    foreach (GridViewRow FILA_GRID in Grid_Percepcion_Deduccion.Rows) {
                        if (FILA_GRID is GridViewRow) {
                            if (!String.IsNullOrEmpty(FILA_GRID.Cells[1].Text.Trim())) {
                                Percepcion_Deduccion_ID = FILA_GRID.Cells[1].Text.Trim();

                                Dr_Registros_Encontrados = Dt_Datos_Consultados.Select("PERCEPCION_DEDUCCION_ID=" + Percepcion_Deduccion_ID);

                                if (Dr_Registros_Encontrados.Length > 0) {
                                    foreach (DataRow REGISTRO in Dr_Registros_Encontrados) {
                                        if (REGISTRO is DataRow) {
                                            if (!String.IsNullOrEmpty(REGISTRO["Cantidad"].ToString().Trim()))
                                            {
                                                ((TextBox)FILA_GRID.Cells[4].FindControl(TextBox_ID)).Text =
                                                    String.Format("{0:c}", Convert.ToDouble(REGISTRO["Cantidad"].ToString().Trim()));

                                                ((TextBox)FILA_GRID.Cells[5].FindControl(TextBox_ID + "_Real")).Text =
                                                    String.Format("{0:c}", Convert.ToDouble(REGISTRO["Cantidad"].ToString().Trim()));


                                            }
                                        }
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
            throw new Exception("Error al leer las cantidades de los controles TextBox que se encuentran dentro del GridView. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Crear_Lista_Percepciones_Con_Monto
    /// 
    /// DESCRIPCION : Crea un datatable con la información de las percepciones.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 04/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected DataSet Crear_Lista_Percepciones_Con_Monto(DataTable Dt_Percepciones)
    {
        DataSet Ds_Percepciones = new DataSet();
        DataTable Dt_Total_Percepciones = new DataTable();
        DataTable Dt_Percepciones_Sin_Montos_Ceros = new DataTable();
        Cls_Cat_Nom_Percepciones_Deducciones_Business INF_PERCEPCION = null;
        Double Total_Percepciones = 0.0;
        Double Monto = 0.0;
        Double Grava = 0.0;
        Double Exenta = 0.0;
        Numalet Obj_Numale = new Numalet();

        try
        {
            Dt_Total_Percepciones.Columns.Add("TOTAL_PERCEPCIONES", typeof(Double));
            Dt_Total_Percepciones.Columns.Add("TOTAL_PERCEPCIONES_LETRA", typeof(String));
            Dt_Total_Percepciones.Columns.Add("TOTAL_LETRA", typeof(String));
            Dt_Total_Percepciones.Columns.Add("LISTADO_PERCEPCIONES", typeof(String));

            Dt_Percepciones_Sin_Montos_Ceros.Columns.Add("NOMBRE", typeof(String));
            Dt_Percepciones_Sin_Montos_Ceros.Columns.Add("Percepcion_Deduccion", typeof(String));
            Dt_Percepciones_Sin_Montos_Ceros.Columns.Add("Monto", typeof(Double));
            Dt_Percepciones_Sin_Montos_Ceros.Columns.Add("Grava", typeof(Double));
            Dt_Percepciones_Sin_Montos_Ceros.Columns.Add("Exenta", typeof(Double));


            if (Dt_Percepciones is DataTable)
            {
                if (Dt_Percepciones.Rows.Count > 0)
                {
                    foreach (DataRow PERCEPCIONES in Dt_Percepciones.Rows)
                    {
                        if (PERCEPCIONES is DataRow)
                        {
                            if (!String.IsNullOrEmpty(PERCEPCIONES["Percepcion_Deduccion"].ToString().Trim()))
                            {
                                INF_PERCEPCION = Consultar_Percepcion_Deduccion(PERCEPCIONES["Percepcion_Deduccion"].ToString().Trim());

                                if (!String.IsNullOrEmpty(PERCEPCIONES["Monto"].ToString().Trim()))
                                    Monto = Convert.ToDouble(PERCEPCIONES["Monto"].ToString().Trim());

                                if (!String.IsNullOrEmpty(PERCEPCIONES["Grava"].ToString().Trim()))
                                    Grava = Convert.ToDouble(PERCEPCIONES["Grava"].ToString().Trim());

                                if (!String.IsNullOrEmpty(PERCEPCIONES["Exenta"].ToString().Trim()))
                                    Exenta = Convert.ToDouble(PERCEPCIONES["Exenta"].ToString().Trim());

                                if (Monto > 0)
                                {
                                    DataRow Dr_Percepcion = Dt_Percepciones_Sin_Montos_Ceros.NewRow();
                                    Dr_Percepcion["Percepcion_Deduccion"] = INF_PERCEPCION.P_PERCEPCION_DEDUCCION_ID;
                                    Dr_Percepcion["NOMBRE"] = INF_PERCEPCION.P_NOMBRE.ToUpper();
                                    Dr_Percepcion["Monto"] = Monto;
                                    Dr_Percepcion["Grava"] = Grava;
                                    Dr_Percepcion["Exenta"] = Exenta;
                                    Dt_Percepciones_Sin_Montos_Ceros.Rows.Add(Dr_Percepcion);

                                    Total_Percepciones += Monto;
                                }

                                Monto = 0;
                                Grava = 0;
                                Exenta = 0;
                            }
                        }
                    }
                }
            }

            Ds_Percepciones.Tables.Add(Dt_Percepciones_Sin_Montos_Ceros);
            Ds_Percepciones.Tables.Add(Dt_Total_Percepciones);

            DataRow Dr_Total_Percepciones = Dt_Total_Percepciones.NewRow();
            Dr_Total_Percepciones["TOTAL_PERCEPCIONES"] = Total_Percepciones;
            Dr_Total_Percepciones["TOTAL_PERCEPCIONES_LETRA"] = Convertir_Cantidad_Letras(Total_Percepciones);
            Dr_Total_Percepciones["LISTADO_PERCEPCIONES"] = Crear_Listado_Conceptos(Dt_Percepciones_Sin_Montos_Ceros);
            Dt_Total_Percepciones.Rows.Add(Dr_Total_Percepciones);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al crear la lista de percepciones con montos mayores a cero. Error: [" + Ex.Message + "]");
        }
        return Ds_Percepciones;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Crear_Lista_Deducciones_Con_Monto
    /// 
    /// DESCRIPCION : Crea un datatable con la información de las deducciones.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 04/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected DataSet Crear_Lista_Deducciones_Con_Monto(DataTable Dt_Deducciones)
    {
        DataSet Ds_Deducciones = new DataSet();
        DataTable Dt_Totales_Deducciones = new DataTable();
        DataTable Dt_Deducciones_Sin_Montos_Ceros = new DataTable();
        Cls_Cat_Nom_Percepciones_Deducciones_Business INF_DEDUCCION = null;
        Double Total_Deducciones = 0.0;
        Double Monto = 0.0;
        Double Grava = 0.0;
        Double Exenta = 0.0;
        Numalet Obj_Numale = new Numalet();

        try
        {
            Dt_Totales_Deducciones.Columns.Add("TOTAL_DEDUCCIONES", typeof(Double));
            Dt_Totales_Deducciones.Columns.Add("TOTAL_DEDUCCIONES_LETRA", typeof(String));
            Dt_Totales_Deducciones.Columns.Add("TOTAL_LETRA", typeof(String));
            Dt_Totales_Deducciones.Columns.Add("LISTADO_DEDUCCIONES", typeof(String));

            Dt_Deducciones_Sin_Montos_Ceros.Columns.Add("NOMBRE", typeof(String));
            Dt_Deducciones_Sin_Montos_Ceros.Columns.Add("Percepcion_Deduccion", typeof(String));
            Dt_Deducciones_Sin_Montos_Ceros.Columns.Add("Monto", typeof(Double));
            Dt_Deducciones_Sin_Montos_Ceros.Columns.Add("Grava", typeof(Double));
            Dt_Deducciones_Sin_Montos_Ceros.Columns.Add("Exenta", typeof(Double));

            if (Dt_Deducciones is DataTable)
            {
                if (Dt_Deducciones.Rows.Count > 0)
                {
                    foreach (DataRow DEDUCCIONES in Dt_Deducciones.Rows)
                    {
                        if (DEDUCCIONES is DataRow)
                        {
                            if (!String.IsNullOrEmpty(DEDUCCIONES["Percepcion_Deduccion"].ToString().Trim()))
                            {
                                INF_DEDUCCION = Consultar_Percepcion_Deduccion(DEDUCCIONES["Percepcion_Deduccion"].ToString().Trim());

                                if (!String.IsNullOrEmpty(DEDUCCIONES["Monto"].ToString().Trim()))
                                    Monto = Convert.ToDouble(DEDUCCIONES["Monto"].ToString().Trim());

                                if (!String.IsNullOrEmpty(DEDUCCIONES["Grava"].ToString().Trim()))
                                    Grava = Convert.ToDouble(DEDUCCIONES["Grava"].ToString().Trim());

                                if (!String.IsNullOrEmpty(DEDUCCIONES["Exenta"].ToString().Trim()))
                                    Exenta = Convert.ToDouble(DEDUCCIONES["Exenta"].ToString().Trim());

                                if (Monto > 0)
                                {
                                    DataRow Dr_Percepcion = Dt_Deducciones_Sin_Montos_Ceros.NewRow();
                                    Dr_Percepcion["Percepcion_Deduccion"] = INF_DEDUCCION.P_PERCEPCION_DEDUCCION_ID;
                                    Dr_Percepcion["NOMBRE"] = INF_DEDUCCION.P_NOMBRE.ToUpper();
                                    Dr_Percepcion["Monto"] = Monto;
                                    Dr_Percepcion["Grava"] = Grava;
                                    Dr_Percepcion["Exenta"] = Exenta;
                                    Dt_Deducciones_Sin_Montos_Ceros.Rows.Add(Dr_Percepcion);

                                    Total_Deducciones += Monto;
                                }

                                Monto = 0;
                                Grava = 0;
                                Exenta = 0;
                            }
                        }
                    }
                }
            }

            DataRow Dr_Total_Deducciones = Dt_Totales_Deducciones.NewRow();
            Dr_Total_Deducciones["TOTAL_DEDUCCIONES"] = Total_Deducciones;
            Dr_Total_Deducciones["TOTAL_DEDUCCIONES_LETRA"] = Convertir_Cantidad_Letras(Total_Deducciones);
            Dr_Total_Deducciones["LISTADO_DEDUCCIONES"] = Crear_Listado_Conceptos(Dt_Deducciones_Sin_Montos_Ceros);
            Dt_Totales_Deducciones.Rows.Add(Dr_Total_Deducciones);

            Ds_Deducciones.Tables.Add(Dt_Deducciones_Sin_Montos_Ceros);
            Ds_Deducciones.Tables.Add(Dt_Totales_Deducciones);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al crear la lista de deducciones con montos mayores a cero. Error: [" + Ex.Message + "]");
        }
        return Ds_Deducciones;
    }
    #endregion

    #region (Metodos Consulta)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Tipos_Nomina
    ///DESCRIPCIÓN: Consulta y carga el ctrl que almacena los tipos de nómina que se 
    ///             encuentran dados de alta actualmente en el sistema.
    ///
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 4/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Combo_Tipos_Nomina()
    {
        Cls_Cat_Tipos_Nominas_Negocio Tipos_Nominas = new Cls_Cat_Tipos_Nominas_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Tipos_Nominas = null;//Variable que almacenara una lista de las nominas que existe actualmente en el sistema.

        try
        {
            //Consultamos los tipo de nómina que existen actualmente en el sistema.
            Dt_Tipos_Nominas = Tipos_Nominas.Consulta_Datos_Tipo_Nomina();
            //Cargamos el combo que corresponde a los tipo de nómina en el sistema.
            Cmb_Tipo_Nomina.DataSource = Dt_Tipos_Nominas;
            Cmb_Tipo_Nomina.DataTextField = Cat_Nom_Tipos_Nominas.Campo_Nomina;
            Cmb_Tipo_Nomina.DataValueField = Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID;
            Cmb_Tipo_Nomina.DataBind();
            Cmb_Tipo_Nomina.Items.Insert(0, new ListItem("< Seleccione >", ""));
            Cmb_Tipo_Nomina.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los tipos de nómina que existén actualmente en el sistema. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Periodos_Catorcenales_Nomina
    ///DESCRIPCIÓN: Consulta los periodos catorcenales para el 
    ///calendario de nomina seleccionado.
    ///PARAMETROS: Nomina_ID.- Indica el calendario de nomina del cuál se desea consultar
    ///                        los periodos catorcenales.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 4/Febrero/2011
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
            //Consultamos los detalles del calendario de nómina seleccionado.
            Consulta_Calendario_Nomina_Periodos.P_Nomina_ID = Nomina_ID;
            Dt_Periodos_Catorcenales = Consulta_Calendario_Nomina_Periodos.Consulta_Detalles_Nomina();

            if (Dt_Periodos_Catorcenales != null)
            {
                if (Dt_Periodos_Catorcenales.Rows.Count > 0)
                {
                    //Cargamos el combo de periodos catorcenales que almacena el calendario de nómina actualmente.
                    Cmb_Periodo.DataSource = Dt_Periodos_Catorcenales;
                    Cmb_Periodo.DataTextField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                    Cmb_Periodo.DataValueField = Cat_Nom_Nominas_Detalles.Campo_Detalle_Nomina_ID;
                    Cmb_Periodo.DataBind();
                    Cmb_Periodo.Items.Insert(0, new ListItem("< Seleccione >", ""));
                    Cmb_Periodo.SelectedIndex = -1;

                    Validar_Periodos_Pago(Cmb_Periodo);
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
    ///NOMBRE DE LA FUNCIÓN: Consultar_Calendario_Nominas
    ///DESCRIPCIÓN: Consulta los calendarios de nomina vigentes actualmente.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 4/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Calendario_Nominas()
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Consulta_Calendario_Nominas = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Calendario_Nominas = null;//Variable que almacenara una lista de los calendarios de nomina vigentes.

        try
        {
            //Consultamos los calendarios de nómina que existen actualmente dados de alta en el sistema.
            Dt_Calendario_Nominas = Consulta_Calendario_Nominas.Consultar_Calendario_Nominas();

            if (Dt_Calendario_Nominas != null)
            {
                if (Dt_Calendario_Nominas.Rows.Count > 0)
                {
                    //Cargamos el combo de calendario de las nómina.
                    Dt_Calendario_Nominas = Formato_Fecha_Calendario_Nomina(Dt_Calendario_Nominas);
                    Cmb_Calendario_Nomina.DataSource = Dt_Calendario_Nominas;
                    Cmb_Calendario_Nomina.DataTextField = "Nomina";
                    Cmb_Calendario_Nomina.DataValueField = Cat_Nom_Calendario_Nominas.Campo_Nomina_ID;
                    Cmb_Calendario_Nomina.DataBind();
                    Cmb_Calendario_Nomina.Items.Insert(0, new ListItem("< Seleccione >", ""));
                    Cmb_Calendario_Nomina.SelectedIndex = -1;
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No se encontraron nominas vigentes";
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las nominas. Error: [" + Ex.Message + "]");
        }
    }
    /// ************************************************************************************************
    /// Nombre: Consultar_Clave_Pecepcion_Deduccion
    /// 
    /// Descripción: Consulta la clave de la percepcion o deduccion y la une con el nombre del concepto.
    /// 
    /// Parámetros: Dt_Percepcion_Deduccion.- Tabla que almacena un listado con las percepciones o 
    ///             deducciones que le aplican al empleado.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creo: 27/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ************************************************************************************************
    protected void Consultar_Clave_Pecepcion_Deduccion(ref DataTable Dt_Percepcion_Deduccion)
    {
        Cls_Cat_Nom_Percepciones_Deducciones_Business Obj_Percepciones_Deducciones =
            new Cls_Cat_Nom_Percepciones_Deducciones_Business();//Variable de conexión con la capa de negocios.
        DataTable Dt_Percepcion_Deduccion_ = null;//Variable que almacena la información de los conceptos.

        try
        {
            if (Dt_Percepcion_Deduccion is DataTable)
            {
                if (Dt_Percepcion_Deduccion.Rows.Count > 0)
                {
                    foreach (DataRow PERCEPCION_DEDUCCION in Dt_Percepcion_Deduccion.Rows)
                    {
                        if (PERCEPCION_DEDUCCION is DataRow)
                        {
                            if (!String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString().Trim()))
                            {
                                Obj_Percepciones_Deducciones.P_PERCEPCION_DEDUCCION_ID =
                                    PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString().Trim();

                                Dt_Percepcion_Deduccion_ = Obj_Percepciones_Deducciones.Consultar_Percepciones_Deducciones_General();

                                if (Dt_Percepcion_Deduccion_ is DataTable)
                                {
                                    if (Dt_Percepcion_Deduccion_.Rows.Count > 0)
                                    {
                                        foreach (DataRow INNER_PERCEPCION_DEDUCCION in Dt_Percepcion_Deduccion_.Rows)
                                        {
                                            if (INNER_PERCEPCION_DEDUCCION is DataRow)
                                            {
                                                if (!String.IsNullOrEmpty(INNER_PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Clave].ToString().Trim()))
                                                {
                                                    PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Nombre] = "[" +
                                                        INNER_PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Clave].ToString().Trim() + "] -- " +
                                                        PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Nombre];
                                                }
                                            }
                                        }
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
            throw new Exception("Error al consultar la clave de los conceptos. Error: [" + Ex.Message + "]");
        }
    }
    /// ************************************************************************************************
    /// Nombre: Consultar_Percepcion_Deduccion
    /// 
    /// Descripción: Consulta los datos de alguna percepción o deducción.
    /// 
    /// Parámetros: Percepcion_Deduccion_ID.- Clave del concepto a buscar su información.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creo: 27/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ************************************************************************************************
    protected Cls_Cat_Nom_Percepciones_Deducciones_Business Consultar_Percepcion_Deduccion(String Percepcion_Deduccion_ID)
    {
        Cls_Cat_Nom_Percepciones_Deducciones_Business Obj_Percepcion_Deduccion = new Cls_Cat_Nom_Percepciones_Deducciones_Business();
        Cls_Cat_Nom_Percepciones_Deducciones_Business INF_PERCEPCION_DEDUCCION = new Cls_Cat_Nom_Percepciones_Deducciones_Business();
        DataTable Dt_Percepcion_Deduccion = null;

        try
        {
            Obj_Percepcion_Deduccion.P_PERCEPCION_DEDUCCION_ID = Percepcion_Deduccion_ID;
            Dt_Percepcion_Deduccion = Obj_Percepcion_Deduccion.Consultar_Percepciones_Deducciones_General();

            if (Dt_Percepcion_Deduccion is DataTable)
            {
                if (Dt_Percepcion_Deduccion.Rows.Count > 0)
                {
                    foreach (DataRow PERCEPCION_DEDUCCION in Dt_Percepcion_Deduccion.Rows)
                    {
                        if (PERCEPCION_DEDUCCION is DataRow)
                        {

                            if (!String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Nombre].ToString().Trim()))
                            {
                                INF_PERCEPCION_DEDUCCION.P_NOMBRE = "[" + PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Clave].ToString().Trim() + "] -- " +
                                    PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Nombre].ToString().Trim();
                            }

                            if (!String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion].ToString().Trim()))
                                INF_PERCEPCION_DEDUCCION.P_TIPO_ASIGNACION = PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion].ToString().Trim();

                            if (!String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Tipo].ToString().Trim()))
                                INF_PERCEPCION_DEDUCCION.P_TIPO = PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Tipo].ToString().Trim();

                            if (!String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString().Trim()))
                                INF_PERCEPCION_DEDUCCION.P_PERCEPCION_DEDUCCION_ID = PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString().Trim();

                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar la informacion de la percepción y/ deducción. Error: [" + Ex.Message + "]");
        }
        return INF_PERCEPCION_DEDUCCION;
    }
    #endregion

    #region (Prestamos Ajuste ISR)
    /// ***************************************************************************************************************************
    /// Nombre: Afectar_Registros_Prestamos_Ajustes_ISR
    /// 
    /// Descripción: Método que actualiza la información de los registros de prestamos y ajustes de isr para dejarlos
    ///              con un saldo de cero al realizarse el finiquito.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 30/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// ***************************************************************************************************************************
    protected void Afectar_Registros_Prestamos_Ajustes_ISR()
    {
        DataSet Ds_Tablas_Registros_Afectar = null;
        String Ruta_Archivo_Finiquito = String.Empty;
        Cls_Ope_Nom_Pestamos_Negocio Obj_Prestamos = new Cls_Ope_Nom_Pestamos_Negocio();
        Cls_Ope_Nom_Ajuste_ISR_Negocio Obj_Ajustes_ISR = new Cls_Ope_Nom_Ajuste_ISR_Negocio();
        String No_Solicitud = String.Empty;
        String No_Ajuste_ISR = String.Empty;

        try
        {
            //Obtenemos la ruta de donde leeremos el archivo.
            Ruta_Archivo_Finiquito = Server.MapPath("Log_Finiquito");

            Ds_Tablas_Registros_Afectar = Cls_Historial_Nomina_Generada.Leer_Archivo_Obtener_Historial_Nomina_Generada(Ruta_Archivo_Finiquito, "/Log_Finiquito", ".txt");

            if (Ds_Tablas_Registros_Afectar is DataSet)
            {
                if (Ds_Tablas_Registros_Afectar.Tables.Count > 0)
                {
                    foreach (DataTable Dt_Datos in Ds_Tablas_Registros_Afectar.Tables)
                    {
                        if (Dt_Datos is DataTable)
                        {
                            if (Dt_Datos.TableName.ToString().Trim().ToUpper().Equals("PRESTAMO"))
                            {
                                if (Dt_Datos.Rows.Count > 0)
                                {
                                    foreach (DataRow PRESTAMO in Dt_Datos.Rows)
                                    {
                                        if (PRESTAMO is DataRow)
                                        {
                                            if (!String.IsNullOrEmpty(PRESTAMO["NO_SOLICITUD"].ToString().Trim()))
                                            {
                                                No_Solicitud = PRESTAMO["NO_SOLICITUD"].ToString().Trim();

                                                Obj_Prestamos.P_No_Solicitud = No_Solicitud;
                                                Obj_Prestamos.Finiquitar_Prestamo();
                                            }
                                        }
                                    }
                                }
                            }

                            if (Dt_Datos.TableName.ToString().Trim().ToUpper().Equals("AJUSTE_ISR"))
                            {
                                if (Dt_Datos.Rows.Count > 0)
                                {
                                    foreach (DataRow AJUSTE_ISR in Dt_Datos.Rows)
                                    {
                                        if (AJUSTE_ISR is DataRow)
                                        {
                                            if (!String.IsNullOrEmpty(AJUSTE_ISR["NO_AJUSTE_ISR"].ToString().Trim()))
                                            {
                                                No_Ajuste_ISR = AJUSTE_ISR["NO_AJUSTE_ISR"].ToString().Trim();

                                                Obj_Ajustes_ISR.P_No_Ajuste_ISR = No_Ajuste_ISR;
                                                Obj_Ajustes_ISR.Finiquitar_Ajuste_ISR();
                                            }
                                        }
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
            throw new Exception("Error generado al actualizar la informacion de los prestamos y ajustes de ISR. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Finiquito)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Ejecuta_Finiquito_Empleado
    ///
    ///DESCRIPCIÓN: Ejecuta el Finiquito del Empleado. Obtiene todas la percepciones y/o deducciones
    ///             que aplican para este cálculo. y se hace el cálculo del finiquito del empleado.
    ///             
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 04/Febrero/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Ejecuta_Finiquito_Empleado()
    {
        Cls_Ope_Nom_Recibos_Empleados_Negocio Recibo_Nomina_Empleado = new Cls_Ope_Nom_Recibos_Empleados_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Ope_Nom_Finiquitos_Negocio Generar_Finiquitos_Util = new Cls_Ope_Nom_Finiquitos_Negocio();//Variable de conexión con la capa de negocios.
        Cls_Cat_Empleados_Negocios Informacion_Empleado = new Cls_Cat_Empleados_Negocios();//Variable de conexión con la capa de negocios.
        DataTable Dt_Percepciones_Considerar_Finiquito = null;//Percepciones que se consideraran para el finiquito.
        DataTable Dt_Deducciones_Considerar_Finiquito = null;//Deducciones que se conideraran para el finiquito.
        DataTable Dt_Percepciones_IO_Deducciones = null;//Variable que almacena una lista con las percepciones y/o deducciones que le aplicaran al empleado para el cálculo de su Finiquito.
        Double Total_Percepciones = 0.0;//Total Percepciones que aplican para el finiquito del empleado.
        Double Total_Deducciones = 0.0;//Total deducciones que aplican para el finiquito del empleado.
        Double Total_Finiquito = 0.0;//Total del Finiquito del empleado.
        Double Total_Ingresos_Gravables_Empleado = 0.0;//Variable que almacenara el total de ingresos gravables del empleado.
        Double Total_Exenta_Empleado = 0.0;//Variable que almacenara el total que exenta el empleado.
        DateTime Fecha;//Fecha para validar la generación de la nómina.
        String Nomina_Generar = "";//Almacena el tipo de nómina a generar Catorcenal, Prima Vacacional 1ra. Parte ó PV 2da. Parte y Aguinaldo Integrado.
        String Nomina_ID = "";//Variable que almacenará el Identificador de la nómina.
        Int32 No_Nomina = 0;//Variable que almacena el numero de catorcena de la cual se desea generar la nómina.
        String Detalle_Nomina_ID = "";//Variable que identifica el perido seleccionado para generar la nómina.
        String Tipo_Nomina_ID = "";//Variable que almacena el tipo de nómina de la cual se desea generar la nómina.
        Boolean Operacion_Completa = false;
        DataTable Dt_Conceptos_Finiquito = null;

        try
        {
            //Se realiza la validación consultando los resguardos que tiene el empleado actualmente para poder determinar si 
            //será posible generar el finiquito al empleado, ya que si este tiene algún resguardo actualmente, no será pósible
            //generar el Finiquito del mismo.
            if (Grid_Resguardos_Empleado.Rows.Count <= 0)
            {
                #region (Datos Generales Para el Cálculo del Finiquito)
                //Obtenemos la fecha en la que se desea realizar el finiquito del empleado. Esta fecha servira para obtener la fecha
                //para el cálculo del finiquito.
                Fecha = Convert.ToDateTime(Txt_Fin_Catorcena.Text.Trim());
                //Establecemos la fecha en la clase de Ope_Nom_Finiquito.
                Generar_Finiquitos_Util.Fecha_Catorcena_Generar_Nomina = Fecha;
                //Obtenemos los valores que son necesarios para realizar la generación de la nómina.
                Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
                Detalle_Nomina_ID = Cmb_Periodo.SelectedValue.Trim();
                No_Nomina = Convert.ToInt32(Cmb_Periodo.SelectedItem.Text.Trim());
                Tipo_Nomina_ID = Cmb_Tipo_Nomina.SelectedValue.Trim();

                Generar_Finiquitos_Util.P_Nomina_ID = Nomina_ID;
                Generar_Finiquitos_Util.P_No_Nomina = No_Nomina;
                Generar_Finiquitos_Util.P_Detalle_Nomina_ID = Detalle_Nomina_ID;
                Generar_Finiquitos_Util.P_Tipo_Nomina_ID = Tipo_Nomina_ID;
                #endregion

                #region (Obtener Montos Totales)

                //Obtenemos la información de las tablas de percepciones y deducciones que el usuario 
                //habia actualizado previamente.
                if (Session["Dt_PERCEPCIONES"] != null)
                {
                    Dt_Percepciones_Considerar_Finiquito = (DataTable)Session["Dt_PERCEPCIONES"];
                    Session.Remove("Dt_PERCEPCIONES");
                }

                if (Session["Dt_DEDUCCIONES"] != null)
                {
                    Dt_Deducciones_Considerar_Finiquito = (DataTable)Session["Dt_DEDUCCIONES"];
                    Session.Remove("Dt_DEDUCCIONES");
                }

                if (Session["Dt_CONCEPTOS_FINIQUITOS"] != null)
                {
                    Dt_Conceptos_Finiquito = (DataTable)Session["Dt_CONCEPTOS_FINIQUITOS"];
                    Session.Remove("Dt_CONCEPTOS_FINIQUITOS");
                }

                //**[ ACTUALIZAMOS LA INF DE LAS TABLAS DE PERCEPCIONES Y DEDUCCIONES CON LA INF QUE FUE ACTUALIZADA POR EL USUARIO ]**
                Actualizar_Cantidades_Percepciones_Deducciones(Dt_Conceptos_Finiquito, ref Dt_Percepciones_Considerar_Finiquito);
                Actualizar_Cantidades_Percepciones_Deducciones(Dt_Conceptos_Finiquito, ref Dt_Deducciones_Considerar_Finiquito);

                //Obtenemos un aEstructura gral. que almacena todas las percepciones y/o deducciones con sus respectivos montos, cantidad gravable y exenta.
                //Que aplican para el calculo de su finiquito.
                Dt_Percepciones_IO_Deducciones = Integrar_Percepciones_Deducciones_Una_Tabla(Dt_Percepciones_Considerar_Finiquito, Dt_Deducciones_Considerar_Finiquito);

                //Obtenemos el Total de percepciones que aplicaron al calculo del finiquito.
                Total_Percepciones = Sumar_Percepciones_IO_Deducciones(Dt_Percepciones_Considerar_Finiquito, "Monto");
                //Obtenemos el total de Gravado.
                Total_Ingresos_Gravables_Empleado = Sumar_Percepciones_IO_Deducciones(Dt_Percepciones_Considerar_Finiquito, "Grava");
                //Obtenemos los totales de exento.
                Total_Exenta_Empleado = Sumar_Percepciones_IO_Deducciones(Dt_Percepciones_Considerar_Finiquito, "Exenta");
                //Obtenemos el Total de deducciones que aplicaron al calculo del finiquito.
                Total_Deducciones = Sumar_Percepciones_IO_Deducciones(Dt_Deducciones_Considerar_Finiquito, "Monto");
                //Obtenemos el Total de percepciones y/o deducciones que aplicaron al calculo del finiquito.
                Total_Finiquito = (Total_Percepciones - Total_Deducciones);
                #endregion

                #region (Guardar Recibo del Finiquito)
                //Consultamos la informacion del empleado.
                Informacion_Empleado.P_No_Empleado = Txt_No_Empleado.Text.Trim();
                Informacion_Empleado.P_Estatus = "INACTIVO";
                Generar_Finiquitos_Util.Consultar_Informacion_Empleado(ref Informacion_Empleado);
                //Consultamos la información para generar el recibo de nómina.
                Recibo_Nomina_Empleado.P_Nomina_ID = Nomina_ID;
                Recibo_Nomina_Empleado.P_Detalle_Nomina_ID = Detalle_Nomina_ID;
                Recibo_Nomina_Empleado.P_No_Nomina = No_Nomina;
                Recibo_Nomina_Empleado.P_Tipo_Nomina_ID = Tipo_Nomina_ID;
                Recibo_Nomina_Empleado.P_Empleado_ID = Informacion_Empleado.P_Empleado_ID;
                Recibo_Nomina_Empleado.P_Dependencia_ID = Informacion_Empleado.P_Dependencia_ID;
                Recibo_Nomina_Empleado.P_Puesto_ID = Informacion_Empleado.P_Puesto_ID;
                Recibo_Nomina_Empleado.P_Dias_Trabajados = Generar_Finiquitos_Util.Consultar_Dias_Trabajados(Informacion_Empleado.P_Empleado_ID);
                Recibo_Nomina_Empleado.P_Total_Percepciones = Total_Percepciones;
                Recibo_Nomina_Empleado.P_Total_Deducciones = Total_Deducciones;
                Recibo_Nomina_Empleado.P_Total_Nomina = (Total_Percepciones - Total_Deducciones);
                Recibo_Nomina_Empleado.P_Gravado = Total_Ingresos_Gravables_Empleado;
                Recibo_Nomina_Empleado.P_Exento = Total_Exenta_Empleado;
                Recibo_Nomina_Empleado.P_Salario_Diario = Informacion_Empleado.P_Salario_Diario;
                Recibo_Nomina_Empleado.P_Salario_Diario_Integrado = Informacion_Empleado.P_Salario_Diario_Integrado;
                Recibo_Nomina_Empleado.P_Nomina_Generada = "FINIQUITO";

                //Se hace el cierre o cálculo del finiquito al empleado.
                Generar_Finiquitos_Util.Guardar_Recibo_Nomina(Recibo_Nomina_Empleado, Dt_Percepciones_IO_Deducciones);
                #endregion

                #region (Guardar Totales Nomina)
                Generar_Finiquitos_Util.Actualizar_Saldos_Deducciones_Fijas_Proveedor(Txt_No_Empleado.Text.Trim(), Dt_Percepciones_IO_Deducciones);
                //Se construye la SESSION que almacenará todos los conceptos que aplicán para el cálculo del finiquito.
                Generar_Finiquitos_Util.Obtener_Totales_Nomina(Dt_Percepciones_IO_Deducciones);
                //Guardamos en la BD. los totales de los conceptos que aplicaron para el cálculo del finiquito.
                Generar_Finiquitos_Util.Guardar_Totales_Nomina();
                #endregion

                Operacion_Completa = true;
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No es posible generar un finiquito, si el Empleado aún tiene algún resguardo actualmente bajo su responsabilidad.";
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al Ejecutar el Finiquito del Empleado. Error: [" + Ex.Message + "]");
        }
        return Operacion_Completa;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Ejecuta_Finiquito_Empleado
    ///
    ///DESCRIPCIÓN: Ejecuta el Finiquito del Empleado. Obtiene todas la percepciones y/o deducciones
    ///             que aplican para este cálculo. y se hace el cálculo del finiquito del empleado.
    ///             
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 04/Febrero/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Ejecuta_Finiquito_Empleado_Real()
    {
        Cls_Ope_Nom_Recibos_Empleados_Negocio Recibo_Nomina_Empleado = new Cls_Ope_Nom_Recibos_Empleados_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Ope_Nom_Finiquitos_Negocio Generar_Finiquitos_Util = new Cls_Ope_Nom_Finiquitos_Negocio();//Variable de conexión con la capa de negocios.
        Cls_Cat_Empleados_Negocios Informacion_Empleado = new Cls_Cat_Empleados_Negocios();//Variable de conexión con la capa de negocios.
        DataTable Dt_Percepciones_Considerar_Finiquito = null;//Percepciones que se consideraran para el finiquito.
        DataTable Dt_Deducciones_Considerar_Finiquito = null;//Deducciones que se conideraran para el finiquito.
        DataTable Dt_Percepciones_IO_Deducciones = null;//Variable que almacena una lista con las percepciones y/o deducciones que le aplicaran al empleado para el cálculo de su Finiquito.
        Double Total_Percepciones = 0.0;//Total Percepciones que aplican para el finiquito del empleado.
        Double Total_Deducciones = 0.0;//Total deducciones que aplican para el finiquito del empleado.
        Double Total_Finiquito = 0.0;//Total del Finiquito del empleado.
        Double Total_Ingresos_Gravables_Empleado = 0.0;//Variable que almacenara el total de ingresos gravables del empleado.
        Double Total_Exenta_Empleado = 0.0;//Variable que almacenara el total que exenta el empleado.
        DateTime Fecha;//Fecha para validar la generación de la nómina.
        String Nomina_Generar = "";//Almacena el tipo de nómina a generar Catorcenal, Prima Vacacional 1ra. Parte ó PV 2da. Parte y Aguinaldo Integrado.
        String Nomina_ID = "";//Variable que almacenará el Identificador de la nómina.
        Int32 No_Nomina = 0;//Variable que almacena el numero de catorcena de la cual se desea generar la nómina.
        String Detalle_Nomina_ID = "";//Variable que identifica el perido seleccionado para generar la nómina.
        String Tipo_Nomina_ID = "";//Variable que almacena el tipo de nómina de la cual se desea generar la nómina.
        Boolean Operacion_Completa = false;
        DataTable Dt_Conceptos_Finiquito = null;

        try
        {
            //Se realiza la validación consultando los resguardos que tiene el empleado actualmente para poder determinar si 
            //será posible generar el finiquito al empleado, ya que si este tiene algún resguardo actualmente, no será pósible
            //generar el Finiquito del mismo.
            if (Grid_Resguardos_Empleado.Rows.Count <= 0)
            {
                #region (Datos Generales Para el Cálculo del Finiquito)
                //Obtenemos la fecha en la que se desea realizar el finiquito del empleado. Esta fecha servira para obtener la fecha
                //para el cálculo del finiquito.
                Fecha = Convert.ToDateTime(Txt_Fin_Catorcena.Text.Trim());
                //Establecemos la fecha en la clase de Ope_Nom_Finiquito.
                Generar_Finiquitos_Util.Fecha_Catorcena_Generar_Nomina = Fecha;
                //Obtenemos los valores que son necesarios para realizar la generación de la nómina.
                Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
                Detalle_Nomina_ID = Cmb_Periodo.SelectedValue.Trim();
                No_Nomina = Convert.ToInt32(Cmb_Periodo.SelectedItem.Text.Trim());
                Tipo_Nomina_ID = Cmb_Tipo_Nomina.SelectedValue.Trim();

                Generar_Finiquitos_Util.P_Nomina_ID = Nomina_ID;
                Generar_Finiquitos_Util.P_No_Nomina = No_Nomina;
                Generar_Finiquitos_Util.P_Detalle_Nomina_ID = Detalle_Nomina_ID;
                Generar_Finiquitos_Util.P_Tipo_Nomina_ID = Tipo_Nomina_ID;
                #endregion

                #region (Obtener Montos Totales)

                Dt_Percepciones_Considerar_Finiquito = (DataTable)Session["TABLA_PERCEPCIONES"];
                Dt_Deducciones_Considerar_Finiquito = (DataTable)Session["TABLA_DEDUCCIONES"];

                //Obtenemos un aEstructura gral. que almacena todas las percepciones y/o deducciones con sus respectivos montos, cantidad gravable y exenta.
                //Que aplican para el calculo de su finiquito.
                Dt_Percepciones_IO_Deducciones = Integrar_Percepciones_Deducciones_Una_Tabla(Dt_Percepciones_Considerar_Finiquito, Dt_Deducciones_Considerar_Finiquito);

                //Obtenemos el Total de percepciones que aplicaron al calculo del finiquito.
                Total_Percepciones = Sumar_Percepciones_IO_Deducciones(Dt_Percepciones_Considerar_Finiquito, "Monto");
                //Obtenemos el total de Gravado.
                Total_Ingresos_Gravables_Empleado = Sumar_Percepciones_IO_Deducciones(Dt_Percepciones_Considerar_Finiquito, "Grava");
                //Obtenemos los totales de exento.
                Total_Exenta_Empleado = Sumar_Percepciones_IO_Deducciones(Dt_Percepciones_Considerar_Finiquito, "Exenta");
                //Obtenemos el Total de deducciones que aplicaron al calculo del finiquito.
                Total_Deducciones = Sumar_Percepciones_IO_Deducciones(Dt_Deducciones_Considerar_Finiquito, "Monto");
                //Obtenemos el Total de percepciones y/o deducciones que aplicaron al calculo del finiquito.
                Total_Finiquito = (Total_Percepciones - Total_Deducciones);
                #endregion

                #region (Guardar Recibo del Finiquito)
                //Consultamos la informacion del empleado.
                Informacion_Empleado.P_No_Empleado = Txt_No_Empleado.Text.Trim();
                Informacion_Empleado.P_Estatus = "INACTIVO";
                Generar_Finiquitos_Util.Consultar_Informacion_Empleado(ref Informacion_Empleado);
                //Consultamos la información para generar el recibo de nómina.
                Recibo_Nomina_Empleado.P_Nomina_ID = Nomina_ID;
                Recibo_Nomina_Empleado.P_Detalle_Nomina_ID = Detalle_Nomina_ID;
                Recibo_Nomina_Empleado.P_No_Nomina = No_Nomina;
                Recibo_Nomina_Empleado.P_Tipo_Nomina_ID = Tipo_Nomina_ID;
                Recibo_Nomina_Empleado.P_Empleado_ID = Informacion_Empleado.P_Empleado_ID;
                Recibo_Nomina_Empleado.P_Dependencia_ID = Informacion_Empleado.P_Dependencia_ID;
                Recibo_Nomina_Empleado.P_Puesto_ID = Informacion_Empleado.P_Puesto_ID;
                Recibo_Nomina_Empleado.P_Dias_Trabajados = Generar_Finiquitos_Util.Consultar_Dias_Trabajados(Informacion_Empleado.P_Empleado_ID);
                Recibo_Nomina_Empleado.P_Total_Percepciones = Total_Percepciones;
                Recibo_Nomina_Empleado.P_Total_Deducciones = Total_Deducciones;
                Recibo_Nomina_Empleado.P_Total_Nomina = (Total_Percepciones - Total_Deducciones);
                Recibo_Nomina_Empleado.P_Gravado = Total_Ingresos_Gravables_Empleado;
                Recibo_Nomina_Empleado.P_Exento = Total_Exenta_Empleado;
                Recibo_Nomina_Empleado.P_Salario_Diario = Informacion_Empleado.P_Salario_Diario;
                Recibo_Nomina_Empleado.P_Salario_Diario_Integrado = Informacion_Empleado.P_Salario_Diario_Integrado;
                Recibo_Nomina_Empleado.P_Nomina_Generada = "FINIQUITO_SISTEMA";

                //Se hace el cierre o cálculo del finiquito al empleado.
                Generar_Finiquitos_Util.Guardar_Recibo_Nomina(Recibo_Nomina_Empleado, Dt_Percepciones_IO_Deducciones);
                #endregion

                #region (Guardar Totales Nomina)
                //Generar_Finiquitos_Util.Actualizar_Saldos_Deducciones_Fijas_Proveedor(Txt_No_Empleado.Text.Trim(), Dt_Percepciones_IO_Deducciones);
                //Se construye la SESSION que almacenará todos los conceptos que aplicán para el cálculo del finiquito.
                Generar_Finiquitos_Util.Obtener_Totales_Nomina(Dt_Percepciones_IO_Deducciones);
                //Guardamos en la BD. los totales de los conceptos que aplicaron para el cálculo del finiquito.
                Generar_Finiquitos_Util.Guardar_Totales_Nomina();
                #endregion

                Operacion_Completa = true;
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No es posible generar un finiquito, si el Empleado aún tiene algún resguardo actualmente bajo su responsabilidad.";
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al Ejecutar el Finiquito del Empleado. Error: [" + Ex.Message + "]");
        }
        return Operacion_Completa;
    }
    /// ***************************************************************************************************************************
    /// Nombre: Obtener_Percepciones_Deducciones_Propias_Finiquito
    /// 
    /// Descripción: Este método consulta los parámetros para obtener un listado de percepciones y deducciones e identificar
    ///              las que aplican  exclusivamente para el cálculo del finiquito.
    /// 
    /// Parámetros: Dt_Conceptos.- Tabla que almacena las percepciones y deducciones que que le aplican al empleado.
    ///             Dt_Conceptos_Finiquito.- Tabla que almacena las percepciones y deducciones que son propias del
    ///                                      finiquito.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 30/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// ***************************************************************************************************************************
    protected void Obtener_Percepciones_Deducciones_Propias_Finiquito(DataTable Dt_Conceptos, ref DataTable Dt_Conceptos_Finiquito)
    {
        Cls_Cat_Nom_Parametros_Negocio Obj_Parametros = new Cls_Cat_Nom_Parametros_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Parametro = null;//Variable que almacena la información de los parámetros de la nómina.

        try
        {
            //Si la tabla de conceptos aun no tiene la estructura se clona la estructura para crear las columnas.
            if (Dt_Conceptos_Finiquito.Columns.Count == 0)
                Dt_Conceptos_Finiquito = Dt_Conceptos.Clone();

            //Consultamos el parámetro de la nómina.
            Dt_Parametro = Obj_Parametros.Consulta_Parametros();

            if (Dt_Parametro is DataTable)
            {
                if (Dt_Parametro.Rows.Count > 0)
                {
                    foreach (DataRow PARAMETRO in Dt_Parametro.Rows)
                    {
                        if (PARAMETRO is DataRow)
                        {
                            if (Dt_Conceptos is DataTable)
                            {
                                if (Dt_Conceptos.Rows.Count > 0)
                                {
                                    foreach (DataRow CONCEPTO in Dt_Conceptos.Rows)
                                    {
                                        if (CONCEPTO is DataRow)
                                        {
                                            if (PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Indemnizacion].ToString().Trim().Equals(
                                                CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString().Trim()))
                                            {
                                                Dt_Conceptos_Finiquito.ImportRow(CONCEPTO);
                                            }

                                            if (PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Prima_Antiguedad].ToString().Trim().Equals(
                                                CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString().Trim()))
                                            {
                                                Dt_Conceptos_Finiquito.ImportRow(CONCEPTO);
                                            }

                                            if (PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Sueldo_Normal].ToString().Trim().Equals(
                                                CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString().Trim()))
                                            {
                                                Dt_Conceptos_Finiquito.ImportRow(CONCEPTO);
                                            }

                                            if (PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Vacaciones_Pendientes_Pagar].ToString().Trim().Equals(
                                                CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString().Trim()))
                                            {
                                                Dt_Conceptos_Finiquito.ImportRow(CONCEPTO);
                                            }

                                            if (PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Horas_Extra].ToString().Trim().Equals(
                                                CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString().Trim()))
                                            {
                                                Dt_Conceptos_Finiquito.ImportRow(CONCEPTO);
                                            }

                                            if (PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Dia_Domingo].ToString().Trim().Equals(
                                                CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString().Trim()))
                                            {
                                                Dt_Conceptos_Finiquito.ImportRow(CONCEPTO);
                                            }

                                            if (PARAMETRO[Cat_Nom_Parametros.Campo_Percepcion_Dias_Festivos].ToString().Trim().Equals(
                                                CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString().Trim()))
                                            {
                                                Dt_Conceptos_Finiquito.ImportRow(CONCEPTO);
                                            }

                                            if (PARAMETRO[Cat_Nom_Parametros.Campo_Deduccion_Aguinaldo_Pagado_Mas].ToString().Trim().Equals(
                                                CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString().Trim()))
                                            {
                                                Dt_Conceptos_Finiquito.ImportRow(CONCEPTO);
                                            }

                                            if (PARAMETRO[Cat_Nom_Parametros.Campo_Deduccion_Prima_Vacacional_Pagada_Mas].ToString().Trim().Equals(
                                                CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString().Trim()))
                                            {
                                                Dt_Conceptos_Finiquito.ImportRow(CONCEPTO);
                                            }

                                            if (PARAMETRO[Cat_Nom_Parametros.Campo_Deduccion_Vacaciones_Tomadas_Mas].ToString().Trim().Equals(
                                                CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString().Trim()))
                                            {
                                                Dt_Conceptos_Finiquito.ImportRow(CONCEPTO);
                                            }
                                        }
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
            throw new Exception("Error al obtener las percepciones y deducciones que son propias de los finiquitos. Error: [" + Ex.Message + "]");
        }
    }
    /// ***************************************************************************************************************************
    /// Nombre: Ocultar_Percepciones_Exclusivas_Finiquito
    /// 
    /// Descripción: Este método oculta los registros exclusivos del finiquito en la tabla de percepciones y deducciones.
    /// 
    /// Parámetros: Dt_Percepciones.- Tabla que almacena las percepciones que que le aplican al empleado.
    ///             _GridView.- Control que almacena las percepciones del empleado.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 30/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// ***************************************************************************************************************************
    protected void Ocultar_Percepciones_Exclusivas_Finiquito(DataTable Dt_Percepciones, ref GridView _GridView)
    {
        try
        {
            if (_GridView is GridView)
            {
                if (_GridView.Rows.Count > 0)
                {
                    foreach (GridViewRow FILA in _GridView.Rows)
                    {
                        if (FILA is GridViewRow)
                        {
                            String Percepcion_ID = FILA.Cells[1].Text.Trim();

                            if (Dt_Percepciones is DataTable)
                            {
                                if (Dt_Percepciones.Rows.Count > 0)
                                {
                                    foreach (DataRow PERCEPCION in Dt_Percepciones.Rows)
                                    {
                                        if (PERCEPCION is DataRow)
                                        {
                                            if (!String.IsNullOrEmpty(PERCEPCION[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString().Trim()))
                                            {
                                                String Percepcion_ID_Finiquito = PERCEPCION[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString().Trim();

                                                if (Percepcion_ID.Equals(Percepcion_ID_Finiquito))
                                                {
                                                    FILA.Attributes.Add("disabled", "true");
                                                    FILA.Cells[5].Enabled = false;
                                                    FILA.Style.Add("background", "rgb(225,225,0) url(../imagenes/paginas/barraPie.PNG) repeat-x;");
                                                }
                                            }
                                        }
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
            throw new Exception("Error al ocultar las percepciones exclusivas de finiquitos. Error: [" + Ex.Message + "]");
        }
    }
    /// ***************************************************************************************************************************
    /// Nombre: Ocultar_Deducciones_Exclusivas_Finiquito
    /// 
    /// Descripción: Este método oculta los registros exclusivos del finiquito en la tabla de deducciones.
    /// 
    /// Parámetros: Dt_Deducciones.- Tabla que almacena las deducciones que que le aplican al empleado.
    ///             _GridView.- Control que almacena las percepciones del empleado.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 30/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// ***************************************************************************************************************************
    protected void Ocultar_Deducciones_Exclusivas_Finiquito(DataTable Dt_Deducciones, ref GridView _GridView)
    {
        try
        {
            if (_GridView is GridView)
            {
                if (_GridView.Rows.Count > 0)
                {
                    foreach (GridViewRow FILA in _GridView.Rows)
                    {
                        if (FILA is GridViewRow)
                        {
                            String Deduccion_ID = FILA.Cells[1].Text.Trim();

                            if (Dt_Deducciones is DataTable)
                            {
                                if (Dt_Deducciones.Rows.Count > 0)
                                {
                                    foreach (DataRow DEDUCCION in Dt_Deducciones.Rows)
                                    {
                                        if (DEDUCCION is DataRow)
                                        {
                                            if (!String.IsNullOrEmpty(DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString().Trim()))
                                            {
                                                String Deduccion_ID_Finiquito = DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString().Trim();

                                                if (Deduccion_ID.Equals(Deduccion_ID_Finiquito))
                                                {
                                                    FILA.Enabled = false;
                                                    FILA.Style.Add("background", "rgb(225,225,0) url(../imagenes/paginas/toolkit-bg.gif) repeat-x;");
                                                }
                                            }
                                        }
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
            throw new Exception("Error al ocultar las percepciones exclusivas de finiquitos. Error: [" + Ex.Message + "]");
        }
    }
    /// ***************************************************************************************************************************
    /// Nombre: Actualizar_Cantidades_Percepciones_Deducciones
    /// 
    /// Descripción: Este método actualiza las cantidades que el usuario ajusto, en los conceptos exclusivos de finiquitos.
    /// 
    /// Parámetros: Dt_CONCEPTOS.- Tabla que almacena las cantidades actualizadas  de los conceptos exclusivos de finiquitos.
    ///             Dt_TABLA_ACTUALZAR.- Tabla que almacena las percepciones y deducciones que le aplican al empleado y sobre la 
    ///                                  que se actualizaran las.cantidades.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 30/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// ***************************************************************************************************************************
    protected void Actualizar_Cantidades_Percepciones_Deducciones(DataTable Dt_CONCEPTOS, ref DataTable Dt_TABLA_ACTUALZAR)
    {
        try
        {
            if (Dt_CONCEPTOS is DataTable)
            {
                if (Dt_CONCEPTOS.Rows.Count > 0)
                {
                    foreach (DataRow CONCEPTO in Dt_CONCEPTOS.Rows)
                    {
                        if (!String.IsNullOrEmpty(CONCEPTO["Percepcion_Deduccion"].ToString().Trim()))
                        {
                            String Percepcion_Deduccion_ID = CONCEPTO["Percepcion_Deduccion"].ToString().Trim();

                            DataRow[] Registros = Dt_TABLA_ACTUALZAR.Select("Percepcion_Deduccion=" + Percepcion_Deduccion_ID);

                            if (Registros.Length > 0)
                            {
                                Dt_TABLA_ACTUALZAR.Rows.Remove(Registros[0]);
                                Dt_TABLA_ACTUALZAR.ImportRow(CONCEPTO);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al Actualizar las conceptos. Error: [ " + Ex.Message + "]");
        }
    }
    #endregion

    #region (Validaciones)
    ///****************************************************************************************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Generar_O_Regenerar_Finiquito_Empleado
    /// 
    /// DESCRIPCION : Se consulta el empleado al que se le va a generar el finiquito, y se válida
    ///               que este activo, y si la operación a realizar es una regenereción del finiquito
    ///               o una generación del mismo.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 8/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///****************************************************************************************************************************************************
    private void Validar_Generar_O_Regenerar_Finiquito_Empleado()
    {
        Cls_Ope_Nom_Pestamos_Negocio Prestamos = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Ope_Nom_Recibos_Empleados_Negocio Recibos_Empleados = new Cls_Ope_Nom_Recibos_Empleados_Negocio();//Variable de conexión a la capa de negocios. 
        Cls_Ope_Nom_Finiquitos_Negocio Generar_Finiquitos = new Cls_Ope_Nom_Finiquitos_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Cat_Empleados_Negocios Empleados_Informacion = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios.
        String Empleado_Con_Registro_Finiquito = "";//Guardara el identificador único del empleado que ya cuenta con un finiquito.
        String Empleado_A_Registrar_Finiquito = "";//Guardara el identificador único del empleado, el cuál se le aplicará el finiquito.
        String No_Recibo_Empleado = "";//Almacenará el número de recibo de la nómina del empleado.
        String Ruta_Guardar_Archivo = "";//Variable que almacenará la ruta completa donde se guardara el log de la generacion de la nómina.
        StringBuilder Historial_Nomina_Generada = new StringBuilder();//Variable que almacenará todos los cambios realizados al generar la nómina, para poder hacer un rollback si asi es necesario.
        DataSet Ds_Tablas_Afectadas_Generacion_Nomina = null;//Variable que almacena las tabla que fueron afectadas en algunos registros al generar la nómina.
        DataTable Dt_Recibo_Nomina = null;//Variable que almacena el recibo de nómina generado en el finiquito.
        DataTable Dt_Recibo_Previamente_Generado = null;//Guardara la información del recibo previamente generado.
        DataTable Dt_Tabla_Empleados = null;//Guaradara los datos del empleado a generar sus finiquito.
        DataTable Dt_Detalles_Nomina = null;//Variable que almacenra los detalles del periodo seleccionado.
        DataTable Dt_Nominas_Generadas = null;//Variable que almacenara la nomina en la que se realizara el finiquito.
        DateTime Fecha_Inicio = new DateTime();//Fecha de inicio de la catorcena actual.
        DateTime Fecha_Fin = new DateTime();//Fecha de fin de la catorcena actual.


        try
        {
            ///VALIDAR QUE SI SE TRATA DE UNA OPERACIÓN PARA GENERAR UN FINIQUITO O UNA REGENERACIÓN DEL FINIQUITO.
            //Obtenemos la ruta donde se guardara el log de la nómina.
            Ruta_Guardar_Archivo = Server.MapPath("Log_Finiquito");

            if (File.Exists(@Ruta_Guardar_Archivo + "/Log_Finiquito.txt"))
            {
                //Obtenemos la información de las tablas que fueron afectadas al generar la nómina [PRESTAMOS, AJUSTES DE ISR, RECIBOS DE LA NÓMINA Y TOTALES DE LA NÓMINA].
                Ds_Tablas_Afectadas_Generacion_Nomina = Cls_Historial_Nomina_Generada.Leer_Archivo_Obtener_Historial_Nomina_Generada(@Ruta_Guardar_Archivo, "/Log_Finiquito", ".txt");

                Dt_Recibo_Nomina = Ds_Tablas_Afectadas_Generacion_Nomina.Tables["RECIBOS"];
                if (Dt_Recibo_Nomina is DataTable)
                {
                    if (Dt_Recibo_Nomina.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(Dt_Recibo_Nomina.Rows[0][Ope_Nom_Recibos_Empleados.Campo_No_Recibo].ToString()))
                        {
                            No_Recibo_Empleado = Dt_Recibo_Nomina.Rows[0][Ope_Nom_Recibos_Empleados.Campo_No_Recibo].ToString();
                            Recibos_Empleados.P_No_Recibo = No_Recibo_Empleado.Trim();

                            Dt_Recibo_Previamente_Generado = Recibos_Empleados.Consultar_Recibos_Empleados();

                            if (Dt_Recibo_Previamente_Generado is DataTable)
                            {
                                if (Dt_Recibo_Previamente_Generado.Rows.Count > 0)
                                {
                                    if (!string.IsNullOrEmpty(Dt_Recibo_Previamente_Generado.Rows[0][Ope_Nom_Recibos_Empleados.Campo_Empleado_ID].ToString()))
                                    {
                                        Empleado_Con_Registro_Finiquito = Dt_Recibo_Previamente_Generado.Rows[0][Ope_Nom_Recibos_Empleados.Campo_Empleado_ID].ToString();

                                        Empleados_Informacion.P_No_Empleado = Txt_No_Empleado.Text.Trim();
                                        Empleados_Informacion.P_Estatus = "ACTIVO";
                                        Dt_Tabla_Empleados = Empleados_Informacion.Consulta_Empleados_General();

                                        if (Dt_Tabla_Empleados is DataTable)
                                        {
                                            if (Dt_Tabla_Empleados.Rows.Count > 0)
                                            {
                                                if (!string.IsNullOrEmpty(Dt_Tabla_Empleados.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString()))
                                                {
                                                    Empleado_A_Registrar_Finiquito = Dt_Tabla_Empleados.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString();

                                                    if (Empleado_A_Registrar_Finiquito.Equals(Empleado_Con_Registro_Finiquito))
                                                    {
                                                        Btn_Generar_Finiquito.Text = "Regenerar Finiquito";
                                                    }
                                                    else
                                                    {
                                                        Btn_Generar_Finiquito.Text = "Generar Finiquito";
                                                    }
                                                }
                                            }
                                        }

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
            throw new Exception("Error al consultar si se trata de una generación o regeneración del finiquito. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Periodos_Pago
    ///DESCRIPCIÓN: Valida que el empleado solo puedan comenzar a descontar la deduccion 
    ///             a partir del periodo actual.
    ///             
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 05/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Validar_Periodos_Pago(DropDownList Combo)
    {
        Cls_Ope_Nom_Pestamos_Negocio Prestamos_Negocio = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Detalles_Nomina = null;//Variable que almacenra los detalles del periodo seleccionado.
        DateTime Fecha_Actual = DateTime.Now;//Variable que almacenará la fecha actual.
        DateTime Fecha_Inicio = new DateTime();//Variable que almacenará la fecha de inicio de la catorcena.
        DateTime Fecha_Fin = new DateTime();//Variable que almacenará la fecha final de la catorcena.
        Fecha_Actual = Fecha_Actual.AddDays(-14);//Obtenemos la fecha inicial dela catorcena anterior.

        Prestamos_Negocio.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();//Obtenemos la nomina seleccionada.

        foreach (ListItem Elemento in Combo.Items)
        {
            if (Es_Numerico(Elemento.Text.Trim()))
            {
                Prestamos_Negocio.P_No_Nomina = Convert.ToInt32(Elemento.Text.Trim());
                Dt_Detalles_Nomina = Prestamos_Negocio.Consultar_Fechas_Periodo();

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
    ///****************************************************************************************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Informacion_Pagina
    /// 
    /// DESCRIPCION : Valida que se halla ingresado toda la información necesaria. Para ejecutar la operación de deseada.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 8/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///****************************************************************************************************************************************************
    private Boolean Validar_Informacion_Pagina()
    {
        Boolean Datos_Validos = true;//Indica si los datos fueron proporcionados de forma completa.
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

        try
        {
            if (Cmb_Tipo_Nomina.SelectedIndex <= 0)
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Tipo Nómina <br>";
                Datos_Validos = false;
            }

            if (Cmb_Calendario_Nomina.SelectedIndex <= 0)
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La Nómina <br>";
                Datos_Validos = false;
            }

            if (Cmb_Periodo.SelectedIndex <= 0)
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Periodo Catorcenal. <br>";
                Datos_Validos = false;
            }

            if (string.IsNullOrEmpty(Txt_Inicia_Catorcena.Text.Trim()))
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Fecha inicia periodo catorcenal. <br>";
                Datos_Validos = false;
            }

            if (string.IsNullOrEmpty(Txt_Fin_Catorcena.Text.Trim()))
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Fecha fin periodo catorcenal. <br>";
                Datos_Validos = false;
            }

            if (string.IsNullOrEmpty(Txt_Nombre_Empleado.Text.Trim()))
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + No se a consultado ningún empleado a generar su finiquito. <br>";
                Datos_Validos = false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al validar la información de la página de generación de finiquitos. Error: [" + Ex.Message + "]");
        }
        return Datos_Validos;
    }
    #endregion

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
    #endregion

    #endregion

    #region (Grids)

    #region (GridView Percepciones)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Percepciones_RowDataBound
    /// DESCRIPCION : Agrega un identificador al boton de cancelar de la tabla
    /// para identicar la fila seleccionada de tabla.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 04/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Percepciones_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        StringBuilder Datos = new StringBuilder();

        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                ((TextBox)e.Row.Cells[4].FindControl("Txt_Cantidad_Percepcion")).ToolTip = "" + e.Row.RowIndex;
                ((TextBox)e.Row.Cells[4].FindControl("Txt_Cantidad_Percepcion")).Enabled = false;

                String Cantidad = ((TextBox)e.Row.Cells[5].FindControl("Txt_Cantidad_Percepcion")).Text;
                Double Cantidad_ = Convert.ToDouble((!String.IsNullOrEmpty(Cantidad)) ? Cantidad : "0");

                Datos.Append(e.Row.Cells[2].Text.ToUpper() + @"]\r\n\r\n");
                Datos.Append(@"-> Cantidad:  " + String.Format("{0:c}", Convert.ToDouble(Cantidad_)) + @"\r\n");
                Datos.Append(@"-> Grava:  " + String.Format("{0:c}", Convert.ToDouble(e.Row.Cells[6].Text)) + @"\r\n");
                Datos.Append(@"-> Exenta:  " + String.Format("{0:c}", Convert.ToDouble(e.Row.Cells[7].Text)) + @"\r\n");
                e.Row.Cells[2].Attributes.Add("onclick", "alert('" + Datos.ToString() + "');");
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    #endregion

    #region (GridView Deducciones)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Deducciones_RowDataBound
    /// DESCRIPCION : Agrega un identificador al boton de cancelar de la tabla
    /// para identicar la fila seleccionada de tabla.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 04/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Deducciones_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                ((TextBox)e.Row.Cells[4].FindControl("Txt_Cantidad_Deduccion")).ToolTip = "" + e.Row.RowIndex;
                ((TextBox)e.Row.Cells[4].FindControl("Txt_Cantidad_Deduccion")).Enabled = false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    #endregion

    #region (Grid Finiquito)
    protected void Grid_Conceptos_Exclusivos_Finiquitos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType.Equals(DataControlRowType.DataRow))
        {
            ((TextBox)e.Row.Cells[4].FindControl("Txt_Cantidad_Deduccion_Percepcion")).ToolTip = "" + e.Row.RowIndex;
            ((TextBox)e.Row.Cells[4].FindControl("Txt_Cantidad_Deduccion_Percepcion")).Enabled = false;
        }
    }
    #endregion

    #region (Gridview Resguardos Empleado)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Resguardos_Empleado_PageIndexChanging
    ///DESCRIPCIÓN: Cambia de pagina al Grid de Resguardos del Empleado. 
    ///             
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 03/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Resguardos_Empleado_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Resguardos_Empleado.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
            Consultar_Resguardos_Empleado_Actualmente();//Carga los Sindicatos que estan asignadas a la página seleccionada
            Grid_Resguardos_Empleado.SelectedIndex = -1;
            ScriptManager.RegisterStartupScript(UPnl_Generacion_Finiquitos, typeof(string), "Inicializar Eventos JQuery", "inicializarEventos()", true); 
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    #endregion

    #endregion

    #region (Eventos)

    #region (Botones)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Finiquito_Click
    ///DESCRIPCIÓN: Se encarga de tomar todas las percepciones y/o deducciones. a considerar
    ///             para el finiquito
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 04/Febrero/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Generar_Finiquito_Click(object sender, EventArgs e)
    {
        Cls_Ope_Nom_Finiquitos_Negocio Finiquitos_Negocio = new Cls_Ope_Nom_Finiquitos_Negocio();//Variable de conexion con la capa de negocios.
        String Ruta_Guardar_Archivo = "";//Variable que almacenará la ruta completa donde se guardara el log de la generacion de la nómina.
        StringBuilder Historial_Nomina_Generada = new StringBuilder();//Variable que almacenará todos los cambios realizados al generar la nómina, para poder hacer un rollback si asi es necesario.
        DataSet Ds_Tablas_Afectadas_Generacion_Nomina = null;//Variable que almacena las tabla que fueron afectadas en algunos registros al generar la nómina.
        String Nomina_ID = "";//Variable que almacenará el Identificador de la nómina.
        Int32 No_Nomina = 0;//Variable que almacena el numero de catorcena de la cual se desea generar la nómina.
        String Detalle_Nomina_ID = "";//Variable que identifica el perido seleccionado para generar la nómina.
        String Tipo_Nomina_ID = "";//Variable que almacena el tipo de nómina de la cual se desea generar la nómina.

        try
        {
            if (Validar_Informacion_Pagina())
            {
                //Obtenemos la ruta donde se guardara el log de la nómina.
                Ruta_Guardar_Archivo = Server.MapPath("Log_Finiquito");
                //Verificamos si el directorio del log de la nómina existe, en caso contrario se crea. 
                if (!Directory.Exists(Ruta_Guardar_Archivo))
                    Directory.CreateDirectory(Ruta_Guardar_Archivo);


                //Establecemos la variable de session que alamacenará la información historica de los registros
                //que tuvieron alguna afectación, al realizar el barrido de la nómina.
                Cls_Sessiones.Historial_Nomina_Generada = Historial_Nomina_Generada;


                if (Btn_Generar_Finiquito.Text.Equals("Regenerar Finiquito"))
                {
                    //Obtenemos la información de las tablas que fueron afectadas al generar la nómina [PRESTAMOS, AJUSTES DE ISR, RECIBOS DE LA NÓMINA Y TOTALES DE LA NÓMINA].
                    Ds_Tablas_Afectadas_Generacion_Nomina = Cls_Historial_Nomina_Generada.Leer_Archivo_Obtener_Historial_Nomina_Generada(@Ruta_Guardar_Archivo, "/Log_Finiquito", ".txt");
                    //Se ejecuta la actualziación de las tablas afectadas, haciendo un RollBack de los datos afectados.
                    Finiquitos_Negocio.RollBack_Registros_Afectadoos_Generacion_Nomina(Ds_Tablas_Afectadas_Generacion_Nomina);
                    //Validamos que exista el archivo que guarda las tablas que fueron afectadas durante la generación de la nómina.
                    if (File.Exists(@Ruta_Guardar_Archivo + "/Log_Finiquito" + ".txt"))
                    {
                        //Eliminamos el archivo que guardael Historial de la nomina una vez que ya se ha regenerado la nómina.
                        File.Delete(@Ruta_Guardar_Archivo + "/Log_Finiquito" + ".txt");
                    }
                }
                else if (Btn_Generar_Finiquito.Text.Equals("Generar Finiquito"))
                {
                    //Validamos que exista el archivo que guarda las tablas que fueron afectadas durante la generación de la nómina.
                    if (File.Exists(@Ruta_Guardar_Archivo + "/Log_Finiquito" + ".txt"))
                    {
                        //Eliminamos el archivo que guardael Historial de la nomina una vez que ya se ha regenerado la nómina.
                        File.Delete(@Ruta_Guardar_Archivo + "/Log_Finiquito" + ".txt");
                    }
                }

                //Guardamos los datatables para evitar perder la información que ya ha sido actualizada.
                Session["Dt_PERCEPCIONES"] = Crear_DataTable_Percepciones_Deducciones(Grid_Percepciones, "Txt_Cantidad_Percepcion", "Chk_Aplica_Deduccion_Finiquito"); ;
                Session["Dt_DEDUCCIONES"] = Crear_DataTable_Percepciones_Deducciones(Grid_Deducciones, "Txt_Cantidad_Deduccion", "Chk_Aplica_Percepcion_Finiquito");
                Session["Dt_CONCEPTOS_FINIQUITOS"] = Crear_DataTable_Percepciones_Deducciones(Grid_Conceptos_Exclusivos_Finiquitos, "Txt_Cantidad_Deduccion_Percepcion", "Chk_Aplica_Percepcion_Finiquito");

                //Volvemos a consultar la información para generar los datos que almacenaremos en el archivo que
                //se usara para la regeneración del finiquito.
                Txt_No_Empleado_TextChanged(sender, e);

                //Obtenemos los valores que son necesarios para realizar la generación del finiquito.
                Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
                Detalle_Nomina_ID = Cmb_Periodo.SelectedValue.Trim();
                No_Nomina = Convert.ToInt32(Cmb_Periodo.SelectedItem.Text.Trim());
                Tipo_Nomina_ID = Cmb_Tipo_Nomina.SelectedValue.Trim();

                Finiquitos_Negocio.P_Nomina_ID = Nomina_ID;
                Finiquitos_Negocio.P_No_Nomina = No_Nomina;
                Finiquitos_Negocio.P_Detalle_Nomina_ID = Detalle_Nomina_ID;
                Finiquitos_Negocio.P_Tipo_Nomina_ID = Tipo_Nomina_ID;

                //Genenramos el Esquema de la Tabla de Totales de la Nómina a Generar.
                Finiquitos_Negocio.Plantilla_Total_Percepciones_Deducciones();

                //Ejecutamos el Finiquito del Empleado.
                if (Ejecuta_Finiquito_Empleado())
                {
                    Ejecuta_Finiquito_Empleado_Real();
                    //Aqui se guarda la información historica de la nómina generada. 
                    //Registrando cada movimiento que se realizo al generar la nómina en Prestamos y Ajustes de ISR. 
                    Cls_Historial_Nomina_Generada.Escribir_Archivo_Historial_Nomina_Generada(Ruta_Guardar_Archivo, "/Log_Finiquito", ".txt", Cls_Sessiones.Historial_Nomina_Generada);
                    //Eliminamos la Session que almacena, los registros almacenados de Prestamos, Ajustes de ISR, Recibos de la Nómina y Totales de la Nomina.
                    Cls_Sessiones.Historial_Nomina_Generada = null;
                    Afectar_Registros_Prestamos_Ajustes_ISR();

                    //Volvemos a llamar a la función que inicializa el control de los eventos jquery.
                    ScriptManager.RegisterStartupScript(UPnl_Generacion_Finiquitos, typeof(string), "Inicializar Eventos JQuery", "inicializarEventos()", true);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Generar Finiquito", "alert('La Generación del Finiquito se a Compleatado.');", true);
                }
                ScriptManager.RegisterStartupScript(UPnl_Generacion_Finiquitos, typeof(string), "Inicializar Eventos JQuery", "inicializarEventos()", true);
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
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Empleado_Click
    ///DESCRIPCIÓN: Consulta al Empleado en el sistema por su número de empleado.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 04/Febrero/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Empleado_Click(object sender, ImageClickEventArgs e)
    {
        StringBuilder Historial_Nomina_Generada = new StringBuilder();//Variable que almacenará todos los cambios realizados al generar la nómina, para poder hacer un rollback si asi es necesario.

        try
        {
            Cls_Sessiones.Historial_Nomina_Generada = Historial_Nomina_Generada;

            if (Consultar_Mostrar_Informacion_Empleado())
            {
                Consultar_Percepciones_Deducciones_Aplican_Empleado();//Se consultan las percepciones y/o deducciones que aplican para el cálculo del finiquito del empleado.
                Consultar_Resguardos_Empleado_Actualmente();//Se consulta si el empleado actualmente tiene algun resguardo actualmente.
                Validar_Generar_O_Regenerar_Finiquito_Empleado();

                Btn_Generar_Pre_Recibo_Finiquito.Enabled = true;
                Txt_No_Empleado.Enabled = false;
                Cmb_Tipo_Salario.Enabled = false;
            }

            //Volvemos a llamar a la función que inicializa el control de los eventos jquery.
            ScriptManager.RegisterStartupScript(UPnl_Generacion_Finiquitos, typeof(string), "Inicializar Eventos JQuery", "inicializarEventos()", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: IBtn_Refrescar_Pantalla_Click
    ///DESCRIPCIÓN: Refresca la pantalla de finiquitos.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 04/Febrero/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void IBtn_Refrescar_Pantalla_Click(object sender, ImageClickEventArgs e) {
        Response.Redirect("Frm_Ope_Nom_Generacion_Finiquitos.aspx?PAGINA=" + Request.QueryString["PAGINA"]);
        //Volvemos a llamar a la función que inicializa el control de los eventos jquery.
        ScriptManager.RegisterStartupScript(UPnl_Generacion_Finiquitos, typeof(string), "Inicializar Eventos JQuery", "inicializarEventos()", true);
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Cerrar_Finiquito_Click
    ///DESCRIPCIÓN: Se encarga de cerrar el pago del finiquito al empleado
    ///
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Cerrar_Finiquito_Click(object sender, EventArgs e)
    {
        String Ruta_Guardar_Archivo = "";//Variable que almacenará la ruta completa donde se guardara el log de la generacion de la nómina.
        Cls_Cat_Empleados_Negocios Informacion_Empleado = new Cls_Cat_Empleados_Negocios();//Variable de conexión con la capa de negocios.
        DataTable Dt_Tabla_Empleados = null;//Información del empleado.
        String Empleado_ID = "";//Identificador del empleado.

        try
        {
            //Obtenemos la ruta donde se guardara el log de la nómina.
            Ruta_Guardar_Archivo = Server.MapPath("Log_Finiquito");
            //Verificamos si el directorio del log de la nómina existe, en caso contrario se crea. 
            if (Directory.Exists(Ruta_Guardar_Archivo))
            {
                if (File.Exists(@Ruta_Guardar_Archivo + "/Log_Finiquito" + ".txt"))
                {
                    //Eliminamos el archivo que guardael Historial de la nomina una vez que ya se ha regenerado la nómina.
                    File.Delete(@Ruta_Guardar_Archivo + "/Log_Finiquito" + ".txt");
                }
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Cerrar Finiquito", "alert('Finiquito Cerrado');", true);
            //Limpiar_Controles();
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    /// ***************************************************************************************************************************
    /// Nombre: Btn_Actualizar_Tablas_Conceptos_Click
    /// 
    /// Descripción: Evento que actualiza las tablas de percepciones y deducciones que aplican para el finiquito. aquí
    ///              se hará el recalculo de algunas percepciones y deducciones.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 30/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// ***************************************************************************************************************************
    protected void Btn_Actualizar_Tablas_Conceptos_Click(Object sender, EventArgs e)
    {
        Cls_Ayudante_Exentos_Gravados_Nomina Obj_Exentos_Gravados = new Cls_Ayudante_Exentos_Gravados_Nomina();
        DataTable Dt_Percepciones = null;
        DataTable Dt_Deducciones = null;
        DataTable Dt_Conceptos_Finiquito = null;
        String Nomina_ID = String.Empty;
        String No_Nomina = String.Empty;
        String Tipo_Nomina_ID = String.Empty;
        String Fecha_Baja_Empleado = String.Empty;

        try
        {
            Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
            No_Nomina = Cmb_Periodo.SelectedItem.Text.Trim();
            Tipo_Nomina_ID = Cmb_Tipo_Nomina.SelectedValue.Trim();
            Fecha_Baja_Empleado = Txt_Fecha_Baja.Text.Trim();

            //PASO I.- Creamos las tablas de percepciones y deducciones a partir de los grids de percepciones y deducciones.
            Dt_Percepciones = Crear_DataTable_Percepciones_Deducciones(Grid_Percepciones, "Txt_Cantidad_Percepcion", "Chk_Aplica_Deduccion_Finiquito");
            Dt_Deducciones = Crear_DataTable_Percepciones_Deducciones(Grid_Deducciones, "Txt_Cantidad_Deduccion", "Chk_Aplica_Percepcion_Finiquito");
            Dt_Conceptos_Finiquito = Crear_DataTable_Percepciones_Deducciones(Grid_Conceptos_Exclusivos_Finiquitos, "Txt_Cantidad_Deduccion_Percepcion", "Chk_Aplica_Percepcion_Finiquito");

            //PASO II.- Actualizamos las cantidades de las tablas de percepciones y deducciones. 
            //          con las cantidades cambiadas en las tablas de conceptos que solo aplican
            //          al finiquito.
            Actualizar_Cantidades_Percepciones_Deducciones(Dt_Conceptos_Finiquito, ref Dt_Percepciones);
            Actualizar_Cantidades_Percepciones_Deducciones(Dt_Conceptos_Finiquito, ref Dt_Deducciones);

            //PASO III.- actualizamos los montos de las cantidades que gravan y exentan la percepciones que se le aplican
            //           al empleado.
            Obj_Exentos_Gravados.Calcular_Exento_Gravado(ref Dt_Percepciones, ref Dt_Deducciones, Txt_No_Empleado.Text.Trim(), Nomina_ID,
                No_Nomina, Tipo_Nomina_ID, Fecha_Baja_Empleado);

            //PASO IV.- Crear una tabla con los conceptos que son propios del finiquito.
            Dt_Conceptos_Finiquito = new DataTable();
            Obtener_Percepciones_Deducciones_Propias_Finiquito(Dt_Percepciones, ref Dt_Conceptos_Finiquito);
            Obtener_Percepciones_Deducciones_Propias_Finiquito(Dt_Deducciones, ref Dt_Conceptos_Finiquito);

            //PASO V.- Se carga el grid percepciones.
            Grid_Percepciones.Columns[1].Visible = true;
            Grid_Percepciones.Columns[6].Visible = true;
            Grid_Percepciones.Columns[7].Visible = true;
            Dt_Percepciones.DefaultView.Sort = "TIPO_ASIGNACION ASC";
            Grid_Percepciones.DataSource = Dt_Percepciones;
            Grid_Percepciones.DataBind();
            Grid_Percepciones.Columns[1].Visible = false;
            Grid_Percepciones.Columns[6].Visible = false;
            Grid_Percepciones.Columns[7].Visible = false;
            Cargar_Cantidad_Grid_Percepciones_Deducciones(Grid_Percepciones, Dt_Percepciones, "Txt_Cantidad_Percepcion");

            //PASO VI.- Se cargan las deducciones.
            Grid_Deducciones.Columns[1].Visible = true;
            Dt_Deducciones.DefaultView.Sort = "TIPO_ASIGNACION ASC";
            Grid_Deducciones.DataSource = Dt_Deducciones;
            Grid_Deducciones.DataBind();
            Grid_Deducciones.Columns[1].Visible = false;
            Cargar_Cantidad_Grid_Percepciones_Deducciones(Grid_Deducciones, Dt_Deducciones, "Txt_Cantidad_Deduccion");

            //PASO VII.- Se cargan los conceptos que solo aplican al finiquito.
            Grid_Conceptos_Exclusivos_Finiquitos.Columns[1].Visible = true;
            Grid_Conceptos_Exclusivos_Finiquitos.Columns[6].Visible = true;
            Grid_Conceptos_Exclusivos_Finiquitos.Columns[7].Visible = true;
            Dt_Conceptos_Finiquito.DefaultView.Sort = "TIPO_ASIGNACION ASC";
            Grid_Conceptos_Exclusivos_Finiquitos.DataSource = Dt_Conceptos_Finiquito;
            Grid_Conceptos_Exclusivos_Finiquitos.DataBind();
            Grid_Conceptos_Exclusivos_Finiquitos.Columns[1].Visible = false;
            Grid_Conceptos_Exclusivos_Finiquitos.Columns[6].Visible = false;
            Grid_Conceptos_Exclusivos_Finiquitos.Columns[7].Visible = false;
            Cargar_Cantidad_Grid_Percepciones_Deducciones(Grid_Conceptos_Exclusivos_Finiquitos, Dt_Conceptos_Finiquito, "Txt_Cantidad_Deduccion_Percepcion");

            //PASO VIII.- Se ocultan los conceptos que se mostraran en la tabla de conceptos exclusivos del finiquito.
            Ocultar_Percepciones_Exclusivas_Finiquito(Dt_Conceptos_Finiquito, ref Grid_Percepciones);
            Ocultar_Percepciones_Exclusivas_Finiquito(Dt_Conceptos_Finiquito, ref Grid_Deducciones);

            ScriptManager.RegisterStartupScript(UPnl_Generacion_Finiquitos, typeof(string), "Inicializar Eventos JQuery", "inicializarEventos()", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al actualizar los registros de las tablas de percepciones deducciones. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Combos)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Periodos_Catorcenales_Nomina
    ///DESCRIPCIÓN: Consulta los periodos catorcenales de la nomina seleccionada.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Calendario_Nomina_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 index = 0;//Variable que almacena el elemento seleccionado del combo.
        try
        {
            //Obtenemos elemento seleccionado del combo.
            index = Cmb_Calendario_Nomina.SelectedIndex;

            if (index > 0)
            {
                Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario_Nomina.SelectedValue.Trim());
                //Volvemos a llamar a la función que inicializa el control de los eventos jquery.
                ScriptManager.RegisterStartupScript(UPnl_Generacion_Finiquitos, typeof(string), "Inicializar Eventos JQuery", "inicializarEventos()", true);
            }
            else
            {
                Cmb_Periodo.DataSource = new DataTable();
                Cmb_Periodo.DataBind();
                Txt_No_Empleado.Enabled = false;
                Btn_Buscar_Empleado.Enabled = false;
                //Volvemos a llamar a la función que inicializa el control de los eventos jquery.
                ScriptManager.RegisterStartupScript(UPnl_Generacion_Finiquitos, typeof(string), "Inicializar Eventos JQuery", "inicializarEventos()", true);
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
        String Ruta_Guardar_Archivo = "";//Variable que almacenará la ruta completa donde se guardara el log de la generacion de la nómina.
        StringBuilder Historial_Nomina_Generada = new StringBuilder();//Variable que almacenará todos los cambios realizados al generar la nómina, para poder hacer un rollback si asi es necesario.
        DataSet Ds_Tablas_Afectadas_Generacion_Nomina = null;//Variable que almacena las tabla que fueron afectadas en algunos registros al generar la nómina.
        DataTable Dt_Recibo_Nomina = null;//Variable que almacena el recibo de nómina generado en el finiquito.
        DataTable Dt_Recibo_Previamente_Generado = null;
        DataTable Dt_Tabla_Empleados = null;
        String No_Recibo_Empleado = "";
        Cls_Ope_Nom_Pestamos_Negocio Prestamos = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Ope_Nom_Recibos_Empleados_Negocio Recibos_Empleados = new Cls_Ope_Nom_Recibos_Empleados_Negocio();//Variable de conexión a la capa de negocios. 
        Cls_Ope_Nom_Finiquitos_Negocio Generar_Finiquitos = new Cls_Ope_Nom_Finiquitos_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Cat_Empleados_Negocios Empleados_Informacion = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios.
        DataTable Dt_Detalles_Nomina = null;//Variable que almacenra los detalles del periodo seleccionado.
        DataTable Dt_Nominas_Generadas = null;
        DateTime Fecha_Inicio = new DateTime();
        DateTime Fecha_Fin = new DateTime();
        String Empleado_Con_Registro_Finiquito = "";
        String Empleado_A_Registrar_Finiquito = "";

        try
        {
            if (Cmb_Periodo.SelectedIndex > 0)
            {
                Prestamos.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
                Prestamos.P_No_Nomina = Convert.ToInt32(Cmb_Periodo.SelectedItem.Text.Trim());
                Dt_Detalles_Nomina = Prestamos.Consultar_Fechas_Periodo();

                if (Dt_Detalles_Nomina != null)
                {
                    if (Dt_Detalles_Nomina.Rows.Count > 0)
                    {
                        Fecha_Inicio = Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString());
                        Fecha_Fin = Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString());

                        Txt_Inicia_Catorcena.Text = string.Format("{0:dd/MMM/yyyy}", Fecha_Inicio);
                        Txt_Fin_Catorcena.Text = string.Format("{0:dd/MMM/yyyy}", Fecha_Fin);

                        Txt_No_Empleado.Enabled = true;
                        Btn_Buscar_Empleado.Enabled = true;
                    }
                }

                //Volvemos a llamar a la función que inicializa el control de los eventos jquery.
                ScriptManager.RegisterStartupScript(UPnl_Generacion_Finiquitos, typeof(string), "Inicializar Eventos JQuery", "inicializarEventos()", true);
            }
            else
            {
                Txt_No_Empleado.Enabled = false;
                Btn_Buscar_Empleado.Enabled = false;
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    /// ***************************************************************************************************************************
    /// Nombre: Cmb_Tipo_Salario_SelectedIndexChanged
    /// 
    /// Descripción: Método que al seleccionar un tipo de salario ya sea salario de la zona economica del empleado o su 
    ///              salario diario. Para en base a este parámetro realizar el cálculo.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 30/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// ***************************************************************************************************************************
    protected void Cmb_Tipo_Salario_SelectedIndexChanged(Object sender, EventArgs e)
    {
        Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;
        Cls_Cat_Nom_Zona_Economica_Negocio INF_ZONA_ECONOMICA = null;
        Cls_Cat_Nom_Zona_Economica_Negocio Obj_Zona_Economica = new Cls_Cat_Nom_Zona_Economica_Negocio();//Variable de conexión de la zona económica.
        DataTable Dt_Informacion = null;//Variable que almacena  la información consultada.
       

        try
        {
            Btn_Buscar_Empleado.Visible = true;

            if (!String.IsNullOrEmpty(Txt_No_Empleado.Text.Trim()))
            {
                //CONSULTAR LA INFORMACIÓN DEL EMPLEADO.
                INF_EMPLEADO = Cls_Ayudante_Nom_Informacion._Informacion_Empleado(Txt_No_Empleado.Text.Trim());
                //CONSULTAR LA INFORMACIÓN DE LA ZONA ECONÓMICA.
                INF_ZONA_ECONOMICA = Cls_Ayudante_Nom_Informacion._Informacion_Zona_Economica();

                if (Cmb_Tipo_Salario.SelectedIndex > 0)
                {
                    if (Cmb_Tipo_Salario.SelectedItem.Text.Trim().Equals("Salario Diario"))
                    {
                        Lbl_Cantidad_Salario.Text = String.Format("{0:c}", Cls_Ayudante_Nom_Informacion.Obtener_Cantidad_Diaria(INF_EMPLEADO.P_Empleado_ID));
                    }
                    else if (Cmb_Tipo_Salario.SelectedItem.Text.Trim().Equals("SMG2"))
                    {
                        Double SMG2 = (INF_ZONA_ECONOMICA.P_Salario_Diario * 2);
                        Lbl_Cantidad_Salario.Text = String.Format("{0:c}", SMG2);
                    }
                }
                else
                {
                    Btn_Buscar_Empleado.Visible = false;
                }
            }
            else return;

            //Volvemos a llamar a la función que inicializa el control de los eventos jquery.
            ScriptManager.RegisterStartupScript(UPnl_Generacion_Finiquitos, typeof(string), "Inicializar Eventos JQuery", "inicializarEventos()", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al seleccionar un tipo de salario. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Cajas de Texto)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_No_Empleado_TextChanged
    ///DESCRIPCIÓN: Consulta al Empleado en el sistema por su número de empleado.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 04/Febrero/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_No_Empleado_TextChanged(object sender, EventArgs e)
    {
        StringBuilder Historial_Nomina_Generada = new StringBuilder();//Variable que almacenará todos los cambios realizados al generar la nómina, para poder hacer un rollback si asi es necesario.

        try
        {
            Cls_Sessiones.Historial_Nomina_Generada = Historial_Nomina_Generada;

            if (Consultar_Mostrar_Informacion_Empleado())
            {
                Consultar_Percepciones_Deducciones_Aplican_Empleado();//Se consultan las percepciones y/o deducciones que aplican para el cálculo del finiquito del empleado.
                Consultar_Resguardos_Empleado_Actualmente();//Se consulta si el empleado actualmente tiene algun resguardo actualmente.
                Validar_Generar_O_Regenerar_Finiquito_Empleado();
            }

            //Volvemos a llamar a la función que inicializa el control de los eventos jquery.
            ScriptManager.RegisterStartupScript(UPnl_Generacion_Finiquitos, typeof(string), "Inicializar Eventos JQuery", "inicializarEventos()", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }
    #endregion

    #region (Eventos Reportes)
    /// ***********************************************************************************************
    /// NOMBRE: Btn_Generar_Pre_Recibo_Finiquito_Click
    /// 
    /// DESCRIPCIÓN: Evento que ejecuta el reporte de Pre-Recibo finiquitos.
    /// 
    /// PARÁMETROS: No Aplica.
    /// 
    /// USUARIO CREÓ:Juan Alberto Hernández Negrete
    /// FECHA CREÓ: 21/Junio/2011 13:53 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// ***********************************************************************************************
    protected void Btn_Generar_Pre_Recibo_Finiquito_Click(Object sender, EventArgs e)
    {
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();
        System.Data.DataTable Dt_Percepciones = null;
        DataTable Dt_Totales_Percepciones = null;
        DataTable Dt_Deducciones = null;
        DataTable Dt_Totales_Deducciones = null;
        DataTable Dt_Conceptos_Finiquito = null;
        DataTable Dt_Empleados = null;
        System.Data.DataSet Ds_Pre_Recibo_Finiquito = null;
        DataSet Ds_Percepciones = null;
        DataSet Ds_Deducciones = null;
        Double TOTAL_PERCEPCIONES = 0.0;
        Double TOTAL_DEDUCCIONES = 0.0;

        try
        {

            Obj_Empleados.P_No_Empleado = Txt_No_Empleado.Text.Trim();
            Obj_Empleados.P_Estatus = "INACTIVO";
            Dt_Empleados = Obj_Empleados.Consultar_Informacion_Mostrar_Finiquitos();

            Ds_Pre_Recibo_Finiquito = new System.Data.DataSet();
            Dt_Percepciones = Crear_DataTable_Percepciones_Deducciones(Grid_Percepciones, "Txt_Cantidad_Percepcion", "Chk_Aplica_Deduccion_Finiquito");
            Dt_Deducciones = Crear_DataTable_Percepciones_Deducciones(Grid_Deducciones, "Txt_Cantidad_Deduccion", "Chk_Aplica_Percepcion_Finiquito");
            Dt_Conceptos_Finiquito = Crear_DataTable_Percepciones_Deducciones(Grid_Conceptos_Exclusivos_Finiquitos, "Txt_Cantidad_Deduccion_Percepcion", "Chk_Aplica_Percepcion_Finiquito");

            Actualizar_Cantidades_Percepciones_Deducciones(Dt_Conceptos_Finiquito, ref Dt_Percepciones);
            Actualizar_Cantidades_Percepciones_Deducciones(Dt_Conceptos_Finiquito, ref Dt_Deducciones);

            Ds_Percepciones = Crear_Lista_Percepciones_Con_Monto(Dt_Percepciones);
            Ds_Deducciones = Crear_Lista_Deducciones_Con_Monto(Dt_Deducciones);

            Dt_Percepciones = Ds_Percepciones.Tables[0];
            Dt_Totales_Percepciones = Ds_Percepciones.Tables[1];

            Dt_Deducciones = Ds_Deducciones.Tables[0];
            Dt_Totales_Deducciones = Ds_Deducciones.Tables[1];

            TOTAL_PERCEPCIONES = Obtener_Cantidad(Dt_Totales_Percepciones);
            TOTAL_DEDUCCIONES = Obtener_Cantidad(Dt_Totales_Deducciones);

            Agregar_Total_Letra(ref Dt_Totales_Percepciones, (TOTAL_PERCEPCIONES - TOTAL_DEDUCCIONES));
            Agregar_Total_Letra(ref Dt_Totales_Deducciones, (TOTAL_PERCEPCIONES - TOTAL_DEDUCCIONES));

            Dt_Percepciones.TableName = "Dt_Percepciones";
            Dt_Totales_Percepciones.TableName = "Total_Percepciones";
            Dt_Deducciones.TableName = "Dt_Deducciones";
            Dt_Totales_Deducciones.TableName = "Total_Deducciones";
            Dt_Empleados.TableName = "INF_EMPLEADO";

            Ds_Pre_Recibo_Finiquito.Tables.Add(Dt_Percepciones.Copy());
            Ds_Pre_Recibo_Finiquito.Tables.Add(Dt_Totales_Percepciones.Copy());
            Ds_Pre_Recibo_Finiquito.Tables.Add(Dt_Deducciones.Copy());
            Ds_Pre_Recibo_Finiquito.Tables.Add(Dt_Totales_Deducciones.Copy());
            Ds_Pre_Recibo_Finiquito.Tables.Add(Dt_Empleados.Copy());

            Generar_Reporte(ref Ds_Pre_Recibo_Finiquito, "Cr_Rpt_Nom_Pre_Recibo_Finiquito.rpt", "Pre_Recibo_Finiquito" + Session.SessionID + ".pdf");

            //Volvemos a llamar a la función que inicializa el control de los eventos jquery.
            ScriptManager.RegisterStartupScript(UPnl_Generacion_Finiquitos, typeof(string), "Inicialzar Eventos JQuery", "inicializarEventos()", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }
    /// ***********************************************************************************************
    /// NOMBRE: Btn_Generar_Recibo_Finiquito_Click
    /// 
    /// DESCRIPCIÓN: Evento que ejecuta el reporte de Recibo finiquitos.
    /// 
    /// PARÁMETROS: No Aplica.
    /// 
    /// USUARIO CREÓ:Juan Alberto Hernández Negrete
    /// FECHA CREÓ: 21/Junio/2011 13:53 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// ***********************************************************************************************
    protected void Btn_Generar_Recibo_Finiquito_Click(Object sender, EventArgs e)
    {
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();
        System.Data.DataTable Dt_Percepciones = null;
        DataTable Dt_Totales_Percepciones = null;
        DataTable Dt_Deducciones = null;
        DataTable Dt_Totales_Deducciones = null;
        DataTable Dt_Conceptos_Finiquito = null;
        DataTable Dt_Empleados = null;
        System.Data.DataSet Ds_Pre_Recibo_Finiquito = null;
        DataSet Ds_Percepciones = null;
        DataSet Ds_Deducciones = null;
        Double TOTAL_PERCEPCIONES = 0.0;
        Double TOTAL_DEDUCCIONES = 0.0;

        try
        {

            Obj_Empleados.P_No_Empleado = Txt_No_Empleado.Text.Trim();
            Obj_Empleados.P_Estatus = "INACTIVO";
            Dt_Empleados = Obj_Empleados.Consultar_Informacion_Mostrar_Finiquitos();

            Ds_Pre_Recibo_Finiquito = new System.Data.DataSet();
            Dt_Percepciones = Crear_DataTable_Percepciones_Deducciones(Grid_Percepciones, "Txt_Cantidad_Percepcion", "Chk_Aplica_Deduccion_Finiquito");
            Dt_Deducciones = Crear_DataTable_Percepciones_Deducciones(Grid_Deducciones, "Txt_Cantidad_Deduccion", "Chk_Aplica_Percepcion_Finiquito");
            Dt_Conceptos_Finiquito = Crear_DataTable_Percepciones_Deducciones(Grid_Conceptos_Exclusivos_Finiquitos, "Txt_Cantidad_Deduccion_Percepcion", "Chk_Aplica_Percepcion_Finiquito");

            Actualizar_Cantidades_Percepciones_Deducciones(Dt_Conceptos_Finiquito, ref Dt_Percepciones);
            Actualizar_Cantidades_Percepciones_Deducciones(Dt_Conceptos_Finiquito, ref Dt_Deducciones);

            Ds_Percepciones = Crear_Lista_Percepciones_Con_Monto(Dt_Percepciones);
            Ds_Deducciones = Crear_Lista_Deducciones_Con_Monto(Dt_Deducciones);

            Dt_Percepciones = Ds_Percepciones.Tables[0];
            Dt_Totales_Percepciones = Ds_Percepciones.Tables[1];

            Dt_Deducciones = Ds_Deducciones.Tables[0];
            Dt_Totales_Deducciones = Ds_Deducciones.Tables[1];

            TOTAL_PERCEPCIONES = Obtener_Cantidad(Dt_Totales_Percepciones);
            TOTAL_DEDUCCIONES = Obtener_Cantidad(Dt_Totales_Deducciones);

            Agregar_Total_Letra(ref Dt_Totales_Percepciones, (TOTAL_PERCEPCIONES - TOTAL_DEDUCCIONES));
            Agregar_Total_Letra(ref Dt_Totales_Deducciones, (TOTAL_PERCEPCIONES - TOTAL_DEDUCCIONES));

            Dt_Percepciones.TableName = "Dt_Percepciones";
            Dt_Totales_Percepciones.TableName = "Total_Percepciones";
            Dt_Deducciones.TableName = "Dt_Deducciones";
            Dt_Totales_Deducciones.TableName = "Total_Deducciones";
            Dt_Empleados.TableName = "INF_EMPLEADO";

            Ds_Pre_Recibo_Finiquito.Tables.Add(Dt_Percepciones.Copy());
            Ds_Pre_Recibo_Finiquito.Tables.Add(Dt_Totales_Percepciones.Copy());
            Ds_Pre_Recibo_Finiquito.Tables.Add(Dt_Deducciones.Copy());
            Ds_Pre_Recibo_Finiquito.Tables.Add(Dt_Totales_Deducciones.Copy());
            Ds_Pre_Recibo_Finiquito.Tables.Add(Dt_Empleados.Copy());

            Generar_Reporte(ref Ds_Pre_Recibo_Finiquito, "Cr_Rpt_Nom_Recibo_Finiquito.rpt", "Recibo_Finiquito" + Session.SessionID + ".pdf");
            //Volvemos a llamar a la función que inicializa el control de los eventos jquery.
            ScriptManager.RegisterStartupScript(UPnl_Generacion_Finiquitos, typeof(string), "Inicialzar Eventos JQuery", "inicializarEventos()", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte de recibo del finiquito. Error: [" + Ex.Message + "]");
        }
    }
    /// ***********************************************************************************************
    /// NOMBRE: Btn_Generar_Reporte_Liquidacion_Click
    /// 
    /// DESCRIPCIÓN: Evento que ejecuta el reporte de liquidacion del empleado.
    /// 
    /// PARÁMETROS: No Aplica.
    /// 
    /// USUARIO CREÓ:Juan Alberto Hernández Negrete
    /// FECHA CREÓ: 21/Junio/2011 13:53 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// ***********************************************************************************************
    protected void Btn_Generar_Reporte_Liquidacion_Click(Object sender, EventArgs e)
    {
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();
        DataTable Dt_Empleados = null;


        DataTable Dt_Percepciones = null;
        DataTable Dt_Totales_Percepciones = null;
        DataTable Dt_Deducciones = null;
        DataTable Dt_Totales_Deducciones = null;
        DataTable Dt_Conceptos_Finiquito = null;
        DataSet Ds_Pre_Recibo_Finiquito = null;
        DataSet Ds_Percepciones = null;
        DataSet Ds_Deducciones = null;
        Double TOTAL_PERCEPCIONES = 0.0;
        Double TOTAL_DEDUCCIONES = 0.0;

        try
        {
            Obj_Empleados.P_No_Empleado = Txt_No_Empleado.Text.Trim();
            Obj_Empleados.P_Estatus = "INACTIVO";
            Dt_Empleados = Obj_Empleados.Consultar_Informacion_Rpt_Finiquitos();

            Ds_Pre_Recibo_Finiquito = new System.Data.DataSet();
            Dt_Percepciones = Crear_DataTable_Percepciones_Deducciones(Grid_Percepciones, "Txt_Cantidad_Percepcion", "Chk_Aplica_Deduccion_Finiquito");
            Dt_Deducciones = Crear_DataTable_Percepciones_Deducciones(Grid_Deducciones, "Txt_Cantidad_Deduccion", "Chk_Aplica_Percepcion_Finiquito");
            Dt_Conceptos_Finiquito = Crear_DataTable_Percepciones_Deducciones(Grid_Conceptos_Exclusivos_Finiquitos, "Txt_Cantidad_Deduccion_Percepcion", "Chk_Aplica_Percepcion_Finiquito");

            Actualizar_Cantidades_Percepciones_Deducciones(Dt_Conceptos_Finiquito, ref Dt_Percepciones);
            Actualizar_Cantidades_Percepciones_Deducciones(Dt_Conceptos_Finiquito, ref Dt_Deducciones);

            Ds_Percepciones = Crear_Lista_Percepciones_Con_Monto(Dt_Percepciones);
            Ds_Deducciones = Crear_Lista_Deducciones_Con_Monto(Dt_Deducciones);

            Dt_Percepciones = Ds_Percepciones.Tables[0];
            Dt_Totales_Percepciones = Ds_Percepciones.Tables[1];

            Dt_Deducciones = Ds_Deducciones.Tables[0];
            Dt_Totales_Deducciones = Ds_Deducciones.Tables[1];

            TOTAL_PERCEPCIONES = Obtener_Cantidad(Dt_Totales_Percepciones);
            TOTAL_DEDUCCIONES = Obtener_Cantidad(Dt_Totales_Deducciones);

            Agregar_Total_Letra(ref Dt_Totales_Percepciones, (TOTAL_PERCEPCIONES - TOTAL_DEDUCCIONES));
            Agregar_Total_Letra(ref Dt_Totales_Deducciones, (TOTAL_PERCEPCIONES - TOTAL_DEDUCCIONES));

            Dt_Percepciones.TableName = "Dt_Percepciones";
            Dt_Totales_Percepciones.TableName = "Total_Percepciones";
            Dt_Deducciones.TableName = "Dt_Deducciones";
            Dt_Totales_Deducciones.TableName = "Total_Deducciones";
            Dt_Empleados.TableName = "Liquidacion_Empleado";

            Ds_Pre_Recibo_Finiquito.Tables.Add(Dt_Percepciones.Copy());
            Ds_Pre_Recibo_Finiquito.Tables.Add(Dt_Totales_Percepciones.Copy());
            Ds_Pre_Recibo_Finiquito.Tables.Add(Dt_Deducciones.Copy());
            Ds_Pre_Recibo_Finiquito.Tables.Add(Dt_Totales_Deducciones.Copy());
            Ds_Pre_Recibo_Finiquito.Tables.Add(Dt_Empleados.Copy());

            Generar_Reporte(ref Ds_Pre_Recibo_Finiquito, "Cr_Rpt_Nom_Liquidacion_Empleado.rpt", "Liquidacion" + Session.SessionID + ".pdf");
            //Volvemos a llamar a la función que inicializa el control de los eventos jquery.
            ScriptManager.RegisterStartupScript(UPnl_Generacion_Finiquitos, typeof(string), "Inicialzar Eventos JQuery", "inicializarEventos()", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte de liquidación del empleado. Error: [" + Ex.Message + "]");
        }
    }
    /// ***********************************************************************************************
    /// NOMBRE: Btn_Generar_Reporte_Renuncia_Click
    /// 
    /// DESCRIPCIÓN: Evento que ejecuta el reporte de Renuncia del empleado.
    /// 
    /// PARÁMETROS: No Aplica.
    /// 
    /// USUARIO CREÓ:Juan Alberto Hernández Negrete
    /// FECHA CREÓ: 21/Junio/2011 13:53 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// ***********************************************************************************************
    protected void Btn_Generar_Reporte_Renuncia_Click(Object sender, EventArgs e)
    {
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();
        DataTable Dt_Empleados = null;
        DataSet Ds_Empleado = null;

        try
        {
            Obj_Empleados.P_No_Empleado = Txt_No_Empleado.Text.Trim();
            Obj_Empleados.P_Estatus = "INACTIVO";
            Dt_Empleados = Obj_Empleados.Consultar_Informacion_Rpt_Finiquitos();

            Dt_Empleados.TableName = "Liquidacion_Empleado";

            Ds_Empleado = new DataSet();
            Ds_Empleado.Tables.Add(Dt_Empleados.Copy());

            Generar_Reporte(ref Ds_Empleado, "Cr_Rpt_Nom_Renuncia_Empleado.rpt", "Renuncia_" + Session.SessionID + ".pdf");
            //Volvemos a llamar a la función que inicializa el control de los eventos jquery.
            ScriptManager.RegisterStartupScript(UPnl_Generacion_Finiquitos, typeof(string), "Inicialzar Eventos JQuery", "inicializarEventos()", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte de renuncia. Error: [" + Ex.Message + "]");
        }
    }
    /// ***********************************************************************************************
    /// NOMBRE: Btn_Generar_Recibo_Renuncia_Click
    /// 
    /// DESCRIPCIÓN: Evento que ejecuta el reporte de Recibo de Renuncia.
    /// 
    /// PARÁMETROS: No Aplica.
    /// 
    /// USUARIO CREÓ:Juan Alberto Hernández Negrete
    /// FECHA CREÓ: 21/Junio/2011 13:53 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// ***********************************************************************************************
    protected void Btn_Generar_Recibo_Renuncia_Click(Object sender, EventArgs e)
    {
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();
        DataTable Dt_Empleados = null;


        DataTable Dt_Percepciones = null;
        DataTable Dt_Totales_Percepciones = null;
        DataTable Dt_Deducciones = null;
        DataTable Dt_Totales_Deducciones = null;
        DataTable Dt_Conceptos_Finiquito = null;
        DataSet Ds_Pre_Recibo_Finiquito = null;
        DataSet Ds_Percepciones = null;
        DataSet Ds_Deducciones = null;
        Double TOTAL_PERCEPCIONES = 0.0;
        Double TOTAL_DEDUCCIONES = 0.0;

        try
        {
            Obj_Empleados.P_No_Empleado = Txt_No_Empleado.Text.Trim();
            Obj_Empleados.P_Estatus = "INACTIVO";
            Dt_Empleados = Obj_Empleados.Consultar_Informacion_Rpt_Finiquitos();

            Ds_Pre_Recibo_Finiquito = new System.Data.DataSet();
            Dt_Percepciones = Crear_DataTable_Percepciones_Deducciones(Grid_Percepciones, "Txt_Cantidad_Percepcion", "Chk_Aplica_Deduccion_Finiquito");
            Dt_Deducciones = Crear_DataTable_Percepciones_Deducciones(Grid_Deducciones, "Txt_Cantidad_Deduccion", "Chk_Aplica_Percepcion_Finiquito");
            Dt_Conceptos_Finiquito = Crear_DataTable_Percepciones_Deducciones(Grid_Conceptos_Exclusivos_Finiquitos, "Txt_Cantidad_Deduccion_Percepcion", "Chk_Aplica_Percepcion_Finiquito");

            Actualizar_Cantidades_Percepciones_Deducciones(Dt_Conceptos_Finiquito, ref Dt_Percepciones);
            Actualizar_Cantidades_Percepciones_Deducciones(Dt_Conceptos_Finiquito, ref Dt_Deducciones);

            Ds_Percepciones = Crear_Lista_Percepciones_Con_Monto(Dt_Percepciones);
            Ds_Deducciones = Crear_Lista_Deducciones_Con_Monto(Dt_Deducciones);

            Dt_Percepciones = Ds_Percepciones.Tables[0];
            Dt_Totales_Percepciones = Ds_Percepciones.Tables[1];

            Dt_Deducciones = Ds_Deducciones.Tables[0];
            Dt_Totales_Deducciones = Ds_Deducciones.Tables[1];

            TOTAL_PERCEPCIONES = Obtener_Cantidad(Dt_Totales_Percepciones);
            TOTAL_DEDUCCIONES = Obtener_Cantidad(Dt_Totales_Deducciones);

            Agregar_Total_Letra(ref Dt_Totales_Percepciones, (TOTAL_PERCEPCIONES - TOTAL_DEDUCCIONES));
            Agregar_Total_Letra(ref Dt_Totales_Deducciones, (TOTAL_PERCEPCIONES - TOTAL_DEDUCCIONES));

            Dt_Percepciones.TableName = "Dt_Percepciones";
            Dt_Totales_Percepciones.TableName = "Total_Percepciones";
            Dt_Deducciones.TableName = "Dt_Deducciones";
            Dt_Totales_Deducciones.TableName = "Total_Deducciones";
            Dt_Empleados.TableName = "Liquidacion_Empleado";

            Ds_Pre_Recibo_Finiquito.Tables.Add(Dt_Percepciones.Copy());
            Ds_Pre_Recibo_Finiquito.Tables.Add(Dt_Totales_Percepciones.Copy());
            Ds_Pre_Recibo_Finiquito.Tables.Add(Dt_Deducciones.Copy());
            Ds_Pre_Recibo_Finiquito.Tables.Add(Dt_Totales_Deducciones.Copy());
            Ds_Pre_Recibo_Finiquito.Tables.Add(Dt_Empleados.Copy());

            Generar_Reporte(ref Ds_Pre_Recibo_Finiquito, "Cr_Rpt_Nom_Recibo_Renuncia.rpt", "Recibo_Renuncia_" + Session.SessionID + ".pdf");
            //Volvemos a llamar a la función que inicializa el control de los eventos jquery.
            ScriptManager.RegisterStartupScript(UPnl_Generacion_Finiquitos, typeof(string), "Inicialzar Eventos JQuery", "inicializarEventos()", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte de liquidación del empleado. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #endregion
}
