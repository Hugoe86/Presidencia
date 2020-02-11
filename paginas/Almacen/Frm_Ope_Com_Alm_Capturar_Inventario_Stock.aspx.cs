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

public partial class paginas_Compras_Frm_Ope_Com_Alm_Capturar_Inventario_Stock : System.Web.UI.Page
{
    # region Variables
    Cls_Ope_Com_Alm_Administrar_Stock_Negocios Negocio_Inventarios = new Cls_Ope_Com_Alm_Administrar_Stock_Negocios(); // Objeto de la clase de Negocios
    # endregion

    #region Page_Load
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["Dt_Inventario_Capturado"]=null;
            Consultar_Inventarios();

            if (Btn_Busqueda_Avanzada.Visible)
            {
                Configuracion_Acceso_LinkButton("Frm_Ope_Com_Alm_Capturar_Inventario_Stock.aspx");

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
        Negocio_Inventarios = new Cls_Ope_Com_Alm_Administrar_Stock_Negocios();
        DataTable Data_Table_Inventarios = new DataTable();

      try
        { 
            Negocio_Inventarios.P_Tipo_DataTable = "INVENTARIOS";
            Negocio_Inventarios.P_Estatus = "PENDIENTE";

            if (Txt_Busqueda.Text.Trim() != "")
                Negocio_Inventarios.P_No_Inventario = Txt_Busqueda.Text.Trim();

               Data_Table_Inventarios = Negocio_Inventarios.Consultar_DataTable();
            
            if(Data_Table_Inventarios.Rows.Count>0){
                Session["Data_Table_Inventarios"] = Data_Table_Inventarios;
                Grid_Inventario_Pendientes.Columns[5].Visible = true;
                Grid_Inventario_Pendientes.DataSource = Data_Table_Inventarios;
                Grid_Inventario_Pendientes.DataBind();
                Grid_Inventario_Pendientes.Columns[5].Visible = false;

                Div_Inventario_Pendientes.Visible = true;
                Div_Contenedor_Msj_Error.Visible = false;
                Lbl_Inventarios.Visible = true;
            }else{
                Lbl_Mensaje_Error.Text = "No se encontraron inventarios pendientes";
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Inventarios.Visible = false;
                Div_Capturar_Inventario.Visible = false;
                Div_Inventario_Pendientes.Visible = false;
                Btn_Guardar.Visible = false;
            }
      }
      catch (Exception Ex)
      {
          throw new Exception("Error al consultar los inventarios. Error: [" + Ex.Message + "]");
      }
    }
    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_TextBox_Grid
    ///DESCRIPCIÓN:          Valida que el usuario capture las cantidades de los productos en todos los TextBox
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernándz Ramírez
    ///FECHA_CREO:           21/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public bool Validar_TextBox_Grid()
    {
        Boolean Validacion = true;
        for (int i = 0; i < Grid_Capturar_Inventario.Rows.Count; i++)
        {
            TextBox Txt_Cantidad_Capturada = (TextBox)Grid_Capturar_Inventario.Rows[i].FindControl("Txt_Cantidad");

            if (Txt_Cantidad_Capturada.Text.Trim() == "")
            {
                Lbl_Mensaje_Error.Text = "Asignar los productos contados";
                Div_Contenedor_Msj_Error.Visible = true;
                Validacion = false;
                return Validacion;
            }
        }
        return Validacion;
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Datos_Inventario
    ///DESCRIPCIÓN:          Carga una configuracion de los controles del Formulario
    ///PROPIEDADES:     
    ///                      1. Data_Set_Datos_Inventario. Es el dataSet que contiene los datos del los inventarios
    ///CREO:                 Salvador Hernándz Ramírez
    ///FECHA_CREO:           13/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Mostrar_Datos_Inventario(DataSet Data_Set_Datos_Inventario)
    {
        DateTime Fecha_Convertida = new DateTime();

        try
        { 
            String Fecha = Data_Set_Datos_Inventario.Tables[0].Rows[0]["FECHA"].ToString();
            Fecha_Convertida = Convert.ToDateTime(Fecha);
            Txt_Fecha_Creo.Text = String.Format("{0:dd/MMM/yyyy}", Fecha_Convertida);
            Txt_Estatus.Text = Data_Set_Datos_Inventario.Tables[0].Rows[0]["ESTATUS"].ToString();
            Txt_Observaciones.Text = HttpUtility.HtmlDecode(Data_Set_Datos_Inventario.Tables[0].Rows[0]["OBSERVACIONES"].ToString());
            TxtUsuario_Creo.Text = HttpUtility.HtmlDecode(Data_Set_Datos_Inventario.Tables[0].Rows[0]["USUARIO_CREO"].ToString());
            Grid_Capturar_Inventario.DataSource = Data_Set_Datos_Inventario;
            Grid_Capturar_Inventario.DataBind();
            Btn_Guardar.Visible = true;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar los detalles del inventario. Error: [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_DataSet_Inventarios_Stock
    ///DESCRIPCIÓN:          Llena el dataSet "Data_Set_Consulta_Inventario" con los productos
    ///                      que pertenecen al inventario seleccionado por el usuario
    ///PARAMETROS:  
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           13/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_DataSet_Inventarios_Stock(String No_Inventario)
    {
        Negocio_Inventarios = new Cls_Ope_Com_Alm_Administrar_Stock_Negocios();
        DataSet Data_Set_Consulta_Inventario;
        
        try
        {
            if (Session["No_Inventario"] != null)
            Negocio_Inventarios.P_No_Inventario = Session["No_Inventario"].ToString();

            Data_Set_Consulta_Inventario = Negocio_Inventarios.Consulta_Inventarios_General();
            Ds_Alm_Com_Inventario_Stock Ds_Reporte_Stock = new Ds_Alm_Com_Inventario_Stock();
        }
        catch (Exception Ex)
        {
             throw new Exception("Error al consultar los inventarios. Error: [" + Ex.Message + "]");
        }
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
                    Lbl_Error_Busqueda.Text += "+ Fecha no valida <br />";
                    Lbl_Error_Busqueda.Visible = true;

                    Validacion = false;
                }
            }
            else
            {
                Img_Error_Busqueda.Visible = true;
                Lbl_Error_Busqueda.Text += "+ Fecha no valida <br />";
                Lbl_Error_Busqueda.Visible = true;

                Validacion = false;
            }
        }
        return Validacion;
    }

    #endregion  

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN:          Evento utilizado salir de la aplicación o para configurar el formulario a sus parametros de inicio
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernándz Ramírez
    ///FECHA_CREO:           13/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText == "Atras")
        {
            Div_Inventario_Pendientes.Visible = true;
            Div_Capturar_Inventario.Visible = false;
            Btn_Guardar.Visible = false;
            Btn_Salir.AlternateText = "Salir";
            Mostrar_Busqueda(true);
        }
        else
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
    }

    
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
    protected void Grid_Inventario_Pendientes_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow SelectedRow = Grid_Inventario_Pendientes.Rows[Grid_Inventario_Pendientes.SelectedIndex];//GridViewRow representa una fila individual de un control gridview
            String No_Inventario = Convert.ToString(SelectedRow.Cells[1].Text);
            Session["No_Inventario"]= No_Inventario;

