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
using Presidencia.Operacion_Com_Parametros_Directores.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;

public partial class paginas_Compras_Frm_Cat_Com_Paramatros_directores : System.Web.UI.Page
{
    #region Variables
    private const int Const_Estado_Inicial = 0;
    private const int Const_Estado_Nuevo = 1;    
    private const int Const_Estado_Modificar = 3;

    private static DataTable Dt_Parametros = new DataTable();

    private static string M_Busqueda = "";

    #endregion

    #region Page Load / Init

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!Page.IsPostBack)
            {
                Estado_Botones(Const_Estado_Inicial);
                Cargar_Datos();
            }
            Mensaje_Error();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
            Estado_Botones(Const_Estado_Inicial);
        }
    }

    #endregion

    #region Metodos

    #region Metodos ABC
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Modificar_Parametros
    ///DESCRIPCIÓN: Se modifican los parametros SAP en la base de datos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 20/Abril/2011 01:57:55 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private void Modificar_Parametros()
    {
        try
        {

            Cls_Ope_Com_Directores_Parametros_Negocio Parametros_Negocio = new Cls_Ope_Com_Directores_Parametros_Negocio();
            if (Validar_Campos())
            {
                Parametros_Negocio.P_Tesorero = Txt_Tesorero.Text.Trim();
                Parametros_Negocio.P_Oficial_Mayor = Txt_Oficialia.Text.Trim();
                Parametros_Negocio.P_Director_Adquisiciones = Txt_Adquisicion.Text.Trim();
                Parametros_Negocio.Modificar_Parametros();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Parametros SAP", "alert('La Modificación de los parametros fue Exitosa');", true);
                Estado_Botones(Const_Estado_Inicial);
                Cargar_Datos();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Modificar Parametros: " + Ex.Message);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos
    ///DESCRIPCIÓN: consulta los parametros SAP y los asigna a los controles
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 04/20/2011 04:37:26 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Datos()
    {
        try
        {

            Cls_Ope_Com_Directores_Parametros_Negocio Parametros_Negocio = new Cls_Ope_Com_Directores_Parametros_Negocio();
            Dt_Parametros = Parametros_Negocio.Consultar_Parametros();
            if (Dt_Parametros.Rows.Count > 0)
            {
                foreach (DataRow Dt in Dt_Parametros.Rows)
                {
                    Txt_Adquisicion.Text = Dt[Cat_Com_Directores.Campo_Director_Adquisiciones].ToString();
                    Txt_Oficialia.Text = Dt[Cat_Com_Directores.Campo_Oficial_Mayor].ToString();
                    Txt_Tesorero.Text = Dt[Cat_Com_Directores.Campo_Tesorero].ToString();
                }
            }

        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);

        }

    }

    #endregion

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
        Lbl_Error.Text += P_Mensaje + "</br>";
    }
    private void Mensaje_Error()
    {
        Img_Error.Visible = false;
        Lbl_Error.Text = "";
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
        Boolean Habilitado = false;
        switch (P_Estado)
        {
            case 0: //Estado inicial
                Habilitado = false;


                Btn_Modificar.AlternateText = "Modificar";
                Btn_Salir.AlternateText = "Inicio";

                Btn_Modificar.ToolTip = "Modificar";
                Btn_Salir.ToolTip = "Inicio";

                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                
                break;

            case 3: //Modificar
                Habilitado = true;
                Btn_Modificar.Visible = true;

                Btn_Modificar.AlternateText = "Actualizar";
                Btn_Salir.AlternateText = "Cancelar";

                Btn_Modificar.ToolTip = "Actualizar";
                Btn_Salir.ToolTip = "Cancelar";

                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";

                break;
        }
        Txt_Oficialia.Enabled = Habilitado;
        Txt_Tesorero.Enabled = Habilitado;
        Txt_Adquisicion.Enabled = Habilitado;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Validar_Campos
    ///DESCRIPCIÓN: revisa que los controles hallan sido llenados con los datos necesarios
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 04/20/2011 04:50:54 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Boolean Validar_Campos()
    {
        Boolean Resultado = true;

        if (Txt_Oficialia.Text.Trim() == String.Empty || Txt_Oficialia.Text.Trim() == "" || Txt_Oficialia.Text.Trim() == null)
        {
            Mensaje_Error("El campo Oficilia es necesario");
            Resultado = false;
        } if (Txt_Tesorero.Text.Trim() == String.Empty || Txt_Tesorero.Text.Trim() == "" || Txt_Tesorero.Text.Trim() == null)
        {
            Mensaje_Error("El campo Tesorero es necesario");
            Resultado = false;
        }
        if (Txt_Adquisicion.Text.Trim() == String.Empty || Txt_Adquisicion.Text.Trim() == "" || Txt_Adquisicion.Text.Trim() == null)
        {
            Mensaje_Error("El campo Adquisicion es necesario");
            Resultado = false;
        }
        return Resultado;
    }
    #endregion



    #endregion
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Evento para modificar los parametros SAP en la base de datos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 20/Abril/2011 01:59:32 p.m.
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
    ///DESCRIPCIÓN: Evento para salir o cancelar la accion
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 20/Abril/2011 02:00:04 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText == "Inicio")
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");

        }
        else
        {
            Estado_Botones(Const_Estado_Inicial);
        }
    }
}

