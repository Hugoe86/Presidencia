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
using Presidencia.Operacion_Cat_Recepcion_Oficios.Negocio;
using Presidencia.Operacion_Cat_Respuesta_Oficios.Negocios;
using Presidencia.Operacion_Cat_Respuesta_Oficios.Datos;


public partial class paginas_Catastro_Frm_Ope_Cat_Respuesta_Oficios : System.Web.UI.Page
{
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
                    //Btn_Nuevo.Visible = false;
                }
                else
                {
                    if (Validar_Componentes())
                    {
                        Cls_Ope_Cat_Recepcion_Oficios_Negocio Oficio = new Cls_Ope_Cat_Recepcion_Oficios_Negocio();
                        Oficio.P_No_Oficio = Hdf_No_Oficio.Value;
                        Oficio.P_Fecha_Respuesta = Convert.ToDateTime(Txt_Fecha_Respuesta_Oficio.Text);
                        Oficio.P_Hora_Respuesta = Cmb_Hora_Respuesta.SelectedValue + ":" + Cmb_Minutos_Respuesta.SelectedValue + ":" + Cmb_Segundos_Respuesta.SelectedValue;
                        Oficio.P_No_Oficio_Respuesta = Txt_No_Oficio_Respuesta.Text.ToUpper();
                        if ((Oficio.Modificar_Oficio_Respuesta()))
                        {
                            Configuracion_Formulario(true);
                            Llenar_Tabla_Oficios(Grid_Oficios.PageIndex);
                            Btn_Modificar.AlternateText = "Modificar";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                            //Btn_Nuevo.Visible = true;
                            //Btn_Nuevo.AlternateText = "Nuevo";
                            //Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
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
                            Txt_Fecha_Respuesta_Oficio.Text = "";
                            Dtp_Fecha_Respuesta.SelectedDate = null;
                            Cmb_Hora_Respuesta.SelectedValue = "00";
                            Cmb_Minutos_Respuesta.SelectedValue = "00";
                            Cmb_Segundos_Respuesta.SelectedValue = "00";
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


    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Llenar_Tabla_Oficios(0);
    }

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
            //Respuesta
            Cmb_Hora_Respuesta.DataTextField = "TEXT";
            Cmb_Hora_Respuesta.DataValueField = "VALUE";
            Cmb_Hora_Respuesta.DataSource = Dt_Horas.Copy();
            Cmb_Hora_Respuesta.DataBind();
            Cmb_Minutos_Respuesta.DataTextField = "TEXT";
            Cmb_Minutos_Respuesta.DataValueField = "VALUE";
            Cmb_Minutos_Respuesta.DataSource = Dt_Minutos.Copy();
            Cmb_Minutos_Respuesta.DataBind();
            Cmb_Segundos_Respuesta.DataTextField = "TEXT";
            Cmb_Segundos_Respuesta.DataValueField = "VALUE";
            Cmb_Segundos_Respuesta.DataSource = Dt_Segundos.Copy();
            Cmb_Segundos_Respuesta.DataBind();

        }
        catch (Exception E)
        {
            Lbl_Ecabezado_Mensaje.Text = E.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    private void Configuracion_Formulario(Boolean Enabled)
    {
        Txt_Fecha_Oficio.Enabled = false;
        Txt_Dependencia.Enabled = false;
        Txt_Descripcion.Enabled = false;
        Cmb_Dep_Catastro.Enabled = false;
        Cmb_Hora.Enabled = false;
        Cmb_Minutos.Enabled = false;
        Cmb_Segundos.Enabled = false;
        Btn_Buscar.Enabled = Enabled;
        Txt_Busqueda.Enabled = Enabled;
        Grid_Oficios.Enabled = Enabled;
        Twe_Fecha_Oficio.Enabled = false;
        Twe_Fecha_Respuesta.Enabled = !Enabled;
        Txt_Fecha_Respuesta_Oficio.Enabled = false;
        Dtp_Fecha_Oficio.Enabled = false;
        Dtp_Fecha_Respuesta.Enabled = !Enabled;
        Cmb_Hora_Respuesta.Enabled = !Enabled;
        Cmb_Minutos_Respuesta.Enabled = !Enabled;
        Cmb_Segundos_Respuesta.Enabled = !Enabled;
        Txt_No_Oficio_Respuesta.Enabled = !Enabled;
        Txt_No_Oficio_Respuesta.Style["text-align"] = "Right";
        Txt_No_Oficio_Recepcion.Enabled = false;
        Txt_No_Oficio_Recepcion.Style["text-align"] = "Right";
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
            if (Txt_Busqueda.Text.Trim() != "")
            {
                if (Cmb_Filtro_Busqueda.SelectedValue == "FOLIO_RECEPCION")
                {
                    Oficios.P_No_Oficio_Recepcion = Txt_Busqueda.Text.ToUpper();
                }
                else if (Cmb_Filtro_Busqueda.SelectedValue == "FOLIO_RESPUESTA")
                {
                    Oficios.P_No_Oficio_Respuesta = Txt_Busqueda.Text.ToUpper();
                }
                else if (Cmb_Filtro_Busqueda.SelectedValue == "DEPARTAMENTO")
                {
                    Oficios.P_Departamento_Catastro = Txt_Busqueda.Text.ToUpper();
                }
            }
            Dt_Oficios = Oficios.Consultar_Oficios();
            Grid_Oficios.Columns[1].Visible = true;
            Grid_Oficios.Columns[10].Visible = true;
            Grid_Oficios.DataSource = Dt_Oficios;
            Grid_Oficios.PageIndex = Pagina;
            Grid_Oficios.DataBind();
            Grid_Oficios.Columns[1].Visible = false;
            Grid_Oficios.Columns[10].Visible = false;
        }
        catch (Exception E)
        {
            Lbl_Ecabezado_Mensaje.Text = E.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
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
            Configuracion_Formulario(true);
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
            Txt_No_Oficio_Respuesta.Text = "";
            Dtp_Fecha_Oficio.SelectedDate = null;
            Cmb_Dep_Catastro.SelectedIndex = 0;
            Cmb_Hora.SelectedValue = "00";
            Cmb_Minutos.SelectedValue = "00";
            Cmb_Segundos.SelectedValue = "00";
            Txt_Fecha_Respuesta_Oficio.Text = "";
            Dtp_Fecha_Respuesta.SelectedDate = null;
            Cmb_Hora_Respuesta.SelectedValue = "00";
            Cmb_Minutos_Respuesta.SelectedValue = "00";
            Cmb_Segundos_Respuesta.SelectedValue = "00";
            
        }
    }

    protected void Grid_Oficios_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Llenar_Tabla_Oficios(e.NewPageIndex);
    }

    protected void Grid_Oficios_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Oficios.SelectedIndex > -1)
        {
            Txt_Fecha_Respuesta_Oficio.Text = "";
            Dtp_Fecha_Respuesta.SelectedDate = null;
            Cmb_Hora_Respuesta.SelectedValue = "00";
            Cmb_Minutos_Respuesta.SelectedValue = "00";
            Cmb_Segundos_Respuesta.SelectedValue = "00";
            String[] Hora_Recepcion;
            Hdf_No_Oficio.Value = Grid_Oficios.SelectedRow.Cells[1].Text;
            Dtp_Fecha_Oficio.SelectedDate = Convert.ToDateTime(Grid_Oficios.SelectedRow.Cells[3].Text);
            Txt_Fecha_Oficio.Text = Convert.ToDateTime(Grid_Oficios.SelectedRow.Cells[3].Text).ToString("dd/MMM/yyyy");
            Hora_Recepcion = Grid_Oficios.SelectedRow.Cells[4].Text.Split(':');
            Cmb_Hora.SelectedValue = Hora_Recepcion[0];
            Cmb_Minutos.SelectedValue = Hora_Recepcion[1];
            Cmb_Segundos.SelectedValue = Hora_Recepcion[2];
            if (HttpUtility.HtmlDecode(Grid_Oficios.SelectedRow.Cells[6].Text).Trim() != "")
            {
                Dtp_Fecha_Respuesta.SelectedDate = Convert.ToDateTime(HttpUtility.HtmlDecode(Grid_Oficios.SelectedRow.Cells[6].Text));
                Txt_Fecha_Respuesta_Oficio.Text = Convert.ToDateTime(HttpUtility.HtmlDecode(Grid_Oficios.SelectedRow.Cells[6].Text)).ToString("dd/MMM/yyyy");
            }
            Hora_Recepcion = HttpUtility.HtmlDecode(Grid_Oficios.SelectedRow.Cells[7].Text).Split(':');
            if (Hora_Recepcion.Length > 1)
            {
                Cmb_Hora_Respuesta.SelectedValue = Hora_Recepcion[0];
                Cmb_Minutos_Respuesta.SelectedValue = Hora_Recepcion[1];
                Cmb_Segundos_Respuesta.SelectedValue = Hora_Recepcion[2];
            }
            Cmb_Dep_Catastro.SelectedValue = Grid_Oficios.SelectedRow.Cells[8].Text;
            Txt_Dependencia.Text = Grid_Oficios.SelectedRow.Cells[9].Text;
            Txt_Descripcion.Text = Grid_Oficios.SelectedRow.Cells[10].Text;
            if (Grid_Oficios.SelectedRow.Cells[2].Text != "" && Grid_Oficios.SelectedRow.Cells[2].Text != "nbsp;" )
            {
                Txt_No_Oficio_Recepcion.Text = Grid_Oficios.SelectedRow.Cells[2].Text;
                
            }
            else
            {
                Txt_No_Oficio_Recepcion.Text = "";
                
 
            }


            if (Grid_Oficios.SelectedRow.Cells[5].Text != "" && Grid_Oficios.SelectedRow.Cells[5].Text != "&nbsp;")
            {
                Txt_No_Oficio_Respuesta.Text = Grid_Oficios.SelectedRow.Cells[5].Text;
               
            }

            else
            {
                Txt_No_Oficio_Respuesta.Text = "";
            }
            
        }
    }

    private Boolean Validar_Componentes()
    {
        String Mensaje_Error = "Error: ";
        Boolean valido = true;

        if (Txt_Fecha_Respuesta_Oficio.Text.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Seleccione la Fecha de la Respuesta del oficio.";
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
