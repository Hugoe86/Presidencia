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
using System.Globalization;
using Presidencia.Programas.Negocios;
using Presidencia.Tipos_Contratos.Negocios;
using Presidencia.Puestos.Negocios;
using Presidencia.Escolaridad.Negocios;
using Presidencia.Sindicatos.Negocios;
using Presidencia.Turnos.Negocios;
using Presidencia.Zona_Economica.Negocios;
using Presidencia.Tipo_Trabajador.Negocios;
using System.Text.RegularExpressions;
using Presidencia.Vacaciones_Empleado.Negocio;
using Presidencia.Tipos_Nominas.Negocios;
using Presidencia.Utilidades_Nomina;
using Presidencia.Bancos_Nomina.Negocio;
using Presidencia.Catalogo_SAP_Fuente_Financiamiento.Negocio;
using Presidencia.Catalogo_Compras_Proyectos_Programas.Negocio;
using Presidencia.Catalogo_Compras_Partidas.Negocio;
using Presidencia.Percepciones_Deducciones_Fijas.Negocio;
using Presidencia.Prestamos.Negocio;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Dependencias.Negocios;
using Presidencia.Roles.Negocio;
using Presidencia.Areas.Negocios;
using Presidencia.Empleados.Negocios;
using Presidencia.Orden_Judicial.Negocio;

