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
using Presidencia.Constantes;
using Presidencia.Catalogo_Cat_Tasas.Datos;
using Presidencia.Catalogo_Cat_Tasas.Negocio;

public partial class paginas_Catastro_Frm_Cat_Cat_Tasas : System.Web.UI.Page
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
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) 
                Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Configuracion_Formulario(true);
                Llenar_Tabla_Tipos(0);
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
        Txt_Anio.Enabled = !Habilitado;
        Txt_Con_Edificacion.Enabled = !Habilitado;
        Txt_Sin_Edificacion.Enabled = !Habilitado;
        Txt_Valor_Rustico.Enabled = !Habilitado;
        Btn_Buscar.Enabled = !Habilitado;
        Txt_Busqueda.Enabled = Habilitado;
        Grid_Tasas.Enabled = Habilitado;
        
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
    private void Llenar_Tabla_Tipos(int Pagina)
    {
        try
        {
            Cls_Cat_Cat_Tasas_Negocio Tipo = new Cls_Cat_Cat_Tasas_Negocio();
            if (Txt_Busqueda.Text.Trim() != "")
            {
                Tipo.P_Anio = Txt_Anio.Text;
                Tipo.P_Con_Edificacion = Txt_Con_Edificacion.Text;
                Tipo.P_Sin_Edificacion = Txt_Sin_Edificacion.Text;
                Tipo.P_Valor_Rustico = Txt_Valor_Rustico.Text;
            }
            Grid_Tasas.Columns[1].Visible = true;
            Grid_Tasas.DataSource = Tipo.Consultar_Tasa();
            Grid_Tasas.PageIndex = Pagina;
            Grid_Tasas.DataBind();
            Grid_Tasas.Columns[1].Visible = false;

        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }


    private Boolean Validar_Anio_Duplicado(String Anio) {
        Boolean Validacion = true;
        Cls_Cat_Cat_Tasas_Negocio Tipo = new Cls_Cat_Cat_Tasas_Negocio();
        Tipo.P_Anio = Anio.Trim();
        DataTable Dt_Resultados = Tipo.Consultar_Tasa();
        if (Dt_Resultados != null) {
            if (Dt_Resultados.Rows.Count > 0) {
                if (!Hdf_Id_Tasa.Value.Trim().Equals(Dt_Resultados.Rows[0][Cat_Cat_Tasas.Campo_Id_Tasa].ToString().Trim())) {
                    Validacion = false;
                }
            }
        }
        return Validacion;
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
                    Txt_Anio.Text = "";
                    Txt_Con_Edificacion.Text = "";
                    Txt_Sin_Edificacion.Text = "";
                    Txt_Valor_Rustico.Text = "";
                    Txt_Busqueda.Text = "";
                }
                else if (Validar_Componentes())
                {
                    Cls_Cat_Cat_Tasas_Negocio Tipo = new Cls_Cat_Cat_Tasas_Negocio();
                    Tipo.P_Anio = Txt_Anio.Text;
                    Tipo.P_Con_Edificacion = Txt_Con_Edificacion.Text;
                    Tipo.P_Sin_Edificacion = Txt_Sin_Edificacion.Text;
                    Tipo.P_Valor_Rustico = Txt_Valor_Rustico.Text;
                    if ((Tipo.Alta_Tasa()))
                    {
                        Div_Grid_Tab_Val.Visible = true;
                        Llenar_Tabla_Tipos(0);
                        Configuracion_Formulario(true);
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Modificar.Visible = true;
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Grid_Tasas.SelectedIndex = -1;
                        Txt_Anio.Text = "";
                        Txt_Busqueda.Text = "";
                        Txt_Con_Edificacion.Text = "";
                        Txt_Sin_Edificacion.Text = "";
                        Txt_Valor_Rustico.Text = "";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tasas", "alert('Alta Exitosa');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tasas", "alert('Alta Errónea');", true);
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
            if (Grid_Tasas.SelectedIndex > -1)
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
                        Cls_Cat_Cat_Tasas_Negocio Tipo = new Cls_Cat_Cat_Tasas_Negocio();
                        Tipo.P_Anio = Txt_Anio.Text;
                        Tipo.P_Con_Edificacion = Txt_Con_Edificacion.Text;
                        Tipo.P_Sin_Edificacion = Txt_Sin_Edificacion.Text;
                        Tipo.P_Valor_Rustico = Txt_Valor_Rustico.Text;
                        Tipo.P_Id_Tasas = Hdf_Id_Tasa.Value;
                        if ((Tipo.Modificar_Tasa()))
                        {
                            Txt_Busqueda.Text = "";
                            Configuracion_Formulario(true);
                            Llenar_Tabla_Tipos(Grid_Tasas.PageIndex);
                            Btn_Modificar.AlternateText = "Modificar";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                            Btn_Nuevo.Visible = true;
                            Btn_Nuevo.AlternateText = "Nuevo";
                            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                            Btn_Salir.AlternateText = "Salir";
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                            Grid_Tasas.SelectedIndex = -1;
                            Txt_Anio.Text = "";
                            Txt_Con_Edificacion.Text = "";
                            Txt_Sin_Edificacion.Text = "";
                            Txt_Valor_Rustico.Text = "";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), 
                                "Catalogo de Tasas", "alert('Actualización Exitosa.');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(),
                                "Catalogo de Tasas", "alert('Error al intentar Actualizar.');", true);
                        }
                    }
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Seleccione Tasa a modificar.";
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
            Llenar_Tabla_Tipos(Grid_Tasas.PageIndex);
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Grid_Tasas.SelectedIndex = -1;
            Txt_Anio.Text = "";
            Txt_Con_Edificacion.Text = "";
            Txt_Sin_Edificacion.Text = "";
            Txt_Valor_Rustico.Text = "";
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
        Llenar_Tabla_Tipos(0);
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
    protected void Grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Llenar_Tabla_Tipos(e.NewPageIndex);
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
    protected void Grid_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Tasas.SelectedIndex > -1)
        {
            Hdf_Id_Tasa.Value = Grid_Tasas.SelectedRow.Cells[1].Text;
            Txt_Anio.Text = Grid_Tasas.SelectedRow.Cells[2].Text;
            Txt_Con_Edificacion.Text= Grid_Tasas.SelectedRow.Cells[3].Text;
            Txt_Sin_Edificacion.Text = Grid_Tasas.SelectedRow.Cells[4].Text;
            Txt_Valor_Rustico.Text = Grid_Tasas.SelectedRow.Cells[5].Text;
            
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
        
       if (Txt_Anio.Text.Trim() == "")
       {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese el Año.";
            valido = false;
         
        }
       if (Txt_Con_Edificacion.Text.Trim() == "")
       {
           if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese Con Edificacion.";
            valido = false;
       }
       if (Txt_Sin_Edificacion.Text.Trim() == "")
       {
           if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese Sin Edificacion.";
            valido = false;
       }

       if (Txt_Valor_Rustico.Text.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese el Valor Rustico.";
            valido = false;
        }
       if (Txt_Anio.Text.Trim().Length > 0) 
       {
           if (!Validar_Anio_Duplicado(Txt_Anio.Text))
           {
               if (Mensaje_Error.Length > 0)
               {
                   Mensaje_Error += "<br/>";
               }
               Mensaje_Error += "+ El Año ya esta.";
               valido = false;
           }
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

