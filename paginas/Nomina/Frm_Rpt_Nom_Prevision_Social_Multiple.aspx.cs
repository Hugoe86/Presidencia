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
using Presidencia.Empleados.Negocios;
using Presidencia.Empleados.Negocios;
using Presidencia.Dependencias.Negocios;
using Presidencia.Constantes;
using Presidencia.Bancos_Nomina.Negocio;
using Presidencia.Ayudante_Informacion;
using Presidencia.PSM.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Sessiones;
using System.Text;
using Presidencia.Tipos_Nominas.Negocios;

public partial class paginas_Nomina_Frm_Rpt_Nom_Prevision_Social_Multiple : System.Web.UI.Page
{
    #region (Page Load)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Page_Load
    /// 
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    ///               
    /// PARAMETROS  : No Aplica.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 08/Diciembre/2011
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
                Inicializa_Controles();
            }

            Div_Contenedor_Msj_Error.Visible = false;
            Lbl_Ecabezado_Mensaje.Text = String.Empty;
            Lbl_Ecabezado_Mensaje.Visible = false;
        }
        catch (Exception ex)
        {
            Lbl_Ecabezado_Mensaje.Text = ex.Message;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    #endregion

    #region (Metodos)

    #region (Generales)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// 
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    ///               
    /// PARAMETROS  : No Aplica.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 08/Diciembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Limpiar_Controles();
            Habilitar_Controles("Inicial"); //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar
            Consultar_SAP_Unidades_Responsables();
            Consultar_Tipos_Nominas();
            Consultar_Unidades_Responsables();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al inicializar los controles de la página. Error: [" + ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///               para a siguiente operación
    /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
    ///                          si es una alta, modificacion
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 08/Diciembre/2011
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
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Generar_Reporte_PDF.ToolTip = "Modificar";
                    Btn_Generar_Reporte_PDF.Visible = true;
                    Btn_Generar_Reporte_PDF.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Generar_Reporte_PDF.ImageUrl = "~/paginas/imagenes/paginas/icono_rep_pdf.png";
                    break;

                case "Modificar":
                    Habilitado = true;
                    Btn_Generar_Reporte_PDF.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Generar_Reporte_PDF.Visible = true;
                    Btn_Generar_Reporte_PDF.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Generar_Reporte_PDF.ImageUrl = "~/paginas/imagenes/paginas/icono_rep_pdf.png";
                    break;
            }


        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Ctlr
    /// DESCRIPCION : Limpia los Controles de la pagina.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 08/Diciembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        Txt_No_Empleado.Text = String.Empty;
        Txt_Nombre_Empleado.Text = String.Empty;
        Grid_Busqueda_Empleados.SelectedIndex = -1;
    }
    #endregion

    #region (Consultas)
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

    #region (Operación)
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
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
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

                Txt_No_Empleado.Text = HttpUtility.HtmlDecode(Grid_Busqueda_Empleados.Rows[Grid_Busqueda_Empleados.SelectedIndex].Cells[2].Text);
                Txt_Nombre_Empleado.Text = HttpUtility.HtmlDecode(Grid_Busqueda_Empleados.Rows[Grid_Busqueda_Empleados.SelectedIndex].Cells[3].Text);

                MPE_Empleados.Hide();
                UpPnl_Rpt_PSM.Update();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    #endregion

    #region (Eventos)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Reporte_PDF_Click
    ///DESCRIPCIÓN: Actualizar ,los datos bancarios del empleado.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 08/Diciembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Generar_Reporte_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Rpt_Nom_Prevision_Social_Multiple_Negocio Obj_PSM = new Cls_Rpt_Nom_Prevision_Social_Multiple_Negocio();
        DataTable Dt_PSM = null;
        DataTable Dt_Parametros = null;
        DataSet Ds_Reporte = new DataSet();

        try
        {
            if (!String.IsNullOrEmpty(Txt_No_Empleado.Text))
                Obj_PSM.P_No_Empleado = Txt_No_Empleado.Text;

            if (Cmb_Busqueda_Tipo_Nomina.SelectedIndex > 0)
                Obj_PSM.P_Tipo_Nomina_ID = Cmb_Busqueda_Tipo_Nomina.SelectedValue.Trim();

            if (Cmb_Busqueda_Unidad_Responsable.SelectedIndex > 0)
                Obj_PSM.P_Dependencia_ID = Cmb_Busqueda_Unidad_Responsable.SelectedValue.Trim();

            Dt_PSM = Obj_PSM.Reporte_Prevision_Social_Multiple();

            Dt_PSM.TableName = "PSM";
            Dt_Parametros = Consultar_Parametros_Reporte();
            Dt_Parametros.TableName = "DT_GENERALES_REPORTE";
            Ds_Reporte.Tables.Add(Dt_PSM.Copy());
            Ds_Reporte.Tables.Add(Dt_Parametros.Copy());

            Generar_Reporte(ref Ds_Reporte, "Cr_Rpt_Nom_PSM.rpt", "Reporte_PSM" + Session.SessionID + ".pdf");
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Reporte_PDF_Click
    ///DESCRIPCIÓN: Actualizar ,los datos bancarios del empleado.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 08/Diciembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Generar_Reporte_Excel_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Rpt_Nom_Prevision_Social_Multiple_Negocio Obj_PSM = new Cls_Rpt_Nom_Prevision_Social_Multiple_Negocio();
        DataTable Dt_PSM = null;
        DataTable Dt_Parametros = null;
        DataSet Ds_Reporte = new DataSet();

        try
        {
            if (!String.IsNullOrEmpty(Txt_No_Empleado.Text))
                Obj_PSM.P_No_Empleado = Txt_No_Empleado.Text;

            if (Cmb_Busqueda_Tipo_Nomina.SelectedIndex > 0)
                Obj_PSM.P_Tipo_Nomina_ID = Cmb_Busqueda_Tipo_Nomina.SelectedValue.Trim();

            if (Cmb_Busqueda_Unidad_Responsable.SelectedIndex > 0)
                Obj_PSM.P_Dependencia_ID = Cmb_Busqueda_Unidad_Responsable.SelectedValue.Trim();

            Dt_PSM = Obj_PSM.Reporte_Prevision_Social_Multiple();

            Dt_PSM.TableName = "PSM";
            Dt_Parametros = Consultar_Parametros_Reporte();
            Dt_Parametros.TableName = "DT_GENERALES_REPORTE";
            Ds_Reporte.Tables.Add(Dt_PSM.Copy());
            Ds_Reporte.Tables.Add(Dt_Parametros.Copy());

            Exportar_Reporte(Ds_Reporte, "Cr_Rpt_Nom_PSM.rpt", "Reporte_PSM" + Session.SessionID, "xls", ExportFormatType.Excel);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
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
        Cls_Rpt_Nom_Prevision_Social_Multiple_Negocio Obj_PSM = new Cls_Rpt_Nom_Prevision_Social_Multiple_Negocio();
        DataTable Dt_PSM = null;
        DataTable Dt_Parametros = null;
        DataSet Ds_Reporte = new DataSet();

        try
        {
            if (!String.IsNullOrEmpty(Txt_No_Empleado.Text))
                Obj_PSM.P_No_Empleado = Txt_No_Empleado.Text;

            if (Cmb_Busqueda_Tipo_Nomina.SelectedIndex > 0)
                Obj_PSM.P_Tipo_Nomina_ID = Cmb_Busqueda_Tipo_Nomina.SelectedValue.Trim();

            if (Cmb_Busqueda_Unidad_Responsable.SelectedIndex > 0)
                Obj_PSM.P_Dependencia_ID = Cmb_Busqueda_Unidad_Responsable.SelectedValue.Trim();

            Dt_PSM = Obj_PSM.Reporte_Prevision_Social_Multiple();

            Dt_PSM.TableName = "PSM";
            Dt_Parametros = Consultar_Parametros_Reporte();
            Dt_Parametros.TableName = "DT_GENERALES_REPORTE";
            Ds_Reporte.Tables.Add(Dt_PSM.Copy());
            Ds_Reporte.Tables.Add(Dt_Parametros.Copy());

            Exportar_Reporte(Ds_Reporte, "Cr_Rpt_Nom_PSM.rpt", "Reporte_PSM" + Session.SessionID, "doc", ExportFormatType.WordForWindows);
        }
        catch (System.Threading.ThreadAbortException ex)
        { }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte Catálogo de empleados. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Salir de la Operacion Actual
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 17/Febrero/2011 
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
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Habilitar_Controles("Inicial");//Habilita los controles para la siguiente operación del usuario en el catálogo
                Limpiar_Controles();
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
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
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    #endregion


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

        Dt_Parametros.Columns.Add("ELABORO", typeof(string));
        Dt_Parametros.Columns.Add("NOMBRE_REPORTE", typeof(string));
        Dr_Parametro = Dt_Parametros.NewRow();
        Dr_Parametro["ELABORO"] = Cls_Sessiones.Nombre_Empleado.ToUpper();
        Dr_Parametro["NOMBRE_REPORTE"] = "REPORTE DE PSM";
        Dt_Parametros.Rows.Add(Dr_Parametro);

        return Dt_Parametros;
    }

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
}
