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
using Presidencia.Roles.Negocio;
using Presidencia.Dependencias.Negocios;
using Presidencia.Areas.Negocios;
using Presidencia.Empleados.Negocios;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Requisitos_Empleados.Negocios;
using AjaxControlToolkit;
using System.IO;
using System.Globalization;
using Presidencia.Programas.Negocios;
using Presidencia.Tipos_Contratos.Negocios;
using Presidencia.Puestos.Negocios;
using Presidencia.Escolaridad.Negocios;
using Presidencia.Sindicatos.Negocios;
using Presidencia.Turnos.Negocios;
using Presidencia.Zona_Economica.Negocios;
using Presidencia.Tipo_Trabajador.Negocios;
using System.Text.RegularExpressions;
using Presidencia.Vacaciones_Empleado.Negocio;
using Presidencia.Tipos_Nominas.Negocios;
using Presidencia.Utilidades_Nomina;
using Presidencia.Bancos_Nomina.Negocio;
using Presidencia.Catalogo_SAP_Fuente_Financiamiento.Negocio;
using Presidencia.Catalogo_Compras_Proyectos_Programas.Negocio;
using Presidencia.Catalogo_Compras_Partidas.Negocio;
using Presidencia.Percepciones_Deducciones_Fijas.Negocio;
using Presidencia.Prestamos.Negocio;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Cat_Nom_Percepciones_Deducciones_Opcional.Negocios;
using Presidencia.Ayudante_Calendario_Nomina;

