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
using System.Globalization;
using System.Text.RegularExpressions;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Empleados.Negocios;
using Presidencia.Dependencias.Negocios;
using Presidencia.Prestamos.Negocio;
using System.IO;
using Presidencia.Incapacidades.Negocio;
using Presidencia.Dias_Festivos.Negocios;
using System.Collections.Generic;
using System.Text;

public partial class paginas_Nomina_Frm_Ope_Nom_Incapacidades : System.Web.UI.Page
{

    #region (Load/Init)
    ///******************************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Carga inicial de la página
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011 11:22 am 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Configuracion_Inicial();
                ViewState["SortDirection"] = "ASC";
            }

            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }
    #endregion

    #region (Métodos)

    #region (Metodos Generales)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Inicial
    ///DESCRIPCIÓN: Configuracion Inicial del Catalogo de Tiempo de Horas Extra
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011 11:51 am
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Inicial()
    {
        Limpiar_Controles();
        Habilitar_Controles("Inicial");
        Consultar_Dependencias();      
        Consultar_Calendarios_Nomina();
        Cmb_Dependencia.Focus();
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION : Limpia los Controles de la pagina.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            Txt_No_Empleado.Text = String.Empty;
            //Cmb_Calendario_Nomina.SelectedIndex = -1;
            //Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = -1;
            Txt_No_Incapacidad.Text = "";
            Cmb_Estatis.SelectedIndex = -1;
            Cmb_Dependencia.SelectedIndex = -1;
            Cmb_Empleados.SelectedIndex = -1;
            Cmb_Tipo_Incapacidad.SelectedIndex = -1;
            Txt_Porcentaje_Incapacidad.Text = "";
            Txt_Fecha_Inicio.Text = "";
            Txt_Fecha_Fin.Text = "";
            Chk_Aplica_Pago_Cuarto_Dia.Checked = false;
            Chk_Aplica_Extencion_Incapacidad.Checked = false;
            Txt_Comentarios.Text = "";
            Txt_Dias_Incapacidad.Text = "";
            Grid_Incapacidades.DataBind();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al limpiar los controles del formulario. Error: [" + Ex.Message.ToString() + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///               para a siguiente operación
    /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
    ///                          si es una alta, modificacion
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Abril/2011
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

                    Btn_Busqueda_Incapacidad.Enabled = true;

                    Lbl_Mensaje_Error.Text = "";
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    //Cmb_Dependencia.Enabled = false;
                    
                    Configuracion_Acceso("Frm_Ope_Nom_Incapacidades.aspx");
                    Cmb_Dependencia.Focus();
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
                    Btn_Busqueda_Incapacidad.Enabled = false;
                    Cmb_Dependencia.Enabled = false;//Editado
                    Cmb_Estatis.SelectedIndex = 1;
                    Cmb_Calendario_Nomina.Focus();

                    Txt_Porcentaje_Incapacidad.Text = "100";
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

                    Btn_Busqueda_Incapacidad.Enabled = false;
                    Cmb_Calendario_Nomina.Focus();
                    break;
            }

            Txt_No_Empleado.Enabled = true;
            Btn_Consultar.Enabled = true;

            Cmb_Calendario_Nomina.Enabled = Habilitado;
            Cmb_Periodos_Catorcenales_Nomina.Enabled = Habilitado;
            Txt_No_Incapacidad.Enabled = false;
            Cmb_Estatis.Enabled = false;
            Cmb_Estatis.SelectedIndex = 3;
            Cmb_Dependencia.Enabled = true;
            Cmb_Empleados.Enabled = true;
            Cmb_Tipo_Incapacidad.Enabled = Habilitado;
            Txt_Porcentaje_Incapacidad.Enabled = Habilitado;
            Txt_Fecha_Inicio.Enabled = Habilitado;
            Txt_Fecha_Fin.Enabled = false;
            Chk_Aplica_Pago_Cuarto_Dia.Enabled = Habilitado;
            Chk_Aplica_Extencion_Incapacidad.Enabled = Habilitado;
            Txt_Comentarios.Enabled = Habilitado;
            Btn_Fecha_Inicio.Enabled = Habilitado;
            Btn_Fecha_Fin.Enabled = false;
            Txt_Dias_Incapacidad.Enabled = Habilitado;

           // Cmb_Dependencia.Enabled = (Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Datos_Empleado.Rows[0][Cat_Empleados.Campo_Rol_ID].ToString()).Rows[0][Apl_Grupos_Roles.Campo_Grupo_Roles_ID].ToString().Equals("00006")) ? Habilitado : false;
            //Cmb_Estatis.Enabled = (Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Datos_Empleado.Rows[0][Cat_Empleados.Campo_Rol_ID].ToString()).Rows[0][Apl_Grupos_Roles.Campo_Grupo_Roles_ID].ToString().Equals("00006")) ? Habilitado : false;
            Tr_Periodos_Fiscales.Visible = (Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Datos_Empleado.Rows[0][Cat_Empleados.Campo_Rol_ID].ToString()).Rows[0][Apl_Grupos_Roles.Campo_Grupo_Roles_ID].ToString().Equals("00006")) ? true : false;

            Cmb_Calendario_Nomina.Enabled = Habilitado;
            Cmb_Periodos_Catorcenales_Nomina.Enabled = Habilitado;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al Habilitar los Controles del formulario. Error:[" + ex.Message.ToString() + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Agregar_Tooltip_Combos
    /// 
    /// DESCRIPCION : Agrega un Tooltip a los elementos del combo.
    /// 
    /// PARAMETROS  : Cmb_Combo: Combo a agregar tooltip.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Agregar_Tooltip_Combos(DropDownList Cmb_Combo)
    {
        for (int i = 0; i <= Cmb_Combo.Items.Count - 1; i++)
        {
            Cmb_Combo.Items[i].Attributes.Add("Title", Cmb_Combo.Items[i].Text);
        }
        Cmb_Combo.SelectedIndex = -1;
    }
    /// *****************************************************************************************
    /// Nombre: Obtener_Id_Empleado
    /// 
    /// Descripción: Se consulta el identificador del empleado a partir del número de control 
    ///              del mismo.
    /// 
    /// Parámetros: No_Empleado.- Numero de identificación del empleado por recursos humanos.
    /// 
    /// Usuario Creó: Juan Alberto Hernandez Negrete.
    /// Fecha Creó: 07/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *****************************************************************************************
    protected String Obtener_Id_Empleado(String No_Empleado)
    {
        String Empleado_ID = String.Empty;
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexión con la capa de negocios.
        DataTable Dt_Empleado = null;

        try
        {
            Obj_Empleados.P_No_Empleado = No_Empleado;
            Dt_Empleado = Obj_Empleados.Consulta_Empleados_General();

            if (Dt_Empleado is DataTable)
            {
                if (Dt_Empleado.Rows.Count > 0)
                {
                    foreach (DataRow EMPLEADO in Dt_Empleado.Rows)
                    {
                        if (EMPLEADO is DataRow)
                        {
                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Empleado_ID].ToString().Trim()))
                            {
                                Empleado_ID = EMPLEADO[Cat_Empleados.Campo_Empleado_ID].ToString().Trim();
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar el identificador interno del empleado. Error: [" + Ex.Message + "]");
        }
        return Empleado_ID;
    }
    ///**********************************************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Crear_Registros_Incapacidad
    ///
    ///DESCRIPCIÓN: Crear registros de incapacidad.
    ///
    /// PARÁMETROS: NO APLICA.
    /// 
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 14/Diciembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///***********************************************************************************************************************************************
    private DataTable Crear_Registros_Incapacidad(ref StringBuilder Reporte)
    {
        Cls_Ope_Nom_Incapacidades_Negocio Obj_Incapacidades = new Cls_Ope_Nom_Incapacidades_Negocio();//Variable de conexion hacia la capa de negocios.
        DataTable Dt_Incapacidades = null;//Variable que guarda las incapacidades.
        DateTime Fecha_Inicio_Incapacidad = new DateTime();//Variable que almacena la fecha de inicio.
        String No_Nomina = String.Empty;//Variable que almacena el periodo a aplicar las incapacidades.
        String Nomina_ID = String.Empty;//Variable que indica la nomina en la que se aplicaran las incapacidades.
        String Detalle_Nomina_ID = String.Empty;//Variable que almacena el identificador del periodo.
        String Fecha_Inicio = String.Empty;//Variable que almacena la fecha de inicio del periodo de nómina.
        String Fecha_Fin = String.Empty;//Variable que almacena la fecha final del periodo nominal.
        Int32 Dias_Incapacidad = 0;//Variable que almacena los dias de incapacidad.
        Int32 Contador = 1;//Variable que cuanta los días.
        DataTable Dt_Incapacidades_ = new DataTable();//Variable que almacena la tabla de incapacidades.
        Int32 Dias_Periodo_Incapacidad = 0;//Variable que almacena los dias del periodo de incapacidad.

        try
        {
            //Se crea la estructura de las tablas que almacenaran los registros de incapacidad.
            Dt_Incapacidades_.Columns.Add("EMPLEADO_ID", typeof(String));
            Dt_Incapacidades_.Columns.Add("DEPENDENCIA_ID", typeof(String));
            Dt_Incapacidades_.Columns.Add("ESTATUS", typeof(String));
            Dt_Incapacidades_.Columns.Add("TIPO_INCAPACIDAD", typeof(String));
            Dt_Incapacidades_.Columns.Add("APLICA_PAGO_CUARTO_DIA", typeof(String));
            Dt_Incapacidades_.Columns.Add("PORCENTAJE_INCAPACIDAD", typeof(String));
            Dt_Incapacidades_.Columns.Add("EXTENCION_INCAPACIDAD", typeof(String));
            Dt_Incapacidades_.Columns.Add("FECHA_INICIO", typeof(String));
            Dt_Incapacidades_.Columns.Add("FECHA_FIN", typeof(String));
            Dt_Incapacidades_.Columns.Add("NOMINA_ID", typeof(String));
            Dt_Incapacidades_.Columns.Add("NO_NOMINA", typeof(String));
            Dt_Incapacidades_.Columns.Add("DIAS_INCAPACIDAD", typeof(String));

            //Obtenemos los dias de incapacidad totales a los que ele mpleado tiene derecho.
            Dias_Incapacidad = Convert.ToInt32(Txt_Dias_Incapacidad.Text.Trim());

            //Validamos que se halla ingresado 
            if (!String.IsNullOrEmpty(Txt_Fecha_Inicio.Text))
            {
                Fecha_Inicio_Incapacidad = Convert.ToDateTime(Txt_Fecha_Inicio.Text.Trim());
                Obj_Incapacidades.P_Fecha_Fin_Incapacidad = String.Format("{0:dd/MM/yyyy}", Fecha_Inicio_Incapacidad);
                Dt_Incapacidades = Obj_Incapacidades.Identificar_Periodo_Nomina_Reloj_Checador();

                if (Dt_Incapacidades is DataTable)
                {
                    if (Dt_Incapacidades.Rows.Count > 0)
                    {
                        foreach (DataRow PERIODO in Dt_Incapacidades.Rows)
                        {
                            if (PERIODO is DataRow)
                            {

                                if (!String.IsNullOrEmpty(PERIODO[Cat_Nom_Calendario_Reloj.Campo_Consecutivo].ToString()))
                                    Detalle_Nomina_ID = PERIODO[Cat_Nom_Calendario_Reloj.Campo_Consecutivo].ToString();

                                if (!String.IsNullOrEmpty(PERIODO[Cat_Nom_Calendario_Reloj.Campo_Nomina_ID].ToString()))
                                    Nomina_ID = PERIODO[Cat_Nom_Calendario_Reloj.Campo_Nomina_ID].ToString();

                                if (!String.IsNullOrEmpty(PERIODO[Cat_Nom_Calendario_Reloj.Campo_No_Nomina].ToString()))
                                    No_Nomina = PERIODO[Cat_Nom_Calendario_Reloj.Campo_No_Nomina].ToString();

                                if (!String.IsNullOrEmpty(PERIODO[Cat_Nom_Calendario_Reloj.Campo_Fecha_Inicio].ToString()))
                                {
                                    Fecha_Inicio = PERIODO[Cat_Nom_Calendario_Reloj.Campo_Fecha_Inicio].ToString();

                                    if (Contador > 1)
                                        Fecha_Inicio_Incapacidad = Convert.ToDateTime(Fecha_Inicio);
                                }

                                if (!String.IsNullOrEmpty(PERIODO[Cat_Nom_Calendario_Reloj.Campo_Fecha_Fin].ToString()))
                                    Fecha_Fin = PERIODO[Cat_Nom_Calendario_Reloj.Campo_Fecha_Fin].ToString();

                                if (Contador > 1)
                                    Validar_No_Duplicar_Incapacidades(Nomina_ID, Convert.ToInt32(No_Nomina), Cmb_Empleados.SelectedValue.Trim(), ref Reporte,
                                        Fecha_Inicio, Fecha_Fin);
                                else
                                    Validar_No_Duplicar_Incapacidades(Nomina_ID, Convert.ToInt32(No_Nomina), Cmb_Empleados.SelectedValue.Trim(), ref Reporte,
                                        String.Format("{0:dd/MM/yyyy}", Fecha_Inicio_Incapacidad), Fecha_Fin);

                                Dias_Periodo_Incapacidad = Convert.ToDateTime(Fecha_Fin).Subtract(Fecha_Inicio_Incapacidad).Days + 1;

                                Int32 Aux = Dias_Incapacidad;
                                Dias_Incapacidad -= Dias_Periodo_Incapacidad;

                                if (Dias_Incapacidad <= 0)
                                {
                                    DataRow Dr_Incapacidad = Dt_Incapacidades_.NewRow();
                                    Dr_Incapacidad["EMPLEADO_ID"] = Cmb_Empleados.SelectedValue.Trim();
                                    Dr_Incapacidad["DEPENDENCIA_ID"] = Cmb_Dependencia.SelectedValue.Trim();
                                    Dr_Incapacidad["ESTATUS"] = Cmb_Estatis.SelectedItem.Text.Trim();
                                    Dr_Incapacidad["TIPO_INCAPACIDAD"] = Cmb_Tipo_Incapacidad.SelectedItem.Text.Trim();
                                    Dr_Incapacidad["APLICA_PAGO_CUARTO_DIA"] = (Chk_Aplica_Pago_Cuarto_Dia.Checked) ? "SI" : "NO";
                                    Dr_Incapacidad["PORCENTAJE_INCAPACIDAD"] = Txt_Porcentaje_Incapacidad.Text.Trim();
                                    Dr_Incapacidad["EXTENCION_INCAPACIDAD"] = (Chk_Aplica_Extencion_Incapacidad.Checked) ? "SI" : "NO";
                                    Dr_Incapacidad["FECHA_INICIO"] = String.Format("{0:dd/MM/yyyy}", Fecha_Inicio_Incapacidad);
                                    Dr_Incapacidad["FECHA_FIN"] = String.Format("{0:dd/MM/yyyy}", Fecha_Inicio_Incapacidad.AddDays((Aux - 1)));
                                    Dr_Incapacidad["NOMINA_ID"] = Nomina_ID;
                                    Dr_Incapacidad["NO_NOMINA"] = No_Nomina;
                                    Dr_Incapacidad["DIAS_INCAPACIDAD"] = Aux;
                                    Dt_Incapacidades_.Rows.Add(Dr_Incapacidad);

                                    ++Contador;
                                    break;
                                }
                                else
                                {
                                    DataRow Dr_Incapacidad = Dt_Incapacidades_.NewRow();
                                    Dr_Incapacidad["EMPLEADO_ID"] = Cmb_Empleados.SelectedValue.Trim();
                                    Dr_Incapacidad["DEPENDENCIA_ID"] = Cmb_Dependencia.SelectedValue.Trim();
                                    Dr_Incapacidad["ESTATUS"] = Cmb_Estatis.SelectedItem.Text.Trim();
                                    Dr_Incapacidad["TIPO_INCAPACIDAD"] = Cmb_Tipo_Incapacidad.SelectedItem.Text.Trim();
                                    Dr_Incapacidad["APLICA_PAGO_CUARTO_DIA"] = (Chk_Aplica_Pago_Cuarto_Dia.Checked) ? "SI" : "NO";
                                    Dr_Incapacidad["PORCENTAJE_INCAPACIDAD"] = Txt_Porcentaje_Incapacidad.Text.Trim();
                                    Dr_Incapacidad["EXTENCION_INCAPACIDAD"] = (Chk_Aplica_Extencion_Incapacidad.Checked) ? "SI" : "NO";
                                    Dr_Incapacidad["FECHA_INICIO"] = String.Format("{0:dd/MM/yyyy}", Fecha_Inicio_Incapacidad);
                                    Dr_Incapacidad["FECHA_FIN"] = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Fecha_Fin));
                                    Dr_Incapacidad["NOMINA_ID"] = Nomina_ID;
                                    Dr_Incapacidad["NO_NOMINA"] = No_Nomina;
                                    Dr_Incapacidad["DIAS_INCAPACIDAD"] = Dias_Periodo_Incapacidad;
                                    Dt_Incapacidades_.Rows.Add(Dr_Incapacidad);

                                    ++Contador;
                                }
                            }
                        }
                    }
                }
            }

            if (Reporte.Length > 0) {
                Dt_Incapacidades_ = new DataTable();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al crear los registros en automatico. Error: [" + Ex.Message + "]");
        }
        return Dt_Incapacidades_;
    }
    ///**********************************************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Evento_Puente
    ///
    ///DESCRIPCIÓN: Metodo que sirve para dos o mas eventos realicen exctamente la misma operación.
    ///
    /// PARÁMETROS: NO APLICA.
    /// 
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 14/Diciembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///***********************************************************************************************************************************************
    protected void Evento_Puente()
    {
        Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;

        try
        {
            if (Es_Numero(Txt_No_Empleado.Text))
                INF_EMPLEADO = Presidencia.Ayudante_Informacion.Cls_Ayudante_Nom_Informacion._Informacion_Empleado(Txt_No_Empleado.Text.Trim());
            else
                INF_EMPLEADO = Presidencia.Ayudante_Informacion.Cls_Ayudante_Nom_Informacion._Informacion_Empleado(Txt_No_Empleado.Text.Trim());

            if (INF_EMPLEADO != null) {
                if (!String.IsNullOrEmpty(INF_EMPLEADO.P_Estatus)) {
                    if (INF_EMPLEADO.P_Estatus.Trim().ToUpper().Equals("ACTIVO"))
                    {
                        if (!String.IsNullOrEmpty(INF_EMPLEADO.P_Dependencia_ID))
                        {
                            Cmb_Dependencia.SelectedIndex = Cmb_Dependencia.Items.IndexOf(Cmb_Dependencia.Items.FindByValue(INF_EMPLEADO.P_Dependencia_ID));

                            if (!String.IsNullOrEmpty(INF_EMPLEADO.P_Empleado_ID))
                            {
                                Consultar_Empleados_Por_Dependencia(INF_EMPLEADO.P_Dependencia_ID);

                                Cmb_Empleados.SelectedIndex = Cmb_Empleados.Items.IndexOf(Cmb_Empleados.Items.FindByValue(INF_EMPLEADO.P_Empleado_ID));
                            }
                        }
                    }
                    else {
                        Lbl_Mensaje_Error.Text = "El el empleado se encuentra inactivo actualmente.";
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar la información del empleado. Error: [" + Ex.Message + "]");
        }
    }
    /// <summary>
    /// Nombre: Validar_No_Duplicar_Incapacidades
    /// 
    /// Descripción: Método que valida que no se puedan capturar incapacidades que ya fueron capturadas previamente.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creo: 11 Mayo 2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// </summary>
    /// <param name="Nomina_ID">Calendario de Nomina en el cuál actualmente se esta capturando la incapacidad.</param>
    /// <param name="No_Nomina">Catorcena en la cuál entrara la incapacidad.</param>
    /// <param name="Empleado_ID">Identificador interno del sistema para el empleado.</param>
    /// <param name="Reporte">Cadena que guardara en buffer las incapacidades que ya fueron capturadas</param>
    /// <param name="Fecha_Inicio">Fecha de inicio de la incapacidad.</param>
    /// <param name="Fecha_Fin">Fecha de termino de la incapacidad.</param>
    private void Validar_No_Duplicar_Incapacidades(String Nomina_ID, Int32 No_Nomina, String Empleado_ID, ref StringBuilder Reporte,
        String Fecha_Inicio, String Fecha_Fin)
    {
        Cls_Ope_Nom_Incapacidades_Negocio Obj_Incapacidades = new Cls_Ope_Nom_Incapacidades_Negocio();
        DataTable Dt_Incapacidades = null;
        Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;
        Cls_Cat_Nom_Calendario_Nominas_Negocio INF_CALENDARIO = new Cls_Cat_Nom_Calendario_Nominas_Negocio();
        DataTable Dt_Calendario_Nomina = null;
        String Anio_Calendario_Nomina = String.Empty;

        try
        {
            INF_CALENDARIO.P_Nomina_ID = Nomina_ID;
            Dt_Calendario_Nomina = INF_CALENDARIO.Consultar_Calendario_Nominas();

            if (Dt_Calendario_Nomina is DataTable)
            {
                var calendario_nomina = from item_calendario in Dt_Calendario_Nomina.AsEnumerable()
                                        select new
                                        {
                                            Anio = item_calendario.IsNull(Cat_Nom_Calendario_Nominas.Campo_Anio) ?
                                                String.Empty : item_calendario.Field<Decimal>(Cat_Nom_Calendario_Nominas.Campo_Anio).ToString()
                                        };

                if (calendario_nomina != null)
                {
                    foreach (var item_calendario_nomina in calendario_nomina)
                    {
                        Anio_Calendario_Nomina = item_calendario_nomina.Anio;
                    }
                }
            }

            INF_EMPLEADO = Presidencia.Ayudante_Informacion.Cls_Ayudante_Nom_Informacion._Informacion_Empleado(Empleado_ID);

            Obj_Incapacidades.P_Nomina_ID = Nomina_ID;
            Obj_Incapacidades.P_No_Nomina = No_Nomina;
            Obj_Incapacidades.P_Empleado_ID = Empleado_ID;
            Obj_Incapacidades.P_Fecha_Inicio_Incapacidad = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Fecha_Inicio));
            Obj_Incapacidades.P_Fecha_Fin_Incapacidad = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Fecha_Fin));

            Dt_Incapacidades = Obj_Incapacidades.Consultar_Incapacidades();

            if (Dt_Incapacidades is DataTable)
            {
                if (Dt_Incapacidades.Rows.Count > 0)
                {
                    foreach (DataRow item_incapacidad in Dt_Incapacidades.Rows)
                    {
                        if (item_incapacidad is DataRow)
                        {
                            Reporte.AppendLine("<br>________________________________________________________________________________<br>");
                            Reporte.AppendLine("No Empleado: " + INF_EMPLEADO.P_No_Empleado + "<br>");

                            Reporte.Append("Empleado : " + INF_EMPLEADO.P_Nombre + " ");
                            Reporte.AppendLine(INF_EMPLEADO.P_Apellido_Paterno + " ");
                            Reporte.AppendLine(INF_EMPLEADO.P_Apelldo_Materno + "<br>");

                            Reporte.AppendLine("Año: " + Anio_Calendario_Nomina + "<br>");
                            Reporte.AppendLine("Catorcena: " + No_Nomina + "<br>");

                            if (!String.IsNullOrEmpty(item_incapacidad[Ope_Nom_Incapacidades.Campo_Fecha_Inicio].ToString()))
                            {
                                Reporte.AppendLine("Fecha Inicio Incapacidad: " + String.Format("{0:dd MMMM yyyy}",
                                    Convert.ToDateTime(item_incapacidad[Ope_Nom_Incapacidades.Campo_Fecha_Inicio].ToString())) + "<br>");
                            }

                            if (!String.IsNullOrEmpty(item_incapacidad[Ope_Nom_Incapacidades.Campo_Fecha_Fin].ToString()))
                            {
                                Reporte.AppendLine("Fecha Termino Incapacidad: " + String.Format("{0:dd MMMM yyyy}",
                                    Convert.ToDateTime(item_incapacidad[Ope_Nom_Incapacidades.Campo_Fecha_Fin].ToString())) + "<br>");
                            }

                            if (!String.IsNullOrEmpty(item_incapacidad[Ope_Nom_Incapacidades.Campo_Dias_Incapacidad].ToString()))
                                Reporte.AppendLine("Dias Incapacidad: " + item_incapacidad[Ope_Nom_Incapacidades.Campo_Dias_Incapacidad].ToString() + "<br>");

                            Reporte.AppendLine("________________________________________________________________________________<br>");
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al validar que existan incapacidades duplicadas. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Consulta Combos)
    ///******************************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Dependencias
    ///
    ///DESCRIPCIÓN: Consulta las dependencias queestan dadas de alta en el sistema.
    /// 
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011 11:50 am 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************************************************************
    private void Consultar_Dependencias()
    {
        Cls_Cat_Dependencias_Negocio Obj_Dependencias = new Cls_Cat_Dependencias_Negocio();//Variable de conexion con la capan de negocios.
        DataTable Dt_Dependencias = null;//Variable que almacenara un listado de las unidades responsables dadas de alta en el sistema.

        try
        {
            Dt_Dependencias = Obj_Dependencias.Consulta_Dependencias();
            LLenar_Combos(Cmb_Dependencia, Dt_Dependencias, Cat_Dependencias.Campo_Dependencia_ID, Cat_Dependencias.Campo_Nombre);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las unidades responsables dadas de alta en el sistema. Error: [" + Ex.Message + "]");
        }
    }
    ///******************************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Dependencias
    ///
    ///DESCRIPCIÓN: Consulta los empleados que pertencen a la dependencia que es pasada como parámetro a este método.
    ///
    ///PARÁMETROS: Dependencia_ID: Dependencia de la cuál se desea consultar sus empleados.
    /// 
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011 11:41 am 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************************************************************
    private void Consultar_Empleados_Por_Dependencia(String Dependencia_ID)
    {
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios.
        DataTable Dt_Empleados = null;//Variable que almacenara la lista de empelados que pertencen a la Unidad Responsable seleccionada.

        try
        {
            Obj_Empleados.P_Dependencia_ID = Dependencia_ID;
            Dt_Empleados = Obj_Empleados.Consulta_Empleados_General();
            LLenar_Combos(Cmb_Empleados, Dt_Empleados, Cat_Empleados.Campo_Empleado_ID, "EMPLEADOS");           
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los Empleados que pertencen a la unidades responsables seleccionada. Error: [" + Ex.Message + "");
        }
    }
    ///******************************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN: LLenar_Combos
    ///
    ///DESCRIPCIÓN: Carga el combo que es pasado como parámetro con la tabla de datos que tambien es pasada como parámetro.
    ///
    ///PARÁMETROS: Combo: [DropDownList] Control donde se cargaran los datos.
    ///            Dt_Datos: Tabla que contiene el listado a mostrar en el combo.
    ///            Valor: Valor del elemento al seleccionar una opcion del combo.
    ///            Texto_Mostrar: Texto que se mostrara al usuario.
    /// 
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011 11:40 am 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************************************************************
    private void LLenar_Combos(DropDownList Combo, DataTable Dt_Datos, String Valor, String Texto_Mostrar)
    {
        try
        {
            if (Dt_Datos is DataTable) {
                if (Dt_Datos.Rows.Count > 0)
                {
                    Combo.DataSource = Dt_Datos;
                    Combo.DataValueField = Valor;
                    Combo.DataTextField = Texto_Mostrar;
                    Combo.DataBind();
                    Combo.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
                    Combo.SelectedIndex = -1;
                }
                else {
                    Combo.DataSource = new DataTable();
                    Combo.DataBind();
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cargar el combo. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Calendario Nomina)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Calendarios_Nomina
    /// DESCRIPCION : 
    /// 
    /// PARAMETROS:
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Calendarios_Nomina()
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nominales = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Calendarios_Nominales = null;//Variable que almacena los calendarios nominales que existén actualmente en el sistema.
        try
        {
            Dt_Calendarios_Nominales = Obj_Calendario_Nominales.Consultar_Calendario_Nominas();
            Dt_Calendarios_Nominales = Formato_Fecha_Calendario_Nomina(Dt_Calendarios_Nominales);

            if (Dt_Calendarios_Nominales is DataTable)
            {
                Cmb_Calendario_Nomina.DataSource = Dt_Calendarios_Nominales;
                Cmb_Calendario_Nomina.DataTextField = "Nomina";
                Cmb_Calendario_Nomina.DataValueField = Cat_Nom_Calendario_Nominas.Campo_Nomina_ID;
                Cmb_Calendario_Nomina.DataBind();
                Cmb_Calendario_Nomina.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));

                Cmb_Calendario_Nomina.SelectedIndex = Cmb_Calendario_Nomina.Items.IndexOf
                    (Cmb_Calendario_Nomina.Items.FindByText(new Presidencia.Ayudante_Calendario_Nomina.Cls_Ayudante_Calendario_Nomina().P_Anyo));

                if (Cmb_Calendario_Nomina.SelectedIndex > 0) {
                    Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario_Nomina.SelectedValue.Trim());
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los calendarios de nómina que existen actualmente registrados en el sistema. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Periodos_Catorcenales_Nomina
    ///DESCRIPCIÓN: Consulta los periodos catorcenales para el 
    ///calendario de nomina seleccionado.
    ///PARAMETROS: Nomina_ID.- Indica el calendario de nomina del cuál se desea consultar
    ///                        los periodos catorcenales.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Periodos_Catorcenales_Nomina(String Nomina_ID)
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Consulta_Calendario_Nomina_Periodos = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Clase de conexion con la capa de negocios
        DataTable Dt_Periodos_Catorcenales = null;//Variable que almacenra unaa lista de los periodos catorcenales que le correspónden a la nomina seleccionada.

        try
        {
            Consulta_Calendario_Nomina_Periodos.P_Nomina_ID = Nomina_ID;
            Dt_Periodos_Catorcenales = Consulta_Calendario_Nomina_Periodos.Consulta_Detalles_Nomina();
            if (Dt_Periodos_Catorcenales != null)
            {
                if (Dt_Periodos_Catorcenales.Rows.Count > 0)
                {
                    Cmb_Periodos_Catorcenales_Nomina.DataSource = Dt_Periodos_Catorcenales;
                    Cmb_Periodos_Catorcenales_Nomina.DataTextField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                    Cmb_Periodos_Catorcenales_Nomina.DataValueField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                    Cmb_Periodos_Catorcenales_Nomina.DataBind();
                    Cmb_Periodos_Catorcenales_Nomina.Items.Insert(0, new ListItem("< Seleccione >", ""));
                    Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = -1;

                    Validar_Periodos_Pago(Cmb_Periodos_Catorcenales_Nomina);

                    Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = Cmb_Periodos_Catorcenales_Nomina.Items.IndexOf(Cmb_Periodos_Catorcenales_Nomina.Items.FindByText(new Presidencia.Ayudante_Calendario_Nomina.Cls_Ayudante_Calendario_Nomina().P_Periodo));
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No se encontraron periodos catorcenales para la nomina seleccionada.";
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los periodos catorcenales del  calendario de nomina seleccionado. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Formato_Fecha_Calendario_Nomina
    /// DESCRIPCION : Crea el DataTable con la consulta de las nomina vigentes en el 
    /// sistema.
    /// PARAMETROS: Dt_Calendario_Nominas.- Lista de las nominas vigentes actualmente 
    ///             en el sistema.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private DataTable Formato_Fecha_Calendario_Nomina(DataTable Dt_Calendario_Nominas)
    {
        DataTable Dt_Nominas = new DataTable();
        DataRow Renglon_Dt_Clon = null;
        Dt_Nominas.Columns.Add("Nomina", typeof(System.String));
        Dt_Nominas.Columns.Add(Cat_Nom_Calendario_Nominas.Campo_Nomina_ID, typeof(System.String));

        if (Dt_Calendario_Nominas is DataTable)
        {
            foreach (DataRow Renglon in Dt_Calendario_Nominas.Rows)
            {
                if (Renglon is DataRow)
                {
                    Renglon_Dt_Clon = Dt_Nominas.NewRow();
                    Renglon_Dt_Clon["Nomina"] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Fecha_Fin].ToString().Split(new char[] { ' ' })[0].Split(new char[] { '/' })[2];
                    Renglon_Dt_Clon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID];
                    Dt_Nominas.Rows.Add(Renglon_Dt_Clon);
                }
            }
        }
        return Dt_Nominas;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Periodos_Pago
    ///DESCRIPCIÓN: Valida que el empleado solo puedan comenzar a descontar la deduccion 
    ///a partir del periodo actual.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Validar_Periodos_Pago(DropDownList Combo)
    {
        Cls_Ope_Nom_Pestamos_Negocio Prestamos = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Detalles_Nomina = null;//Variable que almacenra los detalles del periodo seleccionado.
        DateTime Fecha_Actual = DateTime.Now;
        DateTime Fecha_Inicio = new DateTime();
        DateTime Fecha_Fin = new DateTime();

        Prestamos.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();

        foreach (ListItem Elemento in Combo.Items)
        {
            if (IsNumeric(Elemento.Text.Trim()))
            {
                Prestamos.P_No_Nomina = Convert.ToInt32(Elemento.Text.Trim());
                Dt_Detalles_Nomina = Prestamos.Consultar_Fechas_Periodo();

                if (Dt_Detalles_Nomina != null)
                {
                    if (Dt_Detalles_Nomina.Rows.Count > 0)
                    {
                        Fecha_Inicio = Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString());
                        Fecha_Fin = Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString());

                        //if (Fecha_Fin >= Fecha_Actual)
                        //{
                        //    Elemento.Enabled = true;
                        //}
                        //else
                        //{
                        //    Elemento.Enabled = false;
                        //}
                    }
                }
            }
        }
    }
    #endregion

    #region (Validaciones)
    ///*********************************************************************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos_Operacion
    /// 
    /// DESCRIPCION : Validar datos requeridos para realizar la operación.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*********************************************************************************************************************************
    private Boolean Validar_Datos_Operacion()
    {
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

        try
        {
            if (string.IsNullOrEmpty(Txt_Porcentaje_Incapacidad.Text.Trim()))
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El [%] Porcentaje de Incapacidad es un dato requerido por la operación. <br>";
                Datos_Validos = false;
            }

            if (string.IsNullOrEmpty(Txt_Fecha_Inicio.Text.Trim()))
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La Fecha de Inicio es un dato requerido por la operación. <br>";
                Datos_Validos = false;
            }
            else {
                if (!Validar_Formato_Fecha(Txt_Fecha_Inicio.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + el Formato de la Fecha de Inicio es incorrecto. Formato [Dia/Mes/Año]. <br>";
                    Datos_Validos = false;
                }
            }

            if (string.IsNullOrEmpty(Txt_Fecha_Fin.Text.Trim()))
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La Fecha Final es un dato requerido por la operación. <br>";
                Datos_Validos = false;
            }
            else
            {
                if (!Validar_Formato_Fecha(Txt_Fecha_Fin.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + el Formato de la Fecha de Fin es incorrecto. Formato [Dia/Mes/Año]. <br>";
                    Datos_Validos = false;
                }
            }

            if (Cmb_Estatis.SelectedIndex <= 0)
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Estatus es un dato requerido por la operación. <br>";
                Datos_Validos = false;
            }

            if (Cmb_Dependencia.SelectedIndex <= 0)
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La Dependecia es un dato requerido por la operación. <br>";
                Datos_Validos = false;
            }

            if (Cmb_Empleados.SelectedIndex <= 0)
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Empleado es un dato requerido por la operación. <br>";
                Datos_Validos = false;
            }

            if (Cmb_Tipo_Incapacidad.SelectedIndex <= 0)
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Tipo de Incapacidad es un dato requerido por la operación. <br>";
                Datos_Validos = false;
            }

            //if (Validar_Formato_Fecha(Txt_Fecha_Inicio.Text.Trim()) && Validar_Formato_Fecha(Txt_Fecha_Fin.Text.Trim()))
            //{
            //    if (!Validar_Fecha_Inicio_Menor_Fecha_Fin(Txt_Fecha_Inicio.Text.Trim(), Txt_Fecha_Fin.Text.Trim()))
            //    {
            //        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La Fecha Inicio de Incapacidad no puede ser mayor a la Fecha Final. Y no puede ser menor o igual a la fecha actual. <br>";
            //        Datos_Validos = false;
            //    }
            //}

            if (Existen_Incapacidades_Pendientes())
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Actualmente existen solicitudes de incapacidaes pendientes para el empleado seleccionado. <br>";
                Datos_Validos = false;
            }

            if (string.IsNullOrEmpty(Txt_Dias_Incapacidad.Text.Trim()))
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Los dias de incapacidad es un dato requerido por la operación. <br>";
                Datos_Validos = false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al validar los datos de la incapacidad del empleado. Error: [" + Ex.Message + "]");
        }
        return Datos_Validos;
    }
    ///*********************************************************************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Formato_Fecha
    /// 
    /// DESCRIPCION : Valida el formato de las fechas.
    /// 
    /// PARÁMETROS: Fecha: Fecha a validar su formato.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*********************************************************************************************************************************
    private Boolean Validar_Formato_Fecha(String Fecha)
    {
        String Cadena_Fecha = String.Empty;//Variable que almacena el formato que debera respetar la fecha.

        try
        {
            Cadena_Fecha = @"^(([0-9])|([0-2][0-9])|([3][0-1]))\/(ene|feb|mar|abr|may|jun|jul|ago|sep|oct|nov|dic)\/\d{4}$";
            if (Fecha != null) return Regex.IsMatch(Fecha, Cadena_Fecha);
            else return false;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al validar la fecha que es pasada como parametro al metodo. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: IsNumeric
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean IsNumeric(String Cadena)
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
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Fecha_Inicio_Menor_Fecha_Fin
    /// DESCRIPCION : Valida que la fecha de inicio sea menor a la final.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Fecha_Inicio_Menor_Fecha_Fin(String Fecha_Inicio, String Fecha_Fin)
    {
        DateTime? Fecha_Inicio_Incapacidad = null;//Variable que almacenar la fecha de inicio de la incapacidad.
        DateTime? Fecha_Final_Incapacidad = null;//Variable que almacenara la fecha final de la incapaciadad.
        Boolean Estatus = false;//Variable que almacena [true/false]. True si la Fecha de inicio es menor a la final y false en caso contrario.

        try
        {
            Fecha_Inicio_Incapacidad = Convert.ToDateTime(Fecha_Inicio);
            Fecha_Final_Incapacidad = Convert.ToDateTime(Fecha_Fin);

            if (((DateTime)Fecha_Inicio_Incapacidad) < ((DateTime)Fecha_Final_Incapacidad))
            {
                Estatus = true;
            }

            if (((DateTime)Fecha_Inicio_Incapacidad) > DateTime.Today) {
                Estatus = true;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al validar que la fecha de inicio no sea mayor a la fecha de final. Error: [" + Ex.Message + "]");
        }
        return Estatus;
    }
    #endregion

    #region(Operaciones [Alta - Modificar - Eliminar - Consultar])
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Alta_Incapacidad
    ///
    ///DESCRIPCIÓN: Método que realiza el alta de la incapacidad al empleado.
    ///
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011 11:51 am
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Alta_Incapacidad(ref StringBuilder Reporte)
    {
        Cls_Ope_Nom_Incapacidades_Negocio Obj_Incapacidades = new Cls_Ope_Nom_Incapacidades_Negocio();//Variable de conexion con la capa de negocios.       
        DataTable Dt_Incapacidades = null;
        String Empleado_ID = String.Empty;

        try
        {
            Dt_Incapacidades = Crear_Registros_Incapacidad(ref Reporte);

            if (Dt_Incapacidades is DataTable)
            {
                if (Dt_Incapacidades.Rows.Count > 0)
                {
                    foreach (DataRow INCAPACIDAD in Dt_Incapacidades.Rows)
                    {
                        if (INCAPACIDAD is DataRow)
                        {

                            if (!String.IsNullOrEmpty(INCAPACIDAD["EMPLEADO_ID"].ToString()))
                                Obj_Incapacidades.P_Empleado_ID = INCAPACIDAD["EMPLEADO_ID"].ToString().Trim();

                            if (!String.IsNullOrEmpty(INCAPACIDAD["DEPENDENCIA_ID"].ToString()))
                                Obj_Incapacidades.P_Dependencia_ID = INCAPACIDAD["DEPENDENCIA_ID"].ToString().Trim();

                            if (!String.IsNullOrEmpty(INCAPACIDAD["ESTATUS"].ToString()))
                                Obj_Incapacidades.P_Estatus = INCAPACIDAD["ESTATUS"].ToString().Trim();

                            if (!String.IsNullOrEmpty(INCAPACIDAD["TIPO_INCAPACIDAD"].ToString()))
                                Obj_Incapacidades.P_Tipo_Incapacidad = INCAPACIDAD["TIPO_INCAPACIDAD"].ToString().Trim();

                            if (!String.IsNullOrEmpty(INCAPACIDAD["APLICA_PAGO_CUARTO_DIA"].ToString()))
                                Obj_Incapacidades.P_Aplica_Pago_Cuarto_Dia = INCAPACIDAD["APLICA_PAGO_CUARTO_DIA"].ToString().Trim();

                            if (!String.IsNullOrEmpty(INCAPACIDAD["PORCENTAJE_INCAPACIDAD"].ToString()))
                                Obj_Incapacidades.P_Porcentaje_Incapacidad = Convert.ToDouble(INCAPACIDAD["PORCENTAJE_INCAPACIDAD"].ToString().Trim());

                            if (!String.IsNullOrEmpty(INCAPACIDAD["EXTENCION_INCAPACIDAD"].ToString()))
                                Obj_Incapacidades.P_Extencion_Incapacidad = INCAPACIDAD["EXTENCION_INCAPACIDAD"].ToString().Trim();

                            if (!String.IsNullOrEmpty(INCAPACIDAD["FECHA_INICIO"].ToString()))
                                Obj_Incapacidades.P_Fecha_Inicio_Incapacidad = INCAPACIDAD["FECHA_INICIO"].ToString().Trim();

                            if (!String.IsNullOrEmpty(INCAPACIDAD["FECHA_FIN"].ToString()))
                                Obj_Incapacidades.P_Fecha_Fin_Incapacidad = INCAPACIDAD["FECHA_FIN"].ToString().Trim();

                            if (!String.IsNullOrEmpty(INCAPACIDAD["NOMINA_ID"].ToString()))
                                Obj_Incapacidades.P_Nomina_ID = INCAPACIDAD["NOMINA_ID"].ToString().Trim();

                            if (!String.IsNullOrEmpty(INCAPACIDAD["NO_NOMINA"].ToString()))
                                Obj_Incapacidades.P_No_Nomina = Convert.ToInt32(INCAPACIDAD["NO_NOMINA"].ToString().Trim());

                            if (!String.IsNullOrEmpty(INCAPACIDAD["DIAS_INCAPACIDAD"].ToString()))
                                Obj_Incapacidades.P_Dias_Incapacidad = Convert.ToInt32(INCAPACIDAD["DIAS_INCAPACIDAD"].ToString().Trim());

                            Obj_Incapacidades.P_Comentarios = Txt_Comentarios.Text.Trim();
                            Obj_Incapacidades.P_Usuario_Creo = Cls_Sessiones.Empleado_ID;
                            Obj_Incapacidades.Alta_Icapacidad();

                            Obj_Incapacidades = new Cls_Ope_Nom_Incapacidades_Negocio();
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el alta de una incapacidad. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Modificar_Incapacidad
    ///
    ///DESCRIPCIÓN: Método que realiza la modificacion de la incapacidad al empleado.
    ///
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011 11:51 am
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Modificar_Incapacidad()
    {
        Cls_Ope_Nom_Incapacidades_Negocio Obj_Incapacidades = new Cls_Ope_Nom_Incapacidades_Negocio();//Variable de conexion con la capa de negocios.       

        try
        {
            Obj_Incapacidades.P_No_Incapacidad = Txt_No_Incapacidad.Text.Trim();
            Obj_Incapacidades.P_Estatus = Cmb_Estatis.SelectedItem.Text.Trim();
            Obj_Incapacidades.P_Tipo_Incapacidad = Cmb_Tipo_Incapacidad.SelectedItem.Text.Trim();
            Obj_Incapacidades.P_Aplica_Pago_Cuarto_Dia = (Chk_Aplica_Pago_Cuarto_Dia.Checked) ? "SI" : "NO";
            Obj_Incapacidades.P_Extencion_Incapacidad = (Chk_Aplica_Extencion_Incapacidad.Checked) ? "SI" : "NO";
            Obj_Incapacidades.P_Porcentaje_Incapacidad = Convert.ToDouble(Txt_Porcentaje_Incapacidad.Text.Trim());
            Obj_Incapacidades.P_Fecha_Inicio_Incapacidad = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(HttpUtility.HtmlDecode(Txt_Fecha_Inicio.Text.Trim())));
            Obj_Incapacidades.P_Fecha_Fin_Incapacidad = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(HttpUtility.HtmlDecode(Txt_Fecha_Fin.Text.Trim())));
            Obj_Incapacidades.P_Comentarios = Txt_Comentarios.Text.Trim();
            Obj_Incapacidades.P_Nomina_ID = (Cmb_Calendario_Nomina.SelectedIndex > 0) ? Cmb_Calendario_Nomina.SelectedValue : "NULL";
            Obj_Incapacidades.P_No_Nomina = (Cmb_Periodos_Catorcenales_Nomina.SelectedIndex > 0) ? Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedItem.Text.Trim()) : 0;
            Obj_Incapacidades.P_Usuario_Modifico = Cls_Sessiones.Empleado_ID;
            Obj_Incapacidades.P_Dias_Incapacidad = Convert.ToInt32(Txt_Dias_Incapacidad.Text.Trim());

            if (Obj_Incapacidades.Modificar_Icapacidad())
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Modificar Incapacidad", "_alert();", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al modificar de una incapacidad. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Eliminar_Incapacidad
    ///
    ///DESCRIPCIÓN: Método que realiza la baja de la incapacidad al empleado.
    ///
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011 11:51 am
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Eliminar_Incapacidad()
    {
        Cls_Ope_Nom_Incapacidades_Negocio Obj_Incapacidades = new Cls_Ope_Nom_Incapacidades_Negocio();//Variable de conexion con la capa de negocios.       

        try
        {
            Obj_Incapacidades.P_No_Incapacidad = Txt_No_Incapacidad.Text.Trim();

            if (Obj_Incapacidades.Eliminar_Icapacidad())
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Eliminar Incapacidad", "_alert();", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al Eliminar una incapacidad. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Control Dias Ajustables)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Calcular_Fecha_Termino_Vacaciones
    /// DESCRIPCION : Calcula la fecha de termino de las vacaciones del empleado
    /// dependiendo de la fecha de inicio seleccionada, sus dias laborales de la semana y
    /// y los dias solicitados.
    /// PARAMETROS: Fecha_Termino_Vacaciones.- Fecha inicial de las vacaciones que al final se
    ///                                        convertira en la fecha de termino de las mismas.
    ///             Dias_Solicitados.- No de dias de vacaciones solicitados por el empleado.
    ///             
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************  
    private DateTime Calcular_Fecha_Termino_Incapacidad(DateTime Fecha_Termino_Incapacidad, Int32 Dias_Incapacidad)
    {
        Boolean[] Dias_Laborales_Semana ;//Varuable que almacenara la configuracion laboral de los dias de la semana para el empleado.

        try
        {
            Dias_Laborales_Semana = Obtener_Dias_Laborales_Empleado();

            //Se realiza un barrido teniendo como fin del ciclo el numero de dia de vacaciones que solicito el empleado.
            for (int index = 1; index <= Dias_Incapacidad; index++)
            {
                //verificamos si es un dia laboral para el empleado.
                if (Verifica_Dia_Laboral_Empleado(Fecha_Termino_Incapacidad, Fecha_Termino_Incapacidad.DayOfWeek, Dias_Laborales_Semana))
                {
                    //Verificamos que no sea el ultimo dia de vacaciones. si es el ultimo dia de vacaciones,
                    //no se realiza el incremente del dia y se toma como fecha termino de vacaciones.
                    //Si no es el ultimo se agrega un dia a la fecha y se continua con el proceso.
                    if (index != Dias_Incapacidad)
                    {
                        Fecha_Termino_Incapacidad = Fecha_Termino_Incapacidad.AddDays(1);
                    }
                }
                else
                {
                    //Si no es un dia laboral para el empleado. 
                    //Se incrementa el dia a la fecha pero el dia no se cuenta como dia de vacacion para el empleado.
                    Fecha_Termino_Incapacidad = Fecha_Termino_Incapacidad.AddDays(1);
                    --index;//Decrementamos el indice para no contarlo como dia vacacional.
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
        return Fecha_Termino_Incapacidad;
    }
    ///***********************************************************************************************************************************************
    /// NOMBRE DE LA FUNCION: Calcular_Fecha_Termino_Vacaciones
    /// DESCRIPCION : Obtiene un matriz con lo días laborales del empleado.
    ///             
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 12/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///***********************************************************************************************************************************************  
    private Boolean [] Obtener_Dias_Laborales_Empleado()
    {
        Cls_Cat_Empleados_Negocios Cat_Empleados_Consulta = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios.
        DataTable Dt_Empleados = null;  //Variable que almacenara una lista de empleados consultados.
        Boolean LUNES = false;          //Dia Lunes
        Boolean MARTES = false;         //Dia Martes
        Boolean MIERCOLES = false;      //Dia Miercoles
        Boolean JUEVES = false;         //Dia Jueves
        Boolean VIERNES = false;        //Dia Viernes
        Boolean SABADO = false;         //Dia Sabado
        Boolean DOMINGO = false;        //Dia Domingo
        Boolean[] Dias_Laborales_Semana = new Boolean[7];//Varuable que almacenara la configuracion laboral de los dias de la semana para el empleado.

        try
        {
            Cat_Empleados_Consulta.P_Empleado_ID = Cmb_Empleados.SelectedValue.Trim();//Establecemos el no de empleado a consultar.
            Dt_Empleados = Cat_Empleados_Consulta.Consulta_Empleados_General();//Ejecutamos la consulta de los empleados.
            //Validamos que la consulta halla encontrado resultados.
            if (Dt_Empleados != null)
            {
                //Vallidamos que por lo menos exista un registro.
                if (Dt_Empleados.Rows.Count > 0)
                {
                    //Obtenemos la configuracion laboral de los dias que si labora el empleado por semana.
                    if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Lunes].ToString())) LUNES = (Dt_Empleados.Rows[0][Cat_Empleados.Campo_Lunes].ToString().Trim().ToUpper().Equals("SI")) ? true : false;
                    if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Martes].ToString())) MARTES = (Dt_Empleados.Rows[0][Cat_Empleados.Campo_Martes].ToString().Trim().ToUpper().Equals("SI")) ? true : false;
                    if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Miercoles].ToString())) MIERCOLES = (Dt_Empleados.Rows[0][Cat_Empleados.Campo_Miercoles].ToString().Trim().ToUpper().Equals("SI")) ? true : false;
                    if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Jueves].ToString())) JUEVES = (Dt_Empleados.Rows[0][Cat_Empleados.Campo_Jueves].ToString().Trim().ToUpper().Equals("SI")) ? true : false;
                    if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Viernes].ToString())) VIERNES = (Dt_Empleados.Rows[0][Cat_Empleados.Campo_Viernes].ToString().Trim().ToUpper().Equals("SI")) ? true : false;
                    if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Sabado].ToString())) SABADO = (Dt_Empleados.Rows[0][Cat_Empleados.Campo_Sabado].ToString().Trim().ToUpper().Equals("SI")) ? true : false;
                    if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Domingo].ToString())) DOMINGO = (Dt_Empleados.Rows[0][Cat_Empleados.Campo_Domingo].ToString().Trim().ToUpper().Equals("SI")) ? true : false;
                }
            }

            //Pasamos la configuracion laboral a la variable que los almacenara.
            Dias_Laborales_Semana[0] = LUNES;
            Dias_Laborales_Semana[1] = MARTES;
            Dias_Laborales_Semana[2] = MIERCOLES;
            Dias_Laborales_Semana[3] = JUEVES;
            Dias_Laborales_Semana[4] = VIERNES;
            Dias_Laborales_Semana[5] = SABADO;
            Dias_Laborales_Semana[6] = DOMINGO;
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
        return Dias_Laborales_Semana;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Verifica_Dia_Laboral_Empleado
    /// DESCRIPCION : Verifica si el dia de la semana es un dia laboral para el empleado.
    /// PARAMETROS:  DayOfWeek Dia.- Dia de la semana a evaluar.
    ///              Dias_Laborales_Semana.- Programacion de dias laborales del empleado.
    ///              
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 12/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///******************************************************************************* 
    private Boolean Verifica_Dia_Laboral_Empleado(DateTime Fecha, DayOfWeek Dia, Boolean[] Dias_Laborales_Semana)
    {
        Boolean Resultado = false;//Variable que almacenara el valor si es o no un dia laboral para el empleado.
        //Se realiza la validacion del dia laboral del empleado.
        switch (Dia)
        {
            case DayOfWeek.Monday:
                if (Dias_Laborales_Semana[0]) Resultado = true;
                if (Es_Un_Dia_Festivo(Fecha)) Resultado = false; 
                break;
            case DayOfWeek.Tuesday:
                if (Dias_Laborales_Semana[1]) Resultado = true;
                if (Es_Un_Dia_Festivo(Fecha)) Resultado = false; 
                break;
            case DayOfWeek.Wednesday:
                if (Dias_Laborales_Semana[2]) Resultado = true;
                if (Es_Un_Dia_Festivo(Fecha)) Resultado = false; 
                break;
            case DayOfWeek.Thursday:
                if (Dias_Laborales_Semana[3]) Resultado = true;
                if (Es_Un_Dia_Festivo(Fecha)) Resultado = false; 
                break;
            case DayOfWeek.Friday:
                if (Dias_Laborales_Semana[4]) Resultado = true;
                if (Es_Un_Dia_Festivo(Fecha)) Resultado = false; 
                break;
            case DayOfWeek.Saturday:
                if (Dias_Laborales_Semana[5]) Resultado = true;
                if (Es_Un_Dia_Festivo(Fecha)) Resultado = false; 
                break;
            case DayOfWeek.Sunday:
                if (Dias_Laborales_Semana[6]) Resultado = true;
                if (Es_Un_Dia_Festivo(Fecha)) Resultado = false; 
                break;
            default:
                Resultado = false;
                break;
        }
        return Resultado;
    }
    ///***************************************************************************************************************************************************************
    /// NOMBRE DE LA FUNCION: No_Es_Un_Dia_Festivo
    /// 
    /// DESCRIPCION : Consulta los dias festivos que se encuentran dados de alta en el sistema
    ///             
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 08/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///***************************************************************************************************************************************************************
    public Boolean Es_Un_Dia_Festivo(DateTime Dia)
    {
        Cls_Tab_Nom_Dias_Festivos_Negocios Obj_Tabulador_Dias_Festivos = new Cls_Tab_Nom_Dias_Festivos_Negocios();//Variable de conexion con la capa de negocios.
        DataTable Dt_Tabulador_Dias_Festivos = null;//Variable que almacenara la lista de dias festivos registrados en el sistema para la nomina seleccionada.
        DateTime? Fecha_Dia_Festivo = null;//Variable que almacenara la fecha del dia festivo.
        Boolean Estatus = false;//Variable que almacenara [true/false]. True si el dia corresponde a un dia festivo. False si no es un dia festivo.

        try
        {
            Obj_Tabulador_Dias_Festivos.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
            Dt_Tabulador_Dias_Festivos = Obj_Tabulador_Dias_Festivos.Consulta_Datos_Dia_Festivo();

            if (Dt_Tabulador_Dias_Festivos is DataTable)
            {
                if (Dt_Tabulador_Dias_Festivos.Rows.Count > 0)
                {
                    foreach (DataRow Renglon in Dt_Tabulador_Dias_Festivos.Rows)
                    {
                        if (Renglon is DataRow)
                        {
                            if (!string.IsNullOrEmpty(Renglon[Tab_Nom_Dias_Festivos.Campo_Fecha].ToString().Trim()))
                            {
                                Fecha_Dia_Festivo = Convert.ToDateTime(Renglon[Tab_Nom_Dias_Festivos.Campo_Fecha].ToString().Trim());

                                if (DateTime.Compare(((DateTime)Fecha_Dia_Festivo), Dia) == 0)
                                {
                                    Estatus = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar si el dia no corresponde a un dia festivo. Error: [" + Ex.Message + "]");
        }
        return Estatus;
    }
    #endregion

    #region (Consultar Incapacidades Pendientes)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Existen_Incapacidades_Pendientes
    /// 
    /// DESCRIPCION : Valida is existen actualmente solicitudes pendientes por autorizar
    ///               incapacidades.
    ///               
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 12/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Existen_Incapacidades_Pendientes()
    {
        Cls_Ope_Nom_Incapacidades_Negocio Obj_Incapacidades = new Cls_Ope_Nom_Incapacidades_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Incapacidades = null;//Variable que almacenara el listado de incapacidades registradas al empelado con un estatus de pendiente.
        Boolean Hay_Alguna_Solicitud_Incapacidad_Pendiente = false;//Variable que almacena el valor true si existe alguna solicitud de incapacidad vigente.

        try
        {
            Obj_Incapacidades.P_Empleado_ID = Cmb_Empleados.SelectedValue.Trim();
            Obj_Incapacidades.P_Estatus = "Pendiente";
            Dt_Incapacidades = Obj_Incapacidades.Consultar_Incapacidades();

            if (Dt_Incapacidades is DataTable) {
                if (Dt_Incapacidades.Rows.Count > 0) {
                    Hay_Alguna_Solicitud_Incapacidad_Pendiente = true;
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar si existen incapacidades pendientes por autorizar del empleado. Error: [" + Ex.Message + "]");
        }
        return Hay_Alguna_Solicitud_Incapacidad_Pendiente;
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
            Botones.Add(Btn_Busqueda_Incapacidad);

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

    #region (Grid)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Incapacidades_Empleados
    ///
    ///DESCRIPCIÓN: Método que realiza la consulta las incapacidades que tiene el 
    ///             empleado dadas de alta en el sistema.
    ///
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Incapacidades_Empleados(String Empleado_ID)
    {
        Cls_Ope_Nom_Incapacidades_Negocio Obj_Incapacidades = new Cls_Ope_Nom_Incapacidades_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Incapacidades_Empleado = null;//Variable que almacenara una lista de las incapacidades que tiene dadas de alta el empleado.

        try
        {
            Obj_Incapacidades.P_Empleado_ID = Empleado_ID;
            Dt_Incapacidades_Empleado = Obj_Incapacidades.Consultar_Incapacidades();
            Grid_Incapacidades.Columns[2].Visible = true;
            LLenar_Grid(Grid_Incapacidades, Dt_Incapacidades_Empleado, 0);
            Grid_Incapacidades.Columns[2].Visible = false;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las incapacidades que tiene el empleado. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: LLenar_Grid
    ///
    ///DESCRIPCIÓN: Método que carga la tabla de incapacidades.
    ///
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void LLenar_Grid(GridView Tabla, DataTable Dt_Datos, Int32 Pagina)
    {
        try
        {
            if (Dt_Datos is DataTable)
            {
                Tabla.DataSource = Dt_Datos;
                Tabla.DataBind();
                Tabla.PageIndex = Pagina;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cargar la tabla de incapacidades de los empleados. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Incapacidades_SelectedIndexChanged
    ///
    ///DESCRIPCIÓN: Evento que obtiene la información del elemento seleccionado
    ///             de la tabla de incapacidades.
    ///
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Incapacidades_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_Nom_Incapacidades_Negocio Obj_Incapacidad = new Cls_Ope_Nom_Incapacidades_Negocio();//Variable de conexion con la capa de negocios.
        String Incapacidad_Seleccionada = String.Empty;//Variable que almacena el identificador de la incapacidad seleccionada.
        DataTable Dt_Informacion_Incapacidad = null;//Variable que almacenara los datos de la incapacidad seleccionada.

        try
        {
            if (Grid_Incapacidades.SelectedIndex != -1) {
                Incapacidad_Seleccionada = Grid_Incapacidades.SelectedRow.Cells[1].Text.Trim();

                Obj_Incapacidad.P_No_Incapacidad = Incapacidad_Seleccionada;
                Dt_Informacion_Incapacidad = Obj_Incapacidad.Consultar_Incapacidades();

                if (Dt_Informacion_Incapacidad is DataTable) {
                    if (Dt_Informacion_Incapacidad.Rows.Count > 0) {
                        foreach (DataRow Renglon in Dt_Informacion_Incapacidad.Rows) {
                            if (Renglon is DataRow) {
                                if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Incapacidades.Campo_No_Incapacidad].ToString()))
                                    Txt_No_Incapacidad.Text = Renglon[Ope_Nom_Incapacidades.Campo_No_Incapacidad].ToString();

                                if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Incapacidades.Campo_Dependencia_ID].ToString()))
                                {
                                    Cmb_Dependencia.SelectedIndex = Cmb_Dependencia.Items.IndexOf(Cmb_Dependencia.Items.FindByValue(Renglon[Ope_Nom_Incapacidades.Campo_Dependencia_ID].ToString()));
                                    Consultar_Empleados_Por_Dependencia(Cmb_Dependencia.SelectedValue.Trim());

                                    if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Incapacidades.Campo_Empleado_ID].ToString()))
                                        Cmb_Empleados.SelectedIndex = Cmb_Empleados.Items.IndexOf(Cmb_Empleados.Items.FindByValue(Renglon[Ope_Nom_Incapacidades.Campo_Empleado_ID].ToString()));
                                }

                                if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Incapacidades.Campo_Estatus].ToString().Trim()))
                                    Cmb_Estatis.SelectedIndex = Cmb_Estatis.Items.IndexOf(Cmb_Estatis.Items.FindByText(Renglon[Ope_Nom_Incapacidades.Campo_Estatus].ToString().Trim()));

                                if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Incapacidades.Campo_Tipo_Incapacidad].ToString()))
                                    Cmb_Tipo_Incapacidad.SelectedIndex = Cmb_Tipo_Incapacidad.Items.IndexOf(Cmb_Tipo_Incapacidad.Items.FindByText(Renglon[Ope_Nom_Incapacidades.Campo_Tipo_Incapacidad].ToString()));

                                if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Incapacidades.Campo_Aplica_Pago_Cuarto_Dia].ToString()))
                                    Chk_Aplica_Pago_Cuarto_Dia.Checked = (Renglon[Ope_Nom_Incapacidades.Campo_Aplica_Pago_Cuarto_Dia].ToString().Trim().ToUpper().Equals("SI")) ? true : false;

                                if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Incapacidades.Campo_Porcentaje_Incapacidad].ToString()))
                                    Txt_Porcentaje_Incapacidad.Text = Renglon[Ope_Nom_Incapacidades.Campo_Porcentaje_Incapacidad].ToString().Trim();

                                if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Incapacidades.Campo_Extencion_Incapacidad].ToString()))
                                    Chk_Aplica_Extencion_Incapacidad.Checked = (Renglon[Ope_Nom_Incapacidades.Campo_Extencion_Incapacidad].ToString().Trim().ToUpper().Equals("SI")) ? true : false;

                                if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Incapacidades.Campo_Fecha_Inicio].ToString()))
                                    Txt_Fecha_Inicio.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Renglon[Ope_Nom_Incapacidades.Campo_Fecha_Inicio].ToString().Trim()));

                                if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Incapacidades.Campo_Fecha_Fin].ToString()))
                                    Txt_Fecha_Fin.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Renglon[Ope_Nom_Incapacidades.Campo_Fecha_Fin].ToString().Trim()));

                                if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Incapacidades.Campo_Comentario].ToString()))
                                    Txt_Comentarios.Text = Renglon[Ope_Nom_Incapacidades.Campo_Comentario].ToString().Trim();

                                if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Incapacidades.Campo_Dias_Incapacidad].ToString()))
                                    Txt_Dias_Incapacidad.Text = Renglon[Ope_Nom_Incapacidades.Campo_Dias_Incapacidad].ToString().Trim();

                                if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Incapacidades.Campo_Nomina_ID].ToString()))
                                {
                                    Cmb_Calendario_Nomina.SelectedIndex = Cmb_Calendario_Nomina.Items.IndexOf(Cmb_Calendario_Nomina.Items.FindByValue(Renglon[Ope_Nom_Incapacidades.Campo_Nomina_ID].ToString()));
                                    Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario_Nomina.SelectedValue.Trim());

                                    if (!string.IsNullOrEmpty(Renglon[Ope_Nom_Incapacidades.Campo_No_Nomina].ToString().Trim()))
                                        Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = Cmb_Periodos_Catorcenales_Nomina.Items.IndexOf(Cmb_Periodos_Catorcenales_Nomina.Items.FindByText(Renglon[Ope_Nom_Incapacidades.Campo_No_Nomina].ToString().Trim()));
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }
    #endregion

    #region (Eventos)

    #region (Eventos Botones)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///
    ///DESCRIPCIÓN: Evento que genera el Alta de la incapacidad.
    ///
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(Object sender, EventArgs e) {
        StringBuilder Reporte = new StringBuilder();

        try
        {
            if (Btn_Nuevo.ToolTip.Equals("Nuevo"))
            {                
                Limpiar_Controles();
                Habilitar_Controles("Nuevo");
            }
            else
            {
                if (Validar_Datos_Operacion())
                {
                    Alta_Incapacidad(ref Reporte);

                    if (Reporte.Length <= 0)
                    {
                        Limpiar_Controles();
                        Habilitar_Controles("Nuevo");
                        //Configuracion_Inicial();
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Información", "javascript:alert('Operación Completa');", true);
                    }
                    else {
                        Lbl_Mensaje_Error.Text = Reporte.ToString();
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                    }
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
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///
    ///DESCRIPCIÓN: Evento que genera la modificacion de la incapacidad.
    ///
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(Object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            if (Btn_Modificar.ToolTip.Equals("Modificar"))
            {
                if (Grid_Incapacidades.SelectedIndex != -1 & !Txt_No_Incapacidad.Text.Equals(""))
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
                if (Validar_Datos_Operacion())
                {
                    Modificar_Incapacidad();
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
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
    ///
    ///DESCRIPCIÓN: Evento que genera la baja de la incapacidad.
    ///
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(Object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            if (Btn_Eliminar.ToolTip.Equals("Eliminar"))
            {
                if (Grid_Incapacidades.SelectedIndex != -1 & !Txt_No_Incapacidad.Text.Equals(""))
                {
                    Eliminar_Incapacidad();
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
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///
    ///DESCRIPCIÓN: Evento que cancela la operacion actual o nos da la opcion de salir
    ///             del formulario.
    ///
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(Object sender, EventArgs e)
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
                Limpiar_Controles();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Incapacidad_Click
    ///
    ///DESCRIPCIÓN: Evento que realiza la búsqueda de de la incapacidad. 
    ///
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Busqueda_Incapacidad_Click(object sender, EventArgs e)
    {
        try
        {
            if (Cmb_Empleados.SelectedIndex > 0)
            {
                Consultar_Incapacidades_Empleados(Cmb_Empleados.SelectedValue.Trim());
            }
            else {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Seleccione el empleado a consultar las Incapacidades que a tenido.";
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las incapacidades que tiene el empleado seleccionado. Error: [" + Ex.Message + "]");
        }
    }

    protected void Btn_Consultar_Click(Object sender, EventArgs e)
    {      
        try
        {
            Evento_Puente();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar la información del empleado. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Eventos Combos)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Calendario_Nomina_SelectedIndexChanged
    ///DESCRIPCIÓN: Consulta los periodos catorcenales de la nomina seleccionada.
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Calendario_Nomina_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 index = Cmb_Calendario_Nomina.SelectedIndex;
        if (index > 0)
        {
            Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario_Nomina.SelectedValue.Trim());
        }
        else
        {
            Cmb_Periodos_Catorcenales_Nomina.DataSource = new DataTable();
            Cmb_Periodos_Catorcenales_Nomina.DataBind();
        }
        Cmb_Calendario_Nomina.Focus();
    }
    ///**********************************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Dependencia_SelectedIndexChanged
    ///DESCRIPCIÓN: Consulta los empleados que pertencen a la unidad responsable seleccionada.
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///**********************************************************************************************
    protected void Cmb_Dependencia_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 index = Cmb_Dependencia.SelectedIndex;//Variable que almacena el indice de la unidad responsable seleccionada.

        if (index > 0)
        {
            Consultar_Empleados_Por_Dependencia(Cmb_Dependencia.SelectedValue.Trim());
        }
        else
        {
            Cmb_Empleados.DataSource = new DataTable();
            Cmb_Empleados.DataBind();
        }
        Cmb_Dependencia.Focus();
    }
    ///**********************************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Tipo_Incapacidad_SelectedIndexChanged
    ///DESCRIPCIÓN: Habilita o deshabilita el campo para ingresar el percentaje de incapacidad.
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///**********************************************************************************************
    protected void Cmb_Tipo_Incapacidad_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 index = Cmb_Tipo_Incapacidad.SelectedIndex;//Variable que almacena el indice de la unidad responsable seleccionada.
        String Tipo_Icapacidad = String.Empty;//Variable que almacena el tipo de incapacidad seleccionada.

        if (index > 0)
        {
            //Div_Aplica_Pago_Cuarto_Dia_Aplica_Incapacidad.Controls[0].

            Tipo_Icapacidad = Cmb_Tipo_Incapacidad.SelectedItem.Text.Trim();

            switch (Tipo_Icapacidad)
            {
                case "Enfermedad General":
                    Txt_Porcentaje_Incapacidad.Enabled = true;
                    Txt_Porcentaje_Incapacidad.Text = "";
                    break;
                case "Maternidad":
                    Txt_Porcentaje_Incapacidad.Enabled = true;
                    Txt_Porcentaje_Incapacidad.Text = "";
                    break;
                case "Riesgo Laboral":
                    Txt_Porcentaje_Incapacidad.Enabled = false;
                    Txt_Porcentaje_Incapacidad.Text = "100";
                    break;
                default:
                    break;
            }
        }
    }
    #endregion

    #region (TextBox)

    protected void Txt_Fecha_Inicio_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Cmb_Empleados.SelectedIndex > 0)
            {
                if (!string.IsNullOrEmpty(Txt_Dias_Incapacidad.Text.Trim()))
                {

                    if (!string.IsNullOrEmpty(Txt_Fecha_Inicio.Text.Trim().Replace("__/___/____", "")))
                    {

                        if (Validar_Formato_Fecha(Txt_Fecha_Inicio.Text))
                        {
                            DateTime Fecha_Inicio_Incapacidad = Convert.ToDateTime(Txt_Fecha_Inicio.Text.Trim());
                            Int32 Dias_Incapacidad = Convert.ToInt32((string.IsNullOrEmpty(Txt_Dias_Incapacidad.Text.Trim())) ? "0" : Txt_Dias_Incapacidad.Text.Trim());
                            //DateTime Fecha_Fin_Incapacidad = Calcular_Fecha_Termino_Incapacidad(Fecha_Inicio_Incapacidad, Dias_Incapacidad);
                            DateTime Fecha_Fin_Incapacidad = Fecha_Inicio_Incapacidad.AddDays(Dias_Incapacidad - 1);
                            Txt_Fecha_Fin.Text = String.Format("{0:dd/MMM/yyyy}", Fecha_Fin_Incapacidad);
                        }
                        else
                        {
                            Txt_Fecha_Fin.Text = "";
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "El formato de la Fecha Inicial no es correcto";
                        }

                    }
                    else
                    {
                        Txt_Fecha_Fin.Text = "";
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "Seleccione la Fecha de Inicio de sus vacaciones.";
                    }
                }
                else
                {
                    Txt_Fecha_Fin.Text = "";
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Ingrese la cantidad de dias de Incapacidad.";
                }
            }
            else {
                //Txt_Fecha_Fin.Text = "";
                //Lbl_Mensaje_Error.Visible = true;
                //Img_Error.Visible = true;
                //Lbl_Mensaje_Error.Text = "";
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al seleccionar la fecha final de la incapacidad. Error: [" + Ex.Message + "]");
        }
    }

    protected void Txt_Dias_Incapacidad_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Cmb_Empleados.SelectedIndex > 0)
            {
                if (!string.IsNullOrEmpty(Txt_Dias_Incapacidad.Text.Trim()))
                {

                    if (!string.IsNullOrEmpty(Txt_Fecha_Inicio.Text.Trim().Replace("__/___/____", "")))
                    {

                        if (Validar_Formato_Fecha(Txt_Fecha_Inicio.Text))
                        {
                            DateTime Fecha_Inicio_Incapacidad = Convert.ToDateTime(Txt_Fecha_Inicio.Text.Trim());
                            Int32 Dias_Incapacidad = Convert.ToInt32((string.IsNullOrEmpty(Txt_Dias_Incapacidad.Text.Trim())) ? "0" : Txt_Dias_Incapacidad.Text.Trim());
                            //DateTime Fecha_Fin_Incapacidad = Calcular_Fecha_Termino_Incapacidad(Fecha_Inicio_Incapacidad, Dias_Incapacidad);
                            DateTime Fecha_Fin_Incapacidad = Fecha_Inicio_Incapacidad.AddDays(Dias_Incapacidad - 1);
                            Txt_Fecha_Fin.Text = String.Format("{0:dd/MMM/yyyy}", Fecha_Fin_Incapacidad);
                        }
                        else
                        {
                            Txt_Fecha_Fin.Text = "";
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "El formato de la Fecha Inicial no es correcto";
                        }

                    }
                    else
                    {
                        Txt_Fecha_Fin.Text = "";
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "Seleccione la Fecha de Inicio de la Incapacidad.";
                    }
                }
                else
                {
                    Txt_Fecha_Fin.Text = "";
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Ingrese la cantidad de dias de Incapacidad.";
                }
            }
            else
            {
                //Txt_Fecha_Fin.Text = "";
                //Lbl_Mensaje_Error.Visible = true;
                //Img_Error.Visible = true;
                //Lbl_Mensaje_Error.Text = "";
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al seleccionar la fecha final de la incapacidad. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #endregion
}
