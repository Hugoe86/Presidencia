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
using Presidencia.Control_Patrimonial_Catalogo_Usos_Inmuebles.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Destinos_Inmuebles.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Bienes_Inmuebles.Negocio;
using Presidencia.Catalogo_Tipos_Predio.Negocio;
using Presidencia.Catalogo_Calles.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Colonias.Negocios;

public partial class paginas_Control_Patrimonial_Frm_Ope_Pat_Entrada_Bienes_Inmuebles : System.Web.UI.Page {

    #region "Page Load"

    protected void Page_Load(object sender, EventArgs e) {
        Lbl_Ecabezado_Mensaje.Text = "";
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
        if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Trim().Length == 0) {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
        }
        if (!IsPostBack) {
            
            Session.Remove("FILTRO_BUSQUEDA_INMUEBLES");
            Llenar_Combo_Destino();
            Llenar_Combo_Tipo_Predio();
            Llenar_Combo_Uso();
            Grid_Listado_Busqueda_Bienes_Inmuebles.PageIndex = 0;
            Llenar_Listado_Busqueda_Bienes_Inmuebles();
        }
    }

    #endregion

    #region "Metodos"

        private void Llenar_Combo_Uso() {
            Cls_Cat_Pat_Com_Usos_Inmuebles_Negocio Uso_Suelo = new Cls_Cat_Pat_Com_Usos_Inmuebles_Negocio();
            Uso_Suelo.P_Estatus = "VIGENTE";
            Cmb_Uso.DataSource = Uso_Suelo.Consultar_Usos();
            Cmb_Uso.DataTextField = "DESCRIPCION";
            Cmb_Uso.DataValueField = "USO_ID";
            Cmb_Uso.DataBind();
            Cmb_Uso.Items.Insert(0, new ListItem("<SELECCIONE>", ""));
        }
            
        private void Llenar_Combo_Destino() {
            Cls_Cat_Pat_Com_Destinos_Inmuebles_Negocio Destino_Suelo = new Cls_Cat_Pat_Com_Destinos_Inmuebles_Negocio();
            Destino_Suelo.P_Estatus = "VIGENTE";
            Cmb_Destino.DataSource = Destino_Suelo.Consultar_Destinos();
            Cmb_Destino.DataTextField = "DESCRIPCION";
            Cmb_Destino.DataValueField = "DESTINO_ID";
            Cmb_Destino.DataBind();
            Cmb_Destino.Items.Insert(0, new ListItem("<SELECCIONE>", ""));
        }
            
        private void Llenar_Combo_Tipo_Predio() {
            Cls_Cat_Pre_Tipos_Predio_Negocio Tipo_Predio = new Cls_Cat_Pre_Tipos_Predio_Negocio();
            Cmb_Tipo_Predio.DataSource = Tipo_Predio.Consultar_Tipo_Predio();
            Cmb_Tipo_Predio.DataTextField = "DESCRIPCION";
            Cmb_Tipo_Predio.DataValueField = "TIPO_PREDIO_ID";
            Cmb_Tipo_Predio.DataBind();
            Cmb_Tipo_Predio.Items.Insert(0, new ListItem("<SELECCIONE>", ""));
        }

        private void Llenar_Listado_Busqueda_Bienes_Inmuebles() {
            Grid_Listado_Busqueda_Bienes_Inmuebles.SelectedIndex = (-1);
            Cls_Ope_Pat_Bienes_Inmuebles_Negocio Tmp_Negocio = new Cls_Ope_Pat_Bienes_Inmuebles_Negocio();
            Tmp_Negocio.P_Calle = Hdf_Calle_ID.Value;
            Tmp_Negocio.P_Colonia = Hdf_Colonia_ID.Value;
            Tmp_Negocio.P_Uso_ID = Cmb_Uso.SelectedItem.Value; 
            Tmp_Negocio.P_Destino_ID = Cmb_Destino.SelectedItem.Value;
            Tmp_Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            Tmp_Negocio.P_Tipo_Predio_ID = Cmb_Tipo_Predio.SelectedItem.Value;
            Tmp_Negocio.P_No_Escritura = Txt_Escritura.Text.Trim();
            Tmp_Negocio.P_Bien_Inmueble_ID = (Txt_Bien_Mueble_ID.Text.Trim().Length > 0) ? String.Format("{0:0000000000}", Convert.ToInt32(Txt_Bien_Mueble_ID.Text.Trim())) : "";
            if (Txt_Superficie_Desde.Text.Trim().Length > 0) { Tmp_Negocio.P_Construccion_Desde = Convert.ToDouble(Txt_Superficie_Desde.Text); }
            if (Txt_Superficie_Hasta.Text.Trim().Length > 0) { Tmp_Negocio.P_Construccion_Hasta = Convert.ToDouble(Txt_Superficie_Hasta.Text); }
            DataTable Dt_Datos = Tmp_Negocio.Consultar_Bienes_Inmuebles();
            Grid_Listado_Busqueda_Bienes_Inmuebles.DataSource = Dt_Datos;
            Grid_Listado_Busqueda_Bienes_Inmuebles.DataBind();
        }

        private void Llenar_Listado_Calles() {
            Grid_Listado_Calles.SelectedIndex = (-1);
            Cls_Cat_Pre_Calles_Negocio Calles = new Cls_Cat_Pre_Calles_Negocio();
            Calles.P_Nombre_Calle = Txt_Nombre_Calles_Buscar.Text.Trim().ToUpper();
            DataTable Resultados_Calles = Calles.Consultar_Nombre();
            Resultados_Calles.Columns[Cat_Pre_Calles.Campo_Calle_ID].ColumnName = "CALLE_ID";
            Resultados_Calles.Columns[Cat_Pre_Calles.Campo_Nombre].ColumnName = "NOMBRE_CALLE";
            Grid_Listado_Calles.Columns[1].Visible = true;
            Grid_Listado_Calles.DataSource = Resultados_Calles;
            Grid_Listado_Calles.DataBind();
            Grid_Listado_Calles.Columns[1].Visible = false;
        }

        private void Llenar_Listado_Cuentas_Predial() {
            Grid_Listado_Cuentas_Predial.SelectedIndex = (-1);
            Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
            Cuentas_Predial.P_Cuenta_Predial = Txt_Nombre_Cuenta_Predial_Buscar.Text.Trim().ToUpper();
            Cuentas_Predial.P_Incluir_Campos_Foraneos = true;
            DataTable Resultados_Cuentas_Predial = Cuentas_Predial.Consultar_Cuenta();
            Grid_Listado_Cuentas_Predial.Columns[1].Visible = true;
            Grid_Listado_Cuentas_Predial.DataSource = Resultados_Cuentas_Predial;
            Grid_Listado_Cuentas_Predial.DataBind();
            Grid_Listado_Cuentas_Predial.Columns[1].Visible = false;
        }

        private void Llenar_Listado_Colonias() {
            Grid_Listado_Colonias.SelectedIndex = (-1);
            Cls_Cat_Ate_Colonias_Negocio Colonias_Negocio = new Cls_Cat_Ate_Colonias_Negocio();
            Colonias_Negocio.P_Nombre = Txt_Nombre_Colonia_Buscar.Text.Trim().ToUpper();
            DataTable Resultados_Colonias = Colonias_Negocio.Consulta_Datos().Tables[0];
            Resultados_Colonias.Columns[Cat_Ate_Colonias.Campo_Nombre].ColumnName = "NOMBRE_COLONIA";
            Resultados_Colonias.DefaultView.Sort = "NOMBRE_COLONIA";
            Grid_Listado_Colonias.Columns[1].Visible = true;
            Grid_Listado_Colonias.DataSource = Resultados_Colonias;
            Grid_Listado_Colonias.DataBind();
            Grid_Listado_Colonias.Columns[1].Visible = false;
        }

