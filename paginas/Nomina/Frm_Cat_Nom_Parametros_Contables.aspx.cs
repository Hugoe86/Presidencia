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
using Presidencia.Informacion_Presupuestal;
using Presidencia.Constantes;
using Presidencia.Parametros_Contables.Negocio;
using Presidencia.Sessiones;

public partial class paginas_Nomina_Frm_Cat_Nom_Parametros_Contables : System.Web.UI.Page
{

    #region (Load/Init)
    /// *************************************************************************************************************************
    /// Nombre: Page_Load
    /// 
    /// Descripción: Carga la configuración inicíal de la página.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Marzo/2012
    /// Usuario modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Configuración_Inicial(); 
            }

            Mostrar_Mensaje(false);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Mostrar_Mensaje(true);
        }
    }
    #endregion

    #region (Métodos) 

    #region (Generales)
    /// *************************************************************************************************************************
    /// Nombre: Configuración_Inicial
    /// 
    /// Descripción: Carga la configuración inicíal de la página.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Marzo/2012
    /// Usuario modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    protected void Configuración_Inicial() {

        try
        {
            Cargar_Ctrl();
            Limpiar_Controles();
            Habilitar_Controles("Inicial");
            Consultar_Parametros_Contables();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cargar la configuración inicial. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************************************************
    /// Nombre: Limpiar_Controles
    /// 
    /// Descripción: Limpiar los controles de la página.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Marzo/2012
    /// Usuario modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    protected void Limpiar_Controles() {
        try
        {
            foreach (DropDownList Ctrl in this.Cmb_Aportaciones_IMSS.Parent.Controls.OfType<DropDownList>())
                Ctrl.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al limpiar los controles de la página. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************************************************
    /// Nombre: Limpiar_Controles
    /// 
    /// Descripción: Habilita y Deshabilita los controles de la forma para prepara la página
    ///               para a siguiente operación
    /// 
    /// Parámetros  : Operacion: Indica la operación que se desea realizar por parte del usuario
    ///                          si es una alta, modificacion.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Marzo/2012
    /// Usuario modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *******************************************************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado = false;

        try
        {

            switch (Operacion)
            {
                case "Inicial":
                    Habilitado = false;
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Nuevo.Visible = Validar_Solo_Exista_1_Parametro_Contable();
                    Btn_Modificar.Visible = !Validar_Solo_Exista_1_Parametro_Contable();
                    Btn_Eliminar.Visible = true;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
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

            foreach (DropDownList Ctrl in this.Cmb_Aportaciones_IMSS.Parent.Controls.OfType<DropDownList>())
                Ctrl.Enabled = Habilitado;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al Habilitar los Controles del formulario. Error:[" + ex.Message.ToString() + "]");
        }
    }
    #endregion

    #region (Consultas)
    /// *************************************************************************************************************************
    /// Nombre: Consultar_Partidas
    /// 
    /// Descripción: Consultar partidas que existen actualmente en el presupuesto.
    /// 
    /// Parámetros  : 
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Marzo/2012
    /// Usuario modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *******************************************************************************************************************
    protected void Consultar_Partidas(DropDownList Ctrl)
    {        
        try
        {
            Ctrl.DataSource = Cls_Help_Nom_Validate_Presupuestal.Consultar_Partidas_General();
            Ctrl.DataTextField = Cat_Sap_Partidas_Especificas.Campo_Nombre;
            Ctrl.DataValueField = Cat_Sap_Partidas_Especificas.Campo_Partida_ID;
            Ctrl.DataBind();

            Ctrl.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Ctrl.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cosultas las partidas. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************************************************
    /// Nombre: Cargar_Ctrl
    /// 
    /// Descripción: Cargar las partidas que existen actualmente en el presupuesto.
    /// 
    /// Parámetros  : 
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Marzo/2012
    /// Usuario modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *******************************************************************************************************************
    protected void Cargar_Ctrl()
    {
        try
        {
            foreach (DropDownList Ctrl in this.Cmb_Aportaciones_IMSS.Parent.Controls.OfType<DropDownList>())
                Consultar_Partidas(Ctrl);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cargar los controles de la página. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************************************************
    /// Nombre: Consultar_Parametros_Contables
    /// 
    /// Descripción: Consultamos los Parámetros Contables.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Marzo/2012
    /// Usuario modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    private void Consultar_Parametros_Contables()
    {
        Cls_Cat_Nom_Parametros_Contables_Negocio INF_PARAMETROS_CONTABLES = new Cls_Cat_Nom_Parametros_Contables_Negocio();//Variable de conexión a la capa de negocios. 
        DataTable Dt_Parametros_Contables = null;//Variable que almacena un listado de parámetros contables.

        try
        {
            Dt_Parametros_Contables = INF_PARAMETROS_CONTABLES.Consultar_Parametros_Contables();

            if (Dt_Parametros_Contables is DataTable) {
                if (Dt_Parametros_Contables.Rows.Count > 0) {
                    foreach (DataRow PARAMETRO in Dt_Parametros_Contables.Rows) {
                        if (PARAMETRO is DataRow) {

                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Aportaciones_IMSS].ToString()))
                                Cmb_Aportaciones_IMSS.SelectedIndex = Cmb_Aportaciones_IMSS.Items.IndexOf(
                                    Cmb_Aportaciones_IMSS.Items.FindByValue(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Aportaciones_IMSS].ToString()));

                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Aportaciones_ISSEG].ToString()))
                                Cmb_Aportaciones_ISSEG.SelectedIndex = Cmb_Aportaciones_ISSEG.Items.IndexOf(
                                    Cmb_Aportaciones_ISSEG.Items.FindByValue(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Aportaciones_ISSEG].ToString()));

                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Cuotas_Fondo_Retiro].ToString()))
                                Cmb_Cuotas_Fondo_Ahorro.SelectedIndex = Cmb_Cuotas_Fondo_Ahorro.Items.IndexOf(
                                    Cmb_Cuotas_Fondo_Ahorro.Items.FindByValue(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Cuotas_Fondo_Retiro].ToString()));

                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Dietas].ToString()))
                                Cmb_Dietas.SelectedIndex = Cmb_Dietas.Items.IndexOf(
                                    Cmb_Dietas.Items.FindByValue(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Dietas].ToString()));

                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Estimulos_Productividad_Eficiencia].ToString()))
                                Cmb_Estimulos_Productividad_Eficiencia.SelectedIndex = Cmb_Estimulos_Productividad_Eficiencia.Items.IndexOf(
                                    Cmb_Estimulos_Productividad_Eficiencia.Items.FindByValue(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Estimulos_Productividad_Eficiencia].ToString()));

                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Gratificaciones_Fin_Anio].ToString()))
                                Cmb_Gratificaciones_Fin_Anio.SelectedIndex = Cmb_Gratificaciones_Fin_Anio.Items.IndexOf(
                                    Cmb_Gratificaciones_Fin_Anio.Items.FindByValue(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Gratificaciones_Fin_Anio].ToString()));

                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Honorarios_Asimilados].ToString()))
                                Cmb_Honorarios_Asimilados.SelectedIndex = Cmb_Honorarios_Asimilados.Items.IndexOf(
                                    Cmb_Honorarios_Asimilados.Items.FindByValue(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Honorarios_Asimilados].ToString()));

                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Horas_Extra].ToString()))
                                Cmb_Horas_Extra.SelectedIndex = Cmb_Horas_Extra.Items.IndexOf(
                                    Cmb_Horas_Extra.Items.FindByValue(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Horas_Extra].ToString()));

                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Impuestos_Sobre_Nominas].ToString()))
                                Cmb_Impuestos_Sobre_Nominas.SelectedIndex = Cmb_Impuestos_Sobre_Nominas.Items.IndexOf(
                                    Cmb_Impuestos_Sobre_Nominas.Items.FindByValue(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Impuestos_Sobre_Nominas].ToString()));

                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Parametro_ID].ToString()))
                                HTxt_Clave_Primaria_Parametro_ID.Value = PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Parametro_ID].ToString();

                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Participacipaciones_Vigilancia].ToString()))
                                Cmb_Participaciones_Vigilancia.SelectedIndex = Cmb_Participaciones_Vigilancia.Items.IndexOf(
                                    Cmb_Participaciones_Vigilancia.Items.FindByValue(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Participacipaciones_Vigilancia].ToString()));

                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Pensiones].ToString()))
                                Cmb_Pensiones.SelectedIndex = Cmb_Pensiones.Items.IndexOf(
                                    Cmb_Pensiones.Items.FindByValue(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Pensiones].ToString()));

                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Prestaciones].ToString()))
                                Cmb_Prestaciones.SelectedIndex = Cmb_Prestaciones.Items.IndexOf(
                                    Cmb_Prestaciones.Items.FindByValue(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Prestaciones].ToString()));

                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Prestaciones_Establecidas_Condiciones_Trabajo].ToString()))
                                Cmb_Prestaciones_Generales_Establecidas_Condiciones_Trabajo.SelectedIndex = Cmb_Prestaciones_Generales_Establecidas_Condiciones_Trabajo.Items.IndexOf(
                                    Cmb_Prestaciones_Generales_Establecidas_Condiciones_Trabajo.Items.FindByValue(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Prestaciones_Establecidas_Condiciones_Trabajo].ToString()));

                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Prevision_Social_Multiple].ToString()))
                                Cmb_Prevision_Social_Multiple.SelectedIndex = Cmb_Prevision_Social_Multiple.Items.IndexOf(
                                    Cmb_Prevision_Social_Multiple.Items.FindByValue(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Prevision_Social_Multiple].ToString()));

                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Prima_Dominical].ToString()))
                                Cmb_Prima_Dominical.SelectedIndex = Cmb_Prima_Dominical.Items.IndexOf(
                                    Cmb_Prima_Dominical.Items.FindByValue(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Prima_Dominical].ToString()));

                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Prima_Vacacional].ToString()))
                                Cmb_Prima_Vacacional.SelectedIndex = Cmb_Prima_Vacacional.Items.IndexOf(
                                    Cmb_Prima_Vacacional.Items.FindByValue(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Prima_Vacacional].ToString()));

                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Remuneraciones_Eventuales].ToString()))
                                Cmb_Remuneraciones_Eventuales.SelectedIndex = Cmb_Remuneraciones_Eventuales.Items.IndexOf(
                                    Cmb_Remuneraciones_Eventuales.Items.FindByValue(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Remuneraciones_Eventuales].ToString()));

                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Sueldos_Base].ToString()))
                                Cmb_Sueldos_Base.SelectedIndex = Cmb_Sueldos_Base.Items.IndexOf(
                                    Cmb_Sueldos_Base.Items.FindByValue(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Sueldos_Base].ToString()));

                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Honorarios].ToString()))
                                Cmb_Honorarios.SelectedIndex = Cmb_Honorarios.Items.IndexOf(
                                    Cmb_Honorarios.Items.FindByValue(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Honorarios].ToString()));

                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Seguros].ToString()))
                                Cmb_Seguros.SelectedIndex = Cmb_Seguros.Items.IndexOf(
                                    Cmb_Seguros.Items.FindByValue(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Seguros].ToString()));

                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Liquidacion_Indemnizacion].ToString()))
                                Cmb_Liq_Indemnizacion.SelectedIndex = Cmb_Liq_Indemnizacion.Items.IndexOf(
                                    Cmb_Liq_Indemnizacion.Items.FindByValue(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Liquidacion_Indemnizacion].ToString()));

                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Prestaciones_Retiro].ToString()))
                                Cmb_Prestaciones_Retiro.SelectedIndex = Cmb_Prestaciones_Retiro.Items.IndexOf(
                                    Cmb_Prestaciones_Retiro.Items.FindByValue(PARAMETRO[Cat_Nom_Parametros_Contables.Campo_Prestaciones_Retiro].ToString()));
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los parámetros contables. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Operación)
    /// *************************************************************************************************************************
    /// Nombre: Alta_Parametro_Contable
    /// 
    /// Descripción: Alta del Parámetro Contable.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Marzo/2012
    /// Usuario modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    private void Alta_Parametro_Contable()
    {
        Cls_Cat_Nom_Parametros_Contables_Negocio Obj_Parametros_Contables = new Cls_Cat_Nom_Parametros_Contables_Negocio();//Variable de conexión con la capa de negocios.

        try
        {
            Obj_Parametros_Contables.P_Aportaciones_IMSS = Cmb_Aportaciones_IMSS.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Aportaciones_ISSEG = Cmb_Aportaciones_ISSEG.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Cuotas_Fondo_Retiro = Cmb_Cuotas_Fondo_Ahorro.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Dietas = Cmb_Dietas.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Estimulos_Productividad_Eficiencia = Cmb_Estimulos_Productividad_Eficiencia.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Gratificaciones_Fin_Anio = Cmb_Gratificaciones_Fin_Anio.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Honorarios_Asimilados = Cmb_Honorarios_Asimilados.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Horas_Extra = Cmb_Horas_Extra.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Impuestos_Sobre_Nominas = Cmb_Impuestos_Sobre_Nominas.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Participacipaciones_Vigilancia = Cmb_Participaciones_Vigilancia.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Pensiones = Cmb_Pensiones.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Prestaciones = Cmb_Prestaciones.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Prestaciones_Establecidas_Condiciones_Trabajo = Cmb_Prestaciones_Generales_Establecidas_Condiciones_Trabajo.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Prevision_Social_Multiple = Cmb_Prevision_Social_Multiple.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Prima_Dominical = Cmb_Prima_Dominical.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Prima_Vacacional = Cmb_Prima_Vacacional.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Remuneraciones_Eventuales = Cmb_Remuneraciones_Eventuales.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Sueldos_Base = Cmb_Sueldos_Base.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Honorarios = Cmb_Honorarios.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Seguros = Cmb_Seguros.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Liquidaciones_Indemnizacion = Cmb_Liq_Indemnizacion.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Prestaciones_Retiro  = Cmb_Prestaciones_Retiro.SelectedValue.Trim();

            Obj_Parametros_Contables.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;

            if (Obj_Parametros_Contables.Alta())
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Completa');", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al dar el Alta del Parámetro Contable. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************************************************
    /// Nombre: Actualizar_Paramtro_Contable
    /// 
    /// Descripción: Actualizar Parámetro Contable.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Marzo/2012
    /// Usuario modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    private void Actualizar_Paramtro_Contable() {
        Cls_Cat_Nom_Parametros_Contables_Negocio Obj_Parametros_Contables = new Cls_Cat_Nom_Parametros_Contables_Negocio();//Variable de conexión con la capa de negocios.

        try
        {
            Obj_Parametros_Contables.P_PrimaryKey_Parametro_ID = HTxt_Clave_Primaria_Parametro_ID.Value.Trim();
            Obj_Parametros_Contables.P_Aportaciones_IMSS = Cmb_Aportaciones_IMSS.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Aportaciones_ISSEG = Cmb_Aportaciones_ISSEG.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Cuotas_Fondo_Retiro = Cmb_Cuotas_Fondo_Ahorro.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Dietas = Cmb_Dietas.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Estimulos_Productividad_Eficiencia = Cmb_Estimulos_Productividad_Eficiencia.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Gratificaciones_Fin_Anio = Cmb_Gratificaciones_Fin_Anio.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Honorarios_Asimilados = Cmb_Honorarios_Asimilados.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Horas_Extra = Cmb_Horas_Extra.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Impuestos_Sobre_Nominas = Cmb_Impuestos_Sobre_Nominas.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Participacipaciones_Vigilancia = Cmb_Participaciones_Vigilancia.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Pensiones = Cmb_Pensiones.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Prestaciones = Cmb_Prestaciones.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Prestaciones_Establecidas_Condiciones_Trabajo = Cmb_Prestaciones_Generales_Establecidas_Condiciones_Trabajo.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Prevision_Social_Multiple = Cmb_Prevision_Social_Multiple.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Prima_Dominical = Cmb_Prima_Dominical.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Prima_Vacacional = Cmb_Prima_Vacacional.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Remuneraciones_Eventuales = Cmb_Remuneraciones_Eventuales.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Sueldos_Base = Cmb_Sueldos_Base.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Honorarios = Cmb_Honorarios.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Seguros = Cmb_Seguros.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Liquidaciones_Indemnizacion = Cmb_Liq_Indemnizacion.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Prestaciones_Retiro = Cmb_Prestaciones_Retiro.SelectedValue.Trim();
            Obj_Parametros_Contables.P_Usuario_Modifico = Cls_Sessiones.Nombre_Empleado;

            if (Obj_Parametros_Contables.Actualizar())
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Completa');", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al Actualizar el Parámetro Contable. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************************************************
    /// Nombre: Eliminar_Parametro_Contable
    /// 
    /// Descripción: Eliminar el Parámetro Contable.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Marzo/2012
    /// Usuario modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    private void Eliminar_Parametro_Contable() {
        Cls_Cat_Nom_Parametros_Contables_Negocio Obj_Parametros_Contables = new Cls_Cat_Nom_Parametros_Contables_Negocio();//Variable de conexión con la capa de negocios.

        try
        {
            Obj_Parametros_Contables.P_PrimaryKey_Parametro_ID = HTxt_Clave_Primaria_Parametro_ID.Value.Trim();

            if (Obj_Parametros_Contables.Eliminar())
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Completa');", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al Eliminar el Parámetro Contable. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Validación)
    /// *************************************************************************************************************************
    /// Nombre: Validar_Solo_Exista_1_Parametro_Contable
    /// 
    /// Descripción: Validar que solo puede existir 1 parámtro contable.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Marzo/2012
    /// Usuario modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    private Boolean Validar_Solo_Exista_1_Parametro_Contable()
    {
        Cls_Cat_Nom_Parametros_Contables_Negocio Obj_Parametros_Negocio = new Cls_Cat_Nom_Parametros_Contables_Negocio();
        DataTable Dt_Parametros_Contables = null;
        Boolean Estatus = true;

        try
        {
            Dt_Parametros_Contables = Obj_Parametros_Negocio.Consultar_Parametros_Contables();

            if (Dt_Parametros_Contables is DataTable)
            {
                if (Dt_Parametros_Contables.Rows.Count > 0)
                {
                    Estatus = false;
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al validar que solo puede existir 1 parámtro contable. Error: [ " + Ex.Message + "]");
        }
        return Estatus;
    }
    /// *************************************************************************************************************************
    /// Nombre: Validar
    /// 
    /// Descripción: Valida que los datos para realizar la operación esten completos.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Marzo/2012
    /// Usuario modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    private Boolean Validar()
    {
        Cls_Cat_Nom_Parametros_Contables_Negocio Obj_Parametros_Negocio = new Cls_Cat_Nom_Parametros_Contables_Negocio();
        DataTable Dt_Parametros_Contables = null;
        Boolean Estatus = true;

        try
        {
            foreach (DropDownList Ctrl in this.Cmb_Sueldos_Base.Parent.Controls.OfType<DropDownList>())
            {
                if (Ctrl.SelectedIndex <= 0)
                {
                    Estatus = false;
                    Lbl_Mensaje_Error.Text += "+ Partida " + Ctrl.ID.Replace("Cmb_", "").Replace("_", " ") + ". <br />";
                }
            }

            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode("Falta seleccionar: <br>" + Crear_Tabla_Mostrar_Errores_Pagina(Lbl_Mensaje_Error.Text));
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al validar los datos. Error: [" + Ex.Message + "]");
        }
        return Estatus;
    }
    /// *************************************************************************************************************************
    /// Nombre: Mostrar_Mensaje
    /// 
    /// Descripción: Muestra el mensaje arrojado por el sistema para dar información al usurio del sistema.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Marzo/2012
    /// Usuario modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    private void Mostrar_Mensaje(Boolean Estado)
    {
        try
        {
            Crear_Tabla_Mostrar_Errores_Pagina(Lbl_Mensaje_Error.Text);
            Lbl_Mensaje_Error.Visible = Estado;
            Img_Error.Visible = Estado;
            if (!Estado) Lbl_Mensaje_Error.Text = String.Empty;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el mensaje de error. Error: [" + Ex.Message + "]");
        }
    }
    /// ********************************************************************************
    /// Nombre: Crear_Tabla_Mostrar_Errores_Pagina
    /// Descripción: Crea la tabla que almacenara que datos son requeridos 
    /// por el sistema
    /// Creo: Juan Alberto Hernández Negrete 
    /// Fecha Creo: 20/Octubre/2010
    /// Modifico:
    /// Fecha Modifico:
    /// Causa Modifico:
    /// ********************************************************************************
    private String Crear_Tabla_Mostrar_Errores_Pagina(String Errores)
    {
        String Tabla_Inicio = "<table style='width:100%px;font-size:10px;color:red;text-align:left;'>";
        String Tabla_Cierra = "</table>";
        String Fila_Inicia = "<tr>";
        String Fila_Cierra = "</tr>";
        String Celda_Inicia = "<td style='width:25%;text-align:left;vertical-align:top;font-size:10px;' " +
                                "onmouseover=this.style.background='#DFE8F6';this.style.color='#000000'" +
                                " onmouseout=this.style.background='#ffffff';this.style.color='red'>";
        String Celda_Cierra = "</td>";
        char[] Separador = { '+' };
        String[] _Errores_Temp = Errores.Replace("<br>", "").Split(Separador);
        String[] _Errores = new String[(_Errores_Temp.Length - 1)];
        String Tabla;
        String Filas = "";
        String Celdas = "";
        int Contador_Celdas = 1;
        for (int i = 0; i < _Errores.Length; i++) _Errores[i] = _Errores_Temp[i + 1];

        Tabla = Tabla_Inicio;
        for (int i = 0; i < _Errores.Length; i++)
        {
            if (Contador_Celdas == 5)
            {
                Filas += Fila_Inicia;
                Filas += Celdas;
                Filas += Fila_Cierra;
                Celdas = "";
                Contador_Celdas = 0;
                i = i - 1;
            }
            else
            {
                Celdas += Celda_Inicia;
                Celdas += "<b style='font-size:12px;'>+</b>" + _Errores[i];
                Celdas += Celda_Cierra;
            }
            Contador_Celdas = Contador_Celdas + 1;
        }
        if (_Errores.Length < 5 || Contador_Celdas > 0)
        {
            Filas += Fila_Inicia;
            Filas += Celdas;
            Filas += Fila_Cierra;
        }
        Tabla += Filas;
        Tabla += Tabla_Cierra;
        return Tabla;
    }
    #endregion

    #endregion

    #region (Eventos)
    /// *************************************************************************************************************************
    /// Nombre: Btn_Nuevo_Click
    /// 
    /// Descripción: Alta Parámetro Contable.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Marzo/2012
    /// Usuario modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    protected void Btn_Nuevo_Click(Object sender, EventArgs e)
    {
        try
        {
            if (Btn_Nuevo.ToolTip.Trim().ToUpper().Equals("NUEVO"))
            {
                Limpiar_Controles();
                Habilitar_Controles("Nuevo");
            }
            else if (Btn_Nuevo.ToolTip.Trim().ToUpper().Equals("DAR DE ALTA"))
            {
                if (Validar())
                {
                    Alta_Parametro_Contable();
                    Configuración_Inicial();
                }
                else {
                    Mostrar_Mensaje(true);
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Mostrar_Mensaje(true);
        }
    }
    /// *************************************************************************************************************************
    /// Nombre: Btn_Modificar_Click
    /// 
    /// Descripción: Actualiza los Parámetros Contables.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Marzo/2012
    /// Usuario modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    protected void Btn_Modificar_Click(Object sender, EventArgs e) {
        try
        {
            if (Btn_Modificar.ToolTip.Trim().ToUpper().Equals("MODIFICAR"))
            {
                Habilitar_Controles("Modificar");
            }
            else if (Btn_Modificar.ToolTip.Trim().ToUpper().Equals("ACTUALIZAR"))
            {
                if (Validar())
                {
                    Actualizar_Paramtro_Contable();
                    Configuración_Inicial();
                }
                else
                {
                    Mostrar_Mensaje(true);
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Mostrar_Mensaje(true);
        }
    }
    /// *************************************************************************************************************************
    /// Nombre: Btn_Eliminar_Click
    /// 
    /// Descripción: Elimina el Parámetro Contable.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Marzo/2012
    /// Usuario modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    protected void Btn_Eliminar_Click(Object sender, EventArgs e)
    {
        try
        {
            if (Btn_Eliminar.ToolTip.Trim().ToUpper().Equals("ELIMINAR"))
            {
                Eliminar_Parametro_Contable();
                Configuración_Inicial();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Mostrar_Mensaje(true);
        }
    }
    /// *************************************************************************************************************************
    /// Nombre: Btn_Salir_Click
    /// 
    /// Descripción: Cancela la operación actual o permite salir de la página actual.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Marzo/2012
    /// Usuario modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    protected void Btn_Salir_Click(Object sender, EventArgs e) {
        try
        {
            if (Btn_Salir.ToolTip.Trim().ToUpper().Equals("INICIO"))
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Configuración_Inicial();//Habilita los controles para la siguiente operación del usuario en el catálogo
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Mostrar_Mensaje(true);           
        }
    }
    #endregion

}
