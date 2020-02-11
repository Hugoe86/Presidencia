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
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Calendarizar_Presupuesto.Negocio;

public partial class paginas_Presupuestos_Frm_Ope_Psp_Autorizar_Calendario_Presupuesto : System.Web.UI.Page
{
    #region PAGE LOAD

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!IsPostBack)
            {
                Autorizar_Presupuesto_Inicio();
            }
        }

    #endregion

    #region METODOS

    #region (Metodos Generales)
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Autorizar_Presupuesto_Inicio
        ///DESCRIPCIÓN          : Metodo para el inicio de la pagina
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 22/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        private void Autorizar_Presupuesto_Inicio()
        {
            try
            {
                Mostrar_Ocultar_Error(false);
                Limpiar_Formulario("Todo");
                Estado_Botones("inicial");
                Habilitar_Forma(false);
                Llenar_Grid_Dependencias_Presupuestadas();
                Div_Dependencias_Presupuestadas.Style.Add("display", "block");
                Div_Partidas_Asignadas.Style.Add("display", "none");
                Grid_Dependencias_Presupuestadas.SelectedIndex = -1;
                Grid_Partida_Asignada.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Calendarizar_Presupuesto_Inicio ERROR[" + ex.Message + "]");
            }
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Habilitar_Forma
        ///DESCRIPCIÓN          : Metodo para habilitar o deshabilitar los controles de la pagina
        ///PROPIEDADES          1 Estatus true o false para habilitar los controles
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 10/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        private void Habilitar_Forma(Boolean Estatus)
        {
            try
            {
                Txt_Unidad_Responsable.ReadOnly = true ;
                Txt_Limite_Presupuestal.ReadOnly = true;
                Txt_Programa.ReadOnly = true;
                Cmb_Capitulos.Enabled = false;
                Cmb_Partida_Especifica.Enabled = false;
                Txt_Justificacion.ReadOnly = true;
                Txt_Enero.ReadOnly = true;
                Txt_Febrero.ReadOnly = true;
                Txt_Marzo.ReadOnly = true;
                Txt_Abril.ReadOnly = true;
                Txt_Mayo.ReadOnly = true;
                Txt_Junio.ReadOnly = true;
                Txt_Julio.ReadOnly = true;
                Txt_Agosto.ReadOnly = true;
                Txt_Septiembre.ReadOnly = true;
                Txt_Octubre.ReadOnly = true;
                Txt_Noviembre.ReadOnly = true;
                Txt_Diciembre.ReadOnly = true;
                Cmb_Producto.Enabled = false;
                Cmb_Estatus.Enabled = Estatus;
                Cmb_Producto.Enabled = false;
                Txt_Comentarios.Enabled = Estatus;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Habilitar_Forma ERROR[" + ex.Message + "]");
            }
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Limpiar_Formulario
        ///DESCRIPCIÓN          : Metodo para limpiar los controles de la pagina
        ///PROPIEDADES          1 Accion para saber que vamos a limpiar
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 10/Noviembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        private void Limpiar_Formulario(String Accion)
        {
            try
            {
                switch(Accion)
                { 
                    case "Datos_Producto":
                        Txt_Enero.Text = "";
                        Txt_Febrero.Text = "";
                        Txt_Marzo.Text = "";
                        Txt_Abril.Text = "";
                        Txt_Mayo.Text = "";
                        Txt_Junio.Text = "";
                        Txt_Julio.Text = "";
                        Txt_Agosto.Text = "";
                        Txt_Septiembre.Text = "";
                        Txt_Octubre.Text = "";
                        Txt_Noviembre.Text = "";
                        Txt_Diciembre.Text = "";
                        Txt_Total.Text = "";
                        Txt_Justificacion.Text = "";
                        Hf_Producto_ID.Value = "";
                        Hf_Precio.Value = "";
                        Lbl_Cantidad.Text = "";
                        Lbl_Txt_Enero.Text = "";
                        Lbl_Txt_Febrero.Text = "";
                        Lbl_Txt_Marzo.Text = "";
                        Lbl_Txt_Abril.Text = "";
                        Lbl_Txt_Mayo.Text = "";
                        Lbl_Txt_Junio.Text = "";
                        Lbl_Txt_Julio.Text = "";
                        Lbl_Txt_Agosto.Text = "";
                        Lbl_Txt_Septiembre.Text = "";
                        Lbl_Txt_Octubre.Text = "";
                        Lbl_Txt_Noviembre.Text = "";
                        Lbl_Txt_Diciembre.Text = "";
                        Cmb_Capitulos.SelectedIndex = -1;
                        Cmb_Partida_Especifica.SelectedIndex = -1;
                        Cmb_Producto.SelectedIndex = -1;
                        Cmb_Stock.SelectedIndex = -1;
                        break;
                    case "Todo":
                        Txt_Enero.Text = "";
                        Txt_Febrero.Text = "";
                        Txt_Marzo.Text = "";
                        Txt_Abril.Text = "";
                        Txt_Mayo.Text = "";
                        Txt_Junio.Text = "";
                        Txt_Julio.Text = "";
                        Txt_Agosto.Text = "";
                        Txt_Septiembre.Text = "";
                        Txt_Octubre.Text = "";
                        Txt_Noviembre.Text = "";
                        Txt_Diciembre.Text = "";
                        Txt_Total.Text = "";
                        Lbl_Cantidad.Text = "";
                        Lbl_Txt_Enero.Text = "";
                        Lbl_Txt_Febrero.Text = "";
                        Lbl_Txt_Marzo.Text = "";
                        Lbl_Txt_Abril.Text = "";
                        Lbl_Txt_Mayo.Text = "";
                        Lbl_Txt_Junio.Text = "";
                        Lbl_Txt_Julio.Text = "";
                        Lbl_Txt_Agosto.Text = "";
                        Lbl_Txt_Septiembre.Text = "";
                        Lbl_Txt_Octubre.Text = "";
                        Lbl_Txt_Noviembre.Text = "";
                        Lbl_Txt_Diciembre.Text = "";
                        Txt_Comentarios.Text = "";
                        Txt_Justificacion.Text = "";
                        Txt_Limite_Presupuestal.Text = "";
                        Txt_Programa.Text = "";
                        Txt_Total_Ajuste.Text = "";
                        Txt_Unidad_Responsable.Text = "";
                        Cmb_Capitulos.SelectedIndex = -1;
                        Cmb_Partida_Especifica.SelectedIndex = -1;
                        Cmb_Producto.SelectedIndex = -1;
                        Cmb_Stock.SelectedIndex = -1;
                        Cmb_Estatus.SelectedIndex = -1;
                        Hf_Anio.Value = "";
                        Hf_Dependencia_ID.Value = "";
                        Hf_Fte_Financiamiento.Value = "";
                        Hf_Precio.Value = "";
                        Hf_Producto_ID.Value = "";
                        Hf_Programa.Value = "";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Limpiar_Formulario ERROR[" + ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Estado_Botones
        ///DESCRIPCIÓN          : metodo que muestra los botones de acuerdo al estado en el que se encuentre
        ///PARAMETROS:          1.- String Estado: El estado de los botones solo puede tomar 
        ///                        + inicial
        ///                        + nuevo
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 10/Noviembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        private void Estado_Botones(String Estado)
        {
            DataTable Dt_Partidas_Asignadas = new DataTable();
            Dt_Partidas_Asignadas = (DataTable)Session["Dt_Partidas_Asignadas"];
            try
            {
                switch (Estado)
                {
                    case "inicial":
                        //Boton Nuevo
                        Btn_Nuevo.ToolTip = "Nuevo";
                        Btn_Nuevo.Visible = true;
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";

                        //Boton Salir
                        Btn_Salir.ToolTip = "Salir";
                        Btn_Salir.Enabled = true;
                        Btn_Salir.Visible = true;
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        break;
                    case "nuevo":
                        //Boton Nuevo
                        Btn_Nuevo.ToolTip = "Guardar";
                        Btn_Nuevo.Enabled = true;
                        Btn_Nuevo.Visible = true;
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                        //Boton Salir
                        Btn_Salir.ToolTip = "Cancelar";
                        Btn_Salir.Enabled = true;
                        Btn_Salir.Visible = true;
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        Div_Dependencias_Presupuestadas.Style.Add("display", "none");
                        Div_Partidas_Asignadas.Style.Add("display", "block");
                        break;
                }//fin del switch
            }
            catch (Exception ex)
            {
                throw new Exception("Error al habilitar el  Estado_Botones ERROR[" + ex.Message + "]");
            }
        }

        //********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Limpiar_Sessiones
        ///DESCRIPCIÓN          : Metodo para limpiar las sessiones del formulario
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 12/Noviembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        private void Limpiar_Sessiones()
        {
            Session["Dt_Partidas_Asignadas"] = null;
        }

        //********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Validar_Datos
        ///DESCRIPCIÓN          : Metodo para validar los datos del formulario
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 23/Noviembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        private Boolean Validar_Datos()
        {
            Boolean Datos_Validos = true;
            Lbl_Error.Text = String.Empty;

            try
            {
                if (Cmb_Estatus.SelectedIndex <= 0)
                {
                    Lbl_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Seleccionar un estatus. <br />";
                    Datos_Validos = false;
                }
                else 
                {
                    if (Cmb_Estatus.SelectedValue == "RECHAZADO")
                    {
                        if (String.IsNullOrEmpty(Txt_Comentarios.Text.Trim()))
                        {
                            Lbl_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Introducir un comentario. <br />";
                            Datos_Validos = false;
                        }
                    }
                }
                return Datos_Validos;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al tratar de validar los datos Error[" + ex.Message + "]");
            }
        }

        //********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Mostrar_Ocultar_Error
        ///DESCRIPCIÓN          : Metodo para mostrar u ocultar los errores
        ///PROPIEDADES          1 Estatus ocultar o mostrar
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 12/Noviembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        private void Mostrar_Ocultar_Error(Boolean Estatus)
        {
            Lbl_Error.Visible = Estatus;
            Lbl_Encanezado_Error.Visible = Estatus;
            Img_Error.Visible = Estatus;
        }

        //********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : LLenar_Presupuesto_Dependencia
        ///DESCRIPCIÓN          : Metodo para mostrar los datos del presupuesto dela dependencia
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 14/Noviembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        private void LLenar_Presupuesto_Dependencia()
        {
            Cls_Ope_Psp_Calendarizar_Presupuesto_Negocio Negocio = new Cls_Ope_Psp_Calendarizar_Presupuesto_Negocio();
            DataTable Dt_Datos_Dependencia = new DataTable();
            try
            {

                Hf_Fte_Financiamiento.Value = "";
                Hf_Programa.Value = "";
                if (!String.IsNullOrEmpty(Hf_Dependencia_ID.Value.Trim()))
                {
                    Negocio.P_Dependencia_ID = Hf_Dependencia_ID.Value.Trim();
                    Dt_Datos_Dependencia = Negocio.Consultar_Presupuesto_Dependencia();
                    if (Dt_Datos_Dependencia != null)
                    {
                        if (Dt_Datos_Dependencia.Rows.Count > 0)
                        {
                            Hf_Fte_Financiamiento.Value = Dt_Datos_Dependencia.Rows[0]["FTE_FINANCIAMIENTO_ID"].ToString().Trim();
                            Hf_Programa.Value = Dt_Datos_Dependencia.Rows[0]["PROYECTO_PROGRAMA_ID"].ToString().Trim();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al tratar de llenar los datos del presupuesto de la dependencia Error[" + ex.Message + "]");
            }
        }

        //********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Combo_Stock
        ///DESCRIPCIÓN          : Metodo para consultar si una partida es de stock o no
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 14/Noviembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        private void Consultar_Combo_Stock()
        {
            Cls_Ope_Psp_Calendarizar_Presupuesto_Negocio Negocio = new Cls_Ope_Psp_Calendarizar_Presupuesto_Negocio();
            DataTable Dt_Partida = new DataTable();
            try
            {
                if (Cmb_Partida_Especifica.SelectedIndex > 0)
                {
                    Negocio.P_Anio_Presupuesto = Hf_Anio.Value.Trim();
                    Negocio.P_Partida_ID = Cmb_Partida_Especifica.SelectedValue.Trim();
                    Dt_Partida = Negocio.Consultar_Partida_Stock();
                    if (Dt_Partida != null)
                    {
                        if (Dt_Partida.Rows.Count > 0)
                        {
                            Cmb_Stock.SelectedValue = "SI";
                        }
                        else
                        {
                            Cmb_Stock.SelectedValue = "NO";
                        }
                    }
                    else
                    {
                        Cmb_Stock.SelectedValue = "NO";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al calcular el total del presupuesto. Error[" + ex.Message + "]");
            }
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Llenar_Grid_Partida_Asignada
        ///DESCRIPCIÓN          : Metodo para llenar el grid anidado de las partidas asignadas
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 16/Noviembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        private void Llenar_Grid_Partida_Asignada()
        {
            Cls_Ope_Psp_Calendarizar_Presupuesto_Negocio Negocio = new Cls_Ope_Psp_Calendarizar_Presupuesto_Negocio();
            DataTable Dt_Session = new DataTable();
            DataTable Dt_Partidas_Asignadas = new DataTable();
            Dt_Session = (DataTable)Session["Dt_Partidas_Asignadas"];
            Limpiar_Sessiones();
            try
            {
                if (Dt_Session != null)
                {
                    if (Dt_Session.Rows.Count > 0)
                    {
                        Dt_Partidas_Asignadas = Dt_Session; ;
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(Hf_Dependencia_ID.Value.Trim()))
                        {
                            Negocio.P_Dependencia_ID = Hf_Dependencia_ID.Value.Trim();
                            Negocio.P_Estatus = "GENERADO";
                            Dt_Partidas_Asignadas = Negocio.Consultar_Partidas_Asignadas();
                            Session["Dt_Partidas_Asignadas"] = Dt_Partidas_Asignadas;
                        }
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(Hf_Dependencia_ID.Value.Trim()))
                    {
                        Negocio.P_Dependencia_ID = Hf_Dependencia_ID.Value.Trim();
                        Negocio.P_Estatus = "GENERADO";
                        Dt_Partidas_Asignadas = Negocio.Consultar_Partidas_Asignadas();
                        Session["Dt_Partidas_Asignadas"] = Dt_Partidas_Asignadas;
                    }
                }
                if (Dt_Partidas_Asignadas != null)
                {
                    if (Dt_Partidas_Asignadas.Rows.Count > 0)
                    {
                        Obtener_Consecutivo_ID();
                        Dt_Partidas_Asignadas = Crear_Dt_Partida_Asignada();
                        Grid_Partida_Asignada.Columns[1].Visible = true;
                        Grid_Partida_Asignada.DataSource = Dt_Partidas_Asignadas;
                        Grid_Partida_Asignada.DataBind();
                        Grid_Partida_Asignada.Columns[1].Visible = false;
                    }
                    else
                    {
                        Grid_Partida_Asignada.DataSource = Dt_Partidas_Asignadas;
                        Grid_Partida_Asignada.DataBind();
                    }
                }
                else
                {
                    Grid_Partida_Asignada.DataSource = Dt_Partidas_Asignadas;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al tratar de llenar la tabla de las partidas asignadas Error[" + ex.Message + "]");
            }
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Crear_Dt_Partida_Asignada
        ///DESCRIPCIÓN          : Metodo para crear el datatable de las partidas asignadas del grid anidado
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 16/Noviembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        private DataTable Crear_Dt_Partida_Asignada()
        {
            DataTable Dt_Partida_Asignada = new DataTable();
            DataTable Dt_Session = new DataTable();
            Dt_Session = (DataTable)Session["Dt_Partidas_Asignadas"];
            String Partida_Id = string.Empty;
            Double Ene = 0.00;
            Double Feb = 0.00;
            Double Mar = 0.00;
            Double Abr = 0.00;
            Double May = 0.00;
            Double Jun = 0.00;
            Double Jul = 0.00;
            Double Ago = 0.00;
            Double Sep = 0.00;
            Double Oct = 0.00;
            Double Nov = 0.00;
            Double Dic = 0.00;
            Double Total = 0.00;
            DataRow Fila;
            String Clave = String.Empty;
            Boolean Iguales;

            try
            {
                if (Dt_Session != null)
                {
                    if (Dt_Session.Rows.Count > 0)
                    {
                        //creamos las columnas del datatable donde se guardaran los datos
                        Dt_Partida_Asignada.Columns.Add("PARTIDA_ID", System.Type.GetType("System.String"));
                        Dt_Partida_Asignada.Columns.Add("CLAVE", System.Type.GetType("System.String"));
                        Dt_Partida_Asignada.Columns.Add("TOTAL_ENE", System.Type.GetType("System.String"));
                        Dt_Partida_Asignada.Columns.Add("TOTAL_FEB", System.Type.GetType("System.String"));
                        Dt_Partida_Asignada.Columns.Add("TOTAL_MAR", System.Type.GetType("System.String"));
                        Dt_Partida_Asignada.Columns.Add("TOTAL_ABR", System.Type.GetType("System.String"));
                        Dt_Partida_Asignada.Columns.Add("TOTAL_MAY", System.Type.GetType("System.String"));
                        Dt_Partida_Asignada.Columns.Add("TOTAL_JUN", System.Type.GetType("System.String"));
                        Dt_Partida_Asignada.Columns.Add("TOTAL_JUL", System.Type.GetType("System.String"));
                        Dt_Partida_Asignada.Columns.Add("TOTAL_AGO", System.Type.GetType("System.String"));
                        Dt_Partida_Asignada.Columns.Add("TOTAL_SEP", System.Type.GetType("System.String"));
                        Dt_Partida_Asignada.Columns.Add("TOTAL_OCT", System.Type.GetType("System.String"));
                        Dt_Partida_Asignada.Columns.Add("TOTAL_NOV", System.Type.GetType("System.String"));
                        Dt_Partida_Asignada.Columns.Add("TOTAL_DIC", System.Type.GetType("System.String"));
                        Dt_Partida_Asignada.Columns.Add("TOTAL", System.Type.GetType("System.String"));


                        foreach (DataRow Dr_Sessiones in Dt_Session.Rows)
                        {
                            //Obtenemos la partida id y la clave
                            Partida_Id = Dr_Sessiones["PARTIDA_ID"].ToString().Trim();
                            Clave = Dr_Sessiones["CLAVE_PARTIDA"].ToString().Trim();
                            Iguales = false;
                            //verificamos si la partida no a sido ya agrupada
                            if (Dt_Partida_Asignada.Rows.Count > 0)
                            {
                                foreach (DataRow Dr_Partidas in Dt_Partida_Asignada.Rows)
                                {
                                    if (Dr_Partidas["PARTIDA_ID"].ToString().Trim().Equals(Partida_Id))
                                    {
                                        Iguales = true;
                                    }
                                }
                            }

                            // tomamos los datos del datatable para agrupar las partidas asignadas
                            if (!Iguales)
                            {
                                foreach (DataRow Dr_Session in Dt_Session.Rows)
                                {
                                    if (Dr_Session["PARTIDA_ID"].ToString().Trim().Equals(Partida_Id))
                                    {
                                        Ene = Ene + Convert.ToDouble(String.IsNullOrEmpty(Dr_Session["ENERO"].ToString()) ? "0" : Dr_Session["ENERO"].ToString().Trim());
                                        Feb = Feb + Convert.ToDouble(String.IsNullOrEmpty(Dr_Session["FEBRERO"].ToString()) ? "0" : Dr_Session["FEBRERO"].ToString().Trim());
                                        Mar = Mar + Convert.ToDouble(String.IsNullOrEmpty(Dr_Session["MARZO"].ToString()) ? "0" : Dr_Session["MARZO"].ToString().Trim());
                                        Abr = Abr + Convert.ToDouble(String.IsNullOrEmpty(Dr_Session["ABRIL"].ToString()) ? "0" : Dr_Session["ABRIL"].ToString().Trim());
                                        May = May + Convert.ToDouble(String.IsNullOrEmpty(Dr_Session["MAYO"].ToString()) ? "0" : Dr_Session["MAYO"].ToString().Trim());
                                        Jun = Jun + Convert.ToDouble(String.IsNullOrEmpty(Dr_Session["JUNIO"].ToString()) ? "0" : Dr_Session["JUNIO"].ToString().Trim());
                                        Jul = Jul + Convert.ToDouble(String.IsNullOrEmpty(Dr_Session["JULIO"].ToString()) ? "0" : Dr_Session["JULIO"].ToString().Trim());
                                        Ago = Ago + Convert.ToDouble(String.IsNullOrEmpty(Dr_Session["AGOSTO"].ToString()) ? "0" : Dr_Session["AGOSTO"].ToString().Trim());
                                        Sep = Sep + Convert.ToDouble(String.IsNullOrEmpty(Dr_Session["SEPTIEMBRE"].ToString()) ? "0" : Dr_Session["SEPTIEMBRE"].ToString().Trim());
                                        Oct = Oct + Convert.ToDouble(String.IsNullOrEmpty(Dr_Session["OCTUBRE"].ToString()) ? "0" : Dr_Session["OCTUBRE"].ToString().Trim());
                                        Nov = Nov + Convert.ToDouble(String.IsNullOrEmpty(Dr_Session["NOVIEMBRE"].ToString()) ? "0" : Dr_Session["NOVIEMBRE"].ToString().Trim());
                                        Dic = Dic + Convert.ToDouble(String.IsNullOrEmpty(Dr_Session["DICIEMBRE"].ToString()) ? "0" : Dr_Session["DICIEMBRE"].ToString().Trim());
                                    }
                                }
                                Total = Ene + Feb + Mar + Abr + May + Jun + Jul + Ago + Sep + Oct + Nov + Dic;

                                Fila = Dt_Partida_Asignada.NewRow();
                                Fila["PARTIDA_ID"] = Partida_Id;
                                Fila["CLAVE"] = Clave;
                                Fila["TOTAL_ENE"] = String.Format("{0:##,###,##0.00}", Ene);
                                Fila["TOTAL_FEB"] = String.Format("{0:##,###,##0.00}", Feb);
                                Fila["TOTAL_MAR"] = String.Format("{0:##,###,##0.00}", Mar);
                                Fila["TOTAL_ABR"] = String.Format("{0:##,###,##0.00}", Abr);
                                Fila["TOTAL_MAY"] = String.Format("{0:##,###,##0.00}", May);
                                Fila["TOTAL_JUN"] = String.Format("{0:##,###,##0.00}", Jun);
                                Fila["TOTAL_JUL"] = String.Format("{0:##,###,##0.00}", Jul);
                                Fila["TOTAL_AGO"] = String.Format("{0:##,###,##0.00}", Ago);
                                Fila["TOTAL_SEP"] = String.Format("{0:##,###,##0.00}", Sep);
                                Fila["TOTAL_OCT"] = String.Format("{0:##,###,##0.00}", Oct);
                                Fila["TOTAL_NOV"] = String.Format("{0:##,###,##0.00}", Nov);
                                Fila["TOTAL_DIC"] = String.Format("{0:##,###,##0.00}", Dic);
                                Fila["TOTAL"] = String.Format("{0:##,###,##0.00}", Total);
                                Dt_Partida_Asignada.Rows.Add(Fila);

                                Ene = 0.00;
                                Feb = 0.00;
                                Mar = 0.00;
                                Abr = 0.00;
                                May = 0.00;
                                Jun = 0.00;
                                Jul = 0.00;
                                Ago = 0.00;
                                Sep = 0.00;
                                Oct = 0.00;
                                Nov = 0.00;
                                Dic = 0.00;
                                Total = 0.00;
                            }
                        }
                    }
                }
                return Dt_Partida_Asignada;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al tratar de crear la tabla de partidas asignadas Error[" + ex.Message + "]");
            }
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Crear_Dt_Detalles
        ///DESCRIPCIÓN          : Metodo para crear el datatable de los detalles de las partidas asignadas del grid 
        ///PROPIEDADES          1 Partida_ID del cual obtendremos los detalles
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 16/Noviembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        private DataTable Crear_Dt_Detalles(String Partida_ID)
        {
            DataTable Dt_Detalle = new DataTable();
            DataTable Dt_Session = new DataTable();
            Dt_Session = (DataTable)Session["Dt_Partidas_Asignadas"];
            DataRow Fila;

            try
            {
                if (Dt_Session != null)
                {
                    if (Dt_Session.Rows.Count > 0)
                    {
                        Dt_Detalle.Columns.Add("DEPENDENCIA_ID", System.Type.GetType("System.String"));
                        Dt_Detalle.Columns.Add("PROYECTO_ID", System.Type.GetType("System.String"));
                        Dt_Detalle.Columns.Add("CAPITULO_ID", System.Type.GetType("System.String"));
                        Dt_Detalle.Columns.Add("PARTIDA_ID", System.Type.GetType("System.String"));
                        Dt_Detalle.Columns.Add("PRODUCTO_ID", System.Type.GetType("System.String"));
                        Dt_Detalle.Columns.Add("PRECIO", System.Type.GetType("System.Double"));
                        Dt_Detalle.Columns.Add("JUSTIFICACION", System.Type.GetType("System.String"));
                        Dt_Detalle.Columns.Add("CLAVE_PARTIDA", System.Type.GetType("System.String"));
                        Dt_Detalle.Columns.Add("CLAVE_PRODUCTO", System.Type.GetType("System.String"));
                        Dt_Detalle.Columns.Add("ENERO", System.Type.GetType("System.String"));
                        Dt_Detalle.Columns.Add("FEBRERO", System.Type.GetType("System.String"));
                        Dt_Detalle.Columns.Add("MARZO", System.Type.GetType("System.String"));
                        Dt_Detalle.Columns.Add("ABRIL", System.Type.GetType("System.String"));
                        Dt_Detalle.Columns.Add("MAYO", System.Type.GetType("System.String"));
                        Dt_Detalle.Columns.Add("JUNIO", System.Type.GetType("System.String"));
                        Dt_Detalle.Columns.Add("JULIO", System.Type.GetType("System.String"));
                        Dt_Detalle.Columns.Add("AGOSTO", System.Type.GetType("System.String"));
                        Dt_Detalle.Columns.Add("SEPTIEMBRE", System.Type.GetType("System.String"));
                        Dt_Detalle.Columns.Add("OCTUBRE", System.Type.GetType("System.String"));
                        Dt_Detalle.Columns.Add("NOVIEMBRE", System.Type.GetType("System.String"));
                        Dt_Detalle.Columns.Add("DICIEMBRE", System.Type.GetType("System.String"));
                        Dt_Detalle.Columns.Add("IMPORTE_TOTAL", System.Type.GetType("System.String"));
                        Dt_Detalle.Columns.Add("ID", System.Type.GetType("System.String"));

                        foreach (DataRow Dr_Session in Dt_Session.Rows)
                        {
                            if (Dr_Session["PARTIDA_ID"].ToString().Trim().Equals(Partida_ID.Trim()))
                            {
                                Fila = Dt_Detalle.NewRow();
                                Fila["DEPENDENCIA_ID"] = Dr_Session["DEPENDENCIA_ID"].ToString().Trim();
                                Fila["PROYECTO_ID"] = Dr_Session["PROYECTO_ID"].ToString().Trim();
                                Fila["CAPITULO_ID"] = Dr_Session["CAPITULO_ID"].ToString().Trim();
                                Fila["PARTIDA_ID"] = Dr_Session["PARTIDA_ID"].ToString().Trim();
                                Fila["PRODUCTO_ID"] = Dr_Session["PRODUCTO_ID"].ToString().Trim();
                                Fila["PRECIO"] = Dr_Session["PRECIO"];
                                Fila["JUSTIFICACION"] = Dr_Session["JUSTIFICACION"].ToString().Trim();
                                Fila["CLAVE_PARTIDA"] = Dr_Session["CLAVE_PARTIDA"].ToString().Trim();
                                Fila["CLAVE_PRODUCTO"] = Dr_Session["CLAVE_PRODUCTO"].ToString().Trim();
                                Fila["ENERO"] = String.Format("{0:#,###,##0.00}", Dr_Session["ENERO"]);
                                Fila["FEBRERO"] = String.Format("{0:#,###,##0.00}", Dr_Session["FEBRERO"]);
                                Fila["MARZO"] = String.Format("{0:#,###,##0.00}", Dr_Session["MARZO"]);
                                Fila["ABRIL"] = String.Format("{0:#,###,##0.00}", Dr_Session["ABRIL"]);
                                Fila["MAYO"] = String.Format("{0:#,###,##0.00}", Dr_Session["MAYO"]);
                                Fila["JUNIO"] = String.Format("{0:#,###,##0.00}", Dr_Session["JUNIO"]);
                                Fila["JULIO"] = String.Format("{0:#,###,##0.00}", Dr_Session["JULIO"]);
                                Fila["AGOSTO"] = String.Format("{0:#,###,##0.00}", Dr_Session["AGOSTO"]);
                                Fila["SEPTIEMBRE"] = String.Format("{0:#,###,##0.00}", Dr_Session["SEPTIEMBRE"]);
                                Fila["OCTUBRE"] = String.Format("{0:#,###,##0.00}", Dr_Session["OCTUBRE"]);
                                Fila["NOVIEMBRE"] = String.Format("{0:#,###,##0.00}", Dr_Session["NOVIEMBRE"]);
                                Fila["DICIEMBRE"] = String.Format("{0:#,###,##0.00}", Dr_Session["DICIEMBRE"]);
                                Fila["IMPORTE_TOTAL"] = String.Format("{0:#,###,##0.00}", Dr_Session["IMPORTE_TOTAL"]);
                                Fila["ID"] = Dr_Session["ID"].ToString().Trim();
                                Dt_Detalle.Rows.Add(Fila);
                            }
                        }
                    }
                }
                return Dt_Detalle;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al tratar de crear la tabla de partidas asignadas Error[" + ex.Message + "]");
            }
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Obtener_Consecutivo_ID
        ///DESCRIPCIÓN          : Metodo para obtener el consecutivo del datatable 
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 16/Noviembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        private void Obtener_Consecutivo_ID()
        {
            DataTable Dt_Partidas_Asignadas = new DataTable();
            Dt_Partidas_Asignadas = (DataTable)Session["Dt_Partidas_Asignadas"];
            int Contador;
            try
            {
                if (Dt_Partidas_Asignadas != null)
                {
                    if (Dt_Partidas_Asignadas.Columns.Count > 0)
                    {
                        if (Dt_Partidas_Asignadas.Rows.Count > 0)
                        {
                            Contador = -1;
                            foreach (DataRow Dr in Dt_Partidas_Asignadas.Rows)
                            {
                                Contador++;
                                Dr["ID"] = Contador.ToString().Trim();
                            }
                        }
                    }
                }
                Session["Dt_Partidas_Asignadas"] = Dt_Partidas_Asignadas;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el datatable con consecutivo Erro[" + ex.Message + "]");
            }
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Llenar_Grid_Dependencias_Presupuestadas
        ///DESCRIPCIÓN          : Metodo para llenar el grid de las dependencias presupuestadas
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 23/Noviembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        private void Llenar_Grid_Dependencias_Presupuestadas()
        {
            Cls_Ope_Psp_Calendarizar_Presupuesto_Negocio Negocio = new Cls_Ope_Psp_Calendarizar_Presupuesto_Negocio();
            DataTable Dt_Dependencias = new DataTable();
            try
            {
                Grid_Dependencias_Presupuestadas.DataBind();
                Dt_Dependencias = Negocio.Consultar_Dependencias_Presupuestadas();
                if (Dt_Dependencias != null)
                {
                    if (Dt_Dependencias.Rows.Count > 0)
                    {
                        Grid_Dependencias_Presupuestadas.Columns[1].Visible = true;
                        Grid_Dependencias_Presupuestadas.Columns[2].Visible = true;
                        Grid_Dependencias_Presupuestadas.Columns[3].Visible = true;
                        Grid_Dependencias_Presupuestadas.Columns[4].Visible = true;
                        Grid_Dependencias_Presupuestadas.DataSource = Dt_Dependencias;
                        Grid_Dependencias_Presupuestadas.DataBind();
                        Grid_Dependencias_Presupuestadas.Columns[1].Visible = false;
                        Grid_Dependencias_Presupuestadas.Columns[2].Visible = false;
                        Grid_Dependencias_Presupuestadas.Columns[3].Visible = false;
                        Grid_Dependencias_Presupuestadas.Columns[4].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al tratar de llenar la tabla de las partidas asignadas Error[" + ex.Message + "]");
            }
        }
    #endregion

    #region (Metodos Combos)
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Llenar_Combo_Capitulos
        ///DESCRIPCIÓN          : Metodo para llenar el combo de los capitulos
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 10/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        private void Llenar_Combo_Capitulos()
        {
            Cls_Ope_Psp_Calendarizar_Presupuesto_Negocio Calendarizar_Negocio = new Cls_Ope_Psp_Calendarizar_Presupuesto_Negocio();
            DataTable Dt_Capitulos = new DataTable();
            try
            {
                if (!String.IsNullOrEmpty(Hf_Dependencia_ID.Value.Trim()))
                {
                    Calendarizar_Negocio.P_Dependencia_ID = Hf_Dependencia_ID.Value.Trim();
                }

                Dt_Capitulos = Calendarizar_Negocio.Consultar_Capitulos();
                if (Dt_Capitulos != null)
                {
                    if (Dt_Capitulos.Rows.Count > 0)
                    {
                        Cmb_Capitulos.Items.Clear();
                        Cmb_Capitulos.DataValueField = Cat_SAP_Capitulos.Campo_Capitulo_ID;
                        Cmb_Capitulos.DataTextField = "NOMBRE";
                        Cmb_Capitulos.DataSource = Dt_Capitulos;
                        Cmb_Capitulos.DataBind();
                    }
                }
                Cmb_Capitulos.Items.Insert(0, new ListItem("<-- SELECCIONE -->", ""));
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Llenar_Combo_Capitulos ERROR[" + ex.Message + "]");
            }
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Llenar_Combo_Partidas
        ///DESCRIPCIÓN          : Metodo para llenar el combo de las partidas de un capitulo
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 10/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        private void Llenar_Combo_Partidas()
        {
            Cls_Ope_Psp_Calendarizar_Presupuesto_Negocio Calendarizar_Negocio = new Cls_Ope_Psp_Calendarizar_Presupuesto_Negocio();
            DataTable Dt_Partidas = new DataTable();
            try
            {
                if (Cmb_Capitulos.SelectedIndex > 0)
                {
                    Calendarizar_Negocio.P_Capitulo_ID = Cmb_Capitulos.SelectedItem.Value.Trim();
                    Dt_Partidas = Calendarizar_Negocio.Consultar_Partidas();
                    if (Dt_Partidas != null)
                    {
                        if (Dt_Partidas.Rows.Count > 0)
                        {
                            Cmb_Partida_Especifica.Items.Clear();
                            Cmb_Partida_Especifica.DataValueField = Cat_Com_Partidas.Campo_Partida_ID;
                            Cmb_Partida_Especifica.DataTextField = "NOMBRE";
                            Cmb_Partida_Especifica.DataSource = Dt_Partidas;
                            Cmb_Partida_Especifica.DataBind();
                            Cmb_Partida_Especifica.Items.Insert(0, new ListItem("<-- SELECCIONE -->", ""));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Llenar_Combo_Partidas ERROR[" + ex.Message + "]");
            }
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Llenar_Combo_Partidas
        ///DESCRIPCIÓN          : Metodo para llenar el combo de las partidas de un capitulo
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 11/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        private void Llenar_Combo_Productos()
        {
            Cls_Ope_Psp_Calendarizar_Presupuesto_Negocio Calendarizar_Negocio = new Cls_Ope_Psp_Calendarizar_Presupuesto_Negocio();
            DataTable Dt_Productos = new DataTable();
            try
            {
                if (Cmb_Partida_Especifica.SelectedIndex > 0)
                {
                    Calendarizar_Negocio.P_Partida_ID = Cmb_Partida_Especifica.SelectedItem.Value.Trim();
                    Dt_Productos = Calendarizar_Negocio.Consultar_Productos();
                    if (Dt_Productos != null)
                    {
                        if (Dt_Productos.Rows.Count > 0)
                        {
                            Cmb_Producto.Items.Clear();
                            Cmb_Producto.DataValueField = Cat_Com_Productos.Campo_Producto_ID;
                            Cmb_Producto.DataTextField = "CLAVE_PRODUCTO";
                            Cmb_Producto.DataSource = Dt_Productos;
                            Cmb_Producto.DataBind();
                            Cmb_Producto.Items.Insert(0, new ListItem("<-- SELECCIONE -->", ""));
                            Tr_Productos.Visible = true;
                        }
                        else
                        {
                            Tr_Productos.Visible = false;
                        }
                    }
                    else
                    {
                        Tr_Productos.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Llenar_Combo_Productos ERROR[" + ex.Message + "]");
            }
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Llenar_Combo_Estatus
        ///DESCRIPCIÓN          : Metodo para llenar el combo del estatus del presupuesto
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 22/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        private void Llenar_Combo_Estatus()
        {
            try
            {
                Cmb_Estatus.Items.Clear();
                Cmb_Estatus.Items.Insert(0, new ListItem("<-- SELECCIONE -->", ""));
                Cmb_Estatus.Items.Insert(1, new ListItem("RECHAZADO", "RECHAZADO"));
                Cmb_Estatus.Items.Insert(2, new ListItem("AUTORIZADO", "AUTORIZADO"));
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Llenar_Combo_Estatus ERROR[" + ex.Message + "]");
            }
        }
    #endregion

    #endregion

    #region EVENTOS

    #region EVENTOS GENERALES
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Salir_Click
        ///DESCRIPCIÓN          : Evento del boton de salir
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 10/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        protected void Btn_Salir_Click(object sender, EventArgs e)
        {
            switch (Btn_Salir.ToolTip)
            {
                case "Cancelar":
                    Limpiar_Sessiones();
                    Autorizar_Presupuesto_Inicio();
                    break;
                case "Salir":
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "window.close();", true);
                    break;
                case "Regresar":
                    Limpiar_Sessiones();
                    Autorizar_Presupuesto_Inicio();
                    break;
            }//fin del switch
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Nuevo_Click
        ///DESCRIPCIÓN          : Evento del boton nuevo
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 10/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        protected void Btn_Nuevo_Click(object sender, EventArgs e)
        {
            Cls_Ope_Psp_Calendarizar_Presupuesto_Negocio Negocio = new Cls_Ope_Psp_Calendarizar_Presupuesto_Negocio();
            Mostrar_Ocultar_Error(false);
            DataTable Dt_Partidas_Asignadas = new DataTable();
            try
            {
                switch (Btn_Nuevo.ToolTip)
                {
                    case "Nuevo":
                        if (!String.IsNullOrEmpty(Hf_Dependencia_ID.Value.Trim()))
                        {
                            Estado_Botones("nuevo");
                            Limpiar_Formulario("Datos_Producto");
                            Habilitar_Forma(true);
                            if (!String.IsNullOrEmpty(Hf_Dependencia_ID.Value.Trim()))
                            {
                                Cmb_Capitulos.Enabled = false;
                                Cmb_Capitulos.Items.Clear();
                            }
                        }
                        else {
                            Lbl_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Seleccionar una Unidad Responsable Presupuestada. <br />";
                            Mostrar_Ocultar_Error(true);
                        }
                        break;
                    case "Guardar":
                        if (Validar_Datos())
                        {
                            Negocio.P_Anio_Presupuesto = Hf_Anio.Value.Trim();
                            Negocio.P_Fuente_Financiamiento_ID = Hf_Fte_Financiamiento.Value.Trim();
                            Negocio.P_Dependencia_ID = Hf_Dependencia_ID.Value.Trim();
                            Negocio.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
                            Negocio.P_Estatus = Cmb_Estatus.SelectedValue.Trim();
                            Negocio.P_Comentario = Txt_Comentarios.Text.Trim();

                            if (Negocio.Guardar_Historial_Calendario())
                            {
                                Limpiar_Sessiones();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alta", "alert('Alta Exitosa');", true);
                                Autorizar_Presupuesto_Inicio();
                            }
                        }
                        else
                        {
                            Mostrar_Ocultar_Error(true);
                        }
                        break;
                }//fin del swirch
            }
            catch (Exception ex)
            {
                throw new Exception("Error al tratar de dar de alta los datos ERROR[" + ex.Message + "]");
            }
        }//fin del boton Nuevo

    #endregion

    #region EVENTOS GRID
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Grid_Partidas_Asignadas_RowDataBound
        ///DESCRIPCIÓN          : Evento del grid del registro que seleccionaremos
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 16/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        protected void Grid_Partida_Asignada_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable Dt_Partidas_Asignadas = new DataTable();
            Dt_Partidas_Asignadas = (DataTable)Session["Dt_Partidas_Asignadas"];

            try
            {
                if (Dt_Partidas_Asignadas != null)
                {
                    if (Dt_Partidas_Asignadas.Rows.Count > 0)
                    {
                        GridView Grid_Partidas_Detalle = (GridView)e.Row.Cells[4].FindControl("Grid_Partidas_Asignadas_Detalle");
                        DataTable Dt_Detalles = new DataTable();
                        String Partida_ID = String.Empty;

                        if (e.Row.RowType == DataControlRowType.DataRow)
                        {
                            Partida_ID = e.Row.Cells[1].Text.Trim();
                            Dt_Detalles = Crear_Dt_Detalles(Partida_ID);

                            Grid_Partidas_Detalle.Columns[0].Visible = true;
                            Grid_Partidas_Detalle.Columns[1].Visible = true;
                            Grid_Partidas_Detalle.Columns[2].Visible = true;
                            Grid_Partidas_Detalle.Columns[3].Visible = true;
                            Grid_Partidas_Detalle.Columns[4].Visible = true;
                            Grid_Partidas_Detalle.Columns[5].Visible = true;
                            Grid_Partidas_Detalle.Columns[6].Visible = true;
                            Grid_Partidas_Detalle.Columns[7].Visible = true;
                            Grid_Partidas_Detalle.Columns[8].Visible = true;
                            Grid_Partidas_Detalle.Columns[23].Visible = true;
                            Grid_Partidas_Detalle.DataSource = Dt_Detalles;
                            Grid_Partidas_Detalle.DataBind();
                            Grid_Partidas_Detalle.Columns[1].Visible = false;
                            Grid_Partidas_Detalle.Columns[2].Visible = false;
                            Grid_Partidas_Detalle.Columns[3].Visible = false;
                            Grid_Partidas_Detalle.Columns[4].Visible = false;
                            Grid_Partidas_Detalle.Columns[5].Visible = false;
                            Grid_Partidas_Detalle.Columns[6].Visible = false;
                            Grid_Partidas_Detalle.Columns[7].Visible = false;
                            Grid_Partidas_Detalle.Columns[8].Visible = false;
                            Grid_Partidas_Detalle.Columns[23].Visible = false;

                            if (Btn_Nuevo.ToolTip == "Guardar")
                            {
                                Grid_Partidas_Detalle.Columns[0].Visible = false;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error:[" + Ex.Message + "]");
            }
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Grid_Partidas_Asignadas_Detalle_SelectedIndexChanged
        ///DESCRIPCIÓN          : Evento del grid del registro que seleccionaremos
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 22/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        protected void Grid_Partidas_Asignadas_Detalle_SelectedIndexChanged(object sender, EventArgs e)
        {
            Double Ene = 0.00;
            Double Feb = 0.00;
            Double Mar = 0.00;
            Double Abr = 0.00;
            Double May = 0.00;
            Double Jun = 0.00;
            Double Jul = 0.00;
            Double Ago = 0.00;
            Double Sep = 0.00;
            Double Oct = 0.00;
            Double Nov = 0.00;
            Double Dic = 0.00;
            Double Precio = 0.00;
            DataTable Dt_Partidas_Asignadas = new DataTable();
            Int32 No_Fila = -1;
            String Id = String.Empty;
            try
            {
                Limpiar_Formulario("Datos_Producto");

                Id = ((GridView)sender).SelectedRow.Cells[23].Text;
                Dt_Partidas_Asignadas = (DataTable)Session["Dt_Partidas_Asignadas"];
                if (Dt_Partidas_Asignadas != null)
                {
                    if (Dt_Partidas_Asignadas.Rows.Count > 0)
                    {
                        foreach (DataRow Dr in Dt_Partidas_Asignadas.Rows)
                        {
                            No_Fila++;
                            if (Dr["ID"].ToString().Trim().Equals(Id))
                            {
                                
                                Llenar_Combo_Capitulos();
                                LLenar_Presupuesto_Dependencia();
                                Cmb_Capitulos.SelectedIndex = Cmb_Capitulos.Items.IndexOf(Cmb_Capitulos.Items.FindByValue(Dt_Partidas_Asignadas.Rows[No_Fila]["CAPITULO_ID"].ToString().Trim()));
                                Llenar_Combo_Partidas();
                                Cmb_Partida_Especifica.SelectedIndex = Cmb_Partida_Especifica.Items.IndexOf(Cmb_Partida_Especifica.Items.FindByValue(Dt_Partidas_Asignadas.Rows[No_Fila]["PARTIDA_ID"].ToString().Trim()));
                                Llenar_Combo_Productos();
                                Consultar_Combo_Stock();
                                Txt_Justificacion.Text = Dt_Partidas_Asignadas.Rows[No_Fila]["JUSTIFICACION"].ToString().Trim();
                                if (!string.IsNullOrEmpty(Dt_Partidas_Asignadas.Rows[No_Fila]["PRODUCTO_ID"].ToString().Trim()))
                                {
                                    Cmb_Producto.SelectedIndex = Cmb_Producto.Items.IndexOf(Cmb_Producto.Items.FindByValue(Dt_Partidas_Asignadas.Rows[No_Fila]["PRODUCTO_ID"].ToString().Trim()));
                                    Precio = Convert.ToDouble(String.IsNullOrEmpty(Dt_Partidas_Asignadas.Rows[No_Fila]["PRECIO"].ToString().Trim()) ? "0" : Dt_Partidas_Asignadas.Rows[No_Fila]["PRECIO"].ToString().Trim());

                                    if (Precio > 0)
                                    {
                                        Ene = Convert.ToDouble(String.IsNullOrEmpty(Dt_Partidas_Asignadas.Rows[No_Fila]["ENERO"].ToString().Trim()) ? "0" : Dt_Partidas_Asignadas.Rows[No_Fila]["ENERO"].ToString().Trim());
                                        Feb = Convert.ToDouble(String.IsNullOrEmpty(Dt_Partidas_Asignadas.Rows[No_Fila]["FEBRERO"].ToString().Trim()) ? "0" : Dt_Partidas_Asignadas.Rows[No_Fila]["FEBRERO"].ToString().Trim());
                                        Mar = Convert.ToDouble(String.IsNullOrEmpty(Dt_Partidas_Asignadas.Rows[No_Fila]["MARZO"].ToString().Trim()) ? "0" : Dt_Partidas_Asignadas.Rows[No_Fila]["MARZO"].ToString().Trim());
                                        Abr = Convert.ToDouble(String.IsNullOrEmpty(Dt_Partidas_Asignadas.Rows[No_Fila]["ABRIL"].ToString().Trim()) ? "0" : Dt_Partidas_Asignadas.Rows[No_Fila]["ABRIL"].ToString().Trim());
                                        May = Convert.ToDouble(String.IsNullOrEmpty(Dt_Partidas_Asignadas.Rows[No_Fila]["MAYO"].ToString().Trim()) ? "0" : Dt_Partidas_Asignadas.Rows[No_Fila]["MAYO"].ToString().Trim());
                                        Jun = Convert.ToDouble(String.IsNullOrEmpty(Dt_Partidas_Asignadas.Rows[No_Fila]["JUNIO"].ToString().Trim()) ? "0" : Dt_Partidas_Asignadas.Rows[No_Fila]["JUNIO"].ToString().Trim());
                                        Jul = Convert.ToDouble(String.IsNullOrEmpty(Dt_Partidas_Asignadas.Rows[No_Fila]["JULIO"].ToString().Trim()) ? "0" : Dt_Partidas_Asignadas.Rows[No_Fila]["JULIO"].ToString().Trim());
                                        Ago = Convert.ToDouble(String.IsNullOrEmpty(Dt_Partidas_Asignadas.Rows[No_Fila]["AGOSTO"].ToString().Trim()) ? "0" : Dt_Partidas_Asignadas.Rows[No_Fila]["AGOSTO"].ToString().Trim());
                                        Sep = Convert.ToDouble(String.IsNullOrEmpty(Dt_Partidas_Asignadas.Rows[No_Fila]["SEPTIEMBRE"].ToString().Trim()) ? "0" : Dt_Partidas_Asignadas.Rows[No_Fila]["SEPTIEMBRE"].ToString().Trim());
                                        Oct = Convert.ToDouble(String.IsNullOrEmpty(Dt_Partidas_Asignadas.Rows[No_Fila]["OCTUBRE"].ToString().Trim()) ? "0" : Dt_Partidas_Asignadas.Rows[No_Fila]["OCTUBRE"].ToString().Trim());
                                        Nov = Convert.ToDouble(String.IsNullOrEmpty(Dt_Partidas_Asignadas.Rows[No_Fila]["NOVIEMBRE"].ToString().Trim()) ? "0" : Dt_Partidas_Asignadas.Rows[No_Fila]["NOVIEMBRE"].ToString().Trim());
                                        Dic = Convert.ToDouble(String.IsNullOrEmpty(Dt_Partidas_Asignadas.Rows[No_Fila]["DICIEMBRE"].ToString().Trim()) ? "0" : Dt_Partidas_Asignadas.Rows[No_Fila]["DICIEMBRE"].ToString().Trim());

                                        Lbl_Cantidad.Text = "Cantidad";
                                        Lbl_Txt_Enero.Text = Convert.ToString(Ene / Precio);
                                        Lbl_Txt_Febrero.Text = Convert.ToString(Feb / Precio);
                                        Lbl_Txt_Marzo.Text = Convert.ToString(Mar / Precio);
                                        Lbl_Txt_Abril.Text = Convert.ToString(Abr / Precio);
                                        Lbl_Txt_Mayo.Text = Convert.ToString(May / Precio);
                                        Lbl_Txt_Junio.Text = Convert.ToString(Jun / Precio);
                                        Lbl_Txt_Julio.Text = Convert.ToString(Jul / Precio);
                                        Lbl_Txt_Agosto.Text = Convert.ToString(Ago / Precio);
                                        Lbl_Txt_Septiembre.Text = Convert.ToString(Sep / Precio);
                                        Lbl_Txt_Octubre.Text = Convert.ToString(Oct / Precio);
                                        Lbl_Txt_Noviembre.Text = Convert.ToString(Nov / Precio);
                                        Lbl_Txt_Diciembre.Text = Convert.ToString(Dic / Precio);
                                    }
                                }

                                Txt_Enero.Text = Dt_Partidas_Asignadas.Rows[No_Fila]["ENERO"].ToString().Trim();
                                Txt_Febrero.Text = Dt_Partidas_Asignadas.Rows[No_Fila]["FEBRERO"].ToString().Trim();
                                Txt_Marzo.Text = Dt_Partidas_Asignadas.Rows[No_Fila]["MARZO"].ToString().Trim();
                                Txt_Abril.Text = Dt_Partidas_Asignadas.Rows[No_Fila]["ABRIL"].ToString().Trim();
                                Txt_Mayo.Text = Dt_Partidas_Asignadas.Rows[No_Fila]["MAYO"].ToString().Trim();
                                Txt_Junio.Text = Dt_Partidas_Asignadas.Rows[No_Fila]["JUNIO"].ToString().Trim();
                                Txt_Julio.Text = Dt_Partidas_Asignadas.Rows[No_Fila]["JULIO"].ToString().Trim();
                                Txt_Agosto.Text = Dt_Partidas_Asignadas.Rows[No_Fila]["AGOSTO"].ToString().Trim();
                                Txt_Septiembre.Text = Dt_Partidas_Asignadas.Rows[No_Fila]["SEPTIEMBRE"].ToString().Trim();
                                Txt_Octubre.Text = Dt_Partidas_Asignadas.Rows[No_Fila]["OCTUBRE"].ToString().Trim();
                                Txt_Noviembre.Text = Dt_Partidas_Asignadas.Rows[No_Fila]["NOVIEMBRE"].ToString().Trim();
                                Txt_Diciembre.Text = Dt_Partidas_Asignadas.Rows[No_Fila]["DICIEMBRE"].ToString().Trim();
                                Txt_Total.Text = Dt_Partidas_Asignadas.Rows[No_Fila]["IMPORTE_TOTAL"].ToString().Trim();
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al seleccionar el registro el grid. Error[" + ex.Message + "]");
            }
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Grid_Partidas_Asignadas_Detalle_RowCreated
        ///DESCRIPCIÓN          : Evento del grid del movimiento del cursor sobre los registros
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 22/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        protected void Grid_Partidas_Asignadas_Detalle_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#507CD1';this.style.color='#FFFFFF'");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;this.style.color=this.originalstyle;");
            }
        }

        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Grid_Dependencias_Presupuestadas_SelectedIndexChanged
        ///DESCRIPCIÓN          : Ejecuta la operacion de cambio de seleccion en listado 
        ///                       de unidades responsables.
        ///PARAMETROS           : 
        ///CREO                 : Leslie Gonzalez Vázquez
        ///FECHA_CREO           : 23/Noviembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Grid_Dependencias_Presupuestadas_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cls_Ope_Psp_Calendarizar_Presupuesto_Negocio Negocio = new Cls_Ope_Psp_Calendarizar_Presupuesto_Negocio(); //instancia con la clase de negocio
            String Dependencia_ID = String.Empty; ;
            DataTable Dt_Capitulos = new DataTable();
            try
            {
                if (Grid_Dependencias_Presupuestadas.SelectedIndex > (-1))
                {
                    Div_Dependencias_Presupuestadas.Style.Add("display", "none");
                    Div_Partidas_Asignadas.Style.Add("display", "block");
                    Mostrar_Ocultar_Error(false);
                    Limpiar_Formulario("Todo");
                    Hf_Dependencia_ID.Value = HttpUtility.HtmlDecode(Grid_Dependencias_Presupuestadas.SelectedRow.Cells[1].Text).Trim();
                    Llenar_Combo_Capitulos();
                    Llenar_Combo_Estatus();
                    LLenar_Presupuesto_Dependencia();
                    Estado_Botones("inicial");
                    Llenar_Grid_Partida_Asignada();
                    Habilitar_Forma(false);
                    Tr_Productos.Visible = false;
                    Estado_Botones("inicial");
                    Btn_Salir.ToolTip = "Regresar";

                    Txt_Unidad_Responsable.Text = HttpUtility.HtmlDecode(Grid_Dependencias_Presupuestadas.SelectedRow.Cells[5].Text).Trim();
                    Txt_Programa.Text = HttpUtility.HtmlDecode(Grid_Dependencias_Presupuestadas.SelectedRow.Cells[2].Text).Trim();
                    Txt_Limite_Presupuestal.Text = String.Format("{0:c}", Convert.ToDouble(String.IsNullOrEmpty(Grid_Dependencias_Presupuestadas.SelectedRow.Cells[3].Text.Trim()) ? "0" : Grid_Dependencias_Presupuestadas.SelectedRow.Cells[3].Text.Trim()));
                    Txt_Total_Ajuste.Text = String.Format("{0:c}", Convert.ToDouble(String.IsNullOrEmpty(Grid_Dependencias_Presupuestadas.SelectedRow.Cells[7].Text.Trim()) ? "0" : Grid_Dependencias_Presupuestadas.SelectedRow.Cells[7].Text.Replace("$", "").Trim()));
                    Hf_Anio.Value = HttpUtility.HtmlDecode(Grid_Dependencias_Presupuestadas.SelectedRow.Cells[6].Text).Trim();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al querer seleccionar un registro del grid de unidades Responsables Error[" + ex.Message + "]");
            }
        }
    #endregion

    #endregion
}
