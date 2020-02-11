using System;
using System;
using System.IO;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Linq;
using System.Windows.Forms;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Bandeja_Solicitudes_Tramites.Negocio;
using Presidencia.Sessiones;
using Presidencia.Plantillas_Word;
using Presidencia.Registro_Peticion.Datos;
using System.Drawing;
using System.Drawing.Drawing2D;
using Presidencia.Constantes;
using AjaxControlToolkit;
using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Presidencia.Catalogo_Perfiles.Negocio;
using Presidencia.Sessiones;
using Presidencia.Dependencias.Negocios;
using Presidencia.Empleados.Negocios;
using Presidencia.Tramites_Perfiles_Empleados.Negocio;
using System.Data.OracleClient;

public partial class paginas_Tramites_Frm_Ope_Tra_Perfiles_Empleado : System.Web.UI.Page
{
    #region Page Load
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: 
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  25/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
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
                Inicializar_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            }
            string Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergente/Frm_Busqueda_Avanzada_Empleado.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Empleado.Attributes.Add("onclick", Ventana_Modal);

        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    #endregion

    #region Metodos Generales
    ///*******************************************************************************
    ///NOMBRE:          Inicializa_Controles
    ///DESCRIPCIÓN:     Inicializa los controles
    ///PARAMETROS: 
    ///CREO:            Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:      25/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Inicializar_Controles()
    {
        try
        {
            Limpia_Controles();             
            Habilitar_Controles("Inicial"); 
            Llenar_Combo_Perfiles();
            Llenar_Combo_Unidad_Responsable();
            
        }
        catch (Exception ex)
        {
            throw new Exception("Inicializa_Controles " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE:          Limpia_Controles
    ///DESCRIPCIÓN:     Limpa los controles
    ///PARAMETROS: 
    ///CREO:            Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:      25/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Limpia_Controles()
    {
        try
        {
            Txt_Busqueda.Text = "";
            Txt_Filtro_Nombre_Empleado.Text = "";
            Txt_Filtro_Numero_Empleado.Text = "";

            if (Cmb_Filtro_Unidad_Responsable.SelectedIndex > 0)
            {
                Cmb_Filtro_Unidad_Responsable.SelectedIndex = 0;
            }

            if (Cmb_Empleado.SelectedIndex > 0)
            {
                Cmb_Empleado.SelectedIndex = 0;
            }

            if (Cmb_Perfil.SelectedIndex > 0)
            {
                Cmb_Perfil.SelectedIndex = 0;
            }

            Grid_Perfiles_Empleado.DataSource = new DataTable();
            Grid_Perfiles_Empleado.DataBind();

            Grid_Buscar_Perfil.DataSource = new DataTable();
            Grid_Buscar_Perfil.DataBind();

            Session.Remove("Dt_Perfil_Empleado");
            Session.Remove("Grid_Color_Activo"); 
        }
        catch (Exception ex)
        {
            throw new Exception("Limpia_Controles " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE:          Llenar_Combo_Perfiles
    ///DESCRIPCIÓN:     Llenara el combo de los perfiles
    ///PARAMETROS: 
    ///CREO:            Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:      25/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Llenar_Combo_Perfiles()
    { 
        Cls_Cat_Tra_Perfiles_Negocio Negocio_Consultar_Perfil = new Cls_Cat_Tra_Perfiles_Negocio();
        try
        {
            Negocio_Consultar_Perfil.P_Tipo_DataTable = "PERFILES";

            Cmb_Perfil.DataSource = Negocio_Consultar_Perfil.Consultar_DataTable();
            Cmb_Perfil.DataValueField = Cat_Tra_Perfiles.Campo_Perfil_ID;
            Cmb_Perfil.DataTextField = Cat_Tra_Perfiles.Campo_Nombre;
            Cmb_Perfil.DataBind();
            Cmb_Perfil.Items.Insert(0, "< SELECCIONE >");

            Cmb_Perfil.SelectedIndex = 0;

        }
        catch (Exception ex)
        {
            throw new Exception("Llenar_Combo_Perfiles " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE:          Llenar_Combo_Empleados
    ///DESCRIPCIÓN:     Llenara el combo de los perfiles
    ///PARAMETROS: 
    ///CREO:            Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:      25/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Llenar_Combo_Empleados(DataTable Dt_Consulta)
    {
        Cls_Ope_Tra_Perfiles_Empleado_Negocio Negocio_Consultar_Empleado = new Cls_Ope_Tra_Perfiles_Empleado_Negocio();
        try
        {
            //Cmb_Empleado.DataSource = Negocio_Consultar_Empleado.Consultar_Empleado();
            Cmb_Empleado.DataSource = Dt_Consulta;
            Cmb_Empleado.DataValueField = Cat_Empleados.Campo_Empleado_ID;
            Cmb_Empleado.DataTextField = "Nombre_Empleado";
            Cmb_Empleado.DataBind();
            Cmb_Empleado.Items.Insert(0, "< SELECCIONE >");

        }
        catch (Exception ex)
        {
            throw new Exception("Llenar_Combo_Empleados " + ex.Message.ToString());
        }
    }

   
    ///*******************************************************************************
    ///NOMBRE:          Llenar_Combo_Unidad_Responsable
    ///DESCRIPCIÓN:     Llenara el combo de los perfiles
    ///PARAMETROS: 
    ///CREO:            Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:      25/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Llenar_Combo_Unidad_Responsable()
    {
        Cls_Cat_Dependencias_Negocio Negocio_Responsable = new Cls_Cat_Dependencias_Negocio();
        DataTable Dt_Unidad_Responsable = new DataTable();
        try
        {
            //  1 para la unidad resposable
            Dt_Unidad_Responsable = Negocio_Responsable.Consulta_Dependencias();
            //   2 SE ORDENA LA TABLA POR 
            DataView Dv_Ordenar = new DataView(Dt_Unidad_Responsable);
            Dv_Ordenar.Sort = Cat_Dependencias.Campo_Nombre;

            Dt_Unidad_Responsable = Dv_Ordenar.ToTable();
            Cmb_Filtro_Unidad_Responsable.DataSource = Dt_Unidad_Responsable;
            Cmb_Filtro_Unidad_Responsable.DataValueField = Cat_Dependencias.Campo_Dependencia_ID;
            Cmb_Filtro_Unidad_Responsable.DataTextField = Cat_Dependencias.Campo_Nombre;
            Cmb_Filtro_Unidad_Responsable.DataBind();
            Cmb_Filtro_Unidad_Responsable.Items.Insert(0, "< SELECCIONE >");

        }
        catch (Exception ex)
        {
            throw new Exception("Llenar_Combo_Unidad_Responsable " + ex.Message.ToString());
        }
    }


    ///*******************************************************************************
    ///NOMBRE:          Llenar_Grid_Perfil
    ///DESCRIPCIÓN:     Llenara el combo de los perfiles
    ///PARAMETROS:      Dt_Perfil Es la informacion que se ingresara en la tabla
    ///CREO:            Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:      25/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Llenar_Grid_Perfil(DataTable Dt_Perfil)
    {
        try
        {
            Grid_Perfiles_Empleado.Columns[0].Visible = true;
            Grid_Perfiles_Empleado.Columns[1].Visible = true;
            Grid_Perfiles_Empleado.Columns[2].Visible = true;
            Grid_Perfiles_Empleado.DataSource = Dt_Perfil;
            Grid_Perfiles_Empleado.DataBind();
            Grid_Perfiles_Empleado.Columns[1].Visible = false;
            Grid_Perfiles_Empleado.Columns[2].Visible = false;
            Grid_Perfiles_Empleado.Columns[0].Visible = false;
            Session["Dt_Perfil_Empleado"] = Dt_Perfil;

        }
        catch (Exception ex)
        {
            throw new Exception("Llenar_Grid_Perfil " + ex.Message.ToString());
        }
    }

    
    ///*******************************************************************************
    ///NOMBRE:          Alta_Perfil_Empleado
    ///DESCRIPCIÓN:     enlazara la informacion con la capa de negocios
    ///PARAMETROS:      
    ///CREO:            Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:      26/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Alta_Perfil_Empleado()
    {
        Cls_Ope_Tra_Perfiles_Empleado_Negocio Negocio_Alta = new Cls_Ope_Tra_Perfiles_Empleado_Negocio();
        try
        {
            Negocio_Alta.P_Dt_Perfil_Empleado = (DataTable)(Session["Dt_Perfil_Empleado"]);
            Negocio_Alta.Guardar_Perfil();
        }
        catch (Exception ex)
        {
            throw new Exception("Alta_Perfil_Empleado " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE:          Modificar_Perfil_Empleado
    ///DESCRIPCIÓN:     enlazara la informacion con la capa de negocios 
    ///                     para poder realizar las modificaciones
    ///PARAMETROS:      
    ///CREO:            Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:      28/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Modificar_Perfil_Empleado()
    {
        Cls_Ope_Tra_Perfiles_Empleado_Negocio Negocio_Modificar = new Cls_Ope_Tra_Perfiles_Empleado_Negocio();
        try
        {
            Negocio_Modificar.P_Dt_Perfil_Empleado = (DataTable)(Session["Dt_Perfil_Empleado"]);
            Negocio_Modificar.P_Empleado_ID = Cmb_Empleado.SelectedValue;
            Negocio_Modificar.Modificar_Perfil();
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar_Perfil_Empleado " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE:          Eliminar_Perfil_Empleado
    ///DESCRIPCIÓN:     enlazara la informacion con la capa de negocios para poder eliminar 
    ///                     los empleados con este perfil
    ///PARAMETROS:      
    ///CREO:            Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:      28/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Eliminar_Perfil_Empleado()
    {
        OracleConnection Conexion = new OracleConnection();
        OracleCommand Comando = new OracleCommand();
        OracleTransaction Transaccion = null;

        Cls_Ope_Tra_Perfiles_Empleado_Negocio Negocio_Modificar = new Cls_Ope_Tra_Perfiles_Empleado_Negocio();
        try
        {
            Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
            Conexion.Open();
            Transaccion = Conexion.BeginTransaction();
            Comando.Connection = Conexion;
            Comando.Transaction = Transaccion;

            for (int Cnt_For = 0; Cnt_For < Grid_Perfiles_Empleado.Rows.Count; Cnt_For++)
            {
                Negocio_Modificar.P_Empleado_ID = Grid_Perfiles_Empleado.Rows[Cnt_For].Cells[1].Text;
                Negocio_Modificar.P_Perfil_ID = Grid_Perfiles_Empleado.Rows[Cnt_For].Cells[2].Text;
                Negocio_Modificar.P_Cmmd = Comando;
                Negocio_Modificar.Eliminar_Perfil();
            }

            Transaccion.Commit();
        }
        catch (OracleException Ex)
        {
            if (Transaccion != null)
            {
                Transaccion.Rollback();
            }
            throw new Exception("Error: " + Ex.Message);
        }
        catch (DBConcurrencyException Ex)
        {
            if (Transaccion != null)
            {
                Transaccion.Rollback();
            }
            throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
        }
        catch (Exception ex)
        {
            if (Transaccion != null)
            {
                Transaccion.Rollback();
            } 
            throw new Exception("Eliminar_Perfil_Empleado " + ex.Message.ToString());
        }
        finally
        {
            Conexion.Close();
        }
    }


    ///*******************************************************************************
    ///NOMBRE:          Validar_Datos
    ///DESCRIPCIÓN:     Validara los datos para poder continuar con el proceso
    ///PARAMETROS: 
    ///CREO:            Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:      25/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Boolean Validar_Datos()
    {
        String Espacios_Blanco;
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
        Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";

        try
        {
            if (Session["Dt_Perfil_Empleado"] == null)
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "* Debe de asignar un perfil a un empleado. <br>";
                Datos_Validos = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Llenar_Grid_Perfil " + ex.Message.ToString());
        }

        return Datos_Validos;
    }

    ///*******************************************************************************
    ///NOMBRE:          Validar_Perfil
    ///DESCRIPCIÓN:     Validara los datos para poder ingresar el perfil al grid
    ///PARAMETROS: 
    ///CREO:            Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:      25/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Boolean Validar_Perfil()
    {
        String Espacios_Blanco;
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
        Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";

        if (Cmb_Empleado.Items.Count == 0)
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "* Seleccione algun empleado. <br>";
            Datos_Validos = false;
        }
        else
        {
            if (Cmb_Empleado.SelectedIndex == 0)
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "* Seleccione algun empleado. <br>";
                Datos_Validos = false;
            }
        }

        if (Cmb_Perfil.SelectedIndex == 0)
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "* Seleccione algun perfil. <br>";
            Datos_Validos = false;
        }

        return Datos_Validos;
    }
            
    ///*******************************************************************************
    ///NOMBRE:          
    ///DESCRIPCIÓN:     Habilitara los controles para poder ealizar las operaciones
    ///PARAMETROS:      Boolean Habilitar:si mostrara el mensaje de error
    ///CREO:            Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:      25/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Mostrar_Mensaje(Boolean Habilitar , String Texto_Error)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = Habilitar;
            Img_Error.Visible = Habilitar;
            Lbl_Mensaje_Error.Text = Texto_Error;
        }
        catch (Exception ex)
        {
            throw new Exception("Mostrar_Mensaje " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE:          Habilitar_Controles
    ///DESCRIPCIÓN:     Habilitara los controles para poder ealizar las operaciones
    ///PARAMETROS:      String Operaciones:el tipo de operacion
    ///CREO:            Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:      25/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
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
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Salir";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = true;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    break;

                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Eliminar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    break;

                case "Modificar":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    break;
            }
            Txt_Busqueda.Enabled = !Habilitado;
            Txt_Filtro_Nombre_Empleado.Enabled = Habilitado;
            Txt_Filtro_Numero_Empleado.Enabled = Habilitado;
            Cmb_Filtro_Unidad_Responsable.Enabled = Habilitado;
            Cmb_Empleado.Enabled = false;
            Cmb_Perfil.Enabled = Habilitado;
            Grid_Perfiles_Empleado.Enabled = Habilitado;
            Btn_Buscar_Empleado.Enabled = Habilitado;
            Btn_Buscar_Empleado.Visible = Habilitado;

            Mostrar_Mensaje(false, "");
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    #endregion

    #region Botones
    ///*******************************************************************************
    ///NOMBRE:          Btn_Nuevo_Click
    ///DESCRIPCIÓN:     Realizara los metodos requeridos para dar de alta el perfil
    ///PARAMETROS: 
    ///CREO:            Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:      26/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Mostrar_Mensaje(false, "");
           
            if (Btn_Nuevo.ToolTip == "Nuevo")
            {
                Inicializar_Controles();          //Limpia los controles de la forma para poder introducir nuevos datos
                Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
            }
            else
            {
                if (Validar_Datos())
                {
                    Alta_Perfil_Empleado();
                    Inicializar_Controles();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Btn_Nuevo_Click", "alert('Alta de Perfil Exitosa');", true);
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Btn_Nuevo_Click " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE:          Btn_Modificar_Click
    ///DESCRIPCIÓN:     Realizara los metodos requeridos para modificar el perfil
    ///PARAMETROS: 
    ///CREO:            Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:      28/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Mostrar_Mensaje(false, "");

            if (Btn_Modificar.ToolTip == "Modificar")
            {
                if (Cmb_Empleado.SelectedIndex > 0)
                {
                    Habilitar_Controles("Modificar");
                }
                else
                {
                    Mostrar_Mensaje(true , "Seleccione algun perfil");
                }
            }
            else
            {
                if (Validar_Datos())
                {
                    Modificar_Perfil_Empleado();
                    Inicializar_Controles();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Btn_Modificar_Click", "alert('Modificacion Exitosa');", true);
                }
            }
            
        }
        catch (Exception ex)
        {
            throw new Exception("Btn_Modificar_Click " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    ///NOMBRE:          Btn_Eliminar_Click
    ///DESCRIPCIÓN:     Realizara los metodos requeridos para eliminar un perfil
    ///PARAMETROS: 
    ///CREO:            Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:      28/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Mostrar_Mensaje(false, "");

            if (Grid_Perfiles_Empleado.Rows.Count > 0)
            {
                Eliminar_Perfil_Empleado();
                Inicializar_Controles();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Btn_Eliminar_Click", "alert('Baja Exitosa');", true);
            }
            else
            {
                Mostrar_Mensaje(true, "Seleccione algun perfil");
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Btn_Eliminar_Click " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    ///NOMBRE:          Btn_Salir_Click
    ///DESCRIPCIÓN:     Realizara los metodos requeridos para salir
    ///PARAMETROS: 
    ///CREO:            Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:      25/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
       if (Btn_Salir.ToolTip == "Salir")
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
       else
       {
           Inicializar_Controles();
       }
        
    }
    ///*******************************************************************************
    ///NOMBRE:          Btn_Buscar_Click
    ///DESCRIPCIÓN:     Realizara los metodos requeridos buscar un perfil
    ///PARAMETROS: 
    ///CREO:            Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:      25/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Dt_Datos_Empleado = new DataTable();
        Cls_Ope_Tra_Perfiles_Empleado_Negocio Negocio_Filtro_Unidad_Responsable = new Cls_Ope_Tra_Perfiles_Empleado_Negocio();
        try
        {
            Mostrar_Mensaje(false, "");
            Div_Buscar_Perfil.Style.Value = "display:block";
            Negocio_Filtro_Unidad_Responsable.P_Nombre_Empleado = Txt_Busqueda.Text.ToUpper().Trim();
            Dt_Datos_Empleado = Negocio_Filtro_Unidad_Responsable.Consultar_Empleado();
            Session["Dt_Empleado_Grid"] = Dt_Datos_Empleado;
            Llenar_Grid_Perfiles_Busqueda(Dt_Datos_Empleado);
        }
        catch (Exception ex)
        {
            throw new Exception("Btn_Buscar_Click " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    ///NOMBRE:          Llenar_Grid_Perfiles
    ///DESCRIPCIÓN:     Realizara los metodos requeridos buscar un perfil
    ///PARAMETROS: 
    ///CREO:            Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:      25/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Llenar_Grid_Perfiles_Busqueda(DataTable Dt_Perfil)
    {
        try
        {
            Grid_Buscar_Perfil.Columns[1].Visible = true;
            Grid_Buscar_Perfil.DataSource = Dt_Perfil;
            Grid_Buscar_Perfil.DataBind();
            Grid_Buscar_Perfil.Columns[1].Visible = false;

            if (Dt_Perfil != null && Dt_Perfil.Rows.Count > 0)
            {
                Grid_Buscar_Perfil.SelectedIndex = -1;
            }

        }
        catch (Exception ex)
        {
            throw new Exception("Llenar_Grid_Perfiles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    ///NOMBRE:          Btn_Buscar_Empleado_Click
    ///DESCRIPCIÓN:     Realizara los metodos requeridos buscar al empleado
    ///PARAMETROS: 
    ///CREO:            Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:      26/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Empleado_Click(object sender, EventArgs e)
    {
        DataTable Dt_Empleado = new DataTable();
        String Empleado_ID = ""; 
        Cls_Ope_Tra_Perfiles_Empleado_Negocio Negocio_Buscar_Perfil = new Cls_Ope_Tra_Perfiles_Empleado_Negocio();
        DataTable Dt_Perfil = new DataTable(); 
        DataTable Dt_Datos_Empleado = new DataTable();
        try
        {

            if (Session["BUSQUEDA_EMPLEADO"] != null)
            {
                Boolean Estado = Convert.ToBoolean(Session["BUSQUEDA_EMPLEADO"].ToString());

                if (Estado != false)
                {
                    Empleado_ID = Session["EMPLEADO_ID"].ToString();

                    Llenar_Combo_Empleados((DataTable)Session["Dt_Empleados"]);

                    if (Cmb_Empleado.Items.FindByValue(Empleado_ID) != null)
                    {
                        Cmb_Empleado.SelectedValue = Empleado_ID;

                        Negocio_Buscar_Perfil.P_Empleado_ID = Empleado_ID;
                        Dt_Perfil = Negocio_Buscar_Perfil.Consultar_Perfil();

                        //  se llena el grid principal
                        if (Dt_Perfil != null && Dt_Perfil.Rows.Count > 0)
                            Llenar_Grid_Perfil(Dt_Perfil);

                        Llenar_Combo_Empleados((DataTable)Session["Dt_Empleados"]);

                        //  se carga el combo con el peril y se deshabilita
                        Dt_Empleado = (DataTable)(Session["Dt_Empleado_Grid"]);
                    }

                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Btn_Buscar_Empleado_Click " + ex.Message.ToString(), ex);
        }
    }
    

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Cerrar_Busqueda_Empleado_Click
    ///DESCRIPCIÓN: oculta el div con los filtros para realizar la busqueda de los tramites
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  26/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Cerrar_Busqueda_Empleado_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Div_Filtro_Empleados.Style.Value = "display:none";
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

     ///*******************************************************************************
    ///NOMBRE:          Btn_Buscar_Empleado_Filtro_Click
    ///DESCRIPCIÓN:     Realizara los metodos requeridos buscar un perfil
    ///PARAMETROS: 
    ///CREO:            Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:      25/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Empleado_Filtro_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Cat_Empleados_Negocios Negocio_Empleado = new Cls_Cat_Empleados_Negocios();
        DataTable Dt_Datos_Empleado = new DataTable();
        Cls_Ope_Tra_Perfiles_Empleado_Negocio Negocio_Filtro_Unidad_Responsable = new Cls_Ope_Tra_Perfiles_Empleado_Negocio(); 
        Cls_Ope_Tra_Perfiles_Empleado_Negocio Negocio_Buscar_Perfil = new Cls_Ope_Tra_Perfiles_Empleado_Negocio();
        DataTable Dt_Perfil = new DataTable();
        DataTable Dt_Empleado = new DataTable();
        try
        {
            //  para el nombre
            if (!String.IsNullOrEmpty(Txt_Filtro_Nombre_Empleado.Text))
            {
                Negocio_Filtro_Unidad_Responsable.P_Nombre_Empleado = Txt_Filtro_Nombre_Empleado.Text.ToUpper().Trim();
            }
            //  para el numero de empleado
            if (!String.IsNullOrEmpty(Txt_Filtro_Numero_Empleado.Text))
            {
                //String.Format("{0:00000}", Convert.ToInt32(Reloj_Checador_ID) + 1);
                Negocio_Filtro_Unidad_Responsable.P_Numero_Empleado = String.Format("{0:000000}", Convert.ToInt32(Txt_Filtro_Numero_Empleado.Text.ToUpper().Trim()));
            }

            //  para la unidad responsable
            if (Cmb_Filtro_Unidad_Responsable.SelectedIndex > 0)
            {
                Negocio_Filtro_Unidad_Responsable.P_Unidad_Responsable_ID = Cmb_Filtro_Unidad_Responsable.SelectedValue;
            }
            //Consulta_Empleados_Dependencia
            Dt_Datos_Empleado = Negocio_Filtro_Unidad_Responsable.Consultar_Empleado();

            Llenar_Combo_Empleados(Dt_Datos_Empleado);

            
        }
        catch (Exception ex)
        {
            throw new Exception("Btn_Buscar_Empleado_Click " + ex.Message.ToString(), ex);
        }
    }
    
    ///*******************************************************************************
    ///NOMBRE:          Btn_Agregar_Perfil_Click
    ///DESCRIPCIÓN:     agregara el perfil al grid
    ///PARAMETROS: 
    ///CREO:            Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:      25/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Agregar_Perfil_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Ope_Tra_Perfiles_Empleado_Negocio Negocio_Buscar_Perfil = new Cls_Ope_Tra_Perfiles_Empleado_Negocio();
        DataTable Dt_Perfil = new DataTable();
        DataTable Dt_Consultar_Empleado = new DataTable();
        Boolean Estado = false;
        try
        {
            Mostrar_Mensaje(false, "");

            if (Validar_Perfil())
            {
                if (Session["Dt_Perfil_Empleado"] == null)
                {
                    Dt_Perfil = new DataTable("Dt_Perfil_Empleado");
                    Dt_Perfil.Columns.Add("EMPLEADO_ID", Type.GetType("System.String"));
                    Dt_Perfil.Columns.Add("PERFIL_ID", Type.GetType("System.String"));
                    Dt_Perfil.Columns.Add("NOMBRE_EMPLEADO", Type.GetType("System.String"));
                    Dt_Perfil.Columns.Add("NOMBRE_PERFIL", Type.GetType("System.String"));
                }
                else
                {
                    Dt_Perfil = (DataTable)Session["Dt_Perfil_Empleado"];
                }

                //  buscar que no se encuentre dentro del grid
                if (Dt_Perfil.Rows.Count > 0)
                {
                    foreach (DataRow Registro in Dt_Perfil.Rows)
                    {
                        if ((Cmb_Perfil.SelectedValue == Registro["PERFIL_ID"].ToString()) &&
                            (Cmb_Empleado.SelectedValue == Registro["EMPLEADO_ID"].ToString()))
                        {
                            Estado = true;
                        }
                    }
                }

                if (Estado == false)
                {
                    DataRow Fila = Dt_Perfil.NewRow();
                    Fila["EMPLEADO_ID"] = Cmb_Empleado.SelectedValue;
                    Fila["PERFIL_ID"] = Cmb_Perfil.SelectedValue;
                    Fila["NOMBRE_EMPLEADO"] = Cmb_Empleado.SelectedItem.ToString();
                    Fila["NOMBRE_PERFIL"] = Cmb_Perfil.SelectedItem.ToString();
                    Dt_Perfil.Rows.Add(Fila);

                    Llenar_Grid_Perfil(Dt_Perfil);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Btn_Agregar_Perfil_Click",
                       "alert('El empleado [" + Cmb_Empleado.SelectedItem.ToString() + "] ya cuenta con el perfil [" +
                       Cmb_Perfil.SelectedItem.ToString() + "] asignado por usted');", true);
                }

            }
            else
            {
                Mostrar_Mensaje(true, Lbl_Mensaje_Error.Text);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Btn_Agregar_Perfil_Click " + ex.Message.ToString(), ex);
        }
    }
     ///*******************************************************************************
    ///NOMBRE:          Btn_Img_Quitar_OnClick
    ///DESCRIPCIÓN:     Realizara los el perfil del grid
    ///PARAMETROS: 
    ///CREO:            Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:      25/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Img_Quitar_OnClick(object sender, ImageClickEventArgs e)
    {
        try
        {
            Mostrar_Mensaje(false, "");

            ImageButton Btn_Eliminar = (ImageButton)sender;
            TableCell Tabla = (TableCell)Btn_Eliminar.Parent;
            GridViewRow Row = (GridViewRow)Tabla.Parent;
            Grid_Perfiles_Empleado.SelectedIndex = Row.RowIndex;
            int Fila = Row.RowIndex;

            DataTable Dt_Elimiar_Registro = (DataTable)Session["Dt_Perfil_Empleado"];
            Dt_Elimiar_Registro.Rows.RemoveAt(Fila);
            Session["Dt_Perfil_Empleado"] = Dt_Elimiar_Registro;
            Grid_Perfiles_Empleado.SelectedIndex = (-1);
            Llenar_Grid_Perfil(Dt_Elimiar_Registro);


        }
        catch (Exception ex)
        {
            throw new Exception("Btn_Img_Quitar_OnClick " + ex.Message.ToString(), ex);
        }
    }
    
    #endregion


    #region Combos
    ///*******************************************************************************
    ///NOMBRE:          Cmb_Empleado_OnSelectedIndexChanged
    ///DESCRIPCIÓN:     ocultara el div de busqueda de empleado
    ///PARAMETROS: 
    ///CREO:            Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:      28/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Empleado_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        Div_Filtro_Empleados.Style.Value = "display:none";
    }
    #endregion


    #region Grid
    ///********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Buscar_Perfil_RowDataBound
    ///DESCRIPCIÓN          : cargara los botones
    ///PROPIEDADES          :
    ///CREO                 : Hugo Enrique Ramirez Aguilera
    /// FECHA_CREO          : 18/Diciembre/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    protected void Grid_Buscar_Perfil_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Color Color_Grid = Color.Yellow;
        String Estatus = "";
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Estatus = e.Row.Cells[3].Text;

                if (Session["Grid_Color_Activo"] == null)
                {
                    if (Estatus == "INACTIVO")
                    {
                        Session["Grid_Color_Activo"] = Estatus;
                        for (int Cnt_For = 0; Cnt_For < 5; Cnt_For++)
                        {
                            e.Row.Cells[Cnt_For].BackColor = Color_Grid;
                        }
                    }
                }
            }
            else
            {
                Session.Remove("Grid_Color_Activo");
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    ///*******************************************************************************
    ///NOMBRE:          Grid_Buscar_Perfil_OnSelectedIndexChanged
    ///DESCRIPCIÓN:     cargara todos los empleados que tengan
    ///PARAMETROS: 
    ///CREO:            Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:      28/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Buscar_Perfil_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_Tra_Perfiles_Empleado_Negocio Negocio_Buscar_Perfil = new Cls_Ope_Tra_Perfiles_Empleado_Negocio();
        DataTable Dt_Perfil = new DataTable();
        GridViewRow SelectedRow;
        String Empleado_ID = "";
        String Nombre_Empleado = "";
        DataTable Dt_Empleado = new DataTable();
        try
        {
            if (Grid_Buscar_Perfil.SelectedIndex > (-1))
            {
                
                SelectedRow = Grid_Buscar_Perfil.Rows[Grid_Buscar_Perfil.SelectedIndex];
                Empleado_ID = HttpUtility.HtmlDecode(SelectedRow.Cells[1].Text).ToString();
                Nombre_Empleado = HttpUtility.HtmlDecode(SelectedRow.Cells[2].Text).ToString();
                Limpia_Controles();
                Negocio_Buscar_Perfil.P_Empleado_ID = Empleado_ID;
                Dt_Perfil = Negocio_Buscar_Perfil.Consultar_Perfil();

                //  se llena el grid principal
                if (Dt_Perfil != null && Dt_Perfil.Rows.Count > 0)
                    Llenar_Grid_Perfil(Dt_Perfil);
                
                //  se carga el combo con el peril y se deshabilita
                Dt_Empleado = (DataTable)(Session["Dt_Empleado_Grid"]);
                Llenar_Combo_Empleados(Dt_Empleado);
                Cmb_Empleado.SelectedIndex = Cmb_Empleado.Items.IndexOf(Cmb_Empleado.Items.FindByValue(Empleado_ID));
                Cmb_Empleado.Enabled = false;
                Div_Buscar_Perfil.Style.Value = "display:none";
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Grid_Buscar_Perfil_OnSelectedIndexChanged " + ex.Message.ToString(), ex);
        }
    }
    #endregion
}
