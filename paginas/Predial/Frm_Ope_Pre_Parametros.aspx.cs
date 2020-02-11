using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Operacion_Predial_Parametros.Negocio;

public partial class paginas_Predial_Frm_Ope_Pre_Parametros : System.Web.UI.Page
{

    #region Variables
    private const int Const_Estado_Inicial = 0;
    private const int Const_Estado_Nuevo = 1;    
    private const int Const_Estado_Modificar = 2;        
    private static DataTable Dt_Parametros = new DataTable();
    private static DataTable Dt_Constancias = new DataTable();
    private static String M_Busqueda = "";

    #endregion

    #region Page Load / Init
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            //if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!Page.IsPostBack)
            {
                Estado_Botones(Const_Estado_Inicial);                
            }
            Mensaje_Error();
            Mensaje_Error_Modal();
        }
        catch (Exception Ex)
        {
            //Mensaje_Error(Txt_Pagos.Text.Trim() +" - "+ Ex.Message);
            Estado_Botones(Const_Estado_Inicial);
        }
    }
    
    #endregion
    
    #region Metodos

    #region Metodos Generales
    ///*******************************************************************************
    ///NOMBRE DE LA METODO: LLenar_Combo_Id
    ///        DESCRIPCIÓN: llena todos los combos
    ///         PARAMETROS: 1.- Obj_DropDownList: Combo a llenar
    ///                     2.- Dt_Temporal: DataTable genarada por una consulta a la base de datos
    ///                     3.- Texto: nombre de la columna del dataTable que mostrara el texto en el combo
    ///                     3.- Valor: nombre de la columna del dataTable que mostrara el valor en el combo
    ///                     3.- Seleccion: Id del combo el cual aparecera como seleccionado por default
    ///               CREO: Jesus S. Toledo Rdz.
    ///         FECHA_CREO: 06/9/2010
    ///           MODIFICO:
    ///     FECHA_MODIFICO:
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_ID(DropDownList Obj_DropDownList, DataTable Dt_Temporal, String _Texto, String _Valor, String Seleccion)
    {
        String Texto = "";
        try
        {
            Obj_DropDownList.Items.Clear();
            Obj_DropDownList.Items.Add(new ListItem("< SELECCIONAR >", "0"));
            foreach (DataRow row in Dt_Temporal.Rows)
            {
                if (_Texto.Contains("+"))
                {
                    String[] Array_Texto = _Texto.Split('+');

                    foreach (String Campo in Array_Texto)
                    {
                        Texto = Texto + row[Campo].ToString();
                        Texto = Texto + "  ";
                    }
                }
                else
                {
                    Texto = row[_Texto].ToString();
                }
                Obj_DropDownList.Items.Add(new ListItem(Texto, row[_Valor].ToString()));
                Texto = "";
            }
            Obj_DropDownList.SelectedValue = Seleccion;
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }
    public void Llenar_Combo_ID(DropDownList Obj_DropDownList)
    {
        try
        {
            Obj_DropDownList.Items.Clear();
            Obj_DropDownList.Items.Add(new ListItem("< SELECCIONAR >", "0"));
            Obj_DropDownList.SelectedValue = "0";
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }
    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Mensaje_Error
    ///DESCRIPCION : Muestra el error
    ///PARAMETROS  : P_Texto: texto de un TextBox
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 04-Septiembre-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Mensaje_Error(String P_Mensaje)
    {
        Img_Error.Visible = true;        
        Lbl_Mensaje_Error.Text += P_Mensaje + "</br>";
    }
    private void Mensaje_Error()
    {
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Lbl_Ecabezado_Mensaje.Text = "";
    }
    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Mensaje_Error_Modal
    ///DESCRIPCION : Muestra el error
    ///PARAMETROS  : P_Texto: texto de un TextBox
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 04-Septiembre-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Mensaje_Error_Modal(String P_Mensaje)
    {
        Img_Error_Bus_Pro.Visible = true;
        Lbl_Error_Bus_Pro.Text += P_Mensaje + "</br>";
    }
    private void Mensaje_Error_Modal()
    {
        Img_Error_Bus_Pro.Visible = false;
        Lbl_Error_Bus_Pro.Text = "";        
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Estado_Botones
    ///DESCRIPCIÓN: Metodo para establecer el estado de los botones y componentes del formulario
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/02/2011 05:49:53 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Estado_Botones(int P_Estado)
    {
        Boolean Estado = false;
        switch (P_Estado)
        {
            case 0: //Estado inicial  

                Tab_Contenedor_Parametros.ActiveTabIndex = 0;
                //Txt_Anio.Text = String.Empty;
                Txt_Cuota.Text = String.Empty;
                //Se comentaron lineas por que se quito pestaña de Respaldo en Ley
                //Txt_Respaldo_Adeudo.Text = String.Empty;
                //Txt_Pagos.Text = String.Empty;
                Txt_Constancia_No_Adeudo.Text = "00001";
                Btn_Modificar.AlternateText = "Modificar";
                Btn_Salir.AlternateText = "Inicio";
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Salir.ToolTip = "Inicio";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";                
                Btn_Salir.Enabled = true;
                Btn_Busqueda_Cosntancias.Enabled = false;
                Cargar_Datos_Parametros();
                //Configuracion_Acceso("Frm_Ope_Pre_Parametros.aspx");
                break;

            case 2: //Modificar

                Btn_Modificar.AlternateText = "Actualizar";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Modificar.ToolTip = "Actualizar";
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Busqueda_Cosntancias.Enabled = true;
                Estado = true;

                break;
        }

        //Txt_Anio.Enabled=Estado;
        Txt_Constancia_No_Adeudo.Enabled = Estado;
        Txt_Cuota.Enabled = Estado;
        //Se comentaron lineas por que se quito pestaña de Respaldo en Ley
        //Txt_Respaldo_Adeudo.Enabled = Estado;
        //Se comentaron lineas por que se quito pestaña de Honorarios
        //Txt_Cobro_Honorarios.Enabled = Estado;
        Txt_Tope_Salario.Enabled = Estado;
        //Txt_Pagos.Enabled = Estado;
    }
    #endregion

    #region Metodos ABC

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Modificar_Parametros
    ///DESCRIPCIÓN: se obtienen los datos para modificar los paramentros
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 06/27/2011 11:12:18 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private void Modificar_Parametros()
    {
        Cls_Ope_Pre_Parametros_Negocio Parametros_Negocio = new Cls_Ope_Pre_Parametros_Negocio();
        try
        {
            if (Validar_Campos())
            {
                //Parametros_Negocio.P_Anio_Vigencia = Txt_Anio.Text.Trim();
                Parametros_Negocio.P_Constancia_No_Adeudo = Txt_Constancia_No_Adeudo.Text.Trim().ToUpper();
                Parametros_Negocio.P_Recargas_Traslado = Txt_Cuota.Text.Trim();
                //Se comentaron lineas por que se quito pestaña de Respaldo en Ley
                //Parametros_Negocio.P_Respaldo_Des_Tras = Txt_Respaldo_Adeudo.Text.Trim();
                //Se comentaron lineas por que se quito pestaña de Honorarios
                //Parametros_Negocio.P_Porcentaje_Cobro = Txt_Cobro_Honorarios.Text.Trim();
                Parametros_Negocio.P_Tope_Salario = Txt_Tope_Salario.Text.Trim();
                //Parametros_Negocio.P_Diferencia_Pago = Txt_Pagos.Text.Trim();
                Parametros_Negocio.Modificar_Parametros();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Parámetros", "alert('La modificacion de los parámetros fue Exitosa');", true);
                Estado_Botones(Const_Estado_Inicial);

            }

        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA CLASE: Cargar_Datos_Parametros
    ///DESCRIPCIÓN: consulta y muestra los datos, instancia la clase de negocios para acceder a la consulta
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 06/24/2011 06:24:15 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private void Cargar_Datos_Parametros()
    {
        Cls_Ope_Pre_Parametros_Negocio Parametros_Negocio = new Cls_Ope_Pre_Parametros_Negocio();
        DataTable Resultado_Consulta = new DataTable();

        try
        {
            Resultado_Consulta = Parametros_Negocio.Consultar_Parametros();
            if (Resultado_Consulta.Rows.Count > 0)
            {
                //Txt_Anio.Text = Resultado_Consulta.Rows[0][Ope_Pre_Parametros.Campo_Anio_Vigencia].ToString();
                Txt_Constancia_No_Adeudo.Text = Resultado_Consulta.Rows[0][Ope_Pre_Parametros.Campo_Constancia_No_Adeudo].ToString();
                Txt_Cuota.Text = Resultado_Consulta.Rows[0][Ope_Pre_Parametros.Campo_Recargas_Traslado].ToString();
                //Se comentaron lineas por que se quito pestaña de Respaldo en Ley
                //Txt_Respaldo_Adeudo.Text = Resultado_Consulta.Rows[0][Ope_Pre_Parametros.Campo_Respaldo_Descuento_Tras].ToString();
                //Se comentaron lineas por que se quito pestaña de Honorarios
                //Txt_Cobro_Honorarios.Text = Resultado_Consulta.Rows[0][Ope_Pre_Parametros.Campo_Porcentaje_Cobro_Honorarios].ToString();
                Txt_Tope_Salario.Text = Resultado_Consulta.Rows[0][Ope_Pre_Parametros.Campo_Tope_Salario].ToString();
                //Txt_Pagos.Text = Resultado_Consulta.Rows[0][Ope_Pre_Parametros.Campo_Diferencia_Adeudo].ToString();
            }
            else
            {
                Mensaje_Error("No se encontradon datos");
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Grid_Constancias
    ///DESCRIPCIÓN: Cargar el grid de las constancias
    ///PARAMETROS: 
    ///CREO: jesus toledo
    ///FECHA_CREO: 28/Junio/2011 11:19:31 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private void Cargar_Grid_Constancias(int Page_Index)
    {
        try
        {
            Cls_Ope_Pre_Parametros_Negocio Parametros_Negocio = new Cls_Ope_Pre_Parametros_Negocio();
            //Parametros_Negocio.P_Constancia_ID = Txt_B_Constancia_ID.Text.Trim();
            Parametros_Negocio.P_Constancia_Nombre = Txt_B_Nombre.Text.Trim().ToUpper();
            //Grid_Cosntancias_Busqueda.SelectedRow.Cells[0].Visible = true;
            Dt_Constancias = Parametros_Negocio.Consulta_Constancias();
            Grid_Cosntancias_Busqueda.PageIndex = Page_Index;
            Grid_Cosntancias_Busqueda.DataSource = Dt_Constancias;
            Grid_Cosntancias_Busqueda.DataBind();
            //rid_Cosntancias_Busqueda.SelectedRow.Cells[0].Visible = false ;
        }
        catch (Exception Ex)
        {
            if (Ex.Message.Contains("A field or property with the name "))
            {
                Mensaje_Error_Modal("Ocurrio un error al cargar los datos en el grid");
            }
            else
            {
                Mensaje_Error_Modal(Ex.Message);
            }
            
        }
    } 

    #endregion

    #region Metodos/Validaciones
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Validar_Campos
        ///DESCRIPCIÓN: valdia que se ingresen los campos obligatorios
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 06/27/2011 11:31:53 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        private Boolean Validar_Campos()
        {
            Boolean Resultado = true;            

            //if (Txt_Anio.Text.Trim() == "")
            //{
            //    Lbl_Ecabezado_Mensaje.Text = "Los siguientes datos son necesarios:";
            //    Mensaje_Error("- Año de vigencia");
            //    Resultado = false;
            //}
            if (Txt_Constancia_No_Adeudo.Text.Trim() == "")
            {
                Lbl_Ecabezado_Mensaje.Text = "Los siguientes datos son necesarios:";
                Mensaje_Error("- Identificador de la constancia de no Adeudo");
                Resultado = false;
            }
            if (Txt_Cuota.Text.Trim() == "")
            {
                Lbl_Ecabezado_Mensaje.Text = "Los siguientes datos son necesarios:";
                Mensaje_Error("- Taza de Recargas de Traslado");
                Resultado = false;
            }
            //Se comentaron lineas por que se quito pestaña de Respaldo en Ley
            //if (Txt_Respaldo_Adeudo.Text.Trim() == "")
            //{
            //    Lbl_Ecabezado_Mensaje.Text = "Los siguientes datos son necesarios:";
            //    Mensaje_Error("- Respaldo de ley para descuento de Traslado");
            //    Resultado = false;
            //}
            //if (Txt_Pagos.Text.Trim() == "")
            //{
            //    Lbl_Ecabezado_Mensaje.Text = "Los siguientes datos son necesarios:";
            //    Mensaje_Error("- Tolerancia de Pagos de otras instituciones");
            //    Resultado = false;
            //}
            //Se comentaron lineas por que se quito pestaña de Respaldo en Ley
            //if (Txt_Respaldo_Adeudo.Text.Trim().Length > 250)
            //{
            //    Mensaje_Error("- La longitud del Respaldo de Ley debe ser menos a 250 caracteres");
            //    Txt_Respaldo_Adeudo.Text = Txt_Respaldo_Adeudo.Text.Trim().Substring(0, 250);
            //    Resultado = false;
            //}
            return Resultado;
        }
    #endregion

#endregion

    #region Eventos/Botones
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: se obtienen los datos para modificar los paramentros
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 06/27/2011 11:10:44 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

            if (Btn_Modificar.AlternateText == "Modificar")
            {
                Estado_Botones(Const_Estado_Modificar);
            }
            else if (Btn_Modificar.AlternateText == "Actualizar")
            {
                Modificar_Parametros();
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message.ToString());
        }
    }    
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Salir/Cancelar
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 10/Mayo/2011 12:55:33 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************   
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.AlternateText.Equals("Inicio"))
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Estado_Botones(Const_Estado_Inicial);
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Cosntancias_Click1
    ///DESCRIPCIÓN: despliega la ventana modal para la busqueda de la constancia de no adedudo
    ///PARAMETROS: 
    ///CREO: jesus toledo
    ///FECHA_CREO: 27/Junio/2011 01:36:59 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Busqueda_Cosntancias_Click1(object sender, ImageClickEventArgs e)
    {
        Modal_Busqueda_Constancias.Show();
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Aceptar_Busqueda_Av_Click
    ///DESCRIPCIÓN: Evento para buscar la constancia
    ///PARAMETROS: 
    ///CREO: jesus toledo
    ///FECHA_CREO: 27/junio/2011 01:38:02 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Aceptar_Busqueda_Av_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Cargar_Grid_Constancias(0);
            
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Constancias_Busqueda_SelectedIndexChanged
    ///DESCRIPCIÓN: Ingresa el ID de la Constancia Seleccionada en la caja de Texto
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 26/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Constancias_Busqueda_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Cosntancias_Busqueda.SelectedIndex > (-1))
            {
                //Grid_Cosntancias_Busqueda.SelectedRow.Cells[0].Visible = true;
                Txt_Constancia_No_Adeudo.Text = "";
                Txt_Constancia_No_Adeudo.Text = Grid_Cosntancias_Busqueda.SelectedRow.Cells[1].Text;
                Modal_Busqueda_Constancias.Hide();
                Upd_Parametros_Predial.Update();
                //Grid_Cosntancias_Busqueda.SelectedRow.Cells[0].Visible = false;
            }
    }
       
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
            Botones.Add(Btn_Modificar);            
            

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
