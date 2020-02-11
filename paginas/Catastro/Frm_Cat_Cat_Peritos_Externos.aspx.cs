using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Cat_Peritos_Externos.Negocio;
using Presidencia.Constantes;
using System.Data;
using Presidencia.Operacion_Cat_Recepcion_Documentos_Perito_Externo.Negocio;
using System.IO;

public partial class paginas_Catastro_Frm_Cat_Cat_Peritos_Externos : System.Web.UI.Page
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
                Llenar_Tabla_Peritos_Externos(0);
                Txt_Observaciones.Attributes.Add("OnKeyPress", " ValidarCaracteres(this,250)");
                Txt_Observaciones.Attributes.Add("OnKeyUp", " ValidarCaracteres(this,250)");
                Txt_Observaciones.Attributes.Add("OnKeyPress", " ValidarCaracteres(this,250)");
                Txt_Observaciones.Attributes.Add("OnKeyUp", " ValidarCaracteres(this,250)");
            }
        }
        catch (Exception ex)
        {
            Lbl_Ecabezado_Mensaje.Text = ex.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Img_Error.Visible = true;
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
        Txt_Apellido_Materno.Enabled = !Enabled;
        Txt_Apellido_Paterno.Enabled = !Enabled;
        Txt_Calle.Enabled = !Enabled;
        Txt_Celular.Enabled = !Enabled;
        Txt_Ciudad.Enabled = !Enabled;
        Txt_Colonia.Enabled = !Enabled;
        Txt_Estado.Enabled = !Enabled;
        Txt_Nombre.Enabled = !Enabled;
        Txt_Observaciones.Enabled = !Enabled;
        Txt_Password.Enabled = !Enabled;
        Txt_Password_Confirma.Enabled = !Enabled;
        Txt_Telefono.Enabled = !Enabled;
        Txt_Usuario.Enabled = !Enabled;
        Txt_Busqueda.Enabled = Enabled;
        Grid_Peritos_Externos.Enabled = Enabled;
        Btn_Buscar.Enabled = Enabled;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Campos
    ///DESCRIPCIÓN: Limpia todos los campos del formulario
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Campos()
    {
        int Anio = DateTime.Now.Year;
        Txt_Apellido_Materno.Text = "";
        Txt_Apellido_Paterno.Text = "";
        Txt_Calle.Text = "";
        Txt_Celular.Text = "";
        Txt_Ciudad.Text = "";
        Txt_Colonia.Text = "";
        Txt_Estado.Text = "";
        Txt_Nombre.Text = "";
        Txt_Observaciones.Text = "";
        Txt_Password.Text = "";
        Txt_Password.Attributes.Add("value", "");
        Txt_Password_Confirma.Text = "";
        Txt_Password_Confirma.Attributes.Add("value", "");
        Txt_Telefono.Text = "";
        Txt_Usuario.Text = "";
        Txt_Busqueda.Text = "";
        Cmb_Estatus.SelectedValue = "SELECCIONE";
        Hdf_Perito_Externo.Value = "";
        Txt_Fecha.Text = "31-Dic-"+Anio;
        
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
    private void Llenar_Tabla_Peritos_Externos(int Pagina)
    {
        try
        {
            Cls_Cat_Cat_Peritos_Externos_Negocio Peritos_Externos = new Cls_Cat_Cat_Peritos_Externos_Negocio();
            DataTable Dt_Peritos_Externos;
            if (Txt_Busqueda.Text.Trim() != "")
            {
                Peritos_Externos.P_Nombre = Txt_Busqueda.Text.ToUpper();
            }
            Dt_Peritos_Externos = Peritos_Externos.Consultar_Peritos_Externos();
            Grid_Peritos_Externos.Columns[1].Visible = true;
            Grid_Peritos_Externos.Columns[2].Visible = true;
            Grid_Peritos_Externos.DataSource = Dt_Peritos_Externos;
            Grid_Peritos_Externos.PageIndex = Pagina;
            Grid_Peritos_Externos.DataBind();
            Grid_Peritos_Externos.Columns[1].Visible = false;    
            
        }
        catch (Exception E)
        {
            Lbl_Ecabezado_Mensaje.Text = E.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Img_Error.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Documentos
    ///DESCRIPCIÓN: Establece un objeto de documentos, muestra sus datos en el grid 
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Documentos(int Pagina)
    {
        try
        {
            Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio Documentos = new Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio();
            DataTable Dt_Documentos;
            Documentos.P_Perito_Externo_Id = Hdf_Perito_Externo.Value;
            Dt_Documentos = Documentos.Consultar_Documentos_Perito_Externo();
            Session["Dt_Documentos"] = Dt_Documentos.Copy();
            Grid_Documentos.Columns[0].Visible = true;
            Grid_Documentos.Columns[1].Visible = true;
            Grid_Documentos.DataSource = Dt_Documentos;
            Grid_Documentos.PageIndex = Pagina;
            Grid_Documentos.DataBind();
            Grid_Documentos.Columns[0].Visible = false;
            Grid_Documentos.Columns[1].Visible = false;
        }
        catch (Exception E)
        {
            Lbl_Ecabezado_Mensaje.Text = E.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Img_Error.Visible = true;
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
                Limpiar_Campos();
                Btn_Nuevo.AlternateText = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Modificar.Visible = false;
                Div_Grid_Datos_Peritos.Visible = true;
                Div_Grid_Peritos.Visible = true;
                Div_Seguridad_Peritos_Externos.Visible = true;
                Cmb_Estatus.SelectedValue = "VIGENTE";
                Btn_Eliminar.Visible = false;                
            }
            else if (Validar_Componentes())
            {
                Cls_Cat_Cat_Peritos_Externos_Negocio Perito = new Cls_Cat_Cat_Peritos_Externos_Negocio();
                Perito.P_Apellido_Materno = Txt_Apellido_Materno.Text.ToUpper();
                Perito.P_Apellido_Paterno = Txt_Apellido_Paterno.Text.ToUpper();
                Perito.P_Calle = Txt_Calle.Text.ToUpper();
                Perito.P_Celular = Txt_Celular.Text;
                Perito.P_Ciudad = Txt_Ciudad.Text.ToUpper();
                Perito.P_Colonia = Txt_Colonia.Text.ToUpper();
                Perito.P_Estado = Txt_Estado.Text.ToUpper();
                Perito.P_Estatus = Cmb_Estatus.SelectedValue;
                Perito.P_Nombre = Txt_Nombre.Text.ToUpper();
                Perito.P_Observaciones = Txt_Observaciones.Text.ToUpper();
                Perito.P_Password = Txt_Password.Text;
                Perito.P_Telefono = Txt_Telefono.Text;
                Perito.P_Usuario = Txt_Usuario.Text;
                if ((Perito.Alta_Perito_Externo()))
                {
                    String Url = Server.MapPath("../Catastro/Archivos");
                    System.IO.Directory.CreateDirectory(Url + "/" + Perito.P_Perito_Externo_Id);
                    Div_Grid_Peritos.Visible = true;
                    Div_Grid_Datos_Peritos.Visible = false;
                    Configuracion_Formulario(true);
                    Llenar_Tabla_Peritos_Externos(Grid_Peritos_Externos.PageIndex);
                    Btn_Nuevo.AlternateText = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.Visible = true;
                    Btn_Modificar.AlternateText = "Modificar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Eliminar.Visible = true;
                    Grid_Peritos_Externos.SelectedIndex = -1;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Peritos Externos", "alert('Alta de Perito Externo Exitosa');", true);
                    //Response.Redirect("../Catastro/Frm_Cat_Cat_Peritos_Externos.aspx");
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Peritos Externos", "alert('Alta de Perito Externo Errónea');", true);
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
            if (Grid_Peritos_Externos.SelectedIndex > -1)
            {
                if (Btn_Modificar.AlternateText.Equals("Modificar"))
                {
                    Configuracion_Formulario(false);
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.Visible = false;
                    Btn_Eliminar.Visible = false;
                    Div_Grid_Peritos.Visible = false;
                    Div_Grid_Datos_Peritos.Visible = true;
                    Div_Seguridad_Peritos_Externos.Visible = true;  
                }
                else
                {
                    if (Validar_Componentes())
                    {
                        Cls_Cat_Cat_Peritos_Externos_Negocio Perito = new Cls_Cat_Cat_Peritos_Externos_Negocio();
                        Perito.P_Apellido_Materno = Txt_Apellido_Materno.Text.ToUpper();
                        Perito.P_Apellido_Paterno = Txt_Apellido_Paterno.Text.ToUpper();
                        Perito.P_Calle = Txt_Calle.Text.ToUpper();
                        Perito.P_Celular = Txt_Celular.Text;
                        Perito.P_Ciudad = Txt_Ciudad.Text.ToUpper();
                        Perito.P_Colonia = Txt_Colonia.Text.ToUpper();
                        Perito.P_Estado = Txt_Estado.Text.ToUpper();
                        Perito.P_Estatus = Cmb_Estatus.SelectedValue;
                        Perito.P_Fecha = Txt_Fecha.Text;
                        Perito.P_Nombre = Txt_Nombre.Text.ToUpper();
                        Perito.P_Observaciones = Txt_Observaciones.Text.ToUpper();
                        Perito.P_Password = Txt_Password.Text;
                        Perito.P_Telefono = Txt_Telefono.Text;
                        Perito.P_Usuario = Txt_Usuario.Text;
                        Perito.P_Perito_Externo_Id = Hdf_Perito_Externo.Value;
                        if ((Perito.Modificar_Perito_Externo()))
                        {
                            Div_Grid_Peritos.Visible = true;
                            Div_Grid_Datos_Peritos.Visible = false;
                            Div_Seguridad_Peritos_Externos.Visible = false;
                            Configuracion_Formulario(true);
                            Llenar_Tabla_Peritos_Externos(Grid_Peritos_Externos.PageIndex);
                            Btn_Modificar.AlternateText = "Modificar";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                            Btn_Nuevo.Visible = true;
                            Btn_Nuevo.AlternateText = "Nuevo";
                            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                            Btn_Salir.AlternateText = "Salir";
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                            Btn_Eliminar.Visible = true;
                            Grid_Peritos_Externos.SelectedIndex = -1;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Peritos Externos", "alert('Actualización de Perito Externo Exitosa.');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Peritos Externos", "alert('Error al Actualizar el Perito Externo.');", true);
                        }
                    }
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Seleccione el Perito Externo a Modificar.";
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
            Limpiar_Campos();
            Configuracion_Formulario(true);
            Btn_Nuevo.Visible = true;
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            Btn_Modificar.Visible = true;
            Btn_Modificar.AlternateText = "Modificar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            Btn_Eliminar.Visible = true;
            Llenar_Tabla_Peritos_Externos(Grid_Peritos_Externos.PageIndex);
            Grid_Peritos_Externos.SelectedIndex = -1;
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Div_Grid_Peritos.Visible = true;
            Div_Grid_Datos_Peritos.Visible = true;
            Div_Seguridad_Peritos_Externos.Visible = true;
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
        if(Div_Grid_Peritos.Visible==true)
        Llenar_Tabla_Peritos_Externos(0);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Peritos_Externos_PageIndexChanging
    ///DESCRIPCIÓN: Cambia la página del grid
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Peritos_Externos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Llenar_Tabla_Peritos_Externos(e.NewPageIndex);
        }
        catch (Exception E)
        {
            Lbl_Ecabezado_Mensaje.Text = E.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Peritos_Externos_SelectedIndexChanged
    ///DESCRIPCIÓN: Selecciona un registro del grid y toma los datos de los mismos campos del componente
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Peritos_Externos_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Peritos_Externos.SelectedIndex > -1)
        {
            Hdf_Perito_Externo.Value = Grid_Peritos_Externos.SelectedRow.Cells[1].Text;
            Cls_Cat_Cat_Peritos_Externos_Negocio Perito_Externo = new Cls_Cat_Cat_Peritos_Externos_Negocio();
            DataTable Dt_Perito_Externo;
            Perito_Externo.P_Perito_Externo_Id = Hdf_Perito_Externo.Value;
            Dt_Perito_Externo = Perito_Externo.Consultar_Peritos_Externos();
            Txt_Apellido_Materno.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Apellido_Materno].ToString();
            Txt_Apellido_Paterno.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Apellido_Paterno].ToString();
            Txt_Calle.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Calle].ToString();
            Txt_Celular.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Celular].ToString();
            Txt_Ciudad.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Ciudad].ToString();
            Txt_Colonia.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Colonia].ToString();
            Txt_Estado.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Estado].ToString();
            Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Estatus].ToString()));
            if (Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Fecha].ToString().Trim() != "")
            {
                Txt_Fecha.Text = Convert.ToDateTime(Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Fecha].ToString()).ToString("dd/MMM/yyyy");
            }
            else
            {
                Txt_Fecha.Text = "";
            }
            Txt_Nombre.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Nombre].ToString();
            Txt_Observaciones.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Observaciones].ToString();
            Txt_Password.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Password].ToString();
            Txt_Password.Attributes.Add("value", Txt_Password.Text);
            Txt_Password_Confirma.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Password].ToString();
            Txt_Password_Confirma.Attributes.Add("value", Txt_Password_Confirma.Text);
            Txt_Telefono.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Telefono].ToString();
            Txt_Usuario.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Usuario].ToString();
            Btn_Salir.AlternateText = "Atras";
            Div_Grid_Peritos.Visible = false;
            Div_Grid_Datos_Peritos.Visible = true;
            Llenar_Tabla_Documentos(0);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes
    ///DESCRIPCIÓN: valida que cada componente tenga informacion
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Componentes()
    {
        String Mensaje_Error = "Error: ";
        Boolean valido = true;

        if (Txt_Nombre.Text.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese el Nombre del Perito.";
            valido = false;
        }
        if (Txt_Apellido_Paterno.Text.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese el Apellido Paterno del Perito.";
            valido = false;
        }
        if (Txt_Apellido_Materno.Text.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese el Apellido Materno del perito.";
            valido = false;
        }
        if (Txt_Usuario.Text.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese el Usuario.";
            valido = false;
        }
        if ((Txt_Usuario.Text.Trim() != ""))
        {
            if (!Txt_Usuario.Text.Contains('@'))
            {
                if (Mensaje_Error.Trim() != "") { Mensaje_Error += "<br/>"; }
                Mensaje_Error += "+ Ingrese Correo Valido.";
                valido = false;
            }
        }
        else
        {
            if (Mensaje_Error.Trim() != "") { Mensaje_Error += "<br/>"; }
            Mensaje_Error += "+ Ingrese Correo Valido.";
            valido = false;
        }
        if (Txt_Password.Text.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese la Contraseña.";
            valido = false;
        }
        if (Txt_Password_Confirma.Text.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese la Contraseña nuevamente para confirmar.";
            valido = false;
        }
        if (Txt_Password_Confirma.Text.Trim() != "" && Txt_Password.Text.Trim()!="" && Txt_Password.Text!=Txt_Password_Confirma.Text)
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Las contraseñas no coinciden, favor de verificarlas.";
            valido = false;
        }
        if (Txt_Calle.Text.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese la Calle.";
            valido = false;
        }
        if (Txt_Colonia.Text.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese la Colonia.";
            valido = false;
        }
        if (Txt_Estado.Text.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese el Estado.";
            valido = false;
        }
        if (Txt_Ciudad.Text.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese la Ciudad.";
            valido = false;
        }
        if (Txt_Telefono.Text.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese el Teléfono.";
            valido = false;
        }
        else if (Txt_Telefono.Text.Trim().Length!=10)
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ El Teléfono es incorrecto, se requieren 10 dígitos.";
            valido = false;
        }
        else if ( Txt_Celular.Text.Trim()!="" && Txt_Celular.Text.Trim().Length != 10)
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ El Celular es incorrecto, se requieren 10 dígitos.";
            valido = false;
        }
        if (Cmb_Estatus.SelectedIndex == 0)
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Seleccione un estatus.";
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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
    ///DESCRIPCIÓN: Evento de la 
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        if (Grid_Peritos_Externos.SelectedIndex > -1)
        {
            Cls_Cat_Cat_Peritos_Externos_Negocio Perito_Externo = new Cls_Cat_Cat_Peritos_Externos_Negocio();
            Perito_Externo.P_Perito_Externo_Id = Grid_Peritos_Externos.SelectedRow.Cells[1].Text;
            if (Perito_Externo.Baja_Perito_Externo())
            {
                Llenar_Tabla_Peritos_Externos(Grid_Peritos_Externos.PageIndex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Peritos Externos", "alert('El Perito Externo ha sido dado de Baja exitosamente.');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Peritos Externos", "alert('Error al intentar dar de baja al Perito Externo.');", true);
            }
        }
        else
        {
            Lbl_Ecabezado_Mensaje.Text = "Seleccione un Registro";
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Documentos_DataBound
    ///DESCRIPCIÓN: Carga los componentes del grid
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Documentos_DataBound(object sender, EventArgs e)
    {
        Int16 i = 0;
        DataTable Dt_Documentos = (DataTable)Session["Dt_Documentos"];
        foreach (DataRow Dr_Renglon in Dt_Documentos.Rows)
        {
            if (Dr_Renglon["ACCION"].ToString() == "NADA")
            {
                if (File.Exists(Server.MapPath(Dr_Renglon["RUTA_DOCUMENTO"].ToString())))
                {
                    HyperLink Hlk_Enlace = new HyperLink();
                    Hlk_Enlace.Text = Path.GetFileName(Dr_Renglon["RUTA_DOCUMENTO"].ToString());
                    Hlk_Enlace.NavigateUrl = Dr_Renglon["RUTA_DOCUMENTO"].ToString();
                    Hlk_Enlace.CssClass = "enlace_fotografia";
                    Hlk_Enlace.Target = "blank";
                    Grid_Documentos.Rows[i].Cells[3].Controls.Add(Hlk_Enlace);
                    i++;
                }
            }
        }
    }
}