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
using Presidencia.Limite_Presupuestal.Negocio;
using Presidencia.Ope_Psp_Asignar_Partida.Negocio;
using Presidencia.Paramentros_Presupuestos.Negocio;

public partial class paginas_Presupuestos_Frm_Ope_Psp_Asignar_Partida_Emergente : System.Web.UI.Page
{
    #region PAGE LOAD

        protected void Page_Load(object sender, EventArgs e)
    {
        if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
        if (!IsPostBack)
        {
            Asignar_Partida_Inicio();
        }
    }

    #endregion

    #region METODOS

        #region (Metodos Generales)
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Asignar_Partida_Inicio
        ///DESCRIPCIÓN          : Metodo para el inicio de la pagina
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 24/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        private void Asignar_Partida_Inicio()
        {
            try
            {
                Mostrar_Ocultar_Error(false);
                Limpiar_Formulario("Todo");
                Llenar_Combo_Unidad_Responsable();
                Llenar_Combo_Estatus();
                Estado_Botones("inicial");
                Habilitar_Forma(false);
                Tr_Productos.Visible = false;
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
                Cmb_Unidad_Responsable.Enabled = Estatus;
                Cmb_Capitulos.Enabled = Estatus;
                Cmb_Partida_Especifica.Enabled = Estatus;
                Txt_Justificacion.Enabled = Estatus;
                Txt_Enero.Enabled = Estatus;
                Txt_Febrero.Enabled = Estatus;
                Txt_Marzo.Enabled = Estatus;
                Txt_Abril.Enabled = Estatus;
                Txt_Mayo.Enabled = Estatus;
                Txt_Junio.Enabled = Estatus;
                Txt_Julio.Enabled = Estatus;
                Txt_Agosto.Enabled = Estatus;
                Txt_Septiembre.Enabled = Estatus;
                Txt_Octubre.Enabled = Estatus;
                Txt_Noviembre.Enabled = Estatus;
                Txt_Diciembre.Enabled = Estatus;
                Btn_Agregar.Enabled = Estatus;
                Cmb_Producto.Enabled = Estatus;
                Btn_Buscar_Producto.Enabled = Estatus;
                Cmb_Estatus.Enabled = Estatus;
                Cmb_Fuente_Financiamiento.Enabled = false;
                Cmb_Programa.Enabled = false;

            }
            catch (Exception ex)
            {
                throw new Exception("Error al Habilitar_Forma ERROR[" + ex.Message + "]");
            }
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Limpiar_Formulario
        ///DESCRIPCIÓN          : Metodo para limpiar los controles de la pagina
        ///PROPIEDADES          1 Accion para especificar que parte se limpiara si todo o las partidas asignadas
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
            switch (Accion)
            {
                case "Todo":
                    Cmb_Capitulos.SelectedIndex = -1;
                    Cmb_Partida_Especifica.SelectedIndex = -1;
                    Cmb_Producto.SelectedIndex = -1;
                    Txt_Justificacion.Text = "";
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
                    Cmb_Programa.SelectedIndex = -1;
                    Cmb_Unidad_Responsable.SelectedIndex = -1;
                    Cmb_Fuente_Financiamiento.SelectedIndex = -1;
                    Txt_Total_Ajuste.Text = "";
                    Hf_Anio.Value = "";
                    break;
                case "Datos_Partida":
                    Cmb_Producto.SelectedIndex = -1;
                    Grid_Partida_Asignada.SelectedIndex = -1;
                    Txt_Justificacion.Text = "";
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
                    
                    break;
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
        ///                        + modificar
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
                        Btn_Nuevo.Enabled = true;
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
                        break;
                }//fin del switch
            }
            catch (Exception ex)
            {
                throw new Exception("Error al habilitar el  Estado_Botones ERROR[" + ex.Message + "]");
            }
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Crear_Dt_Partidas_Asignadas
        ///DESCRIPCIÓN          : Metodo para crear las columnas del datatable
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 11/Noviembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        private void Crear_Dt_Partidas_Asignadas()
        {
            DataTable Dt_Partidas_Asignadas = new DataTable();
            try
            {
                Dt_Partidas_Asignadas.Columns.Add("DEPENDENCIA_ID", System.Type.GetType("System.String"));
                Dt_Partidas_Asignadas.Columns.Add("PROYECTO_ID", System.Type.GetType("System.String"));
                Dt_Partidas_Asignadas.Columns.Add("CAPITULO_ID", System.Type.GetType("System.String"));
                Dt_Partidas_Asignadas.Columns.Add("PARTIDA_ID", System.Type.GetType("System.String"));
                Dt_Partidas_Asignadas.Columns.Add("PRODUCTO_ID", System.Type.GetType("System.String"));
                Dt_Partidas_Asignadas.Columns.Add("PRECIO", System.Type.GetType("System.Double"));
                Dt_Partidas_Asignadas.Columns.Add("JUSTIFICACION", System.Type.GetType("System.String"));
                Dt_Partidas_Asignadas.Columns.Add("CLAVE_PARTIDA", System.Type.GetType("System.String"));
                Dt_Partidas_Asignadas.Columns.Add("CLAVE_PRODUCTO", System.Type.GetType("System.String"));
                Dt_Partidas_Asignadas.Columns.Add("ENERO", System.Type.GetType("System.String"));
                Dt_Partidas_Asignadas.Columns.Add("FEBRERO", System.Type.GetType("System.String"));
                Dt_Partidas_Asignadas.Columns.Add("MARZO", System.Type.GetType("System.String"));
                Dt_Partidas_Asignadas.Columns.Add("ABRIL", System.Type.GetType("System.String"));
                Dt_Partidas_Asignadas.Columns.Add("MAYO", System.Type.GetType("System.String"));
                Dt_Partidas_Asignadas.Columns.Add("JUNIO", System.Type.GetType("System.String"));
                Dt_Partidas_Asignadas.Columns.Add("JULIO", System.Type.GetType("System.String"));
                Dt_Partidas_Asignadas.Columns.Add("AGOSTO", System.Type.GetType("System.String"));
                Dt_Partidas_Asignadas.Columns.Add("SEPTIEMBRE", System.Type.GetType("System.String"));
                Dt_Partidas_Asignadas.Columns.Add("OCTUBRE", System.Type.GetType("System.String"));
                Dt_Partidas_Asignadas.Columns.Add("NOVIEMBRE", System.Type.GetType("System.String"));
                Dt_Partidas_Asignadas.Columns.Add("DICIEMBRE", System.Type.GetType("System.String"));
                Dt_Partidas_Asignadas.Columns.Add("IMPORTE_TOTAL", System.Type.GetType("System.String"));
                Dt_Partidas_Asignadas.Columns.Add("ID", System.Type.GetType("System.String"));
                Dt_Partidas_Asignadas.Columns.Add("FUENTE_FINANCIAMIENTO_ID", System.Type.GetType("System.String"));

                Session["Dt_Partidas_Asignadas"] = (DataTable)Dt_Partidas_Asignadas;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al tratar de crear las columnas de la tabla Error[" + ex.Message + "]");
            }
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Agregar_Fila_Dt_Partidas_Asignadas
        ///DESCRIPCIÓN          : Metodo para agregar los datos de las partidas asignadas al datatable
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 11/Noviembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        private void Agregar_Fila_Dt_Partidas_Asignadas()
        {
            DataTable Dt_Partidas_Asignadas = new DataTable();
            DataRow Fila;
            Dt_Partidas_Asignadas = (DataTable)Session["Dt_Partidas_Asignadas"];
            try
            {
                Fila = Dt_Partidas_Asignadas.NewRow();
                Fila["DEPENDENCIA_ID"] = Cmb_Unidad_Responsable.SelectedItem.Value.Trim();
                Fila["PROYECTO_ID"] = Cmb_Programa.SelectedValue.Trim();
                Fila["CAPITULO_ID"] = Cmb_Capitulos.SelectedItem.Value.Trim();
                Fila["PARTIDA_ID"] = Cmb_Partida_Especifica.SelectedItem.Value.Trim();
                if (!string.IsNullOrEmpty(Hf_Producto_ID.Value.Trim())) //verificamos si se selecciono un producto
                {
                    Fila["PRODUCTO_ID"] = Cmb_Producto.SelectedItem.Value.Trim();
                    Fila["PRECIO"] = Convert.ToDouble(String.IsNullOrEmpty(Hf_Precio.Value.Trim()) ? "0" : Hf_Precio.Value.Trim());
                }
                else
                {
                    Fila["PRODUCTO_ID"] = String.Empty;
                    Fila["PRECIO"] = 0;
                }

                Fila["JUSTIFICACION"] = Txt_Justificacion.Text.Trim();

                if (!string.IsNullOrEmpty(Hf_Producto_ID.Value.Trim())) //si existe un producto ponermos su clave y si no la clave de la partida
                {
                    Fila["CLAVE_PARTIDA"] = Cmb_Partida_Especifica.SelectedItem.Text.Substring(0, Cmb_Partida_Especifica.SelectedItem.Text.IndexOf(" ")).Trim();
                    Fila["CLAVE_PRODUCTO"] = Cmb_Producto.SelectedItem.Text.Substring(0, Cmb_Producto.SelectedItem.Text.IndexOf(" ")).Trim();
                }
                else
                {
                    Fila["CLAVE_PARTIDA"] = Cmb_Partida_Especifica.SelectedItem.Text.Substring(0, Cmb_Partida_Especifica.SelectedItem.Text.IndexOf(" ")).Trim();
                    Fila["CLAVE_PRODUCTO"] = String.Empty;
                }
                Fila["ENERO"] = String.Format("{0:#,###,##0.00}", Convert.ToDouble(String.IsNullOrEmpty(Txt_Enero.Text.Trim()) ? "0" : Txt_Enero.Text.Trim()));
                Fila["FEBRERO"] = String.Format("{0:#,###,##0.00}", Convert.ToDouble(String.IsNullOrEmpty(Txt_Febrero.Text.Trim()) ? "0" : Txt_Febrero.Text.Trim()));
                Fila["MARZO"] = String.Format("{0:#,###,##0.00}", Convert.ToDouble(String.IsNullOrEmpty(Txt_Marzo.Text.Trim()) ? "0" : Txt_Marzo.Text.Trim()));
                Fila["ABRIL"] = String.Format("{0:#,###,##0.00}", Convert.ToDouble(String.IsNullOrEmpty(Txt_Abril.Text.Trim()) ? "0" : Txt_Abril.Text.Trim()));
                Fila["MAYO"] = String.Format("{0:#,###,##0.00}", Convert.ToDouble(String.IsNullOrEmpty(Txt_Mayo.Text.Trim()) ? "0" : Txt_Mayo.Text.Trim()));
                Fila["JUNIO"] = String.Format("{0:#,###,##0.00}", Convert.ToDouble(String.IsNullOrEmpty(Txt_Junio.Text.Trim()) ? "0" : Txt_Junio.Text.Trim()));
                Fila["JULIO"] = String.Format("{0:#,###,##0.00}", Convert.ToDouble(String.IsNullOrEmpty(Txt_Julio.Text.Trim()) ? "0" : Txt_Julio.Text.Trim()));
                Fila["AGOSTO"] = String.Format("{0:#,###,##0.00}", Convert.ToDouble(String.IsNullOrEmpty(Txt_Agosto.Text.Trim()) ? "0" : Txt_Agosto.Text.Trim()));
                Fila["SEPTIEMBRE"] = String.Format("{0:#,###,##0.00}", Convert.ToDouble(String.IsNullOrEmpty(Txt_Septiembre.Text.Trim()) ? "0" : Txt_Septiembre.Text.Trim()));
                Fila["OCTUBRE"] = String.Format("{0:#,###,##0.00}", Convert.ToDouble(String.IsNullOrEmpty(Txt_Octubre.Text.Trim()) ? "0" : Txt_Octubre.Text.Trim()));
                Fila["NOVIEMBRE"] = String.Format("{0:#,###,##0.00}", Convert.ToDouble(String.IsNullOrEmpty(Txt_Noviembre.Text.Trim()) ? "0" : Txt_Noviembre.Text.Trim()));
                Fila["DICIEMBRE"] = String.Format("{0:#,###,##0.00}", Convert.ToDouble(String.IsNullOrEmpty(Txt_Diciembre.Text.Trim()) ? "0" : Txt_Diciembre.Text.Trim()));
                Fila["IMPORTE_TOTAL"] = String.Format("{0:#,###,##0.00}", Convert.ToDouble(String.IsNullOrEmpty(Txt_Total.Text.Trim()) ? "0" : Txt_Total.Text.Trim()));
                Fila["ID"] = "";
                Fila["FUENTE_FINANCIAMIENTO_ID"] = Cmb_Fuente_Financiamiento.SelectedValue.Trim();

                Dt_Partidas_Asignadas.Rows.Add(Fila);

                Session["Dt_Partidas_Asignadas"] = (DataTable)Dt_Partidas_Asignadas;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al tratar de crear las columnas de la tabla Error[" + ex.Message + "]");
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
        ///NOMBRE DE LA FUNCIÓN : Limpiar_Sessiones
        ///DESCRIPCIÓN          : Metodo para limpiar las sessiones del formulario
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 12/Noviembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        private Boolean Validar_Datos()
        {
            Cls_Ope_Psp_Asignar_Partida_Negocio Negocio = new Cls_Ope_Psp_Asignar_Partida_Negocio();
            Boolean Datos_Validos = true;
            Lbl_Error.Text = String.Empty;
            DataTable Dt_Partidas_Autorizadas = new DataTable();
            String Partida_Id = String.Empty;
            String Dependencia_Id = String.Empty;
            String Programa_Id = String.Empty;
            String Fte_Financiamiento_Id = String.Empty;

            try
            {
                if (Cmb_Unidad_Responsable.SelectedIndex <= 0)
                {
                    Lbl_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Seleccionar una unidad responsable. <br />";
                    Datos_Validos = false;
                }
                if (Cmb_Programa.SelectedIndex <= 0)
                {
                    Lbl_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Seleccionar un programa. <br />";
                    Datos_Validos = false;
                }
                if (Cmb_Fuente_Financiamiento.SelectedIndex <= 0)
                {
                    Lbl_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Seleccionar una fuente de financiamiento. <br />";
                    Datos_Validos = false;
                }
                if (Cmb_Capitulos.SelectedIndex <= 0)
                {
                    Lbl_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Seleccionar un capítulo. <br />";
                    Datos_Validos = false;
                }
                if (Cmb_Partida_Especifica.SelectedIndex <= 0)
                {
                    Lbl_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Seleccionar una partida. <br />";
                    Datos_Validos = false;
                }
                if (Tr_Productos.Visible)
                {
                    if (String.IsNullOrEmpty(Hf_Producto_ID.Value.Trim()))
                    {
                        Lbl_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Seleccionar un producto. <br />";
                        Datos_Validos = false;
                    }
                }
                else
                {
                    if (String.IsNullOrEmpty(Txt_Justificacion.Text.Trim()))
                    {
                        Lbl_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Introducir una justificación. <br />";
                        Datos_Validos = false;
                    }
                }
                if (String.IsNullOrEmpty(Txt_Total.Text.Trim()) || Txt_Total.Text.Trim() == "0.00")
                {
                    Lbl_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Introducir la cantidad presupuestal para los meses. <br />";
                    Datos_Validos = false;
                }
                if(Datos_Validos)
                {
                    Negocio.P_Anio_Presupuesto = Hf_Anio.Value.Trim();
                    Negocio.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue.Trim();
                    Dt_Partidas_Autorizadas = Negocio.Consultar_Partidas_Autorizadas();

                    if(Dt_Partidas_Autorizadas != null)
                    {
                        if(Dt_Partidas_Autorizadas.Rows.Count > 0)
                        {
                            Partida_Id = Cmb_Partida_Especifica.SelectedValue.Trim();
                            Fte_Financiamiento_Id = Cmb_Fuente_Financiamiento.SelectedValue.Trim();
                            Programa_Id = Cmb_Programa.SelectedValue.Trim();
                            foreach(DataRow Dr in Dt_Partidas_Autorizadas.Rows)
                            {
                                if (Dr["PARTIDA_ID"].ToString().Trim().Equals(Partida_Id) && Dr["FTE_FINANCIAMIENTO_ID"].ToString().Trim().Equals(Fte_Financiamiento_Id) && Dr["PROYECTO_PROGRAMA_ID"].ToString().Trim().Equals(Programa_Id))
                                {
                                    Lbl_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Seleccionar otra partida. Esta partida ya esta presupuestada. <br />";
                                    Datos_Validos = false;
                                    break;
                                }
                            }
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
        ///FECHA_CREO           : 24/Noviembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        private void LLenar_Presupuesto_Dependencia()
        {
            Cls_Ope_Psp_Calendarizar_Presupuesto_Negocio Negocio = new Cls_Ope_Psp_Calendarizar_Presupuesto_Negocio();
            Cls_Ope_Psp_Asignar_Partida_Negocio Partidas_Negocio = new Cls_Ope_Psp_Asignar_Partida_Negocio();
            DataTable Dt_Estatus = new DataTable();
            DataTable Dt_Comentarios = new DataTable();
            try
            {
                Cmb_Estatus.SelectedIndex = -1;
                Txt_Comentarios.Text = "";
                if (Cmb_Unidad_Responsable.SelectedIndex > 0)
                {
                    Partidas_Negocio.P_Anio_Presupuesto = (DateTime.Today.Year).ToString();
                    Partidas_Negocio.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue.Trim();
                    Dt_Estatus = Partidas_Negocio.Consultar_Estatus_Partidas();
                    if (Dt_Estatus != null)
                    {
                        if (Dt_Estatus.Rows.Count > 0)
                        {
                            if (Dt_Estatus.Rows[0]["ESTATUS"].ToString().Trim().Equals("GENERADO")) 
                            {
                                Lbl_Error.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Esperar a que acepten las otras partidas asignadas, ya que no podras agregar mas por el momento.";
                                Mostrar_Ocultar_Error(true);
                                Habilitar_Forma(false);
                                Estado_Botones("inicial");
                                Llenar_Grid_Partida_Asignada();
                            }
                            if (Dt_Estatus.Rows[0]["ESTATUS"].ToString().Trim().Equals("RECHAZADO"))
                            {
                                Negocio.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue.Trim();
                                Negocio.P_Anio_Presupuesto = Hf_Anio.Value.Trim();
                                Dt_Comentarios = Negocio.Consultar_Comentarios();
                                if (Dt_Comentarios != null)
                                {
                                    if (Dt_Comentarios.Rows.Count > 0)
                                    {
                                        Tr_Comentarios.Visible = true;
                                        Txt_Comentarios.Text = "";
                                        Txt_Comentarios.Text = Dt_Comentarios.Rows[0]["COMENTARIO"].ToString().Trim();
                                        Cmb_Estatus.Items.Insert(3, new ListItem("RECHAZADO", "RECHAZADO"));
                                    }
                                }
                            }
                            else
                            {
                                Tr_Comentarios.Visible = false;
                                Txt_Comentarios.Text = "";
                            }
                            Cmb_Estatus.SelectedValue = Dt_Estatus.Rows[0]["ESTATUS"].ToString().Trim();
                        }
                    }
                    else 
                    {
                        Tr_Comentarios.Visible = false;
                        Txt_Comentarios.Text = "";
                    }
                }
                else
                {
                    Tr_Comentarios.Visible = false;
                    Txt_Comentarios.Text = "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al tratar de llenar los datos del presupuesto de la dependencia Error[" + ex.Message + "]");
            }
        }

        //********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Calcular_Total_Presupuestado
        ///DESCRIPCIÓN          : Metodo para calcular el total que se lleva presupuestodo
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 14/Noviembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        private void Calcular_Total_Presupuestado()
        {
            DataTable Dt_Partidas_Asignadas = new DataTable();
            Dt_Partidas_Asignadas = (DataTable)Session["Dt_Partidas_Asignadas"];
            Double Total = 0.00;

            try
            {
                if (Dt_Partidas_Asignadas != null)
                {
                    if (Dt_Partidas_Asignadas.Rows.Count > 0)
                    {
                        foreach (DataRow Dr in Dt_Partidas_Asignadas.Rows)
                        {
                            Total = Total + Convert.ToDouble(String.IsNullOrEmpty(Dr["IMPORTE_TOTAL"].ToString().Trim()) ? "0" : Dr["IMPORTE_TOTAL"].ToString().Trim());
                        }
                    }
                }
                Txt_Total_Ajuste.Text = "";
                Txt_Total_Ajuste.Text = String.Format("{0:c}", Total);
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
        ///FECHA_CREO           : 24/Noviembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        private void Llenar_Grid_Partida_Asignada()
        {
            Cls_Ope_Psp_Asignar_Partida_Negocio Negocio = new Cls_Ope_Psp_Asignar_Partida_Negocio();
            DataTable Dt_Session = new DataTable();
            DataTable Dt_Partidas_Asignadas = new DataTable();
            Dt_Session = (DataTable)Session["Dt_Partidas_Asignadas"];
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
                        if (Cmb_Unidad_Responsable.SelectedIndex > 0)
                        {
                            Negocio.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue.Trim();
                            Dt_Partidas_Asignadas = Negocio.Consultar_Partidas_Asignadas();
                            Session["Dt_Partidas_Asignadas"] = Dt_Partidas_Asignadas;
                        }
                    }
                }
                else
                {
                    if (Cmb_Unidad_Responsable.SelectedIndex > 0)
                    {
                        Negocio.P_Anio_Presupuesto = Hf_Anio.Value.Trim();
                        Negocio.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue.Trim();
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
                        Dt_Detalle.Columns.Add("FUENTE_FINANCIAMIENTO_ID", System.Type.GetType("System.String"));

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
                                Fila["FUENTE_FINANCIAMIENTO_ID"] = Dr_Session["FUENTE_FINANCIAMIENTO_ID"].ToString().Trim();
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

        
    #endregion

        #region (Metodos Combos)
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Llenar_Combo_Unidad_Responsable
        ///DESCRIPCIÓN          : Metodo para llenar el combo de la unidad responsable
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 24/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        private void Llenar_Combo_Unidad_Responsable()
        {
            Cls_Ope_Psp_Asignar_Partida_Negocio Negocio = new Cls_Ope_Psp_Asignar_Partida_Negocio();
            DataTable Dt_Dependencias = new DataTable();
            String Dependencia_Empleado = string.Empty;
            try
            {
                Dt_Dependencias = Negocio.Consultar_Unidad_Responsable();
                if (Dt_Dependencias != null)
                {
                    if (Dt_Dependencias.Rows.Count > 0)
                    {
                        Cmb_Unidad_Responsable.Items.Clear();
                        Cmb_Unidad_Responsable.DataValueField = Cat_Dependencias.Campo_Dependencia_ID;
                        Cmb_Unidad_Responsable.DataTextField = "NOMBRE";
                        Cmb_Unidad_Responsable.DataSource = Dt_Dependencias;
                        Cmb_Unidad_Responsable.DataBind();
                        Cmb_Unidad_Responsable.Items.Insert(0, new ListItem("<-- SELECCIONE -->", ""));
                    }
                }

                //Dependencia_Empleado = Cls_Sessiones.Dependencia_ID_Empleado; //obtenemos la dependencia del usuario logueado
                //if (!string.IsNullOrEmpty(Dependencia_Empleado))
                //{
                //    Cmb_Unidad_Responsable.SelectedIndex = Cmb_Unidad_Responsable.Items.IndexOf(Cmb_Unidad_Responsable.Items.FindByValue(Dependencia_Empleado));
                //}
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Llenar_Combo_Unidad_Responsable ERROR[" + ex.Message + "]");
            }
        }

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
            Cls_Ope_Psp_Asignar_Partida_Negocio Negocio = new Cls_Ope_Psp_Asignar_Partida_Negocio();
            DataTable Dt_Capitulos = new DataTable();
            try
            {
                Dt_Capitulos = Negocio.Consultar_Capitulos();
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
                            Cmb_Stock.SelectedValue = "SI";
                        }
                        else
                        {
                            Tr_Productos.Visible = false;
                            Cmb_Stock.SelectedValue = "NO";
                        }
                    }
                    else
                    {
                        Tr_Productos.Visible = false;
                        Cmb_Stock.SelectedValue = "NO";
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
                Cmb_Estatus.Items.Insert(1, new ListItem("EN CONSTRUCCION", "EN CONSTRUCCION"));
                Cmb_Estatus.Items.Insert(2, new ListItem("GENERADO", "GENERADO"));
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Llenar_Combo_Partidas ERROR[" + ex.Message + "]");
            }
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Llenar_Combo_Programa
        ///DESCRIPCIÓN          : Metodo para llenar el combo de los programas
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 24/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        private void Llenar_Combo_Programa()
        {
            Cls_Ope_Psp_Limite_Presupuestal_Negocio Negocio = new Cls_Ope_Psp_Limite_Presupuestal_Negocio();//Instancia con la clase de negocios
            DataTable Dt_Programas = new DataTable();
            try
            {
                Cmb_Programa.Items.Clear(); //limpiamos el combo
                if (Cmb_Unidad_Responsable.SelectedIndex > 0)
                {
                    Negocio.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedItem.Value.Trim();
                    Dt_Programas = Negocio.Consultar_Programa_Unidades_Responsables();
                    Cmb_Programa.DataSource = Dt_Programas;
                    Cmb_Programa.DataTextField = "NOMBRE";
                    Cmb_Programa.DataValueField = Cat_Sap_Proyectos_Programas.Campo_Proyecto_Programa_Id;
                    Cmb_Programa.DataBind();
                    Cmb_Programa.Items.Insert(0, new ListItem("<-- SELECCIONE -->", ""));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Llenar_Combo_Programa ERROR[" + ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Llenar_Combo_Fuente_Financiamiento
        ///DESCRIPCIÓN          : Llena el combo de fuente de financiamiento
        ///PARAMETROS           : 
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 24/Noviembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        private void Llenar_Combo_Fuente_Financiamiento()
        {
            Cls_Cat_Psp_Parametros_Negocio Parametros_Negocio = new Cls_Cat_Psp_Parametros_Negocio();//Instancia con la clase de negocios
            try
            {
                Cmb_Fuente_Financiamiento.Items.Clear(); //limpiamos el combo
                Cmb_Fuente_Financiamiento.DataSource = Parametros_Negocio.Consultar_Fuente_Financiamiento();
                Cmb_Fuente_Financiamiento.DataTextField = "NOMBRE";
                Cmb_Fuente_Financiamiento.DataValueField = Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID;
                Cmb_Fuente_Financiamiento.DataBind();
                Cmb_Fuente_Financiamiento.Items.Insert(0, new ListItem("<-- SELECCIONE -->", ""));
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Llenar_Combo_Fuente_Financiamiento ERROR[" + ex.Message + "]");
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
                        Asignar_Partida_Inicio();
                        Pnl_Busqueda_Contenedor.Style.Add("display", "none");
                        break;

                    case "Salir":
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "window.close();", true);
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
                Cls_Ope_Psp_Asignar_Partida_Negocio Negocio = new Cls_Ope_Psp_Asignar_Partida_Negocio();
                Mostrar_Ocultar_Error(false);
                DataTable Dt_Partidas_Asignadas = new DataTable();
                try
                {
                    switch (Btn_Nuevo.ToolTip)
                    {
                        case "Nuevo":
                            Estado_Botones("nuevo");
                            Limpiar_Formulario("Todo");
                            Limpiar_Sessiones();
                            Llenar_Combo_Estatus();
                            Habilitar_Forma(true);
                            Hf_Anio.Value = (DateTime.Today.Year).ToString();
                            if (Cmb_Unidad_Responsable.SelectedIndex <= 0)
                            {
                                Cmb_Capitulos.Enabled = false;
                            }
                            Llenar_Grid_Partida_Asignada();
                            break;
                        case "Guardar":
                            Dt_Partidas_Asignadas = (DataTable)Session["Dt_Partidas_Asignadas"];
                            if (Dt_Partidas_Asignadas != null)
                            {
                                if (Dt_Partidas_Asignadas.Rows.Count > 0)
                                {

                                    if (Cmb_Estatus.SelectedIndex <= 0)
                                    {
                                        Lbl_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Seleccionar un estatus. <br />";
                                        Mostrar_Ocultar_Error(true);
                                    }
                                    else
                                    {
                                        if(!Cmb_Estatus.SelectedValue.Trim().Equals("RECHAZADO"))
                                        {
                                            Negocio.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue.Trim();
                                            Negocio.P_Anio_Presupuesto = Hf_Anio.Value.Trim();
                                            Negocio.P_Dt_Datos = Dt_Partidas_Asignadas;
                                            Negocio.P_Total = Txt_Total_Ajuste.Text.Trim().Replace("$", "");
                                            Negocio.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
                                            Negocio.P_Estatus = Cmb_Estatus.SelectedValue.Trim();

                                            if (Negocio.Guardar_Partidas_Asignadas())
                                            {
                                                Limpiar_Sessiones();
                                                Pnl_Busqueda_Contenedor.Style.Add("display", "none");
                                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alta", "alert('Alta Exitosa');", true);
                                                Asignar_Partida_Inicio();
                                                Llenar_Grid_Partida_Asignada();
                                            }
                                        }else
                                        {
                                            Lbl_Error.Text = " &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Seleccionar un estatus diferente.";
                                            Mostrar_Ocultar_Error(true);
                                        }
                                    }
                                }
                                else
                                {
                                    Lbl_Error.Text = " &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Agregar partidas asignadas ";
                                    Mostrar_Ocultar_Error(true);
                                }

                            }
                            else
                            {
                                Lbl_Error.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Agregar partidas asignadas ";
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

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Btn_Modificar_Click
            ///DESCRIPCIÓN          : Evento del boton modificar
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 10/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN...:
            ///*********************************************************************************************************
            /*protected void Btn_Modificar_Click(object sender, EventArgs e)
            {
                Cls_Ope_Psp_Calendarizar_Presupuesto_Negocio Negocio = new Cls_Ope_Psp_Calendarizar_Presupuesto_Negocio();
                Mostrar_Ocultar_Error(false);
                DataTable Dt_Partidas_Asignadas = new DataTable();
                try
                {
                    switch (Btn_Modificar.ToolTip)
                    {
                        //Validacion para actualizar un registro y para habilitar los controles que se requieran
                        case "Modificar":
                            Estado_Botones("modificar");
                            Habilitar_Forma(true);
                            Grid_Partida_Asignada.SelectedIndex = -1;
                            Limpiar_Formulario("Todo");
                            Llenar_Grid_Partida_Asignada();
                            Llenar_Combo_Estatus();
                            break;
                        case "Actualizar":
                            Dt_Partidas_Asignadas = (DataTable)Session["Dt_Partidas_Asignadas"];
                            if (Dt_Partidas_Asignadas != null)
                            {
                                if (Dt_Partidas_Asignadas.Rows.Count > 0)
                                {
                                    if (Cmb_Estatus.SelectedIndex <= 0)
                                    {
                                        Lbl_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Seleccionar un estatus. <br />";
                                        Mostrar_Ocultar_Error(true);
                                    }
                                    else
                                    {
                                        Negocio.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue.Trim();
                                        Negocio.P_Anio_Presupuesto = Hf_Anio.Value.Trim();
                                        Negocio.P_Dt_Datos = Dt_Partidas_Asignadas;
                                        Negocio.P_Total = Txt_Total_Ajuste.Text.Trim().Replace("$", "");
                                        Negocio.P_Usuario_Modifico = Cls_Sessiones.Nombre_Empleado;
                                        Negocio.P_Estatus = Cmb_Estatus.SelectedValue.Trim();
                                        if (Negocio.Modificar_Partidas_Asignadas())
                                        {
                                            Limpiar_Sessiones();
                                            Pnl_Busqueda_Contenedor.Style.Add("display", "none");
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alta", "alert('Modificación Exitosa');", true);
                                            Asignar_Partida_Inicio();
                                        }
                                    }
                                }
                                else
                                {
                                    Lbl_Error.Text = " Agregar partidas asignadas ";
                                    Mostrar_Ocultar_Error(true);
                                }
                            }
                            else
                            {
                                Lbl_Error.Text = " Agregar partidas asignadas ";
                                Mostrar_Ocultar_Error(true);
                            }
                            break;
                    }//fin del switch
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al tratar de modificar los datos ERROR[" + ex.Message + "]");
                }
            }//fin de Modificar*/

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Btn_Agregar_Click
            ///DESCRIPCIÓN          : Evento del boton de agregar un nuevo presupuesto de un producto
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 11/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN...:
            ///*********************************************************************************************************
            protected void Btn_Agregar_Click(object sender, EventArgs e)
            {
                DataTable Dt_Partidas_Asignadas = new DataTable();
                Dt_Partidas_Asignadas = (DataTable)Session["Dt_Partidas_Asignadas"];
                Mostrar_Ocultar_Error(false);
                try
                {
                    if (Validar_Datos())
                    {
                        if (Dt_Partidas_Asignadas == null)
                        {
                            Crear_Dt_Partidas_Asignadas();
                            Dt_Partidas_Asignadas = (DataTable)Session["Dt_Partidas_Asignadas"];
                        }

                        Agregar_Fila_Dt_Partidas_Asignadas();
                        Limpiar_Formulario("Datos_Partida");
                        Llenar_Grid_Partida_Asignada();
                        Calcular_Total_Presupuestado();
                    }
                    else
                    {
                        Mostrar_Ocultar_Error(true);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al tratar de agregar un registro a la tabla. Error[" + ex.Message + "]");
                }
            }

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Btn_Eliminar_Click
            ///DESCRIPCIÓN          : Evento del boton de eliminar un registro de un presupuesto de un producto
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 14/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN...:
            ///*********************************************************************************************************
            protected void Btn_Eliminar_Click(object sender, EventArgs e)
            {
                DataTable Dt_Partidas_Asignadas = new DataTable();
                ImageButton Btn_Eliminar = (ImageButton)sender;
                Int32 No_Fila = -1;
                String Id = String.Empty;
                try
                {
                    Id = Btn_Eliminar.CommandArgument.ToString().Trim();
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
                                    Dt_Partidas_Asignadas.Rows.RemoveAt(No_Fila);
                                    Session["Dt_Partidas_Asignadas"] = Dt_Partidas_Asignadas;
                                    Llenar_Grid_Partida_Asignada();
                                    Calcular_Total_Presupuestado();
                                    break;
                                }
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar el registro el grid. Error[" + ex.Message + "]");
                }
            }
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
                                Grid_Partidas_Detalle.Columns[24].Visible = true;
                                Grid_Partidas_Detalle.Columns[25].Visible = true;
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
                                Grid_Partidas_Detalle.Columns[24].Visible = false;
                                Grid_Partidas_Detalle.Columns[25].Visible = false;

                                if (Btn_Nuevo.ToolTip == "Guardar")
                                {
                                    Grid_Partidas_Detalle.Columns[0].Visible = false;
                                }
                                else
                                {
                                    Grid_Partidas_Detalle.Columns[23].Visible = false;
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

                    Id = ((GridView)sender).SelectedRow.Cells[24].Text;
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
                                    Llenar_Combo_Unidad_Responsable();
                                    Cmb_Unidad_Responsable.SelectedIndex = Cmb_Unidad_Responsable.Items.IndexOf(Cmb_Unidad_Responsable.Items.FindByValue(Dt_Partidas_Asignadas.Rows[No_Fila]["DEPENDENCIA_ID"].ToString().Trim()));
                                    Llenar_Combo_Programa();
                                    Llenar_Combo_Fuente_Financiamiento();
                                    Cmb_Programa.SelectedIndex = Cmb_Programa.Items.IndexOf(Cmb_Programa.Items.FindByValue(Dt_Partidas_Asignadas.Rows[No_Fila]["PROYECTO_ID"].ToString().Trim()));
                                    Cmb_Fuente_Financiamiento.SelectedIndex = Cmb_Fuente_Financiamiento.Items.IndexOf(Cmb_Fuente_Financiamiento.Items.FindByValue(Dt_Partidas_Asignadas.Rows[No_Fila]["FUENTE_FINANCIAMIENTO_ID"].ToString().Trim()));
                                    Llenar_Combo_Capitulos();
                                    LLenar_Presupuesto_Dependencia();
                                    Cmb_Capitulos.SelectedIndex = Cmb_Capitulos.Items.IndexOf(Cmb_Capitulos.Items.FindByValue(Dt_Partidas_Asignadas.Rows[No_Fila]["CAPITULO_ID"].ToString().Trim()));
                                    Llenar_Combo_Partidas();
                                    Cmb_Partida_Especifica.SelectedIndex = Cmb_Partida_Especifica.Items.IndexOf(Cmb_Partida_Especifica.Items.FindByValue(Dt_Partidas_Asignadas.Rows[No_Fila]["PARTIDA_ID"].ToString().Trim()));
                                    Llenar_Combo_Productos();
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
        #endregion

        #region EVENTOS COMBOS
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Cmb_Unidad_Responsable_SelectedIndexChanged
        ///DESCRIPCIÓN          : Evento del combo de unidad responsable
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 10/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///******************************************************************************* 
        protected void Cmb_Unidad_Responsable_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Limpiar_Sessiones();
                Limpiar_Formulario("Datos_Partida");
                Mostrar_Ocultar_Error(false);
                Llenar_Combo_Programa();
                Llenar_Combo_Fuente_Financiamiento();
                Llenar_Combo_Capitulos();
                Cmb_Capitulos.Enabled = true;
                Cmb_Partida_Especifica.SelectedIndex = -1;
                Cmb_Partida_Especifica.Enabled = false;
                Cmb_Producto.SelectedIndex = -1;
                Tr_Productos.Visible = false;
                Cmb_Fuente_Financiamiento.Enabled = true;
                Cmb_Programa.Enabled = true;
                Llenar_Grid_Partida_Asignada();
                Calcular_Total_Presupuestado();
                LLenar_Presupuesto_Dependencia();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en el evento del combo de unidad responsable Error[" + ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Cmb_Capitulo_SelectedIndexChanged
        ///DESCRIPCIÓN          : Evento del combo de Capitulos
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 10/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///******************************************************************************* 
        protected void Cmb_Capitulo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Mostrar_Ocultar_Error(false);
                Llenar_Combo_Partidas();
                Cmb_Partida_Especifica.Enabled = true;
                Cmb_Producto.SelectedIndex = -1;
                Tr_Productos.Visible = false;
                Limpiar_Formulario("Datos_Partida");
            }
            catch (Exception ex)
            {
                throw new Exception("Error en el evento del combo de capitulos Error[" + ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Cmb_Partidas_SelectedIndexChanged
        ///DESCRIPCIÓN          : Evento del combo de partidas
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 11/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///******************************************************************************* 
        protected void Cmb_Partidas_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Mostrar_Ocultar_Error(false);
                Llenar_Combo_Productos();
                Cmb_Producto.Enabled = true;
                Limpiar_Formulario("Datos_Partida");
            }
            catch (Exception ex)
            {
                throw new Exception("Error en el evento del combo de partidas Error[" + ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Cmb_Productos_SelectedIndexChanged
        ///DESCRIPCIÓN          : Evento del combo de productos
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 12/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///******************************************************************************* 
        protected void Cmb_Productos_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cls_Ope_Psp_Calendarizar_Presupuesto_Negocio Negocio = new Cls_Ope_Psp_Calendarizar_Presupuesto_Negocio();
            DataTable Dt_Productos = new DataTable();
            try
            {
                Mostrar_Ocultar_Error(false);
                Limpiar_Formulario("Datos_Producto");
                Grid_Partida_Asignada.SelectedIndex = -1;
                Negocio.P_Partida_ID = Cmb_Partida_Especifica.SelectedItem.Value.Trim();
                Negocio.P_Producto_ID = Cmb_Producto.SelectedItem.Value.Trim();
                Dt_Productos = Negocio.Consultar_Productos();

                if (Dt_Productos != null)
                {
                    if (Dt_Productos.Rows.Count > 0)
                    {
                        Hf_Precio.Value = Dt_Productos.Rows[0]["COSTO"].ToString().Trim();
                        Hf_Producto_ID.Value = Dt_Productos.Rows[0]["PRODUCTO_ID"].ToString().Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en el evento del combo de productos Error[" + ex.Message + "]");
            }
        }
    #endregion

    #endregion

    #region MODAL POPUP
        //*******************************************************************************
        //NOMBRE DE LA FUNCIÓN : Grid_Productos_SelectedIndexChanged
        //DESCRIPCIÓN          : Evento de seleccion de un registro del del grid
        //PARAMETROS           :   
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 15/Noviembre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //*******************************************************************************
        protected void Grid_Productos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Mostrar_Ocultar_Error(false);
                Hf_Producto_ID.Value = HttpUtility.HtmlDecode(Grid_Productos.SelectedRow.Cells[1].Text).Trim();
                Hf_Precio.Value = HttpUtility.HtmlDecode(Grid_Productos.SelectedRow.Cells[4].Text).Trim();
                Cmb_Producto.SelectedIndex = Cmb_Producto.Items.IndexOf(Cmb_Producto.Items.FindByValue(Hf_Producto_ID.Value.Trim()));
                Mpe_Busqueda_Productos.Hide();
                Lbl_Error_Busqueda.Text = "";
                Lbl_Error_Busqueda.Style.Add("display", "none");
                Img_Error_Busqueda.Style.Add("display", "none");
                Txt_Busqueda_Nombre_Producto.Text = "";
                Txt_Busqueda_Clave.Text = "";
                Lbl_Numero_Registros.Text = "";
                Grid_Productos.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en el evento de seleccionar un registro de la tabla de productos Error[" + ex.Message + "]");
            }
        }

        //*******************************************************************************
        //NOMBRE DE LA FUNCIÓN : Btn_Busqueda_Productos_Click
        //DESCRIPCIÓN          : Evento del boton de busquedas de productos
        //PARAMETROS           :   
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 15/Noviembre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //*******************************************************************************
        protected void Btn_Busqueda_Productos_Click(object sender, EventArgs e)
        {
            Cls_Ope_Psp_Calendarizar_Presupuesto_Negocio Negocio = new Cls_Ope_Psp_Calendarizar_Presupuesto_Negocio(); //Variable de conexión hacia la capa de Negocios
            DataTable Dt_Productos; //Variable que obtendra los datos de la consulta 
            try
            {

                if (Cmb_Partida_Especifica.SelectedIndex > 0)
                {
                    if (!String.IsNullOrEmpty(Txt_Busqueda_Clave.Text.Trim()))
                    {
                        Negocio.P_Clave_Producto = Txt_Busqueda_Clave.Text.Trim();
                    }
                    if (!String.IsNullOrEmpty(Txt_Busqueda_Nombre_Producto.Text.Trim()))
                    {
                        Negocio.P_Nombre_Producto = Txt_Busqueda_Nombre_Producto.Text.Trim();
                    }

                    Negocio.P_Partida_ID = Cmb_Partida_Especifica.SelectedValue.Trim();
                    Dt_Productos = Negocio.Consultar_Productos();
                    Llenar_Grid_Productos(Dt_Productos);
                    Mpe_Busqueda_Productos.Show();
                    Lbl_Error_Busqueda.Style.Add("display", "none");
                    Img_Error_Busqueda.Style.Add("display", "none");

                    if (Dt_Productos is DataTable)
                        Lbl_Numero_Registros.Text = "Registros Encontrados: [" + Dt_Productos.Rows.Count + "]";
                    else
                        Lbl_Numero_Registros.Text = "Registros Encontrados: [0]";
                }
                else
                {
                    Lbl_Error_Busqueda.Text = "Favor de seleccionar una partida";
                    Lbl_Error_Busqueda.Style.Add("display", "block");
                    Img_Error_Busqueda.Style.Add("display", "block");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Consulta_Productos " + ex.Message.ToString(), ex);
            }
        }

        //*******************************************************************************
        //NOMBRE DE LA FUNCIÓN : Llenar_Grid_Productos
        //DESCRIPCIÓN          : Metodo para llenar el grid del modal popup de productos
        //PARAMETROS           1 Dt_Productos datatable que contendra los productos que mostraremos
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 15/Noviembre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //*******************************************************************************
        protected void Llenar_Grid_Productos(DataTable Dt_Productos)
        {
            try
            {
                Grid_Productos.DataBind();
                if (Dt_Productos != null)
                {
                    if (Dt_Productos.Rows.Count > 0)
                    {
                        Grid_Productos.Columns[1].Visible = true;
                        Grid_Productos.Columns[4].Visible = true;
                        Grid_Productos.DataSource = Dt_Productos;
                        Grid_Productos.DataBind();
                        Grid_Productos.Columns[1].Visible = false;
                        Grid_Productos.Columns[4].Visible = false;
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error al llenar el grid de productos Error[" + ex.Message + "]");
            }
        }
    #endregion
}
