using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Cat_Calidad_Construccion.Negocio;
using Presidencia.Catalogo_Cat_Tipos_Construccion.Negocio;
using System.Data;
using Presidencia.Constantes;

public partial class paginas_Catastro_Frm_Cat_Cat_Calidad_Construccion : System.Web.UI.Page
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
                Llenar_Tabla_Calidad(0);
                Llenar_Combo_Tipos_Construccion();
                Txt_Calidad.Attributes.Add("OnKeyPress", " ValidarCaracteres(this,50)");
                Txt_Calidad.Attributes.Add("OnKeyUp", " ValidarCaracteres(this,50)");
                Txt_Calidad.Attributes.Add("OnKeyPress", " ValidarCaracteres(this,50)");
                Txt_Calidad.Attributes.Add("OnKeyUp", " ValidarCaracteres(this,50)");
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
    private void Configuracion_Formulario(Boolean Habilitado)
    {
        Txt_Calidad.Enabled = !Habilitado;
        Txt_Clave_Calidad.Enabled = !Habilitado;
        Btn_Buscar.Enabled = Habilitado;
        Txt_Busqueda.Enabled = Habilitado;
        Txt_Clave_Calidad.Style["text-align"] = "Right";
        Cmb_Tipos_Construccion.Enabled = !Habilitado;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Calidad
    ///DESCRIPCIÓN: Llena la tabla de los datos de calidad
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 07/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Calidad(int Pagina)
    {
        try
        {
            Cls_Cat_Cat_Calidad_Construccion_Negocio Motivos = new Cls_Cat_Cat_Calidad_Construccion_Negocio();
            if (Txt_Busqueda.Text.Trim() != "")
            {
                Motivos.P_Calidad = Txt_Busqueda.Text.ToUpper();
            }
            Grid_Calidad.Columns[1].Visible = true;
            Grid_Calidad.Columns[2].Visible = true;
            Grid_Calidad.DataSource = Motivos.Consultar_Calidad_Construccion();
            Grid_Calidad.PageIndex = Pagina;
            Grid_Calidad.DataBind();
            Grid_Calidad.Columns[1].Visible = false;
            Grid_Calidad.Columns[2].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Tipos_Construccion
    ///DESCRIPCIÓN: Llena el combo de Tipos de Construccion
    ///PROPIEDADES:         
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 24/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Tipos_Construccion()
    {
        try
        {
            Cls_Cat_Cat_Tipos_Construccion_Negocio Tipos_Construccion = new Cls_Cat_Cat_Tipos_Construccion_Negocio();
            Tipos_Construccion.P_Estatus = "='VIGENTE' ";
            DataTable tabla = Tipos_Construccion.Consultar_Tipos_Construccion();
            DataRow fila = tabla.NewRow();
            fila[Cat_Cat_Tipos_Construccion.Campo_Tipo_Construccion_Id] = "SELECCIONE";
            fila[Cat_Cat_Tipos_Construccion.Campo_Identificador] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            tabla.Rows.InsertAt(fila, 0);
            Cmb_Tipos_Construccion.DataSource = tabla;
            Cmb_Tipos_Construccion.DataValueField = Cat_Cat_Tipos_Construccion.Campo_Tipo_Construccion_Id;
            Cmb_Tipos_Construccion.DataTextField = Cat_Cat_Tipos_Construccion.Campo_Identificador;
            Cmb_Tipos_Construccion.DataBind();
        }
        catch (Exception Ex)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Ecabezado_Mensaje.Visible = true;
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
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
                Txt_Calidad.Text = "";
                Txt_Clave_Calidad.Text = "";
                Cmb_Tipos_Construccion.SelectedIndex = -1;
            }
            else if (Validar_Componentes())
            {
                Cls_Cat_Cat_Calidad_Construccion_Negocio Calidad = new Cls_Cat_Cat_Calidad_Construccion_Negocio();
                Calidad.P_Calidad = Txt_Calidad.Text.ToUpper();
                Calidad.P_Clave_Calidad = Txt_Clave_Calidad.Text.Trim();
                Calidad.P_Tipo_Construccion_Id = Cmb_Tipos_Construccion.SelectedValue;
                if ((Calidad.Alta_Calidad_Construccion()))
                {
                    Txt_Busqueda.Text = "";
                    Llenar_Tabla_Calidad(0);
                    Configuracion_Formulario(true);
                    Btn_Nuevo.AlternateText = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.Visible = true;
                    Btn_Modificar.AlternateText = "Modificar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Grid_Calidad.SelectedIndex = -1;
                    Cmb_Tipos_Construccion.SelectedIndex = -1;
                    Txt_Calidad.Text = "";
                    Txt_Clave_Calidad.Text = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Calidad de la Construcción", "alert('Alta Exitosa');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Calidad de la Construcción", "alert('Alta Errónea');", true);
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
            if (Grid_Calidad.SelectedIndex > -1)
            {
                if (Btn_Modificar.AlternateText.Equals("Modificar"))
                {
                    Configuracion_Formulario(false);
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.Visible = false;
                    Cmb_Tipos_Construccion.Enabled = false;
                }
                else
                {
                    if (Validar_Componentes())
                    {
                        Cls_Cat_Cat_Calidad_Construccion_Negocio Calidad = new Cls_Cat_Cat_Calidad_Construccion_Negocio();
                        Calidad.P_Calidad = Txt_Calidad.Text.ToUpper(); ;
                        Calidad.P_Clave_Calidad = Txt_Clave_Calidad.Text.Trim();
                        Calidad.P_Calidad_Id = Hdf_Calidad_Id.Value;
                        if ((Calidad.Modificar_Calidad_Construccion()))
                        {
                            Txt_Busqueda.Text = "";
                            Configuracion_Formulario(true);
                            Llenar_Tabla_Calidad(Grid_Calidad.PageIndex);
                            Btn_Modificar.AlternateText = "Modificar";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                            Btn_Nuevo.Visible = true;
                            Btn_Nuevo.AlternateText = "Nuevo";
                            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                            Btn_Salir.AlternateText = "Salir";
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                            Grid_Calidad.SelectedIndex = -1;
                            Cmb_Tipos_Construccion.SelectedIndex = -1;
                            Txt_Clave_Calidad.Text = "";
                            Txt_Calidad.Text = "";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Calidad de Construcción", "alert('Actualización Exitosa.');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Calidad de Construcción", "alert('Error al intentar Actualizar.');", true);
                        }
                    }
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Seleccione la Calidad de Construcción a modificar.";
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
            Btn_Nuevo.Visible = true;
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            Btn_Modificar.Visible = true;
            Btn_Modificar.AlternateText = "Modificar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            Configuracion_Formulario(true);
            Llenar_Tabla_Calidad(Grid_Calidad.PageIndex);
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Grid_Calidad.SelectedIndex = -1;
            Cmb_Tipos_Construccion.SelectedIndex = -1;
            Txt_Calidad.Text = "";
            Txt_Clave_Calidad.Text = "";
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
    ///DESCRIPCIÓN: Evento del botón buscar
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Llenar_Tabla_Calidad(0);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Calidad_PageIndexChanging
    ///DESCRIPCIÓN: Cambia la página del grid
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Calidad_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Llenar_Tabla_Calidad(e.NewPageIndex);
        Hdf_Calidad_Id.Value ="";
        Txt_Calidad.Text = "";
        Txt_Clave_Calidad.Text = "";
        Cmb_Tipos_Construccion.SelectedIndex = -1;
        Grid_Calidad.SelectedIndex = -1;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Calidad_SelectedIndexChanged
    ///DESCRIPCIÓN: Selecciona un registro del grid y toma los datos de los mismos campos del componente
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Calidad_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Calidad.SelectedIndex > -1)
        {
            Hdf_Calidad_Id.Value = Grid_Calidad.SelectedRow.Cells[1].Text;
            Txt_Calidad.Text = Grid_Calidad.SelectedRow.Cells[4].Text;
            Txt_Clave_Calidad.Text = Grid_Calidad.SelectedRow.Cells[5].Text;
            Cmb_Tipos_Construccion.SelectedValue = Grid_Calidad.SelectedRow.Cells[2].Text;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes
    ///DESCRIPCIÓN: Valida que los componentes tengan los datos necesarios para ingresarlos a la BD.
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 22/May/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Componentes()
    {
        String Mensaje_Error = "Error: ";
        Boolean valido = true;

        if (Cmb_Tipos_Construccion.SelectedIndex == -1)
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Seleccione el Tipo de Construcción.";
            valido = false;
        }
        if (Txt_Calidad.Text.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese la Calidad de la Construcción.";
            valido = false;
        }
        if (Txt_Clave_Calidad.Text.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese la Clave de Calidad.";
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