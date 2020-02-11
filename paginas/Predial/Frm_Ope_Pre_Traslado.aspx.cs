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
using Presidencia.Operacion_Predial_Traslado.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using System.IO;

public partial class paginas_predial_Frm_Ope_Pre_Traslado : System.Web.UI.Page {

    #region Page_Load

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 09/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        protected void Page_Load(object sender, EventArgs e){
            if (!IsPostBack) {
                Llenar_Combo_Notarios();
                Llenar_Grid_Contrarecibos(0);
                Tab_Contenedor_Pestagnas.ActiveTabIndex = 0;
            }
            Div_Contenedor_Msj_Error.Visible = false;
        }

    #endregion

    #region Metodos
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Notarios
        ///DESCRIPCIÓN: Llena el Combo de Notarios
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Combo_Notarios() {
            try{
                Cls_Ope_Pre_Traslado_Negocio Traslado_Dominio = new Cls_Ope_Pre_Traslado_Negocio();
                Traslado_Dominio.P_Tipo_DataTable = "LISTAR_NOTARIOS";
                DataTable Notarios = Traslado_Dominio.Consultar_DataTable();
                DataRow Fila_Notario = Notarios.NewRow();
                Fila_Notario["NOTARIO_ID"] = HttpUtility.HtmlDecode("00000");
                Fila_Notario["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
                Notarios.Rows.InsertAt(Fila_Notario, 0);

                Cmb_Estatus.Items.Clear();
                Cmb_Estatus.Items.Add(new ListItem("<SELECCIONE>", "0"));                
                Cmb_Estatus.Items.Add(new ListItem("GENERADO", "GENERADO"));
                Cmb_Estatus.Items.Add(new ListItem("RECHAZADO", "RECHAZADO"));

                //SE QUITO PESTAÑA POR ESO SE ELIMINA COMBO
                //Cmb_Notarios.DataSource = Notarios;
                //Cmb_Notarios.DataValueField = "NOTARIO_ID";
                //Cmb_Notarios.DataTextField = "NOMBRE";
                //Cmb_Notarios.DataBind();
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }                    

        #region Grid
    
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Contrarecibos
            ///DESCRIPCIÓN: Llena el Grid de Contrarecibos
            ///PARAMETROS:     
            ///             1. Pagina.  Pagina en la cual se mostrará el Grid_View
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 10/Noviembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Grid_Contrarecibos(Int32 Pagina) {
                try{
                        Cls_Ope_Pre_Traslado_Negocio Traslado_Dominio = new Cls_Ope_Pre_Traslado_Negocio();
                        Traslado_Dominio.P_Tipo_DataTable = "LISTAR_CONTRARECIBOS";
                        Traslado_Dominio.P_Traslado = "SI";
                        Traslado_Dominio.P_Buscar_Estatus = true;
                        Traslado_Dominio.P_Estatus = "GENERADO";
                        if (Session["Tipo_Busqueda"] != null) {
                            String Tipo_Busqueda = Session["Tipo_Busqueda"].ToString();
                            if (Tipo_Busqueda.Trim().Equals("CONTRARECIBOS")) {
                                if (!String.IsNullOrEmpty(Txt_Cuenta_Predial.Text.Trim()))
                                {
                                    Traslado_Dominio.P_Cuenta_Predial_ID = Txt_Cuenta_Predial.Text.Trim();
                                    Traslado_Dominio.P_Buscar_Estatus = false;
                                }
                                if (!String.IsNullOrEmpty(Txt_No_Contrarecibo.Text.Trim()))
                                {
                                    Traslado_Dominio.P_Buscar_Estatus = false;
                                    Int32 Contrarecibo = 0;
                                    Int32.TryParse(Txt_No_Contrarecibo.Text.Trim(), out Contrarecibo);
                                    Traslado_Dominio.P_No_Contrarecibo = String.Format("{0:0000000000}", Contrarecibo);
                                }
                                
                                if (Txt_Fecha_Escritura.Text.Trim().Length > 0) {
                                    Traslado_Dominio.P_Buscar_Estatus = false;
                                    Traslado_Dominio.P_Buscar_Fecha_Escritura = true;
                                    Traslado_Dominio.P_Fecha_Escritura = Convert.ToDateTime(Txt_Fecha_Escritura.Text.Trim());
                                }
                                if (Txt_No_Escritura.Text.Trim().Length > 0) {
                                    Traslado_Dominio.P_Buscar_Estatus = false;
                                    Traslado_Dominio.P_Buscar_No_Escritura = true;
                                    Traslado_Dominio.P_No_Escritura = Txt_No_Escritura.Text.Trim();
                                }
                                if (Cmb_Estatus.SelectedIndex > 0)
                                {
                                    Traslado_Dominio.P_Buscar_Estatus = true;
                                    Traslado_Dominio.P_Estatus = Cmb_Estatus.SelectedValue.ToString();
                                }

                                //SE ELIMINO POR HABER QUITADO LA PESTAÑE DE LISTADO
                            //} else if (Tipo_Busqueda.Trim().Equals("LISTADOS")) {
                            //    Traslado_Dominio.P_Listadoque_ID = Txt_No_Listado.Text.Trim();
                            //    if (Txt_Fecha_Generacion.Text.Trim().Length > 0) {
                            //        Traslado_Dominio.P_Buscar_Fecha_Generacion = true;
                            //        Traslado_Dominio.P_Fecha_Generacion = Convert.ToDateTime(Txt_Fecha_Generacion.Text.Trim());
                            //    }
                            //    if (Cmb_Notarios.SelectedIndex > 0) {
                            //        Traslado_Dominio.P_Notario_ID = Cmb_Notarios.SelectedItem.Value;
                            //    }
                            }
                        }
                        Traslado_Dominio.P_Contrarecibos_Sin_Calculo = true;
                        Grid_Contrarecibos.Columns[2].Visible = true;
                        Grid_Contrarecibos.DataSource = Traslado_Dominio.Consultar_DataTable();
                        Grid_Contrarecibos.PageIndex = Pagina;
                        Grid_Contrarecibos.DataBind();
                        Grid_Contrarecibos.Columns[2].Visible = false;
                }catch(Exception Ex){
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }            

        #endregion

    #endregion

    #region Grids
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Limpiar_Filtros_Buscar_Contrarecibos_Click
        ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Limpieza de los filtros
        ///             para la busqueda por parte de los Contrarecibos.
        ///PARAMETROS:     
        ///CREO: Jacqueline Ramírez Sierra
        ///FECHA_CREO: 26/Agosto/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************             
            protected void Grid_Contrarecibos_PageIndexChanging(object sender, GridViewPageEventArgs e)
            {
                Grid_Contrarecibos.SelectedIndex = (-1);
                Llenar_Grid_Contrarecibos(e.NewPageIndex);
            }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Contrarecibos_RowDataBound
        ///DESCRIPCIÓN: Evento de RowDataBound del Grid de Contrarecibos
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Contrarecibos_RowDataBound(object sender, GridViewRowEventArgs e) {
            try{
                if (e.Row.RowType == DataControlRowType.DataRow) {
                    if (e.Row.Cells[3].Text == null || e.Row.Cells[3].Text.Trim().Equals("") || e.Row.Cells[3].Text.Trim().Equals("SIN REGISTRO")) {
                        TextBox Caja_Texto_Cuenta = (TextBox)e.Row.Cells[9].FindControl("Txt_Establecer_Cuenta_Predial");
                        Caja_Texto_Cuenta.Visible = true;
                        ImageButton Boton_Cuenta = (ImageButton)e.Row.Cells[9].FindControl("Btn_Establecer_Cuenta_Predial");
                        Boton_Cuenta.Visible = true;
                        Boton_Cuenta.CommandArgument = e.Row.Cells[1].Text.Trim();
                        e.Row.Cells[0].Enabled = false;
                    } else {
                        TextBox Caja_Texto_Cuenta = (TextBox)e.Row.Cells[9].FindControl("Txt_Establecer_Cuenta_Predial");
                        Caja_Texto_Cuenta.Visible = false;
                        ImageButton Boton_Cuenta = (ImageButton)e.Row.Cells[9].FindControl("Btn_Establecer_Cuenta_Predial");
                        Boton_Cuenta.Visible = false;
                        e.Row.Cells[0].Enabled = true;
                    }
                    if (e.Row.Cells[8].Text.Trim().Equals("TRASLADO")) {
                        TextBox Caja_Texto_Cuenta = (TextBox)e.Row.Cells[9].FindControl("Txt_Establecer_Cuenta_Predial");
                        Caja_Texto_Cuenta.Visible = false;
                        ImageButton Boton_Cuenta = (ImageButton)e.Row.Cells[9].FindControl("Btn_Establecer_Cuenta_Predial");
                        Boton_Cuenta.Visible = false;
                        e.Row.Cells[0].Enabled = false;
                    }
                    if (e.Row.Cells[8].Text == "POR VALIDAR" )
                    {                        
                        e.Row.Cells[0].Enabled = false;
                    }
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

    #endregion

    #region Eventos
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Limpiar_Filtros_Buscar_Contrarecibos_Click
        ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Limpieza de los filtros
        ///             para la busqueda por parte de los Contrarecibos.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        protected void Btn_Limpiar_Filtros_Buscar_Contrarecibos_Click(object sender, ImageClickEventArgs e) {
            Txt_Cuenta_Predial.Text = "";
            Txt_No_Contrarecibo.Text = "";
            Txt_Fecha_Escritura.Text = "";
            Txt_No_Escritura.Text = "";
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Contrarecibos_Click
        ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Busqueda de los
        ///             Contrarecibos
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        protected void Btn_Buscar_Contrarecibos_Click(object sender, ImageClickEventArgs e) {
            Session["Tipo_Busqueda"] = "CONTRARECIBOS";
            Llenar_Grid_Contrarecibos(0);
            if (Grid_Contrarecibos.Rows.Count == 0 && (Txt_Cuenta_Predial.Text.Trim().Length > 0 || Txt_No_Contrarecibo.Text.Trim().Length > 0 || Txt_Fecha_Escritura.Text.Trim().Length > 0 || Txt_No_Escritura.Text.Trim().Length > 0)) {
                Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con los filtros establecidos no se encotrarón coincidencias";
                Lbl_Mensaje_Error.Text = "(Se cargaron todos los contrarecibos almacenados y se limpiaron los filtros)";
                Div_Contenedor_Msj_Error.Visible = true;
                Txt_Cuenta_Predial.Text = "";
                Txt_No_Contrarecibo.Text = "";
                Txt_Fecha_Escritura.Text = "";
                Txt_No_Escritura.Text = "";
                Cmb_Estatus.SelectedIndex = 0;
                Session.Remove("Tipo_Busqueda");
                Llenar_Grid_Contrarecibos(0);                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Establecer_Cuenta_Predial_Click
        ///DESCRIPCIÓN: Maneja el Evento del Boton para establecer la cuenta de Predial
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        protected void Btn_Establecer_Cuenta_Predial_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                String No_Contrarecibo = ((ImageButton)sender).CommandArgument.Trim();
                if (Grid_Contrarecibos.Rows.Count > 0 && No_Contrarecibo.Trim().Length > 0)
                {
                    for (Int32 Contador = 0; Contador < Grid_Contrarecibos.Rows.Count; Contador++)
                    {
                        if (Grid_Contrarecibos.Rows[Contador].Cells[1].Text.Trim().Equals(No_Contrarecibo))
                        {
                            TextBox Text_Temporal = (TextBox)Grid_Contrarecibos.Rows[Contador].Cells[9].FindControl("Txt_Establecer_Cuenta_Predial");
                            if (Text_Temporal.Text.Trim().Length == 12)
                            {                                
                                //String Cuenta_Predial_ID = Obtener_Cuenta_Predial_ID(Text_Temporal.Text.Trim());
                                if (!Validar_Cuenta_Existente(Text_Temporal.Text.Trim()))
                                {
                                    String Cuenta_Predial_ID = Crear_Cuenta_Predial_ID(Text_Temporal.Text.Trim());
                                    if (Cuenta_Predial_ID != "")
                                    {
                                        if (Cuenta_Predial_ID.Trim().Length > 0)
                                        {
                                            Cls_Ope_Pre_Traslado_Negocio Traslado_Dominio = new Cls_Ope_Pre_Traslado_Negocio();
                                            Traslado_Dominio.P_No_Contrarecibo = No_Contrarecibo;
                                            Traslado_Dominio.P_Cuenta_Predial_ID = Cuenta_Predial_ID;
                                            Traslado_Dominio.P_Estatus = "PENDIENTE";
                                            Traslado_Dominio.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                                            Traslado_Dominio.Modificar_Contrarecibo();
                                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Operacion_Predial_Traslado", "alert('Cuenta Registrada Exitosamente!')", true);
                                            Llenar_Grid_Contrarecibos(Grid_Contrarecibos.PageIndex);
                                        }
                                        else
                                        {
                                            Lbl_Ecabezado_Mensaje.Text = "Para la Actualización de la cuenta del Contrarecibo '" + No_Contrarecibo + "' es necesario Introducir la Cuenta";
                                            Lbl_Mensaje_Error.Text = "";
                                            Div_Contenedor_Msj_Error.Visible = true;
                                        }
                                    }
                                    else
                                    {
                                        Lbl_Ecabezado_Mensaje.Text = "La Cuenta Predial no pudo ser creada";
                                        //Lbl_Ecabezado_Mensaje.Text = "La Cuenta Predial indicada no existe";
                                        Lbl_Mensaje_Error.Text = "";
                                        Div_Contenedor_Msj_Error.Visible = true;
                                    }
                                }
                                else 
                                {
                                    Lbl_Ecabezado_Mensaje.Text = "La Cuenta Predial ya existe, introduzca otro número de Cuenta Predial";
                                    //Lbl_Ecabezado_Mensaje.Text = "La Cuenta Predial indicada no existe";
                                    Lbl_Mensaje_Error.Text = "";
                                    Div_Contenedor_Msj_Error.Visible = true;
                                }
                                break;
                        }//Validar 12 caracteres de Cuenta
                            else
                            {
                                Lbl_Ecabezado_Mensaje.Text = "Favor de Ingresar Correctamente la Cuenta predial ";
                                Lbl_Mensaje_Error.Text = "";
                                Div_Contenedor_Msj_Error.Visible = true;
                            }
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
        ///NOMBRE DE LA FUNCIÓN : Obtener_Cuenta_Predial_ID
        ///DESCRIPCIÓN          : En base al número de cuenta, obtiene el Id del catálogo
        ///PARAMETROS           : Cuenta_Predial, String con el valor del No. de Cuenta
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 10/Diciembre/2010
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        private String Obtener_Cuenta_Predial_ID(String Cuenta_Predial)
        {
            Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
            DataTable Dt_Cuentas_Predial;
            String Cuenta_Predial_ID = "";

            Cuentas_Predial.P_Campos_Dinamicos = Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
            Cuentas_Predial.P_Filtros_Dinamicos = Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " = '" + Cuenta_Predial + "'";
            Dt_Cuentas_Predial = Cuentas_Predial.Consultar_Cuenta();
            if (Dt_Cuentas_Predial.Rows.Count > 0)
            {
                Cuenta_Predial_ID = Convert.ToString(Dt_Cuentas_Predial.Rows[0][0]);
            }
            return Cuenta_Predial_ID;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Crear_Cuenta_Predial_ID
        ///DESCRIPCIÓN          : Se crea la cuenta en la tabla Cat_Pre_Cuentas_Predial
        ///PARAMETROS           : Cuenta_Predial, String con el valor del No. de Cuenta
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 15/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        private String Crear_Cuenta_Predial_ID(String Cuenta_Predial)
        {
            Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
            String Cuenta_Predial_ID = "";

            Cuentas_Predial.P_Cuenta_Predial = Cuenta_Predial;
            Cuentas_Predial.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
            Cuentas_Predial.P_Estatus = "PENDIENTE";
            if (Cuentas_Predial.Alta_Cuenta())
            {
                DataTable Dt_Cuentas_Predial;
                Cuentas_Predial.P_Campos_Dinamicos = Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                Cuentas_Predial.P_Cuenta_Predial = Cuenta_Predial.ToUpper();
                Dt_Cuentas_Predial = Cuentas_Predial.Consultar_Cuenta();
                if (Dt_Cuentas_Predial != null)
                {
                    if (Dt_Cuentas_Predial.Rows.Count > 0)
                    {
                        Cuenta_Predial_ID = Dt_Cuentas_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
                    }
                }
            }
            return Cuenta_Predial_ID;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Validar_Cuenta_Existente
        ///DESCRIPCIÓN          : Se crea la cuenta en la tabla Cat_Pre_Cuentas_Predial
        ///PARAMETROS           : Cuenta_Predial, String con el valor del No. de Cuenta
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 15/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        private Boolean Validar_Cuenta_Existente(String Cuenta_Predial)
        {
            Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
            String Cuenta_Predial_ID = "";
            Boolean Existe = false;

            Cuentas_Predial.P_Cuenta_Predial = Cuenta_Predial;
            Existe=Cuentas_Predial.Consultar_Cuenta_Existente();
            return Existe;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Listado_Click
        ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Busqueda de los
        ///             Listados
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        protected void Btn_Buscar_Listado_Click(object sender, ImageClickEventArgs e) {
            Session["Tipo_Busqueda"] = "LISTADOS";
            Llenar_Grid_Contrarecibos(0);
            //if (Grid_Contrarecibos.Rows.Count == 0 && (Txt_No_Listado.Text.Trim().Length > 0 || Txt_Fecha_Generacion.Text.Trim().Length > 0 || Cmb_Notarios.SelectedIndex > 0)) {
            //    Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con los filtros establecidos no se encontraron coincidencias";
            //    Lbl_Mensaje_Error.Text = "(Se cargaron todos los contrarecibos almacenados y se limpiaron los filtros)";
            //    Div_Contenedor_Msj_Error.Visible = true;
            //    Txt_No_Listado.Text = "";
            //    Txt_Fecha_Generacion.Text = "";
            //    Cmb_Notarios.SelectedIndex = 0;
            //    Session.Remove("Tipo_Busqueda");
            //    Llenar_Grid_Contrarecibos(0);
            //}
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Limpiar_Filtros_Buscar_Listado_Click
        ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Limpieza de los filtros
        ///             para la busqueda por parte de los Listados.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 09/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        //protected void Btn_Limpiar_Filtros_Buscar_Listado_Click(object sender, ImageClickEventArgs e) {
        //    Txt_No_Listado.Text = "";
        //    Txt_Fecha_Generacion.Text = "";
        //    Cmb_Notarios.SelectedIndex = 0;
        //}

    #endregion

            protected void Grid_Contrarecibos_SelectedIndexChanged(object sender, EventArgs e)
        {            
            Session["Contrarecibo_Traslado"] = Grid_Contrarecibos.Rows[Grid_Contrarecibos.SelectedIndex].Cells[1].Text;
            Session["Estatus_Traslado"] = Grid_Contrarecibos.Rows[Grid_Contrarecibos.SelectedIndex].Cells[8].Text;
            if ( !String.IsNullOrEmpty( Session["Contrarecibo_Traslado"].ToString() ) )
            Response.Redirect("../Predial/Frm_Ope_Pre_Orden_Variacion.aspx");
        }
}