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
using System.Data.OracleClient;
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Paramentros_Presupuestos.Negocio;
using Presidencia.Presupuesto_Cierre_Presupuestal.Negocio;

public partial class paginas_Presupuestos_Frm_Ope_Cierre_Presupuestal : System.Web.UI.Page
{
    #region(Load)
        protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //Refresca la sesion del usuario logeado en el sistema
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            //Valida que existe un usuario logueado en el sistema
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                ViewState["SortDirection"] = "ASC";
                Session["Session_Movimientos_Presupuesto"] = null;

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

    #region(Metodos)
        #region (Metodos Generales)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Inicializa_Controles
            /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda
            ///               realizar diferentes operaciones
            /// PARAMETROS  : 
            /// CREO        : Hugo Enrique Ramírez Aguilera
            /// FECHA_CREO  : 15/Noviembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Inicializa_Controles()
            {
                try
                {
                    Limpiar_Controles(); //Limpia los controles del forma
                    Habilitar_Controles("Inicial");//Inicializa todos los controles
                    Habilitar_Visible(false);
                    Cargar_Combo_Anio();
                    Cargar_Grid();
                   
                    //Consulta_Movimiento_Presupuestal();//busca toda la informacion de las operaciones en la basae de datos
                }
                catch (Exception ex)
                {
                    throw new Exception("Inicializa_Controles " + ex.Message.ToString());
                }
            }

            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Limpiar_Controles
            /// DESCRIPCION : Limpia los controles que se encuentran en la forma
            /// PARAMETROS  : 
            /// CREO        : Hugo Enrique Ramírez Aguilera
            /// FECHA_CREO  : 15/Noviembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Limpiar_Controles()
            {
                try
                {
                    Cmb_Anio.SelectedIndex = 0;
                    Txt_Busqueda.Text = "";
                }
                catch (Exception ex)
                {
                    throw new Exception("Limpia_Controles " + ex.Message.ToString());
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Habilitar_Controles
            /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
            ///               para a siguiente operación
            /// PARAMETROS  : 1.- Operacion: Indica la operación que se desea realizar por parte del usuario
            ///               si es una alta, modificacion
            ///                           
            /// CREO        : Hugo Enrique Ramirez Aguilera
            /// FECHA_CREO  : 15/Noviembre/2011
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
                            Btn_Modificar.Visible = false;


                            Btn_Modificar.CausesValidation = false;

                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                            Configuracion_Acceso("Frm_Ope_Cierre_Presupuestal.aspx");
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
                    Grid_Cierre_Presupuestal.Enabled = Habilitado;
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                }
                catch (Exception ex)
                {
                    throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
                }
            }
     ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Habilitar_Visible
            /// DESCRIPCION : hace visibles a las cajas de texto y etiquetas
            /// PARAMETROS  : 1.- Boolean Habilitado  Pasa el valor de ture o false 
            /// CREO        : Hugo Enrique Ramirez Aguilera
            /// FECHA_CREO  : 15/Noviembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Habilitar_Visible(Boolean Habilitado)
            {
                try
                {
                    Btn_Modificar.Visible = Habilitado;
                }
                catch (Exception ex)
                {
                    throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
                }
            }

        #endregion

        #region(Control Acceso Pagina)
            /// ******************************************************************************
            /// NOMBRE: Configuracion_Acceso
            /// DESCRIPCIÓN: Habilita las operaciones que podrá realizar el usuario en la página.
            /// PARÁMETROS  :
            /// USUARIO CREÓ: Hugo Enrique Ramirez Aguilera
            /// FECHA CREÓ  : 15/Noviembre/2011
            /// USUARIO MODIFICO  :
            /// FECHA MODIFICO    :
            /// CAUSA MODIFICACIÓN:
            /// ******************************************************************************
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
            /// NOMBRE DE LA FUNCION: Es_Numero
            /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
            /// PARÁMETROS  : Cadena.- El dato a evaluar si es numerico.
            /// CREO        : Hugo Enrique Ramirez Aguilera
            /// FECHA_CREO  : 15/Noviembre/2011
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

        #region (Método Consulta)
        #endregion

