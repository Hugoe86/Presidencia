using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Operacion_Cat_Asignacion_Cuentas.Negocio;
using System.Data;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Tipos_Predio.Negocio;
using Presidencia.Constantes;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class paginas_Catastro_Frm_Ope_Cat_Entregas_Avaluos : System.Web.UI.Page
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
                Configuracion_Formulario(false);
                Llenar_Combo_Tipos_Predio();
                Llenar_Entregas(0);
                //Calcular_Total_Entrega_Predios_Municipio();
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
                Cuentas.P_Anio = Txt_Anio.Text.Trim();
                Cuentas.P_No_Entrega = Cmb_Entrega.SelectedValue;
                Cuentas.P_Dt_Cuentas = (DataTable)Session["Dt_Entrega_Nueva"];
                if ((Cuentas.Alta_Entregas()))
                {
                    Configuracion_Formulario(false);
                    Llenar_Entregas(Grid_Entregas.PageIndex);
                    Btn_Modificar.Visible = true;
                    Btn_Modificar.AlternateText = "Modificar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Nuevo.Visible = true;
                    Btn_Nuevo.AlternateText = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Grid_Entregas.SelectedIndex = -1;
                    Limpiar_Formulario();
                    Div_Modificar.Visible = true;
                    Div_Nuevo.Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Entregas", "alert('La entrega ha sido generada exitosamente.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Entregas", "alert('Error al intentar generar la entrega.');", true);
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
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {

    }
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
            Llenar_Entregas(0);
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Grid_Entregas.DataSource = null;
            Grid_Entregas.DataBind();
            Limpiar_Formulario();
        }
    }
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Llenar_Entregas(0);
    }
    protected void Grid_Entregas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Llenar_Entregas(e.NewPageIndex);
    }
    protected void Grid_Entregas_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void Btn_Buscar_Cuentas_Click(object sender, ImageClickEventArgs e)
    {
        Llenar_Entregas_Nueva(0);
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
    protected void Grid_Entrega_Nueva_DataBound(object sender, EventArgs e)
    {
        int cuentas_asignadas = 0;
        DataTable Dt_Cuentas_Asignadas = (DataTable)Session["Dt_Entrega_Nueva"];
        int indice = (Grid_Entrega_Nueva.PageIndex * 200) + 200;
        if (indice <= Dt_Cuentas_Asignadas.Rows.Count)
        {
            indice = 200;
        }
        else
        {
            indice = Dt_Cuentas_Asignadas.Rows.Count - (Grid_Entrega_Nueva.PageIndex * 200);
        }
        for (int i = 0; i < indice; i++)
        {
            CheckBox Chk_Cuenta_Asignada = (CheckBox)Grid_Entrega_Nueva.Rows[i].Cells[0].FindControl("Chk_Cuenta_Asignada");
            if (Dt_Cuentas_Asignadas.Rows[(Grid_Entrega_Nueva.PageIndex * 200) + i]["ACCION"].ToString() == "ALTA")
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
        Txt_Total_Entrega.Text = Dt_Cuentas_Asignadas.Rows.Count.ToString("###,###,###,###,###,##0");
        Txt_Total_Entregas_Seleccionadas.Text = cuentas_asignadas.ToString("###,###,###,###,###,##0");
        if (Grid_Entrega_Nueva.Rows.Count > 0)
        {
            if (Txt_Total_Entrega.Text == Txt_Total_Entregas_Seleccionadas.Text)
            {
                ((CheckBox)Grid_Entrega_Nueva.HeaderRow.Cells[0].FindControl("Chk_Select_Todas")).Checked = true;
            }
            else
            {
                ((CheckBox)Grid_Entrega_Nueva.HeaderRow.Cells[0].FindControl("Chk_Select_Todas")).Checked = false;
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Entrega_Nueva_PageIndexChanging
    ///DESCRIPCIÓN: Cambia la página del grid
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Entrega_Nueva_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable Dt_Cuentas_Prediales = (DataTable)Session["Dt_Entrega_Nueva"];
        Grid_Entrega_Nueva.Columns[1].Visible = true;
        Grid_Entrega_Nueva.DataSource = Dt_Cuentas_Prediales;
        Grid_Entrega_Nueva.PageIndex = e.NewPageIndex;
        Grid_Entrega_Nueva.DataBind();
        Grid_Entrega_Nueva.Columns[1].Visible = false;
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
        DataTable Dt_Cuentas_Asignadas = (DataTable)Session["Dt_Entrega_Nueva"];
        int index = 0;
        CheckBox Chk_temporal = sender as CheckBox;
        GridViewRow Gvr = Chk_temporal.NamingContainer as GridViewRow;
        index = Gvr.DataItemIndex;
        if (Chk_temporal.Checked)
        {
            Dt_Cuentas_Asignadas.Rows[index]["ACCION"] = "ALTA";
            Txt_Total_Entregas_Seleccionadas.Text = (Convert.ToDouble(Txt_Total_Entregas_Seleccionadas.Text) + 1).ToString("###,###,###,###,###,###,###,##0");
        }
        else
        {
            Dt_Cuentas_Asignadas.Rows[index]["ACCION"] = "BAJA";
            Txt_Total_Entregas_Seleccionadas.Text = (Convert.ToDouble(Txt_Total_Entregas_Seleccionadas.Text) - 1).ToString("###,###,###,###,###,###,###,##0");
        }
        if (Txt_Total_Entrega.Text == Txt_Total_Entregas_Seleccionadas.Text)
        {
            ((CheckBox)Grid_Entrega_Nueva.HeaderRow.Cells[0].FindControl("Chk_Select_Todas")).Checked = true;
        }
        else
        {
            ((CheckBox)Grid_Entrega_Nueva.HeaderRow.Cells[0].FindControl("Chk_Select_Todas")).Checked = false;
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
        DataTable Dt_Cuentas_Asignadas = (DataTable)Session["Dt_Entrega_Nueva"];
        CheckBox Chk_temporal = sender as CheckBox;
        if (Chk_temporal.Checked)
        {
            for (int i = 0; i < Dt_Cuentas_Asignadas.Rows.Count; i++)
            {
                Dt_Cuentas_Asignadas.Rows[i]["ACCION"] = "ALTA";
                //((CheckBox)Grid_Cuentas_Prediales.Rows[i].Cells[0].FindControl("Chk_Cuenta_Asignada")).Checked = true;
                Txt_Total_Entregas_Seleccionadas.Text = Dt_Cuentas_Asignadas.Rows.Count.ToString("###,###,###,###,##0");
            }
        }
        else
        {
            for (int i = 0; i < Dt_Cuentas_Asignadas.Rows.Count; i++)
            {
                Dt_Cuentas_Asignadas.Rows[i]["ACCION"] = "BAJA";
                //((CheckBox)Grid_Cuentas_Prediales.Rows[i].Cells[0].FindControl("Chk_Cuenta_Asignada")).Checked = false;
                Txt_Total_Entregas_Seleccionadas.Text = "0";
            }
        }
        Grid_Entrega_Nueva.Columns[1].Visible = true;
        Grid_Entrega_Nueva.DataSource = Dt_Cuentas_Asignadas;
        Grid_Entrega_Nueva.PageIndex = Grid_Entrega_Nueva.PageIndex;
        Grid_Entrega_Nueva.DataBind();
        Grid_Entrega_Nueva.Columns[1].Visible = false;
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
        Txt_Busqueda.Text = "";
        Txt_Cuenta_Predial_Busqueda.Text = "";
        Txt_Calle_Busqueda.Text = "";
        Txt_Colonia_Busqueda.Text = "";
        Txt_No_Ext_Busqueda.Text = "";
        Txt_No_Int_Busqueda.Text = "";
        Txt_Terreno_Menor.Text = "";
        Txt_Terreno.Text = "";
        Txt_Construccion.Text = "";
        Txt_Construccion_Menor.Text = "";
        Txt_Total_Entrega.Text = "0";
        Txt_Total_Entregas_Seleccionadas.Text = "0";
        Txt_Entregas_Estrategicas.Text = "0";
        Txt_Entregas_Municipio.Text = "0";
        Txt_Entregas_Rusticas.Text = "0";
        Txt_Entregas_Urbanas.Text = "0";
        Txt_Anio.Text = DateTime.Now.Year.ToString();
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
        Cmb_Tipo_Predio.Enabled = Enabled;
        Btn_Buscar_Cuentas.Enabled = Enabled;
        Cmb_Entrega.Enabled = !Enabled;
        Txt_Anio.Enabled = !Enabled;
        Txt_Propietario.Enabled = Enabled;
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
    private void Llenar_Entregas_Nueva(int Pagina)
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
            Cuentas.P_Anio_Nulo = true;
            Dt_Cuentas_Prediales = Cuentas.Consultar_Cuentas_Entregar();
            Session["Dt_Entrega_Nueva"] = Dt_Cuentas_Prediales.Copy();
            Grid_Entrega_Nueva.Columns[1].Visible = true;
            Grid_Entrega_Nueva.DataSource = Dt_Cuentas_Prediales;
            Grid_Entrega_Nueva.PageIndex = 0;
            Grid_Entrega_Nueva.DataBind();
            Grid_Entrega_Nueva.Columns[1].Visible = false;
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
    private void Llenar_Entregas(int Pagina)
    {
        try
        {
            Cls_Ope_Cat_Asignacion_Cuentas_Negocio Cuentas = new Cls_Ope_Cat_Asignacion_Cuentas_Negocio();
            DataTable Dt_Cuentas_Asignadas;

            //if (Cmb_Busqueda.SelectedValue == "COLONIA")
            //{
            //    Cuentas.P_Colonia = Txt_Busqueda.Text.ToUpper();
            //}
            //else if (Cmb_Busqueda.SelectedValue == "CALLE")
            //{
            //    Cuentas.P_Calle = Txt_Busqueda.Text.ToUpper();
            //}
            //else if (Cmb_Busqueda.SelectedValue == "PERITO")
            //{
            //    Cuentas.P_Perito = Txt_Busqueda.Text.ToUpper();
            //}
            //else if (Cmb_Busqueda.SelectedValue == "CUENTA_PREDIAL")
            //{
            //    Cuentas.P_Cuenta_Predial = Txt_Busqueda.Text.ToUpper();
            //}
            if (Txt_Anio.Text.Trim() != "")
            {
                Cuentas.P_Anio = Txt_Anio.Text.Trim();
            }
            else
            {
                Txt_Anio.Text = DateTime.Now.Year.ToString();
                Cuentas.P_Anio = DateTime.Now.Year.ToString();
            }
            Cuentas.P_No_Entrega = Cmb_Entrega.SelectedValue;
            Dt_Cuentas_Asignadas = Cuentas.Consultar_Cuentas_Entregar();
            Grid_Entregas.Columns[1].Visible = true;
            Grid_Entregas.Columns[3].Visible = true;
            Grid_Entregas.DataSource = Dt_Cuentas_Asignadas;
            Grid_Entregas.PageIndex = Pagina;
            Grid_Entregas.DataBind();
            Grid_Entregas.Columns[1].Visible = false;
            Grid_Entregas.Columns[3].Visible = false;
            Calcular_Totales_Entrega(Dt_Cuentas_Asignadas);
        }
        catch (Exception E)
        {
            Lbl_Ecabezado_Mensaje.Text = E.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    protected void Cmb_Entrega_SelectedIndexChanged(object sender, EventArgs e)
    {
        Llenar_Entregas(0);
    }
    protected void Txt_Anio_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Txt_Anio.Text.Trim() == "")
            {
                Txt_Anio.Text = DateTime.Now.Year.ToString();
            }
            else
            {
                Txt_Anio.Text = Txt_Anio.Text.Trim();
            }
            Llenar_Entregas(0);
        }
        catch
        {

        }
    }

    private void Calcular_Totales_Entrega(DataTable Dt_Entrega)
    {
        Double Total_Urbano = 0;
        Double Total_Rustico = 0;
        Double Total_Municipio = 0;
        Double Total_Estrategico = 0;
        foreach (DataRow Dr_Renglon in Dt_Entrega.Rows)
        {
            if (Convert.ToDouble(Dr_Renglon["PORCENTAJE_EXENCION"].ToString()) == 100)
            {
                Total_Municipio++;
            }
            else if (Convert.ToDouble(Dr_Renglon["SUPERFICIE_TOTAL"].ToString()) >= 2000)
            {
                Total_Estrategico++;
            }
            else if (Dr_Renglon["TIPO_PREDIO"].ToString()=="URBANO")
            {
                Total_Urbano++;
            }
            else if (Dr_Renglon["TIPO_PREDIO"].ToString() == "RUSTICO")
            {
                Total_Rustico++;
            }
            
        }
        Txt_Entregas_Estrategicas.Text = Total_Estrategico.ToString("###,###,###,###,###,##0");
        Txt_Entregas_Municipio.Text = Total_Municipio.ToString("###,###,###,###,###,##0");
        Txt_Entregas_Urbanas.Text = Total_Urbano.ToString("###,###,###,###,###,##0");
        Txt_Entregas_Rusticas.Text = Total_Rustico.ToString("###,###,###,###,###,##0");

    }



    private Boolean Validar_Componentes()
    {
        DataTable Dt_Entrega_Nueva = (DataTable)Session["Dt_Entrega_Nueva"];
        String Mensaje_Error = "Error: ";
        Boolean valido = true;
        if (Btn_Nuevo.AlternateText == "Dar de Alta")
        {

            if (Dt_Entrega_Nueva==null || Dt_Entrega_Nueva.Rows.Count == 0)
            {
                if (Mensaje_Error.Length > 0)
                {
                    Mensaje_Error += "<br/>";
                }
                Mensaje_Error += "+ Seleccione los avalúos a Entregar.";
                valido = false;
            }
        }
        else if (Btn_Modificar.AlternateText == "Actualizar")
        {
            if (true)
            {
                if (Mensaje_Error.Length > 0)
                {
                    Mensaje_Error += "<br/>";
                }
                Mensaje_Error += "+ Falta informacion por llenar.";
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
    protected void Btn_Imprime_Reporte_Click(object sender, ImageClickEventArgs e)
    {
        Imprimir_Reporte(Crear_Ds_Entregas(), "Rpt_Ope_Cat_Entregas.rpt", "Entrega", "Window_Frm", "Entrega");
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
    private DataSet Crear_Ds_Entregas()
    {
        Ds_Ope_Cat_Entregas Ds_Cuentas_Asignadas = new Ds_Ope_Cat_Entregas();
        DataTable Dt_Cuentas;
        Dt_Cuentas = Ds_Cuentas_Asignadas.Tables["Dt_Entergas"];
        Cls_Ope_Cat_Asignacion_Cuentas_Negocio Cuentas = new Cls_Ope_Cat_Asignacion_Cuentas_Negocio();
        Cuentas.P_No_Entrega = Cmb_Entrega.SelectedValue;
        if (Txt_Anio.Text.Trim() == "")
        {
            Cuentas.P_Anio = DateTime.Now.Year.ToString();
        }
        else
        {
            Cuentas.P_Anio = Txt_Anio.Text.Trim();
        }
        DataTable Dt_Cuentas_Asignadas;
        Dt_Cuentas_Asignadas = Cuentas.Consultar_Cuentas_Entregar();
        if (Dt_Cuentas_Asignadas.Rows.Count > 0)
        {
            DataRow Dr_Renglon_Nuevo;
            foreach (DataRow Dr_Renglon_Actual in Dt_Cuentas_Asignadas.Rows)
            {
                Dr_Renglon_Nuevo = Dt_Cuentas.NewRow();
                Dr_Renglon_Nuevo["CUENTA"] = Dr_Renglon_Actual["CUENTA_PREDIAL"];
                Dr_Renglon_Nuevo["NOMBRE_COLONIA"] = Dr_Renglon_Actual["NOMBRE_COLONIA"];
                Dr_Renglon_Nuevo["NOMBRE_CALLE"] = Dr_Renglon_Actual["NOMBRE_CALLE"];
                Dr_Renglon_Nuevo["NUMERO_EXTERIOR"] = Dr_Renglon_Actual["NO_EXTERIOR"];
                Dr_Renglon_Nuevo["NUMERO_INTERIOR"] = Dr_Renglon_Actual["NO_INTERIOR"];
                Dr_Renglon_Nuevo["FOLIO_PREDIAL"] = Dr_Renglon_Actual["FOLIO_PREDIAL"];
                Dr_Renglon_Nuevo["FOLIO_CATASTRO"] = Dr_Renglon_Actual["FOLIO_CATASTRO"];
                Dr_Renglon_Nuevo["NO_ENTREGA"] = Cuentas.P_Anio + "/" + Cmb_Entrega.SelectedItem.Text.ToUpper();
                Dt_Cuentas.Rows.Add(Dr_Renglon_Nuevo);
            }
        }
        else
        {
            Lbl_Ecabezado_Mensaje.Text = "No existen entregas.";
            Lbl_Ecabezado_Mensaje.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Ds_Cuentas_Asignadas;
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
}