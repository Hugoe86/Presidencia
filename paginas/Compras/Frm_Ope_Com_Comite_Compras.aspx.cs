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
using Presidencia.Comite_Compras.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using System.Collections.Generic;

public partial class paginas_Compras_Frm_Ope_Com_Comite_Compras : System.Web.UI.Page
{

    ///*******************************************************************************
    /// VARIABLES
    ///*******************************************************************************
    #region Variables

    Cls_Ope_Com_Comite_Compras_Negocio Comite_Negocio;

    #endregion Fin_Variables

    ///*******************************************************************************
    /// REGION PAGE_LOAD
    ///*******************************************************************************
    #region Page_Load
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: 
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 31/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
  
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Init
    ///DESCRIPCIÓN: 
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 31/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Page_Init(object sender, EventArgs e)
    {
        Comite_Negocio = new Cls_Ope_Com_Comite_Compras_Negocio();
        DataTable Dt_Comite = Comite_Negocio.Consulta_Comite_Compras();
        if (Dt_Comite.Rows.Count != 0)
        {
            Estatus_Formulario("Inicial");
            Llenar_Grid_Comite();
        }
        else
        {
            Estatus_Formulario("General");
            
        }
    }


    #endregion 

    ///*******************************************************************************
    /// REGION METODOS
    ///*******************************************************************************
    #region Metodos
    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Estado_Formulario
    ///DESCRIPCIÓN: Metodo que permite cambiar las propiedades de los metodos y de los Div de acuerdo al estado que se indique
    ///PARAMETROS:1.- String Estatus: Indica el estado del formulario puede ser Inicia, General,Nuevo o Modificar
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 20/Enero/2011 
    ///MODIFICO: Juan Alberto Hernandez Negrete
    ///FECHA_MODIFICO: 30/Mayo/2011
    ///CAUSA_MODIFICACIÓN: Agrego validacion de accesos del sistema.
    ///*******************************************************************************
    public void Estatus_Formulario(String Estatus)
    {
        switch (Estatus)
        {
            case "Inicial":
                //Manejo de los Div
                Div_Busqueda.Visible = true;
                Div_Listado_Comite.Visible = true;
                Div_Comite_Compras.Visible = false;
                //Boton Nuevo
                Btn_Nuevo.Visible = true;
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                //Boton Modificar
                Btn_Modificar.Visible = false;
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Inicio";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Habilitar_Componentes(false);
                //Asignamos la fecha
                Txt_Fecha.Text = DateTime.Now.ToString("dd/MMM/yyyy");

                Configuracion_Acceso("Frm_Ope_Com_Comite_Compras.aspx");
                break;
            case "General":
                //Manejo de los Div
                Div_Busqueda.Visible =false;
                Div_Listado_Comite.Visible = false;
                Div_Comite_Compras.Visible = true;
                //Boton Nuevo
                Btn_Nuevo.Visible = true;
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                //Boton Modificar
                Btn_Modificar.Visible = true;
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Listado";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Llenar_Combo_Estatus();
                Llenar_Combo_Tipo();
                Habilitar_Componentes(false);
                //Asignamos la fecha 
                Txt_Fecha.Text = DateTime.Now.ToString("dd/MMM/yyyy");

                Configuracion_Acceso("Frm_Ope_Com_Comite_Compras.aspx");
                break;
                
            case "Nuevo":
                //Manejo de los Div
                Div_Busqueda.Visible = false;
                Div_Listado_Comite.Visible = false;
                Div_Comite_Compras.Visible = true;
                //Boton Nuevo
                Btn_Nuevo.Visible = true;
                Btn_Nuevo.ToolTip = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                //Boton Modificar
                Btn_Modificar.Visible = false;
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Llenar_Combo_Estatus();
                Llenar_Combo_Tipo();
                Habilitar_Componentes(true);
                break;
            case "Modificar":
                //Manejo de los Div
                Div_Busqueda.Visible = false;
                Div_Listado_Comite.Visible = false;
                Div_Comite_Compras.Visible = true;
                //Boton Nuevo
                Btn_Nuevo.Visible = false;
                Btn_Nuevo.ToolTip = "Modificar";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                //Boton Modificar
                Btn_Modificar.Visible = true;
                Btn_Modificar.ToolTip = "Actualizar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Habilitar_Componentes(true);
                break;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Estatus()
    ///DESCRIPCIÓN: Metodo que permite cargar los el combo de estatus
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 15/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Habilitar_Componentes(bool Estatus)
    {
        Txt_Folio.Enabled = false;
        Txt_Fecha.Enabled = false;
        Txt_Total.Enabled = false;
        Txt_Justificacion.Enabled = Estatus;
        Txt_Comentario.Enabled = Estatus;
        Cmb_Estatus.Enabled = Estatus;
        Cmb_Tipo.Enabled = Estatus;
        Chk_Requisiciones.Enabled = Estatus;
        Chk_Consolidaciones.Enabled = Estatus;
        Grid_Consolidaciones.Enabled = Estatus;
        Grid_Requisiciones.Enabled = Estatus;
        Btn_Agregar_Requisicion_Consolidaciones.Enabled = Estatus;

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos_Negocio()
    ///DESCRIPCIÓN: Metodo que permite cargar las variables de la clase negocio para hacer update o insert 
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 15/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Cargar_Datos_Negocio()
    {
        Comite_Negocio.P_Folio = Txt_Folio.Text;
        Comite_Negocio.P_Estatus = Cmb_Estatus.SelectedValue;
        Comite_Negocio.P_Tipo = Cmb_Tipo.SelectedValue;
        Comite_Negocio.P_Justificacion = Txt_Justificacion.Text;
        Comite_Negocio.P_Comentarios = Txt_Comentario.Text;
        Comite_Negocio.P_Monto_Total = Txt_Total.Text;
        Comite_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
        Comite_Negocio.P_Usuario_ID = Cls_Sessiones.Empleado_ID;
        //Generamos el listado de licitaciones
        if (Session["Dt_Requisiciones"] != null || Session["Dt_Consolidaciones"] != null)
        {
            //en caso de tener requisiciones y consolidaciones
            Comite_Negocio.P_Lista_Requisiciones = Generar_Listado_Requisiciones();
        }//fin del if 
        else
        {
            Comite_Negocio.P_Lista_Requisiciones = null;
        }
        if (Session["Dt_Consolidaciones"] != null)
        {
            Comite_Negocio.P_Lista_Consolidaciones = Generar_Listado_Consolidaciones();
        }
        else
        {
            Comite_Negocio.P_Lista_Consolidaciones = null;
        }

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Formulario()
    ///DESCRIPCIÓN: Metodo que permite limpiar el formulario y las variables de session 
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 15/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Limpiar_Formulario()
    {
        Txt_Folio.Text = "";
        Txt_Fecha.Text = "";
        Cmb_Tipo.SelectedIndex = 0;
        Cmb_Estatus.SelectedIndex = 0;
        Txt_Justificacion.Text = "";
        Txt_Comentario.Text = "";
        Txt_Total.Text = "";
        Chk_Consolidaciones.Checked = false;
        Chk_Requisiciones.Checked = false;
        Div_Consolidaciones_Busqueda.Visible = false;
        Grid_Consolidaciones.DataSource = new DataTable();
        Grid_Consolidaciones.DataBind();
        Div_Requisiciones_Busqueda.Visible = false;
        Grid_Requisiciones.DataSource = new DataTable();
        Grid_Requisiciones.DataBind();
        
        Session["Total"] = null;
        Session["Dt_Consolidaciones"] = null;
        Session["Dt_Requisiciones"] = null;
        Session["No_Requisicion"] = null;
        Session["No_Consolidacion"] = null;
        Session["No_Comite_Compras"] = null;
        Comite_Negocio = new Cls_Ope_Com_Comite_Compras_Negocio();

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Estatus()
    ///DESCRIPCIÓN: Metodo que permite cargar los el combo de estatus
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 15/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Estatus()
    {
        Cmb_Estatus.Items.Clear();
        Cmb_Estatus.Items.Add("<<SELECCIONAR>>");
        Cmb_Estatus.Items.Add("EN CONSTRUCCION");
        Cmb_Estatus.Items.Add("GENERADA");
        Cmb_Estatus.Items[0].Value = "0";
        Cmb_Estatus.Items[0].Selected = true;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Tipo
    ///DESCRIPCIÓN: Metodo que carga el combo Cmb_Tipo
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 31/enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Tipo()
    {
        Cmb_Tipo.Items.Clear();
        Cmb_Tipo.Items.Add("<<SELECCIONAR>>");
        Cmb_Tipo.Items.Add("PRODUCTO");
        Cmb_Tipo.Items.Add("SERVICIO");
        Cmb_Tipo.Items[0].Value = "0";
        Cmb_Tipo.Items[0].Selected = true;
    }

    #region Metodos_Tab_Container

   
      ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Requisiciones_CheckedChanged
    ///DESCRIPCIÓN: Evento del checkbox de Requisiciones
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 29/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Requisiciones_CheckedChanged(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        if (Chk_Requisiciones.Checked == true)
        {
            Chk_Consolidaciones.Checked = false;
            Div_Consolidaciones_Busqueda.Visible = false;
            if (Cmb_Tipo.SelectedIndex == 0)
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "+ Es necesario indicar el tipo de licitacion si es de PRODUCTO ó SERVICIO<br/>";
                Chk_Requisiciones.Checked = false;
            }
            else
            {
                //Cargamos las variables de negocio necesarias para realizar la consulta
                Comite_Negocio.P_Tipo = Cmb_Tipo.SelectedValue;
                Comite_Negocio.P_No_Consolidacion = null;
                DataTable Dt_Requisiciones = Comite_Negocio.Consultar_Requisiciones();
                if (Dt_Requisiciones.Rows.Count != 0)
                {
                    Div_Requisiciones_Busqueda.Visible = true;
                    Grid_Requisiciones_Busqueda.DataSource = Dt_Requisiciones;
                    Grid_Requisiciones_Busqueda.DataBind();
                }
                else
                {
                    Lbl_Mensaje_Error.Text = "+ No se encontraron Requisiciones </br>";
                    Div_Contenedor_Msj_Error.Visible = true;
                    Chk_Requisiciones.Checked = false;
                }
            }
            

        }//fin if Chk_Requisiciones
        if (Chk_Requisiciones.Checked == false)
        {
            Div_Requisiciones_Busqueda.Visible = false;
            Div_Consolidaciones_Busqueda.Visible = false;
        }
        
    }

      ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Consolidaciones_CheckedChanged
    ///DESCRIPCIÓN: Evento del checkbox de Consolidaciones
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 29/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Consolidaciones_CheckedChanged(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        if (Chk_Consolidaciones.Checked == true)
        {
            Chk_Requisiciones.Checked = false;
            Div_Requisiciones_Busqueda.Visible = false;
            if (Cmb_Tipo.SelectedIndex == 0)
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "+ Es necesario indicar el tipo de licitacion si es de PRODUCTO ó SERVICIO<br/>";
                Chk_Consolidaciones.Checked = false;
            }
            else
            {
                Comite_Negocio.P_Tipo = Cmb_Tipo.SelectedValue;
                Comite_Negocio.P_No_Requisicion = null;
                DataTable Dt_Consolidaciones = Comite_Negocio.Consulta_Consolidaciones();
                if (Dt_Consolidaciones.Rows.Count != 0)
                {
                    Div_Consolidaciones_Busqueda.Visible = true;
                    Grid_Consolidaciones_Busqueda.DataSource = Dt_Consolidaciones;
                    Grid_Consolidaciones_Busqueda.DataBind();
                }
                else
                {
                    Lbl_Mensaje_Error.Text = "+ No se encontraron Consolidaciones </br>";
                    Div_Contenedor_Msj_Error.Visible = true;
                    Chk_Consolidaciones.Checked = false;
                }
            }//fin del if Div_contenedor

        }
        else
        {
            Div_Consolidaciones_Busqueda.Visible = false;
            Div_Requisiciones_Busqueda.Visible = false;
            Chk_Consolidaciones.Checked = false;
        }
        
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Requisicion_Consolidaciones_Click
    ///DESCRIPCIÓN: Evento del boton que agrega las requisiciones o consolidaciones
    ///seleccionadas al grid de la segunda pestaña
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 29/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Agregar_Requisicion_Consolidaciones_Click(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        if (Chk_Requisiciones.Checked == false && Chk_Consolidaciones.Checked == false)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "+ Debe seleccionar una opcion: Consolidacion o Requisiciones";
        }
        else
        {            
            if (Chk_Requisiciones.Checked == true)
            {
                //Creamos un arreglo con las requisiciones seleccionadas
                String[] Requisiciones = Check_Box_Seleccionados(Grid_Requisiciones_Busqueda, "Chk_Requisicion_Seleccionada", "Requisicion");
                //Recorremos el arreglo para insertar cada requisicion en el Comite de COmpras creado
                for (int i = 0; i < Requisiciones.Length; i++)
                {
                    if (Session["Dt_Requisiciones"] != null)
                    {
                        Agregar_Requisicion((DataTable)Session["Dt_Requisiciones"], Requisiciones[i]);
                    }//fin if
                    else
                    {
                        //Creamos la session por primera ves
                        DataTable Dt_Requisiciones = new DataTable();
                        Dt_Requisiciones.Columns.Add("No_Requisicion", typeof(System.String));
                        Dt_Requisiciones.Columns.Add("Folio", typeof(System.String));
                        Dt_Requisiciones.Columns.Add("Fecha", typeof(System.String));
                        Dt_Requisiciones.Columns.Add("Dependencia", typeof(System.String));
                        Dt_Requisiciones.Columns.Add("Area", typeof(System.String));
                        Dt_Requisiciones.Columns.Add("Total", typeof(System.String));
                        Session["Dt_Requisiciones"] = Dt_Requisiciones;
                        //Llenamos el grid
                        Grid_Requisiciones.DataSource = (DataTable)Session["Dt_Requisiciones"];
                        Grid_Requisiciones.DataBind();
                        //Obtenemos los valores de Monto disponible, Monto_Comprometido 
                        Session["Total_Licitacion"] = 0;
                        Agregar_Requisicion(Dt_Requisiciones, Requisiciones[i]);
                        //Limpiamos los componenetes de la Requisicion seleccionada
                        Comite_Negocio.P_No_Requisicion = null;
                    }//fin del else session


                }//fin del for
               

            }
        
        //en caso de seleccionar el checkbox de Consolidaciones
        if (Chk_Consolidaciones.Checked == true)
        {
            //Recorremos grid para detectar los check seleccionados
            String[] Consolidaciones = Check_Box_Seleccionados(Grid_Consolidaciones_Busqueda, "Chk_Consolidacion_Seleccionada", " Consolidacion");
            if (Consolidaciones.Length != 0)
            {
                //insertamos las consolidaciones en el grid de consolidaciones
                for (int i = 0; i < Consolidaciones.Length; i++)
                {
                        if (Session["Dt_Consolidaciones"] != null)
                        {
                            Agregar_Consolidacion((DataTable)Session["Dt_Consolidaciones"], Consolidaciones[i]);
                        }//fin if
                        else
                        {
                            //Creamos la session por primera ves
                            DataTable Dt_Consolidaciones = new DataTable();
                            Dt_Consolidaciones.Columns.Add("No_Consolidacion", typeof(System.String));
                            Dt_Consolidaciones.Columns.Add("Folio", typeof(System.String));
                            Dt_Consolidaciones.Columns.Add("Estatus", typeof(System.String));
                            Dt_Consolidaciones.Columns.Add("Fecha", typeof(System.String));
                            Dt_Consolidaciones.Columns.Add("Total", typeof(System.String));
                            Dt_Consolidaciones.Columns.Add("Lista_Requisiciones", typeof(System.String));
                            Session["Dt_Consolidaciones"] = Dt_Consolidaciones;
                            //Llenamos el grid
                            Grid_Consolidaciones.DataSource = (DataTable)Session["Dt_Consolidaciones"];
                            Grid_Consolidaciones.DataBind();
                            //Obtenemos los valores de Monto disponible, Monto_Comprometido 
                            Session["Total"] = 0;
                            Agregar_Consolidacion(Dt_Consolidaciones, Consolidaciones[i]);
                            //Limpiamos los componenetes de la Requisicion seleccionada
                            Comite_Negocio.P_No_Consolidacion = null;
                        }//fin del else session
                }//fin del For Consolidaciones
            }//fin del if Consolidaciones

        }//fin del if Chk_Consolidaciones.checked
        
        }//fin del Else
        //Dejamos limpia la primer pestaña
        //No mostramos los div de la pestaña 1 
        Div_Consolidaciones_Busqueda.Visible = false;
        Div_Requisiciones_Busqueda.Visible = false;
        //limpiamos los checkbox
        Chk_Requisiciones.Checked = false;
        Chk_Consolidaciones.Checked = false;
        //Mostramos la segunda pestaña
        Tab_Requisiciones_Consolidaciones.ActiveTabIndex = 1;
        //en caso de seleccionar el checkbox de Requisiciones 

    }//Fin del Metodo

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Listado_Consolidaciones
    ///DESCRIPCIÓN: Metodo que permite generar un  listado con las consolidaciones seleccionadas lo cual nos 
    ///permitira cambiar el estatus de las consolidaciones seleccionadas a OCUPADA
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public String Generar_Listado_Consolidaciones()
    {
        String Lista_Consolidaciones = "";
        DataTable Dt_Consolidaciones = (DataTable)Session["Dt_Consolidaciones"];
        if (Dt_Consolidaciones.Rows.Count != 0)
        {
            for (int i = 0; i < Dt_Consolidaciones.Rows.Count; i++)
            {

                if (i == Dt_Consolidaciones.Rows.Count - 1)
                {
                    Lista_Consolidaciones = Lista_Consolidaciones + Dt_Consolidaciones.Rows[i]["No_Consolidacion"].ToString().Trim();
                }
                else
                {
                    Lista_Consolidaciones = Lista_Consolidaciones + Dt_Consolidaciones.Rows[i]["No_Consolidacion"].ToString().Trim() + ",";
                }
            }//fin del for

        }
        return Lista_Consolidaciones;
    }

    #endregion Fin_Metodos_Tab_Container
    
    #endregion Fin_Metodos

    ///*******************************************************************************
    /// REGION GRID
    ///*******************************************************************************
    #region Grid
    ///*******************************************************************************
    /// SUBREGION GRID_REQUISICIONES
    ///*******************************************************************************
    #region Grid_Requisiciones
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Requisiciones_SelectedIndexChanged
    ///DESCRIPCIÓN: Metodo que permite eliminar una requisicion del grid de listado seleccionado
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 11/Enero/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    protected void Grid_Requisiciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataRow[] Renglones;
        DataRow Renglon;
        //Obtenemos el Id del producto seleccionado
        GridViewRow selectedRow = Grid_Requisiciones.Rows[Grid_Requisiciones.SelectedIndex];
        Session["No_Requisicion"] = Grid_Requisiciones.SelectedDataKey["No_Requisicion"].ToString();
        Renglones = ((DataTable)Session["Dt_Requisiciones"]).Select(Ope_Com_Requisiciones.Campo_Requisicion_ID + "='" + Session["No_Requisicion"].ToString() + "'");

        if (Renglones.Length > 0)
        {
            Renglon = Renglones[0];
            DataTable Tabla = (DataTable)Session["Dt_Requisiciones"];
            Tabla.Rows.Remove(Renglon);
            //Asignamos el nuevo valor al datatable de Session
            Session["Dt_Requisiciones"] = Tabla;
            Grid_Requisiciones.SelectedIndex = (-1);
            Grid_Requisiciones.DataSource = Tabla;
            Grid_Requisiciones.DataBind();
            //Calculamos el nuevo importe
            double Monto_eliminado = double.Parse(HttpUtility.HtmlDecode(selectedRow.Cells[6].Text).ToString());
            //Restamos el monto de la requisicion seleccionada al monto total del registro de comite de compras
            Session["Total"] = double.Parse(Session["Total"].ToString()) - Monto_eliminado;
            Txt_Total.Text = Session["Total"].ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Requisiciones_PageIndexChanging
    ///DESCRIPCIÓN: Metodo que permite la paginacion del Grid_Requisicion
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 29/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Requisiciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Requisiciones.PageIndex = e.NewPageIndex;
        Grid_Requisiciones.DataSource = (DataTable)Session["Dt_Requisiciones"];
        Grid_Requisiciones.DataBind();
    }
    #endregion Fin_Grid_Requisiciones
    ///*******************************************************************************
    /// SUBREGION GRID_CONSOLIDACIONES
    ///*******************************************************************************
    #region Grid_Consolidaciones
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Consolidaciones_SelectedIndexChanged
    ///DESCRIPCIÓN: Metodo del grid de consolidaciones al seleccionar un boton
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 23/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Consolidaciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataRow[] Renglones;
        DataRow Renglon;
        //Obtenemos el Id del producto seleccionado
        GridViewRow selectedRow = Grid_Consolidaciones.Rows[Grid_Consolidaciones.SelectedIndex];
        Session["No_Consolidacion"] = Grid_Consolidaciones.SelectedDataKey["No_Consolidacion"].ToString();
        Renglones = ((DataTable)Session["Dt_Consolidaciones"]).Select(Ope_Com_Requisiciones.Campo_No_Consolidacion + "='" + Session["No_Consolidacion"].ToString() + "'");

        if (Renglones.Length > 0)
        {
            Renglon = Renglones[0];
            DataTable Tabla = (DataTable)Session["Dt_Consolidaciones"];
            Tabla.Rows.Remove(Renglon);
            //Asignamos el nuevo valor al datatable de Session
            Session["Dt_Consolidaciones"] = Tabla;
            Grid_Consolidaciones.SelectedIndex = (-1);
            Grid_Consolidaciones.DataSource = Tabla;
            Grid_Consolidaciones.DataBind();
            //Calculamos el nuevo importe
            //String Algo = HttpUtility.HtmlDecode(selectedRow.Cells[5].Text).ToString();
            double Monto_eliminado = double.Parse(HttpUtility.HtmlDecode(selectedRow.Cells[5].Text).ToString());
            Session["Total"] = double.Parse(Session["Total"].ToString()) - Monto_eliminado;
            Txt_Total.Text = Session["Total"].ToString();
        }
    }
      ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Consolidaciones_PageIndexChanging
    ///DESCRIPCIÓN: Metodo que permite la paginacion del Grid_Consolidaciones
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 29/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Consolidaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Consolidaciones.PageIndex = e.NewPageIndex;
        Grid_Consolidaciones.DataSource = (DataTable)Session["Dt_Consolidaciones"];
        Grid_Consolidaciones.DataBind();
    }
    
    #endregion Fin_Grid_Consolidaciones

    ///*******************************************************************************
    /// SUBREGION GRID_LISTA_COMITE
    ///*******************************************************************************
    #region Grid_Lista_Comite

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:  Llenar_Grid_Comite
    ///DESCRIPCIÓN: 
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 31/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Grid_Comite()
    {
        DataTable Dt_Comite = Comite_Negocio.Consulta_Comite_Compras();
        if (Dt_Comite.Rows.Count != 0)
        {
            Grid_Comite_Compras.DataSource = Dt_Comite;
            Grid_Comite_Compras.DataBind();
            Session["Grid_Comite_Compras"] = Dt_Comite;
        }
        else
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "+ No se encontraron Datos <br/>";
            Grid_Comite_Compras.DataSource = new DataTable();
            Grid_Comite_Compras.DataBind();
        }

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Comite_Compras_SelectedIndexChanged
    ///DESCRIPCIÓN: 
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 31/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Comite_Compras_SelectedIndexChanged(object sender, EventArgs e)
    {
        Estatus_Formulario("General");
        GridViewRow Row = Grid_Comite_Compras.SelectedRow;
        Comite_Negocio.P_No_Comite_Compras = Grid_Comite_Compras.SelectedDataKey["No_Comite_Compras"].ToString();
        //Consultamos los datos de lal proceso de comite de compras seleccionado 
        DataTable Dt_Comite_Compras = Comite_Negocio.Consulta_Comite_Compras();
        Session["No_Comite_Compras"] = Dt_Comite_Compras.Rows[0][Ope_Com_Comite_Compras.Campo_No_Comite_Compras].ToString().Trim();
        Txt_Folio.Text = Dt_Comite_Compras.Rows[0][Ope_Com_Comite_Compras.Campo_Folio].ToString();
        Txt_Fecha.Text = Dt_Comite_Compras.Rows[0]["FECHA"].ToString();
        //Cmb_Tipo.SelectedValue = Dt_Comite_Compras.Rows[0][Ope_Com_Comite_Compras.Campo_Tipo].ToString().Trim();
        Cmb_Tipo.SelectedIndex = Cmb_Tipo.Items.IndexOf(Cmb_Tipo.Items.FindByText(Dt_Comite_Compras.Rows[0][Ope_Com_Comite_Compras.Campo_Tipo].ToString().Trim()));
        Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByText(Dt_Comite_Compras.Rows[0][Ope_Com_Comite_Compras.Campo_Estatus].ToString().Trim()));
        Txt_Comentario.Text = Dt_Comite_Compras.Rows[0][Ope_Com_Comite_Compras.Campo_Comentarios].ToString();
        Txt_Justificacion.Text = Dt_Comite_Compras.Rows[0][Ope_Com_Comite_Compras.Campo_Justificacion].ToString();
        Txt_Total.Text = Dt_Comite_Compras.Rows[0][Ope_Com_Comite_Compras.Campo_Monto_Total].ToString();
        Session["Total"] = double.Parse(Dt_Comite_Compras.Rows[0][Ope_Com_Comite_Compras.Campo_Monto_Total].ToString());
        //Llenamos el Grid de Requisiciones en caso de tener
        DataTable Dt_Requisiciones = Comite_Negocio.Consultar_Comite_Detalle_Requisicion();
        if (Dt_Requisiciones != null)
        {
            Grid_Requisiciones.DataSource = Dt_Requisiciones;
            Grid_Requisiciones.DataBind();
            //llenamos la variable de session con el DT de Requisiciones
            Session["Dt_Requisiciones"] = Dt_Requisiciones;
        }
        //Llenamos el Grid de consolidaciones en caso de tener
        DataTable Dt_Consolidaciones = Comite_Negocio.Consultar_Detalle_Consolidacion();
        if (Dt_Consolidaciones != null)
        {
            Grid_Consolidaciones.DataSource = Dt_Consolidaciones;
            Grid_Consolidaciones.DataBind();
            //llenamos la variable de session de las consolidaciones pertenecientes al comite de compras 
            Session["Dt_Consolidaciones"] = Dt_Consolidaciones;
        }
        
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Comite_Compras_PageIndexChanging
    ///DESCRIPCIÓN: Metodo que permite la paginacion del Grid_Comite_Compras
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Comite_Compras_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Comite_Compras.PageIndex = e.NewPageIndex;
        Grid_Comite_Compras.DataSource = Session["Grid_Comite_Compras"];
        Grid_Comite_Compras.DataBind();

    }


    #endregion Fin_Grid_Lista_Comite


    #endregion Fin_Grid

    ///*******************************************************************************
    /// REGION EVENTOS
    ///*******************************************************************************
    #region Eventos

    ///*******************************************************************************
    /// SUBREGION EVENTOS BARRA BOTONES GENERALES Y BUSQUEDAS
    ///*******************************************************************************
    #region Botones_Generales_y_Busqueda

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Metodo que ejecuta el boton de Nuevo
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 31/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        switch (Btn_Nuevo.ToolTip)
        {
            case "Nuevo":
                Habilitar_Componentes(true);
                Estatus_Formulario("Nuevo");
                break;
            case "Dar de Alta":
                Habilitar_Componentes(false);
                try
                {
                    Cargar_Datos_Negocio();
                    //Damos de alta el registro de comite de compras
                    Comite_Negocio.Alta_Comite_Compras();
                    Estatus_Formulario("Inicial");
                    Limpiar_Formulario();
                    Llenar_Grid_Comite();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Comite Compras", "alert('Se dio de alta el Comite de Compras ');", true);
                   
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al dar de alta el Proceso de Comite de Compras :Error[" + Ex.Message + "]");
                }

                break;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Metodo que ejecuta el boton de Modificar
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 31/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        switch (Btn_Modificar.ToolTip)
        {
            case "Modificar":
                Habilitar_Componentes(true);
                Estatus_Formulario("Modificar");

                break;
            case "Actualizar":
                Habilitar_Componentes(false);
                try
                {
                    Cargar_Datos_Negocio();
                    Comite_Negocio.P_No_Comite_Compras = Session["No_Comite_Compras"].ToString();
                    //Modificamos el comite seleccionado 
                    Comite_Negocio.Modificar_Comite_Compras();
                    Estatus_Formulario("Inicial");
                    Limpiar_Formulario();
                    Llenar_Grid_Comite();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Comite Compras", "alert('Se Modifico el Comite de Compras ');", true);
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al dar de alta el Proceso de Comite de Compras :Error[" + Ex.Message + "]");
                }
                break;
        }


    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:Btn_Salir_Click
    ///DESCRIPCIÓN: Metodo que ejecuta el boton de Salir
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 31/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        switch(Btn_Salir.ToolTip)
        {
            case "Cancelar":
                Estatus_Formulario("Inicial");
                Limpiar_Formulario();
                break;
            case "Listado":
                Estatus_Formulario("Inicial");
                Limpiar_Formulario();
                break;
            case "Inicio":
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                Limpiar_Formulario();
                break;
        }

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:Btn_Avanzada_Click
    ///DESCRIPCIÓN: Metodo que ejecuta el boton de Avanzada
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 31/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Avanzada_Click(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:Btn_Buscar_Click
    ///DESCRIPCIÓN: Metodo que ejecuta el boton de Buscar
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 31/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Comite_Negocio = new Cls_Ope_Com_Comite_Compras_Negocio();
        Comite_Negocio.P_Folio = Txt_Busqueda.Text;
        //consultamos y llenamos el grid de comite
        Llenar_Grid_Comite();
        
    }

    #endregion Fin_Botones_Generales

    #region Manejo de Consolidaciones y Requisiciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Agregar_Requisicion
    ///DESCRIPCIÓN: Metodo que permite agregar un nuevo producto al Grid_Productos
    ///PARAMETROS: 1.- DataTable _DataTable: Data_Table que contiene la nueva requisicion a agregar
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 8/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Agregar_Requisicion(DataTable _DataTable, String No_Requisicion)
    {
        //Realizamos la consulta del producto seleccionado
        String Id = No_Requisicion;
        DataRow[] Filas;
        DataTable Dt = (DataTable)Session["Dt_Requisiciones"];
        Filas = _DataTable.Select("No_Requisicion='" + Id + "'");
        if (Filas.Length > 0)
        {
            //Si se encontro algun coincidencia entre el grupo a agregar con alguno agregado anteriormente, se avisa
            //al usuario que elemento ha agregar ya existe en la tabla de grupos.
            Lbl_Mensaje_Error.Text += "+ No se puede agregar la requisición " + Id + " ya que esta ya se ha agregado <br/>";
            Div_Contenedor_Msj_Error.Visible = true;
        }
        else
        {
            Comite_Negocio.P_No_Requisicion = Id;
            //Obtengo los datos de la nueva requisicion a insertar en el GridView
            DataTable Dt_Temporal = Comite_Negocio.Consultar_Requisiciones();
            //
            double Total_Comite = 0;
            if (Session["Total"] != null)
                Total_Comite = double.Parse(Session["Total"].ToString());
            if (!(Dt_Temporal == null))
            {
                if (Dt_Temporal.Rows.Count > 0)
                {
                    DataRow Fila_Nueva = Dt.NewRow();
                    //Asignamos los valores a la fila
                    Fila_Nueva["No_Requisicion"] = Dt_Temporal.Rows[0]["No_Requisicion"].ToString();
                    Fila_Nueva["Folio"] = Dt_Temporal.Rows[0]["Folio"].ToString();
                    Fila_Nueva["Fecha"] = Dt_Temporal.Rows[0]["Fecha"].ToString();
                    Fila_Nueva["Dependencia"] = Dt_Temporal.Rows[0]["Dependencia"].ToString();
                    Fila_Nueva["Area"] = Dt_Temporal.Rows[0]["Area"].ToString();
                    Fila_Nueva["Total"] = Dt_Temporal.Rows[0]["Total"].ToString();
                    Dt.Rows.Add(Fila_Nueva);
                    Dt.AcceptChanges();
                    Session["Dt_Requisiciones"] = Dt;
                    Total_Comite = Total_Comite + double.Parse(Dt_Temporal.Rows[0]["Total"].ToString());
                    Session["Total"] = Total_Comite;
                    Txt_Total.Text = Session["Total"].ToString();
                    Grid_Requisiciones.DataSource = Dt;
                    Grid_Requisiciones.DataBind();
                    Grid_Requisiciones.Visible = true;
                }
            }
        }//fin del else       
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Agregar_Consolidacion
    ///DESCRIPCIÓN: Metodo que permite agregar una nueva Consolidacion al Grid_Consolidaciones
    ///PARAMETROS: 1.- DataTable _DataTable: Data_Table que contiene el nuevo prducto a agregar
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 8/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Agregar_Consolidacion(DataTable _DataTable, String No_Consolidacion)
    {
        //Realizamos la consulta del producto seleccionado
            String Id = No_Consolidacion;
            DataRow[] Filas;
            DataTable Dt = (DataTable)Session["Dt_Consolidaciones"];
            Filas = _DataTable.Select("No_Consolidacion='" + Id + "'");
            if (Filas.Length > 0)
            {
                //Si se encontro algun coincidencia entre el grupo a agregar con alguno agregado anteriormente, se avisa
                //al usuario que elemento ha agregar ya existe en la tabla de grupos.
                Lbl_Mensaje_Error.Text += "+ No se puede agregar la Consolidación " + Id + " ya que esta ya se ha agregado<br/>";
                Div_Contenedor_Msj_Error.Visible = true;
            }
            else
            {
                Comite_Negocio.P_No_Consolidacion = Id;
                //Obtengo los datos de la nueva requisicion a insertar en el GridView
                DataTable Dt_Temporal = Comite_Negocio.Consulta_Consolidaciones();
                //
                double Total_Comite = 0;
                if (Session["Total"] != null)
                    Total_Comite = double.Parse(Session["Total"].ToString());
                if (!(Dt_Temporal == null))
                {
                    if (Dt_Temporal.Rows.Count > 0)
                    {
                        DataRow Fila_Nueva = Dt.NewRow();
                        //Asignamos los valores a la fila
                        Fila_Nueva["No_Consolidacion"] = Dt_Temporal.Rows[0]["No_Consolidacion"].ToString();
                        Fila_Nueva["Folio"] = Dt_Temporal.Rows[0]["Folio"].ToString();
                        Fila_Nueva["Estatus"] = Dt_Temporal.Rows[0]["Estatus"].ToString();
                        Fila_Nueva["Fecha"] = Dt_Temporal.Rows[0]["Fecha"].ToString();
                        Fila_Nueva["Total"] = Dt_Temporal.Rows[0]["Total"].ToString();
                        Fila_Nueva["Lista_Requisiciones"] = Dt_Temporal.Rows[0]["Lista_Requisiciones"].ToString();
                        Dt.Rows.Add(Fila_Nueva);
                        Dt.AcceptChanges();
                        Session["Dt_Consolidaciones"] = Dt;
                        Total_Comite = Total_Comite + double.Parse(Dt_Temporal.Rows[0]["Total"].ToString());
                        Session["Total"] = Total_Comite;
                        Txt_Total.Text = Session["Total"].ToString();
                        Grid_Consolidaciones.DataSource = Dt;
                        Grid_Consolidaciones.DataBind();
                        Grid_Consolidaciones.Visible = true;
                    }
                }
            }//fin del else
        
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Check_Box_Seleccionados
    ///DESCRIPCIÓN: Metodo que debuelve un string con las Re seleccionados
    ///PARAMETROS:    1.- GridView que se recorre
    ///               2.- nombre_check del cual se evalua el estado Checked
    ///               3.- Nombre_ope nombre de la operacion ya sea (Catalogo u operaciones)
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 27/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public String[] Check_Box_Seleccionados(GridView MyGrid, String nombre_check, String Tipo)
    {

        //Variable que guarda el nombre del catalogo seleccionado. Ejem: (Frm_Cat_Ate_Colonias)
        String Check_seleccionado = "";
        //variable que guarda el nombre de la pagina. Ejem: (Colonias)
        
        int num = 0;
        int Num_Check_Sel = Numero_Checks(MyGrid, nombre_check);
        //Arreglo donde se almacenaran las requisiciones seleccionadas 
        String[] Array_aux = new String[Num_Check_Sel];
        
        if (Num_Check_Sel == 0)
        {
            Lbl_Mensaje_Error.Text += "+ Debe seleccionar por lo menos una " + Tipo + "<br />";
            Div_Contenedor_Msj_Error.Visible = true;
        }
        else
        {
            //Recorremos el arreglo para obtener los Id seleccionados
            for (int i = 0; i < MyGrid.Rows.Count; i++)
            {
                GridViewRow row = MyGrid.Rows[i];
                bool isChecked = ((System.Web.UI.WebControls.CheckBox)row.FindControl(nombre_check)).Checked;

                if (isChecked)
                {
                    //Obtiene el id de la requisicion seleccionada
                    Check_seleccionado = Convert.ToString(row.Cells[1].Text);
                    //llenamos el arreglo con los ID de las requisiciones
                    Array_aux[num] = Check_seleccionado;
                    num = num + 1;
                }
            }//fin del for i
        }

        return Array_aux;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Cadena
    ///DESCRIPCIÓN: Metodo que genera una cadena a partir de un arreglo 
    ///PARAMETROS: 1.- String []Arreglo: Arreglo en el que a el listado de los catalogos seleccionados 
    ///            2.- int Longitud: Numero de check seleccionados 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 28/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public String Generar_Cadena(String[] Arreglo, int Longitud)
    {
        //Variable que almacenara los catalogos seleccionados 
        String Cadena = "";
        for (int y = 0; y < Longitud; y++)
        {
            if (y == 0)
            {
                Cadena += Arreglo[y];
            }
            else
            {
                Cadena += " ," + Arreglo[y];
            }

        }//fin del for y

        return Cadena;
    }//fin de generar cadena

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Listado_Requisiciones
    ///DESCRIPCIÓN: Metodo que permite generar un  listado con las requisiciones seleccionadas
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public String Generar_Listado_Requisiciones()
    {
        DataTable Dt_Requisiciones = (DataTable)Session["Dt_Requisiciones"];
        String Lista_Requisiciones = "";
        if (Dt_Requisiciones != null)
        {
            for (int i = 0; i < Dt_Requisiciones.Rows.Count; i++)
            {

                if (i == Dt_Requisiciones.Rows.Count - 1)
                {
                    Lista_Requisiciones = Lista_Requisiciones + Dt_Requisiciones.Rows[i]["No_Requisicion"].ToString().Trim();
                }
                else
                {
                    Lista_Requisiciones = Lista_Requisiciones + Dt_Requisiciones.Rows[i]["No_Requisicion"].ToString().Trim() + ",";
                }
            }//fin del for
        }//fin del if
        DataTable Dt_Consolidaciones = (DataTable)Session["Dt_Consolidaciones"];
        if (Dt_Consolidaciones != null)
        {
            for (int i = 0; i < Dt_Consolidaciones.Rows.Count; i++)
            {

                if (i == Dt_Consolidaciones.Rows.Count - 1)
                {
                    Lista_Requisiciones = Lista_Requisiciones + Dt_Consolidaciones.Rows[i]["Lista_Requisiciones"].ToString().Trim();
                }
                else
                {
                    Lista_Requisiciones = Lista_Requisiciones + Dt_Consolidaciones.Rows[i]["Lista_Requisiciones"].ToString().Trim() + ",";
                }
            }//fin del for

        }

        return Lista_Requisiciones;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Numero_Checks
    ///DESCRIPCIÓN: Metodo que cuenta el numero de checks seleccionados dentro del GridView 
    ///PROPIEDADES:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 28/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public int Numero_Checks(GridView MyGrid, String nombre_check)
    {
        int Numero_Seleccionados = 0;
        //Obtenemos el numero de Checkbox seleccionados
        for (int i = 0; i < MyGrid.Rows.Count; i++)
        {
            GridViewRow row = MyGrid.Rows[i];
            bool isChecked = ((System.Web.UI.WebControls.CheckBox)row.FindControl(nombre_check)).Checked;

            if (isChecked)
            {
                //Variable auxiliar para contar el numero de check seleccionados. 
                Numero_Seleccionados = Numero_Seleccionados + 1;
            }
        }//fin del for i

        return Numero_Seleccionados;
    }

    #endregion Fin Manejo de Consolidaciones y Requisiciones
    ///*******************************************************************************
    /// SUBREGION EVENTOS BUSQUEDA REQUISICIONES
    ///*******************************************************************************
    #region Busqueda_Requisiciones
    
    protected void Btn_Filtro_Requisiciones_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

    }


    #endregion Fin_Busqueda_Requisiciones


    #endregion Fin_Eventos

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

}
