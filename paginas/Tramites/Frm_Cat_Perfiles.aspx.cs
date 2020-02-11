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
using Presidencia.Catalogo_Perfiles.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;

public partial class paginas_tramites_Frm_Cat_Perfiles : System.Web.UI.Page
{

    #region Page Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 06/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;
            if (!IsPostBack)
            {
                Configuracion_Formulario(true);
                Llenar_Grid_Perfiles(0);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    #endregion

    #region Metodos

    #region Generales

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
    ///PARAMETROS:     
    ///             1. estatus.    Estatus en el que se cargara la configuración de los controles.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 06/Octubre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean estatus)
    {
        try
        {
            Btn_Nuevo.Visible = true;
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            Btn_Modificar.Visible = true;
            Btn_Modificar.AlternateText = "Modificar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            Btn_Eliminar.Visible = estatus;
            Txt_Nombre.Enabled = !estatus;
            Txt_Descripcion.Enabled = !estatus;
            Grid_Perfiles.Enabled = estatus;
            Grid_Perfiles.SelectedIndex = (-1);
            Grid_Subprocesos.SelectedIndex = (-1);
            Btn_Buscar_Perfil.Enabled = estatus;
            Txt_Busqueda_Perfil.Enabled = estatus;
            Grid_Subprocesos.Enabled = !estatus;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
    ///DESCRIPCIÓN: Limpia los controles del Formulario
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 06/Octubre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Catalogo()
    {
        try
        {
            Txt_ID_Perfil.Text = "";
            Txt_Nombre.Text = "";
            Txt_Descripcion.Text = "";
            Grid_Subprocesos.DataSource = new DataTable();
            Grid_Subprocesos.DataBind();
            Hdf_Perfil_ID.Value = "";
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    #endregion

    #region SubProcesos - Perfiles

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Obtener_Subprocesos_Seleccionados
    ///DESCRIPCIÓN: Obtiene los Subprocesos que se han seleccionado.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 07/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private DataTable Obtener_Subprocesos_Seleccionados()
    {
        DataTable Tabla = null;
        GridView Gv_Actividades;
        try
        {

            foreach (GridViewRow Fila in Grid_Subprocesos.Rows)
            {
                Gv_Actividades = (GridView)Fila.FindControl("Grid_Detalles_Actividades");

                if (Gv_Actividades is GridView)
                {
                    if (Gv_Actividades.Rows.Count > 0)
                    {
                        foreach (GridViewRow Fila_Detalle in Gv_Actividades.Rows)
                        {
                            if (Fila_Detalle is GridViewRow)
                            {
                                CheckBox Chk_Habilitar = (CheckBox)Fila_Detalle.FindControl("Chck_Activar");
                                if (Chk_Habilitar is CheckBox)
                                {
                                    if (Chk_Habilitar.Checked)
                                    {
                                        if (Tabla == null)
                                        {
                                            Tabla = new DataTable("Subprocesos");
                                            Tabla.Columns.Add("SUBPROCESO_ID", Type.GetType("System.String"));
                                        }
                                        DataRow Fila_Temporal = Tabla.NewRow();
                                        Fila_Temporal["SUBPROCESO_ID"] = Fila_Detalle.Cells[1].Text;
                                        Tabla.Rows.Add(Fila_Temporal);
                                        Tabla.AcceptChanges();
                                    }
                                }

                            }
                        }
                    }
                }
            }
            if (Tabla == null)
            {
                Tabla = new DataTable();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
        return Tabla;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Privilegios_Perfil
    ///DESCRIPCIÓN: Obtiene los Subprocesos que se han seleccionado.
    ///PARAMETROS:     
    ///             1.  Privilegios.    DataTabla con los subprocesos en los que tiene
    ///                                 los privilegios este perfil.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 07/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Privilegios_Perfil(DataTable Privilegios)
    {
        try
        {
            if (Grid_Subprocesos.Rows.Count > 0 && Privilegios.Rows.Count > 0)
            {
                for (int Contador = 0; Contador < Grid_Subprocesos.Rows.Count; Contador++)
                {
                    CheckBox Chk_Temporal = (CheckBox)Grid_Subprocesos.Rows[Contador].Cells[0].Controls[1];
                    String Elemento_ID = Grid_Subprocesos.Rows[Contador].Cells[3].Text;
                    Boolean Encontrado = false;
                    for (int Contador_2 = 0; Contador_2 < Privilegios.Rows.Count; Contador_2++)
                    {
                        if (Elemento_ID.Equals(Privilegios.Rows[Contador_2][0].ToString())) { Encontrado = true; break; }
                    }
                    Chk_Temporal.Checked = Encontrado;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    #endregion

    #region Validaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación de la pestaña de Perfiles.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 06/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Componentes()
    {
        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Txt_Nombre.Text.Trim().Length == 0)
        {
            Mensaje_Error = Mensaje_Error + "+ Introducir el Nombre (Pestaña 1 de 2).";
            Validacion = false;
        }
        if (Txt_Descripcion.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir la Descripci&oacute;n (Pestaña 1 de 2).";
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

    #endregion

    #region Grids

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Perfiles_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de Perfiles
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 07/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Perfiles_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Perfiles.SelectedIndex = (-1);
            Llenar_Grid_Perfiles(e.NewPageIndex);
            Limpiar_Catalogo();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }


    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Detalles_Actividades_RowDataBound
    ///DESCRIPCIÓN: Maneja la paginación del GridView de Perfiles
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 07/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Detalles_Actividades_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Cls_Cat_Tra_Perfiles_Negocio Negocio_Actividad = new Cls_Cat_Tra_Perfiles_Negocio();
        DataTable Dt_Consulta = new DataTable();
        try
        {
           
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            { 
                CheckBox chek = (CheckBox)e.Row.FindControl("Chck_Activar");
                chek.Checked = false;

                if (Hdf_Perfil_ID.Value != "")
                {
                    Negocio_Actividad.P_Perfil_ID = Hdf_Perfil_ID.Value;
                    Dt_Consulta = Negocio_Actividad.Consultar_Actividades_Perfil();

                    if (Dt_Consulta.Rows.Count > 0)
                    {
                        for (int Contador_Actividades = 0; Contador_Actividades < Dt_Consulta.Rows.Count; Contador_Actividades++)
                        {
                            if (e.Row.Cells[1].Text == Dt_Consulta.Rows[Contador_Actividades][Tra_Subprocesos_Perfiles.Campo_Subproceso_ID].ToString().Trim())
                            {
                                chek.Checked = true;
                                break;
                            }  
                        }
                    }
                }
                else
                {
                    chek.Checked = false;
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Subprocesos_RowDataBound
    ///DESCRIPCIÓN: Maneja la paginación del GridView de Perfiles
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 07/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Subprocesos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridView Gv_Detalles = new GridView();
        DataTable Dt_Datos_Detalles = new DataTable();
        DataTable Ds_Consulta = new DataTable(); 
        DataTable Ds_Modificada = new DataTable();
        int Contador;
        Image Img = new Image();
        Img = (Image)e.Row.FindControl("Img_Btn_Expandir");
        Literal Lit = new Literal();
        Lit = (Literal)e.Row.FindControl("Ltr_Inicio");
        Label Lbl_facturas = new Label();
        Lbl_facturas = (Label)e.Row.FindControl("Lbl_Actividades");
        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                Contador = (int)(Session["Contador"]);
                Lit.Text = Lit.Text.Trim().Replace("Renglon_Grid", ("Renglon_Grid" + Lbl_facturas.Text));
                Img.Attributes.Add("OnClick", ("Mostrar_Tabla(\'Renglon_Grid"
                                + (Lbl_facturas.Text + ("\',\'"
                                + (Img.ClientID + "\')")))));
                Dt_Datos_Detalles = ((DataTable)(Session["Dt_Datos_Detalles"]));
                Cls_Cat_Tra_Perfiles_Negocio Subprocesos = new Cls_Cat_Tra_Perfiles_Negocio();
                Subprocesos.P_Tramite_id = Dt_Datos_Detalles.Rows[Contador]["TRAMITE_ID"].ToString();//e.Row.Cells[1].Text;
                Ds_Consulta = Subprocesos.Consultar_Actividades_Tramites();              
                Gv_Detalles = (GridView)e.Row.Cells[3].FindControl("Grid_Detalles_Actividades");
                Gv_Detalles.Columns[1].Visible = true;
                Gv_Detalles.DataSource = Ds_Consulta;
                Gv_Detalles.DataBind();
                Gv_Detalles.Columns[1].Visible = false;
                Session["Contador"] = Contador + 1;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Seleccionar_Requisicion_Click
    ///DESCRIPCIÓN: Metodo para consultar la reserva
    ///CREO: Sergio Manuel Gallardo Andrade
    ///FECHA_CREO: 17/noviembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Seleccionar_Solicitud_Click(object sender, ImageClickEventArgs e)
    {
        Limpiar_Catalogo();
        String ID_Seleccionado = ((ImageButton)sender).CommandArgument;

        ImageButton ImageButton = (ImageButton)sender;
        TableCell TableCell = (TableCell)ImageButton.Parent;
        GridViewRow Row = (GridViewRow)TableCell.Parent;
        Grid_Perfiles.SelectedIndex = Row.RowIndex;
        int Fila = Row.RowIndex;

        //  se carga la informacion del perfil
        Cls_Cat_Tra_Perfiles_Negocio Perfil = new Cls_Cat_Tra_Perfiles_Negocio();
        Perfil.P_Perfil_ID = ID_Seleccionado;
        Perfil = Perfil.Consultar_Datos_Perfil();
        Hdf_Perfil_ID.Value = Perfil.P_Perfil_ID;
        Txt_ID_Perfil.Text = Perfil.P_Perfil_ID;
        Txt_Nombre.Text = Perfil.P_Nombre;
        Txt_Descripcion.Text = Perfil.P_Descripcion;
        //Llenar_Grid_Subprocesos(Perfil.P_Detalles_Subproceso);

        //  para las actividades del tramite
        Cls_Cat_Tra_Perfiles_Negocio Subprocesos = new Cls_Cat_Tra_Perfiles_Negocio();
        //Subprocesos.P_Tipo_DataTable = "SUBPROCESOS";
        DataTable Dt_Actividades_Tramtie = Subprocesos.Consultar_Tramites();
        Llenar_Grid_Subprocesos();
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Perfiles_SelectedIndexChanged
    ///DESCRIPCIÓN: Obtiene los datos del Perfil Seleccionado para mostrarlos a detalle
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 07/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Perfiles_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Perfiles.SelectedIndex > (-1))
            {
                Limpiar_Catalogo();
                String ID_Seleccionado = Grid_Perfiles.SelectedRow.Cells[1].Text;
                Cls_Cat_Tra_Perfiles_Negocio Perfil = new Cls_Cat_Tra_Perfiles_Negocio();
                Perfil.P_Perfil_ID = ID_Seleccionado;
                Perfil = Perfil.Consultar_Datos_Perfil();
                Hdf_Perfil_ID.Value = Perfil.P_Perfil_ID;
                Txt_ID_Perfil.Text = Perfil.P_Perfil_ID;
                Txt_Nombre.Text = Perfil.P_Nombre;
                Txt_Descripcion.Text = Perfil.P_Descripcion;
                //Llenar_Grid_Subprocesos(Perfil.P_Detalles_Subproceso);

                //  para las actividades del tramite
                Cls_Cat_Tra_Perfiles_Negocio Subprocesos = new Cls_Cat_Tra_Perfiles_Negocio();
                //Subprocesos.P_Tipo_DataTable = "SUBPROCESOS";
                DataTable Dt_Actividades_Tramtie = Subprocesos.Consultar_Tramites();
                Llenar_Grid_Subprocesos();

                //System.Threading.Thread.Sleep(1000);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    #endregion

    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Perfiles
    ///DESCRIPCIÓN: Llena la tabla de Perfiles
    ///PARAMETROS:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 06/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Perfiles(int Pagina)
    {
        Cls_Cat_Tra_Perfiles_Negocio Negocio_Perfiles = new Cls_Cat_Tra_Perfiles_Negocio();
        DataTable Dt_Perfiles = new DataTable();
        try
        {
            Negocio_Perfiles.P_Tipo_DataTable = "PERFILES";
            Negocio_Perfiles.P_Nombre = Txt_Busqueda_Perfil.Text.Trim();
            Dt_Perfiles = Negocio_Perfiles.Consultar_DataTable();

            DataView Dv_Ordenar = new DataView(Dt_Perfiles);
            Dv_Ordenar.Sort = Cat_Tra_Perfiles.Campo_Nombre;
            Dt_Perfiles = Dv_Ordenar.ToTable();

            Grid_Perfiles.Columns[1].Visible = true;
            Grid_Perfiles.DataSource = Dt_Perfiles;
            Grid_Perfiles.PageIndex = Pagina;
            Grid_Perfiles.DataBind();
            Grid_Perfiles.Columns[1].Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Subprocesos
    ///DESCRIPCIÓN: Llena la tabla de Subprocesos
    ///PARAMETROS:         
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 07/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Subprocesos()
    {
        Cls_Cat_Tra_Perfiles_Negocio Begocio_Subprocesos = new Cls_Cat_Tra_Perfiles_Negocio();
        DataTable Dt_Datos_Detalles = new DataTable();
        DataTable Dt_Modificado = new DataTable();
        DataTable Dt_Actividades_Tramite = new DataTable();
        try
        {
            Dt_Actividades_Tramite = Begocio_Subprocesos.Consultar_Tramites_Dependencia();

            Dt_Datos_Detalles = ((DataTable)(Session["Dt_Datos_Detalles"]));
            int Contador = 0;
            Session["Contador"] = Contador;

            Session["Dt_Datos_Detalles"] = Dt_Actividades_Tramite;
            Grid_Subprocesos.Columns[1].Visible = true;
            Grid_Subprocesos.DataSource = Dt_Actividades_Tramite;
            Grid_Subprocesos.DataBind();
            Grid_Subprocesos.Columns[1].Visible = false;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Subprocesos_Grid
    ///DESCRIPCIÓN: Llena la tabla de Subprocesos con los subprocesos que estan en la
    ///             Base de Datos, catalogandolos por el Tramite al que pertenecen.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 07/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Subprocesos_Grid()
    {
        try
        {
            Grid_Subprocesos.Columns[1].Visible = true;
            Grid_Subprocesos.Columns[3].Visible = true;
            Cls_Cat_Tra_Perfiles_Negocio Subprocesos = new Cls_Cat_Tra_Perfiles_Negocio();
            Subprocesos.P_Tipo_DataTable = "SUBPROCESOS";
            Grid_Subprocesos.DataSource = Subprocesos.Consultar_DataTable();
            Grid_Subprocesos.DataBind();
            Grid_Subprocesos.Columns[1].Visible = false;
            Grid_Subprocesos.Columns[3].Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    #endregion

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta un nuevo Perfil
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 07/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, EventArgs e)
    {
        Cls_Cat_Tra_Perfiles_Negocio Negocio_Perfil = new Cls_Cat_Tra_Perfiles_Negocio();
        try
        {
            if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
            {
                Configuracion_Formulario(false);
                Limpiar_Catalogo();
                Btn_Nuevo.AlternateText = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Modificar.Visible = false;
                Llenar_Grid_Subprocesos();
            }
            else
            {
                if (Validar_Componentes())
                {

                    Negocio_Perfil.P_Nombre = Txt_Nombre.Text.Trim();
                    Negocio_Perfil.P_Descripcion = Txt_Descripcion.Text.Trim();
                    Negocio_Perfil.P_Detalles_Subproceso = Obtener_Subprocesos_Seleccionados();
                    Negocio_Perfil.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                    Negocio_Perfil.Alta_Perfil();
                    Configuracion_Formulario(true);
                    Limpiar_Catalogo();
                    Llenar_Grid_Perfiles(Grid_Perfiles.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Perfiles", "alert('Alta de Perfil Exitosa');", true);
                    Btn_Nuevo.AlternateText = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Tab_Contenedor_Pestagnas.ActiveTabIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de un Perfil
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 07/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, EventArgs e)
    {
        Cls_Cat_Tra_Perfiles_Negocio Negocio_Perfil = new Cls_Cat_Tra_Perfiles_Negocio();
        try
        {
            if (Btn_Modificar.AlternateText.Equals("Modificar"))
            {
                if (Grid_Perfiles.Rows.Count > 0 && Grid_Perfiles.SelectedIndex > (-1))
                {
                    Configuracion_Formulario(false);
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.Visible = false;
                }
                else
                {
                    Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Modificar.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                if (Validar_Componentes())
                {

                    Negocio_Perfil.P_Perfil_ID = Hdf_Perfil_ID.Value;
                    Negocio_Perfil.P_Nombre = Txt_Nombre.Text.Trim();
                    Negocio_Perfil.P_Descripcion = Txt_Descripcion.Text.Trim();
                    Negocio_Perfil.P_Detalles_Subproceso = Obtener_Subprocesos_Seleccionados();
                    Negocio_Perfil.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                    Negocio_Perfil.Modificar_Perfil();


                    Configuracion_Formulario(true);
                    Limpiar_Catalogo();
                    Llenar_Grid_Perfiles(Grid_Perfiles.PageIndex);

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Perfiles", "alert('Actualización Perfil Exitosa');", true);
                    
                    Btn_Modificar.AlternateText = "Modificar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Tab_Contenedor_Pestagnas.ActiveTabIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Perfil_Click
    ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 07/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Perfil_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Limpiar_Catalogo();
            Grid_Perfiles.SelectedIndex = (-1);
            Llenar_Grid_Perfiles(0);
            if (Grid_Perfiles.Rows.Count == 0 && Txt_Busqueda_Perfil.Text.Trim().Length > 0)
            {
                Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el nombre \"" + Txt_Busqueda_Perfil.Text + "\" no se encotrarón coincidencias";
                Lbl_Mensaje_Error.Text = "(Se cargarón todos los Perfiles almacenados)";
                Div_Contenedor_Msj_Error.Visible = true;
                Txt_Busqueda_Perfil.Text = "";
                Llenar_Grid_Perfiles(0);
            }
            Tab_Contenedor_Pestagnas.ActiveTabIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
    ///DESCRIPCIÓN: Elimina un Perfil de la Base de Datos
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 07/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Perfiles.Rows.Count > 0 && Grid_Perfiles.SelectedIndex > (-1))
            {
                Cls_Cat_Tra_Perfiles_Negocio Perfil = new Cls_Cat_Tra_Perfiles_Negocio();
                Perfil.P_Perfil_ID = Hdf_Perfil_ID.Value;
                Perfil.Elimnar_Perfil();
                Grid_Perfiles.SelectedIndex = (-1);
                Llenar_Grid_Perfiles(Grid_Perfiles.PageIndex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Perfiles", "alert('El Perfil fue eliminado exitosamente');", true);
                Tab_Contenedor_Pestagnas.TabIndex = 0;
                Limpiar_Catalogo();
                Tab_Contenedor_Pestagnas.ActiveTabIndex = 0;
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Eliminar.";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Cancela la operación que esta en proceso (Alta o Actualizar) o 
    ///             Sale del Formulario.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 07/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, EventArgs e)
    {
        if (Btn_Salir.AlternateText.Equals("Salir"))
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Configuracion_Formulario(true);
            Limpiar_Catalogo();
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
        }
    }

    #endregion

}