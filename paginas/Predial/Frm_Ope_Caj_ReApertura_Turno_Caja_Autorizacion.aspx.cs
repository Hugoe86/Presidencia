using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Presidencia.Operaciones_Apertura_Turnos.Negocio;
using Presidencia.Catalogo_Cajas.Negocio;
using Presidencia.Catalogo_Modulos.Negocio;
using Presidencia.Sessiones;

public partial class paginas_Predial_Frm_Ope_Caj_ReApertura_Turno_Caja_Autorizacion : System.Web.UI.Page
{
    #region Load/Init
    ///************************************************************************************************
    /// NOMBRE: Page_Load
    ///
    /// DESCRIPCIÓN: Habilita la configuración inical de la página.
    /// 
    /// PARÁMETROS: No Áplica
    /// 
    /// USUARIO CREO: Ismael Prieto Sánchez 
    /// FECHA CREO: 12/Diciembre/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    ///************************************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Configuracion_Inicial();//Habilita la configuración inical de la página.
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cargar la configuración inicial de la página [Page_Load]. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region Metodos
    ///************************************************************************************************
    /// NOMBRE: Configuracion_Inicial
    ///
    /// DESCRIPCIÓN: Habilita la configuración inical de la página.
    /// 
    /// PARÁMETROS: No Áplica
    /// 
    /// USUARIO CREO: Ismael Prieto Sánchez 
    /// FECHA CREO: 12/Diciembre/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    ///************************************************************************************************
    protected void Configuracion_Inicial()
    {
        Btn_Abrir_Cierre_Turno.Enabled = false;
        Txt_Autorizo_Reapertura.Enabled = false;
        Txt_Observaciones.Enabled = false;
        Limpiar_Controles();//Limpia los controles de la página.
        Llenar_Combo_Modulos();
        Llenar_Combo_Cajas(Cmb_Modulos.SelectedItem.Value);
    }
    ///************************************************************************************************
    /// NOMBRE: Limpiar_Controles
    ///
    /// DESCRIPCIÓN: Limpia los controles de la página.
    /// 
    /// PARÁMETROS: No Áplica
    /// 
    /// USUARIO CREO: Ismael Prieto Sánchez 
    /// FECHA CREO: 12/Diciembre/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    ///************************************************************************************************
    protected void Limpiar_Controles()
    {
        //Limpia el grid de  cierre de día.
        Grid_Turnos_Caja.DataSource = new DataTable();
        Grid_Turnos_Caja.DataBind();
        Grid_Turnos_Caja.SelectedIndex = -1;
        //Limpia las cajas de texto que almacenan los rangos de fecha de busqueda.
        Txt_Fecha_Inicio.Text = String.Empty;
        Txt_Fecha_Fin.Text = String.Empty;
        Cmb_Modulos.SelectedIndex = -1;
        Cmb_Cajas.SelectedIndex = -1;
        Txt_Autorizo_Reapertura.Text = "";
        Txt_Observaciones.Text = "";
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Modulos
    ///DESCRIPCIÓN: Llena el combo de modulos
    ///PROPIEDADES:         
    ///CREO: Ismael Prieto Sánchez
    ///FECHA_CREO: 19/Noviembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Modulos()
    {
        try
        {
            Cls_Cat_Pre_Modulos_Negocio Modulos = new Cls_Cat_Pre_Modulos_Negocio();
            DataTable tabla = Modulos.Consultar_Nombre_Modulos();
            Cmb_Modulos.DataSource = tabla;
            Cmb_Modulos.DataValueField = "MODULO_ID";
            Cmb_Modulos.DataTextField = "UBICACION";
            Cmb_Modulos.DataBind();
            Cmb_Modulos.Items.Insert(0, new ListItem("GLOBAL", ""));
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Cajas
    ///DESCRIPCIÓN: Llena el combo de Cajas
    ///PROPIEDADES: Modulo_ID, pasa el id del modulo a consultar        
    ///CREO: Sergio Manuel Gallardo Andrade
    ///FECHA_CREO: 16/Octubre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Cajas(String Modulo_ID)
    {
        try
        {
            Cls_Cat_Pre_Cajas_Negocio Cajas = new Cls_Cat_Pre_Cajas_Negocio();
            Cajas.P_Modulo = Modulo_ID;
            DataTable tabla = Cajas.Consultar_Cajas_Modulo();
            Cmb_Cajas.DataSource = tabla;
            Cmb_Cajas.DataValueField = "CAJA_ID";
            Cmb_Cajas.DataTextField = "NO_CAJA";
            Cmb_Cajas.DataBind();
            Cmb_Cajas.Items.Insert(0, new ListItem("GLOBAL", ""));
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
        }
    }
    ///************************************************************************************************
    /// NOMBRE: Consultar_Cierres_Turnos_Caja
    ///
    /// DESCRIPCIÓN: Consulta los turnos de caja cerrados
    /// 
    /// PARÁMETROS: No Aplica.
    /// 
    /// USUARIO CREO: Ismael Prieto Sánchez 
    /// FECHA CREO: 12/Diciembre/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    ///************************************************************************************************
    protected void Consultar_Cierres_Turnos_Caja()
    {
        Cls_Ope_Pre_Apertura_Turno_Negocio Rs_Consulta_Turnos_Caja = new Cls_Ope_Pre_Apertura_Turno_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Reapertura_Dias = null;//Variable que almacenara el listado de cierres de día.

        try
        {
            Txt_Fecha_Inicio.Text = Txt_Fecha_Inicio.Text.Replace("__/___/____", String.Empty);
            Txt_Fecha_Fin.Text = Txt_Fecha_Fin.Text.Replace("__/___/____", String.Empty);

            //Establecemos los filtros de busqueda.
            if (!String.IsNullOrEmpty(Txt_Fecha_Inicio.Text))
                Rs_Consulta_Turnos_Caja.P_Fecha_Cierre = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Txt_Fecha_Inicio.Text.Trim()));

            if (!String.IsNullOrEmpty(Txt_Fecha_Fin.Text))
                Rs_Consulta_Turnos_Caja.P_Fecha_Turno = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Txt_Fecha_Fin.Text.Trim()));

            Rs_Consulta_Turnos_Caja.P_Modulo = Cmb_Modulos.SelectedItem.Value;
            Rs_Consulta_Turnos_Caja.P_Caja_Id = Cmb_Cajas.SelectedItem.Value;
            //Ejecutamos la consulta de los turnos de caja cerrados.
            Dt_Reapertura_Dias = Rs_Consulta_Turnos_Caja.Consulta_Turnos_Cerrados();
            
            //Asignamos al grid
            Grid_Turnos_Caja.DataSource = Dt_Reapertura_Dias;
            Grid_Turnos_Caja.DataBind();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los cierre de caja. Error: [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos
    /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
    /// CREO        : Ismael Prieto Sánchez
    /// FECHA_CREO  : 18-Noviembre-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos()
    {
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

        if (string.IsNullOrEmpty(Txt_Autorizo_Reapertura.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El nombre de quien autoriza la reapertura. <br>";
            Datos_Validos = false;
        }
        if (string.IsNullOrEmpty(Txt_Observaciones.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Las observaciones de autorización de reapertura. <br>";
            Datos_Validos = false;
        }

        return Datos_Validos;
    }
    #endregion

    #region Eventos
    protected void Cmb_Modulos_SelectedIndexChanged(object sender, EventArgs e)
    {
        Llenar_Combo_Cajas(Cmb_Modulos.SelectedItem.Value);
    }
    protected void Btn_Busqueda_Turnos_Caja_Click(object sender, ImageClickEventArgs e)
    {
        Consultar_Cierres_Turnos_Caja();
    }
    protected void Btn_Abrir_Cierre_Turno_Click(object sender, EventArgs e)
    {
        Cls_Ope_Pre_Apertura_Turno_Negocio Rs_Reapertura_Turno = new Cls_Ope_Pre_Apertura_Turno_Negocio();//Variable de conexión con la capa de negocios.
        String No_Turno = String.Empty;//No de cierre de dia a reabrir.
        
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (Grid_Turnos_Caja.SelectedIndex != (-1))
            {
                if (Grid_Turnos_Caja.SelectedIndex >= 0)
                {
                    if (Validar_Datos())
                    {
                        No_Turno = Grid_Turnos_Caja.Rows[Grid_Turnos_Caja.SelectedIndex].Cells[1].Text.Trim();

                        Rs_Reapertura_Turno.P_No_Turno = No_Turno;
                        Rs_Reapertura_Turno.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
                        Rs_Reapertura_Turno.P_Nombre_Empleado = Cls_Sessiones.Nombre_Empleado;
                        Rs_Reapertura_Turno.P_ReApertua_Nombre_Autorizo = Txt_Autorizo_Reapertura.Text.Trim().ToUpper();
                        Rs_Reapertura_Turno.P_ReApertua_Observaciones_Autorizo = Txt_Observaciones.Text.Trim().ToUpper();
                        //Se ejecuta la apertura del cierre de día.
                        Rs_Reapertura_Turno.Autoriza_ReAPertura_Turno();

                        //Vuelve los controles ala configuración inicial de la página.
                        Configuracion_Inicial();
                        //Mostramos un mensaje de operación completa.
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Reapertura Turno", "alert('Reapertura realizada');", true);
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al ejecutar la reapertura del cierre del día. [Btn_Abrir_Cierre_Dia_Click]. Error: [" + Ex.Message + "]");
        }
    }
    ///************************************************************************************************
    /// NOMBRE: Grid_Cierres_Dia_SelectedIndexChanged
    ///
    /// DESCRIPCIÓN: Método que se ejecuta al seleccionar un elemento del Grid_Cierres_Dia.
    /// 
    /// PARÁMETROS: No Aplica.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete. 
    /// FECHA CREO: 23/Octubre/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    ///************************************************************************************************
    protected void Grid_Turnos_Caja_SelectedIndexChanged(object sender, EventArgs e)
    {
        String Estatus_Cierre_Dia = String.Empty;//Variable que almacenara el estatus del cierre de dia seleccionado.

        try
        {
            if (Grid_Turnos_Caja.SelectedIndex != (-1))
            {
                if (Grid_Turnos_Caja.SelectedIndex >= 0)
                {
                    Btn_Abrir_Cierre_Turno.Enabled = true;
                    Txt_Autorizo_Reapertura.Enabled = true;
                    Txt_Observaciones.Enabled = true;
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al seleccionar un elemento del Grid_Cierres_Dia. Error: [" + Ex.Message + "]");
        }
    }
    #endregion
    
}