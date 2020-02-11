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
using Presidencia.Constantes;
using Presidencia.Registro_Peticion.Negocios ;
using Presidencia.Sessiones;
using Presidencia.Registro_Peticion.Datos;
using Presidencia.Dependencias.Negocios;
using Presidencia.Asuntos_AC.Negocio;
using Presidencia.Areas.Negocios;

public partial class paginas_Atencion_Ciudadana_Frm_Ope_Atención_Solicitudes : System.Web.UI.Page
{

#region Variables

    static Cls_Cat_Ate_Peticiones_Negocio M_Peticion; // Variable global que definira las opciones de busqueda y consula de peticiones
    private const int Const_Estado_Inicial = 0;
    private const int Const_Estado_Solucion = 1;
    private const int Const_Rol_Administrador = 2;
    private const int Const_Rol_Jefe_Dependencia = 3;
    private const int Const_Rol_Jefe_Area = 4;
    private const int Const_Rol_Administrador_Modulo = 5;

#endregion

#region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            
            if ( Cls_Sessiones.Empleado_ID.Equals(null)  )            
            {
                Response.Redirect("../Paginas_Generales/Frm_Login.aspx");
            }            
            if( !Page.IsPostBack)
            {
                Estado_Botones(Const_Estado_Inicial);
                Cargar_Combos();
                Cargar_Bandeja();

            }            
        }
        catch(Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

#endregion    

#region Métodos
    
    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Cargar_Rol
    ///DESCRIPCION : habilita los controles segun el rol
    ///PARAMETROS  : 
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 27-Mayo-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Cargar_Rol()
    {
        String Grupo_Rol = Cls_Cat_Ate_Peticiones_Datos.Consultar_Grupo_Rol(Cls_Sessiones.Rol_ID);

        switch (Grupo_Rol)
        {
            case "00001"://Administrador total del sistema // Cargar peticiones sin asignar
                Estado_Botones(Const_Rol_Administrador);
                break;
            case "00002"://Administrador de modulo // Cargar peticiones sin asignar
                Estado_Botones(Const_Rol_Administrador_Modulo);
                break;
            case "00003"://Jefe de dependencia
                Estado_Botones(Const_Rol_Jefe_Dependencia);
                break;
            case "00004"://Jefe de area
                Estado_Botones(Const_Rol_Jefe_Area);
                break;
            default:
                Mensaje_Error("No Tiene suficientes privilegios para modificar esta información");
                break;
        }
    }
    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Estado_Botones
    ///DESCRIPCION : determina el estado de los controles del formulario
    ///PARAMETROS  : 
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 31-Mayo-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Estado_Botones(int P_Estado)
    {
        switch (P_Estado)
        {
            case 0:
                Btn_Bandeja.Visible = false;
                Img_Warning.Visible = false;
                Txt_Fecha_Inicio.Enabled = false;
                Txt_Fecha_Termino.Enabled = false;
                Txt_Folio.Enabled = false;
                Cmb_Area.Enabled = false;
                Cmb_Estatus.Enabled = false;
                Cmb_Dependencia.Enabled = false;
                break;

            case 1:                
                    //nada
                break;
            case 2: //Adiministrador
                Btn_Bandeja.Visible = false;
                Txt_Fecha_Inicio.Enabled = true;
                Txt_Fecha_Termino.Enabled = true;
                Txt_Folio.Enabled = true;
                Cmb_Area.Enabled = true;
                Cmb_Estatus.Enabled = true;
                Cmb_Dependencia.Enabled = true;
                M_Peticion.P_Dependencia_ID = null;
                M_Peticion.P_Area_ID = null;
                M_Peticion.P_Estatus = null;
                break;
            case 3: //Jefe Dependencia
                Btn_Bandeja.Visible = false;
                Cmb_Dependencia.SelectedValue = Cls_Cat_Ate_Peticiones_Datos.Consultar_Dependencia_ID(Cls_Sessiones.Empleado_ID);
                Cmb_Dependencia.Enabled = false;
                Cmb_Area.Enabled = true;
                Txt_Fecha_Inicio.Enabled = true;
                Txt_Fecha_Termino.Enabled = true;
                Txt_Folio.Enabled = true;
                Cmb_Estatus.Enabled = true;
                M_Peticion.P_Dependencia_ID = Cls_Cat_Ate_Peticiones_Datos.Consultar_Dependencia_ID(Cls_Sessiones.Empleado_ID);
                M_Peticion.P_Area_ID = null;
                M_Peticion.P_Estatus = "Pendiente y Proceso";
                cargar_Combo_Area();
                Grid_Peticiones.DataSource = M_Peticion.Consulta_Peticion_Bandeja();
                Grid_Peticiones.DataBind();
                break;
            case 4: //jefe Area
                Btn_Bandeja.Visible = false;
                Cmb_Dependencia.SelectedValue = Cls_Cat_Ate_Peticiones_Datos.Consultar_Dependencia_ID(Cls_Sessiones.Empleado_ID);
                cargar_Combo_Area();
                Cmb_Area.SelectedValue = Cls_Cat_Ate_Peticiones_Datos.Consultar_Area_ID(Cls_Sessiones.Empleado_ID);
                Txt_Fecha_Inicio.Enabled = true;
                Txt_Fecha_Termino.Enabled = true;
                Txt_Folio.Enabled = true;
                Cmb_Area.Enabled = false;
                Cmb_Estatus.Enabled = true;
                Cmb_Dependencia.Enabled = false;
                M_Peticion.P_Dependencia_ID = Cls_Cat_Ate_Peticiones_Datos.Consultar_Dependencia_ID(Cls_Sessiones.Empleado_ID);
                M_Peticion.P_Area_ID = Cls_Cat_Ate_Peticiones_Datos.Consultar_Area_ID(Cls_Sessiones.Empleado_ID);
                M_Peticion.P_Estatus = "Pendiente y Proceso";
                Grid_Peticiones.DataSource = M_Peticion.Consulta_Peticion_Bandeja();
                Grid_Peticiones.DataBind();
                break;
            case 5: //Administrador modulo                
                Btn_Bandeja.Visible = true;
                Txt_Fecha_Inicio.Enabled = true;
                Txt_Fecha_Termino.Enabled = true;
                Txt_Folio.Enabled = true;
                Cmb_Area.Enabled = true;
                Cmb_Estatus.Enabled = true;
                Cmb_Dependencia.Enabled = true;
                M_Peticion.P_Dependencia_ID = null;
                M_Peticion.P_Area_ID = null;
                M_Peticion.P_Estatus = null;
                Grid_Peticiones.DataSource = M_Peticion.Consulta_Peticion_Bandeja_No_Asignados();
                Grid_Peticiones.DataBind();
            break;
        }
    }

    private void cargar_Combo_Area()
    {
        //Carga las areas por cada dependencia        
        Cls_Cat_Areas_Negocio Areas_Negocio = new Cls_Cat_Areas_Negocio();
        Areas_Negocio.P_Dependencia_ID = Cmb_Dependencia.SelectedValue;
        Llenar_Combo_ID(Cmb_Area, Areas_Negocio.Consulta_Areas(), Cat_Areas.Campo_Nombre, Cat_Areas.Campo_Area_ID, "0");
    }    
    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Cargar_Bandeja
    ///DESCRIPCION : Consulta las peticiones generadas por el ciudadano con el Id del usuario logeado
    ///PARAMETROS  : 
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 27-Mayo-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Cargar_Bandeja()
    {
        try
        {
            //Hace la instancia de la clase Cls_Cat_Ate_Peticiones_Negocio
            M_Peticion = new Cls_Cat_Ate_Peticiones_Negocio();
            Cargar_Rol();
            //M_Peticion.
                
            Grid_Peticiones.PageIndex = 0;
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }    
    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Busqueda
    ///DESCRIPCION : Busca dependencias filtrada por folio o por dependencia y area
    ///PARAMETROS  : 
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 25-Mayo-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Busqueda()
    {
        try
        {
            M_Peticion = new Cls_Cat_Ate_Peticiones_Negocio();
            if (Txt_Folio.Text.Trim() != "")
            {
                M_Peticion.P_Folio = FormatearTexto(Txt_Folio.Text);
            }
            else
            {
                M_Peticion.P_Folio = null;
            }
            if (Cmb_Dependencia.SelectedIndex > 0)
            {
                M_Peticion.P_Dependencia_ID = Cmb_Dependencia.SelectedValue.ToString();
            }            
            if (Cmb_Estatus.SelectedIndex > 0)
            {
                M_Peticion.P_Estatus = Cmb_Estatus.SelectedValue.ToString();
            }
            else
            {
                M_Peticion.P_Estatus = null;
            }
            if (Cmb_Area.SelectedIndex > 0)
            {
                M_Peticion.P_Area_ID = Cmb_Area.SelectedValue.ToString();
            }
            if (Txt_Fecha_Inicio.Text != String.Empty)
            {
                M_Peticion.P_Fecha_Inicio = (Txt_Fecha_Inicio.Text);
            }
            else
            {
                M_Peticion.P_Fecha_Inicio = null;
            }
            if (Txt_Fecha_Termino.Text != String.Empty)
            {
                M_Peticion.P_Fecha_Final = Txt_Fecha_Termino.Text;
            }
            else
            {
                M_Peticion.P_Fecha_Final = null;
            }
            Grid_Peticiones.DataSource = M_Peticion.Consulta_Peticion_Bandeja();
            Grid_Peticiones.DataBind();
            Grid_Peticiones.PageIndex = 0;
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:FormatearTexto
    ///DESCRIPCION : Toma un string para quitarle espacios al final y comillas 
    ///              simples que causan error en el comando SQL
    ///PARAMETROS  : P_Texto: texto de un TextBox
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 03-Septiembre-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private String FormatearTexto(String P_Texto)
    {
        String Texto = P_Texto.Trim();
        return Texto.Replace("'", "");
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
        Img_Warning.Visible = true;
        Lbl_Warning.Text += P_Mensaje + "</br>";        
    }
    private void Mensaje_Error()
    {
        Img_Warning.Visible = false;
        Lbl_Warning.Text = "";
    }
    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Cargar_Combos
    ///DESCRIPCION : Carga la informacion de los combos
    ///PARAMETROS  : Peticion: datos de la clase de negocio de la peticion
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 26-Mayo-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Cargar_Combos()
    {
        try
        {
            //llenar el combo de las dependencias
            Cls_Cat_Dependencias_Negocio Dependendencias_Negocio = new Cls_Cat_Dependencias_Negocio();
            Llenar_Combo_ID(Cmb_Dependencia, Dependendencias_Negocio.Consulta_Dependencias(), Cat_Dependencias.Campo_Nombre, Cat_Dependencias.Campo_Dependencia_ID, "0");            
        }

        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }
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
    public void Llenar_Combo_ID(DropDownList Obj_DropDownList, DataTable Dt_Temporal, String Texto, String Valor, String Seleccion)
    {
        try
        {
            Obj_DropDownList.Items.Clear();
            Obj_DropDownList.Items.Add(new ListItem("<<SELECCIONE>>", "0"));
            foreach (DataRow row in Dt_Temporal.Rows)
            {
                Obj_DropDownList.Items.Add(new ListItem(row[Texto].ToString(), row[Valor].ToString()));
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
            Obj_DropDownList.Items.Add(new ListItem("<<SELECCIONE>>", "0"));
            Obj_DropDownList.SelectedValue = "0";
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
#endregion

#region Grid
    
    protected void Grid_Peticiones_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //Guardar el ID de la peticion seleccionada y
            //Redirije a la la siguiente pagina para dar
            //respuesta a la peticion seleccionada
            GridViewRow Seleccion = Grid_Peticiones.SelectedRow;
            Session["Peticion_Folio"] = Seleccion.Cells[1].Text;
            Response.Redirect("../Atencion_Ciudadana/Frm_Ope_Ate_Respuesta_Solicitudes.aspx");
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
      
    protected void Grid_Peticiones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.Grid_Peticiones.PageIndex = e.NewPageIndex;
        Grid_Peticiones.DataSource = M_Peticion.Consulta_Peticion_Bandeja();
        Grid_Peticiones.DataBind();
    }

#endregion

#region Eventos

    protected void Btn_Buscar_Peticion_Click(object sender, ImageClickEventArgs e)
    {
        //Realiza la busqueda de peticion filtrada por Folio o por Dependencia y Area
        Mensaje_Error();
        Busqueda();
    }

    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
    }

    protected void Btn_Buscar_Peticion_Click2(object sender, ImageClickEventArgs e)
    {
        Mensaje_Error();
        Busqueda();
    }
    protected void Cmb_Dependencia_SelectedIndexChanged(object sender, EventArgs e)
    {
        cargar_Combo_Area();
    }

    protected void Btn_Bandeja_Click(object sender, EventArgs e)
    {
        Cargar_Bandeja();
    }
#endregion
}