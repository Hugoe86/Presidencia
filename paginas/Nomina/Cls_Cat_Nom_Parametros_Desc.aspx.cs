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
using Presidencia.Parametros_Descuentos.Negocio;
using Presidencia.Sessiones;
using Presidencia.Cat_Nom_Percepciones_Deducciones_Opcional.Negocios;

public partial class paginas_Nomina_Cls_Cat_Nom_Parametros_Desc : System.Web.UI.Page
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
    protected void Configuración_Inicial()
    {

        try
        {
            Cargar_Ctrl();
            Limpiar_Controles();
            Habilitar_Controles("Inicial");
            Consultar_Parametros_Descuentos();
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
    protected void Limpiar_Controles()
    {
        try
        {
            foreach (DropDownList Ctrl in this.Cmb_Concepto_PMO_Mercados.Parent.Controls.OfType<DropDownList>())
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
                    Btn_Nuevo.Visible = Validar_Solo_Exista_1_Parametro_Descuento();
                    Btn_Modificar.Visible = !Validar_Solo_Exista_1_Parametro_Descuento();
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

            foreach (DropDownList Ctrl in this.Cmb_Concepto_PMO_Mercados.Parent.Controls.OfType<DropDownList>())
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
    /// Nombre: Consultar_Deducciones
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
    protected void Consultar_Deducciones(DropDownList Ctrl)
    {
        Cls_Cat_Nom_Percepciones_Deducciones_Business Obj_Conceptos = new Cls_Cat_Nom_Percepciones_Deducciones_Business();
        DataTable Dt_Resultado = null;

        try
        {
            Obj_Conceptos.P_ESTATUS = "ACTIVO";
            Obj_Conceptos.P_TIPO = "DEDUCCION";
            Ctrl.DataSource = Obj_Conceptos.Consultar_Percepciones_Deducciones_General();
            Ctrl.DataTextField = "NOMBRE_CONCEPTO";
            Ctrl.DataValueField = Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID;
            Ctrl.DataBind();

            Ctrl.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Ctrl.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cosultas los conceptos. Error: [" + Ex.Message + "]");
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
            foreach (DropDownList Ctrl in this.Cmb_Concepto_PMO_Mercados.Parent.Controls.OfType<DropDownList>())
                Consultar_Deducciones(Ctrl);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cargar los controles de la página. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************************************************
    /// Nombre: Consultar_Parametros_Descuentos
    /// 
    /// Descripción: Consultamos los Parámetros Descuentos.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Marzo/2012
    /// Usuario modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    private void Consultar_Parametros_Descuentos()
    {
        Cls_Cat_Nom_Parametros_Desc_Negocio Obj_Parametros = new Cls_Cat_Nom_Parametros_Desc_Negocio();
        DataTable Dt_Parametros = null;

        try
        {
            Dt_Parametros = Obj_Parametros.Consultar();

            if (Dt_Parametros is DataTable) {
                var parametro = from item_parametro in Dt_Parametros.AsEnumerable()
                                select new {
                                    Parametro_ID = item_parametro.Field<String>(Cat_Nom_Parametros_Desc.Campo_Parametro_ID),
                                    Desc_PMO_Tesoreria = item_parametro.Field<String>(Cat_Nom_Parametros_Desc.Campo_Desc_PMO_Tesoreria),
                                    Desc_PMO_Mercados = item_parametro.Field<String>(Cat_Nom_Parametros_Desc.Campo_Desc_PMO_Mercados),
                                    Desc_PMO_Corto_Plazo = item_parametro.Field<String>(Cat_Nom_Parametros_Desc.Campo_Desc_PMO_Corto_Plazo),
                                    Desc_PMO_Pago_Aval = item_parametro.Field<String>(Cat_Nom_Parametros_Desc.Campo_Desc_PMO_Pago_Aval),
                                    Desc_PMO_IMUVI = item_parametro.Field<String>(Cat_Nom_Parametros_Desc.Campo_Desc_PMO_IMUVI),
                                    Desc_Llamadas_Tel = item_parametro.Field<String>(Cat_Nom_Parametros_Desc.Campo_Desc_Llamadas_Tel),
                                    Desc_Perdida_Equipo = item_parametro.Field<String>(Cat_Nom_Parametros_Desc.Campo_Desc_Perdida_Equipo),
                                    Desc_Otros_Fijos = item_parametro.Field<String>(Cat_Nom_Parametros_Desc.Campo_Desc_Otros_Fijos),
                                    Desc_Otros_Var = item_parametro.Field<String>(Cat_Nom_Parametros_Desc.Campo_Desc_Otros_Variables),
                                    Desc_Pago_Agua= item_parametro.Field<String>(Cat_Nom_Parametros_Desc.Campo_Desc_Agua),
                                    Desc_Pago_Predial = item_parametro.Field<String>(Cat_Nom_Parametros_Desc.Campo_Desc_Pago_Predial)
                                };

                if (parametro != null) {
                    foreach (var item in parametro) {

                        HTxt_Clave_Primaria_Parametro_ID.Value = item.Parametro_ID;

                        Cmb_Concepto_PMO_Tesoreria.SelectedIndex = Cmb_Concepto_PMO_Tesoreria.Items.IndexOf(Cmb_Concepto_PMO_Tesoreria.Items.FindByValue(
                            item.Desc_PMO_Tesoreria));

                        Cmb_Concepto_PMO_Mercados.SelectedIndex = Cmb_Concepto_PMO_Mercados.Items.IndexOf(Cmb_Concepto_PMO_Mercados.Items.FindByValue(
                            item.Desc_PMO_Mercados));

                        Cmb_Concepto_PMO_Pago_Aval.SelectedIndex = Cmb_Concepto_PMO_Pago_Aval.Items.IndexOf(Cmb_Concepto_PMO_Pago_Aval.Items.FindByValue(
                            item.Desc_PMO_Pago_Aval));

                        Cmb_Concepto_PMO_Corto_Plazo.SelectedIndex = Cmb_Concepto_PMO_Corto_Plazo.Items.IndexOf(Cmb_Concepto_PMO_Corto_Plazo.Items.FindByValue(
                            item.Desc_PMO_Corto_Plazo));

                        Cmb_Concepto_PMO_IMUVI.SelectedIndex = Cmb_Concepto_PMO_IMUVI.Items.IndexOf(Cmb_Concepto_PMO_IMUVI.Items.FindByValue(
                            item.Desc_PMO_IMUVI));

                        Cmb_Concepto_Llamadas_Tel.SelectedIndex = Cmb_Concepto_Llamadas_Tel.Items.IndexOf(Cmb_Concepto_Llamadas_Tel.Items.FindByValue(
                            item.Desc_Llamadas_Tel));

                        Cmb_Concepto_Perdida_Equipo.SelectedIndex = Cmb_Concepto_Perdida_Equipo.Items.IndexOf(Cmb_Concepto_Perdida_Equipo.Items.FindByValue(
                            item.Desc_Perdida_Equipo));

                        Cmb_Concepto_Otros_Descuentos_Fijos.SelectedIndex = Cmb_Concepto_Otros_Descuentos_Fijos.Items.IndexOf(Cmb_Concepto_Otros_Descuentos_Fijos.Items.FindByValue(
                            item.Desc_Otros_Fijos));

                        Cmb_Concepto_Otros_Descuentos_Variables.SelectedIndex = Cmb_Concepto_Otros_Descuentos_Variables.Items.IndexOf(Cmb_Concepto_Otros_Descuentos_Variables.Items.FindByValue(
                            item.Desc_Otros_Var));

                        Cmb_Concepto_Pago_Agua.SelectedIndex = Cmb_Concepto_Pago_Agua.Items.IndexOf(Cmb_Concepto_Pago_Agua.Items.FindByValue(
                            item.Desc_Pago_Agua));

                        Cmb_Concepto_Pago_Predial.SelectedIndex = Cmb_Concepto_Pago_Predial.Items.IndexOf(Cmb_Concepto_Pago_Predial.Items.FindByValue(
                            item.Desc_Pago_Predial.Trim()));
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los parámetros Descuentos. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Operación)
    /// *************************************************************************************************************************
    /// Nombre: Alta
    /// 
    /// Descripción: Alta del Parámetro Descuento.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Marzo/2012
    /// Usuario modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    private void Alta()
    {
        Cls_Cat_Nom_Parametros_Desc_Negocio Obj_Parametro_Descuento = new Cls_Cat_Nom_Parametros_Desc_Negocio();//Variable de conexión con la capa de negocios.

        try
        {
            Obj_Parametro_Descuento.P_Desc_PMO_Mercados = Cmb_Concepto_PMO_Mercados.SelectedValue.Trim();
            Obj_Parametro_Descuento.P_PMO_Tesoreria = Cmb_Concepto_PMO_Tesoreria.SelectedValue.Trim();
            Obj_Parametro_Descuento.P_PMO_Corto_Plazo = Cmb_Concepto_PMO_Corto_Plazo.SelectedValue.Trim();
            Obj_Parametro_Descuento.P_PMO_Pago_Aval = Cmb_Concepto_PMO_Pago_Aval.SelectedValue.Trim();
            Obj_Parametro_Descuento.P_PMO_IMUVI = Cmb_Concepto_PMO_IMUVI.SelectedValue.Trim();
            Obj_Parametro_Descuento.P_Desc_Llamadas_Tel = Cmb_Concepto_Llamadas_Tel.SelectedValue.Trim();
            Obj_Parametro_Descuento.P_Desc_Perdida_Equipo = Cmb_Concepto_Perdida_Equipo.SelectedValue.Trim();
            Obj_Parametro_Descuento.P_Desc_Otros_Fijos= Cmb_Concepto_Otros_Descuentos_Fijos.SelectedValue.Trim();
            Obj_Parametro_Descuento.P_Desc_Otros_Variables = Cmb_Concepto_Otros_Descuentos_Variables.SelectedValue.Trim();
            Obj_Parametro_Descuento.P_Desc_Agua = Cmb_Concepto_Pago_Agua.SelectedValue.Trim();
            Obj_Parametro_Descuento.P_Desc_Pago_Predial = Cmb_Concepto_Pago_Predial.SelectedValue.Trim();

            if (Obj_Parametro_Descuento.Alta())
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Completa');", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al dar el Alta del Parámetro Descuento. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************************************************
    /// Nombre: Actualizar_Paramtro_Descuento
    /// 
    /// Descripción: Actualizar Parámetro Descuento.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Marzo/2012
    /// Usuario modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    private void Actualizar_Paramtro_Descuento()
    {
        Cls_Cat_Nom_Parametros_Desc_Negocio Obj_Parametro_Descuento = new Cls_Cat_Nom_Parametros_Desc_Negocio();//Variable de conexión con la capa de negocios.

        try
        {
            Obj_Parametro_Descuento.P_Parametro_ID = HTxt_Clave_Primaria_Parametro_ID.Value.Trim();

            Obj_Parametro_Descuento.P_Desc_PMO_Mercados = Cmb_Concepto_PMO_Mercados.SelectedValue.Trim();
            Obj_Parametro_Descuento.P_PMO_Tesoreria = Cmb_Concepto_PMO_Tesoreria.SelectedValue.Trim();
            Obj_Parametro_Descuento.P_PMO_Corto_Plazo = Cmb_Concepto_PMO_Corto_Plazo.SelectedValue.Trim();
            Obj_Parametro_Descuento.P_PMO_Pago_Aval = Cmb_Concepto_PMO_Pago_Aval.SelectedValue.Trim();
            Obj_Parametro_Descuento.P_PMO_IMUVI = Cmb_Concepto_PMO_IMUVI.SelectedValue.Trim();
            Obj_Parametro_Descuento.P_Desc_Llamadas_Tel = Cmb_Concepto_Llamadas_Tel.SelectedValue.Trim();
            Obj_Parametro_Descuento.P_Desc_Perdida_Equipo = Cmb_Concepto_Perdida_Equipo.SelectedValue.Trim();
            Obj_Parametro_Descuento.P_Desc_Otros_Fijos = Cmb_Concepto_Otros_Descuentos_Fijos.SelectedValue.Trim();
            Obj_Parametro_Descuento.P_Desc_Otros_Variables = Cmb_Concepto_Otros_Descuentos_Variables.SelectedValue.Trim();
            Obj_Parametro_Descuento.P_Desc_Agua = Cmb_Concepto_Pago_Agua.SelectedValue.Trim();
            Obj_Parametro_Descuento.P_Desc_Pago_Predial = Cmb_Concepto_Pago_Predial.SelectedValue.Trim();

            if (Obj_Parametro_Descuento.Modificar())
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Completa');", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al Actualizar el Parámetro Descuento. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************************************************
    /// Nombre: Eliminar_Parametro_Descuento
    /// 
    /// Descripción: Eliminar el Parámetro Descuento.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Marzo/2012
    /// Usuario modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    private void Eliminar_Parametro_Descuento()
    {
        Cls_Cat_Nom_Parametros_Desc_Negocio Obj_Parametro_Descuento = new Cls_Cat_Nom_Parametros_Desc_Negocio();//Variable de conexión con la capa de negocios.

        try
        {
            Obj_Parametro_Descuento.P_Parametro_ID = HTxt_Clave_Primaria_Parametro_ID.Value.Trim();

            if (Obj_Parametro_Descuento.Eliminar())
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Completa');", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al Eliminar el Parámetro Descuento. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Validación)
    /// *************************************************************************************************************************
    /// Nombre: Validar_Solo_Exista_1_Parametro_Descuento
    /// 
    /// Descripción: Validar que solo puede existir 1 parámtro descuento.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Marzo/2012
    /// Usuario modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    private Boolean Validar_Solo_Exista_1_Parametro_Descuento()
    {
        Cls_Cat_Nom_Parametros_Desc_Negocio Obj_Parametro_Descuento = new Cls_Cat_Nom_Parametros_Desc_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Parametros_Descuentos = null;
        Boolean Estatus = true;

        try
        {
            Dt_Parametros_Descuentos = Obj_Parametro_Descuento.Consultar();

            if (Dt_Parametros_Descuentos is DataTable)
            {
                if (Dt_Parametros_Descuentos.Rows.Count > 0)
                {
                    Estatus = false;
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al validar que solo puede existir 1 parámtro descuento. Error: [ " + Ex.Message + "]");
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
        Boolean Estatus = true;

        try
        {
            foreach (DropDownList Ctrl in this.Cmb_Concepto_PMO_Mercados.Parent.Controls.OfType<DropDownList>())
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
    /// Descripción: Alta Parámetro Descuento.
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
                    Alta();
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
    /// Nombre: Btn_Modificar_Click
    /// 
    /// Descripción: Actualiza los Parámetros Descuento.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Marzo/2012
    /// Usuario modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    protected void Btn_Modificar_Click(Object sender, EventArgs e)
    {
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
                    Actualizar_Paramtro_Descuento();
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
    /// Descripción: Elimina el Parámetro Descuento.
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
                Eliminar_Parametro_Descuento();
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
    protected void Btn_Salir_Click(Object sender, EventArgs e)
    {
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
