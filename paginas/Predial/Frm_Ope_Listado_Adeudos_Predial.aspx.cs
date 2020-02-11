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
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Sessiones;
using Presidencia.Predial_Listado_Adeudos_Predial.Negocio;
using Presidencia.Operacion_Predial_Convenios_Predial.Negocio;
using Presidencia.Operacion_Predial_Dias_Inhabiles.Negocio;
using System.IO;
using Presidencia.Operacion_Predial_Recepcion_Pagos.Negocio;
using Presidencia.Operacion_Predial_Parametros.Negocio;
using Presidencia.Catalogo_Claves_Ingreso.Negocio;
using Presidencia.Operacion_Predial_Convenios_Impuestos_Traslado_Dominio.Negocio;
using Presidencia.Operacion_Predial_Convenios_Fraccionamientos.Negocio;
using Presidencia.Operacion_Predial_Convenios_Derechos_Supervision.Negocio;
using Presidencia.Constantes;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Caja_Pagos.Negocio;

public partial class paginas_Predial_Frm_Ope_Listado_Adeudos_Predial : System.Web.UI.Page
{

    #region  "Page Load"

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Evento Inicial de un Formulario.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 12 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        Lbl_Ecabezado_Mensaje.Text = "";
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
        try
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!Validar_Caja_Abierta_Empleado())
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('NO PUEDE COBRAR SI PRIMERO NO ABRE TURNO EN UNA CAJA.');", true);
                Inhabilitar_Controles();
                Lbl_Ecabezado_Mensaje.Text = "El Formulario se ha inhabilitado por Seguridad.";
                Lbl_Mensaje_Error.Text = "[Es necesario abrir un turno en una caja].";
                Div_Contenedor_Msj_Error.Visible = true;
            }
            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Session["ESTATUS_CUENTAS"] = "IN ('BLOQUEADA','ACTIVA','VIGENTE','PENDIENTE')";
                Session["TIPO_CONTRIBUYENTE"] = "  IN('PROPIETARIO','POSEEDOR')";
                String Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Cuentas_Predial.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                Btn_Busqueda_Avanzada_Cuenta_Predial.Attributes.Add("onclick", Ventana_Modal);
                Visibilidad_Capas("");
                if (Request.QueryString["Referencia"] != null)
                {
                    String Referencia = Request.QueryString["Referencia"].ToString();
                    String No_Pago = Request.QueryString["No_Pago"].ToString();
                    Mostrar_Reporte_Pago_Caja(Referencia, No_Pago);
                }
                
                String Reapertura_Autorizada = Obtener_Dato_Consulta(Ope_Caj_Turnos.Campo_ReApertura_Autorizo, Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos, Ope_Caj_Turnos.Campo_Estatus + " = 'ABIERTO' AND " + Ope_Caj_Turnos.Campo_Empleado_ID + " = '" + Cls_Sessiones.Empleado_ID + "'");
                String Str_Fecha_Apertura_Turno = Obtener_Dato_Consulta(Ope_Caj_Turnos.Campo_Fecha_Turno, Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos, Ope_Caj_Turnos.Campo_Estatus + " = 'ABIERTO' AND " + Ope_Caj_Turnos.Campo_Empleado_ID + " = '" + Cls_Sessiones.Empleado_ID + "'");
                if (Str_Fecha_Apertura_Turno.Trim() != "")
                {
                    if (Convert.ToDateTime(Str_Fecha_Apertura_Turno.Trim()).ToShortDateString() != DateTime.Now.ToShortDateString() && Reapertura_Autorizada == "NO")
                    {
                        Lbl_Mensaje_Error.Text = "El turno actual está abierto desde el " + Convert.ToDateTime(Str_Fecha_Apertura_Turno.Trim()).ToString("dd/MMM/yyyy") + ". Favor de Rectificarlo. Se bloqueó el formulario.";
                        Lbl_Mensaje_Error.Visible = true;
                    }
                }
                if (Lbl_Mensaje_Error.Text.Contains("El turno actual está abierto desde el"))
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Div_Datos_Constribuyente.Visible = false;
                    Div_Datos_Cuenta_Predial.Visible = false;
                    Div_Detalles_Clave_Ingreso.Visible = false;
                    Div_Detalles_Pago_Clave_Ingreso.Visible = false;
                    Div_Listado_Adeudos.Visible = false;
                    Div_Listado_Adeudos_A_Pagar.Visible = false;
                    Txt_Cantidad.Enabled = false;
                    Txt_Clave_Ingreso.Enabled = false;
                    Txt_Cuenta_Predial.Enabled = false;
                    Txt_No_Folio.Enabled = false;
                    Btn_Busqueda_Avanzada_Cuenta_Predial.Enabled = false;
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

    #endregion

    #region  "Metodos"

    #region  "Generales"

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Componentes
    ///DESCRIPCIÓN: Limpia los componentes del Formulario.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 12 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Limpiar_Componentes()
    {
        Hdf_Cuenta_Predial_ID.Value = "";
        Txt_Cuenta_Predial.Text = "";
        Txt_No_Folio.Text = "";
        Txt_Clave_Ingreso.Text = "";
        Txt_Ubicacion_Cuenta.Text = "";
        Txt_Propietario_Cuenta.Text = "";
        Txt_Clave_Ingreso_Fundamento.Text = "";
        Txt_Costo_Unidad.Text = "";
        Txt_Cantidad.Text = "";
        Txt_Total_Pago.Text = "";
        Grid_Listado_Adeudos.DataSource = new DataTable();
        Grid_Listado_Adeudos.DataBind();
        Visibilidad_Capas("");
        Hdf_Clave_Ingreso_ID.Value = "";
        Hdf_Dependencia_ID.Value = "";
        Lbl_Costo_Unidad.Text = "Costo/U [$]";
        Lbl_Cantidad.Text = "Cantidad";
        Txt_Nombre_Contribuyente.Text = "";
        Txt_Observaciones.Text = "";
        Lbl_Estatus.Text = "";
        Lbl_Estatus.Visible = false;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Agrupar_Adeudos
    ///DESCRIPCIÓN: Agrupa los Adeudos [Predial, Fraccionamiento y Derechos
    ///             de Supervisión].
    ///PARAMETROS: 1. Dt_Datos. Datos para llenar el Grid.
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 13 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataTable Agrupar_Adeudos(DataTable Dt_Parametros, String Dato_Agrupar)
    {
        DataTable Dt_Datos = new DataTable();
        Int32 No_Fila = (-1);
        String Referencia = "";
        try
        {
            Dt_Datos.Columns.Add("IDENTIFICADOR", Type.GetType("System.String"));
            Dt_Datos.Columns.Add("TIPO_CONCEPTO", Type.GetType("System.String"));
            Dt_Datos.Columns.Add("REFERENCIA", Type.GetType("System.String"));
            Dt_Datos.Columns.Add("FECHA", Type.GetType("System.DateTime"));
            Dt_Datos.Columns.Add("DESCRIPCION", Type.GetType("System.String"));
            Dt_Datos.Columns.Add("IMPORTE", Type.GetType("System.Double"));

            for (Int32 Contador = 0; Contador < Dt_Parametros.Rows.Count; Contador++)
            {
                if (!Dt_Parametros.Rows[Contador]["TIPO_CONCEPTO"].ToString().Trim().Equals(Dato_Agrupar))
                {
                    Dt_Datos.ImportRow(Dt_Parametros.Rows[Contador]);
                }
                else
                {
                    No_Fila = -1;
                    Referencia = Dt_Parametros.Rows[Contador]["REFERENCIA"].ToString();
                    for (Int32 Cnt_Interno = 0; Cnt_Interno < Dt_Datos.Rows.Count; Cnt_Interno++)
                    {
                        if (Referencia.Trim().Equals(Dt_Datos.Rows[Cnt_Interno]["REFERENCIA"].ToString().Trim()))
                        {
                            No_Fila = Cnt_Interno;
                            break;
                        }
                    }
                    if (No_Fila > (-1))
                    {
                        Double Monto_Agregar = (Dt_Parametros.Rows[Contador]["IMPORTE"] != null) ? Convert.ToDouble(Dt_Parametros.Rows[Contador]["IMPORTE"]) : 0.0;
                        Double Monto_Acumulado = (Dt_Datos.Rows[No_Fila]["IMPORTE"] != null) ? Convert.ToDouble(Dt_Datos.Rows[No_Fila]["IMPORTE"]) : 0.0;
                        Dt_Datos.DefaultView.AllowEdit = true;
                        Dt_Datos.Rows[No_Fila].BeginEdit();
                        Dt_Datos.Rows[No_Fila]["IMPORTE"] = Monto_Acumulado + Monto_Agregar;
                        Dt_Datos.Rows[No_Fila].EndEdit();
                    }
                    else
                    {
                        if ((Dt_Parametros.Rows[Contador]["IMPORTE"].ToString() != ""))
                        {
                            Dt_Datos.ImportRow(Dt_Parametros.Rows[Contador]);
                            Referencia = Dt_Parametros.Rows[Contador]["REFERENCIA"].ToString();
                            No_Fila = Contador;
                        }
                    }
                }
            }
            if (Dato_Agrupar.Trim().Equals("ADEUDO_PREDIAL"))
            {
                if (Hdf_Cuenta_Predial_ID.Value.Trim().Length > 0)
                {
                    if (No_Fila > (-1))
                    {
                        DataTable Dt_Convenio = Buscar_Convenio_Cuenta_Predial();
                        if (Dt_Convenio != null && Dt_Convenio.Rows.Count > 0)
                        {
                            if (Dt_Convenio.Rows.Count > 1)
                            {
                                for (Int32 Contador = 0; Contador < Dt_Convenio.Rows.Count; Contador++)
                                {
                                    DataRow Fila = Dt_Datos.NewRow();
                                    Fila["IDENTIFICADOR"] = Dt_Datos.Rows[No_Fila]["IDENTIFICADOR"];
                                    Fila["TIPO_CONCEPTO"] = Dt_Datos.Rows[No_Fila]["TIPO_CONCEPTO"];
                                    Fila["REFERENCIA"] = Dt_Datos.Rows[No_Fila]["REFERENCIA"];
                                    Fila["FECHA"] = Dt_Convenio.Rows[Contador]["FECHA"];
                                    Fila["DESCRIPCION"] = Dt_Convenio.Rows[Contador]["DESCRIPCION"];
                                    Fila["IMPORTE"] = Dt_Convenio.Rows[Contador]["MONTO"];
                                    Dt_Datos.Rows.Add(Fila);
                                }
                                Dt_Datos.Rows.RemoveAt(No_Fila);
                            }
                            else
                            {
                                Dt_Datos.DefaultView.AllowEdit = true;
                                Dt_Datos.Rows[No_Fila].BeginEdit();
                                Dt_Datos.Rows[No_Fila]["FECHA"] = Dt_Convenio.Rows[0]["FECHA"];
                                Dt_Datos.Rows[No_Fila]["DESCRIPCION"] = Dt_Convenio.Rows[0]["DESCRIPCION"].ToString();
                                Dt_Datos.Rows[No_Fila]["IMPORTE"] = Convert.ToDouble(Dt_Convenio.Rows[0]["MONTO"].ToString());
                                Dt_Datos.Rows[No_Fila].EndEdit();
                            }
                        }
                        else
                        {
                            Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio Negocio = new Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio();
                            Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                            Dt_Datos.DefaultView.AllowEdit = true;
                            Dt_Datos.Rows[No_Fila].BeginEdit();
                            Dt_Datos.Rows[No_Fila]["IMPORTE"] = Negocio.Consultar_Adeudos_Predial_Cuenta();
                            Dt_Datos.Rows[No_Fila].EndEdit();
                        }
                    }
                }
            }
            if (Dato_Agrupar.Trim().Equals("TRASLADO_DOMINIO"))
            {
                if (Hdf_Cuenta_Predial_ID.Value.Trim().Length > 0)
                {
                    if (No_Fila > (-1))
                    {
                        DataTable Dt_Convenio = Buscar_Convenio_Traslado_Dominio(Referencia);
                        if (Dt_Convenio != null && Dt_Convenio.Rows.Count > 0)
                        {
                            if (Dt_Convenio.Rows.Count > 1)
                            {
                                for (Int32 Contador = 0; Contador < Dt_Convenio.Rows.Count; Contador++)
                                {
                                    DataRow Fila = Dt_Datos.NewRow();
                                    Fila["IDENTIFICADOR"] = Dt_Datos.Rows[No_Fila]["IDENTIFICADOR"];
                                    Fila["TIPO_CONCEPTO"] = Dt_Datos.Rows[No_Fila]["TIPO_CONCEPTO"];
                                    Fila["REFERENCIA"] = Dt_Datos.Rows[No_Fila]["REFERENCIA"];
                                    Fila["FECHA"] = Dt_Convenio.Rows[Contador]["FECHA"];
                                    Fila["DESCRIPCION"] = Dt_Convenio.Rows[Contador]["DESCRIPCION"];
                                    Fila["IMPORTE"] = Dt_Convenio.Rows[Contador]["MONTO"];
                                    Dt_Datos.Rows.Add(Fila);
                                }
                                Dt_Datos.Rows.RemoveAt(No_Fila);
                            }
                            else
                            {
                                Dt_Datos.DefaultView.AllowEdit = true;
                                Dt_Datos.Rows[No_Fila].BeginEdit();
                                Dt_Datos.Rows[No_Fila]["FECHA"] = Dt_Convenio.Rows[0]["FECHA"];
                                Dt_Datos.Rows[No_Fila]["DESCRIPCION"] = Dt_Convenio.Rows[0]["DESCRIPCION"].ToString();
                                Dt_Datos.Rows[No_Fila]["IMPORTE"] = Convert.ToDouble(Dt_Convenio.Rows[0]["MONTO"].ToString());
                                Dt_Datos.Rows[No_Fila].EndEdit();
                            }
                        }
                    }
                }
            }
            if (Dato_Agrupar.Trim().Equals("FRACCIONAMIENTO"))
            {
                if (Hdf_Cuenta_Predial_ID.Value.Trim().Length > 0)
                {
                    if (No_Fila > (-1))
                    {
                        DataTable Dt_Convenio = Buscar_Convenio_Fraccionamiento(Referencia);
                        if (Dt_Convenio != null && Dt_Convenio.Rows.Count > 0)
                        {
                            if (Dt_Convenio.Rows.Count > 1)
                            {
                                for (Int32 Contador = 0; Contador < Dt_Convenio.Rows.Count; Contador++)
                                {
                                    DataRow Fila = Dt_Datos.NewRow();
                                    Fila["IDENTIFICADOR"] = Dt_Datos.Rows[No_Fila]["IDENTIFICADOR"];
                                    Fila["TIPO_CONCEPTO"] = Dt_Datos.Rows[No_Fila]["TIPO_CONCEPTO"];
                                    Fila["REFERENCIA"] = Dt_Datos.Rows[No_Fila]["REFERENCIA"];
                                    Fila["FECHA"] = Dt_Convenio.Rows[Contador]["FECHA"];
                                    Fila["DESCRIPCION"] = Dt_Convenio.Rows[Contador]["DESCRIPCION"];
                                    Fila["IMPORTE"] = Dt_Convenio.Rows[Contador]["MONTO"];
                                    Dt_Datos.Rows.Add(Fila);
                                }
                                Dt_Datos.Rows.RemoveAt(No_Fila);
                            }
                            else
                            {
                                Dt_Datos.DefaultView.AllowEdit = true;
                                Dt_Datos.Rows[No_Fila].BeginEdit();
                                Dt_Datos.Rows[No_Fila]["FECHA"] = Dt_Convenio.Rows[0]["FECHA"];
                                Dt_Datos.Rows[No_Fila]["DESCRIPCION"] = Dt_Convenio.Rows[0]["DESCRIPCION"].ToString();
                                Dt_Datos.Rows[No_Fila]["IMPORTE"] = Convert.ToDouble(Dt_Convenio.Rows[0]["MONTO"].ToString());
                                Dt_Datos.Rows[No_Fila].EndEdit();
                            }
                        }
                    }
                }
            }
            if (Dato_Agrupar.Trim().Equals("DERECHO_SUPERVISION"))
            {
                if (Hdf_Cuenta_Predial_ID.Value.Trim().Length > 0)
                {
                    if (No_Fila > (-1))
                    {
                        DataTable Dt_Convenio = Buscar_Convenio_Derechos_Supervision(Referencia);
                        if (Dt_Convenio != null && Dt_Convenio.Rows.Count > 0)
                        {
                            if (Dt_Convenio.Rows.Count > 1)
                            {
                                for (Int32 Contador = 0; Contador < Dt_Convenio.Rows.Count; Contador++)
                                {
                                    DataRow Fila = Dt_Datos.NewRow();
                                    Fila["IDENTIFICADOR"] = Dt_Datos.Rows[No_Fila]["IDENTIFICADOR"];
                                    Fila["TIPO_CONCEPTO"] = Dt_Datos.Rows[No_Fila]["TIPO_CONCEPTO"];
                                    Fila["REFERENCIA"] = Dt_Datos.Rows[No_Fila]["REFERENCIA"];
                                    Fila["FECHA"] = Dt_Convenio.Rows[Contador]["FECHA"];
                                    Fila["DESCRIPCION"] = Dt_Convenio.Rows[Contador]["DESCRIPCION"];
                                    Fila["IMPORTE"] = Dt_Convenio.Rows[Contador]["MONTO"];
                                    Dt_Datos.Rows.Add(Fila);
                                }
                                Dt_Datos.Rows.RemoveAt(No_Fila);
                            }
                            else
                            {
                                Dt_Datos.DefaultView.AllowEdit = true;
                                Dt_Datos.Rows[No_Fila].BeginEdit();
                                Dt_Datos.Rows[No_Fila]["FECHA"] = Dt_Convenio.Rows[0]["FECHA"];
                                Dt_Datos.Rows[No_Fila]["DESCRIPCION"] = Dt_Convenio.Rows[0]["DESCRIPCION"].ToString();
                                Dt_Datos.Rows[No_Fila]["IMPORTE"] = Convert.ToDouble(Dt_Convenio.Rows[0]["MONTO"].ToString());
                                Dt_Datos.Rows[No_Fila].EndEdit();
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = "Verificar.";
            Lbl_Mensaje_Error.Text = "['" + Ex.Message + "']";
            Div_Contenedor_Msj_Error.Visible = false;
        }
        return Dt_Datos;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Adeudos
    ///DESCRIPCIÓN: Llena el Grid de Adeudos.
    ///PARAMETROS: 1. Dt_Datos. Datos para llenar el Grid.
    ///            2. Agrupar. Agrupar similares o no.
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 12 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Llenar_Grid_Adeudos(DataTable Dt_Datos)
    {
        Grid_Listado_Adeudos.Columns[1].Visible = true;
        Grid_Listado_Adeudos.Columns[2].Visible = true;
        Dt_Datos = Agrupar_Adeudos(Dt_Datos, "ADEUDO_PREDIAL");
        Dt_Datos = Agrupar_Adeudos(Dt_Datos, "CONSTANCIA");
        Dt_Datos = Agrupar_Adeudos(Dt_Datos, "TRASLADO_DOMINIO");
        Dt_Datos = Agrupar_Adeudos(Dt_Datos, "DERECHO_SUPERVISION");
        Dt_Datos = Agrupar_Adeudos(Dt_Datos, "FRACCIONAMIENTO");
        Dt_Datos = Agrupar_Adeudos(Dt_Datos, "PASIVO");
        Dt_Datos.DefaultView.Sort = "[DESCRIPCION] ASC";
        Grid_Listado_Adeudos.DataSource = Dt_Datos;
        Grid_Listado_Adeudos.DataBind();
        Grid_Listado_Adeudos.Columns[1].Visible = false;
        Grid_Listado_Adeudos.Columns[2].Visible = false;

        //Valida si hay traslado y predial
        String Ref_Grid = "";
        String Ref_Cuenta = "";
        Boolean Tiene_Traslado = false;
        for (Int32 Contador = 0; Contador < Grid_Listado_Adeudos.Rows.Count; Contador++)
        {
            Ref_Grid = HttpUtility.HtmlDecode(Grid_Listado_Adeudos.Rows[Contador].Cells[3].Text.Trim());
            if (Ref_Grid.StartsWith("TD"))
            {
                Tiene_Traslado = true;
            }
            else
            {
                if (char.IsDigit(Ref_Grid, 1))
                {
                    Ref_Cuenta = Ref_Grid;
                }
            }
        }
        if (Tiene_Traslado && Ref_Cuenta != "")
        {
            Lbl_Ecabezado_Mensaje.Text = "La cuenta consultada adeuda predial y tiene traslado(s) pendiente(s) de pagar, favor de recomendar al contribuyente que debe de pagar PRIMERO el ADEUDO predial.";
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('La cuenta consultada adeuda predial y tiene traslado(s) pendiente(s) de pagar, favor de recomendar al contribuyente que debe de pagar PRIMERO el ADEUDO predial.');", true);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Obtener_Seleccion_Adeudo
    ///DESCRIPCIÓN: Obtiene el Adeudo Seleccionado en el Grid.
    ///PARAMETROS: 1. Dt_Datos. Datos para llenar el Grid.
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 12 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Int32 Obtener_Seleccion_Adeudo()
    {
        Int32 Adeudo_Seleccionado = (-1);
        for (Int32 Contador = 0; Contador < Grid_Listado_Adeudos.Rows.Count; Contador++)
        {
            if (Grid_Listado_Adeudos.Rows[Contador].FindControl("Chk_Seleccion_Adeudo") != null)
            {
                CheckBox Chk_Seleccion_Adeudo_Tmp = (CheckBox)Grid_Listado_Adeudos.Rows[Contador].FindControl("Chk_Seleccion_Adeudo");
                if (Chk_Seleccion_Adeudo_Tmp.Checked)
                {
                    Adeudo_Seleccionado = Convert.ToInt32(Chk_Seleccion_Adeudo_Tmp.TabIndex);
                    break;
                }
            }
        }
        return Adeudo_Seleccionado;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Direccionar_Caja
    ///DESCRIPCIÓN: Direcciona a la caja para ejecutar el pago.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 11 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Direccionar_Pagina(String Pagina, String Referencia)
    {
        try
        {
            Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio Negocio = new Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio();
            if (Pagina.Trim().Equals("Caja"))
            {
                String Ruta_Completa = "../Predial/Frm_Ope_Caj_Pagos.aspx" + "?Referencia=" + Referencia;
                Response.Redirect(Ruta_Completa);
            }
            else if (Pagina.Trim().Equals("Predial"))
            {
                Session["CUENTA_ADEUDO_PREDIAL"] = Referencia;
                String Ruta_Completa = "../Predial/Frm_Ope_Pre_Recepcion_Pagos_Predial.aspx";
                Response.Redirect(Ruta_Completa);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = "A ocurrido una excepción, favor de verificarlo";
            Lbl_Mensaje_Error.Text = "['" + Ex.Message + "']";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Inhabilitar_Controles
    ///DESCRIPCIÓN: Se inhabilitan controles de la Caja.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 14 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Inhabilitar_Controles()
    {
        Btn_Busqueda_Avanzada_Cuenta_Predial.Enabled = false;
        Txt_No_Folio.Enabled = false;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Informacion_Referencia
    ///DESCRIPCIÓN: Se Muestra la información cuando se hace una busqueda por referencia.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 14 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Mostrar_Informacion_Referencia(String Referencia)
    {
        Visibilidad_Capas("NO_FOLIO");
        Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio Negocio = new Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio();
        Negocio.P_Referencia = Referencia;
        DataTable Dt_Referencias = Negocio.Consultar_Adeudos_Referencia();
        if (Dt_Referencias.Rows.Count > 0)
        {
            DataRow Fila = Dt_Referencias.Rows[0];
            if (!String.IsNullOrEmpty(Fila["Cuenta_Predial_ID"].ToString()))
            {
                if (Referencia.StartsWith("TD"))
                {
                    Mostrar_Informacion_Cuenta_Predial(Fila["Cuenta_Predial_ID"].ToString(), false);
                }
                else
                {
                    Mostrar_Informacion_Cuenta_Predial(Fila["Cuenta_Predial_ID"].ToString(), true);
                    Llenar_Grid_Adeudos(Dt_Referencias);
                }
            }
            else
            {
                Txt_Propietario_Cuenta.Text = Fila["Contribuyente"].ToString();
                Visibilidad_Capas("NO_PASIVO");
                Llenar_Grid_Adeudos(Dt_Referencias);
            }
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Informacion_Referencia
    ///DESCRIPCIÓN: Se Muestra la información cuando se hace una busqueda por clave de
    ///             Ingreso.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Mostrar_Informacion_Clave_Ingreso(String Clave_Ingreso)
    {
        Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio Negocio = new Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio();
        Negocio.P_Clave_Ingreso = Clave_Ingreso;
        DataTable Dt_Datos_Clave_Ingreso = Negocio.Consultar_Datos_Clave_Ingreso();
        if (Dt_Datos_Clave_Ingreso != null && Dt_Datos_Clave_Ingreso.Rows.Count > 0)
        {
            Visibilidad_Capas("OTROS_PAGOS");
            Txt_Clave_Ingreso_Descripcion.Text = (!String.IsNullOrEmpty(Dt_Datos_Clave_Ingreso.Rows[0]["DESCRIPCION"].ToString())) ? Dt_Datos_Clave_Ingreso.Rows[0]["DESCRIPCION"].ToString().Trim() : "";
            Txt_Clave_Ingreso_Fundamento.Text = (!String.IsNullOrEmpty(Dt_Datos_Clave_Ingreso.Rows[0]["FUNDAMENTO"].ToString())) ? Dt_Datos_Clave_Ingreso.Rows[0]["FUNDAMENTO"].ToString().Trim() : "";
            Hdf_Dependencia_ID.Value = (!String.IsNullOrEmpty(Dt_Datos_Clave_Ingreso.Rows[0]["DEPENDENCIA_ID"].ToString())) ? Dt_Datos_Clave_Ingreso.Rows[0]["DEPENDENCIA_ID"].ToString().Trim() : "";
            Hdf_Clave_Ingreso_ID.Value = (!String.IsNullOrEmpty(Dt_Datos_Clave_Ingreso.Rows[0]["CLAVE_INGRESO_ID"].ToString())) ? Dt_Datos_Clave_Ingreso.Rows[0]["CLAVE_INGRESO_ID"].ToString().Trim() : "";
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Consulta_Datos_Convenio
    ///DESCRIPCIÓN: Consulta los datos del convenio por la referencia
    ///PARAMETROS: 
    ///CREO: Ismael Prieto Sánchez  
    ///FECHA_CREO: 01/Noviembre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Consulta_Datos_Convenio(String Referencia)
    {
        DataTable Dt_Convenio;
        String No_Convenio = "";
        DataRow Registro;

        string No_Impuesto = "";

        Visibilidad_Capas("NO_FOLIO");
        //Valida para los convenios de predial
        if (Referencia.StartsWith("CPRE"))
        {
            Cls_Ope_Pre_Convenios_Predial_Negocio Rs_Convenio_Predial = new Cls_Ope_Pre_Convenios_Predial_Negocio();
            No_Convenio = String.Format("{0:0000000000}", Convert.ToInt32(Referencia.Substring(4).ToString()));
            Rs_Convenio_Predial.P_Campos_Foraneos = true;
            Rs_Convenio_Predial.P_Campos_Dinamicos = "NO_CONVENIO, CUENTA_PREDIAL_ID, ESTATUS";
            Rs_Convenio_Predial.P_No_Convenio = No_Convenio;
            Dt_Convenio = Rs_Convenio_Predial.Consultar_Convenio_Predial();
            if (Dt_Convenio.Rows.Count > 0)
            {
                //Asigna el registro
                Registro = Dt_Convenio.Rows[0];
                //Valida que sea un convenio activo
                if (Registro["ESTATUS"].ToString() == "ACTIVO")
                {
                    //Asigna la información de la cuenta
                    Mostrar_Informacion_Cuenta_Predial(Registro["CUENTA_PREDIAL"].ToString(), false);
                    //Recorre para seleccionar el tipo convenio predial
                    for (Int32 Contador = 0; Contador < Grid_Listado_Adeudos.Rows.Count; Contador++)
                    {
                        GridViewRow Renglon = Grid_Listado_Adeudos.Rows[Contador];
                        String Concepto = Renglon.Cells[5].Text.Trim();
                        if (Concepto.StartsWith("IMPUESTO DE PREDIAL : No.Convenio"))
                        {
                            Referencia = HttpUtility.HtmlDecode(Renglon.Cells[3].Text.Trim());
                            Direccionar_Pagina("Predial", Referencia);
                            break;
                        }
                    }
                }
                else
                {
                    Lbl_Ecabezado_Mensaje.Text = "No es posible realizar operación sobre el convenio:";
                    Lbl_Mensaje_Error.Text = "El convenio " + Referencia + " se encuentra " + Registro["ESTATUS"].ToString();
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "No es posible realizar operación sobre el convenio:";
                Lbl_Mensaje_Error.Text = "El convenio " + Referencia + " no se encuentra registrado, favor de verificarlo.";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        //Valida para los convenios de traslado, fraccionamientos y derechos de supervision
        if (Referencia.StartsWith("CTRA") || Referencia.StartsWith("CFRA") || Referencia.StartsWith("CDER"))
        {
            DataTable Dt_Consulta = null;
            No_Impuesto = Referencia.Substring(4);
            if (Referencia.StartsWith("CTRA"))
            {
                Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Rs_Convenio_Traslado = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
                Rs_Convenio_Traslado.P_Campos_Foraneos = true;
                Rs_Convenio_Traslado.P_Campos_Dinamicos = "NO_CONVENIO, CUENTA_PREDIAL_ID, NO_CALCULO, ESTATUS, ANIO";
                Rs_Convenio_Traslado.P_No_Convenio = Convert.ToInt64(No_Impuesto).ToString("0000000000");
                Dt_Consulta = Rs_Convenio_Traslado.Consultar_Convenio_Traslado_Dominio();
            }
            if (Referencia.StartsWith("CFRA"))
            {
                Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio Rs_Convenio_Fraccionamiento = new Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio();
                Rs_Convenio_Fraccionamiento.P_Campos_Foraneos = true;
                Rs_Convenio_Fraccionamiento.P_Campos_Dinamicos = "NO_CONVENIO, CUENTA_PREDIAL_ID, NO_IMPUESTO_FRACCIONAMIENTO, ESTATUS, ANIO";
                Rs_Convenio_Fraccionamiento.P_No_Convenio = Convert.ToInt64(No_Impuesto).ToString("0000000000");
                Dt_Consulta = Rs_Convenio_Fraccionamiento.Consultar_Convenio_Fraccionamiento();
            }
            if (Referencia.StartsWith("CDER"))
            {
                Cls_Ope_Pre_Convenios_Derechos_Supervision_Negocio Rs_Convenio_Derechos = new Cls_Ope_Pre_Convenios_Derechos_Supervision_Negocio();
                Rs_Convenio_Derechos.P_Campos_Foraneos = true;
                Rs_Convenio_Derechos.P_Campos_Dinamicos = "NO_CONVENIO, CUENTA_PREDIAL_ID, NO_IMPUESTO_DERECHO_SUPERVISIO, ESTATUS, ANIO";
                Rs_Convenio_Derechos.P_No_Convenio = Convert.ToInt64(No_Impuesto).ToString("0000000000");
                Dt_Consulta = Rs_Convenio_Derechos.Consultar_Convenio_Derecho_Supervisions();
            }
            Dt_Convenio = Dt_Consulta;
            if (Dt_Convenio.Rows.Count > 0)
            {
                //Asigna el registro
                Registro = Dt_Convenio.Rows[0];
                //Forma el numero de impuesto
                if (Referencia.StartsWith("CTRA"))
                {
                    No_Impuesto = "TD" + Convert.ToDouble(Registro["No_Calculo"].ToString()).ToString() + Registro["Anio"].ToString();
                }
                if (Referencia.StartsWith("CFRA"))
                {
                    No_Impuesto = "IMP" + Registro["Anio"].ToString().Substring(2) + Registro["No_Impuesto_Fraccionamiento"].ToString();
                }
                if (Referencia.StartsWith("CDER"))
                {
                    No_Impuesto = "DER" + Registro["Anio"].ToString().Substring(2) + Registro["No_Impuesto_Derecho_Supervisio"].ToString();
                }
                //Valida que sea un convenio activo
                if (Registro["ESTATUS"].ToString() == "ACTIVO")
                {
                    //Asigna la información de la cuenta
                    Mostrar_Informacion_Cuenta_Predial(Registro["CUENTA_PREDIAL"].ToString(), false);
                    //Recorre para seleccionar el tipo convenio predial
                    for (Int32 Contador = 0; Contador < Grid_Listado_Adeudos.Rows.Count; Contador++)
                    {
                        GridViewRow Renglon = Grid_Listado_Adeudos.Rows[Contador];
                        String Concepto = Renglon.Cells[5].Text.Trim();
                        if (Concepto.StartsWith("TRASLADO DE DOMINIO : No.Convenio"))
                        {
                            Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Conv = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
                            Conv.P_No_Convenio = Concepto.Substring(34, 10);
                            Conv.Consultar_Convenio_Traslado_Dominio();
                            foreach (DataRow Dr_Conv in Conv.P_Dt_Parcialidades.Rows)
                            {
                                if (Dr_Conv[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus].ToString() == "POR PAGAR")
                                {
                                    DateTime DaTe = Convert.ToDateTime(Dr_Conv[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Fecha_Vencimiento].ToString()).AddDays(1);
                                    if (DateTime.Compare(DaTe, DateTime.Now) < 1)
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "GACO", "alert('No se puede realizar el pago de este convenio ya que esta vencido.');", true);
                                        return;
                                    }
                                    break;
                                }
                            }
                            Referencia = HttpUtility.HtmlDecode(Renglon.Cells[3].Text.Trim());
                            if (Referencia == No_Impuesto)
                            {
                                Alta_Pasivo_Convenios(Referencia, Concepto);
                                Direccionar_Pagina("Caja", Referencia);
                                break;
                            }
                        }
                        else if (Concepto.StartsWith("IMPUESTO DE FRACCIONAMIENTO : No.Convenio"))
                        {
                            Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio Conv = new Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio();
                            Conv.P_No_Convenio = Concepto.Substring(42, 10);
                            Conv.Consultar_Convenio_Fraccionamiento();
                            foreach (DataRow Dr_Conv in Conv.P_Dt_Parcialidades.Rows)
                            {
                                if (Dr_Conv[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus].ToString() == "POR PAGAR")
                                {
                                    DateTime DaTe = Convert.ToDateTime(Dr_Conv[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Fecha_Vencimiento].ToString()).AddDays(1);
                                    if (DateTime.Compare(DaTe, DateTime.Now) < 1)
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "GACO", "alert('No se puede realizar el pago de este convenio ya que esta vencido.');", true);
                                        return;
                                    }
                                    break;
                                }
                            }
                            Referencia = HttpUtility.HtmlDecode(Renglon.Cells[3].Text.Trim());
                            if (Referencia == No_Impuesto)
                            {
                                Alta_Pasivo_Convenios(Referencia, Concepto);
                                Direccionar_Pagina("Caja", Referencia);
                                break;
                            }
                        }
                        else if (Concepto.StartsWith("DERECHOS DE SUPERVISION : No.Convenio"))
                        {
                            Cls_Ope_Pre_Convenios_Derechos_Supervision_Negocio Conv = new Cls_Ope_Pre_Convenios_Derechos_Supervision_Negocio();
                            Conv.P_No_Convenio = Concepto.Substring(38, 10);
                            Conv.Consultar_Convenio_Derecho_Supervisions();
                            foreach (DataRow Dr_Conv in Conv.P_Dt_Parcialidades.Rows)
                            {
                                if (Dr_Conv[Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Estatus].ToString() == "POR PAGAR")
                                {
                                    DateTime DaTe = Convert.ToDateTime(Dr_Conv[Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Fecha_Vencimiento].ToString()).AddDays(1);
                                    if (DateTime.Compare(DaTe, DateTime.Now) < 1)
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "GACO", "alert('No se puede realizar el pago de este convenio ya que esta vencido.');", true);
                                        return;
                                    }
                                    break;
                                }
                            }
                            Referencia = HttpUtility.HtmlDecode(Renglon.Cells[3].Text.Trim());
                            if (Referencia == No_Impuesto)
                            {
                                Alta_Pasivo_Convenios(Referencia, Concepto);
                                Direccionar_Pagina("Caja", Referencia);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    Lbl_Ecabezado_Mensaje.Text = "No es posible realizar operación sobre el convenio:";
                    Lbl_Mensaje_Error.Text = "El convenio " + Referencia + " se encuentra " + Registro["ESTATUS"].ToString();
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "No es posible realizar operación sobre el convenio:";
                Lbl_Mensaje_Error.Text = "El convenio " + Referencia + " no se encuentra registrado, favor de verificarlo.";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Reporte_Pago_Caja
    ///DESCRIPCIÓN: Muestra un reporte de Pago.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 16 Octubre 2011 [Domingo ¬¬]
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Mostrar_Reporte_Pago_Caja(String Referencia, String No_Pago)
    {
        //Envia a impresion el recibo
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pago_Caja", "window.open('Frm_Ope_Pre_Impresion_Recibo.aspx?Referencia=" + Referencia + "&No_Pago=" + No_Pago + "','Imprimir_Recibo');", true);
        //Si viene de traslado consulta el adeudo
        if (Referencia.StartsWith("TD"))
        {
            //Obtiene la cuenta predial id
            String Campos = Ope_Caj_Pagos.Campo_Cuenta_Predial_ID;
            String Filtros = Ope_Caj_Pagos.Campo_Documento + " = '" + Referencia + "'";
            Filtros += " AND " + Ope_Caj_Pagos.Campo_No_Pago + " = '" + String.Format("{0:0000000000}", Convert.ToDouble(No_Pago)) + "'";
            String Cuenta_Predial_ID = Obtener_Dato_Consulta(Campos, Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos, Filtros);
            if (!String.IsNullOrEmpty(Cuenta_Predial_ID)) //Si encontro la cuenta predial del pago
            {
                //Asigna los campos para obtener el adeudo
                Campos = "NVL(SUM((NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_1 + ",0) - NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_1 + ",0))";
                Campos = Campos + " + (NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_2 + ",0) - NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_2 + ",0))";
                Campos = Campos + " + (NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_3 + ",0) - NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_3 + ",0))";
                Campos = Campos + " + (NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_4 + ",0) - NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_4 + ",0))";
                Campos = Campos + " + (NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_5 + ",0) - NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_5 + ",0))";
                Campos = Campos + " + (NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_6 + ",0) - NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_6 + ",0))),'0') AS IMPORTE";
                //Asigna los filtros del adeudo
                Filtros = Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + " = '" + Cuenta_Predial_ID + "'";
                //Obtiene el monto del adeudo de predial
                Double Importe_Adeudo = Convert.ToDouble(Obtener_Dato_Consulta(Campos, Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial, Filtros));
                if (Importe_Adeudo > 0)
                {
                    //Consulta la cuenta predial
                    Campos = Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial;
                    Filtros = Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = '" + Cuenta_Predial_ID + "'";
                    Txt_Cuenta_Predial.Text = Obtener_Dato_Consulta(Campos, Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas, Filtros);
                    String Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim().ToUpper();
                    Limpiar_Componentes();
                    if (Cuenta_Predial.Trim().Length > 0)
                    {
                        Txt_Cuenta_Predial.Text = Cuenta_Predial;
                        Mostrar_Informacion_Cuenta_Predial(Cuenta_Predial, false);
                        Cuenta_Suspendida();
                    }
                    //Manda mensaje de aviso
                    Lbl_Ecabezado_Mensaje.Text = "La cuenta del traslado que acaba de pagar genero adeudo de predial, favor de recomendar al contribuyente que debe de pagar su ADEUDO predial.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('La cuenta del traslado que acaba de pagar genero adeudo de predial, favor de recomendar al contribuyente que debe de pagar su ADEUDO predial.');", true);
                }
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Abrir_Ventana
    ///DESCRIPCIÓN: Abre en otra ventana el archivo pdf
    ///PARÁMETROS : Nombre_Archivo: Guarda el nombre del archivo que se desea abrir
    ///                             para mostrar los datos al usuario
    ///CREO       : Yazmin A Delgado Gómez
    ///FECHA_CREO : 25-Agosto-2011
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************
    private void Abrir_Ventana(String Nombre_Archivo)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";
        try
        {
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Nombre_Archivo + "', 'popup')", true);
            Pagina = Pagina + Nombre_Archivo;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt",
            "window.open('" + Pagina + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Abrir_Ventana " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Calcular_Pago_Otros_Ingresos
    ///DESCRIPCIÓN: Calcula el Total de un pago
    ///PARÁMETROS : 
    ///CREO       : Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO : 25-Octubre-2011
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************
    private void Calcular_Pago_Otros_Ingresos()
    {
        Double Costo = 0.0;
        Int32 Cantidad = 0;
        if (Lbl_Costo_Unidad.Text == "Costo/U [$]")
        {
            if (Txt_Cantidad.Text.Trim().Length > 0)
            {
                Cantidad = Convert.ToInt32(Txt_Cantidad.Text.Trim());
            }
            if (Txt_Costo_Unidad.Text.Trim().Length > 0)
            {
                Costo = Convert.ToDouble(Txt_Costo_Unidad.Text.Trim().Replace("$", ""));
            }
            Txt_Total_Pago.Text = String.Format("{0:c}", (Cantidad * Costo));
        }
        else if (Lbl_Costo_Unidad.Text == "Importe")
        {
            if (Txt_Cantidad.Text == "" || Txt_Cantidad.Text == "0")
            {
                Txt_Cantidad.Text = "1";
            }
            else
            {
                //if (Txt_Costo_Unidad.Text == "")
                //{
                //    Txt_Costo_Unidad.Text = Convert.ToDouble(Txt_Costo_Unidad.Text).ToString("###,###,##0.00");
                //}
            }
            if (Txt_Costo_Unidad.Text == "")
            {
                Txt_Total_Pago.Text = "0.00";
            }
            else
            {
                Txt_Total_Pago.Text = Convert.ToDouble(Txt_Costo_Unidad.Text).ToString("###,###,##0.00");
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Visibilidad_Capas
    ///DESCRIPCIÓN: Hace visibles o invisibles las capas.
    ///PARÁMETROS : 
    ///CREO       : Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO : 25-Octubre-2011
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************
    private void Visibilidad_Capas(String Tipo)
    {
        Lbl_Propietario_Cuenta.Visible = true;
        Lbl_Ubicacion_Cuenta.Visible = true;
        Txt_Ubicacion_Cuenta.Visible = true;
        Txt_Propietario_Cuenta.Visible = true;
        if (Tipo.Trim().Equals("CUENTA_PREDIAL"))
        {
            Div_Listado_Adeudos_A_Pagar.Visible = true;
            Div_Datos_Cuenta_Predial.Visible = true;
            Div_Detalles_Pago_Clave_Ingreso.Visible = false;
            Div_Detalles_Clave_Ingreso.Visible = false;
            Div_Datos_Constribuyente.Visible = false;
        }
        else if (Tipo.Trim().Equals("NO_FOLIO"))
        {
            Div_Listado_Adeudos_A_Pagar.Visible = true;
            Div_Datos_Cuenta_Predial.Visible = false;
            Div_Detalles_Pago_Clave_Ingreso.Visible = false;
            Div_Detalles_Clave_Ingreso.Visible = false;
            Div_Datos_Constribuyente.Visible = false;
        }
        else if (Tipo.Trim().Equals("OTROS_PAGOS"))
        {
            Div_Listado_Adeudos_A_Pagar.Visible = false;
            Div_Datos_Cuenta_Predial.Visible = false;
            Div_Detalles_Pago_Clave_Ingreso.Visible = true;
            Div_Detalles_Clave_Ingreso.Visible = true;
            Div_Datos_Constribuyente.Visible = true;
        }
        else if (Tipo.Trim().Equals("NO_PASIVO"))
        {
            Div_Listado_Adeudos_A_Pagar.Visible = true;
            Div_Datos_Cuenta_Predial.Visible = true;
            Div_Detalles_Pago_Clave_Ingreso.Visible = false;
            Div_Detalles_Clave_Ingreso.Visible = false;
            Div_Datos_Constribuyente.Visible = false;
            Lbl_Propietario_Cuenta.Visible = true;
            Lbl_Ubicacion_Cuenta.Visible = false;
            Txt_Ubicacion_Cuenta.Visible = false;
            Txt_Propietario_Cuenta.Visible = true;
        }
        else
        {
            Div_Listado_Adeudos_A_Pagar.Visible = false;
            Div_Datos_Cuenta_Predial.Visible = false;
            Div_Detalles_Pago_Clave_Ingreso.Visible = false;
            Div_Detalles_Clave_Ingreso.Visible = false;
            Div_Datos_Constribuyente.Visible = false;
        }
    }

    #endregion

    #region  "Interacción con Clases de Negocio y Datos"

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Informacion_Cuenta_Predial
    ///DESCRIPCIÓN: Muestra la información de la Cuenta Predial.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 12 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Mostrar_Informacion_Cuenta_Predial(String Cuenta_Predial, Boolean Solo_Datos_Cuenta)
    {
        Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio Negocio = new Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio();
        String Ventana_Modal;
        String Propiedades;

        Negocio.P_Referencia = Txt_No_Folio.Text.Trim();
        if (Solo_Datos_Cuenta)
        {
            Negocio.P_Cuenta_Predial_ID = Cuenta_Predial;
        }
        else
        {
            Negocio.P_Cuenta_Predial = Cuenta_Predial;
        }
        DataTable Dt_Datos = Negocio.Consultar_Datos_Cuentas_Predial();
        if (Dt_Datos != null && Dt_Datos.Rows.Count > 0)
        {
            Visibilidad_Capas("CUENTA_PREDIAL");
            if (!Solo_Datos_Cuenta)
            {
                Hdf_Cuenta_Predial_ID.Value = (!string.IsNullOrEmpty(Dt_Datos.Rows[0]["CUENTA_PREDIAL_ID"].ToString().Trim())) ? Dt_Datos.Rows[0]["CUENTA_PREDIAL_ID"].ToString().Trim() : "";
                Txt_Cuenta_Predial.Text = (!string.IsNullOrEmpty(Dt_Datos.Rows[0]["CUENTA_PREDIAL"].ToString().Trim())) ? Dt_Datos.Rows[0]["CUENTA_PREDIAL"].ToString().Trim() : "";
            }
            Lbl_Estatus.Visible = false;
            Lbl_Estatus.Text = Dt_Datos.Rows[0]["ESTATUS"].ToString();
            Hdf_Tipo_Suspension.Value = Dt_Datos.Rows[0]["TIPO_SUSPENCION"].ToString();
            if (Dt_Datos.Rows[0]["ESTATUS"].ToString() == "SUSPENDIDA" && (Dt_Datos.Rows[0]["TIPO_SUSPENCION"].ToString() == "PREDIAL" || Dt_Datos.Rows[0]["TIPO_SUSPENCION"].ToString() == "AMBAS"))
            {
                Lbl_Estatus.Visible = true;
            }
            Txt_Propietario_Cuenta.Text = (!string.IsNullOrEmpty(Dt_Datos.Rows[0]["PROPIETARIO"].ToString().Trim())) ? Dt_Datos.Rows[0]["PROPIETARIO"].ToString().Trim() : "---------------------------";
            String Ubicacion = (!string.IsNullOrEmpty(Dt_Datos.Rows[0]["UBICACION"].ToString().Trim())) ? Dt_Datos.Rows[0]["UBICACION"].ToString().Trim() : "";
            Txt_Ubicacion_Cuenta.Text = (!Ubicacion.Trim().Equals("S/N COL.")) ? Ubicacion : "---------------------------";
            Ventana_Modal = "Abrir_Resumen('Ventanas_Emergentes/Frm_Resumen_Caja.aspx";
            Propiedades = ", 'resizable=yes,status=no,width=750,scrollbars=yes');";
            Btn_Resumen_Cuenta.Attributes.Add("onclick", Ventana_Modal + "?Cuenta_Predial=" + Txt_Cuenta_Predial.Text + "'" + Propiedades);
            Ventana_Modal = "Abrir_Ventana_Estado_Cuenta('Ventanas_Emergentes/Resumen_Predial/Frm_Estado_Cuenta.aspx";
            Propiedades = ", 'height=600,width=800,scrollbars=1');";
            Btn_Estado_Cuenta.Attributes.Add("onclick", Ventana_Modal + "?Cuenta_Predial=" + Txt_Cuenta_Predial.Text + "'" + Propiedades);

            if (!Solo_Datos_Cuenta)
            {
                Negocio.P_Cuenta_Predial = (!string.IsNullOrEmpty(Dt_Datos.Rows[0]["CUENTA_PREDIAL"].ToString().Trim())) ? Dt_Datos.Rows[0]["CUENTA_PREDIAL"].ToString().Trim() : "";
                Negocio.P_Cuenta_Predial_ID = (!string.IsNullOrEmpty(Dt_Datos.Rows[0]["CUENTA_PREDIAL_ID"].ToString().Trim())) ? Dt_Datos.Rows[0]["CUENTA_PREDIAL_ID"].ToString().Trim() : "";
                Dt_Datos = Negocio.Consultar_Adeudos_Totales();
                Llenar_Grid_Adeudos(Dt_Datos);
            }
        }
        else
        {
            Lbl_Ecabezado_Mensaje.Text = "No se ha encontrado la Cuenta predial";
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = false;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Validar_Caja_Abierta_Empleado
    ///DESCRIPCIÓN: Valida que el empleado tenga una Caja Abierta.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 14 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Boolean Validar_Caja_Abierta_Empleado()
    {
        Boolean Caja_Abierta = false;
        Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio Negocio = new Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio();
        Negocio.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
        Caja_Abierta = Negocio.Consultar_Apertura_Turno_Existente();
        return Caja_Abierta;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Buscar_Convenio_Cuenta_Predial
    ///DESCRIPCIÓN: Busca un convenio Activo para la cuenta predial.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 22 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataTable Buscar_Convenio_Cuenta_Predial()
    {
        DataTable Dt_Convenio = new DataTable();
        Dt_Convenio.Columns.Add("NO_CONVENIO", Type.GetType("System.String"));
        Dt_Convenio.Columns.Add("DESCRIPCION", Type.GetType("System.String"));
        Dt_Convenio.Columns.Add("MONTO", Type.GetType("System.Double"));
        Dt_Convenio.Columns.Add("FECHA", Type.GetType("System.DateTime"));
        if (Hdf_Cuenta_Predial_ID.Value.Trim().Length > 0)
        {
            Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio Negocio = new Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio();
            Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            DataTable Dt_Temporal = Negocio.Consultar_Convenio_Cuenta_Predia();
            if (Dt_Temporal != null && Dt_Temporal.Rows.Count > 0)
            {
                if (!Validar_Convenio_No_Imcumplido(Dt_Temporal))
                {
                    DataRow Fila = Dt_Convenio.NewRow();
                    Fila["NO_CONVENIO"] = Dt_Temporal.Rows[0]["NO_CONVENIO"].ToString().Trim();
                    Fila["DESCRIPCION"] = "IMPUESTO DE PREDIAL : No.Convenio " + Dt_Temporal.Rows[0]["NO_CONVENIO"].ToString().Trim() + ", Parcialidad: " + Dt_Temporal.Rows[0]["PARCIALIDAD"].ToString().Trim() + '/' + Dt_Temporal.Rows[0]["NUMERO_PARCIALIDADES"].ToString().Trim();
                    Fila["MONTO"] = Convert.ToDouble(Dt_Temporal.Rows[0]["IMPORTE"].ToString().Trim());
                    Fila["FECHA"] = Convert.ToDateTime(Dt_Temporal.Rows[0]["FECHA_VENCIMIENTO"].ToString().Trim());
                    Dt_Convenio.Rows.Add(Fila);
                }
                else
                {
                    if (Dt_Temporal != null && Dt_Temporal.Rows.Count > 0)
                    {
                        for (Int32 Contador = 0; Contador < 1; Contador++)
                        {
                            DataRow Fila = Dt_Convenio.NewRow();
                            Fila["NO_CONVENIO"] = Dt_Temporal.Rows[Contador]["NO_CONVENIO"].ToString().Trim();
                            Fila["DESCRIPCION"] = "IMPUESTO DE PREDIAL : No.Convenio " + Dt_Temporal.Rows[Contador]["NO_CONVENIO"].ToString().Trim() + ", Parcialidad: " + Dt_Temporal.Rows[Contador]["PARCIALIDAD"].ToString().Trim() + '/' + Dt_Temporal.Rows[0]["NUMERO_PARCIALIDADES"].ToString().Trim() +
                                                  "... INCUMPLIDO";
                            Fila["MONTO"] = Convert.ToDouble(Dt_Temporal.Rows[Contador]["IMPORTE"].ToString().Trim());
                            Fila["FECHA"] = Convert.ToDateTime(Dt_Temporal.Rows[Contador]["FECHA_VENCIMIENTO"].ToString().Trim());
                            Dt_Convenio.Rows.Add(Fila);
                        }
                    }
                }
            }
        }
        return Dt_Convenio;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Buscar_Convenio_Traslado_Dominio
    ///DESCRIPCIÓN: Busca un convenio Activo para la cuenta predial.
    ///PARAMETROS: Referencia, pasa el dato dela referencia del traslado
    ///CREO: Ismael Prieto Sánchez
    ///FECHA_CREO: 1/Noviembre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataTable Buscar_Convenio_Traslado_Dominio(String Referencia)
    {
        string No_Calculo = "";
        Int32 Anio_Calculo = 0;

        No_Calculo = Referencia.Substring(2);
        Anio_Calculo = 0;
        //if (No_Calculo.Length > 4)
        //{
        Anio_Calculo = Convert.ToInt16(No_Calculo.Substring(No_Calculo.Length - 4));
        No_Calculo = No_Calculo.Substring(0, No_Calculo.Length - 4);
        //}

        DataTable Dt_Convenio = new DataTable();
        Dt_Convenio.Columns.Add("NO_CONVENIO", Type.GetType("System.String"));
        Dt_Convenio.Columns.Add("DESCRIPCION", Type.GetType("System.String"));
        Dt_Convenio.Columns.Add("MONTO", Type.GetType("System.Double"));
        Dt_Convenio.Columns.Add("FECHA", Type.GetType("System.DateTime"));
        if (Hdf_Cuenta_Predial_ID.Value.Trim().Length > 0)
        {
            Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio Negocio = new Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio();
            Negocio.P_No_Traslado = Convert.ToInt64(No_Calculo).ToString("0000000000");
            Negocio.P_Anio_Traslado = Anio_Calculo;
            Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            DataTable Dt_Temporal = Negocio.Consultar_Convenio_Traslado_Dominio();
            if (Dt_Temporal != null && Dt_Temporal.Rows.Count > 0)
            {
                if (!Validar_Convenio_No_Imcumplido(Dt_Temporal))
                {
                    DataRow Fila = Dt_Convenio.NewRow();
                    Fila["NO_CONVENIO"] = Dt_Temporal.Rows[0]["NO_CONVENIO"].ToString().Trim();
                    Fila["DESCRIPCION"] = "TRASLADO DE DOMINIO : No.Convenio " + Dt_Temporal.Rows[0]["NO_CONVENIO"].ToString().Trim() + ", Parcialidad: " + Dt_Temporal.Rows[0]["PARCIALIDAD"].ToString().Trim();
                    Fila["MONTO"] = Convert.ToDouble(Dt_Temporal.Rows[0]["IMPORTE"].ToString().Trim());
                    Fila["FECHA"] = Convert.ToDateTime(Dt_Temporal.Rows[0]["FECHA_VENCIMIENTO"].ToString().Trim());
                    Dt_Convenio.Rows.Add(Fila);
                }
                else
                {
                    if (Dt_Temporal != null && Dt_Temporal.Rows.Count > 0)
                    {
                        for (Int32 Contador = 0; Contador < 1; Contador++)
                        {
                            DataRow Fila = Dt_Convenio.NewRow();
                            Fila["NO_CONVENIO"] = Dt_Temporal.Rows[Contador]["NO_CONVENIO"].ToString().Trim();
                            Fila["DESCRIPCION"] = "TRASLADO DE DOMINIO : No.Convenio " + Dt_Temporal.Rows[Contador]["NO_CONVENIO"].ToString().Trim() + ", Parcialidad: " + Dt_Temporal.Rows[Contador]["PARCIALIDAD"].ToString().Trim() +
                                                  "... INCUMPLIDO";
                            Fila["MONTO"] = Convert.ToDouble(Dt_Temporal.Rows[Contador]["IMPORTE"].ToString().Trim());
                            Fila["FECHA"] = Convert.ToDateTime(Dt_Temporal.Rows[Contador]["FECHA_VENCIMIENTO"].ToString().Trim());
                            Dt_Convenio.Rows.Add(Fila);
                        }
                    }
                }
            }
        }
        return Dt_Convenio;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Buscar_Convenio_Fraccionamiento
    ///DESCRIPCIÓN: Busca un convenio Activo para la cuenta predial.
    ///PARAMETROS: Referencia, pasa el dato dela referencia del fraccionamiento
    ///CREO: Ismael Prieto Sánchez
    ///FECHA_CREO: 13/Noviembre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataTable Buscar_Convenio_Fraccionamiento(String Referencia)
    {
        string No_Impuesto = "";
        Int32 Anio_Calculo = 0;

        No_Impuesto = Referencia.Substring(3);
        Anio_Calculo = 0;
        //if (No_Impuesto.Length > 4)
        //{
        Anio_Calculo = 2000 + Convert.ToInt16(No_Impuesto.Substring(0, 2));
        No_Impuesto = No_Impuesto.Substring(2);
        //}

        DataTable Dt_Convenio = new DataTable();
        Dt_Convenio.Columns.Add("NO_CONVENIO", Type.GetType("System.String"));
        Dt_Convenio.Columns.Add("DESCRIPCION", Type.GetType("System.String"));
        Dt_Convenio.Columns.Add("MONTO", Type.GetType("System.Double"));
        Dt_Convenio.Columns.Add("FECHA", Type.GetType("System.DateTime"));
        if (Hdf_Cuenta_Predial_ID.Value.Trim().Length > 0)
        {
            Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio Negocio = new Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio();
            Negocio.P_No_Impuesto_Fraccionamiento = Convert.ToInt64(No_Impuesto).ToString("0000000000");
            Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            DataTable Dt_Temporal = Negocio.Consultar_Convenio_Fraccionamiento();
            if (Dt_Temporal != null && Dt_Temporal.Rows.Count > 0)
            {
                if (!Validar_Convenio_No_Imcumplido(Dt_Temporal))
                {
                    DataRow Fila = Dt_Convenio.NewRow();
                    Fila["NO_CONVENIO"] = Dt_Temporal.Rows[0]["NO_CONVENIO"].ToString().Trim();
                    Fila["DESCRIPCION"] = "IMPUESTO DE FRACCIONAMIENTO : No.Convenio " + Dt_Temporal.Rows[0]["NO_CONVENIO"].ToString().Trim() + ", Parcialidad: " + Dt_Temporal.Rows[0]["PARCIALIDAD"].ToString().Trim() + "/" + Dt_Temporal.Rows[0]["NUMERO_PARCIALIDADES"].ToString().Trim();
                    Fila["MONTO"] = Convert.ToDouble(Dt_Temporal.Rows[0]["IMPORTE"].ToString().Trim());
                    Fila["FECHA"] = Convert.ToDateTime(Dt_Temporal.Rows[0]["FECHA_VENCIMIENTO"].ToString().Trim());
                    Dt_Convenio.Rows.Add(Fila);
                }
                else
                {
                    if (Dt_Temporal != null && Dt_Temporal.Rows.Count > 0)
                    {
                        for (Int32 Contador = 0; Contador < 1; Contador++)
                        {
                            DataRow Fila = Dt_Convenio.NewRow();
                            Fila["NO_CONVENIO"] = Dt_Temporal.Rows[Contador]["NO_CONVENIO"].ToString().Trim();
                            Fila["DESCRIPCION"] = "IMPUESTO DE FRACCIONAMIENTO : No.Convenio " + Dt_Temporal.Rows[Contador]["NO_CONVENIO"].ToString().Trim() + ", Parcialidad: " + Dt_Temporal.Rows[Contador]["PARCIALIDAD"].ToString().Trim() + "/" + Dt_Temporal.Rows[0]["NUMERO_PARCIALIDADES"].ToString().Trim() +
                                                  "... INCUMPLIDO";
                            Fila["MONTO"] = Convert.ToDouble(Dt_Temporal.Rows[Contador]["IMPORTE"].ToString().Trim());
                            Fila["FECHA"] = Convert.ToDateTime(Dt_Temporal.Rows[Contador]["FECHA_VENCIMIENTO"].ToString().Trim());
                            Dt_Convenio.Rows.Add(Fila);
                        }
                    }
                }
            }
        }
        return Dt_Convenio;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Buscar_Convenio_Derechos_Supervision
    ///DESCRIPCIÓN: Busca un convenio Activo para la cuenta predial.
    ///PARAMETROS: Referencia, pasa el dato dela referencia de derechos de supervision
    ///CREO: Ismael Prieto Sánchez
    ///FECHA_CREO: 13/Noviembre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataTable Buscar_Convenio_Derechos_Supervision(String Referencia)
    {
        string No_Impuesto = "";
        Int32 Anio_Calculo = 0;

        No_Impuesto = Referencia.Substring(3);
        Anio_Calculo = 0;
        //if (No_Impuesto.Length > 4)
        //{
        Anio_Calculo = 2000 + Convert.ToInt16(No_Impuesto.Substring(0, 2));
        No_Impuesto = No_Impuesto.Substring(2);
        //}

        DataTable Dt_Convenio = new DataTable();
        Dt_Convenio.Columns.Add("NO_CONVENIO", Type.GetType("System.String"));
        Dt_Convenio.Columns.Add("DESCRIPCION", Type.GetType("System.String"));
        Dt_Convenio.Columns.Add("MONTO", Type.GetType("System.Double"));
        Dt_Convenio.Columns.Add("FECHA", Type.GetType("System.DateTime"));
        if (Hdf_Cuenta_Predial_ID.Value.Trim().Length > 0)
        {
            Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio Negocio = new Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio();
            Negocio.P_No_Impuesto_Derecho_Supervision = Convert.ToInt64(No_Impuesto).ToString("0000000000");
            Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            DataTable Dt_Temporal = Negocio.Consultar_Convenio_Derechos_Supervision();
            if (Dt_Temporal != null && Dt_Temporal.Rows.Count > 0)
            {
                if (!Validar_Convenio_No_Imcumplido(Dt_Temporal))
                {
                    DataRow Fila = Dt_Convenio.NewRow();
                    Fila["NO_CONVENIO"] = Dt_Temporal.Rows[0]["NO_CONVENIO"].ToString().Trim();
                    Fila["DESCRIPCION"] = "DERECHOS DE SUPERVISION : No.Convenio " + Dt_Temporal.Rows[0]["NO_CONVENIO"].ToString().Trim() + ", Parcialidad: " + Dt_Temporal.Rows[0]["PARCIALIDAD"].ToString().Trim() + "/" + Dt_Temporal.Rows[0]["NUMERO_PARCIALIDADES"].ToString().Trim();
                    Fila["MONTO"] = Convert.ToDouble(Dt_Temporal.Rows[0]["IMPORTE"].ToString().Trim());
                    Fila["FECHA"] = Convert.ToDateTime(Dt_Temporal.Rows[0]["FECHA_VENCIMIENTO"].ToString().Trim());
                    Dt_Convenio.Rows.Add(Fila);
                }
                else
                {
                    if (Dt_Temporal != null && Dt_Temporal.Rows.Count > 0)
                    {
                        for (Int32 Contador = 0; Contador < 1; Contador++)
                        {
                            DataRow Fila = Dt_Convenio.NewRow();
                            Fila["NO_CONVENIO"] = Dt_Temporal.Rows[Contador]["NO_CONVENIO"].ToString().Trim();
                            Fila["DESCRIPCION"] = "DERECHOS DE SUPERVISION : No.Convenio " + Dt_Temporal.Rows[Contador]["NO_CONVENIO"].ToString().Trim() + ", Parcialidad: " + Dt_Temporal.Rows[Contador]["PARCIALIDAD"].ToString().Trim() + "/" + Dt_Temporal.Rows[0]["NUMERO_PARCIALIDADES"].ToString().Trim() +
                                                  "... INCUMPLIDO";
                            Fila["MONTO"] = Convert.ToDouble(Dt_Temporal.Rows[Contador]["IMPORTE"].ToString().Trim());
                            Fila["FECHA"] = Convert.ToDateTime(Dt_Temporal.Rows[Contador]["FECHA_VENCIMIENTO"].ToString().Trim());
                            Dt_Convenio.Rows.Add(Fila);
                        }
                    }
                }
            }
        }
        return Dt_Convenio;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Validar_Convenio_No_Imcumplido
    ///DESCRIPCIÓN: Valdia el Incumplimiento de un Convenio de Predial.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 22 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Boolean Validar_Convenio_No_Imcumplido(DataTable Dt_Parcialidades)
    {
        Cls_Ope_Caj_Pagos_Negocio Rs_Consulta_Datos_Turno = new Cls_Ope_Caj_Pagos_Negocio(); //Variable de conexión hacia la capa de Negocios
        Cls_Ope_Pre_Dias_Inhabiles_Negocio Dias_Inhabiles = new Cls_Ope_Pre_Dias_Inhabiles_Negocio(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Caja; //Variable que obtendra los datos de la consulta  de la caja del empleado
        DataTable Dt_Turno; //Variable que obtendra los datos de la consulta de la fecha de aplicacion
        Boolean Convenio_Incumplido = false; //Almacena para saber si se incumplio o no la parcialidad
        DateTime Fecha_Actual = DateTime.Today; //Almacena la fecha de aplicacion o actual
        String Caja_ID = "";    //Almacena el id de la caja del empleado

        //Obtiene la consulta de la caja del empleado
        Rs_Consulta_Datos_Turno.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
        Dt_Caja = Rs_Consulta_Datos_Turno.Consulta_Caja_Empleado();
        if (Dt_Caja.Rows.Count > 0)
        {
            foreach (DataRow Registro in Dt_Caja.Rows)
            {
                Caja_ID = Registro["CAJA_ID"].ToString();
            }
        }

        //Obtiene la fecha de aplicacion
        Rs_Consulta_Datos_Turno.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
        Rs_Consulta_Datos_Turno.P_Caja_ID = Caja_ID;
        Dt_Turno = Rs_Consulta_Datos_Turno.Consulta_Datos_Turno();
        if (Dt_Turno.Rows.Count > 0)
        {
            foreach (DataRow Registro in Dt_Turno.Rows)
            {
                Fecha_Actual = Convert.ToDateTime(Registro["APLICACION_PAGO"].ToString());
            }
        }
        else
        {
            Fecha_Actual = DateTime.Today;
        }

        //Valida que tenga parcialidades
        if (Dt_Parcialidades != null && Dt_Parcialidades.Rows.Count > 0)
        {
            for (Int32 Contador = 0; Contador < Dt_Parcialidades.Rows.Count; Contador++)
            {
                if (!String.IsNullOrEmpty(Dt_Parcialidades.Rows[Contador]["FECHA_VENCIMIENTO"].ToString()))
                {
                    DateTime Fecha_Vencimiento = Convert.ToDateTime(Dt_Parcialidades.Rows[Contador]["FECHA_VENCIMIENTO"].ToString());
                    Fecha_Vencimiento = Convert.ToDateTime(Dias_Inhabiles.Calcular_Fecha(Fecha_Vencimiento.ToShortDateString(), "10"));
                    if (Fecha_Vencimiento < Fecha_Actual)
                    {
                        Convenio_Incumplido = true;
                        break;
                    }
                }
            }
        }
        return Convenio_Incumplido;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Alta_Pasivo_Otros_Pagos
    ///DESCRIPCIÓN: Carga el Pasivo en la Tabla a Pagar.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Alta_Pasivo_Otros_Pagos()
    {
        Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio Negocio = new Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio();
        Negocio.P_Referencia = "OTRPAG" + Txt_Clave_Ingreso.Text.Trim();
        Negocio.P_Clave_Ingreso = Hdf_Clave_Ingreso_ID.Value.Trim();
        Negocio.P_Descripcion = Txt_Clave_Ingreso_Descripcion.Text.Trim();
        Negocio.P_Dependencia_ID = Hdf_Dependencia_ID.Value.Trim();
        Negocio.P_Cantidad = Convert.ToInt32(Txt_Cantidad.Text.Trim());
        Negocio.P_Monto = Convert.ToDouble(Txt_Total_Pago.Text.Trim().Replace("$", ""));
        Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado.Trim();
        Negocio.P_Contribuyente = Txt_Nombre_Contribuyente.Text.ToUpper();
        Negocio.P_Observaciones = Txt_Observaciones.Text.ToUpper();
        Negocio.Alta_Pasivo();
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Alta_Pasivo_Convenios
    ///DESCRIPCIÓN: Carga el Pasivo en la Tabla a Pagar.
    ///PARAMETROS: Referencia, pasa la referencia para la consulta, Concepto, pasa la descripcion para obtener el convenio
    ///CREO: Ismael Prieto Sánchez  
    ///FECHA_CREO: 2 Noviembre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Alta_Pasivo_Convenios(String Referencia, String Concepto)
    {
        String No_Convenio = "";
        String No_Pago_Convenio = "";
        Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio Negocio = new Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio();
        DataTable Dt_Convenio = new DataTable();
        Double Impuestos = 0;
        Double Impuesto_Division = 0;
        Double Constancias = 0;
        Double Multas = 0;
        Double Recargos = 0;
        Double Moratorios = 0;
        String Tipo_Predio_ID = "";

        //Crea la tabla de detalles
        DataTable Dt_Adeudos_Convenios = new DataTable();
        Dt_Adeudos_Convenios.Columns.Add("NO_CONVENIO", Type.GetType("System.String"));
        Dt_Adeudos_Convenios.Columns.Add("NO_PAGO", Type.GetType("System.String"));

        //Crea la tabla de totales
        DataTable Dt_Adeudos_Totales = new DataTable();
        Dt_Adeudos_Totales.Columns.Add("CONCEPTO", Type.GetType("System.String"));
        Dt_Adeudos_Totales.Columns.Add("MONTO", Type.GetType("System.Double"));

        //Elimina el pasivo pendiente de pago
        Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio Elimina_Pasivo = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
        Elimina_Pasivo.P_Cuenta_Predial = Referencia;
        Elimina_Pasivo.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value.Trim();
        Elimina_Pasivo.P_Estatus = "POR PAGAR";
        Elimina_Pasivo.Eliminar_Pasivos_No_Pagados_Anteriormente();

        //Valida para los convenios de traslado
        if (Referencia.StartsWith("TD") || Referencia.StartsWith("CTRA"))
        {
            No_Convenio = Concepto.Substring(34);
            No_Convenio = No_Convenio.Substring(0, 10);
            Negocio.P_No_Convenio = No_Convenio;
            No_Pago_Convenio = Concepto.Substring(58).Trim();
            No_Pago_Convenio = No_Pago_Convenio.Substring(0, No_Pago_Convenio.IndexOf("/"));
            Negocio.P_No_Convenio_Pago = Convert.ToInt32(No_Pago_Convenio);
            Dt_Convenio = Negocio.Consultar_Convenio_Traslado_Dominio_Parcialidades();
        }
        //Valida para los convenios de fraccionamientos
        if (Referencia.StartsWith("IMP") || Referencia.StartsWith("CFRA"))
        {
            No_Convenio = Concepto.Substring(42);
            No_Convenio = No_Convenio.Substring(0, 10);
            Negocio.P_No_Convenio = No_Convenio;
            No_Pago_Convenio = Concepto.Substring(66).Trim();
            No_Pago_Convenio = No_Pago_Convenio.Substring(0, No_Pago_Convenio.IndexOf("/"));
            Negocio.P_No_Convenio_Pago = Convert.ToInt32(No_Pago_Convenio);
            Dt_Convenio = Negocio.Consultar_Convenio_Fraccionamiento_Parcialidades();
        }
        //Valida para los convenios de derechos de supervision
        if (Referencia.StartsWith("DER") || Referencia.StartsWith("CDER"))
        {
            No_Convenio = Concepto.Substring(38);
            No_Convenio = No_Convenio.Substring(0, 10);
            Negocio.P_No_Convenio = No_Convenio;
            No_Pago_Convenio = Concepto.Substring(62).Trim();
            No_Pago_Convenio = No_Pago_Convenio.Substring(0, No_Pago_Convenio.IndexOf("/"));
            Negocio.P_No_Convenio_Pago = Convert.ToInt32(No_Pago_Convenio);
            Dt_Convenio = Negocio.Consultar_Convenio_Derechos_Supervision_Parcialidades();
        }
        if (Dt_Convenio.Rows.Count > 0)
        {
            //Obtiene los montos de la parcialidad
            foreach (DataRow Registro in Dt_Convenio.Rows)
            {
                if (Referencia.StartsWith("TD") || Referencia.StartsWith("CTRA"))
                {
                    Tipo_Predio_ID = Registro["TIPO_PREDIO_ID"].ToString();
                }
                else
                {
                    Tipo_Predio_ID = "";
                }
                DataRow Fila_Nueva = Dt_Adeudos_Convenios.NewRow();
                Fila_Nueva["NO_CONVENIO"] = No_Convenio;
                Fila_Nueva["NO_PAGO"] = Registro["NO_PAGO"].ToString();
                Dt_Adeudos_Convenios.Rows.Add(Fila_Nueva);

                Impuestos += Convert.ToDouble(Registro["MONTO_IMPUESTO"].ToString());
                if (Referencia.StartsWith("TD") || Referencia.StartsWith("CTRA"))
                {
                    Impuesto_Division += Convert.ToDouble(Registro["MONTO_IMPUESTO"].ToString());
                }
                Constancias += Convert.ToDouble(Registro["MONTO"].ToString());
                Multas += Convert.ToDouble(Registro["MONTO_MULTAS"].ToString());
                Recargos += Convert.ToDouble(Registro["RECARGOS_ORDINARIOS"].ToString());
                Moratorios += Convert.ToDouble(Registro["RECARGOS_MORATORIOS"].ToString());
            }
            Session["ADEUDO_PREDIAL_CAJA"] = Dt_Adeudos_Convenios;

            //Registra los pasivos
            if (Impuestos > 0)
            {
                Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio RP_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
                RP_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                RP_Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                RP_Negocio.P_Cuenta_Predial = Referencia;
                if (Referencia.StartsWith("TD"))
                {
                    if (Tipo_Predio_ID == "00001")
                    {
                        RP_Negocio.P_Clave_Ingreso = Obtener_Clave_Ingreso("TRASLADO", "IMPUESTO URBANO");
                    }
                    else if (Tipo_Predio_ID == "00002")
                    {
                        RP_Negocio.P_Clave_Ingreso = Obtener_Clave_Ingreso("TRASLADO", "IMPUESTO RUSTICO");
                    }
                    else
                    {
                        RP_Negocio.P_Clave_Ingreso = Obtener_Clave_Ingreso("TRASLADO", "IMPUESTO");
                    }
                    RP_Negocio.P_Descripcion = "IMPUESTO DE TRASLADO";
                }
                else if (Referencia.StartsWith("IMP"))
                {
                    RP_Negocio.P_Clave_Ingreso = Obtener_Clave_Ingreso("FRACCIONAMIENTOS", "IMPUESTO");
                    RP_Negocio.P_Descripcion = "IMPUESTO DE FRACCIONAMIENTO";
                }
                else if (Referencia.StartsWith("DER"))
                {
                    RP_Negocio.P_Clave_Ingreso = Obtener_Clave_Ingreso("DERECHOS DE SUPERVISION", "IMPUESTO");
                    RP_Negocio.P_Descripcion = "DERECHOS DE SUPERVISION";
                }
                RP_Negocio.P_Fecha_Tramite = DateTime.Today;
                RP_Negocio.P_Fecha_Vencimiento = DateTime.Today;
                RP_Negocio.P_Monto = Impuestos;
                RP_Negocio.P_Estatus = "POR PAGAR";
                RP_Negocio.P_Dependencia = Obtener_Dependencia_Clave_Ingreso(RP_Negocio.P_Clave_Ingreso);
                RP_Negocio.P_Contribuyente = Txt_Propietario_Cuenta.Text;
                RP_Negocio.Alta_Pasivo();
                //Agrega la Fila de Impuesto
                DataRow Fila = Dt_Adeudos_Totales.NewRow();
                if (Referencia.StartsWith("TD"))
                {
                    Fila["CONCEPTO"] = "IMPUESTO DE TRASLADO";
                }
                else if (Referencia.StartsWith("DER"))
                {
                    Fila["CONCEPTO"] = "DERECHOS DE SUPERVISION";
                }
                else
                {
                    Fila["CONCEPTO"] = "IMPUESTO";
                }
                Fila["MONTO"] = Impuestos;
                Dt_Adeudos_Totales.Rows.Add(Fila);
            }
            if (Impuesto_Division > 0)
            {
                Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio RP_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
                RP_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                RP_Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                RP_Negocio.P_Cuenta_Predial = Referencia;
                if (Referencia.StartsWith("TD"))
                {
                    if (Tipo_Predio_ID == "00001")
                    {
                        RP_Negocio.P_Clave_Ingreso = Obtener_Clave_Ingreso("TRASLADO", "IMPUESTO URBANO");
                    }
                    else if (Tipo_Predio_ID == "00002")
                    {
                        RP_Negocio.P_Clave_Ingreso = Obtener_Clave_Ingreso("TRASLADO", "IMPUESTO RUSTICO");
                    }
                    else
                    {
                        RP_Negocio.P_Clave_Ingreso = Obtener_Clave_Ingreso("TRASLADO", "IMPUESTO");
                    }
                    RP_Negocio.P_Descripcion = "IMPUESTO DE DIVISION";
                }
                RP_Negocio.P_Fecha_Tramite = DateTime.Today;
                RP_Negocio.P_Fecha_Vencimiento = DateTime.Today;
                RP_Negocio.P_Monto = Impuesto_Division;
                RP_Negocio.P_Estatus = "POR PAGAR";
                RP_Negocio.P_Dependencia = Obtener_Dependencia_Clave_Ingreso(RP_Negocio.P_Clave_Ingreso);
                RP_Negocio.P_Contribuyente = Txt_Propietario_Cuenta.Text;
                RP_Negocio.Alta_Pasivo();
                //Agrega la Fila de Impuesto
                DataRow Fila = Dt_Adeudos_Totales.NewRow();
                if (Referencia.StartsWith("TD"))
                {
                    Fila["CONCEPTO"] = "IMPUESTO DE DIVISION";
                }
                else
                {
                    Fila["CONCEPTO"] = "IMPUESTO";
                }
                Fila["MONTO"] = Impuesto_Division;
                Dt_Adeudos_Totales.Rows.Add(Fila);
            }
            if (Constancias > 0)
            {
                // consultar id de constancia de no adeudo del catalogo de parametros
                Cls_Cat_Pre_Claves_Ingreso_Negocio Rs_Claves_Ingreso = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
                Cls_Ope_Pre_Parametros_Negocio Rs_Parametros = new Cls_Ope_Pre_Parametros_Negocio();
                DataTable Dt_Parametros = Rs_Parametros.Consultar_Parametros();
                DataTable Dt_Clave;

                Rs_Claves_Ingreso.P_Tipo = String.Empty;
                Rs_Claves_Ingreso.P_Tipo_Predial_Traslado = String.Empty;
                Rs_Claves_Ingreso.P_Documento_ID = Dt_Parametros.Rows[0][Ope_Pre_Parametros.Campo_Constancia_No_Adeudo].ToString();
                Dt_Clave = Rs_Claves_Ingreso.Consultar_Clave_Ingreso();

                Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio RP_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
                RP_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                RP_Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                RP_Negocio.P_Cuenta_Predial = Referencia;
                RP_Negocio.P_Clave_Ingreso = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID].ToString();
                RP_Negocio.P_Descripcion = "CONSTANCIA";
                RP_Negocio.P_Fecha_Tramite = DateTime.Today;
                RP_Negocio.P_Fecha_Vencimiento = DateTime.Today;
                RP_Negocio.P_Monto = Constancias;
                RP_Negocio.P_Estatus = "POR PAGAR";
                RP_Negocio.P_Dependencia = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID].ToString();
                RP_Negocio.P_Contribuyente = Txt_Propietario_Cuenta.Text;
                RP_Negocio.Alta_Pasivo();
                //Agrega la Fila de Constancia
                DataRow Fila = Dt_Adeudos_Totales.NewRow();
                Fila["CONCEPTO"] = "CONSTANCIA";
                Fila["MONTO"] = Constancias;
                Dt_Adeudos_Totales.Rows.Add(Fila);
            }
            if (Multas > 0)
            {
                Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio RP_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
                RP_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                RP_Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                RP_Negocio.P_Cuenta_Predial = Referencia;
                if (Referencia.StartsWith("TD"))
                {
                    RP_Negocio.P_Clave_Ingreso = Obtener_Clave_Ingreso("TRASLADO", "MULTAS");
                    RP_Negocio.P_Descripcion = "MULTAS";
                }
                else if (Referencia.StartsWith("IMP"))
                {
                    DataTable Dt_Clave;
                    Cls_Cat_Pre_Claves_Ingreso_Negocio Claves_Ingreso = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
                    Claves_Ingreso.P_Tipo = "FRACCIONAMIENTOS";
                    Claves_Ingreso.P_Tipo_Predial_Traslado = "MULTAS";
                    Dt_Clave = Claves_Ingreso.Consultar_Clave_Ingreso();
                    RP_Negocio.P_Clave_Ingreso = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID].ToString();
                    RP_Negocio.P_Descripcion = "MULTAS";
                }
                else if (Referencia.StartsWith("DER"))
                {
                    DataTable Dt_Clave;
                    Cls_Cat_Pre_Claves_Ingreso_Negocio Claves_Ingreso = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
                    Claves_Ingreso.P_Tipo = "DERECHOS DE SUPERVISION";
                    Claves_Ingreso.P_Tipo_Predial_Traslado = "MULTAS";
                    Dt_Clave = Claves_Ingreso.Consultar_Clave_Ingreso();
                    RP_Negocio.P_Clave_Ingreso = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID].ToString();
                    RP_Negocio.P_Descripcion = "MULTAS";
                }
                RP_Negocio.P_Fecha_Tramite = DateTime.Today;
                RP_Negocio.P_Fecha_Vencimiento = DateTime.Today;
                RP_Negocio.P_Monto = Multas;
                RP_Negocio.P_Estatus = "POR PAGAR";
                RP_Negocio.P_Dependencia = Obtener_Dependencia_Clave_Ingreso(RP_Negocio.P_Clave_Ingreso);
                RP_Negocio.P_Contribuyente = Txt_Propietario_Cuenta.Text;
                RP_Negocio.Alta_Pasivo();
                //Agrega la Fila de Multas
                DataRow Fila = Dt_Adeudos_Totales.NewRow();
                Fila["CONCEPTO"] = "MULTAS";
                Fila["MONTO"] = Multas;
                Dt_Adeudos_Totales.Rows.Add(Fila);
            }
            if (Recargos > 0)
            {
                Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio RP_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
                RP_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                RP_Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                RP_Negocio.P_Cuenta_Predial = Referencia;
                if (Referencia.StartsWith("TD"))
                {
                    RP_Negocio.P_Clave_Ingreso = Obtener_Clave_Ingreso("TRASLADO", "RECARGOS ORDINARIOS");
                    RP_Negocio.P_Descripcion = "RECARGOS ORDINARIOS";
                }
                else if (Referencia.StartsWith("IMP"))
                {
                    RP_Negocio.P_Clave_Ingreso = Obtener_Clave_Ingreso("FRACCIONAMIENTOS", "RECARGOS ORDINARIOS");
                    RP_Negocio.P_Descripcion = "RECARGOS ORDINARIOS";
                }
                else if (Referencia.StartsWith("DER"))
                {
                    RP_Negocio.P_Clave_Ingreso = Obtener_Clave_Ingreso("DERECHOS DE SUPERVISION", "RECARGOS ORDINARIOS");
                    RP_Negocio.P_Descripcion = "RECARGOS ORDINARIOS";
                }
                RP_Negocio.P_Fecha_Tramite = DateTime.Today;
                RP_Negocio.P_Fecha_Vencimiento = DateTime.Today;
                RP_Negocio.P_Monto = Recargos;
                RP_Negocio.P_Estatus = "POR PAGAR";
                RP_Negocio.P_Dependencia = Obtener_Dependencia_Clave_Ingreso(RP_Negocio.P_Clave_Ingreso);
                RP_Negocio.P_Contribuyente = Txt_Propietario_Cuenta.Text;
                RP_Negocio.Alta_Pasivo();
                //Agrega la Fila de Recargos Ordinarios
                DataRow Fila = Dt_Adeudos_Totales.NewRow();
                Fila["CONCEPTO"] = "RECARGOS";
                Fila["MONTO"] = Recargos;
                Dt_Adeudos_Totales.Rows.Add(Fila);
            }
            if (Moratorios > 0)
            {
                Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio RP_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
                RP_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                RP_Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                RP_Negocio.P_Cuenta_Predial = Referencia;
                if (Referencia.StartsWith("TD"))
                {
                    RP_Negocio.P_Clave_Ingreso = Obtener_Clave_Ingreso("TRASLADO", "RECARGOS MORATORIOS");
                    RP_Negocio.P_Descripcion = "RECARGOS MORATORIOS";
                }
                else if (Referencia.StartsWith("IMP"))
                {
                    RP_Negocio.P_Clave_Ingreso = Obtener_Clave_Ingreso("FRACCIONAMIENTOS", "RECARGOS MORATORIOS");
                    RP_Negocio.P_Descripcion = "RECARGOS MORATORIOS";
                }
                else if (Referencia.StartsWith("DER"))
                {
                    RP_Negocio.P_Clave_Ingreso = Obtener_Clave_Ingreso("DERECHOS DE SUPERVISION", "RECARGOS MORATORIOS");
                    RP_Negocio.P_Descripcion = "RECARGOS MORATORIOS";
                }
                RP_Negocio.P_Fecha_Tramite = DateTime.Today;
                RP_Negocio.P_Fecha_Vencimiento = DateTime.Today;
                RP_Negocio.P_Monto = Moratorios;
                RP_Negocio.P_Estatus = "POR PAGAR";
                RP_Negocio.P_Dependencia = Obtener_Dependencia_Clave_Ingreso(RP_Negocio.P_Clave_Ingreso);
                RP_Negocio.P_Contribuyente = Txt_Propietario_Cuenta.Text;
                RP_Negocio.Alta_Pasivo();
                //Agrega la Fila de Recargos Moratorios
                DataRow Fila = Dt_Adeudos_Totales.NewRow();
                Fila["CONCEPTO"] = "MORATORIOS";
                Fila["MONTO"] = Moratorios;
                Dt_Adeudos_Totales.Rows.Add(Fila);
            }
            Session["ADEUDO_PREDIAL_DETALLES"] = Dt_Adeudos_Totales;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Obtener_Clave_Ingreso
    ///DESCRIPCIÓN:Obtiene la Clave de Ingreso.
    ///PARAMETROS: Tipo. Tipo de la Clave que se buscara
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private String Obtener_Clave_Ingreso(String Tipo, String Clave_Ingreso)
    {
        String Clave = null;
        Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio RP_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
        RP_Negocio.P_Clave_Ingreso = Clave_Ingreso;
        RP_Negocio.P_Tipo_Clave_Ingreso = Tipo;
        DataTable Dt_Temporal = RP_Negocio.Consultar_Clave_Ingreso();
        if (Dt_Temporal.Rows.Count > 0)
        {
            Clave = Dt_Temporal.Rows[0]["CLAVE_INGRESO"].ToString();
        }
        return Clave;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Obtener_Clave_Ingreso
    ///DESCRIPCIÓN:Obtiene la Dependencia de una Clave de Ingreso.
    ///PARAMETROS: Tipo. Tipo de la Clave que se buscara
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private String Obtener_Dependencia_Clave_Ingreso(String Clave_Ingreso)
    {
        String Dependencia = null;
        Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio RP_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
        RP_Negocio.P_Clave_Ingreso = Clave_Ingreso;
        DataTable Dt_Temporal = RP_Negocio.Consultar_Dependencia();
        if (Dt_Temporal.Rows.Count > 0)
        {
            Dependencia = Dt_Temporal.Rows[0]["DEPENDENCIA"].ToString();
        }
        return Dependencia;
    }
    #endregion

    #endregion

    #region  "Grids"

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Adeudos_RowDataBound
    ///DESCRIPCIÓN: Ejecuta el Metodo RowDataBound del Grid de Adeudos.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 12 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Listado_Adeudos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("Chk_Seleccion_Adeudo") != null)
                {
                    CheckBox Chk_Seleccion_Adeudo = (CheckBox)e.Row.FindControl("Chk_Seleccion_Adeudo");
                    Chk_Seleccion_Adeudo.TabIndex = (short)e.Row.RowIndex;
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

    #endregion

    #region  "Eventos"

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Avanzada_Cuenta_Predial_Click
    ///DESCRIPCIÓN: Lanza la Busqueda de la Cuenta_Predial y carga los adeudos.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 12 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Busqueda_Avanzada_Cuenta_Predial_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Limpiar_Componentes();
            String Cuenta_Predial = null;
            if (Session["CUENTA_PREDIAL"] != null)
            {
                Cuenta_Predial = Session["CUENTA_PREDIAL"].ToString();
                Mostrar_Informacion_Cuenta_Predial(Cuenta_Predial, false);
                Cuenta_Suspendida();
            }
            Session.Remove("CUENTA_PREDIAL_ID");
            Session.Remove("CUENTA_PREDIAL");
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = "Error:";
            Lbl_Mensaje_Error.Text = "['" + Ex.Message + "']";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Chk_Seleccion_Adeudo_CheckedChanged
    ///DESCRIPCIÓN: Carga como seleccionado el Adeudo para pagarlo.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 12 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Seleccion_Adeudo_CheckedChanged(object sender, EventArgs e)
    {
        Int32 Concepto_Seleccionado = Obtener_Seleccion_Adeudo();
        Boolean Valida_Orden_Traslado = true;

        if (Concepto_Seleccionado > (-1))
        {
            //Selecciona la fila
            GridViewRow Fila_Seleccionada = Grid_Listado_Adeudos.Rows[Concepto_Seleccionado];
            String Tipo_Concepto = HttpUtility.HtmlDecode(Fila_Seleccionada.Cells[2].Text.Trim());
            String Referencia = HttpUtility.HtmlDecode(Fila_Seleccionada.Cells[3].Text.Trim());
            String Concepto = HttpUtility.HtmlDecode(Fila_Seleccionada.Cells[5].Text.Trim());
            //Recorre para cuando es de traslado
            if (Referencia.StartsWith("TD"))
            {
                String Ref_Grid = Referencia;
                string Ref_No_Calculo = Ref_Grid.Substring(2);
                Int32 Ref_Anio_Calculo = 0;
                Ref_Anio_Calculo = Convert.ToInt16(Ref_No_Calculo.Substring(Ref_No_Calculo.Length - 4));
                Ref_No_Calculo = Ref_No_Calculo.Substring(0, Ref_No_Calculo.Length - 4);
                for (Int32 Contador = 0; Contador < Grid_Listado_Adeudos.Rows.Count; Contador++)
                {
                    Ref_Grid = HttpUtility.HtmlDecode(Grid_Listado_Adeudos.Rows[Contador].Cells[3].Text.Trim());
                    if (Ref_Grid.StartsWith("TD"))
                    {
                        string Grid_No_Calculo = Ref_Grid.Substring(2);
                        Int32 Grid_Anio_Calculo = 0;
                        Grid_Anio_Calculo = Convert.ToInt16(Grid_No_Calculo.Substring(Grid_No_Calculo.Length - 4));
                        Grid_No_Calculo = Grid_No_Calculo.Substring(0, Grid_No_Calculo.Length - 4);
                        if (Ref_Anio_Calculo < Grid_Anio_Calculo)
                        {
                            Valida_Orden_Traslado = false;
                            CheckBox Chk_Seleccion_Adeudo_Tmp = (CheckBox)Fila_Seleccionada.FindControl("Chk_Seleccion_Adeudo");
                            Chk_Seleccion_Adeudo_Tmp.Checked = false;
                            break;
                        }
                        else
                        {
                            if (Convert.ToInt32(Ref_No_Calculo) > Convert.ToInt32(Grid_No_Calculo))
                            {
                                Valida_Orden_Traslado = false;
                                CheckBox Chk_Seleccion_Adeudo_Tmp = (CheckBox)Fila_Seleccionada.FindControl("Chk_Seleccion_Adeudo");
                                Chk_Seleccion_Adeudo_Tmp.Checked = false;
                                break;
                            }
                            else
                            {
                                Valida_Orden_Traslado = true;
                            }
                        }
                    }
                }
            }
            //Redirecciona de acuerdo al tipo de pago
            if (Valida_Orden_Traslado)
            {
                if (Tipo_Concepto.Trim().Equals("ADEUDO_PREDIAL"))
                {
                    if (Lbl_Estatus.Text == "SUSPENDIDA" && (Hdf_Tipo_Suspension.Value == "PREDIAL" || Hdf_Tipo_Suspension.Value == "AMBAS"))
                    {
                        Lbl_Estatus.Visible = true;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('Cuenta SUSPENDIDA, no es posible realizar pagos de impuesto predial.');", true);
                        return;
                    }
                    Direccionar_Pagina("Predial", Referencia);
                }
                else
                {
                    if (Referencia.StartsWith("TD"))
                    {
                        if (Concepto.StartsWith("TRASLADO DE DOMINIO : No.Convenio"))
                        {
                            Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Conv = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
                            Conv.P_No_Convenio = Concepto.Substring(34, 10);
                            Conv.Consultar_Convenio_Traslado_Dominio();
                            foreach (DataRow Dr_Conv in Conv.P_Dt_Parcialidades.Rows)
                            {
                                if (Dr_Conv[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus].ToString() == "POR PAGAR")
                                {
                                    DateTime DaTe = Convert.ToDateTime(Dr_Conv[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Fecha_Vencimiento].ToString()).AddDays(1);
                                    if (DateTime.Compare(DaTe, DateTime.Now) < 1)
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "GACO", "alert('No se puede realizar el pago de este convenio ya que esta vencido.');", true);
                                        return;
                                    }
                                    break;
                                }
                            }
                            Alta_Pasivo_Convenios(Referencia, Concepto);
                        }
                    }
                    if (Referencia.StartsWith("IMP"))
                    {
                        if (Concepto.StartsWith("IMPUESTO DE FRACCIONAMIENTO : No.Convenio"))
                        {
                            Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio Conv = new Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio();
                            Conv.P_No_Convenio = Concepto.Substring(42, 10);
                            Conv.Consultar_Convenio_Fraccionamiento();
                            foreach (DataRow Dr_Conv in Conv.P_Dt_Parcialidades.Rows)
                            {
                                if (Dr_Conv[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus].ToString() == "POR PAGAR")
                                {
                                    DateTime DaTe = Convert.ToDateTime(Dr_Conv[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Fecha_Vencimiento].ToString()).AddDays(1);
                                    if (DateTime.Compare(DaTe, DateTime.Now) < 1)
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "GACO", "alert('No se puede realizar el pago de este convenio ya que esta vencido.');", true);
                                        return;
                                    }
                                    break;
                                }
                            }
                            Alta_Pasivo_Convenios(Referencia, Concepto);
                        }
                    }
                    if (Referencia.StartsWith("DER"))
                    {
                        if (Concepto.StartsWith("DERECHOS DE SUPERVISION : No.Convenio"))
                        {
                            Cls_Ope_Pre_Convenios_Derechos_Supervision_Negocio Conv = new Cls_Ope_Pre_Convenios_Derechos_Supervision_Negocio();
                            Conv.P_No_Convenio = Concepto.Substring(38, 10);
                            Conv.Consultar_Convenio_Derecho_Supervisions();
                            foreach (DataRow Dr_Conv in Conv.P_Dt_Parcialidades.Rows)
                            {
                                if (Dr_Conv[Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Estatus].ToString() == "POR PAGAR")
                                {
                                    DateTime DaTe = Convert.ToDateTime(Dr_Conv[Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Fecha_Vencimiento].ToString()).AddDays(1);
                                    if (DateTime.Compare(DaTe, DateTime.Now) < 1)
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "GACO", "alert('No se puede realizar el pago de este convenio ya que esta vencido.');", true);
                                        return;
                                    }
                                    break;
                                }
                            }
                            Alta_Pasivo_Convenios(Referencia, Concepto);
                        }
                    }
                    Direccionar_Pagina("Caja", Referencia);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('Existe un traslado anterior que debe de pagarse primero, favor de verificarlo.');", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('Es necesario Seleccionar el Concepto a Pagar');", true);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_No_Folio_TextChanged
    ///DESCRIPCIÓN: Hace la busqueda por referencia.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 14 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_No_Folio_TextChanged(object sender, EventArgs e)
    {
        String Referencia = Txt_No_Folio.Text.Trim().ToUpper();
        Limpiar_Componentes();
        if (Referencia.Length > 0)
        {
            Txt_No_Folio.Text = Referencia;
            if (Referencia.StartsWith("TD") && Referencia.Trim().Length < 7)
            {
                return;
            }
            else if (Referencia.StartsWith("TD"))
            {
                Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Conv = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
                String Calculo;
                Calculo = Referencia.Substring(2);
                Calculo = Calculo.Substring(0, Calculo.Length - 4);
                Conv.P_Anio = Referencia.Substring(Referencia.Length - 4, 4);
                Conv.P_No_Calculo = Convert.ToDouble(Calculo).ToString("0000000000");
                Conv.P_Filtros_Dinamicos = Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Calculo + "='" + Conv.P_No_Calculo + "' AND " + Ope_Pre_Convenios_Traslados_Dominio.Campo_Anio + "=" + Conv.P_Anio + " AND " + Ope_Pre_Convenios_Traslados_Dominio.Campo_Estatus + " IN ('ACTIVO','INCUMPLIDO','PENDIENTE')";
                if (Conv.Consultar_Convenio_Traslado_Dominio().Rows.Count > 0)
                {
                    return;
                }
            }
            if ((Referencia.StartsWith("IMP") || Referencia.StartsWith("DER")) && Referencia.Trim().Length < 3)
            {
                return;
            }
            else if (Referencia.StartsWith("IMP"))
            {
                Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio Conv = new Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio();
                Conv.P_Filtros_Dinamicos = Ope_Pre_Convenios_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + "='" + Convert.ToInt32(Referencia.Substring(5)).ToString("0000000000") + "' AND " + Ope_Pre_Convenios_Fraccionamientos.Campo_Estatus + " IN ('ACTIVO','INCUMPLIDO','PENDIENTE') AND " + Ope_Pre_Convenios_Fraccionamientos.Campo_Anio + "=20" + Referencia.Substring(3, 2);
                if (Conv.Consultar_Convenio_Fraccionamiento().Rows.Count > 0)
                {
                    return;
                }
            }
            if (Referencia.StartsWith("CPRE") || Referencia.StartsWith("CTRA") || Referencia.StartsWith("CFRA") || Referencia.StartsWith("CDER"))
            {
                Consulta_Datos_Convenio(Referencia);
                Cuenta_Suspendida();
            }
            else
            {
                Mostrar_Informacion_Referencia(Referencia);
                Cuenta_Suspendida();
            }
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Limpiar_Formulario_Click
    ///DESCRIPCIÓN: Limpia el Formulario.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 14 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Limpiar_Formulario_Click(object sender, ImageClickEventArgs e)
    {
        Limpiar_Componentes();
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Se sale del Formulario.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 14 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_Cuenta_Predial_TextChanged
    ///DESCRIPCIÓN: Ejecuta una busqueda de Cuenta Predial metida Manualmente.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 14 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Cuenta_Predial_TextChanged(object sender, EventArgs e)
    {
        String Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim().ToUpper();
        Limpiar_Componentes();
        if (Cuenta_Predial.Trim().Length > 0)
        {
            Txt_Cuenta_Predial.Text = Cuenta_Predial;
            Mostrar_Informacion_Cuenta_Predial(Cuenta_Predial, false);
            Cuenta_Suspendida();
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_Clave_Ingreso_TextChanged
    ///DESCRIPCIÓN: Ejecuta una busqueda de Clave de Ingreso metida Manualmente.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Clave_Ingreso_TextChanged(object sender, EventArgs e)
    {
        String Clave_Ingreso = Txt_Clave_Ingreso.Text.Trim();
        Limpiar_Componentes();
        if (Clave_Ingreso.Trim().Length > 0)
        {
            Txt_Clave_Ingreso.Text = Clave_Ingreso;
            Mostrar_Informacion_Clave_Ingreso(Clave_Ingreso);
            if (Hdf_Clave_Ingreso_ID.Value != "")
            {
                Consultar_Costos_Claves();
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_Costo_Unidad_TextChanged
    ///DESCRIPCIÓN: Cambio de Texto en las Caja de Costo Unitario.
    ///PARÁMETROS : 
    ///CREO       : Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO : 25-Octubre-2011
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************
    protected void Txt_Costo_Unidad_TextChanged(object sender, EventArgs e)
    {
        Calcular_Pago_Otros_Ingresos();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_Cantidad_TextChanged
    ///DESCRIPCIÓN: Cambio de Texto en las Caja de Cantidad.
    ///PARÁMETROS : 
    ///CREO       : Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO : 25-Octubre-2011
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************
    protected void Txt_Cantidad_TextChanged(object sender, EventArgs e)
    {
        Calcular_Pago_Otros_Ingresos();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Pagar_Caja_Click
    ///DESCRIPCIÓN: Evento Click para Pagar en Caja.
    ///PARÁMETROS : 
    ///CREO       : Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO : 25-Octubre-2011
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************
    protected void Btn_Pagar_Caja_Click(object sender, EventArgs e)
    {
        try
        {
            Double Total_Pagar = 0.0;
            if (Txt_Total_Pago.Text.Trim().Length > 0)
            {
                Total_Pagar = Convert.ToDouble(Txt_Total_Pago.Text.Trim().Replace("$", ""));
            }
            if (Total_Pagar > 0 && Txt_Nombre_Contribuyente.Text.Trim() != "")
            {
                Alta_Pasivo_Otros_Pagos();
                Direccionar_Pagina("Caja", "OTRPAG" + Txt_Clave_Ingreso.Text.Trim());
            }
            else
            {
                String Mensaje_Error = "";
                if (Total_Pagar == 0)
                {
                    Mensaje_Error += "El Valor a Pagar es de $0.00. Verificar";
                }
                if (Txt_Nombre_Contribuyente.Text.Trim() == "")
                {
                    if (Mensaje_Error != "")
                    {
                        Mensaje_Error += ", ";
                    }
                    Mensaje_Error += "Introduzca el nombre del contribuyente";
                }
                Mensaje_Error += ". Por favor.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "GACO", "alert('" + Mensaje_Error + "');", true);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = "Error:";
            Lbl_Mensaje_Error.Text = "['" + Ex.Message + "']";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Costos_Claves
    ///DESCRIPCIÓN: Consulta el costo de la clave de ingreso
    ///PARÁMETROS : 
    ///CREO       : Miguel Angel Bedolla Moreno
    ///FECHA_CREO : 17-Noviembre-2011
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************
    private void Consultar_Costos_Claves()
    {
        DataTable Costo;
        Cls_Cat_Pre_Claves_Ingreso_Negocio Clave_Costo = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
        Clave_Costo.P_Clave_Ingreso_ID = Hdf_Clave_Ingreso_ID.Value;
        Clave_Costo.P_Anio = DateTime.Now.ToString("yyyy");
        Costo = Clave_Costo.Consultar_Costos_Claves();
        if (Costo.Rows.Count > 0)
        {
            Lbl_Costo_Unidad.Text = "Costo/U [$]";
            Lbl_Total_Pago.Text = "Total";
            Txt_Costo_Unidad.Enabled = false;
            Txt_Cantidad.Enabled = true;
            Txt_Costo_Unidad.Text = Convert.ToDouble(Costo.Rows[0]["COSTO"].ToString()).ToString("###,###,##0.00");
            Txt_Cantidad.Text = "";
            Txt_Cantidad_TextChanged(null, null);
            Txt_Cantidad.Focus();
        }
        else
        {
            Lbl_Costo_Unidad.Text = "Importe";
            Lbl_Total_Pago.Text = "Total";
            Txt_Cantidad.Text = "1";
            Txt_Cantidad.Enabled = true;
            Txt_Costo_Unidad.Text = "";
            Txt_Costo_Unidad.Enabled = true;
            Txt_Cantidad_TextChanged(null, null);
            Txt_Costo_Unidad.Focus();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Obtener_Dato_Consulta
    ///DESCRIPCIÓN          : Consulta el Campo dado de la Tabla Indicada
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benvides Guardado
    ///FECHA_CREO           : 24/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private String Obtener_Dato_Consulta(String Campo, String Tabla, String Condiciones)
    {
        String Mi_SQL;
        String Dato_Consulta = "";

        try
        {
            Mi_SQL = "SELECT " + Campo;
            if (Tabla != "")
            {
                Mi_SQL += " FROM " + Tabla;
            }
            if (Condiciones != "")
            {
                Mi_SQL += " WHERE " + Condiciones;
            }

            OracleDataReader Dr_Dato = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            if (Dr_Dato.Read())
            {
                if (Dr_Dato[0] != null)
                {
                    Dato_Consulta = Dr_Dato[0].ToString();
                }
                else
                {
                    Dato_Consulta = "";
                }
                Dr_Dato.Close();
            }
            else
            {
                Dato_Consulta = "";
            }
            if (Dr_Dato != null)
            {
                Dr_Dato.Close();
            }
            Dr_Dato = null;
        }
        catch
        {
        }
        finally
        {
        }

        return Dato_Consulta;
    }

    #endregion

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Limpiar_Formulario_Click
    ///DESCRIPCIÓN: Limpia el Formulario.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 14 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cuenta_Suspendida()
    {
        Cls_Cat_Pre_Cuentas_Predial_Negocio Cuenta = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        Cuenta.P_Campos_Dinamicos = Cat_Pre_Cuentas_Predial.Campo_Estatus;
        if (Hdf_Cuenta_Predial_ID.Value.Trim() != "")
        {
            Cuenta.P_Filtros_Dinamicos = Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "='" + Hdf_Cuenta_Predial_ID.Value + "'";
        }
        else if (Txt_No_Folio.Text.Trim().Contains("IMP"))
        {
            String Campos = Ope_Pre_Impuestos_Fraccionamientos.Campo_Cuenta_Predial_ID;
            String Filtros = Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + "='" + Convert.ToInt32(Txt_No_Folio.Text.Trim().Substring(5)).ToString("0000000000") + "'";
            Cuenta.P_Filtros_Dinamicos = Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "='" + Obtener_Dato_Consulta(Campos, Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos, Filtros) + "'";
        }
        else if (Txt_No_Folio.Text.Trim().Contains("DER"))
        {
            String Campos = Ope_Pre_Impuestos_Derechos_Supervision.Campo_Cuenta_Predial_ID;
            String Filtros = Ope_Pre_Impuestos_Derechos_Supervision.Campo_No_Impuesto_Derecho_Supervision + "='" + Convert.ToInt32(Txt_No_Folio.Text.Trim().Substring(5)).ToString("0000000000") + "'";
            Cuenta.P_Filtros_Dinamicos = Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "='" + Obtener_Dato_Consulta(Campos, Ope_Pre_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Impuestos_Derechos_Supervision, Filtros) + "'";
        }
        else if (Txt_No_Folio.Text.Trim().Contains("TD"))
        {
            String Campos = Ope_Pre_Calculo_Imp_Traslado.Campo_Cuenta_Predial_Id;
            String Filtros = Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo + "='" + Convert.ToInt32(Txt_No_Folio.Text.Trim().Substring(6)).ToString("0000000000") + "' AND " + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + "=" + Txt_No_Folio.Text.Trim().Substring(2, 4);
            Cuenta.P_Filtros_Dinamicos = Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "='" + Obtener_Dato_Consulta(Campos, Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado, Filtros) + "'";
        }
        DataTable Dt_Cuenta = Cuenta.Consultar_Cuenta();
        if (Dt_Cuenta.Rows.Count > 0 && Dt_Cuenta.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString() == "SUSPENDIDA")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('La cuenta se encuentra Suspendida.');", true);
            Lbl_Estatus.Text = "SUSPENDIDA";
            Lbl_Estatus.Visible = true;
            //Div_Contenedor_Msj_Error.Visible = true;
        }
    }
}