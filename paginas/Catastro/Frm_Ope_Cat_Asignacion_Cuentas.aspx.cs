using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Catalogo_Cat_Peritos_Internos.Negocio;
using System.Data;
using Presidencia.Constantes;
using Presidencia.Operacion_Cat_Asignacion_Cuentas.Negocio;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Tipos_Predio.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class paginas_Catastro_Frm_Ope_Cat_Asignacion_Cuentas : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                String Ventana_Modal = "";
                Session["Activa"] = true;//Variable para mantener la session activa.
                Llenar_Combo_Tipos_Predio();
                Limpiar_Formulario();
                Configuracion_Formulario(false);
                Llenar_Cuentas_Asignadas(0);
                Ventana_Modal = "Abrir_Ventana_Modal('../Predial/Ventanas_Emergentes/Frm_Busqueda_Avanzada_Colonias_Calles.aspx', 'center:yes;resizable:yes;status:no;dialogWidth:580px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                Btn_Busqueda_Avanzada_Colonias.Attributes.Add("onclick", Ventana_Modal);

                Ventana_Modal = "Abrir_Ventana_Modal('../Predial/Ventanas_Emergentes/Frm_Busqueda_Avanzada_Colonias_Calles.aspx', 'center:yes;resizable:yes;status:no;dialogWidth:580px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                Btn_Busqueda_Avanzada_Calles.Attributes.Add("onclick", Ventana_Modal);
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
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Formulario
    ///DESCRIPCIÓN: Limpia todos los campos del formulario
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Limpiar_Formulario()
    {
        Txt_Busqueda_Nombre.Text = "";
        Txt_Busqueda.Text = "";
        Txt_Calle_Busqueda.Text = "";
        Txt_Colonia_Busqueda.Text = "";
        Txt_Cuenta_Predial.Text = "";
        Txt_Cuenta_Predial_Busqueda.Text = "";
        Txt_No_Ext_Busqueda.Text = "";
        Txt_No_Int_Busqueda.Text = "";
        Txt_Perito_Interno.Text = "";
        Txt_Perito_Interno_N.Text = "";
        Txt_Terreno_Menor.Text = "";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Formulario
    ///DESCRIPCIÓN: Limpia todos los campos del formulario
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Configuracion_Formulario(Boolean Enabled)
    {
        Txt_Busqueda.Enabled = !Enabled;
        Txt_Cuenta_Predial.Enabled = false;
        Txt_Perito_Interno.Enabled = false;
        Grid_Cuentas_Asignadas.Enabled = !Enabled;
        Btn_Mostrar_Busqueda_Avanzada_Peritos.Enabled = Enabled;
        Btn_Buscar.Enabled = !Enabled;
        Txt_Calle_Busqueda.Enabled = Enabled;
        Txt_Colonia_Busqueda.Enabled = Enabled;
        Txt_Cuenta_Predial_Busqueda.Enabled = Enabled;
        Txt_No_Ext_Busqueda.Enabled = Enabled;
        Txt_No_Int_Busqueda.Enabled = Enabled;
        Txt_Terreno.Enabled = Enabled;
        Txt_Terreno_Menor.Enabled = Enabled;
        Txt_Construccion.Enabled = Enabled;
        Txt_Construccion_Menor.Enabled = Enabled;
        Txt_Efecto_Anio.Enabled = Enabled;
        Txt_Efecto_Bimestre.Enabled = Enabled;
        Txt_Propietario.Enabled = Enabled;
        Cmb_Tipo_Predio.Enabled = Enabled;
        Txt_Perito_Interno_N.Enabled = false;
        Btn_Buscar_Cuentas.Enabled = Enabled;
        Btn_Mostrar_Busqueda_Avanzada_Peritos_Internos.Enabled = Enabled;
        Txt_Total_Cuentas.Style["Text-align"] = "Right";
        Txt_Total_Cuentas_Asignadas.Style["Text-align"] = "Right";
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
                Configuracion_Formulario(true);
                Btn_Nuevo.AlternateText = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Modificar.Visible = false;
                Div_Nuevo.Visible = true;
                Div_Modificar.Visible = false;
            }
            else if (Validar_Componentes())
            {
                Cls_Ope_Cat_Asignacion_Cuentas_Negocio Cuentas = new Cls_Ope_Cat_Asignacion_Cuentas_Negocio();
                Cuentas.P_Perito_Interno_Id = Hdf_Perito_Interno_Id.Value;
                Cuentas.P_Dt_Cuentas = (DataTable)Session["Dt_Cuentas_Asignadas"];
                if ((Cuentas.Alta_Cuenta_Asignada()))
                {
                    Configuracion_Formulario(false);
                    Llenar_Cuentas_Asignadas(Grid_Cuentas_Asignadas.PageIndex);
                    Btn_Modificar.Visible = true;
                    Btn_Modificar.AlternateText = "Modificar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Nuevo.Visible = true;
                    Btn_Nuevo.AlternateText = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Grid_Peritos_Externos.SelectedIndex = -1;
                    Limpiar_Formulario();
                    Session["Dt_Cuentas_Asignadas"] = null;
                    Grid_Cuentas_Prediales.DataSource = null;
                    Grid_Cuentas_Prediales.DataBind();
                    Div_Nuevo.Visible = false;
                    Div_Modificar.Visible=true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Asignación de Peritos Internos", "alert('El perito Interno ha sido asignado exitosamente.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Asignación de Peritos Internos", "alert('Error al intentar asignar el Perito Interno.');", true);
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
            if (Grid_Cuentas_Asignadas.SelectedIndex > -1)
            {
                if (Btn_Modificar.AlternateText.Equals("Modificar"))
                {
                    Configuracion_Formulario(true);
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.Visible = false;
                    Div_Modificar.Visible = true;
                    Div_Nuevo.Visible = false;
                }
                else
                {
                    if (Validar_Componentes())
                    {
                        Cls_Ope_Cat_Asignacion_Cuentas_Negocio Cuenta = new Cls_Ope_Cat_Asignacion_Cuentas_Negocio();
                        Cuenta.P_Cuenta_Predial_Id = Hdf_Cuenta_Predial_Id.Value;
                        Cuenta.P_Perito_Interno_Id = Hdf_Perito_Interno_Id.Value;
                        Cuenta.P_No_Asignacion = Hdf_No_Asignacion.Value;
                        if ((Cuenta.Modificar_Cuenta_Asignada()))
                        {
                            Configuracion_Formulario(false);
                            Llenar_Cuentas_Asignadas(Grid_Cuentas_Asignadas.PageIndex);
                            Btn_Modificar.AlternateText = "Modificar";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                            Btn_Nuevo.Visible = true;
                            Btn_Nuevo.AlternateText = "Nuevo";
                            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                            Btn_Salir.AlternateText = "Salir";
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                            Grid_Peritos_Externos.SelectedIndex = -1;
                            Grid_Cuentas_Asignadas.SelectedIndex = -1;
                            Limpiar_Formulario();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Asignación de Cuentas Prediales", "alert('Modificación Exitosa.');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Asignación de Cuentas Prediales", "alert('Error al intentar modificar.');", true);
                        }
                    }
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Seleccione el Perito a re-asignar";
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
            Llenar_Cuentas_Asignadas(0);
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Grid_Peritos_Externos.DataSource = null;
            Grid_Peritos_Externos.DataBind();
            Limpiar_Formulario();
            Session["Dt_Cuentas_Asignadas"] = null;
            Grid_Cuentas_Prediales.DataSource = null;
            Grid_Cuentas_Prediales.DataBind();
            Div_Nuevo.Visible = false;
            Div_Modificar.Visible = true;
        }
    }
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Llenar_Cuentas_Asignadas(0);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Calles_PageIndexChanging
    ///DESCRIPCIÓN: Cambia la página del grid
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Cuentas_Asignadas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Llenar_Cuentas_Asignadas(e.NewPageIndex);
    }

    protected void Grid_Cuentas_Asignadas_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Cuentas_Asignadas.SelectedIndex > -1)
        {
            Hdf_Cuenta_Predial_Id.Value = Grid_Cuentas_Asignadas.SelectedRow.Cells[4].Text;
            Txt_Cuenta_Predial.Text = Grid_Cuentas_Asignadas.SelectedRow.Cells[5].Text;
            Hdf_Perito_Interno_Id.Value = Grid_Cuentas_Asignadas.SelectedRow.Cells[2].Text;
            Txt_Perito_Interno.Text = Grid_Cuentas_Asignadas.SelectedRow.Cells[3].Text;
            Hdf_No_Asignacion.Value = Grid_Cuentas_Asignadas.SelectedRow.Cells[1].Text;
        }
    }

    protected void Btn_Busqueda_Empleados_Click(object sender, EventArgs e)
    {
        Llenar_Tabla_Peritos_Internos(0);
    }

    protected void Btn_Buscar_Cuentas_Click(object sender, ImageClickEventArgs e)
    {
        Llenar_Cuentas_Prediales(0);
    }

    protected void Btn_Cerrar_Ventana_Click(object sender, ImageClickEventArgs e)
    {
        Mpe_Busqueda_Peritos_Internos.Hide();
        Txt_Busqueda_Nombre.Text = "";
        Grid_Peritos_Externos.DataSource = null;
        Grid_Peritos_Externos.DataBind();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Calles_PageIndexChanging
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
            Txt_Perito_Interno_N.Text = Txt_Perito_Interno.Text;
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
    ///NOMBRE DE LA FUNCIÓN: Guardar_Cuentas_Prediales
    ///DESCRIPCIÓN: Cambia la página del grid
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected DataTable Guardar_Cuentas_Prediales()
    {
        DataTable Dt_Cuentas_Prediales = (DataTable)Session["Dt_Cuentas_Asignadas"];
        //Dt_Cuentas_Prediales.Columns.Add("ACCION", typeof(String));
        for (int i = 0; i < Dt_Cuentas_Prediales.Rows.Count; i++)
        {
            CheckBox Chk_Asignada = (CheckBox)Grid_Cuentas_Prediales.Rows[i].Cells[0].FindControl("Chk_Cuenta_Asignada");
            if (Chk_Asignada.Checked)
            {
                Dt_Cuentas_Prediales.Rows[i]["ACCION"] = "ALTA";
            }
            else
            {
                Dt_Cuentas_Prediales.Rows[i]["ACCION"] = "NADA";
            }
        }
        return Dt_Cuentas_Prediales;
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
    private void Llenar_Cuentas_Asignadas(int Pagina)
    {
        try
        {
            Cls_Ope_Cat_Asignacion_Cuentas_Negocio Cuentas = new Cls_Ope_Cat_Asignacion_Cuentas_Negocio();
            DataTable Dt_Cuentas_Asignadas;

            if (Cmb_Busqueda.SelectedValue == "COLONIA")
            {
                Cuentas.P_Colonia = Txt_Busqueda.Text.ToUpper();
            }
            else if (Cmb_Busqueda.SelectedValue == "CALLE")
            {
                Cuentas.P_Calle = Txt_Busqueda.Text.ToUpper();
            }
            else if (Cmb_Busqueda.SelectedValue == "PERITO")
            {
                Cuentas.P_Perito = Txt_Busqueda.Text.ToUpper();
            }
            else if (Cmb_Busqueda.SelectedValue == "CUENTA_PREDIAL")
            {
                Cuentas.P_Cuenta_Predial = Txt_Busqueda.Text.ToUpper();
            }
            Dt_Cuentas_Asignadas = Cuentas.Consultar_Cuentas_Asignadas();
            Grid_Cuentas_Asignadas.Columns[1].Visible = true;
            Grid_Cuentas_Asignadas.Columns[2].Visible = true;
            Grid_Cuentas_Asignadas.Columns[4].Visible = true;
            Grid_Cuentas_Asignadas.DataSource = Dt_Cuentas_Asignadas;
            Grid_Cuentas_Asignadas.PageIndex = Pagina;
            Grid_Cuentas_Asignadas.DataBind();
            Grid_Cuentas_Asignadas.Columns[1].Visible = false;
            Grid_Cuentas_Asignadas.Columns[2].Visible = false;
            Grid_Cuentas_Asignadas.Columns[4].Visible = false;
        }
        catch (Exception E)
        {
            Lbl_Ecabezado_Mensaje.Text = E.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
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
    private void Llenar_Cuentas_Prediales(int Pagina)
    {
        try
        {
            Cls_Ope_Cat_Asignacion_Cuentas_Negocio Cuentas = new Cls_Ope_Cat_Asignacion_Cuentas_Negocio();
            DataTable Dt_Cuentas_Prediales;
            if (Txt_Colonia_Busqueda.Text.Trim() != "")
            {
                Cuentas.P_Colonia = Txt_Colonia_Busqueda.Text.ToUpper();
            }
            if (Txt_Calle_Busqueda.Text.Trim() != "")
            {
                Cuentas.P_Calle = Txt_Calle_Busqueda.Text.ToUpper();
            }
            if (Txt_Cuenta_Predial_Busqueda.Text.Trim() != "")
            {
                Cuentas.P_Cuenta_Predial = Txt_Cuenta_Predial_Busqueda.Text.ToUpper();
            }
            if (Txt_No_Ext_Busqueda.Text.Trim() != "")
            {
                Cuentas.P_No_Ext = Txt_No_Ext_Busqueda.Text.ToUpper();
            }
            if (Txt_No_Int_Busqueda.Text.Trim() != "")
            {
                Cuentas.P_No_Int = Txt_No_Int_Busqueda.Text.ToUpper();
            }
            if (Txt_Terreno.Text.Trim() != "")
            {
                Cuentas.P_Superficie_Terreno = Txt_Terreno.Text.Trim();
            }
            if (Txt_Terreno_Menor.Text.Trim() != "")
            {
                Cuentas.P_Superficie_Terreno_Menor = Txt_Terreno_Menor.Text.Trim();
            }
            if (Txt_Construccion.Text.Trim() != "")
            {
                Cuentas.P_Superficie_Construccion = Txt_Construccion.Text.Trim();
            }
            if (Txt_Construccion_Menor.Text.Trim() != "")
            {
                Cuentas.P_Superficie_Construccion_Menor = Txt_Construccion_Menor.Text.Trim();
            }
            if (Txt_Efecto_Anio.Text.Trim() != "")
            {
                Cuentas.P_Efecto_Anio = Txt_Efecto_Anio.Text.Trim();
            }
            if (Txt_Efecto_Bimestre.Text.Trim() != "")
            {
                Cuentas.P_Efecto_Bimestre = Txt_Efecto_Bimestre.Text.Trim();
            }
            if (Txt_Propietario.Text.Trim() != "")
            {
                Cuentas.P_Propietario = Txt_Propietario.Text.ToUpper();
            }
            if (Cmb_Tipo_Predio.SelectedItem.Text != "<SELECCIONE>")
            {
                Cuentas.P_Tipo_Predio = Cmb_Tipo_Predio.SelectedItem.Text;
            }
            Dt_Cuentas_Prediales = Cuentas.Consultar_Cuentas_Prediales();
            Session["Dt_Cuentas_Asignadas"] = Dt_Cuentas_Prediales.Copy();
            Grid_Cuentas_Prediales.Columns[1].Visible = true;
            Grid_Cuentas_Prediales.DataSource = Dt_Cuentas_Prediales;
            Grid_Cuentas_Prediales.PageIndex = 0;
            Grid_Cuentas_Prediales.DataBind();
            Grid_Cuentas_Prediales.Columns[1].Visible = false;
        }
        catch (Exception E)
        {
            Lbl_Ecabezado_Mensaje.Text = E.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Cuentas_Prediales_PageIndexChanging
    ///DESCRIPCIÓN: Cambia la página del grid
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Cuentas_Prediales_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable Dt_Cuentas_Prediales = (DataTable)Session["Dt_Cuentas_Asignadas"];
        Grid_Cuentas_Prediales.Columns[1].Visible = true;
        Grid_Cuentas_Prediales.DataSource = Dt_Cuentas_Prediales;
        Grid_Cuentas_Prediales.PageIndex = e.NewPageIndex;
        Grid_Cuentas_Prediales.DataBind();
        Grid_Cuentas_Prediales.Columns[1].Visible = false;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Cuentas_Prediales_DataBound
    ///DESCRIPCIÓN: carga los datos en los componentes del grid
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 24/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Grid_Cuentas_Prediales_DataBound(object sender, EventArgs e)
    {
        int cuentas_asignadas = 0;
        DataTable Dt_Cuentas_Asignadas = (DataTable)Session["Dt_Cuentas_Asignadas"];
        if (Dt_Cuentas_Asignadas != null)
        {
            int indice = (Grid_Cuentas_Prediales.PageIndex * 200) + 200;
            if (indice <= Dt_Cuentas_Asignadas.Rows.Count)
            {
                indice = 200;
            }
            else
            {
                indice = Dt_Cuentas_Asignadas.Rows.Count - (Grid_Cuentas_Prediales.PageIndex * 200);
            }
            for (int i = 0; i < indice; i++)
            {
                CheckBox Chk_Cuenta_Asignada = (CheckBox)Grid_Cuentas_Prediales.Rows[i].Cells[0].FindControl("Chk_Cuenta_Asignada");
                if (Dt_Cuentas_Asignadas.Rows[(Grid_Cuentas_Prediales.PageIndex * 200) + i]["ACCION"].ToString() == "ALTA")
                {
                    Chk_Cuenta_Asignada.Checked = true;
                }
                else
                {
                    Chk_Cuenta_Asignada.Checked = false;
                }
            }
            foreach (DataRow Dr_Renglon in Dt_Cuentas_Asignadas.Rows)
            {
                if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                {
                    cuentas_asignadas++;
                }
            }
            Txt_Total_Cuentas.Text = Dt_Cuentas_Asignadas.Rows.Count.ToString("###,###,###,###,###,##0");
            Txt_Total_Cuentas_Asignadas.Text = cuentas_asignadas.ToString("###,###,###,###,###,##0");
            if (Grid_Cuentas_Prediales.Rows.Count > 0)
            {
                if (Txt_Total_Cuentas.Text == Txt_Total_Cuentas_Asignadas.Text)
                {
                    ((CheckBox)Grid_Cuentas_Prediales.HeaderRow.Cells[0].FindControl("Chk_Select_Todas")).Checked = true;
                }
                else
                {
                    ((CheckBox)Grid_Cuentas_Prediales.HeaderRow.Cells[0].FindControl("Chk_Select_Todas")).Checked = false;
                }
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Tipos_Predio
    ///DESCRIPCIÓN: Llena el combo con los tipos de predio
    ///PROPIEDADES:    
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Tipos_Predio()
    {
        try
        {
            Cls_Cat_Pre_Tipos_Predio_Negocio Tipos_Predio = new Cls_Cat_Pre_Tipos_Predio_Negocio();
            DataTable Dt_Tipos_Predio;
            Dt_Tipos_Predio = Tipos_Predio.Consultar_Tipo_Predio();
            DataRow Dr_Tipo_Predio = Dt_Tipos_Predio.NewRow();
            Dr_Tipo_Predio[Cat_Pre_Tipos_Predio.Campo_Descripcion] = "<SELECCIONE>";
            Dr_Tipo_Predio[Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID] = "SELECCIONE";
            Dt_Tipos_Predio.Rows.InsertAt(Dr_Tipo_Predio, 0);
            Cmb_Tipo_Predio.DataSource = Dt_Tipos_Predio;
            Cmb_Tipo_Predio.DataTextField = Cat_Pre_Tipos_Predio.Campo_Descripcion;
            Cmb_Tipo_Predio.DataValueField = Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID;
            Cmb_Tipo_Predio.DataBind();
        }
        catch (Exception E)
        {
            Lbl_Ecabezado_Mensaje.Text = E.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    private Boolean Validar_Componentes()
    {
        String Mensaje_Error = "Error: ";
        Boolean valido = true;
        if (Btn_Nuevo.AlternateText == "Dar de Alta")
        {

            if (Hdf_Perito_Interno_Id.Value.Trim() == "")
            {
                if (Mensaje_Error.Length > 0)
                {
                    Mensaje_Error += "<br/>";
                }
                Mensaje_Error += "+ Seleccione el Perito interno a asignar.";
                valido = false;
            }

            if (((DataTable)Session["Dt_Cuentas_Asignadas"]) == null)
            {
                if (Mensaje_Error.Length > 0)
                {
                    Mensaje_Error += "<br/>";
                }
                Mensaje_Error += "+ Agregue cuentas al Perito interno";
                valido = false;
            }
        }
        else if (Btn_Modificar.AlternateText == "Actualizar")
        {
            if (Hdf_Perito_Interno_Id.Value.Trim() == "")
            {
                if (Mensaje_Error.Length > 0)
                {
                    Mensaje_Error += "<br/>";
                }
                Mensaje_Error += "+ Seleccione el Perito interno a asignar.";
                valido = false;
            }
            if (Hdf_Cuenta_Predial_Id.Value.Trim() == "")
            {
                if (Mensaje_Error.Length > 0)
                {
                    Mensaje_Error += "<br/>";
                }
                Mensaje_Error += "+ Seleccione la cuenta predial.";
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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Tipos_Predio
    ///DESCRIPCIÓN: Llena el combo con los tipos de predio
    ///PROPIEDADES:    
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Chk_Cuenta_Asignada_CheckedChanged(object sender, EventArgs e)
    {
        DataTable Dt_Cuentas_Asignadas = (DataTable)Session["Dt_Cuentas_Asignadas"];
        int index = 0;
        CheckBox Chk_temporal = sender as CheckBox;
        GridViewRow Gvr = Chk_temporal.NamingContainer as GridViewRow;
        index = Gvr.DataItemIndex;
        if (Chk_temporal.Checked)
        {
            Dt_Cuentas_Asignadas.Rows[index]["ACCION"] = "ALTA";
            Txt_Total_Cuentas_Asignadas.Text = (Convert.ToDouble(Txt_Total_Cuentas_Asignadas.Text) + 1).ToString("###,###,###,###,###,###,###,###");
        }
        else
        {
            Dt_Cuentas_Asignadas.Rows[index]["ACCION"] = "BAJA";
            Txt_Total_Cuentas_Asignadas.Text = (Convert.ToDouble(Txt_Total_Cuentas_Asignadas.Text) - 1).ToString("###,###,###,###,###,###,###,###");
        }
        if (Txt_Total_Cuentas.Text == Txt_Total_Cuentas_Asignadas.Text)
        {
            ((CheckBox)Grid_Cuentas_Prediales.HeaderRow.Cells[0].FindControl("Chk_Select_Todas")).Checked = true;
        }
        else
        {
            ((CheckBox)Grid_Cuentas_Prediales.HeaderRow.Cells[0].FindControl("Chk_Select_Todas")).Checked = false;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Tipos_Predio
    ///DESCRIPCIÓN: Llena el combo con los tipos de predio
    ///PROPIEDADES:    
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Chk_Select_Todas_CheckedChanged(object sender, EventArgs e)
    {
        DataTable Dt_Cuentas_Asignadas = (DataTable)Session["Dt_Cuentas_Asignadas"];
        CheckBox Chk_temporal = sender as CheckBox;
        if (Chk_temporal.Checked)
        {
            for (int i = 0; i < Dt_Cuentas_Asignadas.Rows.Count; i++)
            {
                Dt_Cuentas_Asignadas.Rows[i]["ACCION"] = "ALTA";
                //((CheckBox)Grid_Cuentas_Prediales.Rows[i].Cells[0].FindControl("Chk_Cuenta_Asignada")).Checked = true;
                Txt_Total_Cuentas_Asignadas.Text = Dt_Cuentas_Asignadas.Rows.Count.ToString("###,###,###,###,###");
            }
        }
        else
        {
            for (int i = 0; i < Dt_Cuentas_Asignadas.Rows.Count; i++)
            {
                Dt_Cuentas_Asignadas.Rows[i]["ACCION"] = "BAJA";
                //((CheckBox)Grid_Cuentas_Prediales.Rows[i].Cells[0].FindControl("Chk_Cuenta_Asignada")).Checked = false;
                Txt_Total_Cuentas_Asignadas.Text = "0";
            }
        }
        Grid_Cuentas_Prediales.Columns[1].Visible = true;
        Grid_Cuentas_Prediales.DataSource = Dt_Cuentas_Asignadas;
        Grid_Cuentas_Prediales.PageIndex = Grid_Cuentas_Prediales.PageIndex;
        Grid_Cuentas_Prediales.DataBind();
        Grid_Cuentas_Prediales.Columns[1].Visible = false;
    }

    protected void Btn_Buscar_Colonias_Click(object sender, ImageClickEventArgs e)
    {
        if (Session["BUSQUEDA_COLONIAS_CALLES"] != null)
        {
            if (Convert.ToBoolean(Session["BUSQUEDA_COLONIAS_CALLES"]) == true)
            {
                Txt_Colonia_Busqueda.Text = Session["NOMBRE_COLONIA"].ToString().Replace("&nbsp;", "");
                Txt_Calle_Busqueda.Text = Session["NOMBRE_CALLE"].ToString().Replace("&nbsp;", "");
            }
        }
    }

    protected void Btn_Imprimir_Cuentas_Click(object sender, ImageClickEventArgs e)
    {
        Imprimir_Reporte(Crear_Ds_Cuentas_Asignadas_Perito(), "Rpt_Ope_Cat_Asignacion_Cuentas_Perito.rpt", "Cuentas_Asignadas_Perito", "Window_Frm", "Cuentas_Asignadas_Perito");
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Avaluo_Urbano
    ///DESCRIPCIÓN          : Crea un DataSet para el reporte del avalúo urbano
    ///PARAMETROS: 
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 22/Junio/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Cuentas_Asignadas_Perito()
    {
        Ds_Ope_Cat_Cuentas_Asignadas Ds_Cuentas_Asignadas = new Ds_Ope_Cat_Cuentas_Asignadas();
        DataTable Dt_Cuentas;
        Dt_Cuentas = Ds_Cuentas_Asignadas.Tables["DT_CUENTAS_ASIGNADAS"];
        DataTable Dt_Perito;
        Dt_Perito = Ds_Cuentas_Asignadas.Tables["DT_DATOS_PERITO"];



        DataTable Dt_Cuentas_Asignadas;
        Dt_Cuentas_Asignadas = (DataTable)Session["Dt_Cuentas_Asignadas"];
        if (Dt_Cuentas_Asignadas.Rows.Count > 0)
        {

            DataRow Dr_Renglon_Nuevo;
            DataRow Dr_Renglon_Nuevo_P;
            foreach (DataRow Dr_Renglon_Actual in Dt_Cuentas_Asignadas.Rows)
            {
                if (Dr_Renglon_Actual["ACCION"].ToString().ToUpper() == "ALTA")
                {
                    Dr_Renglon_Nuevo = Dt_Cuentas.NewRow();
                    Dr_Renglon_Nuevo["CUENTA_PREDIAL"] = Dr_Renglon_Actual["CUENTA_PREDIAL"];
                    Dr_Renglon_Nuevo["NOMBRE_COLONIA"] = Dr_Renglon_Actual["NOMBRE_COLONIA"];
                    Dr_Renglon_Nuevo["NOMBRE_CALLE"] = Dr_Renglon_Actual["NOMBRE_CALLE"];
                    Dr_Renglon_Nuevo["NO_EXTERIOR"] = Dr_Renglon_Actual["NO_EXTERIOR"];
                    Dr_Renglon_Nuevo["NO_INTERIOR"] = Dr_Renglon_Actual["NO_INTERIOR"];
                    Dr_Renglon_Nuevo["EFECTOS"] = Dr_Renglon_Actual["EFECTOS"];
                    Dr_Renglon_Nuevo["SUPERFICIE_CONSTRUIDA"] = Dr_Renglon_Actual["SUPERFICIE_CONSTRUIDA"];
                    Dr_Renglon_Nuevo["SUPERFICIE_TOTAL"] = Dr_Renglon_Actual["SUPERFICIE_TOTAL"];
                    Dr_Renglon_Nuevo["AGRUPACION"] = "A";

                    Dt_Cuentas.Rows.Add(Dr_Renglon_Nuevo);


                }
            }
            Dr_Renglon_Nuevo_P = Dt_Perito.NewRow();
            Dr_Renglon_Nuevo_P["NOMBRE_PERITO"] = Txt_Perito_Interno.Text.ToUpper();
            Dr_Renglon_Nuevo_P["TOTAL_CUENTAS"] = Txt_Total_Cuentas_Asignadas.Text;
            Dt_Perito.Rows.Add(Dr_Renglon_Nuevo_P);
        }
        else
        {
            Lbl_Ecabezado_Mensaje.Text = "Usted no es un perito interno vigente.";
            Lbl_Ecabezado_Mensaje.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Ds_Cuentas_Asignadas;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Avaluo_Urbano
    ///DESCRIPCIÓN          : Crea un DataSet para el reporte del avalúo urbano
    ///PARAMETROS: 
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 22/Junio/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Colonias_Evaluar()
    {
        Ds_Ope_Cat_Colonias_Evaluar Ds_Cuentas_Asignadas = new Ds_Ope_Cat_Colonias_Evaluar();
        DataTable Dt_Cuentas;
        Dt_Cuentas = Ds_Cuentas_Asignadas.Tables["DT_COLONIAS"];

        try
        {
            Dt_Cuentas = Llenar_Colonias_Actualizar();
            Dt_Cuentas.TableName = "DT_COLONIAS";
            Ds_Cuentas_Asignadas.Tables.Remove("DT_COLONIAS");
            Ds_Cuentas_Asignadas.Tables.Add(Dt_Cuentas.Copy());
        }
        catch
        {
        }
        return Ds_Cuentas_Asignadas;
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
    private DataTable Llenar_Colonias_Actualizar()
    {
        DataTable Dt_Cuentas_Prediales = new DataTable(); ;
        try
        {
            Cls_Ope_Cat_Asignacion_Cuentas_Negocio Cuentas = new Cls_Ope_Cat_Asignacion_Cuentas_Negocio();
            if (Txt_Efecto_Anio.Text.Trim() != "")
            {
                Cuentas.P_Efecto_Anio = Txt_Efecto_Anio.Text.Trim();
            }
            else
            {
                Cuentas.P_Efecto_Anio = DateTime.Now.Year.ToString();
            }
            if (Txt_Efecto_Bimestre.Text.Trim() != "")
            {
                Cuentas.P_Efecto_Bimestre = Txt_Efecto_Bimestre.Text.Trim();
            }
            else
            {
                Cuentas.P_Efecto_Bimestre = (DateTime.Now.Month / 2).ToString();
            }
            Dt_Cuentas_Prediales = Cuentas.Consultas_Colonias_Actualizar();
        }
        catch (Exception E)
        {
            Lbl_Ecabezado_Mensaje.Text = E.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Dt_Cuentas_Prediales;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Imprimir_Reporte
    ///DESCRIPCIÓN          : Genera el reporte de Crystal con los datos proporcionados en el DataTable y lo manda a la impresora default
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Imprimir_Reporte(DataSet Ds_Convenios, String Nombre_Reporte, String Nombre_Archivo, String Formato, String Tipo)
    {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Catastro/" + Nombre_Reporte);
        try
        {
            Reporte.Load(File_Path);
            Reporte.SetDataSource(Ds_Convenios);
        }
        catch
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "No se pudo cargar el reporte para su impresión";
        }

        String Archivo_PDF = Nombre_Archivo + ".pdf";  // Es el nombre del PDF que se va a generar    
        try
        {
            ExportOptions Export_Options = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
            Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Archivo_PDF);
            Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
            Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
            Reporte.Export(Export_Options);
        }
        catch (Exception Ex)
        {
            //Lbl_Mensaje_Error.Visible = true;
            //Lbl_Mensaje_Error.Text = "No se pudo exportar el reporte";
        }

        try
        {
            Mostrar_Reporte(Archivo_PDF, Tipo, Formato);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Mostrar_Reporte
    ///DESCRIPCIÓN          : Visualiza en pantalla el reporte indicado
    ///PARAMETROS           : Nombre_Reporte: cadena con el nombre del archivo.
    ///                     : Formato: Exensión del archivo a visualizar.
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Mostrar_Reporte(String Nombre_Reporte, String Formato, String Frm_Formato)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            //if (Formato == "PDF")
            //{
            Pagina = Pagina + Nombre_Reporte;
            ScriptManager.RegisterStartupScript(this, this.GetType(), Frm_Formato, "window.open('" + Pagina + "', '" + Formato + "','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            //}
            //else if (Formato == "Excel")
            //{
            //    String Ruta = "../../Reporte/" + Nombre_Reporte;
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            //}
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    protected void Btn_Imprimir_Colonias_Click(object sender, ImageClickEventArgs e)
    {
        Imprimir_Reporte(Crear_Ds_Colonias_Evaluar(), "Rpt_Ope_Cat_Colonias_Evaluar.rpt", "Colonias_Actualizar", "Window_Frm", "Colonias_Actualizar");
    }
}