public partial class paginas_Nomina_Frm_Ope_Nom_Captura_Fijos : System.Web.UI.Page
{
    #region (Load/Init)
    /// **********************************************************************************************
    /// NOMBRE: Page_Load
    ///
    /// DESCRIPCION: Carga la configuración inicial de la página.
    /// 
    /// PARÁMETROS: No aplica.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 2/Julio/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACION:
    /// **********************************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Inicializa_Controles();//Inicializamos la configuración inical de la página.
            }

            //Limpiamos y ocultamos los mensajes de error.
            Lbl_Mensaje_Error.Text = String.Empty; ;
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }
    #endregion

    #region (Métodos)

    #region (Métodos Generales)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 02/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Limpiar_Controles();
            Habilitar_Controles("Inicial");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 02/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado = false; ///Indica si el control de la forma va hacer habilitado para utilización del usuario

        try
        {
            switch (Operacion)
            {
                case "Inicial":
                    Habilitado = false;
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Modificar.Visible = true;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    break;

                case "Nuevo":
                    Habilitado = true;
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Modificar.Visible = false;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    break;

                case "Modificar":
                    Habilitado = true;
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    break;
            }

            Grid_Empleados.Enabled = !Habilitado;
            Grid_Tipo_Nomina_Deducciones.Enabled = Habilitado;
            Grid_Tipo_Nomina_Percepciones.Enabled = Habilitado;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    /// **********************************************************************************************
    /// NOMBRE: Limpiar_Controles
    ///
    /// DESCRIPCION: Limpia los controles de la página a un estado inicial..
    /// 
    /// PARÁMETROS: No Aplica.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 2/Julio/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACION:
    /// **********************************************************************************************
    private void Limpiar_Controles()
    {
        Grid_Empleados.SelectedIndex = (-1);

        Grid_Tipo_Nomina_Deducciones.DataSource = new DataTable();
        Grid_Tipo_Nomina_Deducciones.DataBind();

        Grid_Tipo_Nomina_Percepciones.DataSource = new DataTable();
        Grid_Tipo_Nomina_Percepciones.DataBind();
    }
    /// **********************************************************************************************
    /// NOMBRE: Juntar_Clave_Nombre_Concepto
    ///
    /// DESCRIPCION: Une la clave con el nombre del concepto.
    /// 
    /// PARÁMETROS: Dt_Concepto.- Tabla de los conceptos.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 2/Julio/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACION:
    /// **********************************************************************************************
    protected DataTable Juntar_Clave_Nombre_Concepto(DataTable Dt_Concepto)
    {
        try
        {
            if (Dt_Concepto is DataTable)
            {
                if (Dt_Concepto.Rows.Count > 0)
                {
                    foreach (DataRow CONCEPTO in Dt_Concepto.Rows)
                    {
                        if (CONCEPTO is DataRow)
                        {
                            CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Nombre] =
                                "[" + CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Clave].ToString().Trim() + "] -- " +
                                CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Nombre].ToString().Trim();
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al unir la clave del concepto con el nombre. Error: [" + Ex.Message + "]");
        }
        return Dt_Concepto;
    }
    /// NOMBRE: Obtener_Identificador_Empleado
    ///
    /// DESCRIPCION: Obtienen el identificador del empleado a partir del numero de 
    ///              empleado.
    /// 
    /// PARÁMETROS: Dt_Concepto.- Tabla de los conceptos.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 2/Julio/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACION:
    /// **********************************************************************************************
    protected String Obtener_Identificador_Empleado(String No_Empleado)
    {
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();
        DataTable Dt_Empleado = null;
        String Empleado_ID = String.Empty;

        try
        {
            Obj_Empleados.P_No_Empleado = No_Empleado;
            Dt_Empleado = Obj_Empleados.Consulta_Empleados_General();

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
            throw new Exception("Error al obtener el identificador del empleado. Error: [" + Ex.Message + "]");
        }
        return Empleado_ID;
    }
    /// ********************************************************************************************************************
    /// Nombre: Habilitar_Deshabilitar_Fila
    /// 
    /// Descripción: Habilita o deshabilita la fila según el estatus del concepto.
    /// 
    /// Parámetros: FILA.- Fila del grid que se evaluara y validara.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creo: 7/Septiembre/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ********************************************************************************************************************
    protected void Habilitar_Deshabilitar_Fila(ref GridViewRowEventArgs FILA)
    {
        Cls_Cat_Nom_Percepciones_Deducciones_Business Obj_CONCEPTOS = new Cls_Cat_Nom_Percepciones_Deducciones_Business();
        DataTable Dt_CONCEPTOS = null;
        String PERCEPCION_DEDUCCION_ID = String.Empty;

        try
        {
            if (FILA is GridViewRowEventArgs)
            {
                if (FILA.Row is GridViewRow)
                {
                    if (!String.IsNullOrEmpty(FILA.Row.Cells[0].ToString().Trim()))
                    {
                        PERCEPCION_DEDUCCION_ID = FILA.Row.Cells[0].Text.ToString().Trim();

                        Obj_CONCEPTOS.P_PERCEPCION_DEDUCCION_ID = PERCEPCION_DEDUCCION_ID;
                        Dt_CONCEPTOS = Obj_CONCEPTOS.Consultar_Percepciones_Deducciones_General();

                        if (Dt_CONCEPTOS is DataTable)
                        {
                            if (Dt_CONCEPTOS.Rows.Count > 0)
                            {
                                foreach (DataRow CONCEPTO in Dt_CONCEPTOS.Rows)
                                {
                                    if (CONCEPTO is DataRow)
                                    {
                                        if (!String.IsNullOrEmpty(CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Estatus].ToString().Trim()))
                                        {
                                            if (CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Estatus].ToString().Trim().ToUpper().Equals("INACTIVO"))
                                            {
                                                FILA.Row.Enabled = false;
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
            throw new Exception("Error al habilitar o deshabilitar alguna percepción o deduccion. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Consultas)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Empleados_Avanzada
    /// DESCRIPCION : Ejecuta la busqueda de empleados
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 13/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Empleados_Avanzada()
    {
        Cls_Cat_Empleados_Negocios Rs_Consulta_Ca_Empleados = new Cls_Cat_Empleados_Negocios(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Empleados; //Variable que obtendra los datos de la consulta 

        try
        {
            if (!string.IsNullOrEmpty(Txt_Busqueda_No_Empleado.Text.Trim())) 
                Rs_Consulta_Ca_Empleados.P_No_Empleado = Txt_Busqueda_No_Empleado.Text.Trim();

            if (!string.IsNullOrEmpty(Txt_Busqueda_Nombre_Empleado.Text.Trim())) 
                Rs_Consulta_Ca_Empleados.P_Nombre = Txt_Busqueda_Nombre_Empleado.Text.Trim();

            Rs_Consulta_Ca_Empleados.P_Estatus = "ACTIVO";

            Dt_Empleados = Rs_Consulta_Ca_Empleados.Consulta_Empleados_General(); //Consulta todos los Empleados que coindican con lo proporcionado por el usuario
            Session["Consulta_Empleados"] = Dt_Empleados;
            Llena_Grid_Empleados();
            Mpe_Busqueda_Empleados.Hide();
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Empleados " + ex.Message.ToString(), ex);
        }
    }
    #endregion

    #region (Modulo Cat_Nom_Empl_Perc_Dedu_Det [Control Tipos Nomina (Percepciones y/o Deducciones)])
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar_Grid_Tipo_Nomina_Percepciones_Deducciones
    /// DESCRIPCION : Cargar el grid con los datos pasados como parámetros.
    /// 
    /// PARAMETROS  : Dt_Datos.- Tabla con la informacion a cargar en el GridView
    ///               GridView_General .- GridView donde serán mostrados los datos.
    ///               
    /// CREO        : Juan Alberto Hernández Negrete
    /// FECHA_CREO  : 05/Enero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Grid_Tipo_Nomina_Percepciones_Deducciones(DataTable Dt_Datos, GridView GridView_General, String TextBox_ID)
    {
        GridView_General.Columns[0].Visible = true;
        GridView_General.DataSource = Dt_Datos;
        GridView_General.DataBind();
        GridView_General.SelectedIndex = -1;
        GridView_General.Columns[0].Visible = false;

        Cargar_Cantidad_Grid_Percepciones_Deducciones(GridView_General, Dt_Datos, TextBox_ID);
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Crear_DataTable_Percepciones_Deducciones
    /// DESCRIPCION : Crea una tabla con las percepciones y deducciones que aplican para 
    ///               el empleado.
    /// 
    /// PARAMETROS  : Grid_Tipo_Nomina_Percepciones_Deducciones.- Grid de donde se tomará
    ///               la información para generar la tabvle de percepciones deducciones
    ///               que le aplicaran al empleado.
    ///               
    /// CREO        : Juan Alberto Hernández Negrete
    /// FECHA_CREO  : 05/Enero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private DataTable Crear_DataTable_Percepciones_Deducciones(GridView Grid_Tipo_Nomina_Percepciones_Deducciones, String Txt_Cantidad_ID,
        String Txt_Importe_ID, String Txt_Saldo_ID, String Txt_Cantidad_Retenida_ID)
    {
        DataTable Dt_Percepciones_Deducciones_Aplican = null;//Variable que almacenara una lista de percepciones deducciones que aplican para el empleado.
        String Cantidad_Text = "";
        String Importe = String.Empty;
        String Saldo = String.Empty;
        String Cantidad_Retenida = String.Empty;

        try
        {
            Dt_Percepciones_Deducciones_Aplican = new DataTable();
            Dt_Percepciones_Deducciones_Aplican.Columns.Add(Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID, typeof(String));
            Dt_Percepciones_Deducciones_Aplican.Columns.Add(Cat_Nom_Percepcion_Deduccion.Campo_Nombre, typeof(String));
            Dt_Percepciones_Deducciones_Aplican.Columns.Add(Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad, typeof(String));
            Dt_Percepciones_Deducciones_Aplican.Columns.Add(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Importe, typeof(String));
            Dt_Percepciones_Deducciones_Aplican.Columns.Add(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Saldo, typeof(String));
            Dt_Percepciones_Deducciones_Aplican.Columns.Add(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad_Retenida, typeof(String));
            Dt_Percepciones_Deducciones_Aplican.Columns.Add(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Nomina_ID, typeof(String));
            Dt_Percepciones_Deducciones_Aplican.Columns.Add(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_No_Nomina, typeof(String));

            if (Grid_Tipo_Nomina_Percepciones_Deducciones is GridView)
            {
                for (int Contador_Fila = 0; Contador_Fila < Grid_Tipo_Nomina_Percepciones_Deducciones.Rows.Count; Contador_Fila++)
                {
                    DataRow PERCEPCION_DEDUCCION = Dt_Percepciones_Deducciones_Aplican.NewRow();
                    PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID] = Grid_Tipo_Nomina_Percepciones_Deducciones.Rows[Contador_Fila].Cells[0].Text;
                    PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Nombre] = Grid_Tipo_Nomina_Percepciones_Deducciones.Rows[Contador_Fila].Cells[1].Text;

                    Cantidad_Text = ((TextBox)Grid_Tipo_Nomina_Percepciones_Deducciones.Rows[Contador_Fila].Cells[4].FindControl(Txt_Cantidad_ID)).Text.Trim().Replace("$", "");
                    Importe = ((TextBox)Grid_Tipo_Nomina_Percepciones_Deducciones.Rows[Contador_Fila].Cells[5].FindControl(Txt_Importe_ID)).Text.Trim().Replace("$", "");
                    Saldo = ((TextBox)Grid_Tipo_Nomina_Percepciones_Deducciones.Rows[Contador_Fila].Cells[6].FindControl(Txt_Saldo_ID)).Text.Trim().Replace("$", "");
                    Cantidad_Retenida = ((TextBox)Grid_Tipo_Nomina_Percepciones_Deducciones.Rows[Contador_Fila].Cells[7].FindControl(Txt_Cantidad_Retenida_ID)).Text.Trim().Replace("$", "");

                    PERCEPCION_DEDUCCION[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad] = ((!String.IsNullOrEmpty(Cantidad_Text))) ? Convert.ToDouble(Cantidad_Text) : 0;
                    PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Importe] = ((!String.IsNullOrEmpty(Importe))) ? Convert.ToDouble(Importe) : 0;
                    PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Saldo] = ((!String.IsNullOrEmpty(Saldo))) ? Convert.ToDouble(Saldo) : 0;
                    PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad_Retenida] = ((!String.IsNullOrEmpty(Cantidad_Retenida))) ? Convert.ToDouble(Cantidad_Retenida) : 0;
                    PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Nomina_ID] = ((DropDownList)Grid_Tipo_Nomina_Percepciones_Deducciones.Rows[Contador_Fila].Cells[8].FindControl("Cmb_Calendario_Nomina")).SelectedValue.Trim();
                    PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_No_Nomina] = ((DropDownList)Grid_Tipo_Nomina_Percepciones_Deducciones.Rows[Contador_Fila].Cells[8].FindControl("Cmb_Periodos_Catorcenales_Nomina")).SelectedValue.Trim();

                    Dt_Percepciones_Deducciones_Aplican.Rows.Add(PERCEPCION_DEDUCCION);
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar al el el DataTable Percepciones Deducciones. Error: [" + Ex.Message + "]");
        }
        return Dt_Percepciones_Deducciones_Aplican;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Obtener_Percepciones_Deducciones_Tipo_Nomina
    /// DESCRIPCION : Obtiene las Percepciones y deducciones correspondientes al Tipo Nomina
    ///               seleccionado.
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernández Negrete
    /// FECHA_CREO  : 05/Enero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected DataTable Obtener_Percepciones_Deducciones_Tipo_Nomina(String Tipo_Nomina_ID, String Tipo)
    {
        Cls_Cat_Tipos_Nominas_Negocio Consulta_Percepciones_Deducciones = new Cls_Cat_Tipos_Nominas_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Tipo_Nomina_Percepciones_Deducciones = null;//variable que almacenara una lista de percepciones.
        try
        {
            Consulta_Percepciones_Deducciones.P_Tipo = Tipo;
            Consulta_Percepciones_Deducciones.P_Tipo_Nomina_ID = Tipo_Nomina_ID;
            Dt_Tipo_Nomina_Percepciones_Deducciones = Consulta_Percepciones_Deducciones.Consulta_Percepciones_Deducciones_Nomina();
            Dt_Tipo_Nomina_Percepciones_Deducciones = Juntar_Clave_Nombre_Concepto(Dt_Tipo_Nomina_Percepciones_Deducciones);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
        return Dt_Tipo_Nomina_Percepciones_Deducciones;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar_Cantidad_Grid_Percepciones_Deducciones
    /// DESCRIPCION : Carga la cantidad correspodiente a la percepcion o deduccion 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 11/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Cantidad_Grid_Percepciones_Deducciones(GridView Grid_Percepcion_Deduccion, DataTable Dt_Datos_Consultados, String TextBox_ID)
    {
        int index = 0;
        try
        {
            if (Dt_Datos_Consultados is DataTable)
            {
                if (Dt_Datos_Consultados.Rows.Count > 0)
                {
                    foreach (DataRow Renglon in Dt_Datos_Consultados.Rows)
                    {
                        if (Renglon is DataRow)
                        {
                            TextBox Txt_Cantidad = (TextBox)Grid_Percepcion_Deduccion.Rows[index].Cells[4].FindControl(TextBox_ID);
                            Txt_Cantidad.Text = String.Format("{0:#,###,##0.00}", Convert.ToDouble((string.IsNullOrEmpty(Renglon[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad].ToString()) || Renglon[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad].ToString().Equals("$  _,___,___.__")) ? "0" : Renglon[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad].ToString()));
                            index = index + 1;
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    ///****************************************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Grid_Percepciones_Deducciones
    /// DESCRIPCION : Checked o UnChecked la percepción o deduccion del grid de Tipo Nómina
    ///               Percepciones o Deducciones
    /// 
    /// PARAMETROS  : Dt_Datos.- Tabla con la informacion a cargar en el GridView
    ///               GridView_General .- GridView donde serán mostrados los datos.
    ///               
    /// CREO        : Juan Alberto Hernández Negrete
    /// FECHA_CREO  : 05/Enero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///****************************************************************************************************
    private void Aplicar_Cantidades_Grid_Percepciones_Deducciones(GridView Grid_Tipo_Nomina_Percepciones_Deducciones,
        DataTable Dt_Percepciones_Deducciones, String Txt_Cantidad_ID, String Txt_Importe_ID, String Txt_Saldo_ID, String Txt_Cantidad_Retenida)
    {
        try
        {
            if (Grid_Tipo_Nomina_Percepciones_Deducciones is GridView)
            {
                for (int Contador_Fila = 0; Contador_Fila < Grid_Tipo_Nomina_Percepciones_Deducciones.Rows.Count; Contador_Fila++)
                {
                    if (Dt_Percepciones_Deducciones is DataTable)
                    {
                        if (Dt_Percepciones_Deducciones.Rows.Count > 0)
                        {
                            foreach (DataRow PERCEPCION_DEDUCCION in Dt_Percepciones_Deducciones.Rows)
                            {
                                if (PERCEPCION_DEDUCCION is DataRow)
                                {
                                    if (PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString().Equals(Grid_Tipo_Nomina_Percepciones_Deducciones.Rows[Contador_Fila].Cells[0].Text))
                                    {
                                        if (!String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad].ToString().Trim()))
                                            ((TextBox)Grid_Tipo_Nomina_Percepciones_Deducciones.Rows[Contador_Fila].Cells[4].FindControl(Txt_Cantidad_ID)).Text =
                                                String.Format("{0:#,###,##0.00}", Convert.ToDouble(PERCEPCION_DEDUCCION[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad].ToString()));
                                        else
                                            ((TextBox)Grid_Tipo_Nomina_Percepciones_Deducciones.Rows[Contador_Fila].Cells[4].FindControl(Txt_Cantidad_ID)).Text = "0.00";

                                        if (!String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Importe].ToString().Trim()))
                                            ((TextBox)Grid_Tipo_Nomina_Percepciones_Deducciones.Rows[Contador_Fila].Cells[5].FindControl(Txt_Importe_ID)).Text =
                                                String.Format("{0:#,###,##0.00}", Convert.ToDouble(PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Importe].ToString()));
                                        else
                                            ((TextBox)Grid_Tipo_Nomina_Percepciones_Deducciones.Rows[Contador_Fila].Cells[5].FindControl(Txt_Importe_ID)).Text = "0.00";

                                        if (!String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Saldo].ToString().Trim()))
                                            ((TextBox)Grid_Tipo_Nomina_Percepciones_Deducciones.Rows[Contador_Fila].Cells[6].FindControl(Txt_Saldo_ID)).Text =
                                                String.Format("{0:#,###,##0.00}", Convert.ToDouble(PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Saldo].ToString()));
                                        else
                                            ((TextBox)Grid_Tipo_Nomina_Percepciones_Deducciones.Rows[Contador_Fila].Cells[6].FindControl(Txt_Saldo_ID)).Text = "0.00";

                                        if (!String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad_Retenida].ToString().Trim()))
                                            ((TextBox)Grid_Tipo_Nomina_Percepciones_Deducciones.Rows[Contador_Fila].Cells[7].FindControl(Txt_Cantidad_Retenida)).Text =
                                                String.Format("{0:#,###,##0.00}", Convert.ToDouble(PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad_Retenida].ToString()));
                                        else
                                            ((TextBox)Grid_Tipo_Nomina_Percepciones_Deducciones.Rows[Contador_Fila].Cells[7].FindControl(Txt_Cantidad_Retenida)).Text = "0.00";


                                        DropDownList Cmb_Calendario = ((DropDownList)Grid_Tipo_Nomina_Percepciones_Deducciones.Rows[Contador_Fila].Cells[8].FindControl("Cmb_Calendario_Nomina"));
                                        DropDownList Cmb_Periodos = ((DropDownList)Grid_Tipo_Nomina_Percepciones_Deducciones.Rows[Contador_Fila].Cells[8].FindControl("Cmb_Periodos_Catorcenales_Nomina"));

                                        Cmb_Calendario.SelectedIndex = Cmb_Calendario.Items.IndexOf(Cmb_Calendario.Items.FindByValue(PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Nomina_ID].ToString().Trim()));

                                        if (Cmb_Calendario.SelectedIndex > 0)
                                        {
                                            //Consultamos los periodos de pago que le pertencen a la nómina seleccionada.
                                            Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario.SelectedValue.Trim(), Cmb_Periodos, Cmb_Calendario);
                                            Cmb_Periodos.SelectedIndex = Cmb_Periodos.Items.IndexOf(Cmb_Periodos.Items.FindByValue(PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_No_Nomina].ToString().Trim()));
                                        }

                                        break;
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
            throw new Exception("Error al generar al el el DataTable Percepciones Deducciones. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Operacion [Alta - Modificar - Eliminar])
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Empleado
    /// DESCRIPCION : Modifica los datos del Empleado con los proporcionados por el usuario en la BD
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
    /// MODIFICO          :Juan Alberto Hernandez Negrete
    /// FECHA_MODIFICO    :3/Noviembre/2010
    /// CAUSA_MODIFICACION: Completar el Catalogo
    ///*******************************************************************************
    private void Modificar_Empleado()
    {
        Cls_Ope_Nom_Percepciones_Deducciones_Fijas_Negocio Obj_Percepciones_Deducciones_Fijas = new Cls_Ope_Nom_Percepciones_Deducciones_Fijas_Negocio();

        try
        {
            ///Percepciones Deducciones Tipo Nomina
            Obj_Percepciones_Deducciones_Fijas.P_Dt_Percepciones_Tipo_Nomina = Crear_DataTable_Percepciones_Deducciones(Grid_Tipo_Nomina_Percepciones, "Txt_Cantidad_Percepcion", "Txt_Cantidad_Importe", "Txt_Saldo", "Txt_Cantidad_Retenida");
            Obj_Percepciones_Deducciones_Fijas.P_Dt_Deducciones_Tipo_Nomina = Crear_DataTable_Percepciones_Deducciones(Grid_Tipo_Nomina_Deducciones, "Txt_Cantidad_Deduccion", "Txt_Cantidad_Importe", "Txt_Saldo", "Txt_Cantidad_Retenida");
            Obj_Percepciones_Deducciones_Fijas.P_Empleado_ID = Grid_Empleados.Rows[Grid_Empleados.SelectedIndex].Cells[1].Text.Trim();
            Obj_Percepciones_Deducciones_Fijas.P_Concepto = "TIPO_NOMINA";

            Obj_Percepciones_Deducciones_Fijas.Registro_Percepciones_Deducciones_Tipo_Nomina(); //Sustituye los datos que se encuentran en la BD por lo que introdujo el usuario
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Conceptos Fijos",
                "alert('Operación Completa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar_Empleado " + ex.Message.ToString(), ex);
        }
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
    private void Consultar_Calendarios_Nomina(DropDownList Cmb_Calendario_Nomina)
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
    private void Consultar_Periodos_Catorcenales_Nomina(String Nomina_ID, DropDownList Cmb_Periodos_Catorcenales_Nomina,
        DropDownList Cmb_Calendario_Nomina)
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

                    Validar_Periodos_Pago(Cmb_Periodos_Catorcenales_Nomina, Cmb_Calendario_Nomina);
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
    private void Validar_Periodos_Pago(DropDownList Combo, DropDownList Cmb_Calendario_Nomina)
    {
        Cls_Ope_Nom_Pestamos_Negocio Prestamos = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Detalles_Nomina = null;//Variable que almacenra los detalles del periodo seleccionado.
        DateTime Fecha_Actual = DateTime.Now;
        DateTime Fecha_Inicio = new DateTime();
        DateTime Fecha_Fin = new DateTime();

        Prestamos.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();

        foreach (ListItem Elemento in Combo.Items)
        {
            if (Es_Numerico(Elemento.Text.Trim()))
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
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Es_Numerico
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Es_Numerico(String Valor)
    {
        Boolean Estatus = true;

        try
        {
            Char[] Caracteres = Valor.ToCharArray();

            for (int Contador_Caracteres = 0; Contador_Caracteres < Caracteres.Length; Contador_Caracteres++)
            {
                if (!Char.IsDigit(Caracteres[Contador_Caracteres])) return false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al Validar si es un dato numerico. Error [" + Ex.Message + "]");
        }
        return Estatus;
    }
    #endregion

    #endregion

    #region (Grid)

    #region (Grid Empleados)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Empleados_SelectedIndexChanged
    /// DESCRIPCION : Consulta los datos del Empleado que selecciono el usuario
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 12-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Empleados_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Empleados_Negocios Rs_Consulta_Cat_Empleados = new Cls_Cat_Empleados_Negocios(); //Variable de conexión a la capa de Negocios para la consulta de los datos del empleado
        DataTable Dt_Empleados; //Variable que obtendra los datos de la consulta

        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Rs_Consulta_Cat_Empleados.P_Empleado_ID = Grid_Empleados.SelectedRow.Cells[1].Text;
            Dt_Empleados = Rs_Consulta_Cat_Empleados.Consulta_Datos_Empleado(); //Consulta los datos del empleado que fue seleccionado por el usuario

            if (Dt_Empleados is DataTable)
            {
                if (Dt_Empleados.Rows.Count > 0)
                {
                    //Agrega los valores de los campos a los controles correspondientes de la forma
                    foreach (DataRow Registro in Dt_Empleados.Rows)
                    {
                        ///Recursos Humanos
                        if (!string.IsNullOrEmpty(Registro[Cat_Empleados.Campo_Tipo_Nomina_ID].ToString()))
                        {
                            DataTable Dt_Tipo_Nomina_Percepciones = Obtener_Percepciones_Deducciones_Tipo_Nomina(Registro[Cat_Empleados.Campo_Tipo_Nomina_ID].ToString(), "PERCEPCION");
                            Cargar_Grid_Tipo_Nomina_Percepciones_Deducciones(Dt_Tipo_Nomina_Percepciones, Grid_Tipo_Nomina_Percepciones, "Txt_Cantidad_Percepcion");

                            DataTable Dt_Tipo_Nomina_Deducciones = Obtener_Percepciones_Deducciones_Tipo_Nomina(Registro[Cat_Empleados.Campo_Tipo_Nomina_ID].ToString(), "DEDUCCION");
                            Cargar_Grid_Tipo_Nomina_Percepciones_Deducciones(Dt_Tipo_Nomina_Deducciones, Grid_Tipo_Nomina_Deducciones, "Txt_Cantidad_Deduccion");

                            Rs_Consulta_Cat_Empleados.P_Empleado_ID = Grid_Empleados.SelectedRow.Cells[1].Text;
                            DataTable Dt_Percepciones_Deducciones_Tipo_Nomina = Rs_Consulta_Cat_Empleados.Consultar_Percepciones_Deducciones_Tipo_Nomina();

                            Aplicar_Cantidades_Grid_Percepciones_Deducciones(Grid_Tipo_Nomina_Percepciones, Dt_Percepciones_Deducciones_Tipo_Nomina, "Txt_Cantidad_Percepcion", "Txt_Cantidad_Importe", "Txt_Saldo", "Txt_Cantidad_Retenida");
                            Aplicar_Cantidades_Grid_Percepciones_Deducciones(Grid_Tipo_Nomina_Deducciones, Dt_Percepciones_Deducciones_Tipo_Nomina, "Txt_Cantidad_Deduccion", "Txt_Cantidad_Importe", "Txt_Saldo", "Txt_Cantidad_Retenida");
                        }
                    }
                }
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
    /// NOMBRE DE LA FUNCION: Grid_Empleados_PageIndexChanging
    /// DESCRIPCION : Cambia la pagina de la tabla de empleados
    ///               
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Empleados_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            //Limpia_Controles();                        //Limpia todos los controles de la forma
            Grid_Empleados.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
            Llena_Grid_Empleados();                    //Carga los Empleados que estan asignados a la página seleccionada

            Grid_Empleados.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llena_Grid_Empleados
    /// DESCRIPCION : Llena el grid con los Empleados que fueron obtenidos de la consulta
    ///               Consulta_Empleados
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llena_Grid_Empleados()
    {
        DataTable Dt_Empleados; //Variable que obtendra los datos de la consulta 
        try
        {
            Grid_Empleados.Columns[1].Visible = true;
            Grid_Empleados.DataBind();
            Dt_Empleados = (DataTable)Session["Consulta_Empleados"];
            Grid_Empleados.DataSource = Dt_Empleados;
            Grid_Empleados.DataBind();
            Grid_Empleados.Columns[1].Visible = false;
            Grid_Empleados.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_Empleados " + ex.Message.ToString(), ex);
        }
    }
    /// **************************************************************************************************************************************
    /// NOMBRE: Grid_Empleados_Sorting
    /// 
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// 
    /// CREÓ:   Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 18/Febrero/2011 19:04 pm.
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// **************************************************************************************************************************************
    protected void Grid_Empleados_Sorting(object sender, GridViewSortEventArgs e)
    {
        Consulta_Empleados_Avanzada();
        DataTable Dt_Calendario_Nominas = (Grid_Empleados.DataSource as DataTable);

        if (Dt_Calendario_Nominas != null)
        {
            DataView Dv_Calendario_Nominas = new DataView(Dt_Calendario_Nominas);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Calendario_Nominas.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Calendario_Nominas.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Empleados.DataSource = Dv_Calendario_Nominas;
            Grid_Empleados.DataBind();
        }
    }
    #endregion

    #region (Grid Percepciones)
    ///********************************************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Tipo_Nomina_Percepciones_RowDataBound
    /// DESCRIPCION : Agregamos algunas validaciones al GridView antes de que sea renderizado
    ///               al usuario.  
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 08/Enero/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///********************************************************************************************************
    protected void Grid_Tipo_Nomina_Percepciones_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                if (e.Row.Cells[2].Text.Trim().ToUpper().Equals("FIJA"))
                {
                    e.Row.Visible = true;
                    Habilitar_Deshabilitar_Fila(ref e);
                }
                else
                {
                    e.Row.Visible = false;
                }

                Consultar_Calendarios_Nomina(((DropDownList)e.Row.FindControl("Cmb_Calendario_Nomina")));
                ((DropDownList)e.Row.FindControl("Cmb_Calendario_Nomina")).ToolTip = e.Row.RowIndex.ToString();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    #endregion

    #region (Grid Deducciones)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Tipo_Nomina_Deduccion_RowDataBound
    /// DESCRIPCION : Agregamos algunas validaciones al GridView antes de que sea renderizado
    ///               al usuario. 
    ///               
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 08/Enero/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Tipo_Nomina_Deduccion_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                if (e.Row.Cells[2].Text.Trim().ToUpper().Equals("FIJA"))
                {
                    e.Row.Visible = true;
                    Habilitar_Deshabilitar_Fila(ref e);
                }
                else
                {
                    e.Row.Visible = false;
                }

                Consultar_Calendarios_Nomina(((DropDownList)e.Row.FindControl("Cmb_Calendario_Nomina")));
                ((DropDownList)e.Row.FindControl("Cmb_Calendario_Nomina")).ToolTip = e.Row.RowIndex.ToString();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    #endregion

    #endregion

    #region (Eventos)

    #region (Botones [Alta - Modificar - Consultar])
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Modificar_Click
    /// DESCRIPCION : Modificar al Empleado Seleccionado
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            if (Btn_Modificar.ToolTip.Equals("Modificar"))
            {
                if (Grid_Empleados.SelectedIndex != (-1))
                {
                    Habilitar_Controles("Modificar");
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el registro que desea modificar sus datos <br>";
                }
            }
            else
            {
                Modificar_Empleado();
                Habilitar_Controles("Inicial");
                Limpiar_Controles();
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
    /// NOMBRE DE LA FUNCION: Btn_Salir_Click
    /// DESCRIPCION : Salir o Cancelar la Operacion Actual
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, EventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip == "Salir")
            {
                Limpiar_Controles();
                Session.Remove("Consulta_Empleados");
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Inicializa_Controles();//Habilita los controles para la siguiente operación del usuario en el catálogo
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
    /// NOMBRE DE LA FUNCION: Btn_Busqueda_Empleados_Click
    /// DESCRIPCION : Ejecuta la busqueda de empleados
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 13/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Busqueda_Empleados_Click(object sender, EventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(Txt_Busqueda_No_Empleado.Text))
                Txt_Busqueda_No_Empleado.Text = String.Format("{0:000000}", Convert.ToInt64(Txt_Busqueda_No_Empleado.Text.Trim()));

            if (!String.IsNullOrEmpty(Txt_Busqueda_No_Empleado.Text) ||
                (!String.IsNullOrEmpty(Txt_Busqueda_Nombre_Empleado.Text) && (Txt_Busqueda_Nombre_Empleado.Text.Trim().Length >= 5)))
            {
                Mpe_Busqueda_Empleados.Hide();
                Consulta_Empleados_Avanzada();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    #endregion

    #region (ModalPopup)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Cerrar_Ventana_Click
    /// DESCRIPCION : Cierra la ventana de busqueda de empleados.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 13/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Cerrar_Ventana_Click(object sender, ImageClickEventArgs e)
    {
        Mpe_Busqueda_Empleados.Hide();
    }
    #endregion

    #region (Combos)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Calendario_Nomina_SelectedIndexChanged
    ///DESCRIPCIÓN: Consulta los periodos catorcenales de la nomina seleccionada.
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Calendario_Nomina_P_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList Cmb_Periodos = new DropDownList();
            DropDownList Cmb_Calendario_Nomina = (DropDownList)sender;
            Int32 Indice = Cmb_Calendario_Nomina.SelectedIndex;

            if (Indice > 0)
            {
                //Obtenemos el control que almacena los periodos de pago.
                Cmb_Periodos = (DropDownList)Grid_Tipo_Nomina_Percepciones.Rows[Convert.ToInt32(Cmb_Calendario_Nomina.ToolTip.Trim())].FindControl("Cmb_Periodos_Catorcenales_Nomina");
                //Consultamos los periodos de pago que le pertencen a la nómina seleccionada.
                Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario_Nomina.SelectedValue.Trim(), Cmb_Periodos, Cmb_Calendario_Nomina);
            }
            else
            {
                Cmb_Periodos.DataSource = new DataTable();
                Cmb_Periodos.DataBind();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
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
    protected void Cmb_Calendario_Nomina_D_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList Cmb_Periodos = new DropDownList();
            DropDownList Cmb_Calendario_Nomina = (DropDownList)sender;
            Int32 Indice = Cmb_Calendario_Nomina.SelectedIndex;

            if (Indice > 0)
            {
                //Obtenemos el control que almacena los periodos de pago.
                Cmb_Periodos = (DropDownList)Grid_Tipo_Nomina_Deducciones.Rows[Convert.ToInt32(Cmb_Calendario_Nomina.ToolTip.Trim())].FindControl("Cmb_Periodos_Catorcenales_Nomina");
                //Consultamos los periodos de pago que le pertencen a la nómina seleccionada.
                Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario_Nomina.SelectedValue.Trim(), Cmb_Periodos, Cmb_Calendario_Nomina);
            }
            else
            {
                Cmb_Periodos.DataSource = new DataTable();
                Cmb_Periodos.DataBind();
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

    #region (TextBox)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_Busqueda_No_Empleado_TextChanged
    ///
    ///DESCRIPCIÓN: Consultar al empleado.
    ///
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: 15/02/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_Busqueda_No_Empleado_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(Txt_Busqueda_No_Empleado.Text))
                Txt_Busqueda_No_Empleado.Text = String.Format("{0:000000}", Convert.ToInt64(Txt_Busqueda_No_Empleado.Text.Trim()));

            if (!String.IsNullOrEmpty(Txt_Busqueda_No_Empleado.Text) || 
                (!String.IsNullOrEmpty(Txt_Busqueda_Nombre_Empleado.Text) && (Txt_Busqueda_Nombre_Empleado.Text.Trim().Length >= 5)))
            {
                Mpe_Busqueda_Empleados.Hide();
                Consulta_Empleados_Avanzada();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al buscar al empleado. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #endregion
}