        #region(Validacion)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Validar_Movimiento_Presupuestal
            /// DESCRIPCION : Validar que se hallan proporcionado todos los datos
            /// CREO        : Hugo Enrique Ramirez Aguilera
            /// FECHA_CREO  : 15/Noviembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private Boolean Validar_Movimiento_Presupuestal()
            {
                return false;
            }
        #endregion
            #region (Metodos Operacion)
                ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Modificar_Cierre
            ///DESCRIPCIÓN: toma la informacion para poder realizar la modificacion
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  16/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Modificar_Cierre()
            {
                Cls_Ope_Psp_Cierre_Presupuestal_Negocio Modificar = new Cls_Ope_Psp_Cierre_Presupuestal_Negocio();  
                DropDownList Combo;
                try
                {
                    Combo = (DropDownList)Grid_Cierre_Presupuestal.Rows[0].Cells[1].FindControl("Cmb_Estatus");
                    Modificar.P_Enero = Combo.SelectedValue;
                    Combo = (DropDownList)Grid_Cierre_Presupuestal.Rows[1].Cells[1].FindControl("Cmb_Estatus");
                    Modificar.P_Febrero = Combo.SelectedValue;
                    Combo = (DropDownList)Grid_Cierre_Presupuestal.Rows[2].Cells[1].FindControl("Cmb_Estatus");
                    Modificar.P_Marzo = Combo.SelectedValue;
                    Combo = (DropDownList)Grid_Cierre_Presupuestal.Rows[3].Cells[1].FindControl("Cmb_Estatus");
                    Modificar.P_Abril = Combo.SelectedValue;
                    Combo = (DropDownList)Grid_Cierre_Presupuestal.Rows[4].Cells[1].FindControl("Cmb_Estatus");
                    Modificar.P_Mayo = Combo.SelectedValue;
                    Combo = (DropDownList)Grid_Cierre_Presupuestal.Rows[5].Cells[1].FindControl("Cmb_Estatus");
                    Modificar.P_Junio = Combo.SelectedValue;
                    Combo = (DropDownList)Grid_Cierre_Presupuestal.Rows[6].Cells[1].FindControl("Cmb_Estatus");
                    Modificar.P_Julio = Combo.SelectedValue;
                    Combo = (DropDownList)Grid_Cierre_Presupuestal.Rows[7].Cells[1].FindControl("Cmb_Estatus");
                    Modificar.P_Agosto = Combo.SelectedValue;
                    Combo = (DropDownList)Grid_Cierre_Presupuestal.Rows[8].Cells[1].FindControl("Cmb_Estatus");
                    Modificar.P_Septiembre = Combo.SelectedValue;
                    Combo = (DropDownList)Grid_Cierre_Presupuestal.Rows[9].Cells[1].FindControl("Cmb_Estatus");
                    Modificar.P_Octubre = Combo.SelectedValue;
                    Combo = (DropDownList)Grid_Cierre_Presupuestal.Rows[10].Cells[1].FindControl("Cmb_Estatus");
                    Modificar.P_Noviembre = Combo.SelectedValue;
                    Combo = (DropDownList)Grid_Cierre_Presupuestal.Rows[11].Cells[1].FindControl("Cmb_Estatus");
                    Modificar.P_Diciembre = Combo.SelectedValue;
                    Modificar.P_Usuario_Creo= Cls_Sessiones.Nombre_Empleado;;
                    Modificar.P_Anio = Cmb_Anio.SelectedValue;
                    Modificar.Modificar_Cierre_Presupuestal();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Cierre Presupuestal", "alert('Modificación Exitosa');", true);
                    Inicializa_Controles();
                }

                catch (Exception ex)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = ex.Message.ToString();
                }

            }
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Construir_Tabla_Meses
            ///DESCRIPCIÓN: Carga el datatable con los meses del año
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  16/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected DataTable Construir_Tabla_Meses()
            {
                DataTable Dt_Meses = new DataTable();
                try
                {
                    Dt_Meses.Columns.Add("Mes");
                    Dt_Meses.Columns.Add("Estatus");

                    DataRow Dr_Mes = Dt_Meses.NewRow();
                    Dr_Mes["Mes"] = "Enero";
                    Dt_Meses.Rows.Add(Dr_Mes);
                    Dr_Mes = Dt_Meses.NewRow();
                    Dr_Mes["Mes"] = "Febrero";
                    Dt_Meses.Rows.Add(Dr_Mes);
                    Dr_Mes = Dt_Meses.NewRow();
                    Dr_Mes["Mes"] = "Marzo";
                    Dt_Meses.Rows.Add(Dr_Mes);
                    Dr_Mes = Dt_Meses.NewRow();
                    Dr_Mes["Mes"] = "Abril";
                    Dt_Meses.Rows.Add(Dr_Mes);
                    Dr_Mes = Dt_Meses.NewRow();
                    Dr_Mes["Mes"] = "Mayo";
                    Dt_Meses.Rows.Add(Dr_Mes);
                    Dr_Mes = Dt_Meses.NewRow();
                    Dr_Mes["Mes"] = "Junio";
                    Dt_Meses.Rows.Add(Dr_Mes);
                    Dr_Mes = Dt_Meses.NewRow();
                    Dr_Mes["Mes"] = "Julio";
                    Dt_Meses.Rows.Add(Dr_Mes);
                    Dr_Mes = Dt_Meses.NewRow();
                    Dr_Mes["Mes"] = "Agosto";
                    Dt_Meses.Rows.Add(Dr_Mes);
                    Dr_Mes = Dt_Meses.NewRow();
                    Dr_Mes["Mes"] = "Septiembre";
                    Dt_Meses.Rows.Add(Dr_Mes);
                    Dr_Mes = Dt_Meses.NewRow();
                    Dr_Mes["Mes"] = "Octubre";
                    Dt_Meses.Rows.Add(Dr_Mes);
                    Dr_Mes = Dt_Meses.NewRow();
                    Dr_Mes["Mes"] = "Noviembre";
                    Dt_Meses.Rows.Add(Dr_Mes);
                    Dr_Mes = Dt_Meses.NewRow();
                    Dr_Mes["Mes"] = "Diciembre";
                    Dt_Meses.Rows.Add(Dr_Mes);


                }

                catch (Exception ex)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = ex.Message.ToString();
                }
                return Dt_Meses;
            }
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Estatus_Grid
            ///DESCRIPCIÓN: Carga el datatable con los estatus de cada meses del año
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  16/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected DataTable Llenar_Estatus_Grid(DataTable Dt_Consulta)
            {
                DataTable Dt_Meses = new DataTable();
                String Estatus;
                DropDownList Combo;
                try
                {
                    foreach (DataRow Registro in Dt_Consulta.Rows)
                    {
                        Estatus = (Registro[Ope_Psp_Cierre_Presup.Campo_Enero].ToString());
                        Combo = (DropDownList)Grid_Cierre_Presupuestal.Rows[0].Cells[1].FindControl("Cmb_Estatus");
                        Combo.SelectedValue = Estatus;

                        Estatus = (Registro[Ope_Psp_Cierre_Presup.Campo_Febrero].ToString());
                        Combo = (DropDownList)Grid_Cierre_Presupuestal.Rows[1].Cells[1].FindControl("Cmb_Estatus");
                        Combo.SelectedValue = Estatus;

                        Estatus = (Registro[Ope_Psp_Cierre_Presup.Campo_Marzo].ToString());
                        Combo = (DropDownList)Grid_Cierre_Presupuestal.Rows[2].Cells[1].FindControl("Cmb_Estatus");
                        Combo.SelectedValue = Estatus;

                        Estatus = (Registro[Ope_Psp_Cierre_Presup.Campo_Abril].ToString());
                        Combo = (DropDownList)Grid_Cierre_Presupuestal.Rows[3].Cells[1].FindControl("Cmb_Estatus");
                        Combo.SelectedValue = Estatus;

                        Estatus = (Registro[Ope_Psp_Cierre_Presup.Campo_Mayo].ToString());
                        Combo = (DropDownList)Grid_Cierre_Presupuestal.Rows[4].Cells[1].FindControl("Cmb_Estatus");
                        Combo.SelectedValue = Estatus;

                        Estatus = (Registro[Ope_Psp_Cierre_Presup.Campo_Junio].ToString());
                        Combo = (DropDownList)Grid_Cierre_Presupuestal.Rows[5].Cells[1].FindControl("Cmb_Estatus");
                        Combo.SelectedValue = Estatus;

                        Estatus = (Registro[Ope_Psp_Cierre_Presup.Campo_Julio].ToString());
                        Combo = (DropDownList)Grid_Cierre_Presupuestal.Rows[6].Cells[1].FindControl("Cmb_Estatus");
                        Combo.SelectedValue = Estatus;

                        Estatus = (Registro[Ope_Psp_Cierre_Presup.Campo_Agosto].ToString());
                        Combo = (DropDownList)Grid_Cierre_Presupuestal.Rows[7].Cells[1].FindControl("Cmb_Estatus");
                        Combo.SelectedValue = Estatus;

                        Estatus = (Registro[Ope_Psp_Cierre_Presup.Campo_Septiembre].ToString());
                        Combo = (DropDownList)Grid_Cierre_Presupuestal.Rows[8].Cells[1].FindControl("Cmb_Estatus");
                        Combo.SelectedValue = Estatus;

                        Estatus = (Registro[Ope_Psp_Cierre_Presup.Campo_Octubre].ToString());
                        Combo = (DropDownList)Grid_Cierre_Presupuestal.Rows[9].Cells[1].FindControl("Cmb_Estatus");
                        Combo.SelectedValue = Estatus;

                        Estatus = (Registro[Ope_Psp_Cierre_Presup.Campo_Noviembre].ToString());
                        Combo = (DropDownList)Grid_Cierre_Presupuestal.Rows[10].Cells[1].FindControl("Cmb_Estatus");
                        Combo.SelectedValue = Estatus;

                        Estatus = (Registro[Ope_Psp_Cierre_Presup.Campo_Diciembre].ToString());
                        Combo = (DropDownList)Grid_Cierre_Presupuestal.Rows[11].Cells[1].FindControl("Cmb_Estatus");
                        Combo.SelectedValue = Estatus;
                    }

                }

                catch (Exception ex)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = ex.Message.ToString();
                }
                return Dt_Meses;
            }
            #endregion
    #endregion

    #region(Eventos)
        #region(Botones)
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
            ///DESCRIPCIÓN: Habilita las cajas de texto necesarias para crear un Nuevo Movimiento
            ///             se convierte en dar alta cuando oprimimos Nuevo y dar alta  Crea un registro  
            ///                de un movimiento presupuestal en la base de datos
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  15/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
            {
                try
                {

                    
                   
                }

                catch (Exception ex)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = ex.Message.ToString();
                }

            }
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
            ///DESCRIPCIÓN: Habilita las cajas de texto necesarias para poder Modificar la informacion,
            ///             se convierte en actualizar cuando oprimimos Modificar y se actualiza el registro 
            ///             en la base de datos
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  15/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
            {
                try
                {
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    Lbl_Mensaje_Error.Text = "";

                   
                        if (Btn_Modificar.ToolTip == "Modificar")
                        {
                            Habilitar_Controles("Modificar"); //Habilita los controles para la introducción de datos por parte del usuario
                        }
                        else
                        {
                            Modificar_Cierre();
                        }
                    
                   
                    
                }
                catch (Exception ex)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = ex.Message.ToString();
                }
            }
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
            ///DESCRIPCIÓN: Elimina un registro de un Asunto de la base de datos
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  15/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
            {
                try
                {
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    Lbl_Mensaje_Error.Text = "";
                }
                catch (Exception ex)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = ex.Message.ToString();
                }
            }
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Movimiento_Presupuestal_Click
            ///DESCRIPCIÓN: Busca el movimiento y lo carga en el grid
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  15/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
            {
                DataTable Dt_Meses = new DataTable();//tabla temporal para cargar el grid
                DataTable Dt_Consulta = new DataTable();
                Cls_Ope_Psp_Cierre_Presupuestal_Negocio Consulta = new Cls_Ope_Psp_Cierre_Presupuestal_Negocio();
                try
                {
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    Lbl_Mensaje_Error.Text = "";
                    if (!string.IsNullOrEmpty(Txt_Busqueda.Text.Trim()))
                    {
                        Dt_Meses = Construir_Tabla_Meses();
                        //para la consulta por año
                        Consulta.P_Anio = Txt_Busqueda.Text;
                        Dt_Consulta = Consulta.Consultar_Estatus();

                        if (Dt_Consulta.Rows.Count > 0)
                        {
                            Cmb_Anio.SelectedValue = Txt_Busqueda.Text;
                            Grid_Cierre_Presupuestal.DataSource = Dt_Meses;
                            Grid_Cierre_Presupuestal.DataBind();
                            Grid_Cierre_Presupuestal.SelectedIndex = -1;
                            Llenar_Estatus_Grid(Dt_Consulta);
                        }
                        else
                        {
                            Inicializa_Controles();
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "No se encuentra registro con ese año";
                            
                        }
                    }
                }
                catch (Exception ex)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = ex.Message.ToString();
                }
            }
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
            ///DESCRIPCIÓN: Cancela la operacion actual qye se este realizando
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  15/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
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
                        Inicializa_Controles();
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

        #region(ComboBox)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Cargar_Combo_Anio
            /// DESCRIPCION: Consulta los Capítulos dados de alta en la base de datos
            ///PARAMETROS: 
            /// CREO: Hugo Enrique Ramírez Aguilera
            /// FECHA_CREO: 15/Noviembre/2011
            /// MODIFICO:
            /// FECHA_MODIFICO:
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Cargar_Combo_Anio()
            {
                DataTable Dt_Anio; //Variable que obtendrá los datos de la consulta 
                Cls_Cat_Psp_Parametros_Negocio Anio = new Cls_Cat_Psp_Parametros_Negocio();
                try
                {
                    Cmb_Anio.Items.Clear();
                    Dt_Anio = Anio.Consultar_Parametros();


                    Cmb_Anio.DataSource = Dt_Anio;
                    Cmb_Anio.DataValueField = Cat_Psp_Parametros.Campo_Anio_Presupuestar;
                    Cmb_Anio.DataTextField = Cat_Psp_Parametros.Campo_Anio_Presupuestar;
                    Cmb_Anio.DataBind();
                    Cmb_Anio.Items.Insert(0, "----- < SELECCIONE AÑO > -----");
                    Cmb_Anio.SelectedIndex = 0;
                    

                }
                catch (Exception ex)
                {
                    throw new Exception("Cargar_Combo_Capitulos " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Cmb_Anio_OnSelectedIndexChanged
            ///DESCRIPCIÓN: habilita el siguiente combo y pasa la informacion de la clave
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  08/Noviembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Cmb_Anio_OnSelectedIndexChanged(object sender, EventArgs e)
            {
                DataTable Dt_Meses = new DataTable();//tabla temporal para cargar el grid
                DataTable Dt_Consulta = new DataTable();
                Cls_Ope_Psp_Cierre_Presupuestal_Negocio Consulta = new Cls_Ope_Psp_Cierre_Presupuestal_Negocio();
                try
                {   
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    Lbl_Mensaje_Error.Text = "";
                    Grid_Cierre_Presupuestal.DataSource = null;
                    Grid_Cierre_Presupuestal.DataBind();

                    Dt_Meses= Construir_Tabla_Meses();
                    //para la consulta
                    if (Cmb_Anio.SelectedIndex == 0)
                    {
                        Inicializa_Controles();
                    }
                    else
                    {
                        Consulta.P_Anio = Cmb_Anio.SelectedValue;
                        Dt_Consulta = Consulta.Consultar_Estatus();

                        if (Dt_Consulta.Rows.Count > 0)
                        {
                            Grid_Cierre_Presupuestal.DataSource = Dt_Meses;
                            Grid_Cierre_Presupuestal.DataBind();
                            Grid_Cierre_Presupuestal.SelectedIndex = -1;
                            Llenar_Estatus_Grid(Dt_Consulta);
                            Habilitar_Visible(true);
                        }
                        else
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "No se encuentra registro con ese año";
                            Habilitar_Visible(false);
                        }
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
    #endregion

    #region(Grid)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Cargar_Grid
        /// DESCRIPCION: Consulta los Capítulos dados de alta en la base de datos
        ///PARAMETROS: 
        /// CREO: Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO: 16/Noviembre/2011
        /// MODIFICO:
        /// FECHA_MODIFICO:
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Cargar_Grid()
        {
            try
            {
                Session["Session_Cierre_Presupuestal"] = null;
                DataTable Dt_Temporal = new DataTable();
                Dt_Temporal = (DataTable)Session["Session_Cierre_Presupuestal"];
                Grid_Cierre_Presupuestal.DataSource = Dt_Temporal;
                Session["Session_Cierre_Presupuestal"] = Dt_Temporal;
                Grid_Cierre_Presupuestal.DataBind();
                Grid_Cierre_Presupuestal.SelectedIndex = -1;
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
