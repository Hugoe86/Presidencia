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
using Presidencia.Cat_Nom_Percepciones_Deducciones_Opcional.Negocios;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Collections.Generic;
using Presidencia.Tipos_Nominas.Negocios;
using Presidencia.Cuentas_Contables.Negocio;

public partial class paginas_Nomina_Frm_Cat_Nom_Percepcion_Deduccion : System.Web.UI.Page
{
    #region (Init/Load)
    protected void Page_Init(object sender, EventArgs e)
    {
        Crear_Columnas_Grid_Percepciones_Deducciones(Grid_Percepciones_Deducciones);//Aqui se crean las columnas de la tabla de percepciones deducciones.
        ViewState["SortDirection"] = "ASC";//Indicamos el orden que tendran las filas del grid al cargarse incialmente.
    }
    /// *********************************************************************************
    /// NOMBRE : Page_Load
    /// DESCRIPCIÓN: Configuracion al Cargar la pagina completamente.
    /// CREO: Juan Alberto Hernández Negrete
    /// FECHA CREO:20 de Septiembre del 2010
    /// MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACION:
    ///*********************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        if( !IsPostBack){
            Estado_Inicial();
            Habilitar_Controles("Inicial");
            Limpiar_Controles(); 
        }
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
    }
    #endregion

    #region Grid

    #region PERCEPCIONES DEDUCCIONES OPCIONALES
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Fill_Grid_Percepciones_Deducciones
    /// DESCRIPCIÓN: LLena el Grid de Percepciones y Deducciones.
    /// CREO: Juan Alberto Hernandez Negrete
    /// FECHA_CREO: 09/Septiembre/2010 14:20 p.m.
    /// MODIFICO:
    /// FECHA_MODIFICO
    /// CAUSA_MODIFICACIÓN   
    ///*******************************************************************************
    private void Consulta_Percepciones_Deducciones()
    {
        Cls_Cat_Nom_Percepciones_Deducciones_Business Cat_Percepciones_Deducciones = new Cls_Cat_Nom_Percepciones_Deducciones_Business();//Variable de conexion con la capa de negocios.
        DataTable DT_Percepciones_Deducciones;//Lista de Percepciones y/o Deducciones.
        Int32 Total_Registros = 1;
        try
        {
            DT_Percepciones_Deducciones = Cat_Percepciones_Deducciones.Fill_Grid_Percepciones_Deducciones();
            Grid_Percepciones_Deducciones.DataSource = DT_Percepciones_Deducciones;
            Grid_Percepciones_Deducciones.DataBind();

            if (DT_Percepciones_Deducciones is DataTable)
            {
                Total_Registros = (DT_Percepciones_Deducciones.Rows.Count == 0) ? Total_Registros : DT_Percepciones_Deducciones.Rows.Count;
                custPager.TotalPages = ((Total_Registros % Grid_Percepciones_Deducciones.PageSize) == 0)? 
                    (Total_Registros / Grid_Percepciones_Deducciones.PageSize):
                    (Total_Registros / Grid_Percepciones_Deducciones.PageSize + 1);
            }
            else {
                custPager.TotalPages = 1;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cargar el Grid de Percepciones Deducciones. Error: ["+ Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Grid_Percepciones_Deducciones_SelectedIndexChanged
    /// DESCRIPCIÓN: Evento de Seleccion que carga todos los datos de la percepcion deduccion seleccionada.
    /// CREO: Juan Alberto Hernandez Negrete
    /// FECHA_CREO: 09/Septiembre/2010 14:20 p.m.
    /// MODIFICO:
    /// FECHA_MODIFICO
    /// CAUSA_MODIFICACIÓN   
    ///*******************************************************************************
    protected void Grid_Percepciones_Deducciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        //int index = Grid_Percepciones_Deducciones.SelectedIndex;
        int index = Grid_Percepciones_Deducciones.SelectedIndex; 
        if (index != -1)
        {
            //Load Data Percepccion Deduccion Seleccionada
            Cargar_Informacion_Percepciones_Deducciones(index);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Grid_Percepciones_Deducciones_PageIndexChanging
    /// DESCRIPCIÓN: Cambia la página de la tabla según la opción seleccionada.
    /// CREO: Juan Alberto Hernandez Negrete
    /// FECHA_CREO: 09/Septiembre/2010 14:20 p.m.
    /// MODIFICO:
    /// FECHA_MODIFICO
    /// CAUSA_MODIFICACIÓN   
    ///*******************************************************************************
    protected void Grid_Percepciones_Deducciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Percepciones_Deducciones.PageIndex = e.NewPageIndex;
            Busqueda_Conceptos();
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode("<b>+</b> " +
                                     "Error generado al cambiar de página en la tabla de Percepciones Deducciones. Error: ["+ex.Message+"]");
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    /// **************************************************************************************************************************************
    /// NOMBRE: Grid_Calendario_Nominas_Sorting
    /// 
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// 
    /// CREÓ:   Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 18/Febrero/2011 19:04 pm.
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// **************************************************************************************************************************************
    protected void Grid_Percepciones_Deducciones_Sorting(object sender, GridViewSortEventArgs e)
    {
        Consulta_Percepciones_Deducciones();
        DataTable Dt_Calendario_Nominas = (Grid_Percepciones_Deducciones.DataSource as DataTable);

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

            Grid_Percepciones_Deducciones.DataSource = Dv_Calendario_Nominas;
            Grid_Percepciones_Deducciones.DataBind();
        }
    }
    #endregion

    #region CREAR COLUMNAS DE LOS GRIDVIEW'S
    /// *********************************************************************************
    /// NOMBRE : Crear_Columnas_Grid_Percepciones_Deducciones
    /// DESCRIPCIÓN: Crear columnas del GridView de Perc Dedu
    /// CREO: Juan Alberto Hernández Negrete
    /// FECHA CREO:20 de Septiembre del 2010
    /// MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACION:
    ///*********************************************************************************
    private void Crear_Columnas_Grid_Percepciones_Deducciones(GridView Grid_Percepcion_Deduccion)
    {
        BoundField Columna_Percepcion_Deduccion_ID = new BoundField();
        Columna_Percepcion_Deduccion_ID.HeaderText = "ID";
        Columna_Percepcion_Deduccion_ID.DataField = Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID;
        Columna_Percepcion_Deduccion_ID.ItemStyle.Width = new Unit(10, UnitType.Percentage);
        Columna_Percepcion_Deduccion_ID.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
        Columna_Percepcion_Deduccion_ID.HeaderStyle.Width = new Unit(10, UnitType.Percentage);
        Columna_Percepcion_Deduccion_ID.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
        Columna_Percepcion_Deduccion_ID.SortExpression = Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID;

        BoundField Columna_Clave = new BoundField();
        Columna_Clave.HeaderText = "Clave";
        Columna_Clave.DataField = Cat_Nom_Percepcion_Deduccion.Campo_Clave;
        Columna_Clave.ItemStyle.Width = new Unit(10, UnitType.Percentage);
        Columna_Clave.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
        Columna_Clave.HeaderStyle.Width = new Unit(10, UnitType.Percentage);
        Columna_Clave.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
        Columna_Clave.SortExpression = Cat_Nom_Percepcion_Deduccion.Campo_Clave;

        BoundField Columna_Nombre = new BoundField();
        Columna_Nombre.HeaderText = "Nombre";
        Columna_Nombre.DataField = Cat_Nom_Percepcion_Deduccion.Campo_Nombre;
        Columna_Nombre.ItemStyle.Width = new Unit(50, UnitType.Percentage);
        Columna_Nombre.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
        Columna_Nombre.HeaderStyle.Width = new Unit(50, UnitType.Percentage);
        Columna_Nombre.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
        Columna_Nombre.SortExpression = Cat_Nom_Percepcion_Deduccion.Campo_Nombre;

        BoundField Columna_Tipo = new BoundField();
        Columna_Tipo.HeaderText = "Tipo";
        Columna_Tipo.DataField = Cat_Nom_Percepcion_Deduccion.Campo_Tipo;
        Columna_Tipo.ItemStyle.Width = new Unit(20, UnitType.Percentage);
        Columna_Tipo.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
        Columna_Tipo.HeaderStyle.Width = new Unit(20, UnitType.Percentage);
        Columna_Tipo.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;

        BoundField Columna_Tipo_Asignacion = new BoundField();
        Columna_Tipo_Asignacion.HeaderText = "Asignacion";
        Columna_Tipo_Asignacion.DataField = Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion;
        Columna_Tipo_Asignacion.ItemStyle.Width = new Unit(20, UnitType.Percentage);
        Columna_Tipo_Asignacion.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
        Columna_Tipo_Asignacion.HeaderStyle.Width = new Unit(20, UnitType.Percentage);
        Columna_Tipo_Asignacion.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;

        Grid_Percepcion_Deduccion.Columns.Insert(1, Columna_Percepcion_Deduccion_ID);
        Grid_Percepcion_Deduccion.Columns.Insert(2, Columna_Clave);
        Grid_Percepcion_Deduccion.Columns.Insert(3, Columna_Nombre);
        Grid_Percepcion_Deduccion.Columns.Insert(4, Columna_Tipo);
        Grid_Percepcion_Deduccion.Columns.Insert(5, Columna_Tipo_Asignacion);
    }
    #endregion

    #endregion

    #region Métodos

    #region METODOS GENERALES
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Estado_Inicial
    /// DESCRIPCIÓN: Vuelve el formulario asu estado inicial.
    /// CREO: Juan Alberto Hernandez Negrete
    /// FECHA_CREO: 09/Septiembre/2010 14:20 p.m.
    /// MODIFICO:
    /// FECHA_MODIFICO
    /// CAUSA_MODIFICACIÓN   
    ///******************************************************************************
    private void Estado_Inicial() {
        Limpiar_Controles();
        Busqueda_Conceptos();
        Consultar_Cuentas_Contables();
    }
    ///******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Habilitar_Controles
    /// DESCRIPCIÓN: Habilita controles según la configuracion deseada.
    /// CREO: Juan Alberto Hernandez Negrete
    /// FECHA_CREO: 09/Septiembre/2010 14:20 p.m.
    /// MODIFICO:
    /// FECHA_MODIFICO
    /// CAUSA_MODIFICACIÓN   
    ///******************************************************************************
    private void Habilitar_Controles(String Modo) {
        try {
            Boolean Habilitado = false;

            //Seleccionar el modo
            switch (Modo) {
                case "Inicial":
                    Habilitado = false;
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = true;
                    Btn_Salir.Visible = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Nuevo.ImageUrl = "../imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Modificar.ImageUrl = "../imagenes/paginas/icono_modificar.png";
                    Btn_Salir.ToolTip = "Salir";
                    Btn_Salir.ImageUrl = "../imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;

                    Txt_Percepcion_Deduccion_ID.Enabled = false;
                    Cmb_Tipo_Deduccion_Percepcion.Enabled = false;
                    Cmb_Estatus.Enabled = Habilitado;
                    Txt_Nombre.Enabled = false;
                    Cmb_Aplicar.Enabled = false;
                    Cmb_Asignar.Enabled = false;
                    Chk_Gravable.Enabled = false;
                    Txt_Cantidad_Porcentual_Gravable.Enabled = false;
                    Txt_Comentarios.Enabled = false;
                    Txt_Busqueda_Percepciones_Deducciones.Enabled = true;
                    Btn_Buscar_Percepcion_Deduccion.Enabled = true;
                    Grid_Percepciones_Deducciones.SelectedIndex = -1;
                    Chk_Aplica_Concepto_Calculo_IMSS.Enabled = false;

                    Configuracion_Acceso("Frm_Cat_Nom_Percepciones_Deducciones.aspx");
                    break;
                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Eliminar.Visible = false;
                    Btn_Salir.Visible = true;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Nuevo.ImageUrl = "../imagenes/paginas/icono_guardar.png";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Modificar.ImageUrl = "../imagenes/paginas/icono_modificar.png";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Salir.ImageUrl = "../imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    
                    Txt_Percepcion_Deduccion_ID.Enabled = false;
                    Cmb_Tipo_Deduccion_Percepcion.Enabled = true;
                    Cmb_Estatus.SelectedIndex = 1;
                    Cmb_Estatus.Enabled = Habilitado;
                    Txt_Nombre.Enabled = true;
                    Cmb_Aplicar.Enabled = Habilitado;
                    Cmb_Asignar.Enabled = false;
                    Chk_Gravable.Enabled = false;
                    Txt_Cantidad_Porcentual_Gravable.Enabled = false;
                    Txt_Comentarios.Enabled = true;
                    Txt_Busqueda_Percepciones_Deducciones.Enabled = false;
                    Btn_Buscar_Percepcion_Deduccion.Enabled = false;
                    Chk_Aplica_Concepto_Calculo_IMSS.Enabled = false;
                    break;

                case "Modificar":
                    Habilitado = true;
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = false;
                    Btn_Salir.Visible = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Nuevo.ImageUrl = "../imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Modificar.ImageUrl = "../imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Salir.ImageUrl = "../imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;

                    Txt_Percepcion_Deduccion_ID.Enabled = false;
                    //No se permite cambiar una percepcion a deduccion
                    Cmb_Tipo_Deduccion_Percepcion.Enabled = false;
                    Txt_Nombre.Enabled = true;
                    Cmb_Estatus.Enabled = Habilitado;
                    Cmb_Aplicar.Enabled = Habilitado;
                    Cmb_Asignar.Enabled = false;
                    Chk_Gravable.Enabled = false;
                    Txt_Cantidad_Porcentual_Gravable.Enabled = false;
                    Txt_Comentarios.Enabled = true;

                    Txt_Busqueda_Percepciones_Deducciones.Enabled = false;
                    Btn_Buscar_Percepcion_Deduccion.Enabled = false;

                    if (Cmb_Tipo_Deduccion_Percepcion.SelectedItem.Text.ToString().Trim().ToUpper().Equals("PERCEPCION"))
                    {
                        Chk_Aplica_Concepto_Calculo_IMSS.Enabled = true;
                    }
                    break;
                default: break;
            }//End Switch

            Btn_Iniciar_Busqueda.Enabled = !Habilitado;
            Txt_Clave_Concepto.Enabled = Habilitado;
            Cmb_Cuentas_Contables.Enabled = Habilitado;
            Cmb_Concepto.Enabled = Habilitado;
            Grid_Percepciones_Deducciones.Enabled = !Habilitado;            
        } catch (Exception ex) {
            throw new Exception(ex.Message.ToString());
        }//End Catch
    }
    ///******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Limpiar_Ctlr
    /// DESCRIPCIÓN: Limpiar todos los controles.
    /// CREO: Juan Alberto Hernandez Negrete
    /// FECHA_CREO: 09/Septiembre/2010 14:20 p.m.
    /// MODIFICO:
    /// FECHA_MODIFICO
    /// CAUSA_MODIFICACIÓN   
    ///******************************************************************************
    private void Limpiar_Controles() {
        Txt_Percepcion_Deduccion_ID.Text = "";
        Txt_Nombre.Text = "";
        Cmb_Tipo_Deduccion_Percepcion.SelectedIndex = -1;
        Cmb_Aplicar.SelectedIndex = -1;
        Cmb_Estatus.SelectedIndex = -1;
        //Cmb_Asignar.SelectedIndex = -1;
        Chk_Gravable.Checked = false;
        Txt_Cantidad_Porcentual_Gravable.Text = "";
        TWM_Txt_Cantidad_Porcentual_Gravable.WatermarkText = "No Aplica";
        TWM_Txt_Cantidad_Porcentual_Gravable.WatermarkCssClass = "inhabilitado";        
        Txt_Comentarios.Text = "";
        Cmb_Concepto.SelectedIndex = -1;
        Chk_Aplica_Concepto_Calculo_IMSS.Checked = false;
        Txt_Clave_Concepto.Text = "";

        Txt_Clave_Percepcion_Deduccion_Busqueda.Text = String.Empty;
        Cmb_Tipo_Busqueda.SelectedIndex = -1;
        Cmb_Estatus_Busqueda.SelectedIndex = -1;
        Txt_Nombre_Busqueda.Text = String.Empty;
        Cmb_Aplica_Busqueda.SelectedIndex = -1;
        Cmb_Concepto_Busqueda.SelectedIndex = -1;
        Cmb_Asignar.SelectedIndex = -1;

        Cmb_Cuentas_Contables.SelectedIndex = -1;
    }
    ///******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Configuracion_Btn_Nuevo
    /// DESCRIPCIÓN: Carga Informacion de acuerdo a una configuracion preestablecida.
    /// CREO: Juan Alberto Hernandez Negrete
    /// FECHA_CREO: 09/Septiembre/2010 14:20 p.m.
    /// MODIFICO:
    /// FECHA_MODIFICO
    /// CAUSA_MODIFICACIÓN   
    ///******************************************************************************
    private void Configuracion_Btn_Nuevo() {
        //Parte de Perc Dedu
        Grid_Percepciones_Deducciones.SelectedIndex = -1;
        //Cuando es nuevo siempre aplica
        Cmb_Aplicar.SelectedIndex = 1;
        Cmb_Estatus.SelectedIndex = 1;
    }
    ///******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Configuracion_Btn_Cancelar
    /// DESCRIPCIÓN: Vuelve a la configuracion de inicio.
    /// CREO: Juan Alberto Hernandez Negrete
    /// FECHA_CREO: 09/Septiembre/2010 14:20 p.m.
    /// MODIFICO:
    /// FECHA_MODIFICO
    /// CAUSA_MODIFICACIÓN   
    ///******************************************************************************
    private void Configuracion_Btn_Cancelar() {
        Consulta_Percepciones_Deducciones();
    }
    ///******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Cargar_Informacion
    /// DESCRIPCIÓN: Consulta la Percepcion Deduccion Seleccionada y Carga los datos 
    /// generales de la percepcion dedudccion seleccionada. 
    /// CREO: Juan Alberto Hernandez Negrete
    /// FECHA_CREO: 11/Septiembre/2010 9:30 a.m.
    /// MODIFICO:
    /// FECHA_MODIFICO
    /// CAUSA_MODIFICACIÓN   
    ///******************************************************************************
    private void Cargar_Informacion_Percepciones_Deducciones(int index)
    {
        String Percepcion_Deduccion_ID = Grid_Percepciones_Deducciones.Rows[index].Cells[1].Text.Trim();//Obtenemos el Id de la percepcion.
        DataTable Dt_Percepciones_Deduccions = null;//Obtenemos la lista de percepciones y/o deducciones.
        Cls_Cat_Nom_Percepciones_Deducciones_Business Cat_Percepciones_Deducciones = new Cls_Cat_Nom_Percepciones_Deducciones_Business();//Variable de conexion con la capa de negocios.

        try
        {
            Dt_Percepciones_Deduccions = Cat_Percepciones_Deducciones.Busqueda_Percepcion_Deduccion_Por_ID(Percepcion_Deduccion_ID);
            //Carga los Datos Generales de la Percepcion Deduccion
            if (Dt_Percepciones_Deduccions != null && Dt_Percepciones_Deduccions.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(Dt_Percepciones_Deduccions.Rows[0][Cat_Nom_Percepcion_Deduccion.Campo_Cuenta_Contable_ID].ToString()))
                    Cmb_Cuentas_Contables.SelectedIndex = Cmb_Cuentas_Contables.Items.IndexOf(Cmb_Cuentas_Contables.Items.FindByValue(
                        Dt_Percepciones_Deduccions.Rows[0][Cat_Nom_Percepcion_Deduccion.Campo_Cuenta_Contable_ID].ToString()));

                if (!string.IsNullOrEmpty(Dt_Percepciones_Deduccions.Rows[0][Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString()))
                    Txt_Percepcion_Deduccion_ID.Text = Dt_Percepciones_Deduccions.Rows[0][Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString();
                if (!string.IsNullOrEmpty(Dt_Percepciones_Deduccions.Rows[0][Cat_Nom_Percepcion_Deduccion.Campo_Nombre].ToString()))
                    Txt_Nombre.Text = Dt_Percepciones_Deduccions.Rows[0][Cat_Nom_Percepcion_Deduccion.Campo_Nombre].ToString();
                if (!string.IsNullOrEmpty(Dt_Percepciones_Deduccions.Rows[0][Cat_Nom_Percepcion_Deduccion.Campo_Estatus].ToString()))
                    Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByText(Dt_Percepciones_Deduccions.Rows[0][Cat_Nom_Percepcion_Deduccion.Campo_Estatus].ToString()));
                if (!string.IsNullOrEmpty(Dt_Percepciones_Deduccions.Rows[0][Cat_Nom_Percepcion_Deduccion.Campo_Clave].ToString()))
                    Txt_Clave_Concepto.Text = Dt_Percepciones_Deduccions.Rows[0][Cat_Nom_Percepcion_Deduccion.Campo_Clave].ToString();

                if (!string.IsNullOrEmpty(Dt_Percepciones_Deduccions.Rows[0][Cat_Nom_Percepcion_Deduccion.Campo_Tipo].ToString()))
                {
                    Cmb_Tipo_Deduccion_Percepcion.SelectedIndex = Cmb_Tipo_Deduccion_Percepcion.Items.IndexOf(Cmb_Tipo_Deduccion_Percepcion.Items.FindByText(Dt_Percepciones_Deduccions.Rows[0]["TIPO"].ToString()));
                }
                if (!string.IsNullOrEmpty(Dt_Percepciones_Deduccions.Rows[0][Cat_Nom_Percepcion_Deduccion.Campo_Aplicar].ToString()))
                    Cmb_Aplicar.SelectedIndex = Cmb_Aplicar.Items.IndexOf(Cmb_Aplicar.Items.FindByText(Dt_Percepciones_Deduccions.Rows[0][Cat_Nom_Percepcion_Deduccion.Campo_Aplicar].ToString()));
                if (!string.IsNullOrEmpty(Dt_Percepciones_Deduccions.Rows[0][Cat_Nom_Percepcion_Deduccion.Campo_Concepto].ToString()))
                {
                    Cmb_Concepto.SelectedIndex = Cmb_Concepto.Items.IndexOf(Cmb_Concepto.Items.FindByText(Dt_Percepciones_Deduccions.Rows[0][Cat_Nom_Percepcion_Deduccion.Campo_Concepto].ToString()));
                    Consultar_Tipo_Asignacion_Por_Concepto(Cmb_Concepto.SelectedItem.Text);

                    if (!string.IsNullOrEmpty(Dt_Percepciones_Deduccions.Rows[0][Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion].ToString()))
                        Cmb_Asignar.SelectedIndex = Cmb_Asignar.Items.IndexOf(Cmb_Asignar.Items.FindByText(Dt_Percepciones_Deduccions.Rows[0][Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion].ToString()));
                }
                if (!string.IsNullOrEmpty(Dt_Percepciones_Deduccions.Rows[0][Cat_Nom_Percepcion_Deduccion.Campo_Gravable].ToString()))
                {
                    Chk_Gravable.Checked = ((Dt_Percepciones_Deduccions.Rows[0][Cat_Nom_Percepcion_Deduccion.Campo_Gravable].ToString().Equals("0") || Dt_Percepciones_Deduccions.Rows[0]["GRAVABLE"].ToString().Equals("")) ? false : true);
                    if (Chk_Gravable.Checked)
                    {
                        if (!string.IsNullOrEmpty(Dt_Percepciones_Deduccions.Rows[0][Cat_Nom_Percepcion_Deduccion.Campo_Porcentaje_Gravable].ToString()))
                            Txt_Cantidad_Porcentual_Gravable.Text = Dt_Percepciones_Deduccions.Rows[0][Cat_Nom_Percepcion_Deduccion.Campo_Porcentaje_Gravable].ToString();
                    }
                    else
                    {
                        Txt_Cantidad_Porcentual_Gravable.Text = "";
                    }
                }
                if (!string.IsNullOrEmpty(Dt_Percepciones_Deduccions.Rows[0][Cat_Nom_Percepcion_Deduccion.Campo_Comentarios].ToString()))
                    Txt_Comentarios.Text = Dt_Percepciones_Deduccions.Rows[0][Cat_Nom_Percepcion_Deduccion.Campo_Comentarios].ToString();


                if (!string.IsNullOrEmpty(Dt_Percepciones_Deduccions.Rows[0][Cat_Nom_Percepcion_Deduccion.Campo_Aplica_IMSS].ToString()))
                {
                    if (Dt_Percepciones_Deduccions.Rows[0][Cat_Nom_Percepcion_Deduccion.Campo_Aplica_IMSS].ToString().Trim().ToUpper().Equals("SI"))
                    {
                        Chk_Aplica_Concepto_Calculo_IMSS.Checked = true;
                    }
                    else
                    {
                        Chk_Aplica_Concepto_Calculo_IMSS.Checked = false;
                    }
                }
                else {
                    Chk_Aplica_Concepto_Calculo_IMSS.Checked = false;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al seleccionar una percepcion y realizar la carga de datos en los controles del catalogo. Error: [" + ex.Message + "]");
        }
    }
    ///******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Consultar_Tipo_Asignacion_Por_Concepto
    /// DESCRIPCIÓN: Carga las opciones de Asignacion por el concepto seleccionado.
    /// CREO: Juan Alberto Hernandez Negrete
    /// FECHA_CREO: 09/Septiembre/2010 14:20 p.m.
    /// MODIFICO:
    /// FECHA_MODIFICO
    /// CAUSA_MODIFICACIÓN   
    ///******************************************************************************
    private void Consultar_Tipo_Asignacion_Por_Concepto(String Concepto)
    {
        switch (Concepto)
        {
            case "TIPO_NOMINA":
                Cmb_Asignar.DataSource = new DataTable();
                Cmb_Asignar.DataBind();
                Cmb_Asignar.Items.Insert(0, new ListItem("<Seleccione>", ""));
                Cmb_Asignar.Items.Insert(1, new ListItem("FIJA", "FIJA"));
                Cmb_Asignar.Items.Insert(2, new ListItem("VARIABLE", "VARIABLE"));
                Cmb_Asignar.Items.Insert(3, new ListItem("OPERACION", "OPERACION"));
                break;
            case "SINDICATO":
                Cmb_Asignar.DataSource = new DataTable();
                Cmb_Asignar.DataBind();
                Cmb_Asignar.Items.Insert(0, new ListItem("<Seleccione>", ""));
                Cmb_Asignar.Items.Insert(1, new ListItem("FIJA", "FIJA"));
                Cmb_Asignar.Items.Insert(2, new ListItem("OPERACION", "OPERACION"));
                break;
            default:
                Cmb_Asignar.Enabled = false;
                break;
        }
    }
    /// **************************************************************************************************************************
    /// Nombre: Generar_Clave_Consecutiva_Percepcion_Deduccion
    /// 
    /// Descripción: Consulta de acuerdo al tipo [Percepción-Deducción] un listado para revizar las claves y obtener la 
    ///              clave consecutiva de acuerdo al tipo. Ej. P-001.
    /// 
    /// Parámetros: Tipo.- Este parámetro indica si se tratara de obtener el consecutivo de la clave de percepciones o deducciones.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 4/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// **************************************************************************************************************************
    protected String Generar_Clave_Consecutiva_Percepcion_Deduccion(String Tipo)
    {
        DataTable Dt_Percepciones_Deducciones = null;
        Cls_Cat_Nom_Percepciones_Deducciones_Business Obj_Perc_Dedu = new Cls_Cat_Nom_Percepciones_Deducciones_Business();
        String Consecutivo_Clave = String.Empty;
        Int32 Consecutivo = 0;

        try
        {
            Obj_Perc_Dedu.P_TIPO = Tipo;
            Dt_Percepciones_Deducciones = Obj_Perc_Dedu.Consultar_Maxima_Clave();

            if (Dt_Percepciones_Deducciones is DataTable)
            {
                if (Dt_Percepciones_Deducciones.Rows.Count > 0)
                {
                    foreach (DataRow PERCEPCION_DEDUCCION in Dt_Percepciones_Deducciones.Rows)
                    {
                        if (PERCEPCION_DEDUCCION is DataRow)
                        {
                            if (!String.IsNullOrEmpty(PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Clave].ToString().Trim()))
                            {
                                Consecutivo = Convert.ToInt32(PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Clave].ToString().Trim()) + 1;
                                Consecutivo_Clave = String.Format("{0:000}", Consecutivo);
                            }
                        }
                    }
                }
                else Consecutivo_Clave = "001";
            }
            else Consecutivo_Clave = "001";
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar la clave consecutiva. Error: [" + Ex.Message + "]");
        }
        return Consecutivo_Clave;
    }
    /// **************************************************************************************************************************
    /// Nombre: Busqueda_Conceptos
    /// 
    /// Descripción: Ejecuta una busqueda avanzada por los filtros especificados de los conceptos.
    /// 
    /// Parámetros: Tipo.- Este parámetro indica si se tratara de obtener el consecutivo de la clave de percepciones o deducciones.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 4/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// **************************************************************************************************************************
    protected void Busqueda_Conceptos()
    {
        Cls_Cat_Nom_Percepciones_Deducciones_Business Obj_Conceptos = new Cls_Cat_Nom_Percepciones_Deducciones_Business();//Variable de conexion con la capa de negocio.
        DataTable Dt_Conceptos = null;
        Int32 Total_Registros = 1;

        try
        {
            if (!String.IsNullOrEmpty(Txt_Busqueda_Percepciones_Deducciones.Text.Trim()))
                Obj_Conceptos.P_NOMBRE = Txt_Busqueda_Percepciones_Deducciones.Text.Trim();

            if (!String.IsNullOrEmpty(Txt_Clave_Percepcion_Deduccion_Busqueda.Text.Trim()))
                Obj_Conceptos.P_CLAVE = Txt_Clave_Percepcion_Deduccion_Busqueda.Text.Trim();

            if (Cmb_Tipo_Busqueda.SelectedIndex > 0)
                Obj_Conceptos.P_TIPO = Cmb_Tipo_Busqueda.SelectedItem.Text.Trim();

            if (Cmb_Estatus_Busqueda.SelectedIndex > 0)
                Obj_Conceptos.P_ESTATUS = Cmb_Estatus_Busqueda.SelectedItem.Text.Trim();

            if (!String.IsNullOrEmpty(Txt_Nombre_Busqueda.Text.Trim()))
                Obj_Conceptos.P_NOMBRE = Txt_Nombre_Busqueda.Text.Trim();

            if (Cmb_Aplica_Busqueda.SelectedIndex > 0)
                Obj_Conceptos.P_APLICAR = Cmb_Aplica_Busqueda.SelectedItem.Text.Trim();

            if (Cmb_Concepto_Busqueda.SelectedIndex > 0)
                Obj_Conceptos.P_Concepto = Cmb_Concepto_Busqueda.SelectedItem.Text.Trim();

            Dt_Conceptos = Obj_Conceptos.Consultar_Percepciones_Deducciones_General();

            Grid_Percepciones_Deducciones.Columns[1].Visible = true;
            Grid_Percepciones_Deducciones.DataSource = Dt_Conceptos;
            Grid_Percepciones_Deducciones.DataBind();
            Grid_Percepciones_Deducciones.SelectedIndex = -1;
            Grid_Percepciones_Deducciones.Columns[1].Visible = false;

            if (Dt_Conceptos is DataTable)
            {
                Total_Registros = (Dt_Conceptos.Rows.Count == 0) ? Total_Registros : Dt_Conceptos.Rows.Count;
                custPager.TotalPages = ((Total_Registros % Grid_Percepciones_Deducciones.PageSize) == 0) ?
                    (Total_Registros / Grid_Percepciones_Deducciones.PageSize) :
                    (Total_Registros / Grid_Percepciones_Deducciones.PageSize + 1);
            }
            else
            {
                custPager.TotalPages = 1;
            }

            Mpe_Busqueda_Conceptos.Hide();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error generado en el método Busqueda_Conceptos. Error: [" + Ex.Message + "]");
        }
    }
    /// *****************************************************************************************************************
    /// Nombre: Modificar_Percepciones_Deducciones_Tipo_Nomina
    /// 
    /// Descripción: Se modifican las percepciones y deducciones que le aplican al tipo de nómina y al empleado.
    /// 
    /// Parámetros: Percepcion_Deduccion_ID.- Identificador del conceptos a aplicar al empleado y asus tipos de nomina.
    ///             Operacion.- Indica si la operacion a realizar corresponde a un alta o a una baja de algun concepto.
    ///
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 4/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *****************************************************************************************************************
    protected void Modificar_Percepciones_Deducciones_Tipo_Nomina(String Percepcion_Deduccion_ID, String Operacion)
    {
        Cls_Cat_Tipos_Nominas_Negocio Obj_Tipos_Nomina = new Cls_Cat_Tipos_Nominas_Negocio();
        Cls_Cat_Nom_Percepciones_Deducciones_Business Obj_Perc_Dedu = new Cls_Cat_Nom_Percepciones_Deducciones_Business();
        DataTable Dt_Tipos_Nomina = null;
        DataTable Dt_Percepciones = null;
        DataTable Dt_Deducciones = null;

        String TIPO_NOMINA_ID = String.Empty;
        String NOMINA = String.Empty;
        Double DIAS_PRIMA_VACACIONAL_1 = 0;
        Double DIAS_PRIMA_VACACIONAL_2 = 0;
        Double DIAS_AGUINALDO = 0;
        Double DIAS_EXENTA_PRIMA_VACACIONAL = 0;
        Double DIAS_EXENTA_AGUINALDO = 0;
        Double DESPENSA = 0.0;
        String COMENTARIOS = String.Empty;
        String USUARIO_MODIFICO = Cls_Sessiones.Nombre_Empleado;
        String APLICA_ISR = String.Empty;
        String ACTUALIZAR_SALARIO = String.Empty;
        String DIAS_PRIMA_ANTIGUEDAD = String.Empty;

        try
        {
            Dt_Tipos_Nomina = Obj_Tipos_Nomina.Consulta_Datos_Tipo_Nomina();

            if (Dt_Tipos_Nomina is DataTable)
            {
                if (Dt_Tipos_Nomina.Rows.Count > 0)
                {
                    foreach (DataRow TIPOS_NOMINA in Dt_Tipos_Nomina.Rows)
                    {
                        if (TIPOS_NOMINA is DataRow)
                        {
                            if (!String.IsNullOrEmpty(TIPOS_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID].ToString().Trim()))
                            {
                                if (!String.IsNullOrEmpty(TIPOS_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID].ToString().Trim()))
                                    TIPO_NOMINA_ID = TIPOS_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID].ToString().Trim();
                                if (!String.IsNullOrEmpty(TIPOS_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Nomina].ToString().Trim()))
                                    NOMINA = TIPOS_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Nomina].ToString().Trim();
                                if (!String.IsNullOrEmpty(TIPOS_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_1].ToString().Trim()))
                                    DIAS_PRIMA_VACACIONAL_1 = Convert.ToDouble(TIPOS_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_1].ToString().Trim());
                                if (!String.IsNullOrEmpty(TIPOS_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_2].ToString().Trim()))
                                    DIAS_PRIMA_VACACIONAL_2 = Convert.ToDouble(TIPOS_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_2].ToString().Trim());
                                if (!String.IsNullOrEmpty(TIPOS_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Aguinaldo].ToString().Trim()))
                                    DIAS_AGUINALDO = Convert.ToDouble(TIPOS_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Aguinaldo].ToString().Trim());
                                if (!String.IsNullOrEmpty(TIPOS_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Exenta_Prima_Vacacional].ToString().Trim()))
                                    DIAS_EXENTA_PRIMA_VACACIONAL = Convert.ToDouble(TIPOS_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Exenta_Prima_Vacacional].ToString().Trim());
                                if (!String.IsNullOrEmpty(TIPOS_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Exenta_Aguinaldo].ToString().Trim()))
                                    DIAS_EXENTA_AGUINALDO = Convert.ToDouble(TIPOS_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Exenta_Aguinaldo].ToString().Trim());
                                if (!String.IsNullOrEmpty(TIPOS_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Despensa].ToString().Trim()))
                                    DESPENSA = Convert.ToDouble(TIPOS_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Despensa].ToString().Trim());
                                if (!String.IsNullOrEmpty(TIPOS_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Comentarios].ToString().Trim()))
                                    COMENTARIOS = TIPOS_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Comentarios].ToString().Trim();
                                if (!String.IsNullOrEmpty(TIPOS_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Aplica_ISR].ToString().Trim()))
                                    APLICA_ISR = TIPOS_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Aplica_ISR].ToString().Trim();
                                if (!String.IsNullOrEmpty(TIPOS_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Nomina].ToString().Trim()))
                                    ACTUALIZAR_SALARIO = TIPOS_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Actualizar_Salario].ToString().Trim();
                                if (!String.IsNullOrEmpty(TIPOS_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Antiguedad].ToString().Trim()))
                                    DIAS_PRIMA_ANTIGUEDAD = TIPOS_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Antiguedad].ToString().Trim();


                                Obj_Tipos_Nomina.P_Tipo_Nomina_ID = TIPO_NOMINA_ID;
                                Obj_Tipos_Nomina.P_Nomina = NOMINA;
                                Obj_Tipos_Nomina.P_Dias_Prima_Vacacional_1 = DIAS_PRIMA_VACACIONAL_1;
                                Obj_Tipos_Nomina.P_Dias_Prima_Vacacional_2 = DIAS_PRIMA_VACACIONAL_2;
                                Obj_Tipos_Nomina.P_Dias_Aguinaldo = DIAS_AGUINALDO;
                                Obj_Tipos_Nomina.P_Dias_Exenta_Prima_Vacacional = DIAS_EXENTA_PRIMA_VACACIONAL;
                                Obj_Tipos_Nomina.P_Dias_Exenta_Aguinaldo = DIAS_EXENTA_AGUINALDO;
                                Obj_Tipos_Nomina.P_Despensa = DESPENSA;
                                Obj_Tipos_Nomina.P_Comentarios = COMENTARIOS;
                                Obj_Tipos_Nomina.P_Nombre_Usuario = USUARIO_MODIFICO;
                                Obj_Tipos_Nomina.P_Aplica_ISR = APLICA_ISR;
                                Obj_Tipos_Nomina.P_Actualizar_Salario = ACTUALIZAR_SALARIO;
                                Obj_Tipos_Nomina.P_Dias_Prima_Antiguedad = ((String.IsNullOrEmpty(DIAS_PRIMA_ANTIGUEDAD)) ? "0" : DIAS_PRIMA_ANTIGUEDAD);

                                if (Operacion.Trim().ToUpper().Equals("ALTA"))
                                {
                                    Dt_Percepciones = Crear_Tabla_Conceptos(TIPO_NOMINA_ID, "PERCEPCION", Percepcion_Deduccion_ID);
                                    Dt_Deducciones = Crear_Tabla_Conceptos(TIPO_NOMINA_ID, "DEDUCCION", Percepcion_Deduccion_ID);
                                }
                                else if (Operacion.Trim().ToUpper().Equals("BAJA"))
                                {
                                    Dt_Percepciones = Quitar_Concepto(TIPO_NOMINA_ID, "PERCEPCION", Percepcion_Deduccion_ID);
                                    Dt_Deducciones = Quitar_Concepto(TIPO_NOMINA_ID, "DEDUCCION", Percepcion_Deduccion_ID);
                                }

                                Obj_Tipos_Nomina.P_Tipo_Nomina_ID = TIPO_NOMINA_ID;
                                Obj_Tipos_Nomina.P_Percepciones_Nomina = Dt_Percepciones;
                                Obj_Tipos_Nomina.P_Deducciones_Nomina = Dt_Deducciones;

                                Obj_Tipos_Nomina.Modificar_Tipo_Nomina();
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al modificar los conceptos que le pertencen al tipo de nomina. Error: [" + Ex.Message + "]");
        }
    }
    /// *****************************************************************************************************************
    /// Nombre: Crear_Tabla_Conceptos
    /// 
    /// Descripción: Crea la tabla con los conceptos que le aplicaran al empleado y  los tipos de nómina.
    /// 
    /// Parámetros: Tipo_Nomina_ID.- Tipo de nomina a agregar el concepto.
    ///             Tipo.- Indica si el concepto corresponde  a una percepcion a una deduccion.
    ///             Percepcion_Deduccion_ID.-  Es el identificador del concepto a dar de alta.
    ///
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 4/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *****************************************************************************************************************
    protected DataTable Crear_Tabla_Conceptos(String Tipo_Nomina_ID, String Tipo, String Percepcion_Deduccion_ID)
    {
        Cls_Cat_Nom_Percepciones_Deducciones_Business Obj_Percepciones_Deducciones = new Cls_Cat_Nom_Percepciones_Deducciones_Business();
        DataTable Dt_Percepciones_Deducciones = null;

        try
        {
            Obj_Percepciones_Deducciones.P_Tipo_Nomina_ID = Tipo_Nomina_ID;
            Obj_Percepciones_Deducciones.P_TIPO = Tipo;
            Dt_Percepciones_Deducciones = Obj_Percepciones_Deducciones.Consultar_Percepciones_Deducciones_Tipo_Nomina();

            if (Cmb_Tipo_Deduccion_Percepcion.SelectedItem.Text.Trim().ToUpper().Equals(Tipo))
            {
                //Agregamos el nuevo concepto agregado.
                DataRow NUEVA_Percepcion_Deduccion = Dt_Percepciones_Deducciones.NewRow();
                NUEVA_Percepcion_Deduccion[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID] = Percepcion_Deduccion_ID;
                NUEVA_Percepcion_Deduccion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad] = "0.00";
                NUEVA_Percepcion_Deduccion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Aplica_Todos] = "SI";
                Dt_Percepciones_Deducciones.Rows.Add(NUEVA_Percepcion_Deduccion);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al crear la tabla de conceptos. Error: [" + Ex.Message + "]");
        }
        return Dt_Percepciones_Deducciones;
    }
    /// *****************************************************************************************************************
    /// Nombre: Quitar_Concepto
    /// 
    /// Descripción: Crea la tabla con los conceptos que le aplicaran al empleado y  los tipos de nómina.
    /// 
    /// Parámetros: Tipo_Nomina_ID.- Tipo de nomina a agregar el concepto.
    ///             Tipo.- Indica si el concepto corresponde  a una percepcion a una deduccion.
    ///             Percepcion_Deduccion_ID.-  Es el identificador del concepto a dar de alta.
    ///
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 4/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *****************************************************************************************************************
    protected DataTable Quitar_Concepto(String Tipo_Nomina_ID, String Tipo, String Percepcion_Deduccion_ID)
    {
        Cls_Cat_Nom_Percepciones_Deducciones_Business Obj_Percepciones_Deducciones = new Cls_Cat_Nom_Percepciones_Deducciones_Business();
        DataTable Dt_Percepciones_Deducciones = null;

        try
        {
            Obj_Percepciones_Deducciones.P_Tipo_Nomina_ID = Tipo_Nomina_ID;
            Obj_Percepciones_Deducciones.P_TIPO = Tipo;
            Dt_Percepciones_Deducciones = Obj_Percepciones_Deducciones.Consultar_Percepciones_Deducciones_Tipo_Nomina();

            if (Cmb_Tipo_Deduccion_Percepcion.SelectedItem.Text.Trim().ToUpper().Equals(Tipo))
            {
                DataRow[] Filas = Dt_Percepciones_Deducciones.Select(Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + "=" + Percepcion_Deduccion_ID);

                if (Filas.Length > 0)
                {
                    Dt_Percepciones_Deducciones.Rows.Remove(Filas[0]);
                    Dt_Percepciones_Deducciones.AcceptChanges();
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al crear la tabla de conceptos. Error: [" + Ex.Message + "]");
        }
        return Dt_Percepciones_Deducciones;
    }
    #endregion

    #region OPERACIONES ALTA - ACTUALIZAR - BAJA
    ///******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Alta
    /// DESCRIPCIÓN: Se ejecuta el Alta de un Percepcion Deduccion
    /// CREO: Juan Alberto Hernandez Negrete
    /// FECHA_CREO: 11/Septiembre/2010 9:30 a.m.
    /// MODIFICO:
    /// FECHA_MODIFICO
    /// CAUSA_MODIFICACIÓN   
    ///******************************************************************************/
    private Boolean Alta()
    {
        Cls_Cat_Nom_Percepciones_Deducciones_Business Cat_Percepciones_Deducciones_Opcionales = new Cls_Cat_Nom_Percepciones_Deducciones_Business();
        Boolean Operacion_Completa = true;

        try
        {
            //Datos Generales para dar de alta la percepción deducción
            Cat_Percepciones_Deducciones_Opcionales.P_NOMBRE = HttpUtility.HtmlDecode(Txt_Nombre.Text.Trim());
            Cat_Percepciones_Deducciones_Opcionales.P_ESTATUS = HttpUtility.HtmlDecode(Cmb_Estatus.SelectedItem.Text.Trim());
            Cat_Percepciones_Deducciones_Opcionales.P_TIPO = HttpUtility.HtmlDecode(Cmb_Tipo_Deduccion_Percepcion.SelectedItem.Text.Trim());
            Cat_Percepciones_Deducciones_Opcionales.P_APLICAR = HttpUtility.HtmlDecode(Cmb_Aplicar.SelectedItem.Text.Trim());
            Cat_Percepciones_Deducciones_Opcionales.P_TIPO_ASIGNACION = HttpUtility.HtmlDecode(Cmb_Asignar.SelectedItem.Text.Trim());
            Cat_Percepciones_Deducciones_Opcionales.P_Concepto = HttpUtility.HtmlDecode(Cmb_Concepto.SelectedItem.Text.Trim());
            Cat_Percepciones_Deducciones_Opcionales.P_GRAVABLE = (Chk_Gravable.Checked ? 1 : 0);
            Cat_Percepciones_Deducciones_Opcionales.P_APLICA_IMSS = (Chk_Aplica_Concepto_Calculo_IMSS.Checked) ? "SI" : "NO";
            Cat_Percepciones_Deducciones_Opcionales.P_CLAVE = Txt_Clave_Concepto.Text.Trim();
            Cat_Percepciones_Deducciones_Opcionales.P_CUENTA_CONTABLE_ID = Cmb_Cuentas_Contables.SelectedValue.Trim();

            if (Chk_Gravable.Checked)
            {
                if (!Txt_Cantidad_Porcentual_Gravable.Text.Equals(""))
                {
                    Cat_Percepciones_Deducciones_Opcionales.P_PORCENTAJE_GRAVABLE = Convert.ToDouble(HttpUtility.HtmlDecode(Txt_Cantidad_Porcentual_Gravable.Text.Trim()));
                }
                else
                {
                    Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode("<b>+</b> " +
                        "No se ha ingresado ningún valor gravante");
                    Div_Contenedor_Msj_Error.Visible = true;
                    return false;
                }
            }
            //Continuar con datos generales
            Cat_Percepciones_Deducciones_Opcionales.P_COMENTARIOS = HttpUtility.HtmlDecode(Txt_Comentarios.Text.Trim());
            Cat_Percepciones_Deducciones_Opcionales.P_USUARIO_CREO = HttpUtility.HtmlDecode(((String)Cls_Sessiones.Nombre_Empleado));
            //Operacion_Completa = Cat_Percepciones_Deducciones_Opcionales.Alta_Percepcion_Deduccion();

            String Identificador = Cat_Percepciones_Deducciones_Opcionales.Alta_Percepcion_Deduccion();

            if (!String.IsNullOrEmpty(Identificador))
            {
                if (Cat_Percepciones_Deducciones_Opcionales.P_Concepto.Trim().ToUpper().Equals("TIPO_NOMINA"))
                {
                    Modificar_Percepciones_Deducciones_Tipo_Nomina(Identificador, "ALTA");
                }

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operacion Exitosa [Alta Percepción Deducción]');", true);
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode("<b>+</b> " + ex.Message);
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Operacion_Completa;

    }//Fin del Metodo de Alta de percepcion Deduccion
    ///******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Actualizar
    /// DESCRIPCIÓN: Se ejecuta la Actualizacion de la Percepcion Deduccion seleccionada
    /// CREO: Juan Alberto Hernandez Negrete
    /// FECHA_CREO: 18/Septiembre/2010 9:38 a.m.
    /// MODIFICO:
    /// FECHA_MODIFICO
    /// CAUSA_MODIFICACIÓN   
    ///******************************************************************************/
    private Boolean Actualizar()
    {
        Cls_Cat_Nom_Percepciones_Deducciones_Business Cat_Percepciones_Deducciones_Opcionales = new Cls_Cat_Nom_Percepciones_Deducciones_Business();
        Boolean Operacion_Completa = true;

        try
        {
            //Datos Generales para dar de alta la percepción deducción
            Cat_Percepciones_Deducciones_Opcionales.P_PERCEPCION_DEDUCCION_ID = HttpUtility.HtmlDecode(Txt_Percepcion_Deduccion_ID.Text.Trim());
            Cat_Percepciones_Deducciones_Opcionales.P_NOMBRE = HttpUtility.HtmlDecode(Txt_Nombre.Text.Trim());
            Cat_Percepciones_Deducciones_Opcionales.P_ESTATUS = HttpUtility.HtmlDecode(Cmb_Estatus.SelectedItem.Text.Trim());
            Cat_Percepciones_Deducciones_Opcionales.P_TIPO = HttpUtility.HtmlDecode(Cmb_Tipo_Deduccion_Percepcion.SelectedItem.Text.Trim());
            Cat_Percepciones_Deducciones_Opcionales.P_APLICAR = HttpUtility.HtmlDecode(Cmb_Aplicar.SelectedItem.Text.Trim());
            Cat_Percepciones_Deducciones_Opcionales.P_TIPO_ASIGNACION = HttpUtility.HtmlDecode(Cmb_Asignar.SelectedItem.Text.Trim());
            Cat_Percepciones_Deducciones_Opcionales.P_Concepto = HttpUtility.HtmlDecode(Cmb_Concepto.SelectedItem.Text.Trim());
            Cat_Percepciones_Deducciones_Opcionales.P_GRAVABLE = (Chk_Gravable.Checked ? 1 : 0);
            Cat_Percepciones_Deducciones_Opcionales.P_APLICA_IMSS = (Chk_Aplica_Concepto_Calculo_IMSS.Checked) ? "SI" : "NO";
            Cat_Percepciones_Deducciones_Opcionales.P_CLAVE = Txt_Clave_Concepto.Text.Trim();
            Cat_Percepciones_Deducciones_Opcionales.P_CUENTA_CONTABLE_ID = Cmb_Cuentas_Contables.SelectedValue.Trim();

            if (Chk_Gravable.Checked)
            {
                if (!Txt_Cantidad_Porcentual_Gravable.Text.Equals(""))
                {
                    Cat_Percepciones_Deducciones_Opcionales.P_PORCENTAJE_GRAVABLE = Convert.ToDouble(HttpUtility.HtmlDecode(Txt_Cantidad_Porcentual_Gravable.Text.Trim()));
                }
                else
                {
                    Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode("<b>+</b> " +
                        "No se ha ingresado ningún valor gravante");
                    Div_Contenedor_Msj_Error.Visible = true;
                    return false;
                }
            }

            //Continuar con datos generales
            Cat_Percepciones_Deducciones_Opcionales.P_COMENTARIOS = HttpUtility.HtmlDecode(Txt_Comentarios.Text.Trim());
            Cat_Percepciones_Deducciones_Opcionales.P_USUARIO_MODIFICO = HttpUtility.HtmlDecode(((String)Cls_Sessiones.Nombre_Empleado));

            //Operacion_Completa = Cat_Percepciones_Deducciones_Opcionales.Actualizar_Percepcion_Deduccion();
            String Identificador = Cat_Percepciones_Deducciones_Opcionales.Actualizar_Percepcion_Deduccion();

            if (!String.IsNullOrEmpty(Identificador))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operacion Exitosa [Actualizar Percepción Deducción]');", true);
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode("<b>+</b> " + ex.Message);
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Operacion_Completa;
    }
    ///******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Baja
    /// DESCRIPCIÓN: Se elimina el registro seleccionado
    /// CREO: Juan Alberto Hernandez Negrete
    /// FECHA_CREO: 18/Septiembre/2010 9:38 a.m.
    /// MODIFICO:
    /// FECHA_MODIFICO
    /// CAUSA_MODIFICACIÓN   
    ///******************************************************************************
    private void Baja()
    {
        Cls_Cat_Nom_Percepciones_Deducciones_Business Cat_Percepciones_Deducciones_Opcionales = new Cls_Cat_Nom_Percepciones_Deducciones_Business();
        Boolean Operacion_Completa = true;
        try
        {
            Cat_Percepciones_Deducciones_Opcionales.P_PERCEPCION_DEDUCCION_ID = Txt_Percepcion_Deduccion_ID.Text.Trim();
            //Operacion_Completa = Cat_Percepciones_Deducciones_Opcionales.Baja_Percepcion_Deduccion();

            //if (Cmb_Concepto.SelectedItem.Text.Trim().ToUpper().Equals("TIPO_NOMINA"))
            //{
            //    Modificar_Percepciones_Deducciones_Tipo_Nomina(Txt_Percepcion_Deduccion_ID.Text.Trim(), "BAJA");
            //}

            String Identificador = Cat_Percepciones_Deducciones_Opcionales.Baja_Percepcion_Deduccion();

            if (!String.IsNullOrEmpty(Identificador))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operacion Exitosa [Eliminar Percepción Deducción]');", true);
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode("<b>+</b> " + ex.Message);
            Div_Contenedor_Msj_Error.Visible = true;
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
            Botones.Add(Btn_Buscar_Percepcion_Deduccion);

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

    #region (Cuenta Contable)
    /// ***********************************************************************************************
    /// Nombre: Consultar_Cuentas_Contables
    /// 
    /// Descripción: Consultar las cuentas contables.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creo: 09/Noviembre/2011.
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ***********************************************************************************************
    private void Consultar_Cuentas_Contables()
    {
        Cls_Cat_Nom_Percepciones_Deducciones_Business Obj_Cuentas_Contables = new Cls_Cat_Nom_Percepciones_Deducciones_Business();//Variable de conexión con la capa de negocios.
        DataTable Dt_Cuentas_Contables = null;//Variable que almacenara un listado de cuentas contables.

        try
        {
            Dt_Cuentas_Contables = Obj_Cuentas_Contables.Consultar_Cuentas_Contables();
            Cmb_Cuentas_Contables.DataSource = Dt_Cuentas_Contables;
            Cmb_Cuentas_Contables.DataTextField = "CUENTA_CONTABLE";
            Cmb_Cuentas_Contables.DataValueField = Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID;
            Cmb_Cuentas_Contables.DataBind();

            Cmb_Cuentas_Contables.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Cuentas_Contables.SelectedIndex = (-1);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las cuentas contables. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #endregion

    #region EVENTOS

    #region Botones [ ALTA - MODIFICAR - BAJA - BUSQUEDA ]
    ///******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    /// DESCRIPCIÓN: Este Boton tiene 2 funcionalidades:
    /// I.- Habilita al usuario para pueda ingresar toda la informacion necesaria 
    /// para dar el alta de una nueva percepcion deduccion.
    /// II.- Habilita al usuario a ejecutar el alta de la percepcion deduccion. 
    /// CREO: Juan Alberto Hernandez Negrete
    /// FECHA_CREO: 22/Agosto/2010 14:20 p.m.
    /// MODIFICO:
    /// FECHA_MODIFICO
    /// CAUSA_MODIFICACIÓN   
    ///******************************************************************************
    protected void Btn_Nuevo_Click(object sender, EventArgs e) {
        try {
            //Verificar el texto del boton
            if (Btn_Nuevo.ToolTip.Equals("Nuevo")) {
                Habilitar_Controles("Nuevo"); //Habilita los controles necesarios para poder capturar los datos del rol a dar de alta
                Limpiar_Controles();         //Limpia los controles de la forma para la operacion a realizar por parte del usuario               
                Configuracion_Btn_Nuevo();
            } else {
                //Verificar si hay valores en la caja de texto
                if (!(Txt_Nombre.Text.Trim().Equals(""))) {
                    if (Cmb_Tipo_Deduccion_Percepcion.SelectedIndex > 0) {
                        if (Cmb_Aplicar.SelectedIndex > 0) {
                            if (Cmb_Asignar.SelectedIndex > 0)
                            {
                                if (Alta())
                                {
                                    Estado_Inicial();
                                    Habilitar_Controles("Inicial");
                                    Limpiar_Controles();
                                }
                            }
                            else
                            {
                                Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode("<b>+</b> " +
                                    "No se ha seleccionado, ningún tipo de asignación, favor de seleccionarlo");
                                Div_Contenedor_Msj_Error.Visible = true;
                            }
                        } else {
                            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode("<b>+</b> " +
                                "No se ha seleccionado ningún la opción de Aplicar, favor de seleccionarlo");
                            Div_Contenedor_Msj_Error.Visible = true;
                        }
                    } else {
                        Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode("<b>+</b> " +
                            "No se ha seleccionado ningún tipo, favor de seleccionarlo");
                        Div_Contenedor_Msj_Error.Visible = true;
                    }//End Else
                } else {
                    Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode("<b>+</b> " +
                        "El Nombre es necesario para realizar la operación de alta.");
                    Div_Contenedor_Msj_Error.Visible = true;
                }//End Else
            }//End Else
        } catch (Exception ex) {
            Lbl_Mensaje_Error.Text = "<b>+</b> Código : [" + ex.Message + "]";
            Div_Contenedor_Msj_Error.Visible = true;
        }//End Catch
    }
    ///******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    /// DESCRIPCIÓN: Este Boton tiene 2 funcionalidades:
    /// I.- Habilita al usuario para pueda ingresar toda la informacion necesaria 
    /// para realizar la actualizacion de una percepcion deduccion existente
    /// II.- Habilita al usuario a ejecutar la actualizacion de la percepcion deduccion. 
    /// CREO: Juan Alberto Hernandez Negrete
    /// FECHA_CREO: 22/Agosto/2010 14:20 p.m.
    /// MODIFICO:
    /// FECHA_MODIFICO
    /// CAUSA_MODIFICACIÓN   
    ///******************************************************************************
    protected void Btn_Modificar_Click(object sender, EventArgs e) {
        try {
            //Verificar el texto del boton
            if (Btn_Modificar.ToolTip.Equals("Modificar")) {
                if (Grid_Percepciones_Deducciones.SelectedIndex != -1) {
                    Habilitar_Controles("Modificar"); //Habilita los controles necesarios para poder capturar los datos del rol a dar de alta                  
                } else {
                    Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode("<b>+</b> " +
                        "No se ha seleccionado ningún elemento de la tabla de Percepciones Deducciones");
                    Div_Contenedor_Msj_Error.Visible = true;                    
                }
            } else {
                //Verificar si hay valores en la caja de texto
                if (!(Txt_Nombre.Text.Trim().Equals(""))) {
                    if (Cmb_Tipo_Deduccion_Percepcion.SelectedIndex > 0) {
                        if (Cmb_Aplicar.SelectedIndex > 0) {
                            if (Cmb_Asignar.SelectedIndex > 0)
                            {
                                if (Actualizar())
                                {
                                    Estado_Inicial();
                                    Habilitar_Controles("Inicial");
                                    Limpiar_Controles();
                                }
                            }
                            else
                            {
                                Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode("<b>+</b> " +
                                    "No se ha seleccionado, ningún tipo de asignación, favor de seleccionarlo");
                                Div_Contenedor_Msj_Error.Visible = true;
                            }
                        } else {
                            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode("<b>+</b> " +
                                "No se ha seleccionado ningún la opción de Aplicar, favor de seleccionarlo");
                            Div_Contenedor_Msj_Error.Visible = true;
                        }
                    } else {
                        Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode("<b>+</b> " +
                            "No se ha seleccionado ningún tipo, favor de seleccionarlo");
                        Div_Contenedor_Msj_Error.Visible = true;
                    }//End Else
                } else {
                    Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode("<b>+</b> " +
                        "El Nombre es necesario para realizar la operación de alta.");
                    Div_Contenedor_Msj_Error.Visible = true;
                }//End Else
            }//End Else
        } catch (Exception ex) {
            Lbl_Mensaje_Error.Text = "<b>+</b> Código : [" + ex.Message + "]";
            Div_Contenedor_Msj_Error.Visible = true;
        }//End Catch
    }
    ///******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
    /// DESCRIPCIÓN: Este Boton tiene 2 funcionalidades:
    /// I.- Habilita al usuario para realizar la baja de una percepcion deduccion.
    /// CREO: Juan Alberto Hernandez Negrete
    /// FECHA_CREO: 22/Agosto/2010 14:20 p.m.
    /// MODIFICO:
    /// FECHA_MODIFICO
    /// CAUSA_MODIFICACIÓN   
    ///******************************************************************************
    protected void Btn_Eliminar_Click(object sender, EventArgs e) {
        try {
            //Verificar si se ha seleccionado un elemento
            if (Grid_Percepciones_Deducciones.SelectedIndex >= 0) {
                //Aqui se realizara la baja de la percepcion deduccion seleccionada
                Baja();
                Estado_Inicial();
                Habilitar_Controles("Inicial");
                Limpiar_Controles();
            } else {
                Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode("<b>+</b> " +
                    "No se ha seleccionado ninguna Percepción Deducción a Eliminar. Por favor seleccione la que desea eliminar.");
                Div_Contenedor_Msj_Error.Visible = true;
            }
        } catch (Exception ex) {
            Lbl_Mensaje_Error.Text = "<b>+</b> Código : [" + ex.Message + "]";
            Div_Contenedor_Msj_Error.Visible = true;
        }//End Catch
    }//End Function

    ///******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    /// DESCRIPCIÓN: Este Boton tiene 2 funcionalidades:
    /// I.- Habilita al usuario para pueda cancelar la operacion que se esta
    /// realizando actualmente.
    /// II.- Habilita al usuario a salir a la pagina principal del sistema.
    /// CREO: Juan Alberto Hernandez Negrete
    /// FECHA_CREO: 22/Agosto/2010 14:20 p.m.
    /// MODIFICO:
    /// FECHA_MODIFICO
    /// CAUSA_MODIFICACIÓN   
    ///******************************************************************************
    protected void Btn_Salir_Click(object sender, EventArgs e) {
        //Verificar el texto del boton
        if (Btn_Salir.ToolTip.Equals("Salir")) {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        } else {
            Habilitar_Controles("Inicial");
            Configuracion_Btn_Cancelar();
            Limpiar_Controles();
        }//End Else
    }//End Function
    ///******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Btn_Buscar_Percepcion_Deduccion_Click
    /// DESCRIPCIÓN: Realizar búsqueda de la percepccion deduccion por el nombre ingresado.
    /// CREO: Juan Alberto Hernandez Negrete
    /// FECHA_CREO: 22/Agosto/2010 14:20 p.m.
    /// MODIFICO:
    /// FECHA_MODIFICO
    /// CAUSA_MODIFICACIÓN   
    ///******************************************************************************
    protected void Btn_Buscar_Percepcion_Deduccion_Click(object sender, EventArgs e) {
        try {
            Habilitar_Controles("Inicial");
            Configuracion_Btn_Cancelar();
            Limpiar_Controles();
            Grid_Percepciones_Deducciones.SelectedIndex = -1;

            Busqueda_Conceptos();

            if ( Grid_Percepciones_Deducciones.Rows.Count == 0 ){
                Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode("<b>+</b> " +
                    "No se encontraron registros con ese nombre");
                Div_Contenedor_Msj_Error.Visible = true;
            }
        } catch (Exception ex) {
            Lbl_Mensaje_Error.Text = "<b>+</b> Error al consultar la tabla de Percepciones y Deducciones. Error: [" + ex.Message + "]";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }//End Function
    protected void Btn_Busqueda_Percepciones_Deducciones_Click(object sender, EventArgs e)
    {
        try
        {
            Busqueda_Conceptos();
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = "<b>+</b> Código : [" + Ex.Message + "]";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    #endregion

    #region EVENTO TXT_CANTIDAD_PORCENTAJE [ VALIDAR FORMATO ] 
    ///*********************************************************************************
    /// NOMBRE : Txt_Cantidad_Porcentual_Gravable_TextChanged
    /// DESCRIPCIÓN: Realiza la validacion del porcentaje gravable ingresado,
    /// tanto en rango como en su formato.
    /// CREO: Juan Alberto Hernández Negrete
    /// FECHA CREO:20 de Septiembre del 2010
    /// MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACION:
    ///*********************************************************************************
    protected void Txt_Cantidad_Porcentual_Gravable_TextChanged(Object sender, System.EventArgs e) {
        Double value = 0;
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;

        if (!Txt_Cantidad_Porcentual_Gravable.Text.Equals("")) {
            try {
                value = Convert.ToDouble(Txt_Cantidad_Porcentual_Gravable.Text);
                if (value < 0 || value > 100) {
                    Txt_Cantidad_Porcentual_Gravable.Text = "";
                    Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode("<b>+</b> El Porcentaje Gravable no puede ser mayor a 100 o menor a 0");
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            } catch (Exception ex) {
                Txt_Cantidad_Porcentual_Gravable.Text = "";

                Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode("<b>+</b> El formato del Porcentaje Gravable es incorrecto, ingrese un dato válido. Rango [0-1]. Error: [" + ex.Message + "]");
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    }
    #endregion

    #region (Combos)
    ///******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Cmb_Tipo_Deduccion_Percepcion_SelectedIndexChanged
    /// DESCRIPCIÓN: Evento que se genera al seleccionar un elemento del combo y  habilita
    /// una configuracion de acuerdo con la opcion seleccionada, ya sea percepcion o deduccion.
    /// CREO: Juan Alberto Hernandez Negrete
    /// FECHA_CREO: 09/Septiembre/2010 14:20 p.m.
    /// MODIFICO:
    /// FECHA_MODIFICO
    /// CAUSA_MODIFICACIÓN   
    ///******************************************************************************
    protected void Cmb_Tipo_Deduccion_Percepcion_SelectedIndexChanged(object sender, EventArgs e)
    {
        String Dato_Seleccionado = Cmb_Tipo_Deduccion_Percepcion.SelectedItem.Text;
        if (Cmb_Tipo_Deduccion_Percepcion.SelectedIndex > 0)
        {
            if (Dato_Seleccionado.Equals("PERCEPCION"))
            {
                Chk_Gravable.Enabled = true;
                Chk_Aplica_Concepto_Calculo_IMSS.Enabled = true;
                Txt_Clave_Concepto.Text = "P-" + Generar_Clave_Consecutiva_Percepcion_Deduccion("PERCEPCION");
            }
            else if (Dato_Seleccionado.Equals("DEDUCCION"))
            {
                Chk_Gravable.Enabled = false;
                Chk_Gravable.Checked = false;
                Txt_Cantidad_Porcentual_Gravable.Enabled = false;
                Txt_Cantidad_Porcentual_Gravable.Text = "";
                TWM_Txt_Cantidad_Porcentual_Gravable.WatermarkText = "NO APLICA";
                TWM_Txt_Cantidad_Porcentual_Gravable.WatermarkCssClass = "watermarked2";
                Txt_Clave_Concepto.Text = "D-" + Generar_Clave_Consecutiva_Percepcion_Deduccion("DEDUCCION");
            }
        }
        else Txt_Clave_Concepto.Text = String.Empty;
    }
    ///******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Cmb_Tipo_Deduccion_Percepcion_SelectedIndexChanged
    /// DESCRIPCIÓN: Evento que se genera al seleccionar un elemento del combo y  habilita
    /// una configuracion de acuerdo con la opcion seleccionada, ya sea percepcion o deduccion.
    /// CREO: Juan Alberto Hernandez Negrete
    /// FECHA_CREO: 09/Septiembre/2010 14:20 p.m.
    /// MODIFICO:
    /// FECHA_MODIFICO
    /// CAUSA_MODIFICACIÓN   
    ///******************************************************************************
    protected void Cmb_Concepto_SelectedIndexChanged(object sender, EventArgs e)
    {
        String Concepto = "";//Tipo de concepto seleccionado para la percepcion y/o deduccion.
        try
        {
            Concepto = Cmb_Concepto.SelectedItem.Text.Trim();
            Consultar_Tipo_Asignacion_Por_Concepto(Concepto);
            Cmb_Asignar.SelectedIndex = -1;
            Cmb_Asignar.Enabled = true;
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = "<b>+</b> Error al seleccionar un concepto. Error: [" + Ex.Message + "]";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    ///******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Cmb_Asignar_SelectedIndexChanged
    /// DESCRIPCIÓN: Habilita o Deshabilita la opcion para ingresar el porcentaje gravable.
    /// CREO: Juan Alberto Hernandez Negrete
    /// FECHA_CREO: 09/Septiembre/2010 14:20 p.m.
    /// MODIFICO:
    /// FECHA_MODIFICO
    /// CAUSA_MODIFICACIÓN   
    ///******************************************************************************
    protected void Cmb_Asignar_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Cmb_Asignar.SelectedItem.Text.Trim().Equals("OPERACION"))
            {
                Txt_Cantidad_Porcentual_Gravable.Enabled = false;
                Chk_Gravable.Checked = false;
                Chk_Gravable.Enabled = false;
                Txt_Cantidad_Porcentual_Gravable.Text = "";
                TWM_Txt_Cantidad_Porcentual_Gravable.WatermarkText = "NO APLICA";
                TWM_Txt_Cantidad_Porcentual_Gravable.WatermarkCssClass = "watermarked2";
            }
            else
            {
                String Dato_Seleccionado = Cmb_Tipo_Deduccion_Percepcion.SelectedItem.Text;

                if (Dato_Seleccionado.Equals("PERCEPCION"))
                {
                    Chk_Gravable.Enabled = true;
                }
                else if (Dato_Seleccionado.Equals("DEDUCCION"))
                {
                    Chk_Gravable.Enabled = false;
                    Chk_Gravable.Checked = false;
                    Txt_Cantidad_Porcentual_Gravable.Enabled = false;
                    Txt_Cantidad_Porcentual_Gravable.Text = "";
                    TWM_Txt_Cantidad_Porcentual_Gravable.WatermarkText = "NO APLICA";
                    TWM_Txt_Cantidad_Porcentual_Gravable.WatermarkCssClass = "watermarked2";
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = "<b>+</b> Error al seleccionar el tipo de asignación. Error: [" + Ex.Message + "]";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    protected void custPager_PageChanged(object sender, CustomPageChangeArgs e)
    {
        Grid_Percepciones_Deducciones.PageSize = e.CurrentPageSize;
        Grid_Percepciones_Deducciones.PageIndex = e.CurrentPageNumber;
        Consulta_Percepciones_Deducciones();
    }
    #endregion

    #region (CheckBox)
    ///******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Chk_Gravable_CheckedChanged
    /// DESCRIPCIÓN: Evento que se genera al seleccionar el checkbox.
    /// CREO: Juan Alberto Hernandez Negrete
    /// FECHA_CREO: 09/Septiembre/2010 14:20 p.m.
    /// MODIFICO:
    /// FECHA_MODIFICO
    /// CAUSA_MODIFICACIÓN   
    ///******************************************************************************
    protected void Chk_Gravable_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_Gravable.Checked)
        {
            Txt_Cantidad_Porcentual_Gravable.Enabled = true;
            TWM_Txt_Cantidad_Porcentual_Gravable.WatermarkText = "APLICA";
            TWM_Txt_Cantidad_Porcentual_Gravable.WatermarkCssClass = "watermarked";
        }
        else
        {
            Txt_Cantidad_Porcentual_Gravable.Enabled = false;
            Txt_Cantidad_Porcentual_Gravable.Text = "";
            TWM_Txt_Cantidad_Porcentual_Gravable.WatermarkText = "NO APLICA";
            TWM_Txt_Cantidad_Porcentual_Gravable.WatermarkCssClass = "watermarked2";
        }
    }
    #endregion 

    #endregion

}
