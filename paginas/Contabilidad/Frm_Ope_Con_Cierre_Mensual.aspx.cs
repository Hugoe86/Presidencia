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
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Cierre_Mensual.Negocio;
using Presidencia.Parametros_Contabilidad.Negocio;

public partial class paginas_Contabilidad_Frm_Cat_Con_Cierre_Mensual : System.Web.UI.Page
{
    #region (Page_Load)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Page_Load
        /// DESCRIPCION : Carga la configuración inicial de los controles de la página.
        /// CREO        : Salvador L. Rea Ayala
        /// FECHA_CREO  : 30/Septiembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
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
                    DateTime FECHA;
                    FECHA = DateTime.Now;
                    string nuevo = Convert.ToString(FECHA.Year);
                    Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                    ViewState["SortDirection"] = "ASC";
                    Cmb_Anio_Contable.SelectedValue = nuevo;
                    Llenar_Grid_Cierres_generales(nuevo);
                    Acciones();
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

    #region (Metodos)
        #region (Metodos Generales)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Inicializa_Controles
            /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
            ///               diferentes operaciones
            /// PARAMETROS  : 
            /// CREO        : Salvador L. Rea Ayala
            /// FECHA_CREO  : 30/Septiembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Inicializa_Controles()
            {
                try
                {
                    Habilitar_Controles("Inicial"); //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar
                    Limpia_Controles();             //Limpia los controles del forma
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message.ToString());
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Limpiar_Controles
            /// DESCRIPCION : Limpia los controles que se encuentran en la forma
            /// PARAMETROS  : 
            /// CREO        : Salvador L. Rea Ayala
            /// FECHA_CREO  : 30/Septiembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Limpia_Controles()
            {
                try
                {
                    
                    //Cmb_Mes_Contable.SelectedIndex = -1;
                    Cmb_Anio_Contable.SelectedIndex = -1;
                    
                }
                catch (Exception ex)
                {
                    throw new Exception("Limpia_Controles " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Habilitar_Controles
            /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
            ///                para a siguiente operación
            /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
            ///                           si es una alta, modificacion
            ///                           
            /// CREO        : Salvador L. Rea Ayala
            /// FECHA_CREO  : 30/Septiembre/2011
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
                            Habilitado = true;
                            Btn_Salir.ToolTip = "Salir";
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                            Configuracion_Acceso("Frm_Ope_Con_Cierre_Mensual.aspx");
                            break;
                    }
                    Cmb_Anio_Contable.Enabled = Habilitado;
                    //Cmb_Mes_Contable.Enabled = Habilitado;
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                }
                catch (Exception ex)
                {
                    throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
                }
            }
        #endregion

        #region (Control Acceso Pagina)
            ///*******************************************************************************
            /// NOMBRE: Configuracion_Acceso
            /// DESCRIPCIÓN: Habilita las operaciones que podrá realizar el usuario en la página.
            /// PARÁMETROS  :
            /// USUARIO CREÓ: Salvador L. Rea Ayala
            /// FECHA CREÓ  : 30/Septiembre/2011
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
                    //Botones.Add(Btn_Nuevo);

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
                        if (!String.IsNullOrEmpty(Request.QueryString["Accion"])) { 
                        }
                        else
                        {
                            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                        }
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
            /// USUARIO CREÓ: Salvador L. Rea Ayala
            /// FECHA CREÓ  : 30/Septiembre/2011
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

        #region (Metodos Validacion)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Validar_Datos_Cierre
            /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
            /// CREO        : Salvador L. Rea Ayala
            /// FECHA_CREO  : 30/Septiembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private Boolean Validar_Datos_Cierre()
            {
                String Espacios_Blanco;
                Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
                Lbl_Mensaje_Error.Text = "Es necesario seleccionar: <br>";
                Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";

                //if (Cmb_Mes_Contable.SelectedIndex == 0)
                //{
                //    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + El mes contable que desea cerrar. <br>";
                //    Datos_Validos = false;
                //}
                if (Cmb_Anio_Contable.SelectedIndex == 0)
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + " + El año del mes que desea cerrar. <br>";
                    Datos_Validos = false;
                }
                return Datos_Validos;
            }
        #endregion

