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
using Presidencia.Control_Patrimonial_Actualizacion_Licencias.Negocio;
using Presidencia.Empleados.Negocios;
using Presidencia.Constantes;
using Presidencia.Dependencias.Negocios;

public partial class paginas_Compras_Frm_Ope_Pat_Actualizacion_Licencias : System.Web.UI.Page {
 
    #region "Page Load"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Evento que se Ejecuta al Cargar la Pagina
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 14/Julio/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Page_Load(object sender, EventArgs e) {
            try {
                Div_Contenedor_Msj_Error.Visible = false;
                if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Trim().Length == 0) {
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
                }
                if (!IsPostBack) {
                    Llenar_Combo_Dependencias();
                    Configuracion_Formulario(false);
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    
    #endregion "Page Load"

    #region "Metodos"

        #region "Modal Busqueda"

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Limpiar_Filtros_Busqueda
            ///DESCRIPCIÓN: Llena el Grid de listado de los Empleados.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 14/Julio/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Combo_Dependencias() {
                try {
                    Cmb_Busqueda_Dependencia.Items.Clear();
                    Cls_Ope_Pat_Actualizacion_Licencias_Negocio Negocio = new Cls_Ope_Pat_Actualizacion_Licencias_Negocio();
                    Cmb_Busqueda_Dependencia.DataSource = Negocio.Consultar_Dependecias();
                    Cmb_Busqueda_Dependencia.DataValueField = "DEPENDENCIA_ID";
                    Cmb_Busqueda_Dependencia.DataTextField = "NOMBRE";
                    Cmb_Busqueda_Dependencia.DataBind();
                    Cmb_Busqueda_Dependencia.Items.Insert(0, new ListItem("< TODAS >", ""));
                } catch(Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Limpiar_Filtros_Busqueda
            ///DESCRIPCIÓN: Llena el Grid de listado de los Empleados.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 14/Julio/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Grid_Listado_Empleados() {
                try {
                    Grid_Listado_Empleados.SelectedIndex = (-1);
                    Grid_Listado_Empleados.Columns[1].Visible = true;
                    Cls_Ope_Pat_Actualizacion_Licencias_Negocio Negocio = new Cls_Ope_Pat_Actualizacion_Licencias_Negocio();
                    if (Txt_Busqueda_No_Empleado.Text.Trim().Length > 0) { Negocio.P_Busqueda_No_Empleado = Txt_Busqueda_No_Empleado.Text.Trim(); }
                    if (Txt_Busqueda_RFC.Text.Trim().Length > 0) { Negocio.P_Busqueda_RFC = Txt_Busqueda_RFC.Text.Trim(); }
                    if (Txt_Busqueda_Nombre_Empleado.Text.Trim().Length > 0) { Negocio.P_Busqueda_Empleado_Nombre = Txt_Busqueda_Nombre_Empleado.Text.Trim(); }
                    if (Cmb_Busqueda_Dependencia.SelectedIndex > 0) { Negocio.P_Busqueda_Dependencia_ID = Cmb_Busqueda_Dependencia.SelectedItem.Value; }
                    Grid_Listado_Empleados.DataSource = Negocio.Consultar_Empleados();
                    Grid_Listado_Empleados.DataBind();
                    Grid_Listado_Empleados.Columns[1].Visible = false;
                } catch(Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

        #endregion "Modal Busqueda"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Campos_Generales
        ///DESCRIPCIÓN: Limpia los campos del Formulario..
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 14/Julio/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Campos_Generales() {
            Hdf_Empleado_ID.Value = "";
            Txt_Dependencia_Empleado.Text = "";
            Txt_RFC_Empleado.Text = "";
            Txt_No_Empledo.Text = "";
            Txt_Nombre_Empleado.Text = "";
            Txt_No_Licencia.Text = "";
            Txt_Fecha_Vencimiento_Licencia.Text = "";
            Txt_Tipo_Licencia.Text = "";
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
        ///DESCRIPCIÓN: Habilita o Inhabilita los campos.
        ///PROPIEDADES: Estatus. Estatus de los campos.    
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 14/Julio/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Configuracion_Formulario(Boolean Estatus) {
            if (Estatus) {
                Btn_Modificar.ToolTip = "Actualizar";
                Btn_Modificar.AlternateText = "Actualizar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Limpiar_Campos.Visible = false;
                Div_Busqueda.Visible = false;
            } else {
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Modificar.AlternateText = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Salir.ToolTip = "Salir";
                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Limpiar_Campos.Visible = true;
                Div_Busqueda.Visible = true;
            }
            Txt_No_Licencia.Enabled = Estatus;
            Btn_Fecha_Vencimiento_Licencia.Visible = Estatus;
            Txt_Tipo_Licencia.Enabled = Estatus;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Mostrar_Detalles_Empleado
        ///DESCRIPCIÓN: Muestra los Datos del Empleado.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 14/Julio/2011
        ///MODIFICO:  Francisco Antonio Gallardo Castañeda.
        ///FECHA_MODIFICO: 14/Julio/2011
        ///CAUSA_MODIFICACIÓN: Se agrego el tipo de Licencia
        ///*******************************************************************************
        private void Mostrar_Detalles_Empleado() {
            if (Hdf_Empleado_ID.Value != null && Hdf_Empleado_ID.Value.Trim().Length > 0) {
                Cls_Cat_Empleados_Negocios Empleado_Negocio = new Cls_Cat_Empleados_Negocios();
                Empleado_Negocio.P_Empleado_ID = Hdf_Empleado_ID.Value.Trim();
                DataTable Dt_Datos_Empleado = Empleado_Negocio.Consulta_Datos_Empleado();
                Limpiar_Campos_Generales();
                if (Dt_Datos_Empleado.Rows.Count > 0) {
                    Hdf_Empleado_ID.Value = (Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Empleado_ID] != null) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString() : "";
                    Cls_Cat_Dependencias_Negocio Dependencia_Negocio = new Cls_Cat_Dependencias_Negocio();
                    Dependencia_Negocio.P_Dependencia_ID = (Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Dependencia_ID] != null) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Dependencia_ID].ToString() : "";
                    DataTable Dt_Datos_Dependencia = Dependencia_Negocio.Consulta_Dependencias();
                    if (Dt_Datos_Dependencia.Rows.Count > 0) {
                        Txt_Dependencia_Empleado.Text += (Dt_Datos_Dependencia.Rows[0][Cat_Dependencias.Campo_Clave] != null) ? Dt_Datos_Dependencia.Rows[0][Cat_Dependencias.Campo_Clave].ToString() : "";
                        if (Txt_Dependencia_Empleado.Text.Trim().Length > 0) { Txt_Dependencia_Empleado.Text += " - "; }
                        Txt_Dependencia_Empleado.Text += (Dt_Datos_Dependencia.Rows[0][Cat_Dependencias.Campo_Nombre] != null) ? Dt_Datos_Dependencia.Rows[0][Cat_Dependencias.Campo_Nombre].ToString() : "";
                    }
                    Txt_RFC_Empleado.Text = (Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_RFC] != null) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_RFC].ToString() : "";
                    Txt_No_Empledo.Text = (Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_No_Empleado] != null) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_No_Empleado].ToString() : "";
                    String Nombre = (Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Paterno] != null) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Paterno].ToString() : "";
                    Nombre = Nombre + ((Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Materno] != null) ? (" " + Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Materno].ToString()) : "");
                    Nombre = Nombre + ((Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Nombre] != null) ? (" " + Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Nombre].ToString()) : "");
                    Txt_Nombre_Empleado.Text = Nombre;
                    Txt_No_Licencia.Text = (Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_No_Licencia_Manejo] != null) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_No_Licencia_Manejo].ToString() : "";
                    if (Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Fecha_Vencimiento_Licencia] != null && Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Fecha_Vencimiento_Licencia].ToString().Trim().Length > 0) {
                        DateTime Fecha_Vencimiento = Convert.ToDateTime(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Fecha_Vencimiento_Licencia]);
                        Txt_Fecha_Vencimiento_Licencia.Text = Fecha_Vencimiento.ToString("dd/MMM/yyyy");
                    }
                    if (Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Tipo_Licencia] != null && Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Tipo_Licencia].ToString().Trim().Length > 0) {
                        Txt_Tipo_Licencia.Text = Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Tipo_Licencia].ToString().Trim();
                    }
                } else {
                    Lbl_Ecabezado_Mensaje.Text = "No se pudierón cargar correctamente los datos del Empleados.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Mostrar_Detalles_Empleado
        ///DESCRIPCIÓN: Muestra los Datos del Empleado.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 14/Julio/2011
        ///MODIFICO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_MODIFICO: 22/Noviembre/2011
        ///CAUSA_MODIFICACIÓN: Se agrego el Tipo de Licencia
        ///*******************************************************************************
        private Boolean Validar_Datos() {
            Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
            String Mensaje_Error = "";
            Boolean Validacion = true;
            if (Txt_No_Licencia.Text.Trim().Length == 0) {
                Mensaje_Error = Mensaje_Error + "+ Introducir el No. de Licencia. <br />";
                Validacion = false;
            }
            if (Txt_Fecha_Vencimiento_Licencia.Text.Trim().Length == 0) {
                Mensaje_Error = Mensaje_Error + "+ Seleccionar la Fecha. <br />";
                Validacion = false;
            }
            if (Txt_Tipo_Licencia.Text.Trim().Length == 0) {
                Mensaje_Error = Mensaje_Error + "+ Introducir el Tipo de Licencia. <br />";
                Validacion = false;
            }
            if (!Validacion) {
                Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
                Div_Contenedor_Msj_Error.Visible = true;
            }
            return Validacion;
        }

    #endregion "Metodos"

    #region "Grids"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Empleados_PageIndexChanged
        ///DESCRIPCIÓN: Maneja el Evento de Cambio de Página en el Grid de Empleados.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 14/Julio/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Listado_Empleados_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                Grid_Listado_Empleados.PageIndex = e.NewPageIndex;
                Llenar_Grid_Listado_Empleados();
                MPE_Busqueda_Empleados.Show();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Empleados_SelectedIndexChanged
        ///DESCRIPCIÓN: Maneja el Evento de Cambio de Selección en el Grid de Empleados.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 14/Julio/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Listado_Empleados_SelectedIndexChanged(object sender, EventArgs e) { 
            try {
                if (Grid_Listado_Empleados.SelectedIndex > (-1)) {
                    Hdf_Empleado_ID.Value = Grid_Listado_Empleados.SelectedRow.Cells[1].Text.Trim();
                    Mostrar_Detalles_Empleado();
                    MPE_Busqueda_Empleados.Hide();
                }
                Grid_Listado_Empleados.SelectedIndex = (-1);
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true; 
            }            
        }

    #endregion "Grids"

    #region "Eventos"

        #region "Modal Busqueda"

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Avanzada_Click
            ///DESCRIPCIÓN: Carga el Modal Popup de Busqueda de Empleados.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 14/Julio/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Btn_Busqueda_Avanzada_Click(object sender, EventArgs e) {
                 try {
                    MPE_Busqueda_Empleados.Show();
                }catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Empleados_Click
            ///DESCRIPCIÓN: Ejecuta la Busqueda Avanzada para el Resguardante.
            ///PARAMETROS:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 24/Octubre/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************    
            protected void Btn_Busqueda_Empleados_Click(object sender, EventArgs e) {
                try {
                    Grid_Listado_Empleados.PageIndex = 0;
                    Llenar_Grid_Listado_Empleados();
                    MPE_Busqueda_Empleados.Show();
                }  catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

        #endregion "Modal Busqueda"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
        ///DESCRIPCIÓN: Cancela la operación que esta en proceso (Alta o Actualizar) o Sale
        ///             del Formulario.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 29/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Salir_Click(object sender, ImageClickEventArgs e) {
            if (Btn_Salir.AlternateText.Equals("Salir")) {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            } else {
                Mostrar_Detalles_Empleado();
                Configuracion_Formulario(false);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
        ///DESCRIPCIÓN: Hace una Actualización de Licencia.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 14/Julio/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e) {
            if (Btn_Modificar.AlternateText.Equals("Modificar")) {
                if (Hdf_Empleado_ID.Value != null && Hdf_Empleado_ID.Value.Trim().Length > 0) {
                    Configuracion_Formulario(true);
                } else {
                    Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Empleado a Actualizar.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            } else {
                if (Validar_Datos()) {
                    Cls_Ope_Pat_Actualizacion_Licencias_Negocio AL_Negocio = new Cls_Ope_Pat_Actualizacion_Licencias_Negocio();
                    AL_Negocio.P_Busqueda_Empleado_ID = Hdf_Empleado_ID.Value;
                    AL_Negocio.P_No_Licencia = Txt_No_Licencia.Text.Trim();
                    AL_Negocio.P_Fecha_Vencimiento = Convert.ToDateTime(Txt_Fecha_Vencimiento_Licencia.Text.Trim());
                    AL_Negocio.P_Tipo_Licencia = Txt_Tipo_Licencia.Text.Trim();
                    AL_Negocio.P_Usuario_Nombre = Cls_Sessiones.Nombre_Empleado;
                    AL_Negocio.Modificar_Datos_Licencia();
                    Mostrar_Detalles_Empleado();
                    Configuracion_Formulario(false);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Actualizaciónn de Licencias", "alert('Se ha Actualizado Exitosamente los datos de la Licencia al Empleado.');", true);
                }
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Limpiar_Campos_Click
        ///DESCRIPCIÓN: Limpia los campos del Formulario.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 07/Septiembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Limpiar_Campos_Click(object sender, ImageClickEventArgs e) {
            Limpiar_Campos_Generales();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Directa_Click
        ///DESCRIPCIÓN: Ejecuta una Busqueda directa.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 22/Noviembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Busqueda_Directa_Click(object sender, ImageClickEventArgs e) {
            try {
                Limpiar_Campos_Generales();
                if (Txt_Busqueda_Directa.Text.Trim().Length > 0) {
                    Cls_Ope_Pat_Actualizacion_Licencias_Negocio AL_Negocio = new Cls_Ope_Pat_Actualizacion_Licencias_Negocio();
                    AL_Negocio.P_Busqueda_No_Empleado = Txt_Busqueda_Directa.Text.Trim();
                    DataTable Dt_Empleados = AL_Negocio.Consultar_Empleados();
                    if (Dt_Empleados.Rows.Count > 0) {
                        Hdf_Empleado_ID.Value = Dt_Empleados.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString().Trim();
                        Mostrar_Detalles_Empleado();
                    } else {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "GACO", "alert('El Empleado no se encontró.');", true); 
                    }
                } else {
                    Lbl_Ecabezado_Mensaje.Text = "Verificar.";
                    Lbl_Mensaje_Error.Text = "'Para Hacer una Búsqueda es Necesario Introducir el No. de Empleado'";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Verificar.";
                Lbl_Mensaje_Error.Text = "Exception ['" + Ex.Message + "']";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

    #endregion "Eventos"

}
