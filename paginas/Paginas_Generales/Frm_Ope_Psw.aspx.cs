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
using Presidencia.Asignar_Password.Negocio;
using Presidencia.Seguridad;
using Presidencia.Constantes;
public partial class paginas_Paginas_Generales_Frm_Ope_Psw : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Cls_Sessiones.Mostrar_Menu = true;
            Llenar_Combos();
            Configurar_Formulario("Inicio");

        }
        Txt_Password.Attributes.Add("value", Txt_Password.Text);
        Txt_Confirmar_Password.Attributes.Add("value", Txt_Confirmar_Password.Text);
        Mostrar_Informacion("", false);
    }

    public void Configurar_Formulario(String Estatus)
    {
        switch(Estatus)
        {
            case "Inicio":
                Btn_Buscar.Enabled = true;
                Grid_Dependencias.Enabled = false;
                Btn_Salir.ToolTip = "Salir";
                Btn_Actualizar_Datos.ToolTip = "Modificar";
                Btn_Actualizar_Datos.Enabled = true;
                Btn_Actualizar_Datos.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Cmb_Dependencias.Enabled = false;
                Btn_Agregar_Dependencia.Enabled = false;
                Txt_Password.Enabled = false;
                Txt_Confirmar_Password.Enabled = false;
                Cmb_Rol_Empleado.Enabled = false;
                Grid_Dependencias.DataSource = new DataTable();
                Grid_Dependencias.DataBind();
                //Campos Limpios
                Txt_No_Empleado.Text = "";
                Txt_Nombre.Text = "";
                Txt_Confirmar_Password.Text = "";
                Txt_Password.Text = "";
                Cmb_Dependencias.SelectedIndex = 0;
                Cmb_Rol_Empleado.SelectedIndex = 0;
                Grid_Dependencias.DataSource = new DataTable();
                Grid_Dependencias.DataBind();


                break;
            case "General":


                break;
            case "Modificar":

                Btn_Buscar.Enabled = false;
                Grid_Dependencias.Enabled = true;
                Btn_Salir.ToolTip = "Salir";
                Btn_Actualizar_Datos.ToolTip = "Actualizar";
                Btn_Actualizar_Datos.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                Cmb_Dependencias.Enabled = true;
                Btn_Agregar_Dependencia.Enabled = true;
                Txt_Password.Enabled = true;
                Txt_Confirmar_Password.Enabled = true;
                Cmb_Rol_Empleado.Enabled = true;
                

                break;

        }//fin del switch

    }

    private void Llenar_Combos() 
    {
        Cls_Ope_Psw_Negocio Negocio = new Cls_Ope_Psw_Negocio();
        DataTable Dt_Roles = Negocio.Consultar_Roles();
        Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Rol_Empleado, Dt_Roles, 1, 0);

        DataTable Dt_Dependencias = Negocio.Consultar_Dependencias();
        Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Dependencias, Dt_Dependencias, 1, 0);
        Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_UR_Empleado, Dt_Dependencias, 1, 0);

    }
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Grid_Dependencias.DataSource = new DataTable();
        Grid_Dependencias.DataBind();
        Session["Dt_Dependencias"] = null;
        Cls_Ope_Psw_Negocio Negocio = new Cls_Ope_Psw_Negocio();
        Negocio.P_No_Empleado = Txt_No_Empleado.Text.Trim();
        DataTable Dt_Empleado = Negocio.Consultar_Empleado();
        if (Dt_Empleado != null && Dt_Empleado.Rows.Count > 0)
        {
            try
            {
                //String Psw = Cls_Seguridad.Desencriptar(Dt_Empleado.Rows[0]["PASSWORD"].ToString().Trim());
                String Psw = (Dt_Empleado.Rows[0]["PASSWORD"].ToString().Trim());
                Txt_Nombre.Text = Dt_Empleado.Rows[0]["NOMBRE_EMPLEADO"].ToString().Trim();
                Txt_Password.Text = Psw;
                Txt_Confirmar_Password.Text = Psw;
                Txt_Password.Attributes.Add("value", Txt_Password.Text);
                Txt_Confirmar_Password.Attributes.Add("value", Txt_Confirmar_Password.Text);
                Cmb_Rol_Empleado.SelectedValue = Dt_Empleado.Rows[0]["ROL_ID"].ToString().Trim();
                Cmb_UR_Empleado.SelectedValue = Dt_Empleado.Rows[0]["DEPENDENCIA_ID"].ToString().Trim();
                Cmb_UR_Empleado.Enabled = false;
                DataTable Dt_Dependencias = Negocio.Consultar_Detalle_UR_Empleado();
                Session["Dt_Dependencias"] = Dt_Dependencias;
                if (Dt_Dependencias.Rows.Count > 0)
                {
                    Grid_Dependencias.DataSource = Dt_Dependencias;
                    Grid_Dependencias.DataBind();
                    Session["Dt_Dependencias"] = Dt_Dependencias;
                   
                }
                else
                {
                    Grid_Dependencias.DataSource = new DataTable();
                    Grid_Dependencias.DataBind();

                }

            }
            catch (Exception Ex)
            {
                Ex.ToString();
                //Mostrar_Informacion(Ex.ToString(), true);
            }
        }
        else 
        {
            Mostrar_Informacion("No se encontró información con el número de empleado proporcionado", true);
            Txt_Nombre.Text = "";
            Txt_Password.Text = "";
            Txt_Confirmar_Password.Text = "";
            Cmb_Rol_Empleado.SelectedIndex = 0;
            Txt_Password.Attributes.Add("value", "");
            Txt_Confirmar_Password.Attributes.Add("value", "");
        }
    }
    ///*******************************************************************************
    //NOMBRE DE LA FUNCIÓN: Mostrar_Información
    //DESCRIPCIÓN: Llena las areas de texto con el registro seleccionado del grid
    //RETORNA: 
    //CREO: Gustavo Angeles Cruz
    //FECHA_CREO: 24/Agosto/2010 
    //MODIFICO:
    //FECHA_MODIFICO:
    //CAUSA_MODIFICACIÓN:
    //********************************************************************************/
    protected void Grid_Dependencias_SelectedIndexChanged(object sender, EventArgs e)
    {

        DataRow[] Renglones;
        DataRow Renglon;
        //Obtenemos el Id del producto seleccionado
        GridViewRow selectedRow = Grid_Dependencias.Rows[Grid_Dependencias.SelectedIndex];
        String Id = Convert.ToString(selectedRow.Cells[1].Text);
        DataTable Dt_Dependencias = (DataTable)Session["Dt_Dependencias"];
        Renglones = ((DataTable)Session["Dt_Dependencias"]).Select(Cat_Empleados.Campo_Dependencia_ID + "='" + Id + "'");

        if (Renglones.Length > 0)
        {
            Renglon = Renglones[0];
            DataTable Tabla = (DataTable)Session["Dt_Dependencias"];
            Tabla.Rows.Remove(Renglon);
            Session["Dt_Dependencias"] = Tabla;
            Grid_Dependencias.SelectedIndex = (-1);
            Grid_Dependencias.DataSource = Tabla;
            Grid_Dependencias.DataBind();
            
            
        }
    }



    private void Mostrar_Informacion(String txt, Boolean mostrar)
    {
        Lbl_Informacion.Style.Add("color", "#990000");
        Lbl_Informacion.Visible = mostrar;
        Img_Warning.Visible = mostrar;
        Lbl_Informacion .Text = txt;
    }
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
    }
    protected void Btn_Actualizar_Datos_Click(object sender, ImageClickEventArgs e)
    {
        Lbl_Informacion.Text = "";
        switch (Btn_Actualizar_Datos.ToolTip)
        {
            case "Modificar":
                if (Txt_No_Empleado.Text == String.Empty)
                {
                    Div_Encabezado.Visible = true;
                    Lbl_Informacion.Text = "Es necesario seleccionar un No_Empleado.";
                }
                else
                    Configurar_Formulario("Modificar");
                break;

            case "Actualizar":
                if (Txt_Password.Text.Trim() == Txt_Confirmar_Password.Text.Trim())
                {
                    Cls_Ope_Psw_Negocio Negocio = new Cls_Ope_Psw_Negocio();
                    Negocio.P_No_Empleado = Txt_No_Empleado.Text.Trim();
                    Negocio.P_Rol_ID = Cmb_Rol_Empleado.SelectedValue.Trim();
                    Negocio.P_Dt_Dependencias = (DataTable)Session["Dt_Dependencias"];
                    //Negocio.P_Password = Cls_Seguridad.Encriptar(Txt_Password.Text.Trim());
                    Negocio.P_Password = Txt_Password.Text.Trim();
                    int Registros = Negocio.Actualizar_Empleado();
                    bool Operacion_Realizada = Negocio.Guardar_Detalle_UR();
                    if ((Registros > 0) && (Operacion_Realizada=true))
                    {
                        Configurar_Formulario("Inicio");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('Se actualizaron datos del empleado');", true);

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('No se actualizaron datos del empleado');", true);
                    }
                }
                else 
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('La confirmación del Password es incorrecta');", true);
                }
        break;
    }
    }



    protected void Btn_Agregar_Dependencia_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Ope_Psw_Negocio Clase_Negocio = new Cls_Ope_Psw_Negocio();
        DataTable Dt_Dependencias = new DataTable();
        if (Session["Dt_Dependencias"] != null)
        {
            
            Dt_Dependencias = (DataTable)Session["Dt_Dependencias"];
            DataRow[] Fila_Nueva;
            Fila_Nueva = Dt_Dependencias.Select("DEPENDENCIA_ID = '" + Cmb_Dependencias.SelectedValue.Trim() + "'");
            if (Fila_Nueva.Length > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                "alert('No se puede agregar la dependencia, ya se ha agregado');", true);
            }
            else
            {
                DataRow Fila = Dt_Dependencias.NewRow();
                Fila["Dependencia_ID"] = Cmb_Dependencias.SelectedValue.ToString();
                Fila["Dependencia"] = Cmb_Dependencias.SelectedItem.Text;
                Session["Dt_Dependencias"] = Dt_Dependencias;
                Dt_Dependencias.Rows.Add(Fila);
                Dt_Dependencias.AcceptChanges();
                Grid_Dependencias.DataSource = Dt_Dependencias;
                Session["Dt_Dependencias"] = Dt_Dependencias;
                Grid_Dependencias.DataBind();
            }
        }//fin if
        else
        {
            //Creamos la tabla 
            
            Dt_Dependencias.Columns.Add("Dependencia_ID", typeof(System.String));
            Dt_Dependencias.Columns.Add("Dependencia", typeof(System.String));
            Session["Dt_Dependencias"] = Dt_Dependencias;
            DataRow[] Fila_Nueva;
            Fila_Nueva = Dt_Dependencias.Select("DEPENDENCIA_ID = '" + Cmb_Dependencias.SelectedValue.Trim() + "'");
            if (Fila_Nueva.Length > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                "alert('No se puede agregar la dependencia, ya se ha agregado');", true);
            }
            else
            {
                DataRow Fila = Dt_Dependencias.NewRow();
                Fila["Dependencia_ID"] = Cmb_Dependencias.SelectedValue.ToString();
                Fila["Dependencia"] = Cmb_Dependencias.SelectedItem.Text;
                Session["Dt_Dependencias"] = Dt_Dependencias;
                Dt_Dependencias.Rows.Add(Fila);
                Dt_Dependencias.AcceptChanges();
                Grid_Dependencias.DataSource = Dt_Dependencias;
                Session["Dt_Dependencias"] = Dt_Dependencias;
                Grid_Dependencias.DataBind();
            }


        }
        }



    }