#endregion

    #region "Grids"

        protected void Grid_Listado_Busqueda_Bienes_Inmuebles_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            Grid_Listado_Busqueda_Bienes_Inmuebles.PageIndex = e.NewPageIndex;
            Llenar_Listado_Busqueda_Bienes_Inmuebles();
        }

        protected void Grid_Listado_Busqueda_Bienes_Inmuebles_SelectedIndexChanged(object sender, EventArgs e) {
            if (Grid_Listado_Busqueda_Bienes_Inmuebles.SelectedIndex > (-1)) {
                String Bien_Inmueble_ID = HttpUtility.HtmlDecode(Grid_Listado_Busqueda_Bienes_Inmuebles.SelectedRow.Cells[1].Text.Trim());
                Session["Operacion_Inicial"] = "VER_BIEN_INMUEBLE";
                Session["Bien_Inmueble_ID"] = Bien_Inmueble_ID;
                Response.Redirect("Frm_Ope_Pat_Bienes_Inmuebles.aspx");
            }
        }

        protected void Grid_Listado_Calles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grid_Listado_Calles.PageIndex = e.NewPageIndex;
            Llenar_Listado_Calles();
        }

        protected void Grid_Listado_Calles_SelectedIndexChanged(object sender, EventArgs e) {
            if (Grid_Listado_Calles.SelectedIndex > (-1)) {
                Hdf_Calle_ID.Value = HttpUtility.HtmlDecode(Grid_Listado_Calles.SelectedRow.Cells[1].Text.Trim());
                Txt_Calle.Text = HttpUtility.HtmlDecode(Grid_Listado_Calles.SelectedRow.Cells[2].Text.Trim());
                Mpe_Calles_Cabecera.Hide();
            }
        }

        protected void Grid_Listado_Cuentas_Predial_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            Grid_Listado_Cuentas_Predial.PageIndex = e.NewPageIndex;
            Llenar_Listado_Cuentas_Predial();
        }

        protected void Grid_Listado_Cuentas_Predial_SelectedIndexChanged(object sender, EventArgs e) {
            if (Grid_Listado_Cuentas_Predial.SelectedIndex > (-1)) {
                Hdf_Cuenta_Predial_ID.Value = HttpUtility.HtmlDecode(Grid_Listado_Cuentas_Predial.SelectedRow.Cells[1].Text.Trim());
                if (Hdf_Cuenta_Predial_ID.Value.Trim().Length > 0) {
                    Cls_Cat_Pre_Cuentas_Predial_Negocio CP_Negocio = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
                    CP_Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value.Trim();
                    CP_Negocio.P_Incluir_Campos_Foraneos = true;
                    DataTable DT_Cuentas_Predial = CP_Negocio.Consultar_Cuenta();
                    if (DT_Cuentas_Predial != null && DT_Cuentas_Predial.Rows.Count > 0) {
                        Txt_Numero_Cuenta_Predial.Text = DT_Cuentas_Predial.Rows[0]["CUENTA_PREDIAL"].ToString().Trim();
                    }
                }
                Mpe_Cuentas_Predial_Cabecera.Hide();
            }
        }

        protected void Grid_Listado_Colonias_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            Grid_Listado_Colonias.PageIndex = e.NewPageIndex;
            Llenar_Listado_Colonias();
        }

        protected void Grid_Listado_Colonias_SelectedIndexChanged(object sender, EventArgs e) {
            if (Grid_Listado_Colonias.SelectedIndex > (-1)) {
                Hdf_Colonia_ID.Value = HttpUtility.HtmlDecode(Grid_Listado_Colonias.SelectedRow.Cells[1].Text.Trim());
                Txt_Colonia.Text = HttpUtility.HtmlDecode(Grid_Listado_Colonias.SelectedRow.Cells[2].Text.Trim());
                Mpe_Colonias.Hide();
            }
        }

    #endregion

    #region "Eventos"

        protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e) {
            Session["Operacion_Inicial"] = "NUEVO";
            Response.Redirect("Frm_Ope_Pat_Bienes_Inmuebles.aspx");
        }
        
        protected void Btn_Salir_Click(object sender, ImageClickEventArgs e) {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }

        protected void Btn_Ejecutar_Busqueda_Calles_Click(object sender, ImageClickEventArgs e) {
            Grid_Listado_Calles.PageIndex = 0;
            Llenar_Listado_Calles();
        }

        protected void Txt_Nombre_Calles_Buscar_TextChanged(object sender, EventArgs e) {
            try {
                Grid_Listado_Calles.PageIndex = 0;
                Llenar_Listado_Calles();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        protected void Btn_Buscar_Calle_Click(object sender, ImageClickEventArgs e) {
            Mpe_Calles_Cabecera.Show();
        }

        protected void Btn_Ejecutar_Busqueda_Cuenta_Predial_Click(object sender, ImageClickEventArgs e) {
            Grid_Listado_Cuentas_Predial.PageIndex = 0;
            Llenar_Listado_Cuentas_Predial();
        }

        protected void Txt_Nombre_Cuenta_Predial_Buscar_TextChanged(object sender, EventArgs e) {
            try {
                Grid_Listado_Cuentas_Predial.PageIndex = 0;
                Llenar_Listado_Cuentas_Predial();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        protected void Btn_Buscar_Numero_Cuenta_Predial_Click(object sender, ImageClickEventArgs e) {
            Mpe_Cuentas_Predial_Cabecera.Show();
        }

        protected void Btn_Ejecutar_Busqueda_Colonia_Click(object sender, ImageClickEventArgs e) {
            Grid_Listado_Colonias.PageIndex = 0;
            Llenar_Listado_Colonias();
        }

        protected void Txt_Nombre_Colonia_Buscar_TextChanged(object sender, EventArgs e) {
            try {
                Grid_Listado_Colonias.PageIndex = 0;
                Llenar_Listado_Colonias();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        protected void Btn_Buscar_Colonia_Click(object sender, ImageClickEventArgs e) {
            Mpe_Colonias.Show();
        }

        protected void Btn_Ejecutar_Busqueda_General_Click(object sender, ImageClickEventArgs e) {
            Grid_Listado_Busqueda_Bienes_Inmuebles.PageIndex = 0;
            Llenar_Listado_Busqueda_Bienes_Inmuebles();
        }


    #endregion

}
