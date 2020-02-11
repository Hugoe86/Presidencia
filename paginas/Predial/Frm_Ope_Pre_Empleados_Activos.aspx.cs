using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Presidencia.Sessiones;
using Presidencia.Operacion_Predial_Empleados_Activos.Negocio;
using Presidencia.Constantes;

public partial class paginas_Predial_Frm_Ope_Pre_Empleados_Activos : System.Web.UI.Page
{
    #region Variables
    private const int Const_Estado_Inicial = 0;
    private const int Const_Estado_Nuevo = 1;
    private const int Const_Estado_Modificar = 2;
    private const int Const_Estado_Buscar = 3;
    //private static DataTable Dt_Empleados = new DataTable();
    private static DataTable Dt_Movimiento = new DataTable();
    private static string M_Busqueda = "";
    public static Boolean BL_ESTADO = true;
    #endregion

    #region Page Load / Init
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            //if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!Page.IsPostBack)
            {                
                Estado_Botones(Const_Estado_Inicial);                
                Consulta_Lista_Empleados();
                Cargar_Grid_Empleados_Activos(0);
            }
            Mensaje_Error();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    private void Consulta_Lista_Empleados()
    {
        
    }
    #endregion

    #region Metodos
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Grid
    ///DESCRIPCIÓN: Realizar la consulta y llenar el grido con estos datos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/03/2011 12:14:35 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Grid_Empleados_Activos(int Page_Index)
    {
        DataTable Dt_Empleados = new DataTable();
        Cls_Ope_Pre_Empleado_Activos_Negocio Empleados_Negocio = new Cls_Ope_Pre_Empleado_Activos_Negocio();
        if (Session["Empleados_Activos_Modificados"] != null)
        Dt_Empleados = (DataTable)Session["Empleados_Activos_Modificados"];
        try
        {
            if (Session["Empleados_Activos_Modificados"] != null)
            {                
                Grid_Empleados_Activos.PageIndex = Page_Index;
                Grid_Empleados_Activos.DataSource = Dt_Empleados;
                Grid_Empleados_Activos.DataBind();
            }
            else

            {
                Empleados_Negocio.P_Empleado_Nombre = M_Busqueda;
                Dt_Empleados = Empleados_Negocio.Consulta_Empleados_Activos();

                if (Dt_Empleados.Rows.Count > 0)
                {
                    Session["Empleados_Activos"] = Dt_Empleados;
                    Grid_Empleados_Activos.PageIndex = Page_Index;
                    Grid_Empleados_Activos.DataSource = Dt_Empleados;
                    Grid_Empleados_Activos.DataBind();
                }
                else
                {
                    Grid_Empleados_Activos.DataSource = null;
                    Grid_Empleados_Activos.DataBind();
                    Mensaje_Error("No se encontraron Empleados registrados en esta Área");
                }
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Grid_Movimientos
    ///DESCRIPCIÓN: Realizar la consulta y llenar el grido con estos datos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/03/2011 12:14:35 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Grid_Movimientos(int Page_Index, String ID)
    {
        try
        {
            if (!Txt_No_Empleado.Text.Trim().Equals(String.Empty))
            {
                Cls_Ope_Pre_Empleado_Activos_Negocio Empleados_Negocio = new Cls_Ope_Pre_Empleado_Activos_Negocio();
                Empleados_Negocio.P_Empleado_ID = ID;
                Dt_Movimiento = Empleados_Negocio.Consulta_Movimientos();

                if (Dt_Movimiento.Rows.Count > 0)
                {
                    Div_Pendientes_Titulo.Visible = true;
                    Grid_Movimientos.PageIndex = Page_Index;
                    Grid_Movimientos.DataSource = Dt_Movimiento;
                    Grid_Movimientos.DataBind();
                }
                else
                {
                    Div_Pendientes_Titulo.Visible = false;
                    Grid_Movimientos.PageIndex = Page_Index;
                    Grid_Movimientos.DataSource = null;
                    Grid_Movimientos.DataBind();
                    Mensaje_Error("El Empleado " + Txt_Nombre_Empleado.Text.Trim() + " no tiene Pendientes asignados");
                    Div_Reasignacion.Visible = false;                    
                }
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA METODO: LLenar_Combo_Id
    ///        DESCRIPCIÓN: llena todos los combos
    ///         PARAMETROS: 1.- Obj_DropDownList: Combo a llenar
    ///                     2.- Dt_Temporal: DataTable genarada por una consulta a la base de datos
    ///                     3.- Texto: nombre de la columna del dataTable que mostrara el texto en el combo
    ///                     3.- Valor: nombre de la columna del dataTable que mostrara el valor en el combo
    ///                     3.- Seleccion: Id del combo el cual aparecera como seleccionado por default
    ///               CREO: Jesus S. Toledo Rdz.
    ///         FECHA_CREO: 06/9/2010
    ///           MODIFICO:
    ///     FECHA_MODIFICO:
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_ID(DropDownList Obj_DropDownList, DataTable Dt_Temporal, String Texto, String Valor, String Seleccion)
    {
        try
        {
            Obj_DropDownList.Items.Clear();
            Obj_DropDownList.Items.Add(new ListItem("<SELECCIONE>", "0"));
            foreach (DataRow row in Dt_Temporal.Rows)
            {
                Obj_DropDownList.Items.Add(new ListItem(row[Texto].ToString(), row[Valor].ToString()));
            }
            Obj_DropDownList.SelectedValue = Seleccion;
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }
    public void Llenar_Combo_ID(DropDownList Obj_DropDownList)
    {
        try
        {
            Obj_DropDownList.Items.Clear();
            Obj_DropDownList.Items.Add(new ListItem("<SELECCIONE>", "0"));
            Obj_DropDownList.SelectedValue = "0";
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Mensaje_Error
    ///DESCRIPCION : Muestra el error
    ///PARAMETROS  : P_Texto: texto de un TextBox
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 04-Septiembre-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Mensaje_Error(String P_Mensaje)
    {
        Img_Error.Visible = true;
        Lbl_Error.Text += P_Mensaje + "</br>";
    }
    private void Mensaje_Error()
    {
        Img_Error.Visible = false;
        Lbl_Error.Text = "";
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Estado_Botones
    ///DESCRIPCIÓN: Metodo para establecer el estado de los botones y componentes del formulario
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/02/2011 05:49:53 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Estado_Botones(int P_Estado)
    {
        switch (P_Estado)
        {
            case 0: //Estado inicial                
                Grid_Empleados_Activos.SelectedIndex = (-1);
                Grid_Empleados_Activos.Enabled = true;
                Grid_Empleados_Activos.Columns[3].Visible = false;        
                Txt_No_Empleado.Text = String.Empty;
                Txt_Nombre_Empleado.Text = String.Empty;
                Txt_No_Empleado.ReadOnly = true;
                Txt_Nombre_Empleado.ReadOnly = true;

                Btn_Busqueda.Enabled = true;                
                Btn_Modificar.Enabled = true;                
                Btn_Salir.Enabled = true;
                Btn_Busqueda.AlternateText = "Buscar";                
                Btn_Modificar.AlternateText = "Modificar";                
                Btn_Salir.AlternateText = "Salir";

                Btn_Busqueda.ToolTip = "Buscar";                
                Btn_Modificar.ToolTip = "Modificar";                
                Btn_Salir.ToolTip = "Salir";

                Div_Pendientes_Titulo.Visible = false;
                Pnl_Datos_Generales.Visible = false;
                Div_Reasignacion.Visible = false;
                Btn_Generar_Tabla.Visible = true;

                Btn_Busqueda.ImageUrl = "~/paginas/imagenes/paginas/busqueda.png";                
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";                
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";        

                break;        

            case 2: //Modificar

                Grid_Empleados_Activos.Enabled = true;
                Grid_Empleados_Activos.Columns[3].Visible = true;        
                                
                Btn_Modificar.Enabled = true;
                Btn_Salir.Enabled = true;
                Btn_Modificar.AlternateText = "Guardar";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Modificar.ToolTip = "Guardar";
                Btn_Salir.ToolTip = "Cancelar";        
                                
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";                
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";        

                break;
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos
    ///DESCRIPCIÓN: metodo que recibe el DataRow seleccionado de la grilla y carga los datos en los componetes de l formulario
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/03/2011 02:07:54 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Datos(DataRow Dr_Empleados)
    {
        try
        {
            Txt_Nombre_Empleado.Text = Dr_Empleados["NOMBRE_EMPLEADO"].ToString();
            Txt_No_Empleado.Text = Dr_Empleados[Cat_Empleados.Campo_No_Empleado].ToString();
            Hdn_Empleado_ID.Value = Dr_Empleados["EMPLEADO_ID"].ToString();
            Cargar_Grid_Movimientos(0, Dr_Empleados["EMPLEADO_ID"].ToString());
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos_Movimientos
    ///DESCRIPCIÓN: metodo que recibe el DataRow seleccionado de la grilla y carga los datos en los componetes de l formulario
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/03/2011 02:07:54 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Datos_Movimientos(DataRow Dr_Movimientos)
    {
        Pnl_Datos_Generales.Visible = true;
        Div_Reasignacion.Visible = true;
        try
        {
            Txt_Clave.Text = Dr_Movimientos[Ope_Pre_Recepcion_Documentos.Campo_Clave_Tramite].ToString();
            Txt_Fecha.Text = string.Format("{0:dd 'de' MMMM 'del' yyyy}", Convert.ToDateTime(Dr_Movimientos[Ope_Pre_Recepcion_Documentos.Campo_Fecha].ToString()));
            Txt_Notaria.Text = Dr_Movimientos[Cat_Pre_Notarios.Campo_Numero_Notaria].ToString();
            Txt_Notario.Text = Dr_Movimientos["NOMBRE_NOTARIO"].ToString();
            Txt_Numero_Recepcion.Text = Dr_Movimientos[Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento].ToString();
            Txt_Numero_Escritura.Text = Dr_Movimientos[Ope_Pre_Recep_Docs_Movs.Campo_Numero_Escritura].ToString();
            Txt_Estatus.Text = Dr_Movimientos[Ope_Pre_Recep_Docs_Movs.Campo_Estatus].ToString();
            Txt_Cuenta_Predial.Text = Dr_Movimientos[Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial].ToString();

        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Validar_Datos
    ///DESCRIPCIÓN: Guardar datos para dar de alta un nuevo registro de un servicio
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/03/2011 10:45:17 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Boolean Validar_Datos()
    {
        Boolean Resultado = true;
        try
        {
            //if (Txt_Nombre.Text.Trim() == "")
            //{
            //    Resultado = false;
            //    Mensaje_Error("Favor de ingresar el nombre de la Subfamilia");
            //}
            //if (Txt_Abreviatura.Text.Trim() == "")
            //{
            //    Resultado = false;
            //    Mensaje_Error("Favor de ingresar la Abreviatura de la Subfamilia");
            //}
            //if (Cmb_Familias.SelectedIndex <= 0)
            //{
            //    Resultado = false;
            //    Mensaje_Error("Favor de especificar la Familia relacionada con la nueva Subfamilia");
            //}
            //if (Cmb_Estatus.SelectedIndex <= 0)
            //{
            //    Resultado = false;
            //    Mensaje_Error("Favor de especificar el Estatus de la Subfamilia");
            //}
            //if (!Txt_Comentarios.Text.Trim().Equals(""))
            //{
            //    if (Txt_Comentarios.Text.Trim().Length >= 250)
            //    {
            //        Txt_Comentarios.Text = Txt_Comentarios.Text.Trim().Substring(0, 250);
            //    }
            //}

        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
        return Resultado;
    }

        #region Metodos Empleados ABC
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Modificar_Empleados_Estatus
    ///DESCRIPCIÓN: modifica los estatus de los empleados
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 10/Mayo/2011 12:56:41 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
            private void Modificar_Empleados_Estatus()
            {
                DataTable Dt_Empleados = new DataTable();
                if (Session["Empleados_Activos"] != null)
                    Dt_Empleados = (DataTable)Session["Empleados_Activos"];
                String Resultado;
                String Mensaje = "";
                DataTable Dt_Empleados_Temp = Dt_Empleados;
                Cls_Ope_Pre_Empleado_Activos_Negocio Empleados_Negocio = new Cls_Ope_Pre_Empleado_Activos_Negocio();
                try
                {                    
                    Empleados_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                    Empleados_Negocio.P_Dt_Empleados = Dt_Empleados_Temp;
                    Resultado = Empleados_Negocio.Modificar_Empleados_Activos();                    
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Empleados Activos", "alert('Se actualizaron los estatus de los Empleados');", true);
                    if (Resultado.Length > 0)
                    {

                        Mensaje = Mensaje + "Los Empleados:";
                        Mensaje = Mensaje + "</br>";
                        Mensaje = Mensaje + Resultado;
                        Mensaje = Mensaje + "cuentan con pendientes asignados, no se actualizó su estatus";
                        Mensaje_Error(Mensaje);
                    }
                    Consulta_Lista_Empleados();
                    Cargar_Grid_Empleados_Activos(0);
                    Estado_Botones(Const_Estado_Inicial);
                }
                catch (Exception Ex)
                {
                    throw new Exception(Ex.Message);
                }
            }
        #endregion

    #endregion

    #region Eventos

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Tabla_Click
    ///DESCRIPCIÓN: genera la tabla de todos los empleados con el estatus del area de translado de dominio
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 05/10/2011 12:59:52 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************            
    protected void Btn_Generar_Tabla_Click(object sender, EventArgs e)
    {
        try
        {
            Cls_Ope_Pre_Empleado_Activos_Negocio Empleados_Negocio = new Cls_Ope_Pre_Empleado_Activos_Negocio();
            Empleados_Negocio.Generar_Tabla_Empleados_Activos();
            Consulta_Lista_Empleados();
            Cargar_Grid_Empleados_Activos(0);
            Estado_Botones(Const_Estado_Inicial);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Evento para modificar los estatus de los empleados
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 10/Mayo/2011 12:56:41 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
                if (Btn_Modificar.AlternateText.Equals("Modificar"))
                {
                    Estado_Botones(Const_Estado_Modificar);
                }
                else
                {
                    Modificar_Empleados_Estatus();
                }            
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);

        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Salir/Cancelar
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 10/Mayo/2011 12:55:33 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************   
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.AlternateText.Equals("Salir"))
            {
                Session["Empleados_Activos_Modificados"] = null;
                Session["Empleados_Activos"] = null;
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Session["Empleados_Activos_Modificados"] = null;
                Estado_Botones(Const_Estado_Inicial);
                Cargar_Grid_Empleados_Activos(0);
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    #endregion

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Empleados_Activos_PageIndexChanging
    ///DESCRIPCIÓN: paginacion del grid
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 05/06/2011 01:02:01 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Grid_Empleados_Activos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Empleados_Activos.SelectedIndex = (-1);
            Cargar_Grid_Empleados_Activos(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Activar_Desactivar_Click
    ///DESCRIPCIÓN: Camiar estatus de empleado
    ///PARAMETROS:
    ///CREO: jtoledo
    ///FECHA_CREO: 10/Mayo/2011 12:53:59 p.m.
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Activar_Desactivar_Click(object sender, EventArgs e)
    {
        String Empleado_Seleccionado;
        ImageButton Btn_Activar_Empleado = null;
        DataTable Dt_Empleados = new DataTable();
        if (Session["Empleados_Activos_Modificados"] != null)
            Dt_Empleados = (DataTable)Session["Empleados_Activos_Modificados"];
        else if (Session["Empleados_Activos"] != null)
            Dt_Empleados = (DataTable)Session["Empleados_Activos"];
        try
        {
            Btn_Activar_Empleado = (ImageButton)sender;
            Empleado_Seleccionado = Btn_Activar_Empleado.CommandArgument;
            
                for (int index = 0; index <= Dt_Empleados.Rows.Count - 1; index++)
                {
                    if (Dt_Empleados.Rows[index][Cat_Empleados.Campo_No_Empleado].ToString() == Empleado_Seleccionado.ToString())
                    {
                        if (Dt_Empleados.Rows[index][Ope_Pre_Empleados_Activos.Campo_Estatus].ToString().Equals("ACTIVO"))
                        {
                            Dt_Empleados.Rows[index][Ope_Pre_Empleados_Activos.Campo_Estatus] = "INACTIVO";
                        }
                        else if (Dt_Empleados.Rows[index][Ope_Pre_Empleados_Activos.Campo_Estatus].ToString().Equals("INACTIVO"))
                        {
                            Dt_Empleados.Rows[index][Ope_Pre_Empleados_Activos.Campo_Estatus] = "ACTIVO";
                        }
                    }

                }
                Session["Empleados_Activos_Modificados"] = Dt_Empleados;
                Cargar_Grid_Empleados_Activos(Grid_Empleados_Activos.PageIndex);            
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Empleados_Activos_Selectedindexchanged
    ///DESCRIPCIÓN: Se llenan las cajas de texto y se carga el grid de movimientos asignados de el empleado selecciona
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 10/Mayo/2011 12:52:55 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Grid_Empleados_Activos_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable Dt_Empleados = new DataTable();
        if (Session["Empleados_Activos"] != null)
            Dt_Empleados = (DataTable)Session["Empleados_Activos"];
        try
        {
            if (Grid_Empleados_Activos.SelectedIndex > (-1) && Session["Empleados_Activos"] != null)
            {
                Grid_Movimientos.SelectedIndex = (-1);
                Pnl_Datos_Generales.Visible = false;
                Cargar_Datos(Dt_Empleados.Rows[Grid_Empleados_Activos.SelectedIndex + (Grid_Empleados_Activos.PageIndex * 5)]);
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Movimientos_SelectedIndexChanged
    ///DESCRIPCIÓN: Se llenan las cajas de texto y se carga el grid de movimientos asignados de el empleado selecciona
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 10/Mayo/2011 12:52:55 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Grid_Movimientos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Empleados_Activos.SelectedIndex > (-1))
            {
                Cargar_Datos_Movimientos(Dt_Movimiento.Rows[Grid_Movimientos.SelectedIndex + (Grid_Movimientos.PageIndex * 5)]);
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }        
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Reasignar_Click
    ///DESCRIPCIÓN: Evento para reasignar movimiento
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 10/Mayo/2011 12:52:55 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************   
    protected void Btn_Reasignar_Click(object sender, EventArgs e)
    {
        String Empleado_Desactivar;
        String Movimiento_ID;
        Cls_Ope_Pre_Empleado_Activos_Negocio Empleados_Negocio = new Cls_Ope_Pre_Empleado_Activos_Negocio();
        try
        {
            Empleado_Desactivar = Hdn_Empleado_ID.Value.ToString();
            Movimiento_ID = Txt_Numero_Recepcion.Text.Trim();
            Empleados_Negocio.P_Empleado_ID = Empleado_Desactivar;
            Empleados_Negocio.P_No_Movimiento = Movimiento_ID;
            Empleados_Negocio.Reasignar_Movimiento();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Reasignacion de Movimientos", "alert('La reasignacion fue Exitosa');", true);
            Cargar_Grid_Movimientos(0,Hdn_Empleado_ID.Value.ToString());
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    protected void Grid_Movimientos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Cargar_Grid_Movimientos(e.NewPageIndex, Hdn_Empleado_ID.Value.ToString());
    }
}