        #region (Metodos Operacion)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Cierre_Mensual
            /// DESCRIPCION : Comienza la operacion del Cierre Mensual
            /// PARAMETROS  : 
            /// CREO        : Salvador L. Rea Ayala
            /// FECHA_CREO  : 15/Septiembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Cierre_Mensual(String Cmb_Mes_Contable)
            {
                Cls_Ope_Con_Cierre_Mensual_Negocio Rs_Cierre_Mensual = new Cls_Ope_Con_Cierre_Mensual_Negocio();    //Objeto de acceso a los metodos.
                DataTable Dt_Cierre_Mensual = null;     //Almacenara los resultados a calcular.
                DataTable Dt_Cuentas = null;            //Almacenara las cuentas que tuvieron movimientos en el mes.
                DataTable Dt_Cuentas_Cierre_Mensual = null; //Almacenara las cuentas existentes de un determinado cierre mensual.
                DataTable Dt_Movimientos = new DataTable();     //Almacenara los resultados de los movimientos de cada una de las cuentas.
                DataTable Dt_Saldo_Inicial = null;      //Almacenara el saldo final del cierre mensual anterior.                
                Double  Debe = 0;
                Double Haber = 0;
                DateTime mes;
                mes = Convert.ToDateTime(String.Format("{0:dd/MMMM/yy}", Convert.ToDateTime("01/" + Cmb_Mes_Contable.Substring(0, 3) + "/" + Cmb_Anio_Contable.SelectedItem.Text.Substring(2, 2))));
                Cmb_Mes_Contable = String.Format("{0:MM}", mes).ToUpper();
                try 
                {
                    Dt_Movimientos.Columns.Add("Cuenta_Contable_ID", typeof(string));
                    Dt_Movimientos.Columns.Add("Debe", typeof(int));
                    Dt_Movimientos.Columns.Add("Haber", typeof(int));
                    Rs_Cierre_Mensual.P_Mes_Anio = Cmb_Mes_Contable + "" + Cmb_Anio_Contable.SelectedItem.Text.Substring(2, 2);
                    Dt_Cuentas = Rs_Cierre_Mensual.Cuentas_Contables_Afectables();
                    Dt_Cierre_Mensual = Rs_Cierre_Mensual.Cierre_Mensual();

                    //llENA EL DATA TABLE CON EL TOTAL HABER, TOTAL DEBE DE LAS CUENTAS QUE SUFRIERON MOVIMIENTOS EN EL MES
                    for (int Cont_Cuentas = 0; Cont_Cuentas < Dt_Cuentas.Rows.Count; Cont_Cuentas++)
                    {
                        for (int Cont_Cierre_Mensual = 0; Cont_Cierre_Mensual < Dt_Cierre_Mensual.Rows.Count; Cont_Cierre_Mensual++)
                        {
                            if (Dt_Cuentas.Rows[Cont_Cuentas][0].ToString() == Dt_Cierre_Mensual.Rows[Cont_Cierre_Mensual][0].ToString())
                            {
                                Debe += Convert.ToDouble(Dt_Cierre_Mensual.Rows[Cont_Cierre_Mensual][1].ToString());
                                Haber += Convert.ToDouble(Dt_Cierre_Mensual.Rows[Cont_Cierre_Mensual][2].ToString());
                                
                            }
                        }
                        Dt_Movimientos.Rows.Add(Convert.ToString(Dt_Cuentas.Rows[Cont_Cuentas]["Cuenta_Contable_ID"].ToString()), Debe, Haber);
                        Debe = Haber = 0;
                    }
                    Rs_Cierre_Mensual.P_Mes_Anio = Cmb_Mes_Contable + "" + Cmb_Anio_Contable.SelectedItem.Text.Substring(2, 2);
                    Dt_Cuentas_Cierre_Mensual = Rs_Cierre_Mensual.Consulta_Cierre_Mensual();
                    if (Dt_Cuentas_Cierre_Mensual.Rows.Count > 0)
                    {
                        Rs_Cierre_Mensual.Limpiar_Cierre_Mensual();
                    }
                    for (int Cont_Cuentas = 0; Cont_Cuentas < Dt_Cuentas.Rows.Count - 1; Cont_Cuentas++)
                    {
                            Rs_Cierre_Mensual.P_Cuenta_Contable_ID = Dt_Movimientos.Rows[Cont_Cuentas][0].ToString();
                            Rs_Cierre_Mensual.P_Fecha_Inicio = String.Format("{0:dd/MM/yy}", Convert.ToDateTime( Cmb_Mes_Contable +"/01/" + Rs_Cierre_Mensual.P_Mes_Anio.Substring(2, 2)));
                            Rs_Cierre_Mensual.P_Fecha_Final = String.Format("{0:dd/MM/yy}", DateTime.Now);
                            Dt_Saldo_Inicial = Rs_Cierre_Mensual.Saldo_Inicial_Cierre_Mensual();
                            if (Dt_Saldo_Inicial.Rows.Count == 0)
                                Rs_Cierre_Mensual.P_Saldo_Inicial = "0";
                            else
                                Rs_Cierre_Mensual.P_Saldo_Inicial = "" + Dt_Saldo_Inicial.Rows[0][0].ToString();

                            Rs_Cierre_Mensual.P_Total_Debe = Dt_Movimientos.Rows[Cont_Cuentas][1].ToString();
                            Rs_Cierre_Mensual.P_Total_Haber = Dt_Movimientos.Rows[Cont_Cuentas][2].ToString();
                            Rs_Cierre_Mensual.P_Saldo_Final = Convert.ToString(Convert.ToDouble(Rs_Cierre_Mensual.P_Saldo_Inicial) + Convert.ToDouble(Rs_Cierre_Mensual.P_Total_Haber) - Convert.ToDouble(Rs_Cierre_Mensual.P_Total_Debe));
                            Rs_Cierre_Mensual.P_Diferencia = "" + (Convert.ToDouble(Rs_Cierre_Mensual.P_Total_Haber) - Convert.ToDouble(Rs_Cierre_Mensual.P_Total_Debe));
                            Rs_Cierre_Mensual.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
                            Rs_Cierre_Mensual.Cierre_Mensual_Alta();                   
                    }
                  
                }
                catch (Exception ex)
                {
                    throw new Exception("Cierre_Mensual" + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Cmb_Anio_Contable_SelectedIndexChanged
            /// DESCRIPCION : Al seleccionar un item automaticamente llenar el grid de cierre mensual
            /// CREO        : Sergio Manuel Gallardo Andrade
            /// FECHA_CREO  : 7/Noviembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            protected void Cmb_Anio_Contable_SelectedIndexChanged(object sender, EventArgs e)
            {
               string anio_seleccionado=Cmb_Anio_Contable.SelectedValue.ToString();
               Llenar_Grid_Cierres_generales(anio_seleccionado);
            }
    // ****************************************************************************************
    //'NOMBRE DE LA FUNCION:Accion
    //'DESCRIPCION : realiza la modificacion del mes 
    //'PARAMETROS  : 
    //'CREO        : Sergio Manuel Gallardo
    //'FECHA_CREO  : 07/Noviembre/2011 12:12 pm
    //'MODIFICO          :
    //'FECHA_MODIFICO    :
    //'CAUSA_MODIFICACION:
    //'****************************************************************************************
    protected  void Acciones(){
        String Accion   = String.Empty;
        String mes  = String.Empty;
        String anio  = String.Empty;
        DataTable Dt_Revisar =null;
        Cls_Ope_Con_Cierre_Mensual_Negocio Rs_Cierre_Mensual = new Cls_Ope_Con_Cierre_Mensual_Negocio();    //Objeto de acceso a los metodos.
        if ( Request.QueryString["Accion"] != null){
            Accion = HttpUtility.UrlDecode(Request.QueryString["Accion"].ToString());
            if (Request.QueryString["id"] != null){
                mes = HttpUtility.UrlDecode(Request.QueryString["id"].ToString());
            }
            if (Request.QueryString["x"] != null){
                anio = HttpUtility.UrlDecode(Request.QueryString["x"].ToString());
            }
            //Response.Clear()
            switch(Accion){
                case "Cerrar_Mes":
                    Cierre_Mensual(mes.ToString());
                    Rs_Cierre_Mensual.P_Mes = mes;
                    Rs_Cierre_Mensual.P_Anio = anio;
                    Rs_Cierre_Mensual.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado.ToString();
                    Dt_Revisar = Rs_Cierre_Mensual.Consulta_Cierre_General();
                    if (Dt_Revisar.Rows.Count > 0)
                    {
                        Rs_Cierre_Mensual.P_Usuario_Modifico  = Cls_Sessiones.Nombre_Empleado.ToString();
                        Rs_Cierre_Mensual.P_Estatus = "CERRADO";
                        Rs_Cierre_Mensual.Modifica_Cierre_Mensual();
                    }
                    else {
                        Rs_Cierre_Mensual.Alta_Cierre_Mensual_General();
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Cierre Mensual", "alert('El Cierre se realizo satisfactoriamente ');", true); 
                    break;
                case "Abrir_Mes":
                    Rs_Cierre_Mensual.P_Mes = mes;
                    Rs_Cierre_Mensual.P_Anio = anio;
                    Rs_Cierre_Mensual.Abrir_Cierre_Mensual();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Cierre Mensual", "alert('El Cierre se realizo satisfactoriamente ');", true); 
                    break;

            }
        }
        }
        #endregion
    #endregion

    #region (Eventos)
        //protected void Btn_Comenzar_Click(object sender, EventArgs e)
        //{
        //    Cierre_Mensual();
        //}
        protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
        {

        }
    #endregion 

    #region (Grid)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Llenar_Grid_Cierres_generales
        /// DESCRIPCION : Llena el grid con los meses 
        /// PARAMETROS  : 
        /// CREO        : Sergio Manuel Gallardo Andrade
        /// FECHA_CREO  : 04/noviembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Llenar_Grid_Cierres_generales(String anio)
        {
            try
            {
                Cls_Ope_Con_Cierre_Mensual_Negocio Rs_Cierre_Mensual = new Cls_Ope_Con_Cierre_Mensual_Negocio();  
                DataRow Registro;
                DataTable Dt_Resultado = new DataTable();
                DateTime fecha = Convert.ToDateTime("01/01/2000");
                DataTable Dt_Mes = new DataTable();  //Variable que obtendra los datos de la consulta 
                // se asignas las columnas al datatable
                Dt_Mes.Columns.Add("Mes", typeof(string));
                Dt_Mes.Columns.Add("Anio", typeof(string));
                Dt_Mes.Columns.Add("Estatus", typeof(string));
                Dt_Mes.Columns.Add("Usuario_Creo", typeof(string));
                Dt_Mes.Columns.Add("Fecha_Creo", typeof(string));
                // se crea el detalle por default
                for (int Cont_Cuentas = 1; Cont_Cuentas <= 12; Cont_Cuentas++) {
                    Registro = Dt_Mes.NewRow();
                    Registro["Mes"] = String.Format("{0:MMMM}", fecha).ToUpper();// se le da formato a la fecha que se regresara 
                    Registro["Anio"] = anio;
                    Registro["Estatus"] ="ABIERTO" ;
                    Registro["Usuario_Creo"] = "";
                    Registro["Fecha_Creo"] = "";
                    Dt_Mes.Rows.Add(Registro);
                    Dt_Mes.AcceptChanges();
                    fecha = fecha.AddMonths(1);// se le suma un mes a la fecha 
                }
                // se consulta la tabla de cierre mensual general para obtener los meses cerrados en el año 
                fecha = Convert.ToDateTime("01/01/2000");
                Rs_Cierre_Mensual.P_Anio = anio;
                Rs_Cierre_Mensual.P_Mes = null;
                Dt_Resultado = Rs_Cierre_Mensual.Consulta_Cierre_General();
                foreach (DataRow Dr in Dt_Mes.Rows) {
                    // se aplica un filtro al datatable por mes para que solo mande el registro del mes que se obtubo
                    Dt_Resultado.DefaultView.RowFilter = "Mes ='" + String.Format("{0:MMMM}", fecha).ToUpper()+"'";
                    if (Dt_Resultado.DefaultView.ToTable().Rows.Count > 0)
                    {
                        Dr.BeginEdit();
                        Dr["Mes"] = Dt_Resultado.DefaultView.ToTable().Rows[0]["Mes"].ToString();
                        Dr["Anio"] = Dt_Resultado.DefaultView.ToTable().Rows[0]["Anio"].ToString();
                        Dr["Estatus"] = Dt_Resultado.DefaultView.ToTable().Rows[0]["Estatus"].ToString();
                        if (Dt_Resultado.DefaultView.ToTable().Rows[0]["Usuario_Modifico"].ToString() != "")
                        {
                            Dr["Usuario_Creo"] = Dt_Resultado.DefaultView.ToTable().Rows[0]["Usuario_Modifico"].ToString();
                            Dr["Fecha_Creo"] = Dt_Resultado.DefaultView.ToTable().Rows[0]["Fecha_Modifico"].ToString();
                        }
                        else {
                            Dr["Usuario_Creo"] = Dt_Resultado.DefaultView.ToTable().Rows[0]["Usuario_Creo"].ToString();
                            Dr["Fecha_Creo"] = Dt_Resultado.DefaultView.ToTable().Rows[0]["Fecha_Creo"].ToString();
                        }

                        Dr.EndEdit();
                        Dr.AcceptChanges();
                        Dt_Mes.AcceptChanges();
                    }
                    fecha = fecha.AddMonths(1);
                }
                Grid_Cierres_Mensuales.Columns[7].Visible = true;
                Grid_Cierres_Mensuales.DataSource = Dt_Mes;   // Se iguala el DataTable con el Grid
                Grid_Cierres_Mensuales.DataBind();    // Se ligan los datos.
                Grid_Cierres_Mensuales.Columns[7].Visible = false;
            }
            catch (Exception ex)
            {
                throw new Exception("Llena_Grid_Meses estatus " + ex.Message.ToString(), ex);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Grid_Cierres_Mensuales_SelectedIndexChanged
        /// DESCRIPCION : 
        /// CREO        : Sergio Manuel Gallardo Andrade
        /// FECHA_CREO  : 09/Noviembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Grid_Cierres_Mensuales_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cls_Ope_Con_Cierre_Mensual_Negocio Rs_Cierre_Mensual = new Cls_Ope_Con_Cierre_Mensual_Negocio();
            DataTable Dt_Bitacora;
            try
            {
                if (Grid_Cierres_Mensuales.SelectedRow.Cells[3].Text == "AFECTADO") 
                {
                    Rs_Cierre_Mensual.P_Anio = Grid_Cierres_Mensuales.SelectedRow.Cells[1].Text;
                    Rs_Cierre_Mensual.P_Mes = Grid_Cierres_Mensuales.SelectedRow.Cells[2].Text;
                    Dt_Bitacora = Rs_Cierre_Mensual.Consulta_Bitacora();
                    if (Dt_Bitacora.Rows.Count > 0)
                    {
                        Grid_Bitacora.DataSource = Dt_Bitacora;
                        Grid_Bitacora.DataBind();
                    }
                    else
                    {
                        Grid_Bitacora.DataSource = null;
                        Grid_Bitacora.DataBind();
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }

        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Grid_Cierres_Mensuales_RowDataBound
        /// DESCRIPCION : 
        /// CREO        : Salvador L. Rea Ayala
        /// FECHA_CREO  : 31/Octubre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Grid_Cierres_Mensuales_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                CheckBox chek = (CheckBox)e.Row.FindControl("Chk_Cerrar");
                CheckBox chek2 = (CheckBox)e.Row.FindControl("Chk_Abrir");
                if (e.Row.RowType == DataControlRowType.DataRow )
                {
                    if (e.Row.Cells[3].Text == "CERRADO")
                    {
                        chek.Checked = true;
                        chek.Enabled = false;
                        chek2.Checked = false;
                        chek2.Enabled = true;
                    }
                    else
                    {
                        chek.Checked = false;
                        chek.Enabled = true;
                        chek2.Checked = true;
                        chek2.Enabled = false;
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }
    #endregion
}
