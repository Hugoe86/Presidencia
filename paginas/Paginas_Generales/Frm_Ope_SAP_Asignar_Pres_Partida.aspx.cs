using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Operacion_SAP_Pres_Partidas.Negocio;
using Presidencia.Catalogo_Compras_Proyectos_Programas.Negocio;
using System.Data;

public partial class paginas_Paginas_Generales_Frm_Ope_SAP_Asignar_Pres_Partida : System.Web.UI.Page
{
    #region Variables
    private const int Const_Estado_Inicial = 0;
    private const int Const_Estado_Nuevo = 1;
    private const int Const_Estado_Modificar = 2;
    private const int Const_Estado_Buscar = 3;
    private const int Const_Estado_Deshabilitado = 4;
    private const int Const_Estado_Partidas = 5;
    private const int Const_Estado_Grid_Proy_Seleccionado = 6;
    private const int Const_Alta = 0;
    private const int Const_Modificar = 1;
    private static double Presupuesto_Anterior;
    private static DataTable Dt_Proyectos_Programas = new DataTable();    
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
                Cargar_Combos();
                Cargar_Grid(0);
            }
            Mensaje_Error();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
            Estado_Botones(Const_Estado_Deshabilitado);

        }
    }
    #endregion

    #region Metodos
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Combos
    ///DESCRIPCIÓN: metodo usado para cargar la informacion de todos los combos del formulario con la respectiva consulta
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/03/2011 08:46:12 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Combos()
    {
        try
        {
            Cls_Cat_Com_Proyectos_Programas_Negocio Proyectos_Programas = new Cls_Cat_Com_Proyectos_Programas_Negocio();
            Llenar_Combo_ID(Cmb_Proyectos, Proyectos_Programas.Consulta_Programas_Proyectos(), Cat_Com_Proyectos_Programas.Campo_Nombre, Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID, "0");            
            Cmb_Partidas.Items.Clear();
        }
        catch (Exception Ex)
        {
            throw new Exception("No se pudieron cargar los datos necesarios" + "</br>" + Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Grid
    ///DESCRIPCIÓN: Realizar la consulta y llenar el grido con estos datos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/03/2011 12:14:35 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Grid(int Page_Index)
    {
        try
        {
            Cls_Ope_SAP_Pres_Partidas_Negocio Proyectos_Programas_Partidas = new Cls_Ope_SAP_Pres_Partidas_Negocio();
            Proyectos_Programas_Partidas.P_Anio_Presupuesto = M_Busqueda;
            Dt_Proyectos_Programas = Proyectos_Programas_Partidas.Consulta_Asignacion_Presupuesto();
            Grid_Proyectos_Programas.PageIndex = Page_Index;
            Grid_Proyectos_Programas.DataSource = Dt_Proyectos_Programas;
            Grid_Proyectos_Programas.DataBind();
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
            Obj_DropDownList.Items.Add(new ListItem("<SELECCIONE>", "0"));
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
            Obj_DropDownList.Items.Add(new ListItem("<SELECCIONE>", "0"));
            Obj_DropDownList.SelectedValue = "0";
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos
    ///DESCRIPCIÓN: metodo que recibe el DataRow seleccionado de la grilla y carga los datos en los componetes de l formulario
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/03/2011 02:07:54 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Datos(DataRow Dr_Proyectos_Programas)
    {
        try
        {
            Txt_Monto_Comprometido.Text = Dr_Proyectos_Programas[Ope_Com_Pres_Partida.Campo_Monto_Comprometido].ToString();
            Txt_Monto_Disponible.Text = Dr_Proyectos_Programas[Ope_Com_Pres_Partida.Campo_Monto_Disponible].ToString();
            Txt_Monto_Ejercido.Text = Dr_Proyectos_Programas[Ope_Com_Pres_Partida.Campo_Monto_Ejercido].ToString();
            Txt_Presupuesto.Text = Dr_Proyectos_Programas[Ope_Com_Pres_Partida.Campo_Monto_Presupuestal].ToString();
            Txt_Anio.Text = Dr_Proyectos_Programas[Ope_Com_Pres_Partida.Campo_Anio_Presupuesto].ToString();
            Cmb_Proyectos.SelectedValue = Dr_Proyectos_Programas[Ope_Com_Pres_Partida.Campo_Proyecto_Programa_ID].ToString();
            Cls_Ope_SAP_Pres_Partidas_Negocio Partidas_Proyectos = new Cls_Ope_SAP_Pres_Partidas_Negocio();
            Partidas_Proyectos.P_Proyecto_Programa_ID = Cmb_Proyectos.SelectedValue;
            Llenar_Combo_ID(Cmb_Partidas, Partidas_Proyectos.Consulta_Partidas_Proyectos(), Cat_Sap_Partidas_Especificas.Campo_Nombre, Cat_Sap_Partidas_Especificas.Campo_Partida_ID, "0");
            Cmb_Partidas.SelectedValue = Dr_Proyectos_Programas[Ope_Com_Pres_Partida.Campo_Partida_ID].ToString();            
            
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
        switch (P_Estado)
        {
            case 0: //Estado inicial

                Mensaje_Error();
                Txt_Busqueda.Text = String.Empty;
                Txt_Presupuesto.Text = String.Empty;
                Txt_Monto_Comprometido.Text = String.Empty;
                Txt_Monto_Disponible.Text = String.Empty;
                Txt_Monto_Ejercido.Text = String.Empty;
                Txt_Anio.Text = String.Empty;
                Txt_Presupuesto.Enabled = false;
                Txt_Monto_Comprometido.Enabled = false;
                Txt_Monto_Disponible.Enabled = false;
                Txt_Monto_Ejercido.Enabled = false;
                Txt_Anio.Enabled = false;
                Cmb_Proyectos.SelectedIndex = 0;
                Cmb_Proyectos.Enabled = false;

                Cmb_Partidas.SelectedIndex = 0;
                Cmb_Partidas.Enabled = false;

                Grid_Proyectos_Programas.Enabled = true;
                Grid_Proyectos_Programas.SelectedIndex = (-1);

                Btn_Busqueda.Enabled = true;                
                Btn_Modificar.Enabled = true;
                Btn_Nuevo.Enabled = true;
                Btn_Salir.Enabled = true;
                Btn_Busqueda.AlternateText = "Buscar";
                
                Btn_Modificar.AlternateText = "Modificar";
                Btn_Nuevo.AlternateText = "Nuevo";
                Btn_Salir.AlternateText = "Salir";

                Btn_Busqueda.ToolTip = "Consultar";                
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Salir.ToolTip = "Inicio";

                Btn_Busqueda.ImageUrl = "~/paginas/imagenes/paginas/busqueda.png";                
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                Configuracion_Acceso("Frm_Ope_SAP_Asignar_Pres_Partida.aspx");
                break;

            case 1: //Nuevo
                Mensaje_Error();

                Txt_Presupuesto.Text = String.Empty;
                Txt_Monto_Comprometido.Text = "0";
                Txt_Monto_Disponible.Text = String.Empty;
                Txt_Monto_Ejercido.Text = "0";
                Txt_Anio.Text = String.Empty;
                Cmb_Proyectos.SelectedIndex = 0;
                Llenar_Combo_ID(Cmb_Partidas);

                Cmb_Proyectos.Enabled = true;
                Cmb_Partidas.Enabled = true;

                Txt_Presupuesto.Enabled = true;                
                Txt_Anio.Enabled = true;                
                Btn_Modificar.Enabled = false;
                Btn_Nuevo.Enabled = true;
                Btn_Salir.Enabled = true;

                Grid_Proyectos_Programas.SelectedIndex = (-1);
                Grid_Proyectos_Programas.Enabled = false;                
                
                Btn_Modificar.AlternateText = "Modificar";
                Btn_Nuevo.AlternateText = "Guardar";
                Btn_Salir.AlternateText = "Cancelar";
                
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Nuevo.ToolTip = "Guardar";
                Btn_Salir.ToolTip = "Cancelar";
                
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar_deshabilitado.png";

                break;

            case 2: //Modificar
                Mensaje_Error();

                Txt_Presupuesto.Enabled = true;
                
                Btn_Modificar.Enabled = true;
                Btn_Nuevo.Enabled = false;
                Btn_Salir.Enabled = true;
                
                Btn_Modificar.AlternateText = "Guardar";
                Btn_Nuevo.AlternateText = "Nuevo";
                Btn_Salir.AlternateText = "Cancelar";
                
                Btn_Modificar.ToolTip = "Guardar";
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Salir.ToolTip = "Cancelar";

                Grid_Proyectos_Programas.Enabled = false;
                
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo_deshabilitado.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";

                break;

            case 3: //Buscar
                Mensaje_Error();

                Txt_Presupuesto.Text = String.Empty;
                Txt_Monto_Comprometido.Text = String.Empty;
                Txt_Monto_Disponible.Text = String.Empty;
                Txt_Monto_Ejercido.Text = String.Empty;
                Txt_Anio.Text = String.Empty;
                break;

            case 4: //Desabilitar

                Txt_Busqueda.Text = String.Empty;
                Txt_Presupuesto.Text = String.Empty;
                Txt_Monto_Comprometido.Text = String.Empty;
                Txt_Monto_Disponible.Text = String.Empty;
                Txt_Monto_Ejercido.Text = String.Empty;
                Txt_Anio.Text = String.Empty;
                Txt_Presupuesto.Enabled = false;
                Txt_Monto_Comprometido.Enabled = false;
                Txt_Monto_Disponible.Enabled = false;
                Txt_Monto_Ejercido.Enabled = false;
                Txt_Anio.Enabled = false;
                Cmb_Proyectos.Enabled = false;
                Cmb_Partidas.Enabled = false;

                Grid_Proyectos_Programas.Enabled = true;
                Grid_Proyectos_Programas.SelectedIndex = (-1);                

                Btn_Busqueda.Enabled = false;                
                Btn_Modificar.Enabled = false;
                Btn_Nuevo.Enabled = false;
                Btn_Salir.Enabled = false;
                Btn_Busqueda.AlternateText = "Buscar";                
                Btn_Modificar.AlternateText = "Modificar";
                Btn_Nuevo.AlternateText = "Nuevo";
                Btn_Salir.AlternateText = "Salir";

                Btn_Busqueda.ToolTip = "Buscar";                
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Salir.ToolTip = "Inicio";

                Btn_Busqueda.ImageUrl = "~/paginas/imagenes/paginas/busqueda.png";                
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                break;

            case 5: //Partidas
                Mensaje_Error();

                Cmb_Partidas.SelectedIndex = 0;
                Cmb_Proyectos.SelectedIndex = 0;
                Llenar_Combo_ID(Cmb_Proyectos);
                Llenar_Combo_ID(Cmb_Proyectos);                
                Grid_Proyectos_Programas.SelectedIndex = (-1);

                break;            
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Validar_Datos
    ///DESCRIPCIÓN: Guardar datos para dar de alta un nuevo registro de un servicio
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/03/2011 10:45:17 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Boolean Validar_Datos()
    {
        Boolean Resultado = true;
        try
        {
            if (Txt_Presupuesto.Text.Trim() == "")
            {
                Resultado = false;
                Mensaje_Error("Favor de ingresar el presupuesto");
            }
            if (Txt_Anio.Text.Trim() == "")
            {
                Resultado = false;
                Mensaje_Error("Favor de ingresar el año del presupuesto");
            }
            if (Txt_Monto_Comprometido.Text.Trim() == "")
            {
                Resultado = false;
                Mensaje_Error("Favor de ingresar el Monto Comprometido");
            }
            if (Txt_Monto_Disponible.Text.Trim() == "")
            {
                Resultado = false;
                Mensaje_Error("Favor de ingresar el Monto Disponible");
            }
            if (Txt_Monto_Ejercido.Text.Trim() == "")
            {
                Resultado = false;
                Mensaje_Error("Favor de ingresar el Monto Ejercido");
            }
            if (Cmb_Proyectos.SelectedIndex <= 0)
            {
                Resultado = false;
                Mensaje_Error("Favor de especificar el Proyecto");
            }
            if (Cmb_Partidas.SelectedIndex <= 0)
            {
                Resultado = false;
                Mensaje_Error("Favor de especificar la Partida");
            }            
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
        return Resultado;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Alta
    ///DESCRIPCIÓN: Realiza el alta de un nuevo registro
    ///PARAMETROS: 
    ///CREO: jesus toledo
    ///FECHA_CREO: 05/03/2011 12:29:45
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Alta(int Opcion)
    {        
        try
        {
            if (Validar_Datos())
            {
                Cls_Ope_SAP_Pres_Partidas_Negocio Proyecto_Programa = new Cls_Ope_SAP_Pres_Partidas_Negocio();

                Proyecto_Programa.P_Proyecto_Programa_ID = Cmb_Proyectos.SelectedValue;
                Proyecto_Programa.P_Partida_Especifica_ID = Cmb_Partidas.SelectedValue;
                Proyecto_Programa.P_Monto_Presupuestal = Txt_Presupuesto.Text.Trim();
                Proyecto_Programa.P_Monto_Comprometido = Txt_Monto_Comprometido.Text.Trim();
                Proyecto_Programa.P_Monto_Disponible = Txt_Monto_Disponible.Text.Trim();
                Proyecto_Programa.P_Monto_Ejercido = Txt_Monto_Ejercido.Text.Trim();
                Proyecto_Programa.P_Anio_Presupuesto = Txt_Anio.Text.Trim();
                Proyecto_Programa.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                if( Opcion == 0 )//Alta
                {
                Proyecto_Programa.Alta_Pres_Partida();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Asignacion de Presupuesto", "alert('La asignacion de Presupuesto fue Exitosa');", true);
                }
                else if (Opcion == 1)//Modificar
                {
                    Proyecto_Programa.P_Pres_Partida_ID = Dt_Proyectos_Programas.Rows[Grid_Proyectos_Programas.SelectedIndex + (Grid_Proyectos_Programas.PageIndex * 5)][0].ToString();
                    Proyecto_Programa.P_Monto_Presupuestal = (Presupuesto_Anterior - Convert.ToDouble(Proyecto_Programa.P_Monto_Presupuestal)).ToString();
                    Proyecto_Programa.Cambio_Pres_Partida();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Proyectos y Programas", "alert('La modificación del Registro Exitosa');", true);                    
                }

                Estado_Botones(Const_Estado_Inicial);
                Txt_Busqueda.Text = "";
                Cargar_Grid(0);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message, Ex);
        }
    } 
    
    #endregion

    #region Eventos
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
            {
                Estado_Botones(Const_Estado_Nuevo);
            }
            else
            {
                Alta(Const_Alta);
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Grid_Proyectos_Programas.SelectedIndex > (-1))
            {
                
                if (Btn_Modificar.AlternateText.Equals("Modificar"))
                {
                    Estado_Botones(Const_Estado_Modificar);
                    Presupuesto_Anterior = Convert.ToDouble(Txt_Presupuesto.Text.Trim());
                }
                else
                {                    
                    Alta(Const_Modificar);
                }
            }
            else
            {
                Mensaje_Error("Favor de seleccionar el registro a modificar");
            }

        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);

        }
    }    
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.AlternateText.Equals("Salir"))
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else if (Btn_Salir.AlternateText.Equals("Listado"))
            {
                Estado_Botones(Const_Estado_Inicial);
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
    protected void Btn_Busqueda_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Estado_Botones(Const_Estado_Buscar);
            Grid_Proyectos_Programas.SelectedIndex = (-1);
            M_Busqueda = Txt_Busqueda.Text.Trim();
            Cargar_Grid(0);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    protected void Cmb_Proyectos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
        Cls_Ope_SAP_Pres_Partidas_Negocio Partidas_Proyectos = new Cls_Ope_SAP_Pres_Partidas_Negocio();
        Partidas_Proyectos.P_Proyecto_Programa_ID = Cmb_Proyectos.SelectedValue;
        Llenar_Combo_ID(Cmb_Partidas, Partidas_Proyectos.Consulta_Partidas_Proyectos(), Cat_Sap_Partidas_Especificas.Campo_Nombre, Cat_Sap_Partidas_Especificas.Campo_Partida_ID, "0");
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    protected void Txt_Presupuesto_TextChanged(object sender, EventArgs e)
    {
        Txt_Monto_Disponible.Text = Txt_Presupuesto.Text.Trim();
    }
    
    #endregion

    #region Eventos Grid
    protected void Grid_Proyectos_Programas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Proyectos_Programas.SelectedIndex = (-1);
            Cargar_Grid(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    protected void Grid_Proyectos_Programas_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Proyectos_Programas.SelectedIndex > (-1))
            {
                Cargar_Datos(Dt_Proyectos_Programas.Rows[Grid_Proyectos_Programas.SelectedIndex + (Grid_Proyectos_Programas.PageIndex * 5)]);                
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
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
            Botones.Add(Btn_Nuevo);
            Botones.Add(Btn_Modificar);
            Botones.Add(Btn_Busqueda);

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