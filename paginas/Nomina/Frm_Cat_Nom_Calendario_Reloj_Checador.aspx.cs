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
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Calendario_Reloj_Checador.Negocio;

public partial class paginas_Nomina_Frm_Cat_Nom_Calendario_Reloj_Checador : System.Web.UI.Page
{
    #region (Page Load)
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                    ViewState["SortDirection"] = "ASC";
                }
                Lbl_Mensaje_Error.Text = String.Empty;
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
    #endregion
        #region (Control Acceso Pagina)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Configuracion_Acceso
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
                Botones.Add(Btn_Buscar_Periodo_Nominal);

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
    #region (Metodos)
        #region (Metodos Generales)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Inicializa_Controles
            /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
            ///               diferentes operaciones
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 01-Septiembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Inicializa_Controles()
            {
                try
                {
                    Habilitar_Controles("Inicial"); //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar
                    Limpia_Controles(); //Limpia los controles del forma
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
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 01-Septiembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Limpia_Controles()
            {
                try
                {
                    Txt_Nomina_ID.Text = "";
                    Txt_Nomina.Text = "";
                    Txt_Busqueda_No_Nomina.Text = "";
                    Txt_Fecha_Inicio_Nomina.Text = "";
                    Txt_Fecha_Termino_Nomina.Text = "";
                    Grid_Calendario_Reloj_Checador.DataSource = new DataTable();
                    Grid_Calendario_Reloj_Checador.DataBind();
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
            ///                          si es una alta, modificacion
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 01-Septiembre-2011
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
                            Btn_Modificar.Visible = true;
                            Btn_Modificar.CausesValidation = false;
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                            Configuracion_Acceso("Frm_Cat_Nom_Calendario_Reloj_Checador.aspx");
                            break;

                        case "Modificar":
                            Habilitado = true;
                            Btn_Modificar.ToolTip = "Actualizar";
                            Btn_Salir.ToolTip = "Cancelar";
                            Btn_Modificar.Visible = true;
                            Btn_Modificar.CausesValidation = true;
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                            break;
                    }
                    Txt_Busqueda_No_Nomina.Enabled = !Habilitado;
                    Btn_Buscar_Periodo_Nominal.Enabled = !Habilitado;
                    Grid_Calendario_Reloj_Checador.Enabled = Habilitado;
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                }
                catch (Exception ex)
                {
                    throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
                }
            }      
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Validar_Datos
            /// DESCRIPCIÓN : Valida la información ingresada en el catálogo de calendario
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 02-Septiembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private Boolean Validar_Datos()
            {
                Boolean Estatus = true;
                Lbl_Mensaje_Error.Text = "Es necesario ingresar:<br />";

                try
                {
                    //Recorre el grid para poder asignar las fecha de inicio y termino del reloj checador
                    for (int Contador_Fila = 0; Contador_Fila < Grid_Calendario_Reloj_Checador.Rows.Count; Contador_Fila++)
                    {
                        if (String.IsNullOrEmpty(((TextBox)Grid_Calendario_Reloj_Checador.Rows[Contador_Fila].Cells[4].FindControl("Txt_Fecha_Inicio_Reloj_Checador")).Text.Trim().Replace("__/___/____", "")))
                        {
                            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Flantan Fechas que proporcionar <br>";
                            Estatus = false;
                            break;
                        }
                        if (String.IsNullOrEmpty(((TextBox)Grid_Calendario_Reloj_Checador.Rows[Contador_Fila].Cells[5].FindControl("Txt_Fecha_Termino_Reloj_Checador")).Text.Trim().Replace("__/___/____", "")))
                        {
                            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Flantan Fechas que proporcionar <br>";
                            Estatus = false;
                            break;
                        }
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al validar los datos. Error: [" + Ex.Message + "]");
                }
                return Estatus;
            }
        #endregion
        #region (Alta-Modificacion-Eliminacion-Consulta)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Calendario_Reloj_Checador
            /// DESCRIPCION : Consulta los datos del calendario nominal y del reloj checador
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 02-Septiembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Calendario_Reloj_Checador()
            {
                Cls_Cat_Nom_Calendario_Reloj_Checador_Negocio Rs_Consulta_Cat_Nom_Calendario_Reloj = new Cls_Cat_Nom_Calendario_Reloj_Checador_Negocio(); //Variable de conexión hacia la capa de Negocios
                DataTable Dt_Calendarios;              //Variable que obtendra los datos de la consulta 
                DateTime Fecha_Reloj = DateTime.Today; //Asigna la fecha a contener el reloj checador

                try
                {
                    Rs_Consulta_Cat_Nom_Calendario_Reloj.P_Anio = Convert.ToInt16(Txt_Busqueda_No_Nomina.Text);
                    Dt_Calendarios = Rs_Consulta_Cat_Nom_Calendario_Reloj.Consulta_Datos_Calendario_Nominal();   //Consulta los datos generales del calendario nominal de acuerdo al año proporcionado por el usuario
                    Limpia_Controles();//Limpia los controles de la forma para poder asiganar los nuevos valores de acuerdo a los obtenidos durante la consulta
                    if (Dt_Calendarios.Rows.Count > 0)
                    {
                        //Si se obtuvieron valores entonces agrega las fechas de inicio y termino para el calendario del reloj checador
                        if (Dt_Calendarios.Rows.Count > 0)
                        {
                            //Recorre el datatable para poder indicar que fechas de inicio y termino se consideran para el reloj checador
                            foreach (DataRow Registro_Calendario in Dt_Calendarios.Rows)
                            {
                                Txt_Nomina_ID.Text = Registro_Calendario[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID].ToString();
                                Txt_Nomina.Text = Registro_Calendario[Cat_Nom_Calendario_Nominas.Campo_Anio].ToString();
                                Txt_Fecha_Inicio_Nomina.Text =String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro_Calendario[Cat_Nom_Calendario_Nominas.Campo_Fecha_Inicio].ToString()));
                                Txt_Fecha_Termino_Nomina.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro_Calendario[Cat_Nom_Calendario_Nominas.Campo_Fecha_Fin].ToString()));
                            }
                            Dt_Calendarios = new DataTable();
                            Rs_Consulta_Cat_Nom_Calendario_Reloj.P_Nomina_ID = Txt_Nomina_ID.Text.ToString();
                            Dt_Calendarios = Rs_Consulta_Cat_Nom_Calendario_Reloj.Consulta_Calenadario_Reloj_Checador(); //Consulta todos los periodos del calendario con sus datos generales
                            //Agrega los valores de la consulta al grid
                            Grid_Calendario_Reloj_Checador.Columns[0].Visible = true;
                            Grid_Calendario_Reloj_Checador.DataSource = Dt_Calendarios;
                            Grid_Calendario_Reloj_Checador.DataBind();
                            Grid_Calendario_Reloj_Checador.Columns[0].Visible = false;
                            //Recorre el grid para poder asignar las fecha de inicio y termino del reloj checador
                            for (int Contador_Fila = 0; Contador_Fila < Grid_Calendario_Reloj_Checador.Rows.Count; Contador_Fila++)
                            {
                                //Recorre el datatable para poder indicar que fechas de inicio y termino se consideran para el reloj checador
                                foreach (DataRow Renglon in Dt_Calendarios.Rows)
                                {
                                    if (Renglon["No_Nomina"].ToString().Equals(Grid_Calendario_Reloj_Checador.Rows[Contador_Fila].Cells[1].Text))
                                    {
                                        //Si la fecha de inicio del reloj se dio de alta entonces asígna a las caja de texto correspondientes los valores
                                        //que fueron proporcionados
                                        if (!String.IsNullOrEmpty(Renglon["Fecha_Inicio_Reloj"].ToString()))
                                        {
                                            ((TextBox)Grid_Calendario_Reloj_Checador.Rows[Contador_Fila].Cells[4].FindControl("Txt_Fecha_Inicio_Reloj_Checador")).Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Renglon["Fecha_Inicio_Reloj"].ToString()));
                                            ((TextBox)Grid_Calendario_Reloj_Checador.Rows[Contador_Fila].Cells[5].FindControl("Txt_Fecha_Termino_Reloj_Checador")).Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Renglon["Fecha_Fin_Reloj"].ToString()));
                                        }
                                        //Si la fecha de inicio aun no ha sido dada de alta entonces quita 8 días a la fecha de inicio y termino del calendario nominal
                                        else
                                        {
                                            Fecha_Reloj = Convert.ToDateTime(Renglon["Fecha_Inicio_Nomina"].ToString()).AddDays((-8));
                                            ((TextBox)Grid_Calendario_Reloj_Checador.Rows[Contador_Fila].Cells[4].FindControl("Txt_Fecha_Inicio_Reloj_Checador")).Text = String.Format("{0:dd/MMM/yyyy}", Fecha_Reloj);
                                            Fecha_Reloj = Convert.ToDateTime(Renglon["Fecha_Fin_Nomina"].ToString()).AddDays((-8));
                                            ((TextBox)Grid_Calendario_Reloj_Checador.Rows[Contador_Fila].Cells[5].FindControl("Txt_Fecha_Termino_Reloj_Checador")).Text = String.Format("{0:dd/MMM/yyyy}", Fecha_Reloj);
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Consulta_Calendario_Reloj_Checador " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Modificar_Calendario_Reloj_Checador
            /// DESCRIPCION : Modifica las fechas del reloj checador por las que fueron
            ///               introducidas por el usuario
            /// PARAMETROS  : 
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 09-Septiembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Modificar_Calendario_Reloj_Checador()
            {
                DataTable Dt_Calendario_Reloj = new DataTable(); //Variable a contener los datos necesarios para poder dar de alta las fechas del mismo
                Cls_Cat_Nom_Calendario_Reloj_Checador_Negocio Rs_Modificar_Cat_Nom_Calendario_Reloj = new Cls_Cat_Nom_Calendario_Reloj_Checador_Negocio(); //Variable de conexión hacia la capa de negocios
                String Fecha_Inicio_Reloj = ""; //Obtiene la fecha de inicio a considerar para el reloj checador
                String Fecha_Fin_Reloj = "";    //Obtiene la fecha de termino a considerar para el reloj checador

                try
                {
                    //Crea la tabla con los registros y sus tipos a contener para asignación de valores del grid
                    Dt_Calendario_Reloj.Columns.Add("Nomina_ID", typeof(String));
                    Dt_Calendario_Reloj.Columns.Add("No_Nomina", typeof(String));
                    Dt_Calendario_Reloj.Columns.Add("Fecha_Inicio", typeof(DateTime));
                    Dt_Calendario_Reloj.Columns.Add("Fecha_Fin", typeof(DateTime));
                    Dt_Calendario_Reloj.Columns.Add("Estatus", typeof(String));

                    Grid_Calendario_Reloj_Checador.Columns[0].Visible = true;
                    //Agrega los valores del grid al DataTable para poder asignarlos a la variable para el alta de los valores
                    for (int Cont_Fila = 0; Cont_Fila < Grid_Calendario_Reloj_Checador.Rows.Count; Cont_Fila++)
                    {
                        DataRow Renglon = Dt_Calendario_Reloj.NewRow(); //Agrega un nuevo registro con todos sus valores
                        Renglon["Nomina_ID"] = Grid_Calendario_Reloj_Checador.Rows[Cont_Fila].Cells[0].Text.ToString();
                        Renglon["No_Nomina"] = Grid_Calendario_Reloj_Checador.Rows[Cont_Fila].Cells[1].Text.ToString();
                        Fecha_Inicio_Reloj = ((TextBox)Grid_Calendario_Reloj_Checador.Rows[Cont_Fila].Cells[4].FindControl("Txt_Fecha_Inicio_Reloj_Checador")).Text;
                        Fecha_Fin_Reloj = ((TextBox)Grid_Calendario_Reloj_Checador.Rows[Cont_Fila].Cells[5].FindControl("Txt_Fecha_Termino_Reloj_Checador")).Text;
                        Renglon["Fecha_Inicio"] =String.Format("{0:MMM/dd/yyyy}", Convert.ToDateTime(Fecha_Inicio_Reloj));
                        Renglon["Fecha_Fin"] =String.Format("{0:MMM/dd/yyyy}", Convert.ToDateTime(Fecha_Fin_Reloj));
                        Renglon["Estatus"] = Grid_Calendario_Reloj_Checador.Rows[Cont_Fila].Cells[6].Text.ToString();
                        Dt_Calendario_Reloj.Rows.Add(Renglon);
                    }
                    Grid_Calendario_Reloj_Checador.Columns[0].Visible = false;
                    Rs_Modificar_Cat_Nom_Calendario_Reloj.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
                    Rs_Modificar_Cat_Nom_Calendario_Reloj.P_Dt_Calendario_Reloj = Dt_Calendario_Reloj;   //Pasa los valores a dar de alta/modificar a la capa de negocios
                    Rs_Modificar_Cat_Nom_Calendario_Reloj.Alta_Modificacion_Calendario_Reloj_Checador(); //Da de Alta/Modifica los valores que fueron introducidos por el usuario

                    Habilitar_Controles("Inicial"); //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones            
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Calendario de Reloj Checador", "alert('La Modificación del Calendario del Reloj Checador fue Exitosa');", true);

                }
                catch (Exception ex)
                {
                    throw new Exception("Modificar_Calendario_Reloj_Checador " + ex.Message.ToString(), ex);
                }
            }
        #endregion
    #endregion
    #region (Grid)
        protected void Grid_Calendario_Reloj_Checador_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[4].Enabled = true;
            e.Row.Cells[5].Enabled = true;
        }
    #endregion
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        { 
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (Btn_Modificar.ToolTip == "Modificar")
            {
                if (!String.IsNullOrEmpty(Txt_Nomina_ID.Text))
                {
                    Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Consulte el periodo fiscal que desea modificar <br>";
                }
            }
            else
            {
                //Valida que todas las fechas de los diferentes periodos esten asignadas
                if (Validar_Datos())
                {
                    Modificar_Calendario_Reloj_Checador(); //Dar de alta o modifica los valores de las fechas de los periodos nominales del reloj checador
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
        }
        catch(Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip == "Salir")
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Inicializa_Controles();//Habilita los controles para la siguiente operación del usuario en el catálogo
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    protected void Btn_Buscar_Periodo_Nominal_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            if (!String.IsNullOrEmpty(Txt_Busqueda_No_Nomina.Text.ToString()))
            {
                //Si el usuario solo propociono los últimos 2 digitos del año entonces concatena al año 20 para poder obtener el año correctamente
                if (Convert.ToInt16(Txt_Busqueda_No_Nomina.Text.ToString().Length) == 2) Txt_Busqueda_No_Nomina.Text = "20" + Txt_Busqueda_No_Nomina.Text.ToString();
                if (Convert.ToInt16(Txt_Busqueda_No_Nomina.Text.ToString().Length) == 4)
                {
                    if (Convert.ToInt16(Txt_Busqueda_No_Nomina.Text.ToString()) <= Convert.ToInt16(DateTime.Today.AddYears(1).Year) && Convert.ToInt16(Txt_Busqueda_No_Nomina.Text.ToString()) >= 2011)
                    {
                        Consulta_Calendario_Reloj_Checador(); //Consulta todos los datos del reloj checador
                        //Si no se encontraron Tipos de Nómina con el Nombre similar proporcionado por el usuario entonces manda un mensaje al usuario
                        if (Grid_Calendario_Reloj_Checador.Rows.Count <= 0)
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "No se encontraron Periodos nominales con el Año proporcionado <br>";
                        }
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "El año proporcionado para la consulta de los periodos no es valido, favor de verificar <br>";
                    }
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No es correcto el formato del año proporcionado debe estar a 4 cifras <br>";
                }
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Proporcione el Año que desea consultar el periodo nominal <br>";
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
}
