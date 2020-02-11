using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Operacion_Cat_Recepcion_Oficios.Negocio;
using System.Data;
using Presidencia.Sessiones;

public partial class paginas_Catastro_Frm_Ope_Cat_Recepcion_Oficios : System.Web.UI.Page
{
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN:
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Configuracion_Formulario(true);
                Llenar_Tabla_Oficios(0);
                Llenar_Combos();
                Txt_Descripcion.Attributes.Add("OnKeyPress", " ValidarCaracteres(this,250)");
                Txt_Descripcion.Attributes.Add("OnKeyUp", " ValidarCaracteres(this,250)");
                Txt_Descripcion.Attributes.Add("OnKeyPress", " ValidarCaracteres(this,250)");
                Txt_Descripcion.Attributes.Add("OnKeyUp", " ValidarCaracteres(this,250)");
            }
        }
        catch (Exception ex)
        {
            Lbl_Ecabezado_Mensaje.Text = ex.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
        Div_Contenedor_Msj_Error.Visible = false;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Establece la configuración del formulario
    ///PROPIEDADES:     Enabled: Especifica si estara habilitado o no el componente
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean Enabled)
    {
        Txt_Fecha_Oficio.Enabled = false;
        Txt_Dependencia.Enabled = !Enabled;
        Txt_Descripcion.Enabled = !Enabled;
        Cmb_Dep_Catastro.Enabled = !Enabled;
        Cmb_Hora.Enabled = !Enabled;
        Cmb_Minutos.Enabled = !Enabled;
        Cmb_Segundos.Enabled = !Enabled;
        Btn_Buscar_oficio.Enabled = Enabled;
        Txt_Busqueda_Oficio.Enabled = Enabled;
        Grid_Oficios.Enabled = Enabled;
        Txt_No_Oficio_Recepcion.Enabled = !Enabled;
        
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Establece la configuración del formulario
    ///PROPIEDADES:     Enabled: Especifica si estara habilitado o no el componente
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Oficios(int Pagina)
    {
        try
        {
            Cls_Ope_Cat_Recepcion_Oficios_Negocio Oficios = new Cls_Ope_Cat_Recepcion_Oficios_Negocio();
            DataTable Dt_Oficios;
            if (Txt_Busqueda_Oficio.Text.Trim() != "")
            {
                if (Cmb_Filtro_Busqueda.SelectedValue == "OFICIO")
                {
                    Oficios.P_No_Oficio_Recepcion = Txt_Busqueda_Oficio.Text.ToUpper();
                }
                else if (Cmb_Filtro_Busqueda.SelectedValue == "DEPARTAMENTO")
                {
                    Oficios.P_Departamento_Catastro = Txt_Busqueda_Oficio.Text.ToUpper();
                }
            }
            Dt_Oficios = Oficios.Consultar_Oficios();
            Grid_Oficios.Columns[1].Visible = true;
            Grid_Oficios.Columns[7].Visible = true;
            Grid_Oficios.DataSource = Dt_Oficios;
            Grid_Oficios.PageIndex = Pagina;
            Grid_Oficios.DataBind();
            Grid_Oficios.Columns[1].Visible = false;
            Grid_Oficios.Columns[7].Visible = false;
        }
        catch (Exception E)
        {
            Lbl_Ecabezado_Mensaje.Text = E.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combos
    ///DESCRIPCIÓN: Establece la configuración del formulario
    ///PROPIEDADES:     Enabled: Especifica si estara habilitado o no el componente
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combos()
    {
        try
        {
            DataTable Dt_Horas = new DataTable();
            Dt_Horas.Columns.Add("TEXT", typeof(String));
            Dt_Horas.Columns.Add("VALUE", typeof(String));
            DataTable Dt_Minutos = new DataTable();
            Dt_Minutos.Columns.Add("TEXT", typeof(String));
            Dt_Minutos.Columns.Add("VALUE", typeof(String));
            DataTable Dt_Segundos = new DataTable();
            Dt_Segundos.Columns.Add("TEXT", typeof(String));
            Dt_Segundos.Columns.Add("VALUE", typeof(String));
            
            DataRow Dr_Horas;
            DataRow Dr_Minutos;
            DataRow Dr_Segundos;
            for (Int16 i = 0; i < 60; i++)
            {
                Dr_Minutos = Dt_Minutos.NewRow();
                Dr_Segundos = Dt_Segundos.NewRow();
                Dr_Minutos["TEXT"] = i.ToString("00");
                Dr_Minutos["VALUE"] = i.ToString("00");
                Dr_Segundos["TEXT"] = i.ToString("00");
                Dr_Segundos["VALUE"] = i.ToString("00");
                Dt_Minutos.Rows.Add(Dr_Minutos);
                Dt_Segundos.Rows.Add(Dr_Segundos);
            }

            for (Int16 i = 0; i < 24; i++)
            {
                Dr_Horas = Dt_Horas.NewRow();
                Dr_Horas["TEXT"] = i.ToString("00");
                Dr_Horas["VALUE"] = i.ToString("00");
                Dt_Horas.Rows.Add(Dr_Horas);
            }
            Cmb_Hora.DataTextField = "TEXT";
            Cmb_Hora.DataValueField = "VALUE";
            Cmb_Hora.DataSource = Dt_Horas;
            Cmb_Hora.DataBind();
            Cmb_Minutos.DataTextField = "TEXT";
            Cmb_Minutos.DataValueField = "VALUE";
            Cmb_Minutos.DataSource = Dt_Minutos;
            Cmb_Minutos.DataBind();
            Cmb_Segundos.DataTextField = "TEXT";
            Cmb_Segundos.DataValueField = "VALUE";
            Cmb_Segundos.DataSource = Dt_Segundos;
            Cmb_Segundos.DataBind();

        }
        catch (Exception E)
        {
            Lbl_Ecabezado_Mensaje.Text = E.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Evento del botón nuevo
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
            {
                Configuracion_Formulario(false);
                Btn_Nuevo.AlternateText = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Modificar.Visible = false;
            }
            else if (Validar_Componentes())
            {
                Cls_Ope_Cat_Recepcion_Oficios_Negocio Oficio = new Cls_Ope_Cat_Recepcion_Oficios_Negocio();
                Oficio.P_Departamento_Catastro = Cmb_Dep_Catastro.SelectedValue;
                Oficio.P_Dependencia = Txt_Dependencia.Text.ToUpper();
                Oficio.P_Descripcion = Txt_Descripcion.Text.ToUpper();
                Oficio.P_Fecha_Recepcion = Convert.ToDateTime(Txt_Fecha_Oficio.Text);
                Oficio.P_Hora_Recepcion = Cmb_Hora.SelectedValue+":"+Cmb_Minutos.SelectedValue+":"+Cmb_Segundos.SelectedValue;
                Oficio.P_No_Oficio_Recepcion = Txt_No_Oficio_Recepcion.Text.ToUpper();
                if ((Oficio.Alta_Oficios()))
                {
                    Configuracion_Formulario(true);
                    Llenar_Tabla_Oficios(Grid_Oficios.PageIndex);
                    Btn_Nuevo.AlternateText = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.Visible = true;
                    Btn_Modificar.AlternateText = "Modificar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Grid_Oficios.SelectedIndex = -1;
                    Txt_Dependencia.Text = "";
                    Txt_Descripcion.Text = "";
                    Txt_Fecha_Oficio.Text = "";
                    Dtp_Fecha_Oficio.SelectedDate = null;
                    Cmb_Dep_Catastro.SelectedIndex = 0;
                    Cmb_Hora.SelectedValue = "00";
                    Cmb_Minutos.SelectedValue = "00";
                    Cmb_Segundos.SelectedValue = "00";
                    Txt_No_Oficio_Recepcion.Text = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Recepción de oficios", "alert('Alta de Oficio Exitosa');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Recepción de oficios", "alert('Alta de Oficio Errónea');", true);
                }
            }
        }
        catch (Exception E)
        {
            Lbl_Ecabezado_Mensaje.Text = E.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Evento del botón modificar
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Grid_Oficios.SelectedIndex > -1)
            {
                if (Btn_Modificar.AlternateText.Equals("Modificar"))
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
                    if (Validar_Componentes())
                    {
                        Cls_Ope_Cat_Recepcion_Oficios_Negocio Oficio = new Cls_Ope_Cat_Recepcion_Oficios_Negocio();
                        Oficio.P_No_Oficio = Hdf_No_Oficio.Value;
                        Oficio.P_Departamento_Catastro = Cmb_Dep_Catastro.SelectedValue;
                        Oficio.P_Dependencia = Txt_Dependencia.Text.ToUpper();
                        Oficio.P_Descripcion = Txt_Descripcion.Text.ToUpper();
                        Oficio.P_Fecha_Recepcion = Convert.ToDateTime(Txt_Fecha_Oficio.Text);
                        Oficio.P_Hora_Recepcion = Cmb_Hora.SelectedValue + ":" + Cmb_Minutos.SelectedValue + ":" + Cmb_Segundos.SelectedValue;
                        Oficio.P_No_Oficio_Recepcion = Txt_No_Oficio_Recepcion.Text.ToUpper();
                        if ((Oficio.Modificar_Oficio()))
                        {
                            Configuracion_Formulario(true);
                            Llenar_Tabla_Oficios(Grid_Oficios.PageIndex);
                            Btn_Modificar.AlternateText = "Modificar";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                            Btn_Nuevo.Visible = true;
                            Btn_Nuevo.AlternateText = "Nuevo";
                            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                            Btn_Salir.AlternateText = "Salir";
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                            Grid_Oficios.SelectedIndex = -1;
                            Txt_Dependencia.Text = "";
                            Txt_Descripcion.Text = "";
                            Txt_Fecha_Oficio.Text = "";
                            Dtp_Fecha_Oficio.SelectedDate = null;
                            Cmb_Dep_Catastro.SelectedIndex = 0;
                            Cmb_Hora.SelectedValue = "00";
                            Cmb_Minutos.SelectedValue = "00";
                            Cmb_Segundos.SelectedValue = "00";
                            Txt_No_Oficio_Recepcion.Text = "";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Recepción de Oficios", "alert('Actualización del Oficio Exitosa.');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Recepción de Oficios", "alert('Error al Actualizar el Oficio.');", true);
                        }
                    }
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Seleccione el Oficio a Modificar.";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Evento del botón salir
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText.Equals("Salir"))
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Configuracion_Formulario(true);
            Btn_Nuevo.Visible = true;
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            Btn_Modificar.Visible = true;
            Btn_Modificar.AlternateText = "Modificar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            Llenar_Tabla_Oficios(Grid_Oficios.PageIndex);
            Grid_Oficios.SelectedIndex = -1;
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Txt_Dependencia.Text = "";
            Txt_Descripcion.Text = "";
            Txt_Fecha_Oficio.Text = "";
            Dtp_Fecha_Oficio.SelectedDate = null;
            Cmb_Dep_Catastro.SelectedIndex = 0;
            Cmb_Hora.SelectedValue = "00";
            Cmb_Minutos.SelectedValue = "00";
            Cmb_Segundos.SelectedValue = "00";
            Txt_No_Oficio_Recepcion.Text = "";
        }
    }

    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Llenar_Tabla_Oficios(0);
    }

    protected void Grid_Oficios_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Llenar_Tabla_Oficios(e.NewPageIndex);
    }

    protected void Grid_Oficios_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Oficios.SelectedIndex > -1)
        {
            String[] Hora_Recepcion;
            Hdf_No_Oficio.Value = Grid_Oficios.SelectedRow.Cells[1].Text;
            Dtp_Fecha_Oficio.SelectedDate = Convert.ToDateTime(Grid_Oficios.SelectedRow.Cells[3].Text);
            Hora_Recepcion = Grid_Oficios.SelectedRow.Cells[4].Text.Split(':');
            Cmb_Hora.SelectedValue = Hora_Recepcion[0];
            Cmb_Minutos.SelectedValue = Hora_Recepcion[1];
            Cmb_Segundos.SelectedValue = Hora_Recepcion[2];
            Cmb_Dep_Catastro.SelectedValue = Grid_Oficios.SelectedRow.Cells[5].Text;
            Txt_Dependencia.Text = Grid_Oficios.SelectedRow.Cells[6].Text;
            Txt_Descripcion.Text = Grid_Oficios.SelectedRow.Cells[7].Text;
            Txt_No_Oficio_Recepcion.Text = Grid_Oficios.SelectedRow.Cells[2].Text;
        }
    }

    private Boolean Validar_Componentes()
    {
        String Mensaje_Error = "Error: ";
        Boolean valido = true;

        if (Txt_Fecha_Oficio.Text.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Seleccione la Fecha de la Recepción del oficio.";
            valido = false;
        }
        if (Txt_Dependencia.Text.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese la dependencia.";
            valido = false;
        }
        if (Cmb_Dep_Catastro.SelectedValue == "SELECCIONE")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Seleccione al Departamento de Catastro al cúal va dirigido el Oficio.";
            valido = false;
        }

        if (!valido)
        {
            Lbl_Ecabezado_Mensaje.Text = Mensaje_Error;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return valido;
    }
    
}