public partial class paginas_Nomina_Frm_Cat_Nom_Tab_Orden_Judicial : System.Web.UI.Page
{
    #region (Load/Init)
    /// *******************************************************************************************************
    /// Nombre: Page_Load
    /// 
    /// Descripción: Carga y habilita la configuración inicial de la página.
    /// 
    /// Parámetros:No Áplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 8/Agosto/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *******************************************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack) {
                Configuracion_Inicial();//Habilitamos la configuración inicial de la pantalla.
            }

            Mostrar_Mensaje(String.Empty, false);//Ocultamos los mensajes mostrados en pantalla.
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje(Ex.Message, true);
        }
    }
    #endregion

    #region (Métodos)

    #region (Generales)
    /// *******************************************************************************************************
    /// Nombre: Configuracion_Inicial
    /// 
    /// Descripción: Carga y habilita la configuración inicial de la página.
    /// 
    /// Parámetros:No Áplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 8/Agosto/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *******************************************************************************************************
    private void Configuracion_Inicial()
    {
        try
        {
            Consultar_Dependencias_Busqueda();
            Consultar_Roles_Busqueda();
            Consultar_Escolaridad_Busqueda();
            Consultar_Puestos_Busqueda();
            Consultar_Sindicatos_Busqueda();
            Consultar_Tipo_Trabajador_Busqueda();
            Consultar_Tipos_Contratos_Busqueda();
            Consultar_Tipos_Nomina_Busqueda();
            Consultar_Turnos_Busqueda();
            Consultar_Zonas_Busqueda();

            Habilitar_Controles("Inicial");
            Limpiar_Controles();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cargar la configuracion inicial de la página. Error: [" + Ex.Message + "]");
        }
    }
    /// *******************************************************************************************************
    /// Nombre: Habilitar_Controles
    /// 
    /// Descripción: Habilita la configuración inicial de la página.
    /// 
    /// Parámetros:No Áplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 8/Agosto/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *******************************************************************************************************
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
                    //Campos de Busqueda
                    Btn_Busqueda.Enabled = true;
                    //Campo de Validacion
                    Mostrar_Mensaje(String.Empty, false);
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
                    Btn_Busqueda.Enabled = true;
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
                    Btn_Busqueda.Enabled = true;
                    break;
            }

            Txt_Orden_Judicial_ID.Enabled = false;
            Txt_Empleado.Enabled = false;
            Txt_Nombre_Beneficiario.Enabled = Habilitado;

            Cmb_Tipo_Desc_Orden_Judicial_Sueldo.Enabled = Habilitado;
            Txt_Cantidad_Porcentaje_Sueldo_OJ.Enabled = Habilitado;
            Cmb_Neto_Bruto_Sueldo_OJ.Enabled = Habilitado;

            Cmb_Tipo_Desc_Orden_Judicial_Aguinaldo.Enabled = Habilitado;
            Txt_Cantidad_Porcentaje_Aguinaldo_OJ.Enabled = Habilitado;
            Cmb_Neto_Bruto_Aguinaldo_OJ.Enabled = Habilitado;

            Cmb_Tipo_Desc_Orden_Judicial_PV.Enabled = Habilitado;
            Txt_Cantidad_Porcentaje_PV_OJ.Enabled = Habilitado;
            Cmb_Neto_Bruto_PV_OJ.Enabled = Habilitado;

            Cmb_Tipo_Desc_Orden_Judicial_Indemnizacion.Enabled = Habilitado;
            Txt_Cantidad_Porcentaje_Indemnizacion_OJ.Enabled = Habilitado;
            Cmb_Neto_Bruto_Indemnizacion_OJ.Enabled = Habilitado;

            Grid_Tabulador_Orden_Judicial.Enabled = !Habilitado;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    /// *******************************************************************************************************
    /// Nombre: Limpiar_Controles
    /// 
    /// Descripción: Limpia los controles de la página a un estado inicial de la página.
    /// 
    /// Parámetros:No Áplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 8/Agosto/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *******************************************************************************************************
    private void Limpiar_Controles()
    {
        Txt_Orden_Judicial_ID.Text = String.Empty;
        Txt_Empleado.Text = String.Empty;
        HTxt_Empleado.Value = String.Empty;
        Txt_Nombre_Beneficiario.Text = String.Empty;

        Cmb_Tipo_Desc_Orden_Judicial_Sueldo.SelectedIndex = (-1);
        Txt_Cantidad_Porcentaje_Sueldo_OJ.Text = "";
        Cmb_Neto_Bruto_Sueldo_OJ.SelectedIndex = (-1);

        Cmb_Tipo_Desc_Orden_Judicial_Aguinaldo.SelectedIndex = (-1);
        Txt_Cantidad_Porcentaje_Aguinaldo_OJ.Text = "";
        Cmb_Neto_Bruto_Aguinaldo_OJ.SelectedIndex = (-1);

        Cmb_Tipo_Desc_Orden_Judicial_PV.SelectedIndex = (-1);
        Txt_Cantidad_Porcentaje_PV_OJ.Text = "";
        Cmb_Neto_Bruto_PV_OJ.SelectedIndex = (-1);

        Cmb_Tipo_Desc_Orden_Judicial_Indemnizacion.SelectedIndex = (-1);
        Txt_Cantidad_Porcentaje_Indemnizacion_OJ.Text = "";
        Cmb_Neto_Bruto_Indemnizacion_OJ.SelectedIndex = (-1);

        Grid_Tabulador_Orden_Judicial.SelectedIndex = (-1);
    }
    #endregion

    #region (Operacion)
    /// *******************************************************************************************************
    /// Nombre: Alta
    /// 
    /// Descripción: Invoca el alta de un registro.
    /// 
    /// Parámetros:No Áplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 8/Agosto/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *******************************************************************************************************
    private void Alta()
    {
        Cls_Cat_Nom_Tab_Orden_Judicial_Negocio Obj_Orden_Judicial = new Cls_Cat_Nom_Tab_Orden_Judicial_Negocio();//Variable de conexión con la capa de negocios.
       
        try
        {
            Obj_Orden_Judicial.P_Empleado_ID = HTxt_Empleado.Value.Trim();
            Obj_Orden_Judicial.P_Beneficiario = Txt_Nombre_Beneficiario.Text.Trim();

            Obj_Orden_Judicial.P_Tipo_Descuento_Orden_Judicial_Sueldo = Cmb_Tipo_Desc_Orden_Judicial_Sueldo.SelectedItem.Text.Trim();
            Obj_Orden_Judicial.P_Cantidad_Porcentaje_Sueldo = Convert.ToDouble(Txt_Cantidad_Porcentaje_Sueldo_OJ.Text.Trim());
            Obj_Orden_Judicial.P_Bruto_Neto_Orden_Judicial_Sueldo = Cmb_Neto_Bruto_Sueldo_OJ.SelectedItem.Text.Trim();

            Obj_Orden_Judicial.P_Tipo_Descuento_Orden_Judicial_Aguinaldo = Cmb_Tipo_Desc_Orden_Judicial_Aguinaldo.SelectedItem.Text.Trim();
            Obj_Orden_Judicial.P_Cantidad_Porcentaje_Aguinaldo = Convert.ToDouble(Txt_Cantidad_Porcentaje_Aguinaldo_OJ.Text.Trim());
            Obj_Orden_Judicial.P_Bruto_Neto_Orden_Aguinaldo = Cmb_Neto_Bruto_Aguinaldo_OJ.SelectedItem.Text.Trim();

            Obj_Orden_Judicial.P_Tipo_Descuento_Orden_Judicial_Prima_Vacacional = Cmb_Tipo_Desc_Orden_Judicial_PV.SelectedItem.Text.Trim();
            Obj_Orden_Judicial.P_Cantidad_Porcentaje_Prima_Vacacional = Convert.ToDouble(Txt_Cantidad_Porcentaje_PV_OJ.Text.Trim());
            Obj_Orden_Judicial.P_Bruto_Neto_Orden_Prima_Vacacional= Cmb_Neto_Bruto_PV_OJ.SelectedItem.Text.Trim();

            Obj_Orden_Judicial.P_Tipo_Descuento_Orden_Judicial_Indemnizacion = Cmb_Tipo_Desc_Orden_Judicial_Indemnizacion.SelectedItem.Text.Trim();
            Obj_Orden_Judicial.P_Cantidad_Porcentaje_Indemnizacion = Convert.ToDouble(Txt_Cantidad_Porcentaje_Indemnizacion_OJ.Text.Trim());
            Obj_Orden_Judicial.P_Bruto_Neto_Orden_Indemnizacion = Cmb_Neto_Bruto_Indemnizacion_OJ.SelectedItem.Text.Trim();

            Obj_Orden_Judicial.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;

            if (Obj_Orden_Judicial.Alta_Parametro_Orden_Judicial())
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Alta",
                    "alert('Operación Completa');", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al invocar el alta de un parámtro de orden judicial al empleado. Error: [" + Ex.Message + "]");
        }
    }
    /// *******************************************************************************************************
    /// Nombre: Actualizar
    /// 
    /// Descripción: Invoca la modificación de un registro.
    /// 
    /// Parámetros:No Áplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 8/Agosto/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *******************************************************************************************************
    private void Actualizar()
    {
        Cls_Cat_Nom_Tab_Orden_Judicial_Negocio Obj_Orden_Judicial = new Cls_Cat_Nom_Tab_Orden_Judicial_Negocio();//Variable de conexión con la capa de negocios.

        try
        {
            Obj_Orden_Judicial.P_Orden_Judicial_ID = Txt_Orden_Judicial_ID.Text.Trim();
            Obj_Orden_Judicial.P_Empleado_ID = HTxt_Empleado.Value.Trim();
            Obj_Orden_Judicial.P_Beneficiario = Txt_Nombre_Beneficiario.Text.Trim();

            Obj_Orden_Judicial.P_Tipo_Descuento_Orden_Judicial_Sueldo = Cmb_Tipo_Desc_Orden_Judicial_Sueldo.SelectedItem.Text.Trim();
            Obj_Orden_Judicial.P_Cantidad_Porcentaje_Sueldo = Convert.ToDouble(Txt_Cantidad_Porcentaje_Sueldo_OJ.Text.Trim());
            Obj_Orden_Judicial.P_Bruto_Neto_Orden_Judicial_Sueldo = Cmb_Neto_Bruto_Sueldo_OJ.SelectedItem.Text.Trim();

            Obj_Orden_Judicial.P_Tipo_Descuento_Orden_Judicial_Aguinaldo = Cmb_Tipo_Desc_Orden_Judicial_Aguinaldo.SelectedItem.Text.Trim();
            Obj_Orden_Judicial.P_Cantidad_Porcentaje_Aguinaldo = Convert.ToDouble(Txt_Cantidad_Porcentaje_Aguinaldo_OJ.Text.Trim());
            Obj_Orden_Judicial.P_Bruto_Neto_Orden_Aguinaldo = Cmb_Neto_Bruto_Aguinaldo_OJ.SelectedItem.Text.Trim();

            Obj_Orden_Judicial.P_Tipo_Descuento_Orden_Judicial_Prima_Vacacional = Cmb_Tipo_Desc_Orden_Judicial_PV.SelectedItem.Text.Trim();
            Obj_Orden_Judicial.P_Cantidad_Porcentaje_Prima_Vacacional = Convert.ToDouble(Txt_Cantidad_Porcentaje_PV_OJ.Text.Trim());
            Obj_Orden_Judicial.P_Bruto_Neto_Orden_Prima_Vacacional = Cmb_Neto_Bruto_PV_OJ.SelectedItem.Text.Trim();

            Obj_Orden_Judicial.P_Tipo_Descuento_Orden_Judicial_Indemnizacion = Cmb_Tipo_Desc_Orden_Judicial_Indemnizacion.SelectedItem.Text.Trim();
            Obj_Orden_Judicial.P_Cantidad_Porcentaje_Indemnizacion = Convert.ToDouble(Txt_Cantidad_Porcentaje_Indemnizacion_OJ.Text.Trim());
            Obj_Orden_Judicial.P_Bruto_Neto_Orden_Indemnizacion = Cmb_Neto_Bruto_Indemnizacion_OJ.SelectedItem.Text.Trim();

            Obj_Orden_Judicial.P_Usuario_Modifico = Cls_Sessiones.Nombre_Empleado;

            if (Obj_Orden_Judicial.Modificar_Parametro_Orden_Judicial())
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Actualizar",
                    "alert('Operación Completa');", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al invocar la modificación de un parámetro de orden judicial al empleado. Error: [" + Ex.Message + "]");
        }
    }
    /// *******************************************************************************************************
    /// Nombre: Eliminar
    /// 
    /// Descripción: Invoca la baja de un registro.
    /// 
    /// Parámetros:No Áplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 8/Agosto/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *******************************************************************************************************
    private void Eliminar()
    {
        Cls_Cat_Nom_Tab_Orden_Judicial_Negocio Obj_Orden_Judicial = new Cls_Cat_Nom_Tab_Orden_Judicial_Negocio();//Variable de conexión con la capa de negocios.

        try
        {
            Obj_Orden_Judicial.P_Orden_Judicial_ID = Txt_Orden_Judicial_ID.Text.Trim();

            if (Obj_Orden_Judicial.Eliminar_Parametro_Orden_Judicial())
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Eliminar",
                    "alert('Operación Completa');", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al invocar la baja de un parámetro de orden judicial al empleado. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Consultas)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Empleados_Avanzada
    /// 
    /// DESCRIPCION : Ejecuta la búsqueda de empleados
    /// 
    /// CREO        : Juan Alberto Hernández Negrete.
    /// FECHA_CREO  : 08/Agosto/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Empleados_Avanzada()
    {
        Cls_Cat_Empleados_Negocios Rs_Consulta_Ca_Empleados = new Cls_Cat_Empleados_Negocios(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Empleados; //Variable que obtendra los datos de la consulta 

        try
        {
            if (!string.IsNullOrEmpty(Txt_Busqueda_No_Empleado.Text.Trim())) Rs_Consulta_Ca_Empleados.P_No_Empleado = Txt_Busqueda_No_Empleado.Text.Trim();
            if (!string.IsNullOrEmpty(Txt_Busqueda_RFC.Text.Trim())) Rs_Consulta_Ca_Empleados.P_RFC = Txt_Busqueda_RFC.Text.Trim();
            if (Cmb_Busqueda_Estatus.SelectedIndex > 0) Rs_Consulta_Ca_Empleados.P_Estatus = Cmb_Busqueda_Estatus.SelectedItem.Text.Trim();
            if (!string.IsNullOrEmpty(Txt_Busqueda_Nombre_Empleado.Text.Trim()) && (Txt_Busqueda_Nombre_Empleado.Text.Trim().Length > 5)) Rs_Consulta_Ca_Empleados.P_Nombre = Txt_Busqueda_Nombre_Empleado.Text.Trim();
            if (Cmb_Busqueda_Rol.SelectedIndex > 0) Rs_Consulta_Ca_Empleados.P_Rol_ID = Cmb_Busqueda_Rol.SelectedValue.Trim();
            if (Cmb_Busqueda_Dependencia.SelectedIndex > 0) Rs_Consulta_Ca_Empleados.P_Dependencia_ID = Cmb_Busqueda_Dependencia.SelectedValue.Trim();
            if (Cmb_Busqueda_Tipo_Contrato.SelectedIndex > 0) Rs_Consulta_Ca_Empleados.P_Tipo_Contrato_ID = Cmb_Busqueda_Tipo_Contrato.SelectedValue.Trim();
            if (Cmb_Busqueda_Areas.SelectedIndex > 0) Rs_Consulta_Ca_Empleados.P_Area_ID = Cmb_Busqueda_Areas.SelectedValue.Trim();
            if (Cmb_Busqueda_Puesto.SelectedIndex > 0) Rs_Consulta_Ca_Empleados.P_Puesto_ID = Cmb_Busqueda_Puesto.SelectedValue.Trim();
            if (Cmb_Busqueda_Escolaridad.SelectedIndex > 0) Rs_Consulta_Ca_Empleados.P_Escolaridad_ID = Cmb_Busqueda_Escolaridad.SelectedValue.Trim();
            if (Cmb_Busqueda_Sindicato.SelectedIndex > 0) Rs_Consulta_Ca_Empleados.P_Sindicado_ID = Cmb_Busqueda_Sindicato.SelectedValue.Trim();
            if (Cmb_Busqueda_Turno.SelectedIndex > 0) Rs_Consulta_Ca_Empleados.P_Turno_ID = Cmb_Busqueda_Turno.SelectedValue.Trim();
            if (Cmb_Busqueda_Zona.SelectedIndex > 0) Rs_Consulta_Ca_Empleados.P_Zona_ID = Cmb_Busqueda_Zona.SelectedValue.Trim();
            if (Cmb_Busqueda_Tipo_Trabajador.SelectedIndex > 0) Rs_Consulta_Ca_Empleados.P_Tipo_Trabajador_ID = Cmb_Busqueda_Tipo_Trabajador.SelectedValue.Trim();
            if (Cmb_Busqueda_Tipo_Nomina.SelectedIndex > 0) Rs_Consulta_Ca_Empleados.P_Tipo_Nomina_ID = Cmb_Busqueda_Tipo_Nomina.SelectedValue.Trim();

            //if (!string.IsNullOrEmpty(Txt_Busqueda_Fecha_Inicio.Text.Trim()) && !string.IsNullOrEmpty(Txt_Busqueda_Fecha_Fin.Text.Trim()))
            //{
            //    if (Validar_Formato_Fecha(Txt_Busqueda_Fecha_Inicio.Text.Trim()) && Validar_Formato_Fecha(Txt_Busqueda_Fecha_Fin.Text.Trim()))
            //    {
            //        Rs_Consulta_Ca_Empleados.P_Fecha_Inicio_Busqueda = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Busqueda_Fecha_Inicio.Text.Trim()));
            //        Rs_Consulta_Ca_Empleados.P_Fecha_Fin_Busqueda = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Busqueda_Fecha_Fin.Text.Trim()));
            //    }
            //}

            Dt_Empleados = Rs_Consulta_Ca_Empleados.Consulta_Empleados_General(); //Consulta todos los Empleados que coindican con lo proporcionado por el usuario
            Session["Consulta_Empleados"] = Dt_Empleados;
            Llena_Grid_Empleados();
            Mpe_Busqueda_Empleados.Hide();
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Empleados " + ex.Message.ToString(), ex);
        }
    }
    #endregion

    #region (Validaciones)
    /// *******************************************************************************************************
    /// Nombre: Validar_Formulario
    /// 
    /// Descripción: Válida que se hallan proporcionado los datos de forma correcta.
    /// 
    /// Parámetros:No Áplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 8/Agosto/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *******************************************************************************************************
    protected Boolean Validar_Formulario()
    {
        Boolean Estatus = true;
        Lbl_Mensaje_Error.Text = "";
        String Espacios = "&nbsp; &nbsp; &nbsp; &nbsp;&nbsp; + ";

        try
        {
            if (String.IsNullOrEmpty(Txt_Empleado.Text.Trim())) {
                Lbl_Mensaje_Error.Text += (Espacios + "Nombre del empleado es un dato requerido por el sistema. <br />");
                Estatus = false;
            }

            if (Cmb_Tipo_Desc_Orden_Judicial_Sueldo.SelectedIndex <= 0)
            {
                Lbl_Mensaje_Error.Text += (Espacios + "Seleccione un tipo de descuento [Sueldo] si es por: [% o cantidad]. <br />");
                Estatus = false;
            }
            if (String.IsNullOrEmpty(Txt_Cantidad_Porcentaje_Sueldo_OJ.Text.Trim()))
            {
                Lbl_Mensaje_Error.Text += (Espacios + "La cantidad o porcentaje [Sueldo] es un dato requerido por el sistema. <br />");
                Estatus = false;
            }
            if (Cmb_Neto_Bruto_Sueldo_OJ.SelectedIndex <= 0)
            {
                Lbl_Mensaje_Error.Text += (Espacios + "Seleccione sobre que se aplicara la retencion [Sueldo] si es al [BRUTO o NETO]. <br />");
                Estatus = false;
            }

            if (Cmb_Tipo_Desc_Orden_Judicial_Aguinaldo.SelectedIndex <= 0)
            {
                Lbl_Mensaje_Error.Text += (Espacios + "Seleccione un tipo de descuento [Aguinaldo] si es por: [% o cantidad]. <br />");
                Estatus = false;
            }
            if (String.IsNullOrEmpty(Txt_Cantidad_Porcentaje_Sueldo_OJ.Text.Trim()))
            {
                Lbl_Mensaje_Error.Text += (Espacios + "La cantidad o porcentaje [Aguinaldo] es un dato requerido por el sistema. <br />");
                Estatus = false;
            }
            if (Cmb_Neto_Bruto_Sueldo_OJ.SelectedIndex <= 0)
            {
                Lbl_Mensaje_Error.Text += (Espacios + "Seleccione sobre que se aplicara la retencion [Aguinaldo] si es al [BRUTO o NETO]. <br />");
                Estatus = false;
            }

            if (Cmb_Tipo_Desc_Orden_Judicial_PV.SelectedIndex <= 0)
            {
                Lbl_Mensaje_Error.Text += (Espacios + "Seleccione un tipo de descuento [Prima Vacacional] si es por: [% o cantidad]. <br />");
                Estatus = false;
            }
            if (String.IsNullOrEmpty(Txt_Cantidad_Porcentaje_PV_OJ.Text.Trim()))
            {
                Lbl_Mensaje_Error.Text += (Espacios + "La cantidad o porcentaje [Prima Vacacional] es un dato requerido por el sistema. <br />");
                Estatus = false;
            }
            if (Cmb_Neto_Bruto_PV_OJ.SelectedIndex <= 0)
            {
                Lbl_Mensaje_Error.Text += (Espacios + "Seleccione sobre que se aplicara la retencion [Prima Vacacional] si es al [BRUTO o NETO]. <br />");
                Estatus = false;
            }

            if (Cmb_Tipo_Desc_Orden_Judicial_Indemnizacion.SelectedIndex <= 0)
            {
                Lbl_Mensaje_Error.Text += (Espacios + "Seleccione un tipo de descuento [Indemnización] si es por: [% o cantidad]. <br />");
                Estatus = false;
            }
            if (String.IsNullOrEmpty(Txt_Cantidad_Porcentaje_Indemnizacion_OJ.Text.Trim()))
            {
                Lbl_Mensaje_Error.Text += (Espacios + "La cantidad o porcentaje [Indemnización] es un dato requerido por el sistema. <br />");
                Estatus = false;
            }
            if (Cmb_Neto_Bruto_Indemnizacion_OJ.SelectedIndex <= 0)
            {
                Lbl_Mensaje_Error.Text += (Espacios + "Seleccione sobre que se aplicara la retencion [Indemnización] si es al [BRUTO o NETO]. <br />");
                Estatus = false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al validar la información ingresada por el usuario. Error: [" + Ex.Message + "]");
        }
        return Estatus;
    }
    /// *******************************************************************************************************
    /// Nombre: Mostrar_Mensaje
    /// 
    /// Descripción: Muestra u oculta los mensajes mostrados en pantalla al usuario.
    /// 
    /// Parámetros:No Áplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 8/Agosto/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *******************************************************************************************************
    protected void Mostrar_Mensaje(String Mensaje, Boolean Estatus)
    {
        try
        {
            Lbl_Mensaje_Error.Text = Mensaje.Trim();
            Lbl_Mensaje_Error.Style.Add("display", ((Estatus) ? "block" : "none"));
            Img_Error.Style.Add("display", ((Estatus) ? "block" : "none"));
        }
        catch (Exception Ex)
        {
            throw new Exception("Error que se produce al mostrar los mensajes al usuario. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Combos Busqueda)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Dependencias_Busqueda
    /// 
    /// DESCRIPCION : Consulta las Dependencias uy Roles que estan dadas de alta en la DB
    /// 
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernández Negrete
    /// FECHA_CREO  : 08/Agosto/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Dependencias_Busqueda()
    {
        DataTable Dt_Dependencias;
        Cls_Cat_Dependencias_Negocio Rs_Consulta_Cat_Dependencias = new Cls_Cat_Dependencias_Negocio();

        try
        {
            Dt_Dependencias = Rs_Consulta_Cat_Dependencias.Consulta_Dependencias();
            Cmb_Busqueda_Dependencia.DataSource = Dt_Dependencias;
            Cmb_Busqueda_Dependencia.DataValueField = "Dependencia_ID";
            Cmb_Busqueda_Dependencia.DataTextField = "Nombre";
            Cmb_Busqueda_Dependencia.DataBind();
            Cmb_Busqueda_Dependencia.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Busqueda_Dependencia.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Dependencias " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Roles
    /// DESCRIPCION : Consulta los Roles que estan dadas de alta en la DB
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Roles_Busqueda()
    {
        DataTable Dt_Roles;
        Cls_Apl_Cat_Roles_Business Rs_Consulta_Apl_Cat_Roles = new Cls_Apl_Cat_Roles_Business();

        try
        {
            Dt_Roles = Rs_Consulta_Apl_Cat_Roles.Llenar_Tbl_Roles();
            Cmb_Busqueda_Rol.DataSource = Dt_Roles;
            Cmb_Busqueda_Rol.DataValueField = "Rol_ID";
            Cmb_Busqueda_Rol.DataTextField = "Nombre";
            Cmb_Busqueda_Rol.DataBind();
            Cmb_Busqueda_Rol.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Busqueda_Rol.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Roles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Areas_Dependencia
    /// DESCRIPCION : Consulta las áreas que tiene asignada la Dependencia
    /// PARAMETROS  : Dependencia_ID: Guarda el ID de la Dependencia a consultar sus áreas
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Areas_Dependencia_Busqueda(String Dependencia_ID)
    {
        DataTable Dt_Areas;
        Cls_Cat_Areas_Negocio Rs_Consulta_Cat_Areas = new Cls_Cat_Areas_Negocio();

        try
        {
            Rs_Consulta_Cat_Areas.P_Dependencia_ID = Dependencia_ID;
            Dt_Areas = Rs_Consulta_Cat_Areas.Consulta_Areas();
            Cmb_Busqueda_Areas.DataSource = Dt_Areas;
            Cmb_Busqueda_Areas.DataValueField = "Area_ID";
            Cmb_Busqueda_Areas.DataTextField = "Nombre";
            Cmb_Busqueda_Areas.DataBind();
            Cmb_Busqueda_Areas.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Busqueda_Areas.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Areas_Dependencia " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Tipos_Contratos
    /// DESCRIPCION : Consulta los tipos de trabajador que existen.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 3/Nov/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Tipos_Contratos_Busqueda()
    {
        DataTable Dt_Tipo_Contratos;
        Cls_Cat_Nom_Tipos_Contratos_Negocio Cat_Nom_Tipo_Contratos = new Cls_Cat_Nom_Tipos_Contratos_Negocio();

        try
        {
            Dt_Tipo_Contratos = Cat_Nom_Tipo_Contratos.Consulta_Tipos_Contratos();
            Cmb_Busqueda_Tipo_Contrato.DataSource = Dt_Tipo_Contratos;
            Cmb_Busqueda_Tipo_Contrato.DataValueField = Presidencia.Constantes.Cat_Nom_Tipos_Contratos.Campo_Tipo_Contrato_ID;
            Cmb_Busqueda_Tipo_Contrato.DataTextField = Presidencia.Constantes.Cat_Nom_Tipos_Contratos.Campo_Descripcion;
            Cmb_Busqueda_Tipo_Contrato.DataBind();
            Cmb_Busqueda_Tipo_Contrato.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Busqueda_Tipo_Contrato.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Tipos_Contratos " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Puestos
    /// DESCRIPCION : Consulta los puestos
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 3/Nov/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Puestos_Busqueda()
    {
        DataTable Dt_Puestos;
        Cls_Cat_Puestos_Negocio Cat_Nom_Puestos = new Cls_Cat_Puestos_Negocio();

        try
        {
            Cat_Nom_Puestos.P_Tipo_DataTable = "PUESTOS";
            Dt_Puestos = Cat_Nom_Puestos.Consulta_DataTable();
            Cmb_Busqueda_Puesto.DataSource = Dt_Puestos;
            Cmb_Busqueda_Puesto.DataValueField = Presidencia.Constantes.Cat_Puestos.Campo_Puesto_ID;
            Cmb_Busqueda_Puesto.DataTextField = Presidencia.Constantes.Cat_Puestos.Campo_Nombre;
            Cmb_Busqueda_Puesto.DataBind();
            Cmb_Busqueda_Puesto.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Busqueda_Puesto.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Roles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Escolaridad
    /// DESCRIPCION : Consulta Escolaridad
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 3/Nov/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Escolaridad_Busqueda()
    {
        DataTable Dt_Escolaridad;
        Cls_Cat_Nom_Escolaridad_Negocio Cat_Nom_Escolaridad = new Cls_Cat_Nom_Escolaridad_Negocio();

        try
        {
            Dt_Escolaridad = Cat_Nom_Escolaridad.Consulta_Escolaridad();
            Cmb_Busqueda_Escolaridad.DataSource = Dt_Escolaridad;
            Cmb_Busqueda_Escolaridad.DataValueField = Presidencia.Constantes.Cat_Nom_Escolaridad.Campo_Escolaridad_ID;
            Cmb_Busqueda_Escolaridad.DataTextField = Presidencia.Constantes.Cat_Nom_Escolaridad.Campo_Escolaridad;
            Cmb_Busqueda_Escolaridad.DataBind();
            Cmb_Busqueda_Escolaridad.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Busqueda_Escolaridad.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Escolaridad " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION:Consultar_Sindicatos
    /// DESCRIPCION : Consulta Programas
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 3/Nov/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Sindicatos_Busqueda()
    {
        DataTable Dt_Sindicatos;
        Cls_Cat_Nom_Sindicatos_Negocio Cat_Nom_Sindicatos = new Cls_Cat_Nom_Sindicatos_Negocio();

        try
        {
            Dt_Sindicatos = Cat_Nom_Sindicatos.Consulta_Sindicato();
            Cmb_Busqueda_Sindicato.DataSource = Dt_Sindicatos;
            Cmb_Busqueda_Sindicato.DataValueField = Presidencia.Constantes.Cat_Nom_Sindicatos.Campo_Sindicato_ID;
            Cmb_Busqueda_Sindicato.DataTextField = Presidencia.Constantes.Cat_Nom_Sindicatos.Campo_Nombre;
            Cmb_Busqueda_Sindicato.DataBind();
            Cmb_Busqueda_Sindicato.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Busqueda_Sindicato.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Sindicatos " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION:Consultar_Turnos
    /// DESCRIPCION : Consulta los turnos
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 3/Nov/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Turnos_Busqueda()
    {
        DataTable Dt_Turnos;
        Cls_Cat_Turnos_Negocio Cat_Turnos = new Cls_Cat_Turnos_Negocio();

        try
        {
            Dt_Turnos = Cat_Turnos.Consulta_Turnos();
            Cmb_Busqueda_Turno.DataSource = Dt_Turnos;
            Cmb_Busqueda_Turno.DataValueField = Presidencia.Constantes.Cat_Turnos.Campo_Turno_ID;
            Cmb_Busqueda_Turno.DataTextField = Presidencia.Constantes.Cat_Turnos.Campo_Descripcion;
            Cmb_Busqueda_Turno.DataBind();
            Cmb_Busqueda_Turno.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Busqueda_Turno.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Turnos " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION:Consultar_Zonas
    /// DESCRIPCION : Consulta las zonas de salario
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 3/Nov/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Zonas_Busqueda()
    {
        DataTable Dt_Zonas;
        Cls_Cat_Nom_Zona_Economica_Negocio Cat_Nom_Zonas = new Cls_Cat_Nom_Zona_Economica_Negocio();

        try
        {
            Dt_Zonas = Cat_Nom_Zonas.Consulta_Zona_Economica();
            Cmb_Busqueda_Zona.DataSource = Dt_Zonas;
            Cmb_Busqueda_Zona.DataValueField = Presidencia.Constantes.Cat_Nom_Zona_Economica.Campo_Zona_ID;
            Cmb_Busqueda_Zona.DataTextField = Presidencia.Constantes.Cat_Nom_Zona_Economica.Campo_Zona_Economica;
            Cmb_Busqueda_Zona.DataBind();
            Cmb_Busqueda_Zona.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Busqueda_Zona.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Zonas " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION:Consultar_Tipo_Trabajador
    /// DESCRIPCION : Consulta los tipos de trabajador
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 3/Nov/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Tipo_Trabajador_Busqueda()
    {
        DataTable Dt_Tipos_Trabajador;
        Cls_Cat_Nom_Tipo_Trabajador_Negocio Cat_Nom_Tipos_Trabajador = new Cls_Cat_Nom_Tipo_Trabajador_Negocio();

        try
        {
            Dt_Tipos_Trabajador = Cat_Nom_Tipos_Trabajador.Consulta_Tipo_Trabajador();
            Cmb_Busqueda_Tipo_Trabajador.DataSource = Dt_Tipos_Trabajador;
            Cmb_Busqueda_Tipo_Trabajador.DataValueField = Presidencia.Constantes.Cat_Nom_Tipo_Trabajador.Campo_Tipo_Trabajador_ID;
            Cmb_Busqueda_Tipo_Trabajador.DataTextField = Presidencia.Constantes.Cat_Nom_Tipo_Trabajador.Campo_Descripcion;
            Cmb_Busqueda_Tipo_Trabajador.DataBind();
            Cmb_Busqueda_Tipo_Trabajador.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Busqueda_Tipo_Trabajador.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Roles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Tipos_Nomina
    /// DESCRIPCION : Consulta los tipos de Nomina que existen
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 5/Nov/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Tipos_Nomina_Busqueda()
    {
        DataTable Dt_Tipos_Nomina;
        Cls_Cat_Empleados_Negocios Cat_Empleados = new Cls_Cat_Empleados_Negocios();

        try
        {
            Dt_Tipos_Nomina = Cat_Empleados.Consultar_Tipos_Nomina();
            Cmb_Busqueda_Tipo_Nomina.DataSource = Dt_Tipos_Nomina;
            Cmb_Busqueda_Tipo_Nomina.DataValueField = Presidencia.Constantes.Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID;
            Cmb_Busqueda_Tipo_Nomina.DataTextField = Presidencia.Constantes.Cat_Nom_Tipos_Nominas.Campo_Nomina;
            Cmb_Busqueda_Tipo_Nomina.DataBind();
            Cmb_Busqueda_Tipo_Nomina.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Busqueda_Tipo_Nomina.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Roles " + ex.Message.ToString(), ex);
        }
    }
    #endregion

    #endregion

    #region (Grids)

    #region (Tabulador Ordenes Judiciales)
    ///*******************************************************************************************************
    /// NOMBRE DE LA FUNCION: Llena_Grid_Tabulador_Orden_Judicial
    /// DESCRIPCION : Llena el grid con los tabulador de orden judicial que fueron obtenidos de la consulta
    ///               Consulta_Empleados
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///********************************************************************************************************
    private void Llena_Grid_Tabulador_Orden_Judicial(DataTable Dt_Orden_Judiciales)
    {
        try
        {
            Grid_Tabulador_Orden_Judicial.Columns[1].Visible = true;
            Grid_Tabulador_Orden_Judicial.DataSource = Dt_Orden_Judiciales;
            Grid_Tabulador_Orden_Judicial.DataBind();
            Grid_Tabulador_Orden_Judicial.Columns[1].Visible = false;
            Grid_Tabulador_Orden_Judicial.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_Empleados " + ex.Message.ToString(), ex);
        }
    }
    /// *******************************************************************************************************
    /// Nombre: Grid_Tabulador_Orden_Judicial_SelectedIndexChanged
    /// 
    /// Descripción: Evento que se ejecuta al seleccionar un elemento de la tabla de tabulador de ordenes 
    ///              judiciales.
    /// 
    /// Parámetros:No Áplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 8/Agosto/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *******************************************************************************************************
    protected void Grid_Tabulador_Orden_Judicial_SelectedIndexChanged(Object sender, EventArgs e)
    {
        Cls_Cat_Nom_Tab_Orden_Judicial_Negocio Obj_Tabulador_Orden_Judicial = new Cls_Cat_Nom_Tab_Orden_Judicial_Negocio();
        DataTable Dt_Tabulador_Orden_Jucicial = null;

        try
        {
            Txt_Orden_Judicial_ID.Text = Grid_Tabulador_Orden_Judicial.SelectedRow.Cells[1].Text.Trim();
            Obj_Tabulador_Orden_Judicial.P_Orden_Judicial_ID = Txt_Orden_Judicial_ID.Text.Trim();
            Dt_Tabulador_Orden_Jucicial = Obj_Tabulador_Orden_Judicial.Consultar_Parametros_Orden_Judicial_Empleado();

            if (Dt_Tabulador_Orden_Jucicial is DataTable) {
                if (Dt_Tabulador_Orden_Jucicial.Rows.Count > 0) {
                    foreach (DataRow PARAMETRO_ORDEN_JUDICIAL in Dt_Tabulador_Orden_Jucicial.Rows) {
                        if (PARAMETRO_ORDEN_JUDICIAL is DataRow) {

                            if (!String.IsNullOrEmpty(PARAMETRO_ORDEN_JUDICIAL[Cat_Nom_Tab_Orden_Judicial.Campo_Orden_Judicial_ID].ToString().Trim()))
                                Txt_Orden_Judicial_ID.Text = PARAMETRO_ORDEN_JUDICIAL[Cat_Nom_Tab_Orden_Judicial.Campo_Orden_Judicial_ID].ToString().Trim();

                            if (!String.IsNullOrEmpty(PARAMETRO_ORDEN_JUDICIAL[Cat_Nom_Tab_Orden_Judicial.Campo_Empleado_ID].ToString().Trim()))
                                HTxt_Empleado.Value = PARAMETRO_ORDEN_JUDICIAL[Cat_Nom_Tab_Orden_Judicial.Campo_Empleado_ID].ToString().Trim();

                            if (!String.IsNullOrEmpty(PARAMETRO_ORDEN_JUDICIAL["EMPLEADO"].ToString().Trim()))
                                Txt_Empleado.Text = PARAMETRO_ORDEN_JUDICIAL["EMPLEADO"].ToString().Trim();

                            if (!String.IsNullOrEmpty(PARAMETRO_ORDEN_JUDICIAL[Cat_Nom_Tab_Orden_Judicial.Campo_Beneficiario].ToString().Trim()))
                                Txt_Nombre_Beneficiario.Text = PARAMETRO_ORDEN_JUDICIAL[Cat_Nom_Tab_Orden_Judicial.Campo_Beneficiario].ToString().Trim();

                            if (!String.IsNullOrEmpty(PARAMETRO_ORDEN_JUDICIAL[Cat_Nom_Tab_Orden_Judicial.Campo_Tipo_Descuento_Orden_Judicial_Sueldo].ToString().Trim()))
                                Cmb_Tipo_Desc_Orden_Judicial_Sueldo.SelectedIndex =
                                    Cmb_Tipo_Desc_Orden_Judicial_Sueldo.Items.IndexOf(Cmb_Tipo_Desc_Orden_Judicial_Sueldo.Items.FindByText(
                                    PARAMETRO_ORDEN_JUDICIAL[Cat_Nom_Tab_Orden_Judicial.Campo_Tipo_Descuento_Orden_Judicial_Sueldo].ToString().Trim()));

                            if (!String.IsNullOrEmpty(PARAMETRO_ORDEN_JUDICIAL[Cat_Nom_Tab_Orden_Judicial.Campo_Cantidad_Porcentaje_Orden_Judicial_Sueldo].ToString().Trim()))
                                Txt_Cantidad_Porcentaje_Sueldo_OJ.Text = String.Format("{0:0.00}", Convert.ToDouble(PARAMETRO_ORDEN_JUDICIAL[Cat_Nom_Tab_Orden_Judicial.Campo_Cantidad_Porcentaje_Orden_Judicial_Sueldo].ToString().Trim()));

                            if (!String.IsNullOrEmpty(PARAMETRO_ORDEN_JUDICIAL[Cat_Nom_Tab_Orden_Judicial.Campo_Bruto_Neto_Orden_Judicial_Sueldo].ToString().Trim()))
                                Cmb_Neto_Bruto_Sueldo_OJ.SelectedIndex =
                                    Cmb_Neto_Bruto_Sueldo_OJ.Items.IndexOf(Cmb_Neto_Bruto_Sueldo_OJ.Items.FindByText(
                                    PARAMETRO_ORDEN_JUDICIAL[Cat_Nom_Tab_Orden_Judicial.Campo_Bruto_Neto_Orden_Judicial_Sueldo].ToString().Trim()));


                            if (!String.IsNullOrEmpty(PARAMETRO_ORDEN_JUDICIAL[Cat_Nom_Tab_Orden_Judicial.Campo_Tipo_Descuento_Orden_Judicial_Aguinaldo].ToString().Trim()))
                                Cmb_Tipo_Desc_Orden_Judicial_Aguinaldo.SelectedIndex =
                                    Cmb_Tipo_Desc_Orden_Judicial_Aguinaldo.Items.IndexOf(Cmb_Tipo_Desc_Orden_Judicial_Aguinaldo.Items.FindByText(
                                    PARAMETRO_ORDEN_JUDICIAL[Cat_Nom_Tab_Orden_Judicial.Campo_Tipo_Descuento_Orden_Judicial_Aguinaldo].ToString().Trim()));

                            if (!String.IsNullOrEmpty(PARAMETRO_ORDEN_JUDICIAL[Cat_Nom_Tab_Orden_Judicial.Campo_Cantidad_Porcentaje_Orden_Judicial_Aguinaldo].ToString().Trim()))
                                Txt_Cantidad_Porcentaje_Aguinaldo_OJ.Text = String.Format("{0:0.00}", Convert.ToDouble(PARAMETRO_ORDEN_JUDICIAL[Cat_Nom_Tab_Orden_Judicial.Campo_Cantidad_Porcentaje_Orden_Judicial_Aguinaldo].ToString().Trim()));

                            if (!String.IsNullOrEmpty(PARAMETRO_ORDEN_JUDICIAL[Cat_Nom_Tab_Orden_Judicial.Campo_Bruto_Neto_Orden_Judicial_Aguinaldo].ToString().Trim()))
                                Cmb_Neto_Bruto_Aguinaldo_OJ.SelectedIndex =
                                    Cmb_Neto_Bruto_Aguinaldo_OJ.Items.IndexOf(Cmb_Neto_Bruto_Aguinaldo_OJ.Items.FindByText(
                                    PARAMETRO_ORDEN_JUDICIAL[Cat_Nom_Tab_Orden_Judicial.Campo_Bruto_Neto_Orden_Judicial_Aguinaldo].ToString().Trim()));


                            if (!String.IsNullOrEmpty(PARAMETRO_ORDEN_JUDICIAL[Cat_Nom_Tab_Orden_Judicial.Campo_Tipo_Descuento_Orden_Judicial_PV].ToString().Trim()))
                                Cmb_Tipo_Desc_Orden_Judicial_PV.SelectedIndex =
                                    Cmb_Tipo_Desc_Orden_Judicial_PV.Items.IndexOf(Cmb_Tipo_Desc_Orden_Judicial_PV.Items.FindByText(
                                    PARAMETRO_ORDEN_JUDICIAL[Cat_Nom_Tab_Orden_Judicial.Campo_Tipo_Descuento_Orden_Judicial_PV].ToString().Trim()));

                            if (!String.IsNullOrEmpty(PARAMETRO_ORDEN_JUDICIAL[Cat_Nom_Tab_Orden_Judicial.Campo_Cantidad_Porcentaje_Orden_Judicial_PV].ToString().Trim()))
                                Txt_Cantidad_Porcentaje_PV_OJ.Text = String.Format("{0:0.00}", Convert.ToDouble(PARAMETRO_ORDEN_JUDICIAL[Cat_Nom_Tab_Orden_Judicial.Campo_Cantidad_Porcentaje_Orden_Judicial_PV].ToString().Trim()));

                            if (!String.IsNullOrEmpty(PARAMETRO_ORDEN_JUDICIAL[Cat_Nom_Tab_Orden_Judicial.Campo_Bruto_Neto_Orden_Judicial_PV].ToString().Trim()))
                                Cmb_Neto_Bruto_PV_OJ.SelectedIndex =
                                    Cmb_Neto_Bruto_PV_OJ.Items.IndexOf(Cmb_Neto_Bruto_PV_OJ.Items.FindByText(
                                    PARAMETRO_ORDEN_JUDICIAL[Cat_Nom_Tab_Orden_Judicial.Campo_Bruto_Neto_Orden_Judicial_PV].ToString().Trim()));


                            if (!String.IsNullOrEmpty(PARAMETRO_ORDEN_JUDICIAL[Cat_Nom_Tab_Orden_Judicial.Campo_Tipo_Descuento_Orden_Judicial_Indemnizacion].ToString().Trim()))
                                Cmb_Tipo_Desc_Orden_Judicial_Indemnizacion.SelectedIndex =
                                    Cmb_Tipo_Desc_Orden_Judicial_Indemnizacion.Items.IndexOf(Cmb_Tipo_Desc_Orden_Judicial_Indemnizacion.Items.FindByText(
                                    PARAMETRO_ORDEN_JUDICIAL[Cat_Nom_Tab_Orden_Judicial.Campo_Tipo_Descuento_Orden_Judicial_Indemnizacion].ToString().Trim()));

                            if (!String.IsNullOrEmpty(PARAMETRO_ORDEN_JUDICIAL[Cat_Nom_Tab_Orden_Judicial.Campo_Cantidad_Porcentaje_Orden_Judicial_Indemnizacion].ToString().Trim()))
                                Txt_Cantidad_Porcentaje_Indemnizacion_OJ.Text = String.Format("{0:0.00}", Convert.ToDouble(PARAMETRO_ORDEN_JUDICIAL[Cat_Nom_Tab_Orden_Judicial.Campo_Cantidad_Porcentaje_Orden_Judicial_Indemnizacion].ToString().Trim()));

                            if (!String.IsNullOrEmpty(PARAMETRO_ORDEN_JUDICIAL[Cat_Nom_Tab_Orden_Judicial.Campo_Bruto_Neto_Orden_Judicial_Indemnizacion].ToString().Trim()))
                                Cmb_Neto_Bruto_Indemnizacion_OJ.SelectedIndex =
                                    Cmb_Neto_Bruto_Indemnizacion_OJ.Items.IndexOf(Cmb_Neto_Bruto_Indemnizacion_OJ.Items.FindByText(
                                    PARAMETRO_ORDEN_JUDICIAL[Cat_Nom_Tab_Orden_Judicial.Campo_Bruto_Neto_Orden_Judicial_Indemnizacion].ToString().Trim()));
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al seleccionar un parámetro de los tabuladores de orden judicial que le aplica al empleado. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Grid Empleados)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Empleados_SelectedIndexChanged
    /// DESCRIPCION : Consulta los datos del Empleado que selecciono el usuario
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 12-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Empleados_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Nom_Tab_Orden_Judicial_Negocio Obj_Orden_Judicial = new Cls_Cat_Nom_Tab_Orden_Judicial_Negocio();
        DataTable Dt_Orden_Judicial = null;

        try
        {
            Txt_Empleado.Text = Grid_Empleados.SelectedRow.Cells[4].Text.Trim();
            HTxt_Empleado.Value = Grid_Empleados.SelectedRow.Cells[1].Text.Trim();
            Mpe_Busqueda_Empleados.Hide();

            if (!String.IsNullOrEmpty(HTxt_Empleado.Value.Trim()))
            {
                Obj_Orden_Judicial.P_Empleado_ID = HTxt_Empleado.Value.Trim();
                Dt_Orden_Judicial = Obj_Orden_Judicial.Consultar_Parametros_Orden_Judicial_Empleado();
                Llena_Grid_Tabulador_Orden_Judicial(Dt_Orden_Judicial);
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje(Ex.Message, true);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Empleados_PageIndexChanging
    /// DESCRIPCION : Cambia la pagina de la tabla de empleados
    ///               
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Empleados_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            //Limpia_Controles();                        //Limpia todos los controles de la forma
            Grid_Empleados.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
            Llena_Grid_Empleados();                    //Carga los Empleados que estan asignados a la página seleccionada

            Grid_Empleados.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje(Ex.Message, true);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llena_Grid_Empleados
    /// DESCRIPCION : Llena el grid con los Empleados que fueron obtenidos de la consulta
    ///               Consulta_Empleados
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llena_Grid_Empleados()
    {
        DataTable Dt_Empleados; //Variable que obtendra los datos de la consulta 
        try
        {
            Grid_Empleados.Columns[1].Visible = true;
            Grid_Empleados.DataBind();
            Dt_Empleados = (DataTable)Session["Consulta_Empleados"];
            Grid_Empleados.DataSource = Dt_Empleados;
            Grid_Empleados.DataBind();
            Grid_Empleados.Columns[1].Visible = false;
            Grid_Empleados.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_Empleados " + ex.Message.ToString(), ex);
        }
    }
    /// **************************************************************************************************************************************
    /// NOMBRE: Grid_Empleados_Sorting
    /// 
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// 
    /// CREÓ:   Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 18/Febrero/2011 19:04 pm.
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// **************************************************************************************************************************************
    protected void Grid_Empleados_Sorting(object sender, GridViewSortEventArgs e)
    {
        Consulta_Empleados_Avanzada();
        DataTable Dt_Calendario_Nominas = (Grid_Empleados.DataSource as DataTable);

        if (Dt_Calendario_Nominas != null)
        {
            DataView Dv_Calendario_Nominas = new DataView(Dt_Calendario_Nominas);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Calendario_Nominas.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Calendario_Nominas.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Empleados.DataSource = Dv_Calendario_Nominas;
            Grid_Empleados.DataBind();
        }
    }
    #endregion

    #endregion

    #region (Eventos)

    #region (Botones)
    /// *******************************************************************************************************
    /// Nombre: Btn_Nuevo_Click
    /// 
    /// Descripción: Da de alta un registro del tabulador de orden judicial.
    /// 
    /// Parámetros:No Áplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 8/Agosto/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *******************************************************************************************************
    protected void Btn_Nuevo_Click(Object sender, EventArgs e) {
        Cls_Cat_Nom_Tab_Orden_Judicial_Negocio Obj_Orden_Judicial = new Cls_Cat_Nom_Tab_Orden_Judicial_Negocio();
        DataTable Dt_Orden_Judicial = null;

        try
        {
            if (Btn_Nuevo.ToolTip.Equals("Nuevo"))
            {
                Habilitar_Controles("Nuevo");
                Limpiar_Controles();
            }
            else
            {
                if (Validar_Formulario())
                {
                    Alta();

                    if (!String.IsNullOrEmpty(HTxt_Empleado.Value.Trim()))
                    {
                        Obj_Orden_Judicial.P_Empleado_ID = HTxt_Empleado.Value.Trim();
                        Dt_Orden_Judicial = Obj_Orden_Judicial.Consultar_Parametros_Orden_Judicial_Empleado();
                        Llena_Grid_Tabulador_Orden_Judicial(Dt_Orden_Judicial);
                    }

                    Configuracion_Inicial();
                }
                else
                {
                    Mostrar_Mensaje(Lbl_Mensaje_Error.Text.Trim(), true);
                }
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje(Ex.Message, true);
        }
    }
    /// *******************************************************************************************************
    /// Nombre: Btn_Modificar_Click
    /// 
    /// Descripción: Actualiza un registro del tabulador de orden judicial de los empleados.
    /// 
    /// Parámetros:No Áplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 8/Agosto/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *******************************************************************************************************
    protected void Btn_Modificar_Click(Object sender, EventArgs e)
    {
        Cls_Cat_Nom_Tab_Orden_Judicial_Negocio Obj_Orden_Judicial = new Cls_Cat_Nom_Tab_Orden_Judicial_Negocio();
        DataTable Dt_Orden_Judicial = null;

        try
        {
            Mostrar_Mensaje(String.Empty, false);//Oculta los mensajes de la pantalla.

            if (Btn_Modificar.ToolTip.Equals("Modificar"))
            {
                if ((Grid_Tabulador_Orden_Judicial.SelectedIndex != -1) && 
                    !(String.IsNullOrEmpty(Txt_Orden_Judicial_ID.Text.Trim())))
                {
                    Habilitar_Controles("Modificar");
                }
                else
                {
                    Mostrar_Mensaje("Seleccione el registro que desea modificar sus datos <br>", true);
                }
            }
            else
            {
                if (Validar_Formulario())
                {
                    Actualizar();

                    if (!String.IsNullOrEmpty(HTxt_Empleado.Value.Trim()))
                    {
                        Obj_Orden_Judicial.P_Empleado_ID = HTxt_Empleado.Value.Trim();
                        Dt_Orden_Judicial = Obj_Orden_Judicial.Consultar_Parametros_Orden_Judicial_Empleado();
                        Llena_Grid_Tabulador_Orden_Judicial(Dt_Orden_Judicial);
                    }

                    Configuracion_Inicial();
                }
                else
                {
                    Mostrar_Mensaje(Lbl_Mensaje_Error.Text.Trim(), true);
                }
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje(Ex.Message, true);
        }
    }
    /// *******************************************************************************************************
    /// Nombre: Btn_Eliminar_Click
    /// 
    /// Descripción: Elimina un registro de tabular de orden judicial de los empleados.
    /// 
    /// Parámetros:No Áplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 8/Agosto/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *******************************************************************************************************
    protected void Btn_Eliminar_Click(Object sender, EventArgs e)
    {
        Cls_Cat_Nom_Tab_Orden_Judicial_Negocio Obj_Orden_Judicial = new Cls_Cat_Nom_Tab_Orden_Judicial_Negocio();
        DataTable Dt_Orden_Judicial = null;

        try
        {
            Mostrar_Mensaje(String.Empty, false);//Ocultamos los mensajes mostrados en pantalla.

            if (Btn_Eliminar.ToolTip.Equals("Eliminar"))
            {
                if ((Grid_Tabulador_Orden_Judicial.SelectedIndex != (-1)) &&
                    !(String.IsNullOrEmpty(Txt_Orden_Judicial_ID.Text.Trim())))
                {
                    Eliminar();

                    if (!String.IsNullOrEmpty(HTxt_Empleado.Value.Trim()))
                    {
                        Obj_Orden_Judicial.P_Empleado_ID = HTxt_Empleado.Value.Trim();
                        Dt_Orden_Judicial = Obj_Orden_Judicial.Consultar_Parametros_Orden_Judicial_Empleado();
                        Llena_Grid_Tabulador_Orden_Judicial(Dt_Orden_Judicial);
                    }

                    Configuracion_Inicial();
                }
                else
                {
                    Mostrar_Mensaje("Seleccione el registro que desea eliminar <br>", true);
                }
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje(Ex.Message, true);
        }
    }
    /// *******************************************************************************************************
    /// Nombre: Btn_Salir_Click
    /// 
    /// Descripción: Cancela la operación o nos da una salida del formulario actual a lapantalla principal.
    /// 
    /// Parámetros:No Áplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 8/Agosto/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *******************************************************************************************************
    protected void Btn_Salir_Click(Object sender, EventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip.Trim().Equals("Inicio"))
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Configuracion_Inicial();//Habilita los controles para la siguiente operación del usuario en el catálogo
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje(Ex.Message, true);
        }
    }
    /// *******************************************************************************************************
    /// Nombre: Btn_Busqueda_Empleados_Click
    /// 
    /// Descripción: Hace una busqueda de los empleados para cargar su información.
    /// 
    /// Parámetros:No Áplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 8/Agosto/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *******************************************************************************************************
    protected void Btn_Busqueda_Empleados_Click(object sender, EventArgs e)
    {
        try
        {
            Consulta_Empleados_Avanzada();
            Mpe_Busqueda_Empleados.Show();
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje(Ex.Message, true);
        }
    }
    #endregion

    #region (Combos)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cmb_Busqueda_Dependencia_SelectedIndexChanged
    /// DESCRIPCION : Cargar las area correspodientes a la dependencia seleccionada.
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Cmb_Busqueda_Dependencia_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Consulta_Areas_Dependencia_Busqueda(Cmb_Busqueda_Dependencia.SelectedValue);
            Cmb_Busqueda_Areas.Enabled = true;
            Mpe_Busqueda_Empleados.Show();
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje(Ex.Message, true);
        }
    }
    #endregion

    #endregion
}
