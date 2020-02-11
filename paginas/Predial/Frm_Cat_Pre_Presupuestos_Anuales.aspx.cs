using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Claves_Ingreso.Negocio;
using Presidencia.Cat_Pre_Presupuestos_Anuales.Negocio;
using System.Data;
using Presidencia.Constantes;

public partial class paginas_Predial_Frm_Cat_Pre_Presupuestos_Anuales : System.Web.UI.Page
{
    #region Page_Load

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Llenar_Tabla_Claves_Ingreso(0);
                Configuracion_Formulario(false);
            }
            //Limpiamos algún mensaje de error que se halla quedado en el log, que muestra los errores del sistema.
            Lbl_Ecabezado_Mensaje.Visible = false;
            Img_Error.Visible = false;
            Lbl_Error.Text = "";
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Visible = true;
            Img_Error.Visible = true;
            Lbl_Error.Text = Ex.Message.ToString();
        }
    }

    #endregion

    #region Configuración

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
    ///PROPIEDADES:     
    ///             1. estatus.    Estatus en el que se cargara la configuración de los
    ///                            controles.
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 22/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean estatus)
    {
        Btn_Nuevo.Visible = true;
        Btn_Nuevo.AlternateText = "Nuevo";
        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
        Btn_Modificar.Visible = true;
        Btn_Modificar.AlternateText = "Modificar";
        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
        Txt_Monto_Presupuesto.Enabled = estatus;
        Txt_Anio.Enabled = estatus;
        Btn_Actualizar_Presupuesto.Enabled = estatus;
        Btn_Agregar_Presupuesto.Enabled = estatus;
        Grid_Claves_Ingreso.Enabled = !estatus;
        Grid_Presupuestos_Anuales.Enabled = estatus;
        Btn_Busqueda.Enabled = !estatus;
        Txt_Busqueda.Enabled = !estatus;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Formulario
    ///DESCRIPCIÓN: Limpia todos los campos del formulario
    ///PROPIEDADES: 
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 10/Enero/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Formulario()
    {
        Hdf_Presupuesto_Id.Value = "";
        Hdf_Clave_Ingreso_Id.Value = "";
        Txt_Clave_Ingreso.Text = "";
        Txt_Anio.Text = "";
        Txt_Monto_Presupuesto.Text = "";
        Grid_Presupuestos_Anuales.DataSource = null;
        Grid_Presupuestos_Anuales.DataBind();
    }

    #endregion

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Claves_Ingreso
    ///DESCRIPCIÓN: Llena el Grid de Claves de Ingreso
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Claves_Ingreso(int Pagina)
    {
        try
        {
            Cls_Cat_Pre_Claves_Ingreso_Negocio Claves = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            Claves.P_Clave = Txt_Busqueda.Text.Trim().ToUpper();
            Grid_Claves_Ingreso.DataSource = Claves.Llenar_Tabla_Claves_Ingreso_Busqueda();
            Grid_Claves_Ingreso.PageIndex = Pagina;
            Grid_Claves_Ingreso.Columns[1].Visible = true;
            Grid_Claves_Ingreso.DataBind();
            Grid_Claves_Ingreso.Columns[1].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Error.Text = Ex.Message;
            Lbl_Ecabezado_Mensaje.Text = "";
            Div_Contenedor_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Presupuestos_Presupuestos
    ///DESCRIPCIÓN: Llena el Grid de Presupuestos
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 10/Enero/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Presupuestos_Presupuestos()
    {
        try
        {
            Cls_Cat_Pre_Presupuestos_Anuales_Negocio Presupustos = new Cls_Cat_Pre_Presupuestos_Anuales_Negocio();
            Presupustos.P_Clave_Ingreso_Id = Hdf_Clave_Ingreso_Id.Value;
            DataTable Dt_Presupuestos = Presupustos.Consultar_Presupuestos();
            Grid_Presupuestos_Anuales.DataSource = Dt_Presupuestos;
            Grid_Presupuestos_Anuales.PageIndex = 0;
            Grid_Presupuestos_Anuales.Columns[1].Visible = true;
            Grid_Presupuestos_Anuales.Columns[2].Visible = true;
            Grid_Presupuestos_Anuales.DataBind();
            Grid_Presupuestos_Anuales.Columns[1].Visible = false;
            Grid_Presupuestos_Anuales.Columns[2].Visible = false;
            Session["Dt_Presupuestos"] = Dt_Presupuestos;
        }
        catch (Exception Ex)
        {
            Lbl_Error.Text = Ex.Message;
            Lbl_Ecabezado_Mensaje.Text = "";
            Div_Contenedor_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Presupuestos
    ///DESCRIPCIÓN: Llena el Grid de Presupuestos
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 10/Enero/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Presupuestos(int Pagina)
    {
        try
        {
            DataTable Dt_Presupuestos = ((DataTable)Session["Dt_Presupuestos"]).Copy();
            foreach (DataRow Dr_Presupuestos in Dt_Presupuestos.Rows)
            {
                if (Dr_Presupuestos["ACCION"].ToString() == "BORRAR")
                {
                    Dr_Presupuestos.Delete();
                }
            }
            Grid_Presupuestos_Anuales.DataSource = Dt_Presupuestos;
            Grid_Presupuestos_Anuales.PageIndex = Pagina;
            Grid_Presupuestos_Anuales.Columns[1].Visible = true;
            Grid_Presupuestos_Anuales.Columns[2].Visible = true;
            Grid_Presupuestos_Anuales.DataBind();
            Grid_Presupuestos_Anuales.Columns[1].Visible = false;
            Grid_Presupuestos_Anuales.Columns[2].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Error.Text = Ex.Message;
            Lbl_Ecabezado_Mensaje.Text = "";
            Div_Contenedor_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Claves_Ingreso_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView General de Claves de Ingreso
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Claves_Ingreso_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Claves_Ingreso.SelectedIndex = (-1);
            Llenar_Tabla_Claves_Ingreso(e.NewPageIndex);
            //Limpiar_Catalogo();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Error.Text = "";
            Div_Contenedor_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Claves_Ingreso_SelectedIndexChanged
    ///DESCRIPCIÓN: Obtiene los datos de la Clave de Ingreso Seleccionada para mostrarlos a detalle
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Claves_Ingreso_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Claves_Ingreso.SelectedIndex > (-1))
            {
                Limpiar_Formulario();
                Hdf_Clave_Ingreso_Id.Value = Grid_Claves_Ingreso.SelectedRow.Cells[1].Text;
                Txt_Clave_Ingreso.Text = Grid_Claves_Ingreso.SelectedRow.Cells[2].Text;
                Cargar_Presupuestos_Presupuestos();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Error.Text = "";
            Div_Contenedor_Error.Visible = true;
        }
    }


    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        if (Grid_Claves_Ingreso.SelectedIndex > -1)
        {
            try
            {

                if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
                {
                    if (Grid_Presupuestos_Anuales.Rows.Count == 0)
                    {
                        Btn_Nuevo.AlternateText = "Dar de Alta";
                        Configuracion_Formulario(true);
                        Btn_Nuevo.AlternateText = "Dar de Alta";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                        Btn_Salir.AlternateText = "Cancelar";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        Btn_Modificar.Visible = false;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Presupuestos Anuales", "alert('La clave de ingreso ya contiene presupuestos anuales asignados.');", true);
                    }
                }
                else
                {
                    if (Validar_Componentes())
                    {
                        Cls_Cat_Pre_Presupuestos_Anuales_Negocio Presupuestos = new Cls_Cat_Pre_Presupuestos_Anuales_Negocio();
                        Presupuestos.P_Dt_Presupustos = (DataTable)Session["Dt_Presupuestos"];
                        Presupuestos.Alta_Presupuestos();
                        Configuracion_Formulario(false);
                        Limpiar_Formulario();
                        Llenar_Tabla_Claves_Ingreso(Grid_Claves_Ingreso.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Presupuestos Anuales", "alert('Alta de Presupuesto Exitoso');", true);
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Grid_Claves_Ingreso.SelectedIndex = -1;
                        Grid_Presupuestos_Anuales.SelectedIndex = -1;
                    }
                }
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Error.Text = "";
                Div_Contenedor_Error.Visible = true;
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Presupuestos Anuales", "alert('Seleccione una clave de ingreso.');", true);
        }
    }
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        if (Grid_Claves_Ingreso.SelectedIndex > -1)
        {
            if (Grid_Presupuestos_Anuales.Rows.Count > 0)
            {
                try
                {
                    if (Btn_Modificar.AlternateText.Equals("Modificar"))
                    {
                        if (Grid_Claves_Ingreso.Rows.Count > 0 && Grid_Claves_Ingreso.SelectedIndex > (-1))
                        {
                            Btn_Modificar.AlternateText = "Actualizar";
                            Configuracion_Formulario(true);
                            Btn_Modificar.AlternateText = "Actualizar";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                            Btn_Salir.AlternateText = "Cancelar";
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                            Btn_Nuevo.Visible = false;
                        }
                        else
                        {
                            Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el registro que se quiere Modificar";
                            Lbl_Error.Text = "";
                            Div_Contenedor_Error.Visible = true;
                        }
                    }
                    else
                    {
                        if (Validar_Componentes())
                        {
                            Cls_Cat_Pre_Presupuestos_Anuales_Negocio Presupuesto = new Cls_Cat_Pre_Presupuestos_Anuales_Negocio();
                            Presupuesto.P_Dt_Presupustos = (DataTable)Session["Dt_Presupuestos"];
                            Presupuesto.Modificar_Presupuestos();
                            Configuracion_Formulario(false);
                            Limpiar_Formulario();
                            Llenar_Tabla_Claves_Ingreso(Grid_Claves_Ingreso.PageIndex);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Presupuestos Anuales", "alert('Actualización de Presupuesto Exitoso');", true);
                            Btn_Modificar.AlternateText = "Modificar";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                            Btn_Salir.AlternateText = "Salir";
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                            Grid_Claves_Ingreso.SelectedIndex = -1;
                            Grid_Presupuestos_Anuales.SelectedIndex = -1;
                        }
                    }
                }
                catch (Exception Ex)
                {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Error.Text = "";
                    Div_Contenedor_Error.Visible = true;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Presupuestos Anuales", "alert('La clave de ingreso no contiene presupuestos anuales asignados.');", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Presupuestos Anuales", "alert('Seleccione una clave de ingreso.');", true);
        }
    }
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText.Equals("Salir"))
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Configuracion_Formulario(false);
            Limpiar_Formulario();
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Grid_Claves_Ingreso.SelectedIndex = -1;
            Grid_Presupuestos_Anuales.SelectedIndex = -1;
        }
    }

    private Boolean Validar_Componentes()
    {
        Boolean Valido = true;
        String Mensaje_Error = "Error: ";
        if (Grid_Presupuestos_Anuales.Rows.Count==0)
        {
            Mensaje_Error += "Introduzca al menos un presupuesto anual.";
            Valido = false;
        }
        if (!Valido)
        {
            Lbl_Error.Text = Mensaje_Error;
            Div_Contenedor_Error.Visible = true;
        }
        return Valido;
    }

    private Boolean Validar_Componentes_Detalles()
    {
        Boolean Valido = true;
        String Mensaje_Error = "Error: ";
        if (Txt_Anio.Text.Trim().Length!=4)
        {
            Mensaje_Error += "Introduzca un año válido.";
            Valido = false;
        }
        if (Txt_Monto_Presupuesto.Text=="")
        {
            if(Mensaje_Error!="")
            { Mensaje_Error += "<br>"; }
            Mensaje_Error += "Introduzca un presupuesto válido.";
            Valido = false;
        }
        if (!Valido)
        {
            Lbl_Error.Text = Mensaje_Error;
            Div_Contenedor_Error.Visible = true;
        }
        return Valido;
    }
    protected void Grid_Presupuestos_Anuales_SelectedIndex_Changed(object sender, EventArgs e)
    {
        if (Grid_Presupuestos_Anuales.SelectedIndex > -1)
        {
            Hdf_Presupuesto_Id.Value = Grid_Presupuestos_Anuales.SelectedRow.Cells[1].Text;
            Txt_Anio.Text = Grid_Presupuestos_Anuales.SelectedRow.Cells[3].Text;
            Txt_Monto_Presupuesto.Text = Grid_Presupuestos_Anuales.SelectedRow.Cells[4].Text.Replace("$","").Replace(",","");
            Hdf_Anio.Value = Grid_Presupuestos_Anuales.SelectedRow.Cells[3].Text;
            Hdf_Importe.Value = Grid_Presupuestos_Anuales.SelectedRow.Cells[4].Text.Replace("$", "").Replace(",", "");
        }
    }
    protected void Grid_Presupuestos_Anuales_PageIndex_Changing(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Presupuestos_Anuales.SelectedIndex = (-1);
            Llenar_Tabla_Presupuestos(e.NewPageIndex);
            //Limpiar_Catalogo();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Error.Text = "";
            Div_Contenedor_Error.Visible = true;
        }
    }
    protected void Btn_Agregar_Presupuesto_Click(object sender, ImageClickEventArgs e)
    {
        if (Validar_Componentes_Detalles())
        {
            DataTable Dt_Presupuestos = (DataTable)Session["Dt_Presupuestos"];
            if (!Encontrar_Presupuesto(Dt_Presupuestos, Txt_Anio.Text, 1))
            {
                DataRow Dr_Renglon_Nuevo = Dt_Presupuestos.NewRow();
                Dr_Renglon_Nuevo[Cat_Pre_Clav_Ing_Presupuestos.Campo_Clave_Ingreso_Id] = Hdf_Clave_Ingreso_Id.Value;
                Dr_Renglon_Nuevo[Cat_Pre_Clav_Ing_Presupuestos.Campo_Presupuesto_Id] = "0";
                Dr_Renglon_Nuevo[Cat_Pre_Clav_Ing_Presupuestos.Campo_Anio] = Txt_Anio.Text;
                Dr_Renglon_Nuevo[Cat_Pre_Clav_Ing_Presupuestos.Campo_Importe] = Txt_Monto_Presupuesto.Text;
                Dr_Renglon_Nuevo["ACCION"] = "AGREGAR";
                Dt_Presupuestos.Rows.Add(Dr_Renglon_Nuevo);
                Session["Dt_Presupuestos"] = Dt_Presupuestos.Copy();
                Grid_Presupuestos_Anuales.DataSource = Dt_Presupuestos;
                foreach (DataRow Dr_Renglon_Actual in Dt_Presupuestos.Rows)
                {
                    if (Dr_Renglon_Actual["ACCION"].ToString() == "BORRAR")
                    {
                        Dr_Renglon_Actual.Delete();
                    }
                }
                Grid_Presupuestos_Anuales.DataBind();
                Txt_Anio.Text = "";
                Txt_Monto_Presupuesto.Text = "";
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Presupuestos Anuales", "alert('Ya esiste un presupuesto para este Año.');", true);
            }
        }
    }
    
    protected void Grid_Presupuestos_Anuales_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Borrar_Presupuesto")
        {
            DataTable Dt_Presupuestos = ((DataTable)Session["Dt_Presupuestos"]).Copy();
            foreach (DataRow Dr_Presupuesto_A_Borrar in Dt_Presupuestos.Rows)
            {
                if (Grid_Presupuestos_Anuales.Rows[Convert.ToInt32(e.CommandArgument)].Cells[3].Text == Dr_Presupuesto_A_Borrar["ANIO"].ToString() && Convert.ToDouble(Grid_Presupuestos_Anuales.Rows[Convert.ToInt32(e.CommandArgument)].Cells[4].Text.Replace("$", "")).ToString() == Dr_Presupuesto_A_Borrar["IMPORTE"].ToString())
                {
                    if (Dr_Presupuesto_A_Borrar["PRESUPUESTO_ID"].ToString() == "0")
                    {
                        Dr_Presupuesto_A_Borrar.Delete();
                        break;
                    }
                    else
                    {
                        Dr_Presupuesto_A_Borrar["ACCION"] = "BORRAR";
                    }
                }
                Txt_Anio.Text = "";
                Txt_Monto_Presupuesto.Text = "";
            }
            Session["Dt_Presupuestos"] = Dt_Presupuestos.Copy();
            foreach (DataRow Dr_Presupuesto in Dt_Presupuestos.Rows)
            {
                if (Dr_Presupuesto["ACCION"].ToString() == "BORRAR")
                {
                    Dr_Presupuesto.Delete();
                }
            }
            Grid_Presupuestos_Anuales.DataSource = Dt_Presupuestos;
            Grid_Presupuestos_Anuales.PageIndex = 0;
            Grid_Presupuestos_Anuales.DataBind();
        }
    }



    protected void Btn_Actualizar_Presupuesto_Click(object sender, ImageClickEventArgs e)
    {
        if (Grid_Presupuestos_Anuales.SelectedIndex > -1)
        {
            if (Validar_Componentes_Detalles())
            {
                DataTable Dt_Presupuestos = (DataTable)Session["Dt_Presupuestos"];
                if (!Encontrar_Presupuesto(Dt_Presupuestos, Txt_Anio.Text, 2))
                {
                    foreach (DataRow Dr_Presupuesto in Dt_Presupuestos.Rows)
                    {
                        if (Dr_Presupuesto["ANIO"].ToString() == Hdf_Anio.Value && Convert.ToDouble(Dr_Presupuesto["IMPORTE"].ToString()).ToString("0.00") == Hdf_Importe.Value)
                        {
                            Dr_Presupuesto["ANIO"] = Txt_Anio.Text;
                            Dr_Presupuesto["IMPORTE"] = Txt_Monto_Presupuesto.Text;
                            if (Dr_Presupuesto["ACCION"].ToString() != "AGREGAR")
                            {
                                Dr_Presupuesto["ACCION"] = "ACTUALIZAR";
                            }
                        }
                    }
                    Session["Dt_Presupuestos"] = Dt_Presupuestos.Copy();
                    Grid_Presupuestos_Anuales.DataSource = Dt_Presupuestos;
                    foreach (DataRow Dr_Renglon_Actual in Dt_Presupuestos.Rows)
                    {
                        if (Dr_Renglon_Actual["ACCION"].ToString() == "BORRAR")
                        {
                            Dr_Renglon_Actual.Delete();
                        }
                    }
                    Grid_Presupuestos_Anuales.DataBind();
                    Txt_Anio.Text = "";
                    Txt_Monto_Presupuesto.Text = "";
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Presupuestos Anuales", "alert('Ya esiste un presupuesto para este Año.');", true);
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Presupuestos Anuales", "alert('Seleccione el presupuesto anual a modificar.');", true);
        }
    }

    protected void Txt_Monto_Presupuesto_TextChanged(object sender, EventArgs e)
    {
        if (Txt_Monto_Presupuesto.Text.Trim() == "")
        {
            Txt_Monto_Presupuesto.Text = "0.00";
        }
        else 
        {
            try
            {
                Txt_Monto_Presupuesto.Text = Convert.ToDouble(Txt_Monto_Presupuesto.Text).ToString("#,###,###,###,###,###,###,##0.00");
            }
            catch
            {
                Txt_Monto_Presupuesto.Text = "0.00";
            }
        }
    }

    protected Boolean Encontrar_Presupuesto(DataTable Dt_Presupuestos, String Anio, Int32 Caso)
    {
        Boolean Encontrado = false;
        if (Caso == 1)
        {
            foreach (DataRow Dr_Renglon_Actual in Dt_Presupuestos.Rows)
            {
                if (Dr_Renglon_Actual["ANIO"].ToString() == Anio && Dr_Renglon_Actual["ACCION"].ToString() != "BORRAR")
                {
                    Encontrado = true;
                    break;
                }
            }
        }
        else
        {
            foreach (DataRow Dr_Renglon_Actual in Dt_Presupuestos.Rows)
            {
                if (Dr_Renglon_Actual["ANIO"].ToString() == Anio && Dr_Renglon_Actual["ACCION"].ToString()!="BORRAR")
                {
                    if (Dr_Renglon_Actual["ANIO"].ToString() == Hdf_Anio.Value && Convert.ToDouble(Dr_Renglon_Actual["IMPORTE"].ToString()).ToString("0.00") == Hdf_Importe.Value)
                    {

                    }
                    else
                    {
                        Encontrado = true;
                    }
                    break;
                }
            }
        }


        return Encontrado;
    }
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //Limpiar_Catalogo();
            Llenar_Tabla_Claves_Ingreso(0);
            if (Grid_Claves_Ingreso.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0)
            {
                Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el Concepto\"" + Txt_Busqueda.Text + "\" no se encotrarón coincidencias";
                Lbl_Error.Text = "(Se cargaron  todas las Claves de ingreso almacenadas)";
                Div_Contenedor_Error.Visible = true;
                Txt_Busqueda.Text = "";
                Llenar_Tabla_Claves_Ingreso(0);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Error.Text = "";
            Div_Contenedor_Error.Visible = true;
        }
    }
}