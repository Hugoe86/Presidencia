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
using Presidencia.Sindicatos.Negocios;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Cat_Nom_Percepciones_Deducciones_Opcional.Negocios;
using Presidencia.Antiguedad_Sindicato.Negocio;
using System.Collections.Generic;

public partial class paginas_Nomina_Frm_Cat_Nom_Sindicatos : System.Web.UI.Page
{
    #region (Page Load)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Page_Load
    /// DESCRIPCION : Carga inicial de la pagina, habilita la configuracion inicial
    ///               de los controles de la pagina.
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 30-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                ViewState["SortDirection"] = "ASC";
            }     
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    #endregion

    #region (Metodos)

    #region(Metodos Generales)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 30-Agosto-2010
    /// MODIFICO          : Juan Alberto Hernándaz Negrete
    /// FECHA_MODIFICO    : 10-Ene-2010
    /// CAUSA_MODIFICACION: Inicializar Ctrl Percepciones y/o Deducciones y Antiguedades.
    ///*******************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Habilitar_Controles("Inicial"); //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar
            Limpia_Controles();             //Limpia los controles del forma
            Consulta_Sindicatos();          //Consulta todas los Sindicatos que fueron dadas de alta en la BD
            Consultar_Percepciones();       //Carga las percepciones que se encuentran actualmente en el sistema
            Consultar_Deducciones();        //Carga las deducciones que se encuentran actualmente en el sistema
            Cosultar_Antiguedad_Sindical(); //Consulta las antiguedades sindicales que se encuentran actualmente en el sistema.
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION : Limpia los controles que se encuentran en la forma
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 30-Agosto-2010
    /// MODIFICO          : Juan Alberto Hernándaz Negrete
    /// FECHA_MODIFICO    : 10-Ene-2010
    /// CAUSA_MODIFICACION: Limpiar Ctrl Percepciones y/o Deducciones y Antiguedades.
    ///*******************************************************************************
    private void Limpia_Controles()
    {
        try
        {
            Txt_Sindicato_ID.Text = "";
            Txt_Nombre_Sindicato.Text = "";
            Txt_Responsable_Sindicato.Text = "";
            Txt_Comentarios_Sindicato.Text = "";
            Txt_Busqueda_Sindicato.Text = "";
            Cmb_Estatus_Sindicato.SelectedIndex = -1;
            Grid_Sindicato.SelectedIndex = -1;

            Grid_Percepciones.DataSource = new DataTable();
            Grid_Deducciones.DataSource = new DataTable();
            Grid_Antiguedad_Sindicato.DataSource = new DataTable();
            Grid_Percepciones.DataBind();
            Grid_Deducciones.DataBind();
            Grid_Antiguedad_Sindicato.DataBind();

            if (Session["Dt_Percepciones_Grid"] != null)
            {
                Session.Remove("Dt_Percepciones_Grid");
            }
            if (Session["Dt_Deducciones_Grid"] != null)
            {
                Session.Remove("Dt_Deducciones_Grid");
            }
            if (Session["Dt_Antiguedad_Sindicato_Grid"] != null)
            {
                Session.Remove("Dt_Antiguedad_Sindicato_Grid");
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Limpiar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Juntar_Clave_Nombre
    /// DESCRIPCION : Junta el nombre del concepto con la clave.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected DataTable Juntar_Clave_Nombre(DataTable Dt_Conceptos)
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
            throw new Exception("Error al unir el nombre con la clave del concepto. Error: [" + Ex.Message + "]");
        }
        return Dt_Conceptos;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///               para a siguiente operación
    /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
    ///                          si es una alta, modificacion
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 19/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado; ///Indica si el control de la forma va hacer habilitado para utilización del usuario

        try
        {
            Habilitado = false;
            switch (Operacion)
            {
                case "Inicial":
                    Habilitado = false;
                    Cmb_Estatus_Sindicato.SelectedIndex = 0;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Salir";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = true;                    
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;                    
                    Cmb_Estatus_Sindicato.Enabled = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                    Configuracion_Acceso("Frm_Cat_Nom_Sindicatos.aspx");
                    break;

                case "Nuevo":
                    Habilitado = true;
                    Cmb_Estatus_Sindicato.SelectedIndex = 0;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Eliminar.Visible = false;                    
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;                    
                    Cmb_Estatus_Sindicato.Enabled = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    break;

                case "Modificar":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = false;                    
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Cmb_Estatus_Sindicato.Enabled = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    break;
            }
            Txt_Nombre_Sindicato.Enabled = Habilitado;
            Txt_Responsable_Sindicato.Enabled = Habilitado;
            Txt_Comentarios_Sindicato.Enabled = Habilitado;
            Txt_Busqueda_Sindicato.Enabled = !Habilitado;
            Btn_Busqueda_Sindicato.Enabled = !Habilitado;            
            Grid_Sindicato.Enabled = !Habilitado;

            Grid_Percepciones.Enabled = Habilitado;
            Grid_Deducciones.Enabled = Habilitado;
            Grid_Antiguedad_Sindicato.Enabled = Habilitado;
            Cmb_Percepciones.Enabled = Habilitado;
            Cmb_Deducciones.Enabled = Habilitado;
            Cmb_Antiguedad_Sindicato.Enabled = Habilitado;
            Btn_Agregar_Percepciones.Enabled = Habilitado;
            Btn_Agregar_Deducciones.Enabled = Habilitado;
            Btn_Agregar_Antiguedad_Sindicato.Enabled = Habilitado;

            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            TC_Sindicatos_Perc_Dedu.ActiveTabIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    #endregion

    #region (Metodos Manejo Cantidades Grids)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Crear_DataTable_Percepciones_Deducciones
    /// DESCRIPCION : Crea un datatable con la informacion de del id de la percepcion
    ///               y la cantidad asignada para la percepcion.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 11/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private DataTable Crear_DataTable_Percepciones_Deducciones(GridView _GridView, String TextBox_ID)
    {
        DataTable Dt_Percepciones_Deducciones = new DataTable();
        Dt_Percepciones_Deducciones.Columns.Add(Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID, typeof(System.String));
        Dt_Percepciones_Deducciones.Columns.Add(Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Cantidad, typeof(System.String));
        DataRow Renglon;

        try
        {
            _GridView.Columns[0].Visible = true;
            for (int index = 0; index < _GridView.Rows.Count; index++)
            {
                Renglon = Dt_Percepciones_Deducciones.NewRow();
                Renglon[Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID] = _GridView.Rows[index].Cells[0].Text;
                Renglon[Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Cantidad] = ((TextBox)_GridView.Rows[index].Cells[3].FindControl(TextBox_ID)).Text;
                Dt_Percepciones_Deducciones.Rows.Add(Renglon);
            }
            _GridView.Columns[0].Visible = false;
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
        return Dt_Percepciones_Deducciones;
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
            foreach (DataRow Renglon in Dt_Datos_Consultados.Rows)
            {
                TextBox Txt_Cantidad = (TextBox)Grid_Percepcion_Deduccion.Rows[index].Cells[3].FindControl(TextBox_ID);
                Txt_Cantidad.Text = String.Format("{0:#0.00}", Convert.ToDouble((string.IsNullOrEmpty(Renglon[Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Cantidad].ToString()) ? "0" : Renglon[Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Cantidad].ToString())));
                index = index + 1;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Crear_DataTable_Antiguedad_Sindicato
    /// DESCRIPCION : Crea un datatable con la informacion de del id de la antiguedad sindical
    ///               y la cantidad asignada para la antiguedad sindical.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 27/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private DataTable Crear_DataTable_Antiguedad_Sindicato(GridView Grid_Ant_Sin_Det, String TextBox_ID)
    {
        DataTable Dt_Antiguedad_Sindicato = new DataTable();
        Dt_Antiguedad_Sindicato.Columns.Add(Cat_Nom_Ant_Sin_Det.Campo_Antiguedad_Sindicato_ID, typeof(System.String));
        Dt_Antiguedad_Sindicato.Columns.Add(Cat_Nom_Ant_Sin_Det.Campo_Monto, typeof(System.String));
        DataRow Renglon;

        try
        {
            for (int Fila_Consultar = 0; Fila_Consultar < Grid_Ant_Sin_Det.Rows.Count; Fila_Consultar++)
            {
                Renglon = Dt_Antiguedad_Sindicato.NewRow();
                Renglon[Cat_Nom_Ant_Sin_Det.Campo_Antiguedad_Sindicato_ID] = Grid_Ant_Sin_Det.Rows[Fila_Consultar].Cells[0].Text;
                Renglon[Cat_Nom_Ant_Sin_Det.Campo_Monto] = ((TextBox)Grid_Ant_Sin_Det.Rows[Fila_Consultar].Cells[2].FindControl(TextBox_ID)).Text;
                Dt_Antiguedad_Sindicato.Rows.Add(Renglon);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
        return Dt_Antiguedad_Sindicato;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar_Cantidad_Grid_Antiguedad_Sindicato
    /// DESCRIPCION : Carga el Monto correspodiente a la antiguedad sindical.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 27/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Cantidad_Grid_Antiguedad_Sindicato(GridView Grid_Ant_Sin_Det, DataTable Dt_Datos_Consultados, String TextBox_ID)
    {
        int Fila_Consultar = 0;
        try
        {
            foreach (DataRow Renglon in Dt_Datos_Consultados.Rows)
            {
                TextBox Txt_Cantidad = (TextBox)Grid_Ant_Sin_Det.Rows[Fila_Consultar].Cells[2].FindControl(TextBox_ID);
                Txt_Cantidad.Text = String.Format("{0:#0.00}", Convert.ToDouble((string.IsNullOrEmpty(Renglon[Cat_Nom_Ant_Sin_Det.Campo_Monto].ToString()) ? "0" : Renglon[Cat_Nom_Ant_Sin_Det.Campo_Monto].ToString())));
                Fila_Consultar = Fila_Consultar + 1;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    #endregion

    #region (Metodos Consulta)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar Combo de Percepciones Deducciones
    /// DESCRIPCION : Carga las Percepciones Deducciones Fijas o Variables que no son calculadas
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Percepciones()
    {
        DataTable Dt_Percepciones = null;
        Cls_Cat_Nom_Sindicatos_Negocio Rs_Consulta_Cat_Nom_Percepcion_Deduccion = new Cls_Cat_Nom_Sindicatos_Negocio();
        try
        {
            Rs_Consulta_Cat_Nom_Percepcion_Deduccion.P_Tipo_Percepcion = "PERCEPCION";
            Dt_Percepciones = Rs_Consulta_Cat_Nom_Percepcion_Deduccion.Consultar_Percepciones_Deducciones_Generales();
            Session["Dt_Percepciones_Combo"] = Dt_Percepciones;
            Cmb_Percepciones.DataSource = Dt_Percepciones;
            Cmb_Percepciones.DataTextField = "NOMBRE_CONCEPTO";
            Cmb_Percepciones.DataValueField = Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID;
            Cmb_Percepciones.DataBind();
            Cmb_Percepciones.Items.Insert(0, new ListItem("< Seleccione >", ""));
            Cmb_Percepciones.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error generado al consultar las Percepciones Deducciones. Error: [" + Ex.Message + "]");
        }

    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar Combo de Percepciones Deducciones
    /// DESCRIPCION : Carga las Percepciones Deducciones Fijas o Variables que no son calculadas
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Deducciones()
    {
        DataTable Dt_Deducciones = null;
        Cls_Cat_Nom_Sindicatos_Negocio Cat_Deducciones = new Cls_Cat_Nom_Sindicatos_Negocio();
        try
        {
            Cat_Deducciones.P_Tipo_Percepcion = "DEDUCCION";
            Dt_Deducciones = Cat_Deducciones.Consultar_Percepciones_Deducciones_Generales();
            Session["Dt_Deducciones_Combo"] = Dt_Deducciones;
            Cmb_Deducciones.DataSource = Dt_Deducciones;
            Cmb_Deducciones.DataTextField = "NOMBRE_CONCEPTO";
            Cmb_Deducciones.DataValueField = Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID;
            Cmb_Deducciones.DataBind();
            Cmb_Deducciones.Items.Insert(0, new ListItem("< Seleccione >", ""));
            Cmb_Deducciones.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error generado al consultar las Deducciones. Error: [" + Ex.Message + "]");
        }

    }
    ///***************************************************************************************************************
    /// NOMBRE DE LA FUNCION: Cosultar_Antiguedad_Sindical
    /// DESCRIPCION : Consulta las antiguedades sindicales que se encuentran registradas
    ///               en el sistema.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 27/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///****************************************************************************************************************
    private void Cosultar_Antiguedad_Sindical() {
        Cls_Cat_Nom_Antiguedad_Sindicato_Negocio Cls_Antigudad_Sindicatos = new Cls_Cat_Nom_Antiguedad_Sindicato_Negocio();//Variable de conexion con la capa de negocio.
        DataTable Dt_Antiguedad_Sindicatos = null;//Variable que almacenara una lista de antiguedades de sindicatos.

        try
        {
            Dt_Antiguedad_Sindicatos = Cls_Antigudad_Sindicatos.Consultar_Antiguedad_Sindicato();//Consltamos las antiguedades existentes en el sistema.
            Session["Dt_Antiguedad_Sindicato_Combo"] = Dt_Antiguedad_Sindicatos;//Generamos la variable de sesion que me ayudara a agregar o eliminar columnas del grid.
            Cmb_Antiguedad_Sindicato.DataSource = Dt_Antiguedad_Sindicatos;
            Cmb_Antiguedad_Sindicato.DataTextField = Cat_Nom_Antiguedad_Sindicato.Campo_Anios;
            Cmb_Antiguedad_Sindicato.DataValueField = Cat_Nom_Antiguedad_Sindicato.Campo_Antiguedad_Sindicato_ID;
            Cmb_Antiguedad_Sindicato.DataBind();
            Cmb_Antiguedad_Sindicato.Items.Insert(0, new ListItem("< Seleccione >", ""));
            Cmb_Antiguedad_Sindicato.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las antiguedades sindicales registradas actualmente en el sistema. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Metodos Validacion)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Campo_Cantidad_In_Gridview
    /// DESCRIPCION : Valida que no exista percepciones y/o deducciones con 
    /// cantidad de cero
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 11/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Campo_Cantidad_In_Gridview(GridView _GridView, String TextBox_ID)
    {
        Boolean Empty_Value_Quantity = true;

        for (int index = 0; index < _GridView.Rows.Count; index++)
        {
            String Cantidad_Text = ((TextBox)_GridView.Rows[index].Cells[3].FindControl(TextBox_ID)).Text;
            String Tipo_Asignacion =  _GridView.Rows[index].Cells[2].Text.ToUpper();

            if (string.IsNullOrEmpty(Cantidad_Text) || Cantidad_Text.Equals("$  _,___,___.__"))
            {
                Empty_Value_Quantity = false;
            }
            else if ((Convert.ToDouble(Cantidad_Text) < 0) && (Tipo_Asignacion.Equals("FIJA")))
            {
                Empty_Value_Quantity = false;
            }
        }
        return Empty_Value_Quantity;
    }
    #endregion

    #region (Metodos de Operacion [Alta - Modificar - Eliminar])
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta_Sindicato
    /// DESCRIPCION : Da de Alta el Sindicato con los datos proporcionados por el usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 30-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Alta_Sindicato()
    {
        Cls_Cat_Nom_Sindicatos_Negocio Rs_Alta_Cat_Nom_Sindicatos = new Cls_Cat_Nom_Sindicatos_Negocio(); //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta
        try
        {
            Rs_Alta_Cat_Nom_Sindicatos.P_Nombre = Txt_Nombre_Sindicato.Text;
            Rs_Alta_Cat_Nom_Sindicatos.P_Responsable= Txt_Responsable_Sindicato.Text;
            Rs_Alta_Cat_Nom_Sindicatos.P_Estatus = Cmb_Estatus_Sindicato.SelectedValue;                                
            Rs_Alta_Cat_Nom_Sindicatos.P_Comentarios = Txt_Comentarios_Sindicato.Text;
            Rs_Alta_Cat_Nom_Sindicatos.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
            Rs_Alta_Cat_Nom_Sindicatos.P_Dt_Percepciones = Crear_DataTable_Percepciones_Deducciones(Grid_Percepciones, "Txt_Cantidad_Percepcion");
            Rs_Alta_Cat_Nom_Sindicatos.P_Dt_Deducciones = Crear_DataTable_Percepciones_Deducciones(Grid_Deducciones, "Txt_Cantidad_Deduccion");
            Rs_Alta_Cat_Nom_Sindicatos.P_Dt_Antiguedad_Sindicatos = Crear_DataTable_Antiguedad_Sindicato(Grid_Antiguedad_Sindicato, "Txt_Cantidad_Antiguedad_Sindical");

            Rs_Alta_Cat_Nom_Sindicatos.Alta_Sindicato(); //Da de alta los datos de el Sindicato proporcionados por el usuario en la BD
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Sindicatos", "alert('El Alta del Sindicato fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Alta_Sindicatos " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Sindicato
    /// DESCRIPCION : Modifica los datos del Sindicato con los proporcionados por el usuario en la BD
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 30-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Modificar_Sindicato()
    {
        Cls_Cat_Nom_Sindicatos_Negocio Rs_Modificar_Cat_Nom_Sindicatos = new Cls_Cat_Nom_Sindicatos_Negocio(); //Variable de conexión hacia la capa de Negocios para envio de datos a modificar
        try
        {
            Rs_Modificar_Cat_Nom_Sindicatos.P_Sindicato_ID = Txt_Sindicato_ID.Text;
            Rs_Modificar_Cat_Nom_Sindicatos.P_Nombre = Txt_Nombre_Sindicato.Text;
            Rs_Modificar_Cat_Nom_Sindicatos.P_Responsable = Txt_Responsable_Sindicato.Text;
            Rs_Modificar_Cat_Nom_Sindicatos.P_Estatus = Cmb_Estatus_Sindicato.SelectedValue;                
            Rs_Modificar_Cat_Nom_Sindicatos.P_Comentarios = Txt_Comentarios_Sindicato.Text;
            Rs_Modificar_Cat_Nom_Sindicatos.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;

            Rs_Modificar_Cat_Nom_Sindicatos.P_Dt_Percepciones = Crear_DataTable_Percepciones_Deducciones(Grid_Percepciones, "Txt_Cantidad_Percepcion");
            Rs_Modificar_Cat_Nom_Sindicatos.P_Dt_Deducciones = Crear_DataTable_Percepciones_Deducciones(Grid_Deducciones, "Txt_Cantidad_Deduccion");
            Rs_Modificar_Cat_Nom_Sindicatos.P_Dt_Antiguedad_Sindicatos = Crear_DataTable_Antiguedad_Sindicato(Grid_Antiguedad_Sindicato, "Txt_Cantidad_Antiguedad_Sindical");

            Rs_Modificar_Cat_Nom_Sindicatos.Modificar_Sindicato(); //Sustituye los datos que se encuentran en la BD por lo que introdujo el usuario
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Sindicatos", "alert('La Modificación del Sindicatos fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar_Sindicato " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Eliminar_Sindicato
    /// DESCRIPCION : Elimina los datos del Sindicato que fue seleccionado por el Usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 30-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Eliminar_Sindicato()
    {
        Cls_Cat_Nom_Sindicatos_Negocio Rs_Eliminar_Cat_Nom_Sindicatos = new Cls_Cat_Nom_Sindicatos_Negocio(); //Variable de conexión hacia la capa de Negocios para la eliminación de los datos
        try
        {
            Rs_Eliminar_Cat_Nom_Sindicatos.P_Sindicato_ID = Txt_Sindicato_ID.Text;
            Rs_Eliminar_Cat_Nom_Sindicatos.Eliminar_Sindicato(); //Elimina el Sindicato que selecciono el usuario de la BD
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Sindicatos", "alert('La Eliminación del Sindicato fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Eliminar_Sindicato " + ex.Message.ToString(), ex);
        }
    }
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
            Botones.Add(Btn_Eliminar);
            Botones.Add(Btn_Busqueda_Sindicato);

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

    #endregion

    #region (Grid)

    #region (GridView Sindicatos)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Sindicato_SelectedIndexChanged
    /// DESCRIPCION : Consulta los datos del Sindicato que selecciono el usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 30-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Sindicato_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Nom_Sindicatos_Negocio Rs_Consulta_Cat_Nom_Sindicatos = new Cls_Cat_Nom_Sindicatos_Negocio(); //Variable de conexión a la capa de Negocios para la consulta de los datos
        DataTable Dt_Sindicatos; //Variable que obtendra los datos de la consulta

        try
        {
            Consultar_Percepciones_Sindicato();
            Consultar_Deducciones_Sindicato();
            Consultar_Antiguedades_Sindicales();

            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Rs_Consulta_Cat_Nom_Sindicatos.P_Sindicato_ID = Grid_Sindicato.SelectedRow.Cells[1].Text;
            Dt_Sindicatos = Rs_Consulta_Cat_Nom_Sindicatos.Consulta_Datos_Sindicato(); //Consulta los datos del Sindicato que fue seleccionado por el usuario
            if (Dt_Sindicatos.Rows.Count > 0)
            {
                //Agrega los valores de los campos a los controles correspondientes de la forma
                foreach (DataRow Registro in Dt_Sindicatos.Rows)
                {
                    Txt_Sindicato_ID.Text = Registro[Cat_Nom_Sindicatos.Campo_Sindicato_ID].ToString();
                    Txt_Nombre_Sindicato.Text = Registro[Cat_Nom_Sindicatos.Campo_Nombre].ToString();
                    Txt_Responsable_Sindicato.Text = Registro[Cat_Nom_Sindicatos.Campo_Responsable].ToString();
                    Txt_Comentarios_Sindicato.Text = Registro[Cat_Nom_Sindicatos.Campo_Comentarios].ToString();
                    Cmb_Estatus_Sindicato.SelectedValue = Registro[Cat_Nom_Sindicatos.Campo_Estatus].ToString();
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
    /// NOMBRE DE LA FUNCION: Grid_Sindicato_PageIndexChanging
    /// DESCRIPCION : Cambia la pagina del Grid de Sindicatos
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 30-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Sindicato_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Limpia_Controles();                        //Limpia todos los controles de la forma
            Grid_Sindicato.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
            Llena_Grid_Sindicatos();                   //Carga los Sindicatos que estan asignadas a la página seleccionada
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Sindicatos
    /// DESCRIPCION : Consulta los Sindicatos que estan dados de alta en la BD
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 30-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Sindicatos()
    {
        Cls_Cat_Nom_Sindicatos_Negocio Rs_Consulta_Cat_Nom_Sindicatos = new Cls_Cat_Nom_Sindicatos_Negocio(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Sindicatos; //Variable que obtendra los datos de la consulta 

        try
        {
            if (Txt_Busqueda_Sindicato.Text != "")
            {
                Rs_Consulta_Cat_Nom_Sindicatos.P_Nombre = Txt_Busqueda_Sindicato.Text;
            }
            Dt_Sindicatos = Rs_Consulta_Cat_Nom_Sindicatos.Consulta_Datos_Sindicato(); //Consulta todos los Sindicatos con sus datos generales
            Session["Consulta_Sindicatos"] = Dt_Sindicatos;
            Llena_Grid_Sindicatos();
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Sindicatos " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llena_Grid_Sindicatos
    /// DESCRIPCION : Llena el grid con los Sindicatos que se encuentran en la BD
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 30-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llena_Grid_Sindicatos()
    {
        DataTable Dt_Sindicatos; //Variable que obtendra los datos de la consulta 
        try
        {
            Grid_Sindicato.DataBind();
            Dt_Sindicatos = (DataTable)Session["Consulta_Sindicatos"];
            Grid_Sindicato.DataSource = Dt_Sindicatos;
            Grid_Sindicato.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_Sindicatos " + ex.Message.ToString(), ex);
        }
    }
    /// **************************************************************************************************************************************
    /// NOMBRE: Grid_Sindicato_Sorting
    /// 
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// 
    /// CREÓ:   Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 18/Febrero/2011 19:04 pm.
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// **************************************************************************************************************************************
    protected void Grid_Sindicato_Sorting(object sender, GridViewSortEventArgs e)
    {
        Consulta_Sindicatos();
        DataTable Dt_Sindicatos = (Grid_Sindicato.DataSource as DataTable);

        if (Dt_Sindicatos != null)
        {
            DataView Dv_Calendario_Nominas = new DataView(Dt_Sindicatos);
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

            Grid_Sindicato.DataSource = Dv_Calendario_Nominas;
            Grid_Sindicato.DataBind();
        }
    }
    #endregion

    #region (GridView Percepciones)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Percepciones_RowDataBound
    /// DESCRIPCION : Agrega un identificador al boton de cancelar de la tabla
    /// para identicar la fila seleccionada de tabla.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Percepciones_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                ((ImageButton)e.Row.Cells[4].FindControl("Btn_Delete_Percepcion")).CommandArgument = e.Row.Cells[0].Text.Trim();
                ((ImageButton)e.Row.Cells[4].FindControl("Btn_Delete_Percepcion")).ToolTip = "Quitar la Percepcion " + e.Row.Cells[1].Text + " al Sindicato";
                ((TextBox)e.Row.Cells[3].FindControl("Txt_Cantidad_Percepcion")).ToolTip = "" + e.Row.RowIndex;

                if (e.Row.Cells[2].Text.Equals("OPERACION") ||
                    e.Row.Cells[2].Text.Equals("VARIABLE"))
                {
                    e.Row.Cells[3].Enabled = false;
                    ((TextBox)e.Row.Cells[3].FindControl("Txt_Cantidad_Percepcion")).Style.Add("display", "none");
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Percepciones_Sindicato
    /// DESCRIPCION : Consultar las Percepciones del Sindicato Seleccionado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Percepciones_Sindicato()
    {
        Cls_Cat_Nom_Sindicatos_Negocio Rs_Consulta_Cat_Nom_Sindicatos = new Cls_Cat_Nom_Sindicatos_Negocio();
        Int32 index = Grid_Sindicato.SelectedIndex;
        DataTable Dt_Percepciones_Sindicato;

        if (index != -1)
        {
            Rs_Consulta_Cat_Nom_Sindicatos.P_Sindicato_ID = Grid_Sindicato.Rows[index].Cells[1].Text;
            Rs_Consulta_Cat_Nom_Sindicatos.P_Tipo_Percepcion = "PERCEPCION";
            Dt_Percepciones_Sindicato = Rs_Consulta_Cat_Nom_Sindicatos.Consultar_Percepciones_Deducciones();
            Dt_Percepciones_Sindicato = Juntar_Clave_Nombre(Dt_Percepciones_Sindicato);
            Session["Dt_Percepciones_Grid"] = Dt_Percepciones_Sindicato;

            Grid_Percepciones.Columns[0].Visible = true;
            Grid_Percepciones.DataSource = (DataTable)Session["Dt_Percepciones_Grid"];
            Grid_Percepciones.DataBind();
            Grid_Percepciones.Columns[0].Visible = false;

            Cargar_Cantidad_Grid_Percepciones_Deducciones(Grid_Percepciones, (DataTable)Session["Dt_Percepciones_Grid"], "Txt_Cantidad_Percepcion");
        }
    }
    #endregion

    #region (GridView Deducciones)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Deducciones_RowDataBound
    /// DESCRIPCION : Agrega un identificador al boton de cancelar de la tabla
    /// para identicar la fila seleccionada de tabla.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 11/Noviembre/2010
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
                ((ImageButton)e.Row.Cells[4].FindControl("Btn_Delete_Deduccion")).CommandArgument = e.Row.Cells[0].Text.Trim();
                ((ImageButton)e.Row.Cells[4].FindControl("Btn_Delete_Deduccion")).ToolTip = "Quitar la Deduccion " + e.Row.Cells[1].Text + " al Sindicato";
                ((TextBox)e.Row.Cells[3].FindControl("Txt_Cantidad_Deduccion")).ToolTip = "" + e.Row.RowIndex;

                if (e.Row.Cells[2].Text.Equals("OPERACION") ||
                    e.Row.Cells[2].Text.Equals("VARIABLE"))
                {
                    e.Row.Cells[3].Enabled = false;
                    ((TextBox)e.Row.Cells[3].FindControl("Txt_Cantidad_Deduccion")).Style.Add("display", "none");
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Deducciones_Sindicato
    /// DESCRIPCION : Consultar las Deducciones del Sindicato Seleccionado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 11/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Deducciones_Sindicato()
    {
        Cls_Cat_Nom_Sindicatos_Negocio Rs_Consulta_Cat_Nom_Sindicatos = new Cls_Cat_Nom_Sindicatos_Negocio();
        int index = Grid_Sindicato.SelectedIndex;
        DataTable Dt_Deducciones_Sindicato;

        if (index != -1)
        {
            Rs_Consulta_Cat_Nom_Sindicatos.P_Sindicato_ID = Grid_Sindicato.Rows[index].Cells[1].Text;
            Rs_Consulta_Cat_Nom_Sindicatos.P_Tipo_Percepcion = "DEDUCCION";
            Dt_Deducciones_Sindicato = Rs_Consulta_Cat_Nom_Sindicatos.Consultar_Percepciones_Deducciones();
            Dt_Deducciones_Sindicato = Juntar_Clave_Nombre(Dt_Deducciones_Sindicato);
            Session["Dt_Deducciones_Grid"] = Dt_Deducciones_Sindicato;

            Grid_Deducciones.Columns[0].Visible = true;
            Grid_Deducciones.DataSource = (DataTable)Session["Dt_Deducciones_Grid"];
            Grid_Deducciones.DataBind();
            Grid_Deducciones.Columns[0].Visible = true;
            Cargar_Cantidad_Grid_Percepciones_Deducciones(Grid_Deducciones, (DataTable)Session["Dt_Deducciones_Grid"], "Txt_Cantidad_Deduccion");
        }
    }
    #endregion

    #region (Grid Antiguedad Sindicatos)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Antiguedad_Sindicato_RowDataBound
    /// DESCRIPCION : Agrega un identificador al boton de cancelar de la tabla
    /// para identicar la fila seleccionada de tabla.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 27/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Antiguedad_Sindicato_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                ((ImageButton)e.Row.Cells[3].FindControl("Btn_Eliminar_Antiguedad_Sindical")).CommandArgument = e.Row.Cells[0].Text.Trim();
                ((ImageButton)e.Row.Cells[3].FindControl("Btn_Eliminar_Antiguedad_Sindical")).ToolTip = "Quitar la Antiguedad Sindical " + e.Row.Cells[1].Text + " al Sindicato";
                ((TextBox)e.Row.Cells[2].FindControl("Txt_Cantidad_Antiguedad_Sindical")).ToolTip = "" + e.Row.RowIndex;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Percepciones_Sindicato
    /// DESCRIPCION : Consultar las Percepciones del Sindicato Seleccionado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Antiguedades_Sindicales()
    {
        Cls_Cat_Nom_Sindicatos_Negocio Cat_Sindicatos = new Cls_Cat_Nom_Sindicatos_Negocio();
        int index = Grid_Sindicato.SelectedIndex;
        DataTable Dt_Antiguedades_Sindicato;

        if (index != -1)
        {
            Cat_Sindicatos.P_Sindicato_ID = Grid_Sindicato.Rows[index].Cells[1].Text;
            Dt_Antiguedades_Sindicato = Cat_Sindicatos.Consultar_Antiguedades_Sindicales();
            Session["Dt_Antiguedad_Sindicato_Grid"] = Dt_Antiguedades_Sindicato;
            Grid_Antiguedad_Sindicato.DataSource = (DataTable)Session["Dt_Antiguedad_Sindicato_Grid"];
            Grid_Antiguedad_Sindicato.DataBind();

            Cargar_Cantidad_Grid_Antiguedad_Sindicato(Grid_Antiguedad_Sindicato, (DataTable)Session["Dt_Antiguedad_Sindicato_Grid"], "Txt_Cantidad_Antiguedad_Sindical");
        }
    }
    #endregion

    #endregion

    #region (Eventos)

    #region (Eventos [Alta - Modificar - Eliminar - Busqueda])
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Busqueda_Sindicato_Click
    /// DESCRIPCION : Ejecuta la busqueda del sindicato
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 30-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Busqueda_Sindicato_Click(object sender, ImageClickEventArgs e)
    {

        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Consulta_Sindicatos(); //Consulta los Nombre que coincidan con el sindicato porporcionado por el usuario
            Limpia_Controles();    //Limpia los controles de la forma
            //Si no se encontraron Sindicatos con un nombre similar al proporcionado por el usuario entonces manda un mensaje al usuario
            if (Grid_Sindicato.Rows.Count <= 0)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No se encontraron Sindicatos con el Nombre proporcionado <br>";
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
    /// NOMBRE DE LA FUNCION: Btn_Nuevo_Click
    /// DESCRIPCION : Ejecuta el Alta de un nuevo Sindicato
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 30-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (Btn_Nuevo.ToolTip == "Nuevo")
            {
                Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                Limpia_Controles();           //Limpia los controles de la forma para poder introducir nuevos datos
            }
            else
            {
                //Si todos los campos requeridos fueron proporcionados por el usuario entonces da de alta los mismo en la base de datos
                if (Txt_Nombre_Sindicato.Text != "" & Txt_Responsable_Sindicato.Text != "" & Txt_Comentarios_Sindicato.Text.Length <= 250)
                {
                    if (Validar_Campo_Cantidad_In_Gridview(Grid_Percepciones, "Txt_Cantidad_Percepcion"))
                    {
                        if (Validar_Campo_Cantidad_In_Gridview(Grid_Deducciones, "Txt_Cantidad_Deduccion"))
                        {
                            if (Validar_Campo_Cantidad_In_Gridview(Grid_Antiguedad_Sindicato, "Txt_Cantidad_Antiguedad_Sindical"))
                            {
                                if (Validar_Campo_Cantidad_In_Gridview(Grid_Antiguedad_Sindicato, "Txt_Cantidad_Antiguedad_Sindical"))
                                {
                                    Alta_Sindicato(); //Da de alta los datos proporcionados por el usuario
                                }
                                else
                                {
                                    Lbl_Mensaje_Error.Visible = true;
                                    Img_Error.Visible = true;
                                    Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Hay Antiguedades sin una cantidad asignada <br>";
                                }
                            }
                        }
                        else {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Hay Deducciones sin una cantidad asignada <br>";
                        }
                    }
                    else {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Hay Percepciones sin una cantidad asignada <br>";                        
                    }
                }
                //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                    if (Txt_Nombre_Sindicato.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Nombre del Sindicato <br>";
                    }
                    if (Txt_Responsable_Sindicato.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Responsable del Sindicato <br>";
                    }
                    if (Txt_Comentarios_Sindicato.Text.Length > 250)
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Los comentarios proporcionados no deben ser mayor a 250 caracteres <br>";
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
    /// NOMBRE DE LA FUNCION: Btn_Modificar_Click
    /// DESCRIPCION : Ejecuta la Modificacion del Sindicato Seleccionado
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 30-Agosto-2010
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
            if (Btn_Modificar.ToolTip == "Modificar")
            {
                if (Txt_Sindicato_ID.Text != "")
                {
                    Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el Sindicato que desea modificar sus datos <br>";
                }
            }
            else
            {
                //Si todos los campos requeridos fueron proporcionados por el usuario entonces modifica estos en la BD
                if (Txt_Nombre_Sindicato.Text != "" & Txt_Responsable_Sindicato.Text != "" & Txt_Comentarios_Sindicato.Text.Length <= 250)
                {
                    if (Validar_Campo_Cantidad_In_Gridview(Grid_Percepciones, "Txt_Cantidad_Percepcion"))
                    {
                        if (Validar_Campo_Cantidad_In_Gridview(Grid_Deducciones, "Txt_Cantidad_Deduccion"))
                        {
                            if (Validar_Campo_Cantidad_In_Gridview(Grid_Antiguedad_Sindicato, "Txt_Cantidad_Antiguedad_Sindical"))
                            {
                               Modificar_Sindicato(); //Modifica los datos del Sindicato con los datos proporcionados por el usuario 
                            }
                            else
                            {
                                Lbl_Mensaje_Error.Visible = true;
                                Img_Error.Visible = true;
                                Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Hay Antiguedades sin una cantidad asignada <br>";
                            }                            
                        }
                        else
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Hay Deducciones sin una cantidad asignada <br>";
                        }
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Hay Percepciones sin una cantidad asignada <br>";
                    }
                }
                //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                    if (Txt_Nombre_Sindicato.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Nombre del Sindicato <br>";
                    }
                    if (Txt_Responsable_Sindicato.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Responsable del Sindicato <br>";
                    }
                    if (Txt_Comentarios_Sindicato.Text.Length > 250)
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Los comentarios proporcionados no deben ser mayor a 250 caracteres <br>";
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
    /// NOMBRE DE LA FUNCION: Btn_Eliminar_Click
    /// DESCRIPCION : Ejecuta la baja del sindicato seleccionado
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 30-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            //Si el usuario selecciono un Sindicato entonces lo elimina de la base de datos
            if (Txt_Sindicato_ID.Text != "")
            {
                Eliminar_Sindicato(); //Elimina el Sindicato que fue seleccionado por el usuario
            }
            //Si el usuario no selecciono algun Sindicato manda un mensaje indicando que es necesario que seleccione algun para
            //poder eliminar
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Seleccione el Sindicato que desea eliminar <br>";
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
    /// NOMBRE DE LA FUNCION: Btn_Salir_Click
    /// DESCRIPCION : Cancela la operacion actual o sirve de salida hacia la pagina
    /// principal
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 30-Agosto-2010
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
                Session.Remove("Consulta_Sindicatos");
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
    #endregion

    #region (Eventos [Agregar- Elimnar Percepciones al Grid Percepciones])
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Agregar_Percepcion
    /// DESCRIPCION : Agrega una nueva percepcion a la tabla de percepciones.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Agregar_Percepcion(object sender, EventArgs e)
    {
        if (Cmb_Percepciones.SelectedIndex > 0)
        {
            if (Session["Dt_Percepciones_Grid"] != null)
            {
                Agregar_Percepcion((DataTable)Session["Dt_Percepciones_Grid"], Grid_Percepciones, Cmb_Percepciones);
            }
            else
            {
                DataTable Dt_Percepciones = new DataTable();
                Dt_Percepciones.Columns.Add(Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID, typeof(System.String));
                Dt_Percepciones.Columns.Add(Cat_Nom_Percepcion_Deduccion.Campo_Nombre, typeof(System.String));
                Dt_Percepciones.Columns.Add(Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion, typeof(System.String));
                Dt_Percepciones.Columns.Add(Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Cantidad, typeof(System.String));

                Session["Dt_Percepciones_Grid"] = Dt_Percepciones;
                Grid_Percepciones.DataSource = (DataTable)Session["Dt_Percepciones_Grid"];
                Grid_Percepciones.DataBind();

                Agregar_Percepcion(Dt_Percepciones, Grid_Percepciones, Cmb_Percepciones);
            }
        }
        else {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                "alert('No se a seleccionado ninguna percepcion a agregar');", true);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Agregar_Percepcion
    /// DESCRIPCION : Agrega una nueva percepcion a la tabla de percepciones.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Agregar_Percepcion(DataTable _DataTable, GridView _GridView, DropDownList _DropDownList)
    {
        DataRow[] Filas;
        DataTable Dt = (DataTable)Session["Dt_Percepciones_Grid"];
        Cls_Cat_Nom_Percepciones_Deducciones_Business Cat_Percepciones_Deducciones = new Cls_Cat_Nom_Percepciones_Deducciones_Business();

        try
        {
            int index = _DropDownList.SelectedIndex;
            if (index > 0)
            {
                Filas = _DataTable.Select(Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + "='" + _DropDownList.SelectedValue.Trim() + "'");
                if (Filas.Length > 0)
                {
                    //Si se encontro algun coincidencia entre el grupo a agregar con alguno agregado anteriormente, se avisa
                    //al usuario que elemento ha agregar ya existe en la tabla de grupos.
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                        "alert('No se puede agregar la Percepción, ya que esta ya se ha agregado');", true);
                    Cmb_Percepciones.SelectedIndex = 0;
                }
                else
                {
                    DataTable Dt_Temporal = Cat_Percepciones_Deducciones.Busqueda_Percepcion_Deduccion_Por_ID(_DropDownList.SelectedValue.Trim());
                    if (!(Dt_Temporal == null))
                    {
                        if (Dt_Temporal.Rows.Count > 0)
                        {
                            DataRow row = Dt.NewRow();
                            row[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID] = Dt_Temporal.Rows[0][0].ToString();
                            row[Cat_Nom_Percepcion_Deduccion.Campo_Nombre] = "[" + Dt_Temporal.Rows[0][15].ToString() + "] - " + Dt_Temporal.Rows[0][1].ToString();
                            row[Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion] = Dt_Temporal.Rows[0][5].ToString();

                            Dt.Rows.Add(row);
                            Dt.AcceptChanges();
                            Session["Dt_Percepciones_Grid"] = Dt;
                            _GridView.Columns[0].Visible = true;
                            _GridView.DataSource = (DataTable)Session["Dt_Percepciones_Grid"];
                            _GridView.DataBind();
                            _GridView.Columns[0].Visible = false;
                            Cmb_Percepciones.SelectedIndex = 0;

                            Cargar_Cantidad_Grid_Percepciones_Deducciones(Grid_Percepciones, (DataTable)Session["Dt_Percepciones_Grid"], "Txt_Cantidad_Percepcion");
                        }
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                    "alert('No se a seleccionado ninguna percepcion a agregar');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al agregar la percepcion al Grid de Percepciones" + Ex.Message);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Delete_Percepcion
    /// DESCRIPCION : Elimina la fila seleccionada del Grid de Percepciones.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Delete_Percepcion(object sender, EventArgs e)
    {
        ImageButton Btn_Eliminar_Percepcion = (ImageButton)sender;
        DataTable Dt_Percepciones = (DataTable)Session["Dt_Percepciones_Grid"];
        DataRow[] Filas = Dt_Percepciones.Select(Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID +
                "='" + Btn_Eliminar_Percepcion.CommandArgument + "'");

        if (!(Filas == null))
        {
            if (Filas.Length > 0)
            {
                Dt_Percepciones.Rows.Remove(Filas[0]);
                Session["Dt_Percepciones_Grid"] = Dt_Percepciones;
                Grid_Percepciones.DataSource = (DataTable)Session["Dt_Percepciones_Grid"];

                Grid_Percepciones.Columns[0].Visible = true;
                Grid_Percepciones.DataBind();
                Cmb_Percepciones.SelectedIndex = 0;
                Grid_Percepciones.Columns[0].Visible = false;

                Cargar_Cantidad_Grid_Percepciones_Deducciones(Grid_Percepciones, (DataTable)Session["Dt_Percepciones_Grid"], "Txt_Cantidad_Percepcion");
            }
        }
    }
    #endregion

    #region (Eventos [Agregar - Eliminar Deducciones al Grid Deducciones])
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Agregar_Deduccion
    /// DESCRIPCION : Agrega una nueva deduccion a la tabla de deducciones.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Agregar_Deduccion(object sender, EventArgs e)
    {
        if (Cmb_Deducciones.SelectedIndex > 0)
        {
            if (Session["Dt_Deducciones_Grid"] != null)
            {
                Agregar_Deduccion((DataTable)Session["Dt_Deducciones_Grid"], Grid_Deducciones, Cmb_Deducciones);

            }
            else
            {
                DataTable Dt_Deducciones = new DataTable();
                Dt_Deducciones.Columns.Add(Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID, typeof(System.String));
                Dt_Deducciones.Columns.Add(Cat_Nom_Percepcion_Deduccion.Campo_Nombre, typeof(System.String));
                Dt_Deducciones.Columns.Add(Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion, typeof(System.String));
                Dt_Deducciones.Columns.Add(Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Cantidad, typeof(System.String));

                Session["Dt_Deducciones_Grid"] = Dt_Deducciones;
                Grid_Deducciones.DataSource = (DataTable)Session["Dt_Deducciones_Grid"];
                Grid_Deducciones.DataBind();

                Agregar_Deduccion(Dt_Deducciones, Grid_Deducciones, Cmb_Deducciones);
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                "alert('No se a seleccionado ninguna percepcion a agregar');", true);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Agregar_Deduccion
    /// DESCRIPCION : Agrega una nueva deduccion a la tabla de deducciones.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 11/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Agregar_Deduccion(DataTable _DataTable, GridView _GridView, DropDownList _DropDownList)
    {
        DataRow[] Filas;
        DataTable Dt = (DataTable)Session["Dt_Deducciones_Grid"];
        Cls_Cat_Nom_Percepciones_Deducciones_Business Cat_Percepciones_Deducciones = new Cls_Cat_Nom_Percepciones_Deducciones_Business();

        try
        {
            int index = _DropDownList.SelectedIndex;
            if (index > 0)
            {
                Filas = _DataTable.Select(Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + "='" + _DropDownList.SelectedValue.Trim() + "'");
                if (Filas.Length > 0)
                {
                    //Si se encontro algun coincidencia entre el grupo a agregar con alguno agregado anteriormente, se avisa
                    //al usuario que elemento ha agregar ya existe en la tabla de grupos.
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                        "alert('No se puede agregar la Deduccion, ya que esta ya se ha agregado');", true);
                    Cmb_Deducciones.SelectedIndex = 0;
                }
                else
                {
                    DataTable Dt_Temporal = Cat_Percepciones_Deducciones.Busqueda_Percepcion_Deduccion_Por_ID(_DropDownList.SelectedValue.Trim());
                    if (!(Dt_Temporal == null))
                    {
                        if (Dt_Temporal.Rows.Count > 0)
                        {
                            DataRow row = Dt.NewRow();
                            row[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID] = Dt_Temporal.Rows[0][0].ToString();
                            row[Cat_Nom_Percepcion_Deduccion.Campo_Nombre] = "[" + Dt_Temporal.Rows[0][15].ToString() + "] - " + Dt_Temporal.Rows[0][1].ToString();
                            row[Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion] = Dt_Temporal.Rows[0][5].ToString();

                            Dt.Rows.Add(row);
                            Dt.AcceptChanges();
                            Session["Dt_Deducciones_Grid"] = Dt;
                            _GridView.Columns[0].Visible = true;
                            _GridView.DataSource = (DataTable)Session["Dt_Deducciones_Grid"];
                            _GridView.DataBind();
                            _GridView.Columns[0].Visible = false;
                            Cmb_Deducciones.SelectedIndex = 0;

                            Cargar_Cantidad_Grid_Percepciones_Deducciones(Grid_Deducciones, (DataTable)Session["Dt_Deducciones_Grid"], "Txt_Cantidad_Deduccion");
                        }
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                    "alert('No se a seleccionado ninguna deduccion a agregar');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al agregar la deduccion al Grid de Deducciones" + Ex.Message);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Delete_Deduccion
    /// DESCRIPCION : Elimina la fila seleccionada del Grid de Deducciones.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 11/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Delete_Deduccion(object sender, EventArgs e)
    {
        ImageButton Btn_Eliminar_Deduccion = (ImageButton)sender;
        DataTable Dt_Deducciones = (DataTable)Session["Dt_Deducciones_Grid"];
        DataRow[] Filas = Dt_Deducciones.Select(Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID +
                "='" + Btn_Eliminar_Deduccion.CommandArgument + "'");

        if (!(Filas == null))
        {
            if (Filas.Length > 0)
            {
                Dt_Deducciones.Rows.Remove(Filas[0]);
                Session["Dt_Deducciones_Grid"] = Dt_Deducciones;
                Grid_Deducciones.Columns[0].Visible = true;
                Grid_Deducciones.DataSource = (DataTable)Session["Dt_Deducciones_Grid"];
                Grid_Deducciones.DataBind();
                Cmb_Deducciones.SelectedIndex = 0;
                Grid_Deducciones.Columns[0].Visible = false;

                Cargar_Cantidad_Grid_Percepciones_Deducciones(Grid_Deducciones, (DataTable)Session["Dt_Deducciones_Grid"], "Txt_Cantidad_Deduccion");
            }
        }
    }
    #endregion

    #region (Eventos - Eliminar Antiguedad Sindicato al Grid Antiguedad Sindicato)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Agregar_Antiguedad_Sindicato_Click
    /// DESCRIPCION : Agrega una nueva antiguedad a la tabla de antiguedades sindicales.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 24/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Agregar_Antiguedad_Sindicato_Click(object sender, EventArgs e)
    {
        if (Cmb_Antiguedad_Sindicato.SelectedIndex > 0)
        {
            if (Session["Dt_Antiguedad_Sindicato_Grid"] != null)
            {
                Agregar_Antiguedad_Sindicato((DataTable)Session["Dt_Antiguedad_Sindicato_Grid"], Grid_Antiguedad_Sindicato, Cmb_Antiguedad_Sindicato);

            }
            else
            {
                DataTable Dt_Antiguedades_Sindicales = new DataTable();
                Dt_Antiguedades_Sindicales.Columns.Add(Cat_Nom_Ant_Sin_Det.Campo_Antiguedad_Sindicato_ID, typeof(System.String));
                Dt_Antiguedades_Sindicales.Columns.Add(Cat_Nom_Antiguedad_Sindicato.Campo_Anios, typeof(System.String));
                Dt_Antiguedades_Sindicales.Columns.Add(Cat_Nom_Ant_Sin_Det.Campo_Monto, typeof(System.String));

                Session["Dt_Antiguedad_Sindicato_Grid"] = Dt_Antiguedades_Sindicales;
                Grid_Antiguedad_Sindicato.DataSource = (DataTable)Session["Dt_Antiguedad_Sindicato_Grid"];
                Grid_Antiguedad_Sindicato.DataBind();

                Agregar_Antiguedad_Sindicato(Dt_Antiguedades_Sindicales, Grid_Antiguedad_Sindicato, Cmb_Antiguedad_Sindicato);
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                "alert('No se a seleccionado ninguna antiguedad a agregar');", true);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Agregar_Antiguedad_Sindicato
    /// DESCRIPCION : Agrega una nueva antiguedad a la tabla de antiguedades sindicales
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 27/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Agregar_Antiguedad_Sindicato(DataTable _DataTable, GridView _GridView, DropDownList _DropDownList)
    {
        DataRow[] Filas;
        DataTable Dt = (DataTable)Session["Dt_Antiguedad_Sindicato_Grid"];
        Cls_Cat_Nom_Antiguedad_Sindicato_Negocio Cls_Antiguedad_Sindicato = new Cls_Cat_Nom_Antiguedad_Sindicato_Negocio();

        try
        {
            int index = _DropDownList.SelectedIndex;
            if (index > 0)
            {
                Filas = _DataTable.Select(Cat_Nom_Antiguedad_Sindicato.Campo_Antiguedad_Sindicato_ID + "='" + _DropDownList.SelectedValue.Trim() + "'");
                if (Filas.Length > 0)
                {
                    //Si se encontro algun coincidencia entre el grupo a agregar con alguno agregado anteriormente, se avisa
                    //al usuario que elemento ha agregar ya existe en la tabla de grupos.
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                        "alert('No se puede agregar la Antiguedad, ya que esta ya se ha agregado');", true);
                    Cmb_Deducciones.SelectedIndex = 0;
                }
                else
                {
                    Cls_Antiguedad_Sindicato.P_Antiguedad_Sindicato_ID = _DropDownList.SelectedValue.Trim();
                    DataTable Dt_Temporal = Cls_Antiguedad_Sindicato.Consultar_Antiguedad_Sindicato();
                    if (!(Dt_Temporal == null))
                    {
                        if (Dt_Temporal.Rows.Count > 0)
                        {
                            DataRow row = Dt.NewRow();
                            row[Cat_Nom_Antiguedad_Sindicato.Campo_Antiguedad_Sindicato_ID] = Dt_Temporal.Rows[0][0].ToString();
                            row[Cat_Nom_Antiguedad_Sindicato.Campo_Anios] = Dt_Temporal.Rows[0][1].ToString();
                            Dt.Rows.Add(row);
                            Dt.AcceptChanges();
                            Session["Dt_Antiguedad_Sindicato_Grid"] = Dt;
                            _GridView.DataSource = (DataTable)Session["Dt_Antiguedad_Sindicato_Grid"];
                            _GridView.DataBind();
                            Cmb_Antiguedad_Sindicato.SelectedIndex = 0;

                            Cargar_Cantidad_Grid_Antiguedad_Sindicato(Grid_Antiguedad_Sindicato, (DataTable)Session["Dt_Antiguedad_Sindicato_Grid"], "Txt_Cantidad_Antiguedad_Sindical");
                        }
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                    "alert('No se a seleccionado ninguna antiguedad a agregar');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al agregar antiguedad al Grid de Antiguedades Sindicales" + Ex.Message);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Eliminar_Antiguedad_Sindical_Click
    /// DESCRIPCION : Elimina la fila seleccionada del Grid de Antiguedades Sindicales.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 27/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Eliminar_Antiguedad_Sindical_Click(object sender, EventArgs e)
    {
        ImageButton Btn_Eliminar_Antiguedad_Sindicato = (ImageButton)sender;
        DataTable Dt_Antiguedad_Sindicato = (DataTable)Session["Dt_Antiguedad_Sindicato_Grid"];
        DataRow[] Filas = Dt_Antiguedad_Sindicato.Select(Cat_Nom_Antiguedad_Sindicato.Campo_Antiguedad_Sindicato_ID +
                "='" + Btn_Eliminar_Antiguedad_Sindicato.CommandArgument + "'");

        if (!(Filas == null))
        {
            if (Filas.Length > 0)
            {
                Dt_Antiguedad_Sindicato.Rows.Remove(Filas[0]);
                Session["Dt_Antiguedad_Sindicato_Grid"] = Dt_Antiguedad_Sindicato;
                Grid_Antiguedad_Sindicato.DataSource = (DataTable)Session["Dt_Antiguedad_Sindicato_Grid"];
                Grid_Antiguedad_Sindicato.DataBind();
                Cmb_Antiguedad_Sindicato.SelectedIndex = 0;

                Cargar_Cantidad_Grid_Antiguedad_Sindicato(Grid_Antiguedad_Sindicato, (DataTable)Session["Dt_Antiguedad_Sindicato_Grid"], "Txt_Cantidad_Antiguedad_Sindical");
            }
        }
    }
    #endregion

    #region (Eventos TextBox Cantidad Inner GridView Percepciones y Deducciones)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Txt_Cantidad_Percepcion_TextChanged
    /// DESCRIPCION : Actualiza la informacion del DataTable de Percepciones
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 11/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Txt_Cantidad_Percepcion_TextChanged(object sender, EventArgs e)
    {
        TextBox _Txt_Cantidad_Percepcion = (TextBox)sender;
        if (Session["Dt_Percepciones_Grid"] != null)
        {
            DataTable Dt_Percepciones = ((DataTable)Session["Dt_Percepciones_Grid"]);
            Dt_Percepciones.DefaultView.AllowEdit = true;
            Dt_Percepciones.Rows[Convert.ToInt32(_Txt_Cantidad_Percepcion.ToolTip)].BeginEdit();
            Dt_Percepciones.Rows[Convert.ToInt32(_Txt_Cantidad_Percepcion.ToolTip)][Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Cantidad] =
                (_Txt_Cantidad_Percepcion.Text.Equals("") || _Txt_Cantidad_Percepcion.Text.Equals("$  _,___,___.__") || _Txt_Cantidad_Percepcion.Text.Contains("$")) ? "0" : _Txt_Cantidad_Percepcion.Text;
            Dt_Percepciones.Rows[Convert.ToInt32(_Txt_Cantidad_Percepcion.ToolTip)].EndEdit();

            Session["Dt_Percepciones_Grid"] = Dt_Percepciones;                
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Txt_Cantidad_Deduccion_TextChanged
    /// DESCRIPCION : Actualiza la informacion del DataTable de Deducciones
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 11/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Txt_Cantidad_Deduccion_TextChanged(object sender, EventArgs e)
    {
        TextBox _Txt_Cantidad_Deduccion = (TextBox)sender;
        if (Session["Dt_Deducciones_Grid"] != null)
        {
            DataTable Dt_Deducciones = ((DataTable)Session["Dt_Deducciones_Grid"]);
            Dt_Deducciones.DefaultView.AllowEdit = true;
            Dt_Deducciones.Rows[Convert.ToInt32(_Txt_Cantidad_Deduccion.ToolTip)].BeginEdit();
            Dt_Deducciones.Rows[Convert.ToInt32(_Txt_Cantidad_Deduccion.ToolTip)][Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Cantidad] =
                (_Txt_Cantidad_Deduccion.Text.Equals("") || _Txt_Cantidad_Deduccion.Text.Equals("$  _,___,___.__") || _Txt_Cantidad_Deduccion.Text.Contains("$")) ? "0" : _Txt_Cantidad_Deduccion.Text;
            Dt_Deducciones.Rows[Convert.ToInt32(_Txt_Cantidad_Deduccion.ToolTip)].EndEdit();

            Session["Dt_Deducciones_Grid"] = Dt_Deducciones;
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Txt_Cantidad_Antiguedad_Sindical_TextChanged
    /// DESCRIPCION : Actualiza la informacion del DataTable de Antiguedades Sindicato 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 27/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Txt_Cantidad_Antiguedad_Sindical_TextChanged(object sender, EventArgs e)
    {
        TextBox _Txt_Monto_Antiguedad_Sindicato = (TextBox)sender;
        if (Session["Dt_Antiguedad_Sindicato_Grid"] != null)
        {
            DataTable Dt_Antiguedad_Sindicato = ((DataTable)Session["Dt_Antiguedad_Sindicato_Grid"]);
            Dt_Antiguedad_Sindicato.DefaultView.AllowEdit = true;
            Dt_Antiguedad_Sindicato.Rows[Convert.ToInt32(_Txt_Monto_Antiguedad_Sindicato.ToolTip)].BeginEdit();
            Dt_Antiguedad_Sindicato.Rows[Convert.ToInt32(_Txt_Monto_Antiguedad_Sindicato.ToolTip)][Cat_Nom_Ant_Sin_Det.Campo_Monto] =
                (_Txt_Monto_Antiguedad_Sindicato.Text.Equals("") || _Txt_Monto_Antiguedad_Sindicato.Text.Equals("$  _,___,___.__") || _Txt_Monto_Antiguedad_Sindicato.Text.Contains("$")) ? "0" : _Txt_Monto_Antiguedad_Sindicato.Text;
            Dt_Antiguedad_Sindicato.Rows[Convert.ToInt32(_Txt_Monto_Antiguedad_Sindicato.ToolTip)].EndEdit();

            Session["Dt_Antiguedad_Sindicato_Grid"] = Dt_Antiguedad_Sindicato;
        }
    }
    #endregion

    #endregion

}
