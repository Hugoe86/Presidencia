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
using Presidencia.Distribuir_a_Proveedores.Negocio;
using System.Net.Mail;
using Presidencia.Constantes;
using Presidencia.Registro_Peticion.Datos;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using Presidencia.Sessiones;
using Presidencia.Orden_Compra.Negocio;

public partial class paginas_Compras_Frm_Ope_Com_Distribuir_Requisiciones_Prov : System.Web.UI.Page
{
    ///*******************************************************************************
    ///PAGE_LOAD
    ///*******************************************************************************
    #region Page_Load

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["SortDirection"] = "ASC";
            Llenar_Combo_Cotizadores();
            Configurar_Formulario("Inicio");
            Llenar_Grid_Requisiciones();
            Cls_Ope_Com_Distribuir_Requisiciones_Prov_Negocio Clase = new Cls_Ope_Com_Distribuir_Requisiciones_Prov_Negocio();
            //Verificar si su rol es jefe de dependencia, admin de modulo o admin de sistema
            DataTable Dt_Grupo_Rol = Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Rol_ID.ToString());
            if (Dt_Grupo_Rol != null)
            {
                String Grupo_Rol = Dt_Grupo_Rol.Rows[0][Apl_Cat_Roles.Campo_Grupo_Roles_ID].ToString();
                if (Grupo_Rol == "00001" || Grupo_Rol == "00002")
                {
                    Cmb_Cotizadores.Enabled = true;
                }
                else
                {
                    Cmb_Cotizadores.Enabled = false;
                }
            }            
        }
    }

    #endregion

    ///*******************************************************************************
    ///METODOS
    ///*******************************************************************************
    #region Metodos
    ///*******************************************************************************
    //NOMBRE DE LA FUNCIÓN: Llenar_Combo_Cotizadores
    //DESCRIPCIÓN:Llenar_Combo_Cotizadores
    //CREO: Gustavo Angeles Cruz
    //FECHA_CREO: 14/Oct/2011 
    //MODIFICO:
    //FECHA_MODIFICO:
    //CAUSA_MODIFICACIÓN:
    //********************************************************************************/
    private void Llenar_Combo_Cotizadores()
    {
        Cls_Ope_Com_Orden_Compra_Negocio Negocio_Compra = new Cls_Ope_Com_Orden_Compra_Negocio();
        DataTable Dt_Cotizadores = Negocio_Compra.Consultar_Cotizadores();
        if (Dt_Cotizadores != null && Dt_Cotizadores.Rows.Count > 0)
        {
            Cls_Util.Llenar_Combo_Con_DataTable_Generico
                (Cmb_Cotizadores, Dt_Cotizadores, Cat_Com_Cotizadores.Campo_Nombre_Completo, Cat_Com_Cotizadores.Campo_Empleado_ID);
            Cmb_Cotizadores.SelectedValue = Cls_Sessiones.Empleado_ID;
        }
        else
        {
            Cmb_Cotizadores.Items.Clear();
            Cmb_Cotizadores.Items.Add("COTIZADORES");
            Cmb_Cotizadores.SelectedIndex = 0;
        }
    }

   ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configurar_Formulario
    ///DESCRIPCIÓN: Metodo que configura el formulario con respecto al estado de habilitado o visible
    ///´de los componentes de la pagina
    ///PARAMETROS: 1.- String Estatus: Estatus que puede tomar el formulario con respecto a sus componentes, ya sea "Inicio" o "Nuevo"
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/JULIO/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Configurar_Formulario(String Estatus)
    {

        switch (Estatus)
        {
            case "Inicio":

                Div_Detalle_Requisicion.Visible = false;
                Div_Grid_Requisiciones.Visible = true;

                //Boton Modificar
                Btn_Nuevo.Visible = true;
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                Btn_Nuevo.Enabled = true;
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Inicio";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                //
                Grid_Requisiciones.Visible = true;
                Grid_Requisiciones.Enabled = true;
                Div_Detalle_Requisicion.Visible = false;
                Btn_Agregar_Proveedor.Enabled = false;
                Cmb_Proveedores.Enabled = false;
                Grid_Proveedores.Enabled = false;
                Btn_Imprimir.Visible = true;
                Btn_Imprimir.Enabled = true;
                Chk_Enviar_Correo.Checked = false;
                Chk_Enviar_Correo.Enabled = false;

                break;
            case "Nuevo":

                Btn_Nuevo.Visible = true;
                Btn_Nuevo.ToolTip = "Guardar";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Nuevo.Enabled = true;
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                //
                Div_Grid_Requisiciones.Visible = false;
                Div_Detalle_Requisicion.Visible = true;
                Btn_Agregar_Proveedor.Enabled = true;
                Cmb_Proveedores.Enabled = true;
                Grid_Proveedores.Enabled = true;
                Btn_Imprimir.Visible = true;
                Btn_Imprimir.Enabled = true;
                Chk_Enviar_Correo.Checked = false;
                Chk_Enviar_Correo.Enabled = true;

                break;
        }//fin del switch

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Proveedores
    ///DESCRIPCIÓN: Metodo que Consulta los proveedores dados de alta en la tabla CAT_COM_PROVEEDORES
    ///PARAMETROS: 1.- DropDownList Cmb_Combo: combo dentro de la pagina a llenar 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/JULIO/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Proveedores(DropDownList Cmb_Combo)
    {
        Cmb_Combo.Items.Clear();
        Cls_Ope_Com_Distribuir_Requisiciones_Prov_Negocio Negocios = new Cls_Ope_Com_Distribuir_Requisiciones_Prov_Negocio();
        Negocios.P_Concepto_ID = Session["Concepto_ID"].ToString().Trim();
        DataTable Data_Table = Negocios.Consultar_Proveedores();
        Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Proveedores, Data_Table);
        Cmb_Proveedores.SelectedIndex = 0;
    }


    public void Limpiar_Variables_Session()
    {
        Session["Concepto_ID"] = null;
        Session["No_Requisicion"] = null;
        Session["Dt_Producto_Servicio"] = null;
        Session["Dt_Proveedores"] = null;
        Session["Dt_Requisiciones"] = null;
        Session["Proveedor_ID"] = null;

    }




    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Enviar_Correo
    ///DESCRIPCIÓN: Envia un correo a el encargado de Almacen para notificar que una solicitud ha sido creada 
    ///             o que hay alguna que no ha sido revisada.
    ///PROPIEDADES: 
    ///             1.  cabecera.       Número de Solicitud de la cual se quieren obtener sus detalles.
    ///             2.  no_apartado.    Número de Apartado de la cual se le quiere notificar al encargado
    ///                                 del almacen.
    ///             3.  solicito.       Nombre de quien hizo la solicitud del apartado.
    ///             4.  fecha.          Fecha del Proyecto del cual se hace la solicitud de Apartado.
    ///             5.  proyecto.       Proyecto del cual se hace la solicitud de apartado.
    ///             6.  descripcion.    Descripción del Proyecto del cual se hizó la solicitud de apartado.
    ///CREO: 
    ///FECHA_CREO: 
    ///MODIFICO:    Francisco Antonio Gallardo Castañeda.
    ///FECHA_MODIFICO:  Junio 2010.
    ///CAUSA_MODIFICACIÓN:  Se adapto para que el funcionamiento del Catalogo de Solicitud de Apartado. 
    ///*******************************************************************************
    public void Enviar_Correo(String Email,String Correo,String Password,String Direccion_IP,String Texto)
    {
        try
        {
            if (Email != "" && Email != null)
            {
                Cls_Mail mail = new Cls_Mail();

                mail.P_Servidor = Direccion_IP;
                mail.P_Envia = Correo;
                mail.P_Password = Password;
                mail.P_Recibe = Email;
                mail.P_Subject = "Municipio de Irapuato ( Petición de Cotización )";
                mail.P_Texto = Texto;
                mail.P_Adjunto = null;//Hacer_Pdf();
                mail.Enviar_Correo();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    #endregion


    ///*******************************************************************************
    ///GRID
    ///*******************************************************************************
    #region Grid

    #region Grid_Requisiciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Requisiciones_SelectedIndexChanged
    ///DESCRIPCIÓN: Evento del Grid_Requisiciones al seleccionar
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/JULIO/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Requisiciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
        //creamos la instancia de la clase de negocios
        Cls_Ope_Com_Distribuir_Requisiciones_Prov_Negocio Clase_Negocio = new Cls_Ope_Com_Distribuir_Requisiciones_Prov_Negocio();
        
        //Consultamos los datos del producto seleccionado
        GridViewRow Row = Grid_Requisiciones.SelectedRow;
        Clase_Negocio.P_No_Requisicion = Grid_Requisiciones.SelectedDataKey["No_Requisicion"].ToString();
        Session["No_Requisicion"] = Clase_Negocio.P_No_Requisicion;
        //Consultamos los detalles del producto seleccionado 
        DataTable Dt_Detalle_Requisicion = Clase_Negocio.Consultar_Detalle_Requisicion();
        Dt_Detalle_Requisicion.Columns.Add("Elaboro", typeof(System.String));
        Dt_Detalle_Requisicion.AcceptChanges();
        Dt_Detalle_Requisicion.Rows[0]["Elaboro"] = Cls_Sessiones.Nombre_Empleado;
        Session["Dt_Detalle_Requisicion"] = Dt_Detalle_Requisicion;
        //Mostramos el div de detalle y el grid de Requisiciones
        Div_Grid_Requisiciones.Visible = false;
        Div_Detalle_Requisicion.Visible = true;
       
        //llenamos la informacion del detalle de la requisicion seleccionada
        Txt_Dependencia.Text = Dt_Detalle_Requisicion.Rows[0]["DEPENDENCIA"].ToString().Trim();
        Txt_Folio.Text = Dt_Detalle_Requisicion.Rows[0]["FOLIO"].ToString().Trim();
        Txt_Concepto.Text = Dt_Detalle_Requisicion.Rows[0]["CONCEPTO"].ToString().Trim();
        Txt_Fecha_Generacion.Text = Dt_Detalle_Requisicion.Rows[0]["FECHA_GENERACION"].ToString().Trim();
        Txt_Tipo.Text = Dt_Detalle_Requisicion.Rows[0]["TIPO"].ToString().Trim();
        Txt_Tipo_Articulo.Text = Dt_Detalle_Requisicion.Rows[0]["TIPO_ARTICULO"].ToString().Trim();
        Txt_Estatus.Text = Dt_Detalle_Requisicion.Rows[0]["ESTATUS"].ToString().Trim();
        Txt_Justificacion.Text = Dt_Detalle_Requisicion.Rows[0]["JUSTIFICACION_COMPRA"].ToString().Trim();
        Txt_Especificacion.Text = Dt_Detalle_Requisicion.Rows[0]["ESPECIFICACION_PROD_SERV"].ToString().Trim();
        Txt_Subtotal.Text = Dt_Detalle_Requisicion.Rows[0]["SUBTOTAL"].ToString().Trim();
        Txt_IEPS.Text = Dt_Detalle_Requisicion.Rows[0]["IEPS"].ToString().Trim();
        Txt_IVA.Text = Dt_Detalle_Requisicion.Rows[0]["IVA"].ToString().Trim();
        Txt_Total.Text = Dt_Detalle_Requisicion.Rows[0]["TOTAL"].ToString().Trim();

        Txt_Compra_Especial.Text = Dt_Detalle_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Especial_Ramo_33].ToString().Trim();
        
        Session["Concepto_ID"] = Dt_Detalle_Requisicion.Rows[0]["CONCEPTO_ID"].ToString().Trim();
        
        //VALIDAMOS EL CAMPO DE VERIFICAR CARACTERISTICAS, GARANTIA Y POLIZAS
        if (Dt_Detalle_Requisicion.Rows[0]["VERIFICACION_ENTREGA"].ToString().Trim() == "NO" || Dt_Detalle_Requisicion.Rows[0]["VERIFICACION_ENTREGA"].ToString().Trim() == String.Empty)
        {
            Chk_Verificacion.Checked = false;
        }
        if (Dt_Detalle_Requisicion.Rows[0]["VERIFICACION_ENTREGA"].ToString().Trim() == "SI")
        {
            Chk_Verificacion.Checked = true;
        }

        //CONSULTAMOS LOS PRODUCTOS DE LA REQUISICION SELECCIONADA
        Clase_Negocio.P_No_Requisicion = Session["No_Requisicion"].ToString().Trim();
        Clase_Negocio.P_Tipo_Articulo = Txt_Tipo_Articulo.Text.Trim();
        DataTable Dt_Producto_Servicio = Clase_Negocio.Consultar_Productos_Servicios();

        if (Dt_Producto_Servicio.Rows.Count != 0)
        {
            Session["Dt_Producto_Servicio"] = Dt_Producto_Servicio;
            Grid_Productos.DataSource = Dt_Producto_Servicio;
            Grid_Productos.DataBind();
        }
        else
        {
            Grid_Productos.DataSource = new DataTable();
            Grid_Productos.DataBind();
        }
        //Llenamos el combo de Proveedores
        
        Llenar_Combo_Proveedores(Cmb_Proveedores);

        //CONSULTAMOS LOS PROVEEDORES A LOS CUALES SE LES ENVIO LA SOLICITUD DE COTIZACION
        Clase_Negocio.P_Concepto_ID = Dt_Detalle_Requisicion.Rows[0]["CONCEPTO_ID"].ToString().Trim();
        DataTable Dt_Proveedores = Clase_Negocio.Consultar_Proveedores_Asignados();
        if (Dt_Proveedores.Rows.Count != 0)
        {
            //llenamos el grid de proveedores
            Grid_Proveedores.DataSource = Dt_Proveedores;
            Grid_Proveedores.DataBind();
            Session["Dt_Proveedores"] = Dt_Proveedores;


        }
        else
        {
            Grid_Proveedores.DataSource = new DataTable();
            Grid_Proveedores.DataBind();
            Session["Dt_Proveedores"] = null;

        }

        //Llenamos el Grid de los Comentarios 
        DataTable Dt_Comentarios = Clase_Negocio.Consultar_Comentarios();
        if(Dt_Comentarios.Rows.Count != 0)
        {
            Grid_Comentarios.DataSource = Dt_Comentarios;
            Grid_Comentarios.DataBind();
        }
        else
        {
            Grid_Comentarios.EmptyDataText = "No se han encontrado registros.";
            //Lbl_Mensaje_Error.Text = "+ No se encontraron datos <br />";
            Grid_Comentarios.DataSource = new DataTable();
            Grid_Comentarios.DataBind();
        }

        Btn_Salir.ToolTip = "Listado";        
        Clase_Negocio.Marcar_Leida_Por_Cotizador();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:Grid_Requisiciones_Sorting
    ///DESCRIPCIÓN: Evento para ordenar por columna seleccionada en el Grid_Requisiciones
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/JULIO/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Requisiciones_Sorting(object sender, GridViewSortEventArgs e)
    {

        DataTable Dt_Requisiciones = (DataTable)Session["Dt_Requisiciones"];

        if (Dt_Requisiciones != null)
        {
            DataView Dv_Requisiciones = new DataView(Dt_Requisiciones);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Requisiciones.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Requisiciones.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Requisiciones.DataSource = Dv_Requisiciones;
            Grid_Requisiciones.DataBind();
            //Guardamos el cambio dentro de la variable de session de Dt_Requisiciones
            Session["Dt_Requisiciones"] = (DataTable)Dv_Requisiciones.Table;
            Dt_Requisiciones = (DataTable)Session["Dt_Requisiciones"];

        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:Llenar_Grid_Requisiciones
    ///DESCRIPCIÓN: Metodo que permite llenar el Grid_Requisiciones
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/JULIO/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Grid_Requisiciones()
    {
        Cls_Ope_Com_Distribuir_Requisiciones_Prov_Negocio Clase_Negocio = new Cls_Ope_Com_Distribuir_Requisiciones_Prov_Negocio();
        //Concultamos las requisiciones
        Clase_Negocio.P_Cotizador_ID = Cmb_Cotizadores.SelectedValue;
        DataTable Dt_Requisiciones = Clase_Negocio.Consultar_Requisiciones();
        if (Dt_Requisiciones.Rows.Count != 0)
        {
            Session["Dt_Requisiciones"] = Dt_Requisiciones;
            Grid_Requisiciones.DataSource = Dt_Requisiciones;
            Grid_Requisiciones.DataBind();
        }
        else
        {
            Session["Dt_Requisiciones"] = null;
            Grid_Requisiciones.DataSource = new DataTable();
            Grid_Requisiciones.DataBind();
        }

    }

    protected void Grid_Requisiciones_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            String Folio = e.Row.Cells[2].Text.Trim();
            DataRow[] Renglon = ((DataTable)Session["Dt_Requisiciones"]).Select("FOLIO = '" + Folio + "'");
            if (Renglon[0]["LEIDO"].ToString().Trim() != "SI")
            {
                e.Row.Cells[2].ForeColor = System.Drawing.Color.Black;
                e.Row.Cells[2].Font.Bold = true;
                e.Row.Cells[3].ForeColor = System.Drawing.Color.Black;
                
                e.Row.Cells[3].Font.Bold = true;
                e.Row.Cells[4].ForeColor = System.Drawing.Color.Black;
                e.Row.Cells[4].Font.Bold = true;
                e.Row.Cells[5].ForeColor = System.Drawing.Color.Black;
                e.Row.Cells[5].Font.Bold = true;
                e.Row.Cells[6].ForeColor = System.Drawing.Color.Black;
                e.Row.Cells[6].Font.Bold = true;
                e.Row.Cells[7].ForeColor = System.Drawing.Color.Black;
                e.Row.Cells[7].Font.Bold = true;
            }
            if (Renglon.Length > 0)
            {
                
                ImageButton Boton = (ImageButton)e.Row.FindControl("Btn_Alerta");
                String Estatus = Renglon[0]["ESTATUS"].ToString().Trim();
               
                if (Renglon[0]["ALERTA"].ToString().Trim() == "ROJA2")
                {
                    Boton.ImageUrl = "../imagenes/gridview/circle_red.png";
                    Boton.Visible = true;
                    Boton.ToolTip = "Req. Por Cancelacion Parcial de OC";
                    
                }

                else if (Renglon[0]["ALERTA"].ToString().Trim() == "AMARILLO2")
                {
                    Boton.ImageUrl = "../imagenes/gridview/circle_yellow.png";
                    Boton.Visible = true;
                    Boton.ToolTip = "Req. Cotizada-Rechazada";
                    
                }
                else
                {
                    Boton.Visible = false;
                }

            }
            //}
        }
    }
    #endregion

    #region Grid_Productos
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:Grid_Productos_Sorting
    ///DESCRIPCIÓN: Evento para ordenar por columna seleccionada en el Grid_Productos
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/JULIO/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Productos_Sorting(object sender, GridViewSortEventArgs e)
    {

        DataTable Dt_Productos = (DataTable)Session["Dt_Producto_Servicio"];

        if (Dt_Productos != null)
        {
            DataView Dv_Productos = new DataView(Dt_Productos);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Productos.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Productos.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Productos.DataSource = Dv_Productos;
            Grid_Productos.DataBind();
        }

    }
    #endregion 

    #region Proveedores
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:Grid_Proveedores_Sorting
    ///DESCRIPCIÓN: Evento para ordenar por columna seleccionada en el Grid_Proveedores
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/JULIO/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Proveedores_Sorting(object sender, GridViewSortEventArgs e)
    {

        DataTable Dt_Proveedores = (DataTable)Session["Dt_Proveedores"];

        if (Dt_Proveedores != null)
        {
            DataView Dv_Proveedores = new DataView(Dt_Proveedores);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Proveedores.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Proveedores.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Proveedores.DataSource = Dv_Proveedores;
            Grid_Proveedores.DataBind();
            //Guardamos el cambio dentro de la variable de session de Dt_Requisiciones
            Session["Dt_Proveedores"] = (DataTable)Dv_Proveedores.Table;
            Dt_Proveedores = (DataTable)Session["Dt_Proveedores"];

        }

    }

    protected void Grid_Proveedores_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataRow[] Renglones;
        DataRow Renglon;
        //Obtenemos el Id del producto seleccionado
        GridViewRow selectedRow = Grid_Proveedores.Rows[Grid_Proveedores.SelectedIndex];
        Session["Proveedor_ID"] = Grid_Proveedores.SelectedDataKey["Proveedor_ID"].ToString();
        Renglones = ((DataTable)Session["Dt_Proveedores"]).Select("PROVEEDOR_ID='" + Session["Proveedor_ID"].ToString() + "'");

        if (Renglones.Length > 0)
        {
            Renglon = Renglones[0];
            DataTable Tabla = (DataTable)Session["Dt_Proveedores"];
            Tabla.Rows.Remove(Renglon);
            //Asignamos el nuevo valor al datatable de Session
            Session["Dt_Proveedores"] = Tabla;
            Grid_Proveedores.SelectedIndex = (-1);
            Grid_Proveedores.DataSource = Tabla;
            Grid_Proveedores.DataBind();
            
        }

    }
    #endregion
    #endregion

    ///*******************************************************************************
    ///EVENTOS
    ///*******************************************************************************
    #region Eventos
    protected void Cmb_Cotizadores_SelectedIndexChanged(object sender, EventArgs e)
    {
        Llenar_Grid_Requisiciones();
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Evento del Boton Salir
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/JULIO/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        switch (Btn_Salir.ToolTip)
        {
            case "Inicio":
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                //LIMPIAMOS VARIABLES DE SESSION
                Session["Dt_Requisiciones"] = null;

                Session["No_Requisicion"] = null;
                Limpiar_Variables_Session();
                
                break;
            case "Cancelar":
                Configurar_Formulario("Inicio");
                Llenar_Grid_Requisiciones();
                Limpiar_Variables_Session();
                break;
            case "Listado":
                Configurar_Formulario("Inicio");
                Llenar_Grid_Requisiciones();
                Limpiar_Variables_Session();
                break;
        }
    }

    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        switch (Btn_Nuevo.ToolTip)
        {
            case "Nuevo":
                Configurar_Formulario("Nuevo");
                break;
            case "Guardar":
                // Validamos que agregara Proveedores
                if (Grid_Proveedores.Rows.Count == 0)
                {
                    Lbl_Mensaje_Error.Text = "Es necesario agregar los provedores";
                    Div_Contenedor_Msj_Error.Visible = true;
                }

                if (Div_Contenedor_Msj_Error.Visible == false)

                {
                    Cls_Ope_Com_Distribuir_Requisiciones_Prov_Negocio Clase_Negocio = new Cls_Ope_Com_Distribuir_Requisiciones_Prov_Negocio();
                    Clase_Negocio.P_No_Requisicion = Session["No_Requisicion"].ToString().Trim();
                    Clase_Negocio.P_Tipo_Articulo = Txt_Tipo_Articulo.Text.Trim();
                    Clase_Negocio.P_Dt_Proveedores = (DataTable)Session["Dt_Proveedores"];

                    //PRIMERO ELIMINAMOS LOS PROVEEDORES QUE HABIAN SIDO AGREGADOS
                    Clase_Negocio.Eliminar_Proveedores();
                    //DAMOS DE ALTA LOS PROVEEDORES
                    bool Operacion_Realizada = Clase_Negocio.Alta_Proveedores_Asignados();
                   

                    if (Operacion_Realizada)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Distribuir a Provedores", "alert('Se enviaron las requisiciones a los proveedores ');", true);
                        //Enviamos los correos al proveedor
                        if (Chk_Enviar_Correo.Checked == true)
                        {
                           //CONSULTAMOS EL CORREO, EL PASSWORD Y LA DIRECCION IP DE LA COTIZADORA
                            DataTable Dt_Datos_Cotizadora = Clase_Negocio.Consultar_Datos_Cotizador();
                            String Correo = Dt_Datos_Cotizadora.Rows[0][Cat_Com_Cotizadores.Campo_Correo].ToString().Trim();
                            String Password = Dt_Datos_Cotizadora.Rows[0][Cat_Com_Cotizadores.Campo_Password_Correo].ToString().Trim();
                            String Direccion_IP = Dt_Datos_Cotizadora.Rows[0][Cat_Com_Cotizadores.Campo_IP_Correo_Saliente].ToString().Trim();
                            DataTable Dt_Invitacion = Clase_Negocio.Consultar_Parametro_Invitacion();
                            String Texto_Invitacion = Dt_Invitacion.Rows[0][Cat_Com_Parametros.Campo_Invitacion_Proveedores].ToString().Trim();

                            if (Clase_Negocio.P_Dt_Proveedores.Rows.Count != 0)
                            {

                                for (int i = 0; i < Clase_Negocio.P_Dt_Proveedores.Rows.Count; i++)
                                {
                                    //Realizamos un for para enviar el correo
                                    try
                                    {
                                        if (Clase_Negocio.P_Dt_Proveedores.Rows[i][Cat_Com_Proveedores.Campo_Correo_Electronico].ToString().Trim() != String.Empty)
                                            Enviar_Correo(Clase_Negocio.P_Dt_Proveedores.Rows[i][Cat_Com_Proveedores.Campo_Correo_Electronico].ToString().Trim(), Correo,Password,Direccion_IP, Texto_Invitacion );
                                    }
                                    catch
                                    {
                                        Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + " El correo no pudo ser enviado a " + Clase_Negocio.P_Dt_Proveedores.Rows[i][Cat_Com_Proveedores.Campo_Correo_Electronico].ToString().Trim() + " </br>";
                                    }
                                }//Fin del For
                            }//fin del IF
                        }//fin del if Checked
                    }//FIN DEL IF
                    if (!Operacion_Realizada)
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Distribuir a Proveedores", "alert('No se pudieron enviar las requisiciones a los proveedores');", true);
                    //Configurar_Formulario("Inicio");
                    //Llenar_Grid_Requisiciones();
                    //Deshabilitamos el grid y combo
                    Grid_Productos.Enabled = false;
                    Cmb_Proveedores.Enabled = false;
                    Chk_Enviar_Correo.Enabled = false;
                    Grid_Proveedores.Enabled = false;
                    Btn_Imprimir.Visible = true;
                    Btn_Imprimir.Enabled = true;
                    //Boton Modificar
                    Btn_Nuevo.Visible = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Nuevo.Enabled = false;
                    //Boton Salir
                    Btn_Salir.Visible = true;
                    Btn_Salir.ToolTip = "Listado";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    
                    

                }


                break;
        }//fin del switch
    }

    protected void Btn_Agregar_Proveedor_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        if (Cmb_Proveedores.SelectedIndex == 0)
        {
            Lbl_Mensaje_Error.Text = "Es necesario seleccionar un Proveedor";
            Div_Contenedor_Msj_Error.Visible = true;

        }
        //En caso de pasar las validaciones
        if (Div_Contenedor_Msj_Error.Visible == false)
        {

            if (Session["Dt_Proveedores"] != null)
            {
                Agregar_Proveedores();
            }//fin if
            else
            {
                //Creamos la session por primera ves
                DataTable Dt_Proveedores = new DataTable();
                Dt_Proveedores.Columns.Add("Proveedor_ID", typeof(System.String));
                Dt_Proveedores.Columns.Add("Nombre", typeof(System.String));
                Dt_Proveedores.Columns.Add("Compania", typeof(System.String));
                Dt_Proveedores.Columns.Add("Telefonos", typeof(System.String));
                Dt_Proveedores.Columns.Add("E_MAIL", typeof(System.String));
                Session["Dt_Proveedores"] = Dt_Proveedores;
                //Llenamos el grid
                Grid_Proveedores.DataSource = (DataTable)Session["Dt_Proveedores"];
                Grid_Proveedores.DataBind();
                //Mandamos llamar el metodo para agregar los proveedores
                Agregar_Proveedores();
            }
        }


    }

    public void Agregar_Proveedores()
    {
        String Id = Cmb_Proveedores.SelectedValue;
        DataRow[] Filas;
        DataTable Dt = (DataTable)Session["Dt_Proveedores"];
        Filas = Dt.Select("Proveedor_ID='" + Id + "'");
        if (Filas.Length > 0)
        {
            //Si se encontro algun coincidencia entre el grupo a agregar con alguno agregado anteriormente, se avisa
            //al usuario que elemento ha agregar ya existe en la tabla de grupos.
            Lbl_Mensaje_Error.Text += "+ No se puede agregar el Proveedor " + Id + " ya que este ya se ha agregado<br/>";
            Div_Contenedor_Msj_Error.Visible = true;
        }
        else
        {
            //Creamos el objeto de la clase de negocios
            Cls_Ope_Com_Distribuir_Requisiciones_Prov_Negocio Clase_Negocio = new Cls_Ope_Com_Distribuir_Requisiciones_Prov_Negocio();
            //cONSULTAMOS EL PROVEEDOR ASIGNADO
            Clase_Negocio.P_Proveedor_ID = Cmb_Proveedores.SelectedValue;
            DataTable Dt_Datos_Proveedor = Clase_Negocio.Consultar_Proveedores_Asignados();
            if (!(Dt_Datos_Proveedor == null))
            {
                if (Dt_Datos_Proveedor.Rows.Count > 0)
                {
                    DataRow Fila_Nueva = Dt.NewRow();
                    //Asignamos los valores a la fila
                    Fila_Nueva["Proveedor_ID"] = Dt_Datos_Proveedor.Rows[0]["Proveedor_ID"].ToString();
                    Fila_Nueva["Nombre"] = Dt_Datos_Proveedor.Rows[0]["Nombre"].ToString();
                    Fila_Nueva["Compania"] = Dt_Datos_Proveedor.Rows[0]["Compania"].ToString();
                    Fila_Nueva["Telefonos"] = Dt_Datos_Proveedor.Rows[0]["Telefonos"].ToString();
                    Fila_Nueva["E_MAIL"] = Dt_Datos_Proveedor.Rows[0]["E_MAIL"].ToString();
                    Dt.Rows.Add(Fila_Nueva);
                    Dt.AcceptChanges();
                    Session["Dt_Proveedores"] = Dt;
                    Grid_Proveedores.DataSource = Dt;
                    Grid_Proveedores.DataBind();
                    Grid_Proveedores.Visible = true;
                }
            }

        }//fin del else


    }


    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        if (Txt_Dependencia.Text.Trim() == String.Empty)
        {
            Lbl_Mensaje_Error.Text = "Es necesario selecionar una Requisicion";
            Div_Contenedor_Msj_Error.Visible = true;
        }

        if (Div_Contenedor_Msj_Error.Visible == false)
        {
            //Agregamos los dataset a los un dataset 
            DataSet Ds_Imprimir_Requisicion = new DataSet();
            DataTable Dt_Detalle_Requisicion= (DataTable)Session["Dt_Detalle_Requisicion"];
            DataTable Dt_Producto_Servicio= (DataTable)Session["Dt_Producto_Servicio"];
            
            Dt_Producto_Servicio.AcceptChanges();
            Dt_Detalle_Requisicion.AcceptChanges();

            Ds_Imprimir_Requisicion.Tables.Add(Dt_Detalle_Requisicion.Copy());
            Ds_Imprimir_Requisicion.Tables[0].TableName = "Dt_Detalle_Requisicion";
            Ds_Imprimir_Requisicion.AcceptChanges();
            Ds_Imprimir_Requisicion.Tables.Add(Dt_Producto_Servicio.Copy());
            Ds_Imprimir_Requisicion.Tables[1].TableName = "Dt_Producto_Servicio";
            Ds_Imprimir_Requisicion.AcceptChanges();
            Ds_Ope_Com_Requisicion_a_Cotizar Ds_Obj = new Ds_Ope_Com_Requisicion_a_Cotizar();


            Generar_Reporte(Ds_Imprimir_Requisicion, Ds_Obj, "Rpt_Ope_Com_Requisicion_a_Cotizar.rpt", "Rpt_Ope_Com_Requisicion_a_Cotizar.pdf");

        }
    }

    #endregion


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN: caraga el data set fisoco con el cual se genera el Reporte especificado
    ///PARAMETROS:  1.-Data_Set_Consulta_DB.- Contiene la informacion de la consulta a la base de datos
    ///             2.-Ds_Reporte, Objeto que contiene la instancia del Data set fisico del Reporte a generar
    ///             3.-Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/Mayo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte(DataSet Data_Set_Consulta_DB, DataSet Ds_Reporte, string Nombre_Reporte, string Nombre_PDF)
    {

        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Compras/" + Nombre_Reporte);
        Reporte.Load(File_Path);
        Ds_Reporte = Data_Set_Consulta_DB;
        Reporte.SetDataSource(Ds_Reporte);
        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Nombre_PDF);
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
        Reporte.Export(Export_Options);
        String Ruta = "../../Reporte/" + Nombre_PDF;
        Mostrar_Reporte(Nombre_PDF, "PDF");
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
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





    protected void Btn_Cancelar_Click(object sender, ImageClickEventArgs e)
    {
        ScriptManager.RegisterStartupScript(
            this, this.GetType(), "Requisiciones", "alert('Operación deshabilitada temporalmente');", true);
    }

    //protected void Grid_Requisiciones_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    String Folio = e.Row.Cells[1].Text.Trim();

        //    DataRow[] Renglon = ((DataTable)Session[P_Dt_Requisiciones]).Select("FOLIO = '" + Folio + "'");
        //    if (Renglon.Length > 0)
        //    {
        //        e.Row.Cells[2].Style.Add("FONT color", "red");

        //        ImageButton Boton = (ImageButton)e.Row.FindControl("Btn_Alerta");
        //        String Estatus = Renglon[0]["ESTATUS"].ToString().Trim();

        //        switch (Estatus)
        //        {
        //            case EST_EN_CONSTRUCCION:
        //                if (Renglon[0]["ALERTA"].ToString().Trim() == "AMARILLO")
        //                {
        //                    Boton.ImageUrl = "../imagenes/gridview/circle_yellow.png";
        //                    Boton.Visible = true;
        //                }
        //                break;
        //            case EST_REVISAR:
        //                Boton.ImageUrl = "../imagenes/gridview/circle_yellow.png";
        //                Boton.Visible = true;
        //                break;
        //            case EST_ALMACEN:
        //                Boton.ImageUrl = "../imagenes/gridview/almacen.png";
        //                Boton.ToolTip = "Pasar por su producto al Almacén";
        //                Boton.Visible = true;
        //                break;
        //            case EST_SURTIDA:
        //                Boton.ImageUrl = "../imagenes/gridview/almacen.png";
        //                Boton.ToolTip = "Pasar por su producto al Almacén";
        //                Boton.Visible = true;
            
        //                break;
        //            default:
        //                Boton.Visible = false;
        //                break;
        //        }

             
        //        char[] chars = { ' ' };
 
        //        String Fecha = Renglon[0]["FECHA_CREO"].ToString().Substring(0, 10);
        //        char[] diagonal = { '/' };


        //    }
        //}
    //}
}
