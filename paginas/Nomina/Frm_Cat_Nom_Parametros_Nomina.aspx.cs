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
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Cat_Parametros_Nomina.Negocio;
using Presidencia.Zona_Economica.Negocios;
using System.Text.RegularExpressions;
using System.Globalization;
using Presidencia.Cat_Nom_Percepciones_Deducciones_Opcional.Negocios;
using System.Collections.Generic;
using Presidencia.Proveedores.Negocios;

public partial class paginas_Nomina_Frm_Cat_Nom_Parametros_Nomina : System.Web.UI.Page
{

    #region (Load)
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Configuracion_Inicial();
        }
        Lbl_Mensaje_Error.Text = "";
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
    }
    #endregion

    #region (Metodos)

    #region (Metodos Generales)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Inicial
    ///DESCRIPCIÓN: Configuracion Inicial del Catalogo de Parametros
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 8/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Inicial()
    {
        Consultar_Zonas();
        Consultar_Proveedores_Nomina();

        Consultar_Percepciones_Deducciones(Cmb_Percepcion_Quinquenio, "PERCEPCION", "OPERACION");
        Consultar_Percepciones_Deducciones(Cmb_Percepcion_Prima_Vacacional, "PERCEPCION", "OPERACION");
        Consultar_Percepciones_Deducciones(Cmb_Percepcion_Prima_Dominical, "PERCEPCION", "OPERACION");
        Consultar_Percepciones_Deducciones(Cmb_Percepcion_Aguinaldo, "PERCEPCION", "OPERACION");
        Consultar_Percepciones_Deducciones(Cmb_Percepcion_Dias_Festivos, "PERCEPCION", "OPERACION");
        Consultar_Percepciones_Deducciones(Cmb_Percepcion_Horas_Extra, "PERCEPCION", "OPERACION");
        Consultar_Percepciones_Deducciones(Cmb_Percepcion_Dia_Doble, "PERCEPCION", "OPERACION");
        Consultar_Percepciones_Deducciones(Cmb_Percepcion_Dia_Domingo, "PERCEPCION", "OPERACION");
        Consultar_Percepciones_Deducciones(Cmb_Percepcion_Ajuste_ISR, "PERCEPCION", "OPERACION");
        Consultar_Percepciones_Deducciones(Cmb_Percepcion_Incapacidades, "PERCEPCION", "OPERACION");
        Consultar_Percepciones_Deducciones(Cmb_Percepcion_Subsidio, "PERCEPCION", "OPERACION");
        Consultar_Percepciones_Deducciones(Cmb_Percepcion_Despensa, "PERCEPCION", "FIJA");
        Consultar_Percepciones_Deducciones(Cmb_Percepcion_Sueldo_Normal, "PERCEPCION", "OPERACION");
        Consultar_Percepciones_Deducciones(Cmb_Percepcion_Prima_Antiguedad, "PERCEPCION", "OPERACION");
        Consultar_Percepciones_Deducciones(Cmb_Percepcion_Indemnizacion, "PERCEPCION", "OPERACION");
        Consultar_Percepciones_Deducciones(Cmb_Percepcion_Vacaciones_Pendientes_Pagar, "PERCEPCION", "OPERACION");
        Consultar_Percepciones_Deducciones(Cmb_Percepcion_Vacaciones, "PERCEPCION", "OPERACION");
        Consultar_Percepciones_Deducciones(Cmb_Percepcion_Fondo_Retiro, "PERCEPCION", "OPERACION");
        Consultar_Percepciones_Deducciones(Cmb_Percepcion_PSM, "PERCEPCION", "OPERACION");
        
        Consultar_Percepciones_Deducciones(Cmb_Deduccion_Faltas, "DEDUCCION", "OPERACION");
        Consultar_Percepciones_Deducciones(Cmb_Deduccion_Retardos, "DEDUCCION", "OPERACION");
        Consultar_Percepciones_Deducciones(Cmb_Deduccion_Fondo_Retiro, "DEDUCCION", "OPERACION");
        Consultar_Percepciones_Deducciones(Cmb_Deduccion_ISR, "DEDUCCION", "OPERACION");
        Consultar_Percepciones_Deducciones(Cmb_Deduccion_Orden_Judicial, "DEDUCCION", "OPERACION");
        Consultar_Percepciones_Deducciones(Cmb_Deduccion_IMSS, "DEDUCCION", "OPERACION");
        Consultar_Percepciones_Deducciones(Cmb_Deduccion_Vacaciones_Tomadas_Mas, "DEDUCCION", "OPERACION");
        Consultar_Percepciones_Deducciones(Cmb_Deduccion_Aguinaldo_Pagado_Mas, "DEDUCCION", "OPERACION");
        Consultar_Percepciones_Deducciones(Cmb_Deduccion_PV_Pagada_Mas, "DEDUCCION", "OPERACION");
        Consultar_Percepciones_Deducciones(Cmb_Deduccion_Sueldo_Pagado_Mas, "DEDUCCION", "OPERACION");
        Consultar_Percepciones_Deducciones(Cmb_Deduccion_Orden_Judicial_Aguinaldo, "DEDUCCION", "OPERACION");
        Consultar_Percepciones_Deducciones(Cmb_Deduccion_Orden_Judicial_Prima_Vacacional, "DEDUCCION", "OPERACION");
        Consultar_Percepciones_Deducciones(Cmb_Deduccion_Orden_Judicial_Indemnizacion, "DEDUCCION", "OPERACION");
        Consultar_Percepciones_Deducciones(Cmb_Deduccion_ISSEG, "DEDUCCION", "OPERACION");

        Habilitar_Controles("Inicial");
        Consultar_Informacion_Parametros();

        if (String.IsNullOrEmpty(Txt_Parametro_ID.Text.Trim()))
        {
            Btn_Nuevo.Enabled = true;
            Configuracion_Acceso("Frm_Cat_Nom_Parametros_Nomina.aspx");
            Btn_Modificar.Enabled = false;
        }
        else
        {
            Btn_Modificar.Enabled = true;
            Configuracion_Acceso("Frm_Cat_Nom_Parametros_Nomina.aspx");
            Btn_Nuevo.Enabled = false;
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Ctlr
    /// DESCRIPCION : Limpia los Controles de la pagina.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 8/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Ctlrs()
    {
        Txt_Parametro_ID.Text = "";
        Cmb_Zona.SelectedIndex = -1;
        Txt_Porcentaje_Prima_Vacacional.Text = "";
        Txt_Porcentaje_Fondo_Retiro.Text = "";
        Txt_Porcentaje_Prima_Dominical.Text = "";
        Txt_Fecha_Prima_Vacacional_1.Text = "";
        Txt_Fecha_Prima_Vacacional_2.Text = "";
        Txt_Salario_Limite_Prestamo.Text = "";
        Txt_Salario_Mensual_Maximo.Text = "";
        Txt_Salario_Diario_Integrado_Topado.Text = "";

        Cmb_Percepcion_Quinquenio.SelectedIndex = -1;
        Cmb_Percepcion_Prima_Vacacional.SelectedIndex = -1;
        Cmb_Percepcion_Prima_Dominical.SelectedIndex = -1;
        Cmb_Percepcion_Aguinaldo.SelectedIndex = -1;
        Cmb_Percepcion_Dias_Festivos.SelectedIndex = -1;
        Cmb_Percepcion_Horas_Extra.SelectedIndex = -1;
        Cmb_Percepcion_Dia_Doble.SelectedIndex = -1;
        Cmb_Percepcion_Dia_Domingo.SelectedIndex = -1;
        Cmb_Percepcion_Ajuste_ISR.SelectedIndex = -1;
        Cmb_Percepcion_Incapacidades.SelectedIndex = -1;
        Cmb_Percepcion_Subsidio.SelectedIndex = -1;
        Cmb_Percepcion_Despensa.SelectedIndex = -1;
        Cmb_Percepcion_Sueldo_Normal.SelectedIndex = -1;
        Cmb_Percepcion_Prima_Antiguedad.SelectedIndex = -1;
        Cmb_Percepcion_Indemnizacion.SelectedIndex = -1;
        Cmb_Percepcion_Vacaciones_Pendientes_Pagar.SelectedIndex = -1;
        Cmb_Percepcion_Vacaciones.SelectedIndex = -1;
        Cmb_Percepcion_Fondo_Retiro.SelectedIndex = -1;
        Cmb_Percepcion_PSM.SelectedIndex = -1;

        Cmb_Deduccion_Faltas.SelectedIndex = -1;
        Cmb_Deduccion_Retardos.SelectedIndex = -1;
        Cmb_Deduccion_Fondo_Retiro.SelectedIndex = -1;
        Cmb_Deduccion_ISR.SelectedIndex = -1;
        Cmb_Deduccion_Orden_Judicial.SelectedIndex = -1;
        Cmb_Deduccion_IMSS.SelectedIndex = -1;
        Cmb_Deduccion_Vacaciones_Tomadas_Mas.SelectedIndex = -1;
        Cmb_Deduccion_Aguinaldo_Pagado_Mas.SelectedIndex = -1;
        Cmb_Deduccion_PV_Pagada_Mas.SelectedIndex = -1;
        Cmb_Deduccion_Sueldo_Pagado_Mas.SelectedIndex = -1;
        Cmb_Deduccion_Orden_Judicial_Aguinaldo.SelectedIndex = -1;
        Cmb_Deduccion_Orden_Judicial_Prima_Vacacional.SelectedIndex = -1;
        Cmb_Deduccion_Orden_Judicial_Indemnizacion.SelectedIndex = -1;
        Cmb_Deduccion_ISSEG.SelectedIndex = -1;

        Txt_IP_Servidor_Reloj_Checador.Text = "";
        Txt_Nombre_Base_Datos_Reloj_Checador.Text = "";
        Txt_Usuario_Base_Datos_Reloj_Checador.Text = "";
        Txt_Password_Base_Datos_Reloj_Checador.Text = "";

        Cmb_Tipo_Calculo_IMSS.SelectedIndex = -1;
        Txt_Minutos_Dia.Text = String.Empty;
        Txt_Minutos_Retardo.Text = String.Empty;
        Txt_Porcentaje_Prevision_Social_Multiple.Text = String.Empty;
        Txt_Porcentaje_Factor_Social.Text = String.Empty;

        Txt_Dias_IMSS.Text = String.Empty;
        Txt_Tope_ISSEG.Text = String.Empty;
        Cmb_Proveedor_Fonacot.SelectedIndex = -1;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///               para a siguiente operación
    /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
    ///                          si es una alta, modificacion
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 8/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado;
        try
        {
            Habilitado = false;
            switch (Operacion)
            {
                case "Inicial":
                    Habilitado = false;
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = true;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Lbl_Mensaje_Error.Text = "";
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    Tc_Parametros.ActiveTabIndex = 0;
                    break;
                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Eliminar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    break;
                case "Modificar":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    break;
            }
            Txt_Parametro_ID.Enabled = false;
            Cmb_Zona.Enabled = Habilitado;
            Txt_Porcentaje_Prima_Vacacional.Enabled = Habilitado;
            Txt_Porcentaje_Fondo_Retiro.Enabled = Habilitado;
            Txt_Porcentaje_Prima_Dominical.Enabled = Habilitado;
            Txt_Fecha_Prima_Vacacional_1.Enabled = Habilitado;
            Txt_Fecha_Prima_Vacacional_2.Enabled = Habilitado;
            Txt_Salario_Limite_Prestamo.Enabled = Habilitado;
            Txt_Salario_Mensual_Maximo.Enabled = Habilitado;
            Txt_Salario_Diario_Integrado_Topado.Enabled = Habilitado;

            Cmb_Percepcion_Quinquenio.Enabled = Habilitado;
            Cmb_Percepcion_Prima_Vacacional.Enabled = Habilitado;
            Cmb_Percepcion_Prima_Dominical.Enabled = Habilitado;
            Cmb_Percepcion_Aguinaldo.Enabled = Habilitado;
            Cmb_Percepcion_Dias_Festivos.Enabled = Habilitado;
            Cmb_Percepcion_Horas_Extra.Enabled = Habilitado;
            Cmb_Percepcion_Dia_Doble.Enabled = Habilitado;
            Cmb_Percepcion_Dia_Domingo.Enabled = Habilitado;
            Cmb_Percepcion_Ajuste_ISR.Enabled = Habilitado;
            Cmb_Percepcion_Incapacidades.Enabled = Habilitado;
            Cmb_Percepcion_Subsidio.Enabled = Habilitado;
            Cmb_Percepcion_Despensa.Enabled = Habilitado;
            Cmb_Percepcion_Sueldo_Normal.Enabled = Habilitado;
            Cmb_Percepcion_Indemnizacion.Enabled = Habilitado;
            Cmb_Percepcion_Prima_Antiguedad.Enabled = Habilitado;
            Cmb_Percepcion_Vacaciones_Pendientes_Pagar.Enabled = Habilitado;
            Cmb_Percepcion_Vacaciones.Enabled = Habilitado;
            Cmb_Percepcion_PSM.Enabled = Habilitado;
            Cmb_Percepcion_Fondo_Retiro.Enabled = Habilitado;

            Cmb_Deduccion_Faltas.Enabled = Habilitado;
            Cmb_Deduccion_Retardos.Enabled = Habilitado;
            Cmb_Deduccion_Fondo_Retiro.Enabled = Habilitado;
            Cmb_Deduccion_ISR.Enabled = Habilitado;
            Cmb_Deduccion_Orden_Judicial.Enabled = Habilitado;
            Cmb_Deduccion_IMSS.Enabled = Habilitado;
            Cmb_Deduccion_Vacaciones_Tomadas_Mas.Enabled = Habilitado;
            Cmb_Deduccion_Aguinaldo_Pagado_Mas.Enabled = Habilitado;
            Cmb_Deduccion_PV_Pagada_Mas.Enabled = Habilitado;
            Cmb_Deduccion_Sueldo_Pagado_Mas.Enabled = Habilitado;
            Cmb_Deduccion_Orden_Judicial_Aguinaldo.Enabled = Habilitado;
            Cmb_Deduccion_Orden_Judicial_Prima_Vacacional.Enabled = Habilitado;
            Cmb_Deduccion_Orden_Judicial_Indemnizacion.Enabled = Habilitado;
            Cmb_Deduccion_ISSEG.Enabled = Habilitado;

            Btn_Txt_Fecha_Prima_Vacacional_1.Enabled = Habilitado;
            Btn_Txt_Fecha_Prima_Vacacional_2.Enabled = Habilitado;

            Txt_IP_Servidor_Reloj_Checador.Enabled = Habilitado;
            Txt_Nombre_Base_Datos_Reloj_Checador.Enabled = Habilitado;
            Txt_Usuario_Base_Datos_Reloj_Checador.Enabled = Habilitado;
            Txt_Password_Base_Datos_Reloj_Checador.Enabled = Habilitado;

            Cmb_Tipo_Calculo_IMSS.Enabled = Habilitado;
            Txt_Minutos_Dia.Enabled = Habilitado;
            Txt_Minutos_Retardo.Enabled = Habilitado;
            Txt_Porcentaje_Prevision_Social_Multiple.Enabled = Habilitado;
            Txt_Porcentaje_Factor_Social.Enabled = Habilitado;

            Txt_Dias_IMSS.Enabled = Habilitado;
            Txt_Tope_ISSEG.Enabled = Habilitado;
            Cmb_Proveedor_Fonacot.Enabled = Habilitado;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Juntar_Clave_Nombre
    /// DESCRIPCION : Junta el nombre del concepto con la clave.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected DataTable Juntar_Clave_Nombre(DataTable Dt_Conceptos)
    {
        try
        {
            if (Dt_Conceptos is DataTable)
            {
                if (Dt_Conceptos.Rows.Count > 0)
                {
                    foreach (DataRow CONCEPTO in Dt_Conceptos.Rows)
                    {
                        if (CONCEPTO is DataRow)
                        {
                            CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Nombre] =
                                "[" + CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Clave].ToString().Trim() + "] -- " +
                                    CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Nombre].ToString().Trim();
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al unir el nombre con la clave del concepto. Error: [" + Ex.Message + "]");
        }
        return Dt_Conceptos;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos_Parametros
    /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 8/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos_Parametros()
    {
        Boolean Datos_Validos = true;
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

        if (String.IsNullOrEmpty(Txt_Tope_ISSEG.Text)) {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Tope ISSEG. <br>";
            Datos_Validos = false;            
        }

        if (Cmb_Zona.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Zona <br>";
            Datos_Validos = false;
        }
        if (string.IsNullOrEmpty(Txt_Porcentaje_Prima_Vacacional.Text))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Porcentaje Prima Vacacional <br>";
            Datos_Validos = false;
        }
        else if ( !(Convert.ToDouble(Txt_Porcentaje_Prima_Vacacional.Text) >= 0 && Convert.ToDouble(Txt_Porcentaje_Prima_Vacacional.Text) <= 100) )
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Porcentaje Prima Vacacional [0 - 100] <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Porcentaje_Fondo_Retiro.Text))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Porcentaje Fondo de Retiro <br>";
            Datos_Validos = false;
        }
        else if (!(Convert.ToDouble(Txt_Porcentaje_Fondo_Retiro.Text) >= 0 && Convert.ToDouble(Txt_Porcentaje_Fondo_Retiro.Text) <= 100))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Porcentaje Fondo de Retiro [0 - 100] <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Porcentaje_Prima_Dominical.Text))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Porcentaje Prima Dominical <br>";
            Datos_Validos = false;
        }
        else if (!(Convert.ToDouble(Txt_Porcentaje_Prima_Dominical.Text) >= 0 && Convert.ToDouble(Txt_Porcentaje_Prima_Dominical.Text) <= 100))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Porcentaje Prima Dominical [0 - 100] <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Fecha_Prima_Vacacional_1.Text))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Fecha Prima Vacacional 1<br>";
            Datos_Validos = false;
        }
        else if (Validar_Formato_Fecha(Txt_Fecha_Prima_Vacacional_1.Text.Trim()) && Validar_Formato_Fecha(Txt_Fecha_Prima_Vacacional_2.Text.Trim()))
        {
            if (!Validar_Fecha(Convert.ToDateTime(Txt_Fecha_Prima_Vacacional_1.Text), Convert.ToDateTime(Txt_Fecha_Prima_Vacacional_2.Text), 1))
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La Fecha para el Pago de la Prima Vacacional 1 [Enero - Junio] <br>";
                Datos_Validos = false;
            }
        }
        else {
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Fecha_Prima_Vacacional_2.Text))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Fecha Prima Vacacional 2 <br>";
            Datos_Validos = false;
        }
        else if (Validar_Formato_Fecha(Txt_Fecha_Prima_Vacacional_1.Text.Trim()) && Validar_Formato_Fecha(Txt_Fecha_Prima_Vacacional_2.Text.Trim()))
        {
            if (!Validar_Fecha(Convert.ToDateTime(Txt_Fecha_Prima_Vacacional_1.Text), Convert.ToDateTime(Txt_Fecha_Prima_Vacacional_2.Text), 2))
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La Fecha para el Pago de la Prima Vacacional 2 [Julio - Diciembre] <br>";
                Datos_Validos = false;
            }
        }
        else {
            if (!Validar_Formato_Fecha(Txt_Fecha_Prima_Vacacional_1.Text.Trim()))
            {
                Txt_Fecha_Prima_Vacacional_1.Text = "";
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Fecha Prima Vacacional 1 <br>";
            }
            if (!Validar_Formato_Fecha(Txt_Fecha_Prima_Vacacional_2.Text.Trim()))
            {
                Txt_Fecha_Prima_Vacacional_2.Text = "";
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Fecha Prima Vacacional 2 <br>";
            }
            Datos_Validos = false;
        }

        if (Cmb_Percepcion_Quinquenio.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Quinquenio <br>";
            Datos_Validos = false;
        }
        if (Cmb_Percepcion_Prima_Vacacional.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Prima Vacacional <br>";
            Datos_Validos = false;
        }
        if (Cmb_Percepcion_Prima_Dominical.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Prima Dominical <br>";
            Datos_Validos = false;
        }
        if (Cmb_Percepcion_Aguinaldo.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Aguinaldo <br>";
            Datos_Validos = false;
        }
        if (Cmb_Percepcion_Dias_Festivos.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Dias Festivos <br>";
            Datos_Validos = false;
        }
        if (Cmb_Percepcion_Horas_Extra.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Horas Extra <br>";
            Datos_Validos = false;
        }
        if (Cmb_Percepcion_Dia_Doble.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Dia Doble <br>";
            Datos_Validos = false;
        }
        //if (Cmb_Percepcion_Dia_Domingo.SelectedIndex <= 0)
        //{
        //    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Dia Domingo <br>";
        //    Datos_Validos = false;
        //}
        if (Cmb_Percepcion_Ajuste_ISR.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Ajuste de ISR <br>";
            Datos_Validos = false;
        }
        if (Cmb_Percepcion_Incapacidades.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Incapacidades <br>";
            Datos_Validos = false;
        }
        if (Cmb_Percepcion_Subsidio.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Subsidio <br>";
            Datos_Validos = false;
        }
        if (Cmb_Percepcion_Despensa.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Despensa <br>";
            Datos_Validos = false;
        }
        if (Cmb_Percepcion_Sueldo_Normal.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Sueldo Normal <br>";
            Datos_Validos = false;
        }
        if (Cmb_Percepcion_Prima_Antiguedad.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Prima Antiguedad <br>";
            Datos_Validos = false;
        }

        //if (Cmb_Percepcion_Indemnizacion.SelectedIndex <= 0)
        //{
        //    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Indemnización <br>";
        //    Datos_Validos = false;
        //}

        //if (Cmb_Percepcion_Vacaciones_Pendientes_Pagar.SelectedIndex <= 0)
        //{
        //    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Vacaciones pendientes por tomar<br>";
        //    Datos_Validos = false;
        //}

        if (Cmb_Percepcion_Vacaciones.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Vacaciones <br>";
            Datos_Validos = false;
        }

        if (Cmb_Percepcion_PSM.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Previsión Social Multiple <br>";
            Datos_Validos = false;
        }

        if (Cmb_Percepcion_Fondo_Retiro.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Fondo de Retiro <br>";
            Datos_Validos = false;
        }

        if (Cmb_Deduccion_Faltas.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Faltas <br>";
            Datos_Validos = false;
        }
        if (Cmb_Deduccion_Retardos.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Retardos <br>";
            Datos_Validos = false;
        }
        if (Cmb_Deduccion_Fondo_Retiro.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Retención a Terceros <br>";
            Datos_Validos = false;
        }
        if (Cmb_Deduccion_ISR.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + ISR <br>";
            Datos_Validos = false;
        }

        if (Cmb_Deduccion_Orden_Judicial.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Orden Judicial <br>";
            Datos_Validos = false;
        }

        if (Cmb_Deduccion_IMSS.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + IMSS <br>";
            Datos_Validos = false;
        }

        //if (Cmb_Deduccion_Vacaciones_Tomadas_Mas.SelectedIndex <= 0)
        //{
        //    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Vacaciones Tomadas Mas <br>";
        //    Datos_Validos = false;
        //}

        //if (Cmb_Deduccion_Aguinaldo_Pagado_Mas.SelectedIndex <= 0)
        //{
        //    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Aguinaldo Pagado de Más <br>";
        //    Datos_Validos = false;
        //}

        //if (Cmb_Deduccion_PV_Pagada_Mas.SelectedIndex <= 0)
        //{
        //    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Prima Vacacional Pagada de Más <br>";
        //    Datos_Validos = false;
        //}

        //if (Cmb_Deduccion_Sueldo_Pagado_Mas.SelectedIndex <= 0)
        //{
        //    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Sueldo pagado de Más <br>";
        //    Datos_Validos = false;
        //}

        //if (Cmb_Deduccion_Orden_Judicial_Aguinaldo.SelectedIndex <= 0)
        //{
        //    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Orden Judicial Aguinaldo <br>";
        //    Datos_Validos = false;
        //}

        //if (Cmb_Deduccion_Orden_Judicial_Prima_Vacacional.SelectedIndex <= 0)
        //{
        //    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Orden Judicial Prima Vacacional.<br>";
        //    Datos_Validos = false;
        //}

        //if (Cmb_Deduccion_Orden_Judicial_Indemnizacion.SelectedIndex <= 0)
        //{
        //    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Orden Judicial Indemnización.<br>";
        //    Datos_Validos = false;
        //}

        if (Cmb_Deduccion_ISSEG.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + ISSEG.<br>";
            Datos_Validos = false;
        }

        if (String.IsNullOrEmpty(Txt_Salario_Limite_Prestamo.Text.Trim()))
        {
            if (Txt_Salario_Limite_Prestamo.Text.Contains("$"))
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Salario Limite Prestamo <br>";
                Datos_Validos = false;
            }
            else
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Salario Limite Prestamo <br>";
                Datos_Validos = false;
            }
        }
        if (String.IsNullOrEmpty(Txt_Salario_Mensual_Maximo.Text.Trim()))
        {
            if (Txt_Salario_Mensual_Maximo.Text.Contains("$"))
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Salario Mensual Máximo <br>";
                Datos_Validos = false;
            }
            else
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Salario Mensual Máximo <br>";
                Datos_Validos = false;
            }
        }
        if (String.IsNullOrEmpty(Txt_Salario_Diario_Integrado_Topado.Text.Trim()))
        {
            if (Txt_Salario_Diario_Integrado_Topado.Text.Contains("$"))
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Salario Diario Integrado Topado <br>";
                Datos_Validos = false;
            }
            else
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Salarioo Diario Integrado Topado <br>";
                Datos_Validos = false;
            }
        }

        if (String.IsNullOrEmpty(Txt_IP_Servidor_Reloj_Checador.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + IP del Servidor de la Base de Datos del Reloj Checador <br>";
            Datos_Validos = false;
        }
        if (String.IsNullOrEmpty(Txt_Nombre_Base_Datos_Reloj_Checador.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Nombre de la Base de Datos del Reloj Checador <br>";
            Datos_Validos = false;
        }
        if (String.IsNullOrEmpty(Txt_Usuario_Base_Datos_Reloj_Checador.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Usuario para el acceso a la Base de Datos del Reloj Checador <br>";
            Datos_Validos = false;
        }
        if (String.IsNullOrEmpty(Txt_Password_Base_Datos_Reloj_Checador.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Password para el acceso a la Base de Datos del Reloj Checador <br>";
            Datos_Validos = false;
        }

        if (Cmb_Tipo_Calculo_IMSS.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Tipo de Calculo de IMSS <br>";
            Datos_Validos = false;
        }

        if (String.IsNullOrEmpty(Txt_Minutos_Dia.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Minutos Dia <br>";
            Datos_Validos = false;
        }

        if (String.IsNullOrEmpty(Txt_Minutos_Retardo.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Minutos Dia <br>";
            Datos_Validos = false;
        }

        if (String.IsNullOrEmpty(Txt_Porcentaje_Prevision_Social_Multiple.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + [%] Prevision Social Multiple <br>";
            Datos_Validos = false;
        }

        if (String.IsNullOrEmpty(Txt_Porcentaje_Factor_Social.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + [%] Factor Social <br>";
            Datos_Validos = false;
        }

        if (Cmb_Proveedor_Fonacot.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Proveedor Fonacot <br>";
            Datos_Validos = false;
        }

        return Datos_Validos;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Fecha
    /// DESCRIPCION : Validar que las fechas correspondan al periodo a validar
    /// [Enero - Junio] o [Julio - Agosto]
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 8/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Fecha(DateTime Date_1, DateTime Date_2, int Periodo) {
        Boolean Fecha_Valida = false;
        try
        {
            int anio_1 = Date_1.Year;
            int anio_2 =Date_2.Year;
            int mes_1 = Date_1.Month;
            int mes_2 = Date_2.Month;

            if (anio_1 == anio_2)
            {
                switch (Periodo)
                {
                    case 1:
                        if (mes_1 >= 1 && mes_1 <= 6) Fecha_Valida = true;
                        break;
                    case 2:
                        if (mes_2 >= 7 && mes_2 <= 12) Fecha_Valida = true;
                        break;
                    default: break;
                }
            }
            return Fecha_Valida;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al Validar la Fechas. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Formato_Fecha
    /// DESCRIPCION : Valida el formato de las fechas.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 23/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Formato_Fecha(String Fecha)
    {
        String Cadena_Fecha = @"^(([0-9])|([0-2][0-9])|([3][0-1]))\/(ene|feb|mar|abr|may|jun|jul|ago|sep|oct|nov|dic)\/\d{4}$";
        if (Fecha != null) return Regex.IsMatch(Fecha, Cadena_Fecha);
        else return false;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Informacion_Parametros
    ///DESCRIPCIÓN: Consulta la información del parámetro seleccionado.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 11/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Informacion_Parametros()
    {
        Cls_Cat_Nom_Parametros_Negocio Cls_Parametros_Nomina = new Cls_Cat_Nom_Parametros_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Parametro_consultado = null;//Variable que almacenara el parametro consultado.

        try
        {
            Dt_Parametro_consultado = Cls_Parametros_Nomina.Consulta_Parametros();
            Cargar_Grid_Parametro(Dt_Parametro_consultado);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error generado al consultar la información del parámetro de la nómina. error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Grid_Parametro
    ///DESCRIPCIÓN: Carga los controles de la pagina con la información consultada.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 11/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Grid_Parametro(DataTable Dt_Parametro)
    {
        try
        {
            if (Dt_Parametro is DataTable)
            {
                foreach (DataRow Renglon in Dt_Parametro.Rows)
                {
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Parametro_ID].ToString())) 
                        Txt_Parametro_ID.Text = Renglon[Cat_Nom_Parametros.Campo_Parametro_ID].ToString();
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Zona_ID].ToString())) 
                        Cmb_Zona.SelectedIndex = Cmb_Zona.Items.IndexOf(Cmb_Zona.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Parametro_ID].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Porcentaje_Prima_Vacacional].ToString()))
                        Txt_Porcentaje_Prima_Vacacional.Text = Renglon[Cat_Nom_Parametros.Campo_Porcentaje_Prima_Vacacional].ToString();
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Porcentaje_Prima_Dominical].ToString())) 
                        Txt_Porcentaje_Prima_Dominical.Text = Renglon[Cat_Nom_Parametros.Campo_Porcentaje_Prima_Dominical].ToString();
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Porcentaje_Fondo_Retiro].ToString())) 
                        Txt_Porcentaje_Fondo_Retiro.Text = Renglon[Cat_Nom_Parametros.Campo_Porcentaje_Fondo_Retiro].ToString();
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Fecha_Prima_Vacacional_1].ToString())) 
                        Txt_Fecha_Prima_Vacacional_1.Text = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Renglon[Cat_Nom_Parametros.Campo_Fecha_Prima_Vacacional_1].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Fecha_Prima_Vacacional_2].ToString())) 
                        Txt_Fecha_Prima_Vacacional_2.Text = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Renglon[Cat_Nom_Parametros.Campo_Fecha_Prima_Vacacional_2].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Quinquenio].ToString())) 
                        Cmb_Percepcion_Quinquenio.SelectedIndex = Cmb_Percepcion_Quinquenio.Items.IndexOf(Cmb_Percepcion_Quinquenio.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Quinquenio].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Prima_Vacacional].ToString())) 
                        Cmb_Percepcion_Prima_Vacacional.SelectedIndex = Cmb_Percepcion_Prima_Vacacional.Items.IndexOf(Cmb_Percepcion_Prima_Vacacional.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Prima_Vacacional].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Prima_Dominical].ToString())) 
                        Cmb_Percepcion_Prima_Dominical.SelectedIndex = Cmb_Percepcion_Prima_Dominical.Items.IndexOf(Cmb_Percepcion_Prima_Dominical.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Prima_Dominical].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Aguinaldo].ToString()))
                        Cmb_Percepcion_Aguinaldo.SelectedIndex = Cmb_Percepcion_Aguinaldo.Items.IndexOf(Cmb_Percepcion_Aguinaldo.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Aguinaldo].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Dias_Festivos].ToString())) 
                        Cmb_Percepcion_Dias_Festivos.SelectedIndex = Cmb_Percepcion_Dias_Festivos.Items.IndexOf(Cmb_Percepcion_Dias_Festivos.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Dias_Festivos].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Horas_Extra].ToString())) 
                        Cmb_Percepcion_Horas_Extra.SelectedIndex = Cmb_Percepcion_Horas_Extra.Items.IndexOf(Cmb_Percepcion_Horas_Extra.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Horas_Extra].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Dia_Doble].ToString())) 
                        Cmb_Percepcion_Dia_Doble.SelectedIndex = Cmb_Percepcion_Dia_Doble.Items.IndexOf(Cmb_Percepcion_Dia_Doble.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Dia_Doble].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Dia_Domingo].ToString())) 
                        Cmb_Percepcion_Dia_Domingo.SelectedIndex = Cmb_Percepcion_Dia_Domingo.Items.IndexOf(Cmb_Percepcion_Dia_Domingo.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Dia_Domingo].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Ajuste_ISR].ToString()))
                        Cmb_Percepcion_Ajuste_ISR.SelectedIndex = Cmb_Percepcion_Ajuste_ISR.Items.IndexOf(Cmb_Percepcion_Ajuste_ISR.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Ajuste_ISR].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Incapacidades].ToString()))
                        Cmb_Percepcion_Incapacidades.SelectedIndex = Cmb_Percepcion_Incapacidades.Items.IndexOf(Cmb_Percepcion_Incapacidades.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Incapacidades].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Subsidio].ToString()))
                        Cmb_Percepcion_Subsidio.SelectedIndex = Cmb_Percepcion_Subsidio.Items.IndexOf(Cmb_Percepcion_Subsidio.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Subsidio].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Despensa].ToString()))
                        Cmb_Percepcion_Despensa.SelectedIndex = Cmb_Percepcion_Despensa.Items.IndexOf(Cmb_Percepcion_Despensa.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Despensa].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Sueldo_Normal].ToString()))
                        Cmb_Percepcion_Sueldo_Normal.SelectedIndex = Cmb_Percepcion_Sueldo_Normal.Items.IndexOf(Cmb_Percepcion_Sueldo_Normal.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Sueldo_Normal].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Prima_Antiguedad].ToString()))
                        Cmb_Percepcion_Prima_Antiguedad.SelectedIndex = Cmb_Percepcion_Prima_Antiguedad.Items.IndexOf(Cmb_Percepcion_Prima_Antiguedad.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Prima_Antiguedad].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Indemnizacion].ToString()))
                        Cmb_Percepcion_Indemnizacion.SelectedIndex = Cmb_Percepcion_Indemnizacion.Items.IndexOf(Cmb_Percepcion_Indemnizacion.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Indemnizacion].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Vacaciones_Pendientes_Pagar].ToString()))
                        Cmb_Percepcion_Vacaciones_Pendientes_Pagar.SelectedIndex = Cmb_Percepcion_Vacaciones_Pendientes_Pagar.Items.IndexOf(Cmb_Percepcion_Vacaciones_Pendientes_Pagar.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Vacaciones_Pendientes_Pagar].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Vacaciones].ToString()))
                        Cmb_Percepcion_Vacaciones.SelectedIndex = Cmb_Percepcion_Vacaciones.Items.IndexOf(Cmb_Percepcion_Vacaciones.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Vacaciones].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Fondo_Retiro].ToString()))
                        Cmb_Percepcion_Fondo_Retiro.SelectedIndex = Cmb_Percepcion_Fondo_Retiro.Items.IndexOf(Cmb_Percepcion_Fondo_Retiro.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Fondo_Retiro].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Prevision_Social_Multiple].ToString()))
                        Cmb_Percepcion_PSM.SelectedIndex = Cmb_Percepcion_PSM.Items.IndexOf(Cmb_Percepcion_PSM.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Percepcion_Prevision_Social_Multiple].ToString()));

                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Deduccion_Faltas].ToString()))
                        Cmb_Deduccion_Faltas.SelectedIndex = Cmb_Deduccion_Faltas.Items.IndexOf(Cmb_Deduccion_Faltas.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Deduccion_Faltas].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Deduccion_Retardos].ToString()))
                        Cmb_Deduccion_Retardos.SelectedIndex = Cmb_Deduccion_Retardos.Items.IndexOf(Cmb_Deduccion_Retardos.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Deduccion_Retardos].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Deduccion_Fondo_Retiro].ToString()))
                        Cmb_Deduccion_Fondo_Retiro.SelectedIndex = Cmb_Deduccion_Fondo_Retiro.Items.IndexOf(Cmb_Deduccion_Fondo_Retiro.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Deduccion_Fondo_Retiro].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Deduccion_ISR ].ToString()))
                        Cmb_Deduccion_ISR.SelectedIndex = Cmb_Deduccion_ISR.Items.IndexOf(Cmb_Deduccion_ISR.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Deduccion_ISR].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Deduccion_Tipo_Desc_Orden_Judicial].ToString()))
                        Cmb_Deduccion_Orden_Judicial.SelectedIndex = Cmb_Deduccion_Orden_Judicial.Items.IndexOf(Cmb_Deduccion_Orden_Judicial.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Deduccion_Tipo_Desc_Orden_Judicial].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Deduccion_IMSS].ToString()))
                        Cmb_Deduccion_IMSS.SelectedIndex = Cmb_Deduccion_IMSS.Items.IndexOf(Cmb_Deduccion_IMSS.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Deduccion_IMSS].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Deduccion_Vacaciones_Tomadas_Mas].ToString()))
                        Cmb_Deduccion_Vacaciones_Tomadas_Mas.SelectedIndex = 
                            Cmb_Deduccion_Vacaciones_Tomadas_Mas.Items.IndexOf(Cmb_Deduccion_Vacaciones_Tomadas_Mas.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Deduccion_Vacaciones_Tomadas_Mas].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Deduccion_Aguinaldo_Pagado_Mas].ToString()))
                        Cmb_Deduccion_Aguinaldo_Pagado_Mas.SelectedIndex =
                            Cmb_Deduccion_Aguinaldo_Pagado_Mas.Items.IndexOf(Cmb_Deduccion_Aguinaldo_Pagado_Mas.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Deduccion_Aguinaldo_Pagado_Mas].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Deduccion_Prima_Vacacional_Pagada_Mas].ToString()))
                        Cmb_Deduccion_PV_Pagada_Mas.SelectedIndex =
                            Cmb_Deduccion_PV_Pagada_Mas.Items.IndexOf(Cmb_Deduccion_PV_Pagada_Mas.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Deduccion_Prima_Vacacional_Pagada_Mas].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Deduccion_Sueldo_Pagado_Mas].ToString()))
                        Cmb_Deduccion_Sueldo_Pagado_Mas.SelectedIndex =
                            Cmb_Deduccion_Sueldo_Pagado_Mas.Items.IndexOf(Cmb_Deduccion_Sueldo_Pagado_Mas.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Deduccion_Sueldo_Pagado_Mas].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Aguinaldo].ToString()))
                        Cmb_Deduccion_Orden_Judicial_Aguinaldo.SelectedIndex =
                            Cmb_Deduccion_Orden_Judicial_Aguinaldo.Items.IndexOf(Cmb_Deduccion_Orden_Judicial_Aguinaldo.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Aguinaldo].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Prima_Vacacional].ToString()))
                        Cmb_Deduccion_Orden_Judicial_Prima_Vacacional.SelectedIndex =
                            Cmb_Deduccion_Orden_Judicial_Prima_Vacacional.Items.IndexOf(Cmb_Deduccion_Orden_Judicial_Prima_Vacacional.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Prima_Vacacional].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Indemnizacion].ToString()))
                        Cmb_Deduccion_Orden_Judicial_Indemnizacion.SelectedIndex =
                            Cmb_Deduccion_Orden_Judicial_Indemnizacion.Items.IndexOf(Cmb_Deduccion_Orden_Judicial_Indemnizacion.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Indemnizacion].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Deduccion_ISSEG].ToString()))
                        Cmb_Deduccion_ISSEG.SelectedIndex =
                            Cmb_Deduccion_ISSEG.Items.IndexOf(Cmb_Deduccion_ISSEG.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Deduccion_ISSEG].ToString()));

                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Salario_Limite_Prestamo].ToString()))
                        Txt_Salario_Limite_Prestamo.Text = String.Format("{0:0.00}", Convert.ToDouble(Renglon[Cat_Nom_Parametros.Campo_Salario_Limite_Prestamo].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Salario_Mensual_Maximo].ToString()))
                        Txt_Salario_Mensual_Maximo.Text = String.Format("{0:0.00}", Convert.ToDouble(Renglon[Cat_Nom_Parametros.Campo_Salario_Mensual_Maximo].ToString()));
                    if (!string.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Salario_Diario_Int_Topado].ToString()))
                        Txt_Salario_Diario_Integrado_Topado.Text = String.Format("{0:0.00}", Convert.ToDouble(Renglon[Cat_Nom_Parametros.Campo_Salario_Diario_Int_Topado].ToString()));

                    if (!String.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_IP_Servidor].ToString()))
                        Txt_IP_Servidor_Reloj_Checador.Text = Renglon[Cat_Nom_Parametros.Campo_IP_Servidor].ToString();
                    if (!String.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Nombre_Base_Datos].ToString()))
                        Txt_Nombre_Base_Datos_Reloj_Checador.Text = Renglon[Cat_Nom_Parametros.Campo_Nombre_Base_Datos].ToString();
                    if (!String.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Usuario_SQL].ToString()))
                        Txt_Usuario_Base_Datos_Reloj_Checador.Text = Renglon[Cat_Nom_Parametros.Campo_Usuario_SQL].ToString();
                    if (!String.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Password_Base_Datos].ToString()))
                    {
                        Txt_Password_Base_Datos_Reloj_Checador.Text = Renglon[Cat_Nom_Parametros.Campo_Password_Base_Datos].ToString();
                        Txt_Password_Base_Datos_Reloj_Checador.Attributes.Add("value", Txt_Password_Base_Datos_Reloj_Checador.Text);
                    }

                    if (!String.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Tipo_IMSS].ToString().Trim()))
                        Cmb_Tipo_Calculo_IMSS.SelectedIndex = Cmb_Tipo_Calculo_IMSS.Items.IndexOf(
                            Cmb_Tipo_Calculo_IMSS.Items.FindByText(Renglon[Cat_Nom_Parametros.Campo_Tipo_IMSS].ToString().Trim()));

                    if (!String.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Minutos_Dia].ToString()))
                        Txt_Minutos_Dia.Text = Renglon[Cat_Nom_Parametros.Campo_Minutos_Dia].ToString().Trim();

                    if (!String.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Minutos_Retardo].ToString()))
                        Txt_Minutos_Retardo.Text = Renglon[Cat_Nom_Parametros.Campo_Minutos_Retardo].ToString().Trim();

                    if (!String.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_ISSEG_Porcentaje_Prevision_Social_Multiple].ToString()))
                        Txt_Porcentaje_Prevision_Social_Multiple.Text = Renglon[Cat_Nom_Parametros.Campo_ISSEG_Porcentaje_Prevision_Social_Multiple].ToString().Trim();

                    if (!String.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_ISSEG_Porcentaje_Aplicar_Empleado].ToString()))
                        Txt_Porcentaje_Factor_Social.Text = Renglon[Cat_Nom_Parametros.Campo_ISSEG_Porcentaje_Aplicar_Empleado].ToString().Trim();

                    if (!String.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Dias_IMSS].ToString()))
                        Txt_Dias_IMSS.Text = Renglon[Cat_Nom_Parametros.Campo_Dias_IMSS].ToString().Trim();

                    if (!String.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Tope_ISSEG].ToString()))
                        Txt_Tope_ISSEG.Text = Renglon[Cat_Nom_Parametros.Campo_Tope_ISSEG].ToString().Trim();

                    if (!String.IsNullOrEmpty(Renglon[Cat_Nom_Parametros.Campo_Proveedor_Fonacot].ToString()))
                        Cmb_Proveedor_Fonacot.SelectedIndex = Cmb_Proveedor_Fonacot.Items.IndexOf(
                            Cmb_Proveedor_Fonacot.Items.FindByValue(Renglon[Cat_Nom_Parametros.Campo_Proveedor_Fonacot].ToString().Trim()));
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error generado al cargar la información del parámetro de la nómina. error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region(Metodos de Operacion [Alta - Modificar - Eliminar - Consultar])
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta_Parametro_Nomina
    /// DESCRIPCION : Ejecuta la Alta de un Parametro para la Nomina
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 8/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Alta_Parametro_Nomina()
    {
        Cls_Cat_Nom_Parametros_Negocio Rs_Alta_Cat_Parametro_Nomina = new Cls_Cat_Nom_Parametros_Negocio();
        try
        {
            Rs_Alta_Cat_Parametro_Nomina.P_Zona_ID = Cmb_Zona.SelectedValue;
            Rs_Alta_Cat_Parametro_Nomina.P_Percepcion_Quinquenio = Cmb_Percepcion_Quinquenio.SelectedValue.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_Percepcion_Prima_Vacacional = Cmb_Percepcion_Prima_Vacacional.SelectedValue.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_Percepcion_Prima_Dominical = Cmb_Percepcion_Prima_Dominical.SelectedValue.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_Percepcion_Aguinaldo = Cmb_Percepcion_Aguinaldo.SelectedValue.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_Percepcion_Dias_Festivos = Cmb_Percepcion_Dias_Festivos.SelectedValue.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_Percepcion_Horas_Extra = Cmb_Percepcion_Horas_Extra.SelectedValue.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_Percepcion_Dia_Doble = Cmb_Percepcion_Dia_Doble.SelectedValue.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_Percepcion_Dia_Domingo = Cmb_Percepcion_Dia_Domingo.SelectedValue.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_Percepcion_Ajuste_ISR = Cmb_Percepcion_Ajuste_ISR.SelectedValue.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_Percepcion_Incapacidades = Cmb_Percepcion_Incapacidades.SelectedValue.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_Percepcion_Subsidio = Cmb_Percepcion_Subsidio.SelectedValue.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_Percepcion_Despensa = Cmb_Percepcion_Despensa.SelectedValue.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_Percepcion_Sueldo_Normal = Cmb_Percepcion_Sueldo_Normal.SelectedValue.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_Percepcion_Prima_Antiguedad = Cmb_Percepcion_Prima_Antiguedad.SelectedValue.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_Percepcion_Indemnizacion = Cmb_Percepcion_Indemnizacion.SelectedValue.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_Percepcion_Vacaciones_Pendientes_Pagar = Cmb_Percepcion_Vacaciones_Pendientes_Pagar.SelectedValue.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_Percepcion_Vacaciones = Cmb_Percepcion_Vacaciones.SelectedValue.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_Percepcion_Prevision_Social_Multiple = Cmb_Percepcion_PSM.SelectedValue.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_Percepcion_Fondo_Retiro = Cmb_Percepcion_Fondo_Retiro.SelectedValue.Trim();

            Rs_Alta_Cat_Parametro_Nomina.P_IP_Servidor = Txt_IP_Servidor_Reloj_Checador.Text.ToString();
            Rs_Alta_Cat_Parametro_Nomina.P_Nombre_Base_Datos = Txt_Nombre_Base_Datos_Reloj_Checador.Text.ToString();
            Rs_Alta_Cat_Parametro_Nomina.P_Usuario_SQL = Txt_Usuario_Base_Datos_Reloj_Checador.Text.ToString();
            Rs_Alta_Cat_Parametro_Nomina.P_Password_Base_Datos = Txt_Password_Base_Datos_Reloj_Checador.Text.ToString();

            Rs_Alta_Cat_Parametro_Nomina.P_Deduccion_Faltas = Cmb_Deduccion_Faltas.SelectedValue.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_Deduccion_Retardos = Cmb_Deduccion_Retardos.SelectedValue.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_Deduccion_Fondo_Retiro = Cmb_Deduccion_Fondo_Retiro.SelectedValue.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_Deduccion_ISR = Cmb_Deduccion_ISR.SelectedValue.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_Deduccion_Tipo_Desc_Orden_Judicial = Cmb_Deduccion_Orden_Judicial.SelectedValue.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_Deduccion_Vacaciones_Tomadas_Mas = Cmb_Deduccion_Vacaciones_Tomadas_Mas.SelectedValue.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_Deduccion_IMSS = Cmb_Deduccion_IMSS.SelectedValue.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_Deduccion_Aguinaldo_Pagado_Mas = Cmb_Deduccion_Aguinaldo_Pagado_Mas.SelectedValue.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_Deduccion_Prima_Vac_Pagada_Mas = Cmb_Deduccion_PV_Pagada_Mas.SelectedValue.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_Deduccion_Sueldo_Pagado_Mas = Cmb_Deduccion_Sueldo_Pagado_Mas.SelectedValue.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_Deduccion_Orden_Judicial_Aguinaldo = Cmb_Deduccion_Orden_Judicial_Aguinaldo.SelectedValue.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_Deduccion_Orden_Judicial_Prima_Vacacional = Cmb_Deduccion_Orden_Judicial_Prima_Vacacional.SelectedValue.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_Deduccion_Orden_Judicial_Indemnizacion = Cmb_Deduccion_Orden_Judicial_Indemnizacion.SelectedValue.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_Deduccion_ISSEG = Cmb_Deduccion_ISSEG.SelectedValue.Trim();

            Rs_Alta_Cat_Parametro_Nomina.P_Porcentaje_Prima_Vacacional = Txt_Porcentaje_Prima_Vacacional.Text.Equals("") ? 0 : Convert.ToDouble(Txt_Porcentaje_Prima_Vacacional.Text);
            Rs_Alta_Cat_Parametro_Nomina.P_Porcentaje_Fondo_Retiro = Txt_Porcentaje_Fondo_Retiro.Text.Equals("") ? 0 : Convert.ToDouble(Txt_Porcentaje_Fondo_Retiro.Text);
            Rs_Alta_Cat_Parametro_Nomina.P_Porcentaje_Prima_Dominical = Txt_Porcentaje_Prima_Dominical.Text.Equals("") ? 0 : Convert.ToDouble(Txt_Porcentaje_Prima_Dominical.Text);
            Rs_Alta_Cat_Parametro_Nomina.P_Fecha_Prima_Vacacional_1 = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Prima_Vacacional_1.Text));
            Rs_Alta_Cat_Parametro_Nomina.P_Fecha_Prima_Vacacional_2 = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Prima_Vacacional_2.Text));
            Rs_Alta_Cat_Parametro_Nomina.P_Salario_Limite_Prestamo = (!String.IsNullOrEmpty(Txt_Salario_Limite_Prestamo.Text.Trim()) || !Txt_Salario_Limite_Prestamo.Text.Trim().Contains("$")) ? Convert.ToDouble(Txt_Salario_Limite_Prestamo.Text.Trim()) : 0;
            Rs_Alta_Cat_Parametro_Nomina.P_Salario_Mensual_Maximo = Txt_Salario_Mensual_Maximo.Text.Equals("") ? 0 : Convert.ToDouble(Txt_Salario_Mensual_Maximo.Text);
            Rs_Alta_Cat_Parametro_Nomina.P_Salario_Diario_Integrado_Topado = Txt_Salario_Diario_Integrado_Topado.Text.Equals("") ? 0 : Convert.ToDouble(Txt_Salario_Diario_Integrado_Topado.Text);

            Rs_Alta_Cat_Parametro_Nomina.P_Tipo_IMSS = Cmb_Tipo_Calculo_IMSS.SelectedItem.Text.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_Minutos_Dia = Txt_Minutos_Dia.Text.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_Minutos_Retardo = Txt_Minutos_Retardo.Text.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_ISSEG_Porcentaje_Prevision_Social_Multiple = Txt_Porcentaje_Prevision_Social_Multiple.Text.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_ISSEG_Porcentaje_Aplicar_Empleado = Txt_Porcentaje_Factor_Social.Text.Trim();

            Rs_Alta_Cat_Parametro_Nomina.P_Dias_IMSS = Txt_Dias_IMSS.Text.Trim();
            Rs_Alta_Cat_Parametro_Nomina.P_Tope_ISSEG = Txt_Tope_ISSEG.Text.Trim().Replace(",", String.Empty);
            Rs_Alta_Cat_Parametro_Nomina.P_Proveedor_Fonacot = Cmb_Proveedor_Fonacot.SelectedValue;

            ///Campos de Auditoria
            Rs_Alta_Cat_Parametro_Nomina.P_Usuario_Creo = HttpUtility.HtmlDecode((String)Cls_Sessiones.Nombre_Empleado);
            if (Rs_Alta_Cat_Parametro_Nomina.Alta_Parametro())
            {
                Configuracion_Inicial();
                Limpiar_Ctlrs();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa [Alta Parámetro]');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al dar la Alta de un Parámetro. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Parametro_Nomina
    /// DESCRIPCION : Ejecuta la Actualizacion de un Parametro Nomina
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 8/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Modificar_Parametro_Nomina()
    {
        Cls_Cat_Nom_Parametros_Negocio Rs_Modificar_Cat_Parametro_Nomina = new Cls_Cat_Nom_Parametros_Negocio();
        try
        {
            Rs_Modificar_Cat_Parametro_Nomina.P_Parametro_ID = Txt_Parametro_ID.Text;
            Rs_Modificar_Cat_Parametro_Nomina.P_Percepcion_Quinquenio = Cmb_Percepcion_Quinquenio.SelectedValue.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Percepcion_Prima_Vacacional = Cmb_Percepcion_Prima_Vacacional.SelectedValue.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Percepcion_Prima_Dominical = Cmb_Percepcion_Prima_Dominical.SelectedValue.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Percepcion_Aguinaldo = Cmb_Percepcion_Aguinaldo.SelectedValue.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Percepcion_Dias_Festivos = Cmb_Percepcion_Dias_Festivos.SelectedValue.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Percepcion_Horas_Extra = Cmb_Percepcion_Horas_Extra.SelectedValue.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Percepcion_Dia_Doble = Cmb_Percepcion_Dia_Doble.SelectedValue.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Percepcion_Dia_Domingo = Cmb_Percepcion_Dia_Domingo.SelectedValue.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Percepcion_Ajuste_ISR = Cmb_Percepcion_Ajuste_ISR.SelectedValue.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Percepcion_Incapacidades = Cmb_Percepcion_Incapacidades.SelectedValue.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Percepcion_Subsidio = Cmb_Percepcion_Subsidio.SelectedValue.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Percepcion_Despensa = Cmb_Percepcion_Despensa.SelectedValue.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Percepcion_Sueldo_Normal = Cmb_Percepcion_Sueldo_Normal.SelectedValue.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Percepcion_Prima_Antiguedad = Cmb_Percepcion_Prima_Antiguedad.SelectedValue.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Percepcion_Indemnizacion = Cmb_Percepcion_Indemnizacion.SelectedValue.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Percepcion_Vacaciones_Pendientes_Pagar = Cmb_Percepcion_Vacaciones_Pendientes_Pagar.SelectedValue.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Percepcion_Vacaciones = Cmb_Percepcion_Vacaciones.SelectedValue.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_IP_Servidor = Txt_IP_Servidor_Reloj_Checador.Text.ToString();
            Rs_Modificar_Cat_Parametro_Nomina.P_Nombre_Base_Datos = Txt_Nombre_Base_Datos_Reloj_Checador.Text.ToString();
            Rs_Modificar_Cat_Parametro_Nomina.P_Usuario_SQL = Txt_Usuario_Base_Datos_Reloj_Checador.Text.ToString();
            Rs_Modificar_Cat_Parametro_Nomina.P_Password_Base_Datos = Txt_Password_Base_Datos_Reloj_Checador.Text.ToString();
            Rs_Modificar_Cat_Parametro_Nomina.P_Percepcion_Prevision_Social_Multiple = Cmb_Percepcion_PSM.SelectedValue.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Percepcion_Fondo_Retiro = Cmb_Percepcion_Fondo_Retiro.SelectedValue.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Percepcion_Prevision_Social_Multiple = Cmb_Percepcion_PSM.SelectedValue.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Percepcion_Fondo_Retiro = Cmb_Percepcion_Fondo_Retiro.SelectedValue.Trim();

            Rs_Modificar_Cat_Parametro_Nomina.P_Deduccion_Faltas = Cmb_Deduccion_Faltas.SelectedValue.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Deduccion_Retardos = Cmb_Deduccion_Retardos.SelectedValue.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Deduccion_Fondo_Retiro = Cmb_Deduccion_Fondo_Retiro.SelectedValue.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Deduccion_ISR = Cmb_Deduccion_ISR.SelectedValue.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Deduccion_Tipo_Desc_Orden_Judicial = Cmb_Deduccion_Orden_Judicial.SelectedValue.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Deduccion_Vacaciones_Tomadas_Mas = Cmb_Deduccion_Vacaciones_Tomadas_Mas.SelectedValue.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Deduccion_Aguinaldo_Pagado_Mas = Cmb_Deduccion_Aguinaldo_Pagado_Mas.SelectedValue.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Deduccion_Prima_Vac_Pagada_Mas = Cmb_Deduccion_PV_Pagada_Mas.SelectedValue.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Deduccion_Sueldo_Pagado_Mas = Cmb_Deduccion_Sueldo_Pagado_Mas.SelectedValue.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Deduccion_IMSS = Cmb_Deduccion_IMSS.SelectedValue.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Deduccion_Orden_Judicial_Aguinaldo = Cmb_Deduccion_Orden_Judicial_Aguinaldo.SelectedValue.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Deduccion_Orden_Judicial_Prima_Vacacional = Cmb_Deduccion_Orden_Judicial_Prima_Vacacional.SelectedValue.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Deduccion_Orden_Judicial_Indemnizacion = Cmb_Deduccion_Orden_Judicial_Indemnizacion.SelectedValue.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Deduccion_ISSEG = Cmb_Deduccion_ISSEG.SelectedValue.Trim();

            Rs_Modificar_Cat_Parametro_Nomina.P_Zona_ID = Cmb_Zona.SelectedValue;   
            Rs_Modificar_Cat_Parametro_Nomina.P_Porcentaje_Prima_Vacacional = Txt_Porcentaje_Prima_Vacacional.Text.Equals("") ? 0 : Convert.ToDouble(Txt_Porcentaje_Prima_Vacacional.Text);
            Rs_Modificar_Cat_Parametro_Nomina.P_Porcentaje_Fondo_Retiro = Txt_Porcentaje_Fondo_Retiro.Text.Equals("") ? 0 : Convert.ToDouble(Txt_Porcentaje_Fondo_Retiro.Text);
            Rs_Modificar_Cat_Parametro_Nomina.P_Porcentaje_Prima_Dominical = Txt_Porcentaje_Prima_Dominical.Text.Equals("") ? 0 : Convert.ToDouble(Txt_Porcentaje_Prima_Dominical.Text);
            Rs_Modificar_Cat_Parametro_Nomina.P_Fecha_Prima_Vacacional_1 = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Prima_Vacacional_1.Text));
            Rs_Modificar_Cat_Parametro_Nomina.P_Fecha_Prima_Vacacional_2 = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Prima_Vacacional_2.Text));
            Rs_Modificar_Cat_Parametro_Nomina.P_Salario_Limite_Prestamo = (!String.IsNullOrEmpty(Txt_Salario_Limite_Prestamo.Text.Trim()) || !Txt_Salario_Limite_Prestamo.Text.Trim().Contains("$")) ? Convert.ToDouble(Txt_Salario_Limite_Prestamo.Text.Trim()) : 0;
            Rs_Modificar_Cat_Parametro_Nomina.P_Salario_Mensual_Maximo = Txt_Salario_Mensual_Maximo.Text.Equals("") ? 0 : Convert.ToDouble(Txt_Salario_Mensual_Maximo.Text);
            Rs_Modificar_Cat_Parametro_Nomina.P_Salario_Diario_Integrado_Topado = Txt_Salario_Diario_Integrado_Topado.Text.Equals("") ? 0 : Convert.ToDouble(Txt_Salario_Diario_Integrado_Topado.Text);

            Rs_Modificar_Cat_Parametro_Nomina.P_Tipo_IMSS = Cmb_Tipo_Calculo_IMSS.SelectedItem.Text.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Minutos_Dia = Txt_Minutos_Dia.Text.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Minutos_Retardo = Txt_Minutos_Retardo.Text.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_ISSEG_Porcentaje_Prevision_Social_Multiple = Txt_Porcentaje_Prevision_Social_Multiple.Text.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_ISSEG_Porcentaje_Aplicar_Empleado = Txt_Porcentaje_Factor_Social.Text.Trim();

            Rs_Modificar_Cat_Parametro_Nomina.P_Dias_IMSS = Txt_Dias_IMSS.Text.Trim();
            Rs_Modificar_Cat_Parametro_Nomina.P_Tope_ISSEG = Txt_Tope_ISSEG.Text.Trim().Replace(",", String.Empty);
            Rs_Modificar_Cat_Parametro_Nomina.P_Proveedor_Fonacot = Cmb_Proveedor_Fonacot.SelectedValue;

            ///Campos de Auditoria
            Rs_Modificar_Cat_Parametro_Nomina.P_Usuario_Creo = HttpUtility.HtmlDecode((String)Cls_Sessiones.Nombre_Empleado);
            if (Rs_Modificar_Cat_Parametro_Nomina.Modificar_Parametro())
            {
                Configuracion_Inicial();
                Limpiar_Ctlrs();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa [Modificar Parámetro]');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al Actualizar a un Parámetro. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Eliminar_Parametro_Nomina
    /// DESCRIPCION : Ejecuta la Baja de un Parametro Nomina
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 8/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Eliminar_Parametro_Nomina()
    {
        Cls_Cat_Nom_Parametros_Negocio Rs_Eliminar_Cat_Parametro_Nomina = new Cls_Cat_Nom_Parametros_Negocio();
        try
        {
            Rs_Eliminar_Cat_Parametro_Nomina.P_Parametro_ID = Txt_Parametro_ID.Text;

            if (Rs_Eliminar_Cat_Parametro_Nomina.Eliminar_Parametro())
            {
                Configuracion_Inicial();
                Limpiar_Ctlrs();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa [Eliminar Parametro]');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al dar de Baja un Parámetro. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Combos)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Zonas
    /// DESCRIPCION : consulta las Zonas Economicas
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 8/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    /// ******************************************************************************
    private void Consultar_Zonas() {
        Cls_Cat_Nom_Zona_Economica_Negocio Cat_Zona = new Cls_Cat_Nom_Zona_Economica_Negocio();
        DataTable Dt_Zonas=null;
        try
        {
            Dt_Zonas = Cat_Zona.Consulta_Zona_Economica();
            Cmb_Zona.DataSource = Dt_Zonas;
            Cmb_Zona.DataValueField = Cat_Nom_Zona_Economica.Campo_Zona_ID;
            Cmb_Zona.DataTextField = Cat_Nom_Zona_Economica.Campo_Zona_Economica;
            Cmb_Zona.DataBind();
            Cmb_Zona.Items.Insert(0, new ListItem("< Seleccione >", ""));
            Cmb_Zona.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las zonas. Error: [" +Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Percepciones_Deducciones
    /// DESCRIPCION : consulta las Zonas Economicas
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 8/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    /// ******************************************************************************
    private void Consultar_Percepciones_Deducciones(DropDownList Combo, String Tipo, String Tipo_Asignacion) {
        Cls_Cat_Nom_Percepciones_Deducciones_Business Consulta_Percepciones_Deducciones = new Cls_Cat_Nom_Percepciones_Deducciones_Business();//Variable de conexion con la capa de negocio.
        DataTable Dt_Percepciones_Deducciones = null;//Variable que almacenará una lista de percepciones.
        try
        {
            Consulta_Percepciones_Deducciones.P_TIPO = Tipo;
            Consulta_Percepciones_Deducciones.P_TIPO_ASIGNACION =Tipo_Asignacion;
            Consulta_Percepciones_Deducciones.P_ESTATUS = "ACTIVO";
            Dt_Percepciones_Deducciones = Consulta_Percepciones_Deducciones.Consulta_Avanzada_Percepciones_Deducciones();

            Dt_Percepciones_Deducciones = Juntar_Clave_Nombre(Dt_Percepciones_Deducciones);
            Combo.DataSource = Dt_Percepciones_Deducciones;
            Combo.DataTextField = Cat_Nom_Percepcion_Deduccion.Campo_Nombre;            
            Combo.DataValueField = Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID;
            Combo.DataBind();
            Combo.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Combo.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las percepciones deducciones. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Proveedores_Nomina
    /// DESCRIPCION : consultamos los proveedores de nomina.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : Abril/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    /// ******************************************************************************
    private void Consultar_Proveedores_Nomina()
    {
        Cls_Cat_Nom_Proveedores_Negocio Obj_Proveedores = new Cls_Cat_Nom_Proveedores_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Proveedores = null;//Listado de proveedores.

        try
        {
            Dt_Proveedores = Obj_Proveedores.Consultar_Proveedores();

            Cmb_Proveedor_Fonacot.DataSource = Dt_Proveedores;
            Cmb_Proveedor_Fonacot.DataTextField = Cat_Nom_Proveedores.Campo_Nombre;
            Cmb_Proveedor_Fonacot.DataValueField = Cat_Nom_Proveedores.Campo_Proveedor_ID;
            Cmb_Proveedor_Fonacot.DataBind();
            Cmb_Proveedor_Fonacot.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Proveedor_Fonacot.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cargar los proveedores de nomina. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Control Acceso Pagina)
    /// *****************************************************************************************************************************
    /// NOMBRE: Configuracion_Acceso
    /// 
    /// DESCRIPCIÓN: Habilita las operaciones que podrá realizar el usuario en la página.
    /// 
    /// PARÁMETROS: No Áplica.
    /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 23/Mayo/2011 10:43 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *****************************************************************************************************************************
    protected void Configuracion_Acceso(String URL_Pagina)
    {
        List<ImageButton> Botones = new List<ImageButton>();//Variable que almacenara una lista de los botones de la página.
        DataRow[] Dr_Menus = null;//Variable que guardara los menus consultados.

        try
        {
            //Agregamos los botones a la lista de botones de la página.
            Botones.Add(Btn_Nuevo);
            Botones.Add(Btn_Modificar);
            Botones.Add(Btn_Eliminar);

            if (!String.IsNullOrEmpty(Request.QueryString["PAGINA"]))
            {
                if (Es_Numero(Request.QueryString["PAGINA"].Trim()))
                {
                    //Consultamos el menu de la página.
                    Dr_Menus = Cls_Sessiones.Menu_Control_Acceso.Select("MENU_ID=" + Request.QueryString["PAGINA"]);

                    if (Dr_Menus.Length > 0)
                    {
                        //Validamos que el menu consultado corresponda a la página a validar.
                        if (Dr_Menus[0][Apl_Cat_Menus.Campo_URL_Link].ToString().Contains(URL_Pagina))
                        {
                            Cls_Util.Configuracion_Acceso_Sistema_SIAS(Botones, Dr_Menus[0]);//Habilitamos la configuracón de los botones.
                        }
                        else
                        {
                            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                    }
                }
                else
                {
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }
            }
            else
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al habilitar la configuración de accesos a la página. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: IsNumeric
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 29/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Es_Numero(String Cadena)
    {
        Boolean Resultado = true;
        Char[] Array = Cadena.ToCharArray();
        try
        {
            for (int index = 0; index < Array.Length; index++)
            {
                if (!Char.IsDigit(Array[index])) return false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al Validar si es un dato numerico. Error [" + Ex.Message + "]");
        }
        return Resultado;
    }
    #endregion

    #endregion

    #region (Eventos)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Alta de un Parámetro Nomina
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 8/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Nuevo.ToolTip.Equals("Nuevo"))
            {
                Habilitar_Controles("Nuevo");
                Limpiar_Ctlrs();
            }
            else
            {
                if (Validar_Datos_Parametros())
                {
                    Alta_Parametro_Nomina();
                    Configuracion_Inicial();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Modificar un Parámetro
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 8/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            if (Btn_Modificar.ToolTip.Equals("Modificar"))
            {
                if (!Txt_Parametro_ID.Text.Equals(""))
                {
                    Habilitar_Controles("Modificar");
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el registro que desea modificar sus datos <br>";
                }
            }
            else
            {
                if (Validar_Datos_Parametros())
                {
                    Modificar_Parametro_Nomina();
                    Configuracion_Inicial();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
    ///DESCRIPCIÓN: Eliminar un Parámetro
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 8/Noviembre/2010  
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            if (Btn_Eliminar.ToolTip.Equals("Eliminar"))
            {
                if (!Txt_Parametro_ID.Text.Equals(""))
                {
                    Eliminar_Parametro_Nomina();
                    Configuracion_Inicial();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el registro que desea eliminar <br>";
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Salir de la Operacion Actual
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 8/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip == "Inicio")
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Configuracion_Inicial();//Habilita los controles para la siguiente operación del usuario en el catálogo
                Limpiar_Ctlrs();
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    #endregion
}
