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
using Presidencia.Administrar_Stock.Negocios;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using System.Collections.Generic;
using Presidencia.Empleados.Negocios;
using Presidencia.Reportes;

public partial class paginas_Compras_Frm_Ope_Com_Alm_Aplicar_Inventario_Stock : System.Web.UI.Page
{
    # region Variables
    
    Cls_Ope_Com_Alm_Administrar_Stock_Negocios Negocio_Inventarios = new Cls_Ope_Com_Alm_Administrar_Stock_Negocios(); // Objeto de la clase de Negocios

    # endregion

    #region Page_Load
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Consultar_Inventarios();

            if (Btn_Busqueda_Avanzada.Visible)
            {
                Configuracion_Acceso_LinkButton("Frm_Ope_Com_Alm_Aplicar_Inventario_Stock.aspx");

                if (Btn_Busqueda_Avanzada.Visible)
                {
                    Txt_Busqueda.Visible = true;
                    Btn_Buscar.Visible = true;
                    Btn_Busqueda_Avanzada.Visible = true;
                }
                else
                {
                    Txt_Busqueda.Visible = false;
                    Btn_Buscar.Visible = false;
                    Btn_Busqueda_Avanzada.Visible = false;
                }
            }
        }
    }
    #endregion

    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Inventarios
    ///DESCRIPCIÓN:          Método utilizado para consultar los inventarios con estatus "Capturado"
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernándz Ramírez
    ///FECHA_CREO:           24/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Consultar_Inventarios()
    {
        System.Data.DataTable Data_Table_Inventarios = new System.Data.DataTable();
        Negocio_Inventarios.P_Tipo_DataTable = "INVENTARIOS";
        Negocio_Inventarios.P_Estatus = "CAPTURADO";

        if (Txt_Busqueda.Text.Trim() != "")
        {
            Negocio_Inventarios.P_No_Inventario = Txt_Busqueda.Text.Trim();
        }
        else
        {
            Negocio_Inventarios.P_No_Inventario = null;
        }
        
      try
        { 
            Data_Table_Inventarios = Negocio_Inventarios.Consultar_DataTable();
            if( Data_Table_Inventarios.Rows.Count>0 ){
                Session["Data_Table_Inventarios"] = Data_Table_Inventarios;
                Grid_Inventario_Capturados.Columns[5].Visible = true;
                Grid_Inventario_Capturados.DataSource = Data_Table_Inventarios;
                Grid_Inventario_Capturados.DataBind();
                Grid_Inventario_Capturados.Columns[5].Visible = false;
                Div_Inventarios_Capturados.Visible = true;
                Div_Contenedor_Msj_Error.Visible = false;
                Lbl_Inventarios.Visible = true;
                Div_Contenedor_Msj_Error.Visible = false;
            }else{
                Lbl_Mensaje_Error.Text = "No se encontraron inventarios capturados";
                Div_Inventarios_Capturados.Visible = false;
                Div_Contenedor_Msj_Error.Visible = true;
                Div_Productos_Inventario.Visible = false;
                Div_Justificación.Visible = false;
                Btn_Aplicar.Visible = false;
            }
            Div_Productos_Inventario.Visible = false;
            Div_Justificación.Visible = false;
            Estado_Incial_Botones();
        }
        catch (Exception Ex)
        {
             throw new Exception("Error al consultar los inventarios. Error: [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Datos_Inventario
    ///DESCRIPCIÓN:          Método utilizado para mostrar en los TextBox y en el DataGrid los datos del inventario seleccionado por el usuario
    ///PROPIEDADES:          1. Data_Set_Datos_Inventario. Es el dataSet que contiene los datos del inventario seleccionado por el usuario          
    ///CREO:                 Salvador Hernándz Ramírez
    ///FECHA_CREO:           24/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Mostrar_Datos_Inventario(DataSet Data_Set_Datos_Inventario)
    {
        DateTime Fecha_Convertida = new DateTime();
       
        try
        {
            if(Data_Set_Datos_Inventario.Tables[0].Rows.Count>0)
            {
                String Fecha = Data_Set_Datos_Inventario.Tables[0].Rows[0]["FECHA"].ToString();
                Fecha_Convertida = Convert.ToDateTime(Fecha);
                Txt_Fecha_Creo.Text = String.Format("{0:dd/MMM/yyyy}", Fecha_Convertida);
                Txt_Estatus.Text = Data_Set_Datos_Inventario.Tables[0].Rows[0]["ESTATUS"].ToString();
                Txt_Observaciones.Text = HttpUtility.HtmlDecode(Data_Set_Datos_Inventario.Tables[0].Rows[0]["OBSERVACIONES"].ToString());
                TxtUsuario_Creo.Text = HttpUtility.HtmlDecode(Data_Set_Datos_Inventario.Tables[0].Rows[0]["USUARIO_CREO"].ToString());
                Grid_Aplicar_Inventario.DataSource = Data_Set_Datos_Inventario;
                Grid_Aplicar_Inventario.DataBind();
                Btn_Aplicar.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar los detalles del inventario. Error: [" + Ex.Message + "]");
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Estado_Botones_Aplicar_Inventario
    ///DESCRIPCIÓN:          Método utilizado para asignarles la imagen y texto a los botones
    ///PROPIEDADES: 
    ///CREO:                 Salvador Hernándz Ramírez
    ///FECHA_CREO:           24/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Estado_Botones_Aplicar_Inventario()
    {
        Btn_Aplicar.AlternateText = "Aplicar";
        Btn_Aplicar.ToolTip = "Aplicar";
        Btn_Aplicar.ImageUrl = "~/paginas/imagenes/gridview/grid_docto.png";
        Btn_Aplicar.OnClientClick = "return confirm('¿Está seguro de aplicar el inventario?');";
        Btn_Cancelar_Proceso.Visible = true;
        Btn_Salir.Visible = false;
        //Btn_Salir.AlternateText = "Cancelar Proceso";
        //Btn_Salir.ToolTip = "Cancelar Proceso";
        //Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/delete.png";
        //Btn_Salir.OnClientClick = "return confirm('¿Está seguro de terminar el proceso?');";
        Btn_Aplicar.Visible = true;
        Lbl_No_Inventario.Text = "Aplicando Inventario " + Session["No_Inventario"].ToString();
        Lbl_No_Inventario.Visible = true;
        Mostrar_Busqueda(false);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Estado_Incial_Botones
    ///DESCRIPCIÓN:          Método utilizado para asignarles la imagen y texto a los botones como estaban en su estado inicial
    ///PROPIEDADES: 
    ///CREO:                 Salvador Hernándz Ramírez
    ///FECHA_CREO:           26/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Estado_Incial_Botones()
    {
        Btn_Aplicar.AlternateText = "NUEVO";
        Btn_Aplicar.ToolTip = "Aplicar Inventario";
        Btn_Aplicar.ImageUrl = "~/paginas/imagenes/gridview/grid_docto.png";
        Btn_Aplicar.OnClientClick = "return confirm('¿Está seguro de aplicar el inventario?');";
        Btn_Salir.AlternateText = "Salir";
        Btn_Salir.ToolTip = "Salir";
        Btn_Salir.Visible = true;
        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
        Mostrar_Busqueda(true);
        Btn_Cancelar_Proceso.Visible = false;
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Busqueda
    ///DESCRIPCIÓN:          Método utilizado para mostrar y ocultar los controles
    ///                      utilizados para realizar la búsqueda simble y abanzada
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           12/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Mostrar_Busqueda(Boolean Estatus)
    {
        Txt_Busqueda.Visible = Estatus;
        Btn_Buscar.Visible = Estatus;
        Btn_Busqueda_Avanzada.Visible = Estatus;

        if (Btn_Busqueda_Avanzada.Visible)
        {
            Configuracion_Acceso_LinkButton("Frm_Ope_Com_Alm_Aplicar_Inventario_Stock.aspx");

            if (Btn_Busqueda_Avanzada.Visible)
            {
                Txt_Busqueda.Visible = true;
                Btn_Buscar.Visible = true;
                Btn_Busqueda_Avanzada.Visible = true;
            }
            else
            {
                Txt_Busqueda.Visible = false;
                Btn_Buscar.Visible = false;
                Btn_Busqueda_Avanzada.Visible = false;
            }
        }
    }


    #region Autentificacion

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Login
    ///DESCRIPCIÓN:          Método utilizado para validar que el usuario y el password sean correctos, que sean correctos y que pertenezcan al grupo de "JEFE DEPENDENCIA"
    ///PROPIEDADES: 
    ///CREO:                 Salvador Hernándz Ramírez
    ///FECHA_CREO: 26/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public bool Validar_Login()
    {
        Boolean Datos_Correctos = false;

        if (Txt_Login.Text != "" & Txt_Password.Text != "")
        {
            if (Autentificacion(Txt_Login.Text.Trim(), Txt_Password.Text.Trim()))
            {
                Datos_Correctos = true;
            }
            else
            {
                Lbl_Error_login.Text = "Incorrecto, favor de verificar";
                Btn_Img_Error_Login.Visible=true;
                Txt_Login.Focus();
                return Datos_Correctos;
            }
        }
        else
        {
            Datos_Correctos = false;
            if (Txt_Login.Text.Trim() == String.Empty)
            {
                Lbl_Error_login.Text = "Proporcione el Nombre de Usuario para acceder al sistema";
                Btn_Img_Error_Login.Visible = true;
                //Btn_Img_Error_Login.Style[HtmlTextWriterStyle.Visibility] = "Show";
                Txt_Login.Focus();
                return Datos_Correctos;
            }
            if (Txt_Password.Text.Trim() == String.Empty)
            {
                Lbl_Mensaje_Error.Text = "Proporcione la Contraseña para acceder al sistema";
                //Btn_Img_Error_Login.Style[HtmlTextWriterStyle.Visibility] = "Show";
                Btn_Img_Error_Login.Visible = true;
                Txt_Password.Focus();
                return Datos_Correctos;
            }
        }
        return Datos_Correctos;
    }


     ///****************************************************************************************
     ///NOMBRE DE LA FUNCION: Autentificacion
     ///DESCRIPCION :         Verificar que el usuario y password sean validos en el sistema para poder
     ///                      aplicar el inventario
     ///PARAMETROS  :         Login: Nombre de usuario
     ///                      Password: Contraseña
     ///CREO        :         Salvador Hernández Ramírez
     ///FECHA_CREO  :         25-Enero-2011
     ///MODIFICO          :
     ///FECHA_MODIFICO    :
     ///CAUSA_MODIFICACION:
     ///****************************************************************************************
    private bool Autentificacion(String No_Usuario, String Contraseña)
    {
        Boolean Datos_Correctos = false;
        DataTable Dt_Datos;                                                                      //Obtiene los datos de la consulta
        Negocio_Inventarios.P_No_Empleado = No_Usuario.Trim();
        Negocio_Inventarios.P_Password = Contraseña.Trim();
       
        try
        { 
            Dt_Datos = Negocio_Inventarios.Consulta_Datos_Usuario();
            if (Dt_Datos.Rows.Count > 0)
            {
                String No_Empleado_Consultado = Dt_Datos.Rows[0]["NO_EMPLEADO"].ToString().Trim();
                String Password_Consultado = Dt_Datos.Rows[0]["PASSWORD"].ToString().Trim();
                String Grupo_Roles_Id = Dt_Datos.Rows[0]["GRUPO_ROLES_ID"].ToString().Trim();
                String Apellido_Paterno = Dt_Datos.Rows[0]["APELLIDO_PATERNO"].ToString().Trim();
                String Apellido_Materno = Dt_Datos.Rows[0]["APELLIDO_MATERNO"].ToString().Trim();
                String Nombre = Dt_Datos.Rows[0]["NOMBRE"].ToString().Trim();
                String Empleado_ID = Dt_Datos.Rows[0]["EMPLEADO_ID"].ToString().Trim();

                if ((No_Empleado_Consultado == No_Usuario) & (Password_Consultado == Contraseña) & (Grupo_Roles_Id == "00003"))  // Ene sta parte se compara que los datos del usuario esten dados de alta y que  pertenezca al grupo de los Jefes
                {
                    Datos_Correctos = true;
                    Session["No_Empleado_Aplico"] = No_Usuario;
                    Session["Nombre_Empleado_Aplico"] = Apellido_Paterno + " " + Apellido_Materno + " " + Nombre;
                    Session["Empleado_ID_Aplico"] = Empleado_ID;
                    return Datos_Correctos;
                }
                else
                {
                    Txt_Login.Text = "";
                    Txt_Password.Text = "";
                    Txt_Login.Focus();
                    Lbl_Error_login.Text = "El Usuario y Password no son correctos, favor de verificar";
                    //Btn_Img_Error_Login.Style[HtmlTextWriterStyle.Visibility] = "Show";
                    Btn_Img_Error_Login.Visible = true;
                    Txt_Login.Focus();
                    Datos_Correctos = false;
                    return Datos_Correctos;
                }
            }
            else
            {
                Txt_Login.Text = "";
                Txt_Password.Text = "";
                Txt_Login.Focus();
                Lbl_Error_login.Text = "El Usuario y Password no son correctos, favor de verificar";
                //Btn_Img_Error_Login.Style[HtmlTextWriterStyle.Visibility] = "Show";
                Btn_Img_Error_Login.Visible = true;
                Txt_Login.Focus();
                Datos_Correctos = false;
                return Datos_Correctos;
            }
            return Datos_Correctos;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al autentificar. Error: [" + Ex.Message + "]");
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Aceptar_Login_Click
    ///DESCRIPCIÓN:          Maneja el evento click del boton "Btn_Aceptar_Login" el cual es utilizado para validar quelos esten correctos lo datos del usuario para autentificarse
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           19/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Aceptar_Login_Click(object sender, EventArgs e)
    {
        if (Validar_Login())
        {
            Div_Justificación.Visible = true;
            Div_Inventarios_Capturados.Visible = false;
            Div_DataGrid.Visible = false;
            Lbl_No_Inventario.Visible = false;
            Btn_Aplicar.Visible = false;
            Estado_Botones_Aplicar_Inventario();
            Txt_Login.Text = "";
            Txt_Password.Text = "";
        }
        else
        {
            Modal_Login.Show();
            Lbl_Error_login.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Cancelar_Login_Click
    ///DESCRIPCIÓN:          Maneja el evento click del boton "Btn_Cancelar_Login" el cual es utilizado para cancelar la operacion de autentificación
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           19/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Cancelar_Login_Click(object sender, EventArgs e)
    {
        Txt_Login.Text = "";
        Txt_Password.Text = "";
    }
    #endregion

    #region Busqueda


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Formato_Fecha
    ///DESCRIPCIÓN:          Metodo que cambia el mes dic a dec para que oracle lo acepte
    ///PARAMETROS:           1.- String Fecha, es la fecha a la cual se le cambiara el formato 
    ///                      en caso de que cumpla la condicion del if
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           15/Abril/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public String Formato_Fecha(String Fecha)
    {
        String Fecha_Valida = Fecha;
        //Se le aplica un split a la fecha 
        String[] aux = Fecha.Split('/');
        //Se modifica a mayusculas para que oracle acepte el formato. 
        switch (aux[1])
        {
            case "dic":
                aux[1] = "DEC";
                break;
        }
        //Concatenamos la fecha, y se cambia el orden a DD-MMM-YYYY para que sea una fecha valida para oracle
        Fecha_Valida = aux[0] + "-" + aux[1] + "-" + aux[2];
        return Fecha_Valida;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Estatus_Inicial_Busqueda
    ///DESCRIPCIÓN:          Evento utilizado para la configurar inicialmente 
    ///                      los componentes de la busqueda avanzada y mostrar el modal
    ///PARAMETROS:           
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           15/Abril/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Estatus_Inicial_Busqueda()
    {
        Chk_Fecha_Abanzada.Checked = false;
        Txt_Fecha_Inicial_B.Text = "";
        Txt_Fecha_Final_B.Text = "";
        Btn_Calendar_Fecha_Inicial.Enabled = false;
        Btn_Calendar_Fecha_Final.Enabled = false;
        Img_Error_Busqueda.Visible = false;
        Lbl_Error_Busqueda.Text = "";
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN:         Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///                     en la busqueda del Modalpopup
    ///PARAMETROS:   
    ///CREO:                Salvador Hernández Ramírez
    ///FECHA_CREO:          19/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public Boolean Verificar_Fecha()
    {
        DateTime Date1 = new DateTime();  // Variables que serviran para hacer la convecion a datetime las fechas y poder validarlas 
        DateTime Date2 = new DateTime();
        Boolean Validacion = true;

        Lbl_Error_Busqueda.Text = "";

            if ((Txt_Fecha_Inicial_B.Text.Length != 0))
            {
                if ((Txt_Fecha_Inicial_B.Text.Length == 11) && (Txt_Fecha_Final_B.Text.Length == 11))
                {
                    // Convertimos el Texto de los TextBox fecha a dateTime
                    Date1 = DateTime.Parse(Txt_Fecha_Inicial_B.Text);
                    Date2 = DateTime.Parse(Txt_Fecha_Final_B.Text);
                    
                    // Validamos que las fechas sean iguales o la final sea mayor que la inicial, de lo contrario se manda un mensaje de error 
                    if ((Date1 < Date2) | (Date1 == Date2))
                    {
                        Validacion = true;
                    }
                    else
                    {
                        Img_Error_Busqueda.Visible = true;
                        Lbl_Error_Busqueda.Text += "+ Fecha no Valida <br />";
                        Lbl_Error_Busqueda.Visible = true;

                        Validacion = false;
                    }
                }
                else
                {
                    Img_Error_Busqueda.Visible = true;
                    Lbl_Error_Busqueda.Text += "+ Fecha no Valida <br />";
                    Lbl_Error_Busqueda.Visible = true;

                    Validacion = false;
                }
            }
            return Validacion;
    }

    #endregion

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN:          Evento utilizado salir de la página o para poner los Div visibles o no visibles
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernándz Ramírez
    ///FECHA_CREO:           24/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText == "Atras")
        {
            Div_Inventarios_Capturados.Visible = true;
            Div_Productos_Inventario.Visible = false;
            Btn_Aplicar.Visible = false;
            Estado_Incial_Botones();
           
            if (Btn_Aplicar.Visible)
            {
                Configuracion_Acceso("Frm_Ope_Com_Alm_Aplicar_Inventario_Stock.aspx");
            }

        }
        else if (Btn_Salir.AlternateText == "Salir")
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Aplicar_Click
    ///DESCRIPCIÓN:          Evento utilizado para llamar el metodo que actualiza los datos capturados por el usuario con los datos contados por el sistema
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernándz Ramírez
    ///FECHA_CREO:           24/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Aplicar_Click(object sender, ImageClickEventArgs e)
    {
          try
             {  
               if (Btn_Aplicar.AlternateText == "NUEVO")
                {
                   Btn_Img_Error_Login.Visible=false;
                   Lbl_Error_login.Text = "";
                   Upd_Panel.Update();
                      
                    Modal_Login.Show();
                }
                else if (Btn_Aplicar.AlternateText == "Aplicar")
                {
                    if (Txt_Justificacion.Text.Trim() != "")
                    {
                        if (Session["Data_Table_Datos_Inventario"]!= null)
                        Negocio_Inventarios.P_Datos_Productos = (DataTable)Session["Data_Table_Datos_Inventario"];

                        if (Session["No_Inventario"] != null)
                        Negocio_Inventarios.P_No_Inventario = Session["No_Inventario"].ToString();

                        Negocio_Inventarios.P_Estatus = "APLICADO";
                        Negocio_Inventarios.P_Tipo_Ajuste = "APLICÓ";

                        if (Txt_Justificacion.Text.Length > 2000)
                        {
                            Negocio_Inventarios.P_Justificacion = Txt_Justificacion.Text.Substring(0, 2000);
                        }
                        else
                        {
                            Negocio_Inventarios.P_Justificacion = Txt_Justificacion.Text;
                        }

                        if (Session["Nombre_Empleado_Aplico"] != null)
                        Negocio_Inventarios.P_Usuario_Modifico = Session["Nombre_Empleado_Aplico"].ToString();

                        if (Session["No_Empleado_Aplico"] != null)
                        Negocio_Inventarios.P_No_Empleado = Session["No_Empleado_Aplico"].ToString();

                        if (Session["Empleado_ID_Aplico"] != null)
                        Negocio_Inventarios.P_Empleado_ID = Session["Empleado_ID_Aplico"].ToString();

                        Negocio_Inventarios.Aplicar_Inventario(); // Se aplica el inventario
                        Div_Contenedor_Msj_Error.Visible = true;
                        Consultar_Productos_Inventario(); // Se consultan los productos del inventario para mostrarlos en el reporte
                        Txt_Busqueda.Text = "";
                        Consultar_Inventarios();
                        Div_Justificación.Visible = false;
                        Div_Productos_Inventario.Visible = false;
                        Estado_Incial_Botones();
                        Btn_Aplicar.Visible = false;
                        Txt_Justificacion.Text = "";
                        
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Text = "Favor de escribir las observaciones";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                }
          }
          catch (Exception Ex)
          {
              throw new Exception("Error al aplicar el inventario. Error: [" + Ex.Message + "]");
          }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Productos_Inventario
    ///DESCRIPCIÓN:          Metodo utilizado para  consultar
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernándz Ramírez
    ///FECHA_CREO:           09/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Consultar_Productos_Inventario()
    {
        Ds_Alm_Com_Inventario_Stock Ds_Reporte_Stock = new Ds_Alm_Com_Inventario_Stock();
        DataTable Dt_Productos_Inventario = new DataTable();
        DataSet Data_Set_Datos_Inventario = new DataSet();
        String No_Inventario="";
        String Formato = "PDF";
        try
        { 
            if (Session["No_Inventario"] != null)
            No_Inventario = Session["No_Inventario"].ToString();
            
            Negocio_Inventarios.P_No_Inventario = No_Inventario;
            Data_Set_Datos_Inventario = Negocio_Inventarios.Consulta_Inventarios_General();

            Dt_Productos_Inventario = Data_Set_Datos_Inventario.Tables[0];

            String Nombre_Reporte_Crystal = "Rpt_Alm_Com_Rep_Comparativo_Stock.rpt";

            Generar_Reporte(Dt_Productos_Inventario, Ds_Reporte_Stock, Nombre_Reporte_Crystal, Formato);
        }
        catch (Exception Ex)
        {
             throw new Exception("Error al consultar los productos del inventario. Error: [" + Ex.Message + "]");
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN:          Carga el data set físico con el cual se genera el Reporte especificado
    ///PARAMETROS:           1.- Dt_Productos_Inventario.- Contiene la informacion de la consulta a la base de datos
    ///                      2.- Ds_Reporte_Stock, Objeto que contiene la instancia del Data set fisico del Reporte a generar
    ///                      3.- Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
    ///                      4.- Nombre_Archivo, Es el nombre del documento que se va a generar en PDF
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           09/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte(DataTable Dt_Productos_Inventario, DataSet Ds_Reporte_Stock, String Nombre_Reporte_Crystal, String Formato)
    {
        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";
        DataRow Renglon;

        try
        {
            // Se llena la tabla Cabecera del DataSet
            Renglon = Dt_Productos_Inventario.Rows[0];
            Ds_Reporte_Stock.Tables[1].ImportRow(Renglon);

            // Se llena la tabla Detalles del DataSet
            for (int Cont_Elementos = 0; Cont_Elementos < Dt_Productos_Inventario.Rows.Count; Cont_Elementos++)
            {
                Renglon = Dt_Productos_Inventario.Rows[Cont_Elementos]; //Instanciar renglon e importarlo
                Ds_Reporte_Stock.Tables[0].ImportRow(Renglon);
            }

            // Ruta donde se encuentra el reporte Crystal
            Ruta_Reporte_Crystal = "../Rpt/Almacen/" + Nombre_Reporte_Crystal;

            // Se crea el nombre del reporte
            String Nombre_Reporte = "Rpt_Comparativo_Stock_" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));

            // Se da el nombre del reporte que se va generar
            if (Formato == "PDF")
                Nombre_Reporte_Generar = Nombre_Reporte + ".pdf";  // Es el nombre del reporte PDF que se va a generar
            else if (Formato == "Excel")
                Nombre_Reporte_Generar = Nombre_Reporte + ".xls";  // Es el nombre del repote en Excel que se va a generar

            Cls_Reportes Reportes = new Cls_Reportes();
            Reportes.Generar_Reporte(ref Ds_Reporte_Stock, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, Formato);
            Mostrar_Reporte(Nombre_Reporte_Generar, Formato);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al llenar el DataSet. Error: [" + Ex.Message + "]");
        }
    }


    /// *************************************************************************************
    /// NOMBRE:              Mostrar_Reporte
    /// DESCRIPCIÓN:         Muestra el reporte en pantalla.
    /// PARÁMETROS:          Nombre_Reporte_Generar.- Nombre que tiene el reporte que se mostrará en pantalla.
    ///                      Formato.- Variable que contiene el formato en el que se va a generar el reporte "PDF" O "Excel"
    /// USUARIO CREO:        Juan Alberto Hernández Negrete.
    /// FECHA CREO:          3/Mayo/2011 18:20 p.m.
    /// USUARIO MODIFICO:    Salvador Hernández Ramírez
    /// FECHA MODIFICO:      16-Mayo-2011
    /// CAUSA MODIFICACIÓN:  Se asigno la opción para que en el mismo método se muestre el reporte en excel
    /// *************************************************************************************
    protected void Mostrar_Reporte(String Nombre_Reporte_Generar, String Formato)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            if (Formato == "PDF")
            {
                Pagina = Pagina + Nombre_Reporte_Generar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt",
                "window.open('" + Pagina + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
            else if (Formato == "Excel")
            {
                String Ruta = "../../Reporte/" + Nombre_Reporte_Generar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Inventario_Pendientes_SelectedIndexChanged
    ///DESCRIPCIÓN:          Evento utilizado obtener el identificador del Inventario seleccionado por el usuario, y en base a este, mostrar los productos del inventario
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernándz Ramírez
    ///FECHA_CREO:           24/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Inventario_Capturados_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet Data_Set_Datos_Inventario = new DataSet();

        GridViewRow SelectedRow = Grid_Inventario_Capturados.Rows[Grid_Inventario_Capturados.SelectedIndex];//GridViewRow representa una fila individual de un control gridview
        String No_Inventario = Convert.ToString(SelectedRow.Cells[1].Text);
        Session["No_Inventario"] = No_Inventario;
        Lbl_No_Inventario.Text = "No. Inventario  " + No_Inventario;
        Lbl_No_Inventario.Visible = true;
        Negocio_Inventarios.P_No_Inventario = No_Inventario;
        Data_Set_Datos_Inventario = Negocio_Inventarios.Consulta_Inventarios_General();
        Mostrar_Datos_Inventario(Data_Set_Datos_Inventario);
        Session["Data_Table_Datos_Inventario"] = Data_Set_Datos_Inventario.Tables[0];
        Btn_Aplicar.Visible = true;
        Btn_Salir.AlternateText = "Atras";
        Btn_Salir.ToolTip = "Atras";
        Mostrar_Busqueda(false);
        Div_Productos_Inventario.Visible = true;
        Div_DataGrid.Visible = true;
        Div_Inventarios_Capturados.Visible = false;

        if (Btn_Aplicar.Visible)
        {
            Configuracion_Acceso("Frm_Ope_Com_Alm_Aplicar_Inventario_Stock.aspx");
        }
    }



    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Inventario_Capturados_PageIndexChanging
    ///DESCRIPCIÓN:          Maneja el evento para llenar las siguientes páginas del grid con la información de la consulta
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           21/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Inventario_Capturados_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Inventario_Capturados.PageIndex = e.NewPageIndex;

        if (Session["Data_Table_Inventarios"] != null)
        Grid_Inventario_Capturados.DataSource = (DataTable)Session["Data_Table_Inventarios"];

        Grid_Inventario_Capturados.DataBind();
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Aplicar_Inventario_PageIndexChangingf
    ///DESCRIPCIÓN:          Maneja el evento para llenar las siguientes páginas del grid con la información de la consulta
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           21/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Aplicar_Inventario_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Aplicar_Inventario.PageIndex = e.NewPageIndex;

        if (Session["Data_Table_Datos_Inventario"] != null)
        Grid_Aplicar_Inventario.DataSource = (DataTable)Session["Data_Table_Datos_Inventario"];

        Grid_Aplicar_Inventario.DataBind();
    }
    # endregion

    #region Busqueda


    //*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Fecha_B_CheckedChanged
    ///DESCRIPCIÓN:          Evento click del boton "Chk_Fecha_B", el cual es utilizado para habilitar  
    ///                      o deshabilitar los controles del panel de busqueda abanzada
    ///PARAMETROS:  
    ///CREO:                 Salvador Hernàndez Ramírez
    ///FECHA_CREO:           15/Abril/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Fecha_B_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_Fecha_Abanzada.Checked)
        {
            Btn_Calendar_Fecha_Inicial.Enabled = true;
            Btn_Calendar_Fecha_Final.Enabled = true;

            Txt_Fecha_Inicial_B.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Txt_Fecha_Final_B.Text = DateTime.Now.ToString("dd/MMM/yyyy");

            Btn_Aceptar_Busqueda.Enabled = true;
        }
        else
        {
            Btn_Calendar_Fecha_Inicial.Enabled = false;
            Btn_Calendar_Fecha_Final.Enabled = false;

            Btn_Aceptar_Busqueda.Enabled = false;
        }
        Modal_Busqueda.Show();
    }


    //*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Aceptar_Busqueda_Click
    ///DESCRIPCIÓN:          Evento click del boton "Btn_Aceptar_Busqueda", el cual es utilizado para 
    ///                      aceptar el proceso de busqueda abanzada 
    ///PARAMETROS:  
    ///CREO:                 Salvador Hernàndez Ramírez
    ///FECHA_CREO:           15/Abril/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Aceptar_Busqueda_Click(object sender, EventArgs e)
    {
        DataTable Data_Table_Inventarios = new DataTable();
        Negocio_Inventarios.P_Tipo_DataTable = "INVENTARIOS";
        Negocio_Inventarios.P_Estatus = "CAPTURADO";

        try
        {
            if (Verificar_Fecha())
            {
                Negocio_Inventarios.P_Fecha_Inicial = Formato_Fecha(Txt_Fecha_Inicial_B.Text.Trim());
                Negocio_Inventarios.P_Fecha_Final = Formato_Fecha(Txt_Fecha_Final_B.Text.Trim());
                Data_Table_Inventarios = Negocio_Inventarios.Consultar_DataTable();

                if (Data_Table_Inventarios.Rows.Count > 0)
                {
                    Session["Data_Table_Inventarios"] = Data_Table_Inventarios;
                    Grid_Inventario_Capturados.DataSource = Data_Table_Inventarios;
                    Grid_Inventario_Capturados.DataBind();
                    Div_Inventarios_Capturados.Visible = true;
                    Lbl_Inventarios.Visible = true;
                    Img_Error_Busqueda.Visible = false;
                    Lbl_Error_Busqueda.Text = "";
                    Div_Contenedor_Msj_Error.Visible = false;
                    Modal_Busqueda.Hide();
                }
                else
                {
                    Lbl_Error_Busqueda.Text = "No se Encontraron Inventarios ";
                    Img_Error_Busqueda.Visible = true;
                    Div_Contenedor_Msj_Error.Visible = true;
                    Div_Inventarios_Capturados.Visible = false;
                    Div_Productos_Inventario.Visible = false;
                    Div_Justificación.Visible = false;
                    Btn_Aplicar.Visible = false;
                    Modal_Busqueda.Show();
                }
                Estado_Incial_Botones();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los inventarios. Error: [" + Ex.Message + "]");
        }
    }

    //*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Cancelar_Busqueda_Click
    ///DESCRIPCIÓN:          Evento click del boton "Btn_Cancelar_Busqueda", el cual es utilizado para 
    ///                      cancelar el proceso de busqueda abanzada (oculta el modal)
    ///PARAMETROS:  
    ///CREO:                 Salvador Hernàndez Ramírez
    ///FECHA_CREO:           15/Abril/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Cancelar_Busqueda_Click(object sender, EventArgs e)
    {
        Modal_Busqueda.Hide();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Avanzada_Click
    ///DESCRIPCIÓN:          Evento utilizado para mostrar el modal
    ///PARAMETROS:           
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           15/Abril/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Busqueda_Avanzada_Click(object sender, EventArgs e)
    {
        Estatus_Inicial_Busqueda();
        Modal_Busqueda.Show();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
    ///DESCRIPCIÓN:          Evento utilizado para realizar la busqueda simple
    ///PARAMETROS:           
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           15/Abril/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Consultar_Inventarios();
    }

    #endregion

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
            Botones.Add(Btn_Aplicar);

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
                            Cls_Util.Configuracion_Acceso_Sistema_SIAS_AlternateText(Botones, Dr_Menus[0]); // Habilitamos la configuracón de los botones.
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
    protected void Configuracion_Acceso_LinkButton(String URL_Pagina)
    {
        List<LinkButton> Botones = new List<LinkButton>();//Variable que almacenara una lista de los botones de la página.
        DataRow[] Dr_Menus = null;//Variable que guardara los menus consultados.

        try
        {
            //Agregamos los botones a la lista de botones de la página.
            Botones.Add(Btn_Busqueda_Avanzada);

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
            throw new Exception("Error al validar si es un dato numerico. Error [" + Ex.Message + "]");
        }
        return Resultado;
    }
    #endregion

    protected void Btn_Cancelar_Proceso_Click(object sender, ImageClickEventArgs e)
    {
        Div_Inventarios_Capturados.Visible = true;
        Div_Productos_Inventario.Visible = false;
        Btn_Aplicar.Visible = false;
        Div_Justificación.Visible = false;
        Consultar_Inventarios();
        Estado_Incial_Botones();
        Txt_Justificacion.Text = "";
        Btn_Cancelar_Proceso.Visible = false;
    }
}
