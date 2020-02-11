using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Operacion_Caj_Cierre_Dia.Negocio;
using Presidencia.Operacion_Pre_Caj_Detalles.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;

public partial class paginas_Predial_Frm_Ope_Pre_Cierre_Dia : System.Web.UI.Page
{
    #region Pago_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //Refresca la session del usuario lagueado al sistema.
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            //Valida que exista algun usuario logueado al sistema.
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!IsPostBack)
            {
                Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
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
    #region Metodos
    #region Metodos Generales
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 21-Octubre-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Habilitar_Controles("Inicial");        //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar
            Limpia_Controles();                    //Limpia los controles del forma
            Txt_Busqueda.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Today);
            Consulta_Datos_Generales_Cierre_Dia(); //Consulta todos los datos del cierre de turno del día que desea consultar el empleado
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION : Limpia los controles que se encuentran en la forma
    /// PARAMETROS  : Incializa: Trae valor de verdadero si se va a inicilizar la forma
    ///                          con respecto a si es la primera vez que se realiza la
    ///                          consulta de los valores
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 22-Octubre-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpia_Controles()
    {
        try
        {
            //Datos generales del Ciere de Día
            Txt_Busqueda.Text = "";
            Txt_No_Cierre_Dia.Text = "";
            Txt_Estatus_Cierre_Dia.Text = "";
            Txt_Fecha_Apertura_Dia.Text = "";
            Txt_Hora_Apertura_Dia.Text = "";
            Txt_Fecha_Cierre_Dia.Text = "";
            Txt_Hora_Cierre_Dia.Text = "";
            //Datos de total recaudado
            Txt_Total_Efectivo.Text = "0.00";
            Txt_Total_Bancos.Text = "0.00";
            Txt_Total_Cheques.Text = "0.00";
            Txt_Total_Transferencia.Text = "0.00";
            Txt_Total_Ajuste_Tarifario.Text = "0.00";
            Txt_Monto_Total.Text = "0.00";
            Grid_Cajas_Abiertas.DataSource = new DataTable();
            Grid_Cajas_Abiertas.DataBind();
            Session.Remove("Consulta_Cajas_Abiertas");
        }
        catch (Exception ex)
        {
            throw new Exception("Limpiar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///                para a siguiente operación
    /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
    ///                          si es una modificacion
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 22-Octubre-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado; ///Indica si el control de la forma va hacer habilitado para utilización del usuario

        try
        {
            Habilitado = false;
            switch (Operacion)
            {
                case "Inicial":
                    Habilitado = false;
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Salir";
                    Btn_Modificar.CausesValidation = false;
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Configuracion_Acceso("Frm_Ope_Pre_Cierre_Dia.aspx");
                    break;

                case "Modificar":
                    Habilitado = true;
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Modificar.Visible = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    break;
            }
            //Habilitación de controles generale
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Cajas_Abiertas.Visible = false;
            Btn_Buscar.Enabled = !Habilitado;
            Btn_Fecha_Busqueda.Enabled = !Habilitado;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    #endregion
    #region (Control Acceso Pagina)
    ///*******************************************************************************
    /// NOMBRE      : Configuracion_Acceso
    /// DESCRIPCIÓN : Habilita las operaciones que podrá realizar el usuario en la página.
    /// PARÁMETROS  : No Áplica.
    /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ  : 23/Mayo/2011 10:43 a.m.
    /// USUARIO MODIFICO  :
    /// FECHA MODIFICO    :
    /// CAUSA MODIFICACIÓN:
    ///*******************************************************************************
    protected void Configuracion_Acceso(String URL_Pagina)
    {
        List<ImageButton> Botones = new List<ImageButton>();//Variable que almacenara una lista de los botones de la página.
        DataRow[] Dr_Menus = null;//Variable que guardara los menus consultados.

        try
        {
            //Agregamos los botones a la lista de botones de la página.
            Botones.Add(Btn_Modificar);
            Botones.Add(Btn_Buscar);

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
    /// PARÁMETROS  : Cadena.- El dato a evaluar si es numerico.
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
    #region (Operaciones)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Datos_Generales_Cierre_Dia
    /// DESCRIPCION : 1. Consulta todos los datos generales del Cierre de Día
    ///               2. Consulta los montos totales del todos los pagos que fueron
    ///                  egistrados durante el Turno de Día
    ///               3. Consulta las cajas que puedan estar abiertas en el turno
    ///                  de día
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 25-Octubre-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Datos_Generales_Cierre_Dia()
    {
        Double Monto_Total = 0;                           //Indica la suma total de los registrado en caja
        Double Total_Efectivo = 0;                        //Almacena el monto del efectivo menos el cambio
        DataTable Dt_Formas_Pago_Turno = new DataTable(); //Obtiene las formas de pago que se tuvo durante el día
        DataTable Dt_Datos_Turno = new DataTable();       //Obtiene los valores de la consulta
        DataTable Dt_Cajas_Abiertas = new DataTable();    //Obtiene todas las cajas que estan abiertas del turno que se esta consultando
        Cls_Ope_Caj_Cierre_Dia_Negocio Rs_Consulta_Ope_Caj_Turnos_Dia = new Cls_Ope_Caj_Cierre_Dia_Negocio(); //Variable de conexion hacia la capa de negocio

        try
        {
            Btn_Modificar.Visible = false;
            Lbl_Cajas_Abiertas.Visible = false;
            Rs_Consulta_Ope_Caj_Turnos_Dia.P_Filtro = Txt_Busqueda.Text.ToString();
            Dt_Datos_Turno = Rs_Consulta_Ope_Caj_Turnos_Dia.Consulta_Datos_Cierre_Dia(); //Consulta los datos generales del cierre del día
            Limpia_Controles(); //Limpia los controles de forma
            //muestra los datos generales del turno del día al usuario en los controles correspondientes
            foreach (DataRow Registro in Dt_Datos_Turno.Rows)
            {
                if (!String.IsNullOrEmpty(Registro[Ope_Caj_Turnos_Dia.Campo_No_Turno].ToString())) Txt_No_Cierre_Dia.Text = Registro[Ope_Caj_Turnos_Dia.Campo_No_Turno].ToString();
                if (!String.IsNullOrEmpty(Registro[Ope_Caj_Turnos_Dia.Campo_Estatus].ToString())) Txt_Estatus_Cierre_Dia.Text = Registro[Ope_Caj_Turnos_Dia.Campo_Estatus].ToString();
                if (!String.IsNullOrEmpty(Registro[Ope_Caj_Turnos_Dia.Campo_Fecha_Turno].ToString())) Txt_Fecha_Apertura_Dia.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro[Ope_Caj_Turnos_Dia.Campo_Fecha_Turno].ToString()));
                if (!String.IsNullOrEmpty(Registro[Ope_Caj_Turnos_Dia.Campo_Hora_Apertura].ToString())) Txt_Hora_Apertura_Dia.Text = String.Format("{0:T}", Convert.ToDateTime(Registro[Ope_Caj_Turnos_Dia.Campo_Hora_Apertura].ToString()));
                if (!String.IsNullOrEmpty(Registro[Ope_Caj_Turnos_Dia.Campo_Fecha_Cierre].ToString())) Txt_Fecha_Cierre_Dia.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro[Ope_Caj_Turnos_Dia.Campo_Fecha_Cierre].ToString()));
                if (!String.IsNullOrEmpty(Registro[Ope_Caj_Turnos_Dia.Campo_Hora_Cierre].ToString())) Txt_Hora_Cierre_Dia.Text = String.Format("{0:T}", Convert.ToDateTime(Registro[Ope_Caj_Turnos_Dia.Campo_Hora_Cierre].ToString()));
            }
            //Si se encontro un turno del día consultando entonces consulta sus formas de pago con su monto y si tiene cajas abiertas las muestra en pantalla
            if (!String.IsNullOrEmpty(Txt_No_Cierre_Dia.Text))
            {
                Rs_Consulta_Ope_Caj_Turnos_Dia.P_No_Cierre_Dia = Txt_No_Cierre_Dia.Text.ToString();
                Dt_Formas_Pago_Turno = Rs_Consulta_Ope_Caj_Turnos_Dia.Consultar_Formas_Pago_Turno_Dia(); //Consulta el monto total que fue pagado en las diferentes formas de pago
                //Muestra el monto al usuario que fue cobrado de cuado a sus forma de pago
                foreach (DataRow Renglon in Dt_Formas_Pago_Turno.Rows)
                {
                    if (Renglon[Ope_Caj_Pagos_Detalles.Campo_Forma_Pago].ToString() == "EFECTIVO")
                    {
                        Total_Efectivo = Total_Efectivo + Convert.ToDouble(Renglon["Total_Pagado"].ToString());
                    }
                    if (Renglon[Ope_Caj_Pagos_Detalles.Campo_Forma_Pago].ToString() == "CAMBIO")
                    {
                        Total_Efectivo = Total_Efectivo - Convert.ToDouble(Renglon["Total_Pagado"].ToString());
                    }
                    if (Renglon[Ope_Caj_Pagos_Detalles.Campo_Forma_Pago].ToString() == "BANCO") Txt_Total_Bancos.Text = String.Format("{0:#,##0.00}", Convert.ToDouble(Renglon["Total_Pagado"].ToString()));
                    if (Renglon[Ope_Caj_Pagos_Detalles.Campo_Forma_Pago].ToString() == "CHEQUE") Txt_Total_Cheques.Text = String.Format("{0:#,##0.00}", Convert.ToDouble(Renglon["Total_Pagado"].ToString()));
                    if (Renglon[Ope_Caj_Pagos_Detalles.Campo_Forma_Pago].ToString() == "TRANSFERENCIA") Txt_Total_Transferencia.Text = String.Format("{0:#,##0.00}", Convert.ToDouble(Renglon["Total_Pagado"].ToString()));
                    if (Renglon[Ope_Caj_Pagos_Detalles.Campo_Forma_Pago].ToString() == "AJUSTE TARIFARIO")
                    {
                        Txt_Total_Ajuste_Tarifario.Text = String.Format("{0:#,##0.00}", Convert.ToDouble(Renglon["Total_Pagado"].ToString()));

                    }
                }
                Total_Efectivo = Total_Efectivo - Convert.ToDouble(Txt_Total_Ajuste_Tarifario.Text);
                Txt_Total_Efectivo.Text = String.Format("{0:#,##0.00}", Total_Efectivo);

                //Obtiene el monto total cobrando derante el turno del día
                Monto_Total = 0;
                Monto_Total = Convert.ToDouble(Txt_Total_Efectivo.Text.ToString().Replace(",", ""));
                Monto_Total += Convert.ToDouble(Txt_Total_Bancos.Text.ToString().Replace(",", ""));
                Monto_Total += Convert.ToDouble(Txt_Total_Cheques.Text.ToString().Replace(",", ""));
                Monto_Total += Convert.ToDouble(Txt_Total_Transferencia.Text.ToString().Replace(",", ""));
                Monto_Total += Convert.ToDouble(Txt_Total_Ajuste_Tarifario.Text.ToString().Replace(",", ""));
                Txt_Monto_Total.Text = String.Format("{0:#,##0.00}", Monto_Total);

                Dt_Cajas_Abiertas = Rs_Consulta_Ope_Caj_Turnos_Dia.Consulta_Turnos_Abiertos(); //Consulta las cajas que se encuentran aun abiertas en el turno proporcionado por el usuario
                Grid_Cajas_Abiertas.DataSource = Dt_Cajas_Abiertas;
                Grid_Cajas_Abiertas.DataBind();
                //Si se encontraron turnos abietos entonces muestra que cajas estan abiertas al usuario
                if (Dt_Cajas_Abiertas.Rows.Count > 0)
                {
                    Session["Consulta_Cajas_Abiertas"] = Dt_Cajas_Abiertas;
                    Llena_Grid_Cajas_Abiertas(); //Agrega las cajas obtenidas de la consulta anterior
                }
                //Si no se enontraron turnos de cajas abiertas entonces valida que el estatus del turno sea abierto para poder cerrar el mismo
                else
                {
                    //Si el turno esta abierto entonces habilita los controles de la forma para poder cerrar el turno del dia
                    if (Txt_Estatus_Cierre_Dia.Text.ToString() == "ABIERTO")
                    {
                        Btn_Buscar.Enabled = false;
                        Btn_Fecha_Busqueda.Enabled = false;
                        Btn_Modificar.Visible = true;
                        Habilitar_Controles("Modificar"); //Prepara los controles de la forma para poder cerrar el turno del día
                        Lbl_Cajas_Abiertas.Visible = true;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Consulta_Datos_Generales_Cierre_Dia. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llena_Grid_Cajas_Abiertas
    /// DESCRIPCION : Llena el grid con las Asistencias que pertenecen al empleado que
    ///               fue seleccionado por empleado
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 25-Octubre-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llena_Grid_Cajas_Abiertas()
    {
        DataTable Dt_Cajas_Abiertas; //Variable que obtendra los datos de la consulta 
        try
        {
            Grid_Cajas_Abiertas.DataBind();
            Dt_Cajas_Abiertas = (DataTable)Session["Consulta_Cajas_Abiertas"];
            Grid_Cajas_Abiertas.DataSource = Dt_Cajas_Abiertas;
            Grid_Cajas_Abiertas.DataBind();
            Grid_Cajas_Abiertas.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_Cajas_Abiertas " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cierre_Dia
    /// DESCRIPCION : Cierra el turno del día en la base de datos
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 25-Octubre-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cierre_Dia()
    {
        Cls_Ope_Caj_Cierre_Dia_Negocio Rs_Modifica_Ope_Caj_Turnos_Dia = new Cls_Ope_Caj_Cierre_Dia_Negocio(); //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta
        try
        {
            Rs_Modifica_Ope_Caj_Turnos_Dia.P_No_Cierre_Dia = Txt_No_Cierre_Dia.Text.ToString();
            Rs_Modifica_Ope_Caj_Turnos_Dia.P_Usuario = Cls_Sessiones.Nombre_Empleado;
            Rs_Modifica_Ope_Caj_Turnos_Dia.Modificar_Cierre_Dia();  //Cierra el turno del día con todos sus valores
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Cierre de Dia", "alert('El Cierre del Día fue Exitoso');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Cierre_Dia " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Datos_Impresion_Cierre_Turno
    /// DESCRIPCION : Realiza el DataSet que se necesita para poder pasar los datos
    ///               de la pantalla al reporte de Crystal
    /// PARAMETROS  : 
    /// CREO        : Yazmin Abigail Delgado Gómez
    /// FECHA_CREO  : 24-Octubre-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Datos_Impresion_Cierre_Turno()
    {
        String Ruta_Archivo = @Server.MapPath("../Rpt/Cajas/");//Obtiene la ruta en la cual será guardada el archivo
        String Nombre_Archivo = "Cierre_Turno_Dia" + Session.SessionID + Convert.ToString(String.Format("{0:ddMMMyyyHHmmss}", DateTime.Today)); //Obtiene el nombre del archivo que sera asignado al documento
        Cls_Ope_Pre_Caj_Detalles_Negocio Rs_Cierre_Turno = new Cls_Ope_Pre_Caj_Detalles_Negocio();
        Ds_Rpt_Ope_Caj_Pag_Gen Ds_Cierre_Turno = new Ds_Rpt_Ope_Caj_Pag_Gen();
        DataTable Dt_Cierre_General = new DataTable(); //Variable a conter los valores a pasar al reporte

        try
        {
            Rs_Cierre_Turno.P_Caja_Id = "Todas";
            Rs_Cierre_Turno.P_Fecha = Txt_Fecha_Apertura_Dia.Text;
            Rs_Cierre_Turno.P_Fecha_Final = Txt_Fecha_Apertura_Dia.Text;
            Dt_Cierre_General = Rs_Cierre_Turno.Consulta_Pagos_General();

            foreach (DataRow Registro in Dt_Cierre_General.Rows)
            {
                Registro["FECHA"] = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Txt_Fecha_Apertura_Dia.Text));
                Registro["FECHA_FINAL"] = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Txt_Fecha_Apertura_Dia.Text));
            }

            Dt_Cierre_General.TableName = "DT_Datos_Generales";

            Ds_Cierre_Turno.Clear();
            Ds_Cierre_Turno.Tables.Clear();
            Ds_Cierre_Turno.Tables.Add(Dt_Cierre_General.Copy());

            ReportDocument Reporte = new ReportDocument();
            Reporte.Load(Ruta_Archivo + "Rpt_Ope_Caj_Pagos_General_Enc.rpt");
            Reporte.SetDataSource(Ds_Cierre_Turno);

            DiskFileDestinationOptions m_crDiskFileDestinationOptions = new DiskFileDestinationOptions();

            Nombre_Archivo += ".pdf";
            Ruta_Archivo = @Server.MapPath("../../Reporte/");
            m_crDiskFileDestinationOptions.DiskFileName = Ruta_Archivo + Nombre_Archivo;

            ExportOptions Opciones_Exportacion = new ExportOptions();
            Opciones_Exportacion.ExportDestinationOptions = m_crDiskFileDestinationOptions;
            Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
            Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
            Reporte.Export(Opciones_Exportacion);

            Abrir_Ventana(Nombre_Archivo);
        }
        catch (Exception ex)
        {
            throw new Exception("Datos_Impresion_Cierre_Turno " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Abrir_Ventana
    ///DESCRIPCIÓN: Abre en otra ventana el archivo pdf
    ///PARÁMETROS : Nombre_Archivo: Guarda el nombre del archivo que se desea abrir
    ///                             para mostrar los datos al usuario
    ///CREO       : Yazmin A Delgado Gómez
    ///FECHA_CREO : 12-Octubre-2011
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************
    private void Abrir_Ventana(String Nombre_Archivo)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";
        try
        {
            Pagina = Pagina + Nombre_Archivo;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt",
            "window.open('" + Pagina + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Abrir_Ventana " + ex.Message.ToString(), ex);
        }
    }
    #endregion
    #endregion
    #region Grid
    protected void Grid_Cajas_Abiertas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Limpia_Controles();                             //Limpia todos los controles de la forma
            Grid_Cajas_Abiertas.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
            Llena_Grid_Cajas_Abiertas();                    //Carga las cajas abiertas que estan asignados a la página seleccionada
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    #endregion
    #region Operacion
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        String Fecha_Busqueda; //Obtiene la fecha de consulta que proporciono el usuario
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (!String.IsNullOrEmpty(Txt_Busqueda.Text))
            {
                Fecha_Busqueda = Txt_Busqueda.Text.ToString();
                Consulta_Datos_Generales_Cierre_Dia(); //Consulta los datos de la fecha proporcionada por el usuario
                if (String.IsNullOrEmpty(Txt_No_Cierre_Dia.Text))
                {
                    Txt_Busqueda.Text = Fecha_Busqueda;
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + No hay una apertura de un turno diario en la fecha proporcionada favor de verificar <br>";
                }
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Debe proporcionar la Fecha de la Apertura del Turno de día para poder consultar los datos <br>";
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (Btn_Modificar.ToolTip == "Modificar")
            {
                Habilitar_Controles("Modificar"); //Habilita los controles para la introducción de datos por parte del usuario
            }
            else
            {
                Cierre_Dia();                          //Cierra el turno del Día con todos los datos consultados y proporcionados por el usuario
                Txt_Busqueda.Text = Txt_Fecha_Apertura_Dia.Text;
                Habilitar_Controles("Inicial");        //Habilita los controles para la siguiente operación del usuario
                Consulta_Datos_Generales_Cierre_Dia(); //Consulta todos los datos del cierre de turno del día que desea consultar el empleado
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip == "Salir")
            {
                Session.Remove("Consulta_Cajas_Abiertas");
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Habilitar_Controles("Inicial");//Habilita los controles para la siguiente operación del usuario
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
