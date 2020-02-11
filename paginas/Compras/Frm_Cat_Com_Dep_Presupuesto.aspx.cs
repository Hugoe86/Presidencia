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
using Presidencia.Sessiones;
using Presidencia.Dependencias.Negocios;
using Presidencia.Catalogo_Compras_Presupuesto_Dependencias.Negocio;
using System.Collections.Generic;

public partial class paginas_Compras_Frm_Cat_Com_Dep_Presupuesto : System.Web.UI.Page
{

    #region Variables Globales

    private const int Const_Estado_Inicial = 0;
    private const int Const_Estado_Nuevo = 1;
    private const int Const_Estado_Modificar = 2;
    private const int Const_Estado_Buscar = 3;
    private static DataTable Dt_Presupuestos = new DataTable();
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
        }
    }
    #endregion

    #region Metodos
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Combos
    ///DESCRIPCIÓN: metodo usado para caragar la informacion de todos los combos del formulario con la respectiva consulta
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/03/2011 08:46:12 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Combos()
    {
        //Instancia el objeto de negocio de familias y consulta la lista de estas
        Cls_Cat_Dependencias_Negocio Dependencias_Negocio = new Cls_Cat_Dependencias_Negocio();
        Llenar_Combo_ID(Cmb_Dependencias, Dependencias_Negocio.Consulta_Dependencias(), Cat_Dependencias.Campo_Nombre, Cat_Dependencias.Campo_Dependencia_ID, "0");        

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
            Cls_Cat_Com_Presupuesto_Dependencias_Negocio Presupuesto_Negocio = new Cls_Cat_Com_Presupuesto_Dependencias_Negocio();
            Presupuesto_Negocio.P_Dependencia_ID = M_Busqueda;
            if (Comprobar_Numero(M_Busqueda))
            {
                Presupuesto_Negocio.P_Anio_Presupuesto = Convert.ToInt32(M_Busqueda);
            }
            else
            {
                Presupuesto_Negocio.P_Anio_Presupuesto = 0;
            }

            Dt_Presupuestos = Presupuesto_Negocio.Consulta_Presupuesto();
            Grid_Presupuestos.PageIndex = Page_Index;
            Grid_Presupuestos.DataSource = Dt_Presupuestos;
            Grid_Presupuestos.DataBind();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Comprobar_Numero
    ///DESCRIPCIÓN: metodo para ver si ingreso un dato numerico para la busqueda de presupuesto por año
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/10/2011 11:51:18 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    ///
    private bool Comprobar_Numero(string P_Busqueda)
    {
        bool Resultado = false;
        try
        {
            Convert.ToInt32(P_Busqueda);
            Resultado = true;
        }
        catch (Exception Ex)
        {
            return Resultado;
        }
        return Resultado;
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
    private void Cargar_Datos(DataRow Dr_Presupuestos)
    {
        try
        {
            Txt_Comentarios.Text = Dr_Presupuestos[Cat_Com_Dep_Presupuesto.Campo_Comentarios].ToString();
            Txt_Monto_Disponible.Text = Dr_Presupuestos[Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible].ToString();
            Txt_Monto_Comprometido.Text = Dr_Presupuestos[Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido].ToString();
            Txt_Monto_Presupuestal.Text = Dr_Presupuestos[Cat_Com_Dep_Presupuesto.Campo_Monto_Presupuestal].ToString();
            Txt_Anio_Presupuesto.Text = Dr_Presupuestos[Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto].ToString();
            Cmb_Dependencias.SelectedValue = Dr_Presupuestos[Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID].ToString();
            
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

                Txt_Comentarios.Text = String.Empty;
                Txt_Monto_Presupuestal.Text = String.Empty;
                Txt_Monto_Comprometido.Text = String.Empty;
                Txt_Monto_Disponible.Text = String.Empty;
                Txt_Anio_Presupuesto.Text = String.Empty;
                Cmb_Dependencias.SelectedIndex = 0;

                Txt_Comentarios.Enabled = false;
                Txt_Monto_Presupuestal.Enabled = false;
                Txt_Monto_Comprometido.Enabled = false;
                Txt_Monto_Disponible.Enabled = false;
                Txt_Anio_Presupuesto.Enabled = false;
                Cmb_Dependencias.Enabled = false;

                Grid_Presupuestos.Enabled = true;
                Grid_Presupuestos.SelectedIndex = (-1);

                Btn_Busqueda.Enabled = true;
                Btn_Eliminar.Enabled = true;
                Btn_Modificar.Enabled = true;
                Btn_Nuevo.Enabled = true;
                Btn_Salir.Enabled = true;
                Btn_Busqueda.AlternateText = "Buscar";
                Btn_Eliminar.AlternateText = "Eliminar";
                Btn_Modificar.AlternateText = "Modificar";
                Btn_Nuevo.AlternateText = "Nuevo";
                Btn_Salir.AlternateText = "Salir";

                Btn_Busqueda.ToolTip = "Consultar";
                Btn_Eliminar.ToolTip = "Eliminar";
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Salir.ToolTip = "Salir";

                Btn_Busqueda.ImageUrl = "~/paginas/imagenes/paginas/busqueda.png";
                Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                Configuracion_Acceso("Frm_Cat_Com_Dep_Presupuesto.aspx");
                break;

            case 1: //Nuevo
                Mensaje_Error();

                Txt_Comentarios.Text = String.Empty;
                Txt_Anio_Presupuesto.Text = String.Empty;
                Txt_Monto_Comprometido.Text = String.Empty;
                Txt_Monto_Disponible.Text = String.Empty;
                Txt_Monto_Presupuestal.Text = String.Empty;                                
                Cmb_Dependencias.SelectedIndex = 0;

                Txt_Comentarios.Enabled = true;
                Txt_Monto_Presupuestal.Enabled = true;
                Txt_Monto_Comprometido.Enabled = true;
                Txt_Monto_Disponible.Enabled = true;
                Txt_Anio_Presupuesto.Enabled = true;
                Cmb_Dependencias.Enabled = true;

                Btn_Eliminar.Enabled = false;
                Btn_Modificar.Enabled = false;
                Btn_Nuevo.Enabled = true;
                Btn_Salir.Enabled = true;

                Grid_Presupuestos.SelectedIndex = (-1);
                Grid_Presupuestos.Enabled = false;

                Btn_Eliminar.AlternateText = "Eliminar";
                Btn_Modificar.AlternateText = "Modificar";
                Btn_Nuevo.AlternateText = "Guardar";
                Btn_Salir.AlternateText = "Cancelar";

                Btn_Eliminar.ToolTip = "Eliminar";
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Nuevo.ToolTip = "Guardar";
                Btn_Salir.ToolTip = "Cancelar";

                Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar_deshabilitado.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar_deshabilitado.png";

                break;

            case 2: //Modificar
                Mensaje_Error();

                Txt_Comentarios.Enabled = true;
                Txt_Monto_Presupuestal.Enabled = true;
                Txt_Monto_Comprometido.Enabled = true;
                Txt_Monto_Disponible.Enabled = true;
                Txt_Anio_Presupuesto.Enabled = true;
                Cmb_Dependencias.Enabled = true;
                
                Btn_Eliminar.Enabled = false;
                Btn_Modificar.Enabled = true;
                Btn_Nuevo.Enabled = false;
                Btn_Salir.Enabled = true;

                Btn_Eliminar.AlternateText = "Eliminar";
                Btn_Modificar.AlternateText = "Guardar";
                Btn_Nuevo.AlternateText = "Nuevo";
                Btn_Salir.AlternateText = "Cancelar";

                Btn_Eliminar.ToolTip = "Eliminar";
                Btn_Modificar.ToolTip = "Guardar";
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Salir.ToolTip = "Cancelar";

                Grid_Presupuestos.Enabled = false;

                Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar_deshabilitado.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo_deshabilitado.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";

                break;

            case 3: //Buscar
                Mensaje_Error();

                Txt_Comentarios.Text = String.Empty;
                Txt_Anio_Presupuesto.Text = String.Empty;
                Txt_Monto_Comprometido.Text = String.Empty;
                Txt_Monto_Disponible.Text = String.Empty;
                Txt_Monto_Presupuestal.Text = String.Empty;
                Cmb_Dependencias.SelectedIndex = 0;


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
            if (Txt_Anio_Presupuesto.Text.Trim() == "")
            {
                Resultado = false;
                Mensaje_Error("Favor de ingresar el Año del Presupuesto");
            }
            if (Txt_Monto_Presupuestal.Text.Trim() == "")
            {
                Resultado = false;
                Mensaje_Error("Favor de ingresar el Monto Presupuestal del Presupuesto");
            }
            if (Txt_Monto_Disponible.Text.Trim() == "")
            {
                Resultado = false;
                Mensaje_Error("Favor de ingresar el Monto Disponible del Presupuesto");
            }
            if (Txt_Monto_Comprometido.Text.Trim() == "")
            {
                Resultado = false;
                Mensaje_Error("Favor de ingresar el Monto Comprometido del Presupuesto");
            }
            if (Cmb_Dependencias.SelectedIndex <= 0)
            {
                Resultado = false;
                Mensaje_Error("Favor de especificar la Dependencia relacionada con el Presupuesto");
            }
            
            if (!Txt_Comentarios.Text.Trim().Equals(""))
            {
                if (Txt_Comentarios.Text.Trim().Length >= 250)
                {
                    Txt_Comentarios.Text = Txt_Comentarios.Text.Trim().Substring(0, 250);
                }
            }

        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
        return Resultado;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Alta_Modelo
    ///DESCRIPCIÓN: Realiza el alta de un nuevo registro de un modelo
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 07/02/2011 19:36:45
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Alta_Presupuesto()
    {
        try
        {
            if (Validar_Datos())
            {
                Cls_Cat_Com_Presupuesto_Dependencias_Negocio Presupuestos_Negocio = new Cls_Cat_Com_Presupuesto_Dependencias_Negocio();
                if (!Txt_Comentarios.Text.Equals(""))
                {
                    Presupuestos_Negocio.P_Comentarios = Txt_Comentarios.Text.Trim();
                }
                Presupuestos_Negocio.P_Dependencia_ID = Cmb_Dependencias.SelectedValue;
                Presupuestos_Negocio.P_Anio_Presupuesto = Convert.ToInt32( Txt_Anio_Presupuesto.Text.Trim() );
                Presupuestos_Negocio.P_Monto_Comprometido = Convert.ToDouble( Txt_Monto_Comprometido.Text.Trim() );
                Presupuestos_Negocio.P_Monto_Disponible = Convert.ToDouble( Txt_Monto_Disponible.Text.Trim() );
                Presupuestos_Negocio.P_Monto_Presupuestal = Convert.ToDouble(Txt_Monto_Presupuestal.Text.Trim());

                Presupuestos_Negocio.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
                Presupuestos_Negocio.Alta_Presupuesto();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Presupuestos", "alert('El Alta del Presupuesto fue Exitosa');", true);
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

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Baja
    ///DESCRIPCIÓN: dar de baja un registro de la base de datos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/04/2011 11:25:30 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Baja_Presupuesto()
    {
        try
        {
            Cls_Cat_Com_Presupuesto_Dependencias_Negocio Presupuestos_Negocio = new Cls_Cat_Com_Presupuesto_Dependencias_Negocio();

            Presupuestos_Negocio.P_Presupuesto_ID = Convert.ToInt32(HttpUtility.HtmlDecode(Grid_Presupuestos.SelectedRow.Cells[1].Text.Trim()));
            Presupuestos_Negocio.P_Dependencia_ID = HttpUtility.HtmlDecode(Grid_Presupuestos.SelectedRow.Cells[2].Text.Trim());
            Presupuestos_Negocio.P_Anio_Presupuesto = Convert.ToInt32( HttpUtility.HtmlDecode(Grid_Presupuestos.SelectedRow.Cells[3].Text.Trim()) );
            Presupuestos_Negocio.P_Monto_Comprometido = Convert.ToDouble( HttpUtility.HtmlDecode(Grid_Presupuestos.SelectedRow.Cells[4].Text.Trim()) );
            Presupuestos_Negocio.P_Monto_Presupuestal = Convert.ToDouble( HttpUtility.HtmlDecode(Grid_Presupuestos.SelectedRow.Cells[5].Text.Trim()) );
            Presupuestos_Negocio.P_Monto_Disponible = Convert.ToDouble( HttpUtility.HtmlDecode(Grid_Presupuestos.SelectedRow.Cells[6].Text.Trim()) );

            Presupuestos_Negocio.Baja_Presupuesto();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Presupuestos", "alert('La Baja del Presupuesto fue Exitosa');", true);
            Estado_Botones(Const_Estado_Inicial);
            Txt_Busqueda.Text = "";
            Cargar_Grid(0);
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message, Ex);
        }

    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Modificar
    ///DESCRIPCIÓN: Modifica un registro y lo guarda en la base de datos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/04/2011 11:58:30 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Modificar_Presupuesto()
    {
        try
        {
            if (Validar_Datos())
            {
                Cls_Cat_Com_Presupuesto_Dependencias_Negocio Presupuestos_Negocio = new Cls_Cat_Com_Presupuesto_Dependencias_Negocio();
                if (!Txt_Comentarios.Text.Equals(""))
                {
                    Presupuestos_Negocio.P_Comentarios = Txt_Comentarios.Text.Trim();
                }
                Presupuestos_Negocio.P_Dependencia_ID = Cmb_Dependencias.SelectedValue;
                Presupuestos_Negocio.P_Anio_Presupuesto = Convert.ToInt32(Txt_Anio_Presupuesto.Text.Trim());
                Presupuestos_Negocio.P_Monto_Comprometido = Convert.ToDouble(Txt_Monto_Comprometido.Text.Trim());
                Presupuestos_Negocio.P_Monto_Disponible = Convert.ToDouble(Txt_Monto_Disponible.Text.Trim());
                Presupuestos_Negocio.P_Monto_Presupuestal = Convert.ToDouble(Txt_Monto_Presupuestal.Text.Trim());
                Presupuestos_Negocio.P_Presupuesto_ID = Convert.ToInt32(HttpUtility.HtmlDecode(Grid_Presupuestos.SelectedRow.Cells[1].Text.Trim()));
                Presupuestos_Negocio.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
                Presupuestos_Negocio.Cambio_Presupuesto();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Presupuestos", "alert('La modificación del Presupuesto fue Exitosa');", true);
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
                Alta_Presupuesto();
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
            if (Grid_Presupuestos.SelectedIndex > (-1))
            {
                if (Btn_Modificar.AlternateText.Equals("Modificar"))
                {
                    Estado_Botones(Const_Estado_Modificar);
                }
                else
                {
                    Modificar_Presupuesto();
                }
            }
            else
            {
                Mensaje_Error("Favor de seleccionar el Presupuesto a modificar");
            }

        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);

        }
    }
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Grid_Presupuestos.SelectedIndex > (-1))
            {
                Baja_Presupuesto();
            }
            else
            {
                Mensaje_Error("Favor de seleccionar el Presupuesto a eliminar");
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
    protected void Btn_Buscar_Servicio_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Estado_Botones(Const_Estado_Buscar);
            Grid_Presupuestos.SelectedIndex = (-1);
            M_Busqueda = Txt_Busqueda.Text.Trim();
            Cargar_Grid(0);

        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    #endregion

    #region Eventos Grid
    protected void Grid_Presupuestos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Presupuestos.SelectedIndex = (-1);
            Cargar_Grid(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    protected void Grid_Presupuestos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Presupuestos.SelectedIndex > (-1))
            {
                Cargar_Datos(Dt_Presupuestos.Rows[Grid_Presupuestos.SelectedIndex + (Grid_Presupuestos.PageIndex * 5)]);
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
            Botones.Add(Btn_Eliminar);
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
