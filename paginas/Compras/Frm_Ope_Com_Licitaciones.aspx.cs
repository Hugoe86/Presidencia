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
using Presidencia.Licitaciones_Compras.Negocio;
using Presidencia.Administrar_Requisiciones.Negocios;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Collections.Generic;

public partial class paginas_Compras_Frm_Ope_Com_Licitaciones : System.Web.UI.Page
{

    ///*******************************************************************************
    /// VARIABLES
    ///*******************************************************************************
    #region Variables
    Cls_Ope_Com_Licitaciones_Negocio Licitaciones_Negocio;
    Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio;
    #endregion


    ///*******************************************************************************
    /// REGION PAGE_LOAD
    ///*******************************************************************************
    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Init
    ///DESCRIPCIÓN: 
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Page_Init(object sender, EventArgs e)
    {
        Licitaciones_Negocio = new Cls_Ope_Com_Licitaciones_Negocio();
        Requisicion_Negocio = new Cls_Ope_Com_Administrar_Requisiciones_Negocio();
        Llenar_Grid_Licitaciones();
        Estatus_Formulario("Inicial");
        if (Grid_Licitaciones.Rows.Count == 0)
        {
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = false;
            Estatus_Formulario("Nuevo");
        }
    }
    #endregion


    ///*******************************************************************************
    /// REGION METODOS
    ///*******************************************************************************
    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Estatus_Formulario
    ///DESCRIPCIÓN: Metodo que sirve para asignar estatus al formulario 
    ///PARAMETROS: String Estatus: Indica si se encuentra en Inicial,General,Nuevo o Modificar
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Estatus_Formulario(String Estatus)
    {
        switch (Estatus)
        {
            case "Inicial":
                //Boton Nuevo
                Btn_Nuevo.Visible = true;
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                //Boton Modificar
                Configuracion_Acceso("Frm_Ope_Com_Licitaciones.aspx");
                Btn_Modificar.Visible = false;
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Inicio";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                //Manejo de los Div
                Div_Licitaciones.Visible = true;
                Div_Datos.Visible = false;
                Div_Busqueda.Visible = true;
                Txt_Fecha_Inicio.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                Txt_Fecha_Fin.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                break;
            case "General":
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
                //Manejo de los Div
                Div_Licitaciones.Visible = false;
                Div_Datos.Visible = true;
                Div_Busqueda.Visible = false;
                Txt_Fecha_Inicio.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                Txt_Fecha_Fin.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                Llenar_Combo_Estatus();
                Llenar_Combo_Tipo();
                Llenar_Combo_Clasificacion();
                Txt_Total.Text = "0.0";
                Habilitar_Campos(false);

                Configuracion_Acceso("Frm_Ope_Com_Licitaciones.aspx");
                break;
            case "Nuevo":
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
                //Manejo de los Div
                Div_Licitaciones.Visible = false;
                Div_Datos.Visible = true;
                Div_Busqueda.Visible = false;
                Llenar_Combo_Estatus();
                Llenar_Combo_Tipo();
                Llenar_Combo_Clasificacion();
                //Fecha
                Txt_Fecha_Inicio.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                Txt_Fecha_Fin.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                Txt_Total.Text = "0.0";
                Habilitar_Campos(true);
                break;
            case "Modificar":
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
                //Manejo de los Div
                Div_Licitaciones.Visible = false;
                Div_Datos.Visible = true;
                Div_Busqueda.Visible = false;
                break;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Habilitar_Campos
    ///DESCRIPCIÓN: Metodo que sirve para habilitar los campos
    ///PARAMETROS: String Estatus: Indica si se encuentra en Inicial,General,Nuevo o Modificar
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Habilitar_Campos(bool Estatus)
    {
        Txt_Justificacion.Enabled = Estatus;
        Txt_Comentario.Enabled = Estatus;
        Cmb_Estatus.Enabled = Estatus;
        Txt_Fecha_Inicio.Enabled = Estatus;
        Btn_Fecha_Inicio.Enabled = Estatus;
        Txt_Fecha_Fin.Enabled = Estatus;
        Btn_Fecha_Fin.Enabled = Estatus;
        Grid_Requisiciones.Enabled = Estatus;
        Cmb_Clasificacion.Enabled = Estatus;
        Cmb_Tipo.Enabled = Estatus;
        Chk_Requisiciones.Enabled = Estatus;
        Chk_Consolidaciones.Enabled = Estatus;
        Btn_Agregar_Requisicion_Consolidaciones.Enabled = Estatus;
        //Btn_Agregar.Enabled = Estatus;
        //Btn_Filtro_Requisiciones.Enabled = Estatus;
        
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Cajas
    ///DESCRIPCIÓN: Metodo que limpia los componentes cajas 
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Limpiar_Cajas()
    {
        Txt_Busqueda.Text = "";
        Txt_Folio.Text = "";
        Txt_Fecha_Inicio.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        Txt_Fecha_Fin.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        Txt_Justificacion.Text = "";
        Txt_Comentario.Text = "";
        //Txt_Requisicion.Text = "";
        Txt_Total.Text = "0.0";
        Grid_Requisiciones.DataBind();        
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar Campos
    ///DESCRIPCIÓN: Metodo que permite validar si estan vacios o no algunos componentes 
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Validar_Campos()
    {
        if (Txt_Justificacion.Text.Trim() == String.Empty)
        {
            Lbl_Mensaje_Error.Text += "+ Es Obligatorio la Justificacion <br/>";
            Div_Contenedor_Msj_Error.Visible = true;
        }
        else{
            Licitaciones_Negocio.P_Justificacion = Txt_Justificacion.Text;
        }//fin del else justificacion
        if (Txt_Comentario.Text.Trim() == String.Empty)
        {
            Lbl_Mensaje_Error.Text += "+ Es Obligatorio un Comentario <br/>";
            Div_Contenedor_Msj_Error.Visible = true;
        }
        else
        {
            Licitaciones_Negocio.P_Comentarios = Txt_Comentario.Text;
        }//fin del else comentatios
        //Validamos las fechas ingresadas por el usuario
        Verificar_Fecha(Txt_Fecha_Inicio, Txt_Fecha_Fin, Lbl_Mensaje_Error);
        if (Lbl_Mensaje_Error.Text != "")
            Div_Contenedor_Msj_Error.Visible = true;
        if (Cmb_Estatus.SelectedIndex == 0)
        {
            Lbl_Mensaje_Error.Text += "+ Es Obligatorio seleccionar un Estatus <br/>";
            Div_Contenedor_Msj_Error.Visible = true;
        }
        else
        {
            Licitaciones_Negocio.P_Estatus = Cmb_Estatus.SelectedValue;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN: Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///en la busqueda del Modalpopup
    ///PARAMETROS: 1.-TextBox Fecha_Inicial 
    ///            2.-TextBox Fecha_Final
    ///            3.-Label Mensaje_Error
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Verificar_Fecha(TextBox Fecha_Inicial, TextBox Fecha_Final, Label Mensaje_Error)
    {

        //Variables que serviran para hacer la convecion a datetime las fechas y poder validarlas 
        DateTime Date1 = new DateTime();
        DateTime Date2 = new DateTime();

            if ((Fecha_Inicial.Text.Length == 11) && (Fecha_Final.Text.Length == 11))
            {
                //Convertimos el Texto de los TextBox fecha a dateTime
                Date1 = DateTime.Parse(Fecha_Inicial.Text);
                Date2 = DateTime.Parse(Fecha_Final.Text);
                //Validamos que las fechas sean iguales o la final sea mayor que la inicias, si no se manda un mensaje de error 
                if ((Date1 < Date2) | (Date1 == Date2))
                {
                    //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                    Licitaciones_Negocio.P_Fecha_Inicio = Formato_Fecha(Fecha_Inicial.Text);
                    Licitaciones_Negocio.P_Fecha_Fin = Formato_Fecha(Fecha_Final.Text);

                }
                else
                {
                    Mensaje_Error.Text += "+ Fecha no valida <br />";
                }
            }
            else
            {
                Mensaje_Error.Text += "+ Fecha no valida <br />";
            }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Formato_Fecha
    ///DESCRIPCIÓN: Metodo que cambia el mes dic a dec para que oracle lo acepte
    ///PARAMETROS:  1.- String Fecha, es la fecha a la cual se le cambiara el formato 
    ///                     en caso de que cumpla la condicion del if
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 2/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public String Formato_Fecha(String Fecha)
    {

        String Fecha_Valida = Fecha;
        //Se le aplica un split a la fecha 
        String[] aux = Fecha.Split('/');
        //Se modifica el es a solo mayusculas para que oracle acepte el formato. 
        switch (aux[1])
        {
            case "dic":
                aux[1] = "DEC";
                break;
        }
        //Concatenamos la fecha, y se cambia el orden a DD-MMM-YYYY para que sea una fecha valida para oracle
        Fecha_Valida = aux[0] + "-" + aux[1] + "-" + aux[2];
        return Fecha_Valida;
    }// fin de Formato_Fecha

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Importe_Acumulado
    ///DESCRIPCIÓN: Metodo que permite agregar un nuevo producto al Grid_Productos
    ///PARAMETROS: 1.- DataTable _DataTable: Data_Table que contiene el nuevo prducto a agregar
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 8/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public double Importe_Acumulado()
    {
        double Importe_total_Acumulado = 0;
        DataTable Dt_Requisiciones = (DataTable)Session["Dt_Requisiciones"];

        //Recorremos el Dt_Productos para calcular el importe 
        if (Dt_Requisiciones.Rows.Count != 0)
        {
            for (int i = 0; i < Dt_Requisiciones.Rows.Count; i++)
            {
                Importe_total_Acumulado = Importe_total_Acumulado + double.Parse(Dt_Requisiciones.Rows[i]["Total"].ToString());
            }
        }
        Session["Total_Licitacion"] = Importe_total_Acumulado;
        return Importe_total_Acumulado;
    }

    #region Metodos_Modal_Requisiciones
    

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Estatus
    ///DESCRIPCIÓN: Metodo que carga el combo Cmb_Estatus
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 8/Noviembre/2010 
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
    ///FECHA_CREO: 8/Noviembre/2010 
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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Tipo
    ///DESCRIPCIÓN: Metodo que carga el combo Cmb_Tipo
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 8/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Clasificacion()
    {
        Cmb_Clasificacion.Items.Clear();
        Cmb_Clasificacion.Items.Add("<<SELECCIONAR>>");
        Cmb_Clasificacion.Items.Add("RESTRINGIDA");
        Cmb_Clasificacion.Items.Add("PUBLICA");
        Cmb_Clasificacion.Items[0].Value = "0";
        Cmb_Clasificacion.Items[0].Selected = true;
    }

    #endregion Fin Metodos_Modal_Requisiciones
   
    #region Modal_Busqueda_Avanzada

    public void Llenar_Combo_Estatus_Busqueda()
    {
        //Cmb_Estatus_Busqueda.Items.Clear();
        //Cmb_Estatus_Busqueda.Items.Add("<<SELECCIONAR>>");
        //Cmb_Estatus_Busqueda.Items.Add("EN CONSTRUCCION");
        //Cmb_Estatus_Busqueda.Items.Add("GENERADA");
        //Cmb_Estatus_Busqueda.Items[0].Value = "0";
        //Cmb_Estatus_Busqueda.Items[0].Selected = true;
    }

    #endregion Fin_Modal_Buisqueda_avanzada


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos_Negocio
    ///DESCRIPCIÓN: Metodo que permite cargar los datos a la clase de negocio 
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Cargar_Datos_Negocio()
    {
        Licitaciones_Negocio.P_Estatus = Cmb_Estatus.SelectedValue;
        Licitaciones_Negocio.P_Justificacion = Txt_Justificacion.Text;
        Licitaciones_Negocio.P_Comentarios = Txt_Comentario.Text;
        Licitaciones_Negocio.P_Fecha_Inicio = Formato_Fecha(Txt_Fecha_Inicio.Text);
        Licitaciones_Negocio.P_Fecha_Fin = Formato_Fecha(Txt_Fecha_Fin.Text);
        Licitaciones_Negocio.P_Monto_Total = Txt_Total.Text;
        Licitaciones_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
        Licitaciones_Negocio.P_Tipo = Cmb_Tipo.SelectedValue;
        Licitaciones_Negocio.P_Clasificacion = Cmb_Clasificacion.SelectedValue;
        //validamos en caso de estar vacia el Dt_Requisiciones
        if (Session["Dt_Requisiciones"] != null || Session["Dt_Consolidaciones"] != null)
        {
            Licitaciones_Negocio.P_Dt_Requisiciones = (DataTable)Session["Dt_Requisiciones"];
            //Generamos la lista de requisiciones seleccionadas por el usuario
            Licitaciones_Negocio.P_Lista_Requisiciones = Generar_Listado_Requisiciones();
        }
        else
        {
            Licitaciones_Negocio.P_Dt_Requisiciones = null;
        }
        //Cargamos las consolidaciones seleccionadas
        if (Session["Dt_Consolidaciones"] != null)
        {
            Licitaciones_Negocio.P_Lista_Consolidaciones = Generar_Listado_Consolidaciones();
        }
        else
        {
            Licitaciones_Negocio.P_Lista_Consolidaciones = null;
        }
        //validamos el folio 
        if (Txt_Folio.Text.Trim() != "")
            Licitaciones_Negocio.P_Folio = Txt_Folio.Text;
        else
            Licitaciones_Negocio.P_Folio = null;
        //Validamos el No_Licitacion
        if (Session["No_Licitacion"] != null)
            Licitaciones_Negocio.P_No_Licitacion = Session["No_Licitacion"].ToString();
        else
            Licitaciones_Negocio.P_No_Licitacion = null;
    }

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
        if (Dt_Requisiciones.Rows.Count != 0)
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
        if(Dt_Consolidaciones != null)
        if (Dt_Consolidaciones.Rows.Count != 0)
        {
            for (int i = 0; i < Dt_Requisiciones.Rows.Count; i++)
            {
                
                if (i == Dt_Requisiciones.Rows.Count - 1)
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
        if (Dt_Consolidaciones != null)
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


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Datos_Negocio
    ///DESCRIPCIÓN: Metodo que permite limpiar los datos de la clase de negocio
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Limpiar_Datos_Negocio()
    {
        Licitaciones_Negocio.P_Estatus = null;
        Licitaciones_Negocio.P_Justificacion = null;
        Licitaciones_Negocio.P_Comentarios = null;
        Licitaciones_Negocio.P_Fecha_Inicio = null;
        Licitaciones_Negocio.P_Fecha_Fin = null;
        Licitaciones_Negocio.P_Monto_Total = null;
        Licitaciones_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
        //validamos en caso de estar vacia el Dt_Requisiciones
        Session["Dt_Requisiciones"] = null;
        Session["Dt_Consolidaciones"] = null;
        Licitaciones_Negocio.P_Dt_Requisiciones = null;
        Licitaciones_Negocio.P_Folio = null;
        Session["No_Licitacion"] = null;
        Session["No_Requisicion"] = null;
        Session["No_Consolidacion"] = null; 
        Session["Total_Licitacion"] = null;
        Licitaciones_Negocio.P_No_Licitacion = null;
        Licitaciones_Negocio.P_No_Consolidacion = null;        
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Datos_Generales
    ///DESCRIPCIÓN: Metodo que valida que se inserten los datos necesarios en el formulario
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    
    public void Validar_Datos_Generales()
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text += "";
        //Validamos el estatus seleccionado 
        if (Cmb_Estatus.SelectedIndex == 0)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text += "+ Es obligatorio seleccionar un estatus <br/>";
        }
        if (Txt_Justificacion.Text.Trim() == String.Empty)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text += "+ Es obligatoria la justificacion <br/>";
        }
        if (Txt_Comentario.Text.Trim() == String.Empty)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text += "+ Es obligatoria un Comentario <br/>";
        }
        if (Cmb_Tipo.SelectedIndex == 0)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text += "+ Es obligatorio seleccionar el tipo PRODUCTO ó SERVICIO<br/>";
        }
        if (Grid_Consolidaciones.Rows.Count == 0 && Grid_Requisiciones.Rows.Count == 0)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text += "+ Es necesario agregar una Requisicion o una Consolidacion<br/>";
        }

        Verificar_Fecha(Txt_Fecha_Inicio, Txt_Fecha_Fin, Lbl_Mensaje_Error);
        if (Lbl_Mensaje_Error.Text != "")
            Div_Contenedor_Msj_Error.Visible = true;

        
    }

    public void Calcular_Total_Licitacion()
    {
        DataTable Dt_Requisiciones = new DataTable();
        DataTable Dt_Consolidaciones = new DataTable();
        Session["Total"] = 0;
        double Total = 0;
        if (Session["Dt_Requisiciones"] != null)
        {
            Dt_Requisiciones = (DataTable)Session["Dt_Requisiciones"];
        }

        if (Session["Dt_Consolidaciones"] != null)
        {
            Dt_Consolidaciones = (DataTable)Session["Dt_Consolidaciones"];
        }

        if (Dt_Requisiciones.Rows.Count != 0)
        {
            for (int i = 0; i < Dt_Requisiciones.Rows.Count; i++)
            {
                Total = Total + double.Parse(Dt_Requisiciones.Rows[i]["Total"].ToString());
            }
        }

        if (Dt_Consolidaciones.Rows.Count != 0)
        {
            for (int i = 0; i < Dt_Consolidaciones.Rows.Count; i++)
            {
                Total = Total + double.Parse(Dt_Consolidaciones.Rows[i]["Total"].ToString());
            }
        }
        Session["Total_Licitacion"] = Total;
    }

    #region Metodos_Tab_Container
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
        //auxiliar para contar el numero de check seleccionados dentro del grid. 
        int num = 0;
        int Num_Check_Sel = Numero_Checks(MyGrid,nombre_check);
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
    ///NOMBRE DE LA FUNCIÓN: Agregar_Consolidacion
    ///DESCRIPCIÓN: Metodo que permite agregar una nueva Consolidacion al Grid_Consolidaciones
    ///PARAMETROS: 1.- DataTable _DataTable: Data_Table que contiene el nuevo prducto a agregar
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 8/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Agregar_Consolidacion(DataTable _DataTable, String[] Consolidaciones_Seleccionadas)
    {
        //Realizamos la consulta del producto seleccionado
        for (int i = 0; i < Consolidaciones_Seleccionadas.Length; i++)
        {
            String Id = Consolidaciones_Seleccionadas[i];
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
                Licitaciones_Negocio.P_No_Consolidacion = Id;
                //Obtengo los datos de la nueva requisicion a insertar en el GridView
                DataTable Dt_Temporal = Licitaciones_Negocio.Consulta_Consolidaciones();
                //
                double Total_licitacion = 0;
                if (Session["Total_Licitacion"] != null)
                    Total_licitacion = double.Parse(Session["Total_Licitacion"].ToString());
                if (!(Dt_Temporal == null))
                {
                    if (Dt_Temporal.Rows.Count > 0)
                    {
                        DataRow Fila_Nueva = Dt.NewRow();
                        //Asignamos los valores a la fila
                        Fila_Nueva["No_Consolidacion"] = Dt_Temporal.Rows[i]["No_Requisicion"].ToString();
                        Fila_Nueva["Folio"] = Dt_Temporal.Rows[i]["Folio"].ToString();
                        Fila_Nueva["Estatus"] = Dt_Temporal.Rows[i]["Estatus"].ToString();
                        Fila_Nueva["Fecha"] = Dt_Temporal.Rows[i]["Fecha"].ToString();
                        Fila_Nueva["Total"] = Dt_Temporal.Rows[i]["Total"].ToString();
                        Fila_Nueva["Lista_Requisiciones"] = Dt_Temporal.Rows[i]["Lista_Requisiciones"].ToString();
                        Dt.Rows.Add(Fila_Nueva);
                        Dt.AcceptChanges();
                        Session["Dt_Consolidaciones"] = Dt;
                        Txt_Total.Text = Session["Total_Licitacion"].ToString();
                        Grid_Consolidaciones.DataSource = Dt;
                        Grid_Consolidaciones.DataBind();
                        Grid_Consolidaciones.Visible = true;
                    }
                }
            }//fin del else
        }//Fin del FOR


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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Agregar_Requisicion
    ///DESCRIPCIÓN: Metodo que permite agregar un nuevo producto al Grid_Productos
    ///PARAMETROS: 1.- DataTable _DataTable: Data_Table que contiene el nuevo prducto a agregar
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 8/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Agregar_Requisicion(DataTable _DataTable, String Requisiciones_ID)
    {
        //Realizamos la consulta del producto seleccionado
            String Id = Requisiciones_ID;
            DataRow[] Filas;
            DataTable Dt = (DataTable)Session["Dt_Requisiciones"];
            Filas = _DataTable.Select("No_Requisicion='" + Id + "'");
            if (Filas.Length > 0)
            {
                //Si se encontro algun coincidencia entre el grupo a agregar con alguno agregado anteriormente, se avisa
                //al usuario que elemento ha agregar ya existe en la tabla de grupos.
                Lbl_Mensaje_Error.Text += "+ No se puede agregar la requisición "+Id+" ya que esta ya se ha agregado <br/>";
                Div_Contenedor_Msj_Error.Visible = true;
            }
            else
            {
                Licitaciones_Negocio.P_Requisicion_ID = Id;
                //Obtengo los datos de la nueva requisicion a insertar en el GridView
                DataTable Dt_Temporal = Licitaciones_Negocio.Consultar_Requisiciones();
                //
                double Total_licitacion = 0;
                if (Session["Total_Licitacion"] != null)
                    Total_licitacion = double.Parse(Session["Total_Licitacion"].ToString());
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
                        Total_licitacion = Total_licitacion + double.Parse(Dt_Temporal.Rows[0]["Total"].ToString());
                        Session["Total_Licitacion"] = Total_licitacion;
                        Txt_Total.Text = Session["Total_Licitacion"].ToString();
                        Grid_Requisiciones.DataSource = Dt;
                        Grid_Requisiciones.DataBind();
                        Grid_Requisiciones.Visible = true;
                    }
                }
            }//fin del else       
    }

    #endregion Fin_Metodos_Tab_Container


    #endregion Fin_Metodos


    ///*******************************************************************************
    /// REGION GRID
    ///*******************************************************************************
    #region Grid

    #region Grid_Licitaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Licitaciones
    ///DESCRIPCIÓN: Metodo que permite llenar el grid de licitaciones
    ///PARAMETROS: 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Ene/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    
    public void Llenar_Grid_Licitaciones()
    {
        DataTable Data_Table = Licitaciones_Negocio.Consultar_Licitaciones();
        if (Data_Table.Rows.Count != 0)
        {
            Grid_Licitaciones.DataSource = Data_Table;
            Grid_Licitaciones.DataBind();
            Session["Dt_Licitaciones"] = Data_Table;
        }
        else
        {
            //Dejamos vacio el grid de licitacion 
            Grid_Licitaciones.DataBind();
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "+ No se encontraron datos <br />";
            Grid_Licitaciones.DataSource = new DataTable();
            Grid_Licitaciones.DataBind();

        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Licitaciones_SelectedIndexChanged
    ///DESCRIPCIÓN: Metodo que permite cargar los datos de acuerdo a lo seleccionado
    ///PARAMETROS: 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Ene/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Licitaciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Cargamos los datos seleccionados a las cajaas de texto 
        Estatus_Formulario("General");
        Llenar_Combo_Estatus();
        GridViewRow Row = Grid_Requisiciones.SelectedRow;
        Licitaciones_Negocio.P_No_Licitacion = Grid_Licitaciones.SelectedDataKey["No_Licitacion"].ToString();
        Session["No_Licitacion"] = Grid_Licitaciones.SelectedDataKey["No_Licitacion"].ToString();
        //Realizamos la consulta que nos traiga todos los datos relacionados con esta licitacion 
        DataTable Dt_Licitacion = Licitaciones_Negocio.Consultar_Licitaciones();
        //Llenamos los datos
        Txt_Folio.Text = Dt_Licitacion.Rows[0][Ope_Com_Licitaciones.Campo_Folio].ToString();
        Cmb_Estatus.SelectedValue = Dt_Licitacion.Rows[0][Ope_Com_Licitaciones.Campo_Estatus].ToString().Trim();
        Txt_Fecha_Inicio.Text = Dt_Licitacion.Rows[0][Ope_Com_Licitaciones.Campo_Fecha_Inicio].ToString();
        Txt_Fecha_Fin.Text = Dt_Licitacion.Rows[0][Ope_Com_Licitaciones.Campo_Fecha_Fin].ToString();
        Txt_Justificacion.Text = Dt_Licitacion.Rows[0][Ope_Com_Licitaciones.Campo_Justificacion].ToString();
        Txt_Comentario.Text = Dt_Licitacion.Rows[0][Ope_Com_Licitaciones.Campo_Comentarios].ToString();
        Txt_Total.Text = Dt_Licitacion.Rows[0][Ope_Com_Licitaciones.Campo_Monto_Total].ToString();
        Cmb_Clasificacion.Enabled = false;
        Cmb_Clasificacion.SelectedIndex = Cmb_Clasificacion.Items.IndexOf(Cmb_Clasificacion.Items.FindByText(Dt_Licitacion.Rows[0][Ope_Com_Licitaciones.Campo_Clasificacion].ToString().Trim()));
        Cmb_Tipo.Enabled = false;
        Cmb_Tipo.SelectedIndex = Cmb_Tipo.Items.IndexOf(Cmb_Tipo.Items.FindByText(Dt_Licitacion.Rows[0][Ope_Com_Licitaciones.Campo_Tipo].ToString().Trim()));
        //Consultamos las requisiciones pertenecientes a esta licitacion
        DataTable Dt_Lic_Detalle = Licitaciones_Negocio.Consultar_Licitacion_Detalle_Requisicion();
        //Cargamos el grid de requisicion
        Grid_Requisiciones.DataSource = Dt_Lic_Detalle;
        Grid_Requisiciones.DataBind();
        //Cargamos las variables de session
        Session["Dt_Requisiciones"] = Dt_Lic_Detalle;
        Session["Total_Licitacion"] = double.Parse(Txt_Total.Text);

        //Cargamos los datos de las consolidaciones seleccionadas para esta licitacion
        DataTable Dt_Lic_Detalle_Consolidaciones = Licitaciones_Negocio.Consultar_Licitacion_Detalle_Consolidacion();
        Grid_Consolidaciones.DataSource = Dt_Lic_Detalle_Consolidaciones;
        Grid_Consolidaciones.DataBind();
        Session["Dt_Consolidaciones"] = Dt_Lic_Detalle_Consolidaciones;        
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Listado_PageIndexChanging
    ///DESCRIPCIÓN: Metodo que permite la paginacion del Grid_Buscar_Requisicion
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 29/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Licitaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Licitaciones.PageIndex = e.NewPageIndex;
        Grid_Licitaciones.DataSource = (DataTable)Session["Dt_Licitaciones"];
        Grid_Licitaciones.DataBind();
        

    }
    #endregion Fin_Grid_Licitaciones

    #region Grid_Buscar_Requisicion


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Buscar_Requisicion_SelectedIndexChanged
    ///DESCRIPCIÓN: Metodo que permite la paginacion del Grid_Buscar_Requisicion
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 29/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    /*protected void Grid_Buscar_Requisicion_SelectedIndexChanged(object sender, EventArgs e)
    {
        ////cargamos el folio de la requisicion seleccionada
        //GridViewRow Row = Grid_Buscar_Requisicion.SelectedRow;
        //Session["No_Requisicion"] = Grid_Buscar_Requisicion.SelectedDataKey["No_Requisicion"].ToString();
        ////Txt_Requisicion.Text = Row.Cells[2].Text;
        //Grid_Buscar_Requisicion.DataBind();
        //Modal_Busqueda_Requisicion.Hide();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Listado_PageIndexChanging
    ///DESCRIPCIÓN: Metodo que permite la paginacion del Grid_Buscar_Requisicion
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 29/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Buscar_Requisicion_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //Grid_Buscar_Requisicion.PageIndex = e.NewPageIndex;
        //Llenar_Grid_Buscar_Requisicion();
        
    }

    public void Llenar_Grid_Buscar_Requisicion()
    {
        //DataTable Data_Table = Licitaciones_Negocio.Consultar_Requisiciones();
        //if (Data_Table.Rows.Count != 0)
        //{
        //    Grid_Buscar_Requisicion.DataSource = Data_Table;
        //    Grid_Buscar_Requisicion.DataBind();
        //}
        //else
        //{
            //Lbl_Mensaje_Error_Requisiciones.Text = "+ No se encontraron datos <br />";
        //}
        //validamos en caso de no encontrar ninguna requisicion 

    }//fin de Fin_Grid_Buscar_Requisiciones
    */
    #endregion Fin_Grid_Buscar_Requisicion

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
        Renglones = ((DataTable)Session["Dt_Requisiciones"]).Select(Ope_Com_Licitacion_Detalle.Campo_No_Requisicion + "='" + Session["No_Requisicion"].ToString() + "'");
        
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
            Calcular_Total_Licitacion();
            Txt_Total.Text = Session["Total_Licitacion"].ToString();
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
        Renglones = ((DataTable)Session["Dt_Consolidaciones"]).Select(Ope_Com_Consolidaciones.Campo_No_Consolidacion + "='" + Session["No_Consolidacion"].ToString() + "'");

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
            Calcular_Total_Licitacion();
            Txt_Total.Text = Session["Total_Licitacion"].ToString();
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

    #endregion Fin_Grid_Consolidacion
    
    #endregion Fin_Grid


    ///*******************************************************************************
    /// REGION EVENTOS
    ///*******************************************************************************
    #region Eventos

    #region Eventos_Barra Busqueda

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Cerrar_Mensaje_Click
    ///DESCRIPCIÓN: Evento del boton Cerrar, que permite limpiar el label de mensaje de error y no mostrar el Div_Contenedor_Mensaje_Error
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 29/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Cerrar_Mensaje_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Evento del boton Salir
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 29/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Limpiar_Cajas();
        //Limpiamos la clase de negocio
        Licitaciones_Negocio = new Cls_Ope_Com_Licitaciones_Negocio();
        //limpiamos las variables de seccion 
        Session["Dt_Requisiciones"] = null;
        Session["Total_Licitacion"] = null;
        Session["No_Requisicion"] = null;
        Session["Dt_Consolidaciones"] = null;
        Session["No_Consolidacion"] = null;
        Session["No_Licitacion"] = null;
        switch (Btn_Salir.ToolTip)
        {
        case "Cancelar":
            Estatus_Formulario("Inicial");
            Limpiar_Datos_Negocio();
            Llenar_Grid_Licitaciones();
        break;
        case "Inicio":
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            Limpiar_Cajas();
        break;
        case "Listado":
            Estatus_Formulario("Inicial");
            Limpiar_Datos_Negocio();
            Llenar_Grid_Licitaciones();
        break;

        }//fin del switch
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Avanzada_Click
    ///DESCRIPCIÓN: Evento del boton busqueda Avanzada
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 29/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Avanzada_Click(object sender, EventArgs e)
    {
        //Modal_Busqueda.Show();
        //Limpiamos la clase de negocio
        Licitaciones_Negocio = new Cls_Ope_Com_Licitaciones_Negocio();
        //Cargamos los datos del Modal de busqueda avanzada
        Lbl_Error_Busqueda.Text = "";
        Llenar_Combo_Estatus_Busqueda();
        Txt_Fecha_Avanzada_1.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        Txt_Fecha_Avanzada_2.Text = DateTime.Now.ToString("dd/MMM/yyyy");


    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
    ///DESCRIPCIÓN: Evento del boton buscar
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 29/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Licitaciones_Negocio = new Cls_Ope_Com_Licitaciones_Negocio();
        if (Txt_Busqueda.Text.Trim() == String.Empty)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text += "+ Es necesario ingresar el folio de la licitacion </br>";
        }
        if (Div_Contenedor_Msj_Error.Visible == false)
        {
            //Cargamos el valor del folio a la clase de negocios
            Licitaciones_Negocio.P_Folio_Busqueda = Txt_Busqueda.Text.Trim();
            //Realizamos el filtro de las licitaciones 
            Llenar_Grid_Licitaciones();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Evento del boton Nuevo
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 29/Diciembre/2010 
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
                Estatus_Formulario("Nuevo");
                Limpiar_Cajas();
                Limpiar_Datos_Negocio();
                Habilitar_Campos(true);
                break;
            case "Dar de Alta":
                //Validamos que los datos ingresados sean correctos
                Validar_Datos_Generales();
                if (Div_Contenedor_Msj_Error.Visible == false)
                {
                    //regresamos al estado inicial
                    Estatus_Formulario("Inicial");
                    Cargar_Datos_Negocio();
                    try
                    {
                        Licitaciones_Negocio.Alta_Licitacion();
                        Limpiar_Datos_Negocio();
                        Estatus_Formulario("Inicial");
                        Llenar_Grid_Licitaciones();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Licitaciones", "alert('Se dio de alta satisfactoriamente la Licitacion');", true);
                    }
                    catch(Exception Ex)
                    {
                        throw new Exception("Error al dar de alta la licitacion :Error[" + Ex.Message + "]");
                    }
                }
                break;
        }

    }



    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Evento del boton modificar
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 29/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        switch(Btn_Modificar.ToolTip)
        {
            case "Modificar":
                Estatus_Formulario("Modificar");
                Habilitar_Campos(true);
                Cmb_Clasificacion.Enabled = false;
                Cmb_Tipo.Enabled = false;

                break;
            case "Actualizar":
                Validar_Datos_Generales();
                if (Div_Contenedor_Msj_Error.Visible == false)
                {
                    try
                    {
                        Cargar_Datos_Negocio();
                        Licitaciones_Negocio.Modificar_Licitacion();
                        Estatus_Formulario("Inicial");
                        Limpiar_Datos_Negocio();
                        Llenar_Grid_Licitaciones();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Licitaciones", "alert('Se modifico satisfactoriamente la Licitacion');", true);
                    }
                    catch (Exception Ex)
                    {
                        throw new Exception("Error al modificar la Licitación :Error[" + Ex.Message + "]");
                    }
                }
                break;
        }
    }

    #endregion Fin_Eventos_Barra_Busqueda

    #region Eventos_Requisiciones

    /*
    protected void Btn_Agregar_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
            //if (Txt_Requisicion.Text.Trim() == String.Empty)
            //{
            //    Div_Contenedor_Msj_Error.Visible = true;
            //    Lbl_Mensaje_Error.Text = "+ Es necesario seleccionar una requisicion <br /> ";
            //}
            //else
            //{
                Grid_Requisiciones.Enabled = true;
            // Txt_Requisicion.Text = "";
                if (Session["Dt_Requisiciones"] != null)
                {
    //                Agregar_Requisicion((DataTable)Session["Dt_Requisiciones"],);
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
                    Dt_Requisiciones.Columns.Add("No_Consolidacion", typeof(System.String));
                    Session["Dt_Requisiciones"] = Dt_Requisiciones;
                    //Llenamos el grid
                    Grid_Requisiciones.DataSource = (DataTable)Session["Dt_Requisiciones"];
                    Grid_Requisiciones.DataBind();
                    //Obtenemos los valores de Monto disponible, Monto_Comprometido 
                    Session["Total_Licitacion"] = 0;
                    Agregar_Requisicion(Dt_Requisiciones);
                    //Limpiamos los componenetes de la Requisicion seleccionada
                    Licitaciones_Negocio.P_Requisicion_ID = null;
                }//fin del else session
           // }//fin del else
    }

    protected void Btn_Filtro_Requisiciones_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        //if (Btn_Filtro_Requisiciones.Visible == true)
        //{
        //    Modal_Busqueda_Requisicion.Show();
        //    //Cargamos los datos del Modal de Buscar Requisiciones
        //    Cargar_Combo_Requisiciones();
        //}//fin del if buscar requisiciones 
    }
    */
    #endregion Fin_Eventos_Requisiciones

    #region Eventos_Modal_Requisiones

    protected void Cmb_Dependencia_SelectedIndexChanged(object sender, EventArgs e)
    {
        //try
        //{
        //    Modal_Busqueda_Requisicion.Show();
        //    if (Cmb_Dependencia.SelectedIndex != 0)
        //    {
        //        Requisicion_Negocio.P_Dependencia_ID = Cmb_Dependencia.SelectedValue;
        //        Cmb_Area.Enabled = true;
        //        Llenar_Combo_Areas();
        //    }
        //    else
        //    {
        //        Cmb_Area.Enabled = false;
        //        Cmb_Area.Items.Clear();
        //        if (Cmb_Area.Items.Count == 0)
        //            Cmb_Area.Items.Add("<<SELECCIONAR>>");
        //        Cmb_Area.SelectedIndex = 0;
        //    }
        //}
        //catch (Exception Ex)
        //{
        //    throw new Exception("Error al modificar la Requisicion. Error: [" + Ex.Message + "]");
        //}
    }

    protected void Btn_Buscar_Requisicion_Click(object sender, EventArgs e)
    {
       // Modal_Busqueda_Requisicion.Show();
       // Lbl_Mensaje_Error_Requisiciones.Text = "";
        //Grid_Buscar_Requisicion.Visible = true;
        ////Validamos los datos seleccionados por el usuario
        //if (Cmb_Area.SelectedIndex != 0)
        //{
        //    Licitaciones_Negocio.P_Area_ID = Cmb_Area.SelectedValue;
        //}
        //if (Cmb_Dependencia.SelectedIndex != 0)
        //{
        //    Licitaciones_Negocio.P_Dependencia_ID = Cmb_Dependencia.SelectedValue;
        //}
        //Verificar_Fecha(Txt_Fecha_1, Txt_Fecha_2, Lbl_Mensaje_Error_Requisiciones);
        //if (Chk_Consolidadas.Checked == true)
        //{
        //    Licitaciones_Negocio.P_Consolidadas = Chk_Consolidadas.Checked;
        //}
        ////Ejecutamos el filtrado en caso de haber pasado las validaciones
        //if (Lbl_Mensaje_Error_Requisiciones.Text == "")
        //{
        //    Llenar_Grid_Buscar_Requisicion();
        //}
    }

    #endregion Fin Eventos_Modal_Requisiones

    #region Busqueda_Avanzada

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Aceptar_Click
    ///DESCRIPCIÓN: Evento del boton Aceptar
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 29/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Aceptar_Click(object sender, EventArgs e)
    {
        Lbl_Error_Busqueda.Text = "";
        //Limpiamos la clase de negocio para realizar la busqueda
        Licitaciones_Negocio = new Cls_Ope_Com_Licitaciones_Negocio();
        //Realializamos las validaciones para mostrar la informacion
        if (Cmb_Estatus_Busqueda.SelectedIndex != 0)
            Licitaciones_Negocio.P_Estatus = Cmb_Estatus_Busqueda.SelectedValue;
        else
            Licitaciones_Negocio.P_Estatus = null;
        Verificar_Fecha(Txt_Fecha_Avanzada_1, Txt_Fecha_Avanzada_2, Lbl_Error_Busqueda);
        Lbl_Error_Busqueda.Visible = true;
        if (Lbl_Error_Busqueda.Text.Trim() == "")
        {
            //Al pasar las validaciones Cargamos los datos a la clase de negocio
            Llenar_Grid_Licitaciones();
            //Modal_Busqueda.Hide();
        }
        else
        {
//            Modal_Busqueda.Show();
        }
    }
    
    #endregion Fin_Busqueda_Avanzada

    ///*******************************************************************************
    /// Region Eventos_Primera_Pestana_TabContainer
    ///*******************************************************************************

    #region Eventos_Primera_Pestana_TabContainer



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
        if (Cmb_Tipo.SelectedIndex == 0)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "+ Es necesario indicar el tipo de licitacion si es de PRODUCTO ó SERVICIO<br/>";
            Chk_Requisiciones.Checked = false;
        }
        if (Cmb_Clasificacion.SelectedIndex == 0)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text += "+ Es necesario indicar la clasificacion que tiene esta licitacion RESTRINGIDA ó PUBLICA <br/>";
            Chk_Requisiciones.Checked = false;
        }

        if (Div_Contenedor_Msj_Error.Visible == false)
        {
            if (Chk_Requisiciones.Checked == true)
            {
                Chk_Consolidaciones.Checked = false;
                Div_Consolidaciones_Busqueda.Visible = false;
                Div_Requisiciones_Busqueda.Visible = true;
                //Cargamos las variables de negocio que se ocupan para dar filtrar las requisiciones
                Licitaciones_Negocio.P_Tipo = Cmb_Tipo.SelectedValue;// Indica si es PRODUCTO ó SERVICIO
                Licitaciones_Negocio.P_Clasificacion = Cmb_Clasificacion.SelectedValue; //Indica si es una licitacion RESTRINGIDA ó PUBLICA
                Licitaciones_Negocio.P_Requisicion_ID = null;
                //Consultamos las requisiciones
                DataTable Dt_Requisiciones = Licitaciones_Negocio.Consultar_Requisiciones();
                if (Dt_Requisiciones.Rows.Count != 0)
                {
                    Grid_Requisiciones_Busqueda.DataSource = Dt_Requisiciones;
                    Grid_Requisiciones_Busqueda.DataBind();
                }
                else
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "+ No se encontraron Requisiciones <br/>";
                }
            }
            if (Chk_Consolidaciones.Checked == false && Chk_Requisiciones.Checked == false)
            {
                Div_Consolidaciones_Busqueda.Visible = false;
                Div_Requisiciones_Busqueda.Visible = false;
            }

        }//fin if proncipal
        
            
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
        if (Cmb_Tipo.SelectedIndex == 0)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "+ Es necesario indicar el tipo de licitacion si es de PRODUCTO ó SERVICIO<br/>";
            Chk_Requisiciones.Checked = false;
        }
        if (Cmb_Clasificacion.SelectedIndex == 0)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text += "+ Es necesario indicar la clasificacion que tiene esta licitacion RESTRINGIDA ó PUBLICA <br/>";
            Chk_Requisiciones.Checked = false;
        }

        if (Div_Contenedor_Msj_Error.Visible == false)
        {
            if (Chk_Consolidaciones.Checked == true)
            {
                Chk_Requisiciones.Checked = false;
                Div_Consolidaciones_Busqueda.Visible = true;
                Div_Requisiciones_Busqueda.Visible = false;
                //Cargamos las variables de negocio que se ocupan para dar filtrar las requisiciones 
                Licitaciones_Negocio.P_Tipo = Cmb_Tipo.SelectedValue;// Indica si es PRODUCTO ó SERVICIO
                Licitaciones_Negocio.P_Clasificacion = Cmb_Clasificacion.SelectedValue; //Indica si es una licitacion RESTRINGIDA ó PUBLICA 
                //Realizamos la busqueda de Consolidaciones
                DataTable Dt_Consolidaciones = Licitaciones_Negocio.Consulta_Consolidaciones();
                if (Dt_Consolidaciones.Rows.Count != 0)
                {
                    Grid_Consolidaciones_Busqueda.DataSource = Dt_Consolidaciones;
                    Grid_Consolidaciones_Busqueda.DataBind();
                }
                else
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "+ No se encontraron Consolidaciones <br/>";
                }


            }
            if (Chk_Consolidaciones.Checked == false && Chk_Requisiciones.Checked == false)
            {
                Div_Consolidaciones_Busqueda.Visible = false;
                Div_Requisiciones_Busqueda.Visible = false;
            }
        }//fin if proncipal
 
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
        if (Div_Contenedor_Msj_Error.Visible == false)
        {

            if (Chk_Consolidaciones.Checked == true)
            {
                //Recorremos grid para detectar los check seleccionados
                String[] Consolidaciones = Check_Box_Seleccionados(Grid_Consolidaciones_Busqueda, "Chk_Consolidacion_Seleccionada", " Consolidacion");
                if (Consolidaciones.Length != 0)
                {
                    //insertamos las consolidaciones en el grid de consolidaciones
                    for (int i = 0; i < Consolidaciones.Length; i++)
                    {
                        if (Consolidaciones.Length != 0)
                        {

                            if (Session["Dt_Consolidaciones"] != null)
                            {
                                Agregar_Consolidacion((DataTable)Session["Dt_Consolidaciones"], Consolidaciones);
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
                                Session["Total_Licitacion"] = 0;
                                Agregar_Consolidacion(Dt_Consolidaciones, Consolidaciones);
                                //Limpiamos los componenetes de la Requisicion seleccionada
                                Licitaciones_Negocio.P_No_Consolidacion = null;
                            }//fin del else session.
                            Calcular_Total_Licitacion();
                            Txt_Total.Text = Session["Total_Licitacion"].ToString();

                        }
                    }
                }
            }
            if (Chk_Requisiciones.Checked == true)
            {
                //Recorremos grid para detectar los check seleccionados
                String[] Requisiciones = Check_Box_Seleccionados(Grid_Requisiciones_Busqueda, "Chk_Requisicion_Seleccionada", "Requisicion");
                if (Requisiciones.Length != 0)
                {
                    //insertamos las requisiciones en el grid de requisiciones
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
                            Session["Total"] = 0;
                            Agregar_Requisicion(Dt_Requisiciones, Requisiciones[i]);
                            //Limpiamos los componenetes de la Requisicion seleccionada
                            Licitaciones_Negocio.P_Requisicion_ID = null;
                        }//fin del else session
                    }//fin del for
                }//fin del if
            }//fin del if Chk_Requisiciones

            //Dejamos limpia la primer pestaña
            //No mostramos los div de la pestaña 1 
            Div_Consolidaciones_Busqueda.Visible = false;
            Div_Requisiciones_Busqueda.Visible = false;
            //limpiamos los checkbox
            Chk_Requisiciones.Checked = false;
            Chk_Consolidaciones.Checked = false;
            //Mostramos la segunda pestaña
            Tab_Requisiciones_Consolidaciones.ActiveTabIndex = 1;

        }//fin del if Div_Contenedor_Msj_Error

    }
    
    #endregion Fin Eventos_Primera_Pestana_TabContainer

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