            Lbl_No_Inventario.Text = "No. Inventario  " + No_Inventario;
            Lbl_No_Inventario.Visible = true;
            DataSet Data_Set_Datos_Inventario = new DataSet();
            Negocio_Inventarios.P_No_Inventario = No_Inventario;
            Data_Set_Datos_Inventario = Negocio_Inventarios.Consulta_Inventarios_General();
            Mostrar_Datos_Inventario(Data_Set_Datos_Inventario);
            Session["Data_Table_Datos_Inventario"] = Data_Set_Datos_Inventario.Tables[0];
            Div_Capturar_Inventario.Visible = true;
            Btn_Guardar.Visible = true;
            Btn_Salir.AlternateText = "Atras";
            Btn_Salir.ToolTip = "Atras";
            Div_Inventario_Pendientes.Visible = false;
            Mostrar_Busqueda(false);
        }catch {
        }
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
            Configuracion_Acceso_LinkButton("Frm_Ope_Com_Alm_Capturar_Inventario_Stock.aspx");

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

        if (Btn_Guardar.Visible)
        {
            Configuracion_Acceso("Frm_Ope_Com_Alm_Capturar_Inventario_Stock.aspx");
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Guardar_Click
    ///DESCRIPCIÓN:          Evento utilizado para guardar en una tabla los productos, sus cantidades "Contados por el usuario y obtener las diferencias"
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernándz Ramírez
    ///FECHA_CREO:           24/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Guardar_Click(object sender, ImageClickEventArgs e)
    {
        try
        { 
            if (Validar_TextBox_Grid())
            {
                DataTable Tabla_Inventario = new DataTable();
                DataTable Data_Table_Productos = new DataTable();
                Data_Table_Productos.Columns.Add("PRODUCTO_ID");
                Data_Table_Productos.Columns.Add("CONTADOS_USUARIO");
                Data_Table_Productos.Columns.Add("DIFERENCIA");
                Data_Table_Productos.Columns.Add("MARBETE");

                Tabla_Inventario = (DataTable)Session["Data_Table_Datos_Inventario"];

                String No_Inventario = Session["No_Inventario"].ToString();
                Lbl_No_Inventario.Text = No_Inventario;
                Negocio_Inventarios.P_No_Inventario = No_Inventario.Trim();

                for (int i = 0; i < Tabla_Inventario.Rows.Count; i++)  // En este for() Se llena la tabla "Data_Table_Productos"
                {
                    TextBox Txt_Cantidad_src = (TextBox)Grid_Capturar_Inventario.Rows[i].FindControl("Txt_Cantidad");
                    Double Cantidad = Convert.ToDouble(Txt_Cantidad_src.Text);
                    Double Existencia = Convert.ToDouble(Tabla_Inventario.Rows[i]["EXISTENCIA"].ToString().Trim());
                    String Marbete = Tabla_Inventario.Rows[i]["MARBETE"].ToString();
                    String Producto_Id = Tabla_Inventario.Rows[i]["PRODUCTO_ID"].ToString();

                    Double Diferencia = 0;
                    if (Cantidad >= Existencia)
                    {
                        Diferencia = (Cantidad - Existencia);
                    }
                    else
                    {
                        Diferencia = (Existencia - Cantidad);
                    }
                    DataRow Registro = Data_Table_Productos.NewRow();
                    Registro["PRODUCTO_ID"] = Producto_Id;
                    Registro["CONTADOS_USUARIO"] = Cantidad;
                    Registro["DIFERENCIA"] = Diferencia;
                    Registro["MARBETE"] = Marbete;
                    Data_Table_Productos.Rows.InsertAt(Registro, i);
                }
                Negocio_Inventarios.P_Datos_Productos = Data_Table_Productos;
                Negocio_Inventarios.P_Estatus = "CAPTURADO";
                Negocio_Inventarios.P_Tipo_Ajuste = "CAPTURÓ";
                Negocio_Inventarios.P_Justificacion = "Se Capturo el inventario";
                Negocio_Inventarios.P_Usuario_Modifico = Cls_Sessiones.Nombre_Empleado;
                Negocio_Inventarios.P_No_Empleado = Cls_Sessiones.No_Empleado;
                Negocio_Inventarios.Guardar_Inventarios_Capturado(); // Se llama al metodo utilizado para  guardar la captura
                Llenar_DataSet_Inventarios_Stock(Session["No_Inventario"].ToString());
                Div_Contenedor_Msj_Error.Visible = false;
                Div_Capturar_Inventario.Visible = false;
                Div_Inventario_Pendientes.Visible = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Captura de Inventario", "alert('Se Capturó el Inventario  " + Session["No_Inventario"].ToString() + "');", true);
                Consultar_Inventarios();
                Btn_Guardar.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al guardar la captura. Error: [" + Ex.Message + "]");
        }
    }

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
        Negocio_Inventarios = new Cls_Ope_Com_Alm_Administrar_Stock_Negocios();
        DataTable Data_Table_Inventarios = new DataTable();

        try
        {
            Negocio_Inventarios.P_Tipo_DataTable = "INVENTARIOS";
            Negocio_Inventarios.P_Estatus = "PENDIENTE";

            if (Verificar_Fecha())
            {
                Negocio_Inventarios.P_Fecha_Inicial = Formato_Fecha(Txt_Fecha_Inicial_B.Text.Trim());
                Negocio_Inventarios.P_Fecha_Final = Formato_Fecha(Txt_Fecha_Final_B.Text.Trim());
                Data_Table_Inventarios = Negocio_Inventarios.Consultar_DataTable();

                if (Data_Table_Inventarios.Rows.Count > 0)
                {
                    Session["Data_Table_Inventarios"] = Data_Table_Inventarios;
                    Grid_Inventario_Pendientes.DataSource = Data_Table_Inventarios;
                    Grid_Inventario_Pendientes.DataBind();
                    Div_Inventario_Pendientes.Visible = true;
                    Div_Contenedor_Msj_Error.Visible = false;
                    Lbl_Inventarios.Visible = true;
                    Modal_Busqueda.Hide();
                    Img_Error_Busqueda.Visible = false;
                    Lbl_Error_Busqueda.Text = "";
                }
                else
                {
                    Lbl_Error_Busqueda.Text = "No se encontraron inventarios ";
                    Img_Error_Busqueda.Visible = true;
                    Lbl_Inventarios.Visible = false;
                    Div_Inventario_Pendientes.Visible = false;
                    Div_Capturar_Inventario.Visible = false;
                    Btn_Guardar.Visible = false;
                    Modal_Busqueda.Show();
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los inventarios de stock. Error: [" + Ex.Message + "]");
        }
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

    #endregion

    # region  GRID

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Inventario_Pendientes_PageIndexChanging
    ///DESCRIPCIÓN:          Maneja el evento para llenar las siguientes páginas del grid con la información de la consulta
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           21/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Inventario_Pendientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Inventario_Pendientes.PageIndex = e.NewPageIndex;

        if (Session["Data_Table_Inventarios"] != null)
        Grid_Inventario_Pendientes.DataSource = (DataTable)Session["Data_Table_Inventarios"];

        Grid_Inventario_Pendientes.DataBind();
    } 
    # endregion
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
            Botones.Add(Btn_Guardar);

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
                    //Consultamos el menú de la página.
                    Dr_Menus = Cls_Sessiones.Menu_Control_Acceso.Select("MENU_ID=" + Request.QueryString["PAGINA"]);

                    if (Dr_Menus.Length > 0)
                    {
                        // Validamos que el menu consultado corresponda a la página a validar.
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

}
