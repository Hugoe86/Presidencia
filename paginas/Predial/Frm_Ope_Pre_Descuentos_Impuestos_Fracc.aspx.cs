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
using Presidencia.Constantes;
using Presidencia.Operacion_Predial_Impuestos_Fraccionamientos.Negocio;
using Presidencia.Operacion_Predial_Convenios_Impuestos_Traslado_Dominio.Negocio;
using Presidencia.Ope_Pre_Descuentos_Fracc.Negocio;
using Operacion_Predial_Orden_Variacion.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Catalogo_Calles.Negocio;
using Presidencia.Operacion_Resumen_Predio.Negocio;
using Presidencia.Operacion_Predial_Convenios_Fraccionamientos.Negocio;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Catalogo_Claves_Ingreso.Negocio;
using Presidencia.Operacion_Calculo_Impuesto_Traslado.Negocio;
using Presidencia.Catalogo_Rangos_De_Descuentos_Por_Rol.Negocio;
using Presidencia.Operacion_Predial_Dias_Inhabiles.Negocio;

public partial class paginas_Predial_Frm_Ope_Pre_Descuentos_Impuestos_Fracc : System.Web.UI.Page
{
    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Page_Load
    ///DESCRIPCIÓN          : Función que se cargar al iniciar la página
    ///PARAMETROS           : 
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 19/Diciembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Configuracion_Formulario(false);
                Cargar_Grid_Impuestos_Derechos_Supervision(0);
                Cargar_Grid_Descuentos_Derechos_Supervision(0);
                Div_Datos.Visible = false;
            }
            //Limpiamos algún mensaje de error que se halla quedado en el log, que muestra los errores del sistema.
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    #endregion

    #region Métodos generales

    protected void Configuracion_Formulario(Boolean Habilitado)
    {
        Txt_Monto_Desc_Multas.Enabled = Habilitado;
        Txt_Monto_Desc_Recargo.Enabled = Habilitado;
        Txt_Observaciones.Enabled = Habilitado;
        Txt_Porcentaje_Desc_Multas.Enabled = Habilitado;
        Txt_Prcentaje_Desc_Recargos.Enabled = Habilitado;
        Txt_Busqueda.Enabled = !Habilitado;
        Btn_Buscar.Enabled = !Habilitado;
        Cmb_Estatus.Enabled = Habilitado;
        Btn_Txt_Fecha_Inicial.Enabled = Habilitado;
        Btn_Txt_Fecha_Vencimiento.Enabled = Habilitado;
        Btn_Busqueda_Avanzada_Impuestos.Enabled = false;
        Grid_Descuentos_Der_Sup.Enabled = !Habilitado;
        Txt_Monto_Desc_Multas.Style["Text-align"] = "right";
        Txt_Monto_Desc_Recargo.Style["Text-align"] = "right";
        Txt_Monto_Impuesto_Der_Sup.Style["Text-align"] = "right";
        Txt_Monto_Multas.Style["Text-align"] = "right";
        Txt_Monto_Recargos.Style["Text-align"] = "right";
        Txt_Porcentaje_Desc_Multas.Style["Text-align"] = "right";
        Txt_Prcentaje_Desc_Recargos.Style["Text-align"] = "right";
        Txt_Total_Por_Pagar.Style["Text-align"] = "right";
    }

    protected void Limpiar_Componentes()
    {
        Txt_Cuenta_Predial.Text = "";
        Txt_Monto_Desc_Multas.Text = "";
        Txt_Monto_Desc_Recargo.Text = "";
        Txt_Monto_Impuesto_Der_Sup.Text = "";
        Txt_Monto_Multas.Text = "";
        Txt_Monto_Recargos.Text = "";
        Txt_No_Exterior.Text = "";
        Txt_No_Interior.Text = "";
        Txt_Observaciones.Text = "";
        Txt_Porcentaje_Desc_Multas.Text = "";
        Txt_Prcentaje_Desc_Recargos.Text = "";
        Txt_Propietario.Text = "";
        Txt_Realizo.Text = "";
        Txt_Total_Por_Pagar.Text = "";
        Txt_Busqueda_Cuenta_Predial.Text = "";
        Txt_Busqueda_No_Impuesto.Text = "";
        Txt_Calle.Text = "";
        Txt_Colonia.Text = "";
        Txt_Fecha_Inicial.Text = "";
        Txt_Fecha_Vencimiento.Text = "";

        Hdf_Cuenta_Predial_ID.Value = "";
        Hdf_No_Impuesto_Der_Sup.Value = "";
        Hdf_No_Descuento.Value = "";
        Hdf_No_Convenio.Value = "";
    }

    #endregion

    #region Cargar Grids y realizar búsquedas

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Grid_Impuestos_Derechos_Supervision
    ///DESCRIPCIÓN          : Llena la tabla de Impuestos de Derechos de supervisión con los registros encontrados.
    ///PARAMETROS           : 1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 31/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Grid_Impuestos_Derechos_Supervision(Int32 Pagina)
    {
        try
        {
            Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio Impuestos_Fraccionamiento = new Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio();
            Impuestos_Fraccionamiento.P_Campos_Dinamicos = "(SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + "." + Ope_Pre_Impuestos_Fraccionamientos.Campo_Cuenta_Predial_ID;
            if (Txt_Busqueda_Cuenta_Predial.Text.Length != 0)
            {
                Impuestos_Fraccionamiento.P_Campos_Dinamicos += " AND " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " LIKE '%" + Txt_Busqueda_Cuenta_Predial.Text + "%'";
            }
            Impuestos_Fraccionamiento.P_Campos_Dinamicos += ") AS Cuenta_Predial, ";
            Impuestos_Fraccionamiento.P_Campos_Dinamicos += Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + ", ";
            Impuestos_Fraccionamiento.P_Campos_Dinamicos += Ope_Pre_Impuestos_Fraccionamientos.Campo_Cuenta_Predial_ID + ", ";
            Impuestos_Fraccionamiento.P_Campos_Dinamicos += Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Vencimiento + ", ";
            Impuestos_Fraccionamiento.P_Campos_Dinamicos += Ope_Pre_Impuestos_Fraccionamientos.Campo_Estatus;
            if (Txt_Busqueda_No_Impuesto.Text.Trim() != "")
            {
                Impuestos_Fraccionamiento.P_Filtros_Dinamicos = "(";
                Impuestos_Fraccionamiento.P_Filtros_Dinamicos += Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + " LIKE '%" + Txt_Busqueda_No_Impuesto.Text.Trim() + "%'";
                Impuestos_Fraccionamiento.P_Filtros_Dinamicos += " OR " + Ope_Pre_Impuestos_Fraccionamientos.Campo_Cuenta_Predial_ID + " IN (SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " LIKE '%" + Txt_Busqueda_No_Impuesto.Text.Trim() + "%')";
                Impuestos_Fraccionamiento.P_Filtros_Dinamicos += ")";
            }
            if (Impuestos_Fraccionamiento.P_Filtros_Dinamicos != "")
            {
                Impuestos_Fraccionamiento.P_Filtros_Dinamicos = " AND ";
            }
            Impuestos_Fraccionamiento.P_Filtros_Dinamicos = Ope_Pre_Impuestos_Fraccionamientos.Campo_Estatus + " = 'POR PAGAR'";
            if (Txt_Busqueda_No_Impuesto.Text.Length != 0)
            {
                Impuestos_Fraccionamiento.P_Filtros_Dinamicos += " AND " + Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + " LIKE '%" + Txt_Busqueda_No_Impuesto.Text + "%'";
            }
            if (Impuestos_Fraccionamiento.P_Filtros_Dinamicos != null && Impuestos_Fraccionamiento.P_Filtros_Dinamicos != "")
            {
                Impuestos_Fraccionamiento.P_Filtros_Dinamicos += " AND ";
            }
            else
            {
                Impuestos_Fraccionamiento.P_Filtros_Dinamicos = "";
            }
            Impuestos_Fraccionamiento.P_Filtros_Dinamicos += Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + " NOT IN (SELECT " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + " FROM " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + " WHERE " + Ope_Pre_Convenios_Fraccionamientos.Campo_Estatus + " IN ('ACTIVO','PENDIENTE','INCUMPLIDO')) ";
            Impuestos_Fraccionamiento.P_Filtros_Dinamicos += " AND " + Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + " NOT IN (SELECT " + Ope_Pre_Descuento_Fracc.Tabla_Ope_Pre_Descuento_Fracc + "." + Ope_Pre_Descuento_Fracc.Campo_No_Impuesto_fraccionamiento + " FROM " + Ope_Pre_Descuento_Fracc.Tabla_Ope_Pre_Descuento_Fracc + " WHERE " + Ope_Pre_Descuento_Fracc.Campo_Estatus + " IN ('VIGENTE','BAJA'))";
            DataTable Tabla = Impuestos_Fraccionamiento.Consultar_Impuestos_Fraccionamiento();
            //if (Tabla != null)
            //{
            Grid_Impuestos_Derechos_Supervision.Columns[2].Visible = true;
            Grid_Impuestos_Derechos_Supervision.PageIndex = Pagina;
            Grid_Impuestos_Derechos_Supervision.DataSource = Tabla;
            foreach (DataRow Renglon_Actual in Tabla.Rows)
            {
                if (Renglon_Actual["CUENTA_PREDIAL"].ToString() == "")
                {
                    Renglon_Actual.Delete();
                }
            }
            Grid_Impuestos_Derechos_Supervision.DataBind();
            Grid_Impuestos_Derechos_Supervision.Columns[2].Visible = false;
            //}
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Grid_Impuestos_Derechos_Supervision
    ///DESCRIPCIÓN          : Llena la tabla de Impuestos de Derechos de supervisión con los registros encontrados.
    ///PARAMETROS           : 1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 31/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Grid_Descuentos_Derechos_Supervision(Int32 Pagina)
    {
        try
        {
            Cls_Ope_Pre_Descuentos_Fracc_Negocio Descuentos = new Cls_Ope_Pre_Descuentos_Fracc_Negocio();
            DataTable Dt_Descuento = new DataTable();
            Int32 Indice = Cmb_Busqueda_General.SelectedIndex;
            switch (Indice)
            {
                case 0:
                    Descuentos.P_Cuenta_Predial = Txt_Busqueda.Text.Trim().ToUpper();
                    break;
                case 1:
                    Int32 No_Descuento;
                    try
                    {
                        No_Descuento = Convert.ToInt32(Txt_Busqueda.Text.Trim());
                        Descuentos.P_No_Descuento = string.Format("{0:0000000000}", No_Descuento);
                    }
                    catch
                    {
                        Descuentos.P_No_Descuento = "";
                    }
                    break;
                case 2:
                    Int32 No_Impuesto;
                    try
                    {
                        No_Impuesto = Convert.ToInt32(Txt_Busqueda.Text.Trim());
                        Descuentos.P_No_Impuesto_Fracc = string.Format("{0:0000000000}", No_Impuesto);
                    }
                    catch
                    {
                        Descuentos.P_No_Impuesto_Fracc = "";
                    }
                    break;
            }
            Grid_Descuentos_Der_Sup.Columns[1].Visible = true;
            Grid_Descuentos_Der_Sup.Columns[2].Visible = true;
            Dt_Descuento = Descuentos.Consultar_Descuentos();
            Grid_Descuentos_Der_Sup.DataSource = Dt_Descuento;
            Grid_Descuentos_Der_Sup.PageIndex = Pagina;
            Grid_Descuentos_Der_Sup.DataBind();
            Grid_Descuentos_Der_Sup.Columns[1].Visible = false;
            Grid_Descuentos_Der_Sup.Columns[2].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Constancia_Propiedad_Click
    ///DESCRIPCIÓN          : Llena la Tabla con la opcion buscada
    ///PARAMETROS          :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 27/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Busqueda_Impuestos_Click(object sender, EventArgs e)
    {
        Cargar_Grid_Impuestos_Derechos_Supervision(0);
        Mpe_Busqueda_Empleados.Show();
    }

    #endregion


    #region Eventos Grid

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Impuestos_Derechos_Supervision_PageIndexChanging1
    ///DESCRIPCIÓN          : Maneja la paginación del GridView
    ///PARAMETROS:
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Impuestos_Derechos_Supervision_PageIndexChanging1(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Impuestos_Derechos_Supervision.SelectedIndex = (-1);
            Cargar_Grid_Impuestos_Derechos_Supervision(e.NewPageIndex);
            Mpe_Busqueda_Empleados.Show();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Impuestos_Derechos_Supervision_SelectedIndexChanged
    ///DESCRIPCIÓN          : Maneja la selección de las filas del GridView
    ///PARAMETROS:     
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 31/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Impuestos_Derechos_Supervision_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Impuestos_Derechos_Supervision.Rows.Count > 0)
        {
            Hdf_No_Impuesto_Der_Sup.Value = Grid_Impuestos_Derechos_Supervision.SelectedRow.Cells[1].Text;
            Cls_Ope_Pre_Descuentos_Fracc_Negocio Descuento = new Cls_Ope_Pre_Descuentos_Fracc_Negocio();
            Descuento.P_No_Impuesto_Fracc = Hdf_No_Impuesto_Der_Sup.Value;
            DataTable Dt_Descuento_Existente = Descuento.Consultar_Descuento_Activo();
            if (Dt_Descuento_Existente != null && Dt_Descuento_Existente.Rows.Count > 0)
            {
                Btn_Salir_Click(null, null);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Descuentos de Fraccionamiento", "alert('Descuento ya aplicado al Impuesto seleccionado')", true);
            }
            Hdf_Cuenta_Predial_ID.Value = Grid_Impuestos_Derechos_Supervision.DataKeys[Grid_Impuestos_Derechos_Supervision.SelectedIndex].Value.ToString();
            Txt_Cuenta_Predial.Text = Grid_Impuestos_Derechos_Supervision.SelectedRow.Cells[3].Text;
            Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio Impuesto = new Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio();
            Impuesto.P_No_Impuesto_Fraccionamiento = Hdf_No_Impuesto_Der_Sup.Value;
            DataTable Dt_Impuesto = new DataTable();
            Dt_Impuesto = Impuesto.Consultar_Impuestos_Con_Convenio();
            if (Dt_Impuesto.Rows.Count == 0)
            {
                //Consultar_Datos_Cuenta_Predial();
                Txt_Cuenta_Predial_TextChanged();
                Cargar_Grid_Impuestos_Derechos_Supervision(0);
                //Mpe_Busqueda_Empleados.Dispose();
                Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Cuenta_Pendiente = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
                Cuenta_Pendiente.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                if (!Cuenta_Pendiente.Consultar_Cuenta_Pendiente())
                {
                    Cargar_Datos();
                }
                //Impuesto.P_Campos_Sumados = true;
                //Impuesto.Consultar_Impuestos_Derecho_Supervisions();
                //Dt_Impuesto = Impuesto.P_Dt_Detalles_Impuestos_Derechos_Supervision;
                Dt_Impuesto = Descuento_En_Reestructura();//Almacena el convenio para consultar los costos corresponeientes, ya sea desde un convenio o impuesto...
                if (Dt_Impuesto.Rows.Count == 0 || (Dt_Impuesto.Rows.Count > 0 && Dt_Impuesto.Rows[0][Ope_Pre_Convenios_Fraccionamientos.Campo_No_Reestructura].ToString().Equals("")))
                {
                    Impuesto.P_Campos_Sumados = true;
                    Impuesto.P_No_Impuesto_Fraccionamiento = Hdf_No_Impuesto_Der_Sup.Value;
                    Impuesto.Consultar_Impuestos_Fraccionamiento();
                    Dt_Impuesto = Impuesto.P_Dt_Detalles_Impuestos_Fraccionamiento;//Almacena los costos del impuesto
                }
                else
                {
                    Dt_Impuesto = Obtener_Dato_Consulta(Dt_Impuesto);//Alamacena los datos de una reestructura o convenio, en caso de ser necesario...
                }
                Llenar_Costos(Dt_Impuesto);
                Txt_Monto_Desc_Recargo.Enabled = true;
                Txt_Monto_Desc_Multas.Enabled = true;
                Txt_Porcentaje_Desc_Multas.Enabled = true;
                Txt_Prcentaje_Desc_Recargos.Enabled = true;
            }
            else
            {
                Btn_Salir_Click(null, null);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Descuentos de Derechos de Supervisión", "alert('Impuesto con Convenio Vigente')", true);
            }
            Mpe_Busqueda_Empleados.Hide();
            Grid_Impuestos_Derechos_Supervision.SelectedIndex = -1;
        }
    }

    #endregion

    #region Consultar datos campos

    protected void Txt_Cuenta_Predial_TextChanged()
    {
        DataTable Dt_Orden;
        if (Hdf_Cuenta_Predial_ID.Value.Length > 0)
        {
            Session["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
            Cls_Cat_Pre_Cuentas_Predial_Negocio Cuenta = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
            Cuenta.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            Cuenta = Cuenta.Consultar_Datos_Propietario();

            Txt_Calle.Text = Cuenta.P_Nombre_Calle;
            Txt_Propietario.Text = Cuenta.P_Nombre_Propietario;
            Txt_Colonia.Text = Cuenta.P_Nombre_Colonia;
            Txt_No_Exterior.Text = Cuenta.P_No_Exterior;
            Txt_No_Interior.Text = Cuenta.P_No_Interior;

            Session["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
            Cls_Ope_Pre_Orden_Variacion_Negocio Orden = new Cls_Ope_Pre_Orden_Variacion_Negocio();
            Orden.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            Dt_Orden = Orden.Consultar_Ordenes_Variacion();
            if (Dt_Orden.Rows.Count == 0)
            {
                return;
            }
            Orden.P_Año = Convert.ToInt32(Dt_Orden.Rows[0][Ope_Pre_Orden_Variacion.Campo_Anio].ToString());
            Orden.P_Orden_Variacion_ID = Dt_Orden.Rows[0][Ope_Pre_Orden_Variacion.Campo_No_Orden_Variacion].ToString();
            Dt_Orden = Orden.Consultar_Domicilio_Y_Propietario();
            if (Dt_Orden.Rows.Count > 0)
            {
                String Dom_Foraneo = "";
                String No_int_not = "";
                String No_ext_not = "";
                String No_Int = "";
                String No_Ext = "";
                String Dom_Not_Colonia = "";
                String Dom_Not_Calle = "";
                String Dom_Colonia = "";
                String Dom_Calle = "";
                foreach (DataRow Renglon_Actual in Dt_Orden.Rows)
                {
                    if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Domicilio_Foraneo].ToString() != "")
                    {
                        Dom_Foraneo = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Domicilio_Foraneo].ToString();
                    }
                    if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_No_Interior_Notificacion].ToString() != "")
                    {
                        No_int_not = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_No_Interior_Notificacion].ToString();
                    }
                    if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_No_Exterior_Notificacion].ToString() != "")
                    {
                        No_ext_not = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_No_Exterior_Notificacion].ToString();
                    }
                    if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_No_Interior].ToString() != "")
                    {
                        No_Int = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_No_Interior].ToString();
                    }
                    if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_No_Exterior].ToString() != "")
                    {
                        No_Ext = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_No_Exterior].ToString();
                    }
                    if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Calle_ID_Notificacion].ToString() != "")
                    {
                        Dom_Not_Calle = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Calle_ID_Notificacion].ToString();
                    }
                    if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Colonia_ID_Notificacion].ToString() != "")
                    {
                        Dom_Not_Colonia = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Colonia_ID_Notificacion].ToString();
                    }
                    if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Calle_Notificacion].ToString() != "")
                    {
                        Dom_Calle = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Calle_Notificacion].ToString();
                    }
                    if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Colonia_Notificacion].ToString() != "")
                    {
                        Dom_Colonia = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Colonia_Notificacion].ToString();
                    }
                }
                if (Dt_Orden.Rows[0]["NOMBRE_CONTRIBUYENTE"].ToString().Trim() != "")
                {
                    Txt_Propietario.Text = Dt_Orden.Rows[0]["NOMBRE_CONTRIBUYENTE"].ToString();
                }
                if (Dom_Foraneo == "SI" && Dom_Foraneo != "")
                {
                    Txt_Calle.Text = Dom_Calle;
                    Txt_Colonia.Text = Dom_Colonia;
                    Txt_No_Exterior.Text = No_ext_not;
                    Txt_No_Interior.Text = No_int_not;
                }
                else if (Dom_Foraneo == "NO" && Dom_Foraneo != "")
                {
                    Cls_Cat_Pre_Calles_Negocio Calle = new Cls_Cat_Pre_Calles_Negocio();
                    Calle.P_Calle_ID = Dom_Not_Calle;
                    Calle.P_Mostrar_Nombre_Calle_Nombre_Colonia = true;
                    if (Calle.P_Calle_ID != "")
                    {
                        DataTable Dt_Calle_Colonia = Calle.Consultar_Nombre_Id_Calles();
                        String[] Calle_Col = Dt_Calle_Colonia.Rows[0]["NOMBRE"].ToString().Split('-');
                        Txt_Calle.Text = Calle_Col[0];
                        Txt_Colonia.Text = Calle_Col[1];
                    }
                    Txt_No_Exterior.Text = No_Ext;
                    Txt_No_Interior.Text = No_Int;
                }
            }
        }
    }

    private void Cargar_Datos()
    {
        try
        {
            if (!String.IsNullOrEmpty(Txt_Cuenta_Predial.Text.Trim()))
            {
                //KONSULTA DATOS CUENTA HACER DS
                Busqueda_Cuentas();
                //LLENAR CAJAS
                if (Session["Ds_Cuenta_Datos"] != null)
                {
                    Cargar_Generales_Cuenta(((DataSet)Session["Ds_Cuenta_Datos"]).Tables["Dt_Generales"]);
                    Busqueda_Propietario();
                }

            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }

    Cls_Ope_Pre_Orden_Variacion_Negocio M_Orden_Negocio;

    private void Busqueda_Cuentas()
    {
        Cls_Ope_Pre_Resumen_Predio_Negocio Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        DataSet Ds_Cuenta;
        try
        {
            Resumen_Predio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            Ds_Cuenta = Resumen_Predio.Consulta_Datos_Cuenta_Generales();
            if (Ds_Cuenta.Tables[0].Rows.Count - 1 >= 0)
            {
                if (Ds_Cuenta.Tables[0].Rows[0]["Cuenta_Predial_Id"].ToString().Trim() != string.Empty)
                {
                    Session["Cuenta_Predial_ID"] = Ds_Cuenta.Tables[0].Rows[0]["Cuenta_Predial_Id"].ToString().Trim();
                }
            }
            if (Ds_Cuenta.Tables[0].Rows.Count > 0)
            {
                Session.Remove("Ds_Cuenta_Datos");
                M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
                Session["Ds_Cuenta_Datos"] = Ds_Cuenta;
            }
            else
            {
                //Mensaje_Error("No se encontraron los datos necesarios para la consulta de la cuenta");
                //Lbl_Mensaje_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            //Mensaje_Error(Ex.Message);
        }

    }


    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Generales_Cuenta
    ///DESCRIPCIÓN: asignar datos generales de cuenta a los controles y objeto de negocio
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/Ago/2011 06:31:08 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    private void Cargar_Generales_Cuenta(DataTable dataTable)
    {
        Cls_Ope_Pre_Resumen_Predio_Negocio Rs_Consulta_Ope_Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio(); //Variable de conexión hacia la capa de Negocios
        try
        {
            Txt_Cuenta_Predial.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial].ToString();
            Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial_ID = dataTable.Rows[0]["Cuenta_Predial_ID"].ToString().Trim();
            DataTable Dt_Ultimo_Movimiento = Rs_Consulta_Ope_Resumen_Predio.Consultar_Ultimo_Movimiento();
            if (Dt_Ultimo_Movimiento.Rows.Count > 0)
            {
                if (Dt_Ultimo_Movimiento.Rows[0]["Identificador"].ToString() != string.Empty)
                {
                    if (Dt_Ultimo_Movimiento.Rows[0]["descripcion"].ToString() != "APERTURA")
                    {
                        //Txt_Ultimo_Movimiento_General.Text = Dt_Ultimo_Movimiento.Rows[0]["Identificador"].ToString().Trim();
                    }
                }
            }
            if (dataTable.Rows[0]["Estatus"].ToString().Trim() == "BLOQUEADA")
            {
                //Lbl_Estatus.Text = " BLOQUEADA" + " " + Lbl_Estatus.Text;
            }
            if (dataTable.Rows[0]["Estatus"].ToString().Trim() == "PENDIENTE")
            {
                //Lbl_Estatus.Text = " Cuenta No Generada";
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Resumen de Predio", "alert('Cuenta No Generada')", true);

                //Bloquear_Controles();
            }
            //Txt_Cuenta_Origen.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen].ToString();
            //M_Orden_Negocio.P_Cuenta_Origen = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen].ToString();
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID].ToString() != string.Empty)
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Tipo_Predio = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID].ToString();
                DataTable Dt_Tipo_Predio = Rs_Consulta_Ope_Resumen_Predio.Consultar_Tipo_Predio();
                //Txt_Tipo_Periodo_Impuestos.Text = Dt_Tipo_Predio.Rows[0]["Descripcion"].ToString().Trim();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID].ToString() != string.Empty)
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Uso_Suelo_ID = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID].ToString();
                DataTable Dt_Uso_Suelo = Rs_Consulta_Ope_Resumen_Predio.Consultar_Uso_Predio();
                //Txt_Uso_Predio_General.Text = Dt_Uso_Suelo.Rows[0]["Descripcion"].ToString();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID].ToString() != string.Empty)
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Estado_Predio = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID].ToString();
                DataTable Dt_Estado_Predio = Rs_Consulta_Ope_Resumen_Predio.Consultar_Estado_Predio();
                //Txt_Estado_Predio_General.Text = Dt_Estado_Predio.Rows[0]["Descripcion"].ToString();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString() != string.Empty)
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Calle_ID = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
                DataTable Dt_Calles = Rs_Consulta_Ope_Resumen_Predio.Consultar_Calle_Generales();
                Txt_Calle.Text = Dt_Calles.Rows[0]["Nombre"].ToString();
                Rs_Consulta_Ope_Resumen_Predio.P_Colonia_ID = Dt_Calles.Rows[0]["Colonia_ID"].ToString();
                DataTable Dt_Colonia = Rs_Consulta_Ope_Resumen_Predio.Consultar_Colonia_Generales();
                Txt_Colonia.Text = Dt_Colonia.Rows[0]["Nombre"].ToString();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString() != string.Empty)
            {
                Txt_No_Exterior.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString() != string.Empty)
            {
                Txt_No_Interior.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString();
            }
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString() != string.Empty)
            //{
            //    //Txt_Estatus_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString();
            //    //M_Orden_Negocio.P_Estatus_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString();
            //}

            ////Txt_Supe_Construida_General.Text = dataTable.Rows[0]["Superficie_Construida"].ToString();
            //M_Orden_Negocio.P_Superficie_Construida = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida].ToString();
            ////Txt_Super_Total_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Total].ToString();
            //M_Orden_Negocio.P_Superficie_Total = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Total].ToString();
            //if (dataTable.Rows[0][Cat_Pre_Calles.Campo_Colonia_ID].ToString() != "")
            //{
            //    Rs_Consulta_Ope_Resumen_Predio.P_Colonia_ID = dataTable.Rows[0][Cat_Pre_Calles.Campo_Colonia_ID].ToString();
            //    DataTable Dt_Colonia = Rs_Consulta_Ope_Resumen_Predio.Consultar_Colonia_Generales();
            //    //Txt_Calle.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
            //    M_Orden_Negocio.P_Ubicacion_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
            //}
            ////Txt_Numero_Exterior_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString();
            //M_Orden_Negocio.P_Exterior_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString();
            ////Txt_Numero_Interior_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString();
            //M_Orden_Negocio.P_Interior_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString();
            ////Txt_Clave_Catastral_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral].ToString();
            //M_Orden_Negocio.P_Clave_Catastral = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral].ToString();
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos].ToString() != string.Empty)
            //{
            //    //Txt_Efectos_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos].ToString();
            //}
            ////Txt_Valor_Fiscal_Impuestos.Text = "$ " + String.Format("{0:#,###,###.00}", Convert.ToDecimal(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal].ToString()));
            //M_Orden_Negocio.P_Valor_Fiscal = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal].ToString();
            ////Txt_Periodo_Corriente_Impuestos.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente].ToString();
            //M_Orden_Negocio.P_Periodo_Corriente_Inicial = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente].ToString();
            ////Txt_Periodo_Corriente_Impuestos.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente].ToString();
            //M_Orden_Negocio.P_Periodo_Corriente_Inicial = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente].ToString();
            //if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString()))
            //{
            //    //Txt_Cuota_Anual_Impuestos.Text = "$ " + String.Format("{0:#,###,###.00}", Convert.ToDecimal(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString()));
            //    //double Cuota_Bimestral = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString()) / 6;
            //    //Txt_Cuota_Bimestral_Impuestos.Text = "$ " + String.Format("{0:#,###,###.00}", Cuota_Bimestral);
            //}
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual] != null)
            //{
            //    M_Orden_Negocio.P_Cuota_Anual = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString());
            //    M_Orden_Negocio.P_Cuota_Bimestral = ((Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString())) / 6);
            //}
            ////Txt_Porciento_Exencion_Impuestos.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion].ToString();
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion] != null)
            //{
            //    M_Orden_Negocio.P_Exencion = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion].ToString());
            //}
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo] != null)
            //{
            //    if (dataTable.Rows[0]["Fecha_Avaluo"].ToString().Trim() == "01/01/0001 12:00:00 a.m.")
            //    {
            //        //Txt_Fecha_Avaluo_Impuestos.Text = "";
            //        M_Orden_Negocio.P_Fecha_Avaluo = Convert.ToDateTime(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo].ToString());
            //    }
            //    else
            //    {
            //        //Txt_Fecha_Avaluo_Impuestos.Text = String.Format("{0:dd/MMM/yyyy}", dataTable.Rows[0]["Fecha_Avaluo"].ToString().Trim());
            //        M_Orden_Negocio.P_Fecha_Avaluo = Convert.ToDateTime(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo].ToString());
            //    }
            //}
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion] != null)
            //{
            //    if (dataTable.Rows[0]["Termino_Exencion"].ToString().Trim() == "01/01/0001 12:00:00 a.m.")
            //    {
            //        //Txt_Fecha_Termino_Extencion.Text = "";
            //        M_Orden_Negocio.P_Fecha_Termina_Exencion = Convert.ToDateTime(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion].ToString());
            //    }
            //    else
            //    {
            //        //Txt_Fecha_Termino_Extencion.Text = String.Format("{0:dd/MMM/yyyy}", dataTable.Rows[0]["Termino_Exencion"].ToString().Trim());
            //        M_Orden_Negocio.P_Fecha_Termina_Exencion = Convert.ToDateTime(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion].ToString());
            //    }

            //}
            ////Txt_Dif_Construccion_Impuestos.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion].ToString();
            //M_Orden_Negocio.P_Diferencia_Construccion = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion].ToString();

            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID] != null)
            //{
            //    M_Orden_Negocio.P_Cuota_Minima = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID].ToString());
            //    //Cmb_Cuota_Minima.SelectedValue = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID].ToString();
            //}
            ////Z1 HAY KU07A FIJ4!!! Seccion de carga de datos de la cuota fija
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija] != null)
            //{
            //    if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "NO" || dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "no" || dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "No")
            //    {
            //        //Chk_Cuota_Fija.Checked = false;
            //        M_Orden_Negocio.P_Cuota_Fija = "NO";
            //    }
            //    if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "SI" || dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "si" || dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "Si")
            //    {
            //        //Chk_Cuota_Fija.Checked = true;
            //        M_Orden_Negocio.P_Cuota_Fija = "SI";

            //        //----K4RG4R D47OZ D3 14 CU0T4 F1J4!!!!!
            //        if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija].ToString() != "")
            //        {
            //            M_Orden_Negocio.P_No_Cuota_Fija = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija].ToString();
            //            //Cargar_Datos_Cuota_Fija(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija].ToString());
            //        }
            //    }
            //}
            //if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID].ToString()))
            //{
            //    M_Orden_Negocio.P_Tasa = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID].ToString());
            //    //Hdn_Tasa_ID.Value = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID].ToString();
            //}

            //if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_ID].ToString()))
            //{
            //    Rs_Consulta_Ope_Resumen_Predio.P_Tasa_Predial_ID = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_ID].ToString();
            //    DataTable Dt_Tasa = Rs_Consulta_Ope_Resumen_Predio.Consultar_Tasa();
            //    if (Dt_Tasa.Rows.Count > 0)
            //    {
            //        //Txt_Tasa_Impuestos.Text = Dt_Tasa.Rows[0]["Descripcion"].ToString();
            //    }
            //}
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo].ToString() != String.Empty)
            //{
            //    //Cmb_Domicilio_Foraneo.SelectedValue = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo].ToString();
            //    M_Orden_Negocio.P_Domicilio_Foraneo = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo].ToString();
            //}
            //if (!String.IsNullOrEmpty(dataTable.Rows[0]["Estado_ID_Notificacion"].ToString()))
            //{
            //    Rs_Consulta_Ope_Resumen_Predio.P_Estado_Predio = (dataTable.Rows[0]["Estado_ID_Notificacion"].ToString());
            //    DataTable Dt_Estado_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Estado_Predio_Propietario();
            //    if (Dt_Estado_Propietario.Rows.Count > 0)
            //    {
            //        //Txt_Estado_Propietario.Text = (Dt_Estado_Propietario.Rows[0]["Descripcion"].ToString());
            //        M_Orden_Negocio.P_Estado_Propietario = (Dt_Estado_Propietario.Rows[0]["Descripcion"].ToString());
            //    }
            //}
            //else if (!String.IsNullOrEmpty(dataTable.Rows[0]["Estado_Notificacion"].ToString()))
            //{
            //    //Txt_Estado_Propietario.Text = dataTable.Rows[0]["Estado_Notificacion"].ToString();
            //}
            //if (!String.IsNullOrEmpty(dataTable.Rows[0]["Ciudad_ID_Notificacion"].ToString()))
            //{
            //    Rs_Consulta_Ope_Resumen_Predio.P_Ciudad_ID = dataTable.Rows[0]["Ciudad_ID_Notificacion"].ToString();
            //    DataTable Dt_Ciudad_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Ciudad();
            //    //Txt_Ciudad_Propietario.Text = Dt_Ciudad_Propietario.Rows[0]["Nombre"].ToString();
            //    M_Orden_Negocio.P_Ciudad_Propietario = dataTable.Rows[0]["Ciudad_ID_Notificacion"].ToString();
            //}
            //else if (!String.IsNullOrEmpty(dataTable.Rows[0]["Ciudad_Notificacion"].ToString()))
            //{
            //    //Txt_Ciudad_Propietario.Text = dataTable.Rows[0]["Ciudad_Notificacion"].ToString();
            //}
            //if (dataTable.Rows[0]["Domicilio_Foraneo"].ToString().Trim() == "SI")
            //{
            //    if (!String.IsNullOrEmpty(dataTable.Rows[0]["Colonia_Notificacion"].ToString()))
            //    {
            //        //Txt_Colonia_Propietario.Text = dataTable.Rows[0]["Colonia_Notificacion"].ToString();
            //    }
            //    if (!String.IsNullOrEmpty(dataTable.Rows[0]["Calle_Notificacion"].ToString()))
            //    {
            //        //Txt_Calle_Propietario.Text = dataTable.Rows[0]["Calle_Notificacion"].ToString();
            //    }
            //}
            //else
            //{

            //    if (!String.IsNullOrEmpty(dataTable.Rows[0]["Colonia_ID_Notificacion"].ToString()))
            //    {
            //        Rs_Consulta_Ope_Resumen_Predio.P_Colonia_ID = dataTable.Rows[0]["Colonia_ID_Notificacion"].ToString();
            //        DataTable DT_Colonia_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Colonia_Generales();
            //        //Txt_Colonia_Propietario.Text = DT_Colonia_Propietario.Rows[0]["Nombre"].ToString();
            //    }

            //    if (!String.IsNullOrEmpty(dataTable.Rows[0]["Calle_ID_Notificacion"].ToString()))
            //    {
            //        Rs_Consulta_Ope_Resumen_Predio.P_Calle_ID = dataTable.Rows[0]["Calle_ID_Notificacion"].ToString();//*
            //        DataTable Dt_Calle_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Calle_Generales();
            //        //Txt_Calle_Propietario.Text = Dt_Calle_Propietario.Rows[0]["Nombre"].ToString();
            //    }
            //}

            ////Txt_Numero_Exterior_Propietario.Text = dataTable.Rows[0]["No_Exterior_Notificacion"].ToString();
            //M_Orden_Negocio.P_Exterior_Propietario = dataTable.Rows[0]["No_Exterior_Notificacion"].ToString();
            ////Txt_Numero_Interior_Propietario.Text = dataTable.Rows[0]["No_Interior_Notificacion"].ToString();
            //M_Orden_Negocio.P_Interior_Propietario = dataTable.Rows[0]["No_Interior_Notificacion"].ToString();
            ////Txt_Cod_Postal_Propietario.Text = dataTable.Rows[0]["Codigo_Postal"].ToString();
            //M_Orden_Negocio.P_CP_Propietario = dataTable.Rows[0]["Codigo_Postal"].ToString();


            //M_Orden_Negocio.P_Cuenta_Predial_ID = dataTable.Rows[0]["Cuenta_Predial_ID"].ToString().Trim();
            //M_Orden_Negocio.P_Dt_Copropietarios = M_Orden_Negocio.Consulta_Co_Propietarios();
            ////Dt_Agregar_Co_Propietarios = M_Orden_Negocio.P_Dt_Copropietarios;
            ////if (Dt_Agregar_Co_Propietarios.Rows.Count - 1 >= 0)
            ////{
            ////    for (int x = 0; x <= Dt_Agregar_Co_Propietarios.Rows.Count - 1; x++)
            ////    {
            ////        if (Dt_Agregar_Co_Propietarios.Rows[0]["Tipo"].ToString().Trim() == "COPROPIETARIO")
            ////        {
            ////            Txt_Copropietarios_Propietario.Text += Dt_Agregar_Co_Propietarios.Rows[x]["Nombre_Contribuyente"].ToString().Trim() + " \t" + Dt_Agregar_Co_Propietarios.Rows[x]["Rfc"].ToString().Trim() + "\n";

            ////        }
            ////    }
            ////}
        }
        catch (Exception Ex)
        {
            throw new Exception("Cargar_Datos_Cuenta: " + Ex.Message);
        }
    }


    private void Busqueda_Propietario()
    {
        DataSet Ds_Prop;
        String Cuenta_Predial_ID = Session["Cuenta_Predial_ID"].ToString().Trim();
        try
        {
            M_Orden_Negocio.P_Cuenta_Predial_ID = Cuenta_Predial_ID;
            Ds_Prop = M_Orden_Negocio.Consulta_Datos_Propietario();
            if (Ds_Prop.Tables[0].Rows.Count > 0)
            {
                Session.Remove("Ds_Prop_Datos");
                Session["Ds_Prop_Datos"] = Ds_Prop;
                //Cargar_Datos_Propietario(((DataSet)Session["Ds_Prop_Datos"]).Tables["Dt_Propietarios"]);
            }
        }
        catch (Exception Ex)
        {
            //Mensaje_Error(Ex.Message);
        }

    }


    private void Llenar_Costos(DataTable Dt_Costos)
    {
        if (Dt_Costos.Rows.Count > 0)
        {
            Txt_Monto_Impuesto_Der_Sup.Text = "$ " + Convert.ToDouble(Dt_Costos.Rows[0]["IMPORTE"].ToString()).ToString("#,###,###,###,###,###,###,##0.00");
            if (Dt_Costos.Rows[0]["MONTO"].ToString().Equals(""))
            {
                Txt_Monto_Multas.Text = "$ 0.00";
            }
            else
            {
                Txt_Monto_Multas.Text = "$ " + Convert.ToDouble(Dt_Costos.Rows[0]["MONTO"].ToString()).ToString("#,###,###,###,###,###,###,##0.00");
            }
            Txt_Monto_Recargos.Text = "$ " + Convert.ToDouble(Dt_Costos.Rows[0]["RECARGOS"].ToString()).ToString("#,###,###,###,###,###,###,##0.00");
            Txt_Porcentaje_Desc_Multas.Text = "0.00";
            Txt_Prcentaje_Desc_Recargos.Text = "0.00";
            Txt_Monto_Desc_Multas.Text = "0.00";
            Txt_Monto_Desc_Recargo.Text = "0.00";
            Calcular_Total_A_Pagar();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Descuentos de Derechos de Supervisión", "alert('Impuesto sin adeudos.')", true);
            Btn_Salir_Click(null, null);
        }
    }

    #endregion

    #region Eventos botones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Nuevo_Click
    ///DESCRIPCIÓN          : Deja los componentes listos para capturar el nuevo descuento.
    ///PARAMETROS:     
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 20/Diciembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
        {
            Cls_Ope_Pre_Dias_Inhabiles_Negocio Calcular_Dias_Inhabilies = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();
            DateTime Fecha_Vencimiento;

            Configuracion_Formulario(true);
            Limpiar_Componentes();
            Btn_Nuevo.AlternateText = "Dar de Alta";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
            Btn_Salir.AlternateText = "Cancelar";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
            Btn_Modificar.Visible = false;
            Btn_Imprimir.Visible = false;
            Cmb_Estatus.SelectedIndex = 1;
            Cmb_Estatus.Enabled = false;
            Txt_Fecha_Inicial.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Fecha_Vencimiento = Calcular_Dias_Inhabilies.Calcular_Fecha(DateTime.Now.ToShortDateString(), "1");
            Txt_Fecha_Vencimiento.Text = Fecha_Vencimiento.ToString("dd/MMM/yyyy");
            Txt_Realizo.Text = Cls_Sessiones.Nombre_Empleado;
            Btn_Busqueda_Avanzada_Impuestos.Enabled = true;
            Txt_Monto_Desc_Recargo.Enabled = false;
            Txt_Monto_Desc_Multas.Enabled = false;
            Txt_Porcentaje_Desc_Multas.Enabled = false;
            Txt_Prcentaje_Desc_Recargos.Enabled = false;
            Grid_Descuentos_Der_Sup.SelectedIndex = -1;
            Div_Grid_Descuentos.Visible = false;
            Div_Datos.Visible = true;
        }
        else
        {
            if (Validar_Campos())
            {
                try
                {
                    Cls_Ope_Pre_Descuentos_Fracc_Negocio Descuento = new Cls_Ope_Pre_Descuentos_Fracc_Negocio();
                    Descuento.P_Cuenta_Predial_Id = Hdf_Cuenta_Predial_ID.Value;
                    Descuento.P_No_Impuesto_Fracc = Hdf_No_Impuesto_Der_Sup.Value;
                    Descuento.P_Referencia = Obtener_Dato_Consulta(Hdf_No_Impuesto_Der_Sup.Value);
                    Descuento.P_Estatus = Cmb_Estatus.SelectedValue;
                    Descuento.P_Fecha = Convert.ToDateTime(Txt_Fecha_Inicial.Text);
                    Descuento.P_Fecha_Vencimiento = Convert.ToDateTime(Txt_Fecha_Vencimiento.Text);
                    Descuento.P_Desc_Multa = Convert.ToDouble(Txt_Porcentaje_Desc_Multas.Text);
                    Descuento.P_Desc_Recargo = Convert.ToDouble(Txt_Prcentaje_Desc_Recargos.Text);
                    Descuento.P_Total_Por_Pagar = Convert.ToDouble(Txt_Total_Por_Pagar.Text.Replace("$", ""));
                    Descuento.P_Realizo = Txt_Realizo.Text;
                    Descuento.P_Observaciones = Txt_Observaciones.Text.ToUpper();
                    Descuento.P_Monto_Multas = Convert.ToDouble(Txt_Monto_Desc_Multas.Text);
                    Descuento.P_Monto_Recargos = Convert.ToDouble(Txt_Monto_Desc_Recargo.Text);
                    Descuento.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
                    Descuento.Alta_Descuento();
                    DataTable Dt_Impuesto;
                    Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio Impuesto = new Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio();
                    Impuesto.P_No_Impuesto_Fraccionamiento = Hdf_No_Impuesto_Der_Sup.Value;
                    Dt_Impuesto = Impuesto.Consultar_Impuestos_Fraccionamiento();
                    Insertar_Pasivo("IMP" + Convert.ToDateTime(Dt_Impuesto.Rows[0][Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Creo].ToString()).ToString("yy") + Convert.ToInt32(Hdf_No_Impuesto_Der_Sup.Value), Descuento);
                    Imprimir_Reporte(Crear_Ds_Descuentos(), "Rpt_Ope_Pre_Descuentos_Fracc.rpt", "Descuento_Fraccionamiento");
                    Configuracion_Formulario(false);
                    Limpiar_Componentes();
                    Cargar_Grid_Descuentos_Derechos_Supervision(Grid_Descuentos_Der_Sup.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Descuentos a Impuestos de Fraccionamiento", "alert('Alta de Descuento Exitosa');", true);
                    Btn_Nuevo.AlternateText = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Imprimir.Visible = true;
                    Btn_Modificar.Visible = true;
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Div_Grid_Descuentos.Visible = true;
                    Div_Datos.Visible = false;
                    Cargar_Grid_Impuestos_Derechos_Supervision(0);
                }
                catch (Exception Exc)
                {
                    Lbl_Ecabezado_Mensaje.Text = Exc.Message;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Visible = true;
                    Lbl_Ecabezado_Mensaje.Visible = true;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Modificar_Click
    ///DESCRIPCIÓN          : Deja los componentes listos para modificación de un descuento.
    ///PARAMETROS:     
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 20/Diciembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        if (Grid_Descuentos_Der_Sup.SelectedIndex > -1)
        {
            if (!Grid_Descuentos_Der_Sup.SelectedRow.Cells[10].Text.Equals("CANCELADO"))
            {
                if (!Convenio_Con_Descuento())
                {
                    if (Btn_Modificar.AlternateText.Equals("Modificar"))
                    {
                        Btn_Modificar.AlternateText = "Actualizar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                        Btn_Salir.AlternateText = "Cancelar";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        Btn_Nuevo.Visible = false;
                        Btn_Imprimir.Visible = false;
                        Configuracion_Formulario(true);
                    }
                    else
                    {
                        if (Validar_Campos())
                        {
                            try
                            {
                                Cls_Ope_Pre_Descuentos_Fracc_Negocio Descuento = new Cls_Ope_Pre_Descuentos_Fracc_Negocio();
                                Descuento.P_No_Descuento = Hdf_No_Descuento.Value;
                                Descuento.P_Cuenta_Predial_Id = Hdf_Cuenta_Predial_ID.Value;
                                Descuento.P_No_Impuesto_Fracc = Hdf_No_Impuesto_Der_Sup.Value;
                                Descuento.P_Referencia = Obtener_Dato_Consulta(Hdf_No_Impuesto_Der_Sup.Value);
                                Descuento.P_Estatus = Cmb_Estatus.SelectedValue;
                                Descuento.P_Fecha = Convert.ToDateTime(Txt_Fecha_Inicial.Text);
                                Descuento.P_Fecha_Vencimiento = Convert.ToDateTime(Txt_Fecha_Vencimiento.Text);
                                Descuento.P_Desc_Multa = Convert.ToDouble(Txt_Porcentaje_Desc_Multas.Text);
                                Descuento.P_Desc_Recargo = Convert.ToDouble(Txt_Prcentaje_Desc_Recargos.Text);
                                Descuento.P_Total_Por_Pagar = Convert.ToDouble(Txt_Total_Por_Pagar.Text.Replace("$", ""));
                                Descuento.P_Realizo = Txt_Realizo.Text;
                                Descuento.P_Observaciones = Txt_Observaciones.Text.ToUpper();
                                Descuento.P_Monto_Multas = Convert.ToDouble(Txt_Monto_Desc_Multas.Text);
                                Descuento.P_Monto_Recargos = Convert.ToDouble(Txt_Monto_Desc_Recargo.Text);
                                Descuento.P_Usuario_Modifico = Cls_Sessiones.Nombre_Empleado;
                                Descuento.Modificar_Descuentos();
                                DataTable Dt_Impuesto;
                                Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio Impuesto = new Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio();
                                Impuesto.P_No_Impuesto_Fraccionamiento = Hdf_No_Impuesto_Der_Sup.Value;
                                Dt_Impuesto = Impuesto.Consultar_Impuestos_Fraccionamiento();
                                Insertar_Pasivo("IMP" + Convert.ToDateTime(Dt_Impuesto.Rows[0][Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Creo].ToString()).ToString("yy") + Convert.ToInt32(Hdf_No_Impuesto_Der_Sup.Value), Descuento);
                                if (Cmb_Estatus.SelectedValue == "VIGENTE")
                                {
                                    Insertar_Pasivo("IMP" + Convert.ToDateTime(Dt_Impuesto.Rows[0][Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Creo].ToString()).ToString("yy") + Convert.ToInt32(Hdf_No_Impuesto_Der_Sup.Value), Descuento);
                                }
                                else
                                {
                                    Eliminar_Pasivo("IMP" + Convert.ToDateTime(Dt_Impuesto.Rows[0][Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Creo].ToString()).ToString("yy") + Convert.ToInt32(Hdf_No_Impuesto_Der_Sup.Value));
                                }
                                if (Cmb_Estatus.SelectedValue == "VIGENTE")
                                {
                                    Imprimir_Reporte(Crear_Ds_Descuentos(), "Rpt_Ope_Pre_Descuentos_Fracc.rpt", "Descuento_Fraccionamiento");
                                }
                                Configuracion_Formulario(false);
                                Limpiar_Componentes();
                                Cargar_Grid_Descuentos_Derechos_Supervision(Grid_Descuentos_Der_Sup.PageIndex);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Descuentos a Impuestos de Fraccionamiento", "alert('Actualización de Descuento Exitosa');", true);
                                Btn_Modificar.AlternateText = "Modificar";
                                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                                Btn_Salir.AlternateText = "Salir";
                                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                                Btn_Imprimir.Visible = true;
                                Btn_Nuevo.Visible = true;
                                Div_Grid_Descuentos.Visible = true;
                                Div_Datos.Visible = false;
                                Grid_Descuentos_Der_Sup.SelectedIndex = -1;
                                Cargar_Grid_Impuestos_Derechos_Supervision(0);
                            }
                            catch (Exception Exc)
                            {
                                Lbl_Ecabezado_Mensaje.Text = Exc.Message;
                                Img_Error.Visible = true;
                                Lbl_Mensaje_Error.Visible = true;
                                Lbl_Ecabezado_Mensaje.Visible = true;
                                Lbl_Mensaje_Error.Text = "";
                                Div_Contenedor_Msj_Error.Visible = true;
                            }
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Descuentos de Derechos de Supervisión", "alert('Descuento aplicado aún vigente.');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Descuentos de Derechos de Supervisión", "alert('El descuento se encuentra CANCELADO.');", true);
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Imprimir_Click
    ///DESCRIPCIÓN          : Manda imprimir el descuento
    ///PARAMETROS:     
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 20/Diciembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        if (Grid_Descuentos_Der_Sup.SelectedIndex > -1)
        {
            Imprimir_Reporte(Crear_Ds_Descuentos(), "Rpt_Ope_Pre_Descuentos_Fracc.rpt", "Descuento_Fraccionamiento");
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Salir_Click
    ///DESCRIPCIÓN          : regresa los componentes a su estado inicial o redirecciona a la página principal.
    ///PARAMETROS:     
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 20/Diciembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText.Equals("Salir"))
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Btn_Nuevo.Visible = true;
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            Btn_Modificar.Visible = true;
            Btn_Modificar.AlternateText = "Modificar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            Btn_Imprimir.Visible = true;
            Configuracion_Formulario(false);
            Limpiar_Componentes();
            Cargar_Grid_Impuestos_Derechos_Supervision(0);
            Cargar_Grid_Descuentos_Derechos_Supervision(0);
            Grid_Impuestos_Derechos_Supervision.SelectedIndex = -1;
            Grid_Descuentos_Der_Sup.SelectedIndex = -1;
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Btn_Salir.AlternateText = "Salir";
            Div_Grid_Descuentos.Visible = true;
            Div_Datos.Visible = false; ;
        }
    }

    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Limpiar_Componentes();
            Grid_Descuentos_Der_Sup.SelectedIndex = (-1);
            Cargar_Grid_Descuentos_Derechos_Supervision(0);
            if (Grid_Impuestos_Derechos_Supervision.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0)
            {
                Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda de \"" + Txt_Busqueda.Text + "\" no se encotrarón coincidencias";
                Lbl_Mensaje_Error.Text = "";
                //Lbl_Mensaje_Error.Text = "(Se cargarón todos los Impuestos de Derechos de Supervisión encontrados)";
                Div_Contenedor_Msj_Error.Visible = true;
                Txt_Busqueda.Text = "";
                //Llenar_Tabla_Detalles_Impuestos_Derechos_Supervision(0);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    protected void Btn_Cerrar_Ventana_Click(object sender, EventArgs e)
    {
        Mpe_Busqueda_Empleados.Hide();
    }

    #endregion

    #region Eventos Cajas de Texto

    protected void Txt_Porcentaje_Desc_Recargos_Text_Changed(object sender, EventArgs e)
    {
        if (Txt_Porcentaje_Desc_Multas.Text == "")
        {
            Txt_Prcentaje_Desc_Recargos.Text = "0.00";
            Txt_Monto_Desc_Recargo.Text = "0.00";
        }
        else
        {
            try
            {
                Double Porc_Desc_Recargo = 0;
                Double Monto_Desc_Recargo = 0;
                Double Monto_Recargos = 0;
                Monto_Recargos = Convert.ToDouble(Convert.ToDouble(Txt_Monto_Recargos.Text.Replace("$", "")).ToString("#,###,###,###,###,###,###,##0.00"));
                Porc_Desc_Recargo = Convert.ToDouble(Convert.ToDouble(Convert.ToDouble(Txt_Prcentaje_Desc_Recargos.Text)).ToString("#,###,###,###,###,###,###,###0.00"));
                Monto_Desc_Recargo = (Porc_Desc_Recargo / 100) * Monto_Recargos;
                Txt_Prcentaje_Desc_Recargos.Text = Porc_Desc_Recargo.ToString("##0.00");
                Txt_Monto_Desc_Recargo.Text = Monto_Desc_Recargo.ToString("#,###,###,###,###,###,###,##0.00");
                Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio Descuento = new Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio();
                Descuento.P_Empleado_Id = Cls_Sessiones.Empleado_ID;
                DataTable Dt_Desc = Descuento.Consultar_Rangos_De_Descuento_Por_Rol_Completo();
                Boolean Entro = false;
                foreach (DataRow Dr_Desc in Dt_Desc.Rows)
                {
                    if (Dr_Desc[Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Tipo].ToString().Equals("FRACC"))
                    {
                        if (Convert.ToDouble(Txt_Prcentaje_Desc_Recargos.Text) > Convert.ToDouble(Dr_Desc[Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Porcentaje].ToString()))
                        {
                            Txt_Prcentaje_Desc_Recargos.Text = Convert.ToDouble(Dr_Desc[Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Porcentaje].ToString()).ToString("##0.00");
                            Txt_Monto_Desc_Recargo.Text = (Monto_Recargos * Convert.ToDouble(Dr_Desc[Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Porcentaje].ToString()) / 100).ToString("#,###,###,###,###,###,###,##0.00");
                            Entro = true;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Descuento a Impuesto de Fraccionamiento", "alert('El descuento máximo a otorgar en recargos es del: " + Dr_Desc[Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Porcentaje].ToString() + "%.')", true);
                        }
                        else
                        {
                            Calcular_Total_A_Pagar();
                            return;
                        }
                    }
                }
                if (Dt_Desc.Rows.Count == 0 || !Entro)
                {
                    Txt_Prcentaje_Desc_Recargos.Text = "0.00";
                    Txt_Monto_Desc_Recargo.Text = "0.00";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Descuento a Impuesto de Fraccionamiento", "alert('No tiene privilegios para dar descuentos en recargos.')", true);
                }
            }
            catch
            {
                Txt_Prcentaje_Desc_Recargos.Text = "0.00";
                Txt_Monto_Desc_Recargo.Text = "0.00";
            }
            Calcular_Total_A_Pagar();
        }
    }

    protected void Calcular_Total_A_Pagar()
    {
        Double Total_Impuesto;
        Double Total_Recargos;
        Double Total_Multas;
        Double Desc_Recargos;
        Double Desc_Multas;
        Total_Impuesto = Convert.ToDouble(Txt_Monto_Impuesto_Der_Sup.Text.Replace("$", ""));
        Total_Recargos = Convert.ToDouble(Txt_Monto_Recargos.Text.Replace("$", ""));
        Total_Multas = Convert.ToDouble(Txt_Monto_Multas.Text.Replace("$", ""));
        Desc_Recargos = Convert.ToDouble(Txt_Monto_Desc_Recargo.Text);
        Desc_Multas = Convert.ToDouble(Txt_Monto_Desc_Multas.Text);
        Txt_Total_Por_Pagar.Text = "$ " + (Total_Impuesto + Total_Recargos + Total_Multas - Desc_Recargos - Desc_Multas).ToString("#,###,###,###,###,###,###,##0.00");
    }

    protected void Txt_Porcentaje_Desc_Multas_Text_Changed(object sender, EventArgs e)
    {
        if (Txt_Porcentaje_Desc_Multas.Text == "")
        {
            Txt_Porcentaje_Desc_Multas.Text = "0.00";
            Txt_Monto_Desc_Multas.Text = "0.00";
        }
        else
        {
            try
            {
                Double Porc_Desc_Multas = 0;
                Double Monto_Desc_Multas = 0;
                Double Monto_Multas = 0;
                Monto_Multas = Convert.ToDouble(Convert.ToDouble(Txt_Monto_Multas.Text.Replace("$", "")).ToString("#,###,###,###,###,###,###,##0.00"));
                Porc_Desc_Multas = Convert.ToDouble(Convert.ToDouble(Convert.ToDouble(Txt_Porcentaje_Desc_Multas.Text)).ToString("#,###,###,###,###,###,###,###0.00"));
                Monto_Desc_Multas = (Porc_Desc_Multas / 100) * Monto_Multas;
                Txt_Porcentaje_Desc_Multas.Text = Porc_Desc_Multas.ToString("##0.00");
                Txt_Monto_Desc_Multas.Text = Monto_Desc_Multas.ToString("#,###,###,###,###,###,###,##0.00");
                Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio Descuento = new Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio();
                Descuento.P_Empleado_Id = Cls_Sessiones.Empleado_ID;
                DataTable Dt_Desc = Descuento.Consultar_Rangos_De_Descuento_Por_Rol_Completo();
                Boolean Entro = false;
                foreach (DataRow Dr_Desc in Dt_Desc.Rows)
                {
                    if (Dr_Desc[Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Tipo].ToString().Equals("MULTAS"))
                    {
                        if (Convert.ToDouble(Txt_Porcentaje_Desc_Multas.Text) > Convert.ToDouble(Dr_Desc[Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Porcentaje].ToString()))
                        {
                            Txt_Porcentaje_Desc_Multas.Text = Convert.ToDouble(Dr_Desc[Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Porcentaje].ToString()).ToString("##0.00");
                            Txt_Monto_Desc_Multas.Text = (Monto_Multas * Convert.ToDouble(Dr_Desc[Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Porcentaje].ToString()) / 100).ToString("#,###,###,###,###,###,###,##0.00");
                            Entro = true;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Descuento a Impuesto de Fraccionamiento", "alert('El descuento máximo a otorgar en multas es del: " + Dr_Desc[Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Porcentaje].ToString() + "%.')", true);
                        }
                        else
                        {
                            Calcular_Total_A_Pagar();
                            return;
                        }
                    }
                }
                if (Dt_Desc.Rows.Count == 0 || !Entro)
                {
                    Txt_Monto_Desc_Multas.Text = "0.00";
                    Txt_Porcentaje_Desc_Multas.Text = "0.00";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Descuento a Impuesto de Fraccionamiento", "alert('No tiene privilegios para dar descuentos en multas.')", true);
                }
            }
            catch
            {
                Txt_Porcentaje_Desc_Multas.Text = "0.00";
                Txt_Monto_Desc_Multas.Text = "0.00";
            }
        }
        Calcular_Total_A_Pagar();
    }

    protected void Txt_Monto_Desc_Multas_Text_Changed(object sender, EventArgs e)
    {
        if (Txt_Monto_Multas.Text.Replace("$", "").Trim() == "0.00")
        {
            Txt_Monto_Desc_Multas.Text = "0.00";
            Txt_Porcentaje_Desc_Multas.Text = "0.00";
        }
        else
        {
            if (Txt_Monto_Desc_Multas.Text == "")
            {
                Txt_Monto_Desc_Multas.Text = "0.00";
                Txt_Porcentaje_Desc_Multas.Text = "0.00";
            }
            else
            {
                try
                {
                    Double Monto_Desc_Multas = 0;
                    Double Monto_Multas = 0;
                    Double Porcentaje_Multas;
                    Monto_Multas = Convert.ToDouble(Convert.ToDouble(Txt_Monto_Multas.Text.Replace("$", "")).ToString("#,###,###,###,###,###,###,##0.00"));
                    Monto_Desc_Multas = Convert.ToDouble(Convert.ToDouble(Txt_Monto_Desc_Multas.Text).ToString("#,###,###,###,###,###,###,##0.00"));
                    Porcentaje_Multas = (Monto_Desc_Multas / Monto_Multas) * 100;
                    Txt_Monto_Desc_Multas.Text = Monto_Desc_Multas.ToString("#,###,###,###,###,###,###,##0.00");
                    Txt_Porcentaje_Desc_Multas.Text = Porcentaje_Multas.ToString("##0.00");
                    Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio Descuento = new Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio();
                    Descuento.P_Empleado_Id = Cls_Sessiones.Empleado_ID;
                    DataTable Dt_Desc = Descuento.Consultar_Rangos_De_Descuento_Por_Rol_Completo();
                    Boolean Entro = false;
                    foreach (DataRow Dr_Desc in Dt_Desc.Rows)
                    {
                        if (Dr_Desc[Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Tipo].ToString().Equals("MULTAS"))
                        {
                            if (Convert.ToDouble(Txt_Porcentaje_Desc_Multas.Text) > Convert.ToDouble(Dr_Desc[Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Porcentaje].ToString()))
                            {
                                Txt_Porcentaje_Desc_Multas.Text = Convert.ToDouble(Dr_Desc[Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Porcentaje].ToString()).ToString("##0.00");
                                Txt_Monto_Desc_Multas.Text = (Monto_Multas * Convert.ToDouble(Dr_Desc[Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Porcentaje].ToString()) / 100).ToString("#,###,###,###,###,###,###,##0.00");
                                Entro = true;
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Descuentos a Impuestos de Fraccionamiento", "alert('El monto de descuento máximo a otorgar en Multas es de: " + Txt_Monto_Desc_Multas.Text + ".');", true);
                            }
                            else
                            {
                                Calcular_Total_A_Pagar();
                                return;
                            }
                        }
                    }
                    if (Dt_Desc.Rows.Count == 0 || !Entro)
                    {
                        Txt_Monto_Desc_Multas.Text = "0.00";
                        Txt_Porcentaje_Desc_Multas.Text = "0.00";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Descuento a Impuesto de Fraccionamiento", "alert('No tiene privilegios para dar descuentos en multas.')", true);
                    }
                }
                catch
                {
                    Txt_Monto_Desc_Multas.Text = "0.00";
                    Txt_Porcentaje_Desc_Multas.Text = "0.00";
                }
            }
        }
        Calcular_Total_A_Pagar();
    }

    protected void Txt_Monto_Desc_Recargo_Text_Changed(object sender, EventArgs e)
    {
        if (Txt_Monto_Recargos.Text.Replace("$", "").Trim() == "0.00")
        {
            Txt_Monto_Desc_Recargo.Text = "0.00";
            Txt_Prcentaje_Desc_Recargos.Text = "0.00";
        }
        else
        {
            if (Txt_Monto_Desc_Recargo.Text == "")
            {
                Txt_Monto_Desc_Recargo.Text = "0.00";
                Txt_Prcentaje_Desc_Recargos.Text = "0.00";
            }
            else
            {
                try
                {
                    Double Monto_Desc_recargos = 0;
                    Double Monto_Recargos = 0;
                    Double Porcentaje_recargos;
                    Monto_Recargos = Convert.ToDouble(Convert.ToDouble(Txt_Monto_Recargos.Text.Replace("$", "")).ToString("#,###,###,###,###,###,###,##0.00"));
                    Monto_Desc_recargos = Convert.ToDouble(Convert.ToDouble(Txt_Monto_Desc_Recargo.Text).ToString("#,###,###,###,###,###,###,##0.00"));
                    Porcentaje_recargos = (Monto_Desc_recargos / Monto_Recargos) * 100;
                    Txt_Monto_Desc_Recargo.Text = Monto_Desc_recargos.ToString("#,###,###,###,###,###,###,##0.00");
                    Txt_Prcentaje_Desc_Recargos.Text = Porcentaje_recargos.ToString("##0.00");
                    Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio Descuento = new Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio();
                    Descuento.P_Empleado_Id = Cls_Sessiones.Empleado_ID;
                    DataTable Dt_Desc = Descuento.Consultar_Rangos_De_Descuento_Por_Rol_Completo();
                    Boolean Entro = false;
                    foreach (DataRow Dr_Desc in Dt_Desc.Rows)
                    {
                        if (Dr_Desc[Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Tipo].ToString().Equals("FRACC"))
                        {
                            if (Convert.ToDouble(Txt_Prcentaje_Desc_Recargos.Text) > Convert.ToDouble(Dr_Desc[Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Porcentaje].ToString()))
                            {
                                Txt_Prcentaje_Desc_Recargos.Text = Convert.ToDouble(Dr_Desc[Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Porcentaje].ToString()).ToString("##0.00");
                                Txt_Monto_Desc_Recargo.Text = (Monto_Recargos * Convert.ToDouble(Dr_Desc[Cat_Pre_Rangos_De_Descuento_Por_Rol.Campo_Porcentaje].ToString()) / 100).ToString("#,###,###,###,###,###,###,##0.00");
                                Entro = true;
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Descuento a Impuesto de Fraccionamiento", "alert('El monto de descuento máximo a otorgar en regargos es de: " + Txt_Monto_Desc_Recargo.Text + ".')", true);
                            }
                            else
                            {
                                Calcular_Total_A_Pagar();
                                return;
                            }
                        }
                    }
                    if (Dt_Desc.Rows.Count == 0 || !Entro)
                    {
                        Txt_Prcentaje_Desc_Recargos.Text = "0.00";
                        Txt_Monto_Desc_Recargo.Text = "0.00";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Descuento a Impuesto de Fraccionamiento", "alert('No tiene privilegios para dar descuentos en recargos.')", true);
                    }
                }
                catch
                {
                    Txt_Monto_Desc_Recargo.Text = "0.00";
                    Txt_Prcentaje_Desc_Recargos.Text = "0.00";
                }
            }
        }
        Calcular_Total_A_Pagar();
    }

    #endregion

    #region Validaciones

    protected Boolean Validar_Campos()
    {
        String Error = "";
        Boolean Datos_Llenos = true;
        if (Txt_Cuenta_Predial.Text == "")
        {
            Error += "Seleccione una cuenta predial. ";
            Datos_Llenos = false;
        }
        if (Txt_Fecha_Inicial.Text.Trim() == "")
        {
            if (!Datos_Llenos) { Error = Error + "<br>"; }
            Error += "Seleccione un fecha inicial. ";
            Datos_Llenos = false;
        }
        if (Cmb_Estatus.SelectedIndex == 0)
        {
            if (!Datos_Llenos) { Error = Error + "<br>"; }
            Error += "Seleccione un estatus. ";
            Datos_Llenos = false;
        }
        if (Txt_Fecha_Vencimiento.Text == "")
        {
            if (!Datos_Llenos) { Error = Error + "<br>"; }
            Error += "Seleccione una fecha de vencimiento. ";
            Datos_Llenos = false;
        }
        if (Txt_Prcentaje_Desc_Recargos.Text == "")
        {
            if (!Datos_Llenos) { Error = Error + "<br>"; }
            Error += "Introduzca un porcentaje de descuento de recargos. ";
            Datos_Llenos = false;
        }
        if (Txt_Porcentaje_Desc_Multas.Text == "")
        {
            if (!Datos_Llenos) { Error = Error + "<br>"; }
            Error += "Introduzca un porcentaje de descuento de multas. ";
            Datos_Llenos = false;
        }
        if (Txt_Monto_Desc_Recargo.Text == "")
        {
            if (!Datos_Llenos) { Error = Error + "<br>"; }
            Error += "Introduzca un monto de descuento en recargos. ";
            Datos_Llenos = false;
        }
        if (Txt_Monto_Desc_Multas.Text == "")
        {
            if (!Datos_Llenos) { Error = Error + "<br>"; }
            Error += "Introduzca un monto de descuento en multas. ";
            Datos_Llenos = false;
        }
        if (!Datos_Llenos)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Error;
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Datos_Llenos;
    }

    #endregion

    protected void Grid_Descuentos_Der_Sup_Page_Index_Changing(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Limpiar_Componentes();
            Grid_Descuentos_Der_Sup.SelectedIndex = (-1);
            Cargar_Grid_Descuentos_Derechos_Supervision(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    protected void Grid_Descuentos_Der_Sup_Selected_Index_Changed(object sender, EventArgs e)
    {
        if (Grid_Descuentos_Der_Sup.SelectedIndex > -1)
        {
            Limpiar_Componentes();
            DataTable Dt_Descuentos = new DataTable();
            Cls_Ope_Pre_Descuentos_Fracc_Negocio Descuento = new Cls_Ope_Pre_Descuentos_Fracc_Negocio();
            Descuento.P_No_Descuento = Grid_Descuentos_Der_Sup.SelectedRow.Cells[1].Text;
            Dt_Descuentos = Descuento.Consultar_Descuentos();
            if (Dt_Descuentos.Rows.Count > 0)
            {
                Txt_Cuenta_Predial.Text = Dt_Descuentos.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial].ToString();
                Hdf_Cuenta_Predial_ID.Value = Dt_Descuentos.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
                Hdf_No_Descuento.Value = Dt_Descuentos.Rows[0][Ope_Pre_Descuento_Fracc.Campo_No_Descuento].ToString();
                Hdf_No_Impuesto_Der_Sup.Value = Dt_Descuentos.Rows[0][Ope_Pre_Descuento_Fracc.Campo_No_Impuesto_fraccionamiento].ToString();
                DataTable Dt_Impuesto = new DataTable(); //Almacenará el convenio y después los costos...

                Dt_Impuesto = Descuento_En_Reestructura();//Almacena el convenio para consultar los costos corresponeientes, ya sea desde un convenio o impuesto...
                if (Dt_Impuesto.Rows.Count == 0 || (Dt_Impuesto.Rows.Count > 0 && Dt_Impuesto.Rows[0][Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Reestructura].ToString().Equals("")))
                {
                    Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio Impuesto = new Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio();
                    Impuesto.P_Campos_Sumados = true;
                    Impuesto.P_No_Impuesto_Fraccionamiento = Hdf_No_Impuesto_Der_Sup.Value;
                    Impuesto.Consultar_Impuestos_Fraccionamiento();
                    Dt_Impuesto = Impuesto.P_Dt_Detalles_Impuestos_Fraccionamiento;//Almacena los costos del impuesto
                }
                else
                {
                    Dt_Impuesto = Obtener_Dato_Consulta(Dt_Impuesto);//Alamacena los datos de una reestructura o convenio, en caso de ser necesario...
                }
                Llenar_Costos(Dt_Impuesto);
                Txt_Fecha_Inicial.Text = Convert.ToDateTime(Dt_Descuentos.Rows[0][Ope_Pre_Descuento_Fracc.Campo_Fecha_Inicial].ToString()).ToString("dd/MMM/yyyy");
                Txt_Fecha_Vencimiento.Text = Convert.ToDateTime(Dt_Descuentos.Rows[0][Ope_Pre_Descuento_Fracc.Campo_Fecha_Vencimiento].ToString()).ToString("dd/MMM/yyyy");
                Txt_Realizo.Text = Dt_Descuentos.Rows[0][Ope_Pre_Descuento_Fracc.Campo_Realizo].ToString();
                Txt_Observaciones.Text = Dt_Descuentos.Rows[0][Ope_Pre_Descuento_Fracc.Campo_Observaciones].ToString();
                Txt_Prcentaje_Desc_Recargos.Text = Convert.ToDouble(Dt_Descuentos.Rows[0][Ope_Pre_Descuento_Fracc.Campo_Desc_Recargo].ToString()).ToString("##0.00");
                Txt_Porcentaje_Desc_Multas.Text = Convert.ToDouble(Dt_Descuentos.Rows[0][Ope_Pre_Descuento_Fracc.Campo_Desc_Multa].ToString()).ToString("##0.00");
                Txt_Monto_Desc_Multas.Text = Convert.ToDouble(Dt_Descuentos.Rows[0][Ope_Pre_Descuento_Fracc.Campo_Monto_Multas].ToString()).ToString("#,###,###,###,###,###,###,##0.00");
                Txt_Monto_Desc_Recargo.Text = Convert.ToDouble(Dt_Descuentos.Rows[0][Ope_Pre_Descuento_Fracc.Campo_Monto_Recargos].ToString()).ToString("#,###,###,###,###,###,###,##0.00");
                Txt_Total_Por_Pagar.Text = "$ " + Convert.ToDouble(Dt_Descuentos.Rows[0][Ope_Pre_Descuento_Fracc.Campo_Total_Por_Pagar].ToString()).ToString("#,###,###,###,###,###,###,##0.00");
                Cmb_Estatus.SelectedValue = Dt_Descuentos.Rows[0][Ope_Pre_Descuento_Fracc.Campo_Estatus].ToString();
                Calcular_Total_A_Pagar();
                Txt_Cuenta_Predial_TextChanged();
                Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Cuenta_Pendiente = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
                Cuenta_Pendiente.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                if (!Cuenta_Pendiente.Consultar_Cuenta_Pendiente())
                {
                    Cargar_Datos();
                }
                Div_Datos.Visible = true;
                Div_Grid_Descuentos.Visible = false;
                Btn_Salir.AlternateText = "Atrás";
            }
        }
    }

    protected Boolean Convenio_Con_Descuento()
    {
        DataTable Dt_Convenio;
        Boolean Desc_Aplicado = false;
        Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio Convenio = new Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio();
        Convenio.P_Campos_Dinamicos = Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio;
        Convenio.P_Filtros_Dinamicos = Ope_Pre_Convenios_Fraccionamientos.Campo_No_Descuento + "='" + Hdf_No_Descuento.Value + "' AND " + Ope_Pre_Convenios_Fraccionamientos.Campo_Estatus + " IN ('ACTIVO','INCUMPLIDO')";
        Dt_Convenio = Convenio.Consultar_Convenio_Fraccionamiento();
        if (Dt_Convenio.Rows.Count > 0)
        {
            Desc_Aplicado = true;
        }
        return Desc_Aplicado;
    }

    protected DataTable Descuento_En_Reestructura()
    {
        DataTable Dt_Convenio;
        Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio Convenio = new Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio();
        Convenio.P_Campos_Dinamicos = Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + ", " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Reestructura + "," + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + ", " + Ope_Pre_Convenios_Fraccionamientos.Campo_Estatus;
        Convenio.P_Filtros_Dinamicos = Ope_Pre_Convenios_Fraccionamientos.Campo_No_Descuento + "='" + Hdf_No_Descuento.Value + "' AND " + Ope_Pre_Convenios_Derechos_Supervision.Campo_Estatus + " IN ('PENDIENTE')";
        Dt_Convenio = Convenio.Consultar_Convenio_Fraccionamiento();
        return Dt_Convenio;
    }

    //protected DataTable Costos_En_Convenio()
    //{
    //    DataTable Dt_Convenio;
    //    Cls_Ope_Pre_Convenios_Derechos_Supervision_Negocio Convenio = new Cls_Ope_Pre_Convenios_Derechos_Supervision_Negocio();
    //    Convenio.P_Campos_Dinamicos = Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Convenio + ", " + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Reestructura + "," + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Impuesto_Dereho_Supervisio + ", " + Ope_Pre_Convenios_Derechos_Supervision.Campo_Estatus;
    //    Convenio.P_Filtros_Dinamicos = Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Descuento + "='" + Hdf_No_Descuento.Value + "' AND " + Ope_Pre_Convenios_Derechos_Supervision.Campo_Estatus + " IN ('PENDIENTE')";
    //    Dt_Convenio = Convenio.Consultar_Convenio_Derecho_Supervisions();
    //    return Dt_Convenio;
    //}

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Obtener_Dato_Consulta
    ///DESCRIPCIÓN          : Consulta los montos de un convenio o reestructura según sea el caso
    ///PARAMETROS:     
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 21/Diciembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private DataTable Obtener_Dato_Consulta(DataTable Dt_Convenio)
    {
        String Mi_SQL;
        DataTable Dt_Montos = new DataTable();

        try
        {
            Mi_SQL = "SELECT SUM(" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Multas + ") AS MONTO, SUM(" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Impuesto + ") AS IMPORTE, SUM(" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Ordinarios + ") AS RECARGOS FROM " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + " WHERE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + "='" + Dt_Convenio.Rows[0][Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio].ToString() + "' AND ";
            if (Dt_Convenio.Rows[0][Ope_Pre_Convenios_Fraccionamientos.Campo_No_Reestructura].ToString() == "1")
            {
                Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + " IS NULL AND ";
            }
            else
            {
                Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + "=" + (Convert.ToInt32(Dt_Convenio.Rows[0][Ope_Pre_Convenios_Fraccionamientos.Campo_No_Reestructura].ToString()) - 1) + " AND ";
            }
            Mi_SQL += Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus + "='CANCELADO'";
            DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            if (dataset != null)
            {
                Dt_Montos = dataset.Tables[0];
            }
        }
        catch
        {
        }
        finally
        {
        }

        return Dt_Montos;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Validar_Componentes
    ///DESCRIPCIÓN          : Consulta el Campo dado de la Tabla Indicada
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benvides Guardado
    ///FECHA_CREO           : 24/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private String Obtener_Dato_Consulta(String No_Impuesto)
    {
        String Mi_SQL;
        String Dato_Consulta = "";

        try
        {
            Mi_SQL = "SELECT 'IMP'||TO_CHAR(" + Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Creo + ",'YY')||TO_NUMBER(" + Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + ") AS FOLIO FROM " + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + " WHERE " + Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + "='" + No_Impuesto + "'";

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

    #region Imprimir reporte

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Descuentos
    ///DESCRIPCIÓN          : Crea un DataTable con las columnas y datos del descuento
    ///PARAMETROS: 
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 04/Enero/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Descuentos()
    {
        Ds_Ope_Pre_Descuentos_Fracc Ds_Descuentos = new Ds_Ope_Pre_Descuentos_Fracc();
        DataRow Dr_Descuentos;

        foreach (DataTable Dt_Descuentos in Ds_Descuentos.Tables)
        {
            if (Dt_Descuentos.TableName == "Dt_Ope_Pre_Descuentos_Fracc")
            {
                Dr_Descuentos = Dt_Descuentos.NewRow();
                Dr_Descuentos["CUENTA_PREDIAL"] = Txt_Cuenta_Predial.Text;
                Dr_Descuentos["PROPIETARIO"] = Txt_Propietario.Text; ;
                Dr_Descuentos["CALLE"] = Txt_Calle.Text;
                Dr_Descuentos["NO_EXTERIOR"] = Txt_No_Exterior.Text;
                Dr_Descuentos["NO_INTERIOR"] = Txt_No_Interior.Text;
                Dr_Descuentos["COLONIA"] = Txt_Colonia.Text; ;
                Dr_Descuentos["FECHA"] = Txt_Fecha_Inicial.Text.ToUpper();
                Dr_Descuentos["FECHA_VENCIMIENTO"] = Txt_Fecha_Vencimiento.Text.ToUpper();
                Dr_Descuentos["IMP_FRACCIONAMIENTO"] = "$ " + Txt_Monto_Impuesto_Der_Sup.Text.Replace("$", "");
                Dr_Descuentos["RECARGOS"] = "$ " + Txt_Monto_Recargos.Text.Replace("$", "");
                Dr_Descuentos["PORC_DESC_RECARGOS"] = Txt_Prcentaje_Desc_Recargos.Text + " %";
                Dr_Descuentos["MONT_DESC_RECARGOS"] = "$ " + Txt_Monto_Desc_Recargo.Text;
                Dr_Descuentos["MULTAS"] = "$ " + Txt_Monto_Multas.Text.Replace("$", "");
                Dr_Descuentos["PORC_DESC_MULTAS"] = Txt_Porcentaje_Desc_Multas.Text + " %";
                Dr_Descuentos["MONT_DESC_MULTAS"] = "$ " + Txt_Monto_Desc_Multas.Text;
                Dr_Descuentos["TOTAL_PAGAR"] = Txt_Total_Por_Pagar.Text;
                Dr_Descuentos["REALIZO"] = Txt_Realizo.Text;
                Dt_Descuentos.Rows.Add(Dr_Descuentos);
            }
        }

        return Ds_Descuentos;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Imprimir_Reporte
    ///DESCRIPCIÓN          : Genera el reporte de Crystal con los datos proporcionados en el DataTable y lo manda a la impresora default
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Imprimir_Reporte(DataSet Ds_Convenios, String Nombre_Reporte, String Nombre_Archivo)
    {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte);
        try
        {
            Reporte.Load(File_Path);
            Reporte.SetDataSource(Ds_Convenios);
        }
        catch
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "No se pudo cargar el reporte para su impresión";
        }

        String Archivo_PDF = Nombre_Archivo + ".pdf";  // Es el nombre del PDF que se va a generar    
        try
        {
            ExportOptions Export_Options = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
            Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Archivo_PDF);
            Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
            Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
            Reporte.Export(Export_Options);
        }
        catch //(Exception Ex)
        {
            //Lbl_Mensaje_Error.Visible = true;
            //Lbl_Mensaje_Error.Text = "No se pudo exportar el reporte";
        }

        try
        {
            Mostrar_Reporte(Archivo_PDF);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Mostrar_Reporte
    ///DESCRIPCIÓN          : Visualiza en pantalla el reporte indicado
    ///PARAMETROS           : Nombre_Reporte: cadena con el nombre del archivo.
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Mostrar_Reporte(String Nombre_Reporte)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            Pagina = Pagina + Nombre_Reporte;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Rpt", "window.open('" + Pagina + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Insertar_Pasivo
    ///DESCRIPCIÓN          : Consulta el Costo del Documento y lo Inserta en Pasivo
    ///PARAMETROS:     
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 22/Septiembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Insertar_Pasivo(String Referencia, Cls_Ope_Pre_Descuentos_Fracc_Negocio Desc)
    {
        try
        {
            //OracleConnection Cn = new OracleConnection();
            //OracleCommand Cmd = new OracleCommand();
            //OracleTransaction Trans = null;
            Cls_Cat_Pre_Claves_Ingreso_Negocio Claves_Ingreso = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Calculo_Impuesto_Traslado = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio();
            DataTable Dt_Clave;

            ////// crear transaccion para modificar tabla de calculos y de adeudos folio
            ////Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            ////Cn.Open();
            ////Trans = Cn.BeginTransaction();
            ////Cmd.Connection = Cn;
            ////Cmd.Transaction = Trans;
            ////Calculo_Impuesto_Traslado.P_Cmd_Calculo = Cmd;
            Calculo_Impuesto_Traslado.P_Referencia = Referencia + "' AND " + Ope_Ing_Pasivo.Campo_Descripcion + " LIKE '%DESCUENTO%";
            Calculo_Impuesto_Traslado.Eliminar_Referencias_Pasivo();

            if (Desc.P_Monto_Multas > 0)
            {
                Claves_Ingreso.P_Tipo = "FRACCIONAMIENTOS";
                Claves_Ingreso.P_Tipo_Predial_Traslado = "MULTAS";
                Dt_Clave = Claves_Ingreso.Consultar_Clave_Ingreso();
                if (Dt_Clave.Rows.Count > 0)
                {
                    Calculo_Impuesto_Traslado.P_Referencia = Referencia;
                    Calculo_Impuesto_Traslado.P_Descripcion = "DESCUENTO MULTAS";
                    Calculo_Impuesto_Traslado.P_Estatus = "POR PAGAR";
                    Calculo_Impuesto_Traslado.P_Clave_Ingreso_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID].ToString();
                    Calculo_Impuesto_Traslado.P_Dependencia_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID].ToString();
                    Calculo_Impuesto_Traslado.P_Monto_Total_Pagar = (Desc.P_Monto_Multas * -1).ToString("0.00");
                    Calculo_Impuesto_Traslado.P_Fecha_Tramite = DateTime.Now.ToString("dd/MMM/yyyy");
                    Calculo_Impuesto_Traslado.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                    Calculo_Impuesto_Traslado.P_Fecha_Vencimiento_Pasivo = Desc.P_Fecha_Vencimiento.ToString("dd/MMM/yyyy");
                    Calculo_Impuesto_Traslado.Alta_Pasivo();
                }
            }

            if (Desc.P_Monto_Recargos > 0)
            {
                Claves_Ingreso.P_Tipo = "FRACCIONAMIENTOS";
                Claves_Ingreso.P_Tipo_Predial_Traslado = "RECARGOS ORDINARIOS";
                Dt_Clave = Claves_Ingreso.Consultar_Clave_Ingreso();
                if (Dt_Clave.Rows.Count > 0)
                {
                    Calculo_Impuesto_Traslado.P_Referencia = Referencia;
                    Calculo_Impuesto_Traslado.P_Descripcion = "DESCUENTO ORDINARIOS";
                    Calculo_Impuesto_Traslado.P_Estatus = "POR PAGAR";
                    Calculo_Impuesto_Traslado.P_Clave_Ingreso_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID].ToString();
                    Calculo_Impuesto_Traslado.P_Dependencia_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID].ToString();
                    Calculo_Impuesto_Traslado.P_Monto_Total_Pagar = (Desc.P_Monto_Recargos * -1).ToString("0.00");
                    Calculo_Impuesto_Traslado.P_Fecha_Tramite = DateTime.Now.ToString("dd/MMM/yyyy");
                    Calculo_Impuesto_Traslado.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                    Calculo_Impuesto_Traslado.P_Fecha_Vencimiento_Pasivo = Desc.P_Fecha_Vencimiento.ToString("dd/MMM/yyyy");
                    Calculo_Impuesto_Traslado.Alta_Pasivo();
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = "El Pasivo no pudo ser insertado: " + Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Eliminar_Pasivo
    ///DESCRIPCIÓN          : Elimina los pasivos de descuento.
    ///PARAMETROS:     
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 10/Marzo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Eliminar_Pasivo(String Referencia)
    {
        try
        {
            //OracleConnection Cn = new OracleConnection();
            //OracleCommand Cmd = new OracleCommand();
            //OracleTransaction Trans = null;
            Cls_Cat_Pre_Claves_Ingreso_Negocio Claves_Ingreso = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Calculo_Impuesto_Traslado = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio();

            ////// crear transaccion para modificar tabla de calculos y de adeudos folio
            ////Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            ////Cn.Open();
            ////Trans = Cn.BeginTransaction();
            ////Cmd.Connection = Cn;
            ////Cmd.Transaction = Trans;
            ////Calculo_Impuesto_Traslado.P_Cmd_Calculo = Cmd;

            Calculo_Impuesto_Traslado.P_Referencia = Referencia + "' AND " + Ope_Ing_Pasivo.Campo_Descripcion + " LIKE '%DESCUENTO%";
            Calculo_Impuesto_Traslado.Eliminar_Referencias_Pasivo();

        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = "El Pasivo no pudo ser insertado: " + Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    #endregion

}