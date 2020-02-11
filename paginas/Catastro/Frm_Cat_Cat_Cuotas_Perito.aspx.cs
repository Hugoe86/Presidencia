using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Rangos_De_Descuentos_Por_Rol.Negocio;
using Presidencia.Constantes;
using Presidencia.Catalogo_Cat_Peritos_Internos.Negocio;
using System.Data;
using Presidencia.Catalogo_Cat_Cuotas_Perito.Negocio;

public partial class paginas_Catastro_Frm_Cat_Cat_Cuotas_Perito : System.Web.UI.Page
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
                Llenar_Tabla_Cuotas(0);
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

        Txt_Perito_Interno.Enabled = !Enabled;
        Txt_Anio.Enabled = !Enabled;
        Txt_1_Entrega.Enabled = !Enabled;
        Txt_2_Entrega.Enabled = !Enabled;
        Txt_3_Entrega.Enabled = !Enabled;
        Txt_4_Entrega.Enabled = !Enabled;
        Txt_5_Entrega.Enabled = !Enabled;
        Txt_6_Entrega.Enabled = !Enabled;
        Txt_7_Entrega.Enabled = !Enabled;
        Btn_Busqueda_Empleados.Enabled = Enabled;
        
        Btn_Busqueda_Empleados.Enabled = !Enabled;
        Grid_Cuotas.Enabled = !Enabled;
        
        //Grid_Peritos_Externos.Enabled = Enabled;
        Btn_Mostrar_Busqueda_Avanzada_Peritos_Internos.Enabled = !Enabled;
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
        Txt_Perito_Interno.Text="";
        Txt_Anio.Text="";
        Txt_1_Entrega.Text="";
        Txt_2_Entrega.Text="";
        Txt_3_Entrega.Text="";
        Txt_4_Entrega.Text="";
        Txt_5_Entrega.Text="";
        Txt_6_Entrega.Text="";
        Txt_7_Entrega.Text="";
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
                        Limpiar_Campos();
                        Grid_Cuotas.Enabled = false;

                        Grid_Cuotas.SelectedIndex = -1;


                       //Session["Tabla_Factores"] = Crear_Dt_Tabla_Factores();
                  
                  
                }
                else if (Validar_Componentes_Nuevo())
                {
                    if (Txt_Anio.Text.Trim() != "")
                    {
                        DataTable Dt_Tabla_Cuota = (DataTable)Session["Cuotas"];
                        if (!Existe_Valor_Construccion(Dt_Tabla_Cuota, Txt_Anio.Text.Trim(), Hdf_Perito_Interno_Id.Value))
                        {
                            Cls_Cat_Cat_Cuotas_Perito_Negocio Tabla_Cuotas = new Cls_Cat_Cat_Cuotas_Perito_Negocio();


                            Tabla_Cuotas.P_Perito_Interno_Id = Hdf_Perito_Interno_Id.Value;
                            Tabla_Cuotas.P_Anio = Txt_Anio.Text;
                            Tabla_Cuotas.P_Primera_Entrega = Txt_1_Entrega.Text;
                            Tabla_Cuotas.P_Segunda_Entrega = Txt_2_Entrega.Text;
                            Tabla_Cuotas.P_Tercera_Entrega = Txt_3_Entrega.Text;
                            Tabla_Cuotas.P_Cuarta_Entrega = Txt_4_Entrega.Text;
                            Tabla_Cuotas.P_Quinta_Entrega = Txt_5_Entrega.Text;
                            Tabla_Cuotas.P_Sexta_Entrega = Txt_6_Entrega.Text;
                            Tabla_Cuotas.P_Septima_Entrega = Txt_7_Entrega.Text;
                            Tabla_Cuotas.Alta_Cuota_Peritos();
                            Configuracion_Formulario(true);
                            Llenar_Tabla_Cuotas(Grid_Cuotas.PageIndex);
                            Btn_Nuevo.AlternateText = "Nuevo";
                            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                            Btn_Modificar.Visible = true;
                            Btn_Modificar.AlternateText = "Modificar";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                            Btn_Salir.AlternateText = "Salir";
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                            Grid_Cuotas.SelectedIndex = -1;

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Cuotas Perito", "alert('Alta Exitosa');", true);
                            Limpiar_Campos();
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Cuotas Perito", "alert('Alta Errónea');", true);
                }
                }
            
            catch (Exception E)
            {
                Lbl_Ecabezado_Mensaje.Text = E.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        //}


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
            if (Btn_Modificar.AlternateText.Equals("Modificar"))
            {
                if (Grid_Cuotas.Rows.Count > 0)
                {
                    Configuracion_Formulario(false);
                    Txt_Perito_Interno.Enabled = false;
                    Txt_Anio.Enabled = false;
                    Btn_Mostrar_Busqueda_Avanzada_Peritos_Internos.Enabled = false;
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.Visible = false;
                    Limpiar_Campos();
                    DataTable Dt_Tabla_Cuotas;
                    Cls_Cat_Cat_Cuotas_Perito_Negocio Tabla_Cuotas = new Cls_Cat_Cat_Cuotas_Perito_Negocio();
                    Dt_Tabla_Cuotas = Tabla_Cuotas.Consultar_Tabla_Cuotas_Perito();
                    Session["Tabla_Factores"] = Dt_Tabla_Cuotas.Copy();
                }
                else
                {
                    Lbl_Ecabezado_Mensaje.Text = "No hay registros en la tabla de valores que modificar.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                if (Validar_Componentes_Nuevo())
                {
                    Cls_Cat_Cat_Cuotas_Perito_Negocio Tabla_Cuotas = new Cls_Cat_Cat_Cuotas_Perito_Negocio();
                    Tabla_Cuotas.P_Cuota_Perito_Id = Hdf_Couta_Perito_Id.Value;
                    Tabla_Cuotas.P_Anio = Txt_Anio.Text;
                    Tabla_Cuotas.P_Primera_Entrega = Txt_1_Entrega.Text;
                    Tabla_Cuotas.P_Segunda_Entrega = Txt_2_Entrega.Text;
                    Tabla_Cuotas.P_Tercera_Entrega = Txt_3_Entrega.Text;
                    Tabla_Cuotas.P_Cuarta_Entrega = Txt_4_Entrega.Text;
                    Tabla_Cuotas.P_Quinta_Entrega = Txt_5_Entrega.Text;
                    Tabla_Cuotas.P_Sexta_Entrega = Txt_6_Entrega.Text;
                    Tabla_Cuotas.P_Septima_Entrega = Txt_7_Entrega.Text;

                    if ((Tabla_Cuotas.Modificar_Cuotas_Peritos()))
                    {
                        Configuracion_Formulario(true);
                        Llenar_Tabla_Cuotas(Grid_Cuotas.PageIndex);
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Nuevo.Visible = true;
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Grid_Cuotas.Enabled = true;
                        Grid_Cuotas.SelectedIndex = -1;
                        Grid_Cuotas.Enabled = false;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Factores de Cobro de Avaluos", "alert('Actualización Exitosa.');", true);
                        Limpiar_Campos();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Factores de Cobro de Avaluos", "alert('Error al intentar Actualizar.');", true);
                    }
                }
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
            //Llenar_Tabla_Valores(Grid_Valores.PageIndex);
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Grid_Cuotas.SelectedIndex = -1;
            Limpiar_Campos();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Valores_SelectedIndexChanged
    ///DESCRIPCIÓN: Selecciona un elemento del grid de tramos y toma sus valores correspondientes
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Cuotas_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Cuotas.SelectedIndex > -1)
            {

              
                Txt_Anio.Text = HttpUtility.HtmlDecode(Grid_Cuotas.SelectedRow.Cells[3].Text);
                Txt_1_Entrega.Text = Grid_Cuotas.SelectedRow.Cells[4].Text;
                Txt_2_Entrega.Text = Grid_Cuotas.SelectedRow.Cells[5].Text;
                Txt_3_Entrega.Text = Grid_Cuotas.SelectedRow.Cells[6].Text;
                Txt_4_Entrega.Text = Grid_Cuotas.SelectedRow.Cells[7].Text;
                Txt_5_Entrega.Text = Grid_Cuotas.SelectedRow.Cells[8].Text;
                Txt_6_Entrega.Text = Grid_Cuotas.SelectedRow.Cells[9].Text;
                Txt_7_Entrega.Text = Grid_Cuotas.SelectedRow.Cells[10].Text;
                Hdf_Perito_Interno_Id.Value = Grid_Cuotas.SelectedRow.Cells[2].Text;
                Hdf_Couta_Perito_Id.Value = Grid_Cuotas.SelectedRow.Cells[1].Text;
                Cls_Cat_Cat_Peritos_Internos_Negocio Peritos_Internos = new Cls_Cat_Cat_Peritos_Internos_Negocio();
                DataTable Dt_Peritos_Int;
                Peritos_Internos.P_Perito_Interno_Id = Hdf_Perito_Interno_Id.Value;
                Dt_Peritos_Int = Peritos_Internos.Consultar_Peritos_Internos();
                Txt_Perito_Interno.Text = Dt_Peritos_Int.Rows[0]["NOMBRE"] + " " + Dt_Peritos_Int.Rows[0]["APELLIDO_PATERNO"] + " " + Dt_Peritos_Int.Rows[0]["APELLIDO_MATERNO"]; 
                
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Seleccione un Factor de Cobro de Avaluos.";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
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
    ///NOMBRE DE LA FUNCIÓN: Grid_Tipos_Construccion_PageIndexChanging
    ///DESCRIPCIÓN: Cambia la página del grid
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Cuotas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
		
           Llenar_Tabla_Cuotas(e.NewPageIndex);
           Grid_Cuotas.SelectedIndex = -1;
           Limpiar_Campos();
           Hdf_Couta_Perito_Id.Value = "";
           Hdf_Perito_Interno_Id.Value = "";
           DataTable Dt_Tabla_Cuotas = (DataTable)Session["Cuotas"];
           Grid_Cuotas.Columns[1].Visible = true;
           Grid_Cuotas.Columns[2].Visible = true;
           Dt_Tabla_Cuotas.DefaultView.Sort = "ANIO DESC";
           Grid_Cuotas.DataSource = Dt_Tabla_Cuotas;
           Grid_Cuotas.PageIndex = e.NewPageIndex;
           Grid_Cuotas.DataBind();
           Grid_Cuotas.Columns[1].Visible = false;
           Grid_Cuotas.Columns[2].Visible = false;
        }
        catch (Exception E)
        {
           Lbl_Ecabezado_Mensaje.Text = E.Message;
           Lbl_Mensaje_Error.Text = "";
           Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Cerrar_Ventana_Click
    /// DESCRIPCION : Cierra la ventana de busqueda de empleados.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 13/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Cerrar_Ventana_Click(object sender, ImageClickEventArgs e)
    {
        Mpe_Busqueda_Peritos_Internos.Hide();
        Txt_Busqueda_Nombre.Text = "";
        Grid_Peritos_Externos.DataSource = null;
        Grid_Peritos_Externos.DataBind();
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Peritos_PageIndexChanging
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
            Llenar_Tabla_Peritos_Internos(e.NewPageIndex);
        }
        catch (Exception E)
        {
            Lbl_Ecabezado_Mensaje.Text = E.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Calles_SelectedIndexChanged
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
            Hdf_Perito_Interno_Id.Value = Grid_Peritos_Externos.SelectedRow.Cells[1].Text;
            Cls_Cat_Cat_Peritos_Internos_Negocio Perito = new Cls_Cat_Cat_Peritos_Internos_Negocio();
            DataTable Dt_Empleado;
            Perito.P_Perito_Interno_Id = Hdf_Perito_Interno_Id.Value;
            Perito.P_Empleado_Id = Grid_Peritos_Externos.SelectedRow.Cells[2].Text;
            Dt_Empleado = Perito.Consultar_Empleados();
            Txt_Perito_Interno.Text = Dt_Empleado.Rows[0][Cat_Empleados.Campo_Nombre].ToString() + " " + Dt_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Paterno].ToString() + " " + Dt_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Materno].ToString();
            //Txt_Perito_Interno_N.Text = Txt_Perito_Interno.Text;
            Txt_Busqueda_Nombre.Text = "";
            Grid_Peritos_Externos.SelectedIndex = -1;
            Grid_Peritos_Externos.Columns[1].Visible = true;
            Grid_Peritos_Externos.Columns[2].Visible = true;
            Grid_Peritos_Externos.DataSource = null;
            Grid_Peritos_Externos.PageIndex = 0;
            Grid_Peritos_Externos.DataBind();
            Grid_Peritos_Externos.Columns[1].Visible = false;
            Grid_Peritos_Externos.Columns[2].Visible = false;
            Mpe_Busqueda_Peritos_Internos.Hide();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Empleados_Click
    ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 23/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Busqueda_Empleados_Click(object sender, EventArgs e)
    {
        Llenar_Tabla_Peritos_Internos(0);

    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llenar_Empleados
    /// DESCRIPCION : carga todos los empleados, dependiendo de la búsqueda a realizar
    /// CREO        : Miguel Angel Bedolla Moreno
    /// FECHA_CREO  : 15/Mayo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llenar_Tabla_Peritos_Internos(int Pagina)
    {
        try
        {
            Cls_Cat_Cat_Peritos_Internos_Negocio Peritos_Internos = new Cls_Cat_Cat_Peritos_Internos_Negocio();
            DataTable Dt_Peritos_Int;
            Peritos_Internos.P_Empleado_Nombre = Txt_Busqueda_Nombre.Text.ToUpper();
            Dt_Peritos_Int = Peritos_Internos.Consultar_Peritos_Internos();
            Grid_Peritos_Externos.Columns[1].Visible = true;
            Grid_Peritos_Externos.Columns[2].Visible = true;
            Grid_Peritos_Externos.DataSource = Dt_Peritos_Int;
            Grid_Peritos_Externos.PageIndex = Pagina;
            Grid_Peritos_Externos.DataBind();
            Grid_Peritos_Externos.Columns[1].Visible = false;
            Grid_Peritos_Externos.Columns[2].Visible = false;
        }
        catch (Exception E)
        {
            Lbl_Ecabezado_Mensaje.Text = E.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Nuevo
    ///DESCRIPCIÓN: Valida los datos ingresados cuando se van a dar de alta los tramos por primera vez
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Componentes_Nuevo()
    {
        Boolean Valido = true;
        String Msj_Error = "Error: ";
        if (Txt_Perito_Interno.Text == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese El perito a asignar las Cuotas .";
            Valido = false;
        }
        if (Txt_Anio.Text == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese El año a asignar las Cuotas .";
            Valido = false;
        }
        if (!Valido)
        {
            Lbl_Ecabezado_Mensaje.Text = Msj_Error;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Valido;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Cuotas
    ///DESCRIPCIÓN: Llena la tabla de Cuotas
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 22/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Cuotas(int Pagina)
    {
        try
        {
            DataTable Dt_Tabla_Cuotas;
            Cls_Cat_Cat_Cuotas_Perito_Negocio Tabla_Cuotas = new Cls_Cat_Cat_Cuotas_Perito_Negocio();
            Grid_Cuotas.Columns[1].Visible = true;
            Grid_Cuotas.Columns[2].Visible = true;
            Dt_Tabla_Cuotas = Tabla_Cuotas.Consultar_Tabla_Cuotas_Perito();
            //Dt_Tabla_Cuotas.DefaultView.Sort = "ANIO DESC";
            Grid_Cuotas.DataSource = Dt_Tabla_Cuotas;
            Grid_Cuotas.PageIndex = Pagina;
            Grid_Cuotas.DataBind();
            Grid_Cuotas.Columns[1].Visible = false;
            Grid_Cuotas.Columns[2].Visible = false;
            Session["Cuotas"] = Dt_Tabla_Cuotas.Copy();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Existe_Tramo_Calle
    ///DESCRIPCIÓN: Determina si los datos ingresado ya existen en el grid de tramos.
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Existe_Valor_Construccion(DataTable Dt_Tabla_Cuota, String Anio,String Perito)
    {
        Boolean Existe = false;

        foreach (DataRow Dr_Renglon in Dt_Tabla_Cuota.Rows)
        {
            if (Dr_Renglon["ANIO"].ToString() == Anio && Dr_Renglon["PERITO_INTERNO_ID"].ToString()==Perito)
            {
                Existe = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Cuotas Peritos", "alert('Ya existe un registro para este perito en el Año: " + Anio + "');", true);
                break;
            }
        }
        return Existe;
    }
}

