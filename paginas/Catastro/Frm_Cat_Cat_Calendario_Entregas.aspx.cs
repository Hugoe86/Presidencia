using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using System.Data;
using Presidencia.Catalogo_Cat_Calendario_Entregas.Datos;
using Presidencia.Catalogo_Cat_Calendario_Entregas.Negocio;

public partial class paginas_Catastro_Frm_Cat_Cat_Calendario_Entregas : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!IsPostBack)
            {
                Session["Activa"] = true;
                Configuracion_Formulario(true);
                Llenar_Calendario(0);
               // Existe_Valor_Construccion();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message.ToString();

        }
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
        Txt_Anio.Enabled = !Enabled;
        Txt_1_Entrega.Enabled = !Enabled;
        Txt_1_Entrega_Real.Enabled = !Enabled;
        Txt_2_Entrega.Enabled = !Enabled;
        Txt_2_Entrega_Real.Enabled = !Enabled;
        Txt_3_Entrega.Enabled = !Enabled;
        Txt_3_Entrega_Real.Enabled = !Enabled;
        Txt_4_Entrega.Enabled = !Enabled;
        Txt_4_Entrega_Real.Enabled = !Enabled;
        Txt_5_Entrega.Enabled = !Enabled;
        Txt_5_Entrega_Real.Enabled = !Enabled;
        Txt_6_Entrega.Enabled = !Enabled;
        Txt_6_Entrega_Real.Enabled = !Enabled;
        Txt_7_Entrega.Enabled = !Enabled;
        Txt_7_Entrega_Real.Enabled = !Enabled;
        
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
    private void Limpiar_Formulario()
    {
        Txt_Anio.Text = "";
        Txt_1_Entrega.Text = "";
        Txt_1_Entrega_Real.Text = "";
        Txt_2_Entrega.Text = "";
        Txt_2_Entrega_Real.Text = "";
        Txt_3_Entrega.Text = "";
        Txt_3_Entrega_Real.Text = "";
        Txt_4_Entrega.Text = "";
        Txt_4_Entrega_Real.Text = "";
        Txt_5_Entrega.Text = "";
        Txt_5_Entrega_Real.Text = "";
        Txt_6_Entrega.Text = "";
        Txt_6_Entrega_Real.Text = "";
        Txt_7_Entrega.Text = "";
        Txt_7_Entrega_Real.Text = "";
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
                Limpiar_Formulario();
                Grid_Fecha_Entrega.Enabled = false;
                Grid_Fecha_Entrega.SelectedIndex = -1;
            }
            else if (Validar_Componentes_Nuevo())
            {
                if (Txt_Anio.Text.Trim() != "")
                {

                    DataTable Dt_Fechas = (DataTable)Session["Anio"];
                    if (!Existe_Valor_Construccion(Dt_Fechas, Txt_Anio.Text.Trim()))
                    {
                        Cls_Cat_Cat_Calendario_Entregas_Negocio Calendario_Fechas = new Cls_Cat_Cat_Calendario_Entregas_Negocio();
                        Calendario_Fechas.P_Fecha_Entrega_Id = Hdf_Fecha_Id.Value;
                        Calendario_Fechas.P_Anio = Txt_Anio.Text;
                        Calendario_Fechas.P_Fecha_Primera_Entrega = Txt_1_Entrega.Text;
                        Calendario_Fechas.P_Fecha_Primera_Entrega_Real = Txt_1_Entrega_Real.Text;
                        Calendario_Fechas.P_Fecha_Segunda_Entrega = Txt_2_Entrega.Text;
                        Calendario_Fechas.P_Fecha_Segunda_Entrega_Real = Txt_2_Entrega_Real.Text;
                        Calendario_Fechas.P_Fecha_Tercera_Entrega = Txt_3_Entrega.Text;
                        Calendario_Fechas.P_Fecha_Tercera_Entrega_Real = Txt_3_Entrega_Real.Text;
                        Calendario_Fechas.P_Fecha_Cuarta_Entrega = Txt_4_Entrega.Text;
                        Calendario_Fechas.P_Fecha_Cuarta_Entrega_Real = Txt_4_Entrega_Real.Text;
                        Calendario_Fechas.P_Fecha_Quinta_Entrega = Txt_5_Entrega.Text;
                        Calendario_Fechas.P_Fecha_Quinta_Entrega_Real = Txt_5_Entrega_Real.Text;
                        Calendario_Fechas.P_Fecha_Sexta_Entrega = Txt_6_Entrega.Text;
                        Calendario_Fechas.P_Fecha_Sexta_Entrega_Real = Txt_6_Entrega_Real.Text;
                        Calendario_Fechas.P_Fecha_Septima_Entrega = Txt_7_Entrega.Text;
                        Calendario_Fechas.P_Fecha_Septima_Entrega_Real = Txt_7_Entrega_Real.Text;
                        Calendario_Fechas.Alta_Calendario_Entregas();
                        Configuracion_Formulario(true);
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Modificar.Visible = true;
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Grid_Fecha_Entrega.SelectedIndex = -1;
                        Grid_Fecha_Entrega.DataSource = null;
                        Grid_Fecha_Entrega.DataBind();
                        Llenar_Calendario(Grid_Fecha_Entrega.PageIndex);
                        Limpiar_Formulario();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Calendario Fechas", "alert('Alta Exitosa');", true);
                        Llenar_Calendario(0);
                    }
                    
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Calendario Fechas", "alert('Alta Errónea');", true);
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
    ///NOMBRE DE LA FUNCIÓN: Existe_Valor_Construccion
    ///DESCRIPCIÓN: Determina si los datos ingresado ya existen en el grid de Valores.
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Existe_Valor_Construccion(DataTable Dt_Tabla_Anio, String Anio)
    {
        Boolean Existe = false;
        Cls_Cat_Cat_Calendario_Entregas_Negocio anio = new Cls_Cat_Cat_Calendario_Entregas_Negocio();
        foreach (DataRow Dr_Renglon in Dt_Tabla_Anio.Rows)
        {
            if (Dr_Renglon["ANIO"].ToString() == Anio)
            {
                Existe = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tabla de Valores Catastrales", "alert('Ya existen valores para el año " + Anio + "');", true);
                break;
            }
        }
        return Existe;
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
                if (Grid_Fecha_Entrega.Rows.Count > 0)
                {
                    
                    Configuracion_Formulario(false);
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.Visible = false;
                    Limpiar_Formulario();
                    DataTable Dt_Tabla_Cuotas;
                    Cls_Cat_Cat_Calendario_Entregas_Negocio Tabla_Cuotas = new Cls_Cat_Cat_Calendario_Entregas_Negocio();
                    Dt_Tabla_Cuotas = Tabla_Cuotas.Consulta_Calendario_Entregas();
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
                    Cls_Cat_Cat_Calendario_Entregas_Negocio Calendario = new Cls_Cat_Cat_Calendario_Entregas_Negocio();
                    Calendario.P_Fecha_Entrega_Id = Hdf_Fecha_Id.Value;
                    Calendario.P_Anio = Txt_Anio.Text;
                    Calendario.P_Fecha_Primera_Entrega = Txt_1_Entrega.Text;
                    Calendario.P_Fecha_Primera_Entrega_Real = Txt_1_Entrega_Real.Text;
                    Calendario.P_Fecha_Segunda_Entrega = Txt_2_Entrega.Text;
                    Calendario.P_Fecha_Segunda_Entrega_Real = Txt_2_Entrega_Real.Text;
                    Calendario.P_Fecha_Tercera_Entrega = Txt_3_Entrega.Text;
                    Calendario.P_Fecha_Tercera_Entrega_Real = Txt_3_Entrega_Real.Text;
                    Calendario.P_Fecha_Cuarta_Entrega = Txt_4_Entrega.Text;
                    Calendario.P_Fecha_Cuarta_Entrega_Real = Txt_4_Entrega_Real.Text;
                    Calendario.P_Fecha_Quinta_Entrega = Txt_5_Entrega.Text;
                    Calendario.P_Fecha_Quinta_Entrega_Real = Txt_5_Entrega_Real.Text;
                    Calendario.P_Fecha_Sexta_Entrega = Txt_6_Entrega.Text;
                    Calendario.P_Fecha_Sexta_Entrega_Real = Txt_6_Entrega_Real.Text;
                    Calendario.P_Fecha_Septima_Entrega = Txt_7_Entrega.Text;
                    Calendario.P_Fecha_Septima_Entrega_Real = Txt_7_Entrega_Real.Text;

                    if ((Calendario.Modificar_Calendario_Entregas()))
                    {
                        Configuracion_Formulario(true);
                        Llenar_Calendario(Grid_Fecha_Entrega.PageIndex);
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Nuevo.Visible = true;
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Grid_Fecha_Entrega.Enabled = true;
                        Grid_Fecha_Entrega.SelectedIndex = -1;
                        //Grid_Fecha_Entrega.Enabled = false;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Calendario de Fechas", "alert('Actualización Exitosa.');", true);
                        Limpiar_Formulario();
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
            Grid_Fecha_Entrega.SelectedIndex = -1;
            Limpiar_Formulario();
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
    protected void Grid_Fecha_Entrega_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Fecha_Entrega.SelectedIndex > -1)
            {
                Txt_Anio.Text = HttpUtility.HtmlDecode(Grid_Fecha_Entrega.SelectedRow.Cells[2].Text);
                Hdf_Fecha_Id.Value = Grid_Fecha_Entrega.SelectedRow.Cells[1].Text;
                Txt_1_Entrega.Text = Grid_Fecha_Entrega.SelectedRow.Cells[3].Text;
                Txt_1_Entrega_Real.Text = HttpUtility.HtmlDecode(Grid_Fecha_Entrega.SelectedRow.Cells[4].Text);
                Txt_2_Entrega.Text = Grid_Fecha_Entrega.SelectedRow.Cells[5].Text;
                Txt_2_Entrega_Real.Text = HttpUtility.HtmlDecode((Grid_Fecha_Entrega.SelectedRow.Cells[6].Text));
                Txt_3_Entrega.Text = Grid_Fecha_Entrega.SelectedRow.Cells[7].Text;
                Txt_3_Entrega_Real.Text = HttpUtility.HtmlDecode(Grid_Fecha_Entrega.SelectedRow.Cells[8].Text);
                Txt_4_Entrega.Text = Grid_Fecha_Entrega.SelectedRow.Cells[9].Text;
                Txt_4_Entrega_Real.Text = HttpUtility.HtmlDecode(Grid_Fecha_Entrega.SelectedRow.Cells[10].Text);
                Txt_5_Entrega.Text = Grid_Fecha_Entrega.SelectedRow.Cells[11].Text;
                Txt_5_Entrega_Real.Text = HttpUtility.HtmlDecode(Grid_Fecha_Entrega.SelectedRow.Cells[12].Text);
                Txt_6_Entrega.Text = Grid_Fecha_Entrega.SelectedRow.Cells[13].Text;
                Txt_6_Entrega_Real.Text = HttpUtility.HtmlDecode(Grid_Fecha_Entrega.SelectedRow.Cells[14].Text);
                Txt_7_Entrega.Text = Grid_Fecha_Entrega.SelectedRow.Cells[15].Text;
                Txt_7_Entrega_Real.Text = HttpUtility.HtmlDecode( Grid_Fecha_Entrega.SelectedRow.Cells[16].Text);
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Seleccione un Año del Calendario.";
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

    protected void Grid_Fecha_Entrega_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            //Llenar_Calendario(e.NewPageIndex);
            Grid_Fecha_Entrega.SelectedIndex = -1;
            Txt_1_Entrega.Text = "";
            Txt_2_Entrega.Text = "";
            
            DataTable Dt_Calendario = (DataTable)Session["Calendario_Fecha"];
            Grid_Fecha_Entrega.Columns[1].Visible = true;
            //Grid_Fecha_Entrega.Columns[2].Visible = true;
            Dt_Calendario.DefaultView.RowFilter = " ACCION <> 'BAJA'";
            Dt_Calendario.DefaultView.Sort = "ANIO DESC";
            Grid_Fecha_Entrega.DataSource = Dt_Calendario;
            Grid_Fecha_Entrega.PageIndex = e.NewPageIndex;
            Grid_Fecha_Entrega.DataBind();
            Grid_Fecha_Entrega.Columns[1].Visible = false;
            //Grid_Fecha_Entrega.Columns[2].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
         

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Calendario
    ///DESCRIPCIÓN: Llena la tabla de Cuotas
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 22/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Calendario(int Pagina)
   {
        try
        {
            DataTable Dt_Tabla_Calendario;
            Cls_Cat_Cat_Calendario_Entregas_Negocio Tabla_Cuotas = new Cls_Cat_Cat_Calendario_Entregas_Negocio();
            Grid_Fecha_Entrega.Columns[1].Visible = true;
           // Grid_Fecha_Entrega.Columns[3].Visible = true;
            Dt_Tabla_Calendario = Tabla_Cuotas.Consulta_Calendario_Entregas();            
            Grid_Fecha_Entrega.DataSource = Dt_Tabla_Calendario;
            Grid_Fecha_Entrega.PageIndex = Pagina;
            Grid_Fecha_Entrega.DataBind();
            Grid_Fecha_Entrega.Columns[1].Visible = false;
            //Grid_Fecha_Entrega.Columns[3].Visible = false; 
            Session["Anio"] = Dt_Tabla_Calendario.Copy();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
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
        DataTable Dt_Calendario_Fechas = (DataTable)Session["Calendario_Fecha"];
        if (Txt_1_Entrega.Text == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese Fecha 1a Entrada.";
            Valido = false;
        }
        if (Txt_2_Entrega.Text == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese Fecha 2a Entrada.";
            Valido = false;
        }
        if (Txt_3_Entrega.Text == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese Fecha 3a Entrada.";
            Valido = false;
        }
        if (Txt_4_Entrega.Text == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese Fecha 4a Entrada.";
            Valido = false;
        }
        if (Txt_5_Entrega.Text == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese Fecha 5a Entrada.";
            Valido = false;
        }
        if (Txt_6_Entrega.Text == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese Fecha 6a Entrada.";
            Valido = false;
        }
        if (Txt_7_Entrega.Text == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese Fecha 7a Entrada.";
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

